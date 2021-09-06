namespace Amellar.Modules.LiquidationReports
{
    partial class frmSummaryofCollection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSummaryofCollection));
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.rdoMainBus = new System.Windows.Forms.RadioButton();
            this.rdoDist = new System.Windows.Forms.RadioButton();
            this.rdoBrgy = new System.Windows.Forms.RadioButton();
            this.rdoOwnKind = new System.Windows.Forms.RadioButton();
            this.rdoBusStat = new System.Windows.Forms.RadioButton();
            this.rdoLineBus = new System.Windows.Forms.RadioButton();
            this.cmbDist = new System.Windows.Forms.ComboBox();
            this.lblTeller = new System.Windows.Forms.Label();
            this.cmbBrgy = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbBusType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbLineBus = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbOrgnKind = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbBusStat = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.chkGenReport = new System.Windows.Forms.CheckBox();
            this.btnSim = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnStan = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSimDist = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnStanDist = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.chkGC = new System.Windows.Forms.CheckBox();
            this.chkStandardFormat = new System.Windows.Forms.CheckBox();
            this.kryptonHeader3 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.chkSimplifiedFormat = new System.Windows.Forms.CheckBox();
            this.frameWithShadow2 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow1 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow3 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow4 = new Amellar.Common.SOA.FrameWithShadow();
            this.chkFilterReport = new System.Windows.Forms.CheckBox();
            this.rdoTotalCol = new System.Windows.Forms.RadioButton();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.lblYear = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(10, 6);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader1.Size = new System.Drawing.Size(165, 24);
            this.kryptonHeader1.TabIndex = 22;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Group By";
            this.kryptonHeader1.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader1.Values.Image")));
            // 
            // rdoMainBus
            // 
            this.rdoMainBus.AutoSize = true;
            this.rdoMainBus.Location = new System.Drawing.Point(18, 79);
            this.rdoMainBus.Name = "rdoMainBus";
            this.rdoMainBus.Size = new System.Drawing.Size(93, 17);
            this.rdoMainBus.TabIndex = 1;
            this.rdoMainBus.Text = "Main Business";
            this.rdoMainBus.UseVisualStyleBackColor = true;
            this.rdoMainBus.Click += new System.EventHandler(this.rdoMainBus_Click);
            // 
            // rdoDist
            // 
            this.rdoDist.AutoSize = true;
            this.rdoDist.Enabled = false;
            this.rdoDist.Location = new System.Drawing.Point(18, 55);
            this.rdoDist.Name = "rdoDist";
            this.rdoDist.Size = new System.Drawing.Size(57, 17);
            this.rdoDist.TabIndex = 25;
            this.rdoDist.Text = "District";
            this.rdoDist.UseVisualStyleBackColor = true;
            this.rdoDist.Click += new System.EventHandler(this.rdoDist_Click);
            // 
            // rdoBrgy
            // 
            this.rdoBrgy.AutoSize = true;
            this.rdoBrgy.Checked = true;
            this.rdoBrgy.Location = new System.Drawing.Point(18, 33);
            this.rdoBrgy.Name = "rdoBrgy";
            this.rdoBrgy.Size = new System.Drawing.Size(70, 17);
            this.rdoBrgy.TabIndex = 0;
            this.rdoBrgy.TabStop = true;
            this.rdoBrgy.Text = "Barangay";
            this.rdoBrgy.UseVisualStyleBackColor = true;
            this.rdoBrgy.Click += new System.EventHandler(this.rdoBrgy_Click);
            this.rdoBrgy.CheckedChanged += new System.EventHandler(this.rdoBrgy_CheckedChanged);
            // 
            // rdoOwnKind
            // 
            this.rdoOwnKind.AutoSize = true;
            this.rdoOwnKind.Location = new System.Drawing.Point(18, 103);
            this.rdoOwnKind.Name = "rdoOwnKind";
            this.rdoOwnKind.Size = new System.Drawing.Size(99, 17);
            this.rdoOwnKind.TabIndex = 2;
            this.rdoOwnKind.Text = "Ownership Kind";
            this.rdoOwnKind.UseVisualStyleBackColor = true;
            this.rdoOwnKind.Click += new System.EventHandler(this.rdoOwnKind_Click);
            // 
            // rdoBusStat
            // 
            this.rdoBusStat.AutoSize = true;
            this.rdoBusStat.Location = new System.Drawing.Point(18, 128);
            this.rdoBusStat.Name = "rdoBusStat";
            this.rdoBusStat.Size = new System.Drawing.Size(100, 17);
            this.rdoBusStat.TabIndex = 3;
            this.rdoBusStat.Text = "Business Status";
            this.rdoBusStat.UseVisualStyleBackColor = true;
            this.rdoBusStat.Click += new System.EventHandler(this.rdoBusStat_Click);
            // 
            // rdoLineBus
            // 
            this.rdoLineBus.AutoSize = true;
            this.rdoLineBus.Location = new System.Drawing.Point(18, 152);
            this.rdoLineBus.Name = "rdoLineBus";
            this.rdoLineBus.Size = new System.Drawing.Size(102, 17);
            this.rdoLineBus.TabIndex = 4;
            this.rdoLineBus.Text = "Line of Business";
            this.rdoLineBus.UseVisualStyleBackColor = true;
            this.rdoLineBus.Click += new System.EventHandler(this.rdoLineBus_Click);
            this.rdoLineBus.CheckedChanged += new System.EventHandler(this.rdoLineBus_CheckedChanged);
            // 
            // cmbDist
            // 
            this.cmbDist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbDist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDist.Enabled = false;
            this.cmbDist.FormattingEnabled = true;
            this.cmbDist.Location = new System.Drawing.Point(288, 35);
            this.cmbDist.Name = "cmbDist";
            this.cmbDist.Size = new System.Drawing.Size(169, 21);
            this.cmbDist.TabIndex = 6;
            this.cmbDist.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbDist_KeyPress);
            // 
            // lblTeller
            // 
            this.lblTeller.AutoSize = true;
            this.lblTeller.Location = new System.Drawing.Point(197, 40);
            this.lblTeller.Name = "lblTeller";
            this.lblTeller.Size = new System.Drawing.Size(70, 13);
            this.lblTeller.TabIndex = 60;
            this.lblTeller.Text = "District Name";
            // 
            // cmbBrgy
            // 
            this.cmbBrgy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbBrgy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBrgy.Enabled = false;
            this.cmbBrgy.FormattingEnabled = true;
            this.cmbBrgy.Location = new System.Drawing.Point(288, 60);
            this.cmbBrgy.Name = "cmbBrgy";
            this.cmbBrgy.Size = new System.Drawing.Size(169, 21);
            this.cmbBrgy.TabIndex = 7;
            this.cmbBrgy.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbDist_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(197, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 62;
            this.label1.Text = "Barangay Name";
            // 
            // cmbBusType
            // 
            this.cmbBusType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbBusType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBusType.Enabled = false;
            this.cmbBusType.FormattingEnabled = true;
            this.cmbBusType.Location = new System.Drawing.Point(288, 85);
            this.cmbBusType.Name = "cmbBusType";
            this.cmbBusType.Size = new System.Drawing.Size(169, 21);
            this.cmbBusType.TabIndex = 8;
            this.cmbBusType.SelectedIndexChanged += new System.EventHandler(this.cmbBusType_SelectedIndexChanged);
            this.cmbBusType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbDist_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(197, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 64;
            this.label2.Text = "Business Type";
            // 
            // cmbLineBus
            // 
            this.cmbLineBus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbLineBus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbLineBus.Enabled = false;
            this.cmbLineBus.FormattingEnabled = true;
            this.cmbLineBus.Location = new System.Drawing.Point(288, 110);
            this.cmbLineBus.Name = "cmbLineBus";
            this.cmbLineBus.Size = new System.Drawing.Size(169, 21);
            this.cmbLineBus.TabIndex = 9;
            this.cmbLineBus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbDist_KeyPress);
            this.cmbLineBus.DropDown += new System.EventHandler(this.cmbLineBus_DropDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(197, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 66;
            this.label3.Text = "Line of Business";
            // 
            // cmbOrgnKind
            // 
            this.cmbOrgnKind.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbOrgnKind.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbOrgnKind.Enabled = false;
            this.cmbOrgnKind.FormattingEnabled = true;
            this.cmbOrgnKind.Items.AddRange(new object[] {
            "ALL",
            "SINGLE PROPRIETORSHIP",
            "PARTNERSHIP",
            "CORPORATION",
            "COOPERATIVE"});
            this.cmbOrgnKind.Location = new System.Drawing.Point(288, 135);
            this.cmbOrgnKind.Name = "cmbOrgnKind";
            this.cmbOrgnKind.Size = new System.Drawing.Size(169, 21);
            this.cmbOrgnKind.TabIndex = 10;
            this.cmbOrgnKind.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbDist_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(197, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 68;
            this.label4.Text = "Orgn. Kind";
            // 
            // cmbBusStat
            // 
            this.cmbBusStat.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbBusStat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBusStat.Enabled = false;
            this.cmbBusStat.FormattingEnabled = true;
            this.cmbBusStat.Items.AddRange(new object[] {
            "ALL",
            "NEW",
            "REN",
            "RET"});
            this.cmbBusStat.Location = new System.Drawing.Point(288, 160);
            this.cmbBusStat.Name = "cmbBusStat";
            this.cmbBusStat.Size = new System.Drawing.Size(169, 21);
            this.cmbBusStat.TabIndex = 11;
            this.cmbBusStat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbDist_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(197, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 70;
            this.label5.Text = "Business Status";
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(171, 245);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(79, 20);
            this.dtpTo.TabIndex = 13;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(64, 245);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(79, 20);
            this.dtpFrom.TabIndex = 12;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(146, 249);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 74;
            this.label6.Text = "To";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 249);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 73;
            this.label7.Text = "From";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(376, 285);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(73, 25);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(297, 285);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(73, 25);
            this.btnGenerate.TabIndex = 15;
            this.btnGenerate.Values.Text = "&Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // chkGenReport
            // 
            this.chkGenReport.AutoSize = true;
            this.chkGenReport.Location = new System.Drawing.Point(34, 272);
            this.chkGenReport.Name = "chkGenReport";
            this.chkGenReport.Size = new System.Drawing.Size(202, 17);
            this.chkGenReport.TabIndex = 14;
            this.chkGenReport.Text = "Select format using generated reports";
            this.chkGenReport.UseVisualStyleBackColor = true;
            // 
            // btnSim
            // 
            this.btnSim.Location = new System.Drawing.Point(127, 335);
            this.btnSim.Name = "btnSim";
            this.btnSim.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSim.Size = new System.Drawing.Size(115, 25);
            this.btnSim.TabIndex = 80;
            this.btnSim.TabStop = false;
            this.btnSim.Values.Text = "Simplified";
            this.btnSim.Click += new System.EventHandler(this.btnSim_Click);
            // 
            // btnStan
            // 
            this.btnStan.Location = new System.Drawing.Point(10, 357);
            this.btnStan.Name = "btnStan";
            this.btnStan.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnStan.Size = new System.Drawing.Size(115, 25);
            this.btnStan.TabIndex = 79;
            this.btnStan.TabStop = false;
            this.btnStan.Values.Text = "Standard";
            this.btnStan.Click += new System.EventHandler(this.btnStan_Click);
            // 
            // btnSimDist
            // 
            this.btnSimDist.Location = new System.Drawing.Point(127, 361);
            this.btnSimDist.Name = "btnSimDist";
            this.btnSimDist.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSimDist.Size = new System.Drawing.Size(115, 25);
            this.btnSimDist.TabIndex = 82;
            this.btnSimDist.TabStop = false;
            this.btnSimDist.Values.Text = "Simplified-District";
            this.btnSimDist.Click += new System.EventHandler(this.btnSimDist_Click);
            // 
            // btnStanDist
            // 
            this.btnStanDist.Location = new System.Drawing.Point(10, 362);
            this.btnStanDist.Name = "btnStanDist";
            this.btnStanDist.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnStanDist.Size = new System.Drawing.Size(115, 25);
            this.btnStanDist.TabIndex = 81;
            this.btnStanDist.TabStop = false;
            this.btnStanDist.Values.Text = "Standard-District";
            this.btnStanDist.Click += new System.EventHandler(this.btnStanDist_Click);
            // 
            // kryptonHeader2
            // 
            this.kryptonHeader2.AutoSize = false;
            this.kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader2.Location = new System.Drawing.Point(10, 212);
            this.kryptonHeader2.Name = "kryptonHeader2";
            this.kryptonHeader2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader2.Size = new System.Drawing.Size(257, 24);
            this.kryptonHeader2.TabIndex = 84;
            this.kryptonHeader2.Values.Description = "";
            this.kryptonHeader2.Values.Heading = "Covered Period";
            this.kryptonHeader2.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader2.Values.Image")));
            // 
            // chkGC
            // 
            this.chkGC.AutoSize = true;
            this.chkGC.Location = new System.Drawing.Point(34, 293);
            this.chkGC.Name = "chkGC";
            this.chkGC.Size = new System.Drawing.Size(171, 17);
            this.chkGC.TabIndex = 14;
            this.chkGC.Text = "Show Gross Receipts / Capital";
            this.chkGC.UseVisualStyleBackColor = true;
            this.chkGC.Visible = false;
            // 
            // chkStandardFormat
            // 
            this.chkStandardFormat.AutoSize = true;
            this.chkStandardFormat.Location = new System.Drawing.Point(303, 244);
            this.chkStandardFormat.Name = "chkStandardFormat";
            this.chkStandardFormat.Size = new System.Drawing.Size(69, 17);
            this.chkStandardFormat.TabIndex = 85;
            this.chkStandardFormat.Text = "Standard";
            this.chkStandardFormat.UseVisualStyleBackColor = true;
            this.chkStandardFormat.CheckedChanged += new System.EventHandler(this.chkStandardFormat_CheckedChanged);
            // 
            // kryptonHeader3
            // 
            this.kryptonHeader3.AutoSize = false;
            this.kryptonHeader3.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader3.Location = new System.Drawing.Point(277, 212);
            this.kryptonHeader3.Name = "kryptonHeader3";
            this.kryptonHeader3.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader3.Size = new System.Drawing.Size(192, 24);
            this.kryptonHeader3.TabIndex = 87;
            this.kryptonHeader3.Values.Description = "";
            this.kryptonHeader3.Values.Heading = "Report Format";
            this.kryptonHeader3.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader3.Values.Image")));
            // 
            // chkSimplifiedFormat
            // 
            this.chkSimplifiedFormat.AutoSize = true;
            this.chkSimplifiedFormat.Location = new System.Drawing.Point(382, 244);
            this.chkSimplifiedFormat.Name = "chkSimplifiedFormat";
            this.chkSimplifiedFormat.Size = new System.Drawing.Size(70, 17);
            this.chkSimplifiedFormat.TabIndex = 88;
            this.chkSimplifiedFormat.Text = "Simplified";
            this.chkSimplifiedFormat.UseVisualStyleBackColor = true;
            this.chkSimplifiedFormat.CheckedChanged += new System.EventHandler(this.chkSimplifiedFormat_CheckedChanged);
            // 
            // frameWithShadow2
            // 
            this.frameWithShadow2.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow2.Location = new System.Drawing.Point(179, 6);
            this.frameWithShadow2.Name = "frameWithShadow2";
            this.frameWithShadow2.Size = new System.Drawing.Size(292, 200);
            this.frameWithShadow2.TabIndex = 58;
            // 
            // frameWithShadow1
            // 
            this.frameWithShadow1.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow1.Location = new System.Drawing.Point(5, 6);
            this.frameWithShadow1.Name = "frameWithShadow1";
            this.frameWithShadow1.Size = new System.Drawing.Size(172, 200);
            this.frameWithShadow1.TabIndex = 21;
            // 
            // frameWithShadow3
            // 
            this.frameWithShadow3.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow3.Location = new System.Drawing.Point(5, 212);
            this.frameWithShadow3.Name = "frameWithShadow3";
            this.frameWithShadow3.Size = new System.Drawing.Size(264, 111);
            this.frameWithShadow3.TabIndex = 83;
            // 
            // frameWithShadow4
            // 
            this.frameWithShadow4.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow4.Location = new System.Drawing.Point(273, 212);
            this.frameWithShadow4.Name = "frameWithShadow4";
            this.frameWithShadow4.Size = new System.Drawing.Size(198, 111);
            this.frameWithShadow4.TabIndex = 86;
            // 
            // chkFilterReport
            // 
            this.chkFilterReport.AutoSize = true;
            this.chkFilterReport.Location = new System.Drawing.Point(324, 265);
            this.chkFilterReport.Name = "chkFilterReport";
            this.chkFilterReport.Size = new System.Drawing.Size(109, 17);
            this.chkFilterReport.TabIndex = 89;
            this.chkFilterReport.Text = "Filter Report View";
            this.chkFilterReport.UseVisualStyleBackColor = true;
            // 
            // rdoTotalCol
            // 
            this.rdoTotalCol.AutoSize = true;
            this.rdoTotalCol.Location = new System.Drawing.Point(18, 176);
            this.rdoTotalCol.Name = "rdoTotalCol";
            this.rdoTotalCol.Size = new System.Drawing.Size(103, 17);
            this.rdoTotalCol.TabIndex = 90;
            this.rdoTotalCol.Text = "Total Collections";
            this.rdoTotalCol.UseVisualStyleBackColor = true;
            this.rdoTotalCol.Click += new System.EventHandler(this.rdoTotalCol_Click);
            this.rdoTotalCol.CheckedChanged += new System.EventHandler(this.rdoTotalCol_CheckedChanged_1);
            // 
            // cmbYear
            // 
            this.cmbYear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbYear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbYear.DropDownHeight = 90;
            this.cmbYear.Enabled = false;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.IntegralHeight = false;
            this.cmbYear.ItemHeight = 13;
            this.cmbYear.Location = new System.Drawing.Point(84, 245);
            this.cmbYear.MaximumSize = new System.Drawing.Size(169, 0);
            this.cmbYear.MinimumSize = new System.Drawing.Size(169, 0);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(169, 21);
            this.cmbYear.TabIndex = 91;
            this.cmbYear.Text = " ";
            this.cmbYear.Visible = false;
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(31, 248);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(50, 13);
            this.lblYear.TabIndex = 92;
            this.lblYear.Text = "Tax Year";
            this.lblYear.Visible = false;
            // 
            // frmSummaryofCollection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 329);
            this.Controls.Add(this.lblYear);
            this.Controls.Add(this.cmbYear);
            this.Controls.Add(this.rdoTotalCol);
            this.Controls.Add(this.chkFilterReport);
            this.Controls.Add(this.chkSimplifiedFormat);
            this.Controls.Add(this.kryptonHeader3);
            this.Controls.Add(this.chkStandardFormat);
            this.Controls.Add(this.kryptonHeader2);
            this.Controls.Add(this.btnSimDist);
            this.Controls.Add(this.btnStanDist);
            this.Controls.Add(this.btnSim);
            this.Controls.Add(this.btnStan);
            this.Controls.Add(this.chkGC);
            this.Controls.Add(this.chkGenReport);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbBusStat);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbOrgnKind);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbLineBus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbBusType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbBrgy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDist);
            this.Controls.Add(this.lblTeller);
            this.Controls.Add(this.frameWithShadow2);
            this.Controls.Add(this.rdoLineBus);
            this.Controls.Add(this.rdoBusStat);
            this.Controls.Add(this.rdoMainBus);
            this.Controls.Add(this.rdoOwnKind);
            this.Controls.Add(this.rdoDist);
            this.Controls.Add(this.rdoBrgy);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.frameWithShadow1);
            this.Controls.Add(this.frameWithShadow3);
            this.Controls.Add(this.frameWithShadow4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(481, 357);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(481, 357);
            this.Name = "frmSummaryofCollection";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Summary of Collection";
            this.Load += new System.EventHandler(this.frmSummaryofCollection_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow1;
        private System.Windows.Forms.RadioButton rdoMainBus;
        private System.Windows.Forms.RadioButton rdoDist;
        private System.Windows.Forms.RadioButton rdoBrgy;
        private System.Windows.Forms.RadioButton rdoOwnKind;
        private System.Windows.Forms.RadioButton rdoBusStat;
        private System.Windows.Forms.RadioButton rdoLineBus;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow2;
        private System.Windows.Forms.ComboBox cmbDist;
        private System.Windows.Forms.Label lblTeller;
        private System.Windows.Forms.ComboBox cmbBrgy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBusType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbLineBus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbOrgnKind;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbBusStat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private System.Windows.Forms.CheckBox chkGenReport;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSim;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnStan;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSimDist;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnStanDist;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow3;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader2;
        private System.Windows.Forms.CheckBox chkGC;
        private System.Windows.Forms.CheckBox chkStandardFormat;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow4;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader3;
        private System.Windows.Forms.CheckBox chkSimplifiedFormat;
        private System.Windows.Forms.CheckBox chkFilterReport;
        private System.Windows.Forms.RadioButton rdoTotalCol;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Label lblYear;
    }
}