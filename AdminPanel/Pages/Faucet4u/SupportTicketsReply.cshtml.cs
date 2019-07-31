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
    public class SupportTicketsReplyModel : PageModel
    {
        private static string TableData(string referenceInput)
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.faucet4uSQLConnectionString))
                {
                    dynamic[] result = new dynamic[2];
                    DynamicParameters paramtersToPass = new DynamicParameters();
                    paramtersToPass.Add("ReferenceInput", referenceInput);

                    //result[0] = connectionObject.Query(@"BEGIN
                    //                                     DECLARE @Reference uniqueidentifier = @ReferenceInput
                    //                                     SELECT Reply, DateTime, Username FROM SupportTicketsReply WHERE Reference = @Reference ORDER BY DateTime DESC
                    //                                     END", paramtersToPass);
                    result[0] = connectionObject.Query("SELECT * FROM SupportTicketsReply WHERE Reference = @ReferenceInput ORDER BY DateTime DESC", paramtersToPass);
                    result[1] = connectionObject.Query("SELECT Message FROM SupportTickets WHERE Reference = @ReferenceInput", paramtersToPass);
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

                string supportTicketReference = Request.Cookies["supportTicketReference"];


                if (supportTicketReference != null)
                {
                    ViewData["TableData"] = TableData(supportTicketReference);
                    ViewData["supportTicketReference"] = supportTicketReference;
                    return Page();
                }

                return RedirectToPage("SupportTickets");

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
                        ViewData["TableData"] = TableData(Request.Form["reference"]);
                        ViewData["supportTicketReference"] = Request.Cookies["supportTicketReference"];

                        return Page();

                    }

                    ViewData["Failed"] = "Failed";
                    ViewData["TableData"] = TableData(Request.Form["reference"]);
                    ViewData["supportTicketReference"] = Request.Cookies["supportTicketReference"];

                    return Page();
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.ToString();
                ViewData["TableData"] = TableData(Request.Form["reference"]);
                ViewData["supportTicketReference"] = Request.Cookies["supportTicketReference"];

                return Page();
            }
        }

        public IActionResult OnPostReplySupportTicket()
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
                                                    DECLARE @Username varchar(10) = 'Admin'
                                                    DECLARE @Reference uniqueidentifier = @ReferenceInput
                                                    DECLARE @ReplyMessage varchar(max) = @ReplyInput

                                                    IF @ReplyMessage IS NOT NULL AND @ReplyMessage != ''
                                                    BEGIN
                                                    INSERT INTO SupportTicketsReply(Reference, Username, Reply, [Read]) VALUES(@Reference, @Username, @ReplyMessage, 1)
                                                    UPDATE SupportTickets SET LastReplier = @Username WHERE Reference = @Reference
                                                    END
                                                    END", new { ReferenceInput = Request.Form["reference"], ReplyInput = Request.Form["reply"] }) > 0)
                    {
                        dynamic result = connectionObject.QueryFirstOrDefault(@"BEGIN
                                                    DECLARE @Username varchar(12)
                                                    SELECT @Username = Username FROM SupportTickets WHERE Reference = @Reference
                                                    SELECT Email FROM Users WHERE Username = @Username
                                                    END", new { Reference = Request.Form["reference"] });

                        string emailSubject = $"Support ticket reply referenced {Request.Form["reference"]}";
                        string emailMessage = $"{Request.Form["reply"]}";


                        Task.Factory.StartNew(() =>
                            GlobalConnections.Helper.User.Email.SendEmail(result.Email, emailSubject, emailMessage));

                        ViewData["Success"] = "Success";
                        ViewData["TableData"] = TableData(Request.Form["reference"]);
                        ViewData["supportTicketReference"] = Request.Cookies["supportTicketReference"];

                        return Page();

                    }

                    ViewData["Failed"] = "Failed";
                    ViewData["TableData"] = TableData(Request.Form["reference"]);
                    ViewData["supportTicketReference"] = Request.Cookies["supportTicketReference"];

                    return Page();
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.ToString();
                ViewData["TableData"] = TableData(Request.Form["reference"]);
                ViewData["supportTicketReference"] = Request.Cookies["supportTicketReference"];

                return Page();
            }
        }
    }
}