using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using DAL;

namespace BL
{
    public class AutorizacionService
    {
        private readonly ComponenteDAL _compDal;

        public AutorizacionService()
        {
            _compDal = new ComponenteDAL();
        }

        public bool TienePermiso(Usuario usuario, string patenteNombre)
        {
            if (usuario == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(patenteNombre))
            {
                return false;
            }

            return usuario.TienePermiso(patenteNombre);
        }

        public Componente ConstruirArbol(int componenteId)
        {
            HashSet<int> componentesVisitados = new HashSet<int>();
            return ConstruirArbolInterno(componenteId, componentesVisitados);
        }

        private Componente ConstruirArbolInterno(int componenteId, HashSet<int> componentesVisitados)
        {
            if (componentesVisitados.Contains(componenteId))
            {
                return null;
            }

            componentesVisitados.Add(componenteId);

            Componente componente = _compDal.Obtener(componenteId);
            if (componente == null)
            {
                return null;
            }

            if (componente is Familia familia)
            {
                List<Componente> hijos = _compDal.ObtenerHijosDeFamilia(componente.Id);
                foreach (Componente hijo in hijos)
                {
                    Componente subarbol = ConstruirArbolInterno(hijo.Id, componentesVisitados);
                    if (subarbol != null)
                    {
                        familia.AgregarHijo(subarbol);
                    }
                }
            }

            return componente;
        }

        public List<Componente> ObtenerArbolUsuario(int usuarioId)
        {
            List<Componente> raices = _compDal.ObtenerRaicesDeUsuario(usuarioId);
            List<Componente> componentes = new List<Componente>();

            foreach (Componente raiz in raices)
            {
                Componente subarbol = ConstruirArbol(raiz.Id);
                if (subarbol != null)
                {
                    componentes.Add(subarbol);
                }
            }

            return componentes;
        }

        public int CrearPatente(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                throw new ArgumentException("Descripción inválida.", nameof(descripcion));
            }

            return _compDal.CrearPatente(descripcion.Trim());
        }

        public int CrearFamilia(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                throw new ArgumentException("Descripción inválida.", nameof(descripcion));
            }

