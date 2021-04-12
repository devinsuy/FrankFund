using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;


namespace REST.Controllers
{
    public class APIHelper: ControllerBase
    {
        private readonly HashSet<string> validKeys;

        public APIHelper()
        {
            validKeys = new HashSet<string> {
                "f2f1178729cb2e1c9188ed847066743c4e843a21",     // AuthAutumn.json["private_key_id"]
                "c55f8d138f6ccfd43612b15c98706943e1f4bea3",     // AuthDevin.json["private_key_id"]
                "bd0eecf7cf275751a421a6101272f559b0391fa0",     // AuthKenny.json["private_key_id"]
                "446cc7cf5ad5efab7a1a645cb8f3efbea08cb6b4"      // AuthRachel.json["private_key_id"]
            };
        }

        public bool validAPIKey(string apiKey)
        {
            return this.validKeys.Contains(apiKey);
        }

        public IActionResult serveJson(string json)
        {
            if (json.Equals("{}"))
            {
                return NoContent();
            }
            else
            {
                json.Replace("\\", "");
                return Content(json, "application/json");
            }
        }

        public IActionResult serveErrorMsg(string msg)
        {
            return Conflict(new { message = msg });
        }

        public string getSingleAttrJSON(string key, string val)
        {
            return "{\"" + key + "\":" + val + "}";
        }

        // Validate a request body, each key should be a valid attribute of the object
        public bool validAttributes(HashSet<string> objAttributes, HashSet<string> reqAttributes)
        {
            foreach(string attr in reqAttributes)
            {
                if (!objAttributes.Contains(attr))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
