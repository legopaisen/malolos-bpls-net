namespace Amellar.BPLS.Billing
{
    partial class frmBilling
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtTaxYear = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtQtr = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBusinessName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOwnersName = new System.Windows.Forms.TextBox();
            this.dgvTaxFees = new System.Windows.Forms.DataGridView();
            this.Compute = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TaxFees = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeesCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeesType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnBusinessAgent = new System.Windows.Forms.Button();
            this.cmbBnsType = new System.Windows.Forms.ComboBox();
            this.dgvAddlInfo = new System.Windows.Forms.DataGridView();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBnsCode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCapital = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtGross = new System.Windows.Forms.TextBox();
            this.txtPreGross = new System.Windows.Forms.TextBox();
            this.txtVATGross = new System.Windows.Forms.TextBox();
            this.lblGross = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnEditAddlInfo = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.btnCancelAddlInfo = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRetrieveBilling = new System.Windows.Forms.Button();
            this.btnViewSOA = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtAdjGross = new System.Windows.Forms.TextBox();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.kryptonHeader3 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.btnDelqDues = new System.Windows.Forms.Button();
            this.lblCaption = new System.Windows.Forms.Label();
            this.txtPrevGrossCapital = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtBusinessAdd = new System.Windows.Forms.TextBox();
            this.bin = new BIN.BIN();
            this.frameWithShadow1 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow2 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow3 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow4 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow5 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow6 = new Amellar.Common.SOA.FrameWithShadow();
            this.btnPrevGrossCapital = new System.Windows.Forms.Button();
            this.chkTagReass = new System.Windows.Forms.CheckBox();
            this.btnPayHist = new System.Windows.Forms.Button();
            this.frameWithShadow7 = new Amellar.Common.SOA.FrameWithShadow();
            this.chkCTC = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaxFees)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAddlInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(148, 45);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtTaxYear
            // 
            this.txtTaxYear.Location = new System.Drawing.Point(406, 46);
            this.txtTaxYear.Name = "txtTaxYear";
            this.txtTaxYear.ReadOnly = true;
            this.txtTaxYear.Size = new System.Drawing.Size(54, 20);
            this.txtTaxYear.TabIndex = 2;
            this.txtTaxYear.TextChanged += new System.EventHandler(this.txtTaxYear_TextChanged);
            this.txtTaxYear.Leave += new System.EventHandler(this.txtTaxYear_Leave);
            this.txtTaxYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTaxYear_KeyPress);
            this.txtTaxYear.Enter += new System.EventHandler(this.txtTaxYear_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(354, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tax Year";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(466, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Quarter";
            // 
            // txtQtr
            // 
            this.txtQtr.Location = new System.Drawing.Point(514, 46);
            this.txtQtr.Name = "txtQtr";
            this.txtQtr.ReadOnly = true;
            this.txtQtr.Size = new System.Drawing.Size(17, 20);
            this.txtQtr.TabIndex = 5;
            this.txtQtr.TextChanged += new System.EventHandler(this.txtQtr_TextChanged);
            this.txtQtr.Leave += new System.EventHandler(this.txtQtr_Leave);
            this.txtQtr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQtr_KeyPress);
            this.txtQtr.Enter += new System.EventHandler(this.txtQtr_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Business Name";
            // 
            // txtBusinessName
            // 
            this.txtBusinessName.Location = new System.Drawing.Point(111, 89);
            this.txtBusinessName.Name = "txtBusinessName";
            this.txtBusinessName.ReadOnly = true;
            this.txtBusinessName.Size = new System.Drawing.Size(423, 20);
            this.txtBusinessName.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Tax Payer\'s Name";
            // 
            // txtOwnersName
            // 
            this.txtOwnersName.Location = new System.Drawing.Point(111, 135);
            this.txtOwnersName.Name = "txtOwnersName";
            this.txtOwnersName.ReadOnly = true;
            this.txtOwnersName.Size = new System.Drawing.Size(423, 20);
            this.txtOwnersName.TabIndex = 11;
            // 
            // dgvTaxFees
            // 
            this.dgvTaxFees.AllowUserToAddRows = false;
            this.dgvTaxFees.AllowUserToDeleteRows = false;
            this.dgvTaxFees.AllowUserToResizeColumns = false;
            this.dgvTaxFees.AllowUserToResizeRows = false;
            this.dgvTaxFees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTaxFees.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Compute,
            this.TaxFees,
            this.Amount,
            this.FeesCode,
            this.FeesType});
            this.dgvTaxFees.Location = new System.Drawing.Point(6, 454);
            this.dgvTaxFees.Name = "dgvTaxFees";
            this.dgvTaxFees.RowHeadersVisible = false;
            this.dgvTaxFees.Size = new System.Drawing.Size(381, 225);
            this.dgvTaxFees.TabIndex = 27;
            this.dgvTaxFees.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTaxFees_CellContentDoubleClick);
            this.dgvTaxFees.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTaxFees_CellContentClick);
            // 
            // Compute
            // 
            this.Compute.HeaderText = "";
            this.Compute.Name = "Compute";
            this.Compute.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Compute.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Compute.Width = 20;
            // 
            // TaxFees
            // 
            this.TaxFees.HeaderText = "Taxes, Fees and Additional Charges";
            this.TaxFees.Name = "TaxFees";
            this.TaxFees.ReadOnly = true;
            this.TaxFees.Width = 250;
            // 
            // Amount
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Amount.DefaultCellStyle = dataGridViewCellStyle1;
            this.Amount.HeaderText = "Amount";
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            this.Amount.Width = 80;
            // 
            // FeesCode
            // 
            this.FeesCode.HeaderText = "";
            this.FeesCode.Name = "FeesCode";
            this.FeesCode.ReadOnly = true;
            this.FeesCode.Visible = false;
            // 
            // FeesType
            // 
            this.FeesType.HeaderText = "";
            this.FeesType.Name = "FeesType";
            this.FeesType.ReadOnly = true;
            this.FeesType.Visible = false;
            // 
            // txtTotal
            // 
            this.txtTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotal.Location = new System.Drawing.Point(266, 685);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.ReadOnly = true;
            this.txtTotal.Size = new System.Drawing.Size(121, 22);
            this.txtTotal.TabIndex = 27;
            this.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotal.TextChanged += new System.EventHandler(this.txtTotal_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(229, 690);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "Total";
            // 
            // btnBusinessAgent
            // 
            this.btnBusinessAgent.Enabled = false;
            this.btnBusinessAgent.Location = new System.Drawing.Point(414, 575);
            this.btnBusinessAgent.Name = "btnBusinessAgent";
            this.btnBusinessAgent.Size = new System.Drawing.Size(107, 23);
            this.btnBusinessAgent.TabIndex = 31;
            this.btnBusinessAgent.Text = "&Business Agent";
            this.btnBusinessAgent.UseVisualStyleBackColor = true;
            this.btnBusinessAgent.Visible = false;
            // 
            // cmbBnsType
            // 
            this.cmbBnsType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBnsType.FormattingEnabled = true;
            this.cmbBnsType.Location = new System.Drawing.Point(92, 213);
            this.cmbBnsType.Name = "cmbBnsType";
            this.cmbBnsType.Size = new System.Drawing.Size(212, 21);
            this.cmbBnsType.TabIndex = 14;
            this.cmbBnsType.SelectedIndexChanged += new System.EventHandler(this.cmbBnsType_SelectedIndexChanged);
            // 
            // dgvAddlInfo
            // 
            this.dgvAddlInfo.AllowUserToAddRows = false;
            this.dgvAddlInfo.AllowUserToDeleteRows = false;
            this.dgvAddlInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAddlInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Code,
            this.Desc,
            this.Type,
            this.Unit});
            this.dgvAddlInfo.Cursor = System.Windows.Forms.Cursors.Default;
            this.dgvAddlInfo.Location = new System.Drawing.Point(16, 262);
            this.dgvAddlInfo.Name = "dgvAddlInfo";
            this.dgvAddlInfo.RowHeadersVisible = false;
            this.dgvAddlInfo.Size = new System.Drawing.Size(352, 137);
            this.dgvAddlInfo.TabIndex = 0;
            this.dgvAddlInfo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAddlInfo_CellValueChanged);
            // 
            // Code
            // 
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.Visible = false;
            // 
            // Desc
            // 
            this.Desc.HeaderText = "Description";
            this.Desc.Name = "Desc";
            this.Desc.Width = 250;
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.Width = 35;
            // 
            // Unit
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.NullValue = null;
            this.Unit.DefaultCellStyle = dataGridViewCellStyle2;
            this.Unit.HeaderText = "Unit";
            this.Unit.Name = "Unit";
            this.Unit.Width = 40;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(310, 217);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Business Code";
            // 
            // txtBnsCode
            // 
            this.txtBnsCode.Location = new System.Drawing.Point(387, 214);
            this.txtBnsCode.Name = "txtBnsCode";
            this.txtBnsCode.ReadOnly = true;
            this.txtBnsCode.Size = new System.Drawing.Size(66, 20);
            this.txtBnsCode.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 218);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Business Type";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(370, 280);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Capital";
            // 
            // txtCapital
            // 
            this.txtCapital.Location = new System.Drawing.Point(429, 277);
            this.txtCapital.Name = "txtCapital";
            this.txtCapital.ReadOnly = true;
            this.txtCapital.Size = new System.Drawing.Size(100, 20);
            this.txtCapital.TabIndex = 19;
            this.txtCapital.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCapital.Leave += new System.EventHandler(this.txtCapital_Leave);
            this.txtCapital.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCapital_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(370, 306);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Gross";
            // 
            // txtGross
            // 
            this.txtGross.Location = new System.Drawing.Point(429, 303);
            this.txtGross.Name = "txtGross";
            this.txtGross.ReadOnly = true;
            this.txtGross.Size = new System.Drawing.Size(100, 20);
            this.txtGross.TabIndex = 21;
            this.txtGross.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtGross.TextChanged += new System.EventHandler(this.txtGross_TextChanged);
            this.txtGross.Leave += new System.EventHandler(this.txtGross_Leave);
            this.txtGross.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGross_KeyPress);
            // 
            // txtPreGross
            // 
            this.txtPreGross.Location = new System.Drawing.Point(429, 329);
            this.txtPreGross.Name = "txtPreGross";
            this.txtPreGross.ReadOnly = true;
            this.txtPreGross.Size = new System.Drawing.Size(100, 20);
            this.txtPreGross.TabIndex = 22;
            this.txtPreGross.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPreGross.Leave += new System.EventHandler(this.txtPreGross_Leave);
            this.txtPreGross.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPreGross_KeyPress);
            // 
            // txtVATGross
            // 
            this.txtVATGross.Location = new System.Drawing.Point(429, 355);
            this.txtVATGross.Name = "txtVATGross";
            this.txtVATGross.ReadOnly = true;
            this.txtVATGross.Size = new System.Drawing.Size(100, 20);
            this.txtVATGross.TabIndex = 23;
            this.txtVATGross.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtVATGross.Leave += new System.EventHandler(this.txtVATGross_Leave);
            this.txtVATGross.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVATGross_KeyPress);
            // 
            // lblGross
            // 
            this.lblGross.AutoSize = true;
            this.lblGross.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGross.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblGross.Location = new System.Drawing.Point(370, 332);
            this.lblGross.Name = "lblGross";
            this.lblGross.Size = new System.Drawing.Size(53, 13);
            this.lblGross.TabIndex = 24;
            this.lblGross.Text = "Pre-Gross";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(370, 358);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 25;
            this.label11.Text = "VAT-Gross";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(453, 218);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Status";
            // 
            // btnEditAddlInfo
            // 
            this.btnEditAddlInfo.Enabled = false;
            this.btnEditAddlInfo.Location = new System.Drawing.Point(376, 377);
            this.btnEditAddlInfo.Name = "btnEditAddlInfo";
            this.btnEditAddlInfo.Size = new System.Drawing.Size(75, 23);
            this.btnEditAddlInfo.TabIndex = 26;
            this.btnEditAddlInfo.Text = "&Edit";
            this.btnEditAddlInfo.UseVisualStyleBackColor = true;
            this.btnEditAddlInfo.Click += new System.EventHandler(this.btnEditAddlInfo_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(493, 213);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(36, 20);
            this.txtStatus.TabIndex = 9;
            // 
            // btnCancelAddlInfo
            // 
            this.btnCancelAddlInfo.Enabled = false;
            this.btnCancelAddlInfo.Location = new System.Drawing.Point(457, 377);
            this.btnCancelAddlInfo.Name = "btnCancelAddlInfo";
            this.btnCancelAddlInfo.Size = new System.Drawing.Size(75, 23);
            this.btnCancelAddlInfo.TabIndex = 27;
            this.btnCancelAddlInfo.Text = "&Cancel";
            this.btnCancelAddlInfo.UseVisualStyleBackColor = true;
            this.btnCancelAddlInfo.Click += new System.EventHandler(this.btnCancelAddlInfo_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(414, 675);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(107, 23);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "Cl&ose";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRetrieveBilling
            // 
            this.btnRetrieveBilling.BackColor = System.Drawing.SystemColors.Control;
            this.btnRetrieveBilling.Enabled = false;
            this.btnRetrieveBilling.Location = new System.Drawing.Point(414, 466);
            this.btnRetrieveBilling.Name = "btnRetrieveBilling";
            this.btnRetrieveBilling.Size = new System.Drawing.Size(107, 23);
            this.btnRetrieveBilling.TabIndex = 28;
            this.btnRetrieveBilling.Text = "&Retrieve Billing";
            this.btnRetrieveBilling.UseVisualStyleBackColor = false;
            this.btnRetrieveBilling.Click += new System.EventHandler(this.btnRetrieveBilling_Click);
            // 
            // btnViewSOA
            // 
            this.btnViewSOA.Enabled = false;
            this.btnViewSOA.Location = new System.Drawing.Point(414, 524);
            this.btnViewSOA.Name = "btnViewSOA";
            this.btnViewSOA.Size = new System.Drawing.Size(107, 23);
            this.btnViewSOA.TabIndex = 29;
            this.btnViewSOA.Text = "View SOA";
            this.btnViewSOA.UseVisualStyleBackColor = true;
            this.btnViewSOA.Click += new System.EventHandler(this.btnViewSOA_Click);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(414, 495);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(107, 23);
            this.btnSave.TabIndex = 30;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtAdjGross
            // 
            this.txtAdjGross.Location = new System.Drawing.Point(429, 329);
            this.txtAdjGross.Name = "txtAdjGross";
            this.txtAdjGross.ReadOnly = true;
            this.txtAdjGross.Size = new System.Drawing.Size(100, 20);
            this.txtAdjGross.TabIndex = 48;
            this.txtAdjGross.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtAdjGross.Visible = false;
            this.txtAdjGross.Leave += new System.EventHandler(this.txtAdjGross_Leave);
            this.txtAdjGross.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAdjGross_KeyPress);
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader1.Location = new System.Drawing.Point(7, 5);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonHeader1.Size = new System.Drawing.Size(534, 30);
            this.kryptonHeader1.TabIndex = 49;
            this.kryptonHeader1.Text = "Business Information";
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Business Information";
            this.kryptonHeader1.Values.Image = null;
            this.kryptonHeader1.Paint += new System.Windows.Forms.PaintEventHandler(this.kryptonHeader1_Paint);
            // 
            // kryptonHeader2
            // 
            this.kryptonHeader2.AutoSize = false;
            this.kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader2.Location = new System.Drawing.Point(7, 171);
            this.kryptonHeader2.Name = "kryptonHeader2";
            this.kryptonHeader2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonHeader2.Size = new System.Drawing.Size(534, 30);
            this.kryptonHeader2.TabIndex = 50;
            this.kryptonHeader2.Text = "Additional Information";
            this.kryptonHeader2.Values.Description = "";
            this.kryptonHeader2.Values.Heading = "Additional Information";
            this.kryptonHeader2.Values.Image = null;
            // 
            // kryptonHeader3
            // 
            this.kryptonHeader3.AutoSize = false;
            this.kryptonHeader3.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader3.Location = new System.Drawing.Point(6, 420);
            this.kryptonHeader3.Name = "kryptonHeader3";
            this.kryptonHeader3.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonHeader3.Size = new System.Drawing.Size(534, 30);
            this.kryptonHeader3.TabIndex = 51;
            this.kryptonHeader3.Text = "Taxes, Fees and Additional Charges";
            this.kryptonHeader3.Values.Description = "";
            this.kryptonHeader3.Values.Heading = "Taxes, Fees and Additional Charges";
            this.kryptonHeader3.Values.Image = null;
            // 
            // btnDelqDues
            // 
            this.btnDelqDues.BackColor = System.Drawing.SystemColors.Control;
            this.btnDelqDues.Enabled = false;
            this.btnDelqDues.Location = new System.Drawing.Point(229, 45);
            this.btnDelqDues.Name = "btnDelqDues";
            this.btnDelqDues.Size = new System.Drawing.Size(107, 23);
            this.btnDelqDues.TabIndex = 28;
            this.btnDelqDues.Text = "&Delinquent Dues";
            this.btnDelqDues.UseVisualStyleBackColor = false;
            this.btnDelqDues.Visible = false;
            this.btnDelqDues.Click += new System.EventHandler(this.btnDelqDues_Click);
            // 
            // lblCaption
            // 
            this.lblCaption.AutoSize = true;
            this.lblCaption.Location = new System.Drawing.Point(16, 242);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(71, 13);
            this.lblCaption.TabIndex = 8;
            this.lblCaption.Text = "Capital/Gross";
            // 
            // txtPrevGrossCapital
            // 
            this.txtPrevGrossCapital.Location = new System.Drawing.Point(102, 237);
            this.txtPrevGrossCapital.Name = "txtPrevGrossCapital";
            this.txtPrevGrossCapital.ReadOnly = true;
            this.txtPrevGrossCapital.Size = new System.Drawing.Size(101, 20);
            this.txtPrevGrossCapital.TabIndex = 16;
            this.txtPrevGrossCapital.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(16, 115);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(90, 13);
            this.label13.TabIndex = 10;
            this.label13.Text = "Business Address";
            // 
            // txtBusinessAdd
            // 
            this.txtBusinessAdd.Location = new System.Drawing.Point(111, 112);
            this.txtBusinessAdd.Name = "txtBusinessAdd";
            this.txtBusinessAdd.ReadOnly = true;
            this.txtBusinessAdd.Size = new System.Drawing.Size(423, 20);
            this.txtBusinessAdd.TabIndex = 11;
            // 
            // bin
            // 
            this.bin.GetBINSeries = "";
            this.bin.GetDistCode = "";
            this.bin.GetLGUCode = "";
            this.bin.GetTaxYear = "";
            this.bin.Location = new System.Drawing.Point(6, 46);
            this.bin.Name = "bin";
            this.bin.Size = new System.Drawing.Size(138, 20);
            this.bin.TabIndex = 0;
            // 
            // frameWithShadow1
            // 
            this.frameWithShadow1.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow1.Location = new System.Drawing.Point(345, 40);
            this.frameWithShadow1.Name = "frameWithShadow1";
            this.frameWithShadow1.Size = new System.Drawing.Size(197, 38);
            this.frameWithShadow1.TabIndex = 52;
            // 
            // frameWithShadow2
            // 
            this.frameWithShadow2.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow2.Location = new System.Drawing.Point(7, 81);
            this.frameWithShadow2.Name = "frameWithShadow2";
            this.frameWithShadow2.Size = new System.Drawing.Size(534, 87);
            this.frameWithShadow2.TabIndex = 53;
            // 
            // frameWithShadow3
            // 
            this.frameWithShadow3.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow3.Location = new System.Drawing.Point(6, 204);
            this.frameWithShadow3.Name = "frameWithShadow3";
            this.frameWithShadow3.Size = new System.Drawing.Size(535, 209);
            this.frameWithShadow3.TabIndex = 54;
            // 
            // frameWithShadow4
            // 
            this.frameWithShadow4.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow4.Location = new System.Drawing.Point(396, 457);
            this.frameWithShadow4.Name = "frameWithShadow4";
            this.frameWithShadow4.Size = new System.Drawing.Size(144, 105);
            this.frameWithShadow4.TabIndex = 55;
            // 
            // frameWithShadow5
            // 
            this.frameWithShadow5.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow5.Location = new System.Drawing.Point(396, 564);
            this.frameWithShadow5.Name = "frameWithShadow5";
            this.frameWithShadow5.Size = new System.Drawing.Size(144, 48);
            this.frameWithShadow5.TabIndex = 56;
            this.frameWithShadow5.Visible = false;
            // 
            // frameWithShadow6
            // 
            this.frameWithShadow6.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow6.Location = new System.Drawing.Point(396, 665);
            this.frameWithShadow6.Name = "frameWithShadow6";
            this.frameWithShadow6.Size = new System.Drawing.Size(144, 48);
            this.frameWithShadow6.TabIndex = 57;
            // 
            // btnPrevGrossCapital
            // 
            this.btnPrevGrossCapital.Enabled = false;
            this.btnPrevGrossCapital.Location = new System.Drawing.Point(209, 235);
            this.btnPrevGrossCapital.Name = "btnPrevGrossCapital";
            this.btnPrevGrossCapital.Size = new System.Drawing.Size(75, 23);
            this.btnPrevGrossCapital.TabIndex = 58;
            this.btnPrevGrossCapital.Text = "Edit";
            this.btnPrevGrossCapital.UseVisualStyleBackColor = true;
            this.btnPrevGrossCapital.Click += new System.EventHandler(this.btnPrevGrossCapital_Click);
            // 
            // chkTagReass
            // 
            this.chkTagReass.AutoSize = true;
            this.chkTagReass.Enabled = false;
            this.chkTagReass.Location = new System.Drawing.Point(373, 241);
            this.chkTagReass.Name = "chkTagReass";
            this.chkTagReass.Size = new System.Drawing.Size(135, 17);
            this.chkTagReass.TabIndex = 59;
            this.chkTagReass.Text = "Tag for Re-assessment";
            this.chkTagReass.UseVisualStyleBackColor = true;
            this.chkTagReass.Visible = false;
            // 
            // btnPayHist
            // 
            this.btnPayHist.Location = new System.Drawing.Point(414, 624);
            this.btnPayHist.Name = "btnPayHist";
            this.btnPayHist.Size = new System.Drawing.Size(107, 23);
            this.btnPayHist.TabIndex = 60;
            this.btnPayHist.Text = "Payment History";
            this.btnPayHist.UseVisualStyleBackColor = true;
            this.btnPayHist.Click += new System.EventHandler(this.btnPayHist_Click);
            // 
            // frameWithShadow7
            // 
            this.frameWithShadow7.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow7.Location = new System.Drawing.Point(396, 614);
            this.frameWithShadow7.Name = "frameWithShadow7";
            this.frameWithShadow7.Size = new System.Drawing.Size(144, 48);
            this.frameWithShadow7.TabIndex = 61;
            // 
            // chkCTC
            // 
            this.chkCTC.AutoSize = true;
            this.chkCTC.Enabled = false;
            this.chkCTC.Location = new System.Drawing.Point(373, 260);
            this.chkCTC.Name = "chkCTC";
            this.chkCTC.Size = new System.Drawing.Size(71, 17);
            this.chkCTC.TabIndex = 62;
            this.chkCTC.Text = "CTC Paid";
            this.chkCTC.UseVisualStyleBackColor = true;
            this.chkCTC.Visible = false;
            // 
            // frmBilling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(547, 717);
            this.Controls.Add(this.chkCTC);
            this.Controls.Add(this.btnPayHist);
            this.Controls.Add(this.frameWithShadow7);
            this.Controls.Add(this.chkTagReass);
            this.Controls.Add(this.btnPrevGrossCapital);
            this.Controls.Add(this.kryptonHeader3);
            this.Controls.Add(this.kryptonHeader2);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnViewSOA);
            this.Controls.Add(this.btnCancelAddlInfo);
            this.Controls.Add(this.btnDelqDues);
            this.Controls.Add(this.btnRetrieveBilling);
            this.Controls.Add(this.btnBusinessAgent);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btnEditAddlInfo);
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.lblCaption);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dgvTaxFees);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblGross);
            this.Controls.Add(this.txtBusinessAdd);
            this.Controls.Add(this.txtOwnersName);
            this.Controls.Add(this.txtVATGross);
            this.Controls.Add(this.txtQtr);
            this.Controls.Add(this.txtPreGross);
            this.Controls.Add(this.txtGross);
            this.Controls.Add(this.txtTaxYear);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCapital);
            this.Controls.Add(this.txtBusinessName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPrevGrossCapital);
            this.Controls.Add(this.txtBnsCode);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.bin);
            this.Controls.Add(this.dgvAddlInfo);
            this.Controls.Add(this.cmbBnsType);
            this.Controls.Add(this.txtAdjGross);
            this.Controls.Add(this.frameWithShadow1);
            this.Controls.Add(this.frameWithShadow2);
            this.Controls.Add(this.frameWithShadow3);
            this.Controls.Add(this.frameWithShadow4);
            this.Controls.Add(this.frameWithShadow5);
            this.Controls.Add(this.frameWithShadow6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBilling";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Billing";
            this.Load += new System.EventHandler(this.frmBilling_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBilling_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaxFees)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAddlInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BIN.BIN bin;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtTaxYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtQtr;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBusinessName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtOwnersName;
        private System.Windows.Forms.DataGridView dgvTaxFees;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnBusinessAgent;
        private System.Windows.Forms.ComboBox cmbBnsType;
        private System.Windows.Forms.DataGridView dgvAddlInfo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBnsCode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCapital;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtGross;
        private System.Windows.Forms.TextBox txtPreGross;
        private System.Windows.Forms.TextBox txtVATGross;
        private System.Windows.Forms.Label lblGross;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Button btnCancelAddlInfo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnViewSOA;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.TextBox txtAdjGross;
        public ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        public ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader2;
        public ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader3;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow1;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow2;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow3;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow4;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow5;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow6;
        public System.Windows.Forms.Button btnEditAddlInfo;
        public System.Windows.Forms.Button btnRetrieveBilling;
        public System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.Button btnDelqDues;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.TextBox txtPrevGrossCapital;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtBusinessAdd;
        public System.Windows.Forms.Button btnPrevGrossCapital;
        public System.Windows.Forms.CheckBox chkTagReass;
        private System.Windows.Forms.Button btnPayHist;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow7;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Compute;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxFees;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeesCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeesType;
        public System.Windows.Forms.CheckBox chkCTC;
    }
}