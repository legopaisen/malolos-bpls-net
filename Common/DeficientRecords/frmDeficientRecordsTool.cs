using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using System.Collections;
using Amellar.Common.StringUtilities;

namespace Amellar.Common.DeficientRecords
{
    /* Missing implementations
      (1) Does not allow same key to be inserted.
      (2) def_field_valid
    */
    //@author R.D.Ong
    public partial class frmDeficientRecordsTool : Form
    {

        private Hashtable m_hshCategories;
        private List<DeficientRecordsToolTypeItem> m_lstDeficientTypes;
        private List<DeficientRecordsToolInfoItem> m_lstDeficientInfos;
        private List<DeficientRecordsToolCommonItem> m_lstAccountTypes;
        private List<DeficientRecordsToolCommonItem> m_lstRequiredFields;

        //for editing
        private List<DeficientRecordsToolTypeItem> m_lstDeficientTypesEdit;
        private List<DeficientRecordsToolInfoItem> m_lstDeficientInfosEdit;
        private List<DeficientRecordsToolCommonItem> m_lstAccountTypesEdit;
        private List<DeficientRecordsToolCommonItem> m_lstRequiredFieldsEdit;


        private enum CategoryState
        {
            WaitState,
            AddState,
            EditState,
            DisabledState
        }

        private enum DeficiencyState
        {
            ViewState,
            UpdateState
        }


        //L, B, M, I, A

        private CategoryState m_enuCategoryState;
        private DeficiencyState m_enuDeficiencyState;

        private DeficiencyState m_enuAccountTypeState;
        private DeficiencyState m_enuRequiredFieldsState;

        public frmDeficientRecordsTool()
        {
            InitializeComponent();

            //deficient types and additional info
            m_hshCategories = new Hashtable();
            m_lstDeficientTypes = new List<DeficientRecordsToolTypeItem>();
            m_lstDeficientInfos = new List<DeficientRecordsToolInfoItem>();

            //account type
            m_lstAccountTypes = new List<DeficientRecordsToolCommonItem>();
            m_lstRequiredFields = new List<DeficientRecordsToolCommonItem>();

            //edit
            m_lstDeficientTypesEdit = new List<DeficientRecordsToolTypeItem>();
            m_lstDeficientInfosEdit = new List<DeficientRecordsToolInfoItem>();
            m_lstAccountTypesEdit = new List<DeficientRecordsToolCommonItem>();
            m_lstRequiredFieldsEdit = new List<DeficientRecordsToolCommonItem>();
        }

        private void frmDeficientRecordsTool_Load(object sender, EventArgs e)
        {
            //account types
            dgvAccountType.Columns.Clear();
            dgvAccountType.Columns.Add("Type", "Type");
            dgvAccountType.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvAccountType.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvAccountType.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvAccountType.Columns.Add("Length", "Length");
            dgvAccountType.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvAccountType.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvAccountType.Columns[1].Width = 80;

            //deficient types
            dgvDeficientTypes.Columns.Clear();
            dgvDeficientTypes.Columns.Add("Code", "Code");
            dgvDeficientTypes.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvDeficientTypes.Columns[0].ReadOnly = true;
            dgvDeficientTypes.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            dgvDeficientTypes.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDeficientTypes.Columns[0].Width = 50;
            dgvDeficientTypes.Columns.Add("DeficiencyType", "Deficiency Type");
            dgvDeficientTypes.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvDeficientTypes.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvDeficientTypes.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDeficientTypes.Columns[1].Width = 100;
            
            //dgvDeficientTypes.Columns.Add("AutoCheck", "Auto Check");
            dgvDeficientTypes.Columns.Add(new DataGridViewCheckBoxColumn());
            dgvDeficientTypes.Columns[2].HeaderText = "W/ Validation";
            dgvDeficientTypes.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvDeficientTypes.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDeficientTypes.Columns[2].Width = 60;

            dgvDeficientTypes.Columns.Add("FieldCode", "Field Code");
            dgvDeficientTypes.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvDeficientTypes.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvDeficientTypes.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDeficientTypes.Columns[3].Width = 70;

            //dgvDeficientTypes.Columns.Add("Block Transaction", "Block Transaction");
            dgvDeficientTypes.Columns.Add(new DataGridViewCheckBoxColumn());
            dgvDeficientTypes.Columns[4].HeaderText = "With BIN";
            dgvDeficientTypes.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvDeficientTypes.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDeficientTypes.Columns[4].Width = 50;

            dgvDeficientTypes.Columns.Add(new DataGridViewCheckBoxColumn());
            dgvDeficientTypes.Columns[5].HeaderText = "With OR";
            dgvDeficientTypes.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvDeficientTypes.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDeficientTypes.Columns[5].Width = 50;
            

            //additional info
            dgvAdditionalInfo.Columns.Clear();
            dgvAdditionalInfo.Columns.Add("InfoCode", "Info Code");
            dgvAdditionalInfo.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvAdditionalInfo.Columns[0].ReadOnly = true;
            dgvAdditionalInfo.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            dgvAdditionalInfo.Columns[0].Width = 80;
            dgvAdditionalInfo.Columns[0].DefaultCellStyle.SelectionBackColor = Color.White;
            dgvAdditionalInfo.Columns[0].DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvAdditionalInfo.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvAdditionalInfo.Columns.Add("Information", "Information");
            dgvAdditionalInfo.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvAdditionalInfo.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvAdditionalInfo.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgvAdditionalInfo.Columns.Add("Required", "Required");
            dgvAdditionalInfo.Columns.Add(new DataGridViewCheckBoxColumn());
            dgvAdditionalInfo.Columns[2].HeaderText = "Required";
            dgvAdditionalInfo.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvAdditionalInfo.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvAdditionalInfo.Columns[2].Width = 100;

            dgvRequiredFields.Columns.Clear();
            dgvRequiredFields.Columns.Add("Code", "Code");
            dgvRequiredFields.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvRequiredFields.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRequiredFields.Columns.Add("FieldName" , "Field Name");
            dgvRequiredFields.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvRequiredFields.Columns[0].Width = 80;
            dgvRequiredFields.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvRequiredFields.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvSolutions.Columns.Clear();
            dgvSolutions.Columns.Add("Code", "Code");
            dgvSolutions.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvSolutions.Columns.Add("Solution", "Solution");
            dgvSolutions.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvSolutions.Columns[0].Width = 80;
            dgvSolutions.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvSolutions.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSolutions.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.LoadCategories(); //deficient types
            btnCategoryDelete.Enabled = false;
            btnCategoryEdit.Enabled = false;

            this.LoadAccountTypes(); //account types
            this.LoadRequiredFields(); //required fields

            this.Category = CategoryState.WaitState;
            this.Deficiency = DeficiencyState.ViewState;
            this.AccountType = DeficiencyState.ViewState;
            this.RequiredFields = DeficiencyState.ViewState;
        }

