using System;
using System.IO;
using ServiceLayer;
using DataAccessLayer.Models;
using System.Collections.Generic;


namespace ServiceLayer
{
    class debug
    {
        static void authenticateGCP()
        {
            // Devin's Credentials
            //string pathToCreds = "/Users/devin/Documents/GitHub/FrankFund/Credentials/AuthDevin.json";

            // Autumn's Credentials
            string pathToCreds = "/Users/steve/OneDrive/Documents/GitHub/FrankFund/Credentials/AuthAutumn.json";

            // Kenneth's Credentials
            //string pathToCreds = "/Users/015909177/Desktop/Github Repos/FrankFund/Credentials/AuthKenny.json";

            //Rachel's Credentials 
            //string pathToCreds = "C:/Data/Spring 2021/CECS 491B/Senior Project/Credentials/AuthRachel.json";

            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToCreds);
        }

        // --------------------------------------------------- Begin Savings Goal Testing ---------------------------------------------------
        static SavingsGoal testSGCreateByDate()
        {
            Console.WriteLine("\n-------------------- Test: Creating A New Savings Goal By End Date, Write --------------------");

            // Query DB for the next avail ID
            SavingsGoalService sgService = new SavingsGoalService();
            long SGID = sgService.getNextAvailID();
            Console.WriteLine("Next available SGID that can be assigned: " + SGID + "\n");

            // Create a new Savings Goal for the amount of $150 ending on December 20th, dynamically calculate payments
            DateTime endDate = new DateTime(2021, 12, 20, 0, 0, 0).Date;
            SavingsGoal sampleGoal = new SavingsGoal(SGID, "Christmas Gift", (decimal)150.00, contrPeriod.Monthly, endDate);

            // Print summary of goal that was just created
            Console.WriteLine("Savings Goal Summary:\n---------------------");
            Console.WriteLine(sampleGoal);

            // Write SavingsGoal object to DB
            sgService.write(sampleGoal);

            return sampleGoal;
        }

        static SavingsGoal testSGCreateByContrAmt()
        {
            Console.WriteLine("\n\n-------------------- Test: Creating A New Savings Goal By Contribution Amount, Write --------------------");

            // Query DB for the next avail ID
            SavingsGoalService sgService = new SavingsGoalService();
            long SGID = sgService.getNextAvailID();
            Console.WriteLine("Next available SGID that can be assigned: " + SGID + "\n");

            // Create a new Savings Goal for the amount of $300 dynamically calculate end date
            DateTime endDate = new DateTime(2021, 12, 20, 0, 0, 0).Date;
            SavingsGoal sampleGoal = new SavingsGoal(SGID, "Tuition", (decimal)3425.00, contrPeriod.Weekly, (decimal)325.00);

            // Print summary of goal that was just created
            Console.WriteLine("Savings Goal Summary:\n---------------------");
            Console.WriteLine(sampleGoal);

            // Write SavingsGoal object to DB
            sgService.write(sampleGoal);

            return sampleGoal;
        }

        static void testSGShowSerialize(SavingsGoal sg)
        {
            Console.WriteLine("\n\n-------------------- Test: Display String Serialization of SavingsGoal Object --------------------");

            // Serialize the runtime object to String[]
            SavingsGoalService sgService = new SavingsGoalService();
            string[] serialized = sgService.serialize(sg);
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
            Console.WriteLine(sgService.getJSON(sg));
        }

        static void testSGReadModifyRewrite(long SGID)
        {
            Console.WriteLine("\n\n-------------------- Test: Recreate, Modify, Rewrite Existing SavingsGoal--------------------");

            // Reinstantiate a SavingsGoal from DB records
            SavingsGoalService sgService = new SavingsGoalService();
            SavingsGoal existingGoal = sgService.getUsingID(SGID);

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
            sgService.write(existingGoal);
        }


        // --------------------------------------------------- Begin User Account Testing ---------------------------------------------------

