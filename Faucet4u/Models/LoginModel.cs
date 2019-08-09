using Faucet4u.Annotations;
using API.GlobalConnections.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet4u.Models
{
    public class LoginModel
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
        [UsernameExists(ErrorMessage = UsernameVariable.doesNotExist)]
        [IsNotLocked(ErrorMessage = UserVariable.accountLocked)]
        public string Username { get; set; }

        [Required(ErrorMessage = PasswordVariable.requiredErrorMessage)]
        [DataType(DataType.Password)]
        [StringLength(PasswordVariable.maximumLength, MinimumLength = PasswordVariable.minimumLength, ErrorMessage = PasswordVariable.rangeErrorMessage)]
        public string Password { get; set; }

        public string LSI { get; set; }

        public string IP { get; set; }
    }
}
