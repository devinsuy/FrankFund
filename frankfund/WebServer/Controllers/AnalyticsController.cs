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
        private readonly AnalyticsService ans;
        public AnalyticsController(ILogger<ReceiptController> logger)
        {
            _logger = logger;
            api = new APIHelper();
            ans = new AnalyticsService();
        }


        [Route("api/Analytics/CategoryPercentages/AllTime&user={user}&apikey={apikey}")]
        [HttpGet]
        public IActionResult GetAllTimeCategoryPercentages(string user, string apikey)
        {
            if (!api.validAPIKey(apikey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            return api.serveJson(ans.getJSON(ans.getAllTimeCategoryBreakdown(user)));
        }


        [Route("api/Analytics/CategorySpending/AllTime&user={user}&apikey={apikey}")]
        [HttpGet]
        public IActionResult GetAllTimeCategorySpending(string user, string apikey)
        {
            if (!api.validAPIKey(apikey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            return api.serveJson(ans.getJSON(ans.getAllTimeSpendingPerCategory(user)));
        }


        [Route("api/Analytics/MonthlySpending/PastYear&user={user}&apikey={apikey}")]
        [HttpGet]
        public IActionResult GetPastYearMonthlySpending(string user, string apikey)
        {
            if (!api.validAPIKey(apikey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            return api.serveJson(ans.getJSON(ans.getSpendingPerMonthPastYear(user)));
        }


        [Route("api/Analytics/TotalSpending/AllTime&user={user}&apikey={apikey}")]
        [HttpGet]
        public IActionResult GetAllTimeTotalSpending(string user, string apikey)
        {
            if (!api.validAPIKey(apikey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            return api.serveJson(api.getSingleAttrJSON("Total", Convert.ToString(ans.getTotalSpending(user))));
        }


        [Route("api/Analytics/TotalSavings/AllTime&user={user}&apikey={apikey}")]
        [HttpGet]
        public IActionResult GetAllTimeTotalSavings(string user, string apikey)
        {
            if (!api.validAPIKey(apikey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            return api.serveJson(api.getSingleAttrJSON("TotalSavings", Convert.ToString(ans.getTotalSavings(user))));
        }

        [Route("api/Analytics/TotalSavings/ThisYear&user={user}&apikey={apikey}")]
        [HttpGet]
        public IActionResult GetAllTimeTotalSavingsThisYear(string user, string apikey)
        {
            if (!api.validAPIKey(apikey))
            {
                return new UnauthorizedObjectResult("Invalid API key");
            }
            return api.serveJson(api.getSingleAttrJSON("TotalSavings", Convert.ToString(ans.getTotalSavingsThisYear(user))));
        }

    }
}