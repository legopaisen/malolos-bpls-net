namespace Amellar.Modules.CompromiseAgreement
{
    partial class frmCompromise
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCompromise));
            this.txtBnsAdd = new System.Windows.Forms.TextBox();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblTitle = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.txtOwnAdd = new System.Windows.Forms.TextBox();
            this.txtOwnName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.bin1 = new BIN.BIN();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCurrentYear = new System.Windows.Forms.TextBox();
            this.kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.containerWithShadow3 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSave = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.dgView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRefNum = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtNoPayment = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.dtpApp = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.label12 = new System.Windows.Forms.Label();
            this.txtGrandTotal = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgView)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBnsAdd
            // 
            this.txtBnsAdd.Location = new System.Drawing.Point(119, 100);
            this.txtBnsAdd.Name = "txtBnsAdd";
            this.txtBnsAdd.ReadOnly = true;
            this.txtBnsAdd.Size = new System.Drawing.Size(275, 20);
            this.txtBnsAdd.TabIndex = 85;
            // 
            // txtBnsName
            // 
            this.txtBnsName.Location = new System.Drawing.Point(119, 74);
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(275, 20);
            this.txtBnsName.TabIndex = 83;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(210, 40);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(73, 25);
            this.btnSearch.TabIndex = 81;
            this.btnSearch.Values.Text = "S&earch";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = false;
            this.lblTitle.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.lblTitle.Location = new System.Drawing.Point(7, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.lblTitle.Size = new System.Drawing.Size(560, 24);
            this.lblTitle.TabIndex = 86;
            this.lblTitle.Values.Description = "";
            this.lblTitle.Values.Heading = "Business Information";
            this.lblTitle.Values.Image = ((System.Drawing.Image)(resources.GetObject("lblTitle.Values.Image")));
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 77;
            this.label4.Text = "Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 80;
            this.label2.Text = "Business Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 78;
            this.label1.Text = "BIN";
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(6, 3);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(562, 137);
            this.containerWithShadow1.TabIndex = 76;
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(8, 146);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader1.Size = new System.Drawing.Size(559, 24);
            this.kryptonHeader1.TabIndex = 88;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Owner Information";
            this.kryptonHeader1.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader1.Values.Image")));
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(6, 143);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(562, 102);
            this.containerWithShadow2.TabIndex = 87;
            // 
            // txtOwnAdd
            // 
            this.txtOwnAdd.Location = new System.Drawing.Point(119, 205);
            this.txtOwnAdd.Name = "txtOwnAdd";
            this.txtOwnAdd.ReadOnly = true;
            this.txtOwnAdd.Size = new System.Drawing.Size(275, 20);
            this.txtOwnAdd.TabIndex = 92;
            // 
            // txtOwnName
            // 
            this.txtOwnName.Location = new System.Drawing.Point(119, 180);
            this.txtOwnName.Name = "txtOwnName";
            this.txtOwnName.ReadOnly = true;
            this.txtOwnName.Size = new System.Drawing.Size(275, 20);
            this.txtOwnName.TabIndex = 91;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 89;
            this.label5.Text = "Address";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 183);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 13);
            this.label6.TabIndex = 90;
            this.label6.Text = "Tax Payer\'s Name";
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(63, 43);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(143, 20);
            this.bin1.TabIndex = 93;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(391, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 94;
            this.label7.Text = "Current Year";
            // 
            // txtCurrentYear
            // 
            this.txtCurrentYear.Location = new System.Drawing.Point(463, 41);
            this.txtCurrentYear.Name = "txtCurrentYear";
            this.txtCurrentYear.ReadOnly = true;
            this.txtCurrentYear.Size = new System.Drawing.Size(64, 20);
            this.txtCurrentYear.TabIndex = 83;
            this.txtCurrentYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // kryptonHeader2
            // 
            this.kryptonHeader2.AutoSize = false;
            this.kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader2.Location = new System.Drawing.Point(9, 251);
            this.kryptonHeader2.Name = "kryptonHeader2";
            this.kryptonHeader2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader2.Size = new System.Drawing.Size(558, 24);
            this.kryptonHeader2.TabIndex = 96;
            this.kryptonHeader2.Values.Description = "";
            this.kryptonHeader2.Values.Heading = "Compromise Details";
            this.kryptonHeader2.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader2.Values.Image")));
            // 
            // containerWithShadow3
            // 
            this.containerWithShadow3.Location = new System.Drawing.Point(6, 248);
            this.containerWithShadow3.Name = "containerWithShadow3";
            this.containerWithShadow3.Size = new System.Drawing.Size(562, 214);
            this.containerWithShadow3.TabIndex = 95;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(494, 463);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(73, 25);
            this.btnCancel.TabIndex = 98;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(415, 463);
            this.btnSave.Name = "btnSave";
            this.btnSave.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSave.Size = new System.Drawing.Size(73, 25);
            this.btnSave.TabIndex = 97;
            this.btnSave.Values.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgView
            // 
            this.dgView.AllowUserToAddRows = false;
            this.dgView.AllowUserToDeleteRows = false;
            this.dgView.AllowUserToResizeRows = false;
            this.dgView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.dgView.Location = new System.Drawing.Point(16, 314);
            this.dgView.Name = "dgView";
            this.dgView.ReadOnly = true;
            this.dgView.RowHeadersVisible = false;
            this.dgView.Size = new System.Drawing.Size(543, 106);
            this.dgView.TabIndex = 99;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Tax Year";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Amount Due";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Surcharge";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Penalty";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Total Amount Due";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 130;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 281);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 26);
            this.label8.TabIndex = 90;
            this.label8.Text = "Reference \r\nNumber";
            // 
            // txtRefNum
            // 
            this.txtRefNum.Location = new System.Drawing.Point(74, 287);
            this.txtRefNum.Name = "txtRefNum";
            this.txtRefNum.ReadOnly = true;
            this.txtRefNum.Size = new System.Drawing.Size(91, 20);
            this.txtRefNum.TabIndex = 91;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(170, 281);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 26);
            this.label9.TabIndex = 90;
            this.label9.Text = "No. of \r\nPayments";
            // 
            // txtNoPayment
            // 
            this.txtNoPayment.Location = new System.Drawing.Point(227, 287);
            this.txtNoPayment.Name = "txtNoPayment";
            this.txtNoPayment.ReadOnly = true;
            this.txtNoPayment.Size = new System.Drawing.Size(67, 20);
            this.txtNoPayment.TabIndex = 91;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(298, 281);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 26);
            this.label10.TabIndex = 90;
            this.label10.Text = "Date of \r\nApproval";
            // 
            // dtpApp
            // 
            this.dtpApp.CustomFormat = "MM/dd/yyyy";
            this.dtpApp.Enabled = false;
            this.dtpApp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpApp.Location = new System.Drawing.Point(350, 284);
            this.dtpApp.Name = "dtpApp";
            this.dtpApp.Size = new System.Drawing.Size(77, 20);
            this.dtpApp.TabIndex = 100;
            this.dtpApp.Value = new System.DateTime(2014, 6, 26, 0, 0, 0, 0);
            this.dtpApp.ValueChanged += new System.EventHandler(this.dtpApp_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(432, 281);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 26);
            this.label11.TabIndex = 90;
            this.label11.Text = "Start of \r\nPayment";
            // 
            // dtpStart
            // 
            this.dtpStart.CustomFormat = "MM/dd/yyyy";
            this.dtpStart.Enabled = false;
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(482, 284);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(77, 20);
            this.dtpStart.TabIndex = 100;
            this.dtpStart.Value = new System.DateTime(2014, 6, 26, 0, 0, 0, 0);
            this.dtpStart.ValueChanged += new System.EventHandler(this.dtpApp_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(324, 429);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 13);
            this.label12.TabIndex = 89;
            this.label12.Text = "Grand Total";
            // 
            // txtGrandTotal
            // 
            this.txtGrandTotal.Location = new System.Drawing.Point(415, 426);
            this.txtGrandTotal.Name = "txtGrandTotal";
            this.txtGrandTotal.ReadOnly = true;
            this.txtGrandTotal.Size = new System.Drawing.Size(141, 20);
            this.txtGrandTotal.TabIndex = 92;
            this.txtGrandTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // frmCompromise
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 494);
            this.Controls.Add(this.txtRefNum);
            this.Controls.Add(this.txtGrandTotal);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.dtpApp);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtNoPayment);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.dgView);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.kryptonHeader2);
            this.Controls.Add(this.containerWithShadow3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.bin1);
            this.Controls.Add(this.txtOwnAdd);
            this.Controls.Add(this.txtOwnName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.containerWithShadow2);
            this.Controls.Add(this.txtBnsAdd);
            this.Controls.Add(this.txtCurrentYear);
            this.Controls.Add(this.txtBnsName);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.containerWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCompromise";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Compromise Agreement";
            this.Load += new System.EventHandler(this.frmCompromise_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBnsAdd;
        private System.Windows.Forms.TextBox txtBnsName;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader lblTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private System.Windows.Forms.TextBox txtOwnAdd;
        private System.Windows.Forms.TextBox txtOwnName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private BIN.BIN bin1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCurrentYear;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader2;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow3;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSave;
        private System.Windows.Forms.DataGridView dgView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtRefNum;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtNoPayment;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dtpApp;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtGrandTotal;
    }
}

