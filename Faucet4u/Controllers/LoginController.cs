using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Helper.User;
using Faucet4u.GlobalConnections.Variable;
using Faucet4u.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Faucet4u.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        // POST: api/Login
        [HttpPost]
        public ActionResult Post([FromBody] LoginModel bodyValue)
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    if (GetUserCountry.IsDifferent(bodyValue.username, bodyValue.ip))
                    {
                        connectionObject.Execute("Exec InsertCheatHistory @UsernameInput = @Username, @Case = 4", new { Username = bodyValue.username });
                    }


                    string queryToExecute = "Select Password from Users where Username = @Username";
                    DynamicParameters paramtersToPass = new DynamicParameters();
                    paramtersToPass.Add("Username", bodyValue.username, DbType.String, ParameterDirection.Input);
                    dynamic hashedPassword = connectionObject.QueryFirstOrDefault(queryToExecute, paramtersToPass);
                    if (hashedPassword != null)
                    {
                        if (BCrypt.Net.BCrypt.Verify(bodyValue.password, hashedPassword.Password))
                        {


                            Guid sessionIdGuid = Guid.NewGuid();
                            queryToExecute = @"BEGIN
                                                Update Users set SessionId = @SessionId, SessionExpiry = @SessionExpiry, LastLogin = getdate() where Username = @Username
                                                END";
                            paramtersToPass.Add("SessionId", sessionIdGuid, DbType.Guid, ParameterDirection.Input);
                            paramtersToPass.Add("SessionExpiry", DateTime.Now.AddHours(Other.sessionExpiryHoursToAdd).ToString("MM/dd/yyyy HH:mm:ss"), DbType.DateTime, ParameterDirection.Input);
                            paramtersToPass.Add("Success", true, DbType.Boolean, ParameterDirection.Input);

                            int affectedRows = connectionObject.Execute(queryToExecute, paramtersToPass);
                            if (Convert.ToBoolean(affectedRows))
                            {
                                if (bodyValue.lsi != null)
                                {
                                    CheatTest.CheckMultipleAccount(sessionIdGuid.ToString(), bodyValue.lsi);
                                }
                                connectionObject.Execute("INSERT INTO LoginHistory(Username, Success) VALUES(@Username, @Success)", paramtersToPass);
                                Log.Info(Guid.NewGuid(), String.Format(Logs.loginUserLoggedIn, bodyValue.username), IPinString: GetUserIPAddress.String(this.HttpContext));
                                return Ok(new { sessionId = sessionIdGuid.ToString() });
                            }

                        }
                    }

                    paramtersToPass.Add("Success", false, DbType.Boolean, ParameterDirection.Input);
                    connectionObject.Execute("INSERT INTO LoginHistory(Username, Success) VALUES(@Username, @Success)", paramtersToPass);

                }

                Log.Hack(Guid.NewGuid(), String.Format(Logs.loginFailedMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), Username: bodyValue.username);
                return BadRequest(new { errors = new { message = new[] { UserVariable.loginFailedMessage } } });

            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString());
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });

            }
        }
    }
}
