namespace NewBankModule
{
    partial class frmBank
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
            this.frameWithShadow1 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow2 = new Amellar.Common.SOA.FrameWithShadow();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEdit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAdd = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.dgvBanks = new System.Windows.Forms.DataGridView();
            this.col_bnkcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_bnknm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_branch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bol_add = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBnkName = new System.Windows.Forms.TextBox();
            this.txtBankBranch = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBanks)).BeginInit();
            this.SuspendLayout();
            // 
            // frameWithShadow1
            // 
            this.frameWithShadow1.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow1.Location = new System.Drawing.Point(7, 6);
            this.frameWithShadow1.Name = "frameWithShadow1";
            this.frameWithShadow1.Size = new System.Drawing.Size(487, 260);
            this.frameWithShadow1.TabIndex = 0;
            // 
            // frameWithShadow2
            // 
            this.frameWithShadow2.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow2.Location = new System.Drawing.Point(6, 270);
            this.frameWithShadow2.Name = "frameWithShadow2";
            this.frameWithShadow2.Size = new System.Drawing.Size(487, 128);
            this.frameWithShadow2.TabIndex = 1;
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(12, 6);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparklePurple;
            this.kryptonHeader1.Size = new System.Drawing.Size(478, 28);
            this.kryptonHeader1.TabIndex = 2;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "List of Banks";
            // 
            // kryptonHeader2
            // 
            this.kryptonHeader2.AutoSize = false;
            this.kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader2.Location = new System.Drawing.Point(12, 272);
            this.kryptonHeader2.Name = "kryptonHeader2";
            this.kryptonHeader2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparklePurple;
            this.kryptonHeader2.Size = new System.Drawing.Size(478, 28);
            this.kryptonHeader2.TabIndex = 3;
            this.kryptonHeader2.Values.Description = "";
            this.kryptonHeader2.Values.Heading = "Bank Information";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(412, 404);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparklePurple;
            this.btnClose.Size = new System.Drawing.Size(68, 27);
            this.btnClose.TabIndex = 4;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(338, 404);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparklePurple;
            this.btnEdit.Size = new System.Drawing.Size(68, 27);
            this.btnEdit.TabIndex = 6;
            this.btnEdit.Values.Text = "Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(264, 404);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparklePurple;
            this.btnAdd.Size = new System.Drawing.Size(68, 27);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Values.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgvBanks
            // 
            this.dgvBanks.AllowUserToAddRows = false;
            this.dgvBanks.AllowUserToDeleteRows = false;
            this.dgvBanks.AllowUserToResizeColumns = false;
            this.dgvBanks.AllowUserToResizeRows = false;
            this.dgvBanks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBanks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_bnkcode,
            this.col_bnknm,
            this.col_branch,
            this.bol_add});
            this.dgvBanks.Location = new System.Drawing.Point(19, 49);
            this.dgvBanks.Name = "dgvBanks";
            this.dgvBanks.RowHeadersVisible = false;
            this.dgvBanks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvBanks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBanks.Size = new System.Drawing.Size(461, 198);
            this.dgvBanks.TabIndex = 8;
            this.dgvBanks.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBanks_CellClick);
            // 
            // col_bnkcode
            // 
            this.col_bnkcode.HeaderText = "Code";
            this.col_bnkcode.Name = "col_bnkcode";
            this.col_bnkcode.Width = 50;
            // 
            // col_bnknm
            // 
            this.col_bnknm.HeaderText = "Bank Name";
            this.col_bnknm.Name = "col_bnknm";
            this.col_bnknm.Width = 200;
            // 
            // col_branch
            // 
            this.col_branch.HeaderText = "Branch";
            this.col_branch.Name = "col_branch";
            this.col_branch.Width = 200;
            // 
            // bol_add
            // 
            this.bol_add.HeaderText = "Address";
            this.bol_add.Name = "bol_add";
            this.bol_add.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 312);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Bank Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 340);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Branch:";
            // 
            // txtBnkName
            // 
            this.txtBnkName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBnkName.Location = new System.Drawing.Point(103, 309);
            this.txtBnkName.Name = "txtBnkName";
            this.txtBnkName.ReadOnly = true;
            this.txtBnkName.Size = new System.Drawing.Size(377, 20);
            this.txtBnkName.TabIndex = 11;
            // 
            // txtBankBranch
            // 
            this.txtBankBranch.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBankBranch.Location = new System.Drawing.Point(103, 335);
            this.txtBankBranch.Name = "txtBankBranch";
            this.txtBankBranch.ReadOnly = true;
            this.txtBankBranch.Size = new System.Drawing.Size(377, 20);
            this.txtBankBranch.TabIndex = 12;
            // 
            // txtAddress
            // 
            this.txtAddress.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAddress.Location = new System.Drawing.Point(103, 361);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.ReadOnly = true;
            this.txtAddress.Size = new System.Drawing.Size(377, 20);
            this.txtAddress.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 366);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Address:";
            // 
            // frmBank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(501, 438);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBankBranch);
            this.Controls.Add(this.txtBnkName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvBanks);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.kryptonHeader2);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.frameWithShadow2);
            this.Controls.Add(this.frameWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBank";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bank Module";
            this.Load += new System.EventHandler(this.frmBank_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBanks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.SOA.FrameWithShadow frameWithShadow1;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow2;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEdit;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAdd;
        private System.Windows.Forms.DataGridView dgvBanks;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_bnkcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_bnknm;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_branch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBnkName;
        private System.Windows.Forms.TextBox txtBankBranch;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn bol_add;
    }
}

