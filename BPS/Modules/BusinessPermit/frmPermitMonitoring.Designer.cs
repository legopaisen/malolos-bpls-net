namespace Amellar.Modules.BusinessPermit
{
    partial class frmPermitMonitoring
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.TaxYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bns_nm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.own_nm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.permit_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Released_to = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rel_dt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rel_tm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recvd_by = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OwnCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTaxyear = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.txtReleasedBy = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnTag = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.txtRcvdBy = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtOwnName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBnsTaxYear = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPermitNo = new System.Windows.Forms.TextBox();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.bin1 = new Amellar.Common.BIN.BIN();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnRemove = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnPrint);
            this.groupBox1.Controls.Add(this.btnGenerate);
            this.groupBox1.Controls.Add(this.dgvList);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtTaxyear);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(727, 302);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Released Permit ";
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(648, 26);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint.Size = new System.Drawing.Size(68, 25);
            this.btnPrint.TabIndex = 15;
            this.btnPrint.Text = "Print";
            this.btnPrint.Values.ExtraText = "";
            this.btnPrint.Values.Image = null;
            this.btnPrint.Values.ImageStates.ImageCheckedNormal = null;
            this.btnPrint.Values.ImageStates.ImageCheckedPressed = null;
            this.btnPrint.Values.ImageStates.ImageCheckedTracking = null;
            this.btnPrint.Values.Text = "Print";
            this.btnPrint.Visible = false;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(183, 23);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(68, 25);
            this.btnGenerate.TabIndex = 14;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.Values.ExtraText = "";
            this.btnGenerate.Values.Image = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedNormal = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedPressed = null;
            this.btnGenerate.Values.ImageStates.ImageCheckedTracking = null;
            this.btnGenerate.Values.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToResizeRows = false;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TaxYear,
            this.bin,
            this.bns_nm,
            this.own_nm,
            this.permit_no,
            this.Released_to,
            this.rel_dt,
            this.rel_tm,
            this.recvd_by,
            this.OwnCode});
            this.dgvList.Location = new System.Drawing.Point(8, 56);
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.Size = new System.Drawing.Size(710, 239);
            this.dgvList.TabIndex = 13;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // TaxYear
            // 
            this.TaxYear.HeaderText = "Tax Year";
            this.TaxYear.Name = "TaxYear";
            this.TaxYear.ReadOnly = true;
            this.TaxYear.Width = 80;
            // 
            // bin
            // 
            this.bin.HeaderText = "BIN";
            this.bin.Name = "bin";
            this.bin.ReadOnly = true;
            this.bin.Width = 120;
            // 
            // bns_nm
            // 
            this.bns_nm.HeaderText = "Business Name";
            this.bns_nm.Name = "bns_nm";
            this.bns_nm.ReadOnly = true;
            this.bns_nm.Width = 200;
            // 
            // own_nm
            // 
            this.own_nm.HeaderText = "Owner\'s Name";
            this.own_nm.Name = "own_nm";
            this.own_nm.ReadOnly = true;
            this.own_nm.Width = 150;
            // 
            // permit_no
            // 
            this.permit_no.HeaderText = "Permit No.";
            this.permit_no.Name = "permit_no";
            this.permit_no.ReadOnly = true;
            // 
            // Released_to
            // 
            this.Released_to.HeaderText = "Released To";
            this.Released_to.Name = "Released_to";
            this.Released_to.ReadOnly = true;
            // 
            // rel_dt
            // 
            this.rel_dt.HeaderText = "Released Date";
            this.rel_dt.Name = "rel_dt";
            this.rel_dt.ReadOnly = true;
            // 
            // rel_tm
            // 
            this.rel_tm.HeaderText = "Released Time";
            this.rel_tm.Name = "rel_tm";
            this.rel_tm.ReadOnly = true;
            // 
            // recvd_by
            // 
            this.recvd_by.HeaderText = "Received By";
            this.recvd_by.Name = "recvd_by";
            this.recvd_by.ReadOnly = true;
            // 
            // OwnCode
            // 
            this.OwnCode.HeaderText = "OwnCode";
            this.OwnCode.Name = "OwnCode";
            this.OwnCode.ReadOnly = true;
            this.OwnCode.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tax Year:";
            // 
            // txtTaxyear
            // 
            this.txtTaxyear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTaxyear.Location = new System.Drawing.Point(89, 28);
            this.txtTaxyear.MaxLength = 4;
            this.txtTaxyear.Name = "txtTaxyear";
            this.txtTaxyear.Size = new System.Drawing.Size(79, 20);
            this.txtTaxyear.TabIndex = 0;
            this.txtTaxyear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnRemove);
            this.groupBox2.Controls.Add(this.txtTime);
            this.groupBox2.Controls.Add(this.txtReleasedBy);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.btnTag);
            this.groupBox2.Controls.Add(this.dtpTime);
            this.groupBox2.Controls.Add(this.dtpDate);
            this.groupBox2.Controls.Add(this.txtRcvdBy);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.txtOwnName);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtBnsTaxYear);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtPermitNo);
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Controls.Add(this.bin1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtBnsName);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 320);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(727, 199);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tagging";
            // 
            // txtTime
            // 
            this.txtTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTime.Location = new System.Drawing.Point(615, 139);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(69, 20);
            this.txtTime.TabIndex = 44;
            this.txtTime.Visible = false;
            // 
            // txtReleasedBy
            // 
            this.txtReleasedBy.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtReleasedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReleasedBy.Location = new System.Drawing.Point(158, 167);
            this.txtReleasedBy.Name = "txtReleasedBy";
            this.txtReleasedBy.Size = new System.Drawing.Size(388, 20);
            this.txtReleasedBy.TabIndex = 43;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(32, 174);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 13);
            this.label7.TabIndex = 42;
            this.label7.Text = "Permit Released By:";
            // 
            // btnTag
            // 
            this.btnTag.Location = new System.Drawing.Point(562, 164);
            this.btnTag.Name = "btnTag";
            this.btnTag.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnTag.Size = new System.Drawing.Size(101, 25);
            this.btnTag.TabIndex = 41;
            this.btnTag.Text = "Tag Permit";
            this.btnTag.Values.ExtraText = "";
            this.btnTag.Values.Image = null;
            this.btnTag.Values.ImageStates.ImageCheckedNormal = null;
            this.btnTag.Values.ImageStates.ImageCheckedPressed = null;
            this.btnTag.Values.ImageStates.ImageCheckedTracking = null;
            this.btnTag.Values.Text = "Tag Permit";
            this.btnTag.Click += new System.EventHandler(this.btnTag_Click);
            // 
            // dtpTime
            // 
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpTime.Location = new System.Drawing.Point(281, 139);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.Size = new System.Drawing.Size(117, 20);
            this.dtpTime.TabIndex = 40;
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(158, 139);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(117, 20);
            this.dtpDate.TabIndex = 39;
            // 
            // txtRcvdBy
            // 
            this.txtRcvdBy.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRcvdBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRcvdBy.Location = new System.Drawing.Point(158, 112);
            this.txtRcvdBy.Name = "txtRcvdBy";
            this.txtRcvdBy.Size = new System.Drawing.Size(388, 20);
            this.txtRcvdBy.TabIndex = 38;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(32, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 13);
            this.label6.TabIndex = 37;
            this.label6.Text = "Released Date && Time:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(32, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "Permit Released To:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(32, 90);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(76, 13);
            this.label17.TabIndex = 35;
            this.label17.Text = "Owner\'s Name";
            // 
            // txtOwnName
            // 
            this.txtOwnName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOwnName.Location = new System.Drawing.Point(118, 83);
            this.txtOwnName.Name = "txtOwnName";
            this.txtOwnName.ReadOnly = true;
            this.txtOwnName.Size = new System.Drawing.Size(428, 20);
            this.txtOwnName.TabIndex = 34;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(290, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Tax Year";
            // 
            // txtBnsTaxYear
            // 
            this.txtBnsTaxYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBnsTaxYear.Location = new System.Drawing.Point(346, 30);
            this.txtBnsTaxYear.Name = "txtBnsTaxYear";
            this.txtBnsTaxYear.ReadOnly = true;
            this.txtBnsTaxYear.Size = new System.Drawing.Size(69, 20);
            this.txtBnsTaxYear.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(432, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Permit No.";
            // 
            // txtPermitNo
            // 
            this.txtPermitNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPermitNo.Location = new System.Drawing.Point(494, 30);
            this.txtPermitNo.Name = "txtPermitNo";
            this.txtPermitNo.ReadOnly = true;
            this.txtPermitNo.Size = new System.Drawing.Size(118, 20);
            this.txtPermitNo.TabIndex = 6;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(200, 26);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(68, 25);
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
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(35, 28);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(167, 23);
            this.bin1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(32, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Business Name";
            // 
            // txtBnsName
            // 
            this.txtBnsName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBnsName.Location = new System.Drawing.Point(118, 60);
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(428, 20);
            this.txtBnsName.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "BIN";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 120;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Business Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 200;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Owner\'s Name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Permit No.";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Released Date";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Released Time";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Received By";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(627, 525);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(101, 25);
            this.btnClose.TabIndex = 44;
            this.btnClose.Text = "Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(620, 26);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnRemove.Size = new System.Drawing.Size(101, 25);
            this.btnRemove.TabIndex = 45;
            this.btnRemove.Text = "Remove Permit";
            this.btnRemove.Values.ExtraText = "";
            this.btnRemove.Values.Image = null;
            this.btnRemove.Values.ImageStates.ImageCheckedNormal = null;
            this.btnRemove.Values.ImageStates.ImageCheckedPressed = null;
            this.btnRemove.Values.ImageStates.ImageCheckedTracking = null;
            this.btnRemove.Values.Text = "Remove Permit";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // frmPermitMonitoring
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 557);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmPermitMonitoring";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Permit Monitoring";
            this.Load += new System.EventHandler(this.frmPermitMonitoring_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTaxyear;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBnsName;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private Amellar.Common.BIN.BIN bin1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPermitNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBnsTaxYear;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtOwnName;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnTag;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.TextBox txtRcvdBy;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPrint;
        private System.Windows.Forms.TextBox txtReleasedBy;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn bin;
        private System.Windows.Forms.DataGridViewTextBoxColumn bns_nm;
        private System.Windows.Forms.DataGridViewTextBoxColumn own_nm;
        private System.Windows.Forms.DataGridViewTextBoxColumn permit_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn Released_to;
        private System.Windows.Forms.DataGridViewTextBoxColumn rel_dt;
        private System.Windows.Forms.DataGridViewTextBoxColumn rel_tm;
        private System.Windows.Forms.DataGridViewTextBoxColumn recvd_by;
        private System.Windows.Forms.DataGridViewTextBoxColumn OwnCode;
        private System.Windows.Forms.TextBox txtTime;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnRemove;
    }
}