
// RMC 20161230 modified abstract teller report for Binan

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.LiquidationReports
{
    public partial class frmAbstractTeller_Binan : Form
    {
        private string m_sReportName = string.Empty;
        private string m_sRevYear = string.Empty;
        public int m_iReportSwitch = 0;
        int iRowFeesIndex = 0;
        int iRowTaxIndex = 0;
        int iCheckFees = 0;
        int iCheckTax = 0;

        public frmAbstractTeller_Binan()
        {
            InitializeComponent();
        }

        private void frmAbstractTeller_Binan_Load(object sender, EventArgs e)
        {
            m_sRevYear = AppSettingsManager.GetConfigObject("07");
            rdoOr.Checked = true;   // RMC 20170123 Added RCD date option in Abstract of Teller
            LoadTeller();
            LoadFees();
            LoadBussLine();
        }

        private void LoadTeller()
        {
            String sQuery = "";
            String sGetString = "";
            
            sQuery = string.Format("select teller, ln from tellers order by teller");
            sGetString = "teller";
            
            cmbTeller.Items.Clear();
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                cmbTeller.Items.Add("");
                while (pSet.Read())
                {
                    cmbTeller.Items.Add(pSet.GetString(sGetString).Trim());
                }
            }

            if (cmbTeller.Items.Count > 0)
                cmbTeller.SelectedIndex = 0;
            pSet.Close();
        }

        private void LoadFees()
        {
            dgvFees.Rows.Clear();
            OracleResultSet result = new OracleResultSet();

            result.Query = "select * from tax_and_fees_table where rev_year = '" + m_sRevYear + "' order by fees_code";
            if (result.Execute())
                while (result.Read())
                    dgvFees.Rows.Add(false, result.GetString("fees_code"), result.GetString("fees_desc"));
            result.Close();
        }

        private void LoadBussLine()
        {
            OracleResultSet result = new OracleResultSet();

            result.Query = "select * from bns_table where fees_code = 'B' and length(bns_code) = 2 order by bns_code";
            if(result.Execute())
            {
                while(result.Read())
                {
                    dgvTax.Rows.Add(false,result.GetString("bns_code"), result.GetString("bns_desc"));
                }
            }
            result.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cmbTeller.Text.ToString().Trim() == "")
            {
                
                MessageBox.Show("Please select teller","Abstract by Teller",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }
            iCheckFees = 0;
            for (int i = 0; i < dgvFees.RowCount; i++)
            {
                if ((bool)dgvFees.Rows[i].Cells[0].Value == true)
                    iCheckFees++;

                if (iCheckFees > 10)
                {
                    MessageBox.Show("Maximum number of individual column for Regulatory Fees reached.\nPlease limit to ten (10) fees only.");
                    dgvFees.Rows[iRowFeesIndex].Cells[0].Value = false;
                    iRowFeesIndex = 0;
                    return;
                }
            }

            iCheckTax = 0;
            for (int i = 0; i < dgvTax.RowCount; i++)
            {
                if ((bool)dgvTax.Rows[i].Cells[0].Value == true)
                    iCheckTax++;

                if (iCheckTax > 9)
                {
                    MessageBox.Show("Maximum number of individual column for Line of Business reached.\nPlease limit to nine (9) line of business only.");
                    dgvTax.Rows[iRowTaxIndex].Cells[0].Value = false;
                    iRowTaxIndex = 0;
                    return;
                }
            }

            m_sReportName = "DAILY REPORT OF BUSINESS TAX AND OTHER FEES COLLECTIONS";

            if (iCheckTax == 0)
            {
                MessageBox.Show("The first nine (9) Line of Business will be displayed in individual column.\nThe rest will be automatically consolidated under OTHER BUSS TAX.", "NO LINE OF BUSINESS SELECTED", MessageBoxButtons.OK, MessageBoxIcon.Information); // RMC 20140909 Migration QA
                PrepareDefaultTax();
            }
            else
                PrepareSelectedTax();

            if (iCheckFees == 0)
            {
                MessageBox.Show("The first ten (10) Regulatory Fees will be displayed in individual column.\nThe rest will be automatically consolidated under OTHER FEES.", "NO REGULATORY FEES SELECTED", MessageBoxButtons.OK, MessageBoxIcon.Information); // RMC 20140909 Migration QA
                PrepareDefaultFees();
            }
            else
                PrepareSelectedFees();

            if (AppSettingsManager.GenerateInfo(m_sReportName))
            {
                OracleResultSet pSet = new OracleResultSet();

                pSet.Query = string.Format("DELETE FROM REPORT_ABSTRACT WHERE REPORT_NAME = '{0}' AND USER_CODE = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet.ExecuteNonQuery();
                pSet.Close();

                pSet.Query = string.Format("DELETE FROM GEN_INFO WHERE REPORT_NAME = '{0}'", m_sReportName);
                pSet.ExecuteNonQuery();
                pSet.Close();

                DateTime m_dtSaveTime = AppSettingsManager.GetSystemDate();
                String sCurrentDate = string.Format("{1}/{2}/{0}",
                        m_dtSaveTime.Year,
                        m_dtSaveTime.Month,
                        m_dtSaveTime.Day);//  {3}:{4}:{5}, m_dtSaveTime.Hour, m_dtSaveTime.Minute, m_dtSaveTime.Second);

                pSet.Query = string.Format("INSERT INTO GEN_INFO VALUES('{0}',sysdate,'{1}',{2},'COL')", m_sReportName, AppSettingsManager.SystemUser.UserCode, 1);
                pSet.ExecuteNonQuery();
                pSet.Close();

                frmLiqReports dlg = new frmLiqReports();
                dlg.iReportSwitch = 2;
                dlg.ReportSwitch = "AbstractCollectByTellerBinan";

                dlg.ReportTitle = m_sReportName;
                dlg.DateFrom = dtpFrom.Value;
                dlg.DateTo = dtpTo.Value;
                dlg.Teller = cmbTeller.Text.Trim();
                dlg.iReportSwitch = m_iReportSwitch;
                // RMC 20170123 Added RCD date option in Abstract of Teller (s)
                if (rdoOr.Checked == true)
                    dlg.BasedonORDate = true;
                else
                    dlg.BasedonORDate = false;
                // RMC 20170123 Added RCD date option in Abstract of Teller (e)
                dlg.ShowDialog();

                
            }
        }

        private void PrepareDefaultFees()
        {
            OracleResultSet result = new OracleResultSet();

            result.Query = "delete from BTAX_REP_ABSTRACT where user_code = :1 and report_name = :2 and fees_code not like 'B%'";
            result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode.Trim());
            result.AddParameter(":2", m_sReportName);
            if (result.ExecuteNonQuery() != 0)
            { }
            result.Close();

            for (int i = 0; i < 10; i++) 
            {
                result.Query = "insert into BTAX_REP_ABSTRACT values (:1, :2, :3)";
                result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode.Trim());
                result.AddParameter(":2", dgvFees.Rows[i].Cells[1].Value.ToString());
                result.AddParameter(":3", m_sReportName);
                if (result.ExecuteNonQuery() != 0)
                { }
                result.Close();
            }
        }

        private void PrepareSelectedFees()
        {
            OracleResultSet result = new OracleResultSet();
            int iCnt = 0;
            result.Query = "delete from BTAX_REP_ABSTRACT where user_code = :1 and report_name = :2 and fees_code not like 'B%'";
            result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode.Trim());
            result.AddParameter(":2", m_sReportName);
            if (result.ExecuteNonQuery() != 0)
            { }
            result.Close();

            for (int i = 0; i < dgvFees.RowCount; i++)
            {
                if ((bool)dgvFees.Rows[i].Cells[0].Value == true && iCnt < 10)
                {
                    iCnt++;
                    result.Query = "insert into BTAX_REP_ABSTRACT values (:1, :2, :3)";
                    result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode.Trim());
                    result.AddParameter(":2", dgvFees.Rows[i].Cells[1].Value.ToString());
                    result.AddParameter(":3", m_sReportName);
                    if (result.ExecuteNonQuery() != 0)
                    { }
                    result.Close();
                }
            }

            if (iCnt < 10)
            {
                for (int i = 0; i < dgvFees.RowCount; i++)
                {
                    if ((bool)dgvFees.Rows[i].Cells[0].Value == false && iCnt < 10)
                    {
                        iCnt++;
                        result.Query = "insert into BTAX_REP_ABSTRACT values (:1, :2, :3)";
                        result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode.Trim());
                        result.AddParameter(":2", dgvFees.Rows[i].Cells[1].Value.ToString());
                        result.AddParameter(":3", m_sReportName);
                        if (result.ExecuteNonQuery() != 0)
                        { }
                        result.Close();
                    }
                }
            }

        }

        private void PrepareDefaultTax()
        {
            OracleResultSet result = new OracleResultSet();

            result.Query = "delete from BTAX_REP_ABSTRACT where user_code = :1 and report_name = :2 and fees_code like 'B%'";
            result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode.Trim());
            result.AddParameter(":2", m_sReportName);
            if (result.ExecuteNonQuery() != 0)
            { }
            result.Close();

            for (int i = 0; i < 9; i++)
            {
                result.Query = "insert into BTAX_REP_ABSTRACT values (:1, :2, :3)";
                result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode.Trim());
                result.AddParameter(":2", "B"+dgvTax.Rows[i].Cells[1].Value.ToString());
                result.AddParameter(":3", m_sReportName);
                if (result.ExecuteNonQuery() != 0)
                { }
                result.Close();
            }
        }

        private void PrepareSelectedTax()
        {
            OracleResultSet result = new OracleResultSet();
            int iCnt = 0;

            result.Query = "delete from BTAX_REP_ABSTRACT where user_code = :1 and report_name = :2 and fees_code like 'B%'";
            result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode.Trim());
            result.AddParameter(":2", m_sReportName);
            if (result.ExecuteNonQuery() != 0)
            { }
            result.Close();

            for (int i = 0; i < dgvTax.RowCount; i++)
            {
                if ((bool)dgvTax.Rows[i].Cells[0].Value == true && iCnt < 9)
                {
                    iCnt++;
                    result.Query = "insert into BTAX_REP_ABSTRACT values (:1, :2, :3)";
                    result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode.Trim());
                    result.AddParameter(":2", "B"+dgvTax.Rows[i].Cells[1].Value.ToString());
                    result.AddParameter(":3", m_sReportName);
                    if (result.ExecuteNonQuery() != 0)
                    { }
                    result.Close();
                }
            }

            if (iCnt < 9)
            {
                for (int i = 0; i < dgvTax.RowCount; i++)
                {
                    if ((bool)dgvTax.Rows[i].Cells[0].Value == false && iCnt < 9)
                    {
                        iCnt++;
                        result.Query = "insert into BTAX_REP_ABSTRACT values (:1, :2, :3)";
                        result.AddParameter(":1", AppSettingsManager.SystemUser.UserCode.Trim());
                        result.AddParameter(":2", "B"+dgvTax.Rows[i].Cells[1].Value.ToString());
                        result.AddParameter(":3", m_sReportName);
                        if (result.ExecuteNonQuery() != 0)
                        { }
                        result.Close();
                    }
                }
            }
        }

        private void dgvFees_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            iRowFeesIndex = 0;
            if (e.RowIndex != -1)
            {
                iRowFeesIndex = e.RowIndex;
                iCheckFees = 0;

                for (int i = 0; i < dgvFees.RowCount; i++)
                {
                    if ((bool)dgvFees.Rows[i].Cells[0].Value == true)
                        iCheckFees++;


                    if (iCheckFees > 10)
                    {
                        
                        MessageBox.Show("Maximum number of individual column for abstract report reached.\nPlease limit to ten (10) fees only.");
                        dgvFees.Rows[iRowFeesIndex].Cells[0].Value = false;
                        iRowFeesIndex = 0;
                        return;
                    }
                }


            }
        }

        private void dgvTax_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            iRowTaxIndex = 0;
            if (e.RowIndex != -1)
            {
                iRowTaxIndex = e.RowIndex;
                iCheckTax = 0;

                for (int i = 0; i < dgvTax.RowCount; i++)
                {
                    if ((bool)dgvTax.Rows[i].Cells[0].Value == true)
                        iCheckTax++;


                    if (iCheckTax > 9)
                    {

                        MessageBox.Show("Maximum number of individual column for abstract report reached.\nPlease limit to nine (9) line of business only.");
                        dgvTax.Rows[iRowTaxIndex].Cells[0].Value = false;
                        iRowTaxIndex = 0;
                        return;
                    }
                }


            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}