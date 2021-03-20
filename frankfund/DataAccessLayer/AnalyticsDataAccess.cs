using System;
using Google.Cloud.BigQuery.V2;

namespace DataAccessLayer
{
    public class AnalyticsDataAccess
    {
        private readonly DataHelper dataHelper;
        private readonly string[] tables;

        public AnalyticsDataAccess()
        {
            this.dataHelper = new DataHelper();
            this.tables = new string[]
            {
                this.dataHelper.getQualifiedTableName("Accounts"),
                this.dataHelper.getQualifiedTableName("Receipts"),
                this.dataHelper.getQualifiedTableName("SavingsGoals"),
                this.dataHelper.getQualifiedTableName("Subscriptions"),
                this.dataHelper.getQualifiedTableName("Transactions")
            };
        }

        // Return spending per category for a given user
        public BigQueryResults getSpendingPerCategory(long accID)
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
    }
}
