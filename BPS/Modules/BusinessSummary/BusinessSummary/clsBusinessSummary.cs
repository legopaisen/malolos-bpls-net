using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.AuditTrail;
using System.Collections;
using System.Threading;
using Amellar.Common.DynamicProgressBar;


namespace BusinessSummary
{
    class clsBusinessSummary
    {
        public static void LoadByBarangay(frmReport frmReport, String strRegYear)
        {
            List<String> strDistrict = new List<string>();
            List<String> strBrgy = new List<string>();
            Boolean _BasedOnRegistration = false;

            if (strRegYear != "")
                _BasedOnRegistration = true;

            int iBnsCount = 0;
            double dCapital = 0.00;
            double dCapitalAddl = 0.00;

            double dGross = 0.00;
            double dGrossAddl = 0.00;
            double dPreGR = 0.00;

            int iBnsCountTotalNew = 0;
            int iBnsCountTotalRen = 0;
            int iBnsCountTotalRet = 0;

            double dTotalAmountNew = 0.00;
            double dTotalAmountRen = 0.00;
            double dTotalAmountRet = 0.00;

            int iGrandTotalBnsNew = 0;
            int iGrandTotalBnsRen = 0;
            int iGrandTotalBnsRet = 0;

            double dGrandTotalAmtNew = 0;
            double dGrandTotalAmtRen = 0;
            double dGrandTotalAmtRet = 0;

            frmReport.axVSPrinter1.MarginTop = 350;
            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;
            
            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618

            frmReport.axVSPrinter1.FontSize = 8.0f;
            strDistrict = DataAccess.DistrictList();



            for (int i = 0; i != strDistrict.Count; i++)
            {

                if (strDistrict[i] == "")
                {
                    strDistrict[i] = " ";
                    frmReport.axVSPrinter1.Paragraph = String.Empty;
                    frmReport.axVSPrinter1.FontBold = true;
                    frmReport.axVSPrinter1.Table = string.Format("<16000;{0}", "NO DISTRICT");
                    frmReport.axVSPrinter1.FontBold = false;
                }
                else
                {
                    frmReport.axVSPrinter1.Table = String.Empty;
                    frmReport.axVSPrinter1.FontBold = true;
                    frmReport.axVSPrinter1.Table = string.Format("<16000;{0}", strDistrict[i]);
                    frmReport.axVSPrinter1.FontBold = false;
                }

                strBrgy = DataAccess.BarangayList(strDistrict[i]);
                for (int j = 0; j != strBrgy.Count; j++)
                {

                    if (strBrgy[j] == "")
                        strBrgy[j] = " ";

                    object objData = "";
                    string strAmount = "";
                    double dTmpAmount = 0.00;

                    iBnsCount = DataAccess.BusinessCount(strBrgy[j], strDistrict[i], "NEW", "", "", strRegYear, _BasedOnRegistration);
                    dCapital = DataAccess.Capital(strBrgy[j], strDistrict[i], "NEW", "", "", "", strRegYear, _BasedOnRegistration);
                    dCapitalAddl = DataAccess.CapitalAddl(strBrgy[j], strDistrict[i], "NEW", "", "", strRegYear, _BasedOnRegistration);
                    iBnsCountTotalNew += iBnsCount;
                    dTmpAmount = dCapital + dCapitalAddl;
                    dTotalAmountNew += dTmpAmount;

                    strAmount = string.Format("{0:#,##0.00}", dTmpAmount);
                    objData += strBrgy[j] + "|" + iBnsCount.ToString("#,##0") + "|" + strAmount;

                    iBnsCount = DataAccess.BusinessCount(strBrgy[j], strDistrict[i], "REN", "", "", strRegYear, _BasedOnRegistration);
                    dGross = DataAccess.Gross(strBrgy[j], strDistrict[i], "REN", "", "", strRegYear, _BasedOnRegistration);
                    dGrossAddl = DataAccess.GrossAddl(strBrgy[j], strDistrict[i], "REN", "", "", "", strRegYear, _BasedOnRegistration);
                    dPreGR = DataAccess.PreGR(strBrgy[j], strDistrict[i], "REN", "", "", "", strRegYear, _BasedOnRegistration);
                    iBnsCountTotalRen += iBnsCount;
                    //dTmpAmount = (dGross + dGrossAddl) + dPreGR;
                    dTmpAmount = (dGross + dGrossAddl); // RMC 20171114 correction in Management report, do not include presumptive gr in total gr
                    dTotalAmountRen += dTmpAmount;

                    strAmount = string.Format("{0:#,##0.00}", dTmpAmount);
                    objData += "|" + iBnsCount.ToString("#,##0") + "|" + strAmount;

                    iBnsCount = DataAccess.BusinessCount(strBrgy[j], strDistrict[i], "RET", "", "", strRegYear, _BasedOnRegistration);
                    dGross = DataAccess.Gross(strBrgy[j], strDistrict[i], "RET", "", "", strRegYear, _BasedOnRegistration);
                    dGrossAddl = DataAccess.GrossAddl(strBrgy[j], strDistrict[i], "RET", "", "", "", strRegYear, _BasedOnRegistration);
                    dPreGR = DataAccess.PreGR(strBrgy[j], strDistrict[i], "RET", "", "", "", strRegYear, _BasedOnRegistration);
                    iBnsCountTotalRet += iBnsCount;
                    //dTmpAmount = (dGross + dGrossAddl) + dPreGR;
                    dTmpAmount = (dGross + dGrossAddl); // RMC 20171114 correction in Management report, do not include presumptive gr in total gr
                    dTotalAmountRet += dTmpAmount;

                    strAmount = string.Format("{0:#,##0.00}", dTmpAmount);
                    objData += "|" + iBnsCount.ToString("#,##0") + "|" + strAmount;                    

                    frmReport.axVSPrinter1.Table = String.Format("<3000|>1000|>2000|>2000|>3000|>2000|>3000;{0}", objData);

                }

                frmReport.axVSPrinter1.FontBold = true;
                frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                string strTotalAmtNew = string.Format("{0:#,##0.00}", dTotalAmountNew);
                string strTotalAmtRen = string.Format("{0:#,##0.00}", dTotalAmountRen);
                string strTotalAmtRet = string.Format("{0:#,##0.00}", dTotalAmountRet);
                frmReport.axVSPrinter1.Table = String.Format(">3000|>1000|>2000|>2000|>3000|>2000|>3000;{0}",
                    "TOTAL" + "|" + iBnsCountTotalNew.ToString("#,##0") + "|" + strTotalAmtNew + "|" +
                    iBnsCountTotalRen.ToString("#,##0") + "|" + strTotalAmtRen + "|" +
                    iBnsCountTotalRet.ToString("#,##0") + "|" + strTotalAmtRet);
                frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

                iGrandTotalBnsNew += iBnsCountTotalNew;
                iGrandTotalBnsRen += iBnsCountTotalRen;
                iGrandTotalBnsRet += iBnsCountTotalRet;

                dGrandTotalAmtNew += dTotalAmountNew;
                dGrandTotalAmtRen += dTotalAmountRen;
                dGrandTotalAmtRet += dTotalAmountRet;
            }

            frmReport.axVSPrinter1.Paragraph = "";
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            string strGrandTotalAmtNew = string.Format("{0:#,##0.00}", dGrandTotalAmtNew);
            string strGrandTotalAmtRen = string.Format("{0:#,##0.00}", dGrandTotalAmtRen);
            string strGrandTotalAmtRet = string.Format("{0:#,##0.00}", dGrandTotalAmtRet);
            frmReport.axVSPrinter1.Table = String.Format(">3000|>1000|>2000|>2000|>3000|>2000|>3000;{0}",
                "GRAND TOTAL" + "|" + iGrandTotalBnsNew.ToString("#,##0") + "|" + strGrandTotalAmtNew + "|" +
                iGrandTotalBnsRen.ToString("#,##0") + "|" + strGrandTotalAmtRen + "|" +
                iGrandTotalBnsRet.ToString("#,##0") + "|" + strGrandTotalAmtRet);
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            frmReport.axVSPrinter1.FontBold = false;

            int iTotalBnsCount = iGrandTotalBnsNew + iGrandTotalBnsRen + iGrandTotalBnsRet;
            frmReport.axVSPrinter1.Paragraph = "";
            frmReport.axVSPrinter1.Table = string.Format("<16000;{0}{1}", "Total No. of Businesses: ", iTotalBnsCount.ToString("#,##0"));

            frmReport.axVSPrinter1.EndDoc();
        }

