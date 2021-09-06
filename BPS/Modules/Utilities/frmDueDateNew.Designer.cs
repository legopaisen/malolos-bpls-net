namespace Amellar.Modules.Utilities
{
    partial class frmDueDateNew
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
            this.dtpJan = new System.Windows.Forms.DateTimePicker();
            this.lbl1 = new System.Windows.Forms.Label();
            this.dtpJul = new System.Windows.Forms.DateTimePicker();
            this.lbl2 = new System.Windows.Forms.Label();
            this.dtpApr = new System.Windows.Forms.DateTimePicker();
            this.dtpOct = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtYear = new System.Windows.Forms.TextBox();
            this.btnView = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEdit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.containerWithShadow6 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtDay = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdo3 = new System.Windows.Forms.RadioButton();
            this.rdo2 = new System.Windows.Forms.RadioButton();
            this.rdo1 = new System.Windows.Forms.RadioButton();
            this.btnEditSur = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtpJan
            // 
            this.dtpJan.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpJan.Location = new System.Drawing.Point(142, 24);
            this.dtpJan.Name = "dtpJan";
            this.dtpJan.Size = new System.Drawing.Size(101, 20);
            this.dtpJan.TabIndex = 3;
            this.dtpJan.ValueChanged += new System.EventHandler(this.dtpJan_ValueChanged);
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl1.Location = new System.Drawing.Point(31, 30);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(64, 13);
            this.lbl1.TabIndex = 5;
            this.lbl1.Text = "First Quarter";
            // 
            // dtpJul
            // 
            this.dtpJul.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpJul.Location = new System.Drawing.Point(142, 76);
            this.dtpJul.Name = "dtpJul";
            this.dtpJul.Size = new System.Drawing.Size(101, 20);
            this.dtpJul.TabIndex = 9;
            this.dtpJul.ValueChanged += new System.EventHandler(this.dtpJul_ValueChanged);
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2.Location = new System.Drawing.Point(31, 82);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(69, 13);
            this.lbl2.TabIndex = 5;
            this.lbl2.Text = "Third Quarter";
            // 
            // dtpApr
            // 
            this.dtpApr.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpApr.Location = new System.Drawing.Point(142, 50);
            this.dtpApr.Name = "dtpApr";
            this.dtpApr.Size = new System.Drawing.Size(101, 20);
            this.dtpApr.TabIndex = 6;
            this.dtpApr.ValueChanged += new System.EventHandler(this.dtpApr_ValueChanged);
            // 
            // dtpOct
            // 
            this.dtpOct.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpOct.Location = new System.Drawing.Point(142, 102);
            this.dtpOct.Name = "dtpOct";
            this.dtpOct.Size = new System.Drawing.Size(101, 20);
            this.dtpOct.TabIndex = 12;
            this.dtpOct.ValueChanged += new System.EventHandler(this.dtpOct_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(31, 107);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Fourth Quarter";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(32, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "YEAR";
            // 
            // txtYear
            // 
            this.txtYear.Location = new System.Drawing.Point(76, 25);
            this.txtYear.MaxLength = 4;
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(72, 20);
            this.txtYear.TabIndex = 1;
            this.txtYear.Leave += new System.EventHandler(this.txtYear_Leave);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(174, 20);
            this.btnView.Name = "btnView";
            this.btnView.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnView.Size = new System.Drawing.Size(81, 25);
            this.btnView.TabIndex = 2;
            this.btnView.Text = "View";
            this.btnView.Values.ExtraText = "";
            this.btnView.Values.Image = null;
            this.btnView.Values.ImageStates.ImageCheckedNormal = null;
            this.btnView.Values.ImageStates.ImageCheckedPressed = null;
            this.btnView.Values.ImageStates.ImageCheckedTracking = null;
            this.btnView.Values.Text = "View";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(162, 155);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnEdit.Size = new System.Drawing.Size(81, 25);
            this.btnEdit.TabIndex = 15;
            this.btnEdit.Text = "Edit";
            this.btnEdit.Values.ExtraText = "";
            this.btnEdit.Values.Image = null;
            this.btnEdit.Values.ImageStates.ImageCheckedNormal = null;
            this.btnEdit.Values.ImageStates.ImageCheckedPressed = null;
            this.btnEdit.Values.ImageStates.ImageCheckedTracking = null;
            this.btnEdit.Values.Text = "Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(174, 403);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(81, 25);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // containerWithShadow6
            // 
            this.containerWithShadow6.Location = new System.Drawing.Point(12, 12);
            this.containerWithShadow6.Name = "containerWithShadow6";
            this.containerWithShadow6.Size = new System.Drawing.Size(267, 54);
            this.containerWithShadow6.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtDay);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lbl1);
            this.groupBox1.Controls.Add(this.dtpJan);
            this.groupBox1.Controls.Add(this.btnEdit);
            this.groupBox1.Controls.Add(this.lbl2);
            this.groupBox1.Controls.Add(this.dtpJul);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dtpApr);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.dtpOct);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(267, 188);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Quarter Due Dates  (Interest Computation) ";
            // 
            // txtDay
            // 
            this.txtDay.Location = new System.Drawing.Point(142, 129);
            this.txtDay.MaxLength = 4;
            this.txtDay.Name = "txtDay";
            this.txtDay.Size = new System.Drawing.Size(101, 20);
            this.txtDay.TabIndex = 24;
            this.txtDay.Leave += new System.EventHandler(this.txtDay_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(31, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Monthly cut-off (Day)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(31, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Second Quarter";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdo3);
            this.groupBox2.Controls.Add(this.rdo2);
            this.groupBox2.Controls.Add(this.rdo1);
            this.groupBox2.Controls.Add(this.btnEditSur);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 266);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(267, 131);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Surcharge Computation ";
            // 
            // rdo3
            // 
            this.rdo3.AutoSize = true;
            this.rdo3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdo3.Location = new System.Drawing.Point(34, 73);
            this.rdo3.Name = "rdo3";
            this.rdo3.Size = new System.Drawing.Size(146, 17);
            this.rdo3.TabIndex = 37;
            this.rdo3.TabStop = true;
            this.rdo3.Text = "Based on Remaining Due";
            this.rdo3.UseVisualStyleBackColor = true;
            this.rdo3.CheckedChanged += new System.EventHandler(this.rdo3_CheckedChanged);
            // 
            // rdo2
            // 
            this.rdo2.AutoSize = true;
            this.rdo2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdo2.Location = new System.Drawing.Point(34, 49);
            this.rdo2.Name = "rdo2";
            this.rdo2.Size = new System.Drawing.Size(137, 17);
            this.rdo2.TabIndex = 36;
            this.rdo2.TabStop = true;
            this.rdo2.Text = "Based on Full Year Due";
            this.rdo2.UseVisualStyleBackColor = true;
            this.rdo2.CheckedChanged += new System.EventHandler(this.rdo2_CheckedChanged);
            // 
            // rdo1
            // 
            this.rdo1.AutoSize = true;
            this.rdo1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdo1.Location = new System.Drawing.Point(34, 26);
            this.rdo1.Name = "rdo1";
            this.rdo1.Size = new System.Drawing.Size(131, 17);
            this.rdo1.TabIndex = 35;
            this.rdo1.TabStop = true;
            this.rdo1.Text = "Based on Quarter Due";
            this.rdo1.UseVisualStyleBackColor = true;
            this.rdo1.CheckedChanged += new System.EventHandler(this.rdo1_CheckedChanged);
            // 
            // btnEditSur
            // 
            this.btnEditSur.Location = new System.Drawing.Point(162, 96);
            this.btnEditSur.Name = "btnEditSur";
            this.btnEditSur.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnEditSur.Size = new System.Drawing.Size(81, 25);
            this.btnEditSur.TabIndex = 15;
            this.btnEditSur.Text = "Edit";
            this.btnEditSur.Values.ExtraText = "";
            this.btnEditSur.Values.Image = null;
            this.btnEditSur.Values.ImageStates.ImageCheckedNormal = null;
            this.btnEditSur.Values.ImageStates.ImageCheckedPressed = null;
            this.btnEditSur.Values.ImageStates.ImageCheckedTracking = null;
            this.btnEditSur.Values.Text = "Edit";
            this.btnEditSur.Visible = false;
            this.btnEditSur.Click += new System.EventHandler(this.btnEditSur_Click);
            // 
            // frmDueDateNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(293, 436);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.txtYear);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.containerWithShadow6);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDueDateNew";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Penalty Configuration";
            this.Load += new System.EventHandler(this.frmDueDate_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpJan;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.DateTimePicker dtpJul;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.DateTimePicker dtpApr;
        private System.Windows.Forms.DateTimePicker dtpOct;
        private System.Windows.Forms.Label label8;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtYear;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnView;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEdit;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdo3;
        private System.Windows.Forms.RadioButton rdo2;
        private System.Windows.Forms.RadioButton rdo1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEditSur;
        private System.Windows.Forms.TextBox txtDay;
        private System.Windows.Forms.Label label1;
    }
}