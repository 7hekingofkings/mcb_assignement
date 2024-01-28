using System.Configuration;
using System.Web.Configuration;

namespace Common.Utility.Resources
{
    /// <summary>Class to read or write from app.config file.</summary>
    public class clsConfiguration
    {
        #region Private declarations

        private readonly Configuration objConfiguration;

        #endregion

        #region Public declaraions

        #region Enumerations

        /// <summary>Enumeration for type of applications.</summary>
        public enum enuApplicationType
        {
            DesktopApp = 0,
            WebApp = 1
        }

        #endregion

        #endregion

        #region Constructors

        /// <summary>Constructor.</summary>
        public clsConfiguration(enuApplicationType eApplicationType)
        {
            switch (eApplicationType)
            {
                case enuApplicationType.DesktopApp:
                    objConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    break;
                case enuApplicationType.WebApp:
                    objConfiguration = WebConfigurationManager.OpenWebConfiguration("~");
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Public methods

        /// <summary>Create configuration file.</summary>
        /// <param name="sSectionName">Section name to be created.</param>
        /// <param name="objConfigurationSection">Configuration section object.</param>
        public void CreateConfigurationSection(string sSectionName,
                                               ConfigurationSection objConfigurationSection)
        {
            if (string.IsNullOrEmpty(sSectionName))
                return;

            if (objConfigurationSection == null)
                return;

            if (VerifyifSectionExists(sSectionName))
                RemoveSection(sSectionName);

            objConfiguration.Sections.Add(sSectionName, objConfigurationSection);

            objConfigurationSection.SectionInformation.ForceSave = true;
            objConfiguration.Save(ConfigurationSaveMode.Full);
        }

        /// <summary>Retrieve the configuration file.</summary>
        /// <param name="sSectionName">Section to retrieve.</param>
        /// <returns>Configurations.</returns>
        public ConfigurationSection GetConfigurationSectionValues(string sSectionName)
        {
            // Declarations
            ConfigurationSection objConfigurationSection;

            objConfigurationSection = objConfiguration.GetSection(sSectionName);

            return objConfigurationSection;
        }

        /// <summary>Verify if a section already exists in the config file.</summary>
        /// <param name="sSectionName"> Section name.</param>
        /// <returns>True, if sections exists.</returns>
        public bool VerifyifSectionExists(string sSectionName)
        {
            // Declarations
            bool fReturnValue = false;

            if (GetConfigurationSectionValues(sSectionName) == null)
                fReturnValue = false;
            else
                fReturnValue = true;

            return fReturnValue;
        }
        /// <summary>Remove a section.</summary>
        /// <param name="sSectionName">Section name to remove.</param>
        public void RemoveSection(string sSectionName)
        {
            if (VerifyifSectionExists(sSectionName))
                objConfiguration.Sections.Remove(sSectionName);
        }

        /// <summary>Create custom section in app setting.</summary>
        /// <param name="sKey">Key.</param>
        /// <param name="sSectionName">Section name</param>
        public void CreateAppSetting(string sKey,string sSectionName)
        {
            if (VerifyifAppSettingHasKey(sKey))
                RemoveAppSetting(sKey);

            objConfiguration.AppSettings.Settings.Add(sKey,sSectionName);
            objConfiguration.Save();
        }

        /// <summary>Remove Key from app setting.</summary>
        /// <param name="sKey">Key to remove.</param>
        public void RemoveAppSetting(string sKey)
        {
            objConfiguration.AppSettings.Settings.Remove(sKey);
            objConfiguration.Save();
        }

        /// <summary>Verifry if key exists</summary>
        /// <param name="sKey">Key to verify.</param>
        /// <returns>True, if keys exists.</returns>
        public bool VerifyifAppSettingHasKey(string sKey)
        {
            if(objConfiguration.AppSettings.Settings[sKey] != null)
            {
                return true;
            }

            return false;
        }
      
        /// <summary>Retrieve all App setting keys.</summary>
        /// <returns>List of keys.</returns>
        public string[] GetAllAppSettingKeys()
        {
            return objConfiguration.AppSettings.Settings.AllKeys;
        }

        /// <summary>Save configuration.</summary>
        public void Save()
        {
            objConfiguration.Save();
        }

        #endregion
    }
}
