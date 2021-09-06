using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing.Common;
using ZXing;
using ZXing.QrCode;
using System.IO;
using Amellar.Common.DataConnector;
using Amellar.Common.EncryptUtilities;


namespace QRCODE
{
    public partial class frmQR : Form
    {
        public frmQR()
        {
            InitializeComponent();
        }

        byte[] blobData = null;
        string sBin = "";
        QrCodeEncodingOptions options = new QrCodeEncodingOptions();

        private void frmQR_Load(object sender, EventArgs e)
        {
            Task();
        }

        public void Task()
        {
            options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = 250,
                Height = 250,
            };
            var writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;

            try
            {
                //String sPath = Directory.GetCurrentDirectory() + "\\QR\\QRCODE.txt";
                String sPath = Directory.GetCurrentDirectory() + "\\QRCODE.txt";

                StreamReader SR = new StreamReader(sPath);
                string strFileText = "";
                strFileText = SR.ReadToEnd();
                SR.Close();
                //strFileText = decrypt.EncryptString(strFileText);
                //strFileText = decrypt.DecryptString(strFileText);
                sBin = strFileText.Remove(24);
                sBin = sBin.Remove(0, 5);

                txtSample.Text = strFileText;
                btnGenerate.PerformClick();
                SaveImage(sBin);
                this.Close();
            }
            catch { this.Close(); }
        }

        private void SaveImage(string sBin)
        {
            try
            {
                Image image = pBoxQR.Image;
                MemoryStream ms = new MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                blobData = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(blobData, 0, System.Convert.ToInt32(blobData.Length));
                ms.Close();
            }
            catch { }

            OracleResultSet result = new OracleResultSet();
            result.Query = "DELETE FROM BO_QR WHERE BIN = :1";
            result.AddParameter(":1", sBin);
            result.ExecuteNonQuery();

            result.Query = "INSERT INTO BO_QR(BIN, QR_CODE) VALUES (:1, :2)";
            result.AddParameter(":1", sBin);
            result.AddParameter(":2", blobData);

            if (result.ExecuteNonQuery() == 0)
                result.Rollback();
            else
            {
                if (!result.Commit())
                    result.Rollback();

            }
            result.Close();
        }

        private void txtSample_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtSample.Text) || String.IsNullOrEmpty(txtSample.Text))
            {
                pBoxQR.Image = null;
                MessageBox.Show("Text not found", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var qr = new ZXing.BarcodeWriter();
                qr.Options = options;
                qr.Format = ZXing.BarcodeFormat.QR_CODE;
                var result = new Bitmap(qr.Write(txtSample.Text.Trim()));
                pBoxQR.Image = result;
                //txtSample.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bitmap = new Bitmap(pBoxQR.Image);
                BarcodeReader reader = new BarcodeReader { AutoRotate = true, TryInverted = true };
                Result result = reader.Decode(bitmap);
                string decoded = result.ToString().Trim();
                txtSample.Text = decoded;
            }
            catch (Exception)
            {
                MessageBox.Show("Image not found", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //update : Feb 3,2018
        }

        private void pBoxQR_Click(object sender, EventArgs e)
        {
            if (pBoxQR.Image == null)
            {
                MessageBox.Show("Image not found", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SaveFileDialog save = new SaveFileDialog();
                save.CreatePrompt = true;
                save.OverwritePrompt = true;
                save.FileName = "QR";
                save.Filter = "PNG|*.png|JPEG|*.jpg|BMP|*.bmp|GIF|*.gif";
                if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    pBoxQR.Image.Save(save.FileName);
                    save.InitialDirectory = Environment.GetFolderPath
                                (Environment.SpecialFolder.Desktop);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();
            try
            {
                result.Query = "select * from BO_QR where bin = '"+ sBin + "'";
                if (result.Execute())
                    if (result.Read())
                        blobData = result.GetBlob(1);
            }
            catch
            {
                throw new Exception("Error in searching ");
            }

            result.Close();
            Image image = (Bitmap)((new ImageConverter()).ConvertFrom(blobData));
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = image;
        }

    }
}
