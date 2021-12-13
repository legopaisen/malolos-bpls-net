namespace Amellar.Modules.HealthPermit
{
    partial class frmPendingApproval
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
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnTrail = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.bin2 = new BIN.BIN();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(848, 451);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(92, 28);
            this.btnClose.TabIndex = 33;
            this.btnClose.Text = "Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(318, 451);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(92, 28);
            this.btnSearch.TabIndex = 37;
            this.btnSearch.Text = "Search";
            this.btnSearch.Values.ExtraText = "";
            this.btnSearch.Values.Image = null;
            this.btnSearch.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearch.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearch.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnTrail
            // 
            this.btnTrail.Location = new System.Drawing.Point(416, 451);
            this.btnTrail.Name = "btnTrail";
            this.btnTrail.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnTrail.Size = new System.Drawing.Size(92, 28);
            this.btnTrail.TabIndex = 38;
            this.btnTrail.Text = "Approval Trail";
            this.btnTrail.Values.ExtraText = "";
            this.btnTrail.Values.Image = null;
            this.btnTrail.Values.ImageStates.ImageCheckedNormal = null;
            this.btnTrail.Values.ImageStates.ImageCheckedPressed = null;
            this.btnTrail.Values.ImageStates.ImageCheckedTracking = null;
            this.btnTrail.Values.Text = "Approval Trail";
            this.btnTrail.Click += new System.EventHandler(this.btnTrail_Click);
            // 
            // bin2
            // 
            this.bin2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bin2.GetBINSeries = "";
            this.bin2.GetDistCode = "";
            this.bin2.GetLGUCode = "";
            this.bin2.GetTaxYear = "";
            this.bin2.Location = new System.Drawing.Point(84, 456);
            this.bin2.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.bin2.Name = "bin2";
            this.bin2.Size = new System.Drawing.Size(197, 23);
            this.bin2.TabIndex = 39;
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvList.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvList.Location = new System.Drawing.Point(9, 10);
            this.dgvList.Margin = new System.Windows.Forms.Padding(2);
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.RowTemplate.Height = 24;
            this.dgvList.Size = new System.Drawing.Size(932, 423);
            this.dgvList.TabIndex = 0;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(47, 462);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 17);
            this.label1.TabIndex = 36;
            this.label1.Text = "BIN";
            // 
            // frmPendingApproval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 491);
            this.ControlBox = false;
            this.Controls.Add(this.bin2);
            this.Controls.Add(this.btnTrail);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvList);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "frmPendingApproval";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pending Approval";
            this.Load += new System.EventHandler(this.frmPendingApproval_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnTrail;
        private BIN.BIN bin2;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.Label label1;
    }
}