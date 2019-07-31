using Dapper;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Variable;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using AnnotationsVariable = Faucet4u.GlobalConnections.Variable.AnnotationsVariable;

namespace Faucet4u.Annotations
{
    public class EmailExists : ValidationAttribute
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


                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    string queryToExecute = "Select Email from Users where Email = @Email";
                    dynamic queryResult = connectionObject.QueryFirstOrDefault(queryToExecute, new { Email = valueAsString });
                    if (queryResult != null)
                    {
                        string emailFromQueryResult = queryResult.Email;
                        if (valueAsString.Equals(emailFromQueryResult, StringComparison.OrdinalIgnoreCase))
                        {
                            return ValidationResult.Success;
                        }
                    }

                }
                Log.Hack(Guid.NewGuid(), String.Format(Logs.emailExistsUseOfNonExistingEmailMessage, valueAsString));
                return errorResult.Value;
            }
            catch (Exception ex)
            {
                Log.Error(Guid.NewGuid(), String.Format(AnnotationsVariable.emailExistsUnknownErrorMessage, valueAsString), ex.ToString());
                return errorResult.Value;

            }

        }

    }
}