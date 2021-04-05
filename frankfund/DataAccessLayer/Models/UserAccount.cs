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
        // TODO: Have to look into the Facebook and Google API more on what it stores
        //public int FacebookID;
        //public int GoogleID;

        // ---------------------------------------- UserAccount Constructors  ----------------------------------------
        public UserAccount(long AccountID, string username, string email, string pass)
        {
            this.AccountID = AccountID;
            this.AccountUsername = username;
            this.EmailAddress = email;
            this.PasswordHash = pass; // Removed byte[] passSalt from constructor because it gets generated in UserAccountService
            //this.PasswordSalt = passSalt;
            //this.FacebookID = fb;
            //this.GoogleID = google;
        }

        public UserAccount(string username, string email, string pass)
        {
            this.AccountUsername = username;
            this.EmailAddress = email;
            this.PasswordHash = pass;
            //this.PasswordSalt = passSalt;
            //this.FacebookID = fb;
            //this.GoogleID = google;
        }
        public UserAccount(long AccountID, string username, string email, string pass, byte[] passSalt)
        {
            this.AccountUsername = username;
            this.EmailAddress = email;
            this.PasswordHash = pass;
            this.PasswordSalt = passSalt;
            //this.FacebookID = fb;
            //this.GoogleID = google;
        }

        // ---------------------------------------- UserAccount Setters ----------------------------------------

    }
}
