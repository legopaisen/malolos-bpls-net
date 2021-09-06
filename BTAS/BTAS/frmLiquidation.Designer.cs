namespace BTAS
{
    partial class frmLiquidation
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
            this.kryptonHeaderGroup1 = new ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup();
            this.btnAbsCollectBrgy = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnLORCD = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAbsCMemo = new ComponentFactory.Krypton.Toolkit.KryptonDropButton();
            this.kryptonContextMenu1 = new ComponentFactory.Krypton.Toolkit.KryptonContextMenu();
            this.kryptonContextMenuItems1 = new ComponentFactory.Krypton.Toolkit.KryptonContextMenuItems();
            this.kryptonContextMenuItem1 = new ComponentFactory.Krypton.Toolkit.KryptonContextMenuItem();
            this.kryptonContextMenuItem2 = new ComponentFactory.Krypton.Toolkit.KryptonContextMenuItem();
            this.btnDCreport = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAbsDMemo = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnLstCancChk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAbsCancOR = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnTellerRecCollect = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAbsPstOR = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnTellerTrans = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAbsCollect = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnReprintRCD = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAbsChk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnRCD = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).BeginInit();
            this.kryptonHeaderGroup1.Panel.SuspendLayout();
            this.kryptonHeaderGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonHeaderGroup1
            // 
            this.kryptonHeaderGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonHeaderGroup1.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeaderGroup1.Name = "kryptonHeaderGroup1";
            // 
            // kryptonHeaderGroup1.Panel
            // 
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnAbsCollectBrgy);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnLORCD);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnAbsCMemo);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnDCreport);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnAbsDMemo);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnLstCancChk);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnAbsCancOR);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnTellerRecCollect);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnAbsPstOR);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnTellerTrans);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnAbsCollect);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnReprintRCD);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnAbsChk);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.btnRCD);
            this.kryptonHeaderGroup1.Panel.Paint += new System.Windows.Forms.PaintEventHandler(this.kryptonHeaderGroup1_Panel_Paint);
            this.kryptonHeaderGroup1.Size = new System.Drawing.Size(468, 378);
            this.kryptonHeaderGroup1.TabIndex = 0;
            this.kryptonHeaderGroup1.ValuesPrimary.Heading = "Liquidation";
            this.kryptonHeaderGroup1.ValuesPrimary.Image = global::BTAS.Properties.Resources.liquidation_reports;
            this.kryptonHeaderGroup1.ValuesSecondary.Heading = "";
            // 
            // btnAbsCollectBrgy
            // 
            this.btnAbsCollectBrgy.Location = new System.Drawing.Point(236, 99);
            this.btnAbsCollectBrgy.Name = "btnAbsCollectBrgy";
            this.btnAbsCollectBrgy.Size = new System.Drawing.Size(227, 42);
            this.btnAbsCollectBrgy.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnAbsCollectBrgy.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnAbsCollectBrgy.TabIndex = 14;
            this.btnAbsCollectBrgy.Values.Image = global::BTAS.Properties.Resources.abstract_collection;
            this.btnAbsCollectBrgy.Values.Text = "Abstract of Collections -\r\nBarangay Fees";
            this.btnAbsCollectBrgy.Click += new System.EventHandler(this.btnAbsCollectBrgy_Click);
            // 
            // btnLORCD
            // 
            this.btnLORCD.Location = new System.Drawing.Point(3, 291);
            this.btnLORCD.Name = "btnLORCD";
            this.btnLORCD.Size = new System.Drawing.Size(227, 42);
            this.btnLORCD.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnLORCD.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnLORCD.TabIndex = 13;
            this.btnLORCD.Values.Image = global::BTAS.Properties.Resources.report_files;
            this.btnLORCD.Values.Text = "Liquidating Officer RCD";
            this.btnLORCD.Click += new System.EventHandler(this.btnLORCD_Click);
            // 
            // btnAbsCMemo
            // 
            this.btnAbsCMemo.KryptonContextMenu = this.kryptonContextMenu1;
            this.btnAbsCMemo.Location = new System.Drawing.Point(236, 291);
            this.btnAbsCMemo.Name = "btnAbsCMemo";
            this.btnAbsCMemo.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnAbsCMemo.Size = new System.Drawing.Size(227, 42);
            this.btnAbsCMemo.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.btnAbsCMemo.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnAbsCMemo.TabIndex = 12;
            this.btnAbsCMemo.Values.Image = global::BTAS.Properties.Resources.abstract_CreditMemo;
            this.btnAbsCMemo.Values.Text = "Abstract of Credit Memos";
            this.btnAbsCMemo.Click += new System.EventHandler(this.btnAbsCMemo_Click);
            // 
            // kryptonContextMenu1
            // 
            this.kryptonContextMenu1.Items.AddRange(new ComponentFactory.Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuItems1});
            this.kryptonContextMenu1.Tag = null;
            // 
            // kryptonContextMenuItems1
            // 
            this.kryptonContextMenuItems1.Items.AddRange(new ComponentFactory.Krypton.Toolkit.KryptonContextMenuItemBase[] {
            this.kryptonContextMenuItem1,
            this.kryptonContextMenuItem2});
            // 
            // kryptonContextMenuItem1
            // 
            this.kryptonContextMenuItem1.Image = global::BTAS.Properties.Resources.abstract_CreditMemos_ExcessCheck;
            this.kryptonContextMenuItem1.Text = "Excess from Tax Credit";
            this.kryptonContextMenuItem1.Click += new System.EventHandler(this.kryptonContextMenuItem1_Click);
            // 
            // kryptonContextMenuItem2
            // 
            this.kryptonContextMenuItem2.Image = global::BTAS.Properties.Resources.abstract_CreditMemos_ExcessTax;
            this.kryptonContextMenuItem2.Text = "Excess Tax Credit";
            this.kryptonContextMenuItem2.Click += new System.EventHandler(this.kryptonContextMenuItem2_Click);
            // 
            // btnDCreport
            // 
            this.btnDCreport.Location = new System.Drawing.Point(3, 243);
            this.btnDCreport.Name = "btnDCreport";
            this.btnDCreport.Size = new System.Drawing.Size(227, 42);
            this.btnDCreport.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnDCreport.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnDCreport.TabIndex = 6;
            this.btnDCreport.Values.Image = global::BTAS.Properties.Resources.debitcredit_report;
            this.btnDCreport.Values.Text = "Debit Credit Reports";
            this.btnDCreport.Click += new System.EventHandler(this.btnDCreport_Click);
            // 
            // btnAbsDMemo
            // 
            this.btnAbsDMemo.Location = new System.Drawing.Point(236, 243);
            this.btnAbsDMemo.Name = "btnAbsDMemo";
            this.btnAbsDMemo.Size = new System.Drawing.Size(227, 42);
            this.btnAbsDMemo.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnAbsDMemo.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnAbsDMemo.TabIndex = 11;
            this.btnAbsDMemo.Values.Image = global::BTAS.Properties.Resources.abstract_debitMemo;
            this.btnAbsDMemo.Values.Text = "Abstract of Debit Memos";
            this.btnAbsDMemo.Click += new System.EventHandler(this.btnAbsDMemo_Click);
            // 
            // btnLstCancChk
            // 
            this.btnLstCancChk.Location = new System.Drawing.Point(3, 195);
            this.btnLstCancChk.Name = "btnLstCancChk";
            this.btnLstCancChk.Size = new System.Drawing.Size(227, 42);
            this.btnLstCancChk.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnLstCancChk.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnLstCancChk.TabIndex = 5;
            this.btnLstCancChk.Values.Image = global::BTAS.Properties.Resources.list_cancelled_checks;
            this.btnLstCancChk.Values.Text = "List of Cancelled Check";
            this.btnLstCancChk.Click += new System.EventHandler(this.btnLstCancChk_Click);
            // 
            // btnAbsCancOR
            // 
            this.btnAbsCancOR.Location = new System.Drawing.Point(236, 195);
            this.btnAbsCancOR.Name = "btnAbsCancOR";
            this.btnAbsCancOR.Size = new System.Drawing.Size(227, 42);
            this.btnAbsCancOR.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnAbsCancOR.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnAbsCancOR.TabIndex = 10;
            this.btnAbsCancOR.Values.Image = global::BTAS.Properties.Resources.abstract_cencelledor;
            this.btnAbsCancOR.Values.Text = "Abstract of Cancelled O.R.";
            this.btnAbsCancOR.Click += new System.EventHandler(this.btnAbsCancOR_Click);
            // 
            // btnTellerRecCollect
            // 
            this.btnTellerRecCollect.Location = new System.Drawing.Point(3, 147);
            this.btnTellerRecCollect.Name = "btnTellerRecCollect";
            this.btnTellerRecCollect.Size = new System.Drawing.Size(227, 42);
            this.btnTellerRecCollect.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnTellerRecCollect.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnTellerRecCollect.TabIndex = 4;
            this.btnTellerRecCollect.Values.Image = global::BTAS.Properties.Resources.teller_reportcollection;
            this.btnTellerRecCollect.Values.Text = "Teller\'s Report of Collections";
            this.btnTellerRecCollect.Click += new System.EventHandler(this.btnTellerRecCollect_Click);
            // 
            // btnAbsPstOR
            // 
            this.btnAbsPstOR.Location = new System.Drawing.Point(236, 147);
            this.btnAbsPstOR.Name = "btnAbsPstOR";
            this.btnAbsPstOR.Size = new System.Drawing.Size(227, 42);
            this.btnAbsPstOR.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnAbsPstOR.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnAbsPstOR.TabIndex = 9;
            this.btnAbsPstOR.Values.Image = global::BTAS.Properties.Resources.abstract_postedor;
            this.btnAbsPstOR.Values.Text = "Abstract of Posted O.R.";
            this.btnAbsPstOR.Click += new System.EventHandler(this.btnAbsPstOR_Click);
            // 
            // btnTellerTrans
            // 
            this.btnTellerTrans.Location = new System.Drawing.Point(3, 99);
            this.btnTellerTrans.Name = "btnTellerTrans";
            this.btnTellerTrans.Size = new System.Drawing.Size(227, 42);
            this.btnTellerTrans.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnTellerTrans.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnTellerTrans.TabIndex = 3;
            this.btnTellerTrans.Values.Image = global::BTAS.Properties.Resources.teller_transaction;
            this.btnTellerTrans.Values.Text = "Teller Transaction";
            this.btnTellerTrans.Click += new System.EventHandler(this.btnTellerTrans_Click);
            // 
            // btnAbsCollect
            // 
            this.btnAbsCollect.Location = new System.Drawing.Point(236, 51);
            this.btnAbsCollect.Name = "btnAbsCollect";
            this.btnAbsCollect.Size = new System.Drawing.Size(227, 42);
            this.btnAbsCollect.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnAbsCollect.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnAbsCollect.TabIndex = 8;
            this.btnAbsCollect.Values.Image = global::BTAS.Properties.Resources.abstract_collection;
            this.btnAbsCollect.Values.Text = "Abstract of Collections";
            this.btnAbsCollect.Click += new System.EventHandler(this.btnAbsCollect_Click);
            // 
            // btnReprintRCD
            // 
            this.btnReprintRCD.Location = new System.Drawing.Point(3, 51);
            this.btnReprintRCD.Name = "btnReprintRCD";
            this.btnReprintRCD.Size = new System.Drawing.Size(227, 42);
            this.btnReprintRCD.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnReprintRCD.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnReprintRCD.TabIndex = 2;
            this.btnReprintRCD.Values.Image = global::BTAS.Properties.Resources.RCD_reprint;
            this.btnReprintRCD.Values.Text = "Reprint RCD";
            this.btnReprintRCD.Click += new System.EventHandler(this.btnReprintRCD_Click);
            // 
            // btnAbsChk
            // 
            this.btnAbsChk.Location = new System.Drawing.Point(236, 3);
            this.btnAbsChk.Name = "btnAbsChk";
            this.btnAbsChk.Size = new System.Drawing.Size(227, 42);
            this.btnAbsChk.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnAbsChk.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnAbsChk.TabIndex = 7;
            this.btnAbsChk.Values.Image = global::BTAS.Properties.Resources.abstract_check;
            this.btnAbsChk.Values.Text = "Abstract of Checks";
            this.btnAbsChk.Click += new System.EventHandler(this.btnAbsChk_Click);
            // 
            // btnRCD
            // 
            this.btnRCD.Location = new System.Drawing.Point(3, 3);
            this.btnRCD.Name = "btnRCD";
            this.btnRCD.Size = new System.Drawing.Size(227, 42);
            this.btnRCD.StateCommon.Content.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.btnRCD.StateCommon.Content.Image.ImageV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.btnRCD.TabIndex = 1;
            this.btnRCD.Values.Image = global::BTAS.Properties.Resources.RCD;
            this.btnRCD.Values.Text = "RCD";
            this.btnRCD.Click += new System.EventHandler(this.btnRCD_Click);
            // 
            // frmLiquidation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 378);
            this.Controls.Add(this.kryptonHeaderGroup1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLiquidation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmLiquidation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).EndInit();
            this.kryptonHeaderGroup1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).EndInit();
            this.kryptonHeaderGroup1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroup1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnRCD;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnReprintRCD;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnDCreport;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnLstCancChk;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnTellerRecCollect;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnTellerTrans;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAbsDMemo;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAbsCancOR;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAbsPstOR;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAbsCollect;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAbsChk;
        private ComponentFactory.Krypton.Toolkit.KryptonDropButton btnAbsCMemo;
        private ComponentFactory.Krypton.Toolkit.KryptonContextMenu kryptonContextMenu1;
        private ComponentFactory.Krypton.Toolkit.KryptonContextMenuItems kryptonContextMenuItems1;
        private ComponentFactory.Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItem1;
        private ComponentFactory.Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItem2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnLORCD;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnAbsCollectBrgy;
    }
}