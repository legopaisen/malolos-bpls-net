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

namespace ReturnDeclareOR
{
    public partial class frmORInventory : Form
    {
        public frmORInventory()
        {
            InitializeComponent();
        }

        private void frmORInventory_Load(object sender, EventArgs e)
        {
            this.LoadList();
            this.m_lstExceptionControl.Add(txtDateCreated.Name);
            this.m_lstExceptionControl.Add(txtCreatedBy.Name);
            this.CurrentFormState(FormState.InitialState, dgView, btnAdd, btnEdit, btnDelete, btnClose, this.grpFields);            
        }

        public enum FormState
        {
            InitialState,
            AddState,
            EditState
        }

        private FormState m_formState;

        /// <summary>
        /// CurrentFormState
        /// </summary>
        /// <param name="formState">State if Form</param>
        /// <param name="dataGridView">DataGridView</param>
        /// <param name="Values">0 = btnAdd, 1 = btnEdit, 2 = btnDelete, 3 = btnClose, 4 = controlParent</param>
        private void CurrentFormState(FormState formState, DataGridView dataGridView, params Control[] Values)
        {
            m_formState = formState;

            if (formState == FormState.InitialState)
            {
                Values[0].Text = "&Add";
                Values[1].Text = "&Edit";
                Values[3].Text = "&Close";

                Values[0].Enabled = true;
                Values[1].Enabled = true;
                Values[2].Enabled = true;

                this.EnableClearControls(Values[4], false, true);
                dataGridView.Enabled = true;

                this.LoadList();
            }
            else if (formState == FormState.AddState)
            {
                Values[0].Text = "&Save";
                Values[1].Text = "&Edit";
                Values[3].Text = "&Cancel";

                Values[0].Enabled = true;
                Values[1].Enabled = false;
                Values[2].Enabled = false;

                this.EnableClearControls(Values[4], true, true);
                dataGridView.Enabled = false;
            }
            else if (formState == FormState.EditState)
            {
                Values[0].Text = "&Add";
                Values[1].Text = "&Update";
                Values[3].Text = "&Cancel";

                Values[0].Enabled = false;
                Values[1].Enabled = true;
                Values[2].Enabled = false;

                this.EnableClearControls(Values[4], true, false);
                dataGridView.Enabled = false;
            }
        }

        private List<string> m_lstExceptionControl = new List<string>();

