using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.BigQuery.V2;

namespace DataAccessLayer
{
    public class UserAccountDataAccess
    {
        private readonly DataHelper dataHelper;
        private readonly string tableID;
        public UserAccountDataAccess()
        {
            this.dataHelper = new DataHelper();
            this.tableID = dataHelper.getQualifiedTableName("Accounts");
        }

        public BigQueryResults GetUserAccountUsingID(string ID)
        {
            string query = $"SELECT * FROM {tableID} WHERE AccountID = {ID}";        
            return this.dataHelper.query(query, parameters: null);
        }

        public long getNextAvailID(){
            return this.dataHelper.getNextAvailID(this.tableID);
        }
    }
}
