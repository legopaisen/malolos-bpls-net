namespace QRCODE
{
    partial class frmQR
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
            this.txtSample = new System.Windows.Forms.TextBox();
            this.pBoxQR = new System.Windows.Forms.PictureBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxQR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSample
            // 
            this.txtSample.Location = new System.Drawing.Point(12, 12);
            this.txtSample.Multiline = true;
            this.txtSample.Name = "txtSample";
            this.txtSample.Size = new System.Drawing.Size(346, 357);
            this.txtSample.TabIndex = 0;
            this.txtSample.TextChanged += new System.EventHandler(this.txtSample_TextChanged);
            // 
            // pBoxQR
            // 
            this.pBoxQR.Location = new System.Drawing.Point(364, 12);
            this.pBoxQR.Name = "pBoxQR";
            this.pBoxQR.Size = new System.Drawing.Size(346, 357);
            this.pBoxQR.TabIndex = 1;
            this.pBoxQR.TabStop = false;
            this.pBoxQR.Click += new System.EventHandler(this.pBoxQR_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(12, 384);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(346, 51);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(364, 384);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(346, 51);
            this.button2.TabIndex = 2;
            this.button2.Text = "Decode";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(740, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(346, 357);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // frmQR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(120, 7);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.pBoxQR);
            this.Controls.Add(this.txtSample);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmQR";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "QRCode";
            this.Load += new System.EventHandler(this.frmQR_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pBoxQR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSample;
        private System.Windows.Forms.PictureBox pBoxQR;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

