using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using Amellar.Common.ExcelReport;
using Amellar.Common.AppSettings;
using GemBox.Spreadsheet;

namespace Amellar.Modules.DILGReport
{
    public class DILGReportPreview:ReportPreview
    {

        public override bool CreateWorksheet(ExcelWorksheet worksheet, DateTime dtFrom, DateTime dtTo, string strYear, bool blnIncludeSurch, bool blnIncludeSplBns)//MCR 20150331 added blnIncludeSplBns
        {
            SpreadsheetInfo.SetLicense("EPAR-RILW-QD3F-YF0X");
            ExcelFile template = new ExcelFile();
            try
            {
                //template.LoadXls(Directory.GetCurrentDirectory() + "\\ReportTemplate\\" + "DILGReportTemplate.xls");
                string strPath = Path.GetTempFileName();
                File.WriteAllBytes(strPath, global::Amellar.Modules.DILGReport.Properties.Resources.DILGReportTemplate);
                template.LoadXls(File.OpenRead(strPath));
            }
            catch
            {
                return false;
            }

            DILGReportTemplate excelTemplate = new DILGReportTemplate(template.Worksheets[0], worksheet, dtFrom, dtTo, strYear, blnIncludeSurch, blnIncludeSplBns);//MCR 20150331 added blnIncludeSplBns


            return true;
        }

        public override void PreviewReport(DateTime dtFrom, DateTime dtTo, string strYear, bool blnIncludeSurch, bool blnIncludeSplBns)//MCR 20150331 added blnIncludeSplBns
        {
            SpreadsheetInfo.SetLicense("EPAR-RILW-QD3F-YF0X");
            ExcelFile newFile = new ExcelFile();
            newFile.Worksheets.Add("DILG");

            if (this.CreateWorksheet(newFile.Worksheets[0], dtFrom, dtTo, strYear, blnIncludeSurch, blnIncludeSplBns))
            {
                string strFileName = "LGU Quarterly Progress Report" + string.Format("-{0:MMMddyyyy}-{1:MMMddyyyy} ", dtFrom, dtTo) + string.Format("({0:MMM dd, yyyy})", AppSettingsManager.GetSystemDate()) + ".xls";
                CheckExcelFileName(strFileName);

                string strPath = Directory.GetCurrentDirectory() + "\\ReportFile\\";
                if (!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);

                newFile.SaveXls(Directory.GetCurrentDirectory() + "\\ReportFile\\" + strFileName);

                OpenSaveFile(strFileName, newFile, out strFileName);

                try
                {
                    Process.Start(strFileName);
                }
                catch (Exception) { }
            }
        }
    }
}
