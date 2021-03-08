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

        public BigQueryResults GetTransactionUsingID(int ID)
        {
            string query = $"SELECT * from {this.tableID} WHERE TID = {ID}";
            return this.dataHelper.query(query, parameters: null);
        }

        /* Write a Transaction to DB Params: String array of serialized newly created Transaction object */

        public void writeTransaction(Transaction transaction, bool newlyCreated, bool changed)
        {
            string query;
            if (!newlyCreated)
            {
                if (changed)
                {
                    query = $"DELETE FROM {this.tableID} WHERE SGID = {transaction.getTID()}";
                    Console.WriteLine("Transaction with TID " + transaction.getTID() + " was changed, updating records");
                    this.dataHelper.query(query);
                }
                else
                    return;
            }
            query = $"INSERT INTO {this.tableID} VALUES ("
                + getNextAvailID() + ","                                            // TransactionID
                + transaction.getAccountID() + ","                                  // AccountID
                + transaction.getTransactionName() + ","                            // Transaction Name
                + transaction.getDateTransactionMade() + ","                        // DateTime Transaction was made
                + transaction.getIsExpense() + ","                                  // Expense or Income
                + transaction.getTransactionCategory() + ","                        // Transaction Category
                + transaction.getDateTransactionEntered() + ",";                    // DateTime Transaction entered
            Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
            this.dataHelper.query(query);
        }

        public long getNextAvailID()
        {
            return this.dataHelper.getNextAvailID(this.tableID);
        }
    }
}
