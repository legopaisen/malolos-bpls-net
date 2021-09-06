namespace Amellar.Modules.OwnerProfile
{
    partial class frmOwnershipRecord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOwnershipRecord));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnOK = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtAcctCode = new System.Windows.Forms.TextBox();
            this.txtOwName = new System.Windows.Forms.TextBox();
            this.txtOwAdd = new System.Windows.Forms.TextBox();
            this.btnClearFields = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSearchOwner = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.axVSPrinter1 = new AxVSPrinter7Lib.AxVSPrinter();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSettingPageSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSettingPrintPage = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.axVSPrinter1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 485);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Account Code";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 512);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Owner\'s Name";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 539);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Owner\'s Address";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(304, 561);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(62, 25);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "C&ancel";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "C&ancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(236, 561);
            this.btnOK.Name = "btnOK";
            this.btnOK.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOK.Size = new System.Drawing.Size(62, 25);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "&OK";
            this.btnOK.Values.ExtraText = "";
            this.btnOK.Values.Image = null;
            this.btnOK.Values.ImageStates.ImageCheckedNormal = null;
            this.btnOK.Values.ImageStates.ImageCheckedPressed = null;
            this.btnOK.Values.ImageStates.ImageCheckedTracking = null;
            this.btnOK.Values.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtAcctCode
            // 
            this.txtAcctCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtAcctCode.Location = new System.Drawing.Point(115, 481);
            this.txtAcctCode.Name = "txtAcctCode";
            this.txtAcctCode.Size = new System.Drawing.Size(88, 20);
            this.txtAcctCode.TabIndex = 13;
            this.txtAcctCode.TextChanged += new System.EventHandler(this.txtAcctCode_TextChanged);
            this.txtAcctCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAcctCode_KeyPress);
            // 
            // txtOwName
            // 
            this.txtOwName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtOwName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtOwName.Location = new System.Drawing.Point(115, 508);
            this.txtOwName.Name = "txtOwName";
            this.txtOwName.Size = new System.Drawing.Size(251, 20);
            this.txtOwName.TabIndex = 13;
            this.txtOwName.TextChanged += new System.EventHandler(this.txtOwName_TextChanged);
            // 
            // txtOwAdd
            // 
            this.txtOwAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtOwAdd.Enabled = false;
            this.txtOwAdd.Location = new System.Drawing.Point(115, 535);
            this.txtOwAdd.Name = "txtOwAdd";
            this.txtOwAdd.ReadOnly = true;
            this.txtOwAdd.Size = new System.Drawing.Size(251, 20);
            this.txtOwAdd.TabIndex = 13;
            // 
            // btnClearFields
            // 
            this.btnClearFields.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearFields.Location = new System.Drawing.Point(287, 476);
            this.btnClearFields.Name = "btnClearFields";
            this.btnClearFields.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClearFields.Size = new System.Drawing.Size(62, 25);
            this.btnClearFields.TabIndex = 15;
            this.btnClearFields.Text = "&Clear";
            this.btnClearFields.Values.ExtraText = "";
            this.btnClearFields.Values.Image = null;
            this.btnClearFields.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClearFields.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClearFields.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClearFields.Values.Text = "&Clear";
            this.btnClearFields.Click += new System.EventHandler(this.btnClearFields_Click);
            // 
            // btnSearchOwner
            // 
            this.btnSearchOwner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearchOwner.Location = new System.Drawing.Point(219, 476);
            this.btnSearchOwner.Name = "btnSearchOwner";
            this.btnSearchOwner.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearchOwner.Size = new System.Drawing.Size(62, 25);
            this.btnSearchOwner.TabIndex = 14;
            this.btnSearchOwner.Text = "&Search";
            this.btnSearchOwner.Values.ExtraText = "";
            this.btnSearchOwner.Values.Image = null;
            this.btnSearchOwner.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearchOwner.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearchOwner.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearchOwner.Values.Text = "&Search";
            this.btnSearchOwner.Click += new System.EventHandler(this.btnSearchOwner_Click);
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(9, 435);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader1.Size = new System.Drawing.Size(370, 24);
            this.kryptonHeader1.TabIndex = 17;
            this.kryptonHeader1.Text = "Business Ownership Record Form";
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Business Ownership Record Form";
            this.kryptonHeader1.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader1.Values.Image")));
            // 
            // axVSPrinter1
            // 
            this.axVSPrinter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axVSPrinter1.Location = new System.Drawing.Point(0, 0);
            this.axVSPrinter1.Name = "axVSPrinter1";
            this.axVSPrinter1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axVSPrinter1.OcxState")));
            this.axVSPrinter1.Size = new System.Drawing.Size(505, 604);
            this.axVSPrinter1.TabIndex = 19;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.containerWithShadow1.Location = new System.Drawing.Point(6, 435);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(374, 167);
            this.containerWithShadow1.TabIndex = 18;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolPrint,
            this.toolSettings});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(505, 24);
            this.menuStrip1.TabIndex = 20;
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
            // frmOwnershipRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 604);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.axVSPrinter1);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.btnClearFields);
            this.Controls.Add(this.btnSearchOwner);
            this.Controls.Add(this.txtOwAdd);
            this.Controls.Add(this.txtOwName);
            this.Controls.Add(this.txtAcctCode);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.containerWithShadow1);
            this.Name = "frmOwnershipRecord";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ownership Record";
            this.Load += new System.EventHandler(this.frmOwnershipRecord_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axVSPrinter1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOK;
        private System.Windows.Forms.TextBox txtAcctCode;
        private System.Windows.Forms.TextBox txtOwName;
        private System.Windows.Forms.TextBox txtOwAdd;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClearFields;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearchOwner;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private AxVSPrinter7Lib.AxVSPrinter axVSPrinter1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolPrint;
        private System.Windows.Forms.ToolStripMenuItem toolSettings;
        private System.Windows.Forms.ToolStripMenuItem toolSettingPageSetup;
        private System.Windows.Forms.ToolStripMenuItem toolSettingPrintPage;
    }
}