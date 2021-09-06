using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using Amellar.Common.PrintUtilities;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Common.DeficientRecords
{
    //@author R.D.Ong
    public partial class frmDeficientRecordsToolReport : Form
    {

        //hashmap too difficult to manage
        private List<string> m_lstCategoryCodes;
        private List<string> m_lstCategoryNames;

        private List<string> m_lstDeficientTypeCodes;
        private List<string> m_lstDeficientTypeNames;

        private VSPrinterEmuModel model;

        
        public frmDeficientRecordsToolReport()
        {
            InitializeComponent();

            m_lstCategoryCodes = new List<string>();
            m_lstCategoryNames = new List<string>();

            m_lstDeficientTypeCodes = new List<string>();
            m_lstDeficientTypeNames = new List<string>();

            model = new VSPrinterEmuModel();
            model.LeftMargin = 7;
            model.TopMargin = 7;
            model.MaxY = 1100 - 7;

            //load all categories
            this.LoadCategories();

            cmbCategory.Enabled = chkCategory.Checked;
            if (!chkCategory.Checked && cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;
            cmbType.Enabled = chkType.Checked;
            if (!chkType.Checked && cmbType.Items.Count > 0)
                cmbType.SelectedIndex = 0;
            chkType.Enabled = false;
            chkSubType.Enabled = false;

            btnPrint.Enabled = !(!chkCategory.Checked && !chkType.Checked && !chkSubType.Checked);

        }


        private void LoadCategories()
        {
            m_lstCategoryCodes.Clear();
            m_lstCategoryNames.Clear();
            cmbCategory.Items.Clear();

            cmbCategory.Items.Add("ALL");
            OracleResultSet result = new OracleResultSet();
            //result.Query = "SELECT cat_code, cat_name FROM def_cat_table WHERE sys_type = 'C' ORDER BY cat_code";
            result.Query = "SELECT cat_code, cat_name FROM def_cat_table ORDER BY cat_code";    // RMC 20110815 
            if (result.Execute())
            {
                while (result.Read())
                {
                    m_lstCategoryCodes.Add(result.GetString("cat_code").Trim());
                    m_lstCategoryNames.Add(result.GetString("cat_name").Trim());
                    cmbCategory.Items.Add(m_lstCategoryNames[m_lstCategoryNames.Count - 1]);
                }
            }
            result.Close();

        }

        private void LoadTypes(string strCategoryCode)
        {
            m_lstDeficientTypeCodes.Clear();
            m_lstDeficientTypeNames.Clear();
            cmbType.Items.Clear();

            cmbType.Items.Add("ALL");
            OracleResultSet result = new OracleResultSet();
            //result.Query = string.Format("SELECT def_code, def_name FROM def_rec_table WHERE sys_type = 'C' AND cat_code = '{0}' ORDER BY def_code", strCategoryCode);
            result.Query = string.Format("SELECT def_code, def_name FROM def_rec_table WHERE cat_code = '{0}' ORDER BY def_code", strCategoryCode);  // RMC 20110815
            if (result.Execute())
            {
                while (result.Read())
                {
                    m_lstDeficientTypeCodes.Add(result.GetString("def_code").Trim());
                    m_lstDeficientTypeNames.Add(result.GetString("def_name").Trim());
                    cmbType.Items.Add(m_lstDeficientTypeNames[m_lstDeficientTypeNames.Count - 1]);
                }
            }
            result.Close();
        }


        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            VSPrinterEmuDocument doc = (VSPrinterEmuDocument)sender;
            doc.Render(e);
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            //add check if there is existing records
            model.Clear();
            model.SetFontName("Arial Narrow");
            model.SetFontSize(10);
            model.SetCurrentY(1000);

            model.SetTable("^9000;Republic of the Philippines");
            model.SetTable(string.Format("^9000;{0}", ConfigurationAttributes.LGUName));
            //string strProvinceName = ConfigurationAttributes.ProvinceName;
            string strProvinceName = AppSettingsManager.GetConfigValue("08");   // RMC 20110815
            if (strProvinceName != string.Empty)
            {
                model.SetTable(string.Format("^9000;{0}", strProvinceName));
            }
            //model.SetTable(string.Format("^9000;{0}",ConfigurationAttributes.OfficeName));    // RMC 20110809 deleted
            model.SetTable(string.Empty);
            model.SetFontBold(1);
            model.SetTable("^9000;Master List of Deficient Records By Category");
            model.SetFontBold(0);
            model.SetTable(string.Empty);

            model.SetFontUnderline(1);
            string strTypeLine = string.Empty;
            if (chkCategory.Checked && !chkType.Checked && !chkSubType.Checked)
            {
                model.SetMarginLeft(3000);
                model.SetTable("^1500|<5000|^1000;Category Code|Description|");
            }
            else if (chkCategory.Checked && chkType.Checked && !chkSubType.Checked)
            {
                model.SetMarginLeft(2000);
                model.SetTable("^1500|^1500|<5000;Category|Deficiency Code|Description");
                strTypeLine = "^1500|^1500|<5000;|{0}|{1}";
            }
            else
            {
                model.SetTable("^1500|^1500|^1500|^5000;Category|Def. Code|AdInfo Code|Additional Info Desc");
                strTypeLine = "^1000|<1000|<5000;|{0}|{1}";
            }
            model.SetFontUnderline(0);
            model.SetTable(string.Empty);

            string strCategoryCode = string.Empty;
            string strDeficientTypeCode = string.Empty;

            strCategoryCode = txtCategory.Text.Trim();
            strDeficientTypeCode = txtType.Text.Trim();

            OracleResultSet result = new OracleResultSet();
            int intCount = 1;
            if (strCategoryCode == string.Empty)
                intCount = m_lstCategoryCodes.Count;

            string strTmpCategoryCode = string.Empty;
            string strTmpDeficientTypeCode = string.Empty;


            for (int i = 0; i < intCount; i++)
            {
                if (strTypeLine != string.Empty)
                    model.SetFontBold(1);

                if (strCategoryCode == string.Empty)
                {
                    model.SetTable(string.Format("^1500|<5000;{0}|{1}", m_lstCategoryCodes[i], m_lstCategoryNames[i]));
                    strTmpCategoryCode = m_lstCategoryCodes[i];
                }
                else
                {
                    int intCategoryIndex = cmbCategory.SelectedIndex - 1;
                    strTmpCategoryCode = strCategoryCode;
                    model.SetTable(string.Format("^1500|<5000;{0}|{1}", strCategoryCode, m_lstCategoryNames[intCategoryIndex]));
                }
                if (strTypeLine != string.Empty)
                {
                    model.SetFontBold(0);
                    model.SetTable(string.Empty);
                }

                if (chkType.Checked)
                {
                    if (strDeficientTypeCode == string.Empty)
                        //result.Query = string.Format("SELECT def_code, def_name FROM def_rec_table WHERE sys_type = 'C' AND cat_code = '{0}'  ORDER BY def_code", strTmpCategoryCode);
                        result.Query = string.Format("SELECT def_code, def_name FROM def_rec_table WHERE cat_code = '{0}'  ORDER BY def_code", strTmpCategoryCode);  // RMC 20110815
                    else
                        //result.Query = string.Format("SELECT def_code, def_name FROM def_rec_table WHERE sys_type = 'C' AND cat_code = '{0}' AND def_code = '{1}' ORDER BY def_code", strTmpCategoryCode, 
                        result.Query = string.Format("SELECT def_code, def_name FROM def_rec_table WHERE cat_code = '{0}' AND def_code = '{1}' ORDER BY def_code", strTmpCategoryCode,   // RMC 20110815
                            strDeficientTypeCode);
                    if (result.Execute())
                    {
                        while (result.Read())
                        {
                            strTmpDeficientTypeCode = result.GetString("def_code").Trim();
                            model.SetTable(string.Format(strTypeLine, strTmpDeficientTypeCode, result.GetString("def_name").Trim()));

                            if (chkSubType.Checked)
                            {
                                OracleResultSet result2 = new OracleResultSet();
                                /*result2.Query = string.Format("SELECT adinfo_code, adinfo_name FROM def_adinfo_table WHERE sys_type = 'C' AND cat_code = '{0}' AND def_code = '{1}' ORDER BY adinfo_code",
                                    strTmpCategoryCode, strTmpDeficientTypeCode);*/
                                result2.Query = string.Format("SELECT adinfo_code, adinfo_name FROM def_adinfo_table WHERE def_code = '{0}' ORDER BY adinfo_code", strTmpDeficientTypeCode);    // RMC 20110815
                                if (result2.Execute())
                                {
                                    while (result2.Read())
                                    {
                                        model.SetTable(string.Format("^1500|^1500|^1500|<5000;||{0}|{1}", result2.GetString("adinfo_code").Trim(),
                                            result2.GetString("adinfo_name").Trim()));
                                    }
                                }
                                result2.Close();
                            }
                        }
                    }
                }
                if (strTypeLine != string.Empty)
                    model.SetTable(string.Empty);
            }

            result.Close();

            model.SetTable(string.Empty);
            model.SetTable(string.Format("<2000|<5000;Printed By:|{0}", AppSettingsManager.SystemUser.UserName));
            model.SetTable(string.Format("<2000|<5000;Printed Date:|{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate()));


            //print 
            VSPrinterEmuDocument doc = new VSPrinterEmuDocument();
            doc.Model = model;
            doc.Model.Reset();
            doc.DefaultPageSettings.Landscape = false;
            doc.DefaultPageSettings.Margins.Top = 7;
            doc.DefaultPageSettings.Margins.Left = 7;
            doc.DefaultPageSettings.Margins.Bottom = 7;
            doc.DefaultPageSettings.Margins.Right = 7;
            doc.DefaultPageSettings.PaperSize = new PaperSize("", 850, 1100);

            doc.PrintPage += this.Doc_PrintPage;

            frmMyPrintPreviewDialog dlgPreview = new frmMyPrintPreviewDialog();
            //System.Windows.Forms.PrintPreviewDialog dlgPreview = new System.Windows.Forms.PrintPreviewDialog();
            dlgPreview.Document = doc;
            dlgPreview.ClientSize = new System.Drawing.Size(640, 480);
            dlgPreview.ShowDialog();

            model.Dispose();
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void chkCategory_CheckedChanged(object sender, EventArgs e)
        {
            cmbCategory.Enabled = chkCategory.Checked;
            if (!chkCategory.Checked && cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;
            if (chkCategory.Checked)
            {
                chkType.Checked = false;
                chkType.Enabled = true;

                chkSubType.Checked = false;
                chkSubType.Enabled = false;
            }
            else
            {
                chkType.Checked = false;
                chkType.Enabled = false;
                cmbType.Enabled = false;
                if (cmbType.Items.Count > 0)
                    cmbType.SelectedIndex = 0;
                chkSubType.Checked = false;
                chkSubType.Enabled = false;
            }

            btnPrint.Enabled = !(!chkCategory.Checked && !chkType.Checked && !chkSubType.Checked);
        }

        private void chkType_CheckedChanged(object sender, EventArgs e)
        {
            cmbType.Enabled = chkType.Checked;
            //determine if there is subtype for this category and type

            chkSubType.Enabled = chkType.Checked;
            if (!chkType.Checked)
                chkSubType.Checked = false;

            btnPrint.Enabled = !(!chkCategory.Checked && !chkType.Checked && !chkSubType.Checked);
        }

        private void chkSubType_CheckedChanged(object sender, EventArgs e)
        {

            btnPrint.Enabled = !(!chkCategory.Checked && !chkType.Checked && !chkSubType.Checked);
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedIndex == -1 || cmbCategory.SelectedIndex == 0)
                txtCategory.Text = string.Empty;
            else
                txtCategory.Text = m_lstCategoryCodes[cmbCategory.SelectedIndex - 1];
            this.LoadTypes(txtCategory.Text);
            if (cmbType.Items.Count > 0)
                cmbType.SelectedIndex = 0;
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbType.SelectedIndex == -1 || cmbType.SelectedIndex == 0)
                txtType.Text = string.Empty;
            else
                txtType.Text = m_lstDeficientTypeCodes[cmbType.SelectedIndex - 1];
        }
    }
}