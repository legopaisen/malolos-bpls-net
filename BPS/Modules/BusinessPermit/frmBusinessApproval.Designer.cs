namespace Amellar.Modules.BusinessPermit
{
    partial class frmBusinessApproval
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.BIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BNSNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BNSCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BNSSTAT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TAXYEAR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OwnerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STATUS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATEPAID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.APPROVEDBY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTaxYear = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOwner = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnApprove = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnExit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.rdoApproved = new System.Windows.Forms.RadioButton();
            this.rdoPending = new System.Windows.Forms.RadioButton();
            this.rdoALL = new System.Windows.Forms.RadioButton();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bin2 = new Amellar.Common.BIN.BIN();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToResizeColumns = false;
            this.dgvList.AllowUserToResizeRows = false;
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle25.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle25.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle25.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle25;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BIN,
            this.BNSNAME,
            this.BNSCODE,
            this.BNSSTAT,
            this.TAXYEAR,
            this.OwnerName,
            this.STATUS,
            this.DATEPAID,
            this.APPROVEDBY});
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle26.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle26.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvList.DefaultCellStyle = dataGridViewCellStyle26;
            this.dgvList.Location = new System.Drawing.Point(16, 143);
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle27.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle27.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle27.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle27.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.RowHeadersDefaultCellStyle = dataGridViewCellStyle27;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(987, 231);
            this.dgvList.TabIndex = 16;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // BIN
            // 
            this.BIN.HeaderText = "BIN";
            this.BIN.Name = "BIN";
            this.BIN.ReadOnly = true;
            this.BIN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.BIN.Width = 130;
            // 
            // BNSNAME
            // 
            this.BNSNAME.HeaderText = "Business Name";
            this.BNSNAME.Name = "BNSNAME";
            this.BNSNAME.ReadOnly = true;
            this.BNSNAME.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.BNSNAME.Width = 200;
            // 
            // BNSCODE
            // 
            this.BNSCODE.HeaderText = "Business Code";
            this.BNSCODE.Name = "BNSCODE";
            this.BNSCODE.ReadOnly = true;
            this.BNSCODE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.BNSCODE.Width = 70;
            // 
            // BNSSTAT
            // 
            this.BNSSTAT.HeaderText = "Business Status";
            this.BNSSTAT.Name = "BNSSTAT";
            this.BNSSTAT.ReadOnly = true;
            this.BNSSTAT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.BNSSTAT.Width = 70;
            // 
            // TAXYEAR
            // 
            this.TAXYEAR.HeaderText = "Tax Year";
            this.TAXYEAR.Name = "TAXYEAR";
            this.TAXYEAR.ReadOnly = true;
            this.TAXYEAR.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TAXYEAR.Width = 60;
            // 
            // OwnerName
            // 
            this.OwnerName.HeaderText = "Owner\'s Name";
            this.OwnerName.Name = "OwnerName";
            this.OwnerName.ReadOnly = true;
            this.OwnerName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.OwnerName.Width = 150;
            // 
            // STATUS
            // 
            this.STATUS.HeaderText = "Status";
            this.STATUS.Name = "STATUS";
            this.STATUS.ReadOnly = true;
            this.STATUS.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DATEPAID
            // 
            this.DATEPAID.HeaderText = "Date Paid";
            this.DATEPAID.Name = "DATEPAID";
            this.DATEPAID.ReadOnly = true;
            this.DATEPAID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // APPROVEDBY
            // 
            this.APPROVEDBY.HeaderText = "Approved By";
            this.APPROVEDBY.Name = "APPROVEDBY";
            this.APPROVEDBY.ReadOnly = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(248, 19);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(92, 25);
            this.btnSearch.TabIndex = 17;
            this.btnSearch.Text = "Search";
            this.btnSearch.Values.ExtraText = "";
            this.btnSearch.Values.Image = null;
            this.btnSearch.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearch.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearch.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "BIN";
            // 
            // txtBnsName
            // 
            this.txtBnsName.Enabled = false;
            this.txtBnsName.Location = new System.Drawing.Point(99, 50);
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(476, 20);
            this.txtBnsName.TabIndex = 22;
            this.txtBnsName.TextChanged += new System.EventHandler(this.txtBnsName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Business Name";
            // 
            // txtTaxYear
            // 
            this.txtTaxYear.Location = new System.Drawing.Point(99, 102);
            this.txtTaxYear.MaxLength = 4;
            this.txtTaxYear.Name = "txtTaxYear";
            this.txtTaxYear.Size = new System.Drawing.Size(63, 20);
            this.txtTaxYear.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Year";
            // 
            // txtOwner
            // 
            this.txtOwner.Enabled = false;
            this.txtOwner.Location = new System.Drawing.Point(99, 76);
            this.txtOwner.Name = "txtOwner";
            this.txtOwner.ReadOnly = true;
            this.txtOwner.Size = new System.Drawing.Size(476, 20);
            this.txtOwner.TabIndex = 26;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Owner\'s Name";
            // 
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(763, 380);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnApprove.Size = new System.Drawing.Size(117, 25);
            this.btnApprove.TabIndex = 27;
            this.btnApprove.Text = "Approve";
            this.btnApprove.Values.ExtraText = "";
            this.btnApprove.Values.Image = null;
            this.btnApprove.Values.ImageStates.ImageCheckedNormal = null;
            this.btnApprove.Values.ImageStates.ImageCheckedPressed = null;
            this.btnApprove.Values.ImageStates.ImageCheckedTracking = null;
            this.btnApprove.Values.Text = "Approve";
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(886, 380);
            this.btnExit.Name = "btnExit";
            this.btnExit.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnExit.Size = new System.Drawing.Size(117, 25);
            this.btnExit.TabIndex = 28;
            this.btnExit.Text = "Exit";
            this.btnExit.Values.ExtraText = "";
            this.btnExit.Values.Image = null;
            this.btnExit.Values.ImageStates.ImageCheckedNormal = null;
            this.btnExit.Values.ImageStates.ImageCheckedPressed = null;
            this.btnExit.Values.ImageStates.ImageCheckedTracking = null;
            this.btnExit.Values.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // rdoApproved
            // 
            this.rdoApproved.AutoSize = true;
            this.rdoApproved.Location = new System.Drawing.Point(212, 103);
            this.rdoApproved.Name = "rdoApproved";
            this.rdoApproved.Size = new System.Drawing.Size(71, 17);
            this.rdoApproved.TabIndex = 29;
            this.rdoApproved.TabStop = true;
            this.rdoApproved.Text = "Approved";
            this.rdoApproved.UseVisualStyleBackColor = true;
            // 
            // rdoPending
            // 
            this.rdoPending.AutoSize = true;
            this.rdoPending.Location = new System.Drawing.Point(289, 103);
            this.rdoPending.Name = "rdoPending";
            this.rdoPending.Size = new System.Drawing.Size(64, 17);
            this.rdoPending.TabIndex = 30;
            this.rdoPending.TabStop = true;
            this.rdoPending.Text = "Pending";
            this.rdoPending.UseVisualStyleBackColor = true;
            this.rdoPending.CheckedChanged += new System.EventHandler(this.rdoPending_CheckedChanged);
            // 
            // rdoALL
            // 
            this.rdoALL.AutoSize = true;
            this.rdoALL.Location = new System.Drawing.Point(170, 103);
            this.rdoALL.Name = "rdoALL";
            this.rdoALL.Size = new System.Drawing.Size(36, 17);
            this.rdoALL.TabIndex = 31;
            this.rdoALL.TabStop = true;
            this.rdoALL.Text = "All";
            this.rdoALL.UseVisualStyleBackColor = true;
            this.rdoALL.CheckedChanged += new System.EventHandler(this.rdoALL_CheckedChanged);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(359, 102);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(90, 25);
            this.btnGenerate.TabIndex = 32;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.Values.ExtraText = "";
            this.btnGenerate.Values.Image = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedNormal = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedPressed = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedTracking = null;
            this.btnGenerate.Values.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "BIN";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 130;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Business Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 200;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Business Code";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 70;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Business Status";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn4.Width = 70;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Tax Year";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn5.Width = 60;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Owner\'s Name";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn6.Width = 150;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Status";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Date Paid";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "Approved By";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // bin2
            // 
            this.bin2.GetBINSeries = "";
            this.bin2.GetDistCode = "";
            this.bin2.GetLGUCode = "";
            this.bin2.GetTaxYear = "";
            this.bin2.Location = new System.Drawing.Point(99, 21);
            this.bin2.Name = "bin2";
            this.bin2.Size = new System.Drawing.Size(141, 23);
            this.bin2.TabIndex = 33;
            // 
            // frmBusinessApproval
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(1016, 415);
            this.ControlBox = false;
            this.Controls.Add(this.bin2);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.rdoALL);
            this.Controls.Add(this.rdoPending);
            this.Controls.Add(this.rdoApproved);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.txtOwner);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtTaxYear);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBnsName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmBusinessApproval";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Business Approval";
            this.Load += new System.EventHandler(this.frmBusinessApproval_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvList;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBnsName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTaxYear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtOwner;
        private System.Windows.Forms.Label label4;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnApprove;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnExit;
        private System.Windows.Forms.RadioButton rdoApproved;
        private System.Windows.Forms.RadioButton rdoPending;
        private System.Windows.Forms.RadioButton rdoALL;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private System.Windows.Forms.DataGridViewTextBoxColumn BIN;
        private System.Windows.Forms.DataGridViewTextBoxColumn BNSNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn BNSCODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn BNSSTAT;
        private System.Windows.Forms.DataGridViewTextBoxColumn TAXYEAR;
        private System.Windows.Forms.DataGridViewTextBoxColumn OwnerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn STATUS;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATEPAID;
        private System.Windows.Forms.DataGridViewTextBoxColumn APPROVEDBY;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private Amellar.Common.BIN.BIN bin2;
    }
}