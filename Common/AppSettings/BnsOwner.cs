using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;

namespace Amellar.Common.AppSettings
{
    public class BnsOwner
    {
        public string m_strOwnLn;
        public string m_strOwnFn;
        public string m_strOwnMi;
        public string m_strOwnCode;

        public BnsOwner()
        {
            m_strOwnCode = string.Empty;
            m_strOwnLn = string.Empty;
            m_strOwnFn = string.Empty;
            m_strOwnMi = string.Empty;
        }

        public BnsOwner(string sOwnCode, string sOwnLn, string sOwnFn, string sOwnMi)
        {
            m_strOwnCode = sOwnCode;
            m_strOwnLn = sOwnLn;
            m_strOwnFn = sOwnFn;
            m_strOwnMi = sOwnMi;
        }
        public string OwnCode
        {
            set { m_strOwnCode = value; }
            get { return m_strOwnCode; }
        }

        public string OwnLn
        {
            set { m_strOwnLn = value; }
            get { return m_strOwnLn; }
        }
        public string OwnFn
        {
            set { m_strOwnFn = value; }
            get { return m_strOwnFn; }
        }
        public string OwnMi
        {
            set { m_strOwnMi = value; }
            get { return m_strOwnMi; }
        }
    }
    public class OwnerList
    {
        private List<BnsOwner> m_lstOwner;

        public OwnerList()
        {
            m_lstOwner = new List<BnsOwner>();
            this.Reset();
        }
        private void Reset()
        {
            m_lstOwner.Clear();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from own_names order by own_code";
            if(result.Execute())
            {
                while(result.Read())
                {
                    m_lstOwner.Add(new BnsOwner(result.GetString("own_code"), result.GetString("own_ln"), result.GetString("own_fn"), result.GetString("own_mi")));
                }
            }
            result.Close();
        }
        public List<BnsOwner> OwnerLst
        {
            get { return m_lstOwner; }
            set { m_lstOwner = value; }
        }

        public BnsOwner GetOwnInfoByCode(string sOwnCode)
        {
            int intCount = m_lstOwner.Count;
            for(int i = 0; i < intCount; i++)
            {
                if (m_lstOwner[i].OwnCode == sOwnCode)
                    return m_lstOwner[i];
            }
            return new BnsOwner();
        }
    }
}
