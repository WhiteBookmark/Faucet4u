using Faucet4u.GlobalConnections;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AnnotationsVariable = Faucet4u.GlobalConnections.Variable.AnnotationsVariable;
using Recaptchav2Variable = Faucet4u.GlobalConnections.Variable.Recaptchav2Variable;

namespace Faucet4u.Annotations
{

    public class Recaptchav2 : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Lazy<ValidationResult> errorResult = new Lazy<ValidationResult>(() => new ValidationResult(ErrorMessage, new String[] { validationContext.MemberName }));

            try
            {
                if (value == null || String.IsNullOrWhiteSpace(value.ToString()))
                {
                    return errorResult.Value;
                }

                String reCaptchav2Response = value.ToString();
                String reCaptchav2Secret = Recaptchav2Variable.secretKey;


                HttpClient httpClient = new HttpClient();
                var httpResponse = httpClient.GetAsync($"{Recaptchav2Variable.verificationLink}?secret={reCaptchav2Secret}&response={reCaptchav2Response}").Result;
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    return errorResult.Value;
                }

                String jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
                JObject jsonData = JObject.Parse(jsonResponse);
                if (jsonData["success"].ToString().Equals("false", StringComparison.OrdinalIgnoreCase))
                {
                    return errorResult.Value;

                }

                return ValidationResult.Success;
            }
            catch (Exception ex)
            {
                Log.Error(Guid.NewGuid(), String.Format(AnnotationsVariable.recaptchav2UnknownErrorMessage, value), ex.ToString());
                return errorResult.Value;
            }

        }
    }
}
