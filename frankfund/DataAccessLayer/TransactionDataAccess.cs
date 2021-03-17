using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.BigQuery.V2;
using DataAccessLayer.Models;

namespace DataAccessLayer
{
    public class TransactionDataAccess: DataAccess
    {
        private readonly DataHelper dataHelper;
        private readonly string tableID;

        public TransactionDataAccess()
        {
            this.dataHelper = new DataHelper();
            this.tableID = this.dataHelper.getQualifiedTableName("Transactions");
        }

        public BigQueryResults getUsingID(long ID)
        {
            string query = $"SELECT * from {this.tableID} WHERE TID = {ID}";
            return this.dataHelper.query(query, parameters: null);
        }

        /* Write a Transaction to DB Params: String array of serialized newly created Transaction object */

        // TODO: Deprecated, please implement write(string[]) and update(string[])
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
                + transaction[2] + ","                                  //SGID
                + $"\"{transaction[3]}\","                            // Transaction Name
                + transaction[4] + ","                                     // Amount of transaction
                + $"\"{transaction[5]}\","                        // DateTime Transaction was made
                + $"\"{transaction[6]}\","                                  // DateTime Transaction entered
                + transaction[7] + ","                        // Expense or Income
                + $"\"{transaction[8]}\")";                    // Transaction Category
            Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
            this.dataHelper.query(query);
        }

        // TODO
        public void write(string[] serializedTransaction)
        {

        }

        // TODO
        public void update(string[] serializedTransaction)
        {

        }

        // TODO
        public void delete(long TID)
        {

        }

        public long getNextAvailID()
        {
            return this.dataHelper.getNextAvailID(this.tableID);
        }

        // Overload wrappers to cast BigQuery Numeric type to C# decimal type
        public decimal castBQNumeric(BigQueryNumeric val)
        {
            return this.dataHelper.castBQNumeric(val);
        }
        public decimal castBQNumeric(object val)
        {
            return this.dataHelper.castBQNumeric(val);
        }
    }
}
