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
    public partial class frmBankTable : Form
    {
        bool boolstate;
        bool booldelselect = false;
        int intColCount;
        public frmBankTable()
        {
            InitializeComponent();
        }

        private void tblBankSearch()
        {
            string strTaxRateQuery ="select bank_code \"BANK CODE\", bank_nm \"BANK NAME\","
                                    + " bank_branch \"BANK BRANCH\", bank_addr \"BANK ADDRESS\","
                                    + " bank_mun \"BANK MUNICIPAL\", bank_prov \"BANK PROVINCE\""
                                    + " from bank_tbl order by bank_code";
          
            DataGridView tblGridView = new DataGridView();

            DataGridViewOracleResultSet dsTaxRates = new DataGridViewOracleResultSet(tblGridView, strTaxRateQuery);
            grdBankTable.DataSource = tblGridView.DataSource;
            grdBankTable.DataMember = tblGridView.DataMember;

            DataGridViewUtilities.HideNewRows(grdBankTable);

            grdBankTable.Columns[2].Visible = false;
            grdBankTable.Columns[3].Visible = false;
            grdBankTable.Columns[4].Visible = false;
            grdBankTable.Columns[5].Visible = false;
            btnViewAll.Text = "Vie&w all Info";
            grdBankTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grdBankTable.Refresh();

        }

        private void textState()
        {
            txtAddress.Enabled = boolstate;
            txtBankName.Enabled = boolstate;
            txtBranch.Enabled = boolstate;
            txtMun.Enabled = boolstate;
            txtProv.Enabled = boolstate;
        }

        private void textClear()
        {
            txtAddress.Text = "";
            txtBankCode.Text = "";
            txtBankName.Text = "";
            txtBranch.Text = "";
            txtMun.Text = "";
            txtProv.Text = "";
        }

        private void textvalidation()
        {

            if (txtAddress.Text == "" || txtBankCode.Text == "" || txtBankName.Text == "" ||
                txtBranch.Text == "" || txtMun.Text == "" || txtProv.Text == "")
            {
                if (btnAdd.Enabled == false || btnEdit.Enabled == false)
                    MessageBox.Show("Fill all the Empty Fields.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {
                if (btnAdd.Text == "&Save")/// button Add has text property of &Save
                {
                    btnAdd.Text = "&Add";
                    boolstate = false;
                    textState();
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnClose.Text = "&Close";
                    grdBankTable.Enabled = true;


                    /// Here comes the Insert query
                    string strAddQuery = "INSERT INTO BANK_TBL VALUES"
                    + " ('" + txtBankCode.Text.Trim()
                    + "', '" + txtBankName.Text.Trim()
                    + "', '" + txtBranch.Text.Trim()
                    + "', '" + txtAddress.Text.Trim()
                    + "', '" + txtMun.Text.Trim()
                    + "', '" + txtProv.Text.Trim()
                    + "')";

                    DataGridViewOracleResultSet dsTaxRates = new DataGridViewOracleResultSet(strAddQuery);
                    tblBankSearch();
                    textClear();
                    MessageBox.Show("Data Successfully Added", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else if (btnEdit.Text == "&Save") /// button Edit has text property of &Save
                {
                    btnEdit.Text = "&Edit";
                    boolstate = false;
                    textState();
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;
                    btnClose.Text = "&Close";

                    string strCode = grdBankTable.SelectedCells[0].Value.ToString();

                    /// Here comes the Update query
                    string strEditQuery = "UPDATE BANK_TBL SET"
                    + " bank_code = '" + txtBankCode.Text.Trim()
                    + "', bank_nm = '" + txtBankName.Text.Trim()
                    + "', bank_branch = '" + txtBranch.Text.Trim()
                    + "', bank_addr = '" + txtAddress.Text.Trim()
                    + "', bank_mun = '" + txtMun.Text.Trim()
                    + "', bank_prov = '" + txtProv.Text.Trim()
                    + "' WHERE bank_code = '" + strCode.Trim() + "'";

                    DataGridViewOracleResultSet dsBankSearch = new DataGridViewOracleResultSet(strEditQuery);
                    tblBankSearch();
                    textClear();
                    MessageBox.Show("Data Successfully Updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }




        private void frmBankTable_Load(object sender, EventArgs e)
        {
            /*
            System.Drawing.Drawing2D.GraphicsPath paths = new System.Drawing.Drawing2D.GraphicsPath();
            DesignDLL design = new DesignDLL(out paths, this.Width, this.Height);
            this.Region = new Region(paths);
            */
            tblBankSearch();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            booldelselect = false;

            if (btnAdd.Text == "&Add")
            {
                grdBankTable.Enabled = false;
                btnAdd.Text = "&Save";
                boolstate = true;
                textState();
                textClear();
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnClose.Text = "&Cancel";

                OracleResultSet result = new OracleResultSet();
                result.Query = "SELECT COUNT(*) bank_code from bank_tbl";
                if (!result.Execute())
                {
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    while (result.Read())
                    {
                        intColCount = result.GetInt(0);
                    }
                intColCount += 1;
                txtBankCode.Text = string.Format("{0:000000#}", intColCount);
                intColCount = 0;
            }
            else
            {
                textvalidation();
            }
        }

        private void grdBankTable_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (grdBankTable.RowCount <= 0)
            {
                MessageBox.Show("No Data To be Delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {
                booldelselect = true;
                string strCode = grdBankTable.SelectedCells[0].Value.ToString().Trim();
                OracleResultSet result = new OracleResultSet();
                result.Query = "select * from bank_tbl where trim(bank_code) = :1";
                result.AddParameter(":1", strCode);

                if (!result.Execute())
                {
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    while (result.Read())
                    {
                        txtBankCode.Text = result.GetString(0).ToString().Trim();
                        txtBankName.Text = result.GetString(1).ToString().Trim();
                        txtBranch.Text = result.GetString(2).ToString().Trim();
                        txtAddress.Text = result.GetString(3).ToString().Trim();
                        txtMun.Text = result.GetString(4).ToString().Trim();
                        txtProv.Text = result.GetString(5).ToString().Trim();
                    }

                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            booldelselect = false;
            if (btnEdit.Text == "&Edit")
            {
                btnEdit.Text = "&Save";
                boolstate = true;
                textState();
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                btnClose.Text = "&Cancel";
            }
            else
            {
                textvalidation();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (grdBankTable.RowCount <= 0)
            {
                MessageBox.Show("No Data To be Delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (booldelselect)
                {
                    if (MessageBox.Show("Are you sure to Delete the selected row", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    { MessageBox.Show("not deleted"); }
                    else
                    {
                        string strCode = grdBankTable.SelectedCells[0].Value.ToString().Trim();
                        int intCode = int.Parse(strCode);
                        //string strDelQuery = "DELETE FROM bank_tbl WHERE bank_code = '" + strCode + "'";


                        string strDelQuery = "DECLARE"
                                           + " counter number;"
                                           + " BEGIN"
                                           + " Select count(*) INTO counter from bank_tbl;"
                                           + " DELETE bank_tbl where bank_code = "
                                           + strCode + ";"
                                           + " FOR i in "
                                           + intCode + "..counter LOOP"
                                           + " UPDATE bank_tbl SET bank_code = trim(to_char(i,'0999999')) where bank_code = i+1;"
                                           + " END LOOP;"
                                           + " END;";

                        DataGridViewOracleResultSet dsBankSearch = new DataGridViewOracleResultSet(strDelQuery);
                        tblBankSearch();
                        textClear();
                        MessageBox.Show("Data Successfully Deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
                else
                    MessageBox.Show("Click From the Table You wish to Delete.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            booldelselect = false;
            if (btnClose.Text == "&Close")
                this.Close();
            else
            {
                textClear();
                boolstate = false;
                textState();
                btnClose.Text = "&Close";
                if (btnAdd.Text == "&Save")
                {
                    btnAdd.Text = "&Add";
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    grdBankTable.Enabled = true;
                }
                else if (btnEdit.Text == "&Save")
                {
                    btnEdit.Text = "&Edit";
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            if (btnViewAll.Text == "Vie&w all Info")
            {
                grdBankTable.Columns[2].Visible = true;
                grdBankTable.Columns[3].Visible = true;
                grdBankTable.Columns[4].Visible = true;
                grdBankTable.Columns[5].Visible = true;
                btnViewAll.Text = "Hid&e some Info";
                grdBankTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            }
            else
            {
                grdBankTable.Columns[2].Visible = false;
                grdBankTable.Columns[3].Visible = false;
                grdBankTable.Columns[4].Visible = false;
                grdBankTable.Columns[5].Visible = false;
                btnViewAll.Text = "Vie&w all Info";
                grdBankTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            }

        }












    }
}