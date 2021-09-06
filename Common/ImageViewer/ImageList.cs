
// RMC 20111207 added hiding of controls in ImageList
// RMC 20111207 changed TRN to BIN, deleted query of year in docblob_tbl

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.DynamicProgressBar;
using System.Threading;

namespace Amellar.Common.ImageViewer
{
    /// <summary>
    /// This image list is used for build-up
    /// </summary>
    public partial class frmImageList : Form
    {
        // RMC 20111206 Added attachment of blob image (s)
        private string m_sSource = string.Empty;

        public string Source
        {
            get { return m_sSource; }
            set { m_sSource = value; }
        }
        // RMC 20111206 Added attachment of blob image (e)

        //for aministrator only 
        public string CustomBlobQuery
        {
            set { m_strCustomBlobQuery = value; }
        }
        private string m_strCustomBlobQuery;


        private bool m_isBuildUp;
        /// <summary>
        /// default is false
        /// </summary>
        public bool IsBuildUp
        {
            get { return m_isBuildUp; }
            set { m_isBuildUp = value; }
        }


        private bool m_isBuildUpPosting;
        /// <summary>
        /// default is false
        /// </summary>
        public bool IsBuildUpPosting
        {
            get { return m_isBuildUpPosting; }
            set { m_isBuildUpPosting = value; }
        }

       
        /// <summary>
        /// default is false
        /// </summary>
        


        private ImageInfo m_objImageInfo;

        /// <summary>
        /// Image informartion to display in the image list
        /// </summary>
        public ImageInfo ImageInformation
        {
            get { return m_objImageInfo; }
            set { m_objImageInfo = value; }
        }

        public bool IsAutoDisplay
        {
            get { return m_blnIsAutoDisplay; }
            set { m_blnIsAutoDisplay = value; }
        }
        private bool m_blnIsAutoDisplay;

        public bool isFortagging
        {
            get { return m_blnIsForTagging; }
            set { m_blnIsForTagging = value; }
        }
        private bool m_blnIsForTagging;

        private bool m_blnIsViewOtherImages = false;
        /// <summary>
        /// AST 20150408 Added This property
        /// </summary>
        public bool IsViewOtherImages
        {
            get { return m_blnIsViewOtherImages; }
            set { m_blnIsViewOtherImages = value; }
        }

        public bool ValidateImage(string strTRN, string strSystem)
        {
            try
            {
                OracleResultSet result = new OracleResultSet();
                result.CreateBlobConnection();
                /*result.CreateNewInstance(0);
            
                if (!result.isConnected)
                    return false;*/

                // AST 20150415 remove condition as gob request (s)
                // RMC 20150226 adjustment in blob configuration (s)
                //if (AppSettingsManager.GetBlobImageConfig() == "F")
                //    result.Query = string.Format("select count(*) from docblob_twopage where bin = '{0}' and sys_type = '{1}'", strTRN, strSystem);
                //else// RMC 20150226 adjustment in blob configuration (e)
                    result.Query = string.Format("select count(*) from docblob_tbl where bin = '{0}' and sys_type = '{1}'", strTRN, strSystem);
                // AST 20150415 remove condition as gob request (e)

                    int intCount = 0;
                    int.TryParse(result.ExecuteScalar(), out intCount);  

                    // RMC 20150529 Corrections in viewing attached image (s)
                    int intCount2 = 0;             
                    result.Query = string.Format("select count(*) from docblob_twopage where bin = '{0}' and sys_type = '{1}'", strTRN, strSystem);
                    int.TryParse(result.ExecuteScalar(), out intCount2);

                    intCount += intCount2;
                    // RMC 20150529 Corrections in viewing attached image (e)

                     

                    

                result.Close();
                return (intCount != 0);
            }
            catch
            {
                return false;
            }
        }

        
        public void setImageInfo(ImageInfo objImageInfo)
        {            
            m_objImageInfo = objImageInfo;
        }
        



        private bool m_blnIsAdjusting;


        frmImageViewer m_frmImage;
        frmDatabaseInfo m_frmDatabaseInfo;

