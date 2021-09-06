namespace TreasurersModule
{
    partial class frmPenaltyTagging
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnTagUnTag = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtBnsAddress = new System.Windows.Forms.TextBox();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bin1 = new BIN.BIN();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.frameWithShadow1 = new Amellar.Common.SOA.FrameWithShadow();
            this.txtMemoranda = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvTagged = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbBrgy = new System.Windows.Forms.ComboBox();
            this.rdoBIN = new System.Windows.Forms.RadioButton();
            this.rdoBrgy = new System.Windows.Forms.RadioButton();
            this.btnPrint = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnApprove = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.chkTagging = new System.Windows.Forms.CheckBox();
            this.chkTagged = new System.Windows.Forms.CheckBox();
            this.Column5 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTagged)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(244, 328);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 23);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "Clear &Fields";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnClose
            // 
            this.btnClose.CausesValidation = false;
            this.btnClose.Location = new System.Drawing.Point(613, 449);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnTagUnTag
            // 
            this.btnTagUnTag.Enabled = false;
            this.btnTagUnTag.Location = new System.Drawing.Point(532, 449);
            this.btnTagUnTag.Name = "btnTagUnTag";
            this.btnTagUnTag.Size = new System.Drawing.Size(75, 23);
            this.btnTagUnTag.TabIndex = 14;
            this.btnTagUnTag.Text = "&Tag";
            this.btnTagUnTag.UseVisualStyleBackColor = true;
            this.btnTagUnTag.Click += new System.EventHandler(this.btnTagUnTag_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(244, 299);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search for &BIN";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtBnsAddress
            // 
            this.txtBnsAddress.Location = new System.Drawing.Point(94, 385);
            this.txtBnsAddress.Multiline = true;
            this.txtBnsAddress.Name = "txtBnsAddress";
            this.txtBnsAddress.ReadOnly = true;
            this.txtBnsAddress.Size = new System.Drawing.Size(594, 20);
            this.txtBnsAddress.TabIndex = 10;
            // 
            // txtBnsName
            // 
            this.txtBnsName.AcceptsReturn = true;
            this.txtBnsName.Location = new System.Drawing.Point(94, 359);
            this.txtBnsName.Multiline = true;
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(594, 20);
            this.txtBnsName.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 364);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Bns Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(75, 299);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(141, 21);
            this.bin1.TabIndex = 3;
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4,
            this.Column1,
            this.Column2});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvList.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvList.Location = new System.Drawing.Point(27, 31);
            this.dgvList.MultiSelect = false;
            this.dgvList.Name = "dgvList";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(314, 259);
            this.dgvList.TabIndex = 1;
            this.dgvList.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_RowEnter);
            // 
            // frameWithShadow1
            // 
            this.frameWithShadow1.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow1.Location = new System.Drawing.Point(10, 8);
            this.frameWithShadow1.Name = "frameWithShadow1";
            this.frameWithShadow1.Size = new System.Drawing.Size(691, 474);
            this.frameWithShadow1.TabIndex = 10;
            // 
            // txtMemoranda
            // 
            this.txtMemoranda.Location = new System.Drawing.Point(94, 409);
            this.txtMemoranda.Multiline = true;
            this.txtMemoranda.Name = "txtMemoranda";
            this.txtMemoranda.Size = new System.Drawing.Size(594, 37);
            this.txtMemoranda.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 411);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Memoranda";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dgvTagged
            // 
            this.dgvTagged.AllowUserToAddRows = false;
            this.dgvTagged.AllowUserToDeleteRows = false;
            this.dgvTagged.AllowUserToResizeColumns = false;
            this.dgvTagged.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTagged.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvTagged.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTagged.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.Column3});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTagged.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvTagged.Location = new System.Drawing.Point(347, 31);
            this.dgvTagged.MultiSelect = false;
            this.dgvTagged.Name = "dgvTagged";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTagged.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvTagged.RowHeadersVisible = false;
            this.dgvTagged.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTagged.Size = new System.Drawing.Size(341, 320);
            this.dgvTagged.TabIndex = 8;
            this.dgvTagged.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_RowEnter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 387);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Bns Add";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(347, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(341, 16);
            this.label4.TabIndex = 33;
            this.label4.Text = "Tagged Businesses";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbBrgy
            // 
            this.cmbBrgy.FormattingEnabled = true;
            this.cmbBrgy.Location = new System.Drawing.Point(75, 326);
            this.cmbBrgy.Name = "cmbBrgy";
            this.cmbBrgy.Size = new System.Drawing.Size(165, 21);
            this.cmbBrgy.TabIndex = 6;
            this.cmbBrgy.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbBrgy_KeyPress);
            // 
            // rdoBIN
            // 
            this.rdoBIN.AutoSize = true;
            this.rdoBIN.Location = new System.Drawing.Point(28, 301);
            this.rdoBIN.Name = "rdoBIN";
            this.rdoBIN.Size = new System.Drawing.Size(43, 17);
            this.rdoBIN.TabIndex = 2;
            this.rdoBIN.TabStop = true;
            this.rdoBIN.Text = "BIN";
            this.rdoBIN.UseVisualStyleBackColor = true;
            this.rdoBIN.CheckedChanged += new System.EventHandler(this.rdoBIN_CheckedChanged);
            // 
            // rdoBrgy
            // 
            this.rdoBrgy.AutoSize = true;
            this.rdoBrgy.Location = new System.Drawing.Point(28, 329);
            this.rdoBrgy.Name = "rdoBrgy";
            this.rdoBrgy.Size = new System.Drawing.Size(49, 17);
            this.rdoBrgy.TabIndex = 5;
            this.rdoBrgy.TabStop = true;
            this.rdoBrgy.Text = "Brgy.";
            this.rdoBrgy.UseVisualStyleBackColor = true;
            this.rdoBrgy.CheckedChanged += new System.EventHandler(this.rdoBIN_CheckedChanged);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(294, 449);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 12;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 454);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "Note: Only the tagged businesses will appear in report.";
            // 
            // btnApprove
            // 
            this.btnApprove.Enabled = false;
            this.btnApprove.Location = new System.Drawing.Point(451, 449);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(75, 23);
            this.btnApprove.TabIndex = 13;
            this.btnApprove.Text = "&Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 487);
            this.splitter1.TabIndex = 38;
            this.splitter1.TabStop = false;
            // 
            // chkTagging
            // 
            this.chkTagging.AutoSize = true;
            this.chkTagging.Location = new System.Drawing.Point(31, 36);
            this.chkTagging.Name = "chkTagging";
            this.chkTagging.Size = new System.Drawing.Size(15, 14);
            this.chkTagging.TabIndex = 39;
            this.chkTagging.UseVisualStyleBackColor = true;
            this.chkTagging.CheckedChanged += new System.EventHandler(this.chkTagging_CheckedChanged);
            // 
            // chkTagged
            // 
            this.chkTagged.AutoSize = true;
            this.chkTagged.Location = new System.Drawing.Point(351, 36);
            this.chkTagged.Name = "chkTagged";
            this.chkTagged.Size = new System.Drawing.Size(15, 14);
            this.chkTagged.TabIndex = 40;
            this.chkTagged.UseVisualStyleBackColor = true;
            this.chkTagged.CheckedChanged += new System.EventHandler(this.chkTagging_CheckedChanged);
            // 
            // Column5
            // 
            this.Column5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Column5.HeaderText = "";
            this.Column5.Name = "Column5";
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column5.Width = 20;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "BIN";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 130;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Tax Year";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 80;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Status";
            this.Column3.Name = "Column3";
            this.Column3.Width = 80;
            // 
            // Column4
            // 
            this.Column4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Column4.HeaderText = "";
            this.Column4.Name = "Column4";
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column4.Width = 20;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "BIN";
            this.Column1.Name = "Column1";
            this.Column1.Width = 170;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Tax Year";
            this.Column2.Name = "Column2";
            this.Column2.Width = 90;
            // 
            // frmPenaltyTagging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 487);
            this.Controls.Add(this.chkTagged);
            this.Controls.Add(this.chkTagging);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.cmbBrgy);
            this.Controls.Add(this.bin1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rdoBrgy);
            this.Controls.Add(this.rdoBIN);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMemoranda);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnTagUnTag);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtBnsAddress);
            this.Controls.Add(this.txtBnsName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvTagged);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.frameWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPenaltyTagging";
            this.Text = "Waive Surcharge/Penalty Tagging";
            this.Load += new System.EventHandler(this.frmPenaltyTagging_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTagged)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClear;
        public System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Button btnTagUnTag;
        private System.Windows.Forms.Button btnSearch;
        public System.Windows.Forms.TextBox txtBnsAddress;
        public System.Windows.Forms.TextBox txtBnsName;
        private System.Windows.Forms.Label label2;
        public BIN.BIN bin1;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow1;
        public System.Windows.Forms.TextBox txtMemoranda;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbBrgy;
        private System.Windows.Forms.RadioButton rdoBIN;
        private System.Windows.Forms.RadioButton rdoBrgy;
        public System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.CheckBox chkTagging;
        private System.Windows.Forms.CheckBox chkTagged;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.DataGridView dgvTagged;
    }
}