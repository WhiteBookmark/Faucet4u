Use Faucet4u;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Users')
BEGIN  

CREATE TABLE [Users] (
    [Id]                         UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Username]                   VARCHAR (10)     NOT NULL,
    [Email]                      VARCHAR (350)    NOT NULL,
    [Password]                   VARCHAR (60)     NOT NULL,
    [IsConfirmed]                BIT              CONSTRAINT [DF_Users_isConfirmed] DEFAULT ((0)) NOT NULL,
    [ConfirmationCode]           UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [ConfirmationCodeExpiryTime] DATETIME         DEFAULT (getdate()) NOT NULL,
    [IP]                         VARCHAR (20)     DEFAULT ('0.0.0.0') NOT NULL,
    [Country]                    VARCHAR (50)     DEFAULT ('Other') NOT NULL,
    [SessionId]                  UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [SessionExpiry]              DATETIME         DEFAULT (getdate()) NOT NULL,
    [FaucetHubBitcoinAddress]    VARCHAR (40)     NULL,
    [Locked]                     BIT              DEFAULT ((0)) NOT NULL,
    [CheatCounter]               INT              DEFAULT ((0)) NOT NULL,
    [Referrer]                   VARCHAR (10)     NULL,
    [Balance]                    DECIMAL (9, 8)   DEFAULT ((0.00000000)) NOT NULL,
    [OfferwallBalance]           DECIMAL (9, 8)   DEFAULT ((0.00000000)) NOT NULL,
    [PurchaseBalance]            DECIMAL (9, 8)   DEFAULT ((0.00000000)) NOT NULL,
    [LinkClaimMarker]            DECIMAL (9, 8)   DEFAULT ((0.00000000)) NOT NULL,
    [LinkClaimMarkerLink]        VARCHAR (MAX)    DEFAULT ('faucet4all.com') NOT NULL,
    [LinkClaimInterval]          DATETIME         DEFAULT (getdate()) NOT NULL,
    [TotalWithdrawn]             DECIMAL (9, 8)   DEFAULT ((0.00000000)) NOT NULL,
    [TotalOfferwallWithdrawn]    DECIMAL (9, 8)   DEFAULT ((0.00000000)) NOT NULL,
    [WithdrawalAmount]           DECIMAL (9, 8)   DEFAULT ((0.00000000)) NOT NULL,
    [OfferwallWithdrawalAmount]  DECIMAL (9, 8)   DEFAULT ((0.00000000)) NOT NULL,
    [LinkShortnerEarning]        DECIMAL (9, 8)   DEFAULT ((0.00000000)) NOT NULL,
    [OfferwallEarning]           DECIMAL (9, 8)   DEFAULT ((0.00000000)) NOT NULL,
    [PTPUniqueEarning]           DECIMAL (9, 8)   DEFAULT ((0.00000000)) NOT NULL,
    [PTPNonUnique1Earning]       DECIMAL (9, 8)   DEFAULT ((0.00000000)) NOT NULL,
    [PTPNonUnique2Earning]       DECIMAL (9, 8)   DEFAULT ((0.00000000)) NOT NULL,
    [PTPNonUnique3Earning]       DECIMAL (9, 8)   DEFAULT ((0.00000000)) NOT NULL,
    [LastLogin]                  DATETIME         NULL,
    [LastClaim]                  DATETIME         NULL,
    [DateRegistered]             DATETIME         DEFAULT (getdate()) NOT NULL,
    [JustClaimed]                BIT              DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY NONCLUSTERED ([Id] ASC),
    CONSTRAINT [Unique_Username] UNIQUE CLUSTERED ([Username] ASC),
    CONSTRAINT [Unique_Email] UNIQUE NONCLUSTERED ([Email] ASC)
);

END

IF COL_LENGTH('Users','Username') IS NULL
BEGIN
PRINT 'Condition satisfied for Column'
END

CREATE TABLE [dbo].[SupportTicketsReply] (
    [ReplyId]   UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Reference] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [DateTime]  DATETIME         DEFAULT (getdate()) NOT NULL,
    [Reply]     VARCHAR (MAX)    NOT NULL,
    [Username]  VARCHAR (10)     NULL,
    [Read]      BIT              DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SupportTicketsReply] PRIMARY KEY CLUSTERED ([ReplyId] ASC)
);

