
// RMC 20120111 corrected saving of scheds
// RMC 20120109 added trim in saving bns schedule

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.Utilities
{
    public class License:Schedule
    {
        OracleResultSet pCmd = new OracleResultSet();     // RMC 20111012 modified query commit in scheds
        string m_strModCode = string.Empty;   // RMC 20180130 added trail in License schedule

        public License(frmSchedule Form): base(Form)
        {
        }
        
        public override void FormLoad()
        {
            string strQuery = string.Empty;

            m_strModCode = "";    // RMC 20180130 added trail in License schedule
            this.ScheduleFrm.LabelOne.Text = "Business Type";
            this.ScheduleFrm.LabelTwo.Text = "Schedules";
            this.ScheduleFrm.Label12.Text = "Sub Categories";
            this.EnableControls(false);
            ScheduleFrm.CheckGross.Checked = true;
            this.LoadBussType();
            this.LoadSchedule();
            this.LoadSurchIntDisc();
            this.LoadNew(); // GDE 20130614
            this.LoadMinMaxTax("B", ScheduleFrm.SelectedBnsSubCode);
        }

        public override void EnableControls(bool blnEnable)
        {
            this.ScheduleFrm.TextLicMinTax.Enabled = blnEnable;
            this.ScheduleFrm.TextLicNewRate.Enabled = blnEnable;
            this.ScheduleFrm.TextInterest.Enabled = blnEnable;
            this.ScheduleFrm.TextSurcharge.Enabled = blnEnable;
            this.ScheduleFrm.TextDiscount.Enabled = blnEnable;
            this.ScheduleFrm.TextExcess.Enabled = blnEnable;
            this.ScheduleFrm.TextAddExcess.Enabled = blnEnable;
            this.ScheduleFrm.CheckGross.Enabled = blnEnable;
            this.ScheduleFrm.CheckLicQtrDec.Enabled = blnEnable;

            this.ScheduleFrm.ListOne.ReadOnly = !blnEnable;
            this.ScheduleFrm.ListTwo.ReadOnly = !blnEnable;

            this.ScheduleFrm.LabelNew.Text = "New Business";
            this.ScheduleFrm.LabelNew.Visible = true;
            this.ScheduleFrm.LabelNewRate.Visible = true;
            this.ScheduleFrm.TextLicNewRate.Visible = true;
            this.ScheduleFrm.LabelNewMinTax.Visible = true;
            this.ScheduleFrm.TextLicMinTax.Visible = true;
            this.ScheduleFrm.CheckLicQtrDec.Visible = true;
            this.ScheduleFrm.CheckFeesMonth.Visible = false;
            this.ScheduleFrm.CheckFeesQtr.Visible = false;
            this.ScheduleFrm.CheckFeesYear.Visible = false;
            this.ScheduleFrm.ButtonConfigQtr.Visible = false;

            this.ScheduleFrm.TextBnsCode.Enabled = false;
            this.ScheduleFrm.TextRevYear.Enabled = false;
            this.ScheduleFrm.TextFeesCode.Enabled = false;
            this.ScheduleFrm.ComboFeesDesc.Enabled = false;
            this.ScheduleFrm.CheckFeesInt.Enabled = false;
            this.ScheduleFrm.CheckFeesQtr.Enabled = false;
            this.ScheduleFrm.CheckFeesSurch.Enabled = false;

            if(ScheduleFrm.CheckGross.Checked == false)   // RMC 20170112 temp
                this.ScheduleFrm.ButtonConfig.Enabled = blnEnable;
            else
                this.ScheduleFrm.ButtonConfig.Enabled = false;

            this.ScheduleFrm.TextMinTax.Enabled = blnEnable;
            this.ScheduleFrm.TextMaxTax.Enabled = blnEnable;

        }

        public override void SelChangeBnsDesc()
        {
            OracleResultSet result = new OracleResultSet();
            
            this.InitializeButtons();
            this.EnableControls(false);

            result.Query = string.Format("select * from bns_table where fees_code = 'B' and trim(bns_desc) = '{0}' and rev_year = '{1}' order by means", ScheduleFrm.ComboBnsDesc.Text, ScheduleFrm.RevYear);
            if (result.Execute())
            {
                if (result.Read())
                {
                    ScheduleFrm.BnsCode = result.GetString("bns_code");
                }
            }
            result.Close();

            result.Query = string.Format("select * from fix_sched where trim(bns_code) like '{0}%' and rev_year = '{1}'", StringUtilities.Left(ScheduleFrm.BnsCode,2), ScheduleFrm.RevYear);
            if (result.Execute())
            {
                if (result.Read())
                {
                    ScheduleFrm.CheckGross.Checked = false;
                }
                else
                {
                    ScheduleFrm.CheckGross.Checked = true;
                }
            }
            result.Close();

            this.LoadBussType();
            if (ScheduleFrm.CheckGross.Checked)
            {
                this.LoadSchedule();
                this.LoadExcess();
            }
            else
            {
                this.LoadFixSched(0);
            }

            this.LoadNew();
            this.LoadSurchIntDisc();
            this.LoadMinMaxTax("B", ScheduleFrm.SelectedBnsSubCode);
        }

        private void LoadBussType() //malolos
        {
            OracleResultSet result = new OracleResultSet();
                        
            this.ClearLists();
            string test = "";
            if (ScheduleFrm.CheckGross.Checked)
            {
                StringBuilder strQuery = new StringBuilder();
                strQuery.Append("select distinct(a.bns_code) \" Code\", b.bns_desc \" Sub Categories\" ");
                strQuery.Append(string.Format("from btax_sched a, bns_table b where substr(a.bns_code,1,2) = '{0}' ", ScheduleFrm.BnsCode));
                strQuery.Append(string.Format("and a.rev_year = '{0}' and length(trim(a.bns_code)) <> 2 and b.fees_code = 'B' ", ScheduleFrm.RevYear));
                strQuery.Append("and a.bns_code = b.bns_code and a.rev_year = b.rev_year order by a.bns_code");
                result.Query = strQuery.ToString();

                if (result.Execute())
                {
                    while (result.Read())
                    {
                        ScheduleFrm.ListOne.Rows.Add(result.GetString(0), result.GetString(1));
                    }
                }
                result.Close();
            }
            else
            {
                StringBuilder strQuery = new StringBuilder();
                strQuery.Append("select distinct(a.bns_code) \" Code\", b.bns_desc \" Sub Categories\" ,b.means \" Type\" ");
                strQuery.Append(string.Format("from fix_sched a, bns_table b where substr(a.bns_code,1,2) = '{0}' ", ScheduleFrm.BnsCode));
                strQuery.Append(string.Format("and a.rev_year = '{0}' and length(trim(a.bns_code)) <> 2 ", ScheduleFrm.RevYear));
			    strQuery.Append("and b.fees_code = 'B' and a.bns_code = b.bns_code and a.rev_year = b.rev_year order by a.bns_code");
                //strQuery.Append("and b.fees_code = 'B' and a.bns_code = b.bns_code and a.rev_year = b.rev_year  and b.bns_code not in (select bns_code from bns_table where (bns_code like '10%' or bns_code like '11%' or bns_code like '12%' or bns_code like '13%' or bns_code like '14%' or bns_code like '15%' ) and fees_code = 'B') order by a.bns_code");
                result.Query = strQuery.ToString();

                if (result.Execute())
                {
                    while (result.Read())
                    {
                        ScheduleFrm.ListOne.Rows.Add(result.GetString(0), result.GetString(1), result.GetString(2));
                    }
                }
                result.Close();

                result.Query = "delete from tmp_fix_sched";
                if (result.ExecuteNonQuery() == 0)
                {
                }

                result.Query = string.Format("insert into tmp_fix_sched select * from fix_sched where trim(bns_code) like '{0}%' and rev_year = '{1}'", ScheduleFrm.BnsCode, ScheduleFrm.RevYear);
                if (result.ExecuteNonQuery() == 0)
                {
                }
            }

            ScheduleFrm.ListOne.Rows.Add("");
        }

        private void LoadSchedule()
        {
            OracleResultSet result = new OracleResultSet();

            string sQuery = string.Empty;

            ScheduleFrm.ListTwo.Columns.Clear();
            ScheduleFrm.ListTwo.Columns.Add("GR1", "GROSS 1");
            ScheduleFrm.ListTwo.Columns.Add("GR2", "GROSS 2");
            ScheduleFrm.ListTwo.Columns.Add("XRATE", "EX-RATE");
            ScheduleFrm.ListTwo.Columns.Add("PLUS", "%PLUS");
            ScheduleFrm.ListTwo.Columns.Add("AMT", "AMOUNT");

            ScheduleFrm.ListTwo.RowHeadersVisible = false;
            ScheduleFrm.ListTwo.Columns[0].Width = 70;
            ScheduleFrm.ListTwo.Columns[1].Width = 80;
            ScheduleFrm.ListTwo.Columns[2].Width = 60;
            ScheduleFrm.ListTwo.Columns[3].Width = 60;
            ScheduleFrm.ListTwo.Columns[4].Width = 60;
            ScheduleFrm.ListTwo.Columns[4].DefaultCellStyle.Format = "c"; 

            ScheduleFrm.ListTwo.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            ScheduleFrm.ListTwo.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            ScheduleFrm.ListTwo.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            ScheduleFrm.ListTwo.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            ScheduleFrm.ListTwo.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            result.Query = string.Format("select gr_1, gr_2, ex_rate, plus_rate, amount from btax_sched "
                 + " where bns_code = '{0}' and rev_year = '{1}' order by gr_1", ScheduleFrm.BnsCode, ScheduleFrm.RevYear);
            if (result.Execute())
            {
                double dgr1 = 0;
                double dgr2 = 0;
                double dexrate = 0;
                double dplusrate = 0;
                double damt = 0;

                string sgr1, sgr2, sexrate, splusrate, samt;

                while (result.Read())
                {
                    dgr1 = result.GetDouble(0);
                    dgr2 = result.GetDouble(1);
                    dexrate = result.GetDouble(2);
                    dplusrate = result.GetDouble(3);
                    damt = result.GetDouble(4);

                    sgr1 = string.Format("{0:##0.00}", dgr1);
                    sgr2 = string.Format("{0:##0.00}", dgr2);
                    sexrate = string.Format("{0:##0.000000}", dexrate);
                    splusrate = string.Format("{0:##0.000000}", dplusrate);
                    samt = string.Format("{0:#,##0.00}", damt);

                    ScheduleFrm.ListTwo.Rows.Add(sgr1,sgr2,sexrate,splusrate,samt);
                }
            }
            result.Close();

            
            
        }

        private void LoadNew()
        {
            OracleResultSet result = new OracleResultSet();

            ScheduleFrm.LicNewRate = "0.00";
            ScheduleFrm.LicMinTax = "0.00";
                        
            result.Query = string.Format("select new_rate,min_tax,is_qtrly from new_table where rev_year = '{0}' and bns_code = '{1}'",ScheduleFrm.RevYear,ScheduleFrm.BnsCode);
            if (result.Execute())
            {
                double dnewrate = 0;
                double dnewmintax = 0;
                string sIsqtr = string.Empty;

                if (result.Read())
                {
                    dnewrate = result.GetDouble("new_rate");
                    dnewmintax = result.GetDouble("min_tax");
                    sIsqtr = result.GetString("is_qtrly");

                    ScheduleFrm.LicNewRate = string.Format("{0:##0.000000}", dnewrate);
                    ScheduleFrm.LicMinTax = string.Format("{0:##0.00}", dnewmintax);

                    if (sIsqtr == "Y")
                        ScheduleFrm.CheckLicQtrDec.Checked = true;
                    else
                        ScheduleFrm.CheckLicQtrDec.Checked = false;
                }
            }
            result.Close();

	    }

        private void LoadSurchIntDisc()
        {
            OracleResultSet result = new OracleResultSet();

            string strFeesCode = "B" + ScheduleFrm.BnsCode;
            
            ScheduleFrm.Surcharge = "";
            ScheduleFrm.Interest = "";
            ScheduleFrm.Discount = "";

            result.Query = string.Format("select * from surch_sched where rev_year = '{0}' and tax_fees_code = '{1}'", ScheduleFrm.RevYear, strFeesCode);
            if(result.Execute())
            {
                double dsurchrate = 0;
                double dpenrate = 0;

                if (result.Read())
                {
                    dsurchrate = result.GetDouble("surch_rate");
                    dpenrate = result.GetDouble("pen_rate");

                    ScheduleFrm.Surcharge = string.Format("{0:##0.00000}", dsurchrate);
                    ScheduleFrm.Interest = string.Format("{0:##0.00000}", dpenrate);
                }
            }
            result.Close();

            result.Query = string.Format("select * from discount_tbl where rev_year = '{0}'", ScheduleFrm.RevYear);
            if (result.Execute())
            {
                double ddiscount = 0;

                if (result.Read())
                {
                    ddiscount = result.GetDouble("discount_rate");

                    ScheduleFrm.Discount = string.Format("{0:##0.00}", ddiscount);
                }
            }
            result.Close();
	
        }

        private void LoadExcess()
        {
            OracleResultSet result = new OracleResultSet();

            ScheduleFrm.Excess = "0.00";
            ScheduleFrm.AddExcess = "0.00";

            result.Query = string.Format("select * from excess_sched where fees_code = 'B' and bns_code = '{0}' and rev_year = '{1}'", ScheduleFrm.BnsCode, ScheduleFrm.RevYear);
            if(result.Execute())
            {
                if (result.Read())
                {
                    ScheduleFrm.Excess = Convert.ToString(result.GetDouble("excess_no"));
                    ScheduleFrm.AddExcess = Convert.ToString(result.GetDouble("excess_amt"));
                }
            }
            result.Close();
		
        }

        public override void Add()
        {
            OracleResultSet result = new OracleResultSet();
            bool bSw = false;
            m_strModCode = "AUTL-A";    // RMC 20180130 added trail in License schedule

            if (ScheduleFrm.ButtonAdd.Text == "&Add")
            {
                ScheduleFrm.ButtonAdd.Text = "&Save";
                ScheduleFrm.ButtonClose.Text = "&Cancel";
                ScheduleFrm.ButtonEdit.Enabled = false;
                ScheduleFrm.ButtonDelete.Enabled = false;
                this.EnableControls(true);

                ScheduleFrm.CheckFees.Enabled = false;
                ScheduleFrm.Excess = "";    // RMC 20140107
                ScheduleFrm.AddExcess = ""; // RMC 20140107

                this.ClearLists();
 
                
                ScheduleFrm.LicNewRate = "0.00";
                ScheduleFrm.LicMinTax = "0.00";
                                
                result.Query = "delete from tmp_fix_sched";
                if(result.ExecuteNonQuery() == 0)
                {}
        
                int intCnt = 0;

                //result.Query = string.Format("select count(*) from bns_table where fees_code = 'B' and length(trim(bns_code)) = 2 and rev_year = '{0}' order by bns_code desc",ScheduleFrm.RevYear);
                int.TryParse(result.ExecuteScalar().ToString(), out intCnt);
                // RMC 20140107 corrected generation of main bns code in Schedules-Add (s)
                result.Query = string.Format("select * from bns_table where fees_code = 'B' and length(trim(bns_code)) = 2 and rev_year = '{0}' order by bns_code desc", ScheduleFrm.RevYear);
                if (result.Execute())
                {
                    if(result.Read())
                        int.TryParse(result.GetString("bns_code"), out intCnt);
                }
                result.Close();
                // RMC 20140107 corrected generation of main bns code in Schedules-Add (e)

                intCnt = intCnt + 1;
                ScheduleFrm.BnsCode = string.Format("{0:0#}", intCnt);
                ScheduleFrm.ComboBnsDesc.Text = "";
                ScheduleFrm.ListOne.Rows.Add("");
                ScheduleFrm.CheckGross.Checked = true;

                ScheduleFrm.ComboBnsDesc.Focus();
            }
            else
            {
                //AFM 20201203 MAO-20-14051	moved if else outside clear controls. for add schedules only
                //AFM 20200917 MAO-20-13622 (s)
                if (AppSettingsManager.SystemUser.UserCode == "SYS_PROG")
                    this.ScheduleFrm.ListOne.ReadOnly = false;
                else
                    this.ScheduleFrm.ListOne.ReadOnly = true; //always true for other users
                //AFM 20200917 MAO-20-13622 (e)

                if (ScheduleFrm.ComboBnsDesc.Text.ToString().Trim() == "")
                {
                    MessageBox.Show("Business description required.", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (ScheduleFrm.LicNewRate == "0.00" || ScheduleFrm.LicNewRate == "" )
                {
                    if (MessageBox.Show("Rate for new business required.\n Continue?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                }

                if(ScheduleFrm.CheckGross.Checked == true)
                {
                    int intRowsCnt = ScheduleFrm.ListTwo.Rows.Count - 1;

                    for (int i = 0; i <= intRowsCnt; i++)
                    {
                        if (ScheduleFrm.ListTwo[4, i].Value == null || ScheduleFrm.ListTwo[4, i].Value == "")
                        {
                            ScheduleFrm.ListTwo.Rows.RemoveAt(i);
                            intRowsCnt--;
                        }
                    }

                    // RMC 20111012 corrected log-out in license-Add, added try & catch
                    try
                    {

                        if (ScheduleFrm.ListTwo[0, 0].Value.ToString() == "" || ScheduleFrm.ListTwo[1, 0].Value.ToString() == ""
                            || ScheduleFrm.ListTwo[4, 0].Value.ToString() == "")
                        {
                            if (MessageBox.Show("Schedule for " + ScheduleFrm.ComboBnsDesc.Text.ToString() + " will not be saved.\n Continue anyway?\n\nYES cancel changes\nNO edit schedule.", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                bSw = false;
                        }
                        else
                        {
                            if (MessageBox.Show("Save?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                // RMC 20111012 modified query commit in scheds (s)
                                pCmd = new OracleResultSet();
                                pCmd.Transaction = true;
                                // RMC 20111012 modified query commit in scheds (e)

                                this.BtaxGrossBase();
                                bSw = true;
                                this.SaveNew();
                                this.SaveSurchIntDisc();
                                this.SaveMinMaxTax("B", ScheduleFrm.SelectedBnsSubCode);

                                // RMC 20111012 modified query commit in scheds (s)
                                if (!pCmd.Commit())
                                {
                                    pCmd.Rollback();
                                    pCmd.Close();
                                    MessageBox.Show("Failed to update records.", string.Empty, MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                                    return;
                                }

                                pCmd.Close();
                                // RMC 20111012 modified query commit in scheds (e)
                            }
                        }
                    }
                    catch   // RMC 20111012 corrected log-out in license-Add, added try & catch
                    {
                        MessageBox.Show("Failed to update records.", string.Empty, MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                }
		        else
		        {
                    if (MessageBox.Show("Save?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			        {
                        // RMC 20111012 modified query commit in scheds (s)
                        pCmd = new OracleResultSet();
                        pCmd.Transaction = true;
                        // RMC 20111012 modified query commit in scheds (e)

				        this.BtaxFixAmtBase();
                        bSw = true;
                        this.SaveNew();
                        this.SaveSurchIntDisc();
                        this.SaveMinMaxTax("B", ScheduleFrm.SelectedBnsSubCode);

                        // RMC 20111012 modified query commit in scheds (s)
                        if (!pCmd.Commit())
                        {
                            pCmd.Rollback();
                            pCmd.Close();
                            MessageBox.Show("Failed to update records.", string.Empty, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }

                        pCmd.Close();
                        // RMC 20111012 modified query commit in scheds (e)
			        }
		        }

                if (bSw)
                {
                    this.InitialFormLoad();
                    this.LoadSchedule();
                    this.InitializeButtons();
                    this.EnableControls(false);
                    ScheduleFrm.ButtonEdit.Enabled = true;
                    ScheduleFrm.ButtonDelete.Enabled = true;
                    ScheduleFrm.CheckFees.Enabled = true;
                }
            }
        }

        public override void Close()
        {
            if (ScheduleFrm.ButtonClose.Text == "&Cancel")
            {
                this.InitializeButtons();
                this.EnableControls(false);
                ScheduleFrm.CheckGross.Checked = true;
                this.InitialFormLoad();
                this.LoadBussType();
                this.LoadSchedule();
                m_strModCode = "";    // RMC 20180130 added trail in License schedule
            }
            else
            {
                ScheduleFrm.Close();
            }

        }

        public override void ClearLists()
        {
            ScheduleFrm.ListOne.Columns.Clear();
            ScheduleFrm.ListOne.Columns.Add("BNSSUBCODE", "Code");
            ScheduleFrm.ListOne.Columns.Add("BNSSUBDESC", "Sub Categories");
                        
            if (ScheduleFrm.CheckGross.Checked == false)
            {
                DataGridViewComboBoxColumn comboBox = new DataGridViewComboBoxColumn();
                comboBox.HeaderCell.Value = "Type";
                ScheduleFrm.ListOne.Columns.Insert(2, comboBox);
                comboBox.Items.AddRange("F", "Q", "A", "QR", "AR");
                ScheduleFrm.Means = "F-Fixed; Q-Qty; A-Area; QR-Qty Range; AR-Area Range";

                ScheduleFrm.ListOne.RowHeadersVisible = false;
                ScheduleFrm.ListOne.Columns[0].Width = 50;
                ScheduleFrm.ListOne.Columns[1].Width = 245;
                ScheduleFrm.ListOne.Columns[2].Width = 50;
            }
            else
            {
                ScheduleFrm.ListOne.RowHeadersVisible = false;
                ScheduleFrm.ListOne.Columns[0].Width = 50;
                ScheduleFrm.ListOne.Columns[1].Width = 295;
                ScheduleFrm.Means = "";
            }

            
            ScheduleFrm.ListTwo.Columns.Clear();
            ScheduleFrm.ListTwo.Columns.Add("GR1", "GROSS 1");
            ScheduleFrm.ListTwo.Columns.Add("GR2", "GROSS 2");
            ScheduleFrm.ListTwo.Columns.Add("XRATE", "EX-RATE");
            ScheduleFrm.ListTwo.Columns.Add("PLUS", "%PLUS");
            ScheduleFrm.ListTwo.Columns.Add("AMT", "AMOUNT");

            ScheduleFrm.ListTwo.RowHeadersVisible = false;
            ScheduleFrm.ListTwo.Columns[0].Width = 70;
            ScheduleFrm.ListTwo.Columns[1].Width = 80;
            ScheduleFrm.ListTwo.Columns[2].Width = 60;
            ScheduleFrm.ListTwo.Columns[3].Width = 60;
            ScheduleFrm.ListTwo.Columns[4].Width = 60;

            ScheduleFrm.ListTwo.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            ScheduleFrm.ListTwo.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            ScheduleFrm.ListTwo.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            ScheduleFrm.ListTwo.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            ScheduleFrm.ListTwo.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            if((this.ScheduleFrm.ButtonAdd.Text == "&Save" || this.ScheduleFrm.ButtonEdit.Text == "&Update")
                && ScheduleFrm.CheckGross.Checked == false)
                ScheduleFrm.ButtonConfig.Enabled = true;
            else
                ScheduleFrm.ButtonConfig.Enabled = false;

            if(this.ScheduleFrm.ButtonAdd.Text == "&Save" || this.ScheduleFrm.ButtonEdit.Text == "&Update")
                ScheduleFrm.ListOne.Rows.Add("");
        }

        private void BtaxGrossBase()
        {
            OracleResultSet result = new OracleResultSet();

            if(ScheduleFrm.BnsCode.ToString() != "" && ScheduleFrm.ComboBnsDesc.Text.ToString() != "")
            {
                this.DeleteTables(true,"");
                

                // insert main bns type	
                this.SaveBns(ScheduleFrm.BnsCode.Trim(), ScheduleFrm.ComboBnsDesc.Text.ToString().Trim(), "G");
                
                //insert sched for main bns type
                this.SaveSchedule(ScheduleFrm.BnsCode.Trim());
                
                
                //insert sub categories
                string strCode = string.Empty;
                string strSubDesc = string.Empty;

                for(int i = 0; i <= ScheduleFrm.ListOne.Rows.Count - 1; i++)
                {
                    strCode = "";
                    strSubDesc = "";

                    if (ScheduleFrm.ListOne[0, i].Value != null && ScheduleFrm.ListOne[1, i].Value != null)
                    {
                        strCode = ScheduleFrm.ListOne[0, i].Value.ToString();
                        strSubDesc = ScheduleFrm.ListOne[1, i].Value.ToString();
                    }

                    if (strCode != "" && strSubDesc != "")
                    {
                        this.SaveBns(strCode, strSubDesc, "GR");
                        this.SaveSchedule(strCode);
                    }
                    else
                    {
                        ScheduleFrm.ListOne.Rows.RemoveAt(i);
                    }
                    
                }
            }
        }

        private void SaveBns(string strBnsCode, string strBnsDesc, string strMeans)
        {
            //OracleResultSet result = new OracleResultSet();   // RMC 20111012 modified query commit in scheds

            // RMC 20111012 modified query commit in scheds, changed result to pCmd

            pCmd.Query = "insert into bns_table (fees_code, bns_code, bns_desc, means, rev_year) values (:1,:2,:3,:4,:5) ";
            pCmd.AddParameter(":1", 'B');
            pCmd.AddParameter(":2", strBnsCode);
            pCmd.AddParameter(":3", strBnsDesc.ToUpper().Trim());   // RMC 20120109 added trim in saving bns schedule
            pCmd.AddParameter(":4", strMeans);
            pCmd.AddParameter(":5", ScheduleFrm.RevYear.Trim());
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

            // RMC 20180130 added trail in License schedule (s)
            string strObject = "Bns Code: " + strBnsCode + " " + strBnsDesc.ToUpper().Trim() + "/Rev Year: " + ScheduleFrm.RevYear;
            if (AuditTrail.InsertTrail(m_strModCode, "bns_table", StringUtilities.HandleApostrophe(strObject)) == 0)
            {
                pCmd.Rollback();
                pCmd.Close();
                MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // RMC 20180130 added trail in License schedule (e)
        }

        private void SaveSchedule(string strBnsCode)
        {
            //OracleResultSet result = new OracleResultSet();   // RMC 20111012 modified query commit in scheds

            // RMC 20111012 modified query commit in scheds, changed result to pCmd
                       
            int intRowsCnt = ScheduleFrm.ListTwo.Rows.Count - 1;

            for (int i = 0; i <= intRowsCnt; i++)
            {
                try
                {
                    string sGr1 = "0";
                    string sGr2 = "0";
                    string sXRate = "0";
                    string sPlus = "0";
                    string sAmt = "0";

                    double dGr1 = 0;
                    double dGr2 = 0;
                    double dXRate = 0;
                    double dPlus = 0;
                    double dAmt = 0;

                    if(ScheduleFrm.ListTwo[0, i].Value != null && ScheduleFrm.ListTwo[0, i].Value != "")
                        sGr1 = ScheduleFrm.ListTwo[0, i].Value.ToString();

                    if (ScheduleFrm.ListTwo[1, i].Value != null && ScheduleFrm.ListTwo[1, i].Value != "")
                        sGr2 = ScheduleFrm.ListTwo[1, i].Value.ToString();

                    if (ScheduleFrm.ListTwo[2, i].Value != null && ScheduleFrm.ListTwo[2, i].Value != "")
                        sXRate = ScheduleFrm.ListTwo[2, i].Value.ToString();

                    if (ScheduleFrm.ListTwo[3, i].Value != null && ScheduleFrm.ListTwo[3, i].Value != "")
                        sPlus = ScheduleFrm.ListTwo[3, i].Value.ToString();

                    if (ScheduleFrm.ListTwo[4, i].Value != null && ScheduleFrm.ListTwo[4, i].Value != "")
                        sAmt = ScheduleFrm.ListTwo[4, i].Value.ToString();

                    dGr1 = Convert.ToDouble(sGr1);
                    sGr1 = string.Format("{0:##0.00}", dGr1);
                    dGr2 = Convert.ToDouble(sGr2);
                    sGr2 = string.Format("{0:##0.00}", dGr2);
                    dXRate = Convert.ToDouble(sXRate);
                    sXRate = string.Format("{0:##0.000000}", dXRate);
                    dPlus = Convert.ToDouble(sPlus);
                    sPlus = string.Format("{0:##0.000000}", dPlus);
                    dAmt = Convert.ToDouble(sAmt);
                    sAmt = string.Format("{0:##0.00}", dAmt);

                    if (sGr1 == "0.00" && sGr2 == "0.00" && sAmt == "0.00")
                    {
                        //do not save with no value
                    }
                    else
                    {
                        if (sGr2 != "0.00")
                        {
                            pCmd.Query = "insert into btax_sched (bns_code, gr_1, gr_2, ex_rate, plus_rate, amount, rev_year) values (:1, :2, :3, :4, :5, :6, :7)";
                            pCmd.AddParameter(":1", strBnsCode);
                            pCmd.AddParameter(":2", sGr1);
                            pCmd.AddParameter(":3", sGr2);
                            pCmd.AddParameter(":4", sXRate);
                            pCmd.AddParameter(":5", sPlus);
                            pCmd.AddParameter(":6", sAmt);
                            pCmd.AddParameter(":7", ScheduleFrm.RevYear.Trim());
                            if (pCmd.ExecuteNonQuery() == 0)
                            {
                            }
                        }
                    }
                }
                catch { }
            }

            if (ScheduleFrm.Excess != "" && ScheduleFrm.AddExcess != "")
            {
                if (Convert.ToDouble(ScheduleFrm.Excess) > 0 && Convert.ToDouble(ScheduleFrm.AddExcess) > 0)
                {
                    pCmd.Query = "insert into excess_sched (fees_code, bns_code, excess_no, excess_amt, rev_year) values (:1,:2,:3,:4,:5) ";
                    pCmd.AddParameter(":1", 'B');
                    pCmd.AddParameter(":2", strBnsCode);
                    pCmd.AddParameter(":3", string.Format("{0:##0.00}", ScheduleFrm.Excess));
                    pCmd.AddParameter(":4", string.Format("{0:##0.00}", ScheduleFrm.AddExcess));
                    pCmd.AddParameter(":5", ScheduleFrm.RevYear);
                    if (pCmd.ExecuteNonQuery() == 0)
                    {
                    }

                }
            }

        }

        private void LoadFixSched(int intRow)
        {
            OracleResultSet result = new OracleResultSet();
            StringBuilder strQuery = new StringBuilder();

            string strMeans = string.Empty;
            string strSubCode = string.Empty;
            string strEntry1 = string.Empty;
            string strEntry2 = string.Empty;
            string strAmount = string.Empty;
            int intTmpRow = 0;

            if (ScheduleFrm.ListOne[0, intRow].Value != null && ScheduleFrm.ListOne[2, intRow].Value != null)
            {
                strSubCode = ScheduleFrm.ListOne[0, intRow].Value.ToString();
                strMeans = ScheduleFrm.ListOne[2, intRow].Value.ToString();
            }

            strQuery.Append(string.Format("select * from tmp_fix_sched where bns_code = '{0}'", strSubCode));

            this.LoadFixSchedHeader(strMeans);
            ScheduleFrm.ListTwo.Rows.Clear();

            if (strMeans == "F")
            {
                strQuery.Append(" order by fix_amount");
                result.Query = strQuery.ToString();

                if (result.Execute())
                {
                    ScheduleFrm.ListTwo.Rows.Add("");
                    while (result.Read())
                    {
                        strEntry1 = result.GetString("fix_name");
                        strAmount = Convert.ToString(result.GetDouble("fix_amount"));
                        intTmpRow = ScheduleFrm.ListTwo.Rows.Count - 1;

                        ScheduleFrm.ListTwo[3, intTmpRow].Value = strEntry1;
                        ScheduleFrm.ListTwo[4, intTmpRow].Value = string.Format("{0:#,##0.00}", Convert.ToDouble(strAmount));
                        //ScheduleFrm.ListTwo.Rows.Add(""); // RMC 20120111 corrected saving of scheds
                    }
                }
                result.Close();
            }
            else if (strMeans == "Q" || strMeans == "A")
            {
                result.Query = strQuery.ToString();
                if (result.Execute())
                {
                    ScheduleFrm.ListTwo.Rows.Add("");
                    while (result.Read())
                    {
                        strAmount = Convert.ToString(result.GetDouble("fix_amount"));
                        intTmpRow = ScheduleFrm.ListTwo.Rows.Count - 1;

                        ScheduleFrm.ListTwo[4, intTmpRow].Value = string.Format("{0:#,##0.00}", Convert.ToDouble(strAmount));
                    }
                }
                result.Close();
            }
            else if (strMeans == "QR")
            {
                strQuery.Append(" order by qty1");
                result.Query = strQuery.ToString();

                if (result.Execute())
                {
                    ScheduleFrm.ListTwo.Rows.Add("");
                    while (result.Read())
                    {
                        strEntry1 = Convert.ToString(result.GetInt("qty1"));
                        strEntry2 = Convert.ToString(result.GetInt("qty2"));
                        strAmount = Convert.ToString(result.GetDouble("fix_amount"));

                        intTmpRow = ScheduleFrm.ListTwo.Rows.Count - 1;

                        ScheduleFrm.ListTwo[2, intTmpRow].Value = strEntry1;
                        ScheduleFrm.ListTwo[3, intTmpRow].Value = strEntry2;
                        ScheduleFrm.ListTwo[4, intTmpRow].Value = string.Format("{0:#,##0.00}", Convert.ToDouble(strAmount));
                        ScheduleFrm.ListTwo.Rows.Add("");
                    }
                }
                result.Close();
            }
            else if (strMeans == "AR")
            {
                strQuery.Append(" order by area1");
                result.Query = strQuery.ToString();

                if (result.Execute())
                {
                    ScheduleFrm.ListTwo.Rows.Add("");
                    while (result.Read())
                    {
                        strEntry1 = Convert.ToString(result.GetDouble("area1"));
                        strEntry2 = Convert.ToString(result.GetDouble("area2"));
                        strAmount = Convert.ToString(result.GetDouble("fix_amount"));

                        intTmpRow = ScheduleFrm.ListTwo.Rows.Count - 1;

                        ScheduleFrm.ListTwo[2, intTmpRow].Value = strEntry1;
                        ScheduleFrm.ListTwo[3, intTmpRow].Value = strEntry2;
                        ScheduleFrm.ListTwo[4, intTmpRow].Value = string.Format("{0:#,##0.00}", Convert.ToDouble(strAmount));
                        ScheduleFrm.ListTwo.Rows.Add("");
                    }
                     
                }
                result.Close();
            }

            // RMC 20140107 (S)
            if (strMeans == "")
            {
                ScheduleFrm.Excess = "";
                ScheduleFrm.AddExcess = "";
            }
            // RMC 20140107 (E)
        }

        private void LoadFixSchedHeader(string strMeans)
        {
            if (strMeans == "F")
            {
                ScheduleFrm.ListTwo.Columns.Clear();
                ScheduleFrm.ListTwo.Columns.Add("GR1", "");
                ScheduleFrm.ListTwo.Columns.Add("GR2", "");
                ScheduleFrm.ListTwo.Columns.Add("XRATE", "");
                ScheduleFrm.ListTwo.Columns.Add("PLUS", "Fixed Schedules");
                ScheduleFrm.ListTwo.Columns.Add("AMT", "AMOUNT");
                ScheduleFrm.ListTwo.RowHeadersVisible = false;
                ScheduleFrm.ListTwo.Columns[0].Visible = false;
                ScheduleFrm.ListTwo.Columns[1].Visible = false;
                ScheduleFrm.ListTwo.Columns[2].Visible = false;
                ScheduleFrm.ListTwo.Columns[3].Width = 200;
                ScheduleFrm.ListTwo.Columns[4].Width = 70;
                ScheduleFrm.ListTwo.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                ScheduleFrm.ListTwo.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                ScheduleFrm.ListTwo.Rows.Add("");
            }
            else if (strMeans == "Q" || strMeans == "A")
            {
                ScheduleFrm.ListTwo.Columns.Clear();
                ScheduleFrm.ListTwo.Columns.Add("GR1", "");
                ScheduleFrm.ListTwo.Columns.Add("GR2", "");
                ScheduleFrm.ListTwo.Columns.Add("XRATE", "");
                ScheduleFrm.ListTwo.Columns.Add("PLUS", "");
                ScheduleFrm.ListTwo.Columns.Add("AMT", "AMOUNT");
                ScheduleFrm.ListTwo.RowHeadersVisible = false;
                ScheduleFrm.ListTwo.Columns[0].Visible = false;
                ScheduleFrm.ListTwo.Columns[1].Visible = false;
                ScheduleFrm.ListTwo.Columns[2].Visible = false;
                ScheduleFrm.ListTwo.Columns[3].Visible = false;
                ScheduleFrm.ListTwo.Columns[4].Width = 100;
                ScheduleFrm.ListTwo.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                ScheduleFrm.ListTwo.Rows.Add("");
            }
            else if (strMeans == "QR")
            {
                ScheduleFrm.ListTwo.Columns.Clear();
                ScheduleFrm.ListTwo.Columns.Add("GR1", "");
                ScheduleFrm.ListTwo.Columns.Add("GR2", "");
                ScheduleFrm.ListTwo.Columns.Add("XRATE", "Quantity 1");
                ScheduleFrm.ListTwo.Columns.Add("PLUS", "Quantity 2");
                ScheduleFrm.ListTwo.Columns.Add("AMT", "AMOUNT");
                ScheduleFrm.ListTwo.RowHeadersVisible = false;
                ScheduleFrm.ListTwo.Columns[0].Visible = false;
                ScheduleFrm.ListTwo.Columns[1].Visible = false;
                ScheduleFrm.ListTwo.Columns[2].Width = 100;
                ScheduleFrm.ListTwo.Columns[3].Width = 100;
                ScheduleFrm.ListTwo.Columns[4].Width = 100;
                ScheduleFrm.ListTwo.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                ScheduleFrm.ListTwo.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                ScheduleFrm.ListTwo.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                ScheduleFrm.ListTwo.Rows.Add("");
            }
            else if (strMeans == "AR")
            {
                ScheduleFrm.ListTwo.Columns.Clear();
                ScheduleFrm.ListTwo.Columns.Add("GR1", "");
                ScheduleFrm.ListTwo.Columns.Add("GR2", "");
                ScheduleFrm.ListTwo.Columns.Add("XRATE", "Area 1");
                ScheduleFrm.ListTwo.Columns.Add("PLUS", "Area 2");
                ScheduleFrm.ListTwo.Columns.Add("AMT", "AMOUNT");
                ScheduleFrm.ListTwo.RowHeadersVisible = false;
                ScheduleFrm.ListTwo.Columns[0].Visible = false;
                ScheduleFrm.ListTwo.Columns[1].Visible = false;
                ScheduleFrm.ListTwo.Columns[2].Width = 100;
                ScheduleFrm.ListTwo.Columns[3].Width = 100;
                ScheduleFrm.ListTwo.Columns[4].Width = 100;
                ScheduleFrm.ListTwo.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                ScheduleFrm.ListTwo.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                ScheduleFrm.ListTwo.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                ScheduleFrm.ListTwo.Rows.Add("");
            }
            else
                ScheduleFrm.ListTwo.Columns.Clear();
            
            
            
        }

        public override void ClickListOne(int intRow, int intCol)
        {
            try
            {
                ScheduleFrm.SelectedBnsSubCode = ScheduleFrm.ListOne[0, intRow].Value.ToString();
                ScheduleFrm.SelectedBnsSubCat = ScheduleFrm.ListOne[1, intRow].Value.ToString();
                if (ScheduleFrm.CheckGross.Checked == false)
                    ScheduleFrm.SelectedMeans = ScheduleFrm.ListOne[2, intRow].Value.ToString();
            }
            catch
            {
                ScheduleFrm.SelectedBnsSubCode = "";
                ScheduleFrm.SelectedBnsSubCat = "";
                if (ScheduleFrm.CheckGross.Checked == false)
                    ScheduleFrm.SelectedMeans = "";
            }

            if (ScheduleFrm.CheckGross.Checked == false)
            {
                if( ScheduleFrm.SelectedBnsSubCode != "" && ScheduleFrm.SelectedBnsSubCat != ""
                    && ScheduleFrm.SelectedMeans != "")
                    this.LoadFixSched(intRow);
                else
                    ScheduleFrm.ListTwo.Columns.Clear();
            }

            this.LoadMinMaxTax("B", ScheduleFrm.SelectedBnsSubCode);

            if (intCol == 0)
            {   //AFM 20200917 MAO-20-13622 (s)
            //    if (AppSettingsManager.SystemUser.UserCode == "SYS_PROG")
            //        ScheduleFrm.ListOne.ReadOnly = false;
            //    //AFM 20200917 MAO-20-13622 (e)
            //    else
            //        ScheduleFrm.ListOne.ReadOnly = true; //false //AFM 20200917 MAO-20-13622

                if (string.IsNullOrEmpty(ScheduleFrm.ListOne[0, intRow].Value.ToString().Trim())) //AFM 20201204 MAO-20-14051
                {
                    ScheduleFrm.ListOne.ReadOnly = false;
                }
                else
                    ScheduleFrm.ListOne.ReadOnly = true;        
            }
            else
            {
                if (ScheduleFrm.ButtonAdd.Text == "&Save" || ScheduleFrm.ButtonEdit.Text == "&Update")
                {
                    //AFM 20200917 MAO-20-13622 (s)
                    //if (AppSettingsManager.SystemUser.UserCode == "SYS_PROG")
                    //    ScheduleFrm.ListOne.ReadOnly = false;
                    ////AFM 20200917 MAO-20-13622 (e)
                    //else
                    //    ScheduleFrm.ListOne.ReadOnly = true; //false //AFM 20200917 MAO-20-13622

                    if (string.IsNullOrEmpty(ScheduleFrm.ListOne[0, intRow].Value.ToString().Trim())) //AFM 20201204 MAO-20-14051
                    {
                        ScheduleFrm.ListOne.ReadOnly = false;                      
                    }
                    else
                        ScheduleFrm.ListOne.ReadOnly = true;                      
                }
            }
        }

        private void BtaxFixAmtBase()
        {
            //OracleResultSet result = new OracleResultSet();   // RMC 20111012 modified query commit in scheds

            // RMC 20111012 modified query commit in scheds, changed result to pCmd

            if(ScheduleFrm.BnsCode != "" && ScheduleFrm.ComboBnsDesc.Text != "")
            {
                this.DeleteTables(true,"");
                

                this.SaveBns(ScheduleFrm.BnsCode, ScheduleFrm.ComboBnsDesc.Text.ToString().Trim(), "NG");
                
                //insert sub categories
                string strCode = string.Empty;
                string strSubDesc = string.Empty;
                string strMeans = string.Empty;

                for (int i = 0; i < ScheduleFrm.ListOne.Rows.Count; ++i)
                {
                    strCode = "";
                    strSubDesc = "";
                    strMeans = "";

                    if (ScheduleFrm.ListOne[0, i].Value != null && ScheduleFrm.ListOne[1, i].Value != null)
                    {
                        strCode = ScheduleFrm.ListOne[0, i].Value.ToString().ToUpper();
                        strSubDesc = ScheduleFrm.ListOne[1, i].Value.ToString(); 
                        strMeans = ScheduleFrm.ListOne[2, i].Value.ToString();
                    }

                    if (strCode != "" && strSubDesc != "")
                    {
                        this.SaveBns(strCode, strSubDesc, strMeans);

                        pCmd.Query = string.Format("insert into fix_sched select * from tmp_fix_sched where bns_code = '{0}' ", strCode);
                        if (pCmd.ExecuteNonQuery() == 0)
                        {
                        }
                    }
                    else
                    {
                        ScheduleFrm.ListOne.Rows.RemoveAt(i);
                    }
                }
                
            }
        }

        public override void ListOneComboEdit(string strSelected)
        {
            int intTempRow, intTempCol;
            string strTemp = string.Empty;

            try
            {
                intTempRow = ScheduleFrm.ListOne.SelectedCells[0].RowIndex;
                intTempCol = ScheduleFrm.ListOne.SelectedCells[0].ColumnIndex;

                if (ScheduleFrm.CheckGross.Checked == false)
                {
                    if (strSelected == "" && ScheduleFrm.ListOne[2, intTempRow].Value != null)
                        strSelected = ScheduleFrm.ListOne[2, intTempRow].Value.ToString();

                    this.m_strMeans = strSelected;

                    if (strSelected != "")
                    {
                        this.LoadFixSchedHeader(strSelected);
                    }
                }
                else
                {
                    if (ScheduleFrm.ButtonAdd.Text == "&Save")
                    {
                        ScheduleFrm.ListTwo.Rows.Add("");
                        ScheduleFrm.ListOne.Rows.Add("");
                    }
                }

                try
                {
                    strTemp = ScheduleFrm.ListOne[1, intTempRow].Value.ToString().ToUpper();
                    ScheduleFrm.ListOne[1, intTempRow].Value = strTemp;

                    //validate duplicate entry
                    if (intTempCol == 1)
                    {
                        string strTempRowValue = string.Empty;

                        for (int i = 0; i <= ScheduleFrm.ListOne.Rows.Count - 1; i++)
                        {
                            if (intTempRow != i)
                            {
                                if (ScheduleFrm.ListOne[1, i].Value != null)
                                {
                                    strTempRowValue = ScheduleFrm.ListOne[1, i].Value.ToString();

                                    if (strTemp == strTempRowValue)
                                    {
                                        MessageBox.Show("Duplicate business category.", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        ScheduleFrm.ListOne[1, intTempRow].Value = "";
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    if (ScheduleFrm.ListOne[0, intTempRow].Value == "" || ScheduleFrm.ListOne[0, intTempRow].Value == null)
                    {
                        frmBnsTypeCode.BnsCode = GenerateSubCode(intTempRow);
                        frmBnsTypeCode.Switch = 1;
                        int iCtr = 0;
                        for (int i = 0; i <= ScheduleFrm.ListOne.Rows.Count - 1; i++)
                        {
                            if (ScheduleFrm.ListOne[0, i].Value != "" && ScheduleFrm.ListOne[0, i].Value != null)
                            {
                                frmBnsTypeCode.ArraySubCat[i + 1] = ScheduleFrm.ListOne[0, i].Value.ToString();
                                iCtr++;
                            }

                        }
                        frmBnsTypeCode.Row = iCtr;
                        frmBnsTypeCode.ShowDialog();

                        ScheduleFrm.ListOne[0, intTempRow].Value = frmBnsTypeCode.BnsCode;
                    }

                    ScheduleFrm.SelectedBnsSubCode = ScheduleFrm.ListOne[0, intTempRow].Value.ToString();
                    ScheduleFrm.SelectedBnsSubCat = ScheduleFrm.ListOne[1, intTempRow].Value.ToString();

                    this.AddListOneRow();
                    this.AddListTwoRow();
                    
                }
                catch
                {
                    ScheduleFrm.SelectedBnsSubCode = "";
                    ScheduleFrm.SelectedBnsSubCat = "";
                }

                
            }
            catch { }
        }

        public override void ListTwoEndEdit(int intCol, int intRow, string strGridPrevValue)
        {
            m_strMeans = ScheduleFrm.SelectedMeans;

            try
            {
                if (ScheduleFrm.CheckGross.Checked)
                {
                    this.EditGross(intCol, intRow, strGridPrevValue);
                                                              
                }
                else
                {
                    if (m_strMeans == "F")
                        this.EditFixedAmt(intCol, intRow, strGridPrevValue);
                    else if (m_strMeans == "Q" || m_strMeans == "A")
                        this.EditQtyArea(intCol, intRow, strGridPrevValue);
                    else if (m_strMeans == "QR")
                        this.EditQtyRange(intCol, intRow, strGridPrevValue);
                    else if (m_strMeans == "AR")
                        this.EditAreaRange(intCol, intRow, strGridPrevValue);
                }

                this.AddListTwoRow();
                
            }
            catch { }
        }

        private void EditGross(int intCol, int intRow, string strGridPrevValue)
        {
            if (strGridPrevValue == "")
                strGridPrevValue = "0";

            string strGridCurrVal = string.Empty;
            strGridPrevValue = string.Format("{0:###0.00}", Convert.ToDouble(strGridPrevValue));

            string strValue = string.Empty;

            try
            {
                strValue = ScheduleFrm.ListTwo[intCol, intRow].Value.ToString();
                strValue = string.Format("{0:###0.00}", Convert.ToDouble(strValue));

                double dtemp = double.Parse(strValue);
            }
            catch{
                MessageBox.Show("Error in Field", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                if (strGridPrevValue == "0.00")
                    ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                else
                    ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                return;
            }

            try
            {
                string strTempVal = string.Empty;

                if (intCol == 4)
                {
                    for (int i = 0; i < ScheduleFrm.ListTwo.Rows.Count; i++)
                    {
                        if (intRow != i)
                        {
                            try
                            {
                                strTempVal = ScheduleFrm.ListTwo[intCol, i].Value.ToString();
                                strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                                if (strValue == strTempVal)
                                {
                                    if (MessageBox.Show("Duplicate amount detected...\nContinue anyway?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                    {
                                        ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                                        return;
                                    }
                                    else
                                        break;
                                }
                            }
                            catch { }
                        }

                     }
                }

                int intDecimal = 0;
                switch (intCol + 1)
                {
                    case 1:
                        intDecimal = 2;
                        break;
                    case 2:
                        intDecimal = 2;
                        break;
                    case 3:
                        intDecimal = 6;
                        break;
                    case 4:
                        intDecimal = 6;
                        break;
                    case 5:
                        intDecimal = 2;
                        break;
                }

                double d;
                d = Convert.ToDouble(ScheduleFrm.ListTwo[intCol, intRow].Value.ToString());

                if (intCol == 0 || intCol == 1)
                    strGridCurrVal = string.Format("{0:##0.00}", d);

                if (intCol == 2 || intCol == 3)
                    strGridCurrVal = string.Format("{0:##0.000000}", d);

                if (intCol == 4)
                    strGridCurrVal = string.Format("{0:#,##0.00}", d);
                
                ScheduleFrm.ListTwo[intCol, intRow].Value = strGridCurrVal;

                //Enable Series
                int intCtr = ScheduleFrm.ListTwo.Rows.Count - 1;
                double dblSeriesGross = 0;
                string strSeries = string.Empty;

                if (intCol == 0)
                {
                    if (ScheduleFrm.ListTwo[1, intRow].Value != null)
                    {
                        strTempVal = ScheduleFrm.ListTwo[1, intRow].Value.ToString().Trim();
                        strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                        if (Convert.ToDouble(strGridCurrVal) > Convert.ToDouble(strTempVal) && strTempVal != "")
                        {
                            if (strGridPrevValue == "0.00")
                                ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                            else
                                ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                        }
                    }
                    if (intRow != 0)
                    {
                        strTempVal = ScheduleFrm.ListTwo[0, intRow - 1].Value.ToString().Trim();
                        strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                        if (Convert.ToDouble(strGridCurrVal) <= Convert.ToDouble(strTempVal) && strTempVal != "")
                        {
                            if (strGridPrevValue == "0.00")
                                ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                            else
                                ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                        }
                        else
                        {
                            strTempVal = ScheduleFrm.ListTwo[intCol, intRow].Value.ToString();
                            strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                            dblSeriesGross = Convert.ToDouble(strTempVal) - .01;
                            strSeries = string.Format("{0:##0.00}", dblSeriesGross);
                            ScheduleFrm.ListTwo[1, intRow - 1].Value = strSeries;
                        }
                    }

                    

                }
                else if (intCol == 1)
                {
                    bool blnIncr = false;

                    strTempVal = ScheduleFrm.ListTwo[0, intRow].Value.ToString().Trim();
                    strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                    if (Convert.ToDouble(strGridCurrVal) < Convert.ToDouble(strTempVal) && strTempVal != "" && Convert.ToDouble(strGridCurrVal) != 0)
                    {
                        if (strGridPrevValue == "0.00")
                            ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                        else
                            ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                    }

                    if (intCtr > intRow)
                    {
                        if (ScheduleFrm.ListTwo[0, intRow + 1].Value != null)
                        {
                            strTempVal = ScheduleFrm.ListTwo[0, intRow + 1].Value.ToString().Trim();
                            if(strTempVal != "")
                                strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));
                        }
                        else
                            strTempVal = "";

                        if (intRow != 0)
                        {
                            if (strTempVal == "")
                                blnIncr = true;
                        }
                        else
                            blnIncr = true;
                    }

                    if (blnIncr)
                    {
                        ScheduleFrm.ListTwo.Rows.Add("");
                        strTempVal = ScheduleFrm.ListTwo[intCol, intRow].Value.ToString();
                        strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                        dblSeriesGross = Convert.ToDouble(strTempVal) + .01;
                        strSeries = string.Format("{0:##0.00}", dblSeriesGross);
                        ScheduleFrm.ListTwo[0, intRow + 1].Value = strSeries;
                    }
                }

               /* if ()
                {
                    ScheduleFrm.ListTwo.Rows.Add("");
                    if (intRow == ScheduleFrm.ListTwo.Rows.Count - 2)
                    {
                        // Enable Series
                        if (ScheduleFrm.ListTwo[1, ScheduleFrm.ListTwo.Rows.Count - 2].Value != null)
                        {
                            strTempVal = ScheduleFrm.ListTwo[1, ScheduleFrm.ListTwo.Rows.Count - 2].Value.ToString();
                            if (strTempVal != "")
                            {
                                strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                                dblSeriesGross = Convert.ToDouble(strTempVal) + .01;
                                strSeries = string.Format("{0:##0.00}", dblSeriesGross);
                                ScheduleFrm.ListTwo[0, ScheduleFrm.ListTwo.Rows.Count - 1].Value = strSeries;
                            }
                        }
                    }
                }*/
            }
            catch
            {
                // no entry in fields
            }

            
        }

        private void EditFixedAmt(int intCol, int intRow, string strGridPrevValue)
        {
            OracleResultSet result = new OracleResultSet();
            if (strGridPrevValue == "")
                strGridPrevValue = "0";

            string strGridCurrVal = string.Empty;
            strGridPrevValue = string.Format("{0:##0.00}", Convert.ToDouble(strGridPrevValue));

            string strValue = string.Empty;
            
            try
            {
                if(ScheduleFrm.ListTwo[intCol, intRow].Value != null)   // RMC 20120111 corrected saving of scheds
                    strValue = ScheduleFrm.ListTwo[intCol, intRow].Value.ToString();
                
                if (intCol == 4)
                {
                    strValue = string.Format("{0:##0.00}", Convert.ToDouble(strValue));
                    double dtemp = double.Parse(strValue);


                }
            }
            catch
            {
                MessageBox.Show("Error in Field", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                if (strGridPrevValue == "0.00")
                    ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                else
                    ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                return;
            }

            string strTempVal = string.Empty;

            try
            {
                if (intCol == 4)
                {
                    for (int i = 0; i < ScheduleFrm.ListTwo.Rows.Count; i++)
                    {
                        if (intRow != i)
                        {
                            if (ScheduleFrm.ListTwo[intCol, i].Value != null)
                            {
                                strTempVal = ScheduleFrm.ListTwo[intCol, i].Value.ToString();
                                strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                                if (strValue == strTempVal)
                                {
                                    if (MessageBox.Show("Duplicate amount detected...\nContinue anyway?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                    {
                                        ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                                        return;
                                    }
                                    else
                                        break;
                                }
                            }
                        }

                        
                        double d;
                        d = Convert.ToDouble(ScheduleFrm.ListTwo[intCol, intRow].Value.ToString());
                        strTempVal = string.Format("{0:#,##0.00}", d);
                        ScheduleFrm.ListTwo[intCol, intRow].Value = strTempVal;
                    }
                }


                bool blnaddFlag = false;

                for (int i = 0; i < ScheduleFrm.ListTwo.Columns.Count; ++i)
                {
                    if (i == 3 || i == 4)
                    {
                        if (ScheduleFrm.ListTwo[i, intRow].Value == null)
                        {
                            blnaddFlag = true;
                            break;
                        }
                    }

                }

                if (!blnaddFlag)
                {

                    // insertion to temp table
                    result.Query = string.Format("delete from tmp_fix_sched where bns_code = '{0}'", ScheduleFrm.SelectedBnsSubCode);
                    if (result.ExecuteNonQuery() == 0)
                    {
                    }

                    for (int i = 0; i < ScheduleFrm.ListTwo.Rows.Count; i++)
                    {
                        string strFDesc = string.Empty;
                        string strFAmt = string.Empty;

                        strFDesc = ScheduleFrm.ListTwo[3, i].Value.ToString().Trim();
                        strFAmt = ScheduleFrm.ListTwo[4, i].Value.ToString();
                        strFAmt = string.Format("{0:##0.00}", Convert.ToDouble(strFAmt));

                        if (strFDesc != "" && strFAmt != "" && 
                            ScheduleFrm.SelectedBnsSubCode != "" && ScheduleFrm.SelectedBnsSubCat != "")
                        {
                            result.Query = "insert into tmp_fix_sched(bns_code,fix_name,fix_amount,rev_year) values (:1,:2,:3,:4)";
                            result.AddParameter(":1", ScheduleFrm.SelectedBnsSubCode);
                            result.AddParameter(":2", strFDesc.ToUpper());
                            result.AddParameter(":3", strFAmt);
                            result.AddParameter(":4", ScheduleFrm.RevYear);
                            if (result.ExecuteNonQuery() == 0)
                            {
                            }
                        }
                        
                    }

                }
            }
            catch
            {
            }
        }

        private void EditQtyArea(int intCol, int intRow, string strGridPrevValue)
        {
            OracleResultSet result = new OracleResultSet();

            if (strGridPrevValue == "")
                strGridPrevValue = "0";

            string strGridCurrVal = string.Empty;
            strGridPrevValue = string.Format("{0:##0.00}", Convert.ToDouble(strGridPrevValue));

            string strValue = string.Empty;

            try
            {
                strValue = ScheduleFrm.ListTwo[intCol, intRow].Value.ToString();
                strValue = string.Format("{0:##0.00}", Convert.ToDouble(strValue));

                if (intCol == 4)
                {
                    double dtemp = double.Parse(strValue);

                }
            }
            catch
            {
                MessageBox.Show("Error in Field", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                if (strGridPrevValue == "0.00")
                    ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                else
                    ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                return;
            }

          //  try
            {

                // insertion to temp table
                result.Query = string.Format("delete from tmp_fix_sched where bns_code = '{0}'", ScheduleFrm.SelectedBnsSubCode);
                if (result.ExecuteNonQuery() == 0)
                {
                }
                
                for (int i = 0; i < ScheduleFrm.ListTwo.Rows.Count; i++)
                {
                    string strFAmt = string.Empty;
                    strFAmt = ScheduleFrm.ListTwo[4, i].Value.ToString();
                    strFAmt = string.Format("{0:##0.00}", Convert.ToDouble(strFAmt));

                    if (strFAmt != "" &&
                            ScheduleFrm.SelectedBnsSubCode != "" && ScheduleFrm.SelectedBnsSubCat != "")
                    {
                        result.Query = "insert into tmp_fix_sched(bns_code,qty1,qty2,fix_amount,rev_year) values (:1,:2,:3,:4,:5)";
                        result.AddParameter(":1", ScheduleFrm.SelectedBnsSubCode);
                        result.AddParameter(":2", '1');
                        result.AddParameter(":3", '1');
                        result.AddParameter(":4", strFAmt);
                        result.AddParameter(":5", ScheduleFrm.RevYear);
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }
                    }
                }

                if (intCol == 4)
                {
                    double d;
                    d = Convert.ToDouble(ScheduleFrm.ListTwo[intCol, intRow].Value.ToString());
                    strValue = string.Format("{0:#,##0.00}", d);

                    ScheduleFrm.ListTwo[intCol, intRow].Value = strValue;
                }
            }
           // catch
           // {
           // }
        }

        private void EditQtyRange(int intCol, int intRow, string strGridPrevValue)
        {
            OracleResultSet result = new OracleResultSet();

            if (strGridPrevValue == "")
                strGridPrevValue = "0";

            double dblSeriesGross = 0;
            string strSeries = string.Empty;

            string strGridCurrVal = string.Empty;
            strGridPrevValue = string.Format("{0:##0.00}", Convert.ToDouble(strGridPrevValue));

            string strValue = string.Empty;

            try
            {
                strValue = ScheduleFrm.ListTwo[intCol, intRow].Value.ToString();
                strValue = string.Format("{0:##0.00}", Convert.ToDouble(strValue));

                double dtemp = double.Parse(strValue);
            }
            catch
            {
                MessageBox.Show("Error in Field", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                if (strGridPrevValue == "0.00")
                    ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                else
                    ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                return;
            }

            string strTempVal = string.Empty;

            try
            {
                if (intCol == 4)
                {
                    for (int i = 0; i < ScheduleFrm.ListTwo.Rows.Count - 1; i++)
                    {
                        if (intRow != i)
                        {
                            strTempVal = ScheduleFrm.ListTwo[intCol, i].Value.ToString();
                            strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                            if (strValue == strTempVal)
                            {
                                if (MessageBox.Show("Duplicate amount detected...\nContinue anyway?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                {
                                    ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                                    return;
                                }
                                else
                                    break;
                            }
                        }
                    }

                }

                int intDecimal = 0;
                switch (intCol + 1)
                {
                    case 3:
                        intDecimal = 0;
                        break;
                    case 4:
                        intDecimal = 0;
                        break;
                    case 5:
                        intDecimal = 2;
                        break;

                }

                double d;
                d = Convert.ToDouble(ScheduleFrm.ListTwo[intCol, intRow].Value.ToString());

                if (intCol == 4)
                    strGridCurrVal = string.Format("{0:#,##0.00}", d);
                
                if (intCol == 2 || intCol == 3)
                    strGridCurrVal = string.Format("{0:##0}", d);

                ScheduleFrm.ListTwo[intCol, intRow].Value = strGridCurrVal;

                if (intCol == 2)
                {
                    if (ScheduleFrm.ListTwo[3, intRow].Value != null)
                    {
                        strTempVal = ScheduleFrm.ListTwo[3, intRow].Value.ToString().Trim();
                        strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                        if (Convert.ToDouble(strGridCurrVal) > Convert.ToDouble(strTempVal) && strTempVal != "")
                        {
                            if (strGridPrevValue == "0.00")
                                ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                            else
                                ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                        }
                    }
                    if (intRow != 0)
                    {
                        strTempVal = ScheduleFrm.ListTwo[2, intRow - 1].Value.ToString().Trim();
                        strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                        if (Convert.ToDouble(strGridCurrVal) <= Convert.ToDouble(strTempVal) && strTempVal != "")
                        {
                            if (strGridPrevValue == "0.00")
                                ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                            else
                                ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                        }
                        else
                        {
                            strTempVal = ScheduleFrm.ListTwo[intCol, intRow].Value.ToString();
                            strTempVal = string.Format("{0:##0}", Convert.ToDouble(strTempVal));

                            dblSeriesGross = Convert.ToDouble(strTempVal) - 1;
                            strSeries = string.Format("{0:##0}", dblSeriesGross);
                            ScheduleFrm.ListTwo[3, intRow - 1].Value = strSeries;
                        }
                    }

                }
                else if (intCol == 3)
                {
                    bool blnIncr = false;
                    int intCtr = ScheduleFrm.ListTwo.Rows.Count - 1;

                    strTempVal = ScheduleFrm.ListTwo[2, intRow].Value.ToString().Trim();
                    strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                    if (Convert.ToDouble(strGridCurrVal) < Convert.ToDouble(strTempVal) && strTempVal != "" && Convert.ToDouble(strGridCurrVal) != 0)
                    {
                        if (strGridPrevValue == "0.00")
                            ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                        else
                            ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                    }

                    if (intCtr > intRow)
                    {
                        if (ScheduleFrm.ListTwo[intCol, intRow + 1].Value != null)
                        {
                            strTempVal = ScheduleFrm.ListTwo[intCol, intRow + 1].Value.ToString().Trim();
                            strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                            if (Convert.ToDouble(strGridCurrVal) >= Convert.ToDouble(strTempVal) && strTempVal != "")
                            {
                                if (strGridPrevValue == "0.00")
                                    ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                                else
                                    ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                            }
                        }
                        else
                            blnIncr = true;
                    }

                    if (blnIncr)
                    {
                        strTempVal = ScheduleFrm.ListTwo[intCol, intRow].Value.ToString();
                        strTempVal = string.Format("{0:##0}", Convert.ToDouble(strTempVal));

                        dblSeriesGross = Convert.ToDouble(strTempVal) + 1;
                        strSeries = string.Format("{0:##0}", dblSeriesGross);
                        ScheduleFrm.ListTwo[2, intRow + 1].Value = strSeries;
                    }
                }

                bool blnaddFlag = false;

                for (int i = 0; i < ScheduleFrm.ListTwo.Columns.Count - 1; ++i)
                {
                    if (i != 0 && i != 1)
                    {
                        if (ScheduleFrm.ListTwo[i, intRow].Value == null)
                        {
                            blnaddFlag = true;
                            break;

                        }
                    }
                }
                
                if (!blnaddFlag)
                {
                    
                    // insertion to temp table
                    result.Query = string.Format("delete from tmp_fix_sched where bns_code = '{0}'", ScheduleFrm.SelectedBnsSubCode);
                    if (result.ExecuteNonQuery() == 0)
                    {
                    }

                    for (int i = 0; i < ScheduleFrm.ListTwo.Rows.Count - 1; i++)
                    {
                        string strEntQ1 = string.Empty;
                        string strEntQ2 = string.Empty;
                        string strQAmt = string.Empty;

                        try
                        {

                            strEntQ1 = ScheduleFrm.ListTwo[2, i].Value.ToString();
                            strEntQ2 = ScheduleFrm.ListTwo[3, i].Value.ToString();
                            strQAmt = ScheduleFrm.ListTwo[4, i].Value.ToString();

                            strEntQ1 = string.Format("{0:##0}", strEntQ1);
                            strEntQ2 = string.Format("{0:##0}", strEntQ2);
                            strQAmt = string.Format("{0:##0.00}", Convert.ToDouble(strQAmt));
                        }
                        catch
                        {
                            strEntQ1 = "";
                            strEntQ2 = "";
                            strQAmt = "";
                        }

                        if (strEntQ1 != "" && strEntQ2 != "" && strQAmt != "" &&
                            ScheduleFrm.SelectedBnsSubCode != "" && ScheduleFrm.SelectedBnsSubCat != "")
                        {
                            result.Query = "insert into tmp_fix_sched(bns_code,qty1,qty2,fix_amount,rev_year) values (:1,:2,:3,:4,:5)";
                            result.AddParameter(":1", ScheduleFrm.SelectedBnsSubCode);
                            result.AddParameter(":2", strEntQ1);
                            result.AddParameter(":3", strEntQ2);
                            result.AddParameter(":4", strQAmt);
                            result.AddParameter(":5", ScheduleFrm.RevYear);
                            if (result.ExecuteNonQuery() == 0)
                            {
                            }

                        }

                    }

                    if (intRow == ScheduleFrm.ListTwo.Rows.Count - 1)
                    {
                        // Enable Series
                        int intRowCount = ScheduleFrm.ListTwo.Rows.Count;
                        if (ScheduleFrm.ListTwo[3, intRowCount - 1].Value != null)
                        {
                            strTempVal = ScheduleFrm.ListTwo[3, intRowCount - 1].Value.ToString();
                            if (strTempVal != "")
                            {
                                strTempVal = string.Format("{0:##0}", Convert.ToDouble(strTempVal));

                                dblSeriesGross = Convert.ToDouble(strTempVal) + 1;
                                 strSeries = string.Format("{0:##0}", dblSeriesGross);
                                ScheduleFrm.ListTwo.Rows.Add("");
                                ScheduleFrm.ListTwo[2, ScheduleFrm.ListTwo.Rows.Count - 1].Value = strSeries;
                            }
                        }
                    }

                }

            }
            catch
            {
            }
        }

        private void EditAreaRange(int intCol, int intRow, string strGridPrevValue)
        {
            OracleResultSet result = new OracleResultSet();

            if (strGridPrevValue == "")
                strGridPrevValue = "0";

            string strGridCurrVal = string.Empty;
            strGridPrevValue = string.Format("{0:##0.00}", Convert.ToDouble(strGridPrevValue));

            string strValue = string.Empty;
            double dblSeriesGross;
            string strSeries = string.Empty;
            bool blnaddFlag = false;

            try
            {
                strValue = ScheduleFrm.ListTwo[intCol, intRow].Value.ToString();
                strValue = string.Format("{0:##0.00}", Convert.ToDouble(strValue));

                double dtemp = double.Parse(strValue);
            }
            catch
            {
                MessageBox.Show("Error in Field", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                if (strGridPrevValue == "0.00")
                    ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                else
                    ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                return;
            }

            string strTempVal = string.Empty;

            try
            {
                if (intCol == 4)
                {
                    for (int i = 0; i < ScheduleFrm.ListTwo.Rows.Count - 1; i++)
                    {
                        if (intRow != i)
                        {
                            strTempVal = ScheduleFrm.ListTwo[intCol, i].Value.ToString();
                            strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                            if (strValue == strTempVal)
                            {
                                if (MessageBox.Show("Duplicate amount detected...\nContinue anyway?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                {
                                    ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                                    return;
                                }
                                else
                                    break;
                            }
                        }
                    }
                }

                double d;
                d = Convert.ToDouble(ScheduleFrm.ListTwo[intCol, intRow].Value.ToString());
                strGridCurrVal = string.Format("{0:##0.00}", d);
                
                ScheduleFrm.ListTwo[intCol, intRow].Value = strGridCurrVal;

                if (intCol == 2)
                {
                    if (ScheduleFrm.ListTwo[3, intRow].Value != null)
                    {
                        strTempVal = ScheduleFrm.ListTwo[3, intRow].Value.ToString().Trim();
                        strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                        if (Convert.ToDouble(strGridCurrVal) > Convert.ToDouble(strTempVal) && strTempVal != "")
                        {
                            if (strGridPrevValue == "0.00")
                                ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                            else
                                ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                        }
                    }
                    if (intRow != 0)
                    {
                        strTempVal = ScheduleFrm.ListTwo[2, intRow - 1].Value.ToString().Trim();
                        strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                        if (Convert.ToDouble(strGridCurrVal) <= Convert.ToDouble(strTempVal) && strTempVal != "")
                        {
                            if (strGridPrevValue == "0.00")
                                ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                            else
                                ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                        }
                        else
                        {
                            strTempVal = ScheduleFrm.ListTwo[intCol, intRow].Value.ToString();
                            strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                            dblSeriesGross = Convert.ToDouble(strTempVal) - .01;
                            strSeries = string.Format("{0:##0.00}", dblSeriesGross);
                            ScheduleFrm.ListTwo[3, intRow - 1].Value = strSeries;
                        }
                    }

                }
                else if (intCol == 3)
                {
                    bool blnIncr = false;
                    int intCtr = ScheduleFrm.ListTwo.Rows.Count - 1;

                    strTempVal = ScheduleFrm.ListTwo[2, intRow].Value.ToString().Trim();
                    strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                    if (Convert.ToDouble(strGridCurrVal) < Convert.ToDouble(strTempVal) && strTempVal != "" && Convert.ToDouble(strGridCurrVal) != 0)
                    {
                        if (strGridPrevValue == "0.00")
                            ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                        else
                            ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                    }

                    if (intCtr > intRow)
                    {
                        if (ScheduleFrm.ListTwo[intCol, intRow + 1].Value != null)
                        {
                            strTempVal = ScheduleFrm.ListTwo[intCol, intRow + 1].Value.ToString().Trim();
                            strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                            if (Convert.ToDouble(strGridCurrVal) >= Convert.ToDouble(strTempVal) && strTempVal != "")
                            {
                                if (strGridPrevValue == "0.00")
                                    ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                                else
                                    ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                            }
                        }
                        else
                            blnIncr = true;
                    }

                    if (blnIncr)
                    {
                        strTempVal = ScheduleFrm.ListTwo[intCol, intRow].Value.ToString();
                        strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                        dblSeriesGross = Convert.ToDouble(strTempVal) + .01;
                        strSeries = string.Format("{0:##0.00}", dblSeriesGross);
                        ScheduleFrm.ListTwo[2, intRow + 1].Value = strSeries;
                    }
                }

                for (int i = 0; i < ScheduleFrm.ListTwo.Columns.Count - 1; ++i)
                {
                    if (i != 0 && i != 1)
                    {
                        if (ScheduleFrm.ListTwo[i, intRow].Value == null)
                        {
                            blnaddFlag = true;
                            break;

                        }
                    }
                }

                if (!blnaddFlag)
                {
                    
                    // insertion to temp table
                    result.Query = string.Format("delete from tmp_fix_sched where bns_code = '{0}'", ScheduleFrm.SelectedBnsSubCode);
                    if (result.ExecuteNonQuery() == 0)
                    {
                    }

                    for (int i = 0; i < ScheduleFrm.ListTwo.Rows.Count; i++)
                    {
                        string strEntQ1 = string.Empty;
                        string strEntQ2 = string.Empty;
                        string strQAmt = string.Empty;

                        try
                        {
                            strEntQ1 = ScheduleFrm.ListTwo[2, i].Value.ToString();
                            strEntQ2 = ScheduleFrm.ListTwo[3, i].Value.ToString();
                            strQAmt = ScheduleFrm.ListTwo[4, i].Value.ToString();
                            strEntQ1 = string.Format("{0:##0.00}", strEntQ1);
                            strEntQ2 = string.Format("{0:##0.00}", strEntQ2);
                            strQAmt = string.Format("{0:##0.00}", Convert.ToDouble(strQAmt));
                        }
                        catch
                        {
                            strEntQ1 = "";
                            strEntQ2 = "";
                            strQAmt = "";
                        }

                        if (strEntQ1 != "" && strEntQ2 != "" && strQAmt != "" &&
                            ScheduleFrm.SelectedBnsSubCode != "" && ScheduleFrm.SelectedBnsSubCat != "")
                        {
                            result.Query = "insert into tmp_fix_sched(bns_code,area1,area2,fix_amount,rev_year) values (:1,:2,:3,:4,:5)";
                            result.AddParameter(":1", ScheduleFrm.SelectedBnsSubCode);
                            result.AddParameter(":2", strEntQ1);
                            result.AddParameter(":3", strEntQ2);
                            result.AddParameter(":4", strQAmt);
                            result.AddParameter(":5", ScheduleFrm.RevYear);
                            if (result.ExecuteNonQuery() == 0)
                            {
                            }

                        }

                    }

                    if (intRow == ScheduleFrm.ListTwo.Rows.Count - 1)
                    {
                        // Enable Series
                        int intRowCount = ScheduleFrm.ListTwo.Rows.Count;
                        if (ScheduleFrm.ListTwo[3, intRowCount - 1].Value != null)
                        {
                            strTempVal = ScheduleFrm.ListTwo[3, intRowCount - 1].Value.ToString();
                            if (strTempVal != "")
                            {
                                strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                                dblSeriesGross = Convert.ToDouble(strTempVal) + .01;
                                strSeries = string.Format("{0:##0.00}", dblSeriesGross);
                                ScheduleFrm.ListTwo.Rows.Add("");
                                ScheduleFrm.ListTwo[2, ScheduleFrm.ListTwo.Rows.Count - 1].Value = strSeries;
                            }
                        }
                    }

                }

            }
            catch
            {
            }
        }

        private void SaveNew()
        {
            //OracleResultSet result = new OracleResultSet();   // RMC 20111012 modified query commit in scheds

            // RMC 20111012 modified query commit in scheds, changed result to pCmd

            string strIsQtrly = "N";

            if(ScheduleFrm.CheckLicQtrDec.Checked == true)
                strIsQtrly = "Y";

            pCmd.Query = string.Format("delete from new_table where rev_year = '{0}' and bns_code = '{1}'", ScheduleFrm.RevYear, ScheduleFrm.BnsCode);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

            pCmd.Query = "insert into new_table (bns_code, new_rate, min_tax, is_qtrly, rev_year) values (:1,:2,:3,:4,:5)";
            pCmd.AddParameter(":1", ScheduleFrm.BnsCode);
            pCmd.AddParameter(":2", string.Format("{0:##0.00000}", ScheduleFrm.LicNewRate));
            pCmd.AddParameter(":3", string.Format("{0:##0.00}", ScheduleFrm.LicMinTax));
            pCmd.AddParameter(":4", strIsQtrly);
            pCmd.AddParameter(":5", ScheduleFrm.RevYear);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

        }

        public override void ButtonConfig()
        {
            using (frmDefaultCodes frmDefaultCodes = new frmDefaultCodes())
            {
                int intTempRow;
                string strTemp = string.Empty;

                intTempRow = ScheduleFrm.ListOne.SelectedCells[0].RowIndex;

                if (ScheduleFrm.ListOne[1, intTempRow].Value != null)
                {
                    frmDefaultCodes.FeesCode = "B";
                    frmDefaultCodes.BnsCode = ScheduleFrm.ListOne[0, intTempRow].Value.ToString();
                    frmDefaultCodes.FeesDesc = ScheduleFrm.ListOne[1, intTempRow].Value.ToString();
                    frmDefaultCodes.CodeType = ScheduleFrm.ListOne[2, intTempRow].Value.ToString();
                    frmDefaultCodes.RevYear = ScheduleFrm.RevYear;
                    frmDefaultCodes.Switch = 0;
                    frmDefaultCodes.ShowDialog();
                }
            }
        }

        public override void Edit()
        {
            OracleResultSet result = new OracleResultSet();
            m_strModCode = "AUTL-E";    // RMC 20180130 added trail in License schedule

            if (ScheduleFrm.ButtonEdit.Text == "&Edit")
            {
                if (!this.CheckSched(StringUtilities.Left(ScheduleFrm.BnsCode, 2),"") || Granted.Grant("AUCS"))
                {
                    ScheduleFrm.ButtonEdit.Text = "&Update";
                    ScheduleFrm.ButtonClose.Text = "&Cancel";
                    ScheduleFrm.ButtonAdd.Enabled = false;
                    ScheduleFrm.ButtonDelete.Enabled = false;
                    ScheduleFrm.CheckFees.Enabled = false;
                    this.EnableControls(true);

                    //AFM 20201203 MAO-20-14051	moved if else outside clear controls. for add schedules only
                    //AFM 20200917 MAO-20-13622 (s)
                    //if (AppSettingsManager.SystemUser.UserCode == "SYS_PROG")
                    //    this.ScheduleFrm.ListOne.ReadOnly = false;
                    //else
                    //    this.ScheduleFrm.ListOne.ReadOnly = true; //always true for other users
                    //AFM 20200917 MAO-20-13622 (e)

                    //this.GenerateSubCode(ScheduleFrm.ListOne.Rows.Count - 1);
                    ScheduleFrm.Excess = "";    // RMC 20140107
                    ScheduleFrm.AddExcess = ""; // RMC 20140107
                }
                else
                {
                    MessageBox.Show("You cannot edit this schedule. Bns code already been used.", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            else //Update
            {
                if (MessageBox.Show("Save?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // RMC 20111012 modified query commit in scheds (s)
                    pCmd = new OracleResultSet();
                    pCmd.Transaction = true;
                    // RMC 20111012 modified query commit in scheds (e)

                    if (ScheduleFrm.CheckGross.Checked == true)
                    {
                        this.BtaxGrossBase();
                    }
                    else
                    {
                        this.BtaxFixAmtBase();
                    }
                    this.SaveNew();
                    this.SaveSurchIntDisc();

                    // RMC 20111012 modified query commit in scheds (s)
                    if (!pCmd.Commit())
                    {
                        pCmd.Rollback();
                        pCmd.Close();
                        MessageBox.Show("Failed to update records.", string.Empty, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    pCmd.Close();
                    // RMC 20111012 modified query commit in scheds (e)

                    this.InitializeButtons();
                    this.LoadBussType();
                    this.LoadSchedule();
                    this.EnableControls(false);
                    ScheduleFrm.ButtonEdit.Enabled = true;
                    ScheduleFrm.ButtonDelete.Enabled = true;
                    ScheduleFrm.CheckFees.Enabled = true;
                    
                }
            }
        }
        private void SaveSurchIntDisc()
        {
            //OracleResultSet result = new OracleResultSet();   // RMC 20111012 modified query commit in scheds

            // RMC 20111012 modified query commit in scheds, changed result to pCmd

            string strFeesCode = "B" + ScheduleFrm.BnsCode;

            pCmd.Query = string.Format("delete from surch_sched where rev_year = '{0}' and tax_fees_code = '{1}'", ScheduleFrm.RevYear, strFeesCode);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

            if (ScheduleFrm.Surcharge == "")
                ScheduleFrm.Surcharge = "0";

            if (ScheduleFrm.Interest == "")
                ScheduleFrm.Interest = "0";

            if (ScheduleFrm.Discount == "")
                ScheduleFrm.Discount = "0";

            pCmd.Query = "insert into surch_sched (surch_rate, pen_rate, rev_year, tax_fees_code) values (:1,:2,:3,:4) ";
            pCmd.AddParameter(":1", string.Format("{0:##0.00}", ScheduleFrm.Surcharge));
            pCmd.AddParameter(":2", string.Format("{0:##0.00}", ScheduleFrm.Interest));
            pCmd.AddParameter(":3", ScheduleFrm.RevYear);
            pCmd.AddParameter(":4", strFeesCode);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

            pCmd.Query = string.Format("delete from discount_tbl where rev_year = '{0}'", ScheduleFrm.RevYear);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

            pCmd.Query = "insert into discount_tbl (discount_rate,term,rev_year) values (:1,:2,:3) ";
            pCmd.AddParameter(":1", string.Format("{0:##0.000000}", ScheduleFrm.Discount));
            pCmd.AddParameter(":2", '1');
            pCmd.AddParameter(":3", ScheduleFrm.RevYear);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }    
        }

        public override void Delete()
        {
            if(!this.CheckSched(StringUtilities.Left(ScheduleFrm.BnsCode,2),""))
            {
                if (MessageBox.Show("WARNING: This will delete the main business along with its sub-categories.\nProceed?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
		        {
                    m_strModCode = "AUTL-D";    // RMC 20180130 added trail in License schedule

		            if (MessageBox.Show("About to Delete?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // RMC 20111012 modified query commit in scheds (s)
                        pCmd = new OracleResultSet();
                        pCmd.Transaction = true;
                        // RMC 20111012 modified query commit in scheds (e)

					    this.DeleteTables(true, "");
                        
                        

                        // RMC 20111012 modified query commit in scheds (s)
                        if (!pCmd.Commit())
                        {
                            pCmd.Rollback();
                            pCmd.Close();
                            MessageBox.Show("Failed to update records.", string.Empty, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }

                        pCmd.Close();
                        // RMC 20111012 modified query commit in scheds (e)

                        // RMC 20180130 added trail in License schedule (s)
                        string strObject = "Bns Code: " + ScheduleFrm.BnsCode + " " + ScheduleFrm.ComboBnsDesc.Text + "/Rev Year: " + ScheduleFrm.RevYear;
                        if (AuditTrail.InsertTrail(m_strModCode, "bns_table", StringUtilities.HandleApostrophe(strObject)) == 0)
                        {
                            pCmd.Rollback();
                            pCmd.Close();
                            MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        // RMC 20180130 added trail in License schedule (e)

                        this.InitialFormLoad();
					}
                }
	        }
	        else
	        {
                MessageBox.Show("You cannot delete this schedule. Bns code already been used.", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
		        return;
	        }
        }

        private void DeleteTables(bool blnMainBuss, string strSubBnsCode)
        {
            //OracleResultSet result = new OracleResultSet();   // RMC 20111012 modified query commit in scheds

            // RMC 20111012 modified query commit in scheds, changed result to pCmd
            if (blnMainBuss)
            {
                pCmd.Query = string.Format("delete from bns_table where fees_code = 'B' and substr(bns_code,1,2) = '{0}' and rev_year = '{1}'", ScheduleFrm.BnsCode, ScheduleFrm.RevYear);
                if (pCmd.ExecuteNonQuery() == 0)
                {
                }

                pCmd.Query = string.Format("delete from btax_sched where substr(bns_code,1,2) = '{0}' and rev_year = '{1}'", ScheduleFrm.BnsCode, ScheduleFrm.RevYear);
                if (pCmd.ExecuteNonQuery() == 0)
                {
                }

                pCmd.Query = string.Format("delete from fix_sched where substr(bns_code,1,2) = '{0}' and rev_year = '{1}'", ScheduleFrm.BnsCode, ScheduleFrm.RevYear);
                if (pCmd.ExecuteNonQuery() == 0)
                {
                }

                //pCmd.Query = string.Format("delete from excess_sched where substr(bns_code,1,2) = '{0}' and rev_year = '{1}'", ScheduleFrm.BnsCode, ScheduleFrm.RevYear);
                pCmd.Query = string.Format("delete from excess_sched where fees_code = 'B' and bns_code = '{0}' and rev_year = '{1}'", ScheduleFrm.BnsCode, ScheduleFrm.RevYear);   // RMC 20170113 corrected error in lost excess sched
                if (pCmd.ExecuteNonQuery() == 0)
                {
                }
                
            }
            else
            {
                pCmd.Query = string.Format("delete from bns_table where fees_code = 'B' and bns_code = '{0}' and rev_year = '{1}'", strSubBnsCode, ScheduleFrm.RevYear);
                if (pCmd.ExecuteNonQuery() == 0)
                {
                }

                pCmd.Query = string.Format("delete from btax_sched where bns_code = '{0}' and rev_year = '{1}'", strSubBnsCode, ScheduleFrm.RevYear);
                if (pCmd.ExecuteNonQuery() == 0)
                {
                }

                pCmd.Query = string.Format("delete from fix_sched where bns_code = '{0}' and rev_year = '{1}'", strSubBnsCode, ScheduleFrm.RevYear);
                if (pCmd.ExecuteNonQuery() == 0)
                {
                }

                //pCmd.Query = string.Format("delete from excess_sched where bns_code = '{0}' and rev_year = '{1}'", strSubBnsCode, ScheduleFrm.RevYear);
                pCmd.Query = string.Format("delete from excess_sched where fees_code = 'B' and bns_code = '{0}' and rev_year = '{1}'", ScheduleFrm.BnsCode, ScheduleFrm.RevYear);   // RMC 20170113 corrected error in lost excess sched
                if (pCmd.ExecuteNonQuery() == 0)
                {
                }

                
            }
        }

        public override void DeleteSubBusiness(int intRow)
        {
            string strSubBnsCode = string.Empty;
            string strSubBnsDesc = string.Empty;

            strSubBnsCode = ScheduleFrm.ListOne[0, intRow].Value.ToString().Trim();
            strSubBnsDesc = ScheduleFrm.ListOne[1, intRow].Value.ToString().Trim();

            if (!this.CheckSched(strSubBnsCode, ""))
            {
                if (MessageBox.Show("Delete "+ strSubBnsDesc + " and its schedules?" , "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.DeleteTables(false, strSubBnsCode);

                    // RMC 20180130 added trail in License schedule (s)
                    string strObject = "Bns Code: " + strSubBnsCode + " " + strSubBnsDesc + "/Rev Year: " + ScheduleFrm.RevYear;
                    if (AuditTrail.InsertTrail("AUTL-D", "bns_table", StringUtilities.HandleApostrophe(strObject)) == 0)
                    {
                        pCmd.Rollback();
                        pCmd.Close();
                        MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    // RMC 20180130 added trail in License schedule (e)

                    this.InitialFormLoad();
                    
                }
            }
            else
            {
                MessageBox.Show("You cannot delete this schedule. Bns code already been used.", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }            
        }

        private void SaveMinMaxTax(string strFeesCode, string strBnsCode)
        {
            //OracleResultSet result = new OracleResultSet();   // RMC 20111012 modified query commit in scheds

            // RMC 20111012 modified query commit in scheds, changed result to pCmd

            double dblMinTax = 0;
            double dblMaxTax = 0;

            if (ScheduleFrm.MinTax.ToString().Trim() == "")
                ScheduleFrm.MinTax = "0";

            if (ScheduleFrm.MaxTax.ToString().Trim() == "")
                ScheduleFrm.MaxTax = "0";

            pCmd.Query = string.Format("delete from minmax_tax_table where trim(fees_code) = '{0}' and bns_code = '{1}'", strFeesCode, strBnsCode);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

            dblMinTax = Convert.ToDouble(ScheduleFrm.MinTax);
            dblMaxTax = Convert.ToDouble(ScheduleFrm.MaxTax);

            ScheduleFrm.MinTax = string.Format("{0:##.00}", dblMinTax);
            ScheduleFrm.MaxTax = string.Format("{0:##.00}", dblMaxTax);

            if (dblMinTax != 0 || dblMaxTax != 0)   // RMC 20150305 corrections in schedules module
            {
                pCmd.Query = "insert into minmax_tax_table (FEES_CODE, BNS_CODE, MIN_TAX, MAX_TAX, REV_YEAR, DATA_TYPE) values (:1,:2,:3,:4,:5,:6)";
                pCmd.AddParameter(":1", strFeesCode);
                pCmd.AddParameter(":2", strBnsCode);
                pCmd.AddParameter(":3", ScheduleFrm.MinTax);
                pCmd.AddParameter(":4", ScheduleFrm.MaxTax);
                pCmd.AddParameter(":5", ScheduleFrm.RevYear);
                pCmd.AddParameter(":6", "F");
                if (pCmd.ExecuteNonQuery() == 0)
                {
                }
            }
        }

        

    }
}
