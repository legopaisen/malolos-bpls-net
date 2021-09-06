
// AST 20140122 BusinessQueue
using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using System.Threading;
using System.Windows.Forms;
using Amellar.Common.DynamicProgressBar;

namespace BusinessRoll
{
    class clsBusinessRoll
    {
        public static bool m_bViewOnly = false; //MCR 20140602

        public static void LoadByBarangay(frmReport frmReport, frmBusinessRoll frmBusinessRoll)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rs = new OracleResultSet();
            OracleResultSet rs2 = new OracleResultSet();

            String strUser = "";
            String strPosition = "";
            String strBrgy = frmBusinessRoll.cboBarangay.Text;
            String strStatus = frmBusinessRoll.cboStatus.Text;
            String strTaxYear = frmBusinessRoll.txtTaxYear.Text;
            Int32 iBnsCount = 0;
            String strBrgyTmp = "";

            object objData = "";
            string strBinTmp = "";
            string strBnsNm = "";
            string strBnsAddr = "";
            string strBnsCode = "";
            string strDTINo = "";
            string strOwnCode = "";
            string strOwnNm = "";
            string strOwnAddr = "";
            string strBnsDesc = "";
            string strStatusTmp = "";
            double dCapitalTmp = 0;
            double dGrossTmp = 0;
            double dTotalCap = 0;
            int iEmpCount = 0; //MCR 03062014
            DateTime dtOperated = new DateTime();
            string strEmpContact = "";
            string strPermitNo = "";
            string strPermitNoTmp = string.Empty;
            DateTime dtPermit = new DateTime();
            string strCapital = "0";
            string strGross = "0";
            bool blnHasRecord = false;
            string strHeading = AppSettingsManager.GetConfigValue("02");
            string sPrevBin = ""; //JARS 20180507

            try
            {
                strUser = AppSettingsManager.SystemUser.UserCode;
                strPosition = AppSettingsManager.SystemUser.Position;
            }
            catch
            {
                strUser = "SYS_PROG";
                strPosition = "SYSTEM PROGRAMMER";
            }

            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal;
            //frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprUser;
            frmReport.axVSPrinter1.FontName = "Arial";
            frmReport.axVSPrinter1.MarginTop = 500;
            frmReport.axVSPrinter1.MarginRight = 100;
            frmReport.axVSPrinter1.MarginLeft = 100;
            frmReport.ReportTitle = "BUSINESS ROLL BY BARANGAY";
            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618
            frmReport.axVSPrinter1.FontSize = 8.0f;

            if (strBrgy == "ALL")
                strBrgy = "%%";

            if (strStatus == "ALL")
                strStatus = "%%";

            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            frmReport.m_objThread = new Thread(frmReport.ThreadProcess);
            frmReport.m_objThread.Start();
            Thread.Sleep(500);

            // RMC 20150429 corrected reports (s)
            if (strTaxYear.Trim() == "")
            {
                rs.Query = "Select sum(a.cnt) from (select Count (*) as cnt from businesses,own_names where businesses.own_code = own_names.own_code ";
                rs.Query += string.Format("and rtrim(businesses.bns_brgy) like '{0}' and rtrim(businesses.bns_stat) like '{1}' ", strBrgy, strStatus);
            }// RMC 20150429 corrected reports (e)
            else
            {
                rs.Query = "Select sum(a.cnt) from (select Count (*) as cnt from businesses,own_names where businesses.own_code = own_names.own_code ";
                rs.Query += string.Format("and rtrim(businesses.bns_brgy) like '{0}' and rtrim(businesses.bns_stat) like '{1}' and businesses.tax_year = '{2}' ", strBrgy, strStatus, strTaxYear);
                rs.Query += "union all select Count (*) as cnt from buss_hist,own_names where buss_hist.own_code = own_names.own_code ";
                rs.Query += string.Format("and rtrim(buss_hist.bns_brgy) like '{0}' and rtrim(buss_hist.bns_stat) like '{1}' and buss_hist.tax_year = '{2}') a", strBrgy, strStatus, strTaxYear);
            }

            int.TryParse(rs.ExecuteScalar(), out intCount);

            frmReport.DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, frmReport.m_form.ProgressBarWork);

            #endregion

