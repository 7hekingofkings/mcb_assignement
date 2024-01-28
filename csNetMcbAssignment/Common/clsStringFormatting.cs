using System;
using System.Linq.Expressions;

namespace Common.Utility.Formatting
{
    /// <summary>Class to format string.</summary>
    public static class clsStringFormatting
    {
        #region Private declarations

        #region Constants

        private const string sWINDOW_NEW_LINE = "\r\n";
        private const string sPOSIX = "\n";

        #endregion

        #endregion

        #region Public methods

        /// <summary>Convert field to sql parameter.</summary>
        /// <param name="sFieldName">Sql field name.</param>
        /// <returns>Converted sql parameter.</returns>
        public static string sConvertToSqlParameter(string sFieldName)
        {
            return string.Format("@{0}", sFieldName);
        }

        /// <summary>Get the name of the property.</summary>
        /// <typeparam name="T">T type object</typeparam>
        /// <param name="objExpression">Object to get property to string.</param>
        /// <returns>Name of the property.</returns>
        public static string sGetPropertyName<T>(Expression<Func<T>> objExpression)
        {
            MemberExpression objMemberExpression = (MemberExpression)objExpression.Body;
            return objMemberExpression.Member.Name;
        }

        /// <summary>Convert a plain text to tile case.</summary>
        /// <param name="sPlainText">Plain text to convert.</param>
        /// <returns>Return string in which the first letter only is converted to uppercase.</returns>
        public static string sTitleCaseString(string sPlainText)
        {
            // Declarations
            string sReturnValue = string.Empty;

            if (string.IsNullOrEmpty(sPlainText))
                return sReturnValue;

            if (sPlainText.Length >= 2)
                sReturnValue = string.Format("{0}{1}"
                                            , sPlainText.Substring(0, 1).ToUpper()
                                            , sPlainText.Substring(1));
            else
                sReturnValue = sPlainText.ToUpper();

            return sReturnValue;
        }

        /// <summary>Convert each first letter in text to upper case.</summary>
        /// <param name="sPlainText">Plain text to convert.</param>
        /// <returns>String with first leter in upper case.</returns>
        public static string sConvertEachFirstLetterInWordToUpper(string sPlainText)
        {
            // Declarations
            string sReturnValue = string.Empty;
            string[] arrsWords = null;

            try
            {
                if (string.IsNullOrEmpty(sPlainText))
                    return sReturnValue;

                if (sPlainText.Contains(" "))
                {
                    arrsWords = sPlainText.Split(' ');

                    foreach (string sWord in arrsWords)
                    {
                        sReturnValue += string.Format("{0} ", sTitleCaseString(sWord));
                    }
                }
                else
                {
                    sReturnValue = sTitleCaseString(sPlainText);
                }
            }
            finally
            {
                if (arrsWords != null)
                    arrsWords = null;
            }

            return sReturnValue.Trim();
        }

        #endregion
    }
}
