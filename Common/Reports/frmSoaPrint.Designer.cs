namespace Amellar.Common.Reports
{
    partial class frmSoaPrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSoaPrint));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSettingPageSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSettingPrintPage = new System.Windows.Forms.ToolStripMenuItem();
            this.axVSPrinter1 = new AxVSPrinter7Lib.AxVSPrinter();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axVSPrinter1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolPrint,
            this.toolSettings});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(839, 24);
            this.menuStrip1.TabIndex = 42;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // ToolPrint
            // 
            this.ToolPrint.Name = "ToolPrint";
            this.ToolPrint.Size = new System.Drawing.Size(44, 20);
            this.ToolPrint.Text = "Print";
            this.ToolPrint.Click += new System.EventHandler(this.ToolPrint_Click);
            // 
            // toolSettings
            // 
            this.toolSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolSettingPageSetup,
            this.toolSettingPrintPage});
            this.toolSettings.Name = "toolSettings";
            this.toolSettings.Size = new System.Drawing.Size(61, 20);
            this.toolSettings.Text = "Settings";
            // 
            // toolSettingPageSetup
            // 
            this.toolSettingPageSetup.Name = "toolSettingPageSetup";
            this.toolSettingPageSetup.Size = new System.Drawing.Size(142, 22);
            this.toolSettingPageSetup.Text = "Page Setup";
            this.toolSettingPageSetup.Click += new System.EventHandler(this.toolSettingPageSetup_Click);
            // 
            // toolSettingPrintPage
            // 
            this.toolSettingPrintPage.Name = "toolSettingPrintPage";
            this.toolSettingPrintPage.Size = new System.Drawing.Size(142, 22);
            this.toolSettingPrintPage.Text = "Printer Setup";
            this.toolSettingPrintPage.Click += new System.EventHandler(this.toolSettingPrintPage_Click);
            // 
            // axVSPrinter1
            // 
            this.axVSPrinter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axVSPrinter1.Location = new System.Drawing.Point(0, 0);
            this.axVSPrinter1.Name = "axVSPrinter1";
            this.axVSPrinter1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axVSPrinter1.OcxState")));
            this.axVSPrinter1.Size = new System.Drawing.Size(839, 508);
            this.axVSPrinter1.TabIndex = 40;
            this.axVSPrinter1.StartDocEvent += new System.EventHandler(this.axVSPrinter1_StartDocEvent);
            // 
            // frmSoaPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 508);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.axVSPrinter1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmSoaPrint";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Soa Print";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmSoaPrint_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axVSPrinter1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
  
        private AxVSPrinter7Lib.AxVSPrinter axVSPrinter1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolPrint;
        private System.Windows.Forms.ToolStripMenuItem toolSettings;
        private System.Windows.Forms.ToolStripMenuItem toolSettingPageSetup;
        private System.Windows.Forms.ToolStripMenuItem toolSettingPrintPage;

    }
}