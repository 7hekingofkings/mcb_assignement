-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 27/01/2023
-- Description: Creation of WDI series table.
-- Script name: udCreateWDISeriesTable.sql
-- ====================================================================================

-- ====================================================================================
-- Create table tblWDI_SERIES
-- ====================================================================================
DECLARE @iEXIST INTEGER = 0
IF OBJECT_ID(N'tblWDI_SERIES', N'U') IS NOT NULL
BEGIN
	SET @iEXIST  = 1
END

IF @iEXIST = 0
BEGIN
	CREATE TABLE tblWDI_SERIES
	(
		SERIES_ID                  INTEGER IDENTITY(1,1) NOT NULL,
        SERIES_CODE                VARCHAR(25)           NOT NULL,
		TOPIC                      VARCHAR(MAX)              NULL,
		INDICATOR_NAME             VARCHAR(200)              NULL,
		SHORT_DEFINITION           VARCHAR(250)              NULL,
		LONG_DEFINITION            VARCHAR(MAX)              NULL,
		UNIT_MEASURE               VARCHAR(50)               NULL,
		PERIODICITY                VARCHAR(25)               NULL,
		BASE_PERIOD                VARCHAR(200)              NULL,
		OTHER_NOTES                VARCHAR(MAX)              NULL,
		AGGREGATION_METHOD         VARCHAR(100)              NULL,
		LIMITATION_EXCEPTIONS      VARCHAR(MAX)              NULL,
		ORIGINAL_SOURCE_NOTES      VARCHAR(MAX)              NULL,
		GENERAL_COMMENTS           VARCHAR(MAX)              NULL,
		SOURCES                    VARCHAR(500)              NULL,
		STATISTICAL_METHODOLOGY    VARCHAR(MAX)              NULL,
		DEVLOPMENT_RELEVANCE       VARCHAR(MAX)              NULL,
		SOURCE_LINKS               VARCHAR(250)              NULL,
		WEB_LINKS                  VARCHAR(250)              NULL,
		RELATED_INDICATORS         VARCHAR(MAX)              NULL,
		LICENSE_TYPE               VARCHAR(50)               NULL
		CONSTRAINT PK_tblWDI_SERIES_ID PRIMARY KEY(SERIES_ID),
	)
END
GO
