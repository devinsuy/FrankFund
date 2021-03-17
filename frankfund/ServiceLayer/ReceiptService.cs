using System;
using DataAccessLayer.Models;
using DataAccessLayer;
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

        }

        // TODO: Query DB to reinstantate and return a receipt object with the given receipt id
        public Receipt getUsingID(long ID)
        {
            return null;
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
            return null;
        }

        /*
        TODO: Serialize a Receipt object into a String array
            Returns: A string array with each element in order of its column attribute (see Receipts DB schema)
        */
        public string[] serialize(Receipt r)
        {
            return null;
        }

        public long getNextAvailID()
        {
            return dataAccess.getNextAvailID();
        }
    }

}
