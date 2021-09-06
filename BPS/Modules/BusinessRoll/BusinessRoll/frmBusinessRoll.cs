using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.DataConnector;
using Amellar.Common.EncryptUtilities;
using Amellar.Common.StringUtilities;
using ComponentFactory.Krypton.Toolkit;

namespace BusinessRoll
{
    public partial class frmBusinessRoll : Form
    {
        public frmBusinessRoll()
        {
            InitializeComponent();
        }

        List<String> m_strBnsCode = new List<String>();
        List<String> m_strBnsDesc = new List<String>();
        List<String> m_strStreet = new List<String>();
        List<String> m_strBnsNatureCode = new List<String>();
        List<String> m_strBnsNature = new List<String>();

        AutoCompleteStringCollection m_AutoStreet = new AutoCompleteStringCollection();
        String m_strReportType = "";
        String m_strUser = "";

        public List<String> BusinessCode
        {
            get { return m_strBnsCode; }
        }

        public List<String> Street
        {
            get { return m_strStreet; }
        }

        public List<String> NatureCode
        {
            get { return m_strBnsNatureCode; }
        }

        bool m_blnBnsQueue = false;// AST 20140122
        public bool BusinessQueue
        {
            set { m_blnBnsQueue = value; }
        }

        bool m_blnUnrenewed = false;
        public bool Unrenewed
        {
            set { m_blnUnrenewed = value; }
        }

        private void frmBusinessRoll_Load(object sender, EventArgs e)
        {
            try { m_strUser = AppSettingsManager.SystemUser.UserCode; }
            catch { m_strUser = "SYS_PROG"; }

            m_strReportType = "BUSINESS ROLL BY BARANGAY";
            List<String> strBarangay = new List<String>(DataAccess.BarangayList());
            for (int i = 0; i != strBarangay.Count; i++)
            {
                cboBarangay.Items.Add(strBarangay[i]);
            }
            cboBarangay.SelectedIndex = 0;

            List<String> strDistrict = new List<String>(DataAccess.DistrictList());
            for (int i = 0; i != strDistrict.Count; i++)
            {
                cboDistrict.Items.Add(strDistrict[i]);
            }
            cboDistrict.SelectedIndex = 0;

            DataAccess.BusinessTypeList(out m_strBnsCode, out m_strBnsDesc);
            for (int i = 0; i != m_strBnsDesc.Count; i++)
            {
                cboBussType.Items.Add(m_strBnsDesc[i]);
            }
            cboBussType.SelectedIndex = 0;

            cboStatus.Items.Add("ALL");
            cboStatus.Items.Add("NEW");
            cboStatus.Items.Add("REN");
            cboStatus.Items.Add("RET");
            cboStatus.SelectedIndex = 0;
            
            OracleResultSet result = new OracleResultSet();
            result.Query = "select distinct businesses.bns_street from businesses,own_names where businesses.own_code = own_names.own_code";
            if (result.Execute())
            {
                cboStreet.Items.Add("ALL");
                while (result.Read())
                {
                    cboStreet.Items.Add(result.GetString("bns_street"));
                }
            }
            result.Close();

            if (m_blnBnsQueue)
            {
                rbBarangay.Checked = false; // AST 20140122
                grpGroupBy.Enabled = false;
                m_strReportType = "Business on Queue";
                this.Text = m_strReportType;    // RMC 20170123 correction in module header for Business Reports
            }

            if (m_blnUnrenewed)
            {
                this.Text = "Unrenewed";
                m_strReportType = "Unrenewed";
                lblStatus.Visible = false;
                cboStatus.Visible = false;
                txtTaxYear.Visible = false;
                lblTaxYear.Visible = false;
                this.Size = new System.Drawing.Size(441, 155); //441, 155
            }

        }

        private void cboBussType_SelectedValueChanged(object sender, EventArgs e)
        {
            DataAccess.BusinessNatureList(m_strBnsCode[cboBussType.SelectedIndex], out m_strBnsNatureCode, out m_strBnsNature);
            cboBussNature.Items.Clear();
            for (int j = 0; j != m_strBnsNature.Count; j++)
            {
                cboBussNature.Items.Add(m_strBnsNature[j]);
            }
            cboBussNature.SelectedIndex = 0;
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is KryptonTextBox || ctrl is KryptonComboBox || ctrl is ComboBox)
                    ctrl.Enabled = false;
            }

            cboStreet.Enabled = false;
            chkByPercentage.Visible = false;
            lblPercent.Visible = false;

