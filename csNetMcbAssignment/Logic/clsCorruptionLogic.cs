using Common.Utility.Connection;
using Common.Utility.Formatting;
using mcb.main.Constants;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mcb.main.Logic
{
    /// <summary>Save corruption to database</summary>
    public class clsCorruptionLogic :IDisposable
    {
        #region Private declarations

        #region Enumerations

        /// <summary>Type of operation to perform on stored procedure.</summary>
        private enum enuStoredProcedureOperations
        {
            /// <summary>Save data.</summary>
            iINSERT = 0,
        }

        #endregion

        #region Variables

        private clsConnection objConnection;

        #endregion

        #endregion

        #region Constructors

        /// <summary>Contructors.</summary>
        /// <param name="objConnection">Connection.</param>
        public clsCorruptionLogic(clsConnection objConnection)
        {
            this.objConnection = objConnection;
        }

        #endregion

        #region Private methods

        /// <summary>Create a list of sql parameters from time series model./summary>
        /// <param name="dtbData">Corruption table.</param>
        /// <returns>List of sql parameters.</returns>
        private List<SqlParameter> CreateListParameters(DataTable dtbData,
                                                        enuStoredProcedureOperations eOperationType)
        {
            return new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName = clsStringFormatting.sConvertToSqlParameter(clsCorruptionConstants.sDATA),
                    SqlDbType = SqlDbType.Structured,
                    Value = dtbData,
                },
            };
        }

        #endregion

        #region Public methods

        /// <summary>Destroy objects.</summary>
        public void Dispose()
        {
            if (objConnection != null)
                objConnection = null;
        }

        /// <summary>Insert data into database.</summary>
        /// <param name="dtbData">Datatable data.</param>

        public void InsertData(DataTable dtbData)
        {
            // Declarations
            List<SqlParameter> lstParameters = null;
            string sPlainPassword = string.Empty;
            try
            {
                // Intialisations
                lstParameters = new List<SqlParameter>();


                lstParameters = CreateListParameters(dtbData,
                                                     enuStoredProcedureOperations.iINSERT);

                objConnection.ExecuteStoredProcedureNonQuery(clsCorruptionConstants.sCORRUPTION_STORED_PROCEDURE,
                                                             lstParameters);
            }
            finally
            {
                if (lstParameters != null)
                {
                    lstParameters.Clear();
                    lstParameters = null;
                }
            }
        }
    }

    #endregion
}
