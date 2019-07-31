using Faucet4u.Annotations;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Variable;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Recaptchav2 = Faucet4u.Annotations.Recaptchav2;
using Recaptchav2Variable = Faucet4u.GlobalConnections.Variable.Recaptchav2Variable;
using Recaptchav3 = Faucet4u.Annotations.Recaptchav3;
using Recaptchav3Variable = Faucet4u.GlobalConnections.Variable.Recaptchav3Variable;

namespace Faucet4u.Models
{
    public class AccountRegisterModel
    {
        //[Required(ErrorMessage = Recaptchav2Variable.requiredErrorMessage)]
        //[Recaptchav2(ErrorMessage = Recaptchav2Variable.invalidMessage)]
        //public string recaptchav2Response { get; set; }

        //[Required(ErrorMessage = Recaptchav3Variable.requiredErrorMessage)]
        //[Recaptchav3(ErrorMessage = Recaptchav3Variable.invalidMessage)]
        //public string recaptchav3Response { get; set; }

        [Required(ErrorMessage = Username.requiredErrorMessage)]
        [StringLength(Username.maximumLength, MinimumLength = Username.minimumLength, ErrorMessage = Username.rangeErrorMessage)]
        [RegularExpression(Username.regularExpression, ErrorMessage = Username.formatErrorMessage)]
        [UsernameAvailable(ErrorMessage = Username.alreadyExistsMessage)]
        public string username { get; set; }

        [Required(ErrorMessage = Email.requiredErrorMessage)]
        [DataType(DataType.EmailAddress, ErrorMessage = Email.formatErrorMessage)]
        [StringLength(Email.maximumLength, MinimumLength = Email.minimumLength, ErrorMessage = Email.rangeErrorMessage)]
        [EmailAvailable(ErrorMessage = Email.alreadyExistsMessage)]
        public string email { get; set; }

        [Required(ErrorMessage = Password.requiredErrorMessage)]
        [DataType(DataType.Password)]
        [StringLength(Password.maximumLength, MinimumLength = Password.minimumLength, ErrorMessage = Password.rangeErrorMessage)]
        public string password { get; set; }

        [Required(ErrorMessage = Password.confirmRequiredErrorMessage)]
        [DataType(DataType.Password)]
        [StringLength(Password.maximumLength, MinimumLength = Password.minimumLength, ErrorMessage = Password.rangeErrorMessage)]
        [Compare("password", ErrorMessage = Password.confirmMismatchErrorMessage)]
        public string confirmPassword { get; set; }

        public string referrer { get; set; }

        public string ip { get; set; }

    }
}
