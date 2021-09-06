using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Amellar.Common.StringUtilities
{
    public static class StringUtilities
    {

        //@author R.D.Ong

        public static string RemoveDoubleSpaces(string strValue)
        {
            string strNewValue = strValue;
            while (strNewValue.IndexOf("  ") != -1)
            {
                strNewValue = strNewValue.Replace("  ", " ");
            }
            return strNewValue;
        }

        /*
        public static string Cut(string strValue, int intStartIndex, int intEndIndex)
        {

        }
        */

        public static string[] UnstripFlushString(string strValue, string[] strValues,
            string[] strTags)
        {
            List<string> strWords = new List<string>();
            string strTmpValue = StringUtilities.RemoveDoubleSpaces(strValue).Trim();
            int intIndex = 0;
            int intTmpIndex1 = 0;
            int intTmpIndex2 = 0;
            int intTagIndex = 0;
            string strTmpValue2 = string.Empty;
            while (intTmpIndex1 != -1)
            {
                intTagIndex = -1;
                intTmpIndex1 = -1;
                for (int i = 0; i < strTags.Length; i++)
                {
                    intTmpIndex2 = strTmpValue.IndexOf(strTags[i], intIndex);
                    if (intTmpIndex2 == -1)
                    {
                    }
                    else if (intTmpIndex1 == -1 || (intTmpIndex2 < intTmpIndex1))
                    {
                        intTmpIndex1 = intTmpIndex2;
                        intTagIndex = i;
                    }
                }
                if (intTmpIndex1 != -1)
                {
                    strTmpValue2 = strTmpValue.Substring(intIndex, intTmpIndex1 - intIndex);
                    if (strTmpValue2 != string.Empty)
                    {
                        string[] strTmpValues = strTmpValue2.Split(' ');
                        for (int j = 0; j < strTmpValues.Length; j++)
                        {
                            if (strTmpValues[j] != string.Empty)
                                strWords.Add(strTmpValues[j]);
                        }
                        //strWords.Add(strTmpValue2);
                    }

                    strWords.Add(strTags[intTagIndex]);
                    intIndex = intTmpIndex1 + strTags[intTagIndex].Length; 
                }

            }

            if (intIndex < strTmpValue.Length)
            {
                string[] strTmpValues = strTmpValue.Substring(intIndex).Split(' ');
                for (int j = 0; j < strTmpValues.Length; j++)
                {
                    if (strTmpValues[j] != string.Empty)
                        strWords.Add(strTmpValues[j]);
                }
            }

            string[] strUnstripValues = new string[strValues.Length];
            intIndex = 0;
            intTmpIndex1 = 0;
            intTmpIndex2 = 0;
            for (int i = 0; i < strUnstripValues.Length; i++)
            {
                intTmpIndex1 = 0;
                intTmpIndex2 = 0;
                while (intTmpIndex1 != -1 && intIndex < strWords.Count)
                {
                    intTmpIndex1 = strValues[i].IndexOf(strWords[intIndex], intTmpIndex2);
                    if (intTmpIndex1 == -1)
                    {
                        for (int j = 0; j < strTags.Length; j++)
                        {
                            if (strWords[intIndex] == strTags[j])
                            {
                                intTmpIndex1 = 0;

                                strUnstripValues[i] = string.Format("{0}{1}", strUnstripValues[i],
                                    strWords[intIndex]);
                                intIndex++;
                                break;
                            }
                        }
                    }
                    else
                    {
                        strUnstripValues[i] = string.Format("{0}{1}", strUnstripValues[i],
                            strValues[i].Substring(intTmpIndex2, 
                            intTmpIndex1 + strWords[intIndex].Length - intTmpIndex2));
                        intTmpIndex2 = intTmpIndex1 + strWords[intIndex].Length;
                        intIndex++;
                    }

                } 
            }

            //JVL fix
            for (int i = 0; i <= strUnstripValues.Length - 1; i++ )
            {
                if (strUnstripValues[i] == null)
                {
                    strUnstripValues[i] = string.Empty;
                }
            }

            return strUnstripValues;
        }

        public static string[] UnstripWrapString(string[] strValues, string[] strTags,
            int[] intTagIndices, int[] intCharIndices)
        {
            string[] strUnstripValues = new string[strValues.Length];
            for (int i = 0; i < strValues.Length; i++)
            {
                strUnstripValues[i] = strValues[i];
            }

            string strExcess = string.Empty;
            int intStartIndex = 0;
            int intCharStartIndex = 0;
            if (strValues.Length > 0)
            {
                int intTmpLength = strValues[0].TrimStart(new char[] { ' ' }).Length;
                if (intTmpLength != strValues[0].Length)
                {
                    intStartIndex = strValues[0].Length - intTmpLength;
                    intCharStartIndex = intStartIndex;
                }
                strExcess = strValues[0].Substring(0, intStartIndex-1);
            }


            string strTmpValue = string.Empty;

            string strValue = string.Empty;
            StringBuilder strUnstripValue = new StringBuilder();
            for (int i = 0; i < strUnstripValues.Length; i++)
            {

                if (i == 0)
                    strTmpValue = strValues[0].TrimStart(new char[] { ' ' });
                else
                    strTmpValue = strValues[i];
                strTmpValue = StringUtilities.RemoveDoubleSpaces(strValues[i]);
                if (i == 0)
                {
                    strTmpValue = string.Format("{0}{1}", strExcess, strTmpValue);
                    intStartIndex = 0;
                }

                strValue = StringUtilities.UnstripString(
                    string.Format("{0}{1}", strUnstripValue.ToString(), strTmpValue), 
                    strTags, intStartIndex, intTagIndices, intCharIndices);
                if (i == 0 && intStartIndex != 0)
                {
                    strValue = string.Format("{0}{1}", strExcess, strValue);
                    strUnstripValues[i] = strValue;
                    strUnstripValue.Append(strValue);
                }
                else
                {
                    strUnstripValues[i] = strValue.Substring(intStartIndex);
                    strUnstripValue.Append(strValue.Substring(intStartIndex));
                }

                intStartIndex = strValue.Length;

            }

            return strUnstripValues;
        }

        public static string UnstripString(string strValue, string[] strTags, int intStartIndex, int[] intTagIndices, int[] intCharIndices)
        {
            string strUnstripValue = strValue;

            int i = 0;
            List<string> lstValues = new List<string>();
            for (i = 0; i < intTagIndices.Length; i++)
            {
                if (intCharIndices[i] >= intStartIndex &&
                    intCharIndices[i] <= intStartIndex + strUnstripValue.Length)
                {
                    strUnstripValue = strUnstripValue.Insert(intCharIndices[i],
                        strTags[intTagIndices[i]]);
                }
            }

            return strUnstripValue;
        }

        //should use word count instead of character count since after wrapping character indices changes.
        public static string StripString(string strValue, string[] strTags, out int[] intTagIndices, out int[] intCharIndices)
        {
            string strStripValue = strValue;

            List<int> lstTagIndices = new List<int>();
            List<int> lstCharIndices = new List<int>();

            lstTagIndices.Clear();
            lstCharIndices.Clear();

            int intIndex = 0;
            int intTagIndex = -1;
            int intCharIndex = 0;
            int intTmpCharIndex = 0;
            while (intCharIndex != -1)
            {
                intTagIndex = -1;
                intCharIndex = -1;
                for (int i = 0; i < strTags.Length; i++)
                {
                    intTmpCharIndex = strStripValue.IndexOf(strTags[i], intIndex);
                    if (lstCharIndices.IndexOf(intTmpCharIndex) == -1 && intTmpCharIndex != -1 &&
                        (intCharIndex == -1 || intTmpCharIndex < intCharIndex))
                    {
                        intTagIndex = i;
                        intCharIndex = intTmpCharIndex;
                    }
                }
                if (intTagIndex != -1)
                {
                    lstTagIndices.Add(intTagIndex);
                    lstCharIndices.Add(intCharIndex);
                    intIndex = intCharIndex + strTags[intTagIndex].Length;
                }
            }

            for (int i = 0; i < strTags.Length; i++)
            {
                strStripValue = strStripValue.Replace(strTags[i], string.Empty);
            }

            intTagIndices = lstTagIndices.ToArray();
            intCharIndices = lstCharIndices.ToArray();

            return strStripValue;
        }

        /*
        Algorithm 

        Base = 1.0	
        WHILE (true)
        {
          Divider = Base/ 2
          Insert(Divider);
          Iterator = Divider
          While (Iterator + Base <1)
          {
            Iterator +=  Base
            Insert(Iterator)
          }
          Base = Divider;
        }
        */

        public static string FlushString(string strValue, float fltWidth, Graphics g, Font font)
        {
            string strOldValue = strValue;
            string strTmpExcess = string.Empty;

            //ignores starting spaces if any
            string strTmpOldValue = strValue.TrimStart(new char[] { ' ' });
            int intTmpExcess = 0;
            if (strOldValue.Length != strTmpOldValue.Length)
                intTmpExcess = strOldValue.Length - strTmpOldValue.Length;
            float fltTmpExcess = 0.0f;
            if (intTmpExcess != 0)
            {
                strTmpExcess = strValue.Substring(0, intTmpExcess);
                fltTmpExcess = g.MeasureString(string.Format("{0}\0", strTmpExcess), font).Width;
            }
            strValue = strValue.Substring(intTmpExcess);

            StringBuilder strNewValue = new StringBuilder();

            string[] strWords = strValue.Split(' ');

            
            float fltTmpWidth = 0.0f;
            fltTmpWidth = g.MeasureString(string.Format("{0}{1}", strTmpExcess, strValue), font).Width;
            
            if (fltTmpWidth >= fltWidth || strWords.Length == 1)
                return strValue;

            double dblBase = 1.0;
            double dblDivider = 0.0;
            double dblIterator = 0.0;

            int intIndex = 0;
            bool blnRunOnce = true;
            while (fltTmpWidth < fltWidth)
            {
                dblDivider = dblBase / 2.0;
                dblIterator = dblDivider;
                blnRunOnce = true;

                while (blnRunOnce || (dblIterator + dblBase) < 1)
                {
                    if (blnRunOnce)
                        blnRunOnce = false;
                    else
                        dblIterator += dblBase;
                    intIndex = (int)(strWords.Length * dblIterator);

                    strWords[intIndex] = string.Format("{0} ", strWords[intIndex]); //adds space
                    
                    strNewValue.Length = 0;
                    for (int i = 0; i < strWords.Length; i++)
                    {
                        if (i != 0)
                            strNewValue.Append(" ");
                        strNewValue.Append(strWords[i]);
                    }

                    fltTmpWidth = g.MeasureString(string.Format("{0}{1}", strTmpExcess, strNewValue.ToString()), font).Width;
                    if (fltTmpWidth > fltWidth)
                    {
                        return string.Format("{0}{1}", strTmpExcess, strOldValue.TrimStart(new char[] {' '}));
                    }
                    strOldValue = strNewValue.ToString();
                }
                dblBase = dblDivider;
            }

            /*
            int intIndex = 0;
            int intBaseIndex = 2;

            int intMaximumIndex = 0;

            while (fltTmpWidth < fltWidth) //should have unit test
            {
                strNewValue.Length = 0;
                intIndex = 0;
                intMaximumIndex = strWords.Length / intBaseIndex; //distributed  fully
                for (int i = 0; i < strWords.Length; i++)
                {
                    if (i != 0)
                        strNewValue.Append(" ");
                    strNewValue.Append(strWords[i]);
                    intIndex++;
                    if (intIndex == intMaximumIndex)
                    {
                        strNewValue.Append(" ");
                        intIndex = 0;
                    }
                }
                fltTmpWidth = g.MeasureString(strNewValue.ToString(), font).Width;
                if (fltTmpWidth > fltWidth || intMaximumIndex <= 1)
                {
                    return strOldValue;
                }
                strOldValue = strNewValue.ToString();
                intBaseIndex++;
            }
            */


            return string.Format("{0}{1}", strTmpExcess, strValue.TrimStart(new char[] { ' ' }));
        }

        public static string[] WrapString(string strValue, float fltWidth, Graphics g, Font font, bool blnIsFlush)
        {
            int intCount = 0;
            List<string> lstValues = new List<string>();
            string[] strWords = strValue.Split(' ');
            string strTmpValue = string.Empty;
            string strTmpValue1 = string.Empty;
            float fltIdxWidth1 = 0.0F;
            float fltIdxWidth2 = 0.0F;
            float fltIdxWidth3 = 0.0F;
            float fltTmpWidth = 0.0F;
            intCount = strWords.Length;
            for (int i = 0; i < intCount; i++)
            {
                strTmpValue1 = strTmpValue;
                if (strTmpValue == string.Empty)
                    strTmpValue = string.Format("{0} ", strWords[i]);
                else if (strTmpValue.Trim() == string.Empty && strWords[i].Trim() != string.Empty) //avoids unwanted space
                    strTmpValue = string.Format("{0}{1}", strTmpValue, strWords[i]);
                else
                    strTmpValue = string.Format("{0} {1}", strTmpValue, strWords[i]);
                fltIdxWidth1 = g.MeasureString(string.Format("{0} \0", strTmpValue), font).Width;
                fltIdxWidth2 = g.MeasureString(string.Format("{0}\0", strTmpValue), font).Width;
                fltIdxWidth3 = g.MeasureString(strWords[i], font).Width;
                if ((fltIdxWidth1) > fltWidth)
                {
                    fltTmpWidth = fltIdxWidth3;
                    strTmpValue = strWords[i];
                    if (blnIsFlush)
                        lstValues.Add(StringUtilities.FlushString(strTmpValue1, fltWidth, g, font));
                    else
                        lstValues.Add(strTmpValue1);

                }
                else if (fltIdxWidth2 > fltWidth)
                {
                    fltTmpWidth = fltIdxWidth3;
                    strTmpValue = strWords[i];

                    if (blnIsFlush)
                        lstValues.Add(StringUtilities.FlushString(strTmpValue1, fltWidth, g, font));
                    else
                        lstValues.Add(strTmpValue1);

                }
                else
                {
                    fltTmpWidth += fltIdxWidth1;
                }
            }
            if (strTmpValue != string.Empty)
                lstValues.Add(strTmpValue);

            return lstValues.ToArray();
        }
        

        public static string CenterString(string strValue, int intLength)
        {
            string strCenteredValue = string.Empty;
            if (intLength <= strValue.Length)
                return strValue;
            int intDiff = (intLength - strValue.Length) / 2;
            strCenteredValue = string.Format("{0}{1}{2}",
                new string(' ', intDiff), strValue, new string(' ',
                intLength - strValue.Length - intDiff));

            return strCenteredValue;
        }

        //public static string[] SplitString(

        /*
        //see http://answers.google.com/answers/threadview?id=349913 for capitalization in titles
        public static string ToTitleCase(string strValue)
        {
            StringBuilder strTitleCase = new StringBuilder();
            string[] strValues = strValue.Split(' ');
            int intCount = strValues.Length;
            for (int i = 0; i < intCount; i++)
            {
            }


            return strTitleCase.ToString();
        }

        public static string ToSentenceCase(string strValue)
        {
            StringBuilder strSentenceCase = new StringBuilder();
            string[] strValues = strValue.Split(' ');
            bool blnIsFirstWord = true;
            for (int i = 0; i < intCount; i++)
            {
                if (strValues[i].Trim() != string.Empty && blnIsFirstWord)
                {
                    strValues[i].ToLower();
                    blnIsFirstWord = false;
                }
            }

            return strSentenceCase;
        }
         */

        // RMC 20101201 (s)
        public static string Left(string param, int length)
        {
            //we start at 0 since we want to get the characters starting from the
            //left and with the specified lenght and assign it to a variable
            string result = string.Empty;

            if (param.Length >= length)
                result = param.Substring(0, length);
            else
                result = param;
            //return the result of the operation
            return result;
        }

        public static string Right(string param, int length)
        {
            //start at the index based on the lenght of the sting minus
            //the specified lenght and assign it a variable
            string result = string.Empty;
            if (param.Length >= length)
                result = param.Substring(param.Length - length, length);
            else
                result = param;
            //return the result of the operation
            return result;
        }

        public static string HandleApostrophe(string strValue)
        {
            string strNewValue = strValue;
            strNewValue = strNewValue.Replace("''", "'");
            strNewValue = strNewValue.Replace("'", "''");
            return strNewValue;
        }
        // RMC 20101201 (e)

        public static string RemoveApostrophe(string strValue)
        {
            string strNewValue = strValue;
            strNewValue = strNewValue.Replace("''", "'");
            return strNewValue;
        }

        public static string SetEmptyToSpace(string strValue)
        {
            //string strNewValue = string.Empty;
            string strNewValue = strValue;  // RMC 20110311
                
            if (strValue == "" || strValue == null)
                strNewValue = " ";

            return strNewValue;
        }

        public static string RemoveOtherCharacters(string strValue)
        {
            //RMC 20110311
            string strNewValue = strValue;
            strNewValue = strNewValue.Replace("|", " ");
            strNewValue = strNewValue.Replace(";", " ");
            return strNewValue.Trim();
        }
    }
}
