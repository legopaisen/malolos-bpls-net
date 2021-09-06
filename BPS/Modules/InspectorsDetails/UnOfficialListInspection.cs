using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using System.Windows.Forms;

namespace Amellar.Modules.InspectorsDetails
{
    public class UnOfficialListInspection:UnOfficialList
    {
        public UnOfficialListInspection(frmUnOfficialList Form)
            : base(Form)
        {
        }

        public override void FormLoad()
        {
            UnOfficialListFrm.chkAll.Checked = true;
            UnOfficialListFrm.chkFilter.Checked = false;
            UnOfficialListFrm.lblInsNo.Text = "Control No.";
            UnOfficialListFrm.chkInspected.Visible = true;
            //UnOfficialListFrm.chkTaxMapped.Visible = true;    // RMC 20150425 disabled viewing of controls for business mapping in business record module, put rem

            this.ViewList();
            this.LoadInspector();
        }
         
        public override void ViewList()
        {
            OracleResultSet pSet = new OracleResultSet();
            int iRecCnt = 0;

            UnOfficialListFrm.dgvList.Columns.Clear();
            //dgvList.Columns.Add("INS_NO", "Inspection No.");
            UnOfficialListFrm.dgvList.Columns.Add("INS_NO", "Control No.");
            UnOfficialListFrm.dgvList.Columns.Add("BNS_NM", "Business Name");
            UnOfficialListFrm.dgvList.Columns.Add("BNS_ADD", "Business Address");
            UnOfficialListFrm.dgvList.Columns.Add("OWN_NM", "Owner Name");
            //dgvList.Columns.Add("INS", "Inspected By");
            UnOfficialListFrm.dgvList.Columns.Add("INS", "Added By");
           // UnOfficialListFrm.dgvList.Columns.Add("LP", "LAND PIN");
            UnOfficialListFrm.dgvList.RowHeadersVisible = false;
            UnOfficialListFrm.dgvList.Columns[0].Width = 100;
            UnOfficialListFrm.dgvList.Columns[1].Width = 150;
            UnOfficialListFrm.dgvList.Columns[2].Width = 150;
            UnOfficialListFrm.dgvList.Columns[3].Width = 150;
            UnOfficialListFrm.dgvList.Columns[4].Width = 100;
           // UnOfficialListFrm.dgvList.Columns[5].Width = 150;
            UnOfficialListFrm.dgvList.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            UnOfficialListFrm.dgvList.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            UnOfficialListFrm.dgvList.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            UnOfficialListFrm.dgvList.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            UnOfficialListFrm.dgvList.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            //UnOfficialListFrm.dgvList.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            if (UnOfficialListFrm.chkAll.Checked == true || UnOfficialListFrm.chkFilter.Checked == true
                || UnOfficialListFrm.chkInspected.Checked == true)
            {
                pSet.Query = "select is_number, bns_nm, bns_house_no ||' '|| bns_street ||' '|| bns_brgy ||' '|| bns_mun, ";
                pSet.Query += " own_ln || ' ' || own_fn, sys_user from unofficial_info_tbl a, own_names b "; // GDE 20120907
                //pSet.Query += " own_ln || ' ' || own_fn, sys_user, land_pin from unofficial_info_tbl a, own_names b, btm_gis_loc c";
                pSet.Query += " where a.own_code = b.own_code and trim(bin_settled) is null"; // GDE 20120907
                //pSet.Query += " where a.own_code = b.own_code and trim(bin_settled) is null and is_number = c.bin";
                /*
                if (UnOfficialListFrm.cmbInspector.Text.ToString() != "")
                    pSet.Query += " and sys_user = '%" + UnOfficialListFrm.cmbInspector.Text.ToString() + "%'";
                if (UnOfficialListFrm.txtBnsName.Text.ToString().Trim() != "")
                    pSet.Query += " and bns_nm like '%" + UnOfficialListFrm.txtBnsName.Text.ToString().Trim() + "%'";
                if (UnOfficialListFrm.txtBnsAdd.Text.ToString().Trim() != "")
                    pSet.Query += " and bns_house_no || bns_street || bns_brgy || bns_mun like '%" + UnOfficialListFrm.txtBnsAdd.Text.ToString().Trim() + "%'";
                if (UnOfficialListFrm.txtOwnName.Text.ToString().Trim() != "")
                    pSet.Query += " and b.own_code in (select own_code from own_names where own_ln || own_fn like '%" + UnOfficialListFrm.txtOwnName.Text.ToString().Trim() + "%')";
                 */
                pSet.Query += " order by bns_nm ";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        UnOfficialListFrm.dgvList.Rows.Add("");
                        UnOfficialListFrm.dgvList[0, iRecCnt].Value = pSet.GetString(0);
                        UnOfficialListFrm.dgvList[1, iRecCnt].Value = StringUtilities.RemoveApostrophe(pSet.GetString(1));
                        UnOfficialListFrm.dgvList[2, iRecCnt].Value = pSet.GetString(2);
                        UnOfficialListFrm.dgvList[3, iRecCnt].Value = pSet.GetString(3);
                        UnOfficialListFrm.dgvList[4, iRecCnt].Value = pSet.GetString(4);
                        //UnOfficialListFrm.dgvList[5, iRecCnt].Value = pSet.GetString(4);
                        iRecCnt++;

                    }

