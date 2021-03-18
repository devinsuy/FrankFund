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

namespace ServiceLayer
{
    public class PasswordService
    {
        public bool CheckMinReqPassword(string password)
        {
            // PAssword Minimum Requirements
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
    }
}
