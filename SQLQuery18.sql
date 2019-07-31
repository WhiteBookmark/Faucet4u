USE Faucet4u;  
GO  
CREATE OR ALTER PROCEDURE SelectBonusAds
@SessionIdInput uniqueidentifier
AS
BEGIN
DECLARE @Username varchar(10)
DECLARE @SessionId uniqueidentifier = @SessionIdInput

DECLARE @FilteredBonusAds TABLE(
    Id uniqueidentifier NOT NULL,
	Link varchar(max) NOT NULL,
	Title varchar(max) NOT NULL,
	[Description] varchar(max) NOT NULL
);

SELECT @Username = Username FROM Users WHERE SessionId = @SessionId AND SessionExpiry > getdate()	                   
IF NOT EXISTS(SELECT Username FROM BonusAdsRecord WHERE Username = @Username)
BEGIN
INSERT INTO BonusAdsRecord(Username) VALUES(@Username)
END
DECLARE @TotalLinks int
SELECT @TotalLinks = COUNT(*) FROM BonusAds
DECLARE @RowNumber int = 1
WHILE(@RowNumber <= @TotalLinks)
BEGIN
DECLARE @Id uniqueidentifier
WITH temporaryTable AS (SELECT (ROW_NUMBER() OVER (ORDER BY BonusAds.Creation)) as row,* FROM BonusAds)
SELECT @Id = Id FROM temporaryTable WHERE row = @RowNumber

DECLARE @IdString varchar(60) = convert(varchar(60), @Id)
DECLARE @Counter int 
DECLARE @MaxLimit int
DECLARE @Credit int
DECLARE @MaxOut bit
Select @MaxLimit = Limit, @Credit = Credit, @MaxOut = MaxOut FROM BonusAds WHERE Id = @Id
	                   			
DECLARE @dynamicQuery nvarchar(max) = 'SELECT @Counter = [' + @IdString + '] FROM BonusAdsRecord WHERE Username = ''' + @Username + ''''
EXEC sp_executeSQl @dynamicQuery, N'@Counter int output', @Counter output
IF NOT(@Counter >= @MaxLimit OR @Credit <= 0 OR @MaxOut = 1)
	BEGIN	                   					
		
		INSERT INTO @FilteredBonusAds (Id, Link, Title, Description)
		SELECT Id, Link, Title, Description FROM BonusAds WHERE Id = @Id AND Credit > 0 AND MaxOut = 0
	END	                                
SET @RowNumber = @RowNumber + 1
END		

SELECT * FROM @FilteredBonusAds	                   	  	
END
GO

EXEC SelectBonusAds @SessionIdInput = '7c45ab88-fde5-4f16-9084-49533f87acb0'
