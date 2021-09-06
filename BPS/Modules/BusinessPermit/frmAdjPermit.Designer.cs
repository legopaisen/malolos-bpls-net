namespace Amellar.Modules.BusinessPermit
{
    partial class frmAdjPermit
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
            this.bin1 = new Amellar.Common.BIN.BIN();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClear = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPermitYear = new System.Windows.Forms.TextBox();
            this.txtPermitSeries = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBNSName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtBNSOwner = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOwnAdd = new System.Windows.Forms.TextBox();
            this.btnUpdate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // bin1
            // 
            this.bin1.GetBINSeries = "";
            this.bin1.GetDistCode = "";
            this.bin1.GetLGUCode = "";
            this.bin1.GetTaxYear = "";
            this.bin1.Location = new System.Drawing.Point(75, 12);
            this.bin1.Name = "bin1";
            this.bin1.Size = new System.Drawing.Size(138, 20);
            this.bin1.TabIndex = 1;
            this.bin1.Leave += new System.EventHandler(this.bin1_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enter BIN:";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(219, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleOrange;
            this.btnSearch.Size = new System.Drawing.Size(97, 25);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "Search";
            this.btnSearch.Values.ExtraText = "";
            this.btnSearch.Values.Image = null;
            this.btnSearch.Values.ImageStates.ImageCheckedNormal = null;
            this.btnSearch.Values.ImageStates.ImageCheckedPressed = null;
            this.btnSearch.Values.ImageStates.ImageCheckedTracking = null;
            this.btnSearch.Values.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(322, 7);
            this.btnClear.Name = "btnClear";
            this.btnClear.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleOrange;
            this.btnClear.Size = new System.Drawing.Size(97, 25);
            this.btnClear.TabIndex = 13;
            this.btnClear.Text = "Clear";
            this.btnClear.Values.ExtraText = "";
            this.btnClear.Values.Image = null;
            this.btnClear.Values.ImageStates.ImageCheckedNormal = null;
            this.btnClear.Values.ImageStates.ImageCheckedPressed = null;
            this.btnClear.Values.ImageStates.ImageCheckedTracking = null;
            this.btnClear.Values.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Permit Number:";
            // 
            // txtPermitYear
            // 
            this.txtPermitYear.Location = new System.Drawing.Point(100, 13);
            this.txtPermitYear.Name = "txtPermitYear";
            this.txtPermitYear.ReadOnly = true;
            this.txtPermitYear.Size = new System.Drawing.Size(32, 20);
            this.txtPermitYear.TabIndex = 15;
            // 
            // txtPermitSeries
            // 
            this.txtPermitSeries.Location = new System.Drawing.Point(138, 13);
            this.txtPermitSeries.Name = "txtPermitSeries";
            this.txtPermitSeries.ReadOnly = true;
            this.txtPermitSeries.Size = new System.Drawing.Size(39, 20);
            this.txtPermitSeries.TabIndex = 16;
            this.txtPermitSeries.Leave += new System.EventHandler(this.txtPermitSeries_Leave);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtPermitSeries);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtPermitYear);
            this.groupBox1.Location = new System.Drawing.Point(12, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(472, 42);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // txtBNSName
            // 
            this.txtBNSName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBNSName.Location = new System.Drawing.Point(100, 13);
            this.txtBNSName.Name = "txtBNSName";
            this.txtBNSName.ReadOnly = true;
            this.txtBNSName.Size = new System.Drawing.Size(366, 20);
            this.txtBNSName.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Business Name:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtBNSName);
            this.groupBox2.Location = new System.Drawing.Point(12, 80);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(472, 42);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtOwnAdd);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txtBNSOwner);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(12, 122);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(472, 71);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            // 
            // txtBNSOwner
            // 
            this.txtBNSOwner.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBNSOwner.Location = new System.Drawing.Point(100, 13);
            this.txtBNSOwner.Name = "txtBNSOwner";
            this.txtBNSOwner.ReadOnly = true;
            this.txtBNSOwner.Size = new System.Drawing.Size(366, 20);
            this.txtBNSOwner.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Owner:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Owner\'s Address:";
            // 
            // txtOwnAdd
            // 
            this.txtOwnAdd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtOwnAdd.Location = new System.Drawing.Point(101, 39);
            this.txtOwnAdd.Name = "txtOwnAdd";
            this.txtOwnAdd.ReadOnly = true;
            this.txtOwnAdd.Size = new System.Drawing.Size(366, 20);
            this.txtOwnAdd.TabIndex = 16;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(284, 204);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleOrange;
            this.btnUpdate.Size = new System.Drawing.Size(97, 25);
            this.btnUpdate.TabIndex = 22;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Values.ExtraText = "";
            this.btnUpdate.Values.Image = null;
            this.btnUpdate.Values.ImageStates.ImageCheckedNormal = null;
            this.btnUpdate.Values.ImageStates.ImageCheckedPressed = null;
            this.btnUpdate.Values.ImageStates.ImageCheckedTracking = null;
            this.btnUpdate.Values.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(387, 204);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.SparkleOrange;
            this.btnCancel.Size = new System.Drawing.Size(97, 25);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Values.ExtraText = "";
            this.btnCancel.Values.Image = null;
            this.btnCancel.Values.ImageStates.ImageCheckedNormal = null;
            this.btnCancel.Values.ImageStates.ImageCheckedPressed = null;
            this.btnCancel.Values.ImageStates.ImageCheckedTracking = null;
            this.btnCancel.Values.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmAdjPermit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 240);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bin1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAdjPermit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Adj Business Permit";
            this.Load += new System.EventHandler(this.frmAdjPermit_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Amellar.Common.BIN.BIN bin1;
        private System.Windows.Forms.Label label1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPermitYear;
        private System.Windows.Forms.TextBox txtPermitSeries;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtBNSName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtOwnAdd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBNSOwner;
        private System.Windows.Forms.Label label4;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnUpdate;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;

    }
}