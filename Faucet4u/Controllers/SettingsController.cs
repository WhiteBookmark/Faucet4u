using System;
using System.Collections;
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
    public class SettingsController : ControllerBase
    {
        // GET: api/Settings
        [HttpGet]
        public ActionResult Get([FromQuery] SettingsGetModel bodyValue)
        {
            try
            {
                //if (GetUserCountry.Compare(new Guid(bodyValue.sessionId), this.HttpContext) == false)
                //{
                //    Log.Hack(Guid.NewGuid(), String.Format(Logs.countryDifferentMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext));
                //    return BadRequest(new { message = UserVariable.countryDifferentMessage });
                //}
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    //string queryToExecute = "Select FaucetHubBitcoinAddress, BitcoinAddress, PerfectMoneyAddress, PayeerAddress, AdvCashAddress, EthereumAddress, BitcoinCashAddress, DogecoinAddress, DashAddress, ZCashAddress, LitecoinAddress, EthereumClassicAddress, PeercoinAddress from Users where SessionId = @SessionId";
                    string queryToExecute = "Select FaucetHubBitcoinAddress from Users where SessionId = @SessionId";
                    DynamicParameters parametersToPass = new DynamicParameters();
                    parametersToPass.Add("SessionId", new Guid(bodyValue.sessionId), DbType.Guid, ParameterDirection.Input);
                    dynamic queryResult = connectionObject.QueryFirstOrDefault(queryToExecute, parametersToPass);
                    if (queryToExecute != null)
                    {
                        Log.Info(Guid.NewGuid(), String.Format(Logs.infoMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext));
                        return Ok(JsonConvert.SerializeObject(queryResult));
                    }
                }
                Guid hackId = Guid.NewGuid();
                Log.Hack(hackId, String.Format(Logs.hackAttemptMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext));
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, hackId.ToString()) } } });
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString());
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });

            }
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync([FromBody] SettingsPutModel bodyValue)
        {
            try
            {
                //if (GetUserCountry.Compare(new Guid(bodyValue.sessionId), this.HttpContext) == false)
                //{
                //    Log.Hack(Guid.NewGuid(), String.Format(Logs.countryDifferentMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), Username: GetUserUsername.String(bodyValue.sessionId));
                //    return BadRequest(new { message = UserVariable.countryDifferentMessage });
                //}
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    //Check if someone else is having same faucethub address as that address is permanent for a faucethub user account
                    if (CheatTest.IsFaucethubAddressExisting(bodyValue.sessionId, bodyValue.FaucetHubBitcoinAddress))
                    {
                        string queryForCheatCounter = @"BEGIN
                                                        DECLARE @Username varchar(10)
                                                        SELECT @Username = Username FROM Users WHERE SessionId = @SessionId
                                                        EXEC InsertCheatHistory @UsernameInput = @Username, @Case = 2
                                                        END";
                        connectionObject.Execute(queryForCheatCounter, new { SessionId = bodyValue.sessionId });

                    }
                    //string queryToExecute = "Update Users Set FaucetHubBitcoinAddress = @FaucetHubBitcoinAddress, BitcoinAddress = @BitcoinAddress, PerfectMoneyAddress = @PerfectMoneyAddress, PayeerAddress = @PayeerAddress, AdvCashAddress = @AdvCashAddress, EthereumAddress = @EthereumAddress, BitcoinCashAddress = @BitcoinCashAddress, DogecoinAddress = @DogecoinAddress, DashAddress =@DashAddress, ZCashAddress = @ZCashAddress, LitecoinAddress = @LitecoinAddress, EthereumClassicAddress = @EthereumClassicAddress, PeercoinAddress = @PeercoinAddress  where SessionId = @SessionId";
                    string queryToExecute = "Update Users Set FaucetHubBitcoinAddress = @FaucetHubBitcoinAddress  where SessionId = @SessionId";
                    DynamicParameters parametersToPass = new DynamicParameters();

                    parametersToPass.Add("FaucetHubBitcoinAddress", bodyValue.FaucetHubBitcoinAddress, DbType.String, ParameterDirection.Input);
                    //parametersToPass.Add("BitcoinAddress", bodyValue.BitcoinAddress, DbType.String, ParameterDirection.Input);
                    //parametersToPass.Add("PerfectMoneyAddress", bodyValue.PerfectMoneyAddress, DbType.String, ParameterDirection.Input);
                    //parametersToPass.Add("PayeerAddress", bodyValue.PayeerAddress, DbType.String, ParameterDirection.Input);
                    //parametersToPass.Add("AdvCashAddress", bodyValue.AdvCashAddress, DbType.String, ParameterDirection.Input);
                    //parametersToPass.Add("EthereumAddress", bodyValue.EthereumAddress, DbType.String, ParameterDirection.Input);
                    //parametersToPass.Add("BitcoinCashAddress", bodyValue.BitcoinCashAddress, DbType.String, ParameterDirection.Input);
                    //parametersToPass.Add("DogecoinAddress", bodyValue.DogecoinAddress, DbType.String, ParameterDirection.Input);
                    //parametersToPass.Add("DashAddress", bodyValue.DashAddress, DbType.String, ParameterDirection.Input);
                    //parametersToPass.Add("ZCashAddress", bodyValue.ZCashAddress, DbType.String, ParameterDirection.Input);
                    //parametersToPass.Add("LitecoinAddress", bodyValue.LitecoinAddress, DbType.String, ParameterDirection.Input);
                    //parametersToPass.Add("EthereumClassicAddress", bodyValue.EthereumClassicAddress, DbType.String, ParameterDirection.Input);
                    //parametersToPass.Add("PeercoinAddress", bodyValue.PeercoinAddress, DbType.String, ParameterDirection.Input);
                    parametersToPass.Add("SessionId", new Guid(bodyValue.sessionId), DbType.Guid, ParameterDirection.Input);

                    int queryResult = connectionObject.Execute(queryToExecute, parametersToPass);

                    if (Convert.ToBoolean(queryResult))
                    {
                        Log.Info(Guid.NewGuid(), String.Format(Logs.infoMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), Username: await GetUserUsername.String(bodyValue.sessionId));
                        return Ok(new { message = UserVariable.settingsUpdationSuccessfulMessage });
                    }
                }
                Guid hackId = Guid.NewGuid();
                Log.Hack(hackId, String.Format(Logs.hackAttemptMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), Username: await GetUserUsername.String(bodyValue.sessionId));
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, hackId.ToString()) } } });
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString(), Username: await GetUserUsername.String(bodyValue.sessionId));
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });

            }

        }

        [HttpPatch]
        public async Task<ActionResult> PatchAsync([FromBody] SettingsPatchModel bodyValue)
        {
            try
            {

                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {

                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(bodyValue.password);
                    string queryToExecute = "Update Users Set Password = @Password  where SessionId = @SessionId AND SessionExpiry > getdate()";

                    DynamicParameters parametersToPass = new DynamicParameters();
                    parametersToPass.Add("Password", hashedPassword, DbType.String, ParameterDirection.Input);
                    parametersToPass.Add("SessionId", new Guid(bodyValue.sessionId), DbType.Guid, ParameterDirection.Input);

                    int queryResult = connectionObject.Execute(queryToExecute, parametersToPass);

                    if (Convert.ToBoolean(queryResult))
                    {
                        Log.Info(Guid.NewGuid(), String.Format(Logs.infoMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), Username: await GetUserUsername.String(bodyValue.sessionId));
                        return Ok(new { message = UserVariable.settingsUpdationSuccessfulMessage });
                    }
                }
                Guid hackId = Guid.NewGuid();
                Log.Hack(hackId, String.Format(Logs.hackAttemptMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), Username: await GetUserUsername.String(bodyValue.sessionId));
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, hackId.ToString()) } } });
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString(), Username: await GetUserUsername.String(bodyValue.sessionId));
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });

            }
        }

        // This one is required to be public (no auth)
        [HttpGet]
        [Route("Faucet")]
        public async Task<ActionResult> GetFaucetAsync()
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    IEnumerable<dynamic> result = await connectionObject.QueryAsync("SELECT * FROM Settings");
                    return Ok(JsonConvert.SerializeObject(result));
                }
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.unknownErrorMessage, Request.Path.Value), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString());
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });

            }
        }
    }
}