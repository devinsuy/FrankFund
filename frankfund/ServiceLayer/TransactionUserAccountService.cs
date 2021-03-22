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
    }
}
