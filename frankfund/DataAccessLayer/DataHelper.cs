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
        private readonly Dictionary<string, string> idDict;                     
        public DataHelper()
        {
            this.projectID = "frankfund";                                                                        // GCP project id is always lowercase
            this.datasetID = "FrankFund";
            this.client = BigQueryClient.Create(projectID);                    

            // Map each table name to its PK
            this.idDict = new Dictionary<string, string>(){
                { this.getQualifiedTableName("Accounts"), "AccountID" },
                { this.getQualifiedTableName("Receipts"), "RID" },
                { this.getQualifiedTableName("SavingsGoals"), "SGID" },
                { this.getQualifiedTableName("Sessions"), "SessionID" },
                { this.getQualifiedTableName("Subscriptions"), "SID" },
                { this.getQualifiedTableName("Transactions"), "TID" }
            };
        }

        public string getQualifiedTableName(string tableID){
            return this.projectID + "." + this.datasetID + "." + tableID;
        }

        public BigQueryResults query(string query, IEnumerable<BigQueryParameter> parameters=null)
        {
            return this.client.ExecuteQuery(query, parameters);
        }

        // Query the DB and get the next available ID
        public long getNextAvailID(string tableID){
            if(!this.idDict.ContainsKey(tableID)){
                throw new KeyNotFoundException("Invalid table id, ensure tableID is fully qualified");
            }

            string query = $"SELECT MAX(CAST({this.idDict[tableID]} AS INT64)) AS maxID FROM {tableID}";
            long maxID = 1;
            Console.WriteLine("Running Query:\n--------------\n" + query + "\n");

            foreach (BigQueryRow row in this.query(query)){                   // Aggregate query will return only a single row
                if(row["maxID"] == null){
                    return 1;
                }
                else{
                    maxID = (long)row["maxID"];
                }
            }
            return maxID + 1;
        }
         
        public decimal castBQNumeric(BigQueryNumeric val){
            return val.ToDecimal(LossOfPrecisionHandling.Truncate);
        }
        public decimal castBQNumeric(object val){
            return castBQNumeric((BigQueryNumeric) val);
        }
        public T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase: true);
        }
    }
}