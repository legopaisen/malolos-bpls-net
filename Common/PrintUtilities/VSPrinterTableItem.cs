using System;
using System.Collections.Generic;
using System.Text;

namespace Amellar.Common.PrintUtilities
{
    //@author R.D.Ong
    public class VSPrinterTableItem
    {
        /*
        public enum TableItemAlignment
        {
            LeftAlign, 
            RightAlign, 
            CenterAlign
        }
        */

        //private TableItemAlignment alignment;
        private int m_intAlignment;
        private int m_intWidth; // -1 width means autosize (based on maximum length)
        private string m_strValue;
        private bool m_blnIsWordWrap;

        private VSPrinterFontProperty fontProperty;

        public VSPrinterTableItem(int intAlignment, int intWidth,
            string strValue)
        {
            m_intAlignment = intAlignment;
            m_intWidth = intWidth;
            m_strValue = strValue;
            fontProperty = new VSPrinterFontProperty();
            m_blnIsWordWrap = false;
        }

        public VSPrinterTableItem(int intAlignment, int intWidth,
            string strValue, bool blnIsWordWrap)
        {
            m_intAlignment = intAlignment;
            m_intWidth = intWidth;
            m_strValue = strValue;
            fontProperty = new VSPrinterFontProperty();
            m_blnIsWordWrap = blnIsWordWrap;
        }

        public bool WordWrap
        {
            get { return m_blnIsWordWrap; }
            set { m_blnIsWordWrap = value; }
        }

        public VSPrinterFontProperty Font
        {
            get { return fontProperty; }
        }

        public int Alignment
        {
            get { return m_intAlignment; }
        }

        public int Width
        {
            get { return m_intWidth; }
        }

        public string Value
        {
            get { return m_strValue; }
            set { m_strValue = value; }
        }

        public VSPrinterTableItem(int intAlignment, int intWidth,
            string strValue, VSPrinterFontProperty f)
        {
            m_intAlignment = intAlignment;
            m_intWidth = intWidth;
            m_strValue = strValue;
            fontProperty = f;
        }
    }
}
