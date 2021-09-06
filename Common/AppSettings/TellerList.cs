using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;

namespace Amellar.Common.AppSettings
{
    public class TellerList
    {
        private List<Teller> m_lstTellers;

        //private string m_strDistrictCode;

        public TellerList()
        {
            m_lstTellers = new List<Teller>();
            this.GetTellerList(AppSettingsManager.GetDistrictCode());
        }

        public List<Teller> Tellers
        {
            get { return m_lstTellers; }
        }

        public Teller GetTeller(string strTellerCode)
        {
            int intCount = m_lstTellers.Count;
            for (int i = 0; i < intCount; i++)
            {
                if (m_lstTellers[i].UserCode == strTellerCode)
                {
                    return m_lstTellers[i];
                }
            }
            return null;
        }

        public void GetTellerList(string strDistrictCode)
        {
            m_lstTellers.Clear();
            OracleResultSet result = new OracleResultSet();
            //string strDistrictCode = AppSettingsManager.GetDistrictCode();
            if (strDistrictCode == "00")
            {
                //result.Query = "SELECT sys_teller, tel_ln, tel_fn, tel_mi, tel_memo, or_code, dist_code FROM tellers, teller_assign WHERE sys_teller = teller_code ORDER BY sys_teller";
                result.Query = "select * from tellers where teller = ':1'";
                result.AddParameter(":1", strDistrictCode);
            }
            else
            {
                //result.Query = "SELECT sys_teller, tel_ln, tel_fn, tel_mi, tel_memo, or_code, dist_code FROM tellers, teller_assign WHERE sys_teller = teller_code AND dist_code = RPAD(:1, 2) ORDER BY sys_teller";
                result.Query = "select * from tellers where teller = ':1' order by teller"; 
                result.AddParameter(":1", strDistrictCode);
            }
            if (result.Execute())
            {
                while (result.Read())
                {
                    m_lstTellers.Add(new Teller(result.GetString("sys_teller").Trim(),
                        result.GetString("tel_ln").Trim(), result.GetString("tel_fn").Trim(),
                        result.GetString("tel_mi").Trim(), result.GetString("tel_memo").Trim(),
                        result.GetString("or_code").Trim(), result.GetString("dist_code").Trim()));
                }
            }
            result.Close();
        }


    }
}
