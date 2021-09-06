namespace Amellar.Common.PrintUtilities
{
    partial class frmMyPrintPreviewDialog
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
            this.ppcPreview = new System.Windows.Forms.PrintPreviewControl();
            this.pnlToolBar = new System.Windows.Forms.Panel();
            this.cboZoom = new System.Windows.Forms.ComboBox();
            this.lblPage = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnFirstPage = new System.Windows.Forms.Button();
            this.pnlToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // ppcPreview
            // 
            this.ppcPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ppcPreview.Location = new System.Drawing.Point(2, 31);
            this.ppcPreview.Name = "ppcPreview";
            this.ppcPreview.Size = new System.Drawing.Size(570, 399);
            this.ppcPreview.TabIndex = 0;
            this.ppcPreview.UseAntiAlias = true;
            // 
            // pnlToolBar
            // 
            this.pnlToolBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlToolBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlToolBar.Controls.Add(this.cboZoom);
            this.pnlToolBar.Controls.Add(this.lblPage);
            this.pnlToolBar.Controls.Add(this.btnExport);
            this.pnlToolBar.Controls.Add(this.btnLastPage);
            this.pnlToolBar.Controls.Add(this.btnPrevPage);
            this.pnlToolBar.Controls.Add(this.btnPrint);
            this.pnlToolBar.Controls.Add(this.btnNextPage);
            this.pnlToolBar.Controls.Add(this.btnFirstPage);
            this.pnlToolBar.Location = new System.Drawing.Point(2, 1);
            this.pnlToolBar.Name = "pnlToolBar";
            this.pnlToolBar.Size = new System.Drawing.Size(574, 32);
            this.pnlToolBar.TabIndex = 1;
            // 
            // cboZoom
            // 
            this.cboZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboZoom.FormattingEnabled = true;
            this.cboZoom.Items.AddRange(new object[] {
            "400%",
            "300%",
            "200%",
            "150%",
            "100%",
            "75%",
            "50%",
            "25%"});
            this.cboZoom.Location = new System.Drawing.Point(264, 5);
            this.cboZoom.Name = "cboZoom";
            this.cboZoom.Size = new System.Drawing.Size(84, 21);
            this.cboZoom.TabIndex = 2;
            this.cboZoom.SelectedIndexChanged += new System.EventHandler(this.cboZoom_SelectedIndexChanged);
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Location = new System.Drawing.Point(64, 8);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(78, 13);
            this.lblPage.TabIndex = 1;
            this.lblPage.Text = "XXXX of XXXX";
            // 
            // btnExport
            // 
            this.btnExport.Image = global::Amellar.Common.PrintUtilities.Properties.Resources.export;
            this.btnExport.Location = new System.Drawing.Point(231, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(27, 23);
            this.btnExport.TabIndex = 0;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnLastPage
            // 
            this.btnLastPage.Image = global::Amellar.Common.PrintUtilities.Properties.Resources.last;
            this.btnLastPage.Location = new System.Drawing.Point(170, 3);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(27, 23);
            this.btnLastPage.TabIndex = 0;
            this.btnLastPage.UseVisualStyleBackColor = true;
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.Image = global::Amellar.Common.PrintUtilities.Properties.Resources.prev;
            this.btnPrevPage.Location = new System.Drawing.Point(31, 3);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(27, 23);
            this.btnPrevPage.TabIndex = 0;
            this.btnPrevPage.UseVisualStyleBackColor = true;
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::Amellar.Common.PrintUtilities.Properties.Resources.printer;
            this.btnPrint.Location = new System.Drawing.Point(203, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(27, 23);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Image = global::Amellar.Common.PrintUtilities.Properties.Resources.next;
            this.btnNextPage.Location = new System.Drawing.Point(142, 3);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(27, 23);
            this.btnNextPage.TabIndex = 0;
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnFirstPage
            // 
            this.btnFirstPage.Image = global::Amellar.Common.PrintUtilities.Properties.Resources.first;
            this.btnFirstPage.Location = new System.Drawing.Point(3, 3);
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new System.Drawing.Size(27, 23);
            this.btnFirstPage.TabIndex = 0;
            this.btnFirstPage.UseVisualStyleBackColor = true;
            this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
            // 
            // frmMyPrintPreviewDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 431);
            this.Controls.Add(this.pnlToolBar);
            this.Controls.Add(this.ppcPreview);
            this.Name = "frmMyPrintPreviewDialog";
            this.ShowIcon = false;
            this.Text = "Print Preview";
            this.Load += new System.EventHandler(this.frmMyPrintPreviewDialog_Load);
            this.pnlToolBar.ResumeLayout(false);
            this.pnlToolBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PrintPreviewControl ppcPreview;
        private System.Windows.Forms.Panel pnlToolBar;
        private System.Windows.Forms.ComboBox cboZoom;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnFirstPage;
    }
}