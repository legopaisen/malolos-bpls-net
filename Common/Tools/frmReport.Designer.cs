namespace Amellar.Common.Tools
{
    partial class frmReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReport));
            this.axVSPrinter1 = new AxVSPrinter7Lib.AxVSPrinter();
            ((System.ComponentModel.ISupportInitialize)(this.axVSPrinter1)).BeginInit();
            this.SuspendLayout();
            // 
            // axVSPrinter1
            // 
            this.axVSPrinter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axVSPrinter1.Location = new System.Drawing.Point(0, 0);
            this.axVSPrinter1.Name = "axVSPrinter1";
            this.axVSPrinter1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axVSPrinter1.OcxState")));
            this.axVSPrinter1.Size = new System.Drawing.Size(420, 345);
            this.axVSPrinter1.TabIndex = 0;
            this.axVSPrinter1.NewPageEvent += new System.EventHandler(this.axVSPrinter1_NewPageEvent);
            this.axVSPrinter1.BeforeFooter += new System.EventHandler(this.axVSPrinter1_BeforeFooter);
            // 
            // frmReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 345);
            this.Controls.Add(this.axVSPrinter1);
            this.Name = "frmReport";
            this.ShowIcon = false;
            this.Text = "Report";
            this.Load += new System.EventHandler(this.frmReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axVSPrinter1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxVSPrinter7Lib.AxVSPrinter axVSPrinter1;
    }
}