        static void testAccCreate()
        {
            UserAccountService uaService = new UserAccountService();
            PasswordService passService = new PasswordService();

            Console.WriteLine("\n-------------------- Test: Getting and Creating a new User Account --------------------");
            //Console.WriteLine("\n1. Testing GetAccountUsingUsername function | Prints out AccountUsername found");
            //String existingAccount = uaService.getUsingUsername("AutumnNguyen").AccountUsername;
            //Console.WriteLine("Username: " + existingAccount);

            //Console.WriteLine("\n2. Testing GetAccountUsingID function | Prints out AccountUsername found");
            //String existingAccount2 = uaService.getUsingID(2).AccountUsername;
            //Console.WriteLine("Username: " + existingAccount2);

            Console.WriteLine("\n3. Testing CreateUserAccount function | Prints out AccountUsername of made account");
            UserAccount testAccount = new UserAccount(5, "test", "test@gmail.com", "Password1", null);
            uaService.write(testAccount);
            Console.WriteLine(passService.checkHash("Password1", testAccount));

            //Console.WriteLine("Username: " + uaService.getUsingID(5).AccountUsername);

            //Console.WriteLine("\n4. Testing DeleteUserAccount function | Prints out AccountUsername of deleted account");
            //Console.WriteLine("Disable Username: " + uaService.getUsingID(5).AccountUsername);
            //uaService.delete(5);

            //Console.WriteLine("\n5. Testing CreateUserAccount function - New | Prints out AccountUsername of made account");
            //UserAccount testAccount2 = new UserAccount("test2", "test2@gmail.com", "password", null);
            //uaService.write(testAccount2);
            //Console.WriteLine("Username: " + uaService.getUsingUsername("test2").AccountUsername);

            //Console.WriteLine("\n6. Testing UpdateUserAccount function | Prints out new AccountUsername of account");
            //Console.WriteLine("Old Username: " + uaService.getUsingUsername("test2").AccountUsername);
            //UserAccount testAccount3 = new UserAccount(5, "testing", "test2@gmail.com", "password", null);
            //uaService.update(testAccount3);
            //Console.WriteLine("New Username: " + uaService.getUsingUsername("testing").AccountUsername);

            //Console.WriteLine("\n7. Testing DeleteUserAccount function | Prints out AccountUsername of deleted account");
            //Console.WriteLine("Disable Username: " + uaService.getUsingUsername("testing").AccountUsername);
            //uaService.delete(5);
        }


        // --------------------------------------------------- Begin Transaction Testing ---------------------------------------------------

        static Transaction testAddTransaction()
        {
            Console.WriteLine("\n-------------------- Test: Creating A New Transaction, Write -------------------------");
            // Query DB for the next avail ID
            TransactionService ts = new TransactionService();
            long TID = ts.getNextAvailID();
            Console.WriteLine("Next available TID that can be assigned: " + TID + "\n");

            // Create a new Transaction
            //Temp Account ID
            long tempAccountID = 1;
            long SGID = 2;
            Transaction sampleTransaction = new Transaction(TID, tempAccountID, SGID, "Netflix", (decimal)9.99, new DateTime(2021, 12, 14, 0, 0, 0).Date, true, transactionCategory.Entertainment);

            // Print summary of goal that was just created
            Console.WriteLine("Transaction Summary:\n---------------------");
            Console.WriteLine(sampleTransaction);

            // Write SavingsGoal object to DB
            ts.write(sampleTransaction);

            return sampleTransaction;
        }

        static Transaction testReinstantiateTransaction(long TID)
        {
            TransactionService ts = new TransactionService();
            return ts.getUsingID(TID);
        }

        static void testModifyRewriteTransaction(long TID)
        {
            TransactionService ts = new TransactionService();
            Transaction t = ts.getUsingID(TID);
            t.setAmount(t.getAmount() * 2);
            ts.update(t);
        }

        static void testDeleteTransaction(long TID)
        {
            TransactionService ts = new TransactionService();
            ts.delete(TID);
        }


        // --------------------------------------------------- Begin Receipt Testing ---------------------------------------------------

        static Receipt testAddReceipt()
        {
            Console.WriteLine("\n-------------------- Test: Creating A New Receipt, Write -------------------------");
            // Query DB for the next avail ID
            ReceiptService rs = new ReceiptService();
            long RID = rs.getNextAvailID();
            Console.WriteLine("Next available RID that can be assigned: " + RID + "\n");

            //Create a new Receipt 
            long tempTransactionID = 1;
            string tempStringURL = "hellothisisatest.png";

            Receipt sampleReceipt = new Receipt(RID, tempTransactionID, tempStringURL, new DateTime(2021, 03, 16, 0, 0, 0), "short note", newlyCreated: true);

            //Print summary of Receipt that was just created
            Console.WriteLine("Receipt Summary: \n-----------------------");
            Console.WriteLine(sampleReceipt);
            rs.write(sampleReceipt);

            return sampleReceipt;
        }

        static void testSerializeReceipt(Receipt r)
        {
            ReceiptService rs = new ReceiptService();
            Console.WriteLine();
            foreach(string s in rs.serialize(r)) {
                Console.WriteLine(s);
            }
        }

