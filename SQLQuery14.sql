USE Faucet4u;  
GO  
CREATE OR ALTER PROCEDURE InsertCheatHistory
@UsernameInput varchar(10),
@Case int
AS

BEGIN

DECLARE @Reason varchar(max)
DECLARE @CheatCounter int

IF (@Case = 1) 
BEGIN 
SET @Reason = 'Trying to claim without visitng link shortners i.e. direct claims (free claims)'
SELECT @CheatCounter = Value FROM Settings WHERE Name = 'CheatCounterForDirectClaims'
END
IF (@Case = 2)
BEGIN
SET @Reason = 'Using faucethubaddress that is already acquired by another user, possible multi accounts'
SELECT @CheatCounter = Value FROM Settings WHERE Name = 'CheatCounterForDuplicateAccount'
END
INSERT INTO CheatHistory(Username, CheatLevel, Reason) VALUES (@UsernameInput, @CheatCounter, @Reason)

END
GO

--Case 1 = Direct claims
--Case 2 = Duplicate faucethub address

EXEC InsertCheatHistory @UsernameInput = 'Raz0rSharp', @Case = 1
