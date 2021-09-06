using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace Amellar.BPLS.TreasurersModule
{
    public partial class frmForReBilling : Form
    {
        private string m_sUserCode, m_sBIN;

        public string UserCode { set { m_sUserCode = value; } }
        public string BIN 
        {
            get { return m_sBIN; }
            set { m_sBIN = value; } 
        }
        
        public frmForReBilling()
        {
           InitializeComponent();
        }

        private void frmForReBilling_Load(object sender, EventArgs e)
        {
            PopDataGrid();
        }
        private void PopDataGrid()
        {
            string sQueryGrid = "select bin \"BIN\", parameter \"Parameter\", prev_tax \"Prev Tax Paid\", curr_tax \"Curr Tax Due\", tax_year \"Tax Year\", memoranda \"Memoranda\" from treasurers_module where action = '1' and user_code =  '"+m_sUserCode+"' order by dt_save desc";
            DataGridViewOracleResultSet view = new DataGridViewOracleResultSet(dgvForReBilling, sQueryGrid, 0, 0);
            dgvForReBilling.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvForReBilling.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvForReBilling.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvForReBilling.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvForReBilling.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvForReBilling.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvForReBilling.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvForReBilling.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvForReBilling.Columns[2].DefaultCellStyle.Format = "#,##0.00";
            dgvForReBilling.Columns[3].DefaultCellStyle.Format = "#,##0.00";
            dgvForReBilling.Columns[5].Visible = false; // memoranda
            dgvForReBilling.Refresh();
            // initial value
            txtMemoranda.Text = dgvForReBilling.Rows[0].Cells[5].Value.ToString().Trim();
            m_sBIN = dgvForReBilling.Rows[0].Cells[0].Value.ToString().Trim();
            
            
        }

        public bool CheckRecordForReBill()
        {
            OracleResultSet pSet = new OracleResultSet();
            bool bResult = false;
            string sUserCode, sMemoranda;
            pSet.Query = "SELECT * FROM treasurers_module WHERE action = '1' AND bin =  :1";
            pSet.AddParameter(":1", m_sBIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sUserCode = pSet.GetString("user_code").Trim();
                    if (MessageBox.Show("This record is for re-billing care of " + sUserCode + ".\n Continue?", "Billing", MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.Yes)
                    {
                        sMemoranda = pSet.GetString("memoranda").Trim();
                        MessageBox.Show("Memoranda: "+sMemoranda, "Billing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        bResult = true;
                    }
                    else
                    {
                        bResult = false;
                        m_sBIN = string.Empty;
                    }
                }
            }
            pSet.Close();
            return bResult;
        }

        public bool WithPendingBilling()
        {
            OracleResultSet pSet = new OracleResultSet();
            bool bResult = false;
            pSet.Query = "SELECT * FROM treasurers_module WHERE action = '1' AND user_code =  :1";
            pSet.AddParameter(":1", m_sUserCode);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    MessageBox.Show("Warning! You have pending record/s for re-billing.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    bResult = true;
                }
                else
                    bResult = false;
            }
            pSet.Close();
            return bResult;
        }

        private void dgvForReBilling_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //txtMemoranda.Text = dgvForReBilling.SelectedRows[e.RowIndex].Cells[5].Value.ToString();
            if (e.RowIndex != -1)
            {
                txtMemoranda.Text = dgvForReBilling.Rows[e.RowIndex].Cells[5].Value.ToString().Trim();
                m_sBIN = dgvForReBilling.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
            }
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Re-Bill BIN: "+m_sBIN+".", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            m_sBIN = string.Empty;
            this.Close();
        }

    }
}