using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Amellar.Common.StringUtilities
{
    /// <summary>
    /// Completely different implementation
    /// @author R.D.Ong
    /// </summary>
    public static class NumberWording
    {
       
        public static string ToWord(Hashtable references1, Hashtable references2, double dblNumber)
        {
            StringBuilder strWording = new StringBuilder();
            string[] strUnits = { "", "Thousand", "Million", "Billion", "Trillion" };

            string strNumber = dblNumber.ToString();
            int intNumDigits = strNumber.Length / 3;
            int intIndex = strNumber.Length % 3;
            if (intIndex != 0)
                intNumDigits++;
            else
                intIndex = 3;

            int i = 0;
            string[] strDigits = new string[intNumDigits];
            strDigits[intNumDigits - 1] = strNumber.Substring(0, intIndex);
            for (i = intNumDigits - 2; i >= 0; i--)
            {
                strDigits[i] = strNumber.Substring(intIndex, 3);
                intIndex += 3;
            }

            int intValue = 0;
            int intStartIdx = 0;
            for (i = intNumDigits - 1; i >= 0; i--)
            {
                intStartIdx = 0;
                if (strDigits[i].Length == 3)
                {
                    intValue = 0;
                    int.TryParse(strDigits[i].Substring(0, 1), out intValue);
                    intStartIdx = 1;
                    if (intValue != 0)
                    {
                        strWording.Append(string.Format("{0} Hundred ", references1[intValue].ToString()));
                    }
                }
                if (strDigits[i].Length >= 2)
                {
                    intValue = 0;
                    int.TryParse(strDigits[i].Substring(intStartIdx, 2), out intValue);
                    if (intValue == 0)
                    {
                        intStartIdx = -1;
                    }
                    else if (references2.ContainsKey(intValue))
                    {
                        strWording.Append(references2[intValue]);
                        intStartIdx = -1;
                    }
                    else
                    {
                        intValue = 0;
                        int.TryParse(string.Format("{0}0", strDigits[i].Substring(intStartIdx, 1)), out intValue);
                        intStartIdx++;
                        if (intValue != 0)
                        {
                            strWording.Append(string.Format("{0} ", references2[intValue]));
                        }
                    }


                }
                if (intStartIdx != -1)
                {
                    intValue = 0;
                    int.TryParse(strDigits[i].Substring(intStartIdx, 1), out intValue);
                    intStartIdx++;
                    if (intValue != 0)
                    {
                        strWording.Append(references1[intValue]);
                    }
                }

                intValue = 0;
                int.TryParse(strDigits[i], out intValue);
                if (intValue != 0)
                    strWording.Append(string.Format(" {0} ", strUnits[i]));
            }
            return strWording.ToString();
        }

        /*
        //see http://www.ego4u.com/en/cram-up/vocabulary/numbers/ordinal
        public static string ToOrdinal(int intNumber)
        {
            string strOrdinal = string.Empty;
            
            string strNumber = intNumber.ToString();
            //get ones 
            int intOnes = 0;
            int.TryParse(strNumber.Substring(strNumber.Length - 1, 1), out intOnes);
            int intCardinal = 0;
            int.TryParse(string.Format("{0}0", strNumber.Substring(0, strNumber.Length - 1)),
                out intCardinal);

            string strCardinal = NumberWording.ToCardinal((double)intCardinal).Trim();
            if (intOnes == 0)
            {
            }
            
            return strOrdinal;
        }
        */
        public static string ToOrdinalFigure(int intNumber)
        {
            string strOrdinal = string.Empty;
            string strNumber = intNumber.ToString();
            int intOnes = 0;
            int.TryParse(strNumber.Substring(strNumber.Length - 1, 1), out intOnes);

            int intTens = 0;
            if (strNumber.Length >= 2)
                int.TryParse(strNumber.Substring(strNumber.Length - 2, 2), out intTens);


            if (intTens == 11 || intTens == 12 || intTens == 13)
                strOrdinal = string.Format("{0}th", intNumber);
            else if (intOnes == 1)
                strOrdinal = string.Format("{0}st", intNumber);
            else if (intOnes == 2)
                strOrdinal = string.Format("{0}nd", intNumber);
            else if (intOnes == 3)
                strOrdinal = string.Format("{0}rd", intNumber);
            else
                strOrdinal = string.Format("{0}th", intNumber);

            return strOrdinal;
        }

        public static string ToCardinal(double dblNumber)
        {
            Hashtable references1 = new Hashtable();
            references1.Add(1, "One");
            references1.Add(2, "Two");
            references1.Add(3, "Three");
            references1.Add(4, "Four");
            references1.Add(5, "Five");
            references1.Add(6, "Six");
            references1.Add(7, "Seven");
            references1.Add(8, "Eight");
            references1.Add(9, "Nine");

            Hashtable references2 = new Hashtable();
            references2.Add(10, "Ten");
            references2.Add(11, "Eleven");
            references2.Add(12, "Twelve");
            references2.Add(13, "Thirteen");
            references2.Add(14, "Fourteen");
            references2.Add(15, "Fifteen");
            references2.Add(16, "Sixteen");
            references2.Add(17, "Seventeen");
            references2.Add(18, "Eighteen");
            references2.Add(19, "Nineteen");
            references2.Add(20, "Twenty");
            references2.Add(30, "Thirty");
            references2.Add(40, "Forty");
            references2.Add(50, "Fifty");
            references2.Add(60, "Sixty");
            references2.Add(70, "Seventy");
            references2.Add(80, "Eighty");
            references2.Add(90, "Ninety");

            return NumberWording.ToWord(references1, references2, dblNumber);
        }

        public static string ToMoneyWord(double dblNumber, char chrDelimiter,
            string strCurrency1, string strCurrency2, string strConnector)
        {
           StringBuilder strMoneyWord = new StringBuilder();
            string strValue = string.Format("{0:####.#0}", dblNumber);
            int intIndex = strValue.IndexOf(chrDelimiter);
            if (intIndex == -1)
                strMoneyWord.Append(string.Format("{0}{1}", NumberWording.ToCardinal(dblNumber),
                    strCurrency1));
            else
            {
                double dblValue1 = 0.0;
                double dblValue2 = 0.0;
                double.TryParse(strValue.Substring(0, intIndex), out dblValue1);
                double.TryParse(strValue.Substring(intIndex + 1), out dblValue2);
                if (dblValue1 != 0)
                    strMoneyWord.Append(string.Format("{0}{1}",
                        NumberWording.ToCardinal(dblValue1).Trim(), strCurrency1));
                if (dblValue1 != 0 && dblValue2 != 0)
                    strMoneyWord.Append(string.Format("{0}", strConnector));
                if (dblValue2 != 0)
                    strMoneyWord.Append(string.Format("{0}{1}",
                        NumberWording.ToCardinal(dblValue2).Trim(), strCurrency2));
            }
            return strMoneyWord.ToString();
        }

        public static string AmountInWords(double dblNumber)
        {
            return NumberWording.ToMoneyWord(dblNumber, '.', " Pesos", " Centavos", " and ");
        }

    }
}
