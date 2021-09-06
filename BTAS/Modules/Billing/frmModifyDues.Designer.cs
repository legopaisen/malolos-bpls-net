namespace Amellar.BPLS.Billing
{
    partial class frmModifyDues
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
            this.txtBusinessName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.bin = new BIN.BIN();
            this.txtOwnersName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dgvTaxFees = new System.Windows.Forms.DataGridView();
            this.frameWithShadow1 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow2 = new Amellar.Common.SOA.FrameWithShadow();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaxFees)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBusinessName
            // 
            this.txtBusinessName.Location = new System.Drawing.Point(111, 57);
            this.txtBusinessName.Name = "txtBusinessName";
            this.txtBusinessName.ReadOnly = true;
            this.txtBusinessName.Size = new System.Drawing.Size(423, 20);
            this.txtBusinessName.TabIndex = 59;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 58;
            this.label3.Text = "Business Name";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(156, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 55;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // bin
            // 
            this.bin.GetBINSeries = "";
            this.bin.GetDistCode = "";
            this.bin.GetLGUCode = "";
            this.bin.GetTaxYear = "";
            this.bin.Location = new System.Drawing.Point(12, 12);
            this.bin.Name = "bin";
            this.bin.Size = new System.Drawing.Size(138, 20);
            this.bin.TabIndex = 54;
            // 
            // txtOwnersName
            // 
            this.txtOwnersName.Location = new System.Drawing.Point(111, 81);
            this.txtOwnersName.Name = "txtOwnersName";
            this.txtOwnersName.ReadOnly = true;
            this.txtOwnersName.Size = new System.Drawing.Size(423, 20);
            this.txtOwnersName.TabIndex = 62;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 61;
            this.label5.Text = "Tax Payer\'s Name";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(322, 456);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(107, 23);
            this.btnSave.TabIndex = 64;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(435, 456);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(107, 23);
            this.btnCancel.TabIndex = 65;
            this.btnCancel.Text = "Cl&ose";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgvTaxFees
            // 
            this.dgvTaxFees.AllowUserToAddRows = false;
            this.dgvTaxFees.AllowUserToDeleteRows = false;
            this.dgvTaxFees.AllowUserToResizeRows = false;
            this.dgvTaxFees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTaxFees.Location = new System.Drawing.Point(19, 131);
            this.dgvTaxFees.Name = "dgvTaxFees";
            this.dgvTaxFees.RowHeadersVisible = false;
            this.dgvTaxFees.Size = new System.Drawing.Size(515, 282);
            this.dgvTaxFees.TabIndex = 66;
            this.dgvTaxFees.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTaxFees_RowLeave);
            this.dgvTaxFees.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTaxFees_CellEndEdit);
            this.dgvTaxFees.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTaxFees_CellClick);
            // 
            // frameWithShadow1
            // 
            this.frameWithShadow1.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow1.Location = new System.Drawing.Point(8, 122);
            this.frameWithShadow1.Name = "frameWithShadow1";
            this.frameWithShadow1.Size = new System.Drawing.Size(534, 328);
            this.frameWithShadow1.TabIndex = 63;
            // 
            // frameWithShadow2
            // 
            this.frameWithShadow2.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow2.Location = new System.Drawing.Point(8, 49);
            this.frameWithShadow2.Name = "frameWithShadow2";
            this.frameWithShadow2.Size = new System.Drawing.Size(534, 67);
            this.frameWithShadow2.TabIndex = 60;
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(435, 419);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.ReadOnly = true;
            this.txtTotal.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtTotal.Size = new System.Drawing.Size(99, 20);
            this.txtTotal.TabIndex = 68;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(393, 423);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 67;
            this.label1.Text = "Total";
            // 
            // frmModifyDues
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(548, 491);
            this.ControlBox = false;
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvTaxFees);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.frameWithShadow1);
            this.Controls.Add(this.txtOwnersName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBusinessName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.bin);
            this.Controls.Add(this.frameWithShadow2);
            this.Name = "frmModifyDues";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Previous Revenue Year Dues";
            this.Load += new System.EventHandler(this.frmModifyDues_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaxFees)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBusinessName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSearch;
        private BIN.BIN bin;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow2;
        private System.Windows.Forms.TextBox txtOwnersName;
        private System.Windows.Forms.Label label5;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow1;
        public System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgvTaxFees;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.Label label1;

    }
}