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
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly APIHelper api;
        private readonly TransactionService ts;
        private readonly HashSet<string> createAttr;
        private readonly HashSet<string> updateAttr;

        public TransactionController(ILogger<TransactionController> logger)
        {
            _logger = logger;
            api = new APIHelper();
            ts = new TransactionService();

            // Attributes that should be specified in request payload if creating a NEW transaction
            // Newly created transaction with assign dateTransactionEntered to DateTime.now(), it should not be passed
            createAttr = new HashSet<string>
            {
                "SGID", "AccountID", "TransactionName", "Amount", "DateTransactionMade",
                "IsExpense", "TransactionCategory"
            };

            // Attributes that should be specified in request payload if updating an EXISTING transaction
            updateAttr = new HashSet<string>
            {
                "SGID", "AccountID", "TransactionName", "Amount", "DateTransactionMade",
                "DateTransactionEntered", "IsExpense", "TransactionCategory"
            };
        }

        // Retrieve a transaction with the given TID,
        // returns Http 204 NoContent if doesn't exist
        [Route("api/transaction/TID={TID}&apikey={apiKey}")]
        [HttpGet]
        public IActionResult GetByID(long TID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (TID < 1)
            {
                return BadRequest("Invalid TID");
            }
            return api.serveJson(ts.getJSON(ts.getUsingID(TID)));
        }

        // Delete a transaction, no effect if a transaction with the given TID doesn't exist
        [Route("api/transaction/TID={TID}&apikey={apiKey}")]
        [HttpDelete]
        public IActionResult DeleteByID(long TID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (TID < 1)
            {
                return BadRequest();
            }
            ts.delete(TID);
            return new OkResult();
        }


        // Create a new transaction with the given TID.
        // Returns Http 409 Conflict if already exists
        [Route("api/transaction/TID={TID}&apikey={apiKey}")]
        [HttpPost]
        public IActionResult CreateByID(long TID, string apiKey, [FromBody] JsonElement reqBody)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (TID < 1)
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
            Transaction t = ts.getUsingID(TID);
            if(t != null)
            {
                return Conflict();
            }

            // Create the transaction with the given TID using the POST payload
            try
            {
                t = new Transaction(
                        TID: TID,
                        accountID: Convert.ToInt64(req["AccountID"]),
                        SGID: Convert.ToInt64(req["SGID"]),
                        transactionName: Convert.ToString(req["TransactionName"]),
                        amount: Convert.ToDecimal(req["Amount"]),
                        dateTransactionMade: Convert.ToDateTime(req["DateTransactionMade"]),
                        isExpense: System.Convert.ToBoolean(req["IsExpense"]),
                        category: ts.castCategory(Convert.ToString(req["TransactionCategory"]))
                    );

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return BadRequest();
            }

            // Write the new transaction
            ts.write(t);
            return new OkResult();
        }

        // Create a new transaction with the next available TID
        [Route("api/transaction/TID&apikey={apiKey}")]
        [HttpPost]
        public IActionResult Create(string apiKey, [FromBody] JsonElement reqBody)
        {
            long TID = ts.getNextAvailID();
            IActionResult res = CreateByID(TID, apiKey, reqBody);

            // Request was invalid, failed to create
            if(!(res is OkResult))
            {
                return res;
            }

            // Otherwise return the TID of the newly created transaction
            return api.serveJson(api.getSingleAttrJSON("TID", TID.ToString()));
        }


        // Update an existing transaction or create if not exists
        [Route("api/transaction/TID={TID}&apikey={apiKey}")]
        [HttpPut]
        public IActionResult UpdateAllByID(long TID, string apiKey, [FromBody] JsonElement reqBody)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (TID < 1)
            {
                return BadRequest();
            }

            Dictionary<string, object> req = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(reqBody));
            HashSet<string> reqAttributes = new HashSet<string>(req.Keys);
            Transaction t = ts.getUsingID(TID);

            // Create the transaction with the given TID if it doesn't exist
            if(t == null)
            {
                // PUT requires request to provide key,value pairs for EVERY Transaction attribute except dateTransactionEntered
                if (!reqAttributes.SetEquals(createAttr))
                {
                    return BadRequest();
                }
                try
                {
                    t = new Transaction(
                            TID: TID,
                            accountID: Convert.ToInt64(req["AccountID"]),
                            SGID: Convert.ToInt64(req["SGID"]),
                            transactionName: Convert.ToString(req["TransactionName"]),
                            amount: Convert.ToDecimal(req["Amount"]),
                            dateTransactionMade: Convert.ToDateTime(req["DateTransactionMade"]),
                            isExpense: Convert.ToBoolean(req["IsExpense"]),
                            category: ts.castCategory(Convert.ToString(req["TransactionCategory"]))
                        );
                }
                // Formatting or improper data typing raised exception, bad request
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return BadRequest();
                }

                // Write the new transaction
                ts.write(t);
            }

            // Otheriwse fufill the PUT request and update the corresponding transaction 
            else
            {
                // HTTP PUT request to update an EXISTING transaction requires ALL fields of the transaction to be specified
                if(!reqAttributes.SetEquals(updateAttr))
                {
                    return BadRequest();
                }

                // TID and AccountID are never modifiable
                try
                {
                    t.setSGID(Convert.ToInt64(req["SGID"]));
                    t.setTransactionName(Convert.ToString(req["TransactionName"]));
                    t.setAmount(Convert.ToDecimal(req["Amount"]));
                    t.setDateTransactionMade(Convert.ToDateTime(req["DateTransactionMade"]));
                    t.setDateTransactionEntered(Convert.ToDateTime(req["DateTransactionEntered"]));
                    t.setIsExpense(Convert.ToBoolean(req["IsExpense"]));
                    t.setTransactionCategory(ts.castCategory(Convert.ToString(req["TransactionCategory"])));
                }
                // Formatting or improper data typing raised exception, bad request
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return BadRequest();
                }

                // Write changes, if any
                ts.update(t);
            }

            return new OkResult();
        }


        // Modify an existing transaction without specifying all attributes in payload,
        // returns Http 404 Not found if doesn't exist
        [Route("api/transaction/TID={TID}&apikey={apiKey}")]
        [HttpPatch]
        public IActionResult UpdateByID(long TID, string apiKey,[FromBody] JsonElement reqBody)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (TID < 1)
            {
                return BadRequest();
            }

            // Validate the attributes of the PATCH request, each attribute specified
            // in the request must be an attribute of a Transaction
            Dictionary<string, object> req = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(reqBody));
            HashSet<string> reqAttributes = new HashSet<string>(req.Keys);
            if (!api.validAttributes(updateAttr, reqAttributes))
            {
                return BadRequest();
            }
            Transaction t = ts.getUsingID(TID);

            // Http POST cannot update a transaction that does not exist
            if (t == null)
            {
                return NotFound();
            }

            // Otherwise fufill the POST request and update the corresponding transaction
            // Http POST may only specify a few attributes to update or provide all of them
            // AccountID is not an updatable attribute

            // Update the transaction with the specified POST attributes
            try
            {
                if (reqAttributes.Contains("SGID"))
                {
                    t.setSGID(Convert.ToInt64(req["SGID"]));
                }
                if (reqAttributes.Contains("TransactionName"))
                {
                    t.setTransactionName(Convert.ToString(req["TransactionName"]));
                }
                if (reqAttributes.Contains("Amount"))
                {
                    t.setAmount(Convert.ToDecimal(req["Amount"]));
                }
                if (reqAttributes.Contains("DateTransactionMade")) {
                    t.setDateTransactionMade(Convert.ToDateTime(req["DateTransactionMade"]));
                }
                if (reqAttributes.Contains("DateTransactionEntered")){
                    t.setDateTransactionEntered(Convert.ToDateTime(req["DateTransactionEntered"]));
                }
                if (reqAttributes.Contains("IsExpense"))
                {
                    t.setIsExpense(Convert.ToBoolean(req["IsExpense"]));
                }
                if (reqAttributes.Contains("TransactionCategory"))
                {
                    t.setTransactionCategory(ts.castCategory(Convert.ToString(req["TransactionCategory"])));
                }
            }
            // Formatting or improper data typing raised exception, bad request
            catch(Exception e){
                Console.WriteLine(e.ToString());
                return BadRequest();
            }

            // Write changes, if any
            ts.update(t);
            return new OkResult();
        }

    }
}
