USE Faucet4u;  
GO  
CREATE PROCEDURE UpdateBalance   
    @UsernameInput nvarchar(10),   
    @AmountInput DECIMAL(9, 8)   
AS   

BEGIN
	DECLARE @Username varchar(10) = @UsernameInput
	DECLARE @Amount DECIMAL(9, 8) = @AmountInput
	DECLARE @Referrer1 varchar(10)
	DECLARE @Referrer2 varchar(10)
	DECLARE @Referrer3 varchar(10)
	DECLARE @Amount1 DECIMAL(9, 8)
	DECLARE @Amount2 DECIMAL(9, 8)
	DECLARE @Amount3 DECIMAL(9, 8)


	SELECT @Referrer1 = Username FROM Users WHERE Referrer = @Username
	SELECT @Referrer2 = Username FROM Users WHERE Referrer = @Referrer1
	SELECT @Referrer3 = Username FROM Users WHERE Referrer = @Referrer2

	DECLARE @Level1 int
	DECLARE @Level2 int
	DECLARE @Level3 int

	SELECT @Level1 = Value FROM Settings WHERE Name = 'Level1'
	SELECT @Level2 = Value FROM Settings WHERE Name = 'Level2'
	SELECT @Level3 = Value FROM Settings WHERE Name = 'Level3'

	SET @Amount1 = convert(DECIMAL(9, 8), @Amount*@Level1/100)
	SET @Amount2 = convert(DECIMAL(9, 8), @Amount*@Level2/100)
	SET @Amount3 = convert(DECIMAL(9, 8), @Amount*@Level3/100)

	UPDATE Users SET Balance = Balance + @Amount WHERE Username = @Username
	UPDATE Users SET Balance = Balance + @Amount1 WHERE Username = @Referrer1
	UPDATE Users SET Balance = Balance + @Amount2 WHERE Username = @Referrer2
	UPDATE Users SET Balance = Balance + @Amount3 WHERE Username = @Referrer3
END
GO  

--EXEC UpdateBalance @UsernameInput = N'Raz0rSharp', @AmountInput = N'0.00010000'; 
--DECLARE @X1 varchar(20) = 'Raz0rSharp'
--DECLARE @X2 varchar(60) = '0.00010000'
--EXEC UpdateBalance @UsernameInput = @X1, @AmountInput = @X2;
