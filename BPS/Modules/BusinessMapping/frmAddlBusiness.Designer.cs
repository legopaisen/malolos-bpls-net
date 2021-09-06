namespace Amellar.Modules.BusinessMapping
{
    partial class frmAddlBusiness
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
            this.dgvOtherLine = new System.Windows.Forms.DataGridView();
            this.grpOtherLine = new System.Windows.Forms.GroupBox();
            this.grpApplyOtherLine = new System.Windows.Forms.GroupBox();
            this.dgvApplyOtherLine = new System.Windows.Forms.DataGridView();
            this.btnSave = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.btnBnsType = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOtherLine)).BeginInit();
            this.grpOtherLine.SuspendLayout();
            this.grpApplyOtherLine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplyOtherLine)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvOtherLine
            // 
            this.dgvOtherLine.AllowUserToAddRows = false;
            this.dgvOtherLine.AllowUserToDeleteRows = false;
            this.dgvOtherLine.AllowUserToResizeRows = false;
            this.dgvOtherLine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOtherLine.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvOtherLine.Location = new System.Drawing.Point(7, 19);
            this.dgvOtherLine.Name = "dgvOtherLine";
            this.dgvOtherLine.Size = new System.Drawing.Size(618, 142);
            this.dgvOtherLine.TabIndex = 1;
            // 
            // grpOtherLine
            // 
            this.grpOtherLine.Controls.Add(this.dgvOtherLine);
            this.grpOtherLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpOtherLine.Location = new System.Drawing.Point(25, 24);
            this.grpOtherLine.Name = "grpOtherLine";
            this.grpOtherLine.Size = new System.Drawing.Size(633, 170);
            this.grpOtherLine.TabIndex = 2;
            this.grpOtherLine.TabStop = false;
            this.grpOtherLine.Text = " Other Line/s of Business ";
            // 
            // grpApplyOtherLine
            // 
            this.grpApplyOtherLine.Controls.Add(this.dgvApplyOtherLine);
            this.grpApplyOtherLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpApplyOtherLine.Location = new System.Drawing.Point(25, 200);
            this.grpApplyOtherLine.Name = "grpApplyOtherLine";
            this.grpApplyOtherLine.Size = new System.Drawing.Size(633, 170);
            this.grpApplyOtherLine.TabIndex = 2;
            this.grpApplyOtherLine.TabStop = false;
            this.grpApplyOtherLine.Text = " Apply Other Line/s of Business ";
            // 
            // dgvApplyOtherLine
            // 
            this.dgvApplyOtherLine.AllowUserToAddRows = false;
            this.dgvApplyOtherLine.AllowUserToDeleteRows = false;
            this.dgvApplyOtherLine.AllowUserToResizeRows = false;
            this.dgvApplyOtherLine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvApplyOtherLine.Location = new System.Drawing.Point(7, 19);
            this.dgvApplyOtherLine.Name = "dgvApplyOtherLine";
            this.dgvApplyOtherLine.Size = new System.Drawing.Size(618, 142);
            this.dgvApplyOtherLine.TabIndex = 1;
            this.dgvApplyOtherLine.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvApplyOtherLine_MouseDown);
            this.dgvApplyOtherLine.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvApplyOtherLine_CellBeginEdit);
            this.dgvApplyOtherLine.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvApplyOtherLine_CellEndEdit);
            this.dgvApplyOtherLine.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvApplyOtherLine_CellClick);
            // 
            // btnSave
            // 
            this.btnSave.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnSave.Location = new System.Drawing.Point(468, 394);
            this.btnSave.Name = "btnSave";
            this.btnSave.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSave.Size = new System.Drawing.Size(92, 25);
            this.btnSave.TabIndex = 9;
            this.btnSave.Values.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnClose.Location = new System.Drawing.Point(566, 394);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(92, 25);
            this.btnClose.TabIndex = 10;
            this.btnClose.Values.Text = "Cancel";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(12, 12);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(657, 380);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // btnBnsType
            // 
            this.btnBnsType.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Standalone;
            this.btnBnsType.Location = new System.Drawing.Point(32, 394);
            this.btnBnsType.Name = "btnBnsType";
            this.btnBnsType.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnBnsType.Size = new System.Drawing.Size(128, 25);
            this.btnBnsType.TabIndex = 26;
            this.btnBnsType.Values.Text = "Business Types";
            this.btnBnsType.Click += new System.EventHandler(this.btnBnsType_Click);
            // 
            // frmAddlBusiness
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 423);
            this.ControlBox = false;
            this.Controls.Add(this.btnBnsType);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grpApplyOtherLine);
            this.Controls.Add(this.grpOtherLine);
            this.Controls.Add(this.containerWithShadow1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmAddlBusiness";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Other line/s of Business";
            this.Load += new System.EventHandler(this.frmAddlBusiness_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOtherLine)).EndInit();
            this.grpOtherLine.ResumeLayout(false);
            this.grpApplyOtherLine.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplyOtherLine)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.DataGridView dgvOtherLine;
        private System.Windows.Forms.GroupBox grpOtherLine;
        private System.Windows.Forms.GroupBox grpApplyOtherLine;
        private System.Windows.Forms.DataGridView dgvApplyOtherLine;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSave;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnBnsType;
    }
}