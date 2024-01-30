using Common.Utility.Connection;
using Common.Utility.Resources;
using System;
using System.CodeDom;
using System.Windows.Forms;

namespace Utilities.Scripts
{
    /// <summary>Create update setting scripts.</summary>
    public class clsScripts : IDisposable
    {
        #region Private declarations

        #region Constants

        private const string sASSEMBLY_NAME = "csNetMcbAssignment";
        private const string sUPDATE_SCRIPT_NAMESPACE = "mcb.main.Scripts.UpdateScripts";
        private const string sSTORED_PRODUCES_NAMESPACE = "mcb.main.Scripts.StoredProcedures";

        private const string sUD_WDI_COUNTRY_TABLE = "udCreateWDICountryTable.sql";
        private const string sUD_WDI_SERIES_TABLE = "udCreateWDISeriesTable.sql";
        private const string sUD_SETTING_TABLE = "udCreateSettingTable.sql";
        private const string sUD_WDI_COUNTRY_SERIES_TABLE = "udCreateWDICountrySeriesTable.sql";
        private const string sUD_WDI_PERIOD_TABLE = "udCreateWDIPeriodTable.sql";
        private const string sUD_WDI_TIME_SERIES = "udCreateWDITimeSeries.sql";
        private const string sUD_WDI_FOOT_NOTE = "udCreateWDIFootNoteTable.sql";
        private const string sUD_WDI_DATA = "udCreateWDIDataTable.sql";

        private const string sSP_SETTING = "spSetting.sql";
        private const string sSP_COUNTRY = "spCountry.sql";
        private const string sSP_SERIES = "spSeries.sql";
        private const string sSP_COUNTRY_SERIES = "spCountrySeries.sql";
        private const string sSP_TIME_SERIES = "spTimeSeries.sql";
        private const string sSP_FOOT_NOTE = "spFootNote.sql";
        private const string sSP_DATA = "spData.sql";


        private const string sTT_FOOT_NOTE = "ttFootNote.sql";
        private const string sTT_DATA = "ttWDIData.sql";


        #endregion

        #region Variables

        private clsConnection objConnection;

        #endregion

        #endregion

        #region Public declarations

        /// <summary>Script types.</summary>
        public enum enuScripts
        {
            /// <summary>Create country table.</summary>
            iUD_CREATE_WDI_COUNTRY_TABLE = 0,

            /// <summary>Create series table.</summary>
            iUD_CREATE_WDI_SERIES_TABLE = 1,

            /// <summary>Create setting table. </summary>
            iUD_CREATE_SETTING_TABLE = 2,

            /// <summary>Create setting procedure.</summary>
            iSP_SETTING = 3,

            /// <summary>Create procedure for country.</summary>
            iSP_COUNTRY = 4,

            /// <summary>Create procedure for series.</summary>
            iSP_SERIES = 5,

            /// <summary>Create country series table.</summary>
            iUD_CREATE_WDI_COUNTRY_SERIES_TABLE = 6,

            /// <summary>Create procedure for country series.</summary>
            iSP_COUNTRY_SERIES = 7,

            /// <summary>Create period table.</summary>
            iUD_CREATE_WDI_PERIOD = 8,

            /// <summary>Create table time series.</summary>
            iUD_CREATE_WDI_TIME_SERIES = 9,

            /// <summary>Create procedure time series.</summary>
            iSP_TIME_SERIES = 10,

            /// <summary>Create procedure for foot note.</summary>
            iUD_CREATE_WDI_FOOT_NOTE = 11,

            /// <summary>Create procedure for foot note.</summary>
            iSP_FOOT_NOTE = 12,

            /// <summary>Create table type foot note.</summary>
            iTT_FOOT_NOTE = 13 ,

            /// <summary>Create wdi data table.</summary>
            iUD_CREATE_WDI_DATA = 14,

            /// <summary>Create table type for data.</summary>
            iTT_DATA = 15,

            /// <summary>Create stored procedure for data</summary>
            iSP_DATA = 16,
        }

        #endregion

        #region Constructors

        /// <summary>Constructor. </summary>
        public clsScripts() { }

        /// <summary>Constructor.</summary>
        /// <param name="objConnection">Connection to database.</param>
        public clsScripts(clsConnection objConnection)
        {
            this.objConnection = objConnection;
        }

        #endregion

        #region Public methods

