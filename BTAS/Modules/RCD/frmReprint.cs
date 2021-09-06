using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using ComponentFactory.Krypton.Toolkit;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.RCD
{
    public partial class frmReprint : Form
    {
        public frmReprint()
        {
            InitializeComponent();
        }

        DateTime m_dtORDate = new DateTime();
        string m_strTellerCode = "";
        OracleResultSet m_Set = new OracleResultSet();
        string m_sUser = "";

        private void GetTellerCode(string strRCDSeries)
        {
            OracleResultSet result = new OracleResultSet();
            //result.Query = "SELECT TELLER,OR_DT FROM RCD_REMIT WHERE RCD_SERIES = '" + strRCDSeries.Trim() + "'";
            result.Query = "SELECT TELLER,OR_DATE FROM PARTIAL_REMIT WHERE RCD_SERIES = '" + strRCDSeries.Trim() + "'"; //MOD MCR20140602
            try
            {
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        m_dtORDate = result.GetDateTime(1);
                        m_strTellerCode = result.GetString(0).Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nPlease contact your software support team", "Err GetTellerCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Trail(OracleResultSet RSet, string sUser, string sModCode, string sTable, string sObject)
        {
            if (AuditTrail.InsertTrail(sModCode, sTable, sObject) == 0)
            {
                m_Set.Rollback();
                m_Set.Close();
                MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cboRCDNo.Items.Count <= 0)
            {
                MessageBox.Show("No Remittance Yet", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (cboRCDNo.Text.Trim() == "")
            {
                MessageBox.Show("Please Select RCD No.", "Reprint RCD", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            GetTellerCode(cboRCDNo.Text);
            //LoadCollection collection = new LoadCollection(m_strTellerCode, m_dtORDate, "", cboRCDNo.Text.Trim()); // MOD MCR 20140602
            
            frmReport form = new frmReport();
            form.Switch = "Re-Print";
            form.RCDNo = cboRCDNo.Text.Trim();
            form.DateTrans = m_dtORDate;
            form.TellerCode = m_strTellerCode;
            //form.ORFrom = collection.ORFrom();
            //form.ORTo = collection.ORTo();
            //form.FormType = collection.FormType();
            //form.Denominations = lstDenomination; //Getting of Denominations
            //form.DenominationsQty = lstDenominationQty; //Getting of Denominations
            //form.DenominationsAmt = lstDenominationAmt;
            form.ShowDialog();

            string strObject = "";
            strObject = "RCD No.:" + cboRCDNo.Text;
            strObject += "/O.R. Date:" + m_dtORDate;
            strObject += "/Teller:" + m_strTellerCode;
            //strObject += "/O.R. Series - From:" + collection.ORFrom() + "/To:" + collection.ORTo();
            Trail(m_Set, m_sUser, "RCD-RG", "RCD_Remit", strObject);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmReprint_Load(object sender, EventArgs e)
        {
            LoadTeller(); //MCR 20140819
        }

        private void LoadRCD()
        {
            cboRCDNo.Items.Clear();
            OracleResultSet result = new OracleResultSet();
            string sDateFrom = dtpFrom.Value.ToString("MM/dd/yyyy");
            string sDateTo = dtpTo.Value.ToString("MM/dd/yyyy");
            //result.Query = "select distinct rcd_series from rcd_remit order by rcd_series desc";
            //result.Query = "select distinct rcd_series from partial_remit where teller = '" + cmbTeller.Text.Trim() + "' and rtrim(or_date) between to_date('" + sDateFrom + "','MM/dd/yyyy') and to_date('" + sDateTo + "','MM/dd/yyyy') order by rcd_series"; //MCR 20140819
            //result.Query = "select distinct rcd_series from partial_remit where teller = '" + cmbTeller.Text.Trim() + "' order by rcd_series desc"; // RMC 20140909 Migration QA  // RMC 20150707 added filtering of RCD date in re-print RCD
            // RMC 20150707 added filtering of RCD date in re-print RCD (s)
            result.Query = "select distinct rcd_series from partial_remit where teller = '" + cmbTeller.Text.Trim() + "' and";
            result.Query += " to_date(to_char(dt_save,'MM/dd/yyyy'),'MM/dd/yyyy') = to_date('" + sDateFrom + "','MM/dd/yyyy')";
            result.Query += " order by rcd_series desc";
            // RMC 20150707 added filtering of RCD date in re-print RCD (e)
            if (result.Execute())
                while (result.Read())
                    cboRCDNo.Items.Add(result.GetString(0).Trim());
            result.Close();
        }

        private void LoadTeller()
        {
            String sQuery = "";
            String sGetString = "";
            sQuery = string.Format("select teller, ln from tellers order by teller");
            cmbTeller.Items.Clear();
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                cmbTeller.Items.Add("");
                while (pSet.Read())
                {
                    cmbTeller.Items.Add(pSet.GetString("teller").Trim());
                }
            }

            if (cmbTeller.Items.Count > 0)
                cmbTeller.SelectedIndex = 0;
            pSet.Close();
        }

        private void cboRCDNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnOk.PerformClick();
        }

        private void buttonSpecAny2_Click(object sender, EventArgs e)
        {
            try { cboRCDNo.SelectedIndex = cboRCDNo.SelectedIndex + 1; }
            catch { }
        }

        private void buttonSpecAny1_Click(object sender, EventArgs e)
        {
            try { cboRCDNo.SelectedIndex = cboRCDNo.SelectedIndex - 1; }
            catch { }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            DateTime cdtToday = new DateTime();
            cdtToday = AppSettingsManager.GetSystemDate();

            if (dtpFrom.Value.Date > cdtToday.Date || dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                MessageBox.Show("Invalid date entry, Please check.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpFrom.Value = AppSettingsManager.GetSystemDate();
            }

            cboRCDNo.Items.Clear(); // RMC 20150707 added filtering of RCD date in re-print RCD
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            DateTime cdtToday = new DateTime();
            cdtToday = AppSettingsManager.GetSystemDate();

            if (dtpTo.Value.Date > cdtToday.Date || dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                MessageBox.Show("Invalid date entry, Please check.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpTo.Value = AppSettingsManager.GetSystemDate();
            }
        }

        private void cboRCDNo_DropDown(object sender, EventArgs e)
        {
            LoadRCD();
        }

        private void cmbTeller_SelectedIndexChanged(object sender, EventArgs e)
        {
            // RMC 20170120 reset rcd no when teller was changed in RCD reprint
            cboRCDNo.Items.Clear();
        }
    }
}
