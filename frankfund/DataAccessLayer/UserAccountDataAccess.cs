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

        public BigQueryResults GetUserAccountUsingUsername(string username)
        {
            string query = $"SELECT * FROM {this.tableID} WHERE AccountUsername = '{username}'";
            // Print out query to Sprint 2 testing purposes. To be deleted in future
            //Console.WriteLine("Query performed: " + query);
            return this.dataHelper.query(query, parameters: null);
        }

        /* Write a SavingsGoal to DB
            Params: String array of serialized newly created SavingsGoal object 
        */

        // TODO: Deprecated, please implement write(string[]) and update(string[])
        public void writeUserAccount(UserAccount userAccount, bool newlyCreated, bool changed)
        {
            string query;
            if (!newlyCreated)
            {                                                      // Only write to DB a pre-existing Account if it changed during runtime
                if (changed)
                {                                                        // Account was updated, delete old record before reinsertion
                    query = $"DELETE FROM {this.tableID} WHERE AccountID = {userAccount.AccountID}";
                    Console.WriteLine("User Account with AccountID " + userAccount.AccountID + " was changed, updating records");
                    query = $"INSERT INTO {this.tableID} VALUES ("
                        + userAccount.AccountID + ", '"               // AccountID
                        + userAccount.AccountUsername.ToString() + "' ,'"   // AccountUsername
                        + userAccount.EmailAddress.ToString() + "' ,'"      // Email Address
                        + userAccount.PasswordHash.ToString() + "' ,"       // PasswordHash
                                                                            // Need to add PasswordSalt
                        + "null" + ","                                           // FacebookID
                        + "null" + ")";                                          // GoogleID
                    Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
                    this.dataHelper.query(query);
                }
                else                                                                // SavingsGoal has not changed, nothing to write
                    return;
            }
            else {
                query = $"INSERT INTO {this.tableID} VALUES ("
                    + getNextAvailID().ToString() + ", '"               // AccountID
                    + userAccount.AccountUsername.ToString() + "' ,'"   // AccountUsername
                    + userAccount.EmailAddress.ToString() + "' ,'"      // Email Address
                    + userAccount.PasswordHash.ToString() + "' ,"       // PasswordHash
                                                                        // Need to add PasswordSalt
                    + "null" + ","                                           // FacebookID
                    + "null" + ")";                                          // GoogleID
                Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
                this.dataHelper.query(query);
            }
        }

        // TODO
        public void write(string[] serializedAcc)
        {

        }

        // TODO
        public void update(string[] serializedAcc)
        {

        }

        // TODO
        public void delete(long accID)
        {

        }

        public void DisableUserAccount(UserAccount userAccount)
        {
            string query;
            query = $"DELETE FROM {this.tableID} WHERE AccountID = {userAccount.AccountID}";
            Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
            this.dataHelper.query(query);
        }

        public long getNextAvailID(){
            return this.dataHelper.getNextAvailID(this.tableID);
        }
    }
}
