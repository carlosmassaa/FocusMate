namespace UI
{
    partial class HistorialListadoTarea
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
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
            this.dataGridViewHistorialTarea = new System.Windows.Forms.DataGridView();
            this.lblHistoriaTarea = new System.Windows.Forms.Label();
            this.btnRevertirSeleccion = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistorialTarea)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewHistorialTarea
            // 
            this.dataGridViewHistorialTarea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewHistorialTarea.Location = new System.Drawing.Point(12, 58);
            this.dataGridViewHistorialTarea.Name = "dataGridViewHistorialTarea";
            this.dataGridViewHistorialTarea.Size = new System.Drawing.Size(1205, 380);
            this.dataGridViewHistorialTarea.TabIndex = 0;
            this.dataGridViewHistorialTarea.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewHistorialTarea_CellContentClick);
            // 
            // lblHistoriaTarea
            // 
            this.lblHistoriaTarea.AutoSize = true;
            this.lblHistoriaTarea.Location = new System.Drawing.Point(12, 19);
            this.lblHistoriaTarea.Name = "lblHistoriaTarea";
            this.lblHistoriaTarea.Size = new System.Drawing.Size(101, 13);
            this.lblHistoriaTarea.TabIndex = 1;
            this.lblHistoriaTarea.Text = "Historial de la Tarea";
            // 
            // btnRevertirSeleccion
            // 
            this.btnRevertirSeleccion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRevertirSeleccion.Location = new System.Drawing.Point(1097, 12);
            this.btnRevertirSeleccion.Name = "btnRevertirSeleccion";
            this.btnRevertirSeleccion.Size = new System.Drawing.Size(120, 30);
            this.btnRevertirSeleccion.TabIndex = 2;
            this.btnRevertirSeleccion.Text = "Recomponer";
            this.btnRevertirSeleccion.UseVisualStyleBackColor = true;
            this.btnRevertirSeleccion.Click += new System.EventHandler(this.btnRevertirSeleccion_Click);
            // 
            // HistorialListadoTarea
            // 
            this.ClientSize = new System.Drawing.Size(1229, 450);
            this.Controls.Add(this.btnRevertirSeleccion);
            this.Controls.Add(this.lblHistoriaTarea);
            this.Controls.Add(this.dataGridViewHistorialTarea);
            this.MinimumSize = new System.Drawing.Size(700, 400);
            this.Name = "HistorialListadoTarea";
            this.Text = "Historial";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistorialTarea)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewHistorialTarea;
        private System.Windows.Forms.Label lblHistoriaTarea;
        private System.Windows.Forms.Button btnRevertirSeleccion;
    }
}