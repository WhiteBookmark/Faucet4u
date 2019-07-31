using Dapper;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Helper.User;
using Faucet4u.GlobalConnections.Variable;
using Faucet4u.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Faucet4u.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetAsync([FromQuery] UserDataModel bodyValue)
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    IEnumerable<dynamic> result = await connectionObject.QueryAsync(@"

SELECT
CASE @RequestedData
WHEN 1 THEN 'First'
WHEN 2 THEN 'Second'
WHEN 3 THEN 'Third'
ELSE 'Other'
END", new
                    {
                        Username = await GetUserUsername.String(bodyValue.sessionId),
                        RequestedData = bodyValue.requestedData
                    });

                    Log.Info(Guid.NewGuid(), String.Format(Logs.infoMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext));
                    return Ok(JsonConvert.SerializeObject(result));
                }
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString());
                return BadRequest(new
                {
                    errors = new
                    {
                        message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) }
                    }
                });

            }
        }
    }
}