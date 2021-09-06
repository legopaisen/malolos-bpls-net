using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Amellar.Common.StringUtilities;

namespace Amellar.Common.PrintUtilities
{

    //@author R.D.Ong
   public class VSPrinterEmuModel
    {
        public enum ItemTypeEnum
        {
            TableType,
            YType,
            LeftMarginType,
            TopMarginType,
            //RDO 120707 (s) for line printing
            GetYType, 
            LineType,
            BorderType,
            //RDO 120707 (e) for line printing
            PageBreakType,
        }     


        private List<VSPrinterTable> m_lstTables;
        private List<long> m_lstYSet;
        private List<long> m_lstLeftMargin;
        private List<long> m_lstTopMargin;

        private List<ItemTypeEnum> m_lstItemTypes;
        private List<Rectangle> m_lstLineTypes;

        private VSPrinterFontProperty fontProperty;

        private List<int> m_lstBorderTypes;

        //for paragraph
        private int m_intAlignment;

        private int m_intItemTypeIndex;
        private int m_intTableIndex;
        private int m_intYSetIndex;
        private int m_intLeftMarginIndex;
        private int m_intTopMarginIndex;

        private long m_lngCurrentY;
        private int m_intLineTypeIndex;
        private int m_intBorderTypeIndex;

        //for GetCurrentY emulation
        //private System.Drawing.Printing.Margins m_marMargin;
        //private System.Drawing.Printing.PaperSize m_objPaperSize;

        //instead of getting margin and paper size we will use this instead
        private int m_intLeft;
        private int m_intTop;
        private int m_intMaxY;
        
        private float m_fltX;
        private float m_fltY;

        public float m_fltPageHeaderY;

        //page numbering
        private int m_intPageCount;

        private StringBuilder m_strHtml; //RDO 04172008 html conversion
        private bool m_blnIsHtml;

        private System.Windows.Forms.Control control;
        private Graphics g;

        public void Dispose()
        {
            for (int i = 0; i < m_lstTables.Count; i++)
            {
                m_lstTables[i].Dispose();
            }
            m_lstTables.Clear();
            
            m_lstTables = null;
            m_lstTables = new List<VSPrinterTable>();

            m_lstItemTypes.Clear();
            m_lstItemTypes = null;
            m_lstItemTypes = new List<ItemTypeEnum>();
            m_lstYSet.Clear();
            m_lstYSet = null;
            m_lstYSet = new List<long>();
            m_lstLeftMargin.Clear();
            m_lstLeftMargin = null;
            m_lstLeftMargin = new List<long>();
            m_lstTopMargin.Clear();
            m_lstTopMargin = null;
            m_lstTopMargin = new List<long>();
            m_lstLineTypes.Clear();
            m_lstLineTypes = null;
            m_lstLineTypes = new List<Rectangle>();
            m_lstBorderTypes.Clear();
            m_lstBorderTypes = null;
            m_lstBorderTypes = new List<int>();
        }

        public VSPrinterEmuModel()
        {
            m_lstItemTypes = new List<ItemTypeEnum>();

            m_lstTables = new List<VSPrinterTable>();
            m_lstYSet = new List<long>();
            m_lstLeftMargin = new List<long>();
            m_lstTopMargin = new List<long>();

            m_lstLineTypes = new List<Rectangle>();
            m_lstBorderTypes = new List<int>();


            //default font
            fontProperty = new VSPrinterFontProperty("Arial Narrow", 8, 0);
            //default alignment
            m_intAlignment = 0;

            //m_marMargin = new System.Drawing.Printing.Margins(50, 50, 50, 50);
            //m_objPaperSize = new System.Drawing.Printing.PaperSize("", 850, 1100);
            //m_intLeft = 50;   // RMC 20110831 put rem
            //m_intTop = 50;    // RMC 20110831 put rem
            //m_intMaxY = 1100 - 50;    // RMC 20110831 put rem
            

            m_intPageCount = 1;

            m_strHtml = new StringBuilder(); //RDO 04172008 html conversion
            m_blnIsHtml = false;

            control = new System.Windows.Forms.Control();
            g = control.CreateGraphics();

        }


