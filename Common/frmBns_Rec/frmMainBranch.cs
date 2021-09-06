
// RMC 20110725 Created module for main branck link-up

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.SearchBusiness;
using Amellar.Common.BPLSApp;
using Amellar.Common.StringUtilities;

namespace Amellar.Common.frmBns_Rec
{
    public partial class frmApplicationForm : Form
    {
        BPLSAppSettingList sList = new BPLSAppSettingList();
        private string m_sAdd = string.Empty;
        private string m_sStreet = string.Empty;
        private string m_sBrgy = string.Empty;
        private string m_sQuery = string.Empty;
        public string m_sOfcType = string.Empty;
        public string m_sBIN = string.Empty;
        public string m_sBranchBIN = string.Empty;

        public frmApplicationForm()
        {
            InitializeComponent();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
            chkOutsideLGU.Text = "Outside " + ConfigurationAttributes.LGUName;
        }

        private void frmMainBranch_Load(object sender, EventArgs e)
        {
            m_sOfcType = "";

            txtMainID.Visible = false;
            bin1.Visible = true;
            btnSave.Enabled = false;

            //load existing data here
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();

            if (btnSearch.Text == "Search Bin")
            {
                frmSearchBusiness frmSearchBns = new frmSearchBusiness();
                frmSearchBns.ModuleCode = "";

                if (chkOutsideLGU.Checked)
                {
                    if (txtMainID.Text.Trim() != "")
                    {
                        frmSearchBns.m_bOusiteManilaSW = true;
                        frmSearchBns.ShowDialog();

                        if (frmSearchBns.sBIN.Length > 1)
                        {
                            txtMainID.Text = frmSearchBns.sBIN;
                            //m_sBIN = txtMainID.Text;
                        }
                        else
                        {
                            return;
                        }
                    }

                    pRec.Query = string.Format("select * from main_ofc_tbl where mid = '{0}'", txtMainID.Text.Trim());
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            txtBnsName.Text = StringUtilities.StringUtilities.RemoveApostrophe(pRec.GetString("bns_nm").Trim());
                            txtBnsType.Text = pRec.GetString("bns_type").Trim();
                            txtBnsAdd.Text = pRec.GetString("bns_add").Trim();
                            txtOwnName.Text = pRec.GetString("owner").Trim();
                        }
                        else
                        {
                            MessageBox.Show("Business does not exist", "BPLS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            txtMainID.Focus();
                            return;
                        }
                    }
                    pRec.Close();


                }
                else
                {
                    frmSearchBns.m_bOusiteManilaSW = false;

                    if (bin1.txtTaxYear.Text.Trim() == "" || bin1.txtBINSeries.Text.Trim() == "")
                    {
                        frmSearchBns.ShowDialog();

                        if (frmSearchBns.sBIN.Length > 1)
                        {
                            bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                            bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
                            m_sBIN = frmSearchBns.sBIN;
                        }
                    }

                    pRec.Query = string.Format("select * from business_que where bin = '{0}'", bin1.GetBin());
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                        }
                        else
                        {
                            pRec.Close();

                            pRec.Query = string.Format("select *  from businesses where bin = '{0}'", bin1.GetBin());
                            if (pRec.Execute())
                            {
                                if (pRec.Read())
                                {
                                }
                                else
                                {
                                    bin1.txtTaxYear.Text = "";
                                    bin1.txtBINSeries.Text = "";
                                }
                            }
                            pRec.Close();
                        }
                    }


