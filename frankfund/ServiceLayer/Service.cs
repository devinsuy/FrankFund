using System;
namespace ServiceLayer
{
    public interface Service<T>
    {
        // ---------------------------------------- Utility Methods ----------------------------------------

        // Serialize object into string array of the objects attributes
        public string[] serialize(T obj);

        // Serialize object into a valid json string (Check if json is valid here: https://jsonlint.com/)
        public string getJSON(T obj);

        // Get the next available Primary Key ID for the object from BigQuery via DataAccess Layer
        public long getNextAvailID();



        // ---------------------------------------- Read, Write, Edit, Delete -------------------------------

        /* Retrieve from BigQuery via DataAccess Layer and return object 
           (EX: return a reinstantiated Transaction from DB records) */
        public T getUsingID(long ID);


        /* Use DataAccess Layer to write or rewrite a object into BigQuery
               (method should have a way of checking whether the class object changed during runtime
               to avoid redundant writing. Use a changed boolean to implement this)
           Should not call DataAccess write() if did not change */
        public void write(T obj);


        // Use DataAccess Layer to delete a object from BigQuery given its primary key identifier
        public void delete(long ID);

    }
}
