using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using Amellar.Common.DataConnector;

namespace Amellar.BPLS.Billing
{
    /// <summary>
    /// ALJ 20090723
    /// base class of BillFee, BillAddl, BillFire
    /// </summary>
    public class Fee
    {
        protected frmBillFee BillFeeForm = null;
        public Fee(frmBillFee Form)
        {
            this.BillFeeForm = Form;
        }

        public virtual void UpdateList()
        {
        }

        public virtual double Compute(string p_sCode)
        {
            return 0.0;
        }

        public virtual double Compute(string p_sCode, string sBrgy)
        {
            return 0.0;
        }

        public virtual double OnButtonCompute()
        {
            return 0.0;
        }

        public virtual void UpdateRows()
        {
        }

        public double Total()
        {
            double fTotal, fAmount;
            fTotal = 0.00;
            int iRows, i;
            iRows = BillFeeForm.Fees.Rows.Count;
            for (i = 0; i < iRows; ++i)
            {
                double.TryParse(BillFeeForm.Fees.Rows[i].Cells[3].Value.ToString(), out fAmount);
                fTotal = fTotal + fAmount;
            }

            return fTotal;
        }



    }
}


