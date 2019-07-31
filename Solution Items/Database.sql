Use Faucet4u;
DECLARE @MaxId int

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Users')
BEGIN
CREATE TABLE [Users] (
    [Id]                         UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,   
    [Username] VARCHAR(10) NOT NULL,
	[Email] VARCHAR(350) NOT NULL, 
    CONSTRAINT [PK_Users] PRIMARY KEY NONCLUSTERED ([Id] ASC),
    CONSTRAINT [Unique_Username] UNIQUE CLUSTERED ([Username] ASC),
    CONSTRAINT [Unique_Email] UNIQUE NONCLUSTERED ([Email] ASC)
);
END

IF COL_LENGTH('Users','Password') IS NULL
BEGIN
ALTER TABLE Users ADD [Password] VARCHAR (60) NOT NULL
END
IF COL_LENGTH('Users','IsConfirmed') IS NULL
BEGIN
ALTER TABLE Users ADD [IsConfirmed] BIT CONSTRAINT [DF_Users_isConfirmed] DEFAULT ((0)) NOT NULL
END
IF COL_LENGTH('Users','ConfirmationCode') IS NULL
BEGIN
ALTER TABLE Users ADD [ConfirmationCode] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL
END
IF COL_LENGTH('Users','ConfirmationCodeExpiryTime') IS NULL
BEGIN
ALTER TABLE Users ADD [ConfirmationCodeExpiryTime] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('Users','IP') IS NULL
BEGIN
ALTER TABLE Users ADD [IP] VARCHAR (20) DEFAULT ('0.0.0.0') NOT NULL
END
IF COL_LENGTH('Users','Country') IS NULL
BEGIN
ALTER TABLE Users ADD [Country] VARCHAR (50) DEFAULT ('Other') NOT NULL
END
IF COL_LENGTH('Users','SessionId') IS NULL
BEGIN
ALTER TABLE Users ADD [SessionId] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL
END
IF COL_LENGTH('Users','SessionExpiry') IS NULL
BEGIN
ALTER TABLE Users ADD [SessionExpiry] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('Users','FaucetHubBitcoinAddress') IS NULL
BEGIN
ALTER TABLE Users ADD [FaucetHubBitcoinAddress] VARCHAR (40) NULL
END
IF COL_LENGTH('Users','Locked') IS NULL
BEGIN
ALTER TABLE Users ADD [Locked] BIT DEFAULT ((0)) NOT NULL
END
IF COL_LENGTH('Users','CheatCounter') IS NULL
BEGIN
ALTER TABLE Users ADD [CheatCounter] INT DEFAULT ((0)) NOT NULL
END
IF COL_LENGTH('Users','Referrer') IS NULL
BEGIN
ALTER TABLE Users ADD [Referrer] VARCHAR (10) NULL
END
IF COL_LENGTH('Users','Balance') IS NULL
BEGIN
ALTER TABLE Users ADD [Balance] DECIMAL(13, 12) DEFAULT ((0.00000000)) NOT NULL
END
IF COL_LENGTH('Users','OfferwallBalance') IS NULL
BEGIN
ALTER TABLE Users ADD [OfferwallBalance] DECIMAL (13, 12) DEFAULT ((0.00000000)) NOT NULL
END
IF COL_LENGTH('Users','PurchaseBalance') IS NULL
BEGIN
ALTER TABLE Users ADD [PurchaseBalance] DECIMAL (9, 8) DEFAULT ((0.00000000)) NOT NULL
END
IF COL_LENGTH('Users','LinkClaimMarker') IS NULL
BEGIN
ALTER TABLE Users ADD [LinkClaimMarker] DECIMAL (9, 8) DEFAULT ((0.00000000)) NOT NULL
END
IF COL_LENGTH('Users','LinkClaimMarkerLink') IS NULL
BEGIN
ALTER TABLE Users ADD [LinkClaimMarkerLink] VARCHAR (MAX) DEFAULT ('faucet4all.com') NOT NULL
END
IF COL_LENGTH('Users','LinkClaimInterval') IS NULL
BEGIN
ALTER TABLE Users ADD [LinkClaimInterval] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('Users','LinkClaimMarkerIP') IS NULL
BEGIN
ALTER TABLE Users ADD [LinkClaimMarkerIP] varchar(40) DEFAULT ('0.0.0.0') NOT NULL
END
IF COL_LENGTH('Users','LinkClaimMarkerId') IS NULL
BEGIN
ALTER TABLE Users ADD [LinkClaimMarkerId] varchar(60) DEFAULT ('Unknown') NOT NULL
END
IF COL_LENGTH('Users','LinkClaimFalseDateTime') IS NULL
BEGIN
ALTER TABLE Users ADD [LinkClaimFalseDateTime] datetime DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('Users','TotalWithdrawn') IS NULL
BEGIN
ALTER TABLE Users ADD [TotalWithdrawn] DECIMAL(9, 8) DEFAULT ((0.00000000)) NOT NULL
END
IF COL_LENGTH('Users','TotalOfferwallWithdrawn') IS NULL
BEGIN
ALTER TABLE Users ADD [TotalOfferwallWithdrawn] DECIMAL (9, 8) DEFAULT ((0.00000000)) NOT NULL
END
IF COL_LENGTH('Users','WithdrawalAmount') IS NULL
BEGIN
ALTER TABLE Users ADD [WithdrawalAmount] DECIMAL (9, 8) DEFAULT ((0.00000000)) NOT NULL
END
IF COL_LENGTH('Users','OfferwallWithdrawalAmount') IS NULL
BEGIN
ALTER TABLE Users ADD [OfferwallWithdrawalAmount]  DECIMAL (9, 8) DEFAULT ((0.00000000)) NOT NULL
END
IF COL_LENGTH('Users','LinkShortnerEarning') IS NULL
BEGIN
ALTER TABLE Users ADD [LinkShortnerEarning] DECIMAL (9, 8) DEFAULT ((0.00000000)) NOT NULL
END
IF COL_LENGTH('Users','BonusAdEarning') IS NULL
BEGIN
ALTER TABLE Users ADD [BonusAdEarning] DECIMAL (9, 8) DEFAULT ((0.00000000)) NOT NULL
END
IF COL_LENGTH('Users','OfferwallEarning') IS NULL
BEGIN
ALTER TABLE Users ADD [OfferwallEarning] DECIMAL (9, 8) DEFAULT ((0.00000000)) NOT NULL
END
IF COL_LENGTH('Users','PTPUniqueEarning') IS NULL
BEGIN
ALTER TABLE Users ADD [PTPUniqueEarning] DECIMAL (9, 8) DEFAULT ((0.00000000)) NOT NULL
END
IF COL_LENGTH('Users','PTPNonUnique1Earning') IS NULL
BEGIN
ALTER TABLE Users ADD [PTPNonUnique1Earning] DECIMAL (9, 8) DEFAULT ((0.00000000)) NOT NULL
END
IF COL_LENGTH('Users','PTPNonUnique2Earning') IS NULL
BEGIN
ALTER TABLE Users ADD [PTPNonUnique2Earning] DECIMAL (9, 8) DEFAULT ((0.00000000)) NOT NULL
END
IF COL_LENGTH('Users','PTPNonUnique3Earning') IS NULL
BEGIN
ALTER TABLE Users ADD [PTPNonUnique3Earning] DECIMAL (9, 8) DEFAULT ((0.00000000)) NOT NULL
END
IF COL_LENGTH('Users','LastLogin') IS NULL
BEGIN
ALTER TABLE Users ADD [LastLogin] DATETIME NULL
END
IF COL_LENGTH('Users','LastClaim') IS NULL
BEGIN
ALTER TABLE Users ADD [LastClaim] DATETIME NULL
END
IF COL_LENGTH('Users','DateRegistered') IS NULL
BEGIN
ALTER TABLE Users ADD [DateRegistered] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('Users','JustClaimed') IS NULL
BEGIN
ALTER TABLE Users ADD [JustClaimed] BIT DEFAULT ((0)) NOT NULL
END
IF COL_LENGTH('Users','ChatBanned') IS NULL
BEGIN
ALTER TABLE Users ADD [ChatBanned] BIT DEFAULT ((0)) NOT NULL
END
IF COL_LENGTH('Users','PTPCredit') IS NULL
BEGIN
ALTER TABLE Users ADD [PTPCredit] BIGINT DEFAULT (0) NOT NULL
END
IF COL_LENGTH('Users','PTPDayCredit') IS NULL
BEGIN
ALTER TABLE Users ADD [PTPDayCredit] BIGINT DEFAULT (0) NOT NULL
END
IF COL_LENGTH('Users','BannerCredit') IS NULL
BEGIN
ALTER TABLE Users ADD [BannerCredit] BIGINT DEFAULT (0) NOT NULL
END
IF COL_LENGTH('Users','BannerDayCredit') IS NULL
BEGIN
ALTER TABLE Users ADD [BannerDayCredit] BIGINT DEFAULT (0) NOT NULL
END
IF COL_LENGTH('Users','BonusAdCredit') IS NULL
BEGIN
ALTER TABLE Users ADD [BonusAdCredit] BIGINT DEFAULT (0) NOT NULL
END
IF COL_LENGTH('Users','BonusAdDayCredit') IS NULL
BEGIN
ALTER TABLE Users ADD [BonusAdDayCredit] BIGINT DEFAULT (0) NOT NULL
END
IF COL_LENGTH('Users','BonusAdConfirmationCode') IS NULL
BEGIN
ALTER TABLE Users ADD [BonusAdConfirmationCode] uniqueidentifier DEFAULT (newid()) NOT NULL
END
IF COL_LENGTH('Users','BonusAdMarkerIP') IS NULL
BEGIN
ALTER TABLE Users ADD [BonusAdMarkerIP] varchar(40) DEFAULT ('0.0.0.0') NOT NULL
END
IF COL_LENGTH('Users','BonusAdMarkerId') IS NULL
BEGIN
ALTER TABLE Users ADD [BonusAdMarkerId] uniqueidentifier DEFAULT (newid()) NOT NULL
END
IF COL_LENGTH('Users','BonusAdLinkMarkerId') IS NULL
BEGIN
ALTER TABLE Users ADD [BonusAdLinkMarkerId] uniqueidentifier DEFAULT (newid()) NOT NULL
END
IF COL_LENGTH('Users','BonusAdLinkFalseDateTime') IS NULL
BEGIN
ALTER TABLE Users ADD [BonusAdLinkFalseDateTime] datetime DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('Users','BonusAdMarkerAmount') IS NULL
BEGIN
ALTER TABLE Users ADD [BonusAdMarkerAmount] DECIMAL (9, 8) DEFAULT ((0.00000000)) NOT NULL
END
IF COL_LENGTH('Users','SocketSession') IS NULL
BEGIN
ALTER TABLE Users ADD [SocketSession] varchar(100) DEFAULT ('Unknown') NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'SupportTicketsReply')
BEGIN
CREATE TABLE [SupportTicketsReply] (
    [ReplyId]   UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_SupportTicketsReply] PRIMARY KEY CLUSTERED ([ReplyId] ASC)
);
END


