using System.Security;

namespace Common.Utility.Security
{
    /// <summary>
    /// A class that provide a measure of security for strings. 
    /// Example:- passwords
    /// </summary>
    public static class clsSecureString
    {
        #region Public methods

        /// <summary>Convert a plan text to a secure string object.</summary>
        /// <param name="sPlainText">Plain text as string.</param>
        /// <param name="fReadonly">Make the secure string readonly.</param>
        /// <returns>Return an object of secure string.</returns>
        public static SecureString ConvertToSecureString(string sPlainText,
                                                          bool fReadonly = true)
        {
            // Declarations
            SecureString objSecureString = new SecureString();

            if (string.IsNullOrEmpty(sPlainText))
                return null;

            foreach (char chCharacter in sPlainText)
            {
                objSecureString.AppendChar(chCharacter);
            }

            if (fReadonly)
                objSecureString.MakeReadOnly();

            return objSecureString;
        }

        #endregion
    }
}
