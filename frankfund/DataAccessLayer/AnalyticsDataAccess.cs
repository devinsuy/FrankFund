using System;
using Google.Cloud.BigQuery.V2;

namespace DataAccessLayer
{
    public class AnalyticsDataAccess
    {
        private readonly DataHelper dataHelper;

        public AnalyticsDataAccess()
        {
            this.dataHelper = new DataHelper();
        }

        public BigQueryResults getAllTimeSpendingPerCategory(long accID)
        {
            string query = "SELECT a.accountID, t.TransactionCategory, SUM(t.amount) AS CategoryTotal"
                + " FROM FrankFund.Transactions t"
                + " INNER JOIN FrankFund.Accounts a"
                + " ON t.AccountID = a.AccountID"
                + $" WHERE a.accountID = {accID}"
                + " GROUP BY a.AccountID, TransactionCategory"
                + " ORDER BY SUM(t.amount) DESC";

            return this.dataHelper.query(query, parameters: null);
        }
        public BigQueryResults getAllTimeSpendingPerCategory(string username)
        {
            string query = "SELECT a.accountID, t.TransactionCategory, SUM(t.amount) AS CategoryTotal"
                + " FROM FrankFund.Transactions t"
                + " INNER JOIN FrankFund.Accounts a"
                + " ON t.AccountID = a.AccountID"
                + $" WHERE a.AccountUsername = '{username}'"
                + " GROUP BY a.AccountID, TransactionCategory"
                + " ORDER BY SUM(t.amount) DESC";

            return this.dataHelper.query(query, parameters: null);
        }


        public BigQueryResults getSpendingPerTime(string username, int num, string period)
        {
            string query = "SELECT SUM(t.Amount) AS Spending FROM `frankfund.FrankFund.Transactions` t"
                + " INNER JOIN `frankfund.FrankFund.Accounts`"
                + " ON acc.AccountID = t.AccountID"
                + $" WHERE acc.AccountUsername = '{username}' AND t.DateTransactionMade >= DATE_SUB(CURRENT_DATE(), INTERVAL {num} {period})";

            return this.dataHelper.query(query, parameters: null);
        }

        public BigQueryResults getSpendingPerTimeWithCategory(string username, int num, string period, string category)
        {
            string query = "SELECT SUM(t.Amount) AS Spending FROM `frankfund.FrankFund.Transactions` t"
                + " INNER JOIN `frankfund.FrankFund.Accounts`"
                + " ON acc.AccountID = t.AccountID"
                + $" WHERE acc.AccountUsername = '{username}' AND t.TransactionCategory = '{category}'"
                + $" AND t.DateTransactionMade >= DATE_SUB(CURRENT_DATE(), INTERVAL {num} {period})";

            return this.dataHelper.query(query, parameters: null);
        }

        public BigQueryResults getSpendingPerMonthPastYear(string username)
        {
            string query = " SELECT * FROM ("
                + " SELECT SUM(t.Amount) AS Total, EXTRACT(MONTH FROM t.DateTransactionMade) AS Month"
                + " FROM `frankfund.FrankFund.Transactions` t"
                + " INNER JOIN `frankfund.FrankFund.Accounts` acc"
                + " ON acc.AccountID = t.AccountID"
                + $" WHERE acc.AccountUsername = '{username}'"
                + " AND t.IsExpense = true"
                + " AND t.DateTransactionMade >= DATE_SUB(CURRENT_DATE(), INTERVAL 1 YEAR)"
                + " AND t.DateTransactionMade < DATE_TRUNC(CURRENT_DATE(), MONTH)"
                + " GROUP BY EXTRACT(MONTH FROM t.DateTransactionMade)"
            + ") ORDER BY Month ASC";

            return this.dataHelper.query(query, parameters: null);
        }

        public BigQueryResults getSavingsPerMonthPastYear(string username)
        {
            string query = " SELECT * FROM ("
                + " SELECT SUM(t.Amount) AS Total, EXTRACT(MONTH FROM t.DateTransactionMade) AS Month"
                + " FROM `frankfund.FrankFund.Transactions` t"
                + " INNER JOIN `frankfund.FrankFund.Accounts` acc"
                + " ON acc.AccountID = t.AccountID"
                + $" WHERE acc.AccountUsername = '{username}'"
                + " AND t.IsExpense = false"
                + " AND t.DateTransactionMade >= DATE_SUB(CURRENT_DATE(), INTERVAL 1 YEAR)"
                + " AND t.DateTransactionMade < DATE_TRUNC(CURRENT_DATE(), MONTH)"
                + " GROUP BY EXTRACT(MONTH FROM t.DateTransactionMade)"
            + ") ORDER BY Month ASC";

            return this.dataHelper.query(query, parameters: null);
        }

        public BigQueryResults getMostExpensiveTransaction(long accID)
        {
            string query = "SELECT * FROM `frankfund.FrankFund.Transactions`"
                + " WHERE AccountID = 12"
                + " AND Amount = (SELECT MAX(Amount) FROM `frankfund.FrankFund.Transactions` WHERE AccountID = 12)"
                + " LIMIT 1;";

            return this.dataHelper.query(query, parameters: null);
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


        // Returns spending across ALL categories for a given user
        public BigQueryResults getTotalSpending(long accID)
        {
            string query = "SELECT SUM(t.amount) AS TotalExpenses"
                + " FROM FrankFund.Transactions t"
                + " INNER JOIN FrankFund.Accounts a"
                + " ON t.AccountID = a.AccountID"
                + $" WHERE a.accountID = {accID}";

            return this.dataHelper.query(query, parameters: null);
        }
        public BigQueryResults getTotalSpending(string username)
        {
            string query = "SELECT SUM(t.amount) AS TotalExpenses"
                + " FROM FrankFund.Transactions t"
                + " INNER JOIN FrankFund.Accounts a"
                + " ON t.AccountID = a.AccountID"
                + $" WHERE a.AccountUsername = \"{username}\"";

            return this.dataHelper.query(query, parameters: null);
        }

        public BigQueryResults getTotalSavings(string username)
        {
            string query = "SELECT DISTINCT SUM(t.Amount) AS TotalSavings"
                + " FROM `frankfund.FrankFund.SavingsGoals` s"
                + " INNER JOIN `frankfund.FrankFund.Transactions` t"
                + " ON s.SGID = t.SGID"
                + " INNER JOIN `frankfund.FrankFund.Accounts` acc"
                + " ON t.AccountID = acc.AccountID"
                + $" WHERE acc.AccountUsername = \"{username}\"";

            return this.dataHelper.query(query, parameters: null);
        }

        public BigQueryResults getTotalSavingsThisYear(string username)
        {
            string query = "SELECT DISTINCT SUM(t.Amount) AS TotalSavings"
                + " FROM `frankfund.FrankFund.SavingsGoals` s"
                + " INNER JOIN `frankfund.FrankFund.Transactions` t"
                + " ON s.SGID = t.SGID"
                + " INNER JOIN `frankfund.FrankFund.Accounts` acc"
                + " ON t.AccountID = acc.AccountID"
                + $" WHERE acc.AccountUsername = \"{username}\""
                + " AND t.DateTransactionMade >= DATE_TRUNC(CURRENT_DATE(), YEAR);";

            return this.dataHelper.query(query, parameters: null);
        }
    }
}
