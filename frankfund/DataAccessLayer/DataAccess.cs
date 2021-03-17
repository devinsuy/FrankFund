using System;
using Google.Cloud.BigQuery.V2;

namespace DataAccessLayer

{
    public interface DataAccess<T>
    {

        // ---------------------------------------- Utility Methods ----------------------------------------

        /* Wrapper method, implement using the following line:
           return this.dataHelper.getNextAvailID(this.tableID) */
        public long getNextAvailID();



        // ---------------------------------------- Read, Write, Edit, Delete -------------------------------


        /* Use DataHelper.query() to query BigQuery to READ the records for the
           object with the given PK Identifier. Return results to ServiceLayer */
        public BigQueryResults getUsingID(long ID);


        // Use DataHelper.query() to WRITE a newly created string serialized object into BigQuery
        public void write(string[] serializedObj);

        // Use DataHelper.query() to DELETE an object from BigQuery given its PK identifier
        public void delete(long ID);

        /* Use DataHelper.query() to REWRITE an existing object that changed at runtime
           This method should call delete(long ID) followed by write(string[] serializedObj) */
        public void rewrite(string[] serializedObj);



    }
}
