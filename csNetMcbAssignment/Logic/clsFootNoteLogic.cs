using Common.Utility.Connection;
using Common.Utility.Formatting;
using mcb.main.Constants;
using mcb.main.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;


namespace mcb.main.Logic
{
    /// <summary>Save foot note to database.</summary>
    public class clsFootNoteLogic: IDisposable
    {
        #region Private declarations

        #region Enumerations

        /// <summary>Type of operation to perform on stored procedure.</summary>
        private enum enuStoredProcedureOperations
        {
            /// <summary>Save foot note.</summary>
            iINSERT_FOOT_NOTE = 0,
        }

        #endregion

        #region Variables

        private clsConnection objConnection;

        #endregion

        #endregion

        #region Constructors

        /// <summary>Contructors.</summary>
        /// <param name="objConnection">Connection.</param>
        public clsFootNoteLogic(clsConnection objConnection)
        {
            this.objConnection = objConnection;
        }

        #endregion

        #region Private methods

        /// <summary>Create a list of sql parameters from time series model./summary>
        /// <param name="dtbFootNote">Foot note table.</param>
        /// <returns>List of sql parameters.</returns>
        private List<SqlParameter> CreateListParameters(DataTable dtbFootNote,
                                                        enuStoredProcedureOperations eOperationType)
        {
            return new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName = clsStringFormatting.sConvertToSqlParameter(clsFootNoteConstants.sFOOT_NOTE),
                    SqlDbType = SqlDbType.Structured,
                    Value = dtbFootNote,
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

        /// <summary>Insert times series into database.</summary>
        /// <param name="dtbFootNote">Datatable foot note.</param>
        /// <returns>Series id.</returns>
        public void InsertFootNote(DataTable dtbFootNote)
        {
            // Declarations
            List<SqlParameter> lstParameters = null;
            string sPlainPassword = string.Empty;
            try
            {
                // Intialisations
                lstParameters = new List<SqlParameter>();


                lstParameters = CreateListParameters(dtbFootNote,
                                                     enuStoredProcedureOperations.iINSERT_FOOT_NOTE);

                objConnection.ExecuteStoredProcedureNonQuery(clsFootNoteConstants.sFOOT_NOTE_STORED_PROCEDURE,
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
