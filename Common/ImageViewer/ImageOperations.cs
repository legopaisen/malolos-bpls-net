using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Amellar.Common.DataConnector;
//using Common.CommandLine.Utility;

namespace Amellar.Common.ImageViewer
{
    /*
         connection arguments
         -host  192.168.0.155 -port 1521 -servicename rnd -userid rpt02011blob -password laM40
         --configwan -userid rpt02011blob -password laM40 
     */

    public class ImageOperations
    {
        private PictureBoxCtrl picImageViewer;
        //private Arguments arguments;

        private bool m_blnIsSavingAllowed;
        private bool m_blnIsBrowseable;

        private int m_intWidth;
        private int m_intHeight;

        private string m_strInitalDirectory;

        private List<string> m_lstQueries;

        private bool m_blnHasConnectionString;
        private bool m_blnIsInsertOnStartup;

        // rmc temp query
        public List<string> Query
        {
            get { return m_lstQueries; }
            set { m_lstQueries = value; }
        }
        // rmc temp

        public ImageOperations(PictureBoxCtrl picture)
        {
            this.picImageViewer = picture;
            m_lstQueries = new List<string>();

            this.ReadArguments();
        }

        


        
        public ImageOperations()
        {
            picImageViewer = new PictureBoxCtrl();
            m_lstQueries = new List<string>();

            this.ReadArguments();
        }

        public bool IsBrowseable
        {
            get { return m_blnIsBrowseable; }
        }

        public bool IsSavingAllowed
        {
            get { return m_blnIsSavingAllowed; }
            set { m_blnIsSavingAllowed = value; }
        }

        public int Width
        {
            get { return m_intWidth; }
        }

        public int Height
        {
            get { return m_intHeight; }
        }

        public string InitialDirectory
        {
            get { return m_strInitalDirectory; }
        }

        public bool InsertQueries()
        {
            return picImageViewer.InsertImageQuery(m_lstQueries);
        }

        public void GetLocalFile(string strFilePath, int intImageIndex, bool blnIsShow)
        {
            if (File.Exists(strFilePath))
            {
                try
                {
                    Bitmap image = new Bitmap(strFilePath);
                    if (picImageViewer.ImageCount != 0 && intImageIndex >= 0 &&
                        picImageViewer.ImageCount > intImageIndex)
                    {
                        picImageViewer.Images[intImageIndex] = (Image)image;
                        /*
                        if (blnIsShow)
                            picImageViewer.ViewImage(intImageIndex);
                        */
                    }
                    
                }
                catch (System.ArgumentException ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString(), "ImageView",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString(), "ImageView",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }

        public bool HasConnectionString
        {
            get { return m_blnHasConnectionString; }
        }

        public bool IsInsertOnStartup
        {
            get { return m_blnIsInsertOnStartup; }
        }


        private void ReadArguments()
        {
            m_blnHasConnectionString = true;
            m_blnIsBrowseable = false;
            m_blnIsSavingAllowed = false;
            m_blnIsSavingAllowed = false;
            m_intWidth = 0;
            m_intHeight = 0;
            m_strInitalDirectory = string.Empty;
            m_blnIsInsertOnStartup = false;
            
            /*
            arguments = new Arguments(Environment.GetCommandLineArgs());

            m_blnHasConnectionString = false;

            if ((arguments["host"] != null || arguments["configwan"] != null) && arguments["userid"] != null)
                m_blnHasConnectionString = true;

            m_blnIsBrowseable = true;
            m_blnIsSavingAllowed = true;
            m_intWidth = 0;
            m_intHeight = 0;
            m_strInitalDirectory = string.Empty;

            //gui
            if (arguments["nobrowse"] != null && arguments["nobrowse"] == "true")
                m_blnIsBrowseable = false;
            if (arguments["nosave"] != null && arguments["nosave"] == "true")
                m_blnIsSavingAllowed = false;

            if (arguments["width"] != null)
                int.TryParse(arguments["width"], out m_intWidth);
            if (arguments["height"] != null)
                int.TryParse(arguments["height"], out m_intHeight);

            if (arguments["dir"] != null)
                m_strInitalDirectory = arguments["dir"];

            if (arguments["load"] != null)
                if (picImageViewer.LoadImageQuery(arguments["load"]))
                {
                }

            int intCount = 1;
            m_lstQueries.Clear();
            while (arguments[string.Format("insert{0}", intCount)] != null)
            {
                m_lstQueries.Add(arguments[string.Format("insert{0}", intCount)]);
                intCount++;
            }

            if (picImageViewer.ImageCount == 0 && intCount == 1)
            {
                while (arguments[string.Format("image{0}", intCount)] != null)
                    intCount++;
            }
            if (picImageViewer.ImageCount == 0)
            {
                picImageViewer.ImageCount = intCount - 1;
                //this.UpdateNavigation();
            }

            //allow loading of local file if no load image query
            if (arguments["load"] == null)
            {
                intCount = 1;
                while (arguments[string.Format("image{0}", intCount)] != null && intCount <= picImageViewer.ImageCount)
                {
                    this.GetLocalFile(arguments[string.Format("image{0}", intCount)], intCount - 1, (intCount == 1));
                    intCount++;
                }
            }

            m_blnIsInsertOnStartup = false;

            if (arguments["save"] != null && arguments["save"] == "true")
                m_blnIsInsertOnStartup = true;
            */
        }
    }
}