        static void testReceiptJSON(Receipt r)
        {
            ReceiptService rs = new ReceiptService();
            Console.WriteLine("\n" + rs.getJSON(r));       
        }

        static Receipt testReinstantiateReceipt(long RID)
        {
            ReceiptService rs = new ReceiptService();
            Receipt r = rs.getUsingID(10);
            return r;
        }

        static void testModifyRewriteReceipt(long RID) {
            ReceiptService rs = new ReceiptService();
            Receipt r = rs.getUsingID(10);
            Console.WriteLine(r);
            r.setNote("short note 3");
            Console.WriteLine("\nAfter change: \n" + r);
            rs.update(r);
        }

        static void testDeleteReceipt(long RID)
        {
            ReceiptService rs = new ReceiptService();
            rs.delete(RID);
        }



        // --------------------------------------------------- Begin Data Analytics Testing ---------------------------------------------------
        static void testSpendingPerCategory(long accID)
        {
            AnalyticsService a = new AnalyticsService();
            
            Dictionary<string, decimal> spendingPerCategory = a.getSpendingPerCategory(accID);

            // Print the spending per category
            Console.WriteLine("For user #" + accID);
            Console.WriteLine("\nSpending Per Category\n---------------------\n");
            foreach(string category in spendingPerCategory.Keys)
            {
                Console.WriteLine("   " + category + " : $" + spendingPerCategory[category]);
            }
        }

        static void testGetTotalSpending(long accID)
        {
            AnalyticsService a = new AnalyticsService();
            decimal totalSpending = a.getTotalSpending(accID);

            // Print the total spending
            Console.WriteLine("For user #" + accID);
            Console.WriteLine("   Total spending: $" + totalSpending);
        }

        static void testCategoryBreakdown(long accID)
        {
            AnalyticsService a = new AnalyticsService();
            Dictionary<string, Tuple<decimal, decimal>> breakdown = a.getCategoryBreakdown(accID);

            Console.WriteLine("For user #" + accID);
            Console.WriteLine("\nCategory Breakdown\n---------------\n");
            foreach(string category in breakdown.Keys)
            {
                Tuple<decimal, decimal> categoryVals = breakdown[category];
                Console.WriteLine("   " + category + ":");
                Console.WriteLine("      Category Total: $" + categoryVals.Item1);
                Console.WriteLine("      % of All Spending: " + categoryVals.Item2);
            }
        }





        // --------------------------------------------------- End Testing ---------------------------------------------------



        static void Main(string[] args){
            // ---------------------------------------------- GCP Auth -------------------------------------------------------------------------
            authenticateGCP();


            //----------------------------------------------- Test: SG Create By End Date, Write------------------------------------------------
            //SavingsGoal sampleOne = testSGCreateByDate();


            // ---------------------------------------------- Test: SG Create By Contribution Amount, Write ------------------------------------
            //SavingsGoal sampleTwo = testSGCreateByContrAmt();


            // ---------------------------------------------- Test: Display String Serialization -----------------------------------------------
            //testSGShowSerialize(sampleOne);


            // ---------------------------------------------- Test: Display JSON Representation ------------------------------------------------
            //testSGShowJSON(sampleTwo);


            // ---------------------------------------------- Sample Test of SG Read, Modify, Rewrite ------------------------------------------
            //testSGReadModifyRewrite(SGID: 1);


            // ---------------------------------------------- Test: Creating User Account ------------------------------------------------------
            //testAccCreate();


            // ---------------------------------------------- Test: Transaction Create ---------------------------------------------------------
            //Transaction sampleTrans1 = testAddTransaction();
            //Transaction t = testReinstantiateTransaction(TID: 41);
            //TransactionService ts = new TransactionService();
            //Console.WriteLine(ts.getJSON(t));
            //testModifyRewriteTransaction(TID: 25);
            //testDeleteTransaction(TID: 42);


            // ---------------------------------------------- Test: Receipt Create ---------------------------------------------------------
            //Receipt r = testAddReceipt();
            //testSerializeReceipt(r);
            //testReceiptJSON(r);
            //Console.WriteLine(testReinstantiateReceipt(RID: 10));
            //testModifyRewriteReceipt(RID: 10);
            //testDeleteReceipt(RID: 11);



            // ---------------------------------------------- Test: Data Analytics ---------------------------------------------------------
            //testSpendingPerCategory(accID: 1);
            //testGetTotalSpending(accID: 1);
            //testCategoryBreakdown(accID: 1);
        }

    }
}