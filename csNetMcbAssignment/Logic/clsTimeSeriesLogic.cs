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
    /// <summary>Save time series to database.</summary>
    public class clsTimeSeriesLogic :IDisposable
    {
        #region Private declarations

        #region Enumerations

        /// <summary>Type of operation to perform on stored procedure.</summary>
        private enum enuStoredProcedureOperations
        {
            /// <summary>Save Time series.</summary>
            iINSERT_TIME_SERIES = 0,
        }

        #endregion

        #region Variables

        private clsConnection objConnection;

        #endregion

        #endregion

        #region Constructors

        /// <summary>Contructors.</summary>
        /// <param name="objConnection">Connection.</param>
        public clsTimeSeriesLogic(clsConnection objConnection)
        {
            this.objConnection = objConnection;
        }

        #endregion

        #region Private methods

        /// <summary>Create a list of sql parameters from time series model./summary>
        /// <param name="objTimeSeriesModel"> Object time series model.</param>
        /// <returns>List of sql parameters.</returns>
        private List<SqlParameter> CreateListParameters(clsTimeSeriesModel objTimeSeriesModel,
                                                        enuStoredProcedureOperations eOperationType)
        {
            return new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName = clsStringFormatting.sConvertToSqlParameter(clsTimeSeriesConstants.sTIME_SERIES_ID),
                    SqlDbType = SqlDbType.Int,
                    Value = objTimeSeriesModel.iTimeSeriesId,
                    Direction = ParameterDirection.InputOutput
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsTimeSeriesConstants.sYEARS),
                    SqlDbType = SqlDbType.Int,
                    Value = objTimeSeriesModel.iYear
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsTimeSeriesConstants.sSERIES_CODE),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objTimeSeriesModel.sSeriesCode
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsTimeSeriesConstants.sDESCRIPTIONS),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objTimeSeriesModel.sDescription
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
        /// <param name="objTimeSeriesModel">Time series model.</param>
        /// <returns>Series id.</returns>
        public int InsertTimeSeries(clsTimeSeriesModel objTimeSeriesModel)
        {
            // Declarations
            List<SqlParameter> lstParameters = null;
            string sPlainPassword = string.Empty;
            try
            {
                // Intialisations
                lstParameters = new List<SqlParameter>();


                lstParameters = CreateListParameters(objTimeSeriesModel,
                                                     enuStoredProcedureOperations.iINSERT_TIME_SERIES);

                objConnection.ExecuteStoredProcedureNonQuery(clsTimeSeriesConstants.sTIMES_SERIES_STORED_PROCEDURE,
                                                             lstParameters);

                objTimeSeriesModel.iTimeSeriesId = (int)lstParameters.AsEnumerable()
                                                                     .Where(x => x.ParameterName == clsStringFormatting.sConvertToSqlParameter(clsTimeSeriesConstants.sTIME_SERIES_ID))
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

            return objTimeSeriesModel.iTimeSeriesId;
        }
    }

    #endregion

}

