namespace NewPayment
{
    partial class QtrSelect
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
            this.frameWithShadow1 = new Amellar.Common.SOA.FrameWithShadow();
            this.trvBin = new System.Windows.Forms.TreeView();
            this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.SuspendLayout();
            // 
            // frameWithShadow1
            // 
            this.frameWithShadow1.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow1.Location = new System.Drawing.Point(8, 14);
            this.frameWithShadow1.Name = "frameWithShadow1";
            this.frameWithShadow1.Size = new System.Drawing.Size(738, 198);
            this.frameWithShadow1.TabIndex = 0;
            // 
            // trvBin
            // 
            this.trvBin.Location = new System.Drawing.Point(27, 32);
            this.trvBin.Name = "trvBin";
            this.trvBin.Size = new System.Drawing.Size(701, 150);
            this.trvBin.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(636, 226);
            this.btnOk.Name = "btnOk";
            this.btnOk.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOk.Size = new System.Drawing.Size(92, 35);
            this.btnOk.TabIndex = 2;
            this.btnOk.Values.Text = "Ok";
            // 
            // QtrSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 273);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.trvBin);
            this.Controls.Add(this.frameWithShadow1);
            this.Name = "QtrSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Quarter/s to be paid";
            this.Load += new System.EventHandler(this.QtrSelect_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Amellar.Common.SOA.FrameWithShadow frameWithShadow1;
        private System.Windows.Forms.TreeView trvBin;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
    }
}