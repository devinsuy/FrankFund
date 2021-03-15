using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLayer;


namespace REST.Controllers
{
    [ApiController]
    public class LandingController : ControllerBase
    {
        private readonly ILogger<LandingController> _logger;

        public LandingController(ILogger<LandingController> logger)
        {
            _logger = logger;
        }

        [Route("")]
        [HttpGet]
        public IActionResult tempLandingPage()
        {
            string[] indexHtml = System.IO.File.ReadAllLines(@"../REST/temp_landing/index.html");
            return new OkObjectResult(string.Join("", indexHtml));
        }
    }
}
