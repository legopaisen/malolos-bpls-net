//////////////////////////
// RMC 20111216 modified date in trailing - should get current date even if date was freezed


//////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Common.AuditTrail
{
    public static class AuditTrail
    {

        public static int InsertTrail(string strModuleCode, string strTable, string strObject)
        {
            OracleResultSet result = new OracleResultSet();

            DateTime dtCurrent = AppSettingsManager.GetSystemDate();
            // RMC 20111216 modified date in trailing - should get current date even if date was freezed (s)
            result.Query = "SELECT SYSDATE FROM DUAL";
            if (result.Execute())
            {
                if (result.Read())
                {
                    dtCurrent = result.GetDateTime(0); // actual system date
                }
            }
            result.Close();
            // RMC 20111216 modified date in trailing - should get current date even if date was freezed (e)
                        
            result.Query = "insert into a_trail (usr_code, tdatetime, mod_code, aff_table, work_station, object) values (:1, :2, :3, :4, :5, :6)";
            result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode);
            result.AddParameter(":2", dtCurrent);
            result.AddParameter(":3", strModuleCode);
            result.AddParameter(":4", strTable);
            result.AddParameter(":5", AppSettingsManager.GetWorkstationName());
            result.AddParameter(":6", strObject);
            if (result.ExecuteNonQuery() == 0)
            {
                return 0;
            }
            
            return 1;
        }
    }
}
