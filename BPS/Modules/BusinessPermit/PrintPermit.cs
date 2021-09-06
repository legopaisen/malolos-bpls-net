using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.PrintUtilities;
using Amellar.Common.AppSettings;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.BusinessPermit
{
    class PrintPermit
    {
        OracleResultSet pSet = new OracleResultSet();
        private VSPrinterEmuModel model;
        private VSPrinterEmuModel pageHeaderModel;
        private string m_sBIN = string.Empty;
        private string m_sTaxYear = string.Empty;
        private string m_sPermitNumber = string.Empty;
        private string m_sReportName = string.Empty;
        private string m_sNotation = string.Empty;

        
        public string BIN
        {
            get { return m_sBIN; }
            set { m_sBIN = value; }
        }

        public string TaxYear
        {
            get { return m_sTaxYear; }
            set { m_sTaxYear = value; }
        }

        public string PermitNumber
        {
            get { return m_sPermitNumber;}
            set {m_sPermitNumber = value;}
        }

        public string ReportName
        {
            get { return m_sReportName; }
            set { m_sReportName = value; }
        }

        public PrintPermit()
        {
            model = new VSPrinterEmuModel();
            model.LeftMargin = 50;
            model.TopMargin = 50;
            model.MaxY = 1100 - 200;

            pageHeaderModel = new VSPrinterEmuModel();
            pageHeaderModel.LeftMargin = 50;
            pageHeaderModel.TopMargin = 500;
            pageHeaderModel.MaxY = 1100 - 200;
            
        }

        public void FormLoad()
        {
            if (m_sReportName == "Mayor's Permit")
                //    LoadOldPermit();
                LoadNewPermit();
            else
                LoadCertOfReg();
            pageHeaderModel.PageCount = model.PageCount;
            PrintReportPreview();
        }

        private void PrintReportPreview()
        {
            model.MaxY = 1100;
            VSPrinterEmuDocument doc = new VSPrinterEmuDocument();
            doc.Model = model;

            doc.PageHeaderModel = pageHeaderModel;
            doc.Model.Reset();

            doc.DefaultPageSettings.Margins.Top = 50;
            doc.DefaultPageSettings.Margins.Left = 100;
            doc.DefaultPageSettings.Margins.Bottom = 50;
            doc.DefaultPageSettings.Margins.Right = 100;

            if (m_sReportName == "Mayor's Permit")
                doc.DefaultPageSettings.PaperSize = new PaperSize("", 850, 1100);
            else
                doc.DefaultPageSettings.PaperSize = new PaperSize("", 850, 1100);

            doc.PrintPage += new PrintPageEventHandler(_Doc_PrintPage);

            frmMyPrintPreviewDialog dlgPreview = new frmMyPrintPreviewDialog();
            dlgPreview.Document = doc;
            dlgPreview.ClientSize = new System.Drawing.Size(640, 480);
            dlgPreview.ShowIcon = true;
            //dlgPreview.IsAllowExport = true;
            dlgPreview.WindowState = FormWindowState.Maximized;
            dlgPreview.ShowDialog();
            model.Dispose();
        }

        private void _Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            //bool blnVisibleState = this.Visible;
            VSPrinterEmuDocument doc = (VSPrinterEmuDocument)sender;
            doc.Render(e);
        }

        private void LoadOldPermit()
        {
            string sBnsNm = "";
		    string sOwnNm = "";
		    string sBnsAddress = "";
		    string sOwnAddress = "";
		    string sBnsCode = "";
		    string sORNo = "";
            string sTotalPayment = "";
		    string sBnsStat = "";
		    string sTelNo = "";
		    string sDay = "";
		    string sMonth = "";
            string sYear = "";
            string sDayth = "";
            DateTime dtpBillDate = AppSettingsManager.GetCurrentDate();
            DateTime dtpPermitExpDt = AppSettingsManager.GetCurrentDate();
            DateTime dtpORDate = AppSettingsManager.GetCurrentDate();

            pSet.Query = string.Format("select * from rep_mp where bin = '{0}' and tax_year = '{1}' and permit_no = '{2}'",m_sBIN,m_sTaxYear,m_sPermitNumber);
	        if(pSet.Execute())
	        {
                if(pSet.Read())
	            {
		            sBnsNm = pSet.GetString("bns_nm");
		            sOwnNm = pSet.GetString("own_nm");
		            sBnsAddress = pSet.GetString("bns_address");
		            sOwnAddress = pSet.GetString("own_address");
		            sBnsCode = pSet.GetString("bns_desc");
		            sORNo = pSet.GetString("or_no");
		            dtpORDate	= pSet.GetDateTime("or_dt");
		            dtpBillDate = pSet.GetDateTime("permit_dt");
		            sTotalPayment = string.Format("{0:##.00}", pSet.GetDouble("amount1"));
		            sBnsStat = pSet.GetString("bns_stat");
		            sTelNo = pSet.GetString("bns_telno");
		            dtpPermitExpDt = pSet.GetDateTime("ml_dt");

                    sDay = dtpBillDate.Day.ToString();
                    sMonth = dtpBillDate.ToString("MMMM");
                    sYear = dtpBillDate.Year.ToString();

		        	if (Convert.ToInt32(sDay) == 11 || Convert.ToInt32(sDay) == 12 || Convert.ToInt32(sDay) == 13)
		            {
			            sDayth = sDay+"th";
		            }
		            else
		            {
			            switch(Convert.ToInt32(sDay))
			            {
				            case 1:sDayth = sDay+"st";break;
				            case 2:sDayth = sDay+"nd";break;
				            case 3:sDayth = sDay+"rd";break;
				            default:sDayth = sDay+"th";break;
                        }
		            }
		
                    model.SetTextAlign(1);
                    model.SetFontBold(1);
                    model.SetFontName("Arial");
                    model.SetCurrentY(2850);
                    model.SetFontSize(14);
		    		model.SetMarginLeft(8200);
                    model.SetTable(string.Format("2750;{0}", m_sPermitNumber));
			        model.SetFontBold(0);
		            model.SetFontSize(11);
		            model.SetCurrentY(5150);
		            model.SetMarginLeft(1700);
                    model.SetTable(string.Format("^4500;{0}", sOwnNm));
                    model.SetCurrentY(5150);
                    model.SetMarginLeft(6600);

                    string sAddr = "";
                    int iLGU = 0;
                    int iAddr = 0;
                    iLGU = AppSettingsManager.GetConfigValue("02").Length + 2;
                    iAddr = sBnsAddress.Length;
                    sAddr = StringUtilities.Left(sBnsAddress, iAddr - iLGU);

                    model.SetTable(string.Format("<5000;{0}", sAddr));
		            model.SetCurrentY(5600);
                    model.SetMarginLeft(6200);
                    model.SetTable(string.Format("<5000;{0}", sBnsNm));
		            model.SetCurrentY(7750);
                    model.SetMarginLeft(6000);
                    model.SetTable(string.Format("^5000;{0}", sDayth));

		            long y;  
		            y = model.GetCurrentY();
                    model.SetMarginLeft(1500);
		            
                    model.SetFontSize(4);
		            model.SetParagraph("");
		            model.SetFontSize(11);
		            model.SetTable(string.Format("^5000;{0}", sMonth));
				
                    model.SetCurrentY(y);
		            model.SetMarginLeft(7000);
		            model.SetFontSize(4);
		            model.SetParagraph("");
		            model.SetFontSize(11);
		            model.SetTable(string.Format("^1000;{0}", sYear.Substring(2,2)));
		            model.SetFontBold(1);
		            model.SetFontSize(9);
                    model.SetMarginLeft(0);
		            model.SetMarginLeft(1700);

		            string sAddlBns = "";

                    model.SetParagraph("");
		            model.SetParagraph("");
		            model.SetTable(string.Format("<1700;{0}", "LINE/S OF BUSINESS:"));
		            model.SetFontBold(0);
		            
                    model.SetTable(string.Format("<1700;{0}"," "+sBnsCode));
		            
                    pSet.Close();

                    pSet.Query = "select bns_code_main from addl_bns";
		            pSet.Query+= string.Format(" where bin = '{0}' and tax_year = '{1}' and bns_stat <> 'RET'", m_sBIN, m_sTaxYear);
		            if(pSet.Execute())
                    {
                        while(pSet.Read())
		                {
				            sAddlBns = pSet.GetString("bns_code_main");
				            sAddlBns = " " + AppSettingsManager.GetBnsDesc(sAddlBns);
                            model.SetTable(string.Format("<1700;{0}", sAddlBns));    
                        }
                    }
                    pSet.Close();
                }
            }
			
			model.SetFontSize(11);
		    model.SetCurrentY(12100);
		    model.SetMarginLeft(3100);
		    model.SetTable("<5000;"+sMonth+" "+sDay+" ,"+sYear);
            model.SetFontSize(4);
		    model.SetParagraph("");
		    model.SetFontSize(11);

            sMonth = dtpPermitExpDt.ToString("MMMM");
		    sDay = dtpPermitExpDt.Day.ToString();
		    sYear = dtpPermitExpDt.Year.ToString();
		    model.SetTable("<5000;"+sMonth+" "+sDay+" ,"+sYear);

		    model.SetFontSize(4);
		    model.SetParagraph("");
		    model.SetFontSize(11);
		    model.SetTable(string.Format("<5000;{0}", sORNo));
		    model.SetFontSize(4);
            model.SetParagraph("");
            model.SetFontSize(11);

            sMonth = dtpORDate.ToString("MMMM");
            sDay = dtpORDate.Day.ToString();
            sYear = dtpORDate.Year.ToString();
		    model.SetTable("<5000;"+sMonth+" "+sDay+" ,"+sYear);
            
            model.SetMarginLeft(1500);		   
            model.SetFontSize(4);
		    model.SetParagraph("");
		    model.SetFontSize(6);
		
            model.SetTable(string.Empty);
            model.SetTable(string.Empty);

            model.SetTable(string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserName));
            model.SetTable(string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate()));

        }

        private void LoadCertOfReg()
        { 
            string sDay = "";
		    string sMonth = "";
            string sYear = "";
            string sDayth = "";
            string sOwnCode = "";

            sDay = AppSettingsManager.GetCurrentDate().Day.ToString();
            sMonth = AppSettingsManager.GetCurrentDate().ToString("MMMM");
            sYear = ConfigurationAttributes.CurrentYear;

            if (Convert.ToInt32(sDay) == 11 || Convert.ToInt32(sDay) == 12 || Convert.ToInt32(sDay) == 13)
            {
                sDayth = sDay + "th";
            }
            else
            {
                switch (Convert.ToInt32(sDay))
                {
                    case 1: sDayth = sDay + "st"; break;
                    case 2: sDayth = sDay + "nd"; break;
                    case 3: sDayth = sDay + "rd"; break;
                    default: sDayth = sDay + "th"; break;
                }
            }
		

            model.SetTextAlign(1);
            model.SetFontBold(1);
            model.SetFontName("Arial");
	        model.SetCurrentY(1000);
	        model.SetTable(">10000;"+m_sPermitNumber);
	        model.SetCurrentY(5200);
            model.SetTextAlign(1);
            sOwnCode = AppSettingsManager.GetBnsOwnCode(m_sBIN);
	        model.SetTable("^10000;"+AppSettingsManager.GetBnsOwner(sOwnCode));
	        model.SetParagraph("");
            model.SetTable(">2000|>3500|>1500;" + sDayth + "|" + sMonth + "|" + sYear);
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
	        model.SetTable("^10000;"+AppSettingsManager.GetBnsName(m_sBIN));
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
	        model.SetFontSize(7);
            model.SetParagraph("");
	        model.SetFontSize(12);
	        model.SetTextAlign(0);
            model.SetTable(">8300;" + sDayth);
	        model.SetTable(">3500|>3500;"+sMonth+"|"+sYear);
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetParagraph("");
            model.SetTable(">5500|>2800;" + sDayth + "|" + sMonth);
	        model.SetTable(">7000;"+sYear);

            // RMC 20110801 (s)
            if (AuditTrail.InsertTrail("ABBP", "rep_mp", "Cert. of registration of " + m_sBIN) == 0)
            {
                pSet.Rollback();
                pSet.Close();
                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // RMC 20110801 (e)
        }

        private void LoadNewPermit()
        {
            // RMC 20110801

            // RMC 20110810 modified whole function
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec2 = new OracleResultSet();
            
            string sBnsNm = "";
            string sOwnNm = "";
            string sBnsAddress = "";
            string sBnsMainDesc = "";
            string sBnsCode = "";
            string sBnsDesc = "";
            string sORNo = "";
            string sOrgnKind = "";
            string sBnsStat = "";
            string sTelNo = "";
            string sDay = "";
            string sCapital = "";
            string sGross = "";
            string sFeeAmt = "";
            string sRemarks = "";
            string sFlrArea = "";
            string sUnitNo = "";
            string sQtrRemarks = "";
            string sORDate = "";
            string sQtrPaid = "";
            DateTime dtpBillDate = AppSettingsManager.GetCurrentDate();
            DateTime dtpPermitExpDt = AppSettingsManager.GetCurrentDate();
            DateTime dtpORDate;

            pSet.Query = string.Format("select distinct * from pay_hist where bin = '{0}' and tax_year = '{1}' order by or_date desc", m_sBIN, m_sTaxYear);  // RMC 20110823 added desc in ordering
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sQtrPaid = pSet.GetString("qtr_paid");  // RMC 20110823
                    sBnsNm = AppSettingsManager.GetBnsName(m_sBIN);
                    sOwnNm = AppSettingsManager.GetBnsOwnCode(m_sBIN);
                    sOwnNm = AppSettingsManager.GetBnsOwner(sOwnNm);
                    sBnsAddress = AppSettingsManager.GetBnsAddress(m_sBIN);
                    sBnsCode = AppSettingsManager.GetBnsCodeMain(m_sBIN);
                    sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);
                    sBnsMainDesc = AppSettingsManager.GetSubBnsDesc(sBnsCode.Substring(0, 2), "B");
                    sORNo = pSet.GetString("or_no");

                    dtpORDate = pSet.GetDateTime("or_date");
                    sORDate = string.Format("{0:MMMM dd, yyyy}", dtpORDate);

                    pRec.Query = string.Format("select * from businesses where bin = '{0}'", m_sBIN);
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sOrgnKind = pRec.GetString("orgn_kind").Trim();
                            sBnsStat = pRec.GetString("bns_stat");
                            sFlrArea = "AREA: " + string.Format("{0:##0.00}", pRec.GetDouble("flr_area")); // RMC 20110810 modified
                            //sFlrArea += " sqm";
                            
                            sUnitNo = "No. of Units: ";     // pending anong field ito?

                            if (sBnsStat == "REN")
                            {
                                sBnsStat = "RENEWAL";
                                sGross = string.Format("{0:#,##0.00}", pRec.GetDouble("gr_1"));
                                sCapital = "";

                                if (sGross == "0.00")
                                    sRemarks = "No Operation";
                            }
                            else if (sBnsStat == "NEW")
                            {
                                sCapital = string.Format("{0:#,##0.00}", pRec.GetDouble("capital"));
                                sGross = "";
                            }
                            
                        }
                    }
                    pRec.Close();
                    
                                                            
                    model.SetTextAlign(1);
                    model.SetFontBold(1);
                    model.SetFontName("Arial");
                    //model.SetCurrentY(1000);
                    model.SetFontSize(11);
                    model.SetMarginLeft(11100);
                    //long y = model.GetCurrentY();
                    model.SetCurrentY(800);
                    model.SetTable(string.Format("<3000;{0}", m_sPermitNumber));
                    model.SetFontBold(0);
                    //model.SetFontSize(11);
                    model.SetCurrentY(2500);
                    model.SetMarginLeft(1700);
                    //long y = model.GetCurrentY();
                    model.SetCurrentY(2699);
                    model.SetTable(string.Format("^11100;{0}", sBnsNm));
                    model.SetCurrentY(3099);
                    model.SetTable(string.Format("^11100;{0}", sBnsAddress));
                    model.SetCurrentY(3499);
                    model.SetTable(string.Format("^11100;{0}", sOwnNm));
                    model.SetCurrentY(3799);
                    model.SetMarginLeft(4000);

                    if (sOrgnKind == "SINGLE PROPRIETORSHIP")
                        sOrgnKind = "SINGLE PROP.";

                    model.SetTable(string.Format("^11100;{0}", sOrgnKind));
                    model.SetMarginLeft(1700);
                    
                    //payment details
                    
                    double dFeeAmt = 0;
                    pRec.Query = string.Format("select fees_due, qtr_paid from or_table where or_no = '{0}' and fees_code = 'B' and bns_code_main = '{1}'", sORNo, sBnsCode);  // RMC 20110810 modified
                    if (pRec.Execute())
                    {
                        while (pRec.Read())
                        {
                            dFeeAmt += pRec.GetDouble(0);
                            //sQtrPaid = pRec.GetString(1);
                        }

                    
                        sFeeAmt = string.Format("{0:#,##0.00}", dFeeAmt);
                    }
                    pRec.Close();

                            if (sQtrPaid == "F")
                                sQtrPaid = "Annually";
                            else
                            {
                                if (sQtrPaid == "1")
                                    sQtrPaid = "1st Qtr";
                                else if (sQtrPaid == "2")
                                    sQtrPaid = "2nd Qtr";
                                else if (sQtrPaid == "3")
                                    sQtrPaid = "3rd Qtr";
                                else
                                    sQtrPaid = "4th Qtr";
                            }
                            
                        
                    //}
                    //pRec.Close();

                    if (sQtrPaid != "Annually")
                    {
                        model.SetCurrentY(6938);
                        model.SetMarginLeft(11000);
                        model.SetFontBold(0);
                        model.SetFontSize(7);
                        sQtrRemarks = "BALANCE TO BE ";
                        model.SetTable(string.Format(">3000;{0}", sQtrRemarks));
                        sQtrRemarks = "PAID ON OR BEFORE:";
                        model.SetTable(string.Format(">3000;{0}", sQtrRemarks));

                        DateTime dtDate;
                        string sDate = "";


                        pRec.Query = "select * from due_dates where due_year = '"+m_sTaxYear+"' order by due_code ";    // RMC 20110819 added filter by due_year
                        if (pRec.Execute())
                        {
                            while(pRec.Read())
                            {
                                if(pRec.GetString("due_code") == "04" ||
                                    pRec.GetString("due_code") == "07" ||
                                    pRec.GetString("due_code") == "10")
                                {
                                    dtDate = pRec.GetDateTime("due_date");
                                    sDate = string.Format("{0:MMMM dd,yyyy}",dtDate);
                                    if(pRec.GetString("due_code") == "04" )
                                        sQtrRemarks = "2nd Qtr. " + sDate;
                                    if (pRec.GetString("due_code") == "07")
                                        sQtrRemarks = "3rd Qtr. " + sDate;
                                    if (pRec.GetString("due_code") == "10")
                                        sQtrRemarks = "4th Qtr. " + sDate;

                                    if (sQtrPaid == "2nd Qtr" && pRec.GetString("due_code") == "04")
                                    { }
                                    else if (sQtrPaid == "3rd Qtr" && pRec.GetString("due_code") == "07")
                                    { }
                                    else
                                        model.SetTable(string.Format(">3000;{0}", sQtrRemarks));
                                       
                                }
                            }
                            
                        }
                        pRec.Close();

                        model.SetMarginLeft(1000);
                    }

                    model.SetCurrentY(5150);
                    model.SetMarginLeft(1000);
                    model.SetFontSize(9);

                    // RMC 20110810 include other line of business (s)
                    string sAddlBns = "";
                    string sAddlStat = "";
                    double dAddlCap = 0;
                    double dAddlGross = 0;

                    int intC = 0;
                    pRec.Query = "select * from addl_bns where bin = '" + m_sBIN + "'";
                    if (pRec.Execute())
                    {
                        
                        while (pRec.Read())
                        {
                            intC++;
                            /*if (intC == 1)
                            {
                                model.SetCurrentY(5547);
                                model.SetParagraph("");
                                model.SetTable("<5000;Other Line/s of Business");
                            }*/
                            // RMC 20110822
                            sAddlStat = pRec.GetString("bns_stat");
                            if (sAddlStat == "NEW")
                                dAddlCap += pRec.GetDouble("capital");
                            else
                                dAddlGross += pRec.GetDouble("gross");

                            /*sAddlBns = AppSettingsManager.GetBnsDesc(pRec.GetString("bns_code_main"));
                            model.SetTable("<100|<5000;" + "|" + sAddlBns);*/
                            // RMC 20110822
                        }

                        if(sCapital != "")
                            dAddlCap += Convert.ToDouble(sCapital);
                        if (sGross != "")
                            dAddlGross += Convert.ToDouble(sGross);

                        if (dAddlCap != 0)
                            sCapital = string.Format("{0:#,##0.00}", dAddlCap);

                        if(dAddlGross != 0)
                            sGross = string.Format("{0:#,##0.00}", dAddlGross);
                    }
                    pRec.Close();
                     // RMC 20110810 include other line of business (e)
                                        
                    model.SetFontBold(1);
                    model.SetCurrentY(5149);    // RMC 20110810
                    //model.SetTable("<3000|>4000|>100|<2000|>1000|>1000|<1000;" + sBnsMainDesc + "|CAPITAL INVESTMENT:||" + sCapital + "|||" + sBnsStat);    // RMC 20110810 adjusted
                    //model.SetTable("<100|<2900|>4000|>100|<2000|>1000|>1000|<1000;" + "|" + sBnsDesc + "|GROSS SALES:||" + sGross + "|||");    // RMC 20110810 adjusted
                    //long y = model.GetCurrentY();

                    // RMC 20110822 (S)
                    // include breakdown of other line's business tax & mayor's permit
                    string sFeesCode = "";
                    string sFeeCode = "";
                    pRec.Query = string.Format("select * from tax_and_fees_table where (fees_desc = '{0}'", StringUtilities.HandleApostrophe(sFeesCode));
                    pRec.Query += " or fees_desc like '%MAYOR%')";
                    pSet.Query += " and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "'"; //MCR 20141118
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sFeesCode = pRec.GetString("fees_code");
                        }
                    }
                    pRec.Close();

                    model.SetTable("<3000|>4000|>100|<2000|>1000|>1000|<1000;" + sBnsMainDesc + "|CAPITAL INVESTMENT:||" + sCapital + "|||" + sBnsStat);    // RMC 20110810 adjusted
                    model.SetTable("<100|<2900|>4000|>100|<2000|>1000|>1000|<1000;"  + "||GROSS SALES:||" + sGross + "|||");    // RMC 20110810 adjusted
                    /*main business*/
                    model.SetTable("<100|<2900|>4000|>500|>1500|>1000|<1000;" + "|" + sBnsDesc+ "|BUSINESS TAX:||" + sFeeAmt + "||" + sQtrPaid); // RMC 20110810 adjusted
                    sFeeCode = AppSettingsManager.GetFeesDesc(sFeesCode);
                    sFeeAmt = GetFeeAmount(sORNo,sFeesCode, sBnsCode, m_sTaxYear);
                    model.SetTable("<3000|>4000|>500|>1500|>1000|<1000;" + "|" + sFeeCode + "||" + sFeeAmt + "||" + sQtrPaid);
                    /*main business*/

                    pRec.Query = string.Format("select distinct bns_code_main from or_table where or_no = '{0}' and fees_code = 'B' and bns_code_main <> '{1}' order by fees_code ", sORNo, sBnsCode);
                    if(pRec.Execute())
                    {
                        string sTempBnsCode = "";
                        string sTempBnsDesc = "";
                        dFeeAmt = 0;

                        while (pRec.Read())
                        {
                            sTempBnsCode = pRec.GetString(0);

                            sFeeAmt = GetFeeAmount(sORNo, "B", sTempBnsCode, m_sTaxYear);
                            sTempBnsDesc = AppSettingsManager.GetBnsDesc(sTempBnsCode);
                            model.SetTable("<100|<2900|>4000|>500|>1500|>1000|<1000;" + "|" + sTempBnsDesc + "|BUSINESS TAX:||" + sFeeAmt + "||" + sQtrPaid);

                            sFeeCode = AppSettingsManager.GetFeesDesc(sFeesCode);
                            sFeeAmt = GetFeeAmount(sORNo, sFeesCode, sTempBnsCode, m_sTaxYear);
                            model.SetTable("<3000|>4000|>500|>1500|>1000|<1000;" + "|" + sFeeCode + "||" + sFeeAmt + "||" + sQtrPaid);
                        }
                    }
                    pRec.Close();
                    // RMC 20110822 (E)

                    model.SetFontBold(0);

                    
                    int intCnt = 0;
                    pRec.Query = string.Format("select * from or_table where or_no = '{0}' and fees_code <> 'B' and fees_code <> '{1}' and tax_year = '{2}' order by fees_code", sORNo, sFeesCode, m_sTaxYear);  // RMC 20110822
                    if (pRec.Execute())
                    {
                        while (pRec.Read())
                        {
                            intCnt++;
                            if (intCnt > 2)
                                sQtrPaid = "";

                            sFeeCode = pRec.GetString("fees_code");
                            sFeeCode = AppSettingsManager.GetFeesDesc(sFeeCode);
                            sFeeAmt = string.Format("{0:#,##0.00}", pRec.GetDouble("fees_due"));
                            //long y = model.GetCurrentY();
                            model.SetTable("<3000|>4000|>500|>1500|>1000|<1000;" + "|" + sFeeCode + "||" + sFeeAmt + "||" + sQtrPaid);  // RMC 20110810 adjusted
                        }
                    }
                    pRec.Close();

                    string sSurch = "";
                    string sPen = "";
                    string sTotal = "";
                    string sCredit = "";

                    double dblSurch = 0;
                    double dblPen = 0;
                    double dblTotal = 0;
                    double dblCredit = 0;

                    pRec.Query = string.Format("select sum(fees_surch) from or_table where or_no = '{0}' and tax_year = '{1}'", sORNo, m_sTaxYear);
                    double.TryParse(pRec.ExecuteScalar(), out dblSurch);
                    sSurch = string.Format("{0:#,##0.00}", dblSurch);
                    model.SetTable("<3000|>4000|>1000|>1000|>1000|<1000;" + sFlrArea + "|25% Penalty||" + sSurch + "||");
                    
                    pRec.Query = string.Format("select sum(fees_pen) from or_table where or_no = '{0}' and tax_year = '{1}'", sORNo, m_sTaxYear);
                    double.TryParse(pRec.ExecuteScalar(), out dblPen);
                    sPen = string.Format("{0:#,##0.00}", dblPen);
                    model.SetTable("<3000|>4000|>1000|>1000|>1000|<1000;" + sUnitNo + "|2% Interest||" + sPen + "||");

                    // tax credit (note: value for clarification )
                    pRec.Query = string.Format("select sum(credit) from debit_credit where bin = '{0}' and or_no = '{0}'", m_sBIN, sORNo);
                    double.TryParse(pRec.ExecuteScalar(), out dblCredit);
                    sCredit = string.Format("{0:#,##0.00}", dblCredit);
                    model.SetTable("<3000|>4000|>1000|>1000|>1000|<1000;" + sRemarks + "|Tax Credits||" + sCredit + "||");

                    pRec.Query = string.Format("select sum(fees_amtdue) from or_table where or_no = '{0}' and tax_year = '{1}'", sORNo, m_sTaxYear);
                    double.TryParse(pRec.ExecuteScalar(), out dblTotal);
                    sTotal = string.Format("{0:#,##0.00}", dblTotal);

                    model.SetParagraph("");
                    model.SetFontBold(1);
                    model.SetTable("<3000|>4000|>1000|>1000|>1000|<1000;" + "|TOTAL TAX PAID:||" + sTotal + "||");
                    model.SetFontBold(0);

                    // RMC 20110822 (s)
                    if(GetNotation(sORNo))
                    {
                        model.SetCurrentY(9222);
                        model.SetMarginLeft(1000);
                        model.SetTable(string.Format("<10000;{0}", m_sNotation));
                    }
                    // RMC 20110822 (e)

                    model.SetCurrentY(9222);
                    model.SetMarginLeft(12800);
                    model.SetTable(string.Format("<3000;{0}", m_sBIN));
                    model.SetCurrentY(9622);
                    model.SetTable(string.Format("<3000;{0}", sORNo));
                    model.SetCurrentY(10022);
                    model.SetTable(string.Format("<3000;{0}", sORDate));
                }
            }

            
            if (AuditTrail.InsertTrail("ABBP", "rep_mp", "Print Permit of " + m_sBIN) == 0)
            {
                pSet.Rollback();
                pSet.Close();
                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }


        private string GetFeeAmount(string sORNo, string sFeeCode, string sBnsCode, string sTaxYear)
        {
            OracleResultSet pRec = new OracleResultSet();
            double dFeeAmt = 0;
            string sFeeAmt = "";

            pRec.Query = string.Format("select fees_due from or_table where or_no = '{0}' and fees_code = '{1}' and bns_code_main = '{2}' and tax_year = '{3}'", sORNo, sFeeCode, sBnsCode, sTaxYear);
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    dFeeAmt += pRec.GetDouble(0);
                }
                sFeeAmt = string.Format("{0:#,##0.00}", dFeeAmt);
            }
            pRec.Close();

            return sFeeAmt;

        }

        public bool GetNotation(string sORNo)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sMinYear = "";
            string sMaxYear = "";
            m_sNotation = "";

            pRec.Query = string.Format("select min(tax_year), max(tax_year) from pay_hist where or_no = '{0}' and tax_year <> '{1}' and bin = '{2}' ", sORNo, m_sTaxYear,m_sBIN);
            if(pRec.Execute())
            {
                if(pRec.Read())
                {
                    sMinYear = pRec.GetString(0);
                    sMaxYear = pRec.GetString(1);

                    pRec.Close();

                    if (sMinYear != sMaxYear)
                        m_sNotation = "Note: With payment for delinquent years: " + sMinYear + "-" + sMaxYear;
                    else if (sMinYear != "")
                        m_sNotation = "Note: With payment for delinquent year: " + sMinYear;
                    else
                        m_sNotation = "";
                    
                }
            }
            pRec.Close();


            if (m_sNotation != "")
                return true;
            
            return false;
        }
    }
}
