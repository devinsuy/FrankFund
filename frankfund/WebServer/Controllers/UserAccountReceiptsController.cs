using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLayer;

namespace REST.Controllers
{
    public class UserAccountReceiptsController : ControllerBase
    {
        private readonly ILogger<UserAccountReceiptsController> _logger;
        private readonly APIHelper api;
        private readonly UserAccountService uas;

        public UserAccountReceiptsController(ILogger<UserAccountReceiptsController> logger)
        {
            _logger = logger;
            api = new APIHelper();
            uas = new UserAccountService();
        }


        // ------------------------------ Account Receipt Endpoints ------------------------------

        // Serve all Receipts associated with a given Accoun
        [Route("api/account/accID={accID}/Receipts&apikey={apiKey}")]
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
            return api.serveJson(uas.getReceiptsFromAccount(accID));
        }

        [Route("api/account/user={user}/Receipts&apikey={apiKey}")]
        [HttpGet]
        public IActionResult getGoals(string user, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            return api.serveJson(uas.getReceiptsFromAccount(user));
        }


    }
}
