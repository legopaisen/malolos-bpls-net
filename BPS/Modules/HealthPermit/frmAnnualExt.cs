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
    public partial class frmAnnualExt : Form
    {
        private string m_sPermitNumber = string.Empty;
        private string m_sCertPermType = string.Empty;
        private string m_sIssuedDate = string.Empty;

        public string BIN
        {
            set { txtBIN.Text = value; }
        }

        public string BnsName
        {
            set { txtBNSName.Text = value; }
        }

        public frmAnnualExt()
        {
            InitializeComponent();
        }

        private void txtArea_Leave(object sender, EventArgs e)
        {
            double dArea = 0;
            double.TryParse(txtArea.Text, out dArea);

            if (dArea == 0)
            {
                MessageBox.Show("Invalid area. Please enter numeric only.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtArea.Text = "";
                return;
            }
        }

        private void frmAnnualExt_Load(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from sanitary_bldg_ext where bin = '" + txtBIN.Text + "'";
            pSet.Query += " and tax_year = '" + ConfigurationAttributes.CurrentYear + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    txtArea.Text = pSet.GetString("area");
                    txtBNSName.Text = pSet.GetString("bns_nm");
                }
            }
            pSet.Close();

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sOtherInfoCode = string.Empty;
            

            pSet.Query = "update sanitary_bldg_ext set area = '" + txtArea.Text + "'";
            pSet.Query += " where bin = '" + txtBIN.Text + "'";
            pSet.Query += " and tax_year = '" + ConfigurationAttributes.CurrentYear + "'";
            if (pSet.ExecuteNonQuery() == 0)
            { }

            pSet.Query = "select * from default_code where rev_year = '" + ConfigurationAttributes.RevYear + "'";
            pSet.Query += " and default_desc like '%METERS%'";  //NUMBER OF SQ METERS
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sOtherInfoCode = pSet.GetString("default_code");
                    
                }
            }
            pSet.Close();

            double dArea = 0;
            double dNewArea = 0;
            double.TryParse(txtArea.Text, out dNewArea);

            pSet.Query = "select * from other_info where bin = '" + txtBIN.Text + "' and tax_year = '" + ConfigurationAttributes.CurrentYear + "' and default_code = '" + sOtherInfoCode + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    dArea = pSet.GetDouble("data");
                }
            }
            pSet.Close();

            dNewArea = dNewArea + dArea;
            string sArea = string.Format("{0:####}", dNewArea);

            pSet.Query = "update other_info set data = '" + sArea + "' where bin = '" + txtBIN.Text + "' and tax_year = '" + ConfigurationAttributes.CurrentYear + "' and default_code = '" + sOtherInfoCode + "'";
            if (pSet.ExecuteNonQuery() == 0)
            { }

            PermitRecord();

            frmPrinting form = new frmPrinting();
            form.ReportType = "Annual Inspection-Ext";
            form.BIN = txtBIN.Text;
            form.PermitNo = m_sPermitNumber;
            form.ShowDialog();

        }

        private void PermitRecord()
        {
            // RMC 20150105 mods in permit printing
            String m_sPermitNumber = "";
            #region GetNextSeries
            OracleResultSet result = new OracleResultSet();
            String sQuery = String.Empty;
            m_sCertPermType = "ANNUAL-EXT";

            result.Query = "select coalesce(max(to_number(permit_number)),0)+1 as permit_number from permit_type where perm_type = 'ANNUAL' and current_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
            if (result.Execute())
            {
                if (result.Read())
                    m_sPermitNumber = AppSettingsManager.GetConfigValue("12") + "-" + result.GetInt(0).ToString("0000");
            }
            result.Close();
            #endregion

            #region CheckExist
            result.Query = "select * from permit_type where bin = '" + txtBIN.Text + "' and current_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
            result.Query += " and perm_type = '" + m_sCertPermType + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    if (m_sCertPermType == "Sanitary")
                        m_sPermitNumber = result.GetString(0) + "-" + result.GetString(2);
                    else if (m_sCertPermType == "ANNUAL-EXT")
                        m_sPermitNumber = result.GetString(2);
                    m_sIssuedDate = result.GetString(3);
                }
            }
            result.Close();
            #endregion

            #region Saving

            if (m_sIssuedDate == "")
            {
                string sCurrentYear = AppSettingsManager.GetConfigValue("12");
                string sPermType = m_sCertPermType;
                string sPermitNumber = m_sPermitNumber.Substring(5);
                string sIssuedDate = AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");
                string sUserCode = AppSettingsManager.SystemUser.UserCode;
                string s_mBin = txtBIN.Text;

                result.Query = "insert into permit_type (current_year,perm_type,permit_number,issued_date,user_code,bin) values('" + sCurrentYear + "', '" + sPermType + "', '" + sPermitNumber + "', '" + sIssuedDate + "', '" + sUserCode + "', '" + s_mBin + "')";
                result.ExecuteNonQuery();
            }
            #endregion
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}