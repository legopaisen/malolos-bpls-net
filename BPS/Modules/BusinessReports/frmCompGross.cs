using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace Amellar.Modules.BusinessReports
{
    public partial class frmCompGross : Form //JARS 20180326
    {
        public frmCompGross()
        {
            InitializeComponent();
        }

        private void LoadBrgy()
        {
            cmbBrgy.Items.Add("");
            cmbBrgy.Items.Add("ALL");
            OracleResultSet pRec = new OracleResultSet();
            pRec.Query = "select trim(brgy_nm) as brgy_nm from brgy order by brgy_nm asc";
            if(pRec.Execute())
            {
                while(pRec.Read())
                {
                    cmbBrgy.Items.Add(pRec.GetString("brgy_nm"));
                }
            }
            pRec.Close();
        }

        private void txtYearTo_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtYearTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtYearFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void frmCompGross_Load(object sender, EventArgs e)
        {
            LoadBrgy();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string sYear1 = "";
            string sYear2 = "";
            string sYear3 = "";
            string sMaxYear = "";

            if(txtYearTo.Text == "" || txtYearFrom.Text == "" /*|| cmbBrgy.Text == ""*/)
            {
                MessageBox.Show("Fill up all fields");
                return;
            }
            if(Convert.ToInt32(txtYearFrom.Text) > Convert.ToInt32(txtYearTo.Text))
            {
                MessageBox.Show("Invalid Tax Year Entry");
                return;
            }

            sYear1 = txtYearFrom.Text.Trim();

            for (int i = Convert.ToInt32(txtYearFrom.Text); i < Convert.ToInt32(txtYearTo.Text); )
            {
                i = i + 1;
                if(sYear2 == "")
                {
                    sYear2 = i.ToString();
                }
                else if(sYear3 == "")
                {
                    sYear3 = i.ToString();
                }
            }


            if (sYear3 == "")
            {
                sMaxYear = sYear2;
            }
            else
            {
                sMaxYear = sYear3;
            }

            //mjbb 20180706 added condition when entered year is same (s)
            if (txtYearFrom.ToString() == txtYearTo.ToString())
            {
                sMaxYear = txtYearTo.Text;
                sYear2 = sMaxYear;
            }
            //mjbb 20180706 added condition when entered year is same (e)

            if ((Convert.ToInt32(txtYearTo.Text) - Convert.ToInt32(txtYearFrom.Text)) > 2)
            {
                MessageBox.Show("Report Generation can only accept up to three (3) inclusive years.");
                return;
            }

            if (MessageBox.Show("Inclusive Years will be Tax Year: "  + sYear1+ " up to Tax Year: " + sMaxYear + " \nContinue?", "Summary of Economic Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                frmBussReport BussReport = new frmBussReport();
                BussReport.BrgyName = cmbBrgy.Text.ToString();
                //BussReport.YearTo = txtYearTo.Text.Trim();
                //BussReport.YearFrom = txtYearFrom.Text.Trim();
                BussReport.Year1 = sYear1;
                BussReport.Year2 = sYear2;
                BussReport.Year3 = sYear3;
                BussReport.ReportName = "Comparative Gross Revenue Report";
                BussReport.ReportSwitch = "ComparativeGrossRevenue";
                BussReport.ShowDialog();
            }
            
        }
    }
}