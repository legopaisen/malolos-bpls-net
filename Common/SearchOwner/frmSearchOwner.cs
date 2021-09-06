///////////////////////

// RMC 20111201 Note: changed Search Owner grid edit mode to EditProgrammatically
// RMC 20111128 disable display of ownersearch dialog if function called from business records txtOwnLn_Leave when no record found  
// RMC 20111128 display owner's address in SearchOwner
// RMC 20111011 added pop-up of same own names upon entry of last name (user-request) 
///////////////////////


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Common.SearchOwner
{
    public partial class frmSearchOwner : Form
    {
        public string m_sOwnLn = string.Empty, m_sOwnFn = string.Empty, m_sOwnMi = string.Empty;
        public string m_sOwnAdd = string.Empty, m_sOwnStreet = string.Empty, m_sOwnBrgy = string.Empty;
        public string m_sOwnZone = string.Empty, m_sOwnDist = string.Empty, m_sOwnMun = string.Empty, m_sOwnProv = string.Empty;
        public string m_strOwnCode = string.Empty;
        public string m_sOwnZip = string.Empty;     // RMC 20110414
        public string m_sPageWatch = string.Empty;  // RMC 20110414
        public bool m_bNoRecordFound = false; // RMC 20111128 disable display of ownersearch dialog if function called from business records txtOwnLn_Leave when no record found  

        public string OwnLastName
        {
            get { return m_sOwnLn; }
            set { m_sOwnLn = value; }
        }

        public frmSearchOwner()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // RMC 20111011 added pop-up of same own names upon entry of last name (user-request) (s)
            if (m_sOwnLn != "")
            {
                txtLn.Text = m_sOwnLn;
                GetValues();
            }
            // RMC 20111011 added pop-up of same own names upon entry of last name (user-request) (e)
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetValues();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            m_strOwnCode = "";  // RMC 20111011 added pop-up of same own names upon entry of last name (user-request)
            
            this.Close();
        }

        private void dgvOwnNames_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            m_strOwnCode = string.Empty;    // RMC 20110809
            m_sOwnZip = string.Empty;   // RMC 20110809
            txtLn.Text = string.Empty;
            txtFn.Text = string.Empty;
            txtMi.Text = string.Empty;
            txtAdd.Text = string.Empty;
            txtOwnCode.Text = string.Empty; // RMC 20111004 added owncode control in Owner search Engine
            
            txtLn.Text = dgvOwnNames.SelectedRows[0].Cells[0].Value.ToString();
            txtFn.Text = dgvOwnNames.SelectedRows[0].Cells[1].Value.ToString();
            txtMi.Text = dgvOwnNames.SelectedRows[0].Cells[2].Value.ToString();
            m_strOwnCode = dgvOwnNames.SelectedRows[0].Cells[10].Value.ToString();   // RMC 20110311
            m_sOwnZip = dgvOwnNames.SelectedRows[0].Cells[11].Value.ToString();   // RMC 20110311
            txtOwnCode.Text = m_strOwnCode; // RMC 20111004 added owncode control in Owner search Engine
            txtAdd.Text = AppSettingsManager.GetBnsOwnAdd(m_strOwnCode);    // RMC 20111128 display owner's address in SearchOwner
        }

        //private void GetValues()
        public void GetValues() // RMC 20111011 added pop-up of same own names upon entry of last name (user-request)
        {
            string sLn, sFn, sMi, sAdd;
            dgvOwnNames.Rows.Clear();
            OracleResultSet pSet = new OracleResultSet();

            // RMC 20111128 disable display of ownersearch dialog if function called from business records txtOwnLn_Leave when no record found(s)
            if (m_sOwnLn != "")
            {
                m_bNoRecordFound = false;
                txtLn.Text = m_sOwnLn;
            }
            // RMC 20111128 disable display of ownersearch dialog if function called from business records txtOwnLn_Leave when no record found(e)


            if (txtLn.Text.Trim() == "")
                sLn = "%";
            else
                sLn = txtLn.Text.Trim();

            if (txtFn.Text.Trim() == "")
                sFn = "%";
            else
                sFn = txtFn.Text.Trim();

            if (txtMi.Text.Trim() == "")
                sMi = "%";
            else
                sMi = txtMi.Text.Trim();

            // RMC 20110414 (s)
            if (m_sPageWatch == "PAGE2")
            {
                pSet.Query = "select * from own_names where own_ln like '" + StringUtilities.StringUtilities.HandleApostrophe(sLn) + "' ";  // RMC 20111014 added handleapostrophe in search owner
                pSet.Query += "and own_fn like '" + StringUtilities.StringUtilities.HandleApostrophe(sFn) + "' and own_mi like '" + sMi + "' "; // RMC 20111014 added handleapostrophe in search owner
            }
            else
            {
                pSet.Query = "select * from own_names ";
                pSet.Query += "where (own_code in (select own_code from businesses) ";
                pSet.Query += "or own_code in (select own_code from business_que)) ";
                pSet.Query += "and trim(own_ln) like '" + StringUtilities.StringUtilities.HandleApostrophe(sLn) + "' "; // RMC 20111014 added handleapostrophe in search owner
                pSet.Query += "and nvl(trim(own_fn),' ') like '" + StringUtilities.StringUtilities.HandleApostrophe(sFn) + "' ";  // RMC 20111014 added handleapostrophe in search owner
                pSet.Query += "and nvl(trim(own_mi),' ') like '" + sMi + "' ";
                pSet.Query += "and own_code = own_code ";
            }

            // RMC 20111012 enabled own code in search owner (s)
            if (txtOwnCode.Text.Trim() != "")
                pSet.Query += " and own_code = '" + txtOwnCode.Text.Trim() + "'";
            // RMC 20111012 enabled own code in search owner (e)
           
            pSet.Query += "order by own_ln asc";
            // RMC 20110414 (e)
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();   // RMC 20110414

                    if (pSet.Execute()) // RMC 20110414
                    {
                        while (pSet.Read())
                        {
                            dgvOwnNames.Rows.Add(pSet.GetString("own_ln").Trim(), pSet.GetString("own_fn").Trim(),
                                pSet.GetString("own_mi").Trim(), pSet.GetString("own_house_no").Trim(),
                                pSet.GetString("own_street").Trim(), pSet.GetString("own_brgy").Trim(),
                                pSet.GetString("own_zone").Trim(), pSet.GetString("own_dist").Trim(),
                                pSet.GetString("own_mun").Trim(), pSet.GetString("own_prov").Trim(),
                                pSet.GetString("own_code").Trim(), pSet.GetString("own_zip").Trim());   // RMC 20110414
                        }
                    }
                    pSet.Close();
                }
                else
                {
                    // RMC 20111128 disable display of ownersearch dialog if function called from business records txtOwnLn_Leave when no record found(s)
                    if (m_sOwnLn != "")
                    {
                        m_bNoRecordFound = true;

                    }// RMC 20111128 disable display of ownersearch dialog if function called from business records txtOwnLn_Leave when no record found(e)
                    else
                    {
                        MessageBox.Show("No record found.", "Search Owner", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLn.Text = string.Empty;
            txtFn.Text = string.Empty;
            txtMi.Text = string.Empty;
            txtAdd.Text = string.Empty;
            txtOwnCode.Text = string.Empty; // RMC 20111004 added owncode control in Owner search Engine
            dgvOwnNames.Rows.Clear();
            txtLn.Focus();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

        }

        private void btnOk1_Click(object sender, EventArgs e)
        {
            try
            {
                m_sOwnLn = dgvOwnNames.SelectedRows[0].Cells[0].Value.ToString();
                m_sOwnFn = dgvOwnNames.SelectedRows[0].Cells[1].Value.ToString();
                m_sOwnMi = dgvOwnNames.SelectedRows[0].Cells[2].Value.ToString();
                m_sOwnAdd = dgvOwnNames.SelectedRows[0].Cells[3].Value.ToString();
                m_sOwnStreet = dgvOwnNames.SelectedRows[0].Cells[4].Value.ToString();
                m_sOwnBrgy = dgvOwnNames.SelectedRows[0].Cells[5].Value.ToString();
                m_sOwnZone = dgvOwnNames.SelectedRows[0].Cells[6].Value.ToString();
                m_sOwnDist = dgvOwnNames.SelectedRows[0].Cells[7].Value.ToString();
                m_sOwnMun = dgvOwnNames.SelectedRows[0].Cells[8].Value.ToString();
                m_sOwnProv = dgvOwnNames.SelectedRows[0].Cells[9].Value.ToString();
                m_sOwnZip = dgvOwnNames.SelectedRows[0].Cells[11].Value.ToString();     // RMC 20110414
                m_strOwnCode = dgvOwnNames.SelectedRows[0].Cells[10].Value.ToString();  // RMC 20111011

                ClearFields();    

                this.Close();
            }
            catch
            {
            }

        }
        private void ClearFields()
        {
            txtLn.Text = string.Empty;
            txtFn.Text = string.Empty;
            txtMi.Text = string.Empty;
            txtAdd.Text = string.Empty;
            txtOwnCode.Text = string.Empty; // RMC 20111004 added owncode control in Owner search Engine
            dgvOwnNames.Rows.Clear();

            
        }

        private void dgvOwnNames_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvOwnNames_CellContentClick(sender, e);

            // RMC 20111201 Note: changed Search Owner grid edit mode to EditProgrammatically
        }

    }
}