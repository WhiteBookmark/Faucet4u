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
    public class PasswordResetController : ControllerBase
    {

        //User put for sending confirmation code for password reset
        [HttpPut]
        public ActionResult Put([FromBody] PasswordResetCodeModel bodyValue)
        {
            try
            {

                bool wasConfirmationCodeSent = PasswordReset.SendCode(bodyValue.email);
                if (wasConfirmationCodeSent == false)
                {
                    throw new Exception();
                }
                Log.Info(Guid.NewGuid(), String.Format(Logs.emailConfirmResendMessage, bodyValue.email), IPinString: GetUserIPAddress.String(this.HttpContext));
                return Ok(new { message = UserVariable.passwordResetCodeMessage });
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.passwordResetCodeUnknownErrorMessage, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString());
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });

            }
        }

        //Use patch for resetting/updating password
        [HttpPatch]
        public ActionResult Patch([FromBody] PasswordResetModel bodyValue)
        {
            try
            {

                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    string queryToExecute = "Select ConfirmationCode from Users where Username = @Username";
                    DynamicParameters paramtersToPass = new DynamicParameters();
                    paramtersToPass.Add("Username", bodyValue.username, DbType.String, ParameterDirection.Input);
                    dynamic resultedConfirmationCode = connectionObject.QueryFirstOrDefault(queryToExecute, paramtersToPass);
                    if (resultedConfirmationCode != null)
                    {
                        if (Guid.Equals(resultedConfirmationCode.ConfirmationCode, new Guid(bodyValue.confirmationCode)))
                        {
                            queryToExecute = "Update Users Set Password = @Password where Username = @Username";
                            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(bodyValue.password);
                            paramtersToPass.Add("@Password", hashedPassword, dbType: DbType.String, direction: ParameterDirection.Input);
                            int affectedRows = connectionObject.Execute(queryToExecute, paramtersToPass);
                            if (Convert.ToBoolean(affectedRows) != true)
                            {
                                throw new Exception();
                            }
                            Log.Info(Guid.NewGuid(), String.Format(Logs.passwordResetMessage, bodyValue.username), IPinString: GetUserIPAddress.String(this.HttpContext));
                            return Ok(new { message = Password.resetSuccessfulMessage });
                        }
                    }
                }

                Log.Hack(Guid.NewGuid(), String.Format(Logs.passwordResetInvalidCodeMessage, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext));
                return BadRequest(new { errors = new { message = new[] { ConfirmationCode.invalidCode } } });
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.passwordResetUnknownErrorMessage, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString());
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });
            }

        }

    }
}
