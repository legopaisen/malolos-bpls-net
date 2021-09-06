namespace Amellar.Common.Reports
{
    partial class frmInspector2
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
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbInspector = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpDateInspected = new System.Windows.Forms.DateTimePicker();
            this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.SuspendLayout();
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(13, 12);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(340, 103);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Inspector:";
            // 
            // cmbInspector
            // 
            this.cmbInspector.FormattingEnabled = true;
            this.cmbInspector.Location = new System.Drawing.Point(120, 30);
            this.cmbInspector.Name = "cmbInspector";
            this.cmbInspector.Size = new System.Drawing.Size(220, 21);
            this.cmbInspector.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Date Inspected:";
            // 
            // dtpDateInspected
            // 
            this.dtpDateInspected.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateInspected.Location = new System.Drawing.Point(120, 65);
            this.dtpDateInspected.Name = "dtpDateInspected";
            this.dtpDateInspected.Size = new System.Drawing.Size(89, 20);
            this.dtpDateInspected.TabIndex = 4;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(246, 65);
            this.btnOk.Name = "btnOk";
            this.btnOk.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOk.Size = new System.Drawing.Size(94, 26);
            this.btnOk.TabIndex = 5;
            this.btnOk.Values.Text = "Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmInspector2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 138);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.dtpDateInspected);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbInspector);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.containerWithShadow1);
            this.Name = "frmInspector2";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inspector Details";
            this.Load += new System.EventHandler(this.frmInspector_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        public System.Windows.Forms.ComboBox cmbInspector;
        public System.Windows.Forms.DateTimePicker dtpDateInspected;
    }
}