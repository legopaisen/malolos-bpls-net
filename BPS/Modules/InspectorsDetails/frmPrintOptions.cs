using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.InspectorsDetails
{
    public partial class frmPrintOptions : Form
    {
        OracleResultSet pSet = new OracleResultSet();
        // private PrintInspection PrintClass = null;    PrintClass = new frmPrintInspection();  // RMC 20171201 modified printing of Inspection report and Notices, changed to vsprinter,put rem
        private string m_strOption = string.Empty;
        private string m_sSource = string.Empty;
        
        public string Option
        {
            get { return m_strOption; }
            set { m_strOption = value; }
        }

        public string Source
        {
            get { return m_sSource; }
            set { m_sSource = value; }
        }

        public frmPrintOptions()
        {
            InitializeComponent();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
        }

        private void frmPrintOptions_Load(object sender, EventArgs e)
        {
            // RMC 20171116 added violation report (s)
            this.label1.Location = new System.Drawing.Point(26, 102);
            this.label2.Location = new System.Drawing.Point(26, 131);
            this.label3.Location = new System.Drawing.Point(142, 130);
            this.dtpDateFrom.Location = new System.Drawing.Point(59, 124);
            this.dtpDateTo.Location = new System.Drawing.Point(164, 125);
            // RMC 20171116 added violation report (e)

            if (m_sSource == "UNOFFICIAL")
            {
                txtIS.Visible = true;
                bin1.Visible = false;
                rdoBin.Text = "IS No.";
            }
            else if (m_sSource == "VIOLATION REPORT")   // RMC 20171116 added violation report
            {
                rdoBin.Visible = false;
                rdoInspector.Visible = false;
                cmbInspector.Visible = false;
                txtIS.Visible = false;
                bin1.Visible = false;

                this.label1.Location = new System.Drawing.Point(26, 66);
                this.label2.Location = new System.Drawing.Point(26, 95);
                this.label3.Location = new System.Drawing.Point(142, 94);
                this.dtpDateFrom.Location = new System.Drawing.Point(59, 88);
                this.dtpDateTo.Location = new System.Drawing.Point(164, 89);
            }
            else
            {
                txtIS.Visible = false;
                bin1.Visible = true;
                rdoBin.Text = "BIN";
            }

            rdoInspector.Checked = true;
            LoadInspectors();
        }

        private void rdoInspector_CheckedChanged(object sender, EventArgs e)
        {
            if (m_sSource == "UNOFFICIAL")
            {
                if (rdoInspector.Checked)
                {
                    cmbInspector.Enabled = true;
                    cmbInspector.Focus();
                    txtIS.Enabled = false;
                    dtpDateFrom.Enabled = true;
                    dtpDateTo.Enabled = true;
                }
            }
            else
            {
                if (rdoInspector.Checked)
                {
                    cmbInspector.Enabled = true;
                    cmbInspector.Focus();
                    bin1.Enabled = false;
                    rdoBin.Checked = false;
                }
            }
            
        }

        private void rdoBin_CheckedChanged(object sender, EventArgs e)
        {
            if (m_sSource == "UNOFFICIAL")
            {
                if (rdoBin.Checked)
                {
                    cmbInspector.Enabled = false;
                    txtIS.Enabled = true;
                    dtpDateFrom.Enabled = false;
                    dtpDateTo.Enabled = false;
                    txtIS.Focus();
                }
            }
            else
            {
                if (rdoBin.Checked)
                {
                    bin1.Enabled = true;
                    rdoInspector.Checked = false;
                    bin1.txtTaxYear.Focus();
                }
            }

        }

        private void LoadInspectors()
        {
            pSet.Query = "select inspector_code from inspector order by inspector_code";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    cmbInspector.Items.Add(pSet.GetString(0).Trim());
                }
            }
            pSet.Close();

            // RMC 20110901 Added auto-tagging if Lessor is not a registered business (s)
            pSet.Query = "select distinct inspector_code from unofficial_dtls where inspector_code not in ";
            pSet.Query += " (select inspector_code from inspector) order by inspector_code";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    cmbInspector.Items.Add(pSet.GetString(0).Trim());
                }
            }
            pSet.Close();
            // RMC 20110901 Added auto-tagging if Lessor is not a registered business (e)

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (m_sSource == "UNOFFICIAL")
            {
                if (rdoInspector.Checked)
                {
                    OnGenerateInspectorUnOfficial();
                }
                else
                {
                    OnGenerateIS();
                }
            }
            else if (m_sSource == "VIOLATION REPORT")   // RMC 20171115 Added history of untagged violations
            {
                OnGenerateViolation();
            }
            else
            {
                if (rdoInspector.Checked)
                {
                    OnGenerateInspector();
                }
                else
                {
                    OnGenerateBin();
                }
            }
        }

        private void OnGenerateInspector()
        {
            string strInspectorCode = string.Empty;
            string strDateFrom = string.Empty;
            string strDateTo = string.Empty;

            strInspectorCode = cmbInspector.Text.Trim();
            strDateFrom = string.Format("{0:MM/dd/yyyy}", dtpDateFrom.Value);
            strDateTo = string.Format("{0:MM/dd/yyyy}", dtpDateTo.Value);

            pSet.Query = string.Format("select * from inspector_details where inspector_code = '{0}' ", strInspectorCode);
            pSet.Query += string.Format(" and to_date(date_inspected,'MM/dd/yyyy') between to_date('{0}','MM/dd/yyyy') and to_date('{1}','MM/dd/yyyy')", strDateFrom, strDateTo);
            pSet.Query += string.Format(" and is_option = '{0}' and is_settled = 'N' order by date_inspected,bin", m_strOption);    // RMC 20110816 added is_settled = 'N'
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    //PrintClass = new PrintInspection();
                    frmPrintInspection PrintClass = new frmPrintInspection();  // RMC 20171201 modified printing of Inspection report and Notices, changed to vsprinter
                    PrintClass.Source = "1";
                    PrintClass.ReportName = "INSPECTOR DETAILS";
                    PrintClass.Inspector = GetInspectorName(strInspectorCode, "inspector");
                    PrintClass.Position = GetInspectorName(strInspectorCode, "position");
                    PrintClass.DateCover = "From " + strDateFrom + " to " + strDateTo;
                    PrintClass.Query = pSet.Query.ToString();
                    //PrintClass.FormLoad();
                    PrintClass.ShowDialog();    // RMC 20171201 modified printing of Inspection report and Notices, changed to vsprinter
                }
                else
                {
                    MessageBox.Show("No record found.");
                    return;
                }
            }
            pSet.Close();
            
        }

        private void OnGenerateBin()
        {
            OracleResultSet pRec = new OracleResultSet();

            string strInspectorCode = string.Empty;
            string strDateFrom = string.Empty;
            string strDateTo = string.Empty;

            strInspectorCode = cmbInspector.Text.Trim();
            strDateFrom = string.Format("{0:MM/dd/yyyy}", dtpDateFrom.Value);
            strDateTo = string.Format("{0:MM/dd/yyyy}", dtpDateTo.Value);

            if (strInspectorCode == "")
            {
                MessageBox.Show("Select inspector");
                return;
            }

            if (bin1.txtBINSeries.Text.Trim() == "" || bin1.txtBINSeries.Text.Trim() == "%")
            {
                pSet.Query = string.Format("select * from inspector_details where inspector_code = '{0}' ", strInspectorCode);
                pSet.Query += string.Format(" and to_date(date_inspected,'MM/dd/yyyy') between to_date('{0}','MM/dd/yyyy') and to_date('{1}','MM/dd/yyyy')", strDateFrom, strDateTo);
                pSet.Query += string.Format(" and is_option = '{0}' and is_settled = 'N' order by date_inspected", m_strOption);    // RMC 20110816 added is_settled = 'N'
            }
            else
            {
                pSet.Query = string.Format("select * from inspector_details where inspector_code = '{0}' and bin = '{1}'", strInspectorCode, bin1.GetBin());
                pSet.Query += string.Format(" and to_date(date_inspected,'MM/dd/yyyy') between to_date('{0}','MM/dd/yyyy') and to_date('{1}','MM/dd/yyyy')", strDateFrom, strDateTo);
                pSet.Query += string.Format(" and is_option = '{0}' and is_settled = 'N' order by date_inspected", m_strOption);    // RMC 20110816 added is_settled = 'N'
            }
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    //PrintClass = new PrintInspection();
                    frmPrintInspection PrintClass = new frmPrintInspection();  // RMC 20171201 modified printing of Inspection report and Notices, changed to vsprinter
                    PrintClass.Source = "2";
                    PrintClass.ReportName = "INSPECTION SERVICES";
                    PrintClass.Inspector = GetInspectorName(strInspectorCode, "inspector");
                    PrintClass.Position = GetInspectorName(strInspectorCode, "position");
                    PrintClass.DateCover = "From " + strDateFrom + " to " + strDateTo;
                    PrintClass.Query = pSet.Query.ToString();
                    //PrintClass.FormLoad();
                    PrintClass.ShowDialog();    // RMC 20171201 modified printing of Inspection report and Notices, changed to vsprinter
                }
                else
                {
                    MessageBox.Show("No record found.");
                    return;
                }
            }
            pSet.Close();
        }

        private string GetInspectorName(string strInspector, string strObject)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sInspector = string.Empty;
            string sLN = string.Empty;
            string sFN = string.Empty;
            string sMI = string.Empty;
            string sPosition = string.Empty;

            pRec.Query = string.Format("select * from inspector where inspector_code = '{0}'", strInspector);
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sLN = pRec.GetString("inspector_ln").Trim();
                    sFN = pRec.GetString("inspector_fn").Trim();
                    sMI = pRec.GetString("inspector_mi").Trim();
                    sInspector = sLN + ", " + sFN + " " + sMI + ".";
                    sPosition = pRec.GetString("position").Trim();
                }
                // RMC 20110901 Added auto-tagging if Lessor is not a registered business (s)
                else
                {
                    pRec.Close();

                    pRec.Query = string.Format("select * from sys_users where usr_code = '{0}'", strInspector);
                    if(pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sLN = pRec.GetString("usr_ln").Trim();
                            sFN = pRec.GetString("usr_fn").Trim();
                            sMI = pRec.GetString("usr_mi").Trim();
                            sInspector = sLN + ", " + sFN + " " + sMI + ".";
                            sPosition = pRec.GetString("usr_pos").Trim();
                        }
                    }
                    pRec.Close();

                }
                // RMC 20110901 Added auto-tagging if Lessor is not a registered business (e)
                
            }
            pRec.Close();

            if (strObject == "inspector")
                return sInspector;
            else
                return sPosition;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnGenerateInspectorUnOfficial()
        {
            string strInspectorCode = string.Empty;
            string strDateFrom = string.Empty;
            string strDateTo = string.Empty;

            strInspectorCode = cmbInspector.Text.Trim();
            strDateFrom = string.Format("{0:MM/dd/yyyy}", dtpDateFrom.Value);
            strDateTo = string.Format("{0:MM/dd/yyyy}", dtpDateTo.Value);

            pSet.Query = string.Format("select * from unofficial_dtls where inspector_code = '{0}' ", strInspectorCode);
            pSet.Query += string.Format(" and to_date(date_inspected,'MM/dd/yyyy') between to_date('{0}','MM/dd/yyyy') and to_date('{1}','MM/dd/yyyy')", strDateFrom, strDateTo);
            //pSet.Query += string.Format(" and is_option = '{0}' order by date_inspected,is_number", m_strOption);
            // RMC 20110816 (s)
            pSet.Query += string.Format(" and is_option = '{0}'", m_strOption);
            pSet.Query += " and is_number not in (select is_number from unofficial_info_tbl where trim(bin_settled) is not null)";
            pSet.Query += " order by date_inspected,is_number";
            // RMC 20110816 (e)
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    //PrintClass = new PrintInspection();
                    frmPrintInspection PrintClass = new frmPrintInspection();  // RMC 20171201 modified printing of Inspection report and Notices, changed to vsprinter
                    PrintClass.Source = "4";
                    PrintClass.ReportName = "INSPECTION DETAILS - Unofficial Businesses";
                    PrintClass.Inspector = GetInspectorName(strInspectorCode, "inspector");
                    PrintClass.Position = GetInspectorName(strInspectorCode, "position");
                    PrintClass.DateCover = "From " + strDateFrom + " to " + strDateTo;
                    PrintClass.Query = pSet.Query.ToString();
                    //PrintClass.FormLoad();
                    PrintClass.ShowDialog();    // RMC 20171201 modified printing of Inspection report and Notices, changed to vsprinter
                }
                else
                {
                    MessageBox.Show("No record found.","Print Report",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    return;
                }
            }
            pSet.Close();
        }

        private void OnGenerateIS()
        {
            string strInspectorCode = string.Empty;

            pSet.Query = string.Format("select * from unofficial_dtls where trim(is_number) = '{0}'", txtIS.Text.Trim());
            // RMC 20110816 (s)
            pSet.Query += " and is_number not in (select is_number from unofficial_info_tbl where trim(bin_settled) is not null)";
            // RMC 20110816 (e)
	        if(pSet.Execute())
            {
                if (pSet.Read())
                {
                    strInspectorCode = pSet.GetString("inspector_code").Trim();

                    //PrintClass = new PrintInspection();
                    frmPrintInspection PrintClass = new frmPrintInspection();  // RMC 20171201 modified printing of Inspection report and Notices, changed to vsprinter
                    PrintClass.Source = "5";
                    PrintClass.ReportName = "INSPECTION SERVICES";
                    PrintClass.Inspector = GetInspectorName(strInspectorCode, "inspector");
                    PrintClass.Position = GetInspectorName(strInspectorCode, "position");
                    PrintClass.Query = pSet.Query.ToString();
                    //PrintClass.FormLoad();
                    PrintClass.ShowDialog();    // RMC 20171201 modified printing of Inspection report and Notices, changed to vsprinter
                }
                else
                {
                    MessageBox.Show("No record found.","Print Report",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    return;
                }
            }
            pSet.Close();
	    }

        private void OnGenerateViolation()  
        {
            // RMC 20171115 Added history of untagged violations

            string strDateFrom = string.Empty;
            string strDateTo = string.Empty;
                        
            strDateFrom = string.Format("{0:MM/dd/yyyy}", dtpDateFrom.Value);
            strDateTo = string.Format("{0:MM/dd/yyyy}", dtpDateTo.Value);

            pSet.Query = string.Format("select inspector_code, bin, date_inspected as trans_date, violation_code, '' as untag_by from violations where to_date(date_inspected,'MM/dd/yyyy') between to_date('{0}','MM/dd/yyyy') and to_date('{1}','MM/dd/yyyy') ", strDateFrom, strDateTo);
            pSet.Query += " union all ";
            pSet.Query += string.Format("select inspector_code, bin, date_untag as trans_date, violation_code, untag_by as untag_by from violations_hist where to_date(date_untag,'MM/dd/yyyy') between to_date('{0}','MM/dd/yyyy') and to_date('{1}','MM/dd/yyyy') ", strDateFrom, strDateTo);
            pSet.Query += " order by bin, trans_date ";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    frmInspectorsReports form = new frmInspectorsReports();
                    form.Query = pSet.Query.ToString();
                    form.ShowDialog();
                }
                else
                {
                    MessageBox.Show("No record found.");
                    return;
                }
            }
            pSet.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}