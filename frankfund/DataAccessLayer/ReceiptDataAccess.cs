using System;
using Google.Cloud.BigQuery.V2;

namespace DataAccessLayer
{
    public class ReceiptDataAccess: DataAccess
    {
        private readonly DataHelper dataHelper;
        private readonly string tableID;
        public ReceiptDataAccess()
        {
            this.dataHelper = new DataHelper();
            this.tableID = this.dataHelper.getQualifiedTableName("Receipts");
        }

        public BigQueryResults getUsingID(long ID)
        {
            string query = $"SELECT * from {this.tableID} WHERE RID = {ID}";
            return this.dataHelper.query(query, parameters: null);
        }

        // TODO: Use DataHelper.query() to WRITE a newly created string serialized object into BigQuery
        public void write(string[] serializedObj)
        {
            _ = $"INSERT INTO {this.tableID} VALUES ("
                + serializedObj[0] + "," //receipt ID (RID)
                + serializedObj[1] + "," //transaction ID (TID)
                + $"\"{serializedObj[2]}\"," //imageURL
                + $"\"{serializedObj[3]}\"," //purchaseDate 
                + $"\"{serializedObj[4]}\")"; //Notes 

        }
        /* public void write(string[] serializedObj, bool newlyCreated, bool changed)
        {
            string query;

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
                + $"\"receipt[4]\")"; //Notes 

        }
        */

        // TODO: Use DataHelper.query() to DELETE an object from BigQuery given its PK identifier
        public void delete(long accID)
        {

        }

        /* TODO: Use DataHelper.query() to REWRITE an existing object that changed at runtime
           This method should call delete(long ID) followed by write(string[] serializedObj) */
        public void update(string[] serializedAcc)
        {

        }

        public long getNextAvailID()
        {
            return this.dataHelper.getNextAvailID(this.tableID);
        }

        // Overload wrappers to cast BigQuery Numeric type to C# decimal type
        public decimal castBQNumeric(BigQueryNumeric val)
        {
            return this.dataHelper.castBQNumeric(val);
        }
        public decimal castBQNumeric(object val)
        {
            return this.dataHelper.castBQNumeric(val);
        }

    }
}
