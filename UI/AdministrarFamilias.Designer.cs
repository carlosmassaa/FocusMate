namespace UI
{
    partial class AdministrarFamilias
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
            this.LabelFamiliadePermisos = new System.Windows.Forms.Label();
            this.dataGridViewFamiliadePermisos = new System.Windows.Forms.DataGridView();
            this.buttonEliminarPermiso = new System.Windows.Forms.Button();
            this.buttonAgregarPermiso = new System.Windows.Forms.Button();
            this.labelPermisos = new System.Windows.Forms.Label();
            this.dataGridViewPermisos = new System.Windows.Forms.DataGridView();
            this.LabelPermisosUsuario = new System.Windows.Forms.Label();
            this.dataGridViewPermisosdeFamilia = new System.Windows.Forms.DataGridView();
            this.labelTituloAdministrarFamilia = new System.Windows.Forms.Label();
            this.buttonNuevoFamilia = new System.Windows.Forms.Button();
            this.buttonEliminarFamilia = new System.Windows.Forms.Button();
            this.buttonEditarFamilia = new System.Windows.Forms.Button();
            this.buttonNuevoPermiso = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFamiliadePermisos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPermisos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPermisosdeFamilia)).BeginInit();
            this.SuspendLayout();
            // 
            // LabelFamiliadePermisos
            // 
            this.LabelFamiliadePermisos.AutoSize = true;
            this.LabelFamiliadePermisos.Location = new System.Drawing.Point(171, 122);
            this.LabelFamiliadePermisos.Name = "LabelFamiliadePermisos";
            this.LabelFamiliadePermisos.Size = new System.Drawing.Size(103, 13);
            this.LabelFamiliadePermisos.TabIndex = 10;
            this.LabelFamiliadePermisos.Text = "Familias de permisos";
            // 
            // dataGridViewFamiliadePermisos
            // 
            this.dataGridViewFamiliadePermisos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewFamiliadePermisos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFamiliadePermisos.Location = new System.Drawing.Point(24, 149);
            this.dataGridViewFamiliadePermisos.MultiSelect = false;
            this.dataGridViewFamiliadePermisos.Name = "dataGridViewFamiliadePermisos";
            this.dataGridViewFamiliadePermisos.ReadOnly = true;
            this.dataGridViewFamiliadePermisos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewFamiliadePermisos.Size = new System.Drawing.Size(388, 244);
            this.dataGridViewFamiliadePermisos.TabIndex = 9;
            // 
            // buttonEliminarPermiso
            // 
            this.buttonEliminarPermiso.Location = new System.Drawing.Point(810, 272);
            this.buttonEliminarPermiso.Name = "buttonEliminarPermiso";
            this.buttonEliminarPermiso.Size = new System.Drawing.Size(97, 29);
            this.buttonEliminarPermiso.TabIndex = 16;
            this.buttonEliminarPermiso.Text = "Eliminar >>>";
            this.buttonEliminarPermiso.UseVisualStyleBackColor = true;
            this.buttonEliminarPermiso.Click += new System.EventHandler(this.buttonEliminarPermiso_Click);
            // 
            // buttonAgregarPermiso
            // 
            this.buttonAgregarPermiso.Location = new System.Drawing.Point(810, 188);
            this.buttonAgregarPermiso.Name = "buttonAgregarPermiso";
            this.buttonAgregarPermiso.Size = new System.Drawing.Size(97, 29);
            this.buttonAgregarPermiso.TabIndex = 15;
            this.buttonAgregarPermiso.Text = "<<< Agregar";
            this.buttonAgregarPermiso.UseVisualStyleBackColor = true;
            this.buttonAgregarPermiso.Click += new System.EventHandler(this.buttonAgregarPermiso_Click);
            // 
            // labelPermisos
            // 
            this.labelPermisos.AutoSize = true;
            this.labelPermisos.Location = new System.Drawing.Point(1057, 122);
            this.labelPermisos.Name = "labelPermisos";
            this.labelPermisos.Size = new System.Drawing.Size(49, 13);
            this.labelPermisos.TabIndex = 14;
            this.labelPermisos.Text = "Permisos";
            // 
            // dataGridViewPermisos
            // 
            this.dataGridViewPermisos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPermisos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPermisos.Location = new System.Drawing.Point(926, 149);
            this.dataGridViewPermisos.MultiSelect = false;
            this.dataGridViewPermisos.Name = "dataGridViewPermisos";
            this.dataGridViewPermisos.ReadOnly = true;
            this.dataGridViewPermisos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPermisos.Size = new System.Drawing.Size(311, 244);
            this.dataGridViewPermisos.TabIndex = 13;
            // 
            // LabelPermisosUsuario
            // 
            this.LabelPermisosUsuario.AutoSize = true;
            this.LabelPermisosUsuario.Location = new System.Drawing.Point(596, 122);
            this.LabelPermisosUsuario.Name = "LabelPermisosUsuario";
            this.LabelPermisosUsuario.Size = new System.Drawing.Size(84, 13);
            this.LabelPermisosUsuario.TabIndex = 12;
            this.LabelPermisosUsuario.Text = "Permisos Familia";
            // 
            // dataGridViewPermisosdeFamilia
            // 
            this.dataGridViewPermisosdeFamilia.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPermisosdeFamilia.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPermisosdeFamilia.Location = new System.Drawing.Point(482, 149);
            this.dataGridViewPermisosdeFamilia.MultiSelect = false;
            this.dataGridViewPermisosdeFamilia.Name = "dataGridViewPermisosdeFamilia";
            this.dataGridViewPermisosdeFamilia.ReadOnly = true;
            this.dataGridViewPermisosdeFamilia.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPermisosdeFamilia.Size = new System.Drawing.Size(311, 244);
            this.dataGridViewPermisosdeFamilia.TabIndex = 11;
            // 
            // labelTituloAdministrarFamilia
            // 
            this.labelTituloAdministrarFamilia.Location = new System.Drawing.Point(499, 29);
            this.labelTituloAdministrarFamilia.Name = "labelTituloAdministrarFamilia";
            this.labelTituloAdministrarFamilia.Size = new System.Drawing.Size(151, 16);
            this.labelTituloAdministrarFamilia.TabIndex = 0;
            this.labelTituloAdministrarFamilia.Text = "Administrar Famila de Permisos";
            // 
            // buttonNuevoFamilia
            // 
            this.buttonNuevoFamilia.Location = new System.Drawing.Point(24, 428);
            this.buttonNuevoFamilia.Name = "buttonNuevoFamilia";
            this.buttonNuevoFamilia.Size = new System.Drawing.Size(112, 23);
            this.buttonNuevoFamilia.TabIndex = 17;
            this.buttonNuevoFamilia.Text = "Agregar";
            this.buttonNuevoFamilia.UseVisualStyleBackColor = true;
            this.buttonNuevoFamilia.Click += new System.EventHandler(this.buttonNuevoFamilia_Click);
            // 
            // buttonEliminarFamilia
            // 
            this.buttonEliminarFamilia.Location = new System.Drawing.Point(300, 428);
            this.buttonEliminarFamilia.Name = "buttonEliminarFamilia";
            this.buttonEliminarFamilia.Size = new System.Drawing.Size(112, 23);
            this.buttonEliminarFamilia.TabIndex = 18;
            this.buttonEliminarFamilia.Text = "Eliminar";
            this.buttonEliminarFamilia.UseVisualStyleBackColor = true;
            this.buttonEliminarFamilia.Click += new System.EventHandler(this.buttonEliminarFamilia_Click);
            // 
            // buttonEditarFamilia
            // 
            this.buttonEditarFamilia.Location = new System.Drawing.Point(162, 428);
            this.buttonEditarFamilia.Name = "buttonEditarFamilia";
            this.buttonEditarFamilia.Size = new System.Drawing.Size(112, 23);
            this.buttonEditarFamilia.TabIndex = 19;
            this.buttonEditarFamilia.Text = "Editar";
            this.buttonEditarFamilia.UseVisualStyleBackColor = true;
            this.buttonEditarFamilia.Click += new System.EventHandler(this.buttonEditarFamilia_Click);
            // 
            // buttonNuevoPermiso
            // 
            this.buttonNuevoPermiso.Location = new System.Drawing.Point(926, 399);
            this.buttonNuevoPermiso.Name = "buttonNuevoPermiso";
            this.buttonNuevoPermiso.Size = new System.Drawing.Size(311, 23);
            this.buttonNuevoPermiso.TabIndex = 20;
            this.buttonNuevoPermiso.Text = "Nuevo Permiso";
            this.buttonNuevoPermiso.UseVisualStyleBackColor = true;
            this.buttonNuevoPermiso.Click += new System.EventHandler(this.buttonNuevoPermiso_Click);
            // 
            // AdministrarFamilias
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1291, 530);
            this.Controls.Add(this.buttonNuevoPermiso);
            this.Controls.Add(this.buttonEditarFamilia);
            this.Controls.Add(this.buttonEliminarFamilia);
            this.Controls.Add(this.buttonNuevoFamilia);
            this.Controls.Add(this.labelTituloAdministrarFamilia);
            this.Controls.Add(this.buttonEliminarPermiso);
            this.Controls.Add(this.buttonAgregarPermiso);
            this.Controls.Add(this.labelPermisos);
            this.Controls.Add(this.dataGridViewPermisos);
            this.Controls.Add(this.LabelPermisosUsuario);
            this.Controls.Add(this.dataGridViewPermisosdeFamilia);
            this.Controls.Add(this.LabelFamiliadePermisos);
            this.Controls.Add(this.dataGridViewFamiliadePermisos);
            this.Name = "AdministrarFamilias";
            this.Text = "AdministrarFamilias";
            this.Load += new System.EventHandler(this.AdministrarFamilias_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFamiliadePermisos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPermisos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPermisosdeFamilia)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label LabelFamiliadePermisos;
        private System.Windows.Forms.DataGridView dataGridViewFamiliadePermisos;
        private System.Windows.Forms.Button buttonEliminarPermiso;
        private System.Windows.Forms.Button buttonAgregarPermiso;
        private System.Windows.Forms.Label labelPermisos;
        private System.Windows.Forms.DataGridView dataGridViewPermisos;
        private System.Windows.Forms.Label LabelPermisosUsuario;
        private System.Windows.Forms.DataGridView dataGridViewPermisosdeFamilia;
        private System.Windows.Forms.Label labelTituloAdministrarFamilia;
        private System.Windows.Forms.Button buttonNuevoFamilia;
        private System.Windows.Forms.Button buttonEliminarFamilia;
        private System.Windows.Forms.Button buttonEditarFamilia;
        private System.Windows.Forms.Button buttonNuevoPermiso;
    }
}