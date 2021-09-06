namespace Amellar.Common.SearchUser
{
    partial class frmSearchUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearchUser));
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMI = new System.Windows.Forms.TextBox();
            this.txtFName = new System.Windows.Forms.TextBox();
            this.txtLName = new System.Windows.Forms.TextBox();
            this.txtUserCode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClearFields = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.cmbPosition = new System.Windows.Forms.ComboBox();
            this.clmUserCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLastNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmFirstNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmMI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvResult
            // 
            this.dgvResult.AllowUserToAddRows = false;
            this.dgvResult.AllowUserToDeleteRows = false;
            this.dgvResult.AllowUserToResizeRows = false;
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmUserCode,
            this.clmLastNm,
            this.clmFirstNm,
            this.clmMI,
            this.clmPosition});
            this.dgvResult.Location = new System.Drawing.Point(15, 148);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.ReadOnly = true;
            this.dgvResult.RowHeadersVisible = false;
            this.dgvResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResult.Size = new System.Drawing.Size(508, 144);
            this.dgvResult.TabIndex = 49;
            this.dgvResult.SelectionChanged += new System.EventHandler(this.dgvResult_SelectionChanged);
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(9, 141);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(518, 159);
            this.label10.TabIndex = 47;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label11.Location = new System.Drawing.Point(5, 145);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(518, 159);
            this.label11.TabIndex = 48;
            // 
            // txtMI
            // 
            this.txtMI.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMI.Location = new System.Drawing.Point(336, 73);
            this.txtMI.Name = "txtMI";
            this.txtMI.Size = new System.Drawing.Size(38, 20);
            this.txtMI.TabIndex = 3;
            // 
            // txtFName
            // 
            this.txtFName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFName.Location = new System.Drawing.Point(88, 73);
            this.txtFName.Name = "txtFName";
            this.txtFName.Size = new System.Drawing.Size(223, 20);
            this.txtFName.TabIndex = 2;
            // 
            // txtLName
            // 
            this.txtLName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLName.Location = new System.Drawing.Point(88, 48);
            this.txtLName.Name = "txtLName";
            this.txtLName.Size = new System.Drawing.Size(286, 20);
            this.txtLName.TabIndex = 1;
            // 
            // txtUserCode
            // 
            this.txtUserCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtUserCode.Location = new System.Drawing.Point(393, 71);
            this.txtUserCode.Name = "txtUserCode";
            this.txtUserCode.ReadOnly = true;
            this.txtUserCode.Size = new System.Drawing.Size(119, 20);
            this.txtUserCode.TabIndex = 33;
            this.txtUserCode.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 41;
            this.label6.Text = "First Name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(313, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(19, 13);
            this.label7.TabIndex = 39;
            this.label7.Text = "MI";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 37;
            this.label5.Text = "Last Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(423, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "User Code";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Position";
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(9, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(518, 93);
            this.label1.TabIndex = 29;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label9.Location = new System.Drawing.Point(5, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(518, 94);
            this.label9.TabIndex = 46;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(411, 97);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(84, 25);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.Values.ExtraText = "";
            this.btnSearch.Values.Image = null;
            this.btnSearch.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearch.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearch.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(434, 307);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(338, 307);
            this.btnOk.Name = "btnOk";
            this.btnOk.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "&Ok";
            this.btnOk.Values.ExtraText = "";
            this.btnOk.Values.Image = null;
            this.btnOk.Values.ImageStates.ImageCheckedNormal = null;
            this.btnOk.Values.ImageStates.ImageCheckedPressed = null;
            this.btnOk.Values.ImageStates.ImageCheckedTracking = null;
            this.btnOk.Values.Text = "&Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnClearFields
            // 
            this.btnClearFields.Location = new System.Drawing.Point(10, 307);
            this.btnClearFields.Name = "btnClearFields";
            this.btnClearFields.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClearFields.Size = new System.Drawing.Size(90, 25);
            this.btnClearFields.TabIndex = 6;
            this.btnClearFields.Text = "Clear &Fields";
            this.btnClearFields.Values.ExtraText = "";
            this.btnClearFields.Values.Image = null;
            this.btnClearFields.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClearFields.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClearFields.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClearFields.Values.Text = "Clear &Fields";
            this.btnClearFields.Click += new System.EventHandler(this.btnClearFields_Click);
            // 
            // kryptonHeader2
            // 
            this.kryptonHeader2.AutoSize = false;
            this.kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader2.Location = new System.Drawing.Point(5, 2);
            this.kryptonHeader2.Name = "kryptonHeader2";
            this.kryptonHeader2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader2.Size = new System.Drawing.Size(526, 36);
            this.kryptonHeader2.TabIndex = 56;
            this.kryptonHeader2.Text = "Search Criteria";
            this.kryptonHeader2.Values.Description = "";
            this.kryptonHeader2.Values.Heading = "Search Criteria";
            this.kryptonHeader2.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader2.Values.Image")));
            // 
            // cmbPosition
            // 
            this.cmbPosition.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbPosition.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbPosition.FormattingEnabled = true;
            this.cmbPosition.Location = new System.Drawing.Point(88, 99);
            this.cmbPosition.Name = "cmbPosition";
            this.cmbPosition.Size = new System.Drawing.Size(286, 21);
            this.cmbPosition.TabIndex = 4;
            // 
            // clmUserCode
            // 
            this.clmUserCode.HeaderText = "User Code";
            this.clmUserCode.Name = "clmUserCode";
            this.clmUserCode.ReadOnly = true;
            this.clmUserCode.Width = 80;
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
            this.clmMI.Width = 50;
            // 
            // clmPosition
            // 
            this.clmPosition.HeaderText = "Position";
            this.clmPosition.Name = "clmPosition";
            this.clmPosition.ReadOnly = true;
            this.clmPosition.Width = 150;
            // 
            // frmSearchUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 338);
            this.Controls.Add(this.cmbPosition);
            this.Controls.Add(this.kryptonHeader2);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnClearFields);
            this.Controls.Add(this.dgvResult);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtMI);
            this.Controls.Add(this.txtFName);
            this.Controls.Add(this.txtLName);
            this.Controls.Add(this.txtUserCode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label9);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSearchUser";
            this.Text = "Search User";
            this.Load += new System.EventHandler(this.frmSearchUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView dgvResult;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtMI;
        private System.Windows.Forms.TextBox txtFName;
        private System.Windows.Forms.TextBox txtLName;
        private System.Windows.Forms.TextBox txtUserCode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClearFields;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader2;
        private System.Windows.Forms.ComboBox cmbPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmUserCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLastNm;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmFirstNm;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmMI;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmPosition;
    }
}

