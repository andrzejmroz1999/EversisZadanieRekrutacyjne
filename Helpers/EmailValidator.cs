using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EversisZadanieRekrutacyjne.Helpers
{
    public static class EmailValidator
    {
        public static bool ValidateEmail(string email)
        {          
            string pattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
