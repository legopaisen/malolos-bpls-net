namespace Amellar.Common.SearchOwner
{
    partial class frmSearchOwner
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLn = new System.Windows.Forms.TextBox();
            this.txtFn = new System.Windows.Forms.TextBox();
            this.txtMi = new System.Windows.Forms.TextBox();
            this.txtAdd = new System.Windows.Forms.TextBox();
            this.dgvOwnNames = new System.Windows.Forms.DataGridView();
            this.clmLastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmFirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmMi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmStreet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBrgy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmZone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmDistrict = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmMun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmProv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmOwnCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zip_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnClear = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnOk1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOwnCode = new System.Windows.Forms.TextBox();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow3 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOwnNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Last Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "First Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(350, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "M.I.:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Address:";
            // 
            // txtLn
            // 
            this.txtLn.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLn.Location = new System.Drawing.Point(93, 23);
            this.txtLn.Name = "txtLn";
            this.txtLn.Size = new System.Drawing.Size(253, 20);
            this.txtLn.TabIndex = 1;
            // 
            // txtFn
            // 
            this.txtFn.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFn.Location = new System.Drawing.Point(93, 48);
            this.txtFn.Name = "txtFn";
            this.txtFn.Size = new System.Drawing.Size(253, 20);
            this.txtFn.TabIndex = 2;
            // 
            // txtMi
            // 
            this.txtMi.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMi.Location = new System.Drawing.Point(375, 48);
            this.txtMi.MaxLength = 2;
            this.txtMi.Name = "txtMi";
            this.txtMi.Size = new System.Drawing.Size(23, 20);
            this.txtMi.TabIndex = 3;
            // 
            // txtAdd
            // 
            this.txtAdd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAdd.Location = new System.Drawing.Point(93, 73);
            this.txtAdd.Name = "txtAdd";
            this.txtAdd.ReadOnly = true;
            this.txtAdd.Size = new System.Drawing.Size(386, 20);
            this.txtAdd.TabIndex = 7;
            // 
            // dgvOwnNames
            // 
            this.dgvOwnNames.AllowUserToAddRows = false;
            this.dgvOwnNames.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOwnNames.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvOwnNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOwnNames.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmLastName,
            this.clmFirstName,
            this.clmMi,
            this.clmAddress,
            this.clmStreet,
            this.clmBrgy,
            this.clmZone,
            this.clmDistrict,
            this.clmMun,
            this.clmProv,
            this.clmOwnCode,
            this.zip_code});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOwnNames.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvOwnNames.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvOwnNames.Location = new System.Drawing.Point(29, 118);
            this.dgvOwnNames.Name = "dgvOwnNames";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOwnNames.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvOwnNames.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOwnNames.Size = new System.Drawing.Size(446, 143);
            this.dgvOwnNames.TabIndex = 0;
            this.dgvOwnNames.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOwnNames_CellClick);
            this.dgvOwnNames.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOwnNames_CellContentClick);
            // 
            // clmLastName
            // 
            this.clmLastName.HeaderText = "Last Name";
            this.clmLastName.Name = "clmLastName";
            // 
            // clmFirstName
            // 
            this.clmFirstName.HeaderText = "First Name";
            this.clmFirstName.Name = "clmFirstName";
            // 
            // clmMi
            // 
            this.clmMi.HeaderText = "MI";
            this.clmMi.Name = "clmMi";
            // 
            // clmAddress
            // 
            this.clmAddress.HeaderText = "Address";
            this.clmAddress.Name = "clmAddress";
            // 
            // clmStreet
            // 
            this.clmStreet.HeaderText = "Street";
            this.clmStreet.Name = "clmStreet";
            // 
            // clmBrgy
            // 
            this.clmBrgy.HeaderText = "Barangay";
            this.clmBrgy.Name = "clmBrgy";
            // 
            // clmZone
            // 
            this.clmZone.HeaderText = "Zone";
            this.clmZone.Name = "clmZone";
            // 
            // clmDistrict
            // 
            this.clmDistrict.HeaderText = "District";
            this.clmDistrict.Name = "clmDistrict";
            // 
            // clmMun
            // 
            this.clmMun.HeaderText = "Municipality / City";
            this.clmMun.Name = "clmMun";
            // 
            // clmProv
            // 
            this.clmProv.HeaderText = "Province";
            this.clmProv.Name = "clmProv";
            // 
            // clmOwnCode
            // 
            this.clmOwnCode.HeaderText = "Own Code";
            this.clmOwnCode.Name = "clmOwnCode";
            // 
            // zip_code
            // 
            this.zip_code.HeaderText = "Zip Code";
            this.zip_code.Name = "zip_code";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(15, 284);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(474, 3);
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(30, 306);
            this.btnClear.Name = "btnClear";
            this.btnClear.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClear.Size = new System.Drawing.Size(75, 25);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear Fields";
            this.btnClear.Values.ExtraText = "";
            this.btnClear.Values.Image = null;
            this.btnClear.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClear.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClear.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClear.Values.Text = "Clear Fields";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnOk1
            // 
            this.btnOk1.Location = new System.Drawing.Point(323, 306);
            this.btnOk1.Name = "btnOk1";
            this.btnOk1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOk1.Size = new System.Drawing.Size(75, 25);
            this.btnOk1.TabIndex = 6;
            this.btnOk1.Text = "Ok";
            this.btnOk1.Values.ExtraText = "";
            this.btnOk1.Values.Image = null;
            this.btnOk1.Values.ImageStates.ImageCheckedNormal = null;
            this.btnOk1.Values.ImageStates.ImageCheckedPressed = null;
            this.btnOk1.Values.ImageStates.ImageCheckedTracking = null;
            this.btnOk1.Values.Text = "Ok";
            this.btnOk1.Click += new System.EventHandler(this.btnOk1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(404, 306);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(404, 46);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(75, 25);
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(350, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Code:";
            // 
            // txtOwnCode
            // 
            this.txtOwnCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtOwnCode.Location = new System.Drawing.Point(404, 23);
            this.txtOwnCode.MaxLength = 20;
            this.txtOwnCode.Name = "txtOwnCode";
            this.txtOwnCode.Size = new System.Drawing.Size(75, 20);
            this.txtOwnCode.TabIndex = 4;
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(12, 109);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(478, 168);
            this.containerWithShadow2.TabIndex = 22;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(12, 12);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(478, 100);
            this.containerWithShadow1.TabIndex = 21;
            // 
            // containerWithShadow3
            // 
            this.containerWithShadow3.Location = new System.Drawing.Point(12, 293);
            this.containerWithShadow3.Name = "containerWithShadow3";
            this.containerWithShadow3.Size = new System.Drawing.Size(478, 52);
            this.containerWithShadow3.TabIndex = 23;
            // 
            // frmSearchOwner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(498, 352);
            this.ControlBox = false;
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.dgvOwnNames);
            this.Controls.Add(this.containerWithShadow2);
            this.Controls.Add(this.txtAdd);
            this.Controls.Add(this.txtOwnCode);
            this.Controls.Add(this.txtMi);
            this.Controls.Add(this.txtFn);
            this.Controls.Add(this.txtLn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.containerWithShadow1);
            this.Controls.Add(this.containerWithShadow3);
            this.MaximumSize = new System.Drawing.Size(514, 390);
            this.MinimumSize = new System.Drawing.Size(514, 390);
            this.Name = "frmSearchOwner";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search Owner";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOwnNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLn;
        private System.Windows.Forms.TextBox txtFn;
        private System.Windows.Forms.TextBox txtMi;
        private System.Windows.Forms.TextBox txtAdd;
        private System.Windows.Forms.DataGridView dgvOwnNames;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClear;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmFirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmMi;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmStreet;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmBrgy;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmZone;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDistrict;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmMun;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmProv;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOwnCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn zip_code;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtOwnCode;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
    }
}

