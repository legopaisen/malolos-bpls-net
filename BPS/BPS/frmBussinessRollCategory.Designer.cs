namespace BPLSBilling
{
    partial class frmBussinessRollCategory
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
            this.btnBnsRollList = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnBnsRollSummary = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.SuspendLayout();
            // 
            // btnBnsRollList
            // 
            this.btnBnsRollList.Location = new System.Drawing.Point(12, 12);
            this.btnBnsRollList.Name = "btnBnsRollList";
            this.btnBnsRollList.Size = new System.Drawing.Size(260, 41);
            this.btnBnsRollList.TabIndex = 2;
            this.btnBnsRollList.Values.Text = "Business Roll List Report";
            this.btnBnsRollList.Click += new System.EventHandler(this.btnBnsRollList_Click);
            // 
            // btnBnsRollSummary
            // 
            this.btnBnsRollSummary.Location = new System.Drawing.Point(12, 59);
            this.btnBnsRollSummary.Name = "btnBnsRollSummary";
            this.btnBnsRollSummary.Size = new System.Drawing.Size(260, 41);
            this.btnBnsRollSummary.TabIndex = 3;
            this.btnBnsRollSummary.Values.Text = "Summary of Economic Data";
            this.btnBnsRollSummary.Click += new System.EventHandler(this.btnBnsRollSummary_Click);
            // 
            // frmBussinessRollCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 110);
            this.Controls.Add(this.btnBnsRollSummary);
            this.Controls.Add(this.btnBnsRollList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBussinessRollCategory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Business Roll Category";
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnBnsRollList;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnBnsRollSummary;

    }
}