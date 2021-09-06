namespace Amellar.Common.User
{
    partial class frmUser
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnNew = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEdit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDelete = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDiscard = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPrint = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtUserCode = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtMI = new System.Windows.Forms.TextBox();
            this.txtPosition = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtConfirmPw = new System.Windows.Forms.TextBox();
            this.lblUserCode = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblMI = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblConfirmPw = new System.Windows.Forms.Label();
            this.lblPosition = new System.Windows.Forms.Label();
            this.lblDivision = new System.Windows.Forms.Label();
            this.cmbDivision = new System.Windows.Forms.ComboBox();
            this.dgvUserList = new System.Windows.Forms.DataGridView();
            this.btnGrantAll = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnRevoke = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.containerWithShadow3 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.cwsMain = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserList)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(663, 304);
            this.btnNew.Name = "btnNew";
            this.btnNew.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnNew.Size = new System.Drawing.Size(62, 25);
            this.btnNew.TabIndex = 1;
            this.btnNew.Text = "New";
            this.btnNew.Values.ExtraText = "";
            this.btnNew.Values.Image = null;
            this.btnNew.Values.ImageStates.ImageCheckedNormal = null;
            this.btnNew.Values.ImageStates.ImageCheckedPressed = null;
            this.btnNew.Values.ImageStates.ImageCheckedTracking = null;
            this.btnNew.Values.Text = "New";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(663, 335);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnEdit.Size = new System.Drawing.Size(62, 25);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit";
            this.btnEdit.Values.ExtraText = "";
            this.btnEdit.Values.Image = null;
            this.btnEdit.Values.ImageStates.ImageCheckedNormal = null;
            this.btnEdit.Values.ImageStates.ImageCheckedPressed = null;
            this.btnEdit.Values.ImageStates.ImageCheckedTracking = null;
            this.btnEdit.Values.Text = "Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(663, 366);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnDelete.Size = new System.Drawing.Size(62, 25);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Values.ExtraText = "";
            this.btnDelete.Values.Image = null;
            this.btnDelete.Values.ImageStates.ImageCheckedNormal = null;
            this.btnDelete.Values.ImageStates.ImageCheckedPressed = null;
            this.btnDelete.Values.ImageStates.ImageCheckedTracking = null;
            this.btnDelete.Values.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnDiscard
            // 
            this.btnDiscard.Location = new System.Drawing.Point(663, 397);
            this.btnDiscard.Name = "btnDiscard";
            this.btnDiscard.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnDiscard.Size = new System.Drawing.Size(62, 25);
            this.btnDiscard.TabIndex = 4;
            this.btnDiscard.Text = "Discard";
            this.btnDiscard.Values.ExtraText = "";
            this.btnDiscard.Values.Image = null;
            this.btnDiscard.Values.ImageStates.ImageCheckedNormal = null;
            this.btnDiscard.Values.ImageStates.ImageCheckedPressed = null;
            this.btnDiscard.Values.ImageStates.ImageCheckedTracking = null;
            this.btnDiscard.Values.Text = "Discard";
            this.btnDiscard.Click += new System.EventHandler(this.btnDiscard_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(663, 428);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint.Size = new System.Drawing.Size(62, 25);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "Print";
            this.btnPrint.Values.ExtraText = "";
            this.btnPrint.Values.Image = null;
            this.btnPrint.Values.ImageStates.ImageCheckedNormal = null;
            this.btnPrint.Values.ImageStates.ImageCheckedPressed = null;
            this.btnPrint.Values.ImageStates.ImageCheckedTracking = null;
            this.btnPrint.Values.Text = "Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(663, 459);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(62, 25);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtUserCode
            // 
            this.txtUserCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtUserCode.Location = new System.Drawing.Point(90, 304);
            this.txtUserCode.MaxLength = 20;
            this.txtUserCode.Name = "txtUserCode";
            this.txtUserCode.Size = new System.Drawing.Size(209, 20);
            this.txtUserCode.TabIndex = 7;
            // 
            // txtLastName
            // 
            this.txtLastName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLastName.Location = new System.Drawing.Point(90, 330);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(209, 20);
            this.txtLastName.TabIndex = 8;
            // 
            // txtFirstName
            // 
            this.txtFirstName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFirstName.Location = new System.Drawing.Point(90, 355);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(150, 20);
            this.txtFirstName.TabIndex = 9;
            // 
            // txtMI
            // 
            this.txtMI.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMI.Location = new System.Drawing.Point(273, 356);
            this.txtMI.MaxLength = 1;
            this.txtMI.Name = "txtMI";
            this.txtMI.Size = new System.Drawing.Size(26, 20);
            this.txtMI.TabIndex = 10;
            // 
            // txtPosition
            // 
            this.txtPosition.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPosition.Location = new System.Drawing.Point(90, 381);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(209, 20);
            this.txtPosition.TabIndex = 11;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(125, 441);
            this.txtPassword.MaxLength = 25;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(88, 20);
            this.txtPassword.TabIndex = 13;
            this.txtPassword.Leave += new System.EventHandler(this.txtPassword_Leave);
            // 
            // txtConfirmPw
            // 
            this.txtConfirmPw.Location = new System.Drawing.Point(125, 467);
            this.txtConfirmPw.MaxLength = 25;
            this.txtConfirmPw.Name = "txtConfirmPw";
            this.txtConfirmPw.PasswordChar = '*';
            this.txtConfirmPw.Size = new System.Drawing.Size(88, 20);
            this.txtConfirmPw.TabIndex = 14;
            // 
            // lblUserCode
            // 
            this.lblUserCode.AutoSize = true;
            this.lblUserCode.Location = new System.Drawing.Point(28, 311);
            this.lblUserCode.Name = "lblUserCode";
            this.lblUserCode.Size = new System.Drawing.Size(57, 13);
            this.lblUserCode.TabIndex = 11;
            this.lblUserCode.Text = "User Code";
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(28, 337);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(58, 13);
            this.lblLastName.TabIndex = 11;
            this.lblLastName.Text = "Last Name";
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(28, 362);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(57, 13);
            this.lblFirstName.TabIndex = 11;
            this.lblFirstName.Text = "First Name";
            // 
            // lblMI
            // 
            this.lblMI.AutoSize = true;
            this.lblMI.Location = new System.Drawing.Point(246, 362);
            this.lblMI.Name = "lblMI";
            this.lblMI.Size = new System.Drawing.Size(25, 13);
            this.lblMI.TabIndex = 11;
            this.lblMI.Text = "M.I.";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(28, 448);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 11;
            this.lblPassword.Text = "Password";
            // 
            // lblConfirmPw
            // 
            this.lblConfirmPw.AutoSize = true;
            this.lblConfirmPw.Location = new System.Drawing.Point(28, 474);
            this.lblConfirmPw.Name = "lblConfirmPw";
            this.lblConfirmPw.Size = new System.Drawing.Size(91, 13);
            this.lblConfirmPw.TabIndex = 11;
            this.lblConfirmPw.Text = "Confirm Password";
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Location = new System.Drawing.Point(28, 388);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(44, 13);
            this.lblPosition.TabIndex = 11;
            this.lblPosition.Text = "Position";
            // 
            // lblDivision
            // 
            this.lblDivision.AutoSize = true;
            this.lblDivision.Location = new System.Drawing.Point(28, 412);
            this.lblDivision.Name = "lblDivision";
            this.lblDivision.Size = new System.Drawing.Size(77, 13);
            this.lblDivision.TabIndex = 12;
            this.lblDivision.Text = "Office/Division";
            // 
            // cmbDivision
            // 
            this.cmbDivision.FormattingEnabled = true;
            this.cmbDivision.Location = new System.Drawing.Point(125, 407);
            this.cmbDivision.MaxDropDownItems = 20;
            this.cmbDivision.Name = "cmbDivision";
            this.cmbDivision.Size = new System.Drawing.Size(174, 21);
            this.cmbDivision.TabIndex = 12;
            // 
            // dgvUserList
            // 
            this.dgvUserList.AllowUserToAddRows = false;
            this.dgvUserList.AllowUserToDeleteRows = false;
            this.dgvUserList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUserList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvUserList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUserList.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvUserList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvUserList.Location = new System.Drawing.Point(31, 26);
            this.dgvUserList.Name = "dgvUserList";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUserList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvUserList.Size = new System.Drawing.Size(694, 235);
            this.dgvUserList.TabIndex = 17;
            this.dgvUserList.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvUserList_CellMouseClick);
            this.dgvUserList.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvUserList_CellMouseDoubleClick);
            this.dgvUserList.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUserList_CellEnter);
            this.dgvUserList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUserList_CellContentClick);
            // 
            // btnGrantAll
            // 
            this.btnGrantAll.Location = new System.Drawing.Point(225, 436);
            this.btnGrantAll.Name = "btnGrantAll";
            this.btnGrantAll.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGrantAll.Size = new System.Drawing.Size(74, 25);
            this.btnGrantAll.TabIndex = 15;
            this.btnGrantAll.Text = "Grant All";
            this.btnGrantAll.Values.ExtraText = "";
            this.btnGrantAll.Values.Image = null;
            this.btnGrantAll.Values.ImageStates.ImageCheckedNormal = null;
            this.btnGrantAll.Values.ImageStates.ImageCheckedPressed = null;
            this.btnGrantAll.Values.ImageStates.ImageCheckedTracking = null;
            this.btnGrantAll.Values.Text = "Grant All";
            this.btnGrantAll.Click += new System.EventHandler(this.btnGrantAll_Click);
            // 
            // btnRevoke
            // 
            this.btnRevoke.Location = new System.Drawing.Point(225, 467);
            this.btnRevoke.Name = "btnRevoke";
            this.btnRevoke.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnRevoke.Size = new System.Drawing.Size(74, 25);
            this.btnRevoke.TabIndex = 16;
            this.btnRevoke.Text = "Revoke All";
            this.btnRevoke.Values.ExtraText = "";
            this.btnRevoke.Values.Image = null;
            this.btnRevoke.Values.ImageStates.ImageCheckedNormal = null;
            this.btnRevoke.Values.ImageStates.ImageCheckedPressed = null;
            this.btnRevoke.Values.ImageStates.ImageCheckedTracking = null;
            this.btnRevoke.Values.Text = "Revoke All";
            this.btnRevoke.Click += new System.EventHandler(this.btnRevoke_Click);
            // 
            // containerWithShadow3
            // 
            this.containerWithShadow3.Location = new System.Drawing.Point(646, 290);
            this.containerWithShadow3.Name = "containerWithShadow3";
            this.containerWithShadow3.Size = new System.Drawing.Size(95, 216);
            this.containerWithShadow3.TabIndex = 2;
            // 
            // cwsMain
            // 
            this.cwsMain.Location = new System.Drawing.Point(12, 290);
            this.cwsMain.Name = "cwsMain";
            this.cwsMain.Size = new System.Drawing.Size(301, 216);
            this.cwsMain.TabIndex = 1;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(12, 12);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(729, 272);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // frmUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(742, 506);
            this.ControlBox = false;
            this.Controls.Add(this.btnRevoke);
            this.Controls.Add(this.btnGrantAll);
            this.Controls.Add(this.dgvUserList);
            this.Controls.Add(this.cmbDivision);
            this.Controls.Add(this.lblDivision);
            this.Controls.Add(this.lblPosition);
            this.Controls.Add(this.lblConfirmPw);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblMI);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.lblUserCode);
            this.Controls.Add(this.txtConfirmPw);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtPosition);
            this.Controls.Add(this.txtMI);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.txtUserCode);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnDiscard);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.containerWithShadow3);
            this.Controls.Add(this.cwsMain);
            this.Controls.Add(this.containerWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUser";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Users";
            this.Load += new System.EventHandler(this.frmUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow cwsMain;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow3;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnNew;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEdit;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDelete;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDiscard;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPrint;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private System.Windows.Forms.TextBox txtUserCode;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtMI;
        private System.Windows.Forms.TextBox txtPosition;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtConfirmPw;
        private System.Windows.Forms.Label lblUserCode;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblMI;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblConfirmPw;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Label lblDivision;
        private System.Windows.Forms.ComboBox cmbDivision;
        private System.Windows.Forms.DataGridView dgvUserList;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGrantAll;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnRevoke;
    }
}