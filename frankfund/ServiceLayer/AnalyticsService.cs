using System;
using DataAccessLayer;
using Google.Cloud.BigQuery.V2;
using DataAccessLayer.Models;
using System.Collections.Generic;

namespace ServiceLayer
{
    public class AnalyticsService
    {
        private readonly AnalyticsDataAccess dataAccess;

        public AnalyticsService()
        {
            this.dataAccess = new AnalyticsDataAccess();
        }

        // Given an account ID, return a dictionary that maps spending category to the sum of
        // expenses in that category for a particular user
        public Dictionary<string,decimal> getSpendingPerCategory(long accID)
        {
            Dictionary<string, decimal> spendingPerCategory = new Dictionary<string, decimal>();
            foreach (BigQueryRow row in dataAccess.getSpendingPerCategory(accID))
            {
                spendingPerCategory.Add(
                    key: Convert.ToString(row["TransactionCategory"]),
                    value: dataAccess.castBQNumeric(row["CategoryTotal"])
                );
            }

            // No acount exists with the given accID
            if(spendingPerCategory.Count == 0)
            {
                return null;
            }
            // Otherwise return the breakdown
            else
            {
                return spendingPerCategory;
            }
        }

        // Overloading wrapper
        public Dictionary<string,decimal> getSpendingPerCategory(UserAccount acc)
        {
            return getSpendingPerCategory(acc.AccountID);
        }
    }
}