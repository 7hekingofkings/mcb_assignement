using System;

namespace Common.Utility.Exceptions
{
    /// <summary>Custom information exceptions.</summary>
    public class clsInfoExceptions : Exception
    {
        #region Constructor

        /// <summary>Constructor.</summary>
        public clsInfoExceptions() { }

        /// <summary>Constructor.</summary>
        /// <param name="sMessage">Information message.</param>
        public clsInfoExceptions(string sMessage)
        : base(sMessage) { }

        /// <summary>Constructor.</summary>
        /// <param name="sMessage">Information message.</param>
        /// <param name="exInner">Error that occur during application execution.</param>
        public clsInfoExceptions(string sMessage, Exception exInner)
            : base(sMessage, exInner) { }

        #endregion
    }
}
