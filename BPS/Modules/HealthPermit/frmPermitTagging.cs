//GMC 20150904 Selecting type of permit to be printed per line of Business
//GMC 20150908 Update permit type when bns_code exist in the permit_tagging table
//GMC 20150910 Update Permit Tagging Module
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.HealthPermit
{
    public partial class frmPermitTagging : Form
    {
        public frmPermitTagging()
        {   
            InitializeComponent();
        }
        private void frmPermitTagging_Load(object sender, EventArgs e)
        {
            UpdateList();
            CheckList(bussTypeList);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
        private void UpdateList()
        {
            string strQuery = string.Empty;
            
            strQuery = string.Format("select bns_desc from bns_table where fees_code = 'B' and length(trim(bns_code)) = 2 order by bns_code");
            this.LoadList(strQuery, bussTypeList);
        }
        private void LoadList(string strQuery, DataGridView dgvList)
        {
            dgvList.Columns.Add(new DataGridViewCheckBoxColumn());

            DataGridViewOracleResultSet dsList = new DataGridViewOracleResultSet(dgvList, strQuery, 0, 0);
            dgvList.RowHeadersVisible = false;
            dgvList.Columns[0].Width = 30;
            dgvList.Columns[1].Width = 300;
            dgvList.Columns[1].ReadOnly = true;
            dgvList.Columns[1].HeaderText = "Business Type";
            dgvList.Refresh();
        }
        private void CheckList(DataGridView dgvList)
        {
            OracleResultSet result = new OracleResultSet();

            string strDesc = string.Empty;
            string strRowDesc = string.Empty;

            result.Query = string.Format("select * from permit_tagging");
            if (result.Execute())
            {
                while (result.Read())
                {
                    strDesc = result.GetString("bns_desc").Trim();
                    for (int i = 0; i < dgvList.Rows.Count; i++)
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
        private void btnTag_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm Business Tagging?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Generate(bussTypeList);
            }
        }
        private void Generate(DataGridView dgvList)
        {
            OracleResultSet result = new OracleResultSet();
            string sBnsCode=string.Empty;
            string sRevYear=string.Empty;
            bool isEqual = false;

            for (int i = 0; i < dgvList.Rows.Count; i++)
            {
                if (dgvList[0, i].Value != null)
                {
                    if ((bool)dgvList[0, i].Value == true)
                    {
                        result.Query = "select * from permit_tagging";
                        if (result.Execute())
                        {
                            while (result.Read())
                            {
                                if (result.GetString("bns_desc").Trim() == dgvList[1, i].Value.ToString().Trim())
                                {
                                    isEqual = true;
                                }
                            }
                        }
                        if (!isEqual)
                        {
                            result.Query = "select bns_code,rev_year from bns_table where trim(bns_desc)=:a";
                            result.AddParameter(":a", dgvList[1, i].Value.ToString().Trim());

                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    sBnsCode = result.GetString("bns_code").Trim();
                                    sRevYear = result.GetString("rev_year").Trim();
                                }
                            }

                            result.Query = "insert into permit_tagging values(:b,:c,:d,:e)";
                            result.AddParameter(":b", sBnsCode);
                            result.AddParameter(":c", dgvList[1, i].Value.ToString().Trim());
                            result.AddParameter(":d", "SPECIAL PERMIT");
                            result.AddParameter(":e", sRevYear);

                            if (result.ExecuteNonQuery() == 0) { }
                        }
                        isEqual = false;
                    }else if ((bool)dgvList[0, i].Value == false){
                        result.Query = "select * from permit_tagging";
                        if (result.Execute())
                        {
                            while (result.Read())
                            {
                                if (result.GetString("bns_desc").Trim() == dgvList[1, i].Value.ToString().Trim())
                                {
                                    result.Query = "delete from permit_tagging where trim(bns_desc)=:f";
                                    result.AddParameter(":f", dgvList[1, i].Value.ToString().Trim());

                                    if (result.ExecuteNonQuery() == 0) { }
                                }
                            }
                        }
                    }
                }
                result.Close();
            }
            MessageBox.Show("Tag Updated!","Special Permit Tagging",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}