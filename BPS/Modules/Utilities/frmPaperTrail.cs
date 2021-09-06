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
using Amellar.Common.SearchBusiness;

namespace Amellar.Modules.Utilities
{
    public partial class frmPaperTrail : Form
    {
        public frmPaperTrail()
        {
            InitializeComponent();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
        }

        int m_iTrail = 0;

        private void frmPaperTrail_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearchBIN_Click(object sender, EventArgs e)
        {
            if (bin1.txtTaxYear.Text != "" || bin1.txtBINSeries.Text != "")
            {
                LoadValues(bin1.GetBin());
            }
            else
            {
                frmSearchBusiness frmSearchBns = new frmSearchBusiness();
                frmSearchBns.ShowDialog();

                if (frmSearchBns.sBIN.Length > 1)
                {
                    bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                    bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
                    LoadValues(bin1.GetBin());
                }
            }
        }

        private void LoadValues(String sBin)
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = @"select BIN,BNS_NM,OWN_CODE from businesses where bin = '" + StringUtilities.HandleApostrophe(sBin) + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    txtBnsName.Text = pSet.GetString(1);
                    txtTaxPayrName.Text = AppSettingsManager.GetBnsOwner(pSet.GetString(2));
                    txtBnsAdd.Text = AppSettingsManager.GetBnsAddress(sBin);
                }
            }
            else
            {
                pSet.Query = @"select BIN,BNS_NM,OWN_CODE from businesses where bin = '" + sBin + "'";
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        txtBnsName.Text = pSet.GetString(1);
                        txtTaxPayrName.Text = AppSettingsManager.GetBnsOwner(pSet.GetString(2));
                        txtBnsAdd.Text = AppSettingsManager.GetBnsAddress(sBin);
                    }
            }
            pSet.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            bin1.txtTaxYear.Text = "";
            bin1.txtBINSeries.Text = "";
            dgView.Rows.Clear();

            foreach (object c in this.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Text = "";
            }

            rbApp.Checked = true;

            //foreach (object c in this.GB1.Controls)
            //{
            //    if (c is RadioButton)
            //        ((RadioButton)c).Checked = false;
            //}
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            String sQuery = "", sObject = "", sYear = "";
            int iRow = 0;
            DateTime cdtDate;
            string m_sBin = bin1.GetBin();
            cdtDate = AppSettingsManager.GetSystemDate();
            dgView.Rows.Clear();

            if (m_iTrail == 0)
            {
                sYear = cdtDate.Year.ToString();
                sObject = m_sBin + "/" + sYear;
                sQuery = string.Format("select * from a_trail where mod_code like 'AA%%' and object = '{0}'", sObject);
            }
            else if (m_iTrail == 1)
                sQuery = string.Format("select * from a_trail where mod_code like 'AI%%' and object = '{0}'", m_sBin);
            else if (m_iTrail == 2)
                sQuery = string.Format("select * from billing_tagging where bin = '{0}'", m_sBin);
            else if (m_iTrail == 3)
                sQuery = string.Format("select * from a_trail where mod_code like 'CAP%%' and object = '{0}'", m_sBin);
            else if (m_iTrail == 4)
                sQuery = string.Format("select * from a_trail where mod_code like 'ABBP%%' and object = '{0}'", m_sBin);
            else if (m_iTrail == 5)
                sQuery = "select * from treasurers_module where bin = '" + m_sBin + "'";

            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iRow++;
                    if (m_iTrail == 0 || m_iTrail == 1 || m_iTrail == 3 || m_iTrail == 4)
                    {
                        dgView.Rows.Add(pSet.GetString("mod_code"), pSet.GetString("usr_code"), pSet.GetString("work_station"), pSet.GetDateTime("tdatetime"));
                    }
                    else if (m_iTrail == 5)
                    {
                        String sModName = "TREAS. MODULE";
                        dgView.Rows.Add(StringUtilities.HandleApostrophe(sModName), pSet.GetString("usr_code"), "", pSet.GetDateTime("dt_save"));
                    }
                    else
                    {
                        dgView.Rows.Add("", pSet.GetString("printed_by"), "", pSet.GetDateTime("tdatetime"));
                    }
                }
            }
            pSet.Close();
            
            if (dgView.RowCount == 0)
            {
                MessageBox.Show("Record not Found.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void rbApp_Click(object sender, EventArgs e)
        {
            m_iTrail = 0;
        }

        private void rbBill_Click(object sender, EventArgs e)
        {
            m_iTrail = 1;
        }

        private void rbSOA_Click(object sender, EventArgs e)
        {
            m_iTrail = 2;
        }

        private void rbPymnt_Click(object sender, EventArgs e)
        {
            m_iTrail = 3;
        }

        private void rbPermit_Click(object sender, EventArgs e)
        {
            m_iTrail = 4;
        }

        private void rbTres_Click(object sender, EventArgs e)
        {
            m_iTrail = 5;
        }

    }
}