        private void LoadAdditionalInfo(string strCategoryCode, string strDeficientTypeCode)
        {
            m_lstDeficientInfos.Clear();
            OracleResultSet result = new OracleResultSet();
            result.Query = string.Format("SELECT adinfo_code, adinfo_name, requirement FROM def_adinfo_table WHERE def_code = '{0}' ORDER BY adinfo_code",
                strDeficientTypeCode);
            if (result.Execute())
            {
                while (result.Read())
                {
                    m_lstDeficientInfos.Add(new DeficientRecordsToolInfoItem(strCategoryCode, result.GetString("adinfo_code").Trim(),
                        result.GetString("adinfo_name").Trim(), result.GetString("requirement").Trim()));
                }
            }

            dgvAdditionalInfo.Rows.Clear();
            int intCount = 0;
            intCount = m_lstDeficientInfos.Count;
            for (int i = 0; i < intCount; i++)
            {
                dgvAdditionalInfo.Rows.Add(m_lstDeficientInfos[i].Key, m_lstDeficientInfos[i].Value,
                    m_lstDeficientInfos[i].IsRequired);
            }
        }

        private void LoadDeficiencyTypes(string strCategoryCode)
        {
            m_lstDeficientTypes.Clear();
            OracleResultSet result = new OracleResultSet();
            OracleResultSet pRec = new OracleResultSet();
            string strFieldCode = string.Empty;

            result.Query = string.Format("SELECT * FROM def_rec_table WHERE cat_code = '{0}' ORDER BY def_code", 
                strCategoryCode);
            if (result.Execute())
            {
                while (result.Read())
                {
                    /*m_lstDeficientTypes.Add(new DeficientRecordsToolTypeItem(strCategoryCode, result.GetString("def_code").Trim(),
                        result.GetString("def_name").Trim(), result.GetString("tag_stat").Trim(),
                        result.GetString("block_trans").Trim(), result.GetString("tag_kind").Trim()));*/

                    strFieldCode = "";
                    pRec.Query = string.Format("select a.def_code, b.field_name from def_field_valid a, def_field_table b where a.def_code = '{0}' and a.def_code = b.field_code",result.GetString("def_code").Trim());
                    if(pRec.Execute())
                    {
                        if (pRec.Read())
                            strFieldCode = pRec.GetString(0) + "  " + pRec.GetString(1);
                    }
                    pRec.Close();

                    m_lstDeficientTypes.Add(new DeficientRecordsToolTypeItem(strCategoryCode, result.GetString("def_code").Trim(),
                        result.GetString("def_name").Trim(), result.GetString("validation").Trim(),
                        strFieldCode, result.GetString("temp_pin").Trim(), result.GetString("temp_or")));
                    
                }
            }

            result.Close();

            dgvDeficientTypes.Rows.Clear();
            int intCount = 0;
            intCount = m_lstDeficientTypes.Count;
            for (int i = 0; i < intCount; i++)
            {
                /*
                dgvDeficientTypes.Rows.Add(m_lstDeficientTypes[i].Key, m_lstDeficientTypes[i].Value,
                    m_lstDeficientTypes[i].IsAutoCheck, m_lstDeficientTypes[i].IsBlockTransaction,
                    m_lstDeficientTypes[i].Kind);
                 */
                DataGridViewRow row = new DataGridViewRow();
                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells[0].Value = m_lstDeficientTypes[i].Key;
                row.Cells.Add(new DataGridViewTextBoxCell());
                row.Cells[1].Value = m_lstDeficientTypes[i].Value;
                row.Cells.Add(new DataGridViewCheckBoxCell());
                row.Cells[2].Value = m_lstDeficientTypes[i].IsAutoCheck;
                row.Cells.Add(this.GetDefField(m_lstDeficientTypes[i].FieldCode));
                row.Cells.Add(new DataGridViewCheckBoxCell());
                row.Cells[4].Value = m_lstDeficientTypes[i].IsWithBIN;
                row.Cells.Add(new DataGridViewCheckBoxCell());
                row.Cells[5].Value = m_lstDeficientTypes[i].IsWithOR;
                dgvDeficientTypes.Rows.Add(row);
            }
        }

        /// <summary>
        /// This method loads all categories.
        /// </summary>
        private void LoadCategories()
        {

            m_hshCategories.Clear();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from def_cat_table order by cat_code";
            if (result.Execute())
            {
                while (result.Read())
                {
                    m_hshCategories.Add(result.GetString("cat_code").Trim(), result.GetString("cat_name").Trim());
                }
            }
            result.Close();

            cmbCategoryName.Items.Clear();
            ArrayList alCategories = new ArrayList(m_hshCategories.Values);
            alCategories.Sort();
            foreach (object category in alCategories)
            {
                cmbCategoryName.Items.Add(category.ToString());
            }
        }

        private void LoadRequiredFields()
        {
            int intSerial = 0;
            m_lstRequiredFields.Clear();
            m_lstRequiredFieldsEdit.Clear();

            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from def_field_table order by field_code";
            if (result.Execute())
            {
                while (result.Read())
                {
                    m_lstRequiredFields.Add(new DeficientRecordsToolCommonItem(++intSerial,
                        result.GetString("field_code").Trim(),
                        result.GetString("field_name").Trim()));
                }
            }
            result.Close();

            int intCount = 0;
            intCount = m_lstRequiredFields.Count;
            dgvRequiredFields.Rows.Clear();
            for (int i = 0; i < intCount; i++)
                dgvRequiredFields.Rows.Add(m_lstRequiredFields[i].Key, m_lstRequiredFields[i].Value);

        }

        private void LoadAccountTypes()
        {
            int intSerial = 0;
            m_lstAccountTypes.Clear();
            m_lstAccountTypesEdit.Clear();

            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from def_rec_acct order by rec_acct_type";
            if (result.Execute())
            {
                while (result.Read())
                {
                    m_lstAccountTypes.Add(new DeficientRecordsToolCommonItem(++intSerial,
                        result.GetString("rec_acct_type").Trim(),
                        result.GetString("char_no").Trim()));
                }
            }
            result.Close();

            int intCount = 0;
            intCount = m_lstAccountTypes.Count;
            dgvAccountType.Rows.Clear();
            for (int i = 0; i < intCount; i++)
                dgvAccountType.Rows.Add(m_lstAccountTypes[i].Key, m_lstAccountTypes[i].Value);

        }

        private void ReloadValues()
        {
            string strCategoryCode = string.Empty;
            strCategoryCode = txtCategoryCode.Text.Trim();
            string strDeficientTypeCode = this.DeficienTypeCode;

            this.LoadDeficiencyTypes(strCategoryCode);
            this.LoadAdditionalInfo(strCategoryCode, strDeficientTypeCode);
        }

        private void cmbCategoryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCategoryCode.Text = string.Empty;
            
            m_lstDeficientTypes.Clear();
            m_lstDeficientInfos.Clear();
            dgvDeficientTypes.Rows.Clear();
            dgvAdditionalInfo.Rows.Clear();

            string strValue = string.Empty;
            if (cmbCategoryName.SelectedIndex != -1)
                strValue = cmbCategoryName.Items[cmbCategoryName.SelectedIndex].ToString();

