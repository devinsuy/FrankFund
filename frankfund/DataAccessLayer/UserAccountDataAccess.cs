using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.BigQuery.V2;
using DataAccessLayer.Models;

namespace DataAccessLayer
{
    public class UserAccountDataAccess: DataAccess
    {
        private readonly DataHelper dataHelper;
        private readonly string tableID;
        public UserAccountDataAccess()
        {
            this.dataHelper = new DataHelper();
            this.tableID = dataHelper.getQualifiedTableName("Accounts");
        }

        public BigQueryResults getUsingID(long ID)
        {
            string query = $"SELECT * FROM {this.tableID} WHERE AccountID = {ID}";
            // Print out query to Sprint 2 testing purposes. To be deleted in future
            //Console.WriteLine("Query performed: " + query);
            return this.dataHelper.query(query, parameters: null);
        }

        public BigQueryResults getUsingUsername(string username)
        {
            string query = $"SELECT * FROM {this.tableID} WHERE AccountUsername = '{username}'";
            // Print out query to Sprint 2 testing purposes. To be deleted in future
            //Console.WriteLine("Query performed: " + query);
            return this.dataHelper.query(query, parameters: null);
        }

        // Use DataHelper.query() to WRITE a newly created string serialized object into BigQuery
        // @serializedAcc : string PK Identifier for Account
        public void write(string[] serializedAcc)
        {
            string query;
            query = $"INSERT INTO {this.tableID} VALUES ("
                + getNextAvailID().ToString() + ","               // AccountID
                + $"\"{serializedAcc[1]}\","                        // AccountUsername
                + $"\"{serializedAcc[2]}\","                        // Email Address
                + $"\"{serializedAcc[3]}\","                        // PasswordHash
                + $"\"{serializedAcc[4]}\","                         // PasswordSalt
                + "null" + ","                                           // FacebookID
                + "null" + ")";                                         // GoogleID
            Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
            this.dataHelper.query(query);
        }


        // Use DataHelper.query() to DELETE an object from BigQuery given its PK identifier
        // @accID : long PK Identifier for Account
        public void delete(long accID)
        {
            string query;
            query = $"DELETE FROM {this.tableID} WHERE AccountID = {accID}";
            Console.WriteLine("Running Delete Query:\n---------------------\n" + query);
            this.dataHelper.query(query);
        }

        /* Use DataHelper.query() to REWRITE an existing object that changed at runtime
           This method should call delete(long ID) followed by write(string[] serializedObj) */
        // @serializedAcc : string PK Identifier for Account
        public void update(string[] serializedAcc)
        {
            delete(long.Parse(serializedAcc[0])); // Call delete(long ID) followed by write(string[] serializedObj)
            Console.WriteLine("User Account with AccountID " + serializedAcc[0] + " was changed, updating records");
            string query;
            query = $"INSERT INTO {this.tableID} VALUES ("
                + serializedAcc[0] + ","                          // AccountID
                + $"\"{serializedAcc[1]}\","                        // AccountUsername
                + $"\"{serializedAcc[2]}\","                        // Email Address
                + $"\"{serializedAcc[3]}\","                        // PasswordHash
                + $"\"{serializedAcc[4]}\","                         // PasswordSalt
                + "null" + ","                                           // FacebookID
                + "null" + ")";                                          // GoogleID
            Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
            this.dataHelper.query(query);
        }

        // --------------------------- DEPRECATED 3/17 ----------------------------
        // TODO: Deprecated, please implement write(string[]) and update(string[])
        //public void writeUserAccount(UserAccount userAccount, bool newlyCreated, bool changed)
        //{
        //    string query;
        //    if (!newlyCreated)
        //    {                                                      // Only write to DB a pre-existing Account if it changed during runtime
        //        if (changed)
        //        {                                                        // Account was updated, delete old record before reinsertion
        //            query = $"DELETE FROM {this.tableID} WHERE AccountID = {userAccount.AccountID}";
        //            Console.WriteLine("User Account with AccountID " + userAccount.AccountID + " was changed, updating records");
        //            query = $"INSERT INTO {this.tableID} VALUES ("
        //                + userAccount.AccountID + ", '"               // AccountID
        //                + userAccount.AccountUsername.ToString() + "' ,'"   // AccountUsername
        //                + userAccount.EmailAddress.ToString() + "' ,'"      // Email Address
        //                + userAccount.PasswordHash.ToString() + "' ,"       // PasswordHash
        //                                                                    // Need to add PasswordSalt
        //                + "null" + ","                                           // FacebookID
        //                + "null" + ")";                                          // GoogleID
        //            Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
        //            this.dataHelper.query(query);
        //        }
        //        else                                                                // SavingsGoal has not changed, nothing to write
        //            return;
        //    }
        //    else {
        //        query = $"INSERT INTO {this.tableID} VALUES ("
        //            + getNextAvailID().ToString() + ", '"               // AccountID
        //            + userAccount.AccountUsername.ToString() + "' ,'"   // AccountUsername
        //            + userAccount.EmailAddress.ToString() + "' ,'"      // Email Address
        //            + userAccount.PasswordHash.ToString() + "' ,"       // PasswordHash
        //                                                                // Need to add PasswordSalt
        //            + "null" + ","                                           // FacebookID
        //            + "null" + ")";                                          // GoogleID
        //        Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
        //        this.dataHelper.query(query);
        //    }
        //}

        //public void DisableUserAccount(UserAccount userAccount)
        //{
        //    string query;
        //    query = $"DELETE FROM {this.tableID} WHERE AccountID = {userAccount.AccountID}";
        //    Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
        //    this.dataHelper.query(query);
        //}
        // --------------------------- DEPRECATED 3/17 ----------------------------

        public long getNextAvailID(){
            return this.dataHelper.getNextAvailID(this.tableID);
        }
    }
}
