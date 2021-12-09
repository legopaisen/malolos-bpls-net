using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.SearchBusiness;

namespace Amellar.Modules.HealthPermit
{
    public partial class frmWorking : Form
    {
        public frmWorking()
        {
            InitializeComponent();
        }

        string m_sBIN = string.Empty;
        //MCR 20150325 (s)
        private string m_sPermitType = string.Empty;
        public string PermitType
        {
            set { m_sPermitType = value; }
        }
        public string m_sOrNo = string.Empty;
        public string m_sOrDate = string.Empty;
        //MCR 20150325 (e)

        private void frmWorkingcs_Load(object sender, EventArgs e)
        {
            if (m_sPermitType == "ARCS")
            {
                lblBIN.Text = "O.R. #";
                lblORNO.Text = m_sOrNo;
                m_sBIN = m_sOrNo;
                ViewRecords();
                pnlWork.Visible = false;
            }
            else
            {
                m_sPermitType = "BPLS";// JHB 20200702 to identify permit type
                pnlWork.Visible = true;
                bin2.GetLGUCode = ConfigurationAttributes.LGUCode;
                bin2.GetDistCode = ConfigurationAttributes.DistCode;
                bin2.txtTaxYear.Focus();
                this.ActiveControl = bin2.txtTaxYear;
            }
        }

        private void PermitRecord()
        {
            String m_sPermitNumber = "";
            String m_sIssuedDate = "";
            #region GetNextSeries
            OracleResultSet result = new OracleResultSet();
            String sQuery = String.Empty;
            result.Query = "select coalesce(max(to_number(permit_number)),0)+1 as permit_number from permit_working_number where current_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
            if (result.Execute())
            {
                if (result.Read())
                    m_sPermitNumber = AppSettingsManager.GetConfigValue("12") + "-" + result.GetInt(0).ToString("0000");
            }
            result.Close();
            #endregion

            #region CheckExist
            result.Query = "select * from permit_working_number where bin = '" + m_sBIN + "' and current_year = '" + AppSettingsManager.GetConfigValue("12") + "' and emp_id = '" + dtgRecord.CurrentRow.Cells[0].Value.ToString() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    m_sPermitNumber = result.GetString(0) + "-" + result.GetString(1);
                    m_sIssuedDate = result.GetString(2);
                }
            }
            result.Close();
            #endregion

            #region Saving

