namespace ReAssessment
{
    partial class frmReAssessment
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvBnsList = new System.Windows.Forms.DataGridView();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.lblNoBns = new System.Windows.Forms.Label();
            this.gbAddlInfo = new System.Windows.Forms.GroupBox();
            this.txtOwnAdd = new System.Windows.Forms.TextBox();
            this.txtBnsAdd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnUnTag = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnTag = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbBrgy = new System.Windows.Forms.ComboBox();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.txtTaxYear = new System.Windows.Forms.TextBox();
            this.rdobyBrgy = new System.Windows.Forms.RadioButton();
            this.rdobyBin = new System.Windows.Forms.RadioButton();
            this.bin1 = new BIN.BIN();
            this.chkFilter = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClear = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPrint = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label5 = new System.Windows.Forms.Label();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.bin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bns_nm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bns_own = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dec_gross = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qtr_paid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBnsList)).BeginInit();
            this.gbAddlInfo.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvBnsList
            // 
            this.dgvBnsList.AllowUserToAddRows = false;
            this.dgvBnsList.AllowUserToDeleteRows = false;
            this.dgvBnsList.AllowUserToResizeColumns = false;
            this.dgvBnsList.AllowUserToResizeRows = false;
            this.dgvBnsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBnsList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.bin,
            this.bns_nm,
            this.bns_own,
            this.dec_gross,
            this.Column2,
            this.qtr_paid});
            this.dgvBnsList.Location = new System.Drawing.Point(18, 14);
            this.dgvBnsList.MultiSelect = false;
            this.dgvBnsList.Name = "dgvBnsList";
            this.dgvBnsList.RowHeadersVisible = false;
            this.dgvBnsList.Size = new System.Drawing.Size(845, 248);
            this.dgvBnsList.TabIndex = 2;
            this.dgvBnsList.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBnsList_RowEnter);
            this.dgvBnsList.DoubleClick += new System.EventHandler(this.dgvBnsList_DoubleClick);
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(22, 18);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(15, 14);
            this.chkAll.TabIndex = 3;
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckStateChanged += new System.EventHandler(this.chkAll_CheckStateChanged);
            // 
            // lblNoBns
            // 
            this.lblNoBns.AutoSize = true;
            this.lblNoBns.Location = new System.Drawing.Point(17, 274);
            this.lblNoBns.Name = "lblNoBns";
            this.lblNoBns.Size = new System.Drawing.Size(125, 13);
            this.lblNoBns.TabIndex = 4;
            this.lblNoBns.Text = "Total No. of Businesses: ";
            // 
            // gbAddlInfo
            // 
            this.gbAddlInfo.Controls.Add(this.txtOwnAdd);
            this.gbAddlInfo.Controls.Add(this.txtBnsAdd);
            this.gbAddlInfo.Controls.Add(this.label3);
            this.gbAddlInfo.Controls.Add(this.label2);
            this.gbAddlInfo.Location = new System.Drawing.Point(370, 314);
            this.gbAddlInfo.Name = "gbAddlInfo";
            this.gbAddlInfo.Size = new System.Drawing.Size(504, 75);
            this.gbAddlInfo.TabIndex = 5;
            this.gbAddlInfo.TabStop = false;
            this.gbAddlInfo.Text = "Addtional Information";
            // 
            // txtOwnAdd
            // 
            this.txtOwnAdd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtOwnAdd.Location = new System.Drawing.Point(116, 44);
            this.txtOwnAdd.Name = "txtOwnAdd";
            this.txtOwnAdd.ReadOnly = true;
            this.txtOwnAdd.Size = new System.Drawing.Size(377, 20);
            this.txtOwnAdd.TabIndex = 8;
            // 
            // txtBnsAdd
            // 
            this.txtBnsAdd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBnsAdd.Location = new System.Drawing.Point(116, 19);
            this.txtBnsAdd.Name = "txtBnsAdd";
            this.txtBnsAdd.ReadOnly = true;
            this.txtBnsAdd.Size = new System.Drawing.Size(377, 20);
            this.txtBnsAdd.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Owner\'s Address:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Business Address:";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(807, 415);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(67, 29);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnUnTag
            // 
            this.btnUnTag.Location = new System.Drawing.Point(775, 268);
            this.btnUnTag.Name = "btnUnTag";
            this.btnUnTag.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnUnTag.Size = new System.Drawing.Size(67, 29);
            this.btnUnTag.TabIndex = 7;
            this.btnUnTag.Text = "Un-Tag";
            this.btnUnTag.Values.ExtraText = "";
            this.btnUnTag.Values.Image = null;
            this.btnUnTag.Values.ImageStates.ImageCheckedNormal = null;
            this.btnUnTag.Values.ImageStates.ImageCheckedPressed = null;
            this.btnUnTag.Values.ImageStates.ImageCheckedTracking = null;
            this.btnUnTag.Values.Text = "Un-Tag";
            this.btnUnTag.Click += new System.EventHandler(this.btnUnTag_Click);
            // 
            // btnTag
            // 
            this.btnTag.Location = new System.Drawing.Point(702, 268);
            this.btnTag.Name = "btnTag";
            this.btnTag.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnTag.Size = new System.Drawing.Size(67, 29);
            this.btnTag.TabIndex = 8;
            this.btnTag.Text = "Tag";
            this.btnTag.Values.ExtraText = "";
            this.btnTag.Values.Image = null;
            this.btnTag.Values.ImageStates.ImageCheckedNormal = null;
            this.btnTag.Values.ImageStates.ImageCheckedPressed = null;
            this.btnTag.Values.ImageStates.ImageCheckedTracking = null;
            this.btnTag.Values.Text = "Tag";
            this.btnTag.Click += new System.EventHandler(this.btnTag_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(282, 67);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(67, 29);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "Search";
            this.btnSearch.Values.ExtraText = "";
            this.btnSearch.Values.Image = null;
            this.btnSearch.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearch.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearch.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "BIN";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 150;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Business Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 250;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Owner\'s Name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 200;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn4.HeaderText = "Gross";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewTextBoxColumn5.HeaderText = "QTR Paid";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Barangay";
            // 
            // cmbBrgy
            // 
            this.cmbBrgy.FormattingEnabled = true;
            this.cmbBrgy.Location = new System.Drawing.Point(81, 88);
            this.cmbBrgy.Name = "cmbBrgy";
            this.cmbBrgy.Size = new System.Drawing.Size(181, 21);
            this.cmbBrgy.TabIndex = 13;
            this.cmbBrgy.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbBrgy_KeyPress);
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(5, 3);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(869, 308);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.txtTaxYear);
            this.gbFilter.Controls.Add(this.rdobyBrgy);
            this.gbFilter.Controls.Add(this.rdobyBin);
            this.gbFilter.Controls.Add(this.bin1);
            this.gbFilter.Controls.Add(this.chkFilter);
            this.gbFilter.Controls.Add(this.label6);
            this.gbFilter.Controls.Add(this.label4);
            this.gbFilter.Controls.Add(this.label1);
            this.gbFilter.Controls.Add(this.btnClear);
            this.gbFilter.Controls.Add(this.btnSearch);
            this.gbFilter.Controls.Add(this.cmbBrgy);
            this.gbFilter.Location = new System.Drawing.Point(6, 314);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(358, 140);
            this.gbFilter.TabIndex = 14;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Filter By";
            // 
            // txtTaxYear
            // 
            this.txtTaxYear.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTaxYear.Location = new System.Drawing.Point(81, 38);
            this.txtTaxYear.Name = "txtTaxYear";
            this.txtTaxYear.ReadOnly = true;
            this.txtTaxYear.Size = new System.Drawing.Size(58, 20);
            this.txtTaxYear.TabIndex = 17;
            // 
            // rdobyBrgy
            // 
            this.rdobyBrgy.AutoSize = true;
            this.rdobyBrgy.Location = new System.Drawing.Point(200, 17);
            this.rdobyBrgy.Name = "rdobyBrgy";
            this.rdobyBrgy.Size = new System.Drawing.Size(70, 17);
            this.rdobyBrgy.TabIndex = 16;
            this.rdobyBrgy.TabStop = true;
            this.rdobyBrgy.Text = "By BRGY";
            this.rdobyBrgy.UseVisualStyleBackColor = true;
            this.rdobyBrgy.CheckedChanged += new System.EventHandler(this.rdobyBin_CheckedChanged);
            // 
            // rdobyBin
            // 
            this.rdobyBin.AutoSize = true;
            this.rdobyBin.Location = new System.Drawing.Point(90, 17);
            this.rdobyBin.Name = "rdobyBin";
            this.rdobyBin.Size = new System.Drawing.Size(58, 17);
            this.rdobyBin.TabIndex = 16;
            this.rdobyBin.TabStop = true;
            this.rdobyBin.Text = "By BIN";
            this.rdobyBin.UseVisualStyleBackColor = true;
            this.rdobyBin.CheckedChanged += new System.EventHandler(this.rdobyBin_CheckedChanged);
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(81, 63);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(138, 20);
            this.bin1.TabIndex = 15;
            // 
            // chkFilter
            // 
            this.chkFilter.AutoSize = true;
            this.chkFilter.Location = new System.Drawing.Point(81, 117);
            this.chkFilter.Name = "chkFilter";
            this.chkFilter.Size = new System.Drawing.Size(153, 17);
            this.chkFilter.TabIndex = 14;
            this.chkFilter.Text = "Show Tagged records only";
            this.chkFilter.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Tax Year";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(47, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "BIN";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(282, 101);
            this.btnClear.Name = "btnClear";
            this.btnClear.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClear.Size = new System.Drawing.Size(67, 29);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.Values.ExtraText = "";
            this.btnClear.Values.Image = null;
            this.btnClear.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClear.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClear.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClear.Values.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(734, 415);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint.Size = new System.Drawing.Size(67, 29);
            this.btnPrint.TabIndex = 9;
            this.btnPrint.Text = "Print";
            this.btnPrint.Values.ExtraText = "";
            this.btnPrint.Values.Image = null;
            this.btnPrint.Values.ImageStates.ImageCheckedNormal = null;
            this.btnPrint.Values.ImageStates.ImageCheckedPressed = null;
            this.btnPrint.Values.ImageStates.ImageCheckedTracking = null;
            this.btnPrint.Values.Text = "Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(388, 415);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(279, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Note: Only the tagged records above will appear in report.";
            // 
            // Column1
            // 
            this.Column1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.Width = 20;
            // 
            // bin
            // 
            this.bin.HeaderText = "BIN";
            this.bin.Name = "bin";
            this.bin.ReadOnly = true;
            this.bin.Width = 150;
            // 
            // bns_nm
            // 
            this.bns_nm.HeaderText = "Business Name";
            this.bns_nm.Name = "bns_nm";
            this.bns_nm.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.bns_nm.Width = 250;
            // 
            // bns_own
            // 
            this.bns_own.HeaderText = "Owner\'s Name";
            this.bns_own.Name = "bns_own";
            this.bns_own.ReadOnly = true;
            this.bns_own.Width = 200;
            // 
            // dec_gross
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dec_gross.DefaultCellStyle = dataGridViewCellStyle1;
            this.dec_gross.HeaderText = "Gross";
            this.dec_gross.Name = "dec_gross";
            this.dec_gross.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Tax Year";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // qtr_paid
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.qtr_paid.DefaultCellStyle = dataGridViewCellStyle2;
            this.qtr_paid.HeaderText = "QTR Paid";
            this.qtr_paid.Name = "qtr_paid";
            this.qtr_paid.ReadOnly = true;
            this.qtr_paid.Visible = false;
            this.qtr_paid.Width = 80;
            // 
            // frmReAssessment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(879, 458);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.btnTag);
            this.Controls.Add(this.btnUnTag);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbAddlInfo);
            this.Controls.Add(this.lblNoBns);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.chkAll);
            this.Controls.Add(this.dgvBnsList);
            this.Controls.Add(this.containerWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmReAssessment";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Re-assessment Module";
            this.Load += new System.EventHandler(this.frmReAssessment_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBnsList)).EndInit();
            this.gbAddlInfo.ResumeLayout(false);
            this.gbAddlInfo.PerformLayout();
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.DataGridView dgvBnsList;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.Label lblNoBns;
        private System.Windows.Forms.GroupBox gbAddlInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtOwnAdd;
        private System.Windows.Forms.TextBox txtBnsAdd;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnUnTag;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnTag;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBrgy;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.CheckBox chkFilter;
        private BIN.BIN bin1;
        private System.Windows.Forms.RadioButton rdobyBin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rdobyBrgy;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPrint;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClear;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTaxYear;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn bin;
        private System.Windows.Forms.DataGridViewTextBoxColumn bns_nm;
        private System.Windows.Forms.DataGridViewTextBoxColumn bns_own;
        private System.Windows.Forms.DataGridViewTextBoxColumn dec_gross;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn qtr_paid;
    }
}

