using System;
using System.Collections.Generic;
using System.Text;

namespace Amellar.Common.DateTimeUtilities
{
    public class QuarterDate
    {
        public static bool GetQuarterDates(int intYear, int intQuarter, 
            out DateTime dtStartQuarter, out DateTime dtEndQuarter)
        {
            bool blnIsQuarterValid = true;
            int intDivisor = 3; //12 div 4
            int intStartMonth = 1;
            int intEndMonth = 12; //months per year
            if (intQuarter <= 4 && intQuarter > 0)
            {
                intStartMonth = (intQuarter - 1) * intDivisor + 1;
                intEndMonth = intStartMonth + intDivisor - 1;
            }
            else
            {
                blnIsQuarterValid = false;
            }
            dtStartQuarter = new DateTime(intYear, intStartMonth, 1);
            dtEndQuarter = new DateTime(intYear, intEndMonth, 
                DateTime.DaysInMonth(intYear, intEndMonth));
            return blnIsQuarterValid;
        }

        public static int GetQuarter(DateTime dtDate)
        {
            if (dtDate.Month > 9)
                return 4;
            else if (dtDate.Month > 6)
                return 3;
            else if (dtDate.Month > 3)
                return 2;
            else
                return 1;
        }

        //should be based on Holiday Adjustment
        //Month Week DayOfWeek  Count
        //x          x           x
        //followAdjustmentRule
        //Month Day
        //Month Day Year
    }

}
