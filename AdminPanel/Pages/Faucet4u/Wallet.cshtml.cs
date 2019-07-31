using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AdminPanel.GlobalConnections.Variable;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdminPanel.Pages.Faucet4u
{
    public class WalletModel : PageModel
    {
        private static HttpClient httpClient = new HttpClient();

        private static string TableData()
        {
            try
            {
                using (SqlConnection connectionObject2 = new SqlConnection(Other.adminPanelSQLConnectionString))
                {
                    dynamic[] result = new dynamic[3];
                    result[0] = httpClient.GetAsync($"{Payment.getMainBalance}?apiKey={Payment.apiKey}&sessionId={Session.overrideValidation}").Result.Content.ReadAsStringAsync().Result;
                    result[1] = httpClient.GetAsync($"{Payment.getMainAddress}?apiKey={Payment.apiKey}&sessionId={Session.overrideValidation}").Result.Content.ReadAsStringAsync().Result;
                    result[2] = connectionObject2.Query("SELECT * FROM WithdrawalHistory");

                    return JsonConvert.SerializeObject(result);
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        public IActionResult OnGet()
        {
            try
            {
                #region Session validation

                string cookieSessionId = Request.Cookies["sessionId"];
                if (cookieSessionId != null)
                {
                    Guid sessionId = new Guid(cookieSessionId);

                    using (SqlConnection connectionObject = new SqlConnection(Other.adminPanelSQLConnectionString))
                    {
                        string queryToExecute = "Select SessionId from AdminPanel where SessionId = @SessionId AND SessionExpiry > @DateTimeNow";
                        DynamicParameters paramtersToPass = new DynamicParameters();

                        paramtersToPass.Add("SessionId", sessionId, DbType.Guid, ParameterDirection.Input);
                        paramtersToPass.Add("DateTimeNow", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), DbType.DateTime, ParameterDirection.Input);

                        dynamic queryResult = connectionObject.QueryFirstOrDefault(queryToExecute, paramtersToPass);

                        if (queryResult != null)
                        {
                            if (!sessionId.Equals(queryResult.SessionId))
                            {
                                return RedirectToPage("/Index");
                            }
                        }
                        else if (queryResult == null)
                        {
                            return RedirectToPage("/Index");
                        }
                    }
                }
                else if (cookieSessionId == null)
                {
                    return RedirectToPage("/Index");
                }

                #endregion


                using (SqlConnection connectionObject = new SqlConnection(Other.faucet4uSQLConnectionString))
                {
                    ViewData["TableData"] = TableData();

                    return Page();
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.ToString();
                return Page();
            }
        }

        public IActionResult OnPostWithdraw()
        {
            try
            {
                #region Session validation

                string cookieSessionId = Request.Cookies["sessionId"];
                if (cookieSessionId != null)
                {
                    Guid sessionId = new Guid(cookieSessionId);

                    using (SqlConnection connectionObject = new SqlConnection(Other.adminPanelSQLConnectionString))
                    {
                        string queryToExecute = "Select SessionId from AdminPanel where SessionId = @SessionId AND SessionExpiry > @DateTimeNow";
                        DynamicParameters paramtersToPass = new DynamicParameters();

                        paramtersToPass.Add("SessionId", sessionId, DbType.Guid, ParameterDirection.Input);
                        paramtersToPass.Add("DateTimeNow", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), DbType.DateTime, ParameterDirection.Input);

                        dynamic queryResult = connectionObject.QueryFirstOrDefault(queryToExecute, paramtersToPass);

                        if (queryResult != null)
                        {
                            if (!sessionId.Equals(queryResult.SessionId))
                            {
                                return RedirectToPage("/Index");
                            }
                        }
                        else if (queryResult == null)
                        {
                            return RedirectToPage("/Index");
                        }
                    }
                }
                else if (cookieSessionId == null)
                {
                    return RedirectToPage("/Index");
                }

                #endregion

                using (SqlConnection connectionObject = new SqlConnection(Other.faucet4uSQLConnectionString))
                {
                    using (SqlConnection connectionObject2 = new SqlConnection(Other.adminPanelSQLConnectionString))
                    {
                        dynamic sessionData = connectionObject2.QueryFirstOrDefault("SELECT * FROM AdminPanel WHERE SessionId = @SessionId AND SessionExpiry > getdate()", new { SessionId = cookieSessionId });
                        string authy = Request.Form["authy"];
                        dynamic withdraw = false;
                        HttpResponseMessage response;

                        if (sessionData.Name == "Suleman")
                        {
                            HttpClient client = new HttpClient();
                            client.DefaultRequestHeaders.Add("X-Authy-API-Key", "d7I8AAxUX5BLC7CDyIq2WeGduLw2v3gu");
                            response = client.GetAsync($"http://api.authy.com/protected/json/verify/{authy}/42666150").Result;

                            if (response.IsSuccessStatusCode)
                            {
                                withdraw = true;

                            }
                            else
                            {
                                ViewData["Error"] = "2FA Code is invalid";
                                ViewData["TableData"] = TableData();
                                return Page();
                            }

                        }
                        else if (sessionData.Name == "Ahmed")
                        {
                            HttpClient client = new HttpClient();
                            client.DefaultRequestHeaders.Add("X-Authy-API-Key", "d7I8AAxUX5BLC7CDyIq2WeGduLw2v3gu");
                            response = client.GetAsync($"http://api.authy.com/protected/json/verify/{authy}/108951050").Result;

                            if (response.IsSuccessStatusCode)
                            {
                                withdraw = true;
                            }
                            else
                            {
                                ViewData["Error"] = "2FA Code is invalid";
                                ViewData["TableData"] = TableData();
                                return Page();
                            }
                        }

                        //Now proceed with signing the payment request
                        if (withdraw == false)
                        {
                            ViewData["Failed"] = "Request failed due to insufficient verification data.";
                            ViewData["TableData"] = TableData();
                            return Page();
                        }
                        else if (withdraw == true)
                        {
                            //Following code is meant for bitcoin withdrawal only
                            string mainBalance = httpClient.GetAsync($"{Payment.getMainBalance}?apiKey={Payment.apiKey}&sessionId={Session.overrideValidation}").Result.Content.ReadAsStringAsync().Result;
                            JObject parsedBalance = JObject.Parse(mainBalance);
                            mainBalance = parsedBalance["result"]["confirmed"].ToString();

                            if (Convert.ToDouble(mainBalance) <= Convert.ToDouble(Request.Form["amount"])) throw new Exception("Insufficient balance");

                            string json = JsonConvert.SerializeObject(new
                            {
                                apiKey = Payment.apiKey,
                                sessionId = Session.overrideValidation,
                                address = Request.Form["address"].ToString(),
                                amount = Request.Form["amount"].ToString()
                            });
                            response = httpClient.PostAsync(Payment.signPayment, new StringContent(json, Encoding.UTF8, "application/json")).Result;
                            string result = response.Content.ReadAsStringAsync().Result;
                            JObject parsedResult = JObject.Parse(result);

                            if (response.IsSuccessStatusCode != true)
                            {
                                ViewData["TableData"] = TableData();
                                ViewData["Error"] = result;
                                return Page();
                            }

                            //Finally broadcast the transaction

                            json = JsonConvert.SerializeObject(new
                            {
                                apiKey = Payment.apiKey,
                                sessionId = Session.overrideValidation,
                                transaction = parsedResult["result"]["hex"].ToString()
                            });

                            response = httpClient.PostAsync(Payment.broadcastTransaction, new StringContent(json, Encoding.UTF8, "application/json")).Result;
                            result = response.Content.ReadAsStringAsync().Result;


                            if (response.IsSuccessStatusCode != true)
                            {
                                ViewData["TableData"] = TableData();
                                ViewData["Error"] = result;
                                return Page();
                            }
                            parsedResult = JObject.Parse(result);

                            //Now we finally update the database and record this transaction

                            Console.WriteLine(parsedResult["result"].ToString());
                            if (connectionObject2.Execute(@"BEGIN
                                                        DECLARE @SessionId uniqueidentifier = TRY_CONVERT(UNIQUEIDENTIFIER, @sessionIdInput)

                                                        INSERT INTO WithdrawalHistory(SessionId, SessionExpiry, Name, Amount, Address, [Transaction]) VALUES(@SessionId, @SessionExpiry, @Name, @Amount, @Address, @Transaction)
                                                        END", new
                            {
                                sessionIdInput = sessionData.SessionId,
                                SessionExpiry = sessionData.SessionExpiry,
                                Name = sessionData.Name,
                                Amount = Request.Form["amount"].ToString(),
                                Address = Request.Form["address"].ToString(),
                                Transaction = parsedResult["result"].ToString()
                            }) > 0)
                            {

                                ViewData["Success"] = "Payment has been sent from our wallet.";
                                ViewData["TableData"] = TableData();
                                return Page();
                            }
                        }

                        ViewData["Failed"] = "Failed";
                        ViewData["TableData"] = TableData();
                        return Page();
                    }

                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.ToString();
                ViewData["TableData"] = TableData();
                return Page();
            }
        }
    }
}