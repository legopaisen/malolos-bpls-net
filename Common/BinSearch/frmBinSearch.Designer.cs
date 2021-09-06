namespace Amellar.Common.BinSearch
{
    partial class frmBinSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBinSearch));
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnBnsInfo = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnViewDocu = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.bin1 = new Amellar.Common.BIN.BIN();
            this.btn_viewPermit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.SuspendLayout();
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(0, -1);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader1.Size = new System.Drawing.Size(289, 25);
            this.kryptonHeader1.TabIndex = 1;
            this.kryptonHeader1.Text = "Search BIN";
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Search BIN";
            this.kryptonHeader1.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader1.Values.Image")));
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter BIN:";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(16, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(261, 41);
            this.label2.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label3.Location = new System.Drawing.Point(12, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(261, 41);
            this.label3.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(16, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(261, 90);
            this.label4.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(12, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(261, 90);
            this.label5.TabIndex = 8;
            // 
            // btnBnsInfo
            // 
            this.btnBnsInfo.Location = new System.Drawing.Point(26, 85);
            this.btnBnsInfo.Name = "btnBnsInfo";
            this.btnBnsInfo.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleOrange;
            this.btnBnsInfo.Size = new System.Drawing.Size(97, 24);
            this.btnBnsInfo.TabIndex = 9;
            this.btnBnsInfo.Text = "Business Info";
            this.btnBnsInfo.Values.ExtraText = "";
            this.btnBnsInfo.Values.Image = null;
            this.btnBnsInfo.Values.ImageStates.ImageCheckedNormal = null;
            this.btnBnsInfo.Values.ImageStates.ImageCheckedPressed = null;
            this.btnBnsInfo.Values.ImageStates.ImageCheckedTracking = null;
            this.btnBnsInfo.Values.Text = "Business Info";
            this.btnBnsInfo.Click += new System.EventHandler(this.btnBnsInfo_Click);
            // 
            // btnViewDocu
            // 
            this.btnViewDocu.Location = new System.Drawing.Point(26, 111);
            this.btnViewDocu.Name = "btnViewDocu";
            this.btnViewDocu.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleOrange;
            this.btnViewDocu.Size = new System.Drawing.Size(97, 25);
            this.btnViewDocu.TabIndex = 10;
            this.btnViewDocu.Text = "View Documents";
            this.btnViewDocu.Values.ExtraText = "";
            this.btnViewDocu.Values.Image = null;
            this.btnViewDocu.Values.ImageStates.ImageCheckedNormal = null;
            this.btnViewDocu.Values.ImageStates.ImageCheckedPressed = null;
            this.btnViewDocu.Values.ImageStates.ImageCheckedTracking = null;
            this.btnViewDocu.Values.Text = "View Documents";
            this.btnViewDocu.Click += new System.EventHandler(this.btnViewDocu_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(173, 84);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleOrange;
            this.btnSearch.Size = new System.Drawing.Size(97, 25);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "Search";
            this.btnSearch.Values.ExtraText = "";
            this.btnSearch.Values.Image = null;
            this.btnSearch.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearch.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearch.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(173, 111);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleOrange;
            this.btnCancel.Size = new System.Drawing.Size(97, 25);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(132, 40);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(138, 20);
            this.bin1.TabIndex = 0;
            // 
            // btn_viewPermit
            // 
            this.btn_viewPermit.Location = new System.Drawing.Point(64, 140);
            this.btn_viewPermit.Name = "btn_viewPermit";
            this.btn_viewPermit.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleOrange;
            this.btn_viewPermit.Size = new System.Drawing.Size(165, 25);
            this.btn_viewPermit.TabIndex = 13;
            this.btn_viewPermit.Text = "BUSINESS PERMIT";
            this.btn_viewPermit.Values.ExtraText = "";
            this.btn_viewPermit.Values.Image = null;
            this.btn_viewPermit.Values.ImageStates.ImageCheckedNormal = null;
            this.btn_viewPermit.Values.ImageStates.ImageCheckedPressed = null;
            this.btn_viewPermit.Values.ImageStates.ImageCheckedTracking = null;
            this.btn_viewPermit.Values.Text = "BUSINESS PERMIT";
            this.btn_viewPermit.Click += new System.EventHandler(this.btn_viewPermit_Click);
            // 
            // frmBinSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(281, 184);
            this.ControlBox = false;
            this.Controls.Add(this.btn_viewPermit);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnViewDocu);
            this.Controls.Add(this.btnBnsInfo);
            this.Controls.Add(this.bin1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.MaximumSize = new System.Drawing.Size(297, 200);
            this.MinimumSize = new System.Drawing.Size(297, 200);
            this.Name = "frmBinSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmBinSearch_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private System.Windows.Forms.Label label1;
        public Amellar.Common.BIN.BIN bin1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnBnsInfo;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnViewDocu;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btn_viewPermit;

    }
}

