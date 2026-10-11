using System.Data;
using DAL;
using DAL.DTOs;
using Microsoft.Data.SqlClient;

namespace BAL
{
    public class UserBusiness
    {
        public static async Task<List<UserDTO>> GetAllUsers()
        {
            try
            {
                return await UserData.GetAllUsers();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all users.", ex);
            }
        }
        
         public static async Task<List<UserDTO>> GetAllCommunityMembers()
         {
            try
            {
                return await UserData.GetAllCommunityMembers();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all users.", ex);
            }
         }

        public static async Task<List<StatusDTO>> GetUserStatus()
        {
            try
            {
                return await UserData.GetUserStatus();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all Status.", ex);
            }
        }
        public static async Task<List<UserDTO>> GetAllUsersByRoleId(int roleId)
        {
            try
            {
                return await UserData.GetAllUsersByRoleId(roleId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all users.", ex);
            }
        }

        public static async Task<List<UserDTO>> GetAllUsersByCommunityRoleId(int roleId)
        {
            try
            {
                return await UserData.GetAllUsersByCommunityRoleId(roleId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all users.", ex);
            }
        }

        public static async Task<UserDTO?> GetUserById(int id)
        {
            try
            {
                return await UserData.GetUserById(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while fetching user with ID {id}.", ex);
            }
        }

        public static async Task<(bool success, int userId)> CreateUser(User_Create user)
        {
            try
            {
                return await UserData.CreateUser(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while creating user.", ex);
            }
        }

        public static async Task<int> UpdateUser(User_Update user)
        {
            try
            {
                return await UserData.UpdateUser(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while updating user with ID {user.UserId}.", ex);
            }
        }

        public static async Task<int> ChangePassword(User_ChangePassword user)
        {
            try
            {
                return await UserData.ChangePassword(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while updating Password with ID {user.UserId}.", ex);
            }
        }

        public static async Task<int> DeleteUser(int id)
        {
            try
            {
                return await UserData.DeleteUser(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while deleting user with ID {id}.", ex);
            }
        }

        public static async Task<User_Login?> Login(User_Login_Request request)
        {
            try
            {
                return await UserData.Login(request);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Faild to login", ex);
            }
        }
        public static async Task<int> UpdateUserRole(int userId, int RoleId,int ChangedBy)
        {
            try
            {
                if (userId <= 0 || ChangedBy <= 0)
                    return -99;

                var changerStatus = UserData.GetUserStatus(ChangedBy);

                if (changerStatus == -1)
                    return -1;


                if (changerStatus != 1) //Active
                    return -4;

                return await UserData.UpdateUserRole(userId, RoleId,ChangedBy);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Faild Update", ex);
            }
        }

        public static async Task<int> UpdateUserStatus(int userId, int RoleId,int ChangedBy)
        {
            try
            {
                if (userId <= 0 || ChangedBy <= 0)
                    return -99;

                var changerStatus = UserData.GetUserStatus(ChangedBy);

                if (changerStatus == -1)
                    return -1;

                
                if (changerStatus != 1) //Active 
                    return -4;

                return await UserData.UpdateUserStatus(userId, RoleId,ChangedBy);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Faild Update", ex);
            }
        }
        public static async Task<int> UpdateUserCommunityRole(int userId, int RoleId, int ChangedBy)
        {
            try
            {
                if (userId <= 0 || ChangedBy <= 0)
                    return -99;

                var changerStatus = UserData.GetUserStatus(ChangedBy);

                if (changerStatus == -1)
                    return -1;


                if (changerStatus != 1) //Active
                    return -4;

                return await UserData.UpdateUserCommunityRole(userId, RoleId, ChangedBy);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Faild Update", ex);
            }
        }

        public static async Task<UserRefreshToken?> GetRefreshTokenByEmail(string email)
        {
            return await UserData.GetRefreshTokenByEmail(email);
        }

        public static async Task<bool> UpdateRefreshToken(string email, string refreshToken, DateTime expiresAt)
        {
            return await UserData.UpdateRefreshToken(email, refreshToken, expiresAt);
        }

        public static async Task<bool> UpdateRefreshTokenRevokedAt(string email, DateTime RefreshTokenRevokedAt)
        {
            return await UserData.UpdateRefreshTokenRevokedAt(email, RefreshTokenRevokedAt);
        }
        public static async Task<List<AuditLogDTO>> GetAuditLogs(int page, int pageSize)
        {
            return await DAL.UserData.GetAuditLogs(page, pageSize);
        }

        public static async Task<User_response?> IsUserExistsByEmail(string email)
        {
            return await DAL.UserData.IsUserExistsByEmail(email);
        }
        public static async Task<bool?> ResetPassword(int UserId, string Password)
        {
            return await DAL.UserData.ResetPassword(UserId, Password);
        }

    }
}