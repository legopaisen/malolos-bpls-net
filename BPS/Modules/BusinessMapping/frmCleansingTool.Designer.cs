namespace Amellar.Modules.BusinessMapping
{
    partial class frmCleansingTool
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
            this.dgvOfficialList = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtOwnCode = new System.Windows.Forms.TextBox();
            this.txtBnsStat = new System.Windows.Forms.TextBox();
            this.txtTaxYear = new System.Windows.Forms.TextBox();
            this.txtPermitNo = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.bin1 = new Amellar.Common.BIN.BIN();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOwnAdd = new System.Windows.Forms.TextBox();
            this.txtBnsAdd = new System.Windows.Forms.TextBox();
            this.txtOwnNm = new System.Windows.Forms.TextBox();
            this.txtBnsNm = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoOwnName = new System.Windows.Forms.RadioButton();
            this.rdoBnsName = new System.Windows.Forms.RadioButton();
            this.rdoPermitNo = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUnofficalOwnAdd = new System.Windows.Forms.TextBox();
            this.txtUnofficalBnsAdd = new System.Windows.Forms.TextBox();
            this.txtUnofficalOwnNm = new System.Windows.Forms.TextBox();
            this.txtTempBIN = new System.Windows.Forms.TextBox();
            this.txtUnofficalBnsNm = new System.Windows.Forms.TextBox();
            this.dgvUnofficialList = new System.Windows.Forms.DataGridView();
            this.btnMatch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPrint = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOfficialList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnofficialList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvOfficialList
            // 
            this.dgvOfficialList.AllowUserToAddRows = false;
            this.dgvOfficialList.AllowUserToDeleteRows = false;
            this.dgvOfficialList.AllowUserToResizeRows = false;
            this.dgvOfficialList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOfficialList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvOfficialList.Location = new System.Drawing.Point(6, 19);
            this.dgvOfficialList.Name = "dgvOfficialList";
            this.dgvOfficialList.Size = new System.Drawing.Size(649, 135);
            this.dgvOfficialList.TabIndex = 3;
            this.dgvOfficialList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOfficialList_CellClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtOwnCode);
            this.groupBox1.Controls.Add(this.txtBnsStat);
            this.groupBox1.Controls.Add(this.txtTaxYear);
            this.groupBox1.Controls.Add(this.txtPermitNo);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.bin1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtOwnAdd);
            this.groupBox1.Controls.Add(this.txtBnsAdd);
            this.groupBox1.Controls.Add(this.txtOwnNm);
            this.groupBox1.Controls.Add(this.txtBnsNm);
            this.groupBox1.Controls.Add(this.dgvOfficialList);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(661, 241);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Official Businesses ";
            // 
            // txtOwnCode
            // 
            this.txtOwnCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOwnCode.Location = new System.Drawing.Point(92, 211);
            this.txtOwnCode.Name = "txtOwnCode";
            this.txtOwnCode.ReadOnly = true;
            this.txtOwnCode.Size = new System.Drawing.Size(56, 20);
            this.txtOwnCode.TabIndex = 15;
            // 
            // txtBnsStat
            // 
            this.txtBnsStat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBnsStat.Location = new System.Drawing.Point(592, 165);
            this.txtBnsStat.Name = "txtBnsStat";
            this.txtBnsStat.ReadOnly = true;
            this.txtBnsStat.Size = new System.Drawing.Size(63, 20);
            this.txtBnsStat.TabIndex = 14;
            // 
            // txtTaxYear
            // 
            this.txtTaxYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTaxYear.Location = new System.Drawing.Point(435, 165);
            this.txtTaxYear.Name = "txtTaxYear";
            this.txtTaxYear.ReadOnly = true;
            this.txtTaxYear.Size = new System.Drawing.Size(64, 20);
            this.txtTaxYear.TabIndex = 14;
            // 
            // txtPermitNo
            // 
            this.txtPermitNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPermitNo.Location = new System.Drawing.Point(275, 165);
            this.txtPermitNo.Name = "txtPermitNo";
            this.txtPermitNo.ReadOnly = true;
            this.txtPermitNo.Size = new System.Drawing.Size(100, 20);
            this.txtPermitNo.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(505, 172);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Business Status";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(381, 172);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Tax Year";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(213, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Permit No.";
            // 
            // bin1
            // 
            this.bin1.Enabled = false;
            this.bin1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(37, 165);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(138, 20);
            this.bin1.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 218);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Owner\'s Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Business Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 172);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "BIN";
            // 
            // txtOwnAdd
            // 
            this.txtOwnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOwnAdd.Location = new System.Drawing.Point(381, 211);
            this.txtOwnAdd.Multiline = true;
            this.txtOwnAdd.Name = "txtOwnAdd";
            this.txtOwnAdd.ReadOnly = true;
            this.txtOwnAdd.Size = new System.Drawing.Size(274, 20);
            this.txtOwnAdd.TabIndex = 12;
            // 
            // txtBnsAdd
            // 
            this.txtBnsAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBnsAdd.Location = new System.Drawing.Point(381, 188);
            this.txtBnsAdd.Multiline = true;
            this.txtBnsAdd.Name = "txtBnsAdd";
            this.txtBnsAdd.ReadOnly = true;
            this.txtBnsAdd.Size = new System.Drawing.Size(274, 20);
            this.txtBnsAdd.TabIndex = 10;
            // 
            // txtOwnNm
            // 
            this.txtOwnNm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOwnNm.Location = new System.Drawing.Point(154, 211);
            this.txtOwnNm.Multiline = true;
            this.txtOwnNm.Name = "txtOwnNm";
            this.txtOwnNm.ReadOnly = true;
            this.txtOwnNm.Size = new System.Drawing.Size(221, 20);
            this.txtOwnNm.TabIndex = 11;
            // 
            // txtBnsNm
            // 
            this.txtBnsNm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBnsNm.Location = new System.Drawing.Point(92, 188);
            this.txtBnsNm.Multiline = true;
            this.txtBnsNm.Name = "txtBnsNm";
            this.txtBnsNm.ReadOnly = true;
            this.txtBnsNm.Size = new System.Drawing.Size(283, 20);
            this.txtBnsNm.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdoOwnName);
            this.groupBox2.Controls.Add(this.rdoBnsName);
            this.groupBox2.Controls.Add(this.rdoPermitNo);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(661, 44);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Match by: ";
            // 
            // rdoOwnName
            // 
            this.rdoOwnName.AutoSize = true;
            this.rdoOwnName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoOwnName.Location = new System.Drawing.Point(361, 19);
            this.rdoOwnName.Name = "rdoOwnName";
            this.rdoOwnName.Size = new System.Drawing.Size(94, 17);
            this.rdoOwnName.TabIndex = 2;
            this.rdoOwnName.TabStop = true;
            this.rdoOwnName.Text = "Owner\'s Name";
            this.rdoOwnName.UseVisualStyleBackColor = true;
            this.rdoOwnName.CheckedChanged += new System.EventHandler(this.rdoOwnName_CheckedChanged);
            // 
            // rdoBnsName
            // 
            this.rdoBnsName.AutoSize = true;
            this.rdoBnsName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoBnsName.Location = new System.Drawing.Point(222, 19);
            this.rdoBnsName.Name = "rdoBnsName";
            this.rdoBnsName.Size = new System.Drawing.Size(98, 17);
            this.rdoBnsName.TabIndex = 1;
            this.rdoBnsName.TabStop = true;
            this.rdoBnsName.Text = "Business Name";
            this.rdoBnsName.UseVisualStyleBackColor = true;
            this.rdoBnsName.CheckedChanged += new System.EventHandler(this.rdoBnsName_CheckedChanged);
            // 
            // rdoPermitNo
            // 
            this.rdoPermitNo.AutoSize = true;
            this.rdoPermitNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoPermitNo.Location = new System.Drawing.Point(92, 19);
            this.rdoPermitNo.Name = "rdoPermitNo";
            this.rdoPermitNo.Size = new System.Drawing.Size(74, 17);
            this.rdoPermitNo.TabIndex = 0;
            this.rdoPermitNo.TabStop = true;
            this.rdoPermitNo.Text = "Permit No.";
            this.rdoPermitNo.UseVisualStyleBackColor = true;
            this.rdoPermitNo.CheckedChanged += new System.EventHandler(this.rdoPermitNo_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txtUnofficalOwnAdd);
            this.groupBox3.Controls.Add(this.txtUnofficalBnsAdd);
            this.groupBox3.Controls.Add(this.txtUnofficalOwnNm);
            this.groupBox3.Controls.Add(this.txtTempBIN);
            this.groupBox3.Controls.Add(this.txtUnofficalBnsNm);
            this.groupBox3.Controls.Add(this.dgvUnofficialList);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(12, 317);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(661, 232);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = " Un-Official Businesses tagged";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 212);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Owner\'s Name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 166);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Temporary BIN";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Business Name";
            // 
            // txtUnofficalOwnAdd
            // 
            this.txtUnofficalOwnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUnofficalOwnAdd.Location = new System.Drawing.Point(381, 205);
            this.txtUnofficalOwnAdd.Multiline = true;
            this.txtUnofficalOwnAdd.Name = "txtUnofficalOwnAdd";
            this.txtUnofficalOwnAdd.ReadOnly = true;
            this.txtUnofficalOwnAdd.Size = new System.Drawing.Size(274, 20);
            this.txtUnofficalOwnAdd.TabIndex = 16;
            // 
            // txtUnofficalBnsAdd
            // 
            this.txtUnofficalBnsAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUnofficalBnsAdd.Location = new System.Drawing.Point(381, 182);
            this.txtUnofficalBnsAdd.Multiline = true;
            this.txtUnofficalBnsAdd.Name = "txtUnofficalBnsAdd";
            this.txtUnofficalBnsAdd.ReadOnly = true;
            this.txtUnofficalBnsAdd.Size = new System.Drawing.Size(274, 20);
            this.txtUnofficalBnsAdd.TabIndex = 14;
            // 
            // txtUnofficalOwnNm
            // 
            this.txtUnofficalOwnNm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUnofficalOwnNm.Location = new System.Drawing.Point(92, 205);
            this.txtUnofficalOwnNm.Multiline = true;
            this.txtUnofficalOwnNm.Name = "txtUnofficalOwnNm";
            this.txtUnofficalOwnNm.ReadOnly = true;
            this.txtUnofficalOwnNm.Size = new System.Drawing.Size(283, 20);
            this.txtUnofficalOwnNm.TabIndex = 15;
            // 
            // txtTempBIN
            // 
            this.txtTempBIN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTempBIN.Location = new System.Drawing.Point(92, 159);
            this.txtTempBIN.Multiline = true;
            this.txtTempBIN.Name = "txtTempBIN";
            this.txtTempBIN.ReadOnly = true;
            this.txtTempBIN.Size = new System.Drawing.Size(283, 20);
            this.txtTempBIN.TabIndex = 13;
            // 
            // txtUnofficalBnsNm
            // 
            this.txtUnofficalBnsNm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUnofficalBnsNm.Location = new System.Drawing.Point(92, 182);
            this.txtUnofficalBnsNm.Multiline = true;
            this.txtUnofficalBnsNm.Name = "txtUnofficalBnsNm";
            this.txtUnofficalBnsNm.ReadOnly = true;
            this.txtUnofficalBnsNm.Size = new System.Drawing.Size(283, 20);
            this.txtUnofficalBnsNm.TabIndex = 13;
            // 
            // dgvUnofficialList
            // 
            this.dgvUnofficialList.AllowUserToAddRows = false;
            this.dgvUnofficialList.AllowUserToDeleteRows = false;
            this.dgvUnofficialList.AllowUserToResizeRows = false;
            this.dgvUnofficialList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUnofficialList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvUnofficialList.Location = new System.Drawing.Point(6, 19);
            this.dgvUnofficialList.Name = "dgvUnofficialList";
            this.dgvUnofficialList.Size = new System.Drawing.Size(649, 135);
            this.dgvUnofficialList.TabIndex = 4;
            this.dgvUnofficialList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnofficialList_CellClick);
            // 
            // btnMatch
            // 
            this.btnMatch.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnMatch.Location = new System.Drawing.Point(480, 554);
            this.btnMatch.Name = "btnMatch";
            this.btnMatch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnMatch.Size = new System.Drawing.Size(92, 25);
            this.btnMatch.TabIndex = 5;
            this.btnMatch.Values.Text = "Match";
            this.btnMatch.Click += new System.EventHandler(this.btnMatch_Click);
            // 
            // btnClose
            // 
            this.btnClose.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnClose.Location = new System.Drawing.Point(580, 554);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(92, 25);
            this.btnClose.TabIndex = 6;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnPrint.Location = new System.Drawing.Point(21, 555);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint.Size = new System.Drawing.Size(212, 25);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Values.Text = "Print matching by Business Name";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // frmCleansingTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 589);
            this.ControlBox = false;
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnMatch);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmCleansingTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Business Mapping Cleansing Tool";
            this.Load += new System.EventHandler(this.frmCleansingTool_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOfficialList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnofficialList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvOfficialList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoBnsName;
        private System.Windows.Forms.RadioButton rdoPermitNo;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvUnofficialList;
        private Amellar.Common.BIN.BIN bin1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBnsAdd;
        private System.Windows.Forms.TextBox txtBnsNm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtOwnAdd;
        private System.Windows.Forms.TextBox txtOwnNm;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtUnofficalOwnAdd;
        private System.Windows.Forms.TextBox txtUnofficalBnsAdd;
        private System.Windows.Forms.TextBox txtUnofficalOwnNm;
        private System.Windows.Forms.TextBox txtUnofficalBnsNm;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnMatch;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private System.Windows.Forms.RadioButton rdoOwnName;
        private System.Windows.Forms.TextBox txtPermitNo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtOwnCode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTempBIN;
        private System.Windows.Forms.TextBox txtBnsStat;
        private System.Windows.Forms.TextBox txtTaxYear;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnPrint;
    }
}