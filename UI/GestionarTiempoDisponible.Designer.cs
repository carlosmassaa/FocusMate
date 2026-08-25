namespace UI
{
    partial class GestionarTiempoDisponible
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.comboBoxSeleccionarUsuarios = new System.Windows.Forms.ComboBox();
            this.btnConfigurarJornada = new System.Windows.Forms.Button();
            this.btnConfigurarBloques = new System.Windows.Forms.Button();
            this.btnGenerarAgenda = new System.Windows.Forms.Button();
            this.lblAgenda = new System.Windows.Forms.Label();
            this.panelCalendario = new System.Windows.Forms.Panel();
            this.panelDetalle = new System.Windows.Forms.Panel();
            this.lblDetalleTexto = new System.Windows.Forms.Label();
            this.lblDetalleTitulo = new System.Windows.Forms.Label();
            this.lblResumen = new System.Windows.Forms.Label();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnExportarPdf = new System.Windows.Forms.Button();
            this.btnGuardarCopiaAgenda = new System.Windows.Forms.Button();
            this.btnCargarCopiaAgenda = new System.Windows.Forms.Button();
            this.panelDetalle.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(12, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(142, 13);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Gestión de tiempo disponible";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(12, 43);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(43, 13);
            this.lblUsuario.TabIndex = 1;
            this.lblUsuario.Text = "Usuario";
            // 
            // comboBoxSeleccionarUsuarios
            // 
            this.comboBoxSeleccionarUsuarios.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSeleccionarUsuarios.FormattingEnabled = true;
            this.comboBoxSeleccionarUsuarios.Location = new System.Drawing.Point(72, 40);
            this.comboBoxSeleccionarUsuarios.Name = "comboBoxSeleccionarUsuarios";
            this.comboBoxSeleccionarUsuarios.Size = new System.Drawing.Size(270, 21);
            this.comboBoxSeleccionarUsuarios.TabIndex = 2;
            this.comboBoxSeleccionarUsuarios.SelectedIndexChanged += new System.EventHandler(this.comboBoxSeleccionarUsuarios_SelectedIndexChanged);
            // 
            // btnConfigurarJornada
            // 
            this.btnConfigurarJornada.Location = new System.Drawing.Point(15, 78);
            this.btnConfigurarJornada.Name = "btnConfigurarJornada";
            this.btnConfigurarJornada.Size = new System.Drawing.Size(145, 28);
            this.btnConfigurarJornada.TabIndex = 3;
            this.btnConfigurarJornada.Text = "Configurar jornada";
            this.btnConfigurarJornada.UseVisualStyleBackColor = true;
            this.btnConfigurarJornada.Click += new System.EventHandler(this.btnConfigurarJornada_Click);
            // 
            // btnConfigurarBloques
            // 
            this.btnConfigurarBloques.Location = new System.Drawing.Point(166, 78);
            this.btnConfigurarBloques.Name = "btnConfigurarBloques";
            this.btnConfigurarBloques.Size = new System.Drawing.Size(145, 28);
            this.btnConfigurarBloques.TabIndex = 4;
            this.btnConfigurarBloques.Text = "Configurar bloques";
            this.btnConfigurarBloques.UseVisualStyleBackColor = true;
            this.btnConfigurarBloques.Click += new System.EventHandler(this.btnConfigurarBloques_Click);
            // 
            // btnGenerarAgenda
            // 
            this.btnGenerarAgenda.Location = new System.Drawing.Point(317, 78);
            this.btnGenerarAgenda.Name = "btnGenerarAgenda";
            this.btnGenerarAgenda.Size = new System.Drawing.Size(145, 28);
            this.btnGenerarAgenda.TabIndex = 5;
            this.btnGenerarAgenda.Text = "Generar agenda";
            this.btnGenerarAgenda.UseVisualStyleBackColor = true;
            this.btnGenerarAgenda.Click += new System.EventHandler(this.btnGenerarAgenda_Click);
            // 
            // lblAgenda
            // 
            this.lblAgenda.AutoSize = true;
            this.lblAgenda.Location = new System.Drawing.Point(12, 125);
            this.lblAgenda.Name = "lblAgenda";
            this.lblAgenda.Size = new System.Drawing.Size(78, 13);
            this.lblAgenda.TabIndex = 6;
            this.lblAgenda.Text = "Agenda laboral";
            // 
            // panelCalendario
            // 
            this.panelCalendario.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCalendario.AutoScroll = true;
            this.panelCalendario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCalendario.Location = new System.Drawing.Point(15, 150);
            this.panelCalendario.Name = "panelCalendario";
            this.panelCalendario.Size = new System.Drawing.Size(915, 500);
            this.panelCalendario.TabIndex = 7;
            // 
            // panelDetalle
            // 
            this.panelDetalle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDetalle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDetalle.Controls.Add(this.lblDetalleTexto);
            this.panelDetalle.Controls.Add(this.lblDetalleTitulo);
            this.panelDetalle.Location = new System.Drawing.Point(945, 150);
            this.panelDetalle.Name = "panelDetalle";
            this.panelDetalle.Size = new System.Drawing.Size(228, 500);
            this.panelDetalle.TabIndex = 8;
            // 
            // lblDetalleTexto
            // 
            this.lblDetalleTexto.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDetalleTexto.Location = new System.Drawing.Point(10, 42);
            this.lblDetalleTexto.Name = "lblDetalleTexto";
            this.lblDetalleTexto.Size = new System.Drawing.Size(205, 440);
            this.lblDetalleTexto.TabIndex = 1;
            this.lblDetalleTexto.Text = "Seleccione un bloque del calendario.";
            // 
            // lblDetalleTitulo
            // 
            this.lblDetalleTitulo.AutoSize = true;
            this.lblDetalleTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblDetalleTitulo.Location = new System.Drawing.Point(10, 13);
            this.lblDetalleTitulo.Name = "lblDetalleTitulo";
            this.lblDetalleTitulo.Size = new System.Drawing.Size(47, 13);
            this.lblDetalleTitulo.TabIndex = 0;
            this.lblDetalleTitulo.Text = "Detalle";
            // 
            // lblResumen
            // 
            this.lblResumen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblResumen.AutoSize = true;
            this.lblResumen.Location = new System.Drawing.Point(12, 668);
            this.lblResumen.Name = "lblResumen";
            this.lblResumen.Size = new System.Drawing.Size(55, 13);
            this.lblResumen.TabIndex = 9;
            this.lblResumen.Text = "Resumen:";
            // 
            // lblMensaje
            // 
            this.lblMensaje.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMensaje.AutoSize = true;
            this.lblMensaje.Location = new System.Drawing.Point(12, 696);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(0, 13);
            this.lblMensaje.TabIndex = 10;
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Location = new System.Drawing.Point(1063, 686);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(110, 28);
            this.btnCerrar.TabIndex = 11;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // btnExportarPdf
            // 
            this.btnExportarPdf.Location = new System.Drawing.Point(468, 78);
            this.btnExportarPdf.Name = "btnExportarPdf";
            this.btnExportarPdf.Size = new System.Drawing.Size(145, 28);
            this.btnExportarPdf.TabIndex = 12;
            this.btnExportarPdf.Text = "Exportar PDF";
            this.btnExportarPdf.UseVisualStyleBackColor = true;
            this.btnExportarPdf.Click += new System.EventHandler(this.btnExportarPdf_Click);
            // 
            // btnGuardarCopiaAgenda
            // 
            this.btnGuardarCopiaAgenda.Location = new System.Drawing.Point(619, 78);
            this.btnGuardarCopiaAgenda.Name = "btnGuardarCopiaAgenda";
            this.btnGuardarCopiaAgenda.Size = new System.Drawing.Size(145, 28);
            this.btnGuardarCopiaAgenda.TabIndex = 13;
            this.btnGuardarCopiaAgenda.Text = "Guardar copia";
            this.btnGuardarCopiaAgenda.UseVisualStyleBackColor = true;
            this.btnGuardarCopiaAgenda.Click += new System.EventHandler(this.btnGuardarCopiaAgenda_Click);
            // 
            // btnCargarCopiaAgenda
            // 
            this.btnCargarCopiaAgenda.Location = new System.Drawing.Point(770, 78);
            this.btnCargarCopiaAgenda.Name = "btnCargarCopiaAgenda";
            this.btnCargarCopiaAgenda.Size = new System.Drawing.Size(145, 28);
            this.btnCargarCopiaAgenda.TabIndex = 14;
            this.btnCargarCopiaAgenda.Text = "Cargar copia";
            this.btnCargarCopiaAgenda.UseVisualStyleBackColor = true;
            this.btnCargarCopiaAgenda.Click += new System.EventHandler(this.btnCargarCopiaAgenda_Click);
            // 
            // GestionarTiempoDisponible
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1187, 726);
            this.Controls.Add(this.btnCargarCopiaAgenda);
            this.Controls.Add(this.btnGuardarCopiaAgenda);
            this.Controls.Add(this.btnExportarPdf);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.lblMensaje);
            this.Controls.Add(this.lblResumen);
            this.Controls.Add(this.panelDetalle);
            this.Controls.Add(this.panelCalendario);
            this.Controls.Add(this.lblAgenda);
            this.Controls.Add(this.btnGenerarAgenda);
            this.Controls.Add(this.btnConfigurarBloques);
            this.Controls.Add(this.btnConfigurarJornada);
            this.Controls.Add(this.comboBoxSeleccionarUsuarios);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.lblTitulo);
            this.Name = "GestionarTiempoDisponible";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gestionar tiempo disponible";
            this.Load += new System.EventHandler(this.GestionarTiempoDisponible_Load);
            this.panelDetalle.ResumeLayout(false);
            this.panelDetalle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.ComboBox comboBoxSeleccionarUsuarios;
        private System.Windows.Forms.Button btnConfigurarJornada;
        private System.Windows.Forms.Button btnConfigurarBloques;
        private System.Windows.Forms.Button btnGenerarAgenda;
        private System.Windows.Forms.Label lblAgenda;
        private System.Windows.Forms.Panel panelCalendario;
        private System.Windows.Forms.Panel panelDetalle;
        private System.Windows.Forms.Label lblDetalleTexto;
        private System.Windows.Forms.Label lblDetalleTitulo;
        private System.Windows.Forms.Label lblResumen;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Button btnExportarPdf;
        private System.Windows.Forms.Button btnGuardarCopiaAgenda;
        private System.Windows.Forms.Button btnCargarCopiaAgenda;
    }
}