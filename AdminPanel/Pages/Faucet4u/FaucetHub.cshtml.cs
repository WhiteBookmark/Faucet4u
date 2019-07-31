using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using AdminPanel.GlobalConnections.Variable;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdminPanel.Pages.Faucet4u
{
    public class FaucetHubModel : PageModel
    {
        private static string TableData()
        {
            try
            {

                using (SqlConnection connectionObject = new SqlConnection(Other.faucet4uSQLConnectionString))
                {
                    dynamic[] result = new dynamic[2];
                    result[0] = connectionObject.Query("SELECT * FROM FaucetHubHistory");

                    HttpClient httpClient = new HttpClient();

                    NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
                    query["api_key"] = FaucetHub.APIKey;
                    query["currency"] = "BTC";
                    string finalQuery = query.ToString();
                    string finalDestination = FaucetHub.getBalance + $"?{finalQuery}";

                    HttpResponseMessage response = httpClient.PostAsync(finalDestination, new FormUrlEncodedContent(new Dictionary<string, string>())).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    JObject jsonData = JObject.Parse(responseString);

                    if (jsonData["status"].ToString().Equals("200"))
                    {
                        result[1] = jsonData;
                        return JsonConvert.SerializeObject(result);
                    }

                    throw new Exception();

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


                ViewData["TableData"] = TableData();
                return Page();

            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.ToString();
                return Page();
            }
        }

        public IActionResult OnPostSendPayment()
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

                    HttpClient httpClient = new HttpClient();

                    NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
                    query["api_key"] = FaucetHub.APIKey;
                    query["currency"] = "BTC";
                    query["address"] = Request.Form["address"];

                    string finalQuery = query.ToString();
                    string finalDestination = FaucetHub.checkAddress + $"?{finalQuery}";

                    HttpResponseMessage response = httpClient.PostAsync(finalDestination, new FormUrlEncodedContent(new Dictionary<string, string>())).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    JObject jsonData = JObject.Parse(responseString);

                    if (jsonData["status"].ToString().Equals("200"))
                    {

                        query["to"] = query["address"];
                        Double amount = Convert.ToDouble(Request.Form["amount"]);
                        amount = amount * 100000000;
                        query["amount"] = amount.ToString();

                        finalQuery = query.ToString();
                        finalDestination = FaucetHub.sendPayment + $"?{finalQuery}";

                        response = httpClient.PostAsync(finalDestination, new FormUrlEncodedContent(new Dictionary<string, string>())).Result;
                        responseString = response.Content.ReadAsStringAsync().Result;
                        jsonData = JObject.Parse(responseString);

                        if (jsonData["status"].ToString().Equals("200"))
                        {

                            int affectedRows = connectionObject.Execute("INSERT INTO FaucetHubHistory(PayoutId, Hash, Address, Amount) VALUES(@PayoutId, @Hash, @Address, @Amount)",
                                new
                                {
                                    PayoutId = jsonData["payout_id"].ToString(),
                                    Hash = jsonData["payout_user_hash"].ToString(),
                                    Address = query["to"],
                                    Amount = Convert.ToDouble(Request.Form["amount"])
                                });

                            if (Convert.ToBoolean(affectedRows) == true)
                            {
                                ViewData["Success"] = "Success";
                                ViewData["TableData"] = TableData();
                                return Page();
                            }
                            else
                            {
                                ViewData["Success"] = "Payment was sent however the database could not update the transaction due to an unknown error";
                                ViewData["TableData"] = TableData();
                                return Page();
                            }
                        }
                        else
                        {
                            ViewData["Failed"] = jsonData.ToString();
                            ViewData["TableData"] = TableData();
                            return Page();
                        }

                    }
                    else
                    {
                        ViewData["Failed"] = jsonData.ToString();
                        ViewData["TableData"] = TableData();
                        return Page();
                    }

                    throw new Exception();
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