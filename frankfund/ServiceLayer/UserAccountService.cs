using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLayer;
using Google.Cloud.BigQuery.V2;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ServiceLayer
{
    public class UserAccountService: Service<UserAccount>
    {
        private readonly UserAccountDataAccess UserAccountDataAccess;
        private readonly TransactionService TransactionService;
        private readonly SavingsGoalService SGService;
        private PasswordService PasswordService;
        public UserAccountService()
        {
            this.UserAccountDataAccess = new UserAccountDataAccess();
            this.PasswordService = new PasswordService();
            this.TransactionService = new TransactionService();
            this.SGService = new SavingsGoalService();
        }

        public UserAccount reinstantiate(BigQueryRow row)
        {
            return new UserAccount(
                    (long)row["AccountID"], (string)row["AccountUsername"],
                    (string)row["EmailAddress"], (string)row["PasswordHash"], (byte[])Encoding.ASCII.GetBytes((string)row["PasswordSalt"])
                //(int)row["FacebookID"], (BigQueryNumeric)row["GoogleID"].ToDecimal(LossOfPrecisionHandling.Truncate)
            );
        }

        /*
        Uses DataAccess Layer to get UserAccount via username
            Params: username - string username for user account
            Returns: A UserAccount object
         */
        public UserAccount getUsingUsername(string username)
        {
            UserAccount user = null;
            foreach (BigQueryRow row in this.UserAccountDataAccess.getUsingUsername(username))
            {
                user = reinstantiate(row);
            }
            return user;
        }

        /*
        Uses DataAccess Layer to get UserAccount via PK Identifier
            Params: ID - long PK Identifier for Account
            Returns: A UserAccount object
         */
        public UserAccount getUsingID(long ID)
        {
            UserAccount user = null;
            foreach (BigQueryRow row in this.UserAccountDataAccess.getUsingID(ID))
            {
                user = reinstantiate(row);
            }
            return user;
        }

        public UserAccount getUsingEmail(string email)
        {
            UserAccount user = null;
            foreach (BigQueryRow row in this.UserAccountDataAccess.getUsingEmail(email))
            {
                user = reinstantiate(row);
            }
            return user;
        }

        /*
        Returns all transactions that is associated with the given account ID ordered by date entered
        Copied from Kenny's TransactionService
            Params: The User Account ID 
            Returns: A list of Transactions associated with the given ID
         */
        public List<Transaction> getTransactionsFromAccount(long accID)
        {
            return TransactionService.getTransactionsFromAccount(accID);
        }

        // Return the JSON representation of the list of SavingsGoals associated with the given AccountID
        public string getSavingsGoalsFromAccount(long accID)
        {
            return SGService.getJSON(SGService.getSavingsGoalsFromAccount(accID));
        }

        // Return the JSON representation of the list of SavingsGoals associated with the given AccountUserName
        public string getSavingsGoalsFromAccount(string username)
        {
            return SGService.getJSON(SGService.getSavingsGoalsFromAccount(username));
        }



        /*
        Uses DataAccess Layer to delete via PK Identifier
            Params: accID - PK Identifier for Account
            Returns: void
         */
        public void delete(long accID)
        {
            this.UserAccountDataAccess.delete(accID);
        }

        public void deleteUsingUsername(string username)
        {
            this.UserAccountDataAccess.deleteUsingUsername(username);
        }

        public void deleteUsingEmail(string email)
        {
            this.UserAccountDataAccess.deleteUsingEmail(email);
        }


        /*
        Use DataAccess Layer to write a NEWLY CREATED object into BigQuery
            Params: userAccount - UserAccount object for inserted user account
            Returns: int -
                0 : Account Creation Success
                1 : Invalid or taken email
                2 : Username taken
                3 : Password too weak
         */
        public int write(UserAccount userAccount)
        {
            bool emailTaken = getUsingEmail(userAccount.EmailAddress) != null;
            // Checking if Email is a valid Email Address
            if (!EmailService.IsValidEmailAddress(userAccount.EmailAddress.ToLower()) || emailTaken) // Checks for valid email address
            {
                return 1;
            }

            // Checking if username already exists
            var retrievedUser = getUsingUsername(userAccount.AccountUsername);
            if (retrievedUser != null) // Checks if user already exists
            {
                Console.WriteLine("Username already exists.");
                return 2;
            }

            // Checks for Password Minimum Requirements
            if(PasswordService.CheckMinReqPassword(userAccount.PasswordHash) == false)
            {
                Console.WriteLine("Password does not meet minimum requirements.");
                return 3;
            }

            // Salts and hashes password for security concerns when storing to the database
            byte[] passwordSalt = PasswordService.GenerateSalt();
            string passwordHashed = PasswordService.HashPassword(userAccount.PasswordHash, passwordSalt);

            userAccount.PasswordSalt = passwordSalt;
            userAccount.PasswordHash = passwordHashed;

            // If all the checks are passed then writeUserAccount to database
            this.UserAccountDataAccess.write(serialize(userAccount));
            return 0;
        }

        /* TODO:
           Write a modified object's changed to BigQuery via DataAccess Layer 
               (method should have a way of checking whether the class object changed during runtime
               to avoid redundant writing. Use a changed boolean to implement this)
           Should not call DataAccess update() if did not change
            Returns: void */
        public void update(UserAccount userAccount)
        {
            // Checking if user account exists
            UserAccount retrievedUser = getUsingID(userAccount.AccountID);
            if (retrievedUser == null) // Checks if user already exists
            {
                Console.WriteLine("User Account does not exist.");
                return;
                //return new OkObjectResult("User Account does not exist");
            }
            else
            {
                // TODO: Need to add password service and salt/hash

                // Checks for Password Minimum Requirements
                if (PasswordService.CheckMinReqPassword(userAccount.PasswordHash) == false)
                {
                    Console.WriteLine("Password does not meet minimum requirements.");
                    return;
                }

                // Checking if Email is a valid Email Address
                if (!EmailService.IsValidEmailAddress(userAccount.EmailAddress.ToLower())) // Checks for valid email address
                {
                    //return new BadRequestObjectResult("Invalid Email address");
                    return;
                }

                // Checking if username already exists
                var retrievedUser2 = getUsingUsername(userAccount.AccountUsername);
                if (retrievedUser2 != null) // Checks if user already exists
                {
                    Console.WriteLine("Username already exists");
                    return;
                    //return new OkObjectResult("User already exists");
                }

                // Salts and hashes password for security concerns when storing to the database
                byte[] passwordSalt = PasswordService.GenerateSalt();
                string passwordHashed = PasswordService.HashPassword(userAccount.PasswordHash, passwordSalt);

                userAccount.PasswordSalt = passwordSalt;
                userAccount.PasswordHash = passwordHashed;

                // If all the checks are passed then writeUserAccount to database
                // with newlyCreated bool = false and changed bool = true
                this.UserAccountDataAccess.update(serialize(userAccount));
                return;
                //return new OkObjectResult("Account successfully updated");
            }
        }

        /*
        Serialize a UserAccount object into a String array
            Params: A UserAccount object to serialize
            Returns: A string array with each element in order of its column attribute (see UserAccount DB schema)
        */
        public string[] serialize(UserAccount acc)
        {
            return new string[] {
                acc.AccountID.ToString(),
                acc.AccountUsername,
                acc.EmailAddress,
                acc.PasswordHash,
                Convert.ToBase64String(acc.PasswordSalt)
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
            string[] serialized = serialize(s);
            string jsonStr = "{"
                + $"\"AccountID\":{serialized[0]},"
                + $"\"AccountUsername\":\"" + serialized[1] + "\","
                + $"\"EmailAddress\":\"" + serialized[2] + "\","
                + $"\"PasswordHash\":\"" + serialized[3] + "\","
                + $"\"PasswordSalt\":\"" + serialized[4] + "\""
            + "}";

            return jsonStr;
        }

        public long getNextAvailID(){
            return UserAccountDataAccess.getNextAvailID();
        }
    }
}
