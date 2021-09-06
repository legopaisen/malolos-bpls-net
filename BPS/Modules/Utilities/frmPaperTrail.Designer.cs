namespace Amellar.Modules.Utilities
{
    partial class frmPaperTrail
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
            this.bin1 = new BIN.BIN();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearchBIN = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.txtBnsAdd = new System.Windows.Forms.TextBox();
            this.txtTaxPayrName = new System.Windows.Forms.TextBox();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.rbApp = new System.Windows.Forms.RadioButton();
            this.GB1 = new System.Windows.Forms.GroupBox();
            this.rbTres = new System.Windows.Forms.RadioButton();
            this.rbPermit = new System.Windows.Forms.RadioButton();
            this.rbPymnt = new System.Windows.Forms.RadioButton();
            this.rbSOA = new System.Windows.Forms.RadioButton();
            this.rbBill = new System.Windows.Forms.RadioButton();
            this.dgView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnFind = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClear = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.GB1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgView)).BeginInit();
            this.SuspendLayout();
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(55, 24);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(138, 20);
            this.bin1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "BIN";
            // 
            // btnSearchBIN
            // 
            this.btnSearchBIN.Location = new System.Drawing.Point(199, 22);
            this.btnSearchBIN.Name = "btnSearchBIN";
            this.btnSearchBIN.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearchBIN.Size = new System.Drawing.Size(81, 24);
            this.btnSearchBIN.TabIndex = 2;
            this.btnSearchBIN.Values.Text = "&Search Bin";
            this.btnSearchBIN.Click += new System.EventHandler(this.btnSearchBIN_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Business Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Business Address";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Tax Payer Name";
            // 
            // txtBnsName
            // 
            this.txtBnsName.Location = new System.Drawing.Point(117, 58);
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(271, 20);
            this.txtBnsName.TabIndex = 4;
            // 
            // txtBnsAdd
            // 
            this.txtBnsAdd.Location = new System.Drawing.Point(117, 84);
            this.txtBnsAdd.Name = "txtBnsAdd";
            this.txtBnsAdd.ReadOnly = true;
            this.txtBnsAdd.Size = new System.Drawing.Size(271, 20);
            this.txtBnsAdd.TabIndex = 5;
            // 
            // txtTaxPayrName
            // 
            this.txtTaxPayrName.Location = new System.Drawing.Point(117, 110);
            this.txtTaxPayrName.Name = "txtTaxPayrName";
            this.txtTaxPayrName.ReadOnly = true;
            this.txtTaxPayrName.Size = new System.Drawing.Size(271, 20);
            this.txtTaxPayrName.TabIndex = 6;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(4, 4);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(446, 417);
            this.containerWithShadow1.TabIndex = 103;
            // 
            // rbApp
            // 
            this.rbApp.AutoSize = true;
            this.rbApp.Checked = true;
            this.rbApp.Location = new System.Drawing.Point(21, 19);
            this.rbApp.Name = "rbApp";
            this.rbApp.Size = new System.Drawing.Size(77, 17);
            this.rbApp.TabIndex = 7;
            this.rbApp.TabStop = true;
            this.rbApp.Text = "Application";
            this.rbApp.UseVisualStyleBackColor = true;
            this.rbApp.Click += new System.EventHandler(this.rbApp_Click);
            // 
            // GB1
            // 
            this.GB1.Controls.Add(this.rbTres);
            this.GB1.Controls.Add(this.rbPermit);
            this.GB1.Controls.Add(this.rbPymnt);
            this.GB1.Controls.Add(this.rbSOA);
            this.GB1.Controls.Add(this.rbBill);
            this.GB1.Controls.Add(this.rbApp);
            this.GB1.Location = new System.Drawing.Point(18, 136);
            this.GB1.Name = "GB1";
            this.GB1.Size = new System.Drawing.Size(370, 71);
            this.GB1.TabIndex = 105;
            this.GB1.TabStop = false;
            // 
            // rbTres
            // 
            this.rbTres.AutoSize = true;
            this.rbTres.Location = new System.Drawing.Point(234, 42);
            this.rbTres.Name = "rbTres";
            this.rbTres.Size = new System.Drawing.Size(115, 17);
            this.rbTres.TabIndex = 12;
            this.rbTres.TabStop = true;
            this.rbTres.Text = "Treasurer\'s Module";
            this.rbTres.UseVisualStyleBackColor = true;
            this.rbTres.Click += new System.EventHandler(this.rbTres_Click);
            // 
            // rbPermit
            // 
            this.rbPermit.AutoSize = true;
            this.rbPermit.Location = new System.Drawing.Point(234, 19);
            this.rbPermit.Name = "rbPermit";
            this.rbPermit.Size = new System.Drawing.Size(54, 17);
            this.rbPermit.TabIndex = 11;
            this.rbPermit.TabStop = true;
            this.rbPermit.Text = "Permit";
            this.rbPermit.UseVisualStyleBackColor = true;
            this.rbPermit.Click += new System.EventHandler(this.rbPermit_Click);
            // 
            // rbPymnt
            // 
            this.rbPymnt.AutoSize = true;
            this.rbPymnt.Location = new System.Drawing.Point(132, 42);
            this.rbPymnt.Name = "rbPymnt";
            this.rbPymnt.Size = new System.Drawing.Size(66, 17);
            this.rbPymnt.TabIndex = 10;
            this.rbPymnt.TabStop = true;
            this.rbPymnt.Text = "Payment";
            this.rbPymnt.UseVisualStyleBackColor = true;
            this.rbPymnt.Click += new System.EventHandler(this.rbPymnt_Click);
            // 
            // rbSOA
            // 
            this.rbSOA.AutoSize = true;
            this.rbSOA.Location = new System.Drawing.Point(132, 19);
            this.rbSOA.Name = "rbSOA";
            this.rbSOA.Size = new System.Drawing.Size(47, 17);
            this.rbSOA.TabIndex = 9;
            this.rbSOA.TabStop = true;
            this.rbSOA.Text = "SOA";
            this.rbSOA.UseVisualStyleBackColor = true;
            this.rbSOA.Click += new System.EventHandler(this.rbSOA_Click);
            // 
            // rbBill
            // 
            this.rbBill.AutoSize = true;
            this.rbBill.Location = new System.Drawing.Point(21, 42);
            this.rbBill.Name = "rbBill";
            this.rbBill.Size = new System.Drawing.Size(52, 17);
            this.rbBill.TabIndex = 8;
            this.rbBill.TabStop = true;
            this.rbBill.Text = "Billing";
            this.rbBill.UseVisualStyleBackColor = true;
            this.rbBill.Click += new System.EventHandler(this.rbBill_Click);
            // 
            // dgView
            // 
            this.dgView.AllowUserToAddRows = false;
            this.dgView.AllowUserToDeleteRows = false;
            this.dgView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.dgView.Location = new System.Drawing.Point(16, 213);
            this.dgView.Name = "dgView";
            this.dgView.ReadOnly = true;
            this.dgView.RowHeadersVisible = false;
            this.dgView.Size = new System.Drawing.Size(424, 161);
            this.dgView.TabIndex = 13;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Transaction Code";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 120;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "User Code";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 80;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Work Station";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Date/Time";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(272, 380);
            this.btnFind.Name = "btnFind";
            this.btnFind.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnFind.Size = new System.Drawing.Size(81, 24);
            this.btnFind.TabIndex = 14;
            this.btnFind.Values.Text = "&Find";
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(359, 380);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(81, 24);
            this.btnClose.TabIndex = 15;
            this.btnClose.Values.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(286, 22);
            this.btnClear.Name = "btnClear";
            this.btnClear.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClear.Size = new System.Drawing.Size(81, 24);
            this.btnClear.TabIndex = 3;
            this.btnClear.Values.Text = "&Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // frmPaperTrail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 421);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.dgView);
            this.Controls.Add(this.GB1);
            this.Controls.Add(this.txtTaxPayrName);
            this.Controls.Add(this.txtBnsAdd);
            this.Controls.Add(this.txtBnsName);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSearchBIN);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bin1);
            this.Controls.Add(this.containerWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmPaperTrail";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Paper Trail";
            this.Load += new System.EventHandler(this.frmPaperTrail_Load);
            this.GB1.ResumeLayout(false);
            this.GB1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BIN.BIN bin1;
        private System.Windows.Forms.Label label1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearchBIN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBnsName;
        private System.Windows.Forms.TextBox txtBnsAdd;
        private System.Windows.Forms.TextBox txtTaxPayrName;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.RadioButton rbApp;
        private System.Windows.Forms.GroupBox GB1;
        private System.Windows.Forms.RadioButton rbTres;
        private System.Windows.Forms.RadioButton rbPermit;
        private System.Windows.Forms.RadioButton rbPymnt;
        private System.Windows.Forms.RadioButton rbSOA;
        private System.Windows.Forms.RadioButton rbBill;
        private System.Windows.Forms.DataGridView dgView;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnFind;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClear;
    }
}