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

namespace Amellar.Modules.BusinessMapping
{
    public partial class frmCleansingTool : Form
    {
        public frmCleansingTool()
        {
            InitializeComponent();
            bin1.txtLGUCode.Text = ConfigurationAttributes.LGUCode;
            bin1.txtDistCode.Text = ConfigurationAttributes.DistCode;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvOfficialList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CellClickOfficial(e.RowIndex);
        }

        private void CellClickOfficial(int iRow)
        {
            string sBIN = "";   //192-00-2011-0006306
            try
            {
                sBIN = dgvOfficialList[0, iRow].Value.ToString();
                bin1.txtTaxYear.Text = sBIN.Substring(7, 4);
                bin1.txtBINSeries.Text = sBIN.Substring(12, 7);
                txtPermitNo.Text = dgvOfficialList[1, iRow].Value.ToString();
                txtBnsNm.Text = dgvOfficialList[2, iRow].Value.ToString();
                txtBnsAdd.Text = dgvOfficialList[3, iRow].Value.ToString();
                txtOwnNm.Text = dgvOfficialList[4, iRow].Value.ToString();
                txtOwnAdd.Text = dgvOfficialList[5, iRow].Value.ToString();
                txtOwnCode.Text = dgvOfficialList[6, iRow].Value.ToString();
                txtTaxYear.Text = dgvOfficialList[7, iRow].Value.ToString();
                txtBnsStat.Text = dgvOfficialList[8, iRow].Value.ToString();
            }
            catch { }

            LoadUnOfficialList();
        }

        private void LoadOfficialList()
        {
            OracleResultSet pRec = new OracleResultSet();

            dgvOfficialList.Columns.Clear();

            dgvOfficialList.DefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Regular);
            dgvOfficialList.Columns.Add("BIN", "BIN");
            dgvOfficialList.Columns.Add("PERMIT", "Permit No.");
            dgvOfficialList.Columns.Add("BNSNM", "Business Name");
            dgvOfficialList.Columns.Add("BNSADD", "Business Address");
            dgvOfficialList.Columns.Add("OWNNM", "Owner's Name");
            dgvOfficialList.Columns.Add("OWNADD", "Owner's Address");
            dgvOfficialList.Columns.Add("OWNCODE", "Owner Code");
            dgvOfficialList.Columns.Add("TAXYEAR", "Tax Year");
            dgvOfficialList.Columns.Add("STAT", "Business Status");
            
            dgvOfficialList.RowHeadersVisible = false;
            dgvOfficialList.Columns[0].Width = 150;
            dgvOfficialList.Columns[1].Width = 100;
            dgvOfficialList.Columns[2].Width = 200;
            dgvOfficialList.Columns[3].Width = 200;
            dgvOfficialList.Columns[4].Width = 200;
            dgvOfficialList.Columns[5].Width = 200;
            dgvOfficialList.Columns[6].Width = 100;
            dgvOfficialList.Columns[7].Width = 100;
            dgvOfficialList.Columns[8].Width = 100;

            dgvOfficialList.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            if (rdoPermitNo.Checked)
            {
                pRec.Query = "select * from businesses where permit_no in ";
                pRec.Query += "(select permit_no from btm_temp_businesses where trim(bin) is null) order by bin";
            }
            else if (rdoBnsName.Checked)
            {
                pRec.Query = "select * from businesses where bns_nm in ";
                pRec.Query += "(select bns_nm from btm_temp_businesses where trim(bin) is null) order by bin";
            }
            else if (rdoOwnName.Checked)
            {
                pRec.Query = "select * from businesses where own_code in ";
                pRec.Query += "(select own_code from btm_temp_businesses where trim(bin) is null) order by bin";
            }
            if (pRec.Query.ToString() != "")
            {
                if (pRec.Execute())
                {
                    int iRow = 0;
                    string sBIN = "";
                    string sOwnCode = "";

                    while (pRec.Read())
                    {
                        dgvOfficialList.Rows.Add("");
                        sBIN = pRec.GetString("bin");
                        sOwnCode = pRec.GetString("own_code");
                        dgvOfficialList[0, iRow].Value = sBIN;
                        dgvOfficialList[1, iRow].Value = pRec.GetString("permit_no");
                        dgvOfficialList[2, iRow].Value = pRec.GetString("bns_nm");
                        dgvOfficialList[3, iRow].Value = AppSettingsManager.GetBnsAddress(sBIN);
                        dgvOfficialList[4, iRow].Value = AppSettingsManager.GetBnsOwner(sOwnCode);
                        dgvOfficialList[5, iRow].Value = AppSettingsManager.GetBnsOwnAdd(sOwnCode);
                        dgvOfficialList[6, iRow].Value = sOwnCode;
                        dgvOfficialList[7, iRow].Value = pRec.GetString("tax_year");
                        dgvOfficialList[8, iRow].Value = pRec.GetString("bns_stat");

                        if (iRow == 0)
                            CellClickOfficial(iRow);

                        iRow++;
                    }
                }
                pRec.Close();
            }
        }

