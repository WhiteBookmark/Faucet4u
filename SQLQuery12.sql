--Creating new support ticket
BEGIN
DECLARE @SessionId uniqueidentifier = 'c5efabd4-37f2-4bf0-8d1d-289c9cedab29'
DECLARE @Username varchar(10)
DECLARE @Email varchar(150)

SELECT @Username = Username FROM Users WHERE SessionId = @SessionId
SELECT @Email = Email FROM Users WHERE SessionId = @SessionId

DECLARE @Subject varchar(max) = 'Hello world'
DECLARE @Message varchar(max) = 'So baiscally this is a test message if you dont know'

IF LEN(@Subject) <= 70 AND @Username IS NOT NULL AND @Email IS NOT NULL AND @Username != '' AND @Email != ''
	BEGIN
	INSERT INTO SupportTickets(Subject, Message, Username, Email) VALUES(@Subject, @Message, @Username, @Email)
	END
END

--Getting list of already created support tickets
BEGIN
DECLARE @SessionId uniqueidentifier = 'd542b163-e29e-4865-9ff4-674d2f28b19a'
DECLARE @Username varchar(10)

SELECT @Username = Username FROM Users WHERE SessionId = @SessionId

SELECT Reference, Subject, DateTime FROM SupportTickets WHERE Username = @Username ORDER BY DateTime DESC
END

--Replying to support ticket
BEGIN
DECLARE @SessionId uniqueidentifier = 'c5efabd4-37f2-4bf0-8d1d-289c9cedab29'
DECLARE @Username varchar(10)
DECLARE @Reference uniqueidentifier = 'bfacf5f0-21c2-4103-89b2-9b9bd8636dfa'
DECLARE @ReplyMessage varchar(max) = 'So baiscally this is a test message if you dont know'
DECLARE @Locked bit

SELECT @Username = Username FROM Users WHERE SessionId = @SessionId
SELECT @Locked = Locked FROM SupportTickets WHERE Reference = @Reference

IF @Locked = 'false' AND @ReplyMessage IS NOT NULL AND @ReplyMessage != '' AND @Username IS NOT NULL AND @Username != ''
INSERT INTO SupportTicketsReply(Reference, Username, Reply) VALUES(@Reference, @Username, @ReplyMessage)
END

--Getting list of all reply messages of a particular support ticket
BEGIN
DECLARE @Reference uniqueidentifier = 'bfacf5f0-21c2-4103-89b2-9b9bd8636dfa'

SELECT Reply, DateTime, Username FROM SupportTicketsReply WHERE Reference = @Reference ORDER BY DateTime DESC
END
