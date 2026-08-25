namespace UI
{
    partial class AgregarIdioma
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
            this.buttonCancelar = new System.Windows.Forms.Button();
            this.ButtonGuardar = new System.Windows.Forms.Button();
            this.labelAgregarIdioma = new System.Windows.Forms.Label();
            this.textBoxIdioma = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonCancelar
            // 
            this.buttonCancelar.Location = new System.Drawing.Point(175, 107);
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.Size = new System.Drawing.Size(136, 23);
            this.buttonCancelar.TabIndex = 6;
            this.buttonCancelar.Text = "Cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            this.buttonCancelar.Click += new System.EventHandler(this.ButtonCancelar_Click);
            // 
            // ButtonGuardar
            // 
            this.ButtonGuardar.Location = new System.Drawing.Point(15, 107);
            this.ButtonGuardar.Name = "ButtonGuardar";
            this.ButtonGuardar.Size = new System.Drawing.Size(136, 23);
            this.ButtonGuardar.TabIndex = 5;
            this.ButtonGuardar.Text = "Guardar";
            this.ButtonGuardar.UseVisualStyleBackColor = true;
            this.ButtonGuardar.Click += new System.EventHandler(this.ButtonGuardar_Click);
            // 
            // labelAgregarIdioma
            // 
            this.labelAgregarIdioma.AutoSize = true;
            this.labelAgregarIdioma.Location = new System.Drawing.Point(26, 48);
            this.labelAgregarIdioma.Name = "labelAgregarIdioma";
            this.labelAgregarIdioma.Size = new System.Drawing.Size(81, 13);
            this.labelAgregarIdioma.TabIndex = 4;
            this.labelAgregarIdioma.Text = "Agregar Idioma:";
            // 
            // textBoxIdioma
            // 
            this.textBoxIdioma.Location = new System.Drawing.Point(175, 41);
            this.textBoxIdioma.Name = "textBoxIdioma";
            this.textBoxIdioma.Size = new System.Drawing.Size(136, 20);
            this.textBoxIdioma.TabIndex = 7;
            // 
            // AgregarIdioma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 178);
            this.Controls.Add(this.textBoxIdioma);
            this.Controls.Add(this.buttonCancelar);
            this.Controls.Add(this.ButtonGuardar);
            this.Controls.Add(this.labelAgregarIdioma);
            this.Name = "AgregarIdioma";
            this.Text = "AgregarIdioma";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancelar;
        private System.Windows.Forms.Button ButtonGuardar;
        private System.Windows.Forms.Label labelAgregarIdioma;
        private System.Windows.Forms.TextBox textBoxIdioma;
    }
}