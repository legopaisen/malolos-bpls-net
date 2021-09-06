using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AuditTrail;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.InspectorsDetails
{
    public partial class frmSendNotice : Form
    {
        private string m_sBin = string.Empty;
        private string m_sNoticeNum = string.Empty;
        private string m_sSource = string.Empty;
        private DateTime m_dtNoticeDate;
        private string m_sNoticeSwitch = string.Empty;
        private bool m_bOK = true;

        public string Bin
        {
            get { return m_sBin; }
            set { m_sBin = value; }
        }

        public string NoticeNum
        {
            get { return m_sNoticeNum; }
            set { m_sNoticeNum = value; }
        }

        public string Source
        {
            get { return m_sSource; }
            set { m_sSource = value; }
        }

        public DateTime NoticeDate
        {
            get { return m_dtNoticeDate; }
            set { m_dtNoticeDate = value; }
        }

        public string NoticeSwitch
        {
            get { return m_sNoticeSwitch; }
            set { m_sNoticeSwitch = value; }
        }

        public bool TransactionOk
        {
            get { return m_bOK; }
            set { m_bOK = value; }
        }

        public frmSendNotice()
        {
            InitializeComponent();
        }

        private void frmSendNotice_Load(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            // RMC 20120417 Modified Final notice format (s)
            string sNoticeDate = "";
            string sCurrentDate = "";

            sNoticeDate = string.Format("{0:MM/dd/yyyy}", dtpDate.Value);
            sCurrentDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
            // RMC 20120417 Modified Final notice format (e)

            if (m_sNoticeSwitch == "ISSUE")
            {
                //if (dtpDate.Value > AppSettingsManager.GetCurrentDate())

                if (Convert.ToDateTime(sNoticeDate) > Convert.ToDateTime(sCurrentDate)) // RMC 20120417 Modified Final notice format
                {
                    MessageBox.Show("Invalid date", "Issue Notice", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                m_dtNoticeDate = dtpDate.Value;

                this.Close();
            }
            else
            {
                //if (dtpDate.Value < m_dtNoticeDate)
                sCurrentDate = string.Format("{0:MM/dd/yyyy}", m_dtNoticeDate); // RMC 20120417 Modified Final notice format

                if (Convert.ToDateTime(sNoticeDate) < Convert.ToDateTime(sCurrentDate)) // RMC 20120417 Modified Final notice format
                {
                    MessageBox.Show("Send date conflicts with notice date.", "Send Notice", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                string sTmpDate = string.Format("{0:yyyyMMdd}", dtpDate.Value);
                //string sCurrentDate = string.Format("{0:yyyyMMdd}", AppSettingsManager.GetCurrentDate());
                sCurrentDate = string.Format("{0:yyyyMMdd}", AppSettingsManager.GetCurrentDate());  // RMC 20120417 Modified Final notice format

                if (Convert.ToInt32(sTmpDate) > Convert.ToInt32(sCurrentDate))
                {
                    MessageBox.Show("Send date conflicts with current date.", "Send Notice", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                string sDate = string.Format("{0:MM/dd/yyyy}", dtpDate.Value);

                if (m_sSource == "Deficient")
                {
                    pSet.Query = string.Format("update official_notice_closure set notice_sent = to_date('{0}','MM/dd/yyyy') where bin = '{1}' and Notice_number = '{2}'", sDate, m_sBin, m_sNoticeNum);
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    MessageBox.Show("Notice Sent.", "Official Business Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (AuditTrail.InsertTrail("ABIDDI-SEND", "official_notice_closure", m_sBin) == 0)
                    {
                        pSet.Rollback();
                        pSet.Close();
                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    this.Close();
                }
                else
                {
                    pSet.Query = string.Format("update unofficial_notice_closure set notice_sent = to_date('{0}','MM/dd/yyyy') where Is_number = '{1}' and Notice_number = '{2}'", sDate, m_sBin, m_sNoticeNum);
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    MessageBox.Show("Notice Sent.", "Unofficial Business Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (AuditTrail.InsertTrail("ABIDUI-SEN", "unofficial_notice_closure", m_sBin) == 0)
                    {
                        pSet.Rollback();
                        pSet.Close();
                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    this.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_bOK = false;
            this.Close();
        }
    }
}