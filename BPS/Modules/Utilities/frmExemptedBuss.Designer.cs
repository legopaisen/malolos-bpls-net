namespace Amellar.Modules.Utilities
{
    partial class frmExemptedBuss
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvBussList = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.containerWithShadow3 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.btnUpdate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.containerWithShadow4 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.dgvSubCatList = new System.Windows.Forms.DataGridView();
            this.txtSubTaxFee = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbBussType = new MultiColumnComboBoxDemo.MultiColumnComboBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBussList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubCatList)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tax/Fee";
            // 
            // dgvBussList
            // 
            this.dgvBussList.AllowUserToAddRows = false;
            this.dgvBussList.AllowUserToDeleteRows = false;
            this.dgvBussList.AllowUserToResizeColumns = false;
            this.dgvBussList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBussList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBussList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBussList.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvBussList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvBussList.Location = new System.Drawing.Point(20, 66);
            this.dgvBussList.Name = "dgvBussList";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBussList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvBussList.RowHeadersVisible = false;
            this.dgvBussList.Size = new System.Drawing.Size(389, 219);
            this.dgvBussList.TabIndex = 3;
            this.dgvBussList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBussList_CellClick);
            this.dgvBussList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBussList_CellContentClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(154, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Main Business Type";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // containerWithShadow3
            // 
            this.containerWithShadow3.Location = new System.Drawing.Point(4, 303);
            this.containerWithShadow3.Name = "containerWithShadow3";
            this.containerWithShadow3.Size = new System.Drawing.Size(850, 54);
            this.containerWithShadow3.TabIndex = 0;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Location = new System.Drawing.Point(657, 314);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnUpdate.Size = new System.Drawing.Size(88, 25);
            this.btnUpdate.TabIndex = 24;
            this.btnUpdate.Values.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(751, 314);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(88, 25);
            this.btnCancel.TabIndex = 24;
            this.btnCancel.Values.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // containerWithShadow4
            // 
            this.containerWithShadow4.Location = new System.Drawing.Point(432, 0);
            this.containerWithShadow4.Name = "containerWithShadow4";
            this.containerWithShadow4.Size = new System.Drawing.Size(422, 304);
            this.containerWithShadow4.TabIndex = 0;
            // 
            // dgvSubCatList
            // 
            this.dgvSubCatList.AllowUserToAddRows = false;
            this.dgvSubCatList.AllowUserToDeleteRows = false;
            this.dgvSubCatList.AllowUserToResizeColumns = false;
            this.dgvSubCatList.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSubCatList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvSubCatList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSubCatList.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvSubCatList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvSubCatList.Location = new System.Drawing.Point(450, 66);
            this.dgvSubCatList.Name = "dgvSubCatList";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSubCatList.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvSubCatList.RowHeadersVisible = false;
            this.dgvSubCatList.Size = new System.Drawing.Size(389, 219);
            this.dgvSubCatList.TabIndex = 3;
            this.dgvSubCatList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSubCatList_CellContentClick);
            // 
            // txtSubTaxFee
            // 
            this.txtSubTaxFee.Location = new System.Drawing.Point(501, 38);
            this.txtSubTaxFee.Name = "txtSubTaxFee";
            this.txtSubTaxFee.ReadOnly = true;
            this.txtSubTaxFee.Size = new System.Drawing.Size(338, 20);
            this.txtSubTaxFee.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(447, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Tax/Fee";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(597, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Sub-Categories";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(4, 0);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(422, 304);
            this.containerWithShadow1.TabIndex = 25;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(163, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "Main Business Type";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cmbBussType
            // 
            this.cmbBussType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbBussType.FormattingEnabled = true;
            this.cmbBussType.Location = new System.Drawing.Point(118, 38);
            this.cmbBussType.Name = "cmbBussType";
            this.cmbBussType.Size = new System.Drawing.Size(291, 21);
            this.cmbBussType.TabIndex = 27;
            this.cmbBussType.SelectedValueChanged += new System.EventHandler(this.cmbBussType_SelectedValueChanged);
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(71, 38);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(41, 20);
            this.txtCode.TabIndex = 28;
            // 
            // frmExemptedBuss
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(859, 359);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.cmbBussType);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.dgvSubCatList);
            this.Controls.Add(this.dgvBussList);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSubTaxFee);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.containerWithShadow3);
            this.Controls.Add(this.containerWithShadow4);
            this.Controls.Add(this.containerWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmExemptedBuss";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Exempted Business";
            this.Load += new System.EventHandler(this.frmExemptedBuss_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBussList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubCatList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvBussList;
        private System.Windows.Forms.Label label3;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow3;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnUpdate;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow4;
        private System.Windows.Forms.DataGridView dgvSubCatList;
        private System.Windows.Forms.TextBox txtSubTaxFee;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.Label label6;
        private MultiColumnComboBoxDemo.MultiColumnComboBox cmbBussType;
        private System.Windows.Forms.TextBox txtCode;
    }
}