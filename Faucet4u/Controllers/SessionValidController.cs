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
using Faucet4u.GlobalConnections.Variable;
using Faucet4u.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    string queryToExecute = "Select SessionId from Users where SessionId = @SessionId AND SessionExpiry > getdate()";
                    DynamicParameters paramtersToPass = new DynamicParameters();
                    paramtersToPass.Add("SessionId", bodyValue.sessionId, DbType.String, ParameterDirection.Input);
                    dynamic result = connectionObject.QueryFirstOrDefault(queryToExecute, paramtersToPass);


                    if (result != null)
                    {
                        if (result.SessionId == new Guid(bodyValue.sessionId))
                        {
                            if ((String.IsNullOrEmpty(bodyValue.lsi) != true) && (String.IsNullOrEmpty(bodyValue.sessionId) != true))
                            {
                                CheatTest.CheckMultipleAccount(bodyValue.sessionId, bodyValue.lsi);
                            }
                            return Ok();
                        }
                    }
                    return BadRequest();
                }
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
