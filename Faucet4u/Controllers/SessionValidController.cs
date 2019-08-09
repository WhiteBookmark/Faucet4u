using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
    public class SessionValidController : ControllerBase
    {

        // GET: api/SessionValid
        [HttpGet]
        public ActionResult Get([FromQuery] SessionValidModel bodyValue)
        {
            try
            {
                //if (GetUserCountry.Compare(new Guid(bodyValue.sessionId), this.HttpContext) == false)
                //{
                //    Log.Hack(Guid.NewGuid(), String.Format(Logs.countryDifferentMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext));
                //    return BadRequest(new { message = UserVariable.countryDifferentMessage });
                //}

                using (SqlConnection connectionObject = new SqlConnection(OtherVariable.SQLConnectionString))
                {
                    string queryToExecute = "Select SessionId from Users where SessionId = @SessionId AND SessionExpiry > getdate()";
                    DynamicParameters paramtersToPass = new DynamicParameters();
                    paramtersToPass.Add("SessionId", bodyValue.SessionId, DbType.String, ParameterDirection.Input);
                    dynamic result = connectionObject.QueryFirstOrDefault(queryToExecute, paramtersToPass);


                    if (result != null)
                    {
                        if (result.SessionId == new Guid(bodyValue.SessionId))
                        {
                            if ((String.IsNullOrEmpty(bodyValue.LSI) != true) && (String.IsNullOrEmpty(bodyValue.SessionId) != true))
                            {
                                CheatTest.CheckMultipleAccount(bodyValue.SessionId, bodyValue.LSI);
                            }
                            return Ok();
                        }
                    }
                    return BadRequest();
                }
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
