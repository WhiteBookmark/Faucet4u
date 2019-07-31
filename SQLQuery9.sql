BEGIN
DECLARE @Link varchar(max) = 'http:\\something'
DECLARE @MaxId int

SELECT @MaxId = max(Id)+1 FROM PTP
INSERT INTO PTP(Id, Link) VALUES(@MaxId, @Link)
END

BEGIN
DECLARE @Id int = 2
DELETE FROM PTP WHERE Id = @Id
UPDATE PTP SET Id = Id -1 WHERE Id > @Id
END