            btnCategoryEdit.Enabled = false;
            btnCategoryDelete.Enabled = false;

            foreach (DictionaryEntry de in m_hshCategories)
            {
                if (strValue == de.Value.ToString())
                {
                    txtCategoryCode.Text = de.Key.ToString();
                    this.LoadDeficiencyTypes(de.Key.ToString());

                    if (m_enuCategoryState == CategoryState.WaitState)
                    {
                        btnCategoryEdit.Enabled = true;
                        //btnCategoryDelete.Enabled = true;
                        btnCategoryDelete.Enabled = false;
                    }
                }
            }
            if (m_enuCategoryState == CategoryState.WaitState)
                btnUpdate.Enabled = cmbCategoryName.SelectedIndex != -1;

            if (m_enuDeficiencyState == DeficiencyState.UpdateState)
            {
                this.AddNewItem(true);
                
                int intCount = 0;
                int intCount2 = 0;
                intCount = m_lstDeficientTypesEdit.Count;
                intCount2 = dgvDeficientTypes.Rows.Count;
                for (int i = 0; i < intCount; i++)
                {
                    for (int j = 0; j < intCount2; j++)
                    {
                        if (txtCategoryCode.Text.Trim() == m_lstDeficientTypesEdit[i].CategoryCode &&
                            dgvDeficientTypes.Rows[j].Cells[0].Value.ToString() == m_lstDeficientTypesEdit[i].Key)
                        {
                            dgvDeficientTypes.Rows[j].Cells[1].Value = m_lstDeficientTypesEdit[i].Value;
                            dgvDeficientTypes.Rows[j].Cells[2].Value = m_lstDeficientTypesEdit[i].IsAutoCheck;
                            dgvDeficientTypes.Rows[j].Cells[3].Value = m_lstDeficientTypesEdit[i].FieldCode;
                            dgvDeficientTypes.Rows[j].Cells[4].Value = m_lstDeficientTypesEdit[i].IsWithBIN;
                            dgvDeficientTypes.Rows[j].Cells[5].Value = m_lstDeficientTypesEdit[i].IsWithOR;
                            dgvDeficientTypes.Rows[j].DefaultCellStyle.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }

        public string DeficienTypeCode
        {
            get
            {
                string strDeficientTypeCode = string.Empty;
                for (int j = 0; j < dgvDeficientTypes.Rows.Count; j++)
                {
                    if (dgvDeficientTypes.Rows[j].Selected)
                    {
                        strDeficientTypeCode = dgvDeficientTypes.Rows[j].Cells[0].Value.ToString();
                        break;
                    }
                }
                return strDeficientTypeCode;
            }
        }


        //new codes
        private string NewCategoryCode()
        {
            int intCategoryCode = 0;
            OracleResultSet result = new OracleResultSet();
            result.Query = "SELECT MAX(cat_code) FROM def_cat_table";
            int.TryParse(result.ExecuteScalar().Trim(), out intCategoryCode);
            result.Close();
            intCategoryCode++;

            return string.Format("{0:00#}", intCategoryCode);
        }

        private DeficiencyState AccountType
        {
            get { return m_enuAccountTypeState; }
            set
            {
                m_enuAccountTypeState = value;
                if (m_enuAccountTypeState == DeficiencyState.ViewState)
                {
                    btnAccountEdit.Text = "Edit";
                    btnAccountCancel.Enabled = false;
                    dgvAccountType.Columns[0].ReadOnly = true;
                }
                else if (m_enuAccountTypeState == DeficiencyState.UpdateState)
                {
                    btnAccountEdit.Text = "Update";
                    btnAccountCancel.Enabled = true;
                    dgvAccountType.Columns[0].ReadOnly = false;

                }
            }
        }

        private DeficiencyState RequiredFields
        {
            get { return m_enuRequiredFieldsState; }
            set
            {
                m_enuRequiredFieldsState = value;
                if (m_enuRequiredFieldsState == DeficiencyState.ViewState)
                {
                    btnReqEdit.Text = "Edit";
                    btnReqCancel.Enabled = false;
                    dgvRequiredFields.Columns[0].ReadOnly = true;
                }
                else if (m_enuRequiredFieldsState == DeficiencyState.UpdateState)
                {
                    btnReqEdit.Text = "Update";
                    btnReqCancel.Enabled = true;
                    dgvRequiredFields.Columns[0].ReadOnly = false;
                }
            }
        }


        private DeficiencyState Deficiency
        {
            get { return m_enuDeficiencyState; }
            set
            {
                m_enuDeficiencyState = value;
                if (m_enuDeficiencyState == DeficiencyState.ViewState)
                {
                    btnPrint.Enabled = true;
                    btnUpdate.Enabled = cmbCategoryName.SelectedIndex != -1;
                    btnCancel.Enabled = false;
                    btnUpdate.Text = "Edit";

                    //reload values
                }
                else if (m_enuDeficiencyState == DeficiencyState.UpdateState)
                {
                    m_lstDeficientTypesEdit.Clear();
                    m_lstDeficientInfosEdit.Clear();

                    btnPrint.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnCancel.Enabled = true;
                    btnUpdate.Text = "Update";
                    //add new item
                    this.AddNewItem(true); 
                }
            }
        }

        private void AddNewItem(bool blnHasDeficientType)
        {
            int intKey = 0;
            if (m_lstDeficientTypes.Count > 0)
                int.TryParse(m_lstDeficientTypes[m_lstDeficientTypes.Count - 1].Key, out intKey);
            intKey++;
            if (blnHasDeficientType)
            {
                if (intKey <= 999)
                {
                    if (dgvDeficientTypes.Rows.Count > 0 && dgvDeficientTypes.Rows[dgvDeficientTypes.Rows.Count - 1].Cells[0].Value.ToString() ==
                        string.Format("{0:00#}", intKey))
                    {
                        //do nothing
                    }
                    else
                    {
                        //dgvDeficientTypes.Rows.Add(string.Format("{0:00#}", intKey), string.Empty, false, false, string.Empty);
                        DataGridViewRow row = new DataGridViewRow();
                        row.Cells.Add(new DataGridViewTextBoxCell());
                        row.Cells[0].Value = string.Format("{0:00#}", intKey);
                        row.Cells.Add(new DataGridViewTextBoxCell());
                        row.Cells[1].Value = string.Empty;
                        row.Cells.Add(new DataGridViewCheckBoxCell());
                        row.Cells[2].Value = false;
                        row.Cells.Add(this.GetDefField(string.Empty));
                        row.Cells.Add(new DataGridViewCheckBoxCell());
                        row.Cells[4].Value = false;
                        row.Cells.Add(new DataGridViewCheckBoxCell());
                        row.Cells[5].Value = false;
                        
                        dgvDeficientTypes.Rows.Add(row);

                    }
                }
            }
            int intAddlKey = 0;
            if (m_lstDeficientInfos.Count > 0)
                int.TryParse(m_lstDeficientInfos[m_lstDeficientInfos.Count - 1].Key.Substring(3), out intAddlKey);
            intAddlKey++;
            int intTmpKey = 0;
            int.TryParse(this.DeficienTypeCode, out intTmpKey);

            if (intAddlKey <= 99 && intKey != intTmpKey)
            {
                if (dgvAdditionalInfo.Rows.Count > 0 && dgvAdditionalInfo.Rows[dgvAdditionalInfo.Rows.Count - 1].Cells[0].Value.ToString() ==
                    string.Format("{0}{1:0#}", this.DeficienTypeCode, intAddlKey))
                {
                }
                else
                {
                    dgvAdditionalInfo.Rows.Add(string.Format("{0}{1:0#}",
                        this.DeficienTypeCode, intAddlKey), string.Empty, false);
                }
            }
        }

        private CategoryState Category
        {
            get { return m_enuCategoryState; }
            set
            {
                bool blnIsCategorySelect = false;
                bool blnIsAddEnabled = false;
                bool blnIsEditEnabled = false;
                bool blnIsDeleteEnabled = false;
                bool blnIsValuesEnabled = false;

                int intCategoryIndex = -1;
                intCategoryIndex = cmbCategoryName.SelectedIndex;

                string strCategoryText = string.Empty;
                strCategoryText = txtCategoryName.Text.Trim();
                
                m_enuCategoryState = value;
                if (m_enuCategoryState == CategoryState.WaitState)
                {
                    blnIsCategorySelect = true;

                    btnCategoryAdd.Text = "Add";
                    btnCategoryEdit.Text = "Edit";
                    btnCategoryDelete.Text = "Delete";

                    blnIsAddEnabled = true;
                    blnIsEditEnabled = intCategoryIndex != -1;
                    //blnIsDeleteEnabled = intCategoryIndex != -1;
                    blnIsDeleteEnabled = false; // intCategoryIndex != -1;
                       
                }
                else if (m_enuCategoryState == CategoryState.AddState)
                {
                    blnIsCategorySelect = false;
                    btnCategoryEdit.Text = "Edit";
                    blnIsEditEnabled = false;

                    btnCategoryAdd.Text = "Save";
                    //add additional validation here if any
                    btnCategoryAdd.Enabled = strCategoryText != string.Empty;

                    btnCategoryDelete.Text = "Cancel";
                    blnIsDeleteEnabled = true;
                }
                else if (m_enuCategoryState == CategoryState.EditState)
                {
                    blnIsCategorySelect = false;
                    btnCategoryAdd.Text = "Add";
                    blnIsAddEnabled = false;

                    btnCategoryEdit.Text = "Update";
                    //add additional validation here if any
                    btnCategoryAdd.Enabled = strCategoryText != string.Empty;

                    btnCategoryDelete.Text = "Cancel";
                    blnIsDeleteEnabled = true;

                }
                else if (m_enuCategoryState == CategoryState.DisabledState)
                {
                    blnIsCategorySelect = true;

                    btnCategoryAdd.Text = "Add";
                    btnCategoryEdit.Text = "Edit";
                    btnCategoryDelete.Text = "Delete";

                    blnIsAddEnabled = false;
                    blnIsEditEnabled = false;
                    blnIsDeleteEnabled = false;
                    blnIsValuesEnabled = true;
                }

                if (blnIsCategorySelect)
                    txtCategoryName.Text = string.Empty;

                txtCategoryName.Enabled = !blnIsCategorySelect;
                txtCategoryName.Visible = !blnIsCategorySelect;
                cmbCategoryName.Enabled = blnIsCategorySelect;
                cmbCategoryName.Visible = blnIsCategorySelect;

                btnCategoryAdd.Enabled = blnIsAddEnabled;
                btnCategoryEdit.Enabled = blnIsEditEnabled;
                btnCategoryDelete.Enabled = blnIsDeleteEnabled;

                dgvDeficientTypes.Columns[1].ReadOnly = !blnIsValuesEnabled;
                dgvDeficientTypes.Columns[2].ReadOnly = !blnIsValuesEnabled;
                dgvDeficientTypes.Columns[3].ReadOnly = !blnIsValuesEnabled;
                dgvDeficientTypes.Columns[4].ReadOnly = !blnIsValuesEnabled;

                dgvAdditionalInfo.Columns[1].ReadOnly = !blnIsValuesEnabled;
                dgvAdditionalInfo.Columns[2].ReadOnly = !blnIsValuesEnabled;

                if (m_enuCategoryState == CategoryState.WaitState)
                {
                    btnPrint.Enabled = true;
                    btnUpdate.Enabled = cmbCategoryName.SelectedIndex != -1;
                    btnCancel.Enabled = false;
                }
                else if (m_enuCategoryState != CategoryState.DisabledState)
                {
                    btnPrint.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnCancel.Enabled = false;
                }
            }
        }
        
        private void btnCategoryAdd_Click(object sender, EventArgs e)
        {
            if (m_enuCategoryState == CategoryState.WaitState)
            {
                txtCategoryCode.Text = this.NewCategoryCode(); //get new category code
                m_lstDeficientTypes.Clear();
                m_lstDeficientInfos.Clear();
                dgvDeficientTypes.Rows.Clear();
                dgvAdditionalInfo.Rows.Clear();

                this.Category = CategoryState.AddState;
            }
            else if (m_enuCategoryState == CategoryState.AddState)
            {
                string strCategoryCode = string.Empty;
                string strCategoryName = string.Empty;
                strCategoryCode = txtCategoryCode.Text.Trim();
                strCategoryName = txtCategoryName.Text.Trim();
                //checks if name already exists
                if (m_hshCategories.ContainsValue(strCategoryName))
                {
                    MessageBox.Show("Category name already used.", string.Empty, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                else if (MessageBox.Show("Save Category?", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes)
                {
                    OracleResultSet result = new OracleResultSet();
                    result.Query = string.Format("INSERT INTO def_cat_table VALUES ('{0}', '{1}')",
                        strCategoryCode, strCategoryName.ToUpper());
                    if (result.ExecuteNonQuery() == 0)
                    {
                        result.Close();
                        MessageBox.Show("Category cannot be saved.", string.Empty, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                    result.Close();
                    this.LoadCategories();
                    this.SetCategorySelect(txtCategoryCode.Text.Trim());
                    this.Category = CategoryState.WaitState;
                }
            }
        }


        private void SetCategorySelect(string strCategoryCode)
        {
            //int intCurrentSelectedIndex = cmbCategoryName.SelectedIndex;
            int intSelectedIndex = -1;
            txtCategoryCode.Text = string.Empty;
            cmbCategoryName.SelectedIndex = -1;
            ArrayList alCategories = new ArrayList();
            foreach (DictionaryEntry de in m_hshCategories)
                alCategories.Add(string.Format("{0}|{1}", de.Value.ToString(), de.Key.ToString()));
            alCategories.Sort();
            int intCount = 0;
            string strCategory = string.Empty;
            intCount = alCategories.Count;
            for (int i = 0; i < intCount; i++)
            {
                strCategory = alCategories[i].ToString();
                if (strCategory.Substring(strCategory.IndexOf("|") + 1) == strCategoryCode)
                {
                    intSelectedIndex = i;
                    break;
                }
            }
            cmbCategoryName.SelectedIndex = intSelectedIndex;
        }

        private void btnCategoryEdit_Click(object sender, EventArgs e)
        {
            if (m_enuCategoryState == CategoryState.WaitState)
            {
                this.Category = CategoryState.EditState;
                txtCategoryName.Text = m_hshCategories[txtCategoryCode.Text.Trim()].ToString();
            }
            else if (m_enuCategoryState == CategoryState.EditState)
            {
                string strCategoryCode = string.Empty;
                string strCategoryName = string.Empty;
                strCategoryCode = txtCategoryCode.Text.Trim();
                strCategoryName = txtCategoryName.Text.Trim();
                //checks if name already exists
                if (m_hshCategories[strCategoryCode].ToString() != strCategoryName && 
                    m_hshCategories.ContainsValue(strCategoryName))
                {
                    MessageBox.Show("Category name already used.", string.Empty, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                else if (MessageBox.Show("Update Category?", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes)
                {
                    OracleResultSet result = new OracleResultSet();
                    result.Query = string.Format("UPDATE def_cat_table SET cat_name = '{0}' WHERE cat_code = '{1}'",
                        strCategoryName, strCategoryCode);
                    if (result.ExecuteNonQuery() == 0)
                    {
                        result.Close();
                        MessageBox.Show("Category cannot be updated.", string.Empty, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                    result.Close();

                    this.LoadCategories();
                    this.SetCategorySelect(strCategoryCode);
                    this.Category = CategoryState.WaitState;
                }
            }
        }

        private void btnCategoryDelete_Click(object sender, EventArgs e)
        {
            int intSelectedIndex = -1;
            intSelectedIndex = cmbCategoryName.SelectedIndex;
            if (m_enuCategoryState == CategoryState.WaitState)
            {
                string strCategoryCode = string.Empty;
                strCategoryCode = txtCategoryCode.Text.Trim();
                if (MessageBox.Show("Are you sure you want to delete this Category?", string.Empty,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleResultSet result = new OracleResultSet();
                    result.Query = string.Format("DELETE FROM def_cat_table WHERE cat_code = '{0}'", strCategoryCode);
                    if (result.ExecuteNonQuery() == 0)
                    {
                        result.Close();
                        MessageBox.Show("Category cannot be deleted.", string.Empty, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                    result.Close();
                    this.LoadCategories();
                }
                else
                {
                    return;
                }
                intSelectedIndex = -1;
            }
            //update everything
            txtCategoryCode.Text = string.Empty;
            cmbCategoryName.SelectedIndex = -1;
            cmbCategoryName.SelectedIndex = intSelectedIndex;

            this.Category = CategoryState.WaitState;
        }

        private void txtCategoryName_TextChanged(object sender, EventArgs e)
        {
            string strCategoryName = string.Empty;
            strCategoryName = txtCategoryName.Text.Trim();
            if (m_enuCategoryState == CategoryState.AddState)
                btnCategoryAdd.Enabled = strCategoryName != string.Empty;
            else if (m_enuCategoryState == CategoryState.EditState)
                btnCategoryEdit.Enabled = strCategoryName != string.Empty;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (m_enuCategoryState == CategoryState.WaitState)
            {
                this.Category = CategoryState.DisabledState;
                this.Deficiency = DeficiencyState.UpdateState;
            }
            else if (m_enuCategoryState == CategoryState.DisabledState)
            {
                string strCategoryCode = string.Empty;
                string strLastDeficientTypeCode = string.Empty;
                string strLastAdditionalCode = string.Empty;

                strCategoryCode = txtCategoryCode.Text.Trim();
                if (m_lstDeficientTypes.Count > 0)
                    strLastDeficientTypeCode = m_lstDeficientTypes[m_lstDeficientTypes.Count - 1].Key;
                if (m_lstDeficientInfos.Count > 0)
                    strLastAdditionalCode = m_lstDeficientInfos[m_lstDeficientInfos.Count - 1].Key;

                OracleResultSet result = new OracleResultSet();
                result.Transaction = true;

                int intCount = 0;
                intCount = m_lstDeficientTypesEdit.Count;
                for (int i = 0; i < intCount; i++)
                {
                    if (strLastDeficientTypeCode == string.Empty || 
                        m_lstDeficientTypesEdit[i].Key.ToString().CompareTo(strLastDeficientTypeCode) > 0)
                    {
                        result.Query = "INSERT INTO def_rec_table VALUES (:1, :2, :3, :4, :5, :6, :7)";
                        result.AddParameter(":1", m_lstDeficientTypesEdit[i].Key);
                        result.AddParameter(":2", m_lstDeficientTypesEdit[i].Value.ToUpper());
                        result.AddParameter(":3", m_lstDeficientTypesEdit[i].CategoryCode);
                        if (m_lstDeficientTypesEdit[i].IsAutoCheck)
                            result.AddParameter(":4", "Y");
                        else
                            result.AddParameter(":4", "N");
                        result.AddParameter(":5", "N");
                        if (m_lstDeficientTypesEdit[i].IsWithBIN)
                            result.AddParameter(":6", "Y");
                        else
                            result.AddParameter(":6", "N");
                        if (m_lstDeficientTypesEdit[i].IsWithOR)
                            result.AddParameter(":7", "Y");
                        else
                            result.AddParameter(":7", "N");

                        if (result.ExecuteNonQuery() == 0)
                        {
                            result.Rollback();
                            result.Close();
                            MessageBox.Show("Failed to insert records.", string.Empty, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }

                        if (m_lstDeficientTypesEdit[i].FieldCode.Trim() != "")
                        {
                            result.Query = "Insert into def_field_valid values (:1, :2)";
                            result.AddParameter(":1", m_lstDeficientTypesEdit[i].Key);
                            result.AddParameter(":2", m_lstDeficientTypesEdit[i].FieldCode);
                            if (result.ExecuteNonQuery() == 0)
                            {
                                result.Rollback();
                                result.Close();
                                MessageBox.Show("Failed to insert records.", string.Empty, MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                return;
                            }
                        }

                    }
                    else
                    {
                        //insert
                        result.Query = "UPDATE def_rec_table SET def_name =:1, validation = :2,  not_reflect = :3, temp_pin = :4, temp_or = :5 WHERE cat_code = :6 AND def_code = :7 ";
                        result.AddParameter(":1", m_lstDeficientTypesEdit[i].Value);
                        if (m_lstDeficientTypesEdit[i].IsAutoCheck)
                            result.AddParameter(":2", "Y");
                        else
                            result.AddParameter(":2", "N");
                        result.AddParameter(":3", "N");

                        if (m_lstDeficientTypesEdit[i].IsWithBIN)
                            result.AddParameter(":4", "Y");
                        else
                            result.AddParameter(":4", "N");
                        if (m_lstDeficientTypesEdit[i].IsWithOR)
                            result.AddParameter(":5", "Y");
                        else
                            result.AddParameter(":5", "N");
                        
                        result.AddParameter(":6", m_lstDeficientTypesEdit[i].CategoryCode);
                        result.AddParameter(":7", m_lstDeficientTypesEdit[i].Key);
                        if (result.ExecuteNonQuery() == 0)
                        {
                            result.Rollback();
                            result.Close();
                            MessageBox.Show("Failed to update records.", string.Empty, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                intCount = m_lstDeficientInfosEdit.Count;
                for (int i = 0; i < intCount; i++)
                {
                    if (strLastAdditionalCode == string.Empty || 
                        m_lstDeficientInfosEdit[i].Key.ToString().CompareTo(strLastAdditionalCode) > 0)
                    {
                        result.Query = "INSERT INTO def_adinfo_table VALUES (:1, :2, :3, :4, :5, 'C')";
                        result.AddParameter(":1", m_lstDeficientInfosEdit[i].Key.Substring(0, 3));
                        result.AddParameter(":2", m_lstDeficientInfosEdit[i].Key);
                        result.AddParameter(":3", m_lstDeficientInfosEdit[i].Value);
                        if (m_lstDeficientInfosEdit[i].IsRequired)
                            result.AddParameter(":4", "Y");
                        else
                            result.AddParameter(":4", "N");
                        result.AddParameter(":5", m_lstDeficientInfosEdit[i].CategoryCode);
                        if (result.ExecuteNonQuery() == 0)
                        {
                            result.Rollback();
                            result.Close();
                            MessageBox.Show("Failed to insert records.", string.Empty, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        //update
                        result.Query = "UPDATE def_adinfo_table SET adinfo_name = :1, requirement = :2 WHERE adinfo_code = :4";

                        result.AddParameter(":1", m_lstDeficientInfosEdit[i].Value);
                        if (m_lstDeficientInfosEdit[i].IsRequired)
                            result.AddParameter(":2", "Y");
                        else
                            result.AddParameter(":2", "N");
                        result.AddParameter(":3", m_lstDeficientInfosEdit[i].CategoryCode);
                        result.AddParameter(":4", m_lstDeficientInfosEdit[i].Key);
                        if (result.ExecuteNonQuery() == 0)
                        {
                            result.Rollback();
                            result.Close();
                            MessageBox.Show("Failed to update records.", string.Empty, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                if (!result.Commit())
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show("Failed to update records.", string.Empty, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                result.Close();

                this.Category = CategoryState.WaitState;
                this.Deficiency = DeficiencyState.ViewState;
                m_lstDeficientTypesEdit.Clear();
                m_lstDeficientInfosEdit.Clear();
                this.ReloadValues();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (m_enuCategoryState == CategoryState.DisabledState)
            {
                this.Category = CategoryState.WaitState;
                this.Deficiency = DeficiencyState.ViewState;
            }
            this.ReloadValues();
        }


        private DataGridViewComboBoxCell GetKindComboBoxCell(string strKind)
        {
            DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();
            cell.Items.Clear();
            ArrayList lstKinds = new ArrayList();
            lstKinds.Add(string.Empty);
            lstKinds.Add("L");
            lstKinds.Add("B");
            lstKinds.Add("M");
            lstKinds.Add("I");
            lstKinds.Add("A");

            for (int i = 0; i < lstKinds.Count; i++)
            {
                cell.Items.Add(lstKinds[i]);
            }
            if (lstKinds.IndexOf(strKind) == -1)
                cell.Value = lstKinds[0];
            else
                cell.Value = lstKinds[lstKinds.IndexOf(strKind)];

            return cell;
        }

        private DataGridViewComboBoxCell GetDefField(string strFieldCode)
        {
            OracleResultSet pSet = new OracleResultSet();
            
            DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();
            cell.Items.Clear();

            ArrayList lstKinds = new ArrayList();
            lstKinds.Add(string.Empty);
                                   
            pSet.Query = "select * from def_field_table order by field_code";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    //cell.Items.AddRange(pSet.GetString("field_code").Trim() + "  " + pSet.GetString("field_name").Trim());
                    lstKinds.Add(pSet.GetString("field_code").Trim() + "  " + pSet.GetString("field_name").Trim());
                }
            }
            pSet.Close();

            for (int i = 0; i < lstKinds.Count; i++)
            {
                cell.Items.Add(lstKinds[i]);
            }
            if (lstKinds.IndexOf(strFieldCode) == -1)
                cell.Value = lstKinds[0];
            else
                cell.Value = lstKinds[lstKinds.IndexOf(strFieldCode)];

            return cell;
        }

        private void dgvDeficientTypes_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (m_enuDeficiencyState == DeficiencyState.UpdateState)
            {
                string strDeficientTypeCode = string.Empty;
                strDeficientTypeCode = dgvDeficientTypes.Rows[e.RowIndex].Cells[0].Value.ToString();
                int intIndex = -1;
                int intCount = m_lstDeficientTypesEdit.Count;
                for (int i = 0; i < intCount; i++)
                {
                    if (m_lstDeficientTypesEdit[i].CategoryCode == txtCategoryCode.Text.Trim() &&
                        m_lstDeficientTypesEdit[i].Key == strDeficientTypeCode)
                    {
                        intIndex = i;
                        break;
                    }
                }
                dgvDeficientTypes.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;

                string strValue = string.Empty;
                bool blnIsAutoCheck = false;
                bool blnIsWithBIN = false;
                bool blnIsWithOR = false;
                string strFieldCode = string.Empty;

                if (dgvDeficientTypes.Rows[e.RowIndex].Cells[1].Value != null)
                {
                    strValue = dgvDeficientTypes.Rows[e.RowIndex].Cells[1].Value.ToString().ToUpper();
                    dgvDeficientTypes.Rows[e.RowIndex].Cells[1].Value = strValue;
                }
                if (dgvDeficientTypes.Rows[e.RowIndex].Cells[2].Value != null)
                    blnIsAutoCheck = (bool) dgvDeficientTypes.Rows[e.RowIndex].Cells[2].Value;
                if (dgvDeficientTypes.Rows[e.RowIndex].Cells[3].Value != null)
                {
                    strFieldCode = dgvDeficientTypes.Rows[e.RowIndex].Cells[3].Value.ToString();
                    strFieldCode = StringUtilities.StringUtilities.Left(strFieldCode, 3);
                }
                if (dgvDeficientTypes.Rows[e.RowIndex].Cells[4].Value != null)
                    blnIsWithBIN = (bool)dgvDeficientTypes.Rows[e.RowIndex].Cells[4].Value;
                if (dgvDeficientTypes.Rows[e.RowIndex].Cells[5].Value != null)
                    blnIsWithOR = (bool)dgvDeficientTypes.Rows[e.RowIndex].Cells[5].Value;
                if (intIndex == -1)
                {
                    string strIsAutoCheck = string.Empty;
                    string strIsWithBIN = string.Empty;
                    string strIsWithOR = string.Empty;

                    strIsAutoCheck = "N";
                    if (blnIsAutoCheck)
                        strIsAutoCheck = "Y";
                    strIsWithBIN = "N";
                    if (blnIsWithBIN)
                        strIsWithBIN = "Y";
                    if (blnIsWithOR)
                        strIsWithOR = "Y";

                    m_lstDeficientTypesEdit.Add(new DeficientRecordsToolTypeItem(
                        txtCategoryCode.Text.Trim(), strDeficientTypeCode, strValue,
                        strIsAutoCheck, strFieldCode, strIsWithBIN, strIsWithOR));

                    intIndex = m_lstDeficientTypesEdit.Count - 1;
                }
                else
                {
                    m_lstDeficientTypesEdit[intIndex].Value = strValue;
                    m_lstDeficientTypesEdit[intIndex].IsAutoCheck = blnIsAutoCheck;
                    m_lstDeficientTypesEdit[intIndex].IsWithBIN = blnIsWithBIN;
                    m_lstDeficientTypesEdit[intIndex].IsWithOR = blnIsWithOR;
                    m_lstDeficientTypesEdit[intIndex].FieldCode = strFieldCode;
                }

                if (strValue == string.Empty)
                {
                    m_lstDeficientTypesEdit.RemoveAt(intIndex);
                    btnUpdate.Enabled = m_lstDeficientInfosEdit.Count > 0 || m_lstDeficientTypesEdit.Count > 0;
                    return;
                }

                intCount = m_lstDeficientTypes.Count;
                for (int i = 0; i < intCount; i++)
                {
                    if (m_lstDeficientTypes[i].CategoryCode == m_lstDeficientTypesEdit[intIndex].CategoryCode &&
                        m_lstDeficientTypes[i].Key == m_lstDeficientTypesEdit[intIndex].Key)
                    {
                        if (m_lstDeficientTypes[i].Value == m_lstDeficientTypesEdit[intIndex].Value &&
                            m_lstDeficientTypes[i].IsAutoCheck == m_lstDeficientTypesEdit[intIndex].IsAutoCheck &&
                            m_lstDeficientTypes[i].IsWithBIN == m_lstDeficientTypesEdit[intIndex].IsWithBIN &&
                            m_lstDeficientTypes[i].IsWithOR == m_lstDeficientTypesEdit[intIndex].IsWithOR &&
                            m_lstDeficientTypes[i].FieldCode == m_lstDeficientTypesEdit[intIndex].FieldCode)
                        {
                            m_lstDeficientTypesEdit.RemoveAt(intIndex);
                            btnUpdate.Enabled = m_lstDeficientInfosEdit.Count > 0 || m_lstDeficientTypesEdit.Count > 0;
                            return;
                        }
                    }
                }

                dgvDeficientTypes.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                btnUpdate.Enabled = m_lstDeficientInfosEdit.Count > 0 || m_lstDeficientTypesEdit.Count > 0;
            }

        }

        private void dgvAdditionalInfo_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (m_enuDeficiencyState == DeficiencyState.UpdateState)
            {
                string strAddlCode = string.Empty;
                strAddlCode = dgvAdditionalInfo.Rows[e.RowIndex].Cells[0].Value.ToString();
                int intIndex = -1;
                int intCount = m_lstDeficientInfosEdit.Count;
                for (int i = 0; i < intCount; i++)
                {
                    if (m_lstDeficientInfosEdit[i].CategoryCode == txtCategoryCode.Text.Trim() &&
                        m_lstDeficientInfosEdit[i].Key == strAddlCode)
                    {
                        intIndex = i;
                        break;
                    }
                }
                dgvAdditionalInfo.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;

                string strValue = string.Empty;
                if (dgvAdditionalInfo.Rows[e.RowIndex].Cells[1].Value != null)
                    strValue = dgvAdditionalInfo.Rows[e.RowIndex].Cells[1].Value.ToString();
                bool blnIsRequired = (bool)dgvAdditionalInfo.Rows[e.RowIndex].Cells[2].Value;
                if (intIndex == -1)
                {
                    string strIsRequired = "N";
                    if (blnIsRequired)
                        strIsRequired = "Y";

                    m_lstDeficientInfosEdit.Add(new DeficientRecordsToolInfoItem(
                        txtCategoryCode.Text.Trim(), strAddlCode, strValue, strIsRequired));
                    intIndex = m_lstDeficientInfosEdit.Count - 1;
                }
                else
                {
                    m_lstDeficientInfosEdit[intIndex].Value = strValue;
                    m_lstDeficientInfosEdit[intIndex].IsRequired = blnIsRequired;
                }

                if (strValue == string.Empty)
                {
                    m_lstDeficientInfosEdit.RemoveAt(intIndex);
                    btnUpdate.Enabled = m_lstDeficientInfosEdit.Count > 0 || m_lstDeficientTypesEdit.Count > 0;
                    return;
                }
                //remove same values
                intCount = m_lstDeficientInfos.Count;
                for (int i = 0; i < intCount; i++)
                {
                    if (m_lstDeficientInfos[i].CategoryCode == m_lstDeficientInfosEdit[intIndex].CategoryCode &&
                        m_lstDeficientInfos[i].Key == m_lstDeficientInfosEdit[intIndex].Key)
                    {
                        if (m_lstDeficientInfos[i].Value == m_lstDeficientInfosEdit[intIndex].Value &&
                            m_lstDeficientInfos[i].IsRequired == m_lstDeficientInfosEdit[intIndex].IsRequired)
                        {
                            m_lstDeficientInfosEdit.RemoveAt(intIndex);
                            btnUpdate.Enabled = m_lstDeficientInfosEdit.Count > 0 || m_lstDeficientTypesEdit.Count > 0;
                            return;
                        }
                    }
                }
                dgvAdditionalInfo.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                btnUpdate.Enabled = m_lstDeficientInfosEdit.Count > 0 || m_lstDeficientTypesEdit.Count > 0;
            }
        }

        private void btnReqEdit_Click(object sender, EventArgs e)
        {
            if (m_enuRequiredFieldsState == DeficiencyState.ViewState)
            {
                this.RequiredFields = DeficiencyState.UpdateState;
                dgvRequiredFields.Rows.Add(string.Empty, string.Empty);
            }
            else if (m_enuRequiredFieldsState == DeficiencyState.UpdateState)
            {
                //save and reset
                int intCount = 0;
                OracleResultSet result = new OracleResultSet();
                result.Transaction = true;
                if (m_lstRequiredFields.Count > 0)
                {
                    result.Query = "DELETE FROM def_field_table";
                    if (result.ExecuteNonQuery() == 0)
                    {
                        result.Rollback();
                        result.Close();
                        MessageBox.Show("Failed to update Required Fields.", string.Empty, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }
                intCount = dgvRequiredFields.Rows.Count;
                string strKey = string.Empty;
                string strValue = string.Empty;
                for (int i = 0; i < intCount; i++)
                {
                    strKey = string.Empty;
                    if (dgvRequiredFields.Rows[i].Cells[0].Value != null)
                        strKey = dgvRequiredFields.Rows[i].Cells[0].Value.ToString().Trim();
                    strValue = string.Empty;
                    if (dgvRequiredFields.Rows[i].Cells[1].Value != null)
                        strValue = dgvRequiredFields.Rows[i].Cells[1].Value.ToString().Trim();
                    if (i == intCount - 1 && strKey == string.Empty)
                        break;

                    result.Query = "INSERT INTO def_field_table VALUES (:1, :2)";
                    result.AddParameter(":1", strKey);
                    result.AddParameter(":2", strValue);
                    if (result.ExecuteNonQuery() == 0)
                    {
                        result.Rollback();
                        result.Close();
                        MessageBox.Show("Failed to update Required Fields.", string.Empty, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }

                if (!result.Commit())
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show("Failed to update Required Fields.", string.Empty, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }   


                result.Close();
                this.RequiredFields = DeficiencyState.ViewState;
                this.LoadRequiredFields();
            }
        }

        private void btnReqCancel_Click(object sender, EventArgs e)
        {
            if (m_enuRequiredFieldsState == DeficiencyState.UpdateState)
            {
                this.RequiredFields = DeficiencyState.ViewState;
                this.LoadRequiredFields();
            }
        }

        private void btnAccountEdit_Click(object sender, EventArgs e)
        {
            if (m_enuAccountTypeState == DeficiencyState.ViewState)
            {
                this.AccountType = DeficiencyState.UpdateState;
                dgvAccountType.Rows.Add(string.Empty, string.Empty);
            }
            else if (m_enuAccountTypeState == DeficiencyState.UpdateState)
            {
                //save and reset
                int intCount = 0;
                OracleResultSet result = new OracleResultSet();
                result.Transaction = true;
                if (m_lstAccountTypes.Count > 0)
                {
                    result.Query = "DELETE FROM def_rec_acct";
                    if (result.ExecuteNonQuery() == 0)
                    {
                        result.Rollback();
                        result.Close();
                        MessageBox.Show("Failed to update Account Types.", string.Empty, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }
                intCount = dgvAccountType.Rows.Count;
                string strKey = string.Empty;
                string strValue = string.Empty;
                for (int i = 0; i < intCount; i++)
                {
                    strKey = string.Empty;
                    if (dgvAccountType.Rows[i].Cells[0].Value != null)
                        strKey = dgvAccountType.Rows[i].Cells[0].Value.ToString().Trim();
                    strValue = string.Empty;
                    if (dgvAccountType.Rows[i].Cells[1].Value != null)
                        strValue = dgvAccountType.Rows[i].Cells[1].Value.ToString().Trim();
                    if (i == intCount -1 && strKey == string.Empty)
                        break;
                    result.Query = "INSERT INTO def_rec_acct VALUES (:1, :2)";
                    result.AddParameter(":1", strKey);
                    result.AddParameter(":2", strValue);
                    if (result.ExecuteNonQuery() == 0)
                    {
                        result.Rollback();
                        result.Close();
                        MessageBox.Show("Failed to update Account Types.", string.Empty, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                }
                
                if (!result.Commit())
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show("Failed to update Account Types.", string.Empty, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                

                result.Close();

                this.AccountType = DeficiencyState.ViewState;
                this.LoadAccountTypes();
            }
        }

        private void btnAccountCancel_Click(object sender, EventArgs e)
        {
            if (m_enuAccountTypeState == DeficiencyState.UpdateState)
            {
                this.AccountType = DeficiencyState.ViewState;
                this.LoadAccountTypes();
            }
        }

        private void dgvAccountType_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (m_enuAccountTypeState == DeficiencyState.UpdateState)
            {
                int intCount = 0;
                intCount = dgvAccountType.Rows.Count;
                string strKey = string.Empty;
                if (dgvAccountType.Rows[e.RowIndex].Cells[0].Value != null)
                    strKey = dgvAccountType.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
                if (e.RowIndex == intCount - 1)
                {
                    if (strKey == string.Empty)
                        dgvAccountType.Rows[e.RowIndex].Cells[1].Value = string.Empty;
                }
                else if (strKey == string.Empty) //restore default value
                {
                    dgvAccountType.Rows[e.RowIndex].Cells[0].Value = m_lstAccountTypes[e.RowIndex].Key;
                }

            }
        }

        private void dgvRequiredFields_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (m_enuRequiredFieldsState == DeficiencyState.UpdateState)
            {
                int intCount = 0;
                intCount = dgvRequiredFields.Rows.Count;
                string strKey = string.Empty;
                if (dgvRequiredFields.Rows[e.RowIndex].Cells[0].Value != null)
                    strKey = dgvRequiredFields.Rows[e.RowIndex].Cells[0].Value.ToString().Trim().ToUpper();
                if (e.RowIndex == intCount - 1)
                {
                    if (strKey == string.Empty)
                        dgvRequiredFields.Rows[e.RowIndex].Cells[1].Value = string.Empty;
                }
                else if (strKey == string.Empty) //restore default value
                {
                    dgvRequiredFields.Rows[e.RowIndex].Cells[0].Value = m_lstRequiredFields[e.RowIndex].Key;
                }

                if (dgvRequiredFields.Rows[e.RowIndex].Cells[1].Value != null)
                {
                    string strTmpValue = dgvRequiredFields.Rows[e.RowIndex].Cells[1].Value.ToString().ToUpper();
                    dgvRequiredFields.Rows[e.RowIndex].Cells[1].Value = strTmpValue;
                }

            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            frmDeficientRecordsToolReport report = new frmDeficientRecordsToolReport();
            report.ShowDialog();
        }

        private void dgvDeficientTypes_SelectionChanged(object sender, EventArgs e)
        {
            string strCategoryCode = string.Empty;
            strCategoryCode = txtCategoryCode.Text.Trim();
            string strDeficientTypeCode = this.DeficienTypeCode;

            this.LoadAdditionalInfo(strCategoryCode, strDeficientTypeCode);

            if (m_enuDeficiencyState == DeficiencyState.UpdateState)
            {
                this.AddNewItem(false);

                int intCount = 0;
                int intCount2 = 0;
                intCount = m_lstDeficientInfosEdit.Count;
                intCount2 = dgvAdditionalInfo.Rows.Count;
                for (int i = 0; i < intCount; i++)
                {
                    for (int j = 0; j < intCount2; j++)
                    {
                        if (txtCategoryCode.Text.Trim() == m_lstDeficientInfosEdit[i].CategoryCode &&
                            dgvAdditionalInfo.Rows[j].Cells[0].Value.ToString() == m_lstDeficientInfosEdit[i].Key)
                        {
                            dgvAdditionalInfo.Rows[j].Cells[1].Value = m_lstDeficientInfosEdit[i].Value;
                            dgvAdditionalInfo.Rows[j].Cells[2].Value = m_lstDeficientInfosEdit[i].IsRequired;
                            dgvAdditionalInfo.Rows[j].DefaultCellStyle.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }


    }
}