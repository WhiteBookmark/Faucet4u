using API.DatabaseModels;
using API.GlobalConnections.Variable;
using Dapper;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Helper.User;
using Faucet4u.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Faucet4u.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountRegisterController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> CreateNewUserAsync([FromBody] AccountRegisterModel BodyValue)
        {
            try
            {
                Users User = new Users
                {
                    Username = BodyValue.Username,
                    Email = BodyValue.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(BodyValue.Password),
                    Country = await GetUserCountry.Parse(BodyValue.IP),
                    Referrer = await GetUserUsername.Exists(BodyValue.Referrer) ? BodyValue.Referrer : null,
                    IP = BodyValue.IP
                };
                await User.SaveAsync();

                Log.Info(new Logs
                {
                    Message = String.Format(LogVariable.newUserRegisteredMessage, BodyValue.Username, BodyValue.Email),
                    IP = GetUserIPAddress.String(this.HttpContext)
                });

                bool WasConfirmationCodeSent = await ConfirmUserEmail.SendCode(BodyValue.Username);
                if (!WasConfirmationCodeSent)
                {
                    return Ok(new { message = UserVariable.accountRegisterFailedSendingConfirmationCodeMessage });
                }
                return Ok(new { message = UserVariable.accountRegisterSuccessfulMessage });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Guid ErrorId = Guid.NewGuid();
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.accountRegisterErrorMessage, JsonConvert.SerializeObject(BodyValue)),
                    IP = GetUserIPAddress.String(this.HttpContext),
                    Username = BodyValue.Username,
                    Exception = ex.ToString()
                });
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, ErrorId.ToString()) } } });
            }
        }
    }
}