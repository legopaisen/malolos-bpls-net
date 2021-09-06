namespace BPLSBilling
{
    partial class frmManagementReport
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
            this.kryptonHeaderGroup1 = new ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup();
            this.btnRetirement = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnBnsQueApp = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnOwnership = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnListofBns = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnBnsRoll = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).BeginInit();
            this.kryptonHeaderGroup1.Panel.SuspendLayout();
            this.kryptonHeaderGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonHeaderGroup1
            // 
            this.kryptonHeaderGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonHeaderGroup1.GroupBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.ButtonButtonSpec;
            this.kryptonHeaderGroup1.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeaderGroup1.Name = "kryptonHeaderGroup1";
            // 
            // kryptonHeaderGroup1.Panel
            // 
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnRetirement);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnBnsQueApp);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnOwnership);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnListofBns);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnBnsRoll);
            this.kryptonHeaderGroup1.Size = new System.Drawing.Size(236, 237);
            this.kryptonHeaderGroup1.TabIndex = 0;
            this.kryptonHeaderGroup1.ValuesPrimary.Heading = "Businesses Reports";
            this.kryptonHeaderGroup1.ValuesPrimary.Image = global::BPLSBilling.Properties.Resources.summary_business;
            this.kryptonHeaderGroup1.ValuesSecondary.Heading = "";
            // 
            // btnRetirement
            // 
            this.btnRetirement.Location = new System.Drawing.Point(3, 100);
            this.btnRetirement.Name = "btnRetirement";
            this.btnRetirement.Size = new System.Drawing.Size(227, 42);
            this.btnRetirement.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnRetirement.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnRetirement.TabIndex = 0;
            this.btnRetirement.Values.Image = global::BPLSBilling.Properties.Resources.retirement_report;
            this.btnRetirement.Values.Text = "Retirement";
            this.btnRetirement.Click += new System.EventHandler(this.btnRetirement_Click);
            // 
            // btnBnsQueApp
            // 
            this.btnBnsQueApp.Location = new System.Drawing.Point(3, 148);
            this.btnBnsQueApp.Name = "btnBnsQueApp";
            this.btnBnsQueApp.Size = new System.Drawing.Size(227, 42);
            this.btnBnsQueApp.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnBnsQueApp.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnBnsQueApp.TabIndex = 0;
            this.btnBnsQueApp.Values.Image = global::BPLSBilling.Properties.Resources.OnQueue_Business;
            this.btnBnsQueApp.Values.Text = "Business On Queue / Application";
            this.btnBnsQueApp.Click += new System.EventHandler(this.btnBnsQueApp_Click);
            // 
            // btnOwnership
            // 
            this.btnOwnership.Location = new System.Drawing.Point(4, 197);
            this.btnOwnership.Name = "btnOwnership";
            this.btnOwnership.Size = new System.Drawing.Size(227, 42);
            this.btnOwnership.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnOwnership.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnOwnership.TabIndex = 0;
            this.btnOwnership.Values.Image = global::BPLSBilling.Properties.Resources.owership_record;
            this.btnOwnership.Values.Text = "Ownership Record Form";
            this.btnOwnership.Visible = false;
            this.btnOwnership.Click += new System.EventHandler(this.btnOwnership_Click);
            // 
            // btnListofBns
            // 
            this.btnListofBns.Location = new System.Drawing.Point(3, 6);
            this.btnListofBns.Name = "btnListofBns";
            this.btnListofBns.Size = new System.Drawing.Size(227, 42);
            this.btnListofBns.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnListofBns.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnListofBns.TabIndex = 0;
            this.btnListofBns.Values.Image = global::BPLSBilling.Properties.Resources.list_business;
            this.btnListofBns.Values.Text = "List of Business";
            this.btnListofBns.Click += new System.EventHandler(this.btnListofBns_Click);
            // 
            // btnBnsRoll
            // 
            this.btnBnsRoll.Location = new System.Drawing.Point(3, 53);
            this.btnBnsRoll.Name = "btnBnsRoll";
            this.btnBnsRoll.Size = new System.Drawing.Size(227, 42);
            this.btnBnsRoll.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnBnsRoll.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnBnsRoll.TabIndex = 0;
            this.btnBnsRoll.Values.Image = global::BPLSBilling.Properties.Resources.business_roll;
            this.btnBnsRoll.Values.Text = "Business Roll";
            this.btnBnsRoll.Click += new System.EventHandler(this.btnBnsRoll_Click);
            // 
            // frmManagementReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 237);
            this.Controls.Add(this.kryptonHeaderGroup1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmManagementReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmManagementReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).EndInit();
            this.kryptonHeaderGroup1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).EndInit();
            this.kryptonHeaderGroup1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroup1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnBnsRoll;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnListofBns;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnRetirement;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnBnsQueApp;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOwnership;
    }
}