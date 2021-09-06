
// RMC 20130212 Data query - merged mods from Tarlac ver, inclusion of history or based on year registration
// RMC 20121212 added selection of tax/regulatory fee paid in data query module

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Amellar.Common.BusinessType;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;

namespace Amellar.Common.Tools
{
    public partial class frmDataQuery : Form
    {
        System.Threading.Thread thrProgress;
        System.Threading.Thread thrUpdateGui;

        string m_strBnsCode = "";
        string m_sCriteria = "";
        string m_sBnsCode = "";
        string m_sQuery = "";
        string m_sORDate1 = "";
        string m_sORDate2 = "";
        string m_sTaxRegCode = "";  // RMC 20121212 added selection of tax/regulatory fee paid in data query module
        bool m_bIsMainCat = false;

        public frmDataQuery()
        {
            InitializeComponent();
        }

        
        private void btnSubCategory_Click(object sender, EventArgs e)
        {
            cmbMainBnsType.Visible = false;
            txtBnsType.Visible = true;
            m_bIsMainCat = false;

            frmBusinessType frmBnsType = new frmBusinessType();
            frmBnsType.SetFormStyle(false);
            frmBnsType.ShowDialog();
            txtBnsType.Text = frmBnsType.m_sBnsDescription;
            m_strBnsCode = frmBnsType.m_strBnsCode;
        }

        private void btnMainCategory_Click(object sender, EventArgs e)
        {
            cmbMainBnsType.Visible = true;
            txtBnsType.Visible = false;
            m_bIsMainCat = true;
        }

