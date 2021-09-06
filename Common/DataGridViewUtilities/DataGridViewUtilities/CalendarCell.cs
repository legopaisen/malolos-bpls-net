using System;
using System.Windows.Forms;
using Amellar.Common.AppSettings;

namespace Amellar.Common.DataGridViewUtilities
{
    //copied from http://msdn.microsoft.com/hi-in/library/7tas5c80(en-us,vs.80).aspx
    public class CalendarCell : DataGridViewTextBoxCell
    {
        private bool m_blnHasMinMaxDate;
        private DateTime m_dtMinDate;
        private DateTime m_dtMaxDate;

        public CalendarCell()
            : base()
        {
            // Use the short date format.
            this.Style.Format = "d";
            m_blnHasMinMaxDate = false;
        }

        public CalendarCell(DateTime dtMinDate, DateTime dtMaxDate):base()
        {
            this.Style.Format = "d";

            m_blnHasMinMaxDate = true;
            m_dtMinDate = dtMinDate;
            m_dtMaxDate = dtMaxDate;
        }
        

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            CalendarEditingControl ctl =
                DataGridView.EditingControl as CalendarEditingControl;
            if (m_blnHasMinMaxDate)
            {
                if (ctl.MinDate > m_dtMaxDate)
                    ctl.MinDate = m_dtMaxDate;
                ctl.MaxDate = m_dtMaxDate;
                ctl.MinDate = m_dtMinDate;
            }
            ctl.Value = (DateTime)this.Value;
        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing contol that CalendarCell uses.
                return typeof(CalendarEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                // Return the type of the value that CalendarCell contains.
                return typeof(DateTime);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                // Use the current date and time as the default value.
               // return DateTime.Now;
                return AppSettingsManager.GetCurrentDate(); // RMC 20110725
            }
        }
    }
}
