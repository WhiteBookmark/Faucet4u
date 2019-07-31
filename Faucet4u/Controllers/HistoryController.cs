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
    public class HistoryController : ControllerBase
    {
        [HttpGet]
        [Route("Login")]
        public async Task<ActionResult> GetLoginAsync([FromQuery] SessionValidModel bodyValue)
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    IEnumerable<dynamic> result = await connectionObject.QueryAsync("SELECT Success, CONVERT(VARCHAR, DateTime, 22) AS DateTime FROM LoginHistory WHERE Username = @Username", new { Username = await GetUserUsername.String(bodyValue.sessionId) });

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

        [HttpGet]
        [Route("Deposit")]
        public async Task<ActionResult> GetDepositAsync([FromQuery] SessionValidModel bodyValue)
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    IEnumerable<dynamic> result = await connectionObject.QueryAsync("SELECT CONVERT(VARCHAR, DepositDate, 22) AS DepositDate, Amount, Confirmations, Remarks FROM DepositHistory WHERE Username = @Username", new { Username = await GetUserUsername.String(bodyValue.sessionId) });

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
        [HttpGet]
        [Route("Withdrawal")]
        public async Task<ActionResult> GetWithdrawalAsync([FromQuery] SessionValidModel bodyValue)
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {

                    IEnumerable<dynamic> result = await connectionObject.QueryAsync("SELECT WithdrawalType, CONVERT(VARCHAR, RequestedDate, 22) AS RequestedDate, RequestedAmount, (SELECT CASE WHEN Paid = 1 THEN 'Approved' WHEN Paid = 0 THEN 'Rejected' ELSE 'Pending' END AS Paid) AS Paid, WalletType, WalletAddress, Remarks FROM Withdrawal WHERE Username = @Username", new { Username = await GetUserUsername.String(bodyValue.sessionId) });

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

        [HttpGet]
        [Route("Order")]
        public async Task<ActionResult> GetOrderAsync([FromQuery] SessionValidModel bodyValue)
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    IEnumerable<dynamic> result = await connectionObject.QueryAsync("SELECT CONVERT(VARCHAR, OrderDateTime, 22) AS OrderDateTime, Name, IsTimeBased, Credit, Price FROM OrderHistory WHERE Username = @Username", new { Username = await GetUserUsername.String(bodyValue.sessionId) });

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

    }
}