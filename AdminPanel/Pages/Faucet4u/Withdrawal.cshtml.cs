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
    public class WithdrawalModel : PageModel
    {
        private static HttpClient httpClient = new HttpClient();

        private static string TableData()
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.faucet4uSQLConnectionString))
                {
                    return JsonConvert.SerializeObject(connectionObject.Query("SELECT * FROM Withdrawal"));
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

        public IActionResult OnPostApproveWithdrawal()
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
                    //using (SqlConnection connectionObject2 = new SqlConnection(Other.adminPanelSQLConnectionString))
                    //{
                    //    string name = connectionObject2.QueryFirstOrDefault("Select Name from AdminPanel where SessionId = @SessionId", new { SessionId = cookieSessionId }).Name;
                    //    string authy = Request.Form["authy"];
                    //    dynamic pay = null;
                    //    HttpResponseMessage response;

                    //    //2 signatures are need to proceed with the withdrawal
                    //    //Signature 1 belongs to Ahmed
                    //    //Signature 2 belongs to Suleman
                    //    if (name == "Suleman")
                    //    {
                    //        HttpClient client = new HttpClient();
                    //        client.DefaultRequestHeaders.Add("X-Authy-API-Key", "d7I8AAxUX5BLC7CDyIq2WeGduLw2v3gu");
                    //        response = client.GetAsync($"http://api.authy.com/protected/json/verify/{authy}/42666150").Result;

                    //        if (response.IsSuccessStatusCode)
                    //        {
                    //            pay = connectionObject.QueryFirstOrDefault(@"BEGIN
                    //                         DECLARE @Reference uniqueidentifier = TRY_CONVERT(UNIQUEIDENTIFIER, @ReferenceInput)
                    //                         DECLARE @Paid BIT
                    //                         DECLARE @Signature1 BIT
                    //                         DECLARE @Pay BIT = 0
                    //                         DECLARE @Address varchar(max)
                    //                         DECLARE @Amount varchar(10)

                    //                         SELECT @Paid = Paid, @Signature1 = Signature1, @Amount = RequestedAmount, @Address = WalletAddress FROM Withdrawal WHERE Reference = @Reference AND Signature2 IS NULL

                    //                         IF(@Paid = 0 OR @Signature1 = 0 OR @Paid = 1)
                    //                         BEGIN
                    //                         THROW 50000, 'Payment is marked as false/true, therefore its state cannot be changed.', 1;
                    //                         END

                    //                         IF(@Signature1 = 1)
                    //                         BEGIN
                    //                         SET @Pay = 1
                    //                         END
                    //                         ELSE IF(@Signature1 IS NULL)
                    //                         BEGIN
                    //                         SET @Pay = 0
                    //                         UPDATE Withdrawal SET Signature2 = 1 WHERE Reference = @Reference AND Signature2 IS NULL
                    //                         END
                    //                         SELECT @Pay AS Pay, @Address AS Address, @Amount AS Amount
                    //                         END", new { ReferenceInput = Request.Form["reference"] });

                    //        }
                    //        else
                    //        {
                    //            ViewData["Error"] = "2FA Code is invalid";
                    //            ViewData["TableData"] = TableData();
                    //            return Page();
                    //        }

                    //    }
                    //    else if (name == "Ahmed")
                    //    {
                    //        HttpClient client = new HttpClient();
                    //        client.DefaultRequestHeaders.Add("X-Authy-API-Key", "d7I8AAxUX5BLC7CDyIq2WeGduLw2v3gu");
                    //        response = client.GetAsync($"http://api.authy.com/protected/json/verify/{authy}/108951050").Result;

                    //        if (response.IsSuccessStatusCode)
                    //        {
                    //            pay = connectionObject.QueryFirstOrDefault(@"BEGIN
                    //                         DECLARE @Reference uniqueidentifier = TRY_CONVERT(UNIQUEIDENTIFIER, @ReferenceInput)
                    //                         DECLARE @Paid BIT
                    //                         DECLARE @Signature2 BIT
                    //                         DECLARE @Pay BIT = 0
                    //                         DECLARE @Address varchar(max)
                    //                         DECLARE @Amount varchar(10)

                    //                         SELECT @Paid = Paid, @Signature2 = Signature2, @Amount = RequestedAmount, @Address = WalletAddress FROM Withdrawal WHERE Reference = @Reference AND Signature1 IS NULL

                    //                         IF(@Paid = 0 OR @Signature2 = 0 OR @Paid = 1)
                    //                         BEGIN
                    //                         THROW 50000, 'Payment is marked as false/true, therefore its state cannot be changed.', 1;
                    //                         END

                    //                         IF(@Signature2 = 1)
                    //                         BEGIN
                    //                         SET @Pay = 1
                    //                         END
                    //                         ELSE IF(@Signature2 IS NULL)
                    //                         BEGIN
                    //                         SET @Pay = 0
                    //                         UPDATE Withdrawal SET Signature1 = 1 WHERE Reference = @Reference AND Signature1 IS NULL
                    //                         END
                    //                         SELECT @Pay AS Pay, @Address AS Address, @Amount AS Amount
                    //                         END", new { ReferenceInput = Request.Form["reference"] });
                    //        }
                    //        else
                    //        {
                    //            ViewData["Error"] = "2FA Code is invalid";
                    //            ViewData["TableData"] = TableData();
                    //            return Page();
                    //        }
                    //    }

                    //    //Now proceed with signing the payment request
                    //    if (pay.Pay == false)
                    //    {
                    //        ViewData["Success"] = "Signature updated";
                    //        ViewData["TableData"] = TableData();
                    //        return Page();
                    //    }
                    //    else if (pay.Pay == true)
                    //    {
                    //        //Following code is meant for bitcoin withdrawal only
                    //        string mainBalance = httpClient.GetAsync($"{Payment.getMainBalance}?apiKey={Payment.apiKey}&sessionId={Session.overrideValidation}").Result.Content.ReadAsStringAsync().Result;
                    //        JObject parsedBalance = JObject.Parse(mainBalance);
                    //        mainBalance = parsedBalance["result"]["confirmed"].ToString();

                    //        if (Convert.ToDouble(mainBalance) <= Convert.ToDouble(pay.Amount)) throw new Exception("Insufficient balance");

                    //        string json = JsonConvert.SerializeObject(new
                    //        {
                    //            apiKey = Payment.apiKey,
                    //            sessionId = Session.overrideValidation,
                    //            address = pay.Address,
                    //            amount = pay.Amount
                    //        });
                    //        response = httpClient.PostAsync(Payment.signPayment, new StringContent(json, Encoding.UTF8, "application/json")).Result;
                    //        string result = response.Content.ReadAsStringAsync().Result;
                    //        JObject parsedResult = JObject.Parse(result);

                    //        if (response.IsSuccessStatusCode != true)
                    //        {
                    //            ViewData["TableData"] = TableData();
                    //            ViewData["Error"] = result;
                    //            return Page();
                    //        }

                    //        //Finally broadcast the transaction

                    //        json = JsonConvert.SerializeObject(new
                    //        {
                    //            apiKey = Payment.apiKey,
                    //            sessionId = Session.overrideValidation,
                    //            transaction = parsedResult["result"]["hex"].ToString()
                    //        });

                    //        response = httpClient.PostAsync(Payment.broadcastTransaction, new StringContent(json, Encoding.UTF8, "application/json")).Result;
                    //        result = response.Content.ReadAsStringAsync().Result;

                    //        if (response.IsSuccessStatusCode != true)
                    //        {
                    //            ViewData["TableData"] = TableData();
                    //            ViewData["Error"] = result;
                    //            return Page();
                    //        }

                    //        //Now we finally update the database and credit the user's balance

                    //        if (connectionObject.Execute(@"BEGIN
                    //                                    DECLARE @Reference uniqueidentifier = TRY_CONVERT(UNIQUEIDENTIFIER, @ReferenceInput)
                    //                                    DECLARE @Remarks varchar(max) = @RemarksInput

                    //                                    UPDATE Withdrawal SET Paid = 'true', PaymentDate = getdate(), PaymentAmount = RequestedAmount, Remarks = @Remarks, Signature1 = 1, Signature2 = 1 WHERE Reference = @Reference
                    //                                    END", new { ReferenceInput = Request.Form["reference"], RemarksInput = Request.Form["remarks"] }) > 0)
                    //        {

                    //            dynamic emailResult = connectionObject.QueryFirstOrDefault(@"BEGIN
                    //                                        DECLARE @Username varchar(10)
                    //                                        SELECT @Username = Username FROM Withdrawal WHERE Reference = @Reference
                    //                                        SELECT (SELECT Email FROM Users WHERE Username = @Username) AS Email, (SELECT TOP 1 WithdrawalType FROM Withdrawal WHERE Reference = @Reference) AS WithdrawalType, (SELECT TOP 1 RequestedAmount FROM Withdrawal WHERE Reference = @Reference) AS Amount, (SELECT TOP 1 WalletType FROM Withdrawal WHERE Reference = @Reference) AS WalletType, (SELECT TOP 1 WalletAddress FROM Withdrawal WHERE Reference = @Reference) AS WalletAddress
                    //                                        END", new { Reference = Request.Form["reference"] });

                    //            string emailSubject = $"Withdrawal request approved";
                    //            string emailMessage = $@"
                    //                                    Your withdrawal request has been approved.
                    //                                    Your funds are being transferred, please allow up to 72 hours to reach them in your wallet.

                    //                                    Withdrawal Type: {emailResult.WithdrawalType}
                    //                                    Amount: {emailResult.Amount} BTC
                    //                                    Wallet Type: {emailResult.WalletType}
                    //                                    Wallet Address: {emailResult.WalletAddress}";

                    //            Task.Factory.StartNew(() =>
                    //                GlobalConnections.Helper.User.Email.SendEmail(emailResult.Email, emailSubject, emailMessage));

                    //        }

                    //        ViewData["Success"] = "Payment has been sent from our wallet.";
                    //        ViewData["TableData"] = TableData();
                    //        return Page();
                    //    }

                    //    ViewData["Failed"] = "Failed";
                    //    ViewData["TableData"] = TableData();
                    //    return Page();
                    //}

                    if (connectionObject.Execute(@"BEGIN
                                                        DECLARE @Reference uniqueidentifier = TRY_CONVERT(UNIQUEIDENTIFIER, @ReferenceInput)
                                                        DECLARE @Remarks varchar(max) = @RemarksInput

                                                        UPDATE Withdrawal SET Paid = 'true', PaymentDate = getdate(), PaymentAmount = RequestedAmount, Remarks = @Remarks, Signature1 = 1, Signature2 = 1 WHERE Reference = @Reference
                                                        END", new { ReferenceInput = Request.Form["reference"], RemarksInput = Request.Form["remarks"] }) > 0)
                    {

                        dynamic emailResult = connectionObject.QueryFirstOrDefault(@"BEGIN
                                                                DECLARE @Username varchar(10)
                                                                SELECT @Username = Username FROM Withdrawal WHERE Reference = @Reference
                                                                SELECT (SELECT Email FROM Users WHERE Username = @Username) AS Email, (SELECT TOP 1 WithdrawalType FROM Withdrawal WHERE Reference = @Reference) AS WithdrawalType, (SELECT TOP 1 RequestedAmount FROM Withdrawal WHERE Reference = @Reference) AS Amount, (SELECT TOP 1 WalletType FROM Withdrawal WHERE Reference = @Reference) AS WalletType, (SELECT TOP 1 WalletAddress FROM Withdrawal WHERE Reference = @Reference) AS WalletAddress
                                                                END", new { Reference = Request.Form["reference"] });

                        string emailSubject = $"Withdrawal request approved";
                        string emailMessage = $@"
                                                            Your withdrawal request has been approved.
                                                            Your funds are being transferred, please allow up to 72 hours to reach them in your wallet.

                                                            Withdrawal Type: {emailResult.WithdrawalType}
                                                            Amount: {emailResult.Amount} BTC
                                                            Wallet Type: {emailResult.WalletType}
                                                            Wallet Address: {emailResult.WalletAddress}";

                        Task.Factory.StartNew(() =>
                            GlobalConnections.Helper.User.Email.SendEmail(emailResult.Email, emailSubject, emailMessage));

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

        public IActionResult OnPostDeclineWithdrawal()
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
                        string name = connectionObject2.QueryFirstOrDefault("Select Name from AdminPanel where SessionId = @SessionId", new { SessionId = cookieSessionId }).Name;

                        if (connectionObject.Execute(@"BEGIN
                                                DECLARE @Reference uniqueidentifier = @ReferenceInput
                                                DECLARE @Remarks varchar(max) = @RemarksInput
                                                DECLARE @Paid BIT = 0
                                                DECLARE @Name varchar(10) = @NameInput

                                                SELECT @Paid = Paid FROM Withdrawal WHERE Reference = @Reference

                                                IF(@Paid = 0 OR @Paid = 1)
                                                BEGIN
                                                THROW 50001, 'Payment is marked as false/true, therefore its state cannot be changed.', 1;
                                                END

                                                IF(@Name = 'Ahmed')
                                                BEGIN
                                                UPDATE Withdrawal SET Paid = 'false', Signature1 = 0, Remarks = @Remarks WHERE Reference = @Reference
                                                END
                                                ELSE IF(@Name = 'Suleman')
                                                BEGIN
                                                UPDATE Withdrawal SET Paid = 'false', Signature2 = 0, Remarks = @Remarks WHERE Reference = @Reference
                                                END

                                                DECLARE @Username varchar(12)
                                                DECLARE @WithdrawalType varchar(15)
                                                DECLARE @Amount decimal(9, 8)
                                                SELECT @Username = Username FROM Withdrawal WHERE Reference = @Reference
                                                SELECT @WithdrawalType = WithdrawalType FROM Withdrawal WHERE Reference = @Reference
                                                SELECT @Amount = RequestedAmount FROM Withdrawal WHERE Reference = @Reference
                                                IF(@WithdrawalType = 'Standard')
                                                BEGIN
                                                UPDATE Users SET Balance += @Amount WHERE Username = @Username
                                                END
                                                ELSE IF(@WithdrawalType = 'Offerwall')
                                                BEGIN
                                                UPDATE Users SET OfferwallBalance += @Amount WHERE Username = @Username
                                                END
                                                END", new { ReferenceInput = Request.Form["reference"], RemarksInput = Request.Form["remarks"], NameInput = name }) > 0)
                        {

                            dynamic result = connectionObject.QueryFirstOrDefault(@"BEGIN
                                                    DECLARE @Username varchar(12)
                                                    SELECT @Username = Username FROM Withdrawal WHERE Reference = @Reference
                                                    SELECT (SELECT Email FROM Users WHERE Username = @Username) AS Email, (SELECT TOP 1 WithdrawalType FROM Withdrawal WHERE Reference = @Reference) AS WithdrawalType, (SELECT TOP 1 RequestedAmount FROM Withdrawal WHERE Reference = @Reference) AS Amount, (SELECT TOP 1 WalletType FROM Withdrawal WHERE Reference = @Reference) AS WalletType, (SELECT TOP 1 WalletAddress FROM Withdrawal WHERE Reference = @Reference) AS WalletAddress
                                                    END", new { Reference = Request.Form["reference"] });

                            string emailSubject = $"Withdrawal request declined";
                            string emailMessage = $"Your withdrawal request has been declined<br>Your funds are have been refunded back into your account balance<hr><b>Withdrawal Type:</b> {result.WithdrawalType}<br><b>Amount:</b> {result.Amount} Bitcoin<br><b>Wallet Type:</b> {result.WalletType}<br><b>Wallet Address:</b> {result.WalletAddress}";

                            Task.Factory.StartNew(() =>
                                GlobalConnections.Helper.User.Email.SendEmail(result.Email, emailSubject, emailMessage));

                            ViewData["Success"] = "Success";
                            ViewData["TableData"] = TableData();
                            return Page();

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