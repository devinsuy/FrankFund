using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.BigQuery.V2;
using DataAccessLayer.Models;

namespace DataAccessLayer
{
    public class UserAccountDataAccess
    {
        private readonly DataHelper dataHelper;
        private readonly string tableID;
        public UserAccountDataAccess()
        {
            this.dataHelper = new DataHelper();
            this.tableID = dataHelper.getQualifiedTableName("Accounts");
        }

        public BigQueryResults GetUserAccountUsingID(string ID)
        {
            string query = $"SELECT * FROM {tableID} WHERE AccountID = {ID}";        
            return this.dataHelper.query(query, parameters: null);
        }

        /* Write a SavingsGoal to DB
            Params: String array of serialized newly created SavingsGoal object 
        */
        public void writeUserAccount(UserAccount userAccount, bool newlyCreated, bool changed)
        {
            string query;
            if (!newlyCreated)
            {                                                      // Only write to DB a pre-existing SavingsGoal if it changed during runtime
                if (changed)
                {                                                        // SavingsGoal was updated, delete old record before reinsertion
                    query = $"DELETE FROM {this.tableID} WHERE SGID = {userAccount.AccountID}";
                    Console.WriteLine("SavingsGoal with SGID " + userAccount.AccountID + " was changed, updating records");
                    this.dataHelper.query(query);
                }
                else                                                                // SavingsGoal has not changed, nothing to write
                    return;
            }
            query = $"INSERT INTO {this.tableID} VALUES ("
                + getNextAvailID() + ","                                            // AccountID
                + userAccount.AccountUsername + ","                                 // UserName
                + userAccount.EmailAddress + ","                                    // Email Address
                + userAccount.PasswordHash + ","                                    // PasswordHash
                                                                                    // Need to add PasswordSalt
                + userAccount.FacebookID + ","                                      // FacebookID
                + userAccount.GoogleID + ",";                                     // GoogleID
            Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
            this.dataHelper.query(query);
        }

        public long getNextAvailID(){
            return this.dataHelper.getNextAvailID(this.tableID);
        }
    }
}
