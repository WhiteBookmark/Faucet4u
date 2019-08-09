using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Helper.User;
using API.GlobalConnections.Variable;
using Faucet4u.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Faucet4u.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PTPBannerNetworkController : ControllerBase
    {
        [HttpGet]
        [Route("Square")]
        public ActionResult GetSquare()
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(OtherVariable.SQLConnectionString))
                {
                    //string queryToExecute = @"
                    //                 BEGIN
                    //                 DECLARE @CounterId int
                    //                 SELECT @CounterId = Id FROM PTPSquareBannerNetworkCounter
                    //                 DECLARE @MaxId int
                    //                 SELECT @MaxId = max(Id) FROM PTPSquareBannerNetwork

                    //                 IF(@CounterId IS NULL OR @CounterId = '')
                    //                  BEGIN
                    //                  INSERT INTO PTPSquareBannerNetworkCounter(Id) VALUES(0)
                    //                  END
                    //                    ELSE IF(@CounterId >= @MaxId)
                    //                  BEGIN
                    //                  UPDATE PTPSquareBannerNetworkCounter SET Id = 0
                    //                  END

                    //                    UPDATE PTPSquareBannerNetworkCounter SET Id = Id +1
                    //                 SELECT HTMLCode FROM PTPSquareBannerNetwork WHERE Id = @CounterId                            
                    //                END";

                    connectionObject.Open();

                    string queryToExecute = @"
                                     BEGIN
                                     DECLARE @Id int
                                     SELECT TOP 1 @Id = Id FROM PTPSquareBannerNetwork ORDER BY NEWID()	                                    
                                     SELECT HTMLCode FROM PTPSquareBannerNetwork WHERE Id = @Id                            
                                     END";

                    dynamic result = connectionObject.QueryFirstOrDefault(queryToExecute);

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest();

            }
        }
        [HttpGet]
        [Route("Standard")]
        public ActionResult GetStandard()
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(OtherVariable.SQLConnectionString))
                {
                    connectionObject.Open();

                    string queryToExecute = @"
                                     BEGIN
                                     DECLARE @Id int
                                     SELECT TOP 1 @Id = Id FROM PTPBannerNetwork ORDER BY NEWID()	                                    
                                     SELECT HTMLCode FROM PTPBannerNetwork WHERE Id = @Id                            
                                     END";

                    dynamic result = connectionObject.QueryFirstOrDefault(queryToExecute);

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest();

            }
        }
    }
}