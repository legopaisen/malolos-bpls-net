namespace Amellar.Common.frmBns_Rec
{
    partial class frmApplicationForm
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
            this.btnSave = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.bin1 = new Amellar.Common.BIN.BIN();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.txtBnsType = new System.Windows.Forms.TextBox();
            this.txtBnsAdd = new System.Windows.Forms.TextBox();
            this.txtOwnName = new System.Windows.Forms.TextBox();
            this.chkOutsideLGU = new System.Windows.Forms.CheckBox();
            this.txtMainID = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(324, 168);
            this.btnSave.Name = "btnSave";
            this.btnSave.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSave.Size = new System.Drawing.Size(82, 24);
            this.btnSave.TabIndex = 28;
            this.btnSave.Values.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(412, 168);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(82, 24);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Values.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(8, 12);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(498, 150);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(120, 26);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(141, 25);
            this.bin1.TabIndex = 29;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(265, 22);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(71, 24);
            this.btnSearch.TabIndex = 28;
            this.btnSearch.Values.Text = "Search Bin";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "BIN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Business Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Business Type";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Business Address";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Taxpayer\'s Name";
            // 
            // txtBnsName
            // 
            this.txtBnsName.Location = new System.Drawing.Point(120, 51);
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(374, 20);
            this.txtBnsName.TabIndex = 31;
            // 
            // txtBnsType
            // 
            this.txtBnsType.Location = new System.Drawing.Point(120, 75);
            this.txtBnsType.Name = "txtBnsType";
            this.txtBnsType.ReadOnly = true;
            this.txtBnsType.Size = new System.Drawing.Size(374, 20);
            this.txtBnsType.TabIndex = 31;
            // 
            // txtBnsAdd
            // 
            this.txtBnsAdd.Location = new System.Drawing.Point(120, 99);
            this.txtBnsAdd.Name = "txtBnsAdd";
            this.txtBnsAdd.ReadOnly = true;
            this.txtBnsAdd.Size = new System.Drawing.Size(374, 20);
            this.txtBnsAdd.TabIndex = 31;
            // 
            // txtOwnName
            // 
            this.txtOwnName.Location = new System.Drawing.Point(120, 123);
            this.txtOwnName.Name = "txtOwnName";
            this.txtOwnName.ReadOnly = true;
            this.txtOwnName.Size = new System.Drawing.Size(374, 20);
            this.txtOwnName.TabIndex = 31;
            // 
            // chkOutsideLGU
            // 
            this.chkOutsideLGU.AutoSize = true;
            this.chkOutsideLGU.Location = new System.Drawing.Point(344, 26);
            this.chkOutsideLGU.Name = "chkOutsideLGU";
            this.chkOutsideLGU.Size = new System.Drawing.Size(87, 17);
            this.chkOutsideLGU.TabIndex = 32;
            this.chkOutsideLGU.Text = "Outside LGU";
            this.chkOutsideLGU.UseVisualStyleBackColor = true;
            this.chkOutsideLGU.CheckStateChanged += new System.EventHandler(this.chkOutsideLGU_CheckStateChanged);
            // 
            // txtMainID
            // 
            this.txtMainID.Location = new System.Drawing.Point(120, 26);
            this.txtMainID.Name = "txtMainID";
            this.txtMainID.Size = new System.Drawing.Size(125, 20);
            this.txtMainID.TabIndex = 33;
            this.txtMainID.Visible = false;
            // 
            // frmApplicationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 201);
            this.Controls.Add(this.bin1);
            this.Controls.Add(this.txtMainID);
            this.Controls.Add(this.chkOutsideLGU);
            this.Controls.Add(this.txtOwnName);
            this.Controls.Add(this.txtBnsAdd);
            this.Controls.Add(this.txtBnsType);
            this.Controls.Add(this.txtBnsName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.containerWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmApplicationForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Main Branch Information";
            this.Load += new System.EventHandler(this.frmMainBranch_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnSave;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBnsName;
        private System.Windows.Forms.TextBox txtBnsType;
        private System.Windows.Forms.TextBox txtBnsAdd;
        private System.Windows.Forms.TextBox txtOwnName;
        private System.Windows.Forms.CheckBox chkOutsideLGU;
        private System.Windows.Forms.TextBox txtMainID;
        public Amellar.Common.BIN.BIN bin1;
    }
}