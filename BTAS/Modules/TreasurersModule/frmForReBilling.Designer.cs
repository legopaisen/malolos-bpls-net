namespace Amellar.BPLS.TreasurersModule
{
    partial class frmForReBilling
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
            this.dgvForReBilling = new System.Windows.Forms.DataGridView();
            this.txtMemoranda = new System.Windows.Forms.TextBox();
            this.btnBill = new System.Windows.Forms.Button();
            this.btnSkip = new System.Windows.Forms.Button();
            this.frameWithShadow1 = new Amellar.Common.SOA.FrameWithShadow();
            this.frameWithShadow2 = new Amellar.Common.SOA.FrameWithShadow();
            ((System.ComponentModel.ISupportInitialize)(this.dgvForReBilling)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvForReBilling
            // 
            this.dgvForReBilling.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvForReBilling.Location = new System.Drawing.Point(24, 20);
            this.dgvForReBilling.Name = "dgvForReBilling";
            this.dgvForReBilling.ReadOnly = true;
            this.dgvForReBilling.Size = new System.Drawing.Size(541, 175);
            this.dgvForReBilling.TabIndex = 0;
            this.dgvForReBilling.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvForReBilling_CellMouseClick);
            // 
            // txtMemoranda
            // 
            this.txtMemoranda.Location = new System.Drawing.Point(24, 228);
            this.txtMemoranda.Multiline = true;
            this.txtMemoranda.Name = "txtMemoranda";
            this.txtMemoranda.ReadOnly = true;
            this.txtMemoranda.Size = new System.Drawing.Size(541, 96);
            this.txtMemoranda.TabIndex = 3;
            // 
            // btnBill
            // 
            this.btnBill.Location = new System.Drawing.Point(212, 345);
            this.btnBill.Name = "btnBill";
            this.btnBill.Size = new System.Drawing.Size(75, 23);
            this.btnBill.TabIndex = 6;
            this.btnBill.Text = "&Bill";
            this.btnBill.UseVisualStyleBackColor = true;
            this.btnBill.Click += new System.EventHandler(this.btnBill_Click);
            // 
            // btnSkip
            // 
            this.btnSkip.Location = new System.Drawing.Point(293, 345);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(75, 23);
            this.btnSkip.TabIndex = 7;
            this.btnSkip.Text = "&Skip";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // frameWithShadow1
            // 
            this.frameWithShadow1.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow1.Location = new System.Drawing.Point(11, 10);
            this.frameWithShadow1.Name = "frameWithShadow1";
            this.frameWithShadow1.Size = new System.Drawing.Size(566, 199);
            this.frameWithShadow1.TabIndex = 8;
            // 
            // frameWithShadow2
            // 
            this.frameWithShadow2.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow2.Location = new System.Drawing.Point(11, 219);
            this.frameWithShadow2.Name = "frameWithShadow2";
            this.frameWithShadow2.Size = new System.Drawing.Size(566, 120);
            this.frameWithShadow2.TabIndex = 9;
            // 
            // frmForReBilling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 380);
            this.ControlBox = false;
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.btnBill);
            this.Controls.Add(this.txtMemoranda);
            this.Controls.Add(this.dgvForReBilling);
            this.Controls.Add(this.frameWithShadow1);
            this.Controls.Add(this.frameWithShadow2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmForReBilling";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "For Re-Billing";
            this.Load += new System.EventHandler(this.frmForReBilling_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvForReBilling)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvForReBilling;
        private System.Windows.Forms.TextBox txtMemoranda;
        private System.Windows.Forms.Button btnBill;
        private System.Windows.Forms.Button btnSkip;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow1;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow2;
    }
}