namespace Amellar.Modules.Utilities
{
    partial class frmDefaultValuesSet
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
            this.label6 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.btnResetDefault = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnOK = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.cmbMainBnsType = new MultiColumnComboBoxDemo.MultiColumnComboBox();
            this.cmbFees = new MultiColumnComboBoxDemo.MultiColumnComboBox();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBnsCode = new System.Windows.Forms.TextBox();
            this.txtBnsCode2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 34;
            this.label6.Text = "Business Type:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(30, 35);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(89, 13);
            this.label19.TabIndex = 33;
            this.label19.Text = "Fees Description:";
            // 
            // btnResetDefault
            // 
            this.btnResetDefault.Location = new System.Drawing.Point(12, 125);
            this.btnResetDefault.Name = "btnResetDefault";
            this.btnResetDefault.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnResetDefault.Size = new System.Drawing.Size(98, 25);
            this.btnResetDefault.TabIndex = 6;
            this.btnResetDefault.Text = "Reset all";
            this.btnResetDefault.Values.ExtraText = "";
            this.btnResetDefault.Values.Image = null;
            this.btnResetDefault.Values.ImageStates.ImageCheckedNormal = null;
            this.btnResetDefault.Values.ImageStates.ImageCheckedPressed = null;
            this.btnResetDefault.Values.ImageStates.ImageCheckedTracking = null;
            this.btnResetDefault.Values.Text = "Reset all";
            this.btnResetDefault.Visible = false;
            this.btnResetDefault.Click += new System.EventHandler(this.btnResetDefault_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(213, 125);
            this.btnOK.Name = "btnOK";
            this.btnOK.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOK.Size = new System.Drawing.Size(113, 25);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.Values.ExtraText = "";
            this.btnOK.Values.Image = null;
            this.btnOK.Values.ImageStates.ImageCheckedNormal = null;
            this.btnOK.Values.ImageStates.ImageCheckedPressed = null;
            this.btnOK.Values.ImageStates.ImageCheckedTracking = null;
            this.btnOK.Values.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cmbMainBnsType
            // 
            this.cmbMainBnsType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbMainBnsType.FormattingEnabled = true;
            this.cmbMainBnsType.Location = new System.Drawing.Point(125, 54);
            this.cmbMainBnsType.Name = "cmbMainBnsType";
            this.cmbMainBnsType.Size = new System.Drawing.Size(189, 21);
            this.cmbMainBnsType.TabIndex = 2;
            this.cmbMainBnsType.SelectedValueChanged += new System.EventHandler(this.cmbMainBnsType_SelectedValueChanged);
            // 
            // cmbFees
            // 
            this.cmbFees.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbFees.FormattingEnabled = true;
            this.cmbFees.Location = new System.Drawing.Point(125, 27);
            this.cmbFees.Name = "cmbFees";
            this.cmbFees.Size = new System.Drawing.Size(189, 21);
            this.cmbFees.TabIndex = 1;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(12, 12);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(314, 107);
            this.containerWithShadow1.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Business Code:";
            // 
            // txtBnsCode
            // 
            this.txtBnsCode.Location = new System.Drawing.Point(125, 81);
            this.txtBnsCode.Name = "txtBnsCode";
            this.txtBnsCode.ReadOnly = true;
            this.txtBnsCode.Size = new System.Drawing.Size(37, 20);
            this.txtBnsCode.TabIndex = 37;
            // 
            // txtBnsCode2
            // 
            this.txtBnsCode2.Location = new System.Drawing.Point(168, 81);
            this.txtBnsCode2.Name = "txtBnsCode2";
            this.txtBnsCode2.Size = new System.Drawing.Size(54, 20);
            this.txtBnsCode2.TabIndex = 3;
            // 
            // frmDefaultValuesSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(336, 161);
            this.Controls.Add(this.txtBnsCode2);
            this.Controls.Add(this.txtBnsCode);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnResetDefault);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.cmbMainBnsType);
            this.Controls.Add(this.cmbFees);
            this.Controls.Add(this.containerWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDefaultValuesSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Set Default Values";
            this.Load += new System.EventHandler(this.frmDefaultValuesSet_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDefaultValuesSet_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label19;
        private MultiColumnComboBoxDemo.MultiColumnComboBox cmbMainBnsType;
        private MultiColumnComboBoxDemo.MultiColumnComboBox cmbFees;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnResetDefault;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBnsCode;
        private System.Windows.Forms.TextBox txtBnsCode2;
    }
}