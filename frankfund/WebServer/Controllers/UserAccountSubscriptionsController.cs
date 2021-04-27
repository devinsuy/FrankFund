using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLayer;
using System;

namespace REST.Controllers
{
    public class UserAccountSubscriptionsController : ControllerBase
    {
        private readonly ILogger<UserAccountSubscriptionsController> _logger;
        private readonly APIHelper api;
        private readonly UserAccountService uas;

        public UserAccountSubscriptionsController(ILogger<UserAccountSubscriptionsController> logger)
        {
            _logger = logger;
            api = new APIHelper();
            uas = new UserAccountService();
        }


        // ------------------------------ Account Subscriptions Endpoints ------------------------------

        // Serve all SavingsGoals associated with a given Account ID
        [Route("api/account/accID={accID}/Subscriptions&apikey={apiKey}")]
        [HttpGet]
        public IActionResult getSubscriptions(long accID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (accID < 1)
            {
                return BadRequest("Invalid Account ID");
            }
            return api.serveJson(uas.getSubscriptionsFromAccount(accID));
        }

        [Route("api/account/user={user}/Subscriptions&apikey={apiKey}")]
        [HttpGet]
        public IActionResult getSubscriptions(string user, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            return api.serveJson(uas.getSubscriptionsFromAccount(user));
        }

        [Route("api/account/user={user}/Subscriptions/count&apikey={apiKey}")]
        [HttpGet]
        public IActionResult getSubscriptionCount(string user, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            return api.serveJson(api.getSingleAttrJSON("SubCount", Convert.ToString(uas.getUserSubscriptionCount(user))));
        }


    }
}
