namespace OCR
{
    partial class frmInicio
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.lblImagen = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.picEntrada = new System.Windows.Forms.PictureBox();
            this.txtRespuestas = new System.Windows.Forms.RichTextBox();
            this.txtSalida = new System.Windows.Forms.RichTextBox();
            this.opnArchivo = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.cmdBuscar = new System.Windows.Forms.Button();
            this.cmdComparar = new System.Windows.Forms.Button();
            this.lblSimilitud = new System.Windows.Forms.Label();
            this.cmdDesaturar = new System.Windows.Forms.Button();
            this.cbTipo = new System.Windows.Forms.ComboBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.txt_prueba = new System.Windows.Forms.TextBox();
            this.cmdPrueba = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEntrada)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(11, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Imagen";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblImagen
            // 
            this.lblImagen.AutoSize = true;
            this.lblImagen.Location = new System.Drawing.Point(93, 17);
            this.lblImagen.Name = "lblImagen";
            this.lblImagen.Size = new System.Drawing.Size(42, 13);
            this.lblImagen.TabIndex = 1;
            this.lblImagen.Text = "Imagen";
            this.lblImagen.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.Color.Silver;
            this.splitContainer1.Location = new System.Drawing.Point(11, 41);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.picEntrada);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1415, 677);
            this.splitContainer1.SplitterDistance = 783;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 2;
            // 
            // picEntrada
            // 
            this.picEntrada.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picEntrada.Location = new System.Drawing.Point(0, 0);
            this.picEntrada.Name = "picEntrada";
            this.picEntrada.Size = new System.Drawing.Size(783, 677);
            this.picEntrada.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picEntrada.TabIndex = 0;
            this.picEntrada.TabStop = false;
            // 
            // txtRespuestas
            // 
            this.txtRespuestas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRespuestas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRespuestas.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRespuestas.Location = new System.Drawing.Point(0, 0);
            this.txtRespuestas.Name = "txtRespuestas";
            this.txtRespuestas.ReadOnly = true;
            this.txtRespuestas.Size = new System.Drawing.Size(622, 101);
            this.txtRespuestas.TabIndex = 1;
            this.txtRespuestas.Text = "";
            // 
            // txtSalida
            // 
            this.txtSalida.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSalida.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSalida.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSalida.Location = new System.Drawing.Point(0, 0);
            this.txtSalida.Name = "txtSalida";
            this.txtSalida.Size = new System.Drawing.Size(622, 566);
            this.txtSalida.TabIndex = 0;
            this.txtSalida.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(204, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Estudiante:";
            // 
            // txtBuscar
            // 
            this.txtBuscar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBuscar.Location = new System.Drawing.Point(261, 15);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(256, 15);
            this.txtBuscar.TabIndex = 4;
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
            // 
            // cmdBuscar
            // 
            this.cmdBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdBuscar.Location = new System.Drawing.Point(1186, 7);
            this.cmdBuscar.Name = "cmdBuscar";
            this.cmdBuscar.Size = new System.Drawing.Size(75, 23);
            this.cmdBuscar.TabIndex = 5;
            this.cmdBuscar.Text = "Buscar";
            this.cmdBuscar.UseVisualStyleBackColor = true;
            this.cmdBuscar.Click += new System.EventHandler(this.cmdBuscar_Click);
            // 
            // cmdComparar
            // 
            this.cmdComparar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdComparar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdComparar.Location = new System.Drawing.Point(1267, 7);
            this.cmdComparar.Name = "cmdComparar";
            this.cmdComparar.Size = new System.Drawing.Size(75, 23);
            this.cmdComparar.TabIndex = 6;
            this.cmdComparar.Text = "Comparar";
            this.cmdComparar.UseVisualStyleBackColor = true;
            this.cmdComparar.Click += new System.EventHandler(this.cmdComparar_Click);
            // 
            // lblSimilitud
            // 
            this.lblSimilitud.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSimilitud.AutoSize = true;
            this.lblSimilitud.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblSimilitud.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSimilitud.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblSimilitud.Location = new System.Drawing.Point(1348, 12);
            this.lblSimilitud.Name = "lblSimilitud";
            this.lblSimilitud.Size = new System.Drawing.Size(0, 18);
            this.lblSimilitud.TabIndex = 7;
            this.lblSimilitud.Visible = false;
            // 
            // cmdDesaturar
            // 
            this.cmdDesaturar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDesaturar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdDesaturar.Location = new System.Drawing.Point(1351, 7);
            this.cmdDesaturar.Name = "cmdDesaturar";
            this.cmdDesaturar.Size = new System.Drawing.Size(75, 23);
            this.cmdDesaturar.TabIndex = 6;
            this.cmdDesaturar.Text = "Desaturar";
            this.cmdDesaturar.UseVisualStyleBackColor = true;
            // 
            // cbTipo
            // 
            this.cbTipo.FormattingEnabled = true;
            this.cbTipo.Items.AddRange(new object[] {
            "Birth",
            "Photo id",
            "Proof"});
            this.cbTipo.Location = new System.Drawing.Point(11, 14);
            this.cbTipo.Name = "cbTipo";
            this.cbTipo.Size = new System.Drawing.Size(187, 21);
            this.cbTipo.TabIndex = 8;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.Silver;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.txtRespuestas);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.txtSalida);
            this.splitContainer2.Size = new System.Drawing.Size(622, 677);
            this.splitContainer2.SplitterDistance = 101;
            this.splitContainer2.SplitterWidth = 10;
            this.splitContainer2.TabIndex = 2;
            // 
            // txt_prueba
            // 
            this.txt_prueba.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_prueba.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_prueba.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_prueba.Location = new System.Drawing.Point(924, 10);
            this.txt_prueba.Name = "txt_prueba";
            this.txt_prueba.Size = new System.Drawing.Size(256, 15);
            this.txt_prueba.TabIndex = 9;
            // 
            // cmdPrueba
            // 
            this.cmdPrueba.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdPrueba.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdPrueba.Location = new System.Drawing.Point(843, 7);
            this.cmdPrueba.Name = "cmdPrueba";
            this.cmdPrueba.Size = new System.Drawing.Size(75, 23);
            this.cmdPrueba.TabIndex = 10;
            this.cmdPrueba.Text = "Prueba";
            this.cmdPrueba.UseVisualStyleBackColor = true;
            this.cmdPrueba.Click += new System.EventHandler(this.cmdPrueba_Click);
            // 
            // frmInicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(1438, 730);
            this.Controls.Add(this.cmdPrueba);
            this.Controls.Add(this.txt_prueba);
            this.Controls.Add(this.cbTipo);
            this.Controls.Add(this.lblSimilitud);
            this.Controls.Add(this.cmdDesaturar);
            this.Controls.Add(this.cmdComparar);
            this.Controls.Add(this.cmdBuscar);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.lblImagen);
            this.Controls.Add(this.button1);
            this.Name = "frmInicio";
            this.Text = "OCR";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmInicio_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picEntrada)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblImagen;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox picEntrada;
        private System.Windows.Forms.RichTextBox txtSalida;
        private System.Windows.Forms.OpenFileDialog opnArchivo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Button cmdBuscar;
        private System.Windows.Forms.Button cmdComparar;
        private System.Windows.Forms.Label lblSimilitud;
        private System.Windows.Forms.Button cmdDesaturar;
        private System.Windows.Forms.RichTextBox txtRespuestas;
        private System.Windows.Forms.ComboBox cbTipo;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox txt_prueba;
        private System.Windows.Forms.Button cmdPrueba;
    }
}