        private void EnableClearControls(Control controlParent, bool IsEnable, bool IsClear)
        {
            foreach (Control ctrl in controlParent.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox textBox = new TextBox();
                    textBox = (TextBox)ctrl;

                    if (IsClear) 
                        ctrl.Text = string.Empty;

                    if (m_lstExceptionControl.Count > 0)
                    {
                        textBox.ReadOnly = !IsEnable;

                        for (int i = 0; i != m_lstExceptionControl.Count; i++)
                        {
                            if (m_lstExceptionControl[i] == textBox.Name)
                            {
                                textBox.ReadOnly = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        textBox.ReadOnly = !IsEnable;
                    }
                }
            }
        }

        private void LoadList()
        {
            OracleResultSet result = new OracleResultSet();

            result.Query = "SELECT * FROM OR_Inventory ORDER BY OR_From, OR_To";
            if (result.Execute())
            {
                dgView.Rows.Clear();
                while (result.Read()) 
                {
                    dgView.Rows.Add(
                        result.GetString("OR_From"),
                        result.GetString("OR_To"),
                        result.GetDateTime("Inv_Date").ToShortDateString(),
                        result.GetString("User_Code"));
                }
            }
            result.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "&Add")
            {
                this.CurrentFormState(FormState.AddState, dgView, btnAdd, btnEdit, btnDelete, btnClose, this.grpFields);
                txtDateCreated.Text = AppSettingsManager.GetSystemDate().ToShortDateString();
                txtCreatedBy.Text = AppSettingsManager.SystemUser.UserCode;
                txtORFrom.Focus();
            }
            else 
            {

                if (!ValidateORSeries())
                    return;

                if (!ValidateORConflict())
                    return;

                if (!ValidateORUsed())
                    return;

                if (this.Save())
                {
                    MessageBox.Show("New record saved successfully.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else 
                {
                    MessageBox.Show("An error occurred while data record.\nPlease try again.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                this.CurrentFormState(FormState.InitialState, dgView, btnAdd, btnEdit, btnDelete, btnClose, this.grpFields);

            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (btnEdit.Text == "&Edit")
            {
                this.CurrentFormState(FormState.EditState, dgView, btnAdd, btnEdit, btnDelete, btnClose, this.grpFields);                
                txtORFrom.Focus();
            }
            else
            {

                if (!ValidateORSeries())
                    return;

                if (!ValidateORConflict())
                    return;

                if (!ValidateORUsed())
                    return;

                if (this.Update())
                {
                    MessageBox.Show("Record updated successfully.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("An error occurred while updating data.\nPlease try again.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                this.CurrentFormState(FormState.InitialState, dgView, btnAdd, btnEdit, btnDelete, btnClose, this.grpFields);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!ValidateORUsed())
                return;

            if (MessageBox.Show("Are you sure you want to delete this record?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (this.Delete())
                {
                    MessageBox.Show("Record deleted successfully.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("An error occurred while deleting data.\nPlease try again.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                this.CurrentFormState(FormState.InitialState, dgView, btnAdd, btnEdit, btnDelete, btnClose, this.grpFields);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "&Cancel")
            {
                this.CurrentFormState(FormState.InitialState, dgView, btnAdd, btnEdit, btnDelete, btnClose, this.grpFields);
            }
            else
            {
                this.Close();
            }
        }

        private void dgView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!string.IsNullOrEmpty(dgView.CurrentRow.Cells[0].Value.ToString().Trim()))
            {
                txtORFrom.Text = dgView.CurrentRow.Cells[0].Value.ToString();
                txtORTo.Text = dgView.CurrentRow.Cells[1].Value.ToString();
                txtDateCreated.Text = dgView.CurrentRow.Cells[2].Value.ToString();
                txtCreatedBy.Text = dgView.CurrentRow.Cells[3].Value.ToString();
            }
        }

        private bool Save()
        {
            OracleResultSet result = new OracleResultSet();
            result.Transaction = true;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("'{0}', ", txtORFrom.Text);
            sb.AppendFormat("'{0}', ", txtORTo.Text);
            sb.AppendFormat("to_date('{0}', 'MM/dd/yyyy'), ", AppSettingsManager.GetCurrentDate().ToShortDateString());
            sb.AppendFormat("'{0}' ", AppSettingsManager.SystemUser.UserCode);

            result.Query = string.Format("INSERT INTO OR_Inventory VALUES ({0})", sb.ToString());
            if (result.ExecuteNonQuery() == 0)
            {
                result.Rollback();
                result.Close();
                return false;
            }

            result.Commit();

            return true;
        }

        private bool Update()
        {
            OracleResultSet result = new OracleResultSet();
            result.Transaction = true;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("OR_From = '{0}', ", txtORFrom.Text);
            sb.AppendFormat("OR_To = '{0}' ", txtORTo.Text);

            result.Query = string.Format("UPDATE OR_Inventory SET {0} WHERE OR_From = '{1}' AND OR_To = '{2}'", sb.ToString(), dgView.CurrentRow.Cells[0].Value.ToString(), dgView.CurrentRow.Cells[1].Value.ToString());
            if (result.ExecuteNonQuery() == 0)
            {
                result.Rollback();
                result.Close();
                return false;
            }

            result.Commit();

            return true;
        }

        private bool Delete()
        {
            OracleResultSet result = new OracleResultSet();
            result.Transaction = true;

            result.Query = string.Format("DELETE FROM OR_Inventory WHERE OR_From = '{0}' AND OR_To = '{1}'", txtORFrom.Text, txtORTo.Text);
            if (result.ExecuteNonQuery() == 0)
            {
                result.Rollback();
                result.Close();
                return false;
            }

            result.Commit();

            return true;
        }

        private void txtORFrom_Leave(object sender, EventArgs e)
        {
            if (m_formState == FormState.AddState || m_formState == FormState.EditState)
            {
                if (string.IsNullOrEmpty(txtORFrom.Text.Trim()))
                {
                    txtORTo.Clear();
                }
                else /*if ((!string.IsNullOrEmpty(txtORFrom.Text.Trim()) || txtORFrom.Text.Trim().Length < 7)
                    && string.IsNullOrEmpty(txtORTo.Text.Trim()))*/
                {
                    //this.CreateSeries(); //JARS 20171027 ON SITE
                }
            }
        }

        private void txtORTo_Leave(object sender, EventArgs e)
        {
            if (m_formState == FormState.AddState || m_formState == FormState.EditState)
            {
                //this.CreateSeries(); //JARS 20171027 ON SITE
            }
            ValidateNumber(); //JARS 20171103
        }

        private void CreateSeries()
        {
            if (!string.IsNullOrEmpty(txtORFrom.Text.Trim()))
            {
                int intORFrom = 0;
                int intORTo = 0;
                int.TryParse(txtORFrom.Text, out intORFrom);

                if (intORFrom > 0)
                {
                    txtORFrom.Text = intORFrom.ToString("0000000");
                    intORTo = intORFrom + 49;
                    txtORTo.Text = intORTo.ToString("0000000");
                }
            }
        }

        private void ValidateNumber() //JARS 20171103
        {
            int intORFrom = 0;
            int intORTo = 0;
            int iDiff = 0;
            int.TryParse(txtORFrom.Text, out intORFrom);
            int.TryParse(txtORTo.Text, out intORTo);

            iDiff = intORTo - intORFrom;

            if(iDiff > 50)
            {
                MessageBox.Show("OR Inventory must not exceed 50 O.R. Numbers", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtORTo.Text = "";
                txtORTo.Focus();
                return;
            }

        }

        private void txtORSeries_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)08) // Digits and Backspace only
                e.Handled = true;
            else
                e.Handled = false;
        }

        private bool ValidateORSeries()
        {
            int intORFrom = 0;
            int intORTo = 0;

            int.TryParse(txtORFrom.Text, out intORFrom);
            int.TryParse(txtORTo.Text, out intORTo);

            if (intORFrom <= 0)
            {
                MessageBox.Show("Please enter O.R. From", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (intORTo <= 0)
            {
                MessageBox.Show("Please enter O.R. To", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (intORFrom > intORTo)
            {
                MessageBox.Show("It seems that your O.R. From is greater than O.R. To\nPlease enter valid O.R. Series", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }

        private bool ValidateORConflict()
        {
            OracleResultSet result = new OracleResultSet();

            result.Query = "SELECT * FROM OR_Inventory ";
            result.Query += string.Format("WHERE ({0} BETWEEN OR_From AND OR_To OR {1} BETWEEN OR_From AND OR_To) ", txtORFrom.Text, txtORTo.Text);
            if (m_formState == FormState.EditState)
                result.Query += string.Format("AND (OR_From <> '{0}' AND OR_To <> '{1}') ", dgView.CurrentRow.Cells[0].Value.ToString(), dgView.CurrentRow.Cells[1].Value.ToString());
            if (result.Execute())
            {
                if (result.Read())
                {
                    result.Close();
                    MessageBox.Show("Your O.R. Series conflict with other existing series.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            result.Close();

            return true;

        }

        private bool ValidateORUsed()
        {
            OracleResultSet result = new OracleResultSet();

            result.Query = string.Format("SELECT * FROM OR_Used WHERE OR_No BETWEEN {0} AND {1}", txtORFrom.Text, txtORTo.Text);
            if (result.Execute())
            {
                if (result.Read())
                {
                    result.Close();
                    //MessageBox.Show("O.R. Series already been used.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    MessageBox.Show("O.R. Series already been used.\nDeletion not allowed.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop); // RMC 20150501 QA BTAS
                    return false;
                }
            }
            result.Close();

            // RMC 20150501 QA BTAS (s)
            result.Query = string.Format("SELECT * FROM or_assigned WHERE to_OR_No BETWEEN {0} AND {1}", txtORFrom.Text, txtORTo.Text);
            if (result.Execute())
            {
                if (result.Read())
                {
                    result.Close();
                    MessageBox.Show("O.R. Series already been declared to a teller.\nDeletion not allowed.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop); 
                    return false;
                }
            }
            result.Close();

            result.Query = string.Format("SELECT * FROM or_assigned_hist WHERE to_OR_No BETWEEN {0} AND {1}", txtORFrom.Text, txtORTo.Text);
            if (result.Execute())
            {
                if (result.Read())
                {
                    result.Close();
                    MessageBox.Show("O.R. Series already been declared to a teller.\nDeletion not allowed.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            result.Close();
            // RMC 20150501 QA BTAS (e)

            return true;
        }

        private void frmORInventoryONL_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to close?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;
        }

        private void lblORDec_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmReturnDeclareOR FormReturnDeclareOR = new frmReturnDeclareOR();
            FormReturnDeclareOR.IsORInvInstance = true;
            FormReturnDeclareOR.ShowDialog();
            this.LoadList();
        }

       
    }
}