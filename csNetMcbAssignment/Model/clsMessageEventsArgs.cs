using System;

namespace mcb.main.Model
{
    /// <summary>Message and event model.</summary>
    public class clsMessageEventsArgs :EventArgs
    {
        #region Public properties

        /// <summary>Event data time.</summary>
        public DateTime dtEventDateTime { get; set; }

        /// <summary>Event message.</summary>
        public string sMessage { get; set; }

        #endregion
    }
}
