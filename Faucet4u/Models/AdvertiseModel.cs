using Faucet4u.Annotations;
using Faucet4u.GlobalConnections.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet4u.Models
{
    public class AdvertiseModel
    {
        [Required(ErrorMessage = Session.requiredErrorMessage)]
        [RegularExpression(Session.regularExpression, ErrorMessage = Session.formatErrorMessage)]
        [SessionValid(ErrorMessage = Session.invalidSessionId)]
        [SessionNotExpired(ErrorMessage = Session.expiredErrorMessage)]
        public string sessionId { get; set; }
        public string type { get; set; }
        public bool isTimeBased { get; set; } = false;
        public string reference { get; set; }
        public string name { get; set; } = "PTP";
        public string link { get; set; } = "http://faucet4all.com";
        public string imageLink { get; set; } = "http://kingbtc.co/banners/banner1.gif";
        public string targetLink { get; set; } = "http://kingbtc.co/?ref=admin";
        public int creditInput { get; set; } = 0;
        public string title { get; set; } = "Bonus Ad";
        public string description { get; set; } = "Click to view the Bonus Ad";
    }
}
