namespace mcb.main.Model
{
    /// <summary>Series model.</summary>
    public class clsSeriesModel
    {
        #region Public properties

        /// <summary>Series id.</summary>
        public int iSeriesId { get; set; }

        /// <summary>Series code.</summary>
        public string sSeriesCode { get; set; }

        /// <summary>Topic.</summary>
        public string sTopic { get; set; }

        /// <summary>Indicator name.</summary>
        public string sIndicatorName { get; set; }

        /// <summary>Short definition.</summary>
        public string sShortDefinition { get;set; }

        /// <summary>Long definition.</summary>
        public string sLongDefinition { get; set; }

        /// <summary>Unit of measure.</summary>
        public string sUnitOfMeasure { get;set; }

        /// <summary>Periodicity.</summary>
        public string sPeriodicity { get; set; }

        /// <summary>Base period.</summary>
        public string sBasePeriod { get; set; }

        /// <summary>Other notes.</summary>
        public string sOtherNotes { get; set; }

        /// <summary>Aggregation method.</summary>
        public string sAggregationMethod { get; set; }

        /// <summary>Limitation exceptions.</summary>
        public string sLimitationExceptions { get; set; }

        /// <summary>Original source notes.</summary>
        public string sOriginalSourceNotes { get; set; }

        /// <summary>General comments.</summary>
        public string sGeneralComments { get; set; }

        /// <summary>Sources.</summary>
        public string sSources { get; set; }

        /// <summary>Statistical methodology.</summary>
        public string sStatisticalMethodology { get; set; }

        /// <summary>Development relevance.</summary>
        public string sDevelopmentRelevance { get; set; }

        /// <summary>Source links.</summary>
        public string sSourceLinks { get; set; }

        /// <summary>Web links.</summary>
        public string sWebLinks { get; set; }

        /// <summary>Related indicators.</summary>
        public string sRelatedIndicators { get; set; }

        /// <summary>License type.</summary>
        public string sLicenseType { get; set; }

        #endregion
    }
}
