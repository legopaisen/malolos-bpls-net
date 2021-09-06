using System;
using System.Collections.Generic;
using System.Text;

namespace Amellar.Common.DeficientRecords
{
    public class DeficientRecordsToolItem
    {
        private int m_intSerial;
        private string m_strKey;
        private string m_strValue;

        public int Serial
        {
            get { return m_intSerial; }
            set { m_intSerial = value; }
        }

        public string Key
        {
            get { return m_strKey; }
            set { m_strKey = value; }
        }

        public string Value
        {
            get { return m_strValue; }
            set { m_strValue = value; }
        }
    }

}
