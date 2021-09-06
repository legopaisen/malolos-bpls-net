namespace Amellar.Modules.EPS
{
    partial class frmClearance
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
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.lblDtIssued = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblOwnerName = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblBnsName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblControlNum = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.bin = new BIN.BIN();
            this.lblClass = new System.Windows.Forms.Label();
            this.txtClassification = new System.Windows.Forms.TextBox();
            this.cmbSignatory = new System.Windows.Forms.ComboBox();
            this.lblSignatory = new System.Windows.Forms.Label();
            this.lblSignatory2 = new System.Windows.Forms.Label();
            this.cmbSignatory2 = new System.Windows.Forms.ComboBox();
            this.gbInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "BIN";
            // 
            // gbInfo
            // 
            this.gbInfo.Controls.Add(this.lblDtIssued);
            this.gbInfo.Controls.Add(this.label3);
            this.gbInfo.Controls.Add(this.lblOwnerName);
            this.gbInfo.Controls.Add(this.label6);
            this.gbInfo.Controls.Add(this.lblBnsName);
            this.gbInfo.Controls.Add(this.label5);
            this.gbInfo.Controls.Add(this.lblStatus);
            this.gbInfo.Controls.Add(this.label4);
            this.gbInfo.Controls.Add(this.lblControlNum);
            this.gbInfo.Controls.Add(this.label2);
            this.gbInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInfo.Location = new System.Drawing.Point(12, 188);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new System.Drawing.Size(263, 227);
            this.gbInfo.TabIndex = 4;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "INFORMATION";
            // 
            // lblDtIssued
            // 
            this.lblDtIssued.AutoSize = true;
            this.lblDtIssued.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDtIssued.Location = new System.Drawing.Point(109, 201);
            this.lblDtIssued.Name = "lblDtIssued";
            this.lblDtIssued.Size = new System.Drawing.Size(63, 13);
            this.lblDtIssued.TabIndex = 0;
            this.lblDtIssued.Text = "Date issued";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(26, 201);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Date issued";
            // 
            // lblOwnerName
            // 
            this.lblOwnerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOwnerName.Location = new System.Drawing.Point(109, 147);
            this.lblOwnerName.Name = "lblOwnerName";
            this.lblOwnerName.Size = new System.Drawing.Size(143, 53);
            this.lblOwnerName.TabIndex = 0;
            this.lblOwnerName.Text = "Owner Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(20, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Owner Name";
            // 
            // lblBnsName
            // 
            this.lblBnsName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBnsName.Location = new System.Drawing.Point(109, 89);
            this.lblBnsName.Name = "lblBnsName";
            this.lblBnsName.Size = new System.Drawing.Size(143, 49);
            this.lblBnsName.TabIndex = 0;
            this.lblBnsName.Text = "Business Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Business Name";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(109, 56);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(52, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Status";
            // 
            // lblControlNum
            // 
            this.lblControlNum.AutoSize = true;
            this.lblControlNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblControlNum.Location = new System.Drawing.Point(109, 23);
            this.lblControlNum.Name = "lblControlNum";
            this.lblControlNum.Size = new System.Drawing.Size(80, 13);
            this.lblControlNum.TabIndex = 0;
            this.lblControlNum.Text = "Control Number";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Control Number";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(196, 15);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Enabled = false;
            this.btnPrint.Location = new System.Drawing.Point(196, 42);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // bin
            // 
            this.bin.GetBINSeries = "";
            this.bin.GetDistCode = "";
            this.bin.GetLGUCode = "";
            this.bin.GetTaxYear = "";
            this.bin.Location = new System.Drawing.Point(45, 17);
            this.bin.Name = "bin";
            this.bin.Size = new System.Drawing.Size(138, 20);
            this.bin.TabIndex = 1;
            // 
            // lblClass
            // 
            this.lblClass.AutoSize = true;
            this.lblClass.Location = new System.Drawing.Point(16, 64);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(68, 13);
            this.lblClass.TabIndex = 5;
            this.lblClass.Text = "Classification";
            this.lblClass.Visible = false;
            // 
            // txtClassification
            // 
            this.txtClassification.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtClassification.Location = new System.Drawing.Point(12, 80);
            this.txtClassification.Name = "txtClassification";
            this.txtClassification.Size = new System.Drawing.Size(263, 20);
            this.txtClassification.TabIndex = 6;
            this.txtClassification.Visible = false;
            // 
            // cmbSignatory
            // 
            this.cmbSignatory.FormattingEnabled = true;
            this.cmbSignatory.Location = new System.Drawing.Point(12, 120);
            this.cmbSignatory.Name = "cmbSignatory";
            this.cmbSignatory.Size = new System.Drawing.Size(263, 21);
            this.cmbSignatory.TabIndex = 7;
            this.cmbSignatory.Visible = false;
            this.cmbSignatory.SelectedIndexChanged += new System.EventHandler(this.cmbSignatory_SelectedIndexChanged);
            // 
            // lblSignatory
            // 
            this.lblSignatory.AutoSize = true;
            this.lblSignatory.Location = new System.Drawing.Point(16, 103);
            this.lblSignatory.Name = "lblSignatory";
            this.lblSignatory.Size = new System.Drawing.Size(70, 13);
            this.lblSignatory.TabIndex = 8;
            this.lblSignatory.Text = "Evaluated By";
            this.lblSignatory.Visible = false;
            // 
            // lblSignatory2
            // 
            this.lblSignatory2.AutoSize = true;
            this.lblSignatory2.Location = new System.Drawing.Point(16, 144);
            this.lblSignatory2.Name = "lblSignatory2";
            this.lblSignatory2.Size = new System.Drawing.Size(68, 13);
            this.lblSignatory2.TabIndex = 10;
            this.lblSignatory2.Text = "Approved By";
            this.lblSignatory2.Visible = false;
            // 
            // cmbSignatory2
            // 
            this.cmbSignatory2.FormattingEnabled = true;
            this.cmbSignatory2.Location = new System.Drawing.Point(12, 161);
            this.cmbSignatory2.Name = "cmbSignatory2";
            this.cmbSignatory2.Size = new System.Drawing.Size(263, 21);
            this.cmbSignatory2.TabIndex = 9;
            this.cmbSignatory2.Visible = false;
            this.cmbSignatory2.SelectedIndexChanged += new System.EventHandler(this.cmbSignatory2_SelectedIndexChanged);
            // 
            // frmClearance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 423);
            this.Controls.Add(this.lblSignatory2);
            this.Controls.Add(this.cmbSignatory2);
            this.Controls.Add(this.lblSignatory);
            this.Controls.Add(this.cmbSignatory);
            this.Controls.Add(this.txtClassification);
            this.Controls.Add(this.lblClass);
            this.Controls.Add(this.bin);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gbInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmClearance";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clearance";
            this.Load += new System.EventHandler(this.frmClearance_Load);
            this.gbInfo.ResumeLayout(false);
            this.gbInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDtIssued;
        private System.Windows.Forms.Label lblOwnerName;
        private System.Windows.Forms.Label lblBnsName;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnGenerate;
        private BIN.BIN bin;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblClass;
        private System.Windows.Forms.TextBox txtClassification;
        private System.Windows.Forms.ComboBox cmbSignatory;
        private System.Windows.Forms.Label lblSignatory;
        private System.Windows.Forms.Label lblControlNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSignatory2;
        private System.Windows.Forms.ComboBox cmbSignatory2;
    }
}