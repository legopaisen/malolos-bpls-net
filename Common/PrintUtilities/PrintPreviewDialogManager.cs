using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Amellar.Common.PrintUtilities
{
    public class PrintPreviewDialogManager
    {

        public static void PreviewDocument(VSPrinterEmuModel model, bool blnIsLandscape,
            int intTopMargin, int intLeftMargin, int intBottomMargin, int intRightMargin,
            string strPaperSizeName, int intPaperSizeWidth, int intPaperSizeHeight,
            PrintPageEventHandler ppeh, int intClientSizeWidth, int intClientSizeHeight, bool blnIsCustomDialog)
        {
            PrintPreviewDialogManager.PreviewDocument(model, string.Empty, false, false,
                false, null, blnIsLandscape, intTopMargin, intLeftMargin, intBottomMargin, intRightMargin,
                strPaperSizeName, intPaperSizeWidth, intPaperSizeHeight, ppeh, true, intClientSizeWidth,
                intClientSizeHeight, false, blnIsCustomDialog);
        }
        public static void PreviewDocument(VSPrinterEmuModel model, bool blnIsLandscape,
            int intTopMargin, int intLeftMargin, int intBottomMargin, int intRightMargin,
            string strPaperSizeName, int intPaperSizeWidth, int intPaperSizeHeight,
            PrintPageEventHandler ppeh, int intClientSizeWidth, int intClientSizeHeight, bool blnIsMaximized, bool blnIsCustomDialog)
        {
            PrintPreviewDialogManager.PreviewDocument(model, string.Empty, false, false,
                false, null, blnIsLandscape, intTopMargin, intLeftMargin, intBottomMargin, intRightMargin,
                strPaperSizeName, intPaperSizeWidth, intPaperSizeHeight, ppeh, true, intClientSizeWidth,
                intClientSizeHeight, blnIsMaximized, blnIsCustomDialog);
        }

        
        public static void PreviewDocument(VSPrinterEmuModel model, string strImagePath, 
            bool blnIsImageExtended, bool blnIsAllowNullCharacter, bool blnIsForceOnePage,
            VSPrinterEmuModel pageHeaderModel, bool blnIsLandscape, int intTopMargin, int intLeftMargin, int intBottomMargin,  int intRightMargin,
            string strPaperSizeName, int intPaperSizeWidth, int intPaperSizeHeight,
            PrintPageEventHandler ppeh, bool blnIsShowDialog, int intClientSizeWidth, int intClientSizeHeight, 
            bool blnIsMaximized, bool blnIsCustomDialog)
        {
            VSPrinterEmuDocument doc = new VSPrinterEmuDocument();
            doc.Model = model;
            doc.AllowNullCharacter = blnIsAllowNullCharacter;
            if (strImagePath != string.Empty)
                doc.SetImagePath(strImagePath, blnIsImageExtended);
            if (pageHeaderModel != null)
                doc.PageHeaderModel = pageHeaderModel;
            doc.Model.Reset();

            doc.DefaultPageSettings.Landscape = blnIsLandscape;
            doc.DefaultPageSettings.Margins.Top = intTopMargin;
            doc.DefaultPageSettings.Margins.Left = intLeftMargin;
            doc.DefaultPageSettings.Margins.Bottom = intBottomMargin;
            doc.DefaultPageSettings.Margins.Right = intRightMargin;
            if (intPaperSizeWidth != 0 && intPaperSizeHeight != 0)
                doc.DefaultPageSettings.PaperSize = new PaperSize(strPaperSizeName,
                    intPaperSizeWidth, intPaperSizeHeight);
            doc.PrintPage += ppeh;

            if (!blnIsShowDialog)
            {
                doc.Print();
            }
            else
            {
                /*
                PrintDialog pdlg = new PrintDialog();
                pdlg.Document = doc;
                if (pdlg.ShowDialog() == DialogResult.OK)
                {
                    PageSetupDialog psd = new PageSetupDialog();
                    psd.EnableMetric = true; //Ensure all dialog measurements are in metric
                    psd.Document = pdlg.Document;
                    if (psd.ShowDialog() == DialogResult.OK)
                    {
                        doc.DefaultPageSettings = psd.PageSettings;
                */
                if (blnIsCustomDialog)
                {
                    frmMyPrintPreviewDialog dlgPreview = new frmMyPrintPreviewDialog();
                    dlgPreview.Document = doc;
                    dlgPreview.ClientSize = new System.Drawing.Size(intClientSizeWidth, intClientSizeHeight);
                    if (blnIsMaximized)
                        dlgPreview.WindowState = FormWindowState.Maximized;

                    dlgPreview.ShowDialog();
                }
                else
                {
                    PrintPreviewDialog dlgPreview = new PrintPreviewDialog();
                    dlgPreview.Document = doc;
                    dlgPreview.ClientSize = new System.Drawing.Size(intClientSizeWidth, intClientSizeHeight);
                    if (blnIsMaximized)
                        dlgPreview.WindowState = FormWindowState.Maximized;

                    dlgPreview.ShowDialog();
                }
                /*
                    }
                }
                 */
            }

            model.Dispose();
        }


    }
}
