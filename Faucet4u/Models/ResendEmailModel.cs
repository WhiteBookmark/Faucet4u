using Faucet4u.Annotations;
using API.GlobalConnections.Variable;
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
        public string Recaptchav2Response { get; set; }

        [Required(ErrorMessage = Recaptchav3Variable.requiredErrorMessage)]
        [Recaptchav3(ErrorMessage = Recaptchav3Variable.invalidMessage)]
        public string Recaptchav3Response { get; set; }

        [Required(ErrorMessage = EmailVariable.RequiredErrorMessage)]
        [DataType(DataType.EmailAddress, ErrorMessage = EmailVariable.FormatErrorMessage)]
        [StringLength(EmailVariable.MaximumLength, MinimumLength = EmailVariable.MinimumLength, ErrorMessage = EmailVariable.RangeErrorMessage)]
        [EmailExists(ErrorMessage = EmailVariable.DoesNotExistsMessage)]
        [IsEmailNotConfirmed(ErrorMessage = EmailVariable.AlreadyConfirmedMessage)]
        [IsCodeExpired(ErrorMessage = ConfirmationCodeVariable.notExpiredErrorMessage)]
        public string Email { get; set; }

    }
}
