namespace Amellar.Modules.BusinessReports
{
    partial class frmReportListBnsQtrPaid
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.rdodtrange = new System.Windows.Forms.RadioButton();
            this.rdofyear = new System.Windows.Forms.RadioButton();
            this.rdo4thqtr = new System.Windows.Forms.RadioButton();
            this.rdo3rdqtr = new System.Windows.Forms.RadioButton();
            this.rdo2ndqtr = new System.Windows.Forms.RadioButton();
            this.rdo1stqtr = new System.Windows.Forms.RadioButton();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtTaxYear = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.SuspendLayout();
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(15, 15);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader1.Size = new System.Drawing.Size(190, 24);
            this.kryptonHeader1.TabIndex = 111;
            this.kryptonHeader1.TabStop = false;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Quarters";
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(12, 12);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(194, 178);
            this.containerWithShadow1.TabIndex = 22222;
            // 
            // rdodtrange
            // 
            this.rdodtrange.AutoSize = true;
            this.rdodtrange.Location = new System.Drawing.Point(26, 158);
            this.rdodtrange.Name = "rdodtrange";
            this.rdodtrange.Size = new System.Drawing.Size(144, 17);
            this.rdodtrange.TabIndex = 5;
            this.rdodtrange.Text = "Date Range (PAYMENT)";
            this.rdodtrange.UseVisualStyleBackColor = true;
            this.rdodtrange.Click += new System.EventHandler(this.rdodtrange_Click);
            // 
            // rdofyear
            // 
            this.rdofyear.AutoSize = true;
            this.rdofyear.Location = new System.Drawing.Point(26, 135);
            this.rdofyear.Name = "rdofyear";
            this.rdofyear.Size = new System.Drawing.Size(64, 17);
            this.rdofyear.TabIndex = 4;
            this.rdofyear.Text = "Full year";
            this.rdofyear.UseVisualStyleBackColor = true;
            this.rdofyear.Click += new System.EventHandler(this.rdofyear_Click);
            // 
            // rdo4thqtr
            // 
            this.rdo4thqtr.AutoSize = true;
            this.rdo4thqtr.Location = new System.Drawing.Point(26, 114);
            this.rdo4thqtr.Name = "rdo4thqtr";
            this.rdo4thqtr.Size = new System.Drawing.Size(93, 17);
            this.rdo4thqtr.TabIndex = 3;
            this.rdo4thqtr.Text = "Fourth Quarter";
            this.rdo4thqtr.UseVisualStyleBackColor = true;
            this.rdo4thqtr.Click += new System.EventHandler(this.rdo4thqtr_Click);
            // 
            // rdo3rdqtr
            // 
            this.rdo3rdqtr.AutoSize = true;
            this.rdo3rdqtr.Location = new System.Drawing.Point(26, 91);
            this.rdo3rdqtr.Name = "rdo3rdqtr";
            this.rdo3rdqtr.Size = new System.Drawing.Size(87, 17);
            this.rdo3rdqtr.TabIndex = 2;
            this.rdo3rdqtr.Text = "Third Quarter";
            this.rdo3rdqtr.UseVisualStyleBackColor = true;
            this.rdo3rdqtr.Click += new System.EventHandler(this.rdo3rdqtr_Click);
            // 
            // rdo2ndqtr
            // 
            this.rdo2ndqtr.AutoSize = true;
            this.rdo2ndqtr.Location = new System.Drawing.Point(26, 68);
            this.rdo2ndqtr.Name = "rdo2ndqtr";
            this.rdo2ndqtr.Size = new System.Drawing.Size(100, 17);
            this.rdo2ndqtr.TabIndex = 1;
            this.rdo2ndqtr.Text = "Second Quarter";
            this.rdo2ndqtr.UseVisualStyleBackColor = true;
            this.rdo2ndqtr.Click += new System.EventHandler(this.rdo2ndqtr_Click);
            // 
            // rdo1stqtr
            // 
            this.rdo1stqtr.AutoSize = true;
            this.rdo1stqtr.Location = new System.Drawing.Point(26, 45);
            this.rdo1stqtr.Name = "rdo1stqtr";
            this.rdo1stqtr.Size = new System.Drawing.Size(82, 17);
            this.rdo1stqtr.TabIndex = 0;
            this.rdo1stqtr.Text = "First Quarter";
            this.rdo1stqtr.UseVisualStyleBackColor = true;
            this.rdo1stqtr.Click += new System.EventHandler(this.rdo1stqtr_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(244, 141);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(137, 25);
            this.btnGenerate.TabIndex = 9;
            this.btnGenerate.Values.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // txtTaxYear
            // 
            this.txtTaxYear.Location = new System.Drawing.Point(285, 45);
            this.txtTaxYear.MaxLength = 4;
            this.txtTaxYear.Name = "txtTaxYear";
            this.txtTaxYear.Size = new System.Drawing.Size(92, 20);
            this.txtTaxYear.TabIndex = 6;
            this.txtTaxYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTaxYear_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(229, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(50, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "Tax Year";
            // 
            // dtpTo
            // 
            this.dtpTo.Enabled = false;
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(285, 105);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(92, 20);
            this.dtpTo.TabIndex = 8;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Enabled = false;
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(285, 79);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(92, 20);
            this.dtpFrom.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(259, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "To";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(249, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "From";
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(214, 15);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(194, 175);
            this.containerWithShadow2.TabIndex = 36;
            // 
            // frmReportListBnsQtrPaid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 200);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTaxYear);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.rdodtrange);
            this.Controls.Add(this.rdofyear);
            this.Controls.Add(this.rdo4thqtr);
            this.Controls.Add(this.rdo3rdqtr);
            this.Controls.Add(this.rdo2ndqtr);
            this.Controls.Add(this.rdo1stqtr);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.containerWithShadow1);
            this.Controls.Add(this.containerWithShadow2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmReportListBnsQtrPaid";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.Load += new System.EventHandler(this.frmReportListBnsQtrPaid_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.RadioButton rdodtrange;
        private System.Windows.Forms.RadioButton rdofyear;
        private System.Windows.Forms.RadioButton rdo4thqtr;
        private System.Windows.Forms.RadioButton rdo3rdqtr;
        private System.Windows.Forms.RadioButton rdo2ndqtr;
        private System.Windows.Forms.RadioButton rdo1stqtr;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private System.Windows.Forms.TextBox txtTaxYear;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
    }
}