namespace Amellar.Modules.BusinessReports
{
    partial class frmCertification
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
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblPurpose = new System.Windows.Forms.Label();
            this.txtPurpose = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblAcctName = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtMI = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPrint = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoWithRecord = new System.Windows.Forms.RadioButton();
            this.rdoNoRecord = new System.Windows.Forms.RadioButton();
            this.txtRequestedBy = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblPurpose);
            this.groupBox1.Controls.Add(this.txtPurpose);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lblAcctName);
            this.groupBox1.Controls.Add(this.txtAddress);
            this.groupBox1.Controls.Add(this.txtMI);
            this.groupBox1.Controls.Add(this.txtFirstName);
            this.groupBox1.Controls.Add(this.txtCode);
            this.groupBox1.Controls.Add(this.txtLastName);
            this.groupBox1.Location = new System.Drawing.Point(13, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(457, 222);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // lblPurpose
            // 
            this.lblPurpose.AutoSize = true;
            this.lblPurpose.Location = new System.Drawing.Point(17, 154);
            this.lblPurpose.Name = "lblPurpose";
            this.lblPurpose.Size = new System.Drawing.Size(43, 13);
            this.lblPurpose.TabIndex = 8;
            this.lblPurpose.Text = "Pupose";
            // 
            // txtPurpose
            // 
            this.txtPurpose.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPurpose.Location = new System.Drawing.Point(87, 154);
            this.txtPurpose.Multiline = true;
            this.txtPurpose.Name = "txtPurpose";
            this.txtPurpose.Size = new System.Drawing.Size(356, 54);
            this.txtPurpose.TabIndex = 9;
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
            this.btnSearch.Location = new System.Drawing.Point(217, 11);
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
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Own Code";
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
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(87, 12);
            this.txtCode.Multiline = true;
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(124, 20);
            this.txtCode.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(396, 313);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(74, 24);
            this.btnCancel.TabIndex = 10;
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
            this.btnPrint.Location = new System.Drawing.Point(316, 313);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint.Size = new System.Drawing.Size(74, 24);
            this.btnPrint.TabIndex = 9;
            this.btnPrint.Text = "Print";
            this.btnPrint.Values.ExtraText = "";
            this.btnPrint.Values.Image = null;
            this.btnPrint.Values.ImageStates.ImageCheckedNormal = null;
            this.btnPrint.Values.ImageStates.ImageCheckedPressed = null;
            this.btnPrint.Values.ImageStates.ImageCheckedTracking = null;
            this.btnPrint.Values.Text = "Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdoWithRecord);
            this.groupBox2.Controls.Add(this.rdoNoRecord);
            this.groupBox2.Location = new System.Drawing.Point(13, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(457, 40);
            this.groupBox2.TabIndex = 9;
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
            this.rdoWithRecord.CheckedChanged += new System.EventHandler(this.rdoWithRecord_CheckedChanged);
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
            this.rdoNoRecord.CheckedChanged += new System.EventHandler(this.rdoNoRecord_CheckedChanged);
            // 
            // txtRequestedBy
            // 
            this.txtRequestedBy.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRequestedBy.Location = new System.Drawing.Point(100, 279);
            this.txtRequestedBy.Multiline = true;
            this.txtRequestedBy.Name = "txtRequestedBy";
            this.txtRequestedBy.Size = new System.Drawing.Size(356, 20);
            this.txtRequestedBy.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 283);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Requested by:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(7, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(167, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "No. of printed Certifications:";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblCount.Location = new System.Drawing.Point(176, 10);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(52, 18);
            this.lblCount.TabIndex = 12;
            this.lblCount.Text = "label7";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblCount);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(13, 310);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(247, 36);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            // 
            // frmCertification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 354);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtRequestedBy);
            this.Name = "frmCertification";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Certification of Business Ownership";
            this.Load += new System.EventHandler(this.frmCertification_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblAcctName;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnPrint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox txtAddress;
        public System.Windows.Forms.TextBox txtMI;
        public System.Windows.Forms.TextBox txtFirstName;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        public System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoWithRecord;
        private System.Windows.Forms.RadioButton rdoNoRecord;
        public System.Windows.Forms.TextBox txtRequestedBy;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblPurpose;
        public System.Windows.Forms.TextBox txtPurpose;
    }
}