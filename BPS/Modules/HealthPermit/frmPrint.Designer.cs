
namespace Amellar.Modules.HealthPermit
{
    partial class frmPrint
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
            this.btnHealth = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnWork = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.SuspendLayout();
            // 
            // btnHealth
            // 
            this.btnHealth.Location = new System.Drawing.Point(21, 33);
            this.btnHealth.Name = "btnHealth";
            this.btnHealth.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnHealth.Size = new System.Drawing.Size(123, 25);
            this.btnHealth.TabIndex = 4;
            this.btnHealth.Text = "Health Certificate";
            this.btnHealth.Values.ExtraText = "";
            this.btnHealth.Values.Image = null;
            this.btnHealth.Values.ImageStates.ImageCheckedNormal = null;
            this.btnHealth.Values.ImageStates.ImageCheckedPressed = null;
            this.btnHealth.Values.ImageStates.ImageCheckedTracking = null;
            this.btnHealth.Values.Text = "Health Certificate";
            this.btnHealth.Click += new System.EventHandler(this.btnHealth_Click);
            // 
            // btnWork
            // 
            this.btnWork.Location = new System.Drawing.Point(161, 33);
            this.btnWork.Name = "btnWork";
            this.btnWork.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnWork.Size = new System.Drawing.Size(123, 25);
            this.btnWork.TabIndex = 5;
            this.btnWork.Text = "Work Permit";
            this.btnWork.Values.ExtraText = "";
            this.btnWork.Values.Image = null;
            this.btnWork.Values.ImageStates.ImageCheckedNormal = null;
            this.btnWork.Values.ImageStates.ImageCheckedPressed = null;
            this.btnWork.Values.ImageStates.ImageCheckedTracking = null;
            this.btnWork.Values.Text = "Work Permit";
            this.btnWork.Click += new System.EventHandler(this.btnWork_Click);
            // 
            // frmPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 88);
            this.Controls.Add(this.btnWork);
            this.Controls.Add(this.btnHealth);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPrint";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Print Record";
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnHealth;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnWork;
    }
}