                    pRec.Query = string.Format("select * from consol_gr where bin = '{0}' and ofc_type = 'BRANCH'", bin1.GetBin());
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            MessageBox.Show("Business was tagged as branch office.", "BPLS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                    pRec.Close();

                    string sOwnCode = string.Empty;
                    string sBnsCode = string.Empty;

                    pRec.Query = string.Format("select * from business_que where bin = '{0}'", bin1.GetBin());
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            pRec.Close();

                            pRec.Query = string.Format("select * from business_que where bin = '{0}'", bin1.GetBin());
                        }
                        else
                        {
                            pRec.Close();

                            pRec.Query = string.Format("select * from businesses where bin = '{0}'", bin1.GetBin());
                        }
                    }

                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sOwnCode = pRec.GetString("own_code").Trim();
                            sBnsCode = pRec.GetString("bns_code").Trim();
                            txtBnsName.Text = StringUtilities.StringUtilities.RemoveApostrophe(pRec.GetString("bns_nm")).Trim();
                            txtBnsType.Text = AppSettingsManager.GetBnsDesc(sBnsCode);
                            txtBnsAdd.Text = AppSettingsManager.GetBnsAddress(bin1.GetBin());
                            txtOwnName.Text = AppSettingsManager.GetBnsOwner(sOwnCode);
                            m_sBIN = bin1.GetBin();
                        }
                        else
                        {
                            MessageBox.Show("Business does not exist", "BPLS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            bin1.txtTaxYear.Focus();
                            return;
                        }
                    }
                    pRec.Close();
                }

                btnSave.Enabled = true;
                btnSearch.Text = "Clear";
            }
            else
            {
                btnSearch.Text = "Search Bin";
                ClearControls();
            }
        }

        private void LoadValues()
        {
        }

        private void chkOutsideLGU_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkOutsideLGU.CheckState.ToString() == "Checked")
            {
                ClearControls();

                txtMainID.Visible = true;
                bin1.Visible = false;
                EnableControls(true);
            }
            else
            {
                ClearControls();
                txtMainID.Visible = false;
                bin1.Visible = true;
                EnableControls(false);

            }
        }

        private void ClearControls()
        {
            txtBnsName.Text = "";
            txtBnsAdd.Text = "";
            txtBnsType.Text = "";
            txtOwnName.Text = "";
            txtMainID.Text = "";
            bin1.txtTaxYear.Text = "";
            bin1.txtBINSeries.Text = "";
        }

        private void EnableControls(bool blnEnable)
        {
            txtBnsName.ReadOnly = !blnEnable;
            txtBnsAdd.ReadOnly = !blnEnable;
            txtBnsType.ReadOnly = !blnEnable;
            txtOwnName.ReadOnly = !blnEnable;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet result = new OracleResultSet();

            if (chkOutsideLGU.Checked)
            {
                if (txtBnsName.Text.Trim() == "" || txtBnsAdd.Text.Trim() == ""
                    || txtBnsType.Text.Trim() == "" || txtOwnName.Text.Trim() == "")
                {
                    MessageBox.Show("Cannot insert null values. All fields are required.", "BPLS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                txtMainID.Text = GetIDSeries();

                pRec.Query = "insert into main_ofc_tbl values (:1,:2,:3,:4,:5)";
                pRec.AddParameter(":1", txtMainID.Text.Trim());
                pRec.AddParameter(":2", StringUtilities.StringUtilities.HandleApostrophe(txtBnsName.Text.Trim()));
                pRec.AddParameter(":3", StringUtilities.StringUtilities.HandleApostrophe(txtBnsAdd.Text.Trim()));
                pRec.AddParameter(":4", txtBnsType.Text.Trim());
                pRec.AddParameter(":5", txtOwnName.Text.Trim());
                if (pRec.ExecuteNonQuery() == 0)
                { }

                MessageBox.Show("BIN: " + txtMainID.Text + " was generated.", "BPLS", MessageBoxButtons.OK, MessageBoxIcon.Information);

                m_sBIN = txtMainID.Text;
            }

            
            m_sOfcType = "BRANCH";

            // BRANCH
            pRec.Query = string.Format("select * from consol_gr where bin = '{0}' and ofc_type <> 'SINGLE'", m_sBranchBIN);
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    result.Query = string.Format("delete from consol_gr where bin = '{0}' and ofc_type <> 'SINGLE'", m_sBranchBIN);
                    if (result.ExecuteNonQuery() == 0)
                    { }
                }
            }
            pRec.Close();

            pRec.Query = string.Format("insert into consol_gr values('{0}','{1}','{2}')", m_sBranchBIN, m_sOfcType, m_sBIN);
            if (pRec.ExecuteNonQuery() == 0)
            { }

            // MAIN
            pRec.Query = string.Format("select * from consol_gr where bin = '{0}' and ofc_type <> 'SINGLE'", m_sBIN);
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    result.Query = string.Format("delete from consol_gr where bin = '{0}' and ofc_type <> 'SINGLE'", m_sBIN);
                    if (result.ExecuteNonQuery() == 0)
                    { }
                }
            }
            pRec.Close();

            pRec.Query = string.Format("insert into consol_gr values('{0}','MAIN','{1}')", m_sBIN, m_sBIN);
            if (pRec.ExecuteNonQuery() == 0)
            { }

            MessageBox.Show("Business successfully tagged as branch of BIN: " + m_sBIN, "BPLS", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private string GetIDSeries()
        {
            OracleResultSet pRec = new OracleResultSet();

            int iSeries = 0;
            string sSeries = "";

            pRec.Query = string.Format("select count(*) as iCount from main_ofc_tbl");
            int.TryParse(pRec.ExecuteScalar(), out iSeries);

            iSeries = iSeries + 1;
            sSeries = string.Format("{0:###}", iSeries);
            bool bBool = true;

            while (bBool)
            {
                pRec.Query = string.Format("select * from main_ofc_tbl where mid >= '{0}'", sSeries);
                if (pRec.Execute())
                {
                    if (pRec.Read())
                    {
                        iSeries = iSeries + 1;
                    }
                    else
                    {
                        bBool = false;
                    }
                }
                pRec.Close();
            }

            sSeries = string.Format("{0:###}", iSeries);

            return sSeries;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
