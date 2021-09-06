using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Amellar.Common.DataConnector;
using Amellar.Common.DataGridViewUtilities;

namespace Amellar.RPTA.Classes.Bank
{
    
    public partial class frmBankSearch : Form
    {
        bool m_selected = false;

        /*
         * Following variables were added by RDONG
         */
        private string m_strBankCode;
        private string m_strBankName;
        private string m_strBankBranch;
        private string m_sSearchMode;
        private string[] m_arrBankCodes;

        public string BankCode
        {
            get { return m_strBankCode; }
        }

        public string BankName
        {
            get { return m_strBankName; }
        }

        public string BankBranch
        {
            get { return m_strBankBranch; }
        }

        public frmBankSearch()
        {
            InitializeComponent();

            m_strBankCode = string.Empty;
            m_strBankName = string.Empty;
            m_strBankBranch = string.Empty;
        }

        public string[] BankCodesList
        {
            get { return m_arrBankCodes; }
            set { m_arrBankCodes = value; }
        }

        public string SearchMode //AFM 20200513
        {
            get { return m_sSearchMode; }
            set { m_sSearchMode = value; }
        }
	    

        private void tblBankSearch()
        {
            string strBankSearch = string.Empty;
            if(m_sSearchMode == "DUPLICATES") //AFM 20200513 for duplicates in searching chk no reference
            {
                strBankSearch = "select bank_code \"BANK CODE\", TRIM(bank_nm) \"BANK NAME\", TRIM(bank_branch) \"BANK BRANCH\" from bank_table  where bank_code = '" + m_arrBankCodes[0] + "'";
                foreach(string bankcode in m_arrBankCodes)
                    strBankSearch += " OR bank_code = '" + bankcode + "'";
            }  
            else
                strBankSearch = "select bank_code \"BANK CODE\", TRIM(bank_nm) \"BANK NAME\", TRIM(bank_branch) \"BANK BRANCH\" from bank_table order by bank_nm"; //--order by bank_code";

             DataGridView tblGridView = new DataGridView();

             OracleResultSet result = new OracleResultSet();
             DataGridViewOracleResultSet dsTaxRates = new DataGridViewOracleResultSet(tblGridView, strBankSearch); //, DataConnectorManager.Instance.Connection);
             grdBankList.DataSource = tblGridView.DataSource;
             grdBankList.DataMember = tblGridView.DataMember;

             DataGridViewUtilities.HideNewRows(grdBankList);

             grdBankList.Refresh();
             txtBankNames.Focus();
        }

        private void tblSearchName()
        {
            string strBankNames = txtBankNames.Text.ToUpper().Trim();
            string strBankSearch = "select bank_code \"BANK CODE\", TRIM(bank_nm) \"BANK NAME\", TRIM(bank_branch) \"BANK BRANCH\" from bank_table  where bank_nm like" + "'" + strBankNames + "%" + "'";
            DataGridView tblGridView = new DataGridView();
            DataGridViewOracleResultSet BankSearch = new DataGridViewOracleResultSet(tblGridView, strBankSearch);
            grdBankList.DataSource = tblGridView.DataSource;
            grdBankList.DataMember = tblGridView.DataMember;
            
            grdBankList.Refresh();
            txtBankNames.Focus();
        }

        private void frmBankSearch_Load(object sender, EventArgs e)
        {
            /*
            System.Drawing.Drawing2D.GraphicsPath paths = new System.Drawing.Drawing2D.GraphicsPath();
            DesignDLL design = new DesignDLL(out paths, this.Width, this.Height);
            this.Region = new Region(paths);
            */

            tblBankSearch();
        }

        private void grdBankList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            m_selected = true;


            //grdTaxRates.SelectedCells[0].RowIndex.

            //string strBankCode = grdBankList.CurrentRow.Cells[0].Value.ToString().Trim();
            string strBankCode = grdBankList.SelectedCells[0].Value.ToString().Trim();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select bank_addr from bank_table where trim(bank_code) = :1";

            result.AddParameter(":1", strBankCode);

            if (!result.Execute())
            {
                MessageBox.Show(result.ErrorDescription);
            }
            else
            {

                while (result.Read())
                {

                    grdAddr.Rows[0].Cells[0].Value = result.GetString(0).ToString();
                    //                   grdAddr.Rows.Remove(grdAddr.Rows[0]);

                }

            } 
        }

        private void txtAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 20 || e.KeyChar > 122) &&
                 e.KeyChar != 8 && e.KeyChar != 20)
            {
                e.Handled = true;
            }

          

            else
            {
                m_selected = false;

                tblSearchName();
              
                grdAddr.Rows[0].Cells[0].Value = "";
                grdBankList.Refresh();
                grdAddr.Refresh(); 
              
            }
        
        
        }

        //added by RDO
        public static bool GetBankInformation(string strBankCode, out string strBankName, out string strBankBranch)
        {
            bool blnHasInfo = false;
            strBankName = string.Empty;
            strBankBranch = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "SELECT bank_nm, bank_branch FROM bank_table WHERE bank_code = RPAD(:1, 3)";//MOD MCR 20140723 bank_addr
            result.AddParameter(":1", strBankCode);
            if (result.Execute())
            {
                if (result.Read())
                {
                    strBankName = result.GetString(0).Trim();
                    strBankBranch = result.GetString(1).Trim();
                    blnHasInfo = true;
                }
            }
            result.Close();

            return blnHasInfo;
        }
     
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (m_selected)
            {
                grdBankList.Refresh();
                string strBankCode = grdBankList.SelectedCells[0].Value.ToString().Trim();
                //added by RDO
                if (frmBankSearch.GetBankInformation(strBankCode, out m_strBankName, out m_strBankBranch))
                {
                    m_strBankCode = strBankCode;
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("Unknown error.");
                }
                /*
                OracleResultSet result = new OracleResultSet();
                result.Query = "select bank_addr, bank_nm, bank_Branch from bank_tbl where trim(bank_code) = :1";

                result.AddParameter(":1", strBankCode);

                if (!result.Execute())
                {
                    MessageBox.Show(result.ErrorDescription);
                }
                else
                {
                    //while (result.Read())
                    if (result.Read())
                    {
                        m_strBankCode = strBankCode; //result.GetString(0).Trim();
                        m_strBankName = result.GetString(1).Trim();
                        m_strBankBranch = result.GetString(2).Trim();
                        this.Dispose();
                        
                        //frmPaymentCheck PaymentCheck = new frmPaymentCheck(result.GetString(0).Trim(), result.GetString(1).Trim(), result.GetString(2).Trim());
                        //PaymentCheck.Show();
                        //this.Dispose(); 
                    }
                }
                */

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //MCR 20140723
        private void grdBankList_SelectionChanged(object sender, EventArgs e)
        {
            m_selected = true;
            //string strBankCode = grdBankList.CurrentRow.Cells[0].Value.ToString().Trim();
            string strBankCode = "";
            // RMC 20150707 corrections in payment module (s)
            try
            {
                strBankCode = grdBankList.CurrentRow.Cells[0].Value.ToString().Trim();
            }
            catch {
                strBankCode = "";

            }
            // RMC 20150707 corrections in payment module (e)

        }
             
    }
}