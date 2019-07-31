using Dapper;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet4u.Annotations
{
    public class IsEmailNotConfirmed : ValidationAttribute
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

                using (SqlConnection connection = new SqlConnection(Other.SQLConnectionString))
                {
                    string queryToExecute = "Select IsConfirmed from Users where Email = @Email";
                    DynamicParameters parametersToAdd = new DynamicParameters();
                    parametersToAdd.Add("Email", valueAsString, DbType.String, ParameterDirection.Input);
                    dynamic returnedResult = connection.QueryFirstOrDefault(queryToExecute, parametersToAdd);
                    if (returnedResult != null)
                    {
                        if (returnedResult.IsConfirmed)
                        {
                            Log.Hack(Guid.NewGuid(), String.Format(Logs.isEmailNotConfirmedReconfirmingMessaege, valueAsString));
                            return errorResult.Value;
                        }
                    }

                    return ValidationResult.Success;
                }

            }
            catch (Exception ex)
            {
                Log.Error(Guid.NewGuid(), String.Format(AnnotationsVariable.isEmailNotConfirmedUnknownErrorMessage, valueAsString), ex.ToString());
                return errorResult.Value;

            }

        }

    }
}
