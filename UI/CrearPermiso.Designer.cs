namespace UI
{
    partial class CrearPermiso
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxAgregarPermiso = new System.Windows.Forms.TextBox();
            this.labelAgregarPermiso = new System.Windows.Forms.Label();
            this.buttonAgregar = new System.Windows.Forms.Button();
            this.buttonCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxAgregarPermiso
            // 
            this.textBoxAgregarPermiso.Location = new System.Drawing.Point(104, 12);
            this.textBoxAgregarPermiso.Name = "textBoxAgregarPermiso";
            this.textBoxAgregarPermiso.Size = new System.Drawing.Size(163, 20);
            this.textBoxAgregarPermiso.TabIndex = 0;
            // 
            // labelAgregarPermiso
            // 
            this.labelAgregarPermiso.AutoSize = true;
            this.labelAgregarPermiso.Location = new System.Drawing.Point(12, 15);
            this.labelAgregarPermiso.Name = "labelAgregarPermiso";
            this.labelAgregarPermiso.Size = new System.Drawing.Size(86, 13);
            this.labelAgregarPermiso.TabIndex = 1;
            this.labelAgregarPermiso.Text = "Agregar permiso:";
            // 
            // buttonAgregar
            // 
            this.buttonAgregar.Location = new System.Drawing.Point(15, 38);
            this.buttonAgregar.Name = "buttonAgregar";
            this.buttonAgregar.Size = new System.Drawing.Size(118, 23);
            this.buttonAgregar.TabIndex = 2;
            this.buttonAgregar.Text = "Agregar";
            this.buttonAgregar.UseVisualStyleBackColor = true;
            this.buttonAgregar.Click += new System.EventHandler(this.buttonAgregar_Click_1);
            // 
            // buttonCancelar
            // 
            this.buttonCancelar.Location = new System.Drawing.Point(150, 38);
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.Size = new System.Drawing.Size(117, 23);
            this.buttonCancelar.TabIndex = 3;
            this.buttonCancelar.Text = "Cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            // 
            // CrearPermiso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 71);
            this.Controls.Add(this.buttonCancelar);
            this.Controls.Add(this.buttonAgregar);
            this.Controls.Add(this.labelAgregarPermiso);
            this.Controls.Add(this.textBoxAgregarPermiso);
            this.Name = "CrearPermiso";
            this.Text = "CrearPermiso";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxAgregarPermiso;
        private System.Windows.Forms.Label labelAgregarPermiso;
        private System.Windows.Forms.Button buttonAgregar;
        private System.Windows.Forms.Button buttonCancelar;
    }
}