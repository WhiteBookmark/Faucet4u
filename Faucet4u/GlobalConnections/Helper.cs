using API.DatabaseModels;
using API.GlobalConnections.Variable;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Faucet4u.GlobalConnections.Helper.User
{

    public class GetUserIPAddress
    {
        //It is almost impossible to get an error in the following two methods so try-catch is not need
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
        public static string Parse(HttpContext UserHTTPContext)
        {
            try
            {
                if (UserHTTPContext.Request.Headers["CF-IPCountry"].ToString() != null)
                    return new RegionInfo(UserHTTPContext.Request.Headers["CF-IPCountry"]).EnglishName;

                return "Other";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.getUserCountryUnknownErrorMessage, GetUserIPAddress.String(UserHTTPContext)),
                    IP = GetUserIPAddress.String(UserHTTPContext),
                    Exception = ex.ToString()
                });
                return "Other";
            }
        }

        public async static Task<string> Parse(string UserIPAddress)
        {
            try
            {

                using (HttpClient HttpClient = new HttpClient())
                {
                    using (HttpResponseMessage HttpResponse = await HttpClient.GetAsync($"{OtherVariable.IPStackAPI}/{UserIPAddress}?access_key={KeysVariable.IPStackAPIKey}"))
                    {
                        if (HttpResponse.StatusCode != HttpStatusCode.OK)
                        {
                            return "Other";
                        }

                        String JsonResponse = await HttpResponse.Content.ReadAsStringAsync();

                        JObject JsonData = JObject.Parse(JsonResponse);
                        if (JsonData["country_name"].ToString() != null)
                        {
                            return JsonData["country_name"].ToString();
                        }
                    }
                }
                return "Other";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.getUserCountryUnknownErrorMessage, UserIPAddress),
                    IP = UserIPAddress,
                    Exception = ex.ToString()
                });
                return "Other";
            }
        }

        public async static Task<bool> IsDifferent(string UsernameIn, string IPIn)
        {
            try
            {
                string Country = await (from User in DB.Queryable<Users>()
                                        where User.Username.Equals(UsernameIn)
                                        select User.Country).FirstOrDefaultAsync();

                string IPCountry = await Parse(IPIn);

                if (String.Equals(Country, IPCountry, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.getUserCountryUnknownErrorMessage, IPIn),
                    IP = IPIn,
                    Exception = ex.ToString()
                });
                return false;
            }
        }
    }

    public class ConfirmUserEmail
    {
        public async static Task<bool> SendCode(string UsernameOrEmail)
        {
            try
            {
                //First update the confirmation code and expiry time
                Guid ConfirmationCode = Guid.NewGuid();

                await DB.Update<Users>()
                    .Match(Filter => Filter.Eq(User => User.Username, UsernameOrEmail) | Filter.Eq(User => User.Email, UsernameOrEmail))
                    .Modify(User => User.ConfirmationCode, ConfirmationCode)
                    .Modify(User => User.ConfirmationCodeExpiryTime, DateTime.Now.AddMinutes(OtherVariable.MinutesToAddInConfirmationCodeExpiryTime))
                    .ExecuteAsync();

                string Email = await (from User in DB.Queryable<Users>()
                                      where (User.Username.Equals(UsernameOrEmail) || User.Email.Equals(UsernameOrEmail))
                                      select User.Country).FirstOrDefaultAsync();

                NetworkCredential AuthenticationDetails = new NetworkCredential()
                {
                    UserName = EmailClientVariable.Username,
                    Password = EmailClientVariable.Password
                };

                using (SmtpClient ServerEmail = new SmtpClient()
                {
                    Host = EmailClientVariable.Host,
                    Port = EmailClientVariable.Port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = EmailClientVariable.UseDefaultCredentials,
                    Credentials = AuthenticationDetails,
                    EnableSsl = EmailClientVariable.UseSSL,
                    Timeout = EmailClientVariable.Timeout
                })
                {
                    MailAddress FromMailAddress = new MailAddress(EmailClientVariable.FromMailAddress, EmailClientVariable.DisplayName);

                    MailMessage MessageToSend = new MailMessage()
                    {
                        Body = String.Format(EmailClientVariable.BodyMessage, ConfirmationCode.ToString()),
                        From = FromMailAddress,
                        Priority = MailPriority.High,
                        Subject = EmailClientVariable.SubjectMessage
                    };

                    MessageToSend.To.Add(Email);
                    ServerEmail.Send(MessageToSend);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.confirmUserEmailSendCodeUnknownErrorMessage, UsernameOrEmail),
                    Exception = ex.ToString()
                });
                return false;
            }
        }
    }

    public class PasswordReset
    {
        public async static Task<bool> SendCode(string UsernameOrEmail)
        {
            try
            {
                //First update the confirmation code and expiry time
                Guid ConfirmationCode = Guid.NewGuid();

                await DB.Update<Users>()
                    .Match(Filter => Filter.Eq(User => User.Username, UsernameOrEmail) | Filter.Eq(User => User.Email, UsernameOrEmail))
                    .Modify(User => User.ConfirmationCode, ConfirmationCode)
                    .Modify(User => User.ConfirmationCodeExpiryTime, DateTime.Now.AddMinutes(OtherVariable.MinutesToAddInConfirmationCodeExpiryTime))
                    .ExecuteAsync();

                //Now extract the details to send the code via email
                string Email = await (from User in DB.Queryable<Users>()
                                      where (User.Username.Equals(UsernameOrEmail) || User.Email.Equals(UsernameOrEmail))
                                      select User.Country).FirstOrDefaultAsync();

                NetworkCredential AuthenticationDetails = new NetworkCredential()
                {
                    UserName = EmailClientVariable.Username,
                    Password = EmailClientVariable.Password
                };

                using (SmtpClient ServerEmail = new SmtpClient()
                {
                    Host = EmailClientVariable.Host,
                    Port = EmailClientVariable.Port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = EmailClientVariable.UseDefaultCredentials,
                    Credentials = AuthenticationDetails,
                    EnableSsl = EmailClientVariable.UseSSL,
                    Timeout = EmailClientVariable.Timeout
                })
                {
                    MailAddress FromMailAddress = new MailAddress(EmailClientVariable.FromMailAddress, EmailClientVariable.DisplayName);

                    MailMessage MessageToSend = new MailMessage()
                    {
                        Body = String.Format(@"
                                                        Confirmation code: {0}
                                                        Either click the below link or input the code manually by visiting http://www.faucet4all.com/#/PasswordReset
                                                        http://www.faucet4all.com/#/PasswordReset?passwordResetCode={0}", ConfirmationCode.ToString()),
                        From = FromMailAddress,
                        Priority = MailPriority.High,
                        Subject = "Reset your password"
                    };

                    MessageToSend.To.Add(Email);
                    ServerEmail.Send(MessageToSend);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.confirmUserEmailSendCodeUnknownErrorMessage, UsernameOrEmail),
                    Exception = ex.ToString()
                });
                return false;
            }
        }
    }

    public class GetUserUsername
    {
        public async static Task<string> String(string SessionId)
        {
            try
            {
                string Username = await (from User in DB.Queryable<Users>()
                                         where User.SessionId.Equals(new Guid(SessionId))
                                         select User.Username).FirstOrDefaultAsync();
                return Username;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = System.String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name, SessionId),
                    Exception = ex.ToString()
                });
                return null;
            }
        }
        public async static Task<bool> Exists(string UsernameIn)
        {
            try
            {
                string Username = await (from User in DB.Queryable<Users>()
                                         where User.Username.Equals(UsernameIn)
                                         select User.Username).FirstOrDefaultAsync();

                if (System.String.IsNullOrEmpty(Username))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = System.String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name, UsernameIn),
                    Exception = ex.ToString()
                });
                return false;
            }
        }
    }

    public class CheatTest
    {
        public async static Task<bool> IsFaucethubAddressExisting(string SessionId, string FaucetHubBitcoinAddressIn)
        {
            try
            {
                string Username = await GetUserUsername.String(SessionId);
                string FaucetHubBitcoinAddress = await (from User in DB.Queryable<Users>()
                                                        where User.FaucetHubBitcoinAddress.Equals(FaucetHubBitcoinAddressIn)
                                                        && !User.Username.Equals(Username)
                                                        select User.FaucetHubBitcoinAddress).FirstOrDefaultAsync();

                if (FaucetHubBitcoinAddress.Equals(FaucetHubBitcoinAddressIn))
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name, SessionId),
                    Exception = ex.ToString()
                });
                return false;
            }
        }

        public async static void CheckMultipleAccount(string SessionId, string DetectedSessionId)
        {
            try
            {
                if (!(String.IsNullOrEmpty(DetectedSessionId) && (String.IsNullOrEmpty(SessionId))))
                {
                    string Username = await (from User in DB.Queryable<Users>()
                                             where User.SessionId.Equals(new Guid(SessionId))
                                             select User.Username).FirstOrDefaultAsync();

                    string OtherUsername = await (from User in DB.Queryable<Users>()
                                                  where User.SessionId.Equals(new Guid(DetectedSessionId))
                                                  select User.Username).FirstOrDefaultAsync();

                    if ((String.IsNullOrEmpty(OtherUsername) && String.IsNullOrEmpty(Username)))
                    {
                        if (!Username.Equals(OtherUsername))
                        {
                            CheatHistory.Insert(Username, new CheatHistoryModel { CheatLevel = 3 });
                            CheatHistory.Insert(OtherUsername, new CheatHistoryModel { CheatLevel = 3 });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name, SessionId),
                    Exception = ex.ToString()
                });
            }
        }
    }

    public class ConvertTo
    {
        public async static Task<double> Bitcoin(string USD)
        {
            try
            {
                using (HttpClient HttpClient = new HttpClient())
                {
                    using (HttpResponseMessage HttpResponse = await HttpClient.GetAsync($"{OtherVariable.BlockchainRateAPI}?currency=USD&value={USD}"))
                    {
                        if (HttpResponse.StatusCode != HttpStatusCode.OK)
                        {
                            throw new Exception();
                        }
                        String Result = await HttpResponse.Content.ReadAsStringAsync();

                        return Convert.ToDouble(Result);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name, USD),
                    Exception = ex.ToString()
                });
                return 0.00000000;
            }
        }

        public static string MD5(string Input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 MD5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] InputBytes = System.Text.Encoding.ASCII.GetBytes(Input);
                byte[] HashBytes = MD5.ComputeHash(InputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder HexadecimalSringBuilder = new StringBuilder();
                for (int i = 0; i < HashBytes.Length; i++)
                {
                    HexadecimalSringBuilder.Append(HashBytes[i].ToString("X2"));
                }
                return HexadecimalSringBuilder.ToString();
            }
        }

        public static string SHA256(string Input)
        {
            // Use input string to calculate SHA256 hash
            // Create a SHA256
            using (SHA256 SHA256Hash = System.Security.Cryptography.SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] ByteArray = SHA256Hash.ComputeHash(Encoding.UTF8.GetBytes(Input));

                // Convert byte array to a string
                StringBuilder Builder = new StringBuilder();
                for (int i = 0; i < ByteArray.Length; i++)
                {
                    Builder.Append(ByteArray[i].ToString("x2"));
                }
                return Builder.ToString();
            }
        }
    }

    public class EmailHandling
    {
        public static bool SendEmail(string Email, string Subject, string Message)
        {
            try
            {
                NetworkCredential AuthenticationDetails = new NetworkCredential()
                {
                    UserName = EmailClientVariable.Username,
                    Password = EmailClientVariable.Password
                };

                using (SmtpClient ServerEmail = new SmtpClient()
                {
                    Host = EmailClientVariable.Host,
                    Port = EmailClientVariable.Port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = EmailClientVariable.UseDefaultCredentials,
                    Credentials = AuthenticationDetails,
                    EnableSsl = EmailClientVariable.UseSSL,
                    Timeout = EmailClientVariable.Timeout
                })
                {
                    MailAddress FromMailAddress = new MailAddress(EmailClientVariable.FromMailAddress, EmailClientVariable.DisplayName);

                    MailMessage MessageToSend = new MailMessage()
                    {
                        Body = Message,
                        From = FromMailAddress,
                        Priority = MailPriority.High,
                        Subject = Subject,
                        IsBodyHtml = false
                    };

                    MessageToSend.To.Add(Email);
                    ServerEmail.Send(MessageToSend);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name, $"Email: {Email}, Subject: {Subject}, Message: {Message}"),
                    Exception = ex.ToString()
                });
                return false;
            }
        }
    }

    public class CheatHistory
    {
        public async static void Insert(string Username, CheatHistoryModel ModelValues)
        {
            try
            {
                int CheatCounter = 0;
                switch (ModelValues.CheatLevel)
                {
                    case 1:
                        ModelValues.Reason = "Trying to claim without visitng link shortners i.e. direct claims (free claims)";
                        CheatCounter = await (from Setting in DB.Queryable<Settings>()
                                              where Setting.SettingsID.Equals(KeysVariable.SettingsKey)
                                              select Setting.CheatCounterForDirectClaims).FirstOrDefaultAsync();
                        break;
                    case 2:
                        ModelValues.Reason = "Using faucethubaddress that is already acquired by another user, possible multi accounts";
                        CheatCounter = await (from Setting in DB.Queryable<Settings>()
                                              where Setting.SettingsID.Equals(KeysVariable.SettingsKey)
                                              select Setting.CheatCounterForDuplicateAccount).FirstOrDefaultAsync();
                        break;
                    case 3:
                        ModelValues.Reason = "Using multiple accounts to login from same computer";
                        CheatCounter = await (from Setting in DB.Queryable<Settings>()
                                              where Setting.SettingsID.Equals(KeysVariable.SettingsKey)
                                              select Setting.CheatCounterForMultipleAccounts).FirstOrDefaultAsync();
                        break;
                    case 4:
                        ModelValues.Reason = "Detected using a proxy";
                        CheatCounter = await (from Setting in DB.Queryable<Settings>()
                                              where Setting.SettingsID.Equals(KeysVariable.SettingsKey)
                                              select Setting.CheatCounterForProxy).FirstOrDefaultAsync();
                        break;
                    default:
                        goto case 1;
                }

                await DB.Update<Users>()
                    .Match(Filter => Filter.Eq(User => User.Username, Username))
                    .Modify(Method => Method.Push(User => User.CheatHistory, ModelValues))
                    .Modify(Method => Method.Inc(User => User.CheatCounter, CheatCounter))
                    .ExecuteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ModelValues)),
                    Exception = ex.ToString()
                });
            }
        }
    }
    public class GetMaxId
    {
        public async static Task<int> RotatorAsync(string Type, int Increment = 0)
        {
            try
            {
                switch (Type)
                {
                    case "Standard":
                        return await StandardRotatorAsync(Increment);
                    case "Square":
                        return await SquareRotatorAsync(Increment);
                    default:
                        return 0 + Increment;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
        public static int Rotator(string Type, int Increment = 0)
        {
            try
            {
                switch (Type)
                {
                    case "Standard":
                        return StandardRotator(Increment);
                    case "Square":
                        return SquareRotator(Increment);
                    default:
                        return 0 + Increment;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
        public async static Task<int> NetworkAsync(string Type, int Increment = 0)
        {
            try
            {
                switch (Type)
                {
                    case "Standard":
                        return await StandardNetworkAsync(Increment);
                    case "Square":
                        return await SquareNetworkAsync(Increment);
                    case "Skyscraper":
                        return await SkyscraperNetworkAsync(Increment);
                    case "PTPStandard":
                        return await PTPStandardNetworkAsync(Increment);
                    case "PTPSquare":
                        return await PTPSquareNetworkAsync(Increment);
                    default:
                        return 0 + Increment;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
        public static int Network(string Type, int Increment = 0)
        {
            try
            {
                switch (Type)
                {
                    case "Standard":
                        return StandardNetwork(Increment);
                    case "Square":
                        return SquareNetwork(Increment);
                    case "Skyscraper":
                        return SkyscraperNetwork(Increment);
                    case "PTPStandard":
                        return PTPStandardNetwork(Increment);
                    case "PTPSquare":
                        return PTPSquareNetwork(Increment);
                    default:
                        return 0 + Increment;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
        public async static Task<int> StandardRotatorAsync(int Increment = 0)
        {
            try
            {
                int MaxId = await DB.Queryable<Settings>()
                   .Where(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                   //For some reason "element" variable here cannot be used in Pascal case as an error is thrown
                   .Select(Setting => Setting.BannerRotator.Count(element => element.Type.Equals("Standard")))
                   .FirstOrDefaultAsync();
                return MaxId + Increment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
        public static int StandardRotator(int Increment = 0)
        {
            try
            {
                int MaxId = DB.Queryable<Settings>()
                   .Where(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                   //For some reason "element" variable here cannot be used in Pascal case as an error is thrown
                   .Select(Setting => Setting.BannerRotator.Count(element => element.Type.Equals("Standard")))
                   .FirstOrDefault();
                return MaxId + Increment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
        public async static Task<int> SquareRotatorAsync(int Increment = 0)
        {
            try
            {
                int MaxId = await DB.Queryable<Settings>()
                   .Where(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                   //For some reason "element" variable here cannot be used in Pascal case as an error is thrown
                   .Select(Setting => Setting.BannerRotator.Count(element => element.Type.Equals("Square")))
                   .FirstOrDefaultAsync();
                return MaxId + Increment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
        public static int SquareRotator(int Increment = 0)
        {
            try
            {
                int MaxId = DB.Queryable<Settings>()
                   .Where(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                   //For some reason "element" variable here cannot be used in Pascal case as an error is thrown
                   .Select(Setting => Setting.BannerRotator.Count(element => element.Type.Equals("Square")))
                   .FirstOrDefault();
                return MaxId + Increment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
        public async static Task<int> StandardNetworkAsync(int Increment = 0)
        {
            try
            {
                int MaxId = await DB.Queryable<Settings>()
                   .Where(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                   //For some reason "element" variable here cannot be used in Pascal case as an error is thrown
                   .Select(Setting => Setting.BannerNetwork.Count(element => element.Type.Equals("Standard")))
                   .FirstOrDefaultAsync();
                return MaxId + Increment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
        public static int StandardNetwork(int Increment = 0)
        {
            try
            {
                int MaxId = DB.Queryable<Settings>()
                   .Where(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                   //For some reason "element" variable here cannot be used in Pascal case as an error is thrown
                   .Select(Setting => Setting.BannerNetwork.Count(element => element.Type.Equals("Standard")))
                   .FirstOrDefault();
                return MaxId + Increment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
        public async static Task<int> SquareNetworkAsync(int Increment = 0)
        {
            try
            {
                int MaxId = await DB.Queryable<Settings>()
                   .Where(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                   //For some reason "element" variable here cannot be used in Pascal case as an error is thrown
                   .Select(Setting => Setting.BannerNetwork.Count(element => element.Type.Equals("Square")))
                   .FirstOrDefaultAsync();
                return MaxId + Increment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
        public static int SquareNetwork(int Increment = 0)
        {
            try
            {
                int MaxId = DB.Queryable<Settings>()
                   .Where(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                   //For some reason "element" variable here cannot be used in Pascal case as an error is thrown
                   .Select(Setting => Setting.BannerNetwork.Count(element => element.Type.Equals("Square")))
                   .FirstOrDefault();
                return MaxId + Increment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
        public async static Task<int> PTPStandardNetworkAsync(int Increment = 0)
        {
            try
            {
                int MaxId = await DB.Queryable<Settings>()
                   .Where(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                   //For some reason "element" variable here cannot be used in Pascal case as an error is thrown
                   .Select(Setting => Setting.BannerNetwork.Count(element => element.Type.Equals("PTPStandard")))
                   .FirstOrDefaultAsync();
                return MaxId + Increment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
        public static int PTPStandardNetwork(int Increment = 0)
        {
            try
            {
                int MaxId = DB.Queryable<Settings>()
                   .Where(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                   //For some reason "element" variable here cannot be used in Pascal case as an error is thrown
                   .Select(Setting => Setting.BannerNetwork.Count(element => element.Type.Equals("PTPStandard")))
                   .FirstOrDefault();
                return MaxId + Increment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
        public async static Task<int> PTPSquareNetworkAsync(int Increment = 0)
        {
            try
            {
                int MaxId = await DB.Queryable<Settings>()
                   .Where(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                   //For some reason "element" variable here cannot be used in Pascal case as an error is thrown
                   .Select(Setting => Setting.BannerNetwork.Count(element => element.Type.Equals("PTPSquare")))
                   .FirstOrDefaultAsync();
                return MaxId + Increment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
        public static int PTPSquareNetwork(int Increment = 0)
        {
            try
            {
                int MaxId = DB.Queryable<Settings>()
                   .Where(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                   //For some reason "element" variable here cannot be used in Pascal case as an error is thrown
                   .Select(Setting => Setting.BannerNetwork.Count(element => element.Type.Equals("PTPSquare")))
                   .FirstOrDefault();
                return MaxId + Increment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
        public async static Task<int> SkyscraperNetworkAsync(int Increment = 0)
        {
            try
            {
                int MaxId = await DB.Queryable<Settings>()
                   .Where(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                   //For some reason "element" variable here cannot be used in Pascal case as an error is thrown
                   .Select(Setting => Setting.BannerNetwork.Count(element => element.Type.Equals("Skyscraper")))
                   .FirstOrDefaultAsync();
                return MaxId + Increment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
        public static int SkyscraperNetwork(int Increment = 0)
        {
            try
            {
                int MaxId = DB.Queryable<Settings>()
                   .Where(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                   //For some reason "element" variable here cannot be used in Pascal case as an error is thrown
                   .Select(Setting => Setting.BannerNetwork.Count(element => element.Type.Equals("Skyscraper")))
                   .FirstOrDefault();
                return MaxId + Increment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name),
                    Exception = ex.ToString()
                });
                return 0 + Increment;
            }
        }
    }
}