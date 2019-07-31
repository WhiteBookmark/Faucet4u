using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Faucet4u.GlobalConnections;
using Faucet4u.GlobalConnections.Helper.User;
using Faucet4u.GlobalConnections.Variable;
using Faucet4u.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Faucet4u.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BannerNetworkController : ControllerBase
    {
        // GET: api/SessionValid
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    //string queryToExecute = @"
                    //                 BEGIN
                    //                 DECLARE @CounterId int
                    //                 SELECT @CounterId = Id FROM BannerNetworkCounter
                    //                 DECLARE @MaxId int
                    //                 SELECT @MaxId = max(Id) FROM BannerNetwork

                    //                 IF(@CounterId IS NULL OR @CounterId = '')
                    //                  BEGIN
                    //                  INSERT INTO BannerNetworkCounter(Id) VALUES(0)
                    //                  END
                    //                    ELSE IF(@CounterId >= @MaxId)
                    //                  BEGIN
                    //                  UPDATE BannerNetworkCounter SET Id = 0
                    //                  END

                    //                    UPDATE BannerNetworkCounter SET Id = Id +1
                    //                 SELECT HTMLCode FROM BannerNetwork WHERE Id = @CounterId                            
                    //                END";

                    string queryToExecute = @"
                                     BEGIN
                                     DECLARE @Id int
                                     SELECT TOP 1 @Id = Id FROM BannerNetwork ORDER BY NEWID()	                                    
                                     SELECT HTMLCode FROM BannerNetwork WHERE Id = @Id                            
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
        [Route("Square")]
        public ActionResult GetSquare()
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    //string queryToExecute = @"
                    //                 BEGIN
                    //                 DECLARE @CounterId int
                    //                 SELECT @CounterId = Id FROM SquareBannerNetworkCounter
                    //                 DECLARE @MaxId int
                    //                 SELECT @MaxId = max(Id) FROM SquareBannerNetwork

                    //                 IF(@CounterId IS NULL OR @CounterId = '')
                    //                  BEGIN
                    //                  INSERT INTO SquareBannerNetworkCounter(Id) VALUES(0)
                    //                  END
                    //                    ELSE IF(@CounterId >= @MaxId)
                    //                  BEGIN
                    //                  UPDATE SquareBannerNetworkCounter SET Id = 0
                    //                  END

                    //                    UPDATE SquareBannerNetworkCounter SET Id = Id +1
                    //                 SELECT HTMLCode FROM SquareBannerNetwork WHERE Id = @CounterId                            
                    //                END";

                    string queryToExecute = @"
                                     BEGIN
                                     DECLARE @Id int
                                     SELECT TOP 1 @Id = Id FROM SquareBannerNetwork ORDER BY NEWID()	                                    
                                     SELECT HTMLCode FROM SquareBannerNetwork WHERE Id = @Id                            
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
        [Route("Skyscraper")]
        public ActionResult GetSkyscraper()
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                {
                    //string queryToExecute = @"
                    //                 BEGIN
                    //                 DECLARE @CounterId int
                    //                 SELECT @CounterId = Id FROM SkyscraperBannerNetworkCounter
                    //                 DECLARE @MaxId int
                    //                 SELECT @MaxId = max(Id) FROM SkyscraperBannerNetwork

                    //                 IF(@CounterId IS NULL OR @CounterId = '')
                    //                  BEGIN
                    //                  INSERT INTO SkyscraperBannerNetworkCounter(Id) VALUES(0)
                    //                  END
                    //                    ELSE IF(@CounterId >= @MaxId)
                    //                  BEGIN
                    //                  UPDATE SkyscraperBannerNetworkCounter SET Id = 0
                    //                  END

                    //                    UPDATE SkyscraperBannerNetworkCounter SET Id = Id +1
                    //                 SELECT HTMLCode FROM SkyscraperBannerNetwork WHERE Id = @CounterId                            
                    //                END";

                    string queryToExecute = @"
                                     BEGIN
                                     DECLARE @Id int
                                     SELECT TOP 1 @Id = Id FROM SkyscraperBannerNetwork ORDER BY NEWID()	                                    
                                     SELECT HTMLCode FROM SkyscraperBannerNetwork WHERE Id = @Id                            
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