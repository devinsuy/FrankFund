using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLayer;


namespace REST.Controllers
{
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly ILogger<UserAccountController> _logger;
        private readonly APIHelper api;
        private readonly UserAccountService uas;

        public UserAccountController(ILogger<UserAccountController> logger)
        {
            _logger = logger;
            api = new APIHelper();
            uas = new UserAccountService();
        }

        [Route("api/accID={accID}&apikey={apiKey}")]
        [HttpGet]
        public IActionResult GetSGByID(int accID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }
            if (accID < 1)
            {
                return BadRequest();
            }
            long maxAccID = uas.getNextAvailID() - 1;
            if (accID > maxAccID)
            {
                return NoContent();
            }
            return new OkObjectResult(uas.getJSON(uas.GetAccountUsingID(accID)));
        }
    }
}
