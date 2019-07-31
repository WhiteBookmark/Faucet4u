using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Helper.User;
using Faucet4u.GlobalConnections.Variable;
using Faucet4u.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Faucet4u.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OtherController : ControllerBase
    {
        [HttpGet]
        [Route("GetFee")]
        public ActionResult GetFee([FromQuery] OtherModel bodyValue)
        {
            try
            {
                double range1 = 0.00020000;
                double fee1 = 0.00000400;

                double range2 = 0.00050000;
                double fee2 = 0.00000300;

                double range3 = 0.00075000;
                double fee3 = 0.00000200;

                double range4 = 0.00100000;
                double fee4 = 0.00000100;

                double range5 = 0.00250000;
                double fee5 = 0.00000050;

                double range6 = 0.00500000;
                double fee6 = 0.00000025;

                double range7 = 0.01000000;
                double fee7 = 0.00000010;

                double amount = Convert.ToDouble(bodyValue.amount);

                if (amount >= 0 && amount <= range1) return Ok(new { message = fee1 });
                if (amount >= range1 && amount <= range2) return Ok(new { message = fee2 });
                if (amount >= range2 && amount <= range3) return Ok(new { message = fee3 });
                if (amount >= range3 && amount <= range4) return Ok(new { message = fee4 });
                if (amount >= range4 && amount <= range5) return Ok(new { message = fee5 });
                if (amount >= range5 && amount <= range6) return Ok(new { message = fee6 });
                if (amount >= range6 && amount <= range7) return Ok(new { message = fee7 });

                return Ok(new { message = fee7 });
            }
            catch (Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                Log.Error(errorId, String.Format(Logs.unknownErrorMessage, Request.Path.Value, JsonConvert.SerializeObject(bodyValue)), IPinString: GetUserIPAddress.String(this.HttpContext), ExceptionMessage: ex.ToString());
                return BadRequest(new { errors = new { message = new[] { String.Format(UserVariable.unknownErrorMessage, errorId.ToString()) } } });

            }
        }
    }
}