

// RMC 20111221 added Btax Monitoring module


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace Amellar.BPLS.TreasurersModule
{
    public partial class frmMemoranda : Form
    {
        public string m_sQuery = "";
        public string m_sMemo = "";
        public string m_sBIN = "";  // RMC 20120103 added capturing of memoranda in approving btax
        public string m_sTaxYear = "";  // RMC 20120103 added capturing of memoranda in approving btax

        public frmMemoranda()
        {
            InitializeComponent();
        }

        private void frmMemoranda_Load(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            // NOTE: select statement must select only one field

            pSet.Query = m_sQuery.ToString();
	        if(pSet.Execute())
            {
                if(pSet.Read())
	                txtMemo.Text = pSet.GetString(0);
            }
            pSet.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtMemo.Text.Trim() == "")
            {
                MessageBox.Show("Memoranda is required.", "BPLS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                m_sMemo = txtMemo.Text.Trim();
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Cancel transaction?", "Memoranda", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
            
        }
	
    }
}