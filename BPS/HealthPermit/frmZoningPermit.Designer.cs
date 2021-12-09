namespace Amellar.Modules.HealthPermit
{
    partial class frmZoningPermit
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
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMiddleInitial = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearchTmp = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtTmpBin = new System.Windows.Forms.TextBox();
            this.btnNew = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDiscard = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPrint = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDelete = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEdit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtTaxYear = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cboZoning = new System.Windows.Forms.ComboBox();
            this.txtMP = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtSP = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtTCTNo = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnSearchRen = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFirstName
            // 
            this.txtFirstName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFirstName.Location = new System.Drawing.Point(83, 107);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.ReadOnly = true;
            this.txtFirstName.Size = new System.Drawing.Size(206, 20);
            this.txtFirstName.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(295, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "M.I";
            // 
            // txtMiddleInitial
            // 
            this.txtMiddleInitial.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMiddleInitial.Location = new System.Drawing.Point(323, 107);
            this.txtMiddleInitial.MaxLength = 1;
            this.txtMiddleInitial.Name = "txtMiddleInitial";
            this.txtMiddleInitial.ReadOnly = true;
            this.txtMiddleInitial.Size = new System.Drawing.Size(28, 20);
            this.txtMiddleInitial.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "First Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Last Name";
            // 
            // txtLastName
            // 
            this.txtLastName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLastName.Location = new System.Drawing.Point(83, 81);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.ReadOnly = true;
            this.txtLastName.Size = new System.Drawing.Size(268, 20);
            this.txtLastName.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSearchRen);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnSearchTmp);
            this.groupBox1.Controls.Add(this.txtTmpBin);
            this.groupBox1.Controls.Add(this.txtMiddleInitial);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtFirstName);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtLastName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 183);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(368, 138);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Business Owner\'s Name ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "BIN";
            // 
            // btnSearchTmp
            // 
            this.btnSearchTmp.Enabled = false;
            this.btnSearchTmp.Location = new System.Drawing.Point(230, 20);
            this.btnSearchTmp.Name = "btnSearchTmp";
            this.btnSearchTmp.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearchTmp.Size = new System.Drawing.Size(114, 24);
            this.btnSearchTmp.TabIndex = 38;
            this.btnSearchTmp.Text = "Search";
            this.btnSearchTmp.Values.ExtraText = "";
            this.btnSearchTmp.Values.Image = null;
            this.btnSearchTmp.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearchTmp.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearchTmp.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearchTmp.Values.Text = "Search";
            this.btnSearchTmp.Click += new System.EventHandler(this.btnSearchTmp_Click);
            // 
            // txtTmpBin
            // 
            this.txtTmpBin.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTmpBin.Location = new System.Drawing.Point(83, 21);
            this.txtTmpBin.MaxLength = 20;
            this.txtTmpBin.Name = "txtTmpBin";
            this.txtTmpBin.ReadOnly = true;
            this.txtTmpBin.Size = new System.Drawing.Size(138, 20);
            this.txtTmpBin.TabIndex = 37;
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(388, 308);
            this.btnNew.Name = "btnNew";
            this.btnNew.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnNew.Size = new System.Drawing.Size(92, 25);
            this.btnNew.TabIndex = 8;
            this.btnNew.Text = "Add";
            this.btnNew.Values.ExtraText = "";
            this.btnNew.Values.Image = null;
            this.btnNew.Values.ImageStates.ImageCheckedNormal = null;
            this.btnNew.Values.ImageStates.ImageCheckedPressed = null;
            this.btnNew.Values.ImageStates.ImageCheckedTracking = null;
            this.btnNew.Values.Text = "Add";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnDiscard
            // 
            this.btnDiscard.Location = new System.Drawing.Point(388, 432);
            this.btnDiscard.Name = "btnDiscard";
            this.btnDiscard.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnDiscard.Size = new System.Drawing.Size(92, 25);
            this.btnDiscard.TabIndex = 12;
            this.btnDiscard.Text = "Cancel";
            this.btnDiscard.Values.ExtraText = "";
            this.btnDiscard.Values.Image = null;
            this.btnDiscard.Values.ImageStates.ImageCheckedNormal = null;
            this.btnDiscard.Values.ImageStates.ImageCheckedPressed = null;
            this.btnDiscard.Values.ImageStates.ImageCheckedTracking = null;
            this.btnDiscard.Values.Text = "Cancel";
            this.btnDiscard.Click += new System.EventHandler(this.btnDiscard_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(388, 463);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(92, 25);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(388, 401);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint.Size = new System.Drawing.Size(92, 25);
            this.btnPrint.TabIndex = 11;
            this.btnPrint.Text = "Print";
            this.btnPrint.Values.ExtraText = "";
            this.btnPrint.Values.Image = null;
            this.btnPrint.Values.ImageStates.ImageCheckedNormal = null;
            this.btnPrint.Values.ImageStates.ImageCheckedPressed = null;
            this.btnPrint.Values.ImageStates.ImageCheckedTracking = null;
            this.btnPrint.Values.Text = "Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(388, 370);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnDelete.Size = new System.Drawing.Size(92, 25);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Values.ExtraText = "";
            this.btnDelete.Values.Image = null;
            this.btnDelete.Values.ImageStates.ImageCheckedNormal = null;
            this.btnDelete.Values.ImageStates.ImageCheckedPressed = null;
            this.btnDelete.Values.ImageStates.ImageCheckedTracking = null;
            this.btnDelete.Values.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(388, 339);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnEdit.Size = new System.Drawing.Size(92, 25);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "Edit";
            this.btnEdit.Values.ExtraText = "";
            this.btnEdit.Values.Image = null;
            this.btnEdit.Values.ImageStates.ImageCheckedNormal = null;
            this.btnEdit.Values.ImageStates.ImageCheckedPressed = null;
            this.btnEdit.Values.ImageStates.ImageCheckedTracking = null;
            this.btnEdit.Values.Text = "Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvList);
            this.groupBox3.Location = new System.Drawing.Point(12, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(466, 172);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "List of Issued Permits";
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToResizeRows = false;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvList.Location = new System.Drawing.Point(7, 19);
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.Size = new System.Drawing.Size(452, 147);
            this.dgvList.TabIndex = 0;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtTaxYear);
            this.groupBox4.Location = new System.Drawing.Point(388, 183);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(90, 50);
            this.groupBox4.TabIndex = 27;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Tax Year";
            // 
            // txtTaxYear
            // 
            this.txtTaxYear.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTaxYear.Location = new System.Drawing.Point(15, 20);
            this.txtTaxYear.Name = "txtTaxYear";
            this.txtTaxYear.ReadOnly = true;
            this.txtTaxYear.Size = new System.Drawing.Size(59, 20);
            this.txtTaxYear.TabIndex = 20;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cboZoning);
            this.groupBox6.Controls.Add(this.txtMP);
            this.groupBox6.Controls.Add(this.label11);
            this.groupBox6.Controls.Add(this.txtSP);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Controls.Add(this.label13);
            this.groupBox6.Controls.Add(this.txtTCTNo);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Location = new System.Drawing.Point(12, 327);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(368, 165);
            this.groupBox6.TabIndex = 27;
            this.groupBox6.TabStop = false;
            // 
            // cboZoning
            // 
            this.cboZoning.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboZoning.Enabled = false;
            this.cboZoning.FormattingEnabled = true;
            this.cboZoning.Items.AddRange(new object[] {
            "RESIDENTIAL",
            "AGRICULTURAL",
            "COMMERCIAL",
            "INDUSTRIAL",
            "MINERAL",
            "TIMBERLAND"});
            this.cboZoning.Location = new System.Drawing.Point(83, 39);
            this.cboZoning.Name = "cboZoning";
            this.cboZoning.Size = new System.Drawing.Size(182, 21);
            this.cboZoning.TabIndex = 28;
            // 
            // txtMP
            // 
            this.txtMP.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMP.Location = new System.Drawing.Point(142, 112);
            this.txtMP.Multiline = true;
            this.txtMP.Name = "txtMP";
            this.txtMP.ReadOnly = true;
            this.txtMP.Size = new System.Drawing.Size(209, 40);
            this.txtMP.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(19, 117);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(125, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "Municipal Resolution No.";
            // 
            // txtSP
            // 
            this.txtSP.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtSP.Location = new System.Drawing.Point(142, 66);
            this.txtSP.Multiline = true;
            this.txtSP.Name = "txtSP";
            this.txtSP.ReadOnly = true;
            this.txtSP.Size = new System.Drawing.Size(209, 40);
            this.txtSP.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(19, 69);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(94, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "SP Resolution No.";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(20, 17);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(48, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "TCT No.";
            // 
            // txtTCTNo
            // 
            this.txtTCTNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTCTNo.Location = new System.Drawing.Point(83, 14);
            this.txtTCTNo.Name = "txtTCTNo";
            this.txtTCTNo.ReadOnly = true;
            this.txtTCTNo.Size = new System.Drawing.Size(268, 20);
            this.txtTCTNo.TabIndex = 4;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(20, 43);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 13);
            this.label14.TabIndex = 24;
            this.label14.Text = "Zoning";
            // 
            // btnSearchRen
            // 
            this.btnSearchRen.Location = new System.Drawing.Point(230, 50);
            this.btnSearchRen.Name = "btnSearchRen";
            this.btnSearchRen.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearchRen.Size = new System.Drawing.Size(114, 24);
            this.btnSearchRen.TabIndex = 40;
            this.btnSearchRen.Text = "Search Renewal";
            this.btnSearchRen.Values.ExtraText = "";
            this.btnSearchRen.Values.Image = null;
            this.btnSearchRen.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearchRen.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearchRen.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearchRen.Values.Text = "Search Renewal";
            this.btnSearchRen.Click += new System.EventHandler(this.kryptonButton1_Click);
            // 
            // frmZoningPermit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(490, 504);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnDiscard);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmZoningPermit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zoning Permit";
            this.Load += new System.EventHandler(this.frmZoningPermit_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMiddleInitial;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.GroupBox groupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnNew;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDiscard;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPrint;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDelete;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEdit;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtTaxYear;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox txtMP;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtSP;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtTCTNo;
        private System.Windows.Forms.Label label14;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearchTmp;
        private System.Windows.Forms.TextBox txtTmpBin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboZoning;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearchRen;


    }
}