using System;
using System.Collections.Generic;
using System.Data.Sql;
using System.Data;
using System.Windows.Forms;
using Common.Utility.Connection;
using System.Linq;
using Common.Forms.Utility;
using Common.Utility.Exceptions;
using Utilities.Scripts;

namespace csNetMcbAssignment.Forms
{
    /// <summary>Connection form.</summary>
    public partial class frmDatabaseConnection : Form
    {
        #region Private declarations

        #region Constants

        private const string sINSTANCENAME = "InstanceName";
        private const string sSERVERNAME = "ServerName";
        private const string sMESSAGE_BOX_TITLE_ERROR = "Error";
        private const string sKEY = "ConnectionString";
        private const string sMESSAGE_BOX_TITLE_INFORMATION = "Information";
        private const string sSQL_DEMILITER_GO_ENTER = "GO\r\n";
        private const string sSQL_DELIMITER_GO_SPACE = "GO ";
        private const string sSQL_DELIMITER_GO_TAB = "GO\t";

        #endregion

        #region Variables

        private bool _fCancel;

        #endregion

        #endregion

        #region Public Properties

        /// <summary>Indicate if button cancel has been pressed.</summary>
        public bool fCancel
        {
            get { return _fCancel; }
        }

        #endregion

        #region Constructors

        /// <summary>Constructor.</summary>
        public frmDatabaseConnection()
        {
            InitializeComponent();
        }

        #endregion

        #region Private methods

        /// <summary>Find all sql instances on the network.</summary>
        /// <returns>List of sql instances availabe on the network.</returns>
        private List<object> pSqlInstancesFinder()
        {
            // Declarations
            DataTable dtbInternal = null;
            List<object> lstsServerList = null;

            try
            {
                // Initialisations
                lstsServerList = new List<object>();
                dtbInternal = new DataTable();

                dtbInternal = SqlDataSourceEnumerator.Instance.GetDataSources();

                if (dtbInternal.Rows.Count == 0)
                    return null;

                foreach (DataRow objRow in dtbInternal.Rows)
                {
                    if (objRow.Field<string>(sINSTANCENAME) != null)
                        lstsServerList.Add(string.Format("{0}\\{1}", objRow.Field<string>(sSERVERNAME),
                                                                    objRow.Field<string>(sINSTANCENAME)));
                    else
                        lstsServerList.Add(objRow.Field<string>(sSERVERNAME));
                }
            }
            finally
            {
                if (dtbInternal != null)
                {
                    dtbInternal.Dispose();
                    dtbInternal = null;
                }
            }
            return lstsServerList;
        }

        #endregion

        #region Public methods

        /// <summary>Find all servers on the network.</summary>
        public void FinderServers()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (comboBox1.Items.Count > 0)
                    return;

