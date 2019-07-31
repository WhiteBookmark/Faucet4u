using Faucet4u.Annotations;
using Faucet4u.GlobalConnections.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet4u.Models
{
    public class SettingsPutModel
    {
        [Required(ErrorMessage = Recaptchav3Variable.requiredErrorMessage)]
        [Recaptchav3(ErrorMessage = Recaptchav3Variable.invalidMessage)]
        public string recaptchav3Response { get; set; }

        [Required(ErrorMessage = Session.requiredErrorMessage)]
        [RegularExpression(Session.regularExpression, ErrorMessage = Session.formatErrorMessage)]
        [SessionValid(ErrorMessage = Session.invalidSessionId)]
        [SessionNotExpired(ErrorMessage = Session.expiredErrorMessage)]
        public string sessionId { get; set; }

        [Required]
        public string FaucetHubBitcoinAddress { get; set; }
        //public string BitcoinAddress { get; set; }
        //public string PerfectMoneyAddress { get; set; }
        //public string PayeerAddress { get; set; }
        //public string AdvCashAddress { get; set; }
        //public string EthereumAddress { get; set; }
        //public string BitcoinCashAddress { get; set; }
        //public string DogecoinAddress { get; set; }
        //public string DashAddress { get; set; }
        //public string ZCashAddress { get; set; }
        //public string LitecoinAddress { get; set; }
        //public string EthereumClassicAddress { get; set; }
        //public string PeercoinAddress { get; set; }
    }
}
