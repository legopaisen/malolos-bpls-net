namespace SOAForm
{
    partial class frmSOA
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
            this.btnSearchBin = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label1 = new System.Windows.Forms.Label();
            this.bin1 = new PaymentHistory.BIN();
            this.SuspendLayout();
            // 
            // btnSearchBin
            // 
            this.btnSearchBin.Location = new System.Drawing.Point(191, 12);
            this.btnSearchBin.Name = "btnSearchBin";
            this.btnSearchBin.Size = new System.Drawing.Size(90, 25);
            this.btnSearchBin.TabIndex = 0;
            this.btnSearchBin.Values.Text = "kryptonButton1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "BIN:";
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(48, 15);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(138, 20);
            this.bin1.TabIndex = 2;
            // 
            // frmSOA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 556);
            this.Controls.Add(this.bin1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSearchBin);
            this.Name = "frmSOA";
            this.Text = "Statement of Account";
            this.Load += new System.EventHandler(this.frmSOA_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearchBin;
        private System.Windows.Forms.Label label1;
        private PaymentHistory.BIN bin1;
    }
}