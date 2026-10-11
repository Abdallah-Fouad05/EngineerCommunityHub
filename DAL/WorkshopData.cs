using System.Data;
using Microsoft.Data.SqlClient;
using DAL.DTOs;
using static System.Net.Mime.MediaTypeNames;

namespace DAL
{
    public static class WorkshopData
    { 
        public static async Task<List<WorkshopDTO>> GetAllWorkshops()
        {
            List<WorkshopDTO> workshops = new();

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Get_All_Workshops", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    workshops.Add(new WorkshopDTO
                    {
                        WorkshopId = reader.GetInt32(reader.GetOrdinal("WorkShopId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Link = reader.IsDBNull(reader.GetOrdinal("Link"))
                            ? null : reader.GetString(reader.GetOrdinal("Link")),
                        Topics = reader.IsDBNull(reader.GetOrdinal("Topics"))
                            ? null : reader.GetString(reader.GetOrdinal("Topics")),
                        WorkshopDate = reader.GetDateTime(reader.GetOrdinal("EventDate")),
                        StartTime = reader.GetTimeSpan(reader.GetOrdinal("StartTime")),
                        EndTime = reader.IsDBNull(reader.GetOrdinal("EndTime"))
                            ? null : reader.GetTimeSpan(reader.GetOrdinal("EndTime")),

                        Location = reader.GetString(reader.GetOrdinal("Location")),
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

                        Images = reader.IsDBNull(reader.GetOrdinal("Images"))
                            ? null : reader.GetString(reader.GetOrdinal("Images")),
                        Status =  new StatusDTO(reader.GetInt32(reader.GetOrdinal("StatusId")), reader.GetString(reader.GetOrdinal("StatusName")))
                    });
                }

                return workshops;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllWorkshops : ", ex);
            }
        }
        public static async Task<List<WorkshopDTO>> GetAllWorkshopsByStatusId(int StatusId)
        {
            List<WorkshopDTO> workshops = new();

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Get_WorkShops_ByStatusId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            comd.Parameters.AddWithValue("@StatusId", StatusId);
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    workshops.Add(new WorkshopDTO
                    {
                        WorkshopId = reader.GetInt32(reader.GetOrdinal("WorkShopId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Link = reader.IsDBNull(reader.GetOrdinal("Link"))
                            ? null : reader.GetString(reader.GetOrdinal("Link")),
                        Topics = reader.IsDBNull(reader.GetOrdinal("Topics"))
                            ? null : reader.GetString(reader.GetOrdinal("Topics")),
                        WorkshopDate = reader.GetDateTime(reader.GetOrdinal("EventDate")),
                        StartTime = reader.GetTimeSpan(reader.GetOrdinal("StartTime")),
                        EndTime = reader.IsDBNull(reader.GetOrdinal("EndTime"))
                            ? null : reader.GetTimeSpan(reader.GetOrdinal("EndTime")),

                        Location = reader.GetString(reader.GetOrdinal("Location")),
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

                        Images = reader.IsDBNull(reader.GetOrdinal("Images"))
                            ? null : reader.GetString(reader.GetOrdinal("Images")),
                        Status = new StatusDTO(reader.GetInt32(reader.GetOrdinal("StatusId")), reader.GetString(reader.GetOrdinal("StatusName")))
                    });
                }

                return workshops;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllWorkshops : ", ex);
            }
        }
        public static async Task<List<WorkshopDTO>> GetAllWorkshopsByUserId(int UserId)
        {
            List<WorkshopDTO> workshops = new();

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Get_WorkShops_ByUserId", connection)
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
                    workshops.Add(new WorkshopDTO
                    {
                        WorkshopId = reader.GetInt32(reader.GetOrdinal("WorkShopId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Link = reader.IsDBNull(reader.GetOrdinal("Link"))
                            ? null : reader.GetString(reader.GetOrdinal("Link")),
                        Topics = reader.IsDBNull(reader.GetOrdinal("Topics"))
                            ? null : reader.GetString(reader.GetOrdinal("Topics")),
                        WorkshopDate = reader.GetDateTime(reader.GetOrdinal("EventDate")),
                        StartTime = reader.GetTimeSpan(reader.GetOrdinal("StartTime")),
                        EndTime = reader.IsDBNull(reader.GetOrdinal("EndTime"))
                            ? null : reader.GetTimeSpan(reader.GetOrdinal("EndTime")),

                        Location = reader.GetString(reader.GetOrdinal("Location")),
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

                        Images = reader.IsDBNull(reader.GetOrdinal("Images"))
                            ? null : reader.GetString(reader.GetOrdinal("Images")),
                        Status = new StatusDTO(reader.GetInt32(reader.GetOrdinal("StatusId")), reader.GetString(reader.GetOrdinal("StatusName")))
                    });
                }

                return workshops;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllWorkshops : ", ex);
            }
        }
        public static async Task<WorkshopDTO?> GetWorkshopById(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Get_Workshop_ById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@WorkShopId", id);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new WorkshopDTO
                    {
                        WorkshopId = reader.GetInt32(reader.GetOrdinal("WorkShopId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Topics = reader.IsDBNull(reader.GetOrdinal("Topics"))
                            ? null : reader.GetString(reader.GetOrdinal("Topics")),
                        Link = reader.IsDBNull(reader.GetOrdinal("Link"))
                            ? null : reader.GetString(reader.GetOrdinal("Link")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null : reader.GetString(reader.GetOrdinal("Description")),
                        WorkshopDate = reader.GetDateTime(reader.GetOrdinal("EventDate")),
                        StartTime = reader.GetTimeSpan(reader.GetOrdinal("StartTime")),
                        EndTime = reader.IsDBNull(reader.GetOrdinal("EndTime"))
                            ? null : reader.GetTimeSpan(reader.GetOrdinal("EndTime")),

                        Location = reader.GetString(reader.GetOrdinal("Location")),
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

                        Images = reader.IsDBNull(reader.GetOrdinal("Images"))
                            ? null : reader.GetString(reader.GetOrdinal("Images")),
                        Status = new StatusDTO(reader.GetInt32(reader.GetOrdinal("StatusId")), reader.GetString(reader.GetOrdinal("StatusName")))
                    };
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetWorkshopById : ", ex);
            }
        }        
        public static async Task<(bool, int)> CreateWorkshop(Workshop_Created wk)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Create_Workshop", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@Title", wk.Title);
            comd.Parameters.AddWithValue("@Description",
                string.IsNullOrEmpty(wk.Description) ? DBNull.Value : wk.Description);

            comd.Parameters.AddWithValue("@Topics",
              string.IsNullOrEmpty(wk.Topics) ? DBNull.Value : wk.Topics);



            comd.Parameters.AddWithValue("@EventDate", wk.WorkshopDate);

            comd.Parameters.AddWithValue("@StartTime", wk.StartTime);
            
            comd.Parameters.AddWithValue("@EndTime",
                wk.EndTime.HasValue ? wk.EndTime.Value : DBNull.Value);

            comd.Parameters.AddWithValue("@Location", wk.Location);
            comd.Parameters.AddWithValue("@UserId", wk.CreatedBy);

            comd.Parameters.AddWithValue("@Link", wk.Link);

            comd.Parameters.AddWithValue("@Images",
                string.IsNullOrEmpty(wk.Images) ? DBNull.Value : wk.Images);

            SqlParameter outputParam = new SqlParameter("@NewWorkShopId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            comd.Parameters.Add(outputParam);

            try
            {
                await connection.OpenAsync();

                await comd.ExecuteNonQueryAsync();

                int newId = outputParam.Value == DBNull.Value
                    ? -1 : Convert.ToInt32(outputParam.Value);

                return (newId > 0, newId);
            }
            catch (Exception ex)
            {
                throw new Exception("DB Error CreateWorkshop.", ex);
            }
        }
        public static async Task<int> UpdateWorkshop(Workshop_Updated wk)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Update_Workshop", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@WorkShopId", wk.WorkshopId);
            command.Parameters.AddWithValue("@Title", wk.Title);

