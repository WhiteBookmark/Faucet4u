using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Helper.User;
using API.GlobalConnections.Variable;
using Faucet4u.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using API.DatabaseModels;

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

                using (SqlConnection connectionObject = new SqlConnection(OtherVariable.SQLConnectionString))
                {
                    dynamic queryResult = connectionObject.QueryFirstOrDefault("Select Subject, Message, DateTime, Username, Locked from SupportTIckets where Reference = @Reference", new { Reference = bodyValue.Reference });

                    Log.Info(new Logs
                    {
                        Message = String.Format(LogVariable.infoMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)),
                        IP = GetUserIPAddress.String(this.HttpContext),
                    });

                    return Ok(JsonConvert.SerializeObject(queryResult));
                }

                throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Guid errorId = Guid.NewGuid();
                Log.Error(new Logs
                {
                    LogID = errorId,
                    Message = String.Format(LogVariable.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)),
                    IP = GetUserIPAddress.String(this.HttpContext),
                    Exception = ex.ToString()
                });
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });

            }
        }

        // POST: api/SupportTickets
        [HttpPost]
        public ActionResult Post([FromBody] SupportTicketsModel bodyValue)
        {
            try
            {

                using (SqlConnection connectionObject = new SqlConnection(OtherVariable.SQLConnectionString))
                {

                    string queryToExecute = "Insert into SupportTickets(Reference, Subject, Message, Username, Email) Values(@Reference, @Subject, @Message, @Username, @Email)";
                    DynamicParameters paramtersToPass = new DynamicParameters();
                    Guid referenceNumber = Guid.NewGuid();
                    paramtersToPass.Add("Reference", referenceNumber);
                    paramtersToPass.Add("Subject", bodyValue.Subject);
                    paramtersToPass.Add("Message", bodyValue.Message);
                    dynamic usernameFromDatabase = connectionObject.QueryFirstOrDefault("Select Username from Users where SessionId = @SessionId", new { SessionId = bodyValue.SessionId });
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
                    paramtersToPass.Add("Email", bodyValue.Email);

                    if (connectionObject.Execute(queryToExecute, paramtersToPass) > 0)
                    {
                        Log.Info(new Logs
                        {
                            Message = String.Format(LogVariable.infoMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)),
                            IP = GetUserIPAddress.String(this.HttpContext),
                        });

                        return Ok(new { message = String.Format(SupportTicketsVariable.createdSuccessMessage, referenceNumber) });
                    }

                }

                throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Guid errorId = Guid.NewGuid();
                Log.Error(new Logs
                {
                    LogID = errorId,
                    Message = String.Format(LogVariable.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)),
                    IP = GetUserIPAddress.String(this.HttpContext),
                    Exception = ex.ToString()
                });
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });

            }
        }

    }
}
