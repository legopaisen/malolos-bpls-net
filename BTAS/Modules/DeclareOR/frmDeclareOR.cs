using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.BusinessType;
using Amellar.Common.AppSettings;
namespace DeclareOR
{
    public partial class frmDeclareOR : Form
    {
        OracleResultSet result = new OracleResultSet();
        DeclareORClass ORClass;
        DateTime dtSystemDate = DateTime.Now;
        bool bWatch = false;
        int iOrFrom = 0;
        int iOrTo = 0;

        public frmDeclareOR()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if(btnClose.Text == "Close")
                this.Close();
            else
            {
                CleanMe();
            }
        }

        private void frmDeclareOR_Load(object sender, EventArgs e)
        {
            LoadAssignedOR();
        }
        private void LoadAssignedOR()
        {
            dtSystemDate = AppSettingsManager.GetSystemDate();
            dgvOR.Rows.Clear();
            result.Query = "select * from or_assigned order by trn_date desc";
            if (result.Execute())
            {
                while (result.Read())
                {
                    dgvOR.Rows.Add(result.GetString("teller").Trim(), result.GetInt("from_or_no").ToString(), result.GetInt("to_or_no").ToString(), result.GetDateTime("trn_date").ToShortDateString(), result.GetString("assigned_by").Trim());
                }
            }
            result.Close();
        }
        private void CleanMe()
        {
            txtTellerCode.Text = string.Empty;
            txtFn.Text = string.Empty;
            txtMi.Text = string.Empty;
            txtLn.Text = string.Empty;
            txtFrom.Text = string.Empty;
            txtTo.Text = string.Empty;
            txtCurrOr.Text = string.Empty;
            txtTellerCode.Focus();
            btnReturn.Enabled = false;
            btnDeclare.Enabled = false;
            btnSearch.Text = "Search";
            btnClose.Text = "Close";
        }
        private void SearchTeller()
        {
            if (txtTellerCode.Text.Trim() != string.Empty)
            {
                if (btnSearch.Text == "Search")
                {
                    ORClass = new DeclareORClass();
                    ORClass.LoadTeller(txtTellerCode.Text.Trim());
                    if (ORClass.bTellerFound)
                    {
                        txtLn.Text = ORClass.m_sTellerLn;
                        txtFn.Text = ORClass.m_sTellerFn;
                        txtMi.Text = ORClass.m_sTellerMi;
                        if (ORClass.bFlag)
                        {
                            txtFrom.Text = ORClass.m_sFromORNo;
                            txtTo.Text = ORClass.m_sToORNo;
                            txtCurrOr.Text = ORClass.m_sCurrORNo;
                            btnDeclare.Enabled = false;
                            btnReturn.Enabled = true;
                        }
                        else
                        {
                            txtFrom.Text = string.Empty;
                            txtTo.Text = string.Empty;
                            txtCurrOr.Text = string.Empty;
                            btnDeclare.Enabled = true;
                            btnReturn.Enabled = false;
                        }
                        btnSearch.Text = "Clear";
                        btnClose.Text = "Cancel";
                        dgvOR.ReadOnly = true;
                    }
                    else
                        CleanMe();
                }
                else
                {
                    CleanMe();
                    LoadAssignedOR();
                    btnSearch.Text = "Search";
                    btnClose.Text = "Close";
                    dgvOR.ReadOnly = false;
                }
            }
            else
            {
                MessageBox.Show("Please enter teller code.");
                txtTellerCode.Focus();
                return;
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchTeller();
        }

        private void dgvOR_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnSearch.Text == "Search")
            {
                ORClass = new DeclareORClass();
                ORClass.LoadTeller(dgvOR.SelectedRows[0].Cells[0].Value.ToString().Trim());
                txtLn.Text = ORClass.m_sTellerLn;
                txtFn.Text = ORClass.m_sTellerFn;
                txtMi.Text = ORClass.m_sTellerMi;
                if (ORClass.bFlag)
                {
                    txtTellerCode.Text = dgvOR.SelectedRows[0].Cells[0].Value.ToString().Trim();
                    txtFrom.Text = ORClass.m_sFromORNo;
                    txtTo.Text = ORClass.m_sToORNo;
                    txtCurrOr.Text = ORClass.m_sCurrORNo;
                    btnDeclare.Enabled = false;
                    btnReturn.Enabled = true;
                }
                else
                {
                    txtFrom.Text = string.Empty;
                    txtTo.Text = string.Empty;
                    txtCurrOr.Text = string.Empty;
                    btnDeclare.Enabled = true;
                    btnReturn.Enabled = false;
                }
                btnSearch.Text = "Clear";
                btnClose.Text = "Cancel";
                dgvOR.ReadOnly = true;
            }
            else
            {
                CleanMe();
                LoadAssignedOR();
                btnSearch.Text = "Search";
                btnClose.Text = "Close";
            }
        }


        private bool Validations()
        {
            
            int.TryParse(txtFrom.Text.Trim(), out iOrFrom);
            int.TryParse(txtTo.Text.Trim(), out iOrTo);

            if (txtFrom.Text.Trim() == string.Empty)
                bWatch = true;
            else
                bWatch = false;
            if (txtTo.Text.Trim() == string.Empty)
                bWatch = true;
            else
                bWatch = false;
            if (iOrFrom > iOrTo)
                bWatch = true;
            else
                bWatch = false;
            if (iOrFrom == 0)
                bWatch = true;
            else
                bWatch = false;
            if (iOrTo == 0)
                bWatch = true;
            else
                bWatch = false;

            return bWatch;
        }

 
        private void btnDeclare_Click(object sender, EventArgs e)
        {
            
            if(Validations())
            {
                MessageBox.Show("Invalid OR Range.");
                return;
            }
            for(int i = iOrFrom; i <= iOrTo; i++)
            {
                // GDE 20101126 add trn_year
                result.Query = "select * from or_used where or_no = :1";
                result.AddParameter(":1", i);
                if(result.Execute())
                {
                    if(result.Read())
                    {
                        MessageBox.Show("Conflict with OR Range");
                        txtFrom.Focus();
                        return;
                    }
                }
                result.Close();
            }

            for (int i = iOrFrom; i <= iOrTo; i++)
            {
                // GDE 20101126 add trn_year
                result.Query = "select * from or_assigned where from_or_no = :1 and to_or_no = :2";
                result.AddParameter(":1", i);
                result.AddParameter(":2", i);
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        MessageBox.Show("OR is currently assigned to another teller");
                        txtFrom.Focus();
                        return;
                    }
                }
                result.Close();
            }

            // Saving to or_assigned (s){
            
            result.Query = "insert into or_assigned values (:1, :2, :3, :4, :5, '')";
            result.AddParameter(":1", txtTellerCode.Text.Trim());
            result.AddParameter(":2", txtFrom.Text.Trim());
            result.AddParameter(":3", txtTo.Text.Trim());
            result.AddParameter(":4", dtSystemDate);
            result.AddParameter(":5", AppSettingsManager.SystemUser.UserCode);
            if(result.Execute())
            {
                if(result.Read())
                {

                }
            }
            result.Close();
            // Saving to or_assigned (e)}

            // Saving to or_current (s){
            result.Query = "insert into or_current values (:1, :2, :3, :4, '')";
            result.AddParameter(":1", txtTellerCode.Text.Trim());
            result.AddParameter(":2", txtFrom.Text.Trim());
            result.AddParameter(":3", txtTo.Text.Trim());
            result.AddParameter(":4", txtFrom.Text.Trim());
            if (result.Execute())
            {
                if (result.Read())
                {

                }
            }
            result.Close();
            // Saving to or_current (e)}

            txtCurrOr.Text = txtFrom.Text.Trim();
            LoadAssignedOR();
        }
    }
}