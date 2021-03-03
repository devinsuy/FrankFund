using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLayer;
using Google.Cloud.BigQuery.V2;

namespace ServiceLayer
{
    public class SavingsGoalService
    {
        private readonly SavingsGoalDataAccess SavingsGoalDataAccess;
        public SavingsGoalService()
        {
            this.SavingsGoalDataAccess = new SavingsGoalDataAccess();
        }

        // TODO: Parse BigQueryResults into SavingsGoal runtime object
        public string GetAccountUsingID(string ID)
        {
            var retrievedSavingsGoal = this.SavingsGoalDataAccess.GetSavingsGoalUsingID(ID);
            return retrievedSavingsGoal.GetEnumerator().ToString();
        }

        /*
        Serialize a SavingsGoal object into a String array
            Returns: A string array with each element in order of its column attribute (see SavingsGoal DB schema)
        */
        public string[] serializeSavingsGoal(SavingsGoal s){
            return new string[] {
                s.SGID.ToString(),
                s.name,
                s.goalAmt.ToString(),
                s.contrAmt.ToString(),
                s.period.ToString(),
                s.startDate.ToString("yyyy-MM-dd"),
                s.endDate.ToString("yyyy-MM-dd")
            }; 
        }

        /*
        Convert a SavingsGoal object into JSON format
            Params: A SavingsGoal object to convert
            Returns: The JSON string representation of the object
        */
        public string getJSON(SavingsGoal s){
            string[] serialized = serializeSavingsGoal(s);
            string jsonStr = "{"
                + $"\"SGID\":{serialized[0]},"
                + $"\"Name\":\"" + serialized[1] + "\","
                + $"\"GoalAmt\":{serialized[2]},"
                + $"\"ContrAmt\":{serialized[3]},"
                + $"\"Period\":\"" + serialized[4] + "\","
                + $"\"StartDate\":\"" + serialized[5] + "\","
                + $"\"EndDate\":\"" + serialized[6] + "\""
            + "}";
            Console.WriteLine($"\nSavingsGoal JSON Representation:\n--------------------------------\n{jsonStr}\n");
            return jsonStr;
        }
        
        /*
        Serialize a savings goal and write it to the DB
            Params: s - Savings Goal runtime object
        */
        public void writeSavingsGoal(SavingsGoal s){
            string[] serializedSavingsGoal = this.serializeSavingsGoal(s);
            this.SavingsGoalDataAccess.writeSavingsGoal(serializedSavingsGoal);
        }

        /* Wrapper method, query DB for next available SGID
            Returns: Next available SGID (1 + the maximum SGID currently in the DB)
        */
        public long getNextAvailSGID(){
            return SavingsGoalDataAccess.getNextAvailSGID();
        }
    }
}
