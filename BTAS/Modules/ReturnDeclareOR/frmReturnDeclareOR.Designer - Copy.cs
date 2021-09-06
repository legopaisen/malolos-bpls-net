namespace ReturnDeclareOR
{
    partial class frmReturnDeclareOR
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
            this.dgvTellerOR = new System.Windows.Forms.DataGridView();
            this.teller = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.from = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.to = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date_ass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ass_by = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTellerCode = new System.Windows.Forms.TextBox();
            this.txtLName = new System.Windows.Forms.TextBox();
            this.txtFName = new System.Windows.Forms.TextBox();
            this.txtMi = new System.Windows.Forms.TextBox();
            this.kryptonHeader3 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.txtCurrOr = new System.Windows.Forms.TextBox();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblCurrOr = new System.Windows.Forms.Label();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDeclare = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnReturn = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.frameWithShadow3 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow2 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow1 = new Amellar.Common.SOA.FrameWithShadow();
            this.btnClear = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtSeries = new System.Windows.Forms.TextBox();
            this.lblSeries = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTellerOR)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTellerOR
            // 
            this.dgvTellerOR.AllowUserToAddRows = false;
            this.dgvTellerOR.AllowUserToDeleteRows = false;
            this.dgvTellerOR.AllowUserToResizeColumns = false;
            this.dgvTellerOR.AllowUserToResizeRows = false;
            this.dgvTellerOR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTellerOR.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.teller,
            this.from,
            this.to,
            this.date_ass,
            this.ass_by});
            this.dgvTellerOR.Location = new System.Drawing.Point(28, 48);
            this.dgvTellerOR.Name = "dgvTellerOR";
            this.dgvTellerOR.RowHeadersVisible = false;
            this.dgvTellerOR.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTellerOR.Size = new System.Drawing.Size(552, 149);
            this.dgvTellerOR.TabIndex = 1;
            this.dgvTellerOR.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTellerOR_RowEnter);
            this.dgvTellerOR.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTellerOR_CellClick);
            // 
            // teller
            // 
            this.teller.HeaderText = "TELLER";
            this.teller.Name = "teller";
            // 
            // from
            // 
            this.from.HeaderText = "FROM";
            this.from.Name = "from";
            // 
            // to
            // 
            this.to.HeaderText = "TO";
            this.to.Name = "to";
            // 
            // date_ass
            // 
            this.date_ass.HeaderText = "DATE ASSIGNED";
            this.date_ass.Name = "date_ass";
            this.date_ass.Width = 120;
            // 
            // ass_by
            // 
            this.ass_by.HeaderText = "ASSIGNED BY";
            this.ass_by.Name = "ass_by";
            this.ass_by.Width = 110;
            // 
            // kryptonHeader2
            // 
            this.kryptonHeader2.AutoSize = false;
            this.kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader2.Location = new System.Drawing.Point(28, 23);
            this.kryptonHeader2.Name = "kryptonHeader2";
            this.kryptonHeader2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonHeader2.Size = new System.Drawing.Size(552, 22);
            this.kryptonHeader2.TabIndex = 53;
            this.kryptonHeader2.Values.Description = "";
            this.kryptonHeader2.Values.Heading = "List of Tellers and Official Receipt Assignments";
            this.kryptonHeader2.Values.Image = null;
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader1.Location = new System.Drawing.Point(28, 227);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonHeader1.Size = new System.Drawing.Size(552, 22);
            this.kryptonHeader1.TabIndex = 55;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Teller Information";
            this.kryptonHeader1.Values.Image = null;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 264);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 56;
            this.label1.Text = "Teller Code:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 290);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 57;
            this.label2.Text = "Last Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 316);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 58;
            this.label3.Text = "First Name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(486, 316);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 59;
            this.label4.Text = "MI:";
            // 
            // txtTellerCode
            // 
            this.txtTellerCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTellerCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTellerCode.Location = new System.Drawing.Point(95, 259);
            this.txtTellerCode.Name = "txtTellerCode";
            this.txtTellerCode.Size = new System.Drawing.Size(168, 22);
            this.txtTellerCode.TabIndex = 2;
            // 
            // txtLName
            // 
            this.txtLName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLName.Enabled = false;
            this.txtLName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLName.Location = new System.Drawing.Point(95, 286);
            this.txtLName.Name = "txtLName";
            this.txtLName.Size = new System.Drawing.Size(485, 22);
            this.txtLName.TabIndex = 61;
            // 
            // txtFName
            // 
            this.txtFName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFName.Enabled = false;
            this.txtFName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFName.Location = new System.Drawing.Point(95, 313);
            this.txtFName.Name = "txtFName";
            this.txtFName.Size = new System.Drawing.Size(385, 22);
            this.txtFName.TabIndex = 62;
            // 
            // txtMi
            // 
            this.txtMi.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMi.Enabled = false;
            this.txtMi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMi.Location = new System.Drawing.Point(514, 313);
            this.txtMi.Name = "txtMi";
            this.txtMi.Size = new System.Drawing.Size(66, 22);
            this.txtMi.TabIndex = 63;
            // 
            // kryptonHeader3
            // 
            this.kryptonHeader3.AutoSize = false;
            this.kryptonHeader3.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader3.Location = new System.Drawing.Point(28, 370);
            this.kryptonHeader3.Name = "kryptonHeader3";
            this.kryptonHeader3.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonHeader3.Size = new System.Drawing.Size(552, 22);
            this.kryptonHeader3.TabIndex = 65;
            this.kryptonHeader3.Values.Description = "";
            this.kryptonHeader3.Values.Heading = "Official Receipt Information";
            this.kryptonHeader3.Values.Image = null;
            // 
            // txtFrom
            // 
            this.txtFrom.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFrom.Location = new System.Drawing.Point(102, 400);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(152, 22);
            this.txtFrom.TabIndex = 6;
            this.txtFrom.TextChanged += new System.EventHandler(this.txtFrom_TextChanged);
            // 
            // txtTo
            // 
            this.txtTo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTo.Location = new System.Drawing.Point(269, 400);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(152, 22);
            this.txtTo.TabIndex = 7;
            // 
            // txtCurrOr
            // 
            this.txtCurrOr.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCurrOr.Enabled = false;
            this.txtCurrOr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrOr.Location = new System.Drawing.Point(428, 400);
            this.txtCurrOr.Name = "txtCurrOr";
            this.txtCurrOr.Size = new System.Drawing.Size(152, 22);
            this.txtCurrOr.TabIndex = 68;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(166, 425);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(30, 13);
            this.lblFrom.TabIndex = 69;
            this.lblFrom.Text = "From";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(333, 425);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(20, 13);
            this.lblTo.TabIndex = 70;
            this.lblTo.Text = "To";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurrOr
            // 
            this.lblCurrOr.AutoSize = true;
            this.lblCurrOr.Location = new System.Drawing.Point(473, 425);
            this.lblCurrOr.Name = "lblCurrOr";
            this.lblCurrOr.Size = new System.Drawing.Size(60, 13);
            this.lblCurrOr.TabIndex = 71;
            this.lblCurrOr.Text = "Current OR";
            this.lblCurrOr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(269, 255);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(84, 27);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnDeclare
            // 
            this.btnDeclare.Enabled = false;
            this.btnDeclare.Location = new System.Drawing.Point(12, 467);
            this.btnDeclare.Name = "btnDeclare";
            this.btnDeclare.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnDeclare.Size = new System.Drawing.Size(84, 27);
            this.btnDeclare.TabIndex = 8;
            this.btnDeclare.Values.Text = "Declare";
            this.btnDeclare.Click += new System.EventHandler(this.btnDeclare_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.Enabled = false;
            this.btnReturn.Location = new System.Drawing.Point(102, 467);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnReturn.Size = new System.Drawing.Size(84, 27);
            this.btnReturn.TabIndex = 9;
            this.btnReturn.Values.Text = "Return";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(514, 467);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(84, 27);
            this.btnClose.TabIndex = 10;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frameWithShadow3
            // 
            this.frameWithShadow3.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow3.Location = new System.Drawing.Point(10, 359);
            this.frameWithShadow3.Name = "frameWithShadow3";
            this.frameWithShadow3.Size = new System.Drawing.Size(588, 97);
            this.frameWithShadow3.TabIndex = 64;
            // 
            // frameWithShadow2
            // 
            this.frameWithShadow2.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow2.Location = new System.Drawing.Point(10, 216);
            this.frameWithShadow2.Name = "frameWithShadow2";
            this.frameWithShadow2.Size = new System.Drawing.Size(588, 136);
            this.frameWithShadow2.TabIndex = 54;
            // 
            // frameWithShadow1
            // 
            this.frameWithShadow1.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow1.Location = new System.Drawing.Point(10, 12);
            this.frameWithShadow1.Name = "frameWithShadow1";
            this.frameWithShadow1.Size = new System.Drawing.Size(588, 200);
            this.frameWithShadow1.TabIndex = 0;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(359, 255);
            this.btnClear.Name = "btnClear";
            this.btnClear.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClear.Size = new System.Drawing.Size(84, 27);
            this.btnClear.TabIndex = 4;
            this.btnClear.Values.Text = "Clear";
            this.btnClear.Visible = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtSeries
            // 
            this.txtSeries.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtSeries.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSeries.Location = new System.Drawing.Point(28, 400);
            this.txtSeries.MaxLength = 2;
            this.txtSeries.Name = "txtSeries";
            this.txtSeries.Size = new System.Drawing.Size(58, 22);
            this.txtSeries.TabIndex = 5;
            // 
            // lblSeries
            // 
            this.lblSeries.AutoSize = true;
            this.lblSeries.Location = new System.Drawing.Point(37, 425);
            this.lblSeries.Name = "lblSeries";
            this.lblSeries.Size = new System.Drawing.Size(36, 13);
            this.lblSeries.TabIndex = 78;
            this.lblSeries.Text = "Series";
            this.lblSeries.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmReturnDeclareOR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(610, 508);
            this.Controls.Add(this.lblSeries);
            this.Controls.Add(this.txtSeries);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnDeclare);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lblCurrOr);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.txtCurrOr);
            this.Controls.Add(this.txtTo);
            this.Controls.Add(this.txtFrom);
            this.Controls.Add(this.kryptonHeader3);
            this.Controls.Add(this.frameWithShadow3);
            this.Controls.Add(this.txtMi);
            this.Controls.Add(this.txtFName);
            this.Controls.Add(this.txtLName);
            this.Controls.Add(this.txtTellerCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.frameWithShadow2);
            this.Controls.Add(this.kryptonHeader2);
            this.Controls.Add(this.dgvTellerOR);
            this.Controls.Add(this.frameWithShadow1);
            this.Name = "frmReturnDeclareOR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Declare / Return Official Receipts Number";
            this.Load += new System.EventHandler(this.frmReturnDeclareOR_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTellerOR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.SOA.FrameWithShadow frameWithShadow1;
        private System.Windows.Forms.DataGridView dgvTellerOR;
        private System.Windows.Forms.DataGridViewTextBoxColumn teller;
        private System.Windows.Forms.DataGridViewTextBoxColumn from;
        private System.Windows.Forms.DataGridViewTextBoxColumn to;
        private System.Windows.Forms.DataGridViewTextBoxColumn date_ass;
        private System.Windows.Forms.DataGridViewTextBoxColumn ass_by;
        public ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader2;
        public ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTellerCode;
        private System.Windows.Forms.TextBox txtLName;
        private System.Windows.Forms.TextBox txtFName;
        private System.Windows.Forms.TextBox txtMi;
        public ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader3;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow3;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.TextBox txtTo;
        private System.Windows.Forms.TextBox txtCurrOr;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblCurrOr;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDeclare;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnReturn;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClear;
        private System.Windows.Forms.TextBox txtSeries;
        private System.Windows.Forms.Label lblSeries;
    }
}

