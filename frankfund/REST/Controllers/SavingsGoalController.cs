using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DataAccessLayer.Models;
using ServiceLayer;

namespace REST.Controllers
{
    [ApiController]
    public class SavingsGoalController : ControllerBase
    {
        private readonly ILogger<SavingsGoalController> _logger;
        private readonly SavingsGoalService sgs;

        public SavingsGoalController(ILogger<SavingsGoalController> logger)
        {
            _logger = logger;
            sgs = new SavingsGoalService();
        }

        [Route("savingsgoal/{SGID}")]
        [HttpGet]
        public string GetSGByID(int SGID)
        {
            return sgs.getJSON(sgs.GetSavingsGoalUsingID(SGID));
        }
    }
}
