using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.DataGridViewUtilities;


namespace Amellar.Common.DeficientRecords
{
    /// <summary>
    /// This class is needed for tag / untag of deficient property
    /// </summary>
    public partial class frmDeficientRecords : Form
    {

        public bool CancelDeficient
        {
            get { return m_blnIsCancelDeficient; }
        }
        private bool m_blnIsCancelDeficient;

        public List<string> AutoTagQuery
        {
            get { return m_lstAutoTag; }
            set { m_lstAutoTag = value; }
        }
        private List<string> m_lstAutoTag;
        /// <summary>
        /// index 0 string[]     = {user_name", DateTime} 
        ///       1 string       = DeficientType Selected
        ///       2 bool         = Status
        ///       3 List<string[]> = DagagridViewInfoRows
        ///       4 string       = txtremerks.text
        ///       5 string       = txtDir.text
        /// </summary>
        List<object[]> m_lstDeficientInfo;



        private DataGridViewTextBoxColumn m_txtUserColumn;
        private DataGridViewTextBoxColumn m_txtDateTimeColumn;
        private DataGridViewComboBoxColumn m_cmbDeficientTypeColumn;
        private DataGridViewCheckBoxColumn m_chkStatusColumn;

        private DataGridViewTextBoxColumn m_txtAdditionalColumn;
        private DataGridViewTextBoxColumn m_txtAdditionalValueColumn;

        private Color m_defaultCellColor;

        DataGridViewComboBoxCell m_comboBoxCell;

        private List<DataGridViewRow> m_lstRows;
        List<string[]> m_arrLstRowAdditionalInfo;

        private List<string[]> m_lstDeficientType;



        private Color m_HighlightColor;
        //private string m_sysType = "C";
        private string m_sysType = AppSettingsManager.GetSystemType; //MCR 20141209

        private int m_intStateOrdering;
        private int m_intSelectedIndex;
        private string m_strUserCode;

        public string ImageDirectory
        {
            get { return m_strImageDir; }
            set { m_strImageDir = value; }
        }
        private string m_strImageDir;

        public string ORNumber
        {
            get { return m_strORNum.Trim().ToUpper(); }
            set { m_strORNum = value; }
        }
        private string m_strORNum;

        public string BIN
        {
            get { return m_strPin; }
            set { m_strPin = value; }
        }
        private string m_strPin;





        public frmDeficientRecords()
        {
            //test
          //  m_strPin = "172-10-024-024-047";

            InitializeComponent();
            m_intSelectedIndex = 0;
            m_strUserCode = AppSettingsManager.SystemUser.UserCode;

            m_lstDeficientInfo = new List<object[]>();

            m_txtUserColumn = new DataGridViewTextBoxColumn();
            m_txtDateTimeColumn = new DataGridViewTextBoxColumn();
            m_cmbDeficientTypeColumn = new DataGridViewComboBoxColumn();
            m_chkStatusColumn = new DataGridViewCheckBoxColumn();

            m_txtAdditionalColumn = new DataGridViewTextBoxColumn();
            m_txtAdditionalValueColumn = new DataGridViewTextBoxColumn();

            m_arrLstRowAdditionalInfo = new List<string[]>();




            m_lstRows = new List<DataGridViewRow>();
            m_intStateOrdering = 0;

            m_blnIsCancelDeficient = true;


            m_lstAutoTag = new List<string>(); //JVL
        }

        private void InitializedDataGridView()
        {

             m_txtUserColumn.HeaderText = "User Name";
             m_txtDateTimeColumn.HeaderText = "Date Time";
             m_cmbDeficientTypeColumn.HeaderText = "Deficient Types";
             m_chkStatusColumn.HeaderText = "Corrected";

             m_txtUserColumn.Name = "DeficientType";


             m_txtAdditionalColumn.HeaderText = "Additional Information";
             m_txtAdditionalValueColumn.HeaderText = "Value";


             m_txtUserColumn.ReadOnly = true;
             m_txtDateTimeColumn.ReadOnly = true;

            dgvDeficientRecord.Columns.Add(m_txtUserColumn);
            dgvDeficientRecord.Columns.Add(m_txtDateTimeColumn);

            dgvDeficientRecord.Columns.Add(m_cmbDeficientTypeColumn);
            dgvDeficientRecord.Columns.Add(m_chkStatusColumn);


            dgvAdditionalInfo.Columns.Add(m_txtAdditionalColumn);
            dgvAdditionalInfo.Columns.Add(m_txtAdditionalValueColumn);


            int intWidth = dgvDeficientRecord.Width / 4;
            dgvDeficientRecord.Columns[0].Width = intWidth;
            dgvDeficientRecord.Columns[1].Width = intWidth;
            dgvDeficientRecord.Columns[2].Width = dgvDeficientRecord.Width - ((intWidth * 2) + (intWidth / 2)) - 5;         
            dgvDeficientRecord.Columns[3].Width = intWidth / 2;


            intWidth = dgvAdditionalInfo.Width / 4;
            dgvAdditionalInfo.Columns[0].Width = intWidth * 3;
            dgvAdditionalInfo.Columns[1].Width = dgvAdditionalInfo.Width - (intWidth * 3) - 5;

            //m_defaultCellColor = dgvAdditionalInfo.DefaultCellStyle.BackColor;
            m_defaultCellColor = Color.White;
            m_HighlightColor = Color.LightGray;



           // InsertRowInGridView(new string[] { "test", "test", "NO PIN", "Y"});

            /*
            comboboxColumn.Width = dgvDeficientRecord.Columns[2].Width;
            dgvDeficientRecord.Columns.Remove("DeficientType");

            foreach (string[] strDeficientType in m_lstDeficientType.ToArray())
            {
                //split the display in the combo box for good display only
                comboboxColumn.Items.Add(strDeficientType[2]);
            }
            dgvDeficientRecord.Columns.Insert(2, comboboxColumn);
            */


        }


