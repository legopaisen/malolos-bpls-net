namespace Amellar.Modules.Payment
{
    partial class frmCancelPayment
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
            this.frameWithShadow1 = new Amellar.Common.SOA.FrameWithShadow();
            this.label1 = new System.Windows.Forms.Label();
            this.txtORNo = new System.Windows.Forms.TextBox();
            this.btnProceed = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label2 = new System.Windows.Forms.Label();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.kryptonHeader3 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.frameWithShadow2 = new Amellar.Common.SOA.FrameWithShadow();
            this.kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.frameWithShadow3 = new Amellar.Common.SOA.FrameWithShadow();
            this.txtOwnName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOwnAdd = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBnsAdd = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.kryptonHeader4 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.frameWithShadow4 = new Amellar.Common.SOA.FrameWithShadow();
            this.btnInsufficientFund = new System.Windows.Forms.RadioButton();
            this.btnCancelledCheck = new System.Windows.Forms.RadioButton();
            this.btnWrongAcnt = new System.Windows.Forms.RadioButton();
            this.btnOther = new System.Windows.Forms.RadioButton();
            this.txtOthers = new System.Windows.Forms.TextBox();
            this.kryptonHeader5 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.frameWithShadow5 = new Amellar.Common.SOA.FrameWithShadow();
            this.txtBTax = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtFees = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSurch = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPen = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.btnCancelPayment = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtTaxCredit = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // frameWithShadow1
            // 
            this.frameWithShadow1.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow1.Location = new System.Drawing.Point(15, 10);
            this.frameWithShadow1.Name = "frameWithShadow1";
            this.frameWithShadow1.Size = new System.Drawing.Size(576, 83);
            this.frameWithShadow1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "OR Number:";
            // 
            // txtORNo
            // 
            this.txtORNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtORNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtORNo.ForeColor = System.Drawing.Color.MediumBlue;
            this.txtORNo.Location = new System.Drawing.Point(103, 47);
            this.txtORNo.Name = "txtORNo";
            this.txtORNo.Size = new System.Drawing.Size(131, 26);
            this.txtORNo.TabIndex = 2;
            // 
            // btnProceed
            // 
            this.btnProceed.Location = new System.Drawing.Point(240, 47);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnProceed.Size = new System.Drawing.Size(90, 25);
            this.btnProceed.TabIndex = 3;
            this.btnProceed.Text = "Proceed";
            this.btnProceed.Values.ExtraText = "";
            this.btnProceed.Values.Image = null;
            this.btnProceed.Values.ImageStates.ImageCheckedNormal = null;
            this.btnProceed.Values.ImageStates.ImageCheckedPressed = null;
            this.btnProceed.Values.ImageStates.ImageCheckedTracking = null;
            this.btnProceed.Values.Text = "Proceed";
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(445, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Date";
            // 
            // dtDate
            // 
            this.dtDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDate.Location = new System.Drawing.Point(484, 51);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(92, 20);
            this.dtDate.TabIndex = 5;
            this.dtDate.ValueChanged += new System.EventHandler(this.dtDate_ValueChanged);
            // 
            // kryptonHeader3
            // 
            this.kryptonHeader3.AutoSize = false;
            this.kryptonHeader3.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader3.Location = new System.Drawing.Point(24, 14);
            this.kryptonHeader3.Name = "kryptonHeader3";
            this.kryptonHeader3.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonHeader3.Size = new System.Drawing.Size(560, 22);
            this.kryptonHeader3.TabIndex = 68;
            this.kryptonHeader3.Text = "Basic Official Receipt Information";
            this.kryptonHeader3.Values.Description = "";
            this.kryptonHeader3.Values.Heading = "Basic Official Receipt Information";
            this.kryptonHeader3.Values.Image = null;
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader1.Location = new System.Drawing.Point(24, 101);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonHeader1.Size = new System.Drawing.Size(563, 22);
            this.kryptonHeader1.TabIndex = 70;
            this.kryptonHeader1.Text = "Business Owner Information";
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Business Owner Information";
            this.kryptonHeader1.Values.Image = null;
            // 
            // frameWithShadow2
            // 
            this.frameWithShadow2.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow2.Location = new System.Drawing.Point(15, 97);
            this.frameWithShadow2.Name = "frameWithShadow2";
            this.frameWithShadow2.Size = new System.Drawing.Size(576, 93);
            this.frameWithShadow2.TabIndex = 69;
            // 
            // kryptonHeader2
            // 
            this.kryptonHeader2.AutoSize = false;
            this.kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader2.Location = new System.Drawing.Point(24, 200);
            this.kryptonHeader2.Name = "kryptonHeader2";
            this.kryptonHeader2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonHeader2.Size = new System.Drawing.Size(563, 22);
            this.kryptonHeader2.TabIndex = 72;
            this.kryptonHeader2.Text = "Business Information";
            this.kryptonHeader2.Values.Description = "";
            this.kryptonHeader2.Values.Heading = "Business Information";
            this.kryptonHeader2.Values.Image = null;
            // 
            // frameWithShadow3
            // 
            this.frameWithShadow3.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow3.Location = new System.Drawing.Point(15, 196);
            this.frameWithShadow3.Name = "frameWithShadow3";
            this.frameWithShadow3.Size = new System.Drawing.Size(576, 95);
            this.frameWithShadow3.TabIndex = 71;
            // 
            // txtOwnName
            // 
            this.txtOwnName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOwnName.Location = new System.Drawing.Point(103, 129);
            this.txtOwnName.Name = "txtOwnName";
            this.txtOwnName.ReadOnly = true;
            this.txtOwnName.Size = new System.Drawing.Size(476, 22);
            this.txtOwnName.TabIndex = 74;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 73;
            this.label3.Text = "Name:";
            // 
            // txtOwnAdd
            // 
            this.txtOwnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOwnAdd.Location = new System.Drawing.Point(103, 154);
            this.txtOwnAdd.Name = "txtOwnAdd";
            this.txtOwnAdd.ReadOnly = true;
            this.txtOwnAdd.Size = new System.Drawing.Size(476, 22);
            this.txtOwnAdd.TabIndex = 76;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 75;
            this.label4.Text = "Address:";
            // 
            // txtBnsAdd
            // 
            this.txtBnsAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBnsAdd.Location = new System.Drawing.Point(103, 253);
            this.txtBnsAdd.Name = "txtBnsAdd";
            this.txtBnsAdd.ReadOnly = true;
            this.txtBnsAdd.Size = new System.Drawing.Size(476, 22);
            this.txtBnsAdd.TabIndex = 80;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 258);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 79;
            this.label5.Text = "Address:";
            // 
            // txtBnsName
            // 
            this.txtBnsName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBnsName.Location = new System.Drawing.Point(103, 228);
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(476, 22);
            this.txtBnsName.TabIndex = 78;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 233);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 77;
            this.label6.Text = "Business:";
            // 
            // kryptonHeader4
            // 
            this.kryptonHeader4.AutoSize = false;
            this.kryptonHeader4.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader4.Location = new System.Drawing.Point(21, 297);
            this.kryptonHeader4.Name = "kryptonHeader4";
            this.kryptonHeader4.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonHeader4.Size = new System.Drawing.Size(264, 22);
            this.kryptonHeader4.TabIndex = 82;
            this.kryptonHeader4.Text = "Reason of Cancellation";
            this.kryptonHeader4.Values.Description = "";
            this.kryptonHeader4.Values.Heading = "Reason of Cancellation";
            this.kryptonHeader4.Values.Image = null;
            // 
            // frameWithShadow4
            // 
            this.frameWithShadow4.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow4.Location = new System.Drawing.Point(12, 293);
            this.frameWithShadow4.Name = "frameWithShadow4";
            this.frameWithShadow4.Size = new System.Drawing.Size(277, 209);
            this.frameWithShadow4.TabIndex = 81;
            // 
            // btnInsufficientFund
            // 
            this.btnInsufficientFund.AutoSize = true;
            this.btnInsufficientFund.Location = new System.Drawing.Point(45, 345);
            this.btnInsufficientFund.Name = "btnInsufficientFund";
            this.btnInsufficientFund.Size = new System.Drawing.Size(103, 17);
            this.btnInsufficientFund.TabIndex = 83;
            this.btnInsufficientFund.TabStop = true;
            this.btnInsufficientFund.Text = "Insufficient Fund";
            this.btnInsufficientFund.UseVisualStyleBackColor = true;
            this.btnInsufficientFund.Click += new System.EventHandler(this.btnInsufficientFund_Click);
            // 
            // btnCancelledCheck
            // 
            this.btnCancelledCheck.AutoSize = true;
            this.btnCancelledCheck.Location = new System.Drawing.Point(45, 372);
            this.btnCancelledCheck.Name = "btnCancelledCheck";
            this.btnCancelledCheck.Size = new System.Drawing.Size(106, 17);
            this.btnCancelledCheck.TabIndex = 84;
            this.btnCancelledCheck.TabStop = true;
            this.btnCancelledCheck.Text = "Cancelled Check";
            this.btnCancelledCheck.UseVisualStyleBackColor = true;
            this.btnCancelledCheck.Click += new System.EventHandler(this.btnCancelledCheck_Click);
            // 
            // btnWrongAcnt
            // 
            this.btnWrongAcnt.AutoSize = true;
            this.btnWrongAcnt.Location = new System.Drawing.Point(45, 399);
            this.btnWrongAcnt.Name = "btnWrongAcnt";
            this.btnWrongAcnt.Size = new System.Drawing.Size(179, 17);
            this.btnWrongAcnt.TabIndex = 85;
            this.btnWrongAcnt.TabStop = true;
            this.btnWrongAcnt.Text = "Wrong Entry of Account Number";
            this.btnWrongAcnt.UseVisualStyleBackColor = true;
            this.btnWrongAcnt.Click += new System.EventHandler(this.btnWrongAcnt_Click);
            // 
            // btnOther
            // 
            this.btnOther.AutoSize = true;
            this.btnOther.Location = new System.Drawing.Point(45, 426);
            this.btnOther.Name = "btnOther";
            this.btnOther.Size = new System.Drawing.Size(100, 17);
            this.btnOther.TabIndex = 86;
            this.btnOther.TabStop = true;
            this.btnOther.Text = "Others, Specify:";
            this.btnOther.UseVisualStyleBackColor = true;
            this.btnOther.Click += new System.EventHandler(this.btnOther_Click);
            // 
            // txtOthers
            // 
            this.txtOthers.Location = new System.Drawing.Point(46, 449);
            this.txtOthers.Name = "txtOthers";
            this.txtOthers.ReadOnly = true;
            this.txtOthers.Size = new System.Drawing.Size(231, 20);
            this.txtOthers.TabIndex = 87;
            // 
            // kryptonHeader5
            // 
            this.kryptonHeader5.AutoSize = false;
            this.kryptonHeader5.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader5.Location = new System.Drawing.Point(303, 297);
            this.kryptonHeader5.Name = "kryptonHeader5";
            this.kryptonHeader5.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonHeader5.Size = new System.Drawing.Size(285, 22);
            this.kryptonHeader5.TabIndex = 89;
            this.kryptonHeader5.Text = "Amount to be Cancelled";
            this.kryptonHeader5.Values.Description = "";
            this.kryptonHeader5.Values.Heading = "Amount to be Cancelled";
            this.kryptonHeader5.Values.Image = null;
            // 
            // frameWithShadow5
            // 
            this.frameWithShadow5.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow5.Location = new System.Drawing.Point(294, 293);
            this.frameWithShadow5.Name = "frameWithShadow5";
            this.frameWithShadow5.Size = new System.Drawing.Size(298, 209);
            this.frameWithShadow5.TabIndex = 88;
            // 
            // txtBTax
            // 
            this.txtBTax.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBTax.Location = new System.Drawing.Point(411, 328);
            this.txtBTax.Name = "txtBTax";
            this.txtBTax.ReadOnly = true;
            this.txtBTax.Size = new System.Drawing.Size(165, 22);
            this.txtBTax.TabIndex = 91;
            this.txtBTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(310, 333);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 90;
            this.label7.Text = "Business Tax:";
            // 
            // txtFees
            // 
            this.txtFees.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFees.Location = new System.Drawing.Point(411, 353);
            this.txtFees.Name = "txtFees";
            this.txtFees.ReadOnly = true;
            this.txtFees.Size = new System.Drawing.Size(165, 22);
            this.txtFees.TabIndex = 93;
            this.txtFees.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(310, 357);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 13);
            this.label8.TabIndex = 92;
            this.label8.Text = "Regulatory Fees:";
            // 
            // txtSurch
            // 
            this.txtSurch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSurch.Location = new System.Drawing.Point(411, 378);
            this.txtSurch.Name = "txtSurch";
            this.txtSurch.ReadOnly = true;
            this.txtSurch.Size = new System.Drawing.Size(165, 22);
            this.txtSurch.TabIndex = 95;
            this.txtSurch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(310, 381);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 13);
            this.label9.TabIndex = 94;
            this.label9.Text = "Surcharges:";
            // 
            // txtPen
            // 
            this.txtPen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPen.Location = new System.Drawing.Point(411, 403);
            this.txtPen.Name = "txtPen";
            this.txtPen.ReadOnly = true;
            this.txtPen.Size = new System.Drawing.Size(165, 22);
            this.txtPen.TabIndex = 97;
            this.txtPen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(310, 405);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(45, 13);
            this.label10.TabIndex = 96;
            this.label10.Text = "Penalty:";
            // 
            // txtTotal
            // 
            this.txtTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotal.Location = new System.Drawing.Point(412, 459);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.ReadOnly = true;
            this.txtTotal.Size = new System.Drawing.Size(165, 29);
            this.txtTotal.TabIndex = 99;
            this.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(311, 468);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 13);
            this.label11.TabIndex = 98;
            this.label11.Text = "TOTAL";
            // 
            // kryptonBorderEdge1
            // 
            this.kryptonBorderEdge1.Location = new System.Drawing.Point(314, 453);
            this.kryptonBorderEdge1.Name = "kryptonBorderEdge1";
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(265, 1);
            this.kryptonBorderEdge1.TabIndex = 100;
            this.kryptonBorderEdge1.Text = "kryptonBorderEdge1";
            // 
            // btnCancelPayment
            // 
            this.btnCancelPayment.Location = new System.Drawing.Point(376, 508);
            this.btnCancelPayment.Name = "btnCancelPayment";
            this.btnCancelPayment.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancelPayment.Size = new System.Drawing.Size(112, 25);
            this.btnCancelPayment.TabIndex = 101;
            this.btnCancelPayment.Text = "Cancel Payment";
            this.btnCancelPayment.Values.ExtraText = "";
            this.btnCancelPayment.Values.Image = null;
            this.btnCancelPayment.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancelPayment.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancelPayment.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancelPayment.Values.Text = "Cancel Payment";
            this.btnCancelPayment.Click += new System.EventHandler(this.btnCancelPayment_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(494, 508);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(90, 25);
            this.btnClose.TabIndex = 102;
            this.btnClose.Text = "Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtTaxCredit
            // 
            this.txtTaxCredit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTaxCredit.Location = new System.Drawing.Point(411, 428);
            this.txtTaxCredit.Name = "txtTaxCredit";
            this.txtTaxCredit.ReadOnly = true;
            this.txtTaxCredit.Size = new System.Drawing.Size(165, 22);
            this.txtTaxCredit.TabIndex = 104;
            this.txtTaxCredit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(310, 430);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(58, 13);
            this.label12.TabIndex = 103;
            this.label12.Text = "Tax Credit:";
            // 
            // frmCancelPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(603, 545);
            this.Controls.Add(this.txtTaxCredit);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCancelPayment);
            this.Controls.Add(this.kryptonBorderEdge1);
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtPen);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtSurch);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtFees);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtBTax);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.kryptonHeader5);
            this.Controls.Add(this.frameWithShadow5);
            this.Controls.Add(this.txtOthers);
            this.Controls.Add(this.btnOther);
            this.Controls.Add(this.btnWrongAcnt);
            this.Controls.Add(this.btnCancelledCheck);
            this.Controls.Add(this.btnInsufficientFund);
            this.Controls.Add(this.kryptonHeader4);
            this.Controls.Add(this.frameWithShadow4);
            this.Controls.Add(this.txtBnsAdd);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBnsName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtOwnAdd);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtOwnName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.kryptonHeader2);
            this.Controls.Add(this.frameWithShadow3);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.frameWithShadow2);
            this.Controls.Add(this.kryptonHeader3);
            this.Controls.Add(this.dtDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnProceed);
            this.Controls.Add(this.txtORNo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.frameWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCancelPayment";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cancel Payment";
            this.Load += new System.EventHandler(this.frmCancelPayment_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.SOA.FrameWithShadow frameWithShadow1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtORNo;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnProceed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtDate;
        public ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader3;
        public ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow2;
        public ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader2;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow3;
        private System.Windows.Forms.TextBox txtOwnName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtOwnAdd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBnsAdd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBnsName;
        private System.Windows.Forms.Label label6;
        public ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader4;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow4;
        private System.Windows.Forms.RadioButton btnInsufficientFund;
        private System.Windows.Forms.RadioButton btnCancelledCheck;
        private System.Windows.Forms.RadioButton btnWrongAcnt;
        private System.Windows.Forms.RadioButton btnOther;
        private System.Windows.Forms.TextBox txtOthers;
        public ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader5;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow5;
        private System.Windows.Forms.TextBox txtBTax;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtFees;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSurch;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPen;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.Label label11;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancelPayment;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private System.Windows.Forms.TextBox txtTaxCredit;
        private System.Windows.Forms.Label label12;
    }
}