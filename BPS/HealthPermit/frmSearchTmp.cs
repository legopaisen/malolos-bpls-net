

// RMC 20141228 modified permit printing (lubao) - modified whole class


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.HealthPermit
{
    public partial class frmSearchTmp : Form
    {
        private string m_sBIN = string.Empty;
        private string m_sPermit = string.Empty;
        private string m_sTemp = string.Empty; //JARS 20151229
        private string m_sBcont = string.Empty; //JARS 20151229
        private string m_sValue = string.Empty;

        public string Val //JARS 20151229
        {
            get { return m_sValue; }
            set { m_sValue = value; }
        }

        public string sCont //JARS 20151229
        {
            get { return m_sBcont; }
            set { m_sBcont = value; }
        }

        public string sTemp //JARS 20151229
        {
            get { return m_sTemp; }
            set { m_sTemp = value; }
        }

        public string TaxYear
        {
            get { return txtTaxYear.Text; }
            set { txtTaxYear.Text = value; }
        }

        public string BIN
        {
            get { return m_sBIN; }
            set { m_sBIN = value; }
        }

        public string LastName
        {
            get { return txtLastName.Text; }
            set { txtLastName.Text = value; }
        }

        public string FirstName
        {
            get { return txtFirstName.Text; }
            set { txtFirstName.Text = value; }
        }

        public string MI
        {
            get { return txtMiddleInitial.Text; }
            set { txtMiddleInitial.Text = value; }
        }

        public string BnsName
        {
            get { return txtBnsName.Text; }
            set { txtBnsName.Text = value; }
        }

        public string BnsAdd
        {
            get { return txtBnsAdd.Text; }
            set { txtBnsAdd.Text = value; }
        }

        public string Permit
        {
            get { return m_sPermit; }
            set { m_sPermit = value; }
        }

        public frmSearchTmp()
        {
            InitializeComponent();
        }

        private void frmSearchTmp_Load(object sender, EventArgs e)
        {
            dgvList.Columns.Clear();
            if(m_sValue == "false")
            {
                btnNoTemp.Visible = false;
            }
            dgvList.Columns.Add("1", "Temp BIN");
            dgvList.Columns.Add("2", "BIN");
            dgvList.Columns.Add("3", "Business Name");
            dgvList.Columns.Add("4", "Tax Year");
            dgvList.Columns.Add("5", "Last Name");
            dgvList.Columns.Add("6", "First Name");
            dgvList.Columns.Add("7", "M.I.");
            dgvList.Columns.Add("8", "Business Address");
            dgvList.Columns.Add("9", "Occupation");
            dgvList.Columns[0].Width = 80;
            dgvList.Columns[1].Width = 100;
            dgvList.Columns[2].Width = 100;
            dgvList.Columns[3].Width = 80;
            dgvList.Columns[4].Width = 100;
            dgvList.Columns[5].Width = 100;
            dgvList.Columns[6].Width = 20;
            dgvList.Columns[7].Width = 100;
            dgvList.Columns[8].Width = 100;

            OracleResultSet pSet = new OracleResultSet();

            // RMC 20150117 (s)

            
            string sLN = ""; string sFN = ""; string sMI = ""; string sBIN = "";

            if (m_sPermit == "Annual Inspection")
            {
                pSet.Query = " select distinct * from ( ";
                pSet.Query += "select distinct temp_bin, bin, bns_nm, tax_year, bns_add,emp_occupation from emp_names where ";
                pSet.Query += "tax_year = '" + txtTaxYear.Text + "' ";
                pSet.Query += "and temp_bin like '" + txtTaxYear.Text + "%' ";
                pSet.Query += "and bin not in (select bin from annual_insp where tax_year = '" + txtTaxYear.Text + "') and emp_occupation = 'OWNER' ";
                pSet.Query += "union all ";
                pSet.Query += "select distinct temp_bin, bin, bns_nm, tax_year, bns_add,emp_occupation from emp_names where ";
                pSet.Query += "tax_year = '" + txtTaxYear.Text + "' ";
                pSet.Query += "and temp_bin like '" + txtTaxYear.Text + "%' ";
                pSet.Query += "and trim(bin) is null and temp_bin not in (select bin from annual_insp where tax_year = '" + txtTaxYear.Text + "')and emp_occupation = 'OWNER' ";
                pSet.Query += " ) "; //jhb 20191030 add owner in query

                /*MCR 20150218
                pSet.Query = "select distinct temp_bin, bin, bns_nm, tax_year, bns_add, emp_occupation from emp_names where ";
                pSet.Query += "tax_year = '" + txtTaxYear.Text + "' ";
                pSet.Query += "and temp_bin like '" + txtTaxYear.Text + "%' ";
                pSet.Query += "and (trim(bin) is null or length(bin) > 12)";
                pSet.Query += "and temp_bin not in (select bin from annual_insp where tax_year = '" + txtTaxYear.Text + "')";*/
            }
            else
            {
                pSet.Query = "select distinct temp_bin, bin, bns_nm, tax_year, bns_add, emp_occupation from emp_names where ";
        //      pSet.Query += "tax_year = '" + txtTaxYear.Text + "' ";
                pSet.Query += "tax_year = '" + txtTaxYear.Text + "'  and  emp_occupation = 'OWNER' "; //JHB 20200716 display only the OWNER 
                pSet.Query += "and temp_bin like '" + txtTaxYear.Text + "%' ";
                if (m_sPermit == "Application" || m_sPermit == "Zoning") 
                    pSet.Query += "and (trim(bin) is null or temp_bin = bin)"; //MCR 20150121
                
                //pSet.Query += "and (trim(bin) is null or length(bin) > 12)"; //MCR 20150218
            }
           // pSet.Query += "order by bns_nm"; 
            pSet.Query += " order by bin, temp_bin "; 
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sBIN = pSet.GetString(0); 

                    GetOwnName(sBIN, out sLN, out sFN, out sMI);

                    dgvList.Rows.Add(sBIN, pSet.GetString(1), pSet.GetString(2),
                        pSet.GetString(3), sLN, sFN, sMI, pSet.GetString(4), pSet.GetString(5));
                }
            }
            pSet.Close();
            
            
            // RMC 20150117 (e)

                //pSet.Query = "select distinct (TEMP_BIN), BIN, bns_nm, TAX_YEAR, EMP_LN, EMP_FN, EMP_MI, bns_add from emp_names where tax_year = '" + txtTaxYear.Text + "' and emp_occupation = 'OWNER' order by temp_bin";   // RMC 20141228 modified permit printing (lubao)
                /*pSet.Query = "select distinct (TEMP_BIN), BIN, bns_nm, TAX_YEAR, EMP_LN, EMP_FN, EMP_MI, bns_add from emp_names where tax_year = '" + txtTaxYear.Text + "' order by temp_bin";
                
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        // RMC 20150102 mods in permit (s)
                        bool bInclude = false;
                        if (pSet.GetString(1).Trim() == "")
                        {
                            if (m_sPermit == "Application")
                            {
                                bInclude = true;
                            }
                            else
                                bInclude = ValidateBIN(pSet.GetString(0));
                        }
                        else if (pSet.GetString(1).Length < 12)
                        {
                            if (m_sPermit == "Application")
                                bInclude = true;
                        }
                        else
                            bInclude = ValidateBIN(pSet.GetString(1));

                        if (bInclude)   // RMC 20150102 mods in permit (e)
                        {
                            dgvList.Rows.Add(pSet.GetString(0), pSet.GetString(1), pSet.GetString(2),
                            pSet.GetString(3), pSet.GetString(4), pSet.GetString(5), pSet.GetString(6), pSet.GetString(7));
                        }
                    }
                }
                pSet.Close();*/
            
        }

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // RMC 20141228 modified permit printing (lubao) (s)
            if (txtBIN.Text.Trim() != "")
            {
                m_sBIN = txtBIN.Text;
            }
            else
            {
                //m_sBIN = txtTBIN.Text;
                m_sBIN = txtTBIN.Text;
            }
            // RMC 20141228 modified permit printing (lubao) (e)
            this.Close();
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtTBIN.Text = dgvList[0, e.RowIndex].Value.ToString();
                txtBIN.Text = dgvList[1, e.RowIndex].Value.ToString();
                txtBnsName.Text = dgvList[2, e.RowIndex].Value.ToString();
                txtTaxYear.Text = dgvList[3, e.RowIndex].Value.ToString();
                txtLastName.Text = dgvList[4, e.RowIndex].Value.ToString();
                txtFirstName.Text = dgvList[5, e.RowIndex].Value.ToString();
                txtMiddleInitial.Text = dgvList[6, e.RowIndex].Value.ToString();
                txtBnsAdd.Text = dgvList[7, e.RowIndex].Value.ToString();
            }
            catch
            {
                txtTBIN.Text = "";
                txtBIN.Text = "";
                txtBnsName.Text = "";
                txtTaxYear.Text = "";
                txtLastName.Text = "";
                txtFirstName.Text = "";
                txtMiddleInitial.Text = "";
                txtBnsAdd.Text = "";
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtMiddleInitial.Text = "";
            txtTBIN.Text = "";
            txtBIN.Text = "";
            txtBnsName.Text = "";
            txtBnsAdd.Text = "";
        }

        private bool ValidateBIN(string sBIN)
        {
            // RMC 20150102 mods in permit
            OracleResultSet pSet = new OracleResultSet();
            int iCnt = 0;

            pSet.Query = "select count(*) from business_que where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
            pSet.Query += " and bns_stat = 'NEW'";
            int.TryParse(pSet.ExecuteScalar(), out iCnt);
            if (iCnt > 0)
            {
                if (m_sPermit == "Annual Inspection" || m_sPermit == "Zoning")
                {
                    pSet.Close();
                    if(m_sPermit == "Annual Inspection")
                        pSet.Query = "select count(*) from annual_insp where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
                    else
                        pSet.Query = "select count(*) from zoning where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
                    int.TryParse(pSet.ExecuteScalar(), out iCnt);
                    if (iCnt == 0)
                    {
                        return true;
                        
                    }
                    else
                        return false;
                }

                return true;
            }
            else
            {
                pSet.Close();
                pSet.Query = "select count(*) from businesses where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
                pSet.Query += " and bns_stat = 'NEW'";
                int.TryParse(pSet.ExecuteScalar(), out iCnt);
                if (iCnt > 0)
                {
                    if (m_sPermit == "Annual Inspection" || m_sPermit == "Zoning")
                    {
                        if (m_sPermit == "Annual Inspection")
                            pSet.Query = "select count(*) from annual_insp where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
                        else
                            pSet.Query = "select count(*) from zoning where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
                        int.TryParse(pSet.ExecuteScalar(), out iCnt);
                        if (iCnt == 0)
                        {
                            return true;
                        }
                        else
                            return false;
                    }

                    return true;
                }
                else
                {
                    pSet.Close();

                    if (m_sPermit == "Annual Inspection" || m_sPermit == "Zoning")
                    {
                        if (m_sPermit == "Annual Inspection")
                            pSet.Query = "select count(*) from annual_insp where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
                        else
                            pSet.Query = "select count(*) from zoning where bin = '" + sBIN + "' and tax_year = '" + txtTaxYear.Text + "'";
                        int.TryParse(pSet.ExecuteScalar(), out iCnt);
                        if (iCnt == 0)
                        {
                            if (StringUtilities.Left(sBIN, 4) == txtTaxYear.Text)
                                return true;
                            else
                                return false;
                        }
                    }
                    else
                    {
                        //if (m_sPermit == "Health" || m_sPermit == "Sanitary") //MCR 20150116
                        if (m_sPermit == "Health" || m_sPermit == "Sanitary" || m_sPermit == "Application")
                            return true;
                        else
                            return false;
                    }
                }
            }

            return false;
        }

        private void GetOwnName(string p_sBIN, out string o_sLN, out string o_sFN, out string o_sMI)
        {
            // RMC 20150117
            OracleResultSet pName = new OracleResultSet();
            o_sLN = "";
            o_sFN = "";
            o_sMI = "";

            pName.Query = "select * from emp_names where (bin = '" + p_sBIN + "' or temp_bin = '" + p_sBIN + "') and emp_occupation = 'OWNER'";
            if (pName.Execute())
            {
                if (pName.Read())
                {
                    o_sLN = pName.GetString("emp_ln");
                    o_sFN = pName.GetString("emp_fn");
                    o_sMI = pName.GetString("emp_mi");
                }
            }
            pName.Close();
        }

        private void btnNoTemp_Click(object sender, EventArgs e)
        {
            
            if (txtBIN.Text.Trim() != "")
            {
                m_sBIN = txtBIN.Text;
                m_sTemp = "1";
                m_sBcont = "1";
                
                
            }
            else
            {
                m_sBIN = txtTBIN.Text;
            }
                
            
            this.Close();
        }
    }
}