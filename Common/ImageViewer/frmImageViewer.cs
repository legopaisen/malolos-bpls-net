using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Amellar.Common.DataConnector;
//using Common.CommandLine.Utility;
using System.Runtime.InteropServices;

namespace Amellar.Common.ImageViewer
{
    public partial class frmImageViewer : Form
    {
        private ImageOperations operations;
        private int iHeight; // GDE 20090618
        int iScreenWidth = Screen.PrimaryScreen.Bounds.Width;
        int iScreenHeight = Screen.PrimaryScreen.Bounds.Height;

        private ImageInfo m_objImageInfo;
        public ImageInfo ImageInformation
        {
            get { return m_objImageInfo; }
        }

        public void setImageInfo(ImageInfo objImageInfo)
        {
            m_objImageInfo = objImageInfo;
        }
        // RMC 20111206 Added attachment of blob image (s)
        private string m_sLocalFileDirectory = "";
        public string LocalFileDirectory
        {
            get { return m_sLocalFileDirectory; }
            set { m_sLocalFileDirectory = value; }
        }
        // RMC 20111206 Added attachment of blob image (E)

        public frmImageViewer()
        {
            InitializeComponent();
            operations = new ImageOperations(picImageViewer);
            // GDE 20090618
            this.Location = new Point(-5, -5);
            this.StartPosition = FormStartPosition.Manual;
            this.KeyPreview = true;
            // GDE 20090618

            m_objImageInfo = new ImageInfo();
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FileDialog fileDlg = new OpenFileDialog())
            {
                fileDlg.InitialDirectory = operations.InitialDirectory;
                fileDlg.Filter = "Image File (*.jpg;*.bmp;*.gif,*.tif)|*.jpg;*.bmp;*.gif;*.tif";

                if (fileDlg.ShowDialog() == DialogResult.OK)
                {
                    operations.GetLocalFile(fileDlg.FileName, picImageViewer.ImageIndex, true);

                    if (picImageViewer.ImageCount != 0)
                    {
                        picImageViewer.ViewImage(picImageViewer.ImageIndex);
                        this.Text = picImageViewer.Names[picImageViewer.ImageIndex];
                    }
                }
            }
            picImageViewer.Focus();
        }

