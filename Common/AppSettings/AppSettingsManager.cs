// MODIFICATIONS
// GDE 20130116 consider posted payments with taxdues
// RMC 20120301 merged gen log
// RMC 20120112 additional validation in Billing if record already approved in Treasurers module
// ALJ 20111227 save to for SOA Printing
// RMC 20111227 added Gross monitoring module for gross >= 200000
// RMC 20111220 transferred validation of business if business-mapped in AppSettingsManager
// RMC 20111129 modified initialization of serials
// RMC 20111128 display owner's address in SearchOwner
// ALJ 20110907 re-assessment validation
// RMC 20110831 modified GetBnsOwnAdd for owner's query 
// RMC 20110725 Added initialization of serials on change of system Year 

using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using System.IO;
using System.Windows.Forms;
using Amellar.Common.StringUtilities;

namespace Amellar.Common.AppSettings
{
    //public sealed class AppSettingsManager
    public static class AppSettingsManager
    {

        private static string sObject;
        public static SystemUser g_objSystemUser;
        public static string m_sOwnCode = string.Empty;
        public static Teller objTeller;
        public static bool bFreezedDate = false;   // RMC 20110725 modified getting current date
        public static bool m_bOwnerQuery = false;    // RMC 20110831 modified GetBnsOwnAdd for owner's query 

        public static void TellerTransaction(String p_sTeller, String p_sTCode, String p_sBIN, String p_sOrNo, double p_dTAmount, double p_dTCAmount, double p_dCQAmount, double p_dCTender, double p_dChange, String p_sPayType)
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = string.Format(@"insert into teller_transaction(teller,transaction_code,bin,or_no,total_amount,tax_credit,check_amount,cash_tender,change,payment_type,dt_save) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',sysdate)", p_sTeller, p_sTCode, p_sBIN, p_sOrNo, p_dTAmount, p_dTCAmount, p_dCQAmount, p_dCTender, p_dChange, p_sPayType, AppSettingsManager.GetCurrentDate()); //JAA 20190404 added date 
            pSet.ExecuteNonQuery();
        }

        //MCR 20141209 (s)
        private static string m_sSystemType = string.Empty;
        public static string GetSystemType
        {
            get { return m_sSystemType; }
            set { m_sSystemType = value; }
        }
        //MCR 20141209 (e)

