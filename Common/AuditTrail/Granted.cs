using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Common.AuditTrail
{
    public static class Granted
    {
        public static bool Grant(string strCode)
        {
            OracleResultSet result = new OracleResultSet();

            string strUserCode = AppSettingsManager.SystemUser.UserCode;

            result.Query = string.Format("select * from sys_rights where usr_code = '{0}' and usr_rights = '{1}'", strUserCode, strCode);
            if (result.Execute())
            {
                if (result.Read())
                    return true;
                else
                    return false;
            }
            
            return true;
        }
    }
}
