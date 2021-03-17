using System;
using DataAccessLayer;
using Google.Cloud.BigQuery.V2;
using DataAccessLayer.Models;

namespace ServiceLayer
{
    public class SavingsGoalService: Service<SavingsGoal>
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
        public SavingsGoal getUsingID(long SGID)
        {
            SavingsGoal sGoal = null;
            foreach(BigQueryRow row in this.SavingsGoalDataAccess.getUsingID(SGID)){
                sGoal = new SavingsGoal (
                    (long)row["SGID"], (string)row["Name"], 
                    this.SavingsGoalDataAccess.castBQNumeric(row["GoalAmt"]), 
                    this.SavingsGoalDataAccess.castBQNumeric(row["ContrAmt"]), 
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
        public string[] serialize(SavingsGoal s){
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
            if(s == null)
            {
                return "{}";
            }
            string[] serialized = serialize(s);
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
            //Console.WriteLine($"\nSavingsGoal JSON Representation:\n--------------------------------\n{jsonStr}\n");
            return jsonStr;
        }

        /*
        Serialize a savings goal object and write it to the DB
            Params: s - Savings Goal runtime object
        */
        // TODO: Use DataHelper.query() to WRITE a newly created string serialized object into BigQuery

        public void write(SavingsGoal s){
            this.SavingsGoalDataAccess.writeSavingsGoal(this.serialize(s), s.newlyCreated, s.changed);
        }


        // TODO: Use DataHelper.query() to DELETE an object from BigQuery given its PK identifier
        public void delete(long SGID)
        {

        }

        /* TODO: Use DataHelper.query() to REWRITE an existing object that changed at runtime
           This method should call delete(long ID) followed by write(string[] serializedObj) */
        public void update(SavingsGoal s)
        {

        }


        /* Wrapper method, query DB for next available SGID
            Returns: Next available SGID (1 + the maximum SGID currently in the DB)
        */
        public long getNextAvailID(){
            return SavingsGoalDataAccess.getNextAvailID();
        }

    }
}
