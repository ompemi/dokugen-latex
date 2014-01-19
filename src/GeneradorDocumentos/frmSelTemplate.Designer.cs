namespace DokuGen
{
    partial class frmSelTemplate
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
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
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelTemplate));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtTemplate = new System.Windows.Forms.TextBox();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnChooseTemplate = new System.Windows.Forms.Button();
            this.btnNextFromTemp = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnBefFromTemp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(244, 383);
            this.panel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtTemplate);
            this.groupBox2.Controls.Add(this.btnPreview);
            this.groupBox2.Controls.Add(this.btnChooseTemplate);
            this.groupBox2.Location = new System.Drawing.Point(279, 128);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(393, 76);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Elegir plantilla a rellenar";
            // 
            // txtTemplate
            // 
            this.txtTemplate.Location = new System.Drawing.Point(20, 31);
            this.txtTemplate.Name = "txtTemplate";
            this.txtTemplate.Size = new System.Drawing.Size(236, 20);
            this.txtTemplate.TabIndex = 1;
            // 
            // btnPreview
            // 
            this.btnPreview.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPreview.BackgroundImage")));
            this.btnPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPreview.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnPreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreview.Location = new System.Drawing.Point(354, 29);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(26, 22);
            this.btnPreview.TabIndex = 8;
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Visible = false;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnChooseTemplate
            // 
            this.btnChooseTemplate.Location = new System.Drawing.Point(273, 28);
            this.btnChooseTemplate.Name = "btnChooseTemplate";
            this.btnChooseTemplate.Size = new System.Drawing.Size(75, 23);
            this.btnChooseTemplate.TabIndex = 0;
            this.btnChooseTemplate.Text = "Examinar";
            this.btnChooseTemplate.UseVisualStyleBackColor = true;
            this.btnChooseTemplate.Click += new System.EventHandler(this.btnChooseTemplate_Click);
            // 
            // btnNextFromTemp
            // 
            this.btnNextFromTemp.Location = new System.Drawing.Point(597, 422);
            this.btnNextFromTemp.Name = "btnNextFromTemp";
            this.btnNextFromTemp.Size = new System.Drawing.Size(75, 23);
            this.btnNextFromTemp.TabIndex = 2;
            this.btnNextFromTemp.Text = "Siguiente";
            this.btnNextFromTemp.UseVisualStyleBackColor = true;
            this.btnNextFromTemp.Click += new System.EventHandler(this.btnNextFromTemp_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(404, 422);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnBefFromTemp
            // 
            this.btnBefFromTemp.Location = new System.Drawing.Point(516, 422);
            this.btnBefFromTemp.Name = "btnBefFromTemp";
            this.btnBefFromTemp.Size = new System.Drawing.Size(75, 23);
            this.btnBefFromTemp.TabIndex = 4;
            this.btnBefFromTemp.Text = "Anterior";
            this.btnBefFromTemp.UseVisualStyleBackColor = true;
            this.btnBefFromTemp.Click += new System.EventHandler(this.btnBefFromTemp_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(273, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 31);
            this.label1.TabIndex = 5;
            this.label1.Text = "Paso 2:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(279, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(309, 26);
            this.label2.TabIndex = 6;
            this.label2.Text = "Seleccione la plantilla a rellenar deseada sobre las existentes, o \r\ntambien pued" +
                "e abrir una guardada anteriormente.\r\n";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Title = "Elegir plantilla";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(273, 240);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 31);
            this.label3.TabIndex = 5;
            this.label3.Text = "Paso 3:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(276, 299);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(291, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Pulsa siguiente e introduce tus datos y genera el documento\r\n";
            // 
            // frmSelTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 457);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBefFromTemp);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNextFromTemp);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(692, 491);
            this.Name = "frmSelTemplate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DokuGen";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.btnCancel_Click);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtTemplate;
        private System.Windows.Forms.Button btnChooseTemplate;
        private System.Windows.Forms.Button btnNextFromTemp;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnBefFromTemp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnPreview;
    }
}