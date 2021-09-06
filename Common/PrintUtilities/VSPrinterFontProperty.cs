using System;
using System.Collections.Generic;
using System.Text;

namespace Amellar.Common.PrintUtilities
{
    //@author R.D.Ong
    public class VSPrinterFontProperty
    {
        private string m_strFontName;
        private int m_intFontSize;
        private int m_intFontStyle;

        public VSPrinterFontProperty()
        {
            this.Clear();
        }

        public VSPrinterFontProperty(string strFontName,
            int intFontSize, int intFontStyle)
        {
            m_strFontName = strFontName;
            m_intFontSize = intFontSize;
            m_intFontStyle = intFontStyle;
        }

        public void Clear()
        {
            m_strFontName = string.Empty;
            m_intFontStyle = 0;
            m_intFontSize = 0;
        }

        public string FontName
        {
            get { return m_strFontName; }
            set { m_strFontName = value; }
        }

        public int FontSize
        {
            get { return m_intFontSize; }
            set { m_intFontSize = value; }
        }

        public int FontStyle
        {
            get { return m_intFontStyle; }
            set { m_intFontStyle = value; }
        }


        public bool IsEmpty
        {
            get { return (m_strFontName == string.Empty); }
        }

        /*
        public Font GetFont()
        {

        }
        */
    }
}
