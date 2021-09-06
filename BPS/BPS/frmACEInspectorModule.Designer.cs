namespace BPLSBilling
{
    partial class frmACEInspectorModule
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmACEInspectorModule));
            this.btnDefBns = new System.Windows.Forms.Button();
            this.btnUnofBns = new System.Windows.Forms.Button();
            this.btnInspector = new System.Windows.Forms.Button();
            this.btnViolation = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDefBns
            // 
            this.btnDefBns.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.btnDefBns.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnDefBns.FlatAppearance.BorderSize = 0;
            this.btnDefBns.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDefBns.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDefBns.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.btnDefBns.Image = global::BPLSBilling.Properties.Resources.deficient_businesses;
            this.btnDefBns.Location = new System.Drawing.Point(6, 19);
            this.btnDefBns.Name = "btnDefBns";
            this.btnDefBns.Size = new System.Drawing.Size(88, 69);
            this.btnDefBns.TabIndex = 14;
            this.btnDefBns.UseVisualStyleBackColor = false;
            this.btnDefBns.Click += new System.EventHandler(this.btnDefBns_Click);
            // 
            // btnUnofBns
            // 
            this.btnUnofBns.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.btnUnofBns.FlatAppearance.BorderSize = 0;
            this.btnUnofBns.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnofBns.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUnofBns.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.btnUnofBns.Image = global::BPLSBilling.Properties.Resources.unofficial_businesses;
            this.btnUnofBns.Location = new System.Drawing.Point(94, 19);
            this.btnUnofBns.Name = "btnUnofBns";
            this.btnUnofBns.Size = new System.Drawing.Size(88, 69);
            this.btnUnofBns.TabIndex = 15;
            this.btnUnofBns.UseVisualStyleBackColor = false;
            this.btnUnofBns.Click += new System.EventHandler(this.btnUnofBns_Click);
            // 
            // btnInspector
            // 
            this.btnInspector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.btnInspector.FlatAppearance.BorderSize = 0;
            this.btnInspector.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInspector.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInspector.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.btnInspector.Image = global::BPLSBilling.Properties.Resources.ace_inspector;
            this.btnInspector.Location = new System.Drawing.Point(182, 19);
            this.btnInspector.Name = "btnInspector";
            this.btnInspector.Size = new System.Drawing.Size(88, 69);
            this.btnInspector.TabIndex = 16;
            this.btnInspector.UseVisualStyleBackColor = false;
            this.btnInspector.Click += new System.EventHandler(this.btnInspector_Click);
            // 
            // btnViolation
            // 
            this.btnViolation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.btnViolation.FlatAppearance.BorderSize = 0;
            this.btnViolation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViolation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViolation.Image = global::BPLSBilling.Properties.Resources.violation_table_ace;
            this.btnViolation.Location = new System.Drawing.Point(269, 19);
            this.btnViolation.Name = "btnViolation";
            this.btnViolation.Size = new System.Drawing.Size(88, 69);
            this.btnViolation.TabIndex = 17;
            this.btnViolation.UseVisualStyleBackColor = false;
            this.btnViolation.Click += new System.EventHandler(this.btnViolation_Click);
            // 
            // frmACEInspectorModule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(364, 106);
            this.Controls.Add(this.btnViolation);
            this.Controls.Add(this.btnDefBns);
            this.Controls.Add(this.btnInspector);
            this.Controls.Add(this.btnUnofBns);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmACEInspectorModule";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inspector Module";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDefBns;
        private System.Windows.Forms.Button btnInspector;
        private System.Windows.Forms.Button btnUnofBns;
        private System.Windows.Forms.Button btnViolation;
    }
}