using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace Amellar.Modules.EPS
{
    public partial class frmViewParticulars : Form
    {
        public frmViewParticulars()
        {
            InitializeComponent();
        }

        private string m_sBin = "";
        public string Bin
        {
            get { return m_sBin; }
            set { m_sBin = value; }
        }
        

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViewParticulars_Load(object sender, EventArgs e)
        {
            DataTable Payments = new DataTable();
            OracleResultSet pSet = new OracleResultSet();

            Payments.Columns.Add("OR Number");
            Payments.Columns.Add("Tax Year");
            Payments.Columns.Add("Amount");


            pSet.Query = "select distinct OT.OR_NO,OT.FEES_AMTDUE,OT.TAX_YEAR,OT.BNS_CODE_MAIN from pay_hist PH left join or_table OT on PH.or_no = OT.or_no ";
            //pSet.Query += "where PH.bin = '" + m_sBin + "' AND OT.FEES_CODE = '09' order by OT.tax_year ASC, OT.BNS_CODE_MAIN ASC";
            pSet.Query += "where PH.bin = '" + m_sBin + "' AND OT.FEES_CODE = '25' order by OT.tax_year ASC, OT.BNS_CODE_MAIN ASC"; //AFM 20210716 set to fees code of annual insp fee (Malolos)

            if(pSet.Execute())
            {
                while(pSet.Read())
                {
                    Payments.Rows.Add(pSet.GetString("or_no"),pSet.GetString("tax_year"), pSet.GetDouble("fees_amtdue").ToString("#,##0.00"));
                }
            }
            pSet.Close();

            dgvParticulars.DataSource = Payments;
        }
    }
}