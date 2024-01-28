using Common.Utility.Connection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mcb.main.Model;
using Common.Utility.Formatting;
using mcb.main.Constants;
using Common.Utility.Security;

namespace mcb.main.Logic
{
    /// <summary>Save or retrieve setting from database.</summary>
    public class clsSettingLogic: IDisposable
    {
        #region Private declarations

        #region Enumerations

        /// <summary>Type of operation to perform on stored procedure.</summary>
        private enum enuStoredProcedureOperations
        {
            /// <summary>Save setting.</summary>
            iINSERT_SETTINGS = 0,

            /// <summary>Select  setting.</summary>
            iSELECT_SETTING = 1
        }

        #endregion

        #region Variables

        private clsConnection objConnection;

        #endregion

        #endregion

        #region Constructors

        /// <summary>Contructors.</summary>
        /// <param name="objConnection">Connection.</param>
        public clsSettingLogic(clsConnection objConnection)
        {
            this.objConnection = objConnection;
        }

        #endregion

        #region Private methods

        /// <summary>Create a list of sql parameters from MRA settings model./summary>
        /// <param name="objSettingModel"> Object  setting model.</param>
        /// <returns>List of sql parameters.</returns>
        private List<SqlParameter> CreateListParameters(clsSettingModel objSettingModel,
                                                        enuStoredProcedureOperations eOperationType)
        {
            return new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName = clsStringFormatting.sConvertToSqlParameter(clsSettingConstants.sSETTING_ID),
                    SqlDbType = SqlDbType.Int,
                    Value = objSettingModel.iSettingID,
                    Direction = ParameterDirection.InputOutput
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSettingConstants.sIMPORT_PATH),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSettingModel.sImportPath
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSettingConstants.sIMPORTED_PATH),
                    SqlDbType = SqlDbType.VarChar,
                    Value = objSettingModel.sImportedPath
                },

                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsSettingConstants.sEXECUTE_EVERY),
                    SqlDbType = SqlDbType.Int,
                    Value = objSettingModel.iExecuteEvery
                },
                             
                new SqlParameter()
                {
                    ParameterName =  clsStringFormatting.sConvertToSqlParameter(clsConstants.sPARAM_OPERATION_TYPE),
                    SqlDbType = SqlDbType.Int,
                    Value = (int)eOperationType
                }
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

        /// <summary>Insert update setting into database.</summary>
        /// <param name="objSettingModel">Setting model.</param>
        /// <returns>Setting id.</returns>
        public int InsertUpdateSetting(clsSettingModel objSettingModel)
        {
            // Declarations
            List<SqlParameter> lstParameters = null;
            string sPlainPassword = string.Empty;
            try
            {
                // Intialisations
                lstParameters = new List<SqlParameter>();


                lstParameters = CreateListParameters(objSettingModel,
                                                     enuStoredProcedureOperations.iINSERT_SETTINGS);

                objConnection.ExecuteStoredProcedureNonQuery(clsSettingConstants.sSETTING_STORED_PROCEDURE,
                                                             lstParameters);

                objSettingModel.iSettingID = (int)lstParameters.AsEnumerable()
                                                                     .Where(x => x.ParameterName == clsStringFormatting.sConvertToSqlParameter(clsSettingConstants.sSETTING_ID))
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

            return objSettingModel.iSettingID;
        }

        /// <summary>Select settings.</summary>
        /// <returns>Setting.</returns>
        public clsSettingModel SelectSetting()
        {
            // Declarations
            clsSettingModel objSettingModel = null;
            DataTable dtbResult = null;

            try
            {
                // Initialisations
                dtbResult = new DataTable();
                objSettingModel = new clsSettingModel()
                {
                    iSettingID = 0,
                    iExecuteEvery = 0,
                    sImportedPath = string.Empty,
                    sImportPath = string.Empty,
                };

                dtbResult = objConnection.ExecuteProcedureFillDataTable(clsSettingConstants.sSETTING_STORED_PROCEDURE,
                                                                        CreateListParameters(objSettingModel,
                                                                                             enuStoredProcedureOperations.iSELECT_SETTING));

                if (dtbResult == null ||
                    dtbResult.Rows.Count <= 0)
                    return null;

                return dtbResult.AsEnumerable()
                                .Select(objrow => new clsSettingModel()
                                {
                                    iSettingID = objrow.Field<int>(clsSettingConstants.sSETTING_ID),
                                    sImportPath = objrow.Field<string>(clsSettingConstants.sIMPORT_PATH),
                                    sImportedPath = objrow.Field<string>(clsSettingConstants.sIMPORTED_PATH),
                                    iExecuteEvery = objrow.Field<int>(clsSettingConstants.sEXECUTE_EVERY),
                                })
                                .FirstOrDefault();
            }
            finally
            {
                if (objSettingModel != null)
                    objSettingModel = null;

                if (dtbResult != null)
                {
                    dtbResult.Clear();
                    dtbResult.Dispose();
                    dtbResult = null;
                }

                if (objSettingModel == null)
                    objSettingModel = null;
            }
        }

        #endregion
    }
}
