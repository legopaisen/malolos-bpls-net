namespace Amellar.Modules.SpecialOrdinances
{
    partial class frmTagging
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
            this.chkTagging = new System.Windows.Forms.CheckBox();
            this.chkReport = new System.Windows.Forms.CheckBox();
            this.cmbOrdinance = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.txtOwner = new System.Windows.Forms.TextBox();
            this.btnSearchBIN = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDelete = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSave = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.bin1 = new Amellar.Common.BIN.BIN();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRegNo = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpRegDate = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // chkTagging
            // 
            this.chkTagging.AutoSize = true;
            this.chkTagging.Location = new System.Drawing.Point(26, 53);
            this.chkTagging.Name = "chkTagging";
            this.chkTagging.Size = new System.Drawing.Size(45, 17);
            this.chkTagging.TabIndex = 2;
            this.chkTagging.Text = "Tag";
            this.chkTagging.UseVisualStyleBackColor = true;
            this.chkTagging.CheckStateChanged += new System.EventHandler(this.chkTagging_CheckStateChanged);
            // 
            // chkReport
            // 
            this.chkReport.AutoSize = true;
            this.chkReport.Location = new System.Drawing.Point(26, 23);
            this.chkReport.Name = "chkReport";
            this.chkReport.Size = new System.Drawing.Size(58, 17);
            this.chkReport.TabIndex = 1;
            this.chkReport.Text = "Report";
            this.chkReport.UseVisualStyleBackColor = true;
            this.chkReport.CheckStateChanged += new System.EventHandler(this.chkReport_CheckStateChanged);
            // 
            // cmbOrdinance
            // 
            this.cmbOrdinance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrdinance.FormattingEnabled = true;
            this.cmbOrdinance.Location = new System.Drawing.Point(208, 17);
            this.cmbOrdinance.Name = "cmbOrdinance";
            this.cmbOrdinance.Size = new System.Drawing.Size(220, 21);
            this.cmbOrdinance.TabIndex = 3;
            this.cmbOrdinance.SelectedIndexChanged += new System.EventHandler(this.cmbOrdinance_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(113, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select Ordinance";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(113, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "From";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Bns Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Owner";
            // 
            // txtBnsName
            // 
            this.txtBnsName.Location = new System.Drawing.Point(76, 132);
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(352, 20);
            this.txtBnsName.TabIndex = 6;
            // 
            // txtOwner
            // 
            this.txtOwner.Location = new System.Drawing.Point(76, 156);
            this.txtOwner.Name = "txtOwner";
            this.txtOwner.ReadOnly = true;
            this.txtOwner.Size = new System.Drawing.Size(352, 20);
            this.txtOwner.TabIndex = 6;
            // 
            // btnSearchBIN
            // 
            this.btnSearchBIN.Location = new System.Drawing.Point(222, 102);
            this.btnSearchBIN.Name = "btnSearchBIN";
            this.btnSearchBIN.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearchBIN.Size = new System.Drawing.Size(81, 24);
            this.btnSearchBIN.TabIndex = 5;
            this.btnSearchBIN.Text = "Search Bin";
            this.btnSearchBIN.Values.ExtraText = "";
            this.btnSearchBIN.Values.Image = null;
            this.btnSearchBIN.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearchBIN.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearchBIN.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearchBIN.Values.Text = "Search Bin";
            this.btnSearchBIN.Click += new System.EventHandler(this.btnSearchBIN_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Enabled = false;
            this.btnGenerate.Location = new System.Drawing.Point(9, 218);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(81, 24);
            this.btnGenerate.TabIndex = 6;
            this.btnGenerate.Text = "Report";
            this.btnGenerate.Values.ExtraText = "";
            this.btnGenerate.Values.Image = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedNormal = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedPressed = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedTracking = null;
            this.btnGenerate.Values.Text = "Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(178, 218);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnDelete.Size = new System.Drawing.Size(81, 24);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Values.ExtraText = "";
            this.btnDelete.Values.Image = null;
            this.btnDelete.Values.ImageStates.ImageCheckedNormal = null;
            this.btnDelete.Values.ImageStates.ImageCheckedPressed = null;
            this.btnDelete.Values.ImageStates.ImageCheckedTracking = null;
            this.btnDelete.Values.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(264, 218);
            this.btnSave.Name = "btnSave";
            this.btnSave.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSave.Size = new System.Drawing.Size(81, 24);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.Values.ExtraText = "";
            this.btnSave.Values.Image = null;
            this.btnSave.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSave.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSave.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSave.Values.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(150, 50);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(86, 20);
            this.dtpFrom.TabIndex = 10;
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(262, 50);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(87, 20);
            this.dtpTo.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(241, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "to";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 111);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "BIN";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(350, 218);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(81, 24);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(3, 88);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(435, 124);
            this.containerWithShadow2.TabIndex = 0;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(3, 5);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(435, 82);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(76, 104);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(140, 22);
            this.bin1.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 184);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Reg. No.";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // txtRegNo
            // 
            this.txtRegNo.Location = new System.Drawing.Point(76, 179);
            this.txtRegNo.Name = "txtRegNo";
            this.txtRegNo.ReadOnly = true;
            this.txtRegNo.Size = new System.Drawing.Size(140, 20);
            this.txtRegNo.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(279, 184);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Reg. Date";
            this.label8.Click += new System.EventHandler(this.label7_Click);
            // 
            // dtpRegDate
            // 
            this.dtpRegDate.Enabled = false;
            this.dtpRegDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpRegDate.Location = new System.Drawing.Point(341, 179);
            this.dtpRegDate.Name = "dtpRegDate";
            this.dtpRegDate.Size = new System.Drawing.Size(86, 20);
            this.dtpRegDate.TabIndex = 10;
            // 
            // frmTagging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 254);
            this.ControlBox = false;
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpRegDate);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnSearchBIN);
            this.Controls.Add(this.txtRegNo);
            this.Controls.Add(this.txtOwner);
            this.Controls.Add(this.txtBnsName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bin1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbOrdinance);
            this.Controls.Add(this.chkReport);
            this.Controls.Add(this.chkTagging);
            this.Controls.Add(this.containerWithShadow2);
            this.Controls.Add(this.containerWithShadow1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTagging";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Special Ordinance";
            this.Load += new System.EventHandler(this.frmTagging_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkTagging;
        private System.Windows.Forms.CheckBox chkReport;
        private System.Windows.Forms.ComboBox cmbOrdinance;
        private System.Windows.Forms.Label label1;
        private Amellar.Common.BIN.BIN bin1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBnsName;
        private System.Windows.Forms.TextBox txtOwner;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearchBIN;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDelete;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSave;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtRegNo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtpRegDate;
        
    }
}

