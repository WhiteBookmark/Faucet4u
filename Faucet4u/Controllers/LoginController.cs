using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Helper.User;
using API.GlobalConnections.Variable;
using Faucet4u.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using API.DatabaseModels;

namespace Faucet4u.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        // POST: api/Login
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] LoginModel bodyValue)
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(OtherVariable.SQLConnectionString))
                {
                    if (await GetUserCountry.IsDifferent(bodyValue.Username, bodyValue.IP))
                    {
                        connectionObject.Execute("Exec InsertCheatHistory @UsernameInput = @Username, @Case = 4", new { Username = bodyValue.Username });
                    }


                    string queryToExecute = "Select Password from Users where Username = @Username";
                    DynamicParameters paramtersToPass = new DynamicParameters();
                    paramtersToPass.Add("Username", bodyValue.Username, DbType.String, ParameterDirection.Input);
                    dynamic hashedPassword = connectionObject.QueryFirstOrDefault(queryToExecute, paramtersToPass);
                    if (hashedPassword != null)
                    {
                        if (BCrypt.Net.BCrypt.Verify(bodyValue.Password, hashedPassword.Password))
                        {


                            Guid sessionIdGuid = Guid.NewGuid();
                            queryToExecute = @"BEGIN
                                                Update Users set SessionId = @SessionId, SessionExpiry = @SessionExpiry, LastLogin = getdate() where Username = @Username
                                                END";
                            paramtersToPass.Add("SessionId", sessionIdGuid, DbType.Guid, ParameterDirection.Input);
                            paramtersToPass.Add("SessionExpiry", DateTime.Now.AddHours(OtherVariable.SessionExpiryHoursToAdd).ToString("MM/dd/yyyy HH:mm:ss"), DbType.DateTime, ParameterDirection.Input);
                            paramtersToPass.Add("Success", true, DbType.Boolean, ParameterDirection.Input);

                            int affectedRows = connectionObject.Execute(queryToExecute, paramtersToPass);
                            if (Convert.ToBoolean(affectedRows))
                            {
                                if (bodyValue.LSI != null)
                                {
                                    CheatTest.CheckMultipleAccount(sessionIdGuid.ToString(), bodyValue.LSI);
                                }
                                connectionObject.Execute("INSERT INTO LoginHistory(Username, Success) VALUES(@Username, @Success)", paramtersToPass);
                                Log.Info(new Logs
                                {
                                    Message = String.Format(LogVariable.loginUserLoggedIn, bodyValue.Username),
                                    IP = GetUserIPAddress.String(this.HttpContext),
                                });
                                return Ok(new { sessionId = sessionIdGuid.ToString() });
                            }

                        }
                    }

                    paramtersToPass.Add("Success", false, DbType.Boolean, ParameterDirection.Input);
                    connectionObject.Execute("INSERT INTO LoginHistory(Username, Success) VALUES(@Username, @Success)", paramtersToPass);

                }

                Log.Hack(new Logs
                {
                    Message = String.Format(LogVariable.loginFailedMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)),
                    IP = GetUserIPAddress.String(this.HttpContext),
                    Username = bodyValue.Username
                });
                return BadRequest(new { errors = new { message = new[] { UserVariable.loginFailedMessage } } });

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
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });

            }
        }
    }
}
