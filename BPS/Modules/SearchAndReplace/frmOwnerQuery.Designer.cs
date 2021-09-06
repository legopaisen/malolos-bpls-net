namespace Amellar.BPLS.SearchAndReplace
{
    partial class frmOwnerQuery
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
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbBrgy = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.chkName = new System.Windows.Forms.CheckBox();
            this.chkBrgy = new System.Windows.Forms.CheckBox();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.chkBIN = new System.Windows.Forms.CheckBox();
            this.chkPrevOwn = new System.Windows.Forms.CheckBox();
            this.chkOwnPlace = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(160, 174);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(72, 25);
            this.btnGenerate.TabIndex = 8;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.Values.ExtraText = "";
            this.btnGenerate.Values.Image = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedNormal = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedPressed = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedTracking = null;
            this.btnGenerate.Values.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(238, 174);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(72, 25);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Group by:";
            // 
            // cmbBrgy
            // 
            this.cmbBrgy.FormattingEnabled = true;
            this.cmbBrgy.Location = new System.Drawing.Point(89, 70);
            this.cmbBrgy.Name = "cmbBrgy";
            this.cmbBrgy.Size = new System.Drawing.Size(207, 21);
            this.cmbBrgy.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Barangay";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Last Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "First Name";
            // 
            // txtLastName
            // 
            this.txtLastName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLastName.Location = new System.Drawing.Point(89, 97);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(207, 20);
            this.txtLastName.TabIndex = 13;
            // 
            // txtFirstName
            // 
            this.txtFirstName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFirstName.Location = new System.Drawing.Point(89, 123);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(207, 20);
            this.txtFirstName.TabIndex = 13;
            // 
            // chkName
            // 
            this.chkName.AutoSize = true;
            this.chkName.Location = new System.Drawing.Point(107, 24);
            this.chkName.Name = "chkName";
            this.chkName.Size = new System.Drawing.Size(95, 17);
            this.chkName.TabIndex = 14;
            this.chkName.Text = "Owner\'s Name";
            this.chkName.UseVisualStyleBackColor = true;
            this.chkName.CheckStateChanged += new System.EventHandler(this.chkName_CheckStateChanged);
            // 
            // chkBrgy
            // 
            this.chkBrgy.AutoSize = true;
            this.chkBrgy.Location = new System.Drawing.Point(208, 24);
            this.chkBrgy.Name = "chkBrgy";
            this.chkBrgy.Size = new System.Drawing.Size(71, 17);
            this.chkBrgy.TabIndex = 15;
            this.chkBrgy.Text = "Barangay";
            this.chkBrgy.UseVisualStyleBackColor = true;
            this.chkBrgy.CheckStateChanged += new System.EventHandler(this.chkBrgy_CheckStateChanged);
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(7, 54);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(303, 109);
            this.containerWithShadow2.TabIndex = 0;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(7, 12);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(303, 43);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // chkBIN
            // 
            this.chkBIN.AutoSize = true;
            this.chkBIN.Location = new System.Drawing.Point(23, 164);
            this.chkBIN.Name = "chkBIN";
            this.chkBIN.Size = new System.Drawing.Size(84, 17);
            this.chkBIN.TabIndex = 16;
            this.chkBIN.Text = "Print Lessee";
            this.chkBIN.UseVisualStyleBackColor = true;
            // 
            // chkPrevOwn
            // 
            this.chkPrevOwn.AutoSize = true;
            this.chkPrevOwn.Location = new System.Drawing.Point(23, 182);
            this.chkPrevOwn.Name = "chkPrevOwn";
            this.chkPrevOwn.Size = new System.Drawing.Size(124, 17);
            this.chkPrevOwn.TabIndex = 17;
            this.chkPrevOwn.Text = "Print Prev Own Entry";
            this.chkPrevOwn.UseVisualStyleBackColor = true;
            // 
            // chkOwnPlace
            // 
            this.chkOwnPlace.AutoSize = true;
            this.chkOwnPlace.Location = new System.Drawing.Point(23, 201);
            this.chkOwnPlace.Name = "chkOwnPlace";
            this.chkOwnPlace.Size = new System.Drawing.Size(141, 17);
            this.chkOwnPlace.TabIndex = 18;
            this.chkOwnPlace.Text = "Print Owned Place Entry";
            this.chkOwnPlace.UseVisualStyleBackColor = true;
            // 
            // frmOwnerQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 222);
            this.ControlBox = false;
            this.Controls.Add(this.chkOwnPlace);
            this.Controls.Add(this.chkPrevOwn);
            this.Controls.Add(this.chkBIN);
            this.Controls.Add(this.chkBrgy);
            this.Controls.Add(this.chkName);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbBrgy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.containerWithShadow2);
            this.Controls.Add(this.containerWithShadow1);
            this.Name = "frmOwnerQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Printing Options";
            this.Load += new System.EventHandler(this.frmPrintOptions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private System.Windows.Forms.Label label1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private System.Windows.Forms.ComboBox cmbBrgy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.CheckBox chkName;
        private System.Windows.Forms.CheckBox chkBrgy;
        private System.Windows.Forms.CheckBox chkBIN;
        private System.Windows.Forms.CheckBox chkPrevOwn;
        private System.Windows.Forms.CheckBox chkOwnPlace;
    }
}