CREATE TABLE [dbo].[SupportTickets] (
    [Reference] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Subject]   VARCHAR (70)     NOT NULL,
    [Message]   VARCHAR (MAX)    NOT NULL,
    [Locked]    BIT              DEFAULT ((0)) NOT NULL,
    [Username]  VARCHAR (10)     NULL,
    [Email]     VARCHAR (350)    NOT NULL,
    [Read]      BIT              DEFAULT ((0)) NOT NULL,
    [DateTime]  DATETIME         DEFAULT (getdate()) NOT NULL
);

CREATE TABLE [dbo].[DepositHistory] (
    [Reference]     UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Username]      VARCHAR (10)     NOT NULL,
    [DepositDate]   DATETIME         DEFAULT (getdate()) NOT NULL,
    [Amount]        DECIMAL (9, 8)   NOT NULL,
    [WalletType]    VARCHAR (150)    NOT NULL,
    [WalletAddress] VARCHAR (MAX)    NOT NULL,
    [Remarks]       VARCHAR (MAX)    NULL
);

CREATE TABLE [dbo].[OfferwallHistory] (
    [Reference]    UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Username]     VARCHAR (12)     NOT NULL,
    [Offerwall]    VARCHAR (MAX)    NOT NULL,
    [Amount]       DECIMAL (9, 8)   DEFAULT ((0.00000000)) NOT NULL,
    [Status]       VARCHAR (70)     DEFAULT ('Pending') NOT NULL,
    [DateTime]     DATETIME         DEFAULT (getdate()) NOT NULL,
    [CampaignId]   INT              DEFAULT ((0)) NOT NULL,
    [CampaignName] VARCHAR (50)     DEFAULT ('Not found') NOT NULL
);

CREATE TABLE [dbo].[DepositMethod] (
    [Name]    VARCHAR (150) NOT NULL,
    [Address] VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Name] ASC)
);

CREATE TABLE [dbo].[Settings] (
    [Name]  VARCHAR (1000) NOT NULL,
    [Value] VARCHAR (MAX)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Name] ASC)
);

INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'CheatCounterForDirectClaims',N'1');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'CheatCounterForDuplicateAccount',N'1');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'CheatCounterForMultipleAccounts',N'5');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'CheatCounterForProxy',N'5');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'DirectNonUniqueIP1Credit',N'0.00010000');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'DirectNonUniqueIP2Credit',N'0.00010000');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'DirectNonUniqueIP3Credit',N'0.00001000');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'DirectUniqueIPCredit',N'0.00001000');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'Level1',N'10');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'Level2',N'5');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'Level3',N'2');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'OfferwallLevel1',N'10');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'OfferwallLevel2',N'5');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'OfferwallLevel3',N'2');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'LinkClaimMinutes',N'3');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'LinkClaimCredit',N'0.00000100');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'MinimumWithdrawal',N'0.00100000');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'PTPDirectRatio',N'2');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'PTPNonUniqueIP1Credit',N'0.00010000');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'PTPNonUniqueIP2Credit',N'0.00010000');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'PTPNonUniqueIP3Credit',N'0.00001000');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'PTPTotalRatio',N'5');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'PTPUniqueIPCredit',N'0.00001000');
GO
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'ForumLink',N'http://www.ems.com');
GO



CREATE TABLE [dbo].[PTPSource] (
    [site]    VARCHAR (1000) NOT NULL,
    [counter] BIGINT         DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([site] ASC)
);

CREATE TABLE [dbo].[PTPIP] (
    [Username]     VARCHAR (50) NOT NULL,
    [UniqueIP]     VARCHAR (50) NOT NULL,
    [NonUniqueIP1] VARCHAR (50) DEFAULT (NULL) NULL,
    [NonUniqueIP2] VARCHAR (50) DEFAULT (NULL) NULL,
    [NonUniqueIP3] VARCHAR (50) DEFAULT (NULL) NULL
);