        public static bool TaskManager(string p_sObject, string p_sDetails, string p_sMode)
        {
            OracleResultSet pSet = new OracleResultSet();

            if (p_sMode == "DELETE")
            {
                pSet.Query = string.Format("delete from module_info where object = '{0}' and user_code = '{1}' and details = '{2}'", StringUtilities.StringUtilities.HandleApostrophe(p_sObject), StringUtilities.StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode), StringUtilities.StringUtilities.HandleApostrophe(p_sDetails));
                pSet.ExecuteNonQuery();
                return true;
            }
            else
            {
                String sDetails, sUserCode, sTime;

                pSet.Query = string.Format("select * from module_info where object = '{0}'", p_sObject);
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        sDetails = StringUtilities.StringUtilities.HandleApostrophe(pSet.GetString("details").Trim());
                        sUserCode = StringUtilities.StringUtilities.HandleApostrophe(pSet.GetString("user_code").Trim());
                        sTime = pSet.GetDateTime("sys_date").ToString();
                        MessageBox.Show("Access Denied.\n\nModule: " + sDetails + "\nUser: " + sUserCode + "\nTime: " + sTime, "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return true;
                    }
                    else
                    {
                        if (StringUtilities.StringUtilities.HandleApostrophe(p_sObject) != "")
                        {
                            String sCurrentDate = AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");
                            pSet.Query = string.Format("insert into module_info values('{0}',sysdate,'{1}','{2}','COL')", StringUtilities.StringUtilities.HandleApostrophe(p_sObject), StringUtilities.StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode), StringUtilities.StringUtilities.HandleApostrophe(p_sDetails));
                            pSet.ExecuteNonQuery();
                        }
                        return false;
                    }
                pSet.Close();
            }
            return false;
        }

        // MCR20140721 (s)
        public static bool OnCheckIfDiscounted(String p_sCurrentDate)
        {
            OracleResultSet pSet = new OracleResultSet();
            DateTime DtCurrentDate, DtCutOffDate;
            String sQuery = "";

            DtCurrentDate = Convert.ToDateTime(p_sCurrentDate);
            sQuery = string.Format("select * from due_dates where due_year = '{0}' and due_code = '01'", GetSystemDate().Year);
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                if (pSet.Read())
                    DtCutOffDate = pSet.GetDateTime("due_date");
                else
                    DtCutOffDate = Convert.ToDateTime(GetConfigValue("13") + "/" + DtCurrentDate.Year);
            }
            else
                DtCutOffDate = Convert.ToDateTime(GetConfigValue("13") + "/" + DtCurrentDate.Year);
            pSet.Close();

            if (DtCurrentDate.Month == DtCutOffDate.Month)
            {
                if (DtCurrentDate.Day <= DtCutOffDate.Day)
                {
                    if (DtCurrentDate.Year <= DtCutOffDate.Year)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
            {
                if (DtCurrentDate.Month < DtCutOffDate.Month)
                {
                    if (DtCurrentDate.Year <= DtCutOffDate.Year)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        public static bool IsUnderTreasModule(String p_sBin)
        {
            String sQuery;
            bool bResult = false;
            OracleResultSet pSet = new OracleResultSet();

            sQuery = string.Format("select * from treasurers_module where bin = '{0}' and action <> '2'", p_sBin);
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    bResult = true;
            pSet.Close();
            return (bResult);
        }

        public static bool GrossMonitoring(String p_sBin)
        {
            bool bResult = false;
            OracleResultSet pSet = new OracleResultSet();
            String sQuery, sData;

            sQuery = string.Format("select * from gross_monitoring where bin = '{0}' and action = '0'", p_sBin);
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    bResult = true;
            pSet.Close();
            return bResult;
        }

        public static int GetCountFee(String p_sBIN, String p_sFeesCode, String p_sTaxYear)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery;
            int iFeeCount;

            iFeeCount = 0;
            sQuery = "select count(*) as fee_count from taxdues";
            sQuery += " where bin = '" + p_sBIN + "'";
            sQuery += " and tax_year = '" + p_sTaxYear + "'";
            sQuery += " and tax_code = '" + p_sFeesCode + "'";
            pSet.Query = sQuery;
            int.TryParse(pSet.ExecuteScalar(), out iFeeCount);
            pSet.Close();
            return iFeeCount;
        }

        public static double GetPenalty(String sType, String sTaxFeesCode, String sYear)
        {
            OracleResultSet pSet = new OracleResultSet();

            double sInterest = 0, sSurcharge = 0;

            pSet.Query = string.Format("select * from surch_sched where tax_fees_code = '{0}' and rev_year = '{1}'", sTaxFeesCode, sYear);
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sSurcharge = pSet.GetDouble("surch_rate");
                    sInterest = pSet.GetDouble("pen_rate");
                }
                else
                {
                    pSet.Close();

                    pSet.Query = string.Format("select * from surch_sched where tax_fees_code <> 'B' and rev_year = '{0}'", sYear);
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            sSurcharge = pSet.GetDouble("surch_rate");
                            sInterest = pSet.GetDouble("pen_rate");
                        }
                }
            pSet.Close();

            if (sType == "SUR")
                return sSurcharge;
            else
                return sInterest;
        }

        // MCR20140721 (e)

        public static bool bIsSplBns(String p_sBin) // MCR20140711
        {
            String sQuerySplBns;
            bool blnIsSplBns = false;
            OracleResultSet pSet = new OracleResultSet();
            sQuerySplBns = string.Format("select * from spl_business_que where bin = '{0}' ", p_sBin);
            pSet.Query = sQuerySplBns;
            if (pSet.Execute())
            {
                if (pSet.Read())
                    blnIsSplBns = true;
                else
                {
                    pSet.Close();
                    sQuerySplBns = string.Format("select * from spl_businesses where bin = '{0}' ", p_sBin);		// JJP 03222006 include review of prior years
                    pSet.Query = sQuerySplBns;
                    if (pSet.Execute())
                        if (pSet.Read())
                            blnIsSplBns = true;
                }
            }
            pSet.Close();

            return blnIsSplBns;
        }

        public static string GenerateTmpAccountNo()
        {
            OracleResultSet pSet = new OracleResultSet();

            String sQuery, sYear, sTmpAccountNo = "0";
            int iTmpAccountNo, iTmpAccountNo_Cnt;
            bool bLoop = true;

            iTmpAccountNo = -1;
            sQuery = "select * from def_tmp_acct";
            if (pSet.Execute())
                if (pSet.Read())
                    iTmpAccountNo = pSet.GetInt("def_tmp_acctno");
                else
                    MessageBox.Show("NO Serial for Temporary Account No. Contact your System Developer", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            pSet.Close();

            if (iTmpAccountNo >= 0)
            {
                while (bLoop == true)
                {
                    iTmpAccountNo = iTmpAccountNo + 1;
                    sTmpAccountNo = iTmpAccountNo.ToString();

                    sQuery = string.Format("select * from def_tmp_acct where def_tmp_acctno >= {0}", iTmpAccountNo);
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            sQuery = string.Format("update def_tmp_acct set def_tmp_acctno = {0}", iTmpAccountNo);
                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();
                            bLoop = false;
                        }
                    pSet.Close();
                }


                sYear = GetSystemDate().Year.ToString();

                iTmpAccountNo_Cnt = sTmpAccountNo.Length;

                switch (iTmpAccountNo_Cnt)
                {
                    case 1:
                        sTmpAccountNo = "000000" + sTmpAccountNo;
                        break;
                    case 2:
                        sTmpAccountNo = "00000" + sTmpAccountNo;
                        break;
                    case 3:
                        sTmpAccountNo = "0000" + sTmpAccountNo;
                        break;
                    case 4:
                        sTmpAccountNo = "000" + sTmpAccountNo;
                        break;
                    case 5:
                        sTmpAccountNo = "00" + sTmpAccountNo;
                        break;
                    case 6:
                        sTmpAccountNo = "0" + sTmpAccountNo;
                        break;
                    case 7:
                        break;
                }
                sTmpAccountNo = sYear + "-" + sTmpAccountNo;
            }
            return sTmpAccountNo;
        }

        public static string GenerateTempOR()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();

            String sQuery, sOldOR;
            int iOldOR, iOldORCnt;
            bool bLoop = true;

            iOldOR = Convert.ToInt32(GetConfigValue("21"));
            sOldOR = iOldOR.ToString();

            if (sOldOR == "")
            {
                MessageBox.Show("CODE 21:TEMP OR No. Not Found! Contact System Administrator", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            if (iOldOR >= 0)
            {
                while (bLoop == true)
                {
                    iOldOR += 1;
                    sOldOR = iOldOR.ToString();

                    pSet.Query = string.Format("select * from config where code = '21' and object >= {1}", iOldOR);
                    if (pSet.Execute())
                        if (pSet.Read())
                        {
                            pSet.Query = string.Format("update config set object = {0} where code = '21'", iOldOR);
                            pSet.ExecuteNonQuery();
                        }
                    pSet.Close();
                }

                iOldORCnt = sOldOR.Length;

                switch (iOldORCnt)
                {
                    case 1:
                        sOldOR = "TMP000000" + sOldOR;
                        break;
                    case 2:
                        sOldOR = "TMP00000" + sOldOR;
                        break;
                    case 3:
                        sOldOR = "TMP0000" + sOldOR;
                        break;
                    case 4:
                        sOldOR = "TMP000" + sOldOR;
                        break;
                    case 5:
                        sOldOR = "TMP00" + sOldOR;
                        break;
                    case 6:
                        sOldOR = "TMP0" + sOldOR;
                        break;
                    case 7:
                        sOldOR = "TMP" + sOldOR;
                        break;
                }
            }
            return sOldOR;
        }

        public static bool OpenValidation(String p_sHoldField, String p_sHoldAccountNo)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();
            OracleResultSet pSet3 = new OracleResultSet();

            String sFieldCode, sDefCode, m_sRetrieveDefCode = "";

            pSet.Query = string.Format("select * from def_field_table where trim(field_name) like '{0}'", p_sHoldField);	// JGR 09202005 Oracle Adjustment
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sFieldCode = pSet.GetString("field_code").Trim();

                    pSet1.Query = string.Format("select * from def_field_valid where trim(field_code) like '{0}'", sFieldCode);	// JGR 09202005 Oracle Adjustment
                    if (pSet1.Execute())
                        if (pSet1.Read())
                        {
                            sDefCode = pSet.GetString("def_code").Trim();
                            m_sRetrieveDefCode = sDefCode;  // JJP 08242004 RETRIEVE DEF_CODE

                            pSet2.Query = string.Format("select * from def_records_tmp where trim(def_code) like '{0}' and rec_acct_no = '{1}' and def_status <> '-1'", sDefCode, p_sHoldAccountNo);	// JGR 09202005 Oracle Adjustment
                            if (pSet2.Execute())
                                if (pSet2.Read())
                                    return true;
                                else
                                {
                                    pSet3.Query = string.Format("select * from def_records where trim(def_code) like '{0}' and rec_acct_no = '{1}' and def_status <> '-1'", sDefCode, p_sHoldAccountNo);	// JGR 09202005 Oracle Adjustment
                                    if (pSet3.Execute())
                                        if (pSet3.Read())
                                            return true;
                                        else
                                            return false;
                                    pSet3.Close();
                                }
                            pSet2.Close();
                        }
                        else
                            return false;
                    pSet1.Close();
                }
                else
                    return false;
            pSet.Close();

            return false;
        }

        public static string GetBnsOwnerGender(string sOwnCode) //MCR 20140305
        {
            string sGender = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select gender from own_profile where own_code = '" + sOwnCode.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sGender = result.GetString("gender").Trim();
                }
            }
            result.Close();

            return sGender;
        }

        public static string GetBnsOwnerLastName(string sOwnCode) //MCR 20140305
        {
            string sLn = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from own_names where own_code = '" + sOwnCode.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sLn = result.GetString("own_ln").Trim();
                }
            }
            result.Close();

            return sLn;
        }
        public static string GetBnsOwnerFirstName(string sOwnCode) //MCR 20150114
        {
            string sFn = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from own_names where own_code = '" + sOwnCode.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sFn = result.GetString("own_fn").Trim();
                }
            }
            result.Close();

            return sFn;
        }
        public static string GetBnsOwnerMiName(string sOwnCode) //MCR 20150114
        {
            string sMi = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from own_names where own_code = '" + sOwnCode.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sMi = result.GetString("own_mi").Trim();
                }
            }
            result.Close();

            return sMi;
        }

        public static string ForSearch(String x) //MCR 20140224
        {
            if (x.Trim() == String.Empty)
            {
                x = "%";
            }
            return x;
        }

        public static string GetretAppNo()
        {
            string sAppNo = string.Empty;
            string syear = string.Empty;
            int iSerial = 0;
            int iNewSerial = 0;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from retirement_serial where year = '" + AppSettingsManager.GetSystemDate().Year + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    iSerial = result.GetInt("ret_serial");
                    syear = result.GetString("year").Trim();
                }
                else
                {
                    result.Query = "insert into retirement_serial values(:1,:2)";
                    result.AddParameter(":1", AppSettingsManager.GetSystemDate().Year);
                    result.AddParameter(":2", 1);
                    if (result.ExecuteNonQuery() != 0)
                    { }
                    result.Close();

                    iSerial = 1;
                }
            }
            result.Close();

            result.Query = "update retirement_serial set ret_serial = :1 where year = :2";
            result.AddParameter(":1", iSerial + 1);
            result.AddParameter(":2", AppSettingsManager.GetSystemDate().Year);
            if (result.ExecuteNonQuery() != 0)
            {
            }
            result.Close();

            return iSerial.ToString();
        }

        public static int GetCountBnsPaid(string sBnsBrgy)
        {
            int iBnsCount = 0;
            string sBin = string.Empty;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();

            result.Query = "select count(distinct(bin)) as binCount from btm_temp_businesses where bns_brgy = '" + sBnsBrgy + "' and bin in (select bin from pay_hist where data_mode <> 'UNP' and or_no in (select or_no from or_table))";
            if (result.Execute())
            {
                if (result.Read())
                {
                    iBnsCount = result.GetInt("binCount");
                }
            }
            result.Close();

            return iBnsCount;
        }

        public static double GetAmountBnsPaid(string sBnsBrgy)
        {
            double dBnsAmt = 0;
            string sBin = string.Empty;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();

            result.Query = "select sum(fees_amtdue) as sumFees from or_table where or_no in (select or_no from pay_hist where bin in (select distinct(bin) as bin from btm_temp_businesses where bns_brgy = '" + sBnsBrgy + "') and data_mode <> 'UNP')";
            if (result.Execute())
            {
                if (result.Read())
                {
                    try
                    {
                        dBnsAmt = result.GetDouble("sumFees");
                    }
                    catch
                    {
                        dBnsAmt = 0;
                    }
                }
            }
            result.Close();


            return dBnsAmt;
        }

        public static string GetBankCode()
        {
            string sBankCode = string.Empty;
            int iBankCode = 0;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from bank_table order bank_code desc";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sBankCode = result.GetString("bank_code").Trim();
                    sBankCode = string.Format("{###}", int.Parse(sBankCode.Trim()) + 1);
                }
                else
                    sBankCode = "001";
            }
            result.Close();
            return sBankCode;
        }
        public static Teller TellerUser
        {
            get { return objTeller; }
            set { objTeller = value; }
        }
        public static SystemUser SystemUser
        {
            get { return g_objSystemUser; }
            set { g_objSystemUser = value; }
        }

        public static string GetOwnBrgy(String sBin)
        {
            String sBrgy = "", sQuery;

            sQuery = string.Format("select bns_brgy from business_que where bin = '{0}'", sBin); // ALJ 12152002 change "businesses to business_que"
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    sBrgy = pSet.GetString("bns_brgy").Trim();
                else
                {
                    pSet.Close();
                    sQuery = string.Format("select bns_brgy from businesses where bin = '{0}'", sBin); // ALJ 12152002 change "businesses to business_que"
                    pSet = new OracleResultSet();
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                            sBrgy = pSet.GetString("bns_brgy").Trim();
                }
            pSet.Close();

            return sBrgy;
        }

        public static double ComputeOtherFees(String sOrNo)
        {
            double dAmount = 0;
            String sQuery, sData;

            sQuery = string.Format("select sum(fees_amtdue) as AmountDue from or_table where or_no = '{0}' and fees_code between '07' and '15'", sOrNo);
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    dAmount = pSet.GetDouble("AmountDue");
            pSet.Close();
            return (dAmount);
        }

        public static string GetTaxPayerName(String Bin)
        {
            String sQuery, sUserLN, sUserFN, sUserMI, sTellerName = "", sOwnCode = "";
            OracleResultSet pSet = new OracleResultSet();
            sQuery = string.Format("select own_code from businesses where bin = '{0}'", Bin);
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sOwnCode = StringUtilities.StringUtilities.HandleApostrophe(pSet.GetString("own_code").Trim());
                }
                else
                {
                    pSet.Close();
                    pSet = new OracleResultSet();
                    sQuery = string.Format("select own_code from business_que where bin = '{0}'", Bin);
                    pSet.Query = sQuery;
                    if (pSet.Execute())
                        if (pSet.Read())
                            sOwnCode = StringUtilities.StringUtilities.HandleApostrophe(pSet.GetString("own_code").Trim());
                }
            pSet.Close();

            pSet = new OracleResultSet();
            sQuery = string.Format("select * from own_names where own_code = '{0}'", sOwnCode);
            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sUserLN = StringUtilities.StringUtilities.HandleApostrophe(pSet.GetString("own_ln").Trim());
                    sUserFN = StringUtilities.StringUtilities.HandleApostrophe(pSet.GetString("own_fn").Trim());
                    sUserMI = StringUtilities.StringUtilities.HandleApostrophe(pSet.GetString("own_mi").Trim());
                    sTellerName = sUserFN;
                    if (sUserMI != "" && sUserMI != ".")  //LEO 10172002
                    {
                        sTellerName = sTellerName + " " + sUserMI + ".";
                    }
                    sTellerName = sTellerName + " " + sUserLN;
                }
            pSet.Close();

            return sTellerName;
        }

        public static string GetTeller(String sTellerCode, int swFormat)  //LEO 02052003
        {
            string sQuery, sTeller = String.Empty;

            sQuery = string.Format("select * from tellers where teller = '{0}'", sTellerCode); // CTS 09152003
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    if (swFormat == 0)
                        sTeller = StringUtilities.StringUtilities.HandleApostrophe(pSet.GetString("fn").Trim()) + " " +
                                  StringUtilities.StringUtilities.HandleApostrophe(pSet.GetString("mi").Trim()) + ". " +
                                  StringUtilities.StringUtilities.HandleApostrophe(pSet.GetString("ln").Trim());
                    else
                        sTeller = StringUtilities.StringUtilities.HandleApostrophe(pSet.GetString("ln").Trim()) + ", " +
                                  StringUtilities.StringUtilities.HandleApostrophe(pSet.GetString("fn").Trim()) + " " +
                                  StringUtilities.StringUtilities.HandleApostrophe(pSet.GetString("mi").Trim()) + ".";
                }
            }
            pSet.Close();
            return sTeller;
        }

        /// <summary>
        /// This static method returns current date/time of database server
        /// </summary>
        /// <returns>the current date/time</returns>
        public static DateTime GetSystemDate() // ALJ 20090902 change to public // pending cosder freeze date
        {
            DateTime dtSystemDate = DateTime.Now;
            OracleResultSet result = new OracleResultSet();

            bFreezedDate = false;   // RMC 20110725 modified getting current date

            // ALJ 20100616 (s) test if system date is freezed
            result.Query = "SELECT or_datetime FROM or_date_setup WHERE freeze = '1'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    dtSystemDate = result.GetDateTime(0); // freeze system date
                    bFreezedDate = true;   // RMC 20110725 modified getting current date
                }
                else
                {
                    result.Close(); //MCR 20140618
                    // move inside the else clause
                    result.Query = "SELECT SYSDATE FROM DUAL";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            dtSystemDate = result.GetDateTime(0); // actual system date
                        }
                    }
                    //
                }
            }
            // ALJ 20100616 (e)test if system date is freezed
            result.Close();

            // RMC 20110725 Added initialization of serials on change of system Year (s)
            string sTmpSysYear = string.Empty;
            sTmpSysYear = dtSystemDate.Year.ToString();

            if (AppSettingsManager.GetConfigValue("12").Trim() != sTmpSysYear.Trim())
            {
                result.Query = "update buss_series set bin = '0'";
                if (result.ExecuteNonQuery() == 0)
                { }

                result.Query = "update bill_series set bill_no = '0'";
                if (result.ExecuteNonQuery() == 0)
                { }

                result.Query = "update mp_series set mp_no = '0'";
                if (result.ExecuteNonQuery() == 0)
                { }

                result.Query = string.Format("update config set object = '{0}' where code = '12'", sTmpSysYear);
                if (result.ExecuteNonQuery() == 0)
                { }
            }
            // RMC 20110725 Added initialization of serials on change of system Year (e)

            return dtSystemDate;

            //return new DateTime(2009, 01, 13);
        }

        public static string GetQtr(string m_sDateOperated)
        {
            int iMonth = 0;
            int iDay = 0;
            string sQtr = string.Empty;

            int.TryParse(m_sDateOperated.Substring(0, 2), out iMonth);
            if (iMonth < 4)
                sQtr = "1";
            else if (iMonth < 7)    // RMC 20110414 added else
                sQtr = "2";
            else if (iMonth < 10)   // RMC 20110414 added else
                sQtr = "3";
            else
                sQtr = "4";

            return sQtr;
        }

        //public static bool bEnlistOwner(string strOwnLn, string strOwnFn, string strOwnHouseNo, string strOwnSt, string strOwnBrgy, string strOwnMun, string strOwnProv)
        // RMC 20110414 (s)
        public static string EnlistOwner(string strOwnLn, string strOwnFn, string strOwnMI, string strOwnHouseNo, string strOwnSt, string strOwnDist, string strOwnZone, string strOwnBrgy, string strOwnMun, string strOwnProv, string strOwnZip)
        {
            bool bResult = true;
            OracleResultSet result = new OracleResultSet();
            try
            {
                //result.Query = string.Format("select * from own_names where own_ln = '{0}' and (own_fn = '{1}' or own_fn is null or own_fn = ' ') and (own_mi = '{2}' or own_mi is null or own_mi = ' ') and (own_house_no = '{3}' or own_house_no is null or own_house_no = ' ') and own_street = '{4}' and (own_brgy = '{5}' or own_brgy is null or own_brgy = ' ') and own_mun = '{6}' and (own_prov = '{7}' or own_prov is null or own_prov = ' ') and (own_zip = '{8}' or own_zip is null or own_zip = ' ')", strOwnLn.Trim(), strOwnFn.Trim(), strOwnMI.Trim(), strOwnHouseNo.Trim(), strOwnSt.Trim(), strOwnBrgy.Trim(), strOwnMun.Trim(), strOwnProv.Trim(), strOwnZip.Trim()); // RMC 20110414 added zip
                //result.Query = string.Format("select * from own_names where own_ln = '{0}' and (own_fn = '{1}' or own_fn is null or own_fn = ' ') and (own_mi = '{2}' or own_mi is null or own_mi = ' ') and (own_house_no = '{3}' or own_house_no is null or own_house_no = ' ') and own_street = '{4}' and (own_brgy = '{5}' or own_brgy is null or own_brgy = ' ') and own_mun = '{6}' and (own_prov = '{7}' or own_prov is null or own_prov = ' ') and (own_zip = '{8}' or own_zip is null or own_zip = ' ')", strOwnLn.Trim(), strOwnFn, strOwnMI, strOwnHouseNo, strOwnSt, strOwnBrgy, strOwnMun, strOwnProv, strOwnZip); // RMC 20110725 corrected enlisting of new owner 

                // RMC 20110801 corrected enlisting of new owner (s)
                result.Query = string.Format("select * from own_names where own_ln = '{0}'", StringUtilities.StringUtilities.HandleApostrophe(strOwnLn.Trim()));
                result.Query += string.Format(" and (own_fn = '{0}' or own_fn is null or own_fn = ' ') ", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnFn)));
                result.Query += string.Format(" and (own_mi = '{0}' or own_mi is null or own_mi = ' ') ", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnMI)));
                result.Query += string.Format(" and (own_house_no = '{0}' or own_house_no is null or own_house_no = ' ') ", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnHouseNo)));
                result.Query += string.Format(" and own_street = '{0}' ", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnSt)));
                result.Query += string.Format(" and (own_brgy = '{0}' or own_brgy is null or own_brgy = ' ') ", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnBrgy)));
                result.Query += string.Format(" and own_mun = '{0}' and ", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnMun)));
                result.Query += string.Format(" (own_prov = '{0}' or own_prov is null or own_prov = ' ') ", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnProv)));
                result.Query += string.Format(" and (own_zip = '{0}' or own_zip is null or own_zip = ' ') ", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnZip)));
                // RMC 20110801 corrected enlisting of new owner (e)
                if (result.Execute())
                {
                    if (result.Read())
                        m_sOwnCode = result.GetString("own_code").Trim();
                    //bResult = false;
                    else
                        m_sOwnCode = "";
                }
                result.Close();
            }
            catch (Exception a)
            { MessageBox.Show(a.ToString(), " ", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
            finally
            {
                result.Close();
            }

            if (m_sOwnCode == string.Empty)
            {
                if (MessageBox.Show("Enlist new owner?", "BPLS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
                            Serials.UpdateOwnSerial();
                            m_sOwnCode = Serials.GetOwnSerial();
                            SaveNewOwner(m_sOwnCode, strOwnLn, strOwnFn, strOwnMI, strOwnHouseNo, strOwnSt, strOwnDist, strOwnZone, strOwnBrgy, strOwnMun, strOwnProv, strOwnZip);
                            break;
                        }
                        catch
                        {
                            m_sOwnCode = string.Empty;
                            //an error has occurred
                        }
                    }
                }
            }
            //return bResult;

            return m_sOwnCode;
        }
        // RMC 20110414 (e)


        /// <summary>
        /// This static method sets current date/time of database server 
        /// </summary>
        /// <param name="dtCurrentDate">the date/time to be set</param>
        /// <returns>returns true if new date was successfully set otherwise false</returns>
        public static bool SetCurrentDate(DateTime dtCurrentDate)
        {
            OracleResultSet result = new OracleResultSet();

            result.Query = "SELECT COUNT(*) FROM datetime_setting WHERE description = :1";
            result.AddParameter(":1", "current_datetime");

            int intCount = 0;
            int.TryParse(result.ExecuteScalar().ToString(), out intCount);
            if (intCount == 0)
            {
                result.Query = "INSERT INTO datetime_setting VALUES (:1, :2)";
                result.AddParameter(":1", "current_datetime");
                result.AddParameter(":2", string.Format("{0:MM/dd/yyyy HH:mm:ss}", dtCurrentDate));
                if (result.ExecuteNonQuery() == 0)
                {
                    result.Close();
                    return false;
                }
            }
            else
            {
                result.Query = "UPDATE datetime_setting SET datetime = :1 WHERE description = :2";
                result.AddParameter(":1", string.Format("{0:MM/dd/yyyy HH:mm:ss}", dtCurrentDate));
                result.AddParameter(":2", "current_datetime");
                if (result.ExecuteNonQuery() == 0)
                {
                    result.Close();
                    return false;
                }
            }
            result.Close();
            return true;
        }

        public static bool blnIsConflict(string sBin)
        {
            OracleResultSet result = new OracleResultSet();
            bool blnIsCon = false;
            result.Query = "SELECT * FROM payment_conflict WHERE bin = :1";
            result.AddParameter(":1", sBin);
            if (result.Execute())
            {
                if (result.Read())
                    blnIsCon = true;
            }
            result.Close();
            return blnIsCon;
        }

        public static DateTime GetCurrentDate(bool blnHasUpdate)
        {
            DateTime dtSystemDate = GetSystemDate();
            /*
            if (blnHasUpdate && SetCurrentDate(dtSystemDate))
            {
            }
            else
            {
                OracleResultSet result = new OracleResultSet();
                result.Query = "SELECT TO_DATE(TRIM(datetime),'mm/dd/yyyy HH24:MI:SS') FROM datetime_setting";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        dtSystemDate =  result.GetDateTime(0);
                    }
                }
                result.Close();
            }
            */
            return dtSystemDate;
        }

        public static DateTime GetCurrentDate()
        {
            //return GetCurrentDate(true);
            return GetCurrentDate(false);
        }

        /*
        private static string GetValue(string strTable, string strConfigField, string strValueField, string strCode)
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = string.Format("SELECT {0} FROM {1} WHERE {2} = :1",
                strValueField, strTable, strConfigField);
            result.AddParameter(":1", strCode);
            if (result.Execute())
            {
                if (result.Read())
                {
                    return result.GetString(0).Trim();
                }
            }
            return string.Empty;
        }

        public static string GetConfigValue(string strConfigCode)
        {
            return AppSettingsManager.GetValue("config_table", "subj_code", "value_fld", strConfigCode);
        }

        public static string GetErrorValue(string strErrorCode)
        {
            return AppSettingsManager.GetValue("error_tbl", "error_code", "error_desc");
        }
         */

        /*
        public static string GetConfigValueByDescription(string strConfigDescription)
        {
            string strConfigValue = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "SELECT value_fld FROM config_table WHERE subj_desc = RPAD(:1, 255)";
            result.AddParameter(":1", strConfigDescription);
            if (result.Execute())
            {
                if (result.Read())
                {
                    strConfigValue = result.GetString(0).Trim();
                }
            }
            result.Close();
            return strConfigValue;
        }
        */

        // GDE 20101109
        public static void UpdateTaxDues(string sQtrToPay, string sBin, string sTaxYear)
        {
            OracleResultSet resultUpdateTaxDues = new OracleResultSet();
            resultUpdateTaxDues.Query = "update taxdues set qtr_to_pay = '" + sQtrToPay.Trim() + "' where bin = '" + sBin.Trim() + "' and tax_year = '" + sTaxYear.Trim() + "'";
            if (resultUpdateTaxDues.ExecuteNonQuery() != 0)
            {

            }
            resultUpdateTaxDues.Close();
        }

        public static void DeleteBillNo(string sBin, string sTaxYear)
        {
            OracleResultSet resDeleteBillNo = new OracleResultSet();
            resDeleteBillNo.Query = "delete from bill_no where bin = '" + sBin.Trim() + "' and tax_year = '" + sTaxYear.Trim() + "'";
            if (resDeleteBillNo.ExecuteNonQuery() != 0)
            {

            }
            resDeleteBillNo.Close();
        }
        public static void DeleteTaxDues(string sBin, string sTaxYear)
        {
            OracleResultSet resDeleteTaxdues = new OracleResultSet();
            resDeleteTaxdues.Query = "delete from taxdues where bin = '" + sBin.Trim() + "' and tax_year = '" + sTaxYear.Trim() + "'";
            if (resDeleteTaxdues.ExecuteNonQuery() != 0)
            {

            }
            resDeleteTaxdues.Close();
        }

        public static void DeletePartialPayer(string sBin, string sTaxYear)
        {
            OracleResultSet resDeletePartialPayer = new OracleResultSet();
            resDeletePartialPayer.Query = "delete from partial_payer where bin = '" + sBin.Trim() + "' and tax_year = '" + sTaxYear.Trim() + "'";
            if (resDeletePartialPayer.ExecuteNonQuery() != 0)
            {

            }
            resDeletePartialPayer.Close();
        }

        public static bool Granted(string strGrantedCode)
        {
            bool bGranted = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from sys_rights where usr_code = '" + AppSettingsManager.SystemUser.UserCode + "' and usr_rights = '" + strGrantedCode.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                    bGranted = true;
                else
                {
                    MessageBox.Show("Access Denied!");
                    bGranted = false;
                }
            }
            result.Close();
            return bGranted;
        }
        public static int MultiTaxYear(string sBin)
        {
            int iNoTaxYear = 0;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select count(distinct(tax_year)) as iCount from taxdues where bin = '" + sBin.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    iNoTaxYear = result.GetInt("iCount");
                }
            }
            result.Close();
            return iNoTaxYear;
        }
        public static bool IfAutoGeneratedOR()
        {
            bool bResult = false;
            OracleResultSet resultAutoGenOr = new OracleResultSet();
            resultAutoGenOr.Query = "select * from config where code = '22' and object = 'Y' or object = 'y'";
            if (resultAutoGenOr.Execute())
            {
                if (resultAutoGenOr.Read())
                    bResult = true;
                else
                    bResult = false;
            }
            resultAutoGenOr.Close();

            return bResult;
        }

        public static bool CheckDuplicateOR(string sOrNo)
        {
            bool bResult = false;

            OracleResultSet result = new OracleResultSet();
            result.Query = "select distinct or_no from pay_hist where or_no = '" + sOrNo.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                    bResult = true;
            }
            result.Close();
            return bResult;
        }
        public static string GetLastPaymentInfo(string sBin, string sInfo)
        {
            string sOrNo = string.Empty;
            string sAmount = string.Empty;
            double dAmount = 0;

            OracleResultSet result = new OracleResultSet();
            result.Query = "select distinct * from pay_hist where bin = '" + sBin.Trim() + "' and data_mode <> 'UNP'  order by or_date desc, qtr_paid desc";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sOrNo = result.GetString("or_no").Trim();
                }
            }
            result.Close();

            result.Query = "select sum(fees_amtdue) as amount from or_table where or_no = '" + sOrNo.Trim() + "'";
            if (result.Execute())
            {

                if (result.Read())
                {
                    dAmount = result.GetDouble("amount");
                    sAmount = string.Format("{0:#,##0.00}", dAmount);
                }
            }
            result.Close();

            if (sInfo.Trim() == "OR")
                return sOrNo.Trim();
            else
                return sAmount;
        }

        public static double GetBNSTaxDues(string sBin, string sTaxYear)
        {
            double dResult = 0;
            OracleResultSet result = new OracleResultSet();
            string sMaxTaxYear = string.Empty;
            string sMaxQtrPaid = string.Empty;
            double dBnsTax = 0;

            //result.Query = "select distinct * from pay_hist where bin = :1 order by tax_year desc, qtr_paid desc";
            //result.Query = "select * from pay_temp where bin = '192-00-2011-0000466' and qtr_to_pay not in (select max(qtr_paid) as qtr_paid from pay_hist where tax_year = '2011') and tax_year = '2011' and qtr_to_pay = 'F';
            // result.AddParameter(":1", sBin);
            // if (result.Execute())
            // {
            //    if (result.Read())
            //    {
            //        sMaxQtrPaid = result.GetString("qtr_paid").Trim();
            //       sMaxTaxYear = result.GetString("tax_year").Trim();
            //   }

            // }
            // result.Close();

            // if (sMaxQtrPaid.Trim() == "F")
            //{
            //    sMaxTaxYear = string.Format("{0:0}", int.Parse(sMaxTaxYear) + 1);
            //    sMaxQtrPaid = "0";
            //}
            //if (sMaxQtrPaid.Trim() == "4")
            // {
            //     sMaxTaxYear = string.Format("{0:0}", int.Parse(sMaxTaxYear) + 1);
            //     sMaxQtrPaid = "0";
            // }

            result.Query = "select sum(fees_due) as fees_due from pay_temp where bin = :1 and qtr_to_pay not in (select max(qtr_paid) as qtr_paid from pay_hist where tax_year = :2) and tax_year = :3 and qtr_to_pay = 'F' and fees_code like 'B%'";
            result.AddParameter(":1", sBin);
            result.AddParameter(":2", sTaxYear);
            result.AddParameter(":3", sTaxYear);
            if (result.Execute())
            {
                if (result.Read())
                {
                    try
                    {
                        dResult = result.GetDouble("fees_due");
                    }
                    catch
                    { }
                }
            }
            result.Close();
            return dResult;
        }

        public static double GetSurchDues(string sBin, string sTaxYear)
        {
            /*if (sBin == "192-00-2011-0005615")
                sBin = sBin;*/
            // RMC 20141202 deleted hard-coded bin lgu code
            double dResult = 0;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select sum(fees_surch) as fees_surch from pay_temp where bin = :1 and qtr_to_pay not in (select max(qtr_paid) as qtr_paid from pay_hist where tax_year = :2) and tax_year = :3 and qtr_to_pay = 'F'";
            result.AddParameter(":1", sBin);
            result.AddParameter(":2", sTaxYear);
            result.AddParameter(":3", sTaxYear);
            if (result.Execute())
            {
                if (result.Read())
                {
                    try
                    {
                        dResult = result.GetDouble("fees_surch");
                    }
                    catch
                    { }
                }
            }
            result.Close();
            return dResult;
        }

        public static double GetPenDues(string sBin, string sTaxYear)
        {
            double dResult = 0;
            OracleResultSet result = new OracleResultSet();
            /*if (sBin == "192-00-2011-0005615")
                sBin = sBin;*/
            // RMC 20141202 deleted hard-coded bin lgu code
            result.Query = "select sum(fees_pen) as fees_pen from pay_temp where bin = :1 and qtr_to_pay not in (select max(qtr_paid) as qtr_paid from pay_hist where tax_year = :2) and tax_year = :3 and qtr_to_pay = 'F'";
            result.AddParameter(":1", sBin);
            result.AddParameter(":2", sTaxYear);
            result.AddParameter(":3", sTaxYear);
            if (result.Execute())
            {
                if (result.Read())
                {
                    try
                    {
                        dResult = result.GetDouble("fees_pen");
                    }
                    catch
                    { }
                }
            }
            result.Close();
            return dResult;
        }

        public static double GetRegTaxDues(string sBin, string sTaxYear)
        {
            double dResult = 0;
            OracleResultSet result = new OracleResultSet();
            string sMaxTaxYear = string.Empty;
            string sMaxQtrPaid = string.Empty;
            double dBnsTax = 0;

            result.Query = "select sum(fees_due) as fees_due from pay_temp where bin = :1 and qtr_to_pay not in (select max(qtr_paid) as qtr_paid from pay_hist where tax_year = :2) and tax_year = :3 and qtr_to_pay = 'F' and fees_code_sort <> '00'";
            result.AddParameter(":1", sBin);
            result.AddParameter(":2", sTaxYear);
            result.AddParameter(":3", sTaxYear);
            if (result.Execute())
            {
                if (result.Read())
                {
                    try
                    {
                        dResult = result.GetDouble("fees_due");
                    }
                    catch
                    { }
                }
            }
            result.Close();
            return dResult;
        }

        public static string GetBnsDesc(string sBnsCode)
        {
            OracleResultSet result1 = new OracleResultSet();
            string strRevYear = AppSettingsManager.GetConfigValue("07");    // RMC 20110308

            result1.Query = "select * from bns_table where bns_code = '" + sBnsCode.Trim() + "' and fees_code = 'B' and rev_year = '" + strRevYear + "'";    // RMC 20110308
            if (result1.Execute())
            {
                if (result1.Read())
                {
                    sBnsCode = result1.GetString("bns_desc").Trim();
                }
            }
            result1.Close();
            return sBnsCode;
        }

        public static string GetBnsOwner(string sOwnCode)
        {
            string sLn = string.Empty;
            string sFn = string.Empty;
            string sMi = string.Empty;
            string sName = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from own_names where own_code = '" + sOwnCode.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sLn = result.GetString("own_ln").Trim();
                    sFn = result.GetString("own_fn").Trim();
                    sMi = result.GetString("own_mi").Trim();
                }
            }
            result.Close();

            // GDE 20120531
            if (sFn.Trim() == ".")
                sFn = string.Empty;
            if (sMi.Trim() == ".")
                sMi = string.Empty;
            // GDE 20120531

            if (sMi.Trim() == string.Empty)
                sName = sFn + " " + sLn;
            else
                sName = sFn + " " + sMi + ". " + sLn;

            if (sFn.Trim() == string.Empty && sMi.Trim() == string.Empty)
                sName = sLn;

            return sName;
        }

        public static bool bIsPrinted(string sBin)
        {
            bool bWatch = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from PRINTED_CDO_UNOF where is_number = '" + sBin.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                    bWatch = true;
            }
            result.Close();
            return bWatch;
        }

        public static bool bIsPEZAMember(string sBin)
        {
            bool bWatch = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from BOI_TABLE where bin = '" + sBin.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                    bWatch = true;
            }
            result.Close();
            return bWatch;
        }

        public static string GetBillNoAndDate(string sBin, string sTaxYear, string sBnsCode)
        {
            OracleResultSet result = new OracleResultSet();
            string sBillNo = string.Empty;
            string sBillDate = string.Empty;
            DateTime dBillDate = new DateTime();
            string sInfo = string.Empty;
            result.Query = "select * from bill_no where bin = '" + sBin.Trim() + "' and tax_year = '" + sTaxYear.Trim() + "' and bns_code_main = '" + sBnsCode.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sBillNo = result.GetString("bill_no").Trim();
                    //dBillDate = result.GetDateTime("bill_date");
                    //sBillDate = MonthsInWords(dBillDate);
                    sBillDate = result.GetDateTime("bill_date").ToShortDateString();
                }
            }
            result.Close();

            sInfo = sBillNo + "      " + sBillDate;
            return sInfo;
        }

        public static string GetBillNoAndDate(string sBin, string sTaxYear, string sBnsCode, int iFrmt)
        {
            OracleResultSet result = new OracleResultSet();
            string sBillNo = string.Empty;
            string sBillDate = string.Empty;
            string sInfo = string.Empty;
            result.Query = "select * from bill_no where bin = '" + sBin.Trim() + "' and tax_year = '" + sTaxYear.Trim() + "' and bns_code_main = '" + sBnsCode.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sBillNo = result.GetString("bill_no").Trim();
                    //dBillDate = result.GetDateTime("bill_date");
                    //sBillDate = MonthsInWords(dBillDate);
                    sBillDate = result.GetDateTime("bill_date").ToShortDateString();
                }
            }
            result.Close();
            if (iFrmt == 0)
                sInfo = sBillNo;
            else //1
                sInfo = sBillDate;
            return sInfo;
        }

        public static string GetBnsAddress(string sBin)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result1 = new OracleResultSet();
            string sBnsHouseNo = string.Empty;
            string sBnsStreet = string.Empty;
            string sBnsBrgy = string.Empty;
            string sBnsMun = string.Empty;
            string sAddress = string.Empty;

            result.Query = "select * from businesses where bin = '" + sBin.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sBnsHouseNo = result.GetString("bns_house_no").Trim();
                    sBnsStreet = result.GetString("bns_street").Trim();
                    sBnsBrgy = result.GetString("bns_brgy").Trim();
                    sBnsMun = result.GetString("bns_mun").Trim();
                }
                else
                {
                    result1.Query = "select * from business_que where bin = '" + sBin.Trim() + "'";
                    if (result1.Execute())
                    {
                        if (result1.Read())
                        {
                            sBnsHouseNo = result1.GetString("bns_house_no").Trim();
                            sBnsStreet = result1.GetString("bns_street").Trim();
                            sBnsBrgy = result1.GetString("bns_brgy").Trim();
                            sBnsMun = result1.GetString("bns_mun").Trim();
                        }
                        else
                        {
                            result1.Close();
                            result1.Query = "select * from spl_business_que where bin = '" + sBin.Trim() + "'";
                            if (result1.Execute())
                            {
                                if (result1.Read())
                                {
                                    sBnsHouseNo = result1.GetString("bns_house_no").Trim();
                                    sBnsStreet = result1.GetString("bns_street").Trim();
                                    sBnsBrgy = result1.GetString("bns_brgy").Trim();
                                    sBnsMun = result1.GetString("bns_mun").Trim();
                                }
                                else
                                {
                                    result1.Close();
                                    result1.Query = "select * from spl_businesses where bin = '" + sBin.Trim() + "'";
                                    if (result1.Execute())
                                    {
                                        if (result1.Read())
                                        {
                                            sBnsHouseNo = result1.GetString("bns_house_no").Trim();
                                            sBnsStreet = result1.GetString("bns_street").Trim();
                                            sBnsBrgy = result1.GetString("bns_brgy").Trim();
                                            sBnsMun = result1.GetString("bns_mun").Trim();
                                        }
                                        else
                                        {
                                            // RMC 20151005 corrections in printing Notice of Unofficial business (s)
                                            result1.Close();
                                            result1.Query = "select * from unofficial_info_tbl where is_number = '" + sBin.Trim() + "'";
                                            if (result1.Execute())
                                            {
                                                if (result1.Read())
                                                {
                                                    sBnsHouseNo = result1.GetString("bns_house_no").Trim();
                                                    sBnsStreet = result1.GetString("bns_street").Trim();
                                                    sBnsBrgy = result1.GetString("bns_brgy").Trim();
                                                    sBnsMun = result1.GetString("bns_mun").Trim();
                                                }
                                            }
                                            // RMC 20151005 corrections in printing Notice of Unofficial business (e)
                                        }
                                    }
                                    
                                }
                            }
                        }
                    }
                    result1.Close();
                }
            }
            result.Close();

            /*if(sBnsHouseNo.Trim() == ".")
                //sAddress = sBnsStreet + ", " + sBnsBrgy + ", " + sBnsMun;
                sAddress = sBnsStreet + ", " + sBnsBrgy;
            else
//                sAddress = sBnsHouseNo + " " + sBnsStreet + ", " + sBnsBrgy + ", " + sBnsMun;
                sAddress = sBnsHouseNo + " " + sBnsStreet + ", " + sBnsBrgy;
             */
            // RMC 20110414 put rem

            // RMC 20110414 (s)
            if (sBnsHouseNo == "." || sBnsHouseNo == "")
                sBnsHouseNo = "";
            else
                sBnsHouseNo = sBnsHouseNo + " ";
            if (sBnsStreet == "." || sBnsStreet == "")
                sBnsStreet = "";
            else
                sBnsStreet = sBnsStreet + ", ";
            if (sBnsBrgy == "." || sBnsBrgy == "")
                sBnsBrgy = "";
            else
                //sBnsBrgy = sBnsBrgy + ", ";
                sBnsBrgy = "BARANGAY " + sBnsBrgy + ", ";   // RMC 20170106 print "Barangay" in business address
            if (sBnsMun == "." || sBnsMun == "")
                sBnsMun = "";

            sAddress = sBnsHouseNo + sBnsStreet + sBnsBrgy + sBnsMun;
            // RMC 20110414 (e)

            return sAddress;
        }

        public static string GetLandPin(string sBin)
        {
            OracleResultSet result = new OracleResultSet();
            string sResult = string.Empty;
            result.Query = "select * from btm_gis_loc where bin = '" + sBin.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                    sResult = result.GetString("land_pin").Trim();
            }
            result.Close();
            return sResult;
        }

        public static string GetNoticeDate(string sBin, string sNoticeNum)
        {
            OracleResultSet result = new OracleResultSet();
            string sResult = string.Empty;
            result.Query = "select * from unofficial_notice_closure where is_number = '" + sBin.Trim() + "' and notice_number = '" + sNoticeNum.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                    sResult = result.GetDateTime("notice_date").ToShortDateString();
                else
                {
                    result.Close();
                    result.Query = "select * from official_notice_closure where bin = '" + sBin.Trim() + "' and notice_number = '" + sNoticeNum.Trim() + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                            sResult = result.GetDateTime("notice_date").ToShortDateString();
                    }
                }
            }
            result.Close();
            return sResult;
        }

        public static string GetCDODate(string sBin)
        {
            OracleResultSet result = new OracleResultSet();
            string sResult = string.Empty;
            result.Query = "select * from norec_closure_tagging where is_number = '" + sBin.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                    sResult = result.GetDateTime("tdatetime").ToShortDateString();
                else
                {
                    result.Close();
                    result.Query = "select * from official_closure_tagging where bin = '" + sBin.Trim() + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                            sResult = result.GetDateTime("tdatetime").ToShortDateString();
                    }
                }
            }
            result.Close();
            return sResult;
        }

        public static double GetLastOrAmountMapped(string sBin)
        {
            OracleResultSet result = new OracleResultSet();
            double dResult = 0;
            string sOrNo = string.Empty;

            result.Query = "select sum(fees_amtdue) as Totamt from or_table where or_no in (select distinct(or_no) as or_no from pay_hist where bin = '" + sBin.Trim() + "' and data_mode <> 'UNP' and bin in (select bin from btm_temp_businesses where bin = '" + sBin + "'))";
            if (result.Execute())
            {
                if (result.Read())
                    dResult = result.GetDouble("Totamt");
            }
            result.Close();

            return dResult;
        }

        public static string GetLastOrNoMapped(string sBin)
        {
            OracleResultSet result = new OracleResultSet();
            double dResult = 0;
            string sOrNo = string.Empty;
            result.Query = "select distinct * from pay_hist where bin = '" + sBin.Trim() + "' and data_mode <> 'UNP'  order by tax_year desc, or_date desc, qtr_paid desc";
            if (result.Execute())
            {
                if (result.Read())
                    sOrNo = result.GetString("or_no").Trim();
            }
            result.Close();

            return sOrNo;
        }

        public static string GetCurrQtrPaid(string sBin)
        {
            OracleResultSet result = new OracleResultSet();
            double dResult = 0;
            string sOrNo = string.Empty;
            result.Query = "select distinct * from pay_hist where bin = '" + sBin.Trim() + "' and data_mode <> 'UNP' and (qtr_paid <> 'A' and qtr_paid <> 'P') order by tax_year desc, or_date desc, qtr_paid desc";
            if (result.Execute())
            {
                if (result.Read())
                    sOrNo = result.GetString("qtr_paid").Trim();
            }
            result.Close();

            return sOrNo;
        }

        public static string GetLastOrDateMapped(string sBin)
        {
            OracleResultSet result = new OracleResultSet();
            double dResult = 0;
            string sOrNo = string.Empty;
            DateTime dORDt = DateTime.Now;
            result.Query = "select distinct * from pay_hist where bin = '" + sBin.Trim() + "'  and data_mode <> 'UNP' order by tax_year desc, or_date desc, qtr_paid desc";
            if (result.Execute())
            {
                if (result.Read())
                    sOrNo = result.GetDateTime("or_date").ToShortDateString();
            }
            result.Close();

            return sOrNo;
        }

        public static string GetMappedBnsAddress(string sBin)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result1 = new OracleResultSet();
            string sBnsHouseNo = string.Empty;
            string sBnsStreet = string.Empty;
            string sBnsBrgy = string.Empty;
            string sBnsMun = string.Empty;
            string sAddress = string.Empty;

            result.Query = "select * from btm_businesses where bin = '" + sBin.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sBnsHouseNo = result.GetString("bns_house_no").Trim();
                    sBnsStreet = result.GetString("bns_street").Trim();
                    sBnsBrgy = result.GetString("bns_brgy").Trim();
                    sBnsMun = result.GetString("bns_mun").Trim();
                }
                else
                {
                    result1.Query = "select * from btm_temp_businesses where tbin = '" + sBin.Trim() + "'";
                    if (result1.Execute())
                    {
                        if (result1.Read())
                        {
                            sBnsHouseNo = result1.GetString("bns_house_no").Trim();
                            sBnsStreet = result1.GetString("bns_street").Trim();
                            sBnsBrgy = result1.GetString("bns_brgy").Trim();
                            sBnsMun = result1.GetString("bns_mun").Trim();
                        }
                    }
                    result1.Close();
                }
            }
            result.Close();

            /*if(sBnsHouseNo.Trim() == ".")
                //sAddress = sBnsStreet + ", " + sBnsBrgy + ", " + sBnsMun;
                sAddress = sBnsStreet + ", " + sBnsBrgy;
            else
//                sAddress = sBnsHouseNo + " " + sBnsStreet + ", " + sBnsBrgy + ", " + sBnsMun;
                sAddress = sBnsHouseNo + " " + sBnsStreet + ", " + sBnsBrgy;
             */
            // RMC 20110414 put rem

            // RMC 20110414 (s)
            if (sBnsHouseNo == "." || sBnsHouseNo == "")
                sBnsHouseNo = "";
            else
                sBnsHouseNo = sBnsHouseNo + " ";
            if (sBnsStreet == "." || sBnsStreet == "")
                sBnsStreet = "";
            else
                sBnsStreet = sBnsStreet + ", ";
            if (sBnsBrgy == "." || sBnsBrgy == "")
                sBnsBrgy = "";
            else
                sBnsBrgy = sBnsBrgy + ", ";
            if (sBnsMun == "." || sBnsMun == "")
                sBnsMun = "";

            sAddress = sBnsHouseNo + sBnsStreet + sBnsBrgy + sBnsMun;
            // RMC 20110414 (e)

            return sAddress;
        }

        public static string ValidUntil(string sBin, string sCurrentYear)
        {
            DateTime dtSystemDate;
            DateTime dDate;
            DateTime dCurrDate;
            string sMonth = string.Empty;
            string staxYear = string.Empty;
            //string sCurrentDate = string.Empty;
            string sQtrToPayDate = string.Empty;
            string sDate = string.Empty;
            //sCurrentDate = AppSettingsManager.GetSystemDate().ToShortDateString();

            string sCurrentDate = string.Empty;
            dtSystemDate = AppSettingsManager.GetSystemDate();
            sCurrentDate = string.Format("{0:MM/dd/yyyy}", dtSystemDate);
            OracleResultSet result = new OracleResultSet();
            DateTime dDueDate = new DateTime();
            result.Query = "select qtr_to_pay,tax_year from taxdues where bin = '" + sBin.Trim() + "'order by tax_year desc, qtr_to_pay desc";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sMonth = result.GetString("qtr_to_pay").Trim();
                    staxYear = result.GetString("tax_year").Trim();
                    /*if (sMonth == "1")
                        sQtrToPayDate = "1-JAN-" + staxYear;
                    if (sMonth == "2")
                        sQtrToPayDate = "1-FEB-" + staxYear;
                    if (sMonth == "3")
                        sQtrToPayDate = "1-MAR-" + staxYear;
                    if (sMonth == "4")
                        sQtrToPayDate = "1-APR-" + staxYear;
                    if (sMonth == "5")
                        sQtrToPayDate = "1-MAY-" + staxYear;
                    if (sMonth == "6")
                        sQtrToPayDate = "1-JUN-" + staxYear;
                    if (sMonth == "7")
                        sQtrToPayDate = "1-JUL-" + staxYear;
                    if (sMonth == "8")
                        sQtrToPayDate = "1-AUG-" + staxYear;
                    if (sMonth == "9")
                        sQtrToPayDate = "1-SEP-" + staxYear;
                    if (sMonth == "10")
                        sQtrToPayDate = "1-OCT-" + staxYear;
                    if (sMonth == "11")
                        sQtrToPayDate = "1-NOV-" + staxYear;
                    if (sMonth == "12")
                        sQtrToPayDate = "1-DEC-" + staxYear;*/  // RMC 20161228 corrected validity date in SOA, put rem

                    // RMC 20161228 corrected validity date in SOA (s)
                    if (sMonth == "1")
                        sQtrToPayDate = "1-JAN-" + staxYear;
                    if (sMonth == "2")
                        sQtrToPayDate = "1-APR-" + staxYear;
                    if (sMonth == "3")
                        sQtrToPayDate = "1-JUL-" + staxYear;
                    if (sMonth == "4")
                        sQtrToPayDate = "1-OCT-" + staxYear;
                    // RMC 20161228 corrected validity date in SOA (e)
                }
                else
                    dDueDate = DateTime.Parse(sCurrentDate);
            }
            result.Close();

            dCurrDate = DateTime.Parse(sCurrentDate);

            // AST 20160108 Fixed error prompting in wrong date format (s)
            DateTime dtTmp = new DateTime();
            //DateTime.TryParse(sQtrToPayDate, out dtTmp);  // RMC 20170131 correction in SOA validity date put rem
            // AST 20160108 Fixed error prompting in wrong date format (e)
            dtTmp = dCurrDate;  // RMC 20170131 correction in SOA validity date

            //result.Query = "select * from due_dates where due_year = '" + sCurrentYear + "' and due_date >= '" + sQtrToPayDate + "' order by due_code"; AST 20160108 Fixed error prompting in wrong date format
            //result.Query = "select * from due_dates where due_year = '" + sCurrentYear + "' and due_date >= to_date('" + dtTmp.ToShortDateString() + "', 'MM/dd/yyyy') order by due_code";

            // RMC 20170202 modified validity date in SOA (s)
            string sDueCode = string.Empty;
            sDueCode = string.Format("{0:0#}",dCurrDate.Month);
            result.Query = "select * from due_dates where due_year = '" + sCurrentYear + "' and due_code = '" + sDueCode + "'";
            // RMC 20170202 modified validity date in SOA (e)

            if (result.Execute())
            {
                DateTime dPrevDate;
                while (result.Read())
                {
                    dDueDate = result.GetDateTime("due_date");
                    sDate = dDueDate.ToShortDateString();
                    dDate = DateTime.Parse(sDate);

                    // RMC 20170202 corrected SOA validity date (s)
                    int iMonthDue = 0;
                    int.TryParse(AppSettingsManager.GetConfigValue("14"), out iMonthDue);
                    int iCurrMonth = dCurrDate.Month;
                    int iTmpDays = DateTime.DaysInMonth(dCurrDate.Year, dCurrDate.Month);
                    //JARS 20181107
                    //if (iCurrMonth == 1 || iCurrMonth == 4 || iCurrMonth == 7 || iCurrMonth == 10)
                    //    sDate = dDueDate.ToShortDateString();
                    //else
                    //    sDate = dCurrDate.Month.ToString() + "/" + iTmpDays.ToString() + "/" + dCurrDate.Year.ToString();

                    if (dDueDate.Day == 1)
                    {
                        //get sDate;
                        break;
                    }
                    else
                    {
                        //JARS 20181107 CORRECTION: if past due date, will gate the due date of the next month, MALOLOS CLIENT
                        #region comments
                        //if (iCurrMonth == 1 || iCurrMonth == 4 || iCurrMonth == 7 || iCurrMonth == 10)
                        //{
                        //    if (dCurrDate <= dDueDate)
                        //    {
                        //        sDate = string.Format("{0:MM/dd/yyyy}", dDueDate);
                        //        break;
                        //    }
                        //    else
                        //    {
                        //        //get sDate;
                        //        break;
                        //    }
                        //}
                        #endregion
                        //if (dCurrDate >= dDueDate)
                        if (dCurrDate > dDueDate) // JAA 20190620 modified validity date as per malolos requested
                        {
                            iCurrMonth = dCurrDate.Month + 1;
                            if (iCurrMonth > 12)
                            {
                                iCurrMonth = 1;
                                int iDueYear = dCurrDate.Year + 1;
                            }
                            sDate = iCurrMonth.ToString() + "/" + dDueDate.Day.ToString() + "/" + dCurrDate.Year.ToString();
                            //sDate = string.Format("{0:MM/dd/yyyy}", dDueDate);
                            break;
                        }
                        else
                        {
                            //get sDate;
                            break;
                        }

                    }
                    // RMC 20170202 corrected SOA validity date (e)

                    /*
                    // RMC 20170131 correction in SOA validity date (s)
                    int iMonthDue = 0;
                    int.TryParse(AppSettingsManager.GetConfigValue("14"), out iMonthDue);
                    int iCurrMonth = dCurrDate.Month;

                    if (dCurrDate >= dDueDate && iMonthDue == 1)
                    {
                        int iTmpDays = DateTime.DaysInMonth(dCurrDate.Year, dCurrDate.Month);
                        sDate = dCurrDate.Month.ToString() + "/" + iTmpDays.ToString() + "/" + dCurrDate.Year.ToString();
                        break;
                    }
                    // RMC 20170131 correction in SOA validity date (e) 

                    if (dCurrDate <= dDueDate)
                        break;
                    */  // RMC 20170202 modified validity date in SOA, put rem
                }
            }
            result.Close();

            return sDate;
        }

        public static string MonthsInWords(DateTime dDate)
        {
            string sMonth = string.Empty;
            string sDay = string.Empty;
            string sYear = string.Empty;
            string sDate = string.Empty;
            sDate = string.Format("{0:MM/dd/yyyy}", dDate);
            sMonth = sDate.Substring(0, 2);
            sDay = sDate.Substring(3, 2);
            sYear = sDate.Substring(6, 4);

            if (sMonth == "01")
                sMonth = "January";
            if (sMonth == "02")
                sMonth = "February";
            if (sMonth == "03")
                sMonth = "March";
            if (sMonth == "04")
                sMonth = "April";
            if (sMonth == "05")
                sMonth = "May";
            if (sMonth == "06")
                sMonth = "June";
            if (sMonth == "07")
                sMonth = "July";
            if (sMonth == "08")
                sMonth = "August";
            if (sMonth == "09")
                sMonth = "September";
            if (sMonth == "10")
                sMonth = "October";
            if (sMonth == "11")
                sMonth = "November";
            if (sMonth == "12")
                sMonth = "December";

            sDate = sMonth + " " + sDay + ", " + sYear;
            return sDate;
        }

        // GDE 20101109

        public static bool bIsTagged(string sBin)
        {
            bool bWatch = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from official_closure_tagging where bin = '" + sBin.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                    bWatch = true;
            }
            result.Close();

            result.Query = "select * from norec_closure_tagging where is_number = '" + sBin.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                    bWatch = true;
            }
            result.Close();
            return bWatch;
        }

        public static string GetBnsName(string sBin)
        {
            string sBnsName = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select bns_nm from businesses where bin = '" + sBin.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sBnsName = result.GetString("bns_nm").Trim();
                }
                else
                {
                    result.Close();
                    result.Query = "select bns_nm from business_que where bin = '" + sBin.Trim() + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sBnsName = result.GetString("bns_nm").Trim();
                        }
                        else
                        {
                            result.Close();
                            result.Query = "select bns_nm from btm_businesses where bin = '" + sBin.Trim() + "'";
                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    sBnsName = result.GetString("bns_nm").Trim();
                                }
                                else
                                {
                                    result.Close();
                                    result.Query = "select bns_nm from btm_temp_businesses where tbin = '" + sBin.Trim() + "'";
                                    if (result.Execute())
                                    {
                                        if (result.Read())
                                        {
                                            sBnsName = result.GetString("bns_nm").Trim();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            result.Close();
            return StringUtilities.StringUtilities.RemoveApostrophe(sBnsName);
        }

        public static string GetOwnCode(string sBin)
        {
            string sOwnCode = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select own_code from businesses where bin = '" + sBin.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sOwnCode = result.GetString("own_code").Trim();
                }
                else
                {
                    result.Close();
                    result.Query = "select * from business_que where bin = '" + sBin.Trim() + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sOwnCode = result.GetString("own_code").Trim();
                        }
                        else
                        {
                            result.Close();
                            result.Query = "select * from btm_temp_businesses where tbin = '" + sBin.Trim() + "'";
                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    sOwnCode = result.GetString("own_code").Trim();
                                }
                                else
                                {
                                    result.Close();
                                    result.Query = "select * from btm_businesses where bin = '" + sBin.Trim() + "'";
                                    if (result.Execute())
                                    {
                                        if (result.Read())
                                        {
                                            sOwnCode = result.GetString("own_code").Trim();
                                        }
                                        else
                                        {
                                            result.Close();
                                            result.Query = "select * from spl_businesses where bin = '" + sBin.Trim() + "'";
                                            if (result.Execute())
                                            {
                                                if (result.Read())
                                                {
                                                    sOwnCode = result.GetString("own_code").Trim();
                                                }
                                            }
                                            else
                                            {
                                                result.Close();
                                                result.Query = "select * from spl_business_que where bin = '" + sBin.Trim() + "'";
                                                if (result.Execute())
                                                {
                                                    if (result.Read())
                                                    {
                                                        sOwnCode = result.GetString("own_code").Trim();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            result.Close();

            return sOwnCode;
        }

        public static string GetBnsOwnCode(string sBin)
        {
            string sOwnCode = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select own_code from businesses where bin = '" + sBin.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sOwnCode = result.GetString("own_code").Trim();    // RMC 20110414 changed "busn_code" to "own_code"
                }
                else
                {
                    result.Close();
                    result.Query = "select * from business_que where bin = '" + sBin.Trim() + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sOwnCode = result.GetString("own_code").Trim();    // RMC 20110414 changed "busn_code" to "own_code"
                        }
                    }
                }

            }
            result.Close();

            return sOwnCode;
        }

        public static string GetBnsOwnAdd(string sOwnCode)
        {
            OracleResultSet result = new OracleResultSet();

            string sBnsOwnerAdd = string.Empty;
            string sOwnHouseNo = string.Empty;
            string sOwnStreet = string.Empty;
            string sOwnBrgy = string.Empty;
            string sOwnMun = string.Empty;

            // RMC 20110831 modified GetBnsOwnAdd for owner's query  (s)
            string sProv = string.Empty;
            string sZone = string.Empty;
            string sDistrict = string.Empty;
            // RMC 20110831 modified GetBnsOwnAdd for owner's query  (e)

            result.Query = "select * from own_names where own_code = '" + sOwnCode.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sOwnHouseNo = result.GetString("own_house_no").Trim();
                    sOwnStreet = result.GetString("own_street").Trim();
                    sOwnBrgy = result.GetString("own_brgy").Trim();
                    sOwnMun = result.GetString("own_mun").Trim();

                    // RMC 20110831 modified GetBnsOwnAdd for owner's query  (s)
                    sProv = result.GetString("own_prov").Trim();
                    sZone = result.GetString("own_zone").Trim();
                    sDistrict = result.GetString("own_dist").Trim();

                    if (sProv == "." || sProv == "")
                        sProv = "";
                    else
                        sProv = ", " + sProv;
                    if (sZone == "." || sZone == "ZONE" || sZone == "")
                        sZone = "";
                    else
                        sZone = "ZONE " + sZone + " ";
                    if (sDistrict == "." || sDistrict == "")
                        sDistrict = "";
                    else
                        sDistrict = sDistrict + ", ";
                    // RMC 20110831 modified GetBnsOwnAdd for owner's query  (e)

                    // RMC 20110414 (s)
                    if (sOwnHouseNo == "." || sOwnHouseNo == "")
                        sOwnHouseNo = "";
                    if (sOwnStreet == "." || sOwnStreet == "")
                        sOwnStreet = "";
                    else
                        sOwnStreet = sOwnStreet + ", ";
                    if (sOwnBrgy == "." || sOwnBrgy == "")
                        sOwnBrgy = "";
                    else
                        sOwnBrgy = sOwnBrgy + ", ";
                    if (sOwnMun == "." || sOwnMun == "")
                        sOwnMun = "";

                    // RMC 20110831 modified GetBnsOwnAdd for owner's query  (s)
                    if (m_bOwnerQuery)
                        sBnsOwnerAdd = sOwnHouseNo + ", " + sOwnStreet + ", " + sOwnBrgy + ", " + sZone + ", " + sDistrict + ", " + sOwnMun + ", " + sProv;
                    else   // RMC 20110831 modified GetBnsOwnAdd for owner's query (e)
                        //sBnsOwnerAdd = sOwnHouseNo + sOwnStreet + sOwnBrgy + sOwnMun;
                        sBnsOwnerAdd = sOwnHouseNo + " " + sOwnStreet + " " + sOwnBrgy + " " + sOwnMun; // RMC 20111128 display owner's address in SearchOwner
                    // RMC 20110414 (e)

                    //sBnsOwnerAdd = sOwnHouseNo + " " + sOwnStreet + ", " + sOwnBrgy + ", " + sOwnMun; // RMC 20110414
                }
            }
            result.Close();

            return sBnsOwnerAdd;
        }

        public static string GetBnsOwnAdd(string sOwnCode, bool blnIsPermit)
        {

            OracleResultSet result = new OracleResultSet();

            string sBnsOwnerAdd = string.Empty;
            string sOwnHouseNo = string.Empty;
            string sOwnStreet = string.Empty;
            string sOwnBrgy = string.Empty;
            string sOwnMun = string.Empty;

            // RMC 20110831 modified GetBnsOwnAdd for owner's query  (s)
            string sProv = string.Empty;
            string sZone = string.Empty;
            string sDistrict = string.Empty;
            // RMC 20110831 modified GetBnsOwnAdd for owner's query  (e)

            result.Query = "select * from own_names where own_code = '" + sOwnCode.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sOwnHouseNo = result.GetString("own_house_no").Trim();
                    sOwnStreet = result.GetString("own_street").Trim();
                    sOwnBrgy = result.GetString("own_brgy").Trim();
                    sOwnMun = result.GetString("own_mun").Trim();

                    // RMC 20110831 modified GetBnsOwnAdd for owner's query  (s)
                    sProv = result.GetString("own_prov").Trim();
                    sZone = result.GetString("own_zone").Trim();
                    sDistrict = result.GetString("own_dist").Trim();

                    if (sProv == "." || sProv == "")
                        sProv = "";
                    else
                        sProv = ", " + sProv;
                    if (sZone == "." || sZone == "ZONE" || sZone == "")
                        sZone = "";
                    else
                        sZone = "ZONE " + sZone + " ";
                    if (sDistrict == "." || sDistrict == "")
                        sDistrict = "";
                    else
                        sDistrict = sDistrict + ", ";
                    // RMC 20110831 modified GetBnsOwnAdd for owner's query  (e)



                    // RMC 20110414 (s)
                    if (sOwnHouseNo == "." || sOwnHouseNo == "")
                        sOwnHouseNo = "";
                    if (sOwnStreet == "." || sOwnStreet == "")
                        sOwnStreet = "";
                    else
                        sOwnStreet = sOwnStreet + ", ";
                    if (sOwnBrgy == "." || sOwnBrgy == "")
                        sOwnBrgy = "";
                    else
                        sOwnBrgy = sOwnBrgy + ", ";
                    if (sOwnMun == "." || sOwnMun == "")
                        sOwnMun = "";

                    // RMC 20110831 modified GetBnsOwnAdd for owner's query  (s)
                    if (m_bOwnerQuery)
                        sBnsOwnerAdd = sOwnHouseNo + ", " + sOwnStreet + ", " + sOwnBrgy + ", " + sZone + ", " + sDistrict + ", " + sOwnMun + ", " + sProv;
                    else   // RMC 20110831 modified GetBnsOwnAdd for owner's query (e)
                        //sBnsOwnerAdd = sOwnHouseNo + sOwnStreet + sOwnBrgy + sOwnMun;
                        sBnsOwnerAdd = sOwnHouseNo + " " + sOwnStreet + " " + sOwnBrgy + " " + sOwnMun; // RMC 20111128 display owner's address in SearchOwner
                    // RMC 20110414 (e)

                    if (blnIsPermit && sOwnMun == "ANGONO")
                        sBnsOwnerAdd = sOwnHouseNo + " " + sOwnStreet + " " + sOwnBrgy;

                    //sBnsOwnerAdd = sOwnHouseNo + " " + sOwnStreet + ", " + sOwnBrgy + ", " + sOwnMun; // RMC 20110414
                }
            }
            result.Close();

            return sBnsOwnerAdd;
        }

        public static string GetBnsCodeByDesc(string sDesc)
        {
            OracleResultSet result = new OracleResultSet();

            string strRevYear = AppSettingsManager.GetConfigValue("07");    // RMC 20110308

            result.Query = "select bns_code from bns_table where bns_desc = :1 and fees_code = 'B' and rev_year = :2";
            result.AddParameter(":1", sDesc.Trim());
            result.AddParameter(":2", strRevYear);  // RMC 20110308

            if (result.Execute())
            {
                if (result.Read())
                    sDesc = result.GetString("bns_code").Trim();
                else
                    sDesc = ""; // RMC 20110308
            }
            result.Close();
            return sDesc;
        }

        public static string GetBnsCodeMain(string sBin)
        {
            OracleResultSet result = new OracleResultSet();
            string sBnsCode = string.Empty;
            result.Query = "select bns_code from businesses where bin = :1";
            result.AddParameter(":1", sBin.Trim());
            if (result.Execute())
            {
                if (result.Read())
                {
                    sBnsCode = result.GetString("bns_code").Trim();
                }
                else
                {
                    result.Close();
                    result.Query = "select bns_code from business_que where bin = :1";
                    result.AddParameter(":1", sBin.Trim());
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sBnsCode = result.GetString("bns_code").Trim();
                        }
                    }
                    else
                    {
                        result.Close();
                        result.Query = "select bns_code from buss_hist where bin = :1";
                        result.AddParameter(":1", sBin.Trim());
                        if (result.Execute())
                        {
                            if (result.Read())
                            {
                                sBnsCode = result.GetString("bns_code").Trim();
                            }
                        }

                    }
                }

            }
            result.Close();
            return sBnsCode;
        }

        public static string GetNigViolist(string sBin)
        {
            string sValue = "";
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = @"select NT.DIVISION_CODE,NT.VIOLATION_DESC from nigvio_list NL 
inner join nigvio_tbl NT on NT.DIVISION_CODE = NL.DIVISION_CODE and NT.VIOLATION_CODE = NL.VIOLATION_CODE
where bin = '" + sBin + "'";
            if (pSet.Execute())
                while (pSet.Read())
                    sValue += "(" + pSet.GetString(0).Trim() + ") " +StringUtilities.StringUtilities.RemoveApostrophe(pSet.GetString(1).Trim()) + "\n";
            pSet.Close();

            if (sValue != "")
                sValue = sValue.Remove(sValue.Length - 1, 1);

            return sValue;
        }

        public static string GetBnsAdd(string sBin, string sType) //MCR 20150107 MOD added Type
        {
            OracleResultSet result = new OracleResultSet();

            string sBnsAdd = string.Empty;
            string sBnsHouseNo = string.Empty;
            string sBnsStreet = string.Empty;
            string sBnsBrgy = string.Empty;
            string sBnsMun = string.Empty;

            result.Query = "select * from businesses where bin = '" + sBin.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sBnsHouseNo = result.GetString("bns_house_no").Trim();
                    sBnsStreet = result.GetString("bns_street").Trim();
                    sBnsBrgy = result.GetString("bns_brgy").Trim();
                    sBnsMun = result.GetString("bns_mun").Trim();

                    if (sType == "") //MCR 20150107
                        sBnsAdd = sBnsHouseNo + " " + sBnsStreet + ", " + sBnsBrgy + ", " + sBnsMun;
                    else
                        sBnsAdd = sBnsHouseNo + " " + sBnsStreet + ", " + sBnsBrgy;
                }
                else
                {
                    result.Close();
                    result.Query = "select * from business_que where bin = '" + sBin.Trim() + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sBnsHouseNo = result.GetString("bns_house_no").Trim();
                            sBnsStreet = result.GetString("bns_street").Trim();
                            sBnsBrgy = result.GetString("bns_brgy").Trim();
                            sBnsMun = result.GetString("bns_mun").Trim();

                            if (sType == "") //MCR 20150107
                                sBnsAdd = sBnsHouseNo + " " + sBnsStreet + ", " + sBnsBrgy + ", " + sBnsMun;
                            else
                                sBnsAdd = sBnsHouseNo + " " + sBnsStreet + ", " + sBnsBrgy;
                        }
                        else
                        {
                            result.Close();
                            result.Query = "select * from btm_businesses where bin = '" + sBin.Trim() + "'";
                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    sBnsHouseNo = result.GetString("bns_house_no").Trim();
                                    sBnsStreet = result.GetString("bns_street").Trim();
                                    sBnsBrgy = result.GetString("bns_brgy").Trim();
                                    sBnsMun = result.GetString("bns_mun").Trim();

                                    if (sType == "") //MCR 20150107
                                        sBnsAdd = sBnsHouseNo + " " + sBnsStreet + ", " + sBnsBrgy + ", " + sBnsMun;
                                    else
                                        sBnsAdd = sBnsHouseNo + " " + sBnsStreet + ", " + sBnsBrgy;
                                }
                                else
                                {
                                    result.Close();
                                    result.Query = "select * from btm_temp_businesses where tbin = '" + sBin.Trim() + "'";
                                    if (result.Execute())
                                    {
                                        if (result.Read())
                                        {
                                            sBnsHouseNo = result.GetString("bns_house_no").Trim();
                                            sBnsStreet = result.GetString("bns_street").Trim();
                                            sBnsBrgy = result.GetString("bns_brgy").Trim();
                                            sBnsMun = result.GetString("bns_mun").Trim();

                                            if (sType == "") //MCR 20150107
                                                sBnsAdd = sBnsHouseNo + " " + sBnsStreet + ", " + sBnsBrgy + ", " + sBnsMun;
                                            else
                                                sBnsAdd = sBnsHouseNo + " " + sBnsStreet + ", " + sBnsBrgy;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            result.Close();

            return StringUtilities.StringUtilities.RemoveApostrophe(sBnsAdd);
        }

        public static string GetBnsBrgy(string sBin)
        {
            OracleResultSet result = new OracleResultSet();

            string sBnsAdd = string.Empty;
            string sBnsHouseNo = string.Empty;
            string sBnsStreet = string.Empty;
            string sBnsBrgy = string.Empty;
            string sBnsMun = string.Empty;

            result.Query = "select * from businesses where bin = '" + sBin.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                    sBnsAdd = result.GetString("bns_brgy").Trim();
                else
                {
                    result.Close();
                    result.Query = "select * from business_que where bin = '" + sBin.Trim() + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                            sBnsAdd = result.GetString("bns_brgy").Trim();
                        else
                        {
                            result.Close();
                            result.Query = "select * from btm_businesses where bin = '" + sBin.Trim() + "'";
                            if (result.Execute())
                            {
                                if (result.Read())
                                    sBnsAdd = result.GetString("bns_brgy").Trim();
                                else
                                {
                                    result.Close();
                                    result.Query = "select * from btm_temp_businesses where tbin = '" + sBin.Trim() + "'";
                                    if (result.Execute())
                                        if (result.Read())
                                            sBnsAdd = result.GetString("bns_brgy").Trim();
                                }
                            }
                        }
                    }
                }
            }
            result.Close();

            return sBnsAdd;
        }

        public static string GetCapitalGross(string sBin, string sBnsCode, string sTaxYear, string sType)
        {
            OracleResultSet result = new OracleResultSet();
            string sData = "0";//0.00
            

            if (sType == "PRE")
            {
                result.Query = "select * from declared_gross where bin = :1 and bns_code = :2 and tax_year = :3";
                result.AddParameter(":1", sBin.Trim());
                result.AddParameter(":2", sBnsCode.Trim());
                result.AddParameter(":3", sTaxYear.Trim());
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        sData = string.Format("{0:##0.00}", result.GetDouble("presumptive_gr"));
                    }
                }
                result.Close();
            }

            if (sType == "NEW")
            {
                result.Query = "select * from business_que where bin = :1 and bns_code = :2 and tax_year = :3 and bns_stat = 'NEW'";
                result.AddParameter(":1", sBin.Trim());
                result.AddParameter(":2", sBnsCode.Trim());
                result.AddParameter(":3", sTaxYear.Trim());
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        sData = string.Format("{0:##0.00}", result.GetDouble("capital"));
                    }
                    else
                    {
                        result.Close();
                        result.Query = "select * from businesses where bin = :1 and bns_code = :2 and tax_year = :3 and bns_stat = 'NEW'";
                        result.AddParameter(":1", sBin.Trim());
                        result.AddParameter(":2", sBnsCode.Trim());
                        result.AddParameter(":3", sTaxYear.Trim());
                        if (result.Execute())
                        {
                            if (result.Read())
                            {
                                sData = string.Format("{0:##0.00}", result.GetDouble("capital"));
                            }
                            else
                            {
                                result.Close();
                                result.Query = "select * from buss_hist where bin = :1 and bns_code = :2 and tax_year = :3 and bns_stat = 'NEW'";
                                result.AddParameter(":1", sBin.Trim());
                                result.AddParameter(":2", sBnsCode.Trim());
                                result.AddParameter(":3", sTaxYear.Trim());
                                if (result.Execute())
                                {
                                    if (result.Read())
                                    {
                                        sData = string.Format("{0:##0.00}", result.GetDouble("capital"));
                                    }
                                    else
                                    {
                                        result.Close();
                                        result.Query = "select * from addl_bns where bin = :1 and bns_code_main = :2 and tax_year = :3 and bns_stat = 'NEW'";
                                        result.AddParameter(":1", sBin.Trim());
                                        result.AddParameter(":2", sBnsCode.Trim());
                                        result.AddParameter(":3", sTaxYear.Trim());
                                        if (result.Execute())
                                        {
                                            if (result.Read())
                                            {
                                                sData = string.Format("{0:##0.00}", result.GetDouble("capital"));
                                            }
                                            else
                                            {
                                                // RMC 20180103 corrected data for gross/cap in application form (s)
                                                result.Close();
                                                result.Query = "select * from bill_gross_info where bin = :1 and bns_code = :2 and tax_year = :3 and bns_stat = 'NEW'";
                                                result.AddParameter(":1", sBin.Trim());
                                                result.AddParameter(":2", sBnsCode.Trim());
                                                result.AddParameter(":3", sTaxYear.Trim());
                                                if (result.Execute())
                                                {
                                                    if (result.Read())
                                                    {
                                                        sData = string.Format("{0:##0.00}", result.GetDouble("capital"));
                                                    }

                                                }
                                                // RMC 20180103 corrected data for gross/cap in application form (e)

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                result.Close();
            }
            if (sType == "REN")
            {
                result.Query = "select * from business_que where bin = :1 and bns_code = :2 and tax_year = :3 and bns_stat = 'REN'";
                result.AddParameter(":1", sBin.Trim());
                result.AddParameter(":2", sBnsCode.Trim());
                result.AddParameter(":3", sTaxYear.Trim());
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        sData = string.Format("{0:##0.00}", result.GetDouble("gr_1"));
                    }
                    else
                    {
                        result.Close();
                        //result.Query = "select * from businesses where bin = :1 and bns_code = :2 and tax_year = :3 and bns_stat = 'REN'";
                        result.Query = "select * from bill_gross_info where bin = :1 and bns_code = :2 and tax_year = :3 and bns_stat = 'REN'"; //JARS 20181022
                        result.AddParameter(":1", sBin.Trim());
                        result.AddParameter(":2", sBnsCode.Trim());
                        result.AddParameter(":3", sTaxYear.Trim());
                        if (result.Execute())
                        {
                            if (result.Read())
                            {
                                //sData = string.Format("{0:##0.00}", result.GetDouble("gr_1"));
                                sData = string.Format("{0:##0.00}", result.GetDouble("gross"));
                            }
                            else
                            {
                                result.Close();
                                result.Query = "select * from buss_hist where bin = :1 and bns_code = :2 and tax_year = :3 and bns_stat = 'REN'";
                                result.AddParameter(":1", sBin.Trim());
                                result.AddParameter(":2", sBnsCode.Trim());
                                result.AddParameter(":3", sTaxYear.Trim());
                                if (result.Execute())
                                {
                                    if (result.Read())
                                    {
                                        sData = string.Format("{0:##0.00}", result.GetDouble("gr_1"));
                                    }
                                    else
                                    {
                                        result.Close();
                                        result.Query = "select * from addl_bns where bin = :1 and bns_code_main = :2 and tax_year = :3 and bns_stat = 'REN'";
                                        result.AddParameter(":1", sBin.Trim());
                                        result.AddParameter(":2", sBnsCode.Trim());
                                        result.AddParameter(":3", sTaxYear.Trim());
                                        if (result.Execute())
                                        {
                                            if (result.Read())
                                            {
                                                sData = string.Format("{0:##0.00}", result.GetDouble("gross"));
                                            }
                                            else //JARS 20170815
                                            {
                                                result.Close();
                                                result.Query = "select * from declared_gross where bin = :1 and bns_code = :2 and tax_year = :3";
                                                result.AddParameter(":1", sBin.Trim());
                                                result.AddParameter(":2", sBnsCode.Trim());
                                                result.AddParameter(":3", sTaxYear.Trim());
                                                if (result.Execute())
                                                {
                                                    if (result.Read())
                                                    {
                                                        sData = string.Format("{0:##0.00}", result.GetDouble("declared_gr"));
                                                    }
                                                    else
                                                    {
                                                        // RMC 20180103 corrected data for gross/cap in application form (s)
                                                        result.Close();
                                                        result.Query = "select * from bill_gross_info where bin = :1 and bns_code = :2 and tax_year = :3 and bns_stat = 'REN'";
                                                        result.AddParameter(":1", sBin.Trim());
                                                        result.AddParameter(":2", sBnsCode.Trim());
                                                        result.AddParameter(":3", sTaxYear.Trim());
                                                        if (result.Execute())
                                                        {
                                                            if (result.Read())
                                                            {
                                                                sData = string.Format("{0:##0.00}", result.GetDouble("gross"));
                                                            }

                                                        }// RMC 20180103 corrected data for gross/cap in application form (e)
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                result.Close();
            }

            if (sType == "RET")
            {
                result.Query = "select * from retired_bns where bin = :1 and bns_code_main = :2 and tax_year = :3";
                result.AddParameter(":1", sBin.Trim());
                result.AddParameter(":2", sBnsCode.Trim());
                result.AddParameter(":3", sTaxYear.Trim());
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        sData = string.Format("{0:##0.00}", result.GetDouble("gross"));
                    }
                    else
                    {
                        result.Close();
                        result.Query = "select * from business_que where bin = :1 and bns_code = :2 and tax_year = :3 and bns_stat = 'REN'";
                        result.AddParameter(":1", sBin.Trim());
                        result.AddParameter(":2", sBnsCode.Trim());
                        result.AddParameter(":3", sTaxYear.Trim());
                        if (result.Execute())
                        {
                            if (result.Read())
                            {
                                sData = string.Format("{0:##0.00}", result.GetDouble("gr_1"));
                            }
                            else
                            {
                                result.Close();
                                result.Query = "select * from businesses where bin = :1 and bns_code = :2 and tax_year = :3 and bns_stat = 'REN'";
                                result.AddParameter(":1", sBin.Trim());
                                result.AddParameter(":2", sBnsCode.Trim());
                                result.AddParameter(":3", sTaxYear.Trim());
                                if (result.Execute())
                                {
                                    if (result.Read())
                                    {
                                        sData = string.Format("{0:##0.00}", result.GetDouble("gr_1"));
                                    }
                                    else
                                    {
                                        result.Close();
                                        result.Query = "select * from buss_hist where bin = :1 and bns_code = :2 and tax_year = :3 and bns_stat = 'REN'";
                                        result.AddParameter(":1", sBin.Trim());
                                        result.AddParameter(":2", sBnsCode.Trim());
                                        result.AddParameter(":3", sTaxYear.Trim());
                                        if (result.Execute())
                                        {
                                            if (result.Read())
                                            {
                                                sData = string.Format("{0:##0.00}", result.GetDouble("gr_1"));
                                            }
                                            else
                                            {
                                                result.Close();
                                                result.Query = "select * from addl_bns where bin = :1 and bns_code_main = :2 and tax_year = :3 and bns_stat = 'REN'";
                                                result.AddParameter(":1", sBin.Trim());
                                                result.AddParameter(":2", sBnsCode.Trim());
                                                result.AddParameter(":3", sTaxYear.Trim());
                                                if (result.Execute())
                                                {
                                                    if (result.Read())
                                                    {
                                                        sData = string.Format("{0:##0.00}", result.GetDouble("gross"));
                                                    }
                                                    else
                                                    {
                                                        // RMC 20180103 corrected data for gross/cap in application form (s)
                                                        result.Close();
                                                        result.Query = "select * from bill_gross_info where bin = :1 and bns_code = :2 and tax_year = :3 and (bns_stat = 'RET' or bns_stat = 'REN')";
                                                        result.AddParameter(":1", sBin.Trim());
                                                        result.AddParameter(":2", sBnsCode.Trim());
                                                        result.AddParameter(":3", sTaxYear.Trim());
                                                        if (result.Execute())
                                                        {
                                                            if (result.Read())
                                                            {
                                                                sData = string.Format("{0:##0.00}", result.GetDouble("gross"));
                                                            }

                                                        }// RMC 20180103 corrected data for gross/cap in application form (e)
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        result.Close();
                    }
                }
            }
            if(sType == "ADJ") //JARS 20181022
            {
                result.Query = "select * from bill_gross_info where bin = :1 and bns_code = :2 and tax_year = :3 and (bns_stat = 'RET' or bns_stat = 'REN')";
                result.AddParameter(":1", sBin.Trim());
                result.AddParameter(":2", sBnsCode.Trim());
                result.AddParameter(":3", sTaxYear.Trim());
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        sData = string.Format("{0:##0.00}", result.GetDouble("adj_gross"));
                    }

                }
                result.Close();
            }
            return sData;
        }

        public static double GetCompoundedGross(string sBin)
        {
            double dGross = 0;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select gr_1 from businesses where bin = :1";
            result.AddParameter(":1", sBin);
            if (result.Execute())
            {
                if (result.Read())
                {
                    try
                    {
                        dGross = result.GetDouble("gr_1");
                    }
                    catch
                    {
                        dGross = 0;
                    }
                }
                else
                {
                    result.Close();
                    result.Query = "select gr_1 from business_que where bin = :1";
                    result.AddParameter(":1", sBin);
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            try
                            {
                                dGross = result.GetDouble("gr_1");
                            }
                            catch
                            {
                                dGross = 0;
                            }
                        }
                    }

                }
            }
            result.Close();

            result.Query = "select sum(gross) as sum_gross from addl_bns where bin = :1";
            result.AddParameter(":1", sBin);
            if (result.Execute())
            {
                if (result.Read())
                {
                    try
                    {
                        dGross += result.GetDouble("sum_gross");
                    }
                    catch
                    {
                        dGross += 0;
                    }
                }
            }
            result.Close();
            return dGross;
        }
        public static double GetCompoundedGross(string sBin, string sTaxYear)
        {
            double dGross = 0;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select gr_1 from businesses where bin = :1 and tax_year = :2";
            result.AddParameter(":1", sBin);
            result.AddParameter(":2", sTaxYear);
            if (result.Execute())
            {
                if (result.Read())
                {
                    try
                    {
                        dGross = result.GetDouble("gr_1");
                    }
                    catch
                    {
                        dGross = 0;
                    }
                }
                else
                {
                    result.Close();
                    result.Query = "select gr_1 from business_que where bin = :1 and tax_year = :2";
                    result.AddParameter(":1", sBin);
                    result.AddParameter(":2", sTaxYear);
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            try
                            {
                                dGross = result.GetDouble("gr_1");
                            }
                            catch
                            {
                                dGross = 0;
                            }
                        }
                        else
                        {
                            result.Close();
                            result.Query = "select gr_1 from buss_hist where bin = :1 and tax_year = :2";
                            result.AddParameter(":1", sBin);
                            result.AddParameter(":2", sTaxYear);
                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    try
                                    {
                                        dGross = result.GetDouble("gr_1");
                                    }
                                    catch
                                    {
                                        dGross = 0;
                                    }
                                }
                            }
                        }
                    }


                }

            }
            result.Close();

            result.Query = "select sum(gross) as sum_gross from addl_bns where bin = :1 and tax_year = :2";
            result.AddParameter(":1", sBin);
            result.AddParameter(":2", sTaxYear);
            if (result.Execute())
            {
                if (result.Read())
                {
                    try
                    {
                        dGross += result.GetDouble("sum_gross");
                    }
                    catch
                    {
                        dGross += 0;
                    }
                }
            }
            result.Close();
            return dGross;
        }

        public static bool WithAddlBns(string sBin)
        {
            bool bFlag = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from addl_bns where bin = '" + sBin.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                    bFlag = true;
                else
                    bFlag = false;
            }
            result.Close();
            return bFlag;
        }

        public static bool WithDateReceived(string sBin, string sNoticeNo)
        {
            bool bFlag = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select NOTICE_SENT from unofficial_notice_closure where IS_NUMBER = '" + sBin.Trim() + "' and notice_number = '" + sNoticeNo + "' and notice_sent is not null";
            if (result.Execute())
            {
                if (result.Read())
                    bFlag = true;
                else
                    bFlag = false;
            }
            result.Close();
            return bFlag;
        }

        public static string GetConfigValue(string strConfigCode)
        {
            return AppSettingsManager.GetConfigValue("OBJECT", "config", "code",
                "trim(:1)", strConfigCode);
        }

        public static string GetConfigValueRemarks(string strConfigCode)
        {
            return AppSettingsManager.GetConfigValue("REMARKS", "config", "code",
                "trim(:1)", strConfigCode);
        }

        public static string GetConfigValueByDescription(string strConfigDescription)
        {
            return AppSettingsManager.GetConfigValue("object", "config", "remarks",
                "trim(:1)", strConfigDescription);
        }

        public static string GetAuctionConfigValue(string strAuctionCode)
        {
            return AppSettingsManager.GetConfigValue("value_fld", "auction_config", "subj_code",
                "RPAD(:1, 2)", strAuctionCode);
        }

        public static string GetConfigObject(string sCode)
        {

            OracleResultSet xxx = new OracleResultSet();
            xxx.Query = "SELECT OBJECT FROM CONFIG WHERE TRIM(CODE) = :1";
            xxx.AddParameter(":1", sCode);
            if (xxx.Execute())
            {
                if (xxx.Read() == true)
                {
                    sObject = xxx.GetString("object").Trim();
                }
            }
            xxx.Close();
            return sObject;

        }

        public static string GetInspector(string sCode)
        {
            string sLn = string.Empty;
            string sFn = string.Empty;
            string sMi = string.Empty;
            string sName = string.Empty;
            OracleResultSet xxx = new OracleResultSet();
            xxx.Query = "SELECT * FROM inspector WHERE TRIM(INSPECTOR_CODE) = :1";
            xxx.AddParameter(":1", sCode);
            if (xxx.Execute())
            {
                if (xxx.Read() == true)
                {
                    sLn = xxx.GetString("inspector_ln").Trim();
                    sFn = xxx.GetString("inspector_fn").Trim();
                    sMi = xxx.GetString("inspector_mi").Trim();
                }
            }
            xxx.Close();

            if (sMi.Trim() != string.Empty)
                sName = sFn + " " + sMi + ". " + sLn;
            else
                sName = sFn + " " + sLn;

            return sName;
        }

        public static string GetConfigValue(string strValueField, string strConfigTable,
            string strSubjectField, string strFormatField, string strConfigCode)
        {
            string strConfigValue = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = string.Format("SELECT {0} FROM {1} WHERE {2} = {3}", strValueField,
                strConfigTable, strSubjectField, strFormatField);
            //"SELECT value_fld FROM config_table WHERE subj_code = RPAD(:1, 3)";
            result.AddParameter(":1", strConfigCode);
            if (result.Execute())
            {
                if (result.Read())
                {
                    strConfigValue = result.GetString(0).Trim();
                }
            }
            result.Close();
            return strConfigValue;
        }

        public static bool HasRights(string strUserCode, string strModuleCode)
        {
            bool blnHasRights = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = "SELECT COUNT(*) FROM auth WHERE trim(usr_code) = :1 AND mod_code = RPAD(:2, 10)";
            result.AddParameter(":1", strUserCode);
            result.AddParameter(":2", strModuleCode);
            int intCount = 0;
            int.TryParse(result.ExecuteScalar(), out intCount);
            if (intCount != 0)
                blnHasRights = true;
            result.Close();
            return blnHasRights;
        }

        public static string GetWorkstationName()
        {
            return Environment.MachineName.Trim();
        }

        //RDO 04302008 (s) get municipal code
        public static string GetMunicipalCode(string strMunicipalName)
        {
            string strMunicipalCode = string.Empty;
            string strVersion = string.Empty;
            strVersion = AppSettingsManager.GetConfigValue("60");
            OracleResultSet result = new OracleResultSet();
            if (strVersion == "PM")
            {
                result.Query = "SELECT dist_code FROM districts WHERE dist_nm = RPAD(:1, 20)";
                result.AddParameter(":1", strMunicipalName.Trim());
                strMunicipalCode = result.ExecuteScalar();
            }
            else if ((strVersion == "M" || strVersion == "C" || strVersion == "CC") && AppSettingsManager.GetConfigValue("10") == "Y")
            {
                result.Query = "SELECT dist_code FROM districts";
                strMunicipalCode = result.ExecuteScalar();
            }
            result.Close();

            return strMunicipalCode;
        }
        //RDO 04302008 (e) get municipal code


        //for compatibility with older version
        public static string GetDistrictCode()
        {
            string strDistrictCode = string.Empty;
            string strPath = string.Format("{0}/config_wan.ini", Environment.GetFolderPath(Environment.SpecialFolder.System));
            if (File.Exists(strPath))
            {
                //open and read contents
                FileInfo fileInf = new FileInfo(strPath);
                FileStream data = fileInf.OpenRead();

                Encoding enc = Encoding.UTF8;
                byte[] byteBuffer = new byte[fileInf.Length];
                data.Read(byteBuffer, 0, byteBuffer.Length);
                string strValue = enc.GetString(byteBuffer);
                data.Close();

                string strTray = string.Empty;

                strValue = strValue.Replace('\r', ' ');

                string[] strLines = strValue.Split('\n');
                for (int i = 0; i < strLines.Length; i++)
                {
                    string[] strColumns = strLines[i].Split('=');
                    strTray = strColumns[0].Trim().ToUpper();
                    if (strTray.Length >= 4 && strTray.Substring(0, 4) == "TRAY" && strColumns.Length > 1)
                    {
                        string[] strFields = strColumns[1].Split(',');
                        //need add codes if one district only
                        strDistrictCode = strFields[0];

                        break;
                    }
                }
            }

            return strDistrictCode;
        }

        public static bool GenerateInfo(string m_strRptName)
        {
            OracleResultSet result = new OracleResultSet();

            string strReportDesc, strReportDate, strReportUserCode, strCurrentUser, strMsg;
            bool bSw = false;
            int intReportSwitch = 0;

            result.Query = string.Format("select * from gen_info where report_name = '{0}' order by report_date desc", m_strRptName);
            if (result.Execute())
            {
                if (result.Read())
                {
                    strReportDesc = result.GetString("report_name");
                    //strReportDate = result.GetString("report_date");
                    strReportDate = string.Format("{0:MM/dd/yyyy}", result.GetDateTime("report_date")); // RMC 20110803 
                    strReportUserCode = result.GetString("user_code").Trim();
                    intReportSwitch = result.GetInt("switch");

                    //strMsg = strReportDesc + "\nLast date of generation of : " + strReportDate + "\nby " + strReportUserCode + ".  Do you want continue? ";
                    strMsg = strReportDesc + "\nLast date of generation: " + strReportDate + "\nby " + strReportUserCode + ".  Do you want continue? "; // RMC 20110803 

                    if (MessageBox.Show(strMsg, "BPLS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (intReportSwitch == 1)
                        {
                            if (SystemUser.UserCode == strReportUserCode)
                            {
                                strMsg = SystemUser.UserName + " is still generating the " + strReportDesc;
                                MessageBox.Show(strMsg, "BPLS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                bSw = false;
                            }
                            else
                            {
                                bSw = true;
                            }
                        }
                        else
                        {
                            bSw = true;
                        }
                    }
                    else
                    {
                        bSw = false;
                    }
                }
                else
                    bSw = true;
            }
            result.Close();

            return bSw;
        }

        // RMC 20110309
        public static string GetFeesCodeByDesc(string sDesc, string sFeesType)
        {
            OracleResultSet result = new OracleResultSet();
            string strRevYear = AppSettingsManager.GetConfigValue("07");

            result.Query = "select fees_code from tax_and_fees_table where fees_desc = :1 and rev_year = :2 and fees_type = :3";
            result.AddParameter(":1", sDesc.Trim());
            result.AddParameter(":2", strRevYear);
            result.AddParameter(":3", sFeesType);
            if (result.Execute())
            {
                if (result.Read())
                    sDesc = result.GetString("fees_code").Trim();
                else
                    sDesc = "";
            }
            result.Close();
            return sDesc;
        }

        public static string GetSubBnsDesc(string sBnsCode, string sFeesCode)
        {
            OracleResultSet result = new OracleResultSet();

            string strRevYear = AppSettingsManager.GetConfigValue("07");    // RMC 20110308

            result.Query = "select * from bns_table where bns_code = '" + sBnsCode.Trim() + "' and fees_code = '" + sFeesCode.Trim() + "' and rev_year = '" + strRevYear + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sBnsCode = result.GetString("bns_desc").Trim();
                }
            }
            result.Close();
            return sBnsCode;
        }

        // GDE 20110723
        public static bool MultiBns(string sBin)
        {
            bool bResult = false;
            OracleResultSet result = new OracleResultSet();
            int iBinCount = 0;
            result.Query = "select count(*) as iCount from businesses where own_code in (select own_code from businesses where bin = '" + sBin + "')";
            if (result.Execute())
            {
                if (result.Read())
                {
                    iBinCount = result.GetInt("iCount");
                }
            }
            result.Close();

            if (iBinCount > 1)
                bResult = true;
            else
                bResult = false;
            return bResult;
        }

        public static bool CheckBns(string sBin)
        {
            bool bResult = true;
            OracleResultSet result = new OracleResultSet();


            OracleResultSet pSet = new OracleResultSet();

            bool blnIsSplBns = false;
            pSet.Query = "select * from spl_business_que where bin  = :1";
            pSet.AddParameter(":1", sBin);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    blnIsSplBns = true;
                }
                else
                {
                    pSet.Close();
                    pSet.Query = "select * from spl_businesses where bin  = :1";
                    pSet.AddParameter(":1", sBin);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        { blnIsSplBns = true; }
                    }
                }
            }
            pSet.Close();

            if (blnIsSplBns)
                result.Query = "select * from spl_business_que where bin = '" + sBin + "'";
            else
                result.Query = "select * from business_que where bin = '" + sBin + "'";
            if (result.Execute())
            {
                if (result.Read())
                    bResult = true;
                else
                {
                    result.Close();
                    result.Query = "select bin from businesses where bin = '" + sBin + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                            bResult = true;
                        else
                            bResult = false;
                    }

                }
            }
            result.Close();
            return bResult;
        }
        public static bool TreasurerModule(string sBin)
        {
            bool bResult = false;
            OracleResultSet result = new OracleResultSet();
            //result.Query = "select * from treasurers_module where bin = '" + sBin + "' and (action = '0' or action = '1') and tax_year <= '" + AppSettingsManager.GetSystemDate().Year + "'";
            result.Query = "select * from treasurers_module where bin = '" + sBin + "' and (action = '0' or action = '1') and tax_year <= '" + AppSettingsManager.GetCurrentDate().Year.ToString() + "'";  // RMC 20110725
            if (result.Execute())
            {
                if (result.Read())
                    bResult = true;
                else
                    bResult = false;
            }
            result.Close();
            return bResult;
        }

        /// <summary>
        /// ALJ 20110907 re-assessment validation
        /// *get the action status in treasurers_module
        /// </summary>
        /// <param name="p_sBin"></param>
        /// <param name="p_sTaxYear"></param>
        /// <returns></returns>
        public static string TreasurerModule(string p_sBin, string p_sTaxYear)
        {
            OracleResultSet result = new OracleResultSet();
            string sAction;
            sAction = string.Empty;

            // RMC 20120112 additional validation in Billing if record already approved in Treasurers module (s)
            result.Query = "select * from treasurers_module_tmp where bin = '" + p_sBin + "' and tax_year = '" + p_sTaxYear + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sAction = "2";
                    result.Close();
                }// RMC 20120112 additional validation in Billing if record already approved in Treasurers module (e)
                else
                {
                    result.Close();

                    result.Query = "select * from treasurers_module where bin = '" + p_sBin + "' and tax_year = '" + p_sTaxYear + "' order by action";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sAction = result.GetString("action");
                        }
                    }
                    result.Close();
                }
            }
            return sAction;

        }

        public static bool BounceCheck(string sBin)
        {
            bool bResult = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from bounce_chk_rec where bin = '" + sBin + "'";
            if (result.Execute())
            {
                if (result.Read())
                    bResult = true;
                else
                    bResult = false;
            }
            result.Close();

            return bResult;
        }

        public static bool IsReadyforSOA(string sBin)
        {
            bool bResult = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select bin from ass_taxdues where bin = '" + sBin + "'";
            if (result.Execute())
            {
                if (result.Read())
                    bResult = true;
                else
                {
                    result.Close();
                    result.Query = "select bin from taxdues where bin = '" + sBin + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                            bResult = true;
                    }
                }
            }
            result.Close();

            return bResult;
        }

        public static bool TagForClosure(string sBin)
        {
            bool bResult = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select distinct bin from closure_tagging where bin = '" + sBin + "'";
            if (result.Execute())
            {
                if (result.Read())
                    bResult = true;
                else
                    bResult = false;
            }
            result.Close();

            return bResult;
        }

        public static string GetFeesDesc(string sFeesCode)
        {
            OracleResultSet result = new OracleResultSet();
            //result.Query = "select * from tax_and_fees_table where fees_code = '" + sFeesCode.Trim() + "'";
            result.Query = "select * from tax_and_fees_table where fees_code = '" + sFeesCode.Trim() + "' and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "'"; //MCR 20141118

            if (result.Execute())
            {
                if (result.Read())
                {
                    sFeesCode = result.GetString("fees_desc").Trim();
                }
            }
            result.Close();
            return sFeesCode;
        }

        // RMC 20110414 (s)
        private static void SaveNewOwner(string strOwnCode, string strOwnLn, string strOwnFn, string strOwnMI, string strOwnHouseNo, string strOwnSt, string strOwnDist, string strOwnZone, string strOwnBrgy, string strOwnMun, string strOwnProv, string strOwnZip)
        {
            OracleResultSet result = new OracleResultSet();

            /*result.Query = "insert into own_names values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12)";
            result.AddParameter(":1", strOwnCode);  //OWN_CODE
            result.AddParameter(":2", StringUtilities.StringUtilities.HandleApostrophe(strOwnLn.Trim()));   //OWN_LN
            result.AddParameter(":3", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnFn.Trim())));    //OWN_FN
            result.AddParameter(":4", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnMI.Trim())));    //OWN_MI
            result.AddParameter(":5", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnHouseNo.Trim())));    //OWN_HOUSE_NO
            result.AddParameter(":6", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnSt.Trim())));    //OWN_STREET
            result.AddParameter(":7", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnDist.Trim())));    //OWN_DIST
            result.AddParameter(":8", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnZone.Trim())));    //OWN_ZONE
            result.AddParameter(":9", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnBrgy.Trim())));    //OWN_BRGY
            result.AddParameter(":10", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnMun.Trim())));   //OWN_MUN
            result.AddParameter(":11", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnProv.Trim())));   //OWN_PROV
            result.AddParameter(":12", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnZip.Trim())));   //OWN_ZIP
             */

            // RMC 20111014 modified saving of own names, correcting names with apostrophe (note: dont use AddParameter if going to insert strings with apostrophe) (s)
            result.Query = "insert into own_names values (";
            result.Query += "'" + strOwnCode + "', ";
            result.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(strOwnLn.Trim()) + "', ";
            result.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnFn.Trim())) + "', ";
            result.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnMI.Trim())) + "', ";
            result.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnHouseNo.Trim())) + "', ";
            result.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnSt.Trim())) + "', ";
            result.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnDist.Trim())) + "', ";
            result.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnZone.Trim())) + "', ";
            result.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnBrgy.Trim())) + "', ";
            result.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnMun.Trim())) + "', ";
            result.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnProv.Trim())) + "', ";
            result.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(strOwnZip.Trim())) + "')";
            // RMC 20111014 modified saving of own names, correcting names with apostrophe (note: dont use AddParameter if going to insert strings with apostrophe) (e)

            if (result.ExecuteNonQuery() != 0)
            {
                MessageBox.Show("Owner Saved", "BPLS", MessageBoxButtons.OK, MessageBoxIcon.Information); // RMC 20110725 added button & icon

            }
            result.Close();
        }
        // RMC 20110414 (e)

        // RMC 20110414
        public static string GetUserName(string strUsrCode)
        {
            OracleResultSet recUser = new OracleResultSet();


            string strUsrPos = string.Empty;
            string strUsrDiv = string.Empty;
            string strUserLN = string.Empty;
            string strUserFN = string.Empty;
            string strUserMI = string.Empty;
            string strUserName = string.Empty;

            recUser.Query = "select * from sys_users";
            recUser.Query += " where usr_code = '" + strUsrCode + "'";
            if (recUser.Execute())
            {
                if (recUser.Read())
                {
                    strUsrPos = recUser.GetString("usr_pos").Trim();
                    strUsrDiv = recUser.GetString("usr_div").Trim();
                    strUserLN = recUser.GetString("usr_ln").Trim();
                    strUserFN = recUser.GetString("usr_fn").Trim();
                    strUserMI = recUser.GetString("usr_mi").Trim();

                    strUserName = PersonName.ToPersonName(strUserLN, strUserFN, strUserMI, "L", "F L", "F M. L");
                }
            }
            recUser.Close();

            return strUserName;

        }

        public static string GetUserDiv(string strUsrCode) //MCR 20191121
        {
            OracleResultSet recUserDiv = new OracleResultSet();
            string strUsrDiv = string.Empty;

            recUserDiv.Query = "select * from sys_users";
            recUserDiv.Query += " where usr_code = '" + strUsrCode + "'";
            if (recUserDiv.Execute())
                if (recUserDiv.Read())
                    strUsrDiv = recUserDiv.GetString("usr_div").Trim();
            recUserDiv.Close();

            return strUsrDiv;
        }

        /// <summary>
        /// ALJ 20110907 re-assessment validation
        /// Check if Billing is ready for SOA view/print
        /// </summary>
        /// <param name="p_sBIN"></param>
        /// <param name="p_sTaxYear"></param>
        /// <returns></returns>
        public static bool IsForSoaPrint(string p_sBIN, string p_sTaxYear)
        {
            bool bResult = false;

            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from ass_taxdues where bin = '" + p_sBIN + "' and tax_year = '" + p_sTaxYear + "'";
            if (result.Execute())
            {
                if (result.Read())
                    bResult = true;
                //(s) GDE 20130116 consider posted payments with taxdues
                else
                {
                    result.Close();
                    result.Query = "select * from taxdues where bin = '" + p_sBIN + "' and tax_year = '" + p_sTaxYear + "' and qtr_to_pay >= '1'";  // AST 20160108 added >
                    result.Query += " and (due_state = 'N' or due_state = 'R' or due_state = 'X')";    // RMC 20150921 corrections in billing if user has no access in re-bill  // AST 20160108 considered retirement
                    if (result.Execute())
                    {
                        if (result.Read())
                            bResult = true;
                    }
                }
                //(e) GDE 20130116 consider posted payments with taxdues
            }
            result.Close();
            return bResult;
        }

        private static void UpdateSerials(string sTaxYear)
        {
            // RMC 20111129 modified initialization of serial
            OracleResultSet result = new OracleResultSet();
            int iCnt = 0;

            result.Query = "select count(*) from buss_series where tax_year = '" + sTaxYear + "'";
            int.TryParse(result.ExecuteScalar(), out iCnt);

            if (iCnt == 0)
            {
                result.Query = "insert into buss_series values (";
                result.Query += "'0','" + sTaxYear + "')";
                if (result.ExecuteNonQuery() == 0)
                { }
            }

            result.Query = "select count(*) from bill_series where tax_year = '" + sTaxYear + "'";
            int.TryParse(result.ExecuteScalar(), out iCnt);

            if (iCnt == 0)
            {
                result.Query = "insert into bill_series values (";
                result.Query += "'0','" + sTaxYear + "')";
                if (result.ExecuteNonQuery() == 0)
                { }
            }

            result.Query = "select count(*) from mp_series where tax_year = '" + sTaxYear + "'";
            int.TryParse(result.ExecuteScalar(), out iCnt);

            if (iCnt == 0)
            {
                result.Query = "insert into mp_series values (";
                result.Query += "'0','" + sTaxYear + "')";
                if (result.ExecuteNonQuery() == 0)
                { }
            }

            result.Query = string.Format("update config set object = '{0}' where code = '12'", sTaxYear);
            if (result.ExecuteNonQuery() == 0)
            { }
        }

        public static string GetBrgyCode(string sBrgyName)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sBrgyCode = "";

            pRec.Query = string.Format("select * from brgy where brgy_nm = '{0}'", sBrgyName);
            if (pRec.Execute())
            {
                if (pRec.Read())
                    sBrgyCode = pRec.GetString("brgy_code");
            }
            pRec.Close();

            return sBrgyCode;
        }

        public static string GetBrgyName(string sBrgyCode)
        {
            // RMC 20120330 Modified validation of Business mapping tagging in GIS table
            OracleResultSet pRec = new OracleResultSet();
            string sBrgyName = "";

            pRec.Query = string.Format("select * from brgy where brgy_code = '{0}'", sBrgyCode);
            if (pRec.Execute())
            {
                if (pRec.Read())
                    sBrgyName = pRec.GetString("brgy_nm");
            }
            pRec.Close();

            return sBrgyName;
        }

        // RMC 20111220 transferred validation of business if business-mapped in AppSettingsManager (s)
        public static bool ValidateMapping(string strBIN)
        {
            OracleResultSet pTmp = new OracleResultSet();

            pTmp.Query = "select * from btm_update where bin = '" + strBIN + "' and trim(def_settled) is null";
            if (pTmp.Execute())
            {
                if (pTmp.Read())
                {
                    pTmp.Close();
                    return true;
                }
                else
                {
                    pTmp.Close();
                    return false;
                }
            }

            return true;
        }
        // RMC 20111220 transferred validation of business if business-mapped in AppSettingsManager (e)

        public static bool ValidateGrossMonitoring(string sBIN, string sTaxYear)
        {
            // RMC 20111227 added Gross monitoring module for gross >= 200000
            OracleResultSet pGross = new OracleResultSet();

            pGross.Query = "select * from gross_monitoring where bin = '" + sBIN + "'";
            pGross.Query += " and action = '0' and tax_year = '" + sTaxYear + "'";
            if (pGross.Execute())
            {
                if (pGross.Read())
                {
                    pGross.Close();

                    MessageBox.Show("Gross is subject for approval.\n Printing of SOA not allowed.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else
                {
                    pGross.Close();
                    return true;
                }
            }
            pGross.Close();

            return true;
        }

        /// <summary>
        /// ALJ 20111227 save to for SOA Printing
        /// a record should be exist in ass_taxdues and ass_bill_gross_info in able to print SOA
        /// </summary>
        /// <param name="p_sBIN"></param>
        /// <param name="p_sBaseTaxYear"></param>
        /// <returns></returns>
        public static bool SaveForSoa(string p_sBIN, string p_sBaseTaxYear, string p_sUserCode)
        {
            OracleResultSet pCmd = new OracleResultSet();

            bool bWithTaxdues = false;
            pCmd.Query = "select * from taxdues where bin = :1";
            pCmd.AddParameter(":1", p_sBIN);
            if (pCmd.Execute())
            {
                if (pCmd.Read())
                    bWithTaxdues = true;
            }
            pCmd.Close();

            pCmd.Query = "DELETE FROM ass_taxdues WHERE bin = :1 and tax_year >= :2";
            pCmd.AddParameter(":1", p_sBIN);
            pCmd.AddParameter(":2", p_sBaseTaxYear);
            pCmd.ExecuteNonQuery();
            pCmd.Query = "DELETE FROM ass_bill_gross_info WHERE bin = :1 and tax_year >= :2";
            pCmd.AddParameter(":1", p_sBIN);
            pCmd.AddParameter(":2", p_sBaseTaxYear);
            pCmd.ExecuteNonQuery();

            // Note: Saving should be in this sequence: save to ass_taxdues first then ass_bill_gross_info 

            pCmd.Query = "INSERT INTO ass_taxdues SELECT * FROM taxdues a WHERE bin = :1 AND tax_year >= :2";
            pCmd.AddParameter(":1", p_sBIN);
            pCmd.AddParameter(":2", p_sBaseTaxYear);

            // check and prompt user if zero (0) row/s affected
            if (pCmd.ExecuteNonQuery() == 0)
            {
                return false; // billing not yet created
            }


            pCmd.Query = "INSERT INTO ass_bill_gross_info SELECT a.*, '" + p_sUserCode + "' as usr_code FROM bill_gross_info a WHERE bin = :1 AND tax_year >= :2";
            pCmd.AddParameter(":1", p_sBIN);
            pCmd.AddParameter(":2", p_sBaseTaxYear);
            pCmd.ExecuteNonQuery();

            return true;

        }

        public static void GenerationLog(string sReportName, string sReportUser, string sMode)
        {
            // RMC 20120301 merged gen log
            OracleResultSet pCmd = new OracleResultSet();

            if (sMode == "S")
            {
                pCmd.Query = string.Format("delete from gen_log where report_name = '{0}' and report_user = '{1}'", sReportName, sReportUser);
                if (pCmd.ExecuteNonQuery() == 0)
                { }

                pCmd.Query = string.Format("insert into gen_log values('{0}','{1}',sysdate,'')", sReportName, sReportUser);
                if (pCmd.ExecuteNonQuery() == 0)
                { }

            }
            else if (sMode == "E")
            {
                pCmd.Query = string.Format("update gen_log set end_date = sysdate where report_name ='{0}' and report_user = '{1}'", sReportName, sReportUser);
                if (pCmd.ExecuteNonQuery() == 0)
                { }
            }

        }

        public static string GetOrAmount(string sOrNo, string sBnsCode, string sFeesCode, string sType)
        {
            String sQuery = String.Empty, sData = String.Empty;

            sFeesCode = sFeesCode + "%";
            OracleResultSet pSet = new OracleResultSet();

            if (sType == "D")
                sQuery = string.Format("select coalesce(sum(fees_due),0) as fees_due from or_table where or_no = '{0}' and bns_code_main = '{1}' and fees_code like '{2}'", sOrNo, sBnsCode, sFeesCode);
            else if (sType == "TFD")
                sQuery = string.Format("select coalesce(sum(fees_due),0) as fees_due from or_table where or_no = '{0}' and bns_code_main = '{1}' and fees_code <> 'B'", sOrNo, sBnsCode);
            else if (sType == "AD")
                sQuery = string.Format("select coalesce(sum(fees_amtdue),0) as fees_due from or_table where or_no = '{0}' and bns_code_main = '{1}' and fees_code like '{2}'", sOrNo, sBnsCode, sFeesCode);
            else if (sType == "TFAD")
                sQuery = string.Format("select coalesce(sum(fees_amtdue),0) as fees_due from or_table where or_no = '{0}' and bns_code_main = '{1}' and fees_code <> 'B'", sOrNo, sBnsCode);
            else if (sType == "S")
                sQuery = string.Format("select coalesce(sum(fees_surch),0) as fees_due from or_table where or_no = '{0}' and bns_code_main = '{1}' and fees_code like '{2}'", sOrNo, sBnsCode, sFeesCode);
            else if (sType == "TFS")
                sQuery = string.Format("select coalesce(sum(fees_surch),0) as fees_due from or_table where or_no = '{0}' and bns_code_main = '{1}' and fees_code <> 'B'", sOrNo, sBnsCode);
            else if (sType == "I")
                sQuery = string.Format("select coalesce(sum(fees_pen),0) as fees_due from or_table where or_no = '{0}' and bns_code_main = '{1}' and fees_code like '{2}'", sOrNo, sBnsCode, sFeesCode);
            else if (sType == "TFI")
                sQuery = string.Format("select coalesce(sum(fees_pen),0) as fees_due from or_table where or_no = '{0}' and bns_code_main = '{1}' and fees_code <> 'B'", sOrNo, sBnsCode);
            else if (sType == "P")
                sQuery = string.Format("select coalesce(sum(fees_surch),0)+coalesce(sum(fees_pen),0) as fees_due from or_table where or_no = '{0}' and bns_code_main = '{1}' and fees_code like '{2}'", sOrNo, sBnsCode, sFeesCode);
            else if (sType == "TFP")
                sQuery = string.Format("select coalesce(sum(fees_surch),0)+coalesce(sum(fees_pen),0) as fees_due from or_table where or_no = '{0}' and bns_code_main = '{1}' and fees_code <> 'B'", sOrNo, sBnsCode);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                    sData = string.Format("{0:##0.00}", pSet.GetDouble("fees_due"));
            pSet.Close();

            return sData;
        }

        public static string GetBankBranch(string sBankCode)
        {
            string sBankBranch = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from bank_table where bank_code = :1";
            result.AddParameter(":1", sBankCode.Trim());
            if (result.Execute())
            {
                if (result.Read())
                    sBankBranch = result.GetString("bank_nm").Trim();
            }
            result.Close();
            return sBankBranch;

        }

        public static string GetAbv8FeesDesc(string sFeesCode, int iCnt)
        {
            OracleResultSet result = new OracleResultSet();
            string sFCode = string.Empty;
            //result.Query = "select * from tax_and_fees_table where fees_code = '" + sFeesCode.Trim() + "'";
            result.Query = "select * from tax_and_fees_table where fees_code = '" + sFeesCode.Trim() + "' and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "'"; //MCR 20141118
            if (result.Execute())
            {
                if (result.Read())
                {
                    sFeesCode = result.GetString("fees_desc").Trim();
                    sFCode = result.GetString("fees_code").Trim();
                }
            }
            result.Close();


            //if (iCnt >= 4)
            //{
            //    if (sFCode == "01")
            //        sFeesCode = "MP";
            //    if (sFCode == "02")
            //        sFeesCode = "SPF";
            //    if (sFCode == "03")
            //        sFeesCode = "GBAGE";
            //    if (sFCode == "04")
            //        sFeesCode = "ABIF";
            //    if (sFCode == "05")
            //        sFeesCode = "VS";
            //    if (sFCode == "06")
            //        sFeesCode = "BP";
            //    if (sFCode == "07")
            //        sFeesCode = "HPF";
            //    if (sFCode == "08")
            //        sFeesCode = "WPF";
            //    if (sFCode == "09")
            //        sFeesCode = "ZF";
            //    if (sFCode == "10")
            //        sFeesCode = "GBAGE(F)";
            //    if (sFCode == "11")
            //        sFeesCode = "RFRNL";
            //    if (sFCode == "12")
            //        sFeesCode = "RDMTL";
            //    if (sFCode == "13")
            //        sFeesCode = "RDS";
            //    if (sFCode == "14")
            //        sFeesCode = "RFL";
            //    if (sFCode == "15")
            //        sFeesCode = "RT";
            //    if (sFCode == "16")
            //        sFeesCode = "DFL";
            //    if (sFCode == "17")
            //        sFeesCode = "DDL";
            //    if (sFCode == "18")
            //        sFeesCode = "DT";
            //    if (sFCode == "19")
            //        sFeesCode = "DMT";
            //}

            return sFeesCode;
        }

        public static string GetDTIName(string sBIN)
        {
            // RMC 20150107
            OracleResultSet result = new OracleResultSet();
            string sName = string.Empty;

            result.Query = "select * from dti_bns_name where bin = '" + sBIN + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sName = result.GetString("dti_bns_nm");
                }
            }
            result.Close();

            return sName;
        }

        public static string GetPrevBnsStat(string sBIN) // MCR 20150109
        {
            OracleResultSet result = new OracleResultSet();
            string sBnsStat = string.Empty;
            //result.Query = "select * from buss_hist where bin = '" + sBIN + "' and tax_year < '" + AppSettingsManager.GetSystemDate().Year.ToString() + "'";
            result.Query = "select * from buss_hist where bin = '" + sBIN + "' and tax_year <= '" + AppSettingsManager.GetSystemDate().Year.ToString() + "' order by tax_year desc";    // RMC 20150426 QA corrections
            if (result.Execute())
                if (result.Read())
                    sBnsStat = result.GetString("bns_stat");
            result.Close();

            if (sBnsStat.Trim() == "")
            {
                //result.Query = "select * from businesses where bin = '" + sBIN + "' and tax_year < '" + AppSettingsManager.GetSystemDate().Year.ToString() + "'";
                result.Query = "select * from businesses where bin = '" + sBIN + "' and tax_year <= '" + AppSettingsManager.GetSystemDate().Year.ToString() + "'";  // RMC 20150426 QA corrections
                if (result.Execute())
                    if (result.Read())
                        sBnsStat = result.GetString("bns_stat");
                result.Close();
            }

            return sBnsStat;
        }

        public static void GetPrevBnsStat(string sBIN, out string sBnsStat, out string sDateOperated) // MCR 20150109
        {
            sDateOperated = "";
            sBnsStat = "";
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from buss_hist where bin = '" + sBIN + "' and tax_year < '" + AppSettingsManager.GetSystemDate().Year.ToString() + "'";
            if (result.Execute())
                if (result.Read())
                {
                    sBnsStat = result.GetString("bns_stat");
                    sDateOperated = result.GetDateTime("dt_operated").ToString("MM/dd/yyyy");
                }
            result.Close();

            if (sBnsStat.Trim() == "")
            {
                result.Query = "select * from businesses where bin = '" + sBIN + "' and tax_year < '" + AppSettingsManager.GetSystemDate().Year.ToString() + "'";
                if (result.Execute())
                    if (result.Read())
                    {
                        sBnsStat = result.GetString("bns_stat");
                        sDateOperated = result.GetDateTime("dt_operated").ToString("MM/dd/yyyy");
                    }
                result.Close();
            }
        }

        public static string GetBnsStatonBnsQue(string sBIN) // MCR 20150112
        {
            OracleResultSet result = new OracleResultSet();
            string sBnsStat = string.Empty;
            result.Query = "select bns_stat from business_que where bin = '" + sBIN + "'";
            if (result.Execute())
                if (result.Read())
                    sBnsStat = result.GetString("bns_stat");
            result.Close();

            if (sBnsStat.Trim() == "")
            {
                result.Query = "select bns_stat from businesses where bin = '" + sBIN + "'";
                if (result.Execute())
                    if (result.Read())
                        sBnsStat = result.GetString("bns_stat");
                result.Close();
            }

            return sBnsStat;
        }

        public static string GetBnsTaxYear(string sBin) // MCR 20150113
        {
            string sBnsTaxYear = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select tax_year from businesses where bin = '" + sBin.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sBnsTaxYear = result.GetString("tax_year").Trim();
                }
                else
                {
                    //AFM 20191220
                    result.Close();
                    result.Query = "select tax_year from business_que where bin = '" + sBin.Trim() + "'";
                    if (result.Execute())
                        if (result.Read())
                            sBnsTaxYear = result.GetString("tax_year").Trim();
                    //AFM 20191220
                }
            }
            result.Close();
            return sBnsTaxYear;
        }

        public static string GetBnsOrgKind(string sBin) // MCR 20150114
        {
            OracleResultSet result = new OracleResultSet();
            string sBnsOrgKind = string.Empty;
            result.Query = "select orgn_kind from businesses where bin = '" + sBin + "'";
            if (result.Execute())
                if (result.Read())
                    sBnsOrgKind = result.GetString("orgn_kind");
            result.Close();

            if (sBnsOrgKind.Trim() == "")
            {
                result.Query = "select orgn_kind from business_que where bin = '" + sBin + "'";
                if (result.Execute())
                    if (result.Read())
                        sBnsOrgKind = result.GetString("orgn_kind");
                result.Close();
            }

            return sBnsOrgKind;
        }

        public static void GetLastPaymentInfo(string sBin, string sORNo, out string sTaxyear, out string sQtr, out string sBnsStat)
        {
            sTaxyear = "";
            sQtr = "";
            sBnsStat = "";
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = @"select max(tax_year) tax_year, min(qtr_paid)||'-'||max(qtr_paid) qtr, bns_stat From pay_hist
where bin = '" + sBin + "' and or_no = '" + sORNo + "' group by tax_year, bns_stat order by tax_year desc";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sTaxyear = pSet.GetString(0);
                    sQtr = pSet.GetString(1);
                    sBnsStat = pSet.GetString(2);
                }
            }
            pSet.Close();

            if (sQtr.Trim() != "")
            {
                if (sQtr == "F-F" || sQtr == "1-4")
                    sQtr = "F";
                else if (sQtr == "1-1")
                    sQtr = "1st Qtr";
                else if (sQtr == "2-2")
                    sQtr = "2nd Qtr";
                else if (sQtr == "3-3")
                    sQtr = "3rd Qtr";
                else if (sQtr == "4-4")
                    sQtr = "4th Qtr";
                else
                    sQtr += " Qtr";
            }
        }

        public static void CrossSaveRCDSeries(string sTellerCode, string sRCDSeries, string sRCDCode)
        {
            OracleResultSet pSetBpls = new OracleResultSet();
            OracleResultSet pSetArcs = new OracleResultSet();
            pSetArcs.CreateNewConnectionARCS();
            sTellerCode = StringUtilities.StringUtilities.HandleApostrophe(sTellerCode);
            sRCDSeries = "";

            pSetArcs.Query = @"select * from rcd_series where teller = '" + sTellerCode + "' and year = '" + GetSystemDate().Year + "'";
            if (pSetArcs.Execute())
            {
                if (pSetArcs.Read())
                {
                    sRCDSeries = pSetArcs.GetString("rcd_series");
                    pSetBpls.Query = @"select * from rcd_series where teller = '" + sTellerCode + "'";
                    if (pSetBpls.Execute())
                        if (!pSetBpls.Read())
                        {
                            pSetBpls.Query = "insert into rcd_series values ";
                            pSetBpls.Query += "('" + sRCDSeries + "', ";
                            pSetBpls.Query += "'" + sTellerCode + "', ";
                            pSetBpls.Query += "'" + sRCDCode + "')";
                            if (pSetBpls.ExecuteNonQuery() == 0)
                            { }
                        }
                    pSetBpls.Close();
                }
                else
                {
                    pSetBpls.Query = "insert into rcd_series values ";
                    pSetBpls.Query += "('0', ";
                    pSetBpls.Query += "'" + sTellerCode + "', ";
                    pSetBpls.Query += "'" + sRCDCode + "')";
                    if (pSetBpls.ExecuteNonQuery() == 0)
                    { }
                }
            }
            pSetArcs.Close();
        }

        public static void CrossUpdateRCDSeries(string sTellerCode, string sRCDSeries)
        {
            OracleResultSet pSetBpls = new OracleResultSet();
            OracleResultSet pSetArcs = new OracleResultSet();
            pSetArcs.CreateNewConnectionARCS();
            sTellerCode = StringUtilities.StringUtilities.HandleApostrophe(sTellerCode);
            int iRCDSeriesBPLS = 0;
            int.TryParse(sRCDSeries, out iRCDSeriesBPLS);

            string sRCDSeriesARCS = String.Empty;
            int iRCDSeriesARCS = 0;

            pSetArcs.Query = @"select * from rcd_series where teller = '" + sTellerCode + "' and year = '" + GetSystemDate().Year + "'";
            if (pSetArcs.Execute())
                if (pSetArcs.Read())
                    sRCDSeriesARCS = pSetArcs.GetString("rcd_series");
            pSetArcs.Close();

            int.TryParse(sRCDSeriesARCS, out iRCDSeriesARCS);

            if (iRCDSeriesARCS > iRCDSeriesBPLS)
                iRCDSeriesBPLS = iRCDSeriesARCS;
            iRCDSeriesBPLS++;

            pSetArcs.Query = "update rcd_series set rcd_series = " + iRCDSeriesBPLS + "";
            pSetArcs.Query += " where teller = '" + sTellerCode + "'";
            if (pSetArcs.ExecuteNonQuery() == 0)
            { }

            pSetArcs.Query = "update rcd_series set rcd_series = " + iRCDSeriesBPLS + "";
            pSetArcs.Query += " where teller = '" + sTellerCode + "'";
            if (pSetArcs.ExecuteNonQuery() == 0)
            { }
        }

        public static void GetPrevCapitalorGross(string sBIN, string sTaxYear, string sBnsCode, out int iAmount, out string sLabel)
        {
            string sBnsStat = "";
            sLabel = "";
            iAmount = 0;

            sBnsStat = GetPrevBnsStat(sBIN);
            if (sBnsStat == "NEW")
                sLabel = "Previous Capital";
            else
                sLabel = "Previous Gross";
            sTaxYear = Convert.ToDouble((Convert.ToInt32(sTaxYear) - 1)).ToString();

            OracleResultSet result = new OracleResultSet();
            result.Query = "select gr_1,capital,tax_year,bns_stat from buss_hist where bin = '" + sBIN + "' and tax_year = '" + sTaxYear + "' and bns_stat = '" + sBnsStat + "' and bns_code = '" + sBnsCode + "'";
            if (result.Execute())
                if (result.Read())
                {
                    //if (sBnsStat != "NEW")

                    if (result.GetInt("gr_1") != 0)
                        iAmount = result.GetInt("gr_1");
                    else
                        iAmount = result.GetInt("capital");
                }
                else
                {
                    result.Close();
                    result.Query = "select gr_1,capital,tax_year,bns_stat from businesses where bin = '" + sBIN + "' and tax_year = '" + sTaxYear + "' and bns_stat = '" + sBnsStat + "' and bns_code = '" + sBnsCode + "'";
                    if (result.Execute())
                        if (result.Read())
                        {
                            //if (sBnsStat != "NEW")
                            if (result.GetInt("gr_1") != 0)
                                iAmount = result.GetInt("gr_1");
                            else
                                iAmount = result.GetInt("capital");
                        }
                        else
                        {
                            result.Close();
                            result.Query = "select gross,capital,tax_year,bns_stat from bill_gross_info where bin = '" + sBIN + "' and tax_year = '" + sTaxYear + "' and bns_stat = '" + sBnsStat + "' and bns_code = '" + sBnsCode + "'";
                            if (result.Execute())
                                if (result.Read())
                                {
                                    //if (sBnsStat != "NEW")
                                    if (result.GetInt("gross") != 0)
                                        iAmount = result.GetInt("gross");
                                    else
                                        iAmount = result.GetInt("capital");
                                }
                                else
                                {
                                    result.Close();
                                    result.Query = "select gross,capital,tax_year,bns_stat from addl_bns where bin = '" + sBIN + "' and tax_year = '" + sTaxYear + "' and bns_stat = '" + sBnsStat + "' and bns_code_main = '" + sBnsCode + "'";
                                    if (result.Execute())
                                        if (result.Read())
                                        {
                                            //if (sBnsStat != "NEW")
                                            if (result.GetInt("gross") != 0)
                                                iAmount = result.GetInt("gross");
                                            else
                                                iAmount = result.GetInt("capital");
                                        }
                                }
                        }
                }
            result.Close();
        }

        public static string GetBnsPlate(string sBin) // MCR 20150129
        {
            OracleResultSet result = new OracleResultSet();
            string sBnsPlate = string.Empty;
            result.Query = "select * from buss_plate where bin = '" + sBin.Trim() + "'";
            if (result.Execute())
                if (result.Read())
                    sBnsPlate = result.GetString("bns_plate");
            result.Close();

            return sBnsPlate;
        }

        public static string GetAppNo(string sBin) // MCR 20150204
        {
            string sStat = string.Empty;
            string sTaxYear = string.Empty;
            string sBnsStat = string.Empty;

            sTaxYear = GetBnsTaxYear(sBin);
            sBnsStat = GetBnsStat(sBin);

            OracleResultSet result = new OracleResultSet();
            if (sBnsStat == "NEW")
                result.Query = string.Format("select * from app_permit_no_new where bin = '{0}' and year = '{1}'", sBin, sTaxYear);
            else
                result.Query = string.Format("select * from app_permit_no where bin = '{0}' and year = '{1}'", sBin, sTaxYear);
            if (result.Execute())
                if (result.Read())
                    sStat = result.GetString("app_no");
            result.Close();

            return sStat;
        }

        public static string GetAppNo(string sBin, string sTaxYear) // MCR 20150204
        {
            string sAppNo = string.Empty;
            string sBnsStat = string.Empty;

            sBnsStat = GetBnsStat(sBin);

            OracleResultSet result = new OracleResultSet();
            if (sBnsStat == "NEW")
                result.Query = string.Format("select * from app_permit_no_new where bin = '{0}' and year = '{1}'", sBin, sTaxYear);
            else
                result.Query = string.Format("select * from app_permit_no where bin = '{0}' and year = '{1}'", sBin, sTaxYear);
            if (result.Execute())
                if (result.Read())
                    sAppNo = result.GetString("app_no");
            result.Close();

            return sAppNo;
        }

        public static string GetBnsStat(string sBin) // MCR 20150204
        {
            string sStat = string.Empty;
            string sTaxYear = string.Empty;

            sTaxYear = GetBnsTaxYear(sBin);

            OracleResultSet result = new OracleResultSet();
            result.Query = string.Format("select * from businesses where bin = '{0}' and tax_year = '{1}'", sBin, sTaxYear);
            if (result.Execute())
                if (result.Read())
                    sStat = result.GetString("bns_stat");
            result.Close();

            return sStat;
        }

        public static string GetBlobImageConfig()
        {
            // RMC 20150226 adjustment in blob configuration
            string sConfig = string.Empty;
            string sTmp = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.CreateBlobConnection();

            if (m_sSystemType == "A")
            {
                sTmp = AppSettingsManager.GetConfigValue("62");
            }
            else
            {
                sTmp = AppSettingsManager.GetConfigValue("63");
            }

            result.Query = "select * from sourcedoc_tbl where srcdoc_desc = '" + sTmp + "' and system_code = '" + m_sSystemType + "'";
            if (result.Execute())
            {
                if (result.Read())
                    sConfig = result.GetString("srcdoc_code");
            }
            result.Close();

            return sConfig;
        }

        /// <summary>
        /// AST 20150114
        /// </summary>
        /// <param name="BIN"></param>
        /// <returns></returns>
        public static int GetLastBilledQtr(string BIN)
        {
            int intBilledQtr = 0;
            OracleResultSet result = new OracleResultSet();
            result.Query = @"select tax_year, 
                        case 
                        when qtr_paid = 'F' and (to_char(or_date, 'MM') between '01' and '03') then '1' 
                        when qtr_paid = 'F' and (to_char(or_date, 'MM') between '04' and '06') then '2' 
                        when qtr_paid = 'F' and (to_char(or_date, 'MM') between '07' and '09') then '3' 
                        when qtr_paid = 'F' and (to_char(or_date, 'MM') between '10' and '12') then '4'                         
                        else Qtr_Paid 
                        end as Qtr_Paid 
                        from pay_hist 
                        where bin = :BIN 
                        and tax_year < :TaxYear 
                        union all 
                        select tax_year, qtr qtr_paid from bill_hist where bin = :BIN and tax_year < :TaxYear
                        order by tax_year, qtr_paid";
            result.AddParameter(":BIN", BIN);
            result.AddParameter(":TaxYear", AppSettingsManager.GetCurrentDate().Year);
            if (result.Execute())
            {
                while (result.Read())
                {
                    int.TryParse(result.GetString("Qtr_Paid"), out intBilledQtr);
                }
            }
            result.Close();

            return intBilledQtr;
        }

        /// <summary>
        /// AST 20150114
        /// </summary>
        /// <param name="BIN"></param>
        /// <param name="TaxYear"></param>
        /// <returns></returns>
        public static bool ValidateTaxYearAndQtrPaid(string BIN, string TaxYear)
        {
            return true; // remove this line if needed this function.



            if (GetBnsStat(BIN) != "NEW")
                return true;

            int intLastQtrPaid = 0;
            OracleResultSet result = new OracleResultSet();
            string strQtrQuery = @"case 
            when qtr_paid = 'F' and (to_char(or_date, 'MM') between '01' and '03') then '1' 
            when qtr_paid = 'F' and (to_char(or_date, 'MM') between '04' and '06') then '2' 
            when qtr_paid = 'F' and (to_char(or_date, 'MM') between '07' and '09') then '3' 
            when qtr_paid = 'F' and (to_char(or_date, 'MM') between '10' and '12') then '4' 
            else Qtr_Paid 
            end as Qtr_Paid ";

            result.Query = string.Format("SELECT {0} FROM pay_hist where bin = :BIN and tax_year = :TaxYear order by tax_year, qtr_paid", strQtrQuery);
            result.AddParameter(":BIN", BIN);
            result.AddParameter(":TaxYear", TaxYear);
            if (result.Execute())
            {
                while (result.Read())
                {
                    int.TryParse(result.GetString(0), out intLastQtrPaid);
                }
            }
            result.Close();

            if (intLastQtrPaid == 4)
                return true;
            else if (intLastQtrPaid < 4)
            {
                string strTmpQtr = string.Empty;
                //intLastQtrPaid++; // Increment last qtr paid.
                switch (intLastQtrPaid)
                {
                    case 1:
                        strTmpQtr = "1st";
                        break;
                    case 2:
                        strTmpQtr = "2nd";
                        break;
                    case 3:
                        strTmpQtr = "3rd";
                        break;
                    case 4:
                        strTmpQtr = "4th";
                        break;
                }

                string strMessage = string.Format("You have only {0} quarter paid for Tax-year: {1}.\nPlease pay the remaining quarter.", strTmpQtr, TaxYear);

                if (intLastQtrPaid == 0)
                    strMessage = string.Format("You have no paid for Tax-year: {1}.\nPlease pay the remaining quarter.", strTmpQtr, TaxYear);

                MessageBox.Show(strMessage, "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            return false;
        }

        /// <summary>
        /// AST 20160114 added this method.
        /// they wanted to specify bns type after MAYOR'S PERMIT
        /// e.g. MAYOR'S PERMIT - MAYOR'S PERMIT (SARI SARI STORE)
        /// </summary>
        /// <returns></returns>
        public static string EnhanceFeesDesc(string Query, string BIN, string Description, string FeesCode)
        {
            if (!Description.Contains("MAYOR'S PERMIT FEE"))
                return Description;

            string strDescription = Description;
            string strFeesCode = string.Empty;
            string[] arrFeesCode;
            OracleResultSet result = new OracleResultSet();

            if (FeesCode.Contains("-"))
            {
                arrFeesCode = FeesCode.Split('-');
                strFeesCode = arrFeesCode[arrFeesCode.Length - 1];
            }
            else
            {
                strFeesCode = FeesCode;
            }

            result.Query = string.Format("SELECT REPLACE(Fees_Desc, 'TAX ON ', '') AS Fees_Desc FROM ({0}) ", Query);
            result.Query += string.Format("WHERE Fees_Code LIKE '%{0}' AND Fees_Desc LIKE 'TAX ON%'", strFeesCode);
            if (result.Execute())
            {
                if (result.Read())
                {
                    strDescription = result.GetString("Fees_Desc");
                    strDescription = string.Format("MAYOR'S PERMIT FEE FOR {0}", strDescription);
                }
            }
            result.Close();

            return strDescription;
        }

        /// <summary>
        /// AST 20160128 added this method
        /// </summary>
        /// <param name="ORNo"></param>
        /// <param name="refORNo">This is to avoid looping.</param>
        /// <returns></returns>
        public static string GetOtherOrNumber(string ORNo, string refORNo)
        {
            OracleResultSet result = new OracleResultSet();
            string strOtherORNo = string.Empty;
            result.Query = string.Format("SELECT DISTINCT TRIM(REPLACE(REPLACE(REPLACE(Memo, 'MULTIPLE O.R.: ', ''), '{0}', ''), ',', '')) OR_NO ", ORNo);
            result.Query += string.Format("FROM Pay_Hist WHERE OR_No = '{0}' ", ORNo);
            result.Query += "AND Memo LIKE 'MULTIPLE O.R.:%' ";            
            if (result.Execute())
            {
                if (result.Read())
                {
                    strOtherORNo = result.GetString(0);
                }
            }
            result.Close();

            if (strOtherORNo == refORNo)
                return string.Empty;

            return strOtherORNo;
        }

        public static string GetPermitNo(string sBIN)
        {
            // RMC 20171122 added printing of current permit no. in Payment hist printing
            OracleResultSet result = new OracleResultSet();
            string sPermitNo = "";

            result.Query = "select permit_no from businesses where bin = '" + sBIN + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sPermitNo = result.GetString(0);
                }
            }
            result.Close();

            return sPermitNo;
        }

        public static bool bnsIsDiscDelq(string sBin) //MCR 20180117 Check if Discovery Delinquent
        {
            bool bValue = false;
            bool bIsRen = false;
            int iTaxYearQ = 0;
            int iTaxYearB = 0;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select bin,tax_year,bns_stat from business_que where bin = '" + sBin.Trim() + "' and bns_stat = 'NEW'";
            if (result.Execute())
            {
                if (result.Read())
                    iTaxYearQ = Convert.ToInt32(result.GetString(1));
                else
                {
                    bValue = false;
                    return bValue;
                }
            }
            result.Close();

            result.Query = "select distinct bin,tax_year,due_state from taxdues where bin = '" + sBin.Trim() + "' and (due_state = 'R' or due_state = 'N') order by tax_year desc";
            if (result.Execute())
            {
                if (result.Read())
                {
                    iTaxYearB = Convert.ToInt32(result.GetString(1));
                    if (result.GetString(2) == "R")
                        bIsRen = true;
                }
            }
            result.Close();

            if (iTaxYearB > iTaxYearQ && bIsRen == true)
                bValue = true;

            if (AppSettingsManager.GetCurrentDate().Year > iTaxYearQ && bIsRen == false)
                bValue = true;

            return bValue;
        }

    }
}