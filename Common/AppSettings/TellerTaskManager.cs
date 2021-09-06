using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;

namespace Amellar.Common.AppSettings
{
   public static class TellerTaskManager
    {
        #region //This is for validation of Teller task manager
        public static string isCheckTask(string strTellerCode)
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from teller_lock where trim(teller_name) = :1";
            result.AddParameter(":1", strTellerCode);
            if (result.Execute())
            {
                if (result.Read())
                {
                    return string.Format("The Teller {0} is already used in workstation {1}.", result.GetString("teller_name").Trim(), result.GetString("ws_name").Trim());
            
                }
            }
            result.Close();
            return "if problem occur please check the utilities\\administration tools.";
        }

        public static bool isAddTask(string strTellerCode)
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select count(*) from teller_lock where trim(teller_name) = :1";
            result.AddParameter(":1", strTellerCode);
            int intCount = 1;
            int.TryParse(result.ExecuteScalar(), out intCount);
            if (intCount == 0)
            {
                result.Query = "insert into teller_lock values(:1, :2, :3, :4)";
                result.AddParameter(":1", strTellerCode);
                result.AddParameter(":2", AppSettingsManager.GetWorkstationName());
                result.AddParameter(":3", AppSettingsManager.GetCurrentDate());
                result.AddParameter(":4", AppSettingsManager.SystemUser.UserCode);
                if (result.ExecuteNonQuery() == 1)
                {
                    
                    return true;
                }
            }
            result.Close();
            return false;
        }

        public static bool isRemoveTask(string strTellerCode)
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select count(*) from teller_lock where trim(teller_name) = :1";
            result.AddParameter(":1", strTellerCode);
            int intCount = 0;
            int.TryParse(result.ExecuteScalar(), out intCount);
            if (intCount >= 1)
            {
                result.Query = "delete from teller_lock where trim(teller_name) = :1";
                result.AddParameter(":1", strTellerCode);
                if (result.ExecuteNonQuery() == 1)
                {
                   
                    return true;
                }
            }
            result.Close();
            return false;
        }
        #endregion


    }
}
