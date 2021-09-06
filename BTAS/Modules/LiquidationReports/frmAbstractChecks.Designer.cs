namespace Amellar.Modules.LiquidationReports
{
    partial class frmAbstractChecks
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbstractChecks));
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.frameWithShadow1 = new Amellar.Common.SOA.FrameWithShadow();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rdoCollect = new System.Windows.Forms.RadioButton();
            this.rdoAcctForm = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnOK = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.cmbTeller = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTeller = new System.Windows.Forms.TextBox();
            this.rdoDummy = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(9, 6);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader1.Size = new System.Drawing.Size(253, 24);
            this.kryptonHeader1.TabIndex = 42;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Filter By";
            this.kryptonHeader1.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader1.Values.Image")));
            // 
            // frameWithShadow1
            // 
            this.frameWithShadow1.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow1.Location = new System.Drawing.Point(4, 6);
            this.frameWithShadow1.Name = "frameWithShadow1";
            this.frameWithShadow1.Size = new System.Drawing.Size(260, 248);
            this.frameWithShadow1.TabIndex = 41;
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(169, 99);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(79, 20);
            this.dtpTo.TabIndex = 4;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(58, 99);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(79, 20);
            this.dtpFrom.TabIndex = 3;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(143, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 45;
            this.label4.Text = "To";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 44;
            this.label2.Text = "Covered Period";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 43;
            this.label3.Text = "From";
            // 
            // rdoCollect
            // 
            this.rdoCollect.AutoSize = true;
            this.rdoCollect.Location = new System.Drawing.Point(37, 44);
            this.rdoCollect.Name = "rdoCollect";
            this.rdoCollect.Size = new System.Drawing.Size(76, 17);
            this.rdoCollect.TabIndex = 1;
            this.rdoCollect.Text = "&Collections";
            this.rdoCollect.UseVisualStyleBackColor = true;
            this.rdoCollect.Visible = false;
            this.rdoCollect.Click += new System.EventHandler(this.rdoCollect_Click);
            // 
            // rdoAcctForm
            // 
            this.rdoAcctForm.AutoSize = true;
            this.rdoAcctForm.Location = new System.Drawing.Point(133, 44);
            this.rdoAcctForm.Name = "rdoAcctForm";
            this.rdoAcctForm.Size = new System.Drawing.Size(116, 17);
            this.rdoAcctForm.TabIndex = 2;
            this.rdoAcctForm.Text = "&Accountable Forms";
            this.rdoAcctForm.UseVisualStyleBackColor = true;
            this.rdoAcctForm.Visible = false;
            this.rdoAcctForm.Click += new System.EventHandler(this.rdoAcctForm_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 43;
            this.label1.Text = "Date";
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(58, 130);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(79, 20);
            this.dtpDate.TabIndex = 5;
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(197, 212);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(62, 25);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(129, 212);
            this.btnOK.Name = "btnOK";
            this.btnOK.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOK.Size = new System.Drawing.Size(62, 25);
            this.btnOK.TabIndex = 8;
            this.btnOK.Values.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cmbTeller
            // 
            this.cmbTeller.FormattingEnabled = true;
            this.cmbTeller.Location = new System.Drawing.Point(25, 177);
            this.cmbTeller.Name = "cmbTeller";
            this.cmbTeller.Size = new System.Drawing.Size(223, 21);
            this.cmbTeller.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 50;
            this.label5.Text = "Teller Name";
            // 
            // txtTeller
            // 
            this.txtTeller.Location = new System.Drawing.Point(17, 217);
            this.txtTeller.Name = "txtTeller";
            this.txtTeller.Size = new System.Drawing.Size(100, 20);
            this.txtTeller.TabIndex = 7;
            this.txtTeller.Visible = false;
            // 
            // rdoDummy
            // 
            this.rdoDummy.AutoSize = true;
            this.rdoDummy.Checked = true;
            this.rdoDummy.Location = new System.Drawing.Point(12, 36);
            this.rdoDummy.Name = "rdoDummy";
            this.rdoDummy.Size = new System.Drawing.Size(14, 13);
            this.rdoDummy.TabIndex = 55;
            this.rdoDummy.TabStop = true;
            this.rdoDummy.UseVisualStyleBackColor = true;
            this.rdoDummy.Visible = false;
            // 
            // frmAbstractChecks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 257);
            this.Controls.Add(this.rdoDummy);
            this.Controls.Add(this.txtTeller);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cmbTeller);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.rdoAcctForm);
            this.Controls.Add(this.rdoCollect);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.frameWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(276, 285);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(276, 285);
            this.Name = "frmAbstractChecks";
            this.ShowIcon = false;
            this.Text = "Report of Collections and Remittances";
            this.Load += new System.EventHandler(this.frmAbstractChecks_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow1;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdoCollect;
        private System.Windows.Forms.RadioButton rdoAcctForm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOK;
        private System.Windows.Forms.ComboBox cmbTeller;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTeller;
        private System.Windows.Forms.RadioButton rdoDummy;
    }
}