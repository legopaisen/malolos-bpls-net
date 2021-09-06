using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using System.Data;

namespace Amellar.Common.DataConnector
{
    /// <summary>
    /// Converted to extented class 
    /// </summary>
    public class DataGridViewOracleResultSet : OracleResultSet
    {
        /// <summary>
        /// return the pre-count of rows in gridview
        /// </summary>
        public int RowCount
        {
            get { return m_intRowCount; }
        }

        private int m_intRowCount;

        public DataGridViewOracleResultSet()
        {
            m_intRowCount = 0;
        }

        public DataGridViewOracleResultSet(DataTable tblView, string strQuery, int startRecord)
        {
            this.CreateDataGridViewInstance(tblView, strQuery, startRecord);
        }

        /// <summary>
        /// This is Used for searching made to satisfy a request
        /// </summary>
        /// <param name="strQuery"></param>
        /// <param name="startRecord"></param>
        /// <param name="maxRecord"></param>
        /// <returns></returns>
        private void CreateDataGridViewInstance(DataTable tblView, string strQuery, int startRecord)
        {
            try
            {
                //   tblView = new DataTable();
                m_cmdCommand = new OracleCommand(strQuery, DataConnectorManager.Instance.Connection);
                m_cmdCommand.CommandType = CommandType.Text;

                OracleDataAdapter da = new OracleDataAdapter(m_cmdCommand);
                if(startRecord == -1)
                    da.Fill(tblView);
                else
                    da.Fill(startRecord, 10, tblView);                            
                //MessageBox.Show(tblView.Rows[0][0].ToString());

            }

            catch (OracleException ex) // catches only Oracle errors
            {
                switch (ex.Number)
                {
                    case 1:
                        MessageBox.Show("Error attempting to insert duplicate data.");
                        break;
                    case 12545:
                        MessageBox.Show("The database is unavailable.");
                        break;
                    default:
                        MessageBox.Show("Database error: " + ex.Message.ToString());
                        break;
                }
            }
            catch (Exception ex) // catches any error
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        /// <summary>
        /// This is used for direct populate of data grid commonly used
        /// </summary>
        /// <param name="grdView"></param>
        /// <param name="strQuery"></param>
        /// <param name="startRecord"></param>
        /// <param name="maxRecord"></param>
        public DataGridViewOracleResultSet(DataGridView grdView, string strQuery, int startRecord, int maxRecord)
        {
            this.CreateDataGridViewInstance(grdView, strQuery, startRecord, maxRecord);
        }

        private void CreateDataGridViewInstance(DataGridView grdView, string strQuery, int startRecord, int maxRecord)
        {
            try
            {
                DataTable tbl = new DataTable();
                m_cmdCommand = new OracleCommand(strQuery, DataConnectorManager.Instance.Connection);
                m_cmdCommand.CommandType = CommandType.Text;

                OracleDataAdapter da = new OracleDataAdapter(m_cmdCommand);

                if (startRecord == 0 && maxRecord == 0)
                {
                    da.Fill(tbl);
                }
                else
                {
                    da.Fill(startRecord, maxRecord, tbl);
                }

                grdView.AutoGenerateColumns = true;
                grdView.DataSource = tbl;

                CurrencyManager cm = (CurrencyManager)grdView.BindingContext[grdView.DataSource];//, grdView.DataMember];
                DataView dv = (DataView)cm.List;
                dv.AllowNew = false;
            }

            catch (OracleException ex) // catches only Oracle errors
            {
                switch (ex.Number)
                {
                    case 1:
                        MessageBox.Show("Error attempting to insert duplicate data.");
                        break;
                    case 12545:
                        MessageBox.Show("The database is unavailable.");
                        break;
                    default:
                        MessageBox.Show("Database error: " + ex.Message.ToString());
                        break;
                }
            }
            catch (Exception ex) // catches any error
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /////////////////////////////////////////////////////



        /// <summary>
        /// some code use this method
        /// </summary>
        public DataGridViewOracleResultSet(DataGridView dataGridView, string strQuery)
        {
            this.CreateDataGridViewInstance(dataGridView, strQuery, DataConnectorManager.Instance.Connection);
        }

        /*
                public DataGridViewOracleResultSet(DataGridView grdView, string strQuery, OracleConnection orac)

                {
                    this.CreateDataGridViewInstance(grdView, strQuery, orac);
                }
        */
        /// <summary>
        /// This is an additional method that takes 3 parameter needed System.Windows.Forms
        ///  by joelar
        /// Efficient in retrieving data from database to the DatagridView
        /// </summary>
        /// <param name="grdView"></param>
        /// <param name="strQuery"></param>
        /// <param name="orac"></param>
        // private void CreateDataGridViewInstance(DataGridView grdView, string strQuery, OracleConnection orac)

        private void CreateDataGridViewInstance(DataGridView grdView, string strQuery, OracleConnection orac)
        {
            try
            {

                DataSet ds = new DataSet();
                this.CreateInstance(orac);

                m_cmdCommand = new OracleCommand(strQuery, orac);
                m_cmdCommand.CommandType = CommandType.Text;

                OracleDataAdapter da = new OracleDataAdapter(m_cmdCommand);

                da.Fill(ds, "tblCollection");
                grdView.AutoGenerateColumns = true;
                grdView.DataSource = ds;
                grdView.DataMember = "tblCollection";

                grdView.Refresh();

                m_intRowCount = grdView.Rows.Count;
            }

            catch (OracleException ex) // catches only Oracle errors
            {
                switch (ex.Number)
                {
                    case 1:
                        MessageBox.Show("Error attempting to insert duplicate data.");
                        break;
                    case 12545:
                        MessageBox.Show("The database is unavailable.");
                        break;
                    default:
                        MessageBox.Show("Database error: " + ex.Message.ToString());
                        break;
                }
            }
            catch (Exception ex) // catches any error
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Effient for query only just to update the database
        /// by: joelar
        /// </summary>
        /// <param name="strQuery"></param>
        /// <param name="orac"></param>

        //        public DataGridViewOracleResultSet(string strQuery, OracleConnection orac)
        public DataGridViewOracleResultSet(string strQuery)
        {
            this.DataGridViewExecuteNonQuery(strQuery, DataConnectorManager.Instance.Connection);
        }

        private void DataGridViewExecuteNonQuery(string strQuery, OracleConnection orac)
        {

            try
            {
                DataSet ds = new DataSet();
                OracleCommand cmd = new OracleCommand(strQuery, orac);
                cmd.CommandType = CommandType.Text;
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(ds, "tblCollection");

            }
            catch (OracleException ex) // catches only Oracle errors
            {
                switch (ex.Number)
                {
                    case 1:
                        MessageBox.Show("Error attempting to insert duplicate data.");
                        break;
                    case 12545:
                        MessageBox.Show("The database is unavailable.");
                        break;
                    default:
                        MessageBox.Show("Database error: " + ex.Message.ToString());
                        break;
                }
            }
            catch (Exception ex) // catches any error
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }




    }
}
