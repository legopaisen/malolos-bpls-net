using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.BadChecks
{
    public partial class frmSearchChkNo : Form
    {
        private string m_sBankCode = string.Empty;
                
        public string CheckNo
        {
            get { return txtChkNo.Text; }
            set { txtChkNo.Text = value; }
        }

        public frmSearchChkNo()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            OracleResultSet set = new OracleResultSet();
            bool bRecordFound = false;
            string sChkNo = string.Empty;
            string sBankCode = string.Empty;
            string sBankNm = string.Empty;
            string sAcctNo = string.Empty;
            string sChkAmt = string.Empty;
            string sLastNm = string.Empty;
            string sFirstNm = string.Empty;
            string sMI = string.Empty;
            DateTime dtpChkDt;

            if (btnSearch.Text == "Search")
            {
                ClearControls();

                if (txtORNo.Text.Trim() == "")
                {
                    MessageBox.Show("Enter O.R. to search", "Search", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                dgvList.Columns.Clear();
                dgvList.Columns.Add("BankCode", "Bank Code");
                dgvList.Columns.Add("BankName", "Bank Name");
                dgvList.Columns.Add("CheckNo", "Check No.");
                dgvList.Columns.Add("CheckAmt", "Check Amt.");
                dgvList.Columns.Add("CheckDt", "Check Date");
                dgvList.Columns.Add("AcctNo", "Acct. No.");
                dgvList.Columns.Add("OwnLn", "Last Name");
                dgvList.Columns.Add("OwnFn", "First Name");
                dgvList.Columns.Add("OwnMI", "M.I.");
                dgvList.Columns[0].Visible = false;
                dgvList.Columns[4].Visible = false;

                string sQuery = string.Empty;

                set.Query = "select * from chk_tbl where or_no = '" + StringUtilities.HandleApostrophe(txtORNo.Text.Trim()) + "'";
                if (set.Execute())
                {
                    if (set.Read())
                    {
                        bRecordFound = true;
                        sQuery = set.Query.ToString();
                    }
                    else
                    {
                        set.Close();

                        set.Query = "select * from cancelled_chk where or_no = '" + StringUtilities.HandleApostrophe(txtORNo.Text.Trim()) + "'";
                        if (set.Execute())
                        {
                            if (set.Read())
                            {
                                bRecordFound = true;
                                sQuery = set.Query.ToString();
                            }
                        }
                        else
                        {
                            MessageBox.Show("No record found.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            bRecordFound = false;
                            set.Close();
                            return;
                        }
                    }
                }

                if (bRecordFound)
                {
                    set.Query = sQuery;
                    if (set.Execute())
                    {
                        while (set.Read())
                        {
                            sBankCode = set.GetString("bank_code");
                            sBankNm = AppSettingsManager.GetBankBranch(sBankCode);
                            sChkNo = set.GetString("chk_no");
                            sAcctNo = set.GetString("acct_no");
                            sChkAmt = string.Format("{0:#,###.00}", set.GetDouble("chk_amt"));
                            sLastNm = set.GetString("acct_ln");
                            sFirstNm = set.GetString("acct_fn");
                            sMI = set.GetString("acct_mi");
                            dtpChkDt = set.GetDateTime("chk_date");
                            string sChkDt = string.Format("{0:MM/dd/yyyy}", dtpChkDt);

                            dgvList.Rows.Add(sBankCode, sBankNm, sChkNo, sChkAmt, sChkDt, sAcctNo, sLastNm, sFirstNm, sMI);
                        }
                        btnSearch.Text = "Clear";
                    }
                    set.Close();
                }

            }
            else
            {
                ClearControls();
                txtORNo.Text = "";
                btnSearch.Text = "Search";

            }
        }

        private void ClearControls()
        {
            m_sBankCode = "";
            txtBankName.Text = "";
            txtChkNo.Text = "";
            txtChkAmt.Text = "";
            dtpChkDt.Value = AppSettingsManager.GetCurrentDate();
            txtAcctNo.Text = "";
            txtLastName.Text = "";
            txtFirstName.Text = "";
            txtMI.Text = "";
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (txtChkNo.Text.Trim() == "")
            {
                if (MessageBox.Show("No Check details selected. Continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    this.Dispose();
                else
                    return;
            }

            this.Close();
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DateTime dtChkDt;
                DateTime.TryParse(dgvList[4, e.RowIndex].Value.ToString(), out dtChkDt);
                m_sBankCode = dgvList[0, e.RowIndex].Value.ToString();
                txtBankName.Text = dgvList[1, e.RowIndex].Value.ToString();
                txtChkNo.Text = dgvList[2, e.RowIndex].Value.ToString();
                txtChkAmt.Text = dgvList[3, e.RowIndex].Value.ToString();
                dtpChkDt.Value = dtChkDt;
                txtAcctNo.Text = dgvList[5, e.RowIndex].Value.ToString();
                txtLastName.Text = dgvList[6, e.RowIndex].Value.ToString();
                txtFirstName.Text = dgvList[7, e.RowIndex].Value.ToString();
                txtMI.Text = dgvList[8, e.RowIndex].Value.ToString();
            }
            catch
            {
                m_sBankCode = "";
                txtBankName.Text = "";
                txtChkNo.Text = "";
                txtChkAmt.Text = "";
                dtpChkDt.Value = AppSettingsManager.GetCurrentDate();
                txtAcctNo.Text = "";
                txtLastName.Text = "";
                txtFirstName.Text = "";
                txtMI.Text = "";
            }
        }
    }
}