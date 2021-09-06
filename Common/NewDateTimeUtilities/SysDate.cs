using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;

namespace Amellar.Common.NewDateTimeUtilities
{
    public class SysDate
    {
        public class SystemDate
        {
            public static DateTime GetStartingDate(DateTime dtValue)
            {
                return new DateTime(dtValue.Year, dtValue.Month, dtValue.Day, dtValue.Hour, dtValue.Minute, dtValue.Second);
            }

            public static DateTime GetEndingDate(DateTime dtValue)
            {
                System.Globalization.Calendar calendar = new System.Globalization.GregorianCalendar();
                return new DateTime(dtValue.Year, dtValue.Month, calendar.GetDaysInMonth(dtValue.Year, dtValue.Month),
                    dtValue.Hour, dtValue.Minute, dtValue.Second);
            }
        }
    }
}
