namespace CDOReport
{
    partial class frmReport
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
            this.dgvCDO = new System.Windows.Forms.DataGridView();
            this.ck_field = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.bin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bns_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bns_add = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.own_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dt_tag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnGenerate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRefresh = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.chkAll = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCDO)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCDO
            // 
            this.dgvCDO.AllowUserToAddRows = false;
            this.dgvCDO.AllowUserToDeleteRows = false;
            this.dgvCDO.AllowUserToResizeColumns = false;
            this.dgvCDO.AllowUserToResizeRows = false;
            this.dgvCDO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCDO.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ck_field,
            this.bin,
            this.bns_name,
            this.bns_add,
            this.own_name,
            this.dt_tag});
            this.dgvCDO.Location = new System.Drawing.Point(14, 22);
            this.dgvCDO.Name = "dgvCDO";
            this.dgvCDO.RowHeadersVisible = false;
            this.dgvCDO.Size = new System.Drawing.Size(750, 338);
            this.dgvCDO.TabIndex = 0;
            // 
            // ck_field
            // 
            this.ck_field.HeaderText = "";
            this.ck_field.Name = "ck_field";
            this.ck_field.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ck_field.Width = 30;
            // 
            // bin
            // 
            this.bin.HeaderText = "BIN";
            this.bin.Name = "bin";
            this.bin.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.bin.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.bin.Width = 130;
            // 
            // bns_name
            // 
            this.bns_name.HeaderText = "Business Name";
            this.bns_name.Name = "bns_name";
            this.bns_name.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.bns_name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.bns_name.Width = 300;
            // 
            // bns_add
            // 
            this.bns_add.HeaderText = "Address";
            this.bns_add.Name = "bns_add";
            this.bns_add.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.bns_add.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.bns_add.Width = 300;
            // 
            // own_name
            // 
            this.own_name.HeaderText = "Owner\'s Name";
            this.own_name.Name = "own_name";
            this.own_name.Width = 200;
            // 
            // dt_tag
            // 
            this.dt_tag.HeaderText = "Date Tagged";
            this.dt_tag.Name = "dt_tag";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(693, 377);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnGenerate.Size = new System.Drawing.Size(71, 27);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Values.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "BIN";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 130;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Business Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 300;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Address";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 300;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Owner\'s Name";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 200;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Date Tagged";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(14, 377);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnRefresh.Size = new System.Drawing.Size(107, 27);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Values.Text = "Refresh List";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(24, 27);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(15, 14);
            this.chkAll.TabIndex = 3;
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // frmReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 421);
            this.Controls.Add(this.chkAll);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.dgvCDO);
            this.Name = "frmReport";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "List of Businesses with CDO";
            this.Load += new System.EventHandler(this.frmReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCDO)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnGenerate;
        public System.Windows.Forms.DataGridView dgvCDO;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ck_field;
        private System.Windows.Forms.DataGridViewTextBoxColumn bin;
        private System.Windows.Forms.DataGridViewTextBoxColumn bns_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn bns_add;
        private System.Windows.Forms.DataGridViewTextBoxColumn own_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn dt_tag;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnRefresh;
        private System.Windows.Forms.CheckBox chkAll;
    }
}