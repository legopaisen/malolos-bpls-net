namespace Amellar.Modules.BusinessMapping
{
    partial class frmForm
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
            this.cmbBrgy = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPrint = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.chkPermitNo = new System.Windows.Forms.CheckBox();
            this.chkBussName = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkBlank = new System.Windows.Forms.CheckBox();
            this.chkData = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbBrgy
            // 
            this.cmbBrgy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBrgy.FormattingEnabled = true;
            this.cmbBrgy.Location = new System.Drawing.Point(118, 64);
            this.cmbBrgy.Name = "cmbBrgy";
            this.cmbBrgy.Size = new System.Drawing.Size(226, 21);
            this.cmbBrgy.TabIndex = 1;
            this.cmbBrgy.SelectedValueChanged += new System.EventHandler(this.cmbBrgy_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Barangay";
            // 
            // btnPrint
            // 
            this.btnPrint.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnPrint.Location = new System.Drawing.Point(234, 369);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint.Size = new System.Drawing.Size(92, 25);
            this.btnPrint.TabIndex = 10;
            this.btnPrint.Values.Text = "Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            this.btnClose.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnClose.Location = new System.Drawing.Point(332, 369);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(92, 25);
            this.btnClose.TabIndex = 10;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkPermitNo
            // 
            this.chkPermitNo.AutoSize = true;
            this.chkPermitNo.Location = new System.Drawing.Point(228, 97);
            this.chkPermitNo.Name = "chkPermitNo";
            this.chkPermitNo.Size = new System.Drawing.Size(75, 17);
            this.chkPermitNo.TabIndex = 11;
            this.chkPermitNo.Text = "Permit No.";
            this.chkPermitNo.UseVisualStyleBackColor = true;
            this.chkPermitNo.CheckStateChanged += new System.EventHandler(this.chkPermitNo_CheckStateChanged);
            // 
            // chkBussName
            // 
            this.chkBussName.AutoSize = true;
            this.chkBussName.Location = new System.Drawing.Point(118, 97);
            this.chkBussName.Name = "chkBussName";
            this.chkBussName.Size = new System.Drawing.Size(99, 17);
            this.chkBussName.TabIndex = 11;
            this.chkBussName.Text = "Business Name";
            this.chkBussName.UseVisualStyleBackColor = true;
            this.chkBussName.CheckStateChanged += new System.EventHandler(this.chkBussName_CheckStateChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Order by:";
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(12, 49);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(412, 314);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToResizeRows = false;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Location = new System.Drawing.Point(27, 123);
            this.dgvList.Name = "dgvList";
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.Size = new System.Drawing.Size(386, 204);
            this.dgvList.TabIndex = 12;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkBlank);
            this.groupBox1.Controls.Add(this.chkData);
            this.groupBox1.Location = new System.Drawing.Point(27, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(386, 40);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            // 
            // chkBlank
            // 
            this.chkBlank.AutoSize = true;
            this.chkBlank.Location = new System.Drawing.Point(200, 16);
            this.chkBlank.Name = "chkBlank";
            this.chkBlank.Size = new System.Drawing.Size(99, 17);
            this.chkBlank.TabIndex = 0;
            this.chkBlank.Text = "Print blank form";
            this.chkBlank.UseVisualStyleBackColor = true;
            this.chkBlank.CheckStateChanged += new System.EventHandler(this.chkBlank_CheckStateChanged);
            // 
            // chkData
            // 
            this.chkData.AutoSize = true;
            this.chkData.Location = new System.Drawing.Point(35, 16);
            this.chkData.Name = "chkData";
            this.chkData.Size = new System.Drawing.Size(119, 17);
            this.chkData.TabIndex = 0;
            this.chkData.Text = "Print Form with data";
            this.chkData.UseVisualStyleBackColor = true;
            this.chkData.CheckStateChanged += new System.EventHandler(this.chkData_CheckStateChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 334);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Total selected businesses:";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(351, 334);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(10, 13);
            this.lblCount.TabIndex = 15;
            this.lblCount.Text = " ";
            // 
            // frmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 406);
            this.ControlBox = false;
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.chkBussName);
            this.Controls.Add(this.chkPermitNo);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbBrgy);
            this.Controls.Add(this.containerWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Accomplishment Form";
            this.Load += new System.EventHandler(this.frmForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.ComboBox cmbBrgy;
        private System.Windows.Forms.Label label1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPrint;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private System.Windows.Forms.CheckBox chkPermitNo;
        private System.Windows.Forms.CheckBox chkBussName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkBlank;
        private System.Windows.Forms.CheckBox chkData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCount;
    }
}