CREATE TABLE [dbo].[PTPCounter] (
    [Id] INT DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[PTPBlacklist] (
    [Keyword] VARCHAR (MAX) NOT NULL
);

CREATE TABLE [dbo].[PTP] (
    [Id]       INT           DEFAULT ((1)) NOT NULL,
    [Creation] DATETIME      DEFAULT (getdate()) NOT NULL,
    [Link]     VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Mining] (
    [Percentage] INT DEFAULT ((20)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Percentage] ASC)
);

INSERT INTO [Mining] ([Percentage]) VALUES (
30);
GO

CREATE TABLE [dbo].[Logs] (
    [Guid]      UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [DateTime]  DATETIME         DEFAULT (getdate()) NOT NULL,
    [IP]        VARCHAR (20)     NULL,
    [Username]  VARCHAR (12)     NULL,
    [Type]      VARCHAR (10)     NOT NULL,
    [Message]   VARCHAR (MAX)    NOT NULL,
    [Exception] VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED ([Guid] ASC)
);


CREATE TABLE [dbo].[LoginHistory] (
    [Id]       UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [DateTime] DATETIME         DEFAULT (getdate()) NOT NULL,
    [Username] VARCHAR (12)     NOT NULL,
    [Success]  BIT              DEFAULT ((0)) NOT NULL
);

CREATE TABLE [dbo].[LinkShortnersRecord] (
    [IP]                                   VARCHAR (20) NOT NULL,    
    PRIMARY KEY CLUSTERED ([IP] ASC)
);

CREATE TABLE [dbo].[LinkShortners] (
    [Id]       UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Creation] DATETIME         DEFAULT (getdate()) NOT NULL,
    [Link]     VARCHAR (128)    NOT NULL,
    [Limit]    INT              DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Unique_Link] UNIQUE NONCLUSTERED ([Link] ASC)
);

CREATE TABLE [dbo].[DirectSource] (
    [site]    VARCHAR (1000) NOT NULL,
    [counter] BIGINT         NOT NULL
);

CREATE TABLE [dbo].[DirectIP] (
    [Username]     VARCHAR (50) NOT NULL,
    [UniqueIP]     VARCHAR (50) NOT NULL,
    [NonUniqueIP1] VARCHAR (50) NULL,
    [NonUniqueIP2] VARCHAR (50) NULL,
    [NonUniqueIP3] VARCHAR (50) NULL
);

CREATE TABLE [dbo].[DirectCounter] (
    [Id] INT NOT NULL
);

CREATE TABLE [dbo].[Direct] (
    [Id]       INT           NOT NULL,
    [Creation] DATETIME      NOT NULL,
    [Link]     VARCHAR (MAX) NOT NULL
);

CREATE TABLE [dbo].[CheatHistory] (
    [Reference]  UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Username]   VARCHAR (10)     NOT NULL,
    [CheatLevel] INT              DEFAULT ((1)) NOT NULL,
    [Reason]     VARCHAR (MAX)    NOT NULL,
    PRIMARY KEY CLUSTERED ([Reference] ASC)
);

CREATE TABLE [dbo].[BannerRotatorCounter] (
    [Id]         INT           DEFAULT ((0)) NOT NULL,
    [TargetLink] VARCHAR (MAX) NULL,
    [ImageLink]  VARCHAR (MAX) NULL
);

CREATE TABLE [dbo].[BannerRotator] (
    [Id]         INT           DEFAULT ((1)) NOT NULL,
    [ImageLink]  VARCHAR (MAX) NOT NULL,
    [TargetLink] VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[BannerNetworkCounter] (
    [Id] INT NOT NULL
);


CREATE TABLE [dbo].[Withdrawal] (
    [Reference]       UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Username]        VARCHAR (10)     NOT NULL,
    [RequestedDate]   DATETIME         DEFAULT (getdate()) NOT NULL,
    [RequestedAmount] DECIMAL (9, 8)   NOT NULL,
    [PaymentDate]     DATETIME         NULL,
    [PaymentAmount]   DECIMAL (9, 8)   NULL,
    [Paid]            BIT              DEFAULT (NULL) NULL,
    [WalletType]      VARCHAR (50)     NOT NULL,
    [WalletAddress]   VARCHAR (MAX)    NOT NULL,
    [Remarks]         VARCHAR (MAX)    NULL
);