                    pSet.Close();
                }
            }


            // GDE try if ok - if walang business mapping
            /*
            if (UnOfficialListFrm.chkAll.Checked == true || UnOfficialListFrm.chkFilter.Checked == true
                || UnOfficialListFrm.chkTaxMapped.Checked == true)
            {
                pSet.Query = "select tbin, bns_nm, bns_house_no ||' '|| bns_street ||' '|| bns_brgy ||' '|| bns_mun, ";
                pSet.Query += " own_ln || ' ' || own_fn, bns_user from btm_temp_businesses a, own_names b "; // GDE 20120907
                //pSet.Query += " own_ln || ' ' || own_fn, bns_user, land_pin from btm_temp_businesses a, own_names b, btm_gis_loc c ";
                pSet.Query += " where a.own_code = b.own_code and trim(old_bin) is null";
                //pSet.Query += " where a.own_code = b.own_code and trim(old_bin) is null and tbin = c.bin";
                pSet.Query += " and trim(a.bin) is null";
                if (UnOfficialListFrm.txtBnsName.Text.ToString().Trim() != "")
                    pSet.Query += " and bns_nm like '%" + UnOfficialListFrm.txtBnsName.Text.ToString().Trim() + "%'";
                if (UnOfficialListFrm.txtBnsAdd.Text.ToString().Trim() != "")
                    pSet.Query += " and bns_house_no || bns_street || bns_brgy || bns_mun like '%" + UnOfficialListFrm.txtBnsAdd.Text.ToString().Trim() + "%'";
                if (UnOfficialListFrm.txtOwnName.Text.ToString().Trim() != "")
                    pSet.Query += " and b.own_code in (select own_code from own_names where own_ln || own_fn like '%" + UnOfficialListFrm.txtOwnName.Text.ToString().Trim() + "%')";
                pSet.Query += " order by bns_nm ";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        UnOfficialListFrm.dgvList.Rows.Add("");
                        UnOfficialListFrm.dgvList[0, iRecCnt].Value = pSet.GetString(0);
                        UnOfficialListFrm.dgvList[1, iRecCnt].Value = StringUtilities.RemoveApostrophe(pSet.GetString(1));
                        UnOfficialListFrm.dgvList[2, iRecCnt].Value = pSet.GetString(2);
                        UnOfficialListFrm.dgvList[3, iRecCnt].Value = pSet.GetString(3);
                        UnOfficialListFrm.dgvList[4, iRecCnt].Value = pSet.GetString(4);
                        //UnOfficialListFrm.dgvList[5, iRecCnt].Value = pSet.GetString(5);
                        iRecCnt++;

                        
                    }

                    pSet.Close();
                }
            }
            */
            // GDE try if ok - if walang business mapping

            if (iRecCnt == 0 && UnOfficialListFrm.chkFilter.Checked)
            {
                MessageBox.Show("No record found.", "BPLS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (iRecCnt > 0)
            {
                UnOfficialListFrm.txtInspectionNo.Text = UnOfficialListFrm.dgvList[0, 0].Value.ToString();
                UnOfficialListFrm.txtBnsName.Text = UnOfficialListFrm.dgvList[1, 0].Value.ToString();
                UnOfficialListFrm.txtBnsAdd.Text = UnOfficialListFrm.dgvList[2, 0].Value.ToString();
                UnOfficialListFrm.txtOwnName.Text = UnOfficialListFrm.dgvList[3, 0].Value.ToString();
                UnOfficialListFrm.cmbInspector.Text = UnOfficialListFrm.dgvList[4, 0].Value.ToString();
                //UnOfficialListFrm.cmbInspector.Text = UnOfficialListFrm.dgvList[5, 0].Value.ToString();
                UnOfficialListFrm.btnSearch.Text = "Clear";
            }
            
        }

        public override void LoadInspector()
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from inspector order by inspector_code";
            if (pSet.Execute())
            {

                while (pSet.Read())
                {
                    UnOfficialListFrm.cmbInspector.Items.Add(pSet.GetString("inspector_code"));
                }
            }
            pSet.Close();
        }

        public override void chkFilter_CheckStateChanged(object sender, EventArgs e)
        {
            if (UnOfficialListFrm.chkFilter.CheckState.ToString() == "Checked")
            {
                UnOfficialListFrm.chkAll.Checked = false;
                UnOfficialListFrm.chkInspected.Checked = false;
                UnOfficialListFrm.chkTaxMapped.Checked = false;
                UnOfficialListFrm.cmbInspector.Enabled = true;
                UnOfficialListFrm.txtOwnName.ReadOnly = false;
                UnOfficialListFrm.txtBnsName.ReadOnly = false;
                UnOfficialListFrm.txtBnsAdd.ReadOnly = false;
                UnOfficialListFrm.txtInspectionNo.ReadOnly = false;
                ClearControls();
                this.ClearControls();
            }
        }

        public override void chkInspected_CheckStateChanged(object sender, EventArgs e)
        {
            if (UnOfficialListFrm.chkInspected.CheckState.ToString() == "Checked")
            {
                UnOfficialListFrm.chkAll.Checked = false;
                UnOfficialListFrm.chkFilter.Checked = false;
                UnOfficialListFrm.chkTaxMapped.Checked = false;
                UnOfficialListFrm.cmbInspector.Enabled = false;
                UnOfficialListFrm.txtOwnName.ReadOnly = true;
                UnOfficialListFrm.txtBnsName.ReadOnly = true;
                UnOfficialListFrm.txtBnsAdd.ReadOnly = true;
                UnOfficialListFrm.txtInspectionNo.ReadOnly = true;
                ClearControls();
                this.ViewList();
            }
        }

        public override void chkTaxMapped_CheckStateChanged(object sender, EventArgs e)
        {
            if (UnOfficialListFrm.chkTaxMapped.CheckState.ToString() == "Checked")
            {
                UnOfficialListFrm.chkAll.Checked = false;
                UnOfficialListFrm.chkFilter.Checked = false;
                UnOfficialListFrm.chkInspected.Checked = false;
                UnOfficialListFrm.cmbInspector.Enabled = false;
                UnOfficialListFrm.txtOwnName.ReadOnly = true;
                UnOfficialListFrm.txtBnsName.ReadOnly = true;
                UnOfficialListFrm.txtBnsAdd.ReadOnly = true;
                UnOfficialListFrm.txtInspectionNo.ReadOnly = true;
                ClearControls();
                this.ViewList();
            }
        }

        public override void chkAll_CheckStateChanged(object sender, EventArgs e)
        {
            if (UnOfficialListFrm.chkAll.CheckState.ToString() == "Checked")
            {
                UnOfficialListFrm.chkFilter.Checked = false;
                UnOfficialListFrm.chkInspected.Checked = false;
                UnOfficialListFrm.chkTaxMapped.Checked = false;
                UnOfficialListFrm.cmbInspector.Enabled = false;
                UnOfficialListFrm.txtOwnName.ReadOnly = true;
                UnOfficialListFrm.txtBnsName.ReadOnly = true;
                UnOfficialListFrm.txtBnsAdd.ReadOnly = true;
                UnOfficialListFrm.txtInspectionNo.ReadOnly = true;
                ClearControls();
                this.ViewList();
            }
        }

    }
}
