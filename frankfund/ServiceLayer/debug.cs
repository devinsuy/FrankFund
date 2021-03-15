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
        static void authenticateGCP()
        {
            // Devin's Credentials
            string pathToCreds = "/Users/devin/Documents/GitHub/FrankFund/Credentials/AuthDevin.json";

            // Autumn's Credentials
            //string pathToCreds = "/Users/steve/OneDrive/Documents/GitHub/FrankFund/Credentials/AuthAutumn.json";

            // Kenneth's Credentials
            //string pathToCreds = "/Users/015909177/Desktop/Github Repos/FrankFund/Credentials/AuthKenny.json";

            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToCreds);
        }

        static SavingsGoal testSGCreateByDate()
        {
            Console.WriteLine("\n-------------------- Test: Creating A New Savings Goal By End Date, Write --------------------");

            // Query DB for the next avail ID
            SavingsGoalService sgService = new SavingsGoalService();
            long SGID = sgService.getNextAvailSGID();
            Console.WriteLine("Next available SGID that can be assigned: " + SGID + "\n");

            // Create a new Savings Goal for the amount of $150 ending on December 20th, dynamically calculate payments
            DateTime endDate = new DateTime(2021, 12, 20, 0, 0, 0).Date;
            SavingsGoal sampleGoal = new SavingsGoal(SGID, "Christmas Gift", (decimal)150.00, contrPeriod.Monthly, endDate);

            // Print summary of goal that was just created
            Console.WriteLine("Savings Goal Summary:\n---------------------");
            Console.WriteLine(sampleGoal);

            // Write SavingsGoal object to DB
            sgService.writeSavingsGoal(sampleGoal);

            return sampleGoal;
        }

        static SavingsGoal testSGCreateByContrAmt()
        {
            Console.WriteLine("\n\n-------------------- Test: Creating A New Savings Goal By Contribution Amount, Write --------------------");

            // Query DB for the next avail ID
            SavingsGoalService sgService = new SavingsGoalService();
            long SGID = sgService.getNextAvailSGID();
            Console.WriteLine("Next available SGID that can be assigned: " + SGID + "\n");

            // Create a new Savings Goal for the amount of $300 dynamically calculate end date
            DateTime endDate = new DateTime(2021, 12, 20, 0, 0, 0).Date;
            SavingsGoal sampleGoal = new SavingsGoal(SGID, "Tuition", (decimal)3425.00, contrPeriod.Weekly, (decimal)325.00);

            // Print summary of goal that was just created
            Console.WriteLine("Savings Goal Summary:\n---------------------");
            Console.WriteLine(sampleGoal);

            // Write SavingsGoal object to DB
            sgService.writeSavingsGoal(sampleGoal);

            return sampleGoal;
        }

        static void testSGShowSerialize(SavingsGoal sg)
        {
            Console.WriteLine("\n\n-------------------- Test: Display String Serialization of SavingsGoal Object --------------------");

            // Serialize the runtime object to String[]
            SavingsGoalService sgService = new SavingsGoalService();
            string[] serialized = sgService.serializeSavingsGoal(sg);
            Console.WriteLine("\nSavingsGoal Serialized:\n-----------------------\n");

            // Print the serialized SavingsGoal
            foreach (string attr in serialized)
            {
                Console.WriteLine("   " + attr);
            }
        }

        static void testSGShowJSON(SavingsGoal sg)
        {
            Console.WriteLine("\n\n-------------------- Test: Display JSON of SavingsGoal Object --------------------");

            // Print the JSON representation of the goal we created
            SavingsGoalService sgService = new SavingsGoalService();
            sgService.getJSON(sg);
        }

        static void testSGReadModifyRewrite(long SGID)
        {
            Console.WriteLine("\n\n-------------------- Test: Recreate, Modify, Rewrite Existing SavingsGoal--------------------");

            // Reinstantiate a SavingsGoal from DB records
            SavingsGoalService sgService = new SavingsGoalService();
            SavingsGoal existingGoal = sgService.GetSavingsGoalUsingID(SGID);

            // Print summary of goal that was just reinstantiated
            Console.WriteLine("Savings Goal Summary:\n---------------------");
            Console.WriteLine(existingGoal + "\n");

            // Modify the payment period, system recalculates end date, redisplay summary
            existingGoal.updatePeriod(contrPeriod.Weekly);
            Console.WriteLine("Savings Goal Summary:\n---------------------");
            Console.WriteLine(existingGoal + "\n");

            // Modify the payment amound to half, system recalculates number of payment periods needed and end date, redisplay summary
            existingGoal.updateContrAmt(existingGoal.contrAmt * 2);
            Console.WriteLine("Savings Goal Summary:\n---------------------");
            Console.WriteLine(existingGoal + "\n");

            // Rewrite the modified goal to DB
            sgService.writeSavingsGoal(existingGoal);
        }


        static void testAccCreate()
        {
            UserAccountService uaService = new UserAccountService();

            Console.WriteLine("\n-------------------- Test: Getting and Creating a new User Account --------------------");
            Console.WriteLine("\n1. Testing GetAccountUsingUsername function | Prints out AccountUsername found");
            String existingAccount = uaService.GetAccountUsingUsername("AutumnNguyen").AccountUsername;
            Console.WriteLine("Username: " + existingAccount);

            Console.WriteLine("\n2. Testing GetAccountUsingID function | Prints out AccountUsername found");
            String existingAccount2 = uaService.GetAccountUsingID(2).AccountUsername;
            Console.WriteLine("Username: " + existingAccount2);

            Console.WriteLine("\n3. Testing CreateUserAccount function | Prints out AccountUsername of made account");
            UserAccount testAccount = new UserAccount(5, "test", "test@gmail.com", "password", null);
            uaService.CreateUserAccount(testAccount);
            Console.WriteLine("Username: " + uaService.GetAccountUsingID(5).AccountUsername);

            //Console.WriteLine("\n4. Testing CreateUserAccount function - New | Prints out AccountUsername of made account");
            //UserAccount testAccount2 = new UserAccount(6, "test2", "test2@gmail.com", "password", null);
            //uaService.CreateUserAccount(testAccount2);
            //Console.WriteLine("Username: " + uaService.GetAccountUsingID(6).AccountUsername);

            Console.WriteLine("\n4. Testing DeleteUserAccount function | Prints out AccountUsername of deleted account");
            //UserAccount testAccount = new UserAccount(5, "test", "test@gmail.com", "password", null);
            uaService.DeleteUserAccount(testAccount, true);
            //Console.WriteLine("Delete Username: " + uaService.GetAccountUsingID(5).AccountUsername);
        }
        
        static Transaction testAddTransaction()
        {
            Console.WriteLine("\n-------------------- Test: Creating A New Transaction, Write -------------------------");
            // Query DB for the next avail ID
            TransactionService ts = new TransactionService();
            long TID = ts.getNextAvailTID();
            Console.WriteLine("Next available TID that can be assigned: " + TID + "\n");

            // Create a new Transaction
            //Temp Account ID
            long tempAccountID = 1;
            long SGID = 2;
            Transaction sampleTransaction = new Transaction(TID, tempAccountID, SGID, "Netflix", (decimal)9.99, new DateTime(2021, 12, 14, 0, 0, 0).Date, true, "Entertainment");

            // Print summary of goal that was just created
            Console.WriteLine("Transaction Summary:\n---------------------");
            Console.WriteLine(sampleTransaction);

            // Write SavingsGoal object to DB
            ts.AddTransaction(sampleTransaction);

            return sampleTransaction;
        }



        static void Main(string[] args){
            // ---------------------------------------------- GCP Auth -------------------------------------------------------------------------
            authenticateGCP();


            //----------------------------------------------- Test: SG Create By End Date, Write------------------------------------------------
            SavingsGoal sampleOne = testSGCreateByDate();


            // ---------------------------------------------- Test: SG Create By Contribution Amount, Write ------------------------------------
            SavingsGoal sampleTwo = testSGCreateByContrAmt();


            // ---------------------------------------------- Test: Display String Serialization -----------------------------------------------
            testSGShowSerialize(sampleOne);


            // ---------------------------------------------- Test: Display JSON Representation ------------------------------------------------
            testSGShowJSON(sampleTwo);


            // ---------------------------------------------- Sample Test of SG Read, Modify, Rewrite ------------------------------------------
            //testSGReadModifyRewrite(SGID: 1);


            // ---------------------------------------------- Test: Creating User Account ------------------------------------------------------
            testAccCreate();


            // ---------------------------------------------- Test: Transaction Create ---------------------------------------------------------
            Transaction sampleTrans1 = testAddTransaction();
        }

    }
}