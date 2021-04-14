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
            "SID", "AccountID", "PurchaseDate", "Amount", "RenewFrequency"
        };

            // Attributes that should be specified in request payload if updating an EXISTING transaction
            updateAttr = new HashSet<string>
        {
            "SID", "AccountID", "PurchaseDate", "Amount", "RenewFrequency"
        };
        }

        // Retrieve a Subscription with the given SID,
        // returns Http 204 NoContent if doesn't exist
        [Route("api/SID={SID}&apikey={apiKey}")]
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
        [Route("api/SID={SID}&apikey={apiKey}")]
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
        [Route("api/SID={SID}&apikey={apiKey}")]
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

            // Validate that the POST request contains all necessary attributes to create a NEW transaction and nothing more
            Dictionary<string, object> req = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(reqBody));
            HashSet<string> reqAttributes = new HashSet<string>(req.Keys);

            if (!reqAttributes.SetEquals(createAttr))
            {
                return BadRequest();
            }

            // POST should be used only to create a new Transaction, not allowed if Transaction with given TID already exists
            Subscription s = subservice.getUsingID(SID);
            if (subservice != null)
            {
                return Conflict();
            }

            // Create the transaction with the given TID using the POST payload
            try
            {
                s = new Subscription(
                        accID: Convert.ToInt64(req["AccountID"]),
                        SID: SID,
                        RID: Convert.ToInt64(req["RID"]),
                        purchaseDate: Convert.ToDateTime(req["PurchaseDate"]),
                        purchaseAmount: Convert.ToDecimal(req["Amount"]),
                        frequency: subservice.castFrequency(Convert.ToString(req["RenewFrequency"]))
                    );

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return BadRequest();
            }

            // Write the new transaction
            subservice.write(s);
            return new OkResult();
        }

        // Create a new subscription with the next available SID
        [Route("api/SID&apikey={apiKey}")]
        [HttpPost]
        public IActionResult Create(string apiKey, [FromBody] JsonElement reqBody)
        {
            long SID = subservice.getNextAvailID();
            IActionResult res = CreateByID(SID, apiKey, reqBody);

            // Request was invalid, failed to create
            if (!(res is OkResult))
            {
                return res;
            }

            // Otherwise return the TID of the newly created transaction
            return api.serveJson(api.getSingleAttrJSON("SID", SID.ToString()));
        }

    }
}