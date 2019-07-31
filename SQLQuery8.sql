BEGIN
DECLARE @Username varchar(10)
DECLARE @ConfirmationCode uniqueidentifier = '04448e48-4e8f-40c3-a9f0-1b2911db1207'
DECLARE @LinkClaimMarker decimal(9, 8)

SELECT @Username = Username FROM Users WHERE ConfirmationCode = @ConfirmationCode
SELECT @LinkClaimMarker = LinkClaimMarker FROM Users WHERE ConfirmationCode = @ConfirmationCode

UPDATE Users SET LinkClaimMarker = 0.00000000 WHERE ConfirmationCode = @ConfirmationCode
EXEC UpdateBalance @UsernameInput = @Username, @AmountInput = @LinkClaimMarker
END