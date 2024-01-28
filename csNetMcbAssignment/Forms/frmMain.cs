using Common.Forms.Utility;
using Common.Utility.Connection;
using csNetMcbAssignment.Forms;
using System;
using System.Windows.Forms;
using mcb.main.Forms;

namespace csNetMcbAssignment
{
    /// <summary>Form main.</summary>
    public partial class frmMain : Form
    {
        #region Private declarations

        #region Variables

        private clsConnection objConnection = null;

        #endregion

        #endregion

        #region Constructors

        /// <summary>Constructors.</summary>
        public frmMain()
        {
            InitializeComponent();
            objConnection = new clsConnection();
        }

        #endregion

        #region Private methods

        /// <summary>Check if connection parameters exists.</summary>
        /// <return>True if connection exists.</return>
        private bool CheckifConnectionParametersExists()
        {
            // Declarations
            clsConnectionConfiguration objConnectionConfiguration = null;
            string[] sAppKeys = null;
            bool fExists = false;

            try
            {
                // Initialisations
                objConnectionConfiguration = new clsConnectionConfiguration();
                sAppKeys = objConnectionConfiguration.GetAppSettingKeys();

                if (sAppKeys.Length <= 0)
                    return false;

                objConnectionConfiguration.sSectionName = sAppKeys[0];
                fExists = objConnectionConfiguration.GetDatabaseConfig();

                objConnection.objSQLConnectionStringBuilder = new clsSQLConnectionStringBuilder()
                {
                    sServerName = objConnectionConfiguration.sServerName,
                    sDatabaseName = objConnectionConfiguration.sDatabaseName,
                    fUseSqlCredential = string.IsNullOrEmpty(objConnectionConfiguration.sUserName) &&
                                         string.IsNullOrEmpty(objConnectionConfiguration.sPassword),
                    sUserName = objConnectionConfiguration.sUserName,
                    sPassword = objConnectionConfiguration.sPassword,
                };
                objConnection.OpenConnection();

                return fExists;
            }
            finally
            {
                if (objConnectionConfiguration != null)
                {
                    objConnectionConfiguration.Dispose();
                    objConnectionConfiguration = null;
                }
            }
        }

        /// <summary>Close Application.</summary>
        private void CloseApplication()
        {
            Close();
            Application.Exit();
        }

        #endregion

        #region Events

        /// <summary>Form loading.</summary>
        /// <param name="sender">Object reference.</param>
        /// <param name="e">Event data.</param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            // Declarations
            bool fConnectionDetailExist = false;
            frmDatabaseConnection objFrmDatabaseConnection = null;

            try
            {
                // Initialisations
                fConnectionDetailExist = CheckifConnectionParametersExists();
                objFrmDatabaseConnection = new frmDatabaseConnection();

                if (!fConnectionDetailExist)
                {
                    objFrmDatabaseConnection.ShowDialog();
                    CloseApplication();
                    return;
                }
            }
            finally
            {
                if (objFrmDatabaseConnection != null)
                {
                    objFrmDatabaseConnection.Dispose();
                    objFrmDatabaseConnection = null;
                }
            }
        }

        /// <summary>Button connection configuration clicked.</summary>
        /// <param name="sender">Object reference.</param>
        /// <param name="e">Event data.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            frmDatabaseConnection frmConnection = new frmDatabaseConnection();
            frmConnection.ShowDialog();
        }

        /// <summary>Setting cofiguaration clicked.</summary>
        /// <param name="sender">Object reference.</param>
        /// <param name="e">Event data.</param>
        private void button2_Click(object sender, EventArgs e)
        {
            frmConfiguration objFmConfiguration = new frmConfiguration();
            objFmConfiguration.objConnection = objConnection;
            objFmConfiguration.ShowDialog();
        }

        #endregion

    }
}
