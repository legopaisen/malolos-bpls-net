using System;
using System.Collections.Generic;
using System.Text;

namespace Amellar.Common.PrintUtilities
{
    //@author R.D.Ong
    public class VSPrinterTable
    {
        private List<VSPrinterTableItem> m_lstTables;
        private VSPrinterFontProperty fontProperty;

        //int intLastXPosition;
        //int intLastYPosition;

        public VSPrinterTable()
        {
            m_lstTables = new List<VSPrinterTableItem>();
            fontProperty = new VSPrinterFontProperty();
        }

        public void AddColumn(VSPrinterTableItem item)
        {
            m_lstTables.Add(item);
        }

        public void Dispose()
        {
            m_lstTables.Clear();
            fontProperty.Clear();
        }


        public VSPrinterFontProperty Font
        {
            get { return fontProperty; }
            set { fontProperty = value; }
        }

        public void Clear()
        {
            m_lstTables.Clear();
        }

        public List<VSPrinterTableItem> Items
        {
            get { return m_lstTables; }
            set { m_lstTables = value; }
        }
    }
}
