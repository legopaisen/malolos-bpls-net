namespace Amellar.Modules.Payment
{
    partial class frmAdditionalPayment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdditionalPayment));
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonHeader5 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.label4 = new System.Windows.Forms.Label();
            this.rdoCash = new System.Windows.Forms.RadioButton();
            this.rdoCheck = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(126, 135);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(62, 25);
            this.btnCancel.TabIndex = 145;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(58, 135);
            this.btnOk.Name = "btnOk";
            this.btnOk.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOk.Size = new System.Drawing.Size(62, 25);
            this.btnOk.TabIndex = 146;
            this.btnOk.Values.Text = "&Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // kryptonHeader5
            // 
            this.kryptonHeader5.AutoSize = false;
            this.kryptonHeader5.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader5.Location = new System.Drawing.Point(6, 5);
            this.kryptonHeader5.Name = "kryptonHeader5";
            this.kryptonHeader5.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader5.Size = new System.Drawing.Size(182, 24);
            this.kryptonHeader5.TabIndex = 143;
            this.kryptonHeader5.Values.Description = "";
            this.kryptonHeader5.Values.Heading = "";
            this.kryptonHeader5.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader5.Values.Image")));
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(6, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(182, 124);
            this.label4.TabIndex = 144;
            // 
            // rdoCash
            // 
            this.rdoCash.AutoSize = true;
            this.rdoCash.Location = new System.Drawing.Point(44, 78);
            this.rdoCash.Name = "rdoCash";
            this.rdoCash.Size = new System.Drawing.Size(93, 17);
            this.rdoCash.TabIndex = 147;
            this.rdoCash.TabStop = true;
            this.rdoCash.Text = "Cash Payment";
            this.rdoCash.UseVisualStyleBackColor = true;
            this.rdoCash.Click += new System.EventHandler(this.rdoCash_Click);
            // 
            // rdoCheck
            // 
            this.rdoCheck.AutoSize = true;
            this.rdoCheck.Location = new System.Drawing.Point(44, 101);
            this.rdoCheck.Name = "rdoCheck";
            this.rdoCheck.Size = new System.Drawing.Size(100, 17);
            this.rdoCheck.TabIndex = 147;
            this.rdoCheck.TabStop = true;
            this.rdoCheck.Text = "Check Payment";
            this.rdoCheck.UseVisualStyleBackColor = true;
            this.rdoCheck.Click += new System.EventHandler(this.rdoCheck_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(28, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 32);
            this.label1.TabIndex = 148;
            this.label1.Text = "Choose Additional\r\nPayment";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmAdditionalPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(194, 166);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rdoCheck);
            this.Controls.Add(this.rdoCash);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.kryptonHeader5);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(200, 194);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 194);
            this.Name = "frmAdditionalPayment";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Additional Payment";
            this.Load += new System.EventHandler(this.frmAdditionalPayment_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rdoCash;
        private System.Windows.Forms.RadioButton rdoCheck;
        private System.Windows.Forms.Label label1;
    }
}