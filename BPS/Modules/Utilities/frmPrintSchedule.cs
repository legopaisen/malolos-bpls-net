// RMC 20111001 corrections in printing of schedules


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.Utilities
{
    public partial class frmPrintSchedule : Form
    {
        private PrintSchedule PrintClass = null;
        private string m_strRevYear = string.Empty;
        private TabControl tabMain = new TabControl();
        private TabPage tabLicense = new TabPage("License");
        private TabPage tabFees = new TabPage("Fees");
        private TabPage tabAddl = new TabPage("Additional Charges");

        private DataGridView dgvListLicense = new DataGridView();
        private DataGridView dgvListFees = new DataGridView();
        private DataGridView dgvListAddl = new DataGridView();

        public frmPrintSchedule()
        {
            InitializeComponent();
        }

        private void frmPrintSchedule_Load(object sender, EventArgs e)
        {
            m_strRevYear = AppSettingsManager.GetConfigValue("07");

            this.Controls.Add(tabMain);
            tabMain.Controls.Add(tabLicense);
            tabMain.Controls.Add(tabFees);
            tabMain.Controls.Add(tabAddl);

            tabLicense.Controls.Add(dgvListLicense);
            tabFees.Controls.Add(dgvListFees);
            tabAddl.Controls.Add(dgvListAddl);

            tabMain.SelectedTab = tabAddl;
            tabMain.SelectedTab = tabFees;
            tabMain.SelectedTab = tabLicense;

            tabMain.Location = new Point(12, 12);
            tabMain.Size = new Size(300, 210);

            this.UpdateList();
            this.CheckList(dgvListLicense, "B");
            this.CheckList(dgvListFees, "FS");
            this.CheckList(dgvListAddl, "AD");

            
        }

        private void UpdateList()
        {
            string strQuery = string.Empty;

            strQuery = string.Format("select bns_desc \"Business Type\" from bns_table where length(rtrim(bns_code)) = 2 and rev_year = '{0}' order by bns_code",m_strRevYear);
            this.LoadList(strQuery, dgvListLicense);

            strQuery = string.Format("select fees_desc \"Fees\" from tax_and_fees_table where fees_type = 'FS' and rev_year = '{0}' order by fees_code", m_strRevYear);
            this.LoadList(strQuery, dgvListFees);
            
            strQuery = string.Format("select fees_desc \"Additional Charges\" from tax_and_fees_table where fees_type = 'AD' and rev_year = '{0}' order by fees_code", m_strRevYear);
            this.LoadList(strQuery, dgvListAddl);
        }

        private void LoadList(string strQuery, DataGridView dgvList)
        {
            dgvList.Columns.Add(new DataGridViewCheckBoxColumn());

            DataGridViewOracleResultSet dsList = new DataGridViewOracleResultSet(dgvList, strQuery, 0, 0);
            dgvList.RowHeadersVisible = false;
            dgvList.Columns[0].Width = 30;
            dgvList.Columns[1].Width = 300;
            dgvList.Columns[1].ReadOnly = true;
            dgvList.Refresh();

            dgvList.Size = new Size(290, 180);
            dgvList.AllowUserToResizeRows = false;
            
        }

        private void CheckList(DataGridView dgvList, string strObject)
        {
            OracleResultSet result = new OracleResultSet();
            //license B
            //fees FS
            //addl AD

            string strDesc = string.Empty;
            string strRowDesc = string.Empty;
            
            result.Query = string.Format("select * from print_sched where object = '{0}'", strObject);
            if(result.Execute())
            {
                while(result.Read())
                {
                    strDesc = result.GetString("description").Trim();

                    for(int i = 0; i < dgvList.Rows.Count; i++)
                    {
                        if (dgvList[1, i].Value != null)
                        {
                            strRowDesc = dgvList[1, i].Value.ToString().Trim();

                            if (strRowDesc == strDesc)
                                dgvList[0, i].Value = true;
                        }

                    }
                    
                }
            }
            result.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string strTabTag = string.Empty;
            strTabTag = tabMain.SelectedTab.Text.ToString().Trim();

            string strReportName = string.Empty;
            string strObject = string.Empty;

            //MessageBox.Show(sTabTag);
            if (strTabTag == "License")
            {
                strReportName = "SCHEDULE OF RATES FOR " + AppSettingsManager.GetConfigValue("17");
                GenerateReport(dgvListLicense, "B", strReportName);
            }
            else if (strTabTag == "Fees")
            {
                strReportName = "SCHEDULE OF RATES FOR FEES BY CATEGORY";
                GenerateReport(dgvListFees, "FS", strReportName);
            }
            else //"Additional Charges"
            {
                strReportName = "SCHEDULE OF RATES FOR ADDITIONAL CHARGES";
                GenerateReport(dgvListAddl, "AD", strReportName);
            }


        }
            
        private void GenerateReport(DataGridView dgvList, string strObject, string strReportName)
        {
            OracleResultSet result = new OracleResultSet();

            if(AppSettingsManager.GenerateInfo(strReportName))
            {
                result.Query = string.Format("delete from print_sched where object = '{0}'", strObject);
                if (result.ExecuteNonQuery() == 0)
                {
                }

                for(int i = 0; i < dgvList.Rows.Count; i++)
                {
                    if (dgvList[0, i].Value != null)
                    {
                        if ((bool)dgvList[0, i].Value == true)
                        {
                            result.Query = "insert into print_sched values(:1, :2)";
                            result.AddParameter(":1", strObject);
                            result.AddParameter(":2", dgvList[1, i].Value.ToString().Trim());
                            if (result.ExecuteNonQuery() == 0)
                            {
                            }
                        }
                    }
                }

                result.Query = string.Format("delete from gen_info where report_name = '{0}'", strReportName);
                if (result.ExecuteNonQuery() == 0)
                {
                }

                result.Query = "insert into gen_info values (:1, :2, :3, :4, :5)";
                result.AddParameter(":1", strReportName);
                result.AddParameter(":2", AppSettingsManager.GetCurrentDate());
                result.AddParameter(":3", AppSettingsManager.SystemUser.UserCode);
                result.AddParameter(":4", "1");
                result.AddParameter(":5", "ASS");
                if (result.ExecuteNonQuery() == 0)
                {
                }

                if (strObject == "B")
                    OnReportScheduleLicense(strReportName);
                else if (strObject == "FS")
                    OnReportScheduleFees(strReportName);
                else
                    OnReportScheduleAddl(strReportName);
                
                result.Query = string.Format("update gen_info set switch = 0 where report_name = '{0}' and system = 'ASS'",strReportName);
                if (result.ExecuteNonQuery() == 0)
                {
                }
            }
        }

        private void OnReportScheduleLicense(string strReportName)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            OracleResultSet result3 = new OracleResultSet();

            string strBnsDesc = string.Empty;
            string strBnsCode = string.Empty;
            string strRowBnsDesc = string.Empty;
            string strRowBnsCode = string.Empty;

            result.Query = "delete from rep_sched_license";
            if (result.ExecuteNonQuery() == 0)
            {
            }
	
            for(int iCtr = 0; iCtr < dgvListLicense.Rows.Count; iCtr++)
            {
                if (dgvListLicense[0, iCtr].Value != null)
                {
                    if ((bool)dgvListLicense[0, iCtr].Value == true)
                    {
                        strRowBnsDesc = dgvListLicense[1, iCtr].Value.ToString().Trim();
                        strRowBnsCode = AppSettingsManager.GetBnsCodeByDesc(strRowBnsDesc);

                        result.Query = string.Format("select bns_code, bns_desc from bns_table where rtrim(bns_code) like '{0}%%' and fees_code = 'B' and rev_year = '{1}' order by bns_code", strRowBnsCode, m_strRevYear);
                        if (result.Execute())
                        {
                            bool bSw = true;
                            double dGross1 = 0;
                            double dGross2 = 0;
                            double dExRate = 0;
                            double dPlusRate = 0;
                            double dAmount = 0;
                            string strGross1 = string.Empty;
                            string strGross2 = string.Empty;
                            string strExRate = string.Empty;
                            string strPlusRate = string.Empty;
                            string strAmount = string.Empty;
                            string strMinTax = string.Empty;
                            string strMaxTax = string.Empty;
                            string strExcessNo = string.Empty;
                            string strExcessAmt = string.Empty;

                            while (result.Read())
                            {
                                strBnsCode = result.GetString("bns_code");
                                strBnsDesc = AppSettingsManager.GetBnsDesc(strBnsCode);

                                result2.Query = string.Format("select * from excess_sched where bns_code = '{0}' and fees_code = 'B' and rev_year = '{1}'", strBnsCode, m_strRevYear);
                                if (result2.Execute())
                                {
                                    if (result2.Read())
                                    {
                                        strExcessNo = string.Format("{0:##.00}", result2.GetDouble("excess_no"));
                                        strExcessAmt = string.Format("{0:##.00}", result2.GetDouble("excess_amt"));
                                    }
                                    else
                                    {
                                        strExcessNo = "0";
                                        strExcessAmt = "0";
                                    }
                                }
                                result2.Close();

                                strMinTax = LoadMinMaxTax("B", strBnsCode, "MIN");
                                strMaxTax = LoadMinMaxTax("B", strBnsCode, "MAX");

                                // RMC 20111001 corrections in printing of schedules (s)
                                string strTmpExcessNo = "";
                                string strTmpExcessAmt = "";
                                int iRecCnt = 0;
                                strTmpExcessNo = strExcessNo;
                                strTmpExcessAmt = strExcessAmt;
                                
                                // RMC 20111001 corrections in printing of schedules (e)

                                result2.Query = string.Format("select gr_1, gr_2, ex_rate, plus_rate, amount from btax_sched where bns_code = '{0}' and rev_year = '{1}' order by gr_1 desc", strBnsCode, m_strRevYear);
                                if (result2.Execute())
                                {
                                    if (result2.Read())
                                    {
                                        result2.Close();
                                        if (result2.Execute())
                                        {
                                            while (result2.Read())
                                            {
                                                if (bSw)
                                                {
                                                    // RMC 20111001 corrections in printing of schedules (s)
                                                    iRecCnt++;
                                                    if (iRecCnt == 1)
                                                    {
                                                        strExcessNo = strTmpExcessNo;
                                                        strExcessAmt = strTmpExcessAmt;
                                                    }
                                                    else
                                                    {
                                                        strExcessNo = "0";
                                                        strExcessAmt = "0";
                                                    }
                                                    // RMC 20111001 corrections in printing of schedules (e)

                                                    dGross1 = result2.GetDouble("gr_1");
                                                    dGross2 = result2.GetDouble("gr_2");
                                                    dExRate = result2.GetDouble("ex_rate");
                                                    dPlusRate = result2.GetDouble("plus_rate");
                                                    dAmount = result2.GetDouble("amount");

                                                    strGross1 = string.Format("{0:##.00}", dGross1);
                                                    strGross2 = string.Format("{0:##.00}", dGross2);
                                                    strExRate = string.Format("{0:##.0000000}", dExRate);
                                                    strPlusRate = string.Format("{0:##.0000000}", dPlusRate);
                                                    strAmount = string.Format("{0:##.00}", dAmount);

                                                    result3.Query = "insert into rep_sched_license values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12,:13,:14)";
                                                    result3.AddParameter(":1", strReportName);
                                                    result3.AddParameter(":2", strBnsCode);
                                                    result3.AddParameter(":3", strBnsDesc);
                                                    result3.AddParameter(":4", strGross1);
                                                    result3.AddParameter(":5", strGross2);
                                                    result3.AddParameter(":6", strExRate);
                                                    result3.AddParameter(":7", strPlusRate);
                                                    result3.AddParameter(":8", strAmount);
                                                    result3.AddParameter(":9", AppSettingsManager.SystemUser.UserCode);
                                                    result3.AddParameter(":10", AppSettingsManager.SystemUser.Position);
                                                    result3.AddParameter(":11", strExcessNo);
                                                    result3.AddParameter(":12", strExcessAmt);
                                                    result3.AddParameter(":13", "0");
                                                    result3.AddParameter(":14", "0");
                                                    if (result3.ExecuteNonQuery() == 0)
                                                    {
                                                    }
                                                }
                                                else
                                                {
                                                    result3.Query = "insert into rep_sched_license values (:1,:2,:3,'0','0','0','0','0',:4,:5,'0','0','0','0')";
                                                    result3.AddParameter(":1", strReportName);
                                                    result3.AddParameter(":2", strBnsCode);
                                                    result3.AddParameter(":3", strBnsDesc);
                                                    result3.AddParameter(":4", AppSettingsManager.SystemUser.UserCode);
                                                    result3.AddParameter(":5", AppSettingsManager.SystemUser.Position);
                                                    if (result3.ExecuteNonQuery() == 0)
                                                    {
                                                    }
                                                    break;

                                                }
                                                
                                            }
                                            bSw = false;
                                        }
                                        
                                    }
                                    else
                                    {
                                        result2.Close();

                                        string strBCode;

                                        result2.Query = string.Format("select * from fix_sched where bns_code = '{0}' and rev_year = '{1}'", strBnsCode, m_strRevYear);
                                        if (result2.Execute())
                                        {
                                            while (result2.Read())
                                            {
                                                dAmount = result2.GetDouble("fix_amount");

                                                string sQty, sQty2, sArea, sArea2;
                                                try
                                                {
                                                    sQty = Convert.ToString(result2.GetInt("qty1"));
                                                    sQty2 = Convert.ToString(result2.GetInt("qty2"));
                                                }
                                                catch
                                                {
                                                    sQty = "";
                                                    sQty2 = "";
                                                }
                                                try
                                                {
                                                    sArea = Convert.ToString(result2.GetDouble("area1"));
                                                    sArea2 = Convert.ToString(result2.GetDouble("area2"));
                                                }
                                                catch
                                                {
                                                    sArea = "";
                                                    sArea2 = "";
                                                }

                                                if ((sQty == "" || (sQty == "0" && sQty2 == "0")) && sArea != "")
                                                {
                                                    dGross1 = result2.GetDouble("area1");
                                                    dGross2 = result2.GetDouble("area2");
                                                }
                                                else if (sQty != "" && (sArea == "" || (sArea == "0.00" && sArea2 == "0.00")))
                                                {
                                                    dGross1 = Convert.ToDouble(result2.GetInt("qty1"));
                                                    dGross2 = Convert.ToDouble(result2.GetInt("qty2"));
                                                }

                                                strGross1 = string.Format("{0:##.00}", dGross1);
                                                strGross2 = string.Format("{0:##.00}", dGross2);
                                                strExRate = string.Format("{0:##.0000000}", dExRate);
                                                strPlusRate = string.Format("{0:##.0000000}", dPlusRate);
                                                strAmount = string.Format("{0:##.00}", dAmount);

                                                result3.Query = "insert into rep_sched_license values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12,:13,:14)";
                                                result3.AddParameter(":1", strReportName);
                                                result3.AddParameter(":2", strBnsCode);
                                                result3.AddParameter(":3", strBnsDesc);
                                                result3.AddParameter(":4", strGross1);
                                                result3.AddParameter(":5", strGross2);
                                                result3.AddParameter(":6", strExRate);
                                                result3.AddParameter(":7", strPlusRate);
                                                result3.AddParameter(":8", strAmount);
                                                result3.AddParameter(":9", AppSettingsManager.SystemUser.UserCode);
                                                result3.AddParameter(":10", AppSettingsManager.SystemUser.Position);
                                                result3.AddParameter(":11", strExcessNo);
                                                result3.AddParameter(":12", strExcessAmt);
                                                result3.AddParameter(":13", strMinTax);
                                                result3.AddParameter(":14", strMaxTax);
                                                if (result3.ExecuteNonQuery() == 0)
                                                {
                                                }

                                            }
                                        }
                                        result2.Close();
                                    }
                                   
                                        
                                }
                                
                            }
                        }
                        result.Close();

                    }
                }
			}
            
            PrintReport("1", strReportName);

		}
	
	    private string LoadMinMaxTax(string strFeesCode, string strBnsCode, string strReturn)
        {
            OracleResultSet result = new OracleResultSet();
            string strMinTax = string.Empty;
            string strMaxTax = string.Empty;

            result.Query = string.Format("select * from minmax_tax_table where fees_code = '{0}' and bns_code = '{1}' and rev_year = '{2}'", strFeesCode, strBnsCode, m_strRevYear);
            if (result.Execute())
            {
                if (result.Read())
                {
                    strMinTax = string.Format("{0:##0.00}", result.GetInt("min_tax"));
                    strMaxTax = string.Format("{0:##0.00}", result.GetInt("max_tax"));
                    

                }
                else
                {
                    strMinTax = string.Format("{0:##0.00}", 0);
                    strMaxTax = string.Format("{0:##0.00}", 0);
                    
                }

            }
            result.Close();

            if(strReturn == "MIN")
                return strMinTax;
            else
                return strMaxTax;
        }

        private void OnReportScheduleFees(string strReportName)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rGroup = new OracleResultSet();
            OracleResultSet rData = new OracleResultSet();

            string strRowFeesDesc = string.Empty;
            string strRowFeesCode = string.Empty;
            string strGroupCode = string.Empty;
            string strGroupDesc = string.Empty;

            result.Query = "delete from rep_sched_fees";
            if(result.ExecuteNonQuery() == 0)
            {
            }
            
            /* for progress bar
            result.Query = "select * from print_sched where object = 'FS'";
		    pApp->iProgressTotal = pRecData->GetRecordCount();
	        pRecData->Close();
	        pApp->JobStatus(0);*/

            for (int iCtr = 0; iCtr < dgvListFees.Rows.Count; iCtr++)
            {
                //iProgressCtr++;
                if (dgvListFees[0, iCtr].Value != null)
                {
                    if ((bool)dgvListFees[0, iCtr].Value == true)
                    {
                        strRowFeesDesc = dgvListFees[1, iCtr].Value.ToString().Trim();
                        strRowFeesCode = AppSettingsManager.GetFeesCodeByDesc(strRowFeesDesc, "FS");

                        rGroup.Query = string.Format("select bns_code,bns_desc from bns_table where fees_code = 'B' and length(rtrim(bns_code)) = 2 and rev_year = '{0}' order by bns_code", m_strRevYear);
                        if (rGroup.Execute())
                        {
                            while (rGroup.Read())
                            {
                                strGroupCode = rGroup.GetString("bns_code");
                                strGroupDesc = rGroup.GetString("bns_desc");

                                rData.Query = string.Format("select * from fees_sched where fees_code = '{0}' and rtrim(bns_code) like '{1}%%' and "
                                    + " rev_year = '{2}' order by fees_code,bns_code,qty1,area1,gr_1", strRowFeesCode, strGroupCode, m_strRevYear);
                                if (rData.Execute())
                                {
                                    string strBnsCode = string.Empty;
                                    string strBnsDesc = string.Empty;
                                    string strType = string.Empty;
                                    string strMinFees = string.Empty;
                                    string strMaxFees = string.Empty;
                                    string strRange1 = string.Empty;
                                    string strRange2 = string.Empty;
                                    string strExRate = string.Empty;
                                    string strPlusRate = string.Empty;
                                    string strAmount = string.Empty;
                                    string strExcessNo = string.Empty;
                                    string strExcessAmt = string.Empty;

                                    int intQty1 = 0;
                                    int intQty2 = 0;
                                    double dblArea1 = 0;
                                    double dblArea2 = 0;
                                    double dblGr1 = 0;
                                    double dblGr2 = 0;
                                    double dblExRate = 0;
                                    double dblPlusRate = 0;
                                    double dblAmount = 0;
                                    bool blnSwQty = false;

                                    while (rData.Read())
                                    {
                                        strBnsCode = rData.GetString("bns_code");
                                        strBnsDesc = AppSettingsManager.GetSubBnsDesc(strBnsCode, strRowFeesCode);
                                        intQty1 = rData.GetInt("qty1");
                                        intQty2 = rData.GetInt("qty2");
                                        dblArea1 = rData.GetDouble("area1");
                                        dblArea2 = rData.GetDouble("area2");
                                        dblGr1 = rData.GetDouble("gr_1");
                                        dblGr2 = rData.GetDouble("gr_2");
                                        dblExRate = rData.GetDouble("ex_rate");
                                        dblPlusRate = rData.GetDouble("plus_rate");
                                        dblAmount = rData.GetDouble("amount");

                                        strExRate = string.Format("{0:##.0000000}", dblExRate);
                                        strPlusRate = string.Format("{0:##.0000000}", dblPlusRate);
                                        strAmount = string.Format("{0:##.00}", dblAmount);

                                        strRange1 = "";
                                        strRange2 = "";
                                        blnSwQty = false;

                                        if (intQty1 == 0 && intQty2 == 0 && dblArea1 == 0 && dblArea2 == 0 && dblGr1 == 0 && dblGr2 == 0)
                                        {
                                            strType = "F";
                                            strRange1 = "0";
                                            strRange2 = "0";
                                        }
                                        else
                                        {
                                            if (intQty1 > 0 || intQty2 > 0)
                                            {
                                                blnSwQty = true;

                                                if (intQty1 == 1 && intQty2 == 1)
                                                    strType = "Q";
                                                else
                                                    strType = "QR";

                                                strRange1 = string.Format("{0:##}", intQty1);
                                                strRange2 = string.Format("{0:##}", intQty2);

                                                if (intQty1 == 0 || intQty1 == null)
                                                    strRange1 = "0";

                                                if (intQty2 == 0 || intQty2 == null)
                                                    strRange2 = "0";
                                            }

                                            if (dblArea1 > 0 || dblArea2 > 0)
                                            {
                                                if (dblArea1 == 1 && dblArea2 == 1)
                                                    strType = "A";
                                                else
                                                    strType = "AR";

                                                strRange1 = string.Format("{0:##.00}", dblArea1);
                                                strRange2 = string.Format("{0:##.00}", dblArea2);
                                            }

                                            if (dblGr1 > 0 || dblGr2 > 0)
                                            {
                                                strType = "RR";

                                                strRange1 = string.Format("{0:##.00}", dblGr1);
                                                strRange2 = string.Format("{0:##.00}", dblGr2);
                                            }
                                        }

                                        result.Query = string.Format("select * from excess_sched where bns_code = '{0}' and fees_code = '{1}' and rev_year = '{2}'", strBnsCode, strRowFeesCode, m_strRevYear);
                                        if (result.Execute())
                                        {
                                            if (result.Read())
                                            {
                                                strExcessNo = string.Format("{0:##.00}", result.GetDouble("excess_no"));
                                                strExcessAmt = string.Format("{0:##.00}", result.GetDouble("excess_amt"));
                                            }
                                            else
                                            {
                                                strExcessNo = "0";
                                                strExcessAmt = "0";
                                            }
                                        }
                                        result.Close();

                                        strMinFees = LoadMinMaxTax(strRowFeesCode, strBnsCode, "MIN");
                                        strMaxFees = LoadMinMaxTax(strRowFeesCode, strBnsCode, "MAX");

                                        //if(blnSwQty)
                                        {
                                            result.Query = "insert into rep_sched_fees values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12,:13,:14,:15,:16,:17,:18,:19)";
                                            result.AddParameter(":1", strReportName);
                                            result.AddParameter(":2", strGroupCode);
                                            result.AddParameter(":3", strGroupDesc);
                                            result.AddParameter(":4", strRowFeesCode);
                                            result.AddParameter(":5", strRowFeesDesc);
                                            result.AddParameter(":6", strBnsCode);
                                            result.AddParameter(":7", strBnsDesc);
                                            result.AddParameter(":8", strType);
                                            result.AddParameter(":9", strRange1);
                                            result.AddParameter(":10", strRange2);
                                            result.AddParameter(":11", strExRate);
                                            result.AddParameter(":12", strPlusRate);
                                            result.AddParameter(":13", strAmount);
                                            result.AddParameter(":14", AppSettingsManager.SystemUser.UserCode);
                                            result.AddParameter(":15", AppSettingsManager.SystemUser.Position);
                                            result.AddParameter(":16", strExcessNo);
                                            result.AddParameter(":17", strExcessAmt);
                                            result.AddParameter(":18", strMinFees);
                                            result.AddParameter(":19", strMaxFees);
                                            if (result.ExecuteNonQuery() == 0)
                                            {
                                            }
                                        }
                                    }
                                }
                                rData.Close();
                            }
                        }
                        rGroup.Close();
                    }
                }
            }

	        PrintReport("2", strReportName);
        }

        private void OnReportScheduleAddl(string strReportName)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rData = new OracleResultSet();

            string strRowFeesCode = string.Empty;
            string strRowFeesDesc = string.Empty;
            string strType = string.Empty;
            string strAmount = string.Empty;

            result.Query = "delete from rep_sched_addl";
            if(result.ExecuteNonQuery() == 0)
            {
            }

            // add progress bar here

            for (int iCtr = 0; iCtr < dgvListAddl.Rows.Count; iCtr++)
            {
                if (dgvListAddl[0, iCtr].Value != null)
                {
                    if ((bool)dgvListAddl[0, iCtr].Value == true)
                    {
                        strRowFeesDesc = dgvListAddl[1, iCtr].Value.ToString();
                        strRowFeesCode = AppSettingsManager.GetFeesCodeByDesc(strRowFeesDesc, "AD");

                        rData.Query = string.Format("select * from addl_sched where fees_code = '{0}' and rev_year = '{1}' order by fees_code", strRowFeesCode, m_strRevYear);
                        if (rData.Execute())
                        {
                            if (rData.Read())
                            {
                                strType = rData.GetString("data_type");

                                if (strType == "F")
                                    strType = "Fixed Amount";
                                else if (strType == "Q")
                                    strType = "Quantity";
                                else if (strType == "O")
                                    strType = "User-Input Amount";

                                try
                                {
                                    strAmount = string.Format("{0:##.00}", rData.GetDouble("amount"));
                                }
                                catch
                                {
                                    strAmount = "0";
                                }

                                result.Query = "insert into rep_sched_addl values (:1,:2,:3,:4,:5,:6,:7)";
                                result.AddParameter(":1", strReportName);
                                result.AddParameter(":2", strRowFeesCode);
                                result.AddParameter(":3", strRowFeesDesc);
                                result.AddParameter(":4", strType);
                                result.AddParameter(":5", strAmount);
                                result.AddParameter(":6", AppSettingsManager.SystemUser.UserCode);
                                result.AddParameter(":7", AppSettingsManager.SystemUser.Position);
                                if (result.ExecuteNonQuery() == 0)
                                {
                                }
                            }
                        }
                        rData.Close();
                    }
                }
            }
	        PrintReport("3", strReportName);
        }

        private void PrintReport(string strReportSwitch, string strReportName)
        {
            /*PrintClass = new PrintSchedule();
            PrintClass.Source = strReportSwitch;
            PrintClass.ReportName = strReportName;
            PrintClass.FormLoad();*/

            // RMC 20150518 report corrections (s)
            frmReportSched form = new frmReportSched();
            form.Source = strReportSwitch;
            form.ReportName = strReportName;
            form.ShowDialog();
            // RMC 20150518 report corrections (e)
        }
        
    }
}