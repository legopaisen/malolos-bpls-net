using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Modules.BusinessReports;
using Amellar.Common.AppSettings;

namespace CDOReport
{
    public partial class frmCDOReport : Form
    {
        frmBussReport fBussReport = new frmBussReport();
        public frmCDOReport()
        {
            InitializeComponent();
        }

        private void LoadBrgy()
        {

            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pGisRec = new OracleResultSet();
            string m_sGisBrgy = "";
            cmbBrgy.Items.Clear();

            string sBrgyCode = "";

            try
            {
                pGisRec.CreateNewConnectionGIS();
                pGisRec.Query = "select distinct substr(pin,8,3) from GIS_BUSINESS_LOCATION order by substr(pin,8,3)";
                if (pGisRec.Execute())
                {
                    cmbBrgy.Items.Add("");
                    while (pGisRec.Read())
                    {
                        sBrgyCode = pGisRec.GetString(0);

                        pRec.Query = string.Format("select * from brgy where brgy_code = '{0}'", sBrgyCode);
                        if (pRec.Execute())
                        {
                            if (pRec.Read())
                            {
                                cmbBrgy.Items.Add(pRec.GetString("brgy_nm"));
                            }
                        }
                        pRec.Close();
                    }
                }
                pGisRec.Close();

            }
            catch {
                // RMC 20140724 corrected error in BUsiness Mapping if no GIS (s)
                cmbBrgy.Items.Add("");
                pRec.Query = string.Format("select * from brgy order by brgy_code");
                if (pRec.Execute())
                {
                    while (pRec.Read())
                    {
                        cmbBrgy.Items.Add(pRec.GetString("brgy_nm"));
                    }
                }
                pRec.Close();
                // RMC 20140724 corrected error in BUsiness Mapping if no GIS (e)
            }

            
            

            
        }

        private void frmCDOReport_Load(object sender, EventArgs e)
        {
            LoadBrgy();
            LoadUser();
        }

        private void LoadUser()
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select distinct(a.tagged_by)  from norec_closure_tagging a union select distinct(a.tagged_by)  from official_closure_tagging a";
            if (result.Execute())
            {
                while (result.Read())
                    cmbUser.Items.Add(result.GetString("tagged_by").Trim());
            }
            result.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            
            string sTBIN = string.Empty;
            string sReportUser = string.Empty;
            OracleResultSet result = new OracleResultSet();
            //fBussReport.ReportSwitch = "CDO Report";
            //fBussReport.sBrgy = cmbBrgy.Text.Trim();
            //fBussReport.m_dFrom = dtFrom.Value;
            //fBussReport.m_dTo = dtTo.Value;
            //fBussReport.sUser = cmbUser.Text.Trim();
            //fBussReport.ShowDialog();

            //if (cmbUser.Text.Trim() == string.Empty)
            //    sReportUser = "%";
            //else
            //    sReportUser = cmbUser.Text.Trim();
            /*
            if (MessageBox.Show("Move Printed Records to archive?", "Archiving Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OracleResultSet result = new OracleResultSet();
                OracleResultSet result2 = new OracleResultSet();
                if(chkOfficial.Checked == true && chkUnoff.Checked == false)
                    result.Query = "select bin from official_closure_tagging where tdatetime between '" + string.Format("{0:dd-MMM-yy}", dtFrom.Value) + "' and '" + string.Format("{0:dd-MMM-yy}", dtTo.Value) + "' and tagged_by like '%" + sReportUser + "%'";
                if (chkOfficial.Checked == false && chkUnoff.Checked == true)
                    result.Query = "select is_number as bin from norec_closure_tagging where tdatetime between '" + string.Format("{0:dd-MMM-yy}", dtFrom.Value) + "' and '" + string.Format("{0:dd-MMM-yy}", dtTo.Value) + "' and tagged_by like '%" + sReportUser + "%'";
                if (chkOfficial.Checked == true && chkUnoff.Checked == true)
                    result.Query = "select bin from official_closure_tagging where tdatetime between '" + string.Format("{0:dd-MMM-yy}", dtFrom.Value) + "' and '" + string.Format("{0:dd-MMM-yy}", dtTo.Value) + "' and tagged_by like '%" + sReportUser + "%' union select is_number as bin from norec_closure_tagging where tdatetime between '" + string.Format("{0:dd-MMM-yy}", dtFrom.Value) + "' and '" + string.Format("{0:dd-MMM-yy}", dtTo.Value) + "' and tagged_by like '%" + sReportUser + "%'";
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        sTBIN = result.GetString("bin").Trim();
                        result2.Query = "insert into PRINTED_CDO_UNOF values ('" + sTBIN + "','" + AppSettingsManager.g_objSystemUser.UserCode + "','" + string.Format("{0:dd-MMM-yy}", DateTime.Now) + "')";
                        if (result2.ExecuteNonQuery() != 0)
                        { }
                        result2.Close();
                    }
                }
                result.Close();
            }
             */
            frmReport fReport = new frmReport();
            fReport.sUser = cmbUser.Text.Trim();
            fReport.dtFrom = dtFrom.Value;
            fReport.dtTo = dtTo.Value;
            fReport.sBrgy = cmbBrgy.Text.Trim();
            if (chkOfficial.Checked)
                fReport.bOfficial = true;
            else
                fReport.bOfficial = false;

            if (chkUnoff.Checked)
                fReport.bUnOfficial = true;
            else
                fReport.bUnOfficial = false;

            //fReport.dgvCDO.Rows.Clear();

            string sDateFrom = string.Empty;
            string sDateTo = string.Empty;

            sDateFrom = string.Format("{0:dd-MMM-yyyy}", dtFrom.Value);
            sDateTo = string.Format("{0:dd-MMM-yyyy}", dtTo.Value);
            string sBrgy = string.Empty;
            string sTaggedBy = string.Empty;

