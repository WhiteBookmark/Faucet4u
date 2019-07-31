using System;
using System.Collections.Generic;
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
    public class SupportTicketsReplyController : ControllerBase
    {

        // Get: api/SupportTicketsReply
        [HttpGet]
        public ActionResult Get([FromQuery] SupportTicketsReplyGetModel bodyValue)
        {
            try
            {

                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    var queryResult = connectionObject.Query("Select Reply, Username, DateTime from SupportTicketsReply where Reference = @Reference", new { Reference = bodyValue.reference });

                    Log.Info(Guid.NewGuid(), String.Format(Logs.infoMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext));
                    return Ok(JsonConvert.SerializeObject(queryResult));
                }

                throw new Exception();
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString());
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });

            }
        }

        // POST: api/SupportTicketsReply
        [HttpPost]
        public ActionResult Post([FromBody] SupportTicketsReplyModel bodyValue)
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {

                    string queryToExecute = "Insert into SupportTicketsReply(Reference, Reply, Username) Values(@Reference, @Reply, @Username)";
                    DynamicParameters paramtersToPass = new DynamicParameters();

                    paramtersToPass.Add("Reference", bodyValue.reference);
                    paramtersToPass.Add("Reply", bodyValue.reply);

                    dynamic usernameFromDatabase = connectionObject.QueryFirstOrDefault("Select Username from Users where SessionId = @SessionId", new { SessionId = bodyValue.sessionId });
                    if (usernameFromDatabase != null)
                    {
                        if (usernameFromDatabase.Username != null)
                        {
                            paramtersToPass.Add("Username", usernameFromDatabase.Username);
                        }
                        else
                        {
                            paramtersToPass.Add("Username", null);
                        }
                    }
                    else
                    {
                        paramtersToPass.Add("Username", null);
                    }

                    if (connectionObject.Execute(queryToExecute, paramtersToPass) > 0)
                    {
                        Log.Info(Guid.NewGuid(), String.Format(Logs.infoMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext));
                        return Ok(new { message = "Reply successful" });
                    }

                }
                throw new Exception();
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString());
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });

            }
        }
    }
}
