using System;
using DAL;
using BE;
using Servicioss;

namespace BL
{
    public class AuthManager
    {
        private int PoliticaIntentosInicial = 5;
        private int BloqueoInicialMin = 15;
        private int PasoEscalonIntentos = 3;
        private int MaxBloqueoHoras = 24;

        private const string PatenteBitacora = "AUDITORIA_BITACORA";
        private const string PatenteValidarDv = "VALIDAR_DIGITO_VERIFICADOR";

        private readonly CryptoService cryptoService;
        private readonly BitacoraBL bitacora;
        private readonly AutorizacionService autorizacionService;
        private readonly SesionActual sesionActual;

        private Usuario usuarioActual;
        private readonly UsuarioDAL usuarioDal;
        private readonly IdiomaBL _idiomaBL = new IdiomaBL();

        public AuthManager(CryptoService cryptoService, BitacoraBL servicioBitacora, AutorizacionService autorizacionService)
        {
            this.cryptoService = cryptoService;
            bitacora = servicioBitacora;
            this.autorizacionService = autorizacionService;
            sesionActual = SesionActual.Instance;
            usuarioDal = new UsuarioDAL();
        }

        public LoginResultado IntentarLogin(string usuario, string password)
        {
            LoginResultado resultado = new LoginResultado
            {
                Status = LoginStatus.CredencialesInvalidas,
                Mensaje = "Usuario o contraseña inválidos."
            };

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                resultado.Status = LoginStatus.ParametrosInvalidos;
                resultado.Mensaje = "Debe ingresar usuario y contraseña.";
                return resultado;
            }

            Usuario usuarioEnBase = usuarioDal.ObtenerPorNombre(usuario);
            if (usuarioEnBase == null)
            {
                RegistrarLoginFail(usuario, "LOGIN_FAIL");
                resultado.Status = LoginStatus.CredencialesInvalidas;
                return resultado;
            }

            if (!usuarioEnBase.EstaActivo)
            {
                RegistrarLoginFail(usuarioEnBase.NombreUsuario, "LOGIN_INACTIVO");
                resultado.Status = LoginStatus.CredencialesInvalidas;
                resultado.Mensaje = "El usuario se encuentra desactivado. Contacte a un administrador.";
                return resultado;
            }

            if (EsUsuarioBloqueado(usuarioEnBase))
            {
                resultado.Status = LoginStatus.UsuarioBloqueado;
                resultado.DesbloqueoUtc = usuarioEnBase.BloqueadoHastaUtc;
                TimeSpan tiempoRestante = usuarioEnBase.BloqueadoHastaUtc - DateTime.UtcNow;
                string mensajeTiempo = FormatearTiempo(tiempoRestante);
                resultado.Mensaje = "Usuario bloqueado. Falta " + mensajeTiempo + " (hasta " + usuarioEnBase.BloqueadoHastaUtc.ToLocalTime().ToString("HH:mm:ss") + ".)";
                RegistrarLoginFail(usuarioEnBase.NombreUsuario, "LOGIN_BLOQUEADO");
                return resultado;
            }

