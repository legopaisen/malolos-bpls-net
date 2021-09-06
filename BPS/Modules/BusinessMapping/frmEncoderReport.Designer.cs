namespace Amellar.Modules.BusinessMapping
{
    partial class frmEncoderReport
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
            this.cmbEncoder = new System.Windows.Forms.ComboBox();
            this.cmbBrgy = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBrgyCode = new System.Windows.Forms.TextBox();
            this.dtpDateFr = new System.Windows.Forms.DateTimePicker();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.chkOfficial = new System.Windows.Forms.CheckBox();
            this.chkUnofficial = new System.Windows.Forms.CheckBox();
            this.containerWithShadow3 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.SuspendLayout();
            // 
            // cmbEncoder
            // 
            this.cmbEncoder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEncoder.FormattingEnabled = true;
            this.cmbEncoder.Location = new System.Drawing.Point(85, 66);
            this.cmbEncoder.Name = "cmbEncoder";
            this.cmbEncoder.Size = new System.Drawing.Size(226, 21);
            this.cmbEncoder.TabIndex = 1;
            // 
            // cmbBrgy
            // 
            this.cmbBrgy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBrgy.FormattingEnabled = true;
            this.cmbBrgy.Location = new System.Drawing.Point(127, 93);
            this.cmbBrgy.Name = "cmbBrgy";
            this.cmbBrgy.Size = new System.Drawing.Size(184, 21);
            this.cmbBrgy.TabIndex = 1;
            this.cmbBrgy.SelectedValueChanged += new System.EventHandler(this.cmbBrgy_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Encoder";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Barangay";
            // 
            // txtBrgyCode
            // 
            this.txtBrgyCode.Location = new System.Drawing.Point(85, 94);
            this.txtBrgyCode.Name = "txtBrgyCode";
            this.txtBrgyCode.ReadOnly = true;
            this.txtBrgyCode.Size = new System.Drawing.Size(37, 20);
            this.txtBrgyCode.TabIndex = 3;
            // 
            // dtpDateFr
            // 
            this.dtpDateFr.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateFr.Location = new System.Drawing.Point(85, 146);
            this.dtpDateFr.Name = "dtpDateFr";
            this.dtpDateFr.Size = new System.Drawing.Size(86, 20);
            this.dtpDateFr.TabIndex = 5;
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateTo.Location = new System.Drawing.Point(199, 146);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(86, 20);
            this.dtpDateTo.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "From";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(177, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "to";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(134, 192);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(92, 25);
            this.btnGenerate.TabIndex = 40;
            this.btnGenerate.Values.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(232, 192);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(92, 25);
            this.btnClose.TabIndex = 40;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkOfficial
            // 
            this.chkOfficial.AutoSize = true;
            this.chkOfficial.Location = new System.Drawing.Point(30, 22);
            this.chkOfficial.Name = "chkOfficial";
            this.chkOfficial.Size = new System.Drawing.Size(114, 17);
            this.chkOfficial.TabIndex = 41;
            this.chkOfficial.Text = "Official Businesses";
            this.chkOfficial.UseVisualStyleBackColor = true;
            this.chkOfficial.CheckStateChanged += new System.EventHandler(this.chkOfficial_CheckStateChanged);
            // 
            // chkUnofficial
            // 
            this.chkUnofficial.AutoSize = true;
            this.chkUnofficial.Location = new System.Drawing.Point(180, 22);
            this.chkUnofficial.Name = "chkUnofficial";
            this.chkUnofficial.Size = new System.Drawing.Size(129, 17);
            this.chkUnofficial.TabIndex = 41;
            this.chkUnofficial.Text = "Un-official Businesses";
            this.chkUnofficial.UseVisualStyleBackColor = true;
            this.chkUnofficial.CheckStateChanged += new System.EventHandler(this.chkUnofficial_CheckStateChanged);
            // 
            // containerWithShadow3
            // 
            this.containerWithShadow3.Location = new System.Drawing.Point(12, 9);
            this.containerWithShadow3.Name = "containerWithShadow3";
            this.containerWithShadow3.Size = new System.Drawing.Size(312, 44);
            this.containerWithShadow3.TabIndex = 0;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(12, 52);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(312, 79);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(12, 130);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(312, 59);
            this.containerWithShadow2.TabIndex = 4;
            // 
            // frmEncoderReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 228);
            this.Controls.Add(this.chkUnofficial);
            this.Controls.Add(this.chkOfficial);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.dtpDateTo);
            this.Controls.Add(this.dtpDateFr);
            this.Controls.Add(this.txtBrgyCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbBrgy);
            this.Controls.Add(this.cmbEncoder);
            this.Controls.Add(this.containerWithShadow3);
            this.Controls.Add(this.containerWithShadow1);
            this.Controls.Add(this.containerWithShadow2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmEncoderReport";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Encoder\'s Report";
            this.Load += new System.EventHandler(this.frmEncoderReport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.ComboBox cmbEncoder;
        private System.Windows.Forms.ComboBox cmbBrgy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBrgyCode;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private System.Windows.Forms.DateTimePicker dtpDateFr;
        private System.Windows.Forms.DateTimePicker dtpDateTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow3;
        private System.Windows.Forms.CheckBox chkOfficial;
        private System.Windows.Forms.CheckBox chkUnofficial;
    }
}