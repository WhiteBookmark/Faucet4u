using Faucet4u.Annotations;
using Faucet4u.GlobalConnections.Variable;
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

        [Required(ErrorMessage = Username.requiredErrorMessage)]
        [StringLength(Username.maximumLength, MinimumLength = Username.minimumLength, ErrorMessage = Username.rangeErrorMessage)]
        [RegularExpression(Username.regularExpression, ErrorMessage = Username.formatErrorMessage)]
        [UsernameExists(ErrorMessage = Username.doesNotExist)]
        [IsNotLocked(ErrorMessage = UserVariable.accountLocked)]
        public string username { get; set; }

        [Required(ErrorMessage = Password.requiredErrorMessage)]
        [DataType(DataType.Password)]
        [StringLength(Password.maximumLength, MinimumLength = Password.minimumLength, ErrorMessage = Password.rangeErrorMessage)]
        public string password { get; set; }

        public string lsi { get; set; }

        public string ip { get; set; }
    }
}
