using API.DatabaseModels;
using API.GlobalConnections.Variable;
using Faucet4u.GlobalConnections;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;

namespace Faucet4u.Annotations
{
    public class Recaptchav2 : ValidationAttribute
    {
        protected override ValidationResult IsValid(object Value, ValidationContext ValidationContextSettings)
        {
            Lazy<ValidationResult> ErrorResult = new Lazy<ValidationResult>(() => new ValidationResult(ErrorMessage, new String[] { ValidationContextSettings.MemberName }));

            try
            {
                if (String.IsNullOrEmpty(Value.ToString()) || String.IsNullOrWhiteSpace(Value.ToString()))
                {
                    return ErrorResult.Value;
                }

                string Recaptchav2Response = Value.ToString();
                string Recaptchav2Secret = Recaptchav2Variable.secretKey;

                using (HttpClient HttpClient = new HttpClient())
                {
                    HttpResponseMessage HttpResponse = HttpClient.GetAsync($"{Recaptchav2Variable.verificationLink}?secret={Recaptchav2Secret}&response={Recaptchav2Response}").Result;
                    if (HttpResponse.StatusCode != HttpStatusCode.OK)
                    {
                        return ErrorResult.Value;
                    }

                    String JsonResponse = HttpResponse.Content.ReadAsStringAsync().Result;
                    JObject JsonData = JObject.Parse(JsonResponse);
                    if (JsonData["success"].ToString().Equals("false", StringComparison.OrdinalIgnoreCase))
                    {
                        return ErrorResult.Value;
                    }
                }
                return ValidationResult.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(new Logs
                {
                    Message = String.Format(AnnotationsVariable.recaptchav2UnknownErrorMessage, Value),
                    Exception = ex.ToString()
                });
                return ErrorResult.Value;
            }
        }
    }
}