namespace Amellar.Modules.Retirement
{
    partial class frmRetirement
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.txtBnsAdd = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtAppNo = new System.Windows.Forms.TextBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.chkFull = new System.Windows.Forms.CheckBox();
            this.chkPartial = new System.Windows.Forms.CheckBox();
            this.chkBilling = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkWaiveTaxAndFees = new System.Windows.Forms.CheckBox();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.bin2 = new Amellar.Common.BIN.BIN();
            this.btnCancelApp = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSave = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.btnRollBack = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPrint = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "BIN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Business Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Business Address";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Tax Payer\'s Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Date Closed:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(277, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Application No.";
            // 
            // txtBnsName
            // 
            this.txtBnsName.Location = new System.Drawing.Point(121, 63);
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(322, 20);
            this.txtBnsName.TabIndex = 8;
            // 
            // txtBnsAdd
            // 
            this.txtBnsAdd.Location = new System.Drawing.Point(121, 86);
            this.txtBnsAdd.Name = "txtBnsAdd";
            this.txtBnsAdd.ReadOnly = true;
            this.txtBnsAdd.Size = new System.Drawing.Size(322, 20);
            this.txtBnsAdd.TabIndex = 9;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(121, 109);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(322, 20);
            this.txtName.TabIndex = 10;
            // 
            // txtAppNo
            // 
            this.txtAppNo.Location = new System.Drawing.Point(362, 40);
            this.txtAppNo.Name = "txtAppNo";
            this.txtAppNo.ReadOnly = true;
            this.txtAppNo.Size = new System.Drawing.Size(81, 20);
            this.txtAppNo.TabIndex = 3;
            this.txtAppNo.Leave += new System.EventHandler(this.txtAppNo_Leave);
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(121, 40);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(81, 20);
            this.dtpDate.TabIndex = 2;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(212, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(92, 25);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.Values.ExtraText = "";
            this.btnSearch.Values.Image = null;
            this.btnSearch.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearch.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearch.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvList.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvList.Location = new System.Drawing.Point(21, 158);
            this.dgvList.Name = "dgvList";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.Size = new System.Drawing.Size(425, 175);
            this.dgvList.TabIndex = 15;
            this.dgvList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellEndEdit);
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 373);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(116, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Reason for Retirement:";
            // 
            // txtReason
            // 
            this.txtReason.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtReason.Location = new System.Drawing.Point(21, 389);
            this.txtReason.MaxLength = 300;
            this.txtReason.Multiline = true;
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(283, 52);
            this.txtReason.TabIndex = 6;
            // 
            // chkFull
            // 
            this.chkFull.AutoSize = true;
            this.chkFull.Location = new System.Drawing.Point(141, 342);
            this.chkFull.Name = "chkFull";
            this.chkFull.Size = new System.Drawing.Size(42, 17);
            this.chkFull.TabIndex = 4;
            this.chkFull.Text = "Full";
            this.chkFull.UseVisualStyleBackColor = true;
            this.chkFull.CheckedChanged += new System.EventHandler(this.chkFull_CheckedChanged);
            // 
            // chkPartial
            // 
            this.chkPartial.AutoSize = true;
            this.chkPartial.Location = new System.Drawing.Point(189, 342);
            this.chkPartial.Name = "chkPartial";
            this.chkPartial.Size = new System.Drawing.Size(55, 17);
            this.chkPartial.TabIndex = 5;
            this.chkPartial.Text = "Partial";
            this.chkPartial.UseVisualStyleBackColor = true;
            this.chkPartial.CheckedChanged += new System.EventHandler(this.chkPartial_CheckedChanged);
            // 
            // chkBilling
            // 
            this.chkBilling.AutoSize = true;
            this.chkBilling.Location = new System.Drawing.Point(329, 343);
            this.chkBilling.Name = "chkBilling";
            this.chkBilling.Size = new System.Drawing.Size(71, 17);
            this.chkBilling.TabIndex = 7;
            this.chkBilling.Text = "For Billing";
            this.chkBilling.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 343);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Type of Retirement:";
            // 
            // chkWaiveTaxAndFees
            // 
            this.chkWaiveTaxAndFees.AutoSize = true;
            this.chkWaiveTaxAndFees.Enabled = false;
            this.chkWaiveTaxAndFees.Location = new System.Drawing.Point(329, 366);
            this.chkWaiveTaxAndFees.Name = "chkWaiveTaxAndFees";
            this.chkWaiveTaxAndFees.Size = new System.Drawing.Size(106, 17);
            this.chkWaiveTaxAndFees.TabIndex = 8;
            this.chkWaiveTaxAndFees.Text = "Waive Tax/Fees";
            this.chkWaiveTaxAndFees.UseVisualStyleBackColor = true;
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(4, 146);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(455, 344);
            this.containerWithShadow2.TabIndex = 14;
            // 
            // bin2
            // 
            this.bin2.GetBINSeries = "";
            this.bin2.GetDistCode = "";
            this.bin2.GetLGUCode = "";
            this.bin2.GetTaxYear = "";
            this.bin2.Location = new System.Drawing.Point(63, 15);
            this.bin2.Name = "bin2";
            this.bin2.Size = new System.Drawing.Size(141, 23);
            this.bin2.TabIndex = 1;
            // 
            // btnCancelApp
            // 
            this.btnCancelApp.Enabled = false;
            this.btnCancelApp.Location = new System.Drawing.Point(329, 388);
            this.btnCancelApp.Name = "btnCancelApp";
            this.btnCancelApp.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancelApp.Size = new System.Drawing.Size(117, 25);
            this.btnCancelApp.TabIndex = 9;
            this.btnCancelApp.Text = "Cancel Application";
            this.btnCancelApp.Values.ExtraText = "";
            this.btnCancelApp.Values.Image = null;
            this.btnCancelApp.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancelApp.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancelApp.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancelApp.Values.Text = "Cancel Application";
            this.btnCancelApp.Click += new System.EventHandler(this.btnCancelApp_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(329, 416);
            this.btnSave.Name = "btnSave";
            this.btnSave.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSave.Size = new System.Drawing.Size(117, 25);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.Values.ExtraText = "";
            this.btnSave.Values.Image = null;
            this.btnSave.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSave.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSave.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSave.Values.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(329, 444);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(117, 25);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Close";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(4, 3);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(455, 147);
            this.containerWithShadow1.TabIndex = 17;
            // 
            // btnRollBack
            // 
            this.btnRollBack.Enabled = false;
            this.btnRollBack.Location = new System.Drawing.Point(206, 444);
            this.btnRollBack.Name = "btnRollBack";
            this.btnRollBack.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnRollBack.Size = new System.Drawing.Size(117, 25);
            this.btnRollBack.TabIndex = 18;
            this.btnRollBack.Text = "Reverse Retirement";
            this.btnRollBack.Values.ExtraText = "";
            this.btnRollBack.Values.Image = null;
            this.btnRollBack.Values.ImageStates.ImageCheckedNormal = null;
            this.btnRollBack.Values.ImageStates.ImageCheckedPressed = null;
            this.btnRollBack.Values.ImageStates.ImageCheckedTracking = null;
            this.btnRollBack.Values.Text = "Reverse Retirement";
            this.btnRollBack.Visible = false;
            this.btnRollBack.Click += new System.EventHandler(this.btnRollBack_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(21, 444);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint.Size = new System.Drawing.Size(117, 25);
            this.btnPrint.TabIndex = 18;
            this.btnPrint.Text = "Print";
            this.btnPrint.Values.ExtraText = "";
            this.btnPrint.Values.Image = null;
            this.btnPrint.Values.ImageStates.ImageCheckedNormal = null;
            this.btnPrint.Values.ImageStates.ImageCheckedPressed = null;
            this.btnPrint.Values.ImageStates.ImageCheckedTracking = null;
            this.btnPrint.Values.Text = "Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // bin2
            // 
            this.bin2.GetBINSeries = "";
            this.bin2.GetDistCode = "";
            this.bin2.GetLGUCode = "";
            this.bin2.GetTaxYear = "";
            this.bin2.Location = new System.Drawing.Point(63, 15);
            this.bin2.Name = "bin2";
            this.bin2.Size = new System.Drawing.Size(141, 23);
            this.bin2.TabIndex = 1;
            // 
            // frmRetirement
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(463, 490);
            this.ControlBox = false;
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnRollBack);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.bin2);
            this.Controls.Add(this.btnCancelApp);
            this.Controls.Add(this.chkWaiveTaxAndFees);
            this.Controls.Add(this.chkBilling);
            this.Controls.Add(this.chkPartial);
            this.Controls.Add(this.chkFull);
            this.Controls.Add(this.txtReason);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.txtAppNo);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtBnsAdd);
            this.Controls.Add(this.txtBnsName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.containerWithShadow2);
            this.Controls.Add(this.containerWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRetirement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Retirement";
            this.Load += new System.EventHandler(this.frmRetirement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBnsName;
        private System.Windows.Forms.TextBox txtBnsAdd;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtAppNo;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.CheckBox chkFull;
        private System.Windows.Forms.CheckBox chkPartial;
        private System.Windows.Forms.CheckBox chkBilling;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkWaiveTaxAndFees;
        private Amellar.Common.BIN.BIN bin2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancelApp;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSave;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnRollBack;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPrint;
    }
}

