using API.DatabaseModels;
using API.GlobalConnections.Variable;
using Faucet4u.GlobalConnections;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Faucet4u.Annotations
{
    public class ReferenceExists : ValidationAttribute
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

                Guid Reference = (from SupportTicket in DB.Queryable<SupportTickets>()
                                  where SupportTicket.Reference.Equals(new Guid(ValueAsString))
                                  select SupportTicket.Reference).FirstOrDefault();

                if (Guid.Equals(Reference, new Guid(ValueAsString)))
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