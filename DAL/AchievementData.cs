using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DAL.DTOs;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public static class AchievementData
    {
        public static async Task<List<AchievementDTO>> GetAllAchievements()
        {
            List<AchievementDTO> list = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);

            using SqlCommand comd = new SqlCommand("SP_Get_All_Achievements", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new AchievementDTO
                    {
                        AchievementId = reader.GetInt32(reader.GetOrdinal("AchievementId")),
                        SubTeam = new SubTeam
                        {
                            SubTeamId = reader.GetInt32(reader.GetOrdinal("SubTeamId")),
                            Title = reader.GetString(reader.GetOrdinal("SubTeamTitle"))
                        },
                        CreatedBy = new user
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            Role = new RoleDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("RoleId")),
                                reader.GetString(reader.GetOrdinal("RoleName")),
                                null
                            ),
                            CommunityRole = reader.IsDBNull(reader.GetOrdinal("CommunityRoleId"))? null
                             : new CommunityRoleDTO(
                                 reader.GetInt32(reader.GetOrdinal("CommunityRoleId")),
                                 reader.IsDBNull(reader.GetOrdinal("CommunityRoleName"))
                                     ? null
                                     : reader.GetString(reader.GetOrdinal("CommunityRoleName")),
                                 null
                             ),
                            Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        },

                        Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),

                        CompetitionName = reader.IsDBNull(reader.GetOrdinal("CompetitionName")) ? null : reader.GetString(reader.GetOrdinal("CompetitionName")),
                        ProjectId = reader.IsDBNull(reader.GetOrdinal("ProjectId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProjectId")),

                        CompletedAt = reader.IsDBNull(reader.GetOrdinal("CompletedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CompletedAt")),

                        Videos = reader.IsDBNull(reader.GetOrdinal("Videos")) ? null : reader.GetString(reader.GetOrdinal("Videos")),
                        Images = reader.IsDBNull(reader.GetOrdinal("Images")) ? null : reader.GetString(reader.GetOrdinal("Images")),

                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                    });
                }

                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllAchievements", ex);
            }
        }
        public static async Task<List<AchievementDTO>> GetAllAchievementsByUserId(int UserId)
        {
            List<AchievementDTO> list = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);

            using SqlCommand comd = new SqlCommand("SP_Get_Achievements_ByUserId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            comd.Parameters.AddWithValue("@UserId", UserId);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new AchievementDTO
                    {
                        AchievementId = reader.GetInt32(reader.GetOrdinal("AchievementId")),
                        SubTeam = new SubTeam
                        {
                            SubTeamId = reader.GetInt32(reader.GetOrdinal("SubTeamId")),
                            Title = reader.GetString(reader.GetOrdinal("SubTeamTitle"))
                        },
                        CreatedBy = new user
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
                        },

                        Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),

                        CompetitionName = reader.IsDBNull(reader.GetOrdinal("CompetitionName")) ? null : reader.GetString(reader.GetOrdinal("CompetitionName")),
                        ProjectId = reader.IsDBNull(reader.GetOrdinal("ProjectId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProjectId")),

                        CompletedAt = reader.IsDBNull(reader.GetOrdinal("CompletedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CompletedAt")),

                        Videos = reader.IsDBNull(reader.GetOrdinal("Videos")) ? null : reader.GetString(reader.GetOrdinal("Videos")),
                        Images = reader.IsDBNull(reader.GetOrdinal("Images")) ? null : reader.GetString(reader.GetOrdinal("Images")),

                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                    });
                }

                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllAchievements", ex);
            }
        }
        public static async Task<AchievementDTO?> GetAchievementById(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Get_Achievement_ById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@AchievementId", id);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new AchievementDTO
                    {
                        AchievementId = reader.GetInt32(reader.GetOrdinal("AchievementId")),
                        SubTeam = new SubTeam
                        {
                            SubTeamId = reader.GetInt32(reader.GetOrdinal("SubTeamId")),
                            Title = reader.GetString(reader.GetOrdinal("SubTeamTitle"))
                        },
                        CreatedBy = new user
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
                        },

                        Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),

                        CompetitionName = reader.IsDBNull(reader.GetOrdinal("CompetitionName")) ? null : reader.GetString(reader.GetOrdinal("CompetitionName")),
                        ProjectId = reader.IsDBNull(reader.GetOrdinal("ProjectId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ProjectId")),

                        CompletedAt = reader.IsDBNull(reader.GetOrdinal("CompletedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CompletedAt")),

                        Videos = reader.IsDBNull(reader.GetOrdinal("Videos")) ? null : reader.GetString(reader.GetOrdinal("Videos")),
                        Images = reader.IsDBNull(reader.GetOrdinal("Images")) ? null : reader.GetString(reader.GetOrdinal("Images")),

                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                    };
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAchievementById", ex);
            }
        }     
        public static async Task<(bool, int)> CreateAchievement(Achievement_Created a)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Create_Achievement", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@SubTeamId", a.SubTeamId);
            comd.Parameters.AddWithValue("@Title", (object?)a.Title?? DBNull.Value);
            comd.Parameters.AddWithValue("@Description", (object?)a.Description ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Link", (object?)a.Link ?? DBNull.Value);
            comd.Parameters.AddWithValue("@CompetitionName", (object?)a.CompetitionName ?? DBNull.Value);
            comd.Parameters.AddWithValue("@ProjectId", a.ProjectId <=0 ? DBNull.Value: a.ProjectId);
            comd.Parameters.AddWithValue("@CompletedAt", (object?)a.CompletedAt ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Videos", (object?)a.Videos ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Images", (object?)a.Images ?? DBNull.Value);
            comd.Parameters.AddWithValue("@UserId", a.CreatedBy);

            SqlParameter output = new SqlParameter("@NewAchievementId", SqlDbType.BigInt)
            {
                Direction = ParameterDirection.Output
            };

            comd.Parameters.Add(output);

            try
            {
                await connection.OpenAsync();
                await comd.ExecuteNonQueryAsync();

                int newId = output.Value == DBNull.Value ? -1 : Convert.ToInt32(output.Value);

                return (newId > 0, newId);
            }
            catch (Exception ex)
            {
                throw new Exception("DB Error CreateAchievement", ex);
            }
        }
        public static async Task<int> UpdateAchievement(Achievement_Updated a)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Update_Achievement", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@AchievementId", a.AchievementId);
            comd.Parameters.AddWithValue("@SubTeamId", a.SubTeamId);
            comd.Parameters.AddWithValue("@Title", (object?)a.Title ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Description", (object?)a.Description ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Link", (object?)a.Link ?? DBNull.Value);
            comd.Parameters.AddWithValue("@CompetitionName", (object?)a.CompetitionName ?? DBNull.Value);
            comd.Parameters.AddWithValue("@ProjectId", a.ProjectId <= 0 ? DBNull.Value : a.ProjectId);
            comd.Parameters.AddWithValue("@CompletedAt", (object?)a.CompletedAt ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Videos", (object?)a.Videos ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Images", (object?)a.Images ?? DBNull.Value);

            try
            {
                await connection.OpenAsync();

                object? resultObj = await comd.ExecuteScalarAsync();

                int result = resultObj == null || resultObj == DBNull.Value
                    ? -1
                    : Convert.ToInt32(resultObj);

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error UpdateAchievement", ex);
            }
        }
        public static async Task<int> DeleteAchievement(long id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Delete_Achievement", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@AchievementId", id);

            try
            {
                await connection.OpenAsync();

                object? resultObj = await comd.ExecuteScalarAsync();

                return resultObj == null || resultObj == DBNull.Value
                    ? -1
                    : Convert.ToInt32(resultObj);
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error DeleteAchievement", ex);
            }
        }
    }
}