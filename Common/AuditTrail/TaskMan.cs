using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;

namespace Amellar.Common.AuditTrail
{
    public static class TaskMan
    {
        public static bool IsObjectLock(string sObject, string sDetails, string sMode, string sCode)
        {
            string strUserCode = AppSettingsManager.SystemUser.UserCode;
            string strWorkStationName = AppSettingsManager.GetWorkstationName();
            DateTime dtTime = AppSettingsManager.GetSystemDate();

            if (sMode == "DELETE")
            {
                OracleResultSet result = new OracleResultSet();

                result.Query = "delete from module_info where object = :1 and user_code = :2 and details = :3";
                result.AddParameter(":1", sObject);
                result.AddParameter(":2", strUserCode);
                result.AddParameter(":3", StringUtilities.StringUtilities.HandleApostrophe(sDetails));
                if (result.ExecuteNonQuery() == 0)
                {
                    result.Rollback();
                    result.Close();
                }
                result.Close();
                return true;
            }
            else
            {
                string strDetails, strMessage, strModUserCode;
                OracleResultSet result = new OracleResultSet();

                result.Query = "select * from module_info where trim(object) = :1";
                result.AddParameter(":1", sObject);
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        strDetails = result.GetString("details").Trim();
                        dtTime = result.GetDateTime("sys_date");
                        strModUserCode = result.GetString("user_code").Trim();

                       /* if (strUserCode == strModUserCode)
                            return false;
                        else*/  // RMC 20110810 modified locking of module/record
                        {
                            strMessage = "Access Denied.\n\nModule: " + strDetails + "\nUser: " + strModUserCode + "\nTime: " + dtTime;
                            MessageBox.Show(strMessage, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return true;
                        }
                    }
                    else
                    {
                        if (sObject != string.Empty)
                        {
                            result = new OracleResultSet();

                            result.Query = "insert into module_info (object, sys_date, user_code, details, system) values (:1, :2, :3, :4, :5)";
                            result.AddParameter(":1", sObject.Trim());
                            result.AddParameter(":2", dtTime);
                            result.AddParameter(":3", strUserCode);
                            result.AddParameter(":4", StringUtilities.StringUtilities.HandleApostrophe(sDetails));
                            result.AddParameter(":5", sCode);
                            if (result.ExecuteNonQuery() == 0)
                            {
                                result.Rollback();
                                result.Close();
                            }
                            return false;
                        }
                    }
                }
                result.Close();
            }
            return true;
        }

        public static bool IsObjectLock(string strObject)
        {
            string strUserCode = AppSettingsManager.SystemUser.UserCode;
            DateTime dtTime = AppSettingsManager.GetSystemDate();
            string strMessage = string.Empty;

            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from active_modules where trim(module_code) = :1";
            result.AddParameter(":1", strObject);
            if (result.Execute())
            {
                if (result.Read())
                {
                    dtTime = result.GetDateTime("time_in");
                    strUserCode = result.GetString("user_code").Trim();
                    return true;
                }
            }
            result.Close();

            return false;
        }
    }
}
