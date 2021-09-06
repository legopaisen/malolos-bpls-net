namespace Amellar.Modules.Utilities
{
    partial class frmQuarterConfig
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
            this.lblBnsCode = new System.Windows.Forms.Label();
            this.lblRevYear = new System.Windows.Forms.Label();
            this.txtRevYear = new System.Windows.Forms.TextBox();
            this.lblFeesCode = new System.Windows.Forms.Label();
            this.txtFeesCode = new System.Windows.Forms.TextBox();
            this.txtBnsCode = new System.Windows.Forms.TextBox();
            this.dgvFees = new System.Windows.Forms.DataGridView();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAdd = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtDataType = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbCompare = new System.Windows.Forms.ComboBox();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow3 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFees)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBnsCode
            // 
            this.lblBnsCode.AutoSize = true;
            this.lblBnsCode.Location = new System.Drawing.Point(269, 28);
            this.lblBnsCode.Name = "lblBnsCode";
            this.lblBnsCode.Size = new System.Drawing.Size(77, 13);
            this.lblBnsCode.TabIndex = 26;
            this.lblBnsCode.Text = "Business Code";
            // 
            // lblRevYear
            // 
            this.lblRevYear.AutoSize = true;
            this.lblRevYear.Location = new System.Drawing.Point(26, 28);
            this.lblRevYear.Name = "lblRevYear";
            this.lblRevYear.Size = new System.Drawing.Size(55, 13);
            this.lblRevYear.TabIndex = 27;
            this.lblRevYear.Text = "Rev. Year";
            // 
            // txtRevYear
            // 
            this.txtRevYear.Location = new System.Drawing.Point(85, 25);
            this.txtRevYear.Name = "txtRevYear";
            this.txtRevYear.ReadOnly = true;
            this.txtRevYear.Size = new System.Drawing.Size(54, 20);
            this.txtRevYear.TabIndex = 0;
            // 
            // lblFeesCode
            // 
            this.lblFeesCode.AutoSize = true;
            this.lblFeesCode.Location = new System.Drawing.Point(146, 28);
            this.lblFeesCode.Name = "lblFeesCode";
            this.lblFeesCode.Size = new System.Drawing.Size(58, 13);
            this.lblFeesCode.TabIndex = 28;
            this.lblFeesCode.Text = "Fees Code";
            // 
            // txtFeesCode
            // 
            this.txtFeesCode.Location = new System.Drawing.Point(209, 25);
            this.txtFeesCode.Name = "txtFeesCode";
            this.txtFeesCode.ReadOnly = true;
            this.txtFeesCode.Size = new System.Drawing.Size(54, 20);
            this.txtFeesCode.TabIndex = 0;
            // 
            // txtBnsCode
            // 
            this.txtBnsCode.Location = new System.Drawing.Point(349, 25);
            this.txtBnsCode.Name = "txtBnsCode";
            this.txtBnsCode.ReadOnly = true;
            this.txtBnsCode.Size = new System.Drawing.Size(54, 20);
            this.txtBnsCode.TabIndex = 0;
            this.txtBnsCode.TextChanged += new System.EventHandler(this.txtBnsCode_TextChanged);
            // 
            // dgvFees
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFees.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFees.Location = new System.Drawing.Point(25, 73);
            this.dgvFees.Name = "dgvFees";
            this.dgvFees.RowHeadersVisible = false;
            this.dgvFees.Size = new System.Drawing.Size(448, 176);
            this.dgvFees.TabIndex = 1;
            this.dgvFees.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFees_CellEndEdit);
            this.dgvFees.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvFees_KeyDown);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(385, 276);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(88, 25);
            this.btnClose.TabIndex = 3;
            this.btnClose.Values.Text = "Cancel";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(291, 276);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnAdd.Size = new System.Drawing.Size(88, 25);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Values.Text = "Save";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtDataType
            // 
            this.txtDataType.Location = new System.Drawing.Point(443, 25);
            this.txtDataType.Name = "txtDataType";
            this.txtDataType.ReadOnly = true;
            this.txtDataType.Size = new System.Drawing.Size(30, 20);
            this.txtDataType.TabIndex = 0;
            this.txtDataType.TextChanged += new System.EventHandler(this.txtBnsCode_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(408, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 280);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Compare whichever is higher";
            // 
            // cmbCompare
            // 
            this.cmbCompare.FormattingEnabled = true;
            this.cmbCompare.Location = new System.Drawing.Point(171, 276);
            this.cmbCompare.Name = "cmbCompare";
            this.cmbCompare.Size = new System.Drawing.Size(76, 21);
            this.cmbCompare.TabIndex = 31;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(12, 60);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(474, 210);
            this.containerWithShadow1.TabIndex = 29;
            // 
            // containerWithShadow3
            // 
            this.containerWithShadow3.Location = new System.Drawing.Point(12, 12);
            this.containerWithShadow3.Name = "containerWithShadow3";
            this.containerWithShadow3.Size = new System.Drawing.Size(474, 50);
            this.containerWithShadow3.TabIndex = 24;
            // 
            // frmQuarterConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 307);
            this.Controls.Add(this.cmbCompare);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvFees);
            this.Controls.Add(this.containerWithShadow1);
            this.Controls.Add(this.lblBnsCode);
            this.Controls.Add(this.lblRevYear);
            this.Controls.Add(this.txtRevYear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblFeesCode);
            this.Controls.Add(this.txtFeesCode);
            this.Controls.Add(this.txtDataType);
            this.Controls.Add(this.txtBnsCode);
            this.Controls.Add(this.containerWithShadow3);
            this.Name = "frmQuarterConfig";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure Quarterly Fee";
            this.Load += new System.EventHandler(this.frmQuarterConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFees)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBnsCode;
        private System.Windows.Forms.Label lblRevYear;
        private System.Windows.Forms.TextBox txtRevYear;
        private System.Windows.Forms.Label lblFeesCode;
        private System.Windows.Forms.TextBox txtFeesCode;
        private System.Windows.Forms.TextBox txtBnsCode;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow3;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.DataGridView dgvFees;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAdd;
        private System.Windows.Forms.TextBox txtDataType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbCompare;
    }
}