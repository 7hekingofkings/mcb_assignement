using System;
using Common.Utility.Resources;
using static Common.Utility.Resources.clsConfiguration;
using static Common.Utility.Security.clsTripleDESCryptography;

namespace Common.Forms.Utility
{
    /// <summary>Create/read connection configurations.</summary>
    public class clsConnectionConfiguration : IDisposable
    {
        #region Private declarations

        #region Variables

        private clsConfiguration objConfiguration;
        private clsConnectionSection objConnectionSection;

        #endregion

        #endregion

        #region Constructors

        /// <summary>Consructor.</summary>
        public clsConnectionConfiguration()
        {
            objConfiguration = new clsConfiguration(enuApplicationType.DesktopApp);
            objConnectionSection = new clsConnectionSection();
        }

        #endregion

        #region Public Properties

        /// <summary>Server name.</summary>
        public string sServerName { get; set; }

        /// <summary>Database name.</summary>
        public string sDatabaseName { get; set; }

        /// <summary>User name.</summary>
        public string sUserName { get; set; }

        /// <summary>Password.</summary>
        public string sPassword { get; set; }

        /// <summary>Section to be created or retrieve.</summary>
        public string sSectionName { get; set; }

        #endregion

        #region Public methods

        /// <summary>Destroy objects.</summary>
        public void Dispose()
        {
            if (objConfiguration != null)
                objConfiguration = null;

            if (objConnectionSection != null)
                objConnectionSection = null;
        }

        /// <summary>Create Database configuration.</summary>
        public void CreateDatabaseConfig()
        {
            objConnectionSection.sServerName = sServerName;
            objConnectionSection.sDatabaseName = sDatabaseName;
            objConnectionSection.sUsername = sUserName;
            objConnectionSection.sPassword = string.IsNullOrEmpty(sPassword) ? sPassword : Encrypt(sPassword);

            objConfiguration.CreateConfigurationSection(sSectionName, objConnectionSection);
            objConfiguration.CreateAppSetting(sSectionName, sSectionName);
        }

        /// <summary>Retrieve the database values.</summary>
        /// <returns>True if exits.</returns>
        public bool GetDatabaseConfig()
        {
            objConnectionSection = objConfiguration.GetConfigurationSectionValues(sSectionName) as clsConnectionSection;
            if (objConnectionSection == null)
                return false;

            sServerName = objConnectionSection.sServerName;
            sDatabaseName = objConnectionSection.sDatabaseName;
            sUserName = objConnectionSection.sUsername;
            sPassword = string.IsNullOrEmpty(objConnectionSection.sPassword) ?
                        objConnectionSection.sPassword :
                        Decrypt(objConnectionSection.sPassword);

            return true;
        }

        /// <summary>Get all saved keys in app.config.</summary>
        /// <returns>Arrays of keys.</returns>
        public string[] GetAppSettingKeys()
        {
            return objConfiguration.GetAllAppSettingKeys();
        }

        /// <summary>Remove configuration for app.config.</summary>
        public void RemoveDatabaseConfig()
        {
            objConfiguration.RemoveSection(sSectionName);
            objConfiguration.Save();
        }

        #endregion
    }
}
