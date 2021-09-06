using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using System.IO; //MCR 20210701

namespace Amellar.Modules.Utilities
{
    public partial class frmSignatories : Form
    {
        string sImagename = "";
        byte[] blobData = null;

        public frmSignatories()
        {
            InitializeComponent();
        }

        private void frmSignatories_Load(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Add")
            {
                btnAdd.Text = "Save";
                btnCancel.Text = "Cancel";
                EnableControls(true);
                btnEdit.Enabled = false;
                dgUser.Enabled = false;
            }
            else //Save
            {
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Name is empty");
                    return;
                }
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Position is empty");
                    return;
                }

                if (pBoxSig.Image == null)
                {
                    MessageBox.Show("Image is empty");
                    return;
                }

                SaveRecord();
                MessageBox.Show("Succesfully Saved");
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                FileDialog fldlg = new OpenFileDialog();
                fldlg.InitialDirectory = @":C\";
                fldlg.Filter = "Image File (*.jpg;*.bmp;*.png)|*.jpg;*.bmp;*.png";
                if (fldlg.ShowDialog() == DialogResult.OK)
                {
                    sImagename = fldlg.FileName;
                    Bitmap newimg = new Bitmap(sImagename);
                    pBoxSig.SizeMode = PictureBoxSizeMode.StretchImage;
                    pBoxSig.Image = (Image)newimg;
                }
                fldlg = null;
            }
            catch (System.ArgumentException ae)
            {
                sImagename = " ";
                MessageBox.Show(ae.Message.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (btnEdit.Text == "Edit")
            {
                btnEdit.Text = "Update";
                btnCancel.Text = "Cancel";
                EnableControls(true);
                btnAdd.Enabled = false;
                dgUser.Enabled = false;
            }
            else //Update
            {
                SaveRecord();
                MessageBox.Show("Succesfully Updated");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "Cancel")
            {
                EnableControls(false);
                btnAdd.Enabled = true;
                btnSetActive.Enabled = false;
                dgUser.Enabled = true;
                btnCancel.Enabled = true;
                btnCancel.Text = "Close";
                btnAdd.Text = "Add";
                btnEdit.Text = "Edit";
                LoadRecords();
                pBoxSig.Image = null;
                txtName.Text = "";
                txtPos.Text = "";
                dgUser.ClearSelection();
            }
            else
                this.Close();
        }

        private int GetCurrentID()
        {
            int iValue = 0;

            OracleResultSet result = new OracleResultSet();
            result.Query = "select nvl(max(id),0)+1 from BO_SIGNATORIES";
            if (result.Execute())
                if (result.Read())
                    iValue = result.GetInt(0);
            result.Close();

            return iValue;
        }

        private void SaveRecord()
        {
            try
            {
                using (FileStream fileStream = new FileStream(sImagename, FileMode.Open, FileAccess.Read))
                {
                    blobData = new byte[fileStream.Length];
                    fileStream.Read(blobData, 0, System.Convert.ToInt32(fileStream.Length));
                    fileStream.Close();
                }
            }
            catch { }

            OracleResultSet result = new OracleResultSet();
            if (btnAdd.Text == "Save")
            {
                int iValue = GetCurrentID();
                result.Query = "INSERT INTO BO_SIGNATORIES(id, name, pos, signature, isactive) VALUES (:1,:2,:3,:4,:5)";
                result.AddParameter(":1", iValue);
                result.AddParameter(":2", StringUtilities.HandleApostrophe(txtName.Text.Trim()));
                result.AddParameter(":3", StringUtilities.HandleApostrophe(txtPos.Text.Trim()));
                result.AddParameter(":4", blobData);
                if (iValue == 1)
                    result.AddParameter(":5", 1);
                else
                    result.AddParameter(":5", 0);
            }
            else
            {
                String sValue = dgUser.CurrentRow.Cells[0].Value.ToString();
                result.Query = "UPDATE BO_SIGNATORIES set name = :1, pos = :2, signature = :3 where id = :4";
                
                result.AddParameter(":1", StringUtilities.HandleApostrophe(txtName.Text.Trim()));
                result.AddParameter(":2", StringUtilities.HandleApostrophe(txtPos.Text.Trim()));
                result.AddParameter(":3", blobData);
                result.AddParameter(":4", sValue);
            }

            if (result.ExecuteNonQuery() == 0)
                result.Rollback();
            else
            {
                if (!result.Commit())
                    result.Rollback();
            }
            result.Close();

            btnCancel.PerformClick();
        }

        private void EnableControls(bool blnEnable)
        {
            txtName.ReadOnly = !blnEnable;
            txtPos.ReadOnly = !blnEnable;
            btnAdd.Enabled = blnEnable;
            btnEdit.Enabled = blnEnable;
            btnLoad.Enabled = blnEnable;
            btnCancel.Enabled = blnEnable;
            dgUser.Enabled = blnEnable;
        }

        private void LoadImage()
        {
            if (dgUser.CurrentRow.Cells[0].Value != null)
            {

                OracleResultSet result = new OracleResultSet();
                try
                {
                    result.Query = "select * from BO_SIGNATORIES where id = '" + dgUser.CurrentRow.Cells[0].Value + "'";

                    if (result.Execute())
                        if (result.Read())
                            blobData = result.GetBlob(3);

                    Image image = (Bitmap)((new ImageConverter()).ConvertFrom(blobData));
                    pBoxSig.SizeMode = PictureBoxSizeMode.StretchImage;
                    pBoxSig.Image = image;
                }
                catch
                {
                }
                result.Close();
            }
        }

        private void LoadRecords()
        {
            dgUser.Rows.Clear();
            OracleResultSet pset = new OracleResultSet();
            pset.Query = "select * from BO_SIGNATORIES order by id";
            if (pset.Execute())
            {
                while (pset.Read())
                    dgUser.Rows.Add(pset.GetInt(0), StringUtilities.RemoveApostrophe(pset.GetString(1)), StringUtilities.RemoveApostrophe(pset.GetString(2)), pset.GetInt(4));
            }
            pset.Close();
            dgUser.ClearSelection();
        }

        private void dgUser_SelectionChanged(object sender, EventArgs e)
        {
        }

        private void dgUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnSetActive.Enabled = true;
            LoadImage();
            btnEdit.Enabled = true;
            txtName.Text = dgUser.CurrentRow.Cells[1].Value.ToString();
            txtPos.Text = dgUser.CurrentRow.Cells[2].Value.ToString();
        }

        private void btnSetActive_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Set this record as an active signatory?","",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                btnCancel.Text = "Cancel";
                OracleResultSet result = new OracleResultSet();

                result.Query = "UPDATE BO_SIGNATORIES set isactive = 0";
                result.ExecuteNonQuery();

                String sValue = dgUser.CurrentRow.Cells[0].Value.ToString();
                result.Query = "UPDATE BO_SIGNATORIES set isactive = :1 where id = :2";
                result.AddParameter(":1", 1);
                result.AddParameter(":2", sValue);
                result.ExecuteNonQuery();

                MessageBox.Show("Succesfully Activated");
                btnCancel.PerformClick();
            }
        }

    }
}