namespace Amellar.Modules.LiquidationReports
{
    partial class frmAbstractCollectOLD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbstractCollectOLD));
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.rdoDaily = new System.Windows.Forms.RadioButton();
            this.rdoOR = new System.Windows.Forms.RadioButton();
            this.rdoTeller = new System.Windows.Forms.RadioButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbTeller = new System.Windows.Forms.ComboBox();
            this.lblTeller = new System.Windows.Forms.Label();
            this.rdoDummy = new System.Windows.Forms.RadioButton();
            this.lblRCDSeries = new System.Windows.Forms.Label();
            this.txtORFrom = new System.Windows.Forms.TextBox();
            this.txtORto = new System.Windows.Forms.TextBox();
            this.frameWithShadow1 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow2 = new Amellar.Common.SOA.FrameWithShadow();
            this.cmbRCDSeries = new System.Windows.Forms.ComboBox();
            this.lblTitle = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.SuspendLayout();
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(11, 12);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader1.Size = new System.Drawing.Size(148, 24);
            this.kryptonHeader1.TabIndex = 44;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Group By";
            this.kryptonHeader1.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader1.Values.Image")));
            // 
            // rdoDaily
            // 
            this.rdoDaily.AutoSize = true;
            this.rdoDaily.Location = new System.Drawing.Point(27, 58);
            this.rdoDaily.Name = "rdoDaily";
            this.rdoDaily.Size = new System.Drawing.Size(100, 17);
            this.rdoDaily.TabIndex = 2;
            this.rdoDaily.TabStop = true;
            this.rdoDaily.Text = "Daily Aggregate";
            this.rdoDaily.UseVisualStyleBackColor = true;
            this.rdoDaily.Click += new System.EventHandler(this.rdoDaily_Click);
            // 
            // rdoOR
            // 
            this.rdoOR.AutoSize = true;
            this.rdoOR.Location = new System.Drawing.Point(27, 81);
            this.rdoOR.Name = "rdoOR";
            this.rdoOR.Size = new System.Drawing.Size(102, 17);
            this.rdoOR.TabIndex = 3;
            this.rdoOR.TabStop = true;
            this.rdoOR.Text = "Official Receipts";
            this.rdoOR.UseVisualStyleBackColor = true;
            this.rdoOR.Click += new System.EventHandler(this.rdoOR_Click);
            // 
            // rdoTeller
            // 
            this.rdoTeller.AutoSize = true;
            this.rdoTeller.Location = new System.Drawing.Point(27, 104);
            this.rdoTeller.Name = "rdoTeller";
            this.rdoTeller.Size = new System.Drawing.Size(51, 17);
            this.rdoTeller.TabIndex = 4;
            this.rdoTeller.TabStop = true;
            this.rdoTeller.Text = "Teller";
            this.rdoTeller.UseVisualStyleBackColor = true;
            this.rdoTeller.Click += new System.EventHandler(this.rdoTeller_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(334, 151);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(62, 25);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(255, 151);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(73, 25);
            this.btnGenerate.TabIndex = 11;
            this.btnGenerate.Values.Text = "&Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(320, 63);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(79, 20);
            this.dtpTo.TabIndex = 6;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(213, 63);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(79, 20);
            this.dtpFrom.TabIndex = 5;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(295, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 54;
            this.label4.Text = "To";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(180, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 53;
            this.label3.Text = "From";
            // 
            // cmbTeller
            // 
            this.cmbTeller.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbTeller.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbTeller.FormattingEnabled = true;
            this.cmbTeller.Location = new System.Drawing.Point(227, 100);
            this.cmbTeller.Name = "cmbTeller";
            this.cmbTeller.Size = new System.Drawing.Size(169, 21);
            this.cmbTeller.TabIndex = 9;
            this.cmbTeller.Visible = false;
            this.cmbTeller.Leave += new System.EventHandler(this.cmbTeller_Leave);
            // 
            // lblTeller
            // 
            this.lblTeller.AutoSize = true;
            this.lblTeller.Location = new System.Drawing.Point(180, 102);
            this.lblTeller.Name = "lblTeller";
            this.lblTeller.Size = new System.Drawing.Size(33, 13);
            this.lblTeller.TabIndex = 53;
            this.lblTeller.Text = "Teller";
            this.lblTeller.Visible = false;
            // 
            // rdoDummy
            // 
            this.rdoDummy.AutoSize = true;
            this.rdoDummy.Checked = true;
            this.rdoDummy.Location = new System.Drawing.Point(17, 41);
            this.rdoDummy.Name = "rdoDummy";
            this.rdoDummy.Size = new System.Drawing.Size(14, 13);
            this.rdoDummy.TabIndex = 1;
            this.rdoDummy.TabStop = true;
            this.rdoDummy.UseVisualStyleBackColor = true;
            this.rdoDummy.Visible = false;
            // 
            // lblRCDSeries
            // 
            this.lblRCDSeries.AutoSize = true;
            this.lblRCDSeries.Location = new System.Drawing.Point(10, 157);
            this.lblRCDSeries.Name = "lblRCDSeries";
            this.lblRCDSeries.Size = new System.Drawing.Size(62, 13);
            this.lblRCDSeries.TabIndex = 53;
            this.lblRCDSeries.Text = "RCD Series";
            this.lblRCDSeries.Visible = false;
            // 
            // txtORFrom
            // 
            this.txtORFrom.Location = new System.Drawing.Point(213, 63);
            this.txtORFrom.Name = "txtORFrom";
            this.txtORFrom.Size = new System.Drawing.Size(79, 20);
            this.txtORFrom.TabIndex = 7;
            this.txtORFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtORFrom_KeyPress);
            // 
            // txtORto
            // 
            this.txtORto.Location = new System.Drawing.Point(320, 63);
            this.txtORto.Name = "txtORto";
            this.txtORto.Size = new System.Drawing.Size(79, 20);
            this.txtORto.TabIndex = 8;
            this.txtORto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtORFrom_KeyPress);
            // 
            // frameWithShadow1
            // 
            this.frameWithShadow1.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow1.Location = new System.Drawing.Point(6, 12);
            this.frameWithShadow1.Name = "frameWithShadow1";
            this.frameWithShadow1.Size = new System.Drawing.Size(155, 133);
            this.frameWithShadow1.TabIndex = 43;
            // 
            // frameWithShadow2
            // 
            this.frameWithShadow2.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow2.Location = new System.Drawing.Point(164, 14);
            this.frameWithShadow2.Name = "frameWithShadow2";
            this.frameWithShadow2.Size = new System.Drawing.Size(244, 131);
            this.frameWithShadow2.TabIndex = 57;
            // 
            // cmbRCDSeries
            // 
            this.cmbRCDSeries.FormattingEnabled = true;
            this.cmbRCDSeries.Location = new System.Drawing.Point(78, 152);
            this.cmbRCDSeries.MaxDropDownItems = 5;
            this.cmbRCDSeries.MaxLength = 13;
            this.cmbRCDSeries.Name = "cmbRCDSeries";
            this.cmbRCDSeries.Size = new System.Drawing.Size(151, 21);
            this.cmbRCDSeries.TabIndex = 10;
            this.cmbRCDSeries.Visible = false;
            this.cmbRCDSeries.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbRCDSeries_KeyPress);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = false;
            this.lblTitle.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.lblTitle.Location = new System.Drawing.Point(169, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.lblTitle.Size = new System.Drawing.Size(237, 24);
            this.lblTitle.TabIndex = 58;
            this.lblTitle.Values.Description = "";
            this.lblTitle.Values.Heading = "Covered Period";
            this.lblTitle.Values.Image = ((System.Drawing.Image)(resources.GetObject("lblTitle.Values.Image")));
            // 
            // frmAbstractCollect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 181);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.cmbRCDSeries);
            this.Controls.Add(this.cmbTeller);
            this.Controls.Add(this.lblRCDSeries);
            this.Controls.Add(this.lblTeller);
            this.Controls.Add(this.rdoDummy);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.rdoTeller);
            this.Controls.Add(this.rdoOR);
            this.Controls.Add(this.rdoDaily);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.frameWithShadow1);
            this.Controls.Add(this.frameWithShadow2);
            this.Controls.Add(this.txtORto);
            this.Controls.Add(this.txtORFrom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAbstractCollect";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frmAbstractCollect_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow1;
        private System.Windows.Forms.RadioButton rdoDaily;
        private System.Windows.Forms.RadioButton rdoOR;
        private System.Windows.Forms.RadioButton rdoTeller;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow2;
        private System.Windows.Forms.ComboBox cmbTeller;
        private System.Windows.Forms.Label lblTeller;
        private System.Windows.Forms.RadioButton rdoDummy;
        private System.Windows.Forms.Label lblRCDSeries;
        private System.Windows.Forms.TextBox txtORFrom;
        private System.Windows.Forms.TextBox txtORto;
        private System.Windows.Forms.ComboBox cmbRCDSeries;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader lblTitle;
    }
}