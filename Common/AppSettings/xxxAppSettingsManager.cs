using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using System.IO;


namespace Amellar.Common.AppSettings
{
    //public sealed class AppSettingsManager
    public static class AppSettingsManager
    {
        private static string sObject;
        public static SystemUser g_objSystemUser;
        
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
            result.Query = "SELECT SYSDATE FROM DUAL";
            if (result.Execute())
            {
                if (result.Read())
                {
                    dtSystemDate = result.GetDateTime(0);
                }
            }
            result.Close();
            return dtSystemDate;
            
            //return new DateTime(2009, 01, 13);
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


        public static string GetBnsAddress(string sBin)
        {
            string sAddress = string.Empty;
            string sOwnAdd = string.Empty;
            string sOwnBrgy = string.Empty;
            string sOwnSt = string.Empty;
            string sOwnMun = string.Empty;

            OracleResultSet xxx = new OracleResultSet();
            xxx.Query = "SELECT * from businesses where bin = :1";
            xxx.AddParameter(":1", sBin);
            if (xxx.Execute())
            {
                if (xxx.Read() == true)
                {
                    sOwnAdd = xxx.GetString("bns_house_no").Trim();
                    sOwnBrgy = xxx.GetString("bns_brgy").Trim();
                    sOwnSt = xxx.GetString("bns_street").Trim();
                    sOwnMun = xxx.GetString("bns_mun").Trim();
                    sAddress = sOwnAdd + " " + sOwnSt + ", " + sOwnBrgy + ", " + sOwnMun;
                }
            }
            xxx.Close();
            return sAddress;
        }

        public static string GetOwnerName(string sOwnCode)
        {
            string sName = string.Empty;
            string sOwnFn = string.Empty;
            string sOwnMi = string.Empty;
            string sOwnLn = string.Empty;

            OracleResultSet xxx = new OracleResultSet();
            xxx.Query = "SELECT * from own_names where own_code = :1";
            xxx.AddParameter(":1", sOwnCode);
            if (xxx.Execute())
            {
                if (xxx.Read() == true)
                {
                    sOwnFn = xxx.GetString("own_fn").Trim();
                    sOwnMi = xxx.GetString("own_mi").Trim();
                    sOwnLn = xxx.GetString("own_ln").Trim();
                    if (sOwnMi == "." || sOwnMi.Length == 0)
                        sName = sOwnFn + " " + sOwnLn;
                    else
                        sName = sOwnFn + " " + sOwnMi + ". " + sOwnLn;

                }
            }
            xxx.Close();
            return sName;
        }

        public static string GetBnsType(string sBnsCode)
        {
            string sBnsType = string.Empty;
            OracleResultSet xxx = new OracleResultSet();
            xxx.Query = "SELECT bns_desc from bns_table where bns_code = :1 and fees_code = 'B'";
            xxx.AddParameter(":1", sBnsCode);
            if (xxx.Execute())
            {
                if (xxx.Read() == true)
                {
                    sBnsType = xxx.GetString("bns_desc").Trim();
                }
            }
            xxx.Close();
            return sBnsType;
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
