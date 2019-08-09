using Faucet4u.GlobalConnections;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using API.GlobalConnections.Variable;
using Faucet4u.GlobalConnections.Helper.User;
using API.DatabaseModels;

namespace Faucet4u.Annotations
{
    public class Recaptchav3 : ValidationAttribute
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

                String Recaptchav3Response = Value.ToString();
                String Recaptchav3Secret = Recaptchav3Variable.secretKey;


                using (HttpClient HttpClient = new HttpClient())
                {
                    HttpResponseMessage HttpResponse = HttpClient.GetAsync($"{Recaptchav3Variable.verificationLink}?secret={Recaptchav3Secret}&response={Recaptchav3Response}").Result;

                    if (HttpResponse.StatusCode != HttpStatusCode.OK)
                    {
                        return ErrorResult.Value;
                    }

                    String JsonResponse = HttpResponse.Content.ReadAsStringAsync().Result;
                    JObject JsonData = JObject.Parse(JsonResponse);
                    if (JsonData["success"].ToString().Equals("false", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new Exception(JsonData.ToString());
                    }
                    else if (Convert.ToDouble(JsonData["score"]) < Recaptchav3Variable.minimumScore)
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
                    Message = String.Format(AnnotationsVariable.recaptchav3UnknownErrorMessage, Value),
                    Exception = ex.ToString()
                });
                return ErrorResult.Value;
            }

        }
    }
}
