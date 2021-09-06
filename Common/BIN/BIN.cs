using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
//using Amellar.Common.BIN;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;


namespace BIN
{
    public partial class BIN : UserControl
    {
        public string m_sLguCode, m_sDistCode, m_sTaxYear, m_sBINSeries, m_sBIN;
        OracleResultSet result = new OracleResultSet();

        public BIN()
        {
            InitializeComponent();
            txtLGUCode.Enabled = false;
            txtDistCode.Enabled = false;
        }
        public string GetLGUCode
        {
            get { return txtLGUCode.Text; }
            set { txtLGUCode.Text = value; }
        }
        public string GetDistCode
        {
            get { return txtDistCode.Text; }
            set { txtDistCode.Text = value; }
        }
        public string GetTaxYear
        {
            get { return txtTaxYear.Text; }
            set { txtTaxYear.Text = value; }
        }
        public string GetBINSeries
        {
            get { return txtBINSeries.Text; }
            set { txtBINSeries.Text = value; }
        }

        public string GetBin()
        {
            m_sBIN = txtLGUCode.Text + "-" + txtDistCode.Text + "-" + txtTaxYear.Text + "-" + txtBINSeries.Text;
            return m_sBIN;
        }

        private void txtTaxYear_TextChanged(object sender, EventArgs e)
        {
            if (txtTaxYear.Text.Trim().Length == 4)
                txtBINSeries.Focus();
        }

        private void txtBINSeries_Leave(object sender, EventArgs e)
        {
            int iCount = 0;
            iCount = txtBINSeries.TextLength;

            switch (iCount)
            {
                case 1:
                    {
                        txtBINSeries.Text = "000000" + txtBINSeries.Text;
                        break;
                    }
                case 2:
                    {
                        txtBINSeries.Text = "00000" + txtBINSeries.Text;
                        break;
                    }
                case 3:
                    {
                        txtBINSeries.Text = "0000" + txtBINSeries.Text;
                        break;
                    }
                case 4:
                    {
                        txtBINSeries.Text = "000" + txtBINSeries.Text;
                        break;
                    }
                case 5:
                    {
                        txtBINSeries.Text = "00" + txtBINSeries.Text;
                        break;
                    }
                case 6:
                    {
                        txtBINSeries.Text = "0" + txtBINSeries.Text;
                        break;
                    }
                case 7:
                    {
                        txtBINSeries.Text = txtBINSeries.Text;
                        break;
                    }
            }
        }

        
        
        private void BIN_Load(object sender, EventArgs e)
        {
            txtLGUCode.Text = AppSettingsManager.GetConfigObject("10");
            txtDistCode.Text = AppSettingsManager.GetConfigObject("11");
            
            txtTaxYear.Focus();
        }

        

        
        
        
    }
}