        public static void LoadByDistrict(frmReport frmReport, String strRegYear)
        {
            List<String> strDistrict = new List<string>();
            Boolean _BasedOnRegistration = false;

            if (strRegYear != "")
                _BasedOnRegistration = true;

            int iBnsCount = 0;
            double dCapital = 0.00;
            double dCapitalAddl = 0.00;

            double dGross = 0.00;
            double dGrossAddl = 0.00;
            double dPreGR = 0.00;

            int iBnsCountTotalNew = 0;
            int iBnsCountTotalRen = 0;
            int iBnsCountTotalRet = 0;

            double dTotalAmountNew = 0.00;
            double dTotalAmountRen = 0.00;
            double dTotalAmountRet = 0.00;

            double dTotalPreGRRen = 0.00;
            double dTotalPreGRRet = 0.00;
            
            frmReport.axVSPrinter1.MarginTop = 350;
            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;

            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618

            frmReport.axVSPrinter1.FontSize = 8.0f;
            strDistrict = DataAccess.DistrictList();

            for (int i = 0; i != strDistrict.Count; i++)
            {
                object objData = "";
                string strAmount = "";
                double dTmpAmount = 0.00;
                string strPreGR = "";
                string strTmpDist = "";

                frmReport.axVSPrinter1.Paragraph = String.Empty;

                if (strDistrict[i] == "")
                {
                    strDistrict[i] = " ";
                    strTmpDist = "NO DISTRICT";
                }
                else
                    strTmpDist = strDistrict[i];


                iBnsCount = DataAccess.BusinessCount("", strDistrict[i], "NEW", "", "", strRegYear, _BasedOnRegistration);
                dCapital = DataAccess.Capital("", strDistrict[i], "NEW", "", "", "", strRegYear, _BasedOnRegistration);
                dCapitalAddl = DataAccess.CapitalAddl("", strDistrict[i], "NEW", "", "", strRegYear, _BasedOnRegistration);
                iBnsCountTotalNew += iBnsCount;
                dTmpAmount = dCapital + dCapitalAddl;
                dTotalAmountNew += dTmpAmount;

                strAmount = string.Format("{0:#,##0.00}", dTmpAmount);
                objData += strTmpDist + "|" + iBnsCount.ToString("#,##0") + "|" + strAmount;

                iBnsCount = DataAccess.BusinessCount("", strDistrict[i], "REN", "", "", strRegYear, _BasedOnRegistration);
                dGross = DataAccess.Gross("", strDistrict[i], "REN", "", "", strRegYear, _BasedOnRegistration);
                dGrossAddl = DataAccess.GrossAddl("", strDistrict[i], "REN", "", "", "", strRegYear, _BasedOnRegistration);
                dPreGR = DataAccess.PreGR("", strDistrict[i], "REN", "", "", "", strRegYear, _BasedOnRegistration);
                iBnsCountTotalRen += iBnsCount;
                dTmpAmount = (dGross + dGrossAddl);
                dTotalPreGRRen += dPreGR;
                dTotalAmountRen += dTmpAmount;

                strPreGR = string.Format("{0:#,##0.00}", dPreGR);
                strAmount = string.Format("{0:#,##0.00}", dTmpAmount);
                objData += "|" + iBnsCount.ToString("#,##0") + "|" + strAmount + "|" + strPreGR;

                iBnsCount = DataAccess.BusinessCount("", strDistrict[i], "RET", "", "", strRegYear, _BasedOnRegistration);
                dGross = DataAccess.Gross("", strDistrict[i], "RET", "", "", strRegYear, _BasedOnRegistration);
                dGrossAddl = DataAccess.GrossAddl("", strDistrict[i], "RET", "", "", "", strRegYear, _BasedOnRegistration);
                dPreGR = DataAccess.PreGR("", strDistrict[i], "RET", "", "", "", strRegYear, _BasedOnRegistration);
                iBnsCountTotalRet += iBnsCount;
                dTmpAmount = (dGross + dGrossAddl);
                dTotalPreGRRet += dPreGR;
                dTotalAmountRet += dTmpAmount;

                strPreGR = string.Format("{0:#,##0.00}", dPreGR);
                strAmount = string.Format("{0:#,##0.00}", dTmpAmount);
                objData += "|" + iBnsCount.ToString("#,##0") + "|" + strAmount + "|" + strPreGR;

                frmReport.axVSPrinter1.Table = String.Format("<2000|>1000|>2000|>1000|>2250|>2250|>1000|>2250|>2250;{0}", objData);
            }

            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            string strTotalAmtNew = string.Format("{0:#,##0.00}", dTotalAmountNew);
            string strTotalAmtRen = string.Format("{0:#,##0.00}", dTotalAmountRen);
            string strTotalAmtRet = string.Format("{0:#,##0.00}", dTotalAmountRet);
            frmReport.axVSPrinter1.Table = String.Format(">2000|>1000|>2000|>1000|>2250|>2250|>1000|>2250|>2250;{0}",
                "TOTAL" + "|" + iBnsCountTotalNew.ToString("#,##0") + "|" + strTotalAmtNew + "|" +
                iBnsCountTotalRen.ToString("#,##0") + "|" + strTotalAmtRen + "|" + dTotalPreGRRen + "|" +
                iBnsCountTotalRet.ToString("#,##0") + "|" + strTotalAmtRet + "|" + dTotalPreGRRet);

            frmReport.axVSPrinter1.EndDoc();
        }

        private static int HasSubCat(string BnsCode, int iCnt)
        {
            int iBns = 0;
            iBns = BnsCode.Length;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select count(*) as COUNT from bns_table where  fees_code = 'B' and bns_code like '"+ BnsCode +"%' and length(bns_code) > 4";
            int.TryParse(result.ExecuteScalar(), out iCnt);
            return iCnt;
        }

