namespace Amellar.Modules.EPS
{
    partial class frmViewParticulars
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
            this.dgvParticulars = new System.Windows.Forms.DataGridView();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParticulars)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvParticulars
            // 
            this.dgvParticulars.AllowUserToAddRows = false;
            this.dgvParticulars.AllowUserToDeleteRows = false;
            this.dgvParticulars.AllowUserToResizeRows = false;
            this.dgvParticulars.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParticulars.Location = new System.Drawing.Point(12, 12);
            this.dgvParticulars.Name = "dgvParticulars";
            this.dgvParticulars.RowHeadersVisible = false;
            this.dgvParticulars.Size = new System.Drawing.Size(328, 319);
            this.dgvParticulars.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(252, 337);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(90, 25);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmViewParticulars
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(354, 373);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvParticulars);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmViewParticulars";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmViewParticulars";
            this.Load += new System.EventHandler(this.frmViewParticulars_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvParticulars)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvParticulars;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
    }
}