namespace Amellar.Modules.Utilities
{
    partial class frmDefaultValues
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
            this.btnApplyToAll = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvBnsFee = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.rdoNew = new System.Windows.Forms.RadioButton();
            this.rdoRenewal = new System.Windows.Forms.RadioButton();
            this.rdoRetired = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAdd = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCheckAllX = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnUnCheckAllX = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCheckAllY = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnUnCheckAllY = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtBnsTypeCode = new System.Windows.Forms.TextBox();
            this.cmbBnsType = new MultiColumnComboBoxDemo.MultiColumnComboBox();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBnsFee)).BeginInit();
            this.SuspendLayout();
            // 
            // btnApplyToAll
            // 
            this.btnApplyToAll.Location = new System.Drawing.Point(397, 41);
            this.btnApplyToAll.Name = "btnApplyToAll";
            this.btnApplyToAll.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnApplyToAll.Size = new System.Drawing.Size(217, 25);
            this.btnApplyToAll.TabIndex = 6;
            this.btnApplyToAll.Text = "Apply check to all business category";
            this.btnApplyToAll.Values.ExtraText = "";
            this.btnApplyToAll.Values.Image = null;
            this.btnApplyToAll.Values.ImageStates.ImageCheckedNormal = null;
            this.btnApplyToAll.Values.ImageStates.ImageCheckedPressed = null;
            this.btnApplyToAll.Values.ImageStates.ImageCheckedTracking = null;
            this.btnApplyToAll.Values.Text = "Apply check to all business category";
            this.btnApplyToAll.Click += new System.EventHandler(this.btnApplyToAll_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Business Category:";
            // 
            // dgvBnsFee
            // 
            this.dgvBnsFee.AllowUserToAddRows = false;
            this.dgvBnsFee.AllowUserToResizeColumns = false;
            this.dgvBnsFee.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBnsFee.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBnsFee.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBnsFee.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvBnsFee.Location = new System.Drawing.Point(28, 123);
            this.dgvBnsFee.Name = "dgvBnsFee";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBnsFee.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvBnsFee.Size = new System.Drawing.Size(586, 257);
            this.dgvBnsFee.TabIndex = 19;
            this.dgvBnsFee.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBnsFee_CellContentClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(232, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Selected Schedule for Billing";
            // 
            // rdoNew
            // 
            this.rdoNew.AutoSize = true;
            this.rdoNew.Location = new System.Drawing.Point(43, 391);
            this.rdoNew.Name = "rdoNew";
            this.rdoNew.Size = new System.Drawing.Size(92, 17);
            this.rdoNew.TabIndex = 21;
            this.rdoNew.TabStop = true;
            this.rdoNew.Text = "New Business";
            this.rdoNew.UseVisualStyleBackColor = true;
            this.rdoNew.CheckedChanged += new System.EventHandler(this.rdoNew_CheckedChanged);
            // 
            // rdoRenewal
            // 
            this.rdoRenewal.AutoSize = true;
            this.rdoRenewal.Location = new System.Drawing.Point(43, 414);
            this.rdoRenewal.Name = "rdoRenewal";
            this.rdoRenewal.Size = new System.Drawing.Size(112, 17);
            this.rdoRenewal.TabIndex = 22;
            this.rdoRenewal.TabStop = true;
            this.rdoRenewal.Text = "Renewal Business";
            this.rdoRenewal.UseVisualStyleBackColor = true;
            this.rdoRenewal.CheckedChanged += new System.EventHandler(this.rdoRenewal_CheckedChanged);
            // 
            // rdoRetired
            // 
            this.rdoRetired.AutoSize = true;
            this.rdoRetired.Location = new System.Drawing.Point(43, 437);
            this.rdoRetired.Name = "rdoRetired";
            this.rdoRetired.Size = new System.Drawing.Size(104, 17);
            this.rdoRetired.TabIndex = 23;
            this.rdoRetired.TabStop = true;
            this.rdoRetired.Text = "Retired Business";
            this.rdoRetired.UseVisualStyleBackColor = true;
            this.rdoRetired.CheckedChanged += new System.EventHandler(this.rdoRetired_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(294, 390);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "LEGEND: ";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(526, 473);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(88, 25);
            this.btnClose.TabIndex = 26;
            this.btnClose.Text = "Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(432, 473);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnAdd.Size = new System.Drawing.Size(88, 25);
            this.btnAdd.TabIndex = 25;
            this.btnAdd.Text = "Update";
            this.btnAdd.Values.ExtraText = "";
            this.btnAdd.Values.Image = null;
            this.btnAdd.Values.ImageStates.ImageCheckedNormal = null;
            this.btnAdd.Values.ImageStates.ImageCheckedPressed = null;
            this.btnAdd.Values.ImageStates.ImageCheckedTracking = null;
            this.btnAdd.Values.Text = "Update";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(303, 441);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Y - Default Value";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(303, 410);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "X - Available Schedule";
            // 
            // btnCheckAllX
            // 
            this.btnCheckAllX.Location = new System.Drawing.Point(423, 398);
            this.btnCheckAllX.Name = "btnCheckAllX";
            this.btnCheckAllX.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCheckAllX.Size = new System.Drawing.Size(88, 25);
            this.btnCheckAllX.TabIndex = 25;
            this.btnCheckAllX.Text = "Check All";
            this.btnCheckAllX.Values.ExtraText = "";
            this.btnCheckAllX.Values.Image = null;
            this.btnCheckAllX.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCheckAllX.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCheckAllX.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCheckAllX.Values.Text = "Check All";
            this.btnCheckAllX.Click += new System.EventHandler(this.btnCheckAllX_Click);
            // 
            // btnUnCheckAllX
            // 
            this.btnUnCheckAllX.Location = new System.Drawing.Point(517, 398);
            this.btnUnCheckAllX.Name = "btnUnCheckAllX";
            this.btnUnCheckAllX.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnUnCheckAllX.Size = new System.Drawing.Size(88, 25);
            this.btnUnCheckAllX.TabIndex = 25;
            this.btnUnCheckAllX.Text = "Uncheck All";
            this.btnUnCheckAllX.Values.ExtraText = "";
            this.btnUnCheckAllX.Values.Image = null;
            this.btnUnCheckAllX.Values.ImageStates.ImageCheckedNormal = null;
            this.btnUnCheckAllX.Values.ImageStates.ImageCheckedPressed = null;
            this.btnUnCheckAllX.Values.ImageStates.ImageCheckedTracking = null;
            this.btnUnCheckAllX.Values.Text = "Uncheck All";
            this.btnUnCheckAllX.Click += new System.EventHandler(this.btnUnCheckAllX_Click);
            // 
            // btnCheckAllY
            // 
            this.btnCheckAllY.Location = new System.Drawing.Point(423, 429);
            this.btnCheckAllY.Name = "btnCheckAllY";
            this.btnCheckAllY.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCheckAllY.Size = new System.Drawing.Size(88, 25);
            this.btnCheckAllY.TabIndex = 25;
            this.btnCheckAllY.Text = "Check All";
            this.btnCheckAllY.Values.ExtraText = "";
            this.btnCheckAllY.Values.Image = null;
            this.btnCheckAllY.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCheckAllY.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCheckAllY.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCheckAllY.Values.Text = "Check All";
            this.btnCheckAllY.Click += new System.EventHandler(this.btnCheckAllY_Click);
            // 
            // btnUnCheckAllY
            // 
            this.btnUnCheckAllY.Location = new System.Drawing.Point(517, 429);
            this.btnUnCheckAllY.Name = "btnUnCheckAllY";
            this.btnUnCheckAllY.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnUnCheckAllY.Size = new System.Drawing.Size(88, 25);
            this.btnUnCheckAllY.TabIndex = 25;
            this.btnUnCheckAllY.Text = "Uncheck All";
            this.btnUnCheckAllY.Values.ExtraText = "";
            this.btnUnCheckAllY.Values.Image = null;
            this.btnUnCheckAllY.Values.ImageStates.ImageCheckedNormal = null;
            this.btnUnCheckAllY.Values.ImageStates.ImageCheckedPressed = null;
            this.btnUnCheckAllY.Values.ImageStates.ImageCheckedTracking = null;
            this.btnUnCheckAllY.Values.Text = "Uncheck All";
            this.btnUnCheckAllY.Click += new System.EventHandler(this.btnUnCheckAllY_Click);
            // 
            // txtBnsTypeCode
            // 
            this.txtBnsTypeCode.Location = new System.Drawing.Point(128, 46);
            this.txtBnsTypeCode.Name = "txtBnsTypeCode";
            this.txtBnsTypeCode.ReadOnly = true;
            this.txtBnsTypeCode.Size = new System.Drawing.Size(53, 20);
            this.txtBnsTypeCode.TabIndex = 28;
            // 
            // cmbBnsType
            // 
            this.cmbBnsType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbBnsType.FormattingEnabled = true;
            this.cmbBnsType.Location = new System.Drawing.Point(187, 45);
            this.cmbBnsType.Name = "cmbBnsType";
            this.cmbBnsType.Size = new System.Drawing.Size(203, 21);
            this.cmbBnsType.TabIndex = 27;
            this.cmbBnsType.SelectedValueChanged += new System.EventHandler(this.cmbBnsType_SelectedValueChanged);
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(12, 84);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(614, 386);
            this.containerWithShadow2.TabIndex = 0;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(12, 12);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(614, 74);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(227, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(181, 13);
            this.label6.TabIndex = 32;
            this.label6.Text = "Fees and Business Description";
            // 
            // frmDefaultValues
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 502);
            this.ControlBox = false;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtBnsTypeCode);
            this.Controls.Add(this.cmbBnsType);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnUnCheckAllY);
            this.Controls.Add(this.btnUnCheckAllX);
            this.Controls.Add(this.btnCheckAllY);
            this.Controls.Add(this.btnCheckAllX);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rdoRetired);
            this.Controls.Add(this.rdoRenewal);
            this.Controls.Add(this.rdoNew);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvBnsFee);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnApplyToAll);
            this.Controls.Add(this.containerWithShadow2);
            this.Controls.Add(this.containerWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDefaultValues";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Default Value";
            this.Load += new System.EventHandler(this.frmDefaultValues_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBnsFee)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnApplyToAll;
        private System.Windows.Forms.Label label1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private System.Windows.Forms.DataGridView dgvBnsFee;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdoNew;
        private System.Windows.Forms.RadioButton rdoRenewal;
        private System.Windows.Forms.RadioButton rdoRetired;
        private System.Windows.Forms.Label label3;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAdd;
        private MultiColumnComboBoxDemo.MultiColumnComboBox cmbBnsType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCheckAllX;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnUnCheckAllX;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCheckAllY;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnUnCheckAllY;
        private System.Windows.Forms.TextBox txtBnsTypeCode;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.Label label6;
    }
}