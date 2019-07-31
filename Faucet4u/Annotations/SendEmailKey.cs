using Dapper;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet4u.Annotations
{
    public class SendEmailKey : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Lazy<ValidationResult> errorResult = new Lazy<ValidationResult>(() => new ValidationResult(ErrorMessage, new String[] { validationContext.MemberName }));
            string valueAsString = Convert.ToString(value);
            try
            {


                if (String.IsNullOrWhiteSpace(valueAsString))
                {
                    return errorResult.Value;
                }


                if (String.Equals(valueAsString, Other.sendEmailKey))
                {
                    return ValidationResult.Success;
                }

                Log.Hack(Guid.NewGuid(), String.Format(Logs.hackAttemptMessage, typeof(SendEmailKey).Name, valueAsString));
                return errorResult.Value;
            }
            catch (Exception ex)
            {
                Log.Error(Guid.NewGuid(), String.Format(Logs.unknownErrorMessage, typeof(SendEmailKey).Name, valueAsString), ex.ToString());
                return errorResult.Value;

            }

        }
    }
}