IF COL_LENGTH('SupportTicketsReply','Reference') IS NULL
BEGIN
ALTER TABLE SupportTicketsReply ADD [Reference] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL
END
IF COL_LENGTH('SupportTicketsReply','DateTime') IS NULL
BEGIN
ALTER TABLE SupportTicketsReply ADD [DateTime] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('SupportTicketsReply','Reply') IS NULL
BEGIN
ALTER TABLE SupportTicketsReply ADD [Reply] VARCHAR(MAX) DEFAULT 'User sent an empty reply' NOT NULL
END
IF COL_LENGTH('SupportTicketsReply','Username') IS NULL
BEGIN
ALTER TABLE SupportTicketsReply ADD [Username] VARCHAR (10) NULL
END
IF COL_LENGTH('SupportTicketsReply','Read') IS NULL
BEGIN
ALTER TABLE SupportTicketsReply ADD [Read] BIT DEFAULT ((0)) NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'SupportTickets')
BEGIN
CREATE TABLE [SupportTickets] (
    [Reference] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL
);
END

IF COL_LENGTH('SupportTickets','Subject') IS NULL
BEGIN
ALTER TABLE SupportTickets ADD [Subject] VARCHAR (70) NOT NULL
END
IF COL_LENGTH('SupportTickets','Message') IS NULL
BEGIN
ALTER TABLE SupportTickets ADD [Message] VARCHAR (MAX) NOT NULL
END
IF COL_LENGTH('SupportTickets','Locked') IS NULL
BEGIN
ALTER TABLE SupportTickets ADD [Locked] BIT DEFAULT ((0)) NOT NULL
END
IF COL_LENGTH('SupportTickets','Username') IS NULL
BEGIN
ALTER TABLE SupportTickets ADD [Username] VARCHAR (10) NULL
END
IF COL_LENGTH('SupportTickets','Email') IS NULL
BEGIN
ALTER TABLE SupportTickets ADD [Email] VARCHAR (350) NOT NULL
END
IF COL_LENGTH('SupportTickets','Read') IS NULL
BEGIN
ALTER TABLE SupportTickets ADD [Read] BIT DEFAULT ((0)) NOT NULL
END
IF COL_LENGTH('SupportTickets','DateTime') IS NULL
BEGIN
ALTER TABLE SupportTickets ADD [DateTime]  DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('SupportTickets','LastReplier') IS NULL
BEGIN
ALTER TABLE SupportTickets ADD [LastReplier]  VARCHAR(15) DEFAULT('admin') NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'DepositHistory')
BEGIN
CREATE TABLE [DepositHistory] (
    [Reference]     UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL
);
END


IF COL_LENGTH('DepositHistory','Username') IS NULL
BEGIN
ALTER TABLE DepositHistory ADD [Username] VARCHAR(10) DEFAULT 'Unknown' NOT NULL
END
IF COL_LENGTH('DepositHistory','DepositDate') IS NULL
BEGIN
ALTER TABLE DepositHistory ADD [DepositDate] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('DepositHistory','Amount') IS NULL
BEGIN
ALTER TABLE DepositHistory ADD [Amount] DECIMAL (9, 8) NOT NULL
END
IF COL_LENGTH('DepositHistory','WalletType') IS NULL
BEGIN
ALTER TABLE DepositHistory ADD [WalletType] VARCHAR (150) NOT NULL
END
IF COL_LENGTH('DepositHistory','WalletAddress') IS NULL
BEGIN
ALTER TABLE DepositHistory ADD [WalletAddress] VARCHAR (MAX) NOT NULL
END
IF COL_LENGTH('DepositHistory','Remarks') IS NULL
BEGIN
ALTER TABLE DepositHistory ADD [Remarks] VARCHAR (MAX) NULL
END
IF COL_LENGTH('DepositHistory','Confirmations') IS NULL
BEGIN
ALTER TABLE DepositHistory ADD [Confirmations] int DEFAULT(0) NOT NULL
END
IF COL_LENGTH('DepositHistory','TransactionId') IS NULL
BEGIN
ALTER TABLE DepositHistory ADD [TransactionId] VARCHAR(MAX) DEFAULT('Unknown') NOT NULL
END
IF COL_LENGTH('DepositHistory','InvoiceId') IS NULL
BEGIN
ALTER TABLE DepositHistory ADD [InvoiceId] VARCHAR(MAX) DEFAULT('Unknown') NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'OfferwallHistory')
BEGIN
CREATE TABLE [OfferwallHistory] (
    [Reference]    UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL
);
END


IF COL_LENGTH('OfferwallHistory','Username') IS NULL
BEGIN
ALTER TABLE OfferwallHistory ADD [Username] VARCHAR(12) DEFAULT 'Unknown' NOT NULL
END
IF COL_LENGTH('OfferwallHistory','Offerwall') IS NULL
BEGIN
ALTER TABLE OfferwallHistory ADD [Offerwall] VARCHAR (MAX) NOT NULL
END
IF COL_LENGTH('OfferwallHistory','Amount') IS NULL
BEGIN
ALTER TABLE OfferwallHistory ADD [Amount] DECIMAL (9, 8) DEFAULT ((0.00000000)) NOT NULL
END
IF COL_LENGTH('OfferwallHistory','Status') IS NULL
BEGIN
ALTER TABLE OfferwallHistory ADD [Status] VARCHAR (70) DEFAULT ('Pending') NOT NULL
END
IF COL_LENGTH('OfferwallHistory','DateTime') IS NULL
BEGIN
ALTER TABLE OfferwallHistory ADD [DateTime] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('OfferwallHistory','CampaignId') IS NULL
BEGIN
ALTER TABLE OfferwallHistory ADD [CampaignId] INT DEFAULT ((0)) NOT NULL
END
IF COL_LENGTH('OfferwallHistory','CampaignName') IS NULL
BEGIN
ALTER TABLE OfferwallHistory ADD [CampaignName] VARCHAR (50) DEFAULT ('Not found') NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'DepositMethod')
BEGIN
CREATE TABLE [DepositMethod] (
    [Name]    VARCHAR (150) NOT NULL,    
    PRIMARY KEY CLUSTERED ([Name] ASC)
);
END

IF COL_LENGTH('DepositMethod','Address') IS NULL
BEGIN
ALTER TABLE DepositMethod ADD [Address] VARCHAR(MAX) DEFAULT 'Address not specified' NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Settings')
BEGIN
CREATE TABLE [Settings] (
    [Name]  VARCHAR(1000) DEFAULT 'Unknown Setting' NOT NULL,    
    PRIMARY KEY CLUSTERED ([Name] ASC)
);
END

IF COL_LENGTH('Settings','Value') IS NULL
BEGIN
ALTER TABLE Settings ADD [Value] VARCHAR(MAX) DEFAULT 'Unknown Value'  NOT NULL
END

IF NOT EXISTS(Select 1 From Settings Where Name = 'CheatCounterForDirectClaims')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'CheatCounterForDirectClaims',N'1');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'CheatCounterForDuplicateAccount')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'CheatCounterForDuplicateAccount',N'1');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'CheatCounterForMultipleAccounts')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'CheatCounterForMultipleAccounts',N'5');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'CheatCounterForProxy')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'CheatCounterForProxy',N'5');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'DirectNonUniqueIP1Credit')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'DirectNonUniqueIP1Credit',N'0.00010000');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'DirectNonUniqueIP2Credit')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'DirectNonUniqueIP2Credit',N'0.00010000');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'DirectNonUniqueIP3Credit')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'DirectNonUniqueIP3Credit',N'0.00001000');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'DirectUniqueIPCredit')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'DirectUniqueIPCredit',N'0.00001000');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'Level1')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'Level1',N'10');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'Level2')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'Level2',N'5');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'Level3')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'Level3',N'2');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'PTPLevel1')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'PTPLevel1',N'10');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'PTPLevel2')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'PTPLevel2',N'5');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'PTPLevel3')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'PTPLevel3',N'2');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'OfferwallLevel1')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'OfferwallLevel1',N'10');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'OfferwallLevel2')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'OfferwallLevel2',N'5');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'OfferwallLevel3')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'OfferwallLevel3',N'2');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'LinkClaimMinutes')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'LinkClaimMinutes',N'3');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'LinkClaimCredit')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'LinkClaimCredit',N'0.00000100');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'MinimumWithdrawal')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'MinimumWithdrawal',N'0.00100000');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'MinimumExchangeWithdrawal')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'MinimumExchangeWithdrawal',N'0.00150000');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'MinimumDeposit')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'MinimumDeposit',N'0.00010000');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'MinimumOfferwallWithdrawal')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'MinimumOfferwallWithdrawal',N'0.00100000');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'PTPDirectRatio')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'PTPDirectRatio',N'2');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'PTPNonUniqueIP1Credit')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'PTPNonUniqueIP1Credit',N'0.00010000');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'PTPNonUniqueIP2Credit')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'PTPNonUniqueIP2Credit',N'0.00010000');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'PTPNonUniqueIP3Credit')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'PTPNonUniqueIP3Credit',N'0.00001000');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'PTPTotalRatio')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'PTPTotalRatio',N'5');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'PTPUniqueIPCredit')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'PTPUniqueIPCredit',N'0.00001000');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'ForumLink')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'ForumLink',N'http://www.emoneyspace.com');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'HitsFor1PTPDayCredit')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'HitsFor1PTPDayCredit',N'10000');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'HitsFor1BonusAdDayCredit')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'HitsFor1BonusAdDayCredit',N'10000');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'ImpressionsFor1BannerDayCredit')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'ImpressionsFor1BannerDayCredit',N'10000');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'ImpressionsFor1SquareBannerDayCredit')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'ImpressionsFor1SquareBannerDayCredit',N'10000');
END
IF NOT EXISTS(Select 1 From Settings Where Name = 'BonusAdCredit')
BEGIN
INSERT INTO [Settings] ([Name],[Value]) VALUES (
N'BonusAdCredit',N'0.00000100');
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PTPAbsoluteSource')
BEGIN
CREATE TABLE [PTPAbsoluteSource] (
    [site]    VARCHAR(1000) DEFAULT 'Not Specified' NOT NULL  
);
END

