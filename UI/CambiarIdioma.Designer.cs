namespace UI
{
    partial class CambiarIdioma
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.labelSeleccioneIdioma = new System.Windows.Forms.Label();
            this.ButtonGuardar = new System.Windows.Forms.Button();
            this.buttonCancelar = new System.Windows.Forms.Button();
            this.buttonAgregarIdioma = new System.Windows.Forms.Button();
            this.checkBoxDefaultIdioma = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(187, 51);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(136, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // labelSeleccioneIdioma
            // 
            this.labelSeleccioneIdioma.AutoSize = true;
            this.labelSeleccioneIdioma.Location = new System.Drawing.Point(38, 54);
            this.labelSeleccioneIdioma.Name = "labelSeleccioneIdioma";
            this.labelSeleccioneIdioma.Size = new System.Drawing.Size(112, 13);
            this.labelSeleccioneIdioma.TabIndex = 1;
            this.labelSeleccioneIdioma.Text = "Seleccione un Idioma:";
            // 
            // ButtonGuardar
            // 
            this.ButtonGuardar.Location = new System.Drawing.Point(27, 147);
            this.ButtonGuardar.Name = "ButtonGuardar";
            this.ButtonGuardar.Size = new System.Drawing.Size(136, 23);
            this.ButtonGuardar.TabIndex = 3;
            this.ButtonGuardar.Text = "Guardar";
            this.ButtonGuardar.UseVisualStyleBackColor = true;
            this.ButtonGuardar.Click += new System.EventHandler(this.ButtonGuardar_Click);
            // 
            // buttonCancelar
            // 
            this.buttonCancelar.Location = new System.Drawing.Point(187, 147);
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.Size = new System.Drawing.Size(136, 23);
            this.buttonCancelar.TabIndex = 4;
            this.buttonCancelar.Text = "Cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            this.buttonCancelar.Click += new System.EventHandler(this.buttonCancelar_Click);
            // 
            // buttonAgregarIdioma
            // 
            this.buttonAgregarIdioma.Location = new System.Drawing.Point(27, 188);
            this.buttonAgregarIdioma.Name = "buttonAgregarIdioma";
            this.buttonAgregarIdioma.Size = new System.Drawing.Size(296, 23);
            this.buttonAgregarIdioma.TabIndex = 5;
            this.buttonAgregarIdioma.Text = "Agregar Idioma";
            this.buttonAgregarIdioma.UseVisualStyleBackColor = true;
            this.buttonAgregarIdioma.Click += new System.EventHandler(this.buttonAgregarIdioma_Click_2);
            // 
            // checkBoxDefaultIdioma
            // 
            this.checkBoxDefaultIdioma.AutoSize = true;
            this.checkBoxDefaultIdioma.Location = new System.Drawing.Point(41, 98);
            this.checkBoxDefaultIdioma.Name = "checkBoxDefaultIdioma";
            this.checkBoxDefaultIdioma.Size = new System.Drawing.Size(167, 17);
            this.checkBoxDefaultIdioma.TabIndex = 2;
            this.checkBoxDefaultIdioma.Text = "Usar como idioma por defecto";
            this.checkBoxDefaultIdioma.UseVisualStyleBackColor = true;
            // 
            // CambiarIdioma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 233);
            this.Controls.Add(this.checkBoxDefaultIdioma);
            this.Controls.Add(this.buttonAgregarIdioma);
            this.Controls.Add(this.buttonCancelar);
            this.Controls.Add(this.ButtonGuardar);
            this.Controls.Add(this.labelSeleccioneIdioma);
            this.Controls.Add(this.comboBox1);
            this.Name = "CambiarIdioma";
            this.Text = "CambiarIdioma";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label labelSeleccioneIdioma;
        private System.Windows.Forms.Button ButtonGuardar;
        private System.Windows.Forms.Button buttonCancelar;
        private System.Windows.Forms.Button buttonAgregarIdioma;
        private System.Windows.Forms.CheckBox checkBoxDefaultIdioma;
    }
}