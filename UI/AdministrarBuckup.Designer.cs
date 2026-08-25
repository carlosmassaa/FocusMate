namespace UI
{
    partial class AdministrarBuckup
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblDirCaption;
        private System.Windows.Forms.TextBox txtDirectorio;
        private System.Windows.Forms.Label lblNombreCaption;
        private System.Windows.Forms.TextBox txtNombreArchivo;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
        // NUEVO controles restauración
        private System.Windows.Forms.GroupBox grpCrear;
        private System.Windows.Forms.GroupBox grpRestaurar;
        private System.Windows.Forms.Label lblArchivoSeleccionado;
        private System.Windows.Forms.TextBox txtArchivoSeleccionado;
        private System.Windows.Forms.Button btnSeleccionarBackup;
        private System.Windows.Forms.Button btnRestaurar;
        private System.Windows.Forms.Button btnCancelarRestore;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblDirCaption = new System.Windows.Forms.Label();
            this.txtDirectorio = new System.Windows.Forms.TextBox();
            this.lblNombreCaption = new System.Windows.Forms.Label();
            this.txtNombreArchivo = new System.Windows.Forms.TextBox();
            this.lblHint = new System.Windows.Forms.Label();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.grpCrear = new System.Windows.Forms.GroupBox();
            this.grpRestaurar = new System.Windows.Forms.GroupBox();
            this.lblArchivoSeleccionado = new System.Windows.Forms.Label();
            this.txtArchivoSeleccionado = new System.Windows.Forms.TextBox();
            this.btnSeleccionarBackup = new System.Windows.Forms.Button();
            this.btnRestaurar = new System.Windows.Forms.Button();
            this.btnCancelarRestore = new System.Windows.Forms.Button();
            this.grpCrear.SuspendLayout();
            this.grpRestaurar.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpCrear
            // 
            this.grpCrear.Controls.Add(this.lblDirCaption);
            this.grpCrear.Controls.Add(this.txtDirectorio);
            this.grpCrear.Controls.Add(this.lblNombreCaption);
            this.grpCrear.Controls.Add(this.txtNombreArchivo);
            this.grpCrear.Controls.Add(this.lblHint);
            this.grpCrear.Controls.Add(this.btnGuardar);
            this.grpCrear.Controls.Add(this.btnCancelar);
            this.grpCrear.Location = new System.Drawing.Point(12, 12);
            this.grpCrear.Name = "grpCrear";
            this.grpCrear.Size = new System.Drawing.Size(640, 170);
            this.grpCrear.TabIndex = 0;
            this.grpCrear.TabStop = false;
            this.grpCrear.Text = "Crear backup";
            // 
            // lblDirCaption
            // 
            this.lblDirCaption.AutoSize = true;
            this.lblDirCaption.Location = new System.Drawing.Point(16, 24);
            this.lblDirCaption.Name = "lblDirCaption";
            this.lblDirCaption.Size = new System.Drawing.Size(179, 13);
            this.lblDirCaption.TabIndex = 0;
            this.lblDirCaption.Text = "Directorio de backup (en el servidor):";
            // 
            // txtDirectorio
            // 
            this.txtDirectorio.Location = new System.Drawing.Point(19, 40);
            this.txtDirectorio.Name = "txtDirectorio";
            this.txtDirectorio.ReadOnly = true;
            this.txtDirectorio.Size = new System.Drawing.Size(604, 20);
            this.txtDirectorio.TabIndex = 1;
            // 
            // lblNombreCaption
            // 
            this.lblNombreCaption.AutoSize = true;
            this.lblNombreCaption.Location = new System.Drawing.Point(16, 72);
            this.lblNombreCaption.Name = "lblNombreCaption";
            this.lblNombreCaption.Size = new System.Drawing.Size(140, 13);
            this.lblNombreCaption.TabIndex = 2;
            this.lblNombreCaption.Text = "Nombre de archivo (sin ruta):";
            // 
            // txtNombreArchivo
            // 
            this.txtNombreArchivo.Location = new System.Drawing.Point(19, 88);
            this.txtNombreArchivo.Name = "txtNombreArchivo";
            this.txtNombreArchivo.Size = new System.Drawing.Size(604, 20);
            this.txtNombreArchivo.TabIndex = 3;
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.lblHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblHint.Location = new System.Drawing.Point(16, 111);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(228, 13);
            this.lblHint.TabIndex = 4;
            this.lblHint.Text = "Sugerencia: se agregará .bak si no lo especifica.";
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(457, 133);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(86, 27);
            this.btnGuardar.TabIndex = 5;
            this.btnGuardar.Text = "Guardar backup";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(549, 133);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(74, 27);
            this.btnCancelar.TabIndex = 6;
            this.btnCancelar.Text = "Cerrar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // grpRestaurar
            // 
            this.grpRestaurar.Controls.Add(this.lblArchivoSeleccionado);
            this.grpRestaurar.Controls.Add(this.txtArchivoSeleccionado);
            this.grpRestaurar.Controls.Add(this.btnSeleccionarBackup);
            this.grpRestaurar.Controls.Add(this.btnRestaurar);
            this.grpRestaurar.Controls.Add(this.btnCancelarRestore);
            this.grpRestaurar.Location = new System.Drawing.Point(12, 195);
            this.grpRestaurar.Name = "grpRestaurar";
            this.grpRestaurar.Size = new System.Drawing.Size(640, 150);
            this.grpRestaurar.TabIndex = 1;
            this.grpRestaurar.TabStop = false;
            this.grpRestaurar.Text = "Restaurar backup (reemplaza la base de datos)";
            // 
            // lblArchivoSeleccionado
            // 
            this.lblArchivoSeleccionado.AutoSize = true;
            this.lblArchivoSeleccionado.Location = new System.Drawing.Point(16, 29);
            this.lblArchivoSeleccionado.Name = "lblArchivoSeleccionado";
            this.lblArchivoSeleccionado.Size = new System.Drawing.Size(107, 13);
            this.lblArchivoSeleccionado.TabIndex = 0;
            this.lblArchivoSeleccionado.Text = "Archivo .bak elegido:";
            // 
            // txtArchivoSeleccionado
            // 
            this.txtArchivoSeleccionado.Location = new System.Drawing.Point(19, 45);
            this.txtArchivoSeleccionado.Name = "txtArchivoSeleccionado";
            this.txtArchivoSeleccionado.ReadOnly = true;
            this.txtArchivoSeleccionado.Size = new System.Drawing.Size(604, 20);
            this.txtArchivoSeleccionado.TabIndex = 1;
            // 
            // btnSeleccionarBackup
            // 
            this.btnSeleccionarBackup.Location = new System.Drawing.Point(19, 78);
            this.btnSeleccionarBackup.Name = "btnSeleccionarBackup";
            this.btnSeleccionarBackup.Size = new System.Drawing.Size(140, 27);
            this.btnSeleccionarBackup.TabIndex = 2;
            this.btnSeleccionarBackup.Text = "Seleccionar archivo...";
            this.btnSeleccionarBackup.UseVisualStyleBackColor = true;
            this.btnSeleccionarBackup.Click += new System.EventHandler(this.btnSeleccionarBackup_Click);
            // 
            // btnRestaurar
            // 
            this.btnRestaurar.Location = new System.Drawing.Point(457, 111);
            this.btnRestaurar.Name = "btnRestaurar";
            this.btnRestaurar.Size = new System.Drawing.Size(86, 27);
            this.btnRestaurar.TabIndex = 3;
            this.btnRestaurar.Text = "Restaurar";
            this.btnRestaurar.UseVisualStyleBackColor = true;
            this.btnRestaurar.Click += new System.EventHandler(this.btnRestaurar_Click);
            // 
            // btnCancelarRestore
            // 
            this.btnCancelarRestore.Location = new System.Drawing.Point(549, 111);
            this.btnCancelarRestore.Name = "btnCancelarRestore";
            this.btnCancelarRestore.Size = new System.Drawing.Size(74, 27);
            this.btnCancelarRestore.TabIndex = 4;
            this.btnCancelarRestore.Text = "Cancelar";
            this.btnCancelarRestore.UseVisualStyleBackColor = true;
            this.btnCancelarRestore.Click += new System.EventHandler(this.btnCancelarRestore_Click);
            // 
            // AdministrarBuckup
            // 
            this.AcceptButton = this.btnGuardar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 360);
            this.Controls.Add(this.grpRestaurar);
            this.Controls.Add(this.grpCrear);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdministrarBuckup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Administrar Buckup";
            this.Load += new System.EventHandler(this.AdministrarBuckup_Load);
            this.grpCrear.ResumeLayout(false);
            this.grpCrear.PerformLayout();
            this.grpRestaurar.ResumeLayout(false);
            this.grpRestaurar.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}