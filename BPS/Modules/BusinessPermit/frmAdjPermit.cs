using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.BIN;
using Amellar.Common.SearchBusiness;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.BusinessPermit
{
    public partial class frmAdjPermit : Form
    {
        public frmAdjPermit()
        {
            InitializeComponent();
        }
        private string m_strModule = string.Empty;
        private string m_sPermitYear = string.Empty;
        private string m_sPermitNum = string.Empty;
        private string m_sBIN = string.Empty;

        private void btnSearch_Click(object sender, EventArgs e)
        {

            if (bin1.txtTaxYear.Text == "" || bin1.txtBINSeries.Text == "")
            {
                frmSearchBusiness frmSearchBns = new frmSearchBusiness();
                frmSearchBns.ModuleCode = m_strModule;
                frmSearchBns.ShowDialog();

                if (frmSearchBns.sBIN.Length > 1)
                {
                    bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                    bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
                }
                else
                {
                    bin1.txtTaxYear.Text = "";
                    bin1.txtBINSeries.Text = "";
                    txtBNSName.Text = string.Empty;
                    txtBNSOwner.Text = string.Empty;
                    txtOwnAdd.Text = string.Empty;
                    txtPermitYear.Text = string.Empty;
                    txtPermitSeries.Text = string.Empty;
                }
            }
            m_sBIN = bin1.GetBin();
            LoadInfo(bin1.GetBin());
            GetPermit(bin1.GetBin());
        }

        private void LoadInfo(string sBin)
        {
            txtBNSName.Text = AppSettingsManager.GetBnsName(sBin);
            txtBNSOwner.Text = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(sBin));
            txtOwnAdd.Text = AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetBnsOwnCode(sBin), true);
        }

        private void GetPermit(string sBin)
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from businesses where bin = '"+ sBin +"'";
            if(pSet.Execute())
            {
                if(pSet.Read())
                {
                    m_sPermitNum = pSet.GetString("permit_no").Substring(5, 5);
                    m_sPermitYear = pSet.GetString("permit_no").Substring(0, 4);
                }
            }
            pSet.Close();

            txtPermitSeries.Text = m_sPermitNum;
            txtPermitYear.Text = m_sPermitYear;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAdjPermit_Load(object sender, EventArgs e)
        {
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;

            bin1.txtTaxYear.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string sPermitNo = string.Empty;
            OracleResultSet pSet = new OracleResultSet();

            if (m_sPermitYear == "" || m_sPermitNum == "")
                sPermitNo = m_sPermitYear + "" + m_sPermitNum;
            else
                sPermitNo = m_sPermitYear+ "-" + m_sPermitNum;

            if (!(m_sBIN == "") && m_sBIN != "" && m_sBIN != " ")
            {

                if (MessageBox.Show("Update Permit No. of BIN: " + m_sBIN + " ?", "Adj Business Permit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pSet.Query = "update businesses set permit_no='"+ sPermitNo +"' where bin='"+ m_sBIN +"'";
                    MessageBox.Show("Permit no.: " + sPermitNo + " has been update to BIN: " + m_sBIN + "");
                }
            }
            else
                MessageBox.Show("No Data To be Updated");

            Clear();

            if (AuditTrail.InsertTrail("ABMP-A", "trail table", m_sBIN) == 0) //JARS 20170911
            { }
        }

        private void bin1_Leave(object sender, EventArgs e)
        {
            int ilen_ctr = 0, if_ctr = 0;

            if (bin1.txtBINSeries.Text != "")
            {
                ilen_ctr = bin1.txtBINSeries.Text.Length;
                if (ilen_ctr != 7)
                {
                    for (if_ctr = ilen_ctr; if_ctr < 7; if_ctr++)
                    {
                        bin1.txtBINSeries.Text = "0" + bin1.txtBINSeries.Text.Trim();
                    }
                }
            }
        }

        private void txtPermitSeries_Leave(object sender, EventArgs e)
        {
            int ilen_ctr = 0, if_ctr = 0;

            if (m_sPermitNum != "")
            {
                ilen_ctr = m_sPermitNum.Length;
                if (ilen_ctr != 5)
                {
                    for (if_ctr = ilen_ctr; if_ctr < 5; if_ctr++)
                    {
                        m_sPermitNum = "0" + m_sPermitNum.Trim();
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void Clear()
        {
            txtBNSName.Text = "";
            txtBNSOwner.Text = "";
            txtOwnAdd.Text = "";
            txtPermitSeries.Text = "";
            txtPermitYear.Text = "";
            bin1.txtBINSeries.Text = "";
            bin1.txtTaxYear.Text = "";
        }
    }
}