        private List<string[]> InitializeListDeficiencyType()
        {
            m_comboBoxCell = new DataGridViewComboBoxCell();
            m_lstDeficientType = new List<string[]>();
            m_lstDeficientType.Clear();

            OracleResultSet result = new OracleResultSet();
            result.Query = string.Format(@"select def_code, cat_code, def_name from def_rec_table
                                order by def_name", m_sysType);
//order by def_code, cat_code", m_sysType);
            if (result.Execute())
            {
                string[] strDefRecTable = {"", "", ""};
                m_cmbDeficientTypeColumn.Items.Add(strDefRecTable[2]);
                while (result.Read())
                {
                    strDefRecTable = new string[]{result.GetString(0).Trim(), result.GetString(1).Trim(), result.GetString(2).Trim()};
                    m_cmbDeficientTypeColumn.Items.Add(strDefRecTable[2]);
                    m_lstDeficientType.Add(strDefRecTable);
                }
            }
            return m_lstDeficientType;
        }


        private void InitializedGUI()
        { 
            //based on c++ codes

            bool blnHasNullOr = true;
            bool blnReserveExistingDeficient = true; //for transition data


            bool blnHasExistingDeficienct = false;
            string strQuery = string.Empty;
            DataGridView dgvInfoTemp = new DataGridView();


            OracleResultSet resultDelete3 = new OracleResultSet();
            resultDelete3.Query = string.Format("select count(*) from def_rec_adinfo_tmp where rec_acct_no = '{0}'", m_strPin);
            int intCount3 = 0;
            int.TryParse(resultDelete3.ExecuteScalar(), out intCount3);

            if (intCount3 != 0)
            {
                resultDelete3.Query = string.Format("delete def_rec_adinfo_tmp where rec_acct_no = '{0}'", m_strPin);
                resultDelete3.ExecuteNonQuery();

            }
            resultDelete3.Query = string.Format("select count(*) from def_rec_adinfo where rec_acct_no = '{0}'", m_strPin);
            intCount3 = 0;
            int.TryParse(resultDelete3.ExecuteScalar(), out intCount3);
            if (intCount3 != 0)
            {
                resultDelete3.Query = string.Format("insert into def_rec_adinfo_tmp select * from def_rec_adinfo where rec_acct_no = '{0}'", m_strPin);
                resultDelete3.ExecuteNonQuery();
            }
            resultDelete3.Close();


            if (m_strORNum == string.Empty || m_strORNum == null)
            {
                OracleResultSet resultDelete1 = new OracleResultSet();
                resultDelete1.Query = string.Format("select count(*) from def_records_tmp where rec_acct_no = '{0}'", m_strPin);
                int intCount1 = 0;
                int.TryParse(resultDelete1.ExecuteScalar(), out intCount1);

                if (intCount1 != 0)
                {
                    resultDelete1.Query = string.Format("delete def_records_tmp where rec_acct_no = '{0}'", m_strPin);
                    resultDelete1.ExecuteNonQuery();

                }

                resultDelete1.Query = string.Format("select count(*) from def_records where rec_acct_no = '{0}'", m_strPin);
                intCount1 = 0;
                int.TryParse(resultDelete1.ExecuteScalar(), out intCount1);

                if (intCount1 != 0)
                {
                    resultDelete1.Query = string.Format("insert into def_records_tmp select * from def_records where rec_acct_no = '{0}'", m_strPin);
                    resultDelete1.ExecuteNonQuery();
                }
                resultDelete1.Close();


                strQuery = string.Format("select * from def_records_tmp where rec_acct_no = '{0}' ", m_strPin);
            }
            else
            {
              


                OracleResultSet resultDelete2 = new OracleResultSet();
                resultDelete2.Query = string.Format("select count(*) from def_records_tmp where rec_acct_no = '{0}' and rec_acct_type = '{1}'", m_strPin, m_strORNum);
                int intCount2 = 0;
                int.TryParse(resultDelete2.ExecuteScalar(), out intCount2);

                if (intCount2 != 0)
                {
                    blnHasNullOr = false;

                    resultDelete2.Query = string.Format("delete def_records_tmp where rec_acct_no = '{0}' and rec_acct_type = '{1}' ", m_strPin, m_strORNum);
                    resultDelete2.ExecuteNonQuery();

                }

                resultDelete2.Query = string.Format("select count(*) from def_records where rec_acct_no = '{0}' and rec_acct_type = '{1}'", m_strPin, m_strORNum);
                intCount2 = 0;
                int.TryParse(resultDelete2.ExecuteScalar(), out intCount2);
                if (intCount2 != 0)
                {
                    blnHasNullOr = false;
                    resultDelete2.Query = string.Format("insert into def_records_tmp select * from def_records where rec_acct_no = '{0}' and rec_acct_type = '{1}'", m_strPin, m_strORNum);
                    resultDelete2.ExecuteNonQuery();
                }
                resultDelete2.Close();

                strQuery = string.Format("select * from def_records_tmp where rec_acct_no = '{0}' and rec_acct_type = '{1}'", m_strPin, m_strORNum);




                if (blnHasNullOr && !blnReserveExistingDeficient)
                {

                    resultDelete2.Query = string.Format("select count(*) from def_records_tmp where rec_acct_no = '{0}'", m_strPin);
                    int intCount1 = 0;
                    int.TryParse(resultDelete2.ExecuteScalar(), out intCount1);

                    if (intCount1 != 0)
                    {
                        resultDelete2.Query = string.Format("delete def_records_tmp where rec_acct_no = '{0}'", m_strPin);
                        resultDelete2.ExecuteNonQuery();

                    }

                    resultDelete2.Query = string.Format("select count(*) from def_records where rec_acct_no = '{0}' ", m_strPin);
                    intCount1 = 0;
                    int.TryParse(resultDelete2.ExecuteScalar(), out intCount1);

                    if (intCount1 != 0)
                    {
                        resultDelete2.Query = string.Format("insert into def_records_tmp select * from def_records where rec_acct_no = '{0}'", m_strPin);
                        resultDelete2.ExecuteNonQuery();
                    }
                    resultDelete2.Close();


                    strQuery = string.Format("select * from def_records_tmp where rec_acct_no = '{0}'", m_strPin);


                }






                /*
                OracleResultSet resultDisp = new OracleResultSet();
                int intCountJ = 0;
                resultDisp.Query = string.Format("select * from def_records_tmp where rec_acct_no = RPAD('{0}', 25) and sys_type = '{1}' and rec_acct_type = RPAD('{2}', 20)", m_strPin, m_sysType, m_strORNum);
                int.TryParse(resultDisp.ExecuteScalar(), out intCountJ);
                if (intCountJ != 0)
                {
                    strQuery = string.Format("select * from def_records_tmp where rec_acct_no = RPAD('{0}', 25) and sys_type = '{1}' and rec_acct_type = RPAD('{2}', 20)", m_strPin, m_sysType, m_strORNum);   
                }
                else
                    strQuery = string.Format("select * from def_records_tmp where rec_acct_no = RPAD('{0}', 25) and sys_type = '{1}'", m_strPin, m_sysType);


                resultDisp.Close();
                */

            }


            //here  need to set up the auto tagging

            
            




            OracleResultSet result = new OracleResultSet();

            foreach (string strQ in AutoTagQuery)
            {
                result.Query = strQ;
                result.ExecuteNonQuery();
            }


            result.Query = string.Format("select count(*) from ({0})", strQuery);
            int intCount = 0;
            int.TryParse(result.ExecuteScalar(), out intCount);

            // if found load the existing deficient
            if (intCount != 0)
            {



                blnHasExistingDeficienct = true;

                if (m_strORNum == string.Empty || m_strORNum == null)
                {

                    strQuery = string.Format(@"select user_code, dt_save, def_name, remarks, def_status, img_path from def_records_tmp, def_rec_table
                                where def_records_tmp.def_code = def_rec_table.def_code and rec_acct_no = '{0}'", m_strPin);
                }
                else
                {

                    strQuery = string.Format(@"select user_code, dt_save, def_name, remarks, def_status, img_path from def_records_tmp, def_rec_table
                                where def_records_tmp.def_code = def_rec_table.def_code and rec_acct_no = '{0}' and def_records_tmp.rec_acct_type = '{1}'", m_strPin,m_strORNum);

                    if (blnHasNullOr && !blnReserveExistingDeficient)
                    {
                        strQuery = string.Format(@"select user_code, dt_save, def_name, remarks, def_status, img_path from def_records_tmp, def_rec_table
                                where def_records_tmp.def_code = def_rec_table.def_code and rec_acct_no = '{0}'", m_strPin);                    
                    }

                }
            }

            if (blnHasExistingDeficienct)
            {








                result.Query = strQuery;
                if (result.Execute())
                {
                    while (result.Read())
                    {

                        string[] arrDeficientRows;
                        try
                        {
                            arrDeficientRows = new string[] { result.GetString("user_code").Trim(), result.GetString("dt_save").Trim()};
                            string strDeficientType = result.GetString("def_name").Trim();
                                                        
                            bool blnStatus = false;
                            if(result.GetString("def_status").Trim() == "Y")
                                blnStatus = true;




                            m_arrLstRowAdditionalInfo = new List<string[]>();

                            //string[] aa = new string[] { "d","s"};
                            
                            
                            //rowCollection.Add(aa);
                            //rowCollection.Add(aa);
                            
                            //dgvAdditionalInfo.Rows.AddRange(rowCollection[0]);
                            
                           // dgvInfoTemp = new DataGridView();
                           // dgvInfoTemp.Columns.Add(m_txtAdditionalColumn);
                           // dgvInfoTemp.Columns.Add(m_txtAdditionalValueColumn);


                            foreach (string[] arrDeficientType in m_lstDeficientType)
                            {
                                if (strDeficientType == arrDeficientType[2])
                                {
                                    
                                    foreach (string[] arrAddInfo in ListAdditionalInfo(arrDeficientType))
                                    {
                                        string strAdditionalInfoValue = (AdditionalInfoValues(m_strPin, arrDeficientType[0], arrDeficientType[1], arrAddInfo[0]));
                                        
                                        m_arrLstRowAdditionalInfo.Add(new string[] { arrAddInfo[1], strAdditionalInfoValue.Trim() });
                                            //dgvInfoTemp.Rows.Add(arrAdditionalInfoTemp);
                                    }
                                    break;
                                }

                            }



                            //, result.GetString("def_name"), result.GetString("def_status")};
                            //user_code, dt_save, def_name, remarks, def_status, img_path 

                            //txtImageDir.Text = result.GetString("img_path");
                            //txtRemarks.Text = result.GetString("remarks");
                           // string strRemarks


                            m_lstDeficientInfo.Add(new object[] { arrDeficientRows, strDeficientType, blnStatus, m_arrLstRowAdditionalInfo, result.GetString("remarks").Trim(), result.GetString("img_path").Trim() }); 




                        }
                        catch 
                        {
                            MessageBox.Show("check the count of lstDeficientInfo to the count of dgvDeficient");  
                        }

                        
                        
                    }

                }
            
            
            }


            // here it go to gui
            result.Close();

            LoadListInfo();
            


        }

        private void LoadExistingDeficient(List<object[]> lstObjInfo)
        {
            //m_lstDeficientInfo.Add(new object[] { arrDeficientRows, strDeficientType, blnStatus, m_arrLstRowAdditionalInfo, result.GetString("remarks"), result.GetString("img_path") });
            foreach (object[] arrObjInfo in lstObjInfo)
            { 
              // not yet implemented if and only if pin has existing deficient then it must be implemented immediately  
            
            }

        }


        private string AdditionalInfoValues(string strPin, string strDefCode, string strCatCode, string strAddInfoCode)
        {
          //  List<string[]> lstAdditionalInfo = new List<string[]>();

            OracleResultSet result = new OracleResultSet();
            string strQuery = string.Format("select adinfo_value from def_rec_adinfo_tmp where rec_acct_no = '{0}' and def_code = '{1}' and adinfo_code = '{2}' order by adinfo_code", strPin, strDefCode, strAddInfoCode);
            result.Query = strQuery;
            return result.ExecuteScalar();

            /*
            if (strAdditionalInfoValue != string.Empty)
            {

                arrAdditionalInfoRow = new string[] { strAddInfoTemp, strAddInfoValueTemp }; 
            }

            {
                result.Query = strQuery;

                string strAddInfoCodeTemp = string.Empty;
                string strAddInfoNameTemp = string.Empty;

                //DataGridViewRow dgvAdditionalInfoRowTemp = new DataGridView();
                string[] arrAdditionalInfoRow = new string[]{"", ""};

                if (result.Execute())
                {
                    if(result.Read())
                    {
                        strAddInfoCodeTemp = result.GetString("AdInfo_code").Trim();
                        strAddInfoNameTemp = result.GetString("AdInfo_name").Trim();

                        OracleResultSet resultx = new OracleResultSet();
                        resultx.Query = string.Format("select adinfo_value from def_rec_adinfo_tmp where rec_acct_no = RPAD('{0}', 25) and def_code = RPAD('{1}', 3) and adinfo_code = RPAD('{2}', 5)  and cat_code = RPAD('{3}', 3) and sys_type = '{4}' order by adinfo_code", strPin, strDefCode, strAddInfoCode, strCatCode, m_sysType);
                         resultx.ExecuteScalar();

                        arrAdditionalInfoRow = new string[]{ strAddInfoTemp, strAddInfoValueTemp };
                        lstAdditionalInfo.Add(arrAdditionalInfoRow);
                    }
                }

                return arrAdditionalInfoRow;
            }
            return null;
            */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arrRowsInfo">index of 0 = userName, 1 = DateTime, 2 = DeficientType in combo box, 3 = Value status in check box</param>
        /// <returns></returns>
        private bool InsertRowInGridView(string[] arrRowsInfo)
        {
            dgvDeficientRecord.Rows.Add(arrRowsInfo[0], arrRowsInfo[1]);

            m_intSelectedIndex = m_cmbDeficientTypeColumn.Items.IndexOf(arrRowsInfo[2]);

             //dgvDeficientRecord.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgvDeficientRecord_EditingControlShowing);

           // dgvDeficientRecord.Columns.

//            dgvDeficientRecord.Columns.Remove(2);
            //  dgvDeficientRecord.Columns.RemoveAt(3);

  //          dgvDeficientRecord.Columns.Add(m_cmbDeficientTypeColumn);
           // dgvDeficientRecord.Columns.Add(m_chkStatusColumn);
            
            return true;
        }



        private void frmDeficientRecords_Load(object sender, EventArgs e)
        {
            txtReferenceNo.Text = m_strPin;

            InitializeListDeficiencyType();
            InitializedDataGridView();
            InitializedGUI();

            //if (dgvDeficientRecord.Rows.Count <= 0)
            {
                dgvDeficientRecord.Rows.Add();                
            }

            //dgvDeficientRecord.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgvDeficientRecord_EditingControlShowing);
            btnSave.Enabled = (m_lstDeficientInfo.Count > 0); //JVL20100104
        }

        private List<string[]> ListAdditionalInfo(string[] arrDeficient)
        {
            List<string[]> lstAddInfo = new List<string[]>();

            OracleResultSet result = new OracleResultSet();
            result.Query = string.Format(@"select * from def_adinfo_table where def_code = :1
                                    order by def_code, adinfo_code", m_sysType);
            result.AddParameter(":1", arrDeficient[0]);
            if (result.Execute())
            {
                string[] strDefRecAdditionalInfo = { "", "", "" };
                while (result.Read())
                {
                    strDefRecAdditionalInfo = new string[] { result.GetString("adinfo_code").Trim(), result.GetString("adinfo_name").Trim() };
                    lstAddInfo.Add(strDefRecAdditionalInfo);
                }
            }

            return lstAddInfo;


        }

     
    

        private void dgvDeficientRecord_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                //InsertRowInGridView(string[] arrRowsInfo)
    
                // this condition is only for datagridview check box
                if (e.ColumnIndex == 3) //means the check box column is check or un check
                {
                    if (dgvDeficientRecord[3, e.RowIndex].Value == null || Convert.ToBoolean(dgvDeficientRecord[3, e.RowIndex].Value) == false)
                    {
                        dgvDeficientRecord[3, e.RowIndex].Value = Convert.ToBoolean(true);
                        m_lstDeficientInfo[e.RowIndex].SetValue(true, 2);
                    }
                    else
                    {
                        dgvDeficientRecord[3, e.RowIndex].Value = Convert.ToBoolean(false);
                        m_lstDeficientInfo[e.RowIndex].SetValue(false, 2);
                    }
                }
/*
                else if (e.ColumnIndex == 2)
                {
                    ValidateDeficientType(dgvDeficientRecord.CurrentRow.Cells[2].Value.ToString().Trim());
                }
*/

                
            }
            catch
            { 
            }
        }



        private List<string> GetDeficientTypeList()
        {
            List<string> lstDeficientType = new List<string>();
            foreach (object[] arrObjDeficientName in m_lstDeficientInfo)
            {
                lstDeficientType.Add(arrObjDeficientName.GetValue(1).ToString());
            }
            return lstDeficientType;
        }

        /// <summary>
        /// return false if has duplicate true if none
        /// </summary>
        /// <param name="strDeficientType"></param>
        /// <returns></returns>
        private bool ValidateDeficientType(string strDeficientType)
        {
            try
            {

                // m_lstDeficientInfo.IndexOf(

                string strTempDeficient = strDeficientType; //dgvDeficientRecord.CurrentCell.Value.ToString().Trim();

                if (strTempDeficient == null || strTempDeficient == "")
                {
                    return false;
                }
                else if (GetDeficientTypeList().IndexOf(strTempDeficient) == -1)
                {
                    m_intStateOrdering = 0;

                    foreach (string[] arrDeficientType in m_lstDeficientType)
                    {
                        if (strTempDeficient == arrDeficientType[2])
                        {
                            dgvAdditionalInfo.Rows.Clear();

                            foreach (string[] arrAddInfo in ListAdditionalInfo(arrDeficientType))
                            {

                                dgvAdditionalInfo.Rows.Add(arrAddInfo[1]);
                            }
                        }

                    }
                }
                else
                {


                    //dgvDeficientRecord.Rows.RemoveAt(dgvDeficientRecord.CurrentRow.Index);
                    MessageBox.Show("No Duplication of Entry.");

                    return false;
                }
            }

            catch
            {
                return false;
            }
            return true;

        }

        private void dgvDeficientRecord_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2)
                {

                    string strDeficientTemp = dgvDeficientRecord.CurrentRow.Cells[2].Value.ToString().Trim();
                    bool blnDeficientStatusTemp = Convert.ToBoolean(dgvDeficientRecord.CurrentRow.Cells[3].Value);


                    if (ValidateDeficientType(strDeficientTemp))
                    {

                        string[] arrTemp = { m_strUserCode, string.Format("{0:yyyy-MM-dd HH:mm:ss}", AppSettingsManager.GetCurrentDate()) };
                        dgvDeficientRecord.CurrentRow.SetValues(arrTemp);

                        m_lstDeficientInfo.Add(new object[] { arrTemp, strDeficientTemp, blnDeficientStatusTemp, GetDatagridViewRows(dgvAdditionalInfo), txtRemarks.Text, txtImageDir.Text });
                        dgvDeficientRecord.Rows[e.RowIndex].ReadOnly = true;
                        dgvDeficientRecord.Rows.Add();


                    }
                    else
                    {
                        // dgvDeficientRecord.Rows[e.RowIndex].ReadOnly = false;


                        //this is for tempory fix of validation(s)
                        DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)dgvDeficientRecord.Rows[e.RowIndex].Cells[2];
                        if (cell.Items.Count > 0)
                            cell.Value = cell.Items[0];
                        //(e)


                        dgvAdditionalInfo.Focus();

                    
                    }

                }


            }
          
            catch
            {
               
            }

            btnSave.Enabled = (m_lstDeficientInfo.Count > 0); //JVL20100104
        }