        public int PageCount
        {
            get { return m_intPageCount; }
            set { m_intPageCount = value; }
        }

        public float PageHeaderY
        {
            set { m_fltPageHeaderY = value; }
            get { return m_fltY; }
        }

        private Font CreateFont(VSPrinterFontProperty fontProperty)
        {
            FontStyle fontStyle = new FontStyle();
            if (fontProperty.FontStyle == 0)
                fontStyle = FontStyle.Regular;
            if ((fontProperty.FontStyle & 1) != 0)
                fontStyle |= FontStyle.Bold;
            if ((fontProperty.FontStyle & 2) != 0)
                fontStyle |= FontStyle.Italic;
            if ((fontProperty.FontStyle & 4) != 0)
                fontStyle |= FontStyle.Italic;
            if ((fontProperty.FontStyle & 8) != 0)
                fontStyle |= FontStyle.Underline;
            Font font = new Font(fontProperty.FontName, (float)fontProperty.FontSize, fontStyle);
            return font;
        }

        public int LeftMargin
        {
            set { m_intLeft = value; }
            get { return m_intLeft; }
        }

        public int TopMargin
        {
            set { m_intTop = value; }
            get { return m_intTop; }
        }

        public int MaxY
        {
            set { m_intMaxY = value; }
            get { return m_intMaxY; }
        }

        /*
        public System.Drawing.Printing.PaperSize PaperSize
        {
            set { m_objPaperSize = value; }
        }

        public System.Drawing.Printing.Margins Margins
        {
            //get { return m_marMargin; }
            set { m_marMargin = value; }
        }
        */

        public string ToHtml
        {
            get 
            { 
                //return m_strHtml.ToString(); 
                return string.Format("{0}</table></body></html>", m_strHtml.ToString());
            }
        }

       public string ToExcel
       {
           get
           {
               return m_strHtml.ToString(); 
               //return string.Format("{0}</table></body></html>", m_strHtml.ToString());
           }
       }

        public bool IsHtmlAllow
        {
            get { return m_blnIsHtml; }
            set { m_blnIsHtml = value; }
        }

        public void Reset()
        {
            m_intItemTypeIndex = -1;
            m_intTableIndex = -1;
            m_intYSetIndex = -1;
            m_intLeftMarginIndex = -1;
            //RDO 120707 (s)
            m_intTopMarginIndex = -1;
            m_intLineTypeIndex = -1;
            m_intBorderTypeIndex = -1;
            m_lngCurrentY = 0;
            //RDO 120707 (e)

            //insert left margin and top margin fix here

            m_fltX = (float)m_intLeft; //m_marMargin.Left;
            m_fltY = (float)m_intTop; //m_marMargin.Top;

            //m_intPageCount = 1;
            //m_strHtml.Length = 0; //RDO 04212008

        }

        public int GetItemCount
        {
            get { return m_lstItemTypes.Count; }
        }

        public int ItemIndex
        {
            get { return m_intItemTypeIndex; }
        }

        public bool Done
        {
            get { return (m_intItemTypeIndex + 1>= m_lstItemTypes.Count); }
        }

        public bool Next()
        {
            if (m_intItemTypeIndex + 1 >= m_lstItemTypes.Count)
            {
                return false;
            }
            m_intItemTypeIndex++;
            if (m_lstItemTypes[m_intItemTypeIndex] == ItemTypeEnum.TableType)
                m_intTableIndex++;
            else if (m_lstItemTypes[m_intItemTypeIndex] == ItemTypeEnum.YType)
                m_intYSetIndex++;
            else if (m_lstItemTypes[m_intItemTypeIndex] == ItemTypeEnum.LeftMarginType)
                m_intLeftMarginIndex++;
            //RDO 120707 (s)
            else if (m_lstItemTypes[m_intItemTypeIndex] == ItemTypeEnum.TopMarginType)
                m_intTopMarginIndex++;
            else if (m_lstItemTypes[m_intItemTypeIndex] == ItemTypeEnum.LineType)
                m_intLineTypeIndex++;
            //else if (m_lstItemTypes[m_intBorderTypeIndex] == ItemTypeEnum.BorderType)
            //    m_intLine
            //RDO 120707 (e)

            return true;
        }

