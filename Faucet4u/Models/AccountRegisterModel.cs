using API.GlobalConnections.Variable;
using Faucet4u.Annotations;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = UsernameVariable.requiredErrorMessage)]
        [StringLength(UsernameVariable.maximumLength, MinimumLength = UsernameVariable.minimumLength, ErrorMessage = UsernameVariable.rangeErrorMessage)]
        [RegularExpression(UsernameVariable.regularExpression, ErrorMessage = UsernameVariable.formatErrorMessage)]
        [UsernameAvailable(ErrorMessage = UsernameVariable.alreadyExistsMessage)]
        public string Username { get; set; }

        [Required(ErrorMessage = EmailVariable.RequiredErrorMessage)]
        [DataType(DataType.EmailAddress, ErrorMessage = EmailVariable.FormatErrorMessage)]
        [StringLength(EmailVariable.MaximumLength, MinimumLength = EmailVariable.MinimumLength, ErrorMessage = EmailVariable.RangeErrorMessage)]
        [EmailAvailable(ErrorMessage = EmailVariable.AlreadyExistsMessage)]
        public string Email { get; set; }

        [Required(ErrorMessage = PasswordVariable.requiredErrorMessage)]
        [DataType(DataType.Password)]
        [StringLength(PasswordVariable.maximumLength, MinimumLength = PasswordVariable.minimumLength, ErrorMessage = PasswordVariable.rangeErrorMessage)]
        public string Password { get; set; }

        [Required(ErrorMessage = PasswordVariable.confirmRequiredErrorMessage)]
        [DataType(DataType.Password)]
        [StringLength(PasswordVariable.maximumLength, MinimumLength = PasswordVariable.minimumLength, ErrorMessage = PasswordVariable.rangeErrorMessage)]
        [Compare("Password", ErrorMessage = PasswordVariable.confirmMismatchErrorMessage)]
        public string ConfirmPassword { get; set; }

        public string Referrer { get; set; }

        public string IP { get; set; }
    }
}