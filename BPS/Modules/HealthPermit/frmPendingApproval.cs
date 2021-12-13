using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.HealthPermit
{
    public partial class frmPendingApproval : Form
    {
        private string m_sOffice = string.Empty;

        public frmPendingApproval()
        {
            InitializeComponent();
            bin2.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin2.GetDistCode = ConfigurationAttributes.DistCode;
            bin2.txtTaxYear.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if ((bin2.txtTaxYear.Text == "" || bin2.txtBINSeries.Text == "") && btnSearch.Text == "Search")
            {
                MessageBox.Show("Select BIN to search", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (btnSearch.Text == "Search")
            {
                btnSearch.Text = "Clear";
                
                UpdateList(bin2.GetBin());
            }
            else
            {
                btnSearch.Text = "Search";
                bin2.GetTaxYear = "";
                bin2.GetBINSeries = "";
                UpdateList("");
            }
        }

        private void frmPendingApproval_Load(object sender, EventArgs e)
        {
            UpdateList("");
        }

        private void UpdateList(string p_sBIN)
        {
            dgvList.Columns.Clear();

            dgvList.Columns.Add("1", "Seq.");
            dgvList.Columns.Add("2", "Pending Office");
            dgvList.Columns.Add("3", "Violation");
            dgvList.Columns.Add("4", "BIN");
            dgvList.Columns.Add("5", "Business Name");
            dgvList.Columns.Add("6", "Tax Year");
            dgvList.Columns.Add("7", "Last Name");
            dgvList.Columns.Add("8", "First Name");
            dgvList.Columns.Add("9", "M.I.");
            dgvList.Columns.Add("10", "Business Address");
            dgvList.Columns.Add("11", "Bns Stat");
            dgvList.Columns.Add("12", "Area");
            dgvList.Columns.Add("13", "Lessor");

            dgvList.Columns[0].Width = 0;
            dgvList.Columns[1].Width = 100;
            dgvList.Columns[2].Width = 100;
            dgvList.Columns[3].Width = 180;
            dgvList.Columns[4].Width = 200;
            dgvList.Columns[5].Width = 80;
            dgvList.Columns[6].Width = 100;
            dgvList.Columns[7].Width = 100;
            dgvList.Columns[8].Width = 20;
            dgvList.Columns[9].Width = 180;
            dgvList.Columns[10].Width = 80;
            dgvList.Columns[11].Width = 80;
            dgvList.Columns[12].Width = 80;

            dgvList.Columns[0].Visible = false;

            OracleResultSet pSet = new OracleResultSet();

            string sLN = ""; string sFN = ""; string sMI = ""; string sBIN = "";
            string sBnsHouseNo = ""; string sBnsStreet = ""; string sBnsBrgy = ""; string sAddress = ""; string sBnsMun = "";
            string sBnsStat = ""; string sArea = "";
            string sAppID = "";

            OracleResultSet pRec = new OracleResultSet();
            int iCnt = 0;

            pRec.Query = "select bin, app_id from app_permit_tmp ";
            pRec.Query += " where tax_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
            pRec.Query += " and (bin not in (select bin from trans_approve where tax_year = '" + AppSettingsManager.GetConfigValue("12") + "' having count(*) >= 4 group by bin) ";
            pRec.Query += " or bin in (select bin from eps_assess_app where tax_year = '" + AppSettingsManager.GetConfigValue("12") + "' and app_stat = '3') ";
            pRec.Query += " or bin in (select bin from TRANS_APPROVE_HIST where tax_year = '" + AppSettingsManager.GetConfigValue("12") + "' and remarks = 'DISAPPROVED'))";
            if (!string.IsNullOrEmpty(p_sBIN))
                pRec.Query += " and bin = '" + p_sBIN + "'";
            pRec.Query += " order by app_id";
            bool bDisplay = true;
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    iCnt++;
                    sBIN = pRec.GetString(0);
                    sAppID = string.Format("{0:###}", pRec.GetInt(1));
                    bDisplay = true;

                    string sPlace = string.Empty;
                    string sLessor = string.Empty;

                    pSet.Query = "select bin,bns_nm,tax_year, bns_house_no,bns_street,bns_brgy, bns_mun, own_code,bns_stat,flr_area, place_occupancy, busn_own from business_que ";
                    pSet.Query += " where tax_year <= '" + AppSettingsManager.GetConfigValue("12") + "' and bin = '" + sBIN + "'";  // to include discovery delinq
                    if (m_sOffice == "MARKET")
                        pSet.Query += " and bns_street like '%MARKET%' ";

                    if (pSet.Execute())
                    {
                        while (pSet.Read())
                        {
                            GetOwnName(sBIN, pSet.GetString("own_code").Trim(), out sLN, out sFN, out sMI);

                            sBnsHouseNo = pSet.GetString("bns_house_no").Trim();
                            sBnsStreet = pSet.GetString("bns_street").Trim();

                            sBnsBrgy = pSet.GetString("bns_brgy").Trim();
                            sBnsMun = pSet.GetString("bns_mun").Trim();
                            sBnsStat = pSet.GetString("bns_stat").Trim();
                            sArea = string.Format("{0:####}", pSet.GetDouble("flr_area"));
                            sPlace = pSet.GetString("place_occupancy");
                            if (sPlace.Contains("RENTED"))
                            {
                                sLessor = pSet.GetString("busn_own");
                                sLessor = AppSettingsManager.GetBnsOwner(sLessor);
                            }
                            else
                            {
                                sLessor = "OWNED";
                            }

                            if (sBnsHouseNo == "." || sBnsHouseNo == "")
                                sBnsHouseNo = "";
                            else
                                sBnsHouseNo = sBnsHouseNo + " ";
                            if (sBnsStreet == "." || sBnsStreet == "")
                                sBnsStreet = "";
                            else
                                sBnsStreet = sBnsStreet + ", ";
                            if (sBnsBrgy == "." || sBnsBrgy == "")
                                sBnsBrgy = "";
                            else

                                sBnsBrgy = "BARANGAY " + sBnsBrgy + ", ";
                            if (sBnsMun == "." || sBnsMun == "")
                                sBnsMun = "";

                            sAddress = sBnsHouseNo + sBnsStreet + sBnsBrgy + sBnsMun;

                            bool bMarket = false;
                            if (sBnsStreet.Contains("MARKET"))
                                bMarket = true;

                            string sAppBy = string.Empty;
                            string sViolation = string.Empty;

                            if (CheckIfDisapproved(sBIN))
                            {
                                sAppBy = m_sOffice;
                            }

                            if (CheckIfWithViolation(sBIN))
                                sViolation = m_sOffice + " - WITH VIOLATION";

                            dgvList.Rows.Add(sAppID, sAppBy, sViolation, sBIN, pSet.GetString(1), pSet.GetString(2),
                                sLN, sFN, sMI, sAddress, sBnsStat, sArea, sLessor);
                        }

                    }
                    pSet.Close();

                }
            }
            pRec.Close();

            if (iCnt == 0)
            {
                MessageBox.Show("No record found", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
        }

        private bool CheckIfDisapproved(string sBIN)
        {
            OracleResultSet pSet = new OracleResultSet();
            int iCnt = 0;
            m_sOffice = "";

            pSet.Query = "select * from eps_assess_app where bin = '" + sBIN + "' and tax_year = '" + AppSettingsManager.GetConfigValue("12")+ "' and app_stat = '3'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    m_sOffice = "ENGINEERING: DISAPPROVED - " + pSet.GetString("remarks");
                    return true;
                }
            }
            pSet.Close();


            pSet.Query = "select * from TRANS_APPROVE_HIST where bin = '" + sBIN + "' and tax_year = '" + AppSettingsManager.GetConfigValue("12") + "' and remarks = 'DISAPPROVED' order by app_dt desc";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    m_sOffice = pSet.GetString("office_nm") + ": DISAPPROVED";
                    return true;
                }
            }
            pSet.Close();
            

            return false;

        }


        private bool CheckIfWithViolation(string sBIN)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sViolation = string.Empty;
            string sViolationCode = string.Empty;
            m_sOffice = "";

            pSet.Query = "select * from nigvio_list where bin = '" + sBIN + "' order by violation_code";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    if (!string.IsNullOrEmpty(m_sOffice))
                        m_sOffice += "/ ";
                    m_sOffice += pSet.GetString("division_code");
                    
                    pSet.Close();
                    return true;
                }
            }
            pSet.Close();

            return false;
        }

        private void GetOwnName(string p_sBIN, string p_sOwnCode, out string o_sLN, out string o_sFN, out string o_sMI)
        {
            // RMC 20150117
            OracleResultSet pName = new OracleResultSet();
            o_sLN = "";
            o_sFN = "";
            o_sMI = "";

            pName.Query = "select * from emp_names where (bin = '" + p_sBIN + "' or temp_bin = '" + p_sBIN + "') and emp_occupation = 'OWNER'";
            if (pName.Execute())
            {
                if (pName.Read())
                {
                    o_sLN = pName.GetString("emp_ln");
                    o_sFN = pName.GetString("emp_fn");
                    o_sMI = pName.GetString("emp_mi");
                }
            }
            pName.Close();

            if (string.IsNullOrEmpty(o_sLN))
            {
                o_sLN = AppSettingsManager.GetBnsOwnerLastName(p_sOwnCode);
                o_sFN = AppSettingsManager.GetBnsOwnerFirstName(p_sOwnCode);
                o_sMI = AppSettingsManager.GetBnsOwnerMiName(p_sOwnCode);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTrail_Click(object sender, EventArgs e)
        {
            frmPrinting listform = new frmPrinting();
            listform.ReportType = "ApprovalTrail";
            listform.BIN = bin2.GetBin();
            listform.TaxYear = ConfigurationAttributes.CurrentYear;
            listform.BnsName = AppSettingsManager.GetBnsName(bin2.GetBin());
            listform.BnsAdd = AppSettingsManager.GetBnsAddress(bin2.GetBin());
            listform.ShowDialog();
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string sBIN = string.Empty;
            try
            {
                sBIN = dgvList[3, e.RowIndex].Value.ToString();
            }
            catch { }

            if (!string.IsNullOrEmpty(sBIN))
            {
                bin2.txtTaxYear.Text = sBIN.Substring(7, 4).ToString();
                bin2.txtBINSeries.Text = sBIN.Substring(12, 7).ToString();
            }
        }
    }
}