IF COL_LENGTH('PTPAbsoluteSource','Username') IS NULL
BEGIN
ALTER TABLE PTPAbsoluteSource ADD [Username] varchar(12) DEFAULT ('Unknown') NOT NULL
END
IF COL_LENGTH('PTPAbsoluteSource','counter') IS NULL
BEGIN
ALTER TABLE PTPAbsoluteSource ADD [counter] BIGINT DEFAULT ((0)) NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PTPSource')
BEGIN
CREATE TABLE [PTPSource] (
    [site]    VARCHAR(1000) DEFAULT 'Not Specified' NOT NULL  
);
END

IF COL_LENGTH('PTPSource','Username') IS NULL
BEGIN
ALTER TABLE PTPSource ADD [Username] varchar(12) DEFAULT ('Unknown') NOT NULL
END
IF COL_LENGTH('PTPSource','counter') IS NULL
BEGIN
ALTER TABLE PTPSource ADD [counter] BIGINT DEFAULT ((0)) NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PTPIP')
BEGIN
CREATE TABLE [PTPIP] (
    [Username]     VARCHAR(50) DEFAULT 'Unknown' NOT NULL
);
END

IF COL_LENGTH('PTPIP','UniqueIP') IS NULL
BEGIN
ALTER TABLE PTPIP ADD [UniqueIP] VARCHAR(50) DEFAULT '0.0.0.0' NOT NULL
END
IF COL_LENGTH('PTPIP','NonUniqueIP1') IS NULL
BEGIN
ALTER TABLE PTPIP ADD [NonUniqueIP1] VARCHAR (50) DEFAULT (NULL) NULL
END
IF COL_LENGTH('PTPIP','NonUniqueIP2') IS NULL
BEGIN
ALTER TABLE PTPIP ADD [NonUniqueIP2] VARCHAR (50) DEFAULT (NULL) NULL
END
IF COL_LENGTH('PTPIP','NonUniqueIP3') IS NULL
BEGIN
ALTER TABLE PTPIP ADD [NonUniqueIP3] VARCHAR (50) DEFAULT (NULL) NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PTPCounter')
BEGIN
CREATE TABLE [PTPCounter] (
    [Id] INT DEFAULT ((1)) NOT NULL
);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PTPBlacklist')
BEGIN
CREATE TABLE [PTPBlacklist] (
    [Keyword] VARCHAR (MAX) DEFAULT 'rotator' NOT NULL
);
END

IF NOT EXISTS(Select 1 From PTPBlacklist Where Keyword = 'rotator')
BEGIN
INSERT INTO [PTPBlacklist] ([Keyword]) VALUES ('rotator');
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PTP')
BEGIN
CREATE TABLE [PTP] (
    [Id]       INT           DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
END

IF COL_LENGTH('PTP','Reference') IS NULL
BEGIN
ALTER TABLE PTP ADD [Reference] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL
END
IF COL_LENGTH('PTP','Creation') IS NULL
BEGIN
ALTER TABLE PTP ADD [Creation] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('PTP','Username') IS NULL
BEGIN
ALTER TABLE PTP ADD [Username] varchar(10) DEFAULT ('Admin') NOT NULL
END
IF COL_LENGTH('PTP','Link') IS NULL
BEGIN
ALTER TABLE PTP ADD [Link] VARCHAR(MAX) DEFAULT 'http://faucet4u.com/IRotator3/Rotator.php' NOT NULL
END
IF COL_LENGTH('PTP','HitsReceived') IS NULL
BEGIN
ALTER TABLE PTP ADD [HitsReceived] int DEFAULT 0 NOT NULL
END
IF COL_LENGTH('PTP','IsTimeBased') IS NULL
BEGIN
ALTER TABLE PTP ADD [IsTimeBased] BIT DEFAULT 0 NOT NULL
END
IF COL_LENGTH('PTP','Credit') IS NULL
BEGIN
ALTER TABLE PTP ADD [Credit] BIGINT DEFAULT 0 NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Mining')
BEGIN
CREATE TABLE [Mining] (
    [Percentage] INT DEFAULT ((30)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Percentage] ASC)
);
END

IF ((Select Count(Percentage) From Mining) < 1)
BEGIN
INSERT INTO [Mining] ([Percentage]) VALUES (30);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Logs')
BEGIN
CREATE TABLE [Logs] (
    [Guid]      UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED ([Guid] ASC)
);
END

IF COL_LENGTH('Logs','DateTime') IS NULL
BEGIN
ALTER TABLE Logs ADD [DateTime] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('Logs','IP') IS NULL
BEGIN
ALTER TABLE Logs ADD [IP] VARCHAR (20) NULL
END
IF COL_LENGTH('Logs','Username') IS NULL
BEGIN
ALTER TABLE Logs ADD [Username] VARCHAR(12) NULL
END
IF COL_LENGTH('Logs','Type') IS NULL
BEGIN
ALTER TABLE Logs ADD [Type] VARCHAR(10) DEFAULT 'Unknown' NOT NULL
END
IF COL_LENGTH('Logs','Message') IS NULL
BEGIN
ALTER TABLE Logs ADD [Message] VARCHAR(MAX) DEFAULT 'Message not specified' NOT NULL
END
IF COL_LENGTH('Logs','Exception') IS NULL
BEGIN
ALTER TABLE Logs ADD [Exception] VARCHAR(MAX) DEFAULT 'Exception not specified' NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'LoginHistory')
BEGIN
CREATE TABLE [LoginHistory] (
    [Id] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL 
);
END

IF COL_LENGTH('LoginHistory','DateTime') IS NULL
BEGIN
ALTER TABLE LoginHistory ADD [DateTime] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('LoginHistory','Username') IS NULL
BEGIN
ALTER TABLE LoginHistory ADD [Username] VARCHAR(12) NOT NULL
END
IF COL_LENGTH('LoginHistory','Success') IS NULL
BEGIN
ALTER TABLE LoginHistory ADD [Success]  BIT DEFAULT ((0)) NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'LinkShortnersRecord')
BEGIN
CREATE TABLE [LinkShortnersRecord] (
    [IP]                                   VARCHAR(20) DEFAULT '0.0.0.0' NOT NULL,    
    PRIMARY KEY CLUSTERED ([IP] ASC)
);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'LinkShortners')
BEGIN
CREATE TABLE [LinkShortners] (
    [Id]       UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,    
    [Link]     VARCHAR(max) DEFAULT ('http://faucet4all.com') NOT NULL,    
    PRIMARY KEY CLUSTERED ([Id] ASC),
);
END

IF COL_LENGTH('LinkShortners','Creation') IS NULL
BEGIN
ALTER TABLE LinkShortners ADD [Creation] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('LinkShortners','Limit') IS NULL
BEGIN
ALTER TABLE LinkShortners ADD [Limit] INT DEFAULT ((1)) NOT NULL
END
IF COL_LENGTH('LinkShortners','Remarks') IS NULL
BEGIN
ALTER TABLE LinkShortners ADD [Remarks] varchar(max) DEFAULT ('Testing, this is a new site')
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'DirectAbsoluteSource')
BEGIN
CREATE TABLE [DirectAbsoluteSource] (
    [site]    VARCHAR(1000) DEFAULT 'Unknown' NOT NULL    
);
END

IF COL_LENGTH('DirectAbsoluteSource','Username') IS NULL
BEGIN
ALTER TABLE DirectAbsoluteSource ADD [Username] varchar(12) DEFAULT ('Unknown') NOT NULL
END
IF COL_LENGTH('DirectAbsoluteSource','counter') IS NULL
BEGIN
ALTER TABLE DirectAbsoluteSource ADD [counter] BIGINT DEFAULT (0) NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'DirectSource')
BEGIN
CREATE TABLE [DirectSource] (
    [site]    VARCHAR(1000) DEFAULT 'Unknown' NOT NULL    
);
END

IF COL_LENGTH('DirectSource','Username') IS NULL
BEGIN
ALTER TABLE DirectSource ADD [Username] varchar(12) DEFAULT ('Unknown') NOT NULL
END
IF COL_LENGTH('DirectSource','counter') IS NULL
BEGIN
ALTER TABLE DirectSource ADD [counter] BIGINT DEFAULT (0) NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'DirectIP')
BEGIN
CREATE TABLE [DirectIP] (
    [Username] VARCHAR(50) DEFAULT 'Unknown' NOT NULL
);
END

