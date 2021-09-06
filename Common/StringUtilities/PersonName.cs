using System;
using System.Collections.Generic;
using System.Text;

namespace Amellar.Common.StringUtilities
{
    public static class PersonName
    {

        public static string ToPersonName(string strLastName, string strFirstName, string strMI,
            string strPattern1, string strPattern2, string strPattern3)
        {
            if (strFirstName == string.Empty)
                return PersonName.ToPersonName(strPattern1, strLastName, strFirstName, strMI);
            else if (strMI == string.Empty)
                return PersonName.ToPersonName(strPattern2, strLastName, strFirstName, strMI);
            else
                return PersonName.ToPersonName(strPattern3, strLastName, strFirstName, strMI);

        }

        /*
          Patterns:
                         UC	 LC  I  SC  NC 
            lastname     L   l   k  J   j
            firstname    F   f   e  D   d   
            middlename   M   m   n  O   o

            I - Initial
            SC - Sentence Case //(not yet implemented)
            NC - No Case
  
          Will later add Exceptions: `los`, `lo` for Sentence case
          Will later add handler for Jr. Sr. etc.

          D n. J

         * @author R.D.Ong
        */

        public static string ToPersonName(string strPattern, string strLastName, string strFirstName, string strMI)
        {
            string strPatternTmp = strPattern;
            int intDictionarySize = 15;
            string[] strDictionaryKeys = new string[] { "L", "l", "k", "J", "j", "F", "f", "e", "D", "d", "M", "m", "n", "O", "o" };
            string[] strDictionaryValues = new string[intDictionarySize];

            strDictionaryValues[0] = strLastName.ToUpper();
            strDictionaryValues[1] = strLastName.ToLower();
            strDictionaryValues[2] = string.Empty;
            if (strLastName.Length != 0)
            {
                strDictionaryValues[2] = strLastName.Substring(0, 1);
            }
            strDictionaryValues[3] = strLastName;
            strDictionaryValues[4] = strLastName;

            strDictionaryValues[5] = strFirstName.ToUpper();
            strDictionaryValues[6] = strFirstName.ToLower();
            strDictionaryValues[7] = string.Empty;
            if (strFirstName.Length != 0)
            {
                strDictionaryValues[7] = strFirstName.Substring(0, 1);
            }
            strDictionaryValues[8] = strFirstName;
            strDictionaryValues[9] = strFirstName;

            strDictionaryValues[10] = strMI.ToUpper();
            strDictionaryValues[11] = strMI.ToLower();
            strDictionaryValues[12] = string.Empty;
            if (strMI.Length != 0)
            {
                strDictionaryValues[12] = strMI.Substring(0, 1);
            }
            strDictionaryValues[13] = strMI;
            strDictionaryValues[14] = strMI;

            string strPlaceHolder = "@@@";
            int intIndex = -1;
            for (int i = 0; i < intDictionarySize; i++)
            {
                while (true)
                {
                    intIndex = strPatternTmp.IndexOf(strDictionaryKeys[i]);
                    if (intIndex == -1)
                        break;
                    else
                    {
                        strPatternTmp = string.Format("{0}{1}{2:0#}{3}", strPatternTmp.Substring(0, intIndex),
                            strPlaceHolder, i, strPatternTmp.Substring(intIndex + strDictionaryKeys[i].Length));
                    }
                }
            }
            string strPlaceHolderTmp = string.Empty;
            for (int i = 0; i < intDictionarySize; i++)
            {
                while (true)
                {
                    strPlaceHolderTmp = string.Format("{0}{1:0#}", strPlaceHolder, i);
                    intIndex = strPatternTmp.IndexOf(strPlaceHolderTmp);
                    if (intIndex == -1)
                        break;
                    else
                        strPatternTmp = string.Format("{0}{1}{2}",
                            strPatternTmp.Substring(0, intIndex),
                            strDictionaryValues[i],
                            strPatternTmp.Substring(intIndex + strPlaceHolderTmp.Length));
                }
            }
            return strPatternTmp;
        }
    }
}
