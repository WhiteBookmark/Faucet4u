using Faucet4u.Annotations;
using API.GlobalConnections.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet4u.Models
{
    public class SupportTicketsModel
    {
        //[Required(ErrorMessage = Recaptchav2Variable.requiredErrorMessage)]
        //[Recaptchav2(ErrorMessage = Recaptchav2Variable.invalidMessage)]
        //public string Recaptchav2Response { get; set; }

        //[Required(ErrorMessage = Recaptchav3Variable.requiredErrorMessage)]
        //[Recaptchav3(ErrorMessage = Recaptchav3Variable.invalidMessage)]
        //public string Recaptchav3Response { get; set; }

        public string SessionId { get; set; }

        [Required(ErrorMessage = EmailVariable.RequiredErrorMessage)]
        [DataType(DataType.EmailAddress, ErrorMessage = EmailVariable.FormatErrorMessage)]
        [StringLength(EmailVariable.MaximumLength, MinimumLength = EmailVariable.MinimumLength, ErrorMessage = EmailVariable.RangeErrorMessage)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(70, MinimumLength = 10)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(9999999, MinimumLength = 10)]
        public string Message { get; set; }
    }
}
