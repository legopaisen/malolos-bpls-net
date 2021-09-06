namespace BPLSBilling
{
    partial class frmList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmList));
            this.kryptonHeaderGroup1 = new ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup();
            this.btnEarlyBird = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDILG = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDCDELog = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnBOB = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnGender = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnListofBnsEmpGen = new ComponentFactory.Krypton.Toolkit.KryptonButton();
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
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnEarlyBird);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnDILG);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnDCDELog);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnBOB);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnGender);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnListofBnsEmpGen);
            this.kryptonHeaderGroup1.Size = new System.Drawing.Size(236, 277);
            this.kryptonHeaderGroup1.TabIndex = 0;
            this.kryptonHeaderGroup1.ValuesPrimary.Heading = "Lists";
            this.kryptonHeaderGroup1.ValuesPrimary.Image = global::BPLSBilling.Properties.Resources.reportlist_gender;
            // 
            // btnEarlyBird
            // 
            this.btnEarlyBird.Location = new System.Drawing.Point(3, 195);
            this.btnEarlyBird.Name = "btnEarlyBird";
            this.btnEarlyBird.Size = new System.Drawing.Size(227, 42);
            this.btnEarlyBird.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnEarlyBird.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnEarlyBird.TabIndex = 7;
            this.btnEarlyBird.Values.Image = ((System.Drawing.Image)(resources.GetObject("btnEarlyBird.Values.Image")));
            this.btnEarlyBird.Values.Text = "Early Bird";
            this.btnEarlyBird.Click += new System.EventHandler(this.btnEarlyBird_Click);
            // 
            // btnDILG
            // 
            this.btnDILG.Location = new System.Drawing.Point(3, 147);
            this.btnDILG.Name = "btnDILG";
            this.btnDILG.Size = new System.Drawing.Size(227, 42);
            this.btnDILG.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnDILG.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnDILG.TabIndex = 6;
            this.btnDILG.Values.Image = global::BPLSBilling.Properties.Resources.DILG;
            this.btnDILG.Values.Text = "DILG";
            this.btnDILG.Click += new System.EventHandler(this.btnDILG_Click);
            // 
            // btnDCDELog
            // 
            this.btnDCDELog.Location = new System.Drawing.Point(4, 243);
            this.btnDCDELog.Name = "btnDCDELog";
            this.btnDCDELog.Size = new System.Drawing.Size(227, 42);
            this.btnDCDELog.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnDCDELog.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnDCDELog.TabIndex = 3;
            this.btnDCDELog.Values.Image = ((System.Drawing.Image)(resources.GetObject("btnDCDELog.Values.Image")));
            this.btnDCDELog.Values.Text = "DC/DE Log";
            this.btnDCDELog.Click += new System.EventHandler(this.btnDCDELog_Click);
            // 
            // btnBOB
            // 
            this.btnBOB.Location = new System.Drawing.Point(3, 51);
            this.btnBOB.Name = "btnBOB";
            this.btnBOB.Size = new System.Drawing.Size(227, 42);
            this.btnBOB.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnBOB.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnBOB.TabIndex = 1;
            this.btnBOB.Values.Image = global::BPLSBilling.Properties.Resources.birthday;
            this.btnBOB.Values.Text = "Business Owner\'s Birthday";
            this.btnBOB.Click += new System.EventHandler(this.btnBOB_Click);
            // 
            // btnGender
            // 
            this.btnGender.Location = new System.Drawing.Point(3, 99);
            this.btnGender.Name = "btnGender";
            this.btnGender.Size = new System.Drawing.Size(227, 42);
            this.btnGender.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnGender.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnGender.TabIndex = 1;
            this.btnGender.Values.Image = global::BPLSBilling.Properties.Resources.reportlist_gender;
            this.btnGender.Values.Text = "Owner Gender";
            this.btnGender.Click += new System.EventHandler(this.btnGender_Click);
            // 
            // btnListofBnsEmpGen
            // 
            this.btnListofBnsEmpGen.Location = new System.Drawing.Point(3, 3);
            this.btnListofBnsEmpGen.Name = "btnListofBnsEmpGen";
            this.btnListofBnsEmpGen.Size = new System.Drawing.Size(227, 42);
            this.btnListofBnsEmpGen.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnListofBnsEmpGen.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnListofBnsEmpGen.TabIndex = 1;
            this.btnListofBnsEmpGen.Values.Image = global::BPLSBilling.Properties.Resources.List_Employee_Gender;
            this.btnListofBnsEmpGen.Values.Text = "Business with Employee Gender";
            this.btnListofBnsEmpGen.Click += new System.EventHandler(this.btnListofBnsEmpGen_Click);
            // 
            // frmList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 277);
            this.Controls.Add(this.kryptonHeaderGroup1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmList";
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
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnListofBnsEmpGen;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnBOB;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGender;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDCDELog;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDILG;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEarlyBird;
    }
}