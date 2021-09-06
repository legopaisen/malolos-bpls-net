using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.TextValidation;

namespace Amellar.Common.ImageViewer
{
    public partial class frmDatabaseInfo : Form
    {
        private MoneyTextValidator m_objNumberValidator;

        public bool Cancel
        {
            get { return m_isCancel; }
        }
        private bool m_isCancel;

        public frmDatabaseInfo()
        {
            InitializeComponent();

            m_objImageInfo = new ImageInfo();

            m_objNumberValidator = new MoneyTextValidator();
        }

        //need to add function overload of this form for build up
        public frmDatabaseInfo(string strPin)
        {

        }


        private ImageInfo m_objImageInfo;
        public ImageInfo ImageInformation
        {
            get { return m_objImageInfo; }
        }

        public void setImageInfo(ImageInfo objImageInfo)
        {
            m_objImageInfo = objImageInfo;
        }


        private void ClearFields()
        {
            


        }




        private void form_FormClosing(object sender, EventArgs e)
        {
            /*if (!form.Cancel)
            {
                string strPin = form.Pin.Trim();
                txtPIN.Text = strPin;
                txtOwnerCode.Text = form.OwnCode;
                txtOwnerName.Text = string.Format("{0}, {1} {2}", form.LastName, form.FirstName, form.MiddleInitial);
                txtPrevArp.Text = form.PrevArpn;
                txtARPN.Text = form.Arpn;

                txtAssessedValue.Text = form.AssessValueFrom;
                dtEffectivity.Value = form.EffectivityDateFrom;
                txtTDN.Text = form.Tdn;
                txtPrevAssessedValue.Text = form.PreviAssessValue;
                txtPreviousPin.Text = form.PrevPin;
                txtUpdateCode.Text = form.UpdateCode;

                txtLot.Text = form.LotNumber;
                txtCad.Text = form.CadNumber;
                txtTitle.Text = form.TitleNumber;
                txtSurvey.Text = form.SurveyNumber;



                OracleResultSet result = new OracleResultSet();
                result.Query = string.Format("select memoranda from faas_view where pin = '{0}'", txtPIN.Text.Trim());
                txtMemoranda.Text = result.ExecuteScalar();



                this.LoadPaymentInfo(strPin);


                
                result.Query = string.Format("select * from payment_hist where pin = '{0}' order by tax_year desc, payment_period desc, dt_paid desc", txtPIN.Text.Trim());
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        txtORBasic.Text = result.GetString("bsc_or_no").Trim();
                        dtpORDate.Value = result.GetDateTime("dt_paid");
                        txtActualUse.Text = result.GetString("act_use").Trim();
                        txtRemarks.Text = result.GetString("remarks").Trim();

                        string strSource = result.GetString("source").Trim();
                        if (strSource == "")
                            txtPaymentSource.Text = "Online";
                        else if (strSource == "1")
                            txtPaymentSource.Text = "Posting";
                        else if (strSource == "O")
                            txtPaymentSource.Text = "Offline";

                        StringBuilder strPayment = new StringBuilder();
                        strPayment.Length = 0;

                        string strPaymentType = result.GetString("payment_type").Trim();
                        if (strPaymentType == "F")
                            strPayment.Append("Full ");
                        else
                        {
                            int intPaymentPeriod = 0;
                            int intNoQtr = 0;

                            int.TryParse(result.GetString("payment_period").Trim(), out intPaymentPeriod);
                            int.TryParse(result.GetString("no_of_qtr").Trim(), out intNoQtr);

                            int intPaymentQtr = (intPaymentPeriod + intNoQtr);
                            if (intPaymentQtr > 4)
                                strPayment.Append("Full ");
                            else
                                strPayment.Append(string.Format("Installent quarter {0}-{1}", intPaymentPeriod, intPaymentQtr));

                        }

                        strPayment.Append(result.GetInt("tax_year").ToString());

                        txtTaxYear.Text = strPayment.ToString();

                    }
                }
                

                result.Close();



                //m_objImageInfo.setFaasInfo(m_objImageInfo.Pin);
            }
            else
            {
                //must empty the fields

            }
            form.Dispose();
            btnSearch.Enabled = true;*/
        }

        //Common.Search.frmSearchEngineVersion2 form;

