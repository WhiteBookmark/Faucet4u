using AdminPanel.GlobalConnections.Variable;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AdminPanel.Pages.Faucet4u
{
    public class EmailModel : PageModel
    {
        private static string TableData()
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.faucet4uSQLConnectionString))
                {
                    dynamic[] result = new dynamic[1];
                    result[0] = connectionObject.Query("SELECT * FROM EmailHistory");
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

                #endregion Session validation

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

        public IActionResult OnPostSendEmail()
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

                #endregion Session validation

                using (SqlConnection connectionObject = new SqlConnection(Other.faucet4uSQLConnectionString))
                {
                    dynamic[] result = new dynamic[1];
                    DynamicParameters parametersToPass = new DynamicParameters();
                    parametersToPass.Add("UsernameOrEmail", Request.Form["usernameOrEmail"]);
                    result[0] = connectionObject.Query(@"
                                                        BEGIN
                                                        IF(@UsernameOrEmail = '*')
                                                        BEGIN
                                                        SELECT Email FROM Users
                                                        END
                                                        ELSE
                                                        BEGIN
                                                        SELECT Email FROM Users WHERE Email = @UsernameOrEmail OR Username = @UsernameOrEmail
                                                        END
                                                        SELECT Username, CheatCounter, Locked FROM Users
                                                        END", parametersToPass);

                    string rawResult = JsonConvert.SerializeObject(result[0]);
                    JArray parsedResult = JArray.Parse(rawResult);

                    foreach (JObject root in parsedResult)
                    {
                        foreach (KeyValuePair<String, JToken> app in root)
                        {
                            Task.Factory.StartNew(() =>
                            AdminPanel.GlobalConnections.Helper.User.Email.SendEmail(app.Value.ToString(), Request.Form["subject"], Request.Form["message"]));
                        }
                    }

                    int affectedRows = connectionObject.Execute(@"
                                                        BEGIN
                                                        INSERT INTO EmailHistory(Recipient, Subject, Message) VALUES(@UsernameOrEmail, @Subject, @Message)
                                                        END", new { UsernameOrEmail = Request.Form["usernameOrEmail"], Subject = Request.Form["subject"], Message = Request.Form["message"] });

                    if (Convert.ToBoolean(affectedRows) != true)
                    {
                        ViewData["TableData"] = TableData();
                        ViewData["Success"] = "Email were sent however the history was not updated due to an unknown reason. Please consult with the developer.";
                        return Page();
                    }
                    //Test comment 12345
                    ViewData["TableData"] = TableData();
                    ViewData["Success"] = "Success";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ViewData["TableData"] = TableData();
                ViewData["Error"] = ex.ToString();
                return Page();
            }
        }
    }
}