            bool passwordCorrecto = cryptoService.VerificarPassword(password, usuarioEnBase.PasswordSalt, usuarioEnBase.PasswordHash);
            if (!passwordCorrecto)
            {
                usuarioEnBase.FailedAttempts += 1;
                int minutosBloqueo = 0;

                if (usuarioEnBase.FailedAttempts >= PoliticaIntentosInicial)
                {
                    DateTime hasta = DateTime.UtcNow.AddMinutes(BloqueoInicialMin);
                    DateTime limiteMaximo = DateTime.UtcNow.AddHours(MaxBloqueoHoras);
                    if (hasta > limiteMaximo)
                    {
                        hasta = limiteMaximo;
                    }

                    usuarioEnBase.BloqueadoHastaUtc = hasta;
                    minutosBloqueo = (int)Math.Ceiling((usuarioEnBase.BloqueadoHastaUtc - DateTime.UtcNow).TotalMinutes);
                }

                usuarioDal.Guardar(usuarioEnBase);

                if (minutosBloqueo > 0)
                {
                    resultado.Status = LoginStatus.UsuarioBloqueado;
                    resultado.MinutosBloqueoAplicados = minutosBloqueo;
                    resultado.DesbloqueoUtc = usuarioEnBase.BloqueadoHastaUtc;
                    TimeSpan diferenciaTiempo = usuarioEnBase.BloqueadoHastaUtc - DateTime.UtcNow;
                    resultado.Mensaje = "Credenciales inválidas. Usuario bloqueado por " + FormatearTiempoBloqueo(diferenciaTiempo) + " (hasta " + usuarioEnBase.BloqueadoHastaUtc.ToLocalTime().ToString("HH:mm:ss") + ".)";
                    RegistrarLoginFail(usuarioEnBase.NombreUsuario, "LOGIN_BLOQUEADO");
                    return resultado;
                }

                int intentosFaltantesParaUmbral = CalcularIntentosRestantesParaProximoUmbral(usuarioEnBase.FailedAttempts);
                resultado.IntentosFallidos = usuarioEnBase.FailedAttempts;
                resultado.FaltanParaBloqueo = Math.Max(0, intentosFaltantesParaUmbral);
                resultado.UmbralBloqueoActual = usuarioEnBase.FailedAttempts + resultado.FaltanParaBloqueo;

                if (intentosFaltantesParaUmbral > 0)
                {
                    resultado.Mensaje = "Credenciales inválidas. Intentos: " + usuarioEnBase.FailedAttempts + ". Bloqueo al llegar a " + resultado.UmbralBloqueoActual + " (faltan " + intentosFaltantesParaUmbral + ").";
                }
                else
                {
                    resultado.Mensaje = "Credenciales inválidas.";
                }

                RegistrarLoginFail(usuarioEnBase.NombreUsuario, "LOGIN_FAIL");
                return resultado;
            }

            string detalleIntegridad;
            bool integridadOk = VerificarIntegridad(out detalleIntegridad);

            if (!integridadOk)
            {
                bool tienePermisoDv = false;
                try
                {
                    if (autorizacionService != null)
                    {
                        autorizacionService.CargarPermisosEnUsuario(usuarioEnBase);
                        tienePermisoDv = autorizacionService.TienePermiso(usuarioEnBase, PatenteValidarDv);
                    }
                    else
                    {
                        tienePermisoDv = usuarioEnBase.TienePermiso(PatenteValidarDv);
                    }
                }
                catch
                {
                }

                if (!tienePermisoDv)
                {
                    tienePermisoDv = usuarioDal.UsuarioTienePermisoDescripcion(usuarioEnBase.Id, PatenteValidarDv);
                }

                if (!tienePermisoDv)
                {
                    resultado.Status = LoginStatus.CredencialesInvalidas;
                    resultado.Mensaje = "Se detectaron errores de integridad. Debe iniciar sesión un administrador autorizado (VALIDAR_DIGITO_VERIFICADOR).";
                    RegistrarLoginFail(usuarioEnBase.NombreUsuario, "LOGIN_SIN_PERMISO_DV");
                    return resultado;
                }

                resultado.Status = LoginStatus.CredencialesInvalidas;
                resultado.RequiereAprobacionDv = true;
                resultado.DetalleIntegridad = detalleIntegridad;
                resultado.Mensaje = "Se detectaron errores de integridad en la base de datos (Tarea). ¿Desea reparar ahora?";
                return resultado;
            }

            usuarioEnBase.FailedAttempts = 0;
            usuarioEnBase.BloqueadoHastaUtc = DateTime.MinValue;
            usuarioDal.Guardar(usuarioEnBase);

            try
            {
                if (autorizacionService != null)
                {
                    autorizacionService.CargarPermisosEnUsuario(usuarioEnBase);
                }
            }
            catch
            {
                if (bitacora != null)
                {
                    bitacora.Registrar("CargaPermisos", "Usuario", "FAIL", usuarioEnBase.NombreUsuario, "Seguridad", "");
                }
            }

            if (string.Equals(usuarioEnBase.NombreUsuario, "admin", StringComparison.OrdinalIgnoreCase))
            {
                if (!usuarioEnBase.TienePermiso(PatenteBitacora))
                {
                    Patente permisoBitacora = new Patente
                    {
                        Nombre = PatenteBitacora,
                        Descripcion = "Ver registros de bitácora"
                    };
                    usuarioEnBase.AgregarPermiso(permisoBitacora);
                }
            }

