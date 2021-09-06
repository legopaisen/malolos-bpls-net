// JHMN 20170118 retirement reversal module

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.Retirement
{
    public partial class frmRetirementReversal : Form
    {
        public frmRetirementReversal()
        {
            InitializeComponent();
        }

        private void frmRetirementReversal_Load(object sender, EventArgs e)
        {
            bin1.txtTaxYear.Focus();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;  // RMC 20110311 
            bin1.GetDistCode = ConfigurationAttributes.DistCode;    // RMC 20110311 

            //btnReverse.Enabled = false;   // JHMN 20170118 mods in reverse retirement module
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text == "Search")
            {
                btnSearch.Text = "Clear";
                string sBin = string.Empty;
              

                
                if (bin1.txtTaxYear.Text == "" || bin1.txtBINSeries.Text == "")
                {
                    MessageBox.Show("Fill-out all the textbox first.");
                    return;
                }
                sBin = bin1.GetBin();
              

                OracleResultSet result = new OracleResultSet();

                //result.Query = "SELECT * FROM retired_bns WHERE bin = '" + sBin + "' and bin IN (SELECT bin FROM pay_hist WHERE bns_stat = 'RET')";
                result.Query = "SELECT * FROM retired_bns WHERE bin = '" + sBin + "'and bin IN (SELECT bin FROM pay_hist WHERE bns_stat = 'RET' and data_mode <> 'UNP')";  // RMC 20170321 correction in retire reverse module
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        result.Close();

                        MessageBox.Show("Selected business has undergone retirement with payment.\nReversal is not allowed", "Reverse Retirement", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        bin1.txtTaxYear.Text = string.Empty;
                        bin1.txtBINSeries.Text = string.Empty;
                        btnSearch.Text = "Search";
                        btnReverse.Enabled = false;
                        return;
                    }
                }
                result.Close();

                result.Query = "SELECT * FROM retired_bns WHERE bin IN (SELECT bin FROM addl WHERE bns_stat = 'RET' AND bin = '" + sBin + "') ";
                result.Query += "And bin NOT IN (SELECT bin FROM pay_hist WHERE bns_stat = 'RET' AND bin = '" + sBin + "') AND MAIN = 'N'";  //JHB 20180301 'N' for addl_bns
                result.Query += "union all SELECT * FROM retired_bns WHERE bin IN (SELECT bin FROM businesses WHERE bns_stat = 'RET' AND bin = '" + sBin + "') ";
                result.Query += "AND bin NOT IN (SELECT bin FROM pay_hist WHERE bns_stat = 'RET' AND bin = '" + sBin + "') AND MAIN = 'Y'";

                //result.Query = "SELECT * FROM retired_bns WHERE bin IN (SELECT bin FROM businesses WHERE bns_stat = 'RET' AND bin = '" + sBin + "') AND bin NOT IN (SELECT bin FROM pay_hist WHERE bns_stat = 'RET' AND bin = '" + sBin + "') AND MAIN = 'Y'";
                if (result.Execute())
                    if (result.Read())
                    {
                        this.LoadInfo(sBin.Trim());
                        result.Close();

                        btnReverse.Enabled = true;   // JHMN 20170118 mods in reverse retirement module
                        return;
                    }
                result.Close();

                MessageBox.Show("Selected business has no record of retirement.", "Reverse Retirement", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                bin1.txtTaxYear.Text = string.Empty;
                bin1.txtBINSeries.Text = string.Empty;
                btnSearch.Text = "Search";
                btnReverse.Enabled = false;
                return;
            }
            else
            {
                btnSearch.Text = "Search";

                bin1.txtTaxYear.Text = string.Empty;
                bin1.txtBINSeries.Text = string.Empty;
                txtBNSAdd.Text = string.Empty;
                txtBNSName.Text = string.Empty;
                txtBNSOwner.Text = string.Empty;
                bin1.txtTaxYear.Focus();
                btnReverse.Enabled = false;   // JHMN 20170118 mods in reverse retirement module
            }
        }

        private void LoadInfo(string sBin)
        {
            txtBNSName.Text = AppSettingsManager.GetBnsName(sBin);
            txtBNSAdd.Text = AppSettingsManager.GetBnsAddress(sBin);
            txtBNSOwner.Text = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(sBin));
        }

        private void btnReverse_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            string sBin = string.Empty;
            string sTaxYear = string.Empty;
            sBin = bin1.GetBin();

            DialogResult dgresult = MessageBox.Show("Are you sure you want revert the selected retired business?", "Reverse", MessageBoxButtons.YesNo);
            if(dgresult == DialogResult.Yes){
                result.Query = "SELECT tax_year FROM businesses WHERE bin = '" + sBin + "'";
                if (result.Execute())
                    if (result.Read())
                    {
                        sTaxYear = result.GetString("tax_year");
                        result.Close();
                    }
                result.Close();

                result.Query = "DELETE FROM retired_bns WHERE bin ='" + sBin + "' AND tax_year = '" + sTaxYear + "'";
                if (result.ExecuteNonQuery() == 0)
                {
                }

                
                result.Query = "DELETE FROM businesses WHERE bin ='" + sBin + "' AND tax_year = '" + sTaxYear + "'";
                if (result.ExecuteNonQuery() == 0)
                {
                }

                result.Query = "SELECT * FROM buss_hist WHERE bin = '" + sBin + "' ORDER BY save_tm DESC";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        result2.Query = "INSERT INTO businesses VALUES (";
                        result2.Query += "'" + result.GetString("BIN") + "', ";
                        result2.Query += "'" + StringUtilities.HandleApostrophe(result.GetString("BNS_NM")) + "', ";
                        result2.Query += "'" + result.GetString("BNS_STAT") + "', ";
                        result2.Query += "'" + result.GetString("OWN_CODE") + "', ";
                        result2.Query += "'" + StringUtilities.HandleApostrophe(result.GetString("BNS_TELNO")) + "', ";
                        result2.Query += "'" + StringUtilities.HandleApostrophe(result.GetString("BNS_HOUSE_NO")) + "', ";
                        result2.Query += "'" + StringUtilities.HandleApostrophe(result.GetString("BNS_STREET")) + "', ";
                        result2.Query += "'" + result.GetString("BNS_MUN") + "', ";
                        result2.Query += "'" + result.GetString("BNS_DIST") + "', ";
                        result2.Query += "'" + result.GetString("BNS_ZONE") + "', ";
                        result2.Query += "'" + result.GetString("BNS_BRGY") + "', ";
                        result2.Query += "'" + result.GetString("BNS_PROV") + "', ";
                        result2.Query += "'" + result.GetString("BNS_ZIP") + "', ";
                        result2.Query += "'" + result.GetString("LAND_PIN") + "', ";
                        result2.Query += "'" + result.GetString("BLDG_PIN") + "', ";
                        result2.Query += "'" + result.GetString("MACH_PIN") + "', ";
                        result2.Query += "'" + StringUtilities.HandleApostrophe(result.GetString("POFF_HOUSE_NO")) + "', ";
                        result2.Query += "'" + StringUtilities.HandleApostrophe(result.GetString("POFF_STREET")) + "', ";
                        result2.Query += "'" + result.GetString("POFF_MUN") + "', ";
                        result2.Query += "'" + result.GetString("POFF_DIST") + "', ";
                        result2.Query += "'" + result.GetString("POFF_ZONE") + "', ";
                        result2.Query += "'" + result.GetString("POFF_BRGY") + "', ";
                        result2.Query += "'" + result.GetString("POFF_PROV") + "', ";
                        result2.Query += "'" + result.GetString("POFF_ZIP") + "', ";
                        result2.Query += "'" + result.GetString("ORGN_KIND") + "', ";
                        result2.Query += "'" + StringUtilities.HandleApostrophe(result.GetString("BUSN_OWN")) + "', ";
                        result2.Query += "'" + StringUtilities.HandleApostrophe(result.GetString("CTC_NO")) + "', ";
                        string sCTC_ISSUED_ON = string.Format("{0:MM/dd/yyyy}", result.GetDateTime("CTC_ISSUED_ON"));
                        result2.Query += "to_date('" + sCTC_ISSUED_ON + "', 'MM/dd/yyyy'), ";
                        result2.Query += "'" + result.GetString("CTC_ISSUED_AT") + "', ";
                        result2.Query += "'" + result.GetString("BNS_CODE") + "', ";
                        result2.Query += "'" + result.GetString("SSS_NO") + "', ";
                        string sSSS_ISSUED_ON = string.Format("{0:MM/dd/yyyy}", result.GetDateTime("SSS_ISSUED_ON"));
                        result2.Query += "to_date('" + sSSS_ISSUED_ON + "', 'MM/dd/yyyy'), ";
                        result2.Query += "'" + result.GetString("DTI_REG_NO") + "', ";
                        string sDTI_REG_DT = string.Format("{0:MM/dd/yyyy}", result.GetDateTime("DTI_REG_DT"));
                        result2.Query += "to_date('" + sDTI_REG_DT + "', 'MM/dd/yyyy'), ";
                        result2.Query += "" + result.GetFloat("BLDG_VAL") + ", ";
                        result2.Query += "'" + StringUtilities.HandleApostrophe(result.GetString("PLACE_OCCUPANCY")) + "', ";
                        string sRENT_LEASE_SINCE = string.Format("{0:MM/dd/yyyy}", result.GetDateTime("RENT_LEASE_SINCE"));
                        result2.Query += "to_date('" + sRENT_LEASE_SINCE + "', 'MM/dd/yyyy'), ";
                        result2.Query += "" + result.GetFloat("RENT_LEASE_MO") + ", ";
                        result2.Query += "" + result.GetFloat("FLR_AREA") + ", ";
                        result2.Query += "" + result.GetInt("NUM_STOREYS") + ", ";
                        result2.Query += "" + result.GetFloat("TOT_FLR_AREA") + ", ";
                        result2.Query += "'" + StringUtilities.HandleApostrophe(result.GetString("PREV_BNS_OWN")) + "', ";
                        result2.Query += "" + result.GetInt("NUM_EMPLOYEES") + ", ";
                        result2.Query += "" + result.GetInt("NUM_PROFESSIONAL") + ", ";
                        result2.Query += "" + result.GetFloat("ANNUAL_WAGES") + ", ";
                        result2.Query += "" + result.GetFloat("AVE_ELECTRIC_BILL") + ", ";
                        result2.Query += "" + result.GetFloat("AVE_WATER_BILL") + ", ";
                        result2.Query += "" + result.GetFloat("AVE_PHONE_BILL") + ", ";
                        result2.Query += "" + result.GetFloat("OTHER_UTIL") + ", ";
                        result2.Query += "" + result.GetInt("NUM_DELIV_VEHICLE") + ", ";
                        result2.Query += "" + result.GetInt("NUM_MACHINERIES") + ", ";
                        string sDT_OPERATED = string.Format("{0:MM/dd/yyyy}", result.GetDateTime("DT_OPERATED"));
                        result2.Query += "to_date('" + sDT_OPERATED + "', 'MM/dd/yyyy'), ";
                        result2.Query += "'" + result.GetString("PERMIT_NO") + "', ";
                        string sPERMIT_DT = string.Format("{0:MM/dd/yyyy}", result.GetDateTime("PERMIT_DT"));
                        result2.Query += "to_date('" + sPERMIT_DT + "', 'MM/dd/yyyy'), ";
                        result2.Query += "" + result.GetFloat("GR_1") + ", ";
                        result2.Query += "" + result.GetFloat("GR_2") + ", ";
                        result2.Query += "" + result.GetFloat("CAPITAL") + ", ";
                        result2.Query += "'" + result.GetString("OR_NO") + "', ";
                        result2.Query += "'" + result.GetString("TAX_YEAR") + "', ";
                        result2.Query += "'" + StringUtilities.HandleApostrophe(result.GetString("CANC_REASON")) + "', ";
                        string sCANC_DATE = string.Format("{0:MM/dd/yyyy}", result.GetDateTime("CANC_DATE"));
                        result2.Query += "to_date('" + sCANC_DATE + "', 'MM/dd/yyyy'), ";
                        result2.Query += "'" + result.GetString("CANC_BY") + "', ";
                        result2.Query += "'" + result.GetString("BNS_USER") + "', ";
                        string sSAVE_TM = string.Format("{0:MM/dd/yyyy}", result.GetDateTime("SAVE_TM"));
                        result2.Query += "to_date('" + sSAVE_TM + "', 'MM/dd/yyyy'), ";
                        result2.Query += "'" + StringUtilities.HandleApostrophe(result.GetString("MEMORANDA")) + "', ";
                        result2.Query += "'" + StringUtilities.HandleApostrophe(result.GetString("BNS_EMAIL")) + "', ";
                        result2.Query += "'" + result.GetString("TIN_NO") + "', ";
                        string sTIN_ISSUED_ON = string.Format("{0:MM/dd/yyyy}", result.GetDateTime("TIN_ISSUED_ON"));
                        result2.Query += "to_date('" + sTIN_ISSUED_ON + "', 'MM/dd/yyyy'), ";
                        string sDT_APPLIED = string.Format("{0:MM/dd/yyyy}", result.GetDateTime("DT_APPLIED"));
                        result2.Query += "to_date('" + sDT_APPLIED + "', 'MM/dd/yyyy'))";
                        //MessageBox.Show(result2.Query.ToString());
                        if (result2.ExecuteNonQuery() == 0)
                        {
                        }
                    }
                }
                result.Close();
                

                MessageBox.Show("BIN " + sBin + " has been successfully reverse.", "Retirement Reversal", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (AuditTrail.InsertTrail("AAE-NB-R", "retired_bns, businesses", StringUtilities.HandleApostrophe(sBin)) == 0)
                {
                    result2.Rollback();
                    result2.Close();
                    MessageBox.Show(result2.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                result.Query = "DELETE FROM buss_hist WHERE bin ='" + sBin + "' AND tax_year = '" + sTaxYear + "'";
                if (result.ExecuteNonQuery() == 0)
                {
                }

                btnSearch.Text = "Search";

                bin1.txtTaxYear.Text = string.Empty;
                bin1.txtBINSeries.Text = string.Empty;
                txtBNSAdd.Text = string.Empty;
                txtBNSName.Text = string.Empty;
                txtBNSOwner.Text = string.Empty;
                bin1.txtTaxYear.Focus();
                btnReverse.Enabled = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}