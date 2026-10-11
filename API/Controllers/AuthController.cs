using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BAL;
using DAL.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using BAL.Services;
using System.Security.Cryptography;
using DAL.DTOs.Auth;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DAL;

namespace ASME_Community_API.Controllers
{
    [ApiController]
    [Route("api/v1/Auth")]
    public class AuthController : Controller
    {

        private readonly JwtService _jwtService;
        private readonly EmailServices _emailServices;

        public AuthController(JwtService jwtService,EmailServices emailServices)
        {
            _jwtService = jwtService;
            _emailServices = emailServices;
        }
        private static string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        [HttpPost("refresh")]
        [EnableRateLimiting("AuthLimiter")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
         
            var user = await UserBusiness.GetRefreshTokenByEmail(request.Email.ToString());

            if (user == null)
                return Unauthorized("Invalid refresh request");

            if (user.RefreshTokenRevokedAt != null)
                return Unauthorized("Refresh token is revoked");

            if (user.RefreshTokenExpiresAt == null || user.RefreshTokenExpiresAt <= DateTime.UtcNow)
                return Unauthorized("Refresh token expired");

            bool refreshValid = BCrypt.Net.BCrypt.Verify(request.RefreshToken, user.RefreshToken);
            if (!refreshValid)
                return Unauthorized("Invalid refresh token");


            User_Login Ur = new User_Login
            {
                UserId = user.UserId,
                Email = user.Email,
                Role = user.Role,
                Status = null
            };

            // Issue NEW access token (same claims & signing settings as login)
            var newAccessToken = _jwtService.GenerateToken(Ur); 

            // Rotation: replace refresh token
            var newRefreshToken = GenerateRefreshToken();
            await UserBusiness.UpdateRefreshToken(user.Email,newRefreshToken, DateTime.Now.AddDays(7));

            return Ok(new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }


        [HttpPost("login")]
        [EnableRateLimiting("AuthLimiter")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] User_Login_Request login)
        {
            try
            {
                var user = await UserBusiness.Login(login);

                if (user == null)
                    return Unauthorized(new { message = "Invalid Credentials." });

                if(user.Status.StatusName == "Blocked")
                {
                    return Forbid();
                }

                var accessToken = _jwtService.GenerateToken(user);

                var refreshToken = GenerateRefreshToken();

                // Store refresh token securely (hash + expiry + not revoked)
                await UserBusiness.UpdateRefreshToken(user.Email, refreshToken, DateTime.UtcNow.AddDays(7));

                return Ok(new TokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("SignUp")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateUser([FromForm] User_Create_Request UserRequest)
        {
            try
            {
                if(!Helper.IsPasswordValid(UserRequest.Password))
                    return BadRequest("Invalid password");

                string? fileName = await Services.SaveImageAsync(UserRequest.Image, "Users");


                var NewUser  = new User_Create
                {
                    UserName = UserRequest.UserName,
                    Email = UserRequest.Email,
                    Password = UserRequest.Password,
                    BirthDate = UserRequest.BirthDate,
                    Image = fileName,
                    Bio = UserRequest.Bio,
                    PhoneNumber = UserRequest.PhoneNumber,
                    Address = UserRequest.Address
                };

                var result = await UserBusiness.CreateUser(NewUser);

                if (result.userId == -2)
                {
                    return BadRequest(new
                    {
                        message = "User already exists"
                    });
                }
                if(result.userId <= 0)
                {
                    return BadRequest(new { message = "Failed to create user." });
                }

                UserDTO? userDetails =  await UserBusiness.GetUserById(result.userId);

                if (userDetails == null)
                {
                    return BadRequest(new { message = "Failed to create user." });
                }

                User_Login user = new User_Login
                {
                    UserId = userDetails.UserId,
                    Email = userDetails.Email,
                    Role = userDetails.Role,
                    Status = userDetails.Status
                };

                var accessToken = _jwtService.GenerateToken(user);

                var refreshToken = GenerateRefreshToken();

                // Store refresh token securely (hash + expiry + not revoked)
                await UserBusiness.UpdateRefreshToken(user.Email, refreshToken, DateTime.UtcNow.AddDays(7));

                return Ok(new TokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var user = await UserBusiness.GetRefreshTokenByEmail(request.Email);
            if (user == null)
                return Ok();

            bool refreshValid = BCrypt.Net.BCrypt.Verify(request.RefreshToken, user.RefreshToken);
            if (!refreshValid)
                return Ok();

            if (await UserBusiness.UpdateRefreshTokenRevokedAt(request.Email, DateTime.UtcNow))
                return Ok();

            return Ok();
        }


        [HttpPut("ForgetPassword")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ForgetPassword(string Email)
        {
            try
            {
                User_response? user = await BAL.UserBusiness.IsUserExistsByEmail(Email);

                if (user == null)
                    return Ok();

                var otp = new Random().Next(100000, 999999).ToString();

                await OtpBusiness.SetOTP(user.UserId, otp);

                await _emailServices.SendEmailAsync(Email, "Reset Password", $@"
<div style=""font-family:'Segoe UI', Arial, sans-serif; background:#eef1f5; padding:40px 10px;"">

    <div style=""
        max-width:540px;
        margin:auto;
        background:#ffffff;
        border-radius:14px;
        overflow:hidden;
        box-shadow:0 10px 30px rgba(0,0,0,0.08);
        border:1px solid #d0d7de;
    "">

        <!-- Header -->
        <div style=""
            background:linear-gradient(90deg,#0a66c2,#004182);
            padding:18px 20px;
            text-align:center;
        "">
            <div style=""display:flex; justify-content:center; align-items:center; gap:8px;"">
                
                <!-- Icon -->
                <svg width=""22"" height=""22"" viewBox=""0 0 24 24"" fill=""white"">
                    <path d=""M12 2L2 7l10 5 10-5-10-5zm0 9L2 6v12l10 5 10-5V6l-10 5z""/>
                </svg>

                <h2 style=""margin:0; color:#fff; font-size:18px; font-weight:600;"">
                    ASME Community Hub
                </h2>
            </div>

            <p style=""margin:6px 0 0; color:#dbe9ff; font-size:12px; display:flex; justify-content:center; align-items:center; gap:6px;"">
                
                <!-- Lock icon -->
                <svg width=""14"" height=""14"" viewBox=""0 0 24 24"" fill=""#dbe9ff"">
                    <path d=""M12 1a5 5 0 00-5 5v4H6a2 2 0 00-2 2v9a2 2 0 002 2h12a2 2 0 002-2v-9a2 2 0 00-2-2h-1V6a5 5 0 00-5-5zm-3 9V6a3 3 0 016 0v4H9z""/>
                </svg>

                Secure Password Reset Request
            </p>
        </div>

        <!-- Body -->
        <div style=""padding:28px 26px; color:#1f2328;"">

            <p style=""font-size:14px; margin-bottom:10px;"">
                👋 Hello Engineer
            </p>

            <p style=""font-size:14px; color:#444; line-height:1.6;"">
                We received a request to reset your password for your ASME Community Hub account.
            </p>

            <p style=""font-size:14px; color:#444; margin-top:14px;"">
                Please use the verification code below:
            </p>

            <!-- OTP Box -->
            <div style=""text-align:center; margin:26px 0;"">
                <div style=""
                    display:inline-block;
                    font-size:28px;
                    letter-spacing:10px;
                    font-weight:700;
                    color:#0a66c2;
                    background:#f3f6fb;
                    padding:16px 28px;
                    border-radius:12px;
                    border:2px solid #cfe3ff;
                "">
                    {otp}
                </div>
            </div>

            <!-- Warning box with icon -->
            <div style=""
                background:#f6f8fa;
                padding:12px;
                border-radius:8px;
                text-align:center;
                font-size:12px;
                color:#555;
                display:flex;
                justify-content:center;
                align-items:center;
                gap:6px;
            "">
                <svg width=""16"" height=""16"" viewBox=""0 0 24 24"" fill=""#0a66c2"">
                    <path d=""M12 2a10 10 0 100 20 10 10 0 000-20zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z""/>
                </svg>
                This code expires in <b>15 minutes</b>
            </div>

            <hr style=""border:none; border-top:1px solid #e5e7eb; margin:22px 0;"" />

            <p style=""font-size:12px; color:#6b7280; line-height:1.5;"">
                If you did not request this change, you can safely ignore this email.  
                Your account remains secure.
            </p>

        </div>

        <!-- Footer -->
        <div style=""
            background:#f6f8fa;
            padding:14px;
            text-align:center;
            font-size:11px;
            color:#6b7280;
            display:flex;
            justify-content:center;
            align-items:center;
            gap:6px;
        "">
            <svg width=""14"" height=""14"" viewBox=""0 0 24 24"" fill=""#6b7280"">
                <path d=""M12 2L2 7v10c0 5 10 9 10 9s10-4 10-9V7l-10-5z""/>
            </svg>
            © ASME Community Hub • Engineering Network
        </div>

    </div>
</div>
    ");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("VerifyOTP")]
        public async Task<IActionResult> VerifyOTP(VerifyOTPDTO dto)
        {
            User_response? user = await BAL.UserBusiness.IsUserExistsByEmail(dto.Email);

            if (user == null)
                return BadRequest(new VerifyOTPResponseDTO
                {
                    IsVerified = false,
                    Message = "Email Not Found"
                });

            VerifyOTPResponseDTO? result = await BAL.OtpBusiness.VerifyOTP(user.UserId,dto.Email,dto.OTP);

            if (!result.IsVerified)
                return BadRequest(result);

            string? ResetToken = _jwtService.GenerateResetToken(user.UserId);

            VerifyOTPResponseDTO response = new VerifyOTPResponseDTO
            {
                IsVerified = result.IsVerified,
                Message = result.Message,
                ResetToken = ResetToken
            };

            return Ok(response);
        }

        [HttpPut("ResetPassword")]
        [EnableRateLimiting("AuthLimiter")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO dto)
        {
            if (!Helper.IsPasswordValid(dto.NewPassword))
                return BadRequest("Invalid password");

            // Validate Reset Token
            var principal =
                _jwtService.ValidateResetToken(dto.ResetToken);

            if (principal == null)
                return BadRequest("Invalid or expired reset token");

            // Extract UserId
            int userId = int.Parse(
                principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Update Password
            bool isUpdated =
                await UserData.ResetPassword(userId, dto.NewPassword);

            if (!isUpdated)
                return StatusCode(500, "Failed to reset password");

            return Ok("Password reset successfully");
        }
    }
}