        public frmImageList()
        {
            m_intImageID = 0;
            m_strImageFileName = string.Empty;

            m_strImageTRN = string.Empty;

            m_isBuildUp = false;
            m_isBuildUpPosting = false;           


            m_blnIsAdjusting = true;

            InitializeComponent();
            InitializeDataGrid();

            this.getAllYear();

            m_frmImage = new frmImageViewer();
            m_frmImage.FormClosing += new FormClosingEventHandler(m_frmImage_FormClosing);

            m_frmDatabaseInfo = new frmDatabaseInfo();
            

            m_blnIsAutoDisplay = false;
            m_blnIsForTagging = false;


            m_strCustomBlobQuery = string.Empty; //for adminstrator only
        }


        private void InitializeDataGrid()
        {
            dgvImageList.Columns.Clear();

            dgvImageList.Columns.Add("colId", "Id");
            dgvImageList.Columns[0].Visible = false;

            dgvImageList.Columns.Add("colFileName", "FileName");
            dgvImageList.Columns[1].ReadOnly = true;
            dgvImageList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;            
            
            dgvImageList.Columns.Add(new DataGridViewCheckBoxColumn());
            dgvImageList.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvImageList.Columns[2].HeaderText = "Pending";
            dgvImageList.Columns[2].MinimumWidth = 60;
            dgvImageList.Columns[2].Width = 60;
            dgvImageList.Columns[2].ReadOnly = true;

            dgvImageList.Columns.Add(new DataGridViewCheckBoxColumn());
            dgvImageList.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvImageList.Columns[3].HeaderText = "Reject";
            dgvImageList.Columns[3].MinimumWidth = 60;
            dgvImageList.Columns[3].Width = 60;
            dgvImageList.Columns[3].ReadOnly = true;

        
        }        

        private void getAllYear()
        {
            /*cmbYear.Items.Clear();
            cmbYear.Items.Add("All");

            OracleResultSet result = new OracleResultSet();
            result.CreateBlobConnection();
            //result.CreateNewInstance(0);
            result.Query = "SELECT DISTINCT year FROM docblob_tbl WHERE year IS NOT NULL AND sys_type = 'A' ORDER BY year";
            if (result.Execute())
            {
                while (result.Read())
                {
                    cmbYear.Items.Add(result.GetInt(0));
                }
            }
            cmbYear.SelectedIndex = 0;
            result.Close();*/
        }     

        frmProgress m_form; //
        private Thread m_objThread;
        public delegate void DifferentDelegate(object value, frmProgress.ProgressMode mode);
        public static void DoSomethingDifferent(object value, frmProgress.ProgressMode mode, DifferentDelegate threadFunction)
        {
            threadFunction(value, mode); // NOTE: invoked with a parameter
        }

        private void ThreadProcess()
        {
            using (m_form = new frmProgress())
            {
                
                m_form.TopMost = true;
                
                m_form.ShowDialog();
            }
        }


