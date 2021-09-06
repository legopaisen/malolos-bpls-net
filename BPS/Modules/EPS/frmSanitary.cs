using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Modules.BusinessReports;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.EPS
{
    public partial class frmSanitary : Form
    {
        public frmSanitary()
        {
            InitializeComponent();
            bin.GetLGUCode = AppSettingsManager.GetConfigObject(ConfigurationAttributes.LGUCode);
            bin.GetDistCode = AppSettingsManager.GetConfigObject("11");
        }

        private string m_sORDate = "";
        private string m_sORNo = "";



        private void cmbSignatory_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void frmSanitary_Load(object sender, EventArgs e)
        {
            ClearFields();
            LoadSignatory();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (btnGenerate.Text == "&Generate")
            {
                if (!Checkifpayed())
                {
                    m_sORDate = "";
                    m_sORNo = "";
                    bin.txtTaxYear.Text = "";
                    bin.txtBINSeries.Text = "";
                    bin.txtTaxYear.Focus();
                    return;
                }
                LoadData();
                //GenerateControlNumber();
                btnGenerate.Text = "&Clear";
                btnPrint.Enabled = true;
            }
            else
            {
                ClearFields();
                m_sORDate = "";
                m_sORNo = "";
                bin.txtTaxYear.Text = "";
                bin.txtBINSeries.Text = "";
                btnGenerate.Text = "&Generate";
                btnPrint.Enabled = false;
            }
        }

        private void ClearFields()
        {
            cmbSignatory.Text = "";
            lblBnsName.Text = "";
            lblOwnerName.Text = "";
            lblAddress.Text = "";
        }


        //private void GenerateControlNumber()
        //{
        //    OracleResultSet pSet = new OracleResultSet();
        //    OracleResultSet pSet2 = new OracleResultSet();
        //    String sSeries = "";
        //    bool bCreateNew = false;

        //    //pSet.Query = "select * from eps_control_tbl where bin = '" + bin.GetBin() + "' and tax_year = '" + ConfigurationAttributes.CurrentYear + "' order by eps_control_series desc";
        //    //JARS 20190130 THIS VARIABLE IS SET IN THE FORM LOAD EVENT
        //    pSet.Query = "select * from SANITARY_CTRL_NO ";
        //    if (pSet.Execute())
        //        if (pSet.Read())
        //        {
        //            //JARS 20190109
        //            //if (MessageBox.Show("Create new conrol number? \nPrev. Control number: " + pSet.GetString(2), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //            //{
        //            //}
        //            //else
        //            //{
        //            //lblControlNum.Text = pSet.GetString("eps_control_series");
        //            //sSeries = pSet.GetString("eps_control_series");
        //            lblControlNum.Text = pSet.GetString(m_sControl_Table_Series);
        //            sSeries = pSet.GetString(m_sControl_Table_Series);
        //            lblDtIssued.Text = pSet.GetDateTime("dt_issued").ToString("MM/dd/yyyy");
        //            pSet.Close();
        //            return;
        //            //}
        //        }
        //        else
        //        {
        //            MessageBox.Show("No Control Number detected, system will generate Control Number");
        //            bCreateNew = true;
        //        }
        //    pSet.Close();

        //    if (bCreateNew) //JARS 20190109
        //    {
        //        sSeries = GenerateSeries();
        //        if (m_sClearanceMode == "Engineering")
        //        {
        //            sSeries = ConfigurationAttributes.CurrentYear + "-" + AppSettingsManager.GetCurrentDate().ToString("MM-") + sSeries;
        //        }
        //        else //JARS 20190312 NEW SERIES FOR ZONING CLEARANCE
        //        {
        //            sSeries = "DZC-" + ConfigurationAttributes.CurrentYear + "-" + sSeries;
        //        }
        //    }
        //}
        

        private bool Checkifpayed()
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from pay_hist where bin = '" + bin.GetBin() + "' and tax_year = '" + ConfigurationAttributes.CurrentYear + "'";
            if (pSet.Execute())
                if (!pSet.Read())
                {
                    MessageBox.Show("No payment for current year (" + ConfigurationAttributes.CurrentYear + ")", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    pSet.Close();
                    return false;
                }
                else
                {
                    m_sORNo = pSet.GetString(0);
                    m_sORDate = pSet.GetDateTime(6).ToString("MM/dd/yyyy");
                }
            pSet.Close();
            return true;
        }

        //AFM 20200103 merged from eps clearance. Modified for sanitary
        private void LoadData()
        {
            String sBin = bin.GetBin().Trim();
            OracleResultSet pSet = new OracleResultSet();

            lblBnsName.Text = AppSettingsManager.GetBnsName(sBin);
            lblOwnerName.Text = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(sBin));
            lblAddress.Text = AppSettingsManager.GetBnsAddress(sBin);  
        }

        private void LoadSignatory()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sSig = "";
            string sPos = "";

            pSet.Query = "select * from signatory order by sig_fn";
            cmbSignatory.Items.Add("");
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sSig = pSet.GetString("sig_fn") + " " + pSet.GetString("sig_mi") + " " + pSet.GetString("sig_ln");
                    sPos = pSet.GetString("sig_position");
                    sSig += "\\" + sPos;
                    cmbSignatory.Items.Add(sSig);
                }
            }
            pSet.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //AFM 20200107 sanitary printing module
            frmBussReport frmbussreport = new frmBussReport();
            OracleResultSet result = new OracleResultSet();
            int currYear = DateTime.Now.Year; //for expiration date (always end of year)
            string sDtMonth = dtpDtIssued.Value.ToString("MMMM dd").ToUpper();
            string sDtYear = dtpDtIssued.Value.Year.ToString();
            DateTime ditdit = dtpDtIssued.Value;
            string sSigName = string.Empty;
            string sSigPos = string.Empty;
            try
            {
                string[] arrSignatory = cmbSignatory.Text.Split('\\'); // split name and position
                sSigName = arrSignatory[0];
                sSigPos = arrSignatory[1];
            }
            catch { }

            frmbussreport.ReportSwitch = "SanitaryClearance";
            frmbussreport.ReportName = "SANITARY CLEARANCE";
            frmbussreport.Signatory = sSigName;
            frmbussreport.SigPosition = sSigPos;

            frmbussreport.m_sBussName = lblBnsName.Text;
            frmbussreport.m_sOwnName = lblOwnerName.Text;
            frmbussreport.m_sDtYear = sDtYear.ToString();
            frmbussreport.m_sYrExpiration = currYear.ToString();
            frmbussreport.m_sBussAdd = lblAddress.Text;
            frmbussreport.m_sDtMonth = sDtMonth;
            frmbussreport.BIN = bin.GetBin();
            frmbussreport.ShowDialog();

            result.Query = "insert into sanitary (bin, dt_issued, dt_saved) values (:1, :2, :3)";
            result.AddParameter(":1", bin.GetBin());
            result.AddParameter(":2", ditdit);
            result.AddParameter(":3", AppSettingsManager.GetSystemDate());

            result.ExecuteNonQuery();
            result.Close();

            if (AuditTrail.InsertTrail("AUSC", "ASS-UTIL-SANITARY CLEARANCE", "PRINT SANITARY CLEARANCE: " + bin.GetBin()) == 0)
            {
                // ZA WARUDO
            }
        }

        private void cmbSignatory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbSignatory_KeyPress(object sender, KeyPressEventArgs e) //AFM 20200107
        {
            if (char.IsLower(e.KeyChar))
            {
                e.KeyChar = Convert.ToChar(e.KeyChar.ToString().ToUpper());
                e.Handled = false;
            }
        }

    }
}