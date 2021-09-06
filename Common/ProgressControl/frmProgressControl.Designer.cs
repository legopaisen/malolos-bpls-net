namespace Amellar.Common.ProgressControl
{
    partial class frmProgressControl
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
            this.pBar = new System.Windows.Forms.ProgressBar();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.chkPrintLessee = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // pBar
            // 
            this.pBar.Location = new System.Drawing.Point(5, 27);
            this.pBar.Name = "pBar";
            this.pBar.Size = new System.Drawing.Size(214, 27);
            this.pBar.TabIndex = 0;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(12, 60);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(90, 25);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Values.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click_1);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(121, 60);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(90, 25);
            this.btnClose.TabIndex = 1;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkPrintLessee
            // 
            this.chkPrintLessee.AutoSize = true;
            this.chkPrintLessee.Location = new System.Drawing.Point(12, 4);
            this.chkPrintLessee.Name = "chkPrintLessee";
            this.chkPrintLessee.Size = new System.Drawing.Size(105, 17);
            this.chkPrintLessee.TabIndex = 2;
            this.chkPrintLessee.Text = "Print Lessee Info";
            this.chkPrintLessee.UseVisualStyleBackColor = true;
            // 
            // frmProgressControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 97);
            this.ControlBox = false;
            this.Controls.Add(this.chkPrintLessee);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.pBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProgressControl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generate Report";
            this.Load += new System.EventHandler(this.frmProgressControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ProgressBar pBar;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private System.Windows.Forms.CheckBox chkPrintLessee;

    }
}

