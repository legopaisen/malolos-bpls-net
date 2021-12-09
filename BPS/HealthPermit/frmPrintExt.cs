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

namespace Amellar.Modules.HealthPermit
{
    public partial class frmPrintExt : Form
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

        public string BnsAdd
        {
            set { txtBNSAdd.Text = value; }
        }

        public string Owner
        {
            set { txtBNSOwner.Text = value; }
        }
        

        public frmPrintExt()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sIssuedDate = AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");
                
            pSet.Query = "select * from sanitary_bldg_ext where bin = '" + txtBIN.Text + "'";
            pSet.Query += " and tax_year = '" + ConfigurationAttributes.CurrentYear + "'";
            //pSet.Query += " and bns_nm = '" + StringUtilities.HandleApostrophe(txtBNSName.Text.Trim()) + "'";
            if(pSet.Execute())
            {
                if (!pSet.Read())
                {
                    pSet.Close();

                    pSet.Query = "insert into sanitary_bldg_ext (BIN, TAX_YEAR, BNS_NM,BNS_ADD,USER_CODE,DATE_PRINTED) values (";
                    pSet.Query += "'" + txtBIN.Text + "', ";
                    pSet.Query += "'" + ConfigurationAttributes.CurrentYear + "', ";
                    pSet.Query += "'" + StringUtilities.HandleApostrophe(txtBNSName.Text.Trim()) + "', ";
                    pSet.Query += "'" + StringUtilities.HandleApostrophe(txtBNSAdd.Text.Trim()) + "', ";
                    pSet.Query += "'" + AppSettingsManager.SystemUser.UserCode + "',";
                    pSet.Query += "to_date('" + sIssuedDate + "','MM/dd/yyyy'))";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }
                }
                else
                {
                    pSet.Close();

                    pSet.Query = "update sanitary_bldg_ext set bns_nm = '" + StringUtilities.HandleApostrophe(txtBNSName.Text.Trim()) + "'";
                    pSet.Query += " where bin = '" + txtBIN.Text + "'";
                    pSet.Query += " and tax_year = '" + ConfigurationAttributes.CurrentYear + "'";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }
                }
            }
            pSet.Close();

            CheckofPermitRecord();
            frmPrinting frmPrinting = new frmPrinting();
            m_sIssuedDate = AppSettingsManager.GetSystemDate().ToString("MM/dd/yyy");
            frmPrinting.PermitNo = m_sPermitNumber;
            frmPrinting.BIN = txtBIN.Text;
            frmPrinting.TaxYear = AppSettingsManager.GetConfigValue("12");  // RMC 20141222 modified permit printing
            frmPrinting.ReportType = m_sCertPermType;
            frmPrinting.BnsName = txtBNSName.Text;  // RMC 20141222 modified permit printing
            frmPrinting.BnsAdd = txtBNSAdd.Text;    // RMC 20141222 modified permit printing
            frmPrinting.BnsOwn = txtBNSOwner.Text;  // RMC 20141222 modified permit printing
            //frmPrinting.ORDate = m_sORDate;   // RMC 20141222 modified permit printing
            //frmPrinting.ORNo = m_sORNO;   // RMC 20141222 modified permit printing
            //frmPrinting.FeeAmount = m_sFeeAmount; // RMC 20141222 modified permit printing
            frmPrinting.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtBNSName_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmPrintExt_Load(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from sanitary_bldg_ext where bin = '" + txtBIN.Text +"' and tax_year = '" + ConfigurationAttributes.CurrentYear + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    txtBNSName.Text = pSet.GetString("bns_nm");
                }
            }
            pSet.Close();
        }

        private void CheckofPermitRecord()
        {
            //Note: Sanitary and Sanitary-Ext shares the same serie

            m_sIssuedDate = "";
            #region GetNextSeries
            OracleResultSet result = new OracleResultSet();
            String sQuery = String.Empty;

            m_sCertPermType = "Sanitary-Ext";

            result.Query = "select coalesce(max(to_number(permit_number)),0)+1 as permit_number from permit_type where perm_type = 'Sanitary' and current_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    if (m_sCertPermType == "Sanitary-Ext")
                        m_sPermitNumber = AppSettingsManager.GetConfigValue("12") + "-" + result.GetInt(0).ToString("0000");
                    else if (m_sCertPermType == "Annual Inspection")
                        m_sPermitNumber = result.GetInt(0).ToString("0000");
                }
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
                    if (m_sCertPermType == "Sanitary-Ext")
                        m_sPermitNumber = result.GetString(0) + "-" + result.GetString(2);
                    else if (m_sCertPermType == "Annual Inspection")
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

        private void txtBNSAdd_TextChanged(object sender, EventArgs e)
        {

        }
    }
}