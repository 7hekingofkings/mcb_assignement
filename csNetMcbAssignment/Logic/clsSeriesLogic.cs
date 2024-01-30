using Common.Utility.Connection;
using Common.Utility.Formatting;
using mcb.main.Constants;
using mcb.main.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mcb.main.Logic
{
    /// <summary>Save series to database.</summary>
    public class clsSeriesLogic : IDisposable
    {
        #region Private declarations

        #region Enumerations

        /// <summary>Type of operation to perform on stored procedure.</summary>
        private enum enuStoredProcedureOperations
        {
            /// <summary>Save series.</summary>
            iINSERT_SERIES = 0,
        }

        #endregion

        #region Variables

        private clsConnection objConnection;

        #endregion

        #endregion

        #region Constructors

        /// <summary>Contructors.</summary>
        /// <param name="objConnection">Connection.</param>
        public clsSeriesLogic(clsConnection objConnection)
        {
            this.objConnection = objConnection;
        }

        #endregion

        #region Private methods

        /// <summary>Create a list of sql parameters from series model./summary>
        /// <param name="objSeriesModel"> Object series model.</param>
        /// <returns>List of sql parameters.</returns>
        private List<SqlParameter> CreateListParameters(clsSeriesModel objSeriesModel,
                                                        enuStoredProcedureOperations eOperationType)
        {
            return new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName = clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sSERIES_ID),
                    SqlDbType = SqlDbType.Int,
                    Value = objSeriesModel.iSeriesId,
                    Direction = ParameterDirection.InputOutput
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sSERIES_CODE),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sSeriesCode
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sTOPIC),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sTopic
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sINDICATOR_NAME),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sIndicatorName
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sSHORT_DEFINITION),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sShortDefinition
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sLONG_DEFINITION),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sLongDefinition
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sUNIT_MEASURE),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sUnitOfMeasure
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sPERIODICITY),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sPeriodicity
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sBASE_PERIOD),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sBasePeriod
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sOTHER_NOTES),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sOtherNotes
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sAGGREGATION_METHOD),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sAggregationMethod
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sLIMITATION_EXCEPTIONS),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sLimitationExceptions
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sORIGINAL_SOURCE_NOTES),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sOriginalSourceNotes
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sGENERAL_COMMENTS),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sGeneralComments
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sSOURCES),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sSources
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sSTATISTICAL_METHODOLOGY),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sStatisticalMethodology
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sDEVLOPMENT_RELEVANCE),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sDevelopmentRelevance
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sSOURCE_LINKS),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sSourceLinks
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sWEB_LINKS),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sWebLinks
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sRELATED_INDICATORS),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sRelatedIndicators
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sLICENSE_TYPE),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSeriesModel.sLicenseType
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

        /// <summary>Insert series into database.</summary>
        /// <param name="objSeriesModel">Series model.</param>
        /// <returns>Series id.</returns>
        public int InsertSeries(clsSeriesModel objSeriesModel)
        {
            // Declarations
            List<SqlParameter> lstParameters = null;
            string sPlainPassword = string.Empty;
            try
            {
                // Intialisations
                lstParameters = new List<SqlParameter>();


                lstParameters = CreateListParameters(objSeriesModel,
                                                     enuStoredProcedureOperations.iINSERT_SERIES);

                objConnection.ExecuteStoredProcedureNonQuery(clsSeriesConstants.sSERIES_STORED_PROCEDURE,
                                                             lstParameters);

                objSeriesModel.iSeriesId = (int)lstParameters.AsEnumerable()
                                                             .Where(x => x.ParameterName == clsStringFormatting.sConvertToSqlParameter(clsSeriesConstants.sSERIES_ID))
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

            return objSeriesModel.iSeriesId;
        }
    }

    #endregion
}