CREATE TABLE [dbo].[BannerNetwork] (
    [Id]       INT           DEFAULT ((1)) NOT NULL,
    [HTMLCode] VARCHAR (MAX) NOT NULL
);

CREATE TABLE [dbo].[SquareBannerNetwork] (
    [Id]       INT           DEFAULT ((1)) NOT NULL,
    [HTMLCode] VARCHAR (MAX) NOT NULL
);

CREATE TABLE [dbo].[SquareBannerNetworkCounter] (
    [Id] INT NOT NULL
);

CREATE TABLE [dbo].[SquareBannerRotator] (
    [Id]         INT           DEFAULT ((1)) NOT NULL,
    [ImageLink]  VARCHAR (MAX) NOT NULL,
    [TargetLink] VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[SquareBannerRotatorCounter] (
    [Id]         INT           DEFAULT ((0)) NOT NULL,
    [TargetLink] VARCHAR (MAX) NULL,
    [ImageLink]  VARCHAR (MAX) NULL
);

CREATE TABLE [dbo].[PTPSquareBannerNetwork] (
    [Id]       INT           DEFAULT ((1)) NOT NULL,
    [HTMLCode] VARCHAR (MAX) NOT NULL
);

CREATE TABLE [dbo].[PTPSquareBannerNetworkCounter] (
    [Id] INT NOT NULL
);

CREATE TABLE [dbo].[Advertise] (
    [Reference] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Name]      VARCHAR (70)     NOT NULL,
    [Value]     VARCHAR (70)     NOT NULL,
    [Price]     DECIMAL (9, 8)   DEFAULT ((0.00000000)) NOT NULL
);

CREATE TABLE [dbo].[OrderHistory] (
    [Reference]     UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Username]      VARCHAR (12)     NOT NULL,
    [Name]          VARCHAR (70)     NOT NULL,
    [Value]         VARCHAR (70)     NOT NULL,
    [Price]         DECIMAL (9, 8)   DEFAULT ((0.00000000)) NOT NULL,
    [Status]        VARCHAR (70)     DEFAULT ('Pending') NOT NULL,
    [ImageLink]     VARCHAR (MAX)    NULL,
    [TargetLink]    VARCHAR (MAX)    NULL,
    [Remarks]       VARCHAR (MAX)    NULL,
    [OrderDateTime] DATETIME         DEFAULT (getdate()) NOT NULL
);


USE AdminPanel;

CREATE TABLE [dbo].[AdminPanel] (
    [SessionId]     UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [SessionExpiry] DATETIME         DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([SessionId] ASC)
);

--Following is the script for stored procedures

USE Faucet4u;  
GO  
CREATE PROCEDURE UpdateBalance   
    @UsernameInput nvarchar(10),   
    @AmountInput DECIMAL(9, 8)   
AS   

BEGIN
	DECLARE @Username varchar(10) = @UsernameInput
	DECLARE @Amount DECIMAL(9, 8) = @AmountInput
	DECLARE @Referrer1 varchar(10)
	DECLARE @Referrer2 varchar(10)
	DECLARE @Referrer3 varchar(10)
	DECLARE @Amount1 DECIMAL(9, 8)
	DECLARE @Amount2 DECIMAL(9, 8)
	DECLARE @Amount3 DECIMAL(9, 8)


	SELECT @Referrer1 = Username FROM Users WHERE Referrer = @Username
	SELECT @Referrer2 = Username FROM Users WHERE Referrer = @Referrer1
	SELECT @Referrer3 = Username FROM Users WHERE Referrer = @Referrer2

	DECLARE @Level1 int
	DECLARE @Level2 int
	DECLARE @Level3 int

	SELECT @Level1 = Value FROM Settings WHERE Name = 'Level1'
	SELECT @Level2 = Value FROM Settings WHERE Name = 'Level2'
	SELECT @Level3 = Value FROM Settings WHERE Name = 'Level3'

	SET @Amount1 = convert(DECIMAL(9, 8), @Amount*@Level1/100)
	SET @Amount2 = convert(DECIMAL(9, 8), @Amount*@Level2/100)
	SET @Amount3 = convert(DECIMAL(9, 8), @Amount*@Level3/100)

	UPDATE Users SET Balance = Balance + @Amount WHERE Username = @Username
	UPDATE Users SET Balance = Balance + @Amount1 WHERE Username = @Referrer1
	UPDATE Users SET Balance = Balance + @Amount2 WHERE Username = @Referrer2
	UPDATE Users SET Balance = Balance + @Amount3 WHERE Username = @Referrer3
