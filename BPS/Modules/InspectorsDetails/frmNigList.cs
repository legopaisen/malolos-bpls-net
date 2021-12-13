using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.StringUtilities;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.SearchBusiness;
using Amellar.Common.BPLSApp;

namespace Amellar.Modules.InspectorsDetails
{
    public partial class frmNigList : Form
    {
        public frmNigList()
        {
            InitializeComponent();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
        }

        private string m_sBusinessName = string.Empty;
        private string m_sOwnerName = string.Empty;
        private string m_sSelectedBin = string.Empty;
        private string m_sSelectedDate = string.Empty;
        private string m_sOption = string.Empty;
        private string m_sSelectedDivision = string.Empty;
        private bool isTagged = false; //AFM 20200114
        private string m_sBinDivCollection = string.Empty; //AFM 20200114
        private string m_BinDivision = string.Empty;
        private string m_sDateInsp = string.Empty;
        public string m_sSelectedDiv = string.Empty; //AFM 20200115
        BPLSAppSettingList sList = new BPLSAppSettingList();
        public string m_sBin = string.Empty;

        private void frmNigList_Load(object sender, EventArgs e)
        {
            LoadDivision();
        }

        private void LoadDivision()
        {
            dgvDivision.Rows.Clear();
            OracleResultSet pSet = new OracleResultSet();

            String sValue = AppSettingsManager.GetUserDiv(AppSettingsManager.SystemUser.UserCode);

            if (sValue == "BPLO" || AppSettingsManager.SystemUser.UserCode == "SYS_PROG")
                pSet.Query = "select distinct division_code from nigvio_tbl order by division_code asc";
            else
                pSet.Query = "select distinct division_code from nigvio_tbl where division_code = '" + sValue + "'";
            if (pSet.Execute())
                while (pSet.Read())
                {
                    dgvDivision.Rows.Add(pSet.GetString(0));
                }
            pSet.Close();
        }

        private void LoadViolation()
        {
            dgvDetails.Rows.Clear();
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from nigvio_details where division_code = '" + dgvDivision.CurrentRow.Cells[0].Value.ToString() + "' order by date_inspected asc";
            if (pSet.Execute())
                while (pSet.Read())
                {
                    dgvDetails.Rows.Add(pSet.GetString(1),pSet.GetString(2));
                }
            pSet.Close();
            dgvDetails.ClearSelection();
            btnAdd.Enabled = false;
            btnViolation.Enabled = false;
        }

        private bool ExistingNegative() //AFM 20200114
        {
            OracleResultSet result = new OracleResultSet();
            m_sBinDivCollection = string.Empty;
            result.Query = "select * from NIGVIO_DETAILS where bin = '" + bin1.GetBin() + "'";
            if (result.Execute())
            {
                while (result.Read())
                {
                    m_sBinDivCollection += result.GetString("division_code") + "\n";
                }
                if (result.Execute()) //lagyan ng if haha
                 if (!result.Read())
                    return true;
                return false;
            }

            else
                return true;
        }

        private bool IsRetired() //AFM 20200116 validation for retired business
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from businesses where bin = '" + bin1.GetBin() + "' and bns_stat = 'RET'";
            if (result.Execute())
                if (result.Read())
                    return true;
                else
                    return false;
            else
                return false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (dgvDivision.RowCount <= 0)
                return;

            if (!ExistingNegative() && btnSearch.Text != "Clear") //AFM 20200114
            {
                MessageBox.Show("BIN already tagged in\n" + m_sBinDivCollection.ToString(), "Negative List", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isTagged = true;
            }

            if (btnSearch.Text == "Clear")
            {
                this.ClearControls();
                btnSearch.Text = "Search";
                btnAdd.Enabled = false;
                btnAdd.Text = "Tag";
                bin1.txtTaxYear.Focus();
                m_sSelectedBin = "";
            }
            else
            {
                if (bin1.txtTaxYear.Text != "" || bin1.txtBINSeries.Text != "")
                {
                    if (IsRetired()) //AFM 20200116 validation for retired business
                    {
                        MessageBox.Show("BIN is already retired", "Negative List", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    m_sSelectedBin = bin1.GetBin();
                    GetNigDivision();
                    LoadBusinessInfo(m_sSelectedBin);
                    btnSearch.Text = "Clear";
                    btnAdd.Enabled = true;
                    if (isTagged == true)  //AFM 20200114
                    {
                        btnAdd.Text = "Untag";
                        btnViolation.Enabled = true;
                    }
                    else
                        btnAdd.Text = "Tag";
                    // LoadInspector(m_sSelectedBin);
                }
                else
                {
                    frmSearchBusiness frmSearchBns = new frmSearchBusiness();

                    frmSearchBns.ShowDialog();
                    if (frmSearchBns.sBIN.Length > 1)
                    {
                        bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                        bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();

                        m_sSelectedBin = bin1.GetBin();
                        LoadBusinessInfo(m_sSelectedBin);
                        btnSearch.Text = "Clear";
                        btnAdd.Enabled = true;
                        btnAdd.Text = "Tag";
                        //LoadInspector(m_sSelectedBin);
                    }
                    else
                        this.ClearControls();
                }
            }
        }

        private void LoadBusinessInfo(string strBin)
        {
            string sBnsCode = string.Empty;
            StringBuilder strQuery = new StringBuilder();
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = string.Format("select * from business_que where BIN = '{0}'", strBin);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    strQuery.Append(string.Format("select * from business_que where BIN = '{0}'", strBin));
                }
                else
                {
                    pSet.Close();

                    pSet.Query = string.Format("select * from businesses where BIN = '{0}'", strBin);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            strQuery.Append(string.Format("select * from businesses where BIN = '{0}'", strBin));
                        }
                        else
                        {
                            pSet.Close();

                            pSet.Query = string.Format("select * from buss_hist where BIN = '{0}'", strBin);
                            if (pSet.Execute())
                            {
                                if (pSet.Read())
                                {
                                    strQuery.Append(string.Format("select * from buss_hist where BIN = '{0}'", strBin));
                                }
                            }
                            pSet.Close();
                        }
                    }
                }
            }

