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
        private readonly string transactionsID;
        public UserAccountDataAccess()
        {
            this.dataHelper = new DataHelper();
            this.tableID = dataHelper.getQualifiedTableName("Accounts");
            this.transactionsID = dataHelper.getQualifiedTableName("Transactions");
        }

        /*
        Use DataHelper.query() to GET BigQueryResults for user account from long ID
            Params: long ID - PK Identifier for Account
            Returns: BigQueryResults for found User Account
         */
        public BigQueryResults getUsingID(long ID)
        {
            string query = $"SELECT * FROM {this.tableID} WHERE AccountID = {ID}";
            // Print out query to Sprint 2 testing purposes. To be deleted in future
            //Console.WriteLine("Query performed: " + query);
            return this.dataHelper.query(query, parameters: null);
        }

        /*
        Use DataHelper.query() to GET BigQueryResults for user account from username
            Params: string username - Identifier for Account
            Returns: BigQueryResults for found User Account
         */
        public BigQueryResults getUsingUsername(string username)
        {
            string query = $"SELECT * FROM {this.tableID} WHERE AccountUsername = '{username}'";
            // Print out query to Sprint 2 testing purposes. To be deleted in future
            //Console.WriteLine("Query performed: " + query);
            return this.dataHelper.query(query, parameters: null);
        }

        /*
        Use DataHelper.query() to GET BigQueryResults for Transactions from a user account
            Params: long ID - PK Identifier for Account
            Returns: BigQueryResults for Transactions from a user account
         */
        public BigQueryResults getTransactionsFromAccount(long ID)
        {
            string query = "SELECT * FROM FrankFund.Transactions t"
                            + $" WHERE t.accountID = {ID}"
                            + " ORDER BY DateTransactionEntered";
            return this.dataHelper.query(query, parameters: null);
        }

        /*
        Use DataHelper.query() to WRITE a newly created string serialized object into BigQuery
            Params: serializedAcc : string PK Identifier for Account
            Returns: void
         */
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

        /*
        Use DataHelper.query() to DELETE an object from BigQuery given its PK identifier
            Params: accID : long PK Identifier for Account
            Returns: void
         */
        public void delete(long accID)
        {
            string query;
            query = $"DELETE FROM {this.tableID} WHERE AccountID = {accID}";
            Console.WriteLine("Running Delete Query:\n---------------------\n" + query);
            this.dataHelper.query(query);
        }

        /*
        Use DataHelper.query() to REWRITE an existing object that changed at runtime
        This method should call delete(long ID) followed by write(string[] serializedObj)
            Params: serializedAcc : string PK Identifier for Account
            Returns: void
         */
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

        public long getNextAvailID(){
            return this.dataHelper.getNextAvailID(this.tableID);
        }
    }
}
