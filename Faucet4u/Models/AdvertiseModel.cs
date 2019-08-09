using Faucet4u.Annotations;
using API.GlobalConnections.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet4u.Models
{
    public class AdvertiseModel
    {
        [Required(ErrorMessage = SessionVariable.requiredErrorMessage)]
        [RegularExpression(SessionVariable.regularExpression, ErrorMessage = SessionVariable.formatErrorMessage)]
        [SessionValid(ErrorMessage = SessionVariable.invalidSessionId)]
        [SessionNotExpired(ErrorMessage = SessionVariable.expiredErrorMessage)]
        public string SessionId { get; set; }
        public string Type { get; set; }
        public bool IsTimeBased { get; set; } = false;
        public string Reference { get; set; }
        public string Name { get; set; } = "PTP";
        public string Link { get; set; } = "http://faucet4all.com";
        public string ImageLink { get; set; } = "http://kingbtc.co/banners/banner1.gif";
        public string TargetLink { get; set; } = "http://kingbtc.co/?ref=admin";
        public int CreditInput { get; set; } = 0;
        public string Title { get; set; } = "Bonus Ad";
        public string Description { get; set; } = "Click to view the Bonus Ad";
    }
}
