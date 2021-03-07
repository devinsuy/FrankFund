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
            else
            {
                this.UserAccountDataAccess.writeUserAccount(userAccount, true, false);
                
                return new StatusCodeResult(500);
            }

            // TODO: Check if username already exists in the database

        }

        public string GetAccountUsingID(string ID)
        {
            var retrievedUserAccount = UserAccountDataAccess.GetUserAccountUsingID(ID);
            return retrievedUserAccount.GetEnumerator().ToString();
        }

        public long getNextAvailID(){
            return UserAccountDataAccess.getNextAvailID();
        }
    }
}
