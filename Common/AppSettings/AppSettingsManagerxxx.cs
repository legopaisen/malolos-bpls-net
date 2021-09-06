using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using System.IO;
using Amellar.Common.DataConnector;

namespace Amellar.Common.AppSettings
{
    //public sealed class AppSettingsManager
    public static class AppSettingsManager
    {
        
        private static string sObject;
        public static SystemUser g_objSystemUser;
        public static string m_sOwnCode = string.Empty;
        
        /*
        static readonly AppSettingsManager instance = new AppSettingsManager();

        public static AppSettingsManager Instance
        {
            get
            {
                return instance;
            }
        }
         */

        public static SystemUser SystemUser
        {
            get { return g_objSystemUser; }
            set { g_objSystemUser = value; }
        }

        /// <summary>
        /// This static method returns current date/time of database server
        /// </summary>
        /// <returns>the current date/time</returns>
        public static DateTime GetSystemDate() // ALJ 20090902 change to public // pending cosder freeze date
        {
            
            DateTime dtSystemDate = DateTime.Now;
            OracleResultSet result = new OracleResultSet();
            // ALJ 20100616 (s) test if system date is freezed
            result.Query = "SELECT or_datetime FROM or_date_setup WHERE freeze = '1'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    dtSystemDate = result.GetDateTime(0); // freeze system date
                }
                else
                {
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
            if (iMonth < 7)
                sQtr = "2";
            if (iMonth < 10)
                sQtr = "3";
            else
                sQtr = "4";

            return sQtr;
        }

        public static bool bEnlistOwner(string strOwnLn, string strOwnFn, string strOwnHouseNo, string strOwnSt, string strOwnBrgy, string strOwnMun, string strOwnProv)
        {
            bool bResult = true;
            OracleResultSet result = new OracleResultSet();
            result.Query = string.Format("select * from own_names where own_ln = '{0}' and (own_fn = '{1}' or own_fn is null or own_fn = ' ') and (own_house_no = '{2}' or own_house_no is null or own_house_no = ' ') and own_street = '{3}' and (own_brgy = '{4}' or own_brgy is null or own_brgy = ' ') and own_mun = '{5}' and (own_prov = '{6}' or own_prov is null or own_prov = ' ')", strOwnLn.Trim(), strOwnFn.Trim(), strOwnHouseNo.Trim(), strOwnSt.Trim(), strOwnBrgy.Trim(), strOwnMun.Trim(), strOwnProv.Trim());
            if(result.Execute())
            {
                if (result.Read())
                {
                    m_sOwnCode = result.GetString("own_code").Trim();
                    bResult = false;
                }
            }
            result.Close();
            return bResult;
        }


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

        public static string GetLastPaymentInfo(string sBin, string sInfo)
        {
            string sOrNo = string.Empty;
            string sAmount = string.Empty;
            double dAmount = 0;

            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from pay_hist where bin = '" + sBin.Trim() + "' order by or_date desc, qtr_paid desc";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sOrNo = result.GetString("or_no").Trim();
                }
            }
            result.Close();

            result.Query = "select sum(fees_amtdue) as amount from or_table where or_no = '" + sOrNo.Trim() + "'";
            if(result.Execute())
            {

                if(result.Read())
                {
                    dAmount = result.GetFloat("amount");
                    sAmount = string.Format("{0:#,##0.00}", dAmount);
                }
            }
            result.Close();