        //RDO 120707 (s) for line printing
        public long GetCurrentY()
        {
            /*
            m_lstItemTypes.Add(ItemTypeEnum.GetYType);
            return 999999; //instead of a negative number we use a very high number
            */
            return (long) (m_fltY * 14.4F);
        }

        public long GetCurrentX()
        {
            return (long) (m_fltX * 14.4F);
        }

        public long ActualCurrentY
        {
            set { m_lngCurrentY = value; }
            get { return m_lngCurrentY; }
        }
        //RDO 120707 (e) for line printing

        public ItemTypeEnum GetItemType()
        {
            return m_lstItemTypes[m_intItemTypeIndex]; 
        }

        public VSPrinterTable GetTable()
        {
            return m_lstTables[m_intTableIndex];
        }

        public Rectangle GetLine()
        {
            return m_lstLineTypes[m_intLineTypeIndex];
        }

        public long GetYSet()
        {
            if (this.GetItemType() == ItemTypeEnum.YType)
                return m_lstYSet[m_intYSetIndex];
            return 0;
        }

        public long GetLeftMargin()
        {
            if (this.GetItemType() == ItemTypeEnum.LeftMarginType)
                return m_lstLeftMargin[m_intLeftMarginIndex];
            return 0;
        }

        public long GetTopMargin()
        {
            if (this.GetItemType() == ItemTypeEnum.TopMarginType)
                return m_lstTopMargin[m_intTopMarginIndex];
            return 0;
        }

        public void Clear()
        {
            m_lstItemTypes.Clear();
            m_lstYSet.Clear();
            m_lstTables.Clear();
            m_lstLeftMargin.Clear();
            m_lstTopMargin.Clear();

            m_lngCurrentY = 0;
            m_lstLineTypes.Clear();

            //fontProperty.Clear();
            fontProperty.FontName = "Arial Narrow";
            fontProperty.FontSize = 8;
            fontProperty.FontStyle = 0;

            m_fltX = (float)m_intLeft; //m_marMargin.Left;
            m_fltY = (float)m_intTop; //m_marMargin.Top;

            m_intPageCount = 1;

            m_strHtml.Length = 0; //RDO 04212008
            m_strHtml.Append("<html><body><table>");
        }

        public VSPrinterFontProperty Font
        {
            get { return fontProperty; }
            set { fontProperty = value; }
        }

        public List<VSPrinterTable> Tables
        {
            get { return m_lstTables; }
            set { m_lstTables = value; }
        }

        public void SetCurrentY(long lngCurrentY)
        {
            m_lstYSet.Add(lngCurrentY);
            m_lstItemTypes.Add(ItemTypeEnum.YType);

            m_fltY = (float)lngCurrentY / 14.4F;
        }

        public void SetMarginTop(long lngMarginTop)
        {
            m_lstTopMargin.Add(lngMarginTop);
            m_lstItemTypes.Add(ItemTypeEnum.TopMarginType);

            //m_marMargin.Top = (int)((float) lngMarginTop / 14.4F);
            m_intTop = (int)((float) lngMarginTop / 14.4F);
        }

        public void PageBreak()
        {
            m_lstItemTypes.Add(ItemTypeEnum.PageBreakType);

            m_intPageCount++;

            m_fltX = m_intLeft;
            m_fltY = m_intTop;
        }

        public void SetMarginLeft(long lngMarginLeft)
        {
            m_lstLeftMargin.Add(lngMarginLeft);
            m_lstItemTypes.Add(ItemTypeEnum.LeftMarginType);

            //m_marMargin.Left = (int)((float)lngMarginLeft / 14.4F);
            //m_intLeft = (int)((float)lngMarginLeft / 14.4F);
            m_intLeft = (int)((float)lngMarginLeft / 11.0F);    // RMC 20110831
        }

        public void SetFontName(string strFontName)
        {
            fontProperty.FontName = strFontName;
        }

        public void SetFontSize(int intFontSize)
        {
            fontProperty.FontSize = intFontSize;
        }

        public void SetFontBold(int intStatus)
        {
            if (intStatus == 1)
                fontProperty.FontStyle |= 1;
            else
                fontProperty.FontStyle &= 14;

        }

