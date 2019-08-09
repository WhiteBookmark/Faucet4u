using API.DatabaseModels;
using API.GlobalConnections.Variable;
using Faucet4u.GlobalConnections;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Faucet4u.Annotations
{
    public class IsCodeNotExpired : ValidationAttribute
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
                    return ValidationResult.Success;
                }


                return ErrorResult.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(AnnotationsVariable.isCodeNotExpiredUnknownErrorMessage, Value),
                    Exception = ex.ToString()
                });
                return ErrorResult.Value;
            }
        }
    }
}