namespace Amellar.Common.User
{
    partial class frmPrintUser
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
            this.chkUsers = new System.Windows.Forms.CheckBox();
            this.chkRights = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.cmbModule = new MultiColumnComboBoxDemo.MultiColumnComboBox();
            this.cmbUser = new MultiColumnComboBoxDemo.MultiColumnComboBox();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.SuspendLayout();
            // 
            // chkUsers
            // 
            this.chkUsers.AutoSize = true;
            this.chkUsers.Location = new System.Drawing.Point(61, 29);
            this.chkUsers.Name = "chkUsers";
            this.chkUsers.Size = new System.Drawing.Size(63, 17);
            this.chkUsers.TabIndex = 2;
            this.chkUsers.Text = "USERS";
            this.chkUsers.UseVisualStyleBackColor = true;
            this.chkUsers.CheckStateChanged += new System.EventHandler(this.chkUsers_CheckStateChanged);
            this.chkUsers.CheckedChanged += new System.EventHandler(this.chkUsers_CheckedChanged);
            // 
            // chkRights
            // 
            this.chkRights.AutoSize = true;
            this.chkRights.Location = new System.Drawing.Point(160, 29);
            this.chkRights.Name = "chkRights";
            this.chkRights.Size = new System.Drawing.Size(67, 17);
            this.chkRights.TabIndex = 3;
            this.chkRights.Text = "RIGHTS";
            this.chkRights.UseVisualStyleBackColor = true;
            this.chkRights.CheckStateChanged += new System.EventHandler(this.chkRights_CheckStateChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "User Code";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Module Code";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(192, 169);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(80, 25);
            this.btnClose.TabIndex = 7;
            this.btnClose.Values.Text = "Cancel";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(106, 169);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(80, 25);
            this.btnGenerate.TabIndex = 7;
            this.btnGenerate.Values.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // cmbModule
            // 
            this.cmbModule.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbModule.FormattingEnabled = true;
            this.cmbModule.Location = new System.Drawing.Point(100, 113);
            this.cmbModule.Name = "cmbModule";
            this.cmbModule.Size = new System.Drawing.Size(156, 21);
            this.cmbModule.TabIndex = 4;
            // 
            // cmbUser
            // 
            this.cmbUser.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbUser.FormattingEnabled = true;
            this.cmbUser.Location = new System.Drawing.Point(100, 86);
            this.cmbUser.Name = "cmbUser";
            this.cmbUser.Size = new System.Drawing.Size(156, 21);
            this.cmbUser.TabIndex = 4;
            this.cmbUser.SelectedIndexChanged += new System.EventHandler(this.cmbUser_SelectedIndexChanged);
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(12, 68);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(260, 95);
            this.containerWithShadow2.TabIndex = 1;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(12, 12);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(260, 56);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // frmPrintUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(284, 205);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbModule);
            this.Controls.Add(this.cmbUser);
            this.Controls.Add(this.chkRights);
            this.Controls.Add(this.chkUsers);
            this.Controls.Add(this.containerWithShadow2);
            this.Controls.Add(this.containerWithShadow1);
            this.Name = "frmPrintUser";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print User";
            this.Load += new System.EventHandler(this.frmPrintUser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private System.Windows.Forms.CheckBox chkUsers;
        private System.Windows.Forms.CheckBox chkRights;
        private MultiColumnComboBoxDemo.MultiColumnComboBox cmbUser;
        private MultiColumnComboBoxDemo.MultiColumnComboBox cmbModule;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
    }
}