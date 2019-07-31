using Faucet4u.Annotations;
using Faucet4u.GlobalConnections.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace Faucet4u.Models
{
    public class SendEmailModel
    {
        [SendEmailKey]
        public string key { get; set; }

        public string usernameOrEmailOrSessionId { get; set; }

        public string subject { get; set; }

        public string message { get; set; }
    }
}
