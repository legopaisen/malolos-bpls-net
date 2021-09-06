namespace Amellar.Common.BusinessType
{
    partial class frmBusinessType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBusinessType));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grpMainBnsType = new System.Windows.Forms.GroupBox();
            this.cmbMainBnsType = new System.Windows.Forms.ComboBox();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.label4 = new System.Windows.Forms.Label();
            this.grpCodeDesc = new System.Windows.Forms.GroupBox();
            this.btnClear = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSearchCode = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtBnsDesc = new System.Windows.Forms.TextBox();
            this.txtBnsCode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.kryptonHeader3 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.dgvSubCat = new System.Windows.Forms.DataGridView();
            this.clmCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBnsDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.grpMainBnsType.SuspendLayout();
            this.grpCodeDesc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubCat)).BeginInit();
            this.SuspendLayout();
            // 
            // grpMainBnsType
            // 
            this.grpMainBnsType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMainBnsType.Controls.Add(this.cmbMainBnsType);
            this.grpMainBnsType.Controls.Add(this.kryptonHeader1);
            this.grpMainBnsType.Controls.Add(this.label4);
            this.grpMainBnsType.Location = new System.Drawing.Point(15, 12);
            this.grpMainBnsType.Name = "grpMainBnsType";
            this.grpMainBnsType.Size = new System.Drawing.Size(363, 106);
            this.grpMainBnsType.TabIndex = 0;
            this.grpMainBnsType.TabStop = false;
            // 
            // cmbMainBnsType
            // 
            this.cmbMainBnsType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbMainBnsType.FormattingEnabled = true;
            this.cmbMainBnsType.Location = new System.Drawing.Point(25, 60);
            this.cmbMainBnsType.MaxDropDownItems = 10;
            this.cmbMainBnsType.Name = "cmbMainBnsType";
            this.cmbMainBnsType.Size = new System.Drawing.Size(308, 21);
            this.cmbMainBnsType.TabIndex = 1;
            this.cmbMainBnsType.SelectedIndexChanged += new System.EventHandler(this.cmbMainBnsType_SelectedIndexChanged);
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonHeader1.AutoSize = false;
            this.kryptonHeader1.Location = new System.Drawing.Point(6, 14);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader1.Size = new System.Drawing.Size(351, 30);
            this.kryptonHeader1.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kryptonHeader1.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonHeader1.TabIndex = 0;
            this.kryptonHeader1.Text = "Main Business";
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Main Business";
            this.kryptonHeader1.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader1.Values.Image")));
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label4.Location = new System.Drawing.Point(33, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(308, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "label4";
            // 
            // grpCodeDesc
            // 
            this.grpCodeDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpCodeDesc.Controls.Add(this.btnClear);
            this.grpCodeDesc.Controls.Add(this.btnSearchCode);
            this.grpCodeDesc.Controls.Add(this.txtBnsDesc);
            this.grpCodeDesc.Controls.Add(this.txtBnsCode);
            this.grpCodeDesc.Controls.Add(this.label6);
            this.grpCodeDesc.Controls.Add(this.label5);
            this.grpCodeDesc.Controls.Add(this.kryptonHeader3);
            this.grpCodeDesc.Location = new System.Drawing.Point(16, 6);
            this.grpCodeDesc.Name = "grpCodeDesc";
            this.grpCodeDesc.Size = new System.Drawing.Size(362, 106);
            this.grpCodeDesc.TabIndex = 5;
            this.grpCodeDesc.TabStop = false;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(278, 47);
            this.btnClear.Name = "btnClear";
            this.btnClear.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClear.Size = new System.Drawing.Size(62, 25);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.Values.ExtraText = "";
            this.btnClear.Values.Image = null;
            this.btnClear.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClear.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClear.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClear.Values.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSearchCode
            // 
            this.btnSearchCode.Location = new System.Drawing.Point(210, 47);
            this.btnSearchCode.Name = "btnSearchCode";
            this.btnSearchCode.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearchCode.Size = new System.Drawing.Size(62, 25);
            this.btnSearchCode.TabIndex = 8;
            this.btnSearchCode.Text = "Search";
            this.btnSearchCode.Values.ExtraText = "";
            this.btnSearchCode.Values.Image = null;
            this.btnSearchCode.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearchCode.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearchCode.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearchCode.Values.Text = "Search";
            this.btnSearchCode.Click += new System.EventHandler(this.btnSearchCode_Click);
            // 
            // txtBnsDesc
            // 
            this.txtBnsDesc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBnsDesc.Location = new System.Drawing.Point(74, 76);
            this.txtBnsDesc.Name = "txtBnsDesc";
            this.txtBnsDesc.Size = new System.Drawing.Size(268, 20);
            this.txtBnsDesc.TabIndex = 4;
            // 
            // txtBnsCode
            // 
            this.txtBnsCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBnsCode.Location = new System.Drawing.Point(74, 50);
            this.txtBnsCode.Name = "txtBnsCode";
            this.txtBnsCode.Size = new System.Drawing.Size(111, 20);
            this.txtBnsCode.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Description:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Code:";
            // 
            // kryptonHeader3
            // 
            this.kryptonHeader3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonHeader3.AutoSize = false;
            this.kryptonHeader3.Location = new System.Drawing.Point(6, 14);
            this.kryptonHeader3.Name = "kryptonHeader3";
            this.kryptonHeader3.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader3.Size = new System.Drawing.Size(350, 30);
            this.kryptonHeader3.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kryptonHeader3.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonHeader3.TabIndex = 0;
            this.kryptonHeader3.Text = "Searching by Code or Description";
            this.kryptonHeader3.Values.Description = "";
            this.kryptonHeader3.Values.Heading = "Searching by Code or Description";
            this.kryptonHeader3.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader3.Values.Image")));
            // 
            // dgvSubCat
            // 
            this.dgvSubCat.AllowUserToAddRows = false;
            this.dgvSubCat.AllowUserToDeleteRows = false;
            this.dgvSubCat.AllowUserToResizeColumns = false;
            this.dgvSubCat.AllowUserToResizeRows = false;
            this.dgvSubCat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSubCat.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSubCat.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmCode,
            this.clmBnsDesc});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSubCat.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSubCat.Location = new System.Drawing.Point(21, 173);
            this.dgvSubCat.Name = "dgvSubCat";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSubCat.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSubCat.RowHeadersVisible = false;
            this.dgvSubCat.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSubCat.Size = new System.Drawing.Size(351, 202);
            this.dgvSubCat.TabIndex = 2;
            this.dgvSubCat.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSubCat_CellClick);
            this.dgvSubCat.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSubCat_CellContentClick);
            // 
            // clmCode
            // 
            this.clmCode.Frozen = true;
            this.clmCode.HeaderText = "Code    ";
            this.clmCode.Name = "clmCode";
            this.clmCode.ReadOnly = true;
            this.clmCode.Width = 69;
            // 
            // clmBnsDesc
            // 
            this.clmBnsDesc.Frozen = true;
            this.clmBnsDesc.HeaderText = "Business Description                                           ";
            this.clmBnsDesc.Name = "clmBnsDesc";
            this.clmBnsDesc.ReadOnly = true;
            this.clmBnsDesc.Width = 259;
            // 
            // kryptonHeader2
            // 
            this.kryptonHeader2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonHeader2.AutoSize = false;
            this.kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader2.Location = new System.Drawing.Point(20, 141);
            this.kryptonHeader2.Name = "kryptonHeader2";
            this.kryptonHeader2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.kryptonHeader2.Size = new System.Drawing.Size(352, 22);
            this.kryptonHeader2.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kryptonHeader2.StateCommon.Content.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.kryptonHeader2.TabIndex = 1;
            this.kryptonHeader2.Text = "Sub-Categories";
            this.kryptonHeader2.Values.Description = "";
            this.kryptonHeader2.Values.Heading = "Sub-Categories";
            this.kryptonHeader2.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader2.Values.Image")));
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(292, 426);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(62, 25);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(224, 426);
            this.btnOk.Name = "btnOk";
            this.btnOk.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOk.Size = new System.Drawing.Size(62, 25);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "Ok";
            this.btnOk.Values.ExtraText = "";
            this.btnOk.Values.Image = null;
            this.btnOk.Values.ImageStates.ImageCheckedNormal = null;
            this.btnOk.Values.ImageStates.ImageCheckedPressed = null;
            this.btnOk.Values.ImageStates.ImageCheckedTracking = null;
            this.btnOk.Values.Text = "Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(12, 169);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(366, 241);
            this.containerWithShadow1.TabIndex = 12;
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(12, 410);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(366, 63);
            this.containerWithShadow2.TabIndex = 13;
            // 
            // frmBusinessType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(385, 475);
            this.ControlBox = false;
            this.Controls.Add(this.grpCodeDesc);
            this.Controls.Add(this.grpMainBnsType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.dgvSubCat);
            this.Controls.Add(this.kryptonHeader2);
            this.Controls.Add(this.containerWithShadow1);
            this.Controls.Add(this.containerWithShadow2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmBusinessType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Business Type";
            this.Load += new System.EventHandler(this.frmBusinessType_Load);
            this.grpMainBnsType.ResumeLayout(false);
            this.grpCodeDesc.ResumeLayout(false);
            this.grpCodeDesc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubCat)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpMainBnsType;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private System.Windows.Forms.ComboBox cmbMainBnsType;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgvSubCat;
        private System.Windows.Forms.GroupBox grpCodeDesc;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader3;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmBnsDesc;
        public System.Windows.Forms.TextBox txtBnsCode;
        public System.Windows.Forms.TextBox txtBnsDesc;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClear;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearchCode;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
    }
}

