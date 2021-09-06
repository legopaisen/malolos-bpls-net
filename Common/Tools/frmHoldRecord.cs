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
using Amellar.Common.BIN;
using Amellar.Common.frmBns_Rec;

namespace Amellar.Common.Tools
{
    public partial class frmHoldRecord : Form
    {
        private string m_sTaxYear = string.Empty;
        private string m_sUserCode = string.Empty;
        private string m_sHoldRemarks = string.Empty;

        public frmHoldRecord()
        {
            InitializeComponent();
            bin1.txtLGUCode.Text = ConfigurationAttributes.LGUCode;
            bin1.txtDistCode.Text = ConfigurationAttributes.DistCode;
        }

        private void frmHoldRecord_Load(object sender, EventArgs e)
        {
            LoadList();
        }

        private void LoadList()
        {
            OracleResultSet pRec = new OracleResultSet();

            dgvList.Columns.Clear();

            dgvList.DefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Regular);
            dgvList.Columns.Add("BIN", "BIN");
            dgvList.Columns.Add("TYEAR", "Tax Year");
            dgvList.Columns.Add("BNSNM", "Business Name");
            dgvList.Columns.Add("BNSADD", "Business Address");
            dgvList.Columns.Add("OWNNM", "Owner's Name");
            dgvList.Columns.Add("OWNADD", "Owner's Address");
            dgvList.Columns.Add("STATUS", "Status");
            dgvList.Columns.Add("USER", "User");
            dgvList.Columns.Add("DATE", "Date");
            dgvList.Columns.Add("REMARKS", "Remarks");
            
            dgvList.RowHeadersVisible = false;
            dgvList.Columns[0].Width = 150;
            dgvList.Columns[2].Width = 200;
            dgvList.Columns[4].Width = 200;
            dgvList.Columns[6].Width = 80;
            dgvList.Columns[7].Width = 100;
            dgvList.Columns[8].Width = 100;
            dgvList.Columns[9].Width = 200;
            dgvList.Columns[1].Visible = false;
            dgvList.Columns[3].Visible = false;
            dgvList.Columns[5].Visible = false;
            
            dgvList.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvList.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvList.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvList.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvList.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvList.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvList.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            pRec.Query = "select * from hold_records where status = 'HOLD' order by bin";
            if(pRec.Execute())
            {
                int iRow = 0;
                while(pRec.Read())
                {
                    dgvList.Rows.Add("");
                    dgvList[0, iRow].Value = pRec.GetString("bin").Trim();
                    dgvList[1, iRow].Value = pRec.GetString("tax_year").Trim();
                    dgvList[2, iRow].Value = pRec.GetString("bns_nm").Trim();
                    dgvList[3, iRow].Value = pRec.GetString("bns_addr").Trim();
                    dgvList[4, iRow].Value = pRec.GetString("own_nm").Trim();
                    dgvList[5, iRow].Value = pRec.GetString("own_addr").Trim();
                    dgvList[6, iRow].Value = pRec.GetString("status").Trim();
                    dgvList[7, iRow].Value = pRec.GetString("user_code").Trim();
                    dgvList[8, iRow].Value = pRec.GetString("dt_save").Trim();
                    dgvList[9, iRow].Value = pRec.GetString("remarks").Trim();

                    if (iRow == 0)
                        CellClickList(iRow);
                    iRow++;
                }
            }
            pRec.Close();

            txtRemarks.ReadOnly = true;
        }

