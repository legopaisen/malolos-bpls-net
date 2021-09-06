namespace Amellar.Modules.LiquidationReports
{
    partial class frmAbstractTeller_Binan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbstractTeller_Binan));
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTax = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.dgvFees = new System.Windows.Forms.DataGridView();
            this.kryptonHeader3 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.cmbTeller = new System.Windows.Forms.ComboBox();
            this.lblTeller = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.frameWithShadow3 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow1 = new Amellar.Common.SOA.FrameWithShadow();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoOr = new System.Windows.Forms.RadioButton();
            this.rdoRCD = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFees)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Description";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 230;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Code";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Visible = false;
            this.Column3.Width = 50;
            // 
            // dgvTax
            // 
            this.dgvTax.AllowUserToAddRows = false;
            this.dgvTax.AllowUserToDeleteRows = false;
            this.dgvTax.AllowUserToResizeColumns = false;
            this.dgvTax.AllowUserToResizeRows = false;
            this.dgvTax.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTax.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dgvTax.Location = new System.Drawing.Point(18, 96);
            this.dgvTax.MultiSelect = false;
            this.dgvTax.Name = "dgvTax";
            this.dgvTax.RowHeadersVisible = false;
            this.dgvTax.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvTax.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTax.Size = new System.Drawing.Size(281, 274);
            this.dgvTax.TabIndex = 82;
            this.dgvTax.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTax_CellEndEdit);
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 30;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Code";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Description";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 230;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.Width = 30;
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(11, 66);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader1.Size = new System.Drawing.Size(299, 24);
            this.kryptonHeader1.TabIndex = 81;
            this.kryptonHeader1.Text = "Line of Business";
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Line of Business";
            this.kryptonHeader1.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader1.Values.Image")));
            // 
            // dgvFees
            // 
            this.dgvFees.AllowUserToAddRows = false;
            this.dgvFees.AllowUserToDeleteRows = false;
            this.dgvFees.AllowUserToResizeColumns = false;
            this.dgvFees.AllowUserToResizeRows = false;
            this.dgvFees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvFees.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column3,
            this.Column2});
            this.dgvFees.Location = new System.Drawing.Point(332, 96);
            this.dgvFees.MultiSelect = false;
            this.dgvFees.Name = "dgvFees";
            this.dgvFees.RowHeadersVisible = false;
            this.dgvFees.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvFees.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFees.Size = new System.Drawing.Size(281, 274);
            this.dgvFees.TabIndex = 79;
            this.dgvFees.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFees_CellEndEdit);
            // 
            // kryptonHeader3
            // 
            this.kryptonHeader3.AutoSize = false;
            this.kryptonHeader3.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader3.Location = new System.Drawing.Point(325, 66);
            this.kryptonHeader3.Name = "kryptonHeader3";
            this.kryptonHeader3.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader3.Size = new System.Drawing.Size(299, 24);
            this.kryptonHeader3.TabIndex = 78;
            this.kryptonHeader3.Text = "Regulatory Fees";
            this.kryptonHeader3.Values.Description = "";
            this.kryptonHeader3.Values.Heading = "Regulatory Fees";
            this.kryptonHeader3.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader3.Values.Image")));
            // 
            // cmbTeller
            // 
            this.cmbTeller.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbTeller.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbTeller.FormattingEnabled = true;
            this.cmbTeller.Location = new System.Drawing.Point(284, 22);
            this.cmbTeller.Name = "cmbTeller";
            this.cmbTeller.Size = new System.Drawing.Size(111, 21);
            this.cmbTeller.TabIndex = 69;
            // 
            // lblTeller
            // 
            this.lblTeller.AutoSize = true;
            this.lblTeller.Location = new System.Drawing.Point(247, 25);
            this.lblTeller.Name = "lblTeller";
            this.lblTeller.Size = new System.Drawing.Size(33, 13);
            this.lblTeller.TabIndex = 72;
            this.lblTeller.Text = "Teller";
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(158, 23);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(79, 20);
            this.dtpTo.TabIndex = 68;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(50, 23);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(79, 20);
            this.dtpFrom.TabIndex = 67;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(134, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 74;
            this.label4.Text = "To";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 73;
            this.label3.Text = "From";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(547, 390);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(73, 25);
            this.btnCancel.TabIndex = 71;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(468, 390);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(73, 25);
            this.btnGenerate.TabIndex = 70;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.Values.ExtraText = "";
            this.btnGenerate.Values.Image = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedNormal = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedPressed = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedTracking = null;
            this.btnGenerate.Values.Text = "&Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // frameWithShadow3
            // 
            this.frameWithShadow3.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow3.Location = new System.Drawing.Point(319, 66);
            this.frameWithShadow3.Name = "frameWithShadow3";
            this.frameWithShadow3.Size = new System.Drawing.Size(307, 318);
            this.frameWithShadow3.TabIndex = 77;
            // 
            // frameWithShadow1
            // 
            this.frameWithShadow1.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow1.Location = new System.Drawing.Point(5, 66);
            this.frameWithShadow1.Name = "frameWithShadow1";
            this.frameWithShadow1.Size = new System.Drawing.Size(307, 318);
            this.frameWithShadow1.TabIndex = 83;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpTo);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dtpFrom);
            this.groupBox1.Controls.Add(this.lblTeller);
            this.groupBox1.Controls.Add(this.cmbTeller);
            this.groupBox1.Location = new System.Drawing.Point(10, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(405, 55);
            this.groupBox1.TabIndex = 84;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Covered Period:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdoRCD);
            this.groupBox2.Controls.Add(this.rdoOr);
            this.groupBox2.Location = new System.Drawing.Point(420, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 55);
            this.groupBox2.TabIndex = 85;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filter by:";
            // 
            // rdoOr
            // 
            this.rdoOr.AutoSize = true;
            this.rdoOr.Location = new System.Drawing.Point(24, 23);
            this.rdoOr.Name = "rdoOr";
            this.rdoOr.Size = new System.Drawing.Size(67, 17);
            this.rdoOr.TabIndex = 0;
            this.rdoOr.TabStop = true;
            this.rdoOr.Text = "OR Date";
            this.rdoOr.UseVisualStyleBackColor = true;
            // 
            // rdoRCD
            // 
            this.rdoRCD.AutoSize = true;
            this.rdoRCD.Location = new System.Drawing.Point(106, 23);
            this.rdoRCD.Name = "rdoRCD";
            this.rdoRCD.Size = new System.Drawing.Size(74, 17);
            this.rdoRCD.TabIndex = 0;
            this.rdoRCD.TabStop = true;
            this.rdoRCD.Text = "RCD Date";
            this.rdoRCD.UseVisualStyleBackColor = true;
            // 
            // frmAbstractTeller_Binan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 420);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.dgvTax);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.dgvFees);
            this.Controls.Add(this.kryptonHeader3);
            this.Controls.Add(this.frameWithShadow3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.frameWithShadow1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 448);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 448);
            this.Name = "frmAbstractTeller_Binan";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frmAbstractTeller_Binan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFees)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridView dgvTax;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private System.Windows.Forms.DataGridView dgvFees;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader3;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow3;
        private System.Windows.Forms.ComboBox cmbTeller;
        private System.Windows.Forms.Label lblTeller;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoRCD;
        private System.Windows.Forms.RadioButton rdoOr;
    }
}