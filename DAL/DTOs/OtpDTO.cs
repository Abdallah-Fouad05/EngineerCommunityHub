using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class VerifyOTPDTO
    {
        public string Email { get; set; }
        public string OTP { get; set; }
    }
    public class OTPRequest
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string OTPHash { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; }

    }
    public class VerifyOTPResponseDTO
    {
        public bool IsVerified { get; set; }

        public string Message { get; set; }

        public string? ResetToken { get; set; }
    }
}
