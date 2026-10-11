using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DAL.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public static class UserData
    {
        public static async Task<List<UserDTO>> GetAllUsers()
        {
            List<UserDTO> users = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);

            using SqlCommand command = new SqlCommand("SP_Get_All_Users", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    users.Add(new UserDTO(
                        userId: reader.GetInt32(reader.GetOrdinal("UserId")),
                        userName: reader.GetString(reader.GetOrdinal("UserName")),
                        email: reader.GetString(reader.GetOrdinal("Email")),
                        birthDate: reader.IsDBNull(reader.GetOrdinal("BirthDate"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                        image: reader.IsDBNull(reader.GetOrdinal("Image"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Image")),
                        bio: reader.IsDBNull(reader.GetOrdinal("Bio"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Bio")),
                        phoneNumber: reader.IsDBNull(reader.GetOrdinal("PhoneNumber"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                        address: reader.IsDBNull(reader.GetOrdinal("Address"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Address")),
                        createdAt: reader.IsDBNull(reader.GetOrdinal("CreatedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        updatedAt: reader.IsDBNull(reader.GetOrdinal("UpdatedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                        role: new RoleDTO(reader.GetInt32(reader.GetOrdinal("RoleId")), reader.GetString(reader.GetOrdinal("RoleName")), null),
                        status: new StatusDTO(reader.GetInt32(reader.GetOrdinal("StatusId")), reader.GetString(reader.GetOrdinal("StatusName"))),
                        communityRole: reader.IsDBNull(reader.GetOrdinal("CommunityRoleId"))? null: new CommunityRoleDTO(reader.GetInt32(reader.GetOrdinal("CommunityRoleId")),reader.GetString(reader.GetOrdinal("CommunityRoleName")),null)
                    ));
                }

                return users;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error Get All Users", ex);
            }
        }  
        public static async Task<List<UserDTO>> GetAllCommunityMembers()
        {
            List<UserDTO> users = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);

            using SqlCommand command = new SqlCommand("SP_Get_All_CommunityMembers", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    users.Add(new UserDTO(
                        userId: reader.GetInt32(reader.GetOrdinal("UserId")),
                        userName: reader.GetString(reader.GetOrdinal("UserName")),
                        email: reader.GetString(reader.GetOrdinal("Email")),
                        birthDate: reader.IsDBNull(reader.GetOrdinal("BirthDate"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                        image: reader.IsDBNull(reader.GetOrdinal("Image"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Image")),
                        bio: reader.IsDBNull(reader.GetOrdinal("Bio"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Bio")),
                        phoneNumber: reader.IsDBNull(reader.GetOrdinal("PhoneNumber"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                        address: reader.IsDBNull(reader.GetOrdinal("Address"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Address")),
                        createdAt: reader.IsDBNull(reader.GetOrdinal("CreatedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        updatedAt: reader.IsDBNull(reader.GetOrdinal("UpdatedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                        role: new RoleDTO(reader.GetInt32(reader.GetOrdinal("RoleId")), reader.GetString(reader.GetOrdinal("RoleName")), null),
                        status: new StatusDTO(reader.GetInt32(reader.GetOrdinal("StatusId")), reader.GetString(reader.GetOrdinal("StatusName"))),
                        communityRole: reader.IsDBNull(reader.GetOrdinal("CommunityRoleId")) ? null : new CommunityRoleDTO(reader.GetInt32(reader.GetOrdinal("CommunityRoleId")), reader.GetString(reader.GetOrdinal("CommunityRoleName")), null)
                    ));
                }

                return users;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error Get All Users", ex);
            }
        }
        public static async Task<List<UserDTO>> GetAllUsersByRoleId(int roleId)
        {
            List<UserDTO> users = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);

            using SqlCommand command = new SqlCommand("SP_Get_Users_ByRoleId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@RoleId", roleId);
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    users.Add(new UserDTO(
                        userId: reader.GetInt32(reader.GetOrdinal("UserId")),
                        userName: reader.GetString(reader.GetOrdinal("UserName")),
                        email: reader.GetString(reader.GetOrdinal("Email")),
                        birthDate: reader.IsDBNull(reader.GetOrdinal("BirthDate"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                        image: reader.IsDBNull(reader.GetOrdinal("Image"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Image")),
                        bio: reader.IsDBNull(reader.GetOrdinal("Bio"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Bio")),
                        phoneNumber: reader.IsDBNull(reader.GetOrdinal("PhoneNumber"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                        address: reader.IsDBNull(reader.GetOrdinal("Address"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Address")),
                        createdAt: reader.IsDBNull(reader.GetOrdinal("CreatedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        updatedAt: reader.IsDBNull(reader.GetOrdinal("UpdatedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                        role: new RoleDTO(reader.GetInt32(reader.GetOrdinal("RoleId")), reader.GetString(reader.GetOrdinal("RoleName")), null),
                        status: new StatusDTO(reader.GetInt32(reader.GetOrdinal("StatusId")), reader.GetString(reader.GetOrdinal("StatusName"))),
                        communityRole: reader.IsDBNull(reader.GetOrdinal("CommunityRoleId")) ? null : new CommunityRoleDTO(reader.GetInt32(reader.GetOrdinal("CommunityRoleId")), reader.GetString(reader.GetOrdinal("CommunityRoleName")), null)

                    ));
                }

                return users;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error Get All Users", ex);
            }
        }
        public static async Task<List<UserDTO>> GetAllUsersByCommunityRoleId(int roleId)
        {
            List<UserDTO> users = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);

            using SqlCommand command = new SqlCommand("SP_Get_Users_ByCommunityRoleId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@RoleId", roleId);
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    users.Add(new UserDTO(
                        userId: reader.GetInt32(reader.GetOrdinal("UserId")),
                        userName: reader.GetString(reader.GetOrdinal("UserName")),
                        email: reader.GetString(reader.GetOrdinal("Email")),
                        birthDate: reader.IsDBNull(reader.GetOrdinal("BirthDate"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                        image: reader.IsDBNull(reader.GetOrdinal("Image"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Image")),
                        bio: reader.IsDBNull(reader.GetOrdinal("Bio"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Bio")),
                        phoneNumber: reader.IsDBNull(reader.GetOrdinal("PhoneNumber"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                        address: reader.IsDBNull(reader.GetOrdinal("Address"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Address")),
                        createdAt: reader.IsDBNull(reader.GetOrdinal("CreatedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        updatedAt: reader.IsDBNull(reader.GetOrdinal("UpdatedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                        role: new RoleDTO(reader.GetInt32(reader.GetOrdinal("RoleId")), reader.GetString(reader.GetOrdinal("RoleName")), null),
                        status: new StatusDTO(reader.GetInt32(reader.GetOrdinal("StatusId")), reader.GetString(reader.GetOrdinal("StatusName"))),
                        communityRole: new CommunityRoleDTO(reader.GetInt32(reader.GetOrdinal("CommunityRoleId")), reader.GetString(reader.GetOrdinal("CommunityRoleName")), null)

                    ));
                }

                return users;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error Get All Users", ex);
            }
        }
        public static async Task<UserDTO?> GetUserById(int id)
        {
            using SqlConnection connection = new SqlConnection(Setting.Connection);

            using SqlCommand command = new SqlCommand("SP_Get_User_ById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@UserId", id);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new UserDTO(
                         userId: reader.GetInt32(reader.GetOrdinal("UserId")),
                        userName: reader.GetString(reader.GetOrdinal("UserName")),
                        email: reader.GetString(reader.GetOrdinal("Email")),
                        birthDate: reader.IsDBNull(reader.GetOrdinal("BirthDate"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                        image: reader.IsDBNull(reader.GetOrdinal("Image"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Image")),
                        bio: reader.IsDBNull(reader.GetOrdinal("Bio"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Bio")),
                        phoneNumber: reader.IsDBNull(reader.GetOrdinal("PhoneNumber"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                        address: reader.IsDBNull(reader.GetOrdinal("Address"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Address")),
                        createdAt: reader.IsDBNull(reader.GetOrdinal("CreatedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        updatedAt: reader.IsDBNull(reader.GetOrdinal("UpdatedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                        role: new RoleDTO(reader.GetInt32(reader.GetOrdinal("RoleId")), reader.GetString(reader.GetOrdinal("RoleName")), null),
                        status: new StatusDTO(reader.GetInt32(reader.GetOrdinal("StatusId")), reader.GetString(reader.GetOrdinal("StatusName"))),
                        communityRole: reader.IsDBNull(reader.GetOrdinal("CommunityRoleId")) ? null : new CommunityRoleDTO(reader.GetInt32(reader.GetOrdinal("CommunityRoleId")), reader.GetString(reader.GetOrdinal("CommunityRoleName")), null)
                    );
                }
                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error Get User By Id", ex);
            }
        }
        public static async Task<(bool, int)> CreateUser(User_Create user)
        {
            using SqlConnection connection = new SqlConnection(Setting.Connection);

            using SqlCommand command = new SqlCommand("SP_Create_User", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@UserName", user.UserName);
            command.Parameters.AddWithValue("@Email", user.Email);
            //Slow Hashing + salt Password By BCrypt 
            command.Parameters.AddWithValue("@PasswordHash", BCrypt.Net.BCrypt.HashPassword(user.Password));
            command.Parameters.AddWithValue("@BirthDate", (object?)user.BirthDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@Image", string.IsNullOrEmpty(user.Image) ? DBNull.Value : user.Image);
            command.Parameters.AddWithValue("@Bio", string.IsNullOrEmpty(user.Bio) ? DBNull.Value : user.Bio);
            command.Parameters.AddWithValue("@PhoneNumber", string.IsNullOrEmpty(user.PhoneNumber) ? DBNull.Value : user.PhoneNumber);
            command.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(user.Address) ? DBNull.Value : user.Address);

            SqlParameter output = new SqlParameter("@NewUserId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            command.Parameters.Add(output);

            try
            {
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                int newId = output.Value == DBNull.Value ? -1 : Convert.ToInt32(output.Value);

                return (newId > 0, newId);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601) // Unique constraint
                {
                    return (false, -2); // Duplicate
                }
                else
                {
                    throw new Exception("DB Error Create User", ex);
                }
            }
        }
        public static async Task<int> UpdateUser(User_Update user)
        {
            using SqlConnection connection = new SqlConnection(Setting.Connection);

            using SqlCommand command = new SqlCommand("SP_Update_User", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@UserId", user.UserId);
            command.Parameters.AddWithValue("@UserName", string.IsNullOrEmpty(user.UserName) ? DBNull.Value : user.UserName);
            command.Parameters.AddWithValue("@BirthDate", (object?)user.BirthDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@Image", string.IsNullOrEmpty(user.Image) ? DBNull.Value : user.Image);
            command.Parameters.AddWithValue("@Bio", string.IsNullOrEmpty(user.Bio) ? DBNull.Value : user.Bio);
            command.Parameters.AddWithValue("@PhoneNumber", string.IsNullOrEmpty(user.PhoneNumber) ? DBNull.Value : user.PhoneNumber);
            command.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(user.Address) ? DBNull.Value : user.Address);

            try
            {
                await connection.OpenAsync();

                object? result = await command.ExecuteScalarAsync();

                return result == null || result == DBNull.Value ? -1 : Convert.ToInt32(result);
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error UpdateUser", ex);
            }
        }   
        public static async Task<int> DeleteUser(int id)
        {
            using SqlConnection connection = new SqlConnection(Setting.Connection);

            using SqlCommand command = new SqlCommand("SP_Delete_User", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@UserId", id);

            try
            {
                await connection.OpenAsync();

                object? result = await command.ExecuteScalarAsync();

                return result == null || result == DBNull.Value ? -1 : Convert.ToInt32(result);
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error Delete User", ex);
            }
        }
        public static async Task<User_Login?> Login(User_Login_Request request)
        {
            await using SqlConnection connection = new(Setting.Connection);
            await using SqlCommand command = new("SP_User_Login", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@email", request.Email);

            await connection.OpenAsync();
            await using SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {

                string hash = reader.GetString(reader.GetOrdinal("PasswordHash"));
                bool isValid = BCrypt.Net.BCrypt.Verify(request.Password, hash);
                if (!isValid)
                    return null;

                var user = new User_Login
                {
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Role = new RoleDTO(reader.GetInt32(reader.GetOrdinal("RoleId")), reader.GetString(reader.GetOrdinal("RoleName")), null),
                    Status = new StatusDTO(reader.GetInt32(reader.GetOrdinal("StatusId")), reader.GetString(reader.GetOrdinal("StatusName")))
                };

                return user;

            }
            return null;

        }
        public static async Task<int> ChangePassword(User_ChangePassword user)
        {
            using SqlConnection connection = new SqlConnection(Setting.Connection);


            string query = "SELECT PasswordHash FROM Users WHERE UserId = @UserId";

            using SqlCommand getCmd = new SqlCommand(query, connection);
            getCmd.Parameters.AddWithValue("@UserId", user.UserId);

            try
            {
                await connection.OpenAsync();

                var result = await getCmd.ExecuteScalarAsync();


                if (result == null)
                    return -1;

                string currentHash = result.ToString();


                bool isValid = BCrypt.Net.BCrypt.Verify(user.OldPassword, currentHash);

                if (!isValid)
                    return 0;


                string newHash = BCrypt.Net.BCrypt.HashPassword(user.NewPassword);

                using SqlCommand updateCmd = new SqlCommand("SP_ChangePassword", connection);
                updateCmd.CommandType = System.Data.CommandType.StoredProcedure;

                updateCmd.Parameters.AddWithValue("@UserId", user.UserId);
                updateCmd.Parameters.AddWithValue("@NewPasswordHash", newHash);

                var updateResult = await updateCmd.ExecuteScalarAsync();

                return Convert.ToInt32(updateResult);
            }
            catch
            {
                return -500; // error
            }
        }
        public static async Task<List<StatusDTO?>> GetUserStatus()
        {
            List<StatusDTO> statuses = new();
            using SqlConnection connection = new SqlConnection(Setting.Connection);
            using SqlCommand comd = new SqlCommand("SELECT StatusId, StatusName FROM Status;", connection);
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    statuses.Add(new StatusDTO(reader.GetInt32(reader.GetOrdinal("StatusId")), reader.GetString(reader.GetOrdinal("StatusName"))));
                }
                return statuses;
            }
            catch (Exception ex)
            {
                throw new Exception("DAL Error", ex);
            }

        }
        public static async Task<int> UpdateUserRole(int userId, int newRoleId,int ChangedBy)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(Setting.Connection))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Update_User_Role", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@NewRoleId", newRoleId);
                    cmd.Parameters.AddWithValue("@ChangedBy", ChangedBy);

                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = Convert.ToInt32(reader["Result"]);
                        }
                    }
                }
            }

            return result;
        }
        public static async Task<int> UpdateUserStatus(int userId, int newRoleId, int ChangedBy)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(Setting.Connection))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Update_User_Status", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@StatusId", newRoleId);
                    cmd.Parameters.AddWithValue("@ChangedBy", ChangedBy);

                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = Convert.ToInt32(reader["Result"]);
                        }
                    }
                }
            }

            return result;
        }
        public static async Task<int> UpdateUserCommunityRole(int userId, int newRoleId,int ChangedBy)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(Setting.Connection))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Update_User_CommunityRole", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@NewRoleId",newRoleId);
                    cmd.Parameters.AddWithValue("@ChangedBy", ChangedBy);
                    await con.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = Convert.ToInt32(reader["Result"]);
                        }
                    }
                }
            }

            return result;
        }

        //-----helper
        public  static int GetUserStatus(int userId)
        {
            using SqlConnection conn = new SqlConnection(Setting.Connection);
            using SqlCommand cmd = new SqlCommand("SELECT StatusId FROM Users WHERE UserId = @UserId", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);

            conn.Open();

            var result = cmd.ExecuteScalar();

            // لو اليوزر مش موجود
            if (result == null)
                return -1;

            return Convert.ToInt32(result);
        }
        public static int GetUserRole(int userId)
        {
            using SqlConnection conn = new SqlConnection(Setting.Connection);
            using SqlCommand cmd = new SqlCommand("SELECT RoleId FROM Users WHERE UserId = @UserId", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);

            conn.Open();

            var result = cmd.ExecuteScalar();

            // لو اليوزر مش موجود
            if (result == null)
                return -1;

            return Convert.ToInt32(result);
        }
        public static async Task<User_response?> IsUserExistsByEmail(string email)
        {

            await using SqlConnection connection = new(Setting.Connection);
            await using SqlCommand command = new("Select UserId,Email From Users where Email = @Email;", connection);
            command.Parameters.AddWithValue("@Email",email);

            await connection.OpenAsync();
            await using SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var user = new User_response
                {
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                };
                return user;
            }
            return null;

        }
        public static async Task<bool> ResetPassword(int userId,string Password)
        {
            await using SqlConnection connection =
                new SqlConnection(Setting.Connection);

            string query = @"
                         UPDATE Users
                         SET PasswordHash = @PasswordHash
                         WHERE UserId = @UserId;";

            await using SqlCommand command =
                new SqlCommand(query, connection);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);

            command.Parameters.AddWithValue(
                "@PasswordHash",
                hashedPassword);

            command.Parameters.AddWithValue(
                "@UserId",
                userId);

            await connection.OpenAsync();

            int rowsAffected =
                await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }


        //--Refresh Token
        public static async Task<UserRefreshToken?> GetRefreshTokenByEmail(string email)
        {
            const string query = @"SELECT U.UserId,U.Email,r.RoleId,r.RoleName, U.RefreshTokenHash, U.RefreshTokenExpiresAt, U.RefreshTokenRevokedAt
                                   from Users U inner join Roles r on U.RoleId = r.RoleId
                                    WHERE Email = @Email;";

            await using SqlConnection connection = new(Setting.Connection);
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Email", SqlDbType.NVarChar, 256).Value = email;

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new UserRefreshToken(
                             Convert.ToInt32(reader["UserId"]),
                             reader["Email"]?.ToString(),
                             role: new RoleDTO(reader.GetInt32(reader.GetOrdinal("RoleId")), reader.GetString(reader.GetOrdinal("RoleName")), null) ,
                             reader["RefreshTokenHash"]?.ToString(),
                             reader["RefreshTokenExpiresAt"] != DBNull.Value
                                   ? (DateTime?)reader["RefreshTokenExpiresAt"]
                                   : null,
                             reader["RefreshTokenRevokedAt"] != DBNull.Value
                                   ? (DateTime?)reader["RefreshTokenRevokedAt"]
                                   : null
                        );
                    }
                    return null;
                }
            }


        }

        public static async Task<bool> UpdateRefreshToken(string email, string refreshToken, DateTime expiresAt)
        {
            const string query = @"UPDATE Users
                                            SET 
                                                RefreshTokenHash = @RefreshTokenHash,
                                                RefreshTokenExpiresAt = @RefreshTokenExpiresAt,
                                                RefreshTokenRevokedAt = NULL
                                            OUTPUT inserted.UserId
                                            WHERE Email = @Email;
                                            ";

            await using SqlConnection conn = new(Setting.Connection);
            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@RefreshTokenHash", BCrypt.Net.BCrypt.HashPassword(refreshToken));
            cmd.Parameters.AddWithValue("@RefreshTokenExpiresAt", expiresAt);

            await conn.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();
            return result != null; 
        }

        public static async Task<bool> UpdateRefreshTokenRevokedAt(string email, DateTime RefreshTokenRevokedAt)
        {
            const string query = @"UPDATE Users
                                            SET 
                                                RefreshTokenRevokedAt = @RefreshTokenRevokedAt
                                            OUTPUT inserted.UserId
                                            WHERE Email = @Email;
                                            ";

            await using SqlConnection conn = new(Setting.Connection);
            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@RefreshTokenRevokedAt", RefreshTokenRevokedAt);

            await conn.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();
            return result != null;
        }


        //--AuditLog
        public static async Task<List<AuditLogDTO>> GetAuditLogs(int page, int pageSize)
        {
            var logs = new List<AuditLogDTO>();

            using (SqlConnection conn = new SqlConnection(Setting.Connection))
            {
                string query = @"SELECT Id, Action, PerformedBy, TargetId, Details, CreatedAt
                          FROM AuditLogs
                          ORDER BY CreatedAt DESC
                          OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";


                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        int idIndex = reader.GetOrdinal("Id");
                        int actionIndex = reader.GetOrdinal("Action");
                        int performedByIndex = reader.GetOrdinal("PerformedBy");
                        int targetIdIndex = reader.GetOrdinal("TargetId");
                        int detailsIndex = reader.GetOrdinal("Details");
                        int createdAtIndex = reader.GetOrdinal("CreatedAt");

                        while (await reader.ReadAsync())
                        {
                            logs.Add(new AuditLogDTO
                            {
                                Id = reader.GetInt64(idIndex),
                                Action = reader.GetString(actionIndex),
                                PerformedBy = reader.GetInt32(performedByIndex),
                                TargetId = reader.GetInt32(targetIdIndex),
                                Details = reader.IsDBNull(detailsIndex)
                                    ? null : reader.GetString(detailsIndex),
                                CreatedAt = reader.GetDateTime(createdAtIndex)
                            });
                        }
                    }
                }
            }

            return logs;
        }
    }
}
