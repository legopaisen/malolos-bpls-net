namespace Amellar.Modules.ApplicationRequirements
{
    partial class frmAppRequirement
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
            this.chkNew = new System.Windows.Forms.CheckBox();
            this.chkRenewal = new System.Windows.Forms.CheckBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.btnSave = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.grpBnsStat = new System.Windows.Forms.GroupBox();
            this.btnEdit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDelete = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.grpBnsOrg = new System.Windows.Forms.GroupBox();
            this.chkOrgCoop = new System.Windows.Forms.CheckBox();
            this.chkOrgCorp = new System.Windows.Forms.CheckBox();
            this.chkOrgSingle = new System.Windows.Forms.CheckBox();
            this.chkOrgPartner = new System.Windows.Forms.CheckBox();
            this.grpBnsNtr = new System.Windows.Forms.GroupBox();
            this.chkNtrSpecific = new System.Windows.Forms.CheckBox();
            this.chkNtrAll = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow3 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.chkOtherGross = new System.Windows.Forms.CheckBox();
            this.chkOtherPeza = new System.Windows.Forms.CheckBox();
            this.chkOtherBoi = new System.Windows.Forms.CheckBox();
            this.txtOtherGross = new System.Windows.Forms.TextBox();
            this.txtOthers = new System.Windows.Forms.TextBox();
            this.grpOther = new System.Windows.Forms.GroupBox();
            this.chkMalolos = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.grpBnsStat.SuspendLayout();
            this.grpBnsOrg.SuspendLayout();
            this.grpBnsNtr.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.grpOther.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkNew
            // 
            this.chkNew.AutoSize = true;
            this.chkNew.Location = new System.Drawing.Point(19, 35);
            this.chkNew.Name = "chkNew";
            this.chkNew.Size = new System.Drawing.Size(48, 17);
            this.chkNew.TabIndex = 1;
            this.chkNew.Text = "New";
            this.chkNew.UseVisualStyleBackColor = true;
            // 
            // chkRenewal
            // 
            this.chkRenewal.AutoSize = true;
            this.chkRenewal.Location = new System.Drawing.Point(19, 58);
            this.chkRenewal.Name = "chkRenewal";
            this.chkRenewal.Size = new System.Drawing.Size(68, 17);
            this.chkRenewal.TabIndex = 2;
            this.chkRenewal.Text = "Renewal";
            this.chkRenewal.UseVisualStyleBackColor = true;
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(21, 20);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(46, 20);
            this.txtCode.TabIndex = 4;
            // 
            // txtDesc
            // 
            this.txtDesc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtDesc.Location = new System.Drawing.Point(73, 20);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.ReadOnly = true;
            this.txtDesc.Size = new System.Drawing.Size(489, 20);
            this.txtDesc.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(270, 396);
            this.btnSave.Name = "btnSave";
            this.btnSave.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSave.Size = new System.Drawing.Size(82, 24);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Add";
            this.btnSave.Values.ExtraText = "";
            this.btnSave.Values.Image = null;
            this.btnSave.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSave.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSave.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSave.Values.Text = "Add";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(534, 396);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(82, 24);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Close";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvList.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvList.Location = new System.Drawing.Point(18, 15);
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.Size = new System.Drawing.Size(598, 155);
            this.dgvList.TabIndex = 101;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // grpBnsStat
            // 
            this.grpBnsStat.Controls.Add(this.chkRenewal);
            this.grpBnsStat.Controls.Add(this.chkNew);
            this.grpBnsStat.Enabled = false;
            this.grpBnsStat.Location = new System.Drawing.Point(24, 192);
            this.grpBnsStat.Name = "grpBnsStat";
            this.grpBnsStat.Size = new System.Drawing.Size(110, 111);
            this.grpBnsStat.TabIndex = 102;
            this.grpBnsStat.TabStop = false;
            this.grpBnsStat.Text = " Business Status ";
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(358, 396);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnEdit.Size = new System.Drawing.Size(82, 24);
            this.btnEdit.TabIndex = 5;
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
            this.btnDelete.Location = new System.Drawing.Point(446, 396);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnDelete.Size = new System.Drawing.Size(82, 24);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Values.ExtraText = "";
            this.btnDelete.Values.Image = null;
            this.btnDelete.Values.ImageStates.ImageCheckedNormal = null;
            this.btnDelete.Values.ImageStates.ImageCheckedPressed = null;
            this.btnDelete.Values.ImageStates.ImageCheckedTracking = null;
            this.btnDelete.Values.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // grpBnsOrg
            // 
            this.grpBnsOrg.Controls.Add(this.chkOrgCoop);
            this.grpBnsOrg.Controls.Add(this.chkOrgCorp);
            this.grpBnsOrg.Controls.Add(this.chkOrgSingle);
            this.grpBnsOrg.Controls.Add(this.chkOrgPartner);
            this.grpBnsOrg.Enabled = false;
            this.grpBnsOrg.Location = new System.Drawing.Point(283, 192);
            this.grpBnsOrg.Name = "grpBnsOrg";
            this.grpBnsOrg.Size = new System.Drawing.Size(141, 111);
            this.grpBnsOrg.TabIndex = 102;
            this.grpBnsOrg.TabStop = false;
            this.grpBnsOrg.Text = " Organization Kind ";
            // 
            // chkOrgCoop
            // 
            this.chkOrgCoop.AutoSize = true;
            this.chkOrgCoop.Location = new System.Drawing.Point(16, 88);
            this.chkOrgCoop.Name = "chkOrgCoop";
            this.chkOrgCoop.Size = new System.Drawing.Size(83, 17);
            this.chkOrgCoop.TabIndex = 3;
            this.chkOrgCoop.Text = "Cooperative";
            this.chkOrgCoop.UseVisualStyleBackColor = true;
            // 
            // chkOrgCorp
            // 
            this.chkOrgCorp.AutoSize = true;
            this.chkOrgCorp.Location = new System.Drawing.Point(16, 65);
            this.chkOrgCorp.Name = "chkOrgCorp";
            this.chkOrgCorp.Size = new System.Drawing.Size(80, 17);
            this.chkOrgCorp.TabIndex = 2;
            this.chkOrgCorp.Text = "Corporation";
            this.chkOrgCorp.UseVisualStyleBackColor = true;
            // 
            // chkOrgSingle
            // 
            this.chkOrgSingle.AutoSize = true;
            this.chkOrgSingle.Location = new System.Drawing.Point(16, 19);
            this.chkOrgSingle.Name = "chkOrgSingle";
            this.chkOrgSingle.Size = new System.Drawing.Size(122, 17);
            this.chkOrgSingle.TabIndex = 2;
            this.chkOrgSingle.Text = "Single Proprietorship";
            this.chkOrgSingle.UseVisualStyleBackColor = true;
            // 
            // chkOrgPartner
            // 
            this.chkOrgPartner.AutoSize = true;
            this.chkOrgPartner.Location = new System.Drawing.Point(16, 42);
            this.chkOrgPartner.Name = "chkOrgPartner";
            this.chkOrgPartner.Size = new System.Drawing.Size(79, 17);
            this.chkOrgPartner.TabIndex = 2;
            this.chkOrgPartner.Text = "Partnership";
            this.chkOrgPartner.UseVisualStyleBackColor = true;
            // 
            // grpBnsNtr
            // 
            this.grpBnsNtr.Controls.Add(this.chkNtrSpecific);
            this.grpBnsNtr.Controls.Add(this.chkNtrAll);
            this.grpBnsNtr.Enabled = false;
            this.grpBnsNtr.Location = new System.Drawing.Point(145, 192);
            this.grpBnsNtr.Name = "grpBnsNtr";
            this.grpBnsNtr.Size = new System.Drawing.Size(128, 111);
            this.grpBnsNtr.TabIndex = 102;
            this.grpBnsNtr.TabStop = false;
            this.grpBnsNtr.Text = " Nature of Business ";
            // 
            // chkNtrSpecific
            // 
            this.chkNtrSpecific.AutoSize = true;
            this.chkNtrSpecific.Location = new System.Drawing.Point(15, 58);
            this.chkNtrSpecific.Name = "chkNtrSpecific";
            this.chkNtrSpecific.Size = new System.Drawing.Size(108, 17);
            this.chkNtrSpecific.TabIndex = 2;
            this.chkNtrSpecific.Text = "Specific Nature...";
            this.chkNtrSpecific.UseVisualStyleBackColor = true;
            this.chkNtrSpecific.CheckStateChanged += new System.EventHandler(this.chkNtrSpecific_CheckStateChanged);
            this.chkNtrSpecific.CheckedChanged += new System.EventHandler(this.chkNtrSpecific_CheckedChanged);
            // 
            // chkNtrAll
            // 
            this.chkNtrAll.AutoSize = true;
            this.chkNtrAll.Location = new System.Drawing.Point(15, 35);
            this.chkNtrAll.Name = "chkNtrAll";
            this.chkNtrAll.Size = new System.Drawing.Size(37, 17);
            this.chkNtrAll.TabIndex = 1;
            this.chkNtrAll.Text = "All";
            this.chkNtrAll.UseVisualStyleBackColor = true;
            this.chkNtrAll.CheckStateChanged += new System.EventHandler(this.chkNtrAll_CheckStateChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtCode);
            this.groupBox5.Controls.Add(this.txtDesc);
            this.groupBox5.Location = new System.Drawing.Point(24, 322);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(580, 54);
            this.groupBox5.TabIndex = 104;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = " Description ";
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(5, 185);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(625, 132);
            this.containerWithShadow2.TabIndex = 0;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(5, 5);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(625, 181);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // containerWithShadow3
            // 
            this.containerWithShadow3.Location = new System.Drawing.Point(5, 316);
            this.containerWithShadow3.Name = "containerWithShadow3";
            this.containerWithShadow3.Size = new System.Drawing.Size(625, 77);
            this.containerWithShadow3.TabIndex = 103;
            // 
            // chkOtherGross
            // 
            this.chkOtherGross.AutoSize = true;
            this.chkOtherGross.Location = new System.Drawing.Point(14, 19);
            this.chkOtherGross.Name = "chkOtherGross";
            this.chkOtherGross.Size = new System.Drawing.Size(62, 17);
            this.chkOtherGross.TabIndex = 1;
            this.chkOtherGross.Text = "Gross >";
            this.chkOtherGross.UseVisualStyleBackColor = true;
            this.chkOtherGross.CheckStateChanged += new System.EventHandler(this.chkOtherGross_CheckStateChanged);
            // 
            // chkOtherPeza
            // 
            this.chkOtherPeza.AutoSize = true;
            this.chkOtherPeza.Location = new System.Drawing.Point(14, 42);
            this.chkOtherPeza.Name = "chkOtherPeza";
            this.chkOtherPeza.Size = new System.Drawing.Size(95, 17);
            this.chkOtherPeza.TabIndex = 2;
            this.chkOtherPeza.Text = "PEZA Member";
            this.chkOtherPeza.UseVisualStyleBackColor = true;
            // 
            // chkOtherBoi
            // 
            this.chkOtherBoi.AutoSize = true;
            this.chkOtherBoi.Location = new System.Drawing.Point(14, 65);
            this.chkOtherBoi.Name = "chkOtherBoi";
            this.chkOtherBoi.Size = new System.Drawing.Size(85, 17);
            this.chkOtherBoi.TabIndex = 2;
            this.chkOtherBoi.Text = "BOI Member";
            this.chkOtherBoi.UseVisualStyleBackColor = true;
            // 
            // txtOtherGross
            // 
            this.txtOtherGross.Location = new System.Drawing.Point(78, 16);
            this.txtOtherGross.MaxLength = 100;
            this.txtOtherGross.Name = "txtOtherGross";
            this.txtOtherGross.ReadOnly = true;
            this.txtOtherGross.Size = new System.Drawing.Size(98, 20);
            this.txtOtherGross.TabIndex = 3;
            // 
            // txtOthers
            // 
            this.txtOthers.Location = new System.Drawing.Point(78, 85);
            this.txtOthers.MaxLength = 100;
            this.txtOthers.Name = "txtOthers";
            this.txtOthers.ReadOnly = true;
            this.txtOthers.Size = new System.Drawing.Size(98, 20);
            this.txtOthers.TabIndex = 5;
            // 
            // grpOther
            // 
            this.grpOther.Controls.Add(this.txtOthers);
            this.grpOther.Controls.Add(this.chkMalolos);
            this.grpOther.Controls.Add(this.txtOtherGross);
            this.grpOther.Controls.Add(this.chkOtherBoi);
            this.grpOther.Controls.Add(this.chkOtherPeza);
            this.grpOther.Controls.Add(this.chkOtherGross);
            this.grpOther.Enabled = false;
            this.grpOther.Location = new System.Drawing.Point(434, 192);
            this.grpOther.Name = "grpOther";
            this.grpOther.Size = new System.Drawing.Size(182, 111);
            this.grpOther.TabIndex = 102;
            this.grpOther.TabStop = false;
            this.grpOther.Text = " Other Variables";
            // 
            // chkMalolos
            // 
            this.chkMalolos.AutoSize = true;
            this.chkMalolos.Location = new System.Drawing.Point(14, 88);
            this.chkMalolos.Name = "chkMalolos";
            this.chkMalolos.Size = new System.Drawing.Size(57, 17);
            this.chkMalolos.TabIndex = 4;
            this.chkMalolos.Text = "Others";
            this.chkMalolos.UseVisualStyleBackColor = true;
            this.chkMalolos.CheckStateChanged += new System.EventHandler(this.chkMalolos_CheckStateChanged);
            // 
            // frmAppRequirement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 426);
            this.ControlBox = false;
            this.Controls.Add(this.grpBnsOrg);
            this.Controls.Add(this.grpOther);
            this.Controls.Add(this.grpBnsNtr);
            this.Controls.Add(this.grpBnsStat);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.containerWithShadow2);
            this.Controls.Add(this.containerWithShadow1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.containerWithShadow3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmAppRequirement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Application Requirements";
            this.Load += new System.EventHandler(this.frmAppRequirement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.grpBnsStat.ResumeLayout(false);
            this.grpBnsStat.PerformLayout();
            this.grpBnsOrg.ResumeLayout(false);
            this.grpBnsOrg.PerformLayout();
            this.grpBnsNtr.ResumeLayout(false);
            this.grpBnsNtr.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.grpOther.ResumeLayout(false);
            this.grpOther.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private System.Windows.Forms.CheckBox chkNew;
        private System.Windows.Forms.CheckBox chkRenewal;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.TextBox txtDesc;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSave;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.GroupBox grpBnsStat;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEdit;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDelete;
        private System.Windows.Forms.GroupBox grpBnsOrg;
        private System.Windows.Forms.CheckBox chkOrgSingle;
        private System.Windows.Forms.CheckBox chkOrgPartner;
        private System.Windows.Forms.CheckBox chkOrgCorp;
        private System.Windows.Forms.GroupBox grpBnsNtr;
        private System.Windows.Forms.CheckBox chkNtrSpecific;
        private System.Windows.Forms.CheckBox chkNtrAll;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox chkOrgCoop;
        private System.Windows.Forms.CheckBox chkOtherGross;
        private System.Windows.Forms.CheckBox chkOtherPeza;
        private System.Windows.Forms.CheckBox chkOtherBoi;
        private System.Windows.Forms.TextBox txtOtherGross;
        private System.Windows.Forms.TextBox txtOthers;
        private System.Windows.Forms.GroupBox grpOther;
        private System.Windows.Forms.CheckBox chkMalolos;
    }
}

