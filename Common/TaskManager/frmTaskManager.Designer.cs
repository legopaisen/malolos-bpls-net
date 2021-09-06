namespace Amellar.Common.TaskManager
{
    partial class frmTaskManager
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
            this.txtDetails = new System.Windows.Forms.TextBox();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnEndTask = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnRefresh = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToResizeRows = false;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvList.Location = new System.Drawing.Point(28, 29);
            this.dgvList.Name = "dgvList";
            this.dgvList.Size = new System.Drawing.Size(439, 181);
            this.dgvList.TabIndex = 2;
            this.dgvList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvList_CellClick);
            // 
            // txtDetails
            // 
            this.txtDetails.Location = new System.Drawing.Point(28, 216);
            this.txtDetails.Multiline = true;
            this.txtDetails.Name = "txtDetails";
            this.txtDetails.ReadOnly = true;
            this.txtDetails.Size = new System.Drawing.Size(439, 46);
            this.txtDetails.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(379, 285);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(88, 25);
            this.btnClose.TabIndex = 27;
            this.btnClose.Values.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnEndTask
            // 
            this.btnEndTask.Location = new System.Drawing.Point(285, 285);
            this.btnEndTask.Name = "btnEndTask";
            this.btnEndTask.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnEndTask.Size = new System.Drawing.Size(88, 25);
            this.btnEndTask.TabIndex = 26;
            this.btnEndTask.Values.Text = "End Task";
            this.btnEndTask.Click += new System.EventHandler(this.btnEndTask_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(28, 285);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnRefresh.Size = new System.Drawing.Size(88, 25);
            this.btnRefresh.TabIndex = 26;
            this.btnRefresh.Values.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(12, 12);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(470, 267);
            this.containerWithShadow2.TabIndex = 1;
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(97, 44);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(8, 8);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // frmTaskManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(494, 321);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnEndTask);
            this.Controls.Add(this.txtDetails);
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.containerWithShadow2);
            this.Controls.Add(this.containerWithShadow1);
            this.Name = "frmTaskManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Task Manager";
            this.Load += new System.EventHandler(this.frmTaskManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.TextBox txtDetails;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnEndTask;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnRefresh;
    }
}

