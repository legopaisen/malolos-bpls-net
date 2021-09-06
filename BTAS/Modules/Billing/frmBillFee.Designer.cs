namespace Amellar.BPLS.Billing
{
    partial class frmBillFee
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtTotalDue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEnterQty = new System.Windows.Forms.TextBox();
            this.labelInput = new System.Windows.Forms.Label();
            this.txtEnterAmt = new System.Windows.Forms.TextBox();
            this.btnCompute = new System.Windows.Forms.Button();
            this.dgvBillFee = new System.Windows.Forms.DataGridView();
            this.frameWithShadow1 = new Amellar.Common.SOA.FrameWithShadow();
            this.lblamount = new System.Windows.Forms.Label();
            this.txtamout = new System.Windows.Forms.TextBox();
            this.btnAmount = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBillFee)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTotalDue
            // 
            this.txtTotalDue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalDue.Location = new System.Drawing.Point(444, 209);
            this.txtTotalDue.Name = "txtTotalDue";
            this.txtTotalDue.ReadOnly = true;
            this.txtTotalDue.Size = new System.Drawing.Size(100, 20);
            this.txtTotalDue.TabIndex = 2;
            this.txtTotalDue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotalDue.TextChanged += new System.EventHandler(this.txtTotalDue_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(384, 212);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Total Due";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(394, 237);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(475, 237);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label2.Location = new System.Drawing.Point(12, 232);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(538, 2);
            this.label2.TabIndex = 7;
            this.label2.Text = "               ";
            // 
            // txtEnterQty
            // 
            this.txtEnterQty.Location = new System.Drawing.Point(93, 209);
            this.txtEnterQty.Name = "txtEnterQty";
            this.txtEnterQty.Size = new System.Drawing.Size(100, 20);
            this.txtEnterQty.TabIndex = 8;
            this.txtEnterQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtEnterQty.Visible = false;
            this.txtEnterQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEnterQty_KeyPress);
            // 
            // labelInput
            // 
            this.labelInput.AutoSize = true;
            this.labelInput.Location = new System.Drawing.Point(15, 212);
            this.labelInput.Name = "labelInput";
            this.labelInput.Size = new System.Drawing.Size(74, 13);
            this.labelInput.TabIndex = 9;
            this.labelInput.Text = "Enter Quantity";
            this.labelInput.Visible = false;
            // 
            // txtEnterAmt
            // 
            this.txtEnterAmt.Location = new System.Drawing.Point(93, 209);
            this.txtEnterAmt.Name = "txtEnterAmt";
            this.txtEnterAmt.Size = new System.Drawing.Size(100, 20);
            this.txtEnterAmt.TabIndex = 10;
            this.txtEnterAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtEnterAmt.Visible = false;
            this.txtEnterAmt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEnterAmt_KeyPress);
            // 
            // btnCompute
            // 
            this.btnCompute.Location = new System.Drawing.Point(199, 206);
            this.btnCompute.Name = "btnCompute";
            this.btnCompute.Size = new System.Drawing.Size(75, 23);
            this.btnCompute.TabIndex = 11;
            this.btnCompute.Text = "C&ompute";
            this.btnCompute.UseVisualStyleBackColor = true;
            this.btnCompute.Visible = false;
            this.btnCompute.Click += new System.EventHandler(this.btnCompute_Click);
            // 
            // dgvBillFee
            // 
            this.dgvBillFee.AllowUserToAddRows = false;
            this.dgvBillFee.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBillFee.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBillFee.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBillFee.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvBillFee.Location = new System.Drawing.Point(18, 21);
            this.dgvBillFee.Name = "dgvBillFee";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBillFee.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvBillFee.RowHeadersVisible = false;
            this.dgvBillFee.Size = new System.Drawing.Size(526, 166);
            this.dgvBillFee.TabIndex = 0;
            this.dgvBillFee.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBillFee_CellContentDoubleClick);
            this.dgvBillFee.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBillFee_CellContentClick);
            // 
            // frameWithShadow1
            // 
            this.frameWithShadow1.BackColor = System.Drawing.Color.Transparent;
            this.frameWithShadow1.Location = new System.Drawing.Point(5, 12);
            this.frameWithShadow1.Name = "frameWithShadow1";
            this.frameWithShadow1.Size = new System.Drawing.Size(551, 188);
            this.frameWithShadow1.TabIndex = 12;
            // 
            // lblamount
            // 
            this.lblamount.AutoSize = true;
            this.lblamount.Location = new System.Drawing.Point(13, 240);
            this.lblamount.Name = "lblamount";
            this.lblamount.Size = new System.Drawing.Size(71, 13);
            this.lblamount.TabIndex = 13;
            this.lblamount.Text = "Enter Amount";
            this.lblamount.Visible = false;
            // 
            // txtamout
            // 
            this.txtamout.Location = new System.Drawing.Point(93, 237);
            this.txtamout.Name = "txtamout";
            this.txtamout.Size = new System.Drawing.Size(100, 20);
            this.txtamout.TabIndex = 14;
            this.txtamout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtamout.Visible = false;
            // 
            // btnAmount
            // 
            this.btnAmount.Location = new System.Drawing.Point(199, 235);
            this.btnAmount.Name = "btnAmount";
            this.btnAmount.Size = new System.Drawing.Size(75, 23);
            this.btnAmount.TabIndex = 15;
            this.btnAmount.Text = "C&ompute";
            this.btnAmount.UseVisualStyleBackColor = true;
            this.btnAmount.Visible = false;
            this.btnAmount.Click += new System.EventHandler(this.btnAmount_Click);
            // 
            // frmBillFee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(562, 269);
            this.Controls.Add(this.btnAmount);
            this.Controls.Add(this.txtamout);
            this.Controls.Add(this.lblamount);
            this.Controls.Add(this.dgvBillFee);
            this.Controls.Add(this.btnCompute);
            this.Controls.Add(this.txtEnterAmt);
            this.Controls.Add(this.labelInput);
            this.Controls.Add(this.txtEnterQty);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTotalDue);
            this.Controls.Add(this.frameWithShadow1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBillFee";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmBillFee_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmBillFee_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBillFee)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTotalDue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEnterQty;
        private System.Windows.Forms.Label labelInput;
        private System.Windows.Forms.TextBox txtEnterAmt;
        private System.Windows.Forms.Button btnCompute;
        private System.Windows.Forms.DataGridView dgvBillFee;
        private Amellar.Common.SOA.FrameWithShadow frameWithShadow1;
        private System.Windows.Forms.Label lblamount;
        private System.Windows.Forms.TextBox txtamout;
        private System.Windows.Forms.Button btnAmount;
    }
}