namespace Amellar.Common.DeficientRecords
{
    partial class frmDeficientRecordsToolReport
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
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.chkCategory = new System.Windows.Forms.CheckBox();
            this.grpCriteria = new System.Windows.Forms.GroupBox();
            this.txtType = new System.Windows.Forms.TextBox();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.chkSubType = new System.Windows.Forms.CheckBox();
            this.chkType = new System.Windows.Forms.CheckBox();
            this.grpCriteria.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(250, 107);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(331, 107);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkCategory
            // 
            this.chkCategory.AutoSize = true;
            this.chkCategory.Location = new System.Drawing.Point(6, 19);
            this.chkCategory.Name = "chkCategory";
            this.chkCategory.Size = new System.Drawing.Size(68, 17);
            this.chkCategory.TabIndex = 1;
            this.chkCategory.Text = "Category";
            this.chkCategory.UseVisualStyleBackColor = true;
            this.chkCategory.CheckedChanged += new System.EventHandler(this.chkCategory_CheckedChanged);
            // 
            // grpCriteria
            // 
            this.grpCriteria.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpCriteria.Controls.Add(this.txtType);
            this.grpCriteria.Controls.Add(this.txtCategory);
            this.grpCriteria.Controls.Add(this.cmbType);
            this.grpCriteria.Controls.Add(this.cmbCategory);
            this.grpCriteria.Controls.Add(this.chkSubType);
            this.grpCriteria.Controls.Add(this.chkType);
            this.grpCriteria.Controls.Add(this.chkCategory);
            this.grpCriteria.Location = new System.Drawing.Point(12, 4);
            this.grpCriteria.Name = "grpCriteria";
            this.grpCriteria.Size = new System.Drawing.Size(394, 97);
            this.grpCriteria.TabIndex = 2;
            this.grpCriteria.TabStop = false;
            // 
            // txtType
            // 
            this.txtType.Location = new System.Drawing.Point(347, 44);
            this.txtType.Name = "txtType";
            this.txtType.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(41, 20);
            this.txtType.TabIndex = 3;
            // 
            // txtCategory
            // 
            this.txtCategory.Location = new System.Drawing.Point(347, 19);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.ReadOnly = true;
            this.txtCategory.Size = new System.Drawing.Size(41, 20);
            this.txtCategory.TabIndex = 3;
            // 
            // cmbType
            // 
            this.cmbType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(86, 44);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(254, 21);
            this.cmbType.TabIndex = 2;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // cmbCategory
            // 
            this.cmbCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(86, 17);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(254, 21);
            this.cmbCategory.TabIndex = 2;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // chkSubType
            // 
            this.chkSubType.AutoSize = true;
            this.chkSubType.Location = new System.Drawing.Point(6, 67);
            this.chkSubType.Name = "chkSubType";
            this.chkSubType.Size = new System.Drawing.Size(69, 17);
            this.chkSubType.TabIndex = 1;
            this.chkSubType.Text = "SubType";
            this.chkSubType.UseVisualStyleBackColor = true;
            this.chkSubType.CheckedChanged += new System.EventHandler(this.chkSubType_CheckedChanged);
            // 
            // chkType
            // 
            this.chkType.AutoSize = true;
            this.chkType.Location = new System.Drawing.Point(6, 44);
            this.chkType.Name = "chkType";
            this.chkType.Size = new System.Drawing.Size(50, 17);
            this.chkType.TabIndex = 1;
            this.chkType.Text = "Type";
            this.chkType.UseVisualStyleBackColor = true;
            this.chkType.CheckedChanged += new System.EventHandler(this.chkType_CheckedChanged);
            // 
            // frmDeficientRecordsToolReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 141);
            this.Controls.Add(this.grpCriteria);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(428, 177);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(428, 177);
            this.Name = "frmDeficientRecordsToolReport";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Deficient Records";
            this.grpCriteria.ResumeLayout(false);
            this.grpCriteria.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chkCategory;
        private System.Windows.Forms.GroupBox grpCriteria;
        private System.Windows.Forms.CheckBox chkSubType;
        private System.Windows.Forms.CheckBox chkType;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.TextBox txtCategory;
    }
}