namespace Amellar.Modules.BusinessReports
{
    partial class frmBussList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBussList));
            this.rdoBrgy = new System.Windows.Forms.RadioButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnOK = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbBrgy = new System.Windows.Forms.ComboBox();
            this.cmbOrgKind = new System.Windows.Forms.ComboBox();
            this.cmbBussStatus = new System.Windows.Forms.ComboBox();
            this.txtTaxYear = new System.Windows.Forms.TextBox();
            this.txtGrossFrom = new System.Windows.Forms.TextBox();
            this.txtGrossTo = new System.Windows.Forms.TextBox();
            this.cmbDistName = new System.Windows.Forms.ComboBox();
            this.cmbBussType = new System.Windows.Forms.ComboBox();
            this.cmbNatureBuss = new System.Windows.Forms.ComboBox();
            this.rdoDummy = new System.Windows.Forms.RadioButton();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.rdoDst = new System.Windows.Forms.RadioButton();
            this.rdoMainBus = new System.Windows.Forms.RadioButton();
            this.rdoOwner = new System.Windows.Forms.RadioButton();
            this.rdoBusStat = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.rdoGross = new System.Windows.Forms.RadioButton();
            this.rdoPrmt = new System.Windows.Forms.RadioButton();
            this.rdoTopCap = new System.Windows.Forms.RadioButton();
            this.txtTopCap = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.lblTopCap = new System.Windows.Forms.Label();
            this.lblPercent = new System.Windows.Forms.Label();
            this.chkByPercentage = new System.Windows.Forms.CheckBox();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.SuspendLayout();
            // 
            // rdoBrgy
            // 
            this.rdoBrgy.AutoSize = true;
            this.rdoBrgy.Location = new System.Drawing.Point(19, 35);
            this.rdoBrgy.Name = "rdoBrgy";
            this.rdoBrgy.Size = new System.Drawing.Size(70, 17);
            this.rdoBrgy.TabIndex = 1;
            this.rdoBrgy.Text = "Barangay";
            this.rdoBrgy.UseVisualStyleBackColor = true;
            this.rdoBrgy.Click += new System.EventHandler(this.rdoBrgy_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(428, 277);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(85, 25);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(332, 277);
            this.btnOK.Name = "btnOK";
            this.btnOK.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOK.Size = new System.Drawing.Size(90, 25);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "Generate";
            this.btnOK.Values.ExtraText = "";
            this.btnOK.Values.Image = null;
            this.btnOK.Values.ImageStates.ImageCheckedNormal = null;
            this.btnOK.Values.ImageStates.ImageCheckedPressed = null;
            this.btnOK.Values.ImageStates.ImageCheckedTracking = null;
            this.btnOK.Values.Text = "Generate";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(160, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Barangay Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(160, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "District Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(160, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Business Type";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(160, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Organization Kind";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(160, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Nature of Business";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(160, 158);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Business Status";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(394, 158);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Tax Year";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(160, 184);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Gross Receipts";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(261, 184);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "From";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(261, 210);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(20, 13);
            this.label11.TabIndex = 19;
            this.label11.Text = "To";
            // 
            // cmbBrgy
            // 
            this.cmbBrgy.Enabled = false;
            this.cmbBrgy.FormattingEnabled = true;
            this.cmbBrgy.Location = new System.Drawing.Point(264, 20);
            this.cmbBrgy.Name = "cmbBrgy";
            this.cmbBrgy.Size = new System.Drawing.Size(251, 21);
            this.cmbBrgy.TabIndex = 11;
            this.cmbBrgy.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbBrgy_KeyPress);
            // 
            // cmbOrgKind
            // 
            this.cmbOrgKind.Enabled = false;
            this.cmbOrgKind.FormattingEnabled = true;
            this.cmbOrgKind.Items.AddRange(new object[] {
            "ALL",
            "SINGLE PROPRIETORSHIP",
            "PARTNERSHIP",
            "CORPORATION",
            "COOPERATIVE"});
            this.cmbOrgKind.Location = new System.Drawing.Point(264, 123);
            this.cmbOrgKind.Name = "cmbOrgKind";
            this.cmbOrgKind.Size = new System.Drawing.Size(251, 21);
            this.cmbOrgKind.TabIndex = 15;
            this.cmbOrgKind.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbBrgy_KeyPress);
            // 
            // cmbBussStatus
            // 
            this.cmbBussStatus.Enabled = false;
            this.cmbBussStatus.FormattingEnabled = true;
            this.cmbBussStatus.Items.AddRange(new object[] {
            "ALL",
            "REN",
            "NEW",
            "RET"});
            this.cmbBussStatus.Location = new System.Drawing.Point(264, 150);
            this.cmbBussStatus.Name = "cmbBussStatus";
            this.cmbBussStatus.Size = new System.Drawing.Size(124, 21);
            this.cmbBussStatus.TabIndex = 16;
            this.cmbBussStatus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbBrgy_KeyPress);
            // 
            // txtTaxYear
            // 
            this.txtTaxYear.Location = new System.Drawing.Point(450, 151);
            this.txtTaxYear.Name = "txtTaxYear";
            this.txtTaxYear.Size = new System.Drawing.Size(65, 20);
            this.txtTaxYear.TabIndex = 17;
            this.txtTaxYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTaxYear_KeyPress);
            // 
            // txtGrossFrom
            // 
            this.txtGrossFrom.Enabled = false;
            this.txtGrossFrom.Location = new System.Drawing.Point(309, 177);
            this.txtGrossFrom.Name = "txtGrossFrom";
            this.txtGrossFrom.Size = new System.Drawing.Size(206, 20);
            this.txtGrossFrom.TabIndex = 18;
            this.txtGrossFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtGrossFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTaxYear_KeyPress);
            // 
            // txtGrossTo
            // 
            this.txtGrossTo.Enabled = false;
            this.txtGrossTo.Location = new System.Drawing.Point(309, 203);
            this.txtGrossTo.Name = "txtGrossTo";
            this.txtGrossTo.Size = new System.Drawing.Size(206, 20);
            this.txtGrossTo.TabIndex = 19;
            this.txtGrossTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtGrossTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTaxYear_KeyPress);
            // 
            // cmbDistName
            // 
            this.cmbDistName.Enabled = false;
            this.cmbDistName.FormattingEnabled = true;
            this.cmbDistName.Location = new System.Drawing.Point(264, 44);
            this.cmbDistName.Name = "cmbDistName";
            this.cmbDistName.Size = new System.Drawing.Size(251, 21);
            this.cmbDistName.TabIndex = 11;
            this.cmbDistName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbBrgy_KeyPress);
            // 
            // cmbBussType
            // 
            this.cmbBussType.Enabled = false;
            this.cmbBussType.FormattingEnabled = true;
            this.cmbBussType.Location = new System.Drawing.Point(264, 69);
            this.cmbBussType.Name = "cmbBussType";
            this.cmbBussType.Size = new System.Drawing.Size(251, 21);
            this.cmbBussType.TabIndex = 11;
            this.cmbBussType.SelectedIndexChanged += new System.EventHandler(this.cmbBussType_SelectedIndexChanged);
            this.cmbBussType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbBrgy_KeyPress);
            // 
            // cmbNatureBuss
            // 
            this.cmbNatureBuss.Enabled = false;
            this.cmbNatureBuss.FormattingEnabled = true;
            this.cmbNatureBuss.Location = new System.Drawing.Point(264, 96);
            this.cmbNatureBuss.Name = "cmbNatureBuss";
            this.cmbNatureBuss.Size = new System.Drawing.Size(251, 21);
            this.cmbNatureBuss.TabIndex = 11;
            this.cmbNatureBuss.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbBrgy_KeyPress);
            this.cmbNatureBuss.DropDown += new System.EventHandler(this.cmbNatureBuss_DropDown);
            // 
            // rdoDummy
            // 
            this.rdoDummy.AutoSize = true;
            this.rdoDummy.Location = new System.Drawing.Point(95, 37);
            this.rdoDummy.Name = "rdoDummy";
            this.rdoDummy.Size = new System.Drawing.Size(14, 13);
            this.rdoDummy.TabIndex = 20;
            this.rdoDummy.UseVisualStyleBackColor = true;
            this.rdoDummy.Visible = false;
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(8, 7);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader1.Size = new System.Drawing.Size(129, 24);
            this.kryptonHeader1.TabIndex = 2;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "";
            this.kryptonHeader1.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader1.Values.Image")));
            // 
            // rdoDst
            // 
            this.rdoDst.AutoSize = true;
            this.rdoDst.Location = new System.Drawing.Point(19, 58);
            this.rdoDst.Name = "rdoDst";
            this.rdoDst.Size = new System.Drawing.Size(57, 17);
            this.rdoDst.TabIndex = 2;
            this.rdoDst.Text = "District";
            this.rdoDst.UseVisualStyleBackColor = true;
            this.rdoDst.Click += new System.EventHandler(this.rdoDst_Click);
            // 
            // rdoMainBus
            // 
            this.rdoMainBus.AutoSize = true;
            this.rdoMainBus.Location = new System.Drawing.Point(19, 81);
            this.rdoMainBus.Name = "rdoMainBus";
            this.rdoMainBus.Size = new System.Drawing.Size(93, 17);
            this.rdoMainBus.TabIndex = 3;
            this.rdoMainBus.Text = "Main Business";
            this.rdoMainBus.UseVisualStyleBackColor = true;
            this.rdoMainBus.Click += new System.EventHandler(this.rdoMainBuss_Click);
            // 
            // rdoOwner
            // 
            this.rdoOwner.AutoSize = true;
            this.rdoOwner.Location = new System.Drawing.Point(19, 104);
            this.rdoOwner.Name = "rdoOwner";
            this.rdoOwner.Size = new System.Drawing.Size(99, 17);
            this.rdoOwner.TabIndex = 4;
            this.rdoOwner.Text = "Ownership Kind";
            this.rdoOwner.UseVisualStyleBackColor = true;
            this.rdoOwner.Click += new System.EventHandler(this.rdoOwner_Click);
            // 
            // rdoBusStat
            // 
            this.rdoBusStat.AutoSize = true;
            this.rdoBusStat.Location = new System.Drawing.Point(19, 127);
            this.rdoBusStat.Name = "rdoBusStat";
            this.rdoBusStat.Size = new System.Drawing.Size(100, 17);
            this.rdoBusStat.TabIndex = 5;
            this.rdoBusStat.Text = "Business Status";
            this.rdoBusStat.UseVisualStyleBackColor = true;
            this.rdoBusStat.Click += new System.EventHandler(this.rdoBussStat_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(16, 200);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 3);
            this.label1.TabIndex = 5;
            this.label1.Text = "label1";
            // 
            // rdoGross
            // 
            this.rdoGross.AutoSize = true;
            this.rdoGross.Location = new System.Drawing.Point(19, 149);
            this.rdoGross.Name = "rdoGross";
            this.rdoGross.Size = new System.Drawing.Size(97, 17);
            this.rdoGross.TabIndex = 6;
            this.rdoGross.Text = "Gross Receipts";
            this.rdoGross.UseVisualStyleBackColor = true;
            this.rdoGross.Click += new System.EventHandler(this.rdoGross_Click);
            // 
            // rdoPrmt
            // 
            this.rdoPrmt.AutoSize = true;
            this.rdoPrmt.Location = new System.Drawing.Point(19, 209);
            this.rdoPrmt.Name = "rdoPrmt";
            this.rdoPrmt.Size = new System.Drawing.Size(94, 17);
            this.rdoPrmt.TabIndex = 7;
            this.rdoPrmt.Text = "Permit Number";
            this.rdoPrmt.UseVisualStyleBackColor = true;
            this.rdoPrmt.Visible = false;
            this.rdoPrmt.Click += new System.EventHandler(this.rdoPrmt_Click);
            // 
            // rdoTopCap
            // 
            this.rdoTopCap.AutoSize = true;
            this.rdoTopCap.Location = new System.Drawing.Point(19, 166);
            this.rdoTopCap.Name = "rdoTopCap";
            this.rdoTopCap.Size = new System.Drawing.Size(82, 30);
            this.rdoTopCap.TabIndex = 21;
            this.rdoTopCap.Text = "Top Biggest\r\nInvestments";
            this.rdoTopCap.UseVisualStyleBackColor = true;
            this.rdoTopCap.Click += new System.EventHandler(this.rdoTopCap_Click);
            // 
            // txtTopCap
            // 
            this.txtTopCap.Enabled = false;
            this.txtTopCap.Location = new System.Drawing.Point(264, 229);
            this.txtTopCap.Name = "txtTopCap";
            this.txtTopCap.Size = new System.Drawing.Size(55, 20);
            this.txtTopCap.TabIndex = 22;
            this.txtTopCap.Text = "0";
            this.txtTopCap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTopCap
            // 
            this.lblTopCap.AutoSize = true;
            this.lblTopCap.Location = new System.Drawing.Point(160, 232);
            this.lblTopCap.Name = "lblTopCap";
            this.lblTopCap.Size = new System.Drawing.Size(61, 13);
            this.lblTopCap.TabIndex = 23;
            this.lblTopCap.Text = "Top Capital";
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPercent.Location = new System.Drawing.Point(321, 232);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(16, 13);
            this.lblPercent.TabIndex = 24;
            this.lblPercent.Text = "%";
            // 
            // chkByPercentage
            // 
            this.chkByPercentage.AutoSize = true;
            this.chkByPercentage.Location = new System.Drawing.Point(343, 231);
            this.chkByPercentage.Name = "chkByPercentage";
            this.chkByPercentage.Size = new System.Drawing.Size(96, 17);
            this.chkByPercentage.TabIndex = 25;
            this.chkByPercentage.Text = "By Percentage";
            this.chkByPercentage.UseVisualStyleBackColor = true;
            this.chkByPercentage.CheckedChanged += new System.EventHandler(this.chkByPercentage_CheckedChanged);
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(5, 4);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(132, 266);
            this.containerWithShadow1.TabIndex = 2;
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(143, 4);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(383, 266);
            this.containerWithShadow2.TabIndex = 6;
            // 
            // frmBussList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(538, 312);
            this.Controls.Add(this.chkByPercentage);
            this.Controls.Add(this.lblPercent);
            this.Controls.Add(this.lblTopCap);
            this.Controls.Add(this.txtTopCap);
            this.Controls.Add(this.rdoTopCap);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtTaxYear);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.rdoDummy);
            this.Controls.Add(this.txtGrossTo);
            this.Controls.Add(this.txtGrossFrom);
            this.Controls.Add(this.cmbOrgKind);
            this.Controls.Add(this.cmbBussStatus);
            this.Controls.Add(this.cmbNatureBuss);
            this.Controls.Add(this.cmbBussType);
            this.Controls.Add(this.cmbDistName);
            this.Controls.Add(this.cmbBrgy);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rdoPrmt);
            this.Controls.Add(this.rdoGross);
            this.Controls.Add(this.rdoBusStat);
            this.Controls.Add(this.rdoOwner);
            this.Controls.Add(this.rdoMainBus);
            this.Controls.Add(this.rdoDst);
            this.Controls.Add(this.rdoBrgy);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.containerWithShadow1);
            this.Controls.Add(this.containerWithShadow2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBussList";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "List of Businesses";
            this.Load += new System.EventHandler(this.frmBussList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoBrgy;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbBrgy;
        private System.Windows.Forms.ComboBox cmbOrgKind;
        private System.Windows.Forms.ComboBox cmbBussStatus;
        private System.Windows.Forms.TextBox txtTaxYear;
        private System.Windows.Forms.TextBox txtGrossFrom;
        private System.Windows.Forms.TextBox txtGrossTo;
        private System.Windows.Forms.ComboBox cmbDistName;
        private System.Windows.Forms.ComboBox cmbBussType;
        private System.Windows.Forms.ComboBox cmbNatureBuss;
        private System.Windows.Forms.RadioButton rdoDummy;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private System.Windows.Forms.RadioButton rdoDst;
        private System.Windows.Forms.RadioButton rdoMainBus;
        private System.Windows.Forms.RadioButton rdoOwner;
        private System.Windows.Forms.RadioButton rdoBusStat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdoGross;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.RadioButton rdoPrmt;
        private System.Windows.Forms.RadioButton rdoTopCap;
        public ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTopCap;
        private System.Windows.Forms.Label lblTopCap;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.CheckBox chkByPercentage;
    }
}

