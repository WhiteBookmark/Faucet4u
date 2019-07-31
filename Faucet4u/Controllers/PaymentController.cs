using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

namespace Faucet4u.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        public static HttpClient httpClient = new HttpClient();
        private static string electrumUsername = "faucet4all";
        private static string electrumPassword = "35WtyWW5jj2TI9";
        //private static string electrumHost = "http://66.206.39.103:7778";
        private static string electrumHost = "http://66.206.39.103:7777";
        private static string apiKey = "OyDSf6q7wx%m";

        [HttpGet]
        [Route("GetMainBalance")]
        public ActionResult GetMainBalance([FromQuery] PaymentModel bodyValue)
        {
            try
            {
                if (bodyValue.apiKey != apiKey)
                    return BadRequest("Incorrect key");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{electrumUsername}:{electrumPassword}")));
                string json = JsonConvert.SerializeObject(new
                {
                    id = Guid.NewGuid().ToString(),
                    method = "getbalance",
                    @params = new string[] { }
                });
                HttpResponseMessage response = httpClient.PostAsync(electrumHost, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString());
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });
            }
        }

        [HttpGet]
        [Route("GetMainAddress")]
        public ActionResult GetMainAddress([FromQuery] PaymentModel bodyValue)
        {
            try
            {
                if (bodyValue.apiKey != apiKey)
                    return BadRequest("Incorrect key");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{electrumUsername}:{electrumPassword}")));
                string json = JsonConvert.SerializeObject(new
                {
                    id = Guid.NewGuid().ToString(),
                    method = "createnewaddress",
                    @params = new string[] { }

                });
                HttpResponseMessage response = httpClient.PostAsync(electrumHost, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString());
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });
            }
        }

        [HttpPost]
        [Route("Deposit")]
        public async Task<ActionResult> PostDepositAsync([FromBody] PaymentModel bodyValue)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{electrumUsername}:{electrumPassword}")));
                string transactionMessage = $"Deposit request for user: {GetUserUsername.String(bodyValue.sessionId)} and amount: {bodyValue.amount}";
                string json = JsonConvert.SerializeObject(new
                {
                    id = Guid.NewGuid().ToString(),
                    method = "addrequest",
                    @params = new
                    {
                        amount = bodyValue.amount,
                        memo = transactionMessage,
                        force = "true"
                    }

                });
                HttpResponseMessage response = httpClient.PostAsync(electrumHost, new StringContent(json, Encoding.UTF8, "application/json")).Result;
                string result = response.Content.ReadAsStringAsync().Result;
                JObject parsedResult = JObject.Parse(result);

                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    string username = await GetUserUsername.String(bodyValue.sessionId);
                    string queryToExecute = "INSERT INTO DepositHistory(Username, Amount, WalletType, WalletAddress, InvoiceId, Remarks) VALUES(@Username, @Amount, @WalletType, @WalletAddress, @InvoiceId, @Remarks)";
                    DynamicParameters parametersToPass = new DynamicParameters();
                    parametersToPass.Add("Username", username, DbType.String, ParameterDirection.Input);
                    parametersToPass.Add("Amount", parsedResult["result"]["amount (BTC)"].ToString(), DbType.Decimal, ParameterDirection.Input);
                    parametersToPass.Add("WalletType", "BTC", DbType.String, ParameterDirection.Input);
                    parametersToPass.Add("WalletAddress", parsedResult["result"]["address"].ToString(), DbType.String, ParameterDirection.Input);
                    parametersToPass.Add("Remarks", transactionMessage, DbType.String, ParameterDirection.Input);
                    parametersToPass.Add("InvoiceId", parsedResult["result"]["id"].ToString(), DbType.String, ParameterDirection.Input);

                    connectionObject.Execute(queryToExecute, parametersToPass);
                }
                return Ok(new { message = parsedResult["result"]["address"].ToString() });
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString());
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });
            }
        }

        [HttpGet]
        [Route("GetRequest")]
        public ActionResult GetRequest([FromQuery] PaymentModel bodyValue)
        {
            try
            {
                if (bodyValue.apiKey != apiKey)
                    return BadRequest("Incorrect key");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{electrumUsername}:{electrumPassword}")));
                string json = JsonConvert.SerializeObject(new
                {
                    id = Guid.NewGuid().ToString(),
                    method = "getrequest",
                    @params = new
                    {
                        key = bodyValue.address
                    }
                });
                HttpResponseMessage response = httpClient.PostAsync(electrumHost, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString());
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });
            }
        }

        [HttpPost]
        [Route("SignPayment")]
        public ActionResult PostSignPayment([FromBody] PaymentModel bodyValue)
        {
            try
            {
                if (bodyValue.apiKey != apiKey)
                    return BadRequest("Incorrect key");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{electrumUsername}:{electrumPassword}")));
                string json = JsonConvert.SerializeObject(new
                {
                    id = Guid.NewGuid().ToString(),
                    method = "payto",
                    @params = new
                    {
                        destination = bodyValue.address,
                        amount = bodyValue.amount,
                        rbf = "true",
                        fee = 0.00000400
                    }
                });
                HttpResponseMessage response = httpClient.PostAsync(electrumHost, new StringContent(json, Encoding.UTF8, "application/json")).Result;

                string result = response.Content.ReadAsStringAsync().Result;
                JObject parsedResult = JObject.Parse(result);

                if (parsedResult["error"].ToString() != "")
                {
                    return BadRequest(result);
                }
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString());
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });
            }
        }

        [HttpPost]
        [Route("BroadcastTransaction")]
        public ActionResult PostBroadcastTransaction([FromBody] PaymentModel bodyValue)
        {
            try
            {
                if (bodyValue.apiKey != apiKey)
                    return BadRequest("Incorrect key");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{electrumUsername}:{electrumPassword}")));
                string json = JsonConvert.SerializeObject(new
                {
                    id = Guid.NewGuid().ToString(),
                    method = "broadcast",
                    @params = new
                    {
                        tx = bodyValue.transaction,
                    }
                });
                HttpResponseMessage response = httpClient.PostAsync(electrumHost, new StringContent(json, Encoding.UTF8, "application/json")).Result;
                string result = response.Content.ReadAsStringAsync().Result;
                JObject parsedResult = JObject.Parse(result);

                if (parsedResult["error"].ToString() != "")
                {
                    return BadRequest(result);
                }
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString());
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });
            }
        }

        [HttpGet]
        [Route("GetConfirmations")]
        public ActionResult GetConfirmations([FromQuery] PaymentModel bodyValue)
        {
            try
            {
                if (bodyValue.apiKey != apiKey)
                    return BadRequest("Incorrect key");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{electrumUsername}:{electrumPassword}")));
                string json = JsonConvert.SerializeObject(new
                {
                    id = Guid.NewGuid().ToString(),
                    method = "get_tx_status",
                    @params = new
                    {
                        txid = bodyValue.transaction
                    }
                });
                HttpResponseMessage response = httpClient.PostAsync(electrumHost, new StringContent(json, Encoding.UTF8, "application/json")).Result;
                string result = response.Content.ReadAsStringAsync().Result;
                JObject parsedResult = JObject.Parse(result);

                if (parsedResult["error"].ToString() != "")
                {
                    return BadRequest(result);
                }
                return Ok(result);
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