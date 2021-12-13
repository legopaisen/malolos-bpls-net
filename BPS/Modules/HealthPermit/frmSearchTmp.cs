

//AFM 20211213 MODIFIED CLASS FOR OTHER OFFICES (BASED FROM SANTIAGO)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AppSettings;
using Amellar.Modules.EPS;
using Amellar.Common.LogIn;
using Amellar.Common.TransactionLog;
using Amellar.BPLS.Billing;
using Amellar.Modules.InspectorsDetails;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.HealthPermit
{
    public partial class frmSearchTmp : Form
    {
        private string m_sBIN = string.Empty;
        private string m_sPermit = string.Empty;
        private string m_sOffice = string.Empty;
        private string m_sBnsStat = string.Empty;
        private DateTime m_dTransLogIn = AppSettingsManager.GetSystemDate();
        private bool m_bInit = true;
        private string m_sSwitch = string.Empty;


        public string TaxYear
        {
            get { return txtTaxYear.Text; }
            set { txtTaxYear.Text = value; }
        }

        public string Switch
        {
            get { return m_sSwitch; }
            set { m_sSwitch = value; }
        }

        public string BIN
        {
            get { return m_sBIN; }
            set { m_sBIN = value; }
        }

        public string LastName
        {
            get { return txtLastName.Text; }
            set { txtLastName.Text = value; }
        }

        public string FirstName
        {
            get { return txtFirstName.Text; }
            set { txtFirstName.Text = value; }
        }

        public string MI
        {
            get { return txtMiddleInitial.Text; }
            set { txtMiddleInitial.Text = value; }
        }

        public string BnsName
        {
            get { return txtBnsName.Text; }
            set { txtBnsName.Text = value; }
        }

        public string BnsAdd
        {
            get { return txtBnsAdd.Text; }
            set { txtBnsAdd.Text = value; }
        }

        public string Permit
        {
            get { return m_sPermit; }
            set { m_sPermit = value; }
        }
        public string Office
        {
            get { return m_sOffice; }
            set { m_sOffice = value; }
        }

        public string BnsStat
        {
            get { return m_sBnsStat; }
            set { m_sBnsStat = value; }
        }

        public frmSearchTmp()
        {
            InitializeComponent();
        }

        private void frmSearchTmp_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            OracleResultSet pSet = new OracleResultSet();
            // clean-up
            pSet.Query = "delete from trans_for_approve_tmp where bin in ";
            pSet.Query += " (select bin from trans_approve where office_nm = '" + m_sOffice + "' and tax_year = '" + ConfigurationAttributes.CurrentYear + "')";
            pSet.Query += " and tax_year = '" + ConfigurationAttributes.CurrentYear + "' and office_nm = '" + m_sOffice + "'";
            pSet.ExecuteNonQuery();

            pSet.Query = "delete from trans_for_approve_tmp where office_nm = '" + m_sOffice + "' and app_by = '" + AppSettingsManager.SystemUser.UserCode + "'";
            pSet.ExecuteNonQuery();

            if (m_sSwitch == "LISTAPPROVE")
            {
                this.Text = "MODIFY APPROVAL";
                btnProceed.Visible = false;
                btnApprove.Visible = true;
                btnApprove.Text = "Disapprove";

                //UpdateList();
                btnSearch.Visible = true;
                txtBIN.ReadOnly = false;


            }
            else
            {
                this.Text = "APPROVAL";
                if (m_sOffice == "ENGINEERING")
                {
                    btnApprove.Visible = false;
                    btnProceed.Visible = true;
                    btnNegList.Visible = true;
                }
                if (m_sOffice == "HEALTH")
                {
                    btnApprove.Visible = true;
                    btnProceed.Visible = true;
                    btnProceed.Text = "View Employees";
                    btnNegList.Visible = true;
                }
                if (m_sOffice == "CENRO" || m_sOffice == "MARKET" || m_sOffice == "PLANNING")
                {
                    btnApprove.Visible = true;
                    btnProceed.Visible = false;
                    btnNegList.Visible = true;
                }
                if (m_sOffice == "BPLO")
                {
                    btnApprove.Visible = false;
                    btnProceed.Visible = true;
                    int iCnt = 0;

                    btnRebill.Visible = true;
                }


                UpdateList();
            }

            /*dgvList.Columns.Clear();

            dgvList.Columns.Add("1", "Temp BIN");
            dgvList.Columns.Add("2", "BIN");
            dgvList.Columns.Add("3", "Business Name");
            dgvList.Columns.Add("4", "Tax Year");
            dgvList.Columns.Add("5", "Last Name");
            dgvList.Columns.Add("6", "First Name");
            dgvList.Columns.Add("7", "M.I.");
            dgvList.Columns.Add("8", "Business Address");
            dgvList.Columns.Add("9", "Occupation");
            dgvList.Columns[0].Width = 80;
            dgvList.Columns[1].Width = 100;
            dgvList.Columns[2].Width = 100;
            dgvList.Columns[3].Width = 80;
            dgvList.Columns[4].Width = 100;
            dgvList.Columns[5].Width = 100;
            dgvList.Columns[6].Width = 20;
            dgvList.Columns[7].Width = 100;
            dgvList.Columns[8].Width = 100;

            OracleResultSet pSet = new OracleResultSet();

            // RMC 20150117 (s)

            
            string sLN = ""; string sFN = ""; string sMI = ""; string sBIN = "";

            if (m_sPermit == "Annual Inspection")
            {
                pSet.Query = "select distinct temp_bin, bin, bns_nm, tax_year, bns_add,emp_occupation from emp_names where ";
                pSet.Query += "tax_year = '" + txtTaxYear.Text + "' ";
                pSet.Query += "and temp_bin like '" + txtTaxYear.Text + "%' ";
                pSet.Query += "and bin not in (select bin from annual_insp where tax_year = '" + txtTaxYear.Text + "') ";
                pSet.Query += "union all ";
                pSet.Query += "select distinct temp_bin, bin, bns_nm, tax_year, bns_add,emp_occupation from emp_names where ";
                pSet.Query += "tax_year = '" + txtTaxYear.Text + "' ";
                pSet.Query += "and temp_bin like '" + txtTaxYear.Text + "%' ";
                pSet.Query += "and trim(bin) is null and temp_bin not in (select bin from annual_insp where tax_year = '" + txtTaxYear.Text + "') ";
            }
            else
            {
                pSet.Query = "select distinct temp_bin, bin, bns_nm, tax_year, bns_add, emp_occupation from emp_names where ";
                pSet.Query += "tax_year = '" + txtTaxYear.Text + "' ";
                pSet.Query += "and temp_bin like '" + txtTaxYear.Text + "%' ";
                if (m_sPermit == "Application" || m_sPermit == "Zoning")
                    pSet.Query += "and (trim(bin) is null or temp_bin = bin)"; //MCR 20150121
                    //pSet.Query += "and trim(bin) is null ";
            }
            pSet.Query += "order by bns_nm";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sBIN = pSet.GetString(0); 

                    GetOwnName(sBIN, out sLN, out sFN, out sMI);

                    dgvList.Rows.Add(sBIN, pSet.GetString(1), pSet.GetString(2),
                        pSet.GetString(3), sLN, sFN, sMI, pSet.GetString(4), pSet.GetString(5));
                }
            }
            pSet.Close();
            */

            // RMC 20150117 (e)

            //pSet.Query = "select distinct (TEMP_BIN), BIN, bns_nm, TAX_YEAR, EMP_LN, EMP_FN, EMP_MI, bns_add from emp_names where tax_year = '" + txtTaxYear.Text + "' and emp_occupation = 'OWNER' order by temp_bin";   // RMC 20141228 modified permit printing (lubao)
            /*pSet.Query = "select distinct (TEMP_BIN), BIN, bns_nm, TAX_YEAR, EMP_LN, EMP_FN, EMP_MI, bns_add from emp_names where tax_year = '" + txtTaxYear.Text + "' order by temp_bin";
                
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    // RMC 20150102 mods in permit (s)
                    bool bInclude = false;
                    if (pSet.GetString(1).Trim() == "")
                    {
                        if (m_sPermit == "Application")
                        {
                            bInclude = true;
                        }
                        else
                            bInclude = ValidateBIN(pSet.GetString(0));
                    }
                    else if (pSet.GetString(1).Length < 12)
                    {
                        if (m_sPermit == "Application")
                            bInclude = true;
                    }
                    else
                        bInclude = ValidateBIN(pSet.GetString(1));

                    if (bInclude)   // RMC 20150102 mods in permit (e)
                    {
                        dgvList.Rows.Add(pSet.GetString(0), pSet.GetString(1), pSet.GetString(2),
                        pSet.GetString(3), pSet.GetString(4), pSet.GetString(5), pSet.GetString(6), pSet.GetString(7));
                    }
                }
            }
            pSet.Close();*/

        }
        private void UpdateList()
        {
            if (m_sSwitch == "LISTAPPROVE")
            {
                if (btnSearch.Text == "Clear BIN")
                { }
                else
                    UpdateListApp("");
            }
            else
            {
                UpdateListForApp();
            }
        }
        private void UpdateListApp(string p_sBIN)
        {
            dgvList.Columns.Clear();

            dgvList.Columns.Add("1", "Seq.");
            dgvList.Columns.Add("2", "Violation");
            dgvList.Columns.Add("3", "BIN");
            dgvList.Columns.Add("4", "Business Name");
            dgvList.Columns.Add("5", "Tax Year");
            dgvList.Columns.Add("6", "Last Name");
            dgvList.Columns.Add("7", "First Name");
            dgvList.Columns.Add("8", "M.I.");
            dgvList.Columns.Add("9", "Business Address");
            dgvList.Columns.Add("10", "Bns Stat");
            dgvList.Columns.Add("11", "Area");
            dgvList.Columns.Add("12", "Lessor");

            dgvList.Columns[0].Width = 0;
            dgvList.Columns[1].Width = 180;
            dgvList.Columns[2].Width = 200;
            dgvList.Columns[3].Width = 80;
            dgvList.Columns[4].Width = 100;
            dgvList.Columns[5].Width = 100;
            dgvList.Columns[6].Width = 20;
            dgvList.Columns[7].Width = 180;
            dgvList.Columns[8].Width = 80;
            dgvList.Columns[9].Width = 80;
            dgvList.Columns[10].Width = 80;
            dgvList.Columns[11].Width = 80;

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pRec = new OracleResultSet();

            string sLN = ""; string sFN = ""; string sMI = ""; string sBIN = "";
            string sBnsHouseNo = ""; string sBnsStreet = ""; string sBnsBrgy = ""; string sAddress = ""; string sBnsMun = "";
            string sBnsStat = ""; string sArea = "";
            string sAppID = ""; string sPlace = string.Empty;
            string sLessor = string.Empty;

            // clean-up
            pRec.Query = "delete from trans_for_approve_tmp where bin in ";
            pRec.Query += " (select bin from trans_approve where office_nm = '" + m_sOffice + "' and tax_year = '" + ConfigurationAttributes.CurrentYear + "')";
            pRec.Query += " and tax_year = '" + ConfigurationAttributes.CurrentYear + "' and office_nm = '" + m_sOffice + "'";
            pRec.ExecuteNonQuery();

            pSet.Query = "delete from trans_for_approve_tmp where office_nm = '" + m_sOffice + "' and app_by = '" + AppSettingsManager.SystemUser.UserCode + "'";
            pSet.ExecuteNonQuery();

            pRec.Query = "select * from trans_approve where office_nm = '" + m_sOffice + "'";
            if (!string.IsNullOrEmpty(p_sBIN))
                pRec.Query += " and bin = '" + p_sBIN + "'";
            pRec.Query += " and tax_year = '" + ConfigurationAttributes.CurrentYear + "' order by bin";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBIN = pRec.GetString("bin");
                    sAppID = string.Format("{0:###}", pRec.GetInt(1));

                    pSet.Query = "select bin,bns_nm,tax_year, bns_house_no,bns_street,bns_brgy, bns_mun, own_code,bns_stat,flr_area, place_occupancy, busn_own from business_que ";
                    pSet.Query += " where tax_year = '" + AppSettingsManager.GetConfigValue("12") + "' and bin = '" + sBIN + "'";
                    if (m_sOffice == "MARKET")
                        pSet.Query += " and bns_street like '%MARKET%' ";

                    bool bDisplay = true;
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

                            if (AppSettingsManager.IsForSoaPrint(sBIN, ConfigurationAttributes.CurrentYear))    // IF BILLED CANNOT BE DISAPPROVED
                                bDisplay = false;
                            else
                                bDisplay = true;

                            if (bDisplay)
                                dgvList.Rows.Add(sAppID, AppSettingsManager.GetViolation(m_sOffice, sBIN, ""), sBIN, pSet.GetString(1), pSet.GetString(2),
                                sLN, sFN, sMI, sAddress, sBnsStat, sArea, sLessor);
                        }

                    }
                    pSet.Close();
                }
            }
            pRec.Close();
        }

        private void UpdateListForApp()
        {
            dgvList.Columns.Clear();

            //dgvList.Columns.Add("1", "Temp BIN");
            dgvList.Columns.Add("1", "Seq.");
            dgvList.Columns.Add("2", "Violation");
            dgvList.Columns.Add("3", "BIN");
            dgvList.Columns.Add("4", "Business Name");
            dgvList.Columns.Add("5", "Tax Year");
            dgvList.Columns.Add("6", "Last Name");
            dgvList.Columns.Add("7", "First Name");
            dgvList.Columns.Add("8", "M.I.");
            dgvList.Columns.Add("9", "Business Address");
            dgvList.Columns.Add("10", "Bns Stat");
            dgvList.Columns.Add("11", "Area");
            dgvList.Columns.Add("12", "Lessor");
            //dgvList.Columns.Add("9", "Occupation");
            //dgvList.Columns[0].Width = 80;
            dgvList.Columns[0].Width = 50;
            dgvList.Columns[1].Width = 150;
            dgvList.Columns[2].Width = 180;
            dgvList.Columns[3].Width = 200;
            dgvList.Columns[4].Width = 80;
            dgvList.Columns[5].Width = 100;
            dgvList.Columns[6].Width = 100;
            dgvList.Columns[7].Width = 20;
            dgvList.Columns[8].Width = 180;
            dgvList.Columns[9].Width = 80;
            dgvList.Columns[10].Width = 80;
            dgvList.Columns[11].Width = 80;



            //dgvList.Columns[8].Width = 100;

            OracleResultSet pSet = new OracleResultSet();

            string sLN = ""; string sFN = ""; string sMI = ""; string sBIN = "";
            string sBnsHouseNo = ""; string sBnsStreet = ""; string sBnsBrgy = ""; string sAddress = ""; string sBnsMun = "";
            string sBnsStat = ""; string sArea = "";
            string sAppID = "";

            OracleResultSet pRec = new OracleResultSet();

            pRec.Query = "select bin, app_id from app_permit_tmp ";
            pRec.Query += " where tax_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
            pRec.Query += " and bin not in (select bin from trans_for_approve_tmp where tax_year = '" + AppSettingsManager.GetConfigValue("12") + "' and office_nm = '" + m_sOffice + "') ";
            pRec.Query += " and bin not in (select bin from trans_approve where tax_year = '" + AppSettingsManager.GetConfigValue("12") + "' and office_nm = '" + m_sOffice + "') ";
            // add hierarchy of approval
            if (m_sOffice == "HEALTH")
                pRec.Query += " and bin in (select bin from trans_approve where office_nm = 'PLANNING' and tax_year = '" + AppSettingsManager.GetConfigValue("12") + "')";
            if (m_sOffice == "CENRO")
            {
                pRec.Query += " and bin in (select bin from trans_approve where office_nm = 'HEALTH' and tax_year = '" + AppSettingsManager.GetConfigValue("12") + "')";
            }
            //pRec.Query += " and bin = '175-00-2020-0008966' ";
            pRec.Query += " order by app_id";
            bool bDisplay = true;
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
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

                            if (sBnsStat == "NEW")
                            {
                                if (m_sOffice == "ENGINEERING")
                                {
                                    if (!ValidateApproval(sBIN, "PLANNING"))
                                        bDisplay = false;
                                }
                            }
                            else
                            {
                                if (m_sOffice == "PLANNING")
                                {
                                    if (!ValidateApproval(sBIN, "ENGINEERING"))
                                        bDisplay = false;
                                }
                            }

                            if (m_sOffice == "BPLO")
                            {
                                if (!ValidateRecord(sBIN, bMarket))
                                    bDisplay = false;
                                else
                                    bDisplay = true;
                            }

                            if (bDisplay)
                            {
                                dgvList.Rows.Add(sAppID, AppSettingsManager.GetViolation(m_sOffice, sBIN, ""), sBIN, pSet.GetString(1), pSet.GetString(2),
                                    sLN, sFN, sMI, sAddress, sBnsStat, sArea, sLessor);


                            }
                        }

                    }
                    pSet.Close();

                }
            }
            pRec.Close();

        }

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBIN.Text))
            {
                MessageBox.Show("Select BIN first", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            // RMC 20141228 modified permit printing (lubao) (s)
            if (txtBIN.Text.Trim() != "")
                m_sBIN = txtBIN.Text;
            else
                m_sBIN = txtTBIN.Text;
            // RMC 20141228 modified permit printing (lubao) (e)

            if (string.IsNullOrEmpty(m_sBIN))
                return;

            if (m_sOffice == "ENGINEERING")
            {
                if (!TaskMan.IsObjectLock(m_sBIN, m_sOffice, "ADD", "ASS"))
                {
                    using (frmEPS frmeps = new frmEPS())
                    {
                        frmeps.Office = m_sOffice;
                        frmeps.BIN = m_sBIN;
                        frmeps.m_dTransLogIn = m_dTransLogIn;
                        frmeps.ShowDialog();

                        OracleResultSet pRec = new OracleResultSet();
                        pRec.Query = "delete from trans_for_approve_tmp where bin = '" + m_sBIN + "' and tax_year = '" + AppSettingsManager.GetConfigValue("12") + "' and office_nm = '" + m_sOffice + "'";
                        pRec.ExecuteNonQuery();

                        if (TaskMan.IsObjectLock(m_sBIN, m_sOffice, "DELETE", "ASS"))  // RMC 20141228 modified permit printing (lubao)
                        {
                        }

                        UpdateList();

                        txtFirstName.Text = "";
                        txtLastName.Text = "";
                        txtMiddleInitial.Text = "";
                        txtTBIN.Text = "";
                        txtBIN.Text = "";
                        txtBnsName.Text = "";
                        txtBnsAdd.Text = "";
                        txtArea.Text = "";
                        txtLessor.Text = "";
                        txtViolation.Text = "";
                        txtStatus.Text = "";
                        txtOwnAdd.Text = "";
                        txtTelNo.Text = "";
                        cmbBussLine.Items.Clear();
                    }
                }
            }
            if (m_sOffice == "HEALTH")
            {
                if (string.IsNullOrEmpty(txtBIN.Text))
                {
                    MessageBox.Show("BIN is required", "List", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                frmPrinting listform = new frmPrinting();
                listform.ReportType = "HealthList";
                listform.BIN = txtBIN.Text;
                listform.TaxYear = txtTaxYear.Text;
                listform.ShowDialog();
            }
            if (m_sOffice == "BPLO")
            {
                using (frmBilling BillingForm = new frmBilling())
                {
                    BillingForm.SourceClass = "Billing";
                    BillingForm.Text = "Billing";
                    if (!string.IsNullOrEmpty(txtBIN.Text))
                        BillingForm.BIN = txtBIN.Text;
                    BillingForm.ShowDialog();
                    BillingForm.Dispose();

                    OracleResultSet pRec = new OracleResultSet();
                    pRec.Query = "delete from trans_for_approve_tmp where bin = '" + txtBIN.Text + "' and tax_year = '" + txtTaxYear.Text + "' and office_nm = '" + m_sOffice + "'";
                    pRec.ExecuteNonQuery();

                    if (TaskMan.IsObjectLock(txtBIN.Text, m_sOffice, "DELETE", "ASS"))  // RMC 20141228 modified permit printing (lubao)
                    {
                    }
                }

                UpdateList();

                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtMiddleInitial.Text = "";
                txtTBIN.Text = "";
                txtBIN.Text = "";
                txtBnsName.Text = "";
                txtBnsAdd.Text = "";
                txtArea.Text = "";
                txtLessor.Text = "";
                txtViolation.Text = "";
                txtStatus.Text = "";
                txtOwnAdd.Text = "";
                txtTelNo.Text = "";
                cmbBussLine.Items.Clear();

            }
            //this.Close();
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!m_bInit)
                {
                    if (!String.IsNullOrEmpty(txtBIN.Text.ToString()))
                        TransLog.UpdateLog(txtBIN.Text, m_sBnsStat, ConfigurationAttributes.CurrentYear, m_sOffice + "-CLEAR", m_dTransLogIn, AppSettingsManager.GetSystemDate());
                }

                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = "delete from trans_for_approve_tmp where bin = '" + txtBIN.Text + "' and office_nm = '" + m_sOffice + "'";
                pSet.ExecuteNonQuery();

                m_dTransLogIn = AppSettingsManager.GetSystemDate();

                //txtTBIN.Text = dgvList[0, e.RowIndex].Value.ToString();
                txtViolation.Text = dgvList[1, e.RowIndex].Value.ToString();
                txtBIN.Text = dgvList[2, e.RowIndex].Value.ToString();
                txtBnsName.Text = dgvList[3, e.RowIndex].Value.ToString();
                txtTaxYear.Text = dgvList[4, e.RowIndex].Value.ToString();
                txtLastName.Text = dgvList[5, e.RowIndex].Value.ToString();
                txtFirstName.Text = dgvList[6, e.RowIndex].Value.ToString();
                txtMiddleInitial.Text = dgvList[7, e.RowIndex].Value.ToString();
                txtBnsAdd.Text = dgvList[8, e.RowIndex].Value.ToString();
                txtStatus.Text = dgvList[9, e.RowIndex].Value.ToString();
                txtArea.Text = dgvList[10, e.RowIndex].Value.ToString();
                txtLessor.Text = dgvList[11, e.RowIndex].Value.ToString();
                m_sBIN = txtBIN.Text;
                m_sBnsStat = txtStatus.Text;
                if (txtStatus.Text == "REN")
                    txtStatus.Text = "RENEWAL";


                {
                    LoadOtherInfo();
                    pSet.Query = "insert into trans_for_approve_tmp values (";
                    pSet.Query += " '" + txtBIN.Text + "', ";
                    pSet.Query += "'" + txtTaxYear.Text + "', ";
                    pSet.Query += "'" + m_sOffice + "', ";
                    pSet.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', ";
                    pSet.Query += "to_date('" + AppSettingsManager.GetCurrentDate().ToShortDateString() + "','MM/dd/yyyy'))";
                    pSet.ExecuteNonQuery();
                }
                m_bInit = false;
                // UpdateList(); // temp removed 20210420
            }
            catch
            {
                MessageBox.Show("Error encountered in fetching data");

                txtTBIN.Text = "";
                txtBIN.Text = "";
                txtBnsName.Text = "";
                txtTaxYear.Text = "";
                txtLastName.Text = "";
                txtFirstName.Text = "";
                txtMiddleInitial.Text = "";
                txtBnsAdd.Text = "";
                txtArea.Text = "";
                txtLessor.Text = "";
                txtViolation.Text = "";
                txtStatus.Text = "";
                txtOwnAdd.Text = "";
                txtTelNo.Text = "";
                cmbBussLine.Items.Clear();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "delete from trans_for_approve_tmp where bin = '" + txtBIN.Text + "' and office_nm = '" + m_sOffice + "'";
            pSet.ExecuteNonQuery();

            if (!string.IsNullOrEmpty(txtBIN.Text.ToString()))
                TransLog.UpdateLog(txtBIN.Text, m_sBnsStat, ConfigurationAttributes.CurrentYear, m_sOffice + "-CLEAR", m_dTransLogIn, AppSettingsManager.GetSystemDate());

            UpdateList();

            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtMiddleInitial.Text = "";
            txtTBIN.Text = "";
            txtBIN.Text = "";
            txtBnsName.Text = "";
            txtBnsAdd.Text = "";
            m_sBnsStat = "";
            txtArea.Text = "";
            txtLessor.Text = "";
            txtViolation.Text = "";
            txtStatus.Text = "";
            txtOwnAdd.Text = "";
            txtTelNo.Text = "";
            cmbBussLine.Items.Clear();
        }

        private bool ValidateBIN(string sBIN)
        {
            // RMC 20150102 mods in permit
            OracleResultSet pSet = new OracleResultSet();
            int iCnt = 0;

            pSet.Query = "select count(*) from business_que where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
            pSet.Query += " and bns_stat = 'NEW'";
            int.TryParse(pSet.ExecuteScalar(), out iCnt);
            if (iCnt > 0)
            {
                if (m_sPermit == "Annual Inspection" || m_sPermit == "Planning")
                {
                    pSet.Close();
                    if (m_sPermit == "Annual Inspection")
                        pSet.Query = "select count(*) from annual_insp where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
                    else
                        pSet.Query = "select count(*) from zoning where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
                    int.TryParse(pSet.ExecuteScalar(), out iCnt);
                    if (iCnt == 0)
                    {
                        return true;

                    }
                    else
                        return false;
                }

                return true;
            }
            else
            {
                pSet.Close();
                pSet.Query = "select count(*) from businesses where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
                pSet.Query += " and bns_stat = 'NEW'";
                int.TryParse(pSet.ExecuteScalar(), out iCnt);
                if (iCnt > 0)
                {
                    if (m_sPermit == "Annual Inspection" || m_sPermit == "Planning")
                    {
                        if (m_sPermit == "Annual Inspection")
                            pSet.Query = "select count(*) from annual_insp where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
                        else
                            pSet.Query = "select count(*) from zoning where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
                        int.TryParse(pSet.ExecuteScalar(), out iCnt);
                        if (iCnt == 0)
                        {
                            return true;
                        }
                        else
                            return false;
                    }

                    return true;
                }
                else
                {
                    pSet.Close();

                    if (m_sPermit == "Annual Inspection" || m_sPermit == "Planning")
                    {
                        if (m_sPermit == "Annual Inspection")
                            pSet.Query = "select count(*) from annual_insp where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
                        else
                            pSet.Query = "select count(*) from zoning where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
                        int.TryParse(pSet.ExecuteScalar(), out iCnt);
                        if (iCnt == 0)
                        {
                            if (StringUtilities.Left(sBIN, 4) == txtTaxYear.Text)
                                return true;
                            else
                                return false;
                        }
                    }
                    else
                    {
                        //if (m_sPermit == "Health" || m_sPermit == "Sanitary") //MCR 20150116
                        if (m_sPermit == "Health" || m_sPermit == "Sanitary" || m_sPermit == "Application")
                            return true;
                        else
                            return false;
                    }
                }
            }

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

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (btnApprove.Text == "Disapprove")
            {
                if (string.IsNullOrEmpty(txtBIN.Text))
                {
                    MessageBox.Show("Select BIN to disapprove first", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (AppSettingsManager.IsForSoaPrint(txtBIN.Text, ConfigurationAttributes.CurrentYear))
                {
                    MessageBox.Show("BIN already been billed.\nDisapprove not allowed.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateList();
                    return;
                }

                if (MessageBox.Show("Disapprove this record?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                frmLogIn fLog = new frmLogIn();
                fLog.sFormState = "LOGIN";
                fLog.Text = "Approving Officer";
                fLog.m_sUserCode = AppSettingsManager.SystemUser.UserCode;
                fLog.ShowDialog();

                OracleResultSet pSet = new OracleResultSet();

                if (fLog.m_objSystemUser.UserCode != string.Empty)
                {
                    pSet.Query = string.Format("insert into TRANS_APPROVE_HIST (select a.*, '{0}' from trans_approve a where bin = '{1}' and tax_year >= '{2}' and office_nm = '{3}')", "DISAPPROVED", txtBIN.Text, txtTaxYear.Text, m_sOffice);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = "delete from trans_approve where bin = '" + txtBIN.Text + "' and office_nm = '" + m_sOffice + "'";
                    pSet.Query += " and tax_year >= '" + txtTaxYear.Text + "'";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    if (TaskMan.IsObjectLock(txtBIN.Text, m_sOffice, "DELETE", "ASS"))  // RMC 20141228 modified permit printing (lubao)
                    {
                    }

                    TransLog.UpdateLog(txtBIN.Text, m_sBnsStat, ConfigurationAttributes.CurrentYear, m_sOffice + "-DISAPPROVED", m_dTransLogIn, AppSettingsManager.GetSystemDate());

                    MessageBox.Show("Record disapproved", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (MessageBox.Show("Record not approved in this office.\nContinue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                }

            }
            else
            {
                if (string.IsNullOrEmpty(txtBIN.Text))
                {
                    MessageBox.Show("Select BIN to approve first", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (MessageBox.Show("Approve this record?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                OracleResultSet pSet = new OracleResultSet();

                frmLogIn fLog = new frmLogIn();
                fLog.sFormState = "LOGIN";
                fLog.Text = "Approving Officer";
                fLog.m_sUserCode = AppSettingsManager.SystemUser.UserCode;
                fLog.ShowDialog();

                if (fLog.m_objSystemUser.UserCode != string.Empty)
                {
                    pSet.Query = "delete from trans_approve where bin = '" + txtBIN.Text + "'";
                    pSet.Query += " and tax_year = '" + ConfigurationAttributes.CurrentYear + "'";
                    pSet.Query += " and office_nm = '" + m_sOffice + "'";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    if (!string.IsNullOrEmpty(txtBIN.Text))
                    {
                        pSet.Query = "insert into trans_approve values (";
                        pSet.Query += "'" + txtBIN.Text + "', ";
                        pSet.Query += "'" + ConfigurationAttributes.CurrentYear + "',";
                        pSet.Query += "'" + m_sOffice + "', ";
                        pSet.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', ";
                        pSet.Query += "to_date('" + AppSettingsManager.GetCurrentDate().ToShortDateString() + "','MM/dd/yyyy'))";
                        if (pSet.ExecuteNonQuery() == 0)
                        { }
                    }

                    pSet.Query = "delete from trans_for_approve_tmp where bin = '" + txtBIN.Text + "' and office_nm = '" + m_sOffice + "'";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    if (TaskMan.IsObjectLock(txtBIN.Text, m_sOffice, "DELETE", "ASS"))  // RMC 20141228 modified permit printing (lubao)
                    {
                    }

                    TransLog.UpdateLog(txtBIN.Text, m_sBnsStat, ConfigurationAttributes.CurrentYear, m_sOffice + "-APPROVE", m_dTransLogIn, AppSettingsManager.GetSystemDate());

                    MessageBox.Show("Record approved", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (MessageBox.Show("Record not approved in this office.\nContinue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                }
                UpdateList();
            }


            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtMiddleInitial.Text = "";
            txtTBIN.Text = "";
            txtBIN.Text = "";
            txtBnsName.Text = "";
            txtBnsAdd.Text = "";
            txtArea.Text = "";
            txtLessor.Text = "";
            txtViolation.Text = "";
            txtStatus.Text = "";
            txtOwnAdd.Text = "";
            txtTelNo.Text = "";
            cmbBussLine.Items.Clear();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Z:\Projects\_Clients\Santiago City\Customization\file_example_WAV_2MG");
            player.Play();*/

            if (m_sSwitch == "LISTAPPROVE")
            { }
            else
                UpdateList();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();
            pRec.Query = "delete from trans_for_approve_tmp where bin = '" + m_sBIN + "' and tax_year = '" + AppSettingsManager.GetConfigValue("12") + "' and office_nm = '" + m_sOffice + "'";
            pRec.ExecuteNonQuery();

            UpdateList();
        }

        private void frmSearchTmp_FormClosed(object sender, FormClosedEventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "delete from trans_for_approve_tmp where bin = '" + txtBIN.Text + "' and office_nm = '" + m_sOffice + "'";
            pSet.ExecuteNonQuery();

            if (TaskMan.IsObjectLock(txtBIN.Text, m_sOffice, "DELETE", "ASS"))  // RMC 20141228 modified permit printing (lubao)
            {
            }

            if (!string.IsNullOrEmpty(txtBIN.Text.ToString()))
                TransLog.UpdateLog(txtBIN.Text, m_sBnsStat, ConfigurationAttributes.CurrentYear, m_sOffice + "-CLEAR", m_dTransLogIn, AppSettingsManager.GetSystemDate());

            timer1.Enabled = false;
        }

        private void btnTrail_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            if (string.IsNullOrEmpty(txtBIN.Text))
            {
                MessageBox.Show("Select BIN first", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            pSet.Query = "select * from trans_approve where bin = '" + txtBIN.Text + "' and tax_year = '" + txtTaxYear.Text + "'";
            if (pSet.Execute())
            {
                if (!pSet.Read())
                {
                    pSet.Close();

                    MessageBox.Show("BIN not yet approved in any Offices", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            pSet.Close();

            frmPrinting listform = new frmPrinting();
            listform.ReportType = "ApprovalTrail";
            listform.BIN = txtBIN.Text;
            listform.TaxYear = txtTaxYear.Text;
            listform.BnsName = txtBnsName.Text;
            listform.BnsAdd = txtBnsAdd.Text;
            listform.ShowDialog();

        }

        private bool ValidateRecord(string sBin, bool bMarket)
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from trans_approve where tax_year = '" + AppSettingsManager.GetConfigValue("12") + "' and office_nm = 'ENGINEERING' and bin = '" + sBin + "'";
            if (pSet.Execute())
            {
                if (!pSet.Read())
                {
                    pSet.Close();
                    return false;
                }
            }
            pSet.Close();

            pSet.Query = "select * from trans_approve where tax_year = '" + AppSettingsManager.GetConfigValue("12") + "' and office_nm = 'PLANNING' and bin = '" + sBin + "'";
            if (pSet.Execute())
            {
                if (!pSet.Read())
                {
                    pSet.Close();
                    return false;
                }
            }
            pSet.Close();

            pSet.Query = "select * from trans_approve where tax_year = '" + AppSettingsManager.GetConfigValue("12") + "' and office_nm = 'HEALTH' and bin = '" + sBin + "'";
            if (pSet.Execute())
            {
                if (!pSet.Read())
                {
                    pSet.Close();
                    if (AppSettingsManager.GetConfigObject("87") == "Y")    // RMC 20210217 added override of CHO approval. Requested by sir Henry due to lockdown in CHO
                    { }
                    else
                        return false;
                }
            }
            pSet.Close();

            pSet.Query = "select * from trans_approve where tax_year = '" + AppSettingsManager.GetConfigValue("12") + "' and office_nm = 'CENRO' and bin = '" + sBin + "'";
            if (pSet.Execute())
            {
                if (!pSet.Read())
                {
                    pSet.Close();
                    return false;
                }
            }
            pSet.Close();

            if (bMarket)
            {
                pSet.Query = "select * from trans_approve where tax_year = '" + AppSettingsManager.GetConfigValue("12") + "' and office_nm = 'MARKET' and bin = '" + sBin + "'";
                if (pSet.Execute())
                {
                    if (!pSet.Read())
                    {
                        pSet.Close();
                        return false;
                    }
                }
                pSet.Close();
            }

            return true;
        }

        private void btnRebill_Click(object sender, EventArgs e)
        {
            using (frmBilling BillingForm = new frmBilling())
            {
                BillingForm.SourceClass = "Billing";
                BillingForm.Text = "Billing";
                BillingForm.ShowDialog();
                BillingForm.Dispose();
            }
        }

        private bool ValidateApproval(string sBin, string sOffice)
        {
            OracleResultSet pSet = new OracleResultSet();
            bool bApproved = false;

            pSet.Query = "select * from trans_approve where bin = '" + sBin + "' and office_nm= '" + sOffice + "' and tax_year = '" + ConfigurationAttributes.CurrentYear + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    bApproved = true;
                }
            }
            pSet.Close();

            return bApproved;
        }

        private void btnNegList_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABNL"))
            {
                if (string.IsNullOrEmpty(txtBIN.Text))
                {
                    MessageBox.Show("Select record first", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                frmNigList frmniglist = new frmNigList();
                frmniglist.m_sBin = txtBIN.Text;
                frmniglist.m_sSelectedDiv = m_sOffice;
                frmniglist.ShowDialog();
            }

        }

        private void LoadOtherInfo()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sOwnCode = string.Empty;
            txtTelNo.Text = "";
            txtOwnAdd.Text = "";
            pSet.Query = "select own_code from business_que where bin = '" + txtBIN.Text + "' and tax_year = '" + txtTaxYear.Text + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                    sOwnCode = pSet.GetString(0);
            }
            pSet.Close();

            pSet.Query = "select * from own_profile where own_code = '" + sOwnCode + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    txtTelNo.Text = pSet.GetString("tel_no");
                    txtOwnAdd.Text = AppSettingsManager.GetBnsOwnAdd(sOwnCode);
                }
            }
            pSet.Close();

            if (string.IsNullOrEmpty(txtTelNo.Text) || txtTelNo.Text == ".")
            {
                pSet.Query = "select * from emp_names where (bin = '" + txtBIN.Text + "' or temp_bin = '" + txtBIN.Text + "') and emp_occupation = 'OWNER'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        txtTelNo.Text = pSet.GetString("emp_tin");  //used this field for tel no.
                    }
                }
                pSet.Close();
            }

            LoadLineOfBusiness();
        }

        private void LoadLineOfBusiness()
        {
            OracleResultSet pSet = new OracleResultSet();
            cmbBussLine.Items.Clear();

            pSet.Query = "select bns_code from business_que where bin = '" + txtBIN.Text + "' and tax_year = '" + txtTaxYear.Text + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    cmbBussLine.Items.Add(AppSettingsManager.GetBnsDesc(pSet.GetString(0)));
                }
            }
            pSet.Close();

            pSet.Query = "select bns_code_main from addl_bns_que where bin = '" + txtBIN.Text + "' and tax_year = '" + txtTaxYear.Text + "' and bns_stat <> 'RET'";
            pSet.Query += " union ";
            pSet.Query += "select bns_code_main from addl_bns where bin = '" + txtBIN.Text + "' and tax_year = '" + txtTaxYear.Text + "' and bns_stat <> 'RET'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    cmbBussLine.Items.Add(AppSettingsManager.GetBnsDesc(pSet.GetString(0)));
                }
            }
            pSet.Close();

            cmbBussLine.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text == "Clear BIN")
            {

                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtMiddleInitial.Text = "";
                txtTBIN.Text = "";
                txtBIN.Text = "";
                txtBnsName.Text = "";
                txtBnsAdd.Text = "";
                m_sBnsStat = "";
                txtArea.Text = "";
                txtLessor.Text = "";
                txtViolation.Text = "";
                txtStatus.Text = "";
                txtOwnAdd.Text = "";
                txtTelNo.Text = "";
                cmbBussLine.Items.Clear();
                btnSearch.Text = "Search";
            }
            else
            {
                if (string.IsNullOrEmpty(txtBIN.Text))
                {
                    MessageBox.Show("Indicate BIN to search", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                UpdateListApp(txtBIN.Text);

                if (dgvList.Rows.Count == 0)
                {
                    MessageBox.Show("No record found", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else
                {
                    btnSearch.Text = "Clear BIN";
                }
            }
        }

        private void btnPrintList_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Clear();
            dataTable.Columns.Add("Seq", typeof(String));
            dataTable.Columns.Add("Violation", typeof(String));
            dataTable.Columns.Add("BIN", typeof(String));
            dataTable.Columns.Add("Business Name", typeof(String));
            dataTable.Columns.Add("Tax Year", typeof(String));
            dataTable.Columns.Add("Last Name", typeof(String));
            dataTable.Columns.Add("First Name", typeof(String));
            dataTable.Columns.Add("M.I.", typeof(String));
            dataTable.Columns.Add("Business Address", typeof(String));
            dataTable.Columns.Add("Bns Stat", typeof(String));
            dataTable.Columns.Add("Area", typeof(String));
            dataTable.Columns.Add("Lessor", typeof(String));

            for (int intRow = 0; intRow < dgvList.Rows.Count; intRow++)
            {
                dataTable.Rows.Add(dgvList[0, intRow].Value.ToString(),
                    dgvList[1, intRow].Value.ToString(),
                    dgvList[2, intRow].Value.ToString(),
                    dgvList[3, intRow].Value.ToString(),
                    dgvList[4, intRow].Value.ToString(),
                    dgvList[5, intRow].Value.ToString(),
                    dgvList[6, intRow].Value.ToString(),
                    dgvList[7, intRow].Value.ToString(),
                    dgvList[8, intRow].Value.ToString(),
                    dgvList[9, intRow].Value.ToString(),
                    dgvList[10, intRow].Value.ToString(),
                    dgvList[11, intRow].Value.ToString());
            }

            frmPrinting listform = new frmPrinting();
            listform.ReportType = "PrintList";
            listform.Office = m_sOffice;
            listform.dt = dataTable;
            listform.ShowDialog();
        }
    }
}





//// RMC 20141228 modified permit printing (lubao) - modified whole class


//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Text;
//using System.Windows.Forms;
//using Amellar.Common.DataConnector;
//using Amellar.Common.StringUtilities;

//namespace Amellar.Modules.HealthPermit
//{
//    public partial class frmSearchTmp : Form
//    {
//        private string m_sBIN = string.Empty;
//        private string m_sPermit = string.Empty;

//        public string TaxYear
//        {
//            get { return txtTaxYear.Text; }
//            set { txtTaxYear.Text = value; }
//        }

//        public string BIN
//        {
//            get { return m_sBIN; }
//            set { m_sBIN = value; }
//        }

//        public string LastName
//        {
//            get { return txtLastName.Text; }
//            set { txtLastName.Text = value; }
//        }

//        public string FirstName
//        {
//            get { return txtFirstName.Text; }
//            set { txtFirstName.Text = value; }
//        }

//        public string MI
//        {
//            get { return txtMiddleInitial.Text; }
//            set { txtMiddleInitial.Text = value; }
//        }

//        public string BnsName
//        {
//            get { return txtBnsName.Text; }
//            set { txtBnsName.Text = value; }
//        }

//        public string BnsAdd
//        {
//            get { return txtBnsAdd.Text; }
//            set { txtBnsAdd.Text = value; }
//        }

//        public string Permit
//        {
//            get { return m_sPermit; }
//            set { m_sPermit = value; }
//        }

//        public frmSearchTmp()
//        {
//            InitializeComponent();
//        }

//        private void frmSearchTmp_Load(object sender, EventArgs e)
//        {
//            dgvList.Columns.Clear();

//            dgvList.Columns.Add("1", "Temp BIN");
//            dgvList.Columns.Add("2", "BIN");
//            dgvList.Columns.Add("3", "Business Name");
//            dgvList.Columns.Add("4", "Tax Year");
//            dgvList.Columns.Add("5", "Last Name");
//            dgvList.Columns.Add("6", "First Name");
//            dgvList.Columns.Add("7", "M.I.");
//            dgvList.Columns.Add("8", "Business Address");
//            dgvList.Columns.Add("9", "Occupation");
//            dgvList.Columns[0].Width = 80;
//            dgvList.Columns[1].Width = 100;
//            dgvList.Columns[2].Width = 100;
//            dgvList.Columns[3].Width = 80;
//            dgvList.Columns[4].Width = 100;
//            dgvList.Columns[5].Width = 100;
//            dgvList.Columns[6].Width = 20;
//            dgvList.Columns[7].Width = 100;
//            dgvList.Columns[8].Width = 100;

//            OracleResultSet pSet = new OracleResultSet();

//            // RMC 20150117 (s)

            
//            string sLN = ""; string sFN = ""; string sMI = ""; string sBIN = "";

//            if (m_sPermit == "Annual Inspection")
//            {
//                pSet.Query = "select distinct temp_bin, bin, bns_nm, tax_year, bns_add,emp_occupation from emp_names where ";
//                pSet.Query += "tax_year = '" + txtTaxYear.Text + "' ";
//                pSet.Query += "and temp_bin like '" + txtTaxYear.Text + "%' ";
//                pSet.Query += "and bin not in (select bin from annual_insp where tax_year = '" + txtTaxYear.Text + "') ";
//                pSet.Query += "union all ";
//                pSet.Query += "select distinct temp_bin, bin, bns_nm, tax_year, bns_add,emp_occupation from emp_names where ";
//                pSet.Query += "tax_year = '" + txtTaxYear.Text + "' ";
//                pSet.Query += "and temp_bin like '" + txtTaxYear.Text + "%' ";
//                pSet.Query += "and trim(bin) is null and temp_bin not in (select bin from annual_insp where tax_year = '" + txtTaxYear.Text + "') ";
//            }
//            else
//            {
//                pSet.Query = "select distinct temp_bin, bin, bns_nm, tax_year, bns_add, emp_occupation from emp_names where ";
//                pSet.Query += "tax_year = '" + txtTaxYear.Text + "' ";
//                pSet.Query += "and temp_bin like '" + txtTaxYear.Text + "%' ";
//                if (m_sPermit == "Application" || m_sPermit == "Zoning")
//                    pSet.Query += "and (trim(bin) is null or temp_bin = bin)"; //MCR 20150121
//                    //pSet.Query += "and trim(bin) is null ";
//            }
//            pSet.Query += "order by bns_nm";
//            if (pSet.Execute())
//            {
//                while (pSet.Read())
//                {
//                    sBIN = pSet.GetString(0); 

//                    GetOwnName(sBIN, out sLN, out sFN, out sMI);

//                    dgvList.Rows.Add(sBIN, pSet.GetString(1), pSet.GetString(2),
//                        pSet.GetString(3), sLN, sFN, sMI, pSet.GetString(4), pSet.GetString(5));
//                }
//            }
//            pSet.Close();
            
            
//            // RMC 20150117 (e)

//                //pSet.Query = "select distinct (TEMP_BIN), BIN, bns_nm, TAX_YEAR, EMP_LN, EMP_FN, EMP_MI, bns_add from emp_names where tax_year = '" + txtTaxYear.Text + "' and emp_occupation = 'OWNER' order by temp_bin";   // RMC 20141228 modified permit printing (lubao)
//                /*pSet.Query = "select distinct (TEMP_BIN), BIN, bns_nm, TAX_YEAR, EMP_LN, EMP_FN, EMP_MI, bns_add from emp_names where tax_year = '" + txtTaxYear.Text + "' order by temp_bin";
                
//                if (pSet.Execute())
//                {
//                    while (pSet.Read())
//                    {
//                        // RMC 20150102 mods in permit (s)
//                        bool bInclude = false;
//                        if (pSet.GetString(1).Trim() == "")
//                        {
//                            if (m_sPermit == "Application")
//                            {
//                                bInclude = true;
//                            }
//                            else
//                                bInclude = ValidateBIN(pSet.GetString(0));
//                        }
//                        else if (pSet.GetString(1).Length < 12)
//                        {
//                            if (m_sPermit == "Application")
//                                bInclude = true;
//                        }
//                        else
//                            bInclude = ValidateBIN(pSet.GetString(1));

//                        if (bInclude)   // RMC 20150102 mods in permit (e)
//                        {
//                            dgvList.Rows.Add(pSet.GetString(0), pSet.GetString(1), pSet.GetString(2),
//                            pSet.GetString(3), pSet.GetString(4), pSet.GetString(5), pSet.GetString(6), pSet.GetString(7));
//                        }
//                    }
//                }
//                pSet.Close();*/
            
//        }

//        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
//        {
            
//        }

//        private void btnClose_Click(object sender, EventArgs e)
//        {
//            // RMC 20141228 modified permit printing (lubao) (s)
//            if (txtBIN.Text.Trim() != "")
//                m_sBIN = txtBIN.Text;
//            else
//                m_sBIN = txtTBIN.Text;
//            // RMC 20141228 modified permit printing (lubao) (e)

//            this.Close();
//        }

//        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
//        {
//            try
//            {
//                txtTBIN.Text = dgvList[0, e.RowIndex].Value.ToString();
//                txtBIN.Text = dgvList[1, e.RowIndex].Value.ToString();
//                txtBnsName.Text = dgvList[2, e.RowIndex].Value.ToString();
//                txtTaxYear.Text = dgvList[3, e.RowIndex].Value.ToString();
//                txtLastName.Text = dgvList[4, e.RowIndex].Value.ToString();
//                txtFirstName.Text = dgvList[5, e.RowIndex].Value.ToString();
//                txtMiddleInitial.Text = dgvList[6, e.RowIndex].Value.ToString();
//                txtBnsAdd.Text = dgvList[7, e.RowIndex].Value.ToString();
//            }
//            catch
//            {
//                txtTBIN.Text = "";
//                txtBIN.Text = "";
//                txtBnsName.Text = "";
//                txtTaxYear.Text = "";
//                txtLastName.Text = "";
//                txtFirstName.Text = "";
//                txtMiddleInitial.Text = "";
//                txtBnsAdd.Text = "";
//            }
//        }

//        private void btnClear_Click(object sender, EventArgs e)
//        {
//            txtFirstName.Text = "";
//            txtLastName.Text = "";
//            txtMiddleInitial.Text = "";
//            txtTBIN.Text = "";
//            txtBIN.Text = "";
//            txtBnsName.Text = "";
//            txtBnsAdd.Text = "";
//        }

//        private bool ValidateBIN(string sBIN)
//        {
//            // RMC 20150102 mods in permit
//            OracleResultSet pSet = new OracleResultSet();
//            int iCnt = 0;

//            pSet.Query = "select count(*) from business_que where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
//            pSet.Query += " and bns_stat = 'NEW'";
//            int.TryParse(pSet.ExecuteScalar(), out iCnt);
//            if (iCnt > 0)
//            {
//                if (m_sPermit == "Annual Inspection" || m_sPermit == "Zoning")
//                {
//                    pSet.Close();
//                    if(m_sPermit == "Annual Inspection")
//                        pSet.Query = "select count(*) from annual_insp where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
//                    else
//                        pSet.Query = "select count(*) from zoning where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
//                    int.TryParse(pSet.ExecuteScalar(), out iCnt);
//                    if (iCnt == 0)
//                    {
//                        return true;
                        
//                    }
//                    else
//                        return false;
//                }

//                return true;
//            }
//            else
//            {
//                pSet.Close();
//                pSet.Query = "select count(*) from businesses where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
//                pSet.Query += " and bns_stat = 'NEW'";
//                int.TryParse(pSet.ExecuteScalar(), out iCnt);
//                if (iCnt > 0)
//                {
//                    if (m_sPermit == "Annual Inspection" || m_sPermit == "Zoning")
//                    {
//                        if (m_sPermit == "Annual Inspection")
//                            pSet.Query = "select count(*) from annual_insp where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
//                        else
//                            pSet.Query = "select count(*) from zoning where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
//                        int.TryParse(pSet.ExecuteScalar(), out iCnt);
//                        if (iCnt == 0)
//                        {
//                            return true;
//                        }
//                        else
//                            return false;
//                    }

//                    return true;
//                }
//                else
//                {
//                    pSet.Close();

//                    if (m_sPermit == "Annual Inspection" || m_sPermit == "Zoning")
//                    {
//                        if (m_sPermit == "Annual Inspection")
//                            pSet.Query = "select count(*) from annual_insp where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
//                        else
//                            pSet.Query = "select count(*) from zoning where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
//                        int.TryParse(pSet.ExecuteScalar(), out iCnt);
//                        if (iCnt == 0)
//                        {
//                            if (StringUtilities.Left(sBIN, 4) == txtTaxYear.Text)
//                                return true;
//                            else
//                                return false;
//                        }
//                    }
//                    else
//                    {
//                        //if (m_sPermit == "Health" || m_sPermit == "Sanitary") //MCR 20150116
//                        if (m_sPermit == "Health" || m_sPermit == "Sanitary" || m_sPermit == "Application")
//                            return true;
//                        else
//                            return false;
//                    }
//                }
//            }

//            return false;
//        }

//        private void GetOwnName(string p_sBIN, out string o_sLN, out string o_sFN, out string o_sMI)
//        {
//            // RMC 20150117
//            OracleResultSet pName = new OracleResultSet();
//            o_sLN = "";
//            o_sFN = "";
//            o_sMI = "";

//            pName.Query = "select * from emp_names where (bin = '" + p_sBIN + "' or temp_bin = '" + p_sBIN + "') and emp_occupation = 'OWNER'";
//            if (pName.Execute())
//            {
//                if (pName.Read())
//                {
//                    o_sLN = pName.GetString("emp_ln");
//                    o_sFN = pName.GetString("emp_fn");
//                    o_sMI = pName.GetString("emp_mi");
//                }
//            }
//            pName.Close();
//        }
//    }
//}