using System;
using DataAccessLayer;
using Google.Cloud.BigQuery.V2;
using DataAccessLayer.Models;
using System.Collections.Generic;

namespace ServiceLayer
{
    public class TransactionUserAccountService
    {
        private readonly TransactionUserAccountDataAccess dataAccess;

        public TransactionUserAccountService()
        {
            this.dataAccess = new TransactionUserAccountDataAccess();
        }

        /*
        Serialize a Transaction object into a String array
            Returns: A string array with each element in order of its column attribute (see Transaction DB schema)
        */
        public string[] serialize(Transaction t)
        {
            return new string[] {
                t.getTID().ToString(),
                t.getAccountID().ToString(),
                t.getSGID().ToString(),
                t.getTransactionName().ToString(),
                t.getAmount().ToString(),
                t.getDateTransactionMade().ToString("yyyy-MM-dd"),
                t.getDateTransactionEntered().ToString("yyyy-MM-dd"),
                t.getIsExpense().ToString().ToLower(),                  // BigQuery stores boolean as {True, False}, C# stores as {true, false}
                t.getTransactionCategory().ToString()
            };
        }

        /*
        Convert a Transaction object into JSON format
            Params: A Transaction object to convert
            Returns: The JSON string representation of the object
        */
        public string getJSON(Transaction t)
        {
            if (t == null)
            {
                return "{}";
            }
            string[] serialized = serialize(t);
            string jsonStr = "{"
                + $"\"TID\":{serialized[0]},"
                + $"\"AccountID\":{serialized[1]},"
                + $"\"SGID\":{serialized[2]},"
                + $"\"TransactionName\":\"" + serialized[3] + "\","
                + $"\"Amount\":{serialized[4]},"
                + $"\"DateTransactionMade\":\"" + serialized[5] + "\","
                + $"\"DateTransactionEntered\":\"" + serialized[6] + "\","
                + $"\"IsExpense\":{serialized[7]},"
                + $"\"TransactionCategory\":\"" + serialized[8] + "\""
            + "}";
            return jsonStr;
        }

        /*
        Returns all transactions that is associated with the given account ID ordered by date entered
            Params: The User Account ID 
            Returns: A list of Transactions associated with the given ID
         */
        public List<Transaction> getTransactionsFromAccount(long accID)
        {
            List<Transaction> transactionsList = new List<Transaction>();

            foreach (BigQueryRow row in this.dataAccess.getTransactionsFromAccount(accID))
            {
                Transaction transaction = null;
                long SGID = -1;     // Nullable attr
                if (row["SGID"] != null)
                {
                    SGID = (long)row["SGID"];
                }
                transaction = new Transaction(
                    (long)row["TID"], (long)row["AccountID"], SGID,
                    (string)row["TransactionName"],
                    this.dataAccess.castBQNumeric(row["Amount"]),
                    (DateTime)row["DateTransactionMade"],
                    (DateTime)row["DateTransactionEntered"],
                    (bool)row["IsExpense"],
                    this.dataAccess.ParseEnum<transactionCategory>((string)row["TransactionCategory"])
                );
                transactionsList.Add(transaction);
            }
            return transactionsList;
        }

        /*
        Returns all transactions associated with the given user account ID and sorted by the given category.
            Params: The User Account ID
                    The category
            Returns: A list of Transactions associated with user account sorted by category
        */
        public List<Transaction> getTransactionsFromCategory(long accID, string category)
        {
            List<Transaction> transactionsList = new List<Transaction>();
            foreach (BigQueryRow row in this.dataAccess.getTransactionsFromCategory(accID, category))
            {
                Transaction transaction = null;
                long SGID = -1;     // Nullable attr
                if (row["SGID"] != null)
                {
                    SGID = (long)row["SGID"];
                }
                transaction = new Transaction(
                    (long)row["TID"], (long)row["AccountID"], SGID,
                    (string)row["TransactionName"],
                    this.dataAccess.castBQNumeric(row["Amount"]),
                    (DateTime)row["DateTransactionMade"],
                    (DateTime)row["DateTransactionEntered"],
                    (bool)row["IsExpense"],
                    this.dataAccess.ParseEnum<transactionCategory>((string)row["TransactionCategory"])
                );
                transactionsList.Add(transaction);
            }
            return transactionsList;
        }
    }
}
