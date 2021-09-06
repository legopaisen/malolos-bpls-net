using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;

namespace Amellar.Common.DateTimeUtilities
{
    public class SystemDate
    {
        public static DateTime GetStartingDate(DateTime dtValue)
        {
            return new DateTime(dtValue.Year, dtValue.Month, 1, dtValue.Hour, dtValue.Minute, dtValue.Second);
        }

        public static DateTime GetEndingDate(DateTime dtValue)
        {
            System.Globalization.Calendar calendar = new System.Globalization.GregorianCalendar();
            return new DateTime(dtValue.Year, dtValue.Month, calendar.GetDaysInMonth(dtValue.Year, dtValue.Month),
                dtValue.Hour, dtValue.Minute, dtValue.Second);
        }
    }
}