        private void frmDataQuery_Load(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();

            m_bIsMainCat = false;

            cmbOrgKind.Items.Add("ALL");
            cmbOrgKind.Items.Add("SINGLE PROPRIETORSHIP");
            cmbOrgKind.Items.Add("PARTNERSHIP");
            cmbOrgKind.Items.Add("CORPORATION");

            cmbStatus.Items.Add("ALL");
            cmbStatus.Items.Add("NEW");
            cmbStatus.Items.Add("RENEWAL");
            cmbStatus.Items.Add("RETIRED");

            cmbDueQtr.Items.Add("ALL");
            cmbDueQtr.Items.Add("2");
            cmbDueQtr.Items.Add("3");
            cmbDueQtr.Items.Add("4");

            cmbPayTerm.Items.Add("ALL");
            cmbPayTerm.Items.Add("F");
            cmbPayTerm.Items.Add("I");

            cmbPayMode.Items.Add("ALL");
            cmbPayMode.Items.Add("ONL");
            cmbPayMode.Items.Add("OFL");
            cmbPayMode.Items.Add("POS");

            cmbSortBy.Items.Add("BUSINESS NAME");
            cmbSortBy.Items.Add("OWNER'S NAME");
            cmbSortBy.Items.Add("GROSS/CAPITAL");
            cmbSortBy.Items.Add("AMOUNT BILLED");
            cmbSortBy.Items.Add("AMOUNT PAID");

            cmbSortOrder.Items.Add("ASC");
            cmbSortOrder.Items.Add("DESC");

            cmbBnsBrgy.Items.Clear();
            cmbBnsBrgy.Items.Add("ALL");

            pRec.Query = "select distinct brgy_nm, brgy_code from brgy order by brgy_code asc";
            if(pRec.Execute())
            {
                while (pRec.Read())
                {
                    cmbBnsBrgy.Items.Add(pRec.GetString(0).Trim());
                }
            }
            pRec.Close();

            cmbBnsDist.Items.Clear();
            cmbBnsDist.Items.Add("ALL");

            pRec.Query = "select distinct dist_nm, dist_code from brgy order by dist_code";
            if(pRec.Execute())
            {
                while (pRec.Read())
                {
                    cmbBnsDist.Items.Add(pRec.GetString(0).Trim());
                }
            }
            pRec.Close();

            cmbMainBnsType.Items.Clear();
            cmbMainBnsType.Items.Add("ALL");

            pRec.Query = "select bns_desc, bns_code from bns_table where fees_code = 'B' and length(bns_code) = 2 order by bns_code";
            if(pRec.Execute())
            {
                while (pRec.Read())
                {
                    cmbMainBnsType.Items.Add(pRec.GetString("bns_desc"));
                }

                cmbMainBnsType.SelectedIndex = 0;
            }
            pRec.Close();

            // RMC 20121212 added selection of tax/regulatory fee paid in data query module (s)
            cmbTaxRegF.Items.Clear();
            cmbTaxRegF.Items.Add("");
            cmbTaxRegF.Items.Add("BUSINESS TAX");

            pRec.Query = "select fees_desc from tax_and_fees_table order by fees_code";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    cmbTaxRegF.Items.Add(pRec.GetString(0));
                }
                cmbTaxRegF.SelectedIndex = 0;
            }
            pRec.Close();
            // RMC 20121212 added selection of tax/regulatory fee paid in data query module (e)
        }

        private void cmbBnsDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();

            if (cmbBnsDist.Text.ToString().Trim() != "" && cmbBnsDist.Text.ToString().Trim() != "ALL")
            {
                cmbBnsBrgy.Items.Clear();
                cmbBnsBrgy.Items.Add("ALL");

                pRec.Query = "select distinct brgy_nm, brgy_code from brgy where dist_nm = '" + cmbBnsDist.Text.ToString().Trim() + "' order by brgy_code";
                if (pRec.Execute())
                {
                    while (pRec.Read())
                    {
                        cmbBnsBrgy.Items.Add(pRec.GetString(0).Trim());
                    }
                }
                pRec.Close();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cmbBnsBrgy.SelectedIndex = -1;
            cmbBnsDist.SelectedIndex = -1;
            cmbMainBnsType.SelectedIndex = -1;
            cmbMainBnsType.SelectedIndex = -1;
            cmbMainBnsType.Visible = false;
            cmbOrgKind.SelectedIndex = -1;
            m_bIsMainCat = false;
            txtBnsType.Text = "";
            txtBnsStreet.Text = "";
            txtBnsName.Text = "";
            txtTaxYear.Text = "";
            txtOwnFirstName.Text = "";
            txtOwnLastName.Text = "";
            chkPreGross.Checked = false;
            txtCapitalTo.Text = "";
            txtCapitalFr.Text = "";
            cmbStatus.SelectedIndex = -1;
            txtAmtBillFr.Text = "";
            txtAmtBillTo.Text = "";
            chkDueQtr.Checked = false;
            txtAmtPaidFr.Text = "";
            txtAmtPaidTo.Text = "";
            dtpOrDateTo.Value = AppSettingsManager.GetCurrentDate();
            dtpOrDateFr.Value = AppSettingsManager.GetCurrentDate();
            cmbDueQtr.SelectedIndex = -1;
            cmbPayTerm.SelectedIndex = -1;
            cmbPayMode.SelectedIndex = -1;
            cmbSortOrder.SelectedIndex = -1;
            cmbSortBy.SelectedIndex = -1;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            //string sQuery = "";
            OracleResultSet pQueryCnt = new OracleResultSet();
            //string sORDate1 = "";
            //string sORDate2 = "";
            string sTmpAmtFr = "";
            string sTmpAmtTo = "";
            DateTime odtORDate1 = AppSettingsManager.GetCurrentDate();
            DateTime odtORDate2 = AppSettingsManager.GetCurrentDate();
            bool bHasWhere = false;
            int iTotRecCnt = 0;
            
            m_sCriteria = "";
            
            // validation
            if(txtCapitalFr.Text.ToString().Trim() != "" && txtCapitalTo.Text.ToString().Trim() != "")
            {
                if(Convert.ToDouble(txtCapitalFr.Text.ToString()) > Convert.ToDouble(txtCapitalTo.Text.ToString()))
                {
                    MessageBox.Show("Invalid amount range.", "Data Query", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtCapitalTo.Focus();
                    return;
                }
            }

            if(txtAmtBillFr.Text.ToString().Trim() != "" && txtAmtBillTo.Text.ToString().Trim() != "")
            {
                if(Convert.ToDouble(txtAmtBillFr.Text.ToString()) > Convert.ToDouble(txtAmtBillTo.Text.ToString()))
                {
                    MessageBox.Show("Invalid amount range.", "Data Query", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtAmtBillTo.Focus();
                    return;
                }
            }

            if(txtAmtPaidFr.Text.ToString().Trim() != "" && txtAmtPaidTo.Text.ToString().Trim() != "")
            {
                if(Convert.ToDouble(txtAmtPaidFr.Text.ToString()) > Convert.ToDouble(txtAmtPaidTo.Text.ToString()))
                {
                    MessageBox.Show("Invalid amount range.", "Data Query", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtAmtPaidTo.Focus();
                    return;
                }
            }

            if (dtpOrDateFr.Checked)
                m_sORDate1 = string.Format("{0:MM/dd/yyyy}", dtpOrDateFr.Text);
            else
                m_sORDate1 = "";

            if (dtpOrDateTo.Checked)
                m_sORDate2 = string.Format("{0:MM/dd/yyyy}", dtpOrDateTo.Text);
            else
                m_sORDate2 = "";

            if (m_sORDate1 != "")
            {
                odtORDate1 = Convert.ToDateTime(m_sORDate1);
                if (odtORDate1.Month < 1)
                {
                    m_sORDate1 = "";
                }
            }

            if (m_sORDate2 != "")
            {
                odtORDate2 = Convert.ToDateTime(m_sORDate2);

                if (odtORDate2.Month < 1)
                {
                    m_sORDate2 = "";
                }
            }

            if (m_sORDate1 != "" && m_sORDate2 != "")
            {
                if (odtORDate1 > odtORDate2)
                {
                    MessageBox.Show("Invalid date range.", "Data Query", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            m_sQuery = "select * from businesses";
            pQueryCnt.Query = "select count(*) from businesses";

            if(txtTaxYear.Text == "" || txtTaxYear.Text == "%" || txtTaxYear.Text == "%%")
            {
            }
            else
            {
                m_sCriteria += " Tax Year: " + txtTaxYear.Text;
                if (bHasWhere == false)
                {
                    m_sQuery += " where tax_year like '" + txtTaxYear.Text + "'";
                    bHasWhere = true;

                    pQueryCnt.Query += " where tax_year like '" + txtTaxYear.Text + "'";
                }
            }
            if(txtBnsName.Text == "" || txtBnsName.Text == "%" || txtBnsName.Text == "%%")
            {
            }
            else
            {
                m_sCriteria += " Business Name: " + txtBnsName.Text;
                if (bHasWhere == false)
                {
                    m_sQuery += " where bns_nm like '" + txtBnsName.Text + "'";
                    bHasWhere = true;

                    pQueryCnt.Query += " where bns_nm like '" + txtBnsName.Text + "'";
                }
                else
                {
                    m_sQuery += " and bns_nm like '" + txtBnsName.Text + "'";

                    pQueryCnt.Query += " and bns_nm like '" + txtBnsName.Text + "'";
                }

            }
            if(txtBnsStreet.Text == "" || txtBnsStreet.Text == "%" || txtBnsStreet.Text == "%%")
            {
            }
            else
            {
                m_sCriteria += " Street: " + txtBnsStreet.Text;
                if (bHasWhere == false)
                {
                    m_sQuery += " where bns_street like '" + txtBnsStreet.Text + "%'";
                    bHasWhere = true;

                    pQueryCnt.Query += " where bns_street like '" + txtBnsStreet.Text + "%'";
                }
                else
                {
                    m_sQuery += " and bns_street like '" + txtBnsStreet.Text + "%'";

                    pQueryCnt.Query += " and bns_street like '" + txtBnsStreet.Text + "%'";
                }

            }
            if(cmbBnsBrgy.Text == "" || cmbBnsBrgy.Text == "ALL")
            {
            }
            else
            {
                m_sCriteria += " Barangay: " + cmbBnsBrgy.Text;
                if (bHasWhere == false)
                {
                    /*m_sQuery += " where bns_brgy like '" + cmbBnsBrgy.Text + "%'";
                    bHasWhere = true;

                    pQueryCnt.Query += " where bns_brgy like '" + cmbBnsBrgy.Text + "%'";*/

                    // RMC 20150505 QA reports (s)
                    m_sQuery += " where bns_brgy = '" + cmbBnsBrgy.Text + "'";
                    bHasWhere = true;

                    pQueryCnt.Query += " where bns_brgy = '" + cmbBnsBrgy.Text + "'";
                    // RMC 20150505 QA reports (e)
                }
                else
                {
                    /*m_sQuery += " and bns_brgy like '" + cmbBnsBrgy.Text + "%'";

                    pQueryCnt.Query += " and bns_brgy like '" + cmbBnsBrgy.Text + "%'";*/
                    // RMC 20150505 QA reports (s)
                    m_sQuery += " and bns_brgy = '" + cmbBnsBrgy.Text + "'";

                    pQueryCnt.Query += " and bns_brgy = '" + cmbBnsBrgy.Text + "'";
                    // RMC 20150505 QA reports (e)
                }

            }

            if(AppSettingsManager.GetConfigValue("23") == "Y")
            {
                if(cmbBnsDist.Text == "" || cmbBnsDist.Text == "ALL")
                {
                }
                else
                {
                    m_sCriteria += " District: " + cmbBnsDist.Text;
                    if (bHasWhere == false)
                    {
                        m_sQuery += " where bns_dist like '" + cmbBnsDist.Text + "'";
                        bHasWhere = true;

                        pQueryCnt.Query += " where bns_dist like '" + cmbBnsDist.Text + "'";
                    }
                    else
                    {
                        m_sQuery += " and bns_dist like '" + cmbBnsDist.Text + "'";

                        pQueryCnt.Query += " and bns_dist like '" + cmbBnsDist.Text + "'";
                    }

                }
            }
            if(cmbOrgKind.Text == "" || cmbOrgKind.Text == "ALL")
            {
            }
            else
            {
                m_sCriteria += " Org. Kind: " + cmbOrgKind.Text;
                if (bHasWhere == false)
                {
                    m_sQuery += " where orgn_kind like '" + cmbOrgKind.Text + "'";
                    bHasWhere = true;

                    pQueryCnt.Query += " where orgn_kind like '" + cmbOrgKind.Text + "'";
                }
                else
                {
                    m_sQuery += " and orgn_kind like '" + cmbOrgKind.Text + "'";

                    pQueryCnt.Query += " and orgn_kind like '" + cmbOrgKind.Text + "'";
                }

            }
            if(txtOwnLastName.Text == "" || txtOwnLastName.Text == "%" || txtOwnLastName.Text == "%%")
            {
                if(txtOwnFirstName.Text == "" || txtOwnFirstName.Text == "%" || txtOwnFirstName.Text == "%%")
                {
                }
                else
                {
                    m_sCriteria += " First Name: " + txtOwnFirstName.Text;
                    if (bHasWhere == false)
                    {
                        m_sQuery += " where own_code in (select own_code from own_names where own_fn like '" + txtOwnFirstName.Text + "')";
                        bHasWhere = true;

                        pQueryCnt.Query += " where own_code in (select own_code from own_names where own_fn like '" + txtOwnFirstName.Text + "')";
                    }
                    else
                    {
                        m_sQuery += " and own_code in (select own_code from own_names where own_fn like '" + txtOwnFirstName.Text + "')";

                        pQueryCnt.Query += " and own_code in (select own_code from own_names where own_fn like '" + txtOwnFirstName.Text + "')";
                    }

                }
            }
            else
            {
                if(txtOwnFirstName.Text == "" || txtOwnFirstName.Text == "%" || txtOwnFirstName.Text == "%%")
                {
                    m_sCriteria += " Last Name: " + txtOwnLastName.Text;
                    if (bHasWhere == false)
                    {
                        m_sQuery += " where own_code in (select own_code from own_names where own_ln like '" + txtOwnLastName.Text + "')";
                        bHasWhere = true;

                        pQueryCnt.Query += " where own_code in (select own_code from own_names where own_ln like '" + txtOwnLastName.Text + "')";
                    }
                    else
                    {
                        m_sQuery += " and own_code in (select own_code from own_names where own_ln like '" + txtOwnLastName.Text + "')";

                        pQueryCnt.Query += " and own_code in (select own_code from own_names where own_ln like '" + txtOwnLastName.Text + "')";
                    }


                }
                else
                {
                    m_sCriteria += " Owner: " + txtOwnLastName.Text + " " + txtOwnFirstName.Text;
                    if (bHasWhere == false)
                    {
                        m_sQuery += " where own_code in (select own_code from own_names where own_ln like '" + txtOwnLastName.Text + "' and own_fn like '" + txtOwnFirstName.Text + "')";
                        bHasWhere = true;

                        pQueryCnt.Query += " where own_code in (select own_code from own_names where own_ln like '" + txtOwnLastName.Text + "' and own_fn like '" + txtOwnFirstName.Text + "')";
                    }
                    else
                    {
                        m_sQuery += " and own_code in (select own_code from own_names where own_ln like '" + txtOwnLastName.Text + "' and own_fn like '" + txtOwnFirstName.Text + "')";

                        pQueryCnt.Query += " and own_code in (select own_code from own_names where own_ln like '" + txtOwnLastName.Text + "' and own_fn like '" + txtOwnFirstName.Text + "')";
                    }


                }
            }

            if (m_bIsMainCat)
            {
                if(cmbMainBnsType.Text == "" || cmbMainBnsType.Text == "ALL")
                {
                    m_sBnsCode = "";
                }
                else
                {
                    m_sCriteria += " Classification: " + cmbMainBnsType.Text;
                    if (chkSearch.Checked)
                    {
                        m_sCriteria += "(Main Only)";
                        m_sBnsCode = AppSettingsManager.GetBnsCodeByDesc(cmbMainBnsType.Text.Trim());   // RMC 20140104 corrections in data query
                        if (bHasWhere == false)
                        {
                            m_sQuery += " where bns_code like '" + m_sBnsCode + "%'";
                            bHasWhere = true;

                            pQueryCnt.Query += " where bns_code like '" + m_sBnsCode + "%'";
                        }
                        else
                        {
                            m_sQuery += " and bns_code like '" + m_sBnsCode + "%'";

                            pQueryCnt.Query += " and bns_code like '" + m_sBnsCode + "%'";
                        }
                    }
                    // GDE 20130627
                    else
                    {
                        m_sBnsCode = AppSettingsManager.GetBnsCodeByDesc(cmbMainBnsType.Text.Trim());
                        /*m_sQuery += " and bns_code like '" + m_sBnsCode + "%'";
                        pQueryCnt.Query += " and bns_code like '" + m_sBnsCode + "%'";*/

                        // RMC 20140104 corrections in data query (s)
                        if (bHasWhere == false)
                        {
                            //m_sQuery += " where bns_code like '" + m_sBnsCode + "'";
                            m_sQuery += " where bns_code like '" + m_sBnsCode + "%'";   // RMC 20170130 corrections in data query
                            bHasWhere = true;

                            //pQueryCnt.Query += " where bns_code like '" + m_sBnsCode + "'";
                            pQueryCnt.Query += " where bns_code like '" + m_sBnsCode + "%'";    // RMC 20170130 corrections in data query
                        }
                        else
                        {
                            /*m_sQuery += " and bns_code like '" + m_sBnsCode + "'";

                            pQueryCnt.Query += " and bns_code like '" + m_sBnsCode + "'";*/

                            // RMC 20170130 corrections in data query (s)
                            m_sQuery += " and bns_code like '" + m_sBnsCode + "%'";

                            pQueryCnt.Query += " and bns_code like '" + m_sBnsCode + "%'";
                            // RMC 20170130 corrections in data query (e)
                        }
                        // RMC 20140104 corrections in data query (e)
                    }
                    // GDE 20130627
                }
            }
            else
            {
                if(txtBnsType.Text == "" || txtBnsType.Text == "ALL")
                {
                    m_sBnsCode = "";
                }
                else
                {
                    m_sCriteria += " Classification: " + txtBnsType.Text;
                    if (chkSearch.Checked)
                    {
                        m_sCriteria += "(Main Only)";
                        m_sBnsCode = AppSettingsManager.GetBnsCodeByDesc(txtBnsType.Text.Trim());   // RMC 20140104 corrections in data query 
                        if (bHasWhere == false)
                        {
                            /*m_sQuery += " where bns_code like '" + m_sBnsCode + "'";
                            bHasWhere = true;

                            pQueryCnt.Query += " where bns_code like '" + m_sBnsCode + "'";*/

                            // RMC 20170130 corrections in data query (s)
                            m_sQuery += " where bns_code like '" + m_sBnsCode + "%'";
                            bHasWhere = true;

                            pQueryCnt.Query += " where bns_code like '" + m_sBnsCode + "%'";
                            // RMC 20170130 corrections in data query (e)
                        }
                        else
                        {
                            /*m_sQuery += " and bns_code like '" + m_sBnsCode + "'";

                            pQueryCnt.Query += " and bns_code like '" + m_sBnsCode + "'";*/

                            // RMC 20170130 corrections in data query (s)
                            m_sQuery += " and bns_code like '" + m_sBnsCode + "%'";

                            pQueryCnt.Query += " and bns_code like '" + m_sBnsCode + "%'";
                            // RMC 20170130 corrections in data query (e)
                        }
                    }
                    else
                    {   // RMC 20140104 corrections in data query (s)
                        m_sBnsCode = AppSettingsManager.GetBnsCodeByDesc(txtBnsType.Text.Trim());
                        if (bHasWhere == false)
                        {
                            /*m_sQuery += " where bns_code like '" + m_sBnsCode + "'";
                            bHasWhere = true;

                            pQueryCnt.Query += " where bns_code like '" + m_sBnsCode + "'";*/

                            // RMC 20170130 corrections in data query (s)
                            m_sQuery += " where bns_code like '" + m_sBnsCode + "%'";
                            bHasWhere = true;

                            pQueryCnt.Query += " where bns_code like '" + m_sBnsCode + "%'";
                            // RMC 20170130 corrections in data query (e)
                        }
                        else
                        {
                            /*m_sQuery += " and bns_code like '" + m_sBnsCode + "'";

                            pQueryCnt.Query += " and bns_code like '" + m_sBnsCode + "'";*/

                            // RMC 20170130 corrections in data query (s)
                            m_sQuery += " and bns_code like '" + m_sBnsCode + "%'";

                            pQueryCnt.Query += " and bns_code like '" + m_sBnsCode + "%'";
                            // RMC 20170130 corrections in data query (e)
                        }
                        // RMC 20140104 corrections in data query (e)
                    }
                }
            }
            if(cmbStatus.Text == "" || cmbStatus.Text == "ALL")
            {
            }
            else
            {
                m_sCriteria += " Status: " + cmbStatus.Text;
                if (bHasWhere == false)
                {
                    m_sQuery += " where bns_stat like '" + cmbStatus.Text.Substring(0, 3) + "'";
                    bHasWhere = true;

                    pQueryCnt.Query += " where bns_stat like '" + cmbStatus.Text.Substring(0, 3) + "'";
                }
                else
                {
                    m_sQuery += " and bns_stat like '" + cmbStatus.Text.Substring(0, 3) + "'";

                    pQueryCnt.Query += " and bns_stat like '" + cmbStatus.Text.Substring(0, 3) + "'";
                }

                if (txtCapitalFr.Text != "" || txtCapitalTo.Text != "")
                {
                    sTmpAmtFr = string.Format("{0:#,###.00}", Convert.ToDouble(txtCapitalFr.Text.ToString()));
                    sTmpAmtTo = string.Format("{0:#,###.00}", Convert.ToDouble(txtCapitalTo.Text.ToString()));

                    if (cmbStatus.Text.Substring(0, 3) == "NEW")
                        m_sCriteria += " Capital From: " + sTmpAmtFr + " to " + sTmpAmtTo;
                    else
                        m_sCriteria += " Gross From: " + sTmpAmtFr + " to " + sTmpAmtTo;
                }
            }

            // RMC 20130212 Data query - merged mods from Tarlac ver, inclusion of history or based on year registration (s)
            if (chkIncludeHistory.Checked)
            {
                bHasWhere = false;
                m_sQuery += " union all ";

                m_sQuery += "select * from buss_hist";
                pQueryCnt.Query += "select count(*) from buss_hist";
                    
                if (txtTaxYear.Text == "" || txtTaxYear.Text == "%" || txtTaxYear.Text == "%%")
                {
                }
                else
                {
                    m_sCriteria += " Tax Year: " + txtTaxYear.Text;
                    if (bHasWhere == false)
                    {
                        m_sQuery += " where tax_year like '" + txtTaxYear.Text + "'";
                        bHasWhere = true;

                        pQueryCnt.Query += " where tax_year like '" + txtTaxYear.Text + "'";
                    }
                }
                if (txtBnsName.Text == "" || txtBnsName.Text == "%" || txtBnsName.Text == "%%")
                {
                }
                else
                {
                    m_sCriteria += " Business Name: " + txtBnsName.Text;
                    if (bHasWhere == false)
                    {
                        m_sQuery += " where bns_nm like '" + txtBnsName.Text + "'";
                        bHasWhere = true;

                        pQueryCnt.Query += " where bns_nm like '" + txtBnsName.Text + "'";
                    }
                    else
                    {
                        m_sQuery += " and bns_nm like '" + txtBnsName.Text + "'";

                        pQueryCnt.Query += " and bns_nm like '" + txtBnsName.Text + "'";
                    }

                }
                if (txtBnsStreet.Text == "" || txtBnsStreet.Text == "%" || txtBnsStreet.Text == "%%")
                {
                }
                else
                {
                    m_sCriteria += " Street: " + txtBnsStreet.Text;
                    if (bHasWhere == false)
                    {
                        m_sQuery += " where bns_street like '" + txtBnsStreet.Text + "%'";
                        bHasWhere = true;

                        pQueryCnt.Query += " where bns_street like '" + txtBnsStreet.Text + "%'";
                    }
                    else
                    {
                        m_sQuery += " and bns_street like '" + txtBnsStreet.Text + "%'";

                        pQueryCnt.Query += " and bns_street like '" + txtBnsStreet.Text + "%'";
                    }

                }
                if (cmbBnsBrgy.Text == "" || cmbBnsBrgy.Text == "ALL")
                {
                }
                else
                {
                    m_sCriteria += " Barangay: " + cmbBnsBrgy.Text;
                    if (bHasWhere == false)
                    {
                        m_sQuery += " where bns_brgy like '" + cmbBnsBrgy.Text + "%'";
                        bHasWhere = true;

                        pQueryCnt.Query += " where bns_brgy like '" + cmbBnsBrgy.Text + "%'";
                    }
                    else
                    {
                        m_sQuery += " and bns_brgy like '" + cmbBnsBrgy.Text + "%'";

                        pQueryCnt.Query += " and bns_brgy like '" + cmbBnsBrgy.Text + "%'";
                    }

                }

                if (AppSettingsManager.GetConfigValue("23") == "Y")
                {
                    if (cmbBnsDist.Text == "" || cmbBnsDist.Text == "ALL")
                    {
                    }
                    else
                    {
                        m_sCriteria += " District: " + cmbBnsDist.Text;
                        if (bHasWhere == false)
                        {
                            m_sQuery += " where bns_dist like '" + cmbBnsDist.Text + "'";
                            bHasWhere = true;

                            pQueryCnt.Query += " where bns_dist like '" + cmbBnsDist.Text + "'";
                        }
                        else
                        {
                            m_sQuery += " and bns_dist like '" + cmbBnsDist.Text + "'";

                            pQueryCnt.Query += " and bns_dist like '" + cmbBnsDist.Text + "'";
                        }

                    }
                }
                if (cmbOrgKind.Text == "" || cmbOrgKind.Text == "ALL")
                {
                }
                else
                {
                    m_sCriteria += " Org. Kind: " + cmbOrgKind.Text;
                    if (bHasWhere == false)
                    {
                        m_sQuery += " where orgn_kind like '" + cmbOrgKind.Text + "'";
                        bHasWhere = true;

                        pQueryCnt.Query += " where orgn_kind like '" + cmbOrgKind.Text + "'";
                    }
                    else
                    {
                        m_sQuery += " and orgn_kind like '" + cmbOrgKind.Text + "'";

                        pQueryCnt.Query += " and orgn_kind like '" + cmbOrgKind.Text + "'";
                    }

                }
                if (txtOwnLastName.Text == "" || txtOwnLastName.Text == "%" || txtOwnLastName.Text == "%%")
                {
                    if (txtOwnFirstName.Text == "" || txtOwnFirstName.Text == "%" || txtOwnFirstName.Text == "%%")
                    {
                    }
                    else
                    {
                        m_sCriteria += " First Name: " + txtOwnFirstName.Text;
                        if (bHasWhere == false)
                        {
                            m_sQuery += " where own_code in (select own_code from own_names where own_fn like '" + txtOwnFirstName.Text + "')";
                            bHasWhere = true;

                            pQueryCnt.Query += " where own_code in (select own_code from own_names where own_fn like '" + txtOwnFirstName.Text + "')";
                        }
                        else
                        {
                            m_sQuery += " and own_code in (select own_code from own_names where own_fn like '" + txtOwnFirstName.Text + "')";

                            pQueryCnt.Query += " and own_code in (select own_code from own_names where own_fn like '" + txtOwnFirstName.Text + "')";
                        }

                    }
                }
                else
                {
                    if (txtOwnFirstName.Text == "" || txtOwnFirstName.Text == "%" || txtOwnFirstName.Text == "%%")
                    {
                        m_sCriteria += " Last Name: " + txtOwnLastName.Text;
                        if (bHasWhere == false)
                        {
                            m_sQuery += " where own_code in (select own_code from own_names where own_ln like '" + txtOwnLastName.Text + "')";
                            bHasWhere = true;

                            pQueryCnt.Query += " where own_code in (select own_code from own_names where own_ln like '" + txtOwnLastName.Text + "')";
                        }
                        else
                        {
                            m_sQuery += " and own_code in (select own_code from own_names where own_ln like '" + txtOwnLastName.Text + "')";

                            pQueryCnt.Query += " and own_code in (select own_code from own_names where own_ln like '" + txtOwnLastName.Text + "')";
                        }


                    }
                    else
                    {
                        m_sCriteria += " Owner: " + txtOwnLastName.Text + " " + txtOwnFirstName.Text;
                        if (bHasWhere == false)
                        {
                            m_sQuery += " where own_code in (select own_code from own_names where own_ln like '" + txtOwnLastName.Text + "' and own_fn like '" + txtOwnFirstName.Text + "')";
                            bHasWhere = true;

                            pQueryCnt.Query += " where own_code in (select own_code from own_names where own_ln like '" + txtOwnLastName.Text + "' and own_fn like '" + txtOwnFirstName.Text + "')";
                        }
                        else
                        {
                            m_sQuery += " and own_code in (select own_code from own_names where own_ln like '" + txtOwnLastName.Text + "' and own_fn like '" + txtOwnFirstName.Text + "')";

                            pQueryCnt.Query += " and own_code in (select own_code from own_names where own_ln like '" + txtOwnLastName.Text + "' and own_fn like '" + txtOwnFirstName.Text + "')";
                        }


                    }
                }

                if (m_bIsMainCat)
                {
                    if (cmbMainBnsType.Text == "" || cmbMainBnsType.Text == "ALL")
                    {
                        m_sBnsCode = "";
                    }
                    else
                    {
                        m_sCriteria += " Classification: " + cmbMainBnsType.Text;
                        if (chkSearch.Checked)
                        {
                            m_sCriteria += "(Main Only)";
                            m_sBnsCode = AppSettingsManager.GetBnsCodeByDesc(cmbMainBnsType.Text.Trim());   // RMC 20140104 corrections in data query
                            if (bHasWhere == false)
                            {
                                m_sQuery += " where bns_code like '" + m_sBnsCode + "%'";
                                bHasWhere = true;

                                pQueryCnt.Query += " where bns_code like '" + m_sBnsCode + "%'";
                            }
                            else
                            {
                                m_sQuery += " and bns_code like '" + m_sBnsCode + "%'";

                                pQueryCnt.Query += " and bns_code like '" + m_sBnsCode + "%'";
                            }
                        }
                        else
                        {
                            // RMC 20140104 corrections in data query (s)
                            m_sBnsCode = AppSettingsManager.GetBnsCodeByDesc(cmbMainBnsType.Text.Trim());
                            if (bHasWhere == false)
                            {
                                m_sQuery += " where bns_code like '" + m_sBnsCode + "'";
                                bHasWhere = true;

                                pQueryCnt.Query += " where bns_code like '" + m_sBnsCode + "'";
                            }
                            else
                            {
                                m_sQuery += " and bns_code like '" + m_sBnsCode + "'";

                                pQueryCnt.Query += " and bns_code like '" + m_sBnsCode + "'";
                            }
                            // RMC 20140104 corrections in data query (e)
                        }
                    }
                }
                else
                {
                    if (txtBnsType.Text == "" || txtBnsType.Text == "ALL")
                    {
                        m_sBnsCode = "";
                    }
                    else
                    {
                        m_sCriteria += " Classification: " + txtBnsType.Text;
                        if (chkSearch.Checked)
                        {
                            m_sCriteria += "(Main Only)";
                            m_sBnsCode = AppSettingsManager.GetBnsCodeByDesc(txtBnsType.Text.Trim());   // RMC 20140104 corrections in data query 
                            if (bHasWhere == false)
                            {
                                m_sQuery += " where bns_code like '" + m_sBnsCode + "'";
                                bHasWhere = true;

                                pQueryCnt.Query += " where bns_code like '" + m_sBnsCode + "'";
                            }
                            else
                            {
                                m_sQuery += " and bns_code like '" + m_sBnsCode + "'";

                                pQueryCnt.Query += " and bns_code like '" + m_sBnsCode + "'";
                            }
                        }
                        else
                        {   // RMC 20140104 corrections in data query (s)
                            m_sBnsCode = AppSettingsManager.GetBnsCodeByDesc(txtBnsType.Text.Trim());
                            if (bHasWhere == false)
                            {
                                m_sQuery += " where bns_code like '" + m_sBnsCode + "'";
                                bHasWhere = true;

                                pQueryCnt.Query += " where bns_code like '" + m_sBnsCode + "'";
                            }
                            else
                            {
                                m_sQuery += " and bns_code like '" + m_sBnsCode + "'";

                                pQueryCnt.Query += " and bns_code like '" + m_sBnsCode + "'";
                            }
                            // RMC 20140104 corrections in data query (e)
                        }
                    }
                }
                if (cmbStatus.Text == "" || cmbStatus.Text == "ALL")
                {
                }
                else
                {
                    m_sCriteria += " Status: " + cmbStatus.Text;
                    if (bHasWhere == false)
                    {
                        m_sQuery += " where bns_stat like '" + cmbStatus.Text.Substring(0, 3) + "'";
                        bHasWhere = true;

                        pQueryCnt.Query += " where bns_stat like '" + cmbStatus.Text.Substring(0, 3) + "'";
                    }
                    else
                    {
                        m_sQuery += " and bns_stat like '" + cmbStatus.Text.Substring(0, 3) + "'";

                        pQueryCnt.Query += " and bns_stat like '" + cmbStatus.Text.Substring(0, 3) + "'";
                    }

                    if (txtCapitalFr.Text != "" || txtCapitalTo.Text != "")
                    {
                        sTmpAmtFr = string.Format("{0:#,###.00}", Convert.ToDouble(txtCapitalFr.Text.ToString()));
                        sTmpAmtTo = string.Format("{0:#,###.00}", Convert.ToDouble(txtCapitalTo.Text.ToString()));

                        if (cmbStatus.Text.Substring(0, 3) == "NEW")
                            m_sCriteria += " Capital From: " + sTmpAmtFr + " to " + sTmpAmtTo;
                        else
                            m_sCriteria += " Gross From: " + sTmpAmtFr + " to " + sTmpAmtTo;
                    }
                }
            }
            // RMC 20130212 Data query - merged mods from Tarlac ver, inclusion of history or based on year registration (e)

            if (chkPreGross.Checked)
                m_sCriteria += " (Pre-Gross)";

            try
            {
                sTmpAmtFr = string.Format("{0:#,###.00}", Convert.ToDouble(txtAmtBillFr.Text.ToString()));
            }
            catch
            {
                sTmpAmtFr = "0.00";
            }
            try
            {
                sTmpAmtTo = string.Format("{0:#,###.00}", Convert.ToDouble(txtAmtBillTo.Text.ToString()));
            }
            catch
            {
                sTmpAmtTo = "0.00";
            }

            if(txtAmtBillFr.Text != "" || txtAmtBillTo.Text != "")
                m_sCriteria += " Bill Amount From: " + sTmpAmtFr + " to " + sTmpAmtTo;

            try
            {
                sTmpAmtFr = string.Format("{0:#,###.00}", Convert.ToDouble(txtAmtPaidFr.Text.ToString()));
            }
            catch
            {
                sTmpAmtFr = "0.00";
            }
            try
            {
                sTmpAmtTo = string.Format("{0:#,###.00}", Convert.ToDouble(txtAmtPaidTo.Text.ToString()));
            }
            catch
            {
                sTmpAmtTo = "0.00";
            }

            if(txtAmtPaidFr.Text != "" || txtAmtPaidTo.Text != "")
                m_sCriteria += " Amount Paid From: " + sTmpAmtFr + " to " + sTmpAmtTo;

            if(cmbPayMode.Text != "" && cmbPayMode.Text != "ALL")
                m_sCriteria += " Paymet Mode: " + cmbPayMode.Text;
            if(cmbPayTerm.Text != "" && cmbPayTerm.Text != "ALL")
                m_sCriteria += " Paymet Term: " + cmbPayTerm.Text;
            if(cmbDueQtr.Text != "" && cmbDueQtr.Text != "ALL")
                m_sCriteria += " Due Qtr: " + cmbDueQtr.Text;
            if (m_sORDate1 != "" || m_sORDate2 != "")
            {
                m_sCriteria += " OR Date From: " + m_sORDate1 + " to " + m_sORDate2;

                if(bHasWhere)
                    pQueryCnt.Query += " and ";
                else
                    pQueryCnt.Query += " where ";
                pQueryCnt.Query += " bin in (select bin from pay_hist where or_date between to_date('" + m_sORDate1 + "','MM/dd/yyyy') and to_date('" + m_sORDate2 + "','MM/dd/yyyy'))";
            }
            // RMC 20121212 added selection of tax/regulatory fee paid in data query module (s)
            if (cmbTaxRegF.Text != "" && cmbTaxRegF.Text != "ALL")
            {
                m_sCriteria += " Tax/Fee Paid: " + cmbTaxRegF.Text;
            }
            // RMC 20121212 added selection of tax/regulatory fee paid in data query module (e)

            /*int.TryParse(pQueryCnt.ExecuteScalar(), out iTotRecCnt);
            pBar.Minimum = 1;
            pBar.Maximum = iTotRecCnt;
            pBar.Step = 1;

            thrProgress = new System.Threading.Thread(CallGeneratePL);
            thrUpdateGui = new System.Threading.Thread(ProgressThread);

            thrProgress.Start();
            System.Threading.Thread.Sleep(500);
            thrUpdateGui.Start();
              */    // pending threading me error    
            CallGeneratePL();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbPayTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPayTerm.Text.ToString() == "I")
            {
                chkDueQtr.Enabled = true;
                cmbDueQtr.Enabled = true;
            }
            else
            {
                chkDueQtr.Enabled = false;
                cmbDueQtr.Enabled = false;
            }
        }

        private void CallGeneratePL()
        {
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            int iConvert = 0;
            AppSettingsManager.GenerationLog("DATA QUERY - GENERATE", AppSettingsManager.SystemUser.UserCode, "S");

            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "data_query_business";
            plsqlCmd.ParamValue = m_sQuery;
            plsqlCmd.AddParameter("p_sQuery", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sBnsCode;
            plsqlCmd.AddParameter("p_sBnsCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            if (m_bIsMainCat)
                iConvert = 1;
            else
                iConvert = 0;
            plsqlCmd.ParamValue = iConvert;
            plsqlCmd.AddParameter("p_iMainCat", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Input);

            if (chkSearch.Checked)
                iConvert = 1;
            else
                iConvert = 0;
            plsqlCmd.ParamValue = iConvert;
            plsqlCmd.AddParameter("p_iMainBns", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Input);

            if(chkPreGross.Checked)
                iConvert = 1;
            else
                iConvert = 0;
            plsqlCmd.ParamValue = iConvert;
            plsqlCmd.AddParameter("p_iPreGross", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Input);
            plsqlCmd.ParamValue = txtCapitalFr.Text.ToString();
            plsqlCmd.AddParameter("p_sGrossCap1", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = txtCapitalTo.Text.ToString();
            plsqlCmd.AddParameter("p_sGrossCap2", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = txtAmtBillFr.Text.ToString();
            plsqlCmd.AddParameter("p_sBillAmt1", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = txtAmtBillTo.Text.ToString();
            plsqlCmd.AddParameter("p_sBillAmt2", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = cmbPayMode.Text.ToString();
            plsqlCmd.AddParameter("p_sPayMode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = cmbPayTerm.Text.ToString();
            plsqlCmd.AddParameter("p_sPayTerm", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = cmbDueQtr.Text.ToString();
            plsqlCmd.AddParameter("p_sDueQtr", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sORDate1;
            plsqlCmd.AddParameter("p_sORDate1", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sORDate2;
            plsqlCmd.AddParameter("p_sORDate2", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = txtAmtPaidFr.Text.ToString();
            plsqlCmd.AddParameter("p_sAmtPaid1", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = txtAmtPaidTo.Text.ToString();
            plsqlCmd.AddParameter("p_sAmtPaid2", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = ConfigurationAttributes.RevYear;
            plsqlCmd.AddParameter("p_sRevYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = m_sTaxRegCode;   // RMC 20121212 added selection of tax/regulatory fee paid in data query module
            plsqlCmd.AddParameter("p_sTaxReg", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);   // RMC 20121212 added selection of tax/regulatory fee paid in data query module
            // RMC 20130212 Data query - merged mods from Tarlac ver, inclusion of history or based on year registration (s)
            if (chkIncludeHistory.Checked)
                iConvert = 1;
            else
                iConvert = 0;
            plsqlCmd.ParamValue = iConvert;
            plsqlCmd.AddParameter("p_iHistory", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Input);
            // RMC 20130212 Data query - merged mods from Tarlac ver, inclusion of history or based on year registration (e)
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();
                plsqlCmd.Close();

            }
            
            AppSettingsManager.GenerationLog("DATA QUERY - GENERATE", AppSettingsManager.SystemUser.UserCode, "E");

            if (AuditTrail.AuditTrail.InsertTrail("AUQM", "rep_query_bns report", AppSettingsManager.SystemUser.UserCode) == 0)
            {
                return;
            }
            
            PrintQuery();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkPreGross.Visible = true;

            if(cmbStatus.Text.Trim() == "" || cmbStatus.Text.Trim() == "ALL")
            {
                txtCapitalFr.Enabled = false;
                txtCapitalTo.Enabled = false;
            }
            else
            {
                if (cmbStatus.Text.Trim() == "NEW")
                {
                    lblCapital.Text = "Capital :";
                    chkPreGross.Checked = false;
                    chkPreGross.Visible = false;
                }
                else
                {
                    lblCapital.Text = "Gross   :";
                }

                txtCapitalFr.Enabled = true;
                txtCapitalTo.Enabled = true;
            }
        }

        private void PrintQuery()
        {
            string sQuery = "";

            sQuery = "select * from rep_query_bns";
            sQuery += " order by";

            // Sorted by
            if (cmbSortBy.Text.Trim() == "BUSINESS NAME")
                sQuery += " bns_nm";
            else
                if (cmbSortBy.Text.Trim() == "OWNER'S NAME")
                    sQuery += " own_nm";
                else
                    if (cmbSortBy.Text.Trim() == "GROSS/CAPITAL")
                        sQuery += " tot_grcap";
                    else
                        if (cmbSortBy.Text.Trim() == "AMOUNT BILLED")
                            sQuery += " bill_amt";
                        else
                            if (cmbSortBy.Text.Trim() == "AMOUNT PAID")
                                sQuery += " amt_paid";

            // order (asc or desc)
            if (cmbSortOrder.Text.Trim() == "ASC")
                sQuery += " asc";
            else
                if (cmbSortOrder.Text.Trim() == "DESC")
                    sQuery += " desc";

            // default order by
            if (cmbSortBy.Text.Trim() != "")
                sQuery += ",";
            sQuery += " bin, is_main desc";

            frmReport ReportFrm = new frmReport();
            ReportFrm.ReportName = "LIST OF BUSINESSES BY DATA QUERY";
            ReportFrm.Query = sQuery;
            ReportFrm.Data1 = m_sCriteria;
            ReportFrm.Data2 = cmbSortBy.Text.Trim() + " " + cmbSortOrder.Text.Trim();
            // RMC 20130212 Data query - merged mods from Tarlac ver, inclusion of history or based on year registration (s)
            if (chkIncludeHistory.Checked)
                ReportFrm.HistoryIncluded = true;
            else
                ReportFrm.HistoryIncluded = false;
            // RMC 20130212 Data query - merged mods from Tarlac ver, inclusion of history or based on year registration (e)
            ReportFrm.ShowDialog();
        }

        private void ProgressThread()
        {
            int intCountCurrent = 0;

            bool blnRunOnce = false;

            while (thrProgress.IsAlive)
            {
                blnRunOnce = true;
                System.Threading.Thread.Sleep(10);

                intCountCurrent++;
                try
                {
                    SetProgressDelegate spd = SetProgressFunc;
                    Invoke(spd, intCountCurrent);
                }
                catch
                { }

            }

            if (!blnRunOnce && !thrProgress.IsAlive)
            {
                SetProgressDelegate spd = SetProgressFunc;
                Invoke(spd, pBar.Maximum);
            }
        }


        private delegate void SetProgressDelegate(int intNumber);
        private void SetProgressFunc(int intNumber)
        {

            {
                pBar.Value = intNumber;

                if (pBar.Value == pBar.Maximum && (!thrProgress.IsAlive))
                {
                    MessageBox.Show("Process Completed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();

                }
            }
        }

        private void cmbTaxRegF_SelectedIndexChanged(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();
            m_sTaxRegCode = "";

            //pRec.Query = "select * from tax_and_fees_table where fees_desc = '" + StringUtilities.StringUtilities.HandleApostrophe(cmbTaxRegF.Text.ToString()) + "'";
            pRec.Query = "select * from tax_and_fees_table where fees_desc = '" + StringUtilities.StringUtilities.HandleApostrophe(cmbTaxRegF.Text.ToString()) + "' and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "'"; //MCR 20141118
            if (pRec.Execute())
            {
                if (pRec.Read())
                    m_sTaxRegCode = pRec.GetString("fees_code");
                else
                {
                    if (cmbTaxRegF.Text.ToString() == "BUSINESS TAX")
                        m_sTaxRegCode = "B";
                }
            }
            pRec.Close();
        }
        
    }
}