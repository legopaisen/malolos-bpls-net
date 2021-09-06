namespace BusinessRoll
{
    partial class frmBusinessRoll
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
            this.components = new System.ComponentModel.Container();
            this.grpGroupBy = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.rbTopAssessed = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rbName = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rbLeastPayers = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rbLeastGrosses = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rbTopPayers = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rbTopGrosses = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rbStreet = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rbLineOfBuss = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rbDistrict = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rbBarangay = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.hdrGroupBy = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.lblBarangay = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lblDistrict = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lblBussType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lblBussNature = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lblTopGrossPayer = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lblNameOfStreet = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.cboBarangay = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.cboDistrict = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.cboBussType = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.cboBussNature = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.txtGross = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.cboStatus = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.lblStatus = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.txtTaxYear = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.lblTaxYear = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.chkByPercentage = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cboStreet = new System.Windows.Forms.ComboBox();
            this.lblPercent = new System.Windows.Forms.Label();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            ((System.ComponentModel.ISupportInitialize)(this.grpGroupBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpGroupBy.Panel)).BeginInit();
            this.grpGroupBy.Panel.SuspendLayout();
            this.grpGroupBy.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpGroupBy
            // 
            this.grpGroupBy.Location = new System.Drawing.Point(8, 8);
            this.grpGroupBy.Name = "grpGroupBy";
            // 
            // grpGroupBy.Panel
            // 
            this.grpGroupBy.Panel.Controls.Add(this.rbTopAssessed);
            this.grpGroupBy.Panel.Controls.Add(this.rbName);
            this.grpGroupBy.Panel.Controls.Add(this.rbLeastPayers);
            this.grpGroupBy.Panel.Controls.Add(this.rbLeastGrosses);
            this.grpGroupBy.Panel.Controls.Add(this.rbTopPayers);
            this.grpGroupBy.Panel.Controls.Add(this.rbTopGrosses);
            this.grpGroupBy.Panel.Controls.Add(this.rbStreet);
            this.grpGroupBy.Panel.Controls.Add(this.rbLineOfBuss);
            this.grpGroupBy.Panel.Controls.Add(this.rbDistrict);
            this.grpGroupBy.Panel.Controls.Add(this.rbBarangay);
            this.grpGroupBy.Panel.Controls.Add(this.hdrGroupBy);
            this.grpGroupBy.Size = new System.Drawing.Size(150, 270);
            this.grpGroupBy.TabIndex = 1;
            // 
            // rbTopAssessed
            // 
            this.rbTopAssessed.Location = new System.Drawing.Point(23, 211);
            this.rbTopAssessed.Name = "rbTopAssessed";
            this.rbTopAssessed.Size = new System.Drawing.Size(96, 20);
            this.rbTopAssessed.TabIndex = 3;
            this.rbTopAssessed.Text = "Top Assessed";
            this.rbTopAssessed.Values.ExtraText = "";
            this.rbTopAssessed.Values.Image = null;
            this.rbTopAssessed.Values.Text = "Top Assessed";
            this.rbTopAssessed.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // rbName
            // 
            this.rbName.Location = new System.Drawing.Point(23, 230);
            this.rbName.Name = "rbName";
            this.rbName.Size = new System.Drawing.Size(55, 20);
            this.rbName.TabIndex = 2;
            this.rbName.Text = "Name";
            this.rbName.Values.ExtraText = "";
            this.rbName.Values.Image = null;
            this.rbName.Values.Text = "Name";
            this.rbName.Visible = false;
            this.rbName.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // rbLeastPayers
            // 
            this.rbLeastPayers.Location = new System.Drawing.Point(23, 189);
            this.rbLeastPayers.Name = "rbLeastPayers";
            this.rbLeastPayers.Size = new System.Drawing.Size(89, 20);
            this.rbLeastPayers.TabIndex = 2;
            this.rbLeastPayers.Text = "Least Payers";
            this.rbLeastPayers.Values.ExtraText = "";
            this.rbLeastPayers.Values.Image = null;
            this.rbLeastPayers.Values.Text = "Least Payers";
            this.rbLeastPayers.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // rbLeastGrosses
            // 
            this.rbLeastGrosses.Location = new System.Drawing.Point(23, 166);
            this.rbLeastGrosses.Name = "rbLeastGrosses";
            this.rbLeastGrosses.Size = new System.Drawing.Size(96, 20);
            this.rbLeastGrosses.TabIndex = 2;
            this.rbLeastGrosses.Text = "Least Grosses";
            this.rbLeastGrosses.Values.ExtraText = "";
            this.rbLeastGrosses.Values.Image = null;
            this.rbLeastGrosses.Values.Text = "Least Grosses";
            this.rbLeastGrosses.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // rbTopPayers
            // 
            this.rbTopPayers.Location = new System.Drawing.Point(23, 143);
            this.rbTopPayers.Name = "rbTopPayers";
            this.rbTopPayers.Size = new System.Drawing.Size(82, 20);
            this.rbTopPayers.TabIndex = 2;
            this.rbTopPayers.Text = "Top Payers";
            this.rbTopPayers.Values.ExtraText = "";
            this.rbTopPayers.Values.Image = null;
            this.rbTopPayers.Values.Text = "Top Payers";
            this.rbTopPayers.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // rbTopGrosses
            // 
            this.rbTopGrosses.Location = new System.Drawing.Point(23, 120);
            this.rbTopGrosses.Name = "rbTopGrosses";
            this.rbTopGrosses.Size = new System.Drawing.Size(89, 20);
            this.rbTopGrosses.TabIndex = 2;
            this.rbTopGrosses.Text = "Top Grosses";
            this.rbTopGrosses.Values.ExtraText = "";
            this.rbTopGrosses.Values.Image = null;
            this.rbTopGrosses.Values.Text = "Top Grosses";
            this.rbTopGrosses.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // rbStreet
            // 
            this.rbStreet.Location = new System.Drawing.Point(23, 97);
            this.rbStreet.Name = "rbStreet";
            this.rbStreet.Size = new System.Drawing.Size(55, 20);
            this.rbStreet.TabIndex = 2;
            this.rbStreet.Text = "Street";
            this.rbStreet.Values.ExtraText = "";
            this.rbStreet.Values.Image = null;
            this.rbStreet.Values.Text = "Street";
            this.rbStreet.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // rbLineOfBuss
            // 
            this.rbLineOfBuss.Location = new System.Drawing.Point(23, 74);
            this.rbLineOfBuss.Name = "rbLineOfBuss";
            this.rbLineOfBuss.Size = new System.Drawing.Size(109, 20);
            this.rbLineOfBuss.TabIndex = 2;
            this.rbLineOfBuss.Text = "Line of Business";
            this.rbLineOfBuss.Values.ExtraText = "";
            this.rbLineOfBuss.Values.Image = null;
            this.rbLineOfBuss.Values.Text = "Line of Business";
            this.rbLineOfBuss.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // rbDistrict
            // 
            this.rbDistrict.Enabled = false;
            this.rbDistrict.Location = new System.Drawing.Point(23, 51);
            this.rbDistrict.Name = "rbDistrict";
            this.rbDistrict.Size = new System.Drawing.Size(61, 20);
            this.rbDistrict.TabIndex = 2;
            this.rbDistrict.Text = "District";
            this.rbDistrict.Values.ExtraText = "";
            this.rbDistrict.Values.Image = null;
            this.rbDistrict.Values.Text = "District";
            this.rbDistrict.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // rbBarangay
            // 
            this.rbBarangay.Checked = true;
            this.rbBarangay.Location = new System.Drawing.Point(23, 28);
            this.rbBarangay.Name = "rbBarangay";
            this.rbBarangay.Size = new System.Drawing.Size(73, 20);
            this.rbBarangay.TabIndex = 2;
            this.rbBarangay.Text = "Barangay";
            this.rbBarangay.Values.ExtraText = "";
            this.rbBarangay.Values.Image = null;
            this.rbBarangay.Values.Text = "Barangay";
            this.rbBarangay.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // hdrGroupBy
            // 
            this.hdrGroupBy.Dock = System.Windows.Forms.DockStyle.Top;
            this.hdrGroupBy.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.hdrGroupBy.Location = new System.Drawing.Point(0, 0);
            this.hdrGroupBy.Name = "hdrGroupBy";
            this.hdrGroupBy.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.hdrGroupBy.Size = new System.Drawing.Size(148, 24);
            this.hdrGroupBy.TabIndex = 0;
            this.hdrGroupBy.Text = " ";
            this.hdrGroupBy.Values.Description = "";
            this.hdrGroupBy.Values.Heading = " ";
            this.hdrGroupBy.Values.Image = null;
            // 
            // lblBarangay
            // 
            this.lblBarangay.Location = new System.Drawing.Point(180, 37);
            this.lblBarangay.Name = "lblBarangay";
            this.lblBarangay.Size = new System.Drawing.Size(61, 20);
            this.lblBarangay.TabIndex = 2;
            this.lblBarangay.Text = "Barangay";
            this.lblBarangay.Values.ExtraText = "";
            this.lblBarangay.Values.Image = null;
            this.lblBarangay.Values.Text = "Barangay";
            // 
            // lblDistrict
            // 
            this.lblDistrict.Location = new System.Drawing.Point(180, 64);
            this.lblDistrict.Name = "lblDistrict";
            this.lblDistrict.Size = new System.Drawing.Size(49, 20);
            this.lblDistrict.TabIndex = 2;
            this.lblDistrict.Text = "District";
            this.lblDistrict.Values.ExtraText = "";
            this.lblDistrict.Values.Image = null;
            this.lblDistrict.Values.Text = "District";
            // 
            // lblBussType
            // 
            this.lblBussType.Location = new System.Drawing.Point(180, 91);
            this.lblBussType.Name = "lblBussType";
            this.lblBussType.Size = new System.Drawing.Size(86, 20);
            this.lblBussType.TabIndex = 2;
            this.lblBussType.Text = "Business Type";
            this.lblBussType.Values.ExtraText = "";
            this.lblBussType.Values.Image = null;
            this.lblBussType.Values.Text = "Business Type";
            // 
            // lblBussNature
            // 
            this.lblBussNature.Location = new System.Drawing.Point(180, 118);
            this.lblBussNature.Name = "lblBussNature";
            this.lblBussNature.Size = new System.Drawing.Size(112, 20);
            this.lblBussNature.TabIndex = 2;
            this.lblBussNature.Text = "Nature of Business";
            this.lblBussNature.Values.ExtraText = "";
            this.lblBussNature.Values.Image = null;
            this.lblBussNature.Values.Text = "Nature of Business";
            // 
            // lblTopGrossPayer
            // 
            this.lblTopGrossPayer.Location = new System.Drawing.Point(180, 145);
            this.lblTopGrossPayer.Name = "lblTopGrossPayer";
            this.lblTopGrossPayer.Size = new System.Drawing.Size(100, 20);
            this.lblTopGrossPayer.TabIndex = 2;
            this.lblTopGrossPayer.Text = "Top Gross/Payer";
            this.lblTopGrossPayer.Values.ExtraText = "";
            this.lblTopGrossPayer.Values.Image = null;
            this.lblTopGrossPayer.Values.Text = "Top Gross/Payer";
            // 
            // lblNameOfStreet
            // 
            this.lblNameOfStreet.Location = new System.Drawing.Point(180, 171);
            this.lblNameOfStreet.Name = "lblNameOfStreet";
            this.lblNameOfStreet.Size = new System.Drawing.Size(93, 20);
            this.lblNameOfStreet.TabIndex = 2;
            this.lblNameOfStreet.Text = "Name of Street";
            this.lblNameOfStreet.Values.ExtraText = "";
            this.lblNameOfStreet.Values.Image = null;
            this.lblNameOfStreet.Values.Text = "Name of Street";
            // 
            // cboBarangay
            // 
            this.cboBarangay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBarangay.DropDownWidth = 315;
            this.cboBarangay.FormattingEnabled = false;
            this.cboBarangay.Location = new System.Drawing.Point(286, 37);
            this.cboBarangay.Name = "cboBarangay";
            this.cboBarangay.Size = new System.Drawing.Size(254, 21);
            this.cboBarangay.TabIndex = 3;
            // 
            // cboDistrict
            // 
            this.cboDistrict.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDistrict.DropDownWidth = 121;
            this.cboDistrict.Enabled = false;
            this.cboDistrict.FormattingEnabled = false;
            this.cboDistrict.Location = new System.Drawing.Point(286, 64);
            this.cboDistrict.Name = "cboDistrict";
            this.cboDistrict.Size = new System.Drawing.Size(254, 21);
            this.cboDistrict.TabIndex = 3;
            // 
            // cboBussType
            // 
            this.cboBussType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBussType.DropDownWidth = 121;
            this.cboBussType.Enabled = false;
            this.cboBussType.FormattingEnabled = false;
            this.cboBussType.Location = new System.Drawing.Point(286, 91);
            this.cboBussType.Name = "cboBussType";
            this.cboBussType.Size = new System.Drawing.Size(254, 21);
            this.cboBussType.TabIndex = 3;
            this.cboBussType.SelectedValueChanged += new System.EventHandler(this.cboBussType_SelectedValueChanged);
            // 
            // cboBussNature
            // 
            this.cboBussNature.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBussNature.DropDownWidth = 121;
            this.cboBussNature.Enabled = false;
            this.cboBussNature.FormattingEnabled = false;
            this.cboBussNature.Location = new System.Drawing.Point(286, 118);
            this.cboBussNature.Name = "cboBussNature";
            this.cboBussNature.Size = new System.Drawing.Size(254, 21);
            this.cboBussNature.TabIndex = 3;
            // 
            // txtGross
            // 
            this.txtGross.Enabled = false;
            this.txtGross.Location = new System.Drawing.Point(286, 144);
            this.txtGross.Name = "txtGross";
            this.txtGross.Size = new System.Drawing.Size(55, 20);
            this.txtGross.TabIndex = 4;
            this.txtGross.Text = "0";
            this.txtGross.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtGross.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGross_KeyPress);
            // 
            // cboStatus
            // 
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.DropDownWidth = 121;
            this.cboStatus.FormattingEnabled = false;
            this.cboStatus.Location = new System.Drawing.Point(286, 196);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(66, 21);
            this.cboStatus.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(236, 197);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(44, 20);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Status";
            this.lblStatus.Values.ExtraText = "";
            this.lblStatus.Values.Image = null;
            this.lblStatus.Values.Text = "Status";
            // 
            // txtTaxYear
            // 
            this.txtTaxYear.Location = new System.Drawing.Point(428, 197);
            this.txtTaxYear.MaxLength = 4;
            this.txtTaxYear.Name = "txtTaxYear";
            this.txtTaxYear.Size = new System.Drawing.Size(55, 20);
            this.txtTaxYear.TabIndex = 4;
            this.txtTaxYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTaxYear
            // 
            this.lblTaxYear.Location = new System.Drawing.Point(366, 196);
            this.lblTaxYear.Name = "lblTaxYear";
            this.lblTaxYear.Size = new System.Drawing.Size(56, 20);
            this.lblTaxYear.TabIndex = 2;
            this.lblTaxYear.Text = "Tax Year";
            this.lblTaxYear.Values.ExtraText = "";
            this.lblTaxYear.Values.Image = null;
            this.lblTaxYear.Values.Text = "Tax Year";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(397, 286);
            this.btnOk.Name = "btnOk";
            this.btnOk.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOk.Size = new System.Drawing.Size(75, 24);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "Generate";
            this.btnOk.Values.ExtraText = "";
            this.btnOk.Values.Image = null;
            this.btnOk.Values.ImageStates.ImageCheckedNormal = null;
            this.btnOk.Values.ImageStates.ImageCheckedPressed = null;
            this.btnOk.Values.ImageStates.ImageCheckedTracking = null;
            this.btnOk.Values.Text = "Generate";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(478, 286);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkByPercentage
            // 
            this.chkByPercentage.Location = new System.Drawing.Point(366, 145);
            this.chkByPercentage.Name = "chkByPercentage";
            this.chkByPercentage.Size = new System.Drawing.Size(101, 20);
            this.chkByPercentage.TabIndex = 6;
            this.chkByPercentage.Text = "By Percentage";
            this.chkByPercentage.Values.ExtraText = "";
            this.chkByPercentage.Values.Image = null;
            this.chkByPercentage.Values.Text = "By Percentage";
            this.chkByPercentage.CheckedChanged += new System.EventHandler(this.chkByPercentage_CheckedChanged);
            // 
            // cboStreet
            // 
            this.cboStreet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStreet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStreet.Enabled = false;
            this.cboStreet.FormattingEnabled = true;
            this.cboStreet.Location = new System.Drawing.Point(286, 170);
            this.cboStreet.Name = "cboStreet";
            this.cboStreet.Size = new System.Drawing.Size(254, 21);
            this.cboStreet.TabIndex = 8;
            this.cboStreet.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPercent.Location = new System.Drawing.Point(343, 147);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(17, 13);
            this.lblPercent.TabIndex = 9;
            this.lblPercent.Text = "%";
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(165, 8);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(388, 270);
            this.containerWithShadow1.TabIndex = 10;
            // 
            // frmBusinessRoll
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(568, 322);
            this.Controls.Add(this.lblPercent);
            this.Controls.Add(this.cboStreet);
            this.Controls.Add(this.chkByPercentage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtTaxYear);
            this.Controls.Add(this.txtGross);
            this.Controls.Add(this.cboStatus);
            this.Controls.Add(this.cboBussNature);
            this.Controls.Add(this.cboBussType);
            this.Controls.Add(this.cboDistrict);
            this.Controls.Add(this.cboBarangay);
            this.Controls.Add(this.lblNameOfStreet);
            this.Controls.Add(this.lblTaxYear);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblTopGrossPayer);
            this.Controls.Add(this.lblBussNature);
            this.Controls.Add(this.lblBussType);
            this.Controls.Add(this.lblDistrict);
            this.Controls.Add(this.lblBarangay);
            this.Controls.Add(this.grpGroupBy);
            this.Controls.Add(this.containerWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBusinessRoll";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Business Roll";
            this.Load += new System.EventHandler(this.frmBusinessRoll_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grpGroupBy.Panel)).EndInit();
            this.grpGroupBy.Panel.ResumeLayout(false);
            this.grpGroupBy.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpGroupBy)).EndInit();
            this.grpGroupBy.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonGroup grpGroupBy;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader hdrGroupBy;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbBarangay;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbName;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbLeastPayers;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbLeastGrosses;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbTopPayers;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbTopGrosses;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbStreet;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbLineOfBuss;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbDistrict;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBarangay;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDistrict;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBussType;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBussNature;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTopGrossPayer;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNameOfStreet;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblStatus;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxYear;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        public ComponentFactory.Krypton.Toolkit.KryptonComboBox cboBarangay;
        public ComponentFactory.Krypton.Toolkit.KryptonComboBox cboDistrict;
        public ComponentFactory.Krypton.Toolkit.KryptonComboBox cboBussType;
        public ComponentFactory.Krypton.Toolkit.KryptonComboBox cboBussNature;
        public ComponentFactory.Krypton.Toolkit.KryptonTextBox txtGross;
        public ComponentFactory.Krypton.Toolkit.KryptonComboBox cboStatus;
        public ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxYear;
        public ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkByPercentage;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.ComboBox cboStreet;
        private System.Windows.Forms.Label lblPercent;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbTopAssessed;


    }
}