IF COL_LENGTH('DirectIP','UniqueIP') IS NULL
BEGIN
ALTER TABLE DirectIP ADD [UniqueIP] VARCHAR(50) DEFAULT '0.0.0.0' NOT NULL
END
IF COL_LENGTH('DirectIP','NonUniqueIP1') IS NULL
BEGIN
ALTER TABLE DirectIP ADD [NonUniqueIP1] VARCHAR (50) NULL
END
IF COL_LENGTH('DirectIP','NonUniqueIP2') IS NULL
BEGIN
ALTER TABLE DirectIP ADD [NonUniqueIP2] VARCHAR (50) NULL
END
IF COL_LENGTH('DirectIP','NonUniqueIP3') IS NULL
BEGIN
ALTER TABLE DirectIP ADD [NonUniqueIP3] VARCHAR (50) NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'DirectCounter')
BEGIN
CREATE TABLE [DirectCounter] (
    [Id] INT DEFAULT 1 NOT NULL
);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Direct')
BEGIN
CREATE TABLE [Direct] (
    [Id] INT DEFAULT 1 NOT NULL
);
END

IF COL_LENGTH('Direct','Creation') IS NULL
BEGIN
ALTER TABLE Direct ADD [Creation] DATETIME NOT NULL
END
IF COL_LENGTH('Direct','Link') IS NULL
BEGIN
ALTER TABLE Direct ADD [Link] VARCHAR(MAX) DEFAULT 'Unknown' NOT NULL
END
IF COL_LENGTH('Direct','HitsReceived') IS NULL
BEGIN
ALTER TABLE Direct ADD [HitsReceived] int DEFAULT 0 NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'CheatHistory')
BEGIN
CREATE TABLE [CheatHistory] (
    [Reference]  UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Reference] ASC)
);
END

IF COL_LENGTH('CheatHistory','Username') IS NULL
BEGIN
ALTER TABLE CheatHistory ADD [Username] VARCHAR (10) NOT NULL
END
IF COL_LENGTH('CheatHistory','DateTime') IS NULL
BEGIN
ALTER TABLE CheatHistory ADD [DateTime] DATETIME DEFAULT(getdate()) NOT NULL
END
IF COL_LENGTH('CheatHistory','CheatLevel') IS NULL
BEGIN
ALTER TABLE CheatHistory ADD [CheatLevel] INT DEFAULT ((1)) NOT NULL
END
IF COL_LENGTH('CheatHistory','Reason') IS NULL
BEGIN
ALTER TABLE CheatHistory ADD [Reason] VARCHAR(MAX) DEFAULT 'Unknown' NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'BannerRotatorCounter')
BEGIN
CREATE TABLE [BannerRotatorCounter] (
    [Id] INT DEFAULT ((0)) NOT NULL
);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'BannerRotator')
BEGIN
CREATE TABLE [BannerRotator] (
    [Id] INT DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
END

IF COL_LENGTH('BannerRotator','Reference') IS NULL
BEGIN
ALTER TABLE BannerRotator ADD [Reference] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL
END
IF COL_LENGTH('BannerRotator','Creation') IS NULL
BEGIN
ALTER TABLE BannerRotator ADD [Creation] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('BannerRotator','Username') IS NULL
BEGIN
ALTER TABLE BannerRotator ADD [Username] varchar(10) DEFAULT ('Admin') NOT NULL
END
IF COL_LENGTH('BannerRotator','ImageLink') IS NULL
BEGIN
ALTER TABLE BannerRotator ADD [ImageLink]  VARCHAR (MAX) NOT NULL
END
IF COL_LENGTH('BannerRotator','TargetLink') IS NULL
BEGIN
ALTER TABLE BannerRotator ADD [TargetLink] VARCHAR (MAX) NOT NULL
END
IF COL_LENGTH('BannerRotator','Clicks') IS NULL
BEGIN
ALTER TABLE BannerRotator ADD [Clicks] int DEFAULT 0 NOT NULL
END
IF COL_LENGTH('BannerRotator','Impressions') IS NULL
BEGIN
ALTER TABLE BannerRotator ADD [Impressions] int DEFAULT 0 NOT NULL
END
IF COL_LENGTH('BannerRotator','IsTimeBased') IS NULL
BEGIN
ALTER TABLE BannerRotator ADD [IsTimeBased] BIT DEFAULT 0 NOT NULL
END
IF COL_LENGTH('BannerRotator','Credit') IS NULL
BEGIN
ALTER TABLE BannerRotator ADD [Credit] BIGINT DEFAULT 0 NOT NULL
END


IF NOT EXISTS(Select 1 From BannerRotator Where TargetLink = 'http://faucet4u.com/BannerRotator/Rotator.php')
BEGIN
SELECT @MaxId = max(Id)+1 FROM BannerRotator
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 0
END
INSERT INTO [BannerRotator] ([Id], [ImageLink],[TargetLink]) VALUES (
@MaxId, N'http://faucet4u.com/BannerRotator/bannerImage/bannerImage.gif',N'http://faucet4u.com/BannerRotator/Rotator.php');
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'BannerNetworkCounter')
BEGIN
CREATE TABLE [BannerNetworkCounter] (
    [Id] INT DEFAULT 1 NOT NULL
);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Withdrawal')
BEGIN
CREATE TABLE [Withdrawal] (
    [Reference] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL
);
END

IF COL_LENGTH('Withdrawal','Username') IS NULL
BEGIN
ALTER TABLE Withdrawal ADD [Username] VARCHAR(10) DEFAULT 'Unknown' NOT NULL
END
IF COL_LENGTH('Withdrawal','WithdrawalType') IS NULL
BEGIN
ALTER TABLE Withdrawal ADD [WithdrawalType] VARCHAR(max) DEFAULT 'Standard' NOT NULL
END
IF COL_LENGTH('Withdrawal','RequestedDate') IS NULL
BEGIN
ALTER TABLE Withdrawal ADD [RequestedDate] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('Withdrawal','RequestedAmount') IS NULL
BEGIN
ALTER TABLE Withdrawal ADD [RequestedAmount] DECIMAL(9, 8) DEFAULT 0.00000000  NOT NULL
END
IF COL_LENGTH('Withdrawal','PaymentDate') IS NULL
BEGIN
ALTER TABLE Withdrawal ADD [PaymentDate] DATETIME NULL
END
IF COL_LENGTH('Withdrawal','PaymentAmount') IS NULL
BEGIN
ALTER TABLE Withdrawal ADD [PaymentAmount] DECIMAL(9, 8) NULL
END
IF COL_LENGTH('Withdrawal','Paid') IS NULL
BEGIN
ALTER TABLE Withdrawal ADD [Paid] BIT DEFAULT (NULL) NULL
END
IF COL_LENGTH('Withdrawal','WalletType') IS NULL
BEGIN
ALTER TABLE Withdrawal ADD [WalletType] VARCHAR(50) DEFAULT 'Not specified' NOT NULL
END
IF COL_LENGTH('Withdrawal','WalletAddress') IS NULL
BEGIN
ALTER TABLE Withdrawal ADD [WalletAddress] VARCHAR(MAX) DEFAULT 'Not specified' NOT NULL
END
IF COL_LENGTH('Withdrawal','Remarks') IS NULL
BEGIN
ALTER TABLE Withdrawal ADD  [Remarks] VARCHAR (MAX) NULL
END
IF COL_LENGTH('Withdrawal','Signature1') IS NULL
BEGIN
ALTER TABLE Withdrawal ADD  [Signature1] BIT DEFAULT (NULL)
END
IF COL_LENGTH('Withdrawal','Signature2') IS NULL
BEGIN
ALTER TABLE Withdrawal ADD  [Signature2] BIT DEFAULT (NULL)
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'ChatHistory')
BEGIN
CREATE TABLE [ChatHistory] (
    [Reference] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL
);
END

IF COL_LENGTH('ChatHistory','DateTime') IS NULL
BEGIN
ALTER TABLE ChatHistory ADD [DateTime] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('ChatHistory','Username') IS NULL
BEGIN
ALTER TABLE ChatHistory ADD [Username] VARCHAR(10) DEFAULT 'Unknown' NOT NULL
END
IF COL_LENGTH('ChatHistory','Message') IS NULL
BEGIN
ALTER TABLE ChatHistory ADD [Message] varchar(max) DEFAULT ('Empty message') NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'SkyscraperBannerNetwork')
BEGIN
CREATE TABLE [SkyscraperBannerNetwork] (
    [Id] INT DEFAULT ((1)) NOT NULL    
);
END


IF COL_LENGTH('SkyscraperBannerNetwork','HTMLCode') IS NULL
BEGIN
ALTER TABLE SkyscraperBannerNetwork ADD  [HTMLCode] VARCHAR(MAX) DEFAULT '<p>Advertise here for as low as 1$</p>' NOT NULL
END

IF NOT EXISTS(Select 1 From SkyscraperBannerNetwork Where HTMLCode = '<iframe src="http://faucet4all.com/Ads/AdhitzSkyscraper.html" scrolling="no" style="width:128px; height:608px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>')
BEGIN
SELECT @MaxId = max(Id)+1 FROM SkyscraperBannerNetwork
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 1
END
INSERT INTO [SkyscraperBannerNetwork] ([Id],[HTMLCode]) VALUES (
N'' + @MaxId,N'<iframe src="http://faucet4all.com/Ads/AdhitzSkyscraper.html" scrolling="no" style="width:128px; height:608px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>');
END

IF NOT EXISTS(Select 1 From SkyscraperBannerNetwork Where HTMLCode = '<iframe src="http://faucet4all.com/Ads/AdhitzSkyscraperText.html" scrolling="no" style="width:128px; height:608px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>')
BEGIN
SELECT @MaxId = max(Id)+1 FROM SkyscraperBannerNetwork
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 1
END
INSERT INTO [SkyscraperBannerNetwork] ([Id],[HTMLCode]) VALUES (
N'' + @MaxId,N'<iframe src="http://faucet4all.com/Ads/AdhitzSkyscraperText.html" scrolling="no" style="width:128px; height:608px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>');
END