            return _compDal.CrearFamilia(descripcion.Trim());
        }

        public void ActualizarFamilia(int idFamilia, string nuevaDescripcion)
        {
            if (idFamilia <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(idFamilia));
            }

            if (string.IsNullOrWhiteSpace(nuevaDescripcion))
            {
                throw new ArgumentException("Descripción inválida.", nameof(nuevaDescripcion));
            }

            _compDal.ActualizarFamilia(idFamilia, nuevaDescripcion.Trim());
        }

        public void QuitarHijoDeFamilia(int idFamilia, int idHijo)
        {
            if (idFamilia <= 0 || idHijo <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            HashSet<int> patentesHijo = ObtenerPatentesDeComponente(idHijo);

            UsuarioDAL usuarioDal = new UsuarioDAL();
            List<Usuario> usuarios = usuarioDal.Listar();

            Dictionary<int, int> cobertura = CalcularCoberturaGlobal(usuarios);

            foreach (Usuario usuarioListado in usuarios)
            {
                List<Componente> raicesUsuario = _compDal.ObtenerRaicesDeUsuario(usuarioListado.Id);
                bool tieneFamiliaComoRaiz = raicesUsuario.Any(raiz => raiz.Id == idFamilia);

                bool afecta = tieneFamiliaComoRaiz;
                if (!afecta)
                {
                    List<Componente> arbolUsuario = ObtenerArbolUsuario(usuarioListado.Id);
                    afecta = ContieneComponente(arbolUsuario, idFamilia);
                }

                if (!afecta)
                {
                    continue;
                }

                HashSet<int> patentesSinFamilia = ObtenerPatentesAsignadasUsuarioExcluyendoComponente(usuarioListado.Id, idFamilia);

                IEnumerable<int> patentesQuePerderia = patentesHijo.Where(patenteId => !patentesSinFamilia.Contains(patenteId));
                foreach (int patenteId in patentesQuePerderia)
                {
                    if (cobertura.TryGetValue(patenteId, out int cantidadUsuariosConPatente) && cantidadUsuariosConPatente <= 1)
                    {
                        string nombrePatente = ObtenerNombrePatente(patenteId);
                        throw new InvalidOperationException($"No se puede quitar el hijo: la patente '{nombrePatente}' quedaría sin responsable.");
                    }
                }
            }

            _compDal.QuitarHijoDeFamilia(idFamilia, idHijo);
        }

        public void EliminarFamiliaConAsociaciones(int idFamilia)
        {
            if (idFamilia <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(idFamilia));
            }

            HashSet<int> patentesFamilia = ObtenerPatentesDeFamilia(idFamilia);

            UsuarioDAL usuarioDal = new UsuarioDAL();
            List<Usuario> usuarios = usuarioDal.Listar();
            Dictionary<int, int> cobertura = CalcularCoberturaGlobal(usuarios);

            foreach (int patenteId in patentesFamilia)
            {
                if (cobertura.TryGetValue(patenteId, out int cantidadUsuariosConPatente) && cantidadUsuariosConPatente <= 1)
                {
                    string nombrePatente = ObtenerNombrePatente(patenteId);
                    throw new InvalidOperationException($"No se puede eliminar la familia: la patente '{nombrePatente}' quedaría sin responsable.");
                }
            }

            _compDal.EliminarFamiliaConAsociaciones(idFamilia);
        }

        public void AsignarComponenteAUsuario(int usuarioId, int componenteId)
        {
            if (usuarioId <= 0 || componenteId <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            HashSet<int> patentesUsuario = ObtenerPatentesAsignadasUsuario(usuarioId);
            HashSet<int> patentesComponente = ObtenerPatentesDeComponente(componenteId);

            List<int> patentesRepetidas = patentesUsuario.Intersect(patentesComponente).ToList();
            if (patentesRepetidas.Count > 0)
            {
                string nombresPatentes = string.Join(", ", patentesRepetidas.Select(ObtenerNombrePatente));
                throw new InvalidOperationException($"El usuario ya posee las siguientes patentes: {nombresPatentes}.");
            }

            _compDal.AsignarComponenteAUsuario(usuarioId, componenteId);
        }

        public void QuitarComponenteDeUsuario(int usuarioId, int componenteId)
        {
            if (usuarioId <= 0 || componenteId <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            HashSet<int> patentesComponente = ObtenerPatentesDeComponente(componenteId);

            HashSet<int> patentesSinComponente = ObtenerPatentesAsignadasUsuarioExcluyendoComponente(usuarioId, componenteId);

            HashSet<int> patentesQuePerderia = new HashSet<int>(patentesComponente.Where(patenteId => !patentesSinComponente.Contains(patenteId)));

            if (patentesQuePerderia.Count > 0)
            {
                UsuarioDAL usuarioDal = new UsuarioDAL();
                List<Usuario> usuarios = usuarioDal.Listar();
                Dictionary<int, int> cobertura = CalcularCoberturaGlobal(usuarios);

                foreach (int patenteId in patentesQuePerderia)
                {
                    if (cobertura.TryGetValue(patenteId, out int cantidadUsuariosConPatente) && cantidadUsuariosConPatente <= 1)
                    {
                        string nombrePatente = ObtenerNombrePatente(patenteId);
                        throw new InvalidOperationException($"No se puede quitar la asignación: la patente '{nombrePatente}' quedaría sin responsable.");
                    }
                }
            }

            _compDal.QuitarComponenteDeUsuario(usuarioId, componenteId);
        }

        public void CargarPermisosEnUsuario(Usuario usuario)
        {
            if (usuario == null || usuario.Id <= 0)
            {
                return;
            }

            usuario.LimpiarPermisos();

            List<Componente> arbolPermisos = ObtenerArbolUsuario(usuario.Id);
            foreach (Componente componente in arbolPermisos)
            {
                usuario.AgregarPermiso(componente);
            }
        }

        private bool ContieneComponente(IEnumerable<Componente> nodos, int componenteId)
        {
            if (nodos == null)
            {
                return false;
            }

            foreach (Componente componenteActual in nodos)
            {
                if (componenteActual.Id == componenteId)
                {
                    return true;
                }

                if (componenteActual is Familia familiaComponenteActual && ContieneComponente(familiaComponenteActual.ObtenerHijos(), componenteId))
                {
                    return true;
                }
            }

            return false;
        }

        private Dictionary<int, int> CalcularCoberturaGlobal(List<Usuario> usuarios)
        {
            Dictionary<int, int> cobertura = new Dictionary<int, int>();

            foreach (Usuario usuarioListado in usuarios)
            {
                HashSet<int> patentesUsuario = ObtenerPatentesAsignadasUsuario(usuarioListado.Id);
                foreach (int patenteId in patentesUsuario)
                {
                    if (cobertura.TryGetValue(patenteId, out int cantidadUsuariosConPatente))
                    {
                        cobertura[patenteId] = cantidadUsuariosConPatente + 1;
                    }
                    else
                    {
                        cobertura[patenteId] = 1;
                    }
                }
            }

            return cobertura;
        }

        private HashSet<int> ObtenerPatentesAsignadasUsuario(int usuarioId)
        {
            HashSet<int> patentesResultado = new HashSet<int>();
            List<Componente> raicesUsuario = _compDal.ObtenerRaicesDeUsuario(usuarioId);
            HashSet<int> familiasVisitadas = new HashSet<int>();

            foreach (Componente raiz in raicesUsuario)
            {
                if (raiz is Patente)
                {
                    patentesResultado.Add(raiz.Id);
                }
                else if (raiz is Familia)
                {
                    ColectarPatentesDeFamilia(raiz.Id, patentesResultado, familiasVisitadas);
                }
            }

            return patentesResultado;
        }

        private HashSet<int> ObtenerPatentesAsignadasUsuarioExcluyendoComponente(int usuarioId, int componenteAExcluir)
        {
            HashSet<int> patentesResultado = new HashSet<int>();
            List<Componente> raicesUsuario = _compDal.ObtenerRaicesDeUsuario(usuarioId);
            HashSet<int> familiasVisitadas = new HashSet<int>();

            foreach (Componente raiz in raicesUsuario)
            {
                if (raiz.Id == componenteAExcluir)
                {
                    continue;
                }

                if (raiz is Patente)
                {
                    patentesResultado.Add(raiz.Id);
                }
                else if (raiz is Familia)
                {
                    ColectarPatentesDeFamilia(raiz.Id, patentesResultado, familiasVisitadas);
                }
            }

            return patentesResultado;
        }

        private HashSet<int> ObtenerPatentesDeComponente(int componenteId)
        {
            HashSet<int> patentesResultado = new HashSet<int>();
            Componente componente = _compDal.Obtener(componenteId);
            if (componente == null)
            {
                return patentesResultado;
            }

            HashSet<int> familiasVisitadas = new HashSet<int>();
            if (componente is Patente)
            {
                patentesResultado.Add(componente.Id);
            }
            else if (componente is Familia)
            {
                ColectarPatentesDeFamilia(componente.Id, patentesResultado, familiasVisitadas);
            }

            return patentesResultado;
        }

        private HashSet<int> ObtenerPatentesDeFamilia(int idFamilia)
        {
            HashSet<int> patentesResultado = new HashSet<int>();
            HashSet<int> familiasVisitadas = new HashSet<int>();
            ColectarPatentesDeFamilia(idFamilia, patentesResultado, familiasVisitadas);
            return patentesResultado;
        }

        private void ColectarPatentesDeFamilia(int idFamilia, HashSet<int> patentesSalida, HashSet<int> familiasVisitadas)
        {
            if (familiasVisitadas.Contains(idFamilia))
            {
                return;
            }

            familiasVisitadas.Add(idFamilia);

            List<Componente> hijosFamilia = _compDal.ObtenerHijosDeFamilia(idFamilia);
            foreach (Componente hijo in hijosFamilia)
            {
                if (hijo is Patente)
                {
                    patentesSalida.Add(hijo.Id);
                }
                else if (hijo is Familia)
                {
                    ColectarPatentesDeFamilia(hijo.Id, patentesSalida, familiasVisitadas);
                }
            }
        }

        private string ObtenerNombrePatente(int id)
        {
            Componente componentePatente = _compDal.Obtener(id);
            return componentePatente?.Nombre ?? $"Patente {id}";
        }

        public List<Componente> ListarTodasFamilias() => _compDal.ObtenerTodasFamilias();

        public List<Componente> ListarTodasPatentes() => _compDal.ObtenerTodasPatentes();

        public List<Componente> ObtenerHijosFamilia(int idFamilia) => _compDal.ObtenerHijosDeFamilia(idFamilia);

        public List<Componente> ObtenerAsignacionesUsuario(int usuarioId) => _compDal.ObtenerRaicesDeUsuario(usuarioId);

        public void AgregarHijoAFamilia(int idFamilia, int idHijo)
        {
            if (idFamilia <= 0 || idHijo <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            HashSet<int> patentesExistentes = ObtenerPatentesDeFamilia(idFamilia);
            HashSet<int> patentesAAgregar = ObtenerPatentesDeComponente(idHijo);

            List<int> patentesRepetidas = patentesExistentes.Intersect(patentesAAgregar).ToList();
            if (patentesRepetidas.Count > 0)
            {
                string nombresPatentes = string.Join(", ", patentesRepetidas.Select(ObtenerNombrePatente));
                throw new InvalidOperationException($"La familia ya contiene las siguientes patentes: {nombresPatentes}.");
            }

            _compDal.AgregarHijoAFamilia(idFamilia, idHijo);
        }
    }
}
