namespace Amellar.Modules.InspectorsDetails
{
    partial class frmPrintOptions
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
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.cmbInspector = new System.Windows.Forms.ComboBox();
            this.rdoBin = new System.Windows.Forms.RadioButton();
            this.rdoInspector = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.bin1 = new Amellar.Common.BIN.BIN();
            this.txtIS = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(105, 176);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(72, 25);
            this.btnGenerate.TabIndex = 8;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.Values.ExtraText = "";
            this.btnGenerate.Values.Image = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedNormal = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedPressed = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedTracking = null;
            this.btnGenerate.Values.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(183, 176);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(72, 25);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cmbInspector
            // 
            this.cmbInspector.FormattingEnabled = true;
            this.cmbInspector.Location = new System.Drawing.Point(105, 31);
            this.cmbInspector.Name = "cmbInspector";
            this.cmbInspector.Size = new System.Drawing.Size(136, 21);
            this.cmbInspector.TabIndex = 2;
            // 
            // rdoBin
            // 
            this.rdoBin.AutoSize = true;
            this.rdoBin.Location = new System.Drawing.Point(29, 62);
            this.rdoBin.Name = "rdoBin";
            this.rdoBin.Size = new System.Drawing.Size(43, 17);
            this.rdoBin.TabIndex = 3;
            this.rdoBin.TabStop = true;
            this.rdoBin.Text = "BIN";
            this.rdoBin.UseVisualStyleBackColor = true;
            this.rdoBin.CheckedChanged += new System.EventHandler(this.rdoBin_CheckedChanged);
            // 
            // rdoInspector
            // 
            this.rdoInspector.AutoSize = true;
            this.rdoInspector.Location = new System.Drawing.Point(29, 35);
            this.rdoInspector.Name = "rdoInspector";
            this.rdoInspector.Size = new System.Drawing.Size(69, 17);
            this.rdoInspector.TabIndex = 1;
            this.rdoInspector.TabStop = true;
            this.rdoInspector.Text = "Inspector";
            this.rdoInspector.UseVisualStyleBackColor = true;
            this.rdoInspector.CheckedChanged += new System.EventHandler(this.rdoInspector_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Date Inspected/Verified";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // dtpDateFrom
            // 
            this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateFrom.Location = new System.Drawing.Point(59, 124);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(77, 20);
            this.dtpDateFrom.TabIndex = 6;
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateTo.Location = new System.Drawing.Point(164, 125);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(77, 20);
            this.dtpDateTo.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "from";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(142, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "to";
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(12, 12);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(243, 158);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(105, 58);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(136, 21);
            this.bin1.TabIndex = 4;
            // 
            // txtIS
            // 
            this.txtIS.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtIS.Location = new System.Drawing.Point(105, 58);
            this.txtIS.Name = "txtIS";
            this.txtIS.Size = new System.Drawing.Size(81, 20);
            this.txtIS.TabIndex = 5;
            this.txtIS.Visible = false;
            // 
            // frmPrintOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 211);
            this.Controls.Add(this.txtIS);
            this.Controls.Add(this.bin1);
            this.Controls.Add(this.dtpDateTo);
            this.Controls.Add(this.dtpDateFrom);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rdoInspector);
            this.Controls.Add(this.rdoBin);
            this.Controls.Add(this.cmbInspector);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.containerWithShadow1);
            this.Name = "frmPrintOptions";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Printing Options";
            this.Load += new System.EventHandler(this.frmPrintOptions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ComboBox cmbInspector;
        public System.Windows.Forms.RadioButton rdoBin;
        public System.Windows.Forms.RadioButton rdoInspector;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.DateTimePicker dtpDateFrom;
        public System.Windows.Forms.DateTimePicker dtpDateTo;
        public Amellar.Common.BIN.BIN bin1;
        public System.Windows.Forms.TextBox txtIS;
    }
}