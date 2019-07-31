using Faucet4u.GlobalConnections.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Faucet4u.Annotations;

namespace Faucet4u.Models
{
    public class SupportTicketsReplyGetModel
    {
        //[Required(ErrorMessage = Recaptchav2Variable.requiredErrorMessage)]
        //[Recaptchav2(ErrorMessage = Recaptchav2Variable.invalidMessage)]
        //public string recaptchav2Response { get; set; }

        //[Required(ErrorMessage = Recaptchav3Variable.requiredErrorMessage)]
        //[Recaptchav3(ErrorMessage = Recaptchav3Variable.invalidMessage)]
        //public string recaptchav3Response { get; set; }

        [Required(ErrorMessage = "Ticket reference is required")]
        [RegularExpression(ConfirmationCode.regularExpression, ErrorMessage = "Reference format is invalid")]
        public string reference { get; set; }

    }
}
