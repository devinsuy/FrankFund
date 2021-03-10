using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLayer;
using Google.Cloud.BigQuery.V2;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ServiceLayer
{
    class TransactionService
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
            Transaction transaction = null;
            foreach (BigQueryRow row in this.TransactionDataAccess.GetTransactionUsingID(TID))
            {
                transaction = new Transaction(
                    (long)row["TID"], (long)row["accountID"], (long)row["SGID"],
                    (string)row["transactionName"],
                    ((BigQueryNumeric)row["amount"]).ToDecimal(LossOfPrecisionHandling.Truncate),
                    (DateTime)row["dateTransactionMade"],
                    (bool)row["isExpense"],
                    (string)row["transactionCategory"]
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
