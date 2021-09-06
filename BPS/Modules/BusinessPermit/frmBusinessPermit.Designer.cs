namespace Amellar.Modules.BusinessPermit
{
    partial class frmBusinessPermit
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
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.txtBnsAdd = new System.Windows.Forms.TextBox();
            this.txtTaxYear = new System.Windows.Forms.TextBox();
            this.txtTelNo = new System.Windows.Forms.TextBox();
            this.txtEmployee = new System.Windows.Forms.TextBox();
            this.txtBnsStat = new System.Windows.Forms.TextBox();
            this.txtBnsCode = new System.Windows.Forms.TextBox();
            this.txtBnsDesc = new System.Windows.Forms.TextBox();
            this.txtOrNo = new System.Windows.Forms.TextBox();
            this.txtAppNo = new System.Windows.Forms.TextBox();
            this.txtLicNo = new System.Windows.Forms.TextBox();
            this.txtPlate = new System.Windows.Forms.TextBox();
            this.txtOwnName = new System.Windows.Forms.TextBox();
            this.txtOwnAdd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
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
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.cmbPermitNo = new System.Windows.Forms.ComboBox();
            this.dtpORDate = new System.Windows.Forms.DateTimePicker();
            this.dtpPermitDate = new System.Windows.Forms.DateTimePicker();
            this.dtpAppDate = new System.Windows.Forms.DateTimePicker();
            this.dtpLicDate = new System.Windows.Forms.DateTimePicker();
            this.label16 = new System.Windows.Forms.Label();
            this.btnUpdate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txtNationality = new System.Windows.Forms.TextBox();
            this.txtOwnTin = new System.Windows.Forms.TextBox();
            this.txtOwnSSS = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPrint = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnGeneratePlate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCheckList = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEditRemarks = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEditBnsName = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtDTIBnsName = new System.Windows.Forms.TextBox();
            this.bin1 = new Amellar.Common.BIN.BIN();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(175, 21);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(92, 25);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.Values.ExtraText = "";
            this.btnSearch.Values.Image = null;
            this.btnSearch.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearch.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearch.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtBnsName
            // 
            this.txtBnsName.Location = new System.Drawing.Point(121, 55);
            this.txtBnsName.Multiline = true;
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(243, 20);
            this.txtBnsName.TabIndex = 5;
            // 
            // txtBnsAdd
            // 
            this.txtBnsAdd.Location = new System.Drawing.Point(121, 106);
            this.txtBnsAdd.Name = "txtBnsAdd";
            this.txtBnsAdd.ReadOnly = true;
            this.txtBnsAdd.Size = new System.Drawing.Size(365, 20);
            this.txtBnsAdd.TabIndex = 6;
            // 
            // txtTaxYear
            // 
            this.txtTaxYear.Location = new System.Drawing.Point(426, 55);
            this.txtTaxYear.Name = "txtTaxYear";
            this.txtTaxYear.ReadOnly = true;
            this.txtTaxYear.Size = new System.Drawing.Size(60, 20);
            this.txtTaxYear.TabIndex = 7;
            // 
            // txtTelNo
            // 
            this.txtTelNo.Location = new System.Drawing.Point(121, 131);
            this.txtTelNo.Name = "txtTelNo";
            this.txtTelNo.ReadOnly = true;
            this.txtTelNo.Size = new System.Drawing.Size(68, 20);
            this.txtTelNo.TabIndex = 8;
            // 
            // txtEmployee
            // 
            this.txtEmployee.Location = new System.Drawing.Point(286, 131);
            this.txtEmployee.Name = "txtEmployee";
            this.txtEmployee.ReadOnly = true;
            this.txtEmployee.Size = new System.Drawing.Size(78, 20);
            this.txtEmployee.TabIndex = 9;
            // 
            // txtBnsStat
            // 
            this.txtBnsStat.Location = new System.Drawing.Point(426, 131);
            this.txtBnsStat.Name = "txtBnsStat";
            this.txtBnsStat.ReadOnly = true;
            this.txtBnsStat.Size = new System.Drawing.Size(60, 20);
            this.txtBnsStat.TabIndex = 10;
            // 
            // txtBnsCode
            // 
            this.txtBnsCode.Location = new System.Drawing.Point(121, 156);
            this.txtBnsCode.Name = "txtBnsCode";
            this.txtBnsCode.ReadOnly = true;
            this.txtBnsCode.Size = new System.Drawing.Size(68, 20);
            this.txtBnsCode.TabIndex = 11;
            // 
            // txtBnsDesc
            // 
            this.txtBnsDesc.Location = new System.Drawing.Point(195, 156);
            this.txtBnsDesc.Name = "txtBnsDesc";
            this.txtBnsDesc.ReadOnly = true;
            this.txtBnsDesc.Size = new System.Drawing.Size(291, 20);
            this.txtBnsDesc.TabIndex = 12;
            // 
            // txtOrNo
            // 
            this.txtOrNo.Location = new System.Drawing.Point(121, 180);
            this.txtOrNo.Name = "txtOrNo";
            this.txtOrNo.ReadOnly = true;
            this.txtOrNo.Size = new System.Drawing.Size(68, 20);
            this.txtOrNo.TabIndex = 13;
            // 
            // txtAppNo
            // 
            this.txtAppNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAppNo.Location = new System.Drawing.Point(121, 205);
            this.txtAppNo.Name = "txtAppNo";
            this.txtAppNo.ReadOnly = true;
            this.txtAppNo.Size = new System.Drawing.Size(168, 20);
            this.txtAppNo.TabIndex = 4;
            // 
            // txtLicNo
            // 
            this.txtLicNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLicNo.Location = new System.Drawing.Point(121, 229);
            this.txtLicNo.Name = "txtLicNo";
            this.txtLicNo.ReadOnly = true;
            this.txtLicNo.Size = new System.Drawing.Size(168, 20);
            this.txtLicNo.TabIndex = 6;
            // 
            // txtPlate
            // 
            this.txtPlate.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPlate.Location = new System.Drawing.Point(121, 253);
            this.txtPlate.Name = "txtPlate";
            this.txtPlate.ReadOnly = true;
            this.txtPlate.Size = new System.Drawing.Size(168, 20);
            this.txtPlate.TabIndex = 8;
            // 
            // txtOwnName
            // 
            this.txtOwnName.Location = new System.Drawing.Point(121, 303);
            this.txtOwnName.Name = "txtOwnName";
            this.txtOwnName.ReadOnly = true;
            this.txtOwnName.Size = new System.Drawing.Size(211, 20);
            this.txtOwnName.TabIndex = 17;
            // 
            // txtOwnAdd
            // 
            this.txtOwnAdd.Location = new System.Drawing.Point(121, 329);
            this.txtOwnAdd.Name = "txtOwnAdd";
            this.txtOwnAdd.ReadOnly = true;
            this.txtOwnAdd.Size = new System.Drawing.Size(364, 20);
            this.txtOwnAdd.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(341, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Permit No.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Business Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Business Address";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(370, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Tax Year";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Telephone No.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(195, 138);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "No. of Employee";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(383, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Status";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 161);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Business Type";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(25, 187);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "OR Number";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(195, 186);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "OR Date";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(332, 186);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 13);
            this.label11.TabIndex = 29;
            this.label11.Text = "Permit Date";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(25, 211);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 13);
            this.label12.TabIndex = 30;
            this.label12.Text = "Application No.";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(297, 211);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(97, 13);
            this.label13.TabIndex = 31;
            this.label13.Text = "Date of Application";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(25, 236);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(83, 13);
            this.label14.TabIndex = 32;
            this.label14.Text = "Mayor\'s License";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(25, 260);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(76, 13);
            this.label15.TabIndex = 33;
            this.label15.Text = "Business Plate";
            // 
            // cmbPermitNo
            // 
            this.cmbPermitNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPermitNo.Enabled = false;
            this.cmbPermitNo.FormattingEnabled = true;
            this.cmbPermitNo.Location = new System.Drawing.Point(403, 23);
            this.cmbPermitNo.Name = "cmbPermitNo";
            this.cmbPermitNo.Size = new System.Drawing.Size(83, 21);
            this.cmbPermitNo.TabIndex = 3;
            this.cmbPermitNo.SelectedValueChanged += new System.EventHandler(this.cmbPermitNo_SelectedValueChanged);
            // 
            // dtpORDate
            // 
            this.dtpORDate.Enabled = false;
            this.dtpORDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpORDate.Location = new System.Drawing.Point(242, 180);
            this.dtpORDate.Name = "dtpORDate";
            this.dtpORDate.Size = new System.Drawing.Size(84, 20);
            this.dtpORDate.TabIndex = 35;
            // 
            // dtpPermitDate
            // 
            this.dtpPermitDate.Enabled = false;
            this.dtpPermitDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPermitDate.Location = new System.Drawing.Point(397, 180);
            this.dtpPermitDate.Name = "dtpPermitDate";
            this.dtpPermitDate.Size = new System.Drawing.Size(89, 20);
            this.dtpPermitDate.TabIndex = 36;
            // 
            // dtpAppDate
            // 
            this.dtpAppDate.Enabled = false;
            this.dtpAppDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpAppDate.Location = new System.Drawing.Point(397, 205);
            this.dtpAppDate.Name = "dtpAppDate";
            this.dtpAppDate.Size = new System.Drawing.Size(89, 20);
            this.dtpAppDate.TabIndex = 5;
            // 
            // dtpLicDate
            // 
            this.dtpLicDate.Enabled = false;
            this.dtpLicDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpLicDate.Location = new System.Drawing.Point(396, 230);
            this.dtpLicDate.Name = "dtpLicDate";
            this.dtpLicDate.Size = new System.Drawing.Size(89, 20);
            this.dtpLicDate.TabIndex = 7;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(340, 235);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(54, 13);
            this.label16.TabIndex = 39;
            this.label16.Text = "M.L. Date";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(295, 251);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnUpdate.Size = new System.Drawing.Size(69, 25);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "Edit";
            this.btnUpdate.Values.ExtraText = "";
            this.btnUpdate.Values.Image = null;
            this.btnUpdate.Values.ImageStates.ImageCheckedNormal = null;
            this.btnUpdate.Values.ImageStates.ImageCheckedPressed = null;
            this.btnUpdate.Values.ImageStates.ImageCheckedTracking = null;
            this.btnUpdate.Values.Text = "Edit";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(25, 310);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(76, 13);
            this.label17.TabIndex = 33;
            this.label17.Text = "Owner\'s Name";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(25, 336);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(86, 13);
            this.label18.TabIndex = 33;
            this.label18.Text = "Owner\'s Address";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(338, 310);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(56, 13);
            this.label19.TabIndex = 33;
            this.label19.Text = "Nationality";
            // 
            // txtNationality
            // 
            this.txtNationality.Location = new System.Drawing.Point(397, 303);
            this.txtNationality.Name = "txtNationality";
            this.txtNationality.ReadOnly = true;
            this.txtNationality.Size = new System.Drawing.Size(88, 20);
            this.txtNationality.TabIndex = 9;
            // 
            // txtOwnTin
            // 
            this.txtOwnTin.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtOwnTin.Location = new System.Drawing.Point(121, 355);
            this.txtOwnTin.Name = "txtOwnTin";
            this.txtOwnTin.ReadOnly = true;
            this.txtOwnTin.Size = new System.Drawing.Size(159, 20);
            this.txtOwnTin.TabIndex = 10;
            // 
            // txtOwnSSS
            // 
            this.txtOwnSSS.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtOwnSSS.Location = new System.Drawing.Point(341, 355);
            this.txtOwnSSS.Name = "txtOwnSSS";
            this.txtOwnSSS.ReadOnly = true;
            this.txtOwnSSS.Size = new System.Drawing.Size(145, 20);
            this.txtOwnSSS.TabIndex = 11;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(304, 362);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(28, 13);
            this.label20.TabIndex = 33;
            this.label20.Text = "SSS";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(25, 362);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(25, 13);
            this.label21.TabIndex = 33;
            this.label21.Text = "TIN";
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Location = new System.Drawing.Point(26, 399);
            this.dgvList.Name = "dgvList";
            this.dgvList.Size = new System.Drawing.Size(459, 68);
            this.dgvList.TabIndex = 12;
            // 
            // txtRemarks
            // 
            this.txtRemarks.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRemarks.Location = new System.Drawing.Point(105, 473);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.ReadOnly = true;
            this.txtRemarks.Size = new System.Drawing.Size(380, 42);
            this.txtRemarks.TabIndex = 13;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(45, 473);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(52, 13);
            this.label22.TabIndex = 33;
            this.label22.Text = "Remarks:";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(295, 537);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(106, 25);
            this.btnGenerate.TabIndex = 15;
            this.btnGenerate.Text = "Generate Permit";
            this.btnGenerate.Values.ExtraText = "";
            this.btnGenerate.Values.Image = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedNormal = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedPressed = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedTracking = null;
            this.btnGenerate.Values.Text = "Generate Permit";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(407, 537);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(92, 25);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Close";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(16, 537);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint.Size = new System.Drawing.Size(128, 25);
            this.btnPrint.TabIndex = 14;
            this.btnPrint.Text = "Cert. of Registration";
            this.btnPrint.Values.ExtraText = "";
            this.btnPrint.Values.Image = null;
            this.btnPrint.Values.ImageStates.ImageCheckedNormal = null;
            this.btnPrint.Values.ImageStates.ImageCheckedPressed = null;
            this.btnPrint.Values.ImageStates.ImageCheckedTracking = null;
            this.btnPrint.Values.Text = "Cert. of Registration";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnGeneratePlate
            // 
            this.btnGeneratePlate.Location = new System.Drawing.Point(0, 0);
            this.btnGeneratePlate.Name = "btnGeneratePlate";
            this.btnGeneratePlate.Size = new System.Drawing.Size(90, 25);
            this.btnGeneratePlate.TabIndex = 40;
            this.btnGeneratePlate.Text = "Button";
            this.btnGeneratePlate.Values.ExtraText = "";
            this.btnGeneratePlate.Values.Image = null;
            this.btnGeneratePlate.Values.ImageStates.ImageCheckedNormal = null;
            this.btnGeneratePlate.Values.ImageStates.ImageCheckedPressed = null;
            this.btnGeneratePlate.Values.ImageStates.ImageCheckedTracking = null;
            this.btnGeneratePlate.Values.Text = "Button";
            this.btnGeneratePlate.Visible = false;
            // 
            // btnCheckList
            // 
            this.btnCheckList.Location = new System.Drawing.Point(150, 537);
            this.btnCheckList.Name = "btnCheckList";
            this.btnCheckList.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCheckList.Size = new System.Drawing.Size(128, 25);
            this.btnCheckList.TabIndex = 14;
            this.btnCheckList.Text = "Documents Checklist";
            this.btnCheckList.Values.ExtraText = "";
            this.btnCheckList.Values.Image = null;
            this.btnCheckList.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCheckList.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCheckList.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCheckList.Values.Text = "Documents Checklist";
            this.btnCheckList.Visible = false;
            this.btnCheckList.Click += new System.EventHandler(this.btnCheckList_Click);
            // 
            // btnEditRemarks
            // 
            this.btnEditRemarks.Location = new System.Drawing.Point(26, 490);
            this.btnEditRemarks.Name = "btnEditRemarks";
            this.btnEditRemarks.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnEditRemarks.Size = new System.Drawing.Size(69, 25);
            this.btnEditRemarks.TabIndex = 9;
            this.btnEditRemarks.Text = "Edit";
            this.btnEditRemarks.Values.ExtraText = "";
            this.btnEditRemarks.Values.Image = null;
            this.btnEditRemarks.Values.ImageStates.ImageCheckedNormal = null;
            this.btnEditRemarks.Values.ImageStates.ImageCheckedPressed = null;
            this.btnEditRemarks.Values.ImageStates.ImageCheckedTracking = null;
            this.btnEditRemarks.Values.Text = "Edit";
            this.btnEditRemarks.Click += new System.EventHandler(this.btnEditRemarks_Click);
            // 
            // btnEditBnsName
            // 
            this.btnEditBnsName.Location = new System.Drawing.Point(24, 78);
            this.btnEditBnsName.Name = "btnEditBnsName";
            this.btnEditBnsName.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnEditBnsName.Size = new System.Drawing.Size(145, 25);
            this.btnEditBnsName.TabIndex = 9;
            this.btnEditBnsName.Text = "Edit Business Name";
            this.btnEditBnsName.Values.ExtraText = "";
            this.btnEditBnsName.Values.Image = null;
            this.btnEditBnsName.Values.ImageStates.ImageCheckedNormal = null;
            this.btnEditBnsName.Values.ImageStates.ImageCheckedPressed = null;
            this.btnEditBnsName.Values.ImageStates.ImageCheckedTracking = null;
            this.btnEditBnsName.Values.Text = "Edit Business Name";
            this.btnEditBnsName.Click += new System.EventHandler(this.btnEditBnsName_Click);
            // 
            // txtDTIBnsName
            // 
            this.txtDTIBnsName.Location = new System.Drawing.Point(175, 80);
            this.txtDTIBnsName.Multiline = true;
            this.txtDTIBnsName.Name = "txtDTIBnsName";
            this.txtDTIBnsName.ReadOnly = true;
            this.txtDTIBnsName.Size = new System.Drawing.Size(310, 20);
            this.txtDTIBnsName.TabIndex = 5;
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(26, 23);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(143, 23);
            this.bin1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(12, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(487, 283);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(12, 286);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(487, 100);
            this.groupBox2.TabIndex = 42;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(12, 385);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(487, 145);
            this.groupBox3.TabIndex = 43;
            this.groupBox3.TabStop = false;
            // 
            // frmBusinessPermit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(511, 569);
            this.ControlBox = false;
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.dtpLicDate);
            this.Controls.Add(this.dtpAppDate);
            this.Controls.Add(this.dtpPermitDate);
            this.Controls.Add(this.dtpORDate);
            this.Controls.Add(this.cmbPermitNo);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOwnAdd);
            this.Controls.Add(this.txtNationality);
            this.Controls.Add(this.txtOwnSSS);
            this.Controls.Add(this.txtOwnTin);
            this.Controls.Add(this.txtOwnName);
            this.Controls.Add(this.txtPlate);
            this.Controls.Add(this.txtLicNo);
            this.Controls.Add(this.txtAppNo);
            this.Controls.Add(this.txtOrNo);
            this.Controls.Add(this.txtBnsDesc);
            this.Controls.Add(this.txtBnsCode);
            this.Controls.Add(this.txtBnsStat);
            this.Controls.Add(this.txtEmployee);
            this.Controls.Add(this.txtTelNo);
            this.Controls.Add(this.txtTaxYear);
            this.Controls.Add(this.txtBnsAdd);
            this.Controls.Add(this.txtDTIBnsName);
            this.Controls.Add(this.txtBnsName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCheckList);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnGeneratePlate);
            this.Controls.Add(this.btnEditRemarks);
            this.Controls.Add(this.btnEditBnsName);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.bin1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBusinessPermit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Business Permit";
            this.Load += new System.EventHandler(this.frmBusinessPermit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.BIN.BIN bin1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private System.Windows.Forms.TextBox txtBnsName;
        private System.Windows.Forms.TextBox txtBnsAdd;
        private System.Windows.Forms.TextBox txtTaxYear;
        private System.Windows.Forms.TextBox txtTelNo;
        private System.Windows.Forms.TextBox txtEmployee;
        private System.Windows.Forms.TextBox txtBnsStat;
        private System.Windows.Forms.TextBox txtBnsCode;
        private System.Windows.Forms.TextBox txtBnsDesc;
        private System.Windows.Forms.TextBox txtOrNo;
        private System.Windows.Forms.TextBox txtAppNo;
        private System.Windows.Forms.TextBox txtLicNo;
        private System.Windows.Forms.TextBox txtPlate;
        private System.Windows.Forms.TextBox txtOwnName;
        private System.Windows.Forms.TextBox txtOwnAdd;
        private System.Windows.Forms.Label label1;
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
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cmbPermitNo;
        private System.Windows.Forms.DateTimePicker dtpORDate;
        private System.Windows.Forms.DateTimePicker dtpPermitDate;
        private System.Windows.Forms.DateTimePicker dtpAppDate;
        private System.Windows.Forms.DateTimePicker dtpLicDate;
        private System.Windows.Forms.Label label16;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnUpdate;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtNationality;
        private System.Windows.Forms.TextBox txtOwnTin;
        private System.Windows.Forms.TextBox txtOwnSSS;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label label22;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPrint;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGeneratePlate;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCheckList;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEditRemarks;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEditBnsName;
        private System.Windows.Forms.TextBox txtDTIBnsName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        //private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        //private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        //private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow3;
    }
}

