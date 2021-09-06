namespace Amellar.Modules.Delinquency
{
    partial class frmUnrenewedBusiness
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUnrenewedBusiness));
            this.rdoMainBus = new System.Windows.Forms.RadioButton();
            this.rdoDist = new System.Windows.Forms.RadioButton();
            this.rdoBrgy = new System.Windows.Forms.RadioButton();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbBusType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBrgy = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDist = new System.Windows.Forms.ComboBox();
            this.lblTeller = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.GB1 = new System.Windows.Forms.GroupBox();
            this.rdoBnsName = new System.Windows.Forms.RadioButton();
            this.rdoOwnName = new System.Windows.Forms.RadioButton();
            this.GB1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdoMainBus
            // 
            this.rdoMainBus.AutoSize = true;
            this.rdoMainBus.Checked = true;
            this.rdoMainBus.Location = new System.Drawing.Point(21, 45);
            this.rdoMainBus.Name = "rdoMainBus";
            this.rdoMainBus.Size = new System.Drawing.Size(93, 17);
            this.rdoMainBus.TabIndex = 34;
            this.rdoMainBus.TabStop = true;
            this.rdoMainBus.Text = "Main Business";
            this.rdoMainBus.UseVisualStyleBackColor = true;
            this.rdoMainBus.Click += new System.EventHandler(this.rdoMainBus_Click);
            // 
            // rdoDist
            // 
            this.rdoDist.AutoSize = true;
            this.rdoDist.Location = new System.Drawing.Point(21, 90);
            this.rdoDist.Name = "rdoDist";
            this.rdoDist.Size = new System.Drawing.Size(57, 17);
            this.rdoDist.TabIndex = 30;
            this.rdoDist.Text = "District";
            this.rdoDist.UseVisualStyleBackColor = true;
            this.rdoDist.Click += new System.EventHandler(this.rdoDist_Click);
            // 
            // rdoBrgy
            // 
            this.rdoBrgy.AutoSize = true;
            this.rdoBrgy.Location = new System.Drawing.Point(21, 68);
            this.rdoBrgy.Name = "rdoBrgy";
            this.rdoBrgy.Size = new System.Drawing.Size(70, 17);
            this.rdoBrgy.TabIndex = 29;
            this.rdoBrgy.Text = "Barangay";
            this.rdoBrgy.UseVisualStyleBackColor = true;
            this.rdoBrgy.Click += new System.EventHandler(this.rdoBrgy_Click);
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(10, 10);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader1.Size = new System.Drawing.Size(135, 24);
            this.kryptonHeader1.TabIndex = 27;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Group by";
            this.kryptonHeader1.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader1.Values.Image")));
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 112);
            this.label1.TabIndex = 47;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label9.Location = new System.Drawing.Point(6, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(135, 113);
            this.label9.TabIndex = 48;
            // 
            // cmbBusType
            // 
            this.cmbBusType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbBusType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBusType.Enabled = false;
            this.cmbBusType.FormattingEnabled = true;
            this.cmbBusType.Location = new System.Drawing.Point(255, 18);
            this.cmbBusType.Name = "cmbBusType";
            this.cmbBusType.Size = new System.Drawing.Size(220, 21);
            this.cmbBusType.TabIndex = 69;
            this.cmbBusType.SelectedIndexChanged += new System.EventHandler(this.cmbBusType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(164, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 70;
            this.label2.Text = "Main Business";
            // 
            // cmbBrgy
            // 
            this.cmbBrgy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbBrgy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBrgy.Enabled = false;
            this.cmbBrgy.FormattingEnabled = true;
            this.cmbBrgy.Location = new System.Drawing.Point(255, 45);
            this.cmbBrgy.Name = "cmbBrgy";
            this.cmbBrgy.Size = new System.Drawing.Size(220, 21);
            this.cmbBrgy.TabIndex = 67;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(164, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 68;
            this.label3.Text = "Barangay Name";
            // 
            // cmbDist
            // 
            this.cmbDist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbDist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDist.Enabled = false;
            this.cmbDist.FormattingEnabled = true;
            this.cmbDist.Location = new System.Drawing.Point(255, 72);
            this.cmbDist.Name = "cmbDist";
            this.cmbDist.Size = new System.Drawing.Size(220, 21);
            this.cmbDist.TabIndex = 65;
            // 
            // lblTeller
            // 
            this.lblTeller.AutoSize = true;
            this.lblTeller.Location = new System.Drawing.Point(164, 77);
            this.lblTeller.Name = "lblTeller";
            this.lblTeller.Size = new System.Drawing.Size(70, 13);
            this.lblTeller.TabIndex = 66;
            this.lblTeller.Text = "District Name";
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(153, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(338, 152);
            this.label4.TabIndex = 71;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label5.Location = new System.Drawing.Point(149, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(338, 153);
            this.label5.TabIndex = 72;
            // 
            // btnCancel
            // 
            this.btnCancel.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnCancel.Location = new System.Drawing.Point(413, 121);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(62, 25);
            this.btnCancel.TabIndex = 79;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnGenerate.Location = new System.Drawing.Point(334, 121);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(73, 25);
            this.btnGenerate.TabIndex = 78;
            this.btnGenerate.Values.Text = "&Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // GB1
            // 
            this.GB1.Controls.Add(this.rdoBnsName);
            this.GB1.Controls.Add(this.rdoOwnName);
            this.GB1.Location = new System.Drawing.Point(162, 97);
            this.GB1.Name = "GB1";
            this.GB1.Size = new System.Drawing.Size(119, 60);
            this.GB1.TabIndex = 80;
            this.GB1.TabStop = false;
            this.GB1.Text = "Group By";
            // 
            // rdoBnsName
            // 
            this.rdoBnsName.AutoSize = true;
            this.rdoBnsName.Checked = true;
            this.rdoBnsName.Location = new System.Drawing.Point(12, 17);
            this.rdoBnsName.Name = "rdoBnsName";
            this.rdoBnsName.Size = new System.Drawing.Size(98, 17);
            this.rdoBnsName.TabIndex = 29;
            this.rdoBnsName.TabStop = true;
            this.rdoBnsName.Text = "Business Name";
            this.rdoBnsName.UseVisualStyleBackColor = true;
            this.rdoBnsName.Click += new System.EventHandler(this.rdoBnsName_Click);
            // 
            // rdoOwnName
            // 
            this.rdoOwnName.AutoSize = true;
            this.rdoOwnName.Location = new System.Drawing.Point(12, 35);
            this.rdoOwnName.Name = "rdoOwnName";
            this.rdoOwnName.Size = new System.Drawing.Size(94, 17);
            this.rdoOwnName.TabIndex = 30;
            this.rdoOwnName.Text = "Owner\'s Name";
            this.rdoOwnName.UseVisualStyleBackColor = true;
            this.rdoOwnName.Click += new System.EventHandler(this.rdoOwnName_Click);
            // 
            // frmUnrenewedBusiness
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 173);
            this.Controls.Add(this.GB1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.cmbBusType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbBrgy);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbDist);
            this.Controls.Add(this.lblTeller);
            this.Controls.Add(this.rdoMainBus);
            this.Controls.Add(this.rdoDist);
            this.Controls.Add(this.rdoBrgy);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUnrenewedBusiness";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "List of Unrenewed Businesses";
            this.Load += new System.EventHandler(this.frmUnrenewedBusiness_Load);
            this.GB1.ResumeLayout(false);
            this.GB1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoMainBus;
        private System.Windows.Forms.RadioButton rdoDist;
        private System.Windows.Forms.RadioButton rdoBrgy;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbBusType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBrgy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbDist;
        private System.Windows.Forms.Label lblTeller;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private System.Windows.Forms.GroupBox GB1;
        private System.Windows.Forms.RadioButton rdoBnsName;
        private System.Windows.Forms.RadioButton rdoOwnName;
    }
}

