using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace Amellar.Common.Reports
{
    public partial class frmListofDeclaredOR : Form //JARS 20160621
    {
        public frmListofDeclaredOR()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            frmCompromiseAgreement dlg = new frmCompromiseAgreement();
            String sFromDate, sToDate;
            dlg.ReportTitle = "LIST OF DECLARED OR";
            if (cmbTeller.Text == "ALL")
            {
                dlg.User = "%%";
            }
            else
            {
                dlg.User = cmbTeller.Text;
            }
            sFromDate = dtpFrom.Value.ToString("dd-MMM-yy");
            sToDate = dtpTo.Value.ToString("dd-MMM-yy");
            dlg.FromDate = sFromDate;
            dlg.ToDate = sToDate;
            dlg.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListofDeclaredOR_Load(object sender, EventArgs e)
        {
            LoadTeller();
        }
        private void LoadTeller()
        {
            cmbTeller.Items.Clear();
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select teller from tellers order by teller";
            if (pSet.Execute())
            {
                cmbTeller.Items.Add("ALL");
                while (pSet.Read())
                {
                    cmbTeller.Items.Add(pSet.GetString(0));
                }
            }
            if (cmbTeller.Items.Count > 0)
                cmbTeller.SelectedIndex = 0;
            pSet.Close();
        }

    }
}