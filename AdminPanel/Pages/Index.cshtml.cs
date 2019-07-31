using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AdminPanel.GlobalConnections.Variable;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AdminPanel.Pages
{
    public class IndexModel : PageModel
    {

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
                            if (sessionId.Equals(queryResult.SessionId))
                            {
                                Response.Cookies.Append("sessionId", sessionId.ToString());
                                return RedirectToPage("/Faucet4u/Index");
                            }
                        }
                    }
                }
                else if (cookieSessionId == null)
                {
                    return Page();
                }
                #endregion

                return Page();
            }
            catch (Exception)
            {
                return Page();

            }
        }

        public IActionResult OnPost()
        {
            //Suleman Password: 8B6HTCLrpA&66v
            //Ahmed Password: w00#boqLi!Np

            const string ahmedPassword = "$2a$11$lpBFdu3SPcBW2jsJSeOFe.DPgjT4sCcQdKSloKj.iP60CJVmKMANu";
            const string sulemanPassword = "$2y$10$hoDoPdX3JP97fTzNP/ufiu4viCh3/hpyEncC2UoJl/Dp5jmwd8d.m";
            string password = Request.Form["password"];
            string authy = Request.Form["authy"];

            if (!String.IsNullOrEmpty(password) && BCrypt.Net.BCrypt.Verify(password, sulemanPassword))
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-Authy-API-Key", "d7I8AAxUX5BLC7CDyIq2WeGduLw2v3gu");
                HttpResponseMessage response = client.GetAsync($"http://api.authy.com/protected/json/verify/{authy}/42666150").Result;

                if (response.IsSuccessStatusCode)
                {
                    Guid sessionId = Guid.NewGuid();

                    using (SqlConnection connectionObject = new SqlConnection(Other.adminPanelSQLConnectionString))
                    {
                        string queryToExecute = "Insert into AdminPanel(SessionId, SessionExpiry, Name) Values(@SessionId, @SessionExpiry, 'Suleman')";
                        DynamicParameters paramtersToPass = new DynamicParameters();

                        paramtersToPass.Add("SessionId", sessionId, DbType.Guid, ParameterDirection.Input);
                        paramtersToPass.Add("SessionExpiry", DateTime.Now.AddMinutes(30).ToString("MM/dd/yyyy HH:mm:ss"), DbType.DateTime, ParameterDirection.Input);

                        int queryResult = connectionObject.Execute(queryToExecute, paramtersToPass);

                        if (Convert.ToBoolean(queryResult))
                        {
                            Response.Cookies.Append("sessionId", sessionId.ToString());
                            return RedirectToPage("/Faucet4u/Index");
                        }
                    }
                }

            }
            else if (!String.IsNullOrEmpty(password) && BCrypt.Net.BCrypt.Verify(password, ahmedPassword))
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-Authy-API-Key", "d7I8AAxUX5BLC7CDyIq2WeGduLw2v3gu");
                HttpResponseMessage response = client.GetAsync($"http://api.authy.com/protected/json/verify/{authy}/108951050").Result;

                if (response.IsSuccessStatusCode)
                {
                    Guid sessionId = Guid.NewGuid();

                    using (SqlConnection connectionObject = new SqlConnection(Other.adminPanelSQLConnectionString))
                    {
                        string queryToExecute = "Insert into AdminPanel(SessionId, SessionExpiry, Name) Values(@SessionId, @SessionExpiry, 'Ahmed')";
                        DynamicParameters paramtersToPass = new DynamicParameters();

                        paramtersToPass.Add("SessionId", sessionId, DbType.Guid, ParameterDirection.Input);
                        paramtersToPass.Add("SessionExpiry", DateTime.Now.AddMinutes(30).ToString("MM/dd/yyyy HH:mm:ss"), DbType.DateTime, ParameterDirection.Input);

                        int queryResult = connectionObject.Execute(queryToExecute, paramtersToPass);

                        if (Convert.ToBoolean(queryResult))
                        {
                            Response.Cookies.Append("sessionId", sessionId.ToString());
                            return RedirectToPage("/Faucet4u/Index");
                        }
                    }
                }
            }
            return Page();

        }
    }
}
