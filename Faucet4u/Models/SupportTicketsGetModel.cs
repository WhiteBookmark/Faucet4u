using Faucet4u.Annotations;
using Faucet4u.GlobalConnections.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet4u.Models
{
    public class SupportTicketsGetModel
    {

        //[Required(ErrorMessage = Recaptchav3Variable.requiredErrorMessage)]
        //[Recaptchav3(ErrorMessage = Recaptchav3Variable.invalidMessage)]
        //public string recaptchav3Response { get; set; }

        [Required(ErrorMessage = "Reference is required")]
        [RegularExpression(ConfirmationCode.regularExpression, ErrorMessage = "Reference format is invalid")]
        [ReferenceExists(ErrorMessage = "No support ticket exists with such reference")]
        public string reference { get; set; }
    }
}
