using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Utility.Connection
{
    /// <summary>Create a connection to database.</summary>
    public class clsConnection
    {
        #region Private declarations

        private SqlConnection objSqlConnection;
        private SqlCommand objSqlCommand;
        private SqlTransaction objSqlTransaction;

        #endregion

        #region Public properties

        /// <summary>A delay before the connection timeout.</summary>
        public int? niTimeOut { get; set; }

        /// <summary>Connection details.</summary>
        public clsSQLConnectionStringBuilder objSQLConnectionStringBuilder { set; get; }

        #endregion

        #region Constructors

        /// <summary>Constructor.</summary>
        public clsConnection() { }

        #endregion

        #region Public methods

        /// <summary>Open a connection to the database.</summary>
        public void OpenConnection()
        {
            try
            {
                if (objSQLConnectionStringBuilder == null ||
                    string.IsNullOrEmpty(objSQLConnectionStringBuilder.sSqlConnectionString))
                    return;

                objSqlConnection = new SqlConnection(objSQLConnectionStringBuilder.sSqlConnectionString);

                objSqlConnection.Open();
            }
            catch (SqlException eSqlException)
            {
                throw eSqlException;
            }
        }

        /// <summary>Open an asynchronous connection to the database.</summary>
        public void OpenConnectionAsync()
        {
            try
            {
                if (objSQLConnectionStringBuilder == null ||
                    string.IsNullOrEmpty(objSQLConnectionStringBuilder.sSqlConnectionString))
                    return;

                objSqlConnection = new SqlConnection(objSQLConnectionStringBuilder.sSqlConnectionString);

                objSqlConnection.OpenAsync();
            }
            catch (SqlException eSqlException)
            {
                throw eSqlException;
            }
        }

        /// <summary>Destroy the current connection.</summary>
        public void CloseConnection()
        {
            if (objSqlConnection != null)
            {
                if (objSqlConnection.State != ConnectionState.Closed)
                    objSqlConnection.Close();

                objSqlConnection.Dispose();
                objSqlConnection = null;
            }
        }

        /// <summary>A method to execute queries in database.</summary>
        /// <param name="sQuery">Sql query to be executed.</param>
        public void ExecuteNonQueries(string sQuery)
        {
            try
            {
                // Initialisations
                objSqlCommand = new SqlCommand(sQuery, objSqlConnection);

                if (niTimeOut.HasValue && niTimeOut > 0)
                    objSqlCommand.CommandTimeout = niTimeOut.Value;

                if (objSqlTransaction != null)
                    objSqlCommand.Transaction = objSqlTransaction;

                objSqlCommand.ExecuteNonQuery();
            }
            finally
            {
                niTimeOut = null;

                if (objSqlCommand != null)
                {
                    objSqlCommand.Dispose();
                    objSqlCommand = null;
                }
            }
        }

        /// <summary>A method to execute queries in database asynchronously.</summary>
        /// <param name="sQuery">Sql query to be executed.</param>
        public async Task ExecuteNonQueriesAsync(string sQuery)
        {
            try
            {
                // Initialisations
                objSqlCommand = new SqlCommand(sQuery, objSqlConnection);

                if (niTimeOut.HasValue && niTimeOut > 0)
                    objSqlCommand.CommandTimeout = niTimeOut.Value;

                if (objSqlTransaction != null)
                    objSqlCommand.Transaction = objSqlTransaction;

                await objSqlCommand.ExecuteNonQueryAsync();
            }
            finally
            {
                niTimeOut = null;

                if (objSqlCommand != null)
                {
                    objSqlCommand.Dispose();
                    objSqlCommand = null;
                }
            }
        }

        /// <summary>A method to retrieve data from sql database.</summary>
        /// <param name="sQuery">Sql query to be executed.</param>
        public SqlDataReader DataReader(string sQuery)
        {
            // Declarations
            SqlDataReader objSqlDataReader = null;

            try
            {
                objSqlCommand = new SqlCommand(sQuery, objSqlConnection);
                objSqlCommand.CommandType = CommandType.Text;

                if (niTimeOut.HasValue && niTimeOut > 0)
                    objSqlCommand.CommandTimeout = niTimeOut.Value;

                if (objSqlTransaction != null)
                    objSqlCommand.Transaction = objSqlTransaction;

                objSqlDataReader = objSqlCommand.ExecuteReader();
            }
            finally
            {
                niTimeOut = null;

                if (objSqlCommand != null)
                {
                    objSqlCommand.Dispose();
                    objSqlCommand = null;
                }
            }
            return objSqlDataReader;
        }

        /// <summary>A method to retrieve data from sql database asynchronously.</summary>
        /// <param name="sQuery">Sql query to be executed.</param>
        public async Task<SqlDataReader> DataReaderAsync(string sQuery)
        {
            // Declarations
            SqlDataReader objSqlDataReader = null;

            try
            {
                objSqlCommand = new SqlCommand(sQuery, objSqlConnection);
                objSqlCommand.CommandType = CommandType.Text;

                if (niTimeOut.HasValue && niTimeOut > 0)
                    objSqlCommand.CommandTimeout = niTimeOut.Value;

                if (objSqlTransaction != null)
                    objSqlCommand.Transaction = objSqlTransaction;

                objSqlDataReader = await objSqlCommand.ExecuteReaderAsync();
            }
            finally
            {
                niTimeOut = null;

                if (objSqlCommand != null)
                {
                    objSqlCommand.Dispose();
                    objSqlCommand = null;
                }
            }
            return objSqlDataReader;
        }

        /// <summary>A method to retrieve data from sql database using data adapter.</summary>
        /// <param name="sQuery">Sql query to be executed.</param>
        /// <returns>Returns a datatable.</returns>
        public DataTable FillDataTable(string sQuery)
        {
            // Declarations
            SqlDataAdapter objSqlDataAdapter = null;
            DataTable dtbReturnTable;

            try
            {
                // Initialisation
                dtbReturnTable = new DataTable();
                objSqlDataAdapter = new SqlDataAdapter(sQuery, objSqlConnection);

                if (objSqlTransaction != null)
                    objSqlDataAdapter.SelectCommand.Transaction = objSqlTransaction;

                objSqlDataAdapter.Fill(dtbReturnTable);
            }
            finally
            {
                if (objSqlDataAdapter != null)
                {
                    objSqlDataAdapter.Dispose();
                    objSqlDataAdapter = null;
                }
            }

            return dtbReturnTable;
        }

        /// <summary>A method to retrieve data from sql database using data adapter asynchronously.</summary>
        /// <param name="sQuery">Sql query to be executed.</param>
        /// <returns>Returns a datatable.</returns>
        public async Task<DataTable> FillDataTableAsync(string sQuery)
        {
            // Declarations
            SqlDataAdapter objSqlDataAdapter = null;
            DataTable dtbReturnTable;

            try
            {
                // Initialisation
                dtbReturnTable = new DataTable();
                objSqlDataAdapter = new SqlDataAdapter(sQuery, objSqlConnection);

                if (objSqlTransaction != null)
                    objSqlDataAdapter.SelectCommand.Transaction = objSqlTransaction;

                await Task.Run(() => objSqlDataAdapter.Fill(dtbReturnTable));
            }
            finally
            {
                if (objSqlDataAdapter != null)
                {
                    objSqlDataAdapter.Dispose();
                    objSqlDataAdapter = null;
                }
            }

            return dtbReturnTable;
        }

        /// <summary>Execute scalar.</summary>
        /// <typeparam name="T">Type T</typeparam>
        /// <param name="sQuery">Query string to be executed.</param>
        /// <returns>Returns a value in type T.</returns>
        public T ExecuteScalar<T>(string sQuery)
        {
            // Declarations
            T tReturnValue = default(T);

            try
            {
                // Initialisations
                objSqlCommand = new SqlCommand(sQuery, objSqlConnection);

                if (niTimeOut.HasValue && niTimeOut > 0)
                    objSqlCommand.CommandTimeout = niTimeOut.Value;

                if (objSqlTransaction != null)
                    objSqlCommand.Transaction = objSqlTransaction;

                if (objSqlCommand.ExecuteScalar() != null)
                    tReturnValue = (T)objSqlCommand.ExecuteScalar();
            }
            finally
            {
                niTimeOut = null;

                if (objSqlCommand != null)
                {
                    objSqlCommand.Dispose();
                    objSqlCommand = null;
                }
            }

            return tReturnValue;
        }

        /// <summary>Execute scalar asynchronously.</summary>
        /// <typeparam name="T">Type T</typeparam>
        /// <param name="sQuery">Query string to be executed.</param>
        /// <returns>Returns a value in type T.</returns>
        public async Task<T> ExecuteScalarAsync<T>(string sQuery)
        {
            // Declarations
            T tReturnValue = default(T);

            try
            {
                // Initialisations
                objSqlCommand = new SqlCommand(sQuery, objSqlConnection);

                if (niTimeOut.HasValue && niTimeOut > 0)
                    objSqlCommand.CommandTimeout = niTimeOut.Value;

                if (objSqlTransaction != null)
                    objSqlCommand.Transaction = objSqlTransaction;

                if (objSqlCommand.ExecuteScalar() != null)
                    tReturnValue = (T) await objSqlCommand.ExecuteScalarAsync();
            }
            finally
            {
                niTimeOut = null;

                if (objSqlCommand != null)
                {
                    objSqlCommand.Dispose();
                    objSqlCommand = null;
                }
            }

            return tReturnValue;
        }

        /// <summary>Start a connection transaction.</summary>
        public void BeginTransaction()
        {
            if (objSqlConnection != null)
                objSqlTransaction = objSqlConnection.BeginTransaction();
        }

        /// <summary>Commits the connection transactions.</summary>
        public void CommitTransaction()
        {
            if (objSqlTransaction != null)
            {
                objSqlTransaction.Commit();
                DestroyTransaction();
            }
        }

        /// <summary>Roll back a connection from pending state.</summary>
        public void RollBackTransaction()
        {
            if (objSqlTransaction != null)
            {
                objSqlTransaction.Rollback();
                DestroyTransaction();
            }
        }

        /// <summary>Execute a script in the the database.</summary>
        /// <param name="sScript">The data of the scripts as string.</param>
        public void ExecuteSqlScripts(string sScript)
        {
            // Declarations
            List<string> lstQueries = null;

            try
            {
                // Initialisations
                lstQueries = new List<string>(Regex.Split(sScript,
                                                          @"^\s*GO\s*$",
                                                          RegexOptions.Multiline | RegexOptions.IgnoreCase));
                lstQueries.Remove(string.Empty);

                foreach (string sQuery in lstQueries)
                {
                    objSqlCommand = new SqlCommand(sQuery, objSqlConnection);
                    objSqlCommand.CommandType = CommandType.Text;

                    if (niTimeOut.HasValue && niTimeOut > 0)
                        objSqlCommand.CommandTimeout = niTimeOut.Value;

                    if (objSqlTransaction != null)
                        objSqlCommand.Transaction = objSqlTransaction;

                    objSqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                niTimeOut = null;

                if (objSqlCommand != null)
                {
                    objSqlCommand.Dispose();
                    objSqlCommand = null;
                }

                if (lstQueries != null)
                    lstQueries = null;
            }
        }

        /// <summary>Execute a script in the the database asynchronously.</summary>
        /// <param name="sScript">The data of the scripts as string.</param>
        public async Task ExecuteSqlScriptsAsync(string sScript)
        {
            // Declarations
            List<string> lstQueries = null;

            try
            {
                // Initialisations
                lstQueries = new List<string>(Regex.Split(sScript,
                                                          @"^\s*GO\s*$",
                                                          RegexOptions.Multiline | RegexOptions.IgnoreCase));
                lstQueries.Remove(string.Empty);

                foreach (string sQuery in lstQueries)
                {
                    objSqlCommand = new SqlCommand(sQuery, objSqlConnection);
                    objSqlCommand.CommandType = CommandType.Text;

                    if (niTimeOut.HasValue && niTimeOut > 0)
                        objSqlCommand.CommandTimeout = niTimeOut.Value;

                    if (objSqlTransaction != null)
                        objSqlCommand.Transaction = objSqlTransaction;

                    await objSqlCommand.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                niTimeOut = null;

                if (objSqlCommand != null)
                {
                    objSqlCommand.Dispose();
                    objSqlCommand = null;
                }

                if (lstQueries != null)
                    lstQueries = null;
            }
        }

        /// <summary>Execute a stored procedure without returing anything.</summary>
        /// <param name="sProcedureName">Stored procedure name.</param>
        /// <param name="lstSqlParameter">List of parameters.</param>
        public void ExecuteStoredProcedureNonQuery(string sProcedureName,
                                                   List<SqlParameter> lstSqlParameter)
        {
            try
            {
                // Initialisations
                objSqlCommand = new SqlCommand(sProcedureName, objSqlConnection);
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.Parameters.AddRange(lstSqlParameter.ToArray());

                if (niTimeOut.HasValue && niTimeOut > 0)
                    objSqlCommand.CommandTimeout = niTimeOut.Value;

                if (objSqlTransaction != null)
                    objSqlCommand.Transaction = objSqlTransaction;

                objSqlCommand.ExecuteNonQuery();
            }
            finally
            {
                niTimeOut = null;

                if (objSqlCommand != null)
                {
                    objSqlCommand.Dispose();
                    objSqlCommand = null;
                }
            }
        }

        /// <summary>Execute a stored procedure without returing anything asynchronously.</summary>
        /// <param name="sProcedureName">Stored procedure name.</param>
        /// <param name="lstSqlParameter">List of parameters.</param>
        public async Task ExecuteStoredProcedureNonQueryAsync(string sProcedureName,
                                                              List<SqlParameter> lstSqlParameter)
        {
            try
            {
                // Initialisations
                objSqlCommand = new SqlCommand(sProcedureName, objSqlConnection);
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.Parameters.AddRange(lstSqlParameter.ToArray());

                if (niTimeOut.HasValue && niTimeOut > 0)
                    objSqlCommand.CommandTimeout = niTimeOut.Value;

                if (objSqlTransaction != null)
                    objSqlCommand.Transaction = objSqlTransaction;

               await objSqlCommand.ExecuteNonQueryAsync();
            }
            finally
            {
                niTimeOut = null;

                if (objSqlCommand != null)
                {
                    objSqlCommand.Dispose();
                    objSqlCommand = null;
                }
            }
        }

        /// <summary>Execute a stored procedure.</summary>
        /// <param name="sProcedureName">Stored procedure name.</param>
        /// <param name="lstSqlParameter">List of parameters.</param>
        /// <return>return a data table.</return>
        public DataTable ExecuteProcedureFillDataTable(string sProcedureName,
                                                       List<SqlParameter> lstSqlParameter)
        {
            // Declarations
            DataTable dtbResult;

            try
            {
                // Initialisations
                dtbResult = new DataTable();
                objSqlCommand = new SqlCommand(sProcedureName, objSqlConnection);
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.Parameters.AddRange(lstSqlParameter.ToArray());

                if (niTimeOut.HasValue && niTimeOut > 0)
                    objSqlCommand.CommandTimeout = niTimeOut.Value;

                if (objSqlTransaction != null)
                    objSqlCommand.Transaction = objSqlTransaction;

                using (SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(objSqlCommand))
                {
                    objSqlDataAdapter.Fill(dtbResult);
                }
            }
            finally
            {
                niTimeOut = null;

                if (objSqlCommand != null)
                {
                    objSqlCommand.Dispose();
                    objSqlCommand = null;
                }
            }

            return dtbResult;
        }

        /// <summary>Execute a stored procedure asynchronously.</summary>
        /// <param name="sProcedureName">Stored procedure name.</param>
        /// <param name="lstSqlParameter">List of parameters.</param>
        /// <return>return a data table.</return>
        public async Task<DataTable> ExecuteProcedureFillDataTableAsync(string sProcedureName,
                                                                        List<SqlParameter> lstSqlParameter)
        {
            // Declarations
            DataTable dtbResult;

            try
            {
                // Initialisations
                dtbResult = new DataTable();
                objSqlCommand = new SqlCommand(sProcedureName, objSqlConnection);
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.Parameters.AddRange(lstSqlParameter.ToArray());

                if (niTimeOut.HasValue && niTimeOut > 0)
                    objSqlCommand.CommandTimeout = niTimeOut.Value;

                if (objSqlTransaction != null)
                    objSqlCommand.Transaction = objSqlTransaction;

                using (SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(objSqlCommand))
                {
                    await Task.Run(() => objSqlDataAdapter.Fill(dtbResult));
                }
            }
            finally
            {
                niTimeOut = null;

                if (objSqlCommand != null)
                {
                    objSqlCommand.Dispose();
                    objSqlCommand = null;
                }
            }

            return dtbResult;
        }

        /// <summary>Execute a stored procedure scalar.</summary>
        /// <typeparam name="T">Type T</typeparam>
        /// <param name="sStoredProcedure">Stored procedure name.</param>
        /// <param name="lstSqlParameter">List of sql parameters.</param>
        /// <returns>Returnavalue in type T.</returns>
        public T ExecuteStoredProceduceScalar<T>(string sStoredProcedure,
                                                 List<SqlParameter> lstSqlParameter)
        {
            // Declarations
            T tReturnValue = default(T);

            try
            {
                // Initialisations
                objSqlCommand = new SqlCommand(sStoredProcedure, objSqlConnection);
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.Parameters.AddRange(lstSqlParameter.ToArray());

                if (niTimeOut.HasValue && niTimeOut > 0)
                    objSqlCommand.CommandTimeout = niTimeOut.Value;

                if (objSqlTransaction != null)
                    objSqlCommand.Transaction = objSqlTransaction;

                if (objSqlCommand.ExecuteScalar() != null)
                    tReturnValue = (T)objSqlCommand.ExecuteScalar();
            }
            finally
            {
                niTimeOut = null;

                if (objSqlCommand != null)
                {
                    objSqlCommand.Dispose();
                    objSqlCommand = null;
                }
            }

            return tReturnValue;
        }

        /// <summary>Execute a stored procedure scalar asynchronously.</summary>
        /// <typeparam name="T">Type T</typeparam>
        /// <param name="sStoredProcedure">Stored procedure name.</param>
        /// <param name="lstSqlParameter">List of sql parameters.</param>
        /// <returns>Returnavalue in type T.</returns>
        public async Task<T> ExecuteStoredProceduceScalarAsync<T>(string sStoredProcedure,
                                                             List<SqlParameter> lstSqlParameter)
        {
            // Declarations
            T tReturnValue = default(T);

            try
            {
                // Initialisations
                objSqlCommand = new SqlCommand(sStoredProcedure, objSqlConnection);
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.Parameters.AddRange(lstSqlParameter.ToArray());

                if (niTimeOut.HasValue && niTimeOut > 0)
                    objSqlCommand.CommandTimeout = niTimeOut.Value;

                if (objSqlTransaction != null)
                    objSqlCommand.Transaction = objSqlTransaction;

                if (objSqlCommand.ExecuteScalar() != null)
                    tReturnValue = (T) await objSqlCommand.ExecuteScalarAsync();
            }
            finally
            {
                niTimeOut = null;

                if (objSqlCommand != null)
                {
                    objSqlCommand.Dispose();
                    objSqlCommand = null;
                }
            }

            return tReturnValue;
        }

        #endregion

        #region Private methods

        /// <summary>Destroy the object transaction.</summary>
        private void DestroyTransaction()
        {
            if (objSqlTransaction != null)
            {
                objSqlTransaction.Dispose();
                objSqlTransaction = null;
            }
        }

        #endregion

    }
}
