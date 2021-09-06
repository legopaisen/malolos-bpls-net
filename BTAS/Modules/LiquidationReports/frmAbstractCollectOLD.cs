using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using System.Collections;
using Amellar.Common.Message_Box;
using Amellar.Common.DynamicProgressBar;
using System.Threading;

namespace Amellar.Modules.LiquidationReports
{
    public partial class frmAbstractCollectOLD : Form
    {
        public frmAbstractCollectOLD()
        {
            InitializeComponent();
        }

        private void LoadTeller()
        {
            String sQuery = "";
            String sGetString = "";
            if (m_iReportSwitch == 1)
            {
                sQuery = string.Format("select teller, ln from tellers order by teller");
                sGetString = "teller";
            }
            else
            {
                sQuery = string.Format("select usr_code from sys_users order by usr_code");
                sGetString = "usr_code";
            }

            cmbTeller.Items.Clear();
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                cmbTeller.Items.Add("");
                while (pSet.Read())
                {
                    cmbTeller.Items.Add(pSet.GetString(sGetString));
                }
            }

            if (cmbTeller.Items.Count > 0)
                cmbTeller.SelectedIndex = 0;
            pSet.Close();
        }

        private string m_sReportName = "";

        private bool m_bSw = false;

        private int m_iReportSwitch = 0;

        public int ReportSwitch
        {
            get { return m_iReportSwitch; }
            set { m_iReportSwitch = value; }
        }

        private int m_iRadioAbstract = 0;

        //MCR 20140709 (s)
        frmProgress m_form; 
        private Thread m_objThread;
        public delegate void DifferentDelegate(object value, frmProgress.ProgressMode mode);
        public static void DoSomethingDifferent(object value, frmProgress.ProgressMode mode, DifferentDelegate threadFunction)
        {
            threadFunction(value, mode); // NOTE: invoked with a parameter
        }
        private void ThreadProcess()
        {
            using (m_form = new frmProgress())
            {

                m_form.TopMost = true;

                m_form.ShowDialog();
            }
        }
        //MCR 20140709 (e)

        private string m_sORRangeFrom = String.Empty;
        private string m_sORRangeTo = String.Empty;

        string m_sRevYear = AppSettingsManager.GetConfigObject("07");

        private void EnableControls(bool blnEnable)
        {
            lblRCDSeries.Visible = blnEnable;
            cmbRCDSeries.Visible = blnEnable;
            dtpFrom.Enabled = blnEnable;
            dtpTo.Enabled = blnEnable;
            cmbTeller.Enabled = blnEnable;
        }

