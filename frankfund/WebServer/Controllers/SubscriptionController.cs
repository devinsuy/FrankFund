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
    public class SubscriptionController : ControllerBase
    {
        private readonly ILogger<SubscriptionController> _logger;
        private readonly APIHelper api; 
        private readonly SubscriptionService subservice;
        private readonly HashSet<string> createAttr;
        private readonly HashSet<string> updateAttr;


        public SubscriptionController(ILogger<SubscriptionController> logger)
        {
            _logger = logger;
            api = new APIHelper();
            subservice = new SubscriptionService();

            // Attributes that should be specified in request payload if creating a NEW Subscription
            createAttr = new HashSet<string>
            {
            "RID", "Notes", "AccountID", "PurchaseDate", "Amount", "Notes", "RenewFrequency"
            };

            // Attributes that should be specified in request payload if updating an EXISTING subscription
            updateAttr = new HashSet<string>
            {
            "AccountID", "PurchaseDate", "Amount", "Notes", "RenewFrequency"
            };
        }

        // Retrieve a Subscription with the given SID,
        // returns Http 204 NoContent if doesn't exist
        [Route("api/Subscription/SID={SID}&apikey={apiKey}")]
        [HttpGet]
        public IActionResult GetByID(long SID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            if (SID < 1)
            {
                return BadRequest();
            }
            return api.serveJson(subservice.getJSON(subservice.getUsingID(SID)));
        }

        // Delete a Subscription, no effect if a Subscription with the given SID doesn't exist
        [Route("api/Subscription/SID={SID}&apikey={apiKey}")]
        [HttpDelete]
        public IActionResult DeleteByID(long SID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            if (SID < 1)
            {
                return BadRequest();
            }
            subservice.delete(SID);
            return new OkResult();
        }

        // Create a new Subscription with the given SID.
        // Returns Http 409 Conflict if already exists
        [Route("api/Subscription/SID={SID}&apikey={apiKey}")]
        [HttpPost]
        public IActionResult CreateByID(long SID, string apiKey, [FromBody] JsonElement reqBody)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            if (SID < 1)
            {
                return BadRequest();
            }

            // Validate that the POST request contains all necessary attributes to create a NEW Subscription and nothing more
            Dictionary<string, object> req = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(reqBody));
            HashSet<string> reqAttributes = new HashSet<string>(req.Keys);

            if (!reqAttributes.SetEquals(createAttr))
            {
                return BadRequest("Invalid attribute(s) in request body, expected exactly { SID, AccountID, RID, PurchaseDate, Notes, Amount, RenewFrequency }");
            }

            // POST should be used only to create a new Subscription, not allowed if Subscription with given SID already exists
            Subscription s = subservice.getUsingID(SID);
            if (s != null)
            {
                return Conflict($"A subscription already exists with SID {SID}");
            }

            // Create the Subscription with the given SID using the POST payload
            try
            {
                s = new Subscription(
                        SID: SID,
                        accID: Convert.ToInt64(req["AccountID"]),
                        RID: Convert.ToInt64(req["RID"]),
                        purchaseDate: Convert.ToDateTime(req["PurchaseDate"]),
                        notes: Convert.ToString(req["Notes"]),
                        purchaseAmount: Convert.ToDecimal(req["Amount"]),
                        frequency: subservice.castSubscriptionFrequency(Convert.ToString(req["RenewFrequency"]))
                    );

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return BadRequest();
            }

            // Write the new transaction
            subservice.write(s);
            return api.serveJson(subservice.getJSON(s));
        }

        // Create a new subscription with the next available SID
        [Route("api/Subscription&apikey={apiKey}")]
        [HttpPost]
        public IActionResult Create(string apiKey, [FromBody] JsonElement reqBody)
        {
            long SID = subservice.getNextAvailID();
            return CreateByID(SID, apiKey, reqBody);
        }


        // Update an existing subscription or create if not exists
        [Route("api/Subscription/SID={SID}&apikey={apiKey}")]
        [HttpPut]
        public IActionResult UpdateAllByID(long SID, string apiKey, [FromBody] JsonElement reqBody)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            if (SID < 1)
            {
                return BadRequest();
            }

            Dictionary<string, object> req = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(reqBody));
            HashSet<string> reqAttributes = new HashSet<string>(req.Keys);
            Subscription s = subservice.getUsingID(SID);

            // Create the Subscription with the given SID if it doesn't exist
            if (s == null)
            {
                // PUT requires request to provide key,value pairs for EVERY Subscription attribute 
                if (!reqAttributes.SetEquals(updateAttr))
                {
                    return BadRequest();
                }
                try
                {
                    s = new Subscription(
                        SID: SID,
                        accID: Convert.ToInt64(req["AccountID"]),
                        RID: Convert.ToInt64(req["RID"]),
                        purchaseAmount: Convert.ToDecimal(req["Amount"]),
                        purchaseDate: Convert.ToDateTime(req["PurchaseDate"]),
                        notes: Convert.ToString(req["Notes"]),
                        frequency: subservice.castSubscriptionFrequency(Convert.ToString(req["RenewFrequency"]))
                    );
                }
                // Formatting or improper data typing raised exception, bad request
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return BadRequest();
                }

                // Write the new Subscription
                subservice.write(s);
            }

            // Otherwise fufill the PUT request and update the corresponding subscription 
            else
            {
                // HTTP PUT request to update an EXISTING subscription requires ALL fields of the subscription to be specified
                if (!reqAttributes.SetEquals(updateAttr))
                {
                    return BadRequest();
                }

                // SID and AccountID are never modifiable
                try
                {
                    //s.setRID(Convert.ToInt64(req["RID"]));
                    s.setAmount(Convert.ToDecimal(req["Amount"]));
                    s.setPurchaseDate(Convert.ToDateTime(req["PurchaseDate"]));
                    s.setNotes(Convert.ToString(req["Notes"]));
                    s.setSubscriptionFrequency(subservice.castSubscriptionFrequency(Convert.ToString(req["RenewFrequency"])));
                }
                // Formatting or improper data typing raised exception, bad request
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return BadRequest();
                }

                // Write changes, if any
                subservice.update(s);
            }

            return api.serveJson(subservice.getJSON(s));
        }



        // Modify an existing Subscription without specifying all attributes in payload,
        // returns Http 404 Not found if doesn't exist
        [Route("api/Subscription/SID={SID}&apikey={apiKey}")]
        [HttpPatch]
        public IActionResult UpdateByID(long SID, string apiKey, [FromBody] JsonElement reqBody)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (SID < 1)
            {
                return BadRequest();
            }

            // Validate the attributes of the PATCH request, each attribute specified
            // in the request must be an attribute of a SavingsGoal
            Dictionary<string, object> req = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(reqBody));
            HashSet<string> reqAttributes = new HashSet<string>(req.Keys);
            if (!api.validAttributes(updateAttr, reqAttributes))
            {
                return BadRequest();
            }

            Subscription s = subservice.getUsingID(SID);

            // Http PATCH cannot update a Subscription that does not exist
            if (s == null)
            {
                return NotFound();
            }

            bool attrUpdated = false;
            try {
                if (reqAttributes.Contains("PurchaseDate"))
                {
                    s.setPurchaseDate(Convert.ToDateTime(req["PurchaseDate"]));
                    attrUpdated = true;
                }
                else if (reqAttributes.Contains("Amount"))
                {
                    s.setAmount(Convert.ToDecimal(req["Amount"]));
                    attrUpdated = true;
                }
                else if (reqAttributes.Contains("RenewFrequency"))
                {
                    s.setRenewFrequency(subservice.castSubscriptionFrequency(Convert.ToString(req["RenewFrequency"])));
                    attrUpdated = true;
                }

                else if (reqAttributes.Contains("Notes"))
                {
                    s.setNotes((Convert.ToString(req["Notes"])));
                    attrUpdated = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return BadRequest();
            }
            if (!attrUpdated)
            {
                return BadRequest();
            }

            // Write changes, if any
            subservice.update(s);
            return api.serveJson(subservice.getJSON(s));
        }
    }
}
