using System;
using System.Collections.Generic;
using Google.Cloud.BigQuery.V2;

namespace DataAccessLayer
{
    public class DataHelper
    {
        private readonly BigQueryClient client;
        private readonly string projectID;
        private readonly string datasetID;
        public DataHelper()
        {
            this.projectID = "frankfund";                                                                        // GCP project id is always lowercase
            this.datasetID = "FrankFund";
            this.client = BigQueryClient.Create(projectID);                    
        }

        public string getQualifiedTableName(string tableID){
            return this.projectID + "." + this.datasetID + "." + tableID;
        }

        public BigQueryResults query(string query, IEnumerable<BigQueryParameter> parameters=null)
        {
            return this.client.ExecuteQuery(query, parameters);
        }
    }
}