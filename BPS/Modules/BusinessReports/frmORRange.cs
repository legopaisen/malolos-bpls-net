using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Amellar.Modules.BusinessReports
{
    public partial class frmORRange : Form
    {
        public frmORRange()
        {
            InitializeComponent();
        }
        private string m_sDatefrom = String.Empty;
        private string m_sDateto = String.Empty;

        public string Datefrom
        {
            get { return m_sDatefrom; }
            set { m_sDatefrom = value; }
        }
        public string Dateto
        {
            get { return m_sDateto; }
            set { m_sDateto = value; }
        }

        public string m_sTitle = String.Empty;

        private void frmORRange_Load(object sender, EventArgs e)
        {
            if (m_sTitle == "LIST")
                lblTitle.Text = "Enter Date Range";
            else
                lblTitle.Text = "Enter OR Range";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dtpFrom.Value > dtpTo.Value)
            {
                MessageBox.Show("Invalid Date.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            m_sDatefrom = string.Format("{0}/{1}/{2}", dtpFrom.Value.Month, dtpFrom.Value.Day, dtpFrom.Value.Year);
            m_sDateto = string.Format("{0}/{1}/{2}", dtpTo.Value.Month, dtpTo.Value.Day, dtpTo.Value.Year);

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}