namespace Amellar.Common.frmBns_Rec
{
    partial class frmAppForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAppForm));
            this.axVSPrinter1 = new AxVSPrinter7Lib.AxVSPrinter();
            this.vsPrinterEmuDocument1 = new Amellar.Common.PrintUtilities.VSPrinterEmuDocument();
            ((System.ComponentModel.ISupportInitialize)(this.axVSPrinter1)).BeginInit();
            this.SuspendLayout();
            // 
            // axVSPrinter1
            // 
            this.axVSPrinter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axVSPrinter1.Location = new System.Drawing.Point(0, 0);
            this.axVSPrinter1.Name = "axVSPrinter1";
            this.axVSPrinter1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axVSPrinter1.OcxState")));
            this.axVSPrinter1.Size = new System.Drawing.Size(424, 401);
            this.axVSPrinter1.TabIndex = 0;
            // 
            // vsPrinterEmuDocument1
            // 
            this.vsPrinterEmuDocument1.Model = null;
            this.vsPrinterEmuDocument1.PageHeaderModel = null;
            // 
            // frmAppForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 401);
            this.Controls.Add(this.axVSPrinter1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmAppForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Application Form";
            this.Load += new System.EventHandler(this.frmAppForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axVSPrinter1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxVSPrinter7Lib.AxVSPrinter axVSPrinter1;
        private Amellar.Common.PrintUtilities.VSPrinterEmuDocument vsPrinterEmuDocument1;

    }
}