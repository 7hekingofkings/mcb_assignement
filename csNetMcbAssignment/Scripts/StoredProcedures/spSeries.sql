-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 28/01/2024
-- Description: Procedure for series.
-- Script name: spSeries.sql
-- ====================================================================================

-- ====================================================================================
-- Create stored procedure spSeries.sql
-- ====================================================================================
IF OBJECT_ID(N'spSERIES', N'P') IS NOT NULL
BEGIN
	DROP PROCEDURE spSERIES
END
GO

CREATE PROCEDURE spSERIES
@SERIES_ID                  INTEGER OUTPUT,
@SERIES_CODE                VARCHAR(25),
@TOPIC                      VARCHAR(MAX),
@INDICATOR_NAME             VARCHAR(200),
@SHORT_DEFINITION           VARCHAR(250),
@LONG_DEFINITION            VARCHAR(MAX),
@UNIT_MEASURE               VARCHAR(50),
@PERIODICITY                VARCHAR(25),
@BASE_PERIOD                VARCHAR(200),
@OTHER_NOTES                VARCHAR(MAX),
@AGGREGATION_METHOD         VARCHAR(100),
@LIMITATION_EXCEPTIONS      VARCHAR(MAX),
@ORIGINAL_SOURCE_NOTES      VARCHAR(MAX),
@GENERAL_COMMENTS           VARCHAR(MAX),
@SOURCES                    VARCHAR(500),
@STATISTICAL_METHODOLOGY    VARCHAR(MAX),
@DEVLOPMENT_RELEVANCE       VARCHAR(MAX),
@SOURCE_LINKS               VARCHAR(250),
@WEB_LINKS                  VARCHAR(250),
@RELATED_INDICATORS         VARCHAR(MAX),
@LICENSE_TYPE               VARCHAR(50) 
WITH ENCRYPTION
AS 
BEGIN TRY
	SET NOCOUNT ON

    IF NOT EXISTS(SELECT 1
                  FROM   tblWDI_SERIES tblSeries
                  WHERE  tblSeries.SERIES_CODE = @SERIES_CODE)
    BEGIN
	   INSERT INTO tblWDI_SERIES
	   ( SERIES_CODE               
       , TOPIC                     
       , INDICATOR_NAME            
       , SHORT_DEFINITION          
       , LONG_DEFINITION           
       , UNIT_MEASURE              
       , PERIODICITY               
       , BASE_PERIOD               
       , OTHER_NOTES               
       , AGGREGATION_METHOD        
       , LIMITATION_EXCEPTIONS     
       , ORIGINAL_SOURCE_NOTES     
       , GENERAL_COMMENTS          
       , SOURCES                   
       , STATISTICAL_METHODOLOGY   
       , DEVLOPMENT_RELEVANCE      
       , SOURCE_LINKS              
       , WEB_LINKS                 
       , RELATED_INDICATORS        
       , LICENSE_TYPE)
	   VALUES 
	   ( @SERIES_CODE                       
       , @TOPIC                     
       , @INDICATOR_NAME            
       , @SHORT_DEFINITION          
       , @LONG_DEFINITION           
       , @UNIT_MEASURE              
       , @PERIODICITY               
       , @BASE_PERIOD               
       , @OTHER_NOTES               
       , @AGGREGATION_METHOD        
       , @LIMITATION_EXCEPTIONS     
       , @ORIGINAL_SOURCE_NOTES     
       , @GENERAL_COMMENTS          
       , @SOURCES                   
       , @STATISTICAL_METHODOLOGY   
       , @DEVLOPMENT_RELEVANCE      
       , @SOURCE_LINKS              
       , @WEB_LINKS                 
       , @RELATED_INDICATORS        
       , @LICENSE_TYPE)

       SET @SERIES_ID = SCOPE_IDENTITY()
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