        private void ListImages()
        {
            try
            {
                // RMC 20150226 adjustment in blob configuration (s)
                string sBlobConfig = string.Empty;
                sBlobConfig =AppSettingsManager.GetBlobImageConfig();
                // RMC 20150226 adjustment in blob configuration (e)

                m_objThread = new Thread(this.ThreadProcess);
                m_objThread.Start();

                Thread.Sleep(500);

                if (m_blnIsAdjusting)
                    return;
                if (dgvImageList.Columns == null || dgvImageList.Columns.Count == 0)
                    return;

                m_blnIsAdjusting = true;
                //m_blnIsCheckAll = true;
                //btnCheckAll.Text = "&Select All";

                bool blnIsPending = chkPending.Checked;
                bool blnIsRejected = chkRejected.Checked;
                int intYear = cmbYear.SelectedIndex;


                OracleResultSet result = new OracleResultSet();
                result.CreateBlobConnection();
                /*result.CreateNewInstance(0);
                if (!result.isConnected)
                    return;*/


                StringBuilder strConditionQuery = new StringBuilder();

                if (blnIsPending || blnIsRejected)
                {
                    strConditionQuery.Append(" AND ( ");

                    if (blnIsPending)
                    {
                        strConditionQuery.Append(" ispending = 1 ");
                        if (blnIsRejected)
                            strConditionQuery.Append(" OR ");
                    }
                    if (blnIsRejected)
                    {
                        strConditionQuery.Append(" isrejected = 1 ");
                    }

                    strConditionQuery.Append(" ) ");
                }
                else
                {
                    strConditionQuery.Append(" AND ( ");
                    strConditionQuery.Append(" ispending <> 1 AND ");
                    strConditionQuery.Append(" isrejected <> 1 ");
                    strConditionQuery.Append(" ) ");
                }

                //insert condition for viewing specific
                if (m_objImageInfo.UserCode != null && m_objImageInfo.UserCode != "")
                {
                    strConditionQuery.Append(string.Format(" AND ENCODER = '{0}' ", m_objImageInfo.UserCode));
                }

                if (m_objImageInfo.TRN != null && m_objImageInfo.TRN != "")
                {
                    strConditionQuery.Append(string.Format(" AND PIN = '{0}' ", m_objImageInfo.TRN));
                }
                else
                {
                    strConditionQuery.Append(" AND (PIN IS NULL and deficient is null)");
                }

                if (m_objImageInfo.System != null && m_objImageInfo.System != "")
                {
                    strConditionQuery.Append(string.Format(" AND SYS_TYPE = '{0}' ", m_objImageInfo.System));
                }

                if (intYear > 0)
                    strConditionQuery.Append(string.Format(" AND year = '{0}'", cmbYear.Items[intYear]));

                int intCount = 0;
                int intCountIncreament = 0;
                StringBuilder strQuery = new StringBuilder();

                strQuery.Length = 0;
                // AST 20150415 remove for Gob request (s)
                // RMC 20150226 adjustment in blob configuration (s)
                //if (sBlobConfig == "F")
                //    strQuery.Append("SELECT COUNT(*) FROM docblob_twopage WHERE doc_type = 'F' and sys_type = '" + m_objImageInfo.System + "'");
                //else // RMC 20150226 adjustment in blob configuration (e)
                // AST 20150415 remove for Gob request (e)

                strQuery.Append("SELECT COUNT(*) FROM docblob_tbl WHERE doc_type = 'F' and sys_type = '" + m_objImageInfo.System + "'");
                strQuery.Append(strConditionQuery);


                if (string.IsNullOrEmpty(m_strCustomBlobQuery))
                    result.Query = strQuery.ToString();
                else
                {
                    result.Query = string.Format("select count(*) {0}", m_strCustomBlobQuery);
                }


                if (result.Execute())
                {
                    if (result.Read())
                    {
                        intCount = result.GetInt(0);
                    }
                }

                lblCount.Text = string.Format("There are {0} record(s) found.", intCount);


                DoSomethingDifferent(intCount, frmProgress.ProgressMode.StartProgressMode, m_form.ProgressBarWork);

                strQuery.Length = 0;
                strQuery.Append("SELECT id, file_nm, status, encoder, ispending, isrejected FROM (");

                // AST 20150415 remove for Gob request (s)
                // RMC 20150226 adjustment in blob configuration (s)
                //if (sBlobConfig == "F")
                //    strQuery.Append(" SELECT rownum as row_num,  x.* FROM (SELECT id, file_nm, doc_extn, file_path, status, encoder, ispending, isrejected  FROM docblob_twopage WHERE doc_type = 'F' and sys_type = '" + m_objImageInfo.System + "'");
                //else// RMC 20150226 adjustment in blob configuration (e)
                // AST 20150415 remove for Gob request (s)
                strQuery.Append(" SELECT rownum as row_num,  x.* FROM (SELECT id, file_nm, doc_extn, file_path, status, encoder, ispending, isrejected  FROM docblob_tbl WHERE doc_type = 'F' and sys_type = '" + m_objImageInfo.System + "'");
                strQuery.Append(strConditionQuery.ToString());

                strQuery.Append(" ORDER BY id) x) y");

                if (string.IsNullOrEmpty(m_strCustomBlobQuery))
                    result.Query = strQuery.ToString();
                else
                {
                    result.Query = string.Format("SELECT id, file_nm, status, encoder, ispending, isrejected FROM ( SELECT rownum as row_num,  x.* FROM (SELECT id, file_nm, doc_extn, file_path, status, encoder, ispending, isrejected {0} ORDER BY id) x) y ", m_strCustomBlobQuery);
                }

                if (m_sSource == "VIEW")
                {
                    result.Query = string.Format("select id, file_nm, status, encoder, ispending, isrejected from docblob_twopage where bin = '{0}' ", m_objImageInfo.TRN);
                    //result.Query += "union all ";
                    result.Query += "union ";   // RMC 20150529 Corrections in viewing attached image
                    result.Query += string.Format("select id, file_nm, status, encoder, ispending, isrejected from docblob_tbl where bin = '{0}' ", m_objImageInfo.TRN);
                    result.Query += "order by id, file_nm, status, encoder, ispending, isrejected";
                }

                dgvImageList.Rows.Clear();
                if (result.Execute())
                {
                    while (result.Read())
                    {

                        DoSomethingDifferent(intCountIncreament, frmProgress.ProgressMode.UpdateProgressMode, m_form.ProgressBarWork);
                        intCountIncreament += 1;

                        bool blnIsPendingTmp = false;
                        bool blnIsRejectedTmp = false;

                        if (result.GetInt("ispending") == 1)
                            blnIsPendingTmp = true;
                        if (result.GetInt("isrejected") == 1)
                            blnIsRejectedTmp = true;

                        dgvImageList.Rows.Add(result.GetInt("id"),
                            result.GetString("file_nm").Trim(), blnIsPendingTmp, blnIsRejectedTmp);

                        DoSomethingDifferent(string.Format("{0}/{1}", intCountIncreament, intCount), frmProgress.ProgressMode.LabelMode, m_form.ProgressBarWork);
                        Thread.Sleep(3);
                    }
                }
                result.Close();
                dgvImageList.Invalidate();

                DoSomethingDifferent("", frmProgress.ProgressMode.EndProgressMode, m_form.ProgressBarWork);
                Thread.Sleep(10);
            }
            catch { }
        }

