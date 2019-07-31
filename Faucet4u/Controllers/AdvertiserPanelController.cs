using Dapper;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Helper.User;
using Faucet4u.GlobalConnections.Variable;
using Faucet4u.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Faucet4u.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdvertiserPanelController : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult> GetAsync([FromQuery] AdvertiseModel bodyValue)
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    IEnumerable<dynamic> result = await connectionObject.QueryAsync(@"
                                BEGIN
                                IF(@Name = 'PTP')
                                SELECT * FROM PTP WHERE Username = @Username ORDER BY Creation
                                ELSE IF(@Name = 'Banner')
                                SELECT * FROM BannerRotator WHERE Username = @Username ORDER BY Creation
                                ELSE IF(@Name = 'SquareBanner')
                                SELECT * FROM SquareBannerRotator WHERE Username = @Username ORDER BY Creation
                                ELSE IF(@Name = 'BonusAds')
                                SELECT * FROM BonusAds WHERE Username = @Username ORDER BY Creation

                                END", new { Username = await GetUserUsername.String(bodyValue.sessionId), Name = bodyValue.name });

                    Log.Info(Guid.NewGuid(), String.Format(Logs.infoMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext));
                    return Ok(JsonConvert.SerializeObject(result));
                }
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString());
                return BadRequest(new
                {
                    errors = new
                    {
                        message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) }
                    }
                });

            }
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] AdvertiseModel bodyValue)
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    DynamicParameters dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("SessionIdInput", bodyValue.sessionId);
                    dynamicParameters.Add("Name", bodyValue.name);
                    dynamicParameters.Add("Link", bodyValue.link);
                    dynamicParameters.Add("ImageLink", bodyValue.imageLink);
                    dynamicParameters.Add("TargetLink", bodyValue.targetLink);
                    dynamicParameters.Add("IsTimeBased", bodyValue.isTimeBased);
                    dynamicParameters.Add("CreditInput", bodyValue.creditInput);
                    dynamicParameters.Add("Title", bodyValue.title);
                    dynamicParameters.Add("Description", bodyValue.description);
                    Console.WriteLine(bodyValue.isTimeBased);
                    await connectionObject.ExecuteAsync(@"
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

                                    END", dynamicParameters);

                    Log.Info(Guid.NewGuid(), String.Format(Logs.infoMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext));
                    return Ok(new { message = "Your advertisement has been created." });
                }
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString());
                return BadRequest(new
                {
                    errors = new
                    {
                        message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) }
                    }
                });

            }
        }
    }
}