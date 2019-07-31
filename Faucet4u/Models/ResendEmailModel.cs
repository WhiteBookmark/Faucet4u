using Faucet4u.Annotations;
using Faucet4u.GlobalConnections.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet4u.Models
{
    public class ResendEmailModel
    {
        [Required(ErrorMessage = Recaptchav2Variable.requiredErrorMessage)]
        [Recaptchav2(ErrorMessage = Recaptchav2Variable.invalidMessage)]
        public string recaptchav2Response { get; set; }

        [Required(ErrorMessage = Recaptchav3Variable.requiredErrorMessage)]
        [Recaptchav3(ErrorMessage = Recaptchav3Variable.invalidMessage)]
        public string recaptchav3Response { get; set; }

        [Required(ErrorMessage = Email.requiredErrorMessage)]
        [DataType(DataType.EmailAddress, ErrorMessage = Email.formatErrorMessage)]
        [StringLength(Email.maximumLength, MinimumLength = Email.minimumLength, ErrorMessage = Email.rangeErrorMessage)]
        [EmailExists(ErrorMessage = Email.doesNotExistsMessage)]
        [IsEmailNotConfirmed(ErrorMessage = Email.alreadyConfirmedMessage)]
        [IsCodeExpired(ErrorMessage = ConfirmationCode.notExpiredErrorMessage)]
        public string email { get; set; }

    }
}
