using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Faucet4u.Annotations;
using Faucet4u.GlobalConnections.Variable;

namespace Faucet4u.Models
{
    public class PaymentModel
    {
        public string apiKey { get; set; }

        [Required(ErrorMessage = Session.requiredErrorMessage)]
        [RegularExpression(Session.regularExpression, ErrorMessage = Session.formatErrorMessage)]
        [SessionValid]
        public string sessionId { get; set; }

        public string amount { get; set; }

        public string address { get; set; }

        public string transaction { get; set; }

        public string method { get; set; }

    }
}
