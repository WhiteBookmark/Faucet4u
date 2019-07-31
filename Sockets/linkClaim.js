var cookieParser = require("cookie-parser");
const sql = require("mssql");
var cors = require("cors");
var io = require("socket.io-client");
var socket = io.connect("http://127.0.0.1:3000", { reconnect: false });
var app = require("express")();
var http = require("http").Server(app);
var moment = require("moment");

app.use(cookieParser());

const particularOutdatedSocketKey = "P46$SP*V7Qrz";

var allowedOrigins = [
  "http://localhost:8081",
  "http://localhost:8080",
  "http://localhost:3000"
];

app.use(
  cors({
    origin: function(origin, callback) {
      // allow requests with no origin
      // (like mobile apps or curl requests)
      if (!origin) return callback(null, true);
      if (allowedOrigins.indexOf(origin) === -1) {
        var msg =
          "The CORS policy for this site does not " +
          "allow access from the specified Origin.";
        return callback(new Error(msg), false);
      }
      return callback(null, true);
    }
  })
);

//MS Sql connection string configuration

const config = {
  user: "sa",
  password: "123",
  server: "localhost",
  database: "Faucet4u"
};

app.get("/", function(req, res) {
  new sql.ConnectionPool(config)
    .connect()
    .then(pool => {
      return pool
        .request()
        .input("ConfirmationCodeInput", req.query.confirmationCode)
        .input("LinkClaimMarkerLinkInput", req.headers.referer).query(`BEGIN
                    DECLARE @Username varchar(10)
                    DECLARE @ConfirmationCode uniqueidentifier = TRY_CONVERT(UNIQUEIDENTIFIER, @ConfirmationCodeInput)
                    DECLARE @LinkClaimMarker decimal(9, 8)
                    DECLARE @LinkClaimMarkerLink varchar(max) = @LinkClaimMarkerLinkInput
                    DECLARE @LinkClaimMarkerLinkOriginal varchar(max)
                    DECLARE @LinkClaimMinutes int
                    DECLARE @LinkClaimInterval datetime
                    DECLARE @LinkClaimFalseDateTime datetime = getdate()
                    DECLARE @LinkShortnerId varchar(max)
                    DECLARE @LinkClaimMarkerIP varchar(40)
                    DECLARE @LinkClaimMarkerId varchar(60)
                    DECLARE @dynamicQuery nvarchar(max)

                    SELECT @Username = Username FROM Users WHERE ConfirmationCode = @ConfirmationCode
                    SELECT @LinkClaimMarker = LinkClaimMarker FROM Users WHERE ConfirmationCode = @ConfirmationCode                    
                    SELECT @LinkClaimMarkerLinkOriginal = LinkClaimMarkerLink FROM Users WHERE ConfirmationCode = @ConfirmationCode
                    SELECT @LinkClaimMinutes = Value FROM Settings WHERE Name = 'LinkClaimMinutes'
                    SELECT @LinkClaimInterval = LinkClaimInterval FROM Users WHERE ConfirmationCode = @ConfirmationCode
                    SELECT @LinkShortnerId = LinkClaimInterval, @LinkClaimFalseDateTime = LinkClaimFalseDateTime FROM Users WHERE ConfirmationCode = @ConfirmationCode

                    --IF(@LinkClaimInterval > getdate())
                    --BEGIN
                    --UPDATE Users SET LinkShortnerEarning = LinkShortnerEarning + @LinkClaimMarker, Balance = Balance + LinkClaimMarker, LinkClaimMarker = 0.00000000, LastClaim = getdate(), LinkClaimInterval = DATEADD(minute, @LinkClaimMinutes, getdate()), JustClaimed = 'true' WHERE ConfirmationCode = @ConfirmationCode
                    --EXEC InsertCheatHistory @UsernameInput = @Username, @Case = 1
                    --END
                    
                    --ELSE IF(@LinkClaimInterval < getdate())
                    IF(@ConfirmationCode IS NOT NULL AND @LinkClaimMarker > 0)
                    BEGIN
                    IF (@LinkClaimFalseDateTime > getdate())
                    BEGIN
                    GOTO False_Request
                    END  
                    EXEC UpdateBalance @UsernameInput = @Username, @AmountInput = @LinkClaimMarker
                    INSERT INTO LinkShortnersHistory(Username, Amount, Link) VALUES(@Username, @LinkClaimMarker, @LinkClaimMarkerLinkOriginal)
                    DELETE FROM LinkShortnersHistory WHERE Amount = 0
                    UPDATE Users SET LinkClaimMarker = 0.00000000, LinkShortnerEarning += @LinkClaimMarker, LastClaim = getdate(), LinkClaimInterval = DATEADD(minute, @LinkClaimMinutes, getdate()), JustClaimed = 'true' WHERE ConfirmationCode = @ConfirmationCode
                    SELECT @LinkClaimMarkerIP = LinkClaimMarkerIP, @LinkClaimMarkerId = LinkClaimMarkerId FROM Users WHERE ConfirmationCode = @ConfirmationCode
                    IF NOT EXISTS(Select 1 From LinkShortnersRecord Where IP = @LinkClaimMarkerIP)
                    BEGIN
                    INSERT INTO [LinkShortnersRecord] ([IP]) VALUES (@LinkClaimMarkerIP);
                    END
                    SET @dynamicQuery = 'UPDATE LinkShortnersRecord SET [' + @LinkClaimMarkerId + '] += 1 WHERE IP = ''' + @LinkClaimMarkerIP + ''''
                    EXEC sp_executeSQl @dynamicQuery
                    END     

                    False_Request:  
                    SELECT 1 AS FalseRequest
                    END`);
    })
    .then(result => {
      if (result.recordset[0].FalseRequest === 1) {
        sql.close();
        res.redirect("http://localhost:8081/#/Home");
      } else {
        sql.close();
        res.redirect("http://localhost:8081/#/Home");
      }
    })
    .catch(err => {
      console.log(err);
      sql.close();
      res.redirect("http://localhost:8081/#/Home");
    });
  sql.on("error", err => {
    console.log(err);
    sql.close();
  });
});