            usuarioActual = usuarioEnBase;

            
            sesionActual.IniciarPorDatos(usuarioEnBase.Id, usuarioEnBase.NombreUsuario);
            RegistrarLoginOk(usuarioEnBase);

            
            try
            {
                if (usuarioEnBase.IdiomaId.HasValue && usuarioEnBase.IdiomaId.Value > 0)
                {
                    var traduccionesDefecto = _idiomaBL.ObtenerTraducciones(usuarioEnBase.IdiomaId.Value);
                    if (traduccionesDefecto != null && traduccionesDefecto.Count > 0)
                    {
                        IdiomaService.Instancia.CambiarIdioma(traduccionesDefecto);
                    }
                }
            }
            catch
            {
            }

            resultado.Status = LoginStatus.Exito;
            resultado.Mensaje = "Autenticación correcta.";
            resultado.IntentosFallidos = 0;
            resultado.FaltanParaBloqueo = 0;
            return resultado;
        }

        public bool RegistrarUsuario(string nombreUsuario, string password)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            if (!ValidarPoliticasPassword(password))
            {
                return false;
            }

            UsuarioBL usuarioBL = new UsuarioBL();
            bool registroOk = usuarioBL.RegistrarUsuario(nombreUsuario, password);

            if (registroOk && bitacora != null)
            {
                bitacora.Registrar("RegistrarUsuario", "Usuario", "OK", nombreUsuario, "Seguridad", "");
            }

