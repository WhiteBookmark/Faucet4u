using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Faucet4u.Annotations;
using API.GlobalConnections.Variable;

namespace Faucet4u.Models
{
    public class PaymentModel
    {
        public string APIKey { get; set; }

        [Required(ErrorMessage = SessionVariable.requiredErrorMessage)]
        [RegularExpression(SessionVariable.regularExpression, ErrorMessage = SessionVariable.formatErrorMessage)]
        [SessionValid]
        public string SessionId { get; set; }

        public string Amount { get; set; }

        public string Address { get; set; }

        public string Transaction { get; set; }

        public string Method { get; set; }

    }
}
