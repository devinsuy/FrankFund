using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.Models;
using System.Text.Json;
using DataAccessLayer;

namespace ServiceLayer
{
    public class SessionService
    {
        private readonly PasswordService PasswordService;
        private readonly UserAccountService UserAccountService;
        private readonly SessionDataAccess SessionDataAccess;

        public SessionService()
        {
            this.PasswordService = new PasswordService();
            this.UserAccountService = new UserAccountService();
        }

        /*
        Serialize a Session object into a String array
            Params: A  Session object to serialize
            Returns: A string array with each element in order of its column attribute (see Session DB schema)
        */
        public string[] serialize(Session sess)
        {
            return new string[] {
                sess.SessionID.ToString(),
                sess.JWTToken,
                sess.AccountUsername,
                sess.DateIssued.ToString()
            };
        }

        public ActionResult Login(string usernameoremail, string password)
        {
            // Steps for Implementation
            // 1. Check if user exists through username or email
            // 1.5. Validate Password
            // 2. Create JWT Token ?? TODO
            // 3. Create Session
            // 4. Add Session to DB | TODO
            // 5. Return JWT Token on success ?? TODO

            var user = UserAccountService.getUsingUsername(usernameoremail);
            if (user == null) // If user is not found using username, try with email
            {
                user = UserAccountService.getUsingEmail(usernameoremail);
            }

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
            //string jwtToken = JWTService.CreateToken(); // TODO: Need to look into and add JWTToken
            string jwtToken = "";
            Session session = new Session(user.AccountUsername, jwtToken);

            // Need to make Sessions table in DB in order to add
            this.SessionDataAccess.write(serialize(session));

            // Return JWT Token on success , TODO: Return JWT Token on success
            var jsonString = JsonSerializer.Serialize(session);
            //var jObject = JObject.Parse(jsonString);
            //return new OkObjectResult(jObject.ToString());
            return new OkObjectResult(jsonString);
        }
    }
}
