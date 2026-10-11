using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTOs;

namespace BAL
{
    public class OtpBusiness
    {
        public static async Task<bool?> SetOTP(int UserId, string otp)
        {
            return await DAL.OtpData.SetOTP(UserId, otp);
        }

        public static async Task<VerifyOTPResponseDTO?> VerifyOTP(int userId, string email, string otp)
        {
            return await DAL.OtpData.VerifyOTP(userId, email, otp);
        }
    }

}
