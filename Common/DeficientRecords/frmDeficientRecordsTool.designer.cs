namespace Amellar.Common.DeficientRecords
{
    partial class frmDeficientRecordsTool
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
            this.tbpDeficiencyTools = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pnlDeficiencyType = new System.Windows.Forms.Panel();
            this.dgvDeficientTypes = new System.Windows.Forms.DataGridView();
            this.grpAdditionalInfo = new System.Windows.Forms.GroupBox();
            this.dgvAdditionalInfo = new System.Windows.Forms.DataGridView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.grpCategory = new System.Windows.Forms.GroupBox();
            this.txtCategoryCode = new System.Windows.Forms.TextBox();
            this.btnCategoryDelete = new System.Windows.Forms.Button();
            this.btnCategoryEdit = new System.Windows.Forms.Button();
            this.cmbCategoryName = new System.Windows.Forms.ComboBox();
            this.btnCategoryAdd = new System.Windows.Forms.Button();
            this.txtCategoryName = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnAccountEdit = new System.Windows.Forms.Button();
            this.btnAccountCancel = new System.Windows.Forms.Button();
            this.dgvAccountType = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnReqEdit = new System.Windows.Forms.Button();
            this.btnReqCancel = new System.Windows.Forms.Button();
            this.dgvRequiredFields = new System.Windows.Forms.DataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dgvSolutions = new System.Windows.Forms.DataGridView();
            this.tbpDeficiencyTools.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.pnlDeficiencyType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeficientTypes)).BeginInit();
            this.grpAdditionalInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdditionalInfo)).BeginInit();
            this.grpCategory.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountType)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequiredFields)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSolutions)).BeginInit();
            this.SuspendLayout();
            // 
            // tbpDeficiencyTools
            // 
            this.tbpDeficiencyTools.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbpDeficiencyTools.Controls.Add(this.tabPage3);
            this.tbpDeficiencyTools.Controls.Add(this.tabPage1);
            this.tbpDeficiencyTools.Controls.Add(this.tabPage2);
            this.tbpDeficiencyTools.Controls.Add(this.tabPage4);
            this.tbpDeficiencyTools.Location = new System.Drawing.Point(12, 12);
            this.tbpDeficiencyTools.Name = "tbpDeficiencyTools";
            this.tbpDeficiencyTools.SelectedIndex = 0;
            this.tbpDeficiencyTools.Size = new System.Drawing.Size(600, 420);
            this.tbpDeficiencyTools.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.pnlDeficiencyType);
            this.tabPage3.Controls.Add(this.btnCancel);
            this.tabPage3.Controls.Add(this.btnUpdate);
            this.tabPage3.Controls.Add(this.btnPrint);
            this.tabPage3.Controls.Add(this.grpCategory);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(592, 394);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Deficiency Type & Additional Information";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // pnlDeficiencyType
            // 
            this.pnlDeficiencyType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDeficiencyType.Controls.Add(this.dgvDeficientTypes);
            this.pnlDeficiencyType.Controls.Add(this.grpAdditionalInfo);
            this.pnlDeficiencyType.Location = new System.Drawing.Point(9, 57);
            this.pnlDeficiencyType.Name = "pnlDeficiencyType";
            this.pnlDeficiencyType.Size = new System.Drawing.Size(574, 305);
            this.pnlDeficiencyType.TabIndex = 1;
            // 
            // dgvDeficientTypes
            // 
            this.dgvDeficientTypes.AllowUserToAddRows = false;
            this.dgvDeficientTypes.AllowUserToDeleteRows = false;
            this.dgvDeficientTypes.AllowUserToResizeColumns = false;
            this.dgvDeficientTypes.AllowUserToResizeRows = false;
            this.dgvDeficientTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDeficientTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDeficientTypes.Location = new System.Drawing.Point(3, 3);
            this.dgvDeficientTypes.MultiSelect = false;
            this.dgvDeficientTypes.Name = "dgvDeficientTypes";
            this.dgvDeficientTypes.RowHeadersVisible = false;
            this.dgvDeficientTypes.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvDeficientTypes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDeficientTypes.Size = new System.Drawing.Size(568, 153);
            this.dgvDeficientTypes.TabIndex = 0;
            this.dgvDeficientTypes.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDeficientTypes_CellEndEdit);
            this.dgvDeficientTypes.SelectionChanged += new System.EventHandler(this.dgvDeficientTypes_SelectionChanged);
            // 
            // grpAdditionalInfo
            // 
            this.grpAdditionalInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAdditionalInfo.Controls.Add(this.dgvAdditionalInfo);
            this.grpAdditionalInfo.Location = new System.Drawing.Point(3, 162);
            this.grpAdditionalInfo.Name = "grpAdditionalInfo";
            this.grpAdditionalInfo.Size = new System.Drawing.Size(568, 139);
            this.grpAdditionalInfo.TabIndex = 1;
            this.grpAdditionalInfo.TabStop = false;
            this.grpAdditionalInfo.Text = "Additional Information";
            // 
            // dgvAdditionalInfo
            // 
            this.dgvAdditionalInfo.AllowUserToAddRows = false;
            this.dgvAdditionalInfo.AllowUserToDeleteRows = false;
            this.dgvAdditionalInfo.AllowUserToResizeColumns = false;
            this.dgvAdditionalInfo.AllowUserToResizeRows = false;
            this.dgvAdditionalInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAdditionalInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdditionalInfo.Location = new System.Drawing.Point(6, 22);
            this.dgvAdditionalInfo.Name = "dgvAdditionalInfo";
            this.dgvAdditionalInfo.RowHeadersVisible = false;
            this.dgvAdditionalInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvAdditionalInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAdditionalInfo.Size = new System.Drawing.Size(556, 111);
            this.dgvAdditionalInfo.TabIndex = 0;
            this.dgvAdditionalInfo.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAdditionalInfo_CellEndEdit);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancel.Location = new System.Drawing.Point(508, 368);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnUpdate.Location = new System.Drawing.Point(427, 368);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "Edit";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPrint.Location = new System.Drawing.Point(346, 368);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // grpCategory
            // 
            this.grpCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpCategory.Controls.Add(this.txtCategoryCode);
            this.grpCategory.Controls.Add(this.btnCategoryDelete);
            this.grpCategory.Controls.Add(this.btnCategoryEdit);
            this.grpCategory.Controls.Add(this.cmbCategoryName);
            this.grpCategory.Controls.Add(this.btnCategoryAdd);
            this.grpCategory.Controls.Add(this.txtCategoryName);
            this.grpCategory.ForeColor = System.Drawing.Color.Blue;
            this.grpCategory.Location = new System.Drawing.Point(3, 3);
            this.grpCategory.Name = "grpCategory";
            this.grpCategory.Size = new System.Drawing.Size(586, 47);
            this.grpCategory.TabIndex = 0;
            this.grpCategory.TabStop = false;
            this.grpCategory.Text = "Category";
            // 
            // txtCategoryCode
            // 
            this.txtCategoryCode.Location = new System.Drawing.Point(6, 21);
            this.txtCategoryCode.Name = "txtCategoryCode";
            this.txtCategoryCode.ReadOnly = true;
            this.txtCategoryCode.Size = new System.Drawing.Size(39, 20);
            this.txtCategoryCode.TabIndex = 0;
            // 
            // btnCategoryDelete
            // 
            this.btnCategoryDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCategoryDelete.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCategoryDelete.Location = new System.Drawing.Point(505, 18);
            this.btnCategoryDelete.Name = "btnCategoryDelete";
            this.btnCategoryDelete.Size = new System.Drawing.Size(75, 23);
            this.btnCategoryDelete.TabIndex = 4;
            this.btnCategoryDelete.Text = "Delete";
            this.btnCategoryDelete.UseVisualStyleBackColor = true;
            this.btnCategoryDelete.Click += new System.EventHandler(this.btnCategoryDelete_Click);
            // 
            // btnCategoryEdit
            // 
            this.btnCategoryEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCategoryEdit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCategoryEdit.Location = new System.Drawing.Point(424, 18);
            this.btnCategoryEdit.Name = "btnCategoryEdit";
            this.btnCategoryEdit.Size = new System.Drawing.Size(75, 23);
            this.btnCategoryEdit.TabIndex = 3;
            this.btnCategoryEdit.Text = "Edit";
            this.btnCategoryEdit.UseVisualStyleBackColor = true;
            this.btnCategoryEdit.Click += new System.EventHandler(this.btnCategoryEdit_Click);
            // 
            // cmbCategoryName
            // 
            this.cmbCategoryName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCategoryName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoryName.FormattingEnabled = true;
            this.cmbCategoryName.Location = new System.Drawing.Point(51, 20);
            this.cmbCategoryName.Name = "cmbCategoryName";
            this.cmbCategoryName.Size = new System.Drawing.Size(286, 21);
            this.cmbCategoryName.TabIndex = 1;
            this.cmbCategoryName.SelectedIndexChanged += new System.EventHandler(this.cmbCategoryName_SelectedIndexChanged);
            // 
            // btnCategoryAdd
            // 
            this.btnCategoryAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCategoryAdd.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCategoryAdd.Location = new System.Drawing.Point(343, 18);
            this.btnCategoryAdd.Name = "btnCategoryAdd";
            this.btnCategoryAdd.Size = new System.Drawing.Size(75, 23);
            this.btnCategoryAdd.TabIndex = 2;
            this.btnCategoryAdd.Text = "Add";
            this.btnCategoryAdd.UseVisualStyleBackColor = true;
            this.btnCategoryAdd.Click += new System.EventHandler(this.btnCategoryAdd_Click);
            // 
            // txtCategoryName
            // 
            this.txtCategoryName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCategoryName.Location = new System.Drawing.Point(51, 21);
            this.txtCategoryName.Name = "txtCategoryName";
            this.txtCategoryName.Size = new System.Drawing.Size(286, 20);
            this.txtCategoryName.TabIndex = 4;
            this.txtCategoryName.Visible = false;
            this.txtCategoryName.TextChanged += new System.EventHandler(this.txtCategoryName_TextChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnAccountEdit);
            this.tabPage1.Controls.Add(this.btnAccountCancel);
            this.tabPage1.Controls.Add(this.dgvAccountType);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(592, 394);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Account Type";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnAccountEdit
            // 
            this.btnAccountEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAccountEdit.Location = new System.Drawing.Point(433, 365);
            this.btnAccountEdit.Name = "btnAccountEdit";
            this.btnAccountEdit.Size = new System.Drawing.Size(75, 23);
            this.btnAccountEdit.TabIndex = 1;
            this.btnAccountEdit.Text = "Edit";
            this.btnAccountEdit.UseVisualStyleBackColor = true;
            this.btnAccountEdit.Click += new System.EventHandler(this.btnAccountEdit_Click);
            // 
            // btnAccountCancel
            // 
            this.btnAccountCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAccountCancel.Location = new System.Drawing.Point(514, 365);
            this.btnAccountCancel.Name = "btnAccountCancel";
            this.btnAccountCancel.Size = new System.Drawing.Size(75, 23);
            this.btnAccountCancel.TabIndex = 2;
            this.btnAccountCancel.Text = "Cancel";
            this.btnAccountCancel.UseVisualStyleBackColor = true;
            this.btnAccountCancel.Click += new System.EventHandler(this.btnAccountCancel_Click);
            // 
            // dgvAccountType
            // 
            this.dgvAccountType.AllowUserToAddRows = false;
            this.dgvAccountType.AllowUserToDeleteRows = false;
            this.dgvAccountType.AllowUserToResizeColumns = false;
            this.dgvAccountType.AllowUserToResizeRows = false;
            this.dgvAccountType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAccountType.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAccountType.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccountType.Location = new System.Drawing.Point(3, 6);
            this.dgvAccountType.Name = "dgvAccountType";
            this.dgvAccountType.RowHeadersVisible = false;
            this.dgvAccountType.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvAccountType.Size = new System.Drawing.Size(586, 353);
            this.dgvAccountType.TabIndex = 0;
            this.dgvAccountType.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAccountType_CellEndEdit);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnReqEdit);
            this.tabPage2.Controls.Add(this.btnReqCancel);
            this.tabPage2.Controls.Add(this.dgvRequiredFields);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(592, 394);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Required Fields";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnReqEdit
            // 
            this.btnReqEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReqEdit.Location = new System.Drawing.Point(433, 365);
            this.btnReqEdit.Name = "btnReqEdit";
            this.btnReqEdit.Size = new System.Drawing.Size(75, 23);
            this.btnReqEdit.TabIndex = 1;
            this.btnReqEdit.Text = "Edit";
            this.btnReqEdit.UseVisualStyleBackColor = true;
            this.btnReqEdit.Click += new System.EventHandler(this.btnReqEdit_Click);
            // 
            // btnReqCancel
            // 
            this.btnReqCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReqCancel.Location = new System.Drawing.Point(514, 365);
            this.btnReqCancel.Name = "btnReqCancel";
            this.btnReqCancel.Size = new System.Drawing.Size(75, 23);
            this.btnReqCancel.TabIndex = 2;
            this.btnReqCancel.Text = "Cancel";
            this.btnReqCancel.UseVisualStyleBackColor = true;
            this.btnReqCancel.Click += new System.EventHandler(this.btnReqCancel_Click);
            // 
            // dgvRequiredFields
            // 
            this.dgvRequiredFields.AllowUserToAddRows = false;
            this.dgvRequiredFields.AllowUserToDeleteRows = false;
            this.dgvRequiredFields.AllowUserToResizeColumns = false;
            this.dgvRequiredFields.AllowUserToResizeRows = false;
            this.dgvRequiredFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRequiredFields.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRequiredFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRequiredFields.Location = new System.Drawing.Point(3, 6);
            this.dgvRequiredFields.Name = "dgvRequiredFields";
            this.dgvRequiredFields.RowHeadersVisible = false;
            this.dgvRequiredFields.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvRequiredFields.Size = new System.Drawing.Size(586, 353);
            this.dgvRequiredFields.TabIndex = 0;
            this.dgvRequiredFields.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRequiredFields_CellEndEdit);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dgvSolutions);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(592, 394);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Solutions";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dgvSolutions
            // 
            this.dgvSolutions.AllowUserToAddRows = false;
            this.dgvSolutions.AllowUserToDeleteRows = false;
            this.dgvSolutions.AllowUserToResizeColumns = false;
            this.dgvSolutions.AllowUserToResizeRows = false;
            this.dgvSolutions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSolutions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSolutions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSolutions.Location = new System.Drawing.Point(4, 4);
            this.dgvSolutions.MultiSelect = false;
            this.dgvSolutions.Name = "dgvSolutions";
            this.dgvSolutions.RowHeadersVisible = false;
            this.dgvSolutions.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvSolutions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSolutions.Size = new System.Drawing.Size(585, 387);
            this.dgvSolutions.TabIndex = 0;
            // 
            // frmDeficientRecordsTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 444);
            this.Controls.Add(this.tbpDeficiencyTools);
            this.Name = "frmDeficientRecordsTool";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Deficiency Tools";
            this.Load += new System.EventHandler(this.frmDeficientRecordsTool_Load);
            this.tbpDeficiencyTools.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.pnlDeficiencyType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeficientTypes)).EndInit();
            this.grpAdditionalInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdditionalInfo)).EndInit();
            this.grpCategory.ResumeLayout(false);
            this.grpCategory.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountType)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequiredFields)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSolutions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbpDeficiencyTools;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvAccountType;
        private System.Windows.Forms.Button btnAccountEdit;
        private System.Windows.Forms.Button btnAccountCancel;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.GroupBox grpCategory;
        private System.Windows.Forms.TextBox txtCategoryCode;
        private System.Windows.Forms.Button btnCategoryDelete;
        private System.Windows.Forms.Button btnCategoryEdit;
        private System.Windows.Forms.ComboBox cmbCategoryName;
        private System.Windows.Forms.Button btnCategoryAdd;
        private System.Windows.Forms.Panel pnlDeficiencyType;
        private System.Windows.Forms.DataGridView dgvDeficientTypes;
        private System.Windows.Forms.GroupBox grpAdditionalInfo;
        private System.Windows.Forms.DataGridView dgvAdditionalInfo;
        private System.Windows.Forms.Button btnReqEdit;
        private System.Windows.Forms.Button btnReqCancel;
        private System.Windows.Forms.DataGridView dgvRequiredFields;
        private System.Windows.Forms.DataGridView dgvSolutions;
        private System.Windows.Forms.TextBox txtCategoryName;
    }
}