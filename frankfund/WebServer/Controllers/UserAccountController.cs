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
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (accID < 1)
            {
                return BadRequest("Invalid Account ID");
            }
            return api.serveJson(uas.getJSON(uas.getUsingID(accID)));
        }

        [Route("api/account/user={user}&apikey={apiKey}")]
        [HttpGet]
        public IActionResult GetByUsername(string user, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            return api.serveJson(uas.getJSON(uas.getUsingUsername(user)));
        }

        [Route("api/account/email={email}&apikey={apiKey}")]
        [HttpGet]
        public IActionResult GetByEmail(string email, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
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
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (accID < 1)
            {
                return BadRequest("Invalid Account ID");
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
                return new UnauthorizedObjectResult("Invalid API key");
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
                return new UnauthorizedObjectResult("Invalid API key");
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
                return new UnauthorizedObjectResult("Invalid API key");
            }

            // Validate that the POST request contains all necessary attributes to create a NEW Account and nothing more
            Dictionary<string, object> req = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(reqBody));
            HashSet<string> reqAttributes = new HashSet<string>(req.Keys);
            if (!reqAttributes.SetEquals(attributes))
            {
                return BadRequest("Request body should contain exactly { AccountUsername, EmailAddress, Password }");
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
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (accID < 1)
            {
                return BadRequest("Invalid Account ID");
            }

            // Validate the attributes of the PATCH request, each attribute specified, given account id
            // in the request must be an attribute of a Account
            Dictionary<string, object> req = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(reqBody));
            HashSet<string> reqAttributes = new HashSet<string>(req.Keys);
            if (!api.validAttributes(attributes, reqAttributes))
            {
                return BadRequest("Invalid attribute(s) in request body");
            }

            UserAccount acc = uas.getUsingID(accID);

            // Http POST cannot update a Account that does not exist
            if (acc == null)
            {
                return NotFound($"No account exists with AccountID {accID}");
            }

            // Otherwise fufill the POST request and update the corresponding Account
            // Http POST may only specify a few attributes to update or provide all of them

            // Update the Account with the specified POST attributes
            try
            {
                if (reqAttributes.Contains("AccountUsername"))
                {
                    acc.AccountUsername = Convert.ToString(req["AccountUsername"]);
                }
                if (reqAttributes.Contains("EmailAddress"))
                {
                    acc.EmailAddress = Convert.ToString(req["EmailAddress"]);
                }
                if (reqAttributes.Contains("Password"))
                {
                    acc.PasswordHash = Convert.ToString(req["Password"]);
                }
            }
            // Formatting or improper data typing raised exception, bad request
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return BadRequest();
            }

            // Write changes, if any
            uas.update(acc);
            return api.serveJson(uas.getJSON(acc));
        }

        // Modify an existing Account without specifying all attributes in payload, given account username
        // returns Http 404 Not found if doesn't exist
        [Route("api/account/user={user}&apikey={apiKey}")]
        [HttpPatch]
        public IActionResult UpdateByUsername(string user, string apiKey, [FromBody] JsonElement reqBody)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }

            // Validate the attributes of the PATCH request, each attribute specified, given account id
            // in the request must be an attribute of a Account
            Dictionary<string, object> req = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(reqBody));
            HashSet<string> reqAttributes = new HashSet<string>(req.Keys);
            if (!api.validAttributes(attributes, reqAttributes))
            {
                return BadRequest("Invalid attribute(s) in request body");
            }

            UserAccount acc = uas.getUsingUsername(user);

            // Http POST cannot update a Account that does not exist
            if (acc == null)
            {
                return NotFound($"No account exists with useruname {user}");
            }

            // Otherwise fufill the POST request and update the corresponding Account
            // Http POST may only specify a few attributes to update or provide all of them

            // Update the Account with the specified POST attributes
            try
            {
                if (reqAttributes.Contains("AccountUsername"))
                {
                    acc.AccountUsername = Convert.ToString(req["AccountUsername"]);
                }
                if (reqAttributes.Contains("EmailAddress"))
                {
                    acc.EmailAddress = Convert.ToString(req["EmailAddress"]);
                }
                if (reqAttributes.Contains("Password"))
                {
                    acc.PasswordHash = Convert.ToString(req["Password"]);
                }
            }
            // Formatting or improper data typing raised exception, bad request
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return BadRequest();
            }

            // Write changes, if any
            uas.update(acc);
            return api.serveJson(uas.getJSON(acc));
        }
    }
}
