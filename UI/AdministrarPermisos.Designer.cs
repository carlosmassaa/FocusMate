namespace UI
{
    partial class AdministrarPermisos
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
            this.labelAdministrarPermisos = new System.Windows.Forms.Label();
            this.dataGridViewUsuarios = new System.Windows.Forms.DataGridView();
            this.labelUsuariosTitulo = new System.Windows.Forms.Label();
            this.dataGridViewPermiossUsuario = new System.Windows.Forms.DataGridView();
            this.LabelPermisosUsuario = new System.Windows.Forms.Label();
            this.dataGridViewPermisos = new System.Windows.Forms.DataGridView();
            this.labelPermisos = new System.Windows.Forms.Label();
            this.buttonAgregarPermiso = new System.Windows.Forms.Button();
            this.buttonEliminarPermiso = new System.Windows.Forms.Button();
            this.labelPermisosPorFamilia = new System.Windows.Forms.Label();
            this.dataGridViewPermisosPorFamilia = new System.Windows.Forms.DataGridView();
            this.buttonNuevoPermiso = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsuarios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPermiossUsuario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPermisos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPermisosPorFamilia)).BeginInit();
            this.SuspendLayout();
            // 
            // labelAdministrarPermisos
            // 
            this.labelAdministrarPermisos.AutoSize = true;
            this.labelAdministrarPermisos.Location = new System.Drawing.Point(604, 32);
            this.labelAdministrarPermisos.Name = "labelAdministrarPermisos";
            this.labelAdministrarPermisos.Size = new System.Drawing.Size(103, 13);
            this.labelAdministrarPermisos.TabIndex = 0;
            this.labelAdministrarPermisos.Text = "Administrar Permisos";
            // 
            // dataGridViewUsuarios
            // 
            this.dataGridViewUsuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUsuarios.Location = new System.Drawing.Point(23, 109);
            this.dataGridViewUsuarios.MultiSelect = false;
            this.dataGridViewUsuarios.Name = "dataGridViewUsuarios";
            this.dataGridViewUsuarios.ReadOnly = true;
            this.dataGridViewUsuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewUsuarios.Size = new System.Drawing.Size(542, 244);
            this.dataGridViewUsuarios.TabIndex = 1;
            // 
            // labelUsuariosTitulo
            // 
            this.labelUsuariosTitulo.AutoSize = true;
            this.labelUsuariosTitulo.Location = new System.Drawing.Point(277, 82);
            this.labelUsuariosTitulo.Name = "labelUsuariosTitulo";
            this.labelUsuariosTitulo.Size = new System.Drawing.Size(48, 13);
            this.labelUsuariosTitulo.TabIndex = 2;
            this.labelUsuariosTitulo.Text = "Usuarios";
            // 
            // dataGridViewPermiossUsuario
            // 
            this.dataGridViewPermiossUsuario.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPermiossUsuario.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPermiossUsuario.Location = new System.Drawing.Point(656, 109);
            this.dataGridViewPermiossUsuario.MultiSelect = false;
            this.dataGridViewPermiossUsuario.Name = "dataGridViewPermiossUsuario";
            this.dataGridViewPermiossUsuario.ReadOnly = true;
            this.dataGridViewPermiossUsuario.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPermiossUsuario.Size = new System.Drawing.Size(264, 244);
            this.dataGridViewPermiossUsuario.TabIndex = 3;
            // 
            // LabelPermisosUsuario
            // 
            this.LabelPermisosUsuario.AutoSize = true;
            this.LabelPermisosUsuario.Location = new System.Drawing.Point(745, 82);
            this.LabelPermisosUsuario.Name = "LabelPermisosUsuario";
            this.LabelPermisosUsuario.Size = new System.Drawing.Size(93, 13);
            this.LabelPermisosUsuario.TabIndex = 4;
            this.LabelPermisosUsuario.Text = "Permisos Usuarios";
            // 
            // dataGridViewPermisos
            // 
            this.dataGridViewPermisos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPermisos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPermisos.Location = new System.Drawing.Point(1117, 109);
            this.dataGridViewPermisos.MultiSelect = false;
            this.dataGridViewPermisos.Name = "dataGridViewPermisos";
            this.dataGridViewPermisos.ReadOnly = true;
            this.dataGridViewPermisos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPermisos.Size = new System.Drawing.Size(264, 244);
            this.dataGridViewPermisos.TabIndex = 5;
            // 
            // labelPermisos
            // 
            this.labelPermisos.AutoSize = true;
            this.labelPermisos.Location = new System.Drawing.Point(1224, 82);
            this.labelPermisos.Name = "labelPermisos";
            this.labelPermisos.Size = new System.Drawing.Size(49, 13);
            this.labelPermisos.TabIndex = 6;
            this.labelPermisos.Text = "Permisos";
            // 
            // buttonAgregarPermiso
            // 
            this.buttonAgregarPermiso.Location = new System.Drawing.Point(976, 150);
            this.buttonAgregarPermiso.Name = "buttonAgregarPermiso";
            this.buttonAgregarPermiso.Size = new System.Drawing.Size(97, 29);
            this.buttonAgregarPermiso.TabIndex = 7;
            this.buttonAgregarPermiso.Text = "<<< Agregar";
            this.buttonAgregarPermiso.UseVisualStyleBackColor = true;
            this.buttonAgregarPermiso.Click += new System.EventHandler(this.buttonAgregarPermiso_Click);
            // 
            // buttonEliminarPermiso
            // 
            this.buttonEliminarPermiso.Location = new System.Drawing.Point(976, 234);
            this.buttonEliminarPermiso.Name = "buttonEliminarPermiso";
            this.buttonEliminarPermiso.Size = new System.Drawing.Size(97, 29);
            this.buttonEliminarPermiso.TabIndex = 8;
            this.buttonEliminarPermiso.Text = "Eliminar >>>";
            this.buttonEliminarPermiso.UseVisualStyleBackColor = true;
            this.buttonEliminarPermiso.Click += new System.EventHandler(this.buttonEliminarPermiso_Click);
            // 
            // labelPermisosPorFamilia
            // 
            this.labelPermisosPorFamilia.AutoSize = true;
            this.labelPermisosPorFamilia.Location = new System.Drawing.Point(1482, 82);
            this.labelPermisosPorFamilia.Name = "labelPermisosPorFamilia";
            this.labelPermisosPorFamilia.Size = new System.Drawing.Size(107, 13);
            this.labelPermisosPorFamilia.TabIndex = 9;
            this.labelPermisosPorFamilia.Text = "Permisos de la familia";
            // 
            // dataGridViewPermisosPorFamilia
            // 
            this.dataGridViewPermisosPorFamilia.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPermisosPorFamilia.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPermisosPorFamilia.Location = new System.Drawing.Point(1404, 109);
            this.dataGridViewPermisosPorFamilia.MultiSelect = false;
            this.dataGridViewPermisosPorFamilia.Name = "dataGridViewPermisosPorFamilia";
            this.dataGridViewPermisosPorFamilia.ReadOnly = true;
            this.dataGridViewPermisosPorFamilia.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPermisosPorFamilia.Size = new System.Drawing.Size(264, 244);
            this.dataGridViewPermisosPorFamilia.TabIndex = 10;
            // 
            // buttonNuevoPermiso
            // 
            this.buttonNuevoPermiso.Location = new System.Drawing.Point(1117, 359);
            this.buttonNuevoPermiso.Name = "buttonNuevoPermiso";
            this.buttonNuevoPermiso.Size = new System.Drawing.Size(264, 23);
            this.buttonNuevoPermiso.TabIndex = 11;
            this.buttonNuevoPermiso.Text = "Nuevo Permiso";
            this.buttonNuevoPermiso.UseVisualStyleBackColor = true;
            this.buttonNuevoPermiso.Click += new System.EventHandler(this.buttonNuevoPermiso_Click);
            // 
            // AdministrarPermisos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1693, 582);
            this.Controls.Add(this.buttonNuevoPermiso);
            this.Controls.Add(this.dataGridViewPermisosPorFamilia);
            this.Controls.Add(this.labelPermisosPorFamilia);
            this.Controls.Add(this.buttonEliminarPermiso);
            this.Controls.Add(this.buttonAgregarPermiso);
            this.Controls.Add(this.labelPermisos);
            this.Controls.Add(this.dataGridViewPermisos);
            this.Controls.Add(this.LabelPermisosUsuario);
            this.Controls.Add(this.dataGridViewPermiossUsuario);
            this.Controls.Add(this.labelUsuariosTitulo);
            this.Controls.Add(this.dataGridViewUsuarios);
            this.Controls.Add(this.labelAdministrarPermisos);
            this.Name = "AdministrarPermisos";
            this.Text = "AdministrarPermisos";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsuarios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPermiossUsuario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPermisos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPermisosPorFamilia)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAdministrarPermisos;
        private System.Windows.Forms.DataGridView dataGridViewUsuarios;
        private System.Windows.Forms.Label labelUsuariosTitulo;
        private System.Windows.Forms.DataGridView dataGridViewPermiossUsuario;
        private System.Windows.Forms.Label LabelPermisosUsuario;
        private System.Windows.Forms.DataGridView dataGridViewPermisos;
        private System.Windows.Forms.Label labelPermisos;
        private System.Windows.Forms.Button buttonAgregarPermiso;
        private System.Windows.Forms.Button buttonEliminarPermiso;
        private System.Windows.Forms.Label labelPermisosPorFamilia;
        private System.Windows.Forms.DataGridView dataGridViewPermisosPorFamilia;
        private System.Windows.Forms.Button buttonNuevoPermiso;
    }
}