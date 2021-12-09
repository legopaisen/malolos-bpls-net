namespace Amellar.Modules.HealthPermit
{
    partial class frmCertificateAndPermit
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
            this.txtBNSOwner = new System.Windows.Forms.TextBox();
            this.txtBNSAdd = new System.Windows.Forms.TextBox();
            this.txtBNSName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPrint = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnOK = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSearchTmp = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtTmpBin = new System.Windows.Forms.TextBox();
            this.rdoRenewal = new System.Windows.Forms.RadioButton();
            this.bin1 = new BIN.BIN();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.rdoNew = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtDTIBnsName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPrint2 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtBNSOwner
            // 
            this.txtBNSOwner.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBNSOwner.Location = new System.Drawing.Point(79, 70);
            this.txtBNSOwner.Name = "txtBNSOwner";
            this.txtBNSOwner.ReadOnly = true;
            this.txtBNSOwner.Size = new System.Drawing.Size(275, 20);
            this.txtBNSOwner.TabIndex = 20;
            // 
            // txtBNSAdd
            // 
            this.txtBNSAdd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBNSAdd.Location = new System.Drawing.Point(79, 45);
            this.txtBNSAdd.Name = "txtBNSAdd";
            this.txtBNSAdd.ReadOnly = true;
            this.txtBNSAdd.Size = new System.Drawing.Size(275, 20);
            this.txtBNSAdd.TabIndex = 19;
            // 
            // txtBNSName
            // 
            this.txtBNSName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBNSName.Location = new System.Drawing.Point(79, 19);
            this.txtBNSName.Name = "txtBNSName";
            this.txtBNSName.ReadOnly = true;
            this.txtBNSName.Size = new System.Drawing.Size(275, 20);
            this.txtBNSName.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Owner";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "BNS Add";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "BNS Name";
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(220, 231);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint.Size = new System.Drawing.Size(68, 24);
            this.btnPrint.TabIndex = 27;
            this.btnPrint.Text = "&Print";
            this.btnPrint.Values.ExtraText = "";
            this.btnPrint.Values.Image = null;
            this.btnPrint.Values.ImageStates.ImageCheckedNormal = null;
            this.btnPrint.Values.ImageStates.ImageCheckedPressed = null;
            this.btnPrint.Values.ImageStates.ImageCheckedTracking = null;
            this.btnPrint.Values.Text = "&Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(294, 231);
            this.btnOK.Name = "btnOK";
            this.btnOK.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOK.Size = new System.Drawing.Size(68, 24);
            this.btnOK.TabIndex = 27;
            this.btnOK.Text = "&Close";
            this.btnOK.Values.ExtraText = "";
            this.btnOK.Values.Image = null;
            this.btnOK.Values.ImageStates.ImageCheckedNormal = null;
            this.btnOK.Values.ImageStates.ImageCheckedPressed = null;
            this.btnOK.Values.ImageStates.ImageCheckedTracking = null;
            this.btnOK.Values.Text = "&Close";
            this.btnOK.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSearchTmp);
            this.groupBox1.Controls.Add(this.txtTmpBin);
            this.groupBox1.Controls.Add(this.rdoRenewal);
            this.groupBox1.Controls.Add(this.bin1);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.rdoNew);
            this.groupBox1.Location = new System.Drawing.Point(8, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(368, 87);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btnSearchTmp
            // 
            this.btnSearchTmp.Enabled = false;
            this.btnSearchTmp.Location = new System.Drawing.Point(240, 51);
            this.btnSearchTmp.Name = "btnSearchTmp";
            this.btnSearchTmp.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearchTmp.Size = new System.Drawing.Size(114, 24);
            this.btnSearchTmp.TabIndex = 42;
            this.btnSearchTmp.Text = "Search";
            this.btnSearchTmp.Values.ExtraText = "";
            this.btnSearchTmp.Values.Image = null;
            this.btnSearchTmp.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearchTmp.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearchTmp.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearchTmp.Values.Text = "Search";
            this.btnSearchTmp.Click += new System.EventHandler(this.btnSearchTmp_Click);
            // 
            // txtTmpBin
            // 
            this.txtTmpBin.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTmpBin.Location = new System.Drawing.Point(93, 52);
            this.txtTmpBin.MaxLength = 20;
            this.txtTmpBin.Name = "txtTmpBin";
            this.txtTmpBin.ReadOnly = true;
            this.txtTmpBin.Size = new System.Drawing.Size(138, 20);
            this.txtTmpBin.TabIndex = 37;
            // 
            // rdoRenewal
            // 
            this.rdoRenewal.AutoSize = true;
            this.rdoRenewal.Location = new System.Drawing.Point(8, 25);
            this.rdoRenewal.Name = "rdoRenewal";
            this.rdoRenewal.Size = new System.Drawing.Size(79, 17);
            this.rdoRenewal.TabIndex = 40;
            this.rdoRenewal.TabStop = true;
            this.rdoRenewal.Text = "RENEWAL";
            this.rdoRenewal.UseVisualStyleBackColor = true;
            this.rdoRenewal.CheckedChanged += new System.EventHandler(this.rdoRenewal_CheckedChanged);
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(93, 24);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(138, 20);
            this.bin1.TabIndex = 39;
            // 
            // btnSearch
            // 
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(240, 22);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(114, 24);
            this.btnSearch.TabIndex = 38;
            this.btnSearch.Text = "Search";
            this.btnSearch.Values.ExtraText = "";
            this.btnSearch.Values.Image = null;
            this.btnSearch.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearch.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearch.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click_1);
            // 
            // rdoNew
            // 
            this.rdoNew.AutoSize = true;
            this.rdoNew.Location = new System.Drawing.Point(8, 53);
            this.rdoNew.Name = "rdoNew";
            this.rdoNew.Size = new System.Drawing.Size(51, 17);
            this.rdoNew.TabIndex = 41;
            this.rdoNew.TabStop = true;
            this.rdoNew.Text = "NEW";
            this.rdoNew.UseVisualStyleBackColor = true;
            this.rdoNew.CheckedChanged += new System.EventHandler(this.rdoNew_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtDTIBnsName);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtBNSName);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtBNSOwner);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtBNSAdd);
            this.groupBox2.Location = new System.Drawing.Point(8, 97);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(368, 128);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // txtDTIBnsName
            // 
            this.txtDTIBnsName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtDTIBnsName.Location = new System.Drawing.Point(79, 96);
            this.txtDTIBnsName.Name = "txtDTIBnsName";
            this.txtDTIBnsName.ReadOnly = true;
            this.txtDTIBnsName.Size = new System.Drawing.Size(275, 20);
            this.txtDTIBnsName.TabIndex = 27;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "DTI Name";
            // 
            // btnPrint2
            // 
            this.btnPrint2.Enabled = false;
            this.btnPrint2.Location = new System.Drawing.Point(8, 231);
            this.btnPrint2.Name = "btnPrint2";
            this.btnPrint2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint2.Size = new System.Drawing.Size(150, 24);
            this.btnPrint2.TabIndex = 31;
            this.btnPrint2.Text = "Extension Bldg";
            this.btnPrint2.Values.ExtraText = "";
            this.btnPrint2.Values.Image = null;
            this.btnPrint2.Values.ImageStates.ImageCheckedNormal = null;
            this.btnPrint2.Values.ImageStates.ImageCheckedPressed = null;
            this.btnPrint2.Values.ImageStates.ImageCheckedTracking = null;
            this.btnPrint2.Values.Text = "Extension Bldg";
            this.btnPrint2.Click += new System.EventHandler(this.btnPrint2_Click);
            // 
            // frmCertificateAndPermit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(382, 266);
            this.ControlBox = false;
            this.Controls.Add(this.btnPrint2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmCertificateAndPermit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sanitary Permit";
            this.Load += new System.EventHandler(this.frmCertificateAndPermit_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtBNSOwner;
        private System.Windows.Forms.TextBox txtBNSAdd;
        private System.Windows.Forms.TextBox txtBNSName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPrint;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearchTmp;
        private System.Windows.Forms.TextBox txtTmpBin;
        private System.Windows.Forms.RadioButton rdoRenewal;
        private BIN.BIN bin1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private System.Windows.Forms.RadioButton rdoNew;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtDTIBnsName;
        private System.Windows.Forms.Label label1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPrint2;
    }
}