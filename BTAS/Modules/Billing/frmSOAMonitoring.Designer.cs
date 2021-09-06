namespace Amellar.BPLS.Billing
{
    partial class frmSOAMonitoring
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTaxYear = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnApprove = new System.Windows.Forms.Button();
            this.btnViewSOA = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnListReturn = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtOwnAddress = new System.Windows.Forms.TextBox();
            this.txtBnsAddress = new System.Windows.Forms.TextBox();
            this.txtOwnName = new System.Windows.Forms.TextBox();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.btnListApproved = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.bin1 = new BIN.BIN();
            this.frameWithShadow1 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow2 = new Amellar.Common.SOA.FrameWithShadow();
            this.BIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrDue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AssessedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ownercode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(505, 357);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "Tax Year";
            // 
            // txtTaxYear
            // 
            this.txtTaxYear.Location = new System.Drawing.Point(561, 353);
            this.txtTaxYear.Name = "txtTaxYear";
            this.txtTaxYear.ReadOnly = true;
            this.txtTaxYear.Size = new System.Drawing.Size(61, 20);
            this.txtTaxYear.TabIndex = 34;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(333, 350);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 29;
            this.btnClear.Text = "Clear Fields";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnClose
            // 
            this.btnClose.CausesValidation = false;
            this.btnClose.Location = new System.Drawing.Point(642, 466);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 28;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.CausesValidation = false;
            this.btnReturn.Location = new System.Drawing.Point(176, 466);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(75, 23);
            this.btnReturn.TabIndex = 30;
            this.btnReturn.Text = "Return";
            this.btnReturn.UseVisualStyleBackColor = true;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(100, 466);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(75, 23);
            this.btnApprove.TabIndex = 32;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // btnViewSOA
            // 
            this.btnViewSOA.Location = new System.Drawing.Point(24, 466);
            this.btnViewSOA.Name = "btnViewSOA";
            this.btnViewSOA.Size = new System.Drawing.Size(75, 23);
            this.btnViewSOA.TabIndex = 31;
            this.btnViewSOA.Text = "View SOA";
            this.btnViewSOA.UseVisualStyleBackColor = true;
            this.btnViewSOA.Click += new System.EventHandler(this.btnViewSOA_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(414, 350);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(85, 23);
            this.btnRefresh.TabIndex = 25;
            this.btnRefresh.Text = "Refresh List";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnListReturn
            // 
            this.btnListReturn.Location = new System.Drawing.Point(546, 466);
            this.btnListReturn.Name = "btnListReturn";
            this.btnListReturn.Size = new System.Drawing.Size(90, 23);
            this.btnListReturn.TabIndex = 26;
            this.btnListReturn.Text = "List of Returns";
            this.btnListReturn.UseVisualStyleBackColor = true;
            this.btnListReturn.Visible = false;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(239, 350);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 23);
            this.btnSearch.TabIndex = 27;
            this.btnSearch.Text = "Search for BIN";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtOwnAddress
            // 
            this.txtOwnAddress.Location = new System.Drawing.Point(419, 413);
            this.txtOwnAddress.Multiline = true;
            this.txtOwnAddress.Name = "txtOwnAddress";
            this.txtOwnAddress.ReadOnly = true;
            this.txtOwnAddress.Size = new System.Drawing.Size(297, 20);
            this.txtOwnAddress.TabIndex = 21;
            // 
            // txtBnsAddress
            // 
            this.txtBnsAddress.Location = new System.Drawing.Point(419, 387);
            this.txtBnsAddress.Multiline = true;
            this.txtBnsAddress.Name = "txtBnsAddress";
            this.txtBnsAddress.ReadOnly = true;
            this.txtBnsAddress.Size = new System.Drawing.Size(297, 20);
            this.txtBnsAddress.TabIndex = 24;
            // 
            // txtOwnName
            // 
            this.txtOwnName.Location = new System.Drawing.Point(91, 413);
            this.txtOwnName.Multiline = true;
            this.txtOwnName.Name = "txtOwnName";
            this.txtOwnName.ReadOnly = true;
            this.txtOwnName.Size = new System.Drawing.Size(228, 20);
            this.txtOwnName.TabIndex = 22;
            // 
            // txtBnsName
            // 
            this.txtBnsName.AcceptsReturn = true;
            this.txtBnsName.Location = new System.Drawing.Point(91, 387);
            this.txtBnsName.Multiline = true;
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(228, 20);
            this.txtBnsName.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 416);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Own Name";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 390);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Bns Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 360);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "BIN";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            this.BIN,
            this.CurrDue,
            this.AssessedBy,
            this.DateTime,
            this.TaxYear,
            this.ownercode});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvList.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvList.Location = new System.Drawing.Point(24, 21);
            this.dgvList.Name = "dgvList";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(692, 323);
            this.dgvList.TabIndex = 16;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // btnListApproved
            // 
            this.btnListApproved.Location = new System.Drawing.Point(444, 466);
            this.btnListApproved.Name = "btnListApproved";
            this.btnListApproved.Size = new System.Drawing.Size(96, 23);
            this.btnListApproved.TabIndex = 41;
            this.btnListApproved.Text = "List of Approved";
            this.btnListApproved.UseVisualStyleBackColor = true;
            this.btnListApproved.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(347, 390);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 42;
            this.label5.Text = "Bns Address";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(334, 416);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 43;
            this.label6.Text = "Owner Address";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(91, 352);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(141, 21);
            this.bin1.TabIndex = 17;
            // 
            // frameWithShadow1
            // 
            this.frameWithShadow1.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow1.Location = new System.Drawing.Point(7, 7);
            this.frameWithShadow1.Name = "frameWithShadow1";
            this.frameWithShadow1.Size = new System.Drawing.Size(721, 444);
            this.frameWithShadow1.TabIndex = 15;
            // 
            // frameWithShadow2
            // 
            this.frameWithShadow2.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow2.Location = new System.Drawing.Point(7, 457);
            this.frameWithShadow2.Name = "frameWithShadow2";
            this.frameWithShadow2.Size = new System.Drawing.Size(721, 47);
            this.frameWithShadow2.TabIndex = 33;
            // 
            // BIN
            // 
            this.BIN.HeaderText = "BIN";
            this.BIN.Name = "BIN";
            this.BIN.Width = 200;
            // 
            // CurrDue
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.CurrDue.DefaultCellStyle = dataGridViewCellStyle2;
            this.CurrDue.HeaderText = "CURR. TAX DUE";
            this.CurrDue.Name = "CurrDue";
            this.CurrDue.ReadOnly = true;
            this.CurrDue.Width = 150;
            // 
            // AssessedBy
            // 
            this.AssessedBy.HeaderText = "ASSESSED BY";
            this.AssessedBy.Name = "AssessedBy";
            this.AssessedBy.ReadOnly = true;
            this.AssessedBy.Width = 120;
            // 
            // DateTime
            // 
            this.DateTime.HeaderText = "DATE BILLED";
            this.DateTime.Name = "DateTime";
            this.DateTime.ReadOnly = true;
            this.DateTime.Width = 125;
            // 
            // TaxYear
            // 
            this.TaxYear.HeaderText = "TAX YEAR";
            this.TaxYear.Name = "TaxYear";
            this.TaxYear.ReadOnly = true;
            // 
            // ownercode
            // 
            this.ownercode.HeaderText = "ownercode";
            this.ownercode.Name = "ownercode";
            this.ownercode.ReadOnly = true;
            this.ownercode.Visible = false;
            // 
            // frmSOAMonitoring
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 519);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnListApproved);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtTaxYear);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.btnViewSOA);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnListReturn);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtOwnAddress);
            this.Controls.Add(this.txtBnsAddress);
            this.Controls.Add(this.txtOwnName);
            this.Controls.Add(this.txtBnsName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bin1);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.frameWithShadow1);
            this.Controls.Add(this.frameWithShadow2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSOAMonitoring";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SOA Monitoring";
            this.Load += new System.EventHandler(this.frmSOAMonitoring_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTaxYear;
        private System.Windows.Forms.Button btnClear;
        public System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Button btnReturn;
        public System.Windows.Forms.Button btnApprove;
        public System.Windows.Forms.Button btnViewSOA;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnListReturn;
        private System.Windows.Forms.Button btnSearch;
        public System.Windows.Forms.TextBox txtOwnAddress;
        public System.Windows.Forms.TextBox txtBnsAddress;
        public System.Windows.Forms.TextBox txtOwnName;
        public System.Windows.Forms.TextBox txtBnsName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public BIN.BIN bin1;
        public System.Windows.Forms.DataGridView dgvList;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow1;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow2;
        private System.Windows.Forms.Button btnListApproved;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewTextBoxColumn BIN;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrDue;
        private System.Windows.Forms.DataGridViewTextBoxColumn AssessedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn ownercode;
    }
}