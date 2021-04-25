using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using ServiceLayer;
using System.Text.Json;
using Newtonsoft.Json;
using JWT.Builder;
using JWT.Algorithms;

namespace REST.Controllers
{
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;
        private readonly APIHelper api;
        private readonly SessionService ss;
        private readonly UserAccountService uas;
        private readonly HashSet<string> attributes;
        private readonly HashSet<string> nullableAttrs;
        const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

        public SessionController(ILogger<SessionController> logger)
        {
            _logger = logger;
            api = new APIHelper();
            ss = new SessionService();
            uas = new UserAccountService();
            attributes = new HashSet<string>()
            {
                "usernameoremail", "password"
            };
        }

        private string generateJwtToken(string user)
        {
            var token = JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                      .WithSecret(secret)
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                      .AddClaim("claim2", "claim2-value")
                      .Encode();

            Console.WriteLine(token);
            return token;
        }

        // ------------------------------ Session GET endpoints ------------------------------

        [Route("api/account/sessID={accID}&apikey={apiKey}")]
        [HttpGet]
        public IActionResult GetByID(long sessID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (sessID < 1)
            {
                return BadRequest();
            }
            return api.serveJson(ss.getJSON(ss.getUsingID(sessID)));
        }

        // ------------------------------ Session DELETE endpoints ------------------------------

        // Delete an account by id, no effect if an account with the given accID doesn't exist
        [Route("api/account/sessID={accID}&apikey={apiKey}")]
        [HttpDelete]
        public IActionResult DeleteByID(long sessID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (sessID < 1)
            {
                return BadRequest();
            }
            ss.Logout(sessID);
            return new OkResult();
        }

        // ------------------------------ Account Create endpoint ------------------------------

        // Create a new account with the next available accID
        [Route("api/session/create&apikey={apiKey}")]
        [HttpPost]
        public IActionResult Create(string apiKey, [FromBody] JsonElement reqBody)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }

            // Validate that the POST request contains all necessary attributes to create a NEW Session and nothing more
            Dictionary<string, object> req = JsonConvert.DeserializeObject<Dictionary<string, object>>(Convert.ToString(reqBody));
            HashSet<string> reqAttributes = new HashSet<string>(req.Keys);
            if (!reqAttributes.SetEquals(attributes))
            {
                return BadRequest("Request body should contain exactly {usernameoremail, password}");
            }

            Session sess = null;
            var jwt = generateJwtToken(Convert.ToString(req["usernameoremail"]));
            // Create the Session with the given variables using the POST payload
            try
            {
                UserAccount user = uas.getUsingUsername(Convert.ToString(req["usernameoremail"]));
                if (user == null) // if user is not found using username, try with email
                {
                    user = uas.getUsingEmail(Convert.ToString(req["usernameoremail"]));
                }

                // create a new session with jwttoken, user.accountid, user.accountusername, user.emailaddress
                sess = new Session(
                        // removed sessionid out of session creation because new id is assigned in sessionservice 
                        jwtToken: jwt,
                        accID: user.AccountID,
                        userName: user.AccountUsername,
                        email: user.EmailAddress
                //date: convert.todatetime(req["dateissued"])
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return BadRequest();
            }

            // Calls login function for SessionService
            switch (ss.Login(Convert.ToString(req["usernameoremail"]), Convert.ToString(req["password"]), jwt))
            {
                case 1:
                    return api.serveErrorMsg("User not found.");
                case 2:
                    return api.serveErrorMsg("Incorrect Password");
                default:
                    //return new OkObjectResult($"Login session created for {Convert.ToString(req["usernameoremail"])}");
                    return api.serveJson(ss.getJSON(sess));
            }
        }
    }
}