            pSet.Query = strQuery.ToString();
            if (strQuery.ToString() == "") //JARS 20170830
            {
                MessageBox.Show("Business already renewed/paid for current tax year.");
                return;
            }
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    m_sOwnerName = AppSettingsManager.GetBnsOwner(pSet.GetString("own_code"));
                    m_sBusinessName = pSet.GetString("bns_nm");

                    txtBnsName.Text = m_sBusinessName;
                    txtBnsAdd.Text = pSet.GetString("bns_house_no").Trim();
                    txtBnsCity.Text = pSet.GetString("bns_mun").Trim();
                    txtBnsStreet.Text = pSet.GetString("bns_street").Trim();
                    txtBnsDist.Text = pSet.GetString("bns_dist").Trim();
                    txtBnsZone.Text = pSet.GetString("bns_zone").Trim();
                    txtBnsBrgy.Text = pSet.GetString("bns_brgy").Trim();
                    txtBnsProv.Text = pSet.GetString("bns_prov").Trim();
                    txtBnsOrgKind.Text = pSet.GetString("orgn_kind").Trim();
                    sBnsCode = pSet.GetString("bns_code").Trim();

                    txtOwnName.Text = m_sOwnerName;

                    sList.OwnName = pSet.GetString("own_code");
                    for (int j = 0; j < sList.OwnNamesSetting.Count; j++)
                    {
                        txtOwnAdd.Text = sList.OwnNamesSetting[j].sOwnHouseNo;
                        txtOwnStreet.Text = sList.OwnNamesSetting[j].sOwnStreet;
                        txtOwnBrgy.Text = sList.OwnNamesSetting[j].sOwnBrgy;
                        txtOwnDist.Text = sList.OwnNamesSetting[j].sOwnDist;
                        txtOwnZone.Text = sList.OwnNamesSetting[j].sOwnZone;
                        txtOwnCity.Text = sList.OwnNamesSetting[j].sOwnMun;
                        txtOwnProv.Text = sList.OwnNamesSetting[j].sOwnProv;
                        txtZip.Text = sList.OwnNamesSetting[j].sOwnZip;  // RMC 20110819
                    }

