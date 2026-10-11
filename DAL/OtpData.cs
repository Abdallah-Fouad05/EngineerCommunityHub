using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTOs;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class OtpData
    {
        public static async Task<bool> SetOTP(int userId, string otp)
        {

            DateTime createdAt = DateTime.Now;
            DateTime expiresAt = createdAt.AddMinutes(10);
            string otphush = BCrypt.Net.BCrypt.HashPassword(otp);

            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            string query = @"
            INSERT INTO OTPRequests
             (UserId, OTPHash, CreatedAt, ExpiresAt, IsUsed)
            VALUES
            (@UserId, @OTPHash, @CreatedAt, @ExpiresAt, 0);";

            await using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@OTPHash", otphush);
            command.Parameters.AddWithValue("@CreatedAt", createdAt);
            command.Parameters.AddWithValue("@ExpiresAt", expiresAt);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return true;
        }
        public static async Task<OTPRequest?> GetActiveOTP(int userId, string purpose)
        {
            await using SqlConnection connection =
                new SqlConnection(Setting.Connection);

            string query = @"
                        SELECT TOP 1
                               Id,
                               UserId,
                               OTPHash,
                               CreatedAt,
                               ExpiresAt,
                               IsUsed
                        FROM OTPRequests
                        WHERE UserId = @UserId
                          AND IsUsed = 0
                          AND ExpiresAt > GETDATE()
                        ORDER BY CreatedAt DESC;";

            await using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserId", userId);

            await connection.OpenAsync();

            using SqlDataReader reader =
                await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new OTPRequest
                {
                    Id = (int)reader["Id"],

                    UserId = (int)reader["UserId"],

                    OTPHash = reader["OTPHash"].ToString(),

                    CreatedAt = (DateTime)reader["CreatedAt"],

                    ExpiresAt = (DateTime)reader["ExpiresAt"],

                    IsUsed = (bool)reader["IsUsed"]
                };
            }

            return null;
        }
        public static async Task<bool> MarkAsUsed(int otpId)
        {
            await using SqlConnection connection =
                new SqlConnection(Setting.Connection);

            string query = @"
                    UPDATE OTPRequests
                    SET IsUsed = 1
                    WHERE Id = @Id;";

            await using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", otpId);

            await connection.OpenAsync();

            int rowsAffected =
                await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }
        public static async Task<VerifyOTPResponseDTO> VerifyOTP(int UserId, string email, string inputOtp)
        {

            OTPRequest? otpRecord = await OtpData.GetActiveOTP(UserId, "ResetPassword");

            if (otpRecord == null)
            {
                return new VerifyOTPResponseDTO
                {
                    IsVerified = false,
                    Message = "OTP expired or not found"
                };
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(inputOtp, otpRecord.OTPHash);

            if (!isValid)
            {
                return new VerifyOTPResponseDTO
                {
                    IsVerified = false,
                    Message = "Invalid OTP"
                };
            }

            // Mark OTP as used
            await OtpData.MarkAsUsed(otpRecord.Id);

            return new VerifyOTPResponseDTO
            {
                IsVerified = true,
                Message = "OTP verified successfully",
                ResetToken = null
            };
        }
    }
}
