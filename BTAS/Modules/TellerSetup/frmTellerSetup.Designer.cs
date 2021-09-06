namespace TellerSetup
{
    partial class frmTellerSetup
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
            this.kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.dgvTellerList = new System.Windows.Forms.DataGridView();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTellerCode = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtFName = new System.Windows.Forms.TextBox();
            this.txtMi = new System.Windows.Forms.TextBox();
            this.txtPosition = new System.Windows.Forms.TextBox();
            this.txtORCode = new System.Windows.Forms.TextBox();
            this.kryptonHeader3 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnAdd = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEdit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDelete = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPrint = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblCounter = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRcdCode = new System.Windows.Forms.TextBox();
            this.frameWithShadow3 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow2 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow1 = new Amellar.Common.SOA.FrameWithShadow();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.l_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rcd_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTellerList)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonHeader2
            // 
            this.kryptonHeader2.AutoSize = false;
            this.kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader2.Location = new System.Drawing.Point(19, 18);
            this.kryptonHeader2.Name = "kryptonHeader2";
            this.kryptonHeader2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonHeader2.Size = new System.Drawing.Size(627, 22);
            this.kryptonHeader2.TabIndex = 52;
            this.kryptonHeader2.Text = "Teller List";
            this.kryptonHeader2.Values.Description = "";
            this.kryptonHeader2.Values.Heading = "Teller List";
            this.kryptonHeader2.Values.Image = null;
            // 
            // dgvTellerList
            // 
            this.dgvTellerList.AllowUserToAddRows = false;
            this.dgvTellerList.AllowUserToDeleteRows = false;
            this.dgvTellerList.AllowUserToResizeColumns = false;
            this.dgvTellerList.AllowUserToResizeRows = false;
            this.dgvTellerList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTellerList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.code,
            this.l_name,
            this.f_name,
            this.mi,
            this.pos,
            this.rcd_code});
            this.dgvTellerList.Location = new System.Drawing.Point(23, 43);
            this.dgvTellerList.Name = "dgvTellerList";
            this.dgvTellerList.ReadOnly = true;
            this.dgvTellerList.RowHeadersVisible = false;
            this.dgvTellerList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvTellerList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTellerList.Size = new System.Drawing.Size(619, 155);
            this.dgvTellerList.TabIndex = 53;
            this.dgvTellerList.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTellerList_RowEnter);
            this.dgvTellerList.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTellerList_CellEnter);
            this.dgvTellerList.Click += new System.EventHandler(this.dgvTellerList_Click);
            this.dgvTellerList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTellerList_CellContentClick);
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader1.Location = new System.Drawing.Point(19, 258);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonHeader1.Size = new System.Drawing.Size(627, 22);
            this.kryptonHeader1.TabIndex = 55;
            this.kryptonHeader1.Text = "Teller Information";
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Teller Information";
            this.kryptonHeader1.Values.Image = null;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 295);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 56;
            this.label1.Text = "Teller Code:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 323);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 57;
            this.label2.Text = "Last Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 374);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 59;
            this.label3.Text = "Position:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 347);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 58;
            this.label4.Text = "First Name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(572, 351);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 13);
            this.label5.TabIndex = 61;
            this.label5.Text = "MI:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(497, 295);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 60;
            this.label6.Text = "OR Code:";
            this.label6.Visible = false;
            // 
            // txtTellerCode
            // 
            this.txtTellerCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTellerCode.Enabled = false;
            this.txtTellerCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTellerCode.Location = new System.Drawing.Point(92, 293);
            this.txtTellerCode.MaxLength = 10;
            this.txtTellerCode.Name = "txtTellerCode";
            this.txtTellerCode.Size = new System.Drawing.Size(117, 22);
            this.txtTellerCode.TabIndex = 1;
            // 
            // txtLastName
            // 
            this.txtLastName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLastName.Enabled = false;
            this.txtLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastName.Location = new System.Drawing.Point(92, 319);
            this.txtLastName.MaxLength = 20;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(550, 22);
            this.txtLastName.TabIndex = 3;
            // 
            // txtFName
            // 
            this.txtFName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFName.Enabled = false;
            this.txtFName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFName.Location = new System.Drawing.Point(92, 345);
            this.txtFName.MaxLength = 30;
            this.txtFName.Name = "txtFName";
            this.txtFName.Size = new System.Drawing.Size(474, 22);
            this.txtFName.TabIndex = 4;
            // 
            // txtMi
            // 
            this.txtMi.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMi.Enabled = false;
            this.txtMi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMi.Location = new System.Drawing.Point(593, 345);
            this.txtMi.MaxLength = 1;
            this.txtMi.Name = "txtMi";
            this.txtMi.Size = new System.Drawing.Size(49, 22);
            this.txtMi.TabIndex = 5;
            // 
            // txtPosition
            // 
            this.txtPosition.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPosition.Enabled = false;
            this.txtPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPosition.Location = new System.Drawing.Point(92, 371);
            this.txtPosition.MaxLength = 30;
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(550, 22);
            this.txtPosition.TabIndex = 6;
            // 
            // txtORCode
            // 
            this.txtORCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtORCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtORCode.Location = new System.Drawing.Point(550, 293);
            this.txtORCode.MaxLength = 3;
            this.txtORCode.Name = "txtORCode";
            this.txtORCode.ReadOnly = true;
            this.txtORCode.Size = new System.Drawing.Size(92, 22);
            this.txtORCode.TabIndex = 67;
            this.txtORCode.Visible = false;
            // 
            // kryptonHeader3
            // 
            this.kryptonHeader3.AutoSize = false;
            this.kryptonHeader3.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Form;
            this.kryptonHeader3.Location = new System.Drawing.Point(19, 429);
            this.kryptonHeader3.Name = "kryptonHeader3";
            this.kryptonHeader3.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003;
            this.kryptonHeader3.Size = new System.Drawing.Size(627, 22);
            this.kryptonHeader3.TabIndex = 69;
            this.kryptonHeader3.Text = "Teller\'s Password";
            this.kryptonHeader3.Values.Description = "";
            this.kryptonHeader3.Values.Heading = "Teller\'s Password";
            this.kryptonHeader3.Values.Image = null;
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(92, 466);
            this.txtPassword.MaxLength = 20;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(184, 22);
            this.txtPassword.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 468);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 70;
            this.label7.Text = "Password:";
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.Enabled = false;
            this.txtConfirmPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmPassword.Location = new System.Drawing.Point(444, 466);
            this.txtConfirmPassword.MaxLength = 20;
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.PasswordChar = '*';
            this.txtConfirmPassword.Size = new System.Drawing.Size(200, 22);
            this.txtConfirmPassword.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(345, 468);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 13);
            this.label8.TabIndex = 72;
            this.label8.Text = "Confirm Password:";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(264, 515);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnAdd.Size = new System.Drawing.Size(73, 30);
            this.btnAdd.TabIndex = 74;
            this.btnAdd.Text = "&Add";
            this.btnAdd.Values.ExtraText = "";
            this.btnAdd.Values.Image = null;
            this.btnAdd.Values.ImageStates.ImageCheckedNormal = null;
            this.btnAdd.Values.ImageStates.ImageCheckedPressed = null;
            this.btnAdd.Values.ImageStates.ImageCheckedTracking = null;
            this.btnAdd.Values.Text = "&Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(343, 515);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnEdit.Size = new System.Drawing.Size(73, 30);
            this.btnEdit.TabIndex = 75;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.Values.ExtraText = "";
            this.btnEdit.Values.Image = null;
            this.btnEdit.Values.ImageStates.ImageCheckedNormal = null;
            this.btnEdit.Values.ImageStates.ImageCheckedPressed = null;
            this.btnEdit.Values.ImageStates.ImageCheckedTracking = null;
            this.btnEdit.Values.Text = "&Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(422, 515);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnDelete.Size = new System.Drawing.Size(73, 30);
            this.btnDelete.TabIndex = 76;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.Values.ExtraText = "";
            this.btnDelete.Values.Image = null;
            this.btnDelete.Values.ImageStates.ImageCheckedNormal = null;
            this.btnDelete.Values.ImageStates.ImageCheckedPressed = null;
            this.btnDelete.Values.ImageStates.ImageCheckedTracking = null;
            this.btnDelete.Values.Text = "&Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(501, 515);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint.Size = new System.Drawing.Size(73, 30);
            this.btnPrint.TabIndex = 77;
            this.btnPrint.Text = "&Print";
            this.btnPrint.Values.ExtraText = "";
            this.btnPrint.Values.Image = null;
            this.btnPrint.Values.ImageStates.ImageCheckedNormal = null;
            this.btnPrint.Values.ImageStates.ImageCheckedPressed = null;
            this.btnPrint.Values.ImageStates.ImageCheckedTracking = null;
            this.btnPrint.Values.Text = "&Print";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(580, 515);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(73, 30);
            this.btnClose.TabIndex = 78;
            this.btnClose.Text = "C&lose";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "C&lose";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblCounter
            // 
            this.lblCounter.AutoSize = true;
            this.lblCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCounter.ForeColor = System.Drawing.Color.Blue;
            this.lblCounter.Location = new System.Drawing.Point(479, 217);
            this.lblCounter.Name = "lblCounter";
            this.lblCounter.Size = new System.Drawing.Size(0, 16);
            this.lblCounter.TabIndex = 79;
            this.lblCounter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(222, 298);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 13);
            this.label9.TabIndex = 80;
            this.label9.Text = "RCD Code:";
            // 
            // txtRcdCode
            // 
            this.txtRcdCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRcdCode.Enabled = false;
            this.txtRcdCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRcdCode.Location = new System.Drawing.Point(299, 293);
            this.txtRcdCode.MaxLength = 10;
            this.txtRcdCode.Name = "txtRcdCode";
            this.txtRcdCode.Size = new System.Drawing.Size(117, 22);
            this.txtRcdCode.TabIndex = 2;
            this.txtRcdCode.Leave += new System.EventHandler(this.txtRcdCode_Leave);
            // 
            // frameWithShadow3
            // 
            this.frameWithShadow3.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow3.Location = new System.Drawing.Point(5, 423);
            this.frameWithShadow3.Name = "frameWithShadow3";
            this.frameWithShadow3.Size = new System.Drawing.Size(649, 85);
            this.frameWithShadow3.TabIndex = 68;
            // 
            // frameWithShadow2
            // 
            this.frameWithShadow2.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow2.Location = new System.Drawing.Point(8, 251);
            this.frameWithShadow2.Name = "frameWithShadow2";
            this.frameWithShadow2.Size = new System.Drawing.Size(649, 166);
            this.frameWithShadow2.TabIndex = 54;
            // 
            // frameWithShadow1
            // 
            this.frameWithShadow1.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow1.Location = new System.Drawing.Point(8, 12);
            this.frameWithShadow1.Name = "frameWithShadow1";
            this.frameWithShadow1.Size = new System.Drawing.Size(649, 234);
            this.frameWithShadow1.TabIndex = 0;
            // 
            // code
            // 
            this.code.HeaderText = "CODE";
            this.code.Name = "code";
            this.code.ReadOnly = true;
            this.code.Width = 80;
            // 
            // l_name
            // 
            this.l_name.HeaderText = "LAST NAME";
            this.l_name.Name = "l_name";
            this.l_name.ReadOnly = true;
            this.l_name.Width = 130;
            // 
            // f_name
            // 
            this.f_name.HeaderText = "FIRST NAME";
            this.f_name.Name = "f_name";
            this.f_name.ReadOnly = true;
            this.f_name.Width = 125;
            // 
            // mi
            // 
            this.mi.HeaderText = "MI";
            this.mi.Name = "mi";
            this.mi.ReadOnly = true;
            this.mi.Width = 30;
            // 
            // pos
            // 
            this.pos.HeaderText = "POSITION";
            this.pos.Name = "pos";
            this.pos.ReadOnly = true;
            this.pos.Width = 150;
            // 
            // rcd_code
            // 
            this.rcd_code.HeaderText = "RCD CODE";
            this.rcd_code.Name = "rcd_code";
            this.rcd_code.ReadOnly = true;
            // 
            // frmTellerSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(665, 558);
            this.Controls.Add(this.txtRcdCode);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblCounter);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.kryptonHeader3);
            this.Controls.Add(this.frameWithShadow3);
            this.Controls.Add(this.txtORCode);
            this.Controls.Add(this.txtPosition);
            this.Controls.Add(this.txtMi);
            this.Controls.Add(this.txtFName);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.txtTellerCode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.kryptonHeader1);
            this.Controls.Add(this.frameWithShadow2);
            this.Controls.Add(this.dgvTellerList);
            this.Controls.Add(this.kryptonHeader2);
            this.Controls.Add(this.frameWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTellerSetup";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Teller Setup";
            this.Load += new System.EventHandler(this.frmTellerSetup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTellerList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.SOA.FrameWithShadow frameWithShadow1;
        public ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader2;
        private System.Windows.Forms.DataGridView dgvTellerList;
        public ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTellerCode;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtFName;
        private System.Windows.Forms.TextBox txtMi;
        private System.Windows.Forms.TextBox txtPosition;
        private System.Windows.Forms.TextBox txtORCode;
        public ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader3;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow3;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Label label8;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAdd;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEdit;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDelete;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPrint;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private System.Windows.Forms.Label lblCounter;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtRcdCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn l_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn f_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn mi;
        private System.Windows.Forms.DataGridViewTextBoxColumn pos;
        private System.Windows.Forms.DataGridViewTextBoxColumn rcd_code;
    }
}

