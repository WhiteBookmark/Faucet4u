using Dapper;
using AdminPanel.GlobalConnections.Variable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.GlobalConnections
{
    namespace Helper
    {
        namespace User
        {


            public class Email
            {
                public static bool SendEmail(string email, string subject, string message, string signature = "Admin")
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