                comboBox1.Items.AddRange(pSqlInstancesFinder().ToArray());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>Find all databases on selected server.</summary>
        private void FindDatabaseOnSelectedServer()
        {
            // Declarations
            DataTable dtbAllDatabases = null;
            clsConnection objConnection = null;
            clsSQLConnectionStringBuilder objSQLConnectionStringBuilder = null;
            string sSQL = "SELECT NAME " +
                          "FROM   SYSDATABASES " +
                          "ORDER BY NAME ASC";

            try
            {
                this.Cursor = Cursors.WaitCursor;
                comboBox2.Items.Clear();

               // Initialisations
               objConnection = new clsConnection();
                objSQLConnectionStringBuilder = new clsSQLConnectionStringBuilder();

                objSQLConnectionStringBuilder.sServerName = comboBox1.Text;

                if (radioButton2.Checked)
                {
                    objSQLConnectionStringBuilder.sUserName = textBox1.Text;
                    objSQLConnectionStringBuilder.sPassword = textBox2.Text;
                }

                objConnection.objSQLConnectionStringBuilder = objSQLConnectionStringBuilder;
                objConnection.OpenConnection();
                dtbAllDatabases = objConnection.FillDataTable(sSQL);
                objConnection.CloseConnection();

                comboBox2.Items.AddRange(dtbAllDatabases.AsEnumerable()
                                                        .Select(x => x.Field<string>(0))
                                                        .ToArray());
            }
            catch (Exception exException)
            {
                MessageBox.Show(exException.Message,
                                sMESSAGE_BOX_TITLE_ERROR,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                if (objSQLConnectionStringBuilder != null)
                    objSQLConnectionStringBuilder = null;

                if (objConnection != null)
                    objConnection = null;

                if (dtbAllDatabases != null)
                {
                    dtbAllDatabases.Dispose();
                    dtbAllDatabases = null;
                }

                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>Save the connection configuration file in app.config.</summary>
        private void SaveConnectionConfiguration()
        {
            // Declarations
            clsConnectionConfiguration objConnectionConfigurations = null;

            try
            {
                //Initialisations
                objConnectionConfigurations = new clsConnectionConfiguration();


                objConnectionConfigurations.sSectionName = sKEY;
                objConnectionConfigurations.sServerName = comboBox1.Text;
                objConnectionConfigurations.sDatabaseName = comboBox2.Text;
                objConnectionConfigurations.sUserName = textBox1.Text;
                objConnectionConfigurations.sPassword = textBox2.Text;

                objConnectionConfigurations.CreateDatabaseConfig();
            }
            finally
            {
                if (objConnectionConfigurations != null)
                {
                    objConnectionConfigurations.Dispose();
                    objConnectionConfigurations = null;
                }
            }
        }

        /// <summary>Create sql scripts.</summary>
        public void CreateSqlScripts()
        {
            // Declarations
            List<string> lstTables = null;
            List<string> lstStoredProcedures = null;

            clsConnection objConnection = null;
            clsScripts objScripts = null;

            try
            {
                // Initialisations
                lstTables = new List<string>();
                lstStoredProcedures = new List<string>();
                objConnection = new clsConnection();
                objScripts = new clsScripts();

                objConnection.objSQLConnectionStringBuilder = new clsSQLConnectionStringBuilder()
                {
                    sServerName = comboBox1.Text,
                    sDatabaseName = comboBox2.Text,
                    fUseSqlCredential = string.IsNullOrEmpty(textBox1.Text) &&
                                         string.IsNullOrEmpty(textBox2.Text),
                    sUserName = textBox1.Text,
                    sPassword = textBox2.Text,
                };

                lstTables.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iUD_CREATE_WDI_COUNTRY_TABLE)
                                             .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                   sSQL_DELIMITER_GO_SPACE,
                                                                   sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));
                lstTables.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iUD_CREATE_WDI_SERIES_TABLE)
                                             .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                   sSQL_DELIMITER_GO_SPACE,
                                                                   sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));

