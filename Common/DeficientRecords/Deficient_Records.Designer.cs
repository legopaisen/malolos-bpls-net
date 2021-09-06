namespace Amellar.Common.DeficientRecords
{
    partial class frmDeficientRecords
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
            this.grpDeficientRecord1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtReferenceNo = new System.Windows.Forms.TextBox();
            this.grpDeficiencyRecrds = new System.Windows.Forms.GroupBox();
            this.dgvDeficientRecord = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvAdditionalInfo = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpDeficientRecord2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtImageDir = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.RichTextBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDelete = new System.Windows.Forms.Button();
            this.grpDeficientRecord1.SuspendLayout();
            this.grpDeficiencyRecrds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeficientRecord)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdditionalInfo)).BeginInit();
            this.grpDeficientRecord2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpDeficientRecord1
            // 
            this.grpDeficientRecord1.Controls.Add(this.label1);
            this.grpDeficientRecord1.Controls.Add(this.label6);
            this.grpDeficientRecord1.Controls.Add(this.txtReferenceNo);
            this.grpDeficientRecord1.Location = new System.Drawing.Point(11, 3);
            this.grpDeficientRecord1.Name = "grpDeficientRecord1";
            this.grpDeficientRecord1.Size = new System.Drawing.Size(495, 50);
            this.grpDeficientRecord1.TabIndex = 0;
            this.grpDeficientRecord1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Reference No.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(97, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = ":";
            // 
            // txtReferenceNo
            // 
            this.txtReferenceNo.Location = new System.Drawing.Point(111, 18);
            this.txtReferenceNo.Name = "txtReferenceNo";
            this.txtReferenceNo.ReadOnly = true;
            this.txtReferenceNo.Size = new System.Drawing.Size(368, 20);
            this.txtReferenceNo.TabIndex = 1;
            // 
            // grpDeficiencyRecrds
            // 
            this.grpDeficiencyRecrds.Controls.Add(this.dgvDeficientRecord);
            this.grpDeficiencyRecrds.ForeColor = System.Drawing.Color.Black;
            this.grpDeficiencyRecrds.Location = new System.Drawing.Point(11, 57);
            this.grpDeficiencyRecrds.Name = "grpDeficiencyRecrds";
            this.grpDeficiencyRecrds.Size = new System.Drawing.Size(495, 174);
            this.grpDeficiencyRecrds.TabIndex = 1;
            this.grpDeficiencyRecrds.TabStop = false;
            this.grpDeficiencyRecrds.Text = "Deficient Records";
            // 
            // dgvDeficientRecord
            // 
            this.dgvDeficientRecord.AllowUserToAddRows = false;
            this.dgvDeficientRecord.AllowUserToDeleteRows = false;
            this.dgvDeficientRecord.AllowUserToResizeColumns = false;
            this.dgvDeficientRecord.AllowUserToResizeRows = false;
            this.dgvDeficientRecord.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDeficientRecord.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvDeficientRecord.Location = new System.Drawing.Point(15, 18);
            this.dgvDeficientRecord.Name = "dgvDeficientRecord";
            this.dgvDeficientRecord.RowHeadersVisible = false;
            this.dgvDeficientRecord.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvDeficientRecord.Size = new System.Drawing.Size(465, 147);
            this.dgvDeficientRecord.TabIndex = 0;
            this.dgvDeficientRecord.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDeficientRecord_CellEndEdit);
            this.dgvDeficientRecord.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDeficientRecord_CellClick);
            this.dgvDeficientRecord.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDeficientRecord_CellContentClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvAdditionalInfo);
            this.groupBox3.ForeColor = System.Drawing.Color.Black;
            this.groupBox3.Location = new System.Drawing.Point(11, 264);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(495, 133);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Additional information";
            // 
            // dgvAdditionalInfo
            // 
            this.dgvAdditionalInfo.AllowUserToAddRows = false;
            this.dgvAdditionalInfo.AllowUserToDeleteRows = false;
            this.dgvAdditionalInfo.AllowUserToResizeColumns = false;
            this.dgvAdditionalInfo.AllowUserToResizeRows = false;
            this.dgvAdditionalInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdditionalInfo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvAdditionalInfo.Location = new System.Drawing.Point(14, 17);
            this.dgvAdditionalInfo.Name = "dgvAdditionalInfo";
            this.dgvAdditionalInfo.RowHeadersVisible = false;
            this.dgvAdditionalInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvAdditionalInfo.Size = new System.Drawing.Size(466, 108);
            this.dgvAdditionalInfo.TabIndex = 0;
            this.dgvAdditionalInfo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAdditionalInfo_CellValueChanged);
            this.dgvAdditionalInfo.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvAdditionalInfo_CellBeginEdit);
            this.dgvAdditionalInfo.Leave += new System.EventHandler(this.dgvAdditionalInfo_Leave);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(350, 482);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "&OK";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(269, 482);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(431, 482);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grpDeficientRecord2
            // 
            this.grpDeficientRecord2.Controls.Add(this.label5);
            this.grpDeficientRecord2.Controls.Add(this.label4);
            this.grpDeficientRecord2.Controls.Add(this.txtImageDir);
            this.grpDeficientRecord2.Controls.Add(this.label3);
            this.grpDeficientRecord2.Controls.Add(this.label2);
            this.grpDeficientRecord2.Controls.Add(this.txtRemarks);
            this.grpDeficientRecord2.Location = new System.Drawing.Point(12, 396);
            this.grpDeficientRecord2.Name = "grpDeficientRecord2";
            this.grpDeficientRecord2.Size = new System.Drawing.Size(494, 82);
            this.grpDeficientRecord2.TabIndex = 6;
            this.grpDeficientRecord2.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(68, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = ":";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(68, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(10, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = ":";
            // 
            // txtImageDir
            // 
            this.txtImageDir.Location = new System.Drawing.Point(80, 53);
            this.txtImageDir.Name = "txtImageDir";
            this.txtImageDir.Size = new System.Drawing.Size(398, 20);
            this.txtImageDir.TabIndex = 2;
            this.txtImageDir.TextChanged += new System.EventHandler(this.txtImageDir_TextChanged);
            this.txtImageDir.Leave += new System.EventHandler(this.txtImageDir_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "&Remarks";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "&Image Dir";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(80, 14);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(398, 32);
            this.txtRemarks.TabIndex = 0;
            this.txtRemarks.Text = "";
            this.txtRemarks.Leave += new System.EventHandler(this.txtRemarks_Leave);
            this.txtRemarks.TextChanged += new System.EventHandler(this.txtRemarks_TextChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "User Code";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 60;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Date/Time Save";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 70;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Additional Info";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 150;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Value";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(394, 237);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(97, 23);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "&Delete Deficient";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.MouseLeave += new System.EventHandler(this.btnDelete_MouseLeave);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.btnDelete.MouseHover += new System.EventHandler(this.btnDelete_MouseHover);
            // 
            // frmDeficientRecords
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 514);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.grpDeficientRecord2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.grpDeficiencyRecrds);
            this.Controls.Add(this.grpDeficientRecord1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDeficientRecords";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Deficient Records";
            this.Load += new System.EventHandler(this.frmDeficientRecords_Load);
            this.grpDeficientRecord1.ResumeLayout(false);
            this.grpDeficientRecord1.PerformLayout();
            this.grpDeficiencyRecrds.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeficientRecord)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdditionalInfo)).EndInit();
            this.grpDeficientRecord2.ResumeLayout(false);
            this.grpDeficientRecord2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpDeficientRecord1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtReferenceNo;
        private System.Windows.Forms.GroupBox grpDeficiencyRecrds;
        private System.Windows.Forms.DataGridView dgvDeficientRecord;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvAdditionalInfo;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpDeficientRecord2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox txtRemarks;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtImageDir;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.Button btnDelete;
    }
}