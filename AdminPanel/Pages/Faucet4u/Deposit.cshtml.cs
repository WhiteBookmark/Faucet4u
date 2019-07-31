using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AdminPanel.GlobalConnections.Variable;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AdminPanel.Pages.Faucet4u
{
    public class DepositModel : PageModel
    {
        private static string TableData()
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.faucet4uSQLConnectionString))
                {
                    dynamic[] result = new dynamic[1];
                    result[0] = connectionObject.Query("SELECT * FROM DepositHistory");
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


        public IActionResult OnPostAddDepositHistory()
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
                    if (connectionObject.Execute(@"BEGIN
                                                INSERT INTO DepositHistory(Username, Amount, WalletType, WalletAddress, Remarks) VALUES(@Username, @Amount, @WalletType, @WalletAddress, @Remarks)
                                                END", new { Username = Request.Form["username"], Amount = Request.Form["amount"], WalletType = Request.Form["type"], WalletAddress = Request.Form["address"], Remarks = Request.Form["remarks"] }) > 0)
                    {
                        ViewData["Success"] = "Success";
                        ViewData["TableData"] = TableData();
                        return Page();

                    }

                    ViewData["Failed"] = "Failed";
                    ViewData["TableData"] = TableData();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.ToString();
                ViewData["TableData"] = TableData();
                return Page();
            }
        }

        public IActionResult OnPostDeleteDepositHistory()
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
                    if (connectionObject.Execute(@"BEGIN
                                                DECLARE @Reference uniqueidentifier = @ReferenceInput
                                                DELETE FROM DepositHistory WHERE Reference = @Reference
                                                END", new { ReferenceInput = Request.Form["reference"] }) > 0)
                    {
                        ViewData["Success"] = "Success";
                        ViewData["TableData"] = TableData();
                        return Page();

                    }

                    ViewData["Failed"] = "Failed";
                    ViewData["TableData"] = TableData();
                    return Page();
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