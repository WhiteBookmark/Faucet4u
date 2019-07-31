BEGIN

	DECLARE @PTPSourceLink varchar(max) = 'www.google.com'
	DECLARE @CounterId int
	SELECT @CounterId = Id FROM PTPCounter
	DECLARE @MaxId int
	SELECT @MaxId = max(Id) FROM PTP
	
	IF(@CounterId >= @MaxId)
		BEGIN
		UPDATE PTPCounter SET Id = 0
		END
	DECLARE @Link varchar(max)
	SELECT @Link = Link FROM PTP WHERE Id = @CounterId

	IF @PTPSourceLink IS NOT NULL AND NOT EXISTS(SELECT site FROM PTPSource WHERE site = @PTPSourceLink)
        BEGIN
			INSERT INTO PTPSource(site) VALUES(@PTPSourceLink)
		END

	UPDATE PTPSource SET counter = counter +1 WHERE site = @PTPSourceLink
	UPDATE PTPCounter SET Id = Id +1

	SELECT (SELECT Link FROM PTP WHERE Link = @Link) AS Link
END
/*This query is for confirming that the user has watche PTP successfully*/
BEGIN
	DECLARE @IP varchar(max) = '192.168.10.1'
	DECLARE @Username varchar(max) = 'Raz0rSharp'
	DECLARE @AmountToCredit DECIMAL (9, 8)

	IF NOT EXISTS(SELECT UniqueIP FROM PTPIP WHERE UniqueIP = @IP AND Username = @Username)
		BEGIN
		IF @Username IS NOT NULL INSERT INTO PTPIP(Username, UniqueIP) VALUES(@Username, @IP)
		SELECT @AmountToCredit = Value FROM Settings WHERE Name = 'PTPUniqueIPCredit'
		UPDATE Users SET Balance = Balance + (SELECT CONVERT(DECIMAL (9, 8), @AmountToCredit)) WHERE Username = @Username
		END
	ELSE IF NOT EXISTS(SELECT NonUniqueIP1 FROM PTPIP WHERE NonUniqueIP1 = @IP AND Username = @Username)
		BEGIN
		UPDATE PTPIP SET NonUniqueIP1 = @IP WHERE UniqueIP = @IP AND Username = @Username
		SELECT @AmountToCredit = Value FROM Settings WHERE Name = 'PTPNonUniqueIP1Credit'
		UPDATE Users SET Balance = Balance + (SELECT CONVERT(DECIMAL (9, 8), @AmountToCredit)) WHERE Username = @Username
		END
	ELSE IF NOT EXISTS(SELECT NonUniqueIP2 FROM PTPIP WHERE NonUniqueIP2 = @IP AND Username = @Username)
		BEGIN
		UPDATE PTPIP SET NonUniqueIP2 = @IP WHERE UniqueIP = @IP AND Username = @Username
		SELECT @AmountToCredit = Value FROM Settings WHERE Name = 'PTPNonUniqueIP2Credit'
		UPDATE Users SET Balance = Balance + (SELECT CONVERT(DECIMAL (9, 8), @AmountToCredit)) WHERE Username = @Username
		END	
	ELSE IF NOT EXISTS(SELECT NonUniqueIP3 FROM PTPIP WHERE NonUniqueIP3 = @IP AND Username = @Username)
		BEGIN
		UPDATE PTPIP SET NonUniqueIP3 = @IP WHERE UniqueIP = @IP AND Username = @Username
		SELECT @AmountToCredit = Value FROM Settings WHERE Name = 'PTPNonUniqueIP3Credit'
		UPDATE Users SET Balance = Balance + (SELECT CONVERT(DECIMAL (9, 8), @AmountToCredit)) WHERE Username = @Username
		END
END