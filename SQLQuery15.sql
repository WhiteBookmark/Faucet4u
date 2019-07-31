--Banner rotator cron job
BEGIN

DECLARE @CounterId int
DECLARE @MaxId int

SELECT @CounterId = Id FROM BannerRotatorCounter
SELECT @MaxId = max(Id) FROM BannerRotator
	
IF(@CounterId IS NULL OR @CounterId = '')
BEGIN
INSERT INTO BannerRotatorCounter(Id, TargetLink, ImageLink) VALUES(0, 'http://faucet4all.com', 'http://faucet4all.com')
END
ELSE IF(@CounterId >= @MaxId)
BEGIN
UPDATE BannerRotatorCounter SET Id = 0
END

DECLARE @TargetLink varchar(max), @ImageLink varchar(max)

SELECT @TargetLink = TargetLink FROM BannerRotator WHERE Id = @CounterId
SELECT @ImageLink = ImageLink FROM BannerRotator WHERE Id = @CounterId

UPDATE BannerRotatorCounter SET Id = Id + 1, TargetLink = @TargetLink, ImageLink = @ImageLink

END