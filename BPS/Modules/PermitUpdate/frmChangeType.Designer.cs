namespace Amellar.Modules.PermitUpdate
{
    partial class frmChangeType
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
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.btnAddlInfo = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPermit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSave = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Location = new System.Drawing.Point(24, 22);
            this.dgvList.Name = "dgvList";
            this.dgvList.Size = new System.Drawing.Size(488, 189);
            this.dgvList.TabIndex = 0;
            this.dgvList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellEndEdit);
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            this.dgvList.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellEnter);
            this.dgvList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellContentClick);
            // 
            // btnAddlInfo
            // 
            this.btnAddlInfo.Location = new System.Drawing.Point(24, 229);
            this.btnAddlInfo.Name = "btnAddlInfo";
            this.btnAddlInfo.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnAddlInfo.Size = new System.Drawing.Size(151, 25);
            this.btnAddlInfo.TabIndex = 5;
            this.btnAddlInfo.Text = "Additional Business Info";
            this.btnAddlInfo.Values.ExtraText = "";
            this.btnAddlInfo.Values.Image = null;
            this.btnAddlInfo.Values.ImageStates.ImageCheckedNormal = null;
            this.btnAddlInfo.Values.ImageStates.ImageCheckedPressed = null;
            this.btnAddlInfo.Values.ImageStates.ImageCheckedTracking = null;
            this.btnAddlInfo.Values.Text = "Additional Business Info";
            this.btnAddlInfo.Click += new System.EventHandler(this.btnAddlInfo_Click);
            // 
            // btnPermit
            // 
            this.btnPermit.Location = new System.Drawing.Point(181, 229);
            this.btnPermit.Name = "btnPermit";
            this.btnPermit.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPermit.Size = new System.Drawing.Size(101, 25);
            this.btnPermit.TabIndex = 5;
            this.btnPermit.Text = "Compute Permit";
            this.btnPermit.Values.ExtraText = "";
            this.btnPermit.Values.Image = null;
            this.btnPermit.Values.ImageStates.ImageCheckedNormal = null;
            this.btnPermit.Values.ImageStates.ImageCheckedPressed = null;
            this.btnPermit.Values.ImageStates.ImageCheckedTracking = null;
            this.btnPermit.Values.Text = "Compute Permit";
            this.btnPermit.Click += new System.EventHandler(this.btnPermit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(322, 229);
            this.btnSave.Name = "btnSave";
            this.btnSave.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSave.Size = new System.Drawing.Size(92, 25);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.Values.ExtraText = "";
            this.btnSave.Values.Image = null;
            this.btnSave.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSave.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSave.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSave.Values.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(420, 229);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(92, 25);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(11, 9);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(513, 266);
            this.containerWithShadow1.TabIndex = 6;
            // 
            // frmChangeType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 280);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnPermit);
            this.Controls.Add(this.btnAddlInfo);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.containerWithShadow1);
            this.Name = "frmChangeType";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change of Business Nature";
            this.Load += new System.EventHandler(this.frmChangeType_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvList;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAddlInfo;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPermit;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSave;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
    }
}