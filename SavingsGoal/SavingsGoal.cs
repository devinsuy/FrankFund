using System;
namespace FrankFund
{
    public enum contrPeriod
    {
        Daily = 1,
        Weekly = 7,
        Monthly = 30
    }

    public class SavingsGoal
    {
        public int SGID { get; init; }
        public string name { get; set; }
        public decimal goalAmt { get; set; }
        public decimal contrAmt { get; set; }
        public contrPeriod period { get; set; }
        public DateTime startDate { get; init; }
        public DateTime endDate { get; set; }
        public int numPeriods { get; set; }

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
            DateTime endDate = startDate.AddDays(numDays);
            return endDate;
        }


        // Constructor: create SavingsGoal with amount by endDate
        public SavingsGoal(string name, decimal goalAmt, contrPeriod period, DateTime endDate)
        {
            this.name = name;
            this.goalAmt = goalAmt;
            this.period = period;
            this.startDate = DateTime.Now;

            this.endDate = endDate;
            this.numPeriods = calcPeriodsWithDate();
            this.contrAmt = calcContrAmt();
        }

        // Constructor: create SavingsGoal with amount and specified contribution
        public SavingsGoal(string name, decimal goalAmt, contrPeriod period, decimal contrAmt)
        {
            this.name = name;
            this.goalAmt = goalAmt;
            this.period = period;
            this.startDate = DateTime.Now;

            this.numPeriods = calcPeriodsWithAmt();
            this.endDate = calcEndDate();
            this.contrAmt = contrAmt;
        }
    }
}
