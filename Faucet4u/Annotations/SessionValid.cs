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
    public class SessionValid : ValidationAttribute
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
                //Override validation
                if (Guid.Equals(new Guid(Session.overrideValidation), new Guid(valueAsString)))
                {
                    return ValidationResult.Success;
                }
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    string queryToExecute = "Select SessionId from Users where SessionId = @SessionId";
                    dynamic queryResult = connectionObject.QueryFirstOrDefault(queryToExecute, new { SessionId = valueAsString });
                    if (queryResult != null)
                    {
                        if (Guid.Equals(queryResult.SessionId, new Guid(valueAsString)))
                        {
                            return ValidationResult.Success;
                        }
                    }

                }
                Log.Hack(Guid.NewGuid(), String.Format(Logs.hackAttemptMessage, typeof(SessionValid).Name, valueAsString));
                return errorResult.Value;
            }
            catch (Exception ex)
            {
                Log.Error(Guid.NewGuid(), String.Format(Logs.unknownErrorMessage, typeof(SessionValid).Name, valueAsString), ex.ToString());
                return errorResult.Value;

            }

        }
    }
}
