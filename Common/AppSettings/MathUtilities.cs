using System;
using System.Collections.Generic;
using System.Text;

namespace Amellar.Common.AppSettings
{
    //should separate to another solution
    public static class MathUtilities
    {
        public static double RoundValue(double dblTmpValue, int intPrecision, MidpointRounding midRound)
        {
            double dblValue = Math.Abs(dblTmpValue);

            StringBuilder strPatternTmp = new StringBuilder();
            strPatternTmp.Append("{0:####.");
            for (int i = 0; i < intPrecision; i++)
                strPatternTmp.Append("0");
            strPatternTmp.Append("}");

            StringBuilder strPattern = new StringBuilder();
            strPattern.Append("0.");
            for (int i = 0; i < intPrecision-1; i++)
                strPattern.Append("0");
            strPattern.Append("1");

            double dblTemp = 0.0;
            double dblTemp2 = 0.0;
            double dblRoundedValue = 0.0;
            string strValue = dblValue.ToString();
            int intDecimal = strValue.LastIndexOf('.');
            int intTemp = 0;
            if (intDecimal == -1) //no decimal places
            {
                //RDO 011608 (s) fix for 0.99999 problem
                dblValue = 0.0;
                double.TryParse(strValue, out dblValue);
                //RDO 012108 (s) should return negative values 
                if (dblTmpValue < 0.0) 
                    return -dblValue;
                else
                //RDO 012108 (e)
                    return dblValue;
                //RDO 011608 (e) fix for 0.99999 problem
                //return dblTmpValue;
            }
            else
            {
                dblTemp = 0.0;
                double.TryParse(strValue.Substring(0, intDecimal), out dblTemp);
                dblRoundedValue = dblTemp;
                dblTemp = 0.0;
                if (strValue.Length >= intDecimal + intPrecision + 1)
                {
                    dblTemp = 0.0;
                    double.TryParse(strValue.Substring(0, intDecimal + intPrecision+1), out dblTemp);
                    dblRoundedValue = dblTemp;

                    intTemp = 0;
                    if (strValue.Length > intDecimal + intPrecision + 1)
                    {
                        int.TryParse(strValue.Substring(intDecimal + intPrecision + 1, 1), out intTemp);
                        if (intTemp > 4)
                        {
                            dblTemp2 = 0.0;
                            double.TryParse(strPattern.ToString(), out dblTemp2);
                            dblRoundedValue += dblTemp2;
                        }
                    }
                    //remove unwanted lines
                    strValue = string.Format(strPatternTmp.ToString(), dblRoundedValue); //dblRoundedValue.ToString();
                    intDecimal = strValue.LastIndexOf('.');
                    if (intDecimal != -1 && strValue.Length >= intDecimal + intPrecision + 1)
                    {
                        dblTemp = 0;
                        double.TryParse(strValue.Substring(0, intDecimal + intPrecision + 1), out dblTemp);
                        dblRoundedValue = dblTemp;
                    }

                }
                else
                {
                    double.TryParse(string.Format("0.{0}", strValue.Substring(intDecimal + 1)), out dblTemp);
                    dblRoundedValue += dblTemp;
                }
            }
            if (dblTmpValue < 0.0)
                return -dblRoundedValue;


            return dblRoundedValue;
        }
    }
}
