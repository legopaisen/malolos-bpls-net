using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.EPS
{
    public partial class frmZoningSeriesConfig : Form
    {
        public frmZoningSeriesConfig()
        {
            InitializeComponent();
        }

        private string m_sCurrentNumber = "";

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetSeries();
        }

        private void GetSeries()
        {
            OracleResultSet pSet = new OracleResultSet();
            int iTmp = 0;

            pSet.Query = "select * from zoning_control_series where year = '"+ AppSettingsManager.GetCurrentDate().ToString("yyyy") +"'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    iTmp = Convert.ToInt32(pSet.GetString("control_series"));
                }
            }
            pSet.Close();

            m_sCurrentNumber = iTmp.ToString();

            lblSeries.Text = iTmp.ToString("0000");
        }

        private void frmZoningSeriesConfig_Load(object sender, EventArgs e)
        {
            GetSeries();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            int iSeries = 0;

            if(btnEdit.Text == "Edit")
            {
                txtContorlSeries.Enabled = true;
                txtContorlSeries.Text = "";
                btnEdit.Text = "Save";
            }
            else
            {
                try
                {
                    iSeries = Convert.ToInt32(txtContorlSeries.Text);
                }
                catch
                {
                    MessageBox.Show("Control Series must be a number");
                    return;
                }

                if(iSeries <= Convert.ToInt32(m_sCurrentNumber))
                {
                    MessageBox.Show("NEW Control Number must not be lower than CURRENT control number, control number might be already used");
                    return;
                }

                pSet.Query = "update zoning_control_series set control_series = '" + iSeries.ToString() + "' where year = '" + AppSettingsManager.GetCurrentDate().ToString("yyyy") + "' ";
                pSet.ExecuteNonQuery();

                MessageBox.Show("Current Control Number is set to " + iSeries.ToString() + ", next generation will have a series of " + (iSeries + 1).ToString());

                btnEdit.Text = "Edit";
                GetSeries();

                txtContorlSeries.Text = "";
                txtContorlSeries.Enabled = false;
            }
        }
    }
}