END
GO  

USE Faucet4u;  
GO  
CREATE OR ALTER PROCEDURE UpdateOfferwallBalance   
    @UsernameInput nvarchar(10),   
    @AmountInput DECIMAL(9, 8)   
AS   

BEGIN
	DECLARE @Username varchar(10) = @UsernameInput
	DECLARE @Amount DECIMAL(9, 8) = @AmountInput
	DECLARE @Referrer1 varchar(10)
	DECLARE @Referrer2 varchar(10)
	DECLARE @Referrer3 varchar(10)
	DECLARE @Amount1 DECIMAL(9, 8)
	DECLARE @Amount2 DECIMAL(9, 8)
	DECLARE @Amount3 DECIMAL(9, 8)


	SELECT @Referrer1 = Username FROM Users WHERE Referrer = @Username
	SELECT @Referrer2 = Username FROM Users WHERE Referrer = @Referrer1
	SELECT @Referrer3 = Username FROM Users WHERE Referrer = @Referrer2

	DECLARE @Level1 int
	DECLARE @Level2 int
	DECLARE @Level3 int

	SELECT @Level1 = Value FROM Settings WHERE Name = 'OfferwallLevel1'
	SELECT @Level2 = Value FROM Settings WHERE Name = 'OfferwallLevel2'
	SELECT @Level3 = Value FROM Settings WHERE Name = 'OfferwallLevel3'

	SET @Amount1 = convert(DECIMAL(9, 8), @Amount*@Level1/100)
	SET @Amount2 = convert(DECIMAL(9, 8), @Amount*@Level2/100)
	SET @Amount3 = convert(DECIMAL(9, 8), @Amount*@Level3/100)

	UPDATE Users SET OfferwallBalance = OfferwallBalance + @Amount WHERE Username = @Username
	UPDATE Users SET OfferwallBalance = OfferwallBalance + @Amount1 WHERE Username = @Referrer1
	UPDATE Users SET OfferwallBalance = OfferwallBalance + @Amount2 WHERE Username = @Referrer2
	UPDATE Users SET OfferwallBalance = OfferwallBalance + @Amount3 WHERE Username = @Referrer3
END
GO 

USE Faucet4u;  
GO  
CREATE OR ALTER PROCEDURE InsertCheatHistory
@UsernameInput varchar(10),
@Case int = 1
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
IF (@Case = 3)
BEGIN
SET @Reason = 'Using multiple accounts to login from same computer'
SELECT @CheatCounter = Value FROM Settings WHERE Name = 'CheatCounterForMultipleAccounts'
END
IF (@Case = 4)
BEGIN
SET @Reason = 'Detected using a proxy'
SELECT @CheatCounter = Value FROM Settings WHERE Name = 'CheatCounterForProxy'
END
INSERT INTO CheatHistory(Username, CheatLevel, Reason) VALUES (@UsernameInput, @CheatCounter, @Reason)
UPDATE Users SET CheatCounter = CheatCounter + @CheatCounter WHERE Username = @UsernameInput

END
GO

USE Faucet4u;  
GO  
CREATE OR ALTER PROCEDURE SelectReferralData
@OriginalUsername varchar(10)
AS

BEGIN

SELECT Username, LinkShortnerEarning, PTPUniqueEarning, PTPNonUnique1Earning, PTPNonUnique2Earning, PTPNonUnique3Earning, OfferwallEarning, LastClaim FROM Users WHERE Referrer = @OriginalUsername

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
	OfferwallEarning decimal(9, 8) NOT NULL,
	LastClaim datetime
);

