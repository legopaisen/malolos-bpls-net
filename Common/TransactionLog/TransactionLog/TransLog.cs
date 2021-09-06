// RMC 20170822 added transaction log feature for tracking of transactions per bin

using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Common.TransactionLog
{
    public static class TransLog
    {
        public static int UpdateLog(string m_sBIN, string m_sAppStat, string m_sTaxYear, string m_sTransCode, DateTime m_dTimeIn,DateTime m_dTimeOut)
        {
            OracleResultSet pCmd = new OracleResultSet();

            pCmd.Query = "insert into trans_log (bin, app_stat, tax_year, trans_code, trans_in, trans_out, usr_code) values (:1,:2,:3,:4,:5,:6,:7)";
            pCmd.AddParameter(":1", m_sBIN);
            pCmd.AddParameter(":2", m_sAppStat);
            pCmd.AddParameter(":3", m_sTaxYear);
            pCmd.AddParameter(":4", m_sTransCode);
            pCmd.AddParameter(":5", m_dTimeIn);
            pCmd.AddParameter(":6", m_dTimeOut);
            pCmd.AddParameter(":7", AppSettingsManager.SystemUser.UserCode);
            if (pCmd.ExecuteNonQuery() == 0)
            {
                return 0;
            }
            return 1;
        }
    }
}
