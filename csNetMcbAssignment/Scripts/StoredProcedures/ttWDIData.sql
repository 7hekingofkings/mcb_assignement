-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 30/01/2024
-- Description: Table type for WDI_data.
-- Script name: ttWDIData.sql
-- ====================================================================================

-- ====================================================================================
-- Create tabl type ttData
-- ====================================================================================
IF OBJECT_ID(N'ttData', N'TT') IS NOT NULL
BEGIN
	DROP TYPE ttData
END
GO

CREATE TYPE ttData AS TABLE
( COUNTRY_NAME VARCHAR(250)
, COUNTRY_CODE VARCHAR(3)
, SERIES_NAME  VARCHAR(MAX)
, SERIES_CODE  VARCHAR(200)
, YEARS        INT
, AMOUNT       FLOAT)
GO


