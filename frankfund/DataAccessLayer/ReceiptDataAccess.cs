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

        // Write a serialized receipt to BigQuery
        public void write(string[] serializedReceipt)
        {
            string query = $"INSERT INTO {this.tableID} VALUES ("
                + serializedReceipt[0] + ","                                              // RID
                + serializedReceipt[1] + ","                                              // TID
                + $"\"{serializedReceipt[2]}\","                                          // ImgURL
                + $"\"{serializedReceipt[3]}\","                                          // PurchaseDate
                + $"\"{serializedReceipt[4]}\")";                                         // Notes
            Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
            this.dataHelper.query(query);
        }


        // Delete a receipt with the given PK identifier
        public void delete(long receiptID)
        {
            string query = $"DELETE from {this.tableID} WHERE RID = {receiptID}";
            this.dataHelper.query(query);
        }


        // Write Receipts changes to BigQuery
        public void update(string[] serializedReceipt)
        {
            // Delete the existing record of the Receipt
            delete(long.Parse(serializedReceipt[0]));

            // Inser the new version of the Receipt
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
