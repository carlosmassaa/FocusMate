namespace UI
{
    partial class MainMidForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tareasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crearTareaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verTareasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generarPlanificacionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionarPlanificacionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.top10ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tiempoDisponibleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administraciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bitacoraDeEventosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeViewPermisosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administrarPermisosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administrarFamiliasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionarUsuarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cargarIdiomaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configuracionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cambiarIdiomaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administrarBuckupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cerrarSesiónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();

            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tareasToolStripMenuItem,
            this.administraciónToolStripMenuItem,
            this.configuracionToolStripMenuItem,
            this.cerrarSesiónToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1182, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";

            // 
            // tareasToolStripMenuItem
            // 
            this.tareasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.crearTareaToolStripMenuItem,
            this.verTareasToolStripMenuItem,
            this.generarPlanificacionToolStripMenuItem,
            this.gestionarPlanificacionesToolStripMenuItem,
            this.top10ToolStripMenuItem,
            this.tiempoDisponibleToolStripMenuItem});
            this.tareasToolStripMenuItem.Name = "tareasToolStripMenuItem";
            this.tareasToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.tareasToolStripMenuItem.Text = "Tareas";
            this.tareasToolStripMenuItem.Click += new System.EventHandler(this.tareasToolStripMenuItem_Click);

            // 
            // crearTareaToolStripMenuItem
            // 
            this.crearTareaToolStripMenuItem.Name = "crearTareaToolStripMenuItem";
            this.crearTareaToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.crearTareaToolStripMenuItem.Text = "Registrar Tarea";
            this.crearTareaToolStripMenuItem.Click += new System.EventHandler(this.crearTareaToolStripMenuItem_Click);

            // 
            // verTareasToolStripMenuItem
            // 
            this.verTareasToolStripMenuItem.Name = "verTareasToolStripMenuItem";
            this.verTareasToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.verTareasToolStripMenuItem.Text = "Ver Tareas";
            this.verTareasToolStripMenuItem.Click += new System.EventHandler(this.verTareasToolStripMenuItem_Click);

            // 
            // generarPlanificacionToolStripMenuItem
            // 
            this.generarPlanificacionToolStripMenuItem.Name = "generarPlanificacionToolStripMenuItem";
            this.generarPlanificacionToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.generarPlanificacionToolStripMenuItem.Text = "Generar Planificación";
            this.generarPlanificacionToolStripMenuItem.Click += new System.EventHandler(this.generarPlanificacionToolStripMenuItem_Click);

            // 
            // gestionarPlanificacionesToolStripMenuItem
            // 
            this.gestionarPlanificacionesToolStripMenuItem.Name = "gestionarPlanificacionesToolStripMenuItem";
            this.gestionarPlanificacionesToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.gestionarPlanificacionesToolStripMenuItem.Text = "Gestionar Planificaciones";
            this.gestionarPlanificacionesToolStripMenuItem.Click += new System.EventHandler(this.gestionarPlanificacionesToolStripMenuItem_Click);

            // 
            // top10ToolStripMenuItem
            // 
            this.top10ToolStripMenuItem.Name = "top10ToolStripMenuItem";
            this.top10ToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.top10ToolStripMenuItem.Text = "TOP 10";
            this.top10ToolStripMenuItem.Click += new System.EventHandler(this.top10ToolStripMenuItem_Click);

            // 
            // tiempoDisponibleToolStripMenuItem
            // 
            this.tiempoDisponibleToolStripMenuItem.Name = "tiempoDisponibleToolStripMenuItem";
            this.tiempoDisponibleToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.tiempoDisponibleToolStripMenuItem.Text = "Tiempo Disponible";
            this.tiempoDisponibleToolStripMenuItem.Click += new System.EventHandler(this.tiempoDisponibleToolStripMenuItem_Click);

            // 
            // administraciónToolStripMenuItem
            // 
            this.administraciónToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bitacoraDeEventosToolStripMenuItem,
            this.treeViewPermisosToolStripMenuItem,
            this.administrarPermisosToolStripMenuItem,
            this.administrarFamiliasToolStripMenuItem,
            this.gestionarUsuarioToolStripMenuItem,
            this.cargarIdiomaToolStripMenuItem});
            this.administraciónToolStripMenuItem.Name = "administraciónToolStripMenuItem";
            this.administraciónToolStripMenuItem.Size = new System.Drawing.Size(100, 20);
            this.administraciónToolStripMenuItem.Text = "Administración";
            this.administraciónToolStripMenuItem.Click += new System.EventHandler(this.administraciónToolStripMenuItem_Click);

            // 
            // bitacoraDeEventosToolStripMenuItem
            // 
            this.bitacoraDeEventosToolStripMenuItem.Name = "bitacoraDeEventosToolStripMenuItem";
            this.bitacoraDeEventosToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.bitacoraDeEventosToolStripMenuItem.Text = "Bitácora de Eventos";
            this.bitacoraDeEventosToolStripMenuItem.Click += new System.EventHandler(this.bitacoraDeEventosToolStripMenuItem_Click);

            // 
            // treeViewPermisosToolStripMenuItem
            // 
            this.treeViewPermisosToolStripMenuItem.Name = "treeViewPermisosToolStripMenuItem";
            this.treeViewPermisosToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.treeViewPermisosToolStripMenuItem.Text = "TreeView Permisos";
            this.treeViewPermisosToolStripMenuItem.Click += new System.EventHandler(this.treeViewPermisosToolStripMenuItem_Click);

            // 
            // administrarPermisosToolStripMenuItem
            // 
            this.administrarPermisosToolStripMenuItem.Name = "administrarPermisosToolStripMenuItem";
            this.administrarPermisosToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.administrarPermisosToolStripMenuItem.Text = "Administrar Permisos";
            this.administrarPermisosToolStripMenuItem.Click += new System.EventHandler(this.administrarPermisosToolStripMenuItem_Click);

            // 
            // administrarFamiliasToolStripMenuItem
            // 
            this.administrarFamiliasToolStripMenuItem.Name = "administrarFamiliasToolStripMenuItem";
            this.administrarFamiliasToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.administrarFamiliasToolStripMenuItem.Text = "Administrar Familias";
            this.administrarFamiliasToolStripMenuItem.Click += new System.EventHandler(this.administrarFamiliasToolStripMenuItem_Click_1);

            // 
            // gestionarUsuarioToolStripMenuItem
            // 
            this.gestionarUsuarioToolStripMenuItem.Name = "gestionarUsuarioToolStripMenuItem";
            this.gestionarUsuarioToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.gestionarUsuarioToolStripMenuItem.Text = "Gestionar Usuarios";
            this.gestionarUsuarioToolStripMenuItem.Click += new System.EventHandler(this.gestionarUsuarioToolStripMenuItem_Click);

            // 
            // cargarIdiomaToolStripMenuItem
            // 
            this.cargarIdiomaToolStripMenuItem.Name = "cargarIdiomaToolStripMenuItem";
            this.cargarIdiomaToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.cargarIdiomaToolStripMenuItem.Text = "Cargar Idioma";
            this.cargarIdiomaToolStripMenuItem.Click += new System.EventHandler(this.cargarIdiomaToolStripMenuItem_Click);

            // 
            // configuracionToolStripMenuItem
            // 
            this.configuracionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cambiarIdiomaToolStripMenuItem,
            this.administrarBuckupToolStripMenuItem});
            this.configuracionToolStripMenuItem.Name = "configuracionToolStripMenuItem";
            this.configuracionToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.configuracionToolStripMenuItem.Text = "Configuracion";
            this.configuracionToolStripMenuItem.Click += new System.EventHandler(this.configuracionToolStripMenuItem_Click);

            // 
            // cambiarIdiomaToolStripMenuItem
            // 
            this.cambiarIdiomaToolStripMenuItem.Name = "cambiarIdiomaToolStripMenuItem";
            this.cambiarIdiomaToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.cambiarIdiomaToolStripMenuItem.Text = "Cambiar Idioma";
            this.cambiarIdiomaToolStripMenuItem.Click += new System.EventHandler(this.cambiarIdiomaToolStripMenuItem_Click);

            // 
            // administrarBuckupToolStripMenuItem
            // 
            this.administrarBuckupToolStripMenuItem.Name = "administrarBuckupToolStripMenuItem";
            this.administrarBuckupToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.administrarBuckupToolStripMenuItem.Text = "Administrar Buckup";
            this.administrarBuckupToolStripMenuItem.Click += new System.EventHandler(this.administrarBuckupToolStripMenuItem_Click);

            // 
            // cerrarSesiónToolStripMenuItem
            // 
            this.cerrarSesiónToolStripMenuItem.Name = "cerrarSesiónToolStripMenuItem";
            this.cerrarSesiónToolStripMenuItem.Size = new System.Drawing.Size(87, 20);
            this.cerrarSesiónToolStripMenuItem.Text = "Cerrar sesión";
            this.cerrarSesiónToolStripMenuItem.Click += new System.EventHandler(this.cerrarSesiónToolStripMenuItem_Click);

            // 
            // MainMidForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 545);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainMidForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainMidForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tareasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem crearTareaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verTareasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generarPlanificacionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestionarPlanificacionesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem top10ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tiempoDisponibleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem administraciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bitacoraDeEventosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem treeViewPermisosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem administrarPermisosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem administrarFamiliasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestionarUsuarioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cargarIdiomaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configuracionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cambiarIdiomaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem administrarBuckupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cerrarSesiónToolStripMenuItem;
    }
}