using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.BigQuery.V2;

namespace DataAccessLayer
{
    public class SavingsGoalDataAccess
    {
        private readonly DataHelper dataHelper;
        private readonly string tableID;
        public SavingsGoalDataAccess()
        {
            this.dataHelper = new DataHelper();
            this.tableID = this.dataHelper.getQualifiedTableName("SavingsGoals");
        }

        public BigQueryResults GetSavingsGoalUsingID(string ID)
        {
            string query = $"SELECT * FROM {this.tableID} WHERE SGID = {ID}";        
            return this.dataHelper.query(query, parameters: null);
        }

        /* Write a NEW SavingsGoal to DB
            Params: String array of serialized newly created SavingsGoal object 
        */
        public void writeSavingsGoal(string[] savingsGoal){
            string query = $"INSERT INTO {this.tableID} VALUES ("
                + Convert.ToInt64(savingsGoal[0]) + ","                             // SGID
                + $"\"{savingsGoal[1]}\","                                          // StartDate
                + Convert.ToDecimal(savingsGoal[2]) + ","                           // GoalAmt
                + Convert.ToDecimal(savingsGoal[3]) + ","                           // ContrAmt
                + $"\"{savingsGoal[4]}\","                                          // StartDate
                + $"\"{savingsGoal[5]}\","                                          // StartDate
                + $"\"{savingsGoal[6]}\")";                                         // EndDate
            Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
            this.dataHelper.query(query);
        }

        // Query the DB and get the next available SGID
        public int getNextAvailSGID(){
            string query = $"SELECT MAX(SGID) AS maxSGID FROM {tableID}";
            int maxSGID = 1;
            Console.WriteLine("Running Query:\n--------------\n" + query + "\n");

            foreach (BigQueryRow row in dataHelper.query(query)){                   // Aggregate query will return only a single row
                if(row["maxSGID"] == null){
                    return 1;
                }
                else{
                    maxSGID = (int)row["maxSGID"];
                }
            }
            return maxSGID + 1;
        }
        
    }
}