        /// <summary>Destroy objects.</summary>
        public void Dispose()
        {
            if (objConnection != null)
                objConnection = null;
        }

        /// <summary>Execute a script into database.</summary>
        /// <param name="eScript">Script type.</param>
        public void ExecuteScript(enuScripts eScript)
        {
            objConnection.ExecuteSqlScripts(pGetScriptData(eScript));
        }

        /// <summary>Get the script codes.</summary>
        /// <param name="eScripts">Script type.</param>
        /// <returns>return script code.</returns>
        public string pGetScriptData(enuScripts eScripts)
        {
            return clsEmbeddedResources.GetEmbeddedResources(sASSEMBLY_NAME, pGetScriptPath(eScripts));
        }

        #endregion

        #region Private methods

        /// <summary>Generate the path of the scripts.</summary>
        /// <param name="eScripts">Script type.</param>
        /// <returns>Path of the script as string.</returns>
        private string pGetScriptPath(enuScripts eScripts)
        {
            switch (eScripts)
            {
                case enuScripts.iUD_CREATE_WDI_COUNTRY_TABLE:
                    return string.Format("{0}.{1}", sUPDATE_SCRIPT_NAMESPACE, sUD_WDI_COUNTRY_TABLE);
                case enuScripts.iUD_CREATE_WDI_SERIES_TABLE:
                    return string.Format("{0}.{1}", sUPDATE_SCRIPT_NAMESPACE, sUD_WDI_SERIES_TABLE);
                case enuScripts.iUD_CREATE_SETTING_TABLE:
                    return string.Format("{0}.{1}", sUPDATE_SCRIPT_NAMESPACE, sUD_SETTING_TABLE);
                case enuScripts.iSP_SETTING:
                    return string.Format("{0}.{1}", sSTORED_PRODUCES_NAMESPACE, sSP_SETTING);
                case enuScripts.iSP_COUNTRY:
                    return string.Format("{0}.{1}", sSTORED_PRODUCES_NAMESPACE, sSP_COUNTRY);
                case enuScripts.iSP_SERIES:
                    return string.Format("{0}.{1}", sSTORED_PRODUCES_NAMESPACE, sSP_SERIES);
                case enuScripts.iUD_CREATE_WDI_COUNTRY_SERIES_TABLE:
                    return string.Format("{0}.{1}", sUPDATE_SCRIPT_NAMESPACE, sUD_WDI_COUNTRY_SERIES_TABLE);
                case enuScripts.iSP_COUNTRY_SERIES:
                    return string.Format("{0}.{1}", sSTORED_PRODUCES_NAMESPACE, sSP_COUNTRY_SERIES);
                case enuScripts.iUD_CREATE_WDI_PERIOD:
                    return string.Format("{0}.{1}", sUPDATE_SCRIPT_NAMESPACE, sUD_WDI_PERIOD_TABLE);
                case enuScripts.iUD_CREATE_WDI_TIME_SERIES:
                    return string.Format("{0}.{1}", sUPDATE_SCRIPT_NAMESPACE, sUD_WDI_TIME_SERIES);
                case enuScripts.iSP_TIME_SERIES:
                    return string.Format("{0}.{1}", sSTORED_PRODUCES_NAMESPACE, sSP_TIME_SERIES);
                case enuScripts.iUD_CREATE_WDI_FOOT_NOTE:
                    return string.Format("{0}.{1}", sUPDATE_SCRIPT_NAMESPACE, sUD_WDI_FOOT_NOTE);
                case enuScripts.iSP_FOOT_NOTE:
                    return string.Format("{0}.{1}", sSTORED_PRODUCES_NAMESPACE, sSP_FOOT_NOTE);
                case enuScripts.iTT_FOOT_NOTE:
                    return string.Format("{0}.{1}", sSTORED_PRODUCES_NAMESPACE, sTT_FOOT_NOTE);
                case enuScripts.iUD_CREATE_WDI_DATA:
                    return string.Format("{0}.{1}", sUPDATE_SCRIPT_NAMESPACE, sUD_WDI_DATA);
                case enuScripts.iTT_DATA:
                    return string.Format("{0}.{1}", sSTORED_PRODUCES_NAMESPACE, sTT_DATA);
                case enuScripts.iSP_DATA:
                    return string.Format("{0}.{1}", sSTORED_PRODUCES_NAMESPACE, sSP_DATA);
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}
