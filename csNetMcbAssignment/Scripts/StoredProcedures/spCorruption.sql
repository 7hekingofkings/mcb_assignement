-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 30/01/2024
-- Description: Procedure for corruption index.
-- Script name: spCorruption.sql
-- ====================================================================================

-- ====================================================================================
-- Create stored procedure spCORRUPTION
-- ====================================================================================
IF OBJECT_ID(N'spCORRUPTION', N'P') IS NOT NULL
BEGIN
	DROP PROCEDURE spCORRUPTION
END
GO

CREATE PROCEDURE spCORRUPTION
@DATA  ttCORRUPTION READONLY
WITH ENCRYPTION
AS 
BEGIN TRY
	SET NOCOUNT ON
	
    --Update
    UPDATE tblData
    SET    tblData.CPI_CORE = tblTemp.CPI_SCORE
	,      tblData.RANKED   = tblTemp.RANKS
	,      tblData.SOURCES  = tblTemp.SOURCES
	,      tblData.ERROR    = tblTemp.STANDARD_ERROR
    FROM   tblWDI_CORRUPTION tblData
    INNER JOIN tblWDI_COUNTRY tblCountry
	ON         tblCountry.COUNTRY_ID = tblData.COUNTRY_ID
	INNER JOIN tblWDI_PERIOD tblPeriod
	ON         tblPeriod.PERIOD_ID = tblData.PERIOD_ID
	INNER JOIN @DATA tblTemp
	ON         tblTemp.COUNTRY_CODE = tblCountry.COUNTRY_CODE
	AND        tblTemp.YEARS        = tblPeriod.YEARS

	-- Insert New
	INSERT INTO tblWDI_CORRUPTION
	( COUNTRY_ID
	, PERIOD_ID
	, CPI_CORE
	, RANKED
	, SOURCES
	, ERROR)
	SELECT tblCountry.COUNTRY_ID
	,      tblPeriod.PERIOD_ID
	,      tblTemp.CPI_SCORE
	,      tblTemp.RANKS
	,      tblTemp.SOURCES
	,      tblTemp.STANDARD_ERROR
	FROM       @DATA tblTemp
	INNER JOIN tblWDI_COUNTRY tblCountry
	ON         tblCountry.COUNTRY_CODE = tblTemp.COUNTRY_CODE
	INNER JOIN tblWDI_PERIOD tblPeriod
	ON         tblPeriod.YEARS = tblTemp.YEARS
	WHERE NOT EXISTS(SELECT 1
	                 FROM   tblWDI_DATA tblData
					 WHERE  tblData.COUNTRY_ID = tblCountry.COUNTRY_ID
					 AND    tblData.PERIOD_ID = tblPeriod.PERIOD_ID)
END TRY           
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
  
    SELECT   
        @ErrorMessage  = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState    = ERROR_STATE();  
    
    RAISERROR (@ErrorMessage,  
               @ErrorSeverity,  
               @ErrorState);  
END CATCH
GO