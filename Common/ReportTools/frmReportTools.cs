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

namespace Amellar.Common.ReportTools
{
    public partial class frmReportTools : Form
    {
        public frmReportTools()
        {
            InitializeComponent();
        }

        private string m_sSystem = "";

        public string SystemName
        {
            set { m_sSystem = value; }
        }

        private void frmReportTools_Load(object sender, EventArgs e)
        {
            PopulatedgView();
        }

        private void PopulatedgView()
        {
            dgView.Rows.Clear();
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select report_name,user_code,report_date,switch from gen_info where system = '" + m_sSystem + "' and switch = 1 order by report_name";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    dgView.Rows.Add(pSet.GetString(0), pSet.GetString(1), pSet.GetDateTime(2), pSet.GetInt(3));
                }
            }
            pSet.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEndTask_Click(object sender, EventArgs e)
        {
            String sQuery, sToolsReportName, sToolsUserCode;
            
            if(dgView.Rows.Count == 0)
            {
                MessageBox.Show("No active generating report(s)", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            sToolsReportName = dgView.CurrentRow.Cells[0].Value.ToString();
            sToolsUserCode = dgView.CurrentRow.Cells[1].Value.ToString();
            if (sToolsReportName.Trim() == "")
            {
                MessageBox.Show("Select a report name", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            OracleResultSet pSet = new OracleResultSet();
            if (MessageBox.Show("Are you sure you want to \nterminate this record?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (sToolsUserCode.Trim() == "")
                    sQuery = string.Format("delete from gen_info where report_name = '{0}' and user_code is null", sToolsReportName);
                else
                    sQuery = string.Format("delete from gen_info where report_name = '{0}' and user_code = '{1}'", sToolsReportName, sToolsUserCode);
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();
                pSet.Close();

                PopulatedgView();
                MessageBox.Show("Successfully Deleted", " ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}