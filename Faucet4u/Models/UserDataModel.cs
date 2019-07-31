using Faucet4u.Annotations;
using Faucet4u.GlobalConnections.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet4u.Models
{
    public class UserDataModel
    {
        [Required(ErrorMessage = Session.requiredErrorMessage)]
        [RegularExpression(Session.regularExpression, ErrorMessage = Session.formatErrorMessage)]
        [SessionValid(ErrorMessage = Session.invalidSessionId)]
        [SessionNotExpired(ErrorMessage = Session.expiredErrorMessage)]
        public string sessionId { get; set; }
        public string lsi { get; set; }
        public string requestedData { get; set; }
    }
}
