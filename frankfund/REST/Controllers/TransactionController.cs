using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using DataAccessLayer.Models;
using ServiceLayer;
using System;


namespace REST.Controllers
{
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly APIHelper api;
        private readonly TransactionService ts;
        private readonly HashSet<string> attributes;

        public TransactionController(ILogger<TransactionController> logger)
        {
            _logger = logger;
            api = new APIHelper();
            ts = new TransactionService();
            attributes = new HashSet<string>
            {
                "SGID", "AccountID", "TransactionName", "Amount", "DateTransactionMade",
                "DateTransactionEntered", "IsExpense", "TransactionCategory"
            };
        }

        [Route("api/TID={TID}&apikey={apiKey}")]
        [HttpGet]
        public IActionResult GetByID(int TID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            if (TID < 1)
            {
                return BadRequest();
            }
            return api.serveJson(ts.getJSON(ts.getUsingID(TID)));
        }

        [Route("api/TID={TID}&apikey={apiKey}")]
        [HttpDelete]
        public IActionResult DeleteByID(int TID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            if (TID < 1)
            {
                return BadRequest();
            }
            ts.delete(TID);
            return new OkResult();
        }


        [Route("api/TID={TID}&apikey={apiKey}")]
        [HttpPut]
        public IActionResult UpdateByID(int TID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            if (TID < 1)
            {
                return BadRequest();
            }

            // Validate if the attributes of the PUT request are attributes of a Transaction
            // PUT requires request to provide key,value pairs for EVERY Transaction attribute
            HashSet<string> reqAttributes = new HashSet<string>(Request.Form.Keys);
            if (!api.validAttributes(attributes, reqAttributes)){
                return BadRequest();
            }

            var req = Request.Form;
            Transaction t = ts.getUsingID(TID);

            // Create the transaction with the given TID if it doesn't exist
            if(t == null)
            {
                // Newly created transaction with assign dateTransactionEntered to DateTime.now(), it should not be passed
                if (reqAttributes.Count != attributes.Count - 1)
                {
                    return BadRequest();
                }

                t = new Transaction(                                        
                        TID:                    TID,
                        accountID:              long.Parse(req["AccountID"]),
                        SGID:                   long.Parse(req["SGID"]),
                        transactionName:        req["TransactionName"],
                        amount:                 decimal.Parse(req["Amount"]),
                        dateTransactionMade:    DateTime.Parse(req["DateTransactionMade"]),
                        isExpense:              System.Convert.ToBoolean(req["IsExpense"]),
                        transactionCategory:    req["TransactionCategory"]
                    );

                // Write the new transaction
                ts.write(t);
            }

            // Otheriwse fufill the PUT request and update the corresponding transaction 
            else
            {
                // HTTP PUT request to update an EXISTING transaction requires all fields of the transaction to be specified
                if(reqAttributes.Count != attributes.Count)
                {
                    return BadRequest();
                }

                // TID and AccountID are never modifiable
                t.setSGID(long.Parse(req["SGID"]));
                t.setTransactionName(req["TransactionName"]);
                t.setAmount(decimal.Parse(req["Amount"]));
                t.setDateTransactionMade(DateTime.Parse(req["DateTransactionMade"]));
                t.setDateTransactionEntered(DateTime.Parse(req["DateTransactionMade"]));
                t.setIsExpense(bool.Parse(req["IsExpense"]));
                t.setTransactionCategory(req["TransactionCategory"]);

                // Write changes, if any
                ts.update(t);
            }

            return new OkResult();
        }

    }
}
