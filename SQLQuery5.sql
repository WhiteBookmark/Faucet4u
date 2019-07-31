BEGIN

DECLARE @DirectSourceLink varchar(max) = 'http'
DECLARE @CounterId int
SELECT @CounterId = Id FROM DirectCounter
DECLARE @MaxId int
SELECT @MaxId = max(Id) FROM Direct
	
IF(@CounterId >= @MaxId)
BEGIN
UPDATE DirectCounter SET Id = 0
END
DECLARE @Link varchar(max)
SELECT @Link = Link FROM Direct WHERE Id = @CounterId

IF @DirectSourceLink IS NOT NULL AND NOT EXISTS(SELECT site FROM DirectSource WHERE site = @DirectSourceLink)
BEGIN
	INSERT INTO DirectSource(site) VALUES(@DirectSourceLink)
END

UPDATE DirectSource SET counter = counter +1 WHERE site = @DirectSourceLink
UPDATE DirectCounter SET Id = Id +1

SELECT (SELECT Link FROM Direct WHERE Link = @Link) AS Link
END