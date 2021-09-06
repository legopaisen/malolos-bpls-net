using System;
using System.Collections.Generic;
using System.Text;

namespace Amellar.Common.DeficientRecords
{
    public class DeficientRecordsToolCommonItem:DeficientRecordsToolItem
    {
        public DeficientRecordsToolCommonItem(int intSerial, string strKey, string strValue)
        {
            this.Serial = intSerial;
            this.Key = strKey;
            this.Value = strValue;
        }
    }
}
