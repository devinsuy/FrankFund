using System;
using Google.Cloud.BigQuery.V2;

namespace DataAccessLayer
{
    public class SavingsGoalDataAccess: DataAccess
    {
        private readonly DataHelper dataHelper;
        private readonly string tableID;
        
        public SavingsGoalDataAccess()
        {
            this.dataHelper = new DataHelper();
            this.tableID = this.dataHelper.getQualifiedTableName("SavingsGoals");
        }
        
        public BigQueryResults getUsingID(long ID)
        {
            string query = $"SELECT * FROM {this.tableID} WHERE SGID = {ID}";        
            return this.dataHelper.query(query, parameters: null);
        }


        // Write a savings goal to BigQuery
        public void write(string[] serializedGoal)
        {
            string query = $"INSERT INTO {this.tableID} VALUES ("
                + serializedGoal[0] + ","                                              // SGID
                + $"\"{serializedGoal[1]}\","                                          // Name
                + serializedGoal[2] + ","                                              // GoalAmt
                + serializedGoal[3] + ","                                              // ContrAmt
                + $"\"{serializedGoal[4]}\","                                          // Period
                + serializedGoal[5] + ","                                              // NumPeriods
                + $"\"{serializedGoal[6]}\","                                          // StartDate
                + $"\"{serializedGoal[7]}\")";                                         // EndDate
            Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
            this.dataHelper.query(query);

        }

        // Delete an existing record of a savings goal with the given PK identifier
        public void delete(long SGID)
        {
            string query = $"DELETE FROM {this.tableID} WHERE SGID = {SGID}";
            this.dataHelper.query(query);
        }

        // Write a Savings Goals changes to BigQuery
        public void update(string[] serializedGoal)
        {
            // Delete the existing record of the savings goal
            delete(long.Parse(serializedGoal[0]));

            // Insert the new version of the savings goal
            write(serializedGoal);
        }

        // Query the DB and get the next available SGID
        public long getNextAvailID(){
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
