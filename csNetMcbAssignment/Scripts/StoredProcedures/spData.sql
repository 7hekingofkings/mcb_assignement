-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 30/01/2024
-- Description: Procedure for data.
-- Script name: spData.sql
-- ====================================================================================

-- ====================================================================================
-- Create stored procedure spData
-- ====================================================================================
IF OBJECT_ID(N'spData', N'P') IS NOT NULL
BEGIN
	DROP PROCEDURE spData
END
GO

CREATE PROCEDURE spData
@DATA  ttData READONLY
WITH ENCRYPTION
AS 
BEGIN TRY
	SET NOCOUNT ON
	
    --Update
    UPDATE tblData
    SET    tblData.AMOUNT = tblTemp.AMOUNT
    FROM   tblWDI_DATA tblData
    INNER JOIN tblWDI_COUNTRY tblCountry
	ON         tblCountry.COUNTRY_ID = tblData.COUNTRY_ID
	INNER JOIN tblWDI_SERIES tblSeries
	ON         tblSeries.SERIES_ID = tblData.SERIES_ID
	INNER JOIN tblWDI_PERIOD tblPeriod
	ON         tblPeriod.PERIOD_ID = tblData.PERIOD_ID
	INNER JOIN @DATA tblTemp
	ON         tblTemp.COUNTRY_CODE = tblCountry.COUNTRY_CODE
	AND        tblTemp.SERIES_CODE  = tblSeries.SERIES_CODE
	AND        tblTemp.YEARS        = tblPeriod.YEARS

	-- Insert New
	INSERT INTO tblWDI_DATA
	( COUNTRY_ID
	, SERIES_ID
	, PERIOD_ID
	, AMOUNT)
	SELECT tblCountry.COUNTRY_ID
	,      tblSeries.SERIES_ID
	,      tblPeriod.PERIOD_ID
	,      tblTemp.AMOUNT
	FROM       @DATA tblTemp
	INNER JOIN tblWDI_COUNTRY tblCountry
	ON         tblCountry.COUNTRY_CODE = tblTemp.COUNTRY_CODE
	INNER JOIN tblWDI_SERIES tblSeries
	ON         tblSeries.SERIES_CODE = tblTemp.SERIES_CODE
	INNER JOIN tblWDI_PERIOD tblPeriod
	ON         tblPeriod.YEARS = tblTemp.YEARS
	WHERE NOT EXISTS(SELECT 1
	                 FROM   tblWDI_DATA tblData
					 WHERE  tblData.COUNTRY_ID = tblCountry.COUNTRY_ID
					 AND    tblData.PERIOD_ID = tblPeriod.PERIOD_ID
					 AND    tblData.SERIES_ID = tblSeries.SERIES_ID)
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