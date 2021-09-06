using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using GemBox.Spreadsheet;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using Amellar.Common.DataConnector;

namespace Amellar.Common.ExcelReport
{
    public abstract class ReportPreview
    {
        protected string m_strWorksheetName;
        protected Hashtable m_hshTotalReferences;        

        public ReportPreview()
        {
            m_strWorksheetName = string.Empty;
            m_hshTotalReferences = new Hashtable();            
        }

        public string WorksheetName
        {
            get { return m_strWorksheetName; }
        }                

        //public abstract bool CreateWorksheet(ExcelWorksheet worksheet, int intQuarter, int intYear);

        //public abstract void PreviewReport(int intQuarter, int intYear);

        public abstract bool CreateWorksheet(ExcelWorksheet worksheet, DateTime dtFrom, DateTime dtTo, string strYear, bool blnIncludeSurch, bool blnIncludeSplBns); //MCR 20150331 added blnIncludeSplBns

        public abstract void PreviewReport(DateTime dtFrom, DateTime dtTo, string strYear, bool blnIncludeSurch, bool blnIncludeSplBns); //MCR 20150331 added blnIncludeSplBns

        public void CheckExcelFileName(string strFileName)
        {
            Process[] processlist = Process.GetProcesses();

            foreach (Process theprocess in processlist)
            {
                string strTitle = theprocess.MainWindowTitle;
                
                if (strTitle.EndsWith(strFileName))
                {
                    try
                    {
                        theprocess.Kill();
                    }
                    catch { }
                }
            }            
        }

        public void OpenSaveFile(string strFileName, ExcelFile exFile, out string strNewFileName)
        {
            strNewFileName = strFileName;
            string strCurrentDirectory = Directory.GetCurrentDirectory();
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Save Report";
                dlg.Filter = "Microsoft Excel Workbook (*.xls)|*.xls";
                dlg.FileName = strFileName;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    exFile.SaveXls(dlg.FileName);
                    strNewFileName = dlg.FileName;
                    dlg.Dispose();
                }
                else
                {
                    strNewFileName = Directory.GetCurrentDirectory() + "\\ReportFile\\" + strFileName; // Temporary for aSREs
                }
            }
            Directory.SetCurrentDirectory(strCurrentDirectory);
        }        
    }
}