            if (sender.Equals(rbBarangay))
            {
                cboBarangay.Enabled = true;
                cboStatus.Enabled = true;
                txtTaxYear.Enabled = true;
                m_strReportType = "BUSINESS ROLL BY BARANGAY";
            }
            else if (sender.Equals(rbDistrict))
            {
                m_strReportType = "BUSINESS ROLL BY DISTRICT";                
            }
            else if (sender.Equals(rbLineOfBuss))
            {
                cboBarangay.Enabled = true;
                cboBussType.Enabled = true;
                cboBussNature.Enabled = true;
                cboStatus.Enabled = true;
                txtTaxYear.Enabled = true;
                m_strReportType = "BUSINESS ROLL BY LINE OF BUSINESS";
            }
            else if (sender.Equals(rbStreet))
            {
                cboBussType.Enabled = true;
                cboBussNature.Enabled = true;
                cboStreet.Enabled = true;
                m_strReportType = "BUSINESS ROLL BY STREET";
            }
            else if (sender.Equals(rbTopGrosses) || sender.Equals(rbTopPayers) || sender.Equals(rbTopAssessed)) //JARS 20170719 rbTopAssessed
            {
                cboBarangay.Enabled = true;
                cboBussType.Enabled = true;
                cboBussNature.Enabled = true;
                txtGross.Enabled = true;
                txtTaxYear.Enabled = true;
                txtTaxYear.Text = AppSettingsManager.GetCurrentDate().Year.ToString();
                chkByPercentage.Visible = true;
                if (sender.Equals(rbTopGrosses))
                    m_strReportType = "BUSINESS ROLL BY TOP GROSSES";
                else if(sender.Equals(rbTopPayers))
                    m_strReportType = "BUSINESS ROLL BY TOP PAYERS";
                else
                    m_strReportType = "BUSINESS ROLL BY TOP ASSESSED TAX"; //JARS 20170719
            }
            else if (sender.Equals(rbLeastGrosses) || sender.Equals(rbLeastPayers))
            {
                cboBarangay.Enabled = true;
                cboBussType.Enabled = true;
                cboBussNature.Enabled = true;
                txtGross.Enabled = true;
                txtTaxYear.Enabled = true;
                txtTaxYear.Text = "2014";
                chkByPercentage.Visible = true;
                if (sender.Equals(rbLeastGrosses))
                    m_strReportType = "BUSINESS ROLL BY LEAST GROSSES";
                else
                    m_strReportType = "BUSINESS ROLL BY LEAST PAYERS";
            }
            else if (sender.Equals(rbName))
            {
                cboDistrict.Enabled = true;
                cboStatus.Enabled = true;
                txtTaxYear.Enabled = true;
                m_strReportType = "BUSINESS ROLL BY NAME";
            }

        }

        private void chkByPercentage_CheckedChanged(object sender, EventArgs e)
        {
            if (chkByPercentage.Checked == true)
                lblPercent.Visible = true;
            else
                lblPercent.Visible = false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            
            int iTaxYear = 0;

            if (rbStreet.Checked)
            {
                if (cboStreet.Text.Trim() == "")
                {
                    MessageBox.Show("Please Select Street Name", "Street", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            if(!m_blnUnrenewed)
                if (txtTaxYear.Text.Trim() != "")
                {
                    int.TryParse(txtTaxYear.Text, out iTaxYear);

                    if (iTaxYear <= 0)
                    {
                        MessageBox.Show("Please type correct tax year", "Tax Year", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }

            if (rbTopGrosses.Checked)
            {
                int iGrossTmp = int.Parse(txtGross.Text);
                if (iGrossTmp <= 0)
                {
                    MessageBox.Show("Please Fill-Up Top Grosses To Generate", "Top Grosses", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            if (rbTopPayers.Checked)
            {
                int iGrossTmp = int.Parse(txtGross.Text);
                if (iGrossTmp <= 0)
                {
                    MessageBox.Show("Please Fill-Up Top Payers To Generate", "Top Payers", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
             
            if(rbTopAssessed.Checked)
            {
                int iGrossTmp = int.Parse(txtGross.Text);
                if (iGrossTmp <= 0)
                {
                    MessageBox.Show("Please Fill-Up Top Payers To Generate", "Top Assessed Tax", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            try
            {
                if (!AppSettingsManager.GenerateInfo(m_strReportType))
                    return;
            }
            catch { }

            CleanGenerateInfo();

            AddGenerateInfo();
            frmReport frmReport = null;
            Cursor.Current = Cursors.WaitCursor;

            if (m_blnUnrenewed)
                frmReport = new frmReport(frmReport.ReportType.Unrenewed, this);
            else
            {
                if (rbBarangay.Checked)
                    frmReport = new frmReport(frmReport.ReportType.Barangay, this);
                else if (rbDistrict.Checked)
                    frmReport = new frmReport(frmReport.ReportType.District, this);
                else if (rbLineOfBuss.Checked)
                    frmReport = new frmReport(frmReport.ReportType.LineOfBusiness, this);
                else if (rbStreet.Checked)
                    frmReport = new frmReport(frmReport.ReportType.Street, this);
                else if (rbTopGrosses.Checked)
                    frmReport = new frmReport(frmReport.ReportType.TopGrosses, this);
                else if (rbTopPayers.Checked)
                    frmReport = new frmReport(frmReport.ReportType.TopPayers, this);
                else if (rbLeastGrosses.Checked)
                    frmReport = new frmReport(frmReport.ReportType.LeastGrosses, this);
                else if (rbLeastPayers.Checked)
                    frmReport = new frmReport(frmReport.ReportType.LeastPayers, this);
                else if (rbName.Checked)
                    frmReport = new frmReport(frmReport.ReportType.Name, this);
                else if (rbTopAssessed.Checked)
                    frmReport = new frmReport(frmReport.ReportType.TopAssessed, this);
                else
                    frmReport = new frmReport(frmReport.ReportType.BusinessOnQueue, this); // AST 20140122
            }

            frmReport.ShowDialog();
            Cursor.Current = Cursors.Default;
            CleanGenerateInfo();
        }

        private void CleanGenerateInfo()
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "delete from rep_bns_roll where report_name = '" + m_strReportType + "' and user_code = '" + m_strUser + "'";
            result.ExecuteNonQuery();

            result.Query = "delete from gen_info where report_name = '" + m_strReportType + "'";
            result.ExecuteNonQuery();
        }

        private void AddGenerateInfo()
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "insert into gen_info values('" + m_strReportType + "', to_date('" + AppSettingsManager.GetCurrentDate().ToShortDateString() + "', 'MM/dd/yyyy'), '" + m_strUser + "', 1, 'ASS')";
            result.ExecuteNonQuery();
        }

        public void UpdateGenerateInfo()
        {
            OracleResultSet resultUG = new OracleResultSet();
            resultUG.Query = "update gen_info set switch = 0 where report_name = '" + m_strReportType + "' and user_code = '" + m_strUser + "'";
            try
            {
                resultUG.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                resultUG.Rollback();
                MessageBox.Show(ex.Message + "\nError to Update Generate Info", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public static void UpdateBnsRollReport(String Heading, String DataGroup, String SubGroup, String Bin, String BnsName, String BnsAddr,
            String OwnerName, String OwnerAddr, String MainBns, String BnsStatus, String Capital, String Gross, String DateOperated,
            String PermitNo, String PermitDate, String TaxPaid, String QtrPaid, String ReportName, String UserCode, String CurrUser,
            String TellNo, int EmpCount)
        {
            //MCR 20140710 (s)
            if (SubGroup == "%%")
                SubGroup = "";

            if (BnsStatus == "%%")
                BnsStatus = "";
            //MCR 20140710 (e)

            OracleResultSet results = new OracleResultSet();
            results.Query = "insert into rep_bns_roll values('";
            results.Query += StringUtilities.HandleApostrophe(Heading) + "','";
            results.Query += StringUtilities.HandleApostrophe(DataGroup) + "','";
            results.Query += StringUtilities.HandleApostrophe(StringUtilities.Left(SubGroup,30)) + "','";
            results.Query += StringUtilities.HandleApostrophe(Bin) + "','";
            results.Query += StringUtilities.HandleApostrophe(BnsName) + "','";
            results.Query += StringUtilities.HandleApostrophe(BnsAddr) + "','";
            results.Query += StringUtilities.HandleApostrophe(StringUtilities.Left(OwnerName.Trim(), 60)) + "','"; //MCR 03062014
            results.Query += StringUtilities.HandleApostrophe(OwnerAddr) + "','";
            results.Query += StringUtilities.HandleApostrophe(MainBns) + "','";
            results.Query += StringUtilities.HandleApostrophe(BnsStatus) + "','";
            results.Query += Convert.ToDouble(Capital) + "','";
            results.Query += Convert.ToDouble(Gross) + "','";
            results.Query += StringUtilities.HandleApostrophe(DateOperated) + "','";
            results.Query += PermitNo + "','";
            results.Query += StringUtilities.HandleApostrophe(PermitDate) + "','";
            results.Query += StringUtilities.HandleApostrophe(TaxPaid) + "','";
            results.Query += QtrPaid + "','";
            results.Query += StringUtilities.HandleApostrophe(ReportName) + "','";
            results.Query += StringUtilities.HandleApostrophe(UserCode) + "','";
            results.Query += StringUtilities.HandleApostrophe(CurrUser) + "','";
            results.Query += StringUtilities.HandleApostrophe(TellNo) + "','";
            results.Query += EmpCount;
            results.Query += "')";
            try
            {
                results.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                results.Rollback();
                results.Close();
                MessageBox.Show(ex.Message + "\nError in Saving " + ReportName, "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            results.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("Are you sure you want to cancel?", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            string myString = cboStreet.Text.ToString().ToUpper();
            cboStreet.SelectionStart = cboStreet.Text.Length;
            cboStreet.Text = myString;
        }

        private void txtGross_KeyPress(object sender, KeyPressEventArgs e)
        {
           if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            e.Handled = true;
        }

    }
}
