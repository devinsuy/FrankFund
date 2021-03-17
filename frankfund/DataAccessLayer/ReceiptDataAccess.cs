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
            string query = "";
            return this.dataHelper.query(query, parameters: null);
        }

        // TODO: Use DataHelper.query() to WRITE a newly created string serialized object into BigQuery
        public void write(string[] serializedAcc)
        {

        }


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
    }
}
