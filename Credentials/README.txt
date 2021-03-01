C# CLI QuickStart Guide:
------------------------
1. Create local development project using:
    "dotnet new console -n <project_name>"

2. cd to your project directory 

3. Before you'll be able to use GCP on your local machine add u need to add the relevant packages: 
    "dotnet add package Google.Cloud.BigQuery.V2"
    "dotnet add package Google.Cloud.Storage.V1"

4. Point the GOOGLE_APPLICATION_CREDENTIALS env variable to the Auth.json file assigned to you using
    System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToCreds);

5. You are now authenticated to make GCP API calls
    (See Github Credentials/GCPDemo/GCPDemo.cs for example authentication and usage of GCP API)

6. Run using:
    dotnet run
