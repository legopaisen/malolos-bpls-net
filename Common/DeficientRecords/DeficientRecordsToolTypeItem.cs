using System;
using System.Collections.Generic;
using System.Text;

namespace Amellar.Common.DeficientRecords
{
    public class DeficientRecordsToolTypeItem : DeficientRecordsToolItem
    {
        private string m_strCategoryCode;
        private bool m_blnIsAutoCheck;
        private bool m_blnWithBIN;
        private bool m_blnWithOR;
        private string m_strFieldCode;

        /*public DeficientRecordsToolTypeItem(string strCategoryCode, string strCode, string strName, string strIsAutoCheck,
            string strIsBlockTransaction, string strKind)
        {
            m_strCategoryCode = strCategoryCode;
            this.Key = strCode;
            this.Value = strName;
            m_blnIsAutoCheck = false;
            if (strIsAutoCheck == "Y")
                m_blnIsAutoCheck = true;
            m_blnIsBlockTransaction = false;
            if (strIsBlockTransaction == "Y")
                m_blnIsBlockTransaction = true;
            m_strKind = strKind;
        }*/

        public DeficientRecordsToolTypeItem(string strCategoryCode, string strCode, string strName, string strIsAutoCheck,
            string strFieldCode, string strWithBIN, string strWithOR)
        {
            m_strCategoryCode = strCategoryCode;
            this.Key = strCode;
            this.Value = strName;
            m_blnIsAutoCheck = false;
            if (strIsAutoCheck == "Y")
                m_blnIsAutoCheck = true;
            m_blnWithBIN = false;
            if (strWithBIN == "Y")
                m_blnWithBIN = true;
            m_blnWithOR = false;
            if (strWithOR == "Y")
                m_blnWithOR = true;
            if (strFieldCode == "")
                strFieldCode = " ";
            m_strFieldCode = strFieldCode;
        }

        /*
        public DeficientRecordsToolTypeItem(int intSerial, string strCode, string strName, bool blnIsAutoCheck,
            bool blnIsBlockTransaction, string strKind)
        {
            this.Serial = intSerial;
            this.Key = strCode;
            this.Value = strName;
            m_blnIsAutoCheck = blnIsAutoCheck;
            m_blnIsBlockTransaction = blnIsBlockTransaction;
            m_strKind = strKind;
        }
        */

        public string CategoryCode
        {
            get { return m_strCategoryCode; }
            set { m_strCategoryCode = value; }
        }

        public bool IsAutoCheck
        {
            get { return m_blnIsAutoCheck; }
            set { m_blnIsAutoCheck = value; }
        }

        public bool IsWithBIN
        {
            get { return m_blnWithBIN; }
            set { m_blnWithBIN = value; }
        }

        public bool IsWithOR
        {
            get { return m_blnWithOR; }
            set { m_blnWithOR = value; }
        }

        public string FieldCode
        {
            get { return m_strFieldCode; }
            set { m_strFieldCode = value; }
        }
    }
}
