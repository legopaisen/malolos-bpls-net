

// RMC 20120125 corrected cancel of double business name warning


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace Amellar.Common.frmBns_Rec
{
    public partial class frmDouble_Buss : Form
    {
        public string m_sAdd = string.Empty;
        public string m_sStreet = string.Empty;
        public string m_sBrgy = string.Empty;
        public string m_sQuery = string.Empty;

        public frmDouble_Buss()
        {
            InitializeComponent();
        }

        private void frmDouble_Buss_Load(object sender, EventArgs e)
        {
            // RMC 20110923 (s)
            OracleResultSet pRec = new OracleResultSet();
            int iCnt = 0;

            pRec.Query = string.Format("select count(*) from businesses a, buss_plate b where a.bin = b.bin and bns_nm = '{0}'", StringUtilities.StringUtilities.HandleApostrophe(txtBussName.Text.Trim()));
            int.TryParse(pRec.ExecuteScalar(), out iCnt);
            if (iCnt > 0)
            {
                m_sQuery = string.Format("select bns_house_no \" Address #\", bns_street \" Street\", bns_brgy \" Barangay\", bns_mun \" Municipal/City\", bns_prov \" Province\", bns_plate \" Business Plate\" "
                   + " from businesses a, buss_plate b where bns_nm = '{0}' and a.bin = b.bin", StringUtilities.StringUtilities.HandleApostrophe(txtBussName.Text.Trim()));
                
            }      
            else
            {
                m_sQuery = string.Format("select bns_house_no \" Address #\", bns_street \" Street\", bns_brgy \" Barangay\", bns_mun \" Municipal/City\", bns_prov \" Province\", ' '\" Business Plate\" "
                   + " from businesses where bns_nm = '{0}'", StringUtilities.StringUtilities.HandleApostrophe(txtBussName.Text.Trim()));
            }
            // RMC 20110923 (e)

            DataGridViewOracleResultSet dsUser = new DataGridViewOracleResultSet(dgvList, m_sQuery, 0, 0);
            dgvList.RowHeadersVisible = false;
            dgvList.Refresh();

            LoadList(0);
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //transferred code to LoadList()
            LoadList(e.RowIndex);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_sAdd = "";
            m_sStreet = "";
            m_sBrgy = "";
            txtBussPlate.Text = ""; // RMC 20110923
            txtBussAdd.Text = "";   // RMC 20120125 corrected cancel of double business name warning
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadList(int iRow)
        {
            // RMC 20110923
            try
            {
                m_sAdd = dgvList[0, iRow].Value.ToString().Trim();
                m_sStreet = dgvList[1, iRow].Value.ToString().Trim();
                m_sBrgy = dgvList[2, iRow].Value.ToString().Trim();
                txtBussAdd.Text = m_sAdd + " " + m_sStreet + " " + m_sBrgy + " ";
                txtBussAdd.Text += dgvList[3, iRow].Value.ToString().Trim() + " ";
                txtBussAdd.Text += dgvList[4, iRow].Value.ToString().Trim();
                txtBussPlate.Text = dgvList[5, iRow].Value.ToString();    // RMC 20110923
            }
            catch
            {
                m_sAdd = "";
                m_sStreet = "";
                m_sBrgy = "";
                txtBussAdd.Text = "";
                txtBussPlate.Text = "";
            }
        }
    }
}