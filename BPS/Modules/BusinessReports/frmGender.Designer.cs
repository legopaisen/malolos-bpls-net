namespace Amellar.Modules.BusinessReports
{
    partial class frmGender
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.dtpOrDateTo = new System.Windows.Forms.DateTimePicker();
            this.dtpOrDateFr = new System.Windows.Forms.DateTimePicker();
            this.label21 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cmbStatus);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.cmbGender);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.btnClose);
            this.groupBox5.Controls.Add(this.btnGenerate);
            this.groupBox5.Controls.Add(this.dtpOrDateTo);
            this.groupBox5.Controls.Add(this.dtpOrDateFr);
            this.groupBox5.Controls.Add(this.label21);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Location = new System.Drawing.Point(12, 5);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(357, 147);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            // 
            // cmbGender
            // 
            this.cmbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGender.FormattingEnabled = true;
            this.cmbGender.Location = new System.Drawing.Point(115, 19);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(139, 21);
            this.cmbGender.TabIndex = 35;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 13);
            this.label10.TabIndex = 34;
            this.label10.Text = "Gender";
            // 
            // btnClose
            // 
            this.btnClose.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnClose.Location = new System.Drawing.Point(249, 114);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(95, 24);
            this.btnClose.TabIndex = 33;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnGenerate.Location = new System.Drawing.Point(135, 114);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(108, 24);
            this.btnGenerate.TabIndex = 32;
            this.btnGenerate.Values.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // dtpOrDateTo
            // 
            this.dtpOrDateTo.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dtpOrDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpOrDateTo.Location = new System.Drawing.Point(244, 79);
            this.dtpOrDateTo.Name = "dtpOrDateTo";
            this.dtpOrDateTo.Size = new System.Drawing.Size(100, 20);
            this.dtpOrDateTo.TabIndex = 24;
            // 
            // dtpOrDateFr
            // 
            this.dtpOrDateFr.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dtpOrDateFr.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpOrDateFr.Location = new System.Drawing.Point(115, 79);
            this.dtpOrDateFr.Name = "dtpOrDateFr";
            this.dtpOrDateFr.Size = new System.Drawing.Size(99, 20);
            this.dtpOrDateFr.TabIndex = 23;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(223, 82);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(20, 13);
            this.label21.TabIndex = 0;
            this.label21.Text = "To";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(11, 82);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(65, 13);
            this.label19.TabIndex = 0;
            this.label19.Text = "Permit Date:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(83, 82);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(30, 13);
            this.label20.TabIndex = 0;
            this.label20.Text = "From";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(115, 46);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(139, 21);
            this.cmbStatus.TabIndex = 37;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 36;
            this.label1.Text = "Business Status";
            // 
            // frmGender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 164);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox5);
            this.Name = "frmGender";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "List of Businesses By Gender";
            this.Load += new System.EventHandler(this.frmGender_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DateTimePicker dtpOrDateTo;
        private System.Windows.Forms.DateTimePicker dtpOrDateFr;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private System.Windows.Forms.ComboBox cmbGender;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label1;
    }
}