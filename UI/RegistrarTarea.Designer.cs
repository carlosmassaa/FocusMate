namespace UI
{
    partial class RegistrarTarea
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
            this.lblEncabezado = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.lblFechaLimite = new System.Windows.Forms.Label();
            this.lblImportancia = new System.Windows.Forms.Label();
            this.lblEnergiaRequerida = new System.Windows.Forms.Label();
            this.lblDuracionEstimada = new System.Windows.Forms.Label();
            this.txtTitulo = new System.Windows.Forms.TextBox();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.comboBoxEnergiaRequerida = new System.Windows.Forms.ComboBox();
            this.numericUpDownDuracionEstimada = new System.Windows.Forms.NumericUpDown();
            this.comboBoxImportancia = new System.Windows.Forms.ComboBox();
            this.dateTimePickerFehcaLimite = new System.Windows.Forms.DateTimePicker();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.lblMensaje = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuracionEstimada)).BeginInit();
            this.SuspendLayout();
            // 
            // lblEncabezado
            // 
            this.lblEncabezado.AutoSize = true;
            this.lblEncabezado.Location = new System.Drawing.Point(343, 28);
            this.lblEncabezado.Name = "lblEncabezado";
            this.lblEncabezado.Size = new System.Drawing.Size(40, 13);
            this.lblEncabezado.TabIndex = 0;
            this.lblEncabezado.Text = "Tareas";
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(60, 87);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(36, 13);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Text = "Titulo:";
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(60, 133);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(66, 13);
            this.lblDescripcion.TabIndex = 2;
            this.lblDescripcion.Text = "Descripción:";
            // 
            // lblFechaLimite
            // 
            this.lblFechaLimite.AutoSize = true;
            this.lblFechaLimite.Location = new System.Drawing.Point(60, 274);
            this.lblFechaLimite.Name = "lblFechaLimite";
            this.lblFechaLimite.Size = new System.Drawing.Size(70, 13);
            this.lblFechaLimite.TabIndex = 3;
            this.lblFechaLimite.Text = "Fecha Limite:";
            // 
            // lblImportancia
            // 
            this.lblImportancia.AutoSize = true;
            this.lblImportancia.Location = new System.Drawing.Point(60, 340);
            this.lblImportancia.Name = "lblImportancia";
            this.lblImportancia.Size = new System.Drawing.Size(65, 13);
            this.lblImportancia.TabIndex = 4;
            this.lblImportancia.Text = "Importancia:";
            // 
            // lblEnergiaRequerida
            // 
            this.lblEnergiaRequerida.AutoSize = true;
            this.lblEnergiaRequerida.Location = new System.Drawing.Point(422, 87);
            this.lblEnergiaRequerida.Name = "lblEnergiaRequerida";
            this.lblEnergiaRequerida.Size = new System.Drawing.Size(100, 13);
            this.lblEnergiaRequerida.TabIndex = 5;
            this.lblEnergiaRequerida.Text = "Energía Requerida:";
            // 
            // lblDuracionEstimada
            // 
            this.lblDuracionEstimada.AutoSize = true;
            this.lblDuracionEstimada.Location = new System.Drawing.Point(422, 150);
            this.lblDuracionEstimada.Name = "lblDuracionEstimada";
            this.lblDuracionEstimada.Size = new System.Drawing.Size(99, 13);
            this.lblDuracionEstimada.TabIndex = 6;
            this.lblDuracionEstimada.Text = "Duracion Estimada:";
            // 
            // txtTitulo
            // 
            this.txtTitulo.Location = new System.Drawing.Point(155, 80);
            this.txtTitulo.Name = "txtTitulo";
            this.txtTitulo.Size = new System.Drawing.Size(100, 20);
            this.txtTitulo.TabIndex = 7;
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(155, 126);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(249, 124);
            this.txtDescripcion.TabIndex = 8;
            // 
            // comboBoxEnergiaRequerida
            // 
            this.comboBoxEnergiaRequerida.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEnergiaRequerida.FormattingEnabled = true;
            this.comboBoxEnergiaRequerida.Location = new System.Drawing.Point(549, 87);
            this.comboBoxEnergiaRequerida.Name = "comboBoxEnergiaRequerida";
            this.comboBoxEnergiaRequerida.Size = new System.Drawing.Size(121, 21);
            this.comboBoxEnergiaRequerida.TabIndex = 9;
            // 
            // numericUpDownDuracionEstimada
            // 
            this.numericUpDownDuracionEstimada.Location = new System.Drawing.Point(549, 148);
            this.numericUpDownDuracionEstimada.Name = "numericUpDownDuracionEstimada";
            this.numericUpDownDuracionEstimada.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownDuracionEstimada.TabIndex = 10;
            // 
            // comboBoxImportancia
            // 
            this.comboBoxImportancia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxImportancia.FormattingEnabled = true;
            this.comboBoxImportancia.Location = new System.Drawing.Point(155, 332);
            this.comboBoxImportancia.Name = "comboBoxImportancia";
            this.comboBoxImportancia.Size = new System.Drawing.Size(249, 21);
            this.comboBoxImportancia.TabIndex = 12;
            // 
            // dateTimePickerFehcaLimite
            // 
            this.dateTimePickerFehcaLimite.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerFehcaLimite.Location = new System.Drawing.Point(155, 268);
            this.dateTimePickerFehcaLimite.Name = "dateTimePickerFehcaLimite";
            this.dateTimePickerFehcaLimite.Size = new System.Drawing.Size(249, 20);
            this.dateTimePickerFehcaLimite.TabIndex = 13;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(576, 336);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(94, 21);
            this.btnCancelar.TabIndex = 14;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(428, 336);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGuardar.Size = new System.Drawing.Size(94, 21);
            this.btnGuardar.TabIndex = 15;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // lblMensaje
            // 
            this.lblMensaje.AutoSize = true;
            this.lblMensaje.Location = new System.Drawing.Point(60, 382);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(0, 13);
            this.lblMensaje.TabIndex = 16;
            // 
            // RegistrarTarea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblMensaje);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.dateTimePickerFehcaLimite);
            this.Controls.Add(this.comboBoxImportancia);
            this.Controls.Add(this.numericUpDownDuracionEstimada);
            this.Controls.Add(this.comboBoxEnergiaRequerida);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.txtTitulo);
            this.Controls.Add(this.lblDuracionEstimada);
            this.Controls.Add(this.lblEnergiaRequerida);
            this.Controls.Add(this.lblImportancia);
            this.Controls.Add(this.lblFechaLimite);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblEncabezado);
            this.Name = "RegistrarTarea";
            this.Text = "RegistrarTarea";
            this.Load += new System.EventHandler(this.RegistrarTarea_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuracionEstimada)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEncabezado;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.Label lblFechaLimite;
        private System.Windows.Forms.Label lblImportancia;
        private System.Windows.Forms.Label lblEnergiaRequerida;
        private System.Windows.Forms.Label lblDuracionEstimada;
        private System.Windows.Forms.TextBox txtTitulo;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.ComboBox comboBoxEnergiaRequerida;
        private System.Windows.Forms.NumericUpDown numericUpDownDuracionEstimada;
        private System.Windows.Forms.ComboBox comboBoxImportancia;
        private System.Windows.Forms.DateTimePicker dateTimePickerFehcaLimite;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Label lblMensaje;
    }
}