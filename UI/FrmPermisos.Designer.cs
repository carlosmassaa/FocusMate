namespace UI
{
    partial class FrmPermisos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox comboBoxSeleccionUsuario;
        private System.Windows.Forms.TreeView treeViewPermisos;
        private System.Windows.Forms.Label lblUsuario;

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
            this.comboBoxSeleccionUsuario = new System.Windows.Forms.ComboBox();
            this.treeViewPermisos = new System.Windows.Forms.TreeView();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxSeleccionUsuario
            // 
            this.comboBoxSeleccionUsuario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSeleccionUsuario.FormattingEnabled = true;
            this.comboBoxSeleccionUsuario.Location = new System.Drawing.Point(84, 12);
            this.comboBoxSeleccionUsuario.Name = "comboBoxSeleccionUsuario";
            this.comboBoxSeleccionUsuario.Size = new System.Drawing.Size(288, 21);
            this.comboBoxSeleccionUsuario.TabIndex = 0;
            this.comboBoxSeleccionUsuario.SelectedIndexChanged += new System.EventHandler(this.comboBoxSeleccionUsuario_SelectedIndexChanged);
            // 
            // treeViewPermisos
            // 
            this.treeViewPermisos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewPermisos.Location = new System.Drawing.Point(12, 44);
            this.treeViewPermisos.Name = "treeViewPermisos";
            this.treeViewPermisos.Size = new System.Drawing.Size(560, 405);
            this.treeViewPermisos.TabIndex = 1;
            this.treeViewPermisos.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewPermisos_AfterSelect);
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(12, 15);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblUsuario.TabIndex = 2;
            this.lblUsuario.Text = "Usuario:";
            // 
            // FrmPermisos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.treeViewPermisos);
            this.Controls.Add(this.comboBoxSeleccionUsuario);
            this.Name = "FrmPermisos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Permisos del Usuario";
            this.Load += new System.EventHandler(this.FrmPermisos_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}