        //RDO 050508 (s) italic
        public void SetFontItalic(int intStatus)
        {
            if (intStatus == 1)
                fontProperty.FontStyle |= 4;
            else
                fontProperty.FontStyle &= 11;
        }
        //RDO 050508 (e) italic

        public void SetFontUnderline(int intStatus)
        {
            if (intStatus == 1)
                fontProperty.FontStyle |= 8;
            else
                fontProperty.FontStyle &= 7;
        }

        public void SetTable(string strData)
        {

            VSPrinterTable table = new VSPrinterTable();
            //table.Font = fontProperty;
            table.Font = new VSPrinterFontProperty(fontProperty.FontName, fontProperty.FontSize,
                fontProperty.FontStyle);

            /*
            System.Windows.Forms.Control control;
            control = new System.Windows.Forms.Control();
            Graphics g = control.CreateGraphics();
            */

            Font font = this.CreateFont(fontProperty);
            List<string> lstValue = new List<string>();
            string[] strSeparator = { "\n" };
            string strValue = string.Empty;
            float ytmp = m_fltY;
            float fltWidth = 0.0F;
            float fltNewWidth = 0.0F;
            float fltCharWidth = 0.0F;
            SizeF size;
            bool blnForceWordWrap = false;
            float maxY = font.GetHeight();

            bool blnIsWordWrap = false;
            int intAlignment = 0;
            int intWidth = 0;
            if (strData == string.Empty)
                strData = "-1;";
            
            //RDO 012808 (s) fix for multiple semicolons problem with 
            //string[] format = strData.Split(';');
            string[] format = new string[2];
            int intIdx = -1;
            
            intIdx = strData.IndexOf(";"); //get first instance
            if (intIdx != -1)
            {
                format[0] = strData.Substring(0, intIdx);
                format[1] = strData.Substring(intIdx + 1);
                format[1] = format[1].Replace(";", "");
            }
            //if (format.Length >= 2) //allow
            if (intIdx != -1)
            //RDO 012808 (e) fix for multiple semicolons
            {
                format[1] = format[1].Replace("@@SC@@", ";");

                string[] format1 = format[0].Split('|');
                string[] format2 = format[1].Split('|');
                intWidth = 0;

                if (format1.Length != 0 && format1.Length == format2.Length)
                {
                    m_fltX = (float)m_intLeft; //m_marMargin.Left;
                    ytmp = m_fltY;
                    if (m_blnIsHtml)
                        m_strHtml.Append("<tr><td><table><tr>");  //RDO 04212008 html conversion
                    bool blnHasWordWrap = false;

                    for (int i = 0; i < format1.Length; i++)
                    {
                        blnIsWordWrap = false;
                        if (format1[i].Length == 0)
                            intAlignment = 0;
                        else if (format1[i][0] == '>') //right alignment
                            intAlignment = 2;
                        else if (format1[i][0] == '^') //center alignment
                            intAlignment = 1;
                        else if (format1[i][0] == '=') //left wrap
                        {
                            intAlignment = 0;
                            blnIsWordWrap = true;
                        }
                        else //if (format1[i][0] == '~') //left alignment
                            intAlignment = 0;
                        intWidth = 0;
                        if (format1[i].Length == 0)
                            intWidth = 1;
                        else
                            int.TryParse(format1[i].Substring(1), out intWidth);
                        table.AddColumn(new VSPrinterTableItem(intAlignment, intWidth,
                            format2[i], blnIsWordWrap));
                        if (intWidth == -1)
                            intWidth = 999999; //a very large number

                        fltWidth = (float) intWidth / 14.4F;
                        lstValue.Clear();
                        if (intWidth != 0)
                        {
                            string[] strValues = format2[i].Split(strSeparator, StringSplitOptions.None);
                            string[] strTempValues = format2[i].Split(strSeparator, StringSplitOptions.None);
                            blnForceWordWrap = false;
                            if (strValues.Length > 1)
                                blnForceWordWrap = true;
                            else if (blnIsWordWrap)
                            {
                                string[] strTags = { "<B>", "</B>", "<I>", "</I>", "<U>", "</U>" };
                                int[] intTagIndices;
                                int[] intCharIndices;

                                string strStripValue = StringUtilities.StringUtilities.StripString(format2[i],
                                    strTags, out intTagIndices,out  intCharIndices);
                                if (intTagIndices.Length == 0)
                                    strStripValue = format2[i];
                                /*
                                strValues = StringUtilities.StringUtilities.WrapString(format2[i], fltWidth, g, font, true);
                                strTempValues = StringUtilities.StringUtilities.WrapString(format2[i], fltWidth, g, font, false);
                                */
                                strValues = StringUtilities.StringUtilities.WrapString(strStripValue, fltWidth, g, font, true);
                                strTempValues = StringUtilities.StringUtilities.WrapString(strStripValue, fltWidth, g, font, false);
                                blnHasWordWrap = true;

                            }

                            float fltYTmp = 0.0F;
                            for (int l = 0; l < strValues.Length; l++)
                            {
                                strValue = strValues[l];
                                if (m_blnIsHtml)
                                {
                                    m_strHtml.Append("<td style = '");
                                    m_strHtml.Append(string.Format("width:{0}px;", fltWidth));
                                    m_strHtml.Append("'");
                                    if (intAlignment == 1)
                                        m_strHtml.Append(" align='center'");
                                    else if (intAlignment == 2)
                                        m_strHtml.Append(" align='right'");
                                    else
                                        m_strHtml.Append(" align='left'");
                                    m_strHtml.Append(">"); //RDO 04212008 html conversion

                                    if (blnHasWordWrap && l != 0)
                                        m_strHtml.Append("<br>");
                                    
                                    if (strValue.Length == 0)
                                        m_strHtml.Append("&nbsp;");
                                    else
                                        m_strHtml.Append(strValue.Replace(" ", "&nbsp;")); //RDO 04212007 html conversion
                                    
                                }

                                if (blnHasWordWrap)
                                {
                                    maxY = font.GetHeight();
                                    m_fltY += maxY;

                                    if (m_fltY + maxY > m_intMaxY) //m_objPaperSize.Height - m_marMargin.Bottom)
                                    {
                                        m_fltY = (float)m_intTop;//m_marMargin.Top;
                                        //new page
                                        if (m_fltPageHeaderY > 0.0F)
                                            m_fltY = m_fltPageHeaderY;

                                        m_intPageCount++; //RDO 011108 page count
                                        
                                        //break and add table prematurely
                                        StringBuilder strTmpValue1 = new StringBuilder();
                                        StringBuilder strTmpValue2 = new StringBuilder();

                                        //columns
                                        int kk = 0;
                                        for (kk = 0; kk < format1.Length; kk++)
                                        {
                                            if (kk != 0)
                                                strTmpValue2.Append("|");
                                            strTmpValue2.Append(format1[kk]);
                                        }

                                        //previous data
                                        //strTmpValue1.Append(";");
                                        strTmpValue2.Append(";");
                                        for (kk = 0; kk < i; kk++)
                                        {
                                            strTmpValue2.Append("|");
                                        }
                                        //current data
                                        strTmpValue1.Length = 0;
                                        for (kk = 0; kk < l; kk++)
                                            strTmpValue1.Append(string.Format("{0} ", strTempValues[kk]));

                                        for (kk = l; kk < strValues.Length; kk++)
                                            strTmpValue2.Append(string.Format("{0} ", strTempValues[kk]));
                                        for (kk = i + 1; kk < format2.Length; kk++)
                                            strTmpValue2.Append(string.Format("|{0}", format2[kk]));

                                        table.Items[table.Items.Count - 1].Value = strTmpValue1.ToString();
                                        m_lstTables.Add(table);
                                        m_lstItemTypes.Add(ItemTypeEnum.TableType);
                                        this.SetTable(strTmpValue2.ToString());
                                        return;
                                    }
                                }
                                else
                                {
                                    fltYTmp += font.GetHeight();
                                }

                                
                                /*
                                float fltYTmp = 0.0F;
                                for (int k = 0; k < 1; k++)
                                {
                                    fltYTmp += font.GetHeight();
                                    ytmp += font.GetHeight();
                                }

                                m_fltX += fltWidth;

                                if (fltYTmp > maxY)
                                    maxY = fltYTmp;
                                else if (font.GetHeight() > maxY)
                                    maxY = font.GetHeight();

                                m_fltY += maxY;

                                if (m_fltY + maxY > m_intMaxY) //m_objPaperSize.Height - m_marMargin.Bottom)
                                {
                                    m_fltY = (float)m_intTop;//m_marMargin.Top;
                                    //new page
                                    if (m_fltPageHeaderY > 0.0F)
                                        m_fltY = m_fltPageHeaderY;

                                    m_intPageCount++; //RDO 011108 page count
                                }
                                */


                            }
                            if (m_blnIsHtml)
                                m_strHtml.Append("</td>"); //RDO 04212008 html conversion

                            if (!blnHasWordWrap)
                            {
                                if (fltYTmp > maxY)
                                    maxY = fltYTmp;
                                else if (font.GetHeight() > maxY)
                                    maxY = font.GetHeight();

                            }

                            m_fltX += fltWidth;

                            /*
                            string strTmpValue = string.Empty;


                            for (int l = 0; l < strValues.Length; l++)
                            {
                                strValue = strValues[l];
                                if (strValue.Length > 1 && strValue[strValue.Length - 1] == ' ')
                                    strValue = string.Format("{0}\0", strValue);

                                if (m_blnIsHtml)
                                {
                                    m_strHtml.Append("<td style = '");
                                    m_strHtml.Append(string.Format("width:{0}px;", fltWidth));
                                    m_strHtml.Append("'");
                                    if (intAlignment == 1)
                                        m_strHtml.Append(" align='center'");
                                    else if (intAlignment == 2)
                                        m_strHtml.Append(" align='right'");
                                    else
                                        m_strHtml.Append(" align='left'");
                                    m_strHtml.Append(">"); //RDO 04212008 html conversion
                                    if (strValue.Length == 0)
                                        m_strHtml.Append("&nbsp;");
                                }
                                while (strValue.Length != 0)
                                {
                                    
                                        size = g.MeasureString(strValue, font);
                                        if (strValue == " ")
                                            break;
                                        if (size.Width > fltWidth)
                                        {
                                            fltCharWidth = size.Width / strValue.Length;
                                            fltNewWidth = 0.0F;
                                            int k = 0;
                                            while ((fltNewWidth + fltCharWidth) < fltWidth)
                                            {
                                                fltNewWidth += fltCharWidth;
                                                k++;
                                            }

                                            strTmpValue = strValue.Substring(0, k);
                                            if (strTmpValue.Length > 1 && strTmpValue[strTmpValue.Length - 1] == ' ')
                                                strTmpValue = string.Format("{0}\0", strTmpValue);

                                            size = g.MeasureString(strTmpValue, font);
                                            bool blnIsAdjust = false;
                                            while (size.Width > fltWidth && k > 0)
                                            {
                                                blnIsAdjust = true;
                                                k--;
                                                strTmpValue = strValue.Substring(0, k);
                                                if (strTmpValue.Length > 1 && strTmpValue[strTmpValue.Length - 1] == ' ')
                                                    strTmpValue = string.Format("{0}\0", strTmpValue);

                                                size = g.MeasureString(strTmpValue, font);
                                            }
                                            if (blnIsAdjust)
                                            {
                                                k++;
                                            }
                                            else
                                            {
                                                while (size.Width < fltWidth && k < strTmpValue.Length)
                                                {
                                                    k++;
                                                    strTmpValue = strValue.Substring(0, k);
                                                    if (strTmpValue.Length > 1 && strTmpValue[strTmpValue.Length - 1] == ' ')
                                                        strTmpValue = string.Format("{0}\0", strTmpValue);
                                                    size = g.MeasureString(strTmpValue, font);
                                                }
                                            }
                                            strTmpValue = strValue.Substring(0, k);
                                            lstValue.Add(strTmpValue);
                                        if (m_blnIsHtml)
                                            m_strHtml.Append(strValue.Substring(0, k).Replace(" ", "&nbsp;")); //RDO 04212007 html conversion
                                        strValue = strValue.Substring(k);

                                    }
                                    else
                                    {
                                        if (m_blnIsHtml)
                                            m_strHtml.Append(strValue.Replace(" ", "&nbsp;")); //RDO 04212007 html conversion
                                        lstValue.Add(strValue);
                                        strValue = string.Empty;
                                    }
                                }

                                if (m_blnIsHtml)
                                    m_strHtml.Append("</td>"); //RDO 04212008 html conversion

                                int intValueCount = lstValue.Count;
                                if (!blnIsWordWrap && !blnForceWordWrap && intValueCount != 0)
                                    intValueCount = 1;

                                float fltYTmp = 0.0F;
                                for (int k = 0; k < intValueCount; k++)
                                {
                                    fltYTmp += font.GetHeight();
                                    ytmp += font.GetHeight();
                                }

                                m_fltX += fltWidth;

                                if (fltYTmp > maxY)
                                    maxY = fltYTmp;
                                else if (font.GetHeight() > maxY)
                                    maxY = font.GetHeight();
                            }
                            */

                        }
                    }
                    
                    if (m_blnIsHtml)
                        m_strHtml.Append("</tr></table></td></tr>"); //RDO 04212008 html conversion

                    if (!blnHasWordWrap)
                    {
                        m_fltY += maxY;

                        if (m_fltY + maxY > m_intMaxY) //m_objPaperSize.Height - m_marMargin.Bottom)
                        {
                            m_fltY = (float)m_intTop;//m_marMargin.Top;
                            //new page
                            if (m_fltPageHeaderY > 0.0F)
                                m_fltY = m_fltPageHeaderY;

                            m_intPageCount++; //RDO 011108 page count
                        }
                    }

                    m_lstTables.Add(table);
                    m_lstItemTypes.Add(ItemTypeEnum.TableType);


                }
                else
                {
                    throw new Exception();
                }
            
            }
        }

