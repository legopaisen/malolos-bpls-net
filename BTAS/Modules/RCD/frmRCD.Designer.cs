namespace Amellar.Modules.RCD
{
    partial class frmRCD
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRCD));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtGrandTotal = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtCheck = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtDebit = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtTotalCollected = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtCash = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtCredit = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.kryptonManager1 = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.cboTeller = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.btnPrev = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.btnNext = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel4 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel5 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel6 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel7 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel8 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel9 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonGroup2 = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.dgViewCoin = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.dgViewPaper = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtTotalPaper = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.txtTotalCoin = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonGroup1 = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.kryptonHeader3 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.kryptonLabel10 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup2.Panel)).BeginInit();
            this.kryptonGroup2.Panel.SuspendLayout();
            this.kryptonGroup2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewCoin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewPaper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1.Panel)).BeginInit();
            this.kryptonGroup1.Panel.SuspendLayout();
            this.kryptonGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtGrandTotal
            // 
            this.txtGrandTotal.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGrandTotal.ForeColor = System.Drawing.Color.Blue;
            this.txtGrandTotal.Location = new System.Drawing.Point(125, 447);
            this.txtGrandTotal.Name = "txtGrandTotal";
            this.txtGrandTotal.ReadOnly = true;
            this.txtGrandTotal.Size = new System.Drawing.Size(169, 20);
            this.txtGrandTotal.TabIndex = 23;
            this.txtGrandTotal.Text = "0";
            this.txtGrandTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtGrandTotal.TextChanged += new System.EventHandler(this.lblGrandTotal_TextChanged);
            // 
            // txtCheck
            // 
            this.txtCheck.Location = new System.Drawing.Point(127, 253);
            this.txtCheck.Name = "txtCheck";
            this.txtCheck.ReadOnly = true;
            this.txtCheck.Size = new System.Drawing.Size(141, 20);
            this.txtCheck.TabIndex = 40;
            this.txtCheck.TabStop = false;
            this.txtCheck.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDebit
            // 
            this.txtDebit.Location = new System.Drawing.Point(127, 142);
            this.txtDebit.Name = "txtDebit";
            this.txtDebit.ReadOnly = true;
            this.txtDebit.Size = new System.Drawing.Size(141, 20);
            this.txtDebit.TabIndex = 38;
            this.txtDebit.TabStop = false;
            this.txtDebit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtTotalCollected
            // 
            this.txtTotalCollected.Location = new System.Drawing.Point(127, 342);
            this.txtTotalCollected.Name = "txtTotalCollected";
            this.txtTotalCollected.ReadOnly = true;
            this.txtTotalCollected.Size = new System.Drawing.Size(141, 20);
            this.txtTotalCollected.TabIndex = 37;
            this.txtTotalCollected.TabStop = false;
            this.txtTotalCollected.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotalCollected.TextChanged += new System.EventHandler(this.txtTotalCollected_TextChanged);
            // 
            // txtCash
            // 
            this.txtCash.Location = new System.Drawing.Point(127, 227);
            this.txtCash.Name = "txtCash";
            this.txtCash.ReadOnly = true;
            this.txtCash.Size = new System.Drawing.Size(141, 20);
            this.txtCash.TabIndex = 41;
            this.txtCash.TabStop = false;
            this.txtCash.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtCredit
            // 
            this.txtCredit.Location = new System.Drawing.Point(127, 169);
            this.txtCredit.Name = "txtCredit";
            this.txtCredit.ReadOnly = true;
            this.txtCredit.Size = new System.Drawing.Size(141, 20);
            this.txtCredit.TabIndex = 39;
            this.txtCredit.TabStop = false;
            this.txtCredit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "MM/dd/yyyy";
            this.dtpDate.Enabled = false;
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(127, 77);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(141, 20);
            this.dtpDate.TabIndex = 36;
            this.dtpDate.TabStop = false;
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            this.dtpDate.Leave += new System.EventHandler(this.dtpDate_Leave);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Enabled = false;
            this.btnGenerate.Location = new System.Drawing.Point(170, 411);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.btnGenerate.Size = new System.Drawing.Size(98, 25);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.Values.ExtraText = "";
            this.btnGenerate.Values.Image = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedNormal = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedPressed = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedTracking = null;
            this.btnGenerate.Values.Text = "&Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(170, 444);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.btnClose.Size = new System.Drawing.Size(98, 25);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "&Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cboTeller
            // 
            this.cboTeller.ButtonSpecs.AddRange(new ComponentFactory.Krypton.Toolkit.ButtonSpecAny[] {
            this.btnPrev,
            this.btnNext});
            this.cboTeller.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTeller.DropDownWidth = 200;
            this.cboTeller.FormattingEnabled = false;
            this.cboTeller.Location = new System.Drawing.Point(127, 48);
            this.cboTeller.Name = "cboTeller";
            this.cboTeller.Size = new System.Drawing.Size(141, 23);
            this.cboTeller.StateCommon.ComboBox.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.cboTeller.StateCommon.ComboBox.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.cboTeller.StateCommon.ComboBox.Border.Rounding = 2;
            this.cboTeller.TabIndex = 50;
            this.cboTeller.TabStop = false;
            this.cboTeller.SelectedIndexChanged += new System.EventHandler(this.cboTeller_SelectedIndexChanged);
            // 
            // btnPrev
            // 
            this.btnPrev.ExtraText = "";
            this.btnPrev.Image = null;
            this.btnPrev.Style = ComponentFactory.Krypton.Toolkit.PaletteButtonStyle.Standalone;
            this.btnPrev.Text = "";
            this.btnPrev.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.ArrowLeft;
            this.btnPrev.UniqueName = "503F26389BE54F03503F26389BE54F03";
            this.btnPrev.Click += new System.EventHandler(this.cboButtonNavigator_Click);
            // 
            // btnNext
            // 
            this.btnNext.ExtraText = "";
            this.btnNext.Image = null;
            this.btnNext.Style = ComponentFactory.Krypton.Toolkit.PaletteButtonStyle.Standalone;
            this.btnNext.Text = "";
            this.btnNext.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.ArrowRight;
            this.btnNext.UniqueName = "DA7910D11E1F4DC1DA7910D11E1F4DC1";
            this.btnNext.Click += new System.EventHandler(this.cboButtonNavigator_Click);
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(33, 48);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(40, 20);
            this.kryptonLabel1.TabIndex = 51;
            this.kryptonLabel1.Text = "Teller";
            this.kryptonLabel1.Values.ExtraText = "";
            this.kryptonLabel1.Values.Image = null;
            this.kryptonLabel1.Values.Text = "Teller";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(33, 77);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(36, 20);
            this.kryptonLabel2.TabIndex = 51;
            this.kryptonLabel2.Text = "Date";
            this.kryptonLabel2.Values.ExtraText = "";
            this.kryptonLabel2.Values.Image = null;
            this.kryptonLabel2.Values.Text = "Date";
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(33, 142);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(71, 20);
            this.kryptonLabel3.TabIndex = 51;
            this.kryptonLabel3.Text = "Total Debit";
            this.kryptonLabel3.Values.ExtraText = "";
            this.kryptonLabel3.Values.Image = null;
            this.kryptonLabel3.Values.Text = "Total Debit";
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Location = new System.Drawing.Point(33, 169);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(74, 20);
            this.kryptonLabel4.TabIndex = 51;
            this.kryptonLabel4.Text = "Total Credit";
            this.kryptonLabel4.Values.ExtraText = "";
            this.kryptonLabel4.Values.Image = null;
            this.kryptonLabel4.Values.Text = "Total Credit";
            // 
            // kryptonLabel5
            // 
            this.kryptonLabel5.Location = new System.Drawing.Point(33, 227);
            this.kryptonLabel5.Name = "kryptonLabel5";
            this.kryptonLabel5.Size = new System.Drawing.Size(67, 20);
            this.kryptonLabel5.TabIndex = 51;
            this.kryptonLabel5.Text = "Total Cash";
            this.kryptonLabel5.Values.ExtraText = "";
            this.kryptonLabel5.Values.Image = null;
            this.kryptonLabel5.Values.Text = "Total Cash";
            // 
            // kryptonLabel6
            // 
            this.kryptonLabel6.Location = new System.Drawing.Point(33, 342);
            this.kryptonLabel6.Name = "kryptonLabel6";
            this.kryptonLabel6.Size = new System.Drawing.Size(92, 20);
            this.kryptonLabel6.TabIndex = 51;
            this.kryptonLabel6.Text = "Total Collected";
            this.kryptonLabel6.Values.ExtraText = "";
            this.kryptonLabel6.Values.Image = null;
            this.kryptonLabel6.Values.Text = "Total Collected";
            // 
            // kryptonLabel7
            // 
            this.kryptonLabel7.Location = new System.Drawing.Point(6, 420);
            this.kryptonLabel7.Name = "kryptonLabel7";
            this.kryptonLabel7.Size = new System.Drawing.Size(38, 20);
            this.kryptonLabel7.TabIndex = 51;
            this.kryptonLabel7.Text = "Total";
            this.kryptonLabel7.Values.ExtraText = "";
            this.kryptonLabel7.Values.Image = null;
            this.kryptonLabel7.Values.Text = "Total";
            // 
            // kryptonLabel8
            // 
            this.kryptonLabel8.Location = new System.Drawing.Point(6, 447);
            this.kryptonLabel8.Name = "kryptonLabel8";
            this.kryptonLabel8.Size = new System.Drawing.Size(74, 20);
            this.kryptonLabel8.TabIndex = 51;
            this.kryptonLabel8.Text = "Grand Total";
            this.kryptonLabel8.Values.ExtraText = "";
            this.kryptonLabel8.Values.Image = null;
            this.kryptonLabel8.Values.Text = "Grand Total";
            // 
            // kryptonLabel9
            // 
            this.kryptonLabel9.Location = new System.Drawing.Point(6, 199);
            this.kryptonLabel9.Name = "kryptonLabel9";
            this.kryptonLabel9.Size = new System.Drawing.Size(38, 20);
            this.kryptonLabel9.TabIndex = 51;
            this.kryptonLabel9.Text = "Total";
            this.kryptonLabel9.Values.ExtraText = "";
            this.kryptonLabel9.Values.Image = null;
            this.kryptonLabel9.Values.Text = "Total";
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonGroup2);
            this.kryptonPanel1.Controls.Add(this.kryptonGroup1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.kryptonPanel1.Size = new System.Drawing.Size(636, 528);
            this.kryptonPanel1.TabIndex = 53;
            // 
            // kryptonGroup2
            // 
            this.kryptonGroup2.Location = new System.Drawing.Point(12, 20);
            this.kryptonGroup2.Name = "kryptonGroup2";
            // 
            // kryptonGroup2.Panel
            // 
            this.kryptonGroup2.Panel.Controls.Add(this.dgViewCoin);
            this.kryptonGroup2.Panel.Controls.Add(this.kryptonHeader2);
            this.kryptonGroup2.Panel.Controls.Add(this.dgViewPaper);
            this.kryptonGroup2.Panel.Controls.Add(this.txtGrandTotal);
            this.kryptonGroup2.Panel.Controls.Add(this.txtTotalPaper);
            this.kryptonGroup2.Panel.Controls.Add(this.kryptonLabel9);
            this.kryptonGroup2.Panel.Controls.Add(this.txtTotalCoin);
            this.kryptonGroup2.Panel.Controls.Add(this.kryptonLabel7);
            this.kryptonGroup2.Panel.Controls.Add(this.kryptonLabel8);
            this.kryptonGroup2.Size = new System.Drawing.Size(303, 482);
            this.kryptonGroup2.TabIndex = 57;
            // 
            // dgViewCoin
            // 
            this.dgViewCoin.AllowUserToAddRows = false;
            this.dgViewCoin.AllowUserToDeleteRows = false;
            this.dgViewCoin.AllowUserToResizeRows = false;
            this.dgViewCoin.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgViewCoin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgViewCoin.ColumnHeadersVisible = false;
            this.dgViewCoin.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dgViewCoin.Enabled = false;
            this.dgViewCoin.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed;
            this.dgViewCoin.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundSheet;
            this.dgViewCoin.GridStyles.StyleDataCells = ComponentFactory.Krypton.Toolkit.GridStyle.Sheet;
            this.dgViewCoin.Location = new System.Drawing.Point(7, 231);
            this.dgViewCoin.MultiSelect = false;
            this.dgViewCoin.Name = "dgViewCoin";
            this.dgViewCoin.RowHeadersWidth = 110;
            this.dgViewCoin.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgViewCoin.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgViewCoin.Size = new System.Drawing.Size(287, 183);
            this.dgViewCoin.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundSheet;
            this.dgViewCoin.TabIndex = 1;
            this.dgViewCoin.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgViewCoin_CellValueChanged);
            this.dgViewCoin.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgViewCoin_CellMouseEnter);
            this.dgViewCoin.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgViewCoin_RowValidated);
            this.dgViewCoin.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgViewCoin_EditingControlShowing);
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTextBoxColumn3.Frozen = true;
            this.dataGridViewTextBoxColumn3.HeaderText = "Quantity";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 60;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn4.Frozen = true;
            this.dataGridViewTextBoxColumn4.HeaderText = "(Coin) Amount";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 115;
            // 
            // kryptonHeader2
            // 
            this.kryptonHeader2.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader2.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeader2.Name = "kryptonHeader2";
            this.kryptonHeader2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.kryptonHeader2.Size = new System.Drawing.Size(301, 22);
            this.kryptonHeader2.TabIndex = 53;
            this.kryptonHeader2.Text = "Denomination";
            this.kryptonHeader2.Values.Description = "";
            this.kryptonHeader2.Values.Heading = "Denomination";
            this.kryptonHeader2.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader2.Values.Image")));
            // 
            // dgViewPaper
            // 
            this.dgViewPaper.AllowUserToAddRows = false;
            this.dgViewPaper.AllowUserToDeleteRows = false;
            this.dgViewPaper.AllowUserToResizeRows = false;
            this.dgViewPaper.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgViewPaper.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgViewPaper.ColumnHeadersVisible = false;
            this.dgViewPaper.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column4});
            this.dgViewPaper.Enabled = false;
            this.dgViewPaper.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed;
            this.dgViewPaper.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundSheet;
            this.dgViewPaper.GridStyles.StyleDataCells = ComponentFactory.Krypton.Toolkit.GridStyle.Sheet;
            this.dgViewPaper.Location = new System.Drawing.Point(7, 31);
            this.dgViewPaper.MultiSelect = false;
            this.dgViewPaper.Name = "dgViewPaper";
            this.dgViewPaper.RowHeadersWidth = 110;
            this.dgViewPaper.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgViewPaper.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgViewPaper.Size = new System.Drawing.Size(287, 162);
            this.dgViewPaper.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundSheet;
            this.dgViewPaper.TabIndex = 0;
            this.dgViewPaper.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgViewPaper_CellValueChanged);
            this.dgViewPaper.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgViewPaper_CellMouseEnter);
            this.dgViewPaper.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgViewPaper_RowValidated);
            this.dgViewPaper.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgViewPaper_EditingControlShowing);
            // 
            // Column1
            // 
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column1.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "Quantity";
            this.Column1.Name = "Column1";
            this.Column1.Width = 60;
            // 
            // Column4
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column4.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column4.Frozen = true;
            this.Column4.HeaderText = "(Paper) Amount";
            this.Column4.Name = "Column4";
            this.Column4.Width = 115;
            // 
            // txtTotalPaper
            // 
            this.txtTotalPaper.Location = new System.Drawing.Point(194, 199);
            this.txtTotalPaper.Name = "txtTotalPaper";
            this.txtTotalPaper.ReadOnly = true;
            this.txtTotalPaper.Size = new System.Drawing.Size(100, 20);
            this.txtTotalPaper.TabIndex = 54;
            this.txtTotalPaper.TabStop = false;
            this.txtTotalPaper.Text = "0";
            this.txtTotalPaper.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtTotalCoin
            // 
            this.txtTotalCoin.Location = new System.Drawing.Point(194, 420);
            this.txtTotalCoin.Name = "txtTotalCoin";
            this.txtTotalCoin.ReadOnly = true;
            this.txtTotalCoin.Size = new System.Drawing.Size(100, 20);
            this.txtTotalCoin.TabIndex = 54;
            this.txtTotalCoin.TabStop = false;
            this.txtTotalCoin.Text = "0";
            this.txtTotalCoin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // kryptonGroup1
            // 
            this.kryptonGroup1.Location = new System.Drawing.Point(321, 21);
            this.kryptonGroup1.Name = "kryptonGroup1";
            // 
            // kryptonGroup1.Panel
            // 
            this.kryptonGroup1.Panel.Controls.Add(this.kryptonLabel4);
            this.kryptonGroup1.Panel.Controls.Add(this.kryptonHeader3);
            this.kryptonGroup1.Panel.Controls.Add(this.cboTeller);
            this.kryptonGroup1.Panel.Controls.Add(this.btnGenerate);
            this.kryptonGroup1.Panel.Controls.Add(this.txtCash);
            this.kryptonGroup1.Panel.Controls.Add(this.txtTotalCollected);
            this.kryptonGroup1.Panel.Controls.Add(this.txtCredit);
            this.kryptonGroup1.Panel.Controls.Add(this.kryptonLabel5);
            this.kryptonGroup1.Panel.Controls.Add(this.txtDebit);
            this.kryptonGroup1.Panel.Controls.Add(this.dtpDate);
            this.kryptonGroup1.Panel.Controls.Add(this.txtCheck);
            this.kryptonGroup1.Panel.Controls.Add(this.kryptonLabel10);
            this.kryptonGroup1.Panel.Controls.Add(this.btnClose);
            this.kryptonGroup1.Panel.Controls.Add(this.kryptonLabel1);
            this.kryptonGroup1.Panel.Controls.Add(this.kryptonLabel2);
            this.kryptonGroup1.Panel.Controls.Add(this.kryptonLabel3);
            this.kryptonGroup1.Panel.Controls.Add(this.kryptonLabel6);
            this.kryptonGroup1.Size = new System.Drawing.Size(303, 482);
            this.kryptonGroup1.TabIndex = 56;
            // 
            // kryptonHeader3
            // 
            this.kryptonHeader3.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeader3.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader3.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeader3.Name = "kryptonHeader3";
            this.kryptonHeader3.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.kryptonHeader3.Size = new System.Drawing.Size(301, 22);
            this.kryptonHeader3.TabIndex = 53;
            this.kryptonHeader3.Text = "Collection";
            this.kryptonHeader3.Values.Description = "";
            this.kryptonHeader3.Values.Heading = "Collection";
            this.kryptonHeader3.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader3.Values.Image")));
            // 
            // kryptonLabel10
            // 
            this.kryptonLabel10.Location = new System.Drawing.Point(33, 253);
            this.kryptonLabel10.Name = "kryptonLabel10";
            this.kryptonLabel10.Size = new System.Drawing.Size(74, 20);
            this.kryptonLabel10.TabIndex = 51;
            this.kryptonLabel10.Text = "Total Check";
            this.kryptonLabel10.Values.ExtraText = "";
            this.kryptonLabel10.Values.Image = null;
            this.kryptonLabel10.Values.Text = "Total Check";
            // 
            // frmRCD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 528);
            this.ControlBox = false;
            this.Controls.Add(this.kryptonPanel1);
            this.MaximumSize = new System.Drawing.Size(652, 566);
            this.MinimumSize = new System.Drawing.Size(652, 566);
            this.Name = "frmRCD";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RCD / Partial Remitance";
            this.Load += new System.EventHandler(this.frmRCD_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup2.Panel)).EndInit();
            this.kryptonGroup2.Panel.ResumeLayout(false);
            this.kryptonGroup2.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup2)).EndInit();
            this.kryptonGroup2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgViewCoin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewPaper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1.Panel)).EndInit();
            this.kryptonGroup1.Panel.ResumeLayout(false);
            this.kryptonGroup1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).EndInit();
            this.kryptonGroup1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtGrandTotal;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCheck;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDebit;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalCollected;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCash;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCredit;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox cboTeller;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny btnPrev;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny btnNext;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel5;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel6;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel7;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel8;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel9;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader3;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader2;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel10;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalCoin;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalPaper;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView dgViewPaper;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView dgViewCoin;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonGroup1;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonGroup2;
    }
}