using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.OwnerProfile
{
    public partial class frmOwnerProfile : Form
    {
        private string m_sOwnCode = string.Empty;
        private string m_sTempDate = string.Empty;

        public string OwnCode
        {
            get { return m_sOwnCode; }
            set { m_sOwnCode = value; }
        }

        public frmOwnerProfile()
        {
            InitializeComponent();
        }

        private void frmOwnerProfile_Load(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            txtOwnCode.Text = m_sOwnCode;

            pSet.Query = "select * from own_profile where own_code = '" + m_sOwnCode + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    //txtOwnCode.Text = pSet.GetString("own_code");
                    txtNationality.Text = pSet.GetString("citizenship");
                    txtTelNo.Text = pSet.GetString("tel_no");
                    txtEmailAdd.Text = pSet.GetString("email_add");
                    cmbGender.Text = pSet.GetString("gender");
                    txtSpouse.Text = pSet.GetString("spouse");
                    txtMiddleName.Text = pSet.GetString("middle_name"); //JARS 20170904
                    if (pSet.GetString("bdate") == "")
                        dtpBDate.Text = "01/01/1899";
                    else
                        dtpBDate.Text = pSet.GetString("bdate");
                }
            }
            pSet.Close();

            txtNationality.Focus();
            this.ActiveControl = txtNationality;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OracleResultSet pCmd = new OracleResultSet();
            if (dtpBDate.Text == "01/01/1899")
                m_sTempDate = "";
            else
                m_sTempDate = dtpBDate.Text;

            if (m_sOwnCode.Trim() == "")
            {
                //m_sOwnCode = AppSettingsManager.SystemUser.UserCode + "1";  //temporary
                //MCR 20141222 (s)
                MessageBox.Show("Owner is not yet enlisted!", "Owner Profile", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
                //MCR 20141222 (e)
            }

            pCmd.Query = "delete from own_profile where own_code = '" + m_sOwnCode + "'";
            if (pCmd.ExecuteNonQuery() == 0)
            { }

            pCmd.Query = "insert into own_profile (own_code, citizenship,tel_no,email_add, gender, bdate, spouse, middle_name) values (";
            pCmd.Query += "'" + m_sOwnCode + "', ";
            pCmd.Query += "'" + StringUtilities.HandleApostrophe(txtNationality.Text) + "', ";
            pCmd.Query += "'" + StringUtilities.HandleApostrophe(txtTelNo.Text) + "', ";
            pCmd.Query += "'" + StringUtilities.HandleApostrophe(txtEmailAdd.Text) + "', ";
            pCmd.Query += "'" + StringUtilities.HandleApostrophe(cmbGender.Text) + "', ";
            pCmd.Query += "'" + StringUtilities.HandleApostrophe(m_sTempDate) + "', ";
            pCmd.Query += "'" + StringUtilities.HandleApostrophe(txtSpouse.Text) + "', ";
            pCmd.Query += "'" + StringUtilities.HandleApostrophe(txtMiddleName.Text) + "')"; //JARS 20170904
            if (pCmd.ExecuteNonQuery() == 0)
            { }
            
            MessageBox.Show("Owner profile saved", "Owner Profile", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNationality_TextChanged(object sender, EventArgs e) //JARS 20170808 AUTO COMPLETE FOR THE WORD 'FILIPINO'
        {
            txtNationality.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtNationality.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection col = new AutoCompleteStringCollection();
            col.Add("FILIPINO");
            txtNationality.AutoCompleteCustomSource = col;
        }
    }
}