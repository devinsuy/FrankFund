using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.BigQuery.V2;

namespace DataAccessLayer
{
    public class UserAccountDataAccess
    {
        private readonly DataHelper dataHelper;
        public UserAccountDataAccess()
        {
            this.dataHelper = new DataHelper();
        }

        public BigQueryResults GetUserAccountUsingID(string ID)
        {
            string tableID = this.dataHelper.getQualifiedTableName("Accounts");
            string query = $"SELECT * FROM {tableID} WHERE AccountID = {ID}";        
            return this.dataHelper.query(query, parameters: null);
        }
    }
}