IF NOT EXISTS(Select 1 From SkyscraperBannerNetwork Where HTMLCode = '<iframe data-aa="1132118" src="//ad.a-ads.com/1132118?size=120x600" scrolling="no" style="width:128px; height:608px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>')
BEGIN
SELECT @MaxId = max(Id)+1 FROM SkyscraperBannerNetwork
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 1
END
INSERT INTO [SkyscraperBannerNetwork] ([Id],[HTMLCode]) VALUES (
N'' + @MaxId,N'<iframe data-aa="1132118" src="//ad.a-ads.com/1132118?size=120x600" scrolling="no" style="width:128px; height:608px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>');
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'SkyscraperBannerNetworkCounter')
BEGIN
CREATE TABLE [SkyscraperBannerNetworkCounter] (
    [Id] INT DEFAULT 1 NOT NULL
);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'BannerNetwork')
BEGIN
CREATE TABLE [BannerNetwork] (
    [Id] INT DEFAULT ((1)) NOT NULL    
);
END


IF COL_LENGTH('BannerNetwork','HTMLCode') IS NULL
BEGIN
ALTER TABLE BannerNetwork ADD  [HTMLCode] VARCHAR(MAX) DEFAULT '<p>Advertise here for as low as 1$</p>' NOT NULL
END

IF NOT EXISTS(Select 1 From BannerNetwork Where HTMLCode = '<iframe src="http://faucet4all.com/Ads/Standard.html" scrolling="no" style="width:476px; height:68px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>')
BEGIN
SELECT @MaxId = max(Id)+1 FROM BannerNetwork
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 1
END
INSERT INTO [BannerNetwork] ([Id],[HTMLCode]) VALUES (
N'' + @MaxId,N'<iframe src="http://faucet4all.com/Ads/Standard.html" scrolling="no" style="width:476px; height:68px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>');
END

IF NOT EXISTS(Select 1 From BannerNetwork Where HTMLCode = '<iframe data-aa="1128057" src="//ad.a-ads.com/1128057?size=468x60" scrolling="no" style="width:476px; height:68px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>')
BEGIN
SELECT @MaxId = max(Id)+1 FROM BannerNetwork
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 1
END
INSERT INTO [BannerNetwork] ([Id],[HTMLCode]) VALUES (
N'' + @MaxId,N'<iframe data-aa="1128057" src="//ad.a-ads.com/1128057?size=468x60" scrolling="no" style="width:476px; height:68px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>');
END

IF NOT EXISTS(Select 1 From BannerNetwork Where HTMLCode = '<iframe src="http://faucet4all.com/Ads/AdhitzStandard.html" scrolling="no" style="width:476px; height:68px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>')
BEGIN
SELECT @MaxId = max(Id)+1 FROM BannerNetwork
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 1
END
INSERT INTO [BannerNetwork] ([Id],[HTMLCode]) VALUES (
N'' + @MaxId,N'<iframe src="http://faucet4all.com/Ads/AdhitzStandard.html" scrolling="no" style="width:476px; height:68px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>');
END

IF NOT EXISTS(Select 1 From BannerNetwork Where HTMLCode = '<iframe src="http://faucet4all.com/Ads/AdhitzStandardText.html" scrolling="no" style="width:476px; height:68px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>')
BEGIN
SELECT @MaxId = max(Id)+1 FROM BannerNetwork
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 1
END
INSERT INTO [BannerNetwork] ([Id],[HTMLCode]) VALUES (
N'' + @MaxId,N'<iframe src="http://faucet4all.com/Ads/AdhitzStandardText.html" scrolling="no" style="width:476px; height:68px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>');
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'SquareBannerNetwork')
BEGIN
CREATE TABLE [SquareBannerNetwork] (
    [Id] INT DEFAULT ((1)) NOT NULL    
);
END

IF COL_LENGTH('SquareBannerNetwork','HTMLCode') IS NULL
BEGIN
ALTER TABLE SquareBannerNetwork ADD  [HTMLCode] VARCHAR(MAX) DEFAULT '<p>Advertise here for as low as 1$</p>' NOT NULL
END


IF NOT EXISTS(Select 1 From SquareBannerNetwork Where HTMLCode = '<iframe src="http://faucet4all.com/Ads/Square.html" scrolling="no" style="width:133px; height:133px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>')
BEGIN
SELECT @MaxId = max(Id)+1 FROM SquareBannerNetwork
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 1
END
INSERT INTO [SquareBannerNetwork] ([Id],[HTMLCode]) VALUES (
N'' + @MaxId,N'<iframe src="http://faucet4all.com/Ads/Square.html" scrolling="no" style="width:133px; height:133px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>');
END

IF NOT EXISTS(Select 1 From SquareBannerNetwork Where HTMLCode = '<iframe data-aa="1128055" src="//ad.a-ads.com/1128055?size=125x125" scrolling="no" style="width:125px; height:125px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>')
BEGIN
SELECT @MaxId = max(Id)+1 FROM SquareBannerNetwork
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 1
END
INSERT INTO [SquareBannerNetwork] ([Id],[HTMLCode]) VALUES (
N'' + @MaxId,N'<iframe data-aa="1128055" src="//ad.a-ads.com/1128055?size=125x125" scrolling="no" style="width:125px; height:125px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>');
END

IF NOT EXISTS(Select 1 From SquareBannerNetwork Where HTMLCode = '<iframe src="http://faucet4all.com/Ads/AdhitzSquare125.html" scrolling="no" style="width:133px; height:133px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>')
BEGIN
SELECT @MaxId = max(Id)+1 FROM SquareBannerNetwork
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 1
END
INSERT INTO [SquareBannerNetwork] ([Id],[HTMLCode]) VALUES (
N'' + @MaxId,N'<iframe src="http://faucet4all.com/Ads/AdhitzSquare125.html" scrolling="no" style="width:133px; height:133px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>');
END

IF NOT EXISTS(Select 1 From SquareBannerNetwork Where HTMLCode = '<iframe src="http://faucet4all.com/Ads/AdhitzSquare125Text.html" scrolling="no" style="width:133px; height:133px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>')
BEGIN
SELECT @MaxId = max(Id)+1 FROM SquareBannerNetwork
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 1
END
INSERT INTO [SquareBannerNetwork] ([Id],[HTMLCode]) VALUES (
N'' + @MaxId,N'<iframe src="http://faucet4all.com/Ads/AdhitzSquare125Text.html" scrolling="no" style="width:133px; height:133px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>');
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'SquareBannerNetworkCounter')
BEGIN
CREATE TABLE [SquareBannerNetworkCounter] (
    [Id] INT DEFAULT 1 NOT NULL
);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'SquareBannerRotator')
BEGIN
CREATE TABLE [SquareBannerRotator] (
    [Id] INT DEFAULT ((1)) NOT NULL,   
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
END

IF COL_LENGTH('SquareBannerRotator','Reference') IS NULL
BEGIN
ALTER TABLE SquareBannerRotator ADD [Reference] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL
END
IF COL_LENGTH('SquareBannerRotator','Creation') IS NULL
BEGIN
ALTER TABLE SquareBannerRotator ADD [Creation] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('SquareBannerRotator','Username') IS NULL
BEGIN
ALTER TABLE SquareBannerRotator ADD [Username] varchar(10) DEFAULT ('Admin') NOT NULL
END
IF COL_LENGTH('SquareBannerRotator','ImageLink') IS NULL
BEGIN
ALTER TABLE SquareBannerRotator ADD [ImageLink]  VARCHAR (MAX) NOT NULL
END
IF COL_LENGTH('SquareBannerRotator','TargetLink') IS NULL
BEGIN
ALTER TABLE SquareBannerRotator ADD [TargetLink] VARCHAR (MAX) NOT NULL
END
IF COL_LENGTH('SquareBannerRotator','Clicks') IS NULL
BEGIN
ALTER TABLE SquareBannerRotator ADD [Clicks] int DEFAULT 0 NOT NULL
END
IF COL_LENGTH('SquareBannerRotator','Impressions') IS NULL
BEGIN
ALTER TABLE SquareBannerRotator ADD [Impressions] int DEFAULT 0 NOT NULL
END
IF COL_LENGTH('SquareBannerRotator','IsTimeBased') IS NULL
BEGIN
ALTER TABLE SquareBannerRotator ADD [IsTimeBased] BIT DEFAULT 0 NOT NULL
END
IF COL_LENGTH('SquareBannerRotator','Credit') IS NULL
BEGIN
ALTER TABLE SquareBannerRotator ADD [Credit] BIGINT DEFAULT 0 NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'SquareBannerRotatorCounter')
BEGIN
CREATE TABLE [SquareBannerRotatorCounter] (
    [Id] INT DEFAULT ((0)) NOT NULL
);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PTPSquareBannerNetwork')
BEGIN
CREATE TABLE [PTPSquareBannerNetwork] (
    [Id] INT DEFAULT ((1)) NOT NULL    
);
END

IF COL_LENGTH('PTPSquareBannerNetwork','HTMLCode') IS NULL
BEGIN
ALTER TABLE PTPSquareBannerNetwork ADD [HTMLCode] VARCHAR(MAX) DEFAULT '<p>Advertise here for as low as 1$</p>' NOT NULL
END


IF NOT EXISTS(Select 1 From PTPSquareBannerNetwork Where HTMLCode = '<iframe src="http://faucet4all.com/Ads/Square.html" scrolling="no" style="width:133px; height:133px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>')
BEGIN
SELECT @MaxId = max(Id)+1 FROM PTPSquareBannerNetwork
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 1
END
INSERT INTO [PTPSquareBannerNetwork] ([Id],[HTMLCode]) VALUES (
N'' + @MaxId,N'<iframe src="http://faucet4all.com/Ads/Square.html" scrolling="no" style="width:133px; height:133px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>');
END

IF NOT EXISTS(Select 1 From PTPSquareBannerNetwork Where HTMLCode = '<iframe data-aa="1130119" src="//ad.a-ads.com/1130119?size=125x125" scrolling="no" style="width:125px; height:125px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>')
BEGIN
SELECT @MaxId = max(Id)+1 FROM PTPSquareBannerNetwork
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 1
END
INSERT INTO [PTPSquareBannerNetwork] ([Id],[HTMLCode]) VALUES (
N'' + @MaxId,N'<iframe data-aa="1130119" src="//ad.a-ads.com/1130119?size=125x125" scrolling="no" style="width:125px; height:125px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>');
END

