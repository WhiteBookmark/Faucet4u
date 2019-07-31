--Alter table LinkShortnersRecord drop constraint [DF__LinkShort__66f2e__2E70E1FD]
--Alter table LinkShortnersRecord drop column [66f2e0b3-db95-4bed-8fd1-df2e5e84f13f]

DECLARE @sql NVARCHAR(MAX)
WHILE 1=1
BEGIN
    SELECT TOP 1 @sql = N'alter table LinkShortnersRecord drop constraint ['+dc.NAME+N']'
    from sys.default_constraints dc
    JOIN sys.columns c
        ON c.default_object_id = dc.object_id
    WHERE 
        dc.parent_object_id = OBJECT_ID('LinkShortnersRecord')
    AND c.name = N'3d317b2b-6848-4f5f-8d98-16c20af78f94'
    IF @@ROWCOUNT = 0 BREAK
    EXEC (@sql)
END

Alter table LinkShortnersRecord drop column [3d317b2b-6848-4f5f-8d98-16c20af78f94]

select * from LinkShortnersRecord