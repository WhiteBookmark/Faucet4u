var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http, { 'pingTimeout': 100000, 'pingInterval': 100000 });


var cors = require('cors');
const sql = require('mssql')
var request = require('request');
var moment = require('moment');
//const BitcoinGateway = require('bitcoin-receive-payments')

//Custom variables for globals
const solveMediaSecret = "E9q-HnaLJfSAe6XI7vHnxvH6.ivgXc7P";
const sendEmailKey = "Ae^SolKz75H9";
const sendEmailAPI = 'https://localhost:5000/api/SendEmail';
const outdatedSocketKey = "uX3e4y^Po8Yi";
const claimDomainKey = "P46$SP*V7Qrz";

var allowedOrigins = ['http://localhost:8081',
    'http://localhost:8080',
    'http://localhost:3005',
    'https://localhost:5000',
    'https://localhost:5001'];


app.use(cors({
    origin: function (origin, callback) {
        // allow requests with no origin 
        // (like mobile apps or curl requests)
        if (!origin) return callback(null, true);
        if (allowedOrigins.indexOf(origin) === -1) {
            var msg = 'The CORS policy for this site does not ' +
                'allow access from the specified Origin.';
            return callback(new Error(msg), false);
        }
        return callback(null, true);
    }
}));

app.get('/', function (req, res) {

    res.send('<h1>Hello world</h1>');
});
//MS Sql connection string configuration

const config = {
    user: 'sa',
    password: '123',
    server: 'localhost',
    database: 'Faucet4u'
}