        public static void LoadByLineOfBusiness(frmReport frmReport, String strRegYear)
        {
            //AFM 20191204 added include subcategories option(s)
            bool includeSub = false;
            if (MessageBox.Show("Include subcategories?", "Report", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                includeSub = true;
            }
            //AFM 20191204 added include subcategories option(e)
            
            int iBnsCount = 0;
            double dCapital = 0.00;
            double dCapitalAddl = 0.00;

            double dGross = 0.00;
            double dGrossAddl = 0.00;
            double dPreGR = 0.00;

            int iBnsCountTotalNew = 0;
            int iBnsCountTotalRen = 0;
            int iBnsCountTotalRet = 0;
            int iBnsCountGrandTotal = 0;

            double dTotalAmountNew = 0.00;
            double dTotalAmountRen = 0.00;
            double dTotalAmountRet = 0.00;
            List<String> strBnsCode = new List<string>();
            List<String> strBnsDesc = new List<string>();

            Boolean _BasedOnRegistration = false;

            if (strRegYear != "")
                _BasedOnRegistration = true;

            frmReport.axVSPrinter1.MarginTop = 350;
            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;

            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618

            frmReport.axVSPrinter1.FontSize = 8.0f;

            frmReport.axVSPrinter1.Paragraph = String.Empty;

            DataAccess.BusinessTable("", "", out strBnsCode, out strBnsDesc, includeSub); //AFM 20191204 added include subcategories option
            for (int i = 0; i != strBnsCode.Count; i++)
            {
                if (strBnsCode[i].Length == 2)//AFM 20191206
                    frmReport.axVSPrinter1.FontBold = true;
                else
                    frmReport.axVSPrinter1.FontBold = false;

                object objData = "";
                string strAmount = "";
                double dTmpAmount = 0.00;

  
                iBnsCount = DataAccess.BusinessCount("", "", "NEW", strBnsCode[i], "", strRegYear, _BasedOnRegistration);
                dCapital = DataAccess.Capital("", "", "NEW", strBnsCode[i], "", "", strRegYear, _BasedOnRegistration);
                dCapitalAddl = DataAccess.CapitalAddl("", "", "NEW", strBnsCode[i], "", strRegYear, _BasedOnRegistration);
                dTmpAmount = dCapital + dCapitalAddl;
                if (strBnsCode[i].Length == 2)//AFM 20191205
                {
                    iBnsCountTotalNew += iBnsCount;
                    dTotalAmountNew += dTmpAmount;
                }

                strAmount = string.Format("{0:#,##0.00}", dTmpAmount);
                objData += strBnsDesc[i] + "|" + iBnsCount.ToString("#,##0") + "|" + strAmount + "|";

                iBnsCount = DataAccess.BusinessCount("", "", "REN", strBnsCode[i], "", strRegYear, _BasedOnRegistration);
                dGross = DataAccess.Gross("", "", "REN", strBnsCode[i], "", strRegYear, _BasedOnRegistration);
                dGrossAddl = DataAccess.GrossAddl("", "", "REN", strBnsCode[i], "", "", strRegYear, _BasedOnRegistration);
                dPreGR = DataAccess.PreGR("", "", "REN", strBnsCode[i], "", "", strRegYear, _BasedOnRegistration);
                //dTmpAmount = dGross + dGrossAddl + dPreGR;
                dTmpAmount = dGross + dGrossAddl;   // RMC 20171114 correction in Management report, do not include presumptive gr in total gr   
                if (strBnsCode[i].Length == 2)//AFM 20191205
                {
                    iBnsCountTotalRen += iBnsCount;
                    dTotalAmountRen += dTmpAmount;
                }

                strAmount = string.Format("{0:#,##0.00}", dTmpAmount);
                objData += iBnsCount.ToString("#,##0") + "|" + strAmount + "|";

                iBnsCount = DataAccess.BusinessCount("", "", "RET", strBnsCode[i], "", strRegYear, _BasedOnRegistration);
                //dGross = DataAccess.Gross("", "", "RET", strBnsCode[i], "", "", _BasedOnRegistration);
                dGross = DataAccess.Gross("", "", "RET", strBnsCode[i], "", strRegYear, _BasedOnRegistration);  // RMC 20171114 correction in Management report, do not include presumptive gr in total gr
                dGrossAddl = DataAccess.GrossAddl("", "", "RET", strBnsCode[i], "", "", strRegYear, _BasedOnRegistration);
                dPreGR = DataAccess.PreGR("", "", "RET", strBnsCode[i], "", "", strRegYear, _BasedOnRegistration);
                //dTmpAmount = dGross + dGrossAddl + dPreGR;
                dTmpAmount = dGross + dGrossAddl;   // RMC 20171114 correction in Management report, do not include presumptive gr in total gr
                if (strBnsCode[i].Length == 2)//AFM 20191205
                {
                    iBnsCountTotalRet += iBnsCount;
                    dTotalAmountRet += dTmpAmount;
                }

                strAmount = string.Format("{0:#,##0.00}", dTmpAmount);
                objData += iBnsCount.ToString("#,##0") + "|" + strAmount;

                frmReport.axVSPrinter1.Table = String.Format("<3000|>1000|>2000|>2000|>3000|>2000|>3000;{0}", objData);

            }
            frmReport.axVSPrinter1.FontBold = true;
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            string strTotalAmtNew = string.Format("{0:#,##0.00}", dTotalAmountNew);
            string strTotalAmtRen = string.Format("{0:#,##0.00}", dTotalAmountRen);
            string strTotalAmtRet = string.Format("{0:#,##0.00}", dTotalAmountRet);
            frmReport.axVSPrinter1.Table = String.Format(">3000|>1000|>2000|>2000|>3000|>2000|>3000;{0}",
                "TOTAL" + "|" + iBnsCountTotalNew.ToString("#,##0") + "|" + strTotalAmtNew + "|" +
                iBnsCountTotalRen.ToString("#,##0") + "|" + strTotalAmtRen + "|" +
                iBnsCountTotalRet.ToString("#,##0") + "|" + strTotalAmtRet);

            iBnsCountGrandTotal = (iBnsCountTotalNew + iBnsCountTotalRen) + iBnsCountTotalRet;
            frmReport.axVSPrinter1.Paragraph = "";
            frmReport.axVSPrinter1.Table = string.Format("<16000;{0}{1:#,##0}", "Total Number of Businesses: ", iBnsCountGrandTotal);
            frmReport.axVSPrinter1.FontBold = false;
            frmReport.axVSPrinter1.EndDoc();
        }

        public static void LoadByOrgKind(frmReport frmReport, String strRegYear)
        {
            List<String> strDistrict = new List<string>();
            List<String> strBrgy = new List<string>();
            int iBnsCount = 0;
            double dCapital = 0.00;
            double dCapitalAddl = 0.00;

            double dGross = 0.00;
            double dGrossAddl = 0.00;
            double dPreGR = 0.00;

            int iTotalBnsCountSingle = 0;
            int iTotalBnsCountPart = 0;
            int iTotalBnsCountCorp = 0;
            int iTotalBnsCountCoop = 0;

            double dTotalInitSingle = 0.00;
            double dTotalInitPart = 0.00;
            double dTotalInitCorp = 0.00;
            double dTotalInitCoop = 0.00;

            double dTotalDecSingle = 0.00;
            double dTotalDecPart = 0.00;
            double dTotalDecCorp = 0.00;
            double dTotalDecCoop = 0.00;

            int iGrandTotalBnsCountSingle = 0;
            int iGrandTotalBnsCountPart = 0;
            int iGrandTotalBnsCountCorp = 0;
            int iGrandTotalBnsCountCoop = 0;

            double dGrandTotalInitSingle = 0.00;
            double dGrandTotalInitPart = 0.00;
            double dGrandTotalInitCorp = 0.00;
            double dGrandTotalInitCoop = 0.00;

            double dGrandTotalDecSingle = 0.00;
            double dGrandTotalDecPart = 0.00;
            double dGrandTotalDecCorp = 0.00;
            double dGrandTotalDecCoop = 0.00;

            Boolean _BasedOnRegistration = false;

            if (strRegYear != "")
                _BasedOnRegistration = true;

            frmReport.axVSPrinter1.MarginTop = 350;
            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;

            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618

            frmReport.axVSPrinter1.FontSize = 8.0f;
            strDistrict = DataAccess.DistrictList();

            for (int i = 0; i != strDistrict.Count; i++)
            {

                if (strDistrict[i] == "")
                {
                    strDistrict[i] = " ";
                    frmReport.axVSPrinter1.Paragraph = String.Empty;
                    frmReport.axVSPrinter1.FontBold = true;
                    frmReport.axVSPrinter1.Table = string.Format("<17500;{0}", "NO DISTRICT");
                    frmReport.axVSPrinter1.FontBold = false;
                }
                else
                {
                    frmReport.axVSPrinter1.Table = String.Empty;
                    frmReport.axVSPrinter1.FontBold = true;
                    frmReport.axVSPrinter1.Table = string.Format("<17500;{0}", strDistrict[i]);
                    frmReport.axVSPrinter1.FontBold = false;
                }

                strBrgy = DataAccess.BarangayList(strDistrict[i]);
                for (int j = 0; j != strBrgy.Count; j++)
                {

                    if (strBrgy[j] == "")
                        strBrgy[j] = " ";

                    object objData = "";
                    string strInitCapital = "";
                    string strDeclaredGR = "";
                    double dTmpAmount = 0.00;

                    //Single proprietorship (s)
                    iBnsCount = DataAccess.BusinessCount(strBrgy[j], strDistrict[i], "", "", "SINGLE PROPRIETORSHIP", strRegYear, _BasedOnRegistration);
                    dCapital = DataAccess.Capital(strBrgy[j], strDistrict[i], "NEW", "", "SINGLE PROPRIETORSHIP", "", strRegYear, _BasedOnRegistration);
                    dCapitalAddl = DataAccess.CapitalAddl(strBrgy[j], strDistrict[i], "NEW", "", "SINGLE PROPRIETORSHIP", strRegYear, _BasedOnRegistration);
                    iTotalBnsCountSingle += iBnsCount;
                    dTmpAmount = dCapital + dCapitalAddl;
                    dTotalInitSingle += dTmpAmount;

                    strInitCapital = string.Format("{0:#,##0.00}", dTmpAmount);
                    dGross = DataAccess.Gross(strBrgy[j], strDistrict[i], "NEW", "", "SINGLE PROPRIETORSHIP", strRegYear, _BasedOnRegistration);
                    dGrossAddl = DataAccess.GrossAddl(strBrgy[j], strDistrict[i], "NEW", "", "SINGLE PROPRIETORSHIP", "", strRegYear, _BasedOnRegistration);
                    dPreGR = DataAccess.PreGR(strBrgy[j], strDistrict[i], "NEW", "", "SINGLE PROPRIETORSHIP", "", strRegYear, _BasedOnRegistration);
                    //dTmpAmount = dGross + dGrossAddl + dPreGR;
                    dTmpAmount = dGross + dGrossAddl;   // RMC 20171114 correction in Management report, do not include presumptive gr in total gr
                    strDeclaredGR = string.Format("{0:#,##0.00}", dTmpAmount);
                    dTotalDecSingle += dTmpAmount;
                    objData += strBrgy[j] + "|" + iBnsCount.ToString("#,##0") + "|" + strInitCapital + "|" + strDeclaredGR + "|";
                    //Single proprietorship (e)


                    //Partnership (s)
                    iBnsCount = DataAccess.BusinessCount(strBrgy[j], strDistrict[i], "", "", "PARTNERSHIP", strRegYear, _BasedOnRegistration);
                    dCapital = DataAccess.Capital(strBrgy[j], strDistrict[i], "NEW", "", "PARTNERSHIP", "", strRegYear, _BasedOnRegistration);
                    dCapitalAddl = DataAccess.CapitalAddl(strBrgy[j], strDistrict[i], "NEW", "", "PARTNERSHIP", strRegYear, _BasedOnRegistration);
                    iTotalBnsCountPart += iBnsCount;
                    dTmpAmount = dCapital + dCapitalAddl;
                    dTotalInitPart += dTmpAmount;

                    strInitCapital = string.Format("{0:#,###.00}", dTmpAmount);
                    dGross = DataAccess.Gross(strBrgy[j], strDistrict[i], "NEW", "", "PARTNERSHIP", strRegYear, _BasedOnRegistration);
                    dGrossAddl = DataAccess.GrossAddl(strBrgy[j], strDistrict[i], "NEW", "", "PARTNERSHIP", "", strRegYear, _BasedOnRegistration);
                    dPreGR = DataAccess.PreGR(strBrgy[j], strDistrict[i], "NEW", "", "PARTNERSHIP", "", strRegYear, _BasedOnRegistration);
                    //dTmpAmount = dGross + dGrossAddl + dPreGR;
                    dTmpAmount = dGross + dGrossAddl;   // RMC 20171114 correction in Management report, do not include presumptive gr in total gr
                    strDeclaredGR = string.Format("{0:#,##0.00}", dTmpAmount);
                    dTotalDecPart += dTmpAmount;
                    objData += iBnsCount.ToString("#,##0") + "|" + strInitCapital + "|" + strDeclaredGR + "|";
                    //Partnership (e)

                    //Corporation (s)
                    iBnsCount = DataAccess.BusinessCount(strBrgy[j], strDistrict[i], "", "", "CORPORATION", strRegYear, _BasedOnRegistration);
                    dCapital = DataAccess.Capital(strBrgy[j], strDistrict[i], "NEW", "", "CORPORATION", "", strRegYear, _BasedOnRegistration);
                    dCapitalAddl = DataAccess.CapitalAddl(strBrgy[j], strDistrict[i], "NEW", "", "CORPORATION", strRegYear, _BasedOnRegistration);
                    iTotalBnsCountCorp += iBnsCount;
                    dTmpAmount = dCapital + dCapitalAddl;
                    dTotalInitCorp += dTmpAmount;

                    strInitCapital = string.Format("{0:#,##0.00}", dTmpAmount);
                    dGross = DataAccess.Gross(strBrgy[j], strDistrict[i], "NEW", "", "CORPORATION", strRegYear, _BasedOnRegistration);
                    dGrossAddl = DataAccess.GrossAddl(strBrgy[j], strDistrict[i], "NEW", "", "CORPORATION", "", strRegYear, _BasedOnRegistration);
                    dPreGR = DataAccess.PreGR(strBrgy[j], strDistrict[i], "NEW", "", "CORPORATION", "", strRegYear, _BasedOnRegistration);
                    //dTmpAmount = dGross + dGrossAddl + dPreGR;
                    dTmpAmount = dGross + dGrossAddl;   // RMC 20171114 correction in Management report, do not include presumptive gr in total gr
                    strDeclaredGR = string.Format("{0:#,##0.00}", dTmpAmount);
                    dTotalDecCorp += dTmpAmount;
                    objData += iBnsCount.ToString("#,##0") + "|" + strInitCapital + "|" + strDeclaredGR + "|";
                    //Corporation (e)

                    //Cooperative (s)
                    iBnsCount = DataAccess.BusinessCount(strBrgy[j], strDistrict[i], "", "", "COOPERATIVE", strRegYear, _BasedOnRegistration);
                    dCapital = DataAccess.Capital(strBrgy[j], strDistrict[i], "NEW", "", "COOPERATIVE", "", strRegYear, _BasedOnRegistration);
                    dCapitalAddl = DataAccess.CapitalAddl(strBrgy[j], strDistrict[i], "NEW", "", "COOPERATIVE", strRegYear, _BasedOnRegistration);
                    iTotalBnsCountCoop += iBnsCount;
                    dTmpAmount = dCapital + dCapitalAddl;
                    dTotalInitCoop += dTmpAmount;

                    strInitCapital = string.Format("{0:#,##0.00}", dTmpAmount);
                    dGross = DataAccess.Gross(strBrgy[j], strDistrict[i], "NEW", "", "COOPERATIVE", strRegYear, _BasedOnRegistration);
                    dGrossAddl = DataAccess.GrossAddl(strBrgy[j], strDistrict[i], "NEW", "", "COOPERATIVE", "", strRegYear, _BasedOnRegistration);
                    dPreGR = DataAccess.PreGR(strBrgy[j], strDistrict[i], "NEW", "", "COOPERATIVE", "", strRegYear, _BasedOnRegistration);
                    //dTmpAmount = dGross + dGrossAddl + dPreGR;
                    dTmpAmount = dGross + dGrossAddl;   // RMC 20171114 correction in Management report, do not include presumptive gr in total gr
                    strDeclaredGR = string.Format("{0:#,##0.00}", dTmpAmount);
                    dTotalDecCoop += dTmpAmount;
                    objData += iBnsCount.ToString("#,##0") + "|" + strInitCapital + "|" + strDeclaredGR;
                    //Cooperative (e)

                    frmReport.axVSPrinter1.Table = String.Format("<1500|>1000|>1500|>1500|>1000|>1500|>1500|>1000|>1500|>1500|>1000|>1500|>1500;{0}", objData);

                }

                frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                string strTotalBnsCountSingle = string.Format("{0:#,##0}", iTotalBnsCountSingle);
                string strTotalBnsCountPart = string.Format("{0:#,##0}", iTotalBnsCountPart);
                string strTotalBnsCountCorp = string.Format("{0:#,##0}", iTotalBnsCountCorp);
                string strTotalBnsCountCoop = string.Format("{0:#,##0}", iTotalBnsCountCoop);
                
                string strTotalInitSingle = string.Format("{0:#,##0.00}", dTotalInitSingle);
                string strTotalInitPart = string.Format("{0:#,##0.00}", dTotalInitPart);
                string strTotalInitCorp = string.Format("{0:#,##0.00}", dTotalInitCorp);
                string strTotalInitCoop = string.Format("{0:#,##0.00}", dTotalInitCoop);

                string strTotalDecSingle = string.Format("{0:#,##0.00}", dTotalDecSingle);
                string strTotalDecPart = string.Format("{0:#,##0.00}", dTotalDecPart);
                string strTotalDecCorp = string.Format("{0:#,##0.00}", dTotalDecCorp);
                string strTotalDecCoop = string.Format("{0:#,##0.00}", dTotalDecCoop);

                frmReport.axVSPrinter1.Table = String.Format("^1500|>1000|>1500|>1500|>1000|>1500|>1500|>1000|>1500|>1500|>1000|>1500|>1500;{0}",
                    "TOTAL" + "|" + strTotalBnsCountSingle + "|" + strTotalInitSingle + "|" + strTotalDecSingle + "|" +
                    strTotalBnsCountPart + "|" + strTotalInitPart + "|" + strTotalDecPart + "|" +
                    strTotalBnsCountCorp + "|" + strTotalInitCorp + "|" + strTotalDecCorp + "|" +
                    strTotalBnsCountCoop + "|" + strTotalInitCoop + "|" + strTotalDecCoop);
                frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

                iGrandTotalBnsCountSingle += iTotalBnsCountSingle;
                iGrandTotalBnsCountPart += iTotalBnsCountPart;
                iGrandTotalBnsCountCorp += iTotalBnsCountCorp;
                iGrandTotalBnsCountCoop += iTotalBnsCountCoop;

                dGrandTotalInitSingle += dTotalInitSingle;
                dGrandTotalInitPart += dTotalInitPart;
                dGrandTotalInitCorp += dTotalInitCorp;
                dGrandTotalInitCoop += dTotalInitCoop;

                dGrandTotalDecSingle += dTotalDecSingle;
                dGrandTotalDecPart += dTotalDecPart;
                dGrandTotalDecCorp += dTotalDecCorp;
                dGrandTotalDecCoop += dTotalDecCoop;
            }

            frmReport.axVSPrinter1.Paragraph = String.Empty;
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            string strGrandTotalBnsCountSingle = string.Format("{0:#,##0}", iGrandTotalBnsCountSingle);
            string strGrandTotalBnsCountPart = string.Format("{0:#,##0}", iGrandTotalBnsCountPart);
            string strGrandTotalBnsCountCorp = string.Format("{0:#,##0}", iGrandTotalBnsCountCorp);
            string strGrandTotalBnsCountCoop = string.Format("{0:#,##0}", iGrandTotalBnsCountCoop);

            string strGrandTotalInitSingle = string.Format("{0:#,##0.00}", dGrandTotalInitSingle);
            string strGrandTotalInitPart = string.Format("{0:#,##0.00}", dGrandTotalInitPart);
            string strGrandTotalInitCorp = string.Format("{0:#,##0.00}", dGrandTotalInitCorp);
            string strGrandTotalInitCoop = string.Format("{0:#,##0.00}", dGrandTotalInitCoop);

            string strGrandTotalDecSingle = string.Format("{0:#,##0.00}", dGrandTotalDecSingle);
            string strGrandTotalDecPart = string.Format("{0:#,##0.00}", dGrandTotalDecPart);
            string strGrandTotalDecCorp = string.Format("{0:#,##0.00}", dGrandTotalDecCorp);
            string strGrandTotalDecCoop = string.Format("{0:#,##0.00}", dGrandTotalDecCoop);

            frmReport.axVSPrinter1.Table = String.Format("^1500|>1000|>1500|>1500|>1000|>1500|>1500|>1000|>1500|>1500|>1000|>1500|>1500;{0}",
                "GRAND TOTAL" + "|" + strGrandTotalBnsCountSingle + "|" + strGrandTotalInitSingle + "|" + strGrandTotalDecSingle + "|" +
                strGrandTotalBnsCountPart + "|" + strGrandTotalInitPart + "|" + strGrandTotalDecPart + "|" +
                strGrandTotalBnsCountCorp + "|" + strGrandTotalInitCorp + "|" + strGrandTotalDecCorp + "|" +
                strGrandTotalBnsCountCoop + "|" + strGrandTotalInitCoop + "|" + strGrandTotalDecCoop);
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            frmReport.axVSPrinter1.Paragraph = String.Empty;
            int iTotalRecords = iTotalBnsCountSingle + iTotalBnsCountPart + iTotalBnsCountCorp + iTotalBnsCountCoop;
            int iTotalRetire = DataAccess.RetireNo(strRegYear);
            int iTotalActive = iTotalRecords - DataAccess.RetireNo(strRegYear);

            frmReport.axVSPrinter1.Table = string.Format("<17500;{0}{1:#,##0}", "Total Number of Active Records: ", iTotalActive.ToString("#,##0"));
            frmReport.axVSPrinter1.Table = string.Format("<17500;{0}{1:#,##0}", "Total Number of Retire Business: ", iTotalRetire.ToString("#,##0"));
            frmReport.axVSPrinter1.Table = string.Format("<17500;{0}{1:#,##0}", "Total Number of Business: ", iTotalRecords.ToString("#,##0"));
            frmReport.axVSPrinter1.EndDoc();
        }

        public static void LoadByGrossReceipt(frmReport frmReport, String strRegYear)
        {
            List<String> strDistrict = new List<string>();
            List<String> strBrgy = new List<string>();
            List<String> strBIN = new List<string>();
            List<String> strTaxYear = new List<string>();
            List<double> dGR = new List<double>();
            List<double> dCapital = new List<double>(); // For Filling use only

            Boolean _BasedOnRegistration = false;

            if (strRegYear != "")
                _BasedOnRegistration = true;

            int iGrandTotalBnsCount1 = 0;
            int iGrandTotalBnsCount2 = 0;
            int iGrandTotalBnsCount3 = 0;
            int iGrandTotalBnsCount4 = 0;
            int iGrandTotalBnsCount5 = 0;

            double dGrandTotalAmt1 = 0;
            double dGrandTotalAmt2 = 0;
            double dGrandTotalAmt3 = 0;
            double dGrandTotalAmt4 = 0;
            double dGrandTotalAmt5 = 0;
            
            double dAmount = 0.00;
            object objData = "";
            frmReport.axVSPrinter1.MarginTop = 350;
            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;

            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618

            frmReport.axVSPrinter1.FontSize = 8.0f;

            strDistrict = DataAccess.DistrictList();
            for (int i = 0; i != strDistrict.Count; i++)
            {

                int iTotalBnsCount1 = 0;
                int iTotalBnsCount2 = 0;
                int iTotalBnsCount3 = 0;
                int iTotalBnsCount4 = 0;
                int iTotalBnsCount5 = 0;

                double dTotalAmt1 = 0;
                double dTotalAmt2 = 0;
                double dTotalAmt3 = 0;
                double dTotalAmt4 = 0;
                double dTotalAmt5 = 0;

                if (strDistrict[i] == "")
                {
                    strDistrict[i] = " ";
                    frmReport.axVSPrinter1.Paragraph = String.Empty;
                    frmReport.axVSPrinter1.FontBold = true;
                    frmReport.axVSPrinter1.Table = string.Format("<16500;{0}", "NO DISTRICT");
                    frmReport.axVSPrinter1.FontBold = false;
                }
                else
                {
                    frmReport.axVSPrinter1.Table = String.Empty;
                    frmReport.axVSPrinter1.FontBold = true;
                    frmReport.axVSPrinter1.Table = string.Format("<16500;{0}", strDistrict[i]);
                    frmReport.axVSPrinter1.FontBold = false;
                }

                strBrgy = DataAccess.BarangayList(strDistrict[i]);
                for (int j = 0; j != strBrgy.Count; j++)
                {

                    DataAccess.Businesses(strBrgy[j], strDistrict[i], "REN", out strBIN, out strTaxYear, out dGR, out dCapital, strRegYear, _BasedOnRegistration);
                    dAmount = 0;
                    objData = "";

                    int iBnsCount1 = 0;
                    int iBnsCount2 = 0;
                    int iBnsCount3 = 0;
                    int iBnsCount4 = 0;
                    int iBnsCount5 = 0;

                    double dAmount1 = 0;
                    double dAmount2 = 0;
                    double dAmount3 = 0;
                    double dAmount4 = 0;
                    double dAmount5 = 0;

                    for (int k = 0; k != strBIN.Count; k++)
                    {
                        
                        string strGR = dGR[k].ToString();
                        double.TryParse(strGR, out dAmount);

                        dAmount += DataAccess.GrossAddl("", "", "REN", "", "", strBIN[k], strTaxYear[k], _BasedOnRegistration);

                        //dAmount += DataAccess.PreGR("", "", "", "", "", strBIN[k], strTaxYear[k], _BasedOnRegistration);  // RMC 20171114 correction in Management report, do not include presumptive gr in total gr, put rem

                        if (dAmount < 100000)
                        {
                            iBnsCount1++;
                            dAmount1 += dAmount;
                        }
                        else if (dAmount >= 100000 && dAmount < 1000000)
                        {
                            iBnsCount2++;
                            dAmount2 += dAmount;
                        }
                        else if (dAmount >= 1000000 && dAmount < 20000000)
                        {
                            iBnsCount3++;
                            dAmount3 += dAmount;
                        }
                        else if (dAmount >= 20000000)
                        {
                            iBnsCount4++;
                            dAmount4 += dAmount;
                        }

                        dAmount5 += dAmount;

                    }

                    iBnsCount5 = strBIN.Count;

                    string strBnsCount1 = string.Format("{0:#,##0}", iBnsCount1);
                    string strBnsCount2 = string.Format("{0:#,##0}", iBnsCount2);
                    string strBnsCount3 = string.Format("{0:#,##0}", iBnsCount3);
                    string strBnsCount4 = string.Format("{0:#,##0}", iBnsCount4);
                    string strBnsCount5 = string.Format("{0:#,##0}", iBnsCount5);

                    string strAmount1 = string.Format("{0:#,##0.00}", dAmount1);
                    string strAmount2 = string.Format("{0:#,##0.00}", dAmount2);
                    string strAmount3 = string.Format("{0:#,##0.00}", dAmount3);
                    string strAmount4 = string.Format("{0:#,##0.00}", dAmount4);
                    string strAmount5 = string.Format("{0:#,##0.00}", dAmount5);

                    objData = strBrgy[j] + "|" + strBnsCount1 + "|" + strAmount1 + "|";
                    objData += strBnsCount2 + "|" + strAmount2 + "|";
                    objData += strBnsCount3 + "|" + strAmount3 + "|";
                    objData += strBnsCount4 + "|" + strAmount4 + "|";
                    objData += strBnsCount5 + "|" + strAmount5;

                    frmReport.axVSPrinter1.Table = string.Format("<1500|>1000|>2000|>1000|>2000|>1000|>2000|>1000|>2000|>1000|>2000;{0}", objData);

                    iTotalBnsCount1 += iBnsCount1;
                    iTotalBnsCount2 += iBnsCount2;
                    iTotalBnsCount3 += iBnsCount3;
                    iTotalBnsCount4 += iBnsCount4;
                    iTotalBnsCount5 += iBnsCount5;

                    dTotalAmt1 += dAmount1;
                    dTotalAmt2 += dAmount2;
                    dTotalAmt3 += dAmount3;
                    dTotalAmt4 += dAmount4;
                    dTotalAmt5 += dAmount5;

                }

                string strTotalBnsCount1 = string.Format("{0:#,##0}", iTotalBnsCount1);
                string strTotalBnsCount2 = string.Format("{0:#,##0}", iTotalBnsCount2);
                string strTotalBnsCount3 = string.Format("{0:#,##0}", iTotalBnsCount3);
                string strTotalBnsCount4 = string.Format("{0:#,##0}", iTotalBnsCount4);
                string strTotalBnsCount5 = string.Format("{0:#,##0}", iTotalBnsCount5);

                string strTotalAmt1 = string.Format("{0:#,##0.00}", dTotalAmt1);
                string strTotalAmt2 = string.Format("{0:#,##0.00}", dTotalAmt2);
                string strTotalAmt3 = string.Format("{0:#,##0.00}", dTotalAmt3);
                string strTotalAmt4 = string.Format("{0:#,##0.00}", dTotalAmt4);
                string strTotalAmt5 = string.Format("{0:#,##0.00}", dTotalAmt5);

                frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                objData = "TOTAL|" + strTotalBnsCount1 + "|" + strTotalAmt1 + "|" +
                                     strTotalBnsCount2 + "|" + strTotalAmt2 + "|" +
                                     strTotalBnsCount3 + "|" + strTotalAmt3 + "|" +
                                     strTotalBnsCount4 + "|" + strTotalAmt4 + "|" +
                                     strTotalBnsCount5 + "|" + strTotalAmt5;
                frmReport.axVSPrinter1.Table = string.Format("<1500|>1000|>2000|>1000|>2000|>1000|>2000|>1000|>2000|>1000|>2000;{0}", objData);
                frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

                iGrandTotalBnsCount1 += iTotalBnsCount1;
                iGrandTotalBnsCount2 += iTotalBnsCount2;
                iGrandTotalBnsCount3 += iTotalBnsCount3;
                iGrandTotalBnsCount4 += iTotalBnsCount4;
                iGrandTotalBnsCount5 += iTotalBnsCount5;

                dGrandTotalAmt1 += dTotalAmt1;
                dGrandTotalAmt2 += dTotalAmt2;
                dGrandTotalAmt3 += dTotalAmt3;
                dGrandTotalAmt4 += dTotalAmt4;
                dGrandTotalAmt5 += dTotalAmt5;
            }

            string strGrandTotalBnsCount1 = string.Format("{0:#,##0}", iGrandTotalBnsCount1);
            string strGrandTotalBnsCount2 = string.Format("{0:#,##0}", iGrandTotalBnsCount2);
            string strGrandTotalBnsCount3 = string.Format("{0:#,##0}", iGrandTotalBnsCount3);
            string strGrandTotalBnsCount4 = string.Format("{0:#,##0}", iGrandTotalBnsCount4);
            string strGrandTotalBnsCount5 = string.Format("{0:#,##0}", iGrandTotalBnsCount5);

            string strGrandTotalAmt1 = string.Format("{0:#,##0.00}", dGrandTotalAmt1);
            string strGrandTotalAmt2 = string.Format("{0:#,##0.00}", dGrandTotalAmt2);
            string strGrandTotalAmt3 = string.Format("{0:#,##0.00}", dGrandTotalAmt3);
            string strGrandTotalAmt4 = string.Format("{0:#,##0.00}", dGrandTotalAmt4);
            string strGrandTotalAmt5 = string.Format("{0:#,##0.00}", dGrandTotalAmt5);

            frmReport.axVSPrinter1.Paragraph = "";
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            objData = "GRAND TOTAL|" + strGrandTotalBnsCount1 + "|" + strGrandTotalAmt1 + "|" +
                                 strGrandTotalBnsCount2 + "|" + strGrandTotalAmt2 + "|" +
                                 strGrandTotalBnsCount3 + "|" + strGrandTotalAmt3 + "|" +
                                 strGrandTotalBnsCount4 + "|" + strGrandTotalAmt4 + "|" +
                                 strGrandTotalBnsCount5 + "|" + strGrandTotalAmt5;
            frmReport.axVSPrinter1.Table = string.Format("<1500|>1000|>2000|>1000|>2000|>1000|>2000|>1000|>2000|>1000|>2000;{0}", objData);
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            frmReport.axVSPrinter1.Paragraph = "";
            frmReport.axVSPrinter1.Table = string.Format("<16500;{0}{1:#,##0}", "Total Number of Businesses ", iGrandTotalBnsCount5);

            frmReport.axVSPrinter1.EndDoc();
        }

        public static void LoadByInitCapital(frmReport frmReport, String strRegYear)
        {
            List<String> strDistrict = new List<string>();
            List<String> strBrgy = new List<string>();
            List<String> strBIN = new List<string>();
            List<String> strTaxYear = new List<string>();
            List<double> dGR = new List<double>(); // For Filling use only
            List<double> dCapital = new List<double>();

            Boolean _BasedOnRegistration = false;

            if (strRegYear != "")
                _BasedOnRegistration = true;

            int iGrandTotalBnsCount1 = 0;
            int iGrandTotalBnsCount2 = 0;
            int iGrandTotalBnsCount3 = 0;
            int iGrandTotalBnsCount4 = 0;
            int iGrandTotalBnsCount5 = 0;
            int iGrandTotalBnsCount6 = 0;
            int iGrandTotalBnsCount7 = 0;
            int iGrandTotalBnsCount8 = 0;

            double dGrandTotalAmt1 = 0;
            double dGrandTotalAmt2 = 0;
            double dGrandTotalAmt3 = 0;
            double dGrandTotalAmt4 = 0;
            double dGrandTotalAmt5 = 0;
            double dGrandTotalAmt6 = 0;
            double dGrandTotalAmt7 = 0;
            double dGrandTotalAmt8 = 0;

            double dAmount = 0.00;
            object objData = "";
            frmReport.axVSPrinter1.MarginTop = 350;
            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;

            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618

            frmReport.axVSPrinter1.FontSize = 8.0f;

            strDistrict = DataAccess.DistrictList();
            for (int i = 0; i != strDistrict.Count; i++)
            {

                int iTotalBnsCount1 = 0;
                int iTotalBnsCount2 = 0;
                int iTotalBnsCount3 = 0;
                int iTotalBnsCount4 = 0;
                int iTotalBnsCount5 = 0;
                int iTotalBnsCount6 = 0;
                int iTotalBnsCount7 = 0;
                int iTotalBnsCount8 = 0;

                double dTotalAmt1 = 0;
                double dTotalAmt2 = 0;
                double dTotalAmt3 = 0;
                double dTotalAmt4 = 0;
                double dTotalAmt5 = 0;
                double dTotalAmt6 = 0;
                double dTotalAmt7 = 0;
                double dTotalAmt8 = 0;

                if (strDistrict[i] == "")
                {
                    strDistrict[i] = " ";
                    frmReport.axVSPrinter1.Paragraph = String.Empty;
                    frmReport.axVSPrinter1.FontBold = true;
                    frmReport.axVSPrinter1.Table = string.Format("<19100;{0}", "NO DISTRICT");
                    frmReport.axVSPrinter1.FontBold = false;
                }
                else
                {
                    frmReport.axVSPrinter1.Table = String.Empty;
                    frmReport.axVSPrinter1.FontBold = true;
                    frmReport.axVSPrinter1.Table = string.Format("<19100;{0}", strDistrict[i]);
                    frmReport.axVSPrinter1.FontBold = false;
                }

                strBrgy = DataAccess.BarangayList(strDistrict[i]);
                for (int j = 0; j != strBrgy.Count; j++)
                {

                    DataAccess.Businesses(strBrgy[j], strDistrict[i], "NEW", out strBIN, out strTaxYear, out dGR, out dCapital, strRegYear, _BasedOnRegistration);
                    dAmount = 0;
                    objData = "";

                    int iBnsCount1 = 0;
                    int iBnsCount2 = 0;
                    int iBnsCount3 = 0;
                    int iBnsCount4 = 0;
                    int iBnsCount5 = 0;
                    int iBnsCount6 = 0;
                    int iBnsCount7 = 0;
                    int iBnsCount8 = 0;

                    double dAmount1 = 0;
                    double dAmount2 = 0;
                    double dAmount3 = 0;
                    double dAmount4 = 0;
                    double dAmount5 = 0;
                    double dAmount6 = 0;
                    double dAmount7 = 0;
                    double dAmount8 = 0;
                    
                    for (int k = 0; k != strBIN.Count; k++)
                    {

                        string strCapital = dCapital[k].ToString();
                        double.TryParse(strCapital, out dAmount);

                        dAmount += DataAccess.Capital("", "", "", "", "", strBIN[k], strTaxYear[k], _BasedOnRegistration);
                        
                        if (dAmount < 100000)
                        {
                            iBnsCount1++;
                            dAmount1 += dAmount;
                        }
                        else if (dAmount >= 100000 && dAmount < 500000)
                        {
                            iBnsCount2++;
                            dAmount2 += dAmount;
                        }
                        else if (dAmount >= 500000 && dAmount < 1000000)
                        {
                            iBnsCount3++;
                            dAmount3 += dAmount;
                        }
                        else if (dAmount >= 1000000 && dAmount < 5000000)
                        {
                            iBnsCount4++;
                            dAmount4 += dAmount;
                        }
                        else if (dAmount >= 5000000 && dAmount < 10000000)
                        {
                            iBnsCount5++;
                            dAmount5 += dAmount;
                        }
                        else if (dAmount >= 10000000 && dAmount < 20000000)
                        {
                            iBnsCount6++;
                            dAmount6 += dAmount;
                        }
                        else if (dAmount >= 20000000)
                        {
                            iBnsCount7++;
                            dAmount7 += dAmount;
                        }

                        dAmount8 += dAmount;

                    }

                    iBnsCount8 = strBIN.Count;

                    string strBnsCount1 = string.Format("{0:#,##0}", iBnsCount1);
                    string strBnsCount2 = string.Format("{0:#,##0}", iBnsCount2);
                    string strBnsCount3 = string.Format("{0:#,##0}", iBnsCount3);
                    string strBnsCount4 = string.Format("{0:#,##0}", iBnsCount4);
                    string strBnsCount5 = string.Format("{0:#,##0}", iBnsCount5);
                    string strBnsCount6 = string.Format("{0:#,##0}", iBnsCount6);
                    string strBnsCount7 = string.Format("{0:#,##0}", iBnsCount7);
                    string strBnsCount8 = string.Format("{0:#,##0}", iBnsCount8);

                    string strAmount1 = string.Format("{0:#,##0.00}", dAmount1);
                    string strAmount2 = string.Format("{0:#,##0.00}", dAmount2);
                    string strAmount3 = string.Format("{0:#,##0.00}", dAmount3);
                    string strAmount4 = string.Format("{0:#,##0.00}", dAmount4);
                    string strAmount5 = string.Format("{0:#,##0.00}", dAmount5);
                    string strAmount6 = string.Format("{0:#,##0.00}", dAmount6);
                    string strAmount7 = string.Format("{0:#,##0.00}", dAmount7);
                    string strAmount8 = string.Format("{0:#,##0.00}", dAmount8);


                    objData = strBrgy[j] + "|" + strBnsCount1 + "|" + strAmount1 + "|";
                    objData += strBnsCount2 + "|" + strAmount2 + "|";
                    objData += strBnsCount3 + "|" + strAmount3 + "|";
                    objData += strBnsCount4 + "|" + strAmount4 + "|";
                    objData += strBnsCount5 + "|" + strAmount5 + "|";
                    objData += strBnsCount6 + "|" + strAmount6 + "|";
                    objData += strBnsCount7 + "|" + strAmount7 + "|";
                    objData += strBnsCount8 + "|" + strAmount8;


                    frmReport.axVSPrinter1.Table = string.Format("<1500|>800|>1400|>800|>1400|>800|>1400|>800|>1400|>800|>1400|>800|>1400|>800|>1400|>800|>1400;{0}", objData);

                    iTotalBnsCount1 += iBnsCount1;
                    iTotalBnsCount2 += iBnsCount2;
                    iTotalBnsCount3 += iBnsCount3;
                    iTotalBnsCount4 += iBnsCount4;
                    iTotalBnsCount5 += iBnsCount5;
                    iTotalBnsCount6 += iBnsCount6;
                    iTotalBnsCount7 += iBnsCount7;
                    iTotalBnsCount8 += iBnsCount8;

                    dTotalAmt1 += dAmount1;
                    dTotalAmt2 += dAmount2;
                    dTotalAmt3 += dAmount3;
                    dTotalAmt4 += dAmount4;
                    dTotalAmt5 += dAmount5;
                    dTotalAmt6 += dAmount6;
                    dTotalAmt7 += dAmount7;
                    dTotalAmt8 += dAmount8;

                }

                string strTotalBnsCount1 = string.Format("{0:#,##0}", iTotalBnsCount1);
                string strTotalBnsCount2 = string.Format("{0:#,##0}", iTotalBnsCount2);
                string strTotalBnsCount3 = string.Format("{0:#,##0}", iTotalBnsCount3);
                string strTotalBnsCount4 = string.Format("{0:#,##0}", iTotalBnsCount4);
                string strTotalBnsCount5 = string.Format("{0:#,##0}", iTotalBnsCount5);
                string strTotalBnsCount6 = string.Format("{0:#,##0}", iTotalBnsCount6);
                string strTotalBnsCount7 = string.Format("{0:#,##0}", iTotalBnsCount7);
                string strTotalBnsCount8 = string.Format("{0:#,##0}", iTotalBnsCount8);

                string strTotalAmt1 = string.Format("{0:#,##0.00}", dTotalAmt1);
                string strTotalAmt2 = string.Format("{0:#,##0.00}", dTotalAmt2);
                string strTotalAmt3 = string.Format("{0:#,##0.00}", dTotalAmt3);
                string strTotalAmt4 = string.Format("{0:#,##0.00}", dTotalAmt4);
                string strTotalAmt5 = string.Format("{0:#,##0.00}", dTotalAmt5);
                string strTotalAmt6 = string.Format("{0:#,##0.00}", dTotalAmt6);
                string strTotalAmt7 = string.Format("{0:#,##0.00}", dTotalAmt7);
                string strTotalAmt8 = string.Format("{0:#,##0#.00}", dTotalAmt8);

                frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                objData = "TOTAL|" + strTotalBnsCount1 + "|" + strTotalAmt1 + "|" +
                                     strTotalBnsCount2 + "|" + strTotalAmt2 + "|" +
                                     strTotalBnsCount3 + "|" + strTotalAmt3 + "|" +
                                     strTotalBnsCount4 + "|" + strTotalAmt4 + "|" +
                                     strTotalBnsCount5 + "|" + strTotalAmt5 + "|" +
                                     strTotalBnsCount6 + "|" + strTotalAmt6 + "|" +
                                     strTotalBnsCount7 + "|" + strTotalAmt7 + "|" +
                                     strTotalBnsCount8 + "|" + strTotalAmt8;
                frmReport.axVSPrinter1.Table = string.Format("<1500|>800|>1400|>800|>1400|>800|>1400|>800|>1400|>800|>1400|>800|>1400|>800|>1400|>800|>1400;{0}", objData);
                frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

                iGrandTotalBnsCount1 += iTotalBnsCount1;
                iGrandTotalBnsCount2 += iTotalBnsCount2;
                iGrandTotalBnsCount3 += iTotalBnsCount3;
                iGrandTotalBnsCount4 += iTotalBnsCount4;
                iGrandTotalBnsCount5 += iTotalBnsCount5;
                iGrandTotalBnsCount6 += iTotalBnsCount6;
                iGrandTotalBnsCount7 += iTotalBnsCount7;
                iGrandTotalBnsCount8 += iTotalBnsCount8;

                dGrandTotalAmt1 += dTotalAmt1;
                dGrandTotalAmt2 += dTotalAmt2;
                dGrandTotalAmt3 += dTotalAmt3;
                dGrandTotalAmt4 += dTotalAmt4;
                dGrandTotalAmt5 += dTotalAmt5;
                dGrandTotalAmt6 += dTotalAmt6;
                dGrandTotalAmt7 += dTotalAmt7;
                dGrandTotalAmt8 += dTotalAmt8;
            }

            string strGrandTotalBnsCount1 = string.Format("{0:#,##0}", iGrandTotalBnsCount1);
            string strGrandTotalBnsCount2 = string.Format("{0:#,##0}", iGrandTotalBnsCount2);
            string strGrandTotalBnsCount3 = string.Format("{0:#,##0}", iGrandTotalBnsCount3);
            string strGrandTotalBnsCount4 = string.Format("{0:#,##0}", iGrandTotalBnsCount4);
            string strGrandTotalBnsCount5 = string.Format("{0:#,##0}", iGrandTotalBnsCount5);
            string strGrandTotalBnsCount6 = string.Format("{0:#,##0}", iGrandTotalBnsCount6);
            string strGrandTotalBnsCount7 = string.Format("{0:#,##0}", iGrandTotalBnsCount7);
            string strGrandTotalBnsCount8 = string.Format("{0:#,##0}", iGrandTotalBnsCount8);

            string strGrandTotalAmt1 = string.Format("{0:#,##0.00}", dGrandTotalAmt1);
            string strGrandTotalAmt2 = string.Format("{0:#,##0.00}", dGrandTotalAmt2);
            string strGrandTotalAmt3 = string.Format("{0:#,##0.00}", dGrandTotalAmt3);
            string strGrandTotalAmt4 = string.Format("{0:#,##0.00}", dGrandTotalAmt4);
            string strGrandTotalAmt5 = string.Format("{0:#,##0.00}", dGrandTotalAmt5);
            string strGrandTotalAmt6 = string.Format("{0:#,##0.00}", dGrandTotalAmt6);
            string strGrandTotalAmt7 = string.Format("{0:#,##0.00}", dGrandTotalAmt7);
            string strGrandTotalAmt8 = string.Format("{0:#,##0.00}", dGrandTotalAmt8);

            frmReport.axVSPrinter1.Paragraph = "";
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            objData = "GRAND TOTAL|" + strGrandTotalBnsCount1 + "|" + strGrandTotalAmt1 + "|" +
                                 strGrandTotalBnsCount2 + "|" + strGrandTotalAmt2 + "|" +
                                 strGrandTotalBnsCount3 + "|" + strGrandTotalAmt3 + "|" +
                                 strGrandTotalBnsCount4 + "|" + strGrandTotalAmt4 + "|" +
                                 strGrandTotalBnsCount5 + "|" + strGrandTotalAmt5 + "|" +
                                 strGrandTotalBnsCount6 + "|" + strGrandTotalAmt6 + "|" +
                                 strGrandTotalBnsCount7 + "|" + strGrandTotalAmt7 + "|" +
                                 strGrandTotalBnsCount8 + "|" + strGrandTotalAmt8;
            frmReport.axVSPrinter1.Table = string.Format("<1500|>800|>1400|>800|>1400|>800|>1400|>800|>1400|>800|>1400|>800|>1400|>800|>1400|>800|>1400;{0}", objData);
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            frmReport.axVSPrinter1.Paragraph = "";
            frmReport.axVSPrinter1.Table = string.Format("<19100;{0}{1:#,##0}", "Total Number of Businesses: ", iGrandTotalBnsCount8);


            frmReport.axVSPrinter1.EndDoc();
        }
    }
}
