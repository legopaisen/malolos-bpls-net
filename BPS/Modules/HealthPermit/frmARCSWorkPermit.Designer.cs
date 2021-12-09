namespace Amellar.Modules.HealthPermit
{
    partial class frmARCSPermits
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtgRecord = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnHealth = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnWorkPermit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblORNo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblORNValue = new System.Windows.Forms.Label();
            this.lblORDValue = new System.Windows.Forms.Label();
            this.lblFeesDValue = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dtgRecord)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgRecord
            // 
            this.dtgRecord.AllowUserToAddRows = false;
            this.dtgRecord.AllowUserToDeleteRows = false;
            this.dtgRecord.AllowUserToResizeRows = false;
            this.dtgRecord.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgRecord.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgRecord.DefaultCellStyle = dataGridViewCellStyle3;
            this.dtgRecord.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dtgRecord.Location = new System.Drawing.Point(8, 8);
            this.dtgRecord.MultiSelect = false;
            this.dtgRecord.Name = "dtgRecord";
            this.dtgRecord.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgRecord.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dtgRecord.RowHeadersVisible = false;
            this.dtgRecord.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgRecord.Size = new System.Drawing.Size(307, 299);
            this.dtgRecord.TabIndex = 2;
            this.dtgRecord.SelectionChanged += new System.EventHandler(this.dtgRecord_SelectionChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "O.R. #";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 70;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "O.R. Date";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 80;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "FEES DESCRIPTION";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 135;
            // 
            // btnHealth
            // 
            this.btnHealth.Location = new System.Drawing.Point(321, 20);
            this.btnHealth.Name = "btnHealth";
            this.btnHealth.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnHealth.Size = new System.Drawing.Size(109, 25);
            this.btnHealth.TabIndex = 35;
            this.btnHealth.Text = "Health Permit";
            this.btnHealth.Values.ExtraText = "";
            this.btnHealth.Values.Image = null;
            this.btnHealth.Values.ImageStates.ImageCheckedNormal = null;
            this.btnHealth.Values.ImageStates.ImageCheckedPressed = null;
            this.btnHealth.Values.ImageStates.ImageCheckedTracking = null;
            this.btnHealth.Values.Text = "Health Permit";
            this.btnHealth.Click += new System.EventHandler(this.btnHealth_Click);
            // 
            // btnWorkPermit
            // 
            this.btnWorkPermit.Location = new System.Drawing.Point(321, 51);
            this.btnWorkPermit.Name = "btnWorkPermit";
            this.btnWorkPermit.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnWorkPermit.Size = new System.Drawing.Size(109, 25);
            this.btnWorkPermit.TabIndex = 34;
            this.btnWorkPermit.Text = "Working Permit";
            this.btnWorkPermit.Values.ExtraText = "";
            this.btnWorkPermit.Values.Image = null;
            this.btnWorkPermit.Values.ImageStates.ImageCheckedNormal = null;
            this.btnWorkPermit.Values.ImageStates.ImageCheckedPressed = null;
            this.btnWorkPermit.Values.ImageStates.ImageCheckedTracking = null;
            this.btnWorkPermit.Values.Text = "Working Permit";
            this.btnWorkPermit.Click += new System.EventHandler(this.btnWorkPermit_Click);
            // 
            // lblORNo
            // 
            this.lblORNo.AutoSize = true;
            this.lblORNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblORNo.Location = new System.Drawing.Point(323, 146);
            this.lblORNo.Name = "lblORNo";
            this.lblORNo.Size = new System.Drawing.Size(45, 13);
            this.lblORNo.TabIndex = 36;
            this.lblORNo.Text = "O.R. #";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(323, 204);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 36;
            this.label2.Text = "O.R. Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(323, 262);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = "Fees Desc.";
            // 
            // lblORNValue
            // 
            this.lblORNValue.AutoSize = true;
            this.lblORNValue.Location = new System.Drawing.Point(333, 168);
            this.lblORNValue.Name = "lblORNValue";
            this.lblORNValue.Size = new System.Drawing.Size(39, 13);
            this.lblORNValue.TabIndex = 36;
            this.lblORNValue.Text = "O.R. #";
            // 
            // lblORDValue
            // 
            this.lblORDValue.AutoSize = true;
            this.lblORDValue.Location = new System.Drawing.Point(333, 226);
            this.lblORDValue.Name = "lblORDValue";
            this.lblORDValue.Size = new System.Drawing.Size(55, 13);
            this.lblORDValue.TabIndex = 36;
            this.lblORDValue.Text = "O.R. Date";
            // 
            // lblFeesDValue
            // 
            this.lblFeesDValue.AutoSize = true;
            this.lblFeesDValue.Location = new System.Drawing.Point(333, 284);
            this.lblFeesDValue.Name = "lblFeesDValue";
            this.lblFeesDValue.Size = new System.Drawing.Size(61, 13);
            this.lblFeesDValue.TabIndex = 36;
            this.lblFeesDValue.Text = "Fees Desc.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(321, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 36;
            this.label1.Text = "Filter O.R.";
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(324, 108);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(106, 20);
            this.txtFilter.TabIndex = 37;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            this.txtFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFilter_KeyPress);
            // 
            // frmARCSPermits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(442, 317);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.lblFeesDValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblORDValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblORNValue);
            this.Controls.Add(this.lblORNo);
            this.Controls.Add(this.btnHealth);
            this.Controls.Add(this.btnWorkPermit);
            this.Controls.Add(this.dtgRecord);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmARCSPermits";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ARCS - Permits";
            this.Load += new System.EventHandler(this.frmARCSWorkPermit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgRecord)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dtgRecord;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnHealth;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnWorkPermit;
        private System.Windows.Forms.Label lblORNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblORNValue;
        private System.Windows.Forms.Label lblORDValue;
        private System.Windows.Forms.Label lblFeesDValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFilter;
    }
}