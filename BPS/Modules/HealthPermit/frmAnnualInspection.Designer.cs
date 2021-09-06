namespace Amellar.Modules.HealthPermit
{
    partial class frmAnnualInspection
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
            this.btnNew = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDiscard = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPrint = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDelete = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEdit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMiddleInitial = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bin1 = new BIN.BIN();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.rdoNew = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSearchTmp = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtTmpBin = new System.Windows.Forms.TextBox();
            this.rdoRenewal = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtDTIName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtLoc = new System.Windows.Forms.TextBox();
            this.txtStallNo = new System.Windows.Forms.TextBox();
            this.txtORNo = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCertOcc = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGrp = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCharOcc = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTaxYear = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnExt = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(380, 282);
            this.btnNew.Name = "btnNew";
            this.btnNew.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnNew.Size = new System.Drawing.Size(92, 25);
            this.btnNew.TabIndex = 8;
            this.btnNew.Text = "Add";
            this.btnNew.Values.ExtraText = "";
            this.btnNew.Values.Image = null;
            this.btnNew.Values.ImageStates.ImageCheckedNormal = null;
            this.btnNew.Values.ImageStates.ImageCheckedPressed = null;
            this.btnNew.Values.ImageStates.ImageCheckedTracking = null;
            this.btnNew.Values.Text = "Add";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnDiscard
            // 
            this.btnDiscard.Location = new System.Drawing.Point(380, 406);
            this.btnDiscard.Name = "btnDiscard";
            this.btnDiscard.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnDiscard.Size = new System.Drawing.Size(92, 25);
            this.btnDiscard.TabIndex = 12;
            this.btnDiscard.Text = "Cancel";
            this.btnDiscard.Values.ExtraText = "";
            this.btnDiscard.Values.Image = null;
            this.btnDiscard.Values.ImageStates.ImageCheckedNormal = null;
            this.btnDiscard.Values.ImageStates.ImageCheckedPressed = null;
            this.btnDiscard.Values.ImageStates.ImageCheckedTracking = null;
            this.btnDiscard.Values.Text = "Cancel";
            this.btnDiscard.Click += new System.EventHandler(this.btnDiscard_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(380, 437);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(92, 25);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(380, 375);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint.Size = new System.Drawing.Size(92, 25);
            this.btnPrint.TabIndex = 11;
            this.btnPrint.Text = "Print";
            this.btnPrint.Values.ExtraText = "";
            this.btnPrint.Values.Image = null;
            this.btnPrint.Values.ImageStates.ImageCheckedNormal = null;
            this.btnPrint.Values.ImageStates.ImageCheckedPressed = null;
            this.btnPrint.Values.ImageStates.ImageCheckedTracking = null;
            this.btnPrint.Values.Text = "Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(380, 344);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnDelete.Size = new System.Drawing.Size(92, 25);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Values.ExtraText = "";
            this.btnDelete.Values.Image = null;
            this.btnDelete.Values.ImageStates.ImageCheckedNormal = null;
            this.btnDelete.Values.ImageStates.ImageCheckedPressed = null;
            this.btnDelete.Values.ImageStates.ImageCheckedTracking = null;
            this.btnDelete.Values.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(380, 313);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnEdit.Size = new System.Drawing.Size(92, 25);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "Edit";
            this.btnEdit.Values.ExtraText = "";
            this.btnEdit.Values.Image = null;
            this.btnEdit.Values.ImageStates.ImageCheckedNormal = null;
            this.btnEdit.Values.ImageStates.ImageCheckedPressed = null;
            this.btnEdit.Values.ImageStates.ImageCheckedTracking = null;
            this.btnEdit.Values.Text = "Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvList);
            this.groupBox3.Location = new System.Drawing.Point(6, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(466, 172);
            this.groupBox3.TabIndex = 31;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "List of Issued Permits";
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToResizeRows = false;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvList.Location = new System.Drawing.Point(7, 19);
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.Size = new System.Drawing.Size(452, 147);
            this.dgvList.TabIndex = 0;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMiddleInitial);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtFirstName);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtLastName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(6, 263);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(368, 79);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Business Owner\'s Name ";
            // 
            // txtMiddleInitial
            // 
            this.txtMiddleInitial.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMiddleInitial.Location = new System.Drawing.Point(323, 45);
            this.txtMiddleInitial.MaxLength = 1;
            this.txtMiddleInitial.Name = "txtMiddleInitial";
            this.txtMiddleInitial.ReadOnly = true;
            this.txtMiddleInitial.Size = new System.Drawing.Size(28, 20);
            this.txtMiddleInitial.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(295, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "M.I";
            // 
            // txtFirstName
            // 
            this.txtFirstName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFirstName.Location = new System.Drawing.Point(83, 45);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.ReadOnly = true;
            this.txtFirstName.Size = new System.Drawing.Size(206, 20);
            this.txtFirstName.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Last Name";
            // 
            // txtLastName
            // 
            this.txtLastName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLastName.Location = new System.Drawing.Point(83, 19);
            this.txtLastName.Multiline = true;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.ReadOnly = true;
            this.txtLastName.Size = new System.Drawing.Size(268, 20);
            this.txtLastName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "First Name";
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(99, 20);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(138, 20);
            this.bin1.TabIndex = 34;
            // 
            // btnSearch
            // 
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(246, 18);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(114, 24);
            this.btnSearch.TabIndex = 33;
            this.btnSearch.Text = "Search";
            this.btnSearch.Values.ExtraText = "";
            this.btnSearch.Values.Image = null;
            this.btnSearch.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearch.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearch.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // rdoNew
            // 
            this.rdoNew.AutoSize = true;
            this.rdoNew.Enabled = false;
            this.rdoNew.Location = new System.Drawing.Point(14, 49);
            this.rdoNew.Name = "rdoNew";
            this.rdoNew.Size = new System.Drawing.Size(51, 17);
            this.rdoNew.TabIndex = 35;
            this.rdoNew.TabStop = true;
            this.rdoNew.Text = "NEW";
            this.rdoNew.UseVisualStyleBackColor = true;
            this.rdoNew.CheckedChanged += new System.EventHandler(this.rdoNew_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSearchTmp);
            this.groupBox2.Controls.Add(this.txtTmpBin);
            this.groupBox2.Controls.Add(this.rdoRenewal);
            this.groupBox2.Controls.Add(this.bin1);
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Controls.Add(this.rdoNew);
            this.groupBox2.Location = new System.Drawing.Point(6, 180);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(368, 79);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Transaction Type ";
            // 
            // btnSearchTmp
            // 
            this.btnSearchTmp.Enabled = false;
            this.btnSearchTmp.Location = new System.Drawing.Point(246, 47);
            this.btnSearchTmp.Name = "btnSearchTmp";
            this.btnSearchTmp.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearchTmp.Size = new System.Drawing.Size(114, 24);
            this.btnSearchTmp.TabIndex = 36;
            this.btnSearchTmp.Text = "Search";
            this.btnSearchTmp.Values.ExtraText = "";
            this.btnSearchTmp.Values.Image = null;
            this.btnSearchTmp.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearchTmp.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearchTmp.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearchTmp.Values.Text = "Search";
            this.btnSearchTmp.Click += new System.EventHandler(this.btnSearchTmp_Click);
            // 
            // txtTmpBin
            // 
            this.txtTmpBin.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTmpBin.Location = new System.Drawing.Point(99, 48);
            this.txtTmpBin.MaxLength = 20;
            this.txtTmpBin.Name = "txtTmpBin";
            this.txtTmpBin.ReadOnly = true;
            this.txtTmpBin.Size = new System.Drawing.Size(138, 20);
            this.txtTmpBin.TabIndex = 26;
            this.txtTmpBin.TextChanged += new System.EventHandler(this.txtTmpBin_TextChanged);
            // 
            // rdoRenewal
            // 
            this.rdoRenewal.AutoSize = true;
            this.rdoRenewal.Enabled = false;
            this.rdoRenewal.Location = new System.Drawing.Point(14, 21);
            this.rdoRenewal.Name = "rdoRenewal";
            this.rdoRenewal.Size = new System.Drawing.Size(79, 17);
            this.rdoRenewal.TabIndex = 35;
            this.rdoRenewal.TabStop = true;
            this.rdoRenewal.Text = "RENEWAL";
            this.rdoRenewal.UseVisualStyleBackColor = true;
            this.rdoRenewal.CheckedChanged += new System.EventHandler(this.rdoRenewal_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.txtDTIName);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.txtLoc);
            this.groupBox4.Controls.Add(this.txtStallNo);
            this.groupBox4.Controls.Add(this.txtORNo);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.dtpDate);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.txtCertOcc);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.txtGrp);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.txtCharOcc);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(6, 344);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(368, 148);
            this.groupBox4.TabIndex = 31;
            this.groupBox4.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 13);
            this.label11.TabIndex = 35;
            this.label11.Text = "DTI Name";
            // 
            // txtDTIName
            // 
            this.txtDTIName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtDTIName.Location = new System.Drawing.Point(74, 13);
            this.txtDTIName.MaxLength = 100;
            this.txtDTIName.Multiline = true;
            this.txtDTIName.Name = "txtDTIName";
            this.txtDTIName.ReadOnly = true;
            this.txtDTIName.Size = new System.Drawing.Size(286, 20);
            this.txtDTIName.TabIndex = 34;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(154, 65);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 13);
            this.label10.TabIndex = 33;
            this.label10.Text = "Location";
            // 
            // txtLoc
            // 
            this.txtLoc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLoc.Location = new System.Drawing.Point(208, 62);
            this.txtLoc.MaxLength = 100;
            this.txtLoc.Name = "txtLoc";
            this.txtLoc.ReadOnly = true;
            this.txtLoc.Size = new System.Drawing.Size(152, 20);
            this.txtLoc.TabIndex = 3;
            // 
            // txtStallNo
            // 
            this.txtStallNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtStallNo.Location = new System.Drawing.Point(251, 113);
            this.txtStallNo.MaxLength = 100;
            this.txtStallNo.Name = "txtStallNo";
            this.txtStallNo.ReadOnly = true;
            this.txtStallNo.Size = new System.Drawing.Size(109, 20);
            this.txtStallNo.TabIndex = 7;
            // 
            // txtORNo
            // 
            this.txtORNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtORNo.Location = new System.Drawing.Point(67, 113);
            this.txtORNo.MaxLength = 100;
            this.txtORNo.Name = "txtORNo";
            this.txtORNo.ReadOnly = true;
            this.txtORNo.Size = new System.Drawing.Size(107, 20);
            this.txtORNo.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(187, 117);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 29;
            this.label9.Text = "Area (SQ.M)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 116);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "O.R. No.";
            // 
            // dtpDate
            // 
            this.dtpDate.Enabled = false;
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(276, 88);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(84, 20);
            this.dtpDate.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(241, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "Date";
            // 
            // txtCertOcc
            // 
            this.txtCertOcc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCertOcc.Location = new System.Drawing.Point(117, 88);
            this.txtCertOcc.MaxLength = 100;
            this.txtCertOcc.Name = "txtCertOcc";
            this.txtCertOcc.ReadOnly = true;
            this.txtCertOcc.Size = new System.Drawing.Size(118, 20);
            this.txtCertOcc.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Cert. of Occupancy";
            // 
            // txtGrp
            // 
            this.txtGrp.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtGrp.Location = new System.Drawing.Point(54, 62);
            this.txtGrp.MaxLength = 100;
            this.txtGrp.Name = "txtGrp";
            this.txtGrp.ReadOnly = true;
            this.txtGrp.Size = new System.Drawing.Size(94, 20);
            this.txtGrp.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Character of Occupancy";
            // 
            // txtCharOcc
            // 
            this.txtCharOcc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCharOcc.Location = new System.Drawing.Point(139, 36);
            this.txtCharOcc.MaxLength = 100;
            this.txtCharOcc.Name = "txtCharOcc";
            this.txtCharOcc.ReadOnly = true;
            this.txtCharOcc.Size = new System.Drawing.Size(221, 20);
            this.txtCharOcc.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Group";
            // 
            // txtTaxYear
            // 
            this.txtTaxYear.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTaxYear.Location = new System.Drawing.Point(15, 20);
            this.txtTaxYear.Name = "txtTaxYear";
            this.txtTaxYear.ReadOnly = true;
            this.txtTaxYear.Size = new System.Drawing.Size(59, 20);
            this.txtTaxYear.TabIndex = 20;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtTaxYear);
            this.groupBox5.Location = new System.Drawing.Point(382, 180);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(90, 50);
            this.groupBox5.TabIndex = 32;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Tax Year";
            // 
            // btnExt
            // 
            this.btnExt.Location = new System.Drawing.Point(380, 468);
            this.btnExt.Name = "btnExt";
            this.btnExt.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnExt.Size = new System.Drawing.Size(92, 25);
            this.btnExt.TabIndex = 13;
            this.btnExt.Text = "Ext. Bldg.";
            this.btnExt.Values.ExtraText = "";
            this.btnExt.Values.Image = null;
            this.btnExt.Values.ImageStates.ImageCheckedNormal = null;
            this.btnExt.Values.ImageStates.ImageCheckedPressed = null;
            this.btnExt.Values.ImageStates.ImageCheckedTracking = null;
            this.btnExt.Values.Text = "Ext. Bldg.";
            this.btnExt.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmAnnualInspection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(482, 504);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnDiscard);
            this.Controls.Add(this.btnExt);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAnnualInspection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Annual Inspection";
            this.Load += new System.EventHandler(this.frmAnnualInspection_Load);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnNew;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDiscard;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPrint;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDelete;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEdit;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtMiddleInitial;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label label2;
        private BIN.BIN bin1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private System.Windows.Forms.RadioButton rdoNew;
        private System.Windows.Forms.GroupBox groupBox2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearchTmp;
        private System.Windows.Forms.TextBox txtTmpBin;
        private System.Windows.Forms.RadioButton rdoRenewal;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtCertOcc;
        private System.Windows.Forms.TextBox txtGrp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCharOcc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTaxYear;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtLoc;
        private System.Windows.Forms.TextBox txtStallNo;
        private System.Windows.Forms.TextBox txtORNo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtDTIName;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnExt;
    }
}