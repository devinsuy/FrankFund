using System;
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

        /* Cast a period string to contrPeriod enum
            Params: The period string to cast
            Returns: The corresponding enum contrPeriod
        */
        public contrPeriod castPeriod(string p){
            if(p.Equals("Daily"))
                return contrPeriod.Daily;
            else if(p.Equals("Weekly"))
                return contrPeriod.Weekly;
            else if(p.Equals("BiWeekly"))
                return contrPeriod.BiWeekly;
            else if(p.Equals("Monthly"))
                return contrPeriod.Monthly;
            else
                return contrPeriod.BiMonthly;
        }

        /* Retrieve a SavingsGoal from db with a given SGID
            Params: The SGID of the Savings Goal to retrieve
            Returns: A reinstantiated Savings Goal matching the SGID or null if non existant
        */
        public SavingsGoal GetSavingsGoalUsingID(int SGID)
        {
            SavingsGoal sGoal = null;
            foreach(BigQueryRow row in this.SavingsGoalDataAccess.GetSavingsGoalUsingID(SGID)){
                sGoal = new SavingsGoal (
                    (long)row["SGID"], (string)row["Name"], 
                    ((BigQueryNumeric)row["GoalAmt"]).ToDecimal(LossOfPrecisionHandling.Truncate), 
                    ((BigQueryNumeric)row["ContrAmt"]).ToDecimal(LossOfPrecisionHandling.Truncate), 
                    this.castPeriod((string)row["Period"]), (long)row["NumPeriods"], 
                    (DateTime)row["StartDate"], (DateTime)row["EndDate"]
                );
            }
            return sGoal;
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
                s.numPeriods.ToString(),
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
                + $"\"NumPeriods\":{serialized[5]},"
                + $"\"StartDate\":\"" + serialized[6] + "\","
                + $"\"EndDate\":\"" + serialized[7] + "\""
            + "}";
            Console.WriteLine($"\nSavingsGoal JSON Representation:\n--------------------------------\n{jsonStr}\n");
            return jsonStr;
        }
        
        /*
        Serialize a savings goal object and write it to the DB
            Params: s - Savings Goal runtime object
        */
        public void writeSavingsGoal(SavingsGoal s){
            this.SavingsGoalDataAccess.writeSavingsGoal(this.serializeSavingsGoal(s), s.newlyCreated, s.changed);
        }

        /* Wrapper method, query DB for next available SGID
            Returns: Next available SGID (1 + the maximum SGID currently in the DB)
        */
        public long getNextAvailSGID(){
            return SavingsGoalDataAccess.getNextAvailSGID();
        }

    }
}
