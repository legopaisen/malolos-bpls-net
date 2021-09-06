using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BusinessRoll;
using Amellar.Modules.BusinessReports;

namespace BPLSBilling
{
    public partial class frmBussinessRollCategory : Form
    {
        public frmBussinessRollCategory()
        {
            InitializeComponent();
        }

        private void btnBnsRollList_Click(object sender, EventArgs e)
         {
            frmBusinessRoll frmBusinessRoll = new frmBusinessRoll();
            frmBusinessRoll.BusinessQueue = false;
            frmBusinessRoll.ShowDialog();
        }

        private void btnBnsRollSummary_Click(object sender, EventArgs e)
        {
            frmCompGross frmCompGross = new frmCompGross();
            frmCompGross.ShowDialog();
        }
    }
}