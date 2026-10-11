using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DAL.DTOs;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public static class SubTeamData
    {  
        public static async Task<List<SubTeamDTO>> GetAllSubTeams()
        {
            List<SubTeamDTO> subTeams = new();

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Get_All_SubTeams", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    subTeams.Add(new SubTeamDTO
                    {
                        SubTeamId = reader.GetInt32(reader.GetOrdinal("SubTeamId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),

                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Description")),

                        Team = new Team
                        {
                            TeamId = reader.GetInt32(reader.GetOrdinal("TeamId")),
                            Title = reader.GetString(reader.GetOrdinal("TeamTitle"))
                        },
                        Head = new user
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

                return subTeams;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllSubTeams", ex);
            }
        }

        public static async Task<List<SubTeamDTO>> GetAllTeamSubTeams(int TeamId)
        {
            List<SubTeamDTO> subTeams = new();

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Get_SubTeams_ByTeamId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@TeamId", TeamId);
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    subTeams.Add(new SubTeamDTO
                    {
                        SubTeamId = reader.GetInt32(reader.GetOrdinal("SubTeamId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),

                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Description")),

                        Team = new Team
                        {
                            TeamId = reader.GetInt32(reader.GetOrdinal("TeamId")),
                            Title = reader.GetString(reader.GetOrdinal("TeamTitle"))
                        },
                        Head = new user
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

                return subTeams;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllSubTeams", ex);
            }
        }
        public static async Task<SubTeamDTO?> GetSubTeamById(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Get_SubTeam_ById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@SubTeamId", id);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new SubTeamDTO
                    {
                        SubTeamId = reader.GetInt32(reader.GetOrdinal("SubTeamId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),

                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Description")),

                        Team = new Team
                        {
                            TeamId = reader.GetInt32(reader.GetOrdinal("TeamId")),
                            Title = reader.GetString(reader.GetOrdinal("TeamTitle"))
                        },
                        Head = new user
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
                throw new Exception("DB Error GetSubTeamById", ex);
            }
        }
        public static async Task<(bool, int)> CreateSubTeam(SubTeam_Created subTeam)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Create_SubTeam", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Title",
                string.IsNullOrEmpty(subTeam.Title) ? DBNull.Value : subTeam.Title);

            command.Parameters.AddWithValue("@Description",
                string.IsNullOrEmpty(subTeam.Description) ? DBNull.Value : subTeam.Description);

            command.Parameters.AddWithValue("@TeamId", subTeam.TeamId);
            command.Parameters.AddWithValue("@HeadId", subTeam.HeadId);

            command.Parameters.AddWithValue("@Images",
                string.IsNullOrEmpty(subTeam.Images) ? DBNull.Value : subTeam.Images);

            SqlParameter output = new SqlParameter("@NewSubTeamId", SqlDbType.Int)
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
                else {
                    throw new Exception("DB Error CreateSubTeam", ex);
                    }
            }
        }
        public static async Task<int> UpdateSubTeam(SubTeam_Updated subTeam)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Update_SubTeam", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@SubTeamId", subTeam.SubTeamId);

            command.Parameters.AddWithValue("@Title",
                string.IsNullOrEmpty(subTeam.Title) ? DBNull.Value : subTeam.Title);

            command.Parameters.AddWithValue("@Description",
                string.IsNullOrEmpty(subTeam.Description) ? DBNull.Value : subTeam.Description);

            command.Parameters.AddWithValue("@TeamId", subTeam.TeamId);

            command.Parameters.AddWithValue("@HeadId",
                subTeam.HeadId.HasValue ? subTeam.HeadId : DBNull.Value);

            command.Parameters.AddWithValue("@Images",
                string.IsNullOrEmpty(subTeam.Images) ? DBNull.Value : subTeam.Images);

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
                    throw new Exception("DB Error UpdateSubTeam", ex);
                }
            }
        }
        public static async Task<int> DeleteSubTeam(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Delete_SubTeam", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@SubTeamId", id);

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
                throw new Exception("DB Error DeleteSubTeam", ex);
            }
        }


        //SUBTEAM MEMBER
        public static async Task<List<SubTeamMembersDTO>> GetAllSubTeamsByUserId(int UserId)
        {
            List<SubTeamMembersDTO> subTeamsMembers = new();

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Get_SubTeams_ByUserId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@UserId",UserId);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    subTeamsMembers.Add(new SubTeamMembersDTO
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        SubTeam = new SubTeam
                        {
                            SubTeamId = reader.GetInt32(reader.GetOrdinal("SubTeamId")),
                            Title = reader.GetString(reader.GetOrdinal("Title"))
                        },
                        Member = new user
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

                        JoinAt = reader.IsDBNull(reader.GetOrdinal("JoinedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("JoinedAt"))
                    });
                }

                return subTeamsMembers;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllSubTeamsMembers", ex);
            }
        }

        public static async Task<List<SubTeamMembersDTO>> GetAllSubTeamMembersBySubTeamId(int SubTeamId)
        {
            List<SubTeamMembersDTO> subTeamsMembers = new();

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Get_SubTeamMembers_BySubTeamId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@SubTeamId", SubTeamId);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    subTeamsMembers.Add(new SubTeamMembersDTO
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        SubTeam = new SubTeam
                        {
                            SubTeamId = reader.GetInt32(reader.GetOrdinal("SubTeamId")),
                            Title = reader.GetString(reader.GetOrdinal("SubTeamTitle"))
                        },
                        Member = new user
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

                        JoinAt = reader.IsDBNull(reader.GetOrdinal("JoinedAt"))
                            ? null
                            : reader.GetDateTime(reader.GetOrdinal("JoinedAt"))
                    });
                }

                return subTeamsMembers;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllSubTeamsMembers", ex);
            }
        }
        public static async Task<(bool, int)> AddMemberToSubTeam(SubTeamMembers_Created member)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Add_SubTeamMember", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@UserId",member.MemberId);

            command.Parameters.AddWithValue("@SubTeamId",member.SubTeamId);

            command.Parameters.AddWithValue("@JoinAt",member.JoinAt ?? null);

            SqlParameter output = new SqlParameter("@NewSubTeamMemberId", SqlDbType.Int)
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
                if(ex.Number == 2627 || ex.Number == 2601)
                {
                    return (false, -2);
                }
                else
                {
                    throw new Exception("DB Error Added SubTeam Member", ex);
                }
            }
        }
        public static async Task<int> RemoveSubTeamMember(SubTeamMembers_Deleted member )
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Remove_SubTeamMember", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@SubTeamId", member.SubTeamId);
            command.Parameters.AddWithValue("@UserId", member.MemberId);

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
                throw new Exception("DB Error Delete SubTeam Member", ex);
            }
        }
    }
}