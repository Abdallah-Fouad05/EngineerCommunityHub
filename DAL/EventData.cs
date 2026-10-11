using System.Data;
using Microsoft.Data.SqlClient;
using DAL.DTOs;

namespace DAL
{
    public static class EventData
    {
        public static async Task<List<EventDTO>> GetAllEvents()
        {
            List<EventDTO> events = new();

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Get_All_Events", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    events.Add(new EventDTO
                    {
                        EventId = reader.GetInt32(reader.GetOrdinal("EventId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Image = reader.IsDBNull(reader.GetOrdinal("Image"))
                            ? null : reader.GetString(reader.GetOrdinal("Image")),
                        StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                        EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                        Location = reader.GetString(reader.GetOrdinal("Location")),
                        CreatedBy = new user { 
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
                        Status = new StatusDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("StatusId")),
                            reader.GetString(reader.GetOrdinal("StatusName"))
                        ),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                    });
                }

                return events;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllEvents : ", ex);
            }
        }
        public static async Task<List<EventDTO>> GetAllEventsByUserId(int UserId)
        {
            List<EventDTO> events = new();

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Get_Events_ByUserId", connection)
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
                    events.Add(new EventDTO
                    {
                        EventId = reader.GetInt32(reader.GetOrdinal("EventId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Image = reader.IsDBNull(reader.GetOrdinal("Image"))
                            ? null : reader.GetString(reader.GetOrdinal("Image")),
                        StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                        EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
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
                                 null
                             ),
                            Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        },
                        Status = new StatusDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("StatusId")),
                            reader.GetString(reader.GetOrdinal("StatusName"))
                        ),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                    });
                }

                return events;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllEvents : ", ex);
            }
        }
        public static async Task<List<EventDTO>> GetAllEventsByStatusId(int statusId)
        {
            List<EventDTO> events = new();

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Get_All_Events_ByStatusId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            comd.Parameters.AddWithValue("@StatusId", statusId);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    events.Add(new EventDTO
                    {
                        EventId = reader.GetInt32(reader.GetOrdinal("EventId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Image = reader.IsDBNull(reader.GetOrdinal("Image"))
                            ? null : reader.GetString(reader.GetOrdinal("Image")),
                        StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                        EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
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
                                 null
                             ),
                            Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        },
                        Status = new StatusDTO
                        (
                           reader.GetInt32(reader.GetOrdinal("StatusId")),
                           reader.GetString(reader.GetOrdinal("StatusName"))
                        ),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                    });
                }

                return events;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllEvents : ", ex);
            }
        }
        public static async Task<EventDTO?> GetEventById(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Get_Event_ById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@EventId", id);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new EventDTO
                    {
                        EventId = reader.GetInt32(reader.GetOrdinal("EventId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null : reader.GetString(reader.GetOrdinal("Description")),
                        Image = reader.IsDBNull(reader.GetOrdinal("Image"))
                            ? null : reader.GetString(reader.GetOrdinal("Image")),
                        StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                        EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
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
                                 null
                             ),
                            Image = reader.IsDBNull(reader.GetOrdinal("UserImage"))
                            ? null : reader.GetString(reader.GetOrdinal("UserImage"))
                        },
                        Status = new StatusDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("StatusId")),
                            reader.GetString(reader.GetOrdinal("StatusName"))
                        ),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                    };
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetEventById : ", ex);
            }
        }
        public static async Task<(bool, int)> CreateEvent(Event_Created ev)
        {
            var defaultStartDate = DateTime.Now;
            var defaultEndDate = DateTime.Now.AddDays(7); // after week 

           
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand comd = new SqlCommand("SP_Create_Event", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@Title", ev.Title);
            comd.Parameters.AddWithValue("@Description",
                string.IsNullOrEmpty(ev.Description) ? DBNull.Value : ev.Description);
            comd.Parameters.AddWithValue("@Image",
                string.IsNullOrEmpty(ev.Image) ? DBNull.Value : ev.Image);
            comd.Parameters.AddWithValue("@StartDate",
               (ev.StartDate == DateTime.MinValue) ? defaultStartDate : ev.StartDate);
            comd.Parameters.AddWithValue("@EndDate",
                (!ev.EndDate.HasValue || ev.EndDate.Value == DateTime.MinValue)
                ? defaultEndDate
                : ev.EndDate.Value);
            comd.Parameters.AddWithValue("@Location", ev.Location);
            comd.Parameters.AddWithValue("@UserId", ev.CreatedBy);

            SqlParameter outputParam = new SqlParameter("@NewEventId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            comd.Parameters.Add(outputParam);

            try
            {
                await connection.OpenAsync();

                await comd.ExecuteNonQueryAsync();

                int newId = outputParam.Value == DBNull.Value ? -1 : Convert.ToInt32(outputParam.Value);

                return (newId > 0, newId);
            }
            catch (Exception ex)
            {
                throw new Exception("DB Error CreateEvent.", ex);
            }
        }
        public static async Task<int> UpdateEvent(Event_Updated ev)
        {
            var defaultStartDate = DateTime.Now;
            var defaultEndDate = DateTime.Now.AddDays(7); 

            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Update_Event", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@EventId", ev.EventId);
            command.Parameters.AddWithValue("@Title", ev.Title);
            command.Parameters.AddWithValue("@Description",
                string.IsNullOrEmpty(ev.Description) ? DBNull.Value : ev.Description);
            command.Parameters.AddWithValue("@Image",
                string.IsNullOrEmpty(ev.Image) ? DBNull.Value : ev.Image);
            command.Parameters.AddWithValue("@StartDate",
               (ev.StartDate == DateTime.MinValue) ? defaultStartDate : ev.StartDate);
            command.Parameters.AddWithValue("@EndDate",
                (!ev.EndDate.HasValue || ev.EndDate.Value == DateTime.MinValue)
                ? defaultEndDate
                : ev.EndDate.Value);
            command.Parameters.AddWithValue("@Location", ev.Location);

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
                throw new Exception("DB Error UpdateEvent", ex);
            }
        }
        public static async Task<int> DeleteEvent(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);
            await using SqlCommand command = new SqlCommand("SP_Delete_Event", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@EventId", id);

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
                throw new Exception("DB Error DeleteEvent", ex);
            }
        }
        public static async Task<List<StatusDTO?>> GetEventStatus()
        {
            List<StatusDTO> statuses = new();
            using SqlConnection connection = new SqlConnection(Setting.Connection);
            using SqlCommand comd = new SqlCommand("SELECT StatusId, StatusName FROM EventStatus;", connection);
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
    }
}