namespace Amellar.Modules.SpecialOrdinances
{
    partial class frmReport
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
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.chkBnsType = new System.Windows.Forms.CheckBox();
            this.chkTagDate = new System.Windows.Forms.CheckBox();
            this.chkDateRange = new System.Windows.Forms.CheckBox();
            this.chkUser = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.dtpTagDate = new System.Windows.Forms.DateTimePicker();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.cmbBnsType = new System.Windows.Forms.ComboBox();
            this.cmbUserCode = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(446, 194);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(81, 24);
            this.btnClose.TabIndex = 9;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(359, 194);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(81, 24);
            this.btnGenerate.TabIndex = 6;
            this.btnGenerate.Values.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkAll);
            this.groupBox1.Controls.Add(this.chkBnsType);
            this.groupBox1.Controls.Add(this.chkTagDate);
            this.groupBox1.Controls.Add(this.chkDateRange);
            this.groupBox1.Controls.Add(this.chkUser);
            this.groupBox1.Location = new System.Drawing.Point(18, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(121, 151);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Printing Option: ";
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(8, 25);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(80, 17);
            this.chkAll.TabIndex = 4;
            this.chkAll.Text = "All Records";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckStateChanged += new System.EventHandler(this.chkAll_CheckStateChanged);
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // chkBnsType
            // 
            this.chkBnsType.AutoSize = true;
            this.chkBnsType.Location = new System.Drawing.Point(8, 119);
            this.chkBnsType.Name = "chkBnsType";
            this.chkBnsType.Size = new System.Drawing.Size(110, 17);
            this.chkBnsType.TabIndex = 3;
            this.chkBnsType.Text = "By Business Type";
            this.chkBnsType.UseVisualStyleBackColor = true;
            this.chkBnsType.CheckStateChanged += new System.EventHandler(this.chkBnsType_CheckStateChanged);
            // 
            // chkTagDate
            // 
            this.chkTagDate.AutoSize = true;
            this.chkTagDate.Location = new System.Drawing.Point(8, 96);
            this.chkTagDate.Name = "chkTagDate";
            this.chkTagDate.Size = new System.Drawing.Size(106, 17);
            this.chkTagDate.TabIndex = 2;
            this.chkTagDate.Text = "By Tagging Date";
            this.chkTagDate.UseVisualStyleBackColor = true;
            this.chkTagDate.CheckStateChanged += new System.EventHandler(this.chkTagDate_CheckStateChanged);
            // 
            // chkDateRange
            // 
            this.chkDateRange.AutoSize = true;
            this.chkDateRange.Location = new System.Drawing.Point(8, 71);
            this.chkDateRange.Name = "chkDateRange";
            this.chkDateRange.Size = new System.Drawing.Size(99, 17);
            this.chkDateRange.TabIndex = 1;
            this.chkDateRange.Text = "By Date Range";
            this.chkDateRange.UseVisualStyleBackColor = true;
            this.chkDateRange.CheckStateChanged += new System.EventHandler(this.chkDateRange_CheckStateChanged);
            // 
            // chkUser
            // 
            this.chkUser.AutoSize = true;
            this.chkUser.Location = new System.Drawing.Point(8, 48);
            this.chkUser.Name = "chkUser";
            this.chkUser.Size = new System.Drawing.Size(91, 17);
            this.chkUser.TabIndex = 0;
            this.chkUser.Text = "By User Code";
            this.chkUser.UseVisualStyleBackColor = true;
            this.chkUser.CheckStateChanged += new System.EventHandler(this.chkUser_CheckStateChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtUserName);
            this.groupBox2.Controls.Add(this.dtpDateTo);
            this.groupBox2.Controls.Add(this.dtpTagDate);
            this.groupBox2.Controls.Add(this.dtpDateFrom);
            this.groupBox2.Controls.Add(this.cmbBnsType);
            this.groupBox2.Controls.Add(this.cmbUserCode);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(145, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(383, 151);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(91, 42);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.ReadOnly = true;
            this.txtUserName.Size = new System.Drawing.Size(283, 20);
            this.txtUserName.TabIndex = 3;
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateTo.Location = new System.Drawing.Point(230, 67);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(81, 20);
            this.dtpDateTo.TabIndex = 2;
            // 
            // dtpTagDate
            // 
            this.dtpTagDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTagDate.Location = new System.Drawing.Point(91, 92);
            this.dtpTagDate.Name = "dtpTagDate";
            this.dtpTagDate.Size = new System.Drawing.Size(81, 20);
            this.dtpTagDate.TabIndex = 2;
            // 
            // dtpDateFrom
            // 
            this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateFrom.Location = new System.Drawing.Point(91, 67);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(81, 20);
            this.dtpDateFrom.TabIndex = 2;
            // 
            // cmbBnsType
            // 
            this.cmbBnsType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBnsType.FormattingEnabled = true;
            this.cmbBnsType.Location = new System.Drawing.Point(91, 117);
            this.cmbBnsType.Name = "cmbBnsType";
            this.cmbBnsType.Size = new System.Drawing.Size(283, 21);
            this.cmbBnsType.TabIndex = 1;
            // 
            // cmbUserCode
            // 
            this.cmbUserCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUserCode.FormattingEnabled = true;
            this.cmbUserCode.Location = new System.Drawing.Point(91, 17);
            this.cmbUserCode.Name = "cmbUserCode";
            this.cmbUserCode.Size = new System.Drawing.Size(140, 21);
            this.cmbUserCode.TabIndex = 1;
            this.cmbUserCode.SelectedIndexChanged += new System.EventHandler(this.cmbUserCode_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Tagging Date";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Business Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(178, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Date To";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Date From";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "User Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "User Code";
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(6, 12);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(531, 176);
            this.containerWithShadow2.TabIndex = 10;
            // 
            // frmReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 230);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.containerWithShadow2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnGenerate);
            this.Name = "frmReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Special Ordinance Report";
            this.Load += new System.EventHandler(this.frmReport_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.CheckBox chkBnsType;
        private System.Windows.Forms.CheckBox chkTagDate;
        private System.Windows.Forms.CheckBox chkDateRange;
        private System.Windows.Forms.CheckBox chkUser;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbBnsType;
        private System.Windows.Forms.ComboBox cmbUserCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpDateTo;
        private System.Windows.Forms.DateTimePicker dtpTagDate;
        private System.Windows.Forms.DateTimePicker dtpDateFrom;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label6;
        
    }
}

