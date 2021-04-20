using System;
using Google.Cloud.BigQuery.V2;

namespace DataAccessLayer
{
    public class ReceiptDataAccess: DataAccess
    {
        private readonly DataHelper dataHelper;
        private readonly StorageBucketHelper storageHelper;
        private readonly string tableID;
        private readonly string transactionTable;

        public ReceiptDataAccess()
        {
            this.dataHelper = new DataHelper();
            this.storageHelper = new StorageBucketHelper("receipt_imgs");
            this.tableID = this.dataHelper.getQualifiedTableName("Receipts");
            this.tableID = this.dataHelper.getQualifiedTableName("Transactions");
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

        // Returns all transactions with a given user ordered by date entered
        public BigQueryResults getTransactionFromRID(long TID, long RID)
        {
            string query = "SELECT * FROM FrankFund.Receipt r"
                + $" WHERE r.TID = {TID}";
            return this.dataHelper.query(query, parameters: null);
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


        // ------------------------------ Receipt Upload/Download ------------------------------

        // Upload a file to the storage bucket under the given users folder
        // Returns: the gs:// url of the image file resource
        public string uploadFile(string userName, string localPathToFile, string fileName)
        {
            return this.storageHelper.uploadFile(userName, localPathToFile, fileName);
        }


        /* Download a file from cloud storage and save it to a given local path: DataAccessLayer/tmp/download/{filename} by default
           Params:
                userName: The name of the user's folder the file is contained in
                fileName: The name of the file in the given folder
           Returns:
                The local location the file was saved
         */
        public string downloadFile(string userName, string fileName, string dstPath)
        {
            return this.storageHelper.downloadFile(userName, fileName, dstPath);
        }


    }
}
