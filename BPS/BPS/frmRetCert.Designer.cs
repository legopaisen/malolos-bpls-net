namespace BPLSBilling
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
            this.txtLGUCode = new System.Windows.Forms.TextBox();
            this.txtDISTCode = new System.Windows.Forms.TextBox();
            this.txtBINYr = new System.Windows.Forms.TextBox();
            this.txtBINSerial = new System.Windows.Forms.TextBox();
            this.dtpCeasedDate = new System.Windows.Forms.DateTimePicker();
            this.lblORDate = new System.Windows.Forms.Label();
            this.lblORNo = new System.Windows.Forms.Label();
            this.txtORNo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpIssuedDate = new System.Windows.Forms.DateTimePicker();
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
            this.btnSearch.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnSearch.Location = new System.Drawing.Point(243, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(53, 25);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClose
            // 
            this.btnClose.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnClose.Location = new System.Drawing.Point(314, 149);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(53, 25);
            this.btnClose.TabIndex = 14;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnPrint.Location = new System.Drawing.Point(243, 149);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint.Size = new System.Drawing.Size(62, 25);
            this.btnPrint.TabIndex = 13;
            this.btnPrint.Values.Text = "Generate";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // txtLGUCode
            // 
            this.txtLGUCode.Location = new System.Drawing.Point(96, 18);
            this.txtLGUCode.Name = "txtLGUCode";
            this.txtLGUCode.ReadOnly = true;
            this.txtLGUCode.Size = new System.Drawing.Size(26, 20);
            this.txtLGUCode.TabIndex = 11;
            this.txtLGUCode.Text = "232";
            // 
            // txtDISTCode
            // 
            this.txtDISTCode.Location = new System.Drawing.Point(124, 18);
            this.txtDISTCode.Name = "txtDISTCode";
            this.txtDISTCode.ReadOnly = true;
            this.txtDISTCode.Size = new System.Drawing.Size(19, 20);
            this.txtDISTCode.TabIndex = 12;
            this.txtDISTCode.Text = "00";
            // 
            // txtBINYr
            // 
            this.txtBINYr.Location = new System.Drawing.Point(146, 18);
            this.txtBINYr.MaxLength = 4;
            this.txtBINYr.Name = "txtBINYr";
            this.txtBINYr.Size = new System.Drawing.Size(32, 20);
            this.txtBINYr.TabIndex = 1;
            this.txtBINYr.TextChanged += new System.EventHandler(this.txtBINYr_TextChanged);
            this.txtBINYr.Leave += new System.EventHandler(this.txtBINYr_Leave);
            // 
            // txtBINSerial
            // 
            this.txtBINSerial.Location = new System.Drawing.Point(182, 18);
            this.txtBINSerial.MaxLength = 7;
            this.txtBINSerial.Name = "txtBINSerial";
            this.txtBINSerial.Size = new System.Drawing.Size(49, 20);
            this.txtBINSerial.TabIndex = 2;
            this.txtBINSerial.Leave += new System.EventHandler(this.txtBINSerial_Leave);
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
            // 
            // lblORNo
            // 
            this.lblORNo.AutoSize = true;
            this.lblORNo.Location = new System.Drawing.Point(48, 123);
            this.lblORNo.Name = "lblORNo";
            this.lblORNo.Size = new System.Drawing.Size(43, 13);
            this.lblORNo.TabIndex = 18;
            this.lblORNo.Text = "OR No.";
            // 
            // txtORNo
            // 
            this.txtORNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtORNo.Location = new System.Drawing.Point(96, 120);
            this.txtORNo.Name = "txtORNo";
            this.txtORNo.ReadOnly = true;
            this.txtORNo.Size = new System.Drawing.Size(127, 20);
            this.txtORNo.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Issued Date";
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
            this.dtpIssuedDate.ValueChanged += new System.EventHandler(this.dtpIssuedDate_ValueChanged);
            // 
            // frmRetCert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 207);
            this.Controls.Add(this.dtpIssuedDate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dtpCeasedDate);
            this.Controls.Add(this.lblORDate);
            this.Controls.Add(this.lblORNo);
            this.Controls.Add(this.txtORNo);
            this.Controls.Add(this.txtBINSerial);
            this.Controls.Add(this.txtBINYr);
            this.Controls.Add(this.txtDISTCode);
            this.Controls.Add(this.txtLGUCode);
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
        private System.Windows.Forms.TextBox txtLGUCode;
        private System.Windows.Forms.TextBox txtDISTCode;
        private System.Windows.Forms.TextBox txtBINYr;
        private System.Windows.Forms.TextBox txtBINSerial;
        private System.Windows.Forms.Label lblORDate;
        private System.Windows.Forms.Label lblORNo;
        private System.Windows.Forms.TextBox txtORNo;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.DateTimePicker dtpCeasedDate;
        public System.Windows.Forms.DateTimePicker dtpIssuedDate;
    }
}