        private void chkPan_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPan.Checked)
            {
                chkZoom.Checked = false;
                picImageViewer.View = PictureBoxCtrl.ViewMode.PanMode;
            }
            else
                chkZoom.Checked = true;

            picImageViewer.Focus();
        }

        private void chkZoom_CheckedChanged(object sender, EventArgs e)
        {
            if (chkZoom.Checked)
            {
                chkPan.Checked = false;
                picImageViewer.View = PictureBoxCtrl.ViewMode.ZoomMode;
            }
            else
            {
                chkPan.Checked = true;
            }
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            picImageViewer.ZoomOut();
            picImageViewer.Focus();
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            picImageViewer.ZoomIn();
            ZoomPlus();
            picImageViewer.Focus();
           
        }

        private void frmImageViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (operations.HasConnectionString)
                DataConnectorManager.Instance.CloseConnection();
        }


        private void frmImageViewer_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.ShowAlways = true;


            chkPan.Checked = true;
            toolTip.SetToolTip(chkPan, "Pan");
            chkZoom.Checked = false;
            toolTip.SetToolTip(chkZoom, "Zoom");

            toolTip.SetToolTip(btnZoomIn, "Zoom In");
            toolTip.SetToolTip(btnZoomOut, "Zoom Out");
            toolTip.SetToolTip(btnBrowse, "Browse");
            toolTip.SetToolTip(btnPrev, "Previous");
            toolTip.SetToolTip(btnNext, "Next");
            toolTip.SetToolTip(btnSave, "Save");
            toolTip.SetToolTip(btnClose, "Close");

            this.UpdateNavigation();

            if (operations.Width != 0)
                this.Width = operations.Width;
            if (operations.Height != 0)
                this.Height = operations.Height;

            if (!operations.IsSavingAllowed)
            {
                btnSave.Enabled = false;
                btnSave.Visible = false;
            }

            if (!operations.IsBrowseable)
            {
                btnBrowse.Enabled = false;
                btnBrowse.Visible = false;
            }

            if (picImageViewer.ImageCount != 0)
            {
                picImageViewer.ViewImage(picImageViewer.ImageIndex);
                this.Text = picImageViewer.Names[picImageViewer.ImageIndex];
            }
            else
            {
                // RMC 20111206 Added attachment of blob image (s)
                if (m_sLocalFileDirectory != "")
                {
                    picImageViewer.InitializeForm();
                    operations.GetLocalFile(m_sLocalFileDirectory, picImageViewer.ImageIndex, true);
                    
                    if (picImageViewer.ImageCount != 0)
                    {
                        picImageViewer.ViewImage(picImageViewer.ImageIndex);
                        
                        //this.Text = picImageViewer.Names[picImageViewer.ImageIndex];
                        //this.Text = m_sLocalFileDirectory;
                    }
                }
                // RMC 20111206 Added attachment of blob image (e)
            }


            //button1.Visible = true;
            //button2.Visible = true;

            //   button1_Click(null, null);    //JVL20091209
        }
        public string LoadImageQuery
        {
            set
            {
                picImageViewer.LoadImageQuery(value);
            }
        }

        

        private void UpdateNavigation()
        {
            if (picImageViewer.ImageCount == 0)
            {
                lblImageList.Text = "0 of 0";
                btnPrev.Enabled = false;
                btnNext.Enabled = false;
            }
            else
            {
                lblImageList.Text = string.Format("{0} of {1}", picImageViewer.ImageIndex + 1, picImageViewer.ImageCount);
                btnPrev.Enabled = (picImageViewer.ImageIndex != 0);
                btnNext.Enabled = ((picImageViewer.ImageIndex + 1) != picImageViewer.ImageCount);

                picImageViewer.ViewImage(picImageViewer.ImageIndex);
                this.Text = picImageViewer.Names[picImageViewer.ImageIndex];
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (picImageViewer.ImageIndex != 0 && picImageViewer.ImageCount != 0)
            {
                picImageViewer.ImageIndex--;
                this.UpdateNavigation();
            }
            picImageViewer.Focus();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if ((picImageViewer.ImageIndex + 1) != picImageViewer.ImageCount && picImageViewer.ImageCount != 0)
            {
                picImageViewer.ImageIndex++;
                this.UpdateNavigation();
            }
            picImageViewer.Focus();
            //MessageBox.Show(picImageViewer.ImageIndex.ToString());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (picImageViewer.HasNullImage)
            {
                MessageBox.Show("Please set all images.", "Image Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!operations.InsertQueries())
            {
                MessageBox.Show("Failed to insert/update blob image.", "Image Viewer",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("Image(s) sucessfully inserted/updated.", "Image Viewer",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            picImageViewer.LeftRotate();
            picImageViewer.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            picImageViewer.RightRotate();
            picImageViewer.Focus();
        }

        private void picImageViewer_KeyDown(object sender, KeyEventArgs e)
        {
            
            char chKey = (char)e.KeyValue;
            string strKey = chKey.ToString();
            if (strKey.ToUpper() == "L")
                picImageViewer.LeftRotate();
            else if (strKey.ToUpper() == "R")
                picImageViewer.RightRotate();
            else if (e.KeyValue == 107)
            {
                picImageViewer.ZoomIn();
                ZoomPlus();
            }
            else if (e.KeyValue == 109)
            {
                picImageViewer.ZoomOut();
                int ih = 0, iw = 0;
                //if (picImageViewer.iWidth >= 600 && this.Width < iScreenWidth)
                if (picImageViewer.iWidth < this.Width && this.Width > 670)
                {
                    this.Width = picImageViewer.iWidth + 20;
                    picImageViewer.Width = this.Width - 20;
                    picImageViewer.AutoScroll = true;

                }

                if (picImageViewer.iHeight < this.Height && this.Height > 200)
                {
                    this.Height = picImageViewer.iHeight + 80;
                    picImageViewer.Height = this.Height - 80;
                    picImageViewer.AutoScroll = true;
                }

            }
            else if (e.KeyValue == 32)
            {
                if ((picImageViewer.ImageIndex + 1) != picImageViewer.ImageCount && picImageViewer.ImageCount != 0)
                {
                    picImageViewer.ImageIndex++;
                    this.UpdateNavigation();
                }
                else
                {
                    if (picImageViewer.ImageIndex != 0 && picImageViewer.ImageCount != 0)
                    {
                        picImageViewer.ImageIndex--;
                        this.UpdateNavigation();
                    }
                }
            }

            else if (e.KeyValue == 27)
                this.Close();
            else if (e.KeyValue == 70)
                this.picImageViewer.FitToPage();
            else if (e.KeyValue == 33)
            {
                SetupScrollBar(true);
                picImageViewer.Focus();
            }
            else if (e.KeyValue == 34)
            {
                SetupScrollBar(false);
                picImageViewer.Focus();
            }
        }
        protected override bool ProcessKeyPreview(ref System.Windows.Forms.Message m)
        {
            switch (m.WParam.ToInt32())
            {
                case 37: // <--- left arrow.
                    {
                        SetupMinVScrollBar(true);
                        picImageViewer.Focus();
                    }
                    
                    break;
                case 38: // <--- up arrow.
                    {
                        SetupMinScrollBar(true);
                        picImageViewer.Focus();
                    }
                    break;
                case 39: // <--- right arrow.
                    {
                        SetupMinVScrollBar(false);
                        picImageViewer.Focus();

                    }
                   
                    break;
                case 40: // <--- down arrow.
                    {
                        SetupMinScrollBar(false);
                        picImageViewer.Focus();
                    }                    
                    break;
            }
            return false;
        }

        private void SetupScrollBar(bool bFlag)
        {
            picImageViewer.MoveScroll(bFlag);
        }
        private void SetupMinScrollBar(bool bFlag)
        {
            picImageViewer.MoveMinScroll(bFlag);
            picImageViewer.Focus();
        }
        private void SetupMinVScrollBar(bool bFlag)
        {
            picImageViewer.MoveMinVScroll(bFlag);
            picImageViewer.Focus();
        }

        private void frmImageViewer_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void chkPan_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void picImageViewer_DragDrop(object sender, DragEventArgs e)
        {
        }

        public void ZoomPlus()
        {
            int iSh2 = 0, iSw2 = 0;
            if (picImageViewer.iHeight >= 100 && picImageViewer.iHeight < iScreenHeight)
                this.Height = picImageViewer.iHeight + 80;
            if (picImageViewer.iWidth >= 670 && this.Width < iScreenWidth)
                this.Width = picImageViewer.iWidth + 20;

            if (this.Height > iScreenHeight && iSh2 == 0)
            {
                this.Height = iScreenHeight - 40;
                iSh2 = 1;
                picImageViewer.Height = this.Height - 80;
                picImageViewer.AutoScroll = true;
            }
            if (this.Width > iScreenWidth)
            {
                this.Width = iScreenWidth;
                picImageViewer.Width = iScreenWidth - 10;
                picImageViewer.AutoScroll = true;
            }
        }


        public bool IsSavingAllowed
        {
            get { return operations.IsSavingAllowed; }
            set { operations.IsSavingAllowed = value; }
        }

        private void picImageViewer_Load(object sender, EventArgs e)
        {
        }

        private void chkPan_Enter(object sender, EventArgs e)
        {
            
        }

        
        
       

    }
}