BEGIN
	DECLARE @ConfirmationCode uniqueidentifier = '2907cc7d-3366-4139-922b-ffd641d61fe4'
	UPDATE Users SET Balance = Balance + LinkClaimMarker, LinkClaimMarker = 0.00000000 WHERE ConfirmationCode = @ConfirmationCode
END