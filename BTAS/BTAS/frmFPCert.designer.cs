namespace BTAS
{
    partial class frmFPCert
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBNSName = new System.Windows.Forms.TextBox();
            this.txtBNSAdd = new System.Windows.Forms.TextBox();
            this.txtBNSOwner = new System.Windows.Forms.TextBox();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPrint = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtLGUCode = new System.Windows.Forms.TextBox();
            this.txtDISTCode = new System.Windows.Forms.TextBox();
            this.txtBINYr = new System.Windows.Forms.TextBox();
            this.txtBINSerial = new System.Windows.Forms.TextBox();
            this.lblORNo = new System.Windows.Forms.Label();
            this.txtORNo = new System.Windows.Forms.TextBox();
            this.lblORDate = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.Addlpnl = new System.Windows.Forms.Panel();
            this.chkIssued = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpIssued = new System.Windows.Forms.DateTimePicker();
            this.dtpORDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.cboTaxYear = new System.Windows.Forms.ComboBox();
            this.rdoYear = new System.Windows.Forms.RadioButton();
            this.rdoQtr = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.Addlpnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "BIN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "BNS Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "BNS Add";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Owner";
            // 
            // txtBNSName
            // 
            this.txtBNSName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBNSName.Location = new System.Drawing.Point(71, 57);
            this.txtBNSName.Name = "txtBNSName";
            this.txtBNSName.ReadOnly = true;
            this.txtBNSName.Size = new System.Drawing.Size(271, 20);
            this.txtBNSName.TabIndex = 4;
            // 
            // txtBNSAdd
            // 
            this.txtBNSAdd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBNSAdd.Location = new System.Drawing.Point(71, 83);
            this.txtBNSAdd.Name = "txtBNSAdd";
            this.txtBNSAdd.ReadOnly = true;
            this.txtBNSAdd.Size = new System.Drawing.Size(271, 20);
            this.txtBNSAdd.TabIndex = 5;
            // 
            // txtBNSOwner
            // 
            this.txtBNSOwner.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBNSOwner.Location = new System.Drawing.Point(71, 108);
            this.txtBNSOwner.Name = "txtBNSOwner";
            this.txtBNSOwner.ReadOnly = true;
            this.txtBNSOwner.Size = new System.Drawing.Size(271, 20);
            this.txtBNSOwner.TabIndex = 6;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(215, 27);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(53, 25);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(291, 183);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(53, 25);
            this.btnClose.TabIndex = 12;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(220, 183);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint.Size = new System.Drawing.Size(62, 25);
            this.btnPrint.TabIndex = 11;
            this.btnPrint.Values.Text = "Generate";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // txtLGUCode
            // 
            this.txtLGUCode.Location = new System.Drawing.Point(71, 32);
            this.txtLGUCode.Name = "txtLGUCode";
            this.txtLGUCode.ReadOnly = true;
            this.txtLGUCode.Size = new System.Drawing.Size(26, 20);
            this.txtLGUCode.TabIndex = 11;
            this.txtLGUCode.Text = "232";
            // 
            // txtDISTCode
            // 
            this.txtDISTCode.Location = new System.Drawing.Point(101, 32);
            this.txtDISTCode.Name = "txtDISTCode";
            this.txtDISTCode.ReadOnly = true;
            this.txtDISTCode.Size = new System.Drawing.Size(19, 20);
            this.txtDISTCode.TabIndex = 12;
            this.txtDISTCode.Text = "00";
            // 
            // txtBINYr
            // 
            this.txtBINYr.Location = new System.Drawing.Point(124, 32);
            this.txtBINYr.MaxLength = 4;
            this.txtBINYr.Name = "txtBINYr";
            this.txtBINYr.Size = new System.Drawing.Size(32, 20);
            this.txtBINYr.TabIndex = 1;
            this.txtBINYr.TextChanged += new System.EventHandler(this.txtBINYr_TextChanged);
            this.txtBINYr.Leave += new System.EventHandler(this.txtBINYr_Leave);
            // 
            // txtBINSerial
            // 
            this.txtBINSerial.Location = new System.Drawing.Point(160, 32);
            this.txtBINSerial.MaxLength = 7;
            this.txtBINSerial.Name = "txtBINSerial";
            this.txtBINSerial.Size = new System.Drawing.Size(49, 20);
            this.txtBINSerial.TabIndex = 2;
            this.txtBINSerial.Leave += new System.EventHandler(this.txtBINSerial_Leave);
            // 
            // lblORNo
            // 
            this.lblORNo.AutoSize = true;
            this.lblORNo.Location = new System.Drawing.Point(14, 9);
            this.lblORNo.Name = "lblORNo";
            this.lblORNo.Size = new System.Drawing.Size(43, 13);
            this.lblORNo.TabIndex = 10;
            this.lblORNo.Text = "OR No.";
            // 
            // txtORNo
            // 
            this.txtORNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtORNo.Location = new System.Drawing.Point(63, 6);
            this.txtORNo.Name = "txtORNo";
            this.txtORNo.Size = new System.Drawing.Size(104, 20);
            this.txtORNo.TabIndex = 7;
            this.txtORNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtORNo_KeyPress);
            // 
            // lblORDate
            // 
            this.lblORDate.AutoSize = true;
            this.lblORDate.Location = new System.Drawing.Point(8, 34);
            this.lblORDate.Name = "lblORDate";
            this.lblORDate.Size = new System.Drawing.Size(49, 13);
            this.lblORDate.TabIndex = 10;
            this.lblORDate.Text = "OR Date";
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(14, 59);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(43, 13);
            this.lblAmount.TabIndex = 10;
            this.lblAmount.Text = "Amount";
            // 
            // txtAmount
            // 
            this.txtAmount.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAmount.Location = new System.Drawing.Point(63, 56);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(104, 20);
            this.txtAmount.TabIndex = 9;
            this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAmount_KeyPress);
            // 
            // Addlpnl
            // 
            this.Addlpnl.Controls.Add(this.chkIssued);
            this.Addlpnl.Controls.Add(this.label5);
            this.Addlpnl.Controls.Add(this.dtpIssued);
            this.Addlpnl.Controls.Add(this.dtpORDate);
            this.Addlpnl.Controls.Add(this.label6);
            this.Addlpnl.Controls.Add(this.lblAmount);
            this.Addlpnl.Controls.Add(this.lblORDate);
            this.Addlpnl.Controls.Add(this.cboTaxYear);
            this.Addlpnl.Controls.Add(this.lblORNo);
            this.Addlpnl.Controls.Add(this.txtORNo);
            this.Addlpnl.Controls.Add(this.txtAmount);
            this.Addlpnl.Location = new System.Drawing.Point(8, 130);
            this.Addlpnl.Name = "Addlpnl";
            this.Addlpnl.Size = new System.Drawing.Size(344, 83);
            this.Addlpnl.TabIndex = 13;
            this.Addlpnl.Visible = false;
            // 
            // chkIssued
            // 
            this.chkIssued.AutoSize = true;
            this.chkIssued.Location = new System.Drawing.Point(171, 35);
            this.chkIssued.Name = "chkIssued";
            this.chkIssued.Size = new System.Drawing.Size(15, 14);
            this.chkIssued.TabIndex = 11;
            this.chkIssued.UseVisualStyleBackColor = true;
            this.chkIssued.CheckedChanged += new System.EventHandler(this.chkIssued_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(193, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Tax Year";
            // 
            // dtpIssued
            // 
            this.dtpIssued.CustomFormat = "MM/dd/yyyy";
            this.dtpIssued.Enabled = false;
            this.dtpIssued.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpIssued.Location = new System.Drawing.Point(257, 30);
            this.dtpIssued.Name = "dtpIssued";
            this.dtpIssued.Size = new System.Drawing.Size(78, 20);
            this.dtpIssued.TabIndex = 8;
            this.dtpIssued.Value = new System.DateTime(2014, 5, 15, 0, 0, 0, 0);
            this.dtpIssued.Leave += new System.EventHandler(this.dtpIssued_Leave);
            // 
            // dtpORDate
            // 
            this.dtpORDate.CustomFormat = "MM/dd/yyyy";
            this.dtpORDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpORDate.Location = new System.Drawing.Point(64, 31);
            this.dtpORDate.Name = "dtpORDate";
            this.dtpORDate.Size = new System.Drawing.Size(103, 20);
            this.dtpORDate.TabIndex = 8;
            this.dtpORDate.Value = new System.DateTime(2014, 5, 15, 0, 0, 0, 0);
            this.dtpORDate.Leave += new System.EventHandler(this.dtpORDate_Leave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(188, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Issued Date";
            // 
            // cboTaxYear
            // 
            this.cboTaxYear.FormattingEnabled = true;
            this.cboTaxYear.Location = new System.Drawing.Point(257, 5);
            this.cboTaxYear.MaxLength = 4;
            this.cboTaxYear.Name = "cboTaxYear";
            this.cboTaxYear.Size = new System.Drawing.Size(78, 21);
            this.cboTaxYear.TabIndex = 10;
            this.cboTaxYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboTaxYear_KeyPress);
            // 
            // rdoYear
            // 
            this.rdoYear.AutoSize = true;
            this.rdoYear.Location = new System.Drawing.Point(75, 9);
            this.rdoYear.Name = "rdoYear";
            this.rdoYear.Size = new System.Drawing.Size(47, 17);
            this.rdoYear.TabIndex = 14;
            this.rdoYear.TabStop = true;
            this.rdoYear.Text = "Year";
            this.rdoYear.UseVisualStyleBackColor = true;
            this.rdoYear.CheckedChanged += new System.EventHandler(this.rdoYear_CheckedChanged);
            // 
            // rdoQtr
            // 
            this.rdoQtr.AutoSize = true;
            this.rdoQtr.Location = new System.Drawing.Point(128, 9);
            this.rdoQtr.Name = "rdoQtr";
            this.rdoQtr.Size = new System.Drawing.Size(60, 17);
            this.rdoQtr.TabIndex = 14;
            this.rdoQtr.TabStop = true;
            this.rdoQtr.Text = "Quarter";
            this.rdoQtr.UseVisualStyleBackColor = true;
            this.rdoQtr.CheckedChanged += new System.EventHandler(this.rdoQtr_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(36, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Type";
            // 
            // frmFPCert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 219);
            this.Controls.Add(this.rdoQtr);
            this.Controls.Add(this.rdoYear);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.Addlpnl);
            this.Controls.Add(this.txtBINSerial);
            this.Controls.Add(this.txtBINYr);
            this.Controls.Add(this.txtDISTCode);
            this.Controls.Add(this.txtLGUCode);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtBNSOwner);
            this.Controls.Add(this.txtBNSAdd);
            this.Controls.Add(this.txtBNSName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(377, 257);
            this.MinimumSize = new System.Drawing.Size(377, 257);
            this.Name = "frmFPCert";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Payment Certificate";
            this.Load += new System.EventHandler(this.frmRetCert_Load);
            this.Addlpnl.ResumeLayout(false);
            this.Addlpnl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBNSName;
        private System.Windows.Forms.TextBox txtBNSAdd;
        private System.Windows.Forms.TextBox txtBNSOwner;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPrint;
        private System.Windows.Forms.TextBox txtLGUCode;
        private System.Windows.Forms.TextBox txtDISTCode;
        private System.Windows.Forms.TextBox txtBINYr;
        private System.Windows.Forms.TextBox txtBINSerial;
        private System.Windows.Forms.Label lblORNo;
        private System.Windows.Forms.TextBox txtORNo;
        private System.Windows.Forms.Label lblORDate;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Panel Addlpnl;
        private System.Windows.Forms.DateTimePicker dtpORDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboTaxYear;
        private System.Windows.Forms.CheckBox chkIssued;
        private System.Windows.Forms.DateTimePicker dtpIssued;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton rdoYear;
        private System.Windows.Forms.RadioButton rdoQtr;
        private System.Windows.Forms.Label label7;
    }
}