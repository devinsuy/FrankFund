using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Google.Cloud.BigQuery.V2;
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
            this.SessionDataAccess = new SessionDataAccess();
        }

        public Session reinstantiate(BigQueryRow row)
        {
            return new Session(
                    (long)row["SessionID"], (string)row["JWTToken"],
                    (long)row["AccountID"], (string)row["AccountUsername"],
                    (string)row["EmailAddress"], (DateTime)row["DateIssued"]
            //(int)row["FacebookID"], (BigQueryNumeric)row["GoogleID"].ToDecimal(LossOfPrecisionHandling.Truncate)
            );
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
                sess.AccountID.ToString(),
                sess.AccountUsername,
                sess.EmailAddress,
                sess.DateIssued.ToString()
            };
        }

        /*
        Uses DataAccess Layer to get Session via PK Identifier
            Params: ID - long PK Identifier for Sesion
            Returns: A Session object
         */
        public Session getUsingID(long ID)
        {
            Session session = null;
            foreach (BigQueryRow row in this.SessionDataAccess.getUsingID(ID))
            {
                session = reinstantiate(row);
            }
            return session;
        }

        public int Login(string usernameoremail, string password, string jwtToken)
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
                return 1; // return api.serveErrorMsg("User not found.");
            }

            // Validate Password, if doesn't match return incorrect password
            if (PasswordService.ValidatePassword(password, user.PasswordSalt, user.PasswordHash) == false)
            {
                return 2; // return api.serveErrorMsg("Incorrect Password");
            }

            // Create Session
            Session session = new Session(jwtToken, user.AccountID, user.AccountUsername, user.EmailAddress);

            // Need to make Sessions table in DB in order to add
            this.SessionDataAccess.write(serialize(session));

            // Return JWT Token on success , TODO: Return JWT Token on success
            //var jsonString = JsonSerializer.Serialize(session);
            //var jObject = JObject.Parse(jsonString);
            //return new OkObjectResult(jObject.ToString());
            //return new OkObjectResult(jsonString);
            return 0;
        }

        public ActionResult Logout(long sessID)
        {
            this.SessionDataAccess.delete(sessID);

            return new OkObjectResult("Successfully Logged Out");
        }

        public ActionResult Logout(string jwt)
        {
            this.SessionDataAccess.deleteJWT(jwt);

            return new OkObjectResult("Successfully Logged Out");
        }

        /*
        Convert a Session object into JSON format
            Params: A Session object to convert
            Returns: The JSON string representation of the object
        */
        public string getJSON(Session s)
        {
            if (s == null)
            {
                return "{}";
            }
            string[] serialized = serialize(s);
            string jsonStr = "{"
                + $"\"SessionID\":{serialized[0]},"
                + $"\"JWTToken\":\"" + serialized[1] + "\","
                + $"\"AccountID\":{serialized[2]},"
                + $"\"AccountUsername\":\"" + serialized[3] + "\","
                + $"\"EmailAddress\":\"" + serialized[4] + "\","
                + $"\"DateIssued\":\"" + serialized[5] + "\""
            + "}";

            return jsonStr;
        }

        public long getNextAvailID()
        {
            return SessionDataAccess.getNextAvailID();
        }
    }
}
