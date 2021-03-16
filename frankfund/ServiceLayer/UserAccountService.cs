using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLayer;
using Google.Cloud.BigQuery.V2;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ServiceLayer
{
    public class UserAccountService
    {
        private readonly UserAccountDataAccess UserAccountDataAccess;
        public UserAccountService()
        {
            this.UserAccountDataAccess = new UserAccountDataAccess();
        }

        public ActionResult CreateUserAccount(UserAccount userAccount)
        {
            // Checking if Email is a valid Email Address
            if (!EmailService.IsValidEmailAddress(userAccount.EmailAddress.ToLower())) // Checks for valid email address
            {
                return new BadRequestObjectResult("Invalid Email address");
            }

            // Checking if username already exists
            var retrievedUser = GetAccountUsingUsername(userAccount.AccountUsername);
            if (retrievedUser != null) // Checks if user already exists
            {
                Console.WriteLine("Username already exists.");
                return new OkObjectResult("User already exists");
            }

            // TODO: Need to add password service and salt/hash and meets requirements
            // Minimum Requirements
            // Uppercase letter (A-Z)
            // Lowercase letter(a-z)
            // Digit(0 - 9)
            // Special Character(~`!@#$%^&*()+=_-{}[]\|:;”’?/<>,.)


            // If all the checks are passed then writeUserAccount to database
            this.UserAccountDataAccess.WriteUserAccount(userAccount, true, false);

            return new OkObjectResult("Account successfully created");


        }

        public ActionResult UpdateUserAccount(UserAccount userAccount)
        {

            // Checking if user account exists
            var retrievedUser = GetAccountUsingUsername(userAccount.AccountUsername);
            if (retrievedUser == null) // Checks if user already exists
            {
                Console.WriteLine("User Account does not exist.");
                return new OkObjectResult("UUser Account does not exist");
            }
            else
            {
                // TODO: Need to add password service and salt/hash and meets requirements
                // Minimum Requirements
                // Uppercase letter (A-Z)
                // Lowercase letter(a-z)
                // Digit(0 - 9)
                // Special Character(~`!@#$%^&*()+=_-{}[]\|:;”’?/<>,.)

                // Checking if Email is a valid Email Address
                if (!EmailService.IsValidEmailAddress(userAccount.EmailAddress.ToLower())) // Checks for valid email address
                {
                    return new BadRequestObjectResult("Invalid Email address");
                }

                // Checking if username already exists
                var retrievedUser2 = GetAccountUsingUsername(userAccount.AccountUsername);
                if (retrievedUser2 != null) // Checks if user already exists
                {
                    Console.WriteLine("Username already exists");
                    return new OkObjectResult("User already exists");
                }

                // If all the checks are passed then writeUserAccount to database
                // with newlyCreated bool = false and changed bool = true
                this.UserAccountDataAccess.WriteUserAccount(userAccount, false, true);
                return new OkObjectResult("Account successfully updated");
            }
        }

        public ActionResult DisableUserAccount(UserAccount userAccount, bool confirm)
        {
            UserAccount user = GetAccountUsingUsername(userAccount.AccountUsername);

            // Need to add password validation as a input and password service

            // User Confirmation to Delete
            if (confirm == true)
            {
                this.UserAccountDataAccess.DisableUserAccount(userAccount);
                 return new OkObjectResult("User successfully deleted");
            }
            else
            {
                return new StatusCodeResult(500);
            }
        }

        public UserAccount GetAccountUsingUsername(string username)
        {
            UserAccount user = null;
            foreach (BigQueryRow row in this.UserAccountDataAccess.GetUserAccountUsingUsername(username))
            {
                user = new UserAccount(
                    (long)row["AccountID"], (string)row["AccountUsername"],
                    (string)row["EmailAddress"], (string)row["Password"], null // Need to Add Password Salt
                    //(int)row["FacebookID"], (BigQueryNumeric)row["GoogleID"].ToDecimal(LossOfPrecisionHandling.Truncate)
                );
            }
            return user;
        }

        public UserAccount GetAccountUsingID(int ID)
        {
            UserAccount user = null;
            foreach (BigQueryRow row in this.UserAccountDataAccess.GetUserAccountUsingID(ID))
            {
                user = new UserAccount(
                    (long)row["AccountID"], (string)row["AccountUsername"],
                    (string)row["EmailAddress"], (string)row["Password"], null // Need to Add Password Salt
                    //(int)row["FacebookID"], (int)row["GoogleID"]
                );
            }
            return user;
        }

        /*
        Serialize a UserAccount object into a String array
            Returns: A string array with each element in order of its column attribute (see UserAccount DB schema)
        */
        public string[] serializeUserAccount(UserAccount acc)
        {
            return new string[] {
                acc.AccountID.ToString(),
                acc.AccountUsername,
                acc.EmailAddress,
                acc.PasswordHash
            };
        }

        /*
        Convert a UserAccount object into JSON format
            Params: A UserAccount object to convert
            Returns: The JSON string representation of the object
        */
        public string getJSON(UserAccount s)
        {
            if (s == null)
            {
                return "{}";
            }
            string[] serialized = serializeUserAccount(s);
            string jsonStr = "{"
                + $"\"AccountID\":{serialized[0]},"
                + $"\"AccountUsername\":\"" + serialized[1] + "\","
                + $"\"EmailAddress\":\"" + serialized[2] + "\","
                + $"\"Password\":\"" + serialized[3] + "\""
            + "}";

            return jsonStr;
        }

        public long getNextAvailID(){
            return UserAccountDataAccess.getNextAvailID();
        }
    }
}
