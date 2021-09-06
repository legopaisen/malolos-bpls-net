namespace Amellar.Modules.BusinessReports
{
    partial class frmBussCert
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoWithRecord = new System.Windows.Forms.RadioButton();
            this.rdoNoRecord = new System.Windows.Forms.RadioButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPrint = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bin1 = new BIN.BIN();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblAcctName = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtMI = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRequestedBy = new System.Windows.Forms.TextBox();
            this.lblPurpose = new System.Windows.Forms.Label();
            this.txtPurpose = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdoWithRecord);
            this.groupBox2.Controls.Add(this.rdoNoRecord);
            this.groupBox2.Location = new System.Drawing.Point(8, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(457, 40);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // rdoWithRecord
            // 
            this.rdoWithRecord.AutoSize = true;
            this.rdoWithRecord.Location = new System.Drawing.Point(252, 13);
            this.rdoWithRecord.Name = "rdoWithRecord";
            this.rdoWithRecord.Size = new System.Drawing.Size(85, 17);
            this.rdoWithRecord.TabIndex = 2;
            this.rdoWithRecord.TabStop = true;
            this.rdoWithRecord.Text = "With Record";
            this.rdoWithRecord.UseVisualStyleBackColor = true;
            this.rdoWithRecord.Visible = false;
            this.rdoWithRecord.Click += new System.EventHandler(this.rdoWithRecord_Click);
            // 
            // rdoNoRecord
            // 
            this.rdoNoRecord.AutoSize = true;
            this.rdoNoRecord.Location = new System.Drawing.Point(123, 13);
            this.rdoNoRecord.Name = "rdoNoRecord";
            this.rdoNoRecord.Size = new System.Drawing.Size(77, 17);
            this.rdoNoRecord.TabIndex = 1;
            this.rdoNoRecord.TabStop = true;
            this.rdoNoRecord.Text = "No Record";
            this.rdoNoRecord.UseVisualStyleBackColor = true;
            this.rdoNoRecord.Visible = false;
            this.rdoNoRecord.Click += new System.EventHandler(this.rdoNoRecord_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(391, 304);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(74, 24);
            this.btnCancel.TabIndex = 19;
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
            this.btnPrint.Location = new System.Drawing.Point(311, 304);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint.Size = new System.Drawing.Size(74, 24);
            this.btnPrint.TabIndex = 17;
            this.btnPrint.Text = "Print";
            this.btnPrint.Values.ExtraText = "";
            this.btnPrint.Values.Image = null;
            this.btnPrint.Values.ImageStates.ImageCheckedNormal = null;
            this.btnPrint.Values.ImageStates.ImageCheckedPressed = null;
            this.btnPrint.Values.ImageStates.ImageCheckedTracking = null;
            this.btnPrint.Values.Text = "Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblPurpose);
            this.groupBox1.Controls.Add(this.txtPurpose);
            this.groupBox1.Controls.Add(this.bin1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lblAcctName);
            this.groupBox1.Controls.Add(this.txtAddress);
            this.groupBox1.Controls.Add(this.txtMI);
            this.groupBox1.Controls.Add(this.txtFirstName);
            this.groupBox1.Controls.Add(this.txtLastName);
            this.groupBox1.Location = new System.Drawing.Point(8, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(457, 229);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(87, 13);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(138, 20);
            this.bin1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Address";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(233, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(74, 24);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(388, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "M.I.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "First Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "BIN";
            // 
            // lblAcctName
            // 
            this.lblAcctName.AutoSize = true;
            this.lblAcctName.Location = new System.Drawing.Point(17, 41);
            this.lblAcctName.Name = "lblAcctName";
            this.lblAcctName.Size = new System.Drawing.Size(58, 13);
            this.lblAcctName.TabIndex = 2;
            this.lblAcctName.Text = "Last Name";
            // 
            // txtAddress
            // 
            this.txtAddress.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAddress.Location = new System.Drawing.Point(87, 91);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(356, 57);
            this.txtAddress.TabIndex = 7;
            // 
            // txtMI
            // 
            this.txtMI.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMI.Location = new System.Drawing.Point(419, 64);
            this.txtMI.MaxLength = 1;
            this.txtMI.Multiline = true;
            this.txtMI.Name = "txtMI";
            this.txtMI.Size = new System.Drawing.Size(24, 20);
            this.txtMI.TabIndex = 6;
            // 
            // txtFirstName
            // 
            this.txtFirstName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFirstName.Location = new System.Drawing.Point(87, 64);
            this.txtFirstName.Multiline = true;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(295, 20);
            this.txtFirstName.TabIndex = 5;
            // 
            // txtLastName
            // 
            this.txtLastName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLastName.Location = new System.Drawing.Point(87, 38);
            this.txtLastName.Multiline = true;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(356, 20);
            this.txtLastName.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 310);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Requested by:";
            // 
            // txtRequestedBy
            // 
            this.txtRequestedBy.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRequestedBy.Location = new System.Drawing.Point(92, 306);
            this.txtRequestedBy.Multiline = true;
            this.txtRequestedBy.Name = "txtRequestedBy";
            this.txtRequestedBy.Size = new System.Drawing.Size(203, 20);
            this.txtRequestedBy.TabIndex = 21;
            // 
            // lblPurpose
            // 
            this.lblPurpose.AutoSize = true;
            this.lblPurpose.Location = new System.Drawing.Point(17, 166);
            this.lblPurpose.Name = "lblPurpose";
            this.lblPurpose.Size = new System.Drawing.Size(46, 13);
            this.lblPurpose.TabIndex = 9;
            this.lblPurpose.Text = "Purpose";
            // 
            // txtPurpose
            // 
            this.txtPurpose.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPurpose.Location = new System.Drawing.Point(87, 166);
            this.txtPurpose.Multiline = true;
            this.txtPurpose.Name = "txtPurpose";
            this.txtPurpose.Size = new System.Drawing.Size(356, 57);
            this.txtPurpose.TabIndex = 10;
            // 
            // frmBussCert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 370);
            this.ControlBox = false;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtRequestedBy);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmBussCert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "With Business";
            this.Load += new System.EventHandler(this.frmBussCert_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoWithRecord;
        private System.Windows.Forms.RadioButton rdoNoRecord;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnPrint;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblAcctName;
        public System.Windows.Forms.TextBox txtAddress;
        public System.Windows.Forms.TextBox txtMI;
        public System.Windows.Forms.TextBox txtFirstName;
        public System.Windows.Forms.TextBox txtLastName;
        private BIN.BIN bin1;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox txtRequestedBy;
        private System.Windows.Forms.Label lblPurpose;
        public System.Windows.Forms.TextBox txtPurpose;
    }
}