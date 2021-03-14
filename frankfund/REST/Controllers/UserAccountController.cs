using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using ServiceLayer;

namespace FrankFund.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly string projectID;
        [HttpGet("{ID}")]
        [Route("api/account/get/")]
        public ActionResult GetAccountUsingID(string ID)
        {
            //UserAccountService uas = new UserAccountService(BigQueryClient.Create(projectID));
            return new OkObjectResult(ID);
        }
    }
}
