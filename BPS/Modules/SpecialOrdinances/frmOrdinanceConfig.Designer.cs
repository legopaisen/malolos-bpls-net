namespace Amellar.Modules.SpecialOrdinances
{
    partial class frmOrdinanceConfig
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.grpExemption = new System.Windows.Forms.GroupBox();
            this.rdoFees = new System.Windows.Forms.RadioButton();
            this.rdoTax = new System.Windows.Forms.RadioButton();
            this.txtYrRange2 = new System.Windows.Forms.TextBox();
            this.txtYrRange1 = new System.Windows.Forms.TextBox();
            this.txtRate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbFees = new System.Windows.Forms.ComboBox();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.grpOrdinance = new System.Windows.Forms.GroupBox();
            this.rdoPeza = new System.Windows.Forms.RadioButton();
            this.rdoBoi = new System.Windows.Forms.RadioButton();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpIncentive = new System.Windows.Forms.GroupBox();
            this.rdoGreen = new System.Windows.Forms.RadioButton();
            this.rdoFiscal = new System.Windows.Forms.RadioButton();
            this.grpOtherConfig = new System.Windows.Forms.GroupBox();
            this.rdoOptional = new System.Windows.Forms.RadioButton();
            this.rdoRequired = new System.Windows.Forms.RadioButton();
            this.grpExemption.SuspendLayout();
            this.grpOrdinance.SuspendLayout();
            this.grpIncentive.SuspendLayout();
            this.grpOtherConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(227, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Fee Desc.";
            // 
            // grpExemption
            // 
            this.grpExemption.Controls.Add(this.rdoFees);
            this.grpExemption.Controls.Add(this.rdoTax);
            this.grpExemption.Controls.Add(this.txtYrRange2);
            this.grpExemption.Controls.Add(this.txtYrRange1);
            this.grpExemption.Controls.Add(this.txtRate);
            this.grpExemption.Controls.Add(this.label4);
            this.grpExemption.Controls.Add(this.label3);
            this.grpExemption.Controls.Add(this.label5);
            this.grpExemption.Controls.Add(this.label1);
            this.grpExemption.Controls.Add(this.cmbFees);
            this.grpExemption.Controls.Add(this.label2);
            this.grpExemption.Enabled = false;
            this.grpExemption.Location = new System.Drawing.Point(24, 101);
            this.grpExemption.Name = "grpExemption";
            this.grpExemption.Size = new System.Drawing.Size(284, 138);
            this.grpExemption.TabIndex = 2;
            this.grpExemption.TabStop = false;
            this.grpExemption.Text = " Exemption ";
            // 
            // rdoFees
            // 
            this.rdoFees.AutoSize = true;
            this.rdoFees.Location = new System.Drawing.Point(17, 49);
            this.rdoFees.Name = "rdoFees";
            this.rdoFees.Size = new System.Drawing.Size(102, 17);
            this.rdoFees.TabIndex = 3;
            this.rdoFees.TabStop = true;
            this.rdoFees.Text = "Regulatory Fees";
            this.rdoFees.UseVisualStyleBackColor = true;
            this.rdoFees.CheckedChanged += new System.EventHandler(this.rdoFees_CheckedChanged);
            // 
            // rdoTax
            // 
            this.rdoTax.AutoSize = true;
            this.rdoTax.Location = new System.Drawing.Point(17, 26);
            this.rdoTax.Name = "rdoTax";
            this.rdoTax.Size = new System.Drawing.Size(88, 17);
            this.rdoTax.TabIndex = 3;
            this.rdoTax.TabStop = true;
            this.rdoTax.Text = "Business Tax";
            this.rdoTax.UseVisualStyleBackColor = true;
            this.rdoTax.CheckedChanged += new System.EventHandler(this.rdoTax_CheckedChanged);
            // 
            // txtYrRange2
            // 
            this.txtYrRange2.Location = new System.Drawing.Point(239, 105);
            this.txtYrRange2.Name = "txtYrRange2";
            this.txtYrRange2.Size = new System.Drawing.Size(25, 20);
            this.txtYrRange2.TabIndex = 2;
            // 
            // txtYrRange1
            // 
            this.txtYrRange1.Location = new System.Drawing.Point(200, 105);
            this.txtYrRange1.Name = "txtYrRange1";
            this.txtYrRange1.Size = new System.Drawing.Size(25, 20);
            this.txtYrRange1.TabIndex = 2;
            // 
            // txtRate
            // 
            this.txtRate.Location = new System.Drawing.Point(50, 105);
            this.txtRate.Name = "txtRate";
            this.txtRate.Size = new System.Drawing.Size(53, 20);
            this.txtRate.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(130, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Year Range";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Rate";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(105, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "%";
            // 
            // cmbFees
            // 
            this.cmbFees.Enabled = false;
            this.cmbFees.FormattingEnabled = true;
            this.cmbFees.Location = new System.Drawing.Point(97, 72);
            this.cmbFees.Name = "cmbFees";
            this.cmbFees.Size = new System.Drawing.Size(174, 21);
            this.cmbFees.TabIndex = 1;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(9, 12);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(312, 333);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // grpOrdinance
            // 
            this.grpOrdinance.Controls.Add(this.rdoPeza);
            this.grpOrdinance.Controls.Add(this.rdoBoi);
            this.grpOrdinance.Enabled = false;
            this.grpOrdinance.Location = new System.Drawing.Point(24, 25);
            this.grpOrdinance.Name = "grpOrdinance";
            this.grpOrdinance.Size = new System.Drawing.Size(138, 70);
            this.grpOrdinance.TabIndex = 3;
            this.grpOrdinance.TabStop = false;
            this.grpOrdinance.Text = " Ordinance ";
            // 
            // rdoPeza
            // 
            this.rdoPeza.AutoSize = true;
            this.rdoPeza.Location = new System.Drawing.Point(27, 42);
            this.rdoPeza.Name = "rdoPeza";
            this.rdoPeza.Size = new System.Drawing.Size(53, 17);
            this.rdoPeza.TabIndex = 0;
            this.rdoPeza.TabStop = true;
            this.rdoPeza.Text = "PEZA";
            this.rdoPeza.UseVisualStyleBackColor = true;
            this.rdoPeza.CheckedChanged += new System.EventHandler(this.rdoPeza_CheckedChanged);
            // 
            // rdoBoi
            // 
            this.rdoBoi.AutoSize = true;
            this.rdoBoi.Location = new System.Drawing.Point(27, 19);
            this.rdoBoi.Name = "rdoBoi";
            this.rdoBoi.Size = new System.Drawing.Size(43, 17);
            this.rdoBoi.TabIndex = 0;
            this.rdoBoi.TabStop = true;
            this.rdoBoi.Text = "BOI";
            this.rdoBoi.UseVisualStyleBackColor = true;
            this.rdoBoi.CheckedChanged += new System.EventHandler(this.rdoBoi_CheckedChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(79, 297);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 25);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(157, 297);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(72, 25);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(236, 297);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 25);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // grpIncentive
            // 
            this.grpIncentive.Controls.Add(this.rdoGreen);
            this.grpIncentive.Controls.Add(this.rdoFiscal);
            this.grpIncentive.Enabled = false;
            this.grpIncentive.Location = new System.Drawing.Point(170, 25);
            this.grpIncentive.Name = "grpIncentive";
            this.grpIncentive.Size = new System.Drawing.Size(138, 70);
            this.grpIncentive.TabIndex = 3;
            this.grpIncentive.TabStop = false;
            this.grpIncentive.Text = " Type of Incentive";
            // 
            // rdoGreen
            // 
            this.rdoGreen.AutoSize = true;
            this.rdoGreen.Location = new System.Drawing.Point(22, 42);
            this.rdoGreen.Name = "rdoGreen";
            this.rdoGreen.Size = new System.Drawing.Size(101, 17);
            this.rdoGreen.TabIndex = 0;
            this.rdoGreen.TabStop = true;
            this.rdoGreen.Text = "Green Incentive";
            this.rdoGreen.UseVisualStyleBackColor = true;
            // 
            // rdoFiscal
            // 
            this.rdoFiscal.AutoSize = true;
            this.rdoFiscal.Location = new System.Drawing.Point(22, 19);
            this.rdoFiscal.Name = "rdoFiscal";
            this.rdoFiscal.Size = new System.Drawing.Size(99, 17);
            this.rdoFiscal.TabIndex = 0;
            this.rdoFiscal.TabStop = true;
            this.rdoFiscal.Text = "Fiscal Incentive";
            this.rdoFiscal.UseVisualStyleBackColor = true;
            this.rdoFiscal.CheckedChanged += new System.EventHandler(this.rdoFiscal_CheckedChanged);
            // 
            // grpOtherConfig
            // 
            this.grpOtherConfig.Controls.Add(this.rdoOptional);
            this.grpOtherConfig.Controls.Add(this.rdoRequired);
            this.grpOtherConfig.Enabled = false;
            this.grpOtherConfig.Location = new System.Drawing.Point(24, 245);
            this.grpOtherConfig.Name = "grpOtherConfig";
            this.grpOtherConfig.Size = new System.Drawing.Size(284, 44);
            this.grpOtherConfig.TabIndex = 5;
            this.grpOtherConfig.TabStop = false;
            // 
            // rdoOptional
            // 
            this.rdoOptional.AutoSize = true;
            this.rdoOptional.Location = new System.Drawing.Point(146, 16);
            this.rdoOptional.Name = "rdoOptional";
            this.rdoOptional.Size = new System.Drawing.Size(64, 17);
            this.rdoOptional.TabIndex = 0;
            this.rdoOptional.TabStop = true;
            this.rdoOptional.Text = "Optional";
            this.rdoOptional.UseVisualStyleBackColor = true;
            // 
            // rdoRequired
            // 
            this.rdoRequired.AutoSize = true;
            this.rdoRequired.Location = new System.Drawing.Point(27, 16);
            this.rdoRequired.Name = "rdoRequired";
            this.rdoRequired.Size = new System.Drawing.Size(68, 17);
            this.rdoRequired.TabIndex = 0;
            this.rdoRequired.TabStop = true;
            this.rdoRequired.Text = "Required";
            this.rdoRequired.UseVisualStyleBackColor = true;
            // 
            // frmOrdinanceConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 356);
            this.ControlBox = false;
            this.Controls.Add(this.grpOtherConfig);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.grpIncentive);
            this.Controls.Add(this.grpOrdinance);
            this.Controls.Add(this.grpExemption);
            this.Controls.Add(this.containerWithShadow1);
            this.Name = "frmOrdinanceConfig";
            this.Text = "Special Ordinances Configuration";
            this.Load += new System.EventHandler(this.frmOrdinanceConfig_Load);
            this.grpExemption.ResumeLayout(false);
            this.grpExemption.PerformLayout();
            this.grpOrdinance.ResumeLayout(false);
            this.grpOrdinance.PerformLayout();
            this.grpIncentive.ResumeLayout(false);
            this.grpIncentive.PerformLayout();
            this.grpOtherConfig.ResumeLayout(false);
            this.grpOtherConfig.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox grpExemption;
        private System.Windows.Forms.ComboBox cmbFees;
        private System.Windows.Forms.TextBox txtRate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdoFees;
        private System.Windows.Forms.RadioButton rdoTax;
        private System.Windows.Forms.TextBox txtYrRange2;
        private System.Windows.Forms.TextBox txtYrRange1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox grpOrdinance;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.RadioButton rdoPeza;
        private System.Windows.Forms.RadioButton rdoBoi;
        private System.Windows.Forms.GroupBox grpIncentive;
        private System.Windows.Forms.RadioButton rdoGreen;
        private System.Windows.Forms.RadioButton rdoFiscal;
        private System.Windows.Forms.GroupBox grpOtherConfig;
        private System.Windows.Forms.RadioButton rdoOptional;
        private System.Windows.Forms.RadioButton rdoRequired;
    }
}