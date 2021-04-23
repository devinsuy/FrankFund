using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLayer;

namespace REST.Controllers
{
    public class UserAccountTransactionsController : ControllerBase
    {
        private readonly ILogger<UserAccountTransactionsController> _logger;
        private readonly APIHelper api;
        private readonly UserAccountService uas;

        public UserAccountTransactionsController(ILogger<UserAccountTransactionsController> logger)
        {
            _logger = logger;
            api = new APIHelper();
            uas = new UserAccountService();
        }


        // ------------------------------ Account Transaction Endpoints ------------------------------

        // Serve all Transactions associated with a given AccountID
        [Route("api/account/accID={accID}/Transactions&apikey={apiKey}")]
        [HttpGet]
        public IActionResult getTransactions(long accID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            return api.serveJson(uas.getTransactionsFromAccount(accID));
        }

        // Serve all Transactions associated with a given username
        [Route("api/account/user={user}/Transactions&apikey={apiKey}")]
        [HttpGet]
        public IActionResult getTransactions(string user, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            return api.serveJson(uas.getTransactionsFromAccount(user));
        }

    }
}
