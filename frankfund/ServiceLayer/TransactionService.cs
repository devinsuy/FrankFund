using System;
using DataAccessLayer;
using Google.Cloud.BigQuery.V2;
using DataAccessLayer.Models;

namespace ServiceLayer
{
    public class TransactionService
    {
        private readonly TransactionDataAccess TransactionDataAccess;

        public TransactionService()
        {
            this.TransactionDataAccess = new TransactionDataAccess();
        }

        /* Retrieve a SavingsGoal from db with a given SGID
            Params: The SGID of the Savings Goal to retrieve
            Returns: A reinstantiated Savings Goal matching the SGID or null if non existant
        */
        public Transaction GetTransactionUsingID(long TID)
        {
            long SGID = -1; // Nullable attribute
            Transaction transaction = null;
            foreach (BigQueryRow row in this.TransactionDataAccess.GetTransactionUsingID(TID))
            {
                if(row["SGID"] != null)
                {
                    SGID = (long)row["SGID"];
                }
                transaction = new Transaction(
                    (long)row["TID"], (long)row["AccountID"], SGID,
                    (string)row["TransactionName"],
                    this.TransactionDataAccess.castBQNumeric(row["Amount"]),
                    (DateTime)row["DateTransactionMade"],
                    (bool)row["IsExpense"],
                    (string)row["TransactionCategory"]
                );
            }
            return transaction;
        }

        /*
        Serialize a Transaction object into a String array
            Returns: A string array with each element in order of its column attribute (see Transaction DB schema)
        */
        public string[] serializeTransaction(Transaction t)
        {
            return new string[] {
                t.getTID().ToString(),
                t.getAccountID().ToString(),
                t.getSGID().ToString(),
                t.getTransactionName().ToString(),
                t.getAmount().ToString(),
                t.getDateTransactionMade().ToString("yyyy-MM-dd"),
                t.getDateTransactionEntered().ToString("yyyy-MM-dd"),
                t.getIsExpense().ToString(),
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
            string[] serialized = serializeTransaction(t);
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
        Serialize a Transaction object and write it to the DB
            Params: t - Transaction runtime object
        */
        public void AddTransaction(Transaction t)
        {
            TransactionDataAccess.writeTransaction(this.serializeTransaction(t), t.newlyCreated, t.changed);
        }

        /* Wrapper method, query DB for next available TID
            Returns: Next available TID (1 + the maximum TID currently in the DB)
        */
        public long getNextAvailTID()
        {
            return TransactionDataAccess.getNextAvailID();
        }
    }
}
