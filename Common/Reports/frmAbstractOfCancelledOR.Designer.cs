namespace Amellar.Common.Reports
{
    partial class frmAbstractOfCancelledOR
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbstractOfCancelledOR));
            this.lblTitle = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.cmbTeller = new System.Windows.Forms.ComboBox();
            this.lblTeller = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.rdoTeller = new System.Windows.Forms.RadioButton();
            this.rdoOR = new System.Windows.Forms.RadioButton();
            this.rdoDaily = new System.Windows.Forms.RadioButton();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.txtORto = new System.Windows.Forms.TextBox();
            this.txtORFrom = new System.Windows.Forms.TextBox();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = false;
            this.lblTitle.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.lblTitle.Location = new System.Drawing.Point(165, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.lblTitle.Size = new System.Drawing.Size(237, 24);
            this.lblTitle.TabIndex = 74;
            this.lblTitle.Text = "Covered Period";
            this.lblTitle.Values.Description = "";
            this.lblTitle.Values.Heading = "Covered Period";
            this.lblTitle.Values.Image = ((System.Drawing.Image)(resources.GetObject("lblTitle.Values.Image")));
            // 
            // cmbTeller
            // 
            this.cmbTeller.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbTeller.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbTeller.FormattingEnabled = true;
            this.cmbTeller.Location = new System.Drawing.Point(223, 87);
            this.cmbTeller.Name = "cmbTeller";
            this.cmbTeller.Size = new System.Drawing.Size(169, 21);
            this.cmbTeller.TabIndex = 67;
            // 
            // lblTeller
            // 
            this.lblTeller.AutoSize = true;
            this.lblTeller.Location = new System.Drawing.Point(176, 89);
            this.lblTeller.Name = "lblTeller";
            this.lblTeller.Size = new System.Drawing.Size(33, 13);
            this.lblTeller.TabIndex = 71;
            this.lblTeller.Text = "Teller";
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(316, 50);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(79, 20);
            this.dtpTo.TabIndex = 64;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(209, 50);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(79, 20);
            this.dtpFrom.TabIndex = 63;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(291, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 73;
            this.label4.Text = "To";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(176, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 72;
            this.label3.Text = "From";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(330, 138);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(62, 25);
            this.btnCancel.TabIndex = 69;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(251, 138);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(73, 25);
            this.btnGenerate.TabIndex = 68;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.Values.ExtraText = "";
            this.btnGenerate.Values.Image = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedNormal = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedPressed = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedTracking = null;
            this.btnGenerate.Values.Text = "&Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // rdoTeller
            // 
            this.rdoTeller.AutoSize = true;
            this.rdoTeller.Location = new System.Drawing.Point(21, 45);
            this.rdoTeller.Name = "rdoTeller";
            this.rdoTeller.Size = new System.Drawing.Size(51, 17);
            this.rdoTeller.TabIndex = 62;
            this.rdoTeller.TabStop = true;
            this.rdoTeller.Text = "Teller";
            this.rdoTeller.UseVisualStyleBackColor = true;
            this.rdoTeller.Click += new System.EventHandler(this.rdoTeller_Click);
            // 
            // rdoOR
            // 
            this.rdoOR.AutoSize = true;
            this.rdoOR.Location = new System.Drawing.Point(21, 71);
            this.rdoOR.Name = "rdoOR";
            this.rdoOR.Size = new System.Drawing.Size(102, 17);
            this.rdoOR.TabIndex = 61;
            this.rdoOR.TabStop = true;
            this.rdoOR.Text = "Official Receipts";
            this.rdoOR.UseVisualStyleBackColor = true;
            this.rdoOR.Click += new System.EventHandler(this.rdoOR_Click);
            // 
            // rdoDaily
            // 
            this.rdoDaily.AutoSize = true;
            this.rdoDaily.Location = new System.Drawing.Point(21, 97);
            this.rdoDaily.Name = "rdoDaily";
            this.rdoDaily.Size = new System.Drawing.Size(100, 17);
            this.rdoDaily.TabIndex = 60;
            this.rdoDaily.TabStop = true;
            this.rdoDaily.Text = "Daily Aggregate";
            this.rdoDaily.UseVisualStyleBackColor = true;
            this.rdoDaily.Click += new System.EventHandler(this.rdoDaily_Click);
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(8, 8);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader1.Size = new System.Drawing.Size(148, 24);
            this.kryptonHeader1.TabIndex = 70;
            this.kryptonHeader1.Text = "Group By";
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Group By";
            this.kryptonHeader1.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader1.Values.Image")));
            // 
            // txtORto
            // 
            this.txtORto.Location = new System.Drawing.Point(316, 50);
            this.txtORto.Name = "txtORto";
            this.txtORto.Size = new System.Drawing.Size(79, 20);
            this.txtORto.TabIndex = 66;
            // 
            // txtORFrom
            // 
            this.txtORFrom.Location = new System.Drawing.Point(209, 50);
            this.txtORFrom.Name = "txtORFrom";
            this.txtORFrom.Size = new System.Drawing.Size(79, 20);
            this.txtORFrom.TabIndex = 65;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(5, 8);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(152, 131);
            this.containerWithShadow1.TabIndex = 75;
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(162, 6);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(241, 131);
            this.containerWithShadow2.TabIndex = 75;
            // 
            // frmAbstractOfCancelledOR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(408, 170);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.cmbTeller);
            this.Controls.Add(this.lblTeller);
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
            this.Controls.Add(this.txtORto);
            this.Controls.Add(this.txtORFrom);
            this.Controls.Add(this.containerWithShadow2);
            this.Controls.Add(this.containerWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAbstractOfCancelledOR";
            this.Text = "Abstract Of Cancelled O.R.";
            this.Load += new System.EventHandler(this.frmAbstractOfCancelledOR_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonHeader lblTitle;
        private System.Windows.Forms.ComboBox cmbTeller;
        private System.Windows.Forms.Label lblTeller;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private System.Windows.Forms.RadioButton rdoTeller;
        private System.Windows.Forms.RadioButton rdoOR;
        private System.Windows.Forms.RadioButton rdoDaily;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private System.Windows.Forms.TextBox txtORto;
        private System.Windows.Forms.TextBox txtORFrom;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
    }
}