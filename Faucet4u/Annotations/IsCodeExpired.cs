using API.DatabaseModels;
using Faucet4u.GlobalConnections;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using API.GlobalConnections.Variable;

namespace Faucet4u.Annotations
{
    public class IsCodeExpired : ValidationAttribute
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

                DateTime ConfirmationCodeExpiryTime = (from User in DB.Queryable<Users>()
                                                       where User.Email.Equals(ValueAsString) || User.Username.Equals(ValueAsString)
                                                       select User.ConfirmationCodeExpiryTime).FirstOrDefault();

                int ComparisonResult = DateTime.Compare(ConfirmationCodeExpiryTime, DateTime.Now);
                if (ComparisonResult > 0)
                {
                    return ErrorResult.Value;
                }


                return ValidationResult.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(AnnotationsVariable.isCodeExpiredUnknownErrorMessage, Value),
                    Exception = ex.ToString()
                });
                return ErrorResult.Value;
            }
        }
    }
}