        private void btnSearch_Click(object sender, EventArgs e)
        {
            /*this.txtPIN.ReadOnly = true;
            this.ClearFields();

            btnSearch.Enabled = false;

            form = new Common.Search.frmSearchEngineVersion2();
            form.FormClosing += new FormClosingEventHandler(form_FormClosing);
            //    using (Common.Search.frmSearchEngineVersion2 form = new Common.Search.frmSearchEngineVersion2())
            {

                form.TopMost = true;
                
                form.Show();



            }
            */
        }        

        private void txtAssessedValue_TextChanged(object sender, EventArgs e)
        {
            
        }        

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearFields();
        }

        private void btnDeficient_Click(object sender, EventArgs e)
        {

            /*OracleResultSet result = new OracleResultSet();
            //frmDeficientRecords form = new frmDeficientRecords();
            using (Amellar.RPTA.Classes.DeficientRecords.frmDeficientRecords form = new Amellar.RPTA.Classes.DeficientRecords.frmDeficientRecords())
            {
                string strPin = m_objImageInfo.Pin;
                if (strPin != null && strPin != "")
                {
                    form.Pin = strPin;
                    form.ORNumber = txtORBasic.Text.Trim();
                    form.TopMost = true;
                    form.ShowDialog();

                    form.SaveListInfo(result);
                    form.SaveDeficientInfo(result);
                }
            }

            //form.Pin = m_strPin;
            //form.ORNumber = m_strOrBasicNumber;

            //form.ShowDialog();

            //m_lstDeficientInfo = form.


            //form.SaveListInfo(result);
            //form.SaveDeficientInfo(result);

            if (!result.Commit())
            {
                result.Rollback();
                result.Close();
            }*/
        }

        private void frmDatabaseInfo_Load(object sender, EventArgs e)
        {
            /*this.dtEffectivity.Enabled = false;
            this.dtpORDate.Enabled = false;

            this.txtPIN.ReadOnly = false;
          //  this.txtPIN.Text = "023-03";
            //JVL20091202
            this.txtPIN.Text = Common.AppSettings.AppSettingsManager.GetConfigValue("01").Trim();*/
        }

