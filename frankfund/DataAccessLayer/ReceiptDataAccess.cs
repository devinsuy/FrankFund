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


        }


        // TODO: Use DataHelper.query() to DELETE an object from BigQuery given its PK identifier
        //write a tester function! 
        public void delete(long receiptID)
        {
            string query = $"DELETE from {this.tableID} WHERE RID = {receiptID}";
        }

        /* TODO: Use DataHelper.query() to REWRITE an existing object that changed at runtime
           This method should call delete(long ID) followed by write(string[] serializedObj) */
        public void update(string[] serializedReceipt)
        {
            delete(serializedReceipt[0]); //delete the receipt based on the receiptID
            write(serializedReceipt);
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
