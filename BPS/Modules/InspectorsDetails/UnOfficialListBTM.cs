using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.InspectorsDetails
{
    public class UnOfficialListBTM:UnOfficialList
    {
        public UnOfficialListBTM(frmUnOfficialList Form)
            : base(Form)
        {
        }

        public override void FormLoad()
        {
            UnOfficialListFrm.chkAll.Checked = true;
            UnOfficialListFrm.chkFilter.Checked = false;
            UnOfficialListFrm.cmbInspector.Enabled = false;
            UnOfficialListFrm.lblInsNo.Text = "Old BIN";
            UnOfficialListFrm.chkInspected.Visible = false;
            UnOfficialListFrm.chkTaxMapped.Visible = false;

            //this.ViewList();
            
        }

        public override void ViewList()
        {
            OracleResultSet pSet = new OracleResultSet();
            int iRecCnt = 0;

            UnOfficialListFrm.dgvList.Columns.Clear();
            UnOfficialListFrm.dgvList.Columns.Add("INS_NO", "Old BIN");
            UnOfficialListFrm.dgvList.Columns.Add("BNS_NM", "Business Name");
            UnOfficialListFrm.dgvList.Columns.Add("BNS_ADD", "Business Address");
            UnOfficialListFrm.dgvList.Columns.Add("OWN_NM", "Owner Name");
            UnOfficialListFrm.dgvList.Columns.Add("INS", "Added By");
            UnOfficialListFrm.dgvList.Columns.Add("LP", "LAND PIN"); // GDE 20120907
            UnOfficialListFrm.dgvList.RowHeadersVisible = false;
            UnOfficialListFrm.dgvList.Columns[0].Width = 150;
            UnOfficialListFrm.dgvList.Columns[1].Width = 150;
            UnOfficialListFrm.dgvList.Columns[2].Width = 150;
            UnOfficialListFrm.dgvList.Columns[3].Width = 150;
            UnOfficialListFrm.dgvList.Columns[4].Width = 50;
            UnOfficialListFrm.dgvList.Columns[5].Width = 150; // GDE 20120907
            UnOfficialListFrm.dgvList.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            UnOfficialListFrm.dgvList.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            UnOfficialListFrm.dgvList.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            UnOfficialListFrm.dgvList.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            UnOfficialListFrm.dgvList.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            UnOfficialListFrm.dgvList.Columns[4].Visible = false;
            UnOfficialListFrm.dgvList.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft; // GDE 20120907

            pSet.Query = "select tbin, bns_nm, bns_house_no ||' '|| bns_street ||' '|| bns_brgy ||' '|| bns_mun, ";
            //pSet.Query += " own_ln || ' ' || own_fn, bns_user from btm_temp_businesses a, own_names b "; // GDE 20120907
            pSet.Query += " own_ln || ' ' || own_fn, bns_user, land_pin from btm_temp_businesses a, own_names b, btm_gis_loc c ";
            pSet.Query += " where a.own_code = b.own_code and trim(old_bin) is not null";
            pSet.Query += " and trim(a.bin) is null and a.tbin = c.bin"; // GDE 20120907
            if (UnOfficialListFrm.txtBnsName.Text.ToString().Trim() != "")
                pSet.Query += " and bns_nm like '" + UnOfficialListFrm.txtBnsName.Text.ToString().Trim() + "'";
            if (UnOfficialListFrm.txtBnsAdd.Text.ToString().Trim() != "")
                pSet.Query += " and bns_house_no || bns_street || bns_brgy || bns_mun like '" + UnOfficialListFrm.txtBnsAdd.Text.ToString().Trim() + "'";
            if (UnOfficialListFrm.txtOwnName.Text.ToString().Trim() != "")
                pSet.Query += " and b.own_code in (select own_code from own_names where own_ln || own_fn like '" + UnOfficialListFrm.txtOwnName.Text.ToString().Trim() + "')";
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
                    UnOfficialListFrm.dgvList[5, iRecCnt].Value = pSet.GetString(5);
                    iRecCnt++;

                }
            }
            pSet.Close();

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
                UnOfficialListFrm.cmbInspector.Text = UnOfficialListFrm.dgvList[5, 0].Value.ToString();
                UnOfficialListFrm.btnSearch.Text = "Clear";
            }
            
        }

        public override void chkFilter_CheckStateChanged(object sender, EventArgs e)
        {
            if (UnOfficialListFrm.chkFilter.CheckState.ToString() == "Checked")
            {
                UnOfficialListFrm.chkAll.Checked = false;
                UnOfficialListFrm.chkInspected.Checked = false;
                UnOfficialListFrm.chkTaxMapped.Checked = false;
                UnOfficialListFrm.cmbInspector.Enabled = false;
                UnOfficialListFrm.txtOwnName.ReadOnly = false;
                UnOfficialListFrm.txtBnsName.ReadOnly = false;
                UnOfficialListFrm.txtBnsAdd.ReadOnly = false;
                UnOfficialListFrm.txtInspectionNo.ReadOnly = false;
                this.ClearControls();
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
