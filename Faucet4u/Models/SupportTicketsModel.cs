using Faucet4u.Annotations;
using Faucet4u.GlobalConnections.Variable;
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
        //public string recaptchav2Response { get; set; }

        //[Required(ErrorMessage = Recaptchav3Variable.requiredErrorMessage)]
        //[Recaptchav3(ErrorMessage = Recaptchav3Variable.invalidMessage)]
        //public string recaptchav3Response { get; set; }

        public string sessionId { get; set; }

        [Required(ErrorMessage = Email.requiredErrorMessage)]
        [DataType(DataType.EmailAddress, ErrorMessage = Email.formatErrorMessage)]
        [StringLength(Email.maximumLength, MinimumLength = Email.minimumLength, ErrorMessage = Email.rangeErrorMessage)]
        public string email { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(70, MinimumLength = 10)]
        public string subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(9999999, MinimumLength = 10)]
        public string message { get; set; }
    }
}
