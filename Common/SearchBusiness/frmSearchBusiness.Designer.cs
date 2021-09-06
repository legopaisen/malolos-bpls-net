namespace Amellar.Common.SearchBusiness
{
    partial class frmSearchBusiness
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearchBusiness));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.txtTaxYear = new System.Windows.Forms.TextBox();
            this.txtLName = new System.Windows.Forms.TextBox();
            this.txtFName = new System.Windows.Forms.TextBox();
            this.txtMI = new System.Windows.Forms.TextBox();
            this.txtBnsAdd = new System.Windows.Forms.TextBox();
            this.txtPermit = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.kryptonBorderEdge2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.btnClearFields = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtOldBin = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtPlate = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.clmBin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBnsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmPermitNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmTaxYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLastNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmFirstNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmMI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmOwnCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlateNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(8, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(518, 106);
            this.label1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Business Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(375, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Permit:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(375, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Tax Year:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Last Name:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "First Name:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(309, 93);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "MI:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 118);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Business Address:";
            // 
            // txtBnsName
            // 
            this.txtBnsName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBnsName.Location = new System.Drawing.Point(103, 44);
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.Size = new System.Drawing.Size(269, 20);
            this.txtBnsName.TabIndex = 1;
            this.txtBnsName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBnsName_KeyPress);
            // 
            // txtTaxYear
            // 
            this.txtTaxYear.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTaxYear.Location = new System.Drawing.Point(428, 44);
            this.txtTaxYear.Name = "txtTaxYear";
            this.txtTaxYear.Size = new System.Drawing.Size(90, 20);
            this.txtTaxYear.TabIndex = 2;
            this.txtTaxYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTaxYear_KeyPress);
            // 
            // txtLName
            // 
            this.txtLName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLName.Location = new System.Drawing.Point(103, 67);
            this.txtLName.Name = "txtLName";
            this.txtLName.Size = new System.Drawing.Size(269, 20);
            this.txtLName.TabIndex = 3;
            this.txtLName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLName_KeyPress);
            // 
            // txtFName
            // 
            this.txtFName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFName.Location = new System.Drawing.Point(103, 90);
            this.txtFName.Name = "txtFName";
            this.txtFName.Size = new System.Drawing.Size(202, 20);
            this.txtFName.TabIndex = 5;
            this.txtFName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFName_KeyPress);
            // 
            // txtMI
            // 
            this.txtMI.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMI.Location = new System.Drawing.Point(328, 90);
            this.txtMI.Name = "txtMI";
            this.txtMI.Size = new System.Drawing.Size(21, 20);
            this.txtMI.TabIndex = 6;
            // 
            // txtBnsAdd
            // 
            this.txtBnsAdd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBnsAdd.Location = new System.Drawing.Point(103, 116);
            this.txtBnsAdd.Name = "txtBnsAdd";
            this.txtBnsAdd.ReadOnly = true;
            this.txtBnsAdd.Size = new System.Drawing.Size(202, 20);
            this.txtBnsAdd.TabIndex = 13;
            // 
            // txtPermit
            // 
            this.txtPermit.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPermit.Location = new System.Drawing.Point(428, 67);
            this.txtPermit.Name = "txtPermit";
            this.txtPermit.Size = new System.Drawing.Size(90, 20);
            this.txtPermit.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label9.Location = new System.Drawing.Point(4, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(518, 106);
            this.label9.TabIndex = 16;
            // 
            // kryptonHeader2
            // 
            this.kryptonHeader2.AutoSize = false;
            this.kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader2.Location = new System.Drawing.Point(6, -1);
            this.kryptonHeader2.Name = "kryptonHeader2";
            this.kryptonHeader2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader2.Size = new System.Drawing.Size(523, 36);
            this.kryptonHeader2.TabIndex = 18;
            this.kryptonHeader2.Text = "Search Criteria";
            this.kryptonHeader2.Values.Description = "";
            this.kryptonHeader2.Values.Heading = "Search Criteria";
            this.kryptonHeader2.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader2.Values.Image")));
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(8, 151);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(518, 159);
            this.label10.TabIndex = 19;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label11.Location = new System.Drawing.Point(4, 155);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(518, 159);
            this.label11.TabIndex = 20;
            // 
            // kryptonBorderEdge1
            // 
            this.kryptonBorderEdge1.Location = new System.Drawing.Point(10, 321);
            this.kryptonBorderEdge1.Name = "kryptonBorderEdge1";
            this.kryptonBorderEdge1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(514, 1);
            this.kryptonBorderEdge1.TabIndex = 21;
            this.kryptonBorderEdge1.Text = "kryptonBorderEdge1";
            // 
            // kryptonBorderEdge2
            // 
            this.kryptonBorderEdge2.Location = new System.Drawing.Point(9, 354);
            this.kryptonBorderEdge2.Name = "kryptonBorderEdge2";
            this.kryptonBorderEdge2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonBorderEdge2.Size = new System.Drawing.Size(514, 1);
            this.kryptonBorderEdge2.TabIndex = 22;
            this.kryptonBorderEdge2.Text = "kryptonBorderEdge2";
            // 
            // btnClearFields
            // 
            this.btnClearFields.Enabled = false;
            this.btnClearFields.Location = new System.Drawing.Point(10, 325);
            this.btnClearFields.Name = "btnClearFields";
            this.btnClearFields.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClearFields.Size = new System.Drawing.Size(90, 25);
            this.btnClearFields.TabIndex = 11;
            this.btnClearFields.Text = "Clear &Fields";
            this.btnClearFields.Values.ExtraText = "";
            this.btnClearFields.Values.Image = null;
            this.btnClearFields.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClearFields.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClearFields.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClearFields.Values.Text = "Clear &Fields";
            this.btnClearFields.Click += new System.EventHandler(this.btnClearFields_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(338, 325);
            this.btnOk.Name = "btnOk";
            this.btnOk.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "&Ok";
            this.btnOk.Values.ExtraText = "";
            this.btnOk.Values.Image = null;
            this.btnOk.Values.ImageStates.ImageCheckedNormal = null;
            this.btnOk.Values.ImageStates.ImageCheckedPressed = null;
            this.btnOk.Values.ImageStates.ImageCheckedTracking = null;
            this.btnOk.Values.Text = "&Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(434, 325);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgvResult
            // 
            this.dgvResult.AllowUserToAddRows = false;
            this.dgvResult.AllowUserToDeleteRows = false;
            this.dgvResult.AllowUserToResizeRows = false;
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmBin,
            this.clmBnsName,
            this.clmPermitNo,
            this.clmTaxYear,
            this.clmLastNm,
            this.clmFirstNm,
            this.clmMI,
            this.clmOwnCode,
            this.PlateNo});
            this.dgvResult.Location = new System.Drawing.Point(14, 158);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.ReadOnly = true;
            this.dgvResult.RowHeadersVisible = false;
            this.dgvResult.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResult.Size = new System.Drawing.Size(508, 144);
            this.dgvResult.TabIndex = 26;
            this.dgvResult.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResult_CellClick);
            this.dgvResult.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResult_CellContentClick);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(434, 112);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(84, 25);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "&Search";
            this.btnSearch.Values.ExtraText = "";
            this.btnSearch.Values.Image = null;
            this.btnSearch.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearch.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearch.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearch.Values.Text = "&Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtOldBin
            // 
            this.txtOldBin.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtOldBin.Location = new System.Drawing.Point(404, 90);
            this.txtOldBin.Multiline = true;
            this.txtOldBin.Name = "txtOldBin";
            this.txtOldBin.Size = new System.Drawing.Size(114, 20);
            this.txtOldBin.TabIndex = 7;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(356, 93);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "Old BIN:";
            // 
            // txtPlate
            // 
            this.txtPlate.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPlate.Location = new System.Drawing.Point(359, 115);
            this.txtPlate.Name = "txtPlate";
            this.txtPlate.Size = new System.Drawing.Size(69, 20);
            this.txtPlate.TabIndex = 28;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(315, 118);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(34, 13);
            this.label13.TabIndex = 27;
            this.label13.Text = "Plate:";
            // 
            // clmBin
            // 
            this.clmBin.HeaderText = "BIN";
            this.clmBin.Name = "clmBin";
            this.clmBin.ReadOnly = true;
            // 
            // clmBnsName
            // 
            this.clmBnsName.HeaderText = "Business Name";
            this.clmBnsName.Name = "clmBnsName";
            this.clmBnsName.ReadOnly = true;
            this.clmBnsName.Width = 150;
            // 
            // clmPermitNo
            // 
            this.clmPermitNo.HeaderText = "Permit Number";
            this.clmPermitNo.Name = "clmPermitNo";
            this.clmPermitNo.ReadOnly = true;
            // 
            // clmTaxYear
            // 
            this.clmTaxYear.HeaderText = "Tax Year";
            this.clmTaxYear.Name = "clmTaxYear";
            this.clmTaxYear.ReadOnly = true;
            // 
            // clmLastNm
            // 
            this.clmLastNm.HeaderText = "Last Name";
            this.clmLastNm.Name = "clmLastNm";
            this.clmLastNm.ReadOnly = true;
            // 
            // clmFirstNm
            // 
            this.clmFirstNm.HeaderText = "First Name";
            this.clmFirstNm.Name = "clmFirstNm";
            this.clmFirstNm.ReadOnly = true;
            // 
            // clmMI
            // 
            this.clmMI.HeaderText = "MI";
            this.clmMI.Name = "clmMI";
            this.clmMI.ReadOnly = true;
            // 
            // clmOwnCode
            // 
            this.clmOwnCode.HeaderText = "Own Code";
            this.clmOwnCode.Name = "clmOwnCode";
            this.clmOwnCode.ReadOnly = true;
            // 
            // PlateNo
            // 
            this.PlateNo.HeaderText = "Plate No";
            this.PlateNo.Name = "PlateNo";
            this.PlateNo.ReadOnly = true;
            // 
            // frmSearchBusiness
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(537, 368);
            this.ControlBox = false;
            this.Controls.Add(this.txtPlate);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dgvResult);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnClearFields);
            this.Controls.Add(this.kryptonBorderEdge2);
            this.Controls.Add(this.kryptonBorderEdge1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.kryptonHeader2);
            this.Controls.Add(this.txtOldBin);
            this.Controls.Add(this.txtPermit);
            this.Controls.Add(this.txtBnsAdd);
            this.Controls.Add(this.txtMI);
            this.Controls.Add(this.txtFName);
            this.Controls.Add(this.txtLName);
            this.Controls.Add(this.txtTaxYear);
            this.Controls.Add(this.txtBnsName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label9);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximumSize = new System.Drawing.Size(543, 396);
            this.MinimumSize = new System.Drawing.Size(543, 396);
            this.Name = "frmSearchBusiness";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.Load += new System.EventHandler(this.frmSearchBusiness_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
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
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBnsName;
        private System.Windows.Forms.TextBox txtTaxYear;
        private System.Windows.Forms.TextBox txtLName;
        private System.Windows.Forms.TextBox txtFName;
        private System.Windows.Forms.TextBox txtMI;
        private System.Windows.Forms.TextBox txtBnsAdd;
        private System.Windows.Forms.TextBox txtPermit;
        private System.Windows.Forms.Label label9;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClearFields;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        public System.Windows.Forms.DataGridView dgvResult;
        private System.Windows.Forms.TextBox txtOldBin;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtPlate;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmBin;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmBnsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmPermitNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTaxYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLastNm;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmFirstNm;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmMI;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOwnCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlateNo;
    }
}

