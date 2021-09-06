using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace Amellar.Modules.BusinessPermit //JARS 20190325 FOR SIGNATORY CONFIG
{
    public partial class frmPermitSignatory : Form
    {
        public frmPermitSignatory()
        {
            InitializeComponent();
        }

        public int m_iId = 0;

        private void LoadValues()
        {
            dgvSignatory.Rows.Clear();
            int id = 0;
            string sLastName = string.Empty;
            string sFirstName = string.Empty;
            string sMiddleInitial = string.Empty;
            string sPosition = string.Empty;

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from signatory order by id asc";

            if(pSet.Execute())
            {
                while(pSet.Read())
                {
                    id = pSet.GetInt("id");
                    sLastName = pSet.GetString("sig_ln");
                    sFirstName = pSet.GetString("sig_fn");
                    sMiddleInitial = pSet.GetString("sig_mi");
                    sPosition = pSet.GetString("sig_position");

                    dgvSignatory.Rows.Add(id.ToString(),sLastName,sFirstName,sMiddleInitial,sPosition);
                }
            }
            pSet.Close();
        }

        private void Clear()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtMiddleInitial.Text = "";
            txtPosition.Text = "";
            cmbPosition.Text = "";
        }

        private void Enable()
        {
            txtFirstName.Enabled = true;
            txtLastName.Enabled = true;
            txtMiddleInitial.Enabled = true;
            txtPosition.Enabled = true;
            cmbPosition.Enabled = true;
        }
        private void Disable()
        {
            txtFirstName.Enabled = false;
            txtLastName.Enabled = false;
            txtMiddleInitial.Enabled = false;
            txtPosition.Enabled = false;
            txtPosition.Enabled = false;
        }

        private void frmPermitSignatory_Load(object sender, EventArgs e)
        {
            LoadValues();
            cmbPosition.Items.Add("");
            cmbPosition.Items.Add("MAYOR");
            cmbPosition.Items.Add("ACTING MAYOR");
            cmbPosition.Items.Add("S.B. MEMBER");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(btnAdd.Text == "Add")
            {
                Clear();
                Enable();

                btnEdit.Enabled = false;
                btnDelete.Enabled = true;
                btnAdd.Text = "Save";
                btnDelete.Text = "Cancel";
                dgvSignatory.Enabled = false;
            }
            else if(btnAdd.Text == "Save")
            {
                int iId = 0;

                if (txtFirstName.Text != "" && txtLastName.Text != "" && txtMiddleInitial.Text != "" && txtPosition.Text != "" /*cmbPosition.Text != ""*/)
                {
                    if (MessageBox.Show("Do you want to Save this Signatory?", "Business Permit Signatory", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        OracleResultSet pSet = new OracleResultSet();

                        pSet.Query = "select max(id) as ID from signatory";
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                iId = pSet.GetInt("ID");
                            }
                        }
                        pSet.Close();

                        iId++;

                        pSet.Query = "insert into signatory values (";
                        pSet.Query += iId + ", ";
                        pSet.Query += "'" + txtLastName.Text.ToString() + "', ";
                        pSet.Query += "'" + txtFirstName.Text.ToString() + "', ";
                        pSet.Query += "'" + txtMiddleInitial.Text.ToString() + "', ";
                        pSet.Query += "'" + txtPosition.Text.ToString() + "')";
                        //pSet.Query += "'" + cmbPosition.Text.ToString() + "')";

                        if (pSet.ExecuteNonQuery() != 0)
                        { }

                        //MessageBox.Show("Business Permit Signatory Added");
                        MessageBox.Show("Signatory Added"); //JARS 20190325 TO USE WITH OTHER PERMIT
                        LoadValues();
                        Clear();
                        Disable();
                        btnAdd.Text = "Add";
                        btnDelete.Text = "Delete";
                        dgvSignatory.Enabled = true;
                        btnDelete.Enabled = true;
                        btnEdit.Enabled = true;
                    }
                    else
                    {
                        return;
                    }

                }
                else
                {
                    MessageBox.Show("Please fill-up all fields");
                    return;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            if(btnDelete.Text == "Delete")
            {
                if (txtLastName.Text == "")
                {
                    MessageBox.Show("Select Signatory Person first.");
                    return;
                }
                else
                {
                    if (MessageBox.Show("Delete " + txtLastName.Text.ToString() + " from business permit signatories?", "Business Permit Signatory", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        pSet.Query = "delete from signatory where id = '" + m_iId + "'";
                        if (pSet.ExecuteNonQuery() != 0)
                        { }

                        MessageBox.Show("Signatory Deleted");
                        LoadValues();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else if(btnDelete.Text == "Cancel")
            {
                btnAdd.Text = "Add";
                btnEdit.Text = "Edit";
                btnDelete.Text = "Delete";
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                dgvSignatory.Enabled = true;
                Clear();
                Disable();
                return;
            }
        }

        private void dgvSignatory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                Populate(e.RowIndex);
            }
        }

        private void dgvSignatory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                Populate(e.RowIndex);
            }
        }

        private void Populate(int i)
        {
            m_iId = Convert.ToInt32(dgvSignatory.Rows[i].Cells[0].Value.ToString());
            txtLastName.Text = dgvSignatory.Rows[i].Cells[1].Value.ToString();
            txtFirstName.Text = dgvSignatory.Rows[i].Cells[2].Value.ToString();
            txtMiddleInitial.Text = dgvSignatory.Rows[i].Cells[3].Value.ToString();
            txtPosition.Text = dgvSignatory.Rows[i].Cells[4].Value.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sToBeUpdated = string.Empty;

            if (btnEdit.Text == "Edit")
            {
                btnAdd.Enabled = false;
                btnDelete.Text = "Cancel";
                btnEdit.Text = "Update";
                Enable();
                sToBeUpdated = m_iId.ToString();
                dgvSignatory.Enabled = false;
            }
            else if(btnEdit.Text == "Update")
            {
                if (txtLastName.Text == "")
                {
                    MessageBox.Show("Select Signatory Person first.");
                    return;
                }
                else
                {
                    sToBeUpdated = m_iId.ToString();
                    if (MessageBox.Show("Update information of this signatory?", "Business Permit Signatory", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        pSet.Query = "update signatory set sig_ln = '" + txtLastName.Text.Trim() + "', sig_fn = '" + txtFirstName.Text.Trim() + "', sig_mi = '" + txtMiddleInitial.Text.Trim() + "', sig_position = '" + txtPosition.Text.Trim() + "' where id = '" + Convert.ToInt32(sToBeUpdated) + "'";
                        //pSet.Query = "update signatory set sig_ln = '" + txtLastName.Text.Trim() + "', sig_fn = '" + txtFirstName.Text.Trim() + "', sig_mi = '" + txtMiddleInitial.Text.Trim() + "', sig_position = '" + cmbPosition.Text.Trim() +"' where id = '" + Convert.ToInt32(sToBeUpdated) + "'";
                        if (pSet.ExecuteNonQuery() != 0)
                        { }

                        MessageBox.Show("Signatory Updated");
                        LoadValues();
                    }
                    else
                    {
                        return;
                    }
                }
                dgvSignatory.Enabled = true;
                btnAdd.Enabled = true;
                btnEdit.Text = "Edit";
                btnDelete.Text = "Delete";
                Disable();
                Clear();
                return;
            }
        }

        private void txtPosition_TextChanged(object sender, EventArgs e)
        {
            //AFM 20200121 character casing changed to normal in properties to imitate exact sanitary permit - malolos ver.
        }
    }
}