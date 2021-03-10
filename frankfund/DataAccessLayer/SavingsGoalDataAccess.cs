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
        
        public BigQueryResults GetSavingsGoalUsingID(long ID)
        {
            string query = $"SELECT * FROM {this.tableID} WHERE SGID = {ID}";        
            return this.dataHelper.query(query, parameters: null);
        }

        /* Write a SavingsGoal to DB
            Params: String array of serialized newly created SavingsGoal object 
        */
        public void writeSavingsGoal(string[] savingsGoal, bool newlyCreated, bool changed){
            string query;
            if(!newlyCreated){                                                      // Only write to DB a pre-existing SavingsGoal if it changed during runtime
                if(changed){                                                        // SavingsGoal was updated, delete old record before reinsertion
                    query = $"DELETE FROM {this.tableID} WHERE SGID = {savingsGoal[0]}";
                    Console.WriteLine("SavingsGoal with SGID " + savingsGoal[0] + " was changed, updating records");
                    this.dataHelper.query(query);
                }
                else                                                                // SavingsGoal has not changed, nothing to write
                    return;
            }
            query = $"INSERT INTO {this.tableID} VALUES ("
                + savingsGoal[0] + ","                                              // SGID
                + $"\"{savingsGoal[1]}\","                                          // Name
                + savingsGoal[2] + ","                                              // GoalAmt
                + savingsGoal[3] + ","                                              // ContrAmt
                + $"\"{savingsGoal[4]}\","                                          // Period
                + savingsGoal[5] + ","                                              // NumPeriods
                + $"\"{savingsGoal[6]}\","                                          // StartDate
                + $"\"{savingsGoal[7]}\")";                                         // EndDate
            Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
            this.dataHelper.query(query);
        }

        // Query the DB and get the next available SGID
        public long getNextAvailSGID(){
            return this.dataHelper.getNextAvailID(this.tableID);
        }

        // Overload wrappers to cast BigQuery Numeric type to C# decimal type
        public decimal castBQNumeric(BigQueryNumeric val){
            return this.dataHelper.castBQNumeric(val);
        }
        public decimal castBQNumeric(object val){
            return this.dataHelper.castBQNumeric(val);
        }
        
    }
}
