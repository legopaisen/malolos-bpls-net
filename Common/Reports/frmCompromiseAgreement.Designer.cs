namespace Amellar.Common.Reports
{
    partial class frmCompromiseAgreement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCompromiseAgreement));
            this.btnOK = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.axVSPrinter1 = new AxVSPrinter7Lib.AxVSPrinter();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSettingPageSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSettingPrintPage = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.axVSPrinter1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(537, 460);
            this.btnOK.Name = "btnOK";
            this.btnOK.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOK.Size = new System.Drawing.Size(80, 25);
            this.btnOK.TabIndex = 35;
            this.btnOK.Values.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // axVSPrinter1
            // 
            this.axVSPrinter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axVSPrinter1.Location = new System.Drawing.Point(0, 0);
            this.axVSPrinter1.Name = "axVSPrinter1";
            this.axVSPrinter1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axVSPrinter1.OcxState")));
            this.axVSPrinter1.Size = new System.Drawing.Size(624, 433);
            this.axVSPrinter1.TabIndex = 39;
            this.axVSPrinter1.NewPageEvent += new System.EventHandler(this.axVSPrinter1_NewPageEvent);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.axVSPrinter1);
            this.splitContainer1.Size = new System.Drawing.Size(624, 463);
            this.splitContainer1.SplitterDistance = 433;
            this.splitContainer1.TabIndex = 40;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolPrint,
            this.toolSettings});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(624, 24);
            this.menuStrip1.TabIndex = 41;
            this.menuStrip1.Text = "menuStrip1";
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
            this.toolSettingPageSetup.Size = new System.Drawing.Size(152, 22);
            this.toolSettingPageSetup.Text = "Page Setup";
            this.toolSettingPageSetup.Click += new System.EventHandler(this.toolSettingPageSetup_Click);
            // 
            // toolSettingPrintPage
            // 
            this.toolSettingPrintPage.Name = "toolSettingPrintPage";
            this.toolSettingPrintPage.Size = new System.Drawing.Size(152, 22);
            this.toolSettingPrintPage.Text = "Printer Setup";
            this.toolSettingPrintPage.Click += new System.EventHandler(this.toolSettingPrintPage_Click);
            // 
            // frmCompromiseAgreement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 487);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.Name = "frmCompromiseAgreement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report";
            this.Load += new System.EventHandler(this.frmCompromiseAgreement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axVSPrinter1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOK;
        private AxVSPrinter7Lib.AxVSPrinter axVSPrinter1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolPrint;
        private System.Windows.Forms.ToolStripMenuItem toolSettings;
        private System.Windows.Forms.ToolStripMenuItem toolSettingPageSetup;
        private System.Windows.Forms.ToolStripMenuItem toolSettingPrintPage;
    }
}