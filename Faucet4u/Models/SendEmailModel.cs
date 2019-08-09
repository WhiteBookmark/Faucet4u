using Faucet4u.Annotations;
using API.GlobalConnections.Variable;
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
        public string Key { get; set; }

        public string UsernameOrEmailOrSessionId { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
    }
}
