using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DAL.DTOs;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public static class BranchData
    {
       
        public static async Task<List<BranchDTO>> GetAllBranches()
        {
            List<BranchDTO> branches = new();

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Get_All_Branches", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    branches.Add(new BranchDTO
                    {
                        BranchId = reader.GetInt32(reader.GetOrdinal("BranchId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("Description")),

                        Images = reader.IsDBNull(reader.GetOrdinal("Images"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("Images")),

                        Leader = new user
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            Role = new RoleDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("RoleId")),
                                reader.GetString(reader.GetOrdinal("RoleName")),
                                null
                            ),
                            CommunityRole = reader.IsDBNull(reader.GetOrdinal("CommunityRoleId")) ? null
                             : new CommunityRoleDTO(
                                 reader.GetInt32(reader.GetOrdinal("CommunityRoleId")),
                                 reader.IsDBNull(reader.GetOrdinal("CommunityRoleName"))
                                     ? null
                                     : reader.GetString(reader.GetOrdinal("CommunityRoleName")),
                                 null
                             ),
                            Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        }
                    });
                }

                return branches;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllBranches", ex);
            }
        }   
        public static async Task<BranchDTO?> GetBranchById(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Get_Branch_ById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@BranchId", id);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new BranchDTO
                    {
                        BranchId = reader.GetInt32(reader.GetOrdinal("BranchId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("Description")),

                        Images = reader.IsDBNull(reader.GetOrdinal("Images"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("Images")),

                         Leader = new user
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            Role = new RoleDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("RoleId")),
                                reader.GetString(reader.GetOrdinal("RoleName")),
                                null
                            ),
                             CommunityRole = reader.IsDBNull(reader.GetOrdinal("CommunityRoleId")) ? null
                             : new CommunityRoleDTO(
                                 reader.GetInt32(reader.GetOrdinal("CommunityRoleId")),
                                 reader.IsDBNull(reader.GetOrdinal("CommunityRoleName"))
                                     ? null
                                     : reader.GetString(reader.GetOrdinal("CommunityRoleName")),
                                 null
                             ),
                             Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        }
                    };
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetBranchById", ex);
            }
        }
        public static async Task<(bool, int)> CreateBranch(Branch_Created branch)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Create_Branch", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Title",
                string.IsNullOrEmpty(branch.Title) ? DBNull.Value : branch.Title);

            command.Parameters.AddWithValue("@Description",
                string.IsNullOrEmpty(branch.Description) ? DBNull.Value : branch.Description);

            command.Parameters.AddWithValue("@LeaderId", branch.LeaderId);

            command.Parameters.AddWithValue("@Images",
                string.IsNullOrEmpty(branch.Images) ? DBNull.Value : branch.Images);

            SqlParameter output = new SqlParameter("@NewBranchId", SqlDbType.Int)
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
                    throw new Exception("DB Error Create Branch", ex);
                }
            }
        }
        public static async Task<int> UpdateBranch(Branch_Updated branch)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Update_Branch", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@BranchId", branch.BranchId);

            command.Parameters.AddWithValue("@Title",
                string.IsNullOrEmpty(branch.Title) ? DBNull.Value : branch.Title);

            command.Parameters.AddWithValue("@Description",
                string.IsNullOrEmpty(branch.Description) ? DBNull.Value : branch.Description);

            command.Parameters.AddWithValue("@LeaderId",branch.LeaderId);

            command.Parameters.AddWithValue("@Images",
                string.IsNullOrEmpty(branch.Images) ? DBNull.Value : branch.Images);

            try
            {
                await connection.OpenAsync();

                object? resultObj = await command.ExecuteScalarAsync();

                int result = resultObj == null || resultObj == DBNull.Value
                                ? -1
                                : Convert.ToInt32(resultObj);

                return result;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601) // Unique constraint
                {
                    return (-2); // Duplicate
                }
                else
                {
                    throw new Exception("DB Error UpdateBranch", ex);
                }
            }
        }
        public static async Task<int> DeleteBranch(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Delete_Branch", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@BranchId", id);

            try
            {
                await connection.OpenAsync();

                object? resultObj = await command.ExecuteScalarAsync();

                int result = resultObj == null || resultObj == DBNull.Value
                                ? -1
                                : Convert.ToInt32(resultObj);

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error DeleteBranch", ex);
            }
        }
    }
}