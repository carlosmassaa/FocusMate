namespace UI
{
    partial class ListadoTareas
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
            this.dataGridViewTareas = new System.Windows.Forms.DataGridView();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnHistorial = new System.Windows.Forms.Button();
            this.buttonOrdenarPorScore = new System.Windows.Forms.Button();
            this.comboBoxSeleccionarUsuarios = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTareas)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewTareas
            // 
            this.dataGridViewTareas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTareas.Location = new System.Drawing.Point(12, 53);
            this.dataGridViewTareas.Name = "dataGridViewTareas";
            this.dataGridViewTareas.Size = new System.Drawing.Size(760, 439);
            this.dataGridViewTareas.TabIndex = 0;
            this.dataGridViewTareas.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTareas_CellContentClick);
            // 
            // btnEditar
            // 
            this.btnEditar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditar.Location = new System.Drawing.Point(12, 505);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(110, 28);
            this.btnEditar.TabIndex = 1;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEliminar.Location = new System.Drawing.Point(128, 505);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(110, 28);
            this.btnEliminar.TabIndex = 2;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // btnHistorial
            // 
            this.btnHistorial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHistorial.Location = new System.Drawing.Point(662, 505);
            this.btnHistorial.Name = "btnHistorial";
            this.btnHistorial.Size = new System.Drawing.Size(110, 28);
            this.btnHistorial.TabIndex = 3;
            this.btnHistorial.Text = "Historial";
            this.btnHistorial.UseVisualStyleBackColor = true;
            this.btnHistorial.Click += new System.EventHandler(this.btnHistorial_Click);
            // 
            // buttonOrdenarPorScore
            // 
            this.buttonOrdenarPorScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOrdenarPorScore.Location = new System.Drawing.Point(244, 505);
            this.buttonOrdenarPorScore.Name = "buttonOrdenarPorScore";
            this.buttonOrdenarPorScore.Size = new System.Drawing.Size(140, 28);
            this.buttonOrdenarPorScore.TabIndex = 4;
            this.buttonOrdenarPorScore.Text = "Ordenar por Score";
            this.buttonOrdenarPorScore.UseVisualStyleBackColor = true;
            this.buttonOrdenarPorScore.Click += new System.EventHandler(this.buttonOrdenarPorScore_Click);
            // 
            // comboBoxSeleccionarUsuarios
            // 
            this.comboBoxSeleccionarUsuarios.FormattingEnabled = true;
            this.comboBoxSeleccionarUsuarios.Location = new System.Drawing.Point(12, 12);
            this.comboBoxSeleccionarUsuarios.Name = "comboBoxSeleccionarUsuarios";
            this.comboBoxSeleccionarUsuarios.Size = new System.Drawing.Size(760, 21);
            this.comboBoxSeleccionarUsuarios.TabIndex = 5;
            this.comboBoxSeleccionarUsuarios.SelectedIndexChanged += new System.EventHandler(this.comboBoxSeleccionarUsuarios_SelectedIndexChanged_1);
            // 
            // ListadoTareas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 541);
            this.Controls.Add(this.comboBoxSeleccionarUsuarios);
            this.Controls.Add(this.buttonOrdenarPorScore);
            this.Controls.Add(this.btnHistorial);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.dataGridViewTareas);
            this.Name = "ListadoTareas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Listado de Tareas";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTareas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewTareas;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnHistorial;
        private System.Windows.Forms.Button buttonOrdenarPorScore;
        private System.Windows.Forms.ComboBox comboBoxSeleccionarUsuarios;
    }
}