using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Controllers
{
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly ILogger<ReceiptController> _logger;
        private readonly APIHelper api;
        private readonly ReceiptService rs; 

        public ReceiptController(ILogger<ReceiptController> logger)
        {
            _logger = logger;
            api = new APIHelper();
            rs = new ReceiptService();
        }

        [Route("api/RID={RID}&apikey={apikey}")]
        [HttpGet]
        public IActionResult GetByID(int RID, string apikey)
        {
            if (!api.validAPIKey(apikey))
            {
                return new UnauthorizedResult();
            }

            if(RID < 1)
            {
                return BadRequest();
            }

            return api.serveJson(rs.getJSON(rs.getUsingID(RID)));
        }

    }
}
