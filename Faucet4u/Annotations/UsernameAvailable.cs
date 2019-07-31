using Dapper;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Variable;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Faucet4u.Annotations
{
    public class UsernameAvailable : ValidationAttribute
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
                    string queryToExecute = "Select Username from Users where Username = @Username";
                    dynamic queryResult = connectionObject.QueryFirstOrDefault(queryToExecute, new { Username = valueAsString });
                    if (queryResult != null)
                    {
                        string usernameFromQueryResult = queryResult.Username;
                        if (valueAsString.Equals(usernameFromQueryResult, StringComparison.OrdinalIgnoreCase))
                        {
                            Log.Hack(Guid.NewGuid(), String.Format(Logs.usernameAvailableReuseUsernameMessage, valueAsString));
                            return errorResult.Value;
                        }
                    }

                }

                return ValidationResult.Success;
            }
            catch (Exception ex)
            {
                Log.Error(Guid.NewGuid(), String.Format(AnnotationsVariable.usernameAvailableUnknownErrorMessage, valueAsString), ex.ToString());

                return errorResult.Value;

            }

        }

    }
}