DECLARE @MaxRows int
SELECT @MaxRows = COUNT(Username) FROM @Level1
DECLARE @RowCounter int = 1
DECLARE @ReferralUsername varchar(10)

WHILE(@RowCounter <= @MaxRows)
BEGIN
SELECT @ReferralUsername = Username FROM (SELECT Username, ROW_NUMBER() OVER (ORDER BY Username ASC) rn FROM @Level1) AS Username WHERE rn = @RowCounter


INSERT INTO @Level2 (Username, LinkShortnerEarning, PTPUniqueEarning, PTPNonUnique1Earning, PTPNonUnique2Earning, PTPNonUnique3Earning, OfferwallEarning, LastClaim)
SELECT Username, LinkShortnerEarning, PTPUniqueEarning, PTPNonUnique1Earning, PTPNonUnique2Earning, PTPNonUnique3Earning, OfferwallEarning, LastClaim FROM Users WHERE Referrer = @ReferralUsername
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
	OfferwallEarning decimal(9, 8) NOT NULL,
	LastClaim datetime
);

SELECT @MaxRows = COUNT(Username) FROM @Level3Referrers
SET @RowCounter = 1

WHILE(@RowCounter <= @MaxRows)
BEGIN

SELECT @ReferralUsername = Username FROM (SELECT Username, ROW_NUMBER() OVER (ORDER BY Username ASC) rn FROM @Level3Referrers) AS Username WHERE rn = @RowCounter

INSERT INTO @Level3 (Username, LinkShortnerEarning, PTPUniqueEarning, PTPNonUnique1Earning, PTPNonUnique2Earning, PTPNonUnique3Earning, OfferwallEarning, LastClaim)
SELECT Username, LinkShortnerEarning, PTPUniqueEarning, PTPNonUnique1Earning, PTPNonUnique2Earning, PTPNonUnique3Earning, OfferwallEarning, LastClaim FROM Users WHERE Username = @ReferralUsername
SET @RowCounter = @RowCounter + 1
END

SELECT * FROM @Level2
SELECT * FROM @Level3
END
GO

USE Faucet4u;  
GO  
CREATE OR ALTER PROCEDURE SelectAllReferralData
@OriginalUsername varchar(10)
AS

BEGIN

SELECT * FROM Users WHERE Referrer = @OriginalUsername

DECLARE @Level1 TABLE(
    Username varchar(10) NOT NULL
);

INSERT INTO @Level1 (Username)
SELECT Username FROM Users WHERE Referrer = @OriginalUsername

DECLARE @Level2 TABLE(
    Username varchar(10) NOT NULL
);

DECLARE @MaxRows int
SELECT @MaxRows = COUNT(Username) FROM @Level1
DECLARE @RowCounter int = 1
DECLARE @ReferralUsername varchar(10)

WHILE(@RowCounter <= @MaxRows)
BEGIN
SELECT @ReferralUsername = Username FROM (SELECT Username, ROW_NUMBER() OVER (ORDER BY Username ASC) rn FROM @Level1) AS Username WHERE rn = @RowCounter


INSERT INTO @Level2 (Username)
SELECT Username FROM Users WHERE Referrer = @ReferralUsername
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
    Username varchar(10) NOT NULL
);

SELECT @MaxRows = COUNT(Username) FROM @Level3Referrers
SET @RowCounter = 1

WHILE(@RowCounter <= @MaxRows)
BEGIN

SELECT @ReferralUsername = Username FROM (SELECT Username, ROW_NUMBER() OVER (ORDER BY Username ASC) rn FROM @Level3Referrers) AS Username WHERE rn = @RowCounter

INSERT INTO @Level3 (Username)
SELECT Username FROM Users WHERE Username = @ReferralUsername
SET @RowCounter = @RowCounter + 1
END

SELECT * FROM Users INNER JOIN @Level2 ON Users.Username = [@Level2].Username;
SELECT * FROM Users INNER JOIN @Level3 ON Users.Username = [@Level3].Username;
END
GO
