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
                "AccountUsername", "EmailAddress", "Password"
            };
        }


        // ------------------------------ Account GET endpoints ------------------------------

        [Route("api/account/accID={accID}&apikey={apiKey}")]
        [HttpGet]
        public IActionResult GetByID(long accID, string apiKey)
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

        [Route("api/account/user={user}&apikey={apiKey}")]
        [HttpGet]
        public IActionResult GetByUsername(string user, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            return api.serveJson(uas.getJSON(uas.getUsingUsername(user)));
        }

        [Route("api/account/email={email}&apikey={apiKey}")]
        [HttpGet]
        public IActionResult GetByEmail(string email, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            return api.serveJson(uas.getJSON(uas.getUsingEmail(email)));
        }


        // ------------------------------ Account DELETE endpoints ------------------------------

        // Delete an account by id, no effect if an account with the given accID doesn't exist
        [Route("api/account/accID={accID}&apikey={apiKey}")]
        [HttpDelete]
        public IActionResult DeleteByID(long accID, string apiKey)
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

        // Delete an account by username, no effect if an account with the username doesn't exist
        [Route("api/account/user={user}&apikey={apiKey}")]
        [HttpDelete]
        public IActionResult DeleteByUsername(string user, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            uas.deleteUsingUsername(user);
            return new OkResult();
        }

        // Delete an account by email, no effect if an account with the email doesn't exist
        [Route("api/account/email={email}&apikey={apiKey}")]
        [HttpDelete]
        public IActionResult DeleteByEmail(string email, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            uas.deleteUsingEmail(email);
            return new OkResult();
        }

        // ------------------------------ Account Create endpoint ------------------------------

        // Create a new account with the next available accID
        [Route("api/account/create&apikey={apiKey}")]
        [HttpPost]
        public IActionResult Create(string apiKey, [FromBody] JsonElement reqBody)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }

            // Validate that the POST request contains all necessary attributes to create a NEW Account and nothing more
            Dictionary<string, object> req = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(reqBody));
            HashSet<string> reqAttributes = new HashSet<string>(req.Keys);
            if (!reqAttributes.SetEquals(attributes))
            {
                return BadRequest("Request body should contain exactly { AccountUsername, EmailAddress, Password}");
            }

            UserAccount acc = null;
            // Create the Account with the given accID using the POST payload
            try
            {
                acc = new UserAccount(
                        // Removed AccountID out of UserAccount creation because new account ID is assigned in UserAccountService 
                        //AccountID: accID,
                        username: Convert.ToString(req["AccountUsername"]),
                        email: Convert.ToString(req["EmailAddress"]),
                        pass: Convert.ToString(req["Password"])
                    //passSalt: null                      // TODO: Fix
                    // Removed byte[] passSalt from constructor because it gets generated in UserAccountService
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return BadRequest();
            }

            // Validate username, email, password strength
            switch(uas.write(acc))
            {
                case 1:
                    return api.serveErrorMsg("Invalid or already taken email address");
                case 2:
                    return api.serveErrorMsg("Username already taken");
                case 3:
                    return api.serveErrorMsg("Password too weak");
                default:
                    return new OkObjectResult($"Account {acc.AccountUsername} successfully registered");
            }

        }

        // ------------------------------ Account Update Endpoint ------------------------------

        // Modify an existing Account without specifying all attributes in payload,
        // returns Http 404 Not found if doesn't exist
        [Route("api/account/accID={accID}&apikey={apiKey}")]
        [HttpPatch]
        public IActionResult UpdateByID(long accID, string apiKey, [FromBody] JsonElement reqBody)
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
            Dictionary<string, object> req = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(reqBody));
            HashSet<string> reqAttributes = new HashSet<string>(req.Keys);
            if (!api.validAttributes(attributes, reqAttributes))
            {
                return BadRequest();
            }

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
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return BadRequest();
            }

            // Write changes, if any
            uas.update(acc);
            return new OkResult();
        }


        // ------------------------------ Account SavingsGoals Endpoints ------------------------------

        // Serve all SavingsGoals associated with a given Account ID
        [Route("api/account/accID={accID}/SavingsGoals&apikey={apiKey}")]
        [HttpGet]
        public IActionResult getGoals(long accID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            if (accID < 1)
            {
                return BadRequest();
            }
            return api.serveJson(uas.getSavingsGoalsFromAccount(accID));
        }

        [Route("api/account/user={user}/SavingsGoals&apikey={apiKey}")]
        [HttpGet]
        public IActionResult getGoals(string user, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            return api.serveJson(uas.getSavingsGoalsFromAccount(user));
        }

        // ------------------------------ Account Transaction Endpoints ------------------------------


    }
}
