using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.StringUtilities;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.AdditionalBusiness
{
    /*public class GridAddlInfo:Grid
    {
        private string m_strTmpBnsCode = string.Empty;
        private string m_strNumberOfEmployee = string.Empty;
	    private string m_strBusinessArea = string.Empty;
        private string m_strPrevValue = string.Empty;

        public GridAddlInfo(frmGrid Form)
            : base(Form)
        {
        }

        public override void FormLoad()
        {
            GridFrm.dgvGrid.Columns.Clear();
            GridFrm.dgvGrid.Columns.Add("CODE", "Code");
            GridFrm.dgvGrid.Columns.Add("DESC", "Description");
            GridFrm.dgvGrid.Columns.Add("TYPE", "Type");
            GridFrm.dgvGrid.Columns.Add("VALUE", "Value");
            GridFrm.dgvGrid.Columns[0].Width = 50;
            GridFrm.dgvGrid.Columns[1].Width = 200;
            GridFrm.dgvGrid.Columns[2].Width = 50;
            GridFrm.dgvGrid.Columns[3].Width = 100;
            GridFrm.dgvGrid.Columns[0].ReadOnly = true;
            GridFrm.dgvGrid.Columns[1].ReadOnly = true;
            GridFrm.dgvGrid.Columns[2].ReadOnly = true;
            GridFrm.dgvGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                        
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            string strDefaultCode = string.Empty;
            string strDefaultDesc = string.Empty;
            string strDefaultType = string.Empty;
            
            this.GridFrm.btnClose.Text = "&Close";

	        m_strTmpBnsCode = StringUtilities.Left(GridFrm.BnsCode,2);
                
            result.Query = string.Format("select distinct(default_code) from default_others where default_fee like '{0}%%' and rev_year = '{1}' order by default_code",m_strTmpBnsCode,GridFrm.RevYear);
			if(result.Execute())
            {
                int x = 0;
		        double d;

                while (result.Read())
                {
                    GridFrm.dgvGrid.Rows.Add("");

                    strDefaultCode = result.GetString("default_code").Trim();
                    GridFrm.dgvGrid[0, x].Value = strDefaultCode;

                    result2.Query = string.Format("select * from default_code where default_code = '{0}' and rev_year = '{1}'", strDefaultCode, GridFrm.RevYear);
                    if (result2.Execute())
                    {
                        if (result2.Read())
                        {
                            strDefaultDesc = result2.GetString("default_desc").Trim();
                            strDefaultType = result2.GetString("data_type").Trim();

                            GridFrm.dgvGrid[1, x].Value = strDefaultDesc;
                            GridFrm.dgvGrid[2, x].Value = strDefaultType;

                        }
                    }
                    result2.Close();

                    string strType = "";

                    result2.Query = "select data,data_type from other_info where bin = :1 and tax_year = :2 and bns_code = :3 and default_code = :4 and data_type = :5 and rev_year = :6";
                    result2.AddParameter(":1", GridFrm.BIN);
                    result2.AddParameter(":2", GridFrm.TaxYear);
                    result2.AddParameter(":3", GridFrm.BnsCode);
                    result2.AddParameter(":4", strDefaultCode);
                    result2.AddParameter(":5", strDefaultType);
                    result2.AddParameter(":6", GridFrm.RevYear);
                    if (result2.Execute())
                    {
                        if (result2.Read())
                        {
                            strType = result2.GetString("data_type");
                            GridFrm.dgvGrid[3, x].Value = result2.GetDouble("data");
                        }
                        else
                        {
                            result2.Close();

                            result2.Query = "select data,data_type, tax_year from other_info where bin = :1	and default_code = :2 and data_type = :3 and rev_year = :4 and bns_code = :5 order by tax_year desc";
                            result2.AddParameter(":1", GridFrm.BIN);
                            result2.AddParameter(":2", strDefaultCode);
                            result2.AddParameter(":3", strDefaultType);
                            result2.AddParameter(":4", GridFrm.RevYear);
                            result2.AddParameter(":5", GridFrm.BnsCode);
                            if (result2.Execute())
                            {
                                if (result2.Read())
                                {
                                    strType = result2.GetString("data_type");
                                    GridFrm.dgvGrid[3, x].Value = result2.GetString("data");
                                }
                                else
                                {
                                    GridFrm.dgvGrid[3, x].Value = "0";
                                }
                            }
                            result2.Close();
                        }
                    }

                    if (GridFrm.dgvGrid[3, x].Value.ToString() != "")
                        d = Convert.ToDouble(GridFrm.dgvGrid[3, x].Value.ToString());
                    else
                        d = 0;

                    if (strType == "A" || strType == "AR" || strType == "RR")
                        GridFrm.dgvGrid[3, x].Value = string.Format("{0:##.00}", d);
                    else
                        GridFrm.dgvGrid[3, x].Value = string.Format("{0:##}", d);

                    x++;
                }
            }
            result.Close();

	        m_strNumberOfEmployee = "";
	        m_strBusinessArea = "";
        }


        public override void Save()
        {
            OracleResultSet result = new OracleResultSet();

            string strBnsCode = string.Empty;

            GridFrm.TempBIN = GridFrm.BIN;

            if(GridFrm.dgvGrid.Rows.Count > 1)  
		    {
                if (MessageBox.Show("Do you want to save changes?", GridFrm.SourceClass, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (GridFrm.TempBIN == "")
                    {
                        DateTime dtCurrent = AppSettingsManager.GetSystemDate();
                        dtCurrent.Year.ToString();

                        GridFrm.TempBIN = string.Format("{0:0000#}-{1:00#}-{2:00#}-{3:00#}:{4:00#}:{5:00#}", dtCurrent.Year.ToString(),dtCurrent.Month.ToString(), dtCurrent.Day.ToString(), dtCurrent.Hour.ToString(), dtCurrent.Minute.ToString(), dtCurrent.Second.ToString());

                    }
                    strBnsCode = StringUtilities.Left(GridFrm.BnsCode, 2);

                    result.Query = string.Format("delete from other_info where bin = '{0}' and tax_year = '{1}' and bns_code = '{2}'", GridFrm.TempBIN, GridFrm.TaxYear, GridFrm.BnsCode);
                    if (result.ExecuteNonQuery() == 0)
                    {
                    }

                    string strCode = string.Empty;
                    string strDesc = string.Empty;
                    string strType = string.Empty;
                    string strValue = string.Empty;
                    double dblValue = 0;

                    for (int iCtr = 0; iCtr < GridFrm.dgvGrid.Rows.Count; iCtr++)
                    {
                        if (GridFrm.dgvGrid[0, iCtr].Value != null)
                        {
                            strCode = GridFrm.dgvGrid[0, iCtr].Value.ToString().Trim();
                            strDesc = GridFrm.dgvGrid[1, iCtr].Value.ToString().Trim();
                            strType = GridFrm.dgvGrid[2, iCtr].Value.ToString().Trim();
                            strValue = GridFrm.dgvGrid[3, iCtr].Value.ToString().Trim();

                            if (strValue != "")
                                dblValue = Convert.ToDouble(strValue);
                            else
                                dblValue = 0;

                            if (dblValue > 0)
                            {
                                if (strType == "A" || strType == "AR" || strType == "RR")
                                    strValue = string.Format("{0:##.00}", dblValue);
                                else
                                    strValue = string.Format("{0:##}", dblValue);

                                result.Query = "insert into other_info (bin, tax_year, bns_code, default_code, data_type, data, rev_year) values(:1,:2,:3,:4,:5,:6,:7)";
                                result.AddParameter(":1", GridFrm.TempBIN);
                                result.AddParameter(":2", GridFrm.TaxYear);
                                result.AddParameter(":3", GridFrm.BnsCode);
                                result.AddParameter(":4", strCode);
                                result.AddParameter(":5", strType);
                                result.AddParameter(":6", strValue);
                                result.AddParameter(":7", GridFrm.RevYear);
                                if (result.ExecuteNonQuery() == 0)
                                {
                                }

                                if (strDesc == "NUMBER OF EMPLOYEES")	// Number of Employee
                                    m_strNumberOfEmployee = strValue;
                                if (strDesc == "AREA OF BUSINESS IN SQM")	// Business Area
                                    m_strBusinessArea = strValue;

                            }
                            else
                            {
                                if (strCode == "0010" && ConfigurationAttributes.LGUCode == "117")
                                {
                                    MessageBox.Show(strDesc + " is required.", GridFrm.SourceClass, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    return;
                                }
                            }
                        }
                    }


                    // delete tax_and_fees after saving of other info to reset billing for the year
                    result.Query = string.Format("delete from tax_and_fees where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", GridFrm.TempBIN, GridFrm.TaxYear, GridFrm.BnsCode);
                    if (result.ExecuteNonQuery() == 0)
                    {
                    }

                    this.Close();
                }
            }
        }

        public override void Close()
        {
            if (GridFrm.btnClose.Text == "&Cancel")
                GridFrm.btnClose.Text = "&Close";
            else
                GridFrm.Close();
        }

        public override void EndEditList(int intCol, int intRow)
        {
            string strValue = string.Empty;
            string strType = string.Empty;

            if (GridFrm.dgvGrid[intCol, intRow].Value != null)
            {
                try
                {
                    strValue = GridFrm.dgvGrid[intCol, intRow].Value.ToString().Trim();
                    strValue = string.Format("{0:##0.00}", Convert.ToDouble(strValue));
                }
                catch
                {
                    MessageBox.Show("Error in Field\nShould be numeric", GridFrm.SourceClass, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    GridFrm.dgvGrid[intCol, intRow].Value = "0.00";
                    return;
                }

                double d;
                d = Convert.ToDouble(strValue);

                strType = GridFrm.dgvGrid[2, intRow].Value.ToString().Trim();

                if (strType == "A" || strType == "AR" || strType == "RR")
                    m_strPrevValue = string.Format("{0:##.00}", d);
                else
                    m_strPrevValue = string.Format("{0:##}", d);

                GridFrm.dgvGrid[intCol, intRow].Value = m_strPrevValue;
            }
        }

        public override void BeginEditList(int intCol, int intRow)
        {
            m_strPrevValue = GridFrm.dgvGrid[intCol, intRow].Value.ToString().Trim();
        }
    }*/
}
