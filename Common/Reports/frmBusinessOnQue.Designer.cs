namespace Amellar.Common.Reports
{
    partial class frmBusinessOnQue
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
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.txtAdd = new System.Windows.Forms.TextBox();
            this.txtOwner = new System.Windows.Forms.TextBox();
            this.dgvBusinessQue = new System.Windows.Forms.DataGridView();
            this.CHK_BOX = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.bin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bns_nm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bns_add = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bns_own = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.own_address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnIssue1stNotice = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblNo = new System.Windows.Forms.Label();
            this.dgvWithNotice = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.btnIssue2ndNotice = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBusinessQue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWithNotice)).BeginInit();
            this.SuspendLayout();
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(24, 22);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(805, 114);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(28, 26);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader1.Size = new System.Drawing.Size(801, 23);
            this.kryptonHeader1.TabIndex = 1;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Business Information";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Registered Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(88, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Address:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(500, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Owner:";
            // 
            // txtBnsName
            // 
            this.txtBnsName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBnsName.Location = new System.Drawing.Point(141, 59);
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.Size = new System.Drawing.Size(348, 20);
            this.txtBnsName.TabIndex = 5;
            this.txtBnsName.TextChanged += new System.EventHandler(this.txtBnsName_TextChanged);
            // 
            // txtAdd
            // 
            this.txtAdd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAdd.Location = new System.Drawing.Point(141, 88);
            this.txtAdd.Name = "txtAdd";
            this.txtAdd.ReadOnly = true;
            this.txtAdd.Size = new System.Drawing.Size(664, 20);
            this.txtAdd.TabIndex = 6;
            // 
            // txtOwner
            // 
            this.txtOwner.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtOwner.Location = new System.Drawing.Point(556, 59);
            this.txtOwner.Name = "txtOwner";
            this.txtOwner.ReadOnly = true;
            this.txtOwner.Size = new System.Drawing.Size(249, 20);
            this.txtOwner.TabIndex = 7;
            // 
            // dgvBusinessQue
            // 
            this.dgvBusinessQue.AllowUserToAddRows = false;
            this.dgvBusinessQue.AllowUserToDeleteRows = false;
            this.dgvBusinessQue.AllowUserToResizeColumns = false;
            this.dgvBusinessQue.AllowUserToResizeRows = false;
            this.dgvBusinessQue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBusinessQue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CHK_BOX,
            this.bin,
            this.bns_nm,
            this.bns_add,
            this.bns_own,
            this.own_address});
            this.dgvBusinessQue.Location = new System.Drawing.Point(24, 152);
            this.dgvBusinessQue.Name = "dgvBusinessQue";
            this.dgvBusinessQue.RowHeadersVisible = false;
            this.dgvBusinessQue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBusinessQue.Size = new System.Drawing.Size(800, 266);
            this.dgvBusinessQue.TabIndex = 8;
            this.dgvBusinessQue.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusinessQue_CellClick);
            // 
            // CHK_BOX
            // 
            this.CHK_BOX.HeaderText = "";
            this.CHK_BOX.Name = "CHK_BOX";
            this.CHK_BOX.Width = 20;
            // 
            // bin
            // 
            this.bin.HeaderText = "BIN";
            this.bin.Name = "bin";
            this.bin.Width = 120;
            // 
            // bns_nm
            // 
            this.bns_nm.HeaderText = "Business Name";
            this.bns_nm.Name = "bns_nm";
            this.bns_nm.Width = 200;
            // 
            // bns_add
            // 
            this.bns_add.HeaderText = "Address";
            this.bns_add.Name = "bns_add";
            this.bns_add.Width = 200;
            // 
            // bns_own
            // 
            this.bns_own.HeaderText = "Owner";
            this.bns_own.Name = "bns_own";
            this.bns_own.Width = 200;
            // 
            // own_address
            // 
            this.own_address.HeaderText = "Owner\'s Address";
            this.own_address.Name = "own_address";
            this.own_address.Width = 200;
            // 
            // btnIssue1stNotice
            // 
            this.btnIssue1stNotice.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnIssue1stNotice.Location = new System.Drawing.Point(24, 440);
            this.btnIssue1stNotice.Name = "btnIssue1stNotice";
            this.btnIssue1stNotice.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnIssue1stNotice.Size = new System.Drawing.Size(107, 28);
            this.btnIssue1stNotice.TabIndex = 9;
            this.btnIssue1stNotice.Values.Text = "Issue 1st Notice";
            this.btnIssue1stNotice.Click += new System.EventHandler(this.btnAddtoInspector_Click);
            // 
            // btnClose
            // 
            this.btnClose.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnClose.Location = new System.Drawing.Point(741, 700);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(83, 28);
            this.btnClose.TabIndex = 10;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(29, 158);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(15, 14);
            this.chkAll.TabIndex = 11;
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "BIN";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 120;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Business Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 200;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Address";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 200;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Owner";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 200;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Owner\'s Address";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 200;
            // 
            // lblNo
            // 
            this.lblNo.ForeColor = System.Drawing.Color.Blue;
            this.lblNo.Location = new System.Drawing.Point(26, 423);
            this.lblNo.Name = "lblNo";
            this.lblNo.Size = new System.Drawing.Size(287, 13);
            this.lblNo.TabIndex = 12;
            // 
            // dgvWithNotice
            // 
            this.dgvWithNotice.AllowUserToAddRows = false;
            this.dgvWithNotice.AllowUserToDeleteRows = false;
            this.dgvWithNotice.AllowUserToResizeColumns = false;
            this.dgvWithNotice.AllowUserToResizeRows = false;
            this.dgvWithNotice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWithNotice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10});
            this.dgvWithNotice.Location = new System.Drawing.Point(24, 504);
            this.dgvWithNotice.Name = "dgvWithNotice";
            this.dgvWithNotice.RowHeadersVisible = false;
            this.dgvWithNotice.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWithNotice.Size = new System.Drawing.Size(800, 185);
            this.dgvWithNotice.TabIndex = 13;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 20;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "BIN";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 120;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Business Name";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Width = 200;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Address";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 200;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "Owner";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Width = 200;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "Owner\'s Address";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.Width = 200;
            // 
            // kryptonHeader2
            // 
            this.kryptonHeader2.AutoSize = false;
            this.kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader2.Location = new System.Drawing.Point(24, 482);
            this.kryptonHeader2.Name = "kryptonHeader2";
            this.kryptonHeader2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader2.Size = new System.Drawing.Size(801, 23);
            this.kryptonHeader2.TabIndex = 14;
            this.kryptonHeader2.Values.Description = "";
            this.kryptonHeader2.Values.Heading = "List of Businesses on Queue with First Notice";
            // 
            // btnIssue2ndNotice
            // 
            this.btnIssue2ndNotice.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnIssue2ndNotice.Location = new System.Drawing.Point(137, 439);
            this.btnIssue2ndNotice.Name = "btnIssue2ndNotice";
            this.btnIssue2ndNotice.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnIssue2ndNotice.Size = new System.Drawing.Size(107, 28);
            this.btnIssue2ndNotice.TabIndex = 15;
            this.btnIssue2ndNotice.Values.Text = "Issue 2st Notice";
            this.btnIssue2ndNotice.Click += new System.EventHandler(this.btnIssue2ndNotice_Click);
            // 
            // frmBusinessOnQue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 740);
            this.Controls.Add(this.btnIssue2ndNotice);
            this.Controls.Add(this.kryptonHeader2);
            this.Controls.Add(this.dgvWithNotice);
            this.Controls.Add(this.lblNo);
            this.Controls.Add(this.chkAll);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnIssue1stNotice);
            this.Controls.Add(this.dgvBusinessQue);
            this.Controls.Add(this.txtOwner);
            this.Controls.Add(this.txtAdd);
            this.Controls.Add(this.txtBnsName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.containerWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBusinessOnQue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Business on Queue";
            this.Load += new System.EventHandler(this.frmBusinessOnQue_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBusinessQue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWithNotice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBnsName;
        private System.Windows.Forms.TextBox txtAdd;
        private System.Windows.Forms.TextBox txtOwner;
        private System.Windows.Forms.DataGridView dgvBusinessQue;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CHK_BOX;
        private System.Windows.Forms.DataGridViewTextBoxColumn bin;
        private System.Windows.Forms.DataGridViewTextBoxColumn bns_nm;
        private System.Windows.Forms.DataGridViewTextBoxColumn bns_add;
        private System.Windows.Forms.DataGridViewTextBoxColumn bns_own;
        private System.Windows.Forms.DataGridViewTextBoxColumn own_address;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnIssue1stNotice;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.Label lblNo;
        private System.Windows.Forms.DataGridView dgvWithNotice;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnIssue2ndNotice;
    }
}