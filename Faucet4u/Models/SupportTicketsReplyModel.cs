using Faucet4u.Annotations;
using Faucet4u.GlobalConnections.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet4u.Models
{
    public class SupportTicketsReplyModel
    {
        //[Required(ErrorMessage = Recaptchav2Variable.requiredErrorMessage)]
        //[Recaptchav2(ErrorMessage = Recaptchav2Variable.invalidMessage)]
        //public string recaptchav2Response { get; set; }

        //[Required(ErrorMessage = Recaptchav3Variable.requiredErrorMessage)]
        //[Recaptchav3(ErrorMessage = Recaptchav3Variable.invalidMessage)]
        //public string recaptchav3Response { get; set; }

        public string sessionId { get; set; }

        [Required(ErrorMessage = "Ticket reference is required")]
        [RegularExpression(ConfirmationCode.regularExpression, ErrorMessage = "Reference format is invalid")]
        public string reference { get; set; }

        [Required(ErrorMessage = "Reply message is required")]
        [StringLength(9999999, MinimumLength = 10)]
        public string reply { get; set; }
    }
}
