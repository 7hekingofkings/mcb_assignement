using System.Configuration;

namespace Common.Forms.Utility
{
    /// <summary>This class is used to populate the connection properties.</summary>
    public class clsConnectionSection : ConfigurationSection
    {
        #region Private declarations

        private const string sSERVER_NAME = "ServerName";
        private const string sDATABASE_NAME = "DatabaseName";
        private const string sUSER_NAME = "Username";
        private const string sPASSWORD = "Password";

        #endregion

        #region Constructor

        /// <summary>Constructor.</summary>
        public clsConnectionSection() { }

        /// <summary>Constructor.</summary>
        /// <param name="sServerName">Server name.</param>
        /// <param name="sDatabaseName">Database name.</param>
        /// <param name="sUsername">Username.</param>
        /// <param name="sPassword">Password.</param>
        public clsConnectionSection(string sServerName
                                   , string sDatabaseName
                                   , string sUsername
                                   , string sPassword)
        {

            this.sServerName = sServerName;
            this.sUsername = sUsername;
            this.sPassword = sPassword;
            this.sDatabaseName = sDatabaseName;
        }

        #endregion

        #region Public Properties

        [ConfigurationProperty(sSERVER_NAME,
         DefaultValue = null,
         IsRequired = true
         )]
        /// <summary>Server name.</summary>
        public string sServerName
        {
            get
            {
                return (string)this[sSERVER_NAME];
            }
            set
            {
                this[sSERVER_NAME] = value;
            }
        }

        [ConfigurationProperty(sDATABASE_NAME,
         DefaultValue = null,
         IsRequired = true
         )]
        /// <summary>Database name.</summary>
        public string sDatabaseName
        {
            get
            {
                return (string)this[sDATABASE_NAME];
            }
            set
            {
                this[sDATABASE_NAME] = value;
            }
        }

        [ConfigurationProperty(sUSER_NAME,
         DefaultValue = null,
         IsRequired = true
         )]
        /// <summary>Database username.</summary>
        public string sUsername
        {
            get
            {
                return (string)this[sUSER_NAME];
            }
            set
            {
                this[sUSER_NAME] = value;
            }
        }

        [ConfigurationProperty(sPASSWORD,
         DefaultValue = null,
         IsRequired = true
         )]
        /// <summary>Database password.</summary>
        public string sPassword
        {
            get
            {
                return (string)this[sPASSWORD];
            }
            set
            {
                this[sPASSWORD] = value;
            }
        }

        #endregion
    }

}
