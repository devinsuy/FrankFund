using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.BigQuery.V2;

namespace DataAccessLayer
{
    public class SavingsGoalDataAccess
    {
        private readonly DataHelper dataHelper;
        public SavingsGoalDataAccess()
        {
            this.dataHelper = new DataHelper();
        }

        public BigQueryResults GetSavingsGoalUsingID(string ID)
        {
            DataHelper 
            BigQueryResults retrievedSavingsGoal = null;

            var client = BigQueryClient.Create(projectID);                    
            var table = client.GetTable(datasetID, tableName);               
            var query = $"SELECT * FROM {projectID}.{datasetID}.{tableName} ORDER BY AccountID DESC";           // Table selection is always in the form: projectID.datasetID.tableName
            var queryResults = client.ExecuteQuery(query, parameters: null);
            Console.WriteLine("Executing query: " + query);
            return retrievedUserAccount;
        }
        
    }
}
