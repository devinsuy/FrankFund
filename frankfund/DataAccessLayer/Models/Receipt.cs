using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DataAccessLayer.Models.Transaction;

namespace DataAccessLayer.Models
{
    public class Receipt
    {
        private readonly long TID; //Transaction ID
        private readonly long RID; //Receipt ID
        private string ImgURL; //URL for image so that it can be stored in data buckets on GCP
        private DateTime PurchaseDate; //Date the transaction was made
        private string Notes; //Notes for additional details the user may want to have

        public bool changed;
        public bool newlyCreated;

        //Constructor 
        public Receipt(long RID, long TID, string ImgURL, DateTime PurchaseDate, string Notes, bool newlyCreated)
        {
            this.RID = RID;
            this.TID = TID;
            this.ImgURL = ImgURL;
            this.PurchaseDate = PurchaseDate;
            this.Notes = Notes;
            this.newlyCreated = newlyCreated;
        }

        //Getter methods
        public long getTID()
        {
            return TID;
        }

        public long getReceiptID()
        {
            return RID;
        }

        public string getImageURL()
        {
            return ImgURL;
        }

        public DateTime getPurchaseDate()
        {
            return PurchaseDate;
        }

        public string getNotes()
        {
            return Notes;
        }

        //Setter Methods

        //Set the purchase date to whatever is entered. Format must be YYDDMM with quotations
        //https://docs.microsoft.com/en-us/dotnet/api/system.datetime?view=net-5.0#parsing-01 
        public void setPurchaseDate(DateTime shoppingDate)
        {
            this.PurchaseDate = shoppingDate;
            changed = true;
        }

        public void setNote(string noteToWrite)
        {
            this.Notes = noteToWrite;
            changed = true;
        }

        public void setImageLink(string imageToURL)
        {
            this.ImgURL = imageToURL;
            changed = true;
            //This still needs to be figured out in the controller?? 
        }

        //Override ToString()
        public override string ToString()
        {
            return "Receipt with RID #" + getReceiptID()
                + $"\n Transaction ID: \"{getTID()}\""
                + $"\n Image URL: \"{getImageURL()}\""
                + $"\n Purchase made on: \" {getPurchaseDate().ToString("yyyy-MM-dd")}\""
                + $"\n Notes: \" {getNotes()}\"";
        }
    }
}