            result.Query = string.Format("select distinct bns_brgy from businesses where bns_brgy like '{0}' order by bns_brgy", strBrgy);
            if (result.Execute())
            {
                while (result.Read())
                {
                    blnHasRecord = false;
                    strBrgyTmp = result.GetString("bns_brgy");

                    // RMC 20150429 corrected reports (s)
                    if (strTaxYear.Trim() == "")
                    {
                        rs.Query = "select businesses.*, own_names.* from businesses,own_names where businesses.own_code = own_names.own_code ";
                        rs.Query += string.Format("and rtrim(businesses.bns_brgy) like '{0}' and rtrim(businesses.bns_stat) like '{1}' ", strBrgyTmp, strStatus);
                        //AFM 20200220 (s)
                        rs.Query += "union all select buss_hist.*, own_names.* from buss_hist,own_names where buss_hist.own_code = own_names.own_code ";
                        rs.Query += string.Format("and rtrim(buss_hist.bns_brgy) like '{0}' and rtrim(buss_hist.bns_stat) like '{1}' ", strBrgyTmp, strStatus);
                        rs.Query += " and bin not in (select bin from businesses)";
                        //include new/renewal applications
                        rs.Query += " union all select business_que.*, own_names.* from business_que,own_names where business_que.own_code = own_names.own_code ";
                        rs.Query += string.Format("and rtrim(business_que.bns_brgy) like '{0}' and rtrim(business_que.bns_stat) like '{1}' ", strBrgyTmp, strStatus);
                        rs.Query += " and business_que.bin not in (select bin from businesses)";
                        rs.Query += "  order by 1, 53 desc";
                        //AFM 20200220 (e)
                    }// RMC 20150429 corrected reports (e)
                    else
                    {
                        rs.Query = "select businesses.*, own_names.* from businesses,own_names where businesses.own_code = own_names.own_code ";
                        rs.Query += string.Format("and rtrim(businesses.bns_brgy) like '{0}' and rtrim(businesses.bns_stat) like '{1}' and businesses.tax_year = '{2}' ", strBrgyTmp, strStatus, strTaxYear);
                        rs.Query += "union all select buss_hist.*, own_names.* from buss_hist,own_names where buss_hist.own_code = own_names.own_code ";
                        rs.Query += string.Format("and rtrim(buss_hist.bns_brgy) like '{0}' and rtrim(buss_hist.bns_stat) like '{1}' and buss_hist.tax_year = '{2}'", strBrgyTmp, strStatus, strTaxYear);
                        rs.Query += string.Format(" and bin not in (select bin from businesses where tax_year = '{0}')", strTaxYear);    // RMC 20170227 correction in Business Roll
                        //AFM 20200220 include new/renewal applications(s)
                        rs.Query += "union all select business_que.*, own_names.* from business_que,own_names where business_que.own_code = own_names.own_code ";
                        rs.Query += string.Format("and rtrim(business_que.bns_brgy) like '{0}' and rtrim(business_que.bns_stat) like '{1}' and business_que.tax_year = '{2}'", strBrgyTmp, strStatus, strTaxYear);
                        rs.Query += string.Format(" and business_que.bin not in (select bin from businesses where tax_year = '{0}')", strTaxYear);
                        rs.Query += "  order by 1, 53 desc";
                        //AFM 20200220 include new/renewal applications(e)
                    }

                    if (rs.Execute())
                    {
                        while (rs.Read())
                        {
                            blnHasRecord = true;
                            frmReport.axVSPrinter1.Paragraph = "";
                            frmReport.axVSPrinter1.FontBold = true;
                            //frmReport.axVSPrinter1.Table = String.Format("<18300;{0}", "Barangay: " + strBrgyTmp); // JHMN 20170309 OLD 19300
                            frmReport.axVSPrinter1.Table = String.Format("<19300;{0}", "Barangay: " + strBrgyTmp); // AFM 20200218
                            frmReport.axVSPrinter1.FontBold = false;
                            break;
                        }
                    }
                    rs.Close();

                    if (blnHasRecord)
                    {
                        if (rs.Execute())
                        {
                            while (rs.Read())
                            {
                                frmReport.DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, frmReport.m_form.ProgressBarWork);
                                intCountIncreament += 1;

                                objData = "";
                                strBinTmp = rs.GetString("bin");
                                if (strBinTmp == sPrevBin)
                                    continue;
                                strBnsNm = rs.GetString("Bns_nm") + ".";
                                strBnsAddr = rs.GetString("bns_house_no") + " ";
                                strBnsAddr += rs.GetString("bns_street") + " ";
                                strBnsAddr += rs.GetString("bns_mun") + " ";
                                strBnsCode = rs.GetString("Bns_code");
                                strOwnCode = rs.GetString("own_code");
                                strOwnNm = rs.GetString("own_fn") + " ";
                                strOwnNm += rs.GetString("own_mi") + " ";
                                strOwnNm += rs.GetString("own_ln") + ".";
                                strOwnAddr = rs.GetString("own_house_no");
                                strOwnAddr += rs.GetString("own_street");
                                strOwnAddr += rs.GetString("own_mun");
                                strStatusTmp = rs.GetString("bns_stat");
                                dCapitalTmp = rs.GetDouble("capital");
                                dGrossTmp = rs.GetDouble("gr_1");
                                dGrossTmp += rs.GetDouble("gr_2");
                                dtOperated = rs.GetDateTime("dt_operated");
                                strEmpContact = rs.GetString("bns_telno");
                                iEmpCount = rs.GetInt("num_employees"); // MCR 03062014
                                strPermitNo = rs.GetString("permit_no");
                                strDTINo = rs.GetString("dti_reg_no");
                                //dtPermit = rs.GetDateTime("permit_dt");

                                // JAV 20170825 (s)
                                string strPermitDt = "";
                                if (rs.GetString("permit_no") == " ")
                                    strPermitDt = " ";
                                else
                                {
                                    dtPermit = rs.GetDateTime("permit_dt");
                                    strPermitDt = string.Format("{0:MM/dd/yyyy}", dtPermit);
                                }
                                // JAV 20170825 (e)

                                //Load Additional
                                rs2.Query = string.Format("select gross,capital from addl_bns where bin = '{0}' and tax_year = '{1}'", strBinTmp, strTaxYear);
                                if (rs2.Execute())
                                {
                                    while (rs2.Read())
                                    {
                                        dCapitalTmp += rs2.GetDouble("capital");
                                        dGrossTmp += rs2.GetDouble("gross");
                                    }
                                }
                                rs2.Close();

                                //rs2.Query = string.Format("select bns_desc from bns_table where fees_code = 'B' and bns_code = '{0}' and rev_year = '1993'", strBnsCode);
                                rs2.Query = string.Format("select bns_desc from bns_table where fees_code = 'B' and bns_code = '{0}' and rev_year = '"+AppSettingsManager.GetConfigValue("07") + "' ", strBnsCode);   // RMC 20150429 corrected reports
                                
                                if (rs2.Execute())
                                {
                                    while (rs2.Read())
                                    {
                                        strBnsDesc = rs2.GetString("bns_desc");
                                    }
                                }
                                rs2.Close();


                                // JAV 20170829 (s)
                                string strBnsDescription = string.Empty;
                                string strOtherBns = string.Empty;
                                rs2.Query = string.Format("select bns_code_main from addl_bns where bin = '{0}' and tax_year = '{1}' order by bns_code_main", strBinTmp, strTaxYear);
                                if (rs2.Execute())
                                {
                                    while (rs2.Read())
                                    {
                                        strBnsDescription = rs2.GetString("bns_code_main");
                                        
                                        OracleResultSet rs3 = new OracleResultSet();
                                        rs3.Query = string.Format("select * from bns_table where fees_code = 'B' and bns_code ='{0}' and rev_year = {1}", strBnsDescription, AppSettingsManager.GetConfigValue("07"));
                                        if (rs3.Execute())
                                        {
                                            while (rs3.Read())
                                            {
                                                strOtherBns += rs3.GetString("bns_desc") + "/";
                                            }
                                        }
                                        rs3.Close();
                                    }
                                }
                                rs2.Close();
                                // JAV 20170829 (e) 
                                if(strBinTmp != sPrevBin) //JARS 20180507
                                {
                                    strCapital = string.Format("{0:#,###.00}", dCapitalTmp);
                                    strGross = string.Format("{0:#,###.00}", dGrossTmp);

                                    strBnsAddr = AppSettingsManager.GetBnsAddress(strBinTmp); // RMC 20171122 added printing of business address in Business Roll by Brgy

                                    //frmReport.axVSPrinter1.Table = string.Format("^1200|<2750|<2750|<2000|^1000|>1600|>1600|^1100|^1500|^1250|^1250;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}",
                                    //    strBinTmp, strBnsNm, strOwnNm, strBnsDesc, strStatusTmp, strCapital, strGross, dtOperated.ToShortDateString(), strEmpContact, strPermitNo, dtPermit.ToShortDateString());
                                    //frmReport.axVSPrinter1.Table = string.Format("^1200|<2750|<2750|<2000|^1000|>1600|>1600|^1100|^1500|^1250|^1250;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}",
                                    //    strBinTmp, strBnsNm, strOwnNm, strBnsDesc, strStatusTmp, strCapital, strGross, dtOperated.ToShortDateString(), strEmpContact, strPermitNo, strPermitDt);
                                  //  frmReport.axVSPrinter1.Table = string.Format("^1200|<2200|<2200|<2000|<1500|^1000|>1600|>1600|^1100|^1100|^1200|^1200|^1200;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}",
                                   //     strBinTmp, strBnsNm , strBnsAddr, strOwnNm, strBnsDesc + " / " + strOtherBns, strDTINo, strStatusTmp, strCapital, strGross, dtOperated.ToShortDateString(), strEmpContact, strPermitNo, strPermitDt);   // RMC 20171122 added printing of business address in Business Roll by Brgy

                                    //frmReport.axVSPrinter1.Table = string.Format("^1200|^2000|^2000|^2000|^2000|<1500|^1000|^1600|^1600|^1100|^1200|^1250|^1250;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}",
                                       //strBinTmp, strBnsNm, strBnsAddr, strOwnNm, strBnsDesc + " / " + strOtherBns, strDTINo, strStatusTmp, strCapital, strGross, dtOperated.ToShortDateString(), strEmpContact, strPermitNo, strPermitDt); // jhb 20190424 revise as per LGU malolos request


                                    frmReport.axVSPrinter1.Table = string.Format("^1200|^1500|^2000|^1500|^1500|<1500|^1300|^1000|^1600|^1600|^1100|^1200|^1250|^1250|^500;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}",
                                       strBinTmp, strBnsNm, strBnsAddr, strOwnNm, strOwnAddr, strBnsDesc + " / " + strOtherBns, strDTINo, strStatusTmp, strCapital, strGross, dtOperated.ToShortDateString(), strEmpContact, strPermitNo, strPermitDt, iEmpCount.ToString()); //AFM 20200218 MAO-20-12359


                                    iBnsCount++;
                                    dTotalCap += dCapitalTmp; //mjbb 20180710 for total capital 

                                    if (m_bViewOnly == false) //MCR 20140602
                                        frmBusinessRoll.UpdateBnsRollReport(strHeading, "", strBrgy, strBinTmp, strBnsNm, strBnsAddr,
                                        strOwnNm, strOwnAddr, strBnsDesc, strStatus, strCapital, strGross,
                                        dtOperated.ToShortDateString(), strPermitNo, dtPermit.ToShortDateString(),
                                        "0", "0", frmReport.Text, strUser, strPosition, strEmpContact, iEmpCount);

                                    frmReport.DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, frmReport.m_form.ProgressBarWork);
                                    Thread.Sleep(3);
                                }
                                sPrevBin = rs.GetString("bin");
                                strPermitNoTmp = rs.GetString("permit_no");
                            }
                        }
                        rs.Close();
                    }
                    dTotalCap = dTotalCap;
                }


                frmReport.DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, frmReport.m_form.ProgressBarWork);
                Thread.Sleep(10);

                frmReport.axVSPrinter1.Paragraph = "";
                frmReport.axVSPrinter1.FontBold = true;
                //frmReport.axVSPrinter1.Table = string.Format("<19300;{0}{1}", "Total No. of Businesses: ", iBnsCount);
                //frmReport.axVSPrinter1.Table = string.Format("<19000;{0}{1}", "Total No. of Businesses: ", iBnsCount);
                frmReport.axVSPrinter1.Table = string.Format("<10000|<9000;{0}{1}|{2}{3}", "Total No. of Businesses: ", iBnsCount, "Total Capital: ", string.Format("{0:#,###.00}", dTotalCap.ToString())); //mjbb 20180710 added display of total capital 
                frmReport.axVSPrinter1.FontBold = false;

            }
            result.Close();

            frmBusinessRoll.UpdateGenerateInfo();

            frmReport.axVSPrinter1.EndDoc();
        }

        public static void LoadByDistrict(frmReport frmReport, frmBusinessRoll frmBusinessRoll)
        {
            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            //frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; // AST 20160126
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprUser;
            frmReport.axVSPrinter1.FontName = "Arial";
            frmReport.axVSPrinter1.MarginLeft = 500;
            frmReport.axVSPrinter1.MarginRight = 500;
            frmReport.axVSPrinter1.MarginTop = 500;
            frmReport.axVSPrinter1.MarginBottom = 500;

            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618
            frmReport.axVSPrinter1.FontSize = 12.0f;


            frmReport.axVSPrinter1.EndDoc();
        }

        public static void LoadByLineOfBusiness(frmReport frmReport, frmBusinessRoll frmBusinessRoll)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rs = new OracleResultSet();
            OracleResultSet rs2 = new OracleResultSet();

            String strHeading = AppSettingsManager.GetConfigValue("02");
            String strUser = "";
            String strPos = "";
            String strBrgy = frmBusinessRoll.cboBarangay.Text;
            String strBnsCode = frmBusinessRoll.BusinessCode[frmBusinessRoll.cboBussType.SelectedIndex];
            String strBnsNatureCode = frmBusinessRoll.NatureCode[frmBusinessRoll.cboBussNature.SelectedIndex];
            String strStatus = frmBusinessRoll.cboStatus.Text;
            String strTaxYear = frmBusinessRoll.txtTaxYear.Text;
            String strEmpContact = string.Empty;    // RMC 20171124 added contact no. in Business Roll by Line of Business

            object objData = "";
            string strBin = "";
            string strBnsNm = "";
            string strBnsAddr = "";
            string strOwnNm = "";
            string strOwnAddr = "";
            string strBnsDesc = "";
            string strStatusTmp = "";
            string strAddlTaxYear = "";
            string strDTINo = "";
            double dGross = 0;
            double dCapital = 0;
            DateTime dtOperated = new DateTime();
            DateTime dtPermit = new DateTime();
            string strPermitNo = "";
            string strCapital = "0";
            string strGross = "0";
            int iBnsCount = 0;
            int iEmpCnt = 0;
            string sBinTmp = string.Empty;
            string sPermitNoTmp = string.Empty;

            if (strBrgy == "ALL")
                strBrgy = "%%";

            if (strBnsCode == "ALL" || strBnsCode == "")
                strBnsCode = "%%";

            if (strStatus == "ALL")
                strStatus = "%%";

            if (strBnsNatureCode != "")
                strBnsCode = strBnsNatureCode;

            try
            {
                strUser = AppSettingsManager.SystemUser.UserCode;
                strPos = AppSettingsManager.SystemUser.Position;
            }
            catch
            {
                strUser = "SYS_PROG";
                strPos = "SYSTEM PROGRAMMER";
            }

            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            frmReport.m_objThread = new Thread(frmReport.ThreadProcess);
            frmReport.m_objThread.Start();
            Thread.Sleep(500);

            rs.Query = "select sum(a.cnt) from (select Count (*) as cnt from businesses,own_names ";
            rs.Query += string.Format("where businesses.own_code = own_names.own_code and rtrim(bns_code) like '{0}' ", strBnsCode);
            rs.Query += string.Format("and bns_brgy like '{0}' and rtrim(bns_stat) like '{1}' ", strBrgy, strStatus);

            if (strTaxYear != "")
                rs.Query += string.Format("and businesses.tax_year = '{0}' ", strTaxYear);

            rs.Query += "union all select Count (*) as cnt from buss_hist,own_names ";
            rs.Query += "where buss_hist.own_code = own_names.own_code ";
            rs.Query += string.Format("and rtrim(buss_hist.bns_code) like '{0}' and buss_hist.bns_brgy like '{1}' ", strBnsCode, strBrgy);
            rs.Query += string.Format("and rtrim(buss_hist.bns_stat) like '{0}'", strStatus);

            if (strTaxYear != "")
                rs.Query += string.Format("and buss_hist.tax_year = '{0}' ", strTaxYear);

            rs.Query += ") a";
            int.TryParse(rs.ExecuteScalar(), out intCount);

            frmReport.DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, frmReport.m_form.ProgressBarWork);

            #endregion


            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; // AST 20160126
            //frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprUser;
            frmReport.axVSPrinter1.FontName = "Arial";
            frmReport.axVSPrinter1.MarginTop = 500;
            frmReport.ReportTitle = "BUSINESS ROLL BY MAIN BUSINESS";
            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618

            result.Query = string.Format("select distinct bns_code from businesses where rtrim(bns_code) like '{0}%' order by bns_code", strBnsCode);
            if (result.Execute())
            {
                while (result.Read())
                {
                    strBnsCode = result.GetString("bns_code"); //MCR 20140710

                    rs.Query = "select distinct businesses.*, own_names.* from businesses,own_names ";
                    rs.Query += string.Format("where businesses.own_code = own_names.own_code and rtrim(bns_code) like '{0}' ", strBnsCode);
                    rs.Query += string.Format("and bns_brgy like '{0}' and rtrim(bns_stat) like '{1}' ", strBrgy, strStatus);

                    if (strTaxYear != "")
                    {// RMC 20150429 corrected reports
                        rs.Query += string.Format("and businesses.tax_year = '{0}' ", strTaxYear);
                    }   // RMC 20171124 corrected no printed data if no tax year indicated in Business Roll by Line of Business

                        /*//GMC 20151023(s)
                        rs.Query += "union select buss_hist.*, own_names.* from buss_hist,own_names ";
                        rs.Query += "where buss_hist.own_code = own_names.own_code ";
                        rs.Query += string.Format("and rtrim(buss_hist.bns_code) like '{0}' and buss_hist.bns_brgy like '{1}' ", strBnsCode, strBrgy);
                        rs.Query += string.Format("and rtrim(buss_hist.bns_stat) like '{0}' ", strStatus);

                        if (strTaxYear != "")
                            rs.Query += string.Format("and buss_hist.tax_year = '{0}' ", strTaxYear);*/
                        ///GMC 20151023(e) 
                        ///
                        // RMC 20170227 correction in Business Roll (s)
                        rs.Query += " union all select distinct buss_hist.*, own_names.* from buss_hist,own_names ";
                        rs.Query += string.Format("where buss_hist.own_code = own_names.own_code and rtrim(bns_code) like '{0}' ", strBnsCode);
                        rs.Query += string.Format("and bns_brgy like '{0}' and rtrim(bns_stat) like '{1}' ", strBrgy, strStatus);
                        if (strTaxYear != "")   // RMC 20171124 corrected no printed data if no tax year indicated in Business Roll by Line of Business
                            rs.Query += string.Format("and buss_hist.tax_year = '{0}' ", strTaxYear);
                        rs.Query += string.Format(" and bin not in (select bin from businesses where tax_year = '{0}')", strTaxYear);
                        // RMC 20170227 correction in Business Roll (e)
                        //AFM 20200220 include new/renewal applications(s)
                        rs.Query += " union all select distinct business_que.*, own_names.* from business_que,own_names ";
                        rs.Query += string.Format("where business_que.own_code = own_names.own_code and rtrim(bns_code) like '{0}' ", strBnsCode);
                        rs.Query += string.Format("and bns_brgy like '{0}' and rtrim(bns_stat) like '{1}' ", strBrgy, strStatus);
                        if (strTaxYear != "")   // RMC 20171124 corrected no printed data if no tax year indicated in Business Roll by Line of Business
                            rs.Query += string.Format("and business_que.tax_year = '{0}' ", strTaxYear);
                        rs.Query += string.Format(" and bin not in (select bin from businesses where tax_year = '{0}')", strTaxYear);
                        rs.Query += "order by 1, 53 desc";
                        //AFM 20200220 include new/renewal applications(e)

                        //}// RMC 20150429 corrected reports
                        
                        if (rs.Execute())
                        {
                            while (rs.Read())
                            {
                                frmReport.DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, frmReport.m_form.ProgressBarWork);
                                intCountIncreament += 1;

                                strBin = rs.GetString("bin");
                                strBnsNm = rs.GetString("bns_nm");
                                if (strBin == sBinTmp) //AFM 20200221 skip duplicate data with blank permit no in buss_hist table
                                    continue;
                                /*strBnsAddr = rs.GetString("bns_house_no") + " ";
                                strBnsAddr += rs.GetString("bns_street") + " ";
                                strBnsAddr += rs.GetString("bns_mun");*/
                                strBnsAddr = AppSettingsManager.GetBnsAddress(strBin); // RMC 20171122 added printing of business address in Business Roll by Brgy

                                strOwnNm = rs.GetString("own_fn") + " ";
                                strOwnNm += rs.GetString("own_mi") + " ";
                                strOwnNm += rs.GetString("own_ln");

                                strOwnAddr = rs.GetString("own_house_no") + " ";
                                strOwnAddr += rs.GetString("own_street") + " ";
                                strOwnAddr += rs.GetString("own_mun");

                                strStatusTmp = rs.GetString("bns_stat");
                                dCapital = rs.GetDouble("capital");
                                dGross = rs.GetDouble("gr_1") + rs.GetDouble("gr_2");
                                dtOperated = rs.GetDateTime("dt_operated");
                                strEmpContact = rs.GetString("bns_telno");  // RMC 20171124 added contact no. in Business Roll by Line of Business
                                strAddlTaxYear = rs.GetString("tax_year");

                                strDTINo = rs.GetString("dti_reg_no");

                                iEmpCnt = rs.GetInt("num_employees");
                                
                                //dtPermit = rs.GetDateTime("permit_dt");
                                strPermitNo = rs.GetString("permit_no");
                                // JAV 20170825 (s)
                                string strPermitDt = "";
                                //AFM 20200217 changed condition (s)
                                //if (rs.GetString("permit_no") == " ")
                                    //strPermitDt = " ";
                                if (string.IsNullOrEmpty(rs.GetString("permit_no")) || rs.GetString("permit_no").Contains(" "))
                                    strPermitDt = "";
                                //AFM 20200217 changed condition (e)
                                else
                                {
                                    dtPermit = rs.GetDateTime("permit_dt");
                                    strPermitDt = string.Format("{0:MM/dd/yyyy}", dtPermit);
                                }
                                // JAV 20170825 (e)
                                 
                                strCapital = string.Format("{0:#,###.00}", dCapital);
                                strGross = string.Format("{0:#,###.00}", dGross);

                                string strAddlCapital = string.Empty;
                                string strAddlGross = string.Empty;
                                double dAddlCapital = 0;
                                double dAddlGross = 0;
                                rs2.Query = string.Format("select capital, gross from addl_bns where bin = '{0}' and tax_year = '{1}' order by bns_code_main", strBin, strTaxYear);
                                if (rs2.Execute())
                                {
                                    while (rs2.Read())
                                    {
                                        dAddlCapital = rs2.GetDouble("capital");
                                        dAddlGross = rs2.GetDouble("gross");
                                        strAddlCapital = string.Format("{0:#,###.00}", dAddlCapital);
                                        strAddlGross = string.Format("{0:#,###.00}", dAddlGross);
                                    }
                                }
                                rs2.Close();

                                //rs2.Query = string.Format("select bns_desc from bns_table where fees_code = 'B' and bns_code = '{0}' and rev_year = '1993'", strBnsCode);
                                rs2.Query = string.Format("select bns_desc from bns_table where fees_code = 'B' and bns_code = '{0}' and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "' ", strBnsCode);   // RMC 20150429 corrected reports
                                if (rs2.Execute())
                                {
                                    while (rs2.Read())
                                    {
                                        strBnsDesc = rs2.GetString("bns_desc");
                                    }
                                }
                                rs2.Close();

                                // JAV 20170829 (s)
                                string strBnsDescription = string.Empty;
                                string strOtherBns = string.Empty;
                               
                                rs2.Query = string.Format("select * from addl_bns where bin = '{0}' and tax_year = '{1}' order by bns_code_main", strBin, strTaxYear);
                                if (rs2.Execute())
                                {
                                    while (rs2.Read())
                                    {
                                        strBnsDescription = rs2.GetString("bns_code_main");
                                        OracleResultSet rs3 = new OracleResultSet();
                                        rs3.Query = string.Format("select * from bns_table where fees_code = 'B' and bns_code ='{0}' and rev_year = {1}", strBnsDescription, AppSettingsManager.GetConfigValue("07"));
                                        if (rs3.Execute())
                                        {
                                            while (rs3.Read())
                                            {
                                                strOtherBns += rs3.GetString("bns_desc") + "/";
                                            }
                                        }
                                        rs3.Close();
                                    }
                                }
                                rs2.Close();


                                
                                // JAV 20170829 (e) 

                                //AFM 20200217 MAO-20-12339 removed query block (s)
                                //rs2.Query = "select bns_stat from buss_hist where bns_code='" + strBnsCode + "'";//GMC 20151023 Print New Business Status if in Buss_hist(s)
                                //if (rs2.Execute())
                                    //if (rs2.Read())
                                        //strStatusTmp = rs2.GetString(0).ToString();
                                //rs2.Close();//GMC 20151023 Print New Business Status if in Buss_hist(e)
                                //AFM 20200217 MAO-20-12339 removed query block (e)

                                //frmReport.axVSPrinter1.Table = String.Format("^2000|<2300|<2300|<2000|^1000|>1600|>1600|^1600|^1250|^1250;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                                // strBin, strBnsNm + strBnsAddr, strOwnNm + strOwnAddr, strBnsDesc, strStatusTmp, strCapital, strGross, dtOperated.ToShortDateString(), strPermitNo, dtPermit.ToShortDateString());
                                //frmReport.axVSPrinter1.Table = String.Format("^2000|<2300|<2300|<2000|^1000|>1600|>1600|^1600|^1250|^1250;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                                // strBin, strBnsNm + strBnsAddr, strOwnNm + strOwnAddr, strBnsDesc ,strStatusTmp, strCapital, strGross, dtOperated.ToShortDateString(), strPermitNo, strPermitDt);
                                /*frmReport.axVSPrinter1.Table = String.Format("^2000|<2200|<2200|<2000|^1000|<1400|>1600|>1600|^1600|^1250|^1250;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}",
                                    strBin, strBnsNm + strBnsAddr, strOwnNm + strOwnAddr, strBnsDesc + " / " + strOtherBns, strDTINo, strStatusTmp, strCapital + " / " + strAddlCapital, strGross + " / " + strAddlGross, dtOperated.ToShortDateString(), strPermitNo, strPermitDt);
                                */
                                // RMC 20171124 added contact no. in Business Roll by Line of Business (s)
                               // frmReport.axVSPrinter1.Table = string.Format("^1200|<2200|<2200|<2000|<1500|^1000|>1600|>1600|^1100|^1100|^1250|^1250;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}",
                               //     strBin, strBnsNm + strBnsAddr, strOwnNm + strOwnAddr, strBnsDesc + " / " + strOtherBns, strDTINo, strStatusTmp, strCapital + " / " + strAddlCapital, strGross + " / " + strAddlGross, dtOperated.ToShortDateString(), strEmpContact, strPermitNo, strPermitDt);
                                // RMC 20171124 added contact no. in Business Roll by Line of Business (e)
                                //frmReport.axVSPrinter1.Table = string.Format("^1200|^2000|^2000|^2000|^2000|<1500|^1000|^1600|^1600|^1100|^1200|^1250|^1250;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}",
                                       //strBin, strBnsNm, strBnsAddr, strOwnNm, strBnsDesc + " / " + strOtherBns, strDTINo, strStatusTmp, strCapital, strGross, dtOperated.ToShortDateString(), strEmpContact, strPermitNo, strPermitDt);   // jhb 20190424 revise as per LGU malolos request

                                frmReport.axVSPrinter1.Table = string.Format("^1200|^1500|^2000|^1500|<1500|^2000|^1000|^800|^1600|^1600|^1100|^1200|^1000|^1000|^500;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}",
                                       strBin, strBnsNm, strBnsAddr, strOwnNm, strOwnAddr,strBnsDesc + " / " + strOtherBns, strDTINo, strStatusTmp, strCapital, strGross, dtOperated.ToShortDateString(), strEmpContact, strPermitNo, strPermitDt, iEmpCnt.ToString());   // AFM 20200218 MAO-20-12359 added column for own address and no. of employees
                                //System.Threading.Thread.Sleep(1000); //MCR 20140618

                                if (m_bViewOnly == false) //MCR 20140602
                                    frmBusinessRoll.UpdateBnsRollReport(strHeading, strBrgy, strBnsDesc, strBin, strBnsNm, strBnsAddr,
                               strOwnNm, strOwnAddr, strBnsDesc, strStatus, strCapital, strGross,
                               dtOperated.ToShortDateString(), strPermitNo, dtPermit.ToShortDateString(),
                              // "0", "0", frmReport.Text, strUser, strPos, "", 0);
                              "0", "0", frmReport.Text, strUser, strPos, strEmpContact, 0); // RMC 20171124 added contact no. in Business Roll by Line of Business

                                iBnsCount++;
                                sBinTmp = rs.GetString("bin");
                                sPermitNoTmp = rs.GetString("permit_no");

                                frmReport.DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, frmReport.m_form.ProgressBarWork);
                                Thread.Sleep(3);
                            }
                        }
                        rs.Close();
                   // } // RMC 20171124 corrected no printed data if no tax year indicated in Business Roll by Line of Business
                }
            }
            result.Close();

            frmReport.DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, frmReport.m_form.ProgressBarWork);
            Thread.Sleep(10);

            frmReport.axVSPrinter1.Paragraph = "";
            frmReport.axVSPrinter1.FontBold = true;
            frmReport.axVSPrinter1.Table = string.Format("<17300;{0}{1}", "Total No. of Businesses: ", iBnsCount);
            frmReport.axVSPrinter1.FontBold = false;

            frmBusinessRoll.UpdateGenerateInfo();

            

            frmReport.axVSPrinter1.EndDoc();
        }

        public static void LoadByStreet(frmReport frmReport, frmBusinessRoll frmBusinessRoll)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rs = new OracleResultSet();
            OracleResultSet rs2 = new OracleResultSet();

            String strHeading = AppSettingsManager.GetConfigValue("02");
            String strUser = "";
            String strPos = "";
            String strBrgyTmp = "";
            String strBnsCode = frmBusinessRoll.BusinessCode[frmBusinessRoll.cboBussType.SelectedIndex];
            String strBnsNatureCode = frmBusinessRoll.NatureCode[frmBusinessRoll.cboBussNature.SelectedIndex];
            String strStreet = StringUtilities.HandleApostrophe(frmBusinessRoll.cboStreet.Text);
            String strTaxYear = frmBusinessRoll.txtTaxYear.Text;


            string strBIN = "";
            string strBnsCodeTmp = "";
            string strBnsNm = "";
            string strBnsAddr = "";
            string strOwnNm = "";
            string strOwnAddr = "";
            string strBnsDesc = "";
            string strStatusTmp = "";
            string strCapital = "0";
            string strGross = "0";
            string strPermitNo = "";
            double dCapital = 0;
            double dGross = 0;
            int iEmpCnt = 0;
            DateTime dtOperated = new DateTime();
            DateTime dtPermit = new DateTime();
            int iBnsCount = 0;
            bool blnHasRecord = false;

            try
            {
                strUser = AppSettingsManager.SystemUser.UserCode;
                strPos = AppSettingsManager.SystemUser.Position;
            }
            catch
            {
                strUser = "SYS_PROG";
                strPos = "SYSTEM PROGRAMMER";
            }

            if (strStreet == "" || strStreet == "ALL")
                strStreet = "%%";

            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            frmReport.m_objThread = new Thread(frmReport.ThreadProcess);
            frmReport.m_objThread.Start();
            Thread.Sleep(500);

            rs.Query = "select Count(*) from businesses,own_names where businesses.own_code = own_names.own_code ";
            rs.Query += string.Format("and businesses.bns_brgy like '%%' and rtrim(bns_stat) like '%%' and businesses.bns_street like '{0}' ", strStreet);
            rs.Query += "order by businesses.bns_street, own_names.own_ln, own_names.own_fn, own_names.own_mi, businesses.bns_nm ";

            int.TryParse(rs.ExecuteScalar(), out intCount);

            frmReport.DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, frmReport.m_form.ProgressBarWork);
            #endregion

            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; // AST 20160126
            //frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprUser;
            frmReport.axVSPrinter1.FontName = "Arial";
            frmReport.axVSPrinter1.MarginTop = 500;
            frmReport.ReportTitle = "BUSINESS ROLL BY STREET";
            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618

            result.Query = "select distinct bns_brgy from businesses where bns_brgy like '%%' order by bns_brgy";
            if (result.Execute())
            {
                while (result.Read())
                {
                    strBrgyTmp = result.GetString("bns_brgy");
                    blnHasRecord = false;

                    rs.Query = "select businesses.*, own_names.* from businesses,own_names where businesses.own_code = own_names.own_code ";
                    //rs.Query += string.Format("and bns_brgy = '{0}' and rtrim(bns_stat) like '%%' and bns_street like '{1}' ", strBrgyTmp, strStreet);
                    rs.Query += string.Format("and bns_brgy = '{0}' and rtrim(bns_stat) like '%%' and (rtrim(businesses.bns_street) like '{1}' or trim(businesses.bns_street) is null) ", strBrgyTmp, strStreet);   // RMC 20170227 correction in Business Roll
                    // RMC 20150429 corrected reports
                    //if (strTaxYear.Trim() != "") //AFM 20200221 removed condition since tax year is default disabled
                    //{
                        //rs.Query += string.Format("and businesses.tax_year = '{0}' ", strTaxYear);
                        rs.Query += " union all ";
                        rs.Query += "select buss_hist.*, own_names.* from buss_hist,own_names where buss_hist.own_code = own_names.own_code ";
                        //rs.Query += string.Format("and buss_hist.bns_brgy = '{0}' and rtrim(buss_hist.bns_stat) like '%%' and buss_hist.bns_street like '{1}' ", strBrgyTmp, strStreet);
                        rs.Query += string.Format("and buss_hist.bns_brgy = '{0}' and rtrim(buss_hist.bns_stat) like '%%' and (rtrim(buss_hist.bns_street) like '{1}' or trim(buss_hist.bns_street) is null) ", strBrgyTmp, strStreet);    // RMC 20170227 correction in Business Roll
                        //rs.Query += string.Format("and buss_hist.tax_year = '{0}' ", strTaxYear);
                        //rs.Query += string.Format(" and bin not in (select bin from businesses where tax_year = '{0}')", strTaxYear);    // RMC 20170227 correction in Business Roll
                        rs.Query += string.Format(" and bin not in (select bin from businesses) ");    // RMC 20170227 correction in Business Roll
                        //AFM 20200221 (s)
                        rs.Query += " union all ";
                        rs.Query += "select business_que.*, own_names.* from business_que,own_names where business_que.own_code = own_names.own_code ";
                        //rs.Query += string.Format("and buss_hist.bns_brgy = '{0}' and rtrim(buss_hist.bns_stat) like '%%' and buss_hist.bns_street like '{1}' ", strBrgyTmp, strStreet);
                        rs.Query += string.Format("and business_que.bns_brgy = '{0}' and rtrim(business_que.bns_stat) like '%%' and (rtrim(business_que.bns_street) like '{1}' or trim(business_que.bns_street) is null) ", strBrgyTmp, strStreet);    // RMC 20170227 correction in Business Roll
                        //rs.Query += string.Format("and business_que.tax_year = '{0}' ", strTaxYear);
                        rs.Query += string.Format(" and bin not in (select bin from businesses)" );   
                        rs.Query += " order by 7,1, 53 desc";
                        //AFM 20200221 (e)

                    //}
                    // RMC 20150429 corrected reports
                    //rs.Query += "order by bns_street, own_ln, own_fn, own_mi, bns_nm ";

                    if (rs.Execute())
                    {
                        if (rs.Read())
                        {
                            frmReport.axVSPrinter1.FontBold = true;
                            frmReport.axVSPrinter1.Table = String.Format("<17300;{0}", "Barangay: " + strBrgyTmp);
                            frmReport.axVSPrinter1.FontBold = false;
                            blnHasRecord = true;
                        }
                    }

                    if (blnHasRecord)
                    {
                        if (rs.Execute())
                        {
                            while (rs.Read())
                            {
                                frmReport.DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, frmReport.m_form.ProgressBarWork);
                                intCountIncreament += 1;

                                strBIN = rs.GetString("bin");

                                strBnsCodeTmp = rs.GetString("bns_code");

                                strBnsNm = rs.GetString("bns_nm");
                                strBnsAddr = rs.GetString("bns_house_no");
                                strBnsAddr += rs.GetString("bns_street");
                                strBnsAddr += rs.GetString("bns_mun");

                                strOwnNm = rs.GetString("own_fn");
                                strOwnNm += rs.GetString("own_mi");
                                strOwnNm += rs.GetString("own_ln");
                                strOwnAddr = rs.GetString("own_house_no");
                                strOwnAddr += rs.GetString("own_street");
                                strOwnAddr += rs.GetString("own_mun");

                                strStatusTmp = rs.GetString("bns_stat");

                                dCapital = rs.GetDouble("capital");

                                dGross = rs.GetDouble("gr_1");
                                dGross += rs.GetDouble("gr_2");

                                strCapital = string.Format("{0:#,###.00}", dCapital);
                                strGross = string.Format("{0:#,###.00}", dGross);

                                dtOperated = rs.GetDateTime("dt_operated");

                                strPermitNo = rs.GetString("permit_no");

                                dtPermit = rs.GetDateTime("permit_dt");

                                iEmpCnt = rs.GetInt("num_employees");


                                //rs2.Query = string.Format("select bns_desc from bns_table where fees_code = 'B' and bns_code = '{0}' and rev_year = '1993'", strBnsCodeTmp);
                                rs2.Query = string.Format("select bns_desc from bns_table where fees_code = 'B' and bns_code = '{0}' and rev_year = '" +AppSettingsManager.GetConfigValue("07") + "'", strBnsCodeTmp);    // RMC 20150429 corrected reports
                                if (rs2.Execute())
                                {
                                    while (rs2.Read())
                                    {
                                        strBnsDesc = rs2.GetString("bns_desc");
                                    }
                                }
                                rs2.Close();

                              //  frmReport.axVSPrinter1.Table = String.Format("^2000|<2500|<2500|<2000|^1000|>1600|>1600|>1600|^1250|^1250;{0}|{1}|{2}|{3}|{4}|{5:#,###.00}|{6:#,###.00}|{7}|{8}|{9}|",
                               //     strBIN, strBnsNm + strBnsAddr, strOwnNm + strOwnAddr, strBnsDesc, strStatusTmp, strCapital, strGross, dtOperated.ToShortDateString(), strPermitNo, dtPermit.ToShortDateString());
                                frmReport.axVSPrinter1.Table = String.Format("^2000|<2000|<2000|<1500|<1500|<1500|^1000|>1500|>1500|>1500|^1250|^1250|^500;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7:#,###.00}|{8:#,###.00}|{9}|{10}|{11}|{12}", //AFM 20200218 MAO-20-12359
                                    strBIN, strBnsNm , strBnsAddr, strOwnNm, strOwnAddr, strBnsDesc, strStatusTmp, strCapital, strGross, dtOperated.ToShortDateString(), strPermitNo, dtPermit.ToShortDateString(), iEmpCnt.ToString());

                                if (m_bViewOnly == false) //MCR 20140602
                                    frmBusinessRoll.UpdateBnsRollReport(strHeading, "", strBnsDesc, strBIN, strBnsNm, strBnsAddr,
                               strOwnNm, strOwnAddr, strBnsDesc, strStatusTmp, strCapital, strGross,
                               dtOperated.ToShortDateString(), strPermitNo, dtPermit.ToShortDateString(),
                               "0", "0", frmReport.Text, strUser, strPos, "", 0);

                                iBnsCount++;
                                frmReport.DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, frmReport.m_form.ProgressBarWork);
                                Thread.Sleep(3);
                            }
                        }
                        rs.Close();
                    }
                }
            }
            result.Close();


            frmReport.DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, frmReport.m_form.ProgressBarWork);
            Thread.Sleep(10);

            frmBusinessRoll.UpdateGenerateInfo();

            frmReport.axVSPrinter1.Paragraph = "";
            frmReport.axVSPrinter1.FontBold = true;
            frmReport.axVSPrinter1.Table = string.Format("<17300;{0}{1}", "Total No. of Businesses: ", iBnsCount);
            frmReport.axVSPrinter1.FontBold = false;

            //String strBnsType = frmBusinessRoll.cboBussType.Text;
            //String strBnsNature = frmBusinessRoll.cboBussNature.Text;
            //String strStreet = frmBusinessRoll.cboStreet.Text.Trim();
            //List<String> lstBrgy = new List<String>(Businesses.BusinessBrgy("ALL"));

            //String strBnsCode = frmBusinessRoll.BusinessCode[frmBusinessRoll.cboBussNature.SelectedIndex];
            //List<String> lstBnsCode = new List<String>(Businesses.BnsCodeList(strBnsCode));
            //String strStatus = "ALL";
            //Int32 iBnsCount = 0;



            //frmReport.axVSPrinter1.StartDoc();
            //for (int i = 0; i != lstBrgy.Count; i++)
            //{
            //    if (lstBrgy[i] == "ALL")
            //        continue;

            //    Businesses businesses = new Businesses();
            //    businesses.LoadBusinesses(lstBrgy[i], strStatus, "", "", strStreet);

            //    if (businesses.HasRecord)
            //    {
            //        if (strStreet == "ALL")
            //        {
            //            frmReport.axVSPrinter1.Paragraph = "";
            //            frmReport.axVSPrinter1.FontBold = true;
            //            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
            //            frmReport.axVSPrinter1.Table = String.Format("<17300;{0}", "Barangay: " + lstBrgy[i]);
            //            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            //            frmReport.axVSPrinter1.FontBold = false;
            //        }
            //        else
            //        {
            //            frmReport.axVSPrinter1.Paragraph = "";
            //            frmReport.axVSPrinter1.FontBold = true;
            //            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
            //            frmReport.axVSPrinter1.Table = String.Format("<17300;{0}", "Street Name: " + strStreet);
            //            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            //            frmReport.axVSPrinter1.FontBold = false;
            //        }
            //    }

            //    for (int k = 0; k != businesses.BIN.Count; k++)
            //    {
            //        iBnsCount++;
            //        Object objData = "";

            //        objData = businesses.BIN[k] + "|";
            //        objData += businesses.BusinessName[k] + "|";
            //        objData += businesses.OwnerName[k] + "|";
            //        objData += businesses.BusinessDesc[k] + "|";
            //        objData += businesses.Status[k] + "|";
            //        objData += businesses.Capital[k] + "|";
            //        objData += businesses.Gross[k] + "|";
            //        objData += businesses.DateOperated[k].ToShortDateString() + "|";
            //        objData += businesses.PermitNo[k] + "|";
            //        objData += businesses.PermitDate[k].ToShortDateString();

            //        frmReport.axVSPrinter1.Table = String.Format("^2000|<2500|<2500|<2000|^1000|>1600|>1600|>1600|^1250|^1250;{0}", objData);

            //        frmBusinessRoll.UpdateBnsRollReport(strHeading, "", lstBrgy[i], businesses.BIN[k], businesses.BusinessName[k], "",
            //            "", "", businesses.BusinessDesc[k], strStatus, businesses.Capital[k].ToString(), businesses.Gross[k].ToString(),
            //            businesses.DateOperated[k].ToShortDateString(), businesses.PermitNo[k], businesses.PermitDate[k].ToShortDateString(),
            //            "0", "0", frmReport.Text, strUser, strPos, "", "");

            //        frmBusinessRoll.UpdateGenerateInfo();

            //    }
            //}

            //frmReport.axVSPrinter1.Paragraph = "";
            //frmReport.axVSPrinter1.FontBold = true;
            //frmReport.axVSPrinter1.Table = string.Format("<17300;{0}{1}", "Total No. of Businesses: ", iBnsCount);
            //frmReport.axVSPrinter1.FontBold = false;

            frmReport.axVSPrinter1.EndDoc();
        }

        public static void LoadByTopGrosses(frmReport frmReport, frmBusinessRoll frmBusinessRoll)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rs = new OracleResultSet();
            OracleResultSet rs1 = new OracleResultSet();

            String strHeading = AppSettingsManager.GetConfigValue("02");
            String strBrgy = frmBusinessRoll.cboBarangay.Text;
            String strBnsType = frmBusinessRoll.cboBussType.Text;
            String strBnsCode = frmBusinessRoll.BusinessCode[frmBusinessRoll.cboBussType.SelectedIndex];
            String strBnsNatureCode = frmBusinessRoll.NatureCode[frmBusinessRoll.cboBussNature.SelectedIndex];
            String strGross = frmBusinessRoll.txtGross.Text;
            String strTaxYear = frmBusinessRoll.txtTaxYear.Text;
            String strUser = "";
            String strPosition = "";

            string strReportTitle = "";
            int iYear = AppSettingsManager.GetCurrentDate().Year;
            string strBIN = "";
            string strBnsCodeTmp = "";
            string strBnsNm = "";
            string strBnsAddr = "";
            string strOwnNm = "";
            string strOwnAddr = "";
            string strBnsDesc = "";
            string strGrossTmp = "0";
            string strPermitNo = "";
            string strORNo = "";
            string strDTINo = ""; //JAV 20170823 ADD DTI No
            string strQtrPaid = "";
            string strTaxPaid = "";
            string strTopTmp = "0";
            string strDTIRegDt = "";
            double dGross = 0;
            double dTaxPaid = 0;
            DateTime dtOperated = new DateTime();
            DateTime dtPermit = new DateTime();
            DateTime dtDTIRegDt = new DateTime();
            int iBnsCount = 0;
            int iEmpCount = 0;

            try
            {
                strUser = AppSettingsManager.SystemUser.UserCode;
                strPosition = AppSettingsManager.SystemUser.Position;
            }
            catch
            {
                strUser = "SYS_PROG";
                strPosition = "SYSTEM PROGRAMMER";
            }

            if (strBrgy == "ALL")
                strBrgy = "%%";

            if (strBnsCode == "ALL" || strBnsCode == "")
                strBnsCode = "%%";

            if (strBnsNatureCode != "")
                strBnsCode = strBnsNatureCode;

            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; // AST 20160126 rem
            //frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprUser;
            frmReport.axVSPrinter1.FontName = "Arial";
            frmReport.axVSPrinter1.MarginTop = 500;

            if (frmBusinessRoll.chkByPercentage.Checked)
                strTopTmp = strGross + "%GR";
            else
                strTopTmp = strGross + "GR";

            int iRecordCnt = 0; //MCR 20140804

            result.Query = string.Format("delete from top_grosspayer_tbl where tax_year = '{0}' and top = '{1}' ", strTaxYear, strTopTmp);
            if (result.ExecuteNonQuery() == 0) { }

            // RMC 20150429 corrected reports (s)
            if (strTaxYear.Trim() == "")
            {
                // AFM 20200218 added num_employees
                result.Query = "select a.bin,bns_nm,bns_code,own_code,sum(distinct gr_1) + sum(gross) as gr,gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no, a.num_employees ";  // RMC 20150806 corrected sorting of business roll top gross, added alias 'gr' 
                result.Query += string.Format("from businesses a, addl_bns b  where rtrim(bns_code) like '{0}' and bns_brgy like '{1}' ", strBnsCode, strBrgy);
                result.Query += "and a.bns_stat = 'REN' and a.bin = b.bin and a.tax_year = b.tax_year and a.bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";
                result.Query += "group by a.bin,bns_nm,bns_code,own_code,gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no, a.num_employees ";

                // RMC 20150806 corrected sorting of business roll top gross (s)
                // this will get all businesses that does not have addl bns, since code above only gets businesses w/ addl bns
                result.Query += "union ";
                result.Query += "select bin,bns_nm,bns_code,own_code,gr_1,gr_1,gr_2, ";
                result.Query += "dt_operated,permit_no,permit_dt,or_no, num_employees from businesses where bin not in (select bin from addl_bns) ";
                result.Query += "and rtrim(bns_code) like '" + strBnsCode + "' and bns_brgy like '" + strBnsCode + "' ";
                result.Query += "and bns_stat = 'REN' and bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";
                // RMC 20150806 corrected sorting of business roll top gross (e)
                result.Query += "order by 5 desc";
            }// RMC 20150429 corrected reports (e)
            else
            {
                //result.Query = "select a.bin,bns_nm,bns_code,own_code,sum(distinct gr_1) + sum(gross) as gr,gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no ";   // RMC 20150806 corrected sorting of business roll top gross, added alias 'gr' 
                //result.Query += string.Format("from businesses a, addl_bns b  where rtrim(bns_code) like '{0}' and bns_brgy like '{1}' and a.tax_year = '{2}' ", strBnsCode, strBrgy, strTaxYear);
                //result.Query += "and a.bns_stat = 'REN' and a.bin = b.bin and a.tax_year = b.tax_year and a.bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";
                //result.Query += "group by a.bin,bns_nm,bns_code,own_code,gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no ";
                //result.Query += "union select a.bin,bns_nm,bns_code,own_code,sum(distinct gr_1) + sum(gross),gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no from buss_hist a ,addl_bns b ";
                //result.Query += string.Format("where rtrim(bns_code) like '{0}' and bns_brgy like '{1}' and a.tax_year = '{2}' and a.bns_stat = 'REN' and a.bin = b.bin and a.tax_year = b.tax_year ", strBnsCode, strBrgy, strTaxYear);
                //result.Query += "and a.bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";
                //result.Query += "group by a.bin,bns_nm,bns_code,own_code,gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no ";

                // AFM 20200218 added num_employees in whole query block
                result.Query = "select distinct a.bin,bns_nm,bns_code,own_code,sum(distinct gr_1) + sum(gross) as gr,gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no,dti_reg_no,dti_reg_dt, a.num_employees ";   
                result.Query += string.Format("from businesses a, addl_bns b  where rtrim(bns_code) like '{0}' and bns_brgy like '{1}' and a.tax_year = '{2}' ", strBnsCode, strBrgy, strTaxYear);
                result.Query += "and a.bns_stat = 'REN' and a.bin = b.bin and a.tax_year = b.tax_year and a.bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";
                result.Query += "group by a.bin,bns_nm,bns_code,own_code,gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no,dti_reg_no,dti_reg_dt, a.num_employees ";
                result.Query += "union select distinct a.bin,bns_nm,bns_code,own_code,sum(distinct gr_1) + sum(gross),gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no,dti_reg_no,dti_reg_dt, a.num_employees from buss_hist a ,addl_bns b ";
                result.Query += string.Format("where rtrim(bns_code) like '{0}' and bns_brgy like '{1}' and a.tax_year = '{2}' and a.bns_stat = 'REN' and a.bin = b.bin and a.tax_year = b.tax_year ", strBnsCode, strBrgy, strTaxYear);
                result.Query += "and a.bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";
                // JAV 20170823
                result.Query += string.Format("and a.bin not in (select bin from businesses where tax_year = '{0}') ",strTaxYear);
                result.Query += string.Format("and a.bin NOT IN (SELECT bin FROM buss_hist WHERE tax_year = '{0}' having count(bin) > 1 group by bin) ", strTaxYear);

                result.Query += "group by a.bin,bns_nm,bns_code,own_code,gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no,dti_reg_no,dti_reg_dt, a.num_employees ";
                //AFM 20200221 (s)
                result.Query += " union select distinct a.bin,bns_nm,bns_code,own_code,sum(distinct gr_1) + sum(gross),gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no,dti_reg_no,dti_reg_dt, a.num_employees from business_que a ,addl_bns b ";
                result.Query += string.Format("where rtrim(bns_code) like '{0}' and bns_brgy like '{1}' and a.tax_year = '{2}' and a.bns_stat = 'REN' and a.bin = b.bin and a.tax_year = b.tax_year ", strBnsCode, strBrgy, strTaxYear);
                result.Query += "and a.bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";
                result.Query += string.Format("and a.bin not in (select bin from businesses where tax_year = '{0}') ", strTaxYear);
                result.Query += string.Format("and a.bin NOT IN (SELECT bin FROM buss_hist WHERE tax_year = '{0}' having count(bin) > 1 group by bin) ", strTaxYear);
                result.Query += "group by a.bin,bns_nm,bns_code,own_code,gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no,dti_reg_no,dti_reg_dt, a.num_employees ";
                //AFM 20200221 (s)


                // RMC 20150806 corrected sorting of business roll top gross (s)
                // this will get all businesses that does not have addl bns, since code above only gets businesses w/ addl bns
                result.Query += "union ";
                result.Query += "select bin,bns_nm,bns_code,own_code,gr_1,gr_1,gr_2, ";
                result.Query += "dt_operated,permit_no,permit_dt,or_no,dti_reg_no,dti_reg_dt, num_employees from businesses where bin not in (select bin from addl_bns where tax_year = '" + strTaxYear + "') and tax_year = '" + strTaxYear + "' ";
                result.Query += "and bns_stat = 'REN' and bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";
                result.Query += "and rtrim(bns_code) like '" + strBnsCode + "' and bns_brgy like '" + strBnsCode + "' ";
                result.Query += "union ";
                result.Query += "select bin,bns_nm,bns_code,own_code,gr_1,gr_1,gr_2, ";
                result.Query += "dt_operated,permit_no,permit_dt,or_no,dti_reg_no,dti_reg_dt, num_employees from buss_hist where bin not in (select bin from addl_bns  where tax_year = '" + strTaxYear + "') and tax_year = '" + strTaxYear + "' ";
                result.Query += "and bns_stat = 'REN' and bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";
                result.Query += "and rtrim(bns_code) like '" + strBnsCode + "' and bns_brgy like '" + strBnsCode + "' ";
                //RMC 20150806 corrected sorting of business roll top gross (e)
                // JAV 20170823
                result.Query += string.Format("and bin not in (select bin from businesses where tax_year = '{0}') ", strTaxYear);
                result.Query += string.Format("and bin in (SELECT bin FROM buss_hist WHERE tax_year = '{0}' having count(bin) > 1 group by bin) ", strTaxYear);
                result.Query += "and permit_dt is not null ";
                // JAV 20170823
                result.Query += "order by 5 desc ";
            }
            /* REM MCR 20140804
            if (result.Execute())
            {
                while (result.Read())
                {
                    iRecordCnt++;
                }
            }
            result.Close();
            */
            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            frmReport.m_objThread = new Thread(frmReport.ThreadProcess);
            frmReport.m_objThread.Start();
            Thread.Sleep(500);
            intCount = Convert.ToInt32(strGross);
            frmReport.DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, frmReport.m_form.ProgressBarWork);
            #endregion

            if (frmBusinessRoll.chkByPercentage.Checked)
            {
                float fGrossTmp = float.Parse(strGross);
                float fGross = iBnsCount * fGrossTmp / 100;
                strGross = string.Format("{0:###}", fGross);
            }

            frmReport.BusinessType = strBnsType;
            frmReport.Gross = strGross;
            frmReport.TaxYear = strTaxYear;
            if (frmBusinessRoll.chkByPercentage.Checked)
                strReportTitle = "BUSINESS ROLL BY TOP " + strGross + "%" + " GROSSES FOR " + iYear;
            else
                strReportTitle = "BUSINESS ROLL BY TOP " + strGross + "" + " GROSSES FOR " + iYear;

            frmReport.ReportTitle = strReportTitle;
            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618
            if (result.Execute())
            {
                while (result.Read())
                {
                    frmReport.DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, frmReport.m_form.ProgressBarWork);
                    intCountIncreament += 1;
                    strBIN = result.GetString("bin");
                    strBnsNm = result.GetString("bns_nm");
                    strBnsAddr = AppSettingsManager.GetBnsAdd(strBIN, "");

                    strOwnNm = AppSettingsManager.GetBnsOwner(result.GetString("own_code"));
                    strOwnAddr = AppSettingsManager.GetBnsOwnAdd(result.GetString("own_code"));

                    iEmpCount = result.GetInt("num_employees");

                    /*dGross = result.GetDouble("gr_1");
                    dGross += result.GetDouble("gr_2");*/
                    // RMC 20150806 corrected sorting of business roll top gross, put rem

                    dtOperated = result.GetDateTime("dt_operated");

                    strPermitNo = result.GetString("permit_no");
                    //dtPermit = result.GetDateTime("permit_dt");
                    
                    // JAV 20170824 (s)
                    string strPermitDt = "";
                    //dtPermit = result.GetDateTime("permit_dt");
                    //strPermitDt = string.Format("{0:dd/MM/yyyy}", dtPermit);
                    if (result.GetString("permit_no") == " " )
                        strPermitDt = "";
                    else
                    {
                        dtPermit = result.GetDateTime("permit_dt");
                        strPermitDt = string.Format("{0:dd/MM/yyyy}", dtPermit);
                    }
                    strDTINo = result.GetString("dti_reg_no");
                    //dtDTIRegDt = result.GetDateTime("dti_reg_dt");
                    //strDTIRegDt = string.Format("{0:dd/MM/yyyy}", dtDTIRegDt);
                    // JAV 20170824 (e)

                    /*rs.Query = string.Format("select sum(gross) as gr_addl from addl_bns where bin = '{0}' and tax_year = '{1}' ", strBnsDesc, strTaxYear);
                    if (rs.Execute())
                    {
                        while (rs.Read())
                        {
                            dGross += rs.GetDouble("gr_addl");
                        }
                    }   
                    rs.Close();*/
                    // RMC 20150806 corrected sorting of business roll top gross, put rem

                    dGross = result.GetDouble("gr");   // RMC 20150806 corrected sorting of business roll top gross

                    strGrossTmp = string.Format("{0:#,###.00}", dGross);

                    strBnsCodeTmp = result.GetString("bns_code");

                    

                    //rs.Query = string.Format("select bns_desc from bns_table where fees_code = 'B' and bns_code = '{0}' and rev_year = '1993' ", strBnsCodeTmp);
                    rs.Query = string.Format("select bns_desc from bns_table where fees_code = 'B' and bns_code = '{0}' and rev_year = '"+AppSettingsManager.GetConfigValue("07") + "' ", strBnsCodeTmp);    // RMC 20150429 corrected reports
                    if (rs.Execute())
                    {
                        while (rs.Read())
                        {
                            strBnsDesc = rs.GetString("bns_desc");
                        }
                    }
                    rs.Close();
                    
                    dTaxPaid = 0; //MCR 20170519
                    rs.Query = string.Format("select distinct or_no, qtr_paid from pay_hist where bin = '{0}' and tax_year = '{1}' ", strBIN, strTaxYear);
                    rs.Query += "order by qtr_paid";
                    if (rs.Execute())
                    {
                        while (rs.Read())
                        {
                            strORNo = rs.GetString("or_no");
                            strQtrPaid = rs.GetString("qtr_paid");

                            //MCR 20170519 transfer here (s)
                            rs1.Query = string.Format("select sum(fees_amtdue) as BtaxAmtDue from or_table where or_no = '{0}'", strORNo);
                            if (rs1.Execute())
                            {
                                while (rs1.Read())
                                    dTaxPaid += rs1.GetDouble("BtaxAmtDue");
                            }
                            rs1.Close();
                            //MCR 20170519 transfer here (e)
                        }
                    }
                    rs.Close();

                    strTaxPaid = string.Format("{0:#,###.00}", dTaxPaid);

                    //frmReport.axVSPrinter1.Table = string.Format("<2500|<2500|<2500|<2500|>2000|^1500|^1500|^1500|>1300|^700;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                    //    strBIN, strBnsNm + strBnsAddr, strOwnNm + strOwnAddr, strBnsDesc, strGrossTmp, dtOperated.ToShortDateString(), strPermitNo, dtPermit.ToShortDateString(), strTaxPaid, strQtrPaid);
                    if (strGrossTmp != ".00") //JARS 20170824
                    {
                        //frmReport.axVSPrinter1.Table = string.Format("<2000|<2500|<2500|<2500|^1000|>2000|^1500|^1500|^1500|>1300|^700;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}",
                            //strBIN, strBnsNm + strBnsAddr, strOwnNm + strOwnAddr, strBnsDesc, strDTINo + "/" + strDTIRegDt, strGrossTmp, dtOperated.ToShortDateString(), strPermitNo, strPermitDt, strTaxPaid, strQtrPaid);
                        //AFM 20200218
                        frmReport.axVSPrinter1.Table = string.Format("<1500|<1500|<1500|<1500|<1500|<1500|^1000|>2000|^1500|^1500|^1500|>1300|^700|^500;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}",
                            strBIN, strBnsNm, strBnsAddr, strOwnNm, strOwnAddr, strBnsDesc, strDTINo + "/" + strDTIRegDt, strGrossTmp, dtOperated.ToShortDateString(), strPermitNo, strPermitDt, strTaxPaid, strQtrPaid, iEmpCount.ToString());
                    }


                    rs.Query = string.Format("insert into top_grosspayer_tbl values('232-00-2013-0001093','2013','10%GR','SYS_PROG',to_date('2014-01-17 13:46:45','YYYY-MM-DD HH24:MI:SS'))",
                        strBIN, strTaxYear, strTopTmp, strUser, AppSettingsManager.GetCurrentDate().ToShortDateString());
                    if (rs.ExecuteNonQuery() == 0) { }
                    if (m_bViewOnly == false) //MCR 20140602
                        frmBusinessRoll.UpdateBnsRollReport(strHeading, "", strBrgy, strBIN, strBnsNm, strBnsAddr,
                               strOwnNm, strOwnAddr, strBnsDesc, "REN", "0", dGross.ToString(),
                               dtOperated.ToShortDateString(), strPermitNo, dtPermit.ToShortDateString(),
                               dTaxPaid.ToString(), strQtrPaid, frmReport.Text, strUser, strPosition, "", 0);

                    iBnsCount++;

                    if (iBnsCount.ToString() == strGross) //MCR 20140804
                        break;

                    frmReport.DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, frmReport.m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
            }
            result.Close();


            frmReport.DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, frmReport.m_form.ProgressBarWork);
            Thread.Sleep(10);

            frmBusinessRoll.UpdateGenerateInfo();

            frmReport.axVSPrinter1.Paragraph = "";
            frmReport.axVSPrinter1.FontBold = true;
            frmReport.axVSPrinter1.Table = string.Format("<18500;{0}", "Total No. of Businesses: " + strGross);
            frmReport.axVSPrinter1.FontBold = false;

            frmReport.axVSPrinter1.EndDoc();
        }

        public static void LoadByTopPayers(frmReport frmReport, frmBusinessRoll frmBusinessRoll)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rs = new OracleResultSet();

            string strHeading = "";
            string strUser = "";
            string strPosition = "";
            string strReportTitle = "";
            string strTmpPR = "";
            string strBnsBrgy = frmBusinessRoll.cboBarangay.Text;
            string strBnsType = frmBusinessRoll.cboBussType.Text;
            string strBnsCode = frmBusinessRoll.BusinessCode[frmBusinessRoll.cboBussType.SelectedIndex];
            string strTaxYear = frmBusinessRoll.txtTaxYear.Text;
            string strGross = frmBusinessRoll.txtGross.Text;
            string strCurrentDate = AppSettingsManager.GetCurrentDate().ToShortDateString();
            int iYear = AppSettingsManager.GetCurrentDate().Year;
            double dRecordCnt = 0;
            string strBnsName = "";
            string strBnsAddr = "";
            string strOwnName = "";
            string strOwnAddr = "";
            string strOwnCode = "";
            string strBnsDesc = "";
            string strBnsOrgn = "";
            double gr1 = 0;
            double gr2 = 0;
            double dGrossReceipt = 0;
            double dCapital = 0;
            double dBTaxAmtDue = 0;
            int iTotalBns = 0;
            string strTopGrossTmp = "0";
            string sCurrBin = "";
            string sPrevBin = "";
            int iEmpCount = 0;

            try
            {
                strUser = AppSettingsManager.SystemUser.UserCode;
                strPosition = AppSettingsManager.SystemUser.Position;
            }
            catch
            {
                strUser = "SYS_PROG";
                strPosition = "SYSTEM PROGRAMMER";
            }

            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; // AST 20160126 rem
            //frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprUser;
            frmReport.axVSPrinter1.FontName = "Arial";
            frmReport.axVSPrinter1.MarginTop = 500;

            if (frmBusinessRoll.chkByPercentage.Checked)
                strReportTitle = "BUSINESS ROLL BY TOP PAYERS" + " FOR " + iYear;
            else
                strReportTitle = "BUSINESS ROLL BY TOP PAYERS" + " FOR " + iYear;

            frmReport.ReportTitle = strReportTitle;


            result.Query = string.Format("delete from rep_bnsrol_top where report_name = '{0}' and user_code = '{1}'", strReportTitle, strUser);
            result.ExecuteNonQuery();

            result.Query = string.Format("delete from gen_info where report_name = '{0}'", strReportTitle);
            result.ExecuteNonQuery();

            if (frmBusinessRoll.chkByPercentage.Checked)
                strTmpPR = strGross + "%PR";
            else
                strTmpPR = strGross + "PR";

            result.Query = string.Format("delete from top_grosspayer_tbl where tax_year = '{0}' and top = '{1}'", strTaxYear, strTmpPR);
            result.ExecuteNonQuery();

            result.Query = string.Format("insert into gen_info values('{0}',to_date('{1}', 'MM/dd/yyyy'), '{2}',{3},'{4}')", strReportTitle, strCurrentDate, strUser, 1, "ASS");
            result.ExecuteNonQuery();

            if (strBnsBrgy == "" || strBnsBrgy == "ALL")
                strBnsBrgy = "%%";

            if (strBnsCode == "")
                strBnsCode = "%%";
            else
                strBnsCode = strBnsCode + "%";

            // RMC 20150429 corrected reports
            if (strTaxYear.Trim() == "")
            {
                //AFM 20200218 added num_employees
                result.Query = "select businesses.bin,sum(or_table.fees_amtdue) as amt_paid, bns_nm,orgn_kind, ";
                result.Query += "businesses.bns_stat,bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,businesses.or_no, businesses.num_employees ";
                result.Query += "from businesses, pay_hist, or_table ";
                result.Query += "where businesses.bin = pay_hist.bin and pay_hist.or_no = or_table.or_no ";
                result.Query += "and trim(businesses.bns_code) like '" + strBnsCode + "' and trim(businesses.bns_brgy) like '" + strBnsBrgy + "' ";
                result.Query += "and businesses.tax_year = pay_hist.tax_year and pay_hist.bns_stat = businesses.bns_stat ";
                result.Query += "group by businesses.bin, bns_nm, businesses.bns_stat,bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,businesses.or_no,orgn_kind, businesses.num_employees ";
                             
                result.Query += "order by amt_paid desc";
            }// RMC 20150429 corrected reports 
            else
            {

                result.Query = "select businesses.bin,sum(or_table.fees_amtdue) as amt_paid, bns_nm,orgn_kind, ";
                result.Query += "businesses.bns_stat,bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,businesses.or_no,businesses.num_employees ";
                result.Query += "from businesses, pay_hist, or_table ";
                result.Query += "where businesses.bin = pay_hist.bin and pay_hist.or_no = or_table.or_no ";
                result.Query += "and trim(businesses.bns_code) like '" + strBnsCode + "' and trim(businesses.bns_brgy) like '" + strBnsBrgy + "' ";
                result.Query += "and businesses.tax_year = '" + strTaxYear + "' and pay_hist.tax_year = '" + strTaxYear + "' and pay_hist.bns_stat = businesses.bns_stat ";
                result.Query += "group by businesses.bin, bns_nm, businesses.bns_stat,bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,businesses.or_no,orgn_kind,businesses.num_employees ";

                result.Query += "union select buss_hist.bin,sum(or_table.fees_amtdue) as amt_paid, bns_nm,orgn_kind, buss_hist.bns_stat, bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,buss_hist.or_no,buss_hist.num_employees ";
                result.Query += "from buss_hist, pay_hist, or_table where buss_hist.bin = pay_hist.bin and pay_hist.or_no = or_table.or_no ";
                result.Query += "and trim(buss_hist.bns_code) like '" + strBnsCode + "' and trim(buss_hist.bns_brgy) like '" + strBnsBrgy + "' and buss_hist.tax_year = '" + strTaxYear + "' ";
                result.Query += "and pay_hist.tax_year = '" + strTaxYear + "' and pay_hist.bns_stat = buss_hist.bns_stat ";
                result.Query += "group by buss_hist.bin, bns_nm, buss_hist.bns_stat, bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,buss_hist.or_no,orgn_kind,buss_hist.num_employees ";
                result.Query += "order by amt_paid desc";
            }
            /* REM MCR 20140804
            if (result.Execute())
            {
                while (result.Read())
                {
                    dRecordCnt++;
                }
            }
            result.Close();
            */
            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            frmReport.m_objThread = new Thread(frmReport.ThreadProcess);
            frmReport.m_objThread.Start();
            Thread.Sleep(500);
            intCount = Convert.ToInt32(strGross);

            frmReport.DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, frmReport.m_form.ProgressBarWork);
            #endregion

            if (frmBusinessRoll.chkByPercentage.Checked)
            {
                double dTmpTopGross = double.Parse(strGross);
                double dPercent = (dRecordCnt * dTmpTopGross) / 100;
                strTopGrossTmp = string.Format("{0:###}", dPercent);
            }
            else
                strTopGrossTmp = strGross;

            frmReport.BusinessType = strBnsType;
            frmReport.Gross = strTopGrossTmp;
            frmReport.TaxYear = strTaxYear;

            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618

            frmReport.axVSPrinter1.FontSize = 8.0f;

            frmReport.axVSPrinter1.Paragraph = "";
            frmReport.axVSPrinter1.FontBold = true;
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
            //frmReport.axVSPrinter1.Table = String.Format("<17300;{0}", "Barangay: " + frmBusinessRoll.cboBarangay.Text);
            frmReport.axVSPrinter1.Table = String.Format("<18300;{0}", "Barangay: " + frmBusinessRoll.cboBarangay.Text); //AFM 20200218
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            frmReport.axVSPrinter1.FontBold = false;

            if (result.Execute())
            {
                while (result.Read())
                {
                    sCurrBin = result.GetString("bin"); //JARS 20170714
                    if(sCurrBin != sPrevBin) //JARS 201707
                    {
                        frmReport.DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, frmReport.m_form.ProgressBarWork);
                        intCountIncreament += 1;
                        object objData = "";
                        string strGrossCapital = ""; // JHMN 20170309 added
                        strBnsName = result.GetString("bns_nm");
                        //strBnsAddr = AppSettingsManager.GetBnsDesc(result.GetString("bns_code"));
                        strBnsAddr = AppSettingsManager.GetBnsAddress(result.GetString("bin")); //AFM 20200218
                        strOwnCode = result.GetString("own_code");
                        strOwnName = AppSettingsManager.GetBnsOwner(strOwnCode);
                        strOwnAddr = AppSettingsManager.GetBnsOwnAdd(strOwnCode);
                        strBnsDesc = AppSettingsManager.GetBnsDesc(result.GetString("bns_code"));
                        strBnsOrgn = result.GetString("orgn_kind");
                        //gr1 = result.GetDouble("gr_1");
                        //gr2 = result.GetDouble("gr_2");
                        //dGrossReceipt = gr1 + gr2;
                        strGrossCapital = AppSettingsManager.GetCapitalGross(result.GetString("bin"), result.GetString("bns_code"), strTaxYear, result.GetString("bns_stat")).ToString(); // JHMN 20170309 to get latest gross declaration
                        dGrossReceipt = Convert.ToDouble(strGrossCapital);
                        dCapital = result.GetDouble("capital");
                        iEmpCount = result.GetInt("num_employees");

                        

                        rs.Query = string.Format(@"select sum(fees_amtdue) as BtaxAmtDue from or_table where or_no in 
                        (select or_no from pay_hist where bin = '{0}' and tax_year = '{1}')",
                            result.GetString("bin"), strTaxYear);
                        if (rs.Execute())
                        {
                            while (rs.Read())
                            {
                                dBTaxAmtDue = rs.GetDouble("BtaxAmtDue");
                            }
                        }
                        rs.Close();

                        // RMC 20150806 added mode of payment & no. of qtrs paid in Business Roll Top Payers report (s)
                        string sMode = string.Empty;
                        string sNoQtrsPaid = string.Empty;
                        int iTmp = 0;
                        int iMode = 0;
                        int iNoQtsPaid = 0;

                        rs.Query = "select qtr_paid, no_of_qtr from pay_hist where bin = '" + result.GetString("bin") + "' and ";
                        rs.Query += " tax_year = '" + strTaxYear + "' and qtr_paid <> 'X' order by qtr_paid desc, no_of_qtr desc";
                        if (rs.Execute())
                        {
                            if (rs.Read())
                            {
                                sMode = rs.GetString("qtr_paid");
                                sNoQtrsPaid = rs.GetString("no_of_qtr");

                                if (sMode == "F")
                                    sNoQtrsPaid = "4";
                                else
                                {
                                    // RMC 20170227 correction in Business Roll (s)
                                    if (sNoQtrsPaid == "")
                                    {
                                        sNoQtrsPaid = sMode; //MOD MCR 20170519 1 to sMode
                                        sMode = "Q";
                                    }
                                    else// RMC 20170227 correction in Business Roll (e)
                                    {
                                        int.TryParse(sMode, out iMode);
                                        int.TryParse(sNoQtrsPaid, out iNoQtsPaid);
                                        iTmp = iMode + iNoQtsPaid - 1;
                                        if (iTmp < 0)
                                            iTmp = 1;

                                        sMode = "Q";
                                        sNoQtrsPaid = string.Format("{0:###}", iTmp);
                                    }
                                }
                            }
                        }
                        rs.Close();
                        // RMC 20150806 added mode of payment & no. of qtrs paid in Business Roll Top Payers report (e)

                        objData = result.GetString("bin") + "|";
                        //objData += strBnsName + "/" + strBnsAddr + "|";
                        objData += strBnsName + "|";
                        objData += strBnsAddr + "|";
                        //objData += strOwnName + "/" + strOwnAddr + "|";
                        objData += strOwnName + "|";
                        objData += strOwnAddr + "|";
                        objData += strBnsDesc + "/" + "REN" + "/" + strBnsOrgn + "|";
                        objData += string.Format("{0:#,###.00}\n{1:#,###.00}", dGrossReceipt, dCapital) + "|";
                        objData += result.GetDateTime("dt_operated").ToShortDateString() + "|";
                        objData += result.GetString("permit_no") + "|";
                        objData += result.GetDateTime("permit_dt").ToShortDateString() + "|";
                        objData += string.Format("{0:#,###.00}", dBTaxAmtDue) + "|";
                        objData += iEmpCount.ToString(); //AFM 20200218
                        //<2500|<3000|<3000|<3000|>2000|^1500|^1500|^1500|>1000
                        //frmReport.axVSPrinter1.Table = string.Format("<2500|<3000|<3000|<3000|>1900|^1200|^1400|^1300|>1700;;{0}", objData);

                        // RMC 20150806 added mode of payment & no. of qtrs paid in Business Roll Top Payers report (s) 
                        //objData += "|" + sMode + "|" + sNoQtrsPaid;
                        //objData += "|" + sMode + "|" + sNoQtrsPaid;
                        //frmReport.axVSPrinter1.Table = string.Format("<1250|<2500|<2500|<2500|>1900|^1200|^1400|^1300|>1500|^700|^500;;{0}", objData);
                        frmReport.axVSPrinter1.Table = string.Format("<2000|<1500|<1500|<1500|<1500|<2500|>1900|^1200|^1400|^1300|>1500|^500;;{0}", objData);
                        
                        // RMC 20150806 added mode of payment & no. of qtrs paid in Business Roll Top Payers report (e)

                        if (m_bViewOnly == false) //MCR 20140602
                            frmBusinessRoll.UpdateBnsRollReport(strHeading, "", strBnsBrgy, result.GetString("bin"), strBnsName, strBnsAddr,
                                  strOwnName, strOwnAddr, strBnsDesc, "REN", "0", strGross,
                                   result.GetDateTime("dt_operated").ToShortDateString(), result.GetString("permit_no"), result.GetDateTime("permit_dt").ToShortDateString(),
                                  dBTaxAmtDue.ToString(), "", frmReport.Text, strUser, strPosition, "", 0);

                        frmReport.DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, frmReport.m_form.ProgressBarWork);
                        Thread.Sleep(3);
                        iTotalBns++;

                        if (iTotalBns.ToString() == strGross)
                            break;
                        sPrevBin = result.GetString("bin");
                    }
                }

                frmReport.axVSPrinter1.Paragraph = "";
                frmReport.axVSPrinter1.FontBold = true;
                frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
                //frmReport.axVSPrinter1.Table = String.Format("<17300;{0}", "Total No of Businesses: " + iTotalBns);
                frmReport.axVSPrinter1.Table = String.Format("<18300;{0}", "Total No of Businesses: " + iTotalBns); //AFM 20200218
                frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                frmReport.axVSPrinter1.FontBold = false;

            }
            result.Close();

            frmReport.DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, frmReport.m_form.ProgressBarWork);
            Thread.Sleep(10);

            frmReport.axVSPrinter1.EndDoc();
        }

        public static void LoadByLeaseGrosses(frmReport frmReport, frmBusinessRoll frmBusinessRoll)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rs = new OracleResultSet();

            String strHeading = AppSettingsManager.GetConfigValue("02");
            String strBrgy = frmBusinessRoll.cboBarangay.Text;
            String strBnsType = frmBusinessRoll.cboBussType.Text;
            String strBnsCode = frmBusinessRoll.BusinessCode[frmBusinessRoll.cboBussType.SelectedIndex];
            String strBnsNatureCode = frmBusinessRoll.NatureCode[frmBusinessRoll.cboBussNature.SelectedIndex];
            String strGross = frmBusinessRoll.txtGross.Text;
            String strTaxYear = frmBusinessRoll.txtTaxYear.Text;
            String strUser = "";
            String strPosition = "";

            string strReportTitle = "";
            int iYear = AppSettingsManager.GetCurrentDate().Year;
            string strBIN = "";
            string strBnsCodeTmp = "";
            string strBnsNm = "";
            string strBnsAddr = "";
            string strOwnNm = "";
            string strOwnAddr = "";
            string strBnsDesc = "";
            string strDTINo = ""; //JAV 20170911 add DTI/SEC
            string strGrossTmp = "0";
            string strPermitNo = "";
            string strORNo = "";
            string strQtrPaid = "";
            string strTaxPaid = "";
            string strTopTmp = "0";
            double dGross = 0;
            double dTaxPaid = 0;
            DateTime dtOperated = new DateTime();
            DateTime dtPermit = new DateTime();
            int iBnsCount = 0;
            int iEmpCount = 0;

            try
            {
                strUser = AppSettingsManager.SystemUser.UserCode;
                strPosition = AppSettingsManager.SystemUser.Position;
            }
            catch
            {
                strUser = "SYS_PROG";
                strPosition = "SYSTEM PROGRAMMER";
            }

            if (strBrgy == "ALL")
                strBrgy = "%%";

            if (strBnsCode == "ALL" || strBnsCode == "")
                strBnsCode = "%%";

            if (strBnsNatureCode != "")
                strBnsCode = strBnsNatureCode;

            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; // AST 20160126
            //frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprUser;
            frmReport.axVSPrinter1.FontName = "Arial";
            frmReport.axVSPrinter1.MarginTop = 500;

            if (frmBusinessRoll.chkByPercentage.Checked)
                strTopTmp = strGross + "%GR";
            else
                strTopTmp = strGross + "GR";

            int iRecordCnt = 0; 

            result.Query = string.Format("delete from top_grosspayer_tbl where tax_year = '{0}' and top = '{1}' ", strTaxYear, strTopTmp);
            if (result.ExecuteNonQuery() == 0) { }

            // RMC 20150429 corrected reports (s)
            if (strTaxYear.Trim() == "")
            {
                //AFM 20200218 added num_employees
                result.Query = "select a.bin,bns_nm,bns_code,own_code,sum(distinct gr_1) + sum(gross) as gr,gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no, a.num_employees ";
                result.Query += string.Format("from businesses a, addl_bns b  where rtrim(bns_code) like '{0}' and bns_brgy like '{1}' ", strBnsCode, strBrgy);
                result.Query += "and a.bns_stat = 'REN' and a.bin = b.bin and a.tax_year = b.tax_year and a.bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";
                result.Query += "group by a.bin,bns_nm,bns_code,own_code,gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no,a.num_employees ";
                result.Query += "order by 5 asc ";
            }// RMC 20150429 corrected reports (e)
            else
            {
                //AFM 20200218 added num_employees
                result.Query = "select a.bin,bns_nm,bns_code,own_code,sum(distinct gr_1) + sum(gross) as gr,gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no,dti_reg_no,a.num_employees "; // JAV 20170911 add a gr name in total of sum(distinct gr_1) + sum(gross) and add DTI/SEC
                result.Query += string.Format("from businesses a, addl_bns b  where rtrim(bns_code) like '{0}' and bns_brgy like '{1}' and a.tax_year = '{2}' ", strBnsCode, strBrgy, strTaxYear);
                result.Query += "and a.bns_stat = 'REN' and a.bin = b.bin and a.tax_year = b.tax_year and a.bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";
                result.Query += "group by a.bin,bns_nm,bns_code,own_code,gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no,dti_reg_no, a.num_employees ";
                result.Query += "union ";
                result.Query += "select a.bin,bns_nm,bns_code,own_code,sum(distinct gr_1) + sum(gross),gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no,dti_reg_no, a.num_employees from buss_hist a ,addl_bns b ";// JAV 20170911 add DTI/SEC
                result.Query += string.Format("where rtrim(bns_code) like '{0}' and bns_brgy like '{1}' and a.tax_year = '{2}' and a.bns_stat = 'REN' and a.bin = b.bin and a.tax_year = b.tax_year ", strBnsCode, strBrgy, strTaxYear);
                result.Query += "and a.bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";
                //JAV 20170824 (s)
                result.Query += string.Format("and a.bin not in (select bin from businesses where tax_year = '{0}') ", strTaxYear);
                result.Query += string.Format("and a.bin not in (select bin from buss_hist where tax_year = '{0}' having count(bin) > 1 group by bin)", strTaxYear);
                //JAV 20170824 (e)
                result.Query += "group by a.bin,bns_nm,bns_code,own_code,gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no,dti_reg_no,a.num_employees ";
                result.Query += "order by 5 asc ";
            }
            /* REM MCR 20140804
            if (result.Execute())
            {
                while (result.Read())
                {
                    iRecordCnt++;
                }
            }
            result.Close();
            */
            #region Progress

            int intCount = 0;
            int intCountIncreament = 0;
            frmReport.m_objThread = new Thread(frmReport.ThreadProcess);
            frmReport.m_objThread.Start();
            Thread.Sleep(500);
            intCount = Convert.ToInt32(strGross);
            frmReport.DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, frmReport.m_form.ProgressBarWork);
            #endregion

            if (frmBusinessRoll.chkByPercentage.Checked)
            {
                float fGrossTmp = float.Parse(strGross);
                float fGross = iBnsCount * fGrossTmp / 100;
                strGross = string.Format("{0:###}", fGross);
            }

            frmReport.BusinessType = strBnsType;
            frmReport.Gross = strGross;
            frmReport.TaxYear = strTaxYear;
            if (frmBusinessRoll.chkByPercentage.Checked)
                strReportTitle = "BUSINESS ROLL BY LEASE " + strGross + "%" + " GROSSES FOR " + iYear;
            else
                strReportTitle = "BUSINESS ROLL BY LEASE " + strGross + "" + " GROSSES FOR " + iYear;

            frmReport.ReportTitle = strReportTitle;
            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618

            if (result.Execute())
            {
                while (result.Read())
                {
                    frmReport.DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, frmReport.m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    strBIN = result.GetString("bin");

                    strBnsNm = result.GetString("bns_nm");
                    strBnsAddr = AppSettingsManager.GetBnsAdd(strBIN, "");
                    //strBnsAddr = result.GetString("bns_house_no");
                    //strBnsAddr += result.GetString("bns_street");
                    //strBnsAddr += result.GetString("bns_mun");

                    strOwnNm = AppSettingsManager.GetBnsOwner(result.GetString("own_code"));
                    strOwnAddr = AppSettingsManager.GetBnsOwnAdd(result.GetString("own_code"));
                    //strOwnNm = result.GetString("own_fn");
                    //strOwnNm += result.GetString("own_mi");
                    //strOwnNm += result.GetString("own_ln");
                    //strOwnAddr = result.GetString("own_house_no");
                    //strOwnAddr += result.GetString("own_street");
                    //strOwnAddr += result.GetString("own_mun");

                    strDTINo = result.GetString("dti_reg_no"); // JAV 20170911 add DTI/SEC

                    iEmpCount = result.GetInt("num_employees"); // AFM 20200218

                    //dGross = result.GetDouble("gr_1");
                    //dGross += result.GetDouble("gr_2");
                    dGross = result.GetDouble("gr");// JAV 20170911 add a gr name in total of sum(distinct gr_1) + sum(gross)

                    dtOperated = result.GetDateTime("dt_operated");

                    strPermitNo = result.GetString("permit_no");

                    dtPermit = result.GetDateTime("permit_dt");

                    rs.Query = string.Format("select sum(gross) as gr_addl from addl_bns where bin = '{0}' and tax_year = '{1}' ", strBnsDesc, strTaxYear);
                    if (rs.Execute())
                    {
                        while (rs.Read())
                        {
                            dGross += rs.GetDouble("gr_addl");
                        }
                    }
                    rs.Close();
                    strGrossTmp = string.Format("{0:#,###.00}", dGross);

                    strBnsCodeTmp = result.GetString("bns_code");
                    //rs.Query = string.Format("select bns_desc from bns_table where fees_code = 'B' and bns_code = '{0}' and rev_year = '1993' ", strBnsCodeTmp);
                    rs.Query = string.Format("select bns_desc from bns_table where fees_code = 'B' and bns_code = '{0}' and rev_year = '"+AppSettingsManager.GetConfigValue("07") + "' ", strBnsCodeTmp);    // RMC 20150429 corrected reports
                    if (rs.Execute())
                    {
                        while (rs.Read())
                        {
                            strBnsDesc = rs.GetString("bns_desc");
                        }
                    }
                    rs.Close();

                    rs.Query = string.Format("select distinct or_no, qtr_paid from pay_hist where bin = '{0}' and tax_year = '{1}' ", strBIN, strTaxYear);
                    rs.Query += "order by qtr_paid";
                    if (rs.Execute())
                    {
                        while (rs.Read())
                        {
                            strORNo = rs.GetString("or_no");
                            strQtrPaid = rs.GetString("qtr_paid");
                        }
                    }
                    rs.Close();

                    rs.Query = string.Format("select sum(fees_amtdue) as BtaxAmtDue from or_table where or_no = '{0}'", strORNo);
                    if (rs.Execute())
                    {
                        while (rs.Read())
                        {
                            dTaxPaid = rs.GetDouble("BtaxAmtDue");
                        }
                    }
                    rs.Close();
                    strTaxPaid = string.Format("{0:#,###.00}", dTaxPaid);

                    //frmReport.axVSPrinter1.Table = string.Format("<2500|<2500|<2500|<2500|>2000|^1500|^1500|^1500|>1300|^700;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                    //    strBIN, strBnsNm + strBnsAddr, strOwnNm + strOwnAddr, strBnsDesc, strGrossTmp, dtOperated.ToShortDateString(), strPermitNo, dtPermit.ToShortDateString(), strTaxPaid, strQtrPaid); // JAV 20170911 add DTI/SEC

                    //frmReport.axVSPrinter1.Table = string.Format("<2100|<2300|<2300|<2200|>1300|>1800|^1500|^1500|^1500|>1300|^700;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}",
                       //strBIN, strBnsNm + strBnsAddr, strOwnNm + strOwnAddr, strBnsDesc, strDTINo ,strGrossTmp, dtOperated.ToShortDateString(), strPermitNo, dtPermit.ToShortDateString(), strTaxPaid, strQtrPaid);
                    //AFM 20200218
                    frmReport.axVSPrinter1.Table = string.Format("<1500|<1500|<1500|<1500|<1500|<2200|>1300|>1800|^1500|^1500|^1500|>1300|^700|^500;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}",
                       strBIN, strBnsNm, strBnsAddr, strOwnNm, strOwnAddr, strBnsDesc, strDTINo, strGrossTmp, dtOperated.ToShortDateString(), strPermitNo, dtPermit.ToShortDateString(), strTaxPaid, strQtrPaid, iEmpCount.ToString());


                    rs.Query = string.Format("insert into top_grosspayer_tbl values('232-00-2013-0001093','2013','10%GR','SYS_PROG',to_date('2014-01-17 13:46:45','YYYY-MM-DD HH24:MI:SS'))",
                        strBIN, strTaxYear, strTopTmp, strUser, AppSettingsManager.GetCurrentDate().ToShortDateString());
                    if (rs.ExecuteNonQuery() == 0) { }

                    if (m_bViewOnly == false) //MCR 20140602
                        frmBusinessRoll.UpdateBnsRollReport(strHeading, "", strBrgy, strBIN, strBnsNm, strBnsAddr,
                               strOwnNm, strOwnAddr, strBnsDesc, "REN", "0", dGross.ToString(),
                               dtOperated.ToShortDateString(), strPermitNo, dtPermit.ToShortDateString(),
                               dTaxPaid.ToString(), strQtrPaid, frmReport.Text, strUser, strPosition, "", 0);

                    iBnsCount++;

                    if (iBnsCount.ToString() == strGross) //MCR 20140804
                        break;

                    frmReport.DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, frmReport.m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }
            }
            result.Close();

            frmReport.DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, frmReport.m_form.ProgressBarWork);
            Thread.Sleep(10);
            frmBusinessRoll.UpdateGenerateInfo();

            frmReport.axVSPrinter1.Paragraph = "";
            frmReport.axVSPrinter1.FontBold = true;
            frmReport.axVSPrinter1.Table = string.Format("<18500;{0}", "Total No. of Businesses: " + strGross);
            frmReport.axVSPrinter1.FontBold = false;

            frmReport.axVSPrinter1.EndDoc();
        }

        public static void LoadByLeastPayers(frmReport frmReport, frmBusinessRoll frmBusinessRoll)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rs = new OracleResultSet();

            string strHeading = "";
            string strUser = "";
            string strPosition = "";
            string strReportTitle = "";
            string strTmpPR = "";
            string strBnsBrgy = frmBusinessRoll.cboBarangay.Text;
            string strBnsType = frmBusinessRoll.cboBussType.Text;
            string strBnsCode = frmBusinessRoll.BusinessCode[frmBusinessRoll.cboBussType.SelectedIndex];
            string strTaxYear = frmBusinessRoll.txtTaxYear.Text;
            string strGross = frmBusinessRoll.txtGross.Text;
            string strCurrentDate = AppSettingsManager.GetCurrentDate().ToShortDateString();
            int iYear = AppSettingsManager.GetCurrentDate().Year;
            double dRecordCnt = 0;
            string strBnsName = "";
            string strBnsAddr = "";
            string strOwnName = "";
            string strOwnAddr = "";
            string strOwnCode = "";
            string strBnsDesc = "";
            string strBnsOrgn = "";
            double gr1 = 0;
            double gr2 = 0;
            double dGrossReceipt = 0;
            double dCapital = 0;
            double dBTaxAmtDue = 0;
            int iTotalBns = 0;
            string strTopGrossTmp = "0";
            int iEmpCnt = 0;

            try
            {
                strUser = AppSettingsManager.SystemUser.UserCode;
                strPosition = AppSettingsManager.SystemUser.Position;
            }
            catch
            {
                strUser = "SYS_PROG";
                strPosition = "SYSTEM PROGRAMMER";
            }

            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; // AST 20160126 rem
            //frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprUser;
            frmReport.axVSPrinter1.FontName = "Arial";
            frmReport.axVSPrinter1.MarginTop = 500;

            if (frmBusinessRoll.chkByPercentage.Checked)
                strReportTitle = "BUSINESS ROLL BY LEAST " + strGross + "%" + " PAYERS FOR " + iYear;
            else
                strReportTitle = "BUSINESS ROLL BY LEAST " + strGross + "" + " PAYERS FOR " + iYear;

            frmReport.ReportTitle = strReportTitle;


            result.Query = string.Format("delete from rep_bnsrol_top where report_name = '{0}' and user_code = '{1}'", strReportTitle, strUser);
            result.ExecuteNonQuery();

            result.Query = string.Format("delete from gen_info where report_name = '{0}'", strReportTitle);
            result.ExecuteNonQuery();

            if (frmBusinessRoll.chkByPercentage.Checked)
                strTmpPR = strGross + "%PR";
            else
                strTmpPR = strGross + "PR";

            result.Query = string.Format("delete from top_grosspayer_tbl where tax_year = '{0}' and top = '{1}'", strTaxYear, strTmpPR);
            result.ExecuteNonQuery();

            result.Query = string.Format("insert into gen_info values('{0}',to_date('{1}', 'MM/dd/yyyy'), '{2}',{3},'{4}')", strReportTitle, strCurrentDate, strUser, 1, "ASS");
            result.ExecuteNonQuery();

            if (strBnsBrgy == "" || strBnsBrgy == "ALL")
                strBnsBrgy = "%%";

            if (strBnsCode == "")
                strBnsCode = "%%";
            else
                strBnsCode = strBnsCode + "%";
            // RMC 20150429 corrected reports (s)
            if (strTaxYear.Trim() == "")
            {
                //AFM 20200218 added num_employees
                result.Query = "select businesses.bin,sum(or_table.fees_amtdue) as amt_paid, bns_nm,orgn_kind, ";
                result.Query += "businesses.bns_stat,bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,businesses.or_no, businesses.num_employees  ";
                result.Query += "from businesses, pay_hist, or_table ";
                result.Query += "where businesses.bin = pay_hist.bin and pay_hist.or_no = or_table.or_no ";
                result.Query += "and trim(businesses.bns_code) like '" + strBnsCode + "' and trim(businesses.bns_brgy) like '" + strBnsBrgy + "' ";
                result.Query += "and businesses.tax_year = pay_hist.tax_year and pay_hist.bns_stat = businesses.bns_stat ";
                result.Query += "group by businesses.bin, bns_nm, businesses.bns_stat,bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,businesses.or_no,orgn_kind, businesses.num_employees ";
                result.Query += "order by amt_paid asc";
            }// RMC 20150429 corrected reports (e)
            else
            {
                result.Query = "select businesses.bin,sum(or_table.fees_amtdue) as amt_paid, bns_nm,orgn_kind, ";
                result.Query += "businesses.bns_stat,bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,businesses.or_no, businesses.num_employees  ";
                result.Query += "from businesses, pay_hist, or_table ";
                result.Query += "where businesses.bin = pay_hist.bin and pay_hist.or_no = or_table.or_no ";
                result.Query += "and trim(businesses.bns_code) like '" + strBnsCode + "' and trim(businesses.bns_brgy) like '" + strBnsBrgy + "' ";
                result.Query += "and businesses.tax_year = '" + strTaxYear + "' and pay_hist.tax_year = '" + strTaxYear + "' and pay_hist.bns_stat = businesses.bns_stat ";
                result.Query += "group by businesses.bin, bns_nm, businesses.bns_stat,bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,businesses.or_no,orgn_kind,businesses.num_employees ";

                result.Query += "union select buss_hist.bin,sum(or_table.fees_amtdue) as amt_paid, bns_nm,orgn_kind, buss_hist.bns_stat, bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,buss_hist.or_no, buss_hist.num_employees ";
                result.Query += "from buss_hist, pay_hist, or_table where buss_hist.bin = pay_hist.bin and pay_hist.or_no = or_table.or_no ";
                result.Query += "and trim(buss_hist.bns_code) like '" + strBnsCode + "' and trim(buss_hist.bns_brgy) like '" + strBnsBrgy + "' and buss_hist.tax_year = '" + strTaxYear + "' ";
                result.Query += "and pay_hist.tax_year = '" + strTaxYear + "' and pay_hist.bns_stat = buss_hist.bns_stat ";
                result.Query += "group by buss_hist.bin, bns_nm, buss_hist.bns_stat, bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,buss_hist.or_no,orgn_kind,buss_hist.num_employees ";
                result.Query += "order by amt_paid asc";
            }
            /* REM MCR 20140804
            if (result.Execute())
            {
                while (result.Read())
                {
                    dRecordCnt++;
                }
            }
            result.Close();
            */
            #region Progress

            int intCount = 0;
            int intCountIncreament = 0;
            frmReport.m_objThread = new Thread(frmReport.ThreadProcess);
            frmReport.m_objThread.Start();
            Thread.Sleep(500);
            intCount = Convert.ToInt32(strGross);
            frmReport.DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, frmReport.m_form.ProgressBarWork);
            #endregion

            if (frmBusinessRoll.chkByPercentage.Checked)
            {
                double dTmpTopGross = double.Parse(strGross);
                double dPercent = (dRecordCnt * dTmpTopGross) / 100;
                strTopGrossTmp = string.Format("{0:###}", dPercent);
            }
            else
                strTopGrossTmp = strGross;

            frmReport.BusinessType = strBnsType;
            frmReport.Gross = strTopGrossTmp;
            frmReport.TaxYear = strTaxYear;

            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618

            frmReport.axVSPrinter1.FontSize = 8.0f;

            frmReport.axVSPrinter1.Paragraph = "";
            frmReport.axVSPrinter1.FontBold = true;
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
            //frmReport.axVSPrinter1.Table = String.Format("<19000;{0}", "Barangay: " + frmBusinessRoll.cboBarangay.Text);
            frmReport.axVSPrinter1.Table = String.Format("<18000;{0}", "Barangay: " + frmBusinessRoll.cboBarangay.Text); //AFM 20200218
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            frmReport.axVSPrinter1.FontBold = false;

            if (result.Execute())
            {
                while (result.Read())
                {
                    frmReport.DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, frmReport.m_form.ProgressBarWork);
                    intCountIncreament += 1;

                    object objData = "";
                    strBnsName = result.GetString("bns_nm");
                    strBnsAddr = AppSettingsManager.GetBnsDesc(result.GetString("bns_code"));
                    strOwnCode = result.GetString("own_code");
                    strOwnName = AppSettingsManager.GetBnsOwner(strOwnCode);
                    strOwnAddr = AppSettingsManager.GetBnsOwnAdd(strOwnCode);
                    strBnsDesc = AppSettingsManager.GetBnsDesc(result.GetString("bns_code"));
                    strBnsOrgn = result.GetString("orgn_kind");
                    gr1 = result.GetDouble("gr_1");
                    gr2 = result.GetDouble("gr_2");
                    dGrossReceipt = gr1 + gr2;
                    dCapital = result.GetDouble("capital");
                    iEmpCnt = result.GetInt("num_employees"); //AFM 20200219

                    rs.Query = string.Format("select sum(fees_amtdue) as BtaxAmtDue from or_table where or_no in (select or_no from pay_hist where bin = '{0}' and tax_year = '{1}')", result.GetString("bin"), strTaxYear);
                    if (rs.Execute())
                    {
                        while (rs.Read())
                        {
                            dBTaxAmtDue = rs.GetDouble("BtaxAmtDue");
                        }
                    }
                    rs.Close();

                    objData = result.GetString("bin") + "|";
                    //objData += strBnsName + "/" + strBnsAddr + "|";
                    //objData += strOwnName + "/" + strOwnAddr + "|";
                    //AFM 20200218 (s)
                    objData += strBnsName + "|";
                    objData += strBnsAddr + "|";
                    objData += strOwnName + "|";
                    objData += strOwnAddr + "|";
                    //AFM 20200218 (e)
                    objData += strBnsDesc + "/" + "REN" + "/" + strBnsOrgn + "|";
                    objData += string.Format("{0:#,###.00}\n{1:#,###.00}", dGrossReceipt, dCapital) + "|";
                    objData += result.GetDateTime("dt_operated").ToShortDateString() + "|";
                    objData += result.GetString("permit_no") + "|";
                    objData += result.GetDateTime("permit_dt").ToShortDateString() + "|";
                    objData += string.Format("{0:#,###.00}", dBTaxAmtDue) + "|";
                    objData += iEmpCnt.ToString(); //AFM 20200218

                    frmReport.axVSPrinter1.Table = string.Format("<1500|<1500|<1500|<1500|<1500|<2500|>2000|^1500|^1500|^1500|>1000|^500;{0}", objData);

                    if (m_bViewOnly == false) //MCR 20140602
                        frmBusinessRoll.UpdateBnsRollReport(strHeading, "", strBnsBrgy, result.GetString("bin"), strBnsName, strBnsAddr,
                             strOwnName, strOwnAddr, strBnsDesc, "REN", dCapital.ToString(), strGross,
                              result.GetDateTime("dt_operated").ToShortDateString(), result.GetString("permit_no"), result.GetDateTime("permit_dt").ToShortDateString(),
                             dBTaxAmtDue.ToString(), "", frmReport.Text, strUser, strPosition, "", 0);


                    iTotalBns++;
                    if (iTotalBns.ToString() == strGross)
                        break;

                    frmReport.DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, frmReport.m_form.ProgressBarWork);
                    Thread.Sleep(3);
                }

                frmReport.axVSPrinter1.Paragraph = "";
                frmReport.axVSPrinter1.FontBold = true;
                frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
                //frmReport.axVSPrinter1.Table = String.Format("<19000;{0}", "Total No of Businesses: " + iTotalBns);
                frmReport.axVSPrinter1.Table = String.Format("<18000;{0}", "Total No of Businesses: " + iTotalBns); //AFM 20200218
                frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                frmReport.axVSPrinter1.FontBold = false;

            }
            result.Close();

            frmReport.DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, frmReport.m_form.ProgressBarWork);
            Thread.Sleep(10);

            frmBusinessRoll.UpdateGenerateInfo();

            frmReport.axVSPrinter1.EndDoc();
        }

        public static void LoadByName(frmReport frmReport, frmBusinessRoll frmBusinessRoll)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rs = new OracleResultSet();
            OracleResultSet rs2 = new OracleResultSet();

            String strHeading = "";
            String strUser = "";
            String strPosition = "";
            String strDistrict = frmBusinessRoll.cboDistrict.Text;
            String strStatus = frmBusinessRoll.cboStatus.Text;
            String strTaxYear = frmBusinessRoll.txtTaxYear.Text;
            String strBrgyTmp = "";
            String strDistrictTmp = "";

            string strBIN = "";
            string strBnsNm = "";
            string strOwnNm = "";
            string strBnsDesc = "";
            string strStatusTmp = "";
            string strCapital = "0";
            string strGross = "0";
            double dCapital = 0;
            double dGross = 0;
            DateTime dtOperated = new DateTime();
            DateTime dtApplication = new DateTime();
            int iBnsCount = 0;

            try
            {
                strUser = AppSettingsManager.SystemUser.UserCode;
                strPosition = AppSettingsManager.SystemUser.Position;
            }
            catch
            {
                strUser = "SYS_PROG";
                strPosition = "SYSTEM PROGRAMMER";
            }

            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            //frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; // AST 20160126 rem
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprUser;
            frmReport.axVSPrinter1.FontName = "Arial";
            frmReport.ReportTitle = "BUSINESS ROLL BY NAME";
            frmReport.axVSPrinter1.MarginTop = 500;
            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618

            frmReport.axVSPrinter1.FontSize = 8.0f;

            if (strDistrict == "" || strDistrict == "ALL")
                strDistrict = "%%";

            if (strStatus == "ALL")
                strStatus = "%%";

            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            frmReport.m_objThread = new Thread(frmReport.ThreadProcess);
            frmReport.m_objThread.Start();
            Thread.Sleep(500);

            if (AppSettingsManager.GetConfigValue("23") == "Y")
            {
                if (strDistrict == "%%")
                    strDistrict = " ";

                rs.Query = "select Count(*) from businesses,own_names where businesses.own_code = own_names.own_code ";
                rs.Query += string.Format("and businesses.bns_dist = '{0}' and rtrim(businesses.bns_stat) like '{1}' ", strDistrict, strStatus);

                if (strTaxYear.Trim() != "")
                    rs.Query += string.Format("and tax_year = '{0}' ", strTaxYear);
            }
            else
            {
                if (strDistrict == "%%")
                    strDistrict = " ";

                rs.Query = "Select sum(a.cnt) from (select Count(*) as cnt from businesses,own_names where businesses.own_code = own_names.own_code ";
                rs.Query += string.Format("and businesses.bns_dist = '{0}' and rtrim(businesses.bns_stat) like '{1}' ", strDistrict, strStatus);
                rs.Query += string.Format("and businesses.tax_year = '{0}' ", strTaxYear);
                rs.Query += "union all select Count(*) as cnt from buss_hist,own_names where buss_hist.own_code = own_names.own_code ";

                if (strTaxYear.Trim() != "")
                    rs.Query += string.Format("and tax_year = '{0}') a", strTaxYear);
            }

            int.TryParse(rs.ExecuteScalar(), out intCount);
            frmReport.DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, frmReport.m_form.ProgressBarWork);

            #endregion

            result.Query = string.Format("select distinct bns_dist dist_nm from businesses where bns_dist like '{0}'", strDistrict);
            if (result.Execute())
            {
                while (result.Read())
                {
                    strDistrictTmp = result.GetString("dist_nm");

                    if (AppSettingsManager.GetConfigValue("23") == "Y")
                    {
                        rs.Query = "select businesses.*, own_names.* from businesses,own_names where businesses.own_code = own_names.own_code ";
                        rs.Query += string.Format("and businesses.bns_dist = '{0}' and rtrim(businesses.bns_stat) like '{1}' ", strDistrictTmp, strStatus);
                    }
                    else
                    {
                        rs.Query = "select businesses.*, own_names.* from businesses,own_names where businesses.own_code = own_names.own_code ";
                        rs.Query += string.Format("and businesses.bns_dist = '{0}' and rtrim(businesses.bns_stat) like '{1}' ", strDistrictTmp, strStatus);
                        rs.Query += string.Format("and businesses.tax_year = '{0}' ", strTaxYear);
                        rs.Query += "union all select buss_hist.*, own_names.* from buss_hist,own_names where buss_hist.own_code = own_names.own_code ";
                    }

                    if (strTaxYear.Trim() != "")
                        rs.Query += string.Format("and tax_year = '{0}' ", strTaxYear);

                    rs.Query += string.Format(" and bin not in (select bin from businesses where tax_year ='{0}'", strTaxYear); // RMC 20170227 correction in Business Roll

                    rs.Query += "order by 2 asc";
                    if (rs.Execute())
                    {
                        while (rs.Read())
                        {
                            frmReport.DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, frmReport.m_form.ProgressBarWork);
                            intCountIncreament += 1;

                            strBIN = rs.GetString("bin");
                            strBnsNm = rs.GetString("bns_nm");
                            strOwnNm = AppSettingsManager.GetBnsOwner(rs.GetString("own_code"));
                            strBnsDesc = AppSettingsManager.GetBnsDesc(rs.GetString("bns_code"));
                            strStatusTmp = rs.GetString("bns_stat");
                            dCapital = rs.GetDouble("capital");
                            dGross = rs.GetDouble("gr_1") + rs.GetDouble("gr_2");
                            dtOperated = rs.GetDateTime("dt_operated");
                            dtApplication = rs.GetDateTime("save_tm");

                            //load gross, capital
                            rs2.Query = string.Format("select gross,capital from addl_bns where bin = '{0}' and tax_year = '{1}' ", strBrgyTmp, strTaxYear);
                            if (rs2.Execute())
                            {
                                while (rs2.Read())
                                {
                                    dGross += rs2.GetDouble("gross");
                                    dCapital += rs2.GetDouble("capital");
                                }
                            }
                            rs2.Close();

                            strGross = string.Format("{0:#,###.00}", dGross);
                            strCapital = string.Format("{0:#,###.00}", dCapital);

                            frmReport.axVSPrinter1.Table = string.Format("^2500|<2500|<2500|<2500|^2000|>1500|>1500|^1500|^1500;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
                                strBIN, strBnsNm, strOwnNm, strBnsDesc, strStatusTmp, strCapital, strGross, dtOperated.ToShortDateString(), dtApplication.ToShortDateString());

                            //   frmBusinessRoll.UpdateBnsRollReport(strHeading, "", "", strBIN, strBnsNm, "",
                            //strOwnNm, "", strBnsDesc, "REN", "0", strGross,
                            // dtOperated.ToShortDateString(), "", "",
                            //"0", "0", frmReport.Text, strUser, strPosition, "", "");

                            iBnsCount++;
                            frmReport.DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, frmReport.m_form.ProgressBarWork);
                            Thread.Sleep(3);
                        }
                    }
                    rs.Close();
                }
            }
            result.Close();


            frmReport.DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, frmReport.m_form.ProgressBarWork);
            Thread.Sleep(10);

            frmReport.axVSPrinter1.Paragraph = "";
            frmReport.axVSPrinter1.FontBold = true;
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
            frmReport.axVSPrinter1.Table = String.Format("<19000;{0}", "Total No of Businesses: " + iBnsCount);
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            frmReport.axVSPrinter1.FontBold = false;

            frmBusinessRoll.UpdateGenerateInfo();

            frmReport.axVSPrinter1.EndDoc();
        }

        public static void LoadByBusinessQueue(frmReport frmReport, frmBusinessRoll frmBusinessRoll)
        {

            OracleResultSet result = new OracleResultSet();
            OracleResultSet rs = new OracleResultSet();
            OracleResultSet rs2 = new OracleResultSet();

            string strUser = "";
            string strPosition = "";
            string strHeading = AppSettingsManager.GetConfigValue("02");
            string strBrgy = frmBusinessRoll.cboBarangay.Text;
            string strStatus = frmBusinessRoll.cboStatus.Text;
            string strTaxYear = frmBusinessRoll.txtTaxYear.Text;

            string strBrgyTmp = "";
            string strBIN = "";
            string strBnsNm = "";
            string strBnsAddr = "";
            string strOwnCode = "";
            string strOwnNm = "";
            string strOwnAddr = "";
            double dCapital = 0;
            double dGross = 0;
            string strCapital = "0";
            string strGross = "0";
            DateTime dtOperated = new DateTime();
            DateTime dtPermit = new DateTime();
            string strPermitNo = "";
            string strBnsCode = "";
            string strBnsDesc = "";
            double dPenalty = 0;
            double dBilledAmt = 0;
            string strPenalty = "";
            string strBilledAmt = "0";
            string strStatusTmp = "";
            int iBnsCount = 0;
            double dGr1 = 0; //JARS 20170906
            double dGr2 = 0;

            // use this to catch if this project not yet attach to main BPLS
            try
            {
                strUser = AppSettingsManager.SystemUser.UserCode;
                strPosition = AppSettingsManager.SystemUser.Position;
            }
            catch
            {
                strUser = "SYS_PROG";
                strPosition = "SYSTEM PROGRAMMER";
            }

            if (strBrgy == "ALL")
                strBrgy = "%%";

            if (strStatus == "ALL")
                strStatus = "%%";

            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            frmReport.m_objThread = new Thread(frmReport.ThreadProcess);
            frmReport.m_objThread.Start();
            Thread.Sleep(500);

            //rs.Query = "select Count(*) from business_que,own_names where business_que.own_code = own_names.own_code ";
            //rs.Query += string.Format("and rtrim(business_que.bns_brgy) like '{0}' and rtrim(business_que.bns_stat) like '{1}' and business_que.bin ", strBrgyTmp, strStatus);
            //rs.Query += string.Format("not in (select bin from businesses where bns_stat like '{0}' ", strStatus);
            //if (strTaxYear != "")
            //    rs.Query += string.Format("and tax_year like '{0}'", strTaxYear);
            //rs.Query += ") ";
            //if (strTaxYear != "")
            //    rs.Query += string.Format("and business_que.tax_year like '{0}'", strTaxYear);

            //AFM 20190726 applied proper quuery for progress bar (s)
            rs.Query = "select count(*) from business_que where tax_year = '" + strTaxYear + "' ";
            rs.Query += "and bns_stat like '" + strStatus + "' ";
            rs.Query += "and bns_brgy like '" + strBrgy + "' ";
            rs.Query += " and bin not in (select bin from businesses where tax_year >= '" + strTaxYear + "')";
            //AFM 20190726 applied proper quuery for progress bar (e)

            int.TryParse(rs.ExecuteScalar(), out intCount);
            frmReport.DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, frmReport.m_form.ProgressBarWork);
            #endregion

            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            //frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; // AST 20160126 rem
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprUser;
            frmReport.axVSPrinter1.FontName = "Arial";
            frmReport.axVSPrinter1.MarginTop = 500;
            frmReport.ReportTitle = "BUSINESS ON QUEUE";
            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618

            frmReport.axVSPrinter1.FontSize = 8.0f;
            #region comments
            /*result.Query = string.Format("select distinct bns_brgy from business_que where bns_brgy like '{0}' and business_que.bin ", strBrgy);
            result.Query += string.Format("not in (select bin from businesses where bns_stat like '{0}' ", strStatus);
            if (strTaxYear != "")
                result.Query += string.Format("and tax_year = '{0}' ", strTaxYear);
            result.Query += ") ";
            result.Query += string.Format("and bns_stat like '{0}' ", strStatus);
            if (strTaxYear != "")
                result.Query += string.Format("and tax_year like '{0}' ", strTaxYear);
            result.Query += string.Format("union select distinct bns_brgy from buss_hist where bns_brgy like '{0}' ", strBrgy);
            if (strTaxYear != "")
                result.Query += string.Format("and tax_year like '{0}' ", strTaxYear);
            result.Query += "order by bns_brgy";
           
            if (result.Execute())
            {
                while (result.Read())
                {
                    strBrgyTmp = result.GetString("bns_brgy");    // RMC 20170227 correction in Business OnQueue

                    rs.Query = "select business_que.*, own_names.* from business_que,own_names where business_que.own_code = own_names.own_code ";
                    rs.Query += string.Format("and rtrim(business_que.bns_brgy) like '{0}' and rtrim(business_que.bns_stat) like '{1}' and business_que.bin ", strBrgyTmp, strStatus);
                    rs.Query += string.Format("not in (select bin from businesses where bns_stat like '{0}' ", strStatus);
                    if (strTaxYear != "")
                        rs.Query += string.Format("and tax_year like '{0}'", strTaxYear);

                    rs.Query += ") ";

                    if (strTaxYear != "")
                        rs.Query += string.Format("and business_que.tax_year like '{0}'", strTaxYear);*/
            // RMC 20170227 correction in Business OnQueue, put rem
            // RMC 20170227 correction in Business OnQueue (s)
            #endregion
            result.Query = "select distinct tax_year from business_que ";
            result.Query += string.Format("where bns_stat like '{0}' ", strStatus);
            result.Query += string.Format("and bns_brgy like '{0}' ", strBrgy);

            if (strTaxYear != "")
                result.Query += string.Format("and tax_year like '{0}' ", strTaxYear);
            result.Query += "order by tax_year desc";
            
            if (result.Execute())
            {
                while (result.Read())
                {
                    strTaxYear = result.GetString(0);

                    frmReport.axVSPrinter1.FontBold = true;
                    frmReport.axVSPrinter1.Table = string.Format("^2500|<2500|<2500|^2500|^2000|>1500|>1500|^1500|>1000;{0}||||||||",strTaxYear);
                    frmReport.axVSPrinter1.FontBold = false;

                    rs.Query = "select * from business_que where tax_year = '" + strTaxYear + "' ";
                    rs.Query += "and bns_stat like '" + strStatus + "' ";
                    rs.Query += "and bns_brgy like '" + strBrgy + "' ";
                    rs.Query += " and bin not in (select bin from businesses where tax_year >= '" + strTaxYear + "')";
                    rs.Query += " order by bin";
            // RMC 20170227 correction in Business OnQueue (e)
                    if (rs.Execute())
                    {
                        while (rs.Read())
                        {
                            frmReport.DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, frmReport.m_form.ProgressBarWork);
                            intCountIncreament += 1;

                            strBrgyTmp = rs.GetString("bns_brgy");  // RMC 20170227 correction in Business OnQueue
                            strBIN = rs.GetString("bin");
                            strBnsCode = rs.GetString("bns_code");
                            strBnsNm = rs.GetString("bns_nm");
                            strBnsAddr = AppSettingsManager.GetBnsAdd(strBIN, "");

                            strOwnCode = rs.GetString("own_code");
                            strOwnNm = AppSettingsManager.GetBnsOwner(strOwnCode);
                            strOwnAddr = AppSettingsManager.GetBnsOwnAdd(strOwnCode);

                            strBnsDesc = AppSettingsManager.GetBnsDesc(strBnsCode);

                            strStatusTmp = rs.GetString("bns_stat");

                            dCapital = rs.GetFloat("capital");
                            
                            dGross = rs.GetDouble("gr_1") + rs.GetDouble("gr_2"); //JARS 20170906 FROM FLOAT TO DOUBLE

                            dtOperated = rs.GetDateTime("dt_operated");
                            dtPermit = rs.GetDateTime("permit_dt");
                            strPermitNo = rs.GetString("permit_no");

                            rs2.Query = string.Format("select gross,capital from addl_bns where bin = '{0}' ", strBIN);
                            if (strTaxYear != "")
                                rs2.Query += "and tax_year = '" + strTaxYear + "' ";
                            if (rs2.Execute())
                            {
                                while (rs2.Read())
                                {
                                    dCapital += rs2.GetFloat("gross");
                                    dGross += rs2.GetFloat("capital");
                                }
                            }
                            rs2.Close();

                            strCapital = string.Format("{0:#,##0.00}", dCapital);
                            strGross = string.Format("{0:#,##0.00}", dGross);

                            strBnsDesc = AppSettingsManager.GetBnsDesc(strBnsCode);

                            rs2.Query = "select sum(amount) as amount from taxdues where bin = '" + strBIN + "'";
                            if (rs2.Execute())
                            {
                                while (rs2.Read())
                                {
                                    try
                                    {
                                        dBilledAmt = rs2.GetFloat("amount");
                                    }
                                    catch { }
                                }
                            }
                            rs2.Close();
                            //AFM 20190725 added penalty in report (s)
                            //rs2.Query = "select sum(fees_pen) as amount from pay_temp where bin = '" + strBIN + "'";
                            rs2.Query = "select sum(fees_pen) as amount from (select DISTINCT FEES_DESC, fees_pen from pay_temp where bin = '" + strBIN + "')";
                            if (rs2.Execute())
                            {
                                while (rs2.Read())
                                {
                                    dPenalty = rs2.GetDouble("amount");
                                }
                            }
                            //AFM 20190725 added penalty in report (e)

                            strBilledAmt = string.Format("{0:#,##0.00}", dBilledAmt);
                            strPenalty = string.Format("{0:#,##0.00}", dPenalty); 

                            frmReport.axVSPrinter1.Table = string.Format("^2500|<2500|<2500|^2500|^2000|>1500|>1500|^1500|>1000|>1000;{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                                strBIN, strBnsNm, strOwnNm, strBnsDesc, strStatusTmp, strCapital, strGross, dtOperated.ToShortDateString(),strPenalty, strBilledAmt);

                            if (m_bViewOnly == false) //MCR 20140602
                                frmBusinessRoll.UpdateBnsRollReport(strHeading, "", strBrgyTmp, strBIN, strBnsNm, strBnsAddr,
                             //strOwnNm, strOwnAddr, strBnsDesc, "REN", strCapital, strGross,
                             strOwnNm, strOwnAddr, strBnsDesc, strStatusTmp, strCapital, strGross,  // RMC 20170227 correction in Business OnQueue
                              dtOperated.ToShortDateString(), strPermitNo, dtPermit.ToShortDateString(),
                             "0", "", frmReport.Text, strUser, strPosition, "", 0);

                            iBnsCount++;

                            frmReport.DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, frmReport.m_form.ProgressBarWork);
                            Thread.Sleep(3);
                        }
                    }
                    rs.Close();
                }
            }
            result.Close();

            frmReport.DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, frmReport.m_form.ProgressBarWork);
            Thread.Sleep(10);

            frmBusinessRoll.UpdateGenerateInfo();

            frmReport.axVSPrinter1.Paragraph = "";
            frmReport.axVSPrinter1.FontBold = true;
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
            frmReport.axVSPrinter1.Table = String.Format("<19000;{0}", "Total No of Businesses: " + iBnsCount);
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            frmReport.axVSPrinter1.FontBold = false;

            frmReport.axVSPrinter1.EndDoc();
        }

        public static void LoadUnrenewed(frmReport frmReport, frmBusinessRoll frmBusinessRoll)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rs = new OracleResultSet();
            OracleResultSet rs2 = new OracleResultSet();

            string strUser = "";
            string strPosition = "";
            string strHeading = AppSettingsManager.GetConfigValue("02");
            string strBrgy = frmBusinessRoll.cboBarangay.Text;
            string strStatus = frmBusinessRoll.cboStatus.Text;
            string strTaxYear = frmBusinessRoll.txtTaxYear.Text;

            string strBrgyTmp = "%%";
            string strBIN = "";
            string strBnsNm = "";
            string strBnsAddr = "";
            string strOwnCode = "";
            string strOwnNm = "";
            string strOwnAddr = "";
            double dCapital = 0;
            double dGross = 0;
            string strCapital = "0";
            string strGross = "0";
            DateTime dtOperated = new DateTime();
            DateTime dtPermit = new DateTime();
            string strPermitNo = "";
            string strBnsCode = "";
            string strBnsDesc = "";
            double dBilledAmt = 0;
            string strBilledAmt = "0";
            string strStatusTmp = "";
            string strTaxyear = "";
            int iBnsCount = 0;

            // use this to catch if this project not yet attach to main BPLS
            try
            {
                strUser = AppSettingsManager.SystemUser.UserCode;
                strPosition = AppSettingsManager.SystemUser.Position;
            }
            catch
            {
                strUser = "SYS_PROG";
                strPosition = "SYSTEM PROGRAMMER";
            }

            if (strBrgy == "ALL")
                strBrgy = "%%";

            if (strStatus == "ALL")
                strStatus = "%%";

            strTaxYear = AppSettingsManager.GetSystemDate().Year.ToString();

            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            frmReport.m_objThread = new Thread(frmReport.ThreadProcess);
            frmReport.m_objThread.Start();
            Thread.Sleep(500);

            rs.Query = "select Count(*) from business_que,own_names where business_que.own_code = own_names.own_code ";
            rs.Query += string.Format("and rtrim(business_que.bns_brgy) like '{0}' and rtrim(business_que.bns_stat) <> 'RET' and business_que.bin ", strBrgyTmp);
            rs.Query += string.Format("not in (select bin from businesses where bns_stat <> 'RET' ");
            rs.Query += string.Format("and tax_year < '{0}'", strTaxYear);
            rs.Query += ") ";
            rs.Query += string.Format("and business_que.tax_year < '{0}'", strTaxYear);

            int.TryParse(rs.ExecuteScalar(), out intCount);
            frmReport.DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, frmReport.m_form.ProgressBarWork);
            #endregion

            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            //frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; // AST 20160126
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprUser;
            frmReport.axVSPrinter1.FontName = "Arial";
            frmReport.axVSPrinter1.MarginTop = 500;
            frmReport.ReportTitle = "UN-RENEWED BUSINESSES";
            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618

            frmReport.axVSPrinter1.FontSize = 8.0f;

            result.Query = string.Format("select distinct bns_brgy from business_que where bns_brgy like '{0}' and business_que.bin ", strBrgy);
            result.Query += string.Format("not in (select bin from businesses where bns_stat <> 'RET' ");
            result.Query += string.Format("and tax_year < '{0}' ", strTaxYear);
            result.Query += ") ";
            result.Query += string.Format("and bns_stat <> 'RET' ");
            result.Query += string.Format("and tax_year < '{0}' ", strTaxYear);
            result.Query += string.Format("union select distinct bns_brgy from buss_hist where bns_brgy like '{0}' ", strBrgy);
            result.Query += string.Format("and tax_year < '{0}' ", strTaxYear);
            result.Query += "order by bns_brgy";
            if (result.Execute())
            {
                while (result.Read())
                {
                    strBrgyTmp = result.GetString("bns_brgy");

                    rs.Query = "select business_que.*, own_names.* from business_que,own_names where business_que.own_code = own_names.own_code ";
                    rs.Query += string.Format("and rtrim(business_que.bns_brgy) like '{0}' and rtrim(business_que.bns_stat) <> 'RET' and business_que.bin ", strBrgyTmp);
                    rs.Query += string.Format("not in (select bin from businesses where bns_stat  <> 'RET' ");
                    rs.Query += string.Format("and tax_year < '{0}'", strTaxYear);
                    rs.Query += ") ";
                    rs.Query += string.Format("and business_que.tax_year < '{0}'", strTaxYear);

                    if (rs.Execute())
                    {
                        while (rs.Read())
                        {
                            frmReport.DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, frmReport.m_form.ProgressBarWork);
                            intCountIncreament += 1;

                            strBIN = rs.GetString("bin");
                            strBnsCode = rs.GetString("bns_code");
                            strBnsNm = rs.GetString("bns_nm");
                            strBnsAddr = AppSettingsManager.GetBnsAdd(strBIN, "");

                            strOwnCode = rs.GetString("own_code");
                            strOwnNm = AppSettingsManager.GetBnsOwner(strOwnCode);
                            strOwnAddr = AppSettingsManager.GetBnsOwnAdd(strOwnCode);

                            strBnsDesc = AppSettingsManager.GetBnsDesc(strBnsCode);

                            strStatusTmp = rs.GetString("bns_stat");

                            //dCapital = rs.GetFloat("capital");
                            //dGross = rs.GetFloat("gr_1") + rs.GetFloat("gr_2");
                            strTaxyear = rs.GetString("tax_year");
                            dtOperated = rs.GetDateTime("dt_operated");
                            dtPermit = rs.GetDateTime("permit_dt");
                            strPermitNo = rs.GetString("permit_no");
                            #region comments
                            /*rs2.Query = string.Format("select gross,capital from addl_bns where bin = '{0}' ", strBIN);
                            if (strTaxYear != "")
                                rs2.Query += "and tax_year = '" + strTaxYear + "' ";
                            if (rs2.Execute())
                            {
                                while (rs2.Read())
                                {
                                    dCapital += rs2.GetFloat("gross");
                                    dGross += rs2.GetFloat("capital");
                                }
                            }
                            rs2.Close();

                            strCapital = string.Format("{0:#,###.00}", dCapital);
                            strGross = string.Format("{0:#,###.00}", dGross);*/
                            #endregion
                            strBnsDesc = AppSettingsManager.GetBnsDesc(strBnsCode);
                            #region comments
                            /*rs2.Query = "select sum(amount) as amount from taxdues where bin = '" + strBIN + "'";
                            if (rs2.Execute())
                            {
                                while (rs2.Read())
                                {
                                    try
                                    {
                                        dBilledAmt = rs2.GetFloat("amount");
                                    }
                                    catch { }
                                }
                            }
                            rs2.Close();

                            strBilledAmt = string.Format("{0:#,###.00}", dBilledAmt);*/
                            #endregion
                            frmReport.axVSPrinter1.Table = string.Format("<2500|<4000|<4000|<3000|<1500|^1000|^1500;{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                                strBIN, strBnsNm, strOwnNm, strBnsDesc, strStatusTmp, strTaxyear, dtOperated.ToShortDateString());

                            if (m_bViewOnly == false) //MCR 20140602
                                /*frmBusinessRoll.UpdateBnsRollReport(strHeading, "", strBrgyTmp, strBIN, strBnsNm, strBnsAddr,
                             strOwnNm, strOwnAddr, strBnsDesc, "REN", strCapital, strGross,
                              dtOperated.ToShortDateString(), strPermitNo, dtPermit.ToShortDateString(),
                             "0", "", frmReport.Text, strUser, strPosition, "", 0);*/

                                frmBusinessRoll.UpdateBnsRollReport(strHeading, "", strBrgyTmp, strBIN, strBnsNm, strBnsAddr,
                             strOwnNm, strOwnAddr, strBnsDesc, "REN", "0", "0",
                              dtOperated.ToShortDateString(), strPermitNo, dtPermit.ToShortDateString(),
                             "0", "", frmReport.Text, strUser, strPosition, "", 0);

                            iBnsCount++;

                            frmReport.DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, frmReport.m_form.ProgressBarWork);
                            Thread.Sleep(3);
                        }
                    }
                    rs.Close();
                }
            }
            result.Close();

            frmReport.DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, frmReport.m_form.ProgressBarWork);
            Thread.Sleep(10);

            frmBusinessRoll.UpdateGenerateInfo();

            frmReport.axVSPrinter1.Paragraph = "";
            frmReport.axVSPrinter1.FontBold = true;
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
            frmReport.axVSPrinter1.Table = String.Format("<19000;{0}", "Total No of Businesses: " + iBnsCount);
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            frmReport.axVSPrinter1.FontBold = false;

            frmReport.axVSPrinter1.EndDoc();
        }
        
        public static void LoadByTopAssessed(frmReport frmReport, frmBusinessRoll frmBusinessRoll) //JARS 20170720
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rs = new OracleResultSet();

            string strHeading = "";
            string strUser = "";
            string strPosition = "";
            string strReportTitle = "";
            string strTmpPR = "";
            string strBnsBrgy = frmBusinessRoll.cboBarangay.Text;
            string strBnsType = frmBusinessRoll.cboBussType.Text;
            string strBnsCode = frmBusinessRoll.BusinessCode[frmBusinessRoll.cboBussType.SelectedIndex];
            string strTaxYear = frmBusinessRoll.txtTaxYear.Text;
            string strGross = frmBusinessRoll.txtGross.Text;
            string strCurrentDate = AppSettingsManager.GetCurrentDate().ToShortDateString();
            int iYear = AppSettingsManager.GetCurrentDate().Year;
            double dRecordCnt = 0;
            string strBnsName = "";
            string strBnsAddr = "";
            string strOwnName = "";
            string strOwnAddr = "";
            string strOwnCode = "";
            string strBnsDesc = "";
            string strBnsOrgn = "";
            double gr1 = 0;
            double gr2 = 0;
            double dGrossReceipt = 0;
            double dCapital = 0;
            double dBTaxAmtDue = 0;
            int iTotalBns = 0;
            string strTopGrossTmp = "0";
            string sCurrBin = "";
            string sPrevBin = "";
            int iEmpCnt = 0;

            try
            {
                strUser = AppSettingsManager.SystemUser.UserCode;
                strPosition = AppSettingsManager.SystemUser.Position;
            }
            catch
            {
                strUser = "SYS_PROG";
                strPosition = "SYSTEM PROGRAMMER";
            }

            frmReport.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLegal; // AST 20160126 rem
            //frmReport.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprUser;
            frmReport.axVSPrinter1.FontName = "Arial";
            frmReport.axVSPrinter1.MarginTop = 500;

            strReportTitle = "BUSINESS ROLL BY TOP ASSESSED TAX FOR " + iYear;

            frmReport.ReportTitle = strReportTitle;


            result.Query = string.Format("delete from rep_bnsrol_top where report_name = '{0}' and user_code = '{1}'", strReportTitle, strUser);
            result.ExecuteNonQuery();

            result.Query = string.Format("delete from gen_info where report_name = '{0}'", strReportTitle);
            result.ExecuteNonQuery();

            if (frmBusinessRoll.chkByPercentage.Checked)
                strTmpPR = strGross + "%PR";
            else
                strTmpPR = strGross + "PR";

            result.Query = string.Format("delete from top_grosspayer_tbl where tax_year = '{0}' and top = '{1}'", strTaxYear, strTmpPR);
            result.ExecuteNonQuery();

            result.Query = string.Format("insert into gen_info values('{0}',to_date('{1}', 'MM/dd/yyyy'), '{2}',{3},'{4}')", strReportTitle, strCurrentDate, strUser, 1, "ASS");
            result.ExecuteNonQuery();

            if (strBnsBrgy == "" || strBnsBrgy == "ALL")
                strBnsBrgy = "%%";

            if (strBnsCode == "")
                strBnsCode = "%%";
            else
                strBnsCode = strBnsCode + "%";

            if (strTaxYear.Trim() == "")
            {
                //AFM 20200218 added num_employees
                result.Query = "select businesses.bin,sum(or_table.fees_amtdue) as amt_paid, bns_nm, businesses.bns_stat, businesses.orgn_kind, ";
                result.Query += "bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,businesses.or_no,businesses.num_employees ";
                result.Query += "from businesses, pay_hist, or_table where businesses.bin = pay_hist.bin and pay_hist.or_no = or_table.or_no and trim(businesses.bns_code) like '"+ strBnsCode +"' ";
                result.Query += "and businesses.bns_brgy like '"+ strBnsBrgy +"' and pay_hist.bns_stat = businesses.bns_stat ";
                result.Query += "group by businesses.bin, bns_nm, businesses.bns_stat, businesseses.orgn_kind,bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,businesses.or_no,businesses.num_employees ";
                result.Query += "order by amt_paid desc";
            }
            else
            {
                //AFM 20200218 added num_employees
                result.Query = "select businesses.bin,sum(or_table.fees_amtdue) as amt_paid, bns_nm, businesses.bns_stat, businesses.orgn_kind, ";
                result.Query += "bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,businesses.or_no,businesses.num_employees ";
                result.Query += "from businesses, pay_hist, or_table where businesses.bin = pay_hist.bin and pay_hist.or_no = or_table.or_no and trim(businesses.bns_code) like '"+ strBnsCode +"' ";
                result.Query += "and businesses.bns_brgy like '"+ strBnsBrgy +"' and businesses.tax_year = '"+ strTaxYear +"' and pay_hist.tax_year = '"+ strTaxYear +"' and pay_hist.bns_stat = businesses.bns_stat ";
                result.Query += "group by businesses.bin, bns_nm, businesses.bns_stat, businesses.orgn_kind, bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,businesses.or_no,businesses.num_employees ";
                result.Query += "union ";
                result.Query += "select buss_hist.bin,sum(or_table.fees_amtdue) as amt_paid, bns_nm, buss_hist.bns_stat, buss_hist.orgn_kind, ";
                result.Query += "bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,buss_hist.or_no,buss_hist.num_employees ";
                result.Query += "from buss_hist, pay_hist, or_table where buss_hist.bin = pay_hist.bin and pay_hist.or_no = or_table.or_no and trim(buss_hist.bns_code) like '"+ strBnsCode +"' ";
                result.Query += "and buss_hist.bns_brgy like '"+ strBnsBrgy +"' and buss_hist.tax_year = '"+strTaxYear+"' and pay_hist.tax_year = '"+ strTaxYear +"' ";
                result.Query += "and pay_hist.bns_stat = buss_hist.bns_stat and buss_hist.bin not in (select bin from businesses where tax_year = '"+ strTaxYear +"') ";
                result.Query += "group by buss_hist.bin, bns_nm, buss_hist.bns_stat, buss_hist.orgn_kind, bns_code,own_code,gr_1,gr_2,capital,dt_operated,permit_no,permit_dt,buss_hist.or_no,buss_hist.num_employees ";
                result.Query += "order by amt_paid desc";
            }
            /* REM MCR 20140804
            if (result.Execute())
            {
                while (result.Read())
                {
                    dRecordCnt++;
                }
            }
            result.Close();
            */
            #region Progress
            int intCount = 0;
            int intCountIncreament = 0;
            frmReport.m_objThread = new Thread(frmReport.ThreadProcess);
            frmReport.m_objThread.Start();
            Thread.Sleep(500);
            intCount = Convert.ToInt32(strGross);

            frmReport.DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, frmReport.m_form.ProgressBarWork);
            #endregion

            if (frmBusinessRoll.chkByPercentage.Checked)
            {
                double dTmpTopGross = double.Parse(strGross);
                double dPercent = (dRecordCnt * dTmpTopGross) / 100;
                strTopGrossTmp = string.Format("{0:###}", dPercent);
            }
            else
                strTopGrossTmp = strGross;

            frmReport.BusinessType = strBnsType;
            frmReport.Gross = strTopGrossTmp;
            frmReport.TaxYear = strTaxYear;

            frmReport.axVSPrinter1.StartDoc();
            frmReport.CreateHeader(); //MCR 20140618

            frmReport.axVSPrinter1.FontSize = 8.0f;

            frmReport.axVSPrinter1.Paragraph = "";
            frmReport.axVSPrinter1.FontBold = true;
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
            //frmReport.axVSPrinter1.Table = String.Format("<17300;{0}", "Barangay: " + frmBusinessRoll.cboBarangay.Text);
            frmReport.axVSPrinter1.Table = String.Format("<18650;{0}", "Barangay: " + frmBusinessRoll.cboBarangay.Text); //AFM 20200218
            frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            frmReport.axVSPrinter1.FontBold = false;

            if (result.Execute())
            {
                while (result.Read())
                {
                    sCurrBin = result.GetString("bin"); //JARS 20170714
                    if (sCurrBin != sPrevBin) //JARS 201707
                    {
                        frmReport.DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, frmReport.m_form.ProgressBarWork);
                        intCountIncreament += 1;
                        object objData = "";
                        string strGrossCapital = ""; // JHMN 20170309 added
                        strBnsName = result.GetString("bns_nm");
                        //strBnsAddr = AppSettingsManager.GetBnsDesc(result.GetString("bns_code"));
                        strBnsAddr = AppSettingsManager.GetBnsAddress(result.GetString("bin")); //AFM 20200218
                        strOwnCode = result.GetString("own_code");
                        strOwnName = AppSettingsManager.GetBnsOwner(strOwnCode);
                        strOwnAddr = AppSettingsManager.GetBnsOwnAdd(strOwnCode);
                        strBnsDesc = AppSettingsManager.GetBnsDesc(result.GetString("bns_code"));
                        strBnsOrgn = result.GetString("orgn_kind");
                        //gr1 = result.GetDouble("gr_1");
                        //gr2 = result.GetDouble("gr_2");
                        //dGrossReceipt = gr1 + gr2;
                        strGrossCapital = AppSettingsManager.GetCapitalGross(result.GetString("bin"), result.GetString("bns_code"), strTaxYear, result.GetString("bns_stat")).ToString(); // JHMN 20170309 to get latest gross declaration
                        dGrossReceipt = Convert.ToDouble(strGrossCapital);
                        dCapital = result.GetDouble("capital");
                        iEmpCnt = result.GetInt("num_employees"); //AFM 20200218


                        // JAV 20170824 (s)

                        double dAssTax = 0;  
                        rs.Query = string.Format("select sum(amount) as BtaxAmtDue from  taxdues where tax_code = 'B' and bin = '{0}' and tax_year = '{1}'",result.GetString("bin"),strTaxYear);
                        if (rs.Execute())
                        {
                            while (rs.Read())
                            {
                                dAssTax = rs.GetDouble("BtaxAmtDue");
                            }
                        }
                        rs.Close();
                        if (dAssTax == 0)
                        {
                            rs.Query = string.Format("select distinct * from taxdues_hist a where bin = '{0}' and tax_year = '{1}' and tax_code = 'B'", result.GetString("bin"), strTaxYear);
                            if (rs.Execute())
                            {
                                while (rs.Read())
                                {
                                    dAssTax += rs.GetDouble("amount");
                                }
                            }
                            rs.Close();
                        }
                        // JAV 20170824 (e)

                        rs.Query = string.Format(@"select sum(fees_amtdue) as BtaxAmtDue from or_table where or_no in 
                        (select or_no from pay_hist where bin = '{0}' and tax_year = '{1}')",
                            result.GetString("bin"), strTaxYear);
                        if (rs.Execute())
                        {
                            while (rs.Read())
                            {
                                dBTaxAmtDue = rs.GetDouble("BtaxAmtDue");
                            }
                        }
                        rs.Close();

                        // RMC 20150806 added mode of payment & no. of qtrs paid in Business Roll Top Payers report (s)
                        string sMode = string.Empty;
                        string sNoQtrsPaid = string.Empty;
                        int iTmp = 0;
                        int iMode = 0;
                        int iNoQtsPaid = 0;

                        rs.Query = "select qtr_paid, no_of_qtr from pay_hist where bin = '" + result.GetString("bin") + "' and ";
                        rs.Query += " tax_year = '" + strTaxYear + "' and qtr_paid <> 'X' order by qtr_paid desc, no_of_qtr desc";
                        if (rs.Execute())
                        {
                            if (rs.Read())
                            {
                                sMode = rs.GetString("qtr_paid");
                                sNoQtrsPaid = rs.GetString("no_of_qtr");

                                if (sMode == "F")
                                    sNoQtrsPaid = "4";
                                else
                                {
                                    // RMC 20170227 correction in Business Roll (s)
                                    if (sNoQtrsPaid == "")
                                    {
                                        sNoQtrsPaid = sMode; //MOD MCR 20170519 1 to sMode
                                        sMode = "Q";
                                    }
                                    else// RMC 20170227 correction in Business Roll (e)
                                    {
                                        int.TryParse(sMode, out iMode);
                                        int.TryParse(sNoQtrsPaid, out iNoQtsPaid);
                                        iTmp = iMode + iNoQtsPaid - 1;
                                        if (iTmp < 0)
                                            iTmp = 1;

                                        sMode = "Q";
                                        sNoQtrsPaid = string.Format("{0:###}", iTmp);
                                    }
                                }
                            }
                        }
                        rs.Close();
                        // RMC 20150806 added mode of payment & no. of qtrs paid in Business Roll Top Payers report (e)

                        objData = result.GetString("bin") + "|";
                        //objData += strBnsName + "/" + strBnsAddr + "|";
                        //objData += strOwnName + "/" + strOwnAddr + "|";
                        objData += strBnsName + "|";
                        objData += strBnsAddr + "|";
                        objData += strOwnName + "|";
                        objData += strOwnAddr + "|";
                        objData += strBnsDesc + "/" + "REN" + "/" + strBnsOrgn + "|";
                        objData += string.Format("{0:#,###.00}\n{1:#,###.00}", dGrossReceipt, dCapital) + "|";
                        objData += result.GetDateTime("dt_operated").ToShortDateString() + "|";
                        objData += result.GetString("permit_no") + "|";
                        objData += result.GetDateTime("permit_dt").ToShortDateString() + "|";
                        objData += string.Format("{0:#,###.00}", dBTaxAmtDue) + "|";
                        objData += string.Format("{0:#,###.00}", dAssTax) + "|";
                        objData += iEmpCnt.ToString(); //AFM 20200218
                        //<2500|<3000|<3000|<3000|>2000|^1500|^1500|^1500|>1000
                        //frmReport.axVSPrinter1.Table = string.Format("<2500|<3000|<3000|<3000|>1900|^1200|^1400|^1300|>1700;;{0}", objData);

                        // RMC 20150806 added mode of payment & no. of qtrs paid in Business Roll Top Payers report (s) 
                        //objData += "|" + sMode + "|" + sNoQtrsPaid;
                        
                        //frmReport.axVSPrinter1.Table = string.Format("<1250|<2500|<2500|<2500|>1900|^1200|^1400|^1300|>1500|^700|^500;;{0}", objData); //JAV 20170824
                        //frmReport.axVSPrinter1.Table = string.Format("<1250|<2500|<2500|<2500|>1900|^1200|^1400|^1300|^1200|>1500;{0}", objData); //JAV 20170824
                        frmReport.axVSPrinter1.Table = string.Format("<1250|<1700|<1700|<1700|<1700|<1700|>1800|^1200|^1400|^1300|^1200|>1500|^500;{0}", objData); //AFM 20200218
                        // RMC 20150806 added mode of payment & no. of qtrs paid in Business Roll Top Payers report (e)

                        if (m_bViewOnly == false) //MCR 20140602
                            frmBusinessRoll.UpdateBnsRollReport(strHeading, "", strBnsBrgy, result.GetString("bin"), strBnsName, strBnsAddr,
                                  strOwnName, strOwnAddr, strBnsDesc, "REN", "0", strGross,
                                   result.GetDateTime("dt_operated").ToShortDateString(), result.GetString("permit_no"), result.GetDateTime("permit_dt").ToShortDateString(),
                                  dBTaxAmtDue.ToString(), "", frmReport.Text, strUser, strPosition, "", 0);

                        frmReport.DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, frmReport.m_form.ProgressBarWork);
                        Thread.Sleep(3);
                        iTotalBns++;

                        if (iTotalBns.ToString() == strGross)
                            break;
                        sPrevBin = result.GetString("bin");
                    }
                }

                frmReport.axVSPrinter1.Paragraph = "";
                frmReport.axVSPrinter1.FontBold = true;
                frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
                //frmReport.axVSPrinter1.Table = String.Format("<17300;{0}", "Total No of Businesses: " + iTotalBns);
                frmReport.axVSPrinter1.Table = String.Format("<18600;{0}", "Total No of Businesses: " + iTotalBns);
                frmReport.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                frmReport.axVSPrinter1.FontBold = false;

            }
            result.Close();

            frmReport.DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, frmReport.m_form.ProgressBarWork);
            Thread.Sleep(10);
            
            frmReport.axVSPrinter1.EndDoc();
        }

    }
}