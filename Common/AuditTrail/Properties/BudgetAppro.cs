using System;
using System.Collections.Generic;
using System.Text;

namespace Classes.Budget
{
    public class BudgetAppro
    {
        private string m_strOfficeCode;
        private int m_intExpCatId;
        private double m_dAmount;
        private int m_intOrderId;
        private int m_intYear;

        public BudgetAppro()
        {
            this.Clear();
        }

        public BudgetAppro(string strOfficeCode, int intExpCatId, double dAmount, int intOrderId, int intYear)
        {
            m_strOfficeCode = strOfficeCode;
            m_intExpCatId = intExpCatId;
            m_dAmount = dAmount;
            m_intOrderId = intOrderId;
            m_intYear = intYear;
        }

        public void Clear()
        {
            m_strOfficeCode = string.Empty;
            m_intExpCatId = -1;
            m_dAmount = 0.00F;
            m_intOrderId = -1;
            m_intYear = -1;
        }

        public string OfficeCode
        {
            get { return m_strOfficeCode; }
        }

        public int ExpCatId
        {
            get { return m_intExpCatId; }
        }

        public double Amount
        {
            get { return m_dAmount; }
        }

        public int OrderId
        {
            get { return m_intOrderId; }
        }

        public int Year
        {
            get { return m_intYear; }
        }
    }
}
