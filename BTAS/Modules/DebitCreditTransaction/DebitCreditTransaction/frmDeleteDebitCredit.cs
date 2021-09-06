
// RMC 20160302 merged tool to delete added dbcr memo from Mati

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.DebitCreditTransaction
{
    public partial class frmDeleteDebitCredit : Form
    {
        public frmDeleteDebitCredit()
        {
            InitializeComponent();
        }

        private void frmDeleteDebitCredit_Load(object sender, EventArgs e)
        {
            txtType.Text = "REMOVE ADDED DBCR";

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            if (txtORNo.Text.Trim() == "")
            {
                MessageBox.Show("Please enter OR No.","Remove Added Credit Memo",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }

            double dAmount = 0;

            double.TryParse(txtAmount.Text.Trim(), out dAmount);

            if (dAmount == 0)
            {
                MessageBox.Show("Please enter Amount.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            string sStat = string.Empty;
            pSet.Query = "select * from dbcr_memo where or_no = '" + txtORNo.Text.Trim() + "'"; //JARS 20171010
            pSet.Query += " and (debit = " + dAmount + " or credit = " + dAmount + ") and served = 'N'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sStat = pSet.GetString("stat"); // stat = 1 is tag for added dbcr

                    if (sStat != "1")
                    {
                        MessageBox.Show("This dbcr memo is from Online transaction.\nDeletion not allowed.",this.Text,MessageBoxButtons.OK,MessageBoxIcon.Stop);
                        return;
                    }
                }
                else
                {
                    pSet.Close();
                    MessageBox.Show("There is no tax credit with this amount", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            pSet.Close();

            if (MessageBox.Show("Do you really want to delete this record?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                int iCnt = 0;
                pSet.Query = "select * from dbcr_memo where or_no = '" + txtORNo.Text.Trim() + "' and stat = '1'"; // stat = 1 is tag for added dbcr //JARS 20171010
                pSet.Query += " and (debit = " + dAmount + " or credit = " + dAmount + ") and served = 'N'";
                if (pSet.Execute())
                {
                    OracleResultSet pCmd = new OracleResultSet();
                    string sORDate = string.Empty;
                    string sBin = string.Empty;
                    string sDebit = string.Empty;
                    string sCredit = string.Empty;
                    string sObject = string.Empty;

                    while (pSet.Read())
                    {
                        btnCancel.Text = "&Cancel";
                        iCnt++;
                        sORDate = string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime("or_date"));
                        sBin = pSet.GetString("bin");
                        sDebit = string.Format("{0:####.00}", pSet.GetDouble("debit"));
                        sCredit = string.Format("{0:####.00}", pSet.GetDouble("credit"));

                        if (Convert.ToDouble(sDebit) != 0)
                        {
                            pCmd.Query = "delete from dbcr_memo where or_no = '" + txtORNo.Text.Trim() + "' and stat = '1'"; //JARS 20171010
                            pCmd.Query += " and debit = '" + sDebit + "' and served = 'N'";
                            if (pCmd.ExecuteNonQuery() == 0)
                            { }

                            sObject = sBin + "-" + sORDate + "-" + txtORNo.Text.Trim() + "-" + sDebit;

                            if (AuditTrail.InsertTrail("CTDC", "dbcr_memo", sObject) == 0)
                            {
                                return;
                            }
                        }

                        if (Convert.ToDouble(sCredit) != 0)
                        {
                            pCmd.Query = "delete from dbcr_memo where or_no = '" + txtORNo.Text.Trim() + "' and stat = '1'"; //JARS 20171010
                            pCmd.Query += " and credit = '" + sCredit + "' and served = 'N'";
                            if (pCmd.ExecuteNonQuery() == 0)
                            { }

                            sObject = sBin + "-" + sORDate + "-" + txtORNo.Text.Trim() + "-" + sCredit;

                            if (AuditTrail.InsertTrail("CTDC", "dbcr_memo", sObject) == 0)
                            {
                                return;
                            }

                        }


                    }
                }
                pSet.Close();

                if (iCnt > 0)
                {
                    MessageBox.Show("Credit memo deleted",this.Text,MessageBoxButtons.OK,MessageBoxIcon.Information);
                    txtORNo.Text = "";
                    txtAmount.Text = "";
                    txtORNo.Focus();
                    btnCancel.Text = "Close";
                }
            }
            else
                return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "&Cancel")
            {
                txtORNo.Text = "";
                txtAmount.Text = "";
                btnCancel.Text = "Close";
            }
            else
            {
                this.Close();
            }
        }

        private void txtORNo_Leave(object sender, EventArgs e)
        {
            if (txtORNo.Text.Trim() != "")
            {
                btnCancel.Text = "&Cancel";
            }
        }
    }
}