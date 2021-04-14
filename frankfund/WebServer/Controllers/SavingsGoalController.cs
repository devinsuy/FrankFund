using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using DataAccessLayer.Models;
using ServiceLayer;
using System;
using System.Text.Json;
using Newtonsoft.Json;

namespace REST.Controllers
{
    [ApiController]
    public class SavingsGoalController : ControllerBase
    {
        private readonly ILogger<SavingsGoalController> _logger;
        private readonly APIHelper api;
        private readonly SavingsGoalService sgs;
        private readonly HashSet<string> createContrAttrs;
        private readonly HashSet<string> createDateAttrs;
        private readonly HashSet<string> updateGoalAmt;
        private readonly HashSet<string> updateContrAmt;
        private readonly HashSet<string> updateableAttrs;

        public SavingsGoalController(ILogger<SavingsGoalController> logger)
        {
            _logger = logger;
            api = new APIHelper();
            sgs = new SavingsGoalService();

            // Required attributes to update an EXISTING SavingsGoal Goal Amount
            // Additional required attribute "ExtendDate":
            //      For updating GoalAmt, whether or not to increase/decrease the number of periods or
            //      increase/decrease payments to reflect the change. Ignored if GoalAmt isn't changed
            updateGoalAmt = new HashSet<string>()
            {
                "GoalAmt", "ExtendDate"
            };

            // Updating an EXISTING SavingsGoal's contribution amount can be done simulataneously with with its period
            updateContrAmt = new HashSet<string>()
            {
                "ContrAmt", "Period"
            };

            // All updatable attributes of an EXISTING SavingsGoal
            updateableAttrs = new HashSet<string>()
            {
                "Name", "GoalAmt", "ContrAmt", "Period", "EndDate", "ExtendDate"
            };


            // Required attributes to create a NEW SavingsGoal by contribution
            createContrAttrs = new HashSet<string>()
            {
                "Name", "AccountID", "GoalAmt", "Period", "ContrAmt"
            };

            // Required attributes to create a NEW SavingsGoal by end date
            createDateAttrs = new HashSet<string>()
            {
                "Name", "AccountID", "GoalAmt", "Period", "EndDate"
            };
        }

        [Route("api/SavingsGoal/SGID={SGID}&apikey={apiKey}")]
        [HttpGet]
        public IActionResult GetSGByID(long SGID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }   
            if(SGID < 1) {
                return BadRequest();
            }
            return api.serveJson(sgs.getJSON(sgs.getUsingID(SGID)));
        }


        // Delete a SavingsGoal, no effect if a SavingsGoal with the given SGID doesn't exist
        [Route("api/SavingsGoal/SGID={SGID}&apikey={apiKey}")]
        [HttpDelete]
        public IActionResult DeleteByID(long SGID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (SGID < 1)
            {
                return BadRequest();
            }
            sgs.delete(SGID);
            return new OkResult();
        }


        // Create a new SavingsGoal with the given SGID.
        // Returns Http 409 Conflict if already exists
        [Route("api/SavingsGoal/SGID={SGID}&apikey={apiKey}")]
        [HttpPost]
        public IActionResult CreateByID(long SGID, string apiKey, [FromBody] JsonElement reqBody)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (SGID < 1)
            {
                return BadRequest();
            }