        private List<string[]> GetDatagridViewRows(DataGridView dgvTable)
        {
            List<string[]> lstStrRowsInfo = new List<string[]>();
            
            
            if (dgvTable.Rows.Count >= 1 || dgvTable.Rows.Count != null)
            {

                for(int i =0; i<= dgvTable.Rows.Count - 1; i++)
                {
                    lstStrRowsInfo.Add(new string[] { DataGridViewUtilities.DataGridViewUtilities.GetStringValue(dgvTable, i, 0), DataGridViewUtilities.DataGridViewUtilities.GetStringValue(dgvTable, i, 1) });
                }

            }
            return lstStrRowsInfo;
        }




        private void dgvDeficientRecord_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            if (e.Control is ComboBox)
            {


                ComboBox cbx = (ComboBox)e.Control;

                cbx.SelectedIndex = 0;//this.dgvDeficientRecord.CurrentCell.RowIndex % 3;

            }

        }


        private void txtRemarks_TextChanged(object sender, EventArgs e)
        {
            dgvDeficientRecord.CurrentRow.DefaultCellStyle.BackColor = m_HighlightColor;
        }

        private void txtRemarks_Leave(object sender, EventArgs e)
        {
            int intCurrentRow = dgvDeficientRecord.CurrentRow.Index;

            dgvDeficientRecord.CurrentRow.DefaultCellStyle.BackColor = m_defaultCellColor;

            if (intCurrentRow <= m_lstDeficientInfo.Count - 1)
            {
                m_lstDeficientInfo[intCurrentRow].SetValue(txtRemarks.Text, 4);
            }

        }

