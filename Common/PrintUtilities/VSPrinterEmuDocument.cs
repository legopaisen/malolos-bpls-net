using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using Amellar.Common.StringUtilities;

namespace Amellar.Common.PrintUtilities
{

    //@author R.D.Ong
    public class VSPrinterEmuDocument:PrintDocument
    {
        //additional features
        //for image only
        private string m_strImagePath;
        //private bool m_blnMarginImage; //determines if image is placed on 0,0 or start of margin
        private bool m_blnStretchImage;

        private VSPrinterEmuModel m_objModel;

        private VSPrinterEmuModel m_objPageHeaderModel; //test only

        /*
        private float m_fltX;
        private float m_fltY;
        */
        private bool m_blnIsForceOnePage;
        private bool m_blnRunOnce = true;


        private int m_intPageNumber; //RDO 01112008 Page Numbering

        private bool m_blnIsNullCharAllowed; //RDO 05308 


        private bool m_blnIsHeaderOnFirstPage; //shows header on first page
        
        private string[] m_strTags = { "<B>", "</B>", "<I>", "</I>", "<U>", "</U>" };

        public VSPrinterEmuDocument()
        {
            m_strImagePath = string.Empty;
            m_blnStretchImage = false;
            m_blnIsForceOnePage = false;
            m_blnRunOnce = true;
            m_intPageNumber = 1;
            m_blnIsNullCharAllowed = false;
            m_blnIsHeaderOnFirstPage = false; 
        }

        /// <summary>
        /// This property sets if header is displayed on first page.
        /// </summary>
        public bool IsHeaderOnFirstPage
        {
            set { m_blnIsHeaderOnFirstPage = value; }
        }

        public bool AllowNullCharacter
        {
            set { m_blnIsNullCharAllowed = value; }
        }


        public bool ForceOnePage
        {
            set { m_blnIsForceOnePage = value; }
        }

        public void SetImagePath(string strImagePath, bool blnStretch)
        {
            if (System.IO.File.Exists(strImagePath))
            {
                m_strImagePath = strImagePath;
                m_blnStretchImage = blnStretch;
                
            }
            else
            {
                m_strImagePath = string.Empty;
            }
            
        }
        //based on margin, on 

        public VSPrinterEmuModel Model
        {
            get { return m_objModel; }
            set { m_objModel = value; }
        }