        private void frmDatabaseInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (form != null && !form.IsDisposed)
                //form.Dispose();
            
        }



        private void LoadInfo(string strPin)
        {
            /*OracleResultSet result = new OracleResultSet();
            int intCount = 0;
            result.Query = string.Format("select count(*) from faas_view where pin = '{0}'", strPin.Trim());
            int.TryParse(result.ExecuteScalar(), out intCount);
            if (intCount == 0)
            {
                MessageBox.Show(string.Format("Pin {0} not found.", strPin), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                return;
            }

            result.Query = string.Format("select * from faas_view where pin = '{0}'", strPin.Trim());
            if (result.Execute())
            {
                if (result.Read())
                {

                    txtPIN.Text = result.GetString("pin").Trim();
                    string strOwnerCode = result.GetString("own_code").Trim();
                    txtOwnerCode.Text = strOwnerCode;

                    Amellar.RPTA.Classes.Owner.Owner ownner = new Amellar.RPTA.Classes.Owner.Owner();
                    ownner.GetOwner(strOwnerCode);

                    txtOwnerName.Text = string.Format("{0}, {1} {2}", ownner.LastName, ownner.FirstName, ownner.MiddleInitial);

                    txtPrevArp.Text = result.GetString("prev_arp").Trim();



                    txtARPN.Text = result.GetString("arpn").Trim();

                    txtAssessedValue.Text = result.GetDouble("ass_val").ToString();


                    dtEffectivity.Value = result.GetDateTime("eff_date");

                    txtTDN.Text = result.GetString("td_num").Trim();

                    txtPrevAssessedValue.Text = result.GetDouble("prev_ass_val").ToString();
                    txtPreviousPin.Text = result.GetString("prev_pin").Trim();

                    txtUpdateCode.Text = result.GetString("update_code").Trim();

                    txtLot.Text = result.GetString("lot_no").Trim();
                    txtCad.Text = result.GetString("cad_no").Trim();
                    txtTitle.Text = result.GetString("oct_no").Trim();
                    txtSurvey.Text = result.GetString("survey_no").Trim();



                    txtMemoranda.Text = result.GetString("memoranda").Trim();



                    this.LoadPaymentInfo(strPin);



                }
            }
            result.Close();*/
        }


        private void LoadPaymentInfo(string strPin)
        {
            /*OracleResultSet result = new OracleResultSet();
            string strOrNum = string.Empty;
            string strTaxYear = string.Empty;

            result.Query = string.Format("select * from payment_hist where pin = '{0}' order by tax_year desc, payment_period desc, dt_paid desc", strPin);
            if (result.Execute())
            {
                if (result.Read())
                {
                    
                    strOrNum = result.GetString("bsc_or_no").Trim();
                    txtORBasic.Text = strOrNum;

                    dtpORDate.Value = result.GetDateTime("dt_paid");
                    txtActualUse.Text = result.GetString("act_use").Trim();
                    txtRemarks.Text = result.GetString("remarks").Trim();

                    txtArpPayment.Text = result.GetString("arp").Trim();

                    string strSource = result.GetString("source").Trim();
                    if (strSource == "")
                        txtPaymentSource.Text = "Online";
                    else if (strSource == "1")
                        txtPaymentSource.Text = "Posting";
                    else if (strSource == "O")
                        txtPaymentSource.Text = "Offline";


                    strTaxYear = result.GetInt("tax_year").ToString();


                    StringBuilder strPayment = new StringBuilder();
                    strPayment.Length = 0;
                

                    string strPaymentType = result.GetString("payment_type").Trim();
                    if (strPaymentType == "F")
                        strPayment.Append("Full ");
                    else
                    {
                        int intPaymentPeriod = 0;
                        int intNoQtr = 0;

                        int.TryParse(result.GetString("payment_period").Trim(), out intPaymentPeriod);
                        int.TryParse(result.GetString("no_of_qtr").Trim(), out intNoQtr);

                        int intPaymentQtr = (intPaymentPeriod + intNoQtr);
                        if (intPaymentQtr > 4)
                            strPayment.Append("Full ");
                        else
                            strPayment.Append(string.Format("Installent quarter {0}-{1} ", intPaymentPeriod, intPaymentQtr));

                    }


                    strPayment.Append(strTaxYear);

                    txtTaxYear.Text = strPayment.ToString();


                }
            }

            if (strOrNum != string.Empty)
            {
                //result.Query = string.Format("select * from payment_hist where bsc_or_no = '{0}' and tax_year = {1} order by payment_period desc, dt_paid desc", strOrNum, strTaxYear);

                result.Query = string.Format("select sum(bsc_tax), sum(sef_tax), sum(bsc_pen - bsc_dis), sum(sef_pen - sef_dis), sum(bsc_tax + bsc_pen - bsc_dis), sum(sef_tax + sef_pen - sef_dis), sum(bsc_tax + sef_tax + bsc_pen + sef_pen - bsc_dis - sef_dis ) from payment_hist where bsc_or_no = '{0}' and tax_year = {1} and pin = '{2}'", strOrNum, strTaxYear, strPin);

                if (result.Execute())
                {
                    if (result.Read())
                    {
                        txtBasic.Text = result.GetDouble(0).ToString();
                        txtSef.Text = result.GetDouble(1).ToString();
                        txtBasicIntDis.Text = result.GetDouble(2).ToString();
                        txtSefIntDis.Text = result.GetDouble(3).ToString();
                        txtBasicTotal.Text = result.GetDouble(4).ToString();
                        txtSefTotal.Text = result.GetDouble(5).ToString();
                        txtTotal.Text = result.GetDouble(6).ToString();
                    }


                }
            }
            result.Close();  */ 
        }




        private void txtPIN_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*if (e.KeyChar == 13)
            {

                if (PinManager.GetPinType(txtPIN.Text.Trim()) == PinManager.PinType.InvalidPin)
                {
                    MessageBox.Show(this, string.Format("{0} is invalid Pin Format..", txtPIN.Text.Trim()), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //txtPIN.Text = "";
                }
                else
                {
                    string strPin = txtPIN.Text.Trim();
                    this.ClearFields();
                    this.LoadInfo(strPin);
                }
            }*/
        }

        private void frmDatabaseInfo_Validated(object sender, EventArgs e)
        {

         

        }

        private void frmDatabaseInfo_Activated(object sender, EventArgs e)
        {
            //this.txtPIN.Focus();
            //this.txtPIN.SelectionStart = txtPIN.Text.Trim().Length;
        }    
    }
}