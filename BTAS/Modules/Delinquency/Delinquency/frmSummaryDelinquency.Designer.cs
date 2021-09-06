namespace Amellar.Modules.Delinquency
{
    partial class frmSummaryDelinquency
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSummaryDelinquency));
            this.rdoMainBus = new System.Windows.Forms.RadioButton();
            this.rdoDist = new System.Windows.Forms.RadioButton();
            this.rdoBrgy = new System.Windows.Forms.RadioButton();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.rdoOwnKnd = new System.Windows.Forms.RadioButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.cmbBusType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBrgy = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDist = new System.Windows.Forms.ComboBox();
            this.lblTeller = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbOrgnKind = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // rdoMainBus
            // 
            this.rdoMainBus.AutoSize = true;
            this.rdoMainBus.Location = new System.Drawing.Point(22, 85);
            this.rdoMainBus.Name = "rdoMainBus";
            this.rdoMainBus.Size = new System.Drawing.Size(93, 17);
            this.rdoMainBus.TabIndex = 52;
            this.rdoMainBus.Text = "Main Business";
            this.rdoMainBus.UseVisualStyleBackColor = true;
            this.rdoMainBus.Click += new System.EventHandler(this.rdoMainBus_Click);
            // 
            // rdoDist
            // 
            this.rdoDist.AutoSize = true;
            this.rdoDist.Location = new System.Drawing.Point(22, 62);
            this.rdoDist.Name = "rdoDist";
            this.rdoDist.Size = new System.Drawing.Size(57, 17);
            this.rdoDist.TabIndex = 51;
            this.rdoDist.Text = "District";
            this.rdoDist.UseVisualStyleBackColor = true;
            this.rdoDist.Click += new System.EventHandler(this.rdoDist_Click);
            // 
            // rdoBrgy
            // 
            this.rdoBrgy.AutoSize = true;
            this.rdoBrgy.Checked = true;
            this.rdoBrgy.Location = new System.Drawing.Point(22, 39);
            this.rdoBrgy.Name = "rdoBrgy";
            this.rdoBrgy.Size = new System.Drawing.Size(70, 17);
            this.rdoBrgy.TabIndex = 50;
            this.rdoBrgy.TabStop = true;
            this.rdoBrgy.Text = "Barangay";
            this.rdoBrgy.UseVisualStyleBackColor = true;
            this.rdoBrgy.Click += new System.EventHandler(this.rdoBrgy_Click);
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(11, 7);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader1.Size = new System.Drawing.Size(135, 24);
            this.kryptonHeader1.TabIndex = 49;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Group by";
            this.kryptonHeader1.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader1.Values.Image")));
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(11, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 130);
            this.label1.TabIndex = 53;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label9.Location = new System.Drawing.Point(7, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(135, 131);
            this.label9.TabIndex = 54;
            // 
            // rdoOwnKnd
            // 
            this.rdoOwnKnd.AutoSize = true;
            this.rdoOwnKnd.Location = new System.Drawing.Point(22, 108);
            this.rdoOwnKnd.Name = "rdoOwnKnd";
            this.rdoOwnKnd.Size = new System.Drawing.Size(99, 17);
            this.rdoOwnKnd.TabIndex = 52;
            this.rdoOwnKnd.Text = "Ownership Kind";
            this.rdoOwnKnd.UseVisualStyleBackColor = true;
            this.rdoOwnKnd.Click += new System.EventHandler(this.rdoOwnKnd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(420, 133);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(62, 25);
            this.btnCancel.TabIndex = 89;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(341, 133);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(73, 25);
            this.btnGenerate.TabIndex = 88;
            this.btnGenerate.Values.Text = "&Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // cmbBusType
            // 
            this.cmbBusType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbBusType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBusType.Enabled = false;
            this.cmbBusType.FormattingEnabled = true;
            this.cmbBusType.Location = new System.Drawing.Point(256, 72);
            this.cmbBusType.Name = "cmbBusType";
            this.cmbBusType.Size = new System.Drawing.Size(220, 21);
            this.cmbBusType.TabIndex = 84;
            this.cmbBusType.SelectedIndexChanged += new System.EventHandler(this.cmbBusType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(167, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 85;
            this.label2.Text = "Main Business";
            // 
            // cmbBrgy
            // 
            this.cmbBrgy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbBrgy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBrgy.Enabled = false;
            this.cmbBrgy.FormattingEnabled = true;
            this.cmbBrgy.Location = new System.Drawing.Point(256, 20);
            this.cmbBrgy.Name = "cmbBrgy";
            this.cmbBrgy.Size = new System.Drawing.Size(220, 21);
            this.cmbBrgy.TabIndex = 82;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(167, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 83;
            this.label3.Text = "Barangay Name";
            // 
            // cmbDist
            // 
            this.cmbDist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbDist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDist.Enabled = false;
            this.cmbDist.FormattingEnabled = true;
            this.cmbDist.Location = new System.Drawing.Point(256, 46);
            this.cmbDist.Name = "cmbDist";
            this.cmbDist.Size = new System.Drawing.Size(220, 21);
            this.cmbDist.TabIndex = 80;
            // 
            // lblTeller
            // 
            this.lblTeller.AutoSize = true;
            this.lblTeller.Location = new System.Drawing.Point(167, 49);
            this.lblTeller.Name = "lblTeller";
            this.lblTeller.Size = new System.Drawing.Size(70, 13);
            this.lblTeller.TabIndex = 81;
            this.lblTeller.Text = "District Name";
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(154, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(338, 158);
            this.label4.TabIndex = 86;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label5.Location = new System.Drawing.Point(150, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(338, 159);
            this.label5.TabIndex = 87;
            // 
            // cmbOrgnKind
            // 
            this.cmbOrgnKind.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbOrgnKind.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbOrgnKind.Enabled = false;
            this.cmbOrgnKind.FormattingEnabled = true;
            this.cmbOrgnKind.Items.AddRange(new object[] {
            "ALL",
            "SINGLE PROPRIETORSHIP",
            "PARTNERSHIP",
            "CORPORATION"});
            this.cmbOrgnKind.Location = new System.Drawing.Point(256, 98);
            this.cmbOrgnKind.Name = "cmbOrgnKind";
            this.cmbOrgnKind.Size = new System.Drawing.Size(220, 21);
            this.cmbOrgnKind.TabIndex = 90;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(167, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 91;
            this.label6.Text = "Ownership Kind";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(167, 128);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 91;
            this.label7.Text = "Date";
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(256, 124);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(79, 20);
            this.dtpFrom.TabIndex = 92;
            // 
            // frmSummaryDelinquency
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(501, 176);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.cmbOrgnKind);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.cmbBusType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbBrgy);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbDist);
            this.Controls.Add(this.lblTeller);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.rdoOwnKnd);
            this.Controls.Add(this.rdoMainBus);
            this.Controls.Add(this.rdoDist);
            this.Controls.Add(this.rdoBrgy);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label9);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSummaryDelinquency";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Summary of Delinquency";
            this.Load += new System.EventHandler(this.frmSummaryDelinquency_Load);
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
        private System.Windows.Forms.RadioButton rdoOwnKnd;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private System.Windows.Forms.ComboBox cmbBusType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBrgy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbDist;
        private System.Windows.Forms.Label lblTeller;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbOrgnKind;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpFrom;
    }
}