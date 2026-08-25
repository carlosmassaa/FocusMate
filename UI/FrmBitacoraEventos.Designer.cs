namespace UI
{
    partial class FrmBitacoraEventos
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
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnExportarCsv = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblDesde = new System.Windows.Forms.Label();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.chkDesde = new System.Windows.Forms.CheckBox();
            this.chkHasta = new System.Windows.Forms.CheckBox();
            this.lblModulo = new System.Windows.Forms.Label();
            this.txtModulo = new System.Windows.Forms.TextBox();
            this.lblAccion = new System.Windows.Forms.Label();
            this.txtAccion = new System.Windows.Forms.TextBox();
            this.lblResultado = new System.Windows.Forms.Label();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.lblTextoLibre = new System.Windows.Forms.Label();
            this.txtTextoLibre = new System.Windows.Forms.TextBox();
            this.lblEntidad = new System.Windows.Forms.Label();
            this.txtEntidad = new System.Windows.Forms.TextBox();
            this.lblUsuarioId = new System.Windows.Forms.Label();
            this.txtUsuarioId = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(12, 12);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(120, 26);
            this.btnBuscar.TabIndex = 0;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(138, 12);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(120, 26);
            this.btnLimpiar.TabIndex = 1;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnExportarCsv
            // 
            this.btnExportarCsv.Location = new System.Drawing.Point(264, 12);
            this.btnExportarCsv.Name = "btnExportarCsv";
            this.btnExportarCsv.Size = new System.Drawing.Size(120, 26);
            this.btnExportarCsv.TabIndex = 2;
            this.btnExportarCsv.Text = "Exportar CSV";
            this.btnExportarCsv.UseVisualStyleBackColor = true;
            this.btnExportarCsv.Click += new System.EventHandler(this.btnExportarCsv_Click);
            // 
            // lblDesde
            // 
            this.lblDesde.Location = new System.Drawing.Point(12, 50);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.Size = new System.Drawing.Size(60, 20);
            this.lblDesde.TabIndex = 19;
            this.lblDesde.Text = "Desde:";
            // 
            // lblHasta
            // 
            this.lblHasta.Location = new System.Drawing.Point(12, 76);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.Size = new System.Drawing.Size(60, 20);
            this.lblHasta.TabIndex = 18;
            this.lblHasta.Text = "Hasta:";
            // 
            // dtpDesde
            // 
            this.dtpDesde.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDesde.Location = new System.Drawing.Point(78, 50);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(180, 20);
            this.dtpDesde.TabIndex = 3;
            // 
            // dtpHasta
            // 
            this.dtpHasta.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHasta.Location = new System.Drawing.Point(78, 76);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(180, 20);
            this.dtpHasta.TabIndex = 4;
            // 
            // chkDesde
            // 
            this.chkDesde.Location = new System.Drawing.Point(264, 50);
            this.chkDesde.Name = "chkDesde";
            this.chkDesde.Size = new System.Drawing.Size(80, 20);
            this.chkDesde.TabIndex = 5;
            this.chkDesde.Text = "Usar";
            this.chkDesde.CheckedChanged += new System.EventHandler(this.chkDesde_CheckedChanged);
            // 
            // chkHasta
            // 
            this.chkHasta.Location = new System.Drawing.Point(264, 76);
            this.chkHasta.Name = "chkHasta";
            this.chkHasta.Size = new System.Drawing.Size(80, 20);
            this.chkHasta.TabIndex = 6;
            this.chkHasta.Text = "Usar";
            this.chkHasta.CheckedChanged += new System.EventHandler(this.chkHasta_CheckedChanged);
            // 
            // lblModulo
            // 
            this.lblModulo.Location = new System.Drawing.Point(360, 40);
            this.lblModulo.Name = "lblModulo";
            this.lblModulo.Size = new System.Drawing.Size(60, 20);
            this.lblModulo.TabIndex = 11;
            this.lblModulo.Text = "Módulo:";
            // 
            // txtModulo
            // 
            this.txtModulo.Location = new System.Drawing.Point(426, 40);
            this.txtModulo.Name = "txtModulo";
            this.txtModulo.Size = new System.Drawing.Size(150, 20);
            this.txtModulo.TabIndex = 8;
            // 
            // lblAccion
            // 
            this.lblAccion.Location = new System.Drawing.Point(360, 66);
            this.lblAccion.Name = "lblAccion";
            this.lblAccion.Size = new System.Drawing.Size(60, 20);
            this.lblAccion.TabIndex = 9;
            this.lblAccion.Text = "Acción:";
            // 
            // txtAccion
            // 
            this.txtAccion.Location = new System.Drawing.Point(426, 66);
            this.txtAccion.Name = "txtAccion";
            this.txtAccion.Size = new System.Drawing.Size(150, 20);
            this.txtAccion.TabIndex = 9;
            // 
            // lblResultado
            // 
            this.lblResultado.Location = new System.Drawing.Point(600, 14);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(70, 20);
            this.lblResultado.TabIndex = 7;
            this.lblResultado.Text = "Resultado:";
            // 
            // txtResultado
            // 
            this.txtResultado.Location = new System.Drawing.Point(676, 14);
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.Size = new System.Drawing.Size(150, 20);
            this.txtResultado.TabIndex = 10;
            // 
            // lblTextoLibre
            // 
            this.lblTextoLibre.Location = new System.Drawing.Point(12, 106);
            this.lblTextoLibre.Name = "lblTextoLibre";
            this.lblTextoLibre.Size = new System.Drawing.Size(70, 20);
            this.lblTextoLibre.TabIndex = 5;
            this.lblTextoLibre.Text = "Texto:";
            // 
            // txtTextoLibre
            // 
            this.txtTextoLibre.Location = new System.Drawing.Point(78, 106);
            this.txtTextoLibre.Name = "txtTextoLibre";
            this.txtTextoLibre.Size = new System.Drawing.Size(748, 20);
            this.txtTextoLibre.TabIndex = 11;
            // 
            // lblEntidad
            // 
            this.lblEntidad.Location = new System.Drawing.Point(600, 40);
            this.lblEntidad.Name = "lblEntidad";
            this.lblEntidad.Size = new System.Drawing.Size(70, 20);
            this.lblEntidad.TabIndex = 3;
            this.lblEntidad.Text = "Entidad:";
            // 
            // txtEntidad
            // 
            this.txtEntidad.Location = new System.Drawing.Point(676, 40);
            this.txtEntidad.Name = "txtEntidad";
            this.txtEntidad.Size = new System.Drawing.Size(150, 20);
            this.txtEntidad.TabIndex = 12;
            // 
            // lblUsuarioId
            // 
            this.lblUsuarioId.Location = new System.Drawing.Point(600, 66);
            this.lblUsuarioId.Name = "lblUsuarioId";
            this.lblUsuarioId.Size = new System.Drawing.Size(70, 20);
            this.lblUsuarioId.TabIndex = 1;
            this.lblUsuarioId.Text = "Usuario Id:";
            // 
            // txtUsuarioId
            // 
            this.txtUsuarioId.Location = new System.Drawing.Point(676, 66);
            this.txtUsuarioId.Name = "txtUsuarioId";
            this.txtUsuarioId.Size = new System.Drawing.Size(150, 20);
            this.txtUsuarioId.TabIndex = 13;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.Location = new System.Drawing.Point(12, 140);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(860, 340);
            this.dataGridView1.TabIndex = 99;
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            // 
            // FrmBitacoraEventos
            // 
            this.ClientSize = new System.Drawing.Size(884, 491);
            this.Controls.Add(this.btnExportarCsv);
            this.Controls.Add(this.txtUsuarioId);
            this.Controls.Add(this.lblUsuarioId);
            this.Controls.Add(this.txtEntidad);
            this.Controls.Add(this.lblEntidad);
            this.Controls.Add(this.txtTextoLibre);
            this.Controls.Add(this.lblTextoLibre);
            this.Controls.Add(this.txtResultado);
            this.Controls.Add(this.lblResultado);
            this.Controls.Add(this.txtAccion);
            this.Controls.Add(this.lblAccion);
            this.Controls.Add(this.txtModulo);
            this.Controls.Add(this.lblModulo);
            this.Controls.Add(this.chkHasta);
            this.Controls.Add(this.chkDesde);
            this.Controls.Add(this.dtpHasta);
            this.Controls.Add(this.dtpDesde);
            this.Controls.Add(this.lblHasta);
            this.Controls.Add(this.lblDesde);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnBuscar);
            this.MinimumSize = new System.Drawing.Size(900, 530);
            this.Name = "FrmBitacoraEventos";
            this.Text = "Bitácora de Eventos";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnExportarCsv;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.CheckBox chkDesde;
        private System.Windows.Forms.CheckBox chkHasta;
        private System.Windows.Forms.Label lblModulo;
        private System.Windows.Forms.TextBox txtModulo;
        private System.Windows.Forms.Label lblAccion;
        private System.Windows.Forms.TextBox txtAccion;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.Label lblTextoLibre;
        private System.Windows.Forms.TextBox txtTextoLibre;
        private System.Windows.Forms.Label lblEntidad;
        private System.Windows.Forms.TextBox txtEntidad;
        private System.Windows.Forms.Label lblUsuarioId;
        private System.Windows.Forms.TextBox txtUsuarioId;
    }
}