
// RMC 20110823 added validation of buss serial
// RMC 20110725 added validation of own_code 

using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;

namespace Amellar.Common.AppSettings
{
    public static class Serials
    {
        public static string m_sOwnCode = string.Empty;
        public static string m_sBinSeries = string.Empty;
        public static bool bSwitch = false;
        public static bool bSwitchBin = false;  // RMC 20110823

        public static string GetOwnSerial()
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select own_code from own_serial";
            if(result.Execute())
            {
                if (result.Read())
                    m_sOwnCode = result.GetString("own_code").Trim();

                // RMC 20161121 correction in getting initial value of own_code (s)
                if (m_sOwnCode == "")
                    m_sOwnCode = "0";
                // RMC 20161121 correction in getting initial value of own_code (e)
            }
            result.Close();

            // RMC 20110725 (s)
            if (!bSwitch)
            {
                while (!CheckOwnCode(m_sOwnCode))
                {
                    UpdateOwnSerial();
                    GetOwnSerial();
                }
            }
            bSwitch = false;
            // RMC 20110725 (e)

            return m_sOwnCode;
        }

        public static void UpdateOwnSerial()
        {
            bSwitch = true; // RMC 20110725
            string strOwnCode = GetOwnSerial();
            int iOwnSerial = 0;
            int.TryParse(strOwnCode, out iOwnSerial);
            iOwnSerial = iOwnSerial + 1;
            strOwnCode = iOwnSerial.ToString();

            OracleResultSet result = new OracleResultSet();
            // RMC 20161121 correction in getting initial value of own_code (s)
            result.Query = "select * from own_serial";
            if (result.Execute())
            {
                if (!result.Read())
                {
                    result.Close();
                    result.Query = "insert into own_serial values (";
                    result.Query += "'" + strOwnCode.Trim() + "')";
                }// RMC 20161121 correction in getting initial value of own_code (e)
                else
                {
                    result.Close();
                    result.Query = "update own_serial set own_code = :1";
                    result.AddParameter(":1", strOwnCode.Trim());
                }
                if (result.ExecuteNonQuery() != 0)
                {
                }
                
            }
            result.Close();
        }

        public static string GetBussSerial()
        {
            OracleResultSet result = new OracleResultSet();

            result.Query = "select bin from buss_series where tax_year = '" + AppSettingsManager.GetConfigValue("12") + "'";   // RMC 20111129 modified initialization of serials
            if (result.Execute())
            {
                if (result.Read())
                    m_sBinSeries = result.GetString("bin").Trim();
                else
                    m_sBinSeries = "1"; // RMC 20111129 modified initialization of serials
            }
            result.Close();

            // RMC 20110725 (s)
            if (!bSwitchBin)
            {
                m_sBinSeries = string.Format("{0:0#####0}", int.Parse(m_sBinSeries));

                while (!CheckBin(m_sBinSeries))
                {
                    UpdateBussSerial();
                    GetBussSerial();
                }
            }
            bSwitchBin = false;
            // RMC 20110725 (e)

            m_sBinSeries = string.Format("{0:0#####0}", int.Parse(m_sBinSeries));
            return m_sBinSeries;
        }

        public static void UpdateBussSerial()
        {
            bSwitchBin = true;  // RMC 20110823  
            string strBinCode = GetBussSerial();
            int iBinSerial = 0;
            int.TryParse(strBinCode, out iBinSerial);
            iBinSerial = iBinSerial + 1;
            strBinCode = iBinSerial.ToString();

            OracleResultSet result = new OracleResultSet();
            // RMC 20111129 modified initialization of serials (s)
            int iCnt = 0;
            result.Query = "select count(*) from buss_series where tax_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
            int.TryParse(result.ExecuteScalar(), out iCnt);

            if (iCnt == 0)
            {
                result.Query = "insert into buss_series values (";
                result.Query += "'1','" + AppSettingsManager.GetConfigValue("12") + "')";
                if (result.ExecuteNonQuery() == 0)
                { }
            }   // RMC 20111129 modified initialization of serials (e)
            else
            {
                result.Query = "update buss_series set bin = :1 where tax_year = :2";   // RMC 20111129 modified initialization of serials
                result.AddParameter(":1", strBinCode.Trim());
                result.AddParameter(":2", AppSettingsManager.GetConfigValue("12")); // RMC 20111129 modified initialization of serials
                if (result.ExecuteNonQuery() != 0)
                {
                }
                result.Close();
            }
        }

        // RMC 20110725 (s)
        public static bool CheckOwnCode(string sOwnCode)
        {
            OracleResultSet result = new OracleResultSet();

            result.Query = string.Format("select * from own_names where own_code = '{0}'", sOwnCode);
            if (result.Execute())
            {
                if (result.Read())
                    return false;
            }
            result.Close();

            return true;
        }
        // RMC 20110725 (e)

        // RMC 20110823 (s)
        public static bool CheckBin(string sBinSeries)
        {
            OracleResultSet result = new OracleResultSet();
            string sBin = "";

            sBin = ConfigurationAttributes.LGUCode + "-" + ConfigurationAttributes.DistCode + "-" + ConfigurationAttributes.CurrentYear + "-" + sBinSeries;

            result.Query = string.Format("select * from businesses where bin = '{0}'",sBin);
            if (result.Execute())
            {
                if (result.Read())
                {
                    result.Close();

                    return false;
                }
                else
                {
                    result.Close();

                    result.Query = string.Format("select * from business_que where bin = '{0}'", sBin);
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            result.Close();

                            return false;
                        }
                    }
                }
            }
            result.Close();

            return true;
        }
        // RMC 20110823 (e)
    }
}
