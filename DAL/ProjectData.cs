using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DAL.DTOs;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace DAL
{
    public static class ProjectData
    {
        public static async Task<List<ProjectDTO>> GetAllProjects()
        {
            List<ProjectDTO> list = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);
            using SqlCommand comd = new SqlCommand("SP_Get_All_Projects", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new ProjectDTO
                    {
                        ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
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
                                                null),
                            Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        },
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description"),
                        Status = new StatusDTO
                        (
                            statusId: reader.GetInt32(reader.GetOrdinal("StatusId")),
                            statusName: reader.GetString(reader.GetOrdinal("StatusName"))
                        ),
                        CompletedAt = reader.IsDBNull(reader.GetOrdinal("CompletedAt")) ? null : reader.GetDateTime("CompletedAt"),
                        Images = reader.IsDBNull(reader.GetOrdinal("Images")) ? null : reader.GetString("Images"),
                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime("CreatedAt")
                    });
                }

                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllProjects", ex);
            }
        }
        public static async Task<List<ProjectDTO>> GetAllProjectsByUserId(int UserId)
        {
            List<ProjectDTO> list = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);
            using SqlCommand comd = new SqlCommand("SP_Get_Projects_ByUserId", connection)
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
                    list.Add(new ProjectDTO
                    {
                        ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
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
                                                null),
                            Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        },
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description"),
                        Status = new StatusDTO
                        (
                            statusId: reader.GetInt32(reader.GetOrdinal("StatusId")),
                            statusName: reader.GetString(reader.GetOrdinal("StatusName"))
                        ),
                        CompletedAt = reader.IsDBNull(reader.GetOrdinal("CompletedAt")) ? null : reader.GetDateTime("CompletedAt"),
                        Images = reader.IsDBNull(reader.GetOrdinal("Images")) ? null : reader.GetString("Images"),
                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime("CreatedAt")                    });
                }

                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllProjects", ex);
            }
        }
        public static async Task<ProjectDTO?> GetProjectById(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Get_Project_ById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@ProjectId", id);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new ProjectDTO { 
                        ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
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
                                                null),
                            Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        },
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description"),
                        Status = new StatusDTO
                        (
                            statusId: reader.GetInt32(reader.GetOrdinal("StatusId")),
                            statusName: reader.GetString(reader.GetOrdinal("StatusName"))
                        ),
                        CompletedAt = reader.IsDBNull(reader.GetOrdinal("CompletedAt")) ? null : reader.GetDateTime("CompletedAt"),
                        Images = reader.IsDBNull(reader.GetOrdinal("Images")) ? null : reader.GetString("Images"),
                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime("CreatedAt")
                    }
                ;
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetProjectById", ex);
            }
        }
        public static async Task<List<ProjectDTO>> GetProjectsBySubTeamId(int id)
        {
            List<ProjectDTO> list = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);
            using SqlCommand comd = new SqlCommand("SP_Get_Projects_BySubTeamId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            comd.Parameters.AddWithValue("@SubTeamId", id);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new ProjectDTO
                    {
                        ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
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
                                                null),
                            Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        },
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description"),
                        Status = new StatusDTO
                        (
                            statusId: reader.GetInt32(reader.GetOrdinal("StatusId")),
                            statusName: reader.GetString(reader.GetOrdinal("StatusName"))
                        ),
                        CompletedAt = reader.IsDBNull(reader.GetOrdinal("CompletedAt")) ? null : reader.GetDateTime("CompletedAt"),
                        Images = reader.IsDBNull(reader.GetOrdinal("Images")) ? null : reader.GetString("Images"),
                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime("CreatedAt")
                    });
                }

                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllProjects", ex);
            }
        }     
        public static async Task<List<ProjectDTO>> GetProjectsByStatusId(int id)
        {
            List<ProjectDTO> list = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);
            using SqlCommand comd = new SqlCommand("SP_Get_All_Projects_ByStatusId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            comd.Parameters.AddWithValue("@StatusId", id);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new ProjectDTO
                    {
                        ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
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
                                                null),
                            Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        },
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description"),
                        Status = new StatusDTO
                        (
                            statusId: reader.GetInt32(reader.GetOrdinal("StatusId")),
                            statusName: reader.GetString(reader.GetOrdinal("StatusName"))
                        ),
                        CompletedAt = reader.IsDBNull(reader.GetOrdinal("CompletedAt")) ? null : reader.GetDateTime("CompletedAt"),
                        Images = reader.IsDBNull(reader.GetOrdinal("Images")) ? null : reader.GetString("Images"),
                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime("CreatedAt")
                    });
                }

                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllProjects", ex);
            }
        }
        public static async Task<(bool, int)> CreateProject(Project_Created p)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Create_Project", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@SubTeamId", p.SubTeamId);
            comd.Parameters.AddWithValue("@Title", p.Title);
            comd.Parameters.AddWithValue("@Description", (object?)p.Description ?? DBNull.Value);
            comd.Parameters.AddWithValue("@StatusId", p.StatusId);
            comd.Parameters.AddWithValue("@CompletedAt", (object?)p.CompletedAt ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Images", (object?)p.Images ?? DBNull.Value);
            comd.Parameters.AddWithValue("@UserId", p.CreatedBy);

            SqlParameter output = new SqlParameter("@NewProjectId", SqlDbType.Int)
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
            catch (SqlException ex)
            {
              
                
                throw new Exception("DB Error Create Project", ex);
            }
        }     
        public static async Task<int> UpdateProject(Project_Updated p)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Update_Project", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@ProjectId", p.ProjectId);
            comd.Parameters.AddWithValue("@SubTeamId", p.SubTeamId);
            comd.Parameters.AddWithValue("@Title", p.Title);
            comd.Parameters.AddWithValue("@Description", (object?)p.Description ?? DBNull.Value);
            comd.Parameters.AddWithValue("@StatusId", p.StatusId);
            comd.Parameters.AddWithValue("@CompletedAt", (object?)p.CompletedAt ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Images", (object?)p.Images ?? DBNull.Value);

            try
            {
                await connection.OpenAsync();

                object? result = await comd.ExecuteScalarAsync();

                return result == null || result == DBNull.Value ? -1 : Convert.ToInt32(result);
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error UpdateProject", ex);
            }
        }       
        public static async Task<int> DeleteProject(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Delete_Project", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@ProjectId", id);

            try
            {
                await connection.OpenAsync();

                object? result = await comd.ExecuteScalarAsync();

                return result == null || result == DBNull.Value ? -1 : Convert.ToInt32(result);
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error DeleteProject", ex);
            }
        }
        public static async Task<List<StatusDTO?>> GetProjectStatus()
        {
            List<StatusDTO> statuses = new();
            using SqlConnection connection = new SqlConnection(Setting.Connection);
            using SqlCommand comd = new SqlCommand("SELECT StatusId, StatusName FROM ProjectStatus;", connection);
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

        //ProjectMember
        public static async Task<List<ProjectMemberDTO>> GetProjectMembers(int ProjectId)
        {
            List<ProjectMemberDTO> list = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);
            using SqlCommand comd = new SqlCommand("SP_Get_ProjectMembers_ByProjectId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            comd.Parameters.AddWithValue("@ProjectId", ProjectId);
            
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new ProjectMemberDTO
                    {
                        ProjectMemberId = reader.GetInt32(reader.GetOrdinal("Id")),
                        ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
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
                        RoleInProject = reader.IsDBNull(reader.GetOrdinal("RoleInProject"))
                            ? null : reader.GetString(reader.GetOrdinal("RoleInProject")),
                        JoinedAt = reader.GetDateTime(reader.GetOrdinal("JoinedAt"))
                    });
                }

                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllProjectMembers", ex);
            }
        }
        public static async Task<List<MemberProjects>> GetProjectsByMemberId(int MemberId)
        {
            List<MemberProjects> list = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);
            using SqlCommand comd = new SqlCommand("SP_Get_All_ProjectsByMemberId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            comd.Parameters.AddWithValue("@UserId", MemberId);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new MemberProjects
                    {
                        ProjectMemberId = reader.GetInt32(reader.GetOrdinal("Id")),
                        Project = new Project
                        {
                            ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description")
                        },
                        SubTeam = new SubTeam
                        {
                            SubTeamId = reader.GetInt32(reader.GetOrdinal("SubTeamId")),
                            Title = reader.GetString(reader.GetOrdinal("SubTeamTitle"))
                        }
                        ,
                        JoinAt = reader.GetDateTime(reader.GetOrdinal("JoinedAt")),
                        RoleInProject = reader.IsDBNull(reader.GetOrdinal("RoleInProject")) ? "Member" : reader.GetString("RoleInProject")

                    });
                }

                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllProjectMembers", ex);
            }
        }
        public static async Task<(bool success, int result)> AddProjectMember(ProjectMember_Created m)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Add_ProjectMember", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@ProjectId", m.ProjectId);
            comd.Parameters.AddWithValue("@UserId", m.UserId);
            comd.Parameters.AddWithValue("@RoleInProject", (object?)m.RoleInProject ?? DBNull.Value);

            SqlParameter output = new SqlParameter("@NewId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            comd.Parameters.Add(output);

            try
            {
                await connection.OpenAsync();
                await comd.ExecuteNonQueryAsync();

                int result = output.Value == DBNull.Value ? -1 : Convert.ToInt32(output.Value);

                return (result > 0, result);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    return (false, -2);
                }
                else
                {
                    throw new Exception("DB Error AddProjectMember", ex);
                }
            }
        }
        public static async Task<int> RemoveProjectMember(int projectId, int userId)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Remove_ProjectMember", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@ProjectId", projectId);
            comd.Parameters.AddWithValue("@UserId", userId);

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
                throw new Exception("DB Error RemoveProjectMember", ex);
            }
        }
    }
}