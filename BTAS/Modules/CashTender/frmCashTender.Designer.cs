namespace CashTender
{
    partial class frmCashTender
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
            this.lblDueAmount = new System.Windows.Forms.Label();
            this.kryptonHeader5 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCheckAmount = new System.Windows.Forms.TextBox();
            this.txtCashAmount = new System.Windows.Forms.TextBox();
            this.txtBalance = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCashTendered = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.label6 = new System.Windows.Forms.Label();
            this.lblChange = new System.Windows.Forms.Label();
            this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow3 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow4 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow5 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.SuspendLayout();
            // 
            // lblDueAmount
            // 
            this.lblDueAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblDueAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDueAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDueAmount.ForeColor = System.Drawing.Color.Blue;
            this.lblDueAmount.Location = new System.Drawing.Point(21, 51);
            this.lblDueAmount.Name = "lblDueAmount";
            this.lblDueAmount.Size = new System.Drawing.Size(353, 45);
            this.lblDueAmount.TabIndex = 1;
            this.lblDueAmount.Text = "label1";
            this.lblDueAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // kryptonHeader5
            // 
            this.kryptonHeader5.AutoSize = false;
            this.kryptonHeader5.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader5.Location = new System.Drawing.Point(21, 21);
            this.kryptonHeader5.Name = "kryptonHeader5";
            this.kryptonHeader5.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonHeader5.Size = new System.Drawing.Size(353, 22);
            this.kryptonHeader5.TabIndex = 72;
            this.kryptonHeader5.Values.Description = "";
            this.kryptonHeader5.Values.Heading = "Amount Due";
            this.kryptonHeader5.Values.Image = null;
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader1.Location = new System.Drawing.Point(21, 120);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonHeader1.Size = new System.Drawing.Size(353, 22);
            this.kryptonHeader1.TabIndex = 74;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Cash / Check Information";
            this.kryptonHeader1.Values.Image = null;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(16, 155);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 25);
            this.label2.TabIndex = 75;
            this.label2.Text = "Check Amount:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(16, 193);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 25);
            this.label3.TabIndex = 76;
            this.label3.Text = "Cash Amount:";
            // 
            // txtCheckAmount
            // 
            this.txtCheckAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCheckAmount.Location = new System.Drawing.Point(180, 148);
            this.txtCheckAmount.Name = "txtCheckAmount";
            this.txtCheckAmount.ReadOnly = true;
            this.txtCheckAmount.Size = new System.Drawing.Size(194, 38);
            this.txtCheckAmount.TabIndex = 77;
            this.txtCheckAmount.Text = "0.00";
            this.txtCheckAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtCashAmount
            // 
            this.txtCashAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCashAmount.Location = new System.Drawing.Point(180, 192);
            this.txtCashAmount.Name = "txtCashAmount";
            this.txtCashAmount.ReadOnly = true;
            this.txtCashAmount.Size = new System.Drawing.Size(194, 38);
            this.txtCashAmount.TabIndex = 78;
            this.txtCashAmount.Text = "0.00";
            this.txtCashAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtBalance
            // 
            this.txtBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBalance.Location = new System.Drawing.Point(180, 253);
            this.txtBalance.Name = "txtBalance";
            this.txtBalance.ReadOnly = true;
            this.txtBalance.Size = new System.Drawing.Size(194, 38);
            this.txtBalance.TabIndex = 80;
            this.txtBalance.Text = "0.00";
            this.txtBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Navy;
            this.label4.Location = new System.Drawing.Point(16, 260);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 25);
            this.label4.TabIndex = 79;
            this.label4.Text = "Balance:";
            // 
            // txtCashTendered
            // 
            this.txtCashTendered.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCashTendered.Location = new System.Drawing.Point(180, 345);
            this.txtCashTendered.Name = "txtCashTendered";
            this.txtCashTendered.Size = new System.Drawing.Size(194, 38);
            this.txtCashTendered.TabIndex = 83;
            this.txtCashTendered.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCashTendered.TextChanged += new System.EventHandler(this.txtCashTendered_TextChanged);
            this.txtCashTendered.Leave += new System.EventHandler(this.txtCashTendered_Leave);
            this.txtCashTendered.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCashTendered_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Navy;
            this.label5.Location = new System.Drawing.Point(16, 352);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 25);
            this.label5.TabIndex = 82;
            this.label5.Text = "Amount:";
            // 
            // kryptonHeader2
            // 
            this.kryptonHeader2.AutoSize = false;
            this.kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader2.Location = new System.Drawing.Point(21, 317);
            this.kryptonHeader2.Name = "kryptonHeader2";
            this.kryptonHeader2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonHeader2.Size = new System.Drawing.Size(353, 22);
            this.kryptonHeader2.TabIndex = 85;
            this.kryptonHeader2.Values.Description = "";
            this.kryptonHeader2.Values.Heading = "Cash Tendered";
            this.kryptonHeader2.Values.Image = null;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Navy;
            this.label6.Location = new System.Drawing.Point(19, 414);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 25);
            this.label6.TabIndex = 86;
            this.label6.Text = "Change:";
            // 
            // lblChange
            // 
            this.lblChange.BackColor = System.Drawing.Color.Transparent;
            this.lblChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChange.ForeColor = System.Drawing.Color.Red;
            this.lblChange.Location = new System.Drawing.Point(137, 414);
            this.lblChange.Name = "lblChange";
            this.lblChange.Size = new System.Drawing.Size(237, 25);
            this.lblChange.TabIndex = 89;
            this.lblChange.Text = "0.00";
            this.lblChange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(198, 460);
            this.btnOk.Name = "btnOk";
            this.btnOk.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOk.Size = new System.Drawing.Size(85, 26);
            this.btnOk.TabIndex = 90;
            this.btnOk.Values.Text = "Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(289, 460);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(85, 26);
            this.btnCancel.TabIndex = 91;
            this.btnCancel.Values.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(10, 10);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(373, 102);
            this.containerWithShadow1.TabIndex = 92;
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(10, 109);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(373, 136);
            this.containerWithShadow2.TabIndex = 92;
            // 
            // containerWithShadow3
            // 
            this.containerWithShadow3.Location = new System.Drawing.Point(10, 242);
            this.containerWithShadow3.Name = "containerWithShadow3";
            this.containerWithShadow3.Size = new System.Drawing.Size(373, 66);
            this.containerWithShadow3.TabIndex = 93;
            // 
            // containerWithShadow4
            // 
            this.containerWithShadow4.Location = new System.Drawing.Point(10, 305);
            this.containerWithShadow4.Name = "containerWithShadow4";
            this.containerWithShadow4.Size = new System.Drawing.Size(373, 96);
            this.containerWithShadow4.TabIndex = 94;
            // 
            // containerWithShadow5
            // 
            this.containerWithShadow5.Location = new System.Drawing.Point(12, 404);
            this.containerWithShadow5.Name = "containerWithShadow5";
            this.containerWithShadow5.Size = new System.Drawing.Size(373, 50);
            this.containerWithShadow5.TabIndex = 95;
            // 
            // frmCashTender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(395, 495);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblChange);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.kryptonHeader2);
            this.Controls.Add(this.txtCashTendered);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBalance);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCashAmount);
            this.Controls.Add(this.txtCheckAmount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.kryptonHeader5);
            this.Controls.Add(this.lblDueAmount);
            this.Controls.Add(this.containerWithShadow2);
            this.Controls.Add(this.containerWithShadow1);
            this.Controls.Add(this.containerWithShadow3);
            this.Controls.Add(this.containerWithShadow4);
            this.Controls.Add(this.containerWithShadow5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(411, 533);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(411, 533);
            this.Name = "frmCashTender";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cash Tender";
            this.Load += new System.EventHandler(this.frmCashTender_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader5;
        public ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        public ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader2;
        private System.Windows.Forms.Label label6;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        public System.Windows.Forms.Label lblDueAmount;
        public System.Windows.Forms.TextBox txtCheckAmount;
        public System.Windows.Forms.TextBox txtCashAmount;
        public System.Windows.Forms.TextBox txtBalance;
        public System.Windows.Forms.Label lblChange;
        public System.Windows.Forms.TextBox txtCashTendered;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow3;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow4;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow5;
    }
}