        private void CellClickList(int iRow)
        {
            string sBIN = "";   //192-00-2011-0006306
            try
            {
                sBIN = dgvList[0, iRow].Value.ToString();
                bin1.txtTaxYear.Text = sBIN.Substring(7, 4);
                bin1.txtBINSeries.Text = sBIN.Substring(12, 7);
                m_sTaxYear = dgvList[1, iRow].Value.ToString();
                txtBnsName.Text = dgvList[2, iRow].Value.ToString();
                txtBnsAdd.Text = dgvList[3, iRow].Value.ToString();
                txtOwnName.Text = dgvList[4, iRow].Value.ToString();
                txtOwnAdd.Text = dgvList[5, iRow].Value.ToString();
                txtStatus.Text = dgvList[6, iRow].Value.ToString();
                m_sUserCode = dgvList[7, iRow].Value.ToString();
                txtRemarks.Text = dgvList[9, iRow].Value.ToString();
                txtUntagRemarks.Text = "";

                btnSearch.Text = "Clear";
                btnTag.Text = "Untag";
                txtUntagRemarks.ReadOnly = false;
            }
            catch { }
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CellClickList(e.RowIndex);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text == "Clear")
            {
                if (TaskMan.IsObjectLock(bin1.GetBin(), "HOLD-RECORD", "DELETE", "ASS"))
                {
                }

                bin1.txtTaxYear.Text = "";
                bin1.txtBINSeries.Text = "";
                txtBnsName.Text = "";
                txtBnsAdd.Text = "";
                txtOwnName.Text = "";
                txtOwnAdd.Text = "";
                txtStatus.Text = "";
                txtRemarks.Text = "";
                m_sTaxYear = "";
                m_sUserCode = "";
                btnTag.Text = "Tag";
                txtRemarks.ReadOnly = true;
                txtUntagRemarks.ReadOnly = true;
                btnSearch.Text = "Search";

                
            }
            else
            {
                if (bin1.txtTaxYear.Text != "" || bin1.txtBINSeries.Text != "")
                {
                    if (!TaskMan.IsObjectLock(bin1.GetBin(), "HOLD-RECORD", "ADD", "ASS"))
                    {
                        LoadValues(bin1.GetBin());
                    }
                }
                else
                {
                    frmSearchBusiness frmSearchBns = new frmSearchBusiness();
                    frmSearchBns.ModuleCode = "";

                    frmSearchBns.ShowDialog();
                    if (frmSearchBns.sBIN.Length > 1)
                    {
                        bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                        bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
                        if (!TaskMan.IsObjectLock(bin1.GetBin(), "HOLD-RECORD", "ADD", "ASS"))
                        {
                            LoadValues(bin1.GetBin());
                        }
                    }
                }
            }
        }

        private void LoadValues(string sBin)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sOwnCode = "";

            pRec.Query = "select * from businesses where bin = '" + sBin + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    txtBnsName.Text = pRec.GetString("bns_nm");
                    txtBnsAdd.Text = AppSettingsManager.GetBnsAddress(sBin);
                    sOwnCode = pRec.GetString("own_code");
                    txtOwnName.Text = AppSettingsManager.GetBnsOwner(sOwnCode);
                    txtOwnAdd.Text = AppSettingsManager.GetBnsOwnAdd(sOwnCode);
                    m_sTaxYear = pRec.GetString("tax_year");
                    
                    txtRemarks.ReadOnly = false;
                    txtUntagRemarks.ReadOnly = true;
                    btnTag.Text = "Tag";
                    btnSearch.Text = "Clear";
                    SearchBinFromEntries();
                    pRec.Close();
                }
                else
                {
                    pRec.Close();

                    pRec.Query = "select * from business_que where bin = '" + sBin + "'";
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            txtBnsName.Text = pRec.GetString("bns_nm");
                            txtBnsAdd.Text = AppSettingsManager.GetBnsAddress(sBin);
                            sOwnCode = pRec.GetString("own_code");
                            txtOwnName.Text = AppSettingsManager.GetBnsOwner(sOwnCode);
                            txtOwnAdd.Text = AppSettingsManager.GetBnsOwnAdd(sOwnCode);
                            m_sTaxYear = pRec.GetString("tax_year"); //AFM 20200130

                            txtRemarks.ReadOnly = false;
                            txtUntagRemarks.ReadOnly = true;
                            btnTag.Text = "Tag";
                            btnSearch.Text = "Clear";
                            SearchBinFromEntries();
                        }
                        else
                        {
                            MessageBox.Show("BIN not found", "Tools: Hold Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                    pRec.Close();
                }
            }

        }

        private void SearchBinFromEntries()
        {
            int iIndex = -1;
            for (int iRow = 0; iRow < dgvList.Rows.Count; iRow++)
            {
                if (dgvList[0, iRow].Value.ToString().Trim() == bin1.GetBin())
                {
                    iIndex = iRow;
                }
            }

            if (iIndex != -1)
            {
                dgvList.CurrentCell = dgvList[0, iIndex];

                txtRemarks.ReadOnly = true;
                txtUntagRemarks.ReadOnly = false;
                btnTag.Text = "Untag";
            }
        }

        private void btnTag_Click(object sender, EventArgs e)
        {
            OracleResultSet pCmd = new OracleResultSet();

            if (btnTag.Text == "Tag")
            {
                if (txtBnsName.Text.ToString().Trim() == "")
                {
                    MessageBox.Show("No record to hold", "Tools: Hold Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (txtRemarks.Text.ToString().Trim() == "")
                {
                    MessageBox.Show("Enter value for remarks", "Tools: Hold Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (MessageBox.Show("Are you sure you want to hold this record?", "Tools: Hold Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sDate = "";
                    sDate = Convert.ToString(AppSettingsManager.GetCurrentDate());
                    
                    pCmd.Query = "insert into hold_records values (";
                    pCmd.Query += "'" + bin1.GetBin() + "',";
                    pCmd.Query += "'" + m_sTaxYear + "',";
                    pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(txtBnsName.Text.ToString()) + "',";
                    pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(txtBnsAdd.Text.ToString()) + "',";
                    pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(txtOwnName.Text.ToString()) + "',";
                    pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(txtOwnAdd.Text.ToString()) + "',";
                    pCmd.Query += "'HOLD',";
                    pCmd.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', ";
                    pCmd.Query += " to_date('" + sDate + "', 'MM/dd/yyyy hh:mi:ss am'), ";
                    pCmd.Query += "'" + StringUtilities.StringUtilities.HandleApostrophe(txtRemarks.Text.ToString()) + "')";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    MessageBox.Show("Record saved.","Tools: Hold Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadList();
                }
            }
            else
            {
                if (txtUntagRemarks.Text.Trim() == "")
                {
                    MessageBox.Show("Enter reason for untagging", "Tools: Hold Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (MessageBox.Show("Are you sure you want to approve this record?", "Tools: Hold Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    
                    string sRemarks = "";
                    sRemarks = txtRemarks.Text.ToString();
                    sRemarks += ". UNTAG REMARKS: " + txtUntagRemarks.Text.ToString().Trim();

                    pCmd.Query = "update hold_records set status = 'APPROVED', remarks = '" + sRemarks + "'";
                    pCmd.Query += " where bin = '" + bin1.GetBin() + "' and status = 'HOLD'";
                    //pCmd.Query += " and user_code = '" + m_sUserCode + "'"; //AFM 20200130 unnecessary
                    pCmd.Query += " and tax_year = '" + m_sTaxYear + "'";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    MessageBox.Show("Record updated.","Tools: Hold Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadList();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            frmReport ReportFrm = new frmReport();
            ReportFrm.ReportName = "LIST OF RECORDS ON HOLD";
            ReportFrm.ShowDialog();
        }
    }
}