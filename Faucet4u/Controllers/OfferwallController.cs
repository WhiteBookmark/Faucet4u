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
    public class OfferwallController : ControllerBase
    {
        [HttpPost]
        [Route("Hitswall")]
        public ActionResult GetHitswall([FromForm] OfferwallModel bodyValue)
        {
            try
            {
                string password = "6u4nnP4kWsoC";
                Console.WriteLine(JsonConvert.SerializeObject(bodyValue));
                if (bodyValue.pwd.Equals(password))
                {
                    using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                    {
                        string queryToExecute = @"BEGIN
                                                    INSERT INTO OfferwallHistory(Username, Offerwall, Amount, Status, CampaignId, CampaignName) VALUES(@Username, @Offerwall, @Amount, @Status, @CampaignId, @CampaignName)
                                                    UPDATE Users SET OfferwallEarning = OfferwallEarning + @Amount WHERE Username = @Username
                                                    EXEC UpdateOfferwallBalance @UsernameInput = @Username, @AmountInput = @Amount
                                                    END
                                                    ";

                        DynamicParameters paramtersToPass = new DynamicParameters();
                        paramtersToPass.Add("Username", bodyValue.u, DbType.String, ParameterDirection.Input);


                        double convertedAmount = ConvertTo.Bitcoin(bodyValue.c);

                        if (bodyValue.t.Equals("1"))
                        {
                            if (bodyValue.s.Equals("1"))
                            {
                                paramtersToPass.Add("Amount", Convert.ToDouble(convertedAmount), DbType.Double, ParameterDirection.Input);
                            }
                            else if (bodyValue.s.Equals("2"))
                            {
                                paramtersToPass.Add("Amount", -Convert.ToDouble(convertedAmount), DbType.Double, ParameterDirection.Input);
                            }
                        }
                        else
                        {
                            convertedAmount = 0.00000000;
                        }

                        paramtersToPass.Add("Offerwall", "Hitswall", DbType.String, ParameterDirection.Input);
                        string status = "Credit";
                        if (bodyValue.t.Equals("1")) status = "Credit";
                        if (bodyValue.t.Equals("2")) status = "Debit";
                        paramtersToPass.Add("Status", status, DbType.String, ParameterDirection.Input);
                        paramtersToPass.Add("CampaignId", bodyValue.cid, DbType.Int32, ParameterDirection.Input);
                        paramtersToPass.Add("CampaignName", bodyValue.cname, DbType.String, ParameterDirection.Input);

                        connectionObject.Execute(queryToExecute, paramtersToPass);
                        return Ok();
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest();

            }
        }

        [HttpGet]
        [Route("PTCWall")]
        public ActionResult GetPTCWall([FromQuery] OfferwallModel bodyValue)
        {
            try
            {
                string password = "yR576UZ92cCiKY121FvNDwZKrY1xV0a";
                Console.WriteLine(JsonConvert.SerializeObject(bodyValue));
                if (bodyValue.pwd.Equals(password))
                {
                    using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                    {
                        string queryToExecute = @"BEGIN
                        INSERT INTO OfferwallHistory(Username, Offerwall, Amount, Status) VALUES(@Username, @Offerwall, @Amount, @Status)
                        UPDATE Users SET OfferwallEarning = OfferwallEarning + @Amount WHERE Username = @Username
                        EXEC UpdateOfferwallBalance @UsernameInput = @Username, @AmountInput = @Amount
                        END
                        ";

                        DynamicParameters paramtersToPass = new DynamicParameters();
                        paramtersToPass.Add("Username", bodyValue.usr, DbType.String, ParameterDirection.Input);

                        double convertedAmount = ConvertTo.Bitcoin(bodyValue.r);

                        if (bodyValue.t.Equals("1"))
                        {
                            if (bodyValue.c.Equals("1"))
                            {
                                paramtersToPass.Add("Amount", Convert.ToDouble(convertedAmount), DbType.Double, ParameterDirection.Input);
                            }
                            else if (bodyValue.c.Equals("2"))
                            {
                                paramtersToPass.Add("Amount", -Convert.ToDouble(convertedAmount), DbType.Double, ParameterDirection.Input);
                            }
                        }
                        else
                        {
                            convertedAmount = 0.00000000;
                        }

                        paramtersToPass.Add("Offerwall", "PTCWall", DbType.String, ParameterDirection.Input);
                        string status = "Credit";
                        if (bodyValue.t.Equals("1")) status = "Credit";
                        if (bodyValue.t.Equals("2")) status = "Debit";

                        paramtersToPass.Add("Status", status, DbType.String, ParameterDirection.Input);

                        connectionObject.Execute(queryToExecute, paramtersToPass);
                        return Ok();
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest();

            }
        }

        [HttpPost]
        [Route("SkippyAds")]
        public ActionResult GetSkippyAds([FromQuery] OfferwallModel bodyValue)
        {
            try
            {
                string password = "wF2PO56SpN1n";
                Console.WriteLine(JsonConvert.SerializeObject(bodyValue));
                if (bodyValue.pwd.Equals(password))
                {
                    using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                    {
                        string queryToExecute = @"BEGIN
                                                    INSERT INTO OfferwallHistory(Username, Offerwall, Amount, Status, CampaignId, CampaignName) VALUES(@Username, @Offerwall, @Amount, @Status, @CampaignId, @CampaignName)
                                                    UPDATE Users SET OfferwallEarning = OfferwallEarning + @Amount WHERE Username = @Username
                                                    EXEC UpdateOfferwallBalance @UsernameInput = @Username, @AmountInput = @Amount
                                                    END
                                                    ";

                        DynamicParameters paramtersToPass = new DynamicParameters();
                        paramtersToPass.Add("Username", bodyValue.u, DbType.String, ParameterDirection.Input);


                        double convertedAmount = ConvertTo.Bitcoin(bodyValue.c);

                        if (bodyValue.t.Equals("1"))
                        {
                            if (bodyValue.s.Equals("1"))
                            {
                                paramtersToPass.Add("Amount", Convert.ToDouble(convertedAmount), DbType.Double, ParameterDirection.Input);
                            }
                            else if (bodyValue.s.Equals("2"))
                            {
                                paramtersToPass.Add("Amount", -Convert.ToDouble(convertedAmount), DbType.Double, ParameterDirection.Input);
                            }
                        }
                        else
                        {
                            convertedAmount = 0.00000000;
                        }

                        paramtersToPass.Add("Offerwall", "SkippyAds", DbType.String, ParameterDirection.Input);
                        string status = "Credit";
                        if (bodyValue.s.Equals("1")) status = "Credit";
                        if (bodyValue.s.Equals("2")) status = "Debit";

                        paramtersToPass.Add("Status", status, DbType.String, ParameterDirection.Input);
                        paramtersToPass.Add("CampaignId", bodyValue.cid, DbType.Int32, ParameterDirection.Input);
                        paramtersToPass.Add("CampaignName", bodyValue.cname, DbType.String, ParameterDirection.Input);

                        connectionObject.Execute(queryToExecute, paramtersToPass);
                        return Ok();
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest();

            }
        }

        [HttpGet]
        [Route("KiwiWall")]
        public ActionResult GetKiwiWall([FromQuery] OfferwallModel bodyValue)
        {
            try
            {
                string password = "DEZELJfOVCMDZa5NbbIYNnOMIjOUpp0z";
                string md5Signature = ConvertTo.MD5($"{bodyValue.sub_id}:{bodyValue.amount}:{password}");
                Console.WriteLine(JsonConvert.SerializeObject(bodyValue));
                Console.WriteLine(md5Signature);
                if (bodyValue.signature.Equals(md5Signature, StringComparison.OrdinalIgnoreCase))
                {
                    using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                    {
                        string queryToExecute = @"BEGIN
                                                    DECLARE @UserId uniqueidentifier = TRY_CONVERT(UNIQUEIDENTIFIER, @Id)
                                                    DECLARE @Username varchar(12)
                                                    SELECT @Username = Username FROM Users WHERE Id = @UserId

                                                    IF(@Username IS NOT NULL AND @Username != '')
                                                    BEGIN
                                                    INSERT INTO OfferwallHistory(Username, Offerwall, Amount, Status, CampaignId, CampaignName) VALUES(@Username, @Offerwall, @Amount, @Status, @CampaignId, @CampaignName)
                                                    UPDATE Users SET OfferwallEarning += @Amount WHERE Username = @Username
                                                    EXEC UpdateOfferwallBalance @UsernameInput = @Username, @AmountInput = @Amount
                                                    END
                                                    END
                                                    ";

                        DynamicParameters paramtersToPass = new DynamicParameters();
                        paramtersToPass.Add("Id", bodyValue.sub_id, DbType.String, ParameterDirection.Input);


                        double convertedAmount = ConvertTo.Bitcoin(bodyValue.amount);


                        if (bodyValue.status.Equals("1"))
                        {
                            paramtersToPass.Add("Amount", Convert.ToDouble(convertedAmount), DbType.Double, ParameterDirection.Input);
                        }
                        else if (bodyValue.status.Equals("2"))
                        {
                            paramtersToPass.Add("Amount", -Convert.ToDouble(convertedAmount), DbType.Double, ParameterDirection.Input);
                        }


                        paramtersToPass.Add("Offerwall", "KiwiWall", DbType.String, ParameterDirection.Input);
                        string status = "Credit";
                        if (bodyValue.status.Equals("1")) status = "Credit";
                        if (bodyValue.status.Equals("2")) status = "Debit";

                        paramtersToPass.Add("Status", status, DbType.String, ParameterDirection.Input);
                        paramtersToPass.Add("CampaignId", bodyValue.offer_id, DbType.Int32, ParameterDirection.Input);
                        paramtersToPass.Add("CampaignName", bodyValue.offer_name, DbType.String, ParameterDirection.Input);

                        connectionObject.Execute(queryToExecute, paramtersToPass);
                        //Kiwiwall requires returning of "1" in order to be considered as successful
                        return Ok("1");
                    }
                }
                Console.WriteLine("Kiwiwall signature didn't match");
                //Kiwiwall requires returning of "0" in order to be considered as un-successful
                return BadRequest("0");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest("0");

            }
        }


        [HttpGet]
        [Route("OfferToro")]
        public ActionResult GetOfferToro([FromQuery] OfferwallModel bodyValue)
        {
            try
            {
                string password = "1700b12a4ade24d275185f1bcff74228";
                string md5Signature = ConvertTo.MD5($"{bodyValue.oid}-{bodyValue.user_id}-{password}");
                Console.WriteLine(JsonConvert.SerializeObject(bodyValue));
                Console.WriteLine(md5Signature);
                if (bodyValue.sig.Equals(md5Signature, StringComparison.OrdinalIgnoreCase))
                {
                    using (SqlConnection connectionObject = new SqlConnection(Other.SQLConnectionString))
                    {
                        string queryToExecute = @"BEGIN
                                                    DECLARE @UserId uniqueidentifier = TRY_CONVERT(UNIQUEIDENTIFIER, @Id)
                                                    DECLARE @Username varchar(12)
                                                    SELECT @Username = Username FROM Users WHERE Id = @UserId

                                                    IF(@Username IS NOT NULL AND @Username != '')
                                                    BEGIN
                                                    INSERT INTO OfferwallHistory(Username, Offerwall, Amount, Status, CampaignId, CampaignName) VALUES(@Username, @Offerwall, @Amount, @Status, @CampaignId, @CampaignName)
                                                    UPDATE Users SET OfferwallEarning += @Amount WHERE Username = @Username
                                                    EXEC UpdateOfferwallBalance @UsernameInput = @Username, @AmountInput = @Amount
                                                    END
                                                    END
                                                    ";

                        DynamicParameters paramtersToPass = new DynamicParameters();
                        paramtersToPass.Add("Id", bodyValue.user_id, DbType.String, ParameterDirection.Input);

                        double convertedAmount = ConvertTo.Bitcoin(bodyValue.amount);

                        if (bodyValue.status.Equals("1"))
                        {
                            paramtersToPass.Add("Amount", Convert.ToDouble(convertedAmount), DbType.Double, ParameterDirection.Input);
                        }
                        else if (bodyValue.status.Equals("2"))
                        {
                            paramtersToPass.Add("Amount", -Convert.ToDouble(convertedAmount), DbType.Double, ParameterDirection.Input);
                        }


                        paramtersToPass.Add("Offerwall", "OfferToro", DbType.String, ParameterDirection.Input);
                        string status = "Credit";
                        if (bodyValue.status.Equals("1")) status = "Credit";
                        if (bodyValue.status.Equals("2")) status = "Debit";

                        paramtersToPass.Add("Status", status, DbType.String, ParameterDirection.Input);
                        paramtersToPass.Add("CampaignId", bodyValue.oid, DbType.Int32, ParameterDirection.Input);
                        paramtersToPass.Add("CampaignName", bodyValue.o_name, DbType.String, ParameterDirection.Input);

                        connectionObject.Execute(queryToExecute, paramtersToPass);
                        //OfferToro requires returning of "1" in order to be considered as successful
                        return Ok("1");
                    }
                }
                Console.WriteLine("OfferToro signature didn't match");
                //OfferToro requires returning of "0" in order to be considered as un-successful
                return BadRequest("0");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest("0");

            }
        }
    }
}