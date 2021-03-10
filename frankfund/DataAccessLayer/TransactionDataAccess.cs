using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.BigQuery.V2;
using DataAccessLayer.Models;

namespace DataAccessLayer
{
    public class TransactionDataAccess
    {
        private readonly DataHelper dataHelper;
        private readonly string tableID;

        public TransactionDataAccess()
        {
            this.dataHelper = new DataHelper();
            this.tableID = this.dataHelper.getQualifiedTableName("Transactions");
        }

        public BigQueryResults GetTransactionUsingID(long ID)
        {
            string query = $"SELECT * from {this.tableID} WHERE TID = {ID}";
            return this.dataHelper.query(query, parameters: null);
        }

        /* Write a Transaction to DB Params: String array of serialized newly created Transaction object */

        public void writeTransaction(string[] transaction, bool newlyCreated, bool changed)
        {
            string query;
            if (!newlyCreated)
            {
                if (changed)
                {
                    query = $"DELETE FROM {this.tableID} WHERE TID = {transaction[0]}";
                    Console.WriteLine("Transaction with TID " + transaction[0] + " was changed, updating records");
                    this.dataHelper.query(query);
                }
                else
                    return;
            }
            query = $"INSERT INTO {this.tableID} VALUES ("
                + transaction[0] + ","                                            // TransactionID
                + transaction[1] + ","                                  // AccountID
                + transaction[2] + ","                            // Transaction Name
                + transaction[3] + ","                                     // Amount of transaction
                + transaction[4] + ","                        // DateTime Transaction was made
                + transaction[5] + ","                                  // Expense or Income
                + transaction[6] + ","                        // Transaction Category
                + transaction[7] + ",";                    // DateTime Transaction entered
            Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
            this.dataHelper.query(query);
        }

        public long getNextAvailID()
        {
            return this.dataHelper.getNextAvailID(this.tableID);
        }
    }
}
