using Common.Utility.Connection;
using Common.Utility.Formatting;
using mcb.main.Constants;
using mcb.main.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace mcb.main.Logic
{
    /// <summary>Save country to database.</summary>
    public class clsCountryLogic : IDisposable
    {
        #region Private declarations

        #region Enumerations

        /// <summary>Type of operation to perform on stored procedure.</summary>
        private enum enuStoredProcedureOperations
        {
            /// <summary>Save country.</summary>
            iINSERT_COUNTRY = 0,
        }

        #endregion

        #region Variables

        private clsConnection objConnection;

        #endregion

        #endregion

        #region Constructors

        /// <summary>Contructors.</summary>
        /// <param name="objConnection">Connection.</param>
        public clsCountryLogic(clsConnection objConnection)
        {
            this.objConnection = objConnection;
        }

        #endregion

        #region Private methods

        /// <summary>Create a list of sql parameters from country model./summary>
        /// <param name="objCountryModel"> Object country model.</param>
        /// <returns>List of sql parameters.</returns>
        private List<SqlParameter> CreateListParameters(clsCountryModel objCountryModel,
                                                        enuStoredProcedureOperations eOperationType)
        {
            return new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName = clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sCOUNTRY_ID),
                    SqlDbType = SqlDbType.Int,
                    Value = objCountryModel.iCountryId,
                    Direction = ParameterDirection.InputOutput
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sCOUNTRY_CODE),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sCountryCode
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sSHORT_NAME),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sShortName
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sTABLE_NAME),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sTableName
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sLONG_NAME),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sLongName
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sALPHA_CODE),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sAlphaCode
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sCURRENCY_UNIT),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sCurrencyUnit
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sSPECIAL_NOTES),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sSpecialNotes
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sREGION),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sRegion
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sINCOME_GROUP),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sIncomeGroup
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sWB_CODE),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sWDCode
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sNATIONAL_ACCOUNT_BASE_YEAR),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sNationalAccountBaseYear
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sNATIONAL_ACCOUNT_REFE_YEAR),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sNationalAccountReferenceYear
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sSNA_PRICE_VALUATION),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sSNAPriceValuation
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sLENDING_CATEGORY),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sLendingCategory
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sOTHER_GROUPS),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sOtherGroups
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sNATIONAL_ACCOUNTS),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sNationalAccounts
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sCONVERSION_FACTOR),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sConversionFactor
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sPPP_SURVEY_YEAR),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sPPPSurveyYear
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sBALANCE_PAYMENT_MANUAL),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sBalancePaymentManual
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sEXTERNAL_DEBT_STATUS),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sExternalDebtStatus
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sSYSTEM_TRADE),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sSystemTrade
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sACCOUNTING_CONCEPT),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sAccountingConcept
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sIMF_DATA),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sIMFData
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sPOPULATION_SENSUS),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sPopulationSensus
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sHOUSEHOLD_SURVEY),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sHouseHoldSurvey
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sINCOME_EXPENDITURE_DATA),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sIncomeExpenditureData
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sREGISTRATION_COMPLETE),
                    SqlDbType = SqlDbType.Bit,
                    Value = objCountryModel.fRegistrationComplete
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sAGRICULTURAL_CENSUS),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sAgriculturalCensus
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sINDUSTRIAL_DATA),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sIndustrialData
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sTRADE_DATA),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objCountryModel.sTradeData
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

        /// <summary>Insert country into database.</summary>
        /// <param name="objCountryModel">Country model.</param>
        /// <returns>Series id.</returns>
        public int InsertCountry(clsCountryModel objCountryModel)
        {
            // Declarations
            List<SqlParameter> lstParameters = null;
            string sPlainPassword = string.Empty;
            try
            {
                // Intialisations
                lstParameters = new List<SqlParameter>();


                lstParameters = CreateListParameters(objCountryModel,
                                                     enuStoredProcedureOperations.iINSERT_COUNTRY);

                objConnection.ExecuteStoredProcedureNonQuery(clsCountryConstants.sCOUNTRY_STORED_PROCEDURE,
                                                             lstParameters);

                objCountryModel.iCountryId = (int)lstParameters.AsEnumerable()
                                                               .Where(x => x.ParameterName == clsStringFormatting.sConvertToSqlParameter(clsCountryConstants.sCOUNTRY_ID))
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

            return objCountryModel.iCountryId;
        }
    }

    #endregion
}
