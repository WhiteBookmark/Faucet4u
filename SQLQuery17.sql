BEGIN
DECLARE @Available bit = 0
DECLARE @Username varchar(10)
DECLARE @SessionId uniqueidentifier = @SessionIdInput
DECLARE @BonusAdConfirmationCode uniqueidentifier = newid()
DECLARE @BonusAdMarkerLink varchar(max)
SELECT @BonusAdMarkerLink = Link FROM BonusAds WHERE Id = @Id
SELECT @Username FROM Users WHERE SessionId = @SessionId AND SessionExpiry > getdate()

IF NOT EXISTS(SELECT Username FROM BonusAdsRecord WHERE Username = @Username)
    BEGIN
	INSERT INTO BonusAdsRecord(Username) VALUES(@Username)
	END
DECLARE @IP varchar(20) = @IPAddress
IF NOT EXISTS(SELECT IP FROM LinkShortnersRecord WHERE IP = @IP)
    BEGIN
	INSERT INTO LinkShortnersRecord(IP) VALUES(@IP)
END

DECLARE @TotalLinks int
SELECT @TotalLinks = COUNT(*) FROM LinkShortners
DECLARE @RowNumber int = 1
WHILE(@RowNumber <= @TotalLinks)
	BEGIN
	    DECLARE @LinkId uniqueidentifier
	    WITH temporaryTable AS (SELECT (ROW_NUMBER() OVER (ORDER BY LinkShortners.Limit)) as row,* FROM LinkShortners)
	    SELECT @LinkId = Id FROM temporaryTable WHERE row = @RowNumber

	    DECLARE @LinkIdString varchar(60) = convert(varchar(60), @LinkId)
	    DECLARE @Counter int 
	    DECLARE @MaxLimit int
	    Select @MaxLimit = Limit from LinkShortners where Id = @LinkId
	                   			
	    DECLARE @dynamicQuery nvarchar(max) = 'SELECT @Counter = [' + @LinkIdString + '] FROM LinkShortnersRecord WHERE IP = ''' + @IP + ''''
	    EXEC sp_executeSQl @dynamicQuery, N'@Counter int output', @Counter output
	    IF NOT(@Counter >= @MaxLimit)
	        BEGIN	            
			    
				UPDATE Users SET BonusAdConfirmationCode = @BonusAdConfirmationCode, BonusAdMarkerIP = @IP, BonusAdMarkerId = @LinkId, BonusAdMarkerLink = @BonusAdMarkerLink WHERE SessionId = @SessionId AND SessionExpiry > getdate()
				SELECT (SELECT Link FROM LinkShortners WHERE Id = @LinkId) AS ResultantLink,
					            (SELECT BonusAdConfirmationCode FROM Users WHERE SessionId = @SessionId AND SessionExpiry > getdate()) AS ConfirmationCode,
					            (SELECT 1 AS Available)
				SET @Available = 1
				BREAK
	        END	                                
	    SET @RowNumber += 1
	END		
	IF(@Available = 0)		
	BEGIN		
	SELECT (SELECT BonusAdMarkerLink FROM Users WHERE SessionId = @SessionId AND SessionExpiry > getdate()) AS ResultantLink,
		   (SELECT BonusAdConfirmationCode FROM Users WHERE SessionId = @SessionId AND SessionExpiry > getdate()) AS ConfirmationCode,
		   (SELECT 0 AS Available)
	END		
END