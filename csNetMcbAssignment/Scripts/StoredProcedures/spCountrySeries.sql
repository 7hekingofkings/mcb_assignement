-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 29/01/2024
-- Description: Procedure for country sereis.
-- Script name: spCountrySeries.sql
-- ====================================================================================

-- ====================================================================================
-- Create stored procedure spCOUNTRY_SERIES
-- ====================================================================================
IF OBJECT_ID(N'spCOUNTRY_SERIES', N'P') IS NOT NULL
BEGIN
	DROP PROCEDURE spCOUNTRY_SERIES
END
GO

CREATE PROCEDURE spCOUNTRY_SERIES
@COUNTRY_SERIES_ID INTEGER OUTPUT,
@COUNTRY_CODE      VARCHAR(3),           
@SERIES_CODE       VARCHAR(200),
@DESCRIPTIONS      VARCHAR(500)
WITH ENCRYPTION
AS 
BEGIN TRY
	SET NOCOUNT ON

    DECLARE @COUNTRY_ID INTEGER
    DECLARE @SERIES_ID INTEGER


    SET @COUNTRY_ID = (SELECT tblCountry.COUNTRY_ID
                       FROM   tblWDI_COUNTRY tblCountry
					   WHERE  tblCountry.COUNTRY_CODE = @COUNTRY_CODE)

	SET @SERIES_ID = (SELECT tblSeries.SERIES_ID
                      FROM   tblWDI_SERIES tblSeries
					  WHERE  tblSeries.SERIES_CODE = @SERIES_CODE)

    IF NOT EXISTS(SELECT 1
                  FROM   tblWDI_COUNTRY_SERIES tblCountrySeries
                  WHERE  tblCountrySeries.COUNTRY_ID = @COUNTRY_ID
				  AND    tblCountrySeries.SERIES_ID = @SERIES_ID)
    BEGIN
		INSERT INTO tblWDI_COUNTRY_SERIES
		( COUNTRY_ID
		, SERIES_ID
		, DESCRIPTIONS)
		VALUES
		( @COUNTRY_ID
		, @SERIES_ID
		, @DESCRIPTIONS)

		SET @COUNTRY_SERIES_ID = SCOPE_IDENTITY()
    END
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