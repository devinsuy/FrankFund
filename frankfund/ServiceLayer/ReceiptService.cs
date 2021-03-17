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

        public void delete(long accID)
        {

        }

        // TODO: Use DataAccess Layer to write a NEWLY CREATED object into BigQuery
        public void write(Receipt acc)
        {
           
           /* string query;

            if (!newlyCreated)
            {
                if (changed)
                {
                    query = $"DELETE FROM {this.tableID} WHERE RID={serializedObj[0]}";
                    Console.WriteLine("Receipt with RID " + serializedObj[0] + "was changed, updating records");
                    this.dataHelper.query(query);
                }
                else
                    return;
            }

            query = $"INSERT INTO {this.tableID} VALUES ("
                + serializedObj[0] + "," //receipt ID (RID)
                + serializedObj[1] + "," //transaction ID (TID)
                + $"\"{serializedObj[2]}\"," //imageURL
                + $"\"{serializedObj[3]}\"," //purchaseDate 
                + $"\"receipt[4]\")"; //Notes */
        }

        // TODO: Query DB to reinstantate and return a receipt object with the given receipt id
        public Receipt getUsingID(long ID)
        {
            string notes = ""; //nullable attribute
            Receipt receipt = null;
            foreach (BigQueryRow row in this.ReceiptDataAccess.getUsingID(RID))
            {
                if(row["RID"] != null)
                {
                    RID = (long)row["RID"];
                }
                receipt = new Receipt(
                    (long)row["RID"], long(row)["TID"],
                    (string)row["ImgURL"],
                    (DateTime)row["PurchaseDate"],
                    notes
                    );
            }
            return receipt;
        }

        /* TODO:
           Write a modified object's changed to BigQuery via DataAccess Layer 
               (method should have a way of checking whether the class object changed during runtime
               to avoid redundant writing. Use a changed boolean to implement this)
           Should not call DataAccess update() if did not change */
        public void update(Receipt r)
        {

        }

        /*
        TODO: Convert a Receipt object into JSON format
            Params: A UserAccount object to convert
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
                + $"\"RID\":{serialized[0]}," //receipt ID
                + $"\"TID\":{serialized[1]}," //transaction ID (TID)
                + $"\"imgURL\":{serialized[2]}\"," //imageURL
                + $"\"purchaseDate\":{serialized[3]}\"," //purchaseDate 
                + $"\"Notes\":{serialized[4]}\")" + "}"; //Notes
            return jsonString;
        }

        /*
        TODO: Serialize a Receipt object into a String array
            Returns: A string array with each element in order of its column attribute (see Receipts DB schema)
        */
        public string[] serialize(Receipt r)
        {
            return new string[]
            {
                r.getReceiptID().ToString(),
                r.getTID().ToString(),
                r.getImageURL(),
                r.getPurchaseDate().ToString(),
                r.getNotes()
            };
        }

        public long getNextAvailID()
        {
            return dataAccess.getNextAvailID();
        }
    }

}
