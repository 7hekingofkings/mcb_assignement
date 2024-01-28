using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utility.Security
{
    /// <summary>Class for 3des encryption and decryption.</summary>
    public static class clsTripleDESCryptography
    {
        #region Private declarations

        private const string sENCRYPTION_3DES_KEY = "AOT_(Add_On_Technologies_Ltd)";

        #endregion

        #region Public methods

        /// <summary>Method for string encryptions.</summary>
        /// <param name="sTextToEncrypt">Text to encrypt.</param>
        /// <returns>Return an encryted text.</returns>
        public static string Encrypt(string sTextToEncrypt)
        {
            // Declarations
            string sReturnValue = string.Empty;
            byte[] byEncrytedArray = null;
            byte[] bySecurityKeyArray = null;
            byte[] byResultArray = null;
            MD5CryptoServiceProvider objMD5CryptoService = null;
            TripleDESCryptoServiceProvider obj3DESCrypService = null;

            try
            {
                // Initialisation
                objMD5CryptoService = new MD5CryptoServiceProvider();
                obj3DESCrypService = new TripleDESCryptoServiceProvider();

                byEncrytedArray = UTF8Encoding.UTF8.GetBytes(sTextToEncrypt);
                bySecurityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(sENCRYPTION_3DES_KEY));
                objMD5CryptoService.Clear();

                obj3DESCrypService.Key = bySecurityKeyArray;
                obj3DESCrypService.Mode = CipherMode.ECB;
                obj3DESCrypService.Padding = PaddingMode.PKCS7;

                byResultArray = obj3DESCrypService.CreateEncryptor().TransformFinalBlock(byEncrytedArray, 0, byEncrytedArray.Length);
                obj3DESCrypService.Clear();

                sReturnValue = Convert.ToBase64String(byResultArray, 0, byResultArray.Length);

            }
            finally
            {
                if (objMD5CryptoService != null)
                {
                    objMD5CryptoService.Dispose();
                    objMD5CryptoService = null;
                }

                if (byEncrytedArray != null)
                    byEncrytedArray = null;

                if (bySecurityKeyArray != null)
                    bySecurityKeyArray = null;

                if (byResultArray != null)
                    byResultArray = null;

                if (obj3DESCrypService != null)
                {
                    obj3DESCrypService.Dispose();
                    obj3DESCrypService = null;
                }
            }

            return sReturnValue;
        }

        /// <summary>Method for string descrytion.</summary>
        /// <param name="sTextToDecrypt">Text to decrypt.</param>
        /// <returns>Returns a decryted text.</returns>
        public static string Decrypt(string sTextToDecrypt)
        {
            // Declarations
            string sReturnValue = string.Empty;
            byte[] byDecryptArray = null;
            byte[] bySecurityKeyArray = null;
            byte[] byResultArray = null;
            MD5CryptoServiceProvider objMD5CryptoService = null;
            TripleDESCryptoServiceProvider obj3DESCrypService = null;

            try
            {
                // Initialisations
                objMD5CryptoService = new MD5CryptoServiceProvider();
                obj3DESCrypService = new TripleDESCryptoServiceProvider();

                byDecryptArray = Convert.FromBase64String(sTextToDecrypt);
                bySecurityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(sENCRYPTION_3DES_KEY));
                objMD5CryptoService.Clear();

                obj3DESCrypService.Key = bySecurityKeyArray;
                obj3DESCrypService.Mode = CipherMode.ECB;
                obj3DESCrypService.Padding = PaddingMode.PKCS7;

                byResultArray = obj3DESCrypService.CreateDecryptor().TransformFinalBlock(byDecryptArray, 0, byDecryptArray.Length);
                obj3DESCrypService.Clear();

                sReturnValue = UTF8Encoding.UTF8.GetString(byResultArray);
            }
            finally
            {
                if (objMD5CryptoService != null)
                {
                    objMD5CryptoService.Dispose();
                    objMD5CryptoService = null;
                }

                if (byDecryptArray != null)
                    byDecryptArray = null;

                if (bySecurityKeyArray != null)
                    bySecurityKeyArray = null;

                if (byResultArray != null)
                    byResultArray = null;

                if (obj3DESCrypService != null)
                {
                    obj3DESCrypService.Dispose();
                    obj3DESCrypService = null;
                }
            }

            return sReturnValue;
        }

        #endregion
    }
}
