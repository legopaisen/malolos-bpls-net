namespace Amellar.Modules.BusinessReports
{
    partial class frmRetCert
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
            this.dtpCeasedDate = new System.Windows.Forms.DateTimePicker();
            this.lblORDate = new System.Windows.Forms.Label();
            this.lblORNo = new System.Windows.Forms.Label();
            this.txtORNo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpIssuedDate = new System.Windows.Forms.DateTimePicker();
            this.bin1 = new Amellar.Common.BIN.BIN();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbInspector = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDoc = new System.Windows.Forms.TextBox();
            this.txtPage = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtBook = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtNotary = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtDueTo = new System.Windows.Forms.TextBox();
            this.rdbIndividual = new System.Windows.Forms.RadioButton();
            this.rdbCorp = new System.Windows.Forms.RadioButton();
            this.txtSeriesYr = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "BIN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "BNS Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "BNS Add";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(53, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Owner";
            // 
            // txtBNSName
            // 
            this.txtBNSName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBNSName.Location = new System.Drawing.Point(96, 43);
            this.txtBNSName.Name = "txtBNSName";
            this.txtBNSName.ReadOnly = true;
            this.txtBNSName.Size = new System.Drawing.Size(275, 20);
            this.txtBNSName.TabIndex = 4;
            // 
            // txtBNSAdd
            // 
            this.txtBNSAdd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBNSAdd.Location = new System.Drawing.Point(96, 69);
            this.txtBNSAdd.Name = "txtBNSAdd";
            this.txtBNSAdd.ReadOnly = true;
            this.txtBNSAdd.Size = new System.Drawing.Size(275, 20);
            this.txtBNSAdd.TabIndex = 5;
            // 
            // txtBNSOwner
            // 
            this.txtBNSOwner.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBNSOwner.Location = new System.Drawing.Point(96, 94);
            this.txtBNSOwner.Name = "txtBNSOwner";
            this.txtBNSOwner.ReadOnly = true;
            this.txtBNSOwner.Size = new System.Drawing.Size(275, 20);
            this.txtBNSOwner.TabIndex = 6;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(243, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(62, 25);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.Values.ExtraText = "";
            this.btnSearch.Values.Image = null;
            this.btnSearch.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearch.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearch.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(322, 357);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(62, 25);
            this.btnClose.TabIndex = 14;
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
            this.btnPrint.Location = new System.Drawing.Point(243, 357);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint.Size = new System.Drawing.Size(62, 25);
            this.btnPrint.TabIndex = 13;
            this.btnPrint.Text = "Generate";
            this.btnPrint.Values.ExtraText = "";
            this.btnPrint.Values.Image = null;
            this.btnPrint.Values.ImageStates.ImageCheckedNormal = null;
            this.btnPrint.Values.ImageStates.ImageCheckedPressed = null;
            this.btnPrint.Values.ImageStates.ImageCheckedTracking = null;
            this.btnPrint.Values.Text = "Generate";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // dtpCeasedDate
            // 
            this.dtpCeasedDate.CustomFormat = "MM/dd/yyyy";
            this.dtpCeasedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCeasedDate.Location = new System.Drawing.Point(96, 145);
            this.dtpCeasedDate.Name = "dtpCeasedDate";
            this.dtpCeasedDate.Size = new System.Drawing.Size(126, 20);
            this.dtpCeasedDate.TabIndex = 16;
            this.dtpCeasedDate.Value = new System.DateTime(2014, 5, 15, 0, 0, 0, 0);
            this.dtpCeasedDate.Visible = false;
            this.dtpCeasedDate.ValueChanged += new System.EventHandler(this.dtpCeasedDate_ValueChanged);
            // 
            // lblORDate
            // 
            this.lblORDate.AutoSize = true;
            this.lblORDate.Location = new System.Drawing.Point(22, 149);
            this.lblORDate.Name = "lblORDate";
            this.lblORDate.Size = new System.Drawing.Size(69, 13);
            this.lblORDate.TabIndex = 17;
            this.lblORDate.Text = "Ceased Date";
            this.lblORDate.Visible = false;
            // 
            // lblORNo
            // 
            this.lblORNo.AutoSize = true;
            this.lblORNo.Location = new System.Drawing.Point(48, 123);
            this.lblORNo.Name = "lblORNo";
            this.lblORNo.Size = new System.Drawing.Size(43, 13);
            this.lblORNo.TabIndex = 18;
            this.lblORNo.Text = "OR No.";
            this.lblORNo.Visible = false;
            // 
            // txtORNo
            // 
            this.txtORNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtORNo.Location = new System.Drawing.Point(96, 120);
            this.txtORNo.Name = "txtORNo";
            this.txtORNo.ReadOnly = true;
            this.txtORNo.Size = new System.Drawing.Size(127, 20);
            this.txtORNo.TabIndex = 15;
            this.txtORNo.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Issued Date";
            this.label5.Visible = false;
            // 
            // dtpIssuedDate
            // 
            this.dtpIssuedDate.CustomFormat = "MM/dd/yyyy";
            this.dtpIssuedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpIssuedDate.Location = new System.Drawing.Point(96, 171);
            this.dtpIssuedDate.Name = "dtpIssuedDate";
            this.dtpIssuedDate.Size = new System.Drawing.Size(126, 20);
            this.dtpIssuedDate.TabIndex = 16;
            this.dtpIssuedDate.Value = new System.DateTime(2014, 5, 15, 0, 0, 0, 0);
            this.dtpIssuedDate.Visible = false;
            this.dtpIssuedDate.ValueChanged += new System.EventHandler(this.dtpIssuedDate_ValueChanged);
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(99, 17);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(138, 20);
            this.bin1.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(40, 212);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Inspector";
            // 
            // cmbInspector
            // 
            this.cmbInspector.FormattingEnabled = true;
            this.cmbInspector.Location = new System.Drawing.Point(99, 209);
            this.cmbInspector.Name = "cmbInspector";
            this.cmbInspector.Size = new System.Drawing.Size(121, 21);
            this.cmbInspector.TabIndex = 21;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(44, 246);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Doc No.";
            // 
            // txtDoc
            // 
            this.txtDoc.Location = new System.Drawing.Point(99, 243);
            this.txtDoc.Name = "txtDoc";
            this.txtDoc.Size = new System.Drawing.Size(46, 20);
            this.txtDoc.TabIndex = 23;
            // 
            // txtPage
            // 
            this.txtPage.Location = new System.Drawing.Point(201, 243);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(46, 20);
            this.txtPage.TabIndex = 25;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(148, 246);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Page No.";
            // 
            // txtBook
            // 
            this.txtBook.Location = new System.Drawing.Point(310, 243);
            this.txtBook.Name = "txtBook";
            this.txtBook.Size = new System.Drawing.Size(46, 20);
            this.txtBook.TabIndex = 27;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(253, 246);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Book No.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 282);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "Notary Person";
            // 
            // txtNotary
            // 
            this.txtNotary.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNotary.Location = new System.Drawing.Point(99, 279);
            this.txtNotary.Name = "txtNotary";
            this.txtNotary.Size = new System.Drawing.Size(285, 20);
            this.txtNotary.TabIndex = 29;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(49, 309);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 13);
            this.label11.TabIndex = 30;
            this.label11.Text = "Due to:";
            // 
            // txtDueTo
            // 
            this.txtDueTo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtDueTo.Location = new System.Drawing.Point(99, 305);
            this.txtDueTo.Name = "txtDueTo";
            this.txtDueTo.Size = new System.Drawing.Size(285, 20);
            this.txtDueTo.TabIndex = 31;
            // 
            // rdbIndividual
            // 
            this.rdbIndividual.AutoSize = true;
            this.rdbIndividual.Location = new System.Drawing.Point(99, 334);
            this.rdbIndividual.Name = "rdbIndividual";
            this.rdbIndividual.Size = new System.Drawing.Size(70, 17);
            this.rdbIndividual.TabIndex = 32;
            this.rdbIndividual.TabStop = true;
            this.rdbIndividual.Text = "Individual";
            this.rdbIndividual.UseVisualStyleBackColor = true;
            // 
            // rdbCorp
            // 
            this.rdbCorp.AutoSize = true;
            this.rdbCorp.Location = new System.Drawing.Point(99, 357);
            this.rdbCorp.Name = "rdbCorp";
            this.rdbCorp.Size = new System.Drawing.Size(79, 17);
            this.rdbCorp.TabIndex = 33;
            this.rdbCorp.TabStop = true;
            this.rdbCorp.Text = "Corporation";
            this.rdbCorp.UseVisualStyleBackColor = true;
            // 
            // txtSeriesYr
            // 
            this.txtSeriesYr.Location = new System.Drawing.Point(286, 209);
            this.txtSeriesYr.MaxLength = 4;
            this.txtSeriesYr.Name = "txtSeriesYr";
            this.txtSeriesYr.Size = new System.Drawing.Size(55, 20);
            this.txtSeriesYr.TabIndex = 35;
            this.txtSeriesYr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSeriesYr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSeriesYr_KeyPress);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(231, 212);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(48, 13);
            this.label12.TabIndex = 34;
            this.label12.Text = "Series of";
            // 
            // frmRetCert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 389);
            this.Controls.Add(this.txtSeriesYr);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.rdbCorp);
            this.Controls.Add(this.rdbIndividual);
            this.Controls.Add(this.txtDueTo);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtNotary);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtBook);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtPage);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtDoc);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbInspector);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.bin1);
            this.Controls.Add(this.dtpIssuedDate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dtpCeasedDate);
            this.Controls.Add(this.lblORDate);
            this.Controls.Add(this.lblORNo);
            this.Controls.Add(this.txtORNo);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtBNSOwner);
            this.Controls.Add(this.txtBNSAdd);
            this.Controls.Add(this.txtBNSName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRetCert";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Retirement Certificate";
            this.Load += new System.EventHandler(this.frmRetCert_Load);
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
        private System.Windows.Forms.Label lblORDate;
        private System.Windows.Forms.Label lblORNo;
        private System.Windows.Forms.TextBox txtORNo;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.DateTimePicker dtpCeasedDate;
        public System.Windows.Forms.DateTimePicker dtpIssuedDate;
        public Amellar.Common.BIN.BIN bin1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbInspector;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDoc;
        private System.Windows.Forms.TextBox txtPage;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBook;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtNotary;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtDueTo;
        private System.Windows.Forms.RadioButton rdbIndividual;
        private System.Windows.Forms.RadioButton rdbCorp;
        private System.Windows.Forms.TextBox txtSeriesYr;
        private System.Windows.Forms.Label label12;
    }
}