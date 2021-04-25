using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using DataAccessLayer.Models;
using ServiceLayer;
using System;
using System.Text.Json;
using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace REST.Controllers
{
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly ILogger<ReceiptController> _logger;
        private readonly APIHelper api;
        private readonly TransactionService ts;
        private readonly HashSet<string> attributes;

        public AnalyticsController(ILogger<ReceiptController> logger)
        {
            _logger = logger;
            api = new APIHelper();
            ts = new TransactionService();
        }

        // Retreive a receipt with the given RID
        // returns Http 204 NoContent if doesn't exist
        [Route("api/Analytics/TransactionsByCategory&apikey={apikey}&username={username}")]
        [HttpGet]
        public IActionResult GetAllTransactionsCategorySorted(string apikey, string username)
        {
            if (!api.validAPIKey(apikey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            return null; // Autumn commented out the below code because of build errors
            //return api.serveJson(ts.getJSON(ts.getTransactionsFromAccountCategorySorted(username)));
        }
    }
}