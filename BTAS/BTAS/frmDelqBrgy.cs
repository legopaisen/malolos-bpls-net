using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.Reports;
namespace BTAS
{
    public partial class frmDelqBrgy : Form
    {
        ReportClass rClass = new ReportClass();
        OracleResultSet result = new OracleResultSet();
        public frmDelqBrgy()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDelqBrgy_Load(object sender, EventArgs e)
        {
            LoadBrgy();
        }

        private void LoadBrgy()
        {
            cmbBrgy.Items.Clear();
            result.Query = "select * from brgy order by brgy_nm";
            if (result.Execute())
            {
                cmbBrgy.Items.Add("ALL");
                while (result.Read())
                    cmbBrgy.Items.Add(result.GetString("brgy_nm").Trim());
            }
            result.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            //rClass.NoticeDelq(cmbBrgy.Text.Trim());
            rClass.NoticeDelqNew(cmbBrgy.Text.Trim()); // CJC 20140506
        }
    }
}