        public VSPrinterEmuModel PageHeaderModel
        {
            get { return m_objPageHeaderModel; }
            set { m_objPageHeaderModel = value; }
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

        public void Render(PrintPageEventArgs e)
        {
            bool blnRunOnce = false;

            if (m_blnRunOnce)
            {
                blnRunOnce = true;

                m_objModel.Reset();
                this.DefaultPageSettings.Margins.Left = m_objModel.LeftMargin;
                this.DefaultPageSettings.Margins.Top = m_objModel.TopMargin;
                m_blnRunOnce = false;
                m_intPageNumber = 1; //RDO 011208 page numbering fix
            }

            //RDO 061008 (s) 
            if (m_blnIsHeaderOnFirstPage && blnRunOnce)
                blnRunOnce = false;
            //RDO 061008 (e)

            if (m_strImagePath != string.Empty)
            {
                if (m_blnStretchImage)
                {
                    e.Graphics.DrawImage(Image.FromFile(m_strImagePath), e.Graphics.VisibleClipBounds);
                }
                else
                {
                    e.Graphics.DrawImage(Image.FromFile(m_strImagePath), e.Graphics.VisibleClipBounds.X, e.Graphics.VisibleClipBounds.Y);
                }
            }

            int intTableCount = m_objModel.Tables.Count;
            int intColumnCount = 0;
            
            Font font = this.CreateFont(m_objModel.Font);

            float x = this.DefaultPageSettings.Margins.Left; //e.MarginBounds.Left;
            float y = this.DefaultPageSettings.Margins.Top; //e.MarginBounds.Top;
            float ytmp = y;

            float maxY = font.GetHeight();

            List<string> lstValue = new List<string>();
            float fltWidth = 0.0F;
            float fltNewWidth = 0.0F;
            float fltCharWidth = 0.0F;
            SizeF size;
            string strValue = string.Empty;
            string strValueTmp = string.Empty;
            string[] strSeparator = { "\n" };
            bool blnForceWordWrap = false;

            int intBorderType = 0;


            if (!blnRunOnce && m_objPageHeaderModel != null) //process page headers if any (not optimized)
            {
                m_objPageHeaderModel.Reset();
                while (m_objPageHeaderModel.Next())
                {
                    if (m_objPageHeaderModel.GetItemType() == VSPrinterEmuModel.ItemTypeEnum.YType)
                    {
                        y = (float)m_objPageHeaderModel.GetYSet() / 14.4F;
                    }
                    else if (m_objPageHeaderModel.GetItemType() == VSPrinterEmuModel.ItemTypeEnum.LeftMarginType)
                    {
                        this.DefaultPageSettings.Margins.Left = (int)((float)m_objPageHeaderModel.GetLeftMargin() / 14.4F);
                    }
                    else if (m_objPageHeaderModel.GetItemType() == VSPrinterEmuModel.ItemTypeEnum.TopMarginType)
                    {
                        this.DefaultPageSettings.Margins.Top = (int)((float)m_objPageHeaderModel.GetTopMargin() / 14.4F);
                    }
                    //RDO 120707 (s)
                    /*
                                    else if (m_objPageHeaderModel.GetItemType() == VSPrinterEmuModel.ItemTypeEnum.GetYType) //temporary implementation
                                    {
                                        m_objPageHeaderModel.ActualCurrentY = (long) (y * 14.4F);
                                    }
                    */
                    else if (m_objPageHeaderModel.GetItemType() == VSPrinterEmuModel.ItemTypeEnum.LineType)
                    {
                        Rectangle rect = m_objPageHeaderModel.GetLine();
                        e.Graphics.DrawLine(Pens.Black, (float)m_objPageHeaderModel.GetCoordinate(rect.Left) / 14.4F,
                            (float)m_objPageHeaderModel.GetCoordinate(rect.Top) / 14.4F,
                            (float)m_objPageHeaderModel.GetCoordinate(rect.Right) / 14.4F,
                            (float)m_objPageHeaderModel.GetCoordinate(rect.Bottom) / 14.4F);
                    }
                    else if (m_objPageHeaderModel.GetItemType() == VSPrinterEmuModel.ItemTypeEnum.BorderType)
                    {
                        intBorderType = m_objPageHeaderModel.BorderType;
                    }
                    else if (m_objPageHeaderModel.GetItemType() == VSPrinterEmuModel.ItemTypeEnum.PageBreakType)
                    {
                        y = e.MarginBounds.Top;
                        e.HasMorePages = true;
                        return;
                    }
                    //RDO 120707 (e)
                    else if (m_objPageHeaderModel.GetItemType() == VSPrinterEmuModel.ItemTypeEnum.TableType)
                    {
                        //for border style
                        List<RectangleF> m_lstBorder = new List<RectangleF>();
                        m_lstBorder.Clear();

                        VSPrinterTable table = m_objPageHeaderModel.GetTable();
                        Font fontTable = font;
                        if (!table.Font.IsEmpty)
                        {
                            fontTable = this.CreateFont(table.Font);
                        }
                        intColumnCount = table.Items.Count;
                        x = this.DefaultPageSettings.Margins.Left; //e.MarginBounds.Left;
                        maxY = 0.0F;
                        for (int j = 0; j < intColumnCount; j++)
                        {
                            Font fontColumn = fontTable;
                            if (!table.Items[j].Font.IsEmpty)
                            {
                                fontColumn = this.CreateFont(table.Items[j].Font);
                            }
                            //fltWidth = m_objPageHeaderModel.Tables[i].Items[j].Width * 0.025F;
                            int intWidth = table.Items[j].Width;
                            if (intWidth == -1)
                                fltWidth = (float)e.PageBounds.Width;
                            else
                                fltWidth = intWidth / 14.4F;

                            lstValue.Clear();
                            if (table.Items[j].Width == 0)
                            {
                                lstValue.Add(table.Items[j].Value);
                            }
                            else
                            {
                                strValueTmp = table.Items[j].Value;

                                //RDO 011108 (s) page reserved words
                                strValueTmp = strValueTmp.Replace("@@PAGENUM@@", string.Format("{0}", m_intPageNumber));
                                strValueTmp = strValueTmp.Replace("@@PAGECOUNT@@", string.Format("{0}", m_objPageHeaderModel.PageCount));
                                strValueTmp = strValueTmp.Replace("@@PAGECOUNTW@@", NumberWording.ToCardinal(m_objPageHeaderModel.PageCount).ToLower());
                                //RDO 011108 (e) page reserved words
                                
                                ytmp = y;

                                if (intBorderType != 0 && strValueTmp != string.Empty)
                                    m_lstBorder.Add(new RectangleF(x, y, fltWidth, 0.0F));

                                string[] strValues = strValueTmp.Split(strSeparator, StringSplitOptions.None);
                                blnForceWordWrap = false;
                                if (strValues.Length > 1)
                                    blnForceWordWrap = true;

                                for (int l = 0; l < strValues.Length; l++)
                                {
                                    strValue = strValues[l];

                                    while (strValue.Length != 0)
                                    {
                                        size = e.Graphics.MeasureString(strValue, fontColumn);
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
                                            size = e.Graphics.MeasureString(strValue.Substring(0, k), fontColumn);
                                            while (size.Width < fltWidth && k < strValue.Length)
                                            {
                                                k++;
                                                size = e.Graphics.MeasureString(strValue.Substring(0, k), fontColumn);
                                            }

                                            lstValue.Add(strValue.Substring(0, k));
                                            strValue = strValue.Substring(k);

                                        }
                                        else
                                        {
                                            lstValue.Add(strValue);
                                            strValue = string.Empty;
                                        }
                                    }
                                }
                            }

                            int intValueCount = lstValue.Count;
                            if (!table.Items[j].WordWrap && !blnForceWordWrap && intValueCount != 0)
                                intValueCount = 1;

                            float fltYTmp = 0.0F;
                            for (int k = 0; k < intValueCount; k++)
                            {
                                //if (k == intValueCount - 1)
                                {
                                    size = e.Graphics.MeasureString(lstValue[k], fontColumn);
                                    float fltExcessWidth = 0.0F;
                                    if (table.Items[j].Alignment == 1)
                                        fltExcessWidth = (fltWidth - size.Width) / 2.0F;
                                    else if (table.Items[j].Alignment == 2)
                                        fltExcessWidth = fltWidth - size.Width;
                                    if (!m_blnIsNullCharAllowed)
                                        lstValue[k] = lstValue[k].Replace("\0", ""); //RDO 051608
                                    
                                    e.Graphics.DrawString(lstValue[k], fontColumn, Brushes.Black, x + fltExcessWidth, ytmp);

                                }
                                /*
                                else
                                {
                                    e.Graphics.DrawString(lstValue[k], fontColumn, Brushes.Black, x, ytmp);
                                 }
                                */
                                fltYTmp += fontColumn.GetHeight();

                                ytmp += fontColumn.GetHeight();
                            }
                            //fltY2 += ytmp;

                            x += fltWidth;

                            if (fltYTmp > maxY)
                                maxY = fltYTmp;
                            else if (fontColumn.GetHeight() > maxY)
                                maxY = fontColumn.GetHeight();

                        }
                        y += maxY;

                        for (int u = 0; u < m_lstBorder.Count; u++)
                        {
                            if (intBorderType == 7 || intBorderType == 5 || intBorderType == 9 || intBorderType == 11 || intBorderType == 12)
                                e.Graphics.DrawLine(Pens.Black, m_lstBorder[u].X, m_lstBorder[u].Y, m_lstBorder[u].X, m_lstBorder[u].Y + maxY);
                            if (intBorderType == 7 || intBorderType == 5 || intBorderType == 9 || intBorderType == 11 || intBorderType == 12)
                                e.Graphics.DrawLine(Pens.Black, m_lstBorder[u].X + m_lstBorder[u].Width, m_lstBorder[u].Y, m_lstBorder[u].X + m_lstBorder[u].Width, m_lstBorder[u].Y + maxY);
                            if (intBorderType == 1 || intBorderType == 7 || intBorderType == 3 || intBorderType == 9 || intBorderType == 11)
                                e.Graphics.DrawLine(Pens.Black, m_lstBorder[u].X, m_lstBorder[u].Y, m_lstBorder[u].X + m_lstBorder[u].Width, m_lstBorder[u].Y);
                            if (intBorderType == 2 || intBorderType == 3 || intBorderType == 9 || intBorderType == 12)
                                e.Graphics.DrawLine(Pens.Black, m_lstBorder[u].X, m_lstBorder[u].Y + maxY, m_lstBorder[u].X + m_lstBorder[u].Width, m_lstBorder[u].Y + maxY);
                        }
                    }
                }
            }


            while (m_objModel.Next())
            {
                if (m_objModel.GetItemType() == VSPrinterEmuModel.ItemTypeEnum.YType)
                {
                    y = (float) m_objModel.GetYSet() / 14.4F;
                }
                else if (m_objModel.GetItemType() == VSPrinterEmuModel.ItemTypeEnum.LeftMarginType)
                {
                    this.DefaultPageSettings.Margins.Left = (int) ((float) m_objModel.GetLeftMargin() / 14.4F);
                }
                else if (m_objModel.GetItemType() == VSPrinterEmuModel.ItemTypeEnum.TopMarginType)
                {
                    this.DefaultPageSettings.Margins.Top = (int) ( (float) m_objModel.GetTopMargin() / 14.4F);
                }
                //RDO 120707 (s)
/*
                else if (m_objModel.GetItemType() == VSPrinterEmuModel.ItemTypeEnum.GetYType) //temporary implementation
                {
                    m_objModel.ActualCurrentY = (long) (y * 14.4F);
                }
*/
                else if (m_objModel.GetItemType() == VSPrinterEmuModel.ItemTypeEnum.LineType)
                {
                    Rectangle rect = m_objModel.GetLine();
                    e.Graphics.DrawLine(Pens.Black, (float) m_objModel.GetCoordinate(rect.Left) / 14.4F,
                        (float) m_objModel.GetCoordinate(rect.Top) / 14.4F, 
                        (float) m_objModel.GetCoordinate(rect.Right) / 14.4F,
                        (float) m_objModel.GetCoordinate(rect.Bottom) / 14.4F);
                }
                else if (m_objModel.GetItemType() == VSPrinterEmuModel.ItemTypeEnum.BorderType)
                {
                    intBorderType = m_objModel.BorderType;
                }
                else if (m_objModel.GetItemType() == VSPrinterEmuModel.ItemTypeEnum.PageBreakType)
                {
                    y = e.MarginBounds.Top;
                    e.HasMorePages = true;
                    return;
                }
                //RDO 120707 (e)
                else if (m_objModel.GetItemType() == VSPrinterEmuModel.ItemTypeEnum.TableType)
                {
                    //for border style
                    List<RectangleF> m_lstBorder = new List<RectangleF>();
                    m_lstBorder.Clear();

                    VSPrinterTable table = m_objModel.GetTable();
                    Font fontTable = font;
                    if (!table.Font.IsEmpty)
                    {
                        fontTable = this.CreateFont(table.Font);
                    }
                    intColumnCount = table.Items.Count;
                    x = this.DefaultPageSettings.Margins.Left; //e.MarginBounds.Left;
                    maxY = 0.0F;

                    //bool blnIsWordWrap = false;
                        
                    for (int j = 0; j < intColumnCount; j++)
                    {
                        Font fontColumn = fontTable;
                        if (!table.Items[j].Font.IsEmpty)
                        {
                            fontColumn = this.CreateFont(table.Items[j].Font);
                        }
                        //fltWidth = m_objModel.Tables[i].Items[j].Width * 0.025F;
                        int intWidth = table.Items[j].Width;
                        if (intWidth == -1)
                            fltWidth = (float)e.PageBounds.Width;
                        else
                            fltWidth = intWidth / 14.4F;


                        lstValue.Clear();
                        if (table.Items[j].Width == 0)
                        {
                            lstValue.Add(table.Items[j].Value);
                        }
                        else
                        {
                            strValueTmp = table.Items[j].Value;
                            
                            //RDO 011108 (s) page reserved words
                            strValueTmp = strValueTmp.Replace("@@PAGENUM@@", string.Format("{0}", m_intPageNumber));
                            strValueTmp = strValueTmp.Replace("@@PAGECOUNT@@", string.Format("{0}", m_objModel.PageCount));
                            strValueTmp = strValueTmp.Replace("@@PAGECOUNTW@@", NumberWording.ToCardinal(m_objModel.PageCount).ToLower());
                            //RDO 011108 (e) page reserved words

                            ytmp = y;

                            if (intBorderType != 0 && strValueTmp != string.Empty)
                                m_lstBorder.Add(new RectangleF(x, y, fltWidth, 0.0F));

                            string[] strValues = strValueTmp.Split(strSeparator, StringSplitOptions.None);
                            blnForceWordWrap = false;

                            
                             
                            int[] intTagIndices;
                            int[] intCharIndices;
                            

                            if (strValues.Length > 1)
                                blnForceWordWrap = true;
                            else if (table.Items[j].WordWrap)
                            {
                                //fix for inconsistent wrapping if graphics is based on e.Graphics
                                using (Graphics g = (new System.Windows.Forms.Control()).CreateGraphics())
                                {
                                    string strStripValue = StringUtilities.StringUtilities.StripString(strValueTmp,
                                        m_strTags, out intTagIndices, out intCharIndices);

                                    /*
                                    strValues = StringUtilities.StringUtilities.WrapString(strValueTmp, fltWidth,
                                        g, font, true);
                                    */
                                    string[] strWrapValues = StringUtilities.StringUtilities.WrapString(strStripValue, fltWidth,
                                        g, font, true);

                                    strValues  = StringUtilities.StringUtilities.UnstripFlushString(strValueTmp, 
                                        strWrapValues, m_strTags);
                                }
                            }
                            string strTmpValue = string.Empty;

                            for (int l = 0; l < strValues.Length; l++)
                            {
                                strValue = strValues[l];
                                if (m_blnIsNullCharAllowed && strValue.Length > 1 && strValue[strValue.Length - 1] == ' ')
                                    strValue = string.Format("{0}\0", strValue);
                                
                                lstValue.Add(strValue);

                                /*
                                while (strValue.Length != 0)
                                {
                                    size = e.Graphics.MeasureString(strValue, fontColumn);
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
                                        if (k == strValue.Length - 1) //allow last missing character to be displayed
                                            k++;
                                        
                                        strTmpValue = strValue.Substring(0, k);

                                        if (strValue.Length > k && strValue[k] != ' ')
                                        {
                                            //look up for 
                                            if (strTmpValue.LastIndexOf(' ') != -1)
                                            {
                                                //validate if left and right entry is empty
                                                if (strValue.Substring(0, strTmpValue.LastIndexOf(' ')).Trim() != string.Empty &&
                                                    strValue.Substring(strTmpValue.LastIndexOf(' ')).Trim() != string.Empty)
                                                {
                                                    k = strTmpValue.LastIndexOf(' ');
                                                    strTmpValue = strValue.Substring(0, k);
                                                }
                                            }

                                        }

                                        if (m_blnIsNullCharAllowed && strTmpValue.Length > 1 && 
                                            strTmpValue[strTmpValue.Length - 1] == ' ')
                                            strTmpValue = string.Format("{0}\0", strTmpValue);

                                        size = e.Graphics.MeasureString(strTmpValue, fontColumn);
                                        bool blnIsAdjust = false;
                                        while (size.Width > fltWidth && k > 0)
                                        {
                                            blnIsAdjust = true;
                                            k--;
                                            strTmpValue = strValue.Substring(0, k);
                                            if (strTmpValue.Length > 1 && strTmpValue[strTmpValue.Length - 1] == ' ')
                                                strTmpValue = string.Format("{0}\0", strTmpValue);

                                            size = e.Graphics.MeasureString(strTmpValue, fontColumn);
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
                                                if (m_blnIsNullCharAllowed && strTmpValue.Length > 1 && 
                                                    strTmpValue[strTmpValue.Length - 1] == ' ')
                                                    strTmpValue = string.Format("{0}\0", strTmpValue);

                                                size = e.Graphics.MeasureString(strTmpValue, fontColumn);
                                            }
                                        }

                                        strTmpValue = strValue.Substring(0, k);
                                        lstValue.Add(strTmpValue);
                                        strValue = strValue.Substring(k);

                                    }
                                    else
                                    {
                                        lstValue.Add(strValue);
                                        strValue = string.Empty;
                                    }
                                }
                                */
                            }
                            

                        }

                        

                        int intValueCount = lstValue.Count;
                        if (!table.Items[j].WordWrap && !blnForceWordWrap && intValueCount != 0)
                            intValueCount = 1;

                        float fltYTmp = 0.0F;

                        for (int k = 0; k < intValueCount; k++)
                        {
                            //if (k == intValueCount - 1)
                            {
                                string strTmpValue = string.Empty;
                                strTmpValue = lstValue[k];
                                if (strTmpValue == null)
                                    strTmpValue = string.Empty;
                                else if (strTmpValue.Length > 1 && strTmpValue[strTmpValue.Length - 1] == ' ')
                                    strTmpValue = string.Format("{0}\0", lstValue[k]);

                                size = e.Graphics.MeasureString(strTmpValue, fontColumn);
                                float fltExcessWidth = 0.0F;
                                if (table.Items[j].Alignment == 1)
                                    fltExcessWidth = (fltWidth - size.Width) / 2.0F;
                                else if (table.Items[j].Alignment == 2)
                                    fltExcessWidth = fltWidth - size.Width;
                                
                                if (!m_blnIsNullCharAllowed)
                                    strTmpValue = strTmpValue.Replace("\0", ""); 
                                
                                
                                int[] intTagIndices;
                                int[] intCharIndices;
                                
                                StringUtilities.StringUtilities.StripString(strTmpValue, m_strTags, out intTagIndices,
                                    out intCharIndices);
                                if (intTagIndices.Length > 0)
                                {
                                    int intLastTmpIdx = -1;
                                    List<string> lstTmpValues = new List<string>();
                                    for (int ww = 0; ww < intTagIndices.Length; ww++)
                                    {
                                        if (ww == 0)
                                        {
                                            lstTmpValues.Add(strTmpValue.Substring(0,
                                                intCharIndices[ww]));
                                            intLastTmpIdx = lstTmpValues.Count - 1;
                                            lstTmpValues.Add(m_strTags[intTagIndices[ww]]);
                                        }
                                        else if (ww == intTagIndices.Length - 1)
                                        {
                                            lstTmpValues.Add(m_strTags[intTagIndices[ww]]);
                                            lstTmpValues.Add(strTmpValue.Substring(
                                                intCharIndices[ww] + m_strTags[intTagIndices[ww]].Length));
                                            intLastTmpIdx = lstTmpValues.Count - 1;
                                        }
                                        else
                                        {
                                            int intTmpX = intCharIndices[ww - 1] + m_strTags[intTagIndices[ww - 1]].Length;
                                            string strTmpX = string.Empty;
                                            strTmpX = strTmpValue.Substring(intTmpX, intCharIndices[ww] - intTmpX);
                                            if (strTmpX != string.Empty)
                                            {
                                                lstTmpValues.Add(strTmpX);
                                                intLastTmpIdx = lstTmpValues.Count - 1;
                                            }
                                            lstTmpValues.Add(m_strTags[intTagIndices[ww]]);
                                        }
                                    }
                                    float xxtmp = x;
                                    Font tmpFont = this.CreateFont(table.Font);
                                    int intFontStyle = table.Font.FontStyle;
                                    for (int ww = 0; ww < lstTmpValues.Count; ww++)
                                    {
                                        if (ww == intLastTmpIdx)
                                            xxtmp += fltExcessWidth;
                                        bool blnIsFound = false;
                                        for (int vv = 0; vv < m_strTags.Length; vv++)
                                        {
                                            if (lstTmpValues[ww] == m_strTags[vv])
                                            {
                                                blnIsFound = true;
                                                break;
                                            }
                                        }
                                        if (blnIsFound)
                                        {
                                            if (lstTmpValues[ww] == "<B>")
                                            {
                                                intFontStyle |= 1;
                                                tmpFont = this.CreateFont(new VSPrinterFontProperty(table.Font.FontName,
                                                    table.Font.FontSize, intFontStyle));
                                            }
                                            else if (lstTmpValues[ww] == "<U>")
                                            {
                                                intFontStyle |= 8;
                                                tmpFont = this.CreateFont(new VSPrinterFontProperty(table.Font.FontName,
                                                    table.Font.FontSize, intFontStyle));
                                            }
                                            else if (lstTmpValues[ww] == "</B>")
                                            {
                                                intFontStyle &= 14;
                                                tmpFont = this.CreateFont(new VSPrinterFontProperty(table.Font.FontName,
                                                    table.Font.FontSize, intFontStyle));
                                            }
                                            else if (lstTmpValues[ww] == "</U>")
                                            {
                                                intFontStyle &= 7;
                                                tmpFont = this.CreateFont(new VSPrinterFontProperty(table.Font.FontName,
                                                    table.Font.FontSize, intFontStyle));
                                            }
                                        }
                                        else
                                        {
                                            SizeF tmpSize = e.Graphics.MeasureString(lstTmpValues[ww], tmpFont);
                                            e.Graphics.DrawString(lstTmpValues[ww], tmpFont, Brushes.Black, xxtmp, ytmp);
                                            xxtmp += tmpSize.Width;
                                        }
                                    }
                                }
                                else
                                {

                                    e.Graphics.DrawString(strTmpValue, fontColumn, Brushes.Black, x + fltExcessWidth, ytmp);
                                }
                                
                                //e.Graphics.DrawString(strTmpValue, fontColumn, Brushes.Black, x + fltExcessWidth, ytmp);
                            }
                            //else
                            //{
                            //    e.Graphics.DrawString(lstValue[k], fontColumn, Brushes.Black, x, ytmp);
                            // }
                            
                            fltYTmp += fontColumn.GetHeight();

                            ytmp += fontColumn.GetHeight();
                        }
                        //fltY2 += ytmp;

                        x += fltWidth;

                        if (fltYTmp > maxY)
                            maxY = fltYTmp;
                        else if (fontColumn.GetHeight() > maxY)
                            maxY = fontColumn.GetHeight();
                    }

                    y += maxY;

                    for (int u = 0; u < m_lstBorder.Count; u++)
                    {
                        if (intBorderType == 7 || intBorderType == 5 || intBorderType == 9 || intBorderType == 11 || intBorderType == 12)
                            e.Graphics.DrawLine(Pens.Black, m_lstBorder[u].X, m_lstBorder[u].Y, m_lstBorder[u].X, m_lstBorder[u].Y + maxY);
                        if (intBorderType == 7 || intBorderType == 5 || intBorderType == 9 || intBorderType == 11 || intBorderType == 12)
                            e.Graphics.DrawLine(Pens.Black, m_lstBorder[u].X + m_lstBorder[u].Width, m_lstBorder[u].Y, m_lstBorder[u].X + m_lstBorder[u].Width, m_lstBorder[u].Y + maxY);
                        if (intBorderType == 1 || intBorderType == 7 || intBorderType == 3 || intBorderType == 9 || intBorderType == 11)
                            e.Graphics.DrawLine(Pens.Black, m_lstBorder[u].X, m_lstBorder[u].Y, m_lstBorder[u].X + m_lstBorder[u].Width, m_lstBorder[u].Y);
                        if (intBorderType == 2 || intBorderType == 3 || intBorderType == 9 || intBorderType == 12)
                            e.Graphics.DrawLine(Pens.Black, m_lstBorder[u].X, m_lstBorder[u].Y + maxY, m_lstBorder[u].X + m_lstBorder[u].Width, m_lstBorder[u].Y + maxY);
                    }


                    //if (y > e.MarginBounds.Bottom )
                    if (y + maxY > e.MarginBounds.Bottom)
                    {
                        if (m_blnIsForceOnePage)
                        {
                            e.HasMorePages = false;
                            m_blnRunOnce = true;
                            return;
                        }
                        y = e.MarginBounds.Top;
                        e.HasMorePages = true;
                        m_intPageNumber++; //RDO 01112008 Page Numbering
                        return;
                    }
                }

            }
            m_blnRunOnce = true;
            e.HasMorePages = false;
        }

    }
}
