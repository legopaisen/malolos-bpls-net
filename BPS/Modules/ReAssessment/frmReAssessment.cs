using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.Reports;
using Amellar.Common.AuditTrail;
using Amellar.Common.StringUtilities;
using Amellar.Modules.BusinessReports;
namespace ReAssessment
{
    public partial class frmReAssessment : Form
    {
        string m_strBIN = string.Empty;
        public frmReAssessment()
        {
            InitializeComponent();
            bin1.GetLGUCode = AppSettingsManager.GetConfigObject(ConfigurationAttributes.LGUCode);
            bin1.GetDistCode = AppSettingsManager.GetConfigObject("11");
            bin1.Enabled = false;
            cmbBrgy.Enabled = false;
            chkFilter.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmReAssessment_Load(object sender, EventArgs e)
        {
            LoadBrgy();
        }

        private void LoadBrgy()
        {
            cmbBrgy.Items.Clear();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from brgy order by brgy_nm";
            if (result.Execute())
            {
                cmbBrgy.Items.Add("ALL");
                while (result.Read())
                    cmbBrgy.Items.Add(result.GetString("brgy_nm").Trim());
            }
            result.Close();
        }

        private void PopulateList(string sBnsBrgy)
        {
            dgvBnsList.Rows.Clear();
            if (sBnsBrgy == "ALL")
                sBnsBrgy = "%";
            OracleResultSet result = new OracleResultSet();
            int iIsTag = 0;
            string sBin = string.Empty;
            result.Query = "select * from REASS_WATCH_LIST where bin in (select bin from businesses where bns_brgy like '" + sBnsBrgy + "%') order by bns_nm";
            if (result.Execute())
            {
                while (result.Read())
                {
                    sBin = result.GetString("bin").Trim();
                    iIsTag = result.GetInt("is_tag");
                    if(iIsTag == 1)
                        dgvBnsList.Rows.Add(true, sBin, AppSettingsManager.GetBnsName(sBin), AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(sBin)), string.Format("{0:#,##0.#0}", AppSettingsManager.GetCompoundedGross(sBin)), AppSettingsManager.GetCurrQtrPaid(sBin)); 
                    else
                        dgvBnsList.Rows.Add(false, sBin, AppSettingsManager.GetBnsName(sBin), AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(sBin)), string.Format("{0:#,##0.#0}", AppSettingsManager.GetCompoundedGross(sBin)), AppSettingsManager.GetCurrQtrPaid(sBin)); 

                }
            }
            result.Close();

            lblNoBns.Text = "Total number of business: " + string.Format("{0:#,##0}", dgvBnsList.RowCount);

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            Generate();
            PopulateList(cmbBrgy.Text.Trim());
            if (AuditTrail.InsertTrail("REASS-GEN", "REASS_WATCH_LIST", cmbBrgy.Text.Trim()) == 0)
            {
                MessageBox.Show("Error retrieving records");
                return;
            }
        }

        private void Generate()
        {
            double dGross = 0;
            string sBin = string.Empty;
            string sTaxYear = string.Empty;
            string sBnsNm = string.Empty;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            result.Query = "select * from businesses where bns_stat = 'REN' and bin not in (select bin from REASS_WATCH_LIST union select bin from APP_REASS) order by bin";
            if (result.Execute())
            {
                while (result.Read())
                {
                    sBin = result.GetString("bin").Trim();
                    sTaxYear = result.GetString("tax_year").Trim();
                    sBnsNm = result.GetString("bns_nm").Trim();
                    //dGross = AppSettingsManager.GetCompoundedGross(sBin);
                    //if (dGross >= 600000.00)
                    //{
                    result2.Query = "insert into REASS_WATCH_LIST values (:1, :2, :3, :4)";
                    result2.AddParameter(":1", sBin);
                    result2.AddParameter(":2", sTaxYear);
                    result2.AddParameter(":3", 0);
                    result2.AddParameter(":4", sBnsNm);
                    if (result2.ExecuteNonQuery() != 0)
                    { }
                    result2.Close();
                    //}
                }
            }
            result.Close();
        }

        private void btnTag_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();
            string sBin = string.Empty;
            string sTaxyear = string.Empty;
            string sBnsName = string.Empty;

