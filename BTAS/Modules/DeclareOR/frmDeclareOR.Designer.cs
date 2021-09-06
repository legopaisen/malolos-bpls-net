namespace DeclareOR
{
    partial class frmDeclareOR
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
            this.dgvOR = new System.Windows.Forms.DataGridView();
            this.teller = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.from = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.to = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date_assigned = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assigned_by = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.txtTellerCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLn = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFn = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMi = new System.Windows.Forms.TextBox();
            this.kryptonHeader3 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.txtCurrOr = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDeclare = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnReturn = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOR)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvOR
            // 
            this.dgvOR.AllowUserToAddRows = false;
            this.dgvOR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOR.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.teller,
            this.from,
            this.to,
            this.date_assigned,
            this.assigned_by});
            this.dgvOR.Location = new System.Drawing.Point(30, 48);
            this.dgvOR.Name = "dgvOR";
            this.dgvOR.RowHeadersVisible = false;
            this.dgvOR.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOR.Size = new System.Drawing.Size(521, 117);
            this.dgvOR.TabIndex = 1;
            this.dgvOR.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOR_CellClick);
            // 
            // teller
            // 
            this.teller.HeaderText = "Teller";
            this.teller.Name = "teller";
            // 
            // from
            // 
            this.from.HeaderText = "From";
            this.from.Name = "from";
            // 
            // to
            // 
            this.to.HeaderText = "To";
            this.to.Name = "to";
            // 
            // date_assigned
            // 
            this.date_assigned.HeaderText = "Date Assigned";
            this.date_assigned.Name = "date_assigned";
            // 
            // assigned_by
            // 
            this.assigned_by.HeaderText = "Assigned By";
            this.assigned_by.Name = "assigned_by";
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(17, 9);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleOrange;
            this.kryptonHeader1.Size = new System.Drawing.Size(555, 24);
            this.kryptonHeader1.TabIndex = 3;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Declared ORs";
            // 
            // kryptonHeader2
            // 
            this.kryptonHeader2.AutoSize = false;
            this.kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader2.Location = new System.Drawing.Point(14, 182);
            this.kryptonHeader2.Name = "kryptonHeader2";
            this.kryptonHeader2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleOrange;
            this.kryptonHeader2.Size = new System.Drawing.Size(555, 24);
            this.kryptonHeader2.TabIndex = 4;
            this.kryptonHeader2.Values.Description = "";
            this.kryptonHeader2.Values.Heading = "Teller";
            // 
            // txtTellerCode
            // 
            this.txtTellerCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTellerCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTellerCode.Location = new System.Drawing.Point(94, 228);
            this.txtTellerCode.Name = "txtTellerCode";
            this.txtTellerCode.Size = new System.Drawing.Size(119, 22);
            this.txtTellerCode.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 233);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Teller Code:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 261);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Last Name:";
            // 
            // txtLn
            // 
            this.txtLn.Enabled = false;
            this.txtLn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLn.Location = new System.Drawing.Point(94, 256);
            this.txtLn.Name = "txtLn";
            this.txtLn.Size = new System.Drawing.Size(454, 22);
            this.txtLn.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 289);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "First Name:";
            // 
            // txtFn
            // 
            this.txtFn.Enabled = false;
            this.txtFn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFn.Location = new System.Drawing.Point(94, 284);
            this.txtFn.Name = "txtFn";
            this.txtFn.Size = new System.Drawing.Size(391, 22);
            this.txtFn.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(494, 289);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "MI:";
            // 
            // txtMi
            // 
            this.txtMi.Enabled = false;
            this.txtMi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMi.Location = new System.Drawing.Point(522, 284);
            this.txtMi.Name = "txtMi";
            this.txtMi.Size = new System.Drawing.Size(29, 22);
            this.txtMi.TabIndex = 12;
            // 
            // kryptonHeader3
            // 
            this.kryptonHeader3.AutoSize = false;
            this.kryptonHeader3.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader3.Location = new System.Drawing.Point(14, 333);
            this.kryptonHeader3.Name = "kryptonHeader3";
            this.kryptonHeader3.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleOrange;
            this.kryptonHeader3.Size = new System.Drawing.Size(555, 24);
            this.kryptonHeader3.TabIndex = 14;
            this.kryptonHeader3.Values.Description = "";
            this.kryptonHeader3.Values.Heading = "OR";
            // 
            // txtFrom
            // 
            this.txtFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFrom.Location = new System.Drawing.Point(38, 378);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(147, 22);
            this.txtFrom.TabIndex = 15;
            // 
            // txtTo
            // 
            this.txtTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTo.Location = new System.Drawing.Point(219, 378);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(147, 22);
            this.txtTo.TabIndex = 16;
            // 
            // txtCurrOr
            // 
            this.txtCurrOr.Enabled = false;
            this.txtCurrOr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrOr.Location = new System.Drawing.Point(400, 378);
            this.txtCurrOr.Name = "txtCurrOr";
            this.txtCurrOr.Size = new System.Drawing.Size(147, 22);
            this.txtCurrOr.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(91, 403);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "FROM";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(286, 417);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "TO";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(434, 403);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "CURRENT OR";
            // 
            // btnDeclare
            // 
            this.btnDeclare.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnDeclare.Enabled = false;
            this.btnDeclare.Location = new System.Drawing.Point(25, 452);
            this.btnDeclare.Name = "btnDeclare";
            this.btnDeclare.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleOrange;
            this.btnDeclare.Size = new System.Drawing.Size(106, 27);
            this.btnDeclare.TabIndex = 21;
            this.btnDeclare.Values.Text = "Declare";
            this.btnDeclare.Click += new System.EventHandler(this.btnDeclare_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnReturn.Enabled = false;
            this.btnReturn.Location = new System.Drawing.Point(152, 452);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleOrange;
            this.btnReturn.Size = new System.Drawing.Size(106, 27);
            this.btnReturn.TabIndex = 22;
            this.btnReturn.Values.Text = "Return";
            // 
            // btnClose
            // 
            this.btnClose.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnClose.Location = new System.Drawing.Point(465, 452);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleOrange;
            this.btnClose.Size = new System.Drawing.Size(106, 27);
            this.btnClose.TabIndex = 23;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnSearch.Location = new System.Drawing.Point(219, 223);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleOrange;
            this.btnSearch.Size = new System.Drawing.Size(106, 27);
            this.btnSearch.TabIndex = 24;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // frmDeclareOR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 494);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnDeclare);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCurrOr);
            this.Controls.Add(this.txtTo);
            this.Controls.Add(this.txtFrom);
            this.Controls.Add(this.kryptonHeader3);
            this.Controls.Add(this.txtMi);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtFn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTellerCode);
            this.Controls.Add(this.kryptonHeader2);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.dgvOR);
            this.Name = "frmDeclareOR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OR Declaration / Cancelation";
            this.Load += new System.EventHandler(this.frmDeclareOR_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn teller;
        private System.Windows.Forms.DataGridViewTextBoxColumn from;
        private System.Windows.Forms.DataGridViewTextBoxColumn to;
        private System.Windows.Forms.DataGridViewTextBoxColumn date_assigned;
        private System.Windows.Forms.DataGridViewTextBoxColumn assigned_by;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader2;
        private System.Windows.Forms.TextBox txtTellerCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMi;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader3;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.TextBox txtTo;
        private System.Windows.Forms.TextBox txtCurrOr;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDeclare;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnReturn;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        public System.Windows.Forms.DataGridView dgvOR;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
    }
}

