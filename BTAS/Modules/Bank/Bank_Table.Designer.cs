namespace Amellar.RPTA.Classes.Bank
{
    partial class frmBankTable
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
            this.grdBankTable = new System.Windows.Forms.DataGridView();
            this.lblBankCode = new System.Windows.Forms.Label();
            this.grpBankCode = new System.Windows.Forms.GroupBox();
            this.btnViewAll = new System.Windows.Forms.Button();
            this.txtProv = new System.Windows.Forms.TextBox();
            this.txtMun = new System.Windows.Forms.TextBox();
            this.txtBranch = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtBankName = new System.Windows.Forms.TextBox();
            this.txtBankCode = new System.Windows.Forms.TextBox();
            this.lblBankAddr = new System.Windows.Forms.Label();
            this.lblA = new System.Windows.Forms.Label();
            this.lblP = new System.Windows.Forms.Label();
            this.lblM = new System.Windows.Forms.Label();
            this.lblB = new System.Windows.Forms.Label();
            this.lblN = new System.Windows.Forms.Label();
            this.lblC = new System.Windows.Forms.Label();
            this.lblBankProv = new System.Windows.Forms.Label();
            this.lblBankMun = new System.Windows.Forms.Label();
            this.lblBankBranch = new System.Windows.Forms.Label();
            this.lblBankName = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdBankTable)).BeginInit();
            this.grpBankCode.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdBankTable
            // 
            this.grdBankTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdBankTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdBankTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdBankTable.Location = new System.Drawing.Point(12, 12);
            this.grdBankTable.Name = "grdBankTable";
            this.grdBankTable.RowHeadersVisible = false;
            this.grdBankTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdBankTable.Size = new System.Drawing.Size(447, 268);
            this.grdBankTable.TabIndex = 0;
            this.grdBankTable.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdBankTable_CellMouseClick);
            // 
            // lblBankCode
            // 
            this.lblBankCode.AutoSize = true;
            this.lblBankCode.Location = new System.Drawing.Point(37, 21);
            this.lblBankCode.Name = "lblBankCode";
            this.lblBankCode.Size = new System.Drawing.Size(60, 13);
            this.lblBankCode.TabIndex = 1;
            this.lblBankCode.Text = "Bank Code";
            // 
            // grpBankCode
            // 
            this.grpBankCode.Controls.Add(this.btnViewAll);
            this.grpBankCode.Controls.Add(this.txtProv);
            this.grpBankCode.Controls.Add(this.txtMun);
            this.grpBankCode.Controls.Add(this.txtBranch);
            this.grpBankCode.Controls.Add(this.txtAddress);
            this.grpBankCode.Controls.Add(this.txtBankName);
            this.grpBankCode.Controls.Add(this.txtBankCode);
            this.grpBankCode.Controls.Add(this.lblBankAddr);
            this.grpBankCode.Controls.Add(this.lblA);
            this.grpBankCode.Controls.Add(this.lblP);
            this.grpBankCode.Controls.Add(this.lblM);
            this.grpBankCode.Controls.Add(this.lblB);
            this.grpBankCode.Controls.Add(this.lblN);
            this.grpBankCode.Controls.Add(this.lblC);
            this.grpBankCode.Controls.Add(this.lblBankProv);
            this.grpBankCode.Controls.Add(this.lblBankMun);
            this.grpBankCode.Controls.Add(this.lblBankBranch);
            this.grpBankCode.Controls.Add(this.lblBankName);
            this.grpBankCode.Controls.Add(this.lblBankCode);
            this.grpBankCode.Location = new System.Drawing.Point(12, 286);
            this.grpBankCode.Name = "grpBankCode";
            this.grpBankCode.Size = new System.Drawing.Size(447, 176);
            this.grpBankCode.TabIndex = 2;
            this.grpBankCode.TabStop = false;
            // 
            // btnViewAll
            // 
            this.btnViewAll.Location = new System.Drawing.Point(319, 16);
            this.btnViewAll.Name = "btnViewAll";
            this.btnViewAll.Size = new System.Drawing.Size(88, 23);
            this.btnViewAll.TabIndex = 11;
            this.btnViewAll.Text = "Vie&w all Info";
            this.btnViewAll.UseVisualStyleBackColor = true;
            this.btnViewAll.Click += new System.EventHandler(this.btnViewAll_Click);
            // 
            // txtProv
            // 
            this.txtProv.Enabled = false;
            this.txtProv.Location = new System.Drawing.Point(140, 145);
            this.txtProv.Name = "txtProv";
            this.txtProv.Size = new System.Drawing.Size(267, 20);
            this.txtProv.TabIndex = 10;
            // 
            // txtMun
            // 
            this.txtMun.Enabled = false;
            this.txtMun.Location = new System.Drawing.Point(140, 120);
            this.txtMun.Name = "txtMun";
            this.txtMun.Size = new System.Drawing.Size(267, 20);
            this.txtMun.TabIndex = 8;
            // 
            // txtBranch
            // 
            this.txtBranch.Enabled = false;
            this.txtBranch.Location = new System.Drawing.Point(140, 70);
            this.txtBranch.Name = "txtBranch";
            this.txtBranch.Size = new System.Drawing.Size(267, 20);
            this.txtBranch.TabIndex = 9;
            // 
            // txtAddress
            // 
            this.txtAddress.Enabled = false;
            this.txtAddress.Location = new System.Drawing.Point(140, 95);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(267, 20);
            this.txtAddress.TabIndex = 2;
            // 
            // txtBankName
            // 
            this.txtBankName.Enabled = false;
            this.txtBankName.Location = new System.Drawing.Point(140, 44);
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.Size = new System.Drawing.Size(267, 20);
            this.txtBankName.TabIndex = 2;
            // 
            // txtBankCode
            // 
            this.txtBankCode.Enabled = false;
            this.txtBankCode.Location = new System.Drawing.Point(140, 18);
            this.txtBankCode.Name = "txtBankCode";
            this.txtBankCode.Size = new System.Drawing.Size(173, 20);
            this.txtBankCode.TabIndex = 2;
            // 
            // lblBankAddr
            // 
            this.lblBankAddr.AutoSize = true;
            this.lblBankAddr.Location = new System.Drawing.Point(35, 98);
            this.lblBankAddr.Name = "lblBankAddr";
            this.lblBankAddr.Size = new System.Drawing.Size(73, 13);
            this.lblBankAddr.TabIndex = 1;
            this.lblBankAddr.Text = "Bank Address";
            // 
            // lblA
            // 
            this.lblA.AutoSize = true;
            this.lblA.Location = new System.Drawing.Point(118, 98);
            this.lblA.Name = "lblA";
            this.lblA.Size = new System.Drawing.Size(10, 13);
            this.lblA.TabIndex = 1;
            this.lblA.Text = ":";
            // 
            // lblP
            // 
            this.lblP.AutoSize = true;
            this.lblP.Location = new System.Drawing.Point(118, 148);
            this.lblP.Name = "lblP";
            this.lblP.Size = new System.Drawing.Size(10, 13);
            this.lblP.TabIndex = 1;
            this.lblP.Text = ":";
            // 
            // lblM
            // 
            this.lblM.AutoSize = true;
            this.lblM.Location = new System.Drawing.Point(118, 123);
            this.lblM.Name = "lblM";
            this.lblM.Size = new System.Drawing.Size(10, 13);
            this.lblM.TabIndex = 1;
            this.lblM.Text = ":";
            // 
            // lblB
            // 
            this.lblB.AutoSize = true;
            this.lblB.Location = new System.Drawing.Point(117, 73);
            this.lblB.Name = "lblB";
            this.lblB.Size = new System.Drawing.Size(10, 13);
            this.lblB.TabIndex = 1;
            this.lblB.Text = ":";
            // 
            // lblN
            // 
            this.lblN.AutoSize = true;
            this.lblN.Location = new System.Drawing.Point(118, 45);
            this.lblN.Name = "lblN";
            this.lblN.Size = new System.Drawing.Size(10, 13);
            this.lblN.TabIndex = 1;
            this.lblN.Text = ":";
            // 
            // lblC
            // 
            this.lblC.AutoSize = true;
            this.lblC.Location = new System.Drawing.Point(118, 21);
            this.lblC.Name = "lblC";
            this.lblC.Size = new System.Drawing.Size(10, 13);
            this.lblC.TabIndex = 1;
            this.lblC.Text = ":";
            // 
            // lblBankProv
            // 
            this.lblBankProv.AutoSize = true;
            this.lblBankProv.Location = new System.Drawing.Point(36, 148);
            this.lblBankProv.Name = "lblBankProv";
            this.lblBankProv.Size = new System.Drawing.Size(77, 13);
            this.lblBankProv.TabIndex = 1;
            this.lblBankProv.Text = "Bank Province";
            // 
            // lblBankMun
            // 
            this.lblBankMun.AutoSize = true;
            this.lblBankMun.Location = new System.Drawing.Point(36, 123);
            this.lblBankMun.Name = "lblBankMun";
            this.lblBankMun.Size = new System.Drawing.Size(80, 13);
            this.lblBankMun.TabIndex = 1;
            this.lblBankMun.Text = "Bank Municipal";
            // 
            // lblBankBranch
            // 
            this.lblBankBranch.AutoSize = true;
            this.lblBankBranch.Location = new System.Drawing.Point(36, 73);
            this.lblBankBranch.Name = "lblBankBranch";
            this.lblBankBranch.Size = new System.Drawing.Size(69, 13);
            this.lblBankBranch.TabIndex = 1;
            this.lblBankBranch.Text = "Bank Branch";
            // 
            // lblBankName
            // 
            this.lblBankName.AutoSize = true;
            this.lblBankName.Location = new System.Drawing.Point(37, 45);
            this.lblBankName.Name = "lblBankName";
            this.lblBankName.Size = new System.Drawing.Size(63, 13);
            this.lblBankName.TabIndex = 1;
            this.lblBankName.Text = "Bank Name";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(43, 468);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(124, 468);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(205, 468);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(286, 468);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 6;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(367, 468);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmBankTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 503);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.grpBankCode);
            this.Controls.Add(this.grdBankTable);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBankTable";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bank Table";
            this.Load += new System.EventHandler(this.frmBankTable_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdBankTable)).EndInit();
            this.grpBankCode.ResumeLayout(false);
            this.grpBankCode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdBankTable;
        private System.Windows.Forms.Label lblBankCode;
        private System.Windows.Forms.GroupBox grpBankCode;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtBankName;
        private System.Windows.Forms.TextBox txtBankCode;
        private System.Windows.Forms.Label lblBankAddr;
        private System.Windows.Forms.Label lblA;
        private System.Windows.Forms.Label lblN;
        private System.Windows.Forms.Label lblC;
        private System.Windows.Forms.Label lblBankName;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtProv;
        private System.Windows.Forms.TextBox txtMun;
        private System.Windows.Forms.TextBox txtBranch;
        private System.Windows.Forms.Label lblP;
        private System.Windows.Forms.Label lblM;
        private System.Windows.Forms.Label lblB;
        private System.Windows.Forms.Label lblBankProv;
        private System.Windows.Forms.Label lblBankMun;
        private System.Windows.Forms.Label lblBankBranch;
        private System.Windows.Forms.Button btnViewAll;
    }
}