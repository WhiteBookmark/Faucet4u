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
    public class SupportTicketsController : ControllerBase
    {
        // POST: api/SupportTickets
        [HttpGet]
        public ActionResult Get([FromQuery] SupportTicketsGetModel bodyValue)
        {
            try
            {

                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    dynamic queryResult = connectionObject.QueryFirstOrDefault("Select Subject, Message, DateTime, Username, Locked from SupportTIckets where Reference = @Reference", new { Reference = bodyValue.reference });

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

        // POST: api/SupportTickets
        [HttpPost]
        public ActionResult Post([FromBody] SupportTicketsModel bodyValue)
        {
            try
            {

                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {

                    string queryToExecute = "Insert into SupportTickets(Reference, Subject, Message, Username, Email) Values(@Reference, @Subject, @Message, @Username, @Email)";
                    DynamicParameters paramtersToPass = new DynamicParameters();
                    Guid referenceNumber = Guid.NewGuid();
                    paramtersToPass.Add("Reference", referenceNumber);
                    paramtersToPass.Add("Subject", bodyValue.subject);
                    paramtersToPass.Add("Message", bodyValue.message);
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
                    paramtersToPass.Add("Email", bodyValue.email);

                    if (connectionObject.Execute(queryToExecute, paramtersToPass) > 0)
                    {
                        Log.Info(Guid.NewGuid(), String.Format(Logs.infoMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext));
                        return Ok(new { message = String.Format(SupportTickets.createdSuccessMessage, referenceNumber) });
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
