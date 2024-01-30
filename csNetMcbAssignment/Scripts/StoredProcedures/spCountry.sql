-- ====================================================================================
-- Author:      Hartish Khoobarry
-- Create date: 28/01/2024
-- Description: Procedure for country.
-- Script name: spCountry.sql
-- ====================================================================================

-- ====================================================================================
-- Create stored procedure spCountry.sql
-- ====================================================================================
IF OBJECT_ID(N'spCOUNTRY', N'P') IS NOT NULL
BEGIN
	DROP PROCEDURE spCOUNTRY
END
GO

CREATE PROCEDURE spCOUNTRY
@COUNTRY_ID                  INTEGER OUTPUT,
@COUNTRY_CODE                VARCHAR(3),           
@SHORT_NAME                  VARCHAR(50),       
@TABLE_NAME                  VARCHAR(200),      
@LONG_NAME                   VARCHAR(500),      
@ALPHA_CODE                  VARCHAR(2),        
@CURRENCY_UNIT               VARCHAR(250),      
@SPECIAL_NOTES               VARCHAR(500),      
@REGION                      VARCHAR(200),      
@INCOME_GROUP                VARCHAR(200),      
@WB_CODE                     VARCHAR(2),        
@NATIONAL_ACCOUNT_BASE_YEAR  VARCHAR(100),      
@NATIONAL_ACCOUNT_REFE_YEAR  VARCHAR(100),      
@SNA_PRICE_VALUATION         VARCHAR(100),      
@LENDING_CATEGORY            VARCHAR(10),       
@OTHER_GROUPS                VARCHAR(10),       
@NATIONAL_ACCOUNTS           VARCHAR(200),      
@CONVERSION_FACTOR           VARCHAR(100),      
@PPP_SURVEY_YEAR             VARCHAR(10),       
@BALANCE_PAYMENT_MANUAL      VARCHAR(50),       
@EXTERNAL_DEBT_STATUS        VARCHAR(20),       
@SYSTEM_TRADE                VARCHAR(100),      
@ACCOUNTING_CONCEPT          VARCHAR(100),      
@IMF_DATA                    VARCHAR(200),      
@POPULATION_SENSUS           VARCHAR(500),      
@HOUSEHOLD_SURVEY            VARCHAR(250),      
@INCOME_EXPENDITURE_DATA     VARCHAR(200),      
@REGISTRATION_COMPLETE       BIT,               
@AGRICULTURAL_CENSUS         VARCHAR(200),      
@INDUSTRIAL_DATA             VARCHAR(50),       
@TRADE_DATA                  VARCHAR(50)    
WITH ENCRYPTION
AS 
BEGIN TRY
	SET NOCOUNT ON

    IF NOT EXISTS(SELECT 1
                  FROM   tblWDI_COUNTRY tblCountry
                  WHERE  tblCountry.COUNTRY_CODE = @COUNTRY_CODE)
    BEGIN
	   INSERT INTO tblWDI_COUNTRY
	   ( COUNTRY_CODE                       
       , SHORT_NAME                      
       , TABLE_NAME                      
       , LONG_NAME                       
       , ALPHA_CODE                      
       , CURRENCY_UNIT                   
       , SPECIAL_NOTES                   
       , REGION                          
       , INCOME_GROUP                    
       , WB_CODE                         
       , NATIONAL_ACCOUNT_BASE_YEAR      
       , NATIONAL_ACCOUNT_REFE_YEAR      
       , SNA_PRICE_VALUATION             
       , LENDING_CATEGORY                
       , OTHER_GROUPS                    
       , NATIONAL_ACCOUNTS               
       , CONVERSION_FACTOR               
       , PPP_SURVEY_YEAR                 
       , BALANCE_PAYMENT_MANUAL          
       , EXTERNAL_DEBT_STATUS            
       , SYSTEM_TRADE                    
       , ACCOUNTING_CONCEPT              
       , IMF_DATA                        
       , POPULATION_SENSUS               
       , HOUSEHOLD_SURVEY                
       , INCOME_EXPENDITURE_DATA         
       , REGISTRATION_COMPLETE          
       , AGRICULTURAL_CENSUS             
       , INDUSTRIAL_DATA                 
       , TRADE_DATA )
	   VALUES 
	   ( @COUNTRY_CODE                       
       , @SHORT_NAME                      
       , @TABLE_NAME                      
       , @LONG_NAME                       
       , @ALPHA_CODE                      
       , @CURRENCY_UNIT                   
       , @SPECIAL_NOTES                   
       , @REGION                          
       , @INCOME_GROUP                    
       , @WB_CODE                         
       , @NATIONAL_ACCOUNT_BASE_YEAR      
       , @NATIONAL_ACCOUNT_REFE_YEAR      
       , @SNA_PRICE_VALUATION             
       , @LENDING_CATEGORY                
       , @OTHER_GROUPS                    
       , @NATIONAL_ACCOUNTS               
       , @CONVERSION_FACTOR               
       , @PPP_SURVEY_YEAR                 
       , @BALANCE_PAYMENT_MANUAL          
       , @EXTERNAL_DEBT_STATUS            
       , @SYSTEM_TRADE                    
       , @ACCOUNTING_CONCEPT              
       , @IMF_DATA                        
       , @POPULATION_SENSUS               
       , @HOUSEHOLD_SURVEY                
       , @INCOME_EXPENDITURE_DATA         
       , @REGISTRATION_COMPLETE          
       , @AGRICULTURAL_CENSUS             
       , @INDUSTRIAL_DATA                 
       , @TRADE_DATA )
      
       SET @COUNTRY_ID = SCOPE_IDENTITY()

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
