namespace Amellar.Modules.EPS
{
    partial class frmEPS
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
            this.dgvEps = new System.Windows.Forms.DataGridView();
            this.txtOwner = new System.Windows.Forms.TextBox();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.txtBnsAdd = new System.Windows.Forms.TextBox();
            this.LblBIN = new System.Windows.Forms.Label();
            this.lblBnsOwner = new System.Windows.Forms.Label();
            this.lblBnsName = new System.Windows.Forms.Label();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblBnsAdd = new System.Windows.Forms.Label();
            this.btnSetApproval = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.grpAssApproval = new System.Windows.Forms.GroupBox();
            this.rdoDisapprove = new System.Windows.Forms.RadioButton();
            this.rdoPennding = new System.Windows.Forms.RadioButton();
            this.rdoApprove = new System.Windows.Forms.RadioButton();
            this.lblAnnualInsFee = new System.Windows.Forms.Label();
            this.lblAppRemarks = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.bin1 = new BIN.BIN();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTaxYear = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBns_Stat = new System.Windows.Forms.TextBox();
            this.dgInspectionFee = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtTotalAnnualFee = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnViewPayments = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPayHist = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPrintTrail = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEps)).BeginInit();
            this.grpAssApproval.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgInspectionFee)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvEps
            // 
            this.dgvEps.AllowUserToAddRows = false;
            this.dgvEps.AllowUserToDeleteRows = false;
            this.dgvEps.AllowUserToResizeRows = false;
            this.dgvEps.ColumnHeadersHeight = 34;
            this.dgvEps.Location = new System.Drawing.Point(20, 127);
            this.dgvEps.Name = "dgvEps";
            this.dgvEps.RowHeadersVisible = false;
            this.dgvEps.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvEps.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEps.Size = new System.Drawing.Size(456, 187);
            this.dgvEps.TabIndex = 8;
            this.dgvEps.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEps_CellContentDoubleClick);
            this.dgvEps.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvEps_CurrentCellDirtyStateChanged);
            this.dgvEps.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEps_CellContentClick);
            // 
            // txtOwner
            // 
            this.txtOwner.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtOwner.Location = new System.Drawing.Point(95, 36);
            this.txtOwner.Name = "txtOwner";
            this.txtOwner.ReadOnly = true;
            this.txtOwner.Size = new System.Drawing.Size(391, 20);
            this.txtOwner.TabIndex = 3;
            // 
            // txtBnsName
            // 
            this.txtBnsName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBnsName.Location = new System.Drawing.Point(95, 62);
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(281, 20);
            this.txtBnsName.TabIndex = 4;
            // 
            // txtBnsAdd
            // 
            this.txtBnsAdd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBnsAdd.Location = new System.Drawing.Point(95, 87);
            this.txtBnsAdd.Name = "txtBnsAdd";
            this.txtBnsAdd.ReadOnly = true;
            this.txtBnsAdd.Size = new System.Drawing.Size(281, 20);
            this.txtBnsAdd.TabIndex = 6;
            // 
            // LblBIN
            // 
            this.LblBIN.AutoSize = true;
            this.LblBIN.Location = new System.Drawing.Point(215, 15);
            this.LblBIN.Name = "LblBIN";
            this.LblBIN.Size = new System.Drawing.Size(25, 13);
            this.LblBIN.TabIndex = 5;
            this.LblBIN.Text = "BIN";
            // 
            // lblBnsOwner
            // 
            this.lblBnsOwner.AutoSize = true;
            this.lblBnsOwner.Location = new System.Drawing.Point(7, 39);
            this.lblBnsOwner.Name = "lblBnsOwner";
            this.lblBnsOwner.Size = new System.Drawing.Size(83, 13);
            this.lblBnsOwner.TabIndex = 6;
            this.lblBnsOwner.Text = "Business Owner";
            // 
            // lblBnsName
            // 
            this.lblBnsName.AutoSize = true;
            this.lblBnsName.Location = new System.Drawing.Point(10, 65);
            this.lblBnsName.Name = "lblBnsName";
            this.lblBnsName.Size = new System.Drawing.Size(80, 13);
            this.lblBnsName.TabIndex = 7;
            this.lblBnsName.Text = "Business Name";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(402, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(84, 25);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "&Search";
            this.btnSearch.Values.ExtraText = "";
            this.btnSearch.Values.Image = null;
            this.btnSearch.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearch.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearch.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearch.Values.Text = "&Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblBnsAdd
            // 
            this.lblBnsAdd.AutoSize = true;
            this.lblBnsAdd.Location = new System.Drawing.Point(19, 90);
            this.lblBnsAdd.Name = "lblBnsAdd";
            this.lblBnsAdd.Size = new System.Drawing.Size(71, 13);
            this.lblBnsAdd.TabIndex = 12;
            this.lblBnsAdd.Text = "Business Add";
            // 
            // btnSetApproval
            // 
            this.btnSetApproval.Enabled = false;
            this.btnSetApproval.Location = new System.Drawing.Point(311, 605);
            this.btnSetApproval.Name = "btnSetApproval";
            this.btnSetApproval.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSetApproval.Size = new System.Drawing.Size(84, 28);
            this.btnSetApproval.TabIndex = 16;
            this.btnSetApproval.Text = "Save";
            this.btnSetApproval.Values.ExtraText = "";
            this.btnSetApproval.Values.Image = null;
            this.btnSetApproval.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSetApproval.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSetApproval.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSetApproval.Values.Text = "Save";
            this.btnSetApproval.Click += new System.EventHandler(this.btnSetApproval_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(401, 605);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(84, 28);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grpAssApproval
            // 
            this.grpAssApproval.Controls.Add(this.rdoDisapprove);
            this.grpAssApproval.Controls.Add(this.rdoPennding);
            this.grpAssApproval.Controls.Add(this.rdoApprove);
            this.grpAssApproval.Location = new System.Drawing.Point(16, 554);
            this.grpAssApproval.Name = "grpAssApproval";
            this.grpAssApproval.Size = new System.Drawing.Size(465, 39);
            this.grpAssApproval.TabIndex = 12;
            this.grpAssApproval.TabStop = false;
            this.grpAssApproval.Text = "Assessment Approval";
            // 
            // rdoDisapprove
            // 
            this.rdoDisapprove.AutoSize = true;
            this.rdoDisapprove.Location = new System.Drawing.Point(337, 15);
            this.rdoDisapprove.Name = "rdoDisapprove";
            this.rdoDisapprove.Size = new System.Drawing.Size(79, 17);
            this.rdoDisapprove.TabIndex = 15;
            this.rdoDisapprove.TabStop = true;
            this.rdoDisapprove.Text = "Disapprove";
            this.rdoDisapprove.UseVisualStyleBackColor = true;
            // 
            // rdoPennding
            // 
            this.rdoPennding.AutoSize = true;
            this.rdoPennding.Location = new System.Drawing.Point(193, 15);
            this.rdoPennding.Name = "rdoPennding";
            this.rdoPennding.Size = new System.Drawing.Size(64, 17);
            this.rdoPennding.TabIndex = 14;
            this.rdoPennding.TabStop = true;
            this.rdoPennding.Text = "Pending";
            this.rdoPennding.UseVisualStyleBackColor = true;
            // 
            // rdoApprove
            // 
            this.rdoApprove.AutoSize = true;
            this.rdoApprove.Location = new System.Drawing.Point(48, 15);
            this.rdoApprove.Name = "rdoApprove";
            this.rdoApprove.Size = new System.Drawing.Size(65, 17);
            this.rdoApprove.TabIndex = 13;
            this.rdoApprove.TabStop = true;
            this.rdoApprove.Text = "Approve";
            this.rdoApprove.UseVisualStyleBackColor = true;
            // 
            // lblAnnualInsFee
            // 
            this.lblAnnualInsFee.AutoSize = true;
            this.lblAnnualInsFee.Location = new System.Drawing.Point(24, 341);
            this.lblAnnualInsFee.Name = "lblAnnualInsFee";
            this.lblAnnualInsFee.Size = new System.Drawing.Size(113, 13);
            this.lblAnnualInsFee.TabIndex = 20;
            this.lblAnnualInsFee.Text = "Annual Inspection Fee";
            // 
            // lblAppRemarks
            // 
            this.lblAppRemarks.AutoSize = true;
            this.lblAppRemarks.Location = new System.Drawing.Point(24, 527);
            this.lblAppRemarks.Name = "lblAppRemarks";
            this.lblAppRemarks.Size = new System.Drawing.Size(94, 13);
            this.lblAppRemarks.TabIndex = 24;
            this.lblAppRemarks.Text = "Approval Remarks";
            // 
            // txtRemarks
            // 
            this.txtRemarks.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRemarks.Location = new System.Drawing.Point(136, 524);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(338, 20);
            this.txtRemarks.TabIndex = 11;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(7, 113);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(482, 219);
            this.containerWithShadow1.TabIndex = 21;
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(7, 330);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(480, 276);
            this.containerWithShadow2.TabIndex = 22;
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(247, 8);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(138, 20);
            this.bin1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(379, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Tax Year";
            // 
            // txtTaxYear
            // 
            this.txtTaxYear.BackColor = System.Drawing.SystemColors.Control;
            this.txtTaxYear.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTaxYear.Location = new System.Drawing.Point(432, 62);
            this.txtTaxYear.Name = "txtTaxYear";
            this.txtTaxYear.ReadOnly = true;
            this.txtTaxYear.Size = new System.Drawing.Size(55, 20);
            this.txtTaxYear.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(392, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Status";
            // 
            // txtBns_Stat
            // 
            this.txtBns_Stat.BackColor = System.Drawing.SystemColors.Control;
            this.txtBns_Stat.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBns_Stat.Location = new System.Drawing.Point(432, 87);
            this.txtBns_Stat.Name = "txtBns_Stat";
            this.txtBns_Stat.ReadOnly = true;
            this.txtBns_Stat.Size = new System.Drawing.Size(55, 20);
            this.txtBns_Stat.TabIndex = 7;
            // 
            // dgInspectionFee
            // 
            this.dgInspectionFee.AllowUserToAddRows = false;
            this.dgInspectionFee.AllowUserToDeleteRows = false;
            this.dgInspectionFee.AllowUserToResizeRows = false;
            this.dgInspectionFee.ColumnHeadersHeight = 34;
            this.dgInspectionFee.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dgInspectionFee.Location = new System.Drawing.Point(22, 359);
            this.dgInspectionFee.Name = "dgInspectionFee";
            this.dgInspectionFee.RowHeadersVisible = false;
            this.dgInspectionFee.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgInspectionFee.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgInspectionFee.Size = new System.Drawing.Size(456, 110);
            this.dgInspectionFee.TabIndex = 9;
            this.dgInspectionFee.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgInspectionFee_CellValueChanged);
            this.dgInspectionFee.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgInspectionFee_CellValidating);
            this.dgInspectionFee.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgInspectionFee_CurrentCellDirtyStateChanged);
            this.dgInspectionFee.SelectionChanged += new System.EventHandler(this.dgInspectionFee_SelectionChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Tax Year";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 150;
            // 
            // Column2
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column2.HeaderText = "Annual Inspection Fee";
            this.Column2.Name = "Column2";
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 280;
            // 
            // txtTotalAnnualFee
            // 
            this.txtTotalAnnualFee.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTotalAnnualFee.Location = new System.Drawing.Point(353, 475);
            this.txtTotalAnnualFee.Name = "txtTotalAnnualFee";
            this.txtTotalAnnualFee.ReadOnly = true;
            this.txtTotalAnnualFee.Size = new System.Drawing.Size(121, 20);
            this.txtTotalAnnualFee.TabIndex = 10;
            this.txtTotalAnnualFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(277, 478);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Total Amount";
            // 
            // btnViewPayments
            // 
            this.btnViewPayments.Enabled = false;
            this.btnViewPayments.Location = new System.Drawing.Point(20, 475);
            this.btnViewPayments.Name = "btnViewPayments";
            this.btnViewPayments.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnViewPayments.Size = new System.Drawing.Size(84, 28);
            this.btnViewPayments.TabIndex = 30;
            this.btnViewPayments.Text = "View";
            this.btnViewPayments.Values.ExtraText = "";
            this.btnViewPayments.Values.Image = null;
            this.btnViewPayments.Values.ImageStates.ImageCheckedNormal = null;
            this.btnViewPayments.Values.ImageStates.ImageCheckedPressed = null;
            this.btnViewPayments.Values.ImageStates.ImageCheckedTracking = null;
            this.btnViewPayments.Values.Text = "View";
            this.btnViewPayments.Click += new System.EventHandler(this.btnViewPayments_Click);
            // 
            // btnPayHist
            // 
            this.btnPayHist.Location = new System.Drawing.Point(10, 605);
            this.btnPayHist.Name = "btnPayHist";
            this.btnPayHist.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPayHist.Size = new System.Drawing.Size(108, 28);
            this.btnPayHist.TabIndex = 62;
            this.btnPayHist.Text = "Payment History";
            this.btnPayHist.Values.ExtraText = "";
            this.btnPayHist.Values.Image = null;
            this.btnPayHist.Values.ImageStates.ImageCheckedNormal = null;
            this.btnPayHist.Values.ImageStates.ImageCheckedPressed = null;
            this.btnPayHist.Values.ImageStates.ImageCheckedTracking = null;
            this.btnPayHist.Values.Text = "Payment History";
            this.btnPayHist.Click += new System.EventHandler(this.btnPayHist_Click);
            // 
            // btnPrintTrail
            // 
            this.btnPrintTrail.Enabled = false;
            this.btnPrintTrail.Location = new System.Drawing.Point(124, 605);
            this.btnPrintTrail.Name = "btnPrintTrail";
            this.btnPrintTrail.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrintTrail.Size = new System.Drawing.Size(84, 28);
            this.btnPrintTrail.TabIndex = 63;
            this.btnPrintTrail.Text = "Trail Log";
            this.btnPrintTrail.Values.ExtraText = "";
            this.btnPrintTrail.Values.Image = null;
            this.btnPrintTrail.Values.ImageStates.ImageCheckedNormal = null;
            this.btnPrintTrail.Values.ImageStates.ImageCheckedPressed = null;
            this.btnPrintTrail.Values.ImageStates.ImageCheckedTracking = null;
            this.btnPrintTrail.Values.Text = "Trail Log";
            this.btnPrintTrail.Click += new System.EventHandler(this.btnPrintTrail_Click);
            // 
            // frmEPS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 637);
            this.ControlBox = false;
            this.Controls.Add(this.btnPrintTrail);
            this.Controls.Add(this.btnPayHist);
            this.Controls.Add(this.btnViewPayments);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTaxYear);
            this.Controls.Add(this.txtBns_Stat);
            this.Controls.Add(this.bin1);
            this.Controls.Add(this.txtTotalAnnualFee);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblAppRemarks);
            this.Controls.Add(this.lblAnnualInsFee);
            this.Controls.Add(this.grpAssApproval);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSetApproval);
            this.Controls.Add(this.lblBnsAdd);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dgInspectionFee);
            this.Controls.Add(this.dgvEps);
            this.Controls.Add(this.lblBnsName);
            this.Controls.Add(this.lblBnsOwner);
            this.Controls.Add(this.LblBIN);
            this.Controls.Add(this.txtBnsAdd);
            this.Controls.Add(this.txtBnsName);
            this.Controls.Add(this.txtOwner);
            this.Controls.Add(this.containerWithShadow1);
            this.Controls.Add(this.containerWithShadow2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmEPS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EPS";
            this.Load += new System.EventHandler(this.frmEPS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEps)).EndInit();
            this.grpAssApproval.ResumeLayout(false);
            this.grpAssApproval.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgInspectionFee)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvEps;
        private System.Windows.Forms.TextBox txtOwner;
        private System.Windows.Forms.TextBox txtBnsName;
        private System.Windows.Forms.TextBox txtBnsAdd;
        private System.Windows.Forms.Label LblBIN;
        private System.Windows.Forms.Label lblBnsOwner;
        private System.Windows.Forms.Label lblBnsName;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private System.Windows.Forms.Label lblBnsAdd;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSetApproval;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private System.Windows.Forms.GroupBox grpAssApproval;
        private System.Windows.Forms.RadioButton rdoDisapprove;
        private System.Windows.Forms.RadioButton rdoPennding;
        private System.Windows.Forms.RadioButton rdoApprove;
        private System.Windows.Forms.Label lblAnnualInsFee;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private System.Windows.Forms.Label lblAppRemarks;
        private System.Windows.Forms.TextBox txtRemarks;
        private BIN.BIN bin1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTaxYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBns_Stat;
        private System.Windows.Forms.DataGridView dgInspectionFee;
        private System.Windows.Forms.TextBox txtTotalAnnualFee;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnViewPayments;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPayHist;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPrintTrail;

    }
}

