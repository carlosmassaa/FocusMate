namespace UI
{
    partial class FrmIntegridadDv
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDetalleCaption;
        private System.Windows.Forms.TextBox txtDetalle;
        private System.Windows.Forms.Button btnAceptarCambios;
        private System.Windows.Forms.Button btnRestaurarBackup;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label lblInfo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblDetalleCaption = new System.Windows.Forms.Label();
            this.txtDetalle = new System.Windows.Forms.TextBox();
            this.btnAceptarCambios = new System.Windows.Forms.Button();
            this.btnRestaurarBackup = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(15, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(258, 20);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Inconsistencias de integridad (DV/DVV)";
            // 
            // lblDetalleCaption
            // 
            this.lblDetalleCaption.AutoSize = true;
            this.lblDetalleCaption.Location = new System.Drawing.Point(16, 45);
            this.lblDetalleCaption.Name = "lblDetalleCaption";
            this.lblDetalleCaption.Size = new System.Drawing.Size(180, 13);
            this.lblDetalleCaption.TabIndex = 1;
            this.lblDetalleCaption.Text = "Detalle de las inconsistencias detectadas:";
            // 
            // txtDetalle
            // 
            this.txtDetalle.Location = new System.Drawing.Point(19, 61);
            this.txtDetalle.Multiline = true;
            this.txtDetalle.Name = "txtDetalle";
            this.txtDetalle.ReadOnly = true;
            this.txtDetalle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetalle.Size = new System.Drawing.Size(560, 150);
            this.txtDetalle.TabIndex = 2;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblInfo.Location = new System.Drawing.Point(16, 217);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(475, 26);
            this.lblInfo.TabIndex = 3;
            this.lblInfo.Text = "Opción 1: Aceptar los datos actuales y recalcular DV/DVV.\r\nOpción 2: Restaurar la base desde un backup existente (selección de archivo .bak).";
            // 
            // btnAceptarCambios
            // 
            this.btnAceptarCambios.Location = new System.Drawing.Point(244, 255);
            this.btnAceptarCambios.Name = "btnAceptarCambios";
            this.btnAceptarCambios.Size = new System.Drawing.Size(115, 32);
            this.btnAceptarCambios.TabIndex = 4;
            this.btnAceptarCambios.Text = "Aceptar cambios";
            this.btnAceptarCambios.UseVisualStyleBackColor = true;
            this.btnAceptarCambios.Click += new System.EventHandler(this.btnAceptarCambios_Click);
            // 
            // btnRestaurarBackup
            // 
            this.btnRestaurarBackup.Location = new System.Drawing.Point(365, 255);
            this.btnRestaurarBackup.Name = "btnRestaurarBackup";
            this.btnRestaurarBackup.Size = new System.Drawing.Size(130, 32);
            this.btnRestaurarBackup.TabIndex = 5;
            this.btnRestaurarBackup.Text = "Restaurar backup...";
            this.btnRestaurarBackup.UseVisualStyleBackColor = true;
            this.btnRestaurarBackup.Click += new System.EventHandler(this.btnRestaurarBackup_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(501, 255);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(78, 32);
            this.btnCancelar.TabIndex = 6;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // FrmIntegridadDv
            // 
            this.AcceptButton = this.btnAceptarCambios;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(600, 305);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnRestaurarBackup);
            this.Controls.Add(this.btnAceptarCambios);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.txtDetalle);
            this.Controls.Add(this.lblDetalleCaption);
            this.Controls.Add(this.lblTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmIntegridadDv";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Integridad de datos";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}