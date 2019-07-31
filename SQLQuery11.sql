BEGIN
	DECLARE @IP varchar(20) = '192.168.10.1'
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
						DECLARE @SessionId uniqueidentifier = '6e351b1b-c9a6-4772-982e-d0f8a905587b'
						DECLARE @ConfirmationCodeReplace uniqueidentifier = newid()
						UPDATE Users SET ConfirmationCode = @ConfirmationCodeReplace, @LinkClaimMarker = 0.00000100 WHERE SessionId = @SessionId
						SELECT (SELECT Link FROM LinkShortners WHERE Id = @LinkId) AS ResultantLink,
									 (SELECT ConfirmationCode FROM Users WHERE SessionId = @SessionId) AS ConfirmationCode
						BREAK
					END				
				SET @RowNumber = @RowNumber + 1
			END			
END