        public void DrawLine(long lngX1, long lngY1, long lngX2, long lngY2)
        {
            m_lstLineTypes.Add(new Rectangle((int)lngX1,(int)lngY1, (int) (lngX2 - lngX1 + 1), (int) (lngY2 - lngY1 + 1))); 
            m_lstItemTypes.Add(ItemTypeEnum.LineType);
        }

        public int GetCoordinate(long lngCoordinate)
        {
            /*
            if (lngCoordinate >= 999999)
            {
                return (int) (m_lngCurrentY + (lngCoordinate - 999999)); //return offset
            }
            */
            return (int) lngCoordinate ;
        }

//Border Layout
//0 - nothing
//1 - bottom
//2 - top
//3 - top and bottom
//4 - box
//5 - left and right
//7 - top, left, and right (?)
//9 - box
//custom
//11 - top, left, and right
//12 - bottom, left, and right

        public void SetTableBorder(int intBorderType)
        {
            m_lstBorderTypes.Add(intBorderType);
            m_lstItemTypes.Add(ItemTypeEnum.BorderType);
        }

    /// <summary>
        /// Border Layout
        ///0 - nothing
        ///1 - top
        ///2 - bottom
        ///3 - top and bottom
        ///4 - box
        ///5 - left and right
        ///7 - top, left, and right (?)
        ///9 - box
        ///custom
        ///11 - top, left, and right
        ///12 - bottom, left, and right
    /// </summary>
        public int BorderType
        {
            get 
            {
                if (m_intBorderTypeIndex + 1 >= m_lstBorderTypes.Count)
                    return 0;
                m_intBorderTypeIndex++;
                return m_lstBorderTypes[m_intBorderTypeIndex];
            }
        }

        public void SetTextAlign(int intAlignment)
        {
            m_intAlignment = intAlignment;
        }

        public void SetParagraph(string strParagraph)
        {
            VSPrinterTable table = new VSPrinterTable();
            table.Font = new VSPrinterFontProperty(fontProperty.FontName,
                fontProperty.FontSize, fontProperty.FontStyle); //fontProperty;
            table.AddColumn(new VSPrinterTableItem(m_intAlignment, -1, strParagraph));
            m_lstTables.Add(table);
            m_lstItemTypes.Add(ItemTypeEnum.TableType);
        }

    }
}