            if (m_sIssuedDate == "")
            {
                string sCurrentYear = AppSettingsManager.GetConfigValue("12");
                string sPermType = "WORKING";
                string sPermitNumber = m_sPermitNumber.Substring(5);
                string sIssuedDate = AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");
                string sUserCode = AppSettingsManager.SystemUser.UserCode;
                string sBin = m_sBIN;
                string sEmpID = dtgRecord.CurrentRow.Cells[0].Value.ToString().Trim();

                result.Query = "insert into permit_working_number (current_year,permit_number,issued_date,user_code,bin,emp_id) values('" + sCurrentYear + "', '" + sPermitNumber + "', '" + sIssuedDate + "', '" + sUserCode + "', '" + sBin + "','" + sEmpID + "')";
                result.ExecuteNonQuery();
            }
            #endregion
        }

        private void GetPermitNo(out string sPermitNO, out string sPermitDate, string sBIN, string sEMPID)
        {
            sPermitNO = "";
            sPermitDate = "";
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from permit_working_number where bin = '" + sBIN + "' and current_year = '" + AppSettingsManager.GetConfigValue("12") + "' and emp_id = '" + sEMPID + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sPermitNO = result.GetString(0) + "-" + result.GetString(1);
                    sPermitDate = result.GetString(2);
                }
            }
            result.Close();
        }

        private void GetORNo(out string sORNO, out string sORDate, string sBIN, string sTaxYear)
        {
            sORDate = "";
            sORNO = "";
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = @"select distinct PH.or_no,PH.or_date from pay_hist PH
            inner join or_table OT on OT.or_no = PH.or_no
            inner join bns_table BT on OT.fees_code = BT.fees_code
            where PH.bin = '" + sBIN + "' and PH.tax_year = '" + sTaxYear + @"'
            and BT.bns_desc like '%WORKING%'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sORDate = pSet.GetDateTime(1).ToString("MM/dd/yyyy");
                    sORNO = pSet.GetString(0);
                }
            }
            pSet.Close();
        }

        private void GetARCSPermitBnsOwnNameAdd(out string sBnsOwnName, out string sBnsOwnAdd, string sORNo)
        {
            OracleResultSet pSet = new OracleResultSet();
            sBnsOwnName = "";
            sBnsOwnAdd = "";
            pSet.Query = "select EMP_LN||', '||EMP_FN||' '||EMP_MI as EMP_NAME,EMP_ADDRESS from emp_names where bin = '" + sORNo + "' and Emp_Occupation = 'OWNER'";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    sBnsOwnName = pSet.GetString(0).Trim();
                    sBnsOwnAdd = pSet.GetString(1).Trim();
                }
            pSet.Close();
        }

        private void ViewRecords()
        {
            String EmployerNM = "";
            String EmployerADD = "";
            String sPermitNO = "";
            String sPermitDate = "";
            String sORNO = "";
            String sORDate = "";
            dtgRecord.Rows.Clear();
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = @"select EMP_ID,EMP_TIN,EMP_CTC_NUMBER,EMP_CTC_ISSUED_ON,EMP_CTC_ISSUED_AT,
            EMP_LN||', '||EMP_FN||' '||EMP_MI as EMP_NAME,EMP_ADDRESS,EMP_GENDER,EMP_DATE_OF_BIRTH,EMP_OCCUPATION,BIN,TAX_YEAR
            from emp_names where emp_occupation <> 'OWNER'";


            if (m_sPermitType == "ARCS")
                pSet.Query += " and (bin = '" + m_sBIN + "' or temp_bin = '" + m_sBIN + "')";
            else
                pSet.Query += " and (bin = '" + m_sBIN + "' or temp_bin = '" + m_sBIN + "') and tax_year = '" + AppSettingsManager.GetBnsTaxYear(m_sBIN) + "'";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    GetPermitNo(out sPermitNO, out sPermitDate, m_sBIN, pSet.GetString(0));
                    if (m_sPermitType == "ARCS")
                    {
                        sORNO = m_sOrNo;
                        sORDate = m_sOrDate;
                        GetARCSPermitBnsOwnNameAdd(out EmployerNM, out EmployerADD, sORNO);
                    }
                    else
                    {
                        GetORNo(out sORNO, out sORDate, m_sBIN, pSet.GetString(11));
                        EmployerADD = AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetBnsOwnCode(m_sBIN));
                        EmployerNM = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(m_sBIN));
                    }
                    dtgRecord.Rows.Add(pSet.GetString(0), sORNO, sORDate, sPermitNO, sPermitDate, pSet.GetString(1), pSet.GetString(2), pSet.GetString(3), pSet.GetString(4), pSet.GetString(5), pSet.GetString(6), pSet.GetString(7), pSet.GetString(8), pSet.GetString(9), EmployerNM, EmployerADD);
                }
            }
            pSet.Close();

            if (dtgRecord.Rows.Count == 0)
            {
                pSet.Query = @"select EMP_ID,EMP_TIN,EMP_CTC_NUMBER,EMP_CTC_ISSUED_ON,EMP_CTC_ISSUED_AT,
                EMP_LN||', '||EMP_FN||' '||EMP_MI as EMP_NAME,EMP_ADDRESS,EMP_GENDER,EMP_DATE_OF_BIRTH,EMP_OCCUPATION,BIN,TAX_YEAR
                from emp_names where (bin = '" + m_sBIN + "' or temp_bin = '" + m_sBIN + "')";

                if (m_sPermitType != "ARCS")
                    pSet.Query += " and tax_year = '" + AppSettingsManager.GetBnsTaxYear(m_sBIN) + "'";

                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        GetPermitNo(out sPermitNO, out sPermitDate, m_sBIN, pSet.GetString(0));
                        if (m_sPermitType == "ARCS")
                        {
                            sORNO = m_sOrNo;
                            sORDate = m_sOrDate;
                            GetARCSPermitBnsOwnNameAdd(out EmployerNM, out EmployerADD, sORNO);
                        }
                        else
                        {
                            GetORNo(out sORNO, out sORDate, m_sBIN, pSet.GetString(11));
                            EmployerADD = AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetBnsOwnCode(m_sBIN));
                            EmployerNM = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(m_sBIN));
                        }
                        dtgRecord.Rows.Add(pSet.GetString(0), sORNO, sORDate, sPermitNO, sPermitDate, pSet.GetString(1), pSet.GetString(2), pSet.GetString(3), pSet.GetString(4), pSet.GetString(5), pSet.GetString(6), pSet.GetString(7), pSet.GetString(8), pSet.GetString(9), EmployerNM, EmployerADD);
                    }
                }
                pSet.Close();

                if (dtgRecord.Rows.Count == 0)
                {
                    MessageBox.Show("No Records Found", "Working Permit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ClearControls();
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string sORNO = "";
            string sORDate = "";

            sORNO = m_sOrNo;
            sORDate = m_sOrDate; 

            string sTaxYear = AppSettingsManager.GetBnsTaxYear(m_sBIN);

            if (m_sPermitType != "ARCS")
            {
                GetORNo(out sORNO, out sORDate, m_sBIN, sTaxYear);
                if (sORNO == "" || sORDate == "")
                {
                    MessageBox.Show("Working Permit is not yet paid", "Working Permit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            PermitRecord();

            frmPrinting frmPrinting = new frmPrinting();
            frmPrinting.ReportType = "Working Permit";
           // frmPrinting.PermitType = "ARCS";
            frmPrinting.PermitType = m_sPermitType; // JHB 20200702 to identify permit type
            frmPrinting.ORNo = sORNO;
            frmPrinting.ORDate = sORDate;
            frmPrinting.BIN = m_sBIN;
            frmPrinting.EmpID = dtgRecord.CurrentRow.Cells[0].Value.ToString();
            frmPrinting.TaxYear = sTaxYear;
            frmPrinting.ShowDialog();
            ViewRecords();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (TaskMan.IsObjectLock(m_sBIN, "WORKING PERMIT", "DELETE", "ASS"))
            {
            }

            if (btnSearch.Text == "Search")
            {
                btnEdit.Enabled = true;
                btnSearch.Text = "Clear";
                if (bin2.txtTaxYear.Text != "" || bin2.txtBINSeries.Text != "")
                {
                    if (!TaskMan.IsObjectLock(bin2.GetBin(), "WORKING PERMIT", "ADD", "ASS"))
                    {
                        m_sBIN = bin2.GetBin();
                        ViewRecords();
                    }
                    else
                    {
                        bin2.txtTaxYear.Text = "";
                        bin2.txtBINSeries.Text = "";
                        m_sBIN = "";
                        btnEdit.Enabled = false;
                        btnSearch.Text = "Search";
                    }
                }
                else
                {
                    frmSearchBusiness frmSearchBns = new frmSearchBusiness();
                    frmSearchBns.ShowDialog();
                    if (frmSearchBns.sBIN.Length > 1)
                    {
                        bin2.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                        bin2.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
                        if (!TaskMan.IsObjectLock(bin2.GetBin(), "HEALTH PERMIT", "ADD", "ASS"))
                        {
                            m_sBIN = bin2.GetBin();
                            ViewRecords();
                        }
                        else
                        {
                            bin2.txtTaxYear.Text = "";
                            bin2.txtBINSeries.Text = "";
                            m_sBIN = "";
                            btnEdit.Enabled = false;
                            btnSearch.Text = "Search";
                        }
                    }
                }
            }
            else
                ClearControls();
        }

        private void ClearControls()
        {
            if (TaskMan.IsObjectLock(bin2.GetBin(), "WORKING PERMIT", "DELETE", "ASS"))
            {
            }

            btnEdit.Enabled = false;
            btnSearch.Text = "Search";
            bin2.txtTaxYear.Text = string.Empty;
            bin2.txtBINSeries.Text = string.Empty;
            txtCTCNo.Text = string.Empty;
            dtpIssuedOn.Text = string.Empty;
            txtIssuedAt.Text = string.Empty;
            dtgRecord.Rows.Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "Close")
            {
                ClearControls();
                this.Close();
            }
            else
            {
                if (m_sPermitType == "ARCS") //MCR 20150325
                    ViewRecords();

                btnEdit.Text = "Edit";
                btnClose.Text = "Close";
                ButtonControls(false);
                btnPrint.Enabled = true;
            }
        }

        private void ButtonControls(bool blnEnable)
        {
            btnPrint.Enabled = blnEnable;
            txtCTCNo.Enabled = blnEnable;
            txtIssuedAt.Enabled = blnEnable;
            dtpIssuedOn.Enabled = blnEnable;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnClose.Text = "Cancel";

            if (btnEdit.Text == "Edit")
            {
                btnEdit.Text = "Update";
                ButtonControls(true);
                btnPrint.Enabled = false;
            }
            else
            {
                if (!CheckingofCTCNo())
                    return;

                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = "update emp_names set EMP_CTC_NUMBER = '" + txtCTCNo.Text + "', EMP_CTC_ISSUED_ON = '" + dtpIssuedOn.Value.ToString("MM/dd/yyyy") + "', EMP_CTC_ISSUED_AT = '" + txtIssuedAt.Text + "' where EMP_ID = '" + dtgRecord.CurrentRow.Cells[0].Value.ToString() + "'";
                pSet.ExecuteNonQuery();
                MessageBox.Show("Successfully Updated", "Working Permit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnEdit.Text = "Edit";
                btnClose.Text = "Close";
                ButtonControls(false);
                btnPrint.Enabled = true;
                ViewRecords();
            }
        }

        private void dtgRecord_SelectionChanged(object sender, EventArgs e)
        {
            if (dtgRecord.RowCount > 0)
            {
                txtCTCNo.Text = dtgRecord.CurrentRow.Cells[6].Value.ToString();
                dtpIssuedOn.Text = dtgRecord.CurrentRow.Cells[7].Value.ToString();
                txtIssuedAt.Text = dtgRecord.CurrentRow.Cells[8].Value.ToString();
                btnEdit.Enabled = true;
            }
            else
            {
                btnEdit.Enabled = false;
                txtCTCNo.Text = "";
                txtIssuedAt.Text = "";
                dtpIssuedOn.Text = "";
            }
        }

        private void frmWorking_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClearControls();
        }

        private bool CheckingofCTCNo()
        {
            /*if (txtCTCNo.Text.Trim() == "")
            {
                MessageBox.Show("CTC No. is required!", "Working Permit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }*/

            // RMC 20150114 disable validation of ctc as per treas (s)
            if (txtCTCNo.Text.Trim() == "")
            {
                if (MessageBox.Show("No CTC No. Continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return false;
            }

            if (txtCTCNo.Text.Trim() != "")
            // RMC 20150114 disable validation of ctc as per treas (e)
            {
                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = @"select distinct emp_ctc_number from emp_names where emp_ctc_number = '" + txtCTCNo.Text.Trim() + "'";
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        MessageBox.Show("CTC No. already exist!", "Working Permit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                pSet.Close();
            }
            return true;
        }
    }
}