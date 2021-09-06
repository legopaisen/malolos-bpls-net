using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.Common.Reports
{
    public partial class frmCompromiseAgreement : Form
    {
        public frmCompromiseAgreement()
        {
            InitializeComponent();
        }

        private string m_sQuery = String.Empty;
        private string m_sReportTitle = String.Empty;
        private string m_sFromDate = String.Empty;
        private string m_sToDate = String.Empty;
        private string m_sUser = String.Empty;
        private int m_iAbstractFormat = 0;
        private int m_iDebitFormat = 0;
        private int m_iGroupFormat = 0;
        private bool m_bInit = true;

        public string Query
        {
            set { m_sQuery = value; }
        }
        public string ReportTitle
        {
            set { m_sReportTitle = value; }
        }
        public string FromDate
        {
            set { m_sFromDate = value; }
        }
        public string ToDate
        {
            set { m_sToDate = value; }
        }
        public string User
        {
            set { m_sUser = value; }
        }
        public int DebitFormat
        {
            set { m_iDebitFormat = value; }
        }
        public int AbstractFormat
        {
            set { m_iAbstractFormat = value; }
        }
        public int GroupFormat
        {
            set { m_iGroupFormat = value; }
        }

        String sContentSize, sContent;

        private void frmCompromiseAgreement_Load(object sender, EventArgs e)
        {
            if (m_sReportTitle == "LIST OF COMPROMISE AGREEMENT")
            {
                CompromistAgreement();
                this.Text = "Compromise Agreement";
            }
            else if (m_sReportTitle == "DEBIT CREDIT REPORTS")
            {
                DebitCreditReport();
                this.Text = "Debit Credit Reports";
            }
            else if (m_sReportTitle.Contains("ABSTRACT OF CANCELLED RECEIPTS"))
                AbstractofCancelledReceipts();
            else if (m_sReportTitle.Contains("ABSTRACT OF EXCESS OF CHECKS"))
                AbstractofExcessofChecks();
            else if (m_sReportTitle.Contains("ABSTRACT OF DEBITED CREDIT MEMOS"))
                AbstractofDebitedCreditMemos();
            else if (m_sReportTitle.Contains("ABSTRACT OF EXCESS OF TAX CREDIT"))
                AbstractofExcessofTaxCredit();
            else if (m_sReportTitle.Contains("RECEIVABLES"))
                ListOfCollectibles();
            else if (m_sReportTitle.Contains("COLLECTIBLES"))
            {
                ExportFile();
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
                axVSPrinter1.FontName = "Arial";
                axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; // 0 portrait 1 landscape
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                axVSPrinter1.MarginLeft = 700;
                axVSPrinter1.MarginTop = 500;

                m_bInit = false;
                CreateHeader();
                SummaryOfCollectibles();
            }
            else if (m_sReportTitle == "LIST OF DECLARED OR") //JARS 20160621
            {
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
                axVSPrinter1.FontName = "Arial";
                axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; // 0 portrait 1 landscape
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                ListDeclaredOR();
            }
            else if (m_sReportTitle == "LIST OF BUSINESSES WITH WAIVED SURCHARGE AND PENALTY")
            {
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
                axVSPrinter1.FontName = "Arial";
                axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; // 0 portrait 1 landscape
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                WaiveSurchPen();
                this.Text = "List of Businesses with Waived Surcharge and Penalty";
            }

            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private string ComputeReg(String sBin, String sTaxYear, bool bFormat)
        {
            String sQuery, sRegFee = "";
            OracleResultSet pSet = new OracleResultSet();

            if (!bFormat)
                sQuery = "select sum(fees_amt) as regdue from rep_list_coll where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "' and user_code = '" + m_sUser + "' and report_name = '" + m_sReportTitle + "'";
            else
                sQuery = "select sum(fees_amt) as regdue from rep_list_coll where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "' and user_code = '" + m_sUser + "' and report_name = '" + m_sReportTitle + "'";

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sRegFee = pSet.GetDouble("regdue").ToString();
                }
            pSet.Close();

            return sRegFee;
        }

        private string ComputeSurchPen(String sBin, String sTax, bool bFormat)
        {
            String sQuery, sChargeAmt = "";

            OracleResultSet pSet = new OracleResultSet();

            if (!bFormat)
                sQuery = "select sum(charges_amt) as chargeamt from rep_list_coll where bin = '" + sBin + "' and tax_year = '" + sTax + "' and user_code = '" + m_sUser + "' and report_name = '" + m_sReportTitle + "'";
            else
                sQuery = "select sum(charges_amt) as chargeamt from rep_list_coll where bin = '" + sBin + "' and tax_year = '" + sTax + "' and user_code = '" + m_sUser + "' and report_name = '" + m_sReportTitle + "'";

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sChargeAmt = pSet.GetDouble("chargeamt").ToString();
                }
            pSet.Close();

            return sChargeAmt;
        }

        private string ComputeTax(String sBin, String sTaxYear, bool bFormat)
        {
            String sTaxDue = "", sQuery;

            OracleResultSet pSet = new OracleResultSet();

            if (!bFormat)
                sQuery = "select sum(tax_amt) as taxdue from rep_list_coll where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "' and user_code = '" + m_sUser + "' and report_name = '" + m_sReportTitle + "'";
            else
                sQuery = "select sum(tax_amt) as taxdue from rep_list_coll where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "' and user_code = '" + m_sUser + "' and report_name = '" + m_sReportTitle + "'";
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sTaxDue = pSet.GetDouble("taxdue").ToString();
                }
            pSet.Close();

            return sTaxDue;
        }

        private void ListOfCollectibles()
        {
            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA3;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;
            axVSPrinter1.MarginLeft = 700;
            axVSPrinter1.MarginBottom = 700;
            axVSPrinter1.MarginTop = 700;
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            axVSPrinter1.FontName = "Arial";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();
            OracleResultSet pSet3 = new OracleResultSet();

            int iCtr = 0;
            String sQuery="", sContactNo="", sNoEmp = "0", sHead, sSubGroup, sBnsAdd, sOwnerAdd, sQtr;
            String sGroup1, sBin, sBnsName, sOwnerName, sMainBns, sStat, sDate, sTaxYear;
            String sC1st, sC2nd, sC3rd, sC4th, sCRegFees, sCSurchPen, sCTotal;
            String sP1st = "0", sP2nd = "0", sP3rd = "0", sP4th = "0", sPRegFees = "0", sPSurchPen = "0", sPTotal = "0";
            String sNoBns, sTotalNoBns, sGrandTotal, sPYear, sContentTotal;

            double dNoBns, dTotalNoBns, dGrandTotal;
            double fCTot1st = 0, fCTot2nd = 0, fCTot3rd = 0, fCTot4th = 0, fCTotRegFees = 0, fCTotSurchPen = 0, fCTotTotal = 0;
            double fPTot1st = 0, fPTot2nd = 0, fPTot3rd = 0, fPTot4th = 0, fPTotRegFees = 0, fPTotSurchPen = 0, fPTotTotal = 0;
            double dCRegFees, dCSurchPen, dCTotal, dPRegFees, dPSurchPen, dPTotal;
            double dC1st, dP1st, dC2nd, dP2nd, dC3rd, dP3rd, dC4th, dP4th;

            String sBlank, sContent, sHeader, sContent2;

            String sCurrentYear = AppSettings.AppSettingsManager.GetConfigValue("12");

            pSet.Query = "select distinct bin from rep_list_coll where report_name = '" + m_sReportTitle + "' and user_code like '" + m_sUser + "' order by bin asc";
            if (pSet.Execute())
            {
                sContentTotal = ">9200|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>700|>700|>700|>700|>700|>700|>700;";
                sContent = "<1200|<2400|<1500|<1800|^600|<1100|^600|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>700|>700|>700|>700|>700|>700|>700;";

                while (pSet.Read())
                {
                    iCtr = iCtr + 1;
                    dNoBns = 0;
                    dTotalNoBns = 0;
                    dGrandTotal = 0;
                    dCRegFees = 0;
                    dCSurchPen = 0;
                    dCTotal = 0;
                    dPRegFees = 0;
                    dPSurchPen = 0;
                    dPTotal = 0;
                    dC1st = 0;
                    dP1st = 0;
                    dC2nd = 0;
                    dP2nd = 0;
                    dC3rd = 0;
                    dP3rd = 0;
                    dC4th = 0;
                    dP4th = 0;
                    sBin = pSet.GetString("bin").Trim();

                    pSet1.Query = "select * from rep_list_coll where bin = '" + sBin + "' and report_name = '" + m_sReportTitle + "' and user_code like '" + m_sUser + "' order by tax_year desc, qtr_to_pay desc ";
                    if (pSet1.Execute())
                        if (pSet1.Read())
                        {
                            sHead = StringUtilities.StringUtilities.HandleApostrophe(pSet1.GetString("heading").Trim());
                            sGroup1 = StringUtilities.StringUtilities.HandleApostrophe(pSet1.GetString("data_group").Trim());
                            sSubGroup = StringUtilities.StringUtilities.HandleApostrophe(pSet1.GetString("subgroup1").Trim());

                            sBnsName = StringUtilities.StringUtilities.HandleApostrophe(pSet1.GetString("bns_nm").Trim());
                            sBnsAdd = StringUtilities.StringUtilities.HandleApostrophe(pSet1.GetString("bns_addr").Trim());
                            sOwnerName = StringUtilities.StringUtilities.HandleApostrophe(pSet1.GetString("owner_name").Trim());
                            sOwnerAdd = StringUtilities.StringUtilities.HandleApostrophe(pSet1.GetString(7).Trim());
                            sMainBns = StringUtilities.StringUtilities.HandleApostrophe(pSet1.GetString("main_bns").Trim());
                            sStat = StringUtilities.StringUtilities.HandleApostrophe(pSet1.GetString("bns_stat").Trim());
                            sDate = StringUtilities.StringUtilities.HandleApostrophe(pSet1.GetString("last_dt_pay").Trim());
                            sTaxYear = StringUtilities.StringUtilities.HandleApostrophe(pSet1.GetString("tax_year").Trim());

                            pSet2.Query = "select bns_telno, num_employees from businesses where bin = '" + sBin + "'";
                            if (pSet2.Execute())
                                if (pSet2.Read())
                                {
                                    sContactNo = StringUtilities.StringUtilities.HandleApostrophe(pSet2.GetString("bns_telno").Trim());
                                    sNoEmp = pSet2.GetInt("num_employees").ToString();
                                }
                                else
                                {
                                    pSet2.Close();
                                    pSet2.Query = "select bns_telno, num_employees from business_que where bin = '" + sBin + "'";
                                    if (pSet2.Execute())
                                        if (pSet2.Read())
                                        {
                                            sContactNo = StringUtilities.StringUtilities.HandleApostrophe(pSet2.GetString("bns_telno").Trim());
                                            sNoEmp = pSet2.GetInt("num_employees").ToString();
                                        }
                                }
                            pSet2.Close();

                            if (sNoEmp == "0" || sNoEmp == "" || sNoEmp == " " || Convert.ToInt32(sNoEmp) > 10000)
                                sNoEmp = "1";
                            if (sContactNo == "" || sContactNo == " ")
                                sContactNo = "_______";

                            if (sTaxYear != AppSettings.AppSettingsManager.GetConfigValue("12"))
                            {
                                if ((Convert.ToInt32(sTaxYear) <= (Convert.ToInt32(sCurrentYear) - 4)) && sTaxYear != sCurrentYear)
                                {
                                    sP4th = ComputeTax(sBin, sTaxYear, true);
                                    sPRegFees = ComputeReg(sBin, sTaxYear, true);
                                    dP4th += Convert.ToDouble(sP4th);
                                    dPRegFees += Convert.ToDouble(sPRegFees);
                                    sPSurchPen = ComputeSurchPen(sBin, sTaxYear, true);
                                    dPSurchPen += Convert.ToDouble(sPSurchPen);

                                }
                                if ((Convert.ToInt32(sTaxYear) == (Convert.ToInt32(sCurrentYear) - 3)) && sTaxYear != sCurrentYear)
                                {
                                    sP3rd = ComputeTax(sBin, sTaxYear, false);
                                    sPRegFees = ComputeReg(sBin, sTaxYear, false);
                                    dP3rd = dP3rd + Convert.ToDouble(sP3rd);
                                    dPRegFees = dPRegFees + Convert.ToDouble(sPRegFees);
                                    sPSurchPen = ComputeSurchPen(sBin, sTaxYear, true);
                                    dPSurchPen = dPSurchPen + Convert.ToDouble(sPSurchPen);
                                }
                                if ((Convert.ToInt32(sTaxYear) == (Convert.ToInt32(sCurrentYear) - 2)) && sTaxYear != sCurrentYear)
                                {
                                    sP2nd = ComputeTax(sBin, sTaxYear, false);
                                    sPRegFees = ComputeReg(sBin, sTaxYear, false);
                                    dP2nd = dP2nd + Convert.ToDouble(sP2nd);
                                    dPRegFees = dPRegFees + Convert.ToDouble(sPRegFees);
                                    sPSurchPen = ComputeSurchPen(sBin, sTaxYear, true);
                                    dPSurchPen = dPSurchPen + Convert.ToDouble(sPSurchPen);
                                }
                                if ((Convert.ToInt32(sTaxYear) == (Convert.ToInt32(sCurrentYear) - 1)) && sTaxYear != sCurrentYear)
                                {
                                    sP1st = ComputeTax(sBin, sTaxYear, false);
                                    sPRegFees = ComputeReg(sBin, sTaxYear, false);
                                    dP1st = dP1st + Convert.ToDouble(sP1st);
                                    dPRegFees = dPRegFees + Convert.ToDouble(sPRegFees);
                                    sPSurchPen = ComputeSurchPen(sBin, sTaxYear, true);
                                    dPSurchPen = dPSurchPen + Convert.ToDouble(sPSurchPen);
                                }

                                sC1st = "0.00";
                                sC2nd = "0.00";
                                sC3rd = "0.00";
                                sC4th = "0.00";
                                sCSurchPen = "0.00";
                                sCRegFees = "0.00";
                                sCTotal = "0.00";

                                dPTotal = dP1st + dP2nd + dP3rd + dP4th + Convert.ToDouble(sPSurchPen) + dPRegFees; //dCRegFees AFM 20210120 MAO-21-14478
                                //sPTotal.Format("%.2f", dPTotal);

                                sContent = "<1200|<2400|<1500|<1800|^600|<1100|^600|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>700|>700|>700|>700|>700|>700|>700;";		// RMC 20120222 modified printing of List of Collectibles
                                sContent += sBin.Trim() + "|" + sBnsName.Trim() + "  /  " + sBnsAdd.Trim() + "|Tel No." + sContactNo + "/ No Emp." + Convert.ToInt32(sNoEmp).ToString("#,###") + "|" + sOwnerName.Trim() + " / " + sMainBns.Trim() + "|" + sStat.Trim() + "|" + sDate.Trim() + "|";
                                sContent += sTaxYear.Trim() + "|" + Convert.ToDouble(sC1st).ToString("#,##0.00") + "|" + Convert.ToDouble(sC2nd).ToString("#,##0.00") + "|" + Convert.ToDouble(sC3rd).ToString("#,##0.00") + "|" + Convert.ToDouble(sC4th).ToString("#,##0.00") + "|" + Convert.ToDouble(sCRegFees).ToString("#,##0.00") + "|";
                                sContent += Convert.ToDouble(sCSurchPen).ToString("#,##0.00") + "|" + Convert.ToDouble(sCTotal).ToString("#,##0.00") + "|" + dP1st.ToString("#,##0.00") + "|" + dP2nd.ToString("#,##0.00") + "|" + dP3rd.ToString("#,##0.00") + "|" + dP4th.ToString("#,##0.00") + "|" + dPRegFees.ToString("#,##0.00") + "|"; //dCRegFees AFM 20210120 MAO-21-14478
                                sContent += Convert.ToDouble(sPSurchPen).ToString("#,##0.00") + "|" + dPTotal.ToString("#,##0.00");

                                axVSPrinter1.Table = sContent;	// RMC 20120222 modified printing of List of Collectibles
                            }

                            if (sTaxYear == sCurrentYear)
                            {
                                pSet2.Query = "select * from rep_list_coll where bin = '" + sBin + "' and user_code = '" + m_sUser + "' and report_name = '" + m_sReportTitle + "'";// and qtr_to_pay = '" + sQtr + "'";
                                if (pSet2.Execute())
                                {
                                    while (pSet2.Read())
                                    {
                                        sQtr = pSet2.GetString("qtr_to_pay").Trim();
                                        if (sQtr == "1")
                                        {
                                            sC1st = pSet2.GetDouble("tax_amt").ToString();
                                            sCRegFees = pSet2.GetDouble("fees_amt").ToString();
                                            dCRegFees += pSet2.GetDouble("fees_amt");
                                            sCSurchPen = pSet2.GetDouble("charges_amt").ToString();
                                            dCSurchPen += pSet2.GetDouble("charges_amt");
                                            dC1st += Convert.ToDouble(sC1st);
                                        }
                                        else if (sQtr == "2")
                                        {
                                            sC2nd = pSet2.GetDouble("tax_amt").ToString();
                                            sCRegFees = pSet2.GetDouble("fees_amt").ToString();
                                            dCRegFees += pSet2.GetDouble("fees_amt");
                                            sCSurchPen = pSet2.GetDouble("charges_amt").ToString();
                                            dCSurchPen += pSet2.GetDouble("charges_amt");
                                            dC2nd += Convert.ToDouble(sC2nd);
                                        }
                                        else if (sQtr == "3")
                                        {
                                            sC3rd = pSet2.GetDouble("tax_amt").ToString();
                                            sCRegFees = pSet2.GetDouble("fees_amt").ToString();
                                            dCRegFees += pSet2.GetDouble("fees_amt");
                                            sCSurchPen = pSet2.GetDouble("charges_amt").ToString();
                                            dCSurchPen += pSet2.GetDouble("charges_amt");
                                            dC3rd += Convert.ToDouble(sC3rd);
                                        }
                                        else if (sQtr == "4")
                                        {
                                            sC4th = pSet2.GetDouble("tax_amt").ToString();
                                            sCRegFees = pSet2.GetDouble("fees_amt").ToString();
                                            dCRegFees += pSet2.GetDouble("fees_amt");
                                            sCSurchPen = pSet2.GetDouble("charges_amt").ToString();
                                            dCSurchPen += pSet2.GetDouble("charges_amt");
                                            dC4th += Convert.ToDouble(sC4th);
                                        }
                                        else
                                        {
                                            sCTotal = pSet2.GetDouble("tax_amt").ToString();
                                            sCRegFees = pSet2.GetDouble("fees_amt").ToString();
                                            dCRegFees += pSet2.GetDouble("fees_amt");
                                            sCSurchPen = pSet2.GetDouble("charges_amt").ToString();
                                            dCSurchPen += pSet2.GetDouble("charges_amt");
                                            dCTotal += Convert.ToDouble(sCTotal);
                                        }
                                    }

                                    //sC1st.Format("%.2f", dC1st);
                                    //sC2nd.Format("%.2f", dC2nd);
                                    //sC3rd.Format("%.2f", dC3rd);
                                    //sC4th.Format("%.2f", dC4th);
                                    //sCSurchPen.Format("%.2f", dCSurchPen);
                                    //sCRegFees.Format("%.2f", dCRegFees);

                                    sP1st = "0.00";
                                    sP2nd = "0.00";
                                    sP3rd = "0.00";
                                    sP4th = "0.00";
                                    sPRegFees = "0.00";
                                    sPSurchPen = "0.00";
                                    sPTotal = "0.00";

                                    dCTotal = dC1st + dC2nd + dC3rd + dC4th + dCSurchPen + dCRegFees;
                                }
                                pSet2.Close();

                                sContent = "<1200|<2400|<1500|<1800|^600|<1100|^600|>1200|>1200|>1200|>1200|>1200|>1200|>1200|>700|>700|>700|>700|>700|>700|>700;";		// RMC 20120222 modified printing of List of Collectibles
                                sContent += sBin.Trim() + "|" + sBnsName.Trim() + "  /  " + sBnsAdd.Trim() + "|Tel No." + sContactNo + "/ No Emp." + sNoEmp + "|" + sOwnerName.Trim() + " / " + sMainBns.Trim() + "|" + sStat.Trim() + "|" + sDate.Trim() + "|";
                                sContent += sTaxYear.Trim() + "|" + dC1st.ToString("#,##0.00") + "|" + dC2nd.ToString("#,##0.00") + "|" + dC3rd.ToString("#,##0.00") + "|" + dC4th.ToString("#,##0.00") + "|" + dCRegFees.ToString("#,##0.00") + "|";
                                sContent += dCSurchPen.ToString("#,##0.00") + "|" + dCTotal.ToString("#,##0.00") + "|" + Convert.ToDouble(sP1st).ToString("#,##0.00") + "|" + Convert.ToDouble(sP2nd).ToString("#,##0.00") + "|" + Convert.ToDouble(sP3rd).ToString("#,##0.00") + "|" + Convert.ToDouble(sP4th).ToString("#,##0.00") + "|" + Convert.ToDouble(sPRegFees).ToString("#,##0.00") + "|";
                                sContent += Convert.ToDouble(sPSurchPen).ToString("#,##0.00") + "|" + Convert.ToDouble(sPTotal).ToString("#,##0.00");

                                axVSPrinter1.Table = sContent;	// RMC 20120222 modified printing of List of Collectibles
                            }
                        }
                    pSet1.Close();

                    fCTot1st = fCTot1st + dC1st;
                    fCTot2nd = fCTot2nd + dC2nd;
                    fCTot3rd = fCTot3rd + dC3rd;
                    fCTot4th = fCTot4th + dC4th;
                    fCTotRegFees += dCRegFees;
                    fCTotSurchPen += dCSurchPen;

                    fCTotTotal += dCTotal;
                    fPTot1st += dP1st;
                    fPTot2nd += dP2nd;
                    fPTot3rd += dP3rd;
                    fPTot4th += dP4th;
                    fPTotRegFees += dPRegFees;
                    fPTotSurchPen += dPSurchPen;
                    fPTotTotal += dPTotal;
                }

                sContentTotal += "Grand Total|" + fCTot1st.ToString("#,##0.00") + "|" + fCTot2nd.ToString("#,##0.00") + "|" + fCTot3rd.ToString("#,##0.00") + "|" + fCTot4th.ToString("#,##0.00") + "|" + fCTotRegFees.ToString("#,##0.00") + "|" + fCTotSurchPen.ToString("#,##0.00") + "|" + fCTotTotal.ToString("#,##0.00") + "|";
                sContentTotal += fPTot1st.ToString("#,##0.00") + "|" + fPTot2nd.ToString("#,##0.00") + "|" + fPTot3rd.ToString("#,##0.00") + "|" + fPTot4th.ToString("#,##0.00") + "|" + fPTotRegFees.ToString("#,##0.00") + "|" + fPTotSurchPen.ToString("#,##0.00") + "|" + fPTotTotal.ToString("#,##0.00");

                axVSPrinter1.FontBold = true;
                axVSPrinter1.Table = sContentTotal;
                axVSPrinter1.FontBold = false;
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.Table = ("<21000;Total Number of Businesses:  " + iCtr.ToString("#,##0"));
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
            }
            pSet.Close();
        }

        private void SummaryOfCollectibles()
        {
            String sHeader, sContent, sQuery, sBinTemp = String.Empty, sBin = String.Empty;
            String sDataGroup = String.Empty, sTemp = String.Empty;
            double dCount = 0, dTax = 0, dFee = 0, dCharge = 0, dTotal = 0;
            double dSubTotalTax = 0, dSubTotalFee = 0, dSubTotalCharge = 0, dSubTotalTotal = 0;
            double dTotalCount = 0, dTotalTax = 0, dTotalFee = 0, dTotalCharge = 0, dTotalTotal = 0;

            /*axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            axVSPrinter1.FontName = "Arial";
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; // 0 portrait 1 landscape
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.MarginLeft = 700;
            axVSPrinter1.MarginTop = 500;
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;*/

            OracleResultSet pSet = new OracleResultSet();
            try
            {
                pSet.Query = m_sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        //sContent = "<1700|>700|>2000|>2000|>2000|>2000;";
                        sBin = pSet.GetString("bin").Trim();
                        // RMC 20161209 correction in top collectibles (s)
                        if (m_sReportTitle.ToString().Contains("TOP"))
                        {
                            sContent = "<2400|>2000|>2000|>2000|>2000;";
                            sDataGroup = sBin;
                        }
                        else
                        {
                            // RMC 20161209 correction in top collectibles (e)
                            sContent = "<1700|>700|>2000|>2000|>2000|>2000;";
                            sDataGroup = pSet.GetString("data_group").Trim();
                        }
                        dTax = pSet.GetDouble("tax_amt");
                        dFee = pSet.GetDouble("fees_amt");
                        dCharge = pSet.GetDouble("charges_amt");
                        dTotal = dTax + dFee + dCharge;


                        dTotalTax += dTax;
                        dTotalCharge += dCharge;
                        dTotalFee += dFee;
                        dTotalTotal += dTotal;

                        if (sTemp == "")
                        {
                            dCount++;
                            dTotalCount++;
                            sTemp = sDataGroup;
                            sBinTemp = sBin;
                            dSubTotalTax += dTax;
                            dSubTotalCharge += dCharge;
                            dSubTotalFee += dFee;
                            dSubTotalTotal += dTotal;
                        }
                        else if (sTemp != sDataGroup)
                        {
                            // RMC 20161209 correction in top collectibles (s)
                            if (m_sReportTitle.ToString().Contains("TOP"))
                            {
                                sContent += sTemp + "/" + AppSettingsManager.GetBnsName(sTemp) + "|" + dSubTotalTax.ToString("#,##0.00") + "|" + dSubTotalFee.ToString("#,##0.00") + "|" + dSubTotalCharge.ToString("#,##0.00") + "|" + dSubTotalTotal.ToString("#,##0.00") + ";";
                            }// RMC 20161209 correction in top collectibles (e)
                            else
                                sContent += sTemp + "|" + dCount.ToString("#,##0") + "|" + dSubTotalTax.ToString("#,##0.00") + "|" + dSubTotalFee.ToString("#,##0.00") + "|" + dSubTotalCharge.ToString("#,##0.00") + "|" + dSubTotalTotal.ToString("#,##0.00") + ";";
                            this.axVSPrinter1.Table = sContent;
                            sTemp = sDataGroup;

                            dCount = 0;

                            sDataGroup = pSet.GetString("data_group").Trim();
                            dSubTotalTax = dTax;
                            dSubTotalCharge = dCharge;
                            dSubTotalFee = dFee;
                            dSubTotalTotal = dTotal;

                            // RMC 20171116 correction in collectibles rpu count (s)
                            dCount++;
                            dTotalCount++;
                            sBinTemp = sBin;
                            // RMC 20171116 correction in collectibles rpu count (e)
                        }
                        else
                        {
                            if (sBin != sBinTemp)
                            {
                                dCount++;
                                dTotalCount++;
                                sBinTemp = sBin;
                            }

                            dSubTotalTax += dTax;
                            dSubTotalCharge += dCharge;
                            dSubTotalFee += dFee;
                            dSubTotalTotal += dTotal;
                            sTemp = sDataGroup;
                        }
                    }
                }
                else
                {
                    sContent = "";
                }
                pSet.Close();

                // RMC 20161209 correction in top collectibles (s)
                if (m_sReportTitle.ToString().Contains("TOP"))
                    sContent = "<2400|>2000|>2000|>2000|>2000;" + sDataGroup + "|" + dSubTotalTax.ToString("#,##0.00") + "|" + dSubTotalFee.ToString("#,##0.00") + "|" + dSubTotalCharge.ToString("#,##0.00") + "|" + dSubTotalTotal.ToString("#,##0.00") + ";";
                else // RMC 20161209 correction in top collectibles (e)
                    sContent = "<1700|>700|>2000|>2000|>2000|>2000;" + sDataGroup + "|" + dCount.ToString("#,##0") + "|" + dSubTotalTax.ToString("#,##0.00") + "|" + dSubTotalFee.ToString("#,##0.00") + "|" + dSubTotalCharge.ToString("#,##0.00") + "|" + dSubTotalTotal.ToString("#,##0.00") + ";";
                this.axVSPrinter1.Table = sContent;
                this.axVSPrinter1.Paragraph = "";
                // RMC 20161209 correction in top collectibles (s)
                if (m_sReportTitle.ToString().Contains("TOP"))
                    sContent = "^2400|>2000|>2000|>2000|>2000;Total" + "|" + dTotalTax.ToString("#,##0.00") + "|" + dTotalFee.ToString("#,##0.00") + "|" + dTotalCharge.ToString("#,##0.00") + "|" + dTotalTotal.ToString("#,##0.00") + ";";
                else // RMC 20161209 correction in top collectibles (e)
                    sContent = "^1700|>700|>2000|>2000|>2000|>2000;Total" + "|" + dTotalCount.ToString("#,##0") + "|" + dTotalTax.ToString("#,##0.00") + "|" + dTotalFee.ToString("#,##0.00") + "|" + dTotalCharge.ToString("#,##0.00") + "|" + dTotalTotal.ToString("#,##0.00") + ";";
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                this.axVSPrinter1.Table = sContent;
                this.axVSPrinter1.FontBold = false;
            }
            catch { }
        }

        private void AbstractofCancelledReceipts()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            axVSPrinter1.FontName = "Arial";
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; // 0 portrait 1 landscape
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.MarginLeft = 720;
            axVSPrinter1.MarginTop = 500;
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;

            String sQuery, sOrNo, sBin, sMemo, sPayType, sDate, sDateTemp = "", sCashAmt = "0", sChkAmt = "0", sOrTotalAmt = "0", sDebitAmt = "0", sTotal = "0";
            String sTotalCash = "0", sTotalChk = "0";
            double dTotal = 0, dTotalCash = 0, dTotalCheck = 0, dSubTotal = 0, dSubTotalCS = 0, dSubTotalCQ = 0;
            int iCountOR = 0, iTotalCount = 0, iCount = 0;
            String sTotalCount = "0", sSubTotalCS = "0", sSubTotalCQ = "0", sSubTotal = "0";

            if (m_iGroupFormat == 2)
                sContentSize = "^2000|>2000|>2200|>2200|>2200;";
            else
                sContentSize = "^1000|^1500|>1500|>1500|>1500|<3600;";

            if (m_iGroupFormat == 0)
                sQuery = string.Format("select distinct(or_no),bin,memo,payment_type from cancelled_payment where teller = '{0}' and date_posted = to_date('{1}','MM/dd/yyyy')", m_sUser, m_sFromDate);
            else if (m_iGroupFormat == 1)
                sQuery = string.Format("select distinct(or_no),bin,memo,payment_type,date_posted from cancelled_payment where date_posted >= to_date('{0}','MM/dd/yyyy') and date_posted <= to_date('{1}','MM/dd/yyyy')", m_sFromDate, m_sToDate);
            else // 2
                sQuery = string.Format("select distinct date_posted from cancelled_payment where date_posted >= to_date('{0}','MM/dd/yyyy') and date_posted <= to_date('{1}','MM/dd/yyyy') order by date_posted", m_sFromDate, m_sToDate);

            if (m_iGroupFormat != 2)
            {
                #region 0 & 1
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        if (m_iGroupFormat == 1)
                        {
                            sDate = pSet.GetDateTime("date_posted").ToShortDateString();
                            if (sDateTemp != sDate)
                            {
                                axVSPrinter1.Paragraph = "";
                                axVSPrinter1.FontSize = 7;
                                axVSPrinter1.Table = ("<10600;" + sDate);
                            }
                        }

                        sOrNo = pSet.GetString(0);
                        sBin = pSet.GetString("bin");
                        sMemo = pSet.GetString("memo");
                        sPayType = pSet.GetString("payment_type");

                        if (sPayType == "CS")
                        {
                            sChkAmt = "0.00";
                            sQuery = string.Format("select sum(fees_amtdue) as cash_amt from cancelled_or where or_no = '{0}'", sOrNo);
                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                    sCashAmt = pSet1.GetDouble("cash_amt").ToString("0.00");
                            pSet1.Close();

                            sTotal = sCashAmt;
                        }
                        else if (sPayType == "CQ")
                        {
                            sCashAmt = "0.00";
                            sQuery = string.Format("select sum(chk_amt) as check_amt from cancelled_chk where or_no = '{0}'", sOrNo);
                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                    sChkAmt = pSet1.GetDouble("check_amt").ToString("0.00");
                            pSet1.Close();

                            sTotal = sChkAmt;
                        }
                        else if (sPayType == "CC")
                        {
                            sQuery = string.Format("select sum(cash_amt) as cash_amt, sum(chk_amt) as check_amt from cancelled_chk where or_no = '{0}'", sOrNo);
                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                {
                                    sCashAmt = pSet1.GetDouble("cash_amt").ToString("0.00");
                                    sChkAmt = pSet1.GetDouble("check_amt").ToString("0.00");
                                }
                            pSet1.Close();

                            sTotal = Convert.ToDouble(Convert.ToDouble(sCashAmt) + Convert.ToDouble(sChkAmt)).ToString("0.00");
                        }
                        else if (sPayType == "CSTC")
                        {
                            sQuery = string.Format("select sum(fees_amtdue) as total_amt from cancelled_or where or_no = '{0}'", sOrNo);
                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                    sOrTotalAmt = pSet1.GetDouble("total_amt").ToString("0.00");
                            pSet1.Close();

                            //sQuery = string.Format("select sum(debit) as debit_amt from dbcr_memo where or_no = '{0}' and memo like 'REVERS%%'", sOrNo);
                            sQuery = string.Format("select sum(debit) as debit_amt from dbcr_memo where or_no = '{0}' and memo like 'REVERS%%'", sOrNo); //JARS 20171010
                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                    sDebitAmt = pSet1.GetDouble("debit_amt").ToString("0.00");
                            pSet1.Close();

                            sCashAmt = Convert.ToDouble(Convert.ToDouble(sOrTotalAmt) - Convert.ToDouble(sDebitAmt)).ToString("0.00");
                            sTotal = sCashAmt;
                        }
                        else if (sPayType == "TC")
                        {
                            sCashAmt = "0.00";
                            sChkAmt = "0.00";
                            sTotal = "0.00";
                        }

                        dTotalCash = dTotalCash + Convert.ToDouble(sCashAmt);
                        dTotalCheck = dTotalCheck + Convert.ToDouble(sChkAmt);
                        dTotal = dTotal + Convert.ToDouble(sTotal);

                        sContent = sOrNo + "|" + sBin + "|" + Convert.ToDouble(sCashAmt).ToString("#,##0.00") + "|" + Convert.ToDouble(sChkAmt).ToString("#,##0.00") + "|" + Convert.ToDouble(sTotal).ToString("#,##0.00") + "|" + sMemo + "";

                        axVSPrinter1.FontSize = 7;
                        axVSPrinter1.Table = sContentSize + sContent;
                    }
                }
                else
                {
                    MessageBox.Show("No Record(s) Found", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                pSet.Close();
                #endregion
            }
            else
            {
                #region 2
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        sSubTotalCS = "0";
                        sSubTotalCQ = "0";
                        sSubTotal = "0";

                        dSubTotalCS = 0;
                        dSubTotalCQ = 0;
                        dSubTotal = 0;

                        sDate = pSet.GetDateTime("date_posted").ToShortDateString();

                        sQuery = string.Format("select * from cancelled_payment where date_posted = to_date('{0}','MM/dd/yyyy')", sDate);
                        pSet2.Query = sQuery;
                        if (pSet2.Execute())
                            while (pSet2.Read())
                            {
                                sCashAmt = "0";
                                sChkAmt = "0";
                                sTotal = "0";

                                sOrNo = pSet2.GetString("or_no");
                                sPayType = pSet2.GetString("payment_type");

                                if (sPayType == "CS")
                                {
                                    sChkAmt = "0.00";
                                    sQuery = string.Format("select sum(fees_amtdue) as cash_amt from cancelled_or where or_no = '{0}'", sOrNo);
                                    pSet1.Query = sQuery;
                                    if (pSet1.Execute())
                                        if (pSet1.Read())
                                            sCashAmt = pSet1.GetDouble("cash_amt").ToString("0.00");
                                    pSet1.Close();

                                    sTotal = sCashAmt;
                                }
                                else if (sPayType == "CQ")
                                {
                                    sCashAmt = "0.00";
                                    sQuery = string.Format("select sum(chk_amt) as check_amt from cancelled_chk where or_no = '{0}'", sOrNo);
                                    pSet1.Query = sQuery;
                                    if (pSet1.Execute())
                                        if (pSet1.Read())
                                            sChkAmt = pSet1.GetDouble("check_amt").ToString("0.00");
                                    pSet1.Close();

                                    sTotal = sChkAmt;
                                }
                                else if (sPayType == "CC")
                                {
                                    sQuery = string.Format("select sum(cash_amt) as cash_amt, sum(chk_amt) as check_amt from cancelled_chk where or_no = '{0}'", sOrNo);
                                    pSet1.Query = sQuery;
                                    if (pSet1.Execute())
                                        if (pSet1.Read())
                                        {
                                            sCashAmt = pSet1.GetDouble("cash_amt").ToString("0.00");
                                            sChkAmt = pSet1.GetDouble("check_amt").ToString("0.00");
                                        }
                                    pSet1.Close();

                                    sTotal = Convert.ToDouble(Convert.ToDouble(sCashAmt) + Convert.ToDouble(sChkAmt)).ToString("0.00");
                                }
                                else if (sPayType == "CSTC")
                                {
                                    sQuery = string.Format("select sum(fees_amtdue) as total_amt from cancelled_or where or_no = '{0}'", sOrNo);
                                    pSet1.Query = sQuery;
                                    if (pSet1.Execute())
                                        if (pSet1.Read())
                                            sOrTotalAmt = pSet1.GetDouble("total_amt").ToString("0.00");
                                    pSet1.Close();

                                    //sQuery = string.Format("select sum(debit) as debit_amt from dbcr_memo where or_no = '{0}' and memo like 'REVERS%%'", sOrNo);
                                    sQuery = string.Format("select sum(debit) as debit_amt from dbcr_memo where or_no = '{0}' and memo like 'REVERS%%'", sOrNo); //JARS 20171010
                                    pSet1.Query = sQuery;
                                    if (pSet1.Execute())
                                        if (pSet1.Read())
                                            sDebitAmt = pSet1.GetDouble("debit_amt").ToString("0.00");
                                    pSet1.Close();

                                    sCashAmt = Convert.ToDouble(Convert.ToDouble(sOrTotalAmt) - Convert.ToDouble(sDebitAmt)).ToString("0.00");
                                    sTotal = sCashAmt;
                                }
                                else if (sPayType == "TC")
                                {
                                    sCashAmt = "0.00";
                                    sChkAmt = "0.00";
                                    sTotal = "0.00";
                                }

                                iTotalCount++;
                                dTotalCash = dTotalCash + Convert.ToDouble(sCashAmt);
                                dTotalCheck = dTotalCheck + Convert.ToDouble(sChkAmt);
                                dTotal = dTotal + Convert.ToDouble(sTotal);

                                iCountOR++;
                                dSubTotalCS = dSubTotalCS + Convert.ToDouble(sCashAmt);
                                dSubTotalCQ = dSubTotalCQ + Convert.ToDouble(sChkAmt);
                                dSubTotal = dSubTotal + Convert.ToDouble(sTotal);

                            }
                        pSet2.Close();

                        sContent = sDate + "|" + iCountOR.ToString() + "|" + dSubTotalCS.ToString("#,##0.00") + "|" + dSubTotalCQ.ToString("#,##0.00") + "|" + dSubTotal.ToString("#,##0.00") + "";
                        axVSPrinter1.Table = sContent;
                        axVSPrinter1.Paragraph = "";

                        axVSPrinter1.FontSize = 7;
                        axVSPrinter1.Table = sContentSize + sContent;
                        iCountOR = 0;
                    }
                }
                else
                {
                    MessageBox.Show("No Record(s) Found", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                pSet.Close();
                #endregion
            }

            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            this.axVSPrinter1.FontBold = true;
            if (m_iGroupFormat == 2)
                axVSPrinter1.Table = "^2000|>2000|>2200|>2200|>2200;TOTAL|" + iTotalCount.ToString("#,##0") + "|" + dTotalCash.ToString("#,##0.00") + "|" + dTotalCheck.ToString("#,##0.00") + "|" + dTotal.ToString("#,##0.00");
            else
                axVSPrinter1.Table = "^2500|>1500|>1500|>1500|>3600;TOTAL|" + dTotalCash.ToString("#,##0.00") + "|" + dTotalCheck.ToString("#,##0.00") + "|" + dTotal.ToString("#,##0.00") + "|";
            this.axVSPrinter1.FontBold = false;
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Table = ("<1200|<5000;Printed by :|" + AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode));
            axVSPrinter1.Table = ("<1200|<5000;Date printed :|" + AppSettingsManager.GetSystemDate());
        }

        private void AbstractofExcessofChecks()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();
            Boolean bReturn = false;

            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            axVSPrinter1.FontName = "Arial";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.MarginLeft = 720;
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;

            String sQuery, sOrNo, sBin, sMemo = "", sPayType, sCashAmt, sChkAmt, sOrTotalAmt, sDebitAmt, sTotal, sCredit;
            String sBlank, sTotalCash, sTotalChk;
            double dTotal = 0, dTotalCash = 0, dTotalCheck = 0;
            String sTotalCount, sCountOR = "", sOrDate = "", sOwner = "";

            int iTotalCount = 0;

            sContentSize = "^1000|^1500|<1500|>800|>1500|>1500|<2900;";

            if (m_iGroupFormat == 0)
                //sQuery = string.Format("select dbcr_memo.* from dbcr_memo where teller = '{0}' and or_date = to_date('{1}','MM/dd/yyyy') and memo like 'REMAIN%%' and multi_pay = 'N' and balance <> '0.00'", m_sUser, m_sFromDate);
                sQuery = string.Format("select dbcr_memo.* from dbcr_memo where teller = '{0}' and or_date = to_date('{1}','MM/dd/yyyy') and memo like 'REMAIN%%' and multi_pay = 'N' and balance <> '0.00'", m_sUser, m_sFromDate); //JARS 20171010
            else if (m_iGroupFormat == 1)
                //sQuery = string.Format("select dbcr_memo.* from dbcr_memo where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and memo like 'REMAIN%%' and multi_pay = 'N' and chk_no in (select chk_no from chk_tbl where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy')) order by or_no", m_sFromDate, m_sToDate);
                sQuery = string.Format("select dbcr_memo.* from dbcr_memo where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and memo like 'REMAIN%%' and multi_pay = 'N' and chk_no in (select chk_no from chk_tbl where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy')) order by or_no", m_sFromDate, m_sToDate); //JARS 20171010
            else // 2
            {
                //sQuery = string.Format("select distinct or_date from dbcr_memo where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and memo like 'REMAIN%%' and multi_pay = 'N' and chk_no in (select chk_no from chk_tbl where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy')) ", m_sFromDate, m_sToDate);
                sQuery = string.Format("select distinct or_date from dbcr_memo where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and memo like 'REMAIN%%' and multi_pay = 'N' and chk_no in (select chk_no from chk_tbl where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy')) ", m_sFromDate, m_sToDate); //JARS 20171010
                sContentSize = "^2000|^2000|>2200|>2200|>2200;";
            }

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sOrNo = "";
                    sCredit = "0";
                    String sChkNo = "";
                    if (m_iGroupFormat != 2)
                    {
                        sOwner = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(pSet.GetString("bin")));
                        sOrNo = pSet.GetString("or_no");
                        sChkNo = pSet.GetString("chk_no");
                        sCredit = pSet.GetDouble("credit").ToString("0.00");
                    }
                    sCashAmt = "0.00";

                    if (m_iGroupFormat == 0)
                    {
                        sQuery = string.Format("select chk_no from chk_tbl where CHK_NO = '{0}' and or_date = to_date('{1}','MM/dd/yyyy')", sChkNo, m_sFromDate);
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                            if (pSet1.Read())
                                sMemo = "EXCESS OF CHECKS";
                            else
                            {
                                sMemo = "EXCESS OF TAX CREDIT";
                                pSet1.Close();
                            }
                        pSet1.Close();
                    }
                    else if (m_iGroupFormat == 1)
                        sMemo = "EXCESS OF CHECKS";

                    if (m_iGroupFormat != 2)
                    {
                        sQuery = string.Format("select distinct bin from pay_hist where or_no = '{0}'", sOrNo);
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                            if (pSet1.Read())
                            {
                                sBin = pSet.GetString("bin");
                                sTotal = sCredit;
                                dTotalCash = dTotalCash + Convert.ToDouble(sCashAmt);
                                dTotalCheck = dTotalCheck + Convert.ToDouble(sCredit);
                                dTotal = dTotal + Convert.ToDouble(sTotal);

                                sContent = sOrNo + "|" + sBin + "|" + sOwner + "|" + Convert.ToDouble(sCashAmt).ToString("#,##0.00") + "|" + Convert.ToDouble(sCredit).ToString("#,##0.00") + "|" + dTotal.ToString("#,##0.00") + "|" + sMemo + "";

                                axVSPrinter1.FontSize = 7;
                                axVSPrinter1.Table = sContentSize + sContent;
                            }
                        pSet1.Close();
                    }
                    else
                    {
                        sOrDate = pSet.GetDateTime("or_date").ToShortDateString();

                        //sQuery = string.Format("select count(distinct or_no) as sTmpCount, sum(credit) as sTmpCredit from dbcr_memo where or_date = to_date('{0}','MM/dd/yyyy') and memo like 'REMAIN%%' and served = 'N'", sOrDate);
                        sQuery = string.Format("select count(distinct or_no) as sTmpCount, sum(credit) as sTmpCredit from dbcr_memo where or_date = to_date('{0}','MM/dd/yyyy') and memo like 'REMAIN%%' and served = 'N'", sOrDate); //JARS 20171010
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                            if (pSet1.Read())
                            {
                                sCountOR = pSet1.GetInt("sTmpCount").ToString();
                                sCredit = pSet1.GetDouble("sTmpCredit").ToString("0.00");
                            }
                        pSet1.Close();

                        iTotalCount = iTotalCount + Convert.ToInt32(sCountOR);
                        sTotal = sCredit;
                        dTotalCash = dTotalCash + Convert.ToDouble(sCashAmt);
                        dTotalCheck = dTotalCheck + Convert.ToDouble(sCredit);
                        dTotal = dTotal + Convert.ToDouble(sTotal);

                        sContent = sOrDate + "|" + sCountOR + "|" + Convert.ToDouble(sCashAmt).ToString("#,##0.00") + "|" + Convert.ToDouble(sCredit).ToString("#,##0.00") + "|" + Convert.ToDouble(sTotal).ToString("#,##0.00") + "";

                        axVSPrinter1.FontSize = 7;
                        axVSPrinter1.Table = sContentSize + sContent;
                    }

                }
            }
            else
            {
                MessageBox.Show("No Record Found.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                bReturn = true;
            }
            pSet.Close();

            if (bReturn == true)
            {
                return;
            }

            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            this.axVSPrinter1.FontBold = true;
            if (m_iGroupFormat != 2)
                axVSPrinter1.Table = "^2500|>1500|<800|>1500|>1500|>2900;TOTAL||" + dTotalCash.ToString("#,##0.00") + "|" + dTotalCheck.ToString("#,##0.00") + "|" + dTotal.ToString("#,##0.00") + "|";
            else
                axVSPrinter1.Table = "^2000|^2000|>2200|>2200|>2200;TOTAL|" + (iTotalCount).ToString("#,##0") + "|" + dTotalCash.ToString("#,##0.00") + "|" + dTotalCheck.ToString("#,##0.00") + "|" + dTotal.ToString("#,##0.00");
            this.axVSPrinter1.FontBold = false;
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Table = "<1200|<5000;Printed by :|" + AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);
            axVSPrinter1.Table = "<1200|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void AbstractofDebitedCreditMemos()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            axVSPrinter1.FontName = "Arial";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.MarginLeft = 720;
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;

            String sQuery, sOrNo, sBin = "", sMemo, sPayType, sCashAmt = "0", sChkAm = "0", sOrTotalAmt = "0", sDebitAmt = "0", sTotal = "0", sCredit = "0", sCMNo, sBalance = "0", sOwnCode = "";
            String sBlank, sTotalCash = "0", sTotalChk = "0", sDebit = "0", sOwnCodeTemp = "", sChkNo;
            String sOwner;
            double dTotalBalance = 0, dTotalCredit = 0, dTotalDebit = 0, dTotalCM = 0;
            String sOwnCredit = "0";
            String sTotalCount, sCountOR = "", sOrDate = "";

            int iTotalCount = 0;
            sContentSize = "^1000|^1500|<1500|>1200|>1300|>1300|<2900;";

            if (m_iGroupFormat == 0)
                //sQuery = string.Format("select distinct or_no,chk_no from dbcr_memo where teller = '{0}' and or_date = to_date('{1}','MM/dd/yyyy') and memo like 'DEBITED%%' and multi_pay <> 'Y'", m_sUser, m_sFromDate);
                sQuery = string.Format("select distinct or_no,chk_no from dbcr_memo where teller = '{0}' and or_date = to_date('{1}','MM/dd/yyyy') and memo like 'DEBITED%%' and multi_pay <> 'Y'", m_sUser, m_sFromDate); //JARS 20171010
            else if (m_iGroupFormat == 1)
                //sQuery = string.Format("select * from dbcr_memo where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and memo like 'DEBITED%%' and multi_pay = 'N'", m_sFromDate, m_sToDate);
                sQuery = string.Format("select * from dbcr_memo where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and memo like 'DEBITED%%' and multi_pay = 'N'", m_sFromDate, m_sToDate); //JARS 20171010
            else //2
            {
                //sQuery = string.Format("select distinct or_date,or_no,debit,cr_memo_no from dbcr_memo where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and memo like 'REMAIN%%' and multi_pay = 'N' and chk_no in (select chk_no from chk_tbl where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy')) ", m_sFromDate, m_sToDate);
                sQuery = string.Format("select distinct or_date,or_no,debit,cr_memo_no from dbcr_memo where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and memo like 'REMAIN%%' and multi_pay = 'N' and chk_no in (select chk_no from chk_tbl where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy')) ", m_sFromDate, m_sToDate); //JARS 20171010
                sContentSize = "^2000|^2000|>2200|>2200|>2200;";
            }

            pSet.Query = sQuery;
            if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        if (m_iGroupFormat == 0)
                        {
                            sOwnCredit = "";
                            dTotalDebit = 0;
                            dTotalCM = 0;
                            dTotalBalance = 0;
                        }
                        sOrNo = pSet.GetString("or_no");

                        sQuery = string.Format("select distinct bin from pay_hist where or_no = '{0}'", sOrNo);
                        if (m_iGroupFormat == 0)
                        {
                            sChkNo = pSet.GetString("chk_no");

                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                    sBin = pSet1.GetString("bin");
                                else
                                {
                                    pSet1.Close();
                                }
                            pSet1.Close();

                            double dCredit = 0, dDebit = 0, dBalance = 0;

                            //sQuery = "select bin,debit,or_date,time_created from dbcr_memo";
                            sQuery = "select bin,debit,or_date,time_created from dbcr_memo"; //JARS 20171010
                            sQuery += " where or_no = '" + sOrNo + "'";
                            sQuery += " and or_date = to_date('" + m_sFromDate + "','MM/dd/yyyy')";
                            sQuery += " and memo like 'DEBITED%'";
                            sQuery += " and multi_pay = 'N'";
                            sQuery += " order by or_date desc, time_created desc";
                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                {
                                    dDebit = pSet1.GetDouble("debit");
                                    sOwnCode = AppSettingsManager.GetBnsOwnCode(pSet1.GetString("bin"));
                                }
                            pSet1.Close();

                            if (sChkNo != null)
                            {
                                //sQuery = "select balance,or_date,time_created from dbcr_memo";
                                sQuery = "select balance,or_date,time_created from dbcr_memo"; //JARS 20171010
                                sQuery += " where or_no = '" + sOrNo + "'";
                                sQuery += " and or_date = to_date('" + m_sFromDate + "','MM/dd/yyyy')";
                                sQuery += " and chk_no = '" + sChkNo + "'";
                                sQuery += " and memo like 'REMAINING%'";
                                sQuery += " and multi_pay = 'N'";
                                sQuery += " order by or_date desc, time_created desc";
                                pSet1.Query = sQuery;
                                if (pSet1.Execute())
                                    if (pSet1.Read())
                                    {
                                        dBalance = pSet1.GetDouble("balance");
                                        pSet1.Close();
                                    }
                                    else
                                    {
                                        //sQuery = "select balance,or_date,time_created from dbcr_memo";
                                        sQuery = "select balance,or_date,time_created from dbcr_memo"; //JARS 20171010
                                        sQuery += " where or_no = '" + sOrNo + "'";
                                        sQuery += " and or_date = to_date('" + m_sFromDate + "','MM/dd/yyyy')";
                                        sQuery += " and memo like 'REMAINING%'";
                                        sQuery += " and multi_pay = 'N'";
                                        sQuery += " order by or_date desc, time_created desc";
                                        pSet1.Query = sQuery;
                                        if (pSet1.Execute())
                                            if (pSet1.Read())
                                                dBalance = pSet1.GetDouble("balance");
                                        pSet1.Close();
                                    }
                            }

                            sOwnCredit = Convert.ToDouble(dBalance + dDebit).ToString("0.00");

                            sOwner = AppSettingsManager.GetBnsOwner(sOwnCode);		// JGR 01162005
                            sMemo = "APPLIED TAX CREDIT";

                            dTotalDebit += dDebit;
                            dTotalCM += Convert.ToDouble(sOwnCredit);
                            dTotalBalance += dBalance;

                            sContent = sOrNo + "||" + sBin + "|" + Convert.ToDouble(sOwnCredit).ToString("#,##0.00") + "|" + dDebit.ToString("#,##0.00") + "|" + dBalance.ToString("#,##0.00") + "|" + sMemo + "";

                            axVSPrinter1.FontSize = 7;
                            axVSPrinter1.Table = sContentSize + sContent;
                        }
                        else// if (m_iGroupFormat == 1)
                        {
                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                    sBin = pSet1.GetString("bin");
                            pSet1.Close();

                            sOwnCode = AppSettingsManager.GetBnsOwnCode(sBin);

                            double dCredit = 0;
                            //sQuery = string.Format("select credit from dbcr_memo where bin = '{0}' and memo not like 'DEBIT%%' and memo not like 'REVERSE%%' and memo not like 'REMAINING%%' and multi_pay = 'N'", sBin);
                            sQuery = string.Format("select credit from dbcr_memo where bin = '{0}' and memo not like 'DEBIT%%' and memo not like 'REVERSE%%' and memo not like 'REMAINING%%' and multi_pay = 'N'", sBin); //JARS 20171010
                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                                while (pSet1.Read())
                                {
                                    dCredit += pSet1.GetDouble("credit");
                                }
                            pSet1.Close();

                            sOwner = AppSettingsManager.GetBnsOwner(sOwnCode);
                            sMemo = "APPLIED TAX CREDIT";
                            sDebit = pSet.GetDouble("debit").ToString("0.00");
                            sCMNo = pSet.GetString("cr_memo_no");

                            sBalance = Convert.ToDouble(Convert.ToDouble(sCredit) - Convert.ToDouble(sDebit)).ToString("#,##0.00");

                            sContent = sOrNo + "||" + sBin + "|" + dCredit.ToString("#,##0.00") + "|" + Convert.ToDouble(sDebit).ToString("#,##0.00") + "|" + sBalance + "|" + sMemo + ";";
                           
                            dTotalCredit += dCredit;
                            dTotalDebit += Convert.ToDouble(sDebit);
                            dTotalBalance += Convert.ToDouble(sBalance);

                            axVSPrinter1.FontSize = 7;
                            axVSPrinter1.Table = sContentSize + sContent;
                        }

                    }
                }
                else
                {
                    MessageBox.Show("No Record(s) Found.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            pSet.Close();

            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            this.axVSPrinter1.FontBold = true;
            if (m_iGroupFormat != 2)
                axVSPrinter1.Table = "^4000|>1200|>1300|>1300|>2900;TOTAL|" + dTotalCM.ToString("#,##0.00") + "|" + dTotalDebit.ToString("#,##0.00") + "|" + dTotalBalance.ToString("#,##0.00") + "|";
            this.axVSPrinter1.FontBold = false;
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Table = "<1200|<5000;Printed by :|" + AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);
            axVSPrinter1.Table = "<1200|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void AbstractofExcessofTaxCredit()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();
            Boolean bReturn = false;

            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            axVSPrinter1.FontName = "Arial";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.MarginLeft = 720;
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;

            String sQuery, sFrom, sTo, sOrNo="", sBin, sMemo = "", sPayType, sCashAmt = "0", sChkAmt, sOrTotalAmt, sDebitAmt, sTotal, sCredit = "0";
            String sBlank, sTotalCash, sTotalChk, sTotalCount, sCountOR = "0";
            double dTotal = 0, dTotalCash = 0, dTotalCheck = 0;
            String sOwner, sOrDate = "";
            int iTotalCount = 0;

            if (m_iGroupFormat != 2)
                sContentSize = "^1000|^1500|<1500|>800|>1500|>1500|<2900;";
            else
                sContentSize = "^2000|^2000|>2200|>2200|>2200;";

            //if (m_iGroupFormat == 0)
            //    sQuery = string.Format("select * from dbcr_memo where teller = '{0}' and or_date = to_date('{1}','MM/dd/yyyy') and memo like 'REMAIN%%'  and multi_pay = 'N'and balance <> '0.00'", m_sUser, m_sFromDate);
            //else if (m_iGroupFormat == 1)
            //    sQuery = string.Format("select * from dbcr_memo where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and memo like 'REMAIN%%' and multi_pay = 'N' and chk_no in (select chk_no from chk_tbl where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy')) order by or_no", m_sFromDate, m_sToDate);
            //else //2
            //    sQuery = string.Format("select distinct or_date from dbcr_memo where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and memo like 'REMAIN%%' and multi_pay = 'N' and chk_no in (select chk_no from chk_tbl where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy')) ", m_sFromDate, m_sToDate);

            //JARS 20171010
            if (m_iGroupFormat == 0)
                sQuery = string.Format("select * from dbcr_memo where teller = '{0}' and or_date = to_date('{1}','MM/dd/yyyy') and memo like 'REMAIN%%'  and multi_pay = 'N'and balance <> '0.00'", m_sUser, m_sFromDate);
            else if (m_iGroupFormat == 1)
                sQuery = string.Format("select * from dbcr_memo where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and memo like 'REMAIN%%' and multi_pay = 'N' and chk_no in (select chk_no from chk_tbl where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy')) order by or_no", m_sFromDate, m_sToDate);
            else //2
                sQuery = string.Format("select distinct or_date from dbcr_memo where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and memo like 'REMAIN%%' and multi_pay = 'N' and chk_no in (select chk_no from chk_tbl where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy')) ", m_sFromDate, m_sToDate);

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    if (m_iGroupFormat == 0)
                    {
                        sOrNo = pSet.GetString("or_no");
                        String sChkNo = pSet.GetString("chk_no");
                        sQuery = string.Format("select chk_no from chk_tbl where CHK_NO = '{0}' and or_date = to_date('{1}','MM/dd/yyyy')", sChkNo, m_sFromDate);
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                            if (pSet1.Read())
                            {
                                sMemo = "EXCESS OF CHECKS";
                                pSet1.Close();
                            }
                            else
                                sMemo = "EXCESS OF TAX CREDIT";
                        pSet1.Close();
                    }
                    else if (m_iGroupFormat == 1)
                    {
                        sOrNo = pSet.GetString("or_no");
                        sMemo = "EXCESS OF CHECKS";
                    }
                    else
                    {
                        sOrDate = pSet.GetDateTime("or_date").ToShortDateString();
                        sCashAmt = "0.00";

                        //sQuery = string.Format("select count(distinct or_no) as sTmpCount, sum(credit) as sTmpCredit from dbcr_memo where or_date = to_date('{0}','MM/dd/yyyy') and memo like 'REMAIN%%' and served = 'N'", sOrDate);
                        sQuery = string.Format("select count(distinct or_no) as sTmpCount, sum(credit) as sTmpCredit from dbcr_memo where or_date = to_date('{0}','MM/dd/yyyy') and memo like 'REMAIN%%' and served = 'N'", sOrDate); //JARS 20171010
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                            if (pSet1.Read())
                            {
                                sCountOR = pSet1.GetInt("sTmpCount").ToString();
                                sCredit = pSet1.GetDouble("sTmpCredit").ToString("0.00");
                            }
                        pSet1.Close();

                        iTotalCount += Convert.ToInt32(sCountOR);

                        sTotal = sCredit;
                        dTotalCash += Convert.ToDouble(sCashAmt);
                        dTotalCheck += Convert.ToDouble(sCredit);
                        dTotal += Convert.ToDouble(sTotal);

                        sContent = sOrDate + "|" + sCountOR + "|" + Convert.ToDouble(sCashAmt).ToString("#,##0.00") + "|" + Convert.ToDouble(sCredit).ToString("#,##0.00") + "|" + Convert.ToDouble(sTotal).ToString("#,##0.00") + "";

                        axVSPrinter1.FontSize = 7;
                        axVSPrinter1.Table = sContentSize + sContent;

                    }

                    if (m_iGroupFormat != 2)
                    {
                        sCredit = pSet.GetDouble("credit").ToString("0.00");
                        sOwner = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(pSet.GetString("bin")));

                        sCashAmt = "0.00";

                        sQuery = string.Format("select distinct bin from pay_hist where or_no = '{0}'", sOrNo);
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                            if (pSet1.Read())
                            {
                                sBin = pSet1.GetString("bin");
                                sTotal = sCredit;
                                dTotalCash += Convert.ToDouble(sCashAmt);
                                dTotalCheck += Convert.ToDouble(sCredit);
                                dTotal += Convert.ToDouble(sTotal);

                                sContent = sOrNo + "|" + sBin + "|" + sOwner + "|" + Convert.ToDouble(sCashAmt).ToString("#,##0.00") + "|" + Convert.ToDouble(sCredit).ToString("#,##0.00") + "|" + Convert.ToDouble(sTotal).ToString("#,##0.00") + "|" + sMemo + "";

                                axVSPrinter1.FontSize = 7;
                                axVSPrinter1.Table = sContentSize + sContent;
                            }
                        pSet1.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record(s) Found.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                bReturn = true; // CTS 01252005 use bReturn termination
            }
            pSet.Close();

            if (bReturn == true)
            {
                return;
            }

            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
            this.axVSPrinter1.FontBold = true;
            if (m_iGroupFormat != 2)
                axVSPrinter1.Table = "^2500|>1500|>800|>1500|>1500|>2900;TOTAL||" + dTotalCash.ToString("#,##0.00") + "|" + dTotalCheck.ToString("#,##0.00") + "|" + dTotal.ToString("#,##0.00") + "|";
            else
                axVSPrinter1.Table = "^2000|^2000|>2200|>2200|>2200;TOTAL|" + iTotalCount + "|" + dTotalCash.ToString("#,##0.00") + "|" + dTotalCheck.ToString("#,##0.00") + "|" + dTotal.ToString("#,##0.00");
            this.axVSPrinter1.FontBold = false;
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Table = "<1200|<5000;Printed by :|" + AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);
            axVSPrinter1.Table = "<1200|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void CompromistAgreement()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();

            String sQuery, sContent;
            String sBIN, sBnsName, sOwnName, sOwnAdd, sOwnCode, sTaxYr, sRefNo, sNoPay, sDtApprvd, sTermPay = "", sTotalDue = "";

            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape; // 0 portrait 1 landscape
            axVSPrinter1.MarginTop = 1000;
            axVSPrinter1.MarginLeft = 1000;
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            axVSPrinter1.FontName = "Times New Roman";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            sQuery = "select * from compromise_tbl a, businesses b, own_names c";
            sQuery += " where a.bin = b.bin and b.own_code = c.own_code order by c.own_ln";
            pSet.Query = sQuery;
            if (pSet.Execute())
                while (pSet.Read())
                {
                    sBIN = pSet.GetString("bin").Trim();
                    sBnsName = pSet.GetString("bns_nm").Trim();
                    sOwnCode = pSet.GetString("own_code").Trim();
                    sOwnName = AppSettingsManager.GetBnsOwner(sOwnCode).Trim();
                    sOwnAdd = AppSettingsManager.GetBnsOwnAdd(sOwnCode).Trim();
                    sTaxYr = pSet.GetString("tax_yearfrom").Trim() + " - ";
                    sTaxYr += pSet.GetString("tax_yearto").Trim();
                    sRefNo = pSet.GetString("ref_no").Trim();
                    sNoPay = pSet.GetInt("no_pay_seq").ToString().Trim();
                    sDtApprvd = pSet.GetDateTime("dt_approve").ToShortDateString().Trim();

                    sQuery = "select term_to_pay,sum(fees_total) as fees_total from compromise_due";
                    sQuery += " where bin = '" + sBIN + "' group by term_to_pay";
                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                        if (pSet1.Read())
                        {
                            sTermPay = pSet1.GetInt("term_to_pay").ToString();
                            sTotalDue = pSet1.GetDouble("fees_total").ToString("#,##0.00");
                        }
                    pSet1.Close();

                    sContent = "<1700|<2250|<2250|<2500|<1200|<1000|>800|^900|>950|>1000;";
                    sContent += sBIN + "|" + sBnsName + "|" + sOwnName + "|" + sOwnAdd + "|" + sRefNo + "|";
                    sContent += sTaxYr + "|" + sNoPay + "|" + sDtApprvd + "|" + sTermPay + "|" + (sTotalDue) + "";
                    axVSPrinter1.Table = sContent;
                }
            pSet.Close();

            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.FontSize = 8;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Table = ("<1200|<5000;Printed by :| " + StringUtilities.StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName));
            axVSPrinter1.Table = ("<1200|<5000;Date printed :| " + AppSettingsManager.GetSystemDate());

        }

        private void DebitCreditReport()
        {
            String sHeader, sContent, sQuery;
            String sUser, sOwnCode, sOwner, sChkNo, sOrDate, sOrNo, sDebit, sCredit, sBalance, sMemo, sTellerCode, sTellerName, sServed, sMultiPay, sStat;
            sContent = "<900|<2000|>1000|>900|>900|>1100|=2500|^1700|^900|>900|^900|^1000;";
            if (m_sUser.Trim() == "ALL")
                sUser = "%";
            else
                sUser = m_sUser;

            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprFanfoldUS;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape; //orPortrait
            axVSPrinter1.MarginTop = 700;
            axVSPrinter1.MarginLeft = 700;
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            axVSPrinter1.FontName = "Arial";
            //axVSPrinter1.SetOrientation(0); // 0 portrait 1 landscape
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            OracleResultSet pSet = new OracleResultSet();
            try
            {
                if (m_iDebitFormat == 1)
                    //sQuery = "select bin,or_no,or_date,debit,credit,balance,memo,cr_memo_no,teller,time_created,served,chk_no,multi_pay,stat from dbcr_memo where teller like '" + sUser + "' and (or_date) between to_date('" + m_sFromDate + "','MM/dd/yyyy') and to_date('" + m_sToDate + "','MM/dd/yyyy') order by or_no asc, memo asc";
                    //sQuery = "select own_code,or_no,or_date,debit,credit,balance,memo,cr_memo_no,teller,time_created,served,chk_no,multi_pay,stat from dbcr_memo where teller like '" + sUser + "' and (or_date) between to_date('" + m_sFromDate + "','MM/dd/yyyy') and to_date('" + m_sToDate + "','MM/dd/yyyy') order by or_no asc, memo asc"; // JARS 20170712 HAD TO ADJUST FOR DBCR_MEMO TABLE
                    sQuery = "select bin,or_no,or_date,debit,credit,balance,memo,cr_memo_no,teller,time_created,served,chk_no,multi_pay,stat from dbcr_memo where teller like '" + sUser + "' and (or_date) between to_date('" + m_sFromDate + "','MM/dd/yyyy') and to_date('" + m_sToDate + "','MM/dd/yyyy') order by or_no asc, memo asc"; //JARS 20171010
                else if (m_iDebitFormat == 2)
                    //sQuery = "select bin,or_no,or_date,debit,credit,balance,memo,cr_memo_no,teller,time_created,served,chk_no,multi_pay,stat from dbcr_memo where teller like '" + sUser + "' and (or_date) between to_date('" + m_sFromDate + "','MM/dd/yyyy') and to_date('" + m_sToDate + "','MM/dd/yyyy') and stat = '0' order by or_no asc, memo asc"; //MCR 20150320 is null to '0'
                    //sQuery = "select own_code,or_no,or_date,debit,credit,balance,memo,cr_memo_no,teller,time_created,served,chk_no,multi_pay,stat from dbcr_memo where teller like '" + sUser + "' and (or_date) between to_date('" + m_sFromDate + "','MM/dd/yyyy') and to_date('" + m_sToDate + "','MM/dd/yyyy') and stat = '0' order by or_no asc, memo asc"; //JARS 20170712 HAD TO ADJUST FOR DBCR_MEMO TABLE
                    sQuery = "select bin,or_no,or_date,debit,credit,balance,memo,cr_memo_no,teller,time_created,served,chk_no,multi_pay,stat from dbcr_memo where teller like '" + sUser + "' and (or_date) between to_date('" + m_sFromDate + "','MM/dd/yyyy') and to_date('" + m_sToDate + "','MM/dd/yyyy') and stat = '0' order by or_no asc, memo asc"; //MCR 20150320 is null to '0' //JARS 20171010
                else
                    //sQuery = "select bin,or_no,or_date,debit,credit,balance,memo,cr_memo_no,teller,time_created,served,chk_no,multi_pay,stat from dbcr_memo where teller like '" + sUser + "' and (or_date) between to_date('" + m_sFromDate + "','MM/dd/yyyy') and to_date('" + m_sToDate + "','MM/dd/yyyy') and stat = '1' order by or_no asc, memo asc";
                    //sQuery = "select own_code,or_no,or_date,debit,credit,balance,memo,cr_memo_no,teller,time_created,served,chk_no,multi_pay,stat from dbcr_memo where teller like '" + sUser + "' and (or_date) between to_date('" + m_sFromDate + "','MM/dd/yyyy') and to_date('" + m_sToDate + "','MM/dd/yyyy') and stat = '0' order by or_no asc, memo asc"; //JARS 20170712 HAD TO ADJUST FOR DBCR_MEMO TABLE
                    sQuery = "select bin,or_no,or_date,debit,credit,balance,memo,cr_memo_no,teller,time_created,served,chk_no,multi_pay,stat from dbcr_memo where teller like '" + sUser + "' and (or_date) between to_date('" + m_sFromDate + "','MM/dd/yyyy') and to_date('" + m_sToDate + "','MM/dd/yyyy') and stat = '1' order by or_no asc, memo asc"; //JARS 20171010
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        sOwnCode = AppSettingsManager.GetOwnCode(pSet.GetString("bin").Trim()); //AFM 20200507 uncommented due to error
                        //sOwnCode = pSet.GetString("own_code"); //JARS 20170712
                        sOwner = AppSettingsManager.GetBnsOwner(sOwnCode).Trim();
                        sOrNo = pSet.GetString("or_no").Trim();
                        sOrDate = pSet.GetDateTime("or_date").ToShortDateString().Trim();
                        sDebit = pSet.GetDouble("debit").ToString("#,##0.00");
                        sCredit = pSet.GetDouble("credit").ToString("#,##0.00");
                        sBalance = pSet.GetDouble("balance").ToString("#,##0.00");
                        sMemo = pSet.GetString("memo").Trim();
                        sTellerCode = pSet.GetString("teller");
                        sTellerName = AppSettingsManager.GetTeller(sTellerCode, 0);
                        sChkNo = pSet.GetString("chk_no").Trim();
                        if (sChkNo.Length <= 3)
                            sChkNo = "";
                        sServed = pSet.GetString("served").Trim();
                        if (sServed == "Y")
                            sServed = "YES";
                        else
                            sServed = "NO";
                        sMultiPay = pSet.GetString("multi_pay").Trim();
                        if (sMultiPay == "Y")
                            sMultiPay = "YES";
                        else
                            sMultiPay = "NO";
                        sStat = pSet.GetString("stat").Trim();
                        if (sStat == "1")
                            sStat = "MANUAL";
                        else
                            sStat = "AUTO-GEN";

                        sContent += sOrNo + "|" + sOwner + "|" + sOrDate + "|" + sDebit + "|" + sCredit + "|" + sBalance + "|" + sMemo + "|" + sTellerName + "|" + sServed + "|" + sChkNo + "|" + sMultiPay + "|" + sStat + ";";
                    }
                }
                else
                {
                    sContent = "";
                }
                pSet.Close();
            }
            catch
            {

            }
            //axVSPrinter1.FontSize = 6;
            axVSPrinter1.Table = sContent;
        }
        private void ListDeclaredOR() //JARS 20160621
        {
            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            axVSPrinter1.MarginTop = 1000;
            axVSPrinter1.MarginLeft = 1000;
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            axVSPrinter1.FontName = "Arial";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();
            string sTeller, sContent, sTellerName;
            string sTrnDate, sOrFrom, sOrTo, sAssigned, sCurrentUser;

            pSet1.Query = "SELECT * FROM (select distinct(teller) from or_assigned where teller like '%" + m_sUser + "%') UNION SELECT * FROM (select distinct(teller) from or_assigned_hist where teller like '%" + m_sUser + "%')";
            if(pSet1.Execute())
            {
                while(pSet1.Read())
                {
                    axVSPrinter1.FontBold = true;
                    sTeller = pSet1.GetString("Teller");
                    sTellerName = AppSettingsManager.GetTeller(sTeller, 1);
                    sContent = "<10000;" + sTellerName;
                    axVSPrinter1.Table = sContent;
                    axVSPrinter1.FontBold = false;

                    //pSet2.Query = "SELECT * FROM (select * from or_assigned where trn_date between '" + m_sFromDate + "' and '" + m_sToDate + "' and teller = '" + sTeller + "') UNION ALL SELECT * FROM (select * from or_assigned_hist where trn_date between '" + m_sFromDate + "' and '" + m_sToDate + "' and teller = '" + sTeller + "')";
                    pSet2.Query = "SELECT * FROM (select TELLER,FROM_OR_NO,TO_OR_NO,TRN_DATE,ASSIGNED_BY,FORM_TYPE,DATE_ASSIGNED from or_assigned where trn_date between '" + m_sFromDate + "' and '" + m_sToDate + "' and teller = '" + sTeller + "') UNION ALL SELECT teller, from_or_no,to_or_no,trn_date,ASSIGNED_BY,FORM_TYPE,DATE_ASSIGNED FROM (select * from or_assigned_hist where trn_date between '" + m_sFromDate + "' and '" + m_sToDate + "' and teller = '" + sTeller + "')"; // JAA 20190305 modified

                    if(pSet2.Execute())
                    {
                        while(pSet2.Read())
                        {
                            sTrnDate = pSet2.GetDateTime("trn_date").ToString("dd-MMM-yy");
                            sOrFrom = pSet2.GetString("from_or_no");
                            sOrTo = pSet2.GetString("to_or_no");
                            sAssigned = pSet2.GetString("assigned_by");
                            sContent = "^2500|^2500|^2500|^2500;" + sTrnDate + "|" + sOrFrom + "|" + sOrTo + "|" + sAssigned;
                            axVSPrinter1.Table = sContent;
                        }
                    }
                }
            }
            pSet2.Close();
            pSet1.Close();
        }
        private void WaiveSurchPen() //MCR 20170602
        {
            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            axVSPrinter1.MarginTop = 1000;
            axVSPrinter1.MarginLeft = 1000;
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            axVSPrinter1.FontName = "Arial";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            OracleResultSet pSet1 = new OracleResultSet();
            string sContent, sValue;

            pSet1.Query = "select WP.*,B.bns_nm from waive_penalty WP inner join businesses B on B.bin = WP.bin";
            if (pSet1.Execute())
            {
                while (pSet1.Read())
                {
                    sValue = pSet1.GetInt(3).ToString();
                    if (sValue == "1")
                        sValue = "APPROVED";
                    else
                        sValue = "PENDING";

                    sContent = "<2000|<3000|<3000|^900|^1100;" + pSet1.GetString(0) + "|" + StringUtilities.StringUtilities.RemoveApostrophe(pSet1.GetString(4)) + "|" + StringUtilities.StringUtilities.RemoveApostrophe(pSet1.GetString(2)) + "|" + pSet1.GetString(1) + "|" + sValue;
                    axVSPrinter1.Table = sContent;
                }
            }
            pSet1.Close(); 
            
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Table = "<1200|<5000;Printed by :|" + AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);
            axVSPrinter1.Table = "<1200|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CreateHeader()
        {
            string sHeader = "", sHeader1 = "", sHeader2 = "";

            sHeader = AppSettingsManager.GetConfigValue("09");
            axVSPrinter1.FontName = "Arial Narrow";
            if (m_sReportTitle.Contains("COLLECTIBLES") || m_sReportTitle == "LIST OF DECLARED OR BY TELLER" || m_sReportTitle.Contains("LIST OF BUSINESSES WITH WAIVED SURCHARGE AND PENALTY"))    // RMC 20150524 corrections in reports
                axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            else
                axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape; // 0 portrait 1 landscape
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            axVSPrinter1.FontSize = 10;
            sHeader1 = "^14500;Republic of the Philippines";
            axVSPrinter1.Table = sHeader1;

            // RMC 20150501 QA BTAS (s)
            string sProv = AppSettingsManager.GetConfigValue("08");
            if (sProv.Trim() != "")
                axVSPrinter1.Table = "^14500;PROVINCE OF " + sProv;
            // RMC 20150501 QA BTAS (e)

            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = "^14500;" + sHeader;
            axVSPrinter1.FontBold = false;
            if (AppSettingsManager.GetConfigObject("01") == "CITY")
                sHeader2 = "^14500;Office of the City Treasurer";
            else if (AppSettingsManager.GetConfigObject("01") == "MUNICIPALITY")
                sHeader2 = "^14500;Office of the Municipal Treasurer";
            axVSPrinter1.Table = sHeader2;


            // RMC 20150524 corrections in reports (s)
            if (m_sReportTitle.Contains("COLLECTIBLES"))
            {
                axVSPrinter1.FontName = "Arial";
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.Table = "^14500;" + m_sReportTitle.ToUpper().Trim();
                axVSPrinter1.FontSize = 8;
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.FontBold = true;
                axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                if (m_sReportTitle.Contains("TOP"))  // RMC 20161209 correction in top collectibles (s)
                {
                    axVSPrinter1.Table = string.Format("^2400|^2000|^2000|^2000|^2000;BIN/Business Name|Tax|Fees|Charges|Total");
                    axVSPrinter1.Table = "^2400|^2000|^2000|^2000|^2000; | | | | | ";
                }
                else// RMC 20161209 correction in top collectibles (e)
                {
                    axVSPrinter1.Table = string.Format("^1700|^700|^2000|^2000|^2000|^2000;Main Business|Count|Tax|Fees|Charges|Total");
                    axVSPrinter1.Table = "^1700|^700|^2000|^2000|^2000|^2000; | | | | | ";
                }
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                axVSPrinter1.FontBold = false;
            }
            // RMC 20150524 corrections in reports (e)
        }

        private void axVSPrinter1_NewPageEvent(object sender, EventArgs e)
        {
            if (m_sReportTitle == "LIST OF COMPROMISE AGREEMENT")
            {
                OracleResultSet pSet = new OracleResultSet();
                CreateHeader();
                string sHeader3 = "";

                axVSPrinter1.Paragraph = "";
                sHeader3 = "^14500;" + m_sReportTitle.ToUpper().Trim();
                axVSPrinter1.FontBold = true;
                axVSPrinter1.FontSize = 8;
                axVSPrinter1.Table = sHeader3;
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.FontName = "Arial";
                axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                axVSPrinter1.Table = ("^1700|^2250|^2250|^2500|^1150|^1000|^800|^900|^950|^1000;Bin|Business Name|Owner's Name|Owner's Add|Ref. No.|Tax Year|No. of Pay't|Date App'd|Term To Pay|Total Due");

                axVSPrinter1.Paragraph = "";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                axVSPrinter1.FontBold = false;
            }
            else if (m_sReportTitle == "DEBIT CREDIT REPORTS")
            {
                CreateHeader();

                axVSPrinter1.FontBold = true;
                axVSPrinter1.Table = "^14500;DEBIT / CREDIT REPORTS";
                axVSPrinter1.Paragraph = "";

                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
                axVSPrinter1.FontName = "Arial";
                axVSPrinter1.FontSize = 8;
                axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                axVSPrinter1.Table = "^900|^2000|^1000|^900|^900|^1100|^2500|^1700|^900|^900|^900|^1000;O.R. No|Owner|O.R. Date|Debit|Credit|Balance|Memo|Teller|Served|Chk. No|Multi Pay|Status";
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.FontBold = false;
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            }
            else if (m_sReportTitle.Contains("ABSTRACT OF"))
            {
                String sHeader3;

                CreateHeader();

                axVSPrinter1.Paragraph = "";
                sHeader3 = "^10600;" + m_sReportTitle.ToUpper();
                axVSPrinter1.FontBold = true;
                axVSPrinter1.FontSize = 8;
                axVSPrinter1.Table = sHeader3;
                axVSPrinter1.FontBold = false;
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;

                if (m_sReportTitle.Contains("ABSTRACT OF CANCELLED RECEIPTS"))
                {
                    if (m_iGroupFormat == 0)
                    {
                        axVSPrinter1.Table = "<5300|>5300;Teller :  " + AppSettingsManager.GetTeller(m_sUser, 0) + " |Date:  " + m_sFromDate;
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                        axVSPrinter1.Table = "^2500|^4500|^3600;|Amount Cancelled|";
                        axVSPrinter1.Table = "^1000|^1500|^1500|^1500|^1500|^3600;O.R. No.|Bin|Cash|Check|Total|Reason For Cancellation";
                    }
                    else if (m_iGroupFormat == 1)
                    {
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.Table = "^10600;Date :  " + m_sFromDate + "  to " + m_sToDate;
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                        axVSPrinter1.Table = "^2500|^4500|^3600;|AMOUNT CANCELLED|";
                        axVSPrinter1.Table = "^1000|^1500|^1500|^1500|^1500|^3600;O.R. No.|Bin|Cash|Check|Total|Reason For Cancellation";
                    }
                    else// if(m_iGroupFormat == 2)
                    {
                        axVSPrinter1.Table = "^10600;Date :  " + m_sFromDate + "  to " + m_sToDate;
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                        axVSPrinter1.Table = "^4000|^6600;|AMOUNT CANCELLED";
                        axVSPrinter1.Table = "^2000|^2000|^2200|^2200|^2200;Date|No. of Cancelled O.Rs|Cash|Check|Total";
                    }
                }
                else if (m_sReportTitle.Contains("ABSTRACT OF EXCESS OF CHECKS"))
                {
                    if (m_iGroupFormat == 0)
                    {
                        axVSPrinter1.Table = "<5300|>5300;Teller :  " + AppSettingsManager.GetTeller(m_sUser, 0) + " |Date:  " + m_sFromDate;
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                        axVSPrinter1.Table = "^4000|^3800|^2900;|AMOUNT CREDITED|";
                        axVSPrinter1.Table = "^1000|^1500|^1500|^800|^1500|^1500|^2900;O.R. No.|BIN|Owner|Cash|Check|Total|Remarks";
                    }
                    else if (m_iGroupFormat == 1)
                    {
                        axVSPrinter1.Table = "^10600;Date :  " + m_sFromDate + "  to " + m_sToDate;
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                        axVSPrinter1.Table = "^4000|^3800|^2900;|AMOUNT CREDITED|";
                        axVSPrinter1.Table = "^1000|^1500|^1500|^800|^1500|^1500|^2900;O.R. No.|Bin|Owner|Cash|Check|Total|Remarks";
                    }
                    else //if(m_iGroupFormat == 2)
                    {
                        axVSPrinter1.Table = "^10600;Date :  " + m_sFromDate + "  to " + m_sToDate;
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                        axVSPrinter1.Table = "^4000|^6600;|AMOUNT CREDITED";
                        axVSPrinter1.Table = "^2000|^2000|^2200|^2200|^2200;Date|No. OF CMs Issued|Cash|Check|Total";
                    }
                }
                else if (m_sReportTitle.Contains("ABSTRACT OF DEBITED CREDIT MEMOS"))
                {
                    axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                    if (m_iGroupFormat == 0)
                    {
                        axVSPrinter1.Table = "<5300|>5300;Teller :  " + AppSettingsManager.GetTeller(m_sUser, 0) + " |Date:  " + m_sFromDate;
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                        axVSPrinter1.Table = "^1000|^1500|^1500|^1300|^1300|^1300|^2900;O.R. No.|C.M. No.|Bin|C.M. Amount|Amount Debited|Balance|Remarks";
                    }
                    else if (m_iGroupFormat == 1)
                    {
                        axVSPrinter1.Table = "^10600;Date :  " + m_sFromDate + "  to " + m_sToDate;
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                        axVSPrinter1.Table = "^1000|^1500|^1500|^1300|^1300|^1300|^2900;O.R. No.|C.M. No.|Bin|C.M. Amount|Amount Debited|Balance|Remarks";
                    }
                    else //if(m_iGroupFormat == 2)
                    {
                        axVSPrinter1.Table = "^10600;Date :  " + m_sFromDate + "  to " + m_sToDate;
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                        axVSPrinter1.Table = "^2000|^2000|^2200|^2200|^2200;Date|No. of Debited CMs|Total C.M. Amount|Total Amount Debited|Balance";
                    }
                }
                else if (m_sReportTitle.Contains("ABSTRACT OF EXCESS OF TAX CREDIT"))
                {
                    axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                    if (m_iGroupFormat == 0)
                    {
                        axVSPrinter1.Table = "<5300|>5300;Teller :  " + AppSettingsManager.GetTeller(m_sUser, 0) + " |Date:  " + m_sFromDate;
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                        axVSPrinter1.Table = "^4000|^3800|^2900;|AMOUNT CREDITED|";
                        axVSPrinter1.Table = "^1000|^1500|^1500|^800|^1500|^1500|^2900;O.R. No.|Bin|Owner|Cash|Tax Credit|Total|Remarks";
                    }
                    else if (m_iGroupFormat == 1)
                    {
                        axVSPrinter1.Table = "^10600;Date :  " + m_sFromDate + "  to " + m_sToDate;
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                        axVSPrinter1.Table = "^4000|^3800|^2900;|AMOUNT CREDITED|";
                        axVSPrinter1.Table = "^1000|^1500|^1500|^800|^1500|^1500|^2900;O.R. No.|Bin|Owner|Cash|Tax Credit|Total|Remarks";
                    }
                    else //if(m_iGroup == 2)
                    {
                        axVSPrinter1.Table = "^10600;Date :  " + m_sFromDate + "  to " + m_sToDate;
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;

                        axVSPrinter1.Table = "^4000|^6600;|AMOUNT CREDITED";
                        axVSPrinter1.Table = "^2000|^2000|^2200|^2200|^2200;Date|No. of CMs Issued|Cash|Tax Credit|Total";
                    }
                }
            }
            else if (m_sReportTitle.Contains("RECEIVABLES"))
            {

                CreateHeader();

                axVSPrinter1.FontName = "Arial";
                axVSPrinter1.Paragraph = "";

                axVSPrinter1.Table = ("^14500;" + m_sReportTitle.ToUpper().Trim() + ";");
                axVSPrinter1.FontBold = false;
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;

                axVSPrinter1.Table = (">11900|>6000;Business Tax Current Year|Business Tax Past Due;");
                axVSPrinter1.FontSize = 8;
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbAll;
                axVSPrinter1.FontBold = true;
                axVSPrinter1.Table = ("^1200|^2400|^1500|^1800|^600|^1100|^600|^1200|^1200|^1200|^1200|^1200|^1200|^1200|^700|^700|^700|^700|^700|^700|^700;Bin|Bus. Name / Address|Tel No. / No. Emp|Owner's Name / Main Business|Stat|Date|Tax Year|1st Qtr|2nd Qtr|3rd Qtr|4th Qtr|Reg. Fees|Sur & Int|Total|1yr|2yr|3yr|>=4yr|Reg. Fees|Sur & Int|Total");
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                axVSPrinter1.FontBold = false;
            }
            else if (m_sReportTitle.Contains("COLLECTIBLES"))
            {
                if (!m_bInit)
                    CreateHeader();

                m_bInit = false;

                /*axVSPrinter1.FontName = "Arial";
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.Table = "^14500;" + m_sReportTitle.ToUpper().Trim();
                axVSPrinter1.FontSize = 8;
                //sHeader3 = "^14500; From Tax Year" + "2011" + " to " + "2014";
                //sHeader3 = "^14500; As of " + AppSettingsManager.GetSystemDate();
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.FontBold = true;
                axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftMiddle;
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                axVSPrinter1.Table = ("^1700|^700|^2000|^2000|^2000|^2000;Main Business|Count|Tax|Fees|Charges|Total");
                axVSPrinter1.Table = "^1700|^700|^2000|^2000|^2000|^2000; | | | | | ";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                axVSPrinter1.FontBold = false;*/
            }
            else if (m_sReportTitle.Contains("LIST OF DECLARED OR")) //JARS 20160621
            {
                CreateHeader();
                string sHeader2;
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                axVSPrinter1.FontBold = true;
                axVSPrinter1.FontSize = 10;
                sHeader2 = "^14500;LIST OF DECLARED ORs";
                axVSPrinter1.Table = sHeader2;
                axVSPrinter1.FontSize = 8;
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                axVSPrinter1.Table = "^2500|^2500|^2500|^2500;Transaction Date|From OR NO|TO OR NO|ASSIGNED BY";
                axVSPrinter1.FontBold = false;
            }
            else if (m_sReportTitle.Contains("LIST OF BUSINESSES WITH WAIVED SURCHARGE AND PENALTY")) //MCR 20170602
            {
                CreateHeader();
                string sHeader2;
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                axVSPrinter1.FontBold = true;
                axVSPrinter1.FontSize = 10;
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.Paragraph = "";
                sHeader2 = "^10000;LIST OF BUSINESSES WITH WAIVED SURCHARGE AND PENALTY";
                axVSPrinter1.Table = sHeader2;
                axVSPrinter1.FontSize = 8;
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbBoxColumns;
                axVSPrinter1.Table = "^2000|^3000|^3000|^900|^1100;BIN|Busineses Name|Memoranda|Tax Year|Status";
                axVSPrinter1.FontBold = false;
            }

            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.FontBold = false;
        }

        private void ToolPrint_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintQuality = VSPrinter7Lib.PrintQualitySettings.pqHigh;

            if (MessageBox.Show("Are you sure you want to print?", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            axVSPrinter1.PrintDoc(1,1,axVSPrinter1.PageCount);
        }

        private void toolSettingPageSetup_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPageSetup);
        }

        private void toolSettingPrintPage_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPrinterSetup);
        }

        private void ExportFile() //MCR 20140828
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.DefaultExt = "html";
                dlg.Title = "Reports";
                dlg.Filter = "HTML Files (*.html;*.htm)|*.html|Excel Files (*.xls)|*.xls";
                dlg.FilterIndex = 3;
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    axVSPrinter1.ExportFormat = VSPrinter7Lib.ExportFormatSettings.vpxPlainHTML;
                    axVSPrinter1.ExportFile = dlg.FileName;
                }
            }
        }

    }
}