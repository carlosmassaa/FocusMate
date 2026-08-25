namespace UI
{
    partial class GenerarPlanificacion
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
            this.comboBoxSeleccionarUsuarios = new System.Windows.Forms.ComboBox();
            this.dataGridViewDetallePlanificacion = new System.Windows.Forms.DataGridView();
            this.btnGenerarPlanificacion = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetallePlanificacion)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(12, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(155, 13);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Generar planificación priorizada";
            this.lblTitulo.Click += new System.EventHandler(this.lblTitulo_Click);
            // 
            // comboBoxSeleccionarUsuarios
            // 
            this.comboBoxSeleccionarUsuarios.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSeleccionarUsuarios.FormattingEnabled = true;
            this.comboBoxSeleccionarUsuarios.Location = new System.Drawing.Point(15, 42);
            this.comboBoxSeleccionarUsuarios.Name = "comboBoxSeleccionarUsuarios";
            this.comboBoxSeleccionarUsuarios.Size = new System.Drawing.Size(757, 21);
            this.comboBoxSeleccionarUsuarios.TabIndex = 1;
            this.comboBoxSeleccionarUsuarios.SelectedIndexChanged += new System.EventHandler(this.comboBoxSeleccionarUsuarios_SelectedIndexChanged);
            // 
            // dataGridViewDetallePlanificacion
            // 
            this.dataGridViewDetallePlanificacion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewDetallePlanificacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDetallePlanificacion.Location = new System.Drawing.Point(15, 80);
            this.dataGridViewDetallePlanificacion.Name = "dataGridViewDetallePlanificacion";
            this.dataGridViewDetallePlanificacion.Size = new System.Drawing.Size(757, 320);
            this.dataGridViewDetallePlanificacion.TabIndex = 2;
            this.dataGridViewDetallePlanificacion.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDetallePlanificacion_CellContentClick);
            // 
            // btnGenerarPlanificacion
            // 
            this.btnGenerarPlanificacion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGenerarPlanificacion.Location = new System.Drawing.Point(15, 416);
            this.btnGenerarPlanificacion.Name = "btnGenerarPlanificacion";
            this.btnGenerarPlanificacion.Size = new System.Drawing.Size(160, 28);
            this.btnGenerarPlanificacion.TabIndex = 3;
            this.btnGenerarPlanificacion.Text = "Generar planificación";
            this.btnGenerarPlanificacion.UseVisualStyleBackColor = true;
            this.btnGenerarPlanificacion.Click += new System.EventHandler(this.btnGenerarPlanificacion_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Location = new System.Drawing.Point(662, 416);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(110, 28);
            this.btnCerrar.TabIndex = 4;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // GenerarPlanificacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnGenerarPlanificacion);
            this.Controls.Add(this.dataGridViewDetallePlanificacion);
            this.Controls.Add(this.comboBoxSeleccionarUsuarios);
            this.Controls.Add(this.lblTitulo);
            this.Name = "GenerarPlanificacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Generar Planificación";
            this.Load += new System.EventHandler(this.GenerarPlanificacion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetallePlanificacion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.ComboBox comboBoxSeleccionarUsuarios;
        private System.Windows.Forms.DataGridView dataGridViewDetallePlanificacion;
        private System.Windows.Forms.Button btnGenerarPlanificacion;
        private System.Windows.Forms.Button btnCerrar;
    }
}