namespace mcb.main.Model
{
    /// <summary>Setting model.</summary>
    public class clsSettingModel
    {
        #region Public properties

        /// <summary>Setting Id.</summary>
        public int iSettingID { get; set; }

        /// <summary>Import path.</summary>
        public string sImportPath { get; set; }

        /// <summary>Imported path.</summary>
        public string sImportedPath { get; set; }

        /// <summary>Scheduler to execute every mintues.</summary>
        public int iExecuteEvery { get; set; }

        #endregion
    }
}
