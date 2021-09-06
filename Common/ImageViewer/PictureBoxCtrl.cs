using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Common.ImageViewer
{
    public partial class PictureBoxCtrl : UserControl
    {
        [System.Runtime.InteropServices.DllImport("icsetcursor.dll")]
        private extern static long FindICWindow(IntPtr ParentWindow, string szCursorName);
        int iScreenWidth = Screen.PrimaryScreen.Bounds.Width;
        int iScreenHeight = Screen.PrimaryScreen.Bounds.Height;
        int iMaxSize = 0;

        private double m_dblMagnificationScale = 1.25;
        private int m_intMagnificationMinMaxStep = 5;

        public int iWidth = 0, iHeight = 0, yy = 0, xx = 0;

        //private Point m_pntScroll; // GDE 20090625
        public Point m_pntScroll;
        private bool m_blnIsScrolling = false;


        public enum ViewMode
        {
            PanMode,
            ZoomMode,
        }

        private ViewMode m_enuViewMode;

        private List<Image> m_lstIconImages;
        private List<String> m_lstIconNames;
        
        private List<String> m_lstIconPaths;
        private List<String> m_lstIconExtensions;

        private int m_intIconIndex;

        /*
        //file paths (not yet implemented)
        private List<string> m_lstFilePaths;
        private List<string> m_lstFileNames;
        private List<string> m_lstFileExtensions;
        */

        public PictureBoxCtrl()
        {
            InitializeComponent();
            m_enuViewMode = ViewMode.PanMode;

            m_lstIconImages = new List<Image>();
            m_lstIconNames = new List<String>();

            m_lstIconPaths = new List<String>();
            m_lstIconExtensions = new List<String>();

            m_intIconIndex = 0;
//            yy = picImage.Height;
  //          xx = picImage.Width;
            /*
            m_lstFilePaths = new List<string>();
            m_lstFileNames = new List<string>();
            m_lstFileExtensions = new List<string>();
            */
        }

        public double MagnificationScale
        {
            get { return m_dblMagnificationScale; }
            set { m_dblMagnificationScale = value; }
        }

        public int MagnificationMinMaxStep
        {
            get { return m_intMagnificationMinMaxStep; }
            set { m_intMagnificationMinMaxStep = value; }
        }

        public ViewMode View
        {
            get { return m_enuViewMode; }
            set 
            {
                m_enuViewMode = value; 
            }
        }

        public Image Image
        {
            get 
            { 
                return picImage.Image; 
            }
            set 
            {
                if (picImage.Image != null)
                {
                    picImage.Image = null;
                }

                
                picImage.SizeMode = PictureBoxSizeMode.Normal; // GDE 20090819
                //picImage.SizeMode = PictureBoxSizeMode.CenterImage;
                picImage.Image = value;
                    if (value != null)
                    {
                        picImage.Size = ((Image)value).Size;
                    }
                   this.FitToPage();

            }
        }

        public string ImagePath
        {
            set
            {
                string strImagePath = value;

            }
        }

        public List<Image> Images
        {
            get { return m_lstIconImages; }
            set { m_lstIconImages = value; }
        }
        
        public List<String> Names
        {
            get { return m_lstIconNames; }
            set { m_lstIconNames = value; }
        }

        public List<String> Paths
        {
            get { return m_lstIconPaths; }
            set { m_lstIconPaths = value; }
        }

        public List<String> Extensions
        {
            get { return m_lstIconExtensions; }
            set { m_lstIconExtensions = value; }
        }

        public bool HasNullImage
        {
            get
            {
                for (int i = 0; i < m_lstIconImages.Count; i++)
                {
                    if (m_lstIconImages[i] == null)
                        return true;
                }
                return false;
            }
        }

        public int ImageCount
        {
            get { return m_lstIconImages.Count; }
            set
            {
                int intValue = (int)value;
                if (m_lstIconImages.Count > intValue)
                {
                    for (int j = m_lstIconImages.Count - 1; j > intValue; j--)
                    {
                        m_lstIconImages.RemoveAt(j);

                        m_lstIconNames.RemoveAt(j);
                        m_lstIconPaths.RemoveAt(j);
                        m_lstIconExtensions.RemoveAt(j);
                    }
                }
                for (int j = m_lstIconImages.Count; j < intValue; j++)
                {
                    m_lstIconImages.Add(null);

                    m_lstIconNames.Add(string.Empty);
                    m_lstIconPaths.Add(string.Empty);
                    m_lstIconExtensions.Add(string.Empty);

                }
            }
        }

        public bool ViewImage(int intIndex)
        {
            if (intIndex < 0 || intIndex >= this.ImageCount)
            {
                return false;
            }
            m_intIconIndex = intIndex;
            this.Image = m_lstIconImages[m_intIconIndex];
            return true;
        }

        public int ImageIndex
        {
            get { return m_intIconIndex; }
            set 
            { 
                m_intIconIndex = value; 
            }
        }

        public bool InsertImageQuery(List<string> strQueries)
        {
            if (m_lstIconImages.Count != strQueries.Count)
                return false;
            try
            {
                OracleResultSet result = new OracleResultSet();
                result.CreateBlobConnection();
                /*result.CreateNewInstance(0);
            
                if (!result.isConnected)
                    return false;*/

                result.Transaction = true;

                bool blnIsInsert = true;
                int intSequence = 0;

                //checks if query is insert
                for (int i = 0; i < strQueries.Count; i++)
                {
                    if (!strQueries[i].ToUpper().Trim().StartsWith("INSERT"))
                    {
                        blnIsInsert = false;
                        break;
                    }
                }
                if (blnIsInsert)
                {
                    // RMC 20150226 adjustment in blob configuration (s)
                    string sBlobConfig = AppSettingsManager.GetBlobImageConfig();
                    // AST 20150415 remove condition as gob request (s)
                    //if (sBlobConfig == "F")
                    //    result.Query = "SELECT dblob_seq_two.nextval FROM dual"; //hardcoded
                    //else// RMC 20150226 adjustment in blob configuration (e)
                        result.Query = "SELECT docblob_seq.nextval FROM dual"; //hardcoded
                    // AST 20150415 remove condition as gob request (e)
                    if (result.Execute())
                    {
                        if (result.Read())
                            intSequence = result.GetInt(0);
                    }
                    if (intSequence == 0)
                        blnIsInsert = false;
                }

                try
                {
                    for (int i = 0; i < strQueries.Count; i++)
                    {
                        //RDO 051109 (s)
                        bool blnHasParameter = (strQueries[i].IndexOf(":1") != -1);
                        string strQuery = strQueries[i];
                        if (blnIsInsert)
                        {
                            strQuery = strQuery.Replace(":2", string.Format("'{0}'", @m_lstIconPaths[i]));
                            strQuery = strQuery.Replace(":3", string.Format("'{0}'", m_lstIconNames[i]));
                            strQuery = strQuery.Replace(":4", string.Format("'{0}'", m_lstIconExtensions[i]));
                            strQuery = strQuery.Replace(":5", string.Format("'{0}'", intSequence + i));
                        }

                        //RDO 051109 (e)

                        result.Query = strQuery;
                        using (MemoryStream stream = new MemoryStream())
                        {
                            //RDO 051109 (s)
                            if (blnHasParameter)
                            {
                                m_lstIconImages[i].Save(stream, m_lstIconImages[i].RawFormat);
                                result.AddParameter(":1", stream.ToArray());
                            }

                            //MessageBox.Show(result.QueryToString());

                            if (result.ExecuteNonQuery() == 0)
                            {
                                result.Rollback();
                                result.Close();
                                stream.Close();
                                return false;
                            }
                            stream.Close();
                        }
                    }
                    if (!result.Commit())
                    {
                        result.Rollback();
                        result.Close();
                        return false;
                    }
                }
                catch (Exception e)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(e.Message, string.Empty, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
                result.Close();

                return true;
            }
            catch { return false; }
        }

        public bool LoadImageQuery(string strQuery)
        {
            try
            {
                m_intIconIndex = 0;
                m_lstIconImages.Clear();
                m_lstIconNames.Clear();

                m_lstIconPaths.Clear();
                m_lstIconExtensions.Clear();

                OracleResultSet result = new OracleResultSet();
                result.CreateBlobConnection();
                /*result.CreateNewInstance(0);
            
                if (!result.isConnected)
                    return false;*/

                try
                {
                    result.Query = strQuery;
                    if (result.Execute())
                    {
                        while (result.Read())
                        {
                            try
                            {
                                byte[] blob = result.GetBlob(0);
                                if (blob != null)
                                {
                                    using (MemoryStream memStream = new MemoryStream(blob))
                                    {
                                        m_lstIconImages.Add(Image.FromStream(memStream));
                                        memStream.Close();
                                    }
                                }

                                m_lstIconNames.Add(result.GetString(1));

                                m_lstIconPaths.Add(string.Empty);
                                m_lstIconExtensions.Add(string.Empty);
                            }
                            catch
                            {
                                result.Close();
                                return false;
                            }
                        }
                    }
                    result.Close();
                }
                catch
                {
                    result.Close();
                    return false;
                }

                return true;
            }
            catch { return false; }
        }


        private void picImage_MouseEnter(object sender, EventArgs e)
        {
            if (!picImage.Focused)
                picImage.Focus();
        }

        private void picImage_MouseClick(object sender, MouseEventArgs e)
        {
            if (m_enuViewMode == ViewMode.ZoomMode)
            {
                if (e.Button == MouseButtons.Left) //zoom in
                    this.ZoomIn();
                else if (e.Button == MouseButtons.Right) //zoom out
                    this.ZoomOut();
            }
        }

        public bool FitToPage()
        {
            if (picImage.Image == null)
                return false;

            double dblAspectRatio = 0.0;
            if (pnlImage.Width > pnlImage.Height)
            {
                dblAspectRatio = pnlImage.Width * 1.0 / picImage.Image.Width;
                picImage.Width = pnlImage.Width;
                picImage.Height = Convert.ToInt32(picImage.Image.Height * dblAspectRatio);
            }
            else
            {
                dblAspectRatio = pnlImage.Height * 1.0 / picImage.Image.Height;
                picImage.Height = pnlImage.Height;
                picImage.Width = Convert.ToInt32(picImage.Image.Width * dblAspectRatio);
            }
            picImage.SizeMode = PictureBoxSizeMode.StretchImage;
            return true;
        }

        public bool LeftRotate()
        {
            return this.Rotate(RotateFlipType.Rotate90FlipXY);
        }

        public bool RightRotate()
        {
            
            return this.Rotate(RotateFlipType.Rotate270FlipXY);
        }

        private bool Rotate(RotateFlipType rft)
        {
            //insert codes here
            int intWidth = picImage.Width;
            int intHeight = picImage.Height;
            picImage.Width = intHeight;
            picImage.Height = intWidth;

            this.Image.RotateFlip(rft);
            this.Refresh();
            return true;
        }

        public bool ZoomIn()
        {
            return this.Zoom(m_dblMagnificationScale, m_intMagnificationMinMaxStep);
        }
        
        public bool ZoomOut()
        {
            return this.Zoom(1.0 / m_dblMagnificationScale, m_intMagnificationMinMaxStep);
        }

        public bool Zoom(double dblMagnification, int intMinMaxRatio)
        {
            bool blnIsMagnify = false;
            if (dblMagnification < 1.0)
            {
                if (picImage.Width > (pnlImage.Width / intMinMaxRatio) &&
                    picImage.Height > (pnlImage.Height / intMinMaxRatio))
                    blnIsMagnify = true;
            }
            else
            {
                if (picImage.Width < (pnlImage.Width * intMinMaxRatio) &&
                    picImage.Height < (pnlImage.Height * intMinMaxRatio))
                    blnIsMagnify = true;
            }

            if (blnIsMagnify)
            {
                picImage.SizeMode = PictureBoxSizeMode.StretchImage;
                picImage.Width = Convert.ToInt32(picImage.Width * dblMagnification);
                picImage.Height = Convert.ToInt32(picImage.Height * dblMagnification);
                iWidth = picImage.Width;
                iHeight = picImage.Height;
            }
            // GDE 20090623 added (s)
                /*
            else
            {
               if(dblMagnification < 1.0)
               {
                   MessageBox.Show("This is the minimum !", "Image Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   return true;
               }
               else
               {
                   MessageBox.Show("This is the maximum !", "Image Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   return true;
               }
            }
                 */
            // GDE 20090623 added (e)

            return blnIsMagnify;
        }
        public void MoveScroll(bool bFlag)
        {
            if(bFlag)
            {
                if(yy >= 0)
                {
                    yy = yy - iHeight;
                    pnlImage.AutoScrollPosition = new Point(xx, 0);
                    yy = 0;
                }
            }
            else
            {
                if(yy <= iHeight)
                {
                    yy = yy + iHeight;
                    pnlImage.AutoScrollPosition = new Point(xx, yy);
                    //yy = iScreenHeight - picImage.Height;
                }
            }
        }
        public void MoveMinScroll(bool bFlag)
        {

            if (bFlag)
            {
                if (yy >= 0)
                {
                    yy = yy - (iHeight/ 10);
                    pnlImage.AutoScrollPosition = new Point(xx, yy);
                }
            }
            else
            {
                if (yy <= picImage.Height)
                {
                    yy = yy + (iHeight / 10);
                    pnlImage.AutoScrollPosition = new Point(xx, yy);

                }
                else
                {
                    yy = picImage.Height;
                }
            }
            
        }
        public void MoveMinVScroll(bool bFlag)
        {

            if (bFlag)
            {
                if (xx >= 0)
                {
                    int xxx = iWidth / 20;
                    xx = xx - xxx;
                    pnlImage.AutoScrollPosition = new Point(xx, yy);
                }
            }
            else
            {
                if (xx <= picImage.Width)
                {
                    int xxx = iWidth / 20;
                    xx = xx + xxx;
                    pnlImage.AutoScrollPosition = new Point(xx, yy);

                }
                else
                {
                    xx = picImage.Width;
                }
            }
            
        }
        private void picImage_MouseDown(object sender, MouseEventArgs e)
        {
            m_blnIsScrolling = true;
            m_pntScroll = new Point(e.X, e.Y);
            //MessageBox.Show(m_pntScroll.ToString());
        }

        private void picImage_MouseUp(object sender, MouseEventArgs e)
        {
            m_blnIsScrolling = false;
            if (e.Button == MouseButtons.Right)
                ZoomOut();
        }

        private void picImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_enuViewMode == ViewMode.PanMode)
            {
                if (m_blnIsScrolling && e.Button == MouseButtons.Left)
                {
                    int x = m_pntScroll.X - e.X;
                    int y = m_pntScroll.Y - e.Y;

                    int cx = pnlImage.AutoScrollPosition.X;
                    int cy = pnlImage.AutoScrollPosition.Y;
                    pnlImage.AutoScrollPosition = new Point(x - cx, y - cy);
                }
                
            }
        }

        private void picImage_DoubleClick(object sender, EventArgs e)
        {
            ZoomIn();
        }

        public void InitializeForm()
        {
            // RMC 20111206 Added attachment of blob image
            m_lstIconImages.Add(null);
            m_lstIconNames.Add(string.Empty);
            m_lstIconPaths.Add(string.Empty);
            m_lstIconExtensions.Add(string.Empty);
        }

        
    }
}
