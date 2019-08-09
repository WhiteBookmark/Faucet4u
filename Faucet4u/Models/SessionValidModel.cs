using Faucet4u.Annotations;
using API.GlobalConnections.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet4u.Models
{
    public class SessionValidModel
    {

        [Required(ErrorMessage = SessionVariable.requiredErrorMessage)]
        [RegularExpression(SessionVariable.regularExpression, ErrorMessage = SessionVariable.formatErrorMessage)]
        [SessionValid(ErrorMessage = SessionVariable.invalidSessionId)]
        [SessionNotExpired(ErrorMessage = SessionVariable.expiredErrorMessage)]
        public string SessionId { get; set; }

        public string LSI { get; set; }
    }
}
