using API.DatabaseModels;
using API.GlobalConnections.Variable;
using Dapper;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Helper.User;
using Faucet4u.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet4u.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdvertiseController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetAdvertisingPackagesAsync([FromQuery] AdvertiseModel BodyValue)
        {
            try
            {
                AdvertisingPackagesModel[] AdvertisingPackages = await (from Setting in DB.Queryable<Settings>()
                                                                        where Setting.SettingsID.Equals(KeysVariable.SettingsKey)
                                                                        select Setting.AdvertisingPackages).FirstOrDefaultAsync();

                Log.Info(new Logs
                {
                    Message = String.Format(LogVariable.infoMessage, Request.Path.Value, JsonConvert.SerializeObject(BodyValue)),
                    IP = GetUserIPAddress.String(this.HttpContext),
                });

                return Ok(JsonConvert.SerializeObject(AdvertisingPackages));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Guid errorId = Guid.NewGuid();
                Log.Error(new Logs
                {
                    LogID = errorId,
                    Message = String.Format(LogVariable.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(BodyValue)),
                    IP = GetUserIPAddress.String(this.HttpContext),
                    Exception = ex.ToString()
                });
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
        public async Task<ActionResult> PurchaseAdvertisingPackageAsync([FromBody] AdvertiseModel bodyValue)
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(OtherVariable.SQLConnectionString))
                {
                    var User = await (from UserData in DB.Queryable<Users>()
                                      where UserData.SessionId.Equals(new Guid(bodyValue.SessionId))
                                      select new { UserData.Username, UserData.PurchaseBalance }).FirstOrDefaultAsync();

                    AdvertisingPackagesModel AdvertisingPackage = await (from Setting in DB.Queryable<Settings>()
                                                                         where Setting.SettingsID.Equals(KeysVariable.SettingsKey)
                                                                         select Array.Find(Setting.AdvertisingPackages,
                                                                         Package => Package.Reference.Equals(new Guid(bodyValue.Reference)))).FirstOrDefaultAsync();

                    if (User.PurchaseBalance >= AdvertisingPackage.Price)
                    {

                    }

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
                                    END", new { SessionIdInput = bodyValue.SessionId, Reference = bodyValue.Reference });

                    Log.Info(new Logs
                    {
                        Message = String.Format(LogVariable.infoMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)),
                        IP = GetUserIPAddress.String(this.HttpContext),
                    });

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Guid errorId = Guid.NewGuid();
                Log.Error(new Logs
                {
                    LogID = errorId,
                    Message = String.Format(LogVariable.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)),
                    IP = GetUserIPAddress.String(this.HttpContext),
                    Exception = ex.ToString()
                });
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