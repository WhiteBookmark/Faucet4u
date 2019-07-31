using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Faucet4u.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Faucet4u.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get([FromQuery] SessionValidModel bodyValue)
        {
            try
            {
                Console.WriteLine(bodyValue.sessionId);
                Console.WriteLine(this.HttpContext.Request.Headers["sessionId"].ToString());
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());

            }
        }
    }
}