                lstTables.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iUD_CREATE_SETTING_TABLE)
                                             .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                   sSQL_DELIMITER_GO_SPACE,
                                                                   sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));

                lstStoredProcedures.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iSP_SETTING)
                                                       .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                             sSQL_DELIMITER_GO_SPACE,
                                                                             sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));

                lstStoredProcedures.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iSP_COUNTRY)
                                                       .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                             sSQL_DELIMITER_GO_SPACE,
                                                                             sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));

                lstStoredProcedures.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iSP_SERIES)
                                                       .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                             sSQL_DELIMITER_GO_SPACE,
                                                                             sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));

                lstTables.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iUD_CREATE_WDI_COUNTRY_SERIES_TABLE)
                                             .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                   sSQL_DELIMITER_GO_SPACE,
                                                                   sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));

                lstStoredProcedures.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iSP_COUNTRY_SERIES)
                                                      .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                             sSQL_DELIMITER_GO_SPACE,
                                                                             sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));

                lstTables.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iUD_CREATE_WDI_PERIOD)
                                             .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                   sSQL_DELIMITER_GO_SPACE,
                                                                   sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));

                lstTables.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iUD_CREATE_WDI_TIME_SERIES)
                                             .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                   sSQL_DELIMITER_GO_SPACE,
                                                                   sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));

                lstStoredProcedures.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iSP_TIME_SERIES)
                                                       .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                             sSQL_DELIMITER_GO_SPACE,
                                                                             sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));

                lstTables.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iUD_CREATE_WDI_FOOT_NOTE)
                                             .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                   sSQL_DELIMITER_GO_SPACE,
                                                                   sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));

                lstStoredProcedures.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iTT_FOOT_NOTE)
                                                       .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                             sSQL_DELIMITER_GO_SPACE,
                                                                             sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));

                lstStoredProcedures.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iSP_FOOT_NOTE)
                                                       .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                             sSQL_DELIMITER_GO_SPACE,
                                                                             sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));

                lstTables.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iUD_CREATE_WDI_DATA)
                                             .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                   sSQL_DELIMITER_GO_SPACE,
                                                                   sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));


                lstStoredProcedures.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iTT_DATA)
                                                       .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                             sSQL_DELIMITER_GO_SPACE,
                                                                             sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));


                lstStoredProcedures.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iSP_DATA)
                                                       .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                             sSQL_DELIMITER_GO_SPACE,
                                                                             sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));

                lstTables.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iUD_CREATE_CORRUPTION_TABLE)
                                             .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                   sSQL_DELIMITER_GO_SPACE,
                                                                   sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));

                lstStoredProcedures.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iTT_CORRUPTION)
                                                       .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                             sSQL_DELIMITER_GO_SPACE,
                                                                             sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));

                lstStoredProcedures.AddRange(objScripts.pGetScriptData(clsScripts.enuScripts.iSP_CORRUPTION)
                                                      .Split(new string[] { sSQL_DEMILITER_GO_ENTER,
                                                                             sSQL_DELIMITER_GO_SPACE,
                                                                             sSQL_DELIMITER_GO_TAB}, StringSplitOptions.RemoveEmptyEntries));


                objConnection.OpenConnection();
                objConnection.BeginTransaction();

                foreach (var sQuery in lstTables)
                {
                    objConnection.ExecuteNonQueries(sQuery);
                }

                foreach (var sQuery in lstStoredProcedures)
                {
                    objConnection.ExecuteNonQueries(sQuery);
                }

                objConnection.CommitTransaction();
             
            }
            catch (Exception eException)
            {
                objConnection.RollBackTransaction();
                MessageBox.Show(eException.Message,
                                    sMESSAGE_BOX_TITLE_ERROR,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
            }
            finally
            {
                if (lstTables != null)
                {
                    lstTables.Clear();
                    lstTables = null;
                }

                if (lstStoredProcedures != null)
                {
                    lstStoredProcedures.Clear();
                    lstTables = null;
                }

                if (objConnection != null)
                {
                    objConnection.CloseConnection();
                    objConnection = null;
                }
            }
        }
    
        #endregion

        #region Events

        /// <summary>Button cancel click.</summary>
        /// <param name="sender">Object reference.</param>
        /// <param name="e">Event data.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            _fCancel = true;
            Close();
        }
               

        /// <summary>Form loading.</summary>
        /// <param name="sender">Object reference.</param>
        /// <param name="e">Event data</param>
        private void frmDatabaseConnection_Load(object sender, EventArgs e)
        {
            FinderServers();
        }

        /// <summary>Radio button checked.</summary>
        /// <param name="sender">Object reference.</param>
        /// <param name="e">Event data</param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2.Checked =!radioButton1.Checked;
            textBox1.Enabled = !radioButton1.Checked;
            textBox2.Enabled = !radioButton1.Checked;
            label2.Enabled = !radioButton1.Checked;
            label3.Enabled = !radioButton1.Checked;
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
        }

        /// <summary>Radio button checked..</summary>
        /// <param name="sender">Object reference.</param>
        /// <param name="e">Event data</param>
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2.Checked = !radioButton1.Checked;
            textBox1.Enabled = !radioButton1.Checked;
            textBox2.Enabled = !radioButton1.Checked;
            label2.Enabled = !radioButton1.Checked;
            label3.Enabled = !radioButton1.Checked;
        }

        /// <summary>Load databases.</summary>
        /// <param name="sender">Object reference.</param>
        /// <param name="e">Event data.</param>
        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            FindDatabaseOnSelectedServer();
        }

        /// <summary>Finish button click.</summary>
        /// <param name="sender">Object reference.</param>
        /// <param name="e">Event data.</param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(comboBox1.Text))
                    throw new clsInfoExceptions("Please enter a server name to connect!");

                if (radioButton2.Checked &
                   string.IsNullOrEmpty(textBox1.Text))
                {
                    textBox1.Focus();
                    throw new clsInfoExceptions("Invalid username!");
                }

                if (string.IsNullOrEmpty(comboBox2.Text))
                    throw new clsInfoExceptions("Please select a database!");

                SaveConnectionConfiguration();
                CreateSqlScripts();
                Close();
            }
            catch (clsInfoExceptions exInfoExceptions)
            {
                MessageBox.Show(exInfoExceptions.Message,
                                sMESSAGE_BOX_TITLE_INFORMATION,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            catch (Exception exException)
            {
               MessageBox.Show(exException.Message,
                               sMESSAGE_BOX_TITLE_ERROR,
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
