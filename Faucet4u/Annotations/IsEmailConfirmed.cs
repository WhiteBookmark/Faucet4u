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

namespace Faucet4u.Annotations
{
    public class IsEmailConfirmed : ValidationAttribute
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

                bool IsConfirmed = (from User in DB.Queryable<Users>()
                                    where User.Email.Equals(ValueAsString)
                                    select User.IsConfirmed).FirstOrDefault();

                if (IsConfirmed)
                {
                    return ValidationResult.Success;
                }

                Log.Hack(new Logs
                {
                    Message = String.Format(LogVariable.isEmaliConfirmedUseOfNonConfirmedEmailMessage, ValueAsString)
                });
                return ErrorResult.Value;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(AnnotationsVariable.isEmailConfirmedUnknownErrorMessage, Value),
                    Exception = ex.ToString()
                });
                return ErrorResult.Value;

            }

        }
    }
}
