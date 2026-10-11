using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DAL.DTOs;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public static class TeamData
    {
        public static async Task<List<TeamDTO>> GetAllTeams()
        {
            List<TeamDTO> teams = new();

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Get_All_Teams", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    teams.Add(new TeamDTO
                    {
                        TeamId = reader.GetInt32(reader.GetOrdinal("TeamId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),

                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Description")),

                        Branch = new Branch
                        {
                             BranchId = reader.GetInt32(reader.GetOrdinal("BranchId")),
                             Title = reader.GetString(reader.GetOrdinal("BranchTitle"))
                        },

                        Director = new user
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
                                                null),
                            Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        },
                        Images = reader.IsDBNull(reader.GetOrdinal("Images"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Images"))
                    });
                }

                return teams;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllTeams", ex);
            }
        }
        public static async Task<List<TeamDTO>> GetAllBranchTeams(int BranchId)
        {
            List<TeamDTO> teams = new();

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Get_Teams_ByBranchId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@BranchId", BranchId);
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    teams.Add(new TeamDTO
                    {
                        TeamId = reader.GetInt32(reader.GetOrdinal("TeamId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),

                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Description")),

                        Branch = new Branch
                        {
                            BranchId = reader.GetInt32(reader.GetOrdinal("BranchId")),
                            Title = reader.GetString(reader.GetOrdinal("BranchTitle"))
                        },

                        Director = new user
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
                                                null),
                            Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        },
                        Images = reader.IsDBNull(reader.GetOrdinal("Images"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Images"))
                    });
                }

                return teams;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllTeams", ex);
            }
        }
        public static async Task<TeamDTO?> GetTeamById(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Get_Team_ById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TeamId", id);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new TeamDTO
                    {
                        TeamId = reader.GetInt32(reader.GetOrdinal("TeamId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),

                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Description")),

                        Branch = new Branch
                        {
                            BranchId = reader.GetInt32(reader.GetOrdinal("BranchId")),
                            Title = reader.GetString(reader.GetOrdinal("BranchTitle"))
                        },

                        Director = new user
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
                                                null),
                            Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        },
                        Images = reader.IsDBNull(reader.GetOrdinal("Images"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Images"))
                    };
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetTeamById", ex);
            }
        }   
        public static async Task<(bool, int)> CreateTeam(Team_Created team)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Create_Team", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Title",
                string.IsNullOrEmpty(team.Title) ? DBNull.Value : team.Title);

            command.Parameters.AddWithValue("@Description",
                string.IsNullOrEmpty(team.Description) ? DBNull.Value : team.Description);

            command.Parameters.AddWithValue("@BranchId", team.BranchId);
            command.Parameters.AddWithValue("@DirectorId", team.DirectorId);

            command.Parameters.AddWithValue("@Images",
                string.IsNullOrEmpty(team.Images) ? DBNull.Value : team.Images);

            SqlParameter output = new SqlParameter("@NewTeamId", SqlDbType.Int)
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
                    throw new Exception("DB Error CreateTeam", ex);
                }
            }
        }
        public static async Task<int> UpdateTeam(Team_Updated team)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Update_Team", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TeamId", team.TeamId);

            command.Parameters.AddWithValue("@Title",
                string.IsNullOrEmpty(team.Title) ? DBNull.Value : team.Title);

            command.Parameters.AddWithValue("@Description",
                string.IsNullOrEmpty(team.Description) ? DBNull.Value : team.Description);

            command.Parameters.AddWithValue("@BranchId", team.BranchId);

            command.Parameters.AddWithValue("@DirectorId",
                team.DirectorId.HasValue ? team.DirectorId : DBNull.Value);

            command.Parameters.AddWithValue("@Images",
                string.IsNullOrEmpty(team.Images) ? DBNull.Value : team.Images);

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
                    throw new Exception("DB Error UpdateTeam", ex);
                }
            }
        }
        public static async Task<int> DeleteTeam(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Delete_Team", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TeamId", id);

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
                throw new Exception("DB Error DeleteTeam", ex);
            }
        }
    }
}