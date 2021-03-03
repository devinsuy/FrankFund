using System;
using Google.Cloud.BigQuery.V2;
using Google.Cloud.Storage.V1;
using System.IO;
using ServiceLayer;

namespace ServiceLayer
{
    class debug
    {
        static void Main(string[] args)
        {
            // Replace with path to wherever Auth<name>.json file is on your local machine
            string pathToCreds = "Credentials/AuthDevin.json";  
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToCreds);

            Console.WriteLine("Hello world");
        }
    }
}