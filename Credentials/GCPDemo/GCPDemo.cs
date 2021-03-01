using System;
using Google.Cloud.BigQuery.V2;
using Google.Cloud.Storage.V1;
using System.IO;

namespace GCPDemo
{
    class GCPDemo
    {
        static void Main(string[] args)
        {
            // -------------------------------------------- Setup GCP Credentials -------------------------------------------- 

            // Replace with path to wherever Auth<name>.json file is on your local machine
            string pathToCreds = "/Users/devin/Documents/GitHub/FrankFund/Credentials/AuthDevin.json";  

            // Once set you can make calls to the GCP API
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToCreds);



            // -------------------------------------------- BigQuery Query Example -------------------------------------------- 
            // Relevant Docs: https://cloud.google.com/bigquery/docs/samples/bigquery-query#bigquery_query-csharp
            
            var projectID = "frankfund";                                                                        // GCP project id is always lowercase
            var datasetID = "FrankFund";
            var tableName = "Accounts";

            var client = BigQueryClient.Create(projectID);                    
            var table = client.GetTable(datasetID, tableName);               
            var query = $"SELECT * FROM {projectID}.{datasetID}.{tableName} ORDER BY AccountID DESC";           // Table selection is always in the form: projectID.datasetID.tableName
            var queryResults = client.ExecuteQuery(query, parameters: null);
            Console.WriteLine("Executing query: " + query);

            /*
            Console Output:
                Executing query: SELECT * FROM frankfund.FrankFund.Accounts ORDER BY AccountID DESC

                Query Results:
                4, KennethTran, k.tran@gmail.com
                3, RachelPai, r.pai@gmail.com
                2, DevinSuy, d.suy@gmail.com
                1, AutumnNguyen, a.nguyen@gmail.com
            */
            Console.WriteLine("\nQuery Results:");
            foreach (BigQueryRow row in queryResults)
            {
                Console.WriteLine($"   {row["AccountID"]}, {row["AccountUsername"]}, {row["EmailAddress"]}");   // Iterate over result set and access current row with row["columnName"]
            }



            // -------------------------------------------- BigQuery Insert Data From .json Stored In Cloud Storage Example -------------------------------------------- 
            // Relevant Docs: https://cloud.google.com/bigquery/docs/samples/bigquery-load-table-gcs-json

            projectID = "frankfund";                                                                        // GCP project id is always lowercase
            datasetID = "FrankFund";
            tableName = "Accounts";

            var schema = new TableSchemaBuilder{
                { "AccountID",          BigQueryDbType.String },
                { "AccountUsername",    BigQueryDbType.String },
                { "EmailAddress",       BigQueryDbType.String },
                { "Password",           BigQueryDbType.String },
                { "FacebookID",         BigQueryDbType.Numeric },
                { "GoogleID",           BigQueryDbType.Numeric }
            }.Build();

            var jobOptions = new CreateLoadJobOptions{
                SourceFormat = FileFormat.NewlineDelimitedJson                                              // NOTE: GCP only accepts single line per object for .json format 
            };

            client = BigQueryClient.Create(projectID);
            var dataset = client.GetDataset(datasetID);
            var tableRef = dataset.GetTableReference(tableName);
            var cloudStorageURI = "gs://frankfund_sandbox/exampleAccountInsert.json";                       // Cloud Storage URI format: gs://<bucket_name>/<file_path_inside_bucket>

            var loadJob = client.CreateLoadJob(cloudStorageURI, tableRef, schema, jobOptions);
            try{
                loadJob.PollUntilCompleted();
                Console.WriteLine("Json file loaded to BigQuery");
            } 
            catch(Google.GoogleApiException e){
                Console.WriteLine(e.Message);
            }


            // -------------------------------------------- Cloud Storage File Retrieval Example -------------------------------------------- 
            // Relevant Docs:
                // https://googleapis.github.io/google-cloud-dotnet/docs/Google.Cloud.Storage.V1/index.html
                // https://docs.microsoft.com/en-us/dotnet/api/system.io.filestream?view=net-5.0
            // Useful reference for future: https://medium.com/net-core/using-google-cloud-storage-in-asp-net-core-74f9c5ee55f5
            
            projectID = "frankfund";
            var bucketName = "frankfund_sandbox";   
            var fileName = "team_members.txt";                                                                     
            var storageClient = StorageClient.Create();

            // Download file from cloud storage bucket to local
            using(var stream = File.OpenWrite(fileName)){
                storageClient.DownloadObject(bucketName, fileName, stream);
            }            

            /* Console Output From Downloaded File:
                Team Frank: Autumn Nguyen, Devin Suy, Kenneth Tran, Rachel Pai
            */
            foreach(string line in File.ReadAllLines(fileName)){
                Console.WriteLine(line);
            }
        }
    }
}
