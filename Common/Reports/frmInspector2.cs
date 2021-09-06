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
    public partial class frmInspector2 : Form
    {
        public frmInspector2()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadInspector()
        {
            cmbInspector.Items.Clear();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from INSPECTOR order by INSPECTOR_CODE";
            if (result.Execute())
            {
                while (result.Read())
                    cmbInspector.Items.Add(result.GetString("INSPECTOR_CODE").Trim());
            }
            result.Close();
        }

        private void frmInspector_Load(object sender, EventArgs e)
        {
            LoadInspector();
        }
    }
}