using Common.Utility.Security;
using System.Data.SqlClient;

namespace Common.Utility.Connection
{
    /// <summary>Build SQL connection string.</summary>
    public class clsSQLConnectionStringBuilder
    {
        #region Private decarations

        private const string sUSER_ID = "User ID";
        private const string sPASSWORD = "Password";

        private string _sSqlConnectionString;
        private bool _fUseSqlCredential = false;

        #endregion

        #region Public properties

        /// <summary>Server name.</summary>
        public string sServerName { get; set; }

        /// <summary>Database name.</summary>
        public string sDatabaseName { get; set; }

        /// <summary>Sql username.</summary>
        public string sUserName { get; set; }

        /// <summary>Sql password</summary>
        public string sPassword { get; set; }

        /// <summary>Generate a constring that can be use with sql credential.</summary>
        public bool fUseSqlCredential
        {
            set
            {
                _fUseSqlCredential = value;
            }
        }

        /// <summary>Sql credential. It hides the username  and password in the connection string.</summary>
        public SqlCredential objSqlCredential
        {
            get { return objSqlCredentialBuilder(); }
        }

        /// <summary>Return sql connection string.</summary>
        public string sSqlConnectionString
        {
            get
            {
                CreateSqlConnectionStringBuilder();
                return _sSqlConnectionString;
            }
        }

        #endregion

        #region Private methods

        /// <summary>Create an  object sqlconnectionstring builder.</summary>
        private void CreateSqlConnectionStringBuilder()
        {
            //Declarations
            SqlConnectionStringBuilder objSqlConnectionStringBuilder = null;

            try
            {
                // Initialisations
                objSqlConnectionStringBuilder = new SqlConnectionStringBuilder();

                objSqlConnectionStringBuilder.DataSource = sServerName;

                if (!string.IsNullOrEmpty(sDatabaseName))
                    objSqlConnectionStringBuilder.InitialCatalog = sDatabaseName;

                if (_fUseSqlCredential || sUserName == null || sUserName == string.Empty)
                {
                    objSqlConnectionStringBuilder.Remove(sUSER_ID);
                    objSqlConnectionStringBuilder.Remove(sPASSWORD);

                    if (!_fUseSqlCredential)
                        objSqlConnectionStringBuilder.IntegratedSecurity = true;
                }
                else
                {
                    objSqlConnectionStringBuilder.UserID = sUserName;
                    objSqlConnectionStringBuilder.Password = sPassword;
                }

                _sSqlConnectionString = objSqlConnectionStringBuilder.ConnectionString;
            }
            finally
            {
                if (objSqlConnectionStringBuilder != null)
                {
                    objSqlConnectionStringBuilder.Clear();
                    objSqlConnectionStringBuilder = null;
                }
            }
        }

        /// <summary>Build sql credential object.</summary>
        /// <returns>Return a sql credential object.</returns>
        private SqlCredential objSqlCredentialBuilder()
        {
            if (string.IsNullOrEmpty(sPassword))
                return null;

            return new SqlCredential(sUserName, clsSecureString.ConvertToSecureString(sPassword));
        }

        #endregion
    }
}