            if (sInfo.Trim() == "OR")
                return sOrNo.Trim();
            else
                return sAmount;
        }

        public static string GetBnsDesc(string sBnsCode)
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from bns_table where bns_code = '" + sBnsCode.Trim() + "' and fees_code = 'B'";
            if(result.Execute())
            {
                if(result.Read())
                {
                    sBnsCode = result.GetString("bns_desc").Trim();
                }
            }
            result.Close();
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

            if (sMi.Trim() == string.Empty)
                sName = sFn + " " + sLn;
            else
                sName = sFn + " " + sMi + ". " + sLn;

            return sName;
        }

        public static string GetBillNoAndDate(string sBin, string sTaxYear, string sBnsCode)
        {
            OracleResultSet result = new OracleResultSet();
            string sBillNo = string.Empty;
            string sBillDate = string.Empty;
            DateTime dBillDate = new DateTime();
            string sInfo = string.Empty;
            result.Query = "select * from bill_no where bin = '" + sBin.Trim() + "' and tax_year = '" + sTaxYear.Trim() + "' and bns_code_main = '" + sBnsCode.Trim() + "'";
            if(result.Execute())
            {
                if(result.Read())
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
                    if(result1.Execute())
                    {
                        if(result1.Read())
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

            if(sBnsHouseNo.Trim() == ".")
                //sAddress = sBnsStreet + ", " + sBnsBrgy + ", " + sBnsMun;
                sAddress = sBnsStreet + ", " + sBnsBrgy;
            else
//                sAddress = sBnsHouseNo + " " + sBnsStreet + ", " + sBnsBrgy + ", " + sBnsMun;
                sAddress = sBnsHouseNo + " " + sBnsStreet + ", " + sBnsBrgy;

            return sAddress;
        }

        public static string ValidUntil(string sBin, string sCurrentYear)
        {
            string sMonth = string.Empty;
            string staxYear = string.Empty;
            string sCurrentDate = string.Empty;
            string sQtrToPayDate = string.Empty;
            string sDate = string.Empty;
            sCurrentDate = DateTime.Now.ToShortDateString();
            OracleResultSet result = new OracleResultSet();
            DateTime dDueDate = new DateTime();
            result.Query = "select qtr_to_pay,tax_year from taxdues where bin = '" + sBin.Trim() + "'order by tax_year desc, qtr_to_pay desc";
            if(result.Execute())
            {
                if (result.Read())
                {
                    sMonth = result.GetString("qtr_to_pay").Trim();
                    staxYear = result.GetString("tax_year").Trim();
                    if (sMonth == "1")
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
                        sQtrToPayDate = "1-DEC-" + staxYear;
                }
                else
                    dDueDate = DateTime.Parse(sCurrentDate);
            }
            result.Close();

            result.Query = "select * from due_dates where due_year = '" + sCurrentYear + "' and due_date >= '" + sQtrToPayDate + "' order by due_code";
            if(result.Execute())
            {
                if(result.Read())
                {
                    dDueDate = result.GetDateTime("due_date");
                    sDate = dDueDate.ToShortDateString();
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
            if(sMonth == "05")
                sMonth = "May";
            if(sMonth == "06")
                sMonth = "June";
            if (sMonth == "07")
                sMonth = "July";
            if(sMonth == "08")
                sMonth = "August";
            if(sMonth == "09")
                sMonth = "September";
            if(sMonth == "10")
                sMonth = "October";
            if(sMonth == "11")
                sMonth = "November";
            if(sMonth == "12")
                sMonth = "December";
                
            sDate = sMonth + " " + sDay + ", " + sYear;
            return sDate;
        }

        // GDE 20101109

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
                    }
                }

            }
            result.Close();
            return sBnsName;
        }

        public static string GetOwnCode(string sBin)
        {
            string sOwnCode = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select own_code from businesses where bin = '" + sBin.Trim() + "'";
            if(result.Execute())
            {
                if(result.Read())
                {
                    sOwnCode = result.GetString("own_code").Trim();
                }
                else
                {
                    result.Close();
                    result.Query = "select * from business_que where bin = '" + sBin.Trim() + "'";
                    if(result.Execute())
                    {
                        if(result.Read())
                        {
                            sOwnCode = result.GetString("own_code").Trim();
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
                    sOwnCode = result.GetString("busn_code").Trim();
                }
                else
                {
                    result.Close();
                    result.Query = "select * from business_que where bin = '" + sBin.Trim() + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            sOwnCode = result.GetString("busn_code").Trim();
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
            
            result.Query = "select * from own_names where own_code = '" + sOwnCode.Trim() + "'";
            if(result.Execute())
            {
                if(result.Read())
                {
                    sOwnHouseNo = result.GetString("own_house_no").Trim();
                    sOwnStreet = result.GetString("own_street").Trim();
                    sOwnBrgy = result.GetString("own_brgy").Trim();
                    sOwnMun = result.GetString("own_mun").Trim();

                    sBnsOwnerAdd = sOwnHouseNo + " " + sOwnStreet + ", " + sOwnBrgy + ", " + sOwnMun;
                }
            }
            result.Close();

            return sBnsOwnerAdd;
        }

        public static string GetBnsCodeByDesc(string sDesc)
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select bns_code from bns_table where bns_desc = :1 and fees_code = 'B'";
            result.AddParameter(":1", sDesc.Trim());
            if(result.Execute())
            {
                if (result.Read())
                    sDesc = result.GetString("bns_code").Trim();
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
            if(result.Execute())
            {
                if(result.Read())
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

        public static string GetBnsAdd(string sBin)
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

                    sBnsAdd = sBnsHouseNo + " " + sBnsStreet + ", " + sBnsBrgy + ", " + sBnsMun;
                }
            }
            result.Close();

            return sBnsAdd;
        }


        public static string GetCapitalGross(string sBin, string sBnsCode, string sTaxYear, string sType)
        {
            OracleResultSet result = new OracleResultSet();
            string sData = "0.00";

            if(sType == "PRE")
            {
                result.Query = "select * from declared_gross where bin = :1 and bns_code = :2 and tax_year = :3";
                result.AddParameter(":1", sBin.Trim());
                result.AddParameter(":2", sBnsCode.Trim());
                result.AddParameter(":3", sTaxYear.Trim());
                if(result.Execute())
                {
                    if(result.Read())
                    {
                        sData = string.Format("{0:#,##0.00}", result.GetDouble("presumptive_gr"));
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
                        sData = string.Format("{0:#,##0.00}", result.GetDouble("capital"));
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
                                sData = string.Format("{0:#,##0.00}", result.GetDouble("capital"));
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
                                        sData = string.Format("{0:#,##0.00}", result.GetDouble("capital"));
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
                                                sData = string.Format("{0:#,##0.00}", result.GetDouble("capital"));
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
            if(sType == "REN")
            {
                result.Query = "select * from business_que where bin = :1 and bns_code = :2 and tax_year = :3 and bns_stat = 'REN'";
                result.AddParameter(":1", sBin.Trim());
                result.AddParameter(":2", sBnsCode.Trim());
                result.AddParameter(":3", sTaxYear.Trim());
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        sData = string.Format("{0:#,##0.00}", result.GetDouble("gr_1"));
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
                                sData = string.Format("{0:#,##0.00}", result.GetDouble("gr_1"));
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
                                        sData = string.Format("{0:#,##0.00}", result.GetDouble("gr_1"));
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
                                                sData = string.Format("{0:#,##0.00}", result.GetDouble("gross"));
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

            if(sType == "RET")
            {
                result.Query = "select * from retired_bns where bin = :1 and bns_code_main = :2 and tax_year = :3";
                result.AddParameter(":1", sBin.Trim());
                result.AddParameter(":2", sBnsCode.Trim());
                result.AddParameter(":3", sTaxYear.Trim());
                if(result.Execute())
                {
                    if(result.Read())
                    {
                        sData = string.Format("{0:#,##0.00}", result.GetDouble("gross"));
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
                                sData = string.Format("{0:#,##0.00}", result.GetDouble("gr_1"));
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
                                        sData = string.Format("{0:#,##0.00}", result.GetDouble("gr_1"));
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
                                                sData = string.Format("{0:#,##0.00}", result.GetDouble("gr_1"));
                                            }
                                            else
                                            {
                                                result.Close();
                                                result.Query = "select * from addl_bns where bin = :1 and bns_code = :2 and tax_year = :3 and bns_stat = 'REN'";
                                                result.AddParameter(":1", sBin.Trim());
                                                result.AddParameter(":2", sBnsCode.Trim());
                                                result.AddParameter(":3", sTaxYear.Trim());
                                                if (result.Execute())
                                                {
                                                    if (result.Read())
                                                    {
                                                        sData = string.Format("{0:#,##0.00}", result.GetDouble("gross"));
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
            return sData;   
        }

        public static bool WithAddlBns(string sBin)
        {
            bool bFlag = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from addl_bns where bin = '" + sBin.Trim() + "'";
            if(result.Execute())
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
            return AppSettingsManager.GetConfigValue("OBJECT", "config_table", "code",
                "RPAD(:1, 3)", strConfigCode);
        }

        public static string GetConfigValueByDescription(string strConfigDescription)
        {
            return AppSettingsManager.GetConfigValue("value_fld", "config_table", "subj_desc",
                "RPAD(:1, 255)", strConfigDescription);
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
            if(xxx.Execute())
            {
                if(xxx.Read()==true)
                {
                    sObject = xxx.GetString("object").Trim();
                }
            }
            xxx.Close();
            return sObject;
            
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

        

    }
}
