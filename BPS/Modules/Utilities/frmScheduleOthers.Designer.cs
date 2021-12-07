namespace Amellar.Modules.Utilities
{
    partial class frmScheduleOthers
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEdit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.dgvListOthers = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.label1 = new System.Windows.Forms.Label();
            this.chkFire = new System.Windows.Forms.CheckBox();
            this.chkInt = new System.Windows.Forms.CheckBox();
            this.lbl1 = new System.Windows.Forms.Label();
            this.chkSurch = new System.Windows.Forms.CheckBox();
            this.lbl2 = new System.Windows.Forms.Label();
            this.chkQtr = new System.Windows.Forms.CheckBox();
            this.lbl3 = new System.Windows.Forms.Label();
            this.chkRate = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkAddl = new System.Windows.Forms.CheckBox();
            this.txtRate = new System.Windows.Forms.TextBox();
            this.containerWithShadow3 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.label3 = new System.Windows.Forms.Label();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.lblHeader = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.brgycode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMinFee = new System.Windows.Forms.TextBox();
            this.cmbBrgy = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btned = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.chkCTC = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListOthers)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(593, 378);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(88, 25);
            this.btnClose.TabIndex = 27;
            this.btnClose.Text = "&Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(499, 378);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnEdit.Size = new System.Drawing.Size(88, 25);
            this.btnEdit.TabIndex = 26;
            this.btnEdit.Text = "&Update";
            this.btnEdit.Values.ExtraText = "";
            this.btnEdit.Values.Image = null;
            this.btnEdit.Values.ImageStates.ImageCheckedNormal = null;
            this.btnEdit.Values.ImageStates.ImageCheckedPressed = null;
            this.btnEdit.Values.ImageStates.ImageCheckedTracking = null;
            this.btnEdit.Values.Text = "&Update";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // dgvListOthers
            // 
            this.dgvListOthers.AllowUserToAddRows = false;
            this.dgvListOthers.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvListOthers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvListOthers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvListOthers.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvListOthers.Location = new System.Drawing.Point(198, 19);
            this.dgvListOthers.Name = "dgvListOthers";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvListOthers.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvListOthers.Size = new System.Drawing.Size(483, 288);
            this.dgvListOthers.TabIndex = 28;
            this.dgvListOthers.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvListOthers_MouseDown);
            this.dgvListOthers.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvListOthers_CellBeginEdit);
            this.dgvListOthers.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListOthers_CellEndEdit);
            this.dgvListOthers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListOthers_CellClick);
            this.dgvListOthers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvListOthers_KeyDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStrip1.Click += new System.EventHandler(this.contextMenuStrip1_Click);
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(182, 3);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(512, 326);
            this.containerWithShadow2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(56, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Rate Base:";
            // 
            // chkFire
            // 
            this.chkFire.AutoSize = true;
            this.chkFire.Location = new System.Drawing.Point(18, 48);
            this.chkFire.Name = "chkFire";
            this.chkFire.Size = new System.Drawing.Size(149, 17);
            this.chkFire.TabIndex = 5;
            this.chkFire.Text = "Fire Safety Inspection Fee";
            this.chkFire.UseVisualStyleBackColor = true;
            this.chkFire.CheckStateChanged += new System.EventHandler(this.chkFire_CheckStateChanged);
            // 
            // chkInt
            // 
            this.chkInt.AutoSize = true;
            this.chkInt.Location = new System.Drawing.Point(29, 232);
            this.chkInt.Name = "chkInt";
            this.chkInt.Size = new System.Drawing.Size(83, 17);
            this.chkInt.TabIndex = 5;
            this.chkInt.Text = "with Interest";
            this.chkInt.UseVisualStyleBackColor = true;
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(11, 35);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(42, 13);
            this.lbl1.TabIndex = 29;
            this.lbl1.Text = "Label 1";
            this.lbl1.Visible = false;
            // 
            // chkSurch
            // 
            this.chkSurch.AutoSize = true;
            this.chkSurch.Location = new System.Drawing.Point(29, 209);
            this.chkSurch.Name = "chkSurch";
            this.chkSurch.Size = new System.Drawing.Size(97, 17);
            this.chkSurch.TabIndex = 5;
            this.chkSurch.Text = "with Surcharge";
            this.chkSurch.UseVisualStyleBackColor = true;
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Location = new System.Drawing.Point(11, 56);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(42, 13);
            this.lbl2.TabIndex = 29;
            this.lbl2.Text = "Label 2";
            this.lbl2.Visible = false;
            // 
            // chkQtr
            // 
            this.chkQtr.AutoSize = true;
            this.chkQtr.Location = new System.Drawing.Point(29, 186);
            this.chkQtr.Name = "chkQtr";
            this.chkQtr.Size = new System.Drawing.Size(68, 17);
            this.chkQtr.TabIndex = 5;
            this.chkQtr.Text = "Quarterly";
            this.chkQtr.UseVisualStyleBackColor = true;
            this.chkQtr.CheckedChanged += new System.EventHandler(this.chkQtr_CheckedChanged);
            // 
            // lbl3
            // 
            this.lbl3.AutoSize = true;
            this.lbl3.Location = new System.Drawing.Point(11, 78);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(42, 13);
            this.lbl3.TabIndex = 29;
            this.lbl3.Text = "Label 3";
            this.lbl3.Visible = false;
            // 
            // chkRate
            // 
            this.chkRate.AutoSize = true;
            this.chkRate.Location = new System.Drawing.Point(29, 150);
            this.chkRate.Name = "chkRate";
            this.chkRate.Size = new System.Drawing.Size(15, 14);
            this.chkRate.TabIndex = 5;
            this.chkRate.UseVisualStyleBackColor = true;
            this.chkRate.CheckStateChanged += new System.EventHandler(this.chkRate_CheckStateChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(93, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "%";
            // 
            // chkAddl
            // 
            this.chkAddl.AutoSize = true;
            this.chkAddl.Location = new System.Drawing.Point(18, 25);
            this.chkAddl.Name = "chkAddl";
            this.chkAddl.Size = new System.Drawing.Size(114, 17);
            this.chkAddl.TabIndex = 5;
            this.chkAddl.Text = "Additional Charges";
            this.chkAddl.UseVisualStyleBackColor = true;
            this.chkAddl.CheckStateChanged += new System.EventHandler(this.chkAddl_CheckStateChanged);
            // 
            // txtRate
            // 
            this.txtRate.Location = new System.Drawing.Point(50, 147);
            this.txtRate.Name = "txtRate";
            this.txtRate.Size = new System.Drawing.Size(39, 20);
            this.txtRate.TabIndex = 30;
            // 
            // containerWithShadow3
            // 
            this.containerWithShadow3.Location = new System.Drawing.Point(9, 116);
            this.containerWithShadow3.Name = "containerWithShadow3";
            this.containerWithShadow3.Size = new System.Drawing.Size(172, 252);
            this.containerWithShadow3.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "-----------------------------";
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(9, 8);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(172, 102);
            this.containerWithShadow1.TabIndex = 2;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(41, 16);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(48, 13);
            this.lblHeader.TabIndex = 29;
            this.lblHeader.Text = "Header";
            this.lblHeader.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkCTC);
            this.panel1.Controls.Add(this.brgycode);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtMinFee);
            this.panel1.Controls.Add(this.cmbBrgy);
            this.panel1.Controls.Add(this.chkSurch);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.chkQtr);
            this.panel1.Controls.Add(this.chkInt);
            this.panel1.Controls.Add(this.txtRate);
            this.panel1.Controls.Add(this.chkFire);
            this.panel1.Controls.Add(this.chkAddl);
            this.panel1.Controls.Add(this.chkRate);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.containerWithShadow3);
            this.panel1.Controls.Add(this.containerWithShadow1);
            this.panel1.Location = new System.Drawing.Point(-5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(197, 399);
            this.panel1.TabIndex = 32;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // brgycode
            // 
            this.brgycode.Enabled = false;
            this.brgycode.Location = new System.Drawing.Point(7, 0);
            this.brgycode.Name = "brgycode";
            this.brgycode.Size = new System.Drawing.Size(60, 20);
            this.brgycode.TabIndex = 36;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 255);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "Minimum Fee";
            // 
            // txtMinFee
            // 
            this.txtMinFee.Location = new System.Drawing.Point(104, 252);
            this.txtMinFee.Name = "txtMinFee";
            this.txtMinFee.Size = new System.Drawing.Size(48, 20);
            this.txtMinFee.TabIndex = 34;
            this.txtMinFee.TextChanged += new System.EventHandler(this.txtMinFee_TextChanged);
            this.txtMinFee.Enter += new System.EventHandler(this.txtMinFee_Enter);
            // 
            // cmbBrgy
            // 
            this.cmbBrgy.FormattingEnabled = true;
            this.cmbBrgy.Location = new System.Drawing.Point(73, 0);
            this.cmbBrgy.Name = "cmbBrgy";
            this.cmbBrgy.Size = new System.Drawing.Size(111, 21);
            this.cmbBrgy.TabIndex = 32;
            this.cmbBrgy.SelectedIndexChanged += new System.EventHandler(this.cmbBrgy_SelectedIndexChanged);
            this.cmbBrgy.SelectedValueChanged += new System.EventHandler(this.cmbBrgy_SelectedValueChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblHeader);
            this.panel2.Controls.Add(this.lbl2);
            this.panel2.Controls.Add(this.lbl1);
            this.panel2.Controls.Add(this.lbl3);
            this.panel2.Location = new System.Drawing.Point(18, 261);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(151, 98);
            this.panel2.TabIndex = 33;
            // 
            // btned
            // 
            this.btned.Enabled = false;
            this.btned.Location = new System.Drawing.Point(405, 378);
            this.btned.Name = "btned";
            this.btned.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btned.Size = new System.Drawing.Size(88, 25);
            this.btned.TabIndex = 33;
            this.btned.Text = "&EDIT";
            this.btned.Values.ExtraText = "";
            this.btned.Values.Image = null;
            this.btned.Values.ImageStates.ImageCheckedNormal = null;
            this.btned.Values.ImageStates.ImageCheckedPressed = null;
            this.btned.Values.ImageStates.ImageCheckedTracking = null;
            this.btned.Values.Text = "&EDIT";
            this.btned.Click += new System.EventHandler(this.btnedclick);
            // 
            // chkCTC
            // 
            this.chkCTC.AutoSize = true;
            this.chkCTC.Location = new System.Drawing.Point(20, 71);
            this.chkCTC.Name = "chkCTC";
            this.chkCTC.Size = new System.Drawing.Size(47, 17);
            this.chkCTC.TabIndex = 37;
            this.chkCTC.Text = "CTC";
            this.chkCTC.UseVisualStyleBackColor = true;
            this.chkCTC.CheckedChanged += new System.EventHandler(this.chkCTC_CheckedChanged);
            // 
            // frmScheduleOthers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(703, 437);
            this.Controls.Add(this.dgvListOthers);
            this.Controls.Add(this.btned);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.containerWithShadow2);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmScheduleOthers";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Other Charges";
            this.Load += new System.EventHandler(this.frmScheduleOthers_Load);
            this.Click += new System.EventHandler(this.frmScheduleOthers_Click);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListOthers)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEdit;
        private System.Windows.Forms.DataGridView dgvListOthers;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkFire;
        private System.Windows.Forms.CheckBox chkInt;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.CheckBox chkSurch;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.CheckBox chkQtr;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.CheckBox chkRate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkAddl;
        private System.Windows.Forms.TextBox txtRate;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow3;
        private System.Windows.Forms.Label label3;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cmbBrgy;
        private System.Windows.Forms.Panel panel2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btned;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMinFee;
        private System.Windows.Forms.TextBox brgycode;
        private System.Windows.Forms.CheckBox chkCTC;
    }
}