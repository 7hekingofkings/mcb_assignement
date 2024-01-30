using Common.Utility.Connection;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using mcb.main.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace mcb.main.Logic
{
    /// <summary>Import data.</summary>
    public class clsImportData
    {
        #region Private declarations

        #region Constants

        public const string sPATTERN = @"""\s*,\s*""";

        #endregion

        #region Variables

        private bool _fExecuting = false;

        #endregion

        #endregion

        #region Public declarations

        public EventHandler<clsMessageEventsArgs> evLogEvent;

        #endregion

        #region Public properties

        /// <summary>Connection object.</summary>
        public clsConnection objConnection { get; set; }

        /// <summary>State of process.</summary>
        public bool fExecuting
        {
            get { return _fExecuting; }
        }

        #endregion

        #region Private methods

        /// <summary>Extract excel data to objects.</summary>
        /// <param name="sFile">Filename.</param>
        /// <param name="sImportedPath">Imported path.</param>
        private void ExtractExclFile(string sFile, string sImportedPath)
        {
            // Declarations
            clsCorruptionLogic objCorruptionLogic = null;
            DataTable dtbData = null;
            DataTable dtbExcel = null;
            string sValue;
            string sConcatValue = string.Empty;

            try
            {
                // Initialisations
                objCorruptionLogic = new clsCorruptionLogic(objConnection);
                dtbExcel = new DataTable();
                dtbData = new DataTable("DATA");
                dtbData.Columns.Add("COUNTRY_NAME", typeof(string));
                dtbData.Columns.Add("COUNTRY_CODE", typeof(string));
                dtbData.Columns.Add("REGION", typeof(string));
                dtbData.Columns.Add("YEARS", typeof(int));
                dtbData.Columns.Add("CPI_SCORE", typeof(float));
                dtbData.Columns.Add("RANKS", typeof(int));
                dtbData.Columns.Add("SOURCES", typeof(float));
                dtbData.Columns.Add("STANDARD_ERROR", typeof(float));
                sValue = string.Empty;

                using (SpreadsheetDocument objSpreadSheetDocument = SpreadsheetDocument.Open(sFile, false))
                {
                    WorkbookPart objWorkbookPart = objSpreadSheetDocument.WorkbookPart;
                    SharedStringTablePart objSstpart = objWorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                    SharedStringTable objSst = objSstpart.SharedStringTable;

                    WorksheetPart objWorksheetPart = objWorkbookPart.WorksheetParts.First();
                    Worksheet objSheet = objWorksheetPart.Worksheet;

                    var rows = objSheet.Descendants<Row>();

                    foreach (Row objRow in rows)
                    {
                        sConcatValue = string.Empty;
                        foreach (Cell objCell in objRow.Elements<Cell>())
                        {
                            if ((objCell.DataType != null) && (objCell.DataType == CellValues.SharedString))
                                sValue = objSst.ChildElements[int.Parse(objCell.CellValue.Text)]
                                               .InnerText;
                            else if (objCell.CellValue != null)
                                sValue = objCell.CellValue.InnerText;

                            if (objRow.RowIndex == 1)
                                dtbExcel.TableName = sValue;

                            if (objRow.RowIndex == 3)
                                dtbExcel.Columns.Add(sValue);

                            sConcatValue += sValue + ";";

                        }

                        if (objRow.RowIndex > 3)
                        {
                            sConcatValue = sConcatValue.Substring(0, sConcatValue.Length - 1);
                            dtbExcel.Rows.Add(sConcatValue.Split(';').ToArray());
                        }

                    }
                }

                foreach (DataRow objRow in dtbExcel.Rows)
                {
                    for (int i = 3; i < 18; i += 4)
                    {

                        dtbData.Rows.Add(objRow[0]
                                        , objRow[1]
                                        , objRow[2]
                                        , int.Parse(dtbExcel.Columns[i].ColumnName.Split(' ').Last())
                                        , objRow[i]
                                        , objRow[i + 1]
                                        , objRow[i + 2]
                                        , objRow[i + 3]);
                    };

                    for (int i = 19; i < dtbExcel.Columns.Count; i += 3)
                    {

                        dtbData.Rows.Add(objRow[0]
                                        , objRow[1]
                                        , objRow[2]
                                        , int.Parse(dtbExcel.Columns[i].ColumnName.Split(' ').Last())
                                        , objRow[i]
                                        , 0
                                        , objRow[i + 1]
                                        , objRow[i + 2]);
                    };
                }

                objConnection.BeginTransaction();
                objConnection.niTimeOut = 10000;
                objCorruptionLogic.InsertData(dtbData);
                objConnection.CommitTransaction();


                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = "Import has completed successfully.",
                    dtEventDateTime = DateTime.Now
                });

            }
            catch (Exception e)
            {
                objConnection.RollBackTransaction();
                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = e.Message,
                    dtEventDateTime = DateTime.Now,
                });
            }
            finally
            {
                if (dtbData != null)
                {
                    dtbData.Clear();
                    dtbData.Dispose();
                    dtbData = null;
                }

                if (dtbExcel != null)
                {
                    dtbExcel.Clear();
                    dtbExcel.Dispose();
                    dtbExcel = null;
                }

                if (objCorruptionLogic != null)
                {
                    objCorruptionLogic.Dispose();
                    objCorruptionLogic = null;
                }
            }
        }

        /// <summary>Extract data to objects.</summary>
        /// <param name="sFile">Filename.</param>
        /// <param name="sImportedPath">Imported path.</param>
        private void ExtractCsvFile(string sFile, string sImportedPath)
        {
            switch (File.ReadAllLines(sFile)
                       .Skip(0)
                       .Take(1)
                       .FirstOrDefault()
                       .Split(',')
                       .Count())
            {
                case 31:
                    ConvertCountryDataToObjectAndSaveToDb(sFile, sImportedPath);
                    break;
                case 21:
                    ConvertSeriesToObjectAndSaveToDb(sFile, sImportedPath);
                    break;
                case 4:
                    SelectBtwCountryAndTime(sFile, sImportedPath);
                    break;
                case 5:
                    ConvertFootNoteDataToObjectAndSaveToDb(sFile, sImportedPath);
                    break;
                case 68:
                    ImportDataAndSaveToDd(sFile, sImportedPath);
                    break;
                default:
                    return;
            }

        }

        /// <summary>Select between country series and time series.</summary>
        /// <param name="sFile">File path.</param>
        /// <param name="sImportedPath">Imported path.</param>
        private void SelectBtwCountryAndTime(string sFile, string sImportedPath)
        {
            if (File.ReadAllLines(sFile)
                    .Skip(0)
                    .Take(1)
                    .FirstOrDefault()
                    .Contains("CountryCode"))
                ConvertCountrySeriesDataToObjectAndSaveToDb(sFile, sImportedPath);
            else
                ConvertTimeSeriesDataToObjectAndSaveToDb(sFile, sImportedPath);
        }

        /// <summary>Import data and save to db</summary>
        /// <param name="sFile">File path.</param>
        /// <param name="sImportedPath">Imported path.</param>
        public void ImportDataAndSaveToDd(string sFile, string sImportedPath)
        {
            // Declarations
            string[] arrKey = null;
            string[] arrFileDetails = null;
            DataTable dtbData = null;
            string[] arrSeriesDetails = null;
            string sMergeLine = string.Empty;
            string sDataDetails = string.Empty;
            List<Dictionary<string, string>> lstDicData = null;
            string sHeader = string.Empty;
            Dictionary<string, string> dicCurrent = null;
            clsDataLogic objDataLogic = null;

            try
            {
                // Initialisations
                lstDicData = new List<Dictionary<string, string>>();
                dicCurrent = new Dictionary<string, string>();
                objDataLogic = new clsDataLogic(objConnection);

                dtbData = new DataTable("DATA");
                dtbData.Columns.Add("COUNTRY_NAME", typeof(string));
                dtbData.Columns.Add("COUNTRY_CODE", typeof(string));
                dtbData.Columns.Add("SERIES_NAME", typeof(string));
                dtbData.Columns.Add("SERIES_CODE", typeof(string));
                dtbData.Columns.Add("YEARS", typeof(int));
                dtbData.Columns.Add("AMOUNT", typeof(float));

                sHeader = File.ReadAllLines(sFile)
                              .Skip(0)
                              .Take(1)
                              .FirstOrDefault();
                arrKey = Regex.Split(sHeader.Substring(1, sHeader.Length - 3), sPATTERN);

                arrFileDetails = File.ReadAllLines(sFile)
                                     .Skip(1)
                                     .ToArray();

                foreach (string sDetails in arrFileDetails)
                {

                    if (sDetails.EndsWith(@""","))
                    {
                        if (!string.IsNullOrEmpty(sMergeLine))
                            sDataDetails = sMergeLine + sDetails;
                        else
                            sDataDetails = sDetails;

                        arrSeriesDetails = Regex.Split(sDataDetails.Substring(1, sDataDetails.Length - 3), sPATTERN);
                        sMergeLine = string.Empty;
                    }
                    else
                    {
                        sMergeLine += sDetails;
                        continue;
                    }

                    if (arrSeriesDetails == null)
                        continue;

                    dicCurrent.Clear();
                    for (int i = 0; i < arrKey.Length; i++)
                    {
                        dicCurrent.Add(arrKey[i], arrSeriesDetails[i]);
                    }
                    lstDicData.Add(dicCurrent);
                }

                foreach (Dictionary<string, string> DicItem in lstDicData)
                {
                    foreach (KeyValuePair<string, string> item in DicItem)
                    {
                        if (item.Key == "Country Name" ||
                            item.Key == "Country Code" ||
                            item.Key == "Indicator Name" ||
                            item.Key == "Indicator Code")
                            continue;

                        if (string.IsNullOrEmpty(item.Value))
                            continue;

                        dtbData.Rows.Add(DicItem["Country Name"]
                                        , DicItem["Country Code"]
                                        , DicItem["Indicator Name"]
                                        , DicItem["Indicator Code"]
                                        , int.Parse(item.Key)
                                        , float.Parse(item.Value));

                    }
                }

                objConnection.BeginTransaction();
                objConnection.niTimeOut = 10000;
                objDataLogic.InsertData(dtbData);
                objConnection.CommitTransaction();

                if (File.Exists(sFile))
                {
                    if (File.Exists(string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile))))
                        File.Delete(string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile)));

                    File.Move(sFile, string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile)));
                }

                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = "Data has been imported successfully.",
                    dtEventDateTime = DateTime.Now
                });

            }
            catch (Exception e)
            {
                objConnection.RollBackTransaction();
                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = e.Message,
                    dtEventDateTime = DateTime.Now,
                });
            }
            finally
            {
                if (arrFileDetails != null)
                    arrFileDetails = null;

                if (arrSeriesDetails != null)
                    arrSeriesDetails = null;

                if (dtbData != null)
                {
                    dtbData.Clear();
                    dtbData.Dispose();
                    dtbData = null;
                }

                if (objDataLogic != null)
                {
                    objDataLogic.Dispose();
                    objDataLogic = null;
                }
            }
        }

        /// <summary>Convert data to object and save to database.</summary>
        /// <param name="sFile">File path.</param>
        /// <param name="sImportedPath">Imported path.</param>
        public void ConvertFootNoteDataToObjectAndSaveToDb(string sFile, string sImportedPath)
        {
            // Declarations
            string[] arrFileDetails = null;
            DataTable dtbFootNote = null;
            string[] arrSeriesDetails = null;
            clsFootNoteLogic objFootNoteLogic = null;
            string sMergeLine = string.Empty;
            string sFNoteDetails = string.Empty;
            ;
            try
            {
                // Initialisations
                objFootNoteLogic = new clsFootNoteLogic(objConnection);
                dtbFootNote = new DataTable("FOOTNOTE");
                dtbFootNote.Columns.Add("COUNTRY_CODE", typeof(string));
                dtbFootNote.Columns.Add("SERIES_CODE", typeof(string));
                dtbFootNote.Columns.Add("YEARS", typeof(int));
                dtbFootNote.Columns.Add("DESCRIPTIONS", typeof(string));

                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = "Importing foot note master files..",
                    dtEventDateTime = DateTime.Now
                });

                arrFileDetails = File.ReadAllLines(sFile)
                                     .Skip(1)
                                     .ToArray();

                foreach (string sFootNoteDetails in arrFileDetails)
                {

                    if (sFootNoteDetails.EndsWith(@""","))
                    {
                        if (!string.IsNullOrEmpty(sMergeLine))
                            sFNoteDetails = sMergeLine + sFootNoteDetails;
                        else
                            sFNoteDetails = sFootNoteDetails;

                        arrSeriesDetails = Regex.Split(sFNoteDetails.Substring(1, sFNoteDetails.Length - 3), sPATTERN);
                        sMergeLine = string.Empty;
                    }
                    else
                    {
                        sMergeLine += sFootNoteDetails;
                        continue;
                    }

                    if (arrSeriesDetails == null)
                        continue;

                    dtbFootNote.Rows.Add(arrSeriesDetails[0]
                                        , arrSeriesDetails[1]
                                        , int.Parse(arrSeriesDetails[2].Substring(2))
                                        , arrSeriesDetails[3]);
                }

                objConnection.BeginTransaction();
                objConnection.niTimeOut = 10000;
                objFootNoteLogic.InsertFootNote(dtbFootNote);
                objConnection.CommitTransaction();

                if (File.Exists(sFile))
                {
                    if (File.Exists(string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile))))
                        File.Delete(string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile)));

                    File.Move(sFile, string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile)));
                }

                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = "Foot note has been imported successfully.",
                    dtEventDateTime = DateTime.Now
                });
            }
            catch (Exception e)
            {
                objConnection.RollBackTransaction();
                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = e.Message,
                    dtEventDateTime = DateTime.Now,
                });
            }
            finally
            {
                if (arrFileDetails != null)
                    arrFileDetails = null;

                if (arrSeriesDetails != null)
                    arrSeriesDetails = null;

                if (dtbFootNote != null)
                {
                    dtbFootNote.Clear();
                    dtbFootNote.Dispose();
                    dtbFootNote = null;
                }

                if (objFootNoteLogic != null)
                {
                    objFootNoteLogic.Dispose();
                    objFootNoteLogic = null;
                }
            }
        }

        /// <summary>Convert data to object and save to database.</summary>
        /// <param name="sFile">File path.</param>
        /// <param name="sImportedPath">Imported path.</param>
        private void ConvertTimeSeriesDataToObjectAndSaveToDb(string sFile, string sImportedPath)
        {
            // Declarations
            string[] arrFileDetails = null;
            List<clsTimeSeriesModel> lstTimeSeries = null;
            string[] arrSeriesDetails = null;
            clsTimeSeriesLogic objTimeSeriesLogic = null;
            string sMergeLine = string.Empty;
            string sTSeriesDetails = string.Empty;

            try
            {
                // Initialisations
                lstTimeSeries = new List<clsTimeSeriesModel>();
                objTimeSeriesLogic = new clsTimeSeriesLogic(objConnection);

                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = "Importing time series master files..",
                    dtEventDateTime = DateTime.Now
                });

                arrFileDetails = File.ReadAllLines(sFile)
                                     .Skip(1)
                                     .ToArray();

                foreach (string sTimeSeriesDetails in arrFileDetails)
                {

                    if (sTimeSeriesDetails.EndsWith(","))
                    {
                        if (!string.IsNullOrEmpty(sMergeLine))
                            sTSeriesDetails = sMergeLine + sTimeSeriesDetails;
                        else
                            sTSeriesDetails = sTimeSeriesDetails;

                        arrSeriesDetails = Regex.Split(sTSeriesDetails.Substring(1, sTSeriesDetails.Length - 3), sPATTERN);
                        sMergeLine = string.Empty;
                    }
                    else
                    {
                        sMergeLine += sTimeSeriesDetails;
                        continue;
                    }

                    if (arrSeriesDetails == null)
                        continue;

                    lstTimeSeries.Add(new clsTimeSeriesModel()
                    {
                        iTimeSeriesId = 0,
                        sSeriesCode = arrSeriesDetails[0],
                        iYear = int.Parse(arrSeriesDetails[1].Substring(2)),
                        sDescription = arrSeriesDetails[2]
                    });
                }

                objConnection.BeginTransaction();
                foreach (clsTimeSeriesModel objTimeSeriesModel in lstTimeSeries)
                {
                    objTimeSeriesLogic.InsertTimeSeries(objTimeSeriesModel);
                }
                objConnection.CommitTransaction();

                if (File.Exists(sFile))
                {
                    if (File.Exists(string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile))))
                        File.Delete(string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile)));

                    File.Move(sFile, string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile)));
                }

                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = "Time series has been imported successfully.",
                    dtEventDateTime = DateTime.Now
                });
            }
            catch (Exception e)
            {
                objConnection.RollBackTransaction();
                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = e.Message,
                    dtEventDateTime = DateTime.Now,
                });
            }
            finally
            {
                if (arrFileDetails != null)
                    arrFileDetails = null;

                if (arrSeriesDetails != null)
                    arrSeriesDetails = null;

                if (lstTimeSeries != null)
                {
                    lstTimeSeries.Clear();
                    lstTimeSeries = null;
                }

                if (objTimeSeriesLogic != null)
                {
                    objTimeSeriesLogic.Dispose();
                    objTimeSeriesLogic = null;
                }
            }
        }

        /// <summary>Convert data to object and save to database.</summary>
        /// <param name="sFile">File path.</param>
        /// <param name="sImportedPath">Imported path.</param>
        private void ConvertCountrySeriesDataToObjectAndSaveToDb(string sFile, string sImportedPath)
        {
            // Declarations
            string[] arrFileDetails = null;
            List<clsCountrySeriesModel> lstCountrySeries = null;
            string[] arrSeriesDetails = null;
            clsCountrySeriesLogic objCountrySeriesLogic = null;
            string sMergeLine = string.Empty;
            string sCountrySeriesDetails = string.Empty;

            try
            {
                // Initialisations
                lstCountrySeries = new List<clsCountrySeriesModel>();
                objCountrySeriesLogic = new clsCountrySeriesLogic(objConnection);

                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = "Importing country series master files..",
                    dtEventDateTime = DateTime.Now
                });

                arrFileDetails = File.ReadAllLines(sFile)
                                     .Skip(1)
                                     .ToArray();

                foreach (string sCountryDetails in arrFileDetails)
                {

                    if (sCountryDetails.EndsWith(","))
                    {
                        if (!string.IsNullOrEmpty(sMergeLine))
                            sCountrySeriesDetails = sMergeLine + sCountryDetails;
                        else
                            sCountrySeriesDetails = sCountryDetails;

                        arrSeriesDetails = Regex.Split(sCountrySeriesDetails.Substring(1, sCountrySeriesDetails.Length - 3), sPATTERN);
                        sMergeLine = string.Empty;
                    }
                    else
                    {
                        sMergeLine += sCountryDetails;
                        continue;
                    }

                    if (arrSeriesDetails == null)
                        continue;

                    lstCountrySeries.Add(new clsCountrySeriesModel()
                    {
                        iCountrySeriesId = 0,
                        sCountryCode = arrSeriesDetails[0],
                        sSeriesCode = arrSeriesDetails[1],
                        sDescription = arrSeriesDetails[2]
                    });
                }

                objConnection.BeginTransaction();
                foreach (clsCountrySeriesModel objCountrySeriesModel in lstCountrySeries)
                {
                    objCountrySeriesLogic.InsertCountrySeries(objCountrySeriesModel);
                }
                objConnection.CommitTransaction();

                if (File.Exists(sFile))
                {
                    if (File.Exists(string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile))))
                        File.Delete(string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile)));

                    File.Move(sFile, string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile)));
                }

                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = "Country series has been imported successfully.",
                    dtEventDateTime = DateTime.Now
                });

            }
            catch (Exception e)
            {
                objConnection.RollBackTransaction();
                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = e.Message,
                    dtEventDateTime = DateTime.Now,
                });
            }
            finally
            {
                if (arrFileDetails != null)
                    arrFileDetails = null;

                if (arrSeriesDetails != null)
                    arrSeriesDetails = null;

                if (lstCountrySeries != null)
                {
                    lstCountrySeries.Clear();
                    lstCountrySeries = null;
                }

                if (objCountrySeriesLogic != null)
                {
                    objCountrySeriesLogic.Dispose();
                    objCountrySeriesLogic = null;
                }
            }
        }

        /// <summary>Convert data to object and save to database.</summary>
        /// <param name="sFile">File path.</param>
        /// <param name="sImportedPath">Imported path.</param>
        private void ConvertCountryDataToObjectAndSaveToDb(string sFile, string sImportedPath)
        {
            // Declarations
            string[] arrFileDetails = null;
            List<clsCountryModel> lstCountry = null;
            string[] arrCountryDetails = null;
            clsCountryLogic objCountryLogic = null;
            string sMergeLine = string.Empty;
            string sCountryDetails = string.Empty;

            try
            {
                // Initialisations
                lstCountry = new List<clsCountryModel>();
                objCountryLogic = new clsCountryLogic(objConnection);

                arrFileDetails = File.ReadAllLines(sFile)
                                     .Skip(1)
                                     .ToArray();

                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = "Importing countries master files..",
                    dtEventDateTime = DateTime.Now
                });


                if (arrFileDetails == null)
                    return;

                foreach (string sCountry in arrFileDetails)
                {

                    if (sCountry.EndsWith(","))
                    {
                        if (!string.IsNullOrEmpty(sMergeLine))
                            sCountryDetails = sMergeLine + sCountry;
                        else
                            sCountryDetails = sCountry;

                        arrCountryDetails = Regex.Split(sCountryDetails.Substring(1, sCountryDetails.Length - 3), sPATTERN);
                        sMergeLine = string.Empty;
                    }
                    else
                    {
                        sMergeLine += sCountry;
                        continue;
                    }

                    if (arrCountryDetails == null)
                    {
                        sMergeLine = string.Empty;
                        continue;
                    }

                    lstCountry.Add(new clsCountryModel()
                    {
                        iCountryId = 0,
                        sCountryCode = arrCountryDetails[0],
                        sShortName = arrCountryDetails[1],
                        sTableName = arrCountryDetails[2],
                        sLongName = arrCountryDetails[3],
                        sAlphaCode = arrCountryDetails[4],
                        sCurrencyUnit = arrCountryDetails[5],
                        sSpecialNotes = arrCountryDetails[6],
                        sRegion = arrCountryDetails[7],
                        sIncomeGroup = arrCountryDetails[8],
                        sWDCode = arrCountryDetails[9],
                        sNationalAccountBaseYear = arrCountryDetails[10],
                        sNationalAccountReferenceYear = arrCountryDetails[11],
                        sSNAPriceValuation = arrCountryDetails[12],
                        sLendingCategory = arrCountryDetails[13],
                        sOtherGroups = arrCountryDetails[14],
                        sNationalAccounts = arrCountryDetails[15],
                        sConversionFactor = arrCountryDetails[16],
                        sPPPSurveyYear = arrCountryDetails[17],
                        sBalancePaymentManual = arrCountryDetails[18],
                        sExternalDebtStatus = arrCountryDetails[19],
                        sSystemTrade = arrCountryDetails[20],
                        sAccountingConcept = arrCountryDetails[21],
                        sIMFData = arrCountryDetails[22],
                        sPopulationSensus = arrCountryDetails[23],
                        sHouseHoldSurvey = arrCountryDetails[24],
                        sIncomeExpenditureData = arrCountryDetails[25],
                        fRegistrationComplete = arrCountryDetails[26].ToLower().Contains("yes") ? true : false,
                        sAgriculturalCensus = arrCountryDetails[27],
                        sIndustrialData = arrCountryDetails[28],
                        sTradeData = arrCountryDetails[29]
                    });
                }

                objConnection.BeginTransaction();
                foreach (clsCountryModel objCountryModel in lstCountry)
                {
                    objCountryLogic.InsertCountry(objCountryModel);
                }
                objConnection.CommitTransaction();

                if (File.Exists(sFile))
                {
                    if (File.Exists(string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile))))
                        File.Delete(string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile)));

                    File.Move(sFile, string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile)));
                }

                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = "Countries has been imported successfully.",
                    dtEventDateTime = DateTime.Now
                });

            }
            catch (Exception e)
            {
                objConnection.RollBackTransaction();
                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = e.Message,
                    dtEventDateTime = DateTime.Now,
                });
            }
            finally
            {
                if (arrFileDetails != null)
                    arrFileDetails = null;

                if (arrCountryDetails != null)
                    arrCountryDetails = null;

                if (lstCountry != null)
                {
                    lstCountry.Clear();
                    lstCountry = null;
                }

                if (objCountryLogic != null)
                {
                    objCountryLogic.Dispose();
                    objCountryLogic = null;
                }
            }
        }

        /// <summary>Convert data to object and save to database.</summary>
        /// <param name="sFile">File path.</param>
        /// <param name="sImportedPath">Imported path.</param>
        private void ConvertSeriesToObjectAndSaveToDb(string sFile, string sImportedPath)
        {
            // Declarations
            string[] arrFileDetails = null;
            List<clsSeriesModel> lstSeries = null;
            string[] arrSeriesDetails = null;
            clsSeriesLogic objSeriesLogic = null;
            string sMergeLine = string.Empty;
            string sSeriesDetails = string.Empty;

            try
            {
                // Initialisations
                lstSeries = new List<clsSeriesModel>();
                objSeriesLogic = new clsSeriesLogic(objConnection);

                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = "Importing series master files..",
                    dtEventDateTime = DateTime.Now
                });

                arrFileDetails = File.ReadAllLines(sFile)
                                     .Skip(1)
                                     .ToArray();

                if (arrFileDetails == null)
                    return;

                foreach (string sSeries in arrFileDetails)
                {
                    if (sSeries.EndsWith(","))
                    {
                        if (!string.IsNullOrEmpty(sMergeLine))
                            sSeriesDetails = sMergeLine + sSeries;
                        else
                            sSeriesDetails = sSeries;

                        arrSeriesDetails = Regex.Split(sSeriesDetails.Substring(1, sSeriesDetails.Length - 3), sPATTERN);
                        sMergeLine = string.Empty;
                    }
                    else
                    {
                        sMergeLine += sSeries;
                        continue;
                    }

                    if (arrSeriesDetails == null)
                    {
                        sMergeLine = string.Empty;
                        continue;
                    }

                    lstSeries.Add(new clsSeriesModel()
                    {
                        iSeriesId = 0,
                        sSeriesCode = arrSeriesDetails[0],
                        sTopic = arrSeriesDetails[1],
                        sIndicatorName = arrSeriesDetails[2],
                        sShortDefinition = arrSeriesDetails[3],
                        sLongDefinition = arrSeriesDetails[4],
                        sUnitOfMeasure = arrSeriesDetails[5],
                        sPeriodicity = arrSeriesDetails[6],
                        sBasePeriod = arrSeriesDetails[7],
                        sOtherNotes = arrSeriesDetails[8],
                        sAggregationMethod = arrSeriesDetails[9],
                        sLimitationExceptions = arrSeriesDetails[10],
                        sOriginalSourceNotes = arrSeriesDetails[11],
                        sGeneralComments = arrSeriesDetails[12],
                        sSources = arrSeriesDetails[13],
                        sStatisticalMethodology = arrSeriesDetails[14],
                        sDevelopmentRelevance = arrSeriesDetails[15],
                        sSourceLinks = arrSeriesDetails[16],
                        sWebLinks = arrSeriesDetails[17],
                        sRelatedIndicators = arrSeriesDetails[18],
                        sLicenseType = arrSeriesDetails[19],
                    });
                }

                objConnection.BeginTransaction();
                foreach (clsSeriesModel objSeriesModel in lstSeries)
                {
                    objSeriesLogic.InsertSeries(objSeriesModel);
                }
                objConnection.CommitTransaction();

                if (File.Exists(sFile))
                {
                    if (File.Exists(string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile))))
                        File.Delete(string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile)));

                    File.Move(sFile, string.Format("{0}\\{1}", sImportedPath, Path.GetFileName(sFile)));
                }

                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = "Series has been imported successfully.",
                    dtEventDateTime = DateTime.Now
                });

            }
            catch (Exception e)
            {
                objConnection.RollBackTransaction();
                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = e.Message,
                    dtEventDateTime = DateTime.Now,
                });
            }
            finally
            {
                if (arrFileDetails != null)
                    arrFileDetails = null;

                if (arrSeriesDetails != null)
                    arrSeriesDetails = null;

                if (lstSeries != null)
                {
                    lstSeries.Clear();
                    lstSeries = null;
                }

                if (objSeriesLogic != null)
                {
                    objSeriesLogic.Dispose();
                    objSeriesLogic = null;
                }
            }
        }

        #endregion

        #region Protected methods

        /// <summary>Call event.</summary>
        /// <param name="e">Event details</param>
        protected virtual void OnEvLogEvent(clsMessageEventsArgs e)
        {
            evLogEvent?.Invoke(this, e);
        }

        #endregion

        #region Public methods

        /// <summary>Import data files.</summary>
        public void ExecuteImport()
        {
            // Declarations
            clsSettingModel objSettingModel = null;
            clsSettingLogic objSettingLogic = null;
            string[] arrFiles = null;

            try
            {
                // Initialisation
                _fExecuting = true;
                objSettingModel = new clsSettingModel();
                objSettingLogic = new clsSettingLogic(objConnection);

                OnEvLogEvent(new clsMessageEventsArgs()
                {
                    sMessage = "Import has started...",
                    dtEventDateTime = DateTime.Now
                });

                objSettingModel = objSettingLogic.SelectSetting();

                if (objSettingModel == null)
                {
                    OnEvLogEvent(new clsMessageEventsArgs()
                    {
                        sMessage = "No setting found.",
                        dtEventDateTime = DateTime.Now
                    });
                    return;
                }

                if (!Directory.Exists(objSettingModel.sImportPath))
                {
                    OnEvLogEvent(new clsMessageEventsArgs()
                    {
                        sMessage = "The import path does not exists.",
                        dtEventDateTime = DateTime.Now
                    });
                    return;
                }

                if (!Directory.Exists(objSettingModel.sImportedPath))
                {
                    OnEvLogEvent(new clsMessageEventsArgs()
                    {
                        sMessage = "The imported path does not exists.",
                        dtEventDateTime = DateTime.Now
                    });
                    return;
                }

                arrFiles = Directory.GetFiles(objSettingModel.sImportPath);

                if (arrFiles == null || arrFiles.Count() == 0)
                {
                    OnEvLogEvent(new clsMessageEventsArgs()
                    {
                        sMessage = string.Format("No file found to import at {0}.", objSettingModel.sImportPath),
                        dtEventDateTime = DateTime.Now
                    });
                    return;
                }

                foreach (string sfile in arrFiles)
                {
                    if (!Path.HasExtension(sfile) &&
                       (!Path.GetExtension(sfile)
                             .ToLower()
                             .Equals(".csv") ||
                        !Path.GetExtension(sfile)
                             .ToLower()
                             .Equals(".xlsx")))
                        continue;

                    if (Path.GetExtension(sfile)
                            .ToLower()
                            .Equals(".csv"))
                        ExtractCsvFile(sfile, objSettingModel.sImportedPath);

                    if (Path.GetExtension(sfile)
                            .ToLower()
                            .Equals(".xlsx"))
                        ExtractExclFile(sfile, objSettingModel.sImportedPath);

                }
            }
            finally
            {
                if (objSettingLogic != null)
                {
                    objSettingLogic.Dispose();
                    objSettingLogic = null;
                }

                if (objSettingModel != null)
                    objSettingModel = null;

                _fExecuting = false;
            }
        }

        #endregion



    }
}
