namespace Amellar.BPLS.TreasurersModule
{
    partial class frmBTaxMonitoring
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.txtOwnName = new System.Windows.Forms.TextBox();
            this.txtBnsAddress = new System.Windows.Forms.TextBox();
            this.txtOwnAddress = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnBilling = new System.Windows.Forms.Button();
            this.btnViewSOA = new System.Windows.Forms.Button();
            this.btnApprove = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.txtTaxYear = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnViewMemo = new System.Windows.Forms.Button();
            this.bin1 = new BIN.BIN();
            this.frameWithShadow1 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow2 = new Amellar.Common.SOA.FrameWithShadow();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOrNo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMemo = new System.Windows.Forms.TextBox();
            this.btnReject = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvList.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvList.Location = new System.Drawing.Point(26, 26);
            this.dgvList.Name = "dgvList";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.Size = new System.Drawing.Size(598, 249);
            this.dgvList.TabIndex = 1;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 298);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "BIN";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 401);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Bns Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 427);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Own Name";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtBnsName
            // 
            this.txtBnsName.AcceptsReturn = true;
            this.txtBnsName.Location = new System.Drawing.Point(93, 398);
            this.txtBnsName.Multiline = true;
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(228, 20);
            this.txtBnsName.TabIndex = 4;
            // 
            // txtOwnName
            // 
            this.txtOwnName.Location = new System.Drawing.Point(93, 424);
            this.txtOwnName.Multiline = true;
            this.txtOwnName.Name = "txtOwnName";
            this.txtOwnName.ReadOnly = true;
            this.txtOwnName.Size = new System.Drawing.Size(228, 20);
            this.txtOwnName.TabIndex = 4;
            // 
            // txtBnsAddress
            // 
            this.txtBnsAddress.Location = new System.Drawing.Point(327, 398);
            this.txtBnsAddress.Multiline = true;
            this.txtBnsAddress.Name = "txtBnsAddress";
            this.txtBnsAddress.ReadOnly = true;
            this.txtBnsAddress.Size = new System.Drawing.Size(297, 20);
            this.txtBnsAddress.TabIndex = 4;
            // 
            // txtOwnAddress
            // 
            this.txtOwnAddress.Location = new System.Drawing.Point(327, 424);
            this.txtOwnAddress.Multiline = true;
            this.txtOwnAddress.Name = "txtOwnAddress";
            this.txtOwnAddress.ReadOnly = true;
            this.txtOwnAddress.Size = new System.Drawing.Size(297, 20);
            this.txtOwnAddress.TabIndex = 4;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(241, 288);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 23);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search for BIN";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(335, 288);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear Fields";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnBilling
            // 
            this.btnBilling.Location = new System.Drawing.Point(468, 471);
            this.btnBilling.Name = "btnBilling";
            this.btnBilling.Size = new System.Drawing.Size(75, 23);
            this.btnBilling.TabIndex = 5;
            this.btnBilling.Text = "Billing";
            this.btnBilling.UseVisualStyleBackColor = true;
            this.btnBilling.Visible = false;
            this.btnBilling.Click += new System.EventHandler(this.btnBilling_Click);
            // 
            // btnViewSOA
            // 
            this.btnViewSOA.Location = new System.Drawing.Point(26, 471);
            this.btnViewSOA.Name = "btnViewSOA";
            this.btnViewSOA.Size = new System.Drawing.Size(75, 23);
            this.btnViewSOA.TabIndex = 5;
            this.btnViewSOA.Text = "View SOA";
            this.btnViewSOA.UseVisualStyleBackColor = true;
            this.btnViewSOA.Click += new System.EventHandler(this.btnViewSOA_Click);
            // 
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(102, 471);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(75, 23);
            this.btnApprove.TabIndex = 5;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.CausesValidation = false;
            this.btnReturn.Location = new System.Drawing.Point(178, 471);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(75, 23);
            this.btnReturn.TabIndex = 5;
            this.btnReturn.Text = "Return";
            this.btnReturn.UseVisualStyleBackColor = true;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // txtTaxYear
            // 
            this.txtTaxYear.Location = new System.Drawing.Point(563, 291);
            this.txtTaxYear.Name = "txtTaxYear";
            this.txtTaxYear.ReadOnly = true;
            this.txtTaxYear.Size = new System.Drawing.Size(61, 20);
            this.txtTaxYear.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(507, 298);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Tax Year";
            // 
            // btnClose
            // 
            this.btnClose.CausesValidation = false;
            this.btnClose.Location = new System.Drawing.Point(549, 471);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnViewMemo
            // 
            this.btnViewMemo.Location = new System.Drawing.Point(253, 471);
            this.btnViewMemo.Name = "btnViewMemo";
            this.btnViewMemo.Size = new System.Drawing.Size(115, 23);
            this.btnViewMemo.TabIndex = 9;
            this.btnViewMemo.Text = "View Memoranda";
            this.btnViewMemo.UseVisualStyleBackColor = true;
            this.btnViewMemo.Click += new System.EventHandler(this.btnViewMemo_Click);
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(93, 290);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(141, 21);
            this.bin1.TabIndex = 2;
            // 
            // frameWithShadow1
            // 
            this.frameWithShadow1.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow1.Location = new System.Drawing.Point(9, 12);
            this.frameWithShadow1.Name = "frameWithShadow1";
            this.frameWithShadow1.Size = new System.Drawing.Size(628, 444);
            this.frameWithShadow1.TabIndex = 0;
            // 
            // frameWithShadow2
            // 
            this.frameWithShadow2.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow2.Location = new System.Drawing.Point(9, 462);
            this.frameWithShadow2.Name = "frameWithShadow2";
            this.frameWithShadow2.Size = new System.Drawing.Size(628, 47);
            this.frameWithShadow2.TabIndex = 6;
            this.frameWithShadow2.Load += new System.EventHandler(this.frameWithShadow2_Load);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(416, 288);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(85, 23);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Refresh List";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 325);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "OR Number";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label5.Visible = false;
            // 
            // txtOrNo
            // 
            this.txtOrNo.AcceptsReturn = true;
            this.txtOrNo.Location = new System.Drawing.Point(93, 322);
            this.txtOrNo.Multiline = true;
            this.txtOrNo.Name = "txtOrNo";
            this.txtOrNo.ReadOnly = true;
            this.txtOrNo.Size = new System.Drawing.Size(116, 20);
            this.txtOrNo.TabIndex = 11;
            this.txtOrNo.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(215, 325);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Memoranda";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label6.Visible = false;
            // 
            // txtMemo
            // 
            this.txtMemo.AcceptsReturn = true;
            this.txtMemo.Location = new System.Drawing.Point(284, 322);
            this.txtMemo.Multiline = true;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Size = new System.Drawing.Size(340, 70);
            this.txtMemo.TabIndex = 13;
            this.txtMemo.Visible = false;
            // 
            // btnReject
            // 
            this.btnReject.Location = new System.Drawing.Point(369, 471);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(75, 23);
            this.btnReject.TabIndex = 14;
            this.btnReject.Text = "Reject OR";
            this.btnReject.UseVisualStyleBackColor = true;
            this.btnReject.Visible = false;
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            // 
            // frmBTaxMonitoring
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 521);
            this.Controls.Add(this.btnReject);
            this.Controls.Add(this.txtMemo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtOrNo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnViewMemo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtTaxYear);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.btnViewSOA);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnBilling);
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
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBTaxMonitoring";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Business Tax Monitoring";
            this.Load += new System.EventHandler(this.frmBTaxMonitoring_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.SOA.FrameWithShadow frameWithShadow1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnBilling;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow2;
        public BIN.BIN bin1;
        public System.Windows.Forms.Button btnViewSOA;
        public System.Windows.Forms.Button btnApprove;
        public System.Windows.Forms.Button btnReturn;
        public System.Windows.Forms.DataGridView dgvList;
        public System.Windows.Forms.TextBox txtBnsName;
        public System.Windows.Forms.TextBox txtOwnName;
        public System.Windows.Forms.TextBox txtBnsAddress;
        public System.Windows.Forms.TextBox txtOwnAddress;
        private System.Windows.Forms.TextBox txtTaxYear;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnViewMemo;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox txtOrNo;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox txtMemo;
        private System.Windows.Forms.Button btnReject;
    }
}