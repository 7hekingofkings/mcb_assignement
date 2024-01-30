using Common.Utility.Connection;
using Common.Utility.Formatting;
using mcb.main.Constants;
using mcb.main.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace mcb.main.Logic
{
    /// <summary>Save country series to database.</summary>
    public class clsCountrySeriesLogic :IDisposable
    {
        #region Private declarations

        #region Enumerations

        /// <summary>Type of operation to perform on stored procedure.</summary>
        private enum enuStoredProcedureOperations
        {
            /// <summary>Save country series.</summary>
            iINSERT_COUNTRY_SERIES = 0,
        }

        #endregion

        #region Variables

        private clsConnection objConnection;

        #endregion

        #endregion

        #region Constructors

        /// <summary>Contructors.</summary>
        /// <param name="objConnection">Connection.</param>
        public clsCountrySeriesLogic(clsConnection objConnection)
        {
            this.objConnection = objConnection;
        }

        #endregion

        #region Private methods

        /// <summary>Create a list of sql parameters from country series model./summary>
        /// <param name="objCountrySeriesModel"> Object country series model.</param>
        /// <returns>List of sql parameters.</returns>
        private List<SqlParameter> CreateListParameters(clsCountrySeriesModel objCountrySeriesModel,
                                                        enuStoredProcedureOperations eOperationType)
        {
            return new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName = clsStringFormatting.sConvertToSqlParameter(clsCountrySeriesConstants.sCOUNTRY_SERIES_ID),
                    SqlDbType = SqlDbType.Int,
                    Value = objCountrySeriesModel.iCountrySeriesId,
                    Direction = ParameterDirection.InputOutput
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountrySeriesConstants.sCOUNTRY_CODE),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountrySeriesModel.sCountryCode
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountrySeriesConstants.sSERIES_CODE),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountrySeriesModel.sSeriesCode
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountrySeriesConstants.sDESCRIPTIONS),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountrySeriesModel.sDescription
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

        /// <summary>Insert country series into database.</summary>
        /// <param name="objCountrySeriesModel">Country series model.</param>
        /// <returns>Series id.</returns>
        public int InsertCountrySeries(clsCountrySeriesModel objCountrySeriesModel)
        {
            // Declarations
            List<SqlParameter> lstParameters = null;
            string sPlainPassword = string.Empty;
            try
            {
                // Intialisations
                lstParameters = new List<SqlParameter>();


                lstParameters = CreateListParameters(objCountrySeriesModel,
                                                     enuStoredProcedureOperations.iINSERT_COUNTRY_SERIES);

                objConnection.ExecuteStoredProcedureNonQuery(clsCountrySeriesConstants.sCOUNTRY_SERIES_STORED_PROCEDURE,
                                                             lstParameters);

                objCountrySeriesModel.iCountrySeriesId = (int)lstParameters.AsEnumerable()
                                                                           .Where(x => x.ParameterName == clsStringFormatting.sConvertToSqlParameter(clsCountrySeriesConstants.sCOUNTRY_SERIES_ID))
                                                                           .Select(y => y.Value).FirstOrDefault();
            }
            finally
            {
                if (lstParameters != null)
                {
                    lstParameters.Clear();
                    lstParameters = null;
                }
            }

            return objCountrySeriesModel.iCountrySeriesId;
        }
    }

    #endregion
}
