namespace Amellar.Modules.Payment
{
    partial class frmPayorInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPayorInfo));
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSave = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonHeader5 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFN = new System.Windows.Forms.TextBox();
            this.txtLN = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDist = new System.Windows.Forms.TextBox();
            this.txtProv = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtBrgy = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtStreet = new System.Windows.Forms.TextBox();
            this.txtZone = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMunCity = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMI = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(441, 266);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(62, 25);
            this.btnCancel.TabIndex = 145;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(373, 266);
            this.btnSave.Name = "btnSave";
            this.btnSave.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSave.Size = new System.Drawing.Size(62, 25);
            this.btnSave.TabIndex = 146;
            this.btnSave.Values.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // kryptonHeader5
            // 
            this.kryptonHeader5.AutoSize = false;
            this.kryptonHeader5.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader5.Location = new System.Drawing.Point(5, 5);
            this.kryptonHeader5.Name = "kryptonHeader5";
            this.kryptonHeader5.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader5.Size = new System.Drawing.Size(498, 24);
            this.kryptonHeader5.TabIndex = 143;
            this.kryptonHeader5.Values.Description = "";
            this.kryptonHeader5.Values.Heading = "";
            this.kryptonHeader5.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader5.Values.Image")));
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(5, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(498, 256);
            this.label4.TabIndex = 144;
            // 
            // txtFN
            // 
            this.txtFN.Location = new System.Drawing.Point(82, 60);
            this.txtFN.Name = "txtFN";
            this.txtFN.Size = new System.Drawing.Size(328, 20);
            this.txtFN.TabIndex = 149;
            // 
            // txtLN
            // 
            this.txtLN.Location = new System.Drawing.Point(82, 36);
            this.txtLN.Name = "txtLN";
            this.txtLN.Size = new System.Drawing.Size(404, 20);
            this.txtLN.TabIndex = 150;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(25, 64);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 13);
            this.label12.TabIndex = 147;
            this.label12.Text = "First Name";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(24, 40);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 148;
            this.label11.Text = "Last Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 148;
            this.label1.Text = "District";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 232);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 147;
            this.label2.Text = "Province";
            // 
            // txtDist
            // 
            this.txtDist.Location = new System.Drawing.Point(82, 132);
            this.txtDist.Name = "txtDist";
            this.txtDist.Size = new System.Drawing.Size(404, 20);
            this.txtDist.TabIndex = 150;
            // 
            // txtProv
            // 
            this.txtProv.Location = new System.Drawing.Point(82, 228);
            this.txtProv.Name = "txtProv";
            this.txtProv.Size = new System.Drawing.Size(404, 20);
            this.txtProv.TabIndex = 149;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 148;
            this.label3.Text = "Address";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 147;
            this.label5.Text = "Barangay";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(82, 84);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(404, 20);
            this.txtAddress.TabIndex = 150;
            // 
            // txtBrgy
            // 
            this.txtBrgy.Location = new System.Drawing.Point(82, 108);
            this.txtBrgy.Name = "txtBrgy";
            this.txtBrgy.Size = new System.Drawing.Size(404, 20);
            this.txtBrgy.TabIndex = 149;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(47, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 148;
            this.label6.Text = "Street";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(47, 184);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 147;
            this.label7.Text = "Zone";
            // 
            // txtStreet
            // 
            this.txtStreet.Location = new System.Drawing.Point(82, 156);
            this.txtStreet.Name = "txtStreet";
            this.txtStreet.Size = new System.Drawing.Size(404, 20);
            this.txtStreet.TabIndex = 150;
            // 
            // txtZone
            // 
            this.txtZone.Location = new System.Drawing.Point(82, 180);
            this.txtZone.Name = "txtZone";
            this.txtZone.Size = new System.Drawing.Size(404, 20);
            this.txtZone.TabIndex = 149;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(30, 208);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 148;
            this.label8.Text = "Mun/City";
            // 
            // txtMunCity
            // 
            this.txtMunCity.Location = new System.Drawing.Point(82, 204);
            this.txtMunCity.Name = "txtMunCity";
            this.txtMunCity.Size = new System.Drawing.Size(404, 20);
            this.txtMunCity.TabIndex = 150;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(416, 62);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 13);
            this.label9.TabIndex = 148;
            this.label9.Text = "MI";
            // 
            // txtMI
            // 
            this.txtMI.Location = new System.Drawing.Point(441, 58);
            this.txtMI.Name = "txtMI";
            this.txtMI.Size = new System.Drawing.Size(45, 20);
            this.txtMI.TabIndex = 150;
            // 
            // frmPayorInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 297);
            this.Controls.Add(this.txtZone);
            this.Controls.Add(this.txtMunCity);
            this.Controls.Add(this.txtBrgy);
            this.Controls.Add(this.txtMI);
            this.Controls.Add(this.txtStreet);
            this.Controls.Add(this.txtProv);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtDist);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtFN);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLN);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.kryptonHeader5);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(515, 325);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(515, 325);
            this.Name = "frmPayorInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Payor Account Info";
            this.Load += new System.EventHandler(this.frmPayorInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSave;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFN;
        private System.Windows.Forms.TextBox txtLN;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDist;
        private System.Windows.Forms.TextBox txtProv;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtBrgy;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtStreet;
        private System.Windows.Forms.TextBox txtZone;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtMunCity;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMI;
    }
}