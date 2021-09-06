namespace Amellar.Modules.Utilities
{
    partial class frmGross
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
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAdd = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEdit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnDelete = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDistRev = new System.Windows.Forms.TextBox();
            this.dgvPreGross = new System.Windows.Forms.DataGridView();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtGross1 = new System.Windows.Forms.TextBox();
            this.txtGross2 = new System.Windows.Forms.TextBox();
            this.cmbBnsDesc = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtToBeEdited = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreGross)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "FEES_CODE";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "BNS_CODE";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "BNS_DESC";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "MEANS";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "REV_YEAR";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(103, 352);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnAdd.Size = new System.Drawing.Size(62, 25);
            this.btnAdd.TabIndex = 28;
            this.btnAdd.Text = "Add";
            this.btnAdd.Values.ExtraText = "";
            this.btnAdd.Values.Image = null;
            this.btnAdd.Values.ImageStates.ImageCheckedNormal = null;
            this.btnAdd.Values.ImageStates.ImageCheckedPressed = null;
            this.btnAdd.Values.ImageStates.ImageCheckedTracking = null;
            this.btnAdd.Values.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Enabled = false;
            this.btnEdit.Location = new System.Drawing.Point(168, 352);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnEdit.Size = new System.Drawing.Size(62, 25);
            this.btnEdit.TabIndex = 29;
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
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(236, 352);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnDelete.Size = new System.Drawing.Size(62, 25);
            this.btnDelete.TabIndex = 30;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Values.ExtraText = "";
            this.btnDelete.Values.Image = null;
            this.btnDelete.Values.ImageStates.ImageCheckedNormal = null;
            this.btnDelete.Values.ImageStates.ImageCheckedPressed = null;
            this.btnDelete.Values.ImageStates.ImageCheckedTracking = null;
            this.btnDelete.Values.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(304, 352);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(62, 25);
            this.btnClose.TabIndex = 31;
            this.btnClose.Text = "Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 235);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 13);
            this.label10.TabIndex = 37;
            this.label10.Text = "Revenue Year:";
            // 
            // txtDistRev
            // 
            this.txtDistRev.Enabled = false;
            this.txtDistRev.Location = new System.Drawing.Point(105, 232);
            this.txtDistRev.Name = "txtDistRev";
            this.txtDistRev.ReadOnly = true;
            this.txtDistRev.Size = new System.Drawing.Size(59, 20);
            this.txtDistRev.TabIndex = 38;
            this.txtDistRev.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // dgvPreGross
            // 
            this.dgvPreGross.AllowUserToAddRows = false;
            this.dgvPreGross.AllowUserToDeleteRows = false;
            this.dgvPreGross.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPreGross.Location = new System.Drawing.Point(17, 18);
            this.dgvPreGross.Name = "dgvPreGross";
            this.dgvPreGross.ReadOnly = true;
            this.dgvPreGross.RowHeadersVisible = false;
            this.dgvPreGross.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPreGross.Size = new System.Drawing.Size(467, 192);
            this.dgvPreGross.TabIndex = 41;
            this.dgvPreGross.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPreGross_CellContentClick);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(20, 311);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(69, 13);
            this.label13.TabIndex = 42;
            this.label13.Text = "Gross Range";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(214, 308);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(19, 13);
            this.label14.TabIndex = 43;
            this.label14.Text = "to ";
            // 
            // txtGross1
            // 
            this.txtGross1.Enabled = false;
            this.txtGross1.Location = new System.Drawing.Point(105, 304);
            this.txtGross1.Name = "txtGross1";
            this.txtGross1.Size = new System.Drawing.Size(103, 20);
            this.txtGross1.TabIndex = 44;
            this.txtGross1.Text = "0";
            this.txtGross1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtGross1.Leave += new System.EventHandler(this.txtGross1_Leave);
            this.txtGross1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGross1_KeyPress);
            // 
            // txtGross2
            // 
            this.txtGross2.Enabled = false;
            this.txtGross2.Location = new System.Drawing.Point(236, 305);
            this.txtGross2.Name = "txtGross2";
            this.txtGross2.Size = new System.Drawing.Size(103, 20);
            this.txtGross2.TabIndex = 45;
            this.txtGross2.Text = "0";
            this.txtGross2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtGross2.Leave += new System.EventHandler(this.txtGross2_Leave);
            this.txtGross2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGross2_KeyPress);
            // 
            // cmbBnsDesc
            // 
            this.cmbBnsDesc.Enabled = false;
            this.cmbBnsDesc.FormattingEnabled = true;
            this.cmbBnsDesc.Location = new System.Drawing.Point(105, 277);
            this.cmbBnsDesc.Name = "cmbBnsDesc";
            this.cmbBnsDesc.Size = new System.Drawing.Size(291, 21);
            this.cmbBnsDesc.TabIndex = 46;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(9, 224);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(483, 114);
            this.label2.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(483, 211);
            this.label1.TabIndex = 47;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 261);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 50;
            this.label5.Text = "Business Type";
            // 
            // txtToBeEdited
            // 
            this.txtToBeEdited.Enabled = false;
            this.txtToBeEdited.Location = new System.Drawing.Point(22, 277);
            this.txtToBeEdited.Name = "txtToBeEdited";
            this.txtToBeEdited.ReadOnly = true;
            this.txtToBeEdited.Size = new System.Drawing.Size(77, 20);
            this.txtToBeEdited.TabIndex = 51;
            this.txtToBeEdited.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // frmGross
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(504, 386);
            this.ControlBox = false;
            this.Controls.Add(this.txtToBeEdited);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbBnsDesc);
            this.Controls.Add(this.txtGross2);
            this.Controls.Add(this.txtGross1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtDistRev);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvPreGross);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGross";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Presumptive Gross Table";
            this.Load += new System.EventHandler(this.frmGross_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreGross)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvBns_Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAdd;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEdit;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDelete;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtDistRev;
        private System.Windows.Forms.DataGridView dgvPreGross;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtGross1;
        private System.Windows.Forms.TextBox txtGross2;
        private System.Windows.Forms.ComboBox cmbBnsDesc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtToBeEdited;

    }
}