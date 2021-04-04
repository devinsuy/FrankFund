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
        public readonly long SessionID; // NEED TODO: getNextSessionID function()
        public string JWTToken { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public DateTime DateIssued { get; set; }
        [Required]
        public DateTime DateExpired { get; set; }

        public Session(string userName, string jwtToken)
        {
            this.UserName = userName;
            this.JWTToken = jwtToken;
            this.DateIssued = DateTime.UtcNow;
            this.DateExpired = DateTime.UtcNow.AddMinutes(30);
        }
    }
}