                    txtBnsType.Text = AppSettingsManager.GetBnsDesc(sBnsCode);
                }
            }
            pSet.Close();
        }

        private void GetNigDivision() //AFM 20200114
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select division_code, date_inspected from NIGVIO_DETAILS where bin = '" + bin1.GetBin() + "' and division_code = '"+ m_sSelectedDiv +"'";
            if (result.Execute())
                if (result.Read())
                {
                    //m_BinDivision = result.GetString("division_code"); //AFM 20200115 removed to avoid redundancy in new engr tool process
                    m_sDateInsp = result.GetString("date_inspected");
                }
        }

        public void ClearControls()
        {
            this.bin1.GetBINSeries = "";
            this.bin1.GetTaxYear = "";
            this.txtBnsName.Text = "";
            this.txtBnsAdd.Text = "";
            this.txtBnsBrgy.Text = "";
            this.txtBnsDist.Text = "";
            this.txtBnsOrgKind.Text = "";
            this.txtBnsStreet.Text = "";
            this.txtBnsZone.Text = "";
            this.txtBnsCity.Text = "";
            this.txtBnsProv.Text = "";
            this.txtBnsType.Text = "";
            this.txtOwnName.Text = "";
            this.txtOwnAdd.Text = "";
            this.txtOwnBrgy.Text = "";
            this.txtOwnDist.Text = "";
            this.txtOwnStreet.Text = "";
            this.txtOwnCity.Text = "";
            this.txtOwnProv.Text = "";
            this.txtOwnZone.Text = "";
            this.btnSearch.Text = "Search";
           // this.dgvViolations.Columns.Clear(); // RMC 20171116 added violation report
        }

        private bool ValidateExist()
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from nigvio_details where division_code = '" + dgvDivision.CurrentRow.Cells[0].Value.ToString() + "' and bin = '" + bin1.GetBin() + "' and date_inspected = '" + dtpDateInspected.Value.ToString("MM/dd/yyyy") + "'";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    MessageBox.Show("Record already exist!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    pSet.Close();
                    return true;
                }
            pSet.Close();
            return false;
        }

        private bool ValidateRecord()
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from nigvio_list where division_code = '" + m_sSelectedDivision + "' and bin = '" + m_sSelectedBin + "' and date_inspected = '" + m_sSelectedDate + "'";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    MessageBox.Show("Record already used!\nCannot be deleted!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    pSet.Close();
                    return true;
                }
            pSet.Close();
            return false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            if (btnAdd.Text == "Tag")
            {
                if (ValidateExist())
                    return;

                pSet.Query = "insert into nigvio_details values(:1,:2,:3)";
                pSet.AddParameter(":1", dgvDivision.CurrentRow.Cells[0].Value.ToString());
                pSet.AddParameter(":2", bin1.GetBin());
                pSet.AddParameter(":3", dtpDateInspected.Value.ToString("MM/dd/yyyy"));
                if (pSet.ExecuteNonQuery() == 0)
                { }
                LoadViolation();

                if (btnSearch.Text == "Clear")
                    btnSearch.PerformClick();
            }
            else // Untag
            {
                if (ValidateRecord())
                    return;

                if (m_sSelectedDivision == "" && m_sSelectedDate == "") //AFM 20200114
                {
                    m_sSelectedDivision = m_sSelectedDiv;
                    m_sSelectedDate = m_sDateInsp;
                }

                pSet.Query = string.Format("delete from nigvio_details where division_code = '{0}'", m_sSelectedDivision);
                pSet.Query += string.Format(" and bin = '{0}' and date_inspected = '{1}'", m_sSelectedBin, m_sSelectedDate);
                if (pSet.ExecuteNonQuery() == 0)
                { }
                LoadViolation();

                if (btnSearch.Text == "Clear")
                    btnSearch.PerformClick();
            }
        }

        private void dgvDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetails.RowCount >= 1)
            {
                btnAdd.Text = "Untag";
                btnAdd.Enabled = true;
                btnViolation.Enabled = true;
                m_sSelectedDivision = dgvDivision.CurrentRow.Cells[0].Value.ToString();
                m_sSelectedBin = dgvDetails.CurrentRow.Cells[0].Value.ToString();
                m_sSelectedDate = dgvDetails.CurrentRow.Cells[1].Value.ToString();
                LoadBusinessInfo(m_sSelectedBin);
            }
            else
            {
                btnAdd.Text = "Tag";
                btnAdd.Enabled = false;
                btnViolation.Enabled = false;
                m_sSelectedDivision = "";
                m_sSelectedBin = "";
                m_sSelectedDate = ""; 
                ClearControls();
            }
        }

        private void btnViolation_Click(object sender, EventArgs e)
        {
            using (frmOrdinanceViolation frmViolation = new frmOrdinanceViolation())
            {
                frmViolation.isNigList = true;
                if (m_sSelectedDivision == "" && m_sSelectedDate == "")
                {
                    m_sSelectedDivision = m_sSelectedDiv;
                    m_sSelectedDate = m_sDateInsp;
                }
                frmViolation.DivisionCode = m_sSelectedDivision;
                frmViolation.Bin = m_sSelectedBin;
                frmViolation.DateInspected = m_sSelectedDate;
                frmViolation.InspectorCode = AppSettingsManager.SystemUser.UserCode;
                frmViolation.ShowDialog();

                LoadViolation(); //AFM 20200101
                //LoadViolation(m_sSelectedInspector, m_sSelectedBin, m_sSelectedDate);
            }
        }

        private void dgvDivision_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnAdd.Text = "Tag";
            btnAdd.Enabled = false;
            btnViolation.Enabled = false;
            m_sSelectedDivision = "";
            m_sSelectedBin = "";
            m_sSelectedDate = "";
            btnSearch.Enabled = true; //AFM 20200115
            ClearControls();

            m_sSelectedDiv = dgvDivision.CurrentRow.Cells[0].Value.ToString(); //AFM 20200115


            if (dgvDivision.RowCount >= 1)
                LoadViolation();
        }
    }
}