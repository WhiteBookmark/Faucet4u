BEGIN
	DECLARE @SessionId uniqueidentifier = '7ab23d2f-572f-45bd-9d9c-001650b1e6dc'
	DECLARE @Amount DECIMAL(9, 8) = '0.10000000'
	DECLARE @WalletType varchar(50) = 'Bitcoin'
	DECLARE @WalletAddress varchar(max) = 'Somewhere'
	DECLARE @Username varchar(10)
	DECLARE @Balance DECIMAL(9, 8)
	DECLARE @MinimumWithdrawal DECIMAL(9, 8)
	

	SELECT @Username = Username FROM Users WHERE SessionId = @SessionId AND  SessionExpiry > getdate()
	SELECT @Balance = Balance FROM Users WHERE SessionId = @SessionId AND  SessionExpiry > getdate()
	SELECT @MinimumWithdrawal = Value FROM Settings WHERE Name = 'MinimumWithdrawal'

	IF @Amount IS NOT NULL AND @Amount <= @Balance AND @Amount >= @MinimumWithdrawal
	AND @WalletType IS NOT NULL AND @WalletType != ''
	AND @WalletAddress IS NOT NULL AND @WalletAddress != ''
	AND @Username IS NOT NULL AND @Username != ''	
		BEGIN
			INSERT INTO Withdrawal(Username, RequestedDate, RequestedAmount, WalletType, WalletAddress) VALUES(@Username, getdate(), @Amount, @WalletType, @WalletAddress)
			UPDATE Users SET Balance = Balance - @Amount WHERE Username = @Username
		END
END