using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    //Reference for how to use enums: https://www.c-sharpcorner.com/article/top-7-c-sharp-enum-enumeration-code-examples/ 
    public enum SubscriptionFrequency
    {
        Weekly,
        Monthly,
        everyThreeMonths,
        everySixMonths,
        Yearly,
        NotSpecified
    };
    public class Subscription
    {
        //ReceiptID 
        private readonly long RID;
        //Subscription ID
        private readonly long SID;
        //Account ID
        private readonly long accID;
        //Purchase Date 
        private DateTime purchaseDate;
        //Optional Note 
        private string notes;
        //Transaction amount 
        private decimal purchaseAmount;
        //Renew Frequency
        private SubscriptionFrequency frequency;

        public bool changed;
        public bool newlyCreated; 

        //FULL Constructor 
        public Subscription(long accID, long SID, long RID, DateTime purchaseDate, string notes, decimal purchaseAmount, SubscriptionFrequency frequency)
        {
            this.accID = accID;
            this.SID = SID;
            this.RID = RID;
            this.purchaseDate = purchaseDate;
            this.notes = notes;
            this.purchaseAmount = purchaseAmount;
            this.frequency = frequency;

        }

        //Constructor without nullable aspects 
        public Subscription(long accID, long SID, long rID, DateTime purchaseDate, decimal purchaseAmount, SubscriptionFrequency frequency)
        {
            this.accID = accID;
            this.SID = SID;
            this.purchaseDate = purchaseDate;
            this.purchaseAmount = purchaseAmount;
            this.frequency = frequency;
        }


        //Getter methods
        public long getAccID()
        {
            return accID;
        }

        public long getRID()
        {
            return RID;
        }

        public long getSID()
        {
            return SID; 
        }

        public DateTime getPurchaseDate()
        {
            return purchaseDate;
        }

        public decimal getPurchaseAmount()
        {
            return purchaseAmount;
        }

        public string getNotes()
        {
            if (notes == null)
            {
                notes = "This subscription does not have any notes attached.";
            }
            return notes;
        }

        public SubscriptionFrequency getFrequency()
        {
            return frequency;
        }

        //Setters 
        public void setSubscriptionFrequency(SubscriptionFrequency frequency)
        {
            if (frequency.Equals(this.frequency))
            {
                return;
            }
            this.frequency = frequency;
            changed = true;
        }

        public void setRID(long RID)
        {
            if (RID == this.RID)
            {
                return;
            }

            changed = true;
        }
        public void setPurchaseDate(DateTime purchaseDate)
        {
            if (purchaseDate.Equals(this.purchaseDate))
            {
                return;
            }

            this.purchaseDate = purchaseDate;
            changed = true;
        }

        public void setNotes(string notes)
        {
            if (notes.Equals(notes))
            {
                return;
            }

            this.notes = notes;
            changed = true;
        }

        public void setAmount(decimal purchaseAmount)
        {
            if(purchaseAmount == this.purchaseAmount)
            {
                return;
            }
            this.purchaseAmount = purchaseAmount;
            changed = true;
        }

        public void setRenewFrequency(SubscriptionFrequency frequency)
        {
            if(frequency.Equals(this.frequency))
            {
                return;
            }

            this.frequency = frequency;
            changed = true;
        }

        //Override the toString()
        public override string ToString()
        {
            return "Subscription with SID #:" + getSID()
                + $"\n Account ID: " + getAccID()
                + $"\n Receipt ID: " + getRID()
                + $"\n Frequency continues every \"{getFrequency()}\""
                + $"\n Most recent purchase made on {getPurchaseDate().ToString("yyyy-MM-dd")}"
                + $"\n Amount: $" + getPurchaseAmount()
                + $"\n These are the notes associated: " + getNotes();
        }


    }
}
