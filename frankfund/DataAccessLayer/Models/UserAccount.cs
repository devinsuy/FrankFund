using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Models
{
    public class UserAccount
    {
        // User Account Attributes
        public readonly long AccountID;
        public string AccountUsername;
        public string EmailAddress;
        public string PasswordHash;
        public byte[] PasswordSalt;
        public long FacebookID;
        public long GoogleID;

        // ---------------------------------------- UserAccount Constructors  ----------------------------------------
        public UserAccount(long AccountID, string username, string email, string pass, byte[] passSalt, long fb, long google)
        {
            this.AccountID = AccountID;
            this.AccountUsername = username;
            this.EmailAddress = email;
            this.PasswordHash = pass;
            // need to add PasswordSalt
            this.FacebookID = fb;
            this.GoogleID = google;
        }

        // ---------------------------------------- UserAccount Setters ----------------------------------------

    }
}
