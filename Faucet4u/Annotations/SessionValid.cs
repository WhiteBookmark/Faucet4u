using API.DatabaseModels;
using API.GlobalConnections.Variable;
using Dapper;
using Faucet4u.GlobalConnections;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Faucet4u.Annotations
{
    public class SessionValid : ValidationAttribute
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
                //Override validation
                if (Guid.Equals(new Guid(SessionVariable.overrideValidation), new Guid(ValueAsString)))
                {
                    return ValidationResult.Success;
                }

                Guid SessionId = (from User in DB.Queryable<Users>()
                                  where User.SessionId.Equals(new Guid(ValueAsString))
                                  select User.SessionId).FirstOrDefault();
                if (Guid.Equals(SessionId, new Guid(ValueAsString)))
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
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(AnnotationsVariable.usernameAvailableUnknownErrorMessage, MethodBase.GetCurrentMethod().Name, ValueAsString),
                    Exception = ex.ToString()
                });
                return ErrorResult.Value;

            }

        }
    }
}
