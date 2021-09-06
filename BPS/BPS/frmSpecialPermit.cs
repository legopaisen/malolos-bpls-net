using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.Reports;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.SearchBusiness;
using Amellar.Common.TransactionLog;
using Amellar.Common.AuditTrail;

namespace BPLSBilling
{
    public partial class frmSpecialPermit : Form
    {
        private DateTime m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
        private string m_sBnsStat = string.Empty;   // RMC 20170822 added transaction log feature for tracking of transactions per bin
        private string m_sTaxYear = string.Empty;   // RMC 20170822 added transaction log feature for tracking of transactions per bin

        public frmSpecialPermit()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSpecialPermit_Load(object sender, EventArgs e)
        {
            bin1.txtLGUCode.Text = AppSettingsManager.GetConfigValue("10");
            bin1.txtDistCode.Text = AppSettingsManager.GetConfigValue("11");
        }

        private void RetrieveRecords()
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from spl_businesses where bin = '" + bin1.txtLGUCode.Text.Trim() + "-" + bin1.txtDistCode.Text.Trim() + "-" + txtBinYear.Text.Trim() + "-" + txtBinSeries.Text.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    txtBnsName.Text = result.GetString("bns_nm");
                    m_sBnsStat = result.GetString("bns_stat");  // RMC 20170822 added transaction log feature for tracking of transactions per bin
                    m_sTaxYear = result.GetString("tax_year");  // RMC 20170822 added transaction log feature for tracking of transactions per bin

