using Faucet4u.Annotations;
using Faucet4u.GlobalConnections.Helper.User;
using Faucet4u.GlobalConnections.Variable;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet4u.Models
{
    public class PasswordResetModel
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
        [UsernameExists(ErrorMessage = Username.doesNotExist)]
        [IsCodeNotExpired(ErrorMessage = ConfirmationCode.expiredErrorMessage)]
        public string username { get; set; }

        [Required(ErrorMessage = ConfirmationCode.requiredErrorMessage)]
        [RegularExpression(ConfirmationCode.regularExpression, ErrorMessage = ConfirmationCode.formatErrorMessage)]
        public string confirmationCode { get; set; }

        [Required(ErrorMessage = Password.requiredErrorMessage)]
        [DataType(DataType.Password)]
        [StringLength(Password.maximumLength, MinimumLength = Password.minimumLength, ErrorMessage = Password.rangeErrorMessage)]
        public string password { get; set; }

        [Required(ErrorMessage = Password.confirmRequiredErrorMessage)]
        [DataType(DataType.Password)]
        [StringLength(Password.maximumLength, MinimumLength = Password.minimumLength, ErrorMessage = Password.rangeErrorMessage)]
        [Compare("password", ErrorMessage = Password.confirmMismatchErrorMessage)]
        public string confirmPassword { get; set; }


    }
}
