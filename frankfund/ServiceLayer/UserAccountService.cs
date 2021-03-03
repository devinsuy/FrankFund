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
        public UserAccountService()
        {
            this.UserAccountDataAccess = new UserAccountDataAccess();
        }

        public string GetAccountUsingID(string ID)
        {
            var retrievedUserAccount = UserAccountDataAccess.GetUserAccountUsingID(ID);
            return retrievedUserAccount.GetEnumerator().ToString();
        }

        public long getNextAvailID(){
            return UserAccountDataAccess.getNextAvailID();
        }
    }
}
