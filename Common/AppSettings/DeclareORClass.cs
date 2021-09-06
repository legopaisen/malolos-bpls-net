using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using System.Windows.Forms;

namespace Amellar.Common.AppSettings
{
    public  class DeclareORClass
    {
        public  string m_sTellerLn = string.Empty;
        public  string m_sTellerFn = string.Empty;
        public  string m_sTellerMi = string.Empty;

        public string m_sFromORNo = string.Empty;
        public string m_sToORNo = string.Empty;
        public string m_sCurrORNo = string.Empty;

        public bool bFlag = false;
        public bool bTellerFound = true;
        
        public bool WithOr(string sTellerCode)
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from or_current where teller = :1";
            result.AddParameter(":1", sTellerCode.Trim());
            if(result.Execute())
            {
                if (result.Read())
                {
                    bFlag = true;
                    m_sFromORNo = result.GetInt("from_or_no").ToString();
                    m_sToORNo = result.GetInt("to_or_no").ToString();
                    m_sCurrORNo = result.GetInt("cur_or_no").ToString();
                }
                else
                    bFlag = false;
            }
            result.Close();
            return bFlag;
        }

        public void LoadTeller(string sTellerCode)
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from tellers where teller = :1";
            result.AddParameter(":1", sTellerCode.Trim());
            if(result.Execute())
            {
                if(result.Read())
                {
                    m_sTellerLn = result.GetString("ln").Trim();
                    m_sTellerFn = result.GetString("fn").Trim();
                    m_sTellerMi = result.GetString("mi").Trim();
                    bTellerFound = true;
                }
                else
                {
                    MessageBox.Show("Teller Not Found.");
                    m_sTellerLn = string.Empty;
                    m_sTellerFn = string.Empty;
                    m_sTellerMi = string.Empty;
                    bTellerFound = false;

                }
            }
            result.Close();

            WithOr(sTellerCode.Trim());
        }
    }
}