        private void frmAbstractCollect_Load(object sender, EventArgs e)
        {
            LoadTeller();

            if (m_iReportSwitch == 1)
                PopulateRCDSeries();

            dtpFrom.Text = AppSettingsManager.GetSystemDate().ToShortDateString();
            dtpTo.Text = AppSettingsManager.GetSystemDate().ToShortDateString();

            if (m_iReportSwitch == 1) //Abstract of Collection
            {
                this.Text = "Abstract Report of Collection";
                cmbTeller.Enabled = false;
            }
            else if (m_iReportSwitch == 2) //Abstract of Posted OR
            {
                this.Text = "Abstract Report of Posted O.R.";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdoDaily_Click(object sender, EventArgs e)
        {
            GetORrangeNO();
            m_iRadioAbstract = 1;
            EnableControls(false);
            dtpFrom.Enabled = true;
            dtpTo.Enabled = true;

            lblTeller.Visible = false;
            cmbTeller.Visible = false;
        }

        private void GetORrangeYES()
        {
            lblTitle.Text = "O.R. Range";
            EnableControls(false);
            txtORFrom.Enabled = true;
            txtORto.Enabled = true;
            txtORFrom.BringToFront();
            txtORto.BringToFront();
        }

        private void GetORrangeNO()
        {
            lblTitle.Text = "Covered Period";
            txtORFrom.Enabled = false;
            txtORto.Enabled = false;
            txtORFrom.SendToBack();
            txtORto.SendToBack();
            EnableControls(false);

            dtpFrom.Enabled = true;
            dtpTo.Enabled = true;

            m_sORRangeFrom = "%";
            m_sORRangeTo = "%";
        }

        private void rdoOR_Click(object sender, EventArgs e)
        {
            lblTeller.Visible = false;
            cmbTeller.Visible = false;

            m_iRadioAbstract = 2;
            if (m_iReportSwitch == 1)
            {
                string sTempValue = "";
                using (frmMsgBoxOptions MsgBoxOptions = new frmMsgBoxOptions())
                {
                    MsgBoxOptions.MessageCaption = "Set by?";
                    MsgBoxOptions.RadioType = 0;
                    MsgBoxOptions.RadioYes = "O.R. Date Range";
                    MsgBoxOptions.RadioNo = "Tax Year";
                    MsgBoxOptions.ShowDialog();

                    sTempValue = MsgBoxOptions.SelectedText;
                }

                if (sTempValue == "Yes")
                    GetORrangeYES();
                else if (sTempValue != String.Empty)
                    GetORrangeNO();
            }
        }

        private void rdoTeller_Click(object sender, EventArgs e)
        {
            GetORrangeNO();
            m_iRadioAbstract = 3;
            EnableControls(false);
            dtpFrom.Enabled = true;
            dtpTo.Enabled = true;

            lblTeller.Visible = true;
            cmbTeller.Visible = true;
            cmbTeller.Enabled = true;

            if (m_iReportSwitch == 1)
            {
                EnableControls(true);
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
            }
        }

        private void txtORFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
               && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            switch (m_iRadioAbstract)
            {
                case 1:
                    {
                        m_sReportName = "ABSTRACT OF RECEIPTS OF BUSINESS TAX AND REGULATORY FEES BY DAILY AGGREGATE";
                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                            OnReportAbstractCollection();
                        break;
                    }
                case 2:
                    {
                        m_sReportName = "ABSTRACT OF RECEIPTS OF BUSINESS TAX AND REGULATORY FEES BY OFFICIAL RECEIPTS";
                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                            OnReportAbstractCollection();
                        break;
                    }
                case 3:
                    {
                        if (m_iReportSwitch == 1)
                            m_sReportName = "ABSTRACT OF RECEIPTS OF BUSINESS TAX AND REGULATORY FEES BY TELLER";
                        else
                            m_sReportName = "ABSTRACT OF POSTED BUSINESS TAX AND REGULATORY FEES BY TELLER";

                        if (AppSettingsManager.GenerateInfo(m_sReportName))
                            OnReportAbstractCollection();
                        break;
                    }
            }
        }

        private void OnReportAbstractCollection()
        {
            try
            {
                String sQuery, sConvert, sGrandTotal;
                String sBrgyName, sOwnKind, sFeesCodeX, sOrNo, sBin = "", sBinV, sBinX, sSet;
                String sFromDate = dtpFrom.Value.ToShortDateString(), sToDate = dtpTo.Value.ToShortDateString(), sCurrentUser;
                //String sFeesCode,sFeesDesc; //sFeesCode[21],sFeesDesc[21];
                int iFees_cnt, iSet, ii, iBtax, iCount, iCtr1, iCtr2, iCtrAll, iProgressCtr;
                double fOrFees_due, fOrFees_surch, fOrFees_pen, fOrFees_amtdue;
                double fSurchPen, fTotalFees; //fHeaderFees[21]
                String sTeller = "";
                double Taxcredit = 0;

                ArrayList arr_fHeaderFees = new ArrayList();
                ArrayList arr_sFeesCode = new ArrayList();
                ArrayList arr_sFeesDesc = new ArrayList();

                iCount = 1;
                iCtr1 = 0;
                iCtrAll = 0;
                iCtr2 = 0;
                iSet = 0;
                iBtax = 0;
                iFees_cnt = 0;

                sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

                OracleResultSet pSet = new OracleResultSet();
                OracleResultSet pSet1 = new OracleResultSet();
                OracleResultSet pSet3 = new OracleResultSet();

                pSet.Query = string.Format("DELETE FROM REPORT_ABSTRACT WHERE REPORT_NAME = '{0}' AND USER_CODE = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet.ExecuteNonQuery();
                pSet.Close();

                pSet.Query = string.Format("DELETE FROM GEN_INFO WHERE REPORT_NAME = '{0}'", m_sReportName);
                pSet.ExecuteNonQuery();
                pSet.Close();

                DateTime m_dtSaveTime = AppSettingsManager.GetSystemDate();
                String sCurrentDate = string.Format("{1}/{2}/{0}",
                        m_dtSaveTime.Year,
                        m_dtSaveTime.Month,
                        m_dtSaveTime.Day);//  {3}:{4}:{5}, m_dtSaveTime.Hour, m_dtSaveTime.Minute, m_dtSaveTime.Second);

                pSet.Query = string.Format("INSERT INTO GEN_INFO VALUES('{0}',TO_DATE('" + sCurrentDate + "','MM/dd/yyyy'),'{1}',{2},'COL')", m_sReportName, AppSettingsManager.SystemUser.UserCode, 1);
                pSet.ExecuteNonQuery();
                pSet.Close();

                arr_sFeesCode.Add("B");
                arr_sFeesDesc.Add("LICENSE TAX");

                arr_fHeaderFees.Add(0);

                sQuery = string.Format("select distinct fees_code,fees_desc from tax_and_fees_table where rev_year = '{0}' order by fees_code", m_sRevYear);
                pSet.Query = sQuery;
                if (pSet.Execute())
                    while (pSet.Read())
                    {
                        arr_fHeaderFees.Add(0);
                        iCtr1 += 1;

                        arr_sFeesCode.Add(pSet.GetString("fees_code").Trim());
                        arr_sFeesDesc.Add(pSet.GetString("fees_desc").Trim());

                        if (arr_sFeesDesc[iCtr1].ToString() == "SANITARY INSPECTION FEE")
                            arr_sFeesDesc[iCtr1] = "SANITARY INSP. FEE";
                        else if (arr_sFeesDesc[iCtr1].ToString() == "OCCUPATIONAL PERMIT FEE")
                            arr_sFeesDesc[iCtr1] = "OCC. PERMIT FEE";
                        else if (arr_sFeesDesc[iCtr1].ToString() == "POLICE CLEARANCE & APPLICATION")
                            arr_sFeesDesc[iCtr1] = "POLICE CLEARANCE";
                        else if (arr_sFeesDesc[iCtr1].ToString() == "HEALTH CERTIFICATE")
                            arr_sFeesDesc[iCtr1] = "HEALTH CERT. FEE";
                        else if (arr_sFeesDesc[iCtr1].ToString() == "BUSINESS REGISTRATION PLATE")
                            arr_sFeesDesc[iCtr1] = "BUSINESS REGISTRATION";
                    }
                pSet.Close();

                #region Progress
                int intCount = 0;
                int intCountIncreament = 0;
                m_objThread = new Thread(this.ThreadProcess);
                m_objThread.Start();
                Thread.Sleep(500);
                if (m_iReportSwitch == 1)
                {
                    if (m_iRadioAbstract == 1)//Daily
                        sQuery = string.Format("select count(distinct(or_date)) from pay_hist where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and time_posted <> ' ' and data_mode <> 'POS'", dtpFrom.Value.ToShortDateString(), dtpTo.Value.ToShortDateString());
                    else if (m_iRadioAbstract == 2)//OR
                    {
                        if (m_sORRangeFrom == "%" || m_sORRangeTo == "%")
                            sQuery = string.Format("select count(distinct(or_no)) from pay_hist where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and time_posted <> ' ' and data_mode <> 'POS'", dtpFrom.Value.ToShortDateString(), dtpTo.Value.ToShortDateString());
                        else
                            sQuery = string.Format("select count(distinct(or_no)) from pay_hist where time_posted <> ' ' and data_mode <> 'POS' and or_no between '{0}' and '{1}'", m_sORRangeFrom, m_sORRangeTo);
                    }
                    else if (m_iRadioAbstract == 3) //Teller
                    {
                        if (cmbTeller.Text == "ALL" || cmbTeller.Text.Trim() == String.Empty)
                            sTeller = "%%";
                        else
                            sTeller = cmbTeller + "%";

                        sQuery = string.Format("select count(distinct(or_no)) from pay_hist where or_no in (select rcd_remit.or_no from partial_remit, rcd_remit where partial_remit.rcd_series = '{0}' and rcd_remit.or_no between partial_remit.or_from and partial_remit.or_to) order by or_no", cmbRCDSeries.Text.Trim());
                    }
                }
                else //m_iReportSwitch == 2
                {
                    if (m_iRadioAbstract == 1)//Daily
                        sQuery = string.Format("select count(distinct(or_date)) from pay_hist where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and time_posted <> ' ' and data_mode = 'POS'", dtpFrom.Value.ToShortDateString(), dtpTo.Value.ToShortDateString());
                    else if (m_iRadioAbstract == 2)//OR
                    {
                        sQuery = string.Format("select count(distinct(or_no)) from pay_hist where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and time_posted <> ' ' and data_mode = 'POS'", dtpFrom.Value.ToShortDateString(), dtpTo.Value.ToShortDateString());
                    }
                    else if (m_iRadioAbstract == 3) //Teller
                    {
                        if (cmbTeller.Text == "ALL" || cmbTeller.Text.Trim() == String.Empty)
                            sTeller = "%%";
                        else
                            sTeller = cmbTeller + "%";

                        sQuery = string.Format("select count(distinct(or_no)) from pay_hist where bns_user like '{0}' and or_date >= to_date('{1}','MM/dd/yyyy') and or_date <= to_date('{2}','MM/dd/yyyy') and time_posted <> ' ' and data_mode = 'POS'", sTeller, dtpFrom.Value.ToShortDateString(), dtpTo.Value.ToShortDateString());
                    }
                }
                pSet.Query = sQuery;
                int.TryParse(pSet.ExecuteScalar(), out intCount);
                DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);
                #endregion


                if (m_iReportSwitch == 1)
                {
                    if (m_iRadioAbstract == 1)//Daily
                        sQuery = string.Format("select distinct(or_date) from pay_hist where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and time_posted <> ' ' and data_mode <> 'POS'", dtpFrom.Value.ToShortDateString(), dtpTo.Value.ToShortDateString());
                    else if (m_iRadioAbstract == 2)//OR
                    {
                        if (m_sORRangeFrom == "%" || m_sORRangeTo == "%")
                            sQuery = string.Format("select distinct(or_no),time_posted from pay_hist where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and time_posted <> ' ' and data_mode <> 'POS'", dtpFrom.Value.ToShortDateString(), dtpTo.Value.ToShortDateString());
                        else
                            sQuery = string.Format("select distinct(or_no),time_posted from pay_hist where time_posted <> ' ' and data_mode <> 'POS' and or_no between '{0}' and '{1}'", m_sORRangeFrom, m_sORRangeTo);
                    }
                    else if (m_iRadioAbstract == 3) //Teller
                    {
                        if (cmbTeller.Text == "ALL" || cmbTeller.Text.Trim() == String.Empty)
                            sTeller = "%%";
                        else
                            sTeller = cmbTeller + "%";

                        sQuery = string.Format("select distinct(or_no),time_posted,teller from pay_hist where or_no in (select rcd_remit.or_no from partial_remit, rcd_remit where partial_remit.rcd_series = '{0}' and rcd_remit.or_no between partial_remit.or_from and partial_remit.or_to) order by or_no", cmbRCDSeries.Text.Trim());
                    }
                }
                else //m_iReportSwitch == 2
                {
                    if (m_iRadioAbstract == 1)//Daily
                        sQuery = string.Format("select distinct(or_date) from pay_hist where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and time_posted <> ' ' and data_mode = 'POS'", dtpFrom.Value.ToShortDateString(), dtpTo.Value.ToShortDateString());
                    else if (m_iRadioAbstract == 2)//OR
                    {
                        sQuery = string.Format("select distinct(or_no),time_posted from pay_hist where or_date >= to_date('{0}','MM/dd/yyyy') and or_date <= to_date('{1}','MM/dd/yyyy') and time_posted <> ' ' and data_mode = 'POS'", dtpFrom.Value.ToShortDateString(), dtpTo.Value.ToShortDateString());
                    }
                    else if (m_iRadioAbstract == 3) //Teller
                    {
                        if (cmbTeller.Text == "ALL" || cmbTeller.Text.Trim() == String.Empty)
                            sTeller = "%%";
                        else
                            sTeller = cmbTeller + "%";

                        sQuery = string.Format("select distinct(or_no),time_posted,teller from pay_hist where bns_user like '{0}' and or_date >= to_date('{1}','MM/dd/yyyy') and or_date <= to_date('{2}','MM/dd/yyyy') and time_posted <> ' ' and data_mode = 'POS'", sTeller, dtpFrom.Value.ToShortDateString(), dtpTo.Value.ToShortDateString());
                    }
                }

                pSet.Query = sQuery;
                if (pSet.Execute())
                    while (pSet.Read())
                    {
                        DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                        intCountIncreament += 1;

                        String sOrDate = "", sTime;
                        String sOrTeller = "";
                        double dTotalDrCr, dDebit, dCredit, dDrCr, dTotalPayment, dTotalCheck;
                        dTotalPayment = 0;
                        dTotalCheck = 0;
                        dTotalDrCr = 0;
                        fSurchPen = 0;
                        fTotalFees = 0;
                        dDebit = 0;
                        dCredit = 0;
                        dDrCr = 0;
                        sOrNo = "";

                        for (ii = 0; ii <= iCtr1; ii++)
                            arr_fHeaderFees[ii] = 0;

                        if (m_iReportSwitch == 1)
                        {
                            if (m_iRadioAbstract == 1) //Daily
                            {
                                sOrDate = pSet.GetDateTime("or_date").ToShortDateString();
                                sQuery = string.Format("select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table where or_no in (select or_no from pay_hist where or_date = to_date('" + sOrDate + "','MM/dd/yyyy') and data_mode <> 'POS') and fees_code like 'B%%' having sum(fees_amtdue) is not null");
                            }
                            else if (m_iRadioAbstract == 2)//OR
                            {
                                sOrNo = pSet.GetString("or_no").Trim();
                                sTime = pSet.GetString("time_posted");
                                sQuery = string.Format("select distinct bin from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS'");
                                pSet3.Query = sQuery;
                                if (pSet3.Execute())
                                    if (pSet3.Read())
                                        sBin = pSet3.GetString("bin").Trim();
                                pSet3.Close();
                                sQuery = string.Format("select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table where or_no = '" + sOrNo + "' and fees_code like 'B%%' having sum(fees_amtdue) is not null"); // GDE 20080903 exclude posted records
                            }
                            else if (m_iRadioAbstract == 3) //Teller
                            {
                                sOrNo = pSet.GetString("or_no").Trim();
                                sTime = pSet.GetString("time_posted");
                                sOrTeller = pSet.GetString("teller").Trim();
                                sQuery = string.Format("select distinct bin from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS'");
                                pSet3.Query = sQuery;
                                if (pSet3.Execute())
                                    if (pSet3.Read())
                                        sBin = pSet3.GetString("bin").Trim();
                                pSet3.Close();
                                sQuery = string.Format("select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table where or_no in (select or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS') and fees_code like 'B%%' having sum(fees_amtdue) is not null");
                            }
                        }
                        else //m_iReportSwitch == 2
                        {
                            if (m_iRadioAbstract == 1) //Daily
                            {
                                sOrDate = pSet.GetDateTime("or_date").ToShortDateString();
                                sQuery = string.Format("select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table where or_no in (select or_no from pay_hist where or_date = to_date('" + sOrDate + "','MM/dd/yyyy') and data_mode = 'POS') and fees_code like 'B%%' having sum(fees_amtdue) is not null");
                            }
                            else if (m_iRadioAbstract == 2)//OR
                            {
                                sOrNo = pSet.GetString("or_no").Trim();
                                sTime = pSet.GetString("time_posted");
                                sQuery = string.Format("select distinct bin from pay_hist where or_no = '" + sOrNo + "' and data_mode = 'POS'");
                                pSet3.Query = sQuery;
                                if (pSet3.Execute())
                                    if (pSet3.Read())
                                        sBin = pSet3.GetString("bin").Trim();
                                pSet3.Close();
                                sQuery = string.Format("select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table where or_no = '" + sOrNo + "' and fees_code like 'B%%' having sum(fees_amtdue) is not null"); // GDE 20080903 exclude posted records
                            }
                            else if (m_iRadioAbstract == 3) //Teller
                            {
                                sOrNo = pSet.GetString("or_no").Trim();
                                sTime = pSet.GetString("time_posted");
                                sOrTeller = pSet.GetString("teller").Trim();
                                sQuery = string.Format("select distinct bin from pay_hist where or_no = '" + sOrNo + "' and data_mode = 'POS'");
                                pSet3.Query = sQuery;
                                if (pSet3.Execute())
                                    if (pSet3.Read())
                                        sBin = pSet3.GetString("bin").Trim();
                                pSet3.Close();
                                sQuery = string.Format("select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table where or_no in (select or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode = 'POS') and fees_code like 'B%%' having sum(fees_amtdue) is not null");
                            }
                        }

                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                            if (pSet1.Read())
                            {
                                //if (pSet1.GetDouble("fees_amtdue").ToString() != null)
                                //{
                                    fOrFees_due = pSet1.GetDouble("fees_due");
                                    fOrFees_surch = pSet1.GetDouble("fees_surch");
                                    fOrFees_pen = pSet1.GetDouble("fees_pen");
                                    fOrFees_amtdue = pSet1.GetDouble("fees_amtdue");

                                    arr_fHeaderFees[0] = Convert.ToDouble(arr_fHeaderFees[0].ToString()) + fOrFees_due;
                                    fSurchPen += (fOrFees_surch + fOrFees_pen);
                                    fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                               // }
                            }
                        pSet1.Close();

                        for (ii = 1; ii <= iCtr1; ii++)
                        {
                            sFeesCodeX = arr_sFeesCode[ii].ToString();
                            if (m_iReportSwitch == 1)
                            {
                                if (m_iRadioAbstract == 1) //Daily
                                    sQuery = @"select sum(fees_due) as fees_due, sum(fees_surch) as fees_surch, sum(fees_pen) as fees_pen, sum(fees_amtdue) as fees_amtdue from or_table where or_no in (select or_no from pay_hist where or_date = to_date('" + sOrDate + "','MM/dd/yyyy') and data_mode <> 'POS') and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";
                                else if (m_iRadioAbstract == 2) // OR
                                {
                                    sQuery = @"select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table";
                                    sQuery += " where or_no = '" + sOrNo + "' and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";
                                }
                                else if (m_iRadioAbstract == 3) // Teller
                                {
                                    sQuery = @"select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table";
                                    sQuery += " where or_no in (select or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode <> 'POS') ";
                                    sQuery += " and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";
                                }
                            }
                            else //m_iReportSwitch == 2
                            {
                                if (m_iRadioAbstract == 1) //Daily
                                    sQuery = @"select sum(fees_due) as fees_due, sum(fees_surch) as fees_surch, sum(fees_pen) as fees_pen, sum(fees_amtdue) as fees_amtdue from or_table where or_no in (select or_no from pay_hist where or_date = to_date('" + sOrDate + "','MM/dd/yyyy') and data_mode = 'POS') and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";
                                else if (m_iRadioAbstract == 2) // OR
                                {
                                    sQuery = @"select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table";
                                    sQuery += " where or_no = '" + sOrNo + "' and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";
                                }
                                else if (m_iRadioAbstract == 3) // Teller
                                {
                                    sQuery = @"select sum(fees_due) as fees_due,sum(fees_surch) as fees_surch,sum(fees_pen) as fees_pen,sum(fees_amtdue) as fees_amtdue from or_table";
                                    sQuery += " where or_no in (select or_no from pay_hist where or_no = '" + sOrNo + "' and data_mode = 'POS') ";
                                    sQuery += " and fees_code = '" + sFeesCodeX + "' having sum(fees_amtdue) is not null";
                                }
                            }

                            pSet1.Query = sQuery;
                            if (pSet1.Execute())
                                if (pSet1.Read())
                                {
                                    //if (pSet1.GetDouble("fees_amtdue").ToString() != null)
                                    //{
                                        fOrFees_due = pSet1.GetDouble("fees_due");
                                        fOrFees_surch = pSet1.GetDouble("fees_surch");
                                        fOrFees_pen = pSet1.GetDouble("fees_pen");
                                        fOrFees_amtdue = pSet1.GetDouble("fees_amtdue");

                                        arr_fHeaderFees[ii] = Convert.ToDouble(arr_fHeaderFees[ii].ToString()) + fOrFees_due;
                                        fSurchPen += (fOrFees_surch + fOrFees_pen);
                                        fTotalFees += (fOrFees_surch + fOrFees_pen + fOrFees_due);
                                  //  }
                                }
                            pSet1.Close();
                        }

                        if (m_iRadioAbstract == 1) //Daily
                        {
                            sQuery = string.Format(@"insert into report_abstract values('{0}','','','','','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}'
    ,'{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}'
    ,'{33}','0','0','{34}','0','0','0',to_date('{35}','MM/DD/YYYY'),to_date('{36}','MM/DD/YYYY'),'{37}','{38}','{39}','{40}','{41}'
    ,to_date('{42}','MM/DD/YYYY'),'{43}','{44}','{45}','{46}','','{47}','{48}','{49}','{50}','0')",
                       StringUtilities.HandleApostrophe(StringUtilities.Left(sOrDate.Trim(), 100)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[0].ToString().Trim(), 800)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[1].ToString().Trim(), 50)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[2].ToString().Trim(), 50)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[3].ToString().Trim(), 50)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[4].ToString().Trim(), 50)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[5].ToString().Trim(), 50)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[6].ToString().Trim(), 50)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[7].ToString().Trim(), 50)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[8].ToString().Trim(), 50)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[9].ToString().Trim(), 50)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[10].ToString().Trim(), 50)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[11].ToString().Trim(), 50)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[12].ToString().Trim(), 50)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[13].ToString().Trim(), 50)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[14].ToString().Trim(), 50)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[15].ToString().Trim(), 50)),
                       Convert.ToDouble(arr_fHeaderFees[0]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[1]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[2]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[3]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[4]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[5]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[6]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[7]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[8]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[9]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[10]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[11]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[12]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[13]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[14]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[15]).ToString("0.##"),
                       fSurchPen, fTotalFees,
                       StringUtilities.HandleApostrophe(sFromDate),
                       StringUtilities.HandleApostrophe(sToDate),
                       StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 20)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 50)),
                       StringUtilities.HandleApostrophe(m_sRevYear),
                       sCurrentDate,
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[16].ToString().Trim(), 50)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[17].ToString().Trim(), 50)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[18].ToString().Trim(), 50)),
                       StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[19].ToString().Trim(), 50)),
                       Convert.ToDouble(arr_fHeaderFees[16]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[17]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[18]).ToString("0.##"),
                       Convert.ToDouble(arr_fHeaderFees[19]).ToString("0.##"));
                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();
                        }
                        else if (m_iRadioAbstract == 2) // OR
                        {
                            sQuery = string.Format(@"insert into report_abstract values('{0}','{51}','','','','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}'
,'{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}'
,'{33}','0','0','{34}','0','0','0',to_date('{35}','MM/DD/YYYY'),to_date('{36}','MM/DD/YYYY'),'{37}','{38}','{39}','{40}','{41}'
,to_date('{42}','MM/DD/YYYY'),'{43}','{44}','{45}','{46}','','{47}','{48}','{49}','{50}','0')",
                           StringUtilities.HandleApostrophe(StringUtilities.Left(sOrNo.Trim(), 100)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[0].ToString().Trim(), 800)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[1].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[2].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[3].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[4].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[5].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[6].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[7].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[8].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[9].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[10].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[11].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[12].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[13].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[14].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[15].ToString().Trim(), 50)),
                           Convert.ToDouble(arr_fHeaderFees[0]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[1]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[2]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[3]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[4]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[5]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[6]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[7]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[8]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[9]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[10]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[11]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[12]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[13]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[14]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[15]).ToString("0.##"),
                           fSurchPen, fTotalFees,
                           StringUtilities.HandleApostrophe(sFromDate),
                           StringUtilities.HandleApostrophe(sToDate),
                           StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 20)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 50)),
                           StringUtilities.HandleApostrophe(m_sRevYear),
                           sCurrentDate,
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[16].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[17].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[18].ToString().Trim(), 50)),
                           StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[19].ToString().Trim(), 50)),
                           Convert.ToDouble(arr_fHeaderFees[16]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[17]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[18]).ToString("0.##"),
                           Convert.ToDouble(arr_fHeaderFees[19]).ToString("0.##"),
                           StringUtilities.HandleApostrophe(sBin.Trim()));
                            pSet.Query = sQuery;
                            pSet.ExecuteNonQuery();
                        }
                        else if (m_iRadioAbstract == 3) // Teller
                        {
                            if (m_iReportSwitch == 1)
                            {
                                String sQuery2;
                                Taxcredit = 0;
                                double dBalance = 0;
                                sQuery2 = "select * from dbcr_memo where or_no = '" + sOrNo + "' and memo = 'DEBITED THRU PAYMENT MADE HAVING OR_NO " + sOrNo + "' and served = 'Y' and multi_pay = 'N'"; // and multi_pay = 'N'";
                                pSet1.Query = sQuery2;
                                if (pSet1.Execute())
                                    if (pSet1.Read())
                                        Taxcredit = pSet1.GetDouble("debit");
                                pSet1.Close();

                                double dExcess = 0;
                                sQuery2 = "select * from dbcr_memo where or_no = '" + sOrNo + "' and memo = 'REMAINING BALANCE AFTER PAYMENT W/ CREDIT HAVING OR " + sOrNo + "' and served = 'N' and multi_pay = 'N'";
                                pSet1.Query = sQuery2;
                                if (pSet1.Execute())
                                    if (pSet1.Read())
                                        dExcess = pSet1.GetDouble("balance");
                                pSet1.Close();

                                sQuery = string.Format(@"insert into report_abstract values('{0}','{1}','{2}','','','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}'
,'{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}'
,'{35}','0','0','{36}','0','0','{37}',to_date('{38}','MM/DD/YYYY'),to_date('{39}','MM/DD/YYYY'),'{40}','{41}','{42}','{43}','{44}'
,to_date('{45}','MM/DD/YYYY'),'{46}','{47}','{48}','{49}','{50}','{51}','{52}','{53}','{54}','{55}')",
                               StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.GetTeller(sOrTeller.Trim(), 0), 100)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(sOrNo.Trim(), 100)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.GetTaxPayerName(sBin.Trim()), 100)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser.Trim(), 100)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[1].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[2].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[3].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[4].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[5].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[6].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[7].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[8].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[9].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[10].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[11].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[12].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[13].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[14].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[15].ToString().Trim(), 50)),
                               Convert.ToDouble(arr_fHeaderFees[0]).ToString("0.##"),
                               Convert.ToDouble(arr_fHeaderFees[1]).ToString("0.##"),
                               Convert.ToDouble(arr_fHeaderFees[2]).ToString("0.##"),
                               Convert.ToDouble(arr_fHeaderFees[3]).ToString("0.##"),
                               Convert.ToDouble(arr_fHeaderFees[4]).ToString("0.##"),
                               Convert.ToDouble(arr_fHeaderFees[5]).ToString("0.##"),
                               Convert.ToDouble(arr_fHeaderFees[6]).ToString("0.##"),
                               Convert.ToDouble(arr_fHeaderFees[7]).ToString("0.##"),
                               Convert.ToDouble(arr_fHeaderFees[8]).ToString("0.##"),
                               Convert.ToDouble(arr_fHeaderFees[9]).ToString("0.##"),
                               Convert.ToDouble(arr_fHeaderFees[10]).ToString("0.##"),
                               Convert.ToDouble(arr_fHeaderFees[11]).ToString("0.##"),
                               Convert.ToDouble(arr_fHeaderFees[12]).ToString("0.##"),
                               Convert.ToDouble(arr_fHeaderFees[13]).ToString("0.##"),
                               Convert.ToDouble(arr_fHeaderFees[14]).ToString("0.##"),
                               Convert.ToDouble(arr_fHeaderFees[15]).ToString("0.##"),
                               fSurchPen, fTotalFees, dExcess,
                               StringUtilities.HandleApostrophe(sFromDate),
                               StringUtilities.HandleApostrophe(sToDate),
                               StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 20)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 50)),
                               StringUtilities.HandleApostrophe(m_sRevYear),
                               sCurrentDate,
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[16].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[17].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[18].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[19].ToString().Trim(), 50)),
                               StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.GetOwnBrgy(sBin.Trim()), 50)),
                               Convert.ToDouble(arr_fHeaderFees[16]).ToString("0.##"),
                               AppSettingsManager.ComputeOtherFees(sOrNo.Trim()).ToString("0.##"),
                               Convert.ToDouble(arr_fHeaderFees[18]).ToString("0.##"),
                               Convert.ToDouble(arr_fHeaderFees[19]).ToString("0.##"), Taxcredit);
                                pSet.Query = sQuery;
                                pSet.ExecuteNonQuery();
                            }
                            else
                            {
                                sQuery = string.Format(@"insert into report_abstract values('{0}','{1}','{2}','','',
'{3}','{4}','{5}','{6}','{7}',
'{8}','{9}','{10}','{11}','{12}',
'{13}','{14}','{15}','{16}','{17}','{18}',
'{19}','{20}','{21}','{22}','{23}',
'{24}','{25}','{26}','{27}','{28}',
'{29}','{30}','{31}','{32}','{33}','{34}',
'{35}','0','0','{36}','0','0','0',
to_date('{37}','MM/DD/YYYY'),to_date('{38}','MM/DD/YYYY'),'{39}','{40}','{41}','{42}','{43}',
to_date('{44}','MM/DD/YYYY'),'{45}','{46}','{47}','{48}','','{49}','{50}','{51}','{52}','0')",

                                              StringUtilities.HandleApostrophe(StringUtilities.Left(sOrTeller.Trim(), 100)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(sOrNo.Trim(), 100)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.GetTaxPayerName(sBin.Trim()), 100)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[0].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[1].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[2].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[3].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[4].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[5].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[6].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[7].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[8].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[9].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[10].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[11].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[12].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[13].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[14].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[15].ToString().Trim(), 50)),
                                              Convert.ToDouble(arr_fHeaderFees[0]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[1]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[2]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[3]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[4]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[5]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[6]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[7]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[8]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[9]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[10]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[11]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[12]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[13]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[14]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[15]).ToString("0.##"),
                                              fSurchPen, fTotalFees,
                                              StringUtilities.HandleApostrophe(sFromDate),
                                              StringUtilities.HandleApostrophe(sToDate),
                                              StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02")),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(m_sReportName, 100)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(AppSettingsManager.SystemUser.UserCode, 20)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(sCurrentUser, 50)),
                                              StringUtilities.HandleApostrophe(m_sRevYear),
                                              sCurrentDate,
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[16].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[17].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[18].ToString().Trim(), 50)),
                                              StringUtilities.HandleApostrophe(StringUtilities.Left(arr_sFeesDesc[19].ToString().Trim(), 50)),
                                    //
                                              Convert.ToDouble(arr_fHeaderFees[16]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[17]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[18]).ToString("0.##"),
                                              Convert.ToDouble(arr_fHeaderFees[19]).ToString("0.##"));
                                //
                                pSet.Query = sQuery;
                                pSet.ExecuteNonQuery();
                            }
                        }

                        DoSomethingDifferent(string.Format("{0:##} %", (Convert.ToDouble(intCountIncreament) / Convert.ToDouble(intCount)) * 100), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                        Thread.Sleep(3);
                    }
                pSet.Close();

                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                Thread.Sleep(10);

                frmLiqReports dlg = new frmLiqReports();
                if (m_iRadioAbstract == 1) //Daily
                    dlg.ReportSwitch = "AbstractCollectByDaily";
                else if (m_iRadioAbstract == 2) //OR
                    dlg.ReportSwitch = "AbstractCollectByOR";
                else if (m_iRadioAbstract == 3) // Teller
                {
                    dlg.iReportSwitch = 2;
                    dlg.ReportSwitch = "AbstractCollectByTeller";
                }
                dlg.ReportTitle = m_sReportName;
                dlg.DateFrom = dtpFrom.Value;
                dlg.DateTo = dtpTo.Value;
                dlg.ShowDialog();

                sQuery = string.Format("update gen_info set switch = 0 where report_name = '{0}' and user_code = '{1}'", m_sReportName, AppSettingsManager.SystemUser.UserCode);
                pSet.Query = sQuery;
                pSet.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void cmbRCDSeries_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
                e.Handled = true;

            // only allow one
            if (e.KeyChar == '-' && (sender as TextBox).Text.IndexOf('-') > -1)
                e.Handled = true;
        }

        private void PopulateRCDSeries()
        {
            String sQuery;
            cmbRCDSeries.Items.Clear();
            cmbRCDSeries.ResetText();
            OracleResultSet pSet = new OracleResultSet();

            if (cmbTeller.Text.Trim() == "")
                sQuery = @"select distinct teller,rcd_series from partial_remit order by rcd_series asc";
            else
                sQuery = @"select distinct teller,rcd_series from partial_remit where teller = '" + cmbTeller.Text.Trim() + "' order by rcd_series asc";

            pSet.Query = sQuery;
            if (pSet.Execute())
                while (pSet.Read())
                {
                    cmbRCDSeries.Items.Add(pSet.GetString(1));
                }
            pSet.Close();
        }

        private void cmbTeller_Leave(object sender, EventArgs e)
        {
            PopulateRCDSeries();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            DateTime cdtToday = new DateTime();
            cdtToday = AppSettingsManager.GetSystemDate();

            if (dtpFrom.Value.Date > cdtToday.Date || dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                MessageBox.Show("Invalid date entry, Please check.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpFrom.Value = AppSettingsManager.GetSystemDate();
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            DateTime cdtToday = new DateTime();
            cdtToday = AppSettingsManager.GetSystemDate();

            if (dtpTo.Value.Date > cdtToday.Date || dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                MessageBox.Show("Invalid date entry, Please check.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpTo.Value = AppSettingsManager.GetSystemDate();
            }
        }

    }
}