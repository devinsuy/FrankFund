using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer.Models
{
    // Model for user session (when they are logged in using the application)
    public class Session
    {
        // Session Attributes
        public readonly long SessionID;
        public string JWTToken { get; set; }
        public long AccountID { get; set; }
        [Required]
        public string AccountUsername { get; set; }
        public string EmailAddress { get; set; }
        [Required]
        public DateTime DateIssued { get; set; }

        public Session(string jwtToken, long accID, string userName, string email)
        {
            this.JWTToken = jwtToken;
            this.AccountID = accID;
            this.AccountUsername = userName;
            this.EmailAddress = email;
            this.DateIssued = DateTime.UtcNow;
        }

        //public Session(string jwtToken, long accID, string userName, string email)
        //{
        //    this.JWTToken = jwtToken;
        //    this.AccountID = accID;
        //    this.AccountUsername = userName;
        //    this.EmailAddress = email;
        //    this.DateIssued = DateTime.UtcNow;
        //}

        public Session(long sessID, string jwtToken, long accID, string userName, string email, DateTime date)
        {
            this.SessionID = sessID;
            this.JWTToken = jwtToken;
            this.AccountID = accID;
            this.AccountUsername = userName;
            this.EmailAddress = email;
            this.DateIssued = date;
        }
    }
}
