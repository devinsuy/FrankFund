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
        private readonly TransactionService ts;

        public UserAccountTransactionsController(ILogger<UserAccountTransactionsController> logger)
        {
            _logger = logger;
            api = new APIHelper();
            uas = new UserAccountService();
            ts = new TransactionService();
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

        // ------------------------------ Account Transactions Filtered To Required Attribute ------------------------------

        // Serve all Transactions associated with a given account, returns only transactions with
        // the specified category
        // returns Http 204 NoContent if doesn't exist
        [Route("api/account/user={user}/Transactions/Filter/WithCategory={category}&apikey={apikey}")]
        [HttpGet]
        public IActionResult GetAllTransactionsCategorySorted(string user, string category, string apikey)
        {
            if (!api.validAPIKey(apikey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            return api.serveJson(ts.getJSON(ts.getTransactionsFromCategory(user, category)));
        }
        [Route("api/account/accID={accID}/Transactions/Filter/WithCategory={category}&apikey={apikey}")]
        [HttpGet]
        public IActionResult GetAllTransactionsCategorySorted(long accID, string category, string apikey)
        {
            if (!api.validAPIKey(apikey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            return api.serveJson(ts.getJSON(ts.getTransactionsFromCategory(accID, category)));
        }


        // Serve all Transactions associated with a given accountID, sorted by the TransactionCategory attribute
        // Params:
        //      The number of days, weeks, months, or years
        //      The choice of sorting; 0 = days, 1 = weeks, 2 = months, 3 = years
        // returns Http 204 NoContent if doesn't exist
        [Route("api/account/accID={accID}/Transactions/Filter/ByTime/num={num}&periodCode={periodCode}&apikey={apikey}")]
        [HttpGet]
        public IActionResult GetAllTransactionsTimeSorted(long accID, int num, int periodCode, string apikey)
        {
            if (!api.validAPIKey(apikey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (accID < 1)
            {
                return BadRequest("Invalid Account ID");
            }
            if (periodCode > 3 || periodCode < 0)
            {
                return BadRequest("Invalid Period Code specified");
            }

            return api.serveJson(ts.getJSON(ts.getSortedTransactionsByTime(accID, num, periodCode)));
        }
        [Route("api/account/user={user}/Transactions/Filter/ByTime/num={num}&periodCode={periodCode}&apikey={apikey}")]
        [HttpGet]
        public IActionResult GetAllTransactionsTimeSorted(string user, int num, int periodCode, string apikey)
        {
            if (!api.validAPIKey(apikey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (periodCode > 3 || periodCode < 0)
            {
                return BadRequest("Invalid Period Code specified");
            }

            return api.serveJson(ts.getJSON(ts.getSortedTransactionsByTime(user, num, periodCode)));
        }


        // ------------------------------ Account Transactions Sorted By Attribute ------------------------------

        // Serve all Transactions associated with a given account, sorted by the TransactionCategory attribute
        // returns Http 204 NoContent if doesn't exist
        [Route("api/account/user={user}/Transactions/Sorted/ByCategory&apikey={apikey}")]
        [HttpGet]
        public IActionResult GetAllTransactionsCategorySorted(string user, string apikey)
        {
            if (!api.validAPIKey(apikey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            return api.serveJson(ts.getJSON(ts.getAllTransactionsCategorySorted(user)));
        }
        [Route("api/account/accID={accID}/Transactions/Sorted/ByCategory&apikey={apikey}")]
        [HttpGet]
        public IActionResult GetAllTransactionsCategorySorted(long accID, string apikey)
        {
            if (!api.validAPIKey(apikey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            if (accID < 1)
            {
                return BadRequest("Invalid Account ID");
            }
            return api.serveJson(ts.getJSON(ts.getAllTransactionsCategorySorted(accID)));
        }

    }
}
