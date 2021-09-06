namespace BTAS
{
    partial class frmOtherReports
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
            this.btnDILG = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnFP = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCompAgree = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDefRec = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDCDELog = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPUP = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).BeginInit();
            this.kryptonHeaderGroup1.Panel.SuspendLayout();
            this.kryptonHeaderGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonHeaderGroup1
            // 
            this.kryptonHeaderGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonHeaderGroup1.HeaderVisibleSecondary = false;
            this.kryptonHeaderGroup1.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeaderGroup1.Name = "kryptonHeaderGroup1";
            // 
            // kryptonHeaderGroup1.Panel
            // 
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnDILG);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnFP);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnCompAgree);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnDefRec);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnDCDELog);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnPUP);
            this.kryptonHeaderGroup1.Size = new System.Drawing.Size(234, 135);
            this.kryptonHeaderGroup1.TabIndex = 0;
            this.kryptonHeaderGroup1.ValuesPrimary.Heading = "Others";
            this.kryptonHeaderGroup1.ValuesPrimary.Image = global::BTAS.Properties.Resources.other_reports;
            // 
            // btnDILG
            // 
            this.btnDILG.Location = new System.Drawing.Point(3, 194);
            this.btnDILG.Name = "btnDILG";
            this.btnDILG.Size = new System.Drawing.Size(227, 42);
            this.btnDILG.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnDILG.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnDILG.TabIndex = 5;
            this.btnDILG.Values.Image = global::BTAS.Properties.Resources.DILG;
            this.btnDILG.Values.Text = "DILG";
            this.btnDILG.Visible = false;
            this.btnDILG.Click += new System.EventHandler(this.btnDILG_Click);
            // 
            // btnFP
            // 
            this.btnFP.Location = new System.Drawing.Point(3, 243);
            this.btnFP.Name = "btnFP";
            this.btnFP.Size = new System.Drawing.Size(227, 42);
            this.btnFP.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnFP.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnFP.TabIndex = 6;
            this.btnFP.Values.Image = global::BTAS.Properties.Resources.certificate;
            this.btnFP.Values.Text = "Payment Certificate";
            this.btnFP.Visible = false;
            this.btnFP.Click += new System.EventHandler(this.btnFP_Click);
            // 
            // btnCompAgree
            // 
            this.btnCompAgree.Location = new System.Drawing.Point(3, 99);
            this.btnCompAgree.Name = "btnCompAgree";
            this.btnCompAgree.Size = new System.Drawing.Size(227, 42);
            this.btnCompAgree.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnCompAgree.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnCompAgree.TabIndex = 4;
            this.btnCompAgree.Values.Image = global::BTAS.Properties.Resources.compromise_agreement;
            this.btnCompAgree.Values.Text = "Compromise Agreement";
            this.btnCompAgree.Visible = false;
            this.btnCompAgree.Click += new System.EventHandler(this.btnCompAgree_Click);
            // 
            // btnDefRec
            // 
            this.btnDefRec.Location = new System.Drawing.Point(3, 51);
            this.btnDefRec.Name = "btnDefRec";
            this.btnDefRec.Size = new System.Drawing.Size(227, 42);
            this.btnDefRec.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnDefRec.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnDefRec.TabIndex = 3;
            this.btnDefRec.Values.Image = global::BTAS.Properties.Resources.deficiency_record;
            this.btnDefRec.Values.Text = "Deficient Records";
            this.btnDefRec.Click += new System.EventHandler(this.btnDefRec_Click);
            // 
            // btnDCDELog
            // 
            this.btnDCDELog.Location = new System.Drawing.Point(2, 286);
            this.btnDCDELog.Name = "btnDCDELog";
            this.btnDCDELog.Size = new System.Drawing.Size(227, 42);
            this.btnDCDELog.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnDCDELog.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnDCDELog.TabIndex = 2;
            this.btnDCDELog.Values.Image = global::BTAS.Properties.Resources.dc_ce_log;
            this.btnDCDELog.Values.Text = "DC/DE Log";
            this.btnDCDELog.Click += new System.EventHandler(this.btnDCDELog_Click);
            // 
            // btnPUP
            // 
            this.btnPUP.Location = new System.Drawing.Point(3, 3);
            this.btnPUP.Name = "btnPUP";
            this.btnPUP.Size = new System.Drawing.Size(227, 42);
            this.btnPUP.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnPUP.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnPUP.TabIndex = 1;
            this.btnPUP.Values.Image = global::BTAS.Properties.Resources.payment_protest;
            this.btnPUP.Values.Text = "Payment Under Protest";
            this.btnPUP.Click += new System.EventHandler(this.btnPUP_Click);
            // 
            // frmOtherReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 135);
            this.Controls.Add(this.kryptonHeaderGroup1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOtherReports";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).EndInit();
            this.kryptonHeaderGroup1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).EndInit();
            this.kryptonHeaderGroup1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroup1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPUP;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCompAgree;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDefRec;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDCDELog;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDILG;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnFP;
    }
}