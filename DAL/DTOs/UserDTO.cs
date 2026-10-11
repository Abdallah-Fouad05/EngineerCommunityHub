using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DAL.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { set; get; }
        public DateTime? BirthDate { set; get; }
        public string Image { get; set; }
        public string? Bio { set; get; }
        public string? PhoneNumber { set; get; }
        public string? Address { set; get; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public RoleDTO? Role { set; get; }
        public StatusDTO? Status { set; get; }
        public CommunityRoleDTO? CommunityRole { set; get; }

        public UserDTO(int userId, string userName, string email, DateTime? birthDate, string? image, string? bio, string? phoneNumber, string? address, DateTime? createdAt, DateTime? updatedAt, RoleDTO? role, StatusDTO? status,CommunityRoleDTO? communityRole)
        {
            UserId = userId;
            UserName = userName;
            Email = email;
            BirthDate = birthDate;
            Image = image;
            Bio = bio;
            PhoneNumber = phoneNumber;
            Address = address;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Role = role;
            Status = status;
            CommunityRole = communityRole;
        }

    }
    public class User_Create
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Image { get; set; }
        public string? Bio { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
    public class User_Update
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Image { get; set; }
        public string? Bio { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
    public class User_Login_Request
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class User_Login
    {
        public int UserId { get; set; }
        public string Email { set; get; }
        public RoleDTO? Role { set; get; }
        public StatusDTO? Status { get; set; }
    }

    public class User_Create_Request
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? BirthDate { get; set; }
        public IFormFile? Image { get; set; }
        public string? Bio { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
    public class User_Update_Request
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? BirthDate { get; set; }
        public IFormFile? Image { get; set; }
        public string? Bio { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }

    public class User_ChangePassword
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class user
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string? Image { get; set; }
        public RoleDTO Role { get; set; }
        public CommunityRoleDTO? CommunityRole { set; get; }
    }

    public class UserRefreshToken
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public RoleDTO Role { get; set; }
        public string  RefreshToken { get; set; }
        public DateTime?  RefreshTokenExpiresAt { get; set; }
        public DateTime?  RefreshTokenRevokedAt { get; set; } 

        public UserRefreshToken(int userID, string email, RoleDTO role, string? rt, DateTime? rte, DateTime? rtr)
        {
            UserId = userID;
            Email = email;
            Role = role;
            RefreshToken = rt;
            RefreshTokenExpiresAt = rte;
            RefreshTokenRevokedAt = rtr;
        }
    }

    public class User_response
    {
        public int UserId { get; set; }
        public string Email { set; get; }
    }
    public class ResetPasswordDTO
    {
        public string ResetToken { get; set; }

        public string NewPassword { get; set; }
    }


}
