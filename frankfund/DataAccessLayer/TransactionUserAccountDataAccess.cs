using System;
using Google.Cloud.BigQuery.V2;

namespace DataAccessLayer
{
    public class TransactionUserAccountDataAccess
    {
        private readonly DataHelper dataHelper;
        private readonly string[] tables;

        public TransactionUserAccountDataAccess()
        {
            this.dataHelper = new DataHelper();
            this.tables = new string[]
            {
                this.dataHelper.getQualifiedTableName("Accounts"),
                this.dataHelper.getQualifiedTableName("Transactions")
            };
        }

        // Returns all transactions with a given user ordered by date entered
        public BigQueryResults getTransactionsFromAccount(long accID)
        {
            string query = "SELECT * FROM FrankFund.Transactions t"
                + $" WHERE t.accountID = {accID}"
                + " ORDER BY DateTransactionEntered";
            return this.dataHelper.query(query, parameters: null);
        }

        // Returns all transactions from a user account given a category ordered by date entered
        public BigQueryResults getTransactionsFromCategory(long accID, string category)
        {
            string query = "SELECT * FROM FrankFund.Transactions t"
                + $" WHERE t.accountID = {accID} AND t.transactionCategory = {category}"
                + " ORDER BY DateTransactionEntered";
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
        public T ParseEnum<T>(string value)
        {
            return this.dataHelper.ParseEnum<T>(value);
        }
    }
}
