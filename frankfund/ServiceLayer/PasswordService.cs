using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using DataAccessLayer;
using Google.Cloud.BigQuery.V2;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace ServiceLayer
{
    public class PasswordService
    {
        // Checks for Password Minimum Requirements
        //  @password : string password
        public bool CheckMinReqPassword(string password)
        {
            // Length of 8 characters
            // Uppercase letter (A-Z)
            // Lowercase letter(a-z)
            // Digit(0 - 9)

            bool meetsRequirement;
            Console.WriteLine("Checking minimum requirements of password: " + password);

            // Length of 8 characters
            if (password.Length < 8)
            {
                Console.WriteLine("The password must have a minimum length of 8 characters.");
                meetsRequirement = false;
            }
            else
            {
                meetsRequirement = true;
            }
            // Uppercase letter (A-Z)
            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                Console.WriteLine("The password must contain at least one capital letter.");
                meetsRequirement = false;
            }
            else
            {
                meetsRequirement = true;
            }
            if (!Regex.IsMatch(password, "[a-z]"))
            {
                Console.WriteLine("The password must contain at least one lowercase letter.");
                meetsRequirement = false;
            }
            else
            {
                meetsRequirement = true;
            }
            if (!Regex.IsMatch(password, "[0-9]"))
            {
                Console.WriteLine("The password must contain at least one number digit.");
                meetsRequirement = false;
            }
            //else if(!Regex.IsMatch(password, "[~`!@#$%^&*()+=_-{}[]\|:;”’?/<>,.]"))
            //{
            //    Console.WriteLine("The password must contain at least one special character.");
            //    meetsRequirement = false;
            //}
            else
            {
                meetsRequirement = true;
            }
            return meetsRequirement;
        }

        // Generates salt for password
        public byte[] GenerateSalt()
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            return salt;
        }

        // Hashes password
        public string HashPassword(string password, byte[] salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }
    }
}