IF NOT EXISTS(Select 1 From PTPSquareBannerNetwork Where HTMLCode = '<iframe src="http://ptp.faucet4all.com/Ads/AdhitzSquare125.html" scrolling="no" style="width:133px; height:133px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>')
BEGIN
SELECT @MaxId = max(Id)+1 FROM PTPSquareBannerNetwork
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 1
END
INSERT INTO [PTPSquareBannerNetwork] ([Id],[HTMLCode]) VALUES (
N'' + @MaxId,N'<iframe src="http://ptp.faucet4all.com/Ads/AdhitzSquare125.html" scrolling="no" style="width:133px; height:133px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>');
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PTPSquareBannerNetworkCounter')
BEGIN
CREATE TABLE [PTPSquareBannerNetworkCounter] (
    [Id] INT DEFAULT 1 NOT NULL
);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Advertise')
BEGIN
CREATE TABLE [Advertise] (
    [Reference] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL  
);
END

IF COL_LENGTH('Advertise','Name') IS NULL
BEGIN
ALTER TABLE Advertise ADD [Name] VARCHAR(70) DEFAULT 'Unkown' NOT NULL
END
IF COL_LENGTH('Advertise','IsTimeBased') IS NULL
BEGIN
ALTER TABLE Advertise ADD  [IsTimeBased] bit DEFAULT ((0)) NOT NULL
END
IF COL_LENGTH('Advertise','Credit') IS NULL
BEGIN
ALTER TABLE Advertise ADD  [Credit] BIGINT DEFAULT 0 NOT NULL
END
IF COL_LENGTH('Advertise','Price') IS NULL
BEGIN
ALTER TABLE Advertise ADD  [Price] DECIMAL (9, 8) DEFAULT ((0.00000000)) NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'OrderHistory')
BEGIN
CREATE TABLE [OrderHistory] (
    [Reference]     UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL
);
END

IF COL_LENGTH('OrderHistory','OrderDateTime') IS NULL
BEGIN
ALTER TABLE OrderHistory ADD [OrderDateTime] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('OrderHistory','Username') IS NULL
BEGIN
ALTER TABLE OrderHistory ADD  [Username] VARCHAR(12) NOT NULL
END
IF COL_LENGTH('OrderHistory','Name') IS NULL
BEGIN
ALTER TABLE OrderHistory ADD  [Name] VARCHAR(70) DEFAULT 'Unknown' NOT NULL
END
IF COL_LENGTH('OrderHistory','IsTimeBased') IS NULL
BEGIN
ALTER TABLE OrderHistory ADD  [IsTimeBased] BIT DEFAULT (0) NOT NULL
END
IF COL_LENGTH('OrderHistory','Credit') IS NULL
BEGIN
ALTER TABLE OrderHistory ADD  [Credit] BIGINT DEFAULT (0) NOT NULL
END
IF COL_LENGTH('OrderHistory','Price') IS NULL
BEGIN
ALTER TABLE OrderHistory ADD  [Price] DECIMAL(9, 8) DEFAULT ((0.00000000)) NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'FaucetHubHistory')
BEGIN
CREATE TABLE [FaucetHubHistory] (
    [Reference]     UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL
);
END

IF COL_LENGTH('FaucetHubHistory','DateTime') IS NULL
BEGIN
ALTER TABLE FaucetHubHistory ADD  [DateTime] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('FaucetHubHistory','PayoutId') IS NULL
BEGIN
ALTER TABLE FaucetHubHistory ADD  [PayoutId] bigint DEFAULT (0) NOT NULL
END
IF COL_LENGTH('FaucetHubHistory','Hash') IS NULL
BEGIN
ALTER TABLE FaucetHubHistory ADD  [Hash] VARCHAR(max) DEFAULT 'Unspecified' NOT NULL
END
IF COL_LENGTH('FaucetHubHistory','Address') IS NULL
BEGIN
ALTER TABLE FaucetHubHistory ADD  [Address] VARCHAR(max) DEFAULT 'Unspecified' NOT NULL
END
IF COL_LENGTH('FaucetHubHistory','Amount') IS NULL
BEGIN
ALTER TABLE FaucetHubHistory ADD  [Amount] decimal(9,8) DEFAULT (0.00000000) NOT NULL
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'LinkShortnersHistory')
BEGIN
CREATE TABLE [LinkShortnersHistory] (
    [Reference]     UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL
);
END

IF COL_LENGTH('LinkShortnersHistory','Username') IS NULL
BEGIN
ALTER TABLE LinkShortnersHistory ADD [Username] VARCHAR(10) DEFAULT 'Unknown' NOT NULL
END
IF COL_LENGTH('LinkShortnersHistory','DateTime') IS NULL
BEGIN
ALTER TABLE LinkShortnersHistory ADD [DateTime] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('LinkShortnersHistory','Amount') IS NULL
BEGIN
ALTER TABLE LinkShortnersHistory ADD [Amount] DECIMAL (9, 8) DEFAULT (0.00000000) NOT NULL
END
IF COL_LENGTH('LinkShortnersHistory','Link') IS NULL
BEGIN
ALTER TABLE LinkShortnersHistory ADD [Link] VARCHAR (MAX) DEFAULT('Unknown') NOT NULL
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'EmailHistory')
BEGIN
CREATE TABLE [EmailHistory] (
    [Reference]     UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL
);
END

IF COL_LENGTH('EmailHistory','Recipient') IS NULL
BEGIN
ALTER TABLE EmailHistory ADD [Recipient] VARCHAR(max) DEFAULT 'Unknown' NOT NULL
END
IF COL_LENGTH('EmailHistory','DateTime') IS NULL
BEGIN
ALTER TABLE EmailHistory ADD [DateTime] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('EmailHistory','Subject') IS NULL
BEGIN
ALTER TABLE EmailHistory ADD [Subject] VARCHAR(max) DEFAULT('Unknown') NOT NULL
END
IF COL_LENGTH('EmailHistory','Message') IS NULL
BEGIN
ALTER TABLE EmailHistory ADD [Message] VARCHAR (MAX) DEFAULT('Unknown') NOT NULL
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'BonusAds')
BEGIN
CREATE TABLE [BonusAds] (
    [Id]       UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,    
    [Link]     VARCHAR(max) DEFAULT ('http://faucet4all.com') NOT NULL,    
    PRIMARY KEY CLUSTERED ([Id] ASC),
);
END

IF COL_LENGTH('BonusAds','Creation') IS NULL
BEGIN
ALTER TABLE BonusAds ADD [Creation] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('BonusAds','Limit') IS NULL
BEGIN
ALTER TABLE BonusAds ADD [Limit] INT DEFAULT ((1)) NOT NULL
END
IF COL_LENGTH('BonusAds','Username') IS NULL
BEGIN
ALTER TABLE BonusAds ADD [Username] varchar(10) DEFAULT ('Admin') NOT NULL
END
IF COL_LENGTH('BonusAds','HitsReceived') IS NULL
BEGIN
ALTER TABLE BonusAds ADD [HitsReceived] int DEFAULT 0 NOT NULL
END
IF COL_LENGTH('BonusAds','IsTimeBased') IS NULL
BEGIN
ALTER TABLE BonusAds ADD [IsTimeBased] BIT DEFAULT 0 NOT NULL
END
IF COL_LENGTH('BonusAds','Credit') IS NULL
BEGIN
ALTER TABLE BonusAds ADD [Credit] BIGINT DEFAULT 0 NOT NULL
END
IF COL_LENGTH('BonusAds','MaxOut') IS NULL
BEGIN
ALTER TABLE BonusAds ADD [MaxOut] BIT DEFAULT 0 NOT NULL
END
IF COL_LENGTH('BonusAds','Title') IS NULL
BEGIN
ALTER TABLE BonusAds ADD [Title] varchar(20) DEFAULT ('Bonus Ad') NOT NULL
END
IF COL_LENGTH('BonusAds','Description') IS NULL
BEGIN
ALTER TABLE BonusAds ADD [Description] varchar(50) DEFAULT ('Click here to view the Bonus Ad') NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'BonusAdsRecord')
BEGIN
CREATE TABLE [BonusAdsRecord] (
    [Username]                                   VARCHAR(10) DEFAULT 'Unknown' NOT NULL,    
    PRIMARY KEY CLUSTERED ([Username] ASC)
);
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'BonusAdsHistory')
BEGIN
CREATE TABLE [BonusAdsHistory] (
    [Reference]     UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL
);
END

IF COL_LENGTH('BonusAdsHistory','Username') IS NULL
BEGIN
ALTER TABLE BonusAdsHistory ADD [Username] VARCHAR(10) DEFAULT 'Unknown' NOT NULL
END
IF COL_LENGTH('BonusAdsHistory','DateTime') IS NULL
BEGIN
ALTER TABLE BonusAdsHistory ADD [DateTime] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('BonusAdsHistory','Amount') IS NULL
BEGIN
ALTER TABLE BonusAdsHistory ADD [Amount] DECIMAL (9, 8) DEFAULT (0.00000000) NOT NULL
END
IF COL_LENGTH('BonusAdsHistory','Link') IS NULL
BEGIN
ALTER TABLE BonusAdsHistory ADD [Link] VARCHAR (MAX) DEFAULT('Unknown') NOT NULL
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PTPBannerNetwork')
BEGIN
CREATE TABLE [PTPBannerNetwork] (
    [Id] INT DEFAULT ((1)) NOT NULL    
);
END

IF COL_LENGTH('PTPBannerNetwork','HTMLCode') IS NULL
BEGIN
ALTER TABLE PTPBannerNetwork ADD [HTMLCode] VARCHAR(MAX) DEFAULT '<p>Advertise here for as low as 1$</p>' NOT NULL
END


