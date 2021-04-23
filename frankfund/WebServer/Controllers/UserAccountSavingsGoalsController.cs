using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLayer;

namespace REST.Controllers
{
    public class UserAccountSavingsGoalsController : ControllerBase
    {
        private readonly ILogger<UserAccountSavingsGoalsController> _logger;
        private readonly APIHelper api;
        private readonly UserAccountService uas;

        public UserAccountSavingsGoalsController(ILogger<UserAccountSavingsGoalsController> logger)
        {
            _logger = logger;
            api = new APIHelper();
            uas = new UserAccountService();
        }


        // ------------------------------ Account SavingsGoals Endpoints ------------------------------

        // Serve all SavingsGoals associated with a given Account ID
        [Route("api/account/accID={accID}/SavingsGoals&apikey={apiKey}")]
        [HttpGet]
        public IActionResult getGoals(long accID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
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
                return new UnauthorizedObjectResult("Invalid API key");
            }
            return api.serveJson(uas.getSavingsGoalsFromAccount(user));
        }


    }
}
