using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using DataAccessLayer.Models;

namespace DataAccessLayer
{
    public class SessionDataAccess : DataAccess
    {
        private readonly DataHelper dataHelper;
        private readonly string tableID;
        public SessionDataAccess()
        {
            this.dataHelper = new DataHelper();
            this.tableID = dataHelper.getQualifiedTableName("Sessions");
        }

        /*
        Use DataHelper.query() to GET BigQueryResults for Session from long ID
            Params: long ID - PK Identifier for Session
            Returns: BigQueryResults for found Session
         */
        public BigQueryResults getUsingID(long ID)
        {
            string query = $"SELECT * FROM {this.tableID} WHERE SessionID = {ID}";
            return this.dataHelper.query(query, parameters: null);
        }

        /*
        Use DataHelper.query() to WRITE a newly created string serialized object into BigQuery
            Params: serializedAcc : string PK Identifier for Session
            Returns: void
         */
        public void write(string[] serializedSess)
        {
            string query;
            query = $"INSERT INTO {this.tableID} VALUES ("
                + getNextAvailID().ToString() + ","               // SessionID
                + $"\"{serializedSess[1]}\","                        // JWTToken
                + $"\"{serializedSess[2]}\","                        // AccountID
                + $"\"{serializedSess[3]}\","                        // AccountUsername
                + $"\"{serializedSess[4]}\","                        // Email Adddress
                + $"\"{serializedSess[5]}\")";                        // DateIssued
            Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
            this.dataHelper.query(query);
        }

        /*
        Use DataHelper.query() to DELETE an object from BigQuery given its PK identifier
            Params: sessID : long PK Identifier for Session
            Returns: void
         */
        public void delete(long sessID)
        {
            string query;
            query = $"DELETE FROM {this.tableID} WHERE SessionID = {sessID}";
            Console.WriteLine("Running Delete Query:\n---------------------\n" + query);
            this.dataHelper.query(query);
        }

        /*
        Use DataHelper.query() to REWRITE an existing object that changed at runtime
        This method should call delete(long ID) followed by write(string[] serializedObj)
            Params: serializedAcc : string PK Identifier for Session
            Returns: void
         */
        public void update(string[] serializedSess)
        {
            delete(long.Parse(serializedSess[0])); // Call delete(long ID) followed by write(string[] serializedObj)
            Console.WriteLine("Session with SessionID " + serializedSess[0] + " was changed, updating records");
            string query;
            query = $"INSERT INTO {this.tableID} VALUES ("
                + serializedSess[0] + ","                          // SessionID
                + $"\"{serializedSess[1]}\","                        // JWTToken
                + $"\"{serializedSess[2]}\","                        // AccountID
                + $"\"{serializedSess[3]}\","                        // AccountUsername
                + $"\"{serializedSess[4]}\","                        // Email Adddress
                + $"\"{serializedSess[5]}\")";                        // DateIssued
            Console.WriteLine("Running Insert Query:\n---------------------\n" + query);
            this.dataHelper.query(query);
        }

        public long getNextAvailID()
        {
            return this.dataHelper.getNextAvailID(this.tableID);
        }
    }
}
