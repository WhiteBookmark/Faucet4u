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
    public class BonusAdsModel : PageModel
    {
        private static string TableData()
        {
            try
            {

                using (SqlConnection connectionObject = new SqlConnection(Other.faucet4uSQLConnectionString))
                {
                    dynamic[] result = new dynamic[1];
                    result[0] = connectionObject.Query("SELECT * FROM BonusAds");
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

        public IActionResult OnPostAddBonusAd()
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
                    Guid linkIdentifier = Guid.NewGuid();
                    if (connectionObject.Execute($@"
                            IF(@Title IS NULL OR @Title = '' OR @Description IS NULL OR @Description = '')
                            BEGIN
                            Insert into BonusAds(Id, Link, IsTimeBased, Credit) values(@Id, @Link, @IsTimeBased, @Credit);
                            ALTER TABLE BonusAdsRecord ADD ""{linkIdentifier.ToString().ToUpper()}"" INT DEFAULT 0 NOT NULL
                            END
                            ELSE
                            BEGIN
                            Insert into BonusAds(Id, Link, IsTimeBased, Credit, Title, Description) values(@Id, @Link, @IsTimeBased, @Credit, @Title, @Description);
                            ALTER TABLE BonusAdsRecord ADD ""{linkIdentifier.ToString().ToUpper()}"" INT DEFAULT 0 NOT NULL
                            END", new { Link = Request.Form["link"], IsTimeBased = Request.Form["isTimeBased"], Credit = Request.Form["credit"], Title = Request.Form["title"], Description = Request.Form["description"], Limit = Convert.ToInt32(Request.Form["limit"]), Id = linkIdentifier }) > 0)
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

        public IActionResult OnPostDeleteBonusAd()
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
                    Guid bonusAdId = new Guid(Request.Form["id"]);

                    if (connectionObject.Execute($@"DECLARE @sql NVARCHAR(MAX)

                       WHILE 1 = 1
                       BEGIN
                           SELECT TOP 1 @sql = N'alter table BonusAdsRecord drop constraint [' + dc.NAME + N']'
                           from sys.default_constraints dc
                           JOIN sys.columns c
                               ON c.default_object_id = dc.object_id
                           WHERE
                               dc.parent_object_id = OBJECT_ID('BonusAdsRecord')
                           AND c.name = N'{bonusAdId.ToString().ToUpper()}'
                           IF @@ROWCOUNT = 0 BREAK
                           EXEC(@sql)
                           END
                       DELETE FROM BonusAds WHERE Id = TRY_CONVERT(UNIQUEIDENTIFIER, @Id)
                       ALTER TABLE BonusAdsRecord DROP COLUMN [{bonusAdId.ToString().ToUpper()}]", new { Id = bonusAdId.ToString() }) > 0)
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

        public IActionResult OnPostUpdateBonusAd()
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
                                                UPDATE BonusAds SET Link = @Link, Title = @Title, Description = @Description WHERE Id = @Id
                                                END",
                                                new
                                                {
                                                    Id = Request.Form["id"],
                                                    Link = Request.Form["link"],
                                                    Title = Request.Form["title"],
                                                    Description = Request.Form["description"]
                                                }) > 0)
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

        public IActionResult OnPostUpdateCreditBonusAd()
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
                                                    IF(@Action = 'Add')
                                                    BEGIN
                                                    UPDATE BonusAds SET Credit += @Credit WHERE Id = @Id
                                                    END
                                                    ELSE IF(@Action = 'Retract')
                                                    BEGIN
                                                    UPDATE BonusAds SET Credit -= @Credit WHERE Id = @Id
                                                    END
                                                    ELSE IF(@Action = 'Replace')
                                                    BEGIN
                                                    UPDATE BonusAds SET Credit = @Credit WHERE Id = @Id
                                                    END
                                                END",
                                                new
                                                {
                                                    Id = Request.Form["id"],
                                                    Credit = Request.Form["credit"],
                                                    Action = Request.Form["creditAction"]
                                                }) > 0)
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