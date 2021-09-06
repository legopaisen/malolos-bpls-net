namespace BPLSBilling
{
    partial class frmSummary
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
            this.btnListofSec = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnListofBns = new ComponentFactory.Krypton.Toolkit.KryptonButton();
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
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnListofSec);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnListofBns);
            this.kryptonHeaderGroup1.Size = new System.Drawing.Size(236, 137);
            this.kryptonHeaderGroup1.TabIndex = 0;
            this.kryptonHeaderGroup1.ValuesPrimary.Heading = "Summary by";
            this.kryptonHeaderGroup1.ValuesPrimary.Image = global::BPLSBilling.Properties.Resources.summaryreport_brgy_sec;
            this.kryptonHeaderGroup1.ValuesSecondary.Heading = "";
            // 
            // btnListofSec
            // 
            this.btnListofSec.Location = new System.Drawing.Point(3, 49);
            this.btnListofSec.Name = "btnListofSec";
            this.btnListofSec.Size = new System.Drawing.Size(227, 42);
            this.btnListofSec.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnListofSec.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnListofSec.TabIndex = 1;
            this.btnListofSec.Values.Image = global::BPLSBilling.Properties.Resources.Summary_business_by_Section;
            this.btnListofSec.Values.Text = "Section";
            this.btnListofSec.Click += new System.EventHandler(this.btnListofSec_Click);
            // 
            // btnListofBns
            // 
            this.btnListofBns.Location = new System.Drawing.Point(3, 5);
            this.btnListofBns.Name = "btnListofBns";
            this.btnListofBns.Size = new System.Drawing.Size(227, 42);
            this.btnListofBns.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnListofBns.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnListofBns.TabIndex = 1;
            this.btnListofBns.Values.Image = global::BPLSBilling.Properties.Resources.summary_business;
            this.btnListofBns.Values.Text = "Barangay";
            this.btnListofBns.Click += new System.EventHandler(this.btnListofBns_Click);
            // 
            // frmSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 137);
            this.Controls.Add(this.kryptonHeaderGroup1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSummary";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Summaries";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).EndInit();
            this.kryptonHeaderGroup1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).EndInit();
            this.kryptonHeaderGroup1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroup1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnListofBns;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnListofSec;
    }
}