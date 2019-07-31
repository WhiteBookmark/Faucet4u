using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Helper.User;
using Faucet4u.GlobalConnections.Variable;
using Faucet4u.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Faucet4u.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get([FromQuery] SendEmailModel bodyValue)
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    dynamic result = connectionObject.QueryFirstOrDefault("SELECT Email FROM Users WHERE Username = @UsernameOrEmailOrSessionId OR Email = @UsernameOrEmailOrSessionId OR SessionId = @UsernameOrEmailOrSessionId", new { UsernameOrEmailOrSessionId = bodyValue.usernameOrEmailOrSessionId });

                    Task.Factory.StartNew(() =>
                            GlobalConnections.Helper.User.Email.SendEmail(result.Email, bodyValue.subject, bodyValue.message));

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest();

            }
        }
    }
}