namespace BPLSBilling
{
    partial class frmSpecial
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
            this.label2 = new System.Windows.Forms.Label();
            this.cmbFeesDesc = new System.Windows.Forms.ComboBox();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtBnsCode = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Business Type Code:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Fees Desc:";
            // 
            // cmbFeesDesc
            // 
            this.cmbFeesDesc.FormattingEnabled = true;
            this.cmbFeesDesc.Location = new System.Drawing.Point(120, 50);
            this.cmbFeesDesc.Name = "cmbFeesDesc";
            this.cmbFeesDesc.Size = new System.Drawing.Size(155, 21);
            this.cmbFeesDesc.TabIndex = 3;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(193, 85);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(78, 24);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Values.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // txtBnsCode
            // 
            this.txtBnsCode.Location = new System.Drawing.Point(120, 24);
            this.txtBnsCode.MaxLength = 2;
            this.txtBnsCode.Name = "txtBnsCode";
            this.txtBnsCode.Size = new System.Drawing.Size(58, 20);
            this.txtBnsCode.TabIndex = 5;
            // 
            // frmSpecial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 130);
            this.Controls.Add(this.txtBnsCode);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.cmbFeesDesc);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "frmSpecial";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Special";
            this.Load += new System.EventHandler(this.frmSpecial_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbFeesDesc;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private System.Windows.Forms.TextBox txtBnsCode;
    }
}