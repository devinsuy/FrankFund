using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLayer;


namespace REST.Controllers
{
    [ApiController]
    public class SavingsGoalController : ControllerBase
    {
        private readonly ILogger<SavingsGoalController> _logger;
        private readonly APIHelper api;
        private readonly SavingsGoalService sgs;

        public SavingsGoalController(ILogger<SavingsGoalController> logger)
        {
            _logger = logger;
            api = new APIHelper();
            sgs = new SavingsGoalService();
        }

        [Route("api/SGID={SGID}&apikey={apiKey}")]
        [HttpGet]
        public IActionResult GetSGByID(int SGID, string apiKey)
        {
            if (!api.validAPIKey(apiKey))
            {
                return new UnauthorizedResult();
            }   
            if(SGID < 1) {
                return BadRequest();
            }
            return api.serveJson(sgs.getJSON(sgs.getUsingID(SGID)));
        }
    }
}
