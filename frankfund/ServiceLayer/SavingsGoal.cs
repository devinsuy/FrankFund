using System;
namespace ServiceLayer
{
    public enum contrPeriod
    {
        Daily = 1,
        Weekly = 7,
        Monthly = 30
    }

    public class SavingsGoal
    {
        public readonly long SGID;
        public string name; 
        public decimal goalAmt;
        public decimal contrAmt;
        public contrPeriod period;
        public DateTime startDate;
        public DateTime endDate;
        public long numPeriods;

        private long calcPeriodsWithDate()
        {
            return (long)Math.Ceiling((endDate - startDate).TotalDays / (long)period);
        }

        private long calcPeriodsWithAmt()
        {
            return (long) Math.Ceiling(goalAmt / contrAmt);
        }

        private decimal calcContrAmt()
        {
            return Math.Round(goalAmt / numPeriods, 2);
        }

        private DateTime calcEndDate()
        {
            long numDays = numPeriods * (long)period;
            DateTime endDate = startDate.AddDays(numDays).Date;
            return endDate;
        }


        // Constructor: create SavingsGoal by endDate
        public SavingsGoal(long SGID, string name, decimal goalAmt, contrPeriod period, DateTime endDate)
        {
            this.SGID = SGID;
            this.name = name;
            this.goalAmt = goalAmt;
            this.period = period;
            this.startDate = DateTime.Now.Date;

            this.endDate = endDate.Date;
            this.numPeriods = calcPeriodsWithDate();
            this.contrAmt = calcContrAmt();
        }

        // Constructor: create SavingsGoal by specified contribution
        public SavingsGoal(long SGID, string name, decimal goalAmt, contrPeriod period, decimal contrAmt)
        {
            this.SGID = SGID;
            this.name = name;
            this.goalAmt = goalAmt;
            this.period = period;
            this.startDate = DateTime.Now.Date;

            this.numPeriods = calcPeriodsWithAmt();
            this.endDate = calcEndDate();
            this.contrAmt = contrAmt;
        }

        // Constructor: reinstantiate a pre-existing SavingsGoal from database records
        public SavingsGoal(long SGID, string name, decimal goalAmt, decimal contrAmt, contrPeriod period, long numPeriods, DateTime startDate, DateTime endDate)
        {
            this.SGID = SGID;
            this.name = name;
            this.goalAmt = goalAmt;
            this.contrAmt = contrAmt;
            this.period = period;
            this.numPeriods = numPeriods;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public string getPeriodNoun(){
            if(period == contrPeriod.Daily){
                return "days";
            }
            else if(period == contrPeriod.Weekly){
                return "weeks";
            }
            else{
                return "months";
            }
        }

        public override string ToString(){
            return $"\"{name}\" Savings Goal"
                + $"\n   For the amount of ${goalAmt}"
                + $"\n   Began on {startDate.ToString("yyyy-MM-dd")} and ends on {endDate.ToString("yyyy-MM-dd")}"
                + $"\n   Requires a {period} contribution of ${contrAmt} for {numPeriods} {getPeriodNoun()}";
        }
    }
}
