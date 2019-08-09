using System;
using System.Collections.Generic;
using System.Data;
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
using Newtonsoft.Json.Linq;
using API.DatabaseModels;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Faucet4u.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailConfirmController : ControllerBase
    {
        //Use patch for confirming email
        [HttpPatch]
        public ActionResult Patch([FromBody] EmailConfirmModel bodyValue)
        {
            try
            {
                //if (GetUserCountry.Compare(bodyValue.email, this.HttpContext) == false)
                //{
                //    Log.Hack(Guid.NewGuid(), String.Format(Logs.countryDifferentMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext));
                //    return BadRequest(new { message = UserVariable.countryDifferentMessage });
                //}

                using (SqlConnection connectionObject = new SqlConnection(OtherVariable.SQLConnectionString))
                {
                    string queryToExecute = "Select ConfirmationCode from Users where Email = @Email";
                    DynamicParameters paramtersToPass = new DynamicParameters();
                    paramtersToPass.Add("Email", bodyValue.Email, DbType.String, ParameterDirection.Input);
                    dynamic resultedConfirmationCode = connectionObject.QueryFirstOrDefault(queryToExecute, paramtersToPass);
                    if (resultedConfirmationCode != null)
                    {
                        if (Guid.Equals(resultedConfirmationCode.ConfirmationCode, new Guid(bodyValue.ConfirmationCode)))
                        {
                            queryToExecute = "Update Users Set IsConfirmed = 1 where Email = @Email";
                            int affectedRows = connectionObject.Execute(queryToExecute, paramtersToPass);
                            if (Convert.ToBoolean(affectedRows) != true)
                            {
                                throw new Exception();
                            }
                            Log.Info(new Logs
                            {
                                Message = String.Format(LogVariable.userConfirmedEmailMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)),
                                IP = GetUserIPAddress.String(this.HttpContext)
                            });
                            return Ok(new { message = UserVariable.emailConfirmSuccessfulMessage });
                        }
                    }
                }

                Log.Hack(new Logs
                {
                    Message = String.Format(LogVariable.emailConfirmInvalidConfirmationCodeMessage, JsonConvert.SerializeObject(bodyValue)),
                    IP = GetUserIPAddress.String(this.HttpContext)
                });
                return BadRequest(new { errors = new { message = new[] { ConfirmationCodeVariable.invalidCode } } });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Guid errorId = Guid.NewGuid();
                Log.Error(new Logs
                {
                    LogID = errorId,
                    Message = String.Format(LogVariable.emailConfirmUnknownErrorMessage, JsonConvert.SerializeObject(bodyValue)),
                    IP = GetUserIPAddress.String(this.HttpContext),
                    Exception = ex.ToString()
                });
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });
            }

        }

        //Use put for resending confirmation code for email
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] ResendEmailModel bodyValue)
        {
            try
            {
                //if (GetUserCountry.Compare(bodyValue.email, this.HttpContext) == false)
                //{
                //    Log.Hack(Guid.NewGuid(), String.Format(Logs.countryDifferentMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext));
                //    return BadRequest(new { message = UserVariable.countryDifferentMessage });
                //}

                bool wasConfirmationCodeSent = await ConfirmUserEmail.SendCode(bodyValue.Email);
                if (wasConfirmationCodeSent == false)
                {
                    throw new Exception();
                }
                Log.Info(new Logs
                {
                    Message = String.Format(LogVariable.emailConfirmResendMessage, Request.Path.Value, bodyValue.Email),
                    IP = GetUserIPAddress.String(this.HttpContext)
                });
                return Ok(new { message = UserVariable.emailConfirmResendMessage });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Guid errorId = Guid.NewGuid();
                Log.Error(new Logs
                {
                    LogID = errorId,
                    Message = String.Format(LogVariable.emailConfirmResendEmailUnknownErrorMessage, JsonConvert.SerializeObject(bodyValue)),
                    IP = GetUserIPAddress.String(this.HttpContext),
                    Exception = ex.ToString()
                });
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });

            }
        }
    }
}
