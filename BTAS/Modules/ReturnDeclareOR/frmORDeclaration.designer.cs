namespace ReturnDeclareOR
{
    partial class frmORDeclaration
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
            this.dgvList = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboTellerCode = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.label5 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.txtMI = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtFName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtLName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.btnClear = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label4 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.label3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.label2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnEdit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtCurrent = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtTo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.btnReturn = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtFrom = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.btnDeclare = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label10 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.label9 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.label8 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.gbAvailableOR = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.dgvAvailableOR = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlTellerInfo = new System.Windows.Forms.Panel();
            this.pnlORSeries = new System.Windows.Forms.Panel();
            this.pnlAvailableOR = new System.Windows.Forms.Panel();
            this.pnlBackground = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbAvailableOR)).BeginInit();
            this.gbAvailableOR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableOR)).BeginInit();
            this.pnlTellerInfo.SuspendLayout();
            this.pnlORSeries.SuspendLayout();
            this.pnlAvailableOR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBackground)).BeginInit();
            this.pnlBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToResizeRows = false;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column10});
            this.dgvList.Location = new System.Drawing.Point(12, 12);
            this.dgvList.MultiSelect = false;
            this.dgvList.Name = "dgvList";
            this.dgvList.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.dgvList.ReadOnly = true;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(436, 177);
            this.dgvList.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.dgvList.TabIndex = 0;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.cboTellerCode);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtMI);
            this.groupBox1.Controls.Add(this.txtFName);
            this.groupBox1.Controls.Add(this.txtLName);
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(436, 124);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // cboTellerCode
            // 
            this.cboTellerCode.DropDownWidth = 121;
            this.cboTellerCode.Enabled = false;
            this.cboTellerCode.FormattingEnabled = false;
            this.cboTellerCode.Location = new System.Drawing.Point(80, 21);
            this.cboTellerCode.Name = "cboTellerCode";
            this.cboTellerCode.Size = new System.Drawing.Size(121, 21);
            this.cboTellerCode.TabIndex = 12;
            this.cboTellerCode.SelectedIndexChanged += new System.EventHandler(this.cboTellerCode_SelectedIndexChanged);
            this.cboTellerCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboTellerCode_KeyPress);
            this.cboTellerCode.DropDown += new System.EventHandler(this.cboTellerCode_DropDown);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(372, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "M.I";
            this.label5.Values.ExtraText = "";
            this.label5.Values.Image = null;
            this.label5.Values.Text = "M.I";
            // 
            // txtMI
            // 
            this.txtMI.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMI.Location = new System.Drawing.Point(406, 79);
            this.txtMI.MaxLength = 2;
            this.txtMI.Name = "txtMI";
            this.txtMI.ReadOnly = true;
            this.txtMI.Size = new System.Drawing.Size(22, 20);
            this.txtMI.TabIndex = 9;
            // 
            // txtFName
            // 
            this.txtFName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFName.Location = new System.Drawing.Point(80, 79);
            this.txtFName.Name = "txtFName";
            this.txtFName.ReadOnly = true;
            this.txtFName.Size = new System.Drawing.Size(286, 20);
            this.txtFName.TabIndex = 8;
            // 
            // txtLName
            // 
            this.txtLName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLName.Location = new System.Drawing.Point(80, 50);
            this.txtLName.Name = "txtLName";
            this.txtLName.ReadOnly = true;
            this.txtLName.Size = new System.Drawing.Size(286, 20);
            this.txtLName.TabIndex = 7;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(288, 19);
            this.btnClear.Name = "btnClear";
            this.btnClear.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparklePurple;
            this.btnClear.Size = new System.Drawing.Size(75, 24);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.Values.ExtraText = "";
            this.btnClear.Values.Image = null;
            this.btnClear.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClear.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClear.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClear.Values.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(207, 19);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparklePurple;
            this.btnSearch.Size = new System.Drawing.Size(75, 24);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "&Search";
            this.btnSearch.Values.ExtraText = "";
            this.btnSearch.Values.Image = null;
            this.btnSearch.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearch.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearch.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearch.Values.Text = "&Search";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "First Name";
            this.label4.Values.ExtraText = "";
            this.label4.Values.Image = null;
            this.label4.Values.Text = "First Name";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Last Name";
            this.label3.Values.ExtraText = "";
            this.label3.Values.Image = null;
            this.label3.Values.Text = "Last Name";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Teller Code";
            this.label2.Values.ExtraText = "";
            this.label2.Values.Image = null;
            this.label2.Values.Text = "Teller Code";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.btnEdit);
            this.groupBox2.Controls.Add(this.txtCurrent);
            this.groupBox2.Controls.Add(this.txtTo);
            this.groupBox2.Controls.Add(this.btnReturn);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.txtFrom);
            this.groupBox2.Controls.Add(this.btnDeclare);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(436, 126);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            // 
            // btnEdit
            // 
            this.btnEdit.Enabled = false;
            this.btnEdit.Location = new System.Drawing.Point(187, 68);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparklePurple;
            this.btnEdit.Size = new System.Drawing.Size(75, 24);
            this.btnEdit.TabIndex = 7;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.Values.ExtraText = "";
            this.btnEdit.Values.Image = null;
            this.btnEdit.Values.ImageStates.ImageCheckedNormal = null;
            this.btnEdit.Values.ImageStates.ImageCheckedPressed = null;
            this.btnEdit.Values.ImageStates.ImageCheckedTracking = null;
            this.btnEdit.Values.Text = "&Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // txtCurrent
            // 
            this.txtCurrent.Enabled = false;
            this.txtCurrent.Location = new System.Drawing.Point(337, 35);
            this.txtCurrent.MaxLength = 7;
            this.txtCurrent.Name = "txtCurrent";
            this.txtCurrent.Size = new System.Drawing.Size(89, 20);
            this.txtCurrent.TabIndex = 17;
            // 
            // txtTo
            // 
            this.txtTo.Enabled = false;
            this.txtTo.Location = new System.Drawing.Point(171, 35);
            this.txtTo.MaxLength = 7;
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(89, 20);
            this.txtTo.TabIndex = 5;
            // 
            // btnReturn
            // 
            this.btnReturn.Enabled = false;
            this.btnReturn.Location = new System.Drawing.Point(268, 68);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparklePurple;
            this.btnReturn.Size = new System.Drawing.Size(75, 24);
            this.btnReturn.TabIndex = 8;
            this.btnReturn.Text = "&Return";
            this.btnReturn.Values.ExtraText = "";
            this.btnReturn.Values.Image = null;
            this.btnReturn.Values.ImageStates.ImageCheckedNormal = null;
            this.btnReturn.Values.ImageStates.ImageCheckedPressed = null;
            this.btnReturn.Values.ImageStates.ImageCheckedTracking = null;
            this.btnReturn.Values.Text = "&Return";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(349, 68);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparklePurple;
            this.btnClose.Size = new System.Drawing.Size(75, 24);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "&Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtFrom
            // 
            this.txtFrom.Enabled = false;
            this.txtFrom.Location = new System.Drawing.Point(46, 35);
            this.txtFrom.MaxLength = 7;
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(89, 20);
            this.txtFrom.TabIndex = 4;
            // 
            // btnDeclare
            // 
            this.btnDeclare.Enabled = false;
            this.btnDeclare.Location = new System.Drawing.Point(106, 68);
            this.btnDeclare.Name = "btnDeclare";
            this.btnDeclare.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparklePurple;
            this.btnDeclare.Size = new System.Drawing.Size(75, 24);
            this.btnDeclare.TabIndex = 6;
            this.btnDeclare.Text = "&Declare";
            this.btnDeclare.Values.ExtraText = "";
            this.btnDeclare.Values.Image = null;
            this.btnDeclare.Values.ImageStates.ImageCheckedNormal = null;
            this.btnDeclare.Values.ImageStates.ImageCheckedPressed = null;
            this.btnDeclare.Values.ImageStates.ImageCheckedTracking = null;
            this.btnDeclare.Values.Text = "&Declare";
            this.btnDeclare.Click += new System.EventHandler(this.btnDeclare_Click);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(3, 35);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 20);
            this.label10.TabIndex = 14;
            this.label10.Text = "From";
            this.label10.Values.ExtraText = "";
            this.label10.Values.Image = null;
            this.label10.Values.Text = "From";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(141, 35);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 20);
            this.label9.TabIndex = 13;
            this.label9.Text = "To";
            this.label9.Values.ExtraText = "";
            this.label9.Values.Image = null;
            this.label9.Values.Text = "To";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(266, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 20);
            this.label8.TabIndex = 12;
            this.label8.Text = "Current OR";
            this.label8.Values.ExtraText = "";
            this.label8.Values.Image = null;
            this.label8.Values.Text = "Current OR";
            // 
            // gbAvailableOR
            // 
            this.gbAvailableOR.Controls.Add(this.dgvAvailableOR);
            this.gbAvailableOR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbAvailableOR.Location = new System.Drawing.Point(0, 0);
            this.gbAvailableOR.Name = "gbAvailableOR";
            this.gbAvailableOR.Size = new System.Drawing.Size(435, 189);
            this.gbAvailableOR.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.gbAvailableOR.TabIndex = 13;
            this.gbAvailableOR.TabStop = false;
            // 
            // dgvAvailableOR
            // 
            this.dgvAvailableOR.AllowUserToAddRows = false;
            this.dgvAvailableOR.AllowUserToDeleteRows = false;
            this.dgvAvailableOR.AllowUserToResizeRows = false;
            this.dgvAvailableOR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAvailableOR.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dgvAvailableOR.Location = new System.Drawing.Point(8, 17);
            this.dgvAvailableOR.Name = "dgvAvailableOR";
            this.dgvAvailableOR.ReadOnly = true;
            this.dgvAvailableOR.RowHeadersVisible = false;
            this.dgvAvailableOR.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAvailableOR.Size = new System.Drawing.Size(418, 136);
            this.dgvAvailableOR.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.dgvAvailableOR.TabIndex = 15;
            this.dgvAvailableOR.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListORNum_CellClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "OR Number From";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 205;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "OR Number To";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 205;
            // 
            // pnlTellerInfo
            // 
            this.pnlTellerInfo.BackColor = System.Drawing.Color.Transparent;
            this.pnlTellerInfo.Controls.Add(this.groupBox1);
            this.pnlTellerInfo.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlTellerInfo.Location = new System.Drawing.Point(12, 195);
            this.pnlTellerInfo.Name = "pnlTellerInfo";
            this.pnlTellerInfo.Size = new System.Drawing.Size(436, 124);
            this.pnlTellerInfo.TabIndex = 16;
            this.pnlTellerInfo.Text = "Teller Information";
            // 
            // pnlORSeries
            // 
            this.pnlORSeries.BackColor = System.Drawing.Color.Transparent;
            this.pnlORSeries.Controls.Add(this.groupBox2);
            this.pnlORSeries.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlORSeries.Location = new System.Drawing.Point(12, 325);
            this.pnlORSeries.Name = "pnlORSeries";
            this.pnlORSeries.Size = new System.Drawing.Size(436, 126);
            this.pnlORSeries.TabIndex = 17;
            this.pnlORSeries.Text = "O.R. Series";
            // 
            // pnlAvailableOR
            // 
            this.pnlAvailableOR.Controls.Add(this.gbAvailableOR);
            this.pnlAvailableOR.Location = new System.Drawing.Point(12, 457);
            this.pnlAvailableOR.Name = "pnlAvailableOR";
            this.pnlAvailableOR.Size = new System.Drawing.Size(435, 189);
            this.pnlAvailableOR.TabIndex = 18;
            this.pnlAvailableOR.Text = "Available O.R Series for";
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.pnlTellerInfo);
            this.pnlBackground.Controls.Add(this.pnlORSeries);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.pnlBackground.Size = new System.Drawing.Size(460, 455);
            this.pnlBackground.TabIndex = 19;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Teller";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "O.R. From";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "O.R. To";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Date Assigned";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Assigned By";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Dec_Series";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "Current O.R.";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            // 
            // frmDeclaration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 455);
            this.ControlBox = false;
            this.Controls.Add(this.pnlAvailableOR);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.pnlBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "frmDeclaration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "O.R. Declaration";
            this.Load += new System.EventHandler(this.frmDeclaration_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbAvailableOR)).EndInit();
            this.gbAvailableOR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableOR)).EndInit();
            this.pnlTellerInfo.ResumeLayout(false);
            this.pnlORSeries.ResumeLayout(false);
            this.pnlAvailableOR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlBackground)).EndInit();
            this.pnlBackground.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel label5;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMI;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFName;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLName;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClear;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel label4;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel label3;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCurrent;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTo;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFrom;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel label10;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel label9;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel label8;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnReturn;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDeclare;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEdit;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView gbAvailableOR;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView dgvAvailableOR;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Panel pnlTellerInfo;
        public ComponentFactory.Krypton.Toolkit.KryptonDataGridView dgvList;
        public ComponentFactory.Krypton.Toolkit.KryptonComboBox cboTellerCode;
        private System.Windows.Forms.Panel pnlORSeries;
        private System.Windows.Forms.Panel pnlAvailableOR;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel pnlBackground;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
    }
}