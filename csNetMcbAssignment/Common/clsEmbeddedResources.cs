using System.IO;
using System.Reflection;

namespace Common.Utility.Resources
{
    /// <summary>Retrieve embeddef resources in any dll.</summary>
    public class clsEmbeddedResources
    {
        #region Public methods

        /// <summary>Get embedded resources from namespace.</summary>
        /// <param name="sAssembly">Assembly to be loaded.</param>
        /// <param name="sNamespace">Namespace to retrieve resources.</param>
        /// <returns>String.</returns>
        public static string GetEmbeddedResources(string sAssembly, string sNamespace)
        {
            // Declarations
            string sReturnValue = string.Empty;
            StreamReader objStreamReader = null;
            Assembly objAssembly = null;

            try
            {
                // Initialisations
                objAssembly = Assembly.Load(sAssembly);
                objStreamReader = new StreamReader(objAssembly.GetManifestResourceStream(sNamespace));

                sReturnValue = objStreamReader.ReadToEnd();
            }
            finally
            {
                if (objStreamReader != null)
                {
                    objStreamReader.Dispose();
                    objStreamReader = null;
                }

                if (objAssembly != null)
                    objAssembly = null;

            }

            return sReturnValue;
        }

        /// <summary>Get embedded resources from namespace.</summary>
        /// <param name="sAssembly">Assembly to be loaded.</param>
        /// <param name="sNamespace">Namespace to retrieve resources.</param>
        /// <returns>Stream.</returns>
        public static FileStream GetEmbeddedResourcesLocation(string sAssembly, string sNamespace)
        {
            // Declarations
            Assembly objAssembly = null;
            try
            {
                // Initialisations
                objAssembly = Assembly.Load(sAssembly);
                return new FileStream(string.Concat(objAssembly.Location, sNamespace), FileMode.Open);
            }
            finally
            {
                if (objAssembly != null)
                    objAssembly = null;
            }
        }

        #endregion
    }
}
