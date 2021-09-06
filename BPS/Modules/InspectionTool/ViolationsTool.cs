using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;

namespace InspectionTool
{
    public static class ViolationsTool
    {
        public static bool IsViolation(string sBin)
        {
            OracleResultSet pRec1 = new OracleResultSet();

            string sDesc = string.Empty;

            pRec1.Query = string.Format("select * from violations where bin = '{0}'", sBin);
            if (pRec1.Execute())
            {
                if (pRec1.Read())
                {
                    return true;
                }
            }
            pRec1.Close();

            return false;
        }

        public static string BinViolation(string sBin)
        {
            // RMC 20171206 modified notice of closure
            OracleResultSet pRec1 = new OracleResultSet();

            string sDesc = string.Empty;
            string sCode = string.Empty;
                
            pRec1.Query = string.Format("select * from violations where bin = '{0}'", sBin);
            if (pRec1.Execute())
            {
                int iCnt = 1;
                while (pRec1.Read())
                {
                    sCode = pRec1.GetString("violation_code");

                    if (iCnt > 1)
                        sDesc += "\n";
                    sDesc += GetViolationDesc(sCode);
                }
            }
            pRec1.Close();

            return sDesc;
        }

        public static string GetViolationDesc(string sCode)
        {
            // RMC 20171206 modified notice of closure
            // copied same in InspectorDetail/Violations/GetViolationDesc

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
