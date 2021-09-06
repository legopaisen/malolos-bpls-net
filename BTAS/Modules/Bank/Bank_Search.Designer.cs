namespace Amellar.RPTA.Classes.Bank
{
    partial class frmBankSearch
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
            this.grpBankSearch = new System.Windows.Forms.GroupBox();
            this.grdAddr = new System.Windows.Forms.DataGridView();
            this.colAddr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grdBankList = new System.Windows.Forms.DataGridView();
            this.grpSearchBy = new System.Windows.Forms.GroupBox();
            this.txtBankNames = new System.Windows.Forms.TextBox();
            this.lblN = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpBankSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAddr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBankList)).BeginInit();
            this.grpSearchBy.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBankSearch
            // 
            this.grpBankSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBankSearch.Controls.Add(this.grdAddr);
            this.grpBankSearch.Controls.Add(this.grdBankList);
            this.grpBankSearch.ForeColor = System.Drawing.Color.Blue;
            this.grpBankSearch.Location = new System.Drawing.Point(12, 12);
            this.grpBankSearch.Name = "grpBankSearch";
            this.grpBankSearch.Size = new System.Drawing.Size(612, 276);
            this.grpBankSearch.TabIndex = 0;
            this.grpBankSearch.TabStop = false;
            this.grpBankSearch.Text = "Bank List";
            // 
            // grdAddr
            // 
            this.grdAddr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdAddr.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdAddr.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdAddr.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAddr});
            this.grdAddr.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdAddr.Location = new System.Drawing.Point(14, 229);
            this.grdAddr.MultiSelect = false;
            this.grdAddr.Name = "grdAddr";
            this.grdAddr.ReadOnly = true;
            this.grdAddr.RowHeadersVisible = false;
            this.grdAddr.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdAddr.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.grdAddr.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdAddr.Size = new System.Drawing.Size(583, 42);
            this.grdAddr.TabIndex = 5;
            this.grdAddr.Visible = false;
            // 
            // colAddr
            // 
            this.colAddr.HeaderText = "Address";
            this.colAddr.Name = "colAddr";
            this.colAddr.ReadOnly = true;
            // 
            // grdBankList
            // 
            this.grdBankList.AllowUserToAddRows = false;
            this.grdBankList.AllowUserToDeleteRows = false;
            this.grdBankList.AllowUserToResizeRows = false;
            this.grdBankList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdBankList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdBankList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdBankList.DefaultCellStyle = dataGridViewCellStyle1;
            this.grdBankList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdBankList.Location = new System.Drawing.Point(14, 17);
            this.grdBankList.MultiSelect = false;
            this.grdBankList.Name = "grdBankList";
            this.grdBankList.ReadOnly = true;
            this.grdBankList.RowHeadersVisible = false;
            this.grdBankList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdBankList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdBankList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grdBankList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdBankList.Size = new System.Drawing.Size(583, 253);
            this.grdBankList.TabIndex = 1;
            this.grdBankList.SelectionChanged += new System.EventHandler(this.grdBankList_SelectionChanged);
            // 
            // grpSearchBy
            // 
            this.grpSearchBy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSearchBy.Controls.Add(this.txtBankNames);
            this.grpSearchBy.Controls.Add(this.lblN);
            this.grpSearchBy.Controls.Add(this.lblName);
            this.grpSearchBy.ForeColor = System.Drawing.Color.Blue;
            this.grpSearchBy.Location = new System.Drawing.Point(12, 290);
            this.grpSearchBy.Name = "grpSearchBy";
            this.grpSearchBy.Size = new System.Drawing.Size(612, 54);
            this.grpSearchBy.TabIndex = 2;
            this.grpSearchBy.TabStop = false;
            this.grpSearchBy.Text = "Search By";
            // 
            // txtBankNames
            // 
            this.txtBankNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBankNames.Location = new System.Drawing.Point(72, 19);
            this.txtBankNames.Name = "txtBankNames";
            this.txtBankNames.Size = new System.Drawing.Size(525, 20);
            this.txtBankNames.TabIndex = 2;
            this.txtBankNames.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAddress_KeyPress);
            // 
            // lblN
            // 
            this.lblN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lblN.AutoSize = true;
            this.lblN.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblN.Location = new System.Drawing.Point(57, 21);
            this.lblN.Name = "lblN";
            this.lblN.Size = new System.Drawing.Size(10, 13);
            this.lblN.TabIndex = 1;
            this.lblN.Text = ":";
            // 
            // lblName
            // 
            this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lblName.AutoSize = true;
            this.lblName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblName.Location = new System.Drawing.Point(12, 22);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(468, 350);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(549, 350);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmBankSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(639, 377);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.grpSearchBy);
            this.Controls.Add(this.grpBankSearch);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBankSearch";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bank Search";
            this.Load += new System.EventHandler(this.frmBankSearch_Load);
            this.grpBankSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAddr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBankList)).EndInit();
            this.grpSearchBy.ResumeLayout(false);
            this.grpSearchBy.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBankSearch;
        private System.Windows.Forms.DataGridView grdBankList;
        private System.Windows.Forms.GroupBox grpSearchBy;
        private System.Windows.Forms.Label lblN;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtBankNames;
        private System.Windows.Forms.DataGridView grdAddr;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAddr;
    }
}