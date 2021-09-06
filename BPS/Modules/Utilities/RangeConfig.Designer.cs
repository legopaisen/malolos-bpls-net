namespace Amellar.Modules.Utilities
{
    partial class frmRangeConfig
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
            this.containerWithShadow3 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.txtBnsCode = new System.Windows.Forms.TextBox();
            this.txtFeesCode = new System.Windows.Forms.TextBox();
            this.lblFeesCode = new System.Windows.Forms.Label();
            this.lblBnsCode = new System.Windows.Forms.Label();
            this.chkGross = new System.Windows.Forms.CheckBox();
            this.chkTaxPaid = new System.Windows.Forms.CheckBox();
            this.chkTaxDue = new System.Windows.Forms.CheckBox();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.dgvFees = new System.Windows.Forms.DataGridView();
            this.chkSwitch = new System.Windows.Forms.CheckBox();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAdd = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblRevYear = new System.Windows.Forms.Label();
            this.txtRevYear = new System.Windows.Forms.TextBox();
            this.btnConfig = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFees)).BeginInit();
            this.SuspendLayout();
            // 
            // containerWithShadow3
            // 
            this.containerWithShadow3.Location = new System.Drawing.Point(13, 12);
            this.containerWithShadow3.Name = "containerWithShadow3";
            this.containerWithShadow3.Size = new System.Drawing.Size(405, 50);
            this.containerWithShadow3.TabIndex = 19;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(13, 58);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(405, 50);
            this.containerWithShadow1.TabIndex = 19;
            // 
            // txtBnsCode
            // 
            this.txtBnsCode.Location = new System.Drawing.Point(352, 25);
            this.txtBnsCode.Name = "txtBnsCode";
            this.txtBnsCode.ReadOnly = true;
            this.txtBnsCode.Size = new System.Drawing.Size(54, 20);
            this.txtBnsCode.TabIndex = 20;
            // 
            // txtFeesCode
            // 
            this.txtFeesCode.Location = new System.Drawing.Point(212, 25);
            this.txtFeesCode.Name = "txtFeesCode";
            this.txtFeesCode.ReadOnly = true;
            this.txtFeesCode.Size = new System.Drawing.Size(54, 20);
            this.txtFeesCode.TabIndex = 0;
            // 
            // lblFeesCode
            // 
            this.lblFeesCode.AutoSize = true;
            this.lblFeesCode.Location = new System.Drawing.Point(149, 28);
            this.lblFeesCode.Name = "lblFeesCode";
            this.lblFeesCode.Size = new System.Drawing.Size(58, 13);
            this.lblFeesCode.TabIndex = 21;
            this.lblFeesCode.Text = "Fees Code";
            this.lblFeesCode.Click += new System.EventHandler(this.lblFeesCode_Click);
            // 
            // lblBnsCode
            // 
            this.lblBnsCode.AutoSize = true;
            this.lblBnsCode.Location = new System.Drawing.Point(272, 28);
            this.lblBnsCode.Name = "lblBnsCode";
            this.lblBnsCode.Size = new System.Drawing.Size(77, 13);
            this.lblBnsCode.TabIndex = 21;
            this.lblBnsCode.Text = "Business Code";
            // 
            // chkGross
            // 
            this.chkGross.AutoSize = true;
            this.chkGross.Location = new System.Drawing.Point(30, 73);
            this.chkGross.Name = "chkGross";
            this.chkGross.Size = new System.Drawing.Size(138, 17);
            this.chkGross.TabIndex = 0;
            this.chkGross.Text = "Based on Gross/Capital";
            this.chkGross.UseVisualStyleBackColor = true;
            this.chkGross.CheckStateChanged += new System.EventHandler(this.chkGross_CheckStateChanged);
            // 
            // chkTaxPaid
            // 
            this.chkTaxPaid.AutoSize = true;
            this.chkTaxPaid.Location = new System.Drawing.Point(174, 73);
            this.chkTaxPaid.Name = "chkTaxPaid";
            this.chkTaxPaid.Size = new System.Drawing.Size(116, 17);
            this.chkTaxPaid.TabIndex = 1;
            this.chkTaxPaid.Text = "Based on Tax Paid";
            this.chkTaxPaid.UseVisualStyleBackColor = true;
            this.chkTaxPaid.CheckStateChanged += new System.EventHandler(this.chkTaxPaid_CheckStateChanged);
            // 
            // chkTaxDue
            // 
            this.chkTaxDue.AutoSize = true;
            this.chkTaxDue.Location = new System.Drawing.Point(296, 73);
            this.chkTaxDue.Name = "chkTaxDue";
            this.chkTaxDue.Size = new System.Drawing.Size(115, 17);
            this.chkTaxDue.TabIndex = 2;
            this.chkTaxDue.Text = "Based on Tax Due";
            this.chkTaxDue.UseVisualStyleBackColor = true;
            this.chkTaxDue.CheckStateChanged += new System.EventHandler(this.chkTaxDue_CheckStateChanged);
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(12, 105);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(405, 267);
            this.containerWithShadow2.TabIndex = 19;
            // 
            // dgvFees
            // 
            this.dgvFees.AllowUserToAddRows = false;
            this.dgvFees.AllowUserToDeleteRows = false;
            this.dgvFees.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFees.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvFees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFees.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvFees.Location = new System.Drawing.Point(25, 118);
            this.dgvFees.Name = "dgvFees";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFees.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvFees.Size = new System.Drawing.Size(381, 235);
            this.dgvFees.TabIndex = 23;
            this.dgvFees.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvFees_CellBeginEdit);
            this.dgvFees.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFees_CellEndEdit);
            // 
            // chkSwitch
            // 
            this.chkSwitch.AutoSize = true;
            this.chkSwitch.Location = new System.Drawing.Point(13, 378);
            this.chkSwitch.Name = "chkSwitch";
            this.chkSwitch.Size = new System.Drawing.Size(162, 17);
            this.chkSwitch.TabIndex = 3;
            this.chkSwitch.Text = "Compute whichever is higher";
            this.chkSwitch.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(345, 378);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(72, 25);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Cancel";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "Cancel";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(267, 378);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnAdd.Size = new System.Drawing.Size(72, 25);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Save";
            this.btnAdd.Values.ExtraText = "";
            this.btnAdd.Values.Image = null;
            this.btnAdd.Values.ImageStates.ImageCheckedNormal = null;
            this.btnAdd.Values.ImageStates.ImageCheckedPressed = null;
            this.btnAdd.Values.ImageStates.ImageCheckedTracking = null;
            this.btnAdd.Values.Text = "Save";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblRevYear
            // 
            this.lblRevYear.AutoSize = true;
            this.lblRevYear.Location = new System.Drawing.Point(27, 28);
            this.lblRevYear.Name = "lblRevYear";
            this.lblRevYear.Size = new System.Drawing.Size(55, 13);
            this.lblRevYear.TabIndex = 21;
            this.lblRevYear.Text = "Rev. Year";
            // 
            // txtRevYear
            // 
            this.txtRevYear.Location = new System.Drawing.Point(86, 25);
            this.txtRevYear.Name = "txtRevYear";
            this.txtRevYear.ReadOnly = true;
            this.txtRevYear.Size = new System.Drawing.Size(54, 20);
            this.txtRevYear.TabIndex = 0;
            // 
            // btnConfig
            // 
            this.btnConfig.Location = new System.Drawing.Point(174, 378);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnConfig.Size = new System.Drawing.Size(72, 25);
            this.btnConfig.TabIndex = 4;
            this.btnConfig.Text = "Config";
            this.btnConfig.Values.ExtraText = "";
            this.btnConfig.Values.Image = null;
            this.btnConfig.Values.ImageStates.ImageCheckedNormal = null;
            this.btnConfig.Values.ImageStates.ImageCheckedPressed = null;
            this.btnConfig.Values.ImageStates.ImageCheckedTracking = null;
            this.btnConfig.Values.Text = "Config";
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // frmRangeConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 412);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.chkSwitch);
            this.Controls.Add(this.dgvFees);
            this.Controls.Add(this.chkTaxDue);
            this.Controls.Add(this.chkTaxPaid);
            this.Controls.Add(this.chkGross);
            this.Controls.Add(this.lblBnsCode);
            this.Controls.Add(this.lblRevYear);
            this.Controls.Add(this.txtRevYear);
            this.Controls.Add(this.lblFeesCode);
            this.Controls.Add(this.txtFeesCode);
            this.Controls.Add(this.txtBnsCode);
            this.Controls.Add(this.containerWithShadow2);
            this.Controls.Add(this.containerWithShadow1);
            this.Controls.Add(this.containerWithShadow3);
            this.Name = "frmRangeConfig";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rate Range Configuration";
            this.Load += new System.EventHandler(this.frmRangeConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFees)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow3;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.TextBox txtBnsCode;
        private System.Windows.Forms.TextBox txtFeesCode;
        private System.Windows.Forms.Label lblFeesCode;
        private System.Windows.Forms.Label lblBnsCode;
        private System.Windows.Forms.CheckBox chkGross;
        private System.Windows.Forms.CheckBox chkTaxPaid;
        private System.Windows.Forms.CheckBox chkTaxDue;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private System.Windows.Forms.DataGridView dgvFees;
        private System.Windows.Forms.CheckBox chkSwitch;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAdd;
        private System.Windows.Forms.Label lblRevYear;
        private System.Windows.Forms.TextBox txtRevYear;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnConfig;
    }
}