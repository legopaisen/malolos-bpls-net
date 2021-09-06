namespace Amellar.Modules.Delinquency
{
    partial class frmListDelinquency
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmListDelinquency));
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTeller = new System.Windows.Forms.Label();
            this.rdoTopDelinq = new System.Windows.Forms.RadioButton();
            this.rdoGross = new System.Windows.Forms.RadioButton();
            this.rdoBusStat = new System.Windows.Forms.RadioButton();
            this.rdoMainBus = new System.Windows.Forms.RadioButton();
            this.rdoOwnKind = new System.Windows.Forms.RadioButton();
            this.rdoDist = new System.Windows.Forms.RadioButton();
            this.rdoBrgy = new System.Windows.Forms.RadioButton();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.cmbBusStat = new System.Windows.Forms.ComboBox();
            this.cmbOrgnKind = new System.Windows.Forms.ComboBox();
            this.cmbLineBus = new System.Windows.Forms.ComboBox();
            this.cmbBusType = new System.Windows.Forms.ComboBox();
            this.cmbBrgy = new System.Windows.Forms.ComboBox();
            this.cmbDist = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtGrossFrom = new System.Windows.Forms.TextBox();
            this.txtGrossTo = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtTop = new System.Windows.Forms.TextBox();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.chkTopDelinq = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(198, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 84;
            this.label5.Text = "Business Status";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(198, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 83;
            this.label4.Text = "Ownership Kind";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(198, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 82;
            this.label3.Text = "Sub Category";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(198, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 81;
            this.label2.Text = "Main Business";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(198, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 80;
            this.label1.Text = "Barangay Name";
            // 
            // lblTeller
            // 
            this.lblTeller.AutoSize = true;
            this.lblTeller.Location = new System.Drawing.Point(198, 50);
            this.lblTeller.Name = "lblTeller";
            this.lblTeller.Size = new System.Drawing.Size(70, 13);
            this.lblTeller.TabIndex = 79;
            this.lblTeller.Text = "District Name";
            // 
            // rdoTopDelinq
            // 
            this.rdoTopDelinq.AutoSize = true;
            this.rdoTopDelinq.Location = new System.Drawing.Point(20, 173);
            this.rdoTopDelinq.Name = "rdoTopDelinq";
            this.rdoTopDelinq.Size = new System.Drawing.Size(98, 17);
            this.rdoTopDelinq.TabIndex = 7;
            this.rdoTopDelinq.Text = "Top Delinquent";
            this.rdoTopDelinq.UseVisualStyleBackColor = true;
            this.rdoTopDelinq.Click += new System.EventHandler(this.rdoTopDelinq_Click);
            // 
            // rdoGross
            // 
            this.rdoGross.AutoSize = true;
            this.rdoGross.Location = new System.Drawing.Point(20, 151);
            this.rdoGross.Name = "rdoGross";
            this.rdoGross.Size = new System.Drawing.Size(97, 17);
            this.rdoGross.TabIndex = 6;
            this.rdoGross.Text = "Gross Receipts";
            this.rdoGross.UseVisualStyleBackColor = true;
            this.rdoGross.Click += new System.EventHandler(this.rdoGross_Click);
            // 
            // rdoBusStat
            // 
            this.rdoBusStat.AutoSize = true;
            this.rdoBusStat.Location = new System.Drawing.Point(20, 107);
            this.rdoBusStat.Name = "rdoBusStat";
            this.rdoBusStat.Size = new System.Drawing.Size(100, 17);
            this.rdoBusStat.TabIndex = 4;
            this.rdoBusStat.Text = "Business Status";
            this.rdoBusStat.UseVisualStyleBackColor = true;
            this.rdoBusStat.Click += new System.EventHandler(this.rdoBusStat_Click);
            // 
            // rdoMainBus
            // 
            this.rdoMainBus.AutoSize = true;
            this.rdoMainBus.Location = new System.Drawing.Point(20, 85);
            this.rdoMainBus.Name = "rdoMainBus";
            this.rdoMainBus.Size = new System.Drawing.Size(93, 17);
            this.rdoMainBus.TabIndex = 3;
            this.rdoMainBus.Text = "Main Business";
            this.rdoMainBus.UseVisualStyleBackColor = true;
            this.rdoMainBus.Click += new System.EventHandler(this.rdoMainBus_Click);
            // 
            // rdoOwnKind
            // 
            this.rdoOwnKind.AutoSize = true;
            this.rdoOwnKind.Location = new System.Drawing.Point(20, 129);
            this.rdoOwnKind.Name = "rdoOwnKind";
            this.rdoOwnKind.Size = new System.Drawing.Size(99, 17);
            this.rdoOwnKind.TabIndex = 5;
            this.rdoOwnKind.Text = "Ownership Kind";
            this.rdoOwnKind.UseVisualStyleBackColor = true;
            this.rdoOwnKind.Click += new System.EventHandler(this.rdoOwnKind_Click);
            // 
            // rdoDist
            // 
            this.rdoDist.AutoSize = true;
            this.rdoDist.Location = new System.Drawing.Point(20, 63);
            this.rdoDist.Name = "rdoDist";
            this.rdoDist.Size = new System.Drawing.Size(57, 17);
            this.rdoDist.TabIndex = 2;
            this.rdoDist.Text = "District";
            this.rdoDist.UseVisualStyleBackColor = true;
            this.rdoDist.Click += new System.EventHandler(this.rdoDist_Click);
            // 
            // rdoBrgy
            // 
            this.rdoBrgy.AutoSize = true;
            this.rdoBrgy.Checked = true;
            this.rdoBrgy.Location = new System.Drawing.Point(20, 41);
            this.rdoBrgy.Name = "rdoBrgy";
            this.rdoBrgy.Size = new System.Drawing.Size(70, 17);
            this.rdoBrgy.TabIndex = 1;
            this.rdoBrgy.TabStop = true;
            this.rdoBrgy.Text = "Barangay";
            this.rdoBrgy.UseVisualStyleBackColor = true;
            this.rdoBrgy.Click += new System.EventHandler(this.rdoBrgy_Click);
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(9, 7);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader1.Size = new System.Drawing.Size(165, 24);
            this.kryptonHeader1.TabIndex = 71;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Group By";
            this.kryptonHeader1.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader1.Values.Image")));
            // 
            // cmbBusStat
            // 
            this.cmbBusStat.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbBusStat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBusStat.Enabled = false;
            this.cmbBusStat.FormattingEnabled = true;
            this.cmbBusStat.Items.AddRange(new object[] {
            "ALL",
            "NEW",
            "REN"});
            this.cmbBusStat.Location = new System.Drawing.Point(287, 151);
            this.cmbBusStat.Name = "cmbBusStat";
            this.cmbBusStat.Size = new System.Drawing.Size(198, 21);
            this.cmbBusStat.TabIndex = 13;
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
            "CORPORATION",
            "COOPERATIVE"});
            this.cmbOrgnKind.Location = new System.Drawing.Point(287, 125);
            this.cmbOrgnKind.Name = "cmbOrgnKind";
            this.cmbOrgnKind.Size = new System.Drawing.Size(198, 21);
            this.cmbOrgnKind.TabIndex = 12;
            // 
            // cmbLineBus
            // 
            this.cmbLineBus.Enabled = false;
            this.cmbLineBus.FormattingEnabled = true;
            this.cmbLineBus.Location = new System.Drawing.Point(287, 99);
            this.cmbLineBus.Name = "cmbLineBus";
            this.cmbLineBus.Size = new System.Drawing.Size(198, 21);
            this.cmbLineBus.TabIndex = 11;
            this.cmbLineBus.SelectedIndexChanged += new System.EventHandler(this.cmbLineBus_SelectedIndexChanged);
            this.cmbLineBus.DropDown += new System.EventHandler(this.cmbLineBus_DropDown);
            // 
            // cmbBusType
            // 
            this.cmbBusType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbBusType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBusType.Enabled = false;
            this.cmbBusType.FormattingEnabled = true;
            this.cmbBusType.Location = new System.Drawing.Point(287, 73);
            this.cmbBusType.Name = "cmbBusType";
            this.cmbBusType.Size = new System.Drawing.Size(198, 21);
            this.cmbBusType.TabIndex = 10;
            this.cmbBusType.SelectedIndexChanged += new System.EventHandler(this.cmbBusType_SelectedIndexChanged);
            // 
            // cmbBrgy
            // 
            this.cmbBrgy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbBrgy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBrgy.Enabled = false;
            this.cmbBrgy.FormattingEnabled = true;
            this.cmbBrgy.Location = new System.Drawing.Point(287, 21);
            this.cmbBrgy.Name = "cmbBrgy";
            this.cmbBrgy.Size = new System.Drawing.Size(198, 21);
            this.cmbBrgy.TabIndex = 8;
            // 
            // cmbDist
            // 
            this.cmbDist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbDist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDist.Enabled = false;
            this.cmbDist.FormattingEnabled = true;
            this.cmbDist.Location = new System.Drawing.Point(287, 47);
            this.cmbDist.Name = "cmbDist";
            this.cmbDist.Size = new System.Drawing.Size(198, 21);
            this.cmbDist.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(9, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(165, 196);
            this.label6.TabIndex = 91;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label9.Location = new System.Drawing.Point(5, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(165, 197);
            this.label9.TabIndex = 92;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(199, 180);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 13);
            this.label10.TabIndex = 84;
            this.label10.Text = "Gross Receipts";
            // 
            // txtGrossFrom
            // 
            this.txtGrossFrom.Enabled = false;
            this.txtGrossFrom.Location = new System.Drawing.Point(287, 177);
            this.txtGrossFrom.Name = "txtGrossFrom";
            this.txtGrossFrom.Size = new System.Drawing.Size(123, 20);
            this.txtGrossFrom.TabIndex = 14;
            this.txtGrossFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtGrossFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTop_KeyPress);
            // 
            // txtGrossTo
            // 
            this.txtGrossTo.Enabled = false;
            this.txtGrossTo.Location = new System.Drawing.Point(287, 202);
            this.txtGrossTo.Name = "txtGrossTo";
            this.txtGrossTo.Size = new System.Drawing.Size(123, 20);
            this.txtGrossTo.TabIndex = 15;
            this.txtGrossTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtGrossTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTop_KeyPress);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(253, 205);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(20, 13);
            this.label11.TabIndex = 84;
            this.label11.Text = "To";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(442, 180);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(26, 13);
            this.label12.TabIndex = 84;
            this.label12.Text = "Top";
            // 
            // txtTop
            // 
            this.txtTop.Enabled = false;
            this.txtTop.Location = new System.Drawing.Point(421, 202);
            this.txtTop.Name = "txtTop";
            this.txtTop.Size = new System.Drawing.Size(64, 20);
            this.txtTop.TabIndex = 16;
            this.txtTop.Text = "0";
            this.txtTop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTop.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTop_KeyPress);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(99, 223);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(62, 25);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(20, 223);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(73, 25);
            this.btnGenerate.TabIndex = 19;
            this.btnGenerate.Values.Text = "&Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(287, 227);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(79, 20);
            this.dtpFrom.TabIndex = 17;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(248, 230);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(30, 13);
            this.label13.TabIndex = 97;
            this.label13.Text = "Date";
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(184, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(317, 252);
            this.label7.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label8.Location = new System.Drawing.Point(180, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(317, 253);
            this.label8.TabIndex = 100;
            // 
            // chkTopDelinq
            // 
            this.chkTopDelinq.AutoSize = true;
            this.chkTopDelinq.Location = new System.Drawing.Point(376, 230);
            this.chkTopDelinq.Name = "chkTopDelinq";
            this.chkTopDelinq.Size = new System.Drawing.Size(116, 17);
            this.chkTopDelinq.TabIndex = 18;
            this.chkTopDelinq.Text = "use previous report";
            this.chkTopDelinq.UseVisualStyleBackColor = true;
            this.chkTopDelinq.Visible = false;
            // 
            // frmListDelinquency
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 268);
            this.Controls.Add(this.chkTopDelinq);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.txtTop);
            this.Controls.Add(this.txtGrossTo);
            this.Controls.Add(this.txtGrossFrom);
            this.Controls.Add(this.cmbBusStat);
            this.Controls.Add(this.cmbOrgnKind);
            this.Controls.Add(this.cmbLineBus);
            this.Controls.Add(this.cmbBusType);
            this.Controls.Add(this.cmbBrgy);
            this.Controls.Add(this.cmbDist);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTeller);
            this.Controls.Add(this.rdoTopDelinq);
            this.Controls.Add(this.rdoGross);
            this.Controls.Add(this.rdoBusStat);
            this.Controls.Add(this.rdoMainBus);
            this.Controls.Add(this.rdoOwnKind);
            this.Controls.Add(this.rdoDist);
            this.Controls.Add(this.rdoBrgy);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmListDelinquency";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "List of Delinquency";
            this.Load += new System.EventHandler(this.frmListDelinquency_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTeller;
        private System.Windows.Forms.RadioButton rdoTopDelinq;
        private System.Windows.Forms.RadioButton rdoGross;
        private System.Windows.Forms.RadioButton rdoBusStat;
        private System.Windows.Forms.RadioButton rdoMainBus;
        private System.Windows.Forms.RadioButton rdoOwnKind;
        private System.Windows.Forms.RadioButton rdoDist;
        private System.Windows.Forms.RadioButton rdoBrgy;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private System.Windows.Forms.ComboBox cmbBusStat;
        private System.Windows.Forms.ComboBox cmbOrgnKind;
        private System.Windows.Forms.ComboBox cmbLineBus;
        private System.Windows.Forms.ComboBox cmbBusType;
        private System.Windows.Forms.ComboBox cmbBrgy;
        private System.Windows.Forms.ComboBox cmbDist;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtGrossFrom;
        private System.Windows.Forms.TextBox txtGrossTo;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtTop;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkTopDelinq;
    }
}