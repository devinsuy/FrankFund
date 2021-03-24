using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Models
{
    public enum transactionCategory{
        Entertainment,
        Restaurants,
        Transportation,
        HomeAndUtilities,
        Education,
        Insurance,
        Health,
        Groceries,
        Deposits,
        Shopping,
        Uncategorized
    };
    public class Transaction
    {
        //Transaction ID
        private readonly long TID;
        //Account ID
        private readonly long accountID;
        //Savings Goal ID
        private long SGID;
        //Name of Transaction
        private string transactionName;
        //Amount of Transaction
        private decimal amount;
        //Date transaction made
        private DateTime dateTransactionMade;
        //Is an expense or income
        private bool isExpense;
        //Category of transaction
        private transactionCategory category;
        //Date transaction was entered
        private DateTime dateTransactionEntered;

        public bool changed;
        public bool newlyCreated;

        //Constructor
        public Transaction(long TID, long accountID, long SGID, string transactionName, decimal amount, DateTime dateTransactionMade, bool isExpense, transactionCategory category)
        {
            this.TID = TID;
            this.accountID = accountID;
            this.SGID = SGID;
            this.transactionName = transactionName;
            this.amount = amount;
            this.dateTransactionMade = dateTransactionMade;
            this.isExpense = isExpense;
            this.category = category;

            //Assign the current time using the DateTime library method Now.
            dateTransactionEntered = DateTime.Now;
            newlyCreated = true;
        }

        // Constructor for reinstantiation
        public Transaction(long TID, long accountID, long SGID, string transactionName, decimal amount, DateTime dateTransactionMade, DateTime dateTransactionEntered, bool isExpense, transactionCategory category)
        {
            this.TID = TID;
            this.accountID = accountID;
            this.SGID = SGID;
            this.transactionName = transactionName;
            this.amount = amount;
            this.dateTransactionMade = dateTransactionMade;
            this.isExpense = isExpense;
            this.category = category;

            this.dateTransactionEntered = dateTransactionEntered;
            this.newlyCreated = false;
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

        public long getSGID()
        {
            return SGID;
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

        public transactionCategory getTransactionCategory()
        {
            return category;
        }

        public DateTime getDateTransactionEntered()
        {
            return dateTransactionEntered;
        }

        //Setter Methods

        public void setTransactionName(string transactionName)
        {
            if (transactionName.Equals(this.transactionName))
            {
                return;
            }
            this.transactionName = transactionName;
            changed = true;
        }

        public void setAmount(decimal amount)
        {
            if(amount == this.amount)
            {
                return;
            }
            this.amount = amount;
            changed = true;
        }

        public void setDateTransactionMade(DateTime dateTransactionMade)
        {
            if (dateTransactionMade.Equals(this.dateTransactionMade))
            {
                return;
            }
            this.dateTransactionMade = dateTransactionMade;
            changed = true;
        }

        public void setDateTransactionEntered(DateTime dateTransactionEntered)
        {
            if (dateTransactionEntered.Equals(this.dateTransactionEntered))
            {
                return;
            }
            this.dateTransactionEntered = dateTransactionEntered;
            changed = true;
        }

        public void setIsExpense(bool isExpense)
        {
            if(isExpense == this.isExpense)
            {
                return;
            }
            this.isExpense = isExpense;
            changed = true;
        }

        public void setTransactionCategory(transactionCategory category)
        {
            if (category.Equals(this.category))
            {
                return;
            }
            this.category = category;
            changed = true;
        }

        public void setSGID(long SGID)
        {
            if(SGID == this.SGID)
            {
                return;
            }
            this.SGID = SGID;
            changed = true;
        }

        public string isExpenseToString()
        {
            return (getIsExpense()) ? "Expense" : "Income";
        }

        //Override ToString()
        public override string ToString()
        {
            return "Transaction with TID #" + getTID()
                + $"\n \"{getTransactionName()}\""
                + $"\n Amount: $" + getAmount()
                + $"\n This transaction is an " + isExpenseToString()
                + $"\n Categorized as \"{getTransactionCategory()}\""
                + $"\n Made on {getDateTransactionMade().ToString("yyyy-MM-dd")}"
                + $"\n This transaction was entered into the system on {getDateTransactionEntered().ToString("yyyy-MM-dd")}";
        }
    }
}
