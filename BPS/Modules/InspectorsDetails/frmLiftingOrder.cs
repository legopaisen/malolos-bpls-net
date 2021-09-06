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

namespace Amellar.Modules.InspectorsDetails
{
    public partial class frmLiftingOrder : Form
    {
        private string m_sBnsType = string.Empty;
        private string m_sBin = string.Empty;
        private bool m_blnSave = false;
        private string m_sIs = string.Empty;

        public string BnsType
        {
            get { return m_sBnsType; }
            set { m_sBnsType = value; }
        }

        public string Bin
        {
            get { return m_sBin; }
            set { m_sBin = value; }
        }

        public bool Save
        {
            get { return m_blnSave; }
            set { m_blnSave = value; }
        }

        public string ISNum
        {
            get { return m_sIs; }
            set { m_sIs = value; }
        }

        public frmLiftingOrder()
        {
            InitializeComponent();
        }

        private void frmLiftingOrder_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_blnSave = false;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            string sDateLifted = string.Empty;

            if (txtRemarks.Text.Trim() == "")
            {
                MessageBox.Show("Please fill out all the fields.", "Closure Un-tagging", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            sDateLifted = string.Format("{0:MM/dd/yyyy}", dtpDate.Value);
            
            if (m_sBnsType == "UNOFFICIAL")
            {
                pSet.Query = "insert into unoff_lift_order values (:1,:2,' ',:3,:4,:5)";
                pSet.AddParameter(":1", m_sIs);
                pSet.AddParameter(":2", DateTime.Parse(sDateLifted));
                pSet.AddParameter(":3", StringUtilities.HandleApostrophe(txtRemarks.Text.Trim()));
                pSet.AddParameter(":4", AppSettingsManager.SystemUser.UserCode);
                pSet.AddParameter(":5", AppSettingsManager.GetCurrentDate());
                if (pSet.ExecuteNonQuery() == 0)
                { }
            }
            else
            {
                pSet.Query = "insert into lifting_order values (:1,:2,' ',:3,:4,:5)";
                pSet.AddParameter(":1", m_sBin);
                pSet.AddParameter(":2", DateTime.Parse(sDateLifted));
                pSet.AddParameter(":3", StringUtilities.HandleApostrophe(txtRemarks.Text.Trim()));
                pSet.AddParameter(":4", AppSettingsManager.SystemUser.UserCode);
                pSet.AddParameter(":5", AppSettingsManager.GetCurrentDate());
                if (pSet.ExecuteNonQuery() == 0)
                { }
            }

            m_blnSave = true;
            this.Close();
        }
    }
}