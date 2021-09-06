using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Common.Reports
{
    public partial class frmBusinessOnQue : Form
    {
        string m_sInspector = string.Empty;
        DateTime dtInspected = DateTime.Now;
        ReportClass rClass = new ReportClass();
        public frmBusinessOnQue()
        {
            InitializeComponent();
        }



        private void frmBusinessOnQue_Load(object sender, EventArgs e)
        {
            LoadBusinesses(string.Empty);
            LoadWithNotice();
        }

        private void LoadWithNotice()
        {
            dgvWithNotice.Rows.Clear();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from ON_QUE_WITH_NOTICE order by bin";
            if (result.Execute())
            {
                while (result.Read())
                {
                    dgvWithNotice.Rows.Add(false, result.GetString("bin").Trim(), AppSettingsManager.GetBnsName(result.GetString("bin")), AppSettingsManager.GetBnsAdd(result.GetString("bin"), ""), AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(result.GetString("bin"))), AppSettingsManager.GetBnsOwnAdd(AppSettingsManager.GetOwnCode(result.GetString("bin"))));
                }
            }
            result.Close();
            
        }

        private void GenerateNotice(string sBin, string sNoticeNo)
        {
            rClass.PrintNoticeOnQue(sBin, m_sInspector, dtInspected, sNoticeNo);
        }

        private void LoadBusinesses(string sBnsNm)
        {
            int iCnt = 0;
            dgvBusinessQue.Rows.Clear();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from business_que where bns_nm like '%" + sBnsNm.Trim() + "%' and bns_stat <> 'NEW' and bin not in (select bin from ON_QUE_WITH_NOTICE) order by bns_nm";
            if (result.Execute())
            {
                while (result.Read())
                {
                    iCnt += 1;
                    dgvBusinessQue.Rows.Add(false, result.GetString("bin").Trim(), AppSettingsManager.GetBnsName(result.GetString("bin")), AppSettingsManager.GetBnsAdd(result.GetString("bin"), ""), AppSettingsManager.GetBnsOwner(result.GetString("own_code")), AppSettingsManager.GetBnsOwnAdd(result.GetString("own_code")));
                }
            }
            result.Close();
            lblNo.Text = "Total Number of Business/es: " + iCnt.ToString();
        }

        private void txtBnsName_TextChanged(object sender, EventArgs e)
        {
            LoadBusinesses(txtBnsName.Text.Trim());
            LoadWithNotice();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAll.Checked)
            {
                for (int i = 0; i < dgvBusinessQue.RowCount; i++)
                    dgvBusinessQue.Rows[i].Cells[0].Value = true;
            }
            else
            {
                for (int i = 0; i < dgvBusinessQue.RowCount; i++)
                    dgvBusinessQue.Rows[i].Cells[0].Value = false;
            }
        }

        private void dgvBusinessQue_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                txtAdd.Text = dgvBusinessQue.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtOwner.Text = dgvBusinessQue.Rows[e.RowIndex].Cells[4].Value.ToString();
            }
        }

        private void btnAddtoInspector_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();
            for (int x = 0; x < dgvBusinessQue.RowCount; x++)
            {
                if ((bool)dgvBusinessQue.Rows[x].Cells[0].Value)
                {
                    result.Query = "insert into ON_QUE_WITH_NOTICE values (:1)";
                    result.AddParameter(":1", dgvBusinessQue.Rows[x].Cells[1].Value.ToString());
                    if (result.ExecuteNonQuery() != 0)
                    { }
                    result.Close();
                    GenerateNotice(dgvBusinessQue.Rows[x].Cells[1].Value.ToString(), "1");
                }
            }

            rClass.PreviewDocu();
            LoadWithNotice();
            LoadBusinesses(txtBnsName.Text.Trim());
        }

        private void btnIssue2ndNotice_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();
            for (int x = 0; x < dgvBusinessQue.RowCount; x++)
            {
                if ((bool)dgvBusinessQue.Rows[x].Cells[0].Value)
                {
                    result.Query = "insert into ON_QUE_WITH_NOTICE values (:1)";
                    result.AddParameter(":1", dgvBusinessQue.Rows[x].Cells[1].Value.ToString());
                    if (result.ExecuteNonQuery() != 0)
                    { }
                    result.Close();
                    GenerateNotice(dgvBusinessQue.Rows[x].Cells[1].Value.ToString(), "2");
                }
            }

            rClass.PreviewDocu();
            LoadWithNotice();
            LoadBusinesses(txtBnsName.Text.Trim());
        }
    }
}