IF NOT EXISTS(Select 1 From PTPBannerNetwork Where HTMLCode = '<iframe src="http://faucet4all.com/Ads/AdhitzStandard.html" scrolling="no" style="width:478px; height:68px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>')
BEGIN
SELECT @MaxId = max(Id)+1 FROM PTPBannerNetwork
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 0
END
INSERT INTO [PTPBannerNetwork] ([Id],[HTMLCode]) VALUES (
N'' + @MaxId,N'<iframe src="http://faucet4all.com/Ads/AdhitzStandard.html" scrolling="no" style="width:478px; height:68px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>');
END

IF NOT EXISTS(Select 1 From PTPBannerNetwork Where HTMLCode = '<iframe src="http://faucet4all.com/Ads/AdhitzStandardText.html" scrolling="no" style="width:478px; height:68px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>')
BEGIN
SELECT @MaxId = max(Id)+1 FROM PTPBannerNetwork
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 0
END
INSERT INTO [PTPBannerNetwork] ([Id],[HTMLCode]) VALUES (
N'' + @MaxId,N'<iframe src="http://faucet4all.com/Ads/AdhitzStandardText.html" scrolling="no" style="width:478px; height:68px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>');
END

IF NOT EXISTS(Select 1 From PTPBannerNetwork Where HTMLCode = '<iframe data-aa="1174119" src="//ad.a-ads.com/1174119?size=468x60" scrolling="no" style="width:478px; height:60px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>')
BEGIN
SELECT @MaxId = max(Id)+1 FROM PTPBannerNetwork
IF(@MaxId IS NULL OR @MaxId = '')
BEGIN
SET @MaxId = 0
END
INSERT INTO [PTPBannerNetwork] ([Id],[HTMLCode]) VALUES (
N'' + @MaxId,N'<iframe data-aa="1174119" src="//ad.a-ads.com/1174119?size=468x60" scrolling="no" style="width:478px; height:60px; border:0px; padding:0; overflow:hidden" allowtransparency="true"></iframe>');
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PTPBannerNetworkCounter')
BEGIN
CREATE TABLE [PTPBannerNetworkCounter] (
    [Id] INT DEFAULT 0 NOT NULL
);
END


USE AdminPanel;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'AdminPanel')
BEGIN
CREATE TABLE [AdminPanel] (
    [SessionId]     UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,    
    PRIMARY KEY CLUSTERED ([SessionId] ASC)
);
END

IF COL_LENGTH('AdminPanel','SessionExpiry') IS NULL
BEGIN
ALTER TABLE AdminPanel ADD [SessionExpiry] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('AdminPanel','Name') IS NULL
BEGIN
ALTER TABLE AdminPanel ADD [Name] varchar(max) DEFAULT ('Unknown') NOT NULL
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'WithdrawalHistory')
BEGIN
CREATE TABLE [WithdrawalHistory] (
    [SessionId]     UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,    
    PRIMARY KEY CLUSTERED ([SessionId] ASC)
);
END

IF COL_LENGTH('WithdrawalHistory','SessionExpiry') IS NULL
BEGIN
ALTER TABLE WithdrawalHistory ADD [SessionExpiry] DATETIME DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('WithdrawalHistory','Name') IS NULL
BEGIN
ALTER TABLE WithdrawalHistory ADD [Name] varchar(max) DEFAULT ('Unknown') NOT NULL
END
IF COL_LENGTH('WithdrawalHistory','DateTime') IS NULL
BEGIN
ALTER TABLE WithdrawalHistory ADD [DateTime] datetime DEFAULT (getdate()) NOT NULL
END
IF COL_LENGTH('WithdrawalHistory','Amount') IS NULL
BEGIN
ALTER TABLE WithdrawalHistory ADD [Amount] decimal(9, 8) DEFAULT (0.00000000) NOT NULL
END
IF COL_LENGTH('WithdrawalHistory','Address') IS NULL
BEGIN
ALTER TABLE WithdrawalHistory ADD [Address] varchar(max) DEFAULT ('Unknown') NOT NULL
END
IF COL_LENGTH('WithdrawalHistory','Transaction') IS NULL
BEGIN
ALTER TABLE WithdrawalHistory ADD [Transaction] varchar(max) DEFAULT ('Unknown') NOT NULL
END
IF COL_LENGTH('WithdrawalHistory','Confirmations') IS NULL
BEGIN
ALTER TABLE WithdrawalHistory ADD [Confirmations] INT DEFAULT (0) NOT NULL
END

--Following is the script for stored procedures

USE Faucet4u;  
GO  
CREATE OR ALTER PROCEDURE UpdateBalance   
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
CREATE OR ALTER PROCEDURE UpdatePTPBalance   
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

	SELECT @Level1 = Value FROM Settings WHERE Name = 'PTPLevel1'
	SELECT @Level2 = Value FROM Settings WHERE Name = 'PTPLevel2'
	SELECT @Level3 = Value FROM Settings WHERE Name = 'PTPLevel3'

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
CREATE OR ALTER PROCEDURE UpdateBonusAdBalance   
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

SELECT Username, LinkShortnerEarning, BonusAdEarning, PTPUniqueEarning, PTPNonUnique1Earning, PTPNonUnique2Earning, PTPNonUnique3Earning, OfferwallEarning, LastClaim FROM Users WHERE Referrer = @OriginalUsername

DECLARE @Level1 TABLE(
    Username varchar(10) NOT NULL
);

INSERT INTO @Level1 (Username)
SELECT Username FROM Users WHERE Referrer = @OriginalUsername

