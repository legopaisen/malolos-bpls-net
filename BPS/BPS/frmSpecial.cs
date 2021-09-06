using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.Reports;
namespace BPLSBilling
{
    public partial class frmSpecial : Form
    {
        public frmSpecial()
        {
            InitializeComponent();
        }

        private void frmSpecial_Load(object sender, EventArgs e)
        {
            LoadFees();
        }

        private void LoadFees()
        {
            cmbFeesDesc.Items.Clear();
            OracleResultSet result = new OracleResultSet();
            //result.Query = "select * from tax_and_fees_table where fees_type = 'AD' order by fees_code";
            result.Query = "select * from tax_and_fees_table where fees_type = 'AD' and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "' order by fees_code"; //MCR 20141118
            
            if (result.Execute())
            {
                cmbFeesDesc.Items.Add("ALL");
                while (result.Read())
                {
                    cmbFeesDesc.Items.Add(result.GetString("fees_desc").Trim());           
                }
            }
            result.Close();
        }
        

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            ReportClass rClass = new ReportClass();
            rClass.Special(txtBnsCode.Text.Trim(), cmbFeesDesc.Text.Trim());
            rClass.PreviewDocu();
        }
    }
}