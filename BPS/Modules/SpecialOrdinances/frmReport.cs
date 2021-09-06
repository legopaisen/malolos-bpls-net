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
using Amellar.Common.AuditTrail;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.SpecialOrdinances
{
    public partial class frmReport : Form
    {
        private string m_sSwitch = string.Empty;

        public string Switch
        {
            get { return m_sSwitch; }
            set { m_sSwitch = value; }
        }

        public frmReport()
        {
            InitializeComponent();
            
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            EnableControls(false);
            LoadUser();
            LoadBnsType();

            if (m_sSwitch == "BOI")
            {
                this.Text = "Print Businesses tagged with BOI";
            }
            else
            {
                this.Text = "Print PEZA members";
            }

            chkAll.Checked = true;
        }

        private void EnableControls(bool blnEnable)
        {
            cmbUserCode.Enabled = blnEnable;
            dtpDateFrom.Enabled = blnEnable;
            dtpDateTo.Enabled = blnEnable;
            dtpTagDate.Enabled = blnEnable;
            cmbBnsType.Enabled = blnEnable;
        }

        private void chkAll_CheckStateChanged(object sender, EventArgs e)
        {
            // prints all records with no filtering
            if (chkAll.CheckState.ToString() == "Checked")
            {
                EnableControls(false);
                chkUser.Checked = false;
                chkDateRange.Checked = false;
                chkTagDate.Checked = false;
                chkBnsType.Checked = false;
            }
        }

        private void chkUser_CheckStateChanged(object sender, EventArgs e)
        {
            /*if (chkUser.CheckState.ToString() == "Checked")
            {
                chkAll.Checked = false;
                chkDateRange.Checked = false;
                chkTagDate.Checked = false;
                chkBnsType.Checked = false;
                EnableControls(false);
                cmbUserCode.Enabled = true;
            }*/

            // enable combination of options
            chkAll.Checked = false;
            EnableControls(true);
        }

        private void chkDateRange_CheckStateChanged(object sender, EventArgs e)
        {
            /*if (chkDateRange.CheckState.ToString() == "Checked")
            {
                chkAll.Checked = false;
                chkUser.Checked = false;
                chkTagDate.Checked = false;
                chkBnsType.Checked = false;
                EnableControls(false);
                dtpDateFrom.Enabled = true;
                dtpDateTo.Enabled = true;
            }*/
            // enable combination of options

            chkAll.Checked = false;
            EnableControls(true);
        }

        private void chkTagDate_CheckStateChanged(object sender, EventArgs e)
        {
            /*if (chkTagDate.CheckState.ToString() == "Checked")
            {
                chkAll.Checked = false;
                chkUser.Checked = false;
                chkDateRange.Checked = false;
                chkBnsType.Checked = false;
                EnableControls(false);
                dtpTagDate.Enabled = true;

            }*/
            // enable combination of options

            chkAll.Checked = false;
            EnableControls(true);
        }

        private void chkBnsType_CheckStateChanged(object sender, EventArgs e)
        {
            /*if (chkBnsType.CheckState.ToString() == "Checked")
            {
                chkAll.Checked = false;
                chkUser.Checked = false;
                chkDateRange.Checked = false;
                chkTagDate.Checked = false;
                EnableControls(false);
                cmbBnsType.Enabled = true;

            }
             */
            // enable combination of options

            chkAll.Checked = false;
            EnableControls(true);
        }

        private void LoadUser()
        {
            OracleResultSet pRec = new OracleResultSet();

            pRec.Query = "select * from sys_users order by usr_code";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    cmbUserCode.Items.Add(pRec.GetString("usr_code"));
                }
            }
            pRec.Close();
        }

        private void LoadBnsType()
        {
            OracleResultSet pRec = new OracleResultSet();

            pRec.Query = "select * from bns_table where fees_code = 'B' and means = 'G' order by bns_code";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    cmbBnsType.Items.Add(pRec.GetString("bns_desc"));
                }
            }
            pRec.Close();
        }

        private void cmbUserCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();

            pRec.Query = string.Format("select trim(usr_ln) ||', '||trim(usr_mi)||' '||trim(usr_fn) from sys_users where usr_code = '{0}'", cmbUserCode.Text.Trim());
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    txtUserName.Text = pRec.GetString(0);
                }
            }
            pRec.Close();
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            //PrintReport PrintClass = new PrintReport();
            frmPrintReportNew PrintClass = new frmPrintReportNew();   // RMC 20120127 modified printing of PEZA report
            PrintClass.m_strSource = m_sSwitch;

            if (chkAll.Checked)
            {
                PrintClass.m_sUser = "%";
                PrintClass.m_sBnsType = "%";
                PrintClass.m_sFrom = "";
                PrintClass.m_sTo = "";
                PrintClass.m_sTag = "";
            }
            else
            {
                if (chkUser.Checked)
                    PrintClass.m_sUser = cmbUserCode.Text.Trim();
                else
                    PrintClass.m_sUser = "%";

                if (chkDateRange.Checked)
                {
                    PrintClass.m_sFrom = string.Format("{0:MM/dd/yyyy}", dtpDateFrom.Value);
                    PrintClass.m_sTo = string.Format("{0:MM/dd/yyyy}", dtpDateTo.Value);
                }
                else
                {
                    PrintClass.m_sFrom = "";
                    PrintClass.m_sTo = "";
                }

                if (chkTagDate.Checked)
                    PrintClass.m_sTag = string.Format("{0:yyyy-MM-dd}", dtpTagDate.Value);
                else
                    PrintClass.m_sTag = "";

                if (chkBnsType.Checked)
                    PrintClass.m_sBnsType = cmbBnsType.Text.Trim();
                else
                    PrintClass.m_sBnsType = "%";

                
            }

            //PrintClass.FormLoad();
            PrintClass.ShowDialog();    // RMC 20120127 modified printing of PEZA report 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}