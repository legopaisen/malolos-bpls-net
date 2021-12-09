using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.SearchBusiness;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Modules.PaymentHistory;
using Amellar.Modules.BusinessReports;

namespace Amellar.Modules.EPS
{
    public partial class frmEPS : Form
    {
        private string m_sModuleCode = string.Empty;
        private string m_strModule = string.Empty;
        private string m_bnStat = string.Empty;
        private bool m_blnIsPaid = false;
        private string m_sBin = string.Empty;
        int iRow = 0;

        public frmEPS()
        {
            InitializeComponent();
        }

        private void frmEPS_Load(object sender, EventArgs e)
        {
            btnPrintTrail.Visible = false; //AFM temporarily hidden; no rms received yet as of 20211006
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
            m_strModule = "BILLING";
            DatagridStruct();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sBns = "";

            if (btnSearch.Text == "&Search")
            {
                btnSearch.Text = "&Clear";
                if (bin1.txtTaxYear.Text.Trim() == string.Empty && bin1.txtBINSeries.Text.Trim() == string.Empty)
                {
                    frmSearchBusiness frmSearchBns = new frmSearchBusiness();
                    frmSearchBns.ModuleCode = m_strModule;
                    frmSearchBns.ShowDialog();
                    sBns = bin1.GetBin();

                    if (frmSearchBns.sBIN.Length > 1)
                    {
                        bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                        bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
                    }
                    else
                    {
                        bin1.txtTaxYear.Text = "";
                        bin1.txtBINSeries.Text = "";
                        btnSearch.Text = "&Search";
                        bin1.txtTaxYear.Focus();
                        return;
                    }
                }

                sBns = bin1.txtLGUCode.Text.Trim() + "-" + bin1.txtDistCode.Text.Trim() + "-" + bin1.txtTaxYear.Text.Trim() + "-" + bin1.txtBINSeries.Text.Trim();
                m_sBin = sBns; //AFM 20210719
                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = "Select * from BUSINESS_QUE where bin = '" + sBns + "'";
                if (pSet.Execute())
                    if (!pSet.Read())
                    {
                        if (CheckifPaid())
                        {
                            //dgInspectionFee.Enabled = false; //AFM 20210719 removed to allow edit for next year
                        }
                        else
                        {
                            MessageBox.Show("No record found.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            bin1.txtTaxYear.Text = "";
                            bin1.txtBINSeries.Text = "";
                            btnSearch.Text = "&Search";
                            bin1.txtTaxYear.Focus();
                            m_sBin = string.Empty;
                            return;
                        }
                    }
                pSet.Close();

                txtOwner.Text = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(sBns));
                txtBnsName.Text = AppSettingsManager.GetBnsName(sBns);
                txtBnsAdd.Text = AppSettingsManager.GetBnsAddress(sBns);
                txtTaxYear.Text = AppSettingsManager.GetBnsTaxYear(sBns);
                m_bnStat = AppSettingsManager.GetBnsStat(sBns);
                txtBns_Stat.Text = m_bnStat;
                btnSetApproval.Enabled = true;
                btnViewPayments.Enabled = true;
                btnPrintTrail.Enabled = true;
                getDataGridValue();
            }
            else
            {
                btnSearch.Text = "&Search";
                bin1.txtTaxYear.Text = string.Empty;
                bin1.txtBINSeries.Text = string.Empty;
                txtOwner.Text = string.Empty;
                txtBnsName.Text = string.Empty;
                txtTaxYear.Text = string.Empty;
                txtBnsAdd.Text = string.Empty;
                txtBns_Stat.Text = string.Empty;
                dgInspectionFee.Rows.Clear();
                txtRemarks.Text = string.Empty;
                bin1.txtTaxYear.Focus();
                btnSetApproval.Enabled = false;
                dgvEps.Rows.Clear();
                rdoApprove.Checked = false;
                rdoPennding.Checked = false;
                rdoDisapprove.Checked = false;
                dgInspectionFee.Enabled = true;
                txtTotalAnnualFee.Text = "";
                btnPrintTrail.Enabled = false;
                m_sBin = string.Empty;
            }
        }

