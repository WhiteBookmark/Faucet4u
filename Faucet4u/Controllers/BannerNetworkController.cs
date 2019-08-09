using API.DatabaseModels;
using API.GlobalConnections.Variable;
using Dapper;
using Faucet4u.GlobalConnections.Helper.User;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Entities;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet4u.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BannerNetworkController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                int CounterId = await DB.Queryable<Records>()
                    .Where(Record => Record.RecordsID.Equals(KeysVariable.RecordsKey))
                    .Select(Record => Record.StandardBannerNetworkCounter)
                    .FirstOrDefaultAsync();
                int MaxId = await GetMaxId.StandardNetworkAsync();
                if (CounterId >= MaxId)
                {
                    await DB.Update<Records>()
                        .Match(Record => Record.RecordsID.Equals(KeysVariable.RecordsKey))
                        .Modify(Filter => Filter.Set(Record => Record.StandardBannerNetworkCounter, 1))
                        .ExecuteAsync();
                    CounterId = 1;
                }
                else
                {
                    await DB.Update<Records>()
                        .Match(Record => Record.RecordsID.Equals(KeysVariable.RecordsKey))
                        .Modify(Filter => Filter.Inc(Record => Record.StandardBannerNetworkCounter, 1))
                        .ExecuteAsync();
                }

                var HTMLCode = await DB.Queryable<Settings>()
                    .Where(Setting => Setting.SettingsID.Equals(KeysVariable.SettingsKey))
                    .Select(Setting => Setting.BannerNetwork.Where(element => element.Id.Equals(CounterId))
                    .Select(code => code.HTMLCode)
                    .First())
                    .FirstOrDefaultAsync();

                Console.WriteLine(CounterId);
                Console.WriteLine(HTMLCode);
                return Ok(new { HTMLCode });
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
                using (SqlConnection connectionObject = new SqlConnection(OtherVariable.SQLConnectionString))
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
                using (SqlConnection connectionObject = new SqlConnection(OtherVariable.SQLConnectionString))
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