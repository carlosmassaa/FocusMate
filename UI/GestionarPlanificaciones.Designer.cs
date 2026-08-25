namespace UI
{
    partial class GestionarPlanificaciones
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
            this.lblUsuario = new System.Windows.Forms.Label();
            this.comboBoxUsuarios = new System.Windows.Forms.ComboBox();
            this.lblEstado = new System.Windows.Forms.Label();
            this.comboBoxEstado = new System.Windows.Forms.ComboBox();
            this.lblDesde = new System.Windows.Forms.Label();
            this.dateTimePickerDesde = new System.Windows.Forms.DateTimePicker();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dateTimePickerHasta = new System.Windows.Forms.DateTimePicker();
            this.btnAplicarFiltros = new System.Windows.Forms.Button();
            this.btnLimpiarFiltros = new System.Windows.Forms.Button();
            this.dataGridViewPlanificaciones = new System.Windows.Forms.DataGridView();
            this.dataGridViewDetallePlanificacion = new System.Windows.Forms.DataGridView();
            this.lblTareasUsuario = new System.Windows.Forms.Label();
            this.dataGridViewTareasUsuario = new System.Windows.Forms.DataGridView();
            this.lblObservacion = new System.Windows.Forms.Label();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.btnCargarPlanificaciones = new System.Windows.Forms.Button();
            this.btnRegistrarRevision = new System.Windows.Forms.Button();
            this.btnRegistrarObservacion = new System.Windows.Forms.Button();
            this.btnAprobar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPlanificaciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetallePlanificacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTareasUsuario)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(12, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(125, 13);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Gestionar planificaciones";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(12, 48);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblUsuario.TabIndex = 1;
            this.lblUsuario.Text = "Usuario:";
            // 
            // comboBoxUsuarios
            // 
            this.comboBoxUsuarios.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUsuarios.FormattingEnabled = true;
            this.comboBoxUsuarios.Location = new System.Drawing.Point(64, 45);
            this.comboBoxUsuarios.Name = "comboBoxUsuarios";
            this.comboBoxUsuarios.Size = new System.Drawing.Size(160, 21);
            this.comboBoxUsuarios.TabIndex = 2;
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(240, 48);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(43, 13);
            this.lblEstado.TabIndex = 3;
            this.lblEstado.Text = "Estado:";
            // 
            // comboBoxEstado
            // 
            this.comboBoxEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEstado.FormattingEnabled = true;
            this.comboBoxEstado.Location = new System.Drawing.Point(289, 45);
            this.comboBoxEstado.Name = "comboBoxEstado";
            this.comboBoxEstado.Size = new System.Drawing.Size(130, 21);
            this.comboBoxEstado.TabIndex = 4;
            // 
            // lblDesde
            // 
            this.lblDesde.AutoSize = true;
            this.lblDesde.Location = new System.Drawing.Point(435, 48);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.Size = new System.Drawing.Size(41, 13);
            this.lblDesde.TabIndex = 5;
            this.lblDesde.Text = "Desde:";
            // 
            // dateTimePickerDesde
            // 
            this.dateTimePickerDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerDesde.Location = new System.Drawing.Point(482, 45);
            this.dateTimePickerDesde.Name = "dateTimePickerDesde";
            this.dateTimePickerDesde.Size = new System.Drawing.Size(105, 20);
            this.dateTimePickerDesde.TabIndex = 6;
            // 
            // lblHasta
            // 
            this.lblHasta.AutoSize = true;
            this.lblHasta.Location = new System.Drawing.Point(603, 48);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.Size = new System.Drawing.Size(38, 13);
            this.lblHasta.TabIndex = 7;
            this.lblHasta.Text = "Hasta:";
            // 
            // dateTimePickerHasta
            // 
            this.dateTimePickerHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerHasta.Location = new System.Drawing.Point(647, 45);
            this.dateTimePickerHasta.Name = "dateTimePickerHasta";
            this.dateTimePickerHasta.Size = new System.Drawing.Size(105, 20);
            this.dateTimePickerHasta.TabIndex = 8;
            // 
            // btnAplicarFiltros
            // 
            this.btnAplicarFiltros.Location = new System.Drawing.Point(765, 42);
            this.btnAplicarFiltros.Name = "btnAplicarFiltros";
            this.btnAplicarFiltros.Size = new System.Drawing.Size(110, 25);
            this.btnAplicarFiltros.TabIndex = 9;
            this.btnAplicarFiltros.Text = "Aplicar filtros";
            this.btnAplicarFiltros.UseVisualStyleBackColor = true;
            this.btnAplicarFiltros.Click += new System.EventHandler(this.btnAplicarFiltros_Click);
            // 
            // btnLimpiarFiltros
            // 
            this.btnLimpiarFiltros.Location = new System.Drawing.Point(881, 42);
            this.btnLimpiarFiltros.Name = "btnLimpiarFiltros";
            this.btnLimpiarFiltros.Size = new System.Drawing.Size(110, 25);
            this.btnLimpiarFiltros.TabIndex = 10;
            this.btnLimpiarFiltros.Text = "Limpiar filtros";
            this.btnLimpiarFiltros.UseVisualStyleBackColor = true;
            this.btnLimpiarFiltros.Click += new System.EventHandler(this.btnLimpiarFiltros_Click);
            // 
            // dataGridViewPlanificaciones
            // 
            this.dataGridViewPlanificaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPlanificaciones.Location = new System.Drawing.Point(15, 82);
            this.dataGridViewPlanificaciones.Name = "dataGridViewPlanificaciones";
            this.dataGridViewPlanificaciones.Size = new System.Drawing.Size(770, 155);
            this.dataGridViewPlanificaciones.TabIndex = 11;
            // 
            // dataGridViewDetallePlanificacion
            // 
            this.dataGridViewDetallePlanificacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDetallePlanificacion.Location = new System.Drawing.Point(15, 255);
            this.dataGridViewDetallePlanificacion.Name = "dataGridViewDetallePlanificacion";
            this.dataGridViewDetallePlanificacion.Size = new System.Drawing.Size(770, 230);
            this.dataGridViewDetallePlanificacion.TabIndex = 12;
            // 
            // lblTareasUsuario
            // 
            this.lblTareasUsuario.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTareasUsuario.AutoSize = true;
            this.lblTareasUsuario.Location = new System.Drawing.Point(844, 82);
            this.lblTareasUsuario.Name = "lblTareasUsuario";
            this.lblTareasUsuario.Size = new System.Drawing.Size(94, 13);
            this.lblTareasUsuario.TabIndex = 13;
            this.lblTareasUsuario.Text = "Tareas del usuario";
            // 
            // dataGridViewTareasUsuario
            // 
            this.dataGridViewTareasUsuario.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTareasUsuario.Location = new System.Drawing.Point(808, 105);
            this.dataGridViewTareasUsuario.Name = "dataGridViewTareasUsuario";
            this.dataGridViewTareasUsuario.Size = new System.Drawing.Size(652, 380);
            this.dataGridViewTareasUsuario.TabIndex = 14;
            // 
            // lblObservacion
            // 
            this.lblObservacion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblObservacion.AutoSize = true;
            this.lblObservacion.Location = new System.Drawing.Point(12, 501);
            this.lblObservacion.Name = "lblObservacion";
            this.lblObservacion.Size = new System.Drawing.Size(70, 13);
            this.lblObservacion.TabIndex = 15;
            this.lblObservacion.Text = "Observación:";
            // 
            // txtObservacion
            // 
            this.txtObservacion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObservacion.Location = new System.Drawing.Point(15, 523);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Size = new System.Drawing.Size(1445, 65);
            this.txtObservacion.TabIndex = 16;
            // 
            // btnCargarPlanificaciones
            // 
            this.btnCargarPlanificaciones.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCargarPlanificaciones.Location = new System.Drawing.Point(15, 605);
            this.btnCargarPlanificaciones.Name = "btnCargarPlanificaciones";
            this.btnCargarPlanificaciones.Size = new System.Drawing.Size(150, 28);
            this.btnCargarPlanificaciones.TabIndex = 17;
            this.btnCargarPlanificaciones.Text = "Actualizar listado";
            this.btnCargarPlanificaciones.UseVisualStyleBackColor = true;
            this.btnCargarPlanificaciones.Click += new System.EventHandler(this.btnCargarPlanificaciones_Click);
            // 
            // btnRegistrarRevision
            // 
            this.btnRegistrarRevision.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRegistrarRevision.Location = new System.Drawing.Point(171, 605);
            this.btnRegistrarRevision.Name = "btnRegistrarRevision";
            this.btnRegistrarRevision.Size = new System.Drawing.Size(150, 28);
            this.btnRegistrarRevision.TabIndex = 18;
            this.btnRegistrarRevision.Text = "Registrar revisión";
            this.btnRegistrarRevision.UseVisualStyleBackColor = true;
            this.btnRegistrarRevision.Click += new System.EventHandler(this.btnRegistrarRevision_Click);
            // 
            // btnRegistrarObservacion
            // 
            this.btnRegistrarObservacion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRegistrarObservacion.Location = new System.Drawing.Point(327, 605);
            this.btnRegistrarObservacion.Name = "btnRegistrarObservacion";
            this.btnRegistrarObservacion.Size = new System.Drawing.Size(160, 28);
            this.btnRegistrarObservacion.TabIndex = 19;
            this.btnRegistrarObservacion.Text = "Registrar observación";
            this.btnRegistrarObservacion.UseVisualStyleBackColor = true;
            this.btnRegistrarObservacion.Click += new System.EventHandler(this.btnRegistrarObservacion_Click);
            // 
            // btnAprobar
            // 
            this.btnAprobar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAprobar.Location = new System.Drawing.Point(493, 605);
            this.btnAprobar.Name = "btnAprobar";
            this.btnAprobar.Size = new System.Drawing.Size(120, 28);
            this.btnAprobar.TabIndex = 20;
            this.btnAprobar.Text = "Aprobar";
            this.btnAprobar.UseVisualStyleBackColor = true;
            this.btnAprobar.Click += new System.EventHandler(this.btnAprobar_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Location = new System.Drawing.Point(1350, 605);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(110, 28);
            this.btnCerrar.TabIndex = 21;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // GestionarPlanificaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1476, 646);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnAprobar);
            this.Controls.Add(this.btnRegistrarObservacion);
            this.Controls.Add(this.btnRegistrarRevision);
            this.Controls.Add(this.btnCargarPlanificaciones);
            this.Controls.Add(this.txtObservacion);
            this.Controls.Add(this.lblObservacion);
            this.Controls.Add(this.dataGridViewTareasUsuario);
            this.Controls.Add(this.lblTareasUsuario);
            this.Controls.Add(this.dataGridViewDetallePlanificacion);
            this.Controls.Add(this.dataGridViewPlanificaciones);
            this.Controls.Add(this.btnLimpiarFiltros);
            this.Controls.Add(this.btnAplicarFiltros);
            this.Controls.Add(this.dateTimePickerHasta);
            this.Controls.Add(this.lblHasta);
            this.Controls.Add(this.dateTimePickerDesde);
            this.Controls.Add(this.lblDesde);
            this.Controls.Add(this.comboBoxEstado);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.comboBoxUsuarios);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.lblTitulo);
            this.Name = "GestionarPlanificaciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gestionar Planificaciones";
            this.Load += new System.EventHandler(this.GestionarPlanificaciones_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPlanificaciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetallePlanificacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTareasUsuario)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.ComboBox comboBoxUsuarios;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.ComboBox comboBoxEstado;
        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.DateTimePicker dateTimePickerDesde;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.DateTimePicker dateTimePickerHasta;
        private System.Windows.Forms.Button btnAplicarFiltros;
        private System.Windows.Forms.Button btnLimpiarFiltros;
        private System.Windows.Forms.DataGridView dataGridViewPlanificaciones;
        private System.Windows.Forms.DataGridView dataGridViewDetallePlanificacion;
        private System.Windows.Forms.Label lblTareasUsuario;
        private System.Windows.Forms.DataGridView dataGridViewTareasUsuario;
        private System.Windows.Forms.Label lblObservacion;
        private System.Windows.Forms.TextBox txtObservacion;
        private System.Windows.Forms.Button btnCargarPlanificaciones;
        private System.Windows.Forms.Button btnRegistrarRevision;
        private System.Windows.Forms.Button btnRegistrarObservacion;
        private System.Windows.Forms.Button btnAprobar;
        private System.Windows.Forms.Button btnCerrar;
    }
}