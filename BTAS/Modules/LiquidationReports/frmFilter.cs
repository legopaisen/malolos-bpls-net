using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Amellar.Modules.LiquidationReports;
using Amellar.Common.StringUtilities;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;

namespace Amellar.Modules.LiquidationReports
{
    public partial class frmFilter : Form
    {
        public frmFilter()
        {
            InitializeComponent();
        }

        private bool m_bProceed = false;
        public bool isProceed
        {
            get { return m_bProceed; }
            set { m_bProceed = value; }
        }

        private string m_sHeaderTitle = "";
        public string HeaderTitle
        {
            set { m_sHeaderTitle = value; }
        }

        private string m_sBnsCode = "";
        public string BnsCode
        {
            set { m_sBnsCode = value; }
        }

        private ArrayList m_arrSelectedValue = new ArrayList();
        public ArrayList arrSelectedValue
        {
            get { return m_arrSelectedValue; }
            set { m_arrSelectedValue = value; }
        }

        private void frmFilter_Load(object sender, EventArgs e)
        {
            KHTitle.Text = m_sHeaderTitle;
            LoadData();
        }

        private void LoadData()
        {
            dgvBns.Rows.Clear();
            OracleResultSet pSet = new OracleResultSet();

            if (m_sHeaderTitle.Contains("Line"))
                pSet.Query = "select bns_code,bns_desc from bns_table where fees_code = 'B' and length(rtrim(bns_code)) > 3 and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "' and rtrim(bns_code) like '" + m_sBnsCode + "%%' order by bns_desc";
            else
                pSet.Query = "select bns_code,bns_desc from bns_table where fees_code = 'B' and length(rtrim(bns_code)) = 2 and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "' order by bns_desc";

            if (pSet.Execute())
                while (pSet.Read())
                    dgvBns.Rows.Add(false, pSet.GetString("bns_code"), pSet.GetString("bns_desc"));
            pSet.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_bProceed = false;
            m_arrSelectedValue = null;
            this.Close();
        }

        private bool ValidateFilter()
        {
            int iCnt = 0;
            for (int i = 0; i < dgvBns.RowCount; i++)
                if ((bool)dgvBns.Rows[i].Cells[0].Value == true)
                {
                    iCnt++;
                    break;
                }

            if (iCnt == 0)
            {
                MessageBox.Show("Select atleast 1(one) business.", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ValidateFilter())
                return;

            m_arrSelectedValue.Clear();
            if (MessageBox.Show("Are you sure you want to proceed?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                for (int i = 0; i < dgvBns.RowCount; i++)
                {
                    if ((bool)dgvBns.Rows[i].Cells[0].Value == true)
                        m_arrSelectedValue.Add("'" + dgvBns.Rows[i].Cells[2].Value + "',");
                }

                m_bProceed = true;
                this.Close();
            }
        }
    }
}