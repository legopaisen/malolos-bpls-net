using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace Amellar.Modules.BusinessMapping
{
    public partial class frmEncoderReport : Form
    {
        public frmEncoderReport()
        {
            InitializeComponent();
        }

        private void frmEncoderReport_Load(object sender, EventArgs e)
        {
            chkOfficial.Checked = true;
            LoadEncoder();
            LoadBarangay();
        }

        private void LoadEncoder()
        {
            OracleResultSet pRec = new OracleResultSet();

            cmbEncoder.Items.Clear();

            if(chkOfficial.Checked == true)
                pRec.Query = "select distinct(bns_user) from btm_businesses order by bns_user";
            else
                pRec.Query = "select distinct(bns_user) from btm_temp_businesses order by bns_user";
            if (pRec.Execute())
            {
                cmbEncoder.Items.Add("ALL");

                while (pRec.Read())
                {
                    cmbEncoder.Items.Add(pRec.GetString(0));
                }
            }
            pRec.Close();
        }

        private void LoadBarangay()
        {
            OracleResultSet pRec = new OracleResultSet();

            cmbBrgy.Items.Clear();

            pRec.Query = "select brgy_nm from brgy order by brgy_code";
            if (pRec.Execute())
            {
                cmbBrgy.Items.Add("ALL");

                while (pRec.Read())
                {
                    cmbBrgy.Items.Add(pRec.GetString(0));
                }
            }
            pRec.Close();
             
        }

        private void chkOfficial_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkOfficial.CheckState.ToString() == "Checked")
            {
                chkUnofficial.Checked = false;
            }

            LoadEncoder();
        }

        private void chkUnofficial_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkUnofficial.CheckState.ToString() == "Checked")
            {
                chkOfficial.Checked = false;
            }
            LoadEncoder();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cmbEncoder.Text.Trim() == "")
            {
                MessageBox.Show("Select encoder", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (cmbBrgy.Text.Trim() == "")
            {
                MessageBox.Show("Select barangay", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            OracleResultSet pRec = new OracleResultSet();
            string sEncoder = "";
            string sBrgy = "";
            string sDateFr = string.Format("{0:MM/dd/yyyy}", dtpDateFr.Value);
            string sDateTo = string.Format("{0:MM/dd/yyyy}", dtpDateTo.Value);
            int iCnt = 0;

            if (cmbEncoder.Text == "ALL")
                sEncoder = "%";
            else
                sEncoder = cmbEncoder.Text.Trim();
            if (cmbBrgy.Text == "ALL")
                sBrgy = "%";
            else
                //sBrgy = txtBrgyCode.Text.Trim();
                sBrgy = cmbBrgy.Text.Trim();

            //EncoderReport ReportClass = new EncoderReport();
            frmReportTest ReportClass = new frmReportTest();

            if(chkOfficial.Checked == true)
            {
                pRec.Query = "select count(*) from btm_businesses where bns_user like '" + sEncoder + "'";
                pRec.Query+= " and bns_brgy like '" + sBrgy + "'";
                pRec.Query+= " and trunc(save_tm) between to_date('" + sDateFr + "','MM/dd/yyyy') and to_date('" + sDateTo + "','MM/dd/yyyy') ";
                int.TryParse(pRec.ExecuteScalar(), out iCnt);

                if(iCnt == 0)
                {
                    MessageBox.Show("No record found.","Business Mapping",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                pRec.Query = "select bns_brgy, bns_user, bin, bns_nm, trunc(save_tm) from btm_businesses";
                pRec.Query += " where bns_user like '" + sEncoder + "'";
                pRec.Query += " and bns_brgy like '" + sBrgy + "'";
                pRec.Query += " and trunc(save_tm) between to_date('" + sDateFr + "','MM/dd/yyyy') and to_date('" + sDateTo + "','MM/dd/yyyy') ";
                pRec.Query += " order by bns_brgy, save_tm, bns_user, bin ";

                //ReportClass.Official = true;
                ReportClass.ReportName = "OFFICIAL BUSINESS";
            }
            else if(chkUnofficial.Checked == true)
            {
                pRec.Query = "select count(*) from btm_temp_businesses where bns_user like '" + sEncoder + "'";
                pRec.Query+= " and bns_brgy like '" + sBrgy + "'";
                pRec.Query += " and trunc(save_tm) between to_date('" + sDateFr + "','MM/dd/yyyy') and to_date('" + sDateTo + "','MM/dd/yyyy') ";
                int.TryParse(pRec.ExecuteScalar(), out iCnt);

                if (iCnt == 0)
                {
                    MessageBox.Show("No record found.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                pRec.Query = "select bns_brgy, bns_user, tbin, bns_nm, trunc(save_tm) from btm_temp_businesses where bns_user like '" + sEncoder + "'";
                pRec.Query += " and bns_brgy like '" + sBrgy + "'";
                pRec.Query += " and trunc(save_tm) between to_date('" + sDateFr + "','MM/dd/yyyy') and to_date('" + sDateTo + "','MM/dd/yyyy') ";
                pRec.Query += " order by bns_brgy, save_tm, bns_user, tbin ";

                //ReportClass.Official = false;
                ReportClass.ReportName = "UNOFFICIAL BUSINESS";

            }

            
            ReportClass.Query = pRec.Query.ToString();
            ReportClass.DateFrom = sDateFr;
            ReportClass.DateTo = sDateTo;
            //ReportClass.FormLoad();
            ReportClass.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbBrgy_SelectedValueChanged(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();

            txtBrgyCode.Text = "";

            pRec.Query = "select * from brgy where brgy_nm = '" + cmbBrgy.Text.Trim() + "'";
            if(pRec.Execute())
            {
                if(pRec.Read())
                {
                    txtBrgyCode.Text = pRec.GetString("brgy_code").Trim();
                }
            }
            pRec.Close();
        }
    }
}