                    btnGenerate.Enabled = true;
                }
                else
                    btnGenerate.Enabled = false;
            }
            result.Close();

            result.Query = "select * from spl_permit_data where bin = '" + bin1.txtLGUCode.Text.Trim() + "-" + bin1.txtDistCode.Text.Trim() + "-" + txtBinYear.Text.Trim() + "-" + txtBinSeries.Text.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    txtSPLNo.Text = result.GetString("spl_no");
                    txtPermitTo.Text = result.GetString("spl_note");
                    dtpExpDate.Value = result.GetDateTime("exp_date");

                    btnGenerate.Text = "Print";
                }
                else
                {
                    btnGenerate.Text = "Generate..";
                }
            }
            result.Close();

            m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            frmSearchBusiness frmSearchBns = new frmSearchBusiness();
            frmSearchBns.ModuleCode = "SPL_PERMIT";
            if (btnSearch.Text == "Clear")
            {
                this.btnClose.Text = "Close";
            }
            else
            {
                if (txtBinYear.Text.Trim() == "" && txtBinSeries.Text.Trim() == "")
                {
                    frmSearchBns.ShowDialog();

                    if (frmSearchBns.sBIN.Length > 1)
                    {
                        txtBinYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                        txtBinSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
                        RetrieveRecords();
                    }
                }
                else
                {
                    pSet.Query = "select * from spl_businesses where bin = '" + bin1.txtLGUCode.Text.Trim() + "-" + bin1.txtDistCode.Text.Trim() + "-" + txtBinYear.Text.Trim() + "-" + txtBinSeries.Text.Trim() + "'";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            RetrieveRecords();
                        }
                        else
                        {
                            frmSearchBns.ShowDialog();

                            if (frmSearchBns.sBIN.Length > 1)
                            {
                                txtBinYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                                txtBinSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
                                RetrieveRecords();
                            }
                            else
                            {
                                txtBinYear.Text = "";
                                txtBinSeries.Text = "";
                            }
                        }
                    }
                    pSet.Close();
                }
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string sOrNo = string.Empty;
            string sOrdate = string.Empty;
            string sOrAmount = string.Empty;
            DateTime dDate = DateTime.Now;
            string sBin = string.Empty;
            double dOrAmount = 0;
            sBin = bin1.txtLGUCode.Text.Trim() + "-" + bin1.txtDistCode.Text.Trim() + "-" + txtBinYear.Text.Trim() + "-" + txtBinSeries.Text.Trim();

            if (btnGenerate.Text == "Generate..")
            {
                //if (txtBnsName.Text.Trim() == string.Empty)
                //{
                //    MessageBox.Show("Bns Name required.");
                //    return;
                //}

                if (txtPermitTo.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Permit classification required.");
                    return;
                }

                if (string.Format("{0:yyy-MM-dd}", dtpExpDate.Value) == string.Format("{0:yyy-MM-dd}", DateTime.Now))
                {
                    MessageBox.Show("Please update expiration date.");
                    return;
                }

                OracleResultSet result = new OracleResultSet();



                result.Query = "select distinct * from pay_hist where bin = '" + sBin + "'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        sOrNo = result.GetString("or_no");
                        dDate = result.GetDateTime("or_date");
                        sOrdate = string.Format("{0:MM/dd/yy}", dDate);
                    }
                }
                result.Close();

                result.Query = "select sum(fees_amtdue) as fAmount from or_table where or_no = '" + sOrNo + "'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        try
                        {
                            dOrAmount = result.GetDouble("fAmount");
                        }
                        catch
                        {
                            dOrAmount = 0;
                        }
                            
                        sOrAmount = string.Format("{0:#,##0.#0}", dOrAmount);
                        
                    }
                }
                result.Close();
                
                txtSPLNo.Text = GetNextSPLNo();
                result.Query = "insert into spl_permit_data values (:1, :2, :3, :4, :5, :6, :7)";
                result.AddParameter(":1", sBin);
                result.AddParameter(":2", txtSPLNo.Text.Trim());
                result.AddParameter(":3", dtpExpDate.Value);
                result.AddParameter(":4", sOrNo);
                result.AddParameter(":5", dDate);
                result.AddParameter(":6", dOrAmount);
                result.AddParameter(":7", txtPermitTo.Text.Trim());
                if (result.ExecuteNonQuery() != 0)
                { }
                result.Close();

                // RMC 20170822 added transaction log feature for tracking of transactions per bin (s)
                TransLog.UpdateLog(sBin, m_sBnsStat, m_sTaxYear, "ABBP", m_dTransLogIn, AppSettingsManager.GetSystemDate());

                if (AuditTrail.InsertTrail("ABBP", "spl_permit_data", "Generate Special Permit of BIN: " + sBin + "for tax year " + m_sTaxYear) == 0)
                {
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // RMC 20170822 added transaction log feature for tracking of transactions per bin (e)

                btnGenerate.Text = "Print";
                ReportClass rClass = new ReportClass();
                rClass.SpecialPermit(txtSPLNo.Text.Trim());
                
            }
            else
            {
                OracleResultSet result = new OracleResultSet();
                result.Query = "select distinct * from pay_hist where bin = '" + sBin + "'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        sOrNo = result.GetString("or_no");
                        dDate = result.GetDateTime("or_date");
                        sOrdate = string.Format("{0:MM/dd/yy}", dDate);
                    }
                }
                result.Close();

                result.Query = "select sum(fees_amtdue) as fAmount from or_table where or_no = '" + sOrNo + "'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        try
                        {
                            dOrAmount = result.GetDouble("fAmount");
                        }
                        catch
                        {
                            dOrAmount = 0;
                        }

                        sOrAmount = string.Format("{0:#,##0.#0}", dOrAmount);

                    }
                }
                result.Close();


               
                result.Query = "delete from spl_permit_data where spl_no = :1";
                result.AddParameter(":1", txtSPLNo.Text.Trim());
                if (result.ExecuteNonQuery() != 0)
                { }
                result.Close();

                result.Query = "insert into spl_permit_data values (:1, :2, :3, :4, :5, :6, :7)";
                result.AddParameter(":1", sBin);
                result.AddParameter(":2", txtSPLNo.Text.Trim());
                result.AddParameter(":3", dtpExpDate.Value);
                result.AddParameter(":4", sOrNo);
                result.AddParameter(":5", dDate);
                result.AddParameter(":6", dOrAmount);
                result.AddParameter(":7", txtPermitTo.Text.Trim());
                if (result.ExecuteNonQuery() != 0)
                { }
                result.Close();
                ReportClass rClass = new ReportClass();
                rClass.SpecialPermit(txtSPLNo.Text.Trim());
            }
        }

        private string GetNextSPLNo()
        {
            OracleResultSet result = new OracleResultSet();
            string sResult = string.Empty;
            int iSeries = 0;
            result.Query = "select * FROM SPL_PERMIT";
            if (result.Execute())
            {
                if (result.Read())
                {
                    try
                    {
                        iSeries = result.GetInt("spl_series");
                    }
                    catch
                    {
                        iSeries = 1;
                    }
                }
            }
            result.Close();

            sResult = string.Format("{0:yy-MM}", DateTime.Now) + "-" + string.Format("{0:00000}", iSeries);

            result.Query = "update spl_permit set spl_series = :1";
            result.AddParameter(":1", iSeries + 1);
            if (result.ExecuteNonQuery() != 0)
            { }
            result.Close();
            return sResult;
        }

        private void txtBinYear_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtBinYear.Text.Trim().Length == 4)
                    txtBinSeries.Focus();
            }
            catch
            { }
        }

        private void txtBinSeries_Leave(object sender, EventArgs e)
        {
            int iCount = 0;
            iCount = txtBinSeries.TextLength;

            switch (iCount)
            {
                case 1:
                    {
                        txtBinSeries.Text = "000000" + txtBinSeries.Text;
                        break;
                    }
                case 2:
                    {
                        txtBinSeries.Text = "00000" + txtBinSeries.Text;
                        break;
                    }
                case 3:
                    {
                        txtBinSeries.Text = "0000" + txtBinSeries.Text;
                        break;
                    }
                case 4:
                    {
                        txtBinSeries.Text = "000" + txtBinSeries.Text;
                        break;
                    }
                case 5:
                    {
                        txtBinSeries.Text = "00" + txtBinSeries.Text;
                        break;
                    }
                case 6:
                    {
                        txtBinSeries.Text = "0" + txtBinSeries.Text;
                        break;
                    }
                case 7:
                    {
                        txtBinSeries.Text = txtBinSeries.Text;
                        break;
                    }

            }
        }
    }
}