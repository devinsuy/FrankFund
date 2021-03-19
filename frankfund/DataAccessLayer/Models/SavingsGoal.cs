using System;
namespace DataAccessLayer.Models
{   
    // Time period noun = numDays in the period
    public enum contrPeriod
    {
        Daily = 1,
        Weekly = 7,
        BiWeekly = 14,
        Monthly = 30,
        BiMonthly = 60
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
        public bool newlyCreated;
        public bool changed;

        public long calcPeriodsWithDate() {
            return (long)Math.Ceiling((endDate - startDate).TotalDays / (long)period);
        }

        public long calcPeriodsWithAmt() {
            return (long) Math.Ceiling(goalAmt / contrAmt);
        }

        public decimal calcContrAmt() {
            return Math.Round(goalAmt / numPeriods, 2);
        }

        public DateTime calcEndDate() {
            long numDays = numPeriods * (long)period;
            DateTime endDate = startDate.AddDays(numDays).Date;
            return endDate;
        }

        public string formatPeriodStr(){
            if(period == contrPeriod.Daily){
                return "for " + numPeriods + " days";
            }
            else if(period == contrPeriod.Weekly){
                return "for " + numPeriods + " weeks";
            }
            else if(period == contrPeriod.BiWeekly){
                return "every other week for " + numPeriods + " periods";
            }
            else if(period == contrPeriod.Monthly){
                return "for " + numPeriods + " months";
            }
            else{
                return "every other month for " + numPeriods + " periods";
            }
        }

        public override string ToString(){
            return $"\"{name}\" Savings Goal With SGID #" + SGID
                + $"\n   For the amount of ${goalAmt}"
                + $"\n   Began on {startDate.ToString("yyyy-MM-dd")} and ends on {endDate.ToString("yyyy-MM-dd")}"
                + $"\n   Requires a {period} contribution of ${contrAmt} {formatPeriodStr()}";
        }

        // ---------------------------------------- SavingsGoal Constructors -----------------------------------

        // Constructor: create SavingsGoal by endDate
        public SavingsGoal(long SGID, string name, decimal goalAmt, contrPeriod period, DateTime endDate)
        {
            this.SGID = SGID;
            this.name = name;
            this.goalAmt = goalAmt;
            this.period = period;
            this.startDate = DateTime.Now.Date;
            this.newlyCreated = true;

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
            this.newlyCreated = true;

            this.contrAmt = contrAmt;
            this.numPeriods = calcPeriodsWithAmt();
            this.endDate = calcEndDate();
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
            this.newlyCreated = this.changed = false;
        }

        // ---------------------------------------- SavingsGoal Setters ----------------------------------------

        public void updateName(string newName){
            if (newName.Equals(name))
            {
                return;
            }
            this.changed = true;
            this.name = newName;
        }

        /* Updating the goal amount requires either the contribution amount or the end date to be recalculated
            Params: extendEndDate - 
                        True: Keep payments fixed per period, but increase the number of periods and end date.
                        False: Keep number of periods and end date fixed, but increase payment amount per period.
        */
        public void updateGoalAmt(decimal newGoalAmt, bool extendEndDate){
            if(newGoalAmt == this.goalAmt)
            {
                return;
            }
            this.changed = true;
            this.goalAmt = newGoalAmt;       

            // Reflect the updated goal amount in either a new end date and # periods or increase the contrAmt
            if(extendEndDate){
                this.numPeriods = this.calcPeriodsWithAmt();
                this.endDate = this.calcEndDate();
            }
            else{
                this.contrAmt = this.calcContrAmt();
            }
        }

        // Updating contribution amount will require the number of periods and end date to recalculate
        public void updateContrAmt(decimal newContrAmt){
            if(newContrAmt == this.contrAmt)
            {
                return;
            }
            this.changed = true;
            this.contrAmt = newContrAmt;
            this.numPeriods = this.calcPeriodsWithAmt();
            this.endDate = this.calcEndDate();
        }

        // Updating end date will require the number of periods and contribution per period to recalculate
        public void updateEndDate(DateTime newEndDate){
            if(newEndDate.Equals(this.endDate))
            {
                return;
            }
            this.changed = true;
            this.endDate = newEndDate;
            this.numPeriods = this.calcPeriodsWithDate();
            this.contrAmt = this.calcContrAmt();
        }

        // Updating period will require end date to recalculate
        public void updatePeriod(contrPeriod newPeriod){
            if(newPeriod.Equals(this.period))
            {
                return;
            }
            this.changed = true;
            this.period = newPeriod;
            this.endDate = this.calcEndDate();
        }

        // Update contrAmt and change to a new contrPeriod, require both numPeriods and endDate to recalculate
        public void updateContrAmtAndPeriod(decimal newContrAmt, contrPeriod newPeriod){
            if(newContrAmt == this.contrAmt && newPeriod.Equals(this.period))
            {
                return;
            }
            this.changed = true;
            this.contrAmt = newContrAmt;
            this.period = newPeriod;
            this.numPeriods = this.calcPeriodsWithAmt();
            this.endDate = this.calcEndDate();
        }
    }
}
