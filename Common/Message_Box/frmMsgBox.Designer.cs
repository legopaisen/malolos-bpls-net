namespace Amellar.Common.Message_Box
{
    partial class frmMsgBox
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
            this.lblMsg = new System.Windows.Forms.Label();
            this.picInfo = new System.Windows.Forms.PictureBox();
            this.picCritical = new System.Windows.Forms.PictureBox();
            this.picQuestion = new System.Windows.Forms.PictureBox();
            this.btnYes = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnNo = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            ((System.ComponentModel.ISupportInitialize)(this.picInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCritical)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picQuestion)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMsg
            // 
            this.lblMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.Location = new System.Drawing.Point(91, 20);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(129, 50);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Text = "xxx";
            // 
            // picInfo
            // 
            this.picInfo.Image = global::Amellar.Common.Message_Box.Properties.Resources.info;
            this.picInfo.Location = new System.Drawing.Point(24, 12);
            this.picInfo.Name = "picInfo";
            this.picInfo.Size = new System.Drawing.Size(62, 58);
            this.picInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picInfo.TabIndex = 3;
            this.picInfo.TabStop = false;
            this.picInfo.Visible = false;
            // 
            // picCritical
            // 
            this.picCritical.Image = global::Amellar.Common.Message_Box.Properties.Resources.critical;
            this.picCritical.Location = new System.Drawing.Point(24, 12);
            this.picCritical.Name = "picCritical";
            this.picCritical.Size = new System.Drawing.Size(62, 58);
            this.picCritical.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCritical.TabIndex = 2;
            this.picCritical.TabStop = false;
            this.picCritical.Visible = false;
            // 
            // picQuestion
            // 
            this.picQuestion.Image = global::Amellar.Common.Message_Box.Properties.Resources.question;
            this.picQuestion.Location = new System.Drawing.Point(24, 12);
            this.picQuestion.Name = "picQuestion";
            this.picQuestion.Size = new System.Drawing.Size(62, 58);
            this.picQuestion.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picQuestion.TabIndex = 1;
            this.picQuestion.TabStop = false;
            this.picQuestion.Visible = false;
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(82, 98);
            this.btnYes.Name = "btnYes";
            this.btnYes.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnYes.Size = new System.Drawing.Size(66, 25);
            this.btnYes.TabIndex = 8;
            this.btnYes.Values.Text = "&Yes";
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnNo
            // 
            this.btnNo.Location = new System.Drawing.Point(154, 98);
            this.btnNo.Name = "btnNo";
            this.btnNo.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnNo.Size = new System.Drawing.Size(66, 25);
            this.btnNo.TabIndex = 9;
            this.btnNo.Values.Text = "&No";
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(12, 3);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(213, 86);
            this.containerWithShadow1.TabIndex = 6;
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(12, 91);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(213, 45);
            this.containerWithShadow2.TabIndex = 7;
            // 
            // frmMsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(242, 140);
            this.ControlBox = false;
            this.Controls.Add(this.picCritical);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.picInfo);
            this.Controls.Add(this.picQuestion);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.containerWithShadow1);
            this.Controls.Add(this.containerWithShadow2);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MaximizeBox = false;
            this.Name = "frmMsgBox";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Message Box";
            this.Load += new System.EventHandler(this.frmMsgBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCritical)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picQuestion)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.PictureBox picQuestion;
        private System.Windows.Forms.PictureBox picCritical;
        private System.Windows.Forms.PictureBox picInfo;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnYes;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnNo;
    }
}

