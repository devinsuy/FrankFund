using System;
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
        
        // TODO: Parse DB query into SavingsGoal runtime
        public BigQueryResults GetSavingsGoalUsingID(int ID)
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
                + $"\"{savingsGoal[4]}\","                                          // Period
                + Convert.ToInt64(savingsGoal[5]) + ","                             // NumPeriods
                + $"\"{savingsGoal[6]}\","                                          // StartDate
                + $"\"{savingsGoal[7]}\")";                                         // EndDate
            Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
            this.dataHelper.query(query);
        }

        // Query the DB and get the next available SGID
        public long getNextAvailSGID(){
            return this.dataHelper.getNextAvailID(this.tableID);
        }
        
    }
}
