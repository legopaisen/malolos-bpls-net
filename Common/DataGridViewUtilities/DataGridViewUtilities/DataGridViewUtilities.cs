using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace Amellar.Common.DataGridViewUtilities
{
    public static class DataGridViewUtilities
    {
        //copied code from http://www.pcreview.co.uk/forums/thread-1312779.php
        //@already deprecated just disable allow user to add new row
        public static void HideNewRows(DataGridView dgv)
        {
            CurrencyManager cm = (CurrencyManager)dgv.BindingContext[
                dgv.DataSource, dgv.DataMember];
            DataView dv = (DataView)cm.List;
            dv.AllowNew = false;
        }

        public static string GetStringValue(DataGridViewSelectedRowCollection dgv, int intRowIndex,
            int intColumnIndex)
        {
            string strValue = string.Empty;
            if (dgv != null && dgv.Count != 0 && dgv.Count > intRowIndex
                && dgv[intRowIndex].Cells.Count != 0 && dgv[intRowIndex].Cells.Count > intColumnIndex &&
                dgv[intRowIndex].Cells[intColumnIndex].Value != null)
            {
                strValue = dgv[intRowIndex].Cells[intColumnIndex].Value.ToString();
            }
            return strValue;
        }

        public static string GetStringValue(DataGridView dgv, int intRowIndex, int intColumnIndex)
        {
            string strValue = string.Empty;
            if (dgv != null && dgv.Rows.Count != 0 && dgv.Rows.Count > intRowIndex
                && dgv.Rows[intRowIndex].Cells.Count != 0 && dgv.Rows[intRowIndex].Cells.Count > intColumnIndex &&
                dgv.Rows[intRowIndex].Cells[intColumnIndex].Value != null)
            {
                strValue = dgv.Rows[intRowIndex].Cells[intColumnIndex].Value.ToString();
            }
            return strValue;
        }

        public static int GetIntValue(DataGridView dgv, int intRowIndex, int intColumnIndex)
        {
            int intAmount = 0;
            if (dgv != null && dgv.Rows.Count != 0 && dgv.Rows.Count > intRowIndex
                && dgv.Rows[intRowIndex].Cells.Count != 0 && dgv.Rows[intRowIndex].Cells.Count > intColumnIndex &&
                dgv.Rows[intRowIndex].Cells[intColumnIndex].Value != null)
            {
                int.TryParse(dgv.Rows[intRowIndex].Cells[intColumnIndex].Value.ToString(), out intAmount);
            }
            return intAmount;
        }

        public static double GetDoubleValue(DataGridView dgv, int intRowIndex, int intColumnIndex)
        {
            double dblAmount = 0.0;
            if (dgv != null && dgv.Rows.Count != 0 && dgv.Rows.Count > intRowIndex
                && dgv.Rows[intRowIndex].Cells.Count != 0 && dgv.Rows[intRowIndex].Cells.Count > intColumnIndex &&
                dgv.Rows[intRowIndex].Cells[intColumnIndex].Value != null)
            {
                double.TryParse(dgv.Rows[intRowIndex].Cells[intColumnIndex].Value.ToString(), out dblAmount);
            }
            return dblAmount;
        }

        public static float GetFloatValue(DataGridView dgv, int intRowIndex, int intColumnIndex)
        {
            float fltAmount = 0.0F;
            if (dgv != null && dgv.Rows.Count != 0 && dgv.Rows.Count > intRowIndex
                && dgv.Rows[intRowIndex].Cells.Count != 0 && dgv.Rows[intRowIndex].Cells.Count > intColumnIndex &&
                dgv.Rows[intRowIndex].Cells[intColumnIndex].Value != null)
            {
                float.TryParse(dgv.Rows[intRowIndex].Cells[intColumnIndex].Value.ToString(), out fltAmount);
            }
            return fltAmount;
        }

    }
}
