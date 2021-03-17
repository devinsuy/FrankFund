using System;
using DataAccessLayer.Models;
using DataAccessLayer;
using Google.Cloud.BigQuery.V2;

namespace ServiceLayer
{
    public class ReceiptService : Service<Receipt>
    {
        private readonly ReceiptDataAccess dataAccess;

        public ReceiptService()
        {
            this.dataAccess = new ReceiptDataAccess();
        }

        // Delete an existing Receipt
        public void delete(long accID)
        {
            dataAccess.delete(accID);
        }

        // Write a NEWLY created receipt into BigQuery
        public void write(Receipt r)
        {
            string[] serializedReceipt = serialize(r);
            dataAccess.write(serializedReceipt);
        }

        // Retrieve and return Receipt object from BigQuery with the given PK identifer
        public Receipt getUsingID(long RID)
        {
            string notes = ""; //nullable attribute
            Receipt receipt = null;
            foreach (BigQueryRow row in this.dataAccess.getUsingID(RID))
            {
                if(row["Notes"] != null)
                {
                    notes = (string)row["Notes"];
                }
                receipt = new Receipt(
                    (long)row["RID"], (long)row["TID"],
                    (string)row["ImgURL"],
                    (DateTime)row["PurchaseDate"],
                    notes,
                    newlyCreated: false
                );
            }
            return receipt;
        }

        // Serialize and update an EXISTING Rceipt in BigQuery only if it CHANGED during runtime
        public void update(Receipt r)
        {
            if (r.changed)
            {
                string[] serializedReceipt = serialize(r);
                this.dataAccess.update(serializedReceipt);
            }
        }

        /*
        Convert a Receipt object into JSON format
            Params: A Receipt object to convert
            Returns: The JSON string representation of the object
        */
        public string getJSON(Receipt r)
        {
            if (r == null)
            {
                return "{}";
            }
            
            string[] serialized = serialize(r);
            string jsonString = "{"
                + $"\"RID\":{serialized[0]},"                       // receipt ID
                + $"\"TID\":{serialized[1]},"                       // transaction ID (TID)
                + $"\"ImgURL\":\"" + serialized[2] + "\","          // ImgURL
                + $"\"PurchaseDate\":\"" + serialized[3] + "\","    // Purchase Date
                + $"\"Notes\":\"" + serialized[4] + "\"}";          // Notes
            return jsonString;
        }

        /*
        Serialize a Receipt object into a String array
            Returns: A string array with each element in order of its column attribute (see Receipts DB schema)
        */
        public string[] serialize(Receipt r)
        {
            return new string[]
            {
                r.getReceiptID().ToString(),
                r.getTID().ToString(),
                r.getImageURL(),
                r.getPurchaseDate().ToString("yyyy-MM-dd"),
                r.getNotes()
            };
        }

        // Get the next available Receipt ID
        public long getNextAvailID()
        {
            return dataAccess.getNextAvailID();
        }
    }

}
