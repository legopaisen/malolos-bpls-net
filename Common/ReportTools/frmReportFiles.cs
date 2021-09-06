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
using Amellar.Modules.BusinessReports;
using BusinessRoll;
using BusinessSummary;

namespace Amellar.Common.ReportTools
{
    public partial class frmReportFiles : Form
    {
        public frmReportFiles()
        {
            InitializeComponent();
        }

        private string m_sSystem = "";
        String sQuery, m_sToolsReportName, m_sToolsUserCode;

        public string SystemName
        {
            set { m_sSystem = value; }
        }

        private void frmReportFiles_Load(object sender, EventArgs e)
        {
            PopulatedgView();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PopulatedgView()
        {
            dgView.Rows.Clear();
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select report_name,user_code,report_date from gen_info where system = '" + m_sSystem + "' order by report_name";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    dgView.Rows.Add(pSet.GetString(0), pSet.GetString(1), pSet.GetDateTime(2), pSet.GetInt(3));
                }
            }
            pSet.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgView.Rows.Count == 0)
            {
                MessageBox.Show("No record(s) found", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            m_sToolsReportName = dgView.CurrentRow.Cells[0].Value.ToString();
            m_sToolsUserCode = dgView.CurrentRow.Cells[1].Value.ToString();

            if (m_sToolsReportName.Trim() == "")
            {
                MessageBox.Show("Select a report name", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (m_sSystem == "COL")
            {
                #region comments
                /*
                CReportStatementOfCollection dlg1;
                CReportSummaryOfCollections dlg2;
                CReportsListOfDelinquency dlg3;
                CReportsListOfUnrenewed dlg4;
                CNoticeOfDelinquency dlg5;
                CReportsSummaryOfDelinquency dlg6;
                CReportsCollectibles dlg7;
                CUsersLog dlg8;
                CPrintingPaymentHistory dlgPayHist; //LEO 08272003
                CReportsNewAbstract dlgNewAbs;

                // CTS 01212005 abstract
                if (m_sToolsReportName == "ABSTRACT OF BUSINESS TAX AND REGULATORY FEES BY DAILY AGGREGATE")
                    dlg1.OnPrintReportStatementBrgy(m_sToolsReportName,m_sRFUserCode);
                // CTS 01212005 abstract

                // CTS 01212005 abstract
                if (m_sToolsReportName == "ABSTRACT OF RECEIPTS OF BUSINESS TAX AND REGULATORY FEES BY DAILY AGGREGATE")
                    dlgNewAbs.OnPrintReportAbstractByDaily(m_sToolsReportName,m_sRFUserCode);
                else if (m_sToolsReportName == "ABSTRACT OF RECEIPTS OF BUSINESS TAX AND REGULATORY FEES BY OFFICIAL RECEIPTS")
                    dlgNewAbs.OnPrintReportAbstractByOR(m_sToolsReportName,m_sRFUserCode);
                else if (m_sToolsReportName == "ABSTRACT OF RECEIPTS OF BUSINESS TAX AND REGULATORY FEES BY TELLER")
                    dlgNewAbs.OnPrintReportAbstractByTellerPost(m_sToolsReportName,m_sRFUserCode);
                // CTS 01212005 abstract

                if (m_sToolsReportName == "STATEMENT OF BUSINESS TAX AND REGULATORY FEES COLLECTIONS BY BARANGAY")
                    dlg1.OnPrintReportStatementBrgy(m_sToolsReportName,m_sRFUserCode);
                else if (m_sToolsReportName == "STATEMENT OF BUSINESS TAX AND REGULATORY FEES COLLECTIONS BY DISTRICT")   // JJP 09302003 for Manila
                    dlg1.OnPrintReportStatementDistrict(m_sToolsReportName,m_sRFUserCode);								   // JJP 09302003 for Manila
                else if (m_sToolsReportName == "STATEMENT OF BUSINESS TAX AND REGULATORY FEES COLLECTIONS BY MAIN BUSINESS")
                    dlg1.OnPrintReportStatementMainBns(m_sToolsReportName,m_sRFUserCode);
                else if (m_sToolsReportName == "STATEMENT OF BUSINESS TAX AND REGULATORY FEES COLLECTIONS BY DAILT AGGREGATE")
                    dlg1.OnPrintReportStatementDaily(m_sToolsReportName,m_sRFUserCode);
                else if (m_sToolsReportName == "STATEMENT OF BUSINESS TAX AND REGULATORY FEES COLLECTIONS BY BUSINESS STATUS")
                    dlg1.OnPrintReportStatementBnsStatus(m_sToolsReportName,m_sRFUserCode);
                else if (m_sToolsReportName == "STATEMENT OF BUSINESS TAX AND REGULATORY FEES COLLECTIONS BY GROSS RANGE")
                    dlg1.OnPrintReportStatementGrossReceipts(m_sToolsReportName,m_sRFUserCode);
                else if (m_sToolsReportName == "STATEMENT OF BUSINESS TAX AND REGULATORY FEES COLLECTIONS BY BUSINESS NAME")
                    dlg1.OnPrintReportStatementBnsName(m_sToolsReportName,m_sRFUserCode);

                if (m_sToolsReportName == "SUMMARY OF COLLECTION BY BARANGAY")
                    dlg2.OnPrintReportSummaryOfCollectionBarangay(m_sToolsReportName,m_sRFUserCode);
                else if (m_sToolsReportName == "SUMMARY OF COLLECTION BY DISTRICT")				   // JJP 09302003 for Manila
                    //dlg2.OnPrintReportSummaryOfCollectionDistrict(m_sToolsReportName,m_sRFUserCode);  // JJP 09302003 for Manila
                {
                    dlg2.m_iOptionFormat = 0;
                    dlg2.OnPrintReportWideSummaryOfCollectionDistrict(m_sToolsReportName,m_sRFUserCode);  // JJP 09302003 for Manila
                }
                else if (m_sToolsReportName == "SUMMARY OF COLLECTION BY MAIN BUSINESS")
                    //dlg2.OnPrintReportSummaryOfCollectionMainBusiness(m_sToolsReportName,m_sRFUserCode);
                {
                    dlg2.m_iOptionFormat = 0;
                    dlg2.OnPrintReportWideSummaryOfCollectionMainBusiness(m_sToolsReportName,m_sRFUserCode);
                }
                else if (m_sToolsReportName == "SUMMARY OF COLLECTION BY BUSINESS STATUS")
                    //dlg2.OnPrintReportSummaryOfCollectionBusinessStatus(m_sToolsReportName,m_sRFUserCode);
                {
                    dlg2.m_iOptionFormat = 0;
                    dlg2.OnPrintReportWideSummaryOfCollectionBusinessStatus(m_sToolsReportName,m_sRFUserCode);
                }
                else if (m_sToolsReportName == "SUMMARY OF COLLECTION BY OWNERSHIP KIND")
                    //dlg2.OnPrintReportSummaryOfCollectionOwnershipKind(m_sToolsReportName,m_sRFUserCode);
                {
                    dlg2.m_iOptionFormat = 0;
                    dlg2.OnPrintReportWideSummaryOfCollectionOrgnKind(m_sToolsReportName,m_sRFUserCode);
                }

                else if (m_sToolsReportName == "LIST OF DELINQUENCY BY BARANGAY")
                    dlg3.OnPrintReportListOfDelinquentByBarangay(m_sToolsReportName,m_sRFUserCode);
                else if (m_sToolsReportName == "LIST OF DELINQUENCY BY DISTRICT")                     // JJP 09302003 for Manila      
                    dlg3.OnPrintReportListOfDelinquentByDistrict(m_sToolsReportName,m_sRFUserCode);   // JJP 09302003 for Manila
                else if (m_sToolsReportName == "LIST OF DELINQUENT BY MAIN BUSINESS")
                    dlg3.OnPrintReportListOfDelinquentByMainBusiness(m_sToolsReportName,m_sRFUserCode);
                else if (m_sToolsReportName == "LIST OF DELINQUENT BY BUSINESS STATUS")
                    dlg3.OnPrintReportListOfDelinquentByStatus(m_sToolsReportName,m_sRFUserCode);
                else if (m_sToolsReportName == "LIST OF DELINQUENT BY OWNERSHIP KIND")
                    dlg3.OnPrintReportListOfDelinquentByOrgnKind(m_sToolsReportName,m_sRFUserCode);
                else if (m_sToolsReportName == "LIST OF DELINQUENT BY GROSS RECEIPTS")
                    dlg3.OnPrintReportListOfDelinquentByGrossReceipts(m_sToolsReportName,m_sRFUserCode);
                else if (m_sToolsReportName == "LIST OF DELINQUENT BY TOP GROSS")
                    dlg3.OnPrintReportListOfDelinquentByTop(m_sToolsReportName,m_sRFUserCode);

                if (m_sToolsReportName == "LIST OF BUSINESSES WITH UNRENEWED PERMITS BY MAIN BUSINESS")
                    dlg4.OnPrintReportListOfUnrenewedMainBusiness(m_sToolsReportName,m_sRFUserCode);
                else if (m_sToolsReportName == "LIST OF BUSINESSES WITH UNRENEWED PERMITS BY BARANGAY")
                    dlg4.OnPrintReportListOfUnrenewedBarangay(m_sToolsReportName,m_sRFUserCode);

                if (m_sToolsReportName == "NOTICE OF DELINQUENCY")
                    dlg5.OnPrintReportNoticeOfDelinquency(m_sToolsReportName,m_sRFUserCode);

                if (m_sToolsReportName == "SUMMARY OF DELINQUENT BY BARANGAY")
                    dlg6.OnPrintReportSummaryOfDelinquentByBarangay(m_sToolsReportName,m_sRFUserCode); 
                else if (m_sToolsReportName == "SUMMARY OF DELINQUENT BY DISTRICT")				    // JJP 09302003 for Manila
                    dlg6.OnPrintReportSummaryOfDelinquentByDistrict(m_sToolsReportName,m_sRFUserCode); // JJP 09302003 for Manila
                else if (m_sToolsReportName == "SUMMARY OF DELINQUENT BY MAIN BUSINESS")               
                    dlg6.OnPrintReportSummaryOfDelinquentByMainBusiness(m_sToolsReportName,m_sRFUserCode);
                else if (m_sToolsReportName == "SUMMARY OF DELINQUENT BY OWNERSHIP KIND")
                    dlg6.OnPrintReportSummaryOfDelinquentByOrgnKind(m_sToolsReportName,m_sRFUserCode);

                //if (m_sToolsReportName == "LIST OF COLLECTIBLES BY BARANGAY") // GDE 20090602
                if (m_sToolsReportName == "LIST OF RECEIVABLES BY BARANGAY")
                {
                    dlg7.m_bIsList = true;
                    dlg7.mv_iRadioBrgy = 0;
                    dlg7.OnPrintReportListOfCollectiblesByBarangay(m_sToolsReportName,m_sRFUserCode);
                }
                //else if (m_sToolsReportName == "LIST OF COLLECTIBLES BY MAIN BUSINESS") // GDE 20090602
                if (m_sToolsReportName == "LIST OF RECEIVABLES BY MAIN BUSINESS")
                {
                    dlg7.m_bIsList = true;
                    dlg7.mv_iRadioBrgy = 1;
                    dlg7.OnPrintReportListOfCollectiblesByBarangay(m_sToolsReportName,m_sRFUserCode);
                }
                //else if (m_sToolsReportName == "LIST OF COLLECTIBLES BY BUSINESS STATUS")// GDE 20090602
                if (m_sToolsReportName == "LIST OF RECEIVABLES BY BUSINESS STATUS")
                {
                    dlg7.m_bIsList = true;
                    dlg7.mv_iRadioBrgy = 2;
                    dlg7.OnPrintReportListOfCollectiblesByBarangay(m_sToolsReportName,m_sRFUserCode);
                }
                //else if (m_sToolsReportName == "LIST OF COLLECTIBLES BY OWNERSHIP KIND")// GDE 20090602
                if (m_sToolsReportName == "LIST OF RECEIVABLES BY OWNERSHIP KIND")
                {
                    dlg7.m_bIsList = true;
                    dlg7.mv_iRadioBrgy = 3;
                    dlg7.OnPrintReportListOfCollectiblesByBarangay(m_sToolsReportName,m_sRFUserCode);
                }
                else if (m_sToolsReportName == "LIST OF TOP N COLLECTIBLES")
                {
                    dlg7.m_bIsList = true;
                    dlg7.mv_iRadioBrgy = 5;
                    // ALJ 07262006 (s) Get Input Class 
                    CGetInput dlgGetInput;
                    dlgGetInput.m_sDialogTitle = "TOP COLLECTIBLES";
                    dlgGetInput.m_sLabel = "Top :";
                    dlgGetInput.DoModal();
                    if (!dlgGetInput.mv_sInput.IsEmpty())
                    {
                        dlg7.mv_sReportName = m_sToolsReportName;
                        dlg7.m_sUserCode = m_sRFUserCode;
                        dlg7.mv_sTop = dlgGetInput.mv_sInput;
                        dlg7.OnReportListOfTopNCollectibles();
                    }
                    // ALJ 07262006 (e) Get Input Class 
                    //dlg7.OnPrintReportListOfCollectiblesByBarangay(m_sToolsReportName,m_sRFUserCode); // ALJ 07262006 put "//"
                }
                else if (m_sToolsReportName == "SUMMARY OF COLLECTIBLES BY DISTRICT")
                {
                    dlg7.m_bIsList = false;
                    dlg7.mv_iRadioBrgy = 4;
                    dlg7.OnPrintReportListOfCollectiblesByBarangay(m_sToolsReportName,m_sRFUserCode);
                }
                else if (m_sToolsReportName == "SUMMARY OF COLLECTIBLES BY BARANGAY")
                {
                    dlg7.m_bIsList = false;
                    dlg7.mv_iRadioBrgy = 0;
                    dlg7.OnPrintReportListOfCollectiblesByBarangay(m_sToolsReportName,m_sRFUserCode);
                }
                else if (m_sToolsReportName == "SUMMARY OF COLLECTIBLES BY MAIN BUSINESS")
                {
                    dlg7.m_bIsList = false;
                    dlg7.mv_iRadioBrgy = 1;
                    dlg7.OnPrintReportListOfCollectiblesByBarangay(m_sToolsReportName,m_sRFUserCode);
                }
                else if (m_sToolsReportName == "SUMMARY OF COLLECTIBLES BY BUSINESS STATUS")
                {
                    dlg7.m_bIsList = false;
                    dlg7.mv_iRadioBrgy = 2;
                    dlg7.OnPrintReportListOfCollectiblesByBarangay(m_sToolsReportName,m_sRFUserCode);
                }
                else if (m_sToolsReportName == "SUMMARY OF COLLECTIBLES BY OWNERSHIP KIND")
                {
                    dlg7.m_bIsList = false;
                    dlg7.mv_iRadioBrgy = 3;
                    dlg7.OnPrintReportListOfCollectiblesByBarangay(m_sToolsReportName,m_sRFUserCode);
                }
                else if (m_sToolsReportName == "USERS LOG BY USER")               
                    dlg8.OnPrintReportAuditTrailUser(m_sToolsReportName,m_sRFUserCode);
                else if (m_sToolsReportName == "USERS LOG BY TRANSACTION")               
                    dlg8.OnPrintReportAuditTrailTransaction(m_sToolsReportName,m_sRFUserCode);
                */
                #endregion
            }
            else //ASS
            {
                frmBussReport dlg = new frmBussReport();
                //BusinessRoll.frmReport dlg1 = null;
                BusinessSummary.frmReport dlg2 = null;
                //CReportSummaryOfAssessment dlg4;
                //CPrintingSchedule dlg5; //LEO 07152003
                //CReportsListOfAssessment dlg6; // ALJ 09192003
                //CUsersLog dlg7;

                if (m_sToolsReportName == "LIST OF BUSINESSES BY BARANGAY")
                {
                    dlg.ReportSwitch = "ListBnsBrgy";//39;
                    dlg.ReportName = m_sToolsReportName;
                    dlg.ShowDialog();
                }
                else if (m_sToolsReportName == "LIST OF BUSINESSES BY DISTRICT")
                {
                    dlg.ReportSwitch = "ListBnsDist";//39;
                    dlg.ReportName = m_sToolsReportName;
                    dlg.ShowDialog();
                }
                else if (m_sToolsReportName == "LIST OF BUSINESSES BY MAIN BUSINESS")
                {
                    dlg.ReportSwitch = "ListBnsMainBusiness";//39;
                    dlg.ReportName = m_sToolsReportName;
                    dlg.ShowDialog();
                }
                else if (m_sToolsReportName == "LIST OF BUSINESSES BY BUSINESS STATUS")
                {
                    dlg.ReportSwitch = "ListBnsBusStatus";//39;
                    dlg.ReportName = m_sToolsReportName;
                    dlg.ShowDialog();
                }
                else if (m_sToolsReportName == "LIST OF BUSINESSES BY ORGANIZATION KIND")
                {
                    dlg.ReportSwitch = "ListBnsOrgKind";//39;
                    dlg.ReportName = m_sToolsReportName;
                    dlg.ShowDialog();
                }
                else if (m_sToolsReportName == "LIST OF BUSINESSES BY GROSS RECEIPTS")
                {
                    dlg.ReportSwitch = "ListBnsGrossReceipt";//39;
                    dlg.ReportName = m_sToolsReportName;
                    dlg.ShowDialog();
                }
                #region comments
                //else if (m_sToolsReportName == "BUSINESS ROLL BY BARANGAY")
                //    dlg2.OnPrintReportBnsRollBrgy(m_sToolsReportName, m_sRFUserCode);
                //else if (m_sToolsReportName == "BUSINESS ROLL BY DISTRICT")
                //    dlg2.OnPrintReportBnsRollDistrict(m_sToolsReportName, m_sRFUserCode);
                //else if (m_sToolsReportName == "BUSINESS ROLL BY MAIN BUSINESS")
                //    dlg2.OnPrintReportBnsRollMainBusiness(m_sToolsReportName, m_sRFUserCode);
                //else if (m_sToolsReportName == "BUSINESS ROLL BY STREET")
                //    dlg2.OnPrintReportBnsRollStreet(m_sToolsReportName, m_sRFUserCode);
                //else if (m_sToolsReportName == "BUSINESS ROLL BY TOP GROSS")
                //    dlg2.OnPrintReportBnsRollTopGross(m_sToolsReportName, m_sRFUserCode);
                //else if (m_sToolsReportName == "BUSINESS ROLL BY TOP PAYER")
                //    dlg2.OnPrintReportBnsRollTopPayer(m_sToolsReportName, m_sRFUserCode);
                //else if (m_sToolsReportName == "BUSINESS ROLL BY LEAST 20% PAYERS FOR 2004")
                //    dlg2.OnPrintReportBnsRollLeastPayer(m_sToolsReportName, m_sRFUserCode);
                //else if (m_sToolsReportName == "BUSINESS ROLL BY TOP 20% PAYERS FOR 2004")
                //    dlg2.OnPrintReportBnsRollTopPayer(m_sToolsReportName, m_sRFUserCode);
                //else if (m_sToolsReportName == "BUSINESS ROLL BY TOP 5% PAYERS FOR 2004")
                //    dlg2.OnPrintReportBnsRollLeastGross(m_sToolsReportName, m_sRFUserCode);
                #endregion
                else if (m_sToolsReportName == "SUMMARY OF BUSINESSES BY BARANGAY")
                {
                    dlg2 = new BusinessSummary.frmReport(BusinessSummary.frmReport.ReportType.By_Barangay, "");
                    dlg2.ShowDialog();
                }
                else if (m_sToolsReportName == "SUMMARY OF BUSINESSES BY DISTRICT")
                {
                    dlg2 = new BusinessSummary.frmReport(BusinessSummary.frmReport.ReportType.By_District, "");
                    dlg2.ShowDialog();
                }
                else if (m_sToolsReportName == "SUMMARY OF BUSINESSES BY MAIN BUSINESS")
                {
                    dlg2 = new BusinessSummary.frmReport(BusinessSummary.frmReport.ReportType.By_Line_Of_Business, "");
                    dlg2.ShowDialog();
                }
                else if (m_sToolsReportName == "SUMMARY OF BUSINESSES BY OWNERSHIPKIND")
                {
                    dlg2 = new BusinessSummary.frmReport(BusinessSummary.frmReport.ReportType.By_Org_Kind, "");
                    dlg2.ShowDialog();
                }
                else if (m_sToolsReportName == "SUMMARY OF BUSINESSES BY GROSS RANGE")
                {
                    dlg2 = new BusinessSummary.frmReport(BusinessSummary.frmReport.ReportType.By_Gross_Receipts, "");
                    dlg2.ShowDialog();
                }
                else if (m_sToolsReportName == "SUMMARY OF BUSINESSES BY CAPITAL RANGE")
                {
                    dlg2 = new BusinessSummary.frmReport(BusinessSummary.frmReport.ReportType.By_Initial_Capital, "");
                    dlg2.ShowDialog();
                }
                else //JARS 201708330 FOR INFORMATION OF REPORT/FILES THAT ARE NOT GENERATING.
                {
                    MessageBox.Show("For Re-printing of this file/report, Please proceed to the respective module.", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                #region comments
                //else if (m_sToolsReportName == "SUMMARY OF ASSESSMENT BY BARANGAY")
                //{
                //    dlg4.OnPrintReportSummaryOfAssessment(m_sToolsReportName, m_sRFUserCode);
                //}
                //else if (m_sToolsReportName == "LIST OF ASSESSMENT BY BARANGAY")
                //{
                //    dlg6.OnPrintReportListOfAssessmentByBarangay(m_sToolsReportName, m_sRFUserCode);
                //}
                //else if (m_sToolsReportName == "USERS LOG BY USER")
                //{
                //    dlg7.OnPrintReportAuditTrailUser(m_sToolsReportName, m_sRFUserCode);
                //}
                //else if (m_sToolsReportName == "USERS LOG BY TRANSACTION")
                //{
                //    dlg7.OnPrintReportAuditTrailTransaction(m_sToolsReportName, m_sRFUserCode);
                //}
                //if (m_sToolsReportName == "SCHEDULE OF RATES FOR " + pApp->mg_sTaxHeader)
                //{
                //    dlg5.OnPrintScheduleLicense();
                //}
                //if (m_sToolsReportName == "SCHEDULE OF RATES FOR FEES BY CATEGORY")
                //{
                //    dlg5.OnPrintScheduleFees();
                //}
                //if (m_sToolsReportName == "SCHEDULE OF RATES FOR ADDITIONAL CHARGES")
                //{
                //    dlg5.OnPrintScheduleAddl();
                //}
                #endregion
            }
        }
    }
}