        private void frmImageList_Load(object sender, EventArgs e)
        {            
            if (m_blnIsAutoDisplay)
            {
                m_blnIsAdjusting = false;
                //this.ListImages(); AST 20150408 remove

                // AST 20150408 Added This Block (s)
                if (m_blnIsViewOtherImages)
                    this.ViewOtherImages(this.Text);
                else
                    this.ListImages();
                // AST 20150408 Added This Block (e)
            }
            
            if (!m_blnIsForTagging)
            {                
                btnPending.Enabled = false;
                btnReject.Enabled = false;
            }
            else
            {                  
                btnPending.Enabled = true;
                btnReject.Enabled = true;
            }

            //this.grpTag.Enabled = m_blnIsForTagging;

            // RMC 20111207 added hiding of controls in ImageList (s)
            if (m_isBuildUp)
            {
                label2.Visible = true;
                groupBox2.Visible = true;
                cmbYear.Visible = false;
                chkPending.Visible = true;
                chkRejected.Visible = true;
                btnDisplay.Visible = true;
                btnPending.Visible = true;
                btnReject.Visible = true;
                grpTag.Visible = true;
                if (m_sSource == "ATTACH")
                    btnBrowse.Visible = true;
                else
                    btnBrowse.Visible = false;
            }
            else
            {
                label2.Visible = false;
                groupBox2.Visible = false;
                cmbYear.Visible = false;
                chkPending.Visible = false;
                chkRejected.Visible = false;
                btnDisplay.Visible = false;
                btnPending.Visible = false;
                btnReject.Visible = false;
                grpTag.Visible = false;
                if (m_sSource == "ATTACH")
                    btnBrowse.Visible = true;
                else
                    btnBrowse.Visible = false;
            }
            // RMC 20111207 added hiding of controls in ImageList (e)

            if(!string.IsNullOrEmpty(m_strCustomBlobQuery)) //for adminstrator only
            {
                grpTag.Enabled = false;
            }
        }    

        private void dgvImageList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvImageList.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }


        private void btnDisplay_Click(object sender, EventArgs e)
        {
           
            m_blnIsAdjusting = false;

            this.ListImages();
           
        }
        


        

