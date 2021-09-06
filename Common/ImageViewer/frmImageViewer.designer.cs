namespace Amellar.Common.ImageViewer
{
    partial class frmImageViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImageViewer));
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblImageList = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.chkZoom = new System.Windows.Forms.CheckBox();
            this.chkPan = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.picImageViewer = new Amellar.Common.ImageViewer.PictureBoxCtrl();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBrowse.Location = new System.Drawing.Point(133, 601);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(58, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.TabStop = false;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnZoomIn.Location = new System.Drawing.Point(82, 601);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(20, 23);
            this.btnZoomIn.TabIndex = 2;
            this.btnZoomIn.TabStop = false;
            this.btnZoomIn.Text = "+";
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnZoomOut.Location = new System.Drawing.Point(106, 601);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(21, 23);
            this.btnZoomOut.TabIndex = 2;
            this.btnZoomOut.TabStop = false;
            this.btnZoomOut.Text = "-";
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrev.Location = new System.Drawing.Point(287, 601);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(43, 23);
            this.btnPrev.TabIndex = 4;
            this.btnPrev.TabStop = false;
            this.btnPrev.Text = "<";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblImageList
            // 
            this.lblImageList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblImageList.AutoSize = true;
            this.lblImageList.Location = new System.Drawing.Point(247, 607);
            this.lblImageList.Name = "lblImageList";
            this.lblImageList.Size = new System.Drawing.Size(34, 13);
            this.lblImageList.TabIndex = 5;
            this.lblImageList.Text = "0 of 0";
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(336, 601);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(43, 23);
            this.btnNext.TabIndex = 4;
            this.btnNext.TabStop = false;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(454, 601);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(66, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = global::Common.ImageViewer.Properties.Resources.SAVEME;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(382, 601);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(66, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.TabStop = false;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chkZoom
            // 
            this.chkZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkZoom.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkZoom.AutoSize = true;
            this.chkZoom.Image = global::Common.ImageViewer.Properties.Resources.zoom;
            this.chkZoom.Location = new System.Drawing.Point(52, 601);
            this.chkZoom.Name = "chkZoom";
            this.chkZoom.Size = new System.Drawing.Size(24, 24);
            this.chkZoom.TabIndex = 3;
            this.chkZoom.TabStop = false;
            this.chkZoom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkZoom.UseVisualStyleBackColor = true;
            this.chkZoom.CheckedChanged += new System.EventHandler(this.chkZoom_CheckedChanged);
            // 
            // chkPan
            // 
            this.chkPan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkPan.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkPan.AutoSize = true;
            this.chkPan.Image = global::Common.ImageViewer.Properties.Resources.pan;
            this.chkPan.Location = new System.Drawing.Point(24, 600);
            this.chkPan.Name = "chkPan";
            this.chkPan.Size = new System.Drawing.Size(24, 24);
            this.chkPan.TabIndex = 3;
            this.chkPan.TabStop = false;
            this.chkPan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkPan.UseVisualStyleBackColor = true;
            this.chkPan.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.chkPan_KeyPress);
            this.chkPan.Enter += new System.EventHandler(this.chkPan_Enter);
            this.chkPan.CheckedChanged += new System.EventHandler(this.chkPan_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.Location = new System.Drawing.Point(198, 601);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(22, 23);
            this.button1.TabIndex = 7;
            this.button1.TabStop = false;
            this.button1.Text = "L";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(219, 601);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(22, 23);
            this.button2.TabIndex = 8;
            this.button2.TabStop = false;
            this.button2.Text = "R";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // picImageViewer
            // 
            this.picImageViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.picImageViewer.AutoScroll = true;
            this.picImageViewer.AutoSize = true;
            this.picImageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picImageViewer.Extensions = ((System.Collections.Generic.List<string>)(resources.GetObject("picImageViewer.Extensions")));
            this.picImageViewer.Image = null;
            this.picImageViewer.ImageCount = 0;
            this.picImageViewer.ImageIndex = 0;
            this.picImageViewer.Images = ((System.Collections.Generic.List<System.Drawing.Image>)(resources.GetObject("picImageViewer.Images")));
            this.picImageViewer.Location = new System.Drawing.Point(1, 0);
            this.picImageViewer.MagnificationMinMaxStep = 5;
            this.picImageViewer.MagnificationScale = 1.25;
            this.picImageViewer.Name = "picImageViewer";
            this.picImageViewer.Names = ((System.Collections.Generic.List<string>)(resources.GetObject("picImageViewer.Names")));
            this.picImageViewer.Paths = ((System.Collections.Generic.List<string>)(resources.GetObject("picImageViewer.Paths")));
            this.picImageViewer.Size = new System.Drawing.Size(591, 597);
            this.picImageViewer.TabIndex = 0;
            this.picImageViewer.View = Amellar.Common.ImageViewer.PictureBoxCtrl.ViewMode.PanMode;
            this.picImageViewer.Load += new System.EventHandler(this.picImageViewer_Load);
            this.picImageViewer.DragDrop += new System.Windows.Forms.DragEventHandler(this.picImageViewer_DragDrop);
            this.picImageViewer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.picImageViewer_KeyDown);
            // 
            // frmImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 636);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblImageList);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.chkZoom);
            this.Controls.Add(this.chkPan);
            this.Controls.Add(this.btnZoomOut);
            this.Controls.Add(this.btnZoomIn);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.picImageViewer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "frmImageViewer";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image Viewer";
            this.Load += new System.EventHandler(this.frmImageViewer_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmImageViewer_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmImageViewer_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.CheckBox chkPan;
        private System.Windows.Forms.CheckBox chkZoom;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label lblImageList;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public PictureBoxCtrl picImageViewer;


    }
}