        private void LoadUnOfficialList()
        {
            OracleResultSet pRec = new OracleResultSet();

            dgvUnofficialList.Columns.Clear();

            dgvUnofficialList.DefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Regular);
            dgvUnofficialList.Columns.Add("TBIN", "Temporary BIN");
            dgvUnofficialList.Columns.Add("PERMIT", "Permit No.");
            dgvUnofficialList.Columns.Add("BNSNM", "Business Name");
            dgvUnofficialList.Columns.Add("BNSADD", "Business Address");
            dgvUnofficialList.Columns.Add("OWNNM", "Owner's Name");
            dgvUnofficialList.Columns.Add("OWNADD", "Owner's Address");

            dgvUnofficialList.RowHeadersVisible = false;
            dgvUnofficialList.Columns[0].Width = 150;
            dgvUnofficialList.Columns[1].Width = 100;
            dgvUnofficialList.Columns[2].Width = 200;
            dgvUnofficialList.Columns[3].Width = 200;
            dgvUnofficialList.Columns[4].Width = 200;
            dgvUnofficialList.Columns[5].Width = 200;
            
            dgvUnofficialList.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            if (rdoPermitNo.Checked)
            {
                pRec.Query = "select * from btm_temp_businesses where permit_no = '" + txtPermitNo.Text.Trim() + "' and trim(bin) is null order by tbin";
            }
            else if (rdoBnsName.Checked)
            {
                pRec.Query = "select * from btm_temp_businesses where bns_nm = '" + StringUtilities.HandleApostrophe(txtBnsNm.Text.Trim()) + "' and trim(bin) is null order by tbin";
            }
            else if (rdoOwnName.Checked)
            {
                pRec.Query = "select * from btm_temp_businesses where own_code = '" + txtOwnCode.Text.Trim() + "' and trim(bin) is null order by tbin";
            }
            if (pRec.Query.ToString() != "")
            {
                if (pRec.Execute())
                {
                    int iRow = 0;
                    string sBIN = "";
                    string sOwnCode = "";
                    string sBnsHouseNo = "";
                    string sBnsStreet = "";
                    string sBnsBrgy = "";
                    string sBnsMun = "";
                    string sAddress = "";

                    while (pRec.Read())
                    {
                        dgvUnofficialList.Rows.Add("");
                        sBIN = pRec.GetString("tbin");
                        sOwnCode = pRec.GetString("own_code");
                        sBnsHouseNo = pRec.GetString("bns_house_no").Trim();
                        sBnsStreet = pRec.GetString("bns_street").Trim();
                        sBnsBrgy = pRec.GetString("bns_brgy").Trim();
                        sBnsMun = pRec.GetString("bns_mun").Trim();

                        if (sBnsHouseNo == "." || sBnsHouseNo == "")
                            sBnsHouseNo = "";
                        else
                            sBnsHouseNo = sBnsHouseNo + " ";
                        if (sBnsStreet == "." || sBnsStreet == "")
                            sBnsStreet = "";
                        else
                            sBnsStreet = sBnsStreet + ", ";
                        if (sBnsBrgy == "." || sBnsBrgy == "")
                            sBnsBrgy = "";
                        else
                            sBnsBrgy = sBnsBrgy + ", ";
                        if (sBnsMun == "." || sBnsMun == "")
                            sBnsMun = "";

                        sAddress = sBnsHouseNo + sBnsStreet + sBnsBrgy + sBnsMun;

                        dgvUnofficialList[0, iRow].Value = sBIN;
                        dgvUnofficialList[1, iRow].Value = pRec.GetString("permit_no");
                        dgvUnofficialList[2, iRow].Value = pRec.GetString("bns_nm");
                        dgvUnofficialList[3, iRow].Value = sAddress;
                        dgvUnofficialList[4, iRow].Value = AppSettingsManager.GetBnsOwner(sOwnCode);
                        dgvUnofficialList[5, iRow].Value = AppSettingsManager.GetBnsOwnAdd(sOwnCode);

                        if (iRow == 0)
                            CellClickUnOfficial(iRow);
                        iRow++;
                    }
                }
                pRec.Close();
            }
        }

        private void ClearControls(string sControlSwitch)
        {
            if (sControlSwitch == "1")
            {
                bin1.txtTaxYear.Text = "";
                bin1.txtBINSeries.Text = "";
                txtPermitNo.Text = "";
                txtBnsNm.Text = "";
                txtBnsAdd.Text = "";
                txtOwnNm.Text = "";
                txtOwnAdd.Text = "";
            }

            if (sControlSwitch == "2")
            {
                txtTempBIN.Text = "";
                txtUnofficalBnsNm.Text = "";
                txtUnofficalBnsAdd.Text = "";
                txtUnofficalOwnNm.Text = "";
                txtUnofficalOwnAdd.Text = "";
            }
        }

        private void rdoPermitNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoPermitNo.Checked)
            {
                rdoBnsName.Checked = false;
                rdoOwnName.Checked = false;
                ClearControls("1");
                ClearControls("2");
                LoadOfficialList();
                LoadUnOfficialList();
                btnPrint.Text = "Print matching by Permit no.";
            }
        }

        private void rdoBnsName_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoBnsName.Checked)
            {
                rdoPermitNo.Checked = false;
                rdoOwnName.Checked = false;
                ClearControls("1");
                ClearControls("2");
                LoadOfficialList();
                LoadUnOfficialList();
                btnPrint.Text = "Print matching by Business Name";
            }
        }

        private void rdoOwnName_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOwnName.Checked)
            {
                rdoPermitNo.Checked = false;
                rdoBnsName.Checked = false;
                ClearControls("1");
                ClearControls("2");
                LoadOfficialList();
                LoadUnOfficialList();
                btnPrint.Text = "Print matching by Owner's Name";
            }
        }

        private void frmCleansingTool_Load(object sender, EventArgs e)
        {
            rdoPermitNo.Checked = true;
        }

        private void dgvUnofficialList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CellClickUnOfficial(e.RowIndex);
        }

        private void CellClickUnOfficial(int iRow)
        {
            
            try
            {
                txtTempBIN.Text = dgvUnofficialList[0, iRow].Value.ToString();
                txtUnofficalBnsNm.Text = dgvUnofficialList[2, iRow].Value.ToString();
                txtUnofficalBnsAdd.Text = dgvUnofficialList[3, iRow].Value.ToString();
                txtUnofficalOwnNm.Text = dgvUnofficialList[4, iRow].Value.ToString();
                txtUnofficalOwnAdd.Text = dgvUnofficialList[5, iRow].Value.ToString();


            }
            catch { }
        }

        private void btnMatch_Click(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pBTMCmd = new OracleResultSet();
            OracleResultSet pGisRec = new OracleResultSet();

            try
            {
                pGisRec.CreateNewConnectionGIS();

                if (txtBnsNm.Text.Trim() == "" || txtUnofficalBnsNm.Text.Trim() == "")
                {
                    MessageBox.Show("Select Business to match", "Business Mapping Cleansing Tool", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                if (MessageBox.Show("Match Temp BIN: " + txtTempBIN.Text + " to BIN: " + bin1.GetBin() + "?", "Business Mapping Cleansing Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sLandPin = "";
                    string sBldgCode = "";
                    string sX = "";
                    string sY = "";
                    string sBrgyCode = "";
                    string sBrgyName = "";
                    int iCnt = 0;

                    pBTMCmd.Query = "update btm_temp_businesses set bin = '" + bin1.GetBin() + "' where tbin = '" + txtTempBIN.Text + "'";
                    if (pBTMCmd.ExecuteNonQuery() == 0)
                    { }

                    pRec.Query = "select * from btm_gis_loc where bin = '" + txtTempBIN.Text.Trim() + "'";
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sLandPin = pRec.GetString("land_pin").Trim();
                            sBldgCode = pRec.GetString("bldg_code").Trim();
                        }
                    }
                    pRec.Close();

                    //192-00-019-005-053
                    sBrgyCode = sLandPin.Substring(7, 3);

                    pRec.Query = "select * from brgy where brgy_code = '" + sBrgyCode + "'";
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sBrgyName = pRec.GetString("brgy_nm");
                        }
                    }
                    pRec.Close();

                    pGisRec.Query = "select * from gis_business_location where pin = '" + sLandPin + "' and bldg_code = '" + sBldgCode + "'";
                    if (pGisRec.Execute())
                    {
                        if (pGisRec.Read())
                        {
                            sX = pGisRec.GetDouble("x").ToString();
                            sY = pGisRec.GetDouble("y").ToString();
                        }
                    }
                    pGisRec.Close();

                    pGisRec.Query = "delete from gis_businesses where bin = '" + bin1.GetBin() + "'";
                    if (pGisRec.ExecuteNonQuery() == 0)
                    { }

                    pGisRec.Query = "delete from gis_bin where bin = '" + bin1.GetBin() + "'";
                    if (pGisRec.ExecuteNonQuery() == 0)
                    { }

                    int iID = 0;
                    iID = GetGisID(pGisRec, sLandPin, sBldgCode);

                    if (iID == 0)
                    {
                        pGisRec.Query = "insert into gis_businesses values (";
                        pGisRec.Query += "id_seq.nextval, ";
                        pGisRec.Query += "'" + bin1.GetBin() + "', ";
                        pGisRec.Query += "'', '" + sLandPin + "',"; // bldg pin,land pin
                        pGisRec.Query += "'0', '.', '.',";	//legend, code, hotlink
                        pGisRec.Query += "'" + StringUtilities.HandleApostrophe(txtBnsNm.Text.Trim()) + "', ";
                        pGisRec.Query += "'" + sX + "', ";	//longitude
                        pGisRec.Query += "'" + sY + "', ";	//latitude
                        pGisRec.Query += "'', ";
                        pGisRec.Query += "'" + sBrgyName + "', ";
                        pGisRec.Query += "'" + sBldgCode + "')";
                        if (pGisRec.ExecuteNonQuery() == 0)
                        { }

                    }

                    pGisRec.Query = "insert into gis_bin values (";
                    pGisRec.Query += "'" + bin1.GetBin() + "', ";
                    pGisRec.Query += "'', '" + sLandPin + "',"; // bldg pin,land pin
                    pGisRec.Query += "" + iID + ", ";
                    pGisRec.Query += "'', ";	// district
                    pGisRec.Query += "'" + sBrgyCode + "', '0', '.', ";
                    pGisRec.Query += "'" + sBldgCode + "', ";
                    pGisRec.Query += "'" + StringUtilities.HandleApostrophe(txtBnsNm.Text.Trim()) + "')";
                    if (pGisRec.ExecuteNonQuery() == 0)
                    { }

                    pGisRec.Query = "select * from gis_business_map where bin = '" + bin1.GetBin() + "'";
                    if (pGisRec.Execute())
                    {
                        if (pGisRec.Read())
                        {
                            pGisRec.Close();

                            pGisRec.Query = "update gis_business_map set mapped = '1'";
                            pGisRec.Query += " where bin = '" + bin1.GetBin() + "'";
                            if (pGisRec.ExecuteNonQuery() == 0)
                            { }
                        }
                        else
                        {
                            pGisRec.Close();

                            pGisRec.Query = "insert into gis_business_map values (";
                            pGisRec.Query += "'" + bin1.GetBin() + "', ";
                            pGisRec.Query += "'" + sBrgyName + "', '1')";
                            if (pGisRec.ExecuteNonQuery() == 0)
                            { }
                        }
                    }

                    MessageBox.Show("Records matched", "Business Mapping Cleansing Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearControls("1");
                    ClearControls("2");
                    LoadOfficialList();

                }

            }
            catch { }
        }

        private int GetGisID(OracleResultSet pGisRec2, string sLandPin, string sBldgCode)
        {
            // RMC 20120207 modifications in business mapping
            int iID = 0;

            pGisRec2.Query = "select * from gis_businesses where land_pin = '" + sLandPin + "'";
            pGisRec2.Query += " and bldgcode = '" + sBldgCode + "'";
            if (pGisRec2.Execute())
            {
                if (pGisRec2.Read())
                {
                    iID = pGisRec2.GetInt("id");

                    pGisRec2.Close();

                    pGisRec2.Query = "update gis_businesses set bin = '', legend = '1'";
                    pGisRec2.Query += " where land_pin = '" + sLandPin + "'";
                    pGisRec2.Query += " and bldgcode = '" + sBldgCode + "'";
                    if (pGisRec2.ExecuteNonQuery() == 0)
                    { }
                }
                else
                    pGisRec2.Close();
            }

            return iID;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();

            string sQuery = "";
            string sMatchFilter = "";

            if (rdoPermitNo.Checked)
            {
                pRec.Query = "select * from businesses where permit_no in ";
                pRec.Query += "(select permit_no from btm_temp_businesses where trim(bin) is null) order by bin";
                sMatchFilter = "by Permit No.";
            }
            else if (rdoBnsName.Checked)
            {
                pRec.Query = "select * from businesses where bns_nm in ";
                pRec.Query += "(select bns_nm from btm_temp_businesses where trim(bin) is null) order by bin";
                sMatchFilter = "by Business Name";
            }
            else if (rdoOwnName.Checked)
            {
                pRec.Query = "select * from businesses where own_code in ";
                pRec.Query += "(select own_code from btm_temp_businesses where trim(bin) is null) order by bin";
                sMatchFilter = "by Owner's Name";
            }
            sQuery = pRec.Query.ToString();

            if (dgvOfficialList.Rows.Count == 0)
            {
                MessageBox.Show("No data to print");
                return;
            }

            frmReportTest PrintClass = new frmReportTest();
            PrintClass.ReportName = "Print Matching";
            PrintClass.MatchFilter = sMatchFilter;
            PrintClass.Query = sQuery;
            PrintClass.ShowDialog();
        }
    
    }
}