        private void btnView_Click(object sender, EventArgs e)
        {
            m_intImageID = 0;
            m_strImageFileName = string.Empty;
            
            m_strImageTRN = string.Empty;

            if (dgvImageList.SelectedRows.Count != 0)
            {
                int intYear = 0;
                int intId = -1;
                if (int.TryParse(dgvImageList.SelectedRows[0].Cells[0].Value.ToString(), out intId) && intId != 0)
                {
                    if (!m_frmImage.IsDisposed)
                    {
                        m_frmImage.Close();
                        m_frmImage.Dispose();

                    }
                    if (!m_frmDatabaseInfo.IsDisposed)
                    {
                        m_frmDatabaseInfo.Close();
                        m_frmDatabaseInfo.Dispose();
                    }



                    m_frmImage = new frmImageViewer();
                    m_frmImage.FormClosing += new FormClosingEventHandler(m_frmImage_FormClosing);
                    m_frmDatabaseInfo = new frmDatabaseInfo();
                   

                    StringBuilder strQuery = new StringBuilder();
                    //strQuery.Append(string.Format("SELECT doc_file, file_nm, year FROM docblob_tbl WHERE id = {0}", intId));

                    // AST 20150415 remove condition as gob request (s)
                    // RMC 20150226 adjustment in blob configuration (s)
                    //if (AppSettingsManager.GetBlobImageConfig() == "F")
                    //    strQuery.Append(string.Format("SELECT doc_file, file_nm FROM docblob_twopage WHERE id = {0}", intId));
                    //else // RMC 20150226 adjustment in blob configuration (e)
                    //{
                    if (m_blnIsViewOtherImages) // AST 20150408
                        strQuery.Append(string.Format("SELECT doc_file, file_nm FROM docblob_twopage WHERE id = {0}", intId));
                    else
                    {
                        string strFileNm = string.Empty;
                        if (m_sSource == "VIEW")
                        {
                            strFileNm = dgvImageList.CurrentRow.Cells[1].Value.ToString();

                            if (strFileNm.Contains("BPLS ASSESSMENT - APPLICATION"))
                                strQuery.Append(string.Format("SELECT doc_file, file_nm FROM docblob_twopage WHERE id = {0}", intId));
                            else
                                strQuery.Append(string.Format("SELECT doc_file, file_nm FROM docblob_tbl WHERE id = {0}", intId));
                        }
                        else
                            strQuery.Append(string.Format("SELECT doc_file, file_nm FROM docblob_tbl WHERE id = {0}", intId));
                    }
                    //}
                        // AST 20150415 remove condition as gob request (e)

                    ImageAppSettings.IsViewOtherImages = m_blnIsViewOtherImages; // AST 20150408

                    // AST 20150416 
                    //for (int i = 1; i < ImageAppSettings.getScannedImagePerPage(); i++)
                    //{
                        //strQuery.Append(string.Format(" OR id = '{0}'", intId + i));
                        strQuery.Append(string.Format(" OR id = '{0}'", intId));
                    //}

                    strQuery.Append(" ORDER BY id");

                    m_frmImage.LoadImageQuery = strQuery.ToString();

                    /*OracleResultSet result = new OracleResultSet();
                    result.CreateBlobConnection();
                   // result.CreateNewInstance(0);
                   // if (result.isConnected)
                    {
                        result.Query = strQuery.ToString();
                        if (result.Execute())
                        {
                            if (result.Read())
                            {
                                intYear = result.GetInt("year");
                                //AppSettingsManager.Year = intYear;
                                
                            }
                        }
                    }
                    result.Close();*/

                    m_frmImage.TopMost = !(m_isBuildUpPosting); // Really? why not equal
                    m_frmImage.TopMost = true;

                    m_frmImage.setImageInfo(new ImageInfo(intId));
                    //m_frmImage.ImageInformation.System = "A";
                    m_frmImage.ImageInformation.System = AppSettingsManager.GetSystemType; //MCR 20141209
                    //m_frmImage.Show(this); // AST 20150319 Remove
                    m_frmImage.Show();  // AST 20150319 Use Void Parameter to remain open
                    

                    if (m_isBuildUpPosting)
                    {
                        //this.WindowState = FormWindowState.Minimized;   
                        m_intImageID = intId;
                        m_strImageFileName = m_frmImage.Text;

                    }                    
                    else
                    {
                        //this.WindowState = FormWindowState.Minimized;
                        m_intImageID = intId;  //JVL20100115
                        m_strImageFileName = m_frmImage.Text; //JVL20100115

                        
                        if (m_blnIsForTagging)
                        {
                            // m_frmDatabaseInfo.TopMost = true;
                            m_frmDatabaseInfo.setImageInfo(new ImageInfo(intId));
                            //m_frmDatabaseInfo.ImageInformation.System = "A";
                            m_frmDatabaseInfo.ImageInformation.System = AppSettingsManager.GetSystemType; //MCR 20141209
                            m_frmDatabaseInfo.Text = dgvImageList.SelectedRows[0].Cells[1].Value.ToString().Trim();
                            
                            m_frmDatabaseInfo.Show();
                        }
                        else
                        {
                            m_frmDatabaseInfo.Close(); //JVL201013 addcondition
                            m_frmDatabaseInfo.Dispose(); //JVL201013 addcondition
                        }
                        
                    }


                }
            }


        }

