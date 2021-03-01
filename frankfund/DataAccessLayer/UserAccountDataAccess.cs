using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.BigQuery.V2;

namespace DataAccessLayer
{
    public class UserAccountDataAccess
    {
        private readonly BigQueryClient client;
        public UserAccountDataAccess(BigQueryClient client)
        {
            this.client = client;
        }

        public BigQueryResults GetUserAccountUsingID(string ID)
        {
            BigQueryResults retrievedUserAccount = null;
            string query = @"SELECT
                CONCAT(
                    'https://stackoverflow.com/questions/',
                    CAST(id as STRING)) as url, view_count
                FROM `bigquery-public-data.stackoverflow.posts_questions`
                WHERE tags like '%google-bigquery%'
                ORDER BY view_count DESC
                LIMIT 10";
            retrievedUserAccount = client.ExecuteQuery(query, parameters: null);
            return retrievedUserAccount;
        }
    }
}
