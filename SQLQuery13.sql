USE Faucet4u;  
GO  
CREATE PROCEDURE SelectReferralData
@OriginalUsername varchar(10)
AS

BEGIN

SELECT Username, LinkShortnerEarning, PTPUniqueEarning, PTPNonUnique1Earning, PTPNonUnique2Earning, PTPNonUnique3Earning, LastClaim FROM Users WHERE Referrer = @OriginalUsername

DECLARE @Level1 TABLE(
    Username varchar(10) NOT NULL
);

INSERT INTO @Level1 (Username)
SELECT Username FROM Users WHERE Referrer = @OriginalUsername

DECLARE @Level2 TABLE(
    Username varchar(10) NOT NULL,
	LinkShortnerEarning decimal(9, 8) NOT NULL,
	PTPUniqueEarning decimal(9, 8) NOT NULL,
	PTPNonUnique1Earning decimal(9, 8) NOT NULL,
	PTPNonUnique2Earning decimal(9, 8) NOT NULL,
	PTPNonUnique3Earning decimal(9, 8) NOT NULL,
	LastClaim datetime
);

DECLARE @MaxRows int
SELECT @MaxRows = COUNT(Username) FROM @Level1
DECLARE @RowCounter int = 1
DECLARE @ReferralUsername varchar(10)

WHILE(@RowCounter <= @MaxRows)
BEGIN
SELECT @ReferralUsername = Username FROM (SELECT Username, ROW_NUMBER() OVER (ORDER BY Username ASC) rn FROM @Level1) AS Username WHERE rn = @RowCounter


INSERT INTO @Level2 (Username, LinkShortnerEarning, PTPUniqueEarning, PTPNonUnique1Earning, PTPNonUnique2Earning, PTPNonUnique3Earning, LastClaim)
SELECT Username, LinkShortnerEarning, PTPUniqueEarning, PTPNonUnique1Earning, PTPNonUnique2Earning, PTPNonUnique3Earning, LastClaim FROM Users WHERE Referrer = @ReferralUsername
SET @RowCounter = @RowCounter + 1
END

DECLARE @Level3Referrers TABLE(
    Username varchar(10) NOT NULL
);

SELECT @MaxRows = COUNT(Username) FROM @Level2
SET @RowCounter = 1


WHILE(@RowCounter <= @MaxRows)
BEGIN

SELECT @ReferralUsername = Username FROM (SELECT Username, ROW_NUMBER() OVER (ORDER BY Username ASC) rn FROM @Level2) AS Username WHERE rn = @RowCounter

INSERT INTO @Level3Referrers (Username)
SELECT Username FROM Users WHERE Referrer = @ReferralUsername
SET @RowCounter = @RowCounter + 1
END

DECLARE @Level3 TABLE(
    Username varchar(10) NOT NULL,
	LinkShortnerEarning decimal(9, 8) NOT NULL,
	PTPUniqueEarning decimal(9, 8) NOT NULL,
	PTPNonUnique1Earning decimal(9, 8) NOT NULL,
	PTPNonUnique2Earning decimal(9, 8) NOT NULL,
	PTPNonUnique3Earning decimal(9, 8) NOT NULL,
	LastClaim datetime
);

SELECT @MaxRows = COUNT(Username) FROM @Level3Referrers
SET @RowCounter = 1

WHILE(@RowCounter <= @MaxRows)
BEGIN

SELECT @ReferralUsername = Username FROM (SELECT Username, ROW_NUMBER() OVER (ORDER BY Username ASC) rn FROM @Level3Referrers) AS Username WHERE rn = @RowCounter

INSERT INTO @Level3 (Username, LinkShortnerEarning, PTPUniqueEarning, PTPNonUnique1Earning, PTPNonUnique2Earning, PTPNonUnique3Earning, LastClaim)
SELECT Username, LinkShortnerEarning, PTPUniqueEarning, PTPNonUnique1Earning, PTPNonUnique2Earning, PTPNonUnique3Earning, LastClaim FROM Users WHERE Username = @ReferralUsername
SET @RowCounter = @RowCounter + 1
END

SELECT * FROM @Level2
SELECT * FROM @Level3
END

GO

