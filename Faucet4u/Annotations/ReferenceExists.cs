using Dapper;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Variable;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using AnnotationsVariable = Faucet4u.GlobalConnections.Variable.AnnotationsVariable;

namespace Faucet4u.Annotations
{
    public class ReferenceExists : ValidationAttribute
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
                    string queryToExecute = "Select Reference from SupportTickets where Reference = @Reference";
                    dynamic queryResult = connectionObject.QueryFirstOrDefault(queryToExecute, new { Reference = valueAsString });
                    if (queryResult != null)
                    {
                        Guid referenceFromTable = queryResult.Reference;
                        if (Guid.Equals(referenceFromTable, new Guid(valueAsString)))
                        {
                            return ValidationResult.Success;
                        }
                    }

                }
                Log.Hack(Guid.NewGuid(), String.Format(Logs.hackAttemptMessage, typeof(ReferenceExists).Name, valueAsString));
                return errorResult.Value;
            }
            catch (Exception ex)
            {
                Log.Error(Guid.NewGuid(), String.Format(Logs.unknownErrorMessage, typeof(ReferenceExists).Name, valueAsString), ex.ToString());
                return errorResult.Value;

            }

        }

    }
}