        private void txtImageDir_TextChanged(object sender, EventArgs e)
        {

            dgvDeficientRecord.CurrentRow.DefaultCellStyle.BackColor = m_HighlightColor; ;
            
        }

        private void txtImageDir_Leave(object sender, EventArgs e)
        {
            int intCurrentRow = dgvDeficientRecord.CurrentRow.Index;
            dgvDeficientRecord.CurrentRow.DefaultCellStyle.BackColor = m_defaultCellColor;
            if (intCurrentRow <= m_lstDeficientInfo.Count - 1)
            {

                m_lstDeficientInfo[intCurrentRow].SetValue(txtImageDir.Text, 5);
            }
        }

        private void dgvAdditionalInfo_Leave(object sender, EventArgs e)
        {
        }

        private void dgvAdditionalInfo_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            dgvDeficientRecord.CurrentRow.DefaultCellStyle.BackColor = m_HighlightColor;
        }




        private void LoadListByDeficientType()
        {
            int intCurrentRow = dgvDeficientRecord.CurrentRow.Index;

            dgvAdditionalInfo.Rows.Clear();
            txtImageDir.Text = "";
            txtRemarks.Text = "";



            if (intCurrentRow <= m_lstDeficientInfo.Count - 1)
            {
                foreach (string[] arrStrAdditionalInfo in ((List<string[]>)(m_lstDeficientInfo[intCurrentRow].GetValue(3))))
                {
                    dgvAdditionalInfo.Rows.Add(arrStrAdditionalInfo);
                }
                
                txtRemarks.Text = m_lstDeficientInfo[intCurrentRow].GetValue(4).ToString();
                txtImageDir.Text = m_lstDeficientInfo[intCurrentRow].GetValue(5).ToString();
            }


            dgvAdditionalInfo.Refresh();
        
        }


        private void LoadListInfo()
        { //m_lstDeficientInfo
            dgvDeficientRecord.Rows.Clear();

            /// <summary>
            /// index 0 string[]     = {user_name", DateTime} 
            ///       1 string       = DeficientType Selected
            ///       2 bool         = Status
            ///       3 List<string[]> = DagagridViewInfoRows
            ///       4 string       = txtremerks.text
            ///       5 string       = txtDir.text
            /// </summary>
            /// 
            for (int i = 0; i <= m_lstDeficientInfo.Count - 1; i++)
            {
                dgvDeficientRecord.Rows.Add(new object[] {((string[])m_lstDeficientInfo[i].GetValue(0))[0], ((string[])m_lstDeficientInfo[i].GetValue(0))[1], m_lstDeficientInfo[i].GetValue(1), Convert.ToBoolean(m_lstDeficientInfo[i].GetValue(2))});

                dgvDeficientRecord.Rows[i].ReadOnly = true;
            }
        }
        private void EditListInfo()
        {
        }
        private void AddListInfo()
        {
        }

        public void SaveListInfo(OracleResultSet result)
        {
            //OracleResultSet result = new OracleResultSet();
            //result.Transaction = true;

            string strQuery = string.Empty;
            int intCount = 0;
            //validate if doble entry
            if (m_strORNum == string.Empty || m_strORNum == null)
            {
                result.Query = string.Format("select count(*) from def_records_tmp where rec_acct_no = '{0}' ", m_strPin);
                intCount = 0;
                int.TryParse(result.ExecuteScalar(), out intCount);
                if (intCount != 0)
                {
                    result.Query = "delete from def_records_tmp where rec_acct_no = :1";
                    result.AddParameter(":1", m_strPin);
                    if (result.ExecuteNonQuery() == 0)
                    { 
                        
                    }
                }
            }
            else
            {
                result.Query = string.Format("select count(*) from def_records_tmp where rec_acct_no = '{0}' and rec_acct_type = '{1}'", m_strPin, m_strORNum);
                intCount = 0;
                int.TryParse(result.ExecuteScalar(), out intCount);
                if (intCount != 0)
                {
                    result.Query = "delete from def_records_tmp where rec_acct_no = :1 and rec_acct_type = :2)";
                    result.AddParameter(":1", m_strPin);
                    result.AddParameter(":2", m_strORNum);
                    result.ExecuteNonQuery();

                    if (m_lstDeficientInfo.Count == 0)
                    {
                        result.Query = string.Format("select count(*) from def_records where rec_acct_no = '{0}' and rec_acct_type = '{1}'", m_strPin, m_strORNum);
                        intCount = 0;
                        int.TryParse(result.ExecuteScalar(), out intCount);
                        if (intCount != 0)
                        {
                            result.Query = "delete from def_records where rec_acct_no = :1 and rec_acct_type = :2";
                            result.AddParameter(":1", m_strPin);
                            result.AddParameter(":2", m_strORNum);
                            result.ExecuteNonQuery();


                        }
                    }

                }
                else
                {
                    result.Query = string.Format("select count(*) from def_records_tmp where rec_acct_no = '{0}'", m_strPin);
                    intCount = 0;
                    int.TryParse(result.ExecuteScalar(), out intCount);
                    if (intCount != 0)
                    {
                        result.Query = "delete from def_records_tmp where rec_acct_no = :1 ";
                        result.AddParameter(":1", m_strPin);
                        if (result.ExecuteNonQuery() == 0)
                        {

                        }

                        if (m_lstDeficientInfo.Count == 0)
                        {
                            result.Query = string.Format("select count(*) from def_records where rec_acct_no = '{0}'", m_strPin);
                            intCount = 0;
                            int.TryParse(result.ExecuteScalar(), out intCount);
                            if (intCount != 0)
                            {
                                result.Query = "delete from def_records where rec_acct_no = :1";
                                result.AddParameter(":1", m_strPin);
                                if (result.ExecuteNonQuery() == 0)
                                {

                                }
                            }
                        }


                    }
  
                }
            }

            string strUser = AppSettingsManager.SystemUser.UserCode;
            DateTime dtCurrent = AppSettingsManager.GetCurrentDate();

//            foreach (string strTempDeficient in GetDeficientTypeList)
            foreach (object[] arrObjInfo in m_lstDeficientInfo)
            {
                string strTempDeficient = arrObjInfo.GetValue(1).ToString();

                foreach (string[] arrDeficientType in m_lstDeficientType)
                {
                    if (strTempDeficient == arrDeficientType[2])
                    {


                        //dgvAdditionalInfo.Rows.Add(arrAddInfo[1]);
                        //here comes the insertion

                             /// <summary>
            /// index 0 string[]     = {user_name", DateTime} 
            ///       1 string       = DeficientType Selected
            ///       2 bool         = Status
            ///       3 List<string[]> = DagagridViewInfoRows
            ///       4 string       = txtremerks.text
            ///       5 string       = txtDir.text
            /// </summary>
            /// 

                            result.Query = "insert into def_records_tmp values(:1, :2, :3, :4, :5, :6, :7, :8)";
                            result.AddParameter(":1", m_strORNum);
                            result.AddParameter(":2", m_strPin);
                            result.AddParameter(":3", arrDeficientType[0]);  //arrDeficientType[0] DEF_CODE

                            string strStatus = "N";
                            if(Convert.ToBoolean(arrObjInfo.GetValue(2)) == true)
                                strStatus = "Y";
                            
                            result.AddParameter(":4", strStatus);
                            result.AddParameter(":5", arrObjInfo.GetValue(4).ToString());
                            result.AddParameter(":6", arrObjInfo.GetValue(5).ToString());
                            
                            result.AddParameter(":7", strUser);
                            result.AddParameter(":8", dtCurrent);
                            
                            if (result.ExecuteNonQuery() == 0)
                            { 

                            }
                        /*
                            result.Query = string.Format("select count(*) from def_rec_adinfo_tmp where rec_acct_no = RPAD('{0}', 25) and sys_type = '{1}'", m_strPin, m_sysType);
                            intCount = 0;
                            int.TryParse(result.ExecuteScalar(), out intCount);
                            if (intCount != 0)
                            {
                                result.Query = "delete from def_rec_adinfo_tmp where rec_acct_no = RPAD(:1, 25) and sys_type = :2";
                                result.AddParameter(":1", m_strPin);
                                result.AddParameter(":2", m_sysType);
                                if (result.ExecuteNonQuery() == 0)
                                {

                                }
                            }*/

                            result.Query = string.Format("select count(*) from def_rec_adinfo_tmp where rec_acct_no = '{0}' and def_code = '{1}' ", m_strPin, arrDeficientType[0]);
                            intCount = 0;
                            int.TryParse(result.ExecuteScalar(), out intCount);
                            if (intCount != 0)
                            {
                                result.Query = "delete from def_rec_adinfo_tmp where rec_acct_no = :1 and def_code = :2 ";
                                result.AddParameter(":1", m_strPin);
                                result.AddParameter(":2", arrDeficientType[0].Trim());
                                if (result.ExecuteNonQuery() == 0)
                                { 
                                
                                }
                            }
                        
                        foreach (string[] arrStr in (List<string[]>)arrObjInfo.GetValue(3))
                        {
                            
                            string strAdditionalInfo = arrStr[0];

                            foreach (string[] arrAddInfo in ListAdditionalInfo(arrDeficientType))
                            {
                                if (strAdditionalInfo == arrAddInfo[1])
                                {
                                    result.Query = "insert into def_rec_adinfo_tmp values (:1, :2, :3, :4)";
                                    result.AddParameter(":1", m_strPin);
                                    result.AddParameter(":2", arrDeficientType[0]);//DEF_CODE
                                    result.AddParameter(":3", arrAddInfo[0]);
                                    result.AddParameter(":4", arrStr[1]);
                                    
                                    if (result.ExecuteNonQuery() == 0)
                                    { 
                                    }

                                    break;
                                }
                            }
                        }

                        /*
                        result.Query = string.Format("select count(*) from def_rec_adinfo where rec_acct_no = RPAD('{0}', 25)", m_strPin);
                        intCount = 0;
                        int.TryParse(result.ExecuteScalar(), out intCount);
                        if (intCount != 0)
                        {
                            result.Query = "delete from def_rec_adinfo where rec_acct_no = RPAD(:1, 25)";
                            result.AddParameter(":1", m_strPin);
                            //result.ExecuteNonQuery();
                            if (result.ExecuteNonQuery() != 0)
                            { 
                            }
                        }
                         */
                        /*
                        //need to validate this query if neccessary
                        result.Query = "insert into def_rec_adinfo select * from def_rec_adinfo where rec_acct_no = RPAD(:1, 25)";
                        result.AddParameter(":1", m_strPin);
                        
                        if (result.ExecuteNonQuery() == 0)
                        { 
                        }
                        //just to prevent 2 times insert*/
                        break;
                    }

                }
            }



            //save def_addlvalues in def_rec_adinfo_tmp table





        }

        private void dgvAdditionalInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int intCurrentRow = dgvDeficientRecord.CurrentRow.Index;
            dgvDeficientRecord.CurrentRow.DefaultCellStyle.BackColor = m_defaultCellColor;
            if (intCurrentRow <= m_lstDeficientInfo.Count - 1)
            {

                m_lstDeficientInfo[intCurrentRow].SetValue(GetDatagridViewRows(dgvAdditionalInfo), 3);
            }

        }

        private void dgvDeficientRecord_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            m_intStateOrdering = e.RowIndex;
            LoadListByDeficientType();
        }



        public void SaveDeficientInfo(OracleResultSet result)
        {
            bool blnExist = false;
            bool blnExistTransact = false;

            OracleResultSet resultx = new OracleResultSet();
//            OracleResultSet result = new OracleResultSet();
//            result.Transaction = true;

            string strQuery = string.Empty;
            string strQueryTransact = string.Empty;

            if (m_lstDeficientInfo.Count > 0)
            {

                if (m_strORNum == string.Empty || m_strORNum == null)
                {
                    strQuery = string.Format("select * from def_records where rec_acct_no = '{0}'", m_strPin.Trim());
                    strQueryTransact = string.Format("select * from def_records_tmp where rec_acct_no = '{0}' ", m_strPin.Trim());
                }
                else
                {
                    strQuery = string.Format("select * from def_records where rec_acct_no = '{0}' and rec_acct_type = '{1}'", m_strPin, m_strORNum);
                    strQueryTransact = string.Format("select * from def_records_tmp where rec_acct_no = '{0}' and rec_acct_type = '{1}'", m_strPin, m_strORNum);
                }

                resultx.Query = string.Format("select count(*) from ({0})", strQuery);
                int intCount = 0;
                int.TryParse(resultx.ExecuteScalar(), out intCount);
                if (intCount != 0)
                {
                    blnExist = true;
                }

                resultx.Query = string.Format("select count(*) from ({0})", strQueryTransact);
                intCount = 0;
                int.TryParse(resultx.ExecuteScalar(), out intCount);
                if (intCount != 0)
                {
                    blnExistTransact = true;
                }


                if (blnExistTransact)
                {
                   
                        resultx.Query = strQueryTransact;
             

                    if (resultx.Execute())
                    {
                        // RMC 20091211 Corrected double saving of entries (S)
                        if (m_strORNum == string.Empty || m_strORNum == null)
                        {
                            result.Query = string.Format("select count(*) from def_records where rec_acct_no = '{0}'", m_strPin);
                            intCount = 0;
                            int.TryParse(result.ExecuteScalar(), out intCount);
                            if (intCount != 0)
                            {
                                result.Query = string.Format("delete from def_records where rec_acct_no = '{0}'", m_strPin);
                                result.ExecuteNonQuery();
                            }
                        }   // RMC 20091211 Corrected double saving of entries (E)
                        else
                        {
                            result.Query = string.Format("select count(*) from def_records where rec_acct_no = '{0}' and rec_acct_type = '{1}'", m_strPin,m_strORNum);
                            //result.Query = string.Format("select count(*) from def_records where rec_acct_no = RPAD('{0}', 25) and sys_type = '{1}'", m_strPin, m_sysType); //JVL 10102008
                            intCount = 0;
                            int.TryParse(result.ExecuteScalar(), out intCount);
                            if (intCount != 0)
                            {
                                result.Query = string.Format("delete from def_records where rec_acct_no = '{0}' and rec_acct_type = '{1}'", m_strPin, m_strORNum);
                                //result.Query = string.Format("delete from def_records where rec_acct_no = RPAD('{0}', 25) and sys_type = '{1}'", m_strPin, m_sysType);
                                result.ExecuteNonQuery();
                            }
                            else
                            {
                                result.Query = string.Format("select count(*) from def_records where rec_acct_no = '{0}'", m_strPin);
                                intCount = 0;
                                int.TryParse(result.ExecuteScalar(), out intCount);
                                if (intCount != 0)
                                {
                                    result.Query = string.Format("delete from def_records where rec_acct_no = '{0}'", m_strPin);
                                    result.ExecuteNonQuery();
                                }
                                                        
                            }
                        }
                        string strEditBy = string.Empty;
                        strEditBy = AppSettingsManager.SystemUser.UserCode;
                        DateTime dtCurrent = AppSettingsManager.GetCurrentDate();


                        while (resultx.Read())
                        {
                            string strCatCode, strRecAcctType, strDefCode, strDefStatus, strRemarks, strImagePath, strUserCode, strDtSave = string.Empty;
                            //strCatCode = resultx.GetString("cat_code").Trim();
                            strRecAcctType = resultx.GetString("rec_acct_type").Trim();
                            strDefCode = resultx.GetString("def_code").Trim();
                            strDefStatus = resultx.GetString("def_status").Trim();
                            strRemarks = resultx.GetString("remarks").Trim();
                            strImagePath = resultx.GetString("img_path").Trim();
                            strUserCode = resultx.GetString("user_code").Trim();
                            DateTime dtSave;
                            dtSave = resultx.GetDateTime("dt_save");

                            result.Query = "insert into def_records values (:1 , :2, :3, :4, :5, :6, :7, :8)";
                            result.AddParameter(":1", strRecAcctType);
                            result.AddParameter(":2", m_strPin);
                            result.AddParameter(":3", strDefCode);
                            result.AddParameter(":4", strDefStatus);
                            result.AddParameter(":5", strRemarks);
                            result.AddParameter(":6", strImagePath);
                            result.AddParameter(":7", strUserCode);
                            result.AddParameter(":8", dtSave);
                            if (result.ExecuteNonQuery() == 0)
                            { 
                            }

                        }

                    }


                    string strAdinfoQuery = " def_rec_adinfo where rec_acct_no = :1";
                    result.Query = "select count(*) from def_rec_adinfo where rec_acct_no = :1";
                    result.AddParameter(":1", m_strPin);
                    int intCount2 = 0;
                    int.TryParse(result.ExecuteScalar(), out intCount2);
                    if (intCount2 != 0)
                    {
                        result.QueryText = "delete from def_rec_adinfo where rec_acct_no = :1 ";
                        result.ExecuteNonQuery();
                    }

                    resultx.QueryText = "select * from def_rec_adinfo_tmp where rec_acct_no = :1";
                    resultx.AddParameter(":1", m_strPin);
                    if (resultx.Execute())
                    {
                        while (resultx.Read())
                        {
                            // this will do if it has an existing entry with or wothout OR numner
                            // do some insertion here with the result Oracleresultset object
                            string strCatCode, strDefCode, strAdditionalInfoCode, strAdditionalInfoValue = string.Empty;

                            //strCatCode = resultx.GetString("cat_code").Trim();
                            strDefCode = resultx.GetString("def_code").Trim();
                            strAdditionalInfoCode = resultx.GetString("adinfo_code").Trim();
                            strAdditionalInfoValue = resultx.GetString("adinfo_value").Trim();

                            result.Query = "insert into def_rec_adinfo values(:1, :2, :3, :4)";
                            result.AddParameter(":1", m_strPin);
                            result.AddParameter(":2", strDefCode);
                            result.AddParameter(":3", strAdditionalInfoCode);
                            result.AddParameter(":4", strAdditionalInfoValue);
                            
                            if (result.ExecuteNonQuery() == 0)
                            { 
                            }
                        }
                    }
                }
                    /*
                else
                {
                    if (blnExist)
                    {
                        resultx.Query = string.Format("select count(*) from def_records where rec_acct_no = RPAD({0}, 25) and sys_type = '{2}' and rec_acct_type = RPAD(:3, 20)", m_strPin, m_sysType, m_strORNum);
                        intCount = 0;
                        int.TryParse(resultx.ExecuteScalar(), out intCount);
                        if (intCount != 0)
                        {
                            result.Query = "delete from def_records where rec_acct_no = RPAD(:1, 25) and sys_type = :2 and rec_acct_type = RPAD(:3, 20)";
                            result.AddParameter(":1", m_strPin);
                            result.AddParameter(":2", m_sysType);
                            result.AddParameter(":3", m_strORNum);
                            result.ExecuteNonQuery();
                        }
                    }
                }*/




                if (blnExist)
                {
                    CorrectedRecord(m_strPin, m_strORNum, result);
                }
            }
        }


        private void CorrectedRecord(string strPin, string m_sRefOrNo, OracleResultSet result)
        {
            OracleResultSet resultx = new OracleResultSet();
            string strQuery, strDefStatus, strRecAcctType, strDefCode, strUserCode, strDtSave, strRemarks, strImagePath = string.Empty;
            strDtSave = string.Format("{0:yyyy-MM-dd HH:mm:ss}", AppSettingsManager.GetCurrentDate());
            strUserCode = AppSettingsManager.SystemUser.UserCode;


            if (m_strORNum == string.Empty || m_strORNum == null)
            {
                resultx.Query = "select * from def_records where rec_acct_no = :1 and rec_acct_type is null";
                resultx.AddParameter(":1", m_strPin);
                
            }
            else
            {
                resultx.Query = "select * from def_records where rec_acct_no = :1 and rec_acct_type = :2";
                resultx.AddParameter(":1", m_strPin);
                resultx.AddParameter(":2", m_strORNum);
           
            }

            if (resultx.Execute())
            {
                while (resultx.Read())
                {
                    strRecAcctType = resultx.GetString("rec_acct_type").Trim();
                    strDefStatus = resultx.GetString("def_status").Trim();
                    strDefCode = resultx.GetString("def_code").Trim();
                    strRemarks = resultx.GetString("remarks").Trim();
                    strImagePath = resultx.GetString("img_path").Trim();



                    if (strDefStatus == "Y")
                    {
                        DateTime dtCurrent = AppSettingsManager.GetCurrentDate();   //RMC 20091215 corrected error 'literal does not match' in inserting date

                        result.Query = "insert into corrected_records values(:1, :2, :3, :4, :5, :6, :7, :8, :9)";
                        result.AddParameter(":1", strRecAcctType);
                        result.AddParameter(":2", m_strPin);
                        result.AddParameter(":3", strDefCode);
                        result.AddParameter(":4", strDefStatus);
                        result.AddParameter(":5", strRemarks);
                        result.AddParameter(":6", strImagePath);
                        result.AddParameter(":7", ""); // this is always empty based on C++ codes
                        result.AddParameter(":8", strUserCode);
                    //    result.AddParameter(":9", strDtSave);
                        result.AddParameter(":9", dtCurrent);   //RMC 20091215 corrected error 'literal does not match' in inserting date
                      
                        //result.ExecuteNonQuery();
                        if (result.ExecuteNonQuery() == 0)
                        { 
                        }

                    }

                }
            }


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvDeficientRecord.Rows.Count > 0)
            {
                m_blnIsCancelDeficient = false;
                
                OracleResultSet result = new OracleResultSet(); //RMC 20091211             
                this.SaveListInfo(result);    //RMC 20091211
                this.SaveDeficientInfo(result);   //RMC 20091211
                this.Close();
                
            }
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.Description = "Select the Directory of Images";
            folderDlg.SelectedPath = @"c:\";
            folderDlg.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_blnIsCancelDeficient = true;
            this.Close();
        }

        private void btnDelete_MouseHover(object sender, EventArgs e)
        {
            dgvDeficientRecord.CurrentRow.DefaultCellStyle.BackColor = m_HighlightColor;
        }

        private void btnDelete_MouseLeave(object sender, EventArgs e)
        {
            dgvDeficientRecord.CurrentRow.DefaultCellStyle.BackColor = m_defaultCellColor;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int intCurrentRow = dgvDeficientRecord.CurrentRow.Index;

            if (intCurrentRow <= m_lstDeficientInfo.Count - 1)
            {
                dgvDeficientRecord.Rows.RemoveAt(intCurrentRow);
                m_lstDeficientInfo.RemoveAt(intCurrentRow);
            }
            //btnSave.Enabled = (m_lstDeficientInfo.Count > 0); //JVL20100104
        }



    }
}