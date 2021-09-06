namespace Amellar.Modules.BadChecks
{
    partial class frmBadCheck
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
            this.btnRemove = new System.Windows.Forms.Button();
            this.grpBankCode = new System.Windows.Forms.GroupBox();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.txtOwnAdd = new System.Windows.Forms.TextBox();
            this.txtOwnName = new System.Windows.Forms.TextBox();
            this.txtBnsAdd = new System.Windows.Forms.TextBox();
            this.lblBnsAdd = new System.Windows.Forms.Label();
            this.lblOwnAdd = new System.Windows.Forms.Label();
            this.lblOwnName = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtChkNo = new System.Windows.Forms.TextBox();
            this.txtBankName = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtAcctNo = new System.Windows.Forms.TextBox();
            this.txtChkAmt = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnEditChk = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdoReason4 = new System.Windows.Forms.RadioButton();
            this.rdoReason3 = new System.Windows.Forms.RadioButton();
            this.rdoReason2 = new System.Windows.Forms.RadioButton();
            this.rdoReason1 = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoChk3 = new System.Windows.Forms.RadioButton();
            this.rdoChk2 = new System.Windows.Forms.RadioButton();
            this.rdoChk1 = new System.Windows.Forms.RadioButton();
            this.dtpChkDate = new System.Windows.Forms.DateTimePicker();
            this.txtMI = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBIN = new System.Windows.Forms.TextBox();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.grpBankCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRemove
            // 
            this.btnRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.ForeColor = System.Drawing.Color.Blue;
            this.btnRemove.Location = new System.Drawing.Point(13, 266);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(298, 23);
            this.btnRemove.TabIndex = 5;
            this.btnRemove.Text = "Remove Business from Restricted Businesses";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // grpBankCode
            // 
            this.grpBankCode.Controls.Add(this.txtBnsName);
            this.grpBankCode.Controls.Add(this.label5);
            this.grpBankCode.Controls.Add(this.txtBIN);
            this.grpBankCode.Controls.Add(this.label4);
            this.grpBankCode.Controls.Add(this.btnRemoveAll);
            this.grpBankCode.Controls.Add(this.dgvList);
            this.grpBankCode.Controls.Add(this.txtOwnAdd);
            this.grpBankCode.Controls.Add(this.btnRemove);
            this.grpBankCode.Controls.Add(this.txtOwnName);
            this.grpBankCode.Controls.Add(this.txtBnsAdd);
            this.grpBankCode.Controls.Add(this.lblBnsAdd);
            this.grpBankCode.Controls.Add(this.lblOwnAdd);
            this.grpBankCode.Controls.Add(this.lblOwnName);
            this.grpBankCode.Location = new System.Drawing.Point(12, 279);
            this.grpBankCode.Name = "grpBankCode";
            this.grpBankCode.Size = new System.Drawing.Size(549, 295);
            this.grpBankCode.TabIndex = 4;
            this.grpBankCode.TabStop = false;
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveAll.ForeColor = System.Drawing.Color.Blue;
            this.btnRemoveAll.Location = new System.Drawing.Point(317, 266);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(213, 23);
            this.btnRemoveAll.TabIndex = 11;
            this.btnRemoveAll.Text = "Remove All Restricted Business";
            this.btnRemoveAll.UseVisualStyleBackColor = true;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToResizeRows = false;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Location = new System.Drawing.Point(13, 19);
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.Size = new System.Drawing.Size(523, 138);
            this.dgvList.TabIndex = 11;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // txtOwnAdd
            // 
            this.txtOwnAdd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtOwnAdd.Enabled = false;
            this.txtOwnAdd.Location = new System.Drawing.Point(116, 240);
            this.txtOwnAdd.Name = "txtOwnAdd";
            this.txtOwnAdd.ReadOnly = true;
            this.txtOwnAdd.Size = new System.Drawing.Size(414, 20);
            this.txtOwnAdd.TabIndex = 10;
            // 
            // txtOwnName
            // 
            this.txtOwnName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtOwnName.Enabled = false;
            this.txtOwnName.Location = new System.Drawing.Point(116, 214);
            this.txtOwnName.Name = "txtOwnName";
            this.txtOwnName.ReadOnly = true;
            this.txtOwnName.Size = new System.Drawing.Size(414, 20);
            this.txtOwnName.TabIndex = 8;
            // 
            // txtBnsAdd
            // 
            this.txtBnsAdd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBnsAdd.Enabled = false;
            this.txtBnsAdd.Location = new System.Drawing.Point(116, 189);
            this.txtBnsAdd.Name = "txtBnsAdd";
            this.txtBnsAdd.ReadOnly = true;
            this.txtBnsAdd.Size = new System.Drawing.Size(414, 20);
            this.txtBnsAdd.TabIndex = 2;
            // 
            // lblBnsAdd
            // 
            this.lblBnsAdd.AutoSize = true;
            this.lblBnsAdd.Location = new System.Drawing.Point(23, 189);
            this.lblBnsAdd.Name = "lblBnsAdd";
            this.lblBnsAdd.Size = new System.Drawing.Size(90, 13);
            this.lblBnsAdd.TabIndex = 1;
            this.lblBnsAdd.Text = "Business Address";
            // 
            // lblOwnAdd
            // 
            this.lblOwnAdd.AutoSize = true;
            this.lblOwnAdd.Location = new System.Drawing.Point(23, 243);
            this.lblOwnAdd.Name = "lblOwnAdd";
            this.lblOwnAdd.Size = new System.Drawing.Size(86, 13);
            this.lblOwnAdd.TabIndex = 1;
            this.lblOwnAdd.Text = "Owner\'s Address";
            // 
            // lblOwnName
            // 
            this.lblOwnName.AutoSize = true;
            this.lblOwnName.Location = new System.Drawing.Point(23, 217);
            this.lblOwnName.Name = "lblOwnName";
            this.lblOwnName.Size = new System.Drawing.Size(76, 13);
            this.lblOwnName.TabIndex = 1;
            this.lblOwnName.Text = "Owner\'s Name";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(58, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Check No.";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 47);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Bank Name";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 72);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Last Name";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(356, 47);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Check Date";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(356, 72);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Acct. No.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "First Name";
            // 
            // txtChkNo
            // 
            this.txtChkNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtChkNo.Location = new System.Drawing.Point(80, 18);
            this.txtChkNo.Name = "txtChkNo";
            this.txtChkNo.Size = new System.Drawing.Size(96, 20);
            this.txtChkNo.TabIndex = 0;
            // 
            // txtBankName
            // 
            this.txtBankName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBankName.Location = new System.Drawing.Point(80, 44);
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.ReadOnly = true;
            this.txtBankName.Size = new System.Drawing.Size(262, 20);
            this.txtBankName.TabIndex = 2;
            // 
            // txtFirstName
            // 
            this.txtFirstName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFirstName.Location = new System.Drawing.Point(80, 94);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.ReadOnly = true;
            this.txtFirstName.Size = new System.Drawing.Size(201, 20);
            this.txtFirstName.TabIndex = 2;
            // 
            // txtLastName
            // 
            this.txtLastName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLastName.Location = new System.Drawing.Point(80, 69);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.ReadOnly = true;
            this.txtLastName.Size = new System.Drawing.Size(262, 20);
            this.txtLastName.TabIndex = 9;
            // 
            // txtAcctNo
            // 
            this.txtAcctNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAcctNo.Location = new System.Drawing.Point(451, 69);
            this.txtAcctNo.Name = "txtAcctNo";
            this.txtAcctNo.ReadOnly = true;
            this.txtAcctNo.Size = new System.Drawing.Size(85, 20);
            this.txtAcctNo.TabIndex = 8;
            // 
            // txtChkAmt
            // 
            this.txtChkAmt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtChkAmt.Location = new System.Drawing.Point(451, 94);
            this.txtChkAmt.Name = "txtChkAmt";
            this.txtChkAmt.ReadOnly = true;
            this.txtChkAmt.Size = new System.Drawing.Size(85, 20);
            this.txtChkAmt.TabIndex = 10;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(182, 16);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(77, 23);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnInsert);
            this.groupBox1.Controls.Add(this.btnEditChk);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.dtpChkDate);
            this.groupBox1.Controls.Add(this.txtMI);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnAddNew);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtChkAmt);
            this.groupBox1.Controls.Add(this.txtAcctNo);
            this.groupBox1.Controls.Add(this.txtLastName);
            this.groupBox1.Controls.Add(this.txtFirstName);
            this.groupBox1.Controls.Add(this.txtBankName);
            this.groupBox1.Controls.Add(this.txtChkNo);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(549, 276);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // btnInsert
            // 
            this.btnInsert.Enabled = false;
            this.btnInsert.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInsert.ForeColor = System.Drawing.Color.Blue;
            this.btnInsert.Location = new System.Drawing.Point(13, 247);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(268, 23);
            this.btnInsert.TabIndex = 18;
            this.btnInsert.Text = "Insert Account owner to Block List";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnEditChk
            // 
            this.btnEditChk.Enabled = false;
            this.btnEditChk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditChk.Location = new System.Drawing.Point(448, 136);
            this.btnEditChk.Name = "btnEditChk";
            this.btnEditChk.Size = new System.Drawing.Size(88, 21);
            this.btnEditChk.TabIndex = 17;
            this.btnEditChk.Text = "Edit";
            this.btnEditChk.UseVisualStyleBackColor = true;
            this.btnEditChk.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdoReason4);
            this.groupBox3.Controls.Add(this.rdoReason3);
            this.groupBox3.Controls.Add(this.rdoReason2);
            this.groupBox3.Controls.Add(this.rdoReason1);
            this.groupBox3.Location = new System.Drawing.Point(265, 132);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(139, 110);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = " Reason ";
            // 
            // rdoReason4
            // 
            this.rdoReason4.AutoSize = true;
            this.rdoReason4.Enabled = false;
            this.rdoReason4.Location = new System.Drawing.Point(22, 87);
            this.rdoReason4.Name = "rdoReason4";
            this.rdoReason4.Size = new System.Drawing.Size(47, 17);
            this.rdoReason4.TabIndex = 3;
            this.rdoReason4.TabStop = true;
            this.rdoReason4.Text = "SPO";
            this.rdoReason4.UseVisualStyleBackColor = true;
            // 
            // rdoReason3
            // 
            this.rdoReason3.AutoSize = true;
            this.rdoReason3.Enabled = false;
            this.rdoReason3.Location = new System.Drawing.Point(22, 65);
            this.rdoReason3.Name = "rdoReason3";
            this.rdoReason3.Size = new System.Drawing.Size(56, 17);
            this.rdoReason3.TabIndex = 2;
            this.rdoReason3.TabStop = true;
            this.rdoReason3.Text = "DAUD";
            this.rdoReason3.UseVisualStyleBackColor = true;
            // 
            // rdoReason2
            // 
            this.rdoReason2.AutoSize = true;
            this.rdoReason2.Enabled = false;
            this.rdoReason2.Location = new System.Drawing.Point(22, 42);
            this.rdoReason2.Name = "rdoReason2";
            this.rdoReason2.Size = new System.Drawing.Size(49, 17);
            this.rdoReason2.TabIndex = 1;
            this.rdoReason2.TabStop = true;
            this.rdoReason2.Text = "DAIF";
            this.rdoReason2.UseVisualStyleBackColor = true;
            // 
            // rdoReason1
            // 
            this.rdoReason1.AutoSize = true;
            this.rdoReason1.Enabled = false;
            this.rdoReason1.Location = new System.Drawing.Point(22, 19);
            this.rdoReason1.Name = "rdoReason1";
            this.rdoReason1.Size = new System.Drawing.Size(100, 17);
            this.rdoReason1.TabIndex = 0;
            this.rdoReason1.TabStop = true;
            this.rdoReason1.Text = "Account Closed";
            this.rdoReason1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdoChk3);
            this.groupBox2.Controls.Add(this.rdoChk2);
            this.groupBox2.Controls.Add(this.rdoChk1);
            this.groupBox2.Location = new System.Drawing.Point(13, 132);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(234, 110);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Check Type ";
            // 
            // rdoChk3
            // 
            this.rdoChk3.AutoSize = true;
            this.rdoChk3.Enabled = false;
            this.rdoChk3.Location = new System.Drawing.Point(21, 74);
            this.rdoChk3.Name = "rdoChk3";
            this.rdoChk3.Size = new System.Drawing.Size(108, 17);
            this.rdoChk3.TabIndex = 6;
            this.rdoChk3.TabStop = true;
            this.rdoChk3.Text = "Manager\'s Check";
            this.rdoChk3.UseVisualStyleBackColor = true;
            this.rdoChk3.CheckedChanged += new System.EventHandler(this.rdoChk3_CheckedChanged);
            // 
            // rdoChk2
            // 
            this.rdoChk2.AutoSize = true;
            this.rdoChk2.Enabled = false;
            this.rdoChk2.Location = new System.Drawing.Point(21, 51);
            this.rdoChk2.Name = "rdoChk2";
            this.rdoChk2.Size = new System.Drawing.Size(185, 17);
            this.rdoChk2.TabIndex = 5;
            this.rdoChk2.TabStop = true;
            this.rdoChk2.Text = "Corporate Check/Personal Check";
            this.rdoChk2.UseVisualStyleBackColor = true;
            this.rdoChk2.CheckedChanged += new System.EventHandler(this.rdoChk2_CheckedChanged);
            // 
            // rdoChk1
            // 
            this.rdoChk1.AutoSize = true;
            this.rdoChk1.Enabled = false;
            this.rdoChk1.Location = new System.Drawing.Point(21, 28);
            this.rdoChk1.Name = "rdoChk1";
            this.rdoChk1.Size = new System.Drawing.Size(101, 17);
            this.rdoChk1.TabIndex = 4;
            this.rdoChk1.TabStop = true;
            this.rdoChk1.Text = "Cashier\'s Check";
            this.rdoChk1.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.rdoChk1.UseVisualStyleBackColor = true;
            this.rdoChk1.CheckedChanged += new System.EventHandler(this.rdoChk1_CheckedChanged);
            // 
            // dtpChkDate
            // 
            this.dtpChkDate.Enabled = false;
            this.dtpChkDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpChkDate.Location = new System.Drawing.Point(451, 44);
            this.dtpChkDate.Name = "dtpChkDate";
            this.dtpChkDate.Size = new System.Drawing.Size(85, 20);
            this.dtpChkDate.TabIndex = 14;
            // 
            // txtMI
            // 
            this.txtMI.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMI.Location = new System.Drawing.Point(317, 94);
            this.txtMI.Name = "txtMI";
            this.txtMI.ReadOnly = true;
            this.txtMI.Size = new System.Drawing.Size(25, 20);
            this.txtMI.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(286, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "M.I.";
            // 
            // btnAddNew
            // 
            this.btnAddNew.Location = new System.Drawing.Point(266, 16);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(77, 23);
            this.btnAddNew.TabIndex = 3;
            this.btnAddNew.Text = "Add New";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(356, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Amount of Check";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 166);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "BIN";
            // 
            // txtBIN
            // 
            this.txtBIN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBIN.Enabled = false;
            this.txtBIN.Location = new System.Drawing.Point(56, 163);
            this.txtBIN.Name = "txtBIN";
            this.txtBIN.ReadOnly = true;
            this.txtBIN.Size = new System.Drawing.Size(120, 20);
            this.txtBIN.TabIndex = 13;
            // 
            // txtBnsName
            // 
            this.txtBnsName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBnsName.Enabled = false;
            this.txtBnsName.Location = new System.Drawing.Point(253, 163);
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(277, 20);
            this.txtBnsName.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(191, 166);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Bns Name";
            // 
            // frmBadCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 586);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpBankCode);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBadCheck";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bad Checks";
            this.Load += new System.EventHandler(this.frmBadCheck_Load);
            this.grpBankCode.ResumeLayout(false);
            this.grpBankCode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.GroupBox grpBankCode;
        private System.Windows.Forms.Button btnRemoveAll;
        private System.Windows.Forms.TextBox txtOwnAdd;
        private System.Windows.Forms.TextBox txtOwnName;
        private System.Windows.Forms.TextBox txtBnsAdd;
        private System.Windows.Forms.Label lblBnsAdd;
        private System.Windows.Forms.Label lblOwnAdd;
        private System.Windows.Forms.Label lblOwnName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtChkNo;
        private System.Windows.Forms.TextBox txtBankName;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtAcctNo;
        private System.Windows.Forms.TextBox txtChkAmt;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtMI;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dtpChkDate;
        private System.Windows.Forms.RadioButton rdoChk3;
        private System.Windows.Forms.RadioButton rdoChk2;
        private System.Windows.Forms.RadioButton rdoChk1;
        private System.Windows.Forms.RadioButton rdoReason4;
        private System.Windows.Forms.RadioButton rdoReason3;
        private System.Windows.Forms.RadioButton rdoReason2;
        private System.Windows.Forms.RadioButton rdoReason1;
        private System.Windows.Forms.Button btnEditChk;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.TextBox txtBnsName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBIN;
        private System.Windows.Forms.Label label4;
    }
}

