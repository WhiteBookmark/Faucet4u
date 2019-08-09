using Faucet4u.Annotations;
using API.GlobalConnections.Variable;
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
        public string Recaptchav3Response { get; set; }

        [Required(ErrorMessage = SessionVariable.requiredErrorMessage)]
        [RegularExpression(SessionVariable.regularExpression, ErrorMessage = SessionVariable.formatErrorMessage)]
        [SessionValid(ErrorMessage = SessionVariable.invalidSessionId)]
        [SessionNotExpired(ErrorMessage = SessionVariable.expiredErrorMessage)]
        public string SessionId { get; set; }

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
