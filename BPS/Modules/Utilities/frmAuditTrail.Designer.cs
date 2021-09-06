namespace Amellar.Modules.Utilities
{
    partial class frmAuditTrail
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
            this.chkBussRec = new System.Windows.Forms.CheckBox();
            this.chkUser = new System.Windows.Forms.CheckBox();
            this.chkModule = new System.Windows.Forms.CheckBox();
            this.btnSearchBIN = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.cmbUser = new System.Windows.Forms.ComboBox();
            this.cmbModule = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMI = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.bin1 = new BIN.BIN();
            this.containerWithShadow3 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.chkUserTrans = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkBussRec
            // 
            this.chkBussRec.AutoSize = true;
            this.chkBussRec.Location = new System.Drawing.Point(26, 23);
            this.chkBussRec.Name = "chkBussRec";
            this.chkBussRec.Size = new System.Drawing.Size(106, 17);
            this.chkBussRec.TabIndex = 1;
            this.chkBussRec.Text = "Business Record";
            this.chkBussRec.UseVisualStyleBackColor = true;
            this.chkBussRec.CheckStateChanged += new System.EventHandler(this.chkBussRec_CheckStateChanged);
            // 
            // chkUser
            // 
            this.chkUser.AutoSize = true;
            this.chkUser.Location = new System.Drawing.Point(26, 46);
            this.chkUser.Name = "chkUser";
            this.chkUser.Size = new System.Drawing.Size(83, 17);
            this.chkUser.TabIndex = 2;
            this.chkUser.Text = "User\'s Code";
            this.chkUser.UseVisualStyleBackColor = true;
            this.chkUser.CheckStateChanged += new System.EventHandler(this.chkUser_CheckStateChanged);
            this.chkUser.CheckedChanged += new System.EventHandler(this.chkUser_CheckedChanged);
            // 
            // chkModule
            // 
            this.chkModule.AutoSize = true;
            this.chkModule.Location = new System.Drawing.Point(26, 69);
            this.chkModule.Name = "chkModule";
            this.chkModule.Size = new System.Drawing.Size(120, 17);
            this.chkModule.TabIndex = 3;
            this.chkModule.Text = "Transaction Module";
            this.chkModule.UseVisualStyleBackColor = true;
            this.chkModule.CheckStateChanged += new System.EventHandler(this.chkModule_CheckStateChanged);
            this.chkModule.CheckedChanged += new System.EventHandler(this.chkModule_CheckedChanged);
            // 
            // btnSearchBIN
            // 
            this.btnSearchBIN.Location = new System.Drawing.Point(371, 21);
            this.btnSearchBIN.Name = "btnSearchBIN";
            this.btnSearchBIN.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearchBIN.Size = new System.Drawing.Size(81, 24);
            this.btnSearchBIN.TabIndex = 7;
            this.btnSearchBIN.Text = "Search";
            this.btnSearchBIN.Values.ExtraText = "";
            this.btnSearchBIN.Values.Image = null;
            this.btnSearchBIN.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearchBIN.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearchBIN.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearchBIN.Values.Text = "Search";
            this.btnSearchBIN.Click += new System.EventHandler(this.btnSearchBIN_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(193, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "BIN";
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(62, 158);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(76, 20);
            this.dtpFrom.TabIndex = 4;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            this.dtpFrom.Enter += new System.EventHandler(this.dtpFrom_Enter);
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(62, 183);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(76, 20);
            this.dtpTo.TabIndex = 5;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            this.dtpTo.Enter += new System.EventHandler(this.dtpTo_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Date Range";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "From";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "To";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(177, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "User Name";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(177, 160);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Module";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(284, 204);
            this.btnOk.Name = "btnOk";
            this.btnOk.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOk.Size = new System.Drawing.Size(81, 24);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "OK";
            this.btnOk.Values.ExtraText = "";
            this.btnOk.Values.Image = null;
            this.btnOk.Values.ImageStates.ImageCheckedNormal = null;
            this.btnOk.Values.ImageStates.ImageCheckedPressed = null;
            this.btnOk.Values.ImageStates.ImageCheckedTracking = null;
            this.btnOk.Values.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(371, 204);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(81, 24);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cmbUser
            // 
            this.cmbUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUser.FormattingEnabled = true;
            this.cmbUser.Location = new System.Drawing.Point(240, 65);
            this.cmbUser.Name = "cmbUser";
            this.cmbUser.Size = new System.Drawing.Size(212, 21);
            this.cmbUser.TabIndex = 8;
            this.cmbUser.SelectedIndexChanged += new System.EventHandler(this.cmbUser_SelectedIndexChanged);
            this.cmbUser.Leave += new System.EventHandler(this.cmbUser_Leave);
            this.cmbUser.SelectedValueChanged += new System.EventHandler(this.cmbUser_SelectedValueChanged);
            // 
            // cmbModule
            // 
            this.cmbModule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModule.FormattingEnabled = true;
            this.cmbModule.Location = new System.Drawing.Point(240, 152);
            this.cmbModule.Name = "cmbModule";
            this.cmbModule.Size = new System.Drawing.Size(212, 21);
            this.cmbModule.TabIndex = 9;
            this.cmbModule.SelectedValueChanged += new System.EventHandler(this.cmbModule_SelectedValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(177, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Last Name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(177, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "First Name";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(397, 121);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "M.I.";
            // 
            // txtMI
            // 
            this.txtMI.Location = new System.Drawing.Point(425, 118);
            this.txtMI.Name = "txtMI";
            this.txtMI.ReadOnly = true;
            this.txtMI.Size = new System.Drawing.Size(27, 20);
            this.txtMI.TabIndex = 16;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(240, 118);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.ReadOnly = true;
            this.txtFirstName.Size = new System.Drawing.Size(151, 20);
            this.txtFirstName.TabIndex = 15;
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(240, 92);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.ReadOnly = true;
            this.txtLastName.Size = new System.Drawing.Size(212, 20);
            this.txtLastName.TabIndex = 14;
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(224, 23);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(141, 22);
            this.bin1.TabIndex = 6;
            // 
            // containerWithShadow3
            // 
            this.containerWithShadow3.Location = new System.Drawing.Point(8, 127);
            this.containerWithShadow3.Name = "containerWithShadow3";
            this.containerWithShadow3.Size = new System.Drawing.Size(145, 98);
            this.containerWithShadow3.TabIndex = 5;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(8, 7);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(145, 119);
            this.containerWithShadow1.TabIndex = 5;
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(159, 7);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(309, 195);
            this.containerWithShadow2.TabIndex = 24;
            // 
            // chkUserTrans
            // 
            this.chkUserTrans.AutoSize = true;
            this.chkUserTrans.Location = new System.Drawing.Point(26, 91);
            this.chkUserTrans.Name = "chkUserTrans";
            this.chkUserTrans.Size = new System.Drawing.Size(114, 17);
            this.chkUserTrans.TabIndex = 25;
            this.chkUserTrans.Text = "User\'s Transaction";
            this.chkUserTrans.UseVisualStyleBackColor = true;
            this.chkUserTrans.Visible = false;
            this.chkUserTrans.CheckedChanged += new System.EventHandler(this.chkUserTrans_CheckStateChanged);
            // 
            // frmAuditTrail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(476, 235);
            this.Controls.Add(this.chkUserTrans);
            this.Controls.Add(this.cmbModule);
            this.Controls.Add(this.cmbUser);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtMI);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnSearchBIN);
            this.Controls.Add(this.chkModule);
            this.Controls.Add(this.chkUser);
            this.Controls.Add(this.chkBussRec);
            this.Controls.Add(this.bin1);
            this.Controls.Add(this.containerWithShadow3);
            this.Controls.Add(this.containerWithShadow1);
            this.Controls.Add(this.containerWithShadow2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAuditTrail";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Audit Trail";
            this.Load += new System.EventHandler(this.frmAuditTrail_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkBussRec;
        private System.Windows.Forms.CheckBox chkUser;
        private System.Windows.Forms.CheckBox chkModule;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.Label label1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        public BIN.BIN bin1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnSearchBIN;
        public System.Windows.Forms.ComboBox cmbUser;
        public System.Windows.Forms.ComboBox cmbModule;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.TextBox txtMI;
        public System.Windows.Forms.TextBox txtFirstName;
        public System.Windows.Forms.TextBox txtLastName;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        public System.Windows.Forms.DateTimePicker dtpFrom;
        public System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.CheckBox chkUserTrans;
    }
}