namespace Amellar.Modules.HealthPermit
{
    partial class frmPermitTagging
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
            this.btnTag = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClose = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.bussTypeList = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.bussTypeList)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTag
            // 
            this.btnTag.Location = new System.Drawing.Point(246, 317);
            this.btnTag.Name = "btnTag";
            this.btnTag.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnTag.Size = new System.Drawing.Size(68, 24);
            this.btnTag.TabIndex = 35;
            this.btnTag.Text = "&Tag";
            this.btnTag.Values.ExtraText = "";
            this.btnTag.Values.Image = null;
            this.btnTag.Values.ImageStates.ImageCheckedNormal = null;
            this.btnTag.Values.ImageStates.ImageCheckedPressed = null;
            this.btnTag.Values.ImageStates.ImageCheckedTracking = null;
            this.btnTag.Values.Text = "&Tag";
            this.btnTag.Click += new System.EventHandler(this.btnTag_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(320, 317);
            this.btnClose.Name = "btnClose";
            this.btnClose.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnClose.Size = new System.Drawing.Size(68, 24);
            this.btnClose.TabIndex = 38;
            this.btnClose.Text = "&Close";
            this.btnClose.Values.ExtraText = "";
            this.btnClose.Values.Image = null;
            this.btnClose.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClose.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClose.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClose.Values.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(11, 9);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(377, 302);
            this.containerWithShadow2.TabIndex = 34;
            // 
            // bussTypeList
            // 
            this.bussTypeList.AllowUserToAddRows = false;
            this.bussTypeList.AllowUserToDeleteRows = false;
            this.bussTypeList.AllowUserToOrderColumns = true;
            this.bussTypeList.AllowUserToResizeColumns = false;
            this.bussTypeList.AllowUserToResizeRows = false;
            this.bussTypeList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.bussTypeList.Location = new System.Drawing.Point(21, 21);
            this.bussTypeList.Name = "bussTypeList";
            this.bussTypeList.RowHeadersVisible = false;
            this.bussTypeList.Size = new System.Drawing.Size(355, 276);
            this.bussTypeList.TabIndex = 39;
            // 
            // frmPermitTagging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(393, 349);
            this.ControlBox = false;
            this.Controls.Add(this.bussTypeList);
            this.Controls.Add(this.containerWithShadow2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnTag);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "frmPermitTagging";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Special Permit Tagging";
            this.Load += new System.EventHandler(this.frmPermitTagging_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bussTypeList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton btnTag;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClose;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private System.Windows.Forms.DataGridView bussTypeList;
    }
}