namespace Amellar.Modules.Collectibles
{
    partial class frmCollectibles
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCollectibles));
            this.chkFilter = new System.Windows.Forms.CheckBox();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtTop = new System.Windows.Forms.TextBox();
            this.cmbBusStat = new System.Windows.Forms.ComboBox();
            this.cmbOrgnKind = new System.Windows.Forms.ComboBox();
            this.cmbLineBus = new System.Windows.Forms.ComboBox();
            this.cmbBusType = new System.Windows.Forms.ComboBox();
            this.cmbBrgy = new System.Windows.Forms.ComboBox();
            this.cmbDist = new System.Windows.Forms.ComboBox();
            this.lblTop = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTeller = new System.Windows.Forms.Label();
            this.rdoTopCollectibles = new System.Windows.Forms.RadioButton();
            this.rdoBusStat = new System.Windows.Forms.RadioButton();
            this.rdoMainBus = new System.Windows.Forms.RadioButton();
            this.rdoOwnKind = new System.Windows.Forms.RadioButton();
            this.rdoDist = new System.Windows.Forms.RadioButton();
            this.rdoBrgy = new System.Windows.Forms.RadioButton();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.GB1 = new System.Windows.Forms.GroupBox();
            this.cmbTaxYear = new System.Windows.Forms.ComboBox();
            this.GB1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkFilter
            // 
            this.chkFilter.AutoSize = true;
            this.chkFilter.Location = new System.Drawing.Point(25, 179);
            this.chkFilter.Name = "chkFilter";
            this.chkFilter.Size = new System.Drawing.Size(73, 17);
            this.chkFilter.TabIndex = 7;
            this.chkFilter.Text = "Filter Year";
            this.chkFilter.UseVisualStyleBackColor = true;
            this.chkFilter.CheckStateChanged += new System.EventHandler(this.chkFilter_CheckStateChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(422, 221);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(62, 25);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(343, 221);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(73, 25);
            this.btnGenerate.TabIndex = 16;
            this.btnGenerate.Values.Text = "&Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // txtTop
            // 
            this.txtTop.Enabled = false;
            this.txtTop.Location = new System.Drawing.Point(286, 223);
            this.txtTop.Name = "txtTop";
            this.txtTop.Size = new System.Drawing.Size(51, 20);
            this.txtTop.TabIndex = 15;
            this.txtTop.Text = "0";
            this.txtTop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTop.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTop_KeyPress);
            // 
            // cmbBusStat
            // 
            this.cmbBusStat.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbBusStat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBusStat.FormattingEnabled = true;
            this.cmbBusStat.Items.AddRange(new object[] {
            "ALL",
            "NEW",
            "RENEWAL",
            "RETIRED"});
            this.cmbBusStat.Location = new System.Drawing.Point(286, 148);
            this.cmbBusStat.Name = "cmbBusStat";
            this.cmbBusStat.Size = new System.Drawing.Size(198, 21);
            this.cmbBusStat.TabIndex = 13;
            // 
            // cmbOrgnKind
            // 
            this.cmbOrgnKind.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbOrgnKind.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbOrgnKind.FormattingEnabled = true;
            this.cmbOrgnKind.Items.AddRange(new object[] {
            "ALL",
            "SINGLE PROPRIETORSHIP",
            "PARTNERSHIP",
            "CORPORATION",
            "COOPERATIVE"});
            this.cmbOrgnKind.Location = new System.Drawing.Point(286, 175);
            this.cmbOrgnKind.Name = "cmbOrgnKind";
            this.cmbOrgnKind.Size = new System.Drawing.Size(198, 21);
            this.cmbOrgnKind.TabIndex = 14;
            // 
            // cmbLineBus
            // 
            this.cmbLineBus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbLineBus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbLineBus.FormattingEnabled = true;
            this.cmbLineBus.Location = new System.Drawing.Point(286, 121);
            this.cmbLineBus.Name = "cmbLineBus";
            this.cmbLineBus.Size = new System.Drawing.Size(198, 21);
            this.cmbLineBus.TabIndex = 12;
            this.cmbLineBus.SelectedIndexChanged += new System.EventHandler(this.cmbLineBus_SelectedIndexChanged);
            this.cmbLineBus.DropDown += new System.EventHandler(this.cmbLineBus_DropDown);
            // 
            // cmbBusType
            // 
            this.cmbBusType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbBusType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBusType.FormattingEnabled = true;
            this.cmbBusType.Location = new System.Drawing.Point(286, 94);
            this.cmbBusType.Name = "cmbBusType";
            this.cmbBusType.Size = new System.Drawing.Size(198, 21);
            this.cmbBusType.TabIndex = 11;
            this.cmbBusType.SelectedIndexChanged += new System.EventHandler(this.cmbBusType_SelectedIndexChanged);
            // 
            // cmbBrgy
            // 
            this.cmbBrgy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbBrgy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBrgy.Enabled = false;
            this.cmbBrgy.FormattingEnabled = true;
            this.cmbBrgy.Location = new System.Drawing.Point(286, 40);
            this.cmbBrgy.Name = "cmbBrgy";
            this.cmbBrgy.Size = new System.Drawing.Size(198, 21);
            this.cmbBrgy.TabIndex = 9;
            // 
            // cmbDist
            // 
            this.cmbDist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbDist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDist.Enabled = false;
            this.cmbDist.FormattingEnabled = true;
            this.cmbDist.Location = new System.Drawing.Point(286, 67);
            this.cmbDist.Name = "cmbDist";
            this.cmbDist.Size = new System.Drawing.Size(198, 21);
            this.cmbDist.TabIndex = 10;
            // 
            // lblTop
            // 
            this.lblTop.AutoSize = true;
            this.lblTop.Location = new System.Drawing.Point(252, 226);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new System.Drawing.Size(26, 13);
            this.lblTop.TabIndex = 129;
            this.lblTop.Text = "Top";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(197, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 130;
            this.label5.Text = "Business Status";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(197, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 127;
            this.label4.Text = "Ownership Kind";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(197, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 126;
            this.label3.Text = "Sub Category";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(197, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 125;
            this.label2.Text = "Main Business";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(197, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 124;
            this.label1.Text = "Barangay Name";
            // 
            // lblTeller
            // 
            this.lblTeller.AutoSize = true;
            this.lblTeller.Location = new System.Drawing.Point(197, 71);
            this.lblTeller.Name = "lblTeller";
            this.lblTeller.Size = new System.Drawing.Size(70, 13);
            this.lblTeller.TabIndex = 123;
            this.lblTeller.Text = "District Name";
            // 
            // rdoTopCollectibles
            // 
            this.rdoTopCollectibles.AutoSize = true;
            this.rdoTopCollectibles.Location = new System.Drawing.Point(25, 156);
            this.rdoTopCollectibles.Name = "rdoTopCollectibles";
            this.rdoTopCollectibles.Size = new System.Drawing.Size(100, 17);
            this.rdoTopCollectibles.TabIndex = 6;
            this.rdoTopCollectibles.Text = "Top Collectibles";
            this.rdoTopCollectibles.UseVisualStyleBackColor = true;
            this.rdoTopCollectibles.Click += new System.EventHandler(this.rdoTopCollectibles_Click);
            // 
            // rdoBusStat
            // 
            this.rdoBusStat.AutoSize = true;
            this.rdoBusStat.Location = new System.Drawing.Point(25, 87);
            this.rdoBusStat.Name = "rdoBusStat";
            this.rdoBusStat.Size = new System.Drawing.Size(100, 17);
            this.rdoBusStat.TabIndex = 3;
            this.rdoBusStat.Text = "Business Status";
            this.rdoBusStat.UseVisualStyleBackColor = true;
            this.rdoBusStat.Click += new System.EventHandler(this.rdoBusStat_Click);
            // 
            // rdoMainBus
            // 
            this.rdoMainBus.AutoSize = true;
            this.rdoMainBus.Location = new System.Drawing.Point(25, 64);
            this.rdoMainBus.Name = "rdoMainBus";
            this.rdoMainBus.Size = new System.Drawing.Size(93, 17);
            this.rdoMainBus.TabIndex = 2;
            this.rdoMainBus.Text = "Main Business";
            this.rdoMainBus.UseVisualStyleBackColor = true;
            this.rdoMainBus.Click += new System.EventHandler(this.rdoMainBus_Click);
            // 
            // rdoOwnKind
            // 
            this.rdoOwnKind.AutoSize = true;
            this.rdoOwnKind.Location = new System.Drawing.Point(25, 110);
            this.rdoOwnKind.Name = "rdoOwnKind";
            this.rdoOwnKind.Size = new System.Drawing.Size(99, 17);
            this.rdoOwnKind.TabIndex = 4;
            this.rdoOwnKind.Text = "Ownership Kind";
            this.rdoOwnKind.UseVisualStyleBackColor = true;
            this.rdoOwnKind.Click += new System.EventHandler(this.rdoOwnKind_Click);
            // 
            // rdoDist
            // 
            this.rdoDist.AutoSize = true;
            this.rdoDist.Location = new System.Drawing.Point(25, 133);
            this.rdoDist.Name = "rdoDist";
            this.rdoDist.Size = new System.Drawing.Size(57, 17);
            this.rdoDist.TabIndex = 5;
            this.rdoDist.Text = "District";
            this.rdoDist.UseVisualStyleBackColor = true;
            this.rdoDist.Click += new System.EventHandler(this.rdoDist_Click);
            // 
            // rdoBrgy
            // 
            this.rdoBrgy.AutoSize = true;
            this.rdoBrgy.Checked = true;
            this.rdoBrgy.Location = new System.Drawing.Point(25, 41);
            this.rdoBrgy.Name = "rdoBrgy";
            this.rdoBrgy.Size = new System.Drawing.Size(70, 17);
            this.rdoBrgy.TabIndex = 1;
            this.rdoBrgy.TabStop = true;
            this.rdoBrgy.Text = "Barangay";
            this.rdoBrgy.UseVisualStyleBackColor = true;
            this.rdoBrgy.Click += new System.EventHandler(this.rdoBrgy_Click);
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(8, 6);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader1.Size = new System.Drawing.Size(165, 24);
            this.kryptonHeader1.TabIndex = 122;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Group By";
            this.kryptonHeader1.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader1.Values.Image")));
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(8, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(165, 252);
            this.label6.TabIndex = 132;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label9.Location = new System.Drawing.Point(4, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(165, 253);
            this.label9.TabIndex = 133;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(183, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(317, 252);
            this.label7.TabIndex = 110;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label8.Location = new System.Drawing.Point(179, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(317, 253);
            this.label8.TabIndex = 135;
            // 
            // GB1
            // 
            this.GB1.Controls.Add(this.cmbTaxYear);
            this.GB1.Location = new System.Drawing.Point(20, 199);
            this.GB1.Name = "GB1";
            this.GB1.Size = new System.Drawing.Size(142, 51);
            this.GB1.TabIndex = 136;
            this.GB1.TabStop = false;
            this.GB1.Text = "Tax Year";
            // 
            // cmbTaxYear
            // 
            this.cmbTaxYear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbTaxYear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbTaxYear.Enabled = false;
            this.cmbTaxYear.FormattingEnabled = true;
            this.cmbTaxYear.Location = new System.Drawing.Point(13, 20);
            this.cmbTaxYear.Name = "cmbTaxYear";
            this.cmbTaxYear.Size = new System.Drawing.Size(116, 21);
            this.cmbTaxYear.TabIndex = 8;
            // 
            // frmCollectibles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 271);
            this.Controls.Add(this.GB1);
            this.Controls.Add(this.chkFilter);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.txtTop);
            this.Controls.Add(this.cmbBusStat);
            this.Controls.Add(this.cmbOrgnKind);
            this.Controls.Add(this.cmbLineBus);
            this.Controls.Add(this.cmbBusType);
            this.Controls.Add(this.cmbBrgy);
            this.Controls.Add(this.cmbDist);
            this.Controls.Add(this.lblTop);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTeller);
            this.Controls.Add(this.rdoTopCollectibles);
            this.Controls.Add(this.rdoBusStat);
            this.Controls.Add(this.rdoMainBus);
            this.Controls.Add(this.rdoOwnKind);
            this.Controls.Add(this.rdoDist);
            this.Controls.Add(this.rdoBrgy);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmCollectibles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Collectibles";
            this.Load += new System.EventHandler(this.frmCollectibles_Load);
            this.GB1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkFilter;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private System.Windows.Forms.TextBox txtTop;
        private System.Windows.Forms.ComboBox cmbBusStat;
        private System.Windows.Forms.ComboBox cmbOrgnKind;
        private System.Windows.Forms.ComboBox cmbLineBus;
        private System.Windows.Forms.ComboBox cmbBusType;
        private System.Windows.Forms.ComboBox cmbBrgy;
        private System.Windows.Forms.ComboBox cmbDist;
        private System.Windows.Forms.Label lblTop;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTeller;
        private System.Windows.Forms.RadioButton rdoTopCollectibles;
        private System.Windows.Forms.RadioButton rdoBusStat;
        private System.Windows.Forms.RadioButton rdoMainBus;
        private System.Windows.Forms.RadioButton rdoOwnKind;
        private System.Windows.Forms.RadioButton rdoDist;
        private System.Windows.Forms.RadioButton rdoBrgy;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox GB1;
        private System.Windows.Forms.ComboBox cmbTaxYear;
    }
}

