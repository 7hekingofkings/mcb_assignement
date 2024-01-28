using Common.Utility.Connection;
using Common.Utility.Resources;
using System;
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

        private const string sSP_SETTING = "spSetting.sql";
        private const string sSP_COUNTRY = "spCountry.sql";
              
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
            iSP_SETTING = 3

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
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}
