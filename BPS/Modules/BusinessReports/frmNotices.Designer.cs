namespace Amellar.Modules.BusinessReports
{
    partial class frmNotices
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
            this.containerWithShadow1 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.rdoRenewal = new System.Windows.Forms.RadioButton();
            this.rdoReassess = new System.Windows.Forms.RadioButton();
            this.containerWithShadow2 = new Amellar.Common.ContainerWithShadow.ContainerWithShadow();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPrint = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBnsName = new System.Windows.Forms.TextBox();
            this.txtBnsOwn = new System.Windows.Forms.TextBox();
            this.bin1 = new Amellar.Common.BIN.BIN();
            this.rdbNoticeClosure = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // containerWithShadow1
            // 
            this.containerWithShadow1.Location = new System.Drawing.Point(12, 12);
            this.containerWithShadow1.Name = "containerWithShadow1";
            this.containerWithShadow1.Size = new System.Drawing.Size(324, 110);
            this.containerWithShadow1.TabIndex = 0;
            // 
            // rdoRenewal
            // 
            this.rdoRenewal.AutoSize = true;
            this.rdoRenewal.Location = new System.Drawing.Point(38, 29);
            this.rdoRenewal.Name = "rdoRenewal";
            this.rdoRenewal.Size = new System.Drawing.Size(116, 17);
            this.rdoRenewal.TabIndex = 1;
            this.rdoRenewal.TabStop = true;
            this.rdoRenewal.Text = "Notice for Renewal";
            this.rdoRenewal.UseVisualStyleBackColor = true;
            this.rdoRenewal.CheckedChanged += new System.EventHandler(this.rdoRenewal_CheckedChanged);
            // 
            // rdoReassess
            // 
            this.rdoReassess.AutoSize = true;
            this.rdoReassess.Location = new System.Drawing.Point(38, 75);
            this.rdoReassess.Name = "rdoReassess";
            this.rdoReassess.Size = new System.Drawing.Size(146, 17);
            this.rdoReassess.TabIndex = 2;
            this.rdoReassess.TabStop = true;
            this.rdoReassess.Text = "Notice for Re-assessment";
            this.rdoReassess.UseVisualStyleBackColor = true;
            this.rdoReassess.Visible = false;
            this.rdoReassess.CheckedChanged += new System.EventHandler(this.rdoReassess_CheckedChanged);
            // 
            // containerWithShadow2
            // 
            this.containerWithShadow2.Location = new System.Drawing.Point(12, 128);
            this.containerWithShadow2.Name = "containerWithShadow2";
            this.containerWithShadow2.Size = new System.Drawing.Size(324, 163);
            this.containerWithShadow2.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "BIN";
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(167, 297);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnPrint.Size = new System.Drawing.Size(74, 24);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "Print";
            this.btnPrint.Values.ExtraText = "";
            this.btnPrint.Values.Image = null;
            this.btnPrint.Values.ImageStates.ImageCheckedNormal = null;
            this.btnPrint.Values.ImageStates.ImageCheckedPressed = null;
            this.btnPrint.Values.ImageStates.ImageCheckedTracking = null;
            this.btnPrint.Values.Text = "Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(247, 297);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(74, 24);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Close";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(247, 141);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnSearch.Size = new System.Drawing.Size(74, 24);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.Values.ExtraText = "";
            this.btnSearch.Values.Image = null;
            this.btnSearch.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearch.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearch.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Business Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 236);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Owner\'s Name";
            // 
            // txtBnsName
            // 
            this.txtBnsName.Location = new System.Drawing.Point(44, 186);
            this.txtBnsName.Multiline = true;
            this.txtBnsName.Name = "txtBnsName";
            this.txtBnsName.ReadOnly = true;
            this.txtBnsName.Size = new System.Drawing.Size(277, 43);
            this.txtBnsName.TabIndex = 31;
            // 
            // txtBnsOwn
            // 
            this.txtBnsOwn.Location = new System.Drawing.Point(44, 252);
            this.txtBnsOwn.Name = "txtBnsOwn";
            this.txtBnsOwn.ReadOnly = true;
            this.txtBnsOwn.Size = new System.Drawing.Size(277, 20);
            this.txtBnsOwn.TabIndex = 32;
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(74, 142);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(148, 23);
            this.bin1.TabIndex = 3;
            // 
            // rdbNoticeClosure
            // 
            this.rdbNoticeClosure.AutoSize = true;
            this.rdbNoticeClosure.Location = new System.Drawing.Point(38, 52);
            this.rdbNoticeClosure.Name = "rdbNoticeClosure";
            this.rdbNoticeClosure.Size = new System.Drawing.Size(106, 17);
            this.rdbNoticeClosure.TabIndex = 33;
            this.rdbNoticeClosure.TabStop = true;
            this.rdbNoticeClosure.Text = "Notice of Closure";
            this.rdbNoticeClosure.UseVisualStyleBackColor = true;
            // 
            // frmNotices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 334);
            this.ControlBox = false;
            this.Controls.Add(this.rdbNoticeClosure);
            this.Controls.Add(this.txtBnsOwn);
            this.Controls.Add(this.txtBnsName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bin1);
            this.Controls.Add(this.rdoReassess);
            this.Controls.Add(this.rdoRenewal);
            this.Controls.Add(this.containerWithShadow1);
            this.Controls.Add(this.containerWithShadow2);
            this.Name = "frmNotices";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Notices";
            this.Load += new System.EventHandler(this.frmNotices_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow1;
        private System.Windows.Forms.RadioButton rdoRenewal;
        private System.Windows.Forms.RadioButton rdoReassess;
        private Amellar.Common.BIN.BIN bin1;
        private Amellar.Common.ContainerWithShadow.ContainerWithShadow containerWithShadow2;
        private System.Windows.Forms.Label label1;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnPrint;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        public ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBnsName;
        private System.Windows.Forms.TextBox txtBnsOwn;
        private System.Windows.Forms.RadioButton rdbNoticeClosure;
    }
}