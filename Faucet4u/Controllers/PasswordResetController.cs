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
using API.DatabaseModels;

namespace Faucet4u.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {

        //User put for sending confirmation code for password reset
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] PasswordResetCodeModel bodyValue)
        {
            try
            {

                bool wasConfirmationCodeSent = await PasswordReset.SendCode(bodyValue.Email);
                if (wasConfirmationCodeSent == false)
                {
                    throw new Exception();
                }
                Log.Info(new Logs
                {
                    Message = String.Format(LogVariable.emailConfirmResendMessage, bodyValue.Email),
                    IP = GetUserIPAddress.String(this.HttpContext),
                });
                return Ok(new { message = UserVariable.passwordResetCodeMessage });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Guid errorId = Guid.NewGuid();
                Log.Error(new Logs
                {
                    LogID = errorId,
                    Message = String.Format(LogVariable.passwordResetCodeUnknownErrorMessage, JsonConvert.SerializeObject(bodyValue)),
                    IP = GetUserIPAddress.String(this.HttpContext),
                    Exception = ex.ToString()
                });
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });

            }
        }

        //Use patch for resetting/updating password
        [HttpPatch]
        public ActionResult Patch([FromBody] PasswordResetModel bodyValue)
        {
            try
            {

                using (SqlConnection connectionObject = new SqlConnection(OtherVariable.SQLConnectionString))
                {
                    string queryToExecute = "Select ConfirmationCode from Users where Username = @Username";
                    DynamicParameters paramtersToPass = new DynamicParameters();
                    paramtersToPass.Add("Username", bodyValue.Username, DbType.String, ParameterDirection.Input);
                    dynamic resultedConfirmationCode = connectionObject.QueryFirstOrDefault(queryToExecute, paramtersToPass);
                    if (resultedConfirmationCode != null)
                    {
                        if (Guid.Equals(resultedConfirmationCode.ConfirmationCode, new Guid(bodyValue.ConfirmationCode)))
                        {
                            queryToExecute = "Update Users Set Password = @Password where Username = @Username";
                            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(bodyValue.Password);
                            paramtersToPass.Add("@Password", hashedPassword, dbType: DbType.String, direction: ParameterDirection.Input);
                            int affectedRows = connectionObject.Execute(queryToExecute, paramtersToPass);
                            if (Convert.ToBoolean(affectedRows) != true)
                            {
                                throw new Exception();
                            }
                            Log.Info(new Logs
                            {
                                Message = String.Format(LogVariable.passwordResetMessage, bodyValue.Username),
                                IP = GetUserIPAddress.String(this.HttpContext),
                            });
                            return Ok(new { message = PasswordVariable.resetSuccessfulMessage });
                        }
                    }
                }

                Log.Hack(new Logs
                {
                    Message = String.Format(LogVariable.passwordResetInvalidCodeMessage, JsonConvert.SerializeObject(bodyValue)),
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
                    Message = String.Format(LogVariable.passwordResetUnknownErrorMessage, JsonConvert.SerializeObject(bodyValue)),
                    IP = GetUserIPAddress.String(this.HttpContext),
                    Exception = ex.ToString()
                });
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });
            }

        }

    }
}
