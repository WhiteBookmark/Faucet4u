using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Helper.User;
using Faucet4u.GlobalConnections.Variable;
using Faucet4u.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using UserVariable = Faucet4u.GlobalConnections.Variable.UserVariable;

namespace Faucet4u.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountRegisterController : ControllerBase
    {
        // POST: api/AccountRegister
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] AccountRegisterModel bodyValue)
        {
            try
            {

                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {

                    DynamicParameters paramtersList = new DynamicParameters();

                    paramtersList.Add("@Username", bodyValue.username, DbType.String, ParameterDirection.Input);
                    paramtersList.Add("@Email", bodyValue.email, DbType.String, ParameterDirection.Input);
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(bodyValue.password);
                    paramtersList.Add("@Password", hashedPassword, dbType: DbType.String, direction: ParameterDirection.Input);
                    paramtersList.Add("@Country", GetUserCountry.Parse(bodyValue.ip), dbType: DbType.String, direction: ParameterDirection.Input);
                    paramtersList.Add("@ReferrerProvided", bodyValue.referrer, DbType.String, ParameterDirection.Input);
                    paramtersList.Add("@IP", bodyValue.ip, DbType.String, ParameterDirection.Input);

                    string queryToExecute = @"BEGIN 
                                                                    DECLARE @Referrer varchar(20) = @ReferrerProvided
                                                                    IF NOT EXISTS(SELECT Username from Users where Username = @Referrer)
                                                                        BEGIN
                                                                    SET @Referrer = null
                                                                        END
                                                                    Insert into Users(Username, Email, Password, Country, Referrer, IP) values(@Username, @Email, @Password, @Country, @Referrer, @IP)
                                                                END";
                    int affectedRows = await connectionObject.ExecuteAsync(queryToExecute, paramtersList);
                    if (Convert.ToBoolean(affectedRows) != true)
                    {
                        throw new Exception();
                    }
                    bool wasConfirmationCodeSent = ConfirmUserEmail.SendCode(bodyValue.username);
                    if (wasConfirmationCodeSent == false)
                    {
                        return Ok(new { message = UserVariable.accountRegisterFailedSendingConfirmationCodeMessage });
                    }

                }
                Log.Info(Guid.NewGuid(), String.Format(Logs.newUserRegisteredMessage, bodyValue.username, bodyValue.email), IPinString: GetUserIPAddress.String(this.HttpContext));
                return Ok(new { message = UserVariable.accountRegisterSuccessfulMessage });
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.accountRegisterErrorMessage, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString(), Username: bodyValue.username);
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });
            }
        }

    }
}
