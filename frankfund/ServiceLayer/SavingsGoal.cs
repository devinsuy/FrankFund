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
        public readonly int SGID;
        public string name; 
        public decimal goalAmt;
        public decimal contrAmt;
        public contrPeriod period;
        public DateTime startDate;
        public DateTime endDate;
        public int numPeriods;

        private int calcPeriodsWithDate()
        {
            return (int)Math.Ceiling((endDate - startDate).TotalDays / (int)period);
        }

        private int calcPeriodsWithAmt()
        {
            return (int) Math.Ceiling(goalAmt / contrAmt);
        }

        private decimal calcContrAmt()
        {
            return Math.Round(goalAmt / numPeriods, 2);
        }

        private DateTime calcEndDate()
        {
            int numDays = numPeriods * (int)period;
            DateTime endDate = startDate.AddDays(numDays).Date;
            return endDate;
        }


        // Constructor: create SavingsGoal with amount by endDate
        public SavingsGoal(string name, decimal goalAmt, contrPeriod period, DateTime endDate)
        {
            this.name = name;
            this.goalAmt = goalAmt;
            this.period = period;
            this.startDate = DateTime.Now;

            this.endDate = endDate.Date;
            this.numPeriods = calcPeriodsWithDate();
            this.contrAmt = calcContrAmt();
        }

        // Constructor: create SavingsGoal with amount and specified contribution
        public SavingsGoal(string name, decimal goalAmt, contrPeriod period, decimal contrAmt)
        {
            this.name = name;
            this.goalAmt = goalAmt;
            this.period = period;
            this.startDate = DateTime.Now.Date;

            this.numPeriods = calcPeriodsWithAmt();
            this.endDate = calcEndDate();
            this.contrAmt = contrAmt;
        }

        // Constructor: reinstantiate an pre-existing SavingsGoal from database records

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
                + $"\n   Began on {startDate.ToShortDateString()} and ends on {endDate.ToShortDateString()}"
                + $"\n   Requires a {period} contribution of ${contrAmt} for {numPeriods} {getPeriodNoun()}";
        }
    }
}
