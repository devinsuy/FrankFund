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
        // Returns null if account not found
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


        // Given an account ID, return the sum of all transactions across ALL categories
        // Returns -1 if account not found
        public decimal getTotalSpending(long accID)
        {
            decimal totalSpending = -1;
            foreach (BigQueryRow row in dataAccess.getTotalSpending(accID))
            {
                totalSpending = dataAccess.castBQNumeric(row["TotalExpenses"]);
            }
            return totalSpending;
        }

        // Overloading wrapper
        public decimal getTotalSpending(UserAccount acc)
        {
            return getTotalSpending(acc.AccountID);
        }


        // Given an account ID, return a breakdown of the spending per category
        // Returns a dictionary that maps the category to a tuple of spending in the category, and percentage of total spending
        // Returns null if account does not exist
        public Dictionary<string, Tuple<decimal, decimal>> getCategoryBreakdown(long accID)
        {
            decimal totalSpending = getTotalSpending(accID);
            Dictionary<string, Tuple<decimal, decimal>> categoryBreakdown = new Dictionary<string, Tuple<decimal, decimal>>();
            Dictionary<string, decimal> spendingPerCategory = getSpendingPerCategory(accID);
            if(spendingPerCategory == null || totalSpending == -1)
            {
                return null;
            }

            decimal currCategoryTotal, currPercentage;
            foreach (string category in spendingPerCategory.Keys)
            {
                currCategoryTotal = spendingPerCategory[category];
                currPercentage = Math.Round(currCategoryTotal / totalSpending, 2);
                categoryBreakdown.Add(category, new Tuple<decimal, decimal>(currCategoryTotal, currPercentage));
            }

            if(categoryBreakdown.Count == 0)
            {
                return null;
            }
            else
            {
                return categoryBreakdown;
            }
        }

        // Overloading wrapper
        public Dictionary<string, Tuple<decimal, decimal>> getCategoryBreakdown(UserAccount acc)
        {
            return getCategoryBreakdown(acc.AccountID);
        }
    }
}