DECLARE @Level2 TABLE(
    Username varchar(10) NOT NULL,
	LinkShortnerEarning decimal(9, 8) NOT NULL,
	BonusAdEarning decimal(9, 8) NOT NULL,
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


INSERT INTO @Level2 (Username, LinkShortnerEarning, BonusAdEarning, PTPUniqueEarning, PTPNonUnique1Earning, PTPNonUnique2Earning, PTPNonUnique3Earning, OfferwallEarning, LastClaim)
SELECT Username, LinkShortnerEarning, BonusAdEarning, PTPUniqueEarning, PTPNonUnique1Earning, PTPNonUnique2Earning, PTPNonUnique3Earning, OfferwallEarning, LastClaim FROM Users WHERE Referrer = @ReferralUsername
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
	BonusAdEarning decimal(9, 8) NOT NULL,
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

INSERT INTO @Level3 (Username, LinkShortnerEarning, BonusAdEarning, PTPUniqueEarning, PTPNonUnique1Earning, PTPNonUnique2Earning, PTPNonUnique3Earning, OfferwallEarning, LastClaim)
SELECT Username, LinkShortnerEarning, BonusAdEarning, PTPUniqueEarning, PTPNonUnique1Earning, PTPNonUnique2Earning, PTPNonUnique3Earning, OfferwallEarning, LastClaim FROM Users WHERE Username = @ReferralUsername
SET @RowCounter = @RowCounter + 1
END

SELECT * FROM @Level2
SELECT * FROM @Level3
END
GO

USE Faucet4u;  
GO  
CREATE OR ALTER PROCEDURE SelectAllReferralData
@OriginalUsername varchar(10),
@Level int
AS

BEGIN

IF (@Level = 1)
BEGIN
SELECT * FROM Users WHERE Referrer = @OriginalUsername
END

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

IF (@Level = 2)
BEGIN
SELECT * FROM Users INNER JOIN @Level2 ON Users.Username = [@Level2].Username;
END
IF (@Level = 3)
BEGIN
SELECT * FROM Users INNER JOIN @Level3 ON Users.Username = [@Level3].Username;
END

END
GO


USE Faucet4u;  
GO  
CREATE OR ALTER PROCEDURE UpdatePTPSourceLinkAndHitsReceivedAndCounter
@UsernameInput varchar(10),
@PTPSourceLinkInput varchar(max),
@PTPAbsoluteSourceLinkInput varchar(max),
@CounterIdInput int
AS

BEGIN

DECLARE @PTPAbsoluteSourceLink varchar(max) = @PTPAbsoluteSourceLinkInput
DECLARE @PTPSourceLink varchar(max) = @PTPSourceLinkInput
DECLARE @Username varchar(12) = @UsernameInput
DECLARE @IsTimeBased bit
DECLARE @Credit int
DECLARE @CounterId int = @CounterIdInput

IF (@PTPSourceLink IS NULL OR @PTPSourceLink = '')
BEGIN
SET @PTPSourceLink = 'Undefined'
END
IF (@Username IS NULL OR @Username = '')
BEGIN
SET @Username = 'Unknown'
END
IF (@PTPSourceLink IS NULL OR @PTPSourceLink = '')
BEGIN
SET @PTPSourceLink = 'Undefined'
END  
IF NOT EXISTS(SELECT site FROM PTPSource WHERE site = @PTPSourceLink AND Username = @Username)
BEGIN
INSERT INTO PTPSource(site, Username) VALUES(@PTPSourceLink, @Username)
END
IF (@PTPAbsoluteSourceLink IS NULL OR @PTPAbsoluteSourceLink = '')
BEGIN
SET @PTPAbsoluteSourceLink = 'Undefined'
END                                        
IF NOT EXISTS(SELECT site FROM PTPAbsoluteSource WHERE site = @PTPAbsoluteSourceLink AND Username = @Username)
BEGIN
INSERT INTO PTPAbsoluteSource(site, Username) VALUES(@PTPAbsoluteSourceLink, @Username)
END

UPDATE PTPSource SET counter += 1 WHERE site = @PTPSourceLink AND Username = @Username
UPDATE PTPAbsoluteSource SET counter += 1 WHERE site = @PTPAbsoluteSourceLink AND Username = @Username

--Now update the hit received and credit

SELECT @IsTimeBased = IsTimeBased, @Credit = Credit FROM PTP WHERE Id = @CounterId

IF (@IsTimeBased = 0)
BEGIN
UPDATE PTP SET HitsReceived += 1, Credit -= 1 WHERE Id = @CounterId	 
END
ELSE IF (@IsTimeBased = 1)
BEGIN
DECLARE @CurrentDateTime DATETIME = getdate()
DECLARE @InitialDateTime DATETIME
SELECT @InitialDateTime = Creation FROM PTP WHERE Id = @CounterId
IF (DATEDIFF(DAY, @InitialDateTime, @CurrentDateTime) >= @Credit)
BEGIN
UPDATE PTP SET HitsReceived += 1, Credit = 0 WHERE Id = @CounterId
END
ELSE
BEGIN
UPDATE PTP SET HitsReceived += 1 WHERE Id = @CounterId
END
END

--Now update PTP Counter for the eligible link

DECLARE @MaxId int
DECLARE @LoopCounter int = 0
DECLARE @HitsReceived int
DECLARE @MaxHits int
SELECT @MaxId = max(Id) FROM PTP

WHILE (@CounterId <= @MaxId)
BEGIN

SET @LoopCounter += 1
SET @CounterId += 1
IF(@CounterId > @MaxId)
BEGIN
SET @CounterId = 0
END

SELECT @Credit = Credit, @IsTimeBased = IsTimeBased, @HitsReceived = HitsReceived FROM PTP WHERE Id = @CounterId

IF(@LoopCounter > @MaxId)
BEGIN
UPDATE PTPCounter SET Id = 0
BREAK
END

IF(@Credit > 0)
BEGIN

SELECT @MaxHits = Value FROM Settings WHERE Name = 'HitsFor1PTPDayCredit'
SET @MaxHits *= @Credit

IF (@IsTimeBased = 1 AND @HitsReceived <= @MaxHits)
BEGIN
UPDATE PTPCounter SET Id = @CounterId
BREAK
END
ELSE IF (@IsTimeBased = 1 AND @HitsReceived > @MaxHits)
BEGIN
Continue
END
ELSE IF (@IsTimeBased = 0)
BEGIN
UPDATE PTPCounter SET Id = @CounterId
BREAK
END

END
END

END
GO



USE Faucet4u;  
GO  
CREATE OR ALTER PROCEDURE UpdateBannerRotatorCounter
AS

BEGIN

DECLARE @MaxId int
DECLARE @CounterId int
DECLARE @LoopCounter int = 0
DECLARE @Impressions int
DECLARE @MaxImpressions int
DECLARE @IsTimeBased bit
DECLARE @Credit int

SELECT TOP 1 @CounterId = Id FROM BannerRotatorCounter
SELECT @MaxId = max(Id) FROM BannerRotator

IF((SELECT Count(Id) FROM BannerRotatorCounter) = 0)
BEGIN
INSERT INTO BannerRotatorCounter(Id) VALUES(0)
END


WHILE (@CounterId <= @MaxId)
BEGIN

SET @LoopCounter += 1
SET @CounterId += 1
IF(@CounterId > @MaxId)
BEGIN
SET @CounterId = 0
END

SELECT @Credit = Credit, @IsTimeBased = IsTimeBased, @Impressions = Impressions FROM BannerRotator WHERE Id = @CounterId

IF(@LoopCounter > @MaxId)
BEGIN
UPDATE BannerRotatorCounter SET Id = 0
BREAK
END

IF(@Credit > 0)
BEGIN

SELECT @MaxImpressions = Value FROM Settings WHERE Name = 'HitsFor1BannerDayCredit'
SET @MaxImpressions *= @Credit

IF (@IsTimeBased = 1 AND @Impressions <= @MaxImpressions)
BEGIN
UPDATE BannerRotatorCounter SET Id = @CounterId
BREAK
END
ELSE IF (@IsTimeBased = 1 AND @Impressions > @MaxImpressions)
BEGIN
Continue
END
ELSE IF (@IsTimeBased = 0)
BEGIN
UPDATE BannerRotatorCounter SET Id = @CounterId
BREAK
END

END
END

END
GO

USE Faucet4u;  
GO  
CREATE OR ALTER PROCEDURE UpdateSquareBannerRotatorCounter
AS

BEGIN

DECLARE @MaxId int
DECLARE @CounterId int
DECLARE @LoopCounter int = 0
DECLARE @Impressions int
DECLARE @MaxImpressions int
DECLARE @IsTimeBased bit
DECLARE @Credit int

SELECT TOP 1 @CounterId = Id FROM SquareBannerRotatorCounter
SELECT @MaxId = max(Id) FROM SquareBannerRotator

IF((SELECT Count(Id) FROM SquareBannerRotatorCounter) = 0)
BEGIN
INSERT INTO SquareBannerRotatorCounter(Id) VALUES(0)
END


WHILE (@CounterId <= @MaxId)
BEGIN

SET @LoopCounter += 1
SET @CounterId += 1
IF(@CounterId > @MaxId)
BEGIN
SET @CounterId = 0
END

SELECT @Credit = Credit, @IsTimeBased = IsTimeBased, @Impressions = Impressions FROM SquareBannerRotator WHERE Id = @CounterId

IF(@LoopCounter > @MaxId)
BEGIN
UPDATE SquareBannerRotatorCounter SET Id = 0
BREAK
END

IF(@Credit > 0)
BEGIN

SELECT @MaxImpressions = Value FROM Settings WHERE Name = 'HitsFor1SquareBannerDayCredit'
SET @MaxImpressions *= @Credit

IF (@IsTimeBased = 1 AND @Impressions <= @MaxImpressions)
BEGIN
UPDATE SquareBannerRotatorCounter SET Id = @CounterId
BREAK
END
ELSE IF (@IsTimeBased = 1 AND @Impressions > @MaxImpressions)
BEGIN
Continue
END
ELSE IF (@IsTimeBased = 0)
BEGIN
UPDATE SquareBannerRotatorCounter SET Id = @CounterId
BREAK
END

END
END

END
GO

USE Faucet4u;  
GO  
CREATE OR ALTER PROCEDURE UpdateBonusAdCredit
@IdInput uniqueidentifier
AS

BEGIN

DECLARE @Id uniqueidentifier = @IdInput
DECLARE @HitsReceived int
DECLARE @MaxHits int
DECLARE @IsTimeBased bit
DECLARE @Credit int

SELECT @Credit = Credit, @IsTimeBased = IsTimeBased, @HitsReceived = HitsReceived FROM BonusAds WHERE Id = @Id
SELECT @MaxHits = Value FROM Settings WHERE Name = 'HitsFor1BonusAdDayCredit'

SET @MaxHits *= @Credit

IF (@IsTimeBased = 0)
BEGIN
UPDATE BonusAds SET HitsReceived += 1, Credit -= 1 WHERE Id = @Id	 
END
ELSE IF (@IsTimeBased = 1)
BEGIN
DECLARE @CurrentDateTime DATETIME = getdate()
DECLARE @InitialDateTime DATETIME
SELECT @InitialDateTime = Creation FROM BonusAds WHERE Id = @Id
IF (DATEDIFF(DAY, @InitialDateTime, @CurrentDateTime) >= @Credit)
BEGIN
UPDATE BonusAds SET HitsReceived += 1, Credit = 0 WHERE Id = @Id
END
ELSE IF (@MaxHits > @HitsReceived)
BEGIN
UPDATE BonusAds SET HitsReceived += 1, MaxOut = 1 WHERE Id = @Id
END
ELSE IF (@MaxHits <= @HitsReceived AND DATEDIFF(DAY, @InitialDateTime, @CurrentDateTime) <= @Credit)
BEGIN
UPDATE BonusAds SET HitsReceived += 1 WHERE Id = @Id
END
END

END
GO

USE Faucet4u;  
GO  
CREATE OR ALTER PROCEDURE SelectBonusAds
@SessionIdInput uniqueidentifier
AS
BEGIN
DECLARE @Username varchar(10)
DECLARE @SessionId uniqueidentifier = @SessionIdInput

DECLARE @FilteredBonusAds TABLE(
    Id uniqueidentifier NOT NULL,
	Link varchar(max) NOT NULL,
	Title varchar(max) NOT NULL,
	[Description] varchar(max) NOT NULL
);

SELECT @Username = Username FROM Users WHERE SessionId = @SessionId AND SessionExpiry > getdate()	                   
IF NOT EXISTS(SELECT Username FROM BonusAdsRecord WHERE Username = @Username)
BEGIN
INSERT INTO BonusAdsRecord(Username) VALUES(@Username)
END
DECLARE @TotalLinks int
SELECT @TotalLinks = COUNT(*) FROM BonusAds
DECLARE @RowNumber int = 1
WHILE(@RowNumber <= @TotalLinks)
BEGIN
DECLARE @Id uniqueidentifier
WITH temporaryTable AS (SELECT (ROW_NUMBER() OVER (ORDER BY BonusAds.Creation)) as row,* FROM BonusAds)
SELECT @Id = Id FROM temporaryTable WHERE row = @RowNumber

DECLARE @IdString varchar(60) = convert(varchar(60), @Id)
DECLARE @Counter int 
DECLARE @MaxLimit int
DECLARE @Credit int
DECLARE @MaxOut bit
Select @MaxLimit = Limit, @Credit = Credit, @MaxOut = MaxOut FROM BonusAds WHERE Id = @Id
	                   			
DECLARE @dynamicQuery nvarchar(max) = 'SELECT @Counter = [' + @IdString + '] FROM BonusAdsRecord WHERE Username = ''' + @Username + ''''
EXEC sp_executeSQl @dynamicQuery, N'@Counter int output', @Counter output
IF NOT(@Counter >= @MaxLimit OR @Credit <= 0 OR @MaxOut = 1)
	BEGIN	                   					
		
		INSERT INTO @FilteredBonusAds (Id, Link, Title, Description)
		SELECT Id, Link, Title, Description FROM BonusAds WHERE Id = @Id AND Credit > 0 AND MaxOut = 0
	END	                                
SET @RowNumber = @RowNumber + 1
END		

SELECT * FROM @FilteredBonusAds	                   	  	
END
GO