            command.Parameters.AddWithValue("@Description",
                string.IsNullOrEmpty(wk.Description) ? DBNull.Value : wk.Description);


            command.Parameters.AddWithValue("@Topics",
                string.IsNullOrEmpty(wk.Topics) ? DBNull.Value : wk.Topics);

            command.Parameters.AddWithValue("@EventDate", wk.WorkshopDate);


            command.Parameters.AddWithValue("@StartTime", wk.StartTime);

            command.Parameters.AddWithValue("@EndTime",
                wk.EndTime.HasValue ? wk.EndTime : DBNull.Value);

            command.Parameters.AddWithValue("@Location", wk.Location);
          

            command.Parameters.AddWithValue("@Link", wk.Link);

            command.Parameters.AddWithValue("@Images",
                string.IsNullOrEmpty(wk.Images) ? DBNull.Value : wk.Images);

            try
            {
                await connection.OpenAsync();

                object? resultObj = await command.ExecuteScalarAsync();

                int result = resultObj == null || resultObj == DBNull.Value
                    ? -1 : Convert.ToInt32(resultObj);

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error UpdateWorkshop", ex);
            }
        }
        public static async Task<int> DeleteWorkshop(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Delete_Workshop", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@WorkShopId", id);

            try
            {
                await connection.OpenAsync();

                object? resultObj = await command.ExecuteScalarAsync();

                int result = resultObj == null || resultObj == DBNull.Value
                    ? -1 : Convert.ToInt32(resultObj);

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error DeleteWorkshop", ex);
            }
        }
    }
}