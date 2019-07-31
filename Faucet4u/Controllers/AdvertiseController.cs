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
    public class AdvertiseController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetAsync([FromQuery] AdvertiseModel bodyValue)
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    IEnumerable<dynamic> result = await connectionObject.QueryAsync("SELECT * FROM Advertise WHERE Name = @Name AND IsTimeBased = @IsTimeBased ORDER BY Price", new { Name = bodyValue.type, IsTimeBased = bodyValue.isTimeBased });

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

        [HttpPatch]
        public async Task<ActionResult> PatchAsync([FromBody] AdvertiseModel bodyValue)
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    await connectionObject.ExecuteAsync(@"
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
                                    END", new { SessionIdInput = bodyValue.sessionId, Reference = bodyValue.reference });

                    Log.Info(Guid.NewGuid(), String.Format(Logs.infoMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext));
                    return Ok();
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