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
    public class BannerRotatorController : ControllerBase
    {

        [HttpGet]
        [Route("TargetLink")]
        public ActionResult GetTargetLink()
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(OtherVariable.SQLConnectionString))
                {
                    string queryToExecute = @"
                                            BEGIN
                                            DECLARE @Id int
                                            DECLARE @TargetLink varchar(max)
                                            SELECT @Id = Id FROM BannerRotatorCounter                                            
                                            SELECT @TargetLink = TargetLink FROM BannerRotator WHERE Id = @Id
                                            UPDATE BannerRotator SET Clicks += 1 WHERE Id = @Id
                                            SELECT TargetLink FROM BannerRotator WHERE Id = @Id
                                            END";
                    dynamic result = connectionObject.QueryFirstOrDefault(queryToExecute);

                    return Redirect(result.TargetLink);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest();

            }
        }

        [HttpGet]
        [Route("ImageLink")]
        public ActionResult GetImageLink()
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(OtherVariable.SQLConnectionString))
                {
                    string queryToExecute = @"
                                            BEGIN
                                            DECLARE @CounterId int
                                            DECLARE @ImageLink varchar(max)
                                            DECLARE @IsTimeBased bit
                                            DECLARE @Credit bigint
                                            SELECT @CounterId = Id FROM BannerRotatorCounter                                            

                                            SELECT @IsTimeBased = IsTimeBased, @Credit = Credit, @ImageLink = ImageLink FROM BannerRotator WHERE Id = @CounterId

                                            IF (@IsTimeBased = 0)
                                            BEGIN
                                            UPDATE BannerRotator SET Impressions += 1, Credit -= 1 WHERE Id = @CounterId	 
                                            END
                                            ELSE IF (@IsTimeBased = 1)
                                            BEGIN
                                            DECLARE @CurrentDateTime DATETIME = getdate()
                                            DECLARE @InitialDateTime DATETIME
                                            SELECT @InitialDateTime = Creation FROM BannerRotator WHERE Id = @CounterId
                                            IF (DATEDIFF(DAY, @InitialDateTime, @CurrentDateTime) >= @Credit)
                                            BEGIN
                                            UPDATE BannerRotator SET Impressions += 1, Credit = 0 WHERE Id = @CounterId
                                            END
                                            ELSE
                                            BEGIN
                                            UPDATE BannerRotator SET Impressions += 1 WHERE Id = @CounterId
                                            END
                                            END

                                            SELECT ImageLink FROM BannerRotator WHERE Id = @CounterId
                                            END";

                    dynamic result = connectionObject.QueryFirstOrDefault(queryToExecute);

                    return Redirect(result.ImageLink);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest();

            }
        }
        [HttpGet]
        [Route("SquareTargetLink")]
        public ActionResult GetSquareTargetLink()
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(OtherVariable.SQLConnectionString))
                {
                    string queryToExecute = @"
                                            BEGIN
                                            DECLARE @Id int
                                            DECLARE @TargetLink varchar(max)
                                            SELECT @Id = Id FROM SquareBannerRotatorCounter                                            
                                            SELECT @TargetLink = TargetLink FROM SquareBannerRotator WHERE Id = @Id
                                            UPDATE SquareBannerRotator SET Clicks += 1 WHERE Id = @Id
                                            SELECT TargetLink FROM SquareBannerRotator WHERE Id = @Id
                                            END";

                    dynamic result = connectionObject.QueryFirstOrDefault(queryToExecute);
                    return Redirect(result.TargetLink);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest();

            }
        }

        [HttpGet]
        [Route("SquareImageLink")]
        public ActionResult GetSquareImageLink()
        {
            try
            {
                using (SqlConnection connectionObject = new SqlConnection(OtherVariable.SQLConnectionString))
                {
                    string queryToExecute = @"
                                            BEGIN
                                            DECLARE @CounterId int
                                            DECLARE @ImageLink varchar(max)
                                            DECLARE @IsTimeBased bit
                                            DECLARE @Credit bigint
                                            SELECT TOP 1 @CounterId = Id FROM SquareBannerRotatorCounter                                            

                                            SELECT @IsTimeBased = IsTimeBased, @Credit = Credit, @ImageLink = ImageLink FROM SquareBannerRotator WHERE Id = @CounterId

                                            IF (@IsTimeBased = 0)
                                            BEGIN
                                            UPDATE SquareBannerRotator SET Impressions += 1, Credit -= 1 WHERE Id = @CounterId	 
                                            END
                                            ELSE IF (@IsTimeBased = 1)
                                            BEGIN
                                            DECLARE @CurrentDateTime DATETIME = getdate()
                                            DECLARE @InitialDateTime DATETIME
                                            SELECT @InitialDateTime = Creation FROM SquareBannerRotator WHERE Id = @CounterId
                                            IF (DATEDIFF(DAY, @InitialDateTime, @CurrentDateTime) >= @Credit)
                                            BEGIN
                                            UPDATE SquareBannerRotator SET Impressions += 1, Credit = 0 WHERE Id = @CounterId
                                            END
                                            ELSE
                                            BEGIN
                                            UPDATE SquareBannerRotator SET Impressions += 1 WHERE Id = @CounterId
                                            END
                                            END

                                            SELECT ImageLink FROM SquareBannerRotator WHERE Id = @CounterId
                                            END";

                    dynamic result = connectionObject.QueryFirstOrDefault(queryToExecute);

                    return Redirect(result.ImageLink);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Redirect("http://moonbitcoin.cash/coin/125x125.gif");

            }
        }
    }
}