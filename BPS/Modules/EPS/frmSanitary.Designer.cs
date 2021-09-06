namespace Amellar.Modules.EPS
{
    partial class frmSanitary
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
            this.label1 = new System.Windows.Forms.Label();
            this.bin = new BIN.BIN();
            this.lblOwnerName = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblBnsName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.Address = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblSignatory = new System.Windows.Forms.Label();
            this.cmbSignatory = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpDtIssued = new System.Windows.Forms.DateTimePicker();
            this.gbInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "BIN";
            // 
            // bin
            // 
            this.bin.GetBINSeries = "";
            this.bin.GetDistCode = "";
            this.bin.GetLGUCode = "";
            this.bin.GetTaxYear = "";
            this.bin.Location = new System.Drawing.Point(44, 10);
            this.bin.Name = "bin";
            this.bin.Size = new System.Drawing.Size(138, 20);
            this.bin.TabIndex = 10;
            // 
            // lblOwnerName
            // 
            this.lblOwnerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOwnerName.Location = new System.Drawing.Point(109, 91);
            this.lblOwnerName.Name = "lblOwnerName";
            this.lblOwnerName.Size = new System.Drawing.Size(143, 53);
            this.lblOwnerName.TabIndex = 0;
            this.lblOwnerName.Text = "Owner Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(20, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Owner Name";
            // 
            // lblBnsName
            // 
            this.lblBnsName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBnsName.Location = new System.Drawing.Point(109, 43);
            this.lblBnsName.Name = "lblBnsName";
            this.lblBnsName.Size = new System.Drawing.Size(143, 49);
            this.lblBnsName.TabIndex = 0;
            this.lblBnsName.Text = "Business Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(14, 30);
            this.label5.MaximumSize = new System.Drawing.Size(130, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 26);
            this.label5.TabIndex = 0;
            this.label5.Text = "Name of Establishment";
            // 
            // btnPrint
            // 
            this.btnPrint.Enabled = false;
            this.btnPrint.Location = new System.Drawing.Point(195, 35);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 12;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(195, 8);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 11;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // gbInfo
            // 
            this.gbInfo.Controls.Add(this.Address);
            this.gbInfo.Controls.Add(this.lblOwnerName);
            this.gbInfo.Controls.Add(this.label6);
            this.gbInfo.Controls.Add(this.lblBnsName);
            this.gbInfo.Controls.Add(this.label5);
            this.gbInfo.Controls.Add(this.lblAddress);
            this.gbInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInfo.Location = new System.Drawing.Point(11, 170);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new System.Drawing.Size(263, 222);
            this.gbInfo.TabIndex = 13;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "INFORMATION";
            // 
            // Address
            // 
            this.Address.AutoSize = true;
            this.Address.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Address.Location = new System.Drawing.Point(26, 148);
            this.Address.Name = "Address";
            this.Address.Size = new System.Drawing.Size(52, 13);
            this.Address.TabIndex = 1;
            this.Address.Text = "Address";
            // 
            // lblAddress
            // 
            this.lblAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress.Location = new System.Drawing.Point(109, 149);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(143, 53);
            this.lblAddress.TabIndex = 3;
            this.lblAddress.Text = "Address";
            // 
            // lblSignatory
            // 
            this.lblSignatory.AutoSize = true;
            this.lblSignatory.Location = new System.Drawing.Point(15, 69);
            this.lblSignatory.Name = "lblSignatory";
            this.lblSignatory.Size = new System.Drawing.Size(51, 13);
            this.lblSignatory.TabIndex = 15;
            this.lblSignatory.Text = "Signatory";
            // 
            // cmbSignatory
            // 
            this.cmbSignatory.FormattingEnabled = true;
            this.cmbSignatory.Location = new System.Drawing.Point(11, 86);
            this.cmbSignatory.Name = "cmbSignatory";
            this.cmbSignatory.Size = new System.Drawing.Size(263, 21);
            this.cmbSignatory.TabIndex = 14;
            this.cmbSignatory.SelectedIndexChanged += new System.EventHandler(this.cmbSignatory_SelectedIndexChanged);
            this.cmbSignatory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbSignatory_KeyPress);
            this.cmbSignatory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbSignatory_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Date Issued";
            // 
            // dtpDtIssued
            // 
            this.dtpDtIssued.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDtIssued.Location = new System.Drawing.Point(11, 134);
            this.dtpDtIssued.Name = "dtpDtIssued";
            this.dtpDtIssued.Size = new System.Drawing.Size(111, 20);
            this.dtpDtIssued.TabIndex = 20;
            // 
            // frmSanitary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 403);
            this.Controls.Add(this.dtpDtIssued);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblSignatory);
            this.Controls.Add(this.cmbSignatory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bin);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.gbInfo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSanitary";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sanitary Clearance";
            this.Load += new System.EventHandler(this.frmSanitary_Load);
            this.gbInfo.ResumeLayout(false);
            this.gbInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private BIN.BIN bin;
        private System.Windows.Forms.Label lblOwnerName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblBnsName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.GroupBox gbInfo;
        private System.Windows.Forms.Label Address;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblSignatory;
        private System.Windows.Forms.ComboBox cmbSignatory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpDtIssued;
    }
}