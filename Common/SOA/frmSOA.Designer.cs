namespace Amellar.Common.SOA
{
    partial class frmSOA
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBin = new System.Windows.Forms.TextBox();
            this.txtLName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMI = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBnsAdd = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTaxYear = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dgvTaxFees = new System.Windows.Forms.DataGridView();
            this.year = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.term = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qtr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.particulars = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.due = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.surch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.due_state = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qtr_to_pay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fees_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkQtr = new System.Windows.Forms.CheckBox();
            this.chkFull = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grpQtr = new System.Windows.Forms.GroupBox();
            this.btn4th = new System.Windows.Forms.CheckBox();
            this.btn3rd = new System.Windows.Forms.CheckBox();
            this.btn2nd = new System.Windows.Forms.CheckBox();
            this.btn1st = new System.Windows.Forms.CheckBox();
            this.txtType = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtStat = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtTotDue = new System.Windows.Forms.TextBox();
            this.txtTotSurch = new System.Windows.Forms.TextBox();
            this.txtTotTotDue = new System.Windows.Forms.TextBox();
            this.txtTotPen = new System.Windows.Forms.TextBox();
            this.txtLGUCode = new System.Windows.Forms.TextBox();
            this.txtDistCode = new System.Windows.Forms.TextBox();
            this.txtBinYr = new System.Windows.Forms.TextBox();
            this.txtBinSeries = new System.Windows.Forms.TextBox();
            this.lblTaxCredit = new System.Windows.Forms.Label();
            this.txtGrandTotTotDue = new System.Windows.Forms.TextBox();
            this.txtGrandTotPen = new System.Windows.Forms.TextBox();
            this.txtGrandTotSurch = new System.Windows.Forms.TextBox();
            this.txtGrabdTotDue = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtCreditLeft = new System.Windows.Forms.TextBox();
            this.chkTaxCredit = new System.Windows.Forms.CheckBox();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSOA = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.chkPartial = new System.Windows.Forms.CheckBox();
            this.bin1 = new BIN.BIN();
            this.frameWithShadow2 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow4 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow1 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow3 = new Amellar.Common.SOA.FrameWithShadow();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaxFees)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.grpQtr.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 69;
            this.label1.Text = "BIN:";
            // 
            // txtBin
            // 
            this.txtBin.Location = new System.Drawing.Point(29, 279);
            this.txtBin.Name = "txtBin";
            this.txtBin.Size = new System.Drawing.Size(147, 20);
            this.txtBin.TabIndex = 1;
            this.txtBin.Visible = false;
            // 
            // txtLName
            // 
            this.txtLName.Location = new System.Drawing.Point(84, 124);
            this.txtLName.Name = "txtLName";
            this.txtLName.ReadOnly = true;
            this.txtLName.Size = new System.Drawing.Size(196, 20);
            this.txtLName.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Last Name:";
            // 
            // txtFName
            // 
            this.txtFName.Location = new System.Drawing.Point(351, 124);
            this.txtFName.Name = "txtFName";
            this.txtFName.ReadOnly = true;
            this.txtFName.Size = new System.Drawing.Size(187, 20);
            this.txtFName.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(285, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "First Name:";
            // 
            // txtMI
            // 
            this.txtMI.Location = new System.Drawing.Point(572, 124);
            this.txtMI.Name = "txtMI";
            this.txtMI.ReadOnly = true;
            this.txtMI.Size = new System.Drawing.Size(23, 20);
            this.txtMI.TabIndex = 11;
            this.txtMI.TextChanged += new System.EventHandler(this.txtMI_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(544, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "MI:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // txtBnsName
            // 
            this.txtBnsName.Location = new System.Drawing.Point(84, 48);
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(511, 20);
            this.txtBnsName.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 8;
            this.label5.Tag = "";
            this.label5.Text = "Bns Name:";
            // 
            // txtBnsAdd
            // 
            this.txtBnsAdd.Location = new System.Drawing.Point(84, 74);
            this.txtBnsAdd.Name = "txtBnsAdd";
            this.txtBnsAdd.ReadOnly = true;
            this.txtBnsAdd.Size = new System.Drawing.Size(511, 20);
            this.txtBnsAdd.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Bns Add:";
            // 
            // txtTaxYear
            // 
            this.txtTaxYear.Location = new System.Drawing.Point(544, 22);
            this.txtTaxYear.Name = "txtTaxYear";
            this.txtTaxYear.ReadOnly = true;
            this.txtTaxYear.Size = new System.Drawing.Size(51, 20);
            this.txtTaxYear.TabIndex = 13;
            this.txtTaxYear.TextChanged += new System.EventHandler(this.txtTaxYear_TextChanged);
            this.txtTaxYear.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtTaxYear_KeyUp);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(485, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Tax Year:";
            // 
            // dgvTaxFees
            // 
            this.dgvTaxFees.AllowUserToAddRows = false;
            this.dgvTaxFees.AllowUserToDeleteRows = false;
            this.dgvTaxFees.AllowUserToResizeColumns = false;
            this.dgvTaxFees.AllowUserToResizeRows = false;
            this.dgvTaxFees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTaxFees.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.year,
            this.term,
            this.qtr,
            this.particulars,
            this.due,
            this.surch,
            this.pen,
            this.total,
            this.due_state,
            this.qtr_to_pay,
            this.fees_code});
            this.dgvTaxFees.Location = new System.Drawing.Point(22, 187);
            this.dgvTaxFees.Name = "dgvTaxFees";
            this.dgvTaxFees.ReadOnly = true;
            this.dgvTaxFees.RowHeadersVisible = false;
            this.dgvTaxFees.Size = new System.Drawing.Size(764, 156);
            this.dgvTaxFees.TabIndex = 28;
            this.dgvTaxFees.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTaxFees_CellValueChanged);
            // 
            // year
            // 
            this.year.Frozen = true;
            this.year.HeaderText = "YEAR";
            this.year.Name = "year";
            this.year.ReadOnly = true;
            this.year.Width = 50;
            // 
            // term
            // 
            this.term.Frozen = true;
            this.term.HeaderText = "";
            this.term.Name = "term";
            this.term.ReadOnly = true;
            this.term.Width = 20;
            // 
            // qtr
            // 
            this.qtr.Frozen = true;
            this.qtr.HeaderText = "";
            this.qtr.Name = "qtr";
            this.qtr.ReadOnly = true;
            this.qtr.Width = 20;
            // 
            // particulars
            // 
            this.particulars.Frozen = true;
            this.particulars.HeaderText = "PARTICULARS";
            this.particulars.Name = "particulars";
            this.particulars.ReadOnly = true;
            this.particulars.Width = 250;
            // 
            // due
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.due.DefaultCellStyle = dataGridViewCellStyle5;
            this.due.Frozen = true;
            this.due.HeaderText = "DUE";
            this.due.Name = "due";
            this.due.ReadOnly = true;
            // 
            // surch
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.surch.DefaultCellStyle = dataGridViewCellStyle6;
            this.surch.Frozen = true;
            this.surch.HeaderText = "SURCHARGE";
            this.surch.Name = "surch";
            this.surch.ReadOnly = true;
            // 
            // pen
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.pen.DefaultCellStyle = dataGridViewCellStyle7;
            this.pen.Frozen = true;
            this.pen.HeaderText = "PENALTY";
            this.pen.Name = "pen";
            this.pen.ReadOnly = true;
            // 
            // total
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.total.DefaultCellStyle = dataGridViewCellStyle8;
            this.total.Frozen = true;
            this.total.HeaderText = "TOTAL DUE";
            this.total.Name = "total";
            this.total.ReadOnly = true;
            // 
            // due_state
            // 
            this.due_state.Frozen = true;
            this.due_state.HeaderText = "Due_State";
            this.due_state.Name = "due_state";
            this.due_state.ReadOnly = true;
            this.due_state.Visible = false;
            // 
            // qtr_to_pay
            // 
            this.qtr_to_pay.Frozen = true;
            this.qtr_to_pay.HeaderText = "Qtr_To_Pay";
            this.qtr_to_pay.Name = "qtr_to_pay";
            this.qtr_to_pay.ReadOnly = true;
            this.qtr_to_pay.Visible = false;
            // 
            // fees_code
            // 
            this.fees_code.Frozen = true;
            this.fees_code.HeaderText = "Fees_code";
            this.fees_code.Name = "fees_code";
            this.fees_code.ReadOnly = true;
            this.fees_code.Visible = false;
            // 
            // chkQtr
            // 
            this.chkQtr.AutoSize = true;
            this.chkQtr.Location = new System.Drawing.Point(93, 19);
            this.chkQtr.Name = "chkQtr";
            this.chkQtr.Size = new System.Drawing.Size(68, 17);
            this.chkQtr.TabIndex = 13;
            this.chkQtr.Text = "Quarterly";
            this.chkQtr.UseVisualStyleBackColor = true;
            this.chkQtr.Click += new System.EventHandler(this.chkQtr_Click);
            this.chkQtr.CheckedChanged += new System.EventHandler(this.chkQtr_CheckedChanged);
            // 
            // chkFull
            // 
            this.chkFull.AutoSize = true;
            this.chkFull.Location = new System.Drawing.Point(17, 19);
            this.chkFull.Name = "chkFull";
            this.chkFull.Size = new System.Drawing.Size(42, 17);
            this.chkFull.TabIndex = 12;
            this.chkFull.Text = "Full";
            this.chkFull.UseVisualStyleBackColor = true;
            this.chkFull.Click += new System.EventHandler(this.chkFull_Click);
            this.chkFull.CheckedChanged += new System.EventHandler(this.chkFull_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkFull);
            this.groupBox1.Controls.Add(this.chkQtr);
            this.groupBox1.Location = new System.Drawing.Point(633, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(167, 46);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Payment Term";
            // 
            // grpQtr
            // 
            this.grpQtr.Controls.Add(this.btn4th);
            this.grpQtr.Controls.Add(this.btn3rd);
            this.grpQtr.Controls.Add(this.btn2nd);
            this.grpQtr.Controls.Add(this.btn1st);
            this.grpQtr.Location = new System.Drawing.Point(633, 53);
            this.grpQtr.Name = "grpQtr";
            this.grpQtr.Size = new System.Drawing.Size(167, 75);
            this.grpQtr.TabIndex = 33;
            this.grpQtr.TabStop = false;
            this.grpQtr.Text = "Quarter";
            // 
            // btn4th
            // 
            this.btn4th.AutoSize = true;
            this.btn4th.Location = new System.Drawing.Point(93, 50);
            this.btn4th.Name = "btn4th";
            this.btn4th.Size = new System.Drawing.Size(41, 17);
            this.btn4th.TabIndex = 73;
            this.btn4th.Text = "4th";
            this.btn4th.UseVisualStyleBackColor = true;
            this.btn4th.Click += new System.EventHandler(this.btn4th_Click);
            this.btn4th.CheckedChanged += new System.EventHandler(this.btn4th_CheckedChanged);
            // 
            // btn3rd
            // 
            this.btn3rd.AutoSize = true;
            this.btn3rd.Location = new System.Drawing.Point(17, 50);
            this.btn3rd.Name = "btn3rd";
            this.btn3rd.Size = new System.Drawing.Size(41, 17);
            this.btn3rd.TabIndex = 72;
            this.btn3rd.Text = "3rd";
            this.btn3rd.UseVisualStyleBackColor = true;
            this.btn3rd.Click += new System.EventHandler(this.btn3rd_Click);
            this.btn3rd.CheckedChanged += new System.EventHandler(this.btn3rd_CheckedChanged);
            // 
            // btn2nd
            // 
            this.btn2nd.AutoSize = true;
            this.btn2nd.Location = new System.Drawing.Point(93, 23);
            this.btn2nd.Name = "btn2nd";
            this.btn2nd.Size = new System.Drawing.Size(44, 17);
            this.btn2nd.TabIndex = 71;
            this.btn2nd.Text = "2nd";
            this.btn2nd.UseVisualStyleBackColor = true;
            this.btn2nd.Click += new System.EventHandler(this.btn2nd_Click);
            this.btn2nd.CheckedChanged += new System.EventHandler(this.btn2nd_CheckedChanged);
            // 
            // btn1st
            // 
            this.btn1st.AutoSize = true;
            this.btn1st.Location = new System.Drawing.Point(17, 23);
            this.btn1st.Name = "btn1st";
            this.btn1st.Size = new System.Drawing.Size(40, 17);
            this.btn1st.TabIndex = 70;
            this.btn1st.Text = "1st";
            this.btn1st.UseVisualStyleBackColor = true;
            this.btn1st.Click += new System.EventHandler(this.btn1st_Click);
            this.btn1st.CheckedChanged += new System.EventHandler(this.btn1st_CheckedChanged);
            // 
            // txtType
            // 
            this.txtType.Location = new System.Drawing.Point(84, 100);
            this.txtType.Name = "txtType";
            this.txtType.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(307, 20);
            this.txtType.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(44, 103);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "Type:";
            // 
            // txtStat
            // 
            this.txtStat.Location = new System.Drawing.Point(443, 100);
            this.txtStat.Name = "txtStat";
            this.txtStat.ReadOnly = true;
            this.txtStat.Size = new System.Drawing.Size(49, 20);
            this.txtStat.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(397, 103);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "Status:";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(537, 100);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(58, 20);
            this.txtCode.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(499, 103);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 38;
            this.label10.Text = "Code:";
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label11.Location = new System.Drawing.Point(23, 346);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(336, 18);
            this.label11.TabIndex = 40;
            this.label11.Text = "SUB-TOTAL:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTotDue
            // 
            this.txtTotDue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotDue.Location = new System.Drawing.Point(362, 345);
            this.txtTotDue.Name = "txtTotDue";
            this.txtTotDue.ReadOnly = true;
            this.txtTotDue.Size = new System.Drawing.Size(101, 20);
            this.txtTotDue.TabIndex = 41;
            this.txtTotDue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtTotSurch
            // 
            this.txtTotSurch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotSurch.Location = new System.Drawing.Point(463, 345);
            this.txtTotSurch.Name = "txtTotSurch";
            this.txtTotSurch.ReadOnly = true;
            this.txtTotSurch.Size = new System.Drawing.Size(101, 20);
            this.txtTotSurch.TabIndex = 42;
            this.txtTotSurch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtTotTotDue
            // 
            this.txtTotTotDue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotTotDue.Location = new System.Drawing.Point(664, 345);
            this.txtTotTotDue.Name = "txtTotTotDue";
            this.txtTotTotDue.ReadOnly = true;
            this.txtTotTotDue.Size = new System.Drawing.Size(101, 20);
            this.txtTotTotDue.TabIndex = 44;
            this.txtTotTotDue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtTotPen
            // 
            this.txtTotPen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotPen.Location = new System.Drawing.Point(564, 345);
            this.txtTotPen.Name = "txtTotPen";
            this.txtTotPen.ReadOnly = true;
            this.txtTotPen.Size = new System.Drawing.Size(100, 20);
            this.txtTotPen.TabIndex = 43;
            this.txtTotPen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtLGUCode
            // 
            this.txtLGUCode.Location = new System.Drawing.Point(84, 21);
            this.txtLGUCode.Name = "txtLGUCode";
            this.txtLGUCode.ReadOnly = true;
            this.txtLGUCode.Size = new System.Drawing.Size(26, 20);
            this.txtLGUCode.TabIndex = 47;
            this.txtLGUCode.Text = "129";
            // 
            // txtDistCode
            // 
            this.txtDistCode.Location = new System.Drawing.Point(111, 21);
            this.txtDistCode.Name = "txtDistCode";
            this.txtDistCode.ReadOnly = true;
            this.txtDistCode.Size = new System.Drawing.Size(19, 20);
            this.txtDistCode.TabIndex = 48;
            this.txtDistCode.Text = "00";
            // 
            // txtBinYr
            // 
            this.txtBinYr.Location = new System.Drawing.Point(132, 21);
            this.txtBinYr.MaxLength = 4;
            this.txtBinYr.Name = "txtBinYr";
            this.txtBinYr.Size = new System.Drawing.Size(32, 20);
            this.txtBinYr.TabIndex = 0;
            this.txtBinYr.TextChanged += new System.EventHandler(this.txtBinYr_TextChanged);
            // 
            // txtBinSeries
            // 
            this.txtBinSeries.Location = new System.Drawing.Point(165, 21);
            this.txtBinSeries.MaxLength = 7;
            this.txtBinSeries.Name = "txtBinSeries";
            this.txtBinSeries.Size = new System.Drawing.Size(50, 20);
            this.txtBinSeries.TabIndex = 2;
            this.txtBinSeries.Layout += new System.Windows.Forms.LayoutEventHandler(this.txtBinSeries_Layout);
            this.txtBinSeries.Leave += new System.EventHandler(this.txtBinSeries_Leave);
            // 
            // lblTaxCredit
            // 
            this.lblTaxCredit.AutoSize = true;
            this.lblTaxCredit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxCredit.ForeColor = System.Drawing.Color.Teal;
            this.lblTaxCredit.Location = new System.Drawing.Point(19, 435);
            this.lblTaxCredit.Name = "lblTaxCredit";
            this.lblTaxCredit.Size = new System.Drawing.Size(307, 20);
            this.lblTaxCredit.TabIndex = 57;
            this.lblTaxCredit.Text = "with available tax credit of P 1,000.00";
            this.lblTaxCredit.Visible = false;
            // 
            // txtGrandTotTotDue
            // 
            this.txtGrandTotTotDue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGrandTotTotDue.Location = new System.Drawing.Point(664, 369);
            this.txtGrandTotTotDue.Name = "txtGrandTotTotDue";
            this.txtGrandTotTotDue.ReadOnly = true;
            this.txtGrandTotTotDue.Size = new System.Drawing.Size(101, 20);
            this.txtGrandTotTotDue.TabIndex = 62;
            this.txtGrandTotTotDue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtGrandTotPen
            // 
            this.txtGrandTotPen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGrandTotPen.Location = new System.Drawing.Point(564, 369);
            this.txtGrandTotPen.Name = "txtGrandTotPen";
            this.txtGrandTotPen.ReadOnly = true;
            this.txtGrandTotPen.Size = new System.Drawing.Size(100, 20);
            this.txtGrandTotPen.TabIndex = 61;
            this.txtGrandTotPen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtGrandTotSurch
            // 
            this.txtGrandTotSurch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGrandTotSurch.Location = new System.Drawing.Point(463, 369);
            this.txtGrandTotSurch.Name = "txtGrandTotSurch";
            this.txtGrandTotSurch.ReadOnly = true;
            this.txtGrandTotSurch.Size = new System.Drawing.Size(101, 20);
            this.txtGrandTotSurch.TabIndex = 60;
            this.txtGrandTotSurch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtGrabdTotDue
            // 
            this.txtGrabdTotDue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGrabdTotDue.Location = new System.Drawing.Point(362, 369);
            this.txtGrabdTotDue.Name = "txtGrabdTotDue";
            this.txtGrabdTotDue.ReadOnly = true;
            this.txtGrabdTotDue.Size = new System.Drawing.Size(101, 20);
            this.txtGrabdTotDue.TabIndex = 59;
            this.txtGrabdTotDue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label13.Location = new System.Drawing.Point(257, 370);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(102, 20);
            this.label13.TabIndex = 58;
            this.label13.Text = "GRAND TOTAL:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCreditLeft
            // 
            this.txtCreditLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCreditLeft.Location = new System.Drawing.Point(120, 370);
            this.txtCreditLeft.Name = "txtCreditLeft";
            this.txtCreditLeft.ReadOnly = true;
            this.txtCreditLeft.Size = new System.Drawing.Size(135, 20);
            this.txtCreditLeft.TabIndex = 64;
            this.txtCreditLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chkTaxCredit
            // 
            this.chkTaxCredit.Location = new System.Drawing.Point(24, 371);
            this.chkTaxCredit.Name = "chkTaxCredit";
            this.chkTaxCredit.Size = new System.Drawing.Size(96, 19);
            this.chkTaxCredit.TabIndex = 18;
            this.chkTaxCredit.Text = "TAX CREDIT:";
            this.chkTaxCredit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkTaxCredit.UseVisualStyleBackColor = true;
            this.chkTaxCredit.CheckedChanged += new System.EventHandler(this.chkTaxCredit_CheckedChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(228, 20);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(86, 24);
            this.btnSearch.TabIndex = 70;
            this.btnSearch.Text = "Search";
            this.btnSearch.Values.ExtraText = "";
            this.btnSearch.Values.Image = null;
            this.btnSearch.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearch.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearch.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnSOA
            // 
            this.btnSOA.Location = new System.Drawing.Point(608, 432);
            this.btnSOA.Name = "btnSOA";
            this.btnSOA.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSOA.Size = new System.Drawing.Size(86, 24);
            this.btnSOA.TabIndex = 71;
            this.btnSOA.Text = "Preview SOA";
            this.btnSOA.Values.ExtraText = "";
            this.btnSOA.Values.Image = null;
            this.btnSOA.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSOA.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSOA.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSOA.Values.Text = "Preview SOA";
            this.btnSOA.Click += new System.EventHandler(this.btnSOA_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(700, 432);
            this.btnOk.Name = "btnOk";
            this.btnOk.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOk.Size = new System.Drawing.Size(86, 24);
            this.btnOk.TabIndex = 72;
            this.btnOk.Text = "OK";
            this.btnOk.Values.ExtraText = "";
            this.btnOk.Values.Image = null;
            this.btnOk.Values.ImageStates.ImageCheckedNormal = null;
            this.btnOk.Values.ImageStates.ImageCheckedPressed = null;
            this.btnOk.Values.ImageStates.ImageCheckedTracking = null;
            this.btnOk.Values.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // chkPartial
            // 
            this.chkPartial.AutoSize = true;
            this.chkPartial.Location = new System.Drawing.Point(650, 134);
            this.chkPartial.Name = "chkPartial";
            this.chkPartial.Size = new System.Drawing.Size(99, 17);
            this.chkPartial.TabIndex = 73;
            this.chkPartial.Text = "Partial Payment";
            this.chkPartial.UseVisualStyleBackColor = true;
            this.chkPartial.Visible = false;
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(84, 24);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(138, 23);
            this.bin1.TabIndex = 74;
            // 
            // frameWithShadow2
            // 
            this.frameWithShadow2.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow2.Location = new System.Drawing.Point(3, 172);
            this.frameWithShadow2.Name = "frameWithShadow2";
            this.frameWithShadow2.Size = new System.Drawing.Size(810, 240);
            this.frameWithShadow2.TabIndex = 66;
            // 
            // frameWithShadow4
            // 
            this.frameWithShadow4.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow4.Location = new System.Drawing.Point(3, 417);
            this.frameWithShadow4.Name = "frameWithShadow4";
            this.frameWithShadow4.Size = new System.Drawing.Size(810, 62);
            this.frameWithShadow4.TabIndex = 68;
            // 
            // frameWithShadow1
            // 
            this.frameWithShadow1.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow1.Location = new System.Drawing.Point(3, 4);
            this.frameWithShadow1.Name = "frameWithShadow1";
            this.frameWithShadow1.Size = new System.Drawing.Size(609, 162);
            this.frameWithShadow1.TabIndex = 65;
            // 
            // frameWithShadow3
            // 
            this.frameWithShadow3.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow3.Location = new System.Drawing.Point(618, 4);
            this.frameWithShadow3.Name = "frameWithShadow3";
            this.frameWithShadow3.Size = new System.Drawing.Size(193, 162);
            this.frameWithShadow3.TabIndex = 67;
            // 
            // frmSOA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(814, 482);
            this.ControlBox = false;
            this.Controls.Add(this.bin1);
            this.Controls.Add(this.chkPartial);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnSOA);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtTaxYear);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.chkTaxCredit);
            this.Controls.Add(this.txtCreditLeft);
            this.Controls.Add(this.txtGrandTotTotDue);
            this.Controls.Add(this.txtGrandTotPen);
            this.Controls.Add(this.txtGrandTotSurch);
            this.Controls.Add(this.txtGrabdTotDue);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.lblTaxCredit);
            this.Controls.Add(this.txtTotTotDue);
            this.Controls.Add(this.txtTotPen);
            this.Controls.Add(this.txtTotSurch);
            this.Controls.Add(this.txtTotDue);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtStat);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtType);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.dgvTaxFees);
            this.Controls.Add(this.txtBnsAdd);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtBnsName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtMI);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtFName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.frameWithShadow2);
            this.Controls.Add(this.frameWithShadow4);
            this.Controls.Add(this.frameWithShadow1);
            this.Controls.Add(this.grpQtr);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtBinSeries);
            this.Controls.Add(this.txtBinYr);
            this.Controls.Add(this.txtDistCode);
            this.Controls.Add(this.txtLGUCode);
            this.Controls.Add(this.frameWithShadow3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(820, 510);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(820, 510);
            this.Name = "frmSOA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.Load += new System.EventHandler(this.frmSOA_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaxFees)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpQtr.ResumeLayout(false);
            this.grpQtr.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBin;
        private System.Windows.Forms.TextBox txtLName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMI;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBnsName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBnsAdd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dgvTaxFees;
        private System.Windows.Forms.CheckBox chkQtr;
        private System.Windows.Forms.CheckBox chkFull;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox grpQtr;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtTotDue;
        private System.Windows.Forms.TextBox txtTotSurch;
        private System.Windows.Forms.TextBox txtTotTotDue;
        private System.Windows.Forms.TextBox txtTotPen;
        private System.Windows.Forms.DataGridViewTextBoxColumn year;
        private System.Windows.Forms.DataGridViewTextBoxColumn term;
        private System.Windows.Forms.DataGridViewTextBoxColumn qtr;
        private System.Windows.Forms.DataGridViewTextBoxColumn particulars;
        private System.Windows.Forms.DataGridViewTextBoxColumn due;
        private System.Windows.Forms.DataGridViewTextBoxColumn surch;
        private System.Windows.Forms.DataGridViewTextBoxColumn pen;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn due_state;
        private System.Windows.Forms.DataGridViewTextBoxColumn qtr_to_pay;
        private System.Windows.Forms.DataGridViewTextBoxColumn fees_code;
        public System.Windows.Forms.TextBox txtStat;
        public System.Windows.Forms.TextBox txtTaxYear;
        private System.Windows.Forms.TextBox txtLGUCode;
        private System.Windows.Forms.TextBox txtDistCode;
        private System.Windows.Forms.TextBox txtBinYr;
        private System.Windows.Forms.TextBox txtBinSeries;
        private System.Windows.Forms.Label lblTaxCredit;
        private System.Windows.Forms.TextBox txtGrandTotTotDue;
        private System.Windows.Forms.TextBox txtGrandTotPen;
        private System.Windows.Forms.TextBox txtGrandTotSurch;
        private System.Windows.Forms.TextBox txtGrabdTotDue;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtCreditLeft;
        private System.Windows.Forms.CheckBox chkTaxCredit;
        private FrameWithShadow frameWithShadow1;
        private FrameWithShadow frameWithShadow2;
        private FrameWithShadow frameWithShadow3;
        private FrameWithShadow frameWithShadow4;
        //private BIN.BIN bin1;  // GDE 20110729
        private System.Windows.Forms.CheckBox btn2nd;
        private System.Windows.Forms.CheckBox btn1st;
        private System.Windows.Forms.CheckBox btn4th;
        private System.Windows.Forms.CheckBox btn3rd;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSOA;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        private System.Windows.Forms.CheckBox chkPartial;
        public BIN.BIN bin1;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
    }
}

