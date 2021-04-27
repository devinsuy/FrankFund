using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.BigQuery.V2;
using DataAccessLayer.Models;

namespace DataAccessLayer
{
    public class SubscriptionDataAccess: DataAccess
    {
        private readonly DataHelper dataHelper;
        private readonly string tableID;

        private readonly string accTable;
        private readonly string transactionTable;
        private readonly string receiptTable; 

        public SubscriptionDataAccess()
        {
            this.dataHelper = new DataHelper();
            this.accTable = this.dataHelper.getQualifiedTableName("Accounts");
            this.transactionTable = this.dataHelper.getQualifiedTableName("Transactions");
            this.receiptTable = this.dataHelper.getQualifiedTableName("Receipts");
            this.tableID = this.dataHelper.getQualifiedTableName("Subscriptions");
        }

        public BigQueryResults getUsingID(long ID)
        {
            string query = $"SELECT * from {this.tableID} WHERE SID = {ID}";
            return this.dataHelper.query(query, parameters: null);
        }

        // Write a Subscription into BigQuery
        public void write(string[] serializedSubscription)
        {
            string query = $"INSERT INTO {this.tableID} VALUES ("
                + serializedSubscription[0] + ","                        // Subscription ID
                + serializedSubscription[1] + ","                        // Account ID
                + serializedSubscription[2] + ","                        // Receipt ID
                + $"\"{serializedSubscription[3]}\","                    // Purchase Date
                + $"\"{serializedSubscription[4]}\"" + ","                        // Notes
                + serializedSubscription[5] + ","                    // Amount
                + $"\"{serializedSubscription[6]}\")";                    // Renew Frequency
           
            //Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
            this.dataHelper.query(query);
        }

        // Delete an existing Subscription with the given PK identifier
        public void delete(long SID)
        {
            string query = $"DELETE FROM {this.tableID} WHERE SID = {SID}";
            this.dataHelper.query(query);
        }

        // Write an existing Subscription that changed during runtime
        public void update(string[] serializedSubscription)
        {
            // Delete the existing records for this Subscription
            delete(long.Parse(serializedSubscription[0]));

            // Write the updated records
            write(serializedSubscription);
        }

        // Returns all Subscriptions with a given user ordered by date entered
        public BigQueryResults getSubscriptionsFromAccount(long accID)
        {
            string query = "SELECT * FROM FrankFund.Subscriptions s"
                + $" WHERE s.accountID = {accID}"
                + " ORDER BY PurchaseDate DESC";
            return this.dataHelper.query(query, parameters: null);
        }

        // Returns all Subscriptions with a given user ordered by date entered
        public BigQueryResults getSubscriptionsFromAccount(string username)
        {
            string query = "SELECT s.SID, s.AccountID, s.RID, s.PurchaseDate, s.Notes, s.Amount, s.RenewFrequency"
                + $" FROM {this.tableID} s"
                + $" INNER JOIN {this.accTable} a"
                + " ON s.AccountID = a.AccountID"
                + $" WHERE a.AccountUsername = '{username}'"
                + " ORDER BY PurchaseDate DESC";
            Console.WriteLine(query);
            return this.dataHelper.query(query, parameters: null);
        }

        // Returns all Subscriptions from a user account given a category ordered by date entered
        public BigQueryResults getSubscriptionViaFrequency(long accID, string frequency)
        {
            string query = "SELECT * FROM FrankFund.Subscriptions s"
                + $" WHERE s.accountID = {accID} AND s.RenewFrequency = \"{frequency}\""
                + " ORDER BY PurchaseDate DESC";
            return this.dataHelper.query(query, parameters: null);
        }

        // Returns all Subscriptions from a user account made in the past n days
        public BigQueryResults SortSubscriptionByDays(long accID, int num)
        {
            string query = "SELECT * FROM FrankFund.Subscriptions s"
                + $" WHERE s.accountID = {accID} AND PurchaseDate >= DATE_SUB(CURRENT_DATE(), INTERVAL {num} DAY)"
                + " ORDER BY PUrchaseDate DESC";
            return this.dataHelper.query(query, parameters: null);
        }

        // Returns all Subscriptions from a user account made in the past n weeks
        public BigQueryResults SortSubscriptionsByWeeks(long accID, int num)
        {
            string query = "SELECT * FROM FrankFund.Subscriptions s"
                + $" WHERE s.accountID = {accID} AND PurchaseDate >= DATE_SUB(CURRENT_DATE(), INTERVAL {num} WEEK)"
                + " ORDER BY PurchaseDate DESC";
            return this.dataHelper.query(query, parameters: null);
        }

        // Returns all Subscriptions from a user account made in the past n weeks
        public BigQueryResults SortSubscriptionsByMonths(long accID, int num)
        {
            string query = "SELECT * FROM FrankFund.Subscriptions s"
                + $" WHERE s.accountID = {accID} AND PurchaseDate >= DATE_SUB(CURRENT_DATE(), INTERVAL {num} MONTH)"
                + " ORDER BY PurchaseDate DESC";
            return this.dataHelper.query(query, parameters: null);
        }

        // Returns all Subscrptions from a user account made in the past n years
        public BigQueryResults SortSubscriptionsByYear(long accID, int num)
        {
            string query = "SELECT * FROM FrankFund.Subscriptions s"
                + $" WHERE s.accountID = {accID} AND PurchaseDate >= DATE_SUB(CURRENT_DATE(), INTERVAL {num} YEAR)"
                + " ORDER BY PurchaseDate DESC";
            return this.dataHelper.query(query, parameters: null);
        }

        public BigQueryResults getUserSubscriptionCount(string username)
        {
            string query = "SELECT COUNT(*) AS NumSubscriptions"
                + " FROM `frankfund.FrankFund.Subscriptions` s"
                + " INNER JOIN `frankfund.FrankFund.Accounts` acc"
                + " ON s.AccountID = acc.AccountID"
                + $" WHERE acc.AccountUsername = \"{username}\"";

            return this.dataHelper.query(query, parameters: null);
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
        public T ParseEnum<T>(string value)
        {
            return this.dataHelper.ParseEnum<T>(value);
        }
    }

}