            // Validate that the POST request contains all necessary attributes to create a NEW SavingsGoal and nothing more
            Dictionary<string, object> req = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(reqBody));
            HashSet<string> reqAttributes = new HashSet<string>(req.Keys);
            bool createByDate = reqAttributes.SetEquals(createDateAttrs);
            bool createByContr = reqAttributes.SetEquals(createContrAttrs);
            if (!createByDate && !createByContr)
            {
                return BadRequest();
            }

            // POST should be used only to create a new SavingsGoal, not allowed if SavingsGoal with given SGID already exists
            SavingsGoal s = sgs.getUsingID(SGID);
            if (s != null)
            {
                return Conflict();
            }

            // Create the SavingsGoal with the given SGID using the POST payload
            try
            {
                // Create a SavingsGoal by a specified end date
                if (createByDate)
                {
                    s = new SavingsGoal(
                        SGID: SGID,
                        accID: Convert.ToInt64(req["AccountID"]),
                        Convert.ToString(req["Name"]),
                        goalAmt: Convert.ToDecimal(req["GoalAmt"]),
                        period: sgs.castPeriod(Convert.ToString(req["Period"])),
                        endDate: Convert.ToDateTime(req["EndDate"])
                    ); ;
                }
                // Create a SavingsGoal by a specified contribution amount
                else
                {
                    s = new SavingsGoal(
                        SGID: SGID,
                        accID: Convert.ToInt64(req["AccountID"]),
                        Convert.ToString(req["Name"]),
                        goalAmt: Convert.ToDecimal(req["GoalAmt"]),
                        period: sgs.castPeriod(Convert.ToString(req["Period"])),
                        contrAmt: Convert.ToDecimal(req["ContrAmt"])
                    );
                }
            }
            catch
            {
                return BadRequest();
            }


            // Write the new SavingsGoal
            sgs.write(s);
            return new OkResult();
        }


        // Create a new SavingsGoal with the next available SGID
        [Route("api/SavingsGoal&apikey={apiKey}")]
        [HttpPost]
        public IActionResult Create(string apiKey, [FromBody] JsonElement reqBody)
        {
            long SGID = sgs.getNextAvailID();
            IActionResult res = CreateByID(SGID, apiKey, reqBody);
            // Request was invalid, failed to create
            if (!(res is OkResult))
            {
                return res;
            }

            // Otherwise return the TID of the newly created transaction
            return api.serveJson(api.getSingleAttrJSON("SGID", SGID.ToString()));
        }


            // Modify an existing SavingsGoal without specifying all attributes in payload,
            // returns Http 404 Not found if doesn't exist
            // PATCH request body should pass no more than a single attribute unless:
            //      Updating GoalAmt, in which a boolean ExtendDate should be passed
            //      Updating ContrAmt AND Period simultaneously
            [Route("api/SavingsGoal/SGID={SGID}&apikey={apiKey}")]
        [HttpPatch]
        public IActionResult UpdateByID(long SGID, string apiKey, [FromBody] JsonElement reqBody)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (SGID < 1)
            {
                return BadRequest();
            }

            // Validate the attributes of the PATCH request, each attribute specified
            // in the request must be an attribute of a SavingsGoal
            Dictionary<string, object> req = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(reqBody));
            HashSet<string> reqAttributes = new HashSet<string>(req.Keys);
            if (!api.validAttributes(updateableAttrs, reqAttributes))
            {
                return BadRequest();
            }

            SavingsGoal s = sgs.getUsingID(SGID);

            // Http PATCH cannot update a SavingsGoal that does not exist
            if (s == null)
            {
                return NotFound();
            }

            // PATCH request body should pass no more than a single attribute unless:
            //      Updating GoalAmt, in which a boolean ExtendDate should be passed
            //      Updating ContrAmt AND Period simultaneously

            try
            {
                if (reqAttributes.Contains("GoalAmt"))
                {
                    // Updating Goal Amount must also specify ExtendDate
                    if (!reqAttributes.SetEquals(updateGoalAmt))
                    {
                        return BadRequest();
                    }
                    s.updateGoalAmt(Convert.ToDecimal(req["GoalAmt"]), Convert.ToBoolean(req["ExtendDate"]));
                }
                else if (reqAttributes.Contains("ContrAmt"))
                {
                    // Update Contribution Amount and Period simultaneously
                    if (reqAttributes.Count > 1)
                    {
                        if (!reqAttributes.SetEquals(updateContrAmt))
                        {
                            return BadRequest();
                        }
                        s.updateContrAmtAndPeriod(Convert.ToDecimal(req["ContrAmt"]), sgs.castPeriod(Convert.ToString(req["Period"])));
                    }
                    // Update just Contribution Amount
                    else
                    {
                        s.updateContrAmt(Convert.ToDecimal(req["ContrAmt"]));
                    }
                }
                // All other attributes should specify only a single value
                else
                {
                    if (reqAttributes.Count != 1)
                    {
                        return BadRequest();
                    }
                    if (reqAttributes.Contains("Name"))
                    {
                        s.updateName(Convert.ToString(req["Name"]));
                    }
                    else if (reqAttributes.Contains("Period"))
                    {
                        s.updatePeriod(sgs.castPeriod(Convert.ToString(req["Period"])));
                    }
                    else if (reqAttributes.Contains("EndDate"))
                    {
                        s.updateEndDate(Convert.ToDateTime(req["EndDate"]));
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return BadRequest();
            }

            // Write changes, if any
            sgs.update(s);
            return api.serveJson(sgs.getJSON(s));
        }
    }
}