            if (cmbBrgy.Text.Trim() == string.Empty)
                sBrgy = "%";
            else
                sBrgy = cmbBrgy.Text.Trim();

            if (cmbUser.Text.Trim() == string.Empty)
                sReportUser = "%";
            else
                sReportUser = cmbUser.Text.Trim();
            string sQuery = string.Empty;

            /*
            if (chkOfficial.Checked == true && chkUnoff.Checked == false)
                sQuery = "select distinct(bin), tdatetime from official_closure_tagging where tdatetime between '" + string.Format("{0:dd-MMM-yy}", dtFrom.Value) + "' and '" + string.Format("{0:dd-MMM-yy}", dtTo.Value) + "' and tagged_by like '%" + sReportUser + "%' and trim(bin) in (select bin from btm_businesses where bns_brgy = '" + cmbBrgy.Text.Trim() + "')";
            if (chkOfficial.Checked == false && chkUnoff.Checked == true)
                sQuery = "select distinct(is_number) as bin, tdatetime from norec_closure_tagging where tdatetime between '" + string.Format("{0:dd-MMM-yy}", dtFrom.Value) + "' and '" + string.Format("{0:dd-MMM-yy}", dtTo.Value) + "' and tagged_by like '%" + sReportUser + "%' and trim(is_number) in (select tbin from btm_temp_businesses where bns_brgy = '" + cmbBrgy.Text.Trim() + "')";
            if (chkOfficial.Checked == true && chkUnoff.Checked == true)
                sQuery = "select distinct(bin), tdatetime from official_closure_tagging where tdatetime between '" + string.Format("{0:dd-MMM-yy}", dtFrom.Value) + "' and '" + string.Format("{0:dd-MMM-yy}", dtTo.Value) + "' and tagged_by like '%" + sReportUser + "%' and bin in (select bin from btm_businesses where bns_brgy = '" + cmbBrgy.Text.Trim() + "') union select is_number as bin, tdatetime from norec_closure_tagging where tdatetime between '" + string.Format("{0:dd-MMM-yy}", dtFrom.Value) + "' and '" + string.Format("{0:dd-MMM-yy}", dtTo.Value) + "' and tagged_by like '%" + sReportUser + "%' and trim(is_number) in (select tbin from btm_temp_businesses where bns_brgy = '" + cmbBrgy.Text.Trim() + "')";
             */

            if (chkOfficial.Checked == true && chkUnoff.Checked == false)
                sQuery = "select distinct(bin), tdatetime from official_closure_tagging where to_date(tdatetime) between '" + sDateFrom + "' and '" + sDateTo + "' and tagged_by like '%" + sReportUser + "%' and trim(bin) in (select bin from btm_businesses where bns_brgy like '" + cmbBrgy.Text.Trim() + "%') union select distinct(bin), tdatetime from official_closure_tagging where to_date(tdatetime) between '" + sDateFrom + "' and '" + sDateTo + "' and tagged_by like '%" + sReportUser + "%' and trim(bin) in (select bin from btm_temp_businesses where bns_brgy like '" + cmbBrgy.Text.Trim() + "%')";
            if (chkOfficial.Checked == false && chkUnoff.Checked == true)
                sQuery = "select distinct(is_number) as bin, tdatetime from norec_closure_tagging where to_date(tdatetime) between '" + sDateFrom + "' and '" + sDateTo + "' and tagged_by like '%" + sReportUser + "%' and trim(is_number) in (select tbin from btm_temp_businesses where bns_brgy like '" + cmbBrgy.Text.Trim() + "%')";
            if (chkOfficial.Checked == true && chkUnoff.Checked == true)
                sQuery = "select distinct(bin), tdatetime from official_closure_tagging where to_date(tdatetime) between '" + sDateFrom + "' and '" + sDateTo + "' and tagged_by like '%" + sReportUser + "%' and bin in (select bin from btm_businesses where bns_brgy like '" + cmbBrgy.Text.Trim() + "%') union select is_number as bin, tdatetime from norec_closure_tagging where to_date(tdatetime) between '" + sDateFrom + "' and '" + sDateTo + "' and tagged_by like '%" + sReportUser + "%' and trim(is_number) in (select tbin from btm_temp_businesses where bns_brgy like '" + cmbBrgy.Text.Trim() + "%')";
            //if (result.Execute())
            //{
            //    while (result.Read())
            //    {
                    //sTBIN = result.GetString("bin").Trim();
                    //result2.Query = "insert into PRINTED_CDO_UNOF values ('" + sTBIN + "','" + AppSettingsManager.g_objSystemUser.UserCode + "','" + string.Format("{0:dd-MMM-yy}", DateTime.Now) + "')";
                    //if (result2.ExecuteNonQuery() != 0)
                    //{ }
                    //result2.Close();

              ///      fReport.dgvCDO.Rows.Add(true, result.GetString("bin").Trim(), AppSettingsManager.GetBnsName(result.GetString("bin").Trim()), AppSettingsManager.GetBnsAdd(result.GetString("bin").Trim()), AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(result.GetString("bin").Trim())), result.GetDateTime("tdatetime").ToShortDateString());
             //   }
            //}
            //result.Close();
            fReport.m_sQuery = sQuery;
            fReport.ShowDialog();

        }

        private void chkOfficial_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkOfficial.Checked)
                fBussReport.bOfficial = true;
            else
                fBussReport.bOfficial = false;
        }

        private void chkUnoff_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkUnoff.Checked)
                fBussReport.bUnofficial = true;
            else
                fBussReport.bUnofficial = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frameWithShadow2_Load(object sender, EventArgs e)
        {

        }

        private void kryptonHeader2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}