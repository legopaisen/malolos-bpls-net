namespace Amellar.Modules.Utilities
{
    partial class frmSplOrd
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle46 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle47 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle48 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnEdit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblRevYear = new System.Windows.Forms.Label();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAdd = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.dgvRevYear = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEffYear = new System.Windows.Forms.TextBox();
            this.cmbRevYear = new System.Windows.Forms.ComboBox();
            this.grpApply = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoMonth = new System.Windows.Forms.RadioButton();
            this.rdoAnnual = new System.Windows.Forms.RadioButton();
            this.rdoQuarter = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoDec = new System.Windows.Forms.RadioButton();
            this.rdoInc = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAmt = new System.Windows.Forms.TextBox();
            this.rdoRate = new System.Windows.Forms.RadioButton();
            this.rdoAmt = new System.Windows.Forms.RadioButton();
            this.txtRate = new System.Windows.Forms.TextBox();
            this.chkFees = new System.Windows.Forms.CheckBox();
            this.chkTax = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnDelete = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.cmbFees = new MultiColumnComboBoxDemo.MultiColumnComboBox();
            this.cmbTax = new MultiColumnComboBoxDemo.MultiColumnComboBox();
            this.containerWithShadow3 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRevYear)).BeginInit();
            this.grpApply.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(128, 445);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnEdit.Size = new System.Drawing.Size(88, 25);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit";
            this.btnEdit.Values.ExtraText = "";
            this.btnEdit.Values.Image = null;
            this.btnEdit.Values.ImageStates.ImageCheckedNormal = null;
            this.btnEdit.Values.ImageStates.ImageCheckedPressed = null;
            this.btnEdit.Values.ImageStates.ImageCheckedTracking = null;
            this.btnEdit.Values.Text = "Edit";
            // 
            // lblRevYear
            // 
            this.lblRevYear.AutoSize = true;
            this.lblRevYear.Location = new System.Drawing.Point(15, 26);
            this.lblRevYear.Name = "lblRevYear";
            this.lblRevYear.Size = new System.Drawing.Size(76, 13);
            this.lblRevYear.TabIndex = 45;
            this.lblRevYear.Text = "Revenue Year";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(316, 445);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(88, 25);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Close";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(34, 445);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnAdd.Size = new System.Drawing.Size(88, 25);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.Values.ExtraText = "";
            this.btnAdd.Values.Image = null;
            this.btnAdd.Values.ImageStates.ImageCheckedNormal = null;
            this.btnAdd.Values.ImageStates.ImageCheckedPressed = null;
            this.btnAdd.Values.ImageStates.ImageCheckedTracking = null;
            this.btnAdd.Values.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgvRevYear
            // 
            this.dgvRevYear.AllowUserToAddRows = false;
            this.dgvRevYear.AllowUserToDeleteRows = false;
            this.dgvRevYear.AllowUserToResizeRows = false;
            dataGridViewCellStyle46.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle46.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle46.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle46.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle46.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle46.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle46.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRevYear.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle46;
            this.dgvRevYear.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle47.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle47.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle47.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle47.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle47.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle47.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle47.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRevYear.DefaultCellStyle = dataGridViewCellStyle47;
            this.dgvRevYear.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvRevYear.Location = new System.Drawing.Point(27, 24);
            this.dgvRevYear.Name = "dgvRevYear";
            dataGridViewCellStyle48.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle48.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle48.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle48.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle48.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle48.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle48.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRevYear.RowHeadersDefaultCellStyle = dataGridViewCellStyle48;
            this.dgvRevYear.Size = new System.Drawing.Size(384, 145);
            this.dgvRevYear.TabIndex = 44;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(209, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "Effectivity Year";
            // 
            // txtEffYear
            // 
            this.txtEffYear.Location = new System.Drawing.Point(293, 23);
            this.txtEffYear.MaxLength = 4;
            this.txtEffYear.Name = "txtEffYear";
            this.txtEffYear.ReadOnly = true;
            this.txtEffYear.Size = new System.Drawing.Size(73, 20);
            this.txtEffYear.TabIndex = 6;
            // 
            // cmbRevYear
            // 
            this.cmbRevYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRevYear.Enabled = false;
            this.cmbRevYear.FormattingEnabled = true;
            this.cmbRevYear.Location = new System.Drawing.Point(103, 23);
            this.cmbRevYear.Name = "cmbRevYear";
            this.cmbRevYear.Size = new System.Drawing.Size(73, 21);
            this.cmbRevYear.TabIndex = 5;
            this.cmbRevYear.SelectedIndexChanged += new System.EventHandler(this.cmbRevYear_SelectedIndexChanged);
            // 
            // grpApply
            // 
            this.grpApply.Controls.Add(this.groupBox3);
            this.grpApply.Controls.Add(this.cmbFees);
            this.grpApply.Controls.Add(this.cmbTax);
            this.grpApply.Controls.Add(this.groupBox2);
            this.grpApply.Controls.Add(this.groupBox1);
            this.grpApply.Controls.Add(this.chkFees);
            this.grpApply.Controls.Add(this.chkTax);
            this.grpApply.Controls.Add(this.cmbRevYear);
            this.grpApply.Controls.Add(this.label1);
            this.grpApply.Controls.Add(this.txtEffYear);
            this.grpApply.Controls.Add(this.lblRevYear);
            this.grpApply.Controls.Add(this.label5);
            this.grpApply.Location = new System.Drawing.Point(27, 175);
            this.grpApply.Name = "grpApply";
            this.grpApply.Size = new System.Drawing.Size(384, 264);
            this.grpApply.TabIndex = 50;
            this.grpApply.TabStop = false;
            this.grpApply.Text = " Apply to:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdoMonth);
            this.groupBox2.Controls.Add(this.rdoAnnual);
            this.groupBox2.Controls.Add(this.rdoQuarter);
            this.groupBox2.Location = new System.Drawing.Point(222, 109);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(144, 140);
            this.groupBox2.TabIndex = 59;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Term: ";
            // 
            // rdoMonth
            // 
            this.rdoMonth.AutoSize = true;
            this.rdoMonth.Enabled = false;
            this.rdoMonth.Location = new System.Drawing.Point(28, 70);
            this.rdoMonth.Name = "rdoMonth";
            this.rdoMonth.Size = new System.Drawing.Size(62, 17);
            this.rdoMonth.TabIndex = 19;
            this.rdoMonth.Text = "Monthly";
            this.rdoMonth.UseVisualStyleBackColor = true;
            this.rdoMonth.CheckedChanged += new System.EventHandler(this.rdoMonth_CheckedChanged);
            // 
            // rdoAnnual
            // 
            this.rdoAnnual.AutoSize = true;
            this.rdoAnnual.Enabled = false;
            this.rdoAnnual.Location = new System.Drawing.Point(28, 24);
            this.rdoAnnual.Name = "rdoAnnual";
            this.rdoAnnual.Size = new System.Drawing.Size(65, 17);
            this.rdoAnnual.TabIndex = 17;
            this.rdoAnnual.Text = "Annually";
            this.rdoAnnual.UseVisualStyleBackColor = true;
            this.rdoAnnual.CheckedChanged += new System.EventHandler(this.rdoAnnual_CheckedChanged);
            // 
            // rdoQuarter
            // 
            this.rdoQuarter.AutoSize = true;
            this.rdoQuarter.Enabled = false;
            this.rdoQuarter.Location = new System.Drawing.Point(28, 47);
            this.rdoQuarter.Name = "rdoQuarter";
            this.rdoQuarter.Size = new System.Drawing.Size(67, 17);
            this.rdoQuarter.TabIndex = 18;
            this.rdoQuarter.Text = "Quarterly";
            this.rdoQuarter.UseVisualStyleBackColor = true;
            this.rdoQuarter.CheckedChanged += new System.EventHandler(this.rdoQuarter_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtAmt);
            this.groupBox1.Controls.Add(this.rdoRate);
            this.groupBox1.Controls.Add(this.rdoAmt);
            this.groupBox1.Controls.Add(this.txtRate);
            this.groupBox1.Location = new System.Drawing.Point(16, 109);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 87);
            this.groupBox1.TabIndex = 57;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Type: ";
            // 
            // rdoDec
            // 
            this.rdoDec.AutoSize = true;
            this.rdoDec.Enabled = false;
            this.rdoDec.Location = new System.Drawing.Point(116, 19);
            this.rdoDec.Name = "rdoDec";
            this.rdoDec.Size = new System.Drawing.Size(71, 17);
            this.rdoDec.TabIndex = 16;
            this.rdoDec.Text = "Decrease";
            this.rdoDec.UseVisualStyleBackColor = true;
            this.rdoDec.CheckedChanged += new System.EventHandler(this.rdoDec_CheckedChanged);
            // 
            // rdoInc
            // 
            this.rdoInc.AutoSize = true;
            this.rdoInc.Enabled = false;
            this.rdoInc.Location = new System.Drawing.Point(27, 19);
            this.rdoInc.Name = "rdoInc";
            this.rdoInc.Size = new System.Drawing.Size(66, 17);
            this.rdoInc.TabIndex = 15;
            this.rdoInc.Text = "Increase";
            this.rdoInc.UseVisualStyleBackColor = true;
            this.rdoInc.CheckedChanged += new System.EventHandler(this.rdoInc_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(175, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 58;
            this.label2.Text = "%";
            // 
            // txtAmt
            // 
            this.txtAmt.Location = new System.Drawing.Point(116, 48);
            this.txtAmt.Name = "txtAmt";
            this.txtAmt.ReadOnly = true;
            this.txtAmt.Size = new System.Drawing.Size(53, 20);
            this.txtAmt.TabIndex = 14;
            // 
            // rdoRate
            // 
            this.rdoRate.AutoSize = true;
            this.rdoRate.Enabled = false;
            this.rdoRate.Location = new System.Drawing.Point(17, 25);
            this.rdoRate.Name = "rdoRate";
            this.rdoRate.Size = new System.Drawing.Size(76, 17);
            this.rdoRate.TabIndex = 11;
            this.rdoRate.Text = "Fixed Rate";
            this.rdoRate.UseVisualStyleBackColor = true;
            this.rdoRate.CheckedChanged += new System.EventHandler(this.rdoRate_CheckedChanged);
            // 
            // rdoAmt
            // 
            this.rdoAmt.AutoSize = true;
            this.rdoAmt.Enabled = false;
            this.rdoAmt.Location = new System.Drawing.Point(17, 49);
            this.rdoAmt.Name = "rdoAmt";
            this.rdoAmt.Size = new System.Drawing.Size(89, 17);
            this.rdoAmt.TabIndex = 13;
            this.rdoAmt.Text = "Fixed Amount";
            this.rdoAmt.UseVisualStyleBackColor = true;
            this.rdoAmt.CheckedChanged += new System.EventHandler(this.rdoAmt_CheckedChanged);
            // 
            // txtRate
            // 
            this.txtRate.Location = new System.Drawing.Point(116, 22);
            this.txtRate.Name = "txtRate";
            this.txtRate.ReadOnly = true;
            this.txtRate.Size = new System.Drawing.Size(53, 20);
            this.txtRate.TabIndex = 12;
            // 
            // chkFees
            // 
            this.chkFees.AutoSize = true;
            this.chkFees.Enabled = false;
            this.chkFees.Location = new System.Drawing.Point(16, 84);
            this.chkFees.Name = "chkFees";
            this.chkFees.Size = new System.Drawing.Size(98, 17);
            this.chkFees.TabIndex = 9;
            this.chkFees.Text = "Regulatory Fee";
            this.chkFees.UseVisualStyleBackColor = true;
            // 
            // chkTax
            // 
            this.chkTax.AutoSize = true;
            this.chkTax.Enabled = false;
            this.chkTax.Location = new System.Drawing.Point(16, 57);
            this.chkTax.Name = "chkTax";
            this.chkTax.Size = new System.Drawing.Size(91, 17);
            this.chkTax.TabIndex = 7;
            this.chkTax.Text = "Business Line";
            this.chkTax.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(192, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "%";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(222, 445);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnDelete.Size = new System.Drawing.Size(88, 25);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Values.ExtraText = "";
            this.btnDelete.Values.Image = null;
            this.btnDelete.Values.ImageStates.ImageCheckedNormal = null;
            this.btnDelete.Values.ImageStates.ImageCheckedPressed = null;
            this.btnDelete.Values.ImageStates.ImageCheckedTracking = null;
            this.btnDelete.Values.Text = "Delete";
            // 
            // cmbFees
            // 
            this.cmbFees.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbFees.FormattingEnabled = true;
            this.cmbFees.Location = new System.Drawing.Point(132, 82);
            this.cmbFees.Name = "cmbFees";
            this.cmbFees.Size = new System.Drawing.Size(234, 21);
            this.cmbFees.TabIndex = 10;
            this.cmbFees.SelectedValueChanged += new System.EventHandler(this.cmbFees_SelectedValueChanged);
            // 
            // cmbTax
            // 
            this.cmbTax.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbTax.FormattingEnabled = true;
            this.cmbTax.Location = new System.Drawing.Point(132, 55);
            this.cmbTax.Name = "cmbTax";
            this.cmbTax.Size = new System.Drawing.Size(234, 21);
            this.cmbTax.TabIndex = 8;
            this.cmbTax.SelectedValueChanged += new System.EventHandler(this.cmbTax_SelectedValueChanged);
            // 
            // containerWithShadow3
            // 
            this.containerWithShadow3.Location = new System.Drawing.Point(12, 12);
            this.containerWithShadow3.Name = "containerWithShadow3";
            this.containerWithShadow3.Size = new System.Drawing.Size(412, 478);
            this.containerWithShadow3.TabIndex = 43;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdoDec);
            this.groupBox3.Controls.Add(this.rdoInc);
            this.groupBox3.Location = new System.Drawing.Point(16, 200);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 49);
            this.groupBox3.TabIndex = 58;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = " Computation: ";
            // 
            // frmSplOrd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(436, 497);
            this.ControlBox = false;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.grpApply);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvRevYear);
            this.Controls.Add(this.containerWithShadow3);
            this.Name = "frmSplOrd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Special Ordinance";
            this.Load += new System.EventHandler(this.frmSplOrd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRevYear)).EndInit();
            this.grpApply.ResumeLayout(false);
            this.grpApply.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEdit;
        private System.Windows.Forms.Label lblRevYear;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAdd;
        private System.Windows.Forms.DataGridView dgvRevYear;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEffYear;
        private System.Windows.Forms.ComboBox cmbRevYear;
        private System.Windows.Forms.GroupBox grpApply;
        private System.Windows.Forms.CheckBox chkFees;
        private System.Windows.Forms.CheckBox chkTax;
        private System.Windows.Forms.TextBox txtRate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rdoAmt;
        private System.Windows.Forms.RadioButton rdoRate;
        private System.Windows.Forms.RadioButton rdoAnnual;
        private System.Windows.Forms.RadioButton rdoQuarter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAmt;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoMonth;
        private System.Windows.Forms.RadioButton rdoDec;
        private System.Windows.Forms.RadioButton rdoInc;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDelete;
        private MultiColumnComboBoxDemo.MultiColumnComboBox cmbFees;
        private MultiColumnComboBoxDemo.MultiColumnComboBox cmbTax;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}