            for (int i = 0; i < dgvBnsList.RowCount; i++)
            {
                if ((bool)dgvBnsList.Rows[i].Cells[0].Value)
                {
                    sBin = dgvBnsList.Rows[i].Cells[1].Value.ToString();
                    sBnsName = dgvBnsList.Rows[i].Cells[2].Value.ToString();
                    sTaxyear = dgvBnsList.Rows[i].Cells[5].Value.ToString();
                    if (!isTAGGED(sBin, sTaxyear))
                    {
                        result.Query = "insert into REASS_WATCH_LIST values(:1,:2,1,:3)";
                        result.AddParameter(":1", sBin);
                        result.AddParameter(":2", sTaxyear);
                        result.AddParameter(":3", StringUtilities.HandleApostrophe(sBnsName));
                        if (result.ExecuteNonQuery() != 0)
                        { }
                        result.Close();
                    }
                }
            }

            MessageBox.Show("Tagging successfully.");
            btnClear.PerformClick();
        }

        private void btnUnTag_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();
            string sBin = string.Empty;
            string sTaxyear = string.Empty;
            for (int i = 0; i < dgvBnsList.RowCount; i++)
            {
                if ((bool)dgvBnsList.Rows[i].Cells[0].Value)
                {
                    if (isTAGGED(sBin, sTaxyear))
                    {
                        sBin = dgvBnsList.Rows[i].Cells[1].Value.ToString();
                        sTaxyear = dgvBnsList.Rows[i].Cells[5].Value.ToString();
                        result.Query = "delete from REASS_WATCH_LIST where bin = :1 and tax_year = :2";
                        result.AddParameter(":1", sBin);
                        result.AddParameter(":2", sTaxyear);
                        if (result.ExecuteNonQuery() != 0)
                        { }
                        result.Close();
                    }
                }
            }

            MessageBox.Show("Selected bin(s) successfully untagged.");
            btnClear.PerformClick();
        }

        private void dgvBnsList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                m_strBIN = dgvBnsList.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtBnsAdd.Text = AppSettingsManager.GetBnsAdd(dgvBnsList.Rows[e.RowIndex].Cells[1].Value.ToString(), "");
                txtOwnAdd.Text = AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetOwnCode(dgvBnsList.Rows[e.RowIndex].Cells[1].Value.ToString()));
            }
        }

        private void chkAll_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkAll.Checked)
            {
                for (int i = 0; i < dgvBnsList.RowCount; i++)
                {
                    dgvBnsList.Rows[i].Cells[0].Value = true;
                }
            }
            else
            {
                for (int i = 0; i < dgvBnsList.RowCount; i++)
                {
                    dgvBnsList.Rows[i].Cells[0].Value = false;
                }
            }
        }

        private void btnIssueNotice_Click(object sender, EventArgs e)
        {
            ReportClass rClass = new ReportClass();
            rClass.i_PageCount = dgvBnsList.RowCount;
            for (int x = 0; x < dgvBnsList.RowCount; x++)
            {
                rClass.Reassessment(dgvBnsList.Rows[x].Cells[1].Value.ToString());
                if (AuditTrail.InsertTrail("REASS-ISSUE-NOTICE", "REASS_WATCH_LIST", dgvBnsList.Rows[x].Cells[1].Value.ToString()) == 0)
                {
                    MessageBox.Show("Error generating reports");
                    return;
                }
            }
            
            rClass.PreviewDocu();
        }

        private void cmbBrgy_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateList(cmbBrgy.Text.Trim());
        }

        private void dgvBnsList_DoubleClick(object sender, EventArgs e)
        {
            /*ReportClass rClass = new ReportClass();
            rClass.Reassessment(m_strBIN.Trim());
            rClass.PreviewDocu();*/
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sQuery = "";
            bool isChecked = false;
            object oSelectedRB = null;
            foreach (object c in this.gbFilter.Controls)
            {
                if (c is RadioButton)
                    if (((RadioButton)c).Checked)
                    {
                        isChecked = true;
                        oSelectedRB = c;
                        break;
                    }
            }

            if (!isChecked)
            {
                MessageBox.Show("Select Filter by");
                return;
            }

            if (oSelectedRB.Equals(rdobyBrgy))
            {
                String sBrgy = cmbBrgy.Text.Trim();
                if (sBrgy == "ALL")
                    sBrgy = "%%";
                else
                    sBrgy = StringUtilities.HandleApostrophe(cmbBrgy.Text.Trim()) + "%";

                sQuery = @"select * from (
select bin,tax_year,bns_brgy from businesses where bns_stat = 'REN' and bns_brgy like '" + sBrgy + @"'
union all 
select bin,tax_year,bns_brgy from business_que where bns_stat = 'REN' and bns_brgy like '" + sBrgy + @"'
union all 
select bin,tax_year,bns_brgy from buss_hist where bns_stat = 'REN' and bns_brgy like '" + sBrgy + @"') a ";

                if (chkFilter.Checked)
                    sQuery += "inner join REASS_WATCH_LIST rwl on rwl.bin = a.bin and rwl.tax_year = a. tax_year ";

                if (txtTaxYear.Text.Trim() != "")
                    sQuery += "where a.tax_year = '" + StringUtilities.HandleApostrophe(txtTaxYear.Text.Trim()) + "'";

            }
            else if (oSelectedRB.Equals(rdobyBin))
            {
                sQuery = @"select * from (
select bin,tax_year,bns_brgy from businesses where bns_stat = 'REN'
union all 
select bin,tax_year,bns_brgy from business_que where bns_stat = 'REN'
union all 
select bin,tax_year,bns_brgy from buss_hist where bns_stat = 'REN') a where a.bin = '" + bin1.GetBin() + "'";

                if (txtTaxYear.Text.Trim() != "")
                    sQuery += " and a.tax_year = '" + StringUtilities.HandleApostrophe(txtTaxYear.Text.Trim()) + "'";
            }
            else
                return;
            sQuery += " order by a.bin,a.tax_year";

            dgvBnsList.Rows.Clear();
            OracleResultSet result = new OracleResultSet();
            string sBin = string.Empty;
            string sTaxyear = string.Empty;
            result.Query = sQuery;
            if (result.Execute())
            {
                while (result.Read())
                {
                    sBin = result.GetString("bin").Trim();
                    sTaxyear = result.GetString("tax_year").Trim();
                    if (chkFilter.Checked)// just to avoid executing dataconnection for the next condition
                        dgvBnsList.Rows.Add(true, sBin, AppSettingsManager.GetBnsName(sBin), AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(sBin)), string.Format("{0:#,##0.#0}", AppSettingsManager.GetCompoundedGross(sBin, sTaxyear)), sTaxyear);
                    else if (isTAGGED(sBin, sTaxyear))
                        dgvBnsList.Rows.Add(true, sBin, AppSettingsManager.GetBnsName(sBin), AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(sBin)), string.Format("{0:#,##0.#0}", AppSettingsManager.GetCompoundedGross(sBin, sTaxyear)), sTaxyear);
                    else
                        dgvBnsList.Rows.Add(false, sBin, AppSettingsManager.GetBnsName(sBin), AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(sBin)), string.Format("{0:#,##0.#0}", AppSettingsManager.GetCompoundedGross(sBin, sTaxyear)), sTaxyear);
                }
            }
            result.Close();

            if (dgvBnsList.RowCount > 0)
                dgvBnsList.Rows[0].Cells[1].Selected = true;

            lblNoBns.Text = "Total number of business: " + string.Format("{0:#,##0}", dgvBnsList.RowCount);
        }

        private void rdobyBin_CheckedChanged(object sender, EventArgs e)
        {
            if (sender.Equals(rdobyBin))
            {
                bin1.Enabled = true;
                bin1.txtTaxYear.Focus();
                cmbBrgy.Enabled = false;
                chkFilter.Checked = false;
                chkFilter.Enabled = false;
            }
            else if(sender.Equals(rdobyBrgy))
            {
                bin1.Enabled = false;
                bin1.txtTaxYear.Text = "";
                bin1.txtBINSeries.Text = "";
                cmbBrgy.Enabled = true;
                chkFilter.Enabled = true;
            }
        }

        private void cmbBrgy_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private bool isTAGGED(string sBin, string sTaxYear)
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * From REASS_WATCH_LIST where bin = '" + sBin + "' and tax_year = '" + sTaxYear + "' and is_tag = 1";
            if(pSet.Execute())
                if (pSet.Read())
                {
                    pSet.Close();
                    return true;
                }
            pSet.Close();
            return false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvBnsList.RowCount > 0)
            {
                frmBussReport dlg = new frmBussReport();
                if (txtTaxYear.Text.Trim() != "")
                    dlg.m_sTaxYear = txtTaxYear.Text;
                if (rdobyBrgy.Checked)
                    dlg.SwitchFilter = 1;
                else
                    dlg.SwitchFilter = 2;
                dlg.BIN = bin1.GetBin();
                dlg.BrgyName = cmbBrgy.Text.Trim();
                dlg.ReportSwitch = "Re-assessment";
                dlg.ReportName = "List of Re-Assessed Businesses";
                dlg.ShowDialog();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dgvBnsList.Rows.Clear();
            txtBnsAdd.Text = "";
            txtOwnAdd.Text = "";
            lblNoBns.Text = "Total number of business: ";
            bin1.txtBINSeries.Text = "";
            bin1.txtTaxYear.Text = "";
            cmbBrgy.Text = "";
            txtTaxYear.Text = "";
            chkFilter.Checked = false;
            rdobyBin.Checked = false;
            rdobyBrgy.Checked = false;
        }
    }
}