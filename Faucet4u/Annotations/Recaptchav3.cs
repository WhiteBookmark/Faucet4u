using Faucet4u.GlobalConnections;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Recaptchav3Variable = Faucet4u.GlobalConnections.Variable.Recaptchav3Variable;
using AnnotationsVariable = Faucet4u.GlobalConnections.Variable.AnnotationsVariable;
using Faucet4u.GlobalConnections.Helper.User;

namespace Faucet4u.Annotations
{
    public class Recaptchav3 : ValidationAttribute
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

                String reCaptchav3Response = value.ToString();
                String reCaptchav3Secret = Recaptchav3Variable.secretKey;


                HttpClient httpClient = new HttpClient();
                var httpResponse = httpClient.GetAsync($"{Recaptchav3Variable.verificationLink}?secret={reCaptchav3Secret}&response={reCaptchav3Response}").Result;
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    return errorResult.Value;
                }

                String jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
                JObject jsonData = JObject.Parse(jsonResponse);
                if (jsonData["success"].ToString().Equals("false", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception(jsonData.ToString());
                }
                else if (Convert.ToDouble(jsonData["score"]) < Recaptchav3Variable.minimumScore)
                {
                    return errorResult.Value;
                }

                return ValidationResult.Success;
            }
            catch (Exception ex)
            {
                Log.Error(Guid.NewGuid(), String.Format(AnnotationsVariable.recaptchav3UnknownErrorMessage, value), ex.ToString());
                return errorResult.Value;
            }

        }
    }
}
