using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Amellar.Common.DateTimeUtilities
{
    //@author R.D.Ong
    public static class DateTimeRange
    {
        [Flags]
        public enum DateTimeRangeStatus
        {
            ReplaceToWithFromOnFromGreaterThanTo = 0x0001,
            ReplaceFromWithToOnFromLessThanTo = 0x0002,
            ReplaceToWithCurrentOnToGreaterThanCurrent = 0x0004,
            ReplaceFromWithCurrentOnFromGreaterThanCurrent = 0x0010,
            All = ReplaceToWithCurrentOnToGreaterThanCurrent | ReplaceFromWithCurrentOnFromGreaterThanCurrent
                | ReplaceToWithFromOnFromGreaterThanTo | ReplaceFromWithToOnFromLessThanTo
        }

        public static bool ValidateDateTimeRange(DateTimePicker dtpFrom, DateTimePicker dtpTo, DateTime dtCurrent, bool blnIsFrom,
            DateTimeRangeStatus status)
        {
            bool blnIsAdjusted = false;
            DateTime dtFrom1;
            DateTime dtTo1;
            blnIsAdjusted = DateTimeRange.ValidateDateTimeRange(dtpFrom.Value, dtpTo.Value,
                dtCurrent,  blnIsFrom, status, out dtFrom1,
                out dtTo1);
            if (blnIsAdjusted)
            {
                dtpFrom.Value = dtFrom1;
                dtpTo.Value = dtTo1;
            }
            return blnIsAdjusted;
        }

        public static bool ValidateDateTimeRange(DateTime dtFrom, DateTime dtTo, DateTime dtCurrent, bool blnIsFrom,
            DateTimeRangeStatus status, out DateTime dtFrom1, out DateTime dtTo1)
        {
            bool blnIsAdjusted = false;

            dtFrom1 = dtFrom;
            dtTo1 = dtTo;

            string strCurrent = string.Empty;
            string strFrom = string.Empty;
            string strTo = string.Empty;

            strCurrent = string.Format("{0:yyyy/MM/dd}", dtCurrent);
            strFrom = string.Format("{0:yyyy/MM/dd}", dtFrom);
            strTo = string.Format("{0:yyyy/MM/dd}", dtTo);

            if (blnIsFrom)
            {
                if (strFrom.CompareTo(strCurrent) > 0)
                {
                    if ((status & DateTimeRangeStatus.ReplaceFromWithCurrentOnFromGreaterThanCurrent) ==
                        DateTimeRangeStatus.ReplaceFromWithCurrentOnFromGreaterThanCurrent)
                    {
                        dtFrom1 = dtCurrent;
                        strFrom = string.Format("{0:yyyy/MM/dd}", dtFrom1);
                        blnIsAdjusted = true;
                    }
                }
                if (strFrom.CompareTo(strTo) > 0)
                {
                    if ((status & DateTimeRangeStatus.ReplaceToWithFromOnFromGreaterThanTo) ==
                        DateTimeRangeStatus.ReplaceToWithFromOnFromGreaterThanTo)
                    {
                        dtTo1 = dtFrom1;
                        blnIsAdjusted = true;
                    }
                    else
                    {
                        dtFrom1 = dtTo;
                        blnIsAdjusted = true;
                    }
                }
            }
            else //should remove this part of code (redundant)
            {
                if (strTo.CompareTo(strCurrent) > 0)
                {
                    if ((status & DateTimeRangeStatus.ReplaceToWithCurrentOnToGreaterThanCurrent) ==
                        DateTimeRangeStatus.ReplaceToWithCurrentOnToGreaterThanCurrent)
                    {
                        dtTo1 = dtCurrent;
                        strTo = string.Format("{0:yyyy/MM/dd}", dtTo1);
                        blnIsAdjusted = true;
                    }
                }

                if (strTo.CompareTo(strFrom) < 0)
                {
                    if ((status & DateTimeRangeStatus.ReplaceFromWithToOnFromLessThanTo) ==
                        DateTimeRangeStatus.ReplaceFromWithToOnFromLessThanTo)
                    {
                        dtFrom1 = dtTo1;
                        blnIsAdjusted = true;
                    }
                    else
                    {
                        dtTo1 = dtFrom;
                        blnIsAdjusted = true;
                    }
                }
            }

            return blnIsAdjusted;
        }
    }
}
