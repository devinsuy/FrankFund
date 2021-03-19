using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using DataAccessLayer.Models;
using ServiceLayer;
using System;


namespace REST.Controllers
{
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly ILogger<UserAccountController> _logger;
        private readonly APIHelper api;
        private readonly UserAccountService uas;
        private readonly HashSet<string> attributes;
        private readonly HashSet<string> nullableAttrs;

        public UserAccountController(ILogger<UserAccountController> logger)
        {
            _logger = logger;
            api = new APIHelper();
            uas = new UserAccountService();
            attributes = new HashSet<string>()
            {
                "AccountID", "AccountUsername", "EmailAddress", "Password"
            };

            // TODO: Pending design for registration using social services
            //nullableAttrs = new HashSet<string>()
            //{
            //    "FacebookID", "GoogleID"
            //};
        }

        [Route("api/accID={accID}&apikey={apiKey}")]
        [HttpGet]
        public IActionResult GetByID(int accID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            if (accID < 1)
            {
                return BadRequest();
            }
            return api.serveJson(uas.getJSON(uas.getUsingID(accID)));
        }

        // Delete an account, no effect if an account with the given accID doesn't exist
        [Route("api/accID={accID}&apikey={apiKey}")]
        [HttpDelete]
        public IActionResult DeleteByID(int accID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            if (accID < 1)
            {
                return BadRequest();
            }
            uas.delete(accID);
            return new OkResult();
        }


        // Create a new account with the given accID.
        // Returns Http 409 Conflict if already exists
        // TODO: Account registration logic may be more complex, byte salt to be updated
        [Route("api/accID={accID}&apikey={apiKey}")]
        [HttpPost]
        public IActionResult CreateByID(int accID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            if (accID < 1)
            {
                return BadRequest();
            }

            // Validate that the POST request contains all necessary attributes to create a NEW Account and nothing more
            HashSet<string> reqAttributes = new HashSet<string>(Request.Form.Keys);
            if (!reqAttributes.SetEquals(attributes))
            {
                return BadRequest();
            }

            // POST should be used only to create a new Account, not allowed if Account with given accID already exists
            UserAccount acc = uas.getUsingID(accID);
            if (acc != null)
            {
                return Conflict();
            }

            // Create the Account with the given accID using the POST payload
            var req = Request.Form;
            acc = new UserAccount(
                    AccountID: accID,
                    username: req["AccountUsername"],
                    email: req["EmailAddress"],
                    pass: req["Password"],
                    passSalt: null                      // TODO: Fix 
                );

            // Write the new Account
            uas.write(acc);
            return new OkResult();
        }


        // Update an existing Account or create if not exists
        [Route("api/accID={accID}&apikey={apiKey}")]
        [HttpPut]
        public IActionResult UpdateAllByID(int accID, string apiKey)
        {
            // TODO: Endpoint not fully implemented
            return new NotFoundResult();


            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            if (accID < 1)
            {
                return BadRequest();
            }

            var req = Request.Form;
            HashSet<string> reqAttributes = new HashSet<string>(Request.Form.Keys);
            UserAccount acc = uas.getUsingID(accID);

            // PUT requires request to provide key,value pairs for EVERY Account attribute 
            if (!reqAttributes.SetEquals(attributes))
            {
                return BadRequest();
            }

            // Create the Account with the given accID if it doesn't exist
            if (acc == null)
            {
                try
                {
                    acc = new UserAccount(
                            AccountID: accID,
                            username: req["AccountUsername"],
                            email: req["EmailAddress"],
                            pass: req["Password"],
                            passSalt: null                                  // TODO: Fix 
                        );
                }
                // Formatting or improper data typing raised exception, bad request
                catch
                {
                    return BadRequest();
                }

                // Write the new Account
                uas.write(acc);
            }

            // Otheriwse fufill the PUT request and update the corresponding Account 
            else
            {
                // accID and AccounaccID are never modifiable
                try
                {
                    // TODO: Set account methods



                }
                // Formatting or improper data typing raised exception, bad request
                catch
                {
                    return BadRequest();
                }

                // Write changes, if any
                uas.update(acc);
            }

            return new OkResult();
        }


        // Modify an existing Account without specifying all attributes in payload,
        // returns Http 404 Not found if doesn't exist
        [Route("api/accID={accID}&apikey={apiKey}")]
        [HttpPatch]
        public IActionResult UpdateByID(int accID, string apiKey)
        {
            // TODO: Endpoint not fully implemented
            return new NotFoundResult();


            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            if (accID < 1)
            {
                return BadRequest();
            }

            // Validate the attributes of the PATCH request, each attribute specified
            // in the request must be an attribute of a Account
            HashSet<string> reqAttributes = new HashSet<string>(Request.Form.Keys);
            if (!api.validAttributes(attributes, reqAttributes))
            {
                return BadRequest();
            }

            var req = Request.Form;
            UserAccount acc = uas.getUsingID(accID);

            // Http POST cannot update a Account that does not exist
            if (acc == null)
            {
                return NotFound();
            }

            // Otherwise fufill the POST request and update the corresponding Account
            // Http POST may only specify a few attributes to update or provide all of them

            // Update the Account with the specified POST attributes
            try
            {
                // TODO: UserAccount setters


            }
            // Formatting or improper data typing raised exception, bad request
            catch
            {
                return BadRequest();
            }

            // Write changes, if any
            uas.update(acc);
            return new OkResult();
        }
    }
}
