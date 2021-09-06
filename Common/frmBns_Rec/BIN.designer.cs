namespace Amellar.Common.BIN
{
    partial class BIN
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtLGUCode = new System.Windows.Forms.TextBox();
            this.txtDistCode = new System.Windows.Forms.TextBox();
            this.txtTaxYear = new System.Windows.Forms.TextBox();
            this.txtBINSeries = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtLGUCode
            // 
            this.txtLGUCode.Location = new System.Drawing.Point(1, 0);
            this.txtLGUCode.MaxLength = 3;
            this.txtLGUCode.Name = "txtLGUCode";
            this.txtLGUCode.Size = new System.Drawing.Size(26, 20);
            this.txtLGUCode.TabIndex = 0;
            // 
            // txtDistCode
            // 
            this.txtDistCode.Location = new System.Drawing.Point(30, 0);
            this.txtDistCode.MaxLength = 2;
            this.txtDistCode.Name = "txtDistCode";
            this.txtDistCode.Size = new System.Drawing.Size(18, 20);
            this.txtDistCode.TabIndex = 1;
            // 
            // txtTaxYear
            // 
            this.txtTaxYear.Location = new System.Drawing.Point(52, 0);
            this.txtTaxYear.MaxLength = 4;
            this.txtTaxYear.Name = "txtTaxYear";
            this.txtTaxYear.Size = new System.Drawing.Size(31, 20);
            this.txtTaxYear.TabIndex = 2;
            this.txtTaxYear.TextChanged += new System.EventHandler(this.txtTaxYear_TextChanged);
            // 
            // txtBINSeries
            // 
            this.txtBINSeries.Location = new System.Drawing.Point(87, 0);
            this.txtBINSeries.MaxLength = 7;
            this.txtBINSeries.Name = "txtBINSeries";
            this.txtBINSeries.Size = new System.Drawing.Size(51, 20);
            this.txtBINSeries.TabIndex = 3;
            this.txtBINSeries.Leave += new System.EventHandler(this.txtBINSeries_Leave);
            // 
            // BIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtBINSeries);
            this.Controls.Add(this.txtTaxYear);
            this.Controls.Add(this.txtDistCode);
            this.Controls.Add(this.txtLGUCode);
            this.Name = "BIN";
            this.Size = new System.Drawing.Size(138, 20);
            //this.Load += new System.EventHandler(this.BIN_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txtLGUCode;
        public System.Windows.Forms.TextBox txtDistCode;
        public System.Windows.Forms.TextBox txtTaxYear;
        public System.Windows.Forms.TextBox txtBINSeries;

    }
}
