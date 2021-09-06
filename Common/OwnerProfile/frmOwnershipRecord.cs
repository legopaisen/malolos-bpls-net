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
using Amellar.Common.AuditTrail;
using Amellar.Common.SearchOwner;

namespace Amellar.Modules.OwnerProfile
{
    public partial class frmOwnershipRecord : Form
    {
        public frmOwnershipRecord()
        {
            InitializeComponent();
        }

        string m_sOwnCode;
        string m_sOwnName;

        private void frmOwnershipRecord_Load(object sender, EventArgs e)
        {

        }

        private void btnSearchOwner_Click(object sender, EventArgs e)
        {
            String sQuery;

            if (txtAcctCode.Text == String.Empty && txtOwName.Text == String.Empty)
            {
                frmSearchOwner frmSearchOwner = new frmSearchOwner();
                frmSearchOwner.ShowDialog();
                txtAcctCode.Text = frmSearchOwner.m_strOwnCode;
                txtOwName.Text = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwner(txtAcctCode.Text));
                txtOwAdd.Text = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwnAdd(txtAcctCode.Text));
            }
            else
            {
                txtOwName.Text = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwner(txtAcctCode.Text));
                txtOwAdd.Text = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwnAdd(txtAcctCode.Text));
            }
        }

        private void txtAcctCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
               && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void axVSPrinter1_BeforeFooter(object sender, EventArgs e)
        {
            this.axVSPrinter1.HdrFontName = "Arial Narrow";
            this.axVSPrinter1.HdrFontSize = (float)8.0;
            this.axVSPrinter1.HdrFontItalic = true;
        }

        private void EnableControls(bool blnEnable)
        {
            txtAcctCode.ReadOnly = blnEnable;
            txtOwName.ReadOnly = blnEnable;
            btnSearchOwner.Enabled = blnEnable;
        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            txtAcctCode.Text = "";
            txtOwAdd.Text = "";
            txtOwName.Text = "";
            txtAcctCode.Enabled = true;
            EnableControls(false);
            btnSearchOwner.Enabled = true;
        }

        private void txtAcctCode_TextChanged(object sender, EventArgs e)
        {
            if (txtAcctCode.Text.Trim() == String.Empty)
            {
                txtOwName.Text = "";
                txtOwAdd.Text = "";
                txtOwName.ReadOnly = false;
            }
            else
            {
                txtOwName.Text = "";
                txtOwAdd.Text = "";
                EnableControls(true);
                txtAcctCode.ReadOnly = false;
            }
        }

        private void CreateHeader()
        {
            String sHeader, sHeader1, sHeader2, sHeader3;

            this.axVSPrinter1.FontName = "Arial Narrow";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.axVSPrinter1.FontSize = 10;
            sHeader1 = "^11000;Republic of the Philippines";
            this.axVSPrinter1.Table = (sHeader1);
            sHeader = "^11000;" + StringUtilities.HandleApostrophe(AppSettingsManager.GetConfigValue("02"));
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.Table = (sHeader);
            this.axVSPrinter1.FontBold = false;

            sHeader2 = "^11000;Office of the City Treasurer";
            this.axVSPrinter1.Table = (sHeader2);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontBold = true;

            sHeader3 = "^11000;BUSINESS OWNERSHIP RECORD FORM";
            this.axVSPrinter1.Table = (sHeader3);
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = 8;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Table = ("<2000|<9000;Account Code  |" + m_sOwnCode);
            this.axVSPrinter1.Table = ("<2000|<9000;Owner's Name  |" + StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwner(m_sOwnCode)));
            this.axVSPrinter1.Table = ("<2000|<9000;Owner's Address  |" + StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwnAdd(m_sOwnCode)));
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
            this.axVSPrinter1.Table = ("<1500|<2500|<2500|<1500|<1300|<1800;Bin|Business Name|Address|Kind|Date Oper.|Line/s of Business");
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            this.axVSPrinter1.FontBold = false;
        }

        private void txtOwName_TextChanged(object sender, EventArgs e)
        {
            if (txtOwName.Text.Trim() == String.Empty)
            {
                txtAcctCode.Text = "";
                txtOwAdd.Text = "";
                EnableControls(true);
                txtOwName.ReadOnly = false;
            }
            else
            {
                EnableControls(false);
                txtAcctCode.ReadOnly = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtOwName.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Please enter the Owner's Last Name.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                return;
            }

            this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;

            this.axVSPrinter1.MarginLeft = 600;
            this.axVSPrinter1.MarginTop = 1000;
            this.axVSPrinter1.MarginBottom = 2000;

            //CreateHeader();
            this.axVSPrinter1.FontName = "Arial";
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;


            //---------- CONTENT ----------//{
            String sQuery, sContent, sBin, sBnsName, sKind, sDateOpr, sBnsCode, sBnsDesc, sBnsAdd, sAddlBns, sTaxYear, sCount;
            String sTemp = "";
            int iCount = 0;

            sContent = "<1500|<2500|<2500|<1500|<1300|<1800;";

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();

            if (txtAcctCode.Text.Trim() != String.Empty)
            {
                m_sOwnCode = txtAcctCode.Text;

                if (sTemp == "")
                {
                    sTemp = m_sOwnCode;
                    CreateHeader();
                }
                else if (sTemp != m_sOwnCode)
                {
                    sTemp = m_sOwnCode;
                    this.axVSPrinter1.NewPage();
                    CreateHeader();
                }

                //this.axVSPrinter1.FontSize = 10;
                //this.axVSPrinter1.Table = ("<9000;" + m_sOwnCode);
                //this.axVSPrinter1.Table = ("<9000;" + StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwner(m_sOwnCode)));
                //this.axVSPrinter1.Table = ("<9000;" + StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwnAdd(m_sOwnCode)));

                sQuery = string.Format("select * from businesses where own_code = '{0}'", m_sOwnCode);

                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        iCount++;
                        sBin = StringUtilities.HandleApostrophe(pSet.GetString("bin"));
                        sBnsName = StringUtilities.HandleApostrophe(pSet.GetString("bns_nm"));
                        sBnsCode = StringUtilities.HandleApostrophe(pSet.GetString("bns_code"));
                        sKind = StringUtilities.HandleApostrophe(pSet.GetString("orgn_kind"));
                        sDateOpr = pSet.GetDateTime("dt_operated").ToShortDateString();
                        sTaxYear = StringUtilities.HandleApostrophe(pSet.GetString("tax_year"));
                        sBnsAdd = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsAddress(sBin));
                        sBnsDesc = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsDesc(sBnsCode));

                        sContent += sBin + "|" + sBnsName + "|" + sBnsAdd + "|";
                        sContent += sKind + "|" + sDateOpr + "|" + sBnsDesc;
                        this.axVSPrinter1.FontSize = 7;
                        this.axVSPrinter1.Table = sContent;

                        sContent = "<1500|<2500|<2500|<1500|<1300|<1800;";

                        sQuery = string.Format("select * from addl_bns where bin = '{0}' and tax_year = '{1}'", sBin, sTaxYear);
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                        {
                            while (pSet1.Read())
                            {
                                //iCount++;
                                sBnsCode = StringUtilities.HandleApostrophe(pSet1.GetString("bns_code_main"));
                                sBnsDesc = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsDesc(sBnsCode));
                                sContent += "|||||" + sBnsDesc;
                                this.axVSPrinter1.Table = sContent;
                                sContent = "<1500|<2500|<2500|<1500|<1300|<1800;";
                            }
                        }
                        pSet1.Close();
                    }
                }
                else
                {
                    MessageBox.Show("No record found.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                pSet.Close();

                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                this.axVSPrinter1.Table = ("<11000;  ");
                this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = ("<5000;Total Number of Businesses: " + iCount.ToString());
                iCount = 0;
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Table = "<1500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
                this.axVSPrinter1.Table = "<1500|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
            }
            else
            {
                string sTempOwnName = String.Empty;
                if (txtOwName.Text.Trim() == String.Empty)
                    sTempOwnName = "%";
                else
                    sTempOwnName = txtOwName.Text.Trim();

                m_sOwnName = StringUtilities.HandleApostrophe(sTempOwnName);
                sQuery = string.Format("select * from own_names where own_ln like '{0}'", m_sOwnName);
                pSet.Query = sQuery;
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        m_sOwnCode = StringUtilities.HandleApostrophe(pSet.GetString("own_code"));

                        if (sTemp == "")
                        {
                            sTemp = m_sOwnCode;
                            CreateHeader();
                        }
                        else if (sTemp != m_sOwnCode)
                        {
                            sTemp = m_sOwnCode;
                            this.axVSPrinter1.NewPage();
                            CreateHeader();
                            //this.axVSPrinter1.NewPageEvent += new EventHandler(axVSPrinter1_NewPageEvent);
                        }

                        //this.axVSPrinter1.FontSize = 10;
                        //this.axVSPrinter1.Table = ("<9000;" + m_sOwnCode);
                        //this.axVSPrinter1.Table = ("<9000;" + StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwner(m_sOwnCode)));
                        //this.axVSPrinter1.Table = ("<9000;" + StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsOwnAdd(m_sOwnCode)));

                        iCount = 0;

                        sQuery = string.Format("select * from businesses where own_code = '{0}'", m_sOwnCode);
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                        {
                            while (pSet1.Read())
                            {
                                iCount++;
                                sBin = StringUtilities.HandleApostrophe(pSet1.GetString("bin"));
                                sBnsName = StringUtilities.HandleApostrophe(pSet1.GetString("bns_nm"));
                                sBnsCode = StringUtilities.HandleApostrophe(pSet1.GetString("bns_code"));
                                sKind = StringUtilities.HandleApostrophe(pSet1.GetString("orgn_kind"));
                                sDateOpr = pSet1.GetDateTime("dt_operated").ToShortDateString();
                                sTaxYear = StringUtilities.HandleApostrophe(pSet1.GetString("tax_year"));
                                sBnsAdd = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsAddress(sBin));
                                sBnsDesc = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsDesc(sBnsCode));

                                sContent += sBin + "|" + sBnsName + "|" + sBnsAdd + "|";
                                sContent += sKind + "|" + sDateOpr + "|" + sBnsDesc;
                                this.axVSPrinter1.Table = sContent;

                                sContent = "<1500|<2500|<2500|<1500|<1300|<1800;";

                                sQuery = string.Format("select * from addl_bns where bin = '{0}' and tax_year = '{1}'", sBin, sTaxYear);
                                OracleResultSet pSet2 = new OracleResultSet();
                                pSet2.Query = sQuery;
                                if (pSet2.Execute())
                                {
                                    while (pSet2.Read())
                                    {
                                        sBnsCode = StringUtilities.HandleApostrophe(pSet2.GetString("bns_code_main"));
                                        sBnsDesc = StringUtilities.HandleApostrophe(AppSettingsManager.GetBnsDesc(sBnsCode));
                                        sContent += "|||||" + sBnsDesc;
                                        this.axVSPrinter1.Table = sContent;
                                        sContent = "<1500|<2500|<2500|<1500|<1300|<1800;";
                                    }
                                }
                                pSet2.Close();
                            }

                            this.axVSPrinter1.Paragraph = "";
                            this.axVSPrinter1.Paragraph = "";
                            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTop;
                            this.axVSPrinter1.Table = ("<11000;  ");
                            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                            this.axVSPrinter1.FontBold = true;
                            this.axVSPrinter1.Table = ("<5000;Total Number of Businesses: " + iCount.ToString());
                            iCount = 0;
                            this.axVSPrinter1.Paragraph = "";
                            this.axVSPrinter1.Table = "<1500|<5000;Printed by :|" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserName);
                            this.axVSPrinter1.Table = "<1500|<5000;Date printed :|" + AppSettingsManager.GetSystemDate();
                        }
                        pSet1.Close();
                    }
                }
                else
                {
                    MessageBox.Show("No record found.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                pSet.Close();
            }
            //---------- CONTENT ----------//}
            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private void axVSPrinter1_NewPageEvent(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(500);

            CreateHeader();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ToolPrint_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintQuality = VSPrinter7Lib.PrintQualitySettings.pqHigh;

            if (MessageBox.Show("Are you sure you want to print?", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            axVSPrinter1.PrintDoc(1,1,axVSPrinter1.PageCount);
        }

        private void toolSettingPageSetup_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPageSetup);
        }

        private void toolSettingPrintPage_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPrinterSetup);
        }

    }
}