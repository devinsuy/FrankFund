using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Models
{
    public class Transaction
    {
        //Transaction ID
        private readonly long TID;
        //Account ID
        private readonly long accountID;
        //private readonly long SGID;
        //Name of Transaction
        private string transactionName;
        //Amount of Transaction
        private decimal amount;
        //Date transaction made
        private DateTime dateTransactionMade;
        //Is an expense or income
        private bool isExpense;
        //Category of transaction
        private string transactionCategory;
        //Date transaction was entered
        private DateTime dateTransactionEntered;

        public bool changed;
        public bool newlyCreated;

        //Constructor
        public Transaction(long TID, long accountID, string transactionName, decimal amount, DateTime dateTransactionMade, bool isExpense, string transactionCategory)
        {
            this.TID = TID;
            this.accountID = accountID;
            this.transactionName = transactionName;
            this.amount = amount;
            this.dateTransactionMade = dateTransactionMade;
            this.isExpense = isExpense;
            this.transactionCategory = transactionCategory;
            //Assign the current time using the DateTime library method Now.
            dateTransactionEntered = DateTime.Now;

            newlyCreated = true;
        }

        //Getter Methods
        public long getTID()
        {
            return TID;
        }

        public long getAccountID()
        {
            return accountID;
        }

        public string getTransactionName()
        {
            return transactionName;
        }

        public decimal getAmount()
        {
            return amount;
        }

        public DateTime getDateTransactionMade()
        {
            return dateTransactionMade;
        }

        public bool getIsExpense()
        {
            return isExpense;
        }

        public string getTransactionCategory()
        {
            return transactionCategory;
        }

        public DateTime getDateTransactionEntered()
        {
            return dateTransactionEntered;
        }

        //Setter Methods

        public void setTransactionName(string transactionName)
        {
            this.transactionName = transactionName;
            changed = true;
        }

        public void setAmount(decimal amount)
        {
            this.amount = amount;
            changed = true;
        }

        public void setDateTransactionMade(DateTime dateTransactionMade)
        {
            this.dateTransactionMade = dateTransactionMade;
            changed = true;
        }

        public void setIsExpense(bool isExpense)
        {
            this.isExpense = isExpense;
            changed = true;
        }

        public void setTransactionCategory(string transactionCategory)
        {
            this.transactionCategory = transactionCategory;
            changed = true;
        }
    }
}
