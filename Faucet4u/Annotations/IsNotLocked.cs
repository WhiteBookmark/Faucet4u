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
    public class IsNotLocked : ValidationAttribute
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

                bool Locked = (from User in DB.Queryable<Users>()
                               where User.Username.Equals(ValueAsString)
                               select User.Locked).FirstOrDefault();

                if (Locked)
                {
                    Log.Hack(new Logs
                    {
                        Message = String.Format(LogVariable.hackAttemptMessage, MethodBase.GetCurrentMethod().Name, ValueAsString)
                    });
                    return ErrorResult.Value;
                }


                return ValidationResult.Success;


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
