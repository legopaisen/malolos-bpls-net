using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;

namespace Amellar.Modules.InspectorsDetails
{
    public static class Violations
    {
        public static string GetViolationDesc(string sCode)
        {
            OracleResultSet pRec1 = new OracleResultSet();

            string sDesc = string.Empty;

            pRec1.Query = string.Format("select violation_desc from violation_table where violation_code = '{0}'", sCode);
            if (pRec1.Execute())
            {
                if (pRec1.Read())
                {
                    sDesc = pRec1.GetString("violation_desc");
                }
            }
            pRec1.Close();

            return sDesc;
        }

        
    }
}
