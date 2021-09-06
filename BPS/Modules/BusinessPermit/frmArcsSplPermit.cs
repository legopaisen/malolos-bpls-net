
// RMC 20171124 customized special permit printing where payment was made at aRCS

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.BusinessPermit
{
    public partial class frmArcsSplPermit : Form
    {
        private string[] m_sArrayInfo = new string[] { "" };

        public frmArcsSplPermit()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();

            // RMC 20171206 adjust printing of for compliance in temp permit (s)
            if (txtOrNo.Text.Trim() == "")
            {
                MessageBox.Show("Enter O.R. No.", "Special Permit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            // RMC 20171206 adjust printing of for compliance in temp permit (e)
            
            if (btnSearch.Text == "Search")
            {
                btnSearch.Text = "Clear";

                pRec.CreateNewConnectionARCS();

                pRec.Query = "select payer_code, sum(fees_amt_due), or_date from payments_info where or_no = '" + txtOrNo.Text.Trim() + "' group by payer_code, or_date";
                if (pRec.Execute())
                {
                    if (pRec.Read())
                    {
                        txtBNSOwner.Text = GetOwnName(pRec.GetString("payer_code"));
                        txtOrAmt.Text = string.Format("{0:#,0##.00}",pRec.GetDouble(1));
                        txtOrDate.Text = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime(2));

                        GetAdditionalInfo();
                        
                    }
                }
                pRec.Close();
            }
            else
            {
                btnSearch.Text = "Search";
                ClearControls();
            }
        }

        private void ClearControls()
        {
            txtOrNo.Focus();
            txtOrNo.Text = "";
            txtOrAmt.Text = "";
            txtOrDate.Text = "";
            txtBNSOwner.Text = "";
            txtBNSName.Text = "";
            txtAddr.Text = "";
            txtEvent.Text = "";
            txtDate.Text = "";
            txtPlace.Text = "";
            txtValidity.Text = "";
            EnableControls(false);
        }

        private string GetOwnName(string sOwnCode)
        {
            OracleResultSet pRec = new OracleResultSet();
            pRec.CreateNewConnectionARCS();
            string sOwnName = string.Empty;

            pRec.Query = "select * from payers where payer_code = '" + sOwnCode + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    if (pRec.GetString("payer_fn").Trim() != "")
                        sOwnName += pRec.GetString("payer_fn").Trim();
                    if(pRec.GetString("payer_mi").Trim() != "")
                        sOwnName += " " + pRec.GetString("payer_mi").Trim();
                    if(pRec.GetString("payer_ln").Trim() != "")
                        sOwnName += " " + pRec.GetString("payer_ln").Trim();
                }
            }
            pRec.Close();

            return sOwnName;
        }

        private void EnableControls(bool blnEnable)
        {
            txtBNSOwner.ReadOnly = !blnEnable;
            txtBNSName.ReadOnly = !blnEnable;
            txtAddr.ReadOnly = !blnEnable;
            txtEvent.ReadOnly = !blnEnable;
            txtDate.ReadOnly = !blnEnable;
            txtPlace.ReadOnly = !blnEnable;
            txtValidity.ReadOnly = !blnEnable;
        }

        private void frmArcsSplPermit_Load(object sender, EventArgs e)
        {
            txtOrNo.Focus();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Print Permit?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OracleResultSet pCmd = new OracleResultSet();

                pCmd.Query = "select * from spl_arcs_permit where arcs_or_no = '" + txtOrNo.Text + "'";
                if (pCmd.Execute())
                {
                    if (!pCmd.Read())
                    {
                        pCmd.Close();
                        pCmd.Query = "insert into spl_arcs_permit values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12)";
                        pCmd.AddParameter(":1", txtOrNo.Text);
                        pCmd.AddParameter(":2", Convert.ToDouble(txtOrAmt.Text));
                        pCmd.AddParameter(":3", txtOrDate.Text);
                        pCmd.AddParameter(":4", StringUtilities.HandleApostrophe(txtBNSOwner.Text));
                        pCmd.AddParameter(":5", StringUtilities.HandleApostrophe(txtBNSName.Text.Trim()));
                        pCmd.AddParameter(":6", StringUtilities.HandleApostrophe(txtAddr.Text.Trim()));
                        pCmd.AddParameter(":7", StringUtilities.HandleApostrophe(txtEvent.Text.Trim()));
                        pCmd.AddParameter(":8", StringUtilities.HandleApostrophe(txtDate.Text.Trim()));
                        pCmd.AddParameter(":9", StringUtilities.HandleApostrophe(txtPlace.Text.Trim()));
                        pCmd.AddParameter(":10", StringUtilities.HandleApostrophe(txtValidity.Text.Trim()));
                        pCmd.AddParameter(":11", StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode));
                        pCmd.AddParameter(":12", AppSettingsManager.GetCurrentDate());
                        if (pCmd.ExecuteNonQuery() == 0)
                        { };
                    }
                }
                pCmd.Close();

                m_sArrayInfo = new string[10];

                m_sArrayInfo[0] = txtBNSName.Text.Trim();
                m_sArrayInfo[1] = txtBNSOwner.Text.Trim();
                m_sArrayInfo[2] = txtAddr.Text.Trim();
                m_sArrayInfo[3] = txtEvent.Text.Trim();
                m_sArrayInfo[4] = txtDate.Text.Trim();
                m_sArrayInfo[5] = txtPlace.Text.Trim();
                m_sArrayInfo[6] = txtValidity.Text.Trim();
                m_sArrayInfo[7] = txtOrNo.Text;
                m_sArrayInfo[8] = txtOrDate.Text;
                m_sArrayInfo[9] = txtOrAmt.Text;

                frmPermitNew form = new frmPermitNew();
                form.ReportSwitch = "Special Permit";
                form.ArrayInfo = m_sArrayInfo;
                form.Show();

                EnableControls(false);
                ClearControls();
            }
        }

        private void GetAdditionalInfo()
        {
            OracleResultSet pRec = new OracleResultSet();

            pRec.Query = "select * from spl_arcs_permit where arcs_or_no = '" + txtOrNo.Text + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    EnableControls(false);

                    txtBNSName.Text = pRec.GetString("BNS_NM");
                    txtAddr.Text = pRec.GetString("BNS_ADDR");
                    txtEvent.Text = pRec.GetString("BNS_EVENT");
                    txtDate.Text = pRec.GetString("BNS_DATE");
                    txtPlace.Text = pRec.GetString("BNS_PLACE");
                    txtValidity.Text = pRec.GetString("BNS_VALIDITY");

                    btnEdit.Visible = true;
                }
                else
                {
                    EnableControls(true);
                    txtBNSName.Focus();
                    btnEdit.Visible = false;
                }
            }
            pRec.Close();

            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (btnEdit.Text == "Edit")
            {
                EnableControls(true);
                btnEdit.Text = "Update";
            }
            else
            {
                if (MessageBox.Show("Update information?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleResultSet pCmd = new OracleResultSet();

                    pCmd.Query = "delete from spl_arcs_permit where arcs_or_no = '" + txtOrNo.Text + "'";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    pCmd.Query = "insert into spl_arcs_permit values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12)";
                    pCmd.AddParameter(":1", txtOrNo.Text);
                    pCmd.AddParameter(":2", Convert.ToDouble(txtOrAmt.Text));
                    pCmd.AddParameter(":3", txtOrDate.Text);
                    pCmd.AddParameter(":4", StringUtilities.HandleApostrophe(txtBNSOwner.Text));
                    pCmd.AddParameter(":5", StringUtilities.HandleApostrophe(txtBNSName.Text.Trim()));
                    pCmd.AddParameter(":6", StringUtilities.HandleApostrophe(txtAddr.Text.Trim()));
                    pCmd.AddParameter(":7", StringUtilities.HandleApostrophe(txtEvent.Text.Trim()));
                    pCmd.AddParameter(":8", StringUtilities.HandleApostrophe(txtDate.Text.Trim()));
                    pCmd.AddParameter(":9", StringUtilities.HandleApostrophe(txtPlace.Text.Trim()));
                    pCmd.AddParameter(":10", StringUtilities.HandleApostrophe(txtValidity.Text.Trim()));
                    pCmd.AddParameter(":11", StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode));
                    pCmd.AddParameter(":12", AppSettingsManager.GetCurrentDate());
                    if (pCmd.ExecuteNonQuery() == 0)
                    { };

                    MessageBox.Show("Information updated", "Special Permit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                EnableControls(false);
                btnEdit.Text = "Edit";
            }
        }

    }
}