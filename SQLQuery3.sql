BEGIN

	DECLARE @PTPSourceLink varchar(max) = 'google'
	DECLARE @RandomNumber int
	SELECT @RandomNumber = FLOOR(RAND()*(5-1+1))+1
	
	IF EXISTS(SELECT Keyword FROM PTPBlacklist WHERE Keyword like '%' + @PTPSourceLink + '%')
		BEGIN
		SELECT '#/' AS Link, 'false' as Valid
		END
	ELSE IF(@RandomNumber <= 2)
		BEGIN
		SELECT 'Invisible' AS Link, 'true' as Valid
		END
	ELSE
		BEGIN
		SELECT '#/' AS Link, 'true' as Valid
		END
END