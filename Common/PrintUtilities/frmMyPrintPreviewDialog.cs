using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;

namespace Amellar.Common.PrintUtilities
{
    public partial class frmMyPrintPreviewDialog : Form
    {
        /// <summary>
        /// by pass the printer settings default is false
        /// </summary>
        public bool IsDirectPrint
        {
            get { return m_isDirectPrint; }
            set { m_isDirectPrint = value; }
        }
        private bool m_isDirectPrint;



        private int m_intPageCount;

        public frmMyPrintPreviewDialog()
        {
            InitializeComponent();
            m_intPageCount = 0;


            //JVL events (s)
            m_isDirectPrint = false;

            clickRL = new MouseButtons();

            ppcPreview.Click += new EventHandler(ppcPreview_Click);
            ppcPreview.MouseDown += new MouseEventHandler(ppcPreview_MouseDown);
            
            //JVL envents (e)
            
        }

        private void cboZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            double dblZoom = 1.0;
            double[] dblZoomRange = { 4.0, 3.0, 2.0, 1.5, 1.0, 0.75, 0.5, 0.25, 1.0, 1.0 };
            if (cboZoom.SelectedIndex != -1)
                dblZoom = dblZoomRange[cboZoom.SelectedIndex];
            ppcPreview.Zoom = dblZoom;
        }

        private void UpdateToolBar()
        {
            if (m_intPageCount == 1)
            {
                btnFirstPage.Enabled = false;
                btnPrevPage.Enabled = false;
                btnNextPage.Enabled = false;
                btnLastPage.Enabled = false;
            }
            else if (ppcPreview.StartPage == 0)
            {
                btnFirstPage.Enabled = false;
                btnPrevPage.Enabled = false;
                btnNextPage.Enabled = true;
                btnLastPage.Enabled = true;
            }
            else if (ppcPreview.StartPage == m_intPageCount - 1)
            {
                btnFirstPage.Enabled = true;
                btnPrevPage.Enabled = true;
                btnNextPage.Enabled = false;
                btnLastPage.Enabled = false;
            }
            else
            {
                btnFirstPage.Enabled = true;
                btnPrevPage.Enabled = true;
                btnNextPage.Enabled = true;
                btnLastPage.Enabled = true;
            }

            lblPage.Text = string.Format("{0:#,##0} of {1:#,##0}", ppcPreview.StartPage + 1, m_intPageCount);
        }

        public VSPrinterEmuDocument Document
        {
            set 
            { 
                VSPrinterEmuDocument doc = (VSPrinterEmuDocument)value;
                ppcPreview.Document = value;
                m_intPageCount = doc.Model.PageCount;
                ppcPreview.Rows = 1; // doc.Model.PageCount;
                this.UpdateToolBar();
            }
            //get { return ppcPreview.Document; }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            VSPrinterEmuDocument doc = (VSPrinterEmuDocument)ppcPreview.Document;
            //show filedialog
            //string strDir = Directory.GetDirectories("./menus.xml");

            string strCurrentDirectory = Directory.GetCurrentDirectory();
            using (SaveFileDialog dlg = new SaveFileDialog()) //fix some issue on stream saving //JVL 09122008mal
            {
                //dlg.Title = "Export to HTML";
                //dlg.Filter = "HTML documents (*.html)|*.html";
                /*dlg.Title = "Export to EXCEL";
                dlg.Filter = "EXCEL documents (*.xls)|*.xls";*/

                dlg.DefaultExt = "html";
                dlg.Title = "Reports";
                dlg.Filter = "HTML Files (*.html;*.htm)|*.html|RTF Files (*.rtf)|*.rtf|Excel Files (*.xls)|*.xls";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(fileStream))
                        {
                            //streamWriter.WriteLine(doc.Model.ToHtml);
                            streamWriter.WriteLine(doc.Model.ToString());
                        }
                    }
                }
            }
            Directory.SetCurrentDirectory(strCurrentDirectory); //JVL fix the resetting of xml menus // JVL 09122008mal
            doc.Dispose();
        }

        private void frmMyPrintPreviewDialog_Load(object sender, EventArgs e)
        {
            cboZoom.SelectedIndex = 4;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDialog pdlg = new PrintDialog();
            pdlg.AllowSomePages = true;
            pdlg.Document = ppcPreview.Document;
            pdlg.PrinterSettings.FromPage = 1;
            pdlg.PrinterSettings.ToPage = m_intPageCount;
            pdlg.PrinterSettings.MinimumPage = 1;
            pdlg.PrinterSettings.MaximumPage = m_intPageCount;


            if (IsDirectPrint) //this condition is requested only by malolos JVL
            {
                PageSetupDialog psd = new PageSetupDialog();
                psd.EnableMetric = true; //Ensure all dialog measurements are in metric
                psd.Document = pdlg.Document;
                psd.Document.PrinterSettings.FromPage = pdlg.PrinterSettings.FromPage;
                psd.Document.PrinterSettings.ToPage = pdlg.PrinterSettings.ToPage;
                psd.Document.Print();
            }
            else if (pdlg.ShowDialog() == DialogResult.OK)
            {

                PageSetupDialog psd = new PageSetupDialog();
                psd.EnableMetric = true; //Ensure all dialog measurements are in metric
                psd.Document = pdlg.Document;
                if (psd.ShowDialog() == DialogResult.OK)
                {
                    //print
                    psd.Document.PrinterSettings.FromPage = pdlg.PrinterSettings.FromPage;
                    psd.Document.PrinterSettings.ToPage = pdlg.PrinterSettings.ToPage;
                    psd.Document.Print();
                }
            }

        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            ppcPreview.StartPage = 0;
            this.UpdateToolBar();
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (ppcPreview.StartPage != 0)
            {
                ppcPreview.StartPage--;
                this.UpdateToolBar();
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (ppcPreview.StartPage != m_intPageCount-1)
            {
                ppcPreview.StartPage++;
                this.UpdateToolBar();
            }

        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            ppcPreview.StartPage = m_intPageCount-1;
            this.UpdateToolBar();
        }


// JVL events (s)
        private int x, y;
        private MouseButtons clickRL;

        public void ppcPreview_Click(object obj, EventArgs e)
        {
            if (clickRL == MouseButtons.Right)
            {
                if(cboZoom.SelectedIndex < cboZoom.Items.Count - 1)
                    cboZoom.SelectedIndex = cboZoom.SelectedIndex + 1;

            }
            else if (clickRL == MouseButtons.Left)
            {
                if (cboZoom.SelectedIndex > 0)
                    cboZoom.SelectedIndex = cboZoom.SelectedIndex - 1;
            }
                
        }

        public void ppcPreview_MouseDown(object obj, MouseEventArgs e)
        {
            clickRL = e.Button;
            x = e.X;
            y = e.Y;

            
        }
//JVL events (e)

    }
}