        private void frmImageList_FormClosing(object sender, FormClosingEventArgs e)
        {
            // AST 20150312 Prompt "Do you want to continue adding records without an image?" in Business Records Add(s)
            if (string.IsNullOrEmpty(m_strImageFileName.Trim()))
            {
                if (MessageBox.Show("Do you want to continue adding records without an image?", this.Text,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!m_frmImage.IsDisposed)
                    {
                        m_frmImage.Close();
                        m_frmImage.Dispose();
                    }

                    if (!m_frmDatabaseInfo.IsDisposed)
                    {
                        m_frmDatabaseInfo.Close();
                        m_frmDatabaseInfo.Dispose();
                    }

                    if (m_form != null && !m_form.IsDisposed)
                        m_form.Dispose();

                    m_intImageID = 0;
                    m_strImageFileName = "";

                    MessageBox.Show("No IMAGE attached to this record, please close the dialog and select an image from the list.", this.Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else 
                {
                    e.Cancel = true;
                    return;
                }
            }
            // AST 20150312 Prompt "Do you want to continue adding records without an image?" in Business Records Add(e)
        }

        private void frmImageList_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        private bool IsValid()
        {
            string strTRN = string.Empty;

            if (m_frmImage.IsDisposed || !m_frmImage.Visible)
            {
                MessageBox.Show(this, "Image Viewer is closed, please dont close when tagging");
                return false;
            }

            

            /*if (!m_isBuildUpPosting)
            {
                if (m_frmDatabaseInfo.IsDisposed || !m_frmDatabaseInfo.Visible)
                {
                    MessageBox.Show(this, "Database Information is close, please dont close when tagging");
                    return false;
                }
                if (m_frmDatabaseInfo.ImageInformation.ID != m_frmImage.ImageInformation.ID)
                {
                    MessageBox.Show(this, "Unique ID is diffent please choose another image");
                    return false;
                }

                strTRN = m_frmDatabaseInfo.ImageInformation.TRN;
            }
            else
            {
                strTRN = m_strImageTRN;
                if (m_intImageID != m_frmImage.ImageInformation.ID)
                {
                    MessageBox.Show(this, "Unique ID is diffent please choose another image");
                    return false;
                }



            }




            if (strTRN == null)
            {
                MessageBox.Show(this, "No TRN is selected.");
                return false;
            }
            else if (strTRN == "")
            {
                MessageBox.Show(this, "No TRN is selected.");
                return false;
            }
            else
            {
                if (m_isBuildUpPosting)
                {                    
                    int intCount = 0;
                    OracleResultSet result = new OracleResultSet();
                    result.Query = string.Format("select count(*) from tricycle_unit where trn = '{0}'", strTRN.Trim());
                    int.TryParse(result.ExecuteScalar(), out intCount);
                    if (intCount == 0)
                    {
                        MessageBox.Show(this, "TRN not exist.");
                        return false;
                    }                    
                }
            }*/
            return true;
        }       

        public int GetRecentImageID
        {
            get { return m_intImageID; }
        }
        private int m_intImageID;
        public string GetRecentImageFileNameDisplay
        {
            get { return m_strImageFileName; }
        }

        private string m_strImageFileName;

        public string GetRecentImagePin
        {
            get { return m_strImageTRN; }
            set { m_strImageTRN = value; }
        }

        private string m_strImageTRN;

        //JVL20091209(s)
        //add a variable and need to be put in config for future purposes
        bool m_blnIsPostAllPayment = false; //default is false mean posting only the latest payment
        //JVL20091209(e)

        public bool UpdateBlobImage(ImageInfo objImageInfo)
        {
            m_strImageTRN = objImageInfo.TRN;

            if (IsValid())
            {

                if (!m_blnIsPostAllPayment || MessageBox.Show(this, string.Format("Do you want to attach the image {0} to the pin {1}", objImageInfo.ImageName, m_strImageTRN), "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    objImageInfo.ID = m_intImageID;
                    ImageTransation objTransaction = new ImageTransation();
                    if (!(objTransaction.UpdateImage(objImageInfo)))
                    {
                        MessageBox.Show(this, "Image failed to tag as match.");
                    }
                    else
                    {
                        m_frmImage.Close();
                        m_frmImage.Dispose();

                        m_frmDatabaseInfo.Dispose();

                        btnDisplay_Click(null, null);

                        return true;
                    }
                }

            }

            return false;
        }

        private void m_frmInformation_FormClosing(object sender, EventArgs e)
        {
            /*int intID = 0;
            string strSysType = "C";

            if (m_isBuildUpPosting || m_isBuildUpPYD)
            {
                intID = m_intImageID;
                m_strImagePin = m_frmInformation.Pin; //JVL20091215
            }
            else
            {
                intID = m_frmDatabaseInfo.ImageInformation.ID;
                strSysType = m_frmDatabaseInfo.ImageInformation.System;
            }

            if (IsValid())
            {

                using (Amellar.RPTA.Classes.DeficientRecords.frmInformationPopUp form = (Amellar.RPTA.Classes.DeficientRecords.frmInformationPopUp)sender)
                {
                    if (form.isValid)
                    {
                        if (form.tagType == Amellar.RPTA.Classes.DeficientRecords.frmInformationPopUp.TagType.UnMatch)
                        {

                            if (MessageBox.Show(this, string.Format("Do you really want to tag unmatch this image name {0}?", m_frmImage.Text), "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {

                                ImageTransation objTransaction = new ImageTransation();
                                if (!(objTransaction.UpdateImage(intID, AppSettingsManager.SystemUser.UserCode, strSysType, form)))
                                {
                                    MessageBox.Show(this, "Image failed to tag as unmatch.");
                                }
                                else
                                {
                                    m_frmImage.Close();
                                    m_frmImage.Dispose();
                                    m_frmDatabaseInfo.Dispose();
                                    btnDisplay_Click(null, null);
                                }

                            }

                        }
                        else if (form.tagType == Amellar.RPTA.Classes.DeficientRecords.frmInformationPopUp.TagType.Cancelled)
                        {
                            if (MessageBox.Show(this, string.Format("Do you really want to tag cancelled this image name {0}?", m_frmImage.Text), "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {

                                ImageTransation objTransaction = new ImageTransation();
                                if (!(objTransaction.UpdateImage(intID, AppSettingsManager.SystemUser.UserCode, strSysType, "CANCEL", form)))
                                {
                                    MessageBox.Show(this, "Image failed to tag as cancelled.");
                                }
                                else
                                {
                                    m_frmImage.Close();
                                    m_frmImage.Dispose();
                                    m_frmDatabaseInfo.Dispose();
                                    btnDisplay_Click(null, null);
                                }

                            }
                        }
                        else if (form.tagType == Amellar.RPTA.Classes.DeficientRecords.frmInformationPopUp.TagType.Pending)
                        {
                            if (MessageBox.Show(this, string.Format("Do you really want to tag pending this image name {0}?", m_frmImage.Text), "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {

                                ImageTransation objTransaction = new ImageTransation();
                                if (!(objTransaction.UpdateImage(intID, AppSettingsManager.SystemUser.UserCode, strSysType, "PENDING", form)))
                                {
                                    MessageBox.Show(this, "Image failed to tag as pending.");
                                }
                                else
                                {
                                    m_frmImage.Close();
                                    m_frmImage.Dispose();
                                    m_frmDatabaseInfo.Dispose();
                                    btnDisplay_Click(null, null);
                                }

                            }

                        }
                        else if (form.tagType == Amellar.RPTA.Classes.DeficientRecords.frmInformationPopUp.TagType.Rejected)
                        {
                            if (MessageBox.Show(this, string.Format("Do you really want to tag rejected this image name {0}?", m_frmImage.Text), "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {

                                ImageTransation objTransaction = new ImageTransation();
                                if (!(objTransaction.UpdateImage(intID, AppSettingsManager.SystemUser.UserCode, strSysType, "REJECTED", form)))
                                {
                                    MessageBox.Show(this, "Image failed to tag as rejected.");
                                }
                                else
                                {
                                    m_frmImage.Close();
                                    m_frmImage.Dispose();
                                    m_frmDatabaseInfo.Dispose();
                                    btnDisplay_Click(null, null);
                                }

                            }
                        }





                    }


                }





                
            }
            m_frmInformation.tagType = new Amellar.RPTA.Classes.DeficientRecords.frmInformationPopUp.TagType();   //JVL20091215
            m_frmInformation.Dispose();
            grpTag.Enabled = true;
            this.Update();
           */
        }             

        private void dgvImageList_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.btnView_Click(null, null);
        }        

        private void btnPending_Click(object sender, EventArgs e)
        {            
            if (IsValid())
            {

                if (MessageBox.Show(this, string.Format("Do you really want to tag pending this image name {0}?", m_frmImage.Text), "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    ImageTransation objTransaction = new ImageTransation();
                    if (!(objTransaction.UpdateImage(m_frmImage.ImageInformation.ID, AppSettingsManager.SystemUser.UserCode, m_frmImage.ImageInformation.System, "PENDING")))
                    {
                        MessageBox.Show(this, "Image failed to tag as pending.");
                    }
                    else
                    {
                        m_frmImage.Dispose();
                        m_frmDatabaseInfo.Dispose();
                        btnDisplay_Click(null, null);
                    }

                }

            }             
        }

        private void btnReject_Click(object sender, EventArgs e)
        {            
            if (IsValid())
            {

                if (MessageBox.Show(this, string.Format("Do you really want to tag rejected this image name {0}?", m_frmImage.Text), "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    ImageTransation objTransaction = new ImageTransation();
                    if (!(objTransaction.UpdateImage(m_frmImage.ImageInformation.ID, AppSettingsManager.SystemUser.UserCode, m_frmImage.ImageInformation.System, "REJECTED")))
                    {
                        MessageBox.Show(this, "Image failed to tag as rejected.");
                    }
                    else
                    {
                        m_frmImage.Dispose();
                        m_frmDatabaseInfo.Dispose();
                        btnDisplay_Click(null, null);
                    }

                }

            }             
        }

        private void frmImageList_Activated(object sender, EventArgs e)
        {
            this.Update();            
        }      
       
        private void m_frmImage_FormClosing(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // RMC 20111207 added hiding of controls in ImageList
            m_frmImage = new frmImageViewer();

            using (FileDialog fileDlg = new OpenFileDialog())
            {
                ImageOperations operations = new ImageOperations();

                fileDlg.InitialDirectory = operations.InitialDirectory;
                fileDlg.Filter = "Image File (*.jpg;*.bmp;*.gif,*.tif)|*.jpg;*.bmp;*.gif;*.tif";

                if (fileDlg.ShowDialog() == DialogResult.OK)
                {
                    m_frmImage.LocalFileDirectory = fileDlg.FileName;
                    string sTmp = System.IO.Path.GetFileName(fileDlg.FileName);
                    m_frmImage.Text = sTmp;
                    //m_frmImage.ImageInformation.System = "A";
                    m_frmImage.ImageInformation.System = AppSettingsManager.GetSystemType; //MCR 20141209
                    m_frmImage.Show();
                    m_strImageFileName = fileDlg.FileName;
                }
            }
            //m_frmImage.picImageViewer.Focus();
        }

        /// <summary>
        /// AST 20150316 Added This Button and Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to use this image?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                btnView.PerformClick(); // This is to put the value of image to public variable, and to be use in records module
                
                this.Close();
            }
        }

        /// <summary>
        /// AST 20150408 Added This Method
        /// </summary>
        public void ViewOtherImages(string BIN)
        {
            OracleResultSet result = new OracleResultSet();
            result.CreateBlobConnection();
            bool blnIsPendingTmp = false;
            bool blnIsRejectedTmp = false;

            result.Query = string.Format("SELECT * FROM Docblob_Twopage WHERE Bin = '{0}' and sys_type = 'A' and ispending <> 1 and isrejected <> 1 and doc_type = 'F' ", BIN);
            if (result.Execute())
            {
                dgvImageList.Rows.Clear();
                while (result.Read())
                {
                    if (result.GetInt("ispending") == 1)
                        blnIsPendingTmp = true;
                    if (result.GetInt("isrejected") == 1)
                        blnIsRejectedTmp = true;

                    dgvImageList.Rows.Add(result.GetInt("id"),
                        result.GetString("file_nm").Trim(), blnIsPendingTmp, blnIsRejectedTmp);
                }
            }
            result.Close();
        }
        
    }
}