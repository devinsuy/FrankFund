using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLayer;
using Google.Cloud.BigQuery.V2;

namespace ServiceLayer
{
    public class UserAccountService
    {
        private readonly UserAccountDataAccess UserAccountDataAccess;
        public UserAccountService(BigQueryClient client)
        {
            this.UserAccountDataAccess = new DataAccessLayer.UserAccountDataAccess(client);
        }

        public string GetAccounts(string ID)
        {
            var retrievedUserAccount = UserAccountDataAccess.GetUserAccountUsingID(ID);
            return retrievedUserAccount.GetEnumerator().ToString();
        }
    }
}
