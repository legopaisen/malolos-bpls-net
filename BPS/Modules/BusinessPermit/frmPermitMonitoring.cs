using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.SearchBusiness;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.BusinessPermit
{
    public partial class frmPermitMonitoring : Form
    {
        private string m_sOwnCode = string.Empty;
        private string m_sPermitDate = string.Empty;
        private string m_sPermitTime = string.Empty;

        public frmPermitMonitoring()
        {
            InitializeComponent();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (txtTaxyear.Text.Trim() == "")
            {
                MessageBox.Show("Indicate tax year to generate","Permit Monitoring",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }

            UpdateList();
            
        }

        private void UpdateList()
        {
            OracleResultSet pRec = new OracleResultSet();

            dgvList.Rows.Clear();
            int iCnt = 0;
            pRec.Query = "select * from permit_monitoring where tax_year = '" + txtTaxyear.Text + "' order by recd_dt desc, recd_tm desc";
            if (pRec.Execute())
            {
                string sBIN = string.Empty;
                string sBnsNm = string.Empty;
                string sTaxYear = string.Empty;
                string sRcdBy = string.Empty;
                string sRcdDt = string.Empty;
                string sRcdTm = string.Empty;
                string sRelBy = string.Empty;
                string sOwnCode = string.Empty;
                string sOwner = string.Empty;
                string sPermitNo = string.Empty;

                while (pRec.Read())
                {
                    iCnt++;

                    sBIN = pRec.GetString("bin");
                    sBnsNm = pRec.GetString("bns_nm");
                    sTaxYear = pRec.GetString("tax_year");
                    sRcdBy = pRec.GetString("recd_by");
                    sRcdDt = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("recd_dt"));
                    sRcdTm = pRec.GetString("recd_tm");
                    sRelBy = pRec.GetString("released_by");
                    sOwnCode = pRec.GetString("own_code");
                    sOwner = AppSettingsManager.GetBnsOwner(sOwnCode);
                    sPermitNo = pRec.GetString("permit_no");

                    dgvList.Rows.Add(sTaxYear, sBIN, sBnsNm, sOwner, sPermitNo, sRcdBy, sRcdDt, sRcdTm, sRelBy, sOwnCode);
                }
            }
            pRec.Close();
        }

        private void btnTag_Click(object sender, EventArgs e)
        {
            OracleResultSet pCmd = new OracleResultSet();

            if (btnTag.Text == "Tag")
            {
                if (MessageBox.Show("Tag released permit of BIN: " + bin1.GetBin() + " for tax year " + txtBnsTaxYear.Text + "?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (txtRcvdBy.Text.Trim() == "")
                    {
                        MessageBox.Show("Recipient is required", "Permit Monitoring", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    if (txtReleasedBy.Text.Trim() == "")
                    {
                        MessageBox.Show("Released by is required", "Permit Monitoring", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    //pCmd.Query = "insert into permit_monitoring values (";
                    //pCmd.Query += "'" + bin1.GetBin() + "', ";
                    //pCmd.Query += "'" + StringUtilities.HandleApostrophe(txtBnsName.Text.Trim())+ "', ";
                    //pCmd.Query += "'" + m_sOwnCode + "', ";
                    //pCmd.Query += "'" + txtPermitNo.Text + "', ";
                    //pCmd.Query += "'" + txtBnsTaxYear.Text + "', ";
                    //pCmd.Query += "'" + StringUtilities.HandleApostrophe(txtRcvdBy.Text.Trim()) + "', ";
                    //pCmd.Query += " to_date('" + dtpDate.Value.ToShortDateString() + "','MM/dd/yyyy'),";
                    ////pCmd.Query += "'" + txtTime.Text +"', ";
                    //pCmd.Query += "'" + dtpTime.Value.ToLongTimeString() + "', ";
                    //pCmd.Query += "'" + StringUtilities.HandleApostrophe(txtReleasedBy.Text.Trim()) + "',";
                    //pCmd.Query += " to_date('"+ m_sPermitDate +"', 'MM/dd/yyyy'),"; //JARS 20170922
                    //pCmd.Query += "'" + m_sPermitTime + "')"; //JARS 20170922
                    //if (pCmd.ExecuteNonQuery() == 0)
                    //{ }
                    pCmd.Query = "insert into permit_monitoring values (";
                    pCmd.Query += "'" + bin1.GetBin() + "', ";
                    pCmd.Query += "'" + StringUtilities.HandleApostrophe(txtBnsName.Text) + "', ";
                    pCmd.Query += "'" + m_sOwnCode + "', ";
                    pCmd.Query += "'" + txtPermitNo.Text + "', ";
                    pCmd.Query += "'" + txtBnsTaxYear.Text + "', ";
                    pCmd.Query += "'" + StringUtilities.HandleApostrophe(txtRcvdBy.Text.Trim()) + "', ";
                    pCmd.Query += " to_date('" + dtpDate.Value.ToShortDateString() + "','MM/dd/yyyy'),";
                    //pCmd.Query += "'" + txtTime.Text +"', ";
                    pCmd.Query += "'" + dtpTime.Value.ToLongTimeString() + "', ";
                    pCmd.Query += "'" + StringUtilities.HandleApostrophe(txtReleasedBy.Text.Trim()) + "')";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    string sObject = "BIN: " + bin1.GetBin() + "/Permit No.: " + txtPermitNo.Text + "/TaxYear: " + txtTaxyear.Text;

                    if (AuditTrail.InsertTrail("ABBP-T", "permit_monitoring", sObject) == 0)
                    {
                        MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    MessageBox.Show("Permit tagged","Permit Monitoring",MessageBoxButtons.OK,MessageBoxIcon.Information);

                    Clear();

                    UpdateList();
                }
            }
            else
            {
                if (MessageBox.Show("Untag released permit of BIN: " + bin1.GetBin() + " for tax year " + txtTaxyear.Text + "?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pCmd.Query = "delete from permit_monitoring where bin = '" + bin1.GetBin() + "'";
                    pCmd.Query += " and tax_year = '" + txtTaxyear.Text + "'";
                    pCmd.Query += " and permit_no = '" + txtPermitNo.Text + "'";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    string sObject = "BIN: " + bin1.GetBin() + "/Permit No.: " + txtPermitNo.Text + "/TaxYear: " + txtTaxyear.Text;
                    sObject += "/Rcvd by: " + txtRcvdBy.Text + "/Rcvd dt: " + dtpDate.Value.ToString() + "/Rcvd tm: " + txtTime.Text;

                    if (AuditTrail.InsertTrail("ABBP-T", "permit_monitoring", sObject) == 0)
                    {
                        MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }
                Clear();
                btnTag.Text = "Tag";
            }
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text == "Search")
            {
                if (bin1.txtTaxYear.Text != "" || bin1.txtBINSeries.Text != "")
                {
                    LoadValues();
                }
                else
                {
                    Clear();

                    frmSearchBusiness frmSearchBns = new frmSearchBusiness();
                    frmSearchBns.ModuleCode = "";

                    frmSearchBns.ShowDialog();
                    if (frmSearchBns.sBIN.Length > 1)
                    {
                        bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                        bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();

                        LoadValues();
                    }
                }
            }
            else
            {
                Clear();
                btnSearch.Text = "Search";
            }
        }

        private void LoadValues()
        {
            OracleResultSet pRec = new OracleResultSet();

            pRec.Query = "select * from businesses where bin = '" + bin1.GetBin() + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    txtBnsTaxYear.Text = pRec.GetString("tax_year");
                    txtPermitNo.Text =  pRec.GetString("permit_no");
                    txtBnsName.Text = pRec.GetString("bns_nm");
                    m_sOwnCode = pRec.GetString("own_code");
                    txtOwnName.Text = AppSettingsManager.GetBnsOwner(m_sOwnCode);
                    m_sPermitDate = pRec.GetDateTime("permit_dt").ToString("MM/dd/yyyy");

                    btnSearch.Text = "Clear";
                    btnTag.Text = "Tag";

                    // RMC 20180118 added enabling/disablng of permit tagging if bin has no generated permit  (s)
                    if (txtPermitNo.Text.Trim() == "")
                    {
                        btnTag.Enabled = false;
                        btnRemove.Enabled = false;
                    }
                    else
                    {
                        btnTag.Enabled = true;
                        btnRemove.Enabled = true;
                    }
                    // RMC 20180118 added enabling/disablng of permit tagging if bin has no generated permit (e)

                    LoadTag();
                }
            }
            pRec.Close();

            pRec.Query = "select to_char(tdatetime, 'HH:MM:SS') as permit_time from a_trail where mod_code = 'ABBP' and object like '%"+bin1.GetBin()+"%' and to_char(tdatetime, 'YYYY') = '"+txtBnsTaxYear.Text+"'"; //JARS 20170922
            if(pRec.Execute())
            {
                if(pRec.Read())
                {
                    m_sPermitTime = pRec.GetString("permit_time");
                }
            }
            pRec.Close();
        }

       
        private void Clear()
        {
            m_sOwnCode = "";
            txtBnsTaxYear.Text = "";
            txtPermitNo.Text = "";
            txtBnsName.Text = "";
            txtOwnName.Text = "";
            txtRcvdBy.Text = "";
            dtpDate.Value = AppSettingsManager.GetCurrentDate();
            //dtpTime.Value = AppSettingsManager.GetCurrentDate();
            txtTime.Text = "";
            bin1.txtBINSeries.Text = "";
            bin1.txtTaxYear.Text = "";
            txtReleasedBy.Text = "";
            btnTag.Text = "Tag";
        }

        private void LoadTag()
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from permit_monitoring where bin = '" + bin1.GetBin() +"' and tax_year = '" + txtTaxyear.Text + "'";
            pSet.Query += " and permit_no = '" + txtPermitNo.Text + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    txtRcvdBy.Text = pSet.GetString("recd_by");
                    txtReleasedBy.Text = pSet.GetString("released_by");
                    dtpDate.Value = pSet.GetDateTime("recd_dt");
                    txtTime.Text = pSet.GetString("recd_tm");
                    DateTime dtpTm;
                    DateTime.TryParse(pSet.GetString("recd_tm"), out dtpTm);
                    dtpTime.Value = dtpTm;

                    btnTag.Text = "Untag";

                    btnRemove.Enabled = false;  // RMC 20180118 added tool to remove wrong bin generated with permit (Malolos request)
                }
                else
                {
                    // RMC 20180118 added tool to remove wrong bin generated with permit (Malolos request) (s)
                    if (txtPermitNo.Text.Trim() != "")
                        btnRemove.Enabled = true;
                    // RMC 20180118 added tool to remove wrong bin generated with permit (Malolos request) (e)
                }
            }
            pSet.Close();
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string sBIN = dgvList[1, e.RowIndex].Value.ToString();
                string sOwnCode = dgvList[9, e.RowIndex].Value.ToString();
                string sRcdDt = dgvList[6, e.RowIndex].Value.ToString();
                string sRcdTm = dgvList[7, e.RowIndex].Value.ToString();

                bin1.txtTaxYear.Text = sBIN.Substring(7, 4).ToString();
                bin1.txtBINSeries.Text = sBIN.Substring(12, 7).ToString();

                txtBnsTaxYear.Text = dgvList[0, e.RowIndex].Value.ToString();
                txtBnsName.Text = dgvList[2, e.RowIndex].Value.ToString();
                txtOwnName.Text = dgvList[3, e.RowIndex].Value.ToString();
                txtPermitNo.Text = dgvList[4, e.RowIndex].Value.ToString();
                txtRcvdBy.Text = dgvList[5, e.RowIndex].Value.ToString(); 
                txtReleasedBy.Text = dgvList[8, e.RowIndex].Value.ToString();

                DateTime dtpTmpDate;
                DateTime.TryParse(sRcdDt, out dtpTmpDate);
                //dtpDate.Value = dtpTmpDate;
                //DateTime.TryParse(sRcdTm, out dtpTmpDate);
                txtTime.Text = sRcdTm;
                DateTime.TryParse(sRcdTm, out dtpTmpDate);
                dtpTime.Value = dtpTmpDate;

                btnTag.Text = "Untag";
                btnSearch.Text = "Clear";
            }
            catch { }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPermitMonitoring_Load(object sender, EventArgs e)
        {
            txtTaxyear.Text = AppSettingsManager.GetConfigValue("12");
            UpdateList();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        { 
            // RMC 20180118 added tool to remove wrong bin generated with permit (Malolos request)
            OracleResultSet pCmd = new OracleResultSet();

            if (MessageBox.Show("Remove generated permit of BIN: " + bin1.GetBin() + " for tax year " + txtBnsTaxYear.Text + "?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                pCmd.Query = "update businesses set permit_no = '', permit_dt = '' where bin = '" + bin1.GetBin() + "'";
                if (pCmd.ExecuteNonQuery() == 0)
                { }

                btnSearch.Text = "Search";

                MessageBox.Show("Permit removed", "Permit Monitoring", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string sObject = "BIN: " + bin1.GetBin() + "/Permit No.: " + txtPermitNo.Text + "/TaxYear: " + txtTaxyear.Text;

                if (AuditTrail.InsertTrail("ABBP-R", "permit_monitoring", sObject) == 0)
                {
                    MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Permit tagged", "Permit Monitoring", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Clear();

                UpdateList();
            }
        }
    }
}