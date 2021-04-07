using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.Models;
using System.Text.Json;

namespace ServiceLayer
{
    public class SessionService
    {
        private readonly PasswordService PasswordService;
        private readonly UserAccountService UserAccountService;

        public SessionService()
        {
            this.PasswordService = new PasswordService();
            this.UserAccountService = new UserAccountService();
        }

        public ActionResult Login(string username, string password)
        {
            // Steps for Implementation
            // 1. Check if user exists
            // 1.5. Validate Password
            // 2. Create JWT Token ?? TODO
            // 3. Create Session
            // 4. Add Session to DB | TODO
            // 5. Return JWT Token on success ?? TODO
            var user = UserAccountService.getUsingUsername(username);

            // Check if user exists, if not found return
            if (user == null)
            {
                return new BadRequestObjectResult("User not found.");
            }

            // Validate Password
            if (!PasswordService.ValidatePassword(password, user.PasswordSalt, user.PasswordHash))
            {
                return new BadRequestObjectResult("Incorrect password.");
            }

            // Create Session
            //string jwtToken = JWTService.CreateToken(); // TODO: Look into JWTToken
            string jwtToken = "";
            Session session = new Session(user.AccountUsername, jwtToken);

            // Need to make Sessions table in DB in order to add
            // Need to make SessionDataAccess to write to DB

            // Return JWT Token on success , TODO: Return JWT Token on success
            var jsonString = JsonSerializer.Serialize(session);
            //var jObject = JObject.Parse(jsonString);
            //return new OkObjectResult(jObject.ToString());
            return new OkObjectResult(jsonString);
        }
    }
}