        private void DatagridStruct()
        {
            dgvEps.Columns.Clear();

            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
            dgvEps.Columns.Add(chk);
            chk.Width = 30;

            dgvEps.Columns.Add("req_code", "Req Code");
            dgvEps.Columns.Add("req_desc", "Req Description");
            dgvEps.Columns.Add("date_issue", "Date Issuance");

            dgvEps.Columns[1].Width = 40;
            dgvEps.Columns[2].Width = 270;
            dgvEps.Columns[3].Width = 100;
        }

        private void getDataGridValue()
        {
            ArrayList arrAssessment = new ArrayList();
            ArrayList arrDateAssessment = new ArrayList();
            OracleResultSet pSet = new OracleResultSet();
            iRow = 0;
            int iFromTY = 0;
            int iToTY = 0;
            if (AppSettingsManager.bnsIsDiscDelq(bin1.GetBin()) == true)
                iFromTY = Convert.ToInt16(txtTaxYear.Text);
            else
                iFromTY = Convert.ToInt16(txtTaxYear.Text) + 1;
            iToTY = Convert.ToInt16(ConfigurationAttributes.CurrentYear);

            if (iFromTY > iToTY)
                iFromTY = iToTY;

            dgvEps.Rows.Clear();
            string sQuery = string.Format("select distinct req_code, req_desc from eps_tbl order by req_code");
            pSet.Query = sQuery;
            if (pSet.Execute())
                while (pSet.Read())
                {
                    dgvEps.Rows.Add(false, pSet.GetString("req_code").Trim(), StringUtilities.RemoveApostrophe(pSet.GetString("req_desc").Trim()));
                    iRow++;
                }
            pSet.Close();

            bool bOnce = false;
            dgInspectionFee.Rows.Clear();
            double dTAF = 0;
            for (int i = iFromTY; i <= iToTY; i++)
            {
                //pSet.Query = "select * from eps_assess_app where bin = '" + bin1.GetBin() + "' and tax_year = '" + i.ToString() + "'";
                if (!m_blnIsPaid)//AFM 20210719 MAO-21-15404
                    pSet.Query = "select * from eps_assess_app where bin = '" + bin1.GetBin() + "' and tax_year >= '" + i.ToString() + "'";
                else
                    pSet.Query = "select * from eps_assess_app where bin = '" + bin1.GetBin() + "' and tax_year > '" + i.ToString() + "'";

                if (pSet.Execute())
                    while (pSet.Read())
                    {
                        dgInspectionFee.Rows.Add(pSet.GetString(1), pSet.GetDouble(3));
                        dTAF += pSet.GetDouble(3);

                        if (bOnce == false)
                        {
                            bOnce = true;
                            txtRemarks.Text = StringUtilities.RemoveApostrophe(pSet.GetString(4));
                            if (pSet.GetString(2) == "1")
                                rdoApprove.Checked = true;
                            else if (pSet.GetString(2) == "2")
                                rdoPennding.Checked = true;
                            else
                                rdoDisapprove.Checked = true;
                        }
                    }
                pSet.Close();
            }

            txtTotalAnnualFee.Text = dTAF.ToString("#,##0.00");
            if (dgInspectionFee.Rows.Count > 0)
            {
                pSet.Query = "select * from eps_assessment where bin = '" + bin1.GetBin() + "' and from_tax_year = '" + iFromTY + "' and to_tax_year = '" + iToTY + "' order by req_code";
                if (pSet.Execute())
                    while (pSet.Read())
                    {
                        arrAssessment.Add(pSet.GetString(3).Trim());
                        arrDateAssessment.Add(pSet.GetString(4).Trim());
                    }
                pSet.Close();

                for (int h = 0; h < arrAssessment.Count; h++)
                    for (int i = 0; i < dgvEps.RowCount; i++)
                    {
                        if (arrAssessment[h].ToString() == dgvEps.Rows[i].Cells[1].Value.ToString())
                        {
                            dgvEps.Rows[i].Cells[0].Value = true;
                            dgvEps.Rows[i].Cells[3].Value = arrDateAssessment[h].ToString();
                            break;
                        }
                    }
            }
            else
            {
                //MCR 20171212 (s)
                arrAssessment.Clear();
                pSet.Query = @"select bin,to_tax_year, REQ_CODE from eps_assessment where bin = '" + bin1.GetBin() + @"' and TO_TAX_YEAR = 
(SELECT MAX(to_tax_year) FROM eps_assessment where bin = '" + bin1.GetBin() + "') order by req_code";
                if (pSet.Execute())
                    while (pSet.Read())
                        arrAssessment.Add(pSet.GetString(2).Trim());
                pSet.Close();

                for (int h = 0; h < arrAssessment.Count; h++)
                    for (int i = 0; i < dgvEps.RowCount; i++)
                    {
                        if (arrAssessment[h].ToString() == dgvEps.Rows[i].Cells[1].Value.ToString())
                        {
                            dgvEps.Rows[i].Cells[0].Value = true;
                            break;
                        }
                    }
                //MCR 20171212 (e)

                String sTaxYear = "";
                pSet.Query = "select tax_year From businesses where bin = '" + bin1.GetBin() + "'";
                if (pSet.Execute())
                    if (pSet.Read())
                        sTaxYear = pSet.GetString(0);
                    else
                    {
                        pSet.Close();
                        pSet.Query = "select tax_year From business_que where bin = '" + bin1.GetBin() + "'";
                        if (pSet.Execute())
                            if (pSet.Read())
                                sTaxYear = pSet.GetString(0);
                            else
                                sTaxYear = ConfigurationAttributes.CurrentYear;
                    }
                pSet.Close();

                int iBnsYear = 0;
                int iCurYear = 0;
                int iRevYear = 0;

                iBnsYear = Convert.ToInt32(sTaxYear);

                if (AppSettingsManager.bnsIsDiscDelq(bin1.GetBin()) != true)
                    iBnsYear++;

                iCurYear = Convert.ToInt32(ConfigurationAttributes.CurrentYear);
                iRevYear = Convert.ToInt32(ConfigurationAttributes.RevYear);
                if (iBnsYear > iCurYear)
                    iBnsYear = iCurYear;

                if (!m_blnIsPaid)//AFM 20210716 MAO-21-15404
                {
                    for (int i = iBnsYear; i <= iCurYear; i++)
                    {
                        if (i >= iRevYear)
                            dgInspectionFee.Rows.Add(i.ToString(), 0);
                    }

                }
                dgInspectionFee.Rows.Add((iCurYear + 1).ToString(), 0); //AFM 20210716 MAO-21-15404
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveHist() //AFM 20210721 MAO-21-15404
        {
            OracleResultSet res = new OracleResultSet();
            double dPrevAmt = 0;
            double dNewAmt = 0;
            string sTaxYear = string.Empty;
            int cnt = 0;
            string sDate = AppSettingsManager.GetSystemDate().ToString("dd-MMM-yy HH:mm:ss");
           

            for (int j = 0; j < dgInspectionFee.Rows.Count; j++)
            {
                sTaxYear = dgInspectionFee.Rows[j].Cells[0].Value.ToString();
                double.TryParse(dgInspectionFee.Rows[j].Cells[1].Value.ToString(), out dNewAmt);

                res.Query = "select count(*) from eps_assess_app where bin = '" + m_sBin + "' and tax_year = '" + sTaxYear + "'";
                int.TryParse(res.ExecuteScalar(), out cnt);
                if (cnt == 0)
                    dPrevAmt = 0;
                else
                {
                    res.Query = "select bin, tax_year, prev_fee, new_fee, sys_user, to_char(max(date_time),'dd-MM-yyyy hh24:mi:ss') as date_time from EPS_ASSESS_HIST where bin = '" + m_sBin + "' and tax_year = '" + sTaxYear + "' group by bin, tax_year, prev_fee, new_fee, sys_user";
                    if (res.Execute())
                        if (res.Read())
                        {
                            dPrevAmt = res.GetDouble("new_fee");
                            if (dPrevAmt == dNewAmt)
                                continue;
                        }
                        else
                            dPrevAmt = 0;
                    res.Close();
                }

                res.Query = string.Format("INSERT INTO EPS_ASSESS_HIST VALUES ('{0}','{1}','{2}','{3}','{4}',TO_DATE('{5}', 'dd/MM/yyyy hh24:mi:ss'))", m_sBin, dgInspectionFee.Rows[j].Cells[0].Value, dPrevAmt, dNewAmt, AppSettingsManager.g_objSystemUser.UserCode, sDate);
                if (res.ExecuteNonQuery() == 0)
                { }
            }

        }

        private void btnSetApproval_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();
            int iRow = 0;

            if (bin1.txtTaxYear.Text == "" && bin1.txtBINSeries.Text == "")
            {
                MessageBox.Show("Please search BIN", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int cnt = 0;
                for (int i = 0; i < dgvEps.Rows.Count; i++)
                {
                    if (dgvEps[0, i].Value != null && (bool)dgvEps[0, i].Value)
                    {
                        cnt++;
                        break;
                    }
                }

                if (cnt == 0)
                {
                    MessageBox.Show("Please check at least one requirement", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (rdoApprove.Checked == false && rdoPennding.Checked == false && rdoDisapprove.Checked == false)
                {
                    MessageBox.Show("Select assessment approval", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                double dAIF = 0;
                double.TryParse(txtTotalAnnualFee.Text, out dAIF);
                if (Convert.ToDouble(dAIF) <= 0)
                {
                    MessageBox.Show("Annual inspection fee amount is 0.00", " ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (MessageBox.Show("Are you sure you want to save?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //if (m_bRecordExist == true)
                    DeleteOldRecord();

                    SaveApproval();
                    SaveAssess();
                    SaveHist();

                    MessageBox.Show("Record saved", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string sTaxYear = "";
                    sTaxYear = ConfigurationAttributes.CurrentYear;

                    string strObj = "";
                    if (rdoApprove.Checked == true)
                    {
                        strObj = "Added Rqmt. BIN: " + bin1.GetBin() + " / Tax Year: " + sTaxYear; ;
                        m_sModuleCode = "AUEA-S-A";
                    }
                    if (rdoPennding.Checked == true)
                    {
                        strObj = "Added Rqmt. BIN: " + bin1.GetBin() + " / Tax Year: " + sTaxYear; ;
                        m_sModuleCode = "AUEA-S-P";
                    }
                    if (rdoDisapprove.Checked == true)
                    {
                        strObj = "Added Rqmt. BIN: " + bin1.GetBin() + " / Tax Year: " + sTaxYear; ;
                        m_sModuleCode = "AUEA-S-D";
                    }

                    if (AuditTrail.InsertTrail(m_sModuleCode, "eps_assess_app", StringUtilities.HandleApostrophe(strObj)) == 0)
                    {
                        result.Rollback();
                        result.Close();
                        MessageBox.Show(result.ErrorDescription, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    strObj = "";
                    strObj = "Added Rqmt. BIN: " + bin1.GetBin() + " / Tax Year: " + sTaxYear; ;
                    m_sModuleCode = "AUEA-A";

                    if (AuditTrail.InsertTrail(m_sModuleCode, "eps_assessment", StringUtilities.HandleApostrophe(strObj)) == 0)
                    {
                        result.Rollback();
                        result.Close();
                        MessageBox.Show(result.ErrorDescription, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    btnSearch.PerformClick();
                }
            }
        }

        private void ResetCheckBoxes()
        {
            for (int i = 0; i < dgvEps.RowCount; i++)
                dgvEps.Rows[i].Cells[0].Value = false;
        }

        private bool CheckifPaid()
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from pay_hist where bin = '" + bin1.GetBin().Trim() + "' and tax_year = '" + AppSettingsManager.GetCurrentDate().Year.ToString() + "' and (qtr_paid != 'A' or qtr_paid != 'P')";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    MessageBox.Show("BIN already paid!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    m_blnIsPaid = true;
                    return true;
                    pSet.Close();
                }
            }
            pSet.Close();

            return false;
        }

        private void SaveAssess()
        {
            OracleResultSet result = new OracleResultSet();
            string sBns = "", sReqCode = "";
            sBns = bin1.GetBin();

            int iFromTY = 0;
            int iToTY = 0;
            iFromTY = Convert.ToInt16(txtTaxYear.Text) + 1;
            iToTY = Convert.ToInt16(ConfigurationAttributes.CurrentYear);
            if (iFromTY > iToTY)
                iFromTY = iToTY;

            try
            {
                for (int l1 = 0; l1 < dgvEps.Rows.Count; l1++)
                {
                    bool chkRow = (bool)dgvEps[0, l1].Value;
                    if (chkRow)
                    {
                        sReqCode = dgvEps[1, l1].Value.ToString();
                        result.Query = string.Format("Insert Into eps_assessment Values('{0}','{1}','{2}','{3}','{4}')", sBns, iFromTY, iToTY, sReqCode, dgvEps[3, l1].Value.ToString());
                        result.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                return;
            }
        }

        private void SaveApproval()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet res1 = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();

            string sBns = "", sTaxYear = "", sBnStat = "", sReqCode = "", sApproval = "", sInspectFee = "", sRemarks = "";
            sBns = bin1.GetBin();
            sRemarks = txtRemarks.Text;

            //MCR 20170624 (s)
            double dInspectFee = 0;
            double.TryParse(sInspectFee, out dInspectFee);
            //MCR 20170624 (e)

            try
            {
                if (rdoApprove.Checked == true)
                    sApproval = "1";
                else if (rdoPennding.Checked == true)
                    sApproval = "2";
                else if (rdoDisapprove.Checked == true)
                    sApproval = "3";
                else
                    return;

                for (int j = 0; j < dgInspectionFee.Rows.Count; j++)
                {
                    result.Query = string.Format("Insert Into eps_assess_app Values('{0}','{1}','{2}','{3}','{4}')", sBns, dgInspectionFee.Rows[j].Cells[0].Value, sApproval, Convert.ToDouble(dgInspectionFee.Rows[j].Cells[1].Value), sRemarks);
                    result.ExecuteNonQuery();

                    result2.Query = string.Format("insert into eps_assess_app_remarks values('{0}','{1}','{2}')", sBns, dgInspectionFee.Rows[j].Cells[0].Value, sRemarks); //AFM 20200102
                    result2.ExecuteNonQuery();
                }
            }
            catch
            {
                return;
            }
        }

        private void DeleteOldRecord()
        {
            OracleResultSet pSet = new OracleResultSet();
            int iFromTY = 0;
            int iToTY = 0;

            //iFromTY = Convert.ToInt16(txtTaxYear.Text) + 1;
            iFromTY = Convert.ToInt16(txtTaxYear.Text);
            iToTY = Convert.ToInt16(ConfigurationAttributes.CurrentYear);

            iToTY += 1; //AFM 20210719 MAO-21-15404 include the following year

            //if (iFromTY > iToTY)
            //    iFromTY = iToTY;

            for (int i = iFromTY; i <= iToTY; i++)
            {
                pSet.Query = "delete from eps_assess_app where bin = '" + bin1.GetBin() + "' and tax_year = '" + i.ToString() + "'";
                pSet.ExecuteNonQuery();
                pSet.Query = "delete from EPS_ASSESS_APP_REMARKS where bin = '" + bin1.GetBin() + "' and tax_year = '" + i.ToString() + "'"; //AFM 20200103
                pSet.ExecuteNonQuery();
            }

            pSet.Query = "delete from eps_assessment where bin = '" + bin1.GetBin() + "' and from_tax_year = '" + iFromTY + "' and to_tax_year = '" + iToTY + "'";
            pSet.ExecuteNonQuery();


        }

        private void txtAnnulInsFee_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar))
                && (e.KeyChar != '.') && (e.KeyChar != '-'))
                e.Handled = true;

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;
        }

        private void dgInspectionFee_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //qty
            decimal varDecimal;
            if (dgInspectionFee.CurrentRow.Cells[1].Selected)
            {
                try
                {
                    if (dgInspectionFee.CurrentRow.Cells[1].Value.ToString() != "")
                    {
                        if (!decimal.TryParse(dgInspectionFee.CurrentRow.Cells[1].Value.ToString().Trim(), out varDecimal) || varDecimal < 0)
                        {
                            MessageBox.Show("Kindly insert numeric value", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            e.Cancel = true;
                        }
                    }
                }
                catch { }
            }
        }

        private void dgInspectionFee_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgInspectionFee.IsCurrentCellDirty)
                dgInspectionFee.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgInspectionFee_SelectionChanged(object sender, EventArgs e)
        {
            try { dgInspectionFee.BeginEdit(true); }
            catch { }
        }

        private void dgInspectionFee_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                double dTAF = 0;
                for (int i = 0; i < dgInspectionFee.Rows.Count; i++)
                    dTAF += Convert.ToDouble(dgInspectionFee.Rows[i].Cells[1].Value);

                txtTotalAnnualFee.Text = dTAF.ToString("#,##0.00");
            }
            catch { }
        }

        private void dgvEps_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bool blnIsChecked = false;
            String sValue = "";
            // get the current checkbox status
            blnIsChecked = (bool)dgvEps.Rows[e.RowIndex].Cells[0].Value;
            sValue = dgvEps.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (blnIsChecked)
            {
                if (sValue == "002") //Engineering Clearance
                    dgvEps.Rows[e.RowIndex].Cells[3].Value = AppSettingsManager.GetCurrentDate().ToString("MM/dd/yyyy");
                if (sValue == "001") //Annual Inspection //AFM 20200114
                    dgvEps.Rows[e.RowIndex].Cells[3].Value = AppSettingsManager.GetCurrentDate().ToString("MM/dd/yyyy");
            }
            else
            {
                if (sValue == "002") //Engineering Clearance
                    dgvEps.Rows[e.RowIndex].Cells[3].Value = "";
                if (sValue == "001") //Annual Inspection //AFM 20200114
                    dgvEps.Rows[e.RowIndex].Cells[3].Value = "";
            }
        }

        private void dgvEps_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvEps.IsCurrentCellDirty)
                dgvEps.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvEps_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvEps_CellContentClick(sender, e);
        }

        private void btnViewPayments_Click(object sender, EventArgs e)
        {
            using(frmViewParticulars frm = new frmViewParticulars())
            {
                frm.Bin = bin1.GetBin();
                frm.ShowDialog();
            }
        }

        private void btnPayHist_Click(object sender, EventArgs e)
        {
            //AFM 20191209 MAO-19-11502
            //if (AppSettingsManager.Granted("CCPH"))
            //AFM 20200123 MAO-20-11968
            if (AppSettingsManager.Granted("ARPH")) //AFM 20200109
            {
                frmPaymentHistory frmPayHist = new frmPaymentHistory();
                frmPayHist.ShowDialog();
            }
        }

        private void btnPrintTrail_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(m_sBin))
            {
                MessageBox.Show("No record selected!");
                return;
            }
            frmBussReport form = new frmBussReport();
            form.ReportSwitch = "EPSTrail";
            form.BIN = m_sBin;
            form.ShowDialog();
        }
    }
}