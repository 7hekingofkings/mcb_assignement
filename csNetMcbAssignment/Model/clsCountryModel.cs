using System.Windows.Forms;

namespace mcb.main.Model
{
    /// <summary>Country model.</summary>
    public class clsCountryModel
    {
        #region Public properties

        /// <summary>Country id.</summary>
        public int iCountryId { get; set; } 

        /// <summary>Country code.</summary>
        public string sCountryCode { get; set; }

        /// <summary>Short name.</summary>
        public string sShortName { get; set; }

        /// <summary>Table name.</summary>
        public string sTableName { get; set; }

        /// <summary>Long name.</summary>
        public string sLongName { get; set; }

        /// <summary>Alpha code.</summary>
        public string sAlphaCode { get;set; }

        /// <summary>Currency unit.</summary>
        public string sCurrencyUnit { get;set; }

        /// <summary>Special notes.</summary>
        public string sSpecialNotes { get; set; }

        /// <summary>Region.</summary>
        public string sRegion { get; set; } 

        /// <summary>Income group.</summary>
        public string sIncomeGroup { get; set; }

        /// <summary>Wd code.</summary>
        public string sWDCode { get; set; }

        /// <summary>National account base year.</summary>
        public string sNationalAccountBaseYear { get; set; }

        /// <summary>National account reference year.</summary>
        public string sNationalAccountReferenceYear { get; set; }

        /// <summary>SNA price valuation.</summary>
        public string sSNAPriceValuation { get; set; }

        /// <summary>Lending category.</summary>
        public string sLendingCategory { get; set; }

        /// <summary>Other groups.</summary>
        public string sOtherGroups { get; set; }

        /// <summary>National Accounts.</summary>
        public string sNationalAccounts { get; set; }

        /// <summary>Conversion factor.</summary>
        public string sConversionFactor { get; set; }

        /// <summary>PPP servey year.</summary>
        public string sPPPSurveyYear { get; set; }

        /// <summary>Balance payment manual.</summary>
        public string sBalancePaymentManual { get; set; }

        /// <summary>External debt status.</summary>
        public string sExternalDebtStatus { get; set; }

        /// <summary>System trade.</summary>
        public string sSystemTrade { get; set; }

        /// <summary>Accounting concept.</summary>
        public string sAccountingConcept { get; set; }

        /// <summary>IMF data.</summary>
        public string sIMFData { get; set; }

        /// <summary>Population sensus.</summary>
        public string sPopulationSensus { get; set; }

        /// <summary>House hold survey.</summary>
        public string sHouseHoldSurvey { get; set; }

        /// <summary>Income expenditure data.</summary>
        public string sIncomeExpenditureData { get;set; }

        /// <summary>Registration complete.</summary>
        public bool fRegistrationComplete { get; set; }

        /// <summary>Agricultural census.</summary>
        public string sAgriculturalCensus { get; set; }

        /// <summary>Industrial data.</summary>
        public string sIndustrialData { get; set; }

        /// <summary>Trade data.</summary>
        public string sTradeData { get;set; }

        #endregion
    }
}
