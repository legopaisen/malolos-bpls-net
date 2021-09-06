namespace Amellar.Modules.BusinessReports
{
    partial class frmCertifications
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCertifications));
            this.btnBusOwnership = new System.Windows.Forms.Button();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCertStatus = new System.Windows.Forms.Button();
            this.btnCertBussPermit = new System.Windows.Forms.Button();
            this.btnWithBuss = new System.Windows.Forms.Button();
            this.btnNoBusiness = new System.Windows.Forms.Button();
            this.btnApplication = new System.Windows.Forms.Button();
            this.btnRetirement = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBusOwnership
            // 
            this.btnBusOwnership.Location = new System.Drawing.Point(8, 287);
            this.btnBusOwnership.Name = "btnBusOwnership";
            this.btnBusOwnership.Size = new System.Drawing.Size(47, 38);
            this.btnBusOwnership.TabIndex = 0;
            this.btnBusOwnership.Text = "Business Ownership";
            this.btnBusOwnership.UseVisualStyleBackColor = true;
            this.btnBusOwnership.Visible = false;
            this.btnBusOwnership.Click += new System.EventHandler(this.btnBusOwnership_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(163, 301);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(74, 24);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Close";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCertStatus);
            this.groupBox1.Controls.Add(this.btnCertBussPermit);
            this.groupBox1.Controls.Add(this.btnWithBuss);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 287);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // btnCertStatus
            // 
            this.btnCertStatus.Image = ((System.Drawing.Image)(resources.GetObject("btnCertStatus.Image")));
            this.btnCertStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCertStatus.Location = new System.Drawing.Point(6, 234);
            this.btnCertStatus.Name = "btnCertStatus";
            this.btnCertStatus.Size = new System.Drawing.Size(223, 38);
            this.btnCertStatus.TabIndex = 15;
            this.btnCertStatus.Text = "Status";
            this.btnCertStatus.UseVisualStyleBackColor = true;
            this.btnCertStatus.Click += new System.EventHandler(this.btnCertStatus_Click);
            // 
            // btnCertBussPermit
            // 
            this.btnCertBussPermit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCertBussPermit.Location = new System.Drawing.Point(6, 190);
            this.btnCertBussPermit.Name = "btnCertBussPermit";
            this.btnCertBussPermit.Size = new System.Drawing.Size(223, 38);
            this.btnCertBussPermit.TabIndex = 14;
            this.btnCertBussPermit.Text = "Cert. of Buss Permit";
            this.btnCertBussPermit.UseVisualStyleBackColor = true;
            this.btnCertBussPermit.Click += new System.EventHandler(this.btnCertBussPermit_Click);
            // 
            // btnWithBuss
            // 
            this.btnWithBuss.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnWithBuss.Location = new System.Drawing.Point(6, 146);
            this.btnWithBuss.Name = "btnWithBuss";
            this.btnWithBuss.Size = new System.Drawing.Size(223, 38);
            this.btnWithBuss.TabIndex = 13;
            this.btnWithBuss.Text = "With Business";
            this.btnWithBuss.UseVisualStyleBackColor = true;
            this.btnWithBuss.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnNoBusiness
            // 
            this.btnNoBusiness.Image = ((System.Drawing.Image)(resources.GetObject("btnNoBusiness.Image")));
            this.btnNoBusiness.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNoBusiness.Location = new System.Drawing.Point(14, 110);
            this.btnNoBusiness.Name = "btnNoBusiness";
            this.btnNoBusiness.Size = new System.Drawing.Size(223, 38);
            this.btnNoBusiness.TabIndex = 3;
            this.btnNoBusiness.Text = "No Business";
            this.btnNoBusiness.UseVisualStyleBackColor = true;
            this.btnNoBusiness.Click += new System.EventHandler(this.btnNoBusiness_Click);
            // 
            // btnApplication
            // 
            this.btnApplication.Image = ((System.Drawing.Image)(resources.GetObject("btnApplication.Image")));
            this.btnApplication.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApplication.Location = new System.Drawing.Point(14, 66);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(223, 38);
            this.btnApplication.TabIndex = 2;
            this.btnApplication.Text = "With Application";
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApplication_Click);
            // 
            // btnRetirement
            // 
            this.btnRetirement.Image = ((System.Drawing.Image)(resources.GetObject("btnRetirement.Image")));
            this.btnRetirement.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRetirement.Location = new System.Drawing.Point(14, 22);
            this.btnRetirement.Name = "btnRetirement";
            this.btnRetirement.Size = new System.Drawing.Size(223, 38);
            this.btnRetirement.TabIndex = 1;
            this.btnRetirement.Text = "Retirement";
            this.btnRetirement.UseVisualStyleBackColor = true;
            this.btnRetirement.Click += new System.EventHandler(this.btnRetirement_Click);
            // 
            // frmCertifications
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 329);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNoBusiness);
            this.Controls.Add(this.btnApplication);
            this.Controls.Add(this.btnRetirement);
            this.Controls.Add(this.btnBusOwnership);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCertifications";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Certifications";
            this.Load += new System.EventHandler(this.frmCertifications_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBusOwnership;
        private System.Windows.Forms.Button btnRetirement;
        private System.Windows.Forms.Button btnApplication;
        private System.Windows.Forms.Button btnNoBusiness;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnWithBuss;
        private System.Windows.Forms.Button btnCertBussPermit;
        private System.Windows.Forms.Button btnCertStatus;
    }
}