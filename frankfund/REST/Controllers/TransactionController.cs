using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLayer;


namespace REST.Controllers
{
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly APIHelper api;
        private readonly TransactionService ts;

        public TransactionController(ILogger<TransactionController> logger)
        {
            _logger = logger;
            api = new APIHelper();
            ts = new TransactionService();
        }

        [Route("api/TID={TID}&apikey={apiKey}")]
        [HttpGet]
        public IActionResult GetSGByID(int TID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            if (TID < 1)
            {
                return BadRequest();
            }
            long maxTID = ts.getNextAvailTID() - 1;
            if (TID > maxTID)
            {
                return NoContent();
            }
            return new OkObjectResult(ts.getJSON(ts.GetTransactionUsingID(TID)));
        }
    }
}
