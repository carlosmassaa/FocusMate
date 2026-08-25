using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BL;
using BE;
using Servicioss;
using System.Drawing;

namespace UI
{
    public partial class FrmPermisos : Form, IIdiomaObserver
    {
        private readonly AutorizacionService _autorizacionService = new AutorizacionService();
        private readonly UsuarioBL _usuarioBl = new UsuarioBL();
        private readonly AuthManager _authManager;

        public FrmPermisos(AuthManager authManager)
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            _authManager = authManager;

            Load += FrmPermisos_Load;
            comboBoxSeleccionUsuario.SelectedIndexChanged += comboBoxSeleccionUsuario_SelectedIndexChanged;
        }

        protected override void OnLoad(EventArgs eventArgs)
        {
            base.OnLoad(eventArgs);
            IdiomaService.Instancia.Suscribir(this);
            Dictionary<string, string> traduccionesActuales = IdiomaService.Instancia.ObtenerTraduccionesActuales();
            if (traduccionesActuales != null && traduccionesActuales.Count > 0)
            {
                ActualizarTraducciones(traduccionesActuales);
            }
            else
            {
                AplicarTraduccionesEstaticas();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs eventArgs)
        {
            IdiomaService.Instancia.Desuscribir(this);
            base.OnFormClosed(eventArgs);
        }

        private void FrmPermisos_Load(object sender, EventArgs eventArgs)
        {
            if (_authManager == null || !_authManager.EstaAutenticado || !_authManager.ValidarPermiso("ACCESO_TREEVIEW"))
            {
                string mensaje = Trad("Permisos_Msg_SinPermiso", "No tiene permiso para ver el árbol de permisos.");
                string titulo = Trad("Permisos_Msg_Titulo", "Permisos");
                MessageBox.Show(this, mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            List<Usuario> usuarios = _usuarioBl.Listar();
            comboBoxSeleccionUsuario.DisplayMember = "NombreUsuario";
            comboBoxSeleccionUsuario.ValueMember = "Id";
            comboBoxSeleccionUsuario.DataSource = usuarios;
            if (usuarios.Count > 0)
            {
                comboBoxSeleccionUsuario.SelectedIndex = 0;
            }
        }

        private void comboBoxSeleccionUsuario_SelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            int usuarioId = 0;
            if (comboBoxSeleccionUsuario.SelectedValue is int valorSeleccionado)
            {
                usuarioId = valorSeleccionado;
            }

            CargarArbol(usuarioId);
        }

        private void CargarArbol(int usuarioId)
        {
            treeViewPermisos.BeginUpdate();
            treeViewPermisos.Nodes.Clear();

            if (usuarioId > 0)
            {
                List<Componente> componentesRaiz = _autorizacionService.ObtenerArbolUsuario(usuarioId);
                for (int indice = 0; indice < componentesRaiz.Count; indice++)
                {
                    TreeNode nodo = CrearNodo(componentesRaiz[indice]);
                    if (nodo != null)
                    {
                        treeViewPermisos.Nodes.Add(nodo);
                    }
                }
            }

            treeViewPermisos.EndUpdate();
            treeViewPermisos.ExpandAll();
        }

        private TreeNode CrearNodo(Componente componente)
        {
            if (componente == null)
            {
                return null;
            }

            string textoNodo;
            if (string.IsNullOrWhiteSpace(componente.Nombre))
            {
                textoNodo = componente.Descripcion;
            }
            else
            {
                textoNodo = componente.Nombre;
            }

            TreeNode nodo = new TreeNode(textoNodo);
            nodo.Tag = componente;

            foreach (Componente hijo in componente.ObtenerHijos())
            {
                TreeNode nodoHijo = CrearNodo(hijo);
                if (nodoHijo != null)
                {
                    nodo.Nodes.Add(nodoHijo);
                }
            }

            return nodo;
        }

        public void ActualizarTraducciones(Dictionary<string, string> traducciones)
        {
            if (traducciones == null)
            {
                return;
            }

            AplicarTraduccionesEstaticas();
        }

        private void AplicarTraduccionesEstaticas()
        {
            string titulo = IdiomaService.Instancia.Traducir("Permisos_Titulo");
            if (!string.IsNullOrWhiteSpace(titulo) && titulo != "Permisos_Titulo")
            {
                Text = titulo;
            }
            else
            {
                Text = "Permisos del Usuario";
            }

            if (lblUsuario != null)
            {
                string etiquetaUsuario = IdiomaService.Instancia.Traducir("Permisos_Label_Usuario");
                if (!string.IsNullOrWhiteSpace(etiquetaUsuario) && etiquetaUsuario != "Permisos_Label_Usuario")
                {
                    lblUsuario.Text = etiquetaUsuario;
                }
                else
                {
                    lblUsuario.Text = "Usuario:";
                }
            }
        }

        private string Trad(string key, string fallback)
        {
            string textoTraducido = IdiomaService.Instancia.Traducir(key);
            if (string.IsNullOrWhiteSpace(textoTraducido) || textoTraducido == key)
            {
                return fallback;
            }

            return textoTraducido;
        }

        private void FrmPermisos_Load_1(object sender, EventArgs eventArgs)
        {
        }

        private void treeViewPermisos_AfterSelect(object sender, TreeViewEventArgs eventArgs)
        {
        }
    }
}