app.get("/BonusAd", function(req, res) {
  var linkToRedirect = "http://localhost:8081/#/Home";

  new sql.ConnectionPool(config)
    .connect()
    .then(pool => {
      return pool
        .request()
        .input("ConfirmationCodeInput", req.query.confirmationCode)
        .input("DirectInput", req.query.direct).query(`BEGIN
                    DECLARE @Username varchar(10)
                    DECLARE @ConfirmationCode uniqueidentifier = TRY_CONVERT(UNIQUEIDENTIFIER, @ConfirmationCodeInput)
                    DECLARE @AmountToCredit decimal(9, 8)
                    DECLARE @BonusAdMarkerIP varchar(40)
                    DECLARE @BonusAdMarkerId uniqueidentifier
                    DECLARE @BonusAdLinkMarkerId uniqueidentifier
                    DECLARE @BonusAdLink varchar(max)
                    DECLARE @BonusAdLinkFalseDateTime datetime = getdate()
                    DECLARE @dynamicQuery nvarchar(max)
                    DECLARE @Direct varchar(5) = @DirectInput

                    SELECT @Username = Username, @BonusAdMarkerIP = BonusAdMarkerIP, @BonusAdMarkerId = BonusAdMarkerId, @BonusAdLinkMarkerId = BonusAdLinkMarkerId, @BonusAdLinkFalseDateTime = BonusAdLinkFalseDateTime, @AmountToCredit = BonusAdMarkerAmount FROM Users WHERE BonusAdConfirmationCode = @ConfirmationCode
                    SELECT @BonusAdLink = Link FROM BonusAds WHERE Id = @BonusAdMarkerId
                    
                    IF(@ConfirmationCode IS NOT NULL AND @Username IS NOT NULL)
                    BEGIN                    
                    IF(@Direct = 'false')
                    BEGIN                    
                    IF (@BonusAdLinkFalseDateTime > getdate())
                    BEGIN
                    GOTO False_Request
                    END                
                    IF NOT EXISTS(Select 1 From LinkShortnersRecord Where IP = @BonusAdMarkerIP)
                    BEGIN
                    INSERT INTO [LinkShortnersRecord] ([IP]) VALUES (@BonusAdMarkerIP);
                    END
                    SET @dynamicQuery = 'UPDATE LinkShortnersRecord SET [' + CAST(@BonusAdLinkMarkerId AS nvarchar(max)) + '] += 1 WHERE IP = ''' + @BonusAdMarkerIP + ''''
                    EXEC sp_executeSQl @dynamicQuery
                    IF NOT EXISTS(Select 1 From BonusAdsRecord Where Username = @Username)
                    BEGIN
                    INSERT INTO [BonusAdsRecord] ([Username]) VALUES (@Username);
                    END
                    SET @dynamicQuery = 'UPDATE BonusAdsRecord SET [' + CAST(@BonusAdMarkerId AS nvarchar(max)) + '] += 1 WHERE Username = ''' + @Username + ''''
                    EXEC sp_executeSQl @dynamicQuery                   
                    EXEC UpdateBonusAdBalance @UsernameInput = @Username, @AmountInput = @AmountToCredit                   
                    INSERT INTO BonusAdsHistory(Username, Amount, Link) VALUES(@Username, @AmountToCredit, @BonusAdLink)                    
                    UPDATE Users SET BonusAdEarning += @AmountToCredit, BonusAdMarkerAmount = 0.00000000 WHERE BonusAdConfirmationCode = @ConfirmationCode
                    EXEC UpdateBonusAdCredit @IdInput = @BonusAdMarkerId
                    DELETE FROM LinkShortnersHistory WHERE Amount = 0
                    DELETE FROM BonusAdsHistory WHERE Amount = 0
                    END 
                    ELSE IF(@Direct = 'true')
                    BEGIN
                    IF NOT EXISTS(Select 1 From BonusAdsRecord Where Username = @Username)
                    BEGIN
                    INSERT INTO [BonusAdsRecord] ([Username]) VALUES (@Username);
                    END
                    SET @dynamicQuery = 'UPDATE BonusAdsRecord SET [' + CAST(@BonusAdMarkerId AS nvarchar(max)) + '] += 1 WHERE Username = ''' + @Username + ''''
                    EXEC sp_executeSQl @dynamicQuery                   
                    EXEC UpdateBonusAdBalance @UsernameInput = @Username, @AmountInput = @AmountToCredit                   
                    INSERT INTO BonusAdsHistory(Username, Amount, Link) VALUES(@Username, @AmountToCredit, @BonusAdLink)                    
                    UPDATE Users SET BonusAdEarning += @AmountToCredit, BonusAdMarkerAmount = 0.00000000 WHERE BonusAdConfirmationCode = @ConfirmationCode
                    EXEC UpdateBonusAdCredit @IdInput = @BonusAdMarkerId
                    DELETE FROM BonusAdsHistory WHERE Amount = 0
                    END 
                    END 

                    SELECT (SELECT Link FROM BonusAds WHERE Id = @BonusAdMarkerId) AS Link,
                           (SELECT SocketSession FROM Users WHERE Username = @Username) AS SocketSession         
                    False_Request:  
                    SELECT 1 AS FalseRequest 
                    END`);
    })
    .then(result => {
      console.log(result.recordset);
      console.log(result.recordset[0]);
      if (result.recordset[0].FalseRequest === 1) {
        sql.close();
        res.redirect(linkToRedirect);
      } else {
        linkToRedirect = result.recordset[0].Link;
        socket.emit("updateParticularUserData", [
          {
            Key: particularOutdatedSocketKey,
            SocketSession: result.recordset[0].SocketSession
          }
        ]);
        socket.emit("sendParticularUserMessage", [
          {
            Key: particularOutdatedSocketKey,
            SocketSession: result.recordset[0].SocketSession,
            Message: "Bonus ad was claimed successfully"
          }
        ]);
        sql.close();
        res.redirect(linkToRedirect);
      }
    })
    .catch(err => {
      console.log(err);
      sql.close();
      res.redirect(linkToRedirect);
    });
  sql.on("error", err => {
    console.log(err);
    sql.close();
  });
});

app.get("/Set", function(req, res) {
  var cook = req.headers.referer;
  var query = req.query.confirmationCode;
  console.log(`${cook} Link shortner sent an API request`);
  res.send(`<h1>${cook}</h1><h1>${query}</h1>`);
});

http.listen(3005, function() {
  console.log(
    "listening on *:3005 " +
      moment(Date.now()).format("YYYY-MM-DD - hh:mm:ss A")
  );
});
