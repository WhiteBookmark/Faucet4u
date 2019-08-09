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
    public class UsernameExists : ValidationAttribute
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

                string Username = (from User in DB.Queryable<Users>()
                                   where User.Username.Equals(ValueAsString)
                                   select User.Username).FirstOrDefault();

                if (!String.IsNullOrEmpty(Username))
                {
                    if (ValueAsString.Equals(Username, StringComparison.OrdinalIgnoreCase))
                    {
                        return ValidationResult.Success;
                    }
                }


                Log.Hack(new Logs
                {
                    Message = String.Format(LogVariable.usernameExistsUseOfNonExistingUsernameMessage, ValueAsString)
                });
                return ErrorResult.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(AnnotationsVariable.usernameExistsUnknownErrorMessage, ValueAsString),
                    Exception = ex.ToString()
                });
                return ErrorResult.Value;

            }

        }
    }
}
