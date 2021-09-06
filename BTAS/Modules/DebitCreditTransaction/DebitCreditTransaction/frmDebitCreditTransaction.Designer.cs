namespace Amellar.Modules.DebitCreditTransaction
{
    partial class frmDebitCreditTransaction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDebitCreditTransaction));
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.bin1 = new BIN.BIN();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTitle = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow3 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.containerWithShadow4 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.kryptonHeader3 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClear = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSave = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.txtTaxPayrName = new System.Windows.Forms.TextBox();
            this.txtTaxPayrAdd = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.rdoDebit = new System.Windows.Forms.RadioButton();
            this.rdoCredit = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtChkNo = new System.Windows.Forms.TextBox();
            this.txtORNo = new System.Windows.Forms.TextBox();
            this.dtpORDate = new System.Windows.Forms.DateTimePicker();
            this.txtDbAmount = new System.Windows.Forms.TextBox();
            this.txtBalance = new System.Windows.Forms.TextBox();
            this.txtCdAmount = new System.Windows.Forms.TextBox();
            this.txtMemo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(4, 3);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(535, 158);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(54, 44);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(143, 20);
            this.bin1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "BIN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Business Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Tax Payer\'s Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Address";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = false;
            this.lblTitle.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.lblTitle.Location = new System.Drawing.Point(7, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.lblTitle.Size = new System.Drawing.Size(531, 24);
            this.lblTitle.TabIndex = 75;
            this.lblTitle.Values.Description = "";
            this.lblTitle.Values.Heading = "Business / Owner Information";
            this.lblTitle.Values.Image = ((System.Drawing.Image)(resources.GetObject("lblTitle.Values.Image")));
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(7, 162);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader1.Size = new System.Drawing.Size(243, 24);
            this.kryptonHeader1.TabIndex = 77;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "References";
            this.kryptonHeader1.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader1.Values.Image")));
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(4, 164);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(247, 125);
            this.containerWithShadow2.TabIndex = 76;
            // 
            // containerWithShadow3
            // 
            this.containerWithShadow3.Location = new System.Drawing.Point(3, 291);
            this.containerWithShadow3.Name = "containerWithShadow3";
            this.containerWithShadow3.Size = new System.Drawing.Size(535, 113);
            this.containerWithShadow3.TabIndex = 76;
            // 
            // kryptonHeader2
            // 
            this.kryptonHeader2.AutoSize = false;
            this.kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader2.Location = new System.Drawing.Point(6, 290);
            this.kryptonHeader2.Name = "kryptonHeader2";
            this.kryptonHeader2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader2.Size = new System.Drawing.Size(531, 24);
            this.kryptonHeader2.TabIndex = 77;
            this.kryptonHeader2.Values.Description = "";
            this.kryptonHeader2.Values.Heading = "Memoranda";
            this.kryptonHeader2.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader2.Values.Image")));
            // 
            // containerWithShadow4
            // 
            this.containerWithShadow4.Location = new System.Drawing.Point(255, 162);
            this.containerWithShadow4.Name = "containerWithShadow4";
            this.containerWithShadow4.Size = new System.Drawing.Size(283, 127);
            this.containerWithShadow4.TabIndex = 76;
            // 
            // kryptonHeader3
            // 
            this.kryptonHeader3.AutoSize = false;
            this.kryptonHeader3.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader3.Location = new System.Drawing.Point(258, 162);
            this.kryptonHeader3.Name = "kryptonHeader3";
            this.kryptonHeader3.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader3.Size = new System.Drawing.Size(279, 25);
            this.kryptonHeader3.TabIndex = 77;
            this.kryptonHeader3.Tag = "";
            this.kryptonHeader3.Values.Description = "";
            this.kryptonHeader3.Values.Heading = "Amout Details";
            this.kryptonHeader3.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader3.Values.Image")));
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(200, 40);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(73, 25);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Values.Text = "S&earch";
            this.btnSearch.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(279, 40);
            this.btnClear.Name = "btnClear";
            this.btnClear.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClear.Size = new System.Drawing.Size(73, 25);
            this.btnClear.TabIndex = 3;
            this.btnClear.Values.Text = "C&lear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(462, 405);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(73, 25);
            this.btnCancel.TabIndex = 80;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(383, 405);
            this.btnSave.Name = "btnSave";
            this.btnSave.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSave.Size = new System.Drawing.Size(73, 25);
            this.btnSave.TabIndex = 79;
            this.btnSave.Values.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtBnsName
            // 
            this.txtBnsName.Location = new System.Drawing.Point(123, 74);
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(275, 20);
            this.txtBnsName.TabIndex = 4;
            // 
            // txtTaxPayrName
            // 
            this.txtTaxPayrName.Location = new System.Drawing.Point(123, 99);
            this.txtTaxPayrName.Name = "txtTaxPayrName";
            this.txtTaxPayrName.ReadOnly = true;
            this.txtTaxPayrName.Size = new System.Drawing.Size(275, 20);
            this.txtTaxPayrName.TabIndex = 5;
            // 
            // txtTaxPayrAdd
            // 
            this.txtTaxPayrAdd.Location = new System.Drawing.Point(123, 124);
            this.txtTaxPayrAdd.Name = "txtTaxPayrAdd";
            this.txtTaxPayrAdd.ReadOnly = true;
            this.txtTaxPayrAdd.Size = new System.Drawing.Size(275, 20);
            this.txtTaxPayrAdd.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 250);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 85;
            this.label5.Text = "OR Date";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 200);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 84;
            this.label6.Text = "Check No.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 225);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 83;
            this.label7.Text = "OR No.";
            // 
            // rdoDebit
            // 
            this.rdoDebit.AutoSize = true;
            this.rdoDebit.Enabled = false;
            this.rdoDebit.Location = new System.Drawing.Point(267, 196);
            this.rdoDebit.Name = "rdoDebit";
            this.rdoDebit.Size = new System.Drawing.Size(107, 17);
            this.rdoDebit.TabIndex = 86;
            this.rdoDebit.TabStop = true;
            this.rdoDebit.Text = "Debit Amount ( - )";
            this.rdoDebit.UseVisualStyleBackColor = true;
            this.rdoDebit.Click += new System.EventHandler(this.rdoDebit_Click);
            // 
            // rdoCredit
            // 
            this.rdoCredit.AutoSize = true;
            this.rdoCredit.Enabled = false;
            this.rdoCredit.Location = new System.Drawing.Point(267, 223);
            this.rdoCredit.Name = "rdoCredit";
            this.rdoCredit.Size = new System.Drawing.Size(112, 17);
            this.rdoCredit.TabIndex = 86;
            this.rdoCredit.TabStop = true;
            this.rdoCredit.Text = "Credit Amount ( + )";
            this.rdoCredit.UseVisualStyleBackColor = true;
            this.rdoCredit.Click += new System.EventHandler(this.rdoCredit_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(283, 256);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 85;
            this.label8.Text = "Balance";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label9.Location = new System.Drawing.Point(335, 245);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(195, 3);
            this.label9.TabIndex = 87;
            // 
            // txtChkNo
            // 
            this.txtChkNo.Location = new System.Drawing.Point(87, 195);
            this.txtChkNo.Name = "txtChkNo";
            this.txtChkNo.Size = new System.Drawing.Size(146, 20);
            this.txtChkNo.TabIndex = 82;
            // 
            // txtORNo
            // 
            this.txtORNo.Location = new System.Drawing.Point(87, 220);
            this.txtORNo.Name = "txtORNo";
            this.txtORNo.Size = new System.Drawing.Size(146, 20);
            this.txtORNo.TabIndex = 82;
            // 
            // dtpORDate
            // 
            this.dtpORDate.CustomFormat = "MM/dd/yyyy";
            this.dtpORDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpORDate.Location = new System.Drawing.Point(87, 246);
            this.dtpORDate.Name = "dtpORDate";
            this.dtpORDate.Size = new System.Drawing.Size(146, 20);
            this.dtpORDate.TabIndex = 88;
            this.dtpORDate.Value = new System.DateTime(2014, 3, 21, 0, 0, 0, 0);
            // 
            // txtDbAmount
            // 
            this.txtDbAmount.Location = new System.Drawing.Point(383, 196);
            this.txtDbAmount.Name = "txtDbAmount";
            this.txtDbAmount.ReadOnly = true;
            this.txtDbAmount.Size = new System.Drawing.Size(146, 20);
            this.txtDbAmount.TabIndex = 82;
            this.txtDbAmount.Text = "0.00";
            this.txtDbAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDbAmount.TextChanged += new System.EventHandler(this.txtDbAmount_TextChanged);
            this.txtDbAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDbAmount_KeyPress);
            // 
            // txtBalance
            // 
            this.txtBalance.Location = new System.Drawing.Point(383, 253);
            this.txtBalance.Name = "txtBalance";
            this.txtBalance.ReadOnly = true;
            this.txtBalance.Size = new System.Drawing.Size(146, 20);
            this.txtBalance.TabIndex = 82;
            this.txtBalance.Text = "0.00";
            this.txtBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBalance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDbAmount_KeyPress);
            // 
            // txtCdAmount
            // 
            this.txtCdAmount.Location = new System.Drawing.Point(383, 221);
            this.txtCdAmount.Name = "txtCdAmount";
            this.txtCdAmount.ReadOnly = true;
            this.txtCdAmount.Size = new System.Drawing.Size(146, 20);
            this.txtCdAmount.TabIndex = 82;
            this.txtCdAmount.Text = "0.00";
            this.txtCdAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCdAmount.TextChanged += new System.EventHandler(this.txtDbAmount_TextChanged);
            this.txtCdAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDbAmount_KeyPress);
            // 
            // txtMemo
            // 
            this.txtMemo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMemo.Location = new System.Drawing.Point(12, 320);
            this.txtMemo.Multiline = true;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMemo.Size = new System.Drawing.Size(519, 70);
            this.txtMemo.TabIndex = 82;
            // 
            // frmDebitCreditTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(542, 434);
            this.Controls.Add(this.dtpORDate);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.rdoCredit);
            this.Controls.Add(this.rdoDebit);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtTaxPayrAdd);
            this.Controls.Add(this.txtTaxPayrName);
            this.Controls.Add(this.txtBalance);
            this.Controls.Add(this.txtORNo);
            this.Controls.Add(this.txtCdAmount);
            this.Controls.Add(this.txtDbAmount);
            this.Controls.Add(this.txtMemo);
            this.Controls.Add(this.txtChkNo);
            this.Controls.Add(this.txtBnsName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.kryptonHeader2);
            this.Controls.Add(this.containerWithShadow3);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bin1);
            this.Controls.Add(this.containerWithShadow1);
            this.Controls.Add(this.kryptonHeader3);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.containerWithShadow4);
            this.Controls.Add(this.containerWithShadow2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmDebitCreditTransaction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Debit / Credit";
            this.Load += new System.EventHandler(this.frmDebitCreditTransaction_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private BIN.BIN bin1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader lblTitle;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow3;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader2;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow4;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader3;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClear;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSave;
        private System.Windows.Forms.TextBox txtBnsName;
        private System.Windows.Forms.TextBox txtTaxPayrName;
        private System.Windows.Forms.TextBox txtTaxPayrAdd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton rdoDebit;
        private System.Windows.Forms.RadioButton rdoCredit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtChkNo;
        private System.Windows.Forms.TextBox txtORNo;
        private System.Windows.Forms.DateTimePicker dtpORDate;
        private System.Windows.Forms.TextBox txtDbAmount;
        private System.Windows.Forms.TextBox txtBalance;
        private System.Windows.Forms.TextBox txtCdAmount;
        private System.Windows.Forms.TextBox txtMemo;
    }
}

