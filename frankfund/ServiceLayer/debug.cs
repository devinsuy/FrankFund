using System;
using Google.Cloud.BigQuery.V2;
using Google.Cloud.Storage.V1;
using System.IO;
using ServiceLayer;
using DataAccessLayer.Models;

namespace ServiceLayer
{
    class debug
    {
        // Test reinstantiation of SavingsGoal from db, modification methods, writing changes back to db
        static void Main(string[] args){
            // Devin's Credentials
            //string pathToCreds = "/Users/devin/Documents/GitHub/FrankFund/Credentials/AuthDevin.json";
            //Autumn's Credentials
            string pathToCreds = "/Users/steve/OneDrive/Documents/GitHub/FrankFund/Credentials/AuthAutumn.json";
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToCreds);

            SavingsGoalService sgService = new SavingsGoalService();
            SavingsGoal existingGoal = sgService.GetSavingsGoalUsingID(1);      
            Console.WriteLine(existingGoal);       
            existingGoal.updatePeriod(contrPeriod.BiWeekly);
            Console.WriteLine("\n" + existingGoal);
            sgService.writeSavingsGoal(existingGoal);

            // ---------------------------------------------- Sample Test of Creating User Account ----------------------------------------------
            UserAccountService uaService = new UserAccountService();

            Console.WriteLine("\n-------------------- Sample Tests of Getting and Creating a new User Account --------------------");
            Console.WriteLine("\n1. Testing GetAccountUsingUsername function | Prints out AccountUsername found");
            String existingAccount = uaService.GetAccountUsingUsername("AutumnNguyen").AccountUsername;
            Console.WriteLine("Username: " + existingAccount);

            Console.WriteLine("\n2. Testing GetAccountUsingID function | Prints out AccountUsername found");
            String existingAccount2 = uaService.GetAccountUsingID(2).AccountUsername;
            Console.WriteLine("Username: " + existingAccount2);

            Console.WriteLine("\n3. Testing CreateUserAccount function | Prints out AccountUsername of made account");
            UserAccount testAccount = new UserAccount (5, "test", "test@gmail.com", "password", null);
            uaService.CreateUserAccount(testAccount);
            Console.WriteLine("Username: " + uaService.GetAccountUsingID(5).AccountUsername);
        }


        // static void Main(string[] args)
        // {
        //     // ---------------------------------------------- GCP AUTH ----------------------------------------------

        //     // Replace with path to wherever Auth<name>.json file is on your local machine
        //     string pathToCreds = "/Users/devin/Documents/GitHub/FrankFund/Credentials/AuthDevin.json";  
        //     System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToCreds);


        //     // ---------------------------------------------- Sample Test of Savings Goal Layers ----------------------------------------------

        //     // SavingsGoalService layer functionality 
        //     SavingsGoalService sgService = new SavingsGoalService();
        //     long SGID = sgService.getNextAvailSGID();
        //     DateTime endDate = new DateTime(2021, 12, 25, 0, 0, 0).Date;
        //     SavingsGoal sampleGoal = new SavingsGoal(SGID, "Christmas Gift", (decimal)250.25, contrPeriod.Monthly, endDate);

        //     // Print goal summary
        //     Console.WriteLine("Savings Goal Summary:\n---------------------\n");
        //     Console.WriteLine(sampleGoal);

        //     // Serialize the runtime object to String[] and JSON string
        //     string[] serialized = sgService.serializeSavingsGoal(sampleGoal);
        //     Console.WriteLine("\nSavingsGoal Serialized:\n-----------------------\n");
        //     foreach(string attr in serialized){
        //         Console.WriteLine("   " + attr);
        //     }
        //     sgService.getJSON(sampleGoal);


        //     // Write SavingsGoal object to DB
        //     sgService.writeSavingsGoal(sampleGoal);
        // }
        // ---------------------------------------------- End Savings Goal Sample Test  ----------------------------------------------



        //     // ---------------------------------------------- Sample Test of Accounts Layers ----------------------------------------------
        //     Console.WriteLine("\nQuerying Account IDs\n--------------------");
        //     UserAccountService accService = new UserAccountService();
        //     long nextID = accService.getNextAvailID();
        //     Console.WriteLine("Next available Account ID: " + nextID);

        //     // ---------------------------------------------- End Accounts Sample Test  ----------------------------------------------



        //         /*
        //         Console Output:

        //             Running Query:
        //             --------------
        //             SELECT MAX(CAST(SGID AS INT64)) AS maxID FROM frankfund.FrankFund.SavingsGoals

        //             Savings Goal Summary:
        //             ---------------------

        //             "Christmas Gift" Savings Goal
        //             For the amount of $250.25
        //             Began on 2021-03-03 and ends on 2021-12-25
        //             Requires a Monthly contribution of $25.02 for 10 months

        //             SavingsGoal Serialized:
        //             -----------------------

        //             3
        //             Christmas Gift
        //             250.25
        //             25.02
        //             Monthly
        //             2021-03-03
        //             2021-12-25

        //             SavingsGoal JSON Representation:
        //             --------------------------------
        //             {"SGID":3,"Name":"Christmas Gift","GoalAmt":250.25,"ContrAmt":25.02,"Period":"Monthly","StartDate":"2021-03-03","EndDate":"2021-12-25"}

        //             Running Insert Query:
        //             ---------------------
        //             INSERT INTO frankfund.FrankFund.SavingsGoals VALUES (3,"Christmas Gift",250.25,25.02,"Monthly","2021-03-03","2021-12-25")


        //             Querying Account IDs
        //             --------------------
        //             Running Query:
        //             --------------
        //             SELECT MAX(CAST(AccountID AS INT64)) AS maxID FROM frankfund.FrankFund.Accounts

        //             Next available Account ID: 5
        //         */
        // }
    }
}