io.on('connection', function (socket) {
    //Send data
    socket.on('requestingUserData', function (sessionId) {


        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("SessionId", JSON.parse(sessionId).sessionId)
                .input("SocketSession", socket.id)
                .query(`BEGIN
UPDATE Users SET SocketSession = @SocketSession WHERE SessionId = @SessionId AND SessionExpiry > getdate()
DECLARE @Username varchar(10)
SELECT @Username = Username FROM Users WHERE SessionId = @SessionId AND SessionExpiry > getdate()
SELECT Id, Username, Email, DateRegistered, FaucetHubBitcoinAddress, LinkClaimInterval, Referrer, Balance, PurchaseBalance, TotalWithdrawn, LinkShortnerEarning, BonusAdEarning, PTPUniqueEarning, PTPNonUnique1Earning, PTPNonUnique2Earning, PTPNonUnique3Earning, OfferwallEarning, TotalOfferwallWithdrawn, OfferwallBalance, JustClaimed, ChatBanned, PTPCredit, PTPDayCredit, BannerCredit, BannerDayCredit, BonusAdCredit, BonusAdDayCredit FROM Users WHERE Username = @Username
SELECT * FROM LoginHistory WHERE Username = @Username
SELECT WithdrawalType, RequestedDate, RequestedAmount, Paid, WalletType, WalletAddress, Remarks FROM Withdrawal WHERE Username = @Username
EXEC SelectReferralData @OriginalUsername = @Username
SELECT Name, Value FROM Settings
SELECT * FROM Advertise WHERE Name = 'PTP' AND IsTimeBased = 0 ORDER BY Price
SELECT * FROM Advertise WHERE Name = 'PTP' AND IsTimeBased = 1 ORDER BY Price
SELECT * FROM Advertise WHERE Name = 'Banner' AND IsTimeBased = 0 ORDER BY Price
SELECT * FROM Advertise WHERE Name = 'Banner' AND IsTimeBased = 1 ORDER BY Price
SELECT * FROM OrderHistory WHERE Username = @Username
SELECT * FROM DepositHistory WHERE Username = @Username AND Confirmations > 5
SELECT TOP 100 Username, DateTime, Message FROM ChatHistory ORDER BY DateTime
SELECT * FROM PTP WHERE Username = @Username ORDER BY Creation
SELECT * FROM BannerRotator WHERE Username = @Username ORDER BY Creation
SELECT * FROM SquareBannerRotator WHERE Username = @Username ORDER BY Creation
SELECT * FROM BonusAds WHERE Username = @Username ORDER BY Creation
SELECT * FROM Advertise WHERE Name = 'BonusAd' AND IsTimeBased = 0 ORDER BY Price
SELECT * FROM Advertise WHERE Name = 'BonusAd' AND IsTimeBased = 1 ORDER BY Price
EXEC SelectBonusAds @SessionIdInput = @SessionId
END`)
        }).then(result => {
            socket.emit('receivingUserData', result);
            sql.close();
        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    //Send user claim link
    //First insert his ip address in database if not already
    socket.on('requestingClaimLink', function (data) {

        request.post(
            'http://verify.solvemedia.com/papi/verify',
            { qs: { privatekey: solveMediaSecret, challenge: JSON.parse(data).challenge, response: JSON.parse(data).response, remoteip: JSON.parse(data).ip } },
            function (error, response, body) {

                if (body.includes("true")) {

                    new sql.ConnectionPool(config).connect().then(pool => {
                        return pool.request()
                            .input("IPAddress", JSON.parse(data).ip)
                            .input("SessionIdInput", JSON.parse(data).sessionId)
                            .query(`
                    BEGIN
	                   DECLARE @Available bit = 0
	                   DECLARE @LinkClaimInterval datetime = getdate()
	                   DECLARE @LinkClaimCredit varchar(11)
	                   SELECT @LinkClaimCredit = Value FROM Settings WHERE Name = 'LinkClaimCredit'
	                   SELECT @LinkClaimInterval = LinkClaimInterval FROM Users WHERE SessionId = @SessionIdInput AND SessionExpiry > getdate()
	                   IF(@LinkClaimInterval < getdate())
	                   BEGIN
	                   DECLARE @IP varchar(20) = @IPAddress
	                   IF NOT EXISTS(SELECT IP FROM LinkShortnersRecord WHERE IP = @IP)
                           BEGIN
	                   		INSERT INTO LinkShortnersRecord(IP) VALUES(@IP)
	                   	END
	                   DECLARE @TotalLinks int
	                   	SELECT @TotalLinks = COUNT(*) FROM LinkShortners
	                   DECLARE @RowNumber int = 1
	                   	WHILE(@RowNumber <= @TotalLinks)
	                   		BEGIN
	                   			DECLARE @LinkId uniqueidentifier
	                   			WITH temporaryTable AS (SELECT (ROW_NUMBER() OVER (ORDER BY LinkShortners.Limit)) as row,* FROM LinkShortners)
	                   			SELECT @LinkId = Id FROM temporaryTable WHERE row = @RowNumber

	                   			DECLARE @LinkIdString varchar(60) = convert(varchar(60), @LinkId)
	                   			DECLARE @Counter int 
	                   			DECLARE @MaxLimit int
	                   			Select @MaxLimit = Limit from LinkShortners where Id = @LinkId
	                   			
	                   			DECLARE @dynamicQuery nvarchar(max) = 'SELECT @Counter = [' + @LinkIdString + '] FROM LinkShortnersRecord WHERE IP = ''' + @IP + ''''
	                   			EXEC sp_executeSQl @dynamicQuery, N'@Counter int output', @Counter output
	                   			IF NOT(@Counter >= @MaxLimit)
	                   				BEGIN
	                   					DECLARE @SessionId uniqueidentifier = @SessionIdInput
			                			DECLARE @ConfirmationCodeReplace uniqueidentifier = newid()
			                			DECLARE @LinkClaimMarkerLink varchar(max)
			                			SELECT @LinkClaimMarkerLink = Link FROM LinkShortners WHERE Id = @LinkId
					                	UPDATE Users SET ConfirmationCode = @ConfirmationCodeReplace, LinkClaimMarker = CAST(@LinkClaimCredit AS decimal(9,8)), LinkClaimMarkerLink = @LinkClaimMarkerLink, LinkClaimMarkerIP = @IP, LinkClaimMarkerId = @LinkId, LinkClaimFalseDateTime = DATEADD(SECOND, 10, getdate()) WHERE SessionId = @SessionId AND SessionExpiry > getdate()
					                	SELECT (SELECT Link FROM LinkShortners WHERE Id = @LinkId) AS ResultantLink,
					                				 (SELECT ConfirmationCode FROM Users WHERE SessionId = @SessionId) AS ConfirmationCode,
					                				 (SELECT 1) AS Available
					                	SET @Available = 1
					                	BREAK
	                   				END	                                
	                   			SET @RowNumber = @RowNumber + 1
	                   		END		
	                   	  END		
	                   	  IF(@Available = 0)		
	                   	  BEGIN		
	                   	  SELECT 0 AS Available		
	                   	  END		
                        END`)
                    }).then(result => {

                        if (result.recordset[0].Available === 0) {

                            result.recordset[0].ResultantLink = 'Not available';
                            result.recordset[0].ConfirmationCode = 'Unknown';
                            socket.emit('message', 'Faucet claims have been maxed out for today, please check back tomorrow');
                        }
                        else if (result.recordset[0].Available === 1) {

                            var linkShortner = result.recordset[0].ResultantLink;
                            var finalTarget = `claim.faucet4all.com?confirmationCode=${result.recordset[0].ConfirmationCode}`;

                            linkShortner = linkShortner.replace("{{}}", encodeURIComponent(finalTarget));

                            request.get(
                                linkShortner,
                                function (error, response, body) {

                                    result.recordset[0].ResultantLink = body;
                                    result.recordset[0].ConfirmationCode = 'Unknown';
                                    socket.emit('receivingClaimLink', result.recordset);
                                    sql.close();
                                })
                        }
                    }).catch(err => {
                        console.log(err);
                        socket.emit('error');
                        sql.close();
                    })
                    sql.on('error', err => {
                        console.log(err)
                        sql.close();
                    })

                }
                else if (body.includes("false")) {
                    socket.emit('captchaInvalid');
                }

            }
        );


    });

    //Send the appropriate PTP type (Normal/Invisible) also send signal for link validity
    socket.on('requestingPTPType', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("SourceLink", JSON.parse(data).referrer)
                .query(`
                                                   BEGIN

	                        DECLARE @PTPSourceLink varchar(max) = @SourceLink
	                        DECLARE @RandomNumber int
	                        DECLARE @PTPDirectRatio int
	                        DECLARE @PTPTotalRatio int
	                        SELECT @PTPDirectRatio = Value FROM Settings WHERE Name = 'PTPDirectRatio'
	                        SELECT @PTPTotalRatio = Value FROM Settings WHERE Name = 'PTPTotalRatio'
	                        SELECT @RandomNumber = FLOOR(RAND()*(@PTPTotalRatio-1+1))+1
	
	                        IF @PTPSourceLink = '' OR @PTPSourceLink IS NULL OR @PTPSourceLink = 'Undefined' OR EXISTS(SELECT Keyword FROM PTPBlacklist WHERE Keyword like '%' + @PTPSourceLink + '%')
		                        BEGIN
		                        SELECT '#/' AS Link, 'false' as Valid
		                        END
	                        ELSE IF(@RandomNumber <= @PTPDirectRatio)
		                        BEGIN
		                        SELECT 'Invisible' AS Link, 'true' as Valid
		                        --During development, set the above from true to false to ensure you only get rotator link
		                        END
	                        ELSE
		                        BEGIN
		                        SELECT '#/' AS Link, 'true' as Valid
		                        END
                        END`)
        }).then(result => {
            socket.emit('receivingPTPType', result.recordset);
            sql.close();

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('requestingPTPLink', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("Username", JSON.parse(data).username)
                .input("AbsoluteSourceLink", JSON.parse(data).absoluteReferrer)
                .input("SourceLink", JSON.parse(data).referrer)
                .query(`
                                     BEGIN
	                                    
	                                    DECLARE @CounterId int
	                                    SELECT @CounterId = Id FROM PTPCounter
	                                    DECLARE @MaxId int
	                                    SELECT @MaxId = max(Id) FROM PTP
	
	                                    IF((SELECT Count(Id) FROM PTPCounter) = 0)
		                                    BEGIN
		                                    INSERT INTO PTPCounter(Id) VALUES(0)
		                                    END	                                    

	                                    DECLARE @Link varchar(max)	                                    
	                                    SELECT @Link = Link FROM PTP WHERE Id = @CounterId
	                                    
		                                EXEC UpdatePTPSourceLinkAndHitsReceivedAndCounter @UsernameInput = @Username, @PTPSourceLinkInput = @SourceLink, @PTPAbsoluteSourceLinkInput = @AbsoluteSourceLink, @CounterIdInput = @CounterId

	                                    SELECT Link FROM PTP WHERE Link = @Link
                                    END`)
        }).then(result => {
            socket.emit('receivingPTPLink', result.recordset);
            sql.close();

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('claimingPTPCredit', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("IPAddress", JSON.parse(data).ip)
                .input("Referrer", JSON.parse(data).username)
                .input("Valid", JSON.parse(data).valid)
                .query(`
                                    BEGIN
	                                DECLARE @IP varchar(max) = @IPAddress
	                                DECLARE @Username varchar(max) = @Referrer
	                                DECLARE @AmountToCredit DECIMAL (9, 8)
	                                IF(@IP IS NULL OR @IP = '')
	                                BEGIN
	                                SET @IP = 'Unknown'
	                                END

	                                IF(@Valid != 'false')
	                                BEGIN

	                                IF NOT EXISTS(SELECT UniqueIP FROM PTPIP WHERE UniqueIP = @IP AND Username = @Username)
		                                BEGIN
		                                IF @Username IS NOT NULL AND @Username != '' INSERT INTO PTPIP(Username, UniqueIP) VALUES(@Username, @IP)
		                                SELECT @AmountToCredit = Value FROM Settings WHERE Name = 'PTPUniqueIPCredit'
		                                EXEC UpdatePTPBalance @UsernameInput = @Username, @AmountInput = @AmountToCredit
		                                UPDATE Users SET PTPUniqueEarning = PTPUniqueEarning + @AmountToCredit WHERE Username = @Username
		                                END
	                                ELSE IF NOT EXISTS(SELECT NonUniqueIP1 FROM PTPIP WHERE NonUniqueIP1 = @IP AND Username = @Username)
		                                BEGIN
		                                UPDATE PTPIP SET NonUniqueIP1 = @IP WHERE UniqueIP = @IP AND Username = @Username
		                                SELECT @AmountToCredit = Value FROM Settings WHERE Name = 'PTPNonUniqueIP1Credit'
		                                EXEC UpdatePTPBalance @UsernameInput = @Username, @AmountInput = @AmountToCredit
		                                UPDATE Users SET PTPNonUnique1Earning = PTPNonUnique1Earning + @AmountToCredit WHERE Username = @Username
		                                END
	                                ELSE IF NOT EXISTS(SELECT NonUniqueIP2 FROM PTPIP WHERE NonUniqueIP2 = @IP AND Username = @Username)
		                                BEGIN
		                                UPDATE PTPIP SET NonUniqueIP2 = @IP WHERE UniqueIP = @IP AND Username = @Username
		                                SELECT @AmountToCredit = Value FROM Settings WHERE Name = 'PTPNonUniqueIP2Credit'
		                                EXEC UpdatePTPBalance @UsernameInput = @Username, @AmountInput = @AmountToCredit
		                                UPDATE Users SET PTPNonUnique2Earning = PTPNonUnique2Earning + @AmountToCredit WHERE Username = @Username
		                                END	
	                                ELSE IF NOT EXISTS(SELECT NonUniqueIP3 FROM PTPIP WHERE NonUniqueIP3 = @IP AND Username = @Username)
		                                BEGIN
		                                UPDATE PTPIP SET NonUniqueIP3 = @IP WHERE UniqueIP = @IP AND Username = @Username
		                                SELECT @AmountToCredit = Value FROM Settings WHERE Name = 'PTPNonUniqueIP3Credit'
		                                EXEC UpdatePTPBalance @UsernameInput = @Username, @AmountInput = @AmountToCredit
		                                UPDATE Users SET PTPNonUnique3Earning = PTPNonUnique3Earning + @AmountToCredit WHERE Username = @Username
		                                END
		                             END
                                END`)
        }).then(result => {
            sql.close();

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('requestingDirectLink', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("UsernameInput", JSON.parse(data).username)
                .input("AbsoluteSourceLink", JSON.parse(data).absoluteReferrer)
                .input("SourceLink", JSON.parse(data).referrer)
                .query(`
                                     BEGIN

	                                    DECLARE @DirectAbsoluteSourceLink varchar(max) = @AbsoluteSourceLink
	                                    DECLARE @DirectSourceLink varchar(max) = @SourceLink
	                                    DECLARE @Username varchar(12) = @UsernameInput
	                                    DECLARE @CounterId int
	                                    SELECT @CounterId = Id FROM DirectCounter
	                                    DECLARE @MaxId int
	                                    SELECT @MaxId = max(Id) FROM Direct
	
	                                    IF(@CounterId IS NULL OR @CounterId = '')
		                                    BEGIN
		                                    INSERT INTO DirectCounter(Id) VALUES(0)
		                                    END
	                                    ELSE IF(@CounterId >= @MaxId)
		                                    BEGIN
		                                    UPDATE DirectCounter SET Id = 0
		                                    END
	                                    DECLARE @Link varchar(max)
	                                    SELECT @Link = Link FROM Direct WHERE Id = @CounterId

	                                    IF (@DirectSourceLink IS NULL OR @DirectSourceLink = '')
                                            BEGIN
			                                    SET @DirectSourceLink = 'Undefined'
		                                    END
                                        IF (@Username IS NULL OR @Username = '')
                                            BEGIN
			                                    SET @Username = 'Unknown'
		                                    END
                                        IF NOT EXISTS(SELECT site FROM DirectSource WHERE site = @DirectSourceLink AND Username = @Username)
                                            BEGIN
			                                    INSERT INTO DirectSource(site, Username) VALUES(@DirectSourceLink, @Username)
		                                    END

	                                    IF (@DirectAbsoluteSourceLink IS NULL OR @DirectAbsoluteSourceLink = '')
                                            BEGIN
			                                    SET @DirectAbsoluteSourceLink = 'Undefined'
		                                    END                                        
                                        IF NOT EXISTS(SELECT site FROM DirectAbsoluteSource WHERE site = @DirectAbsoluteSourceLink AND Username = @Username)
                                            BEGIN
			                                    INSERT INTO DirectAbsoluteSource(site, Username) VALUES(@DirectAbsoluteSourceLink, @Username)
		                                    END

	                                    UPDATE Direct SET HitsReceived += 1 WHERE Link = @Link
	                                    UPDATE DirectSource SET counter += 1 WHERE site = @DirectSourceLink AND Username = @Username
	                                    UPDATE DirectAbsoluteSource SET counter += 1 WHERE site = @DirectAbsoluteSourceLink AND Username = @Username
	                                    UPDATE DirectCounter SET Id += 1

	                                    SELECT SELECT Link FROM Direct WHERE Link = @Link AS Link
                                    END`)
        }).then(result => {
            socket.emit('receivingDirectLink', result.recordset);
            sql.close();

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('claimingDirectCredit', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("IPAddress", JSON.parse(data).ip)
                .input("Referrer", JSON.parse(data).username)
                .input("Valid", JSON.parse(data).valid)
                .query(`
                                     BEGIN
	                                DECLARE @IP varchar(max) = @IPAddress
	                                DECLARE @Username varchar(max) = @Referrer
	                                DECLARE @AmountToCredit DECIMAL (9, 8)

                                    IF(@IP IS NULL OR @IP = '')
	                                BEGIN
	                                SET @IP = 'Unknown'
	                                END

	                                IF(@Valid != 'false')
	                                BEGIN

	                                IF NOT EXISTS(SELECT UniqueIP FROM DirectIP WHERE UniqueIP = @IP AND Username = @Username)
		                                BEGIN
		                                IF @Username IS NOT NULL INSERT INTO DirectIP(Username, UniqueIP) VALUES(@Username, @IP)
		                                SELECT @AmountToCredit = Value FROM Settings WHERE Name = 'DirectUniqueIPCredit'
		                                EXEC UpdatePTPBalance @UsernameInput = @Username, @AmountInput = @AmountToCredit
		                                END
	                                ELSE IF NOT EXISTS(SELECT NonUniqueIP1 FROM DirectIP WHERE NonUniqueIP1 = @IP AND Username = @Username)
		                                BEGIN
		                                UPDATE DirectIP SET NonUniqueIP1 = @IP WHERE UniqueIP = @IP AND Username = @Username
		                                SELECT @AmountToCredit = Value FROM Settings WHERE Name = 'DirectNonUniqueIP1Credit'
		                                EXEC UpdatePTPBalance @UsernameInput = @Username, @AmountInput = @AmountToCredit
		                                END
	                                ELSE IF NOT EXISTS(SELECT NonUniqueIP2 FROM DirectIP WHERE NonUniqueIP2 = @IP AND Username = @Username)
		                                BEGIN
		                                UPDATE DirectIP SET NonUniqueIP2 = @IP WHERE UniqueIP = @IP AND Username = @Username
		                                SELECT @AmountToCredit = Value FROM Settings WHERE Name = 'DirectNonUniqueIP2Credit'
		                                EXEC UpdatePTPBalance @UsernameInput = @Username, @AmountInput = @AmountToCredit
		                                END	
	                                ELSE IF NOT EXISTS(SELECT NonUniqueIP3 FROM DirectIP WHERE NonUniqueIP3 = @IP AND Username = @Username)
		                                BEGIN
		                                UPDATE DirectIP SET NonUniqueIP3 = @IP WHERE UniqueIP = @IP AND Username = @Username
		                                SELECT @AmountToCredit = Value FROM Settings WHERE Name = 'DirectNonUniqueIP3Credit'
		                                EXEC UpdatePTPBalance @UsernameInput = @Username, @AmountInput = @AmountToCredit
		                                END
		                             END
                                END`)
        }).then(result => {
            sql.close();

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('requestingWithdrawal', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("SessionIdInput", JSON.parse(data).sessionId)
                .input("InputAmount", JSON.parse(data).amount)
                .input("Type", JSON.parse(data).type)
                .input("Address", JSON.parse(data).address)
                .query(`
                                    BEGIN
	                                    DECLARE @SessionId uniqueidentifier = @SessionIdInput
	                                    DECLARE @Amount DECIMAL(9, 8) = @InputAmount
	                                    DECLARE @WalletType varchar(50) = @Type
	                                    DECLARE @WalletAddress varchar(max) = @Address
	                                    DECLARE @Username varchar(10)
	                                    DECLARE @Balance DECIMAL(9, 8)
                                        DECLARE @MinimumWithdrawal DECIMAL(9, 8)
    

	                                    SELECT @Username = Username FROM Users WHERE SessionId = @SessionId AND  SessionExpiry > getdate()
	                                    SELECT @Balance = Balance FROM Users WHERE SessionId = @SessionId AND  SessionExpiry > getdate()
	                                    SELECT @MinimumWithdrawal = Value FROM Settings WHERE Name = 'MinimumWithdrawal'

	                                    IF @Amount IS NOT NULL AND @Amount <= @Balance AND @Amount >= @MinimumWithdrawal
	                                    AND @WalletType IS NOT NULL AND @WalletType != ''
	                                    AND @WalletAddress IS NOT NULL AND @WalletAddress != ''
	                                    AND @Username IS NOT NULL AND @Username != ''	
		                                    BEGIN
		                                        IF(@WalletType != 'FaucetHubBitcoin')
			                                    BEGIN
			                                    SET @Amount -= 0.00000400
			                                    END
			                                    INSERT INTO Withdrawal(Username, WithdrawalType, RequestedAmount, WalletType, WalletAddress) VALUES(@Username, 'Standard', @Amount, @WalletType, @WalletAddress)
			                                    UPDATE Users SET Balance -= @Amount WHERE Username = @Username
		                                    END
                                    END`)
        }).then(result => {

            var emailSubject = `Withdrawal request submitted`;
            var emailMessage = `
                                You have submitted a new withdrawal request
                                Please allow up to 1-5 days to receive your payment
                                Amount: ${JSON.parse(data).amount} Bitcoin
                                Wallet Type: ${JSON.parse(data).type}
                                Wallet Address: ${JSON.parse(data).address}`;

            request.get(
                sendEmailAPI,
                { qs: { key: sendEmailKey, usernameOrEmailOrSessionId: JSON.parse(data).sessionId, subject: emailSubject, message: emailMessage } },
                function (error, response, body) {

                    socket.emit('success');
                    sql.close();
                }
            );

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('requestingOfferwallWithdrawal', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("SessionIdInput", JSON.parse(data).sessionId)
                .input("InputAmount", JSON.parse(data).amount)
                .input("Type", JSON.parse(data).type)
                .input("Address", JSON.parse(data).address)
                .query(`
                                    BEGIN
	                                    DECLARE @SessionId uniqueidentifier = @SessionIdInput
	                                    DECLARE @Amount DECIMAL(9, 8) = @InputAmount
	                                    DECLARE @WalletType varchar(50) = @Type
	                                    DECLARE @WalletAddress varchar(max) = @Address
	                                    DECLARE @Username varchar(10)
	                                    DECLARE @Balance DECIMAL(9, 8)
                                        DECLARE @MinimumWithdrawal DECIMAL(9, 8)
    

	                                    SELECT @Username = Username FROM Users WHERE SessionId = @SessionId AND  SessionExpiry > getdate()
	                                    SELECT @Balance = OfferwallBalance FROM Users WHERE SessionId = @SessionId AND  SessionExpiry > getdate()
	                                    SELECT @MinimumWithdrawal = Value FROM Settings WHERE Name = 'MinimumOfferwallWithdrawal'

	                                    IF @Amount IS NOT NULL AND @Amount <= @Balance AND @Amount >= @MinimumWithdrawal
	                                    AND @WalletType IS NOT NULL AND @WalletType != ''
	                                    AND @WalletAddress IS NOT NULL AND @WalletAddress != ''
	                                    AND @Username IS NOT NULL AND @Username != ''	
		                                    BEGIN
			                                    IF(@WalletType != 'FaucetHubBitcoin')
			                                    BEGIN
			                                    SET @Amount -= 0.00000400
			                                    END
			                                    INSERT INTO Withdrawal(Username, WithdrawalType, RequestedAmount, WalletType, WalletAddress) VALUES(@Username, 'Offerwall', @Amount, @WalletType, @WalletAddress)
			                                    UPDATE Users SET OfferwallBalance = OfferwallBalance - @Amount WHERE Username = @Username
		                                    END
                                    END`)
        }).then(result => {

            var emailSubject = `Offerwall withdrawal request submitted`;
            var emailMessage = `
                                You have submitted a new offerwall withdrawal request
                                Please allow up to 1-5 days to receive your payment
                                Amount: ${JSON.parse(data).amount} Bitcoin
                                Wallet Type: ${JSON.parse(data).type}
                                Wallet Address: ${JSON.parse(data).address}`;

            request.get(
                sendEmailAPI,
                { qs: { key: sendEmailKey, usernameOrEmailOrSessionId: JSON.parse(data).sessionId, subject: emailSubject, message: emailMessage } },
                function (error, response, body) {

                    socket.emit('success');
                    sql.close();
                }
            );

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('requestingSupportTickets', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("SessionIdInput", JSON.parse(data).sessionId)
                .query(`
                                    BEGIN
                                    DECLARE @SessionId uniqueidentifier = @SessionIdInput
                                    DECLARE @Username varchar(10)

                                    SELECT @Username = Username FROM Users WHERE SessionId = @SessionId

                                    SELECT Reference, Subject, DateTime, Locked, LastReplier FROM SupportTickets WHERE Username = @Username ORDER BY DateTime DESC
                                    END`)
        }).then(result => {
            socket.emit('receivingSupportTickets', result.recordset);
            sql.close();

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('createSupportTicket', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("SessionIdInput", JSON.parse(data).sessionId)
                .input("SubjectInput", JSON.parse(data).subject)
                .input("MessageInput", JSON.parse(data).message)
                .query(`
                                    BEGIN
                                    DECLARE @SessionId uniqueidentifier = @SessionIdInput
                                    DECLARE @Username varchar(10)
                                    DECLARE @Email varchar(150)

                                    SELECT @Username = Username FROM Users WHERE SessionId = @SessionId
                                    SELECT @Email = Email FROM Users WHERE SessionId = @SessionId

                                    DECLARE @Subject varchar(max) = @SubjectInput
                                    DECLARE @Message varchar(max) = @MessageInput
                                    DECLARE @ReferenceId uniqueidentifier = newid()

                                    IF LEN(@Subject) <= 70 AND @Username IS NOT NULL AND @Email IS NOT NULL AND @Username != '' AND @Email != ''
	                                    BEGIN
	                                    INSERT INTO SupportTickets(Reference, Subject, Message, Username, Email, LastReplier) VALUES(@ReferenceId, @Subject, @Message, @Username, @Email, @Username)
	                                    END                                    
	                                SELECT @ReferenceId AS Reference                                   
                                    END`)
        }).then(result => {

            var emailSubject = `New support ticket created with reference ${result.recordset[0].Reference}`;
            var emailMessage = `
                                You have created a new support ticket with reference ${result.recordset[0].Reference}
                                You will receive a response within the next 48 hours
                                Your subject:
                                ${JSON.parse(data).subject}
                                Your message:
                                ${JSON.parse(data).message}`;

            request.get(
                sendEmailAPI,
                { qs: { key: sendEmailKey, usernameOrEmailOrSessionId: JSON.parse(data).sessionId, subject: emailSubject, message: emailMessage } },
                function (error, response, body) {

                    socket.emit('createdSupportTicket', result.recordset);
                    socket.emit('success');
                    sql.close();
                }
            );

        }).catch(err => {
            socket.emit('error');
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
    });

    socket.on('requestingSupportTicketReplies', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("ReferenceInput", JSON.parse(data).reference)
                .query(`
                                    BEGIN
                                    DECLARE @Reference uniqueidentifier = @ReferenceInput

                                    SELECT Reply, DateTime, Username FROM SupportTicketsReply WHERE Reference = @Reference ORDER BY DateTime DESC
                                    END`)
        }).then(result => {
            socket.emit('receivingSupportTicketReplies', result.recordset);
            sql.close();

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('requestingSupportTicket', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("SessionIdInput", JSON.parse(data).sessionId)
                .input("ReferenceInput", JSON.parse(data).reference)
                .query(`
                                    BEGIN
                                    DECLARE @SessionId uniqueidentifier = @SessionIdInput
                                    DECLARE @Reference uniqueidentifier = @ReferenceInput
                                    DECLARE @Username varchar(10)

                                    SELECT @Username = Username FROM Users WHERE SessionId = @SessionId

                                    SELECT Subject, Message, DateTime, Locked FROM SupportTickets WHERE Username = @Username AND Reference = @Reference ORDER BY DateTime DESC
                                    END`)
        }).then(result => {
            socket.emit('receivingSupportTicket', result.recordset);
            sql.close();

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('replyingSupportTicket', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("SessionIdInput", JSON.parse(data).sessionId)
                .input("ReferenceInput", JSON.parse(data).reference)
                .input("ReplyMessageInput", JSON.parse(data).reply)
                .query(`
                                    BEGIN
                                    DECLARE @SessionId uniqueidentifier = @SessionIdInput
                                    DECLARE @Username varchar(10)
                                    DECLARE @Reference uniqueidentifier = @ReferenceInput
                                    DECLARE @ReplyMessage varchar(max) = @ReplyMessageInput
                                    DECLARE @Locked bit

                                    SELECT @Username = Username FROM Users WHERE SessionId = @SessionId
                                    SELECT @Locked = Locked FROM SupportTickets WHERE Reference = @Reference

                                    IF @Locked = 'false' AND @ReplyMessage IS NOT NULL AND @ReplyMessage != '' AND @Username IS NOT NULL AND @Username != ''
                                    BEGIN
                                    INSERT INTO SupportTicketsReply(Reference, Username, Reply) VALUES(@Reference, @Username, @ReplyMessage)
                                    UPDATE SupportTickets SET LastReplier = @Username WHERE Reference = @Reference
                                    END
                                    SELECT @Username AS Username
                                    END`)
        }).then(result => {

            var emailSubject = `Support ticket reply referenced ${JSON.parse(data).reference}`;
            var emailMessage = `
                                ${JSON.parse(data).reply}
                                Replier: ${result.recordset[0].Username}`;

            request.get(
                sendEmailAPI,
                { qs: { key: sendEmailKey, usernameOrEmailOrSessionId: JSON.parse(data).sessionId, subject: emailSubject, message: emailMessage } },
                function (error, response, body) {

                    socket.emit('repliedSupportTicket');
                    socket.emit('success');
                    sql.close();
                }
            );

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('requestingMiningPercentage', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .query(`
                                    BEGIN
                                    SELECT Percentage FROM Mining                                    
                                    END`)
        }).then(result => {
            socket.emit('receivingMiningPercentage', result.recordset);
            sql.close();

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('purchasingAdvertise', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("SessionIdInput", JSON.parse(data).sessionId)
                .input("Reference", JSON.parse(data).reference)
                .query(`
                                    BEGIN
                                    DECLARE @SessionId uniqueidentifier = @SessionIdInput
                                    DECLARE @PurchaseBalance DECIMAL (9, 8)
                                    DECLARE @Username varchar(12)
                                    DECLARE @Name varchar(20)
                                    DECLARE @IsTimeBased bit
                                    DECLARE @Credit int
                                    DECLARE @Price DECIMAL (9, 8)

                                    SELECT @Username = Username, @PurchaseBalance = PurchaseBalance FROM Users WHERE SessionId = @SessionId AND SessionExpiry > getdate()
                                    SELECT @Name = Name, @IsTimeBased = IsTimeBased, @Credit = Credit, @Price = Price FROM Advertise WHERE Reference = @Reference

                                    IF(@PurchaseBalance >= @Price)
                                    BEGIN
                                    INSERT INTO OrderHistory(Username, Name, IsTimeBased, Credit, Price) VALUES(@Username, @Name, @IsTimeBased, @Credit, @Price)
                                    UPDATE Users SET PurchaseBalance -= @Price WHERE Username = @Username

                                    IF(@Name = 'PTP' AND @IsTimeBased = 0)
                                    BEGIN
                                    UPDATE Users SET PTPCredit += @Credit WHERE Username = @Username
                                    END
                                    ELSE IF(@Name = 'PTP' AND @IsTimeBased = 1)
                                    BEGIN
                                    UPDATE Users SET PTPDayCredit += @Credit WHERE Username = @Username
                                    END
                                    ELSE IF(@Name = 'Banner' AND @IsTimeBased = 0)
                                    BEGIN
                                    UPDATE Users SET BannerCredit += @Credit WHERE Username = @Username
                                    END
                                    ELSE IF(@Name = 'Banner' AND @IsTimeBased = 1)
                                    BEGIN
                                    UPDATE Users SET BannerDayCredit += @Credit WHERE Username = @Username
                                    END

                                    END
                                    END`)
        }).then(result => {

            socket.emit('success');
            socket.emit('dataIsOutdated');
            sql.close();

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('changeJustClaimed', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("SessionIdInput", JSON.parse(data).sessionId)
                .input("JustClaimed", JSON.parse(data).justClaimed)
                .query(`
                                    BEGIN
                                    DECLARE @SessionId uniqueidentifier = @SessionIdInput
                                    UPDATE Users SET JustClaimed = @JustClaimed WHERE SessionId = @SessionId AND SessionExpiry > getdate()
                                    END`)
        }).then(result => {

            sql.close();

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('requestingDepositMethod', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .query(`SELECT Name, Address FROM DepositMethod`)
        }).then(result => {


            //for (var i = 0; i < Object.keys(result.recordset).length; i++) {

            //    if (result.recordset[i].Name === 'Bitcoin') {

            //        const gateway = new BitcoinGateway(result.recordset[i].Address, 'abc');

            //        gateway.createAddress('5554555')
            //            .then((address) => {

            //                result.recordset[i].Address = address.address;

            //            }).catch((error) => {

            //                result.recordset[i].Address = 'Error';
            //                console.log(error);
            //            })
            //    }

            //}
            socket.emit('receivingDepositMethod', result.recordset);
            sql.close();

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('newChatMessage', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("SessionIdInput", JSON.parse(data).sessionId)
                .input("Message", JSON.parse(data).message)
                .query(`
                                    BEGIN
                                    DECLARE @SessionId uniqueidentifier = @SessionIdInput                                    
                                    DECLARE @Username varchar(10)                                    
                                    SELECT @Username = Username FROM Users WHERE SessionId = @SessionId
                                    IF(@Message IS NOT NULL AND @Message != '')
                                    BEGIN
                                    INSERT INTO ChatHistory(Username, Message) VALUES(@Username, @Message)
                                    SELECT @Username AS Username, getdate() AS DateTime, @Message AS Message
                                    END
                                    END`)
        }).then(result => {

            io.emit('receivingChatMessage', result.recordset);
            sql.close();

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('banChatUser', function (data) {

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("SessionId", JSON.parse(data).sessionId)
                .query(`
                                    BEGIN
                                    UPDATE Users SET ChatBanned = 1 WHERE SessionId = @SessionId AND SessionExpiry > getdate()
                                    END`)
        }).then(result => {

            socket.emit('dataIsOutdated');
            sql.close();

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('updateAllUserData', function (data) {

        if (data === outdatedSocketKey)
            io.emit('dataIsOutdated');

    });

    socket.on('placingAdvertisingOrder', function (data) {

        var SessionIdInput = 'f50abe9e-d91d-4f85-8c54-06db10095ecf';
        var Name = 'PTP';
        var Link = 'http://faucet4all.com';
        var ImageLink = 'http://kingbtc.co/banners/banner1.gif';
        var TargetLink = 'http://kingbtc.co/?ref=admin';
        var IsTimeBased = false;
        var CreditInput = 0;
        var Title = 'Bonus Ad';
        var Description = 'Click to view the Bonus Ad';

        var jsonData = JSON.parse(data);

        if (jsonData.hasOwnProperty('sessionId')) SessionIdInput = jsonData.sessionId;
        if (jsonData.hasOwnProperty('name')) Name = jsonData.name;
        if (jsonData.hasOwnProperty('link')) Link = jsonData.link;
        if (jsonData.hasOwnProperty('imageLink')) ImageLink = jsonData.imageLink;
        if (jsonData.hasOwnProperty('targetLink')) TargetLink = jsonData.targetLink;
        if (jsonData.hasOwnProperty('isTimeBasedInput')) IsTimeBased = jsonData.isTimeBasedInput;
        if (jsonData.hasOwnProperty('creditInput')) CreditInput = jsonData.creditInput;
        if (jsonData.hasOwnProperty('title')) Title = jsonData.title;
        if (jsonData.hasOwnProperty('description')) Description = jsonData.description;


        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("SessionIdInput", SessionIdInput)
                .input("Name", Name)
                .input("Link", Link)
                .input("ImageLink", ImageLink)
                .input("TargetLink", TargetLink)
                .input("IsTimeBased", IsTimeBased)
                .input("CreditInput", CreditInput)
                .input("Title", Title)
                .input("Description", Description)
                .query(`
                                    BEGIN
                                    DECLARE @SessionId uniqueidentifier = @SessionIdInput
                                    DECLARE @Username varchar(12)
                                    DECLARE @Credit int = @CreditInput
                                    DECLARE @AccountCredit int
                                    DECLARE @Id int

                                    SELECT @Username = Username FROM Users WHERE SessionId = @SessionId AND SessionExpiry > getdate()                                    

                                    IF(@Name = 'PTP' AND @IsTimeBased = 0)
                                    BEGIN                                  
                                    SELECT @AccountCredit = PTPCredit FROM Users WHERE Username = @Username
                                    IF(@AccountCredit >= @Credit AND @Credit >= 0)
                                    SELECT @Id = max(Id)+1 FROM PTP
                                    INSERT INTO PTP(Id, Username, Link, Credit, IsTimeBased) VALUES(@Id, @Username, @Link, @Credit, @IsTimeBased)
                                    UPDATE Users SET PTPCredit -= @Credit WHERE Username = @Username
                                    END
                                    ELSE IF(@Name = 'PTP' AND @IsTimeBased = 1)
                                    BEGIN
                                    SELECT @AccountCredit = PTPDayCredit FROM Users WHERE Username = @Username
                                    IF(@AccountCredit >= @Credit AND @Credit >= 0)
                                    SELECT @Id = max(Id)+1 FROM PTP
                                    INSERT INTO PTP(Id, Username, Link, Credit, IsTimeBased) VALUES(@Id, @Username, @Link, @Credit, @IsTimeBased)
                                    UPDATE Users SET PTPDayCredit -= @Credit WHERE Username = @Username
                                    END
                                    ELSE IF(@Name = 'Banner' AND @IsTimeBased = 0)
                                    BEGIN                                  
                                    SELECT @AccountCredit = BannerCredit FROM Users WHERE Username = @Username
                                    IF(@AccountCredit >= @Credit AND @Credit >= 0)
                                    SELECT @Id = max(Id)+1 FROM BannerRotator
                                    INSERT INTO BannerRotator(Id, Username, ImageLink, TargetLink, Credit, IsTimeBased) VALUES(@Id, @Username, @ImageLink, @TargetLink, @Credit, @IsTimeBased)
                                    UPDATE Users SET BannerCredit -= @Credit WHERE Username = @Username
                                    END
                                    ELSE IF(@Name = 'Banner' AND @IsTimeBased = 1)
                                    BEGIN
                                    SELECT @AccountCredit = BannerDayCredit FROM Users WHERE Username = @Username
                                    IF(@AccountCredit >= @Credit AND @Credit >= 0)
                                    SELECT @Id = max(Id)+1 FROM BannerRotator
                                    INSERT INTO BannerRotator(Id, Username, ImageLink, TargetLink, Credit, IsTimeBased) VALUES(@Id, @Username, @ImageLink, @TargetLink, @Credit, @IsTimeBased)
                                    UPDATE Users SET BannerDayCredit -= @Credit WHERE Username = @Username
                                    END
                                    ELSE IF(@Name = 'SquareBanner' AND @IsTimeBased = 0)
                                    BEGIN
                                    SELECT @AccountCredit = BannerCredit FROM Users WHERE Username = @Username
                                    IF(@AccountCredit >= @Credit AND @Credit >= 0)
                                    SELECT @Id = max(Id)+1 FROM SquareBannerRotator
                                    INSERT INTO SquareBannerRotator(Id, Username, ImageLink, TargetLink, Credit, IsTimeBased) VALUES(@Id, @Username, @ImageLink, @TargetLink, @Credit, @IsTimeBased)
                                    UPDATE Users SET BannerCredit -= @Credit WHERE Username = @Username
                                    END
                                    ELSE IF(@Name = 'SquareBanner' AND @IsTimeBased = 1)
                                    BEGIN
                                    SELECT @AccountCredit = BannerDayCredit FROM Users WHERE Username = @Username
                                    IF(@AccountCredit >= @Credit AND @Credit >= 0)
                                    SELECT @Id = max(Id)+1 FROM SquareBannerRotator
                                    INSERT INTO SquareBannerRotator(Id, Username, ImageLink, TargetLink, Credit, IsTimeBased) VALUES(@Id, @Username, @ImageLink, @TargetLink, @Credit, @IsTimeBased)
                                    UPDATE Users SET BannerDayCredit -= @Credit WHERE Username = @Username
                                    END
                                    ELSE IF(@Name = 'BonusAd' AND @IsTimeBased = 0)
                                    BEGIN
                                    SELECT @AccountCredit = BonusAdCredit FROM Users WHERE Username = @Username
                                    IF(@AccountCredit >= @Credit AND @Credit >= 0)
                                    INSERT INTO BonusAds(Username, Link, Credit, IsTimeBased, Title, Description) VALUES(@Username, @Link, @Credit, @IsTimeBased, @Title, @Description)
                                    UPDATE Users SET BonusAdCredit -= @Credit WHERE Username = @Username
                                    END
                                    ELSE IF(@Name = 'BonusAd' AND @IsTimeBased = 1)
                                    BEGIN
                                    SELECT @AccountCredit = BonusAdDayCredit FROM Users WHERE Username = @Username
                                    IF(@AccountCredit >= @Credit AND @Credit >= 0)
                                    INSERT INTO BonusAds(Username, Link, Credit, IsTimeBased, Title, Description) VALUES(@Username, @Link, @Credit, @IsTimeBased, @Title, @Description)
                                    UPDATE Users SET BonusAdDayCredit -= @Credit WHERE Username = @Username
                                    END

                                    END`)
        }).then(result => {

            socket.emit('success');
            socket.emit('dataIsOutdated');
            sql.close();

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('updatingAdvertisingOrder', function (data) {

        var SessionIdInput = 'f50abe9e-d91d-4f85-8c54-06db10095ecf';
        var Reference = 'f50abe9e-d91d-4f85-8c54-06db10095ecf';
        var Name = 'PTP';
        var Link = 'http://faucet4all.com';
        var ImageLink = 'http://kingbtc.co/banners/banner1.gif';
        var TargetLink = 'http://kingbtc.co/?ref=admin';
        var IsTimeBased = false;
        var CreditInput = 0;
        var Title = 'Bonus Ad';
        var Description = 'Click to view the Bonus Ad';

        var jsonData = JSON.parse(data);

        if (jsonData.hasOwnProperty('sessionId')) SessionIdInput = jsonData.sessionId;
        if (jsonData.hasOwnProperty('reference')) Reference = jsonData.reference;
        if (jsonData.hasOwnProperty('name')) Name = jsonData.name;
        if (jsonData.hasOwnProperty('link')) Link = jsonData.link;
        if (jsonData.hasOwnProperty('imageLink')) ImageLink = jsonData.imageLink;
        if (jsonData.hasOwnProperty('targetLink')) TargetLink = jsonData.targetLink;
        if (jsonData.hasOwnProperty('isTimeBasedInput')) IsTimeBased = jsonData.isTimeBasedInput;
        if (jsonData.hasOwnProperty('creditInput')) CreditInput = jsonData.creditInput;
        if (jsonData.hasOwnProperty('title')) Title = jsonData.title;
        if (jsonData.hasOwnProperty('description')) Description = jsonData.description;

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("SessionIdInput", SessionIdInput)
                .input("Reference", Reference)
                .input("Name", Name)
                .input("Link", Link)
                .input("ImageLink", ImageLink)
                .input("TargetLink", TargetLink)
                .input("IsTimeBased", IsTimeBased)
                .input("CreditInput", CreditInput)
                .input("Title", Title)
                .input("Description", Description)
                .query(`
                                    BEGIN
                                    DECLARE @SessionId uniqueidentifier = @SessionIdInput
                                    DECLARE @Username varchar(12)
                                    DECLARE @Credit int = @CreditInput
                                    DECLARE @AccountCredit int
                                    DECLARE @Id int


                                    SELECT @Username = Username FROM Users WHERE SessionId = @SessionId AND SessionExpiry > getdate()

                                    IF(@Name = 'PTP' AND @IsTimeBased = 0)
                                    BEGIN                                  
                                    SELECT @AccountCredit = PTPCredit FROM Users WHERE Username = @Username
                                    IF(@AccountCredit >= @Credit AND @Credit >= 0)
                                    UPDATE PTP SET Credit += @Credit, Link = @Link WHERE Reference = @Reference AND Username = @Username AND IsTimeBased = @IsTimeBased
                                    UPDATE Users SET PTPCredit -= @Credit WHERE Username = @Username
                                    END
                                    ELSE IF(@Name = 'PTP' AND @IsTimeBased = 1)
                                    BEGIN
                                    SELECT @AccountCredit = PTPDayCredit FROM Users WHERE Username = @Username
                                    IF(@AccountCredit >= @Credit AND @Credit >= 0)
                                    UPDATE PTP SET Link = @Link WHERE Reference = @Reference AND Username = @Username AND IsTimeBased = @IsTimeBased
                                    END
                                    ELSE IF(@Name = 'Banner' AND @IsTimeBased = 0)
                                    BEGIN
                                    SELECT @AccountCredit = BannerCredit FROM Users WHERE Username = @Username
                                    IF(@AccountCredit >= @Credit AND @Credit >= 0)
                                    UPDATE BannerRotator SET Credit += @Credit, ImageLink = @ImageLink, TargetLink = @TargetLink WHERE Reference = @Reference AND Username = @Username AND IsTimeBased = @IsTimeBased
                                    UPDATE Users SET BannerCredit -= @Credit WHERE Username = @Username
                                    END
                                    ELSE IF(@Name = 'Banner' AND @IsTimeBased = 1)
                                    BEGIN
                                    SELECT @AccountCredit = BannerDayCredit FROM Users WHERE Username = @Username
                                    IF(@AccountCredit >= @Credit AND @Credit >= 0)
                                    UPDATE BannerRotator SET ImageLink = @ImageLink, TargetLink = @TargetLink WHERE Reference = @Reference AND Username = @Username AND IsTimeBased = @IsTimeBased
                                    END
                                    ELSE IF(@Name = 'SquareBanner' AND @IsTimeBased = 0)
                                    BEGIN
                                    SELECT @AccountCredit = BannerCredit FROM Users WHERE Username = @Username
                                    IF(@AccountCredit >= @Credit AND @Credit >= 0)
                                    UPDATE SquareBannerRotator SET Credit += @Credit, ImageLink = @ImageLink, TargetLink = @TargetLink WHERE Reference = @Reference AND Username = @Username AND IsTimeBased = @IsTimeBased
                                    UPDATE Users SET BannerCredit -= @Credit WHERE Username = @Username
                                    END
                                    ELSE IF(@Name = 'SquareBanner' AND @IsTimeBased = 1)
                                    BEGIN
                                    SELECT @AccountCredit = BannerDayCredit FROM Users WHERE Username = @Username
                                    IF(@AccountCredit >= @Credit AND @Credit >= 0)
                                    UPDATE SquareBannerRotator SET ImageLink = @ImageLink, TargetLink = @TargetLink WHERE Reference = @Reference AND Username = @Username AND IsTimeBased = @IsTimeBased
                                    END
                                    ELSE IF(@Name = 'BonusAd' AND @IsTimeBased = 0)
                                    BEGIN
                                    SELECT @AccountCredit = BonusAdCredit FROM Users WHERE Username = @Username
                                    IF(@AccountCredit >= @Credit AND @Credit >= 0)
                                    UPDATE BonusAds SET Credit += @Credit, Link = @Link WHERE Id = @Reference AND Username = @Username AND IsTimeBased = @IsTimeBased
                                    UPDATE Users SET BonusAdCredit -= @Credit WHERE Username = @Username
                                    END
                                    ELSE IF(@Name = 'BonusAd' AND @IsTimeBased = 1)
                                    BEGIN
                                    SELECT @AccountCredit = BonusAdDayCredit FROM Users WHERE Username = @Username
                                    IF(@AccountCredit >= @Credit AND @Credit >= 0)
                                    UPDATE BonusAds SET Link = @Link WHERE Id = @Reference AND Username = @Username AND IsTimeBased = @IsTimeBased
                                    END

                                    END`)
        }).then(result => {

            socket.emit('success');
            socket.emit('dataIsOutdated');
            sql.close();

        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })
    });

    socket.on('requestingTestSocket', function (data) {



    });

    socket.on('claimingBonusAd', function (data) {

        //First insert his username in databse if not already

        new sql.ConnectionPool(config).connect().then(pool => {
            return pool.request()
                .input("IPAddress", JSON.parse(data).ip)
                .input("SessionIdInput", JSON.parse(data).sessionId)
                .input("Id", JSON.parse(data).id)
                .input("SocketSession", socket.id)
                .query(`
                    BEGIN
                        DECLARE @Available bit = 0
                        DECLARE @Username varchar(10)
                        DECLARE @SessionId uniqueidentifier = TRY_CONVERT(UNIQUEIDENTIFIER, @SessionIdInput)
                        DECLARE @BonusAdConfirmationCode uniqueidentifier = newid()
                        DECLARE @BonusAdMarkerAmount decimal(9, 8) = 0
                        SELECT @Username = Username FROM Users WHERE SessionId = @SessionId AND SessionExpiry > getdate()
                        SELECT @BonusAdMarkerAmount = Value FROM Settings WHERE Name = 'BonusAdCredit'

                        IF NOT EXISTS(SELECT Username FROM BonusAdsRecord WHERE Username = @Username)
                            BEGIN
	                        INSERT INTO BonusAdsRecord(Username) VALUES(@Username)
	                        END
                        DECLARE @IP varchar(20) = @IPAddress
                        IF NOT EXISTS(SELECT IP FROM LinkShortnersRecord WHERE IP = @IP)
                            BEGIN
	                        INSERT INTO LinkShortnersRecord(IP) VALUES(@IP)
                        END

                        DECLARE @TotalLinks int
                        SELECT @TotalLinks = COUNT(*) FROM LinkShortners
                        DECLARE @RowNumber int = 1
                        WHILE(@RowNumber <= @TotalLinks)
	                        BEGIN
	                            DECLARE @LinkId uniqueidentifier
	                            WITH temporaryTable AS (SELECT (ROW_NUMBER() OVER (ORDER BY LinkShortners.Limit)) as row,* FROM LinkShortners)
	                            SELECT @LinkId = Id FROM temporaryTable WHERE row = @RowNumber

	                            DECLARE @LinkIdString varchar(60) = convert(varchar(60), @LinkId)
	                            DECLARE @Counter int 
	                            DECLARE @MaxLimit int
	                            Select @MaxLimit = Limit from LinkShortners where Id = @LinkId
	                   			
	                            DECLARE @dynamicQuery nvarchar(max) = 'SELECT @Counter = [' + @LinkIdString + '] FROM LinkShortnersRecord WHERE IP = ''' + @IP + ''''
	                            EXEC sp_executeSQl @dynamicQuery, N'@Counter int output', @Counter output
	                            IF NOT(@Counter >= @MaxLimit)
	                                BEGIN	            
			    
				                        UPDATE Users SET BonusAdConfirmationCode = @BonusAdConfirmationCode, BonusAdMarkerIP = @IP, BonusAdMarkerId = @Id, BonusAdLinkMarkerId = @LinkId, BonusAdLinkFalseDateTime = DATEADD(SECOND, 10, getdate()), BonusAdMarkerAmount = @BonusAdMarkerAmount, SocketSession = @SocketSession WHERE SessionId = @SessionId AND SessionExpiry > getdate()
				                        SELECT (SELECT Link FROM LinkShortners WHERE Id = @LinkId) AS ResultantLink,
					                                    (SELECT BonusAdConfirmationCode FROM Users WHERE SessionId = @SessionId AND SessionExpiry > getdate()) AS ConfirmationCode,
					                                    (SELECT 1) AS Available
				                        SET @Available = 1
				                        BREAK
	                                END	                                
	                            SET @RowNumber += 1
	                        END		
	                        IF(@Available = 0)		
	                        BEGIN		
				            UPDATE Users SET BonusAdConfirmationCode = @BonusAdConfirmationCode, BonusAdMarkerIP = @IP, BonusAdMarkerId = @Id, BonusAdMarkerAmount = @BonusAdMarkerAmount, SocketSession = @SocketSession WHERE SessionId = @SessionId AND SessionExpiry > getdate()
	                        SELECT (SELECT 'Unknown') AS ResultantLink,
		                           (SELECT BonusAdConfirmationCode FROM Users WHERE SessionId = @SessionId AND SessionExpiry > getdate()) AS ConfirmationCode,
		                           (SELECT 0) AS Available
	                        END		
                        END`)
        }).then(result => {

            if (result.recordset[0].Available === 0) {

                //Direct claim in case no more link shortners available for today
                var encodedCode = encodeURIComponent(result.recordset[0].ConfirmationCode);
                result.recordset[0].ResultantLink = `http://claim.faucet4all.com/BonusAd?confirmationCode=${encodedCode}&direct=true`;
                result.recordset[0].ConfirmationCode = 'Unknown';
                socket.emit('receivingBonusAd', result.recordset);
            }
            else if (result.recordset[0].Available === 1) {

                //Redirect towards link shortner which will redirect towards the advertiser's link
                var linkShortner = result.recordset[0].ResultantLink;
                var finalTarget = `claim.faucet4all.com/BonusAd?confirmationCode=${result.recordset[0].ConfirmationCode}&direct=false`;

                linkShortner = linkShortner.replace("{{}}", encodeURIComponent(finalTarget));

                request.get(
                    linkShortner,
                    function (error, response, body) {

                        result.recordset[0].ResultantLink = body;
                        result.recordset[0].ConfirmationCode = 'Unknown';
                        socket.emit('receivingBonusAd', result.recordset);
                    })
            }
            sql.close();
        }).catch(err => {
            console.log(err);
            socket.emit('error');
            sql.close();
        })
        sql.on('error', err => {
            console.log(err)
            sql.close();
        })


    });

    socket.on('updateParticularUserData', function (data) {

        if (data[0].Key === claimDomainKey)
            io.to(`${data[0].SocketSession}`).emit('dataIsOutdated');

    });

    socket.on('sendParticularUserMessage', function (data) {

        if (data[0].Key === claimDomainKey)
            io.to(`${data[0].SocketSession}`).emit('message', data[0].Message);

    });

});
http.listen(3000, function () {
    console.log('listening on http://localhost:3000 ' + moment(Date.now()).format('YYYY-MM-DD - hh:mm:ss A'));
});