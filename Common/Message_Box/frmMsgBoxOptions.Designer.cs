namespace Amellar.Common.Message_Box
{
    partial class frmMsgBoxOptions
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
            System.Windows.Forms.PictureBox picQuestion;
            this.btnNo = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnYes = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblMsg = new System.Windows.Forms.Label();
            this.pnlYN = new System.Windows.Forms.Panel();
            this.rdoPnlYNNo = new System.Windows.Forms.RadioButton();
            this.rdoPnlYNYes = new System.Windows.Forms.RadioButton();
            this.pnlYNB = new System.Windows.Forms.Panel();
            this.rdopnlYNBBoth = new System.Windows.Forms.RadioButton();
            this.rdopnlYNBNo = new System.Windows.Forms.RadioButton();
            this.rdopnlYNBYes = new System.Windows.Forms.RadioButton();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            picQuestion = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(picQuestion)).BeginInit();
            this.pnlYN.SuspendLayout();
            this.pnlYNB.SuspendLayout();
            this.SuspendLayout();
            // 
            // picQuestion
            // 
            picQuestion.Image = global::Amellar.Common.Message_Box.Properties.Resources.question;
            picQuestion.Location = new System.Drawing.Point(16, 11);
            picQuestion.Name = "picQuestion";
            picQuestion.Size = new System.Drawing.Size(62, 58);
            picQuestion.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            picQuestion.TabIndex = 11;
            picQuestion.TabStop = false;
            // 
            // btnNo
            // 
            this.btnNo.Location = new System.Drawing.Point(147, 161);
            this.btnNo.Name = "btnNo";
            this.btnNo.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnNo.Size = new System.Drawing.Size(66, 25);
            this.btnNo.TabIndex = 17;
            this.btnNo.Values.Text = "&Cancel";
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(75, 161);
            this.btnYes.Name = "btnYes";
            this.btnYes.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnYes.Size = new System.Drawing.Size(66, 25);
            this.btnYes.TabIndex = 16;
            this.btnYes.Values.Text = "&Ok";
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // lblMsg
            // 
            this.lblMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.Location = new System.Drawing.Point(83, 13);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(124, 50);
            this.lblMsg.TabIndex = 10;
            this.lblMsg.Text = "xxx";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlYN
            // 
            this.pnlYN.Controls.Add(this.rdoPnlYNNo);
            this.pnlYN.Controls.Add(this.rdoPnlYNYes);
            this.pnlYN.Location = new System.Drawing.Point(17, 70);
            this.pnlYN.Name = "pnlYN";
            this.pnlYN.Size = new System.Drawing.Size(191, 65);
            this.pnlYN.TabIndex = 18;
            // 
            // rdoPnlYNNo
            // 
            this.rdoPnlYNNo.AutoSize = true;
            this.rdoPnlYNNo.Location = new System.Drawing.Point(12, 36);
            this.rdoPnlYNNo.Name = "rdoPnlYNNo";
            this.rdoPnlYNNo.Size = new System.Drawing.Size(39, 17);
            this.rdoPnlYNNo.TabIndex = 0;
            this.rdoPnlYNNo.TabStop = true;
            this.rdoPnlYNNo.Text = "No";
            this.rdoPnlYNNo.UseVisualStyleBackColor = true;
            this.rdoPnlYNNo.Click += new System.EventHandler(this.rdoPnlYNNo_Click);
            // 
            // rdoPnlYNYes
            // 
            this.rdoPnlYNYes.AutoSize = true;
            this.rdoPnlYNYes.Location = new System.Drawing.Point(12, 13);
            this.rdoPnlYNYes.Name = "rdoPnlYNYes";
            this.rdoPnlYNYes.Size = new System.Drawing.Size(43, 17);
            this.rdoPnlYNYes.TabIndex = 0;
            this.rdoPnlYNYes.TabStop = true;
            this.rdoPnlYNYes.Text = "Yes";
            this.rdoPnlYNYes.UseVisualStyleBackColor = true;
            this.rdoPnlYNYes.Click += new System.EventHandler(this.rdoPnlYNYes_Click);
            // 
            // pnlYNB
            // 
            this.pnlYNB.Controls.Add(this.rdopnlYNBBoth);
            this.pnlYNB.Controls.Add(this.rdopnlYNBNo);
            this.pnlYNB.Controls.Add(this.rdopnlYNBYes);
            this.pnlYNB.Location = new System.Drawing.Point(17, 68);
            this.pnlYNB.Name = "pnlYNB";
            this.pnlYNB.Size = new System.Drawing.Size(191, 75);
            this.pnlYNB.TabIndex = 18;
            // 
            // rdopnlYNBBoth
            // 
            this.rdopnlYNBBoth.AutoSize = true;
            this.rdopnlYNBBoth.Location = new System.Drawing.Point(12, 51);
            this.rdopnlYNBBoth.Name = "rdopnlYNBBoth";
            this.rdopnlYNBBoth.Size = new System.Drawing.Size(47, 17);
            this.rdopnlYNBBoth.TabIndex = 0;
            this.rdopnlYNBBoth.TabStop = true;
            this.rdopnlYNBBoth.Text = "Both";
            this.rdopnlYNBBoth.UseVisualStyleBackColor = true;
            this.rdopnlYNBBoth.Click += new System.EventHandler(this.rdopnlYNBBoth_Click);
            // 
            // rdopnlYNBNo
            // 
            this.rdopnlYNBNo.AutoSize = true;
            this.rdopnlYNBNo.Location = new System.Drawing.Point(12, 30);
            this.rdopnlYNBNo.Name = "rdopnlYNBNo";
            this.rdopnlYNBNo.Size = new System.Drawing.Size(39, 17);
            this.rdopnlYNBNo.TabIndex = 0;
            this.rdopnlYNBNo.TabStop = true;
            this.rdopnlYNBNo.Text = "No";
            this.rdopnlYNBNo.UseVisualStyleBackColor = true;
            this.rdopnlYNBNo.Click += new System.EventHandler(this.rdopnlYNBNo_Click);
            // 
            // rdopnlYNBYes
            // 
            this.rdopnlYNBYes.AutoSize = true;
            this.rdopnlYNBYes.Location = new System.Drawing.Point(12, 9);
            this.rdopnlYNBYes.Name = "rdopnlYNBYes";
            this.rdopnlYNBYes.Size = new System.Drawing.Size(43, 17);
            this.rdopnlYNBYes.TabIndex = 0;
            this.rdopnlYNBYes.TabStop = true;
            this.rdopnlYNBYes.Text = "Yes";
            this.rdopnlYNBYes.UseVisualStyleBackColor = true;
            this.rdopnlYNBYes.Click += new System.EventHandler(this.rdopnlYNBYes_Click);
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(5, 154);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(213, 45);
            this.containerWithShadow2.TabIndex = 15;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(5, 4);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(213, 151);
            this.containerWithShadow1.TabIndex = 14;
            // 
            // frmMsgBoxOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 198);
            this.ControlBox = false;
            this.Controls.Add(this.pnlYNB);
            this.Controls.Add(this.pnlYN);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(picQuestion);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.containerWithShadow2);
            this.Controls.Add(this.containerWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMsgBoxOptions";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Message Box";
            this.Load += new System.EventHandler(this.frmMesgBoxOptions_Load);
            ((System.ComponentModel.ISupportInitialize)(picQuestion)).EndInit();
            this.pnlYN.ResumeLayout(false);
            this.pnlYN.PerformLayout();
            this.pnlYNB.ResumeLayout(false);
            this.pnlYNB.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnYes;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Panel pnlYN;
        private System.Windows.Forms.RadioButton rdoPnlYNNo;
        private System.Windows.Forms.RadioButton rdoPnlYNYes;
        private System.Windows.Forms.Panel pnlYNB;
        private System.Windows.Forms.RadioButton rdopnlYNBBoth;
        private System.Windows.Forms.RadioButton rdopnlYNBNo;
        private System.Windows.Forms.RadioButton rdopnlYNBYes;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnNo;
    }
}