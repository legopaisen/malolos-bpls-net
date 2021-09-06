namespace Amellar.Modules.InspectorsDetails
{
    partial class frmUnOfficialList
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
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.chkFilter = new System.Windows.Forms.CheckBox();
            this.cmbInspector = new System.Windows.Forms.ComboBox();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton2 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOwnName = new System.Windows.Forms.TextBox();
            this.txtInspectionNo = new System.Windows.Forms.TextBox();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBnsAdd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblInsNo = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkTaxMapped = new System.Windows.Forms.CheckBox();
            this.chkInspected = new System.Windows.Forms.CheckBox();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
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
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvList.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvList.Location = new System.Drawing.Point(26, 135);
            this.dgvList.Name = "dgvList";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvList.Size = new System.Drawing.Size(842, 236);
            this.dgvList.TabIndex = 9;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(29, 17);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(63, 17);
            this.chkAll.TabIndex = 1;
            this.chkAll.Text = "View All";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckStateChanged += new System.EventHandler(this.chkAll_CheckStateChanged);
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // chkFilter
            // 
            this.chkFilter.AutoSize = true;
            this.chkFilter.Location = new System.Drawing.Point(107, 17);
            this.chkFilter.Name = "chkFilter";
            this.chkFilter.Size = new System.Drawing.Size(67, 17);
            this.chkFilter.TabIndex = 2;
            this.chkFilter.Text = "Filter List";
            this.chkFilter.UseVisualStyleBackColor = true;
            this.chkFilter.CheckStateChanged += new System.EventHandler(this.chkFilter_CheckStateChanged);
            // 
            // cmbInspector
            // 
            this.cmbInspector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInspector.Enabled = false;
            this.cmbInspector.FormattingEnabled = true;
            this.cmbInspector.Location = new System.Drawing.Point(119, 40);
            this.cmbInspector.Name = "cmbInspector";
            this.cmbInspector.Size = new System.Drawing.Size(215, 21);
            this.cmbInspector.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnCancel.Location = new System.Drawing.Point(688, 396);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(87, 25);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Values.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonButton2
            // 
            this.kryptonButton2.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.kryptonButton2.Enabled = false;
            this.kryptonButton2.Location = new System.Drawing.Point(462, 745);
            this.kryptonButton2.Name = "kryptonButton2";
            this.kryptonButton2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonButton2.Size = new System.Drawing.Size(87, 25);
            this.kryptonButton2.TabIndex = 37;
            this.kryptonButton2.Values.Text = "Search";
            // 
            // btnOk
            // 
            this.btnOk.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnOk.Location = new System.Drawing.Point(781, 396);
            this.btnOk.Name = "btnOk";
            this.btnOk.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOk.Size = new System.Drawing.Size(87, 25);
            this.btnOk.TabIndex = 11;
            this.btnOk.Values.Text = "Continue";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 9;
            // 
            // txtOwnName
            // 
            this.txtOwnName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtOwnName.Location = new System.Drawing.Point(630, 66);
            this.txtOwnName.Multiline = true;
            this.txtOwnName.Name = "txtOwnName";
            this.txtOwnName.ReadOnly = true;
            this.txtOwnName.Size = new System.Drawing.Size(225, 20);
            this.txtOwnName.TabIndex = 8;
            // 
            // txtInspectionNo
            // 
            this.txtInspectionNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtInspectionNo.Location = new System.Drawing.Point(630, 40);
            this.txtInspectionNo.Multiline = true;
            this.txtInspectionNo.Name = "txtInspectionNo";
            this.txtInspectionNo.ReadOnly = true;
            this.txtInspectionNo.Size = new System.Drawing.Size(225, 20);
            this.txtInspectionNo.TabIndex = 6;
            // 
            // txtBnsName
            // 
            this.txtBnsName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBnsName.Location = new System.Drawing.Point(107, 65);
            this.txtBnsName.Multiline = true;
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(215, 21);
            this.txtBnsName.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Business Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Inspector";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtBnsAdd
            // 
            this.txtBnsAdd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBnsAdd.Location = new System.Drawing.Point(107, 91);
            this.txtBnsAdd.Multiline = true;
            this.txtBnsAdd.Name = "txtBnsAdd";
            this.txtBnsAdd.ReadOnly = true;
            this.txtBnsAdd.Size = new System.Drawing.Size(748, 20);
            this.txtBnsAdd.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Business Address";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnSearch
            // 
            this.btnSearch.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnSearch.Location = new System.Drawing.Point(769, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(87, 25);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblInsNo
            // 
            this.lblInsNo.AutoSize = true;
            this.lblInsNo.Location = new System.Drawing.Point(548, 45);
            this.lblInsNo.Name = "lblInsNo";
            this.lblInsNo.Size = new System.Drawing.Size(76, 13);
            this.lblInsNo.TabIndex = 11;
            this.lblInsNo.Text = "Inspection No.";
            this.lblInsNo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(548, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Owner\'s Name";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lblInsNo);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chkTaxMapped);
            this.groupBox1.Controls.Add(this.chkInspected);
            this.groupBox1.Controls.Add(this.txtBnsAdd);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtBnsName);
            this.groupBox1.Controls.Add(this.txtInspectionNo);
            this.groupBox1.Controls.Add(this.txtOwnName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(12, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(869, 117);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            // 
            // chkTaxMapped
            // 
            this.chkTaxMapped.AutoSize = true;
            this.chkTaxMapped.Location = new System.Drawing.Point(279, 17);
            this.chkTaxMapped.Name = "chkTaxMapped";
            this.chkTaxMapped.Size = new System.Drawing.Size(107, 17);
            this.chkTaxMapped.TabIndex = 4;
            this.chkTaxMapped.Text = "Tax mapped only";
            this.chkTaxMapped.UseVisualStyleBackColor = true;
            this.chkTaxMapped.CheckStateChanged += new System.EventHandler(this.chkTaxMapped_CheckStateChanged);
            // 
            // chkInspected
            // 
            this.chkInspected.AutoSize = true;
            this.chkInspected.Location = new System.Drawing.Point(178, 17);
            this.chkInspected.Name = "chkInspected";
            this.chkInspected.Size = new System.Drawing.Size(95, 17);
            this.chkInspected.TabIndex = 3;
            this.chkInspected.Text = "Inspected only";
            this.chkInspected.UseVisualStyleBackColor = true;
            this.chkInspected.CheckStateChanged += new System.EventHandler(this.chkInspected_CheckStateChanged);
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(12, 123);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(869, 267);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // frmUnOfficialList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 428);
            this.ControlBox = false;
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.kryptonButton2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cmbInspector);
            this.Controls.Add(this.chkFilter);
            this.Controls.Add(this.chkAll);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.containerWithShadow1);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmUnOfficialList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Inspected Unofficial Businesses";
            this.Load += new System.EventHandler(this.frmUnOfficialList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        public ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton2;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        public System.Windows.Forms.CheckBox chkAll;
        public System.Windows.Forms.CheckBox chkFilter;
        public System.Windows.Forms.ComboBox cmbInspector;
        public System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox txtOwnName;
        public System.Windows.Forms.TextBox txtInspectionNo;
        public System.Windows.Forms.TextBox txtBnsName;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtBnsAdd;
        public System.Windows.Forms.Label label3;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        public System.Windows.Forms.Label lblInsNo;
        public System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.CheckBox chkInspected;
        public System.Windows.Forms.CheckBox chkTaxMapped;
    }
}