namespace Amellar.Modules.Utilities
{
    partial class frmRevYear
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvRevYear = new System.Windows.Forms.DataGridView();
            this.btnAdd = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblRevYear = new System.Windows.Forms.Label();
            this.txtRevYear = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOrdinance = new System.Windows.Forms.TextBox();
            this.btnTax = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFees = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnExempt = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDefault = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtRevYearTo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnEdit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow3 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRevYear)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvRevYear
            // 
            this.dgvRevYear.AllowUserToAddRows = false;
            this.dgvRevYear.AllowUserToDeleteRows = false;
            this.dgvRevYear.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRevYear.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRevYear.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRevYear.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRevYear.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvRevYear.Location = new System.Drawing.Point(27, 24);
            this.dgvRevYear.Name = "dgvRevYear";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRevYear.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRevYear.Size = new System.Drawing.Size(270, 95);
            this.dgvRevYear.TabIndex = 24;
            this.dgvRevYear.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRevYear_CellClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(27, 183);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnAdd.Size = new System.Drawing.Size(88, 25);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.Values.ExtraText = "";
            this.btnAdd.Values.Image = null;
            this.btnAdd.Values.ImageStates.ImageCheckedNormal = null;
            this.btnAdd.Values.ImageStates.ImageCheckedPressed = null;
            this.btnAdd.Values.ImageStates.ImageCheckedTracking = null;
            this.btnAdd.Values.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(207, 183);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(88, 25);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Close";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblRevYear
            // 
            this.lblRevYear.AutoSize = true;
            this.lblRevYear.Location = new System.Drawing.Point(26, 130);
            this.lblRevYear.Name = "lblRevYear";
            this.lblRevYear.Size = new System.Drawing.Size(78, 13);
            this.lblRevYear.TabIndex = 29;
            this.lblRevYear.Text = "Effectivity Year";
            // 
            // txtRevYear
            // 
            this.txtRevYear.Location = new System.Drawing.Point(108, 127);
            this.txtRevYear.MaxLength = 4;
            this.txtRevYear.Name = "txtRevYear";
            this.txtRevYear.ReadOnly = true;
            this.txtRevYear.Size = new System.Drawing.Size(60, 20);
            this.txtRevYear.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Ordinance";
            // 
            // txtOrdinance
            // 
            this.txtOrdinance.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtOrdinance.Location = new System.Drawing.Point(87, 153);
            this.txtOrdinance.Multiline = true;
            this.txtOrdinance.Name = "txtOrdinance";
            this.txtOrdinance.ReadOnly = true;
            this.txtOrdinance.Size = new System.Drawing.Size(210, 20);
            this.txtOrdinance.TabIndex = 4;
            // 
            // btnTax
            // 
            this.btnTax.Enabled = false;
            this.btnTax.Location = new System.Drawing.Point(23, 276);
            this.btnTax.Name = "btnTax";
            this.btnTax.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnTax.Size = new System.Drawing.Size(135, 25);
            this.btnTax.TabIndex = 5;
            this.btnTax.Text = "Taxes";
            this.btnTax.Values.ExtraText = "";
            this.btnTax.Values.Image = null;
            this.btnTax.Values.ImageStates.ImageCheckedNormal = null;
            this.btnTax.Values.ImageStates.ImageCheckedPressed = null;
            this.btnTax.Values.ImageStates.ImageCheckedTracking = null;
            this.btnTax.Values.Text = "Taxes";
            this.btnTax.Click += new System.EventHandler(this.btnTax_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 245);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(270, 16);
            this.label2.TabIndex = 34;
            this.label2.Text = "Preload initial values for the following:";
            // 
            // btnFees
            // 
            this.btnFees.Enabled = false;
            this.btnFees.Location = new System.Drawing.Point(23, 307);
            this.btnFees.Name = "btnFees";
            this.btnFees.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnFees.Size = new System.Drawing.Size(135, 25);
            this.btnFees.TabIndex = 6;
            this.btnFees.Text = "Fees && Other Charges";
            this.btnFees.Values.ExtraText = "";
            this.btnFees.Values.Image = null;
            this.btnFees.Values.ImageStates.ImageCheckedNormal = null;
            this.btnFees.Values.ImageStates.ImageCheckedPressed = null;
            this.btnFees.Values.ImageStates.ImageCheckedTracking = null;
            this.btnFees.Values.Text = "Fees && Other Charges";
            this.btnFees.Click += new System.EventHandler(this.btnFees_Click);
            // 
            // btnExempt
            // 
            this.btnExempt.Enabled = false;
            this.btnExempt.Location = new System.Drawing.Point(164, 276);
            this.btnExempt.Name = "btnExempt";
            this.btnExempt.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnExempt.Size = new System.Drawing.Size(135, 25);
            this.btnExempt.TabIndex = 7;
            this.btnExempt.Text = "Exemption";
            this.btnExempt.Values.ExtraText = "";
            this.btnExempt.Values.Image = null;
            this.btnExempt.Values.ImageStates.ImageCheckedNormal = null;
            this.btnExempt.Values.ImageStates.ImageCheckedPressed = null;
            this.btnExempt.Values.ImageStates.ImageCheckedTracking = null;
            this.btnExempt.Values.Text = "Exemption";
            this.btnExempt.Click += new System.EventHandler(this.btnExempt_Click);
            // 
            // btnDefault
            // 
            this.btnDefault.Enabled = false;
            this.btnDefault.Location = new System.Drawing.Point(164, 307);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnDefault.Size = new System.Drawing.Size(135, 25);
            this.btnDefault.TabIndex = 8;
            this.btnDefault.Text = "Set Default";
            this.btnDefault.Values.ExtraText = "";
            this.btnDefault.Values.Image = null;
            this.btnDefault.Values.ImageStates.ImageCheckedNormal = null;
            this.btnDefault.Values.ImageStates.ImageCheckedPressed = null;
            this.btnDefault.Values.ImageStates.ImageCheckedTracking = null;
            this.btnDefault.Values.Text = "Set Default";
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // txtRevYearTo
            // 
            this.txtRevYearTo.Location = new System.Drawing.Point(196, 127);
            this.txtRevYearTo.MaxLength = 4;
            this.txtRevYearTo.Name = "txtRevYearTo";
            this.txtRevYearTo.ReadOnly = true;
            this.txtRevYearTo.Size = new System.Drawing.Size(60, 20);
            this.txtRevYearTo.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(174, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = "to";
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(117, 183);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnEdit.Size = new System.Drawing.Size(88, 25);
            this.btnEdit.TabIndex = 5;
            this.btnEdit.Text = "Edit";
            this.btnEdit.Values.ExtraText = "";
            this.btnEdit.Values.Image = null;
            this.btnEdit.Values.ImageStates.ImageCheckedNormal = null;
            this.btnEdit.Values.ImageStates.ImageCheckedPressed = null;
            this.btnEdit.Values.ImageStates.ImageCheckedTracking = null;
            this.btnEdit.Values.Text = "Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(12, 228);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(296, 129);
            this.containerWithShadow1.TabIndex = 25;
            // 
            // containerWithShadow3
            // 
            this.containerWithShadow3.Location = new System.Drawing.Point(12, 12);
            this.containerWithShadow3.Name = "containerWithShadow3";
            this.containerWithShadow3.Size = new System.Drawing.Size(296, 210);
            this.containerWithShadow3.TabIndex = 20;
            // 
            // frmRevYear
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(320, 367);
            this.ControlBox = false;
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRevYearTo);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.btnExempt);
            this.Controls.Add(this.btnFees);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnTax);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOrdinance);
            this.Controls.Add(this.lblRevYear);
            this.Controls.Add(this.txtRevYear);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.containerWithShadow1);
            this.Controls.Add(this.dgvRevYear);
            this.Controls.Add(this.containerWithShadow3);
            this.Name = "frmRevYear";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Revenue Year";
            this.Load += new System.EventHandler(this.frmRevYear_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRevYear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow3;
        private System.Windows.Forms.DataGridView dgvRevYear;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAdd;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private System.Windows.Forms.Label lblRevYear;
        private System.Windows.Forms.TextBox txtRevYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOrdinance;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnTax;
        private System.Windows.Forms.Label label2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnFees;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnExempt;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDefault;
        private System.Windows.Forms.TextBox txtRevYearTo;
        private System.Windows.Forms.Label label3;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEdit;

    }
}