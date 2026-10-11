using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public static class Helper
    {
        public static bool IsActive(int userId)
        {
            return DAL.UserData.GetUserStatus(userId) == 1;
        }
        public static bool IsBlocked(int userId)
        {
            return DAL.UserData.GetUserStatus(userId) == 2;
        }
   
       public static bool IsPasswordValid(string password)
            {
                if (string.IsNullOrWhiteSpace(password))
                    return false;

                if (password.Length < 8)
                    return false;

                if (!password.Any(char.IsUpper))
                    return false;

                if (!password.Any(char.IsLower))
                    return false;

                if (!password.Any(char.IsDigit))
                    return false;

                if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                    return false;

                return true;
            }
        
    }
}
