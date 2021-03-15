using System;
using System.Collections.Generic;

namespace REST.Controllers
{
    public class APIHelper
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
    }
}
