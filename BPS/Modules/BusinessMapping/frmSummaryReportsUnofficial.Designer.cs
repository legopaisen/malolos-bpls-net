namespace Amellar.Modules.BusinessMapping
{
    partial class frmSummaryReportsUnofficial
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
            this.cmbBrgy = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnNotice = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.btnPrintList = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.containerWithShadow3 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCnt = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbBrgy
            // 
            this.cmbBrgy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBrgy.FormattingEnabled = true;
            this.cmbBrgy.Location = new System.Drawing.Point(181, 25);
            this.cmbBrgy.Name = "cmbBrgy";
            this.cmbBrgy.Size = new System.Drawing.Size(170, 21);
            this.cmbBrgy.TabIndex = 1;
            this.cmbBrgy.SelectedIndexChanged += new System.EventHandler(this.cmbBrgy_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(110, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Barangay";
            // 
            // btnNotice
            // 
            this.btnNotice.Location = new System.Drawing.Point(124, 478);
            this.btnNotice.Name = "btnNotice";
            this.btnNotice.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnNotice.Size = new System.Drawing.Size(92, 25);
            this.btnNotice.TabIndex = 40;
            this.btnNotice.Values.Text = "Print Notice";
            this.btnNotice.Click += new System.EventHandler(this.btnNotice_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(442, 478);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(92, 25);
            this.btnClose.TabIndex = 40;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
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
            this.dgvList.Location = new System.Drawing.Point(26, 81);
            this.dgvList.Name = "dgvList";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.Size = new System.Drawing.Size(508, 347);
            this.dgvList.TabIndex = 42;
            this.dgvList.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_RowEnter);
            // 
            // btnPrintList
            // 
            this.btnPrintList.Location = new System.Drawing.Point(26, 478);
            this.btnPrintList.Name = "btnPrintList";
            this.btnPrintList.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrintList.Size = new System.Drawing.Size(92, 25);
            this.btnPrintList.TabIndex = 40;
            this.btnPrintList.Values.Text = "Print List";
            this.btnPrintList.Click += new System.EventHandler(this.btnPrintList_Click);
            // 
            // containerWithShadow3
            // 
            this.containerWithShadow3.Location = new System.Drawing.Point(12, 65);
            this.containerWithShadow3.Name = "containerWithShadow3";
            this.containerWithShadow3.Size = new System.Drawing.Size(533, 402);
            this.containerWithShadow3.TabIndex = 0;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(12, 8);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(533, 58);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(387, 436);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 44;
            this.label1.Text = "Total Count";
            // 
            // lblCnt
            // 
            this.lblCnt.AutoSize = true;
            this.lblCnt.Location = new System.Drawing.Point(466, 437);
            this.lblCnt.Name = "lblCnt";
            this.lblCnt.Size = new System.Drawing.Size(10, 13);
            this.lblCnt.TabIndex = 44;
            this.lblCnt.Text = " ";
            // 
            // frmSummaryReportsUnofficial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 511);
            this.ControlBox = false;
            this.Controls.Add(this.lblCnt);
            this.Controls.Add(this.btnNotice);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrintList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbBrgy);
            this.Controls.Add(this.containerWithShadow3);
            this.Controls.Add(this.containerWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmSummaryReportsUnofficial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Business Mapping Report";
            this.Load += new System.EventHandler(this.frmSummaryReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.Label label2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnNotice;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow3;
        public System.Windows.Forms.ComboBox cmbBrgy;
        private System.Windows.Forms.DataGridView dgvList;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPrintList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCnt;
    }
}