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
    public partial class frmClearance : Form
    {
        public frmClearance()
        {
            InitializeComponent();
            bin.GetLGUCode = AppSettingsManager.GetConfigObject(ConfigurationAttributes.LGUCode);
            bin.GetDistCode = AppSettingsManager.GetConfigObject("11");
        }

        private string m_sORDate = "";
        private string m_sORNo = "";

        //JARS 20190130
        private string m_sControl_Table = "";
        private string m_sSeries_Table = "";
        private string m_sControl_Table_Series = "";

        private int m_iId = 0; //JARS 20171118
        private int m_iId2 = 0; //AFM 20210817
        private string m_sSignatory = ""; //JARS 20171118
        private string m_sSignatory2 = ""; //AFM 20210817

        private string m_sClearanceMode = ""; //JARS 20190130 TO USE THIS MODULE FOR ENGINEERING AND ZONING CLEARANCE.
        public string ClearanceMode
        {
            get { return m_sClearanceMode; }
            set { m_sClearanceMode = value; }
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

                GenerateControlNumber();

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
                cmbSignatory.Text = "";
                cmbSignatory2.Text = "";
            }
        }

        private void LoadSignatory()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sSig = "";

            pSet.Query = "select * from signatory order by sig_fn";
            cmbSignatory.Items.Add("");
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sSig = pSet.GetString("sig_fn") + " " + pSet.GetString("sig_mi") + " " + pSet.GetString("sig_ln");
                    cmbSignatory.Items.Add(sSig);
                    cmbSignatory2.Items.Add(sSig); //AFM 20210817
                }
            }
            pSet.Close();
        }

        private void frmClearance_Load(object sender, EventArgs e)
        {
            ClearFields();
            
            //JARS 20190130
            if (m_sClearanceMode == "Engineering")
            {
                m_sControl_Table = "eps_control_tbl";
                m_sSeries_Table = "eps_control_series";
                m_sControl_Table_Series = "eps_control_series";
                this.Text = "Engineering Clearance";
                lblSignatory.Visible = false;
                cmbSignatory.Visible = false;
                cmbSignatory2.Visible = false;
            }
            else if(m_sClearanceMode == "Zoning")
            {
                m_sControl_Table = "zoning_control_tbl";
                m_sSeries_Table = "zoning_control_series";
                m_sControl_Table_Series = "zon_control_series";
                this.Text = "Zoning Clearance";

                //this.txtClassification.Visible = true; //AFM 20210817 MAO-21-15525 disabled
                //this.lblClass.Visible = true;
                cmbSignatory.Visible = true;
                cmbSignatory2.Visible = true;
                lblSignatory.Visible = true;
                lblSignatory2.Visible = true;
                LoadSignatory();
            }
        }

        private void ClearFields()
        {
            foreach (Label c in this.gbInfo.Controls)
            {
                if (c is Label)
                    if (c.Name.Contains("lbl"))
                        ((Label)c).Text = "";
            }
            txtClassification.Text = "";
        }

        private void LoadData()
        {
            String sBin = bin.GetBin().Trim();
            OracleResultSet pSet = new OracleResultSet();

            lblStatus.Text = AppSettingsManager.GetBnsStat(sBin);
            lblBnsName.Text = AppSettingsManager.GetBnsName(sBin);
            lblOwnerName.Text = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(sBin));

            if (m_sClearanceMode == "Zoning")
            {
                pSet.Query = "select zone_class from zoning_control_tbl where bin = '"+ sBin +"'";
                if(pSet.Execute())
                {
                    if(pSet.Read())
                    {
                        txtClassification.Text = pSet.GetString("zone_class").Trim();
                    }
                }
                pSet.Close();
            }

        }

        private void GenerateControlNumber()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();
            String sSeries = "";
            bool bCreateNew = false;

            //pSet.Query = "select * from eps_control_tbl where bin = '" + bin.GetBin() + "' and tax_year = '" + ConfigurationAttributes.CurrentYear + "' order by eps_control_series desc";
            //JARS 20190130 THIS VARIABLE IS SET IN THE FORM LOAD EVENT
            pSet.Query = "select * from "+ m_sControl_Table +" where bin = '" + bin.GetBin() + "' and tax_year = '" + ConfigurationAttributes.CurrentYear + "' order by "+ m_sControl_Table_Series +" desc";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    //JARS 20190109
                    //if (MessageBox.Show("Create new conrol number? \nPrev. Control number: " + pSet.GetString(2), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    //{
                    //}
                    //else
                    //{
                        //lblControlNum.Text = pSet.GetString("eps_control_series");
                        //sSeries = pSet.GetString("eps_control_series");
                        lblControlNum.Text = pSet.GetString(m_sControl_Table_Series);
                        sSeries = pSet.GetString(m_sControl_Table_Series);
                        lblDtIssued.Text = pSet.GetDateTime("dt_issued").ToString("MM/dd/yyyy");
                        pSet.Close();
                        return;
                    //}
                }
                else
                {
                    MessageBox.Show("No Control Number detected, system will generate Control Number");
                    bCreateNew = true;
                }
            pSet.Close();

            if (bCreateNew) //JARS 20190109
            {
                sSeries = GenerateSeries();
                if (m_sClearanceMode == "Engineering")
                {
                    sSeries = ConfigurationAttributes.CurrentYear + "-" + AppSettingsManager.GetCurrentDate().ToString("MM-") + sSeries;
                }
                else //JARS 20190312 NEW SERIES FOR ZONING CLEARANCE
                {
                    //sSeries = "DZC-" + ConfigurationAttributes.CurrentYear + "-" + sSeries;
                    sSeries = ConfigurationAttributes.CurrentYear + "-" + sSeries; //AFM 20210817 MAO-21-15525 new control # format
                }
            }

            if (MessageBox.Show("Are you sure want to create this clearance?\nControl number: " + sSeries, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //pSet.Query = "insert into eps_control_tbl values (:1,:2,:3,sysdate,sysdate)";
                //JARS 20190130
                if (m_sClearanceMode == "Engineering")
                {
                    pSet.Query = "insert into " + m_sControl_Table + " values (:1,:2,:3,sysdate,sysdate)";
                    pSet.AddParameter(":1", bin.GetBin());
                    pSet.AddParameter(":2", ConfigurationAttributes.CurrentYear);
                    pSet.AddParameter(":3", sSeries);
                    pSet.ExecuteNonQuery();
                }
                else
                {
                    pSet.Query = "insert into " + m_sControl_Table + " values (:1,:2,:3,sysdate,sysdate,:4)";
                    pSet.AddParameter(":1", bin.GetBin());
                    pSet.AddParameter(":2", ConfigurationAttributes.CurrentYear);
                    pSet.AddParameter(":3", sSeries);
                    pSet.AddParameter(":4", "");
                    pSet.ExecuteNonQuery();
                }
                
                lblControlNum.Text = sSeries;
                lblDtIssued.Text = AppSettingsManager.GetCurrentDate().ToString("MM/dd/yyyy");

                if (m_sClearanceMode == "Engineering") //JARS 20190130
                {
                    if (AuditTrail.InsertTrail("AUECR-G", "eps_control_tbl", bin.GetBin() + " " + sSeries) == 0)
                    {
                        pSet.Rollback();
                        pSet.Close();
                        MessageBox.Show(pSet.ErrorDescription, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (AuditTrail.InsertTrail("AZZC", "zon_control_tbl", bin.GetBin() + " " + sSeries) == 0)
                    {
                        pSet.Rollback();
                        pSet.Close();
                        MessageBox.Show(pSet.ErrorDescription, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                MessageBox.Show("Created Successfully!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GenerateSeries()
        {
            string sValue = "";
            OracleResultSet pSet = new OracleResultSet();
            //pSet.Query = "select * from eps_control_series where year = '" + ConfigurationAttributes.CurrentYear + "'";
            //JARS 20190130
            pSet.Query = "select * from "+ m_sSeries_Table +" where year = '" + ConfigurationAttributes.CurrentYear + "'";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    int iTmp = 0;
                    iTmp = Convert.ToInt32(pSet.GetString(1));
                    iTmp++;
                    if (m_sClearanceMode == "Engineering" || m_sClearanceMode == "Zoning") //JARS 20190130 //AFM 20210817 MAO-21-15525 added zoning - new zoning series format
                    {
                        sValue = iTmp.ToString("0000");
                    }
                    //else
                    //{
                    //    sValue = iTmp.ToString("00000"); //JARS 20190311 5 DIGITS FOR ZONING
                    //}

                    //pSet.Query = "update eps_control_series set control_series = '" + iTmp.ToString() + "'  where year = '" + ConfigurationAttributes.CurrentYear + "'";
                    //JARS 20190130
                    pSet.Query = "update "+ m_sSeries_Table +" set control_series = '" + iTmp.ToString() + "'  where year = '" + ConfigurationAttributes.CurrentYear + "'";
                    pSet.ExecuteNonQuery();
                }
                else
                {
                    pSet.Close();

                    sValue = "1";
                    //pSet.Query = "insert into eps_control_series values ('" + ConfigurationAttributes.CurrentYear + "','" + sValue + "')";
                    //JARS 20190130
                    pSet.Query = "insert into "+ m_sSeries_Table +" values ('" + ConfigurationAttributes.CurrentYear + "','" + sValue + "')";
                    pSet.ExecuteNonQuery();
                    if (m_sClearanceMode == "Engineering" || m_sClearanceMode == "Zoning") //JARS 20190130 //AFM 20210817 MAO-21-15525 added zoning - new zoning series format
                    {
                        sValue = "0001";
                    }
                    //else
                    //{
                    //    sValue = "00001";  //JARS 20190311 5 DIGITS FOR ZONING
                    //}

                }
            pSet.Close();
            return sValue;
        }

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

        private bool ValidateSignatories() //AFM 20210817
        {
            if (string.IsNullOrEmpty(cmbSignatory.Text) && string.IsNullOrEmpty(cmbSignatory2.Text))
            {
                MessageBox.Show("No signatories selected!","", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            else
                return true;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            if (m_sClearanceMode == "Zoning")
            {
                if (!ValidateSignatories())
                    return;

                pSet.Query = "select * from zoning_control_tbl where bin = '" + bin.GetBin() + "' and tax_year = '" + ConfigurationAttributes.CurrentYear + "'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        pSet2.Query = "update zoning_control_tbl set zone_class = '" + txtClassification.Text.ToUpper().Trim() + "' where bin = '" + bin.GetBin() + "'";
                        pSet2.ExecuteNonQuery();
                    }
                }
                pSet.Close();
            }

            frmBussReport frmbussreport = new frmBussReport();
            if (m_sClearanceMode == "Engineering") //JARS 20190130
            {
                frmbussreport.ReportSwitch = "EPSClearance";
                frmbussreport.ReportName = "ENGINEERING CLEARANCE";
            }
            else if(m_sClearanceMode == "Zoning")
            {
                frmbussreport.ReportSwitch = "ZoningClearance";
                frmbussreport.ReportName = "LOCATIONAL CLEARANCE";
                frmbussreport.ZoneClassification = txtClassification.Text.Trim();
                frmbussreport.Signatory = m_sSignatory;
                frmbussreport.Signatory2 = m_sSignatory2;
                frmbussreport.Id = m_iId;
                frmbussreport.Id2 = m_iId2;
            }
            frmbussreport.m_sControlNo = lblControlNum.Text;
            frmbussreport.m_sApplicantStatus = lblStatus.Text;
            frmbussreport.m_sBussName = lblBnsName.Text;
            frmbussreport.m_sOwnName = lblOwnerName.Text;
            frmbussreport.m_sDateIssued = lblDtIssued.Text;
            frmbussreport.BIN = bin.GetBin();
            frmbussreport.ORDate = m_sORDate;
            frmbussreport.ORNo = m_sORNo;
            frmbussreport.ShowDialog();
        }

        private void cmbSignatory_SelectedIndexChanged(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from signatory where sig_fn||' '||sig_mi||' '||sig_ln = '" + cmbSignatory.Text.Trim() + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    m_sSignatory = cmbSignatory.Text.Trim();
                    m_iId = pSet.GetInt("ID");
                }
            }
            pSet.Close();
        }

        private void cmbSignatory2_SelectedIndexChanged(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from signatory where sig_fn||' '||sig_mi||' '||sig_ln = '" + cmbSignatory2.Text.Trim() + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    m_sSignatory2 = cmbSignatory2.Text.Trim();
                    m_iId2 = pSet.GetInt("ID");
                }
            }
            pSet.Close();
        }
    }
}