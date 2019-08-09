using Dapper;
using Faucet4u.GlobalConnections;
using API.GlobalConnections.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using API.DatabaseModels;
using System.Reflection;

namespace Faucet4u.Annotations
{
    public class SendEmailKey : ValidationAttribute
    {
        protected override ValidationResult IsValid(object Value, ValidationContext ValidationContextSettings)
        {
            Lazy<ValidationResult> ErrorResult = new Lazy<ValidationResult>(() => new ValidationResult(ErrorMessage, new String[] { ValidationContextSettings.MemberName }));
            string ValueAsString = Convert.ToString(Value);
            try
            {

                if (String.IsNullOrWhiteSpace(ValueAsString))
                {
                    return ErrorResult.Value;
                }


                if (String.Equals(ValueAsString, KeysVariable.SendEmailKey))
                {
                    return ValidationResult.Success;
                }
                Log.Hack(new Logs
                {
                    Message = String.Format(LogVariable.hackAttemptMessage, MethodBase.GetCurrentMethod().Name, ValueAsString)
                });
                return ErrorResult.Value;
            }
            catch (Exception ex)
            {
                Log.Error(new Logs
                {
                    Message = String.Format(LogVariable.unknownErrorMessage, MethodBase.GetCurrentMethod().Name, ValueAsString),
                    Exception = ex.ToString()
                });
                return ErrorResult.Value;

            }

        }
    }
}
