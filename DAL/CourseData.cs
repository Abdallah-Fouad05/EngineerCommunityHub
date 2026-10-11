using System.Data;
using DAL.DTOs;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public static class CourseData
    {
        public static async Task<List<CourseDTO>> GetAllCourses()
        {
            List<CourseDTO> list = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);
            using SqlCommand comd = new SqlCommand("SP_Get_All_Courses", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new CourseDTO
                    {
                        CourseId = reader.GetInt32("CourseId"),
                        CourseName = reader.GetString("CourseName"),
                        Title = reader.IsDBNull("Title") ? null : reader.GetString("Title"),
                        Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                        CreatedAt = reader.IsDBNull("CreatedAt") ? null : reader.GetDateTime("CreatedAt"),
                        Image = reader.IsDBNull("Image") ? null : reader.GetString("Image"),
                        Price = reader.IsDBNull("Price") ? null : reader.GetDecimal("Price"),
                        IsPaid = reader.GetBoolean("isPaid"),
                        StartDate = reader.IsDBNull("StartDate") ? null : reader.GetDateTime("StartDate"),
                        LastDate = reader.IsDBNull("LastDate") ? null : reader.GetDateTime("LastDate"),
                        Requirements = reader.IsDBNull("Requirements") ? null : reader.GetString("Requirements"),
                        Skills = reader.IsDBNull("Skills") ? null : reader.GetString("Skills"),
                        Link = reader.IsDBNull("Link") ? null : reader.GetString("Link"),
                        Category = new category(reader.GetInt32("CategoryId"), reader.GetString(reader.GetOrdinal("CategoryName"))),
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
                    });
                }

                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllCourses", ex);
            }
        }
        public static async Task<List<CourseDTO>> GetCoursesByCategoryId(int CategoryId)
        {
            List<CourseDTO> list = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);
            using SqlCommand comd = new SqlCommand("SP_Get_Courses_ByCategoryId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            comd.Parameters.AddWithValue("@CategoryId", CategoryId);
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new CourseDTO
                    {
                        CourseId = reader.GetInt32("CourseId"),
                        CourseName = reader.GetString("CourseName"),
                        Title = reader.IsDBNull("Title") ? null : reader.GetString("Title"),
                        Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                        CreatedAt = reader.IsDBNull("CreatedAt") ? null : reader.GetDateTime("CreatedAt"),
                        Image = reader.IsDBNull("Image") ? null : reader.GetString("Image"),
                        Price = reader.IsDBNull("Price") ? null : reader.GetDecimal("Price"),
                        IsPaid = reader.GetBoolean("isPaid"),
                        StartDate = reader.IsDBNull("StartDate") ? null : reader.GetDateTime("StartDate"),
                        LastDate = reader.IsDBNull("LastDate") ? null : reader.GetDateTime("LastDate"),
                        Requirements = reader.IsDBNull("Requirements") ? null : reader.GetString("Requirements"),
                        Skills = reader.IsDBNull("Skills") ? null : reader.GetString("Skills"),
                        Link = reader.IsDBNull("Link") ? null : reader.GetString("Link"),
                        Category = new category(reader.GetInt32("CategoryId"), reader.GetString(reader.GetOrdinal("CategoryName"))),
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
                    });
                }

                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllCourses", ex);
            }
        }
        public static async Task<CourseDTO?> GetCourseById(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Get_Course_ById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@CourseId", id);

            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new CourseDTO
                    {
                        CourseId = reader.GetInt32("CourseId"),
                        CourseName = reader.GetString("CourseName"),
                        Title = reader.IsDBNull("Title") ? null : reader.GetString("Title"),
                        Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                        CreatedAt = reader.IsDBNull("CreatedAt") ? null : reader.GetDateTime("CreatedAt"),
                        Image = reader.IsDBNull("Image") ? null : reader.GetString("Image"),
                        Price = reader.IsDBNull("Price") ? null : reader.GetDecimal("Price"),
                        IsPaid = reader.GetBoolean("isPaid"),
                        StartDate = reader.IsDBNull("StartDate") ? null : reader.GetDateTime("StartDate"),
                        LastDate = reader.IsDBNull("LastDate") ? null : reader.GetDateTime("LastDate"),
                        Requirements = reader.IsDBNull("Requirements") ? null : reader.GetString("Requirements"),
                        Skills = reader.IsDBNull("Skills") ? null : reader.GetString("Skills"),
                        Link = reader.IsDBNull("Link") ? null : reader.GetString("Link"),
                        Category = new category(reader.GetInt32("CategoryId"), reader.GetString(reader.GetOrdinal("CategoryName"))),
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
                    };
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetCourseById", ex);
            }
        }
        public static async Task<(bool, int)> CreateCourse(Course_Created c)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Create_Course", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@CourseName", c.CourseName);
            comd.Parameters.AddWithValue("@Title", (object?)c.Title ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Description", (object?)c.Description ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Image", (object?)c.Image ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Price", (object?)c.Price ?? DBNull.Value);
            comd.Parameters.AddWithValue("@IsPaid", c.IsPaid);
            comd.Parameters.AddWithValue("@StartDate", (object?)c.StartDate ?? DBNull.Value);
            comd.Parameters.AddWithValue("@LastDate", (object?)c.LastDate ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Requirements", (object?)c.Requirements ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Skills", (object?)c.Skills ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Link", (object?)c.Link ?? DBNull.Value);
            comd.Parameters.AddWithValue("@CategoryId", c.CategoryId);
            comd.Parameters.AddWithValue("@UserId", c.CreatedBy);

            SqlParameter output = new SqlParameter("@NewCourseId", SqlDbType.Int)
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
                throw new Exception("DB Error CreateCourse", ex);
            }
        }
        public static async Task<int> UpdateCourse(Course_Updated c)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Update_Course", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@CourseId", c.CourseId);
            comd.Parameters.AddWithValue("@CourseName", c.CourseName);
            comd.Parameters.AddWithValue("@Title", (object?)c.Title ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Description", (object?)c.Description ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Image", (object?)c.Image ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Price", (object?)c.Price ?? DBNull.Value);
            comd.Parameters.AddWithValue("@IsPaid", c.IsPaid);
            comd.Parameters.AddWithValue("@StartDate", (object?)c.StartDate ?? DBNull.Value);
            comd.Parameters.AddWithValue("@LastDate", (object?)c.LastDate ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Requirements", (object?)c.Requirements ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Skills", (object?)c.Skills ?? DBNull.Value);
            comd.Parameters.AddWithValue("@Link", (object?)c.Link ?? DBNull.Value);
            comd.Parameters.AddWithValue("@CategoryId", c.CategoryId);

            try
            {
                await connection.OpenAsync();

                object? result = await comd.ExecuteScalarAsync();

                return result == null ? -1 : Convert.ToInt32(result);
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error UpdateCourse", ex);
            }
        }
        public static async Task<int> DeleteCourse(int id)
        {
            await using SqlConnection connection = new SqlConnection(Setting.Connection);

            await using SqlCommand comd = new SqlCommand("SP_Delete_Course", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            comd.Parameters.AddWithValue("@CourseId", id);

            try
            {
                await connection.OpenAsync();

                object? result = await comd.ExecuteScalarAsync();

                return result == null ? -1 : Convert.ToInt32(result);
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error DeleteCourse", ex);
            }
        }
        public static async Task<List<CourseDTO>> GetAllCoursesByUserId(int UserId)
        {
            List<CourseDTO> list = new();

            using SqlConnection connection = new SqlConnection(Setting.Connection);
            using SqlCommand comd = new SqlCommand("SP_Get_Courses_ByUserId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            comd.Parameters.AddWithValue("@UserId",UserId);
            try
            {
                await connection.OpenAsync();
                await using SqlDataReader reader = await comd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new CourseDTO
                    {
                        CourseId = reader.GetInt32("CourseId"),
                        CourseName = reader.GetString("CourseName"),
                        Title = reader.IsDBNull("Title") ? null : reader.GetString("Title"),
                        Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                        CreatedAt = reader.IsDBNull("CreatedAt") ? null : reader.GetDateTime("CreatedAt"),
                        Image = reader.IsDBNull("Image") ? null : reader.GetString("Image"),
                        Price = reader.IsDBNull("Price") ? null : reader.GetDecimal("Price"),
                        IsPaid = reader.GetBoolean("isPaid"),
                        StartDate = reader.IsDBNull("StartDate") ? null : reader.GetDateTime("StartDate"),
                        LastDate = reader.IsDBNull("LastDate") ? null : reader.GetDateTime("LastDate"),
                        Requirements = reader.IsDBNull("Requirements") ? null : reader.GetString("Requirements"),
                        Skills = reader.IsDBNull("Skills") ? null : reader.GetString("Skills"),
                        Link = reader.IsDBNull("Link") ? null : reader.GetString("Link"),
                        Category = new category(reader.GetInt32("CategoryId"), reader.GetString(reader.GetOrdinal("CategoryName"))),
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
                    });
                }

                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("DB Error GetAllCourses", ex);
            }
        }
    }
}