using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace Amellar.Modules.DILGReport
{
    public partial class frmTransactionTrail : Form
    {
        int flag = 0;

        public frmTransactionTrail()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmTransactionTrail_Load(object sender, EventArgs e)
        {
            cmbAppStat.Items.Add("ALL");
            cmbAppStat.Items.Add("NEW");
            cmbAppStat.Items.Add("RENEWAL");
            cmbAppStat.Items.Add("RETIRED");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();
            
            if (cmbAppStat.Text == "")
            {
                MessageBox.Show("Complete required fields","Transaction Trail",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            int iRen = 0; int iNew = 0; int iRet = 0;
            string sDateFr = string.Format("{0:MM/dd/yyyy}", dtpDateFr.Value);
            string sDateTo = string.Format("{0:MM/dd/yyyy}", dtpDateTo.Value);

            result.Query = "select count(*) from app_permit_no where to_date(issued_date,'MM/dd/yyyy') between to_date('" + sDateFr + "','MM/dd/yyyy') and to_date('" + sDateTo + "','MM/dd/yyyy')";
            if (result.Execute())
            {
                if (result.Read())
                    iRen = result.GetInt(0);
            }
            result.Close();

            result.Query = "select count(*) from app_permit_no_new where to_date(issued_date,'MM/dd/yyyy') between to_date('" + sDateFr + "','MM/dd/yyyy') and to_date('" + sDateTo + "','MM/dd/yyyy')";
            if (result.Execute())
            {
                if (result.Read())
                    iNew = result.GetInt(0);
            }
            result.Close();

            /*result.Query = "select count(*) from app_permit_no_ret where to_date(issued_date,'MM/dd/yyyy') between to_date('" + sDateFr + "','MM/dd/yyyy') and to_date('" + sDateTo + "','MM/dd/yyyy')";
            if (result.Execute())
            {
                if (result.Read())
                    iRet = result.GetInt(0);
            }
            result.Close();*/

            if ((cmbAppStat.Text == "NEW" && iNew == 0) || (cmbAppStat.Text == "RENEWAL" && iRen == 0) /*|| (cmbAppStat.Text == "RETIRED" && iRet == 0)*/
                || (cmbAppStat.Text == "ALL" && iNew == 0 && iRen == 0 /*&& iRet == 0*/))
            {
                MessageBox.Show("No record found","Transaction Trail",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }

            frmDILGReportForm form = new frmDILGReportForm();
            form.m_sBnsStat = cmbAppStat.Text;
            form.m_dtDateFr = dtpDateFr.Value;
            form.m_dtDateTo = dtpDateTo.Value;
            form.ShowDialog();
        }

        
        
        
    }
}