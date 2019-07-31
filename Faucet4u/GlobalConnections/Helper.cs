using Dapper;
using Faucet4u.GlobalConnections.Variable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Faucet4u.GlobalConnections
{
    namespace Helper
    {
        namespace User
        {

            public class GetUserIPAddress
            {
                public static string String(HttpContext userHTTPContext)
                {
                    if (userHTTPContext.Request.Headers["CF-CONNECTING-IP"].ToString() != null)
                        return userHTTPContext.Request.Headers["CF-CONNECTING-IP"].ToString();

                    return userHTTPContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                }
                public static IPAddress IPAddress(HttpContext userHTTPContext)
                {
                    if (userHTTPContext.Request.Headers["CF-CONNECTING-IP"].ToString() != null)
                        return System.Net.IPAddress.Parse(userHTTPContext.Request.Headers["CF-CONNECTING-IP"].ToString());

                    return userHTTPContext.Connection.RemoteIpAddress.MapToIPv4();
                }

            }
            public class GetUserCountry
            {
                public static string Parse(HttpContext userHTTPContext)
                {
                    try
                    {
                        if (userHTTPContext.Request.Headers["CF-IPCountry"].ToString() != null)
                            return new RegionInfo(userHTTPContext.Request.Headers["CF-IPCountry"]).EnglishName;

                        return "Other";
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Guid.NewGuid(), String.Format(Logs.getUserCountryUnknownErrorMessage, GetUserIPAddress.String(userHTTPContext)), IPinString: GetUserIPAddress.String(userHTTPContext), ExceptionMessage: ex.ToString());
                        Console.WriteLine(ex.ToString());
                        return "Other";
                    }
                }

                public static string Parse(string userIPAddress)
                {
                    try
                    {

                        using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                        {
                            HttpClient httpClient = new HttpClient();
                            var httpResponse = httpClient.GetAsync($"{Other.ipStackAPI}/{userIPAddress}?access_key={Other.ipStackAPIKey}").Result;
                            if (httpResponse.StatusCode != HttpStatusCode.OK)
                            {
                                return "Other";
                            }

                            String jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
                            JObject jsonData = JObject.Parse(jsonResponse);
                            if (jsonData["country_name"].ToString() != null)
                            {
                                return jsonData["country_name"].ToString();

                            }

                            return "Other";
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Guid.NewGuid(), String.Format(Logs.getUserCountryUnknownErrorMessage, userIPAddress), IPinString: userIPAddress, ExceptionMessage: ex.ToString());
                        Console.WriteLine(ex.ToString());
                        return "Other";
                    }
                }

                public static bool IsDifferent(string username, string ip)
                {
                    try
                    {

                        using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                        {
                            dynamic registeredCountry = connectionObject.QueryFirstOrDefault("Select Country From Users Where Username = @Username", new { Username = username });
                            string ipCountry = Parse(ip);

                            if (String.Equals(Convert.ToString(registeredCountry.Country), ipCountry, StringComparison.OrdinalIgnoreCase))
                            {
                                return false;
                            }
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Guid.NewGuid(), String.Format(Logs.getUserCountryUnknownErrorMessage, ip), IPinString: ip, ExceptionMessage: ex.ToString());
                        Console.WriteLine(ex.ToString());
                        return false;
                    }
                }
            }

            public class ConfirmUserEmail
            {
                public static bool SendCode(string usernameOrEmail)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(Other.SQLConnectionString))
                        {
                            //First update the confirmation code and expiry time
                            string queryToExecute = "UPDATE users SET ConfirmationCode = @ConfirmationCode, ConfirmationCodeExpiryTime = @ConfirmationCodeExpiryTime WHERE Username = @UsernameOrEmail OR Email = @UsernameOrEmail";
                            Guid confirmationCode = Guid.NewGuid();
                            DateTime confirmationCodeExpiryTime = DateTime.Now.AddMinutes(Other.minutesToAddInConfirmationCodeExpiryTime);
                            DynamicParameters queryParameters = new DynamicParameters();
                            queryParameters.Add("@ConfirmationCode", confirmationCode, DbType.Guid, ParameterDirection.Input);
                            queryParameters.Add("@ConfirmationCodeExpiryTime", confirmationCodeExpiryTime, DbType.DateTime, ParameterDirection.Input);
                            queryParameters.Add("@UsernameOrEmail", usernameOrEmail, DbType.String, ParameterDirection.Input);
                            int affectedRows = connection.Execute(queryToExecute, queryParameters);
                            if (Convert.ToBoolean(affectedRows) != true)
                                throw new Exception();

                            //Now extract the details to send the code via email
                            queryToExecute = "Select Email from Users where Username = @UsernameOrEmail OR Email = @UsernameOrEmail";
                            dynamic userEmail = connection.QueryFirst(queryToExecute, queryParameters);

                            NetworkCredential authenticationDetails = new NetworkCredential()
                            {
                                UserName = EmailClient.username,
                                Password = EmailClient.password
                            };

                            SmtpClient serverEmail = new SmtpClient()
                            {
                                Host = EmailClient.host,
                                Port = EmailClient.port,
                                DeliveryMethod = SmtpDeliveryMethod.Network,
                                UseDefaultCredentials = EmailClient.useDefaultCredentials,
                                Credentials = authenticationDetails,
                                EnableSsl = EmailClient.useSSL,
                                Timeout = EmailClient.timeout
                            };

                            MailAddress fromMailAddress = new MailAddress(EmailClient.fromMailAddress, EmailClient.displayName);

                            MailMessage messageToSend = new MailMessage()
                            {
                                Body = String.Format(EmailClient.bodyMessage, confirmationCode.ToString()),
                                From = fromMailAddress,
                                Priority = MailPriority.High,
                                Subject = EmailClient.subjectMessage
                            };

                            messageToSend.To.Add(userEmail.Email);
                            serverEmail.Send(messageToSend);

                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Guid.NewGuid(), String.Format(Logs.confirmUserEmailSendCodeUnknownErrorMessage, usernameOrEmail), ex.ToString());
                        Console.WriteLine(ex.ToString());
                        return false;
                    }
                }
            }
            public class PasswordReset
            {
                public static bool SendCode(string usernameOrEmail)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(Other.SQLConnectionString))
                        {
                            //First update the confirmation code and expiry time
                            string queryToExecute = "UPDATE users SET ConfirmationCode = @ConfirmationCode, ConfirmationCodeExpiryTime = @ConfirmationCodeExpiryTime WHERE Username = @UsernameOrEmail OR Email = @UsernameOrEmail";
                            Guid confirmationCode = Guid.NewGuid();
                            DateTime confirmationCodeExpiryTime = DateTime.Now.AddMinutes(Other.minutesToAddInConfirmationCodeExpiryTime);
                            DynamicParameters queryParameters = new DynamicParameters();
                            queryParameters.Add("@ConfirmationCode", confirmationCode, DbType.Guid, ParameterDirection.Input);
                            queryParameters.Add("@ConfirmationCodeExpiryTime", confirmationCodeExpiryTime, DbType.DateTime, ParameterDirection.Input);
                            queryParameters.Add("@UsernameOrEmail", usernameOrEmail, DbType.String, ParameterDirection.Input);
                            int affectedRows = connection.Execute(queryToExecute, queryParameters);
                            if (Convert.ToBoolean(affectedRows) != true)
                                throw new Exception();

                            //Now extract the details to send the code via email
                            queryToExecute = "Select Email from Users where Username = @UsernameOrEmail OR Email = @UsernameOrEmail";
                            dynamic userEmail = connection.QueryFirst(queryToExecute, queryParameters);

                            NetworkCredential authenticationDetails = new NetworkCredential()
                            {
                                UserName = EmailClient.username,
                                Password = EmailClient.password
                            };

                            SmtpClient serverEmail = new SmtpClient()
                            {
                                Host = EmailClient.host,
                                Port = EmailClient.port,
                                DeliveryMethod = SmtpDeliveryMethod.Network,
                                UseDefaultCredentials = EmailClient.useDefaultCredentials,
                                Credentials = authenticationDetails,
                                EnableSsl = EmailClient.useSSL,
                                Timeout = EmailClient.timeout
                            };

                            MailAddress fromMailAddress = new MailAddress(EmailClient.fromMailAddress, EmailClient.displayName);

                            MailMessage messageToSend = new MailMessage()
                            {
                                Body = String.Format(@"
                                                        Confirmation code: {0}
                                                        Either click the below link or input the code manually by visiting http://www.faucet4all.com/#/PasswordReset
                                                        http://www.faucet4all.com/#/PasswordReset?passwordResetCode={0}", confirmationCode.ToString()),
                                From = fromMailAddress,
                                Priority = MailPriority.High,
                                Subject = "Reset your password"
                            };

                            messageToSend.To.Add(userEmail.Email);
                            serverEmail.Send(messageToSend);

                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Guid.NewGuid(), String.Format(Logs.confirmUserEmailSendCodeUnknownErrorMessage, usernameOrEmail), ex.ToString());
                        Console.WriteLine(ex.ToString());
                        return false;
                    }
                }
            }

            public class GetUserUsername
            {
                public async static Task<string> String(string sessionId)
                {
                    try
                    {
                        using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                        {
                            string queryToExecute = "Select Username from Users where SessionId = @SessionId";
                            IEnumerable<dynamic> queryResult = await connectionObject.QueryAsync(queryToExecute, new { SessionId = sessionId });
                            if (queryResult != null)
                            {
                                string convertedUsername = Convert.ToString(queryResult.ElementAt(0).Username);
                                return await Task.FromResult(convertedUsername);
                            }

                        }
                        return await Task.FromResult("");
                    }
                    catch (Exception ex)
                    {

                        Log.Error(Guid.NewGuid(), System.String.Format(Logs.unknownErrorMessage, typeof(GetUserUsername).Name, sessionId), ex.ToString());
                        Console.WriteLine(ex.ToString());
                        return null;
                    }
                }
            }

            public class CheatTest
            {
                public static bool IsFaucethubAddressExisting(string sessionId, string faucethubAddress)
                {
                    try
                    {
                        using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                        {
                            string queryToExecute = "Select FaucetHubBitcoinAddress from Users where FaucetHubBitcoinAddress = @FaucetHubBitcoinAddress AND SessionId != @SessionId";
                            dynamic queryResult = connectionObject.QueryFirstOrDefault(queryToExecute, new { SessionId = sessionId, FaucetHubBitcoinAddress = faucethubAddress });
                            if (queryResult != null)
                            {
                                return true;
                            }

                        }
                        return false;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Guid.NewGuid(), System.String.Format(Logs.unknownErrorMessage, typeof(CheatTest).Name, sessionId), ex.ToString());
                        Console.WriteLine(ex.ToString());
                        return false;
                    }
                }

                public static void CheckMultipleAccount(string sessionId, string detectedSessionId)
                {
                    try
                    {
                        using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                        {

                            if ((detectedSessionId != null) && (sessionId != null))
                            {

                                string queryToExecute = "Select Username from Users where SessionId = @SessionId";

                                DynamicParameters paramtersToPass = new DynamicParameters();

                                Guid detectedSessionIdGuid = new Guid(detectedSessionId);
                                Guid sessionIdGuid = new Guid(sessionId);

                                paramtersToPass.Add("DetectedSessionId", detectedSessionIdGuid, DbType.Guid, ParameterDirection.Input);
                                paramtersToPass.Add("SessionId", sessionIdGuid, DbType.Guid, ParameterDirection.Input);

                                dynamic queryResult = connectionObject.QueryFirstOrDefault(queryToExecute, paramtersToPass);
                                paramtersToPass.Add("Username", queryResult.Username, DbType.String, ParameterDirection.Input);

                                queryToExecute = "SELECT Username FROM Users WHERE SessionId = @DetectedSessionId";
                                dynamic detectedAccount = connectionObject.QueryFirstOrDefault(queryToExecute, paramtersToPass);

                                if (detectedAccount != null)
                                {
                                    if (detectedAccount.Username != queryResult.Username)
                                    {
                                        paramtersToPass.Add("DetectedUsername", detectedAccount.Username, DbType.String, ParameterDirection.Input);

                                        connectionObject.Execute("EXEC InsertCheatHistory @UsernameInput = @Username, @Case = 3", paramtersToPass);
                                        connectionObject.Execute("EXEC InsertCheatHistory @UsernameInput = @DetectedUsername, @Case = 3", paramtersToPass);
                                    }
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Guid.NewGuid(), System.String.Format(Logs.unknownErrorMessage, typeof(CheatTest).Name, sessionId), ex.ToString());
                        Console.WriteLine(ex.ToString());
                    }
                }
            }

            public class ConvertTo
            {
                public static double Bitcoin(string usd)
                {
                    try
                    {

                        HttpClient httpClient = new HttpClient();
                        var httpResponse = httpClient.GetAsync($"{Other.blockchainRateAPI}?currency=USD&value={usd}").Result;

                        if (httpResponse.StatusCode != HttpStatusCode.OK)
                        {

                            throw new Exception();
                        }
                        String result = httpResponse.Content.ReadAsStringAsync().Result;
                        return Convert.ToDouble(result);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Guid.NewGuid(), System.String.Format(Logs.unknownErrorMessage, typeof(ConvertTo).Name, usd), ex.ToString());
                        Console.WriteLine(ex.ToString());
                        return 0.00000000;
                    }
                }

                public static string MD5(string input)
                {
                    // Use input string to calculate MD5 hash
                    using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                    {
                        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                        byte[] hashBytes = md5.ComputeHash(inputBytes);

                        // Convert the byte array to hexadecimal string
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < hashBytes.Length; i++)
                        {
                            sb.Append(hashBytes[i].ToString("X2"));
                        }
                        return sb.ToString();
                    }
                }

                public static string SHA256(string input)
                {
                    // Use input string to calculate SHA256 hash
                    // Create a SHA256   
                    using (SHA256 sha256Hash = System.Security.Cryptography.SHA256.Create())
                    {
                        // ComputeHash - returns byte array  
                        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                        // Convert byte array to a string   
                        StringBuilder builder = new StringBuilder();
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            builder.Append(bytes[i].ToString("x2"));
                        }
                        return builder.ToString();
                    }
                }
            }

            public class Email
            {
                public static bool SendEmail(string email, string subject, string message)
                {
                    try
                    {


                        NetworkCredential authenticationDetails = new NetworkCredential()
                        {
                            UserName = EmailClient.username,
                            Password = EmailClient.password
                        };

                        SmtpClient serverEmail = new SmtpClient()
                        {
                            Host = EmailClient.host,
                            Port = EmailClient.port,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = EmailClient.useDefaultCredentials,
                            Credentials = authenticationDetails,
                            EnableSsl = EmailClient.useSSL,
                            Timeout = EmailClient.timeout
                        };

                        MailAddress fromMailAddress = new MailAddress(EmailClient.fromMailAddress, EmailClient.displayName);

                        MailMessage messageToSend = new MailMessage()
                        {
                            Body = message,
                            From = fromMailAddress,
                            Priority = MailPriority.High,
                            Subject = subject,
                            IsBodyHtml = false
                        };

                        messageToSend.To.Add(email);
                        serverEmail.Send(messageToSend);


                        return true;
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.ToString());
                        return false;
                    }
                }
            }

        }

    }

}