            return registroOk;
        }

        public bool RepararIntegridad(string usuarioNombre)
        {
            try
            {
                TareaBL tareaBL = new TareaBL();
                tareaBL.RepararDVH_Tarea();

                IntegridadBL integridadService = new IntegridadBL();
                integridadService.RecalcularDVV_Tarea();

                if (bitacora != null)
                {
                    string usuarioBitacora;
                    if (usuarioNombre == null)
                    {
                        usuarioBitacora = string.Empty;
                    }
                    else
                    {
                        usuarioBitacora = usuarioNombre;
                    }

                    bitacora.Registrar("REPARAR_INTEGRIDAD", "Tarea", "OK", usuarioBitacora, "Integridad", "Origen=Login");
                }
                return true;
            }
            catch
            {
                if (bitacora != null)
                {
                    string usuarioBitacora;
                    if (usuarioNombre == null)
                    {
                        usuarioBitacora = string.Empty;
                    }
                    else
                    {
                        usuarioBitacora = usuarioNombre;
                    }

                    bitacora.Registrar("REPARAR_INTEGRIDAD", "Tarea", "FAIL", usuarioBitacora, "Integridad", "Origen=Login");
                }
                return false;
            }
        }

        public bool RepararIntegridad()
        {
            string usuarioBitacora;
            if (SesionActual.Instance != null)
            {
                usuarioBitacora = SesionActual.Instance.NombreUsuario;
            }
            else
            {
                usuarioBitacora = string.Empty;
            }
            return RepararIntegridad(usuarioBitacora);
        }

        public bool ValidarPoliticasPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            if (password.Length < 8)
            {
                return false;
            }

            bool tieneMayuscula = false;
            bool tieneMinuscula = false;
            bool tieneNumero = false;
            bool tieneEspecial = false;

            foreach (char caracter in password)
            {
                if (char.IsUpper(caracter))
                {
                    tieneMayuscula = true;
                }
                else if (char.IsLower(caracter))
                {
                    tieneMinuscula = true;
                }
                else if (char.IsDigit(caracter))
                {
                    tieneNumero = true;
                }
                else
                {
                    tieneEspecial = true;
                }
            }

            return tieneMayuscula && tieneMinuscula && tieneNumero && tieneEspecial;
        }

        private bool VerificarIntegridad(out string detalle)
        {
            detalle = string.Empty;

            TareaBL tareaBL = new TareaBL();
            string detalleDvh;
            bool verificacionDvhOk = tareaBL.VerificarDVH_Tarea(out detalleDvh);

            IntegridadBL integridadService = new IntegridadBL();
            string detalleDvv;
            bool verificacionDvvOk = integridadService.VerificarTarea(out detalleDvv);

            if (!verificacionDvhOk || !verificacionDvvOk)
            {
                string detalleDvhNormalizado;
                if (string.IsNullOrWhiteSpace(detalleDvh))
                {
                    detalleDvhNormalizado = string.Empty;
                }
                else
                {
                    detalleDvhNormalizado = detalleDvh;
                }

                string detalleDvvNormalizado;
                if (string.IsNullOrWhiteSpace(detalleDvv))
                {
                    detalleDvvNormalizado = string.Empty;
                }
                else
                {
                    detalleDvvNormalizado = detalleDvv;
                }

                detalle = (detalleDvhNormalizado + " " + detalleDvvNormalizado).Trim();
                return false;
            }

            return true;
        }

        public void Logout()
        {
            if (!sesionActual.EstaAutenticado)
            {
                return;
            }

            if (bitacora != null)
            {
                string nombreUsuario = sesionActual.NombreUsuario;
                if (nombreUsuario == null)
                {
                    nombreUsuario = string.Empty;
                }

                bitacora.Registrar("Logout", "Usuario", "OK", nombreUsuario, "Seguridad", "");
            }

            usuarioActual = null;
            sesionActual.Cerrar();
        }

        private bool EsUsuarioBloqueado(Usuario usuario)
        {
            return usuario.BloqueadoHastaUtc > DateTime.UtcNow;
        }

        private int CalcularIntentosRestantesParaProximoUmbral(int intentosActuales)
        {
            if (intentosActuales < PoliticaIntentosInicial)
            {
                return PoliticaIntentosInicial - intentosActuales;
            }

            int desdeBase = intentosActuales - PoliticaIntentosInicial;
            int resto = desdeBase % PasoEscalonIntentos;
            int faltan = PasoEscalonIntentos - resto;
            if (faltan == PasoEscalonIntentos)
            {
                return 0;
            }

            return faltan;
        }

        private void RegistrarLoginOk(Usuario usuario)
        {
            if (bitacora != null)
            {
                bitacora.Registrar("Login", "Usuario", "OK", usuario.NombreUsuario, "Seguridad", "");
            }
        }

        private void RegistrarLoginFail(string usuario, string tipo)
        {
            if (bitacora != null)
            {
                string nombreUsuario = usuario;
                if (nombreUsuario == null)
                {
                    nombreUsuario = string.Empty;
                }

                bitacora.Registrar(tipo, "Usuario", "FAIL", nombreUsuario, "Seguridad", "");
            }
        }

        private string FormatearTiempo(TimeSpan tiempo)
        {
            if (tiempo.TotalSeconds < 1)
            {
                return "0s";
            }

            if (tiempo.TotalMinutes < 1)
            {
                int segundos = (int)tiempo.TotalSeconds;
                return segundos + "s";
            }

            if (tiempo.TotalHours < 1)
            {
                int minutos = (int)tiempo.TotalMinutes;
                return minutos + "m " + tiempo.Seconds.ToString("D2") + "s";
            }

            if (tiempo.TotalHours < 24)
            {
                int horas = (int)tiempo.TotalHours;
                return horas + "h " + tiempo.Minutes.ToString("D2") + "m";
            }

            int dias = (int)tiempo.TotalDays;
            return dias + "d " + tiempo.Hours + "h";
        }

        private string FormatearTiempoBloqueo(TimeSpan tiempo)
        {
            if (tiempo.TotalHours < 1)
            {
                int minutos = (int)Math.Ceiling(tiempo.TotalMinutes);
                return minutos + "m";
            }

            if (tiempo.TotalHours < 24)
            {
                int horas = (int)tiempo.TotalHours;
                int minutos = tiempo.Minutes;
                if (minutos > 0)
                {
                    return horas + "h " + minutos + "m";
                }

                return horas + "h";
            }

            int dias = (int)tiempo.TotalDays;
            int horasRestantes = tiempo.Hours;
            if (horasRestantes > 0)
            {
                return dias + "d " + horasRestantes + "h";
            }

            return dias + "d";
        }

        public bool EstaAutenticado
        {
            get
            {
                return sesionActual.EstaAutenticado;
            }
        }

        public Usuario UsuarioActual
        {
            get
            {
                return usuarioActual;
            }
        }

        public bool ValidarPermiso(string patenteNombre)
        {
            Usuario usuario = UsuarioActual;
            if (usuario == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(patenteNombre))
            {
                return false;
            }

            if (autorizacionService == null)
            {
                return usuario.TienePermiso(patenteNombre);
            }

            return autorizacionService.TienePermiso(usuario, patenteNombre);
        }
    }
}
