using System;
using System.Collections.Generic;
using System.Text;

namespace Amellar.Common.DeficientRecords
{
    public class DeficientRecordsToolInfoItem:DeficientRecordsToolItem
    {
        private string m_strCategoryCode;
        private bool m_blnIsRequired;

        public DeficientRecordsToolInfoItem(string strCategoryCode, string strCode, string strName, string strIsRequired)
        {
            m_strCategoryCode = strCategoryCode;
            this.Key = strCode;
            this.Value = strName;
            m_blnIsRequired = false;
            if (strIsRequired == "Y")
                m_blnIsRequired = true;
        }

        public string CategoryCode
        {
            get { return m_strCategoryCode; }
            set { m_strCategoryCode = value; }
        }

        public bool IsRequired
        {
            get { return m_blnIsRequired; }
            set { m_blnIsRequired = value; }
        }

    }
}
