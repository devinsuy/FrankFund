using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ServiceLayer
{
    public static class EmailService
    {
        public static bool IsValidEmailAddress(this string address) =>
        new EmailAddressAttribute().IsValid(address ?? throw new ArgumentNullException());
    }
}
