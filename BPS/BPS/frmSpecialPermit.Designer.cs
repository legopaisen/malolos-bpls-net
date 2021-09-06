namespace BPLSBilling
{
    partial class frmSpecialPermit
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
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.txtSPLNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPermitTo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpExpDate = new System.Windows.Forms.DateTimePicker();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtBinYear = new System.Windows.Forms.TextBox();
            this.txtBinSeries = new System.Windows.Forms.TextBox();
            this.bin1 = new Amellar.Common.BIN.BIN();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "BIN:";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(216, 17);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparklePurple;
            this.btnSearch.Size = new System.Drawing.Size(78, 30);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Business Name:";
            // 
            // txtBnsName
            // 
            this.txtBnsName.Location = new System.Drawing.Point(113, 74);
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(388, 20);
            this.txtBnsName.TabIndex = 6;
            // 
            // txtSPLNo
            // 
            this.txtSPLNo.Location = new System.Drawing.Point(367, 22);
            this.txtSPLNo.Name = "txtSPLNo";
            this.txtSPLNo.ReadOnly = true;
            this.txtSPLNo.Size = new System.Drawing.Size(134, 20);
            this.txtSPLNo.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(317, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "SP No.:";
            // 
            // txtPermitTo
            // 
            this.txtPermitTo.Location = new System.Drawing.Point(113, 100);
            this.txtPermitTo.Name = "txtPermitTo";
            this.txtPermitTo.Size = new System.Drawing.Size(388, 20);
            this.txtPermitTo.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Permit to:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Expiration date:";
            // 
            // dtpExpDate
            // 
            this.dtpExpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpExpDate.Location = new System.Drawing.Point(113, 125);
            this.dtpExpDate.Name = "dtpExpDate";
            this.dtpExpDate.Size = new System.Drawing.Size(108, 20);
            this.dtpExpDate.TabIndex = 12;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Enabled = false;
            this.btnGenerate.Location = new System.Drawing.Point(349, 194);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparklePurple;
            this.btnGenerate.Size = new System.Drawing.Size(78, 30);
            this.btnGenerate.TabIndex = 13;
            this.btnGenerate.Values.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(432, 194);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparklePurple;
            this.btnClose.Size = new System.Drawing.Size(78, 30);
            this.btnClose.TabIndex = 14;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtBinYear
            // 
            this.txtBinYear.Location = new System.Drawing.Point(107, 22);
            this.txtBinYear.Name = "txtBinYear";
            this.txtBinYear.Size = new System.Drawing.Size(34, 20);
            this.txtBinYear.TabIndex = 16;
            this.txtBinYear.TextChanged += new System.EventHandler(this.txtBinYear_TextChanged);
            // 
            // txtBinSeries
            // 
            this.txtBinSeries.Location = new System.Drawing.Point(146, 22);
            this.txtBinSeries.Name = "txtBinSeries";
            this.txtBinSeries.Size = new System.Drawing.Size(55, 20);
            this.txtBinSeries.TabIndex = 17;
            this.txtBinSeries.Leave += new System.EventHandler(this.txtBinSeries_Leave);
            // 
            // bin1
            // 
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.Location = new System.Drawing.Point(56, 22);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(49, 23);
            this.bin1.TabIndex = 15;
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(9, 59);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(502, 129);
            this.containerWithShadow2.TabIndex = 4;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(8, 11);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(502, 47);
            this.containerWithShadow1.TabIndex = 1;
            // 
            // frmSpecialPermit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 242);
            this.Controls.Add(this.txtBinSeries);
            this.Controls.Add(this.txtBinYear);
            this.Controls.Add(this.bin1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.dtpExpDate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPermitTo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSPLNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBnsName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.containerWithShadow2);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.containerWithShadow1);
            this.Name = "frmSpecialPermit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Special Permit";
            this.Load += new System.EventHandler(this.frmSpecialPermit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.Label label1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBnsName;
        private System.Windows.Forms.TextBox txtSPLNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPermitTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpExpDate;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private Amellar.Common.BIN.BIN bin1;
        private System.Windows.Forms.TextBox txtBinYear;
        private System.Windows.Forms.TextBox txtBinSeries;
    }
}