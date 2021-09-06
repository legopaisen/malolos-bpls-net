using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.SearchBusiness;
using Amellar.Common.StringUtilities;
using System.Windows.Forms;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.PaymentHistory
{
    class ClassCode
    {
        frmPaymentHistory fPayHist = null;
        public ClassCode (frmPaymentHistory fPayHist)
        {
            this.fPayHist = fPayHist; 
        }
        OracleResultSet result;

        public void LoadValues(string sBin)
        {
            result = new OracleResultSet();
            result.Query = "select distinct * from pay_hist where bin ='" + sBin.Trim() + "'";
            if(result.Execute())
            {
                if (result.Read())
                {

                    fPayHist.lblBnsName.Text = AppSettingsManager.GetBnsName(sBin.Trim());
                    fPayHist.lblBnsAdd.Text = AppSettingsManager.GetBnsAdd(sBin.Trim(), "");
                    fPayHist.lblOwnName.Text = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(sBin.Trim()));
                    fPayHist.lblOwnAdd.Text = AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetOwnCode(sBin.Trim()));
                    fPayHist.lblBnsPlate.Text = AppSettingsManager.GetBnsPlate(sBin.Trim()); //MCR 20150129
                    fPayHist.lblAppNo.Text = AppSettingsManager.GetAppNo(sBin.Trim()); //MCR 20150204
                    // RMC 20170130 enabled and modified tagging of payment under protest (s)
                    fPayHist.btnEditProtest.Enabled = true;
                    // RMC 20170130 enabled and modified tagging of payment under protest (e)
                }
                else
                {
                    // RMC 20170130 enabled and modified tagging of payment under protest (s)
                    fPayHist.btnEditProtest.Enabled = false;
                    // RMC 20170130 enabled and modified tagging of payment under protest (e)
                }
            }
            result.Close();
        }

        public void LoadData(string sBin, string sStartYear)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result1 = new OracleResultSet();

            result.Query = "delete from pay_hist_temp where bin = '" + sBin.Trim() + "'";
            if(result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();
            string sFeesCode = string.Empty;
            string sDue = string.Empty;
            string sSurch = string.Empty;
            string sPen = string.Empty;
            string sTotal = string.Empty;
            double dTotalTax = 0;
            double dTotalFees = 0;
            double dTotalSurchPen = 0;
            double dTotalTotal = 0;
            double dDue = 0;
            double dSurch = 0;
            double dPen = 0;
            double dTotal = 0;
            result.Query = "select distinct(or_no), tax_year, or_date, payment_term, qtr_paid, bill_no from pay_hist where bin = '" + sBin.Trim() + "' and tax_year >= '" + sStartYear.Trim() + "' and data_mode <> 'UNP' order by tax_year asc, or_no asc, or_date asc";
            if (result.Execute())
            {
                while (result.Read())
                {
                    string sBillNo = result.GetString("bill_no");
                    if (sBillNo == "")
                        result1.Query = "select * from or_table where or_no = '" + result.GetString("or_no").Trim() + "' and qtr_paid = '" + result.GetString("qtr_paid").Trim() + "' and tax_year = '" + result.GetString("tax_year").Trim() + "'";
                    else
                        result1.Query = "select * from or_table where bill_no = '" + result.GetString("bill_no").Trim() + "' and qtr_paid = '" + result.GetString("qtr_paid").Trim() + "' and tax_year = '" + result.GetString("tax_year").Trim() + "'";
                    if (result1.Execute())
                    {
                        while (result1.Read())
                        {
                            sFeesCode = result1.GetString("fees_code").Trim();
                            /*
                            sDue = result1.GetString("fees_due").Trim();
                            sSurch = result1.GetString("fees_surch").Trim();
                            sPen = result1.GetString("fees_pen").Trim();
                            sTotal = result1.GetString("fees_amtdue").Trim();
                            double.TryParse(sDue, out dDue);
                            double.TryParse(sSurch, out dSurch);
                            double.TryParse(sPen, out dPen);
                            double.TryParse(sTotal, out dTotal);
                             */
                            dDue = result1.GetDouble("fees_due");
                            try
                            {
                                dSurch = result1.GetDouble("fees_surch");
                            }
                            catch (System.Exception ex)
                            {
                                dSurch = 0;
                            }

                            try
                            {
                                dPen = result1.GetDouble("fees_pen");
                            }
                            catch (System.Exception ex)
                            {
                                dPen = 0;
                            }

                            try
                            {
                                dTotal = result1.GetDouble("fees_amtdue");
                            }
                            catch (System.Exception ex)
                            {
                                dTotal = 0;
                            }


                            if (sFeesCode.Substring(0, 1) == "B")
                                dTotalTax = dTotalTax + dDue;
                            else
                                dTotalFees = dTotalFees + dDue;

                            dTotalSurchPen = dTotalSurchPen + dSurch + dPen;
                            dTotalTotal = dTotalTotal + dTotal;
                        }
                    }
                    result1.Close();


                    //sTax = string.Format("{0:#,##0}", dTotalTax);
                    //sFees = string.Format("{0:#,##0}", dTotalFees);
                    //sSurchPen = string.Format("{0:#,##0}", dTotalSurchPen);
                    //sTotal = string.Format("{0:#,##0}", dTotalTotal);

                    // insert into pay_temp_hist
                    try
                    {
                        result1.Query = "insert into pay_hist_temp values(:1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11)";
                        result1.AddParameter(":1", sBin.Trim());
                        result1.AddParameter(":2", result.GetString("tax_year").Trim());
                        result1.AddParameter(":3", result.GetString("or_no").Trim());
                        result1.AddParameter(":4", result.GetDateTime("or_date"));
                        result1.AddParameter(":5", result.GetString("payment_term").Trim());
                        result1.AddParameter(":6", result.GetString("qtr_paid").Trim());
                        result1.AddParameter(":7", dTotalTax);
                        result1.AddParameter(":8", dTotalFees);
                        result1.AddParameter(":9", dTotalSurchPen);
                        result1.AddParameter(":10", dTotalTotal);
                        result1.AddParameter(":11", result.GetString("bill_no").Trim()); //MCR 20210610
                        if (result1.ExecuteNonQuery() != 0)
                        {

                        }
                        result1.Close();
                    }
                    catch (Exception e)
                    {
                        
                    }
                    dTotalTax = 0;
                    dTotalFees = 0;
                    dTotalSurchPen = 0;
                    dTotalTotal = 0;

                }

            }
            result.Close();

            UpdateList(sBin.Trim());
        }

        public void SearchBinLoad()
        {
            
            if (fPayHist.btnSearch.Text.Trim() == "&Search")
            {
                fPayHist.btnSearch.Text = "Cl&ear";
                if (fPayHist.bin1.txtTaxYear.Text.Trim() == string.Empty && fPayHist.bin1.txtBINSeries.Text.Trim() == string.Empty)
                {
                    using (frmSearchBusiness fSearchBns = new frmSearchBusiness())
                    {
                        fSearchBns.ShowDialog();
                        if (fSearchBns.sBIN.Length > 1)
                        {
                            fPayHist.bin1.txtTaxYear.Text = fSearchBns.sBIN.Substring(7, 4).ToString();
                            fPayHist.bin1.txtBINSeries.Text = fSearchBns.sBIN.Substring(12, 7).ToString();
                            LoadValues(StringUtilities.HandleApostrophe(fPayHist.bin1.GetBin()));
                            LoadData(StringUtilities.HandleApostrophe(fPayHist.bin1.GetBin()), StringUtilities.HandleApostrophe(fPayHist.txtStartYear.Text.Trim()));

                        }
                    }
                }
                else
                {
                    LoadValues(StringUtilities.HandleApostrophe(fPayHist.bin1.GetBin()));
                    LoadData(StringUtilities.HandleApostrophe(fPayHist.bin1.GetBin()), StringUtilities.HandleApostrophe(fPayHist.txtStartYear.Text.Trim()));
                }
            }
            else
            {
                fPayHist.txtStartYear.Text = ClassCode.StartingYear();
                fPayHist.btnSearch.Text = "&Search";
                CleanMe();
            }

        }

        private void UpdateList(string sBin)
        {
            OracleResultSet result2 = new OracleResultSet();
            fPayHist.dgvPaymentInfo.Rows.Clear();
            string sTax = string.Empty;
            string sFees = string.Empty;
            string sSurchPen = string.Empty;
            string sTotal = string.Empty;
            
            result2.Query = "select distinct * from pay_hist_temp where bin = '" + sBin.Trim() + "' order by tax_year,or_no, or_date,term,qtr";
            if(result2.Execute())
            {
                while(result2.Read())
                {
                    sTax = string.Format("{0:#,##0.00}", result2.GetDouble("tax"));
                    sFees = string.Format("{0:#,##0.00}", result2.GetDouble("fees"));
                    sSurchPen = string.Format("{0:#,##0.00}", result2.GetDouble("surch_pen"));
                    sTotal = string.Format("{0:#,##0.00}", result2.GetDouble("total"));
                    //fPayHist.dgvPaymentInfo.Rows.Add(result2.GetString("tax_year").Trim(), result2.GetString("or_no").Trim(), result2.GetDateTime("or_date").ToShortDateString(), result2.GetString("term").Trim(), result2.GetString("qtr").Trim(), sTax, sFees, sSurchPen, sTotal);
                    fPayHist.dgvPaymentInfo.Rows.Add(result2.GetString("tax_year").Trim(), result2.GetString("or_no").Trim(), result2.GetDateTime("or_date").ToShortDateString(), result2.GetString("term").Trim(), result2.GetString("qtr").Trim(), GetNoOfQtr(result2.GetString("or_no").Trim()), sTax, sFees, sSurchPen, sTotal, result2.GetString("bill_no").Trim());    // RMC 20130109 display no. of qtr in payment hist
                }
                
            }
            result2.Close();
        }

        private string GetNoOfQtr(string p_sOrNo)
        {
            // RMC 20130109 display no. of qtr in payment hist
            OracleResultSet pSet = new OracleResultSet();
            string sNoOfQtr = string.Empty;
            string sQtr = string.Empty;
               

            pSet.Query = "select distinct * from pay_hist where or_no = '" + p_sOrNo + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sNoOfQtr = pSet.GetString("no_of_qtr").Trim();
                    sQtr = pSet.GetString("qtr_paid").Trim();

                    if (sQtr == "F")
                        sNoOfQtr = "4";
                    else
                    {
                        if (sNoOfQtr == "")
                        {
                            sNoOfQtr = "1";  
                           
                        }
                    }
                }
            }
            pSet.Close();

            return sNoOfQtr;
        }

        public static string StartingYear()
        {
            OracleResultSet result = new OracleResultSet();
            string sStartYear = string.Empty;
            result.Query = "select distinct tax_year from pay_hist where tax_year <> ' ' order by tax_year asc";
            if(result.Execute())
            {
                if(result.Read())
                {
                    sStartYear = result.GetString("tax_year").Trim();
                }
            }
            result.Close();
            return sStartYear;
        }

        public void CleanMe()
        {
            fPayHist.lblBnsPlate.Text = string.Empty; //MCR 20150129
            fPayHist.lblAppNo.Text = string.Empty; //MCR 20150204
            fPayHist.lblBnsName.Text = string.Empty;
            fPayHist.lblBnsAdd.Text = string.Empty;
            fPayHist.lblOwnName.Text = string.Empty;
            fPayHist.lblOwnAdd.Text = string.Empty;
            fPayHist.bin1.txtTaxYear.Text = string.Empty;
            fPayHist.bin1.txtBINSeries.Text = string.Empty;
            fPayHist.dgvPaymentInfo.Rows.Clear();

            // RMC 20170130 enabled and modified tagging of payment under protest (s)
            fPayHist.btnEditProtest.Enabled = false;
            fPayHist.chkPaymentProtest.Enabled = false;
            fPayHist.chkPaymentProtest.Checked = false;
            fPayHist.m_sRecTaxYear = string.Empty;
            fPayHist.m_sRecOR = string.Empty;
            fPayHist.m_sRecTerm = string.Empty;
            fPayHist.m_sRecQtr = string.Empty;
            // RMC 20170130 enabled and modified tagging of payment under protest (e)
        }

        public void LoadPaymentProtest(string sBin, string sOr, string sTaxYear)
        {
            // RMC 20170130 enabled and modified tagging of payment under protest
            OracleResultSet result = new OracleResultSet();

            result.Query = "select * from protested_payment where bin = '" +sBin+"' and tax_year = '" + sTaxYear + "' and or_no = '" + sOr + "'";
            if (result.Execute())
            {
                if (result.Read())
                    fPayHist.chkPaymentProtest.Checked = true;
                else
                    fPayHist.chkPaymentProtest.Checked = false;
            }
            result.Close();
        }

        public void TagPaymentProtest(string sBin, string sOr, string sTaxYear)
        {
            // RMC 20170130 enabled and modified tagging of payment under protest
            OracleResultSet pCmd = new OracleResultSet();
            string sObject = string.Empty;

            sObject = "BIN: " + sBin + "/Tax Year: " + sTaxYear + "/Or No: " + sOr;
            int iCtr = 0;

            pCmd.Query = "select count(*) from protested_payment where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "' and or_no = '" + sOr + "'";
            if (pCmd.Execute())
            {
                if (pCmd.Read())
                    iCtr = pCmd.GetInt(0);
            }
            pCmd.Close();

            pCmd.Query = "delete from protested_payment where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "' and or_no = '" + sOr + "'";
            if (pCmd.ExecuteNonQuery() == 0)
            { }

            if (fPayHist.chkPaymentProtest.Checked == true)
            {
                pCmd.Query = "insert into protested_payment values (";
                pCmd.Query += "'" + sBin + "', ";
                pCmd.Query += "'" + sOr + "', ";
                pCmd.Query += "'" + sTaxYear + "', ";
                pCmd.Query += "'" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode) + "', ";
                pCmd.Query += "to_date('" + AppSettingsManager.GetCurrentDate().ToShortDateString() + "','MM/dd/yyyy'))";
                if (pCmd.ExecuteNonQuery() == 0)
                { }

                AuditTrail.InsertTrail("CPUP-T", "protested_payment", sObject);
            }
            else
            {
                if (iCtr > 0)
                {
                    AuditTrail.InsertTrail("CPUP-U", "protested_payment", sObject);
                }
            }

            MessageBox.Show("Record updated","Payment Under Protest",MessageBoxButtons.OK,MessageBoxIcon.Information);
            UpdateList(sBin);
        }
    }
}
