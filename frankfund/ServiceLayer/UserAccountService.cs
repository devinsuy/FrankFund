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

            // TODO: Need to add password service and salt/hash

            // If all the checks are passed then writeUserAccount to database
            this.UserAccountDataAccess.WriteUserAccount(userAccount, true, false);

            return new OkObjectResult("Account successfully created");


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

        public long getNextAvailID(){
            return UserAccountDataAccess.getNextAvailID();
        }
    }
}
