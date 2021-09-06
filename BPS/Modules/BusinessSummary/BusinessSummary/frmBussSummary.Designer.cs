namespace BusinessSummary
{
    partial class frmBussSummary
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
            this.grpFilter = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.rbCapitalRange = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rbLineBusiness = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rbGrossRange = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rbDist = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rbOrgKind = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.rbBarangay = new ComponentFactory.Krypton.Toolkit.KryptonRadioButton();
            this.grpHeader = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblRegYr = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.txtYear = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.grpFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpFilter.Panel)).BeginInit();
            this.grpFilter.Panel.SuspendLayout();
            this.grpFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpFilter
            // 
            this.grpFilter.Location = new System.Drawing.Point(12, 12);
            this.grpFilter.Name = "grpFilter";
            // 
            // grpFilter.Panel
            // 
            this.grpFilter.Panel.Controls.Add(this.rbCapitalRange);
            this.grpFilter.Panel.Controls.Add(this.rbLineBusiness);
            this.grpFilter.Panel.Controls.Add(this.rbGrossRange);
            this.grpFilter.Panel.Controls.Add(this.rbDist);
            this.grpFilter.Panel.Controls.Add(this.rbOrgKind);
            this.grpFilter.Panel.Controls.Add(this.rbBarangay);
            this.grpFilter.Panel.Controls.Add(this.grpHeader);
            this.grpFilter.Size = new System.Drawing.Size(143, 199);
            this.grpFilter.TabIndex = 0;
            // 
            // rbCapitalRange
            // 
            this.rbCapitalRange.Location = new System.Drawing.Point(12, 165);
            this.rbCapitalRange.Name = "rbCapitalRange";
            this.rbCapitalRange.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.rbCapitalRange.Size = new System.Drawing.Size(98, 20);
            this.rbCapitalRange.TabIndex = 1;
            this.rbCapitalRange.Text = "Capital Range";
            this.rbCapitalRange.Values.ExtraText = "";
            this.rbCapitalRange.Values.Image = null;
            this.rbCapitalRange.Values.Text = "Capital Range";
            // 
            // rbLineBusiness
            // 
            this.rbLineBusiness.Location = new System.Drawing.Point(12, 87);
            this.rbLineBusiness.Name = "rbLineBusiness";
            this.rbLineBusiness.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.rbLineBusiness.Size = new System.Drawing.Size(109, 20);
            this.rbLineBusiness.TabIndex = 1;
            this.rbLineBusiness.Text = "Line of Business";
            this.rbLineBusiness.Values.ExtraText = "";
            this.rbLineBusiness.Values.Image = null;
            this.rbLineBusiness.Values.Text = "Line of Business";
            this.rbLineBusiness.CheckedChanged += new System.EventHandler(this.rbLineBusiness_CheckedChanged);
            // 
            // rbGrossRange
            // 
            this.rbGrossRange.Location = new System.Drawing.Point(12, 139);
            this.rbGrossRange.Name = "rbGrossRange";
            this.rbGrossRange.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.rbGrossRange.Size = new System.Drawing.Size(91, 20);
            this.rbGrossRange.TabIndex = 1;
            this.rbGrossRange.Text = "Gross Range";
            this.rbGrossRange.Values.ExtraText = "";
            this.rbGrossRange.Values.Image = null;
            this.rbGrossRange.Values.Text = "Gross Range";
            // 
            // rbDist
            // 
            this.rbDist.Location = new System.Drawing.Point(12, 61);
            this.rbDist.Name = "rbDist";
            this.rbDist.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.rbDist.Size = new System.Drawing.Size(61, 20);
            this.rbDist.TabIndex = 1;
            this.rbDist.Text = "District";
            this.rbDist.Values.ExtraText = "";
            this.rbDist.Values.Image = null;
            this.rbDist.Values.Text = "District";
            // 
            // rbOrgKind
            // 
            this.rbOrgKind.Location = new System.Drawing.Point(12, 113);
            this.rbOrgKind.Name = "rbOrgKind";
            this.rbOrgKind.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.rbOrgKind.Size = new System.Drawing.Size(121, 20);
            this.rbOrgKind.TabIndex = 1;
            this.rbOrgKind.Text = "Organization Kind";
            this.rbOrgKind.Values.ExtraText = "";
            this.rbOrgKind.Values.Image = null;
            this.rbOrgKind.Values.Text = "Organization Kind";
            // 
            // rbBarangay
            // 
            this.rbBarangay.Checked = true;
            this.rbBarangay.Location = new System.Drawing.Point(12, 35);
            this.rbBarangay.Name = "rbBarangay";
            this.rbBarangay.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.rbBarangay.Size = new System.Drawing.Size(73, 20);
            this.rbBarangay.TabIndex = 1;
            this.rbBarangay.Text = "Barangay";
            this.rbBarangay.Values.ExtraText = "";
            this.rbBarangay.Values.Image = null;
            this.rbBarangay.Values.Text = "Barangay";
            // 
            // grpHeader
            // 
            this.grpHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpHeader.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.grpHeader.Location = new System.Drawing.Point(0, 0);
            this.grpHeader.Name = "grpHeader";
            this.grpHeader.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.grpHeader.Size = new System.Drawing.Size(141, 24);
            this.grpHeader.TabIndex = 0;
            this.grpHeader.Text = "Filter by";
            this.grpHeader.Values.Description = "";
            this.grpHeader.Values.Heading = "Filter by";
            this.grpHeader.Values.Image = null;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(161, 187);
            this.btnOk.Name = "btnOk";
            this.btnOk.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnOk.Size = new System.Drawing.Size(75, 24);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&Ok";
            this.btnOk.Values.ExtraText = "";
            this.btnOk.Values.Image = null;
            this.btnOk.Values.ImageStates.ImageCheckedNormal = null;
            this.btnOk.Values.ImageStates.ImageCheckedPressed = null;
            this.btnOk.Values.ImageStates.ImageCheckedTracking = null;
            this.btnOk.Values.Text = "&Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(242, 187);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblRegYr
            // 
            this.lblRegYr.Location = new System.Drawing.Point(191, 17);
            this.lblRegYr.Name = "lblRegYr";
            this.lblRegYr.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.lblRegYr.Size = new System.Drawing.Size(103, 20);
            this.lblRegYr.TabIndex = 3;
            this.lblRegYr.Text = "Registration Year";
            this.lblRegYr.Values.ExtraText = "";
            this.lblRegYr.Values.Image = null;
            this.lblRegYr.Values.Text = "Registration Year";
            // 
            // txtYear
            // 
            this.txtYear.AlwaysActive = false;
            this.txtYear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtYear.Enabled = false;
            this.txtYear.InputControlStyle = ComponentFactory.Krypton.Toolkit.InputControlStyle.Ribbon;
            this.txtYear.Location = new System.Drawing.Point(191, 48);
            this.txtYear.MaxLength = 4;
            this.txtYear.Name = "txtYear";
            this.txtYear.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleBlue;
            this.txtYear.Size = new System.Drawing.Size(100, 20);
            this.txtYear.TabIndex = 4;
            // 
            // frmBussSummary
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 227);
            this.ControlBox = false;
            this.Controls.Add(this.txtYear);
            this.Controls.Add(this.lblRegYr);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.grpFilter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmBussSummary";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Summary of Business";
            this.Load += new System.EventHandler(this.frmBussSummary_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grpFilter.Panel)).EndInit();
            this.grpFilter.Panel.ResumeLayout(false);
            this.grpFilter.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpFilter)).EndInit();
            this.grpFilter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonGroup grpFilter;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader grpHeader;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbCapitalRange;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbLineBusiness;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbGrossRange;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbDist;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbOrgKind;
        private ComponentFactory.Krypton.Toolkit.KryptonRadioButton rbBarangay;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRegYr;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtYear;

    }
}

