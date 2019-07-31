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
    public class SupportTicketModel : PageModel
    {
        private static string TableData()
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.faucet4uSQLConnectionString))
                {
                    return JsonConvert.SerializeObject(connectionObject.Query("SELECT * FROM SupportTickets"));
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

        public IActionResult OnPostLockSupportTicket()
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
                                                DECLARE @Reference varchar(max) = @ReferenceInput

                                                UPDATE SupportTickets SET Locked = 'true' WHERE Reference = @Reference
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

        public IActionResult OnPostViewSupportTicket()
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
                                                DECLARE @Reference varchar(max) = @ReferenceInput

                                                UPDATE SupportTickets SET [Read] = 'true' WHERE Reference = @Reference
                                                UPDATE SupportTicketsReply SET [Read] = 'true' WHERE Reference = @Reference
                                                END", new { ReferenceInput = Request.Form["reference"] }) > 0)
                    {
                        Response.Cookies.Append("supportTicketReference", Request.Form["reference"]);
                        return RedirectToPage("SupportTicketsReply");

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