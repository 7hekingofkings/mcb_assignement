-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 29/01/2024
-- Description: Creation of country series table.
-- Script name: udCreateWDICountrySeriesTable.cs
-- ====================================================================================

-- ====================================================================================
-- Create table tblWDI_COUNTRY_SERIES
-- ====================================================================================
DECLARE @iEXIST INTEGER = 0
IF OBJECT_ID(N'tblWDI_COUNTRY_SERIES', N'U') IS NOT NULL
BEGIN
	SET @iEXIST  = 1
END

IF @iEXIST = 0
BEGIN
	CREATE TABLE tblWDI_COUNTRY_SERIES
	(
	  COUNTRY_SERIES_ID INTEGER IDENTITY(1,1) NOT NULL,
      COUNTRY_ID        INTEGER               NOT NULL,
	  SERIES_ID         INTEGER               NOT NULL,
	  DESCRIPTIONS      VARCHAR(500)              NULL,
	  CONSTRAINT PK_tblWDI_COUNTRY_SERIES_ID PRIMARY KEY(COUNTRY_SERIES_ID),
	  CONSTRAINT FK_tblWDI_COUNTRY_SERIES_COUNTRY_ID FOREIGN KEY(COUNTRY_ID)
	  REFERENCES tblWDI_COUNTRY(COUNTRY_ID),
	  CONSTRAINT FK_tblWDI_COUNTRY_SERIES_SERIES_ID FOREIGN KEY(SERIES_ID)
	  REFERENCES tblWDI_SERIES(SERIES_ID)
	)
END
GO

