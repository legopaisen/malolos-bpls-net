using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;

//please move class to Location assembly
namespace Amellar.Common.AppSettings
{
    public class DistrictList
    {

        private List<string> m_lstDistrictCodes;
        private List<string> m_lstDistrictNames;

        public DistrictList()
        {
            m_lstDistrictCodes = new List<string>();
            m_lstDistrictNames = new List<string>();
            this.GetDistrictList(true);
        }

        public List<string> DistrictNames
        {
            get { return m_lstDistrictNames; }
        }

        //RDO 062008 (s) district code
        public List<string> DistrictCodes
        {
            get { return m_lstDistrictCodes; }
        }
        //RDO 062008 (e) district code

        public string GetDistrictCode(string strDistrictName)
        {
            int intIndex = m_lstDistrictNames.IndexOf(strDistrictName);
            if (intIndex == -1)
                return string.Empty;
            return m_lstDistrictCodes[intIndex];
        }

        public string GetDistrictName(string strDistrictCode)
        {
            int intIndex = m_lstDistrictCodes.IndexOf(strDistrictCode);
            if (intIndex == -1)
                return string.Empty;
            return m_lstDistrictNames[intIndex];
        }

        public string GetDistrictCodeByIndex(int intIndex)
        {
            if (intIndex >= 0 && intIndex < m_lstDistrictCodes.Count)
                return m_lstDistrictCodes[intIndex];
            return string.Empty;
        }

        public string GetDistrictCode(int intIndex)
        {
            //RDO 041142008 (s) district code fix for municipality
            if (m_lstDistrictCodes.Count == 1 && intIndex == 0)
                return m_lstDistrictCodes[0];
            //RDO 041142008 (e) district code fix for municipality
            else if (intIndex > 0 && intIndex < m_lstDistrictCodes.Count)
                return m_lstDistrictCodes[intIndex];

            return string.Empty;
        }

        public void GetDistrictList(bool blnIsOrderByDistName)
        {
            m_lstDistrictCodes.Clear();
            m_lstDistrictNames.Clear();

            string strDistrictCode = AppSettingsManager.GetDistrictCode();
            OracleResultSet result = new OracleResultSet();
            if (strDistrictCode == "00")
            {
                if (blnIsOrderByDistName)
                    result.Query = "SELECT dist_code, dist_nm  FROM districts ORDER BY dist_nm";
                else
                    result.Query = "SELECT dist_code, dist_nm  FROM districts ORDER BY dist_code";
            }
            else
            {
                result.Query = "SELECT dist_code, dist_nm  FROM districts WHERE dist_code = RPAD(:1, 2)";
                result.AddParameter(":1", strDistrictCode);
            }
            if (result.Execute())
            {
                while (result.Read())
                {
                    m_lstDistrictCodes.Add(result.GetString("dist_code").Trim());
                    m_lstDistrictNames.Add(result.GetString("dist_nm